using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum PulsarState
{
    Climbing = 0,
    Flipping = 1,
    FlippingWithoutClimb = 2,
    LanePause = 3,
    Descending = 4,
    RimPause = 5,
    Terminal = 7
}

public enum PulsarCollision
{
    None,
    Rim,
    PulsingLane
}

/// <summary>
/// Direct-hatched Incitti Pulsar for the isolated development wave. Its state machine keeps the
/// source's numeric states and literal thresholds; the shared pulse clock belongs to TempestGame.
/// </summary>
public sealed class Pulsar : Enemy
{
    public const int PulseFlipSpeed = 12;
    // At source lv=16, all operands are Int: 20-(16/6)=18 and 16-(16/6)=14.
    public const int LanePauseDuration = 18;
    public const int RimPauseDuration = 14;
    private const int FlipDecisionTicks = 50;

    private PulsarState oldState;

    public Pulsar(int edgeIndex = 0, float height = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(edgeIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        if (height > Level.Depth)
            throw new ArgumentOutOfRangeException(nameof(height));

        EdgeIndex = edgeIndex;
        Height = height;
    }

    public PulsarState State { get; private set; }
    public PulsarState ResumeState => oldState;
    public int TimeToFlip { get; private set; }
    public int PauseTicks { get; private set; }
    public int Direction { get; private set; }
    public int? RimPursuitDirection { get; private set; }
    public float TurnAngle { get; private set; }
    public float TotalTurnAngle { get; private set; }
    public DirectedLaneTransition? ActiveTransition { get; private set; }

    public void Update(Level level, int playerEdge, Func<int, int, int> randomInclusive,
        bool hurryUp = false)
    {
        ArgumentNullException.ThrowIfNull(level);
        ArgumentNullException.ThrowIfNull(randomInclusive);
        TimeToFlip++;

        switch (State)
        {
            case PulsarState.Climbing:
                if (Height < Level.Depth)
                {
                    Height = MathF.Min(Level.Depth, Height + 1);
                    // The counter is incremented first, so the strict >50 decision is update 51.
                    if (TimeToFlip > FlipDecisionTicks)
                    {
                        if (randomInclusive(0, 1) == 1)
                            BeginLanePause(PulsarState.Flipping);
                        else
                            BeginRimPause(PulsarState.Climbing);
                    }
                }
                else
                {
                    Height = Level.Depth;
                    // Source rimit redirects only the top decision; Pulsars keep one-unit motion.
                    if (randomInclusive(0, 1) == 1 || hurryUp)
                        BeginLanePause(PulsarState.Flipping);
                    else
                        BeginRimPause(PulsarState.Descending);
                }
                if (Height >= Level.Depth && RimPursuitDirection.HasValue)
                    Direction = RimPursuitDirection.Value;
                else
                    Direction = level.FindReferenceShortestDirection(EdgeIndex, playerEdge, randomInclusive);

                // Arcade pursuit stays committed even though Incitti resamples while climbing.
                if (Height >= Level.Depth && State == PulsarState.LanePause &&
                    oldState == PulsarState.Flipping)
                    RimPursuitDirection ??= Direction;
                else if (Height >= Level.Depth && State == PulsarState.RimPause &&
                    oldState == PulsarState.Descending)
                    RimPursuitDirection = null;
                break;

            case PulsarState.Descending:
                if (Height > 0)
                {
                    Height = MathF.Max(0, Height - 1);
                    if (TimeToFlip > FlipDecisionTicks)
                    {
                        if (randomInclusive(0, 1) == 1)
                            BeginLanePause(PulsarState.Flipping);
                        else
                            BeginRimPause(PulsarState.Descending);
                    }
                }
                else
                {
                    Height = 0;
                    if (randomInclusive(0, 1) == 1)
                        BeginLanePause(PulsarState.Flipping);
                    else
                        BeginRimPause(PulsarState.Climbing);
                }
                Direction = level.FindReferenceShortestDirection(EdgeIndex, playerEdge, randomInclusive);
                RimPursuitDirection = null;
                break;

            case PulsarState.Flipping:
            case PulsarState.FlippingWithoutClimb:
                DirectedLaneTransition transition = ActiveTransition ??
                    throw new InvalidOperationException("A Pulsar flip requires an active directed transition.");
                TurnAngle += PulseFlipSpeed;
                if (State == PulsarState.Flipping && Height < Level.Depth)
                    Height = MathF.Min(Level.Depth, Height + 1);

                TotalTurnAngle = transition.SweepMagnitudeDegrees;
                if (TurnAngle >= TotalTurnAngle)
                {
                    TurnAngle = 0;
                    ActiveTransition = null;
                    oldState = PulsarState.Climbing;
                    State = PulsarState.RimPause;
                    PauseTicks = RimPauseDuration;
                }
                break;

            case PulsarState.LanePause:
                if (--PauseTicks <= 0)
                {
                    TimeToFlip = 0;
                    State = oldState;
                    int sourceEdge = EdgeIndex;
                    MoveToAdjacentEdge(level);
                    ActiveTransition = level.CalculateDirectedLaneTransition(
                        sourceEdge, EdgeIndex, Direction);
                    TotalTurnAngle = ActiveTransition.Value.SweepMagnitudeDegrees;
                }
                break;

            case PulsarState.RimPause:
                if (--PauseTicks <= 0)
                {
                    TimeToFlip = 0;
                    State = oldState;
                }
                break;

            case PulsarState.Terminal:
                break;
        }
    }

    public bool IsHitBy(Shot shot)
    {
        float separation = Height - shot.Height;
        return shot.EdgeIndex == EdgeIndex && separation > 0 && separation < 16;
    }

    public PulsarCollision CheckPlayerCollision(Player player, bool pulsing)
    {
        if (player.EdgeIndex != EdgeIndex)
            return PulsarCollision.None;

        if (Level.Depth - Height < 8)
        {
            if (State is not (PulsarState.Flipping or PulsarState.FlippingWithoutClimb) ||
                IsWithinFlipKillAngle(TotalTurnAngle, TurnAngle))
                return PulsarCollision.Rim;

            return PulsarCollision.None;
        }

        return pulsing && State is PulsarState.Climbing or PulsarState.Descending
            ? PulsarCollision.PulsingLane
            : PulsarCollision.None;
    }

    public void BeginRimPlayerKill() => State = PulsarState.Terminal;

    public override bool TryZap(int delay) => AcceptZap(delay);

    public static bool IsWithinFlipKillAngle(float totalAngle, float angle) =>
        MathF.Abs(totalAngle - angle) < 30;

    public static bool IsLanePulsing(float height, bool pulsing) => height > 16 && pulsing;

    public override void Update() { }

    private void BeginLanePause(PulsarState resume)
    {
        oldState = resume;
        State = PulsarState.LanePause;
        ActiveTransition = null;
        PauseTicks = LanePauseDuration;
        TimeToFlip = 0;
    }

    private void BeginRimPause(PulsarState resume)
    {
        oldState = resume;
        State = PulsarState.RimPause;
        PauseTicks = RimPauseDuration;
        TimeToFlip = 0;
    }

    private void MoveToAdjacentEdge(Level level)
    {
        int laneIndexDelta = Direction > 0 ? -1 : 1;
        if (!level.TryGetAdjacentLane(EdgeIndex, laneIndexDelta, out int nextLane))
        {
            Direction = -Direction;
            if (RimPursuitDirection.HasValue)
                RimPursuitDirection = Direction;
            laneIndexDelta = -laneIndexDelta;
            if (!level.TryGetAdjacentLane(EdgeIndex, laneIndexDelta, out nextLane))
                throw new InvalidOperationException("A Pulsar has no adjacent playable lane.");
        }

        EdgeIndex = nextLane;
    }

}
