namespace Tempest.Game.Gameplay;

/// <summary>
/// Spike left behind by a Spiker. Shots shorten it by 10; it hurts a player in its lane whose depth
/// is below the spike tip. Spikes persist across life loss and are not enemies for accounting.
/// </summary>
public sealed class Spike
{
    public int EdgeIndex { get; init; }
    public float Height { get; set; }

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;
    public void Shorten() => Height -= 10;
    public bool IsDepleted => Height <= 0;
    public bool HitsPlayer(Player player) => player.EdgeIndex == EdgeIndex && Height > player.Depth;
}
