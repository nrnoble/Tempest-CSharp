namespace Tempest.Game.Levels;

public static class LevelProgression
{
    // Keep direct-start validation tied to the same chain used by live campaign progression.
    public static LevelConfiguration? GetProductionLevel(int displayedLevel)
    {
        if (displayedLevel < LevelConfiguration.Level1.DisplayedLevel)
            return null;

        LevelConfiguration current = LevelConfiguration.Level1;
        while (current.DisplayedLevel < displayedLevel)
        {
            LevelConfiguration? next = GetNextProductionLevel(current);
            if (next is null)
                return null;
            current = next;
        }

        return current.DisplayedLevel == displayedLevel ? current : null;
    }

    public static LevelConfiguration GetLastProductionLevel()
    {
        LevelConfiguration current = LevelConfiguration.Level1;
        while (GetNextProductionLevel(current) is LevelConfiguration next)
            current = next;
        return current;
    }

    public static LevelConfiguration? GetNextProductionLevel(LevelConfiguration current) =>
        ReferenceEquals(current, LevelConfiguration.Level1)
            ? LevelConfiguration.Level2Square
            : ReferenceEquals(current, LevelConfiguration.Level2Square)
                ? LevelConfiguration.Level3Mixed
                : ReferenceEquals(current, LevelConfiguration.Level3Mixed)
                    ? LevelConfiguration.Level4Candidate
                    : ReferenceEquals(current, LevelConfiguration.Level4Candidate)
                        ? LevelConfiguration.Level5Production
                        : ReferenceEquals(current, LevelConfiguration.Level5Production)
                            ? LevelConfiguration.Level6Production
                            : ReferenceEquals(current, LevelConfiguration.Level6Production)
                                ? LevelConfiguration.Level7Production
                                : ReferenceEquals(current, LevelConfiguration.Level7Production)
                                    ? LevelConfiguration.Level8Production
                                    : ReferenceEquals(current, LevelConfiguration.Level8Production)
                                        ? LevelConfiguration.Level9Production
                                        : null;
}
