using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level12WaveSelection
{
    public const int OrdinaryEnemySupply = 24;
    public const int HardDifficultyEnemySupply = 27;
    public const int MaximumActiveEnemies = 6;
    public const int MaximumPhysicalEnemySlots = 7;

    public const int MinimumTankers = 1;
    public const int MaximumTankers = 1;
    public const int MinimumSpikers = 1;
    public const int MaximumSpikers = 2;
    public const int MinimumFlippers = 0;
    public const int MaximumFlippers = 5;
    // Raw parameter-table values only; Level 12 production does not select Pulsars.
    public const int MinimumPulsars = 2;
    public const int MaximumPulsars = 2;
    public const int MinimumFuseballs = 1;
    public const int MaximumFuseballs = 1;

    public const byte StartingSpikeHeightRaw = 0xA8;
    public const byte FlipperMovementProgram = 0x19;
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

        if (!spikerAvailable && !tankerAvailable && !flipperAvailable && !fuseballAvailable)
            return null;

        int choice = randomInclusive(0, 3);
        if (choice == 0 && spikerAvailable)
            return Level3OriginalType.Spiker;
        if (choice == 1 && tankerAvailable)
            return Level3OriginalType.Tanker;
        if (choice == 2 && fuseballAvailable)
            return Level3OriginalType.Fuseball;
        if (flipperAvailable)
            return Level3OriginalType.Flipper;
        if (tankerAvailable)
            return Level3OriginalType.Tanker;
        if (spikerAvailable)
            return Level3OriginalType.Spiker;
        if (fuseballAvailable)
            return Level3OriginalType.Fuseball;
        return null;
    }
}
