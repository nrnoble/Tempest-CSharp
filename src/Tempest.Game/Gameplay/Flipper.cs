using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

// Climbing: rises up the lane. Pausing: rim pause before choosing a pivot direction.
// Flipping: visible pivot to an adjacent lane.
public enum FlipperState
{
    Climbing,
    Pausing,
    Flipping
}

/// <summary>
/// Flipper state machine (Climbing -> Pausing -> Flipping -> Pausing ...). Timing constants are
/// frame/tick-based values taken from Incitti's BlitzMax reference and not yet compared to the arcade.
/// The entity never touches game state: lethal contact is reported through
/// <see cref="CompletedIntoPlayer"/> and consumed by TempestGame after player-shot collision.
/// </summary>
public sealed class Flipper : Enemy
{
    private const float ClimbSpeed = 2;
    private const float TurnSpeedDegrees = 8;
    private const int RimPauseTicks = 24;
    public Flipper(int edge = 0, float height = 0)
        : this(edge, height, 0)
    {
    }

    public Flipper(int edge, float height, int bulletAmmunition)
    {
        EdgeIndex = edge;
        SourceEdge = edge;
        DestinationEdge = edge;
        Height = height;
        BulletAmmunition = Math.Max(0, bulletAmmunition);
        BulletTimer = 16;
        State = FlipperState.Climbing;
    }

    public int SourceEdge { get; private set; }
    public int DestinationEdge { get; private set; }
    public FlipperState State { get; private set; }
    public int PauseTicks { get; private set; }
    public int Direction { get; private set; }
    public float TurnAngle { get; private set; }
    public float TotalTurnAngle { get; private set; }
    // Single pending-player-collision signal. Set on a direct rim arrival in the player's lane
    // (Incitti CheckPlayerColl: unconditional for a climbing Flipper) and on a pivot completing
    // into the player's lane at full depth. A player who leaves before either moment is safe.
    public bool CompletedIntoPlayer { get; private set; }
    public int BulletAmmunition { get; private set; }
    public int BulletTimer { get; private set; }
    // Tanker identity: slot of the original enemy this unit stands for (-1 if none) and cargo
    // unit id (-1 if not Tanker cargo). Used by the development Tanker ledger only.
    public int OriginalSlotId { get; private set; } = -1;
    public int CargoUnitId { get; private set; } = -1;
    public bool IsTankerCargo => CargoUnitId >= 0;

    public EnemyBullet? Update(Level level, int playerEdge, Func<int, int, int>? randomInclusive = null)
    {
        EnemyBullet? bullet = null;
        switch (State)
        {
            case FlipperState.Climbing:
                Height += ClimbSpeed;
                if (Height >= Level.Depth)
                {
                    Height = Level.Depth;
                    if (EdgeIndex == playerEdge)
                        CompletedIntoPlayer = true;
                    else
                        BeginPause();
                }
                bullet = TryFireBullet(EdgeIndex, randomInclusive);
                break;

            case FlipperState.Pausing:
                if (IsTankerCargo && Height < Level.Depth)
                    Height = MathF.Min(Level.Depth, Height + 1);
                PauseTicks--;
                if (PauseTicks <= 0)
                {
                    BeginPivot(level, playerEdge);
                    bullet = TryFireBullet(DestinationEdge, randomInclusive);
                }
                break;

            case FlipperState.Flipping:
                TurnAngle += TurnSpeedDegrees;
                if (Height < Level.Depth)
                {
                    Height += ClimbSpeed;
                    if (Height >= Level.Depth)
                        Height = Level.Depth;
                }
                if (TurnAngle >= TotalTurnAngle)
                    CompletePivot(playerEdge);
                break;
        }

        return bullet;
    }

    // Cargo is born in its mandatory release pivot at Tanker height. Its below-rim direction
    // remains fixed; rim pivots resume player pursuit.
    public static Flipper CreateTankerCargo(
        Level level, TankerCargoSpawn cargo, int cargoUnitId, int bulletAmmunition = 0)
    {
        Flipper flipper = new(cargo.SourceLane, cargo.Height, bulletAmmunition)
        {
            SourceEdge = cargo.SourceLane,
            DestinationEdge = cargo.DestinationLane,
            Direction = cargo.Direction,
            TotalTurnAngle = level.CalculateEdgeTurnAngle(cargo.SourceLane, cargo.DestinationLane),
            State = FlipperState.Flipping,
            CargoUnitId = cargoUnitId
        };
        return flipper;
    }

    internal void SetBulletAmmunition(int ammunition) => BulletAmmunition = Math.Max(0, ammunition);

    // Ordinary climbing Flipper used when cargo re-enters through a fresh Egg after a life loss.
    public static Flipper CreateFreshTankerCargo(int edge, int cargoUnitId, int bulletAmmunition = 0) => new(
        edge, 0, bulletAmmunition)
    {
        CargoUnitId = cargoUnitId
    };

    internal void SetOriginalSlotId(int originalSlotId) => OriginalSlotId = originalSlotId;

    private EnemyBullet? TryFireBullet(int edgeIndex, Func<int, int, int>? randomInclusive)
    {
        if (BulletAmmunition <= 0 || randomInclusive is null)
            return null;

        BulletTimer--;
        if (BulletTimer >= 0)
            return null;

        BulletTimer = 16;
        if (randomInclusive(0, 100) <= 75)
            return null;

        BulletAmmunition--;
        return new EnemyBullet(edgeIndex, Height);
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

    private void BeginPause()
    {
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        TotalTurnAngle = 0;
        State = FlipperState.Pausing;
        PauseTicks = RimPauseTicks;
        TurnAngle = 0;
    }

    // Direction > 0 pivots toward PreviousEdge; the direction is chosen once, at pivot start.
    private void BeginPivot(Level level, int playerEdge)
    {
        if (!IsTankerCargo || Height >= Level.Depth)
            Direction = level.FindShortestDirection(EdgeIndex, playerEdge);
        SourceEdge = EdgeIndex;
        DestinationEdge = Direction > 0
            ? level.PreviousEdge(EdgeIndex)
            : level.NextEdge(EdgeIndex);
        TotalTurnAngle = level.CalculateFlipperTurnAngle(SourceEdge, DestinationEdge);
        TurnAngle = 0;
        State = FlipperState.Flipping;
    }

    private void CompletePivot(int playerEdge)
    {
        EdgeIndex = DestinationEdge;
        SourceEdge = EdgeIndex;
        TurnAngle = 0;
        TotalTurnAngle = 0;

        // Lethal only if the player is in the destination lane at completion.
        if (EdgeIndex == playerEdge && Height >= Level.Depth)
            CompletedIntoPlayer = true;

        BeginPause();
    }

    public override void Update() { }
}
