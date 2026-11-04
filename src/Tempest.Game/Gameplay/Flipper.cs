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
    RimHandoff
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
    internal int ProgrammedFlipsRemaining => programmedFlipsRemaining;
    internal ProgrammedFlipperRouteStep ProgrammedRouteStep => programmedRouteStep;

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
            UpdateProgrammedRoute(level, playerEdge, tuning);
            return;
        }

        bool canFlip = tuning.CanFlip && !IsTankerCargo;
        switch (State)
        {
            case FlipperState.Climbing:
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
                if (canFlip && startedBelowRim)
                    // Incitti's first canflip climb enters type 3 without assigning a pause.
                    BeginPause(0);
                if (canFlip && startedBelowRim)
                    // The ROM selects this route during movement; the global shot phase runs later.
                    Direction = level.FindReferenceShortestDirection(EdgeIndex, playerEdge, randomInclusive);
                break;

            case FlipperState.Pausing:
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
                TurnAngle += tuning.TurnSpeedDegrees;
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

    // Cargo is born in its mandatory release pivot at Tanker height. Its below-rim direction
    // remains fixed; rim pivots resume player pursuit.
    public static Flipper CreateTankerCargo(
        Level level, TankerCargoSpawn cargo, int cargoUnitId,
        bool useLevel7ProgrammedRoute = false)
    {
        Flipper flipper = new(cargo.SourceLane, cargo.Height)
        {
            SourceEdge = cargo.SourceLane,
            DestinationEdge = cargo.DestinationLane,
            Direction = cargo.Direction,
            State = FlipperState.Flipping,
            CargoUnitId = cargoUnitId
        };
        DirectedLaneTransition transition = level.CalculateDirectedLaneTransition(
            cargo.SourceLane, cargo.DestinationLane, cargo.Direction);
        flipper.ActiveTransition = transition;
        flipper.TotalTurnAngle = transition.SweepMagnitudeDegrees;
        if (useLevel7ProgrammedRoute)
            flipper.InitializeProgrammedCargoRoute(cargo.Direction);
        return flipper;
    }

    // Ordinary climbing Flipper used when cargo re-enters through a fresh Egg after a life loss.
    public static Flipper CreateFreshTankerCargo(
        int edge, int cargoUnitId, int? programmedDirection = null)
    {
        Flipper flipper = new(edge, 0) { CargoUnitId = cargoUnitId };
        if (programmedDirection is int direction)
            flipper.InitializeProgrammedRoute(direction);
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

            default:
                throw new InvalidOperationException(
                    $"Unsupported programmed Flipper route step {programmedRouteStep}.");
        }
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
        DestinationEdge = Direction > 0
            ? level.PreviousEdge(EdgeIndex)
            : level.NextEdge(EdgeIndex);
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
        TurnAngle += tuning.TurnSpeedDegrees;
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

        SourceEdge = EdgeIndex;
        DestinationEdge = Direction > 0
            ? level.PreviousEdge(EdgeIndex)
            : level.NextEdge(EdgeIndex);
        DirectedLaneTransition transition = level.CalculateDirectedLaneTransition(
            SourceEdge, DestinationEdge, Direction);
        ActiveTransition = transition;
        TotalTurnAngle = transition.SweepMagnitudeDegrees;
        TurnAngle = 0;
        State = FlipperState.Flipping;
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
