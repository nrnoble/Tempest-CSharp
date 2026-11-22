using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level11WaveSelection
{
    public const int OrdinaryEnemySupply = 27;
    public const int HardDifficultyEnemySupply = 30;
    public const int MaximumActiveEnemies = 6;
    public const int MaximumPhysicalEnemySlots = 7;

    public const int MinimumTankers = 1;
    public const int MaximumTankers = 2;
    public const int MinimumSpikers = 2;
    public const int MaximumSpikers = 3;
    public const int MinimumFlippers = 0;
    public const int MaximumFlippers = 5;
    public const int MinimumPulsars = 0;
    public const int MaximumPulsars = 0;
    public const int MinimumFuseballs = 1;
    public const int MaximumFuseballs = 1;

    public const byte StartingSpikeHeightRaw = 0xB0;
    public const byte FlipperMovementProgram = 0x24;
    public const byte StraightTankerCargoProgram = 0x07;
    public const byte DeepTankerCargoRomAlongThreshold = 0x20;

    public static Level3OriginalType? SelectOriginalType(
        Func<int, int, int> randomInclusive,
        int activeTankers,
        int activeSpikers,
        int activeFuseballs,
        int activeEnemyUnits,
        int activeFlipperUnitsIncludingLatentCargo)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeFuseballs);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        ArgumentOutOfRangeException.ThrowIfNegative(activeFlipperUnitsIncludingLatentCargo);

        if (activeEnemyUnits >= MaximumActiveEnemies)
            return null;

        bool spikerAvailable = activeSpikers < MaximumSpikers;
        bool tankerAvailable = activeTankers < MaximumTankers &&
            activeFlipperUnitsIncludingLatentCargo + Tanker.CarriedFlipperCount <= MaximumFlippers;
        bool flipperAvailable = activeFlipperUnitsIncludingLatentCargo < MaximumFlippers;
        bool fuseballAvailable = activeFuseballs < MaximumFuseballs;

        if (activeSpikers < MinimumSpikers && spikerAvailable)
            return Level3OriginalType.Spiker;
        if (activeTankers < MinimumTankers && tankerAvailable)
            return Level3OriginalType.Tanker;
        if (activeFuseballs < MinimumFuseballs && fuseballAvailable)
            return Level3OriginalType.Fuseball;

        if (!spikerAvailable && !tankerAvailable && !flipperAvailable)
            return null;

        int choice = randomInclusive(0, 2);
        if (choice == 0 && spikerAvailable)
            return Level3OriginalType.Spiker;
        if (choice == 1 && tankerAvailable)
            return Level3OriginalType.Tanker;
        if (flipperAvailable)
            return Level3OriginalType.Flipper;
        if (tankerAvailable)
            return Level3OriginalType.Tanker;
        if (spikerAvailable)
            return Level3OriginalType.Spiker;
        return null;
    }
}
