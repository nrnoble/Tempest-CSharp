namespace Tempest.Game.Levels;

public readonly record struct Level4AdmissionSnapshot(
    int OrdinaryFlippers,
    int ReleasedCargoFlippers,
    int PendingShotCargoFlippers,
    int ActiveTankers,
    int ActiveSpikers,
    int ActiveOtherEnemies)
{
    public int ActiveFlippers =>
        OrdinaryFlippers + ReleasedCargoFlippers + PendingShotCargoFlippers;

    public int ActiveEnemyCount =>
        ActiveFlippers + ActiveTankers + ActiveSpikers + ActiveOtherEnemies;
}

public static class Level4AdmissionPolicy
{
    public const int PhysicalEnemySlotCount = 7;
    public const int FlipperMinimum = 1;
    public const int FlipperMaximum = 4;
    public const int SpikerMinimum = 1;
    public const int SpikerMaximum = 2;

    public static int GetOpenPhysicalSlots(Level4AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Max(0, PhysicalEnemySlotCount - snapshot.ActiveEnemyCount);
    }

    public static int GetFlipperAvailability(Level4AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Min(
            Math.Max(0, FlipperMaximum - snapshot.ActiveFlippers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static int GetSpikerAvailability(Level4AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Min(
            Math.Max(0, SpikerMaximum - snapshot.ActiveSpikers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static IReadOnlyList<Level3OriginalType> GetOrdinaryEligibleTypes(
        Level4AdmissionSnapshot snapshot)
    {
        int flipperAvailability = GetFlipperAvailability(snapshot);
        int spikerAvailability = GetSpikerAvailability(snapshot);
        List<Level3OriginalType> eligible = [];

        if (snapshot.ActiveSpikers < SpikerMinimum && spikerAvailability > 0)
            return [Level3OriginalType.Spiker];
        if (snapshot.ActiveFlippers < FlipperMinimum && flipperAvailability > 0)
            return [Level3OriginalType.Flipper];

        if (flipperAvailability > 0)
            eligible.Add(Level3OriginalType.Flipper);
        if (spikerAvailability > 0)
            eligible.Add(Level3OriginalType.Spiker);
        return eligible;
    }

    public static Level3OriginalType? SelectOrdinaryType(
        Level4AdmissionSnapshot snapshot,
        Func<int, int, int> randomInclusive)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        IReadOnlyList<Level3OriginalType> eligible = GetOrdinaryEligibleTypes(snapshot);
        if (eligible.Count == 0)
            return null;
        if (eligible.Count == 1)
            return eligible[0];

        // Once both ROM-informed minima are met, choose only among currently eligible types.
        return eligible[randomInclusive(0, eligible.Count - 1)];
    }

    public static bool CanAdmitCargoReentry(Level4AdmissionSnapshot snapshot) =>
        GetOpenPhysicalSlots(snapshot) > 0;

    public static bool CanAdmitRetainedOriginal(
        Level4AdmissionSnapshot snapshot,
        Level3OriginalType retainedType)
    {
        Validate(snapshot);
        if (retainedType is not (Level3OriginalType.Flipper or
            Level3OriginalType.Spiker or Level3OriginalType.Tanker))
            return false;

        // Reentry preserves a committed identity; it is not another ordinary species selection.
        return GetOpenPhysicalSlots(snapshot) > 0;
    }

    private static void Validate(Level4AdmissionSnapshot snapshot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ReleasedCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.PendingShotCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveOtherEnemies);
    }
}
