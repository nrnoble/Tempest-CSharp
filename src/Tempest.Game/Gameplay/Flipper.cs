using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum FlipperState
{
    Climbing,
    Pausing,
    Flipping
}

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
    public bool CompletedIntoPlayer { get; private set; }
    public int BulletAmmunition { get; private set; }
    public int BulletTimer { get; private set; }

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
                    BeginPause();
                }
                bullet = TryFireBullet(EdgeIndex, randomInclusive);
                break;

            case FlipperState.Pausing:
                PauseTicks--;
                if (PauseTicks <= 0)
                {
                    BeginPivot(level, playerEdge);
                    bullet = TryFireBullet(DestinationEdge, randomInclusive);
                }
                break;

            case FlipperState.Flipping:
                TurnAngle += TurnSpeedDegrees;
                if (TurnAngle >= TotalTurnAngle)
                    CompletePivot(playerEdge);
                break;
        }

        return bullet;
    }

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
        bool matchingEdge = shot.EdgeIndex == EdgeIndex;
        if (State == FlipperState.Flipping)
            matchingEdge = shot.EdgeIndex == SourceEdge || shot.EdgeIndex == DestinationEdge;

        if (!matchingEdge)
            return false;

        float separation = Height - shot.Height;
        return !(separation <= 0 || separation >= 16);
    }

    public void ClearCompletedCollision() => CompletedIntoPlayer = false;

    private void BeginPause()
    {
        State = FlipperState.Pausing;
        PauseTicks = RimPauseTicks;
        TurnAngle = 0;
    }

    private void BeginPivot(Level level, int playerEdge)
    {
        Direction = level.FindShortestDirection(EdgeIndex, playerEdge);
        SourceEdge = EdgeIndex;
        DestinationEdge = Direction > 0
            ? level.PreviousEdge(EdgeIndex)
            : level.NextEdge(EdgeIndex);
        TotalTurnAngle = level.CalculateEdgeTurnAngle(SourceEdge, DestinationEdge);
        TurnAngle = 0;
        State = FlipperState.Flipping;
    }

    private void CompletePivot(int playerEdge)
    {
        EdgeIndex = DestinationEdge;
        SourceEdge = EdgeIndex;
        TurnAngle = 0;
        TotalTurnAngle = 0;

        if (EdgeIndex == playerEdge)
        {
            CompletedIntoPlayer = true;
            return;
        }

        BeginPause();
    }

    public override void Update() { }
}
