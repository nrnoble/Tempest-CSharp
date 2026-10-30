namespace Tempest.Game.Gameplay;

/// <summary>
/// Development-only (<c>--enemy-bullets-test</c>) Flipper bullet. Travels up its lane at 3 per tick;
/// a player shot below it destroys it (25 points). Bullets are not enemies for level accounting and
/// are discarded on death/reset. Constants are Incitti-derived and unvalidated.
/// </summary>
public sealed class EnemyBullet : Enemy
{
    public EnemyBullet(int edgeIndex, float height)
    {
        EdgeIndex = edgeIndex;
        Height = height;
    }

    public float Rotation { get; private set; }
    public bool IsExpired => Height > Levels.Level.Depth;

    public void Advance()
    {
        Height += 3;
        Rotation = (Rotation + 15) % 360;
    }

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public bool HitsPlayer(Player player) =>
        player.EdgeIndex == EdgeIndex && Levels.Level.Depth - Height < 8;

    public override void Update() { }
}
