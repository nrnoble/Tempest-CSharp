using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level9WaveSelection
{
    public const int MaximumActiveEnemies = 6;
    public const int MinimumTankers = 1;
    public const int MaximumTankers = 2;
    public const int MinimumSpikers = 2;
    public const int MaximumSpikers = 4;
    public const int MinimumFlippers = 0;
    public const int MaximumFlippers = 5;
    public const int MinimumPulsars = 0;
    public const int MaximumPulsars = 0;
    public const int MinimumFuseballs = 0;
    public const int MaximumFuseballs = 0;

    public static Level3OriginalType? SelectOriginalType(
        Func<int, int, int> randomInclusive,
        int activeTankers,
        int activeSpikers,
        int activeEnemyUnits,
        int activeFlipperUnitsIncludingLatentCargo)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        ArgumentOutOfRangeException.ThrowIfNegative(activeFlipperUnitsIncludingLatentCargo);

        if (activeEnemyUnits >= MaximumActiveEnemies)
            return null;

        bool spikerAvailable = activeSpikers < MaximumSpikers;
        bool tankerAvailable = activeTankers < MaximumTankers &&
            activeFlipperUnitsIncludingLatentCargo + Tanker.CarriedFlipperCount <=
            MaximumFlippers;
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

        // ROM post-minimum choice also inspects Spike state and random start position.
        // This stable seeded choice is a bounded Level 9 project translation.
        return randomInclusive(0, 1) == 0
            ? Level3OriginalType.Spiker
            : Level3OriginalType.Tanker;
    }

    public static bool ShouldConvertTerminalSpiker(
        LevelConfiguration configuration,
        int pendingGenericUnits,
        int activeTankers,
        int activeFlipperUnitsIncludingLatentCargo)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentOutOfRangeException.ThrowIfNegative(pendingGenericUnits);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeFlipperUnitsIncludingLatentCargo);

        return configuration.UsesLevel9RomWavePolicy &&
            pendingGenericUnits == 0 &&
            activeTankers < MaximumTankers &&
            activeFlipperUnitsIncludingLatentCargo + Tanker.CarriedFlipperCount <=
            MaximumFlippers;
    }
}
