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
    public float TurnAngle { get; private set; }
    public float TotalTurnAngle { get; private set; }

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
                Direction = FindShortDirection(level, EdgeIndex, playerEdge, randomInclusive);
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
                Direction = FindShortDirection(level, EdgeIndex, playerEdge, randomInclusive);
                break;

            case PulsarState.Flipping:
            case PulsarState.FlippingWithoutClimb:
                // Source state 2 is modeled for its draw/collision rules, but no normal transition
                // in the inspected direct-hatch path enters it.
                TurnAngle += PulseFlipSpeed;
                if (State == PulsarState.Flipping && Height < Level.Depth)
                    Height = MathF.Min(Level.Depth, Height + 1);

                TotalTurnAngle = GetTurnAngle(level, EdgeIndex, Direction);
                if (TurnAngle >= TotalTurnAngle)
                {
                    TurnAngle = 0;
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
                    MoveToAdjacentEdge(level);
                    TotalTurnAngle = GetTurnAngle(level, EdgeIndex, Direction);
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

    public static bool IsWithinFlipKillAngle(float totalAngle, float angle) =>
        MathF.Abs(totalAngle - angle) < 30;

    public static bool IsLanePulsing(float height, bool pulsing) => height > 16 && pulsing;

    public override void Update() { }

    private void BeginLanePause(PulsarState resume)
    {
        oldState = resume;
        State = PulsarState.LanePause;
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
        int edgeCount = level.Continuous
            ? level.Definition.Points.Count
            : level.Definition.Points.Count - 1;
        int lastEdge = edgeCount - 1;

        // On an open tube, reverse before stepping beyond an endpoint; closed tubes wrap instead.
        if (!level.Continuous && (EdgeIndex - Direction < 0 || EdgeIndex - Direction > lastEdge))
            Direction = -Direction;

        if (Direction == 1)
            EdgeIndex = EdgeIndex == 0 ? lastEdge : EdgeIndex - 1;
        else
            EdgeIndex = EdgeIndex == lastEdge ? 0 : EdgeIndex + 1;

        if (!level.Continuous)
            EdgeIndex = Math.Clamp(EdgeIndex, 0, lastEdge);
    }

    private static int FindShortDirection(Level level, int fromEdge, int toEdge,
        Func<int, int, int> randomInclusive)
    {
        int direction;
        if (level.Continuous)
        {
            direction = -(toEdge - fromEdge);
            direction = Math.Abs(direction) > 8 ? -Math.Sign(direction) : Math.Sign(direction);
        }
        else
        {
            direction = -Math.Sign(toEdge - fromEdge);
        }

        // Incitti's Rand(0,1) is called for same-edge ties on every vertical movement update.
        return direction == 0 ? 1 - randomInclusive(0, 1) * 2 : direction;
    }

    private static float GetTurnAngle(Level level, int sourceEdge, int direction)
    {
        int edgeCount = level.Continuous
            ? level.Definition.Points.Count
            : level.Definition.Points.Count - 1;
        int destinationEdge = direction == 1
            ? (sourceEdge - 1 + edgeCount) % edgeCount
            : (sourceEdge + 1) % edgeCount;
        return level.CalculateEdgeTurnAngle(sourceEdge, destinationEdge);
    }
}
