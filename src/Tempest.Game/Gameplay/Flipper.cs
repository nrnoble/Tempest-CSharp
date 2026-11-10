using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

// Climbing: rises up the lane. Pausing: lane/rim pause before an adjacent pivot.
// Flipping: visible pivot to an adjacent lane.
public enum FlipperState
{
    Climbing,
    Pausing,
    Flipping
}

internal enum ProgrammedFlipperRouteStep
{
    Disabled,
    LeadIn,
    Start,
    Turning,
    CompletionBoundary,
    RimHandoff,
    Level8Along,
    Level8Start,
    Level8Turning,
    Level8Straight,
    Level8StraightTurning,
    Level6DeepAlong,
    Level6DeepTurning,
    Level6Straight
}

/// <summary>
/// Flipper state machine (Climbing -> Pausing -> Flipping -> Pausing ...). Its arcade shot delay
/// is advanced by TempestGame's separate firing phase, independently of movement state.
/// Ordinary Level 2-test canflip retains its below-rim direction; rim pursuit latches the first
/// selected direction. Level 7's arcade-observed 2-3-2-2-2 route is a separate,
/// configuration-opt-in path and leaves that accepted rim policy intact.
/// The entity never touches game state: lethal contact is reported through
/// <see cref="CompletedIntoPlayer"/> and consumed by TempestGame after player-shot collision.
/// </summary>
public sealed class Flipper : Enemy
{
    private static readonly int[] Level7ObservedFlipCounts = [2, 3, 2, 2, 2];

    private int programmedPhaseIndex;
    private int programmedFlipsRemaining;
    private ProgrammedFlipperRouteStep programmedRouteStep;
    private bool useLevel5SpikeSensitiveProgram;
    private bool useLevel8ConstantFlipProgram;
    private bool useLevel8StraightCargoProgram;
    private int level6StraightAdvances;

    public Flipper(int edge = 0, float height = 0)
    {
        EdgeIndex = edge;
        SourceEdge = edge;
        DestinationEdge = edge;
        Height = height;
        // The ROM initializes shot_delay to zero, so the first eligible fire phase is ready.
        ShotDelayTicks = 0;
        State = FlipperState.Climbing;
    }

    public int SourceEdge { get; private set; }
    public int DestinationEdge { get; private set; }
    public FlipperState State { get; private set; }
    public int PauseTicks { get; private set; }
    public int Direction { get; private set; }
    public int? RimPursuitDirection { get; private set; }
    public float TurnAngle { get; private set; }
    public float TotalTurnAngle { get; private set; }
    public DirectedLaneTransition? ActiveTransition { get; private set; }
    // Single pending-player-collision signal. Set on a direct rim arrival in the player's lane
    // (Incitti CheckPlayerColl: unconditional for a climbing Flipper) and on a pivot completing
    // into the player's lane at full depth. A player who leaves before either moment is safe.
    public bool CompletedIntoPlayer { get; private set; }
    public int ShotDelayTicks { get; private set; }
    // Tanker identity: slot of the original enemy this unit stands for (-1 if none) and cargo
    // unit id (-1 if not Tanker cargo). The two IDs belong to separate ledger namespaces.
    public int OriginalSlotId { get; private set; } = -1;
    public int CargoUnitId { get; private set; } = -1;
    public bool IsTankerCargo => CargoUnitId >= 0;
    public TankerCargoMovementProgram CargoMovementProgram { get; private set; }
    internal int ProgrammedFlipsRemaining => programmedFlipsRemaining;
    internal ProgrammedFlipperRouteStep ProgrammedRouteStep => programmedRouteStep;
    internal bool UsesLevel8ConstantFlipProgram => useLevel8ConstantFlipProgram;

