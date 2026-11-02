namespace Tempest.Game.Levels;

public static class LevelProgression
{
    public static LevelConfiguration? GetNextProductionLevel(LevelConfiguration current) =>
        ReferenceEquals(current, LevelConfiguration.Level1)
            ? LevelConfiguration.Level2Square
            : ReferenceEquals(current, LevelConfiguration.Level2Square)
                ? LevelConfiguration.Level3Mixed
                : ReferenceEquals(current, LevelConfiguration.Level3Mixed)
                    ? LevelConfiguration.Level4Candidate
                    : null;
}
