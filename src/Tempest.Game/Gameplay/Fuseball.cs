using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum FuseballState
{
    Lateral,
    Climbing,
    Descending,
    TerminalRimward,
    KillingPlayer
}

/// <summary>
/// Direct-hatched Incitti Fuseball. Moves across lane boundaries, alternating lateral travel
/// with vertical excursions. No Tanker cargo or Zapper behavior is implemented.
/// </summary>
public sealed class Fuseball : Enemy
{
    // Unscaled term of Incitti's 3.3 + lv/20; this development wave does not simulate progression.
    public const float DevelopmentClimbSpeed = 3.3f;

    public Fuseball(int edgeIndex = 0, float climbSpeed = DevelopmentClimbSpeed,
        int originalSlotId = -1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(climbSpeed);
        EdgeIndex = edgeIndex;
        ClimbSpeed = climbSpeed;
        OriginalSlotId = originalSlotId;
    }

    public int OriginalSlotId { get; }
    public FuseballState State { get; private set; }
    // w/8 interpolates from the lane's first tube point to its next point, not its midpoint.
    // At 0/8 the cycling Fuseball hugs a radial boundary and cannot be shot or kill the player.
    public int W { get; private set; }
    public int Direction { get; private set; } = 1;
    public int StateTicks { get; private set; }
    public float ClimbSpeed { get; }
    public int KillScore => (int)((Level.Depth - Height) / 100 + 1) * 250;

    public void Update(Level level, int playerEdge, Func<int, int, int> randomInclusive,
        bool hurryUp = false, int? pendingGenericSupply = null)
    {
        ArgumentNullException.ThrowIfNull(level);
        ArgumentNullException.ThrowIfNull(randomInclusive);
        if (pendingGenericSupply is < 0)
            throw new ArgumentOutOfRangeException(nameof(pendingGenericSupply));

        StateTicks++;
        switch (State)
        {
            case FuseballState.Lateral:
                // Source increments cnt before testing cnt > 8: the actual cadence is nine ticks.
                if (StateTicks <= 8)
                    break;
                if (randomInclusive(0, 100) > 90)
                {
                    int distance = playerEdge - EdgeIndex;
                    Direction = distance == 0 ? 2 * randomInclusive(0, 1) - 1 : Math.Sign(distance);
                    if (level.Continuous && Math.Abs(distance) > level.PlayableLaneCount / 2)
                        Direction = -Direction;
                }
                StateTicks = 0;
                W += Direction;
                if (Direction == 1 && W >= 8)
                {
                    if (level.TryGetAdjacentLane(EdgeIndex, 1, out int nextLane))
                    {
                        W = 0;
                        EdgeIndex = nextLane;
                    }
                    else
                    {
                        W = 8;
                        Direction = -Direction;
                    }
                }
                else if (Direction == -1 && W <= 0)
                {
                    if (level.TryGetAdjacentLane(EdgeIndex, -1, out int previousLane))
                    {
                        W = 8;
                        EdgeIndex = previousLane;
                    }
                    else
                    {
                        W = 0;
                        Direction = -Direction;
                    }
                }
                if (W is 0 or 8)
                {
                    State = randomInclusive(1, 2) == 1
                        ? FuseballState.Climbing
                        : FuseballState.Descending;
                    if (hurryUp || Height == 0)
                        State = FuseballState.Climbing;
                }
                break;

            case FuseballState.Climbing:
                if (Height < Level.Depth)
                {
                    if (pendingGenericSupply == 0 &&
                        Height + ClimbSpeed >= Level.Depth)
                    {
                        // PROJECT TRANSLATION: Climbing is the toward-player/rim leg. C# has no
                        // along-tube player_along coordinate, so its final rimward step is the
                        // closest state boundary; test before normal endpoint/reversal handling.
                        State = FuseballState.TerminalRimward;
                        StateTicks = 0;
                        break;
                    }

                    // Incitti applies its existing climb term twice under rimit, not descent speed.
                    Height += ClimbSpeed;
                    if (hurryUp)
                        Height += ClimbSpeed;
                    Height = MathF.Min(Level.Depth, Height);
                    CheckVerticalDuration(randomInclusive);
                }
                else if (pendingGenericSupply == 0)
                {
                    State = FuseballState.TerminalRimward;
                    StateTicks = 0;
                }
                else
                    ResumeLateral();
                break;

            case FuseballState.Descending:
                // Incitti keeps rolling at height zero; it does not immediately resume lateral.
                Height = MathF.Max(0, Height - ClimbSpeed);
                CheckVerticalDuration(randomInclusive);
                break;

            case FuseballState.TerminalRimward:
                // PROJECT TRANSLATION: the ROM retains its toward-player direction; C# represents
                // that as normal-speed height growth, with no later random return to cycling.
                Height = MathF.Min(Level.Depth, Height + ClimbSpeed);
                break;

            case FuseballState.KillingPlayer:
                W = 3;
                break;
        }
    }

    private void CheckVerticalDuration(Func<int, int, int> randomInclusive)
    {
        // Incitti redraws this threshold every update, rather than choosing a duration on entry.
        if (StateTicks > 25 * randomInclusive(1, 3))
            ResumeLateral();
    }

    private void ResumeLateral()
    {
        State = FuseballState.Lateral;
        StateTicks = 0;
    }

    public bool IsHitBy(Shot shot)
    {
        float separation = Height - shot.Height;
        // Terminal movement holds at a lane endpoint instead of cycling W through its interior.
        bool isShootable = State == FuseballState.TerminalRimward || W > 1 && W < 7;
        return shot.EdgeIndex == EdgeIndex && separation > 0 && separation < 16 && isShootable;
    }

    // Only lateral rim travel is lethal; neither vertical state nor player sub-lane position
    // participates in Incitti's rule. Keep this separate from Flipper's rim-arrival collision.
    public bool HitsPlayer(Player player) => player.EdgeIndex == EdgeIndex &&
        State == FuseballState.Lateral && Height == Level.Depth && W > 1 && W < 7;

    public void BeginPlayerKill()
    {
        State = FuseballState.KillingPlayer;
        W = 3;
    }

    public override bool TryZap(int delay) => AcceptZap(delay);

    public override void Update() { }
}
