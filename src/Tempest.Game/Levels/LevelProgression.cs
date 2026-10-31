namespace Tempest.Game.Levels;

public static class LevelProgression
{
    public static LevelConfiguration? GetNextProductionLevel(LevelConfiguration current) =>
        ReferenceEquals(current, LevelConfiguration.Level1)
            ? LevelConfiguration.Level2Square
            : null;
}
