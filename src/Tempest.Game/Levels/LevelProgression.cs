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

    public static LevelConfiguration? GetDirectStartLevel(int displayedLevel) =>
        GetProductionLevel(displayedLevel);

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
                                        : ReferenceEquals(current, LevelConfiguration.Level9Production)
                                            ? LevelConfiguration.Level10Production
                                            : ReferenceEquals(current, LevelConfiguration.Level10Production)
                                                ? LevelConfiguration.Level11Production
                                                : ReferenceEquals(current, LevelConfiguration.Level11Production)
                                                    ? LevelConfiguration.Level12Production
                                                    : ReferenceEquals(current, LevelConfiguration.Level12Production)
                                                        ? LevelConfiguration.Level13Production
                                                        : ReferenceEquals(current, LevelConfiguration.Level13Production)
                                                            ? LevelConfiguration.Level14Production
                                                            : ReferenceEquals(current, LevelConfiguration.Level14Production)
                                                                ? LevelConfiguration.Level15Production
                                                                : ReferenceEquals(current, LevelConfiguration.Level15Production)
                                                                    ? LevelConfiguration.Level16Production
                                                                    : ReferenceEquals(current, LevelConfiguration.Level16Production)
                                                                        ? LevelConfiguration.Level17Production
                                                                        : null;
}
