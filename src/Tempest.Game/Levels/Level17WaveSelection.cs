using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public static class Level17WaveSelection
{
    public const int OrdinaryEnemySupply = 20;
    public const int MaximumPhysicalEnemySlots = 7;

    public const int MinimumFlippers = 0;
    public const int MaximumFlippers = 3;
    public const int MinimumTankers = 1;
    public const int MaximumTankers = 1;
    public const int MinimumSpikers = 0;
    // ROM-BACKED: min/max Spikers are 0/0 for source levels 17-19.
    public const int MaximumSpikers = 0;
    public const int MinimumPulsars = 2;
    public const int MaximumPulsars = 5;
    public const int MinimumFuseballs = 0;
    public const int MaximumFuseballs = 0;

    public const byte FlipperMovementProgram = 0x07;
    public const byte FlipperSpeedRaw = 0xAF;
    public const byte SpikerSpeedRaw = 0x00;
    public const byte TankerCargoType = 0x01;
    public const byte ShotHoldoffRaw = 0x20;
    public const byte DefaultEnemyShotLastSlotIndex = 0x02;
    public const int DefaultMaximumActiveEnemyShots = 3;
    public const float MediumFlipperTankerSpeed = 2.53125f;
    public const float MediumSpikerSpeed = 3f;
    public const float MediumEnemyShotSpeed = 3f;
    public const byte PulsarFlipRateRaw = 0x28;
    public const byte PulseBeatRaw = 0x04;
    public const bool MediumPulsarFiringEnabled = false;

    public static Level3OriginalType? SelectOriginalType(
        Func<int, int, int> randomInclusive,
        int activeTankers,
        int activeSpikers,
        int activePulsars,
        int activeEnemyUnits,
        int activeFlipperUnitsIncludingLatentCargo)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(activePulsars);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        ArgumentOutOfRangeException.ThrowIfNegative(activeFlipperUnitsIncludingLatentCargo);

        List<Level3OriginalType> required = [];
        List<Level3OriginalType> available = [];

        if (activePulsars < MinimumPulsars &&
            CanAdmitOriginalType(Level3OriginalType.Pulsar, activeEnemyUnits))
            required.Add(Level3OriginalType.Pulsar);
        if (activeTankers < MinimumTankers &&
            CanAdmitOriginalType(Level3OriginalType.Tanker, activeEnemyUnits))
            required.Add(Level3OriginalType.Tanker);

        if (activeTankers < MaximumTankers &&
            activeFlipperUnitsIncludingLatentCargo + Tanker.CarriedFlipperCount <= MaximumFlippers &&
            CanAdmitOriginalType(Level3OriginalType.Tanker, activeEnemyUnits))
            available.Add(Level3OriginalType.Tanker);
        if (activePulsars < MaximumPulsars &&
            CanAdmitOriginalType(Level3OriginalType.Pulsar, activeEnemyUnits))
            available.Add(Level3OriginalType.Pulsar);
        if (activeFlipperUnitsIncludingLatentCargo < MaximumFlippers &&
            CanAdmitOriginalType(Level3OriginalType.Flipper, activeEnemyUnits))
            available.Add(Level3OriginalType.Flipper);
        if (activeSpikers < MaximumSpikers &&
            CanAdmitOriginalType(Level3OriginalType.Spiker, activeEnemyUnits))
            available.Add(Level3OriginalType.Spiker);

        IReadOnlyList<Level3OriginalType> choices = required.Count > 0 ? required : available;
        if (choices.Count == 0)
            return null;
        int selected = randomInclusive(0, choices.Count - 1);
        if ((uint)selected >= (uint)choices.Count)
            throw new InvalidOperationException("The Level 17 selector received an out-of-range choice.");
        return choices[selected];
    }

    public static bool CanAdmitOriginalType(Level3OriginalType type, int activeEnemyUnits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        int additionalSlots = type == Level3OriginalType.Tanker ? 2 : 1;
        return activeEnemyUnits + additionalSlots <= MaximumPhysicalEnemySlots;
    }

    public static bool CanAdmitCargoFlipper(int activeEnemyUnits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        return activeEnemyUnits + 1 <= MaximumPhysicalEnemySlots;
    }
}
