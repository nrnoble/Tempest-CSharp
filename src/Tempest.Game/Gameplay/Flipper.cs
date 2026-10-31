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

/// <summary>
/// Flipper state machine (Climbing -> Pausing -> Flipping -> Pausing ...). Timing constants are
/// configuration-backed Incitti values, not measured arcade timing. Ordinary Level 2-test canflip
/// latches direction after the first climb and resamples only at rim pause expiry.
/// The entity never touches game state: lethal contact is reported through
/// <see cref="CompletedIntoPlayer"/> and consumed by TempestGame after player-shot collision.
/// </summary>
public sealed class Flipper : Enemy
{
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
        FlipperConfiguration tuning = level.Configuration.Flipper;
        bool canFlip = tuning.CanFlip && !IsTankerCargo;
        EnemyBullet? bullet = null;
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
                bullet = TryFireBullet(EdgeIndex, randomInclusive);
                if (canFlip && startedBelowRim)
                    // Source order is Shoot(), then FindShortDir. Retain this below the rim.
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
                    bullet = TryFireBullet(DestinationEdge, randomInclusive);
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

    private void BeginPause(int pauseTicks)
    {
        SourceEdge = EdgeIndex;
        DestinationEdge = EdgeIndex;
        TotalTurnAngle = 0;
        State = FlipperState.Pausing;
        PauseTicks = pauseTicks;
        TurnAngle = 0;
    }

    // +1 pivots toward PreviousEdge. Below-rim canflip/cargo retain direction; rim starts resample.
    private void BeginPivot(Level level, int playerEdge, Func<int, int, int>? randomInclusive)
    {
        bool canFlip = level.Configuration.FlipperCanFlip && !IsTankerCargo;
        if ((!IsTankerCargo && !canFlip) || Height >= Level.Depth)
            Direction = canFlip
                ? level.FindReferenceShortestDirection(EdgeIndex, playerEdge, randomInclusive)
                : level.FindShortestDirection(EdgeIndex, playerEdge);
        SourceEdge = EdgeIndex;
        DestinationEdge = Direction > 0
            ? level.PreviousEdge(EdgeIndex)
            : level.NextEdge(EdgeIndex);
        TotalTurnAngle = level.CalculateFlipperTurnAngle(SourceEdge, DestinationEdge);
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
