namespace Tempest.Game.Levels;

public static class Level7WaveSelection
{
    public const int MaximumActiveEnemies = 6;

    private const int MinimumTankers = 1;
    private const int MaximumTankers = 2;
    private const int MinimumSpikers = 2;
    private const int MaximumSpikers = 4;

    public static Level3OriginalType? SelectOriginalType(
        Func<int, int, int> randomInclusive,
        int activeTankers,
        int activeSpikers,
        int activeEnemyUnits)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);

        int availableEnemySlots = MaximumActiveEnemies - activeEnemyUnits;
        if (availableEnemySlots <= 0)
            return null;

        // The ROM's zero Flipper minimum blocks ordinary bottom selection.
        bool spikerAvailable = activeSpikers < MaximumSpikers;
        bool tankerAvailable = activeTankers < MaximumTankers;

        if (!spikerAvailable && !tankerAvailable)
            return null;

        if (spikerAvailable && activeSpikers < MinimumSpikers)
            return Level3OriginalType.Spiker;

        if (tankerAvailable && activeTankers < MinimumTankers)
            return Level3OriginalType.Tanker;

        if (!spikerAvailable)
            return Level3OriginalType.Tanker;
        if (!tankerAvailable)
            return Level3OriginalType.Spiker;

        return randomInclusive(0, 1) == 0
            ? Level3OriginalType.Spiker
            : Level3OriginalType.Tanker;
    }
}
