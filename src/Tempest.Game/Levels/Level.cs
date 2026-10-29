namespace Tempest.Game.Levels;

public sealed class Level
{
    public const float Depth = 400f;
    public LevelState State { get; set; } = LevelState.Begin;
    public bool Continuous { get; set; } = true;
    public void Update() { }
}
