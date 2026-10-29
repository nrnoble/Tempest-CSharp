namespace Tempest.Game.Gameplay;

public sealed class Player
{
    public int EdgeIndex { get; private set; } = 7;
    public float EdgePosition { get; private set; } = 0.5f;
    public int Lives { get; set; } = 3;

    public void Update() { }
}
