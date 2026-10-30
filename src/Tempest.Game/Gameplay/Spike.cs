namespace Tempest.Game.Gameplay;

public sealed class Spike
{
    public int EdgeIndex { get; init; }
    public float Height { get; set; }

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;
    public void Shorten() => Height -= 10;
    public bool IsDepleted => Height <= 0;
    public bool HitsPlayer(Player player) => player.EdgeIndex == EdgeIndex && Height > player.Depth;
}
