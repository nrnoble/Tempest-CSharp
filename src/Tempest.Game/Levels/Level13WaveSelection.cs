using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level13WaveSelection
{
    public const int OrdinaryEnemySupply = 26;
    public const int HardDifficultyEnemySupply = 0x1D;
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

    public const byte StartingSpikeHeightRaw = 0xA0;
    public const byte FlipperMovementProgram = 0x53;
    public const byte FlipperSpeedRaw = 0xA8;
    public const byte SpikerSpeedRaw = 0x00;
    public const byte PulsarFireRaw = 0x00;
    public const byte TankerCargoTypeFirst = 0x01;
    public const byte TankerCargoTypeSecond = 0x01;
    public const byte StraightTankerCargoProgram = 0x07;
    public const byte DeepTankerCargoRomAlongThreshold = 0x20;

    // The ROM picker only tries species whose minimum is nonzero; raw maxima alone
    // do not make a species reachable.
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
        bool fuseballAvailable = activeFuseballs < MaximumFuseballs;

        if (activeSpikers < MinimumSpikers && spikerAvailable)
            return Level3OriginalType.Spiker;
        if (activeTankers < MinimumTankers && tankerAvailable)
            return Level3OriginalType.Tanker;
        if (activeFuseballs < MinimumFuseballs && fuseballAvailable)
            return Level3OriginalType.Fuseball;

        if (!spikerAvailable && !tankerAvailable && !fuseballAvailable)
            return null;

        int choice = randomInclusive(0, 2);
        if (choice == 0 && spikerAvailable)
            return Level3OriginalType.Spiker;
        if (choice == 1 && tankerAvailable)
            return Level3OriginalType.Tanker;
        if (choice == 2 && fuseballAvailable)
            return Level3OriginalType.Fuseball;
        if (tankerAvailable)
            return Level3OriginalType.Tanker;
        if (spikerAvailable)
            return Level3OriginalType.Spiker;
        if (fuseballAvailable)
            return Level3OriginalType.Fuseball;
        return null;
    }
}
