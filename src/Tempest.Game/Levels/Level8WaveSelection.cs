using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level8WaveSelection
{
    public const int MaximumActiveEnemies = 6;

    private const int MaximumFlippers = 5;
    private const int MinimumTankers = 1;
    private const int MaximumTankers = 2;
    private const int MinimumSpikers = 2;
    private const int MaximumSpikers = 4;

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

        // Post-minimum ROM selection also considers Spike heights; this stable seeded
        // choice is the deliberately narrow project translation for this candidate.
        return randomInclusive(0, 1) == 0
            ? Level3OriginalType.Spiker
            : Level3OriginalType.Tanker;
    }
}