    public void Update(Level level, int playerEdge, Func<int, int, int>? randomInclusive = null)
    {
        FlipperConfiguration tuning = level.Configuration.Flipper;
        if (programmedRouteStep == ProgrammedFlipperRouteStep.Disabled &&
            !IsTankerCargo &&
            level.Configuration.BelowRimFlipperProgram ==
                BelowRimFlipperProgram.Level7ArcadeObserved)
            InitializeProgrammedRoute(level.Configuration.ProgrammedFlipperInitialDirection);

        if (programmedRouteStep is not
            (ProgrammedFlipperRouteStep.Disabled or ProgrammedFlipperRouteStep.RimHandoff))
        {
            if (useLevel8ConstantFlipProgram || useLevel8StraightCargoProgram)
                UpdateLevel8ConstantFlipRoute(level, playerEdge, tuning);
            else
                UpdateProgrammedRoute(level, playerEdge, tuning);
            return;
        }

        bool canFlip = tuning.CanFlip && (!IsTankerCargo || useLevel5SpikeSensitiveProgram);
        bool spikeGatedProgram = UsesLevel5SpikeSensitiveProgram(level);
        switch (State)
        {
            case FlipperState.Climbing:
                AdvanceClimb(level, playerEdge, tuning, canFlip, spikeGatedProgram,
                    randomInclusive);
                break;

            case FlipperState.Pausing:
                if (spikeGatedProgram && Height < Level.Depth &&
                    !CanStartSpikeGatedLaneChange(level))
                {
                    // $53 keeps moving after a flip; a false current-lane predicate is not a pause.
                    State = FlipperState.Climbing;
                    PauseTicks = 0;
                    AdvanceClimb(level, playerEdge, tuning, canFlip, spikeGatedProgram,
                        randomInclusive);
                    break;
                }

                bool pausedBelowRim = Height < Level.Depth;
                if ((IsTankerCargo || canFlip) && Height < Level.Depth)
                    Height = MathF.Min(Level.Depth, Height + 1);
                PauseTicks--;
                if (PauseTicks <= 0)
                {
                    BeginPivot(level, playerEdge, randomInclusive);
                }
                else if (canFlip && pausedBelowRim && Height >= Level.Depth && EdgeIndex == playerEdge)
                    // A lane pause can arrive at the rim too; active pivots remain safe until complete.
                    CompletedIntoPlayer = true;
                break;

            case FlipperState.Flipping:
                AdvanceTurnAngle(level, tuning);
                if (Height < Level.Depth)
                {
                    Height += tuning.ClimbSpeed;
                    if (Height >= Level.Depth)
                        Height = Level.Depth;
                }
                if (TurnAngle >= TotalTurnAngle)
                    CompletePivot(playerEdge, canFlip && Height < Level.Depth
                        ? tuning.LanePauseTicks
                        : tuning.RimPauseTicks);
                break;
        }
    }

    // Existing cargo starts in its release pivot. Canonical Level 6 selects a movement program
    // that starts at the release lane instead; all cargo resumes player pursuit at the rim.
    public static Flipper CreateTankerCargo(
        Level level, TankerCargoSpawn cargo, int cargoUnitId,
        bool useLevel7ProgrammedRoute = false,
        bool useLevel5SpikeSensitiveProgram = false,
        bool useLevel8ConstantFlipProgram = false,
        bool useLevel8StraightCargoProgram = false,
        TankerCargoMovementProgram tankerCargoMovementProgram =
            TankerCargoMovementProgram.Existing)
    {
        Flipper flipper = new(cargo.SourceLane, cargo.Height)
        {
            SourceEdge = cargo.SourceLane,
            DestinationEdge = cargo.DestinationLane,
            Direction = cargo.Direction,
            State = FlipperState.Flipping,
            CargoUnitId = cargoUnitId,
            useLevel5SpikeSensitiveProgram = useLevel5SpikeSensitiveProgram,
            useLevel8ConstantFlipProgram = useLevel8ConstantFlipProgram,
            useLevel8StraightCargoProgram = useLevel8StraightCargoProgram,
            CargoMovementProgram = tankerCargoMovementProgram
        };
        DirectedLaneTransition transition = level.CalculateDirectedLaneTransition(
            cargo.SourceLane, cargo.DestinationLane, cargo.Direction);
        flipper.ActiveTransition = transition;
        flipper.TotalTurnAngle = transition.SweepMagnitudeDegrees;
        if (tankerCargoMovementProgram is TankerCargoMovementProgram.Level6Deep0B or
            TankerCargoMovementProgram.Level6Straight07)
            flipper.InitializeLevel6CargoRoute(tankerCargoMovementProgram);
        else if (tankerCargoMovementProgram != TankerCargoMovementProgram.Existing)
            throw new ArgumentOutOfRangeException(nameof(tankerCargoMovementProgram));
        else if (useLevel8ConstantFlipProgram || useLevel8StraightCargoProgram)
            flipper.InitializeLevel8CargoRoute(cargo.Direction, useLevel8ConstantFlipProgram);
        else if (useLevel7ProgrammedRoute)
            flipper.InitializeProgrammedCargoRoute(cargo.Direction);
        return flipper;
    }

