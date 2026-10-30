namespace Tempest.Game.Gameplay;

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