    // Ordinary climbing Flipper used when cargo re-enters through a fresh Egg after a life loss.
    public static Flipper CreateFreshTankerCargo(
        int edge, int cargoUnitId, int? programmedDirection = null,
        bool useLevel5SpikeSensitiveProgram = false, int? initialDirection = null)
    {
        Flipper flipper = new(edge, 0)
        {
            CargoUnitId = cargoUnitId,
            useLevel5SpikeSensitiveProgram = useLevel5SpikeSensitiveProgram
        };
        if (programmedDirection is int direction)
            flipper.InitializeProgrammedRoute(direction);
        else if (initialDirection is int freshDirection)
        {
            if (freshDirection is not (-1 or 1))
                throw new ArgumentOutOfRangeException(nameof(initialDirection));

            flipper.Direction = freshDirection;
        }
        return flipper;
    }

    internal void SetOriginalSlotId(int originalSlotId) => OriginalSlotId = originalSlotId;

    internal bool AdvanceShotDelay()
    {
        if (ShotDelayTicks == 0)
            return true;

        ShotDelayTicks--;
        return false;
    }

    internal void ReloadShotDelay(int ticks)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticks);
        ShotDelayTicks = ticks;
    }

    public bool IsHitBy(Shot shot)
    {
        // While pivoting the Flipper is shootable from both its source and destination lanes.
        // The 0..16 height window means the shot must be just below the Flipper.
        bool matchingEdge = shot.EdgeIndex == EdgeIndex;
        if (State == FlipperState.Flipping)
            matchingEdge = shot.EdgeIndex == SourceEdge || shot.EdgeIndex == DestinationEdge;

        if (!matchingEdge)
            return false;

        float separation = Height - shot.Height;
        return !(separation <= 0 || separation >= 16);
    }

    public override bool TryZap(int delay) => AcceptZap(delay);

    public void ClearCompletedCollision() => CompletedIntoPlayer = false;

    private void BeginPause(int pauseTicks)
    {
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        ActiveTransition = null;
        TotalTurnAngle = 0;
        State = FlipperState.Pausing;
        PauseTicks = pauseTicks;
        TurnAngle = 0;
    }

    private void AdvanceClimb(
        Level level, int playerEdge, FlipperConfiguration tuning, bool canFlip,
        bool spikeGatedProgram, Func<int, int, int>? randomInclusive)
    {
        bool startedBelowRim = Height < Level.Depth;
        Height += tuning.ClimbSpeed;
        if (Height >= Level.Depth)
        {
            Height = Level.Depth;
            if (EdgeIndex == playerEdge)
                CompletedIntoPlayer = true;
            else
                BeginPause(tuning.RimPauseTicks);
        }

        if (canFlip && startedBelowRim &&
            (!spikeGatedProgram || CanStartSpikeGatedLaneChange(level)))
        {
            // ROM $53 moves first, then tests the live Spike at the new along-position.
            // The existing C# route enters its pivot phase without assigning a lane pause.
            BeginPause(0);
            if (!IsTankerCargo)
                Direction = level.FindReferenceShortestDirection(
                    EdgeIndex, playerEdge, randomInclusive);
        }
    }

    private bool UsesLevel5SpikeSensitiveProgram(Level level) =>
        useLevel5SpikeSensitiveProgram ||
        (!IsTankerCargo &&
         level.Configuration.BelowRimFlipperProgram ==
            BelowRimFlipperProgram.Level5SpikeSensitive);

    private bool CanStartSpikeGatedLaneChange(Level level)
    {
        Spike? spike = level.Spikes.FirstOrDefault(candidate => candidate.EdgeIndex == EdgeIndex);
        // C# height rises toward the rim; absent/depleted spikes impose no lane-change gate.
        return spike is null || spike.IsDepleted || Height >= spike.Height;
    }

    private void InitializeProgrammedRoute(int initialDirection)
    {
        if (initialDirection is not (-1 or 1))
            throw new ArgumentOutOfRangeException(nameof(initialDirection));

        programmedPhaseIndex = 0;
        programmedFlipsRemaining = Level7ObservedFlipCounts[programmedPhaseIndex];
        Direction = initialDirection;
        programmedRouteStep = ProgrammedFlipperRouteStep.LeadIn;
    }

    private void InitializeProgrammedCargoRoute(int initialDirection)
    {
        InitializeProgrammedRoute(initialDirection);
        // The release pivot is the cargo's first completed outward lane change.
        programmedRouteStep = ProgrammedFlipperRouteStep.Turning;
    }

    private void InitializeLevel8CargoRoute(int initialDirection, bool constantFlipProgram)
    {
        if (initialDirection is not (-1 or 1))
            throw new ArgumentOutOfRangeException(nameof(initialDirection));
        Direction = initialDirection;
        programmedRouteStep = constantFlipProgram
            ? ProgrammedFlipperRouteStep.Level8Turning
            : ProgrammedFlipperRouteStep.Level8StraightTurning;
    }

    private void UpdateLevel8ConstantFlipRoute(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        switch (programmedRouteStep)
        {
            case ProgrammedFlipperRouteStep.Level8Straight:
                AdvanceLevel8Straight(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.Level8StraightTurning:
                ContinueLevel8CargoPivot(level, playerEdge, tuning,
                    ProgrammedFlipperRouteStep.Level8Straight);
                break;

            case ProgrammedFlipperRouteStep.Level8Along:
                AdvanceLevel8Straight(level, playerEdge, tuning);
                if (Height < Level.Depth)
                    programmedRouteStep = ProgrammedFlipperRouteStep.Level8Start;
                break;

            case ProgrammedFlipperRouteStep.Level8Start:
                if (Height >= Level.Depth)
                    HandOffToRim(tuning);
                else
                {
                    BeginProgrammedPivot(level);
                    programmedRouteStep = ProgrammedFlipperRouteStep.Level8Turning;
                }
                break;

            case ProgrammedFlipperRouteStep.Level8Turning:
                ContinueLevel8CargoPivot(level, playerEdge, tuning,
                    ProgrammedFlipperRouteStep.Level8Along);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported Level 8 Flipper route step {programmedRouteStep}.");
        }
    }

    private void AdvanceLevel8Straight(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        bool startedBelowRim = Height < Level.Depth;
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (startedBelowRim && Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
            HandOffToRim(tuning);
    }

    private void ContinueLevel8CargoPivot(
        Level level, int playerEdge, FlipperConfiguration tuning,
        ProgrammedFlipperRouteStep nextStep)
    {
        DirectedLaneTransition transition = ActiveTransition ??
            throw new InvalidOperationException("A Level 8 Flipper turn requires an active transition.");
        AdvanceTurnAngle(level, tuning);
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (TurnAngle < TotalTurnAngle)
            return;

        EdgeIndex = transition.DestinationLane;
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        ActiveTransition = null;
        TurnAngle = 0;
        TotalTurnAngle = 0;
        PauseTicks = 0;
        State = FlipperState.Climbing;
        if (Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
            HandOffToRim(tuning);
        else
            programmedRouteStep = nextStep;
    }

    private void UpdateProgrammedRoute(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        switch (programmedRouteStep)
        {
            case ProgrammedFlipperRouteStep.LeadIn:
                AdvanceProgrammedAlong(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.Start:
                if (Height >= Level.Depth)
                    HandOffToRim(tuning);
                else
                    BeginProgrammedPivot(level);
                break;

            case ProgrammedFlipperRouteStep.Turning:
                ContinueProgrammedPivot(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.CompletionBoundary:
                AdvanceProgrammedRouteBoundary(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.Level6DeepAlong:
                AdvanceLevel6DeepAlong(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.Level6DeepTurning:
                ContinueLevel6DeepPivot(level, playerEdge, tuning);
                break;

            case ProgrammedFlipperRouteStep.Level6Straight:
                AdvanceLevel6Straight(level, playerEdge, tuning);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported programmed Flipper route step {programmedRouteStep}.");
        }
    }

    private void InitializeLevel6CargoRoute(TankerCargoMovementProgram movementProgram)
    {
        // ROM cargo already has its release lane and direction; $0b/$07 begins there without
        // the shared C# cargo path's extra below-rim release pivot.
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        ActiveTransition = null;
        TurnAngle = 0;
        TotalTurnAngle = 0;
        PauseTicks = 0;
        State = FlipperState.Climbing;
        level6StraightAdvances = 0;
        programmedRouteStep = movementProgram switch
        {
            TankerCargoMovementProgram.Level6Deep0B =>
                ProgrammedFlipperRouteStep.Level6DeepAlong,
            TankerCargoMovementProgram.Level6Straight07 =>
                ProgrammedFlipperRouteStep.Level6Straight,
            _ => throw new ArgumentOutOfRangeException(nameof(movementProgram))
        };
    }

    private void AdvanceLevel6DeepAlong(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        bool startedBelowRim = Height < Level.Depth;
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (startedBelowRim && Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
        {
            HandOffToRim(tuning);
            return;
        }

        level6StraightAdvances++;
        if (level6StraightAdvances < 8)
            return;

        BeginProgrammedPivot(level);
        programmedRouteStep = ProgrammedFlipperRouteStep.Level6DeepTurning;
    }

    private void ContinueLevel6DeepPivot(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        DirectedLaneTransition transition = ActiveTransition ??
            throw new InvalidOperationException("A Level 6 $0b Flipper turn requires an active transition.");
        // ROM $0b holds along-depth during the turn and returns directly to the eight-advance loop.
        AdvanceTurnAngle(level, tuning);
        if (TurnAngle < TotalTurnAngle)
            return;

        EdgeIndex = transition.DestinationLane;
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        ActiveTransition = null;
        TurnAngle = 0;
        TotalTurnAngle = 0;
        PauseTicks = 0;
        State = FlipperState.Climbing;
        if (Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
        {
            HandOffToRim(tuning);
            return;
        }

        level6StraightAdvances = 0;
        programmedRouteStep = ProgrammedFlipperRouteStep.Level6DeepAlong;
    }

    private void AdvanceLevel6Straight(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        bool startedBelowRim = Height < Level.Depth;
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (startedBelowRim && Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
            HandOffToRim(tuning);
    }

    private void AdvanceProgrammedAlong(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        bool startedBelowRim = Height < Level.Depth;
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (startedBelowRim && Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;
        if (Height >= Level.Depth)
            HandOffToRim(tuning);
        else
            programmedRouteStep = ProgrammedFlipperRouteStep.Start;
    }

    private void BeginProgrammedPivot(Level level)
    {
        SourceEdge = EdgeIndex;
        int direction = Direction;
        DestinationEdge = GetDirectedNeighborLane(level, EdgeIndex, ref direction);
        Direction = direction;
        ActiveTransition = level.CalculateDirectedLaneTransition(
            SourceEdge, DestinationEdge, Direction);
        TotalTurnAngle = ActiveTransition.Value.SweepMagnitudeDegrees;
        TurnAngle = 0;
        State = FlipperState.Flipping;
        programmedRouteStep = ProgrammedFlipperRouteStep.Turning;
    }

    private void ContinueProgrammedPivot(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        AdvanceTurnAngle(level, tuning);
        Height = MathF.Min(Level.Depth, Height + tuning.ClimbSpeed);
        if (TurnAngle < TotalTurnAngle)
            return;

        EdgeIndex = DestinationEdge;
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        ActiveTransition = null;
        TurnAngle = 0;
        TotalTurnAngle = 0;
        PauseTicks = 0;
        State = FlipperState.Climbing;
        if (Height >= Level.Depth && EdgeIndex == playerEdge)
            CompletedIntoPlayer = true;

        if (Height >= Level.Depth)
            HandOffToRim(tuning);
        else
            programmedRouteStep = ProgrammedFlipperRouteStep.CompletionBoundary;
    }

    private void AdvanceTurnAngle(Level level, FlipperConfiguration tuning)
    {
        if (Height < Level.Depth ||
            programmedRouteStep is not
                (ProgrammedFlipperRouteStep.Disabled or ProgrammedFlipperRouteStep.RimHandoff))
        {
            TurnAngle += tuning.TurnSpeedDegrees;
            return;
        }

        // Clamp the final step so even huge finite diagnostic rates cannot overflow pivot geometry.
        double rimTurnSpeed = level.RimTurnSpeedDegrees;
        double remainingTurn = TotalTurnAngle - TurnAngle;
        TurnAngle = rimTurnSpeed >= remainingTurn
            ? TotalTurnAngle
            : TurnAngle + (float)rimTurnSpeed;
    }

    private void AdvanceProgrammedRouteBoundary(
        Level level, int playerEdge, FlipperConfiguration tuning)
    {
        programmedFlipsRemaining--;
        if (programmedFlipsRemaining > 0)
        {
            BeginProgrammedPivot(level);
            return;
        }

        Direction = -Direction;
        programmedPhaseIndex = (programmedPhaseIndex + 1) % Level7ObservedFlipCounts.Length;
        programmedFlipsRemaining = Level7ObservedFlipCounts[programmedPhaseIndex];
        if (programmedPhaseIndex != 0)
        {
            BeginProgrammedPivot(level);
            return;
        }

        // Neal's arcade observation is a repeating 2-3-2-2-2 route; keep its route state
        // per Flipper and retain the established lead-in only at the five-block boundary.
        programmedRouteStep = ProgrammedFlipperRouteStep.LeadIn;
        AdvanceProgrammedAlong(level, playerEdge, tuning);
    }

    private void HandOffToRim(FlipperConfiguration tuning)
    {
        programmedRouteStep = ProgrammedFlipperRouteStep.RimHandoff;
        BeginPause(tuning.RimPauseTicks);
    }

    // +1 pivots toward PreviousEdge. Below-rim canflip/cargo retain their route direction;
    // the first rim pivot chooses a separate pursuit direction that remains fixed for this life.
    private void BeginPivot(Level level, int playerEdge, Func<int, int, int>? randomInclusive)
    {
        bool canFlip = level.Configuration.FlipperCanFlip && !IsTankerCargo;
        if (Height >= Level.Depth)
        {
            RimPursuitDirection ??= canFlip
                ? level.FindReferenceShortestDirection(EdgeIndex, playerEdge, randomInclusive)
                : level.FindShortestDirection(EdgeIndex, playerEdge);
            Direction = RimPursuitDirection.Value;
        }
        else if (!IsTankerCargo && !canFlip)
            Direction = level.FindShortestDirection(EdgeIndex, playerEdge);

        if (Direction is not (-1 or 1))
            throw new InvalidOperationException(
                $"A Flipper must have direction -1 or 1 before beginning a pivot; got {Direction}.");

        SourceEdge = EdgeIndex;
        int direction = Direction;
        DestinationEdge = GetDirectedNeighborLane(level, EdgeIndex, ref direction);
        Direction = direction;
        DirectedLaneTransition transition = level.CalculateDirectedLaneTransition(
            SourceEdge, DestinationEdge, Direction);
        ActiveTransition = transition;
        TotalTurnAngle = transition.SweepMagnitudeDegrees;
        TurnAngle = 0;
        State = FlipperState.Flipping;
    }

    private static int GetDirectedNeighborLane(Level level, int sourceLane, ref int direction)
    {
        int laneIndexDelta = direction > 0 ? -1 : 1;
        if (level.TryGetAdjacentLane(sourceLane, laneIndexDelta, out int destination))
            return destination;

        direction = -direction;
        laneIndexDelta = -laneIndexDelta;
        if (!level.TryGetAdjacentLane(sourceLane, laneIndexDelta, out destination))
            throw new InvalidOperationException("A Flipper has no adjacent playable lane.");
        return destination;
    }

    private void CompletePivot(int playerEdge, int pauseTicks)
    {
        EdgeIndex = DestinationEdge;
        SourceEdge = EdgeIndex;
        TurnAngle = 0;
        TotalTurnAngle = 0;

        // Lethal only if the player is in the destination lane at completion.
        if (EdgeIndex == playerEdge && Height >= Level.Depth)
            CompletedIntoPlayer = true;

        BeginPause(pauseTicks);
    }

    public override void Update() { }
}
