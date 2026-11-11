using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public readonly record struct Level7AdmissionSnapshot(
    int OrdinaryFlippers,
    int ReleasedCargoFlippers,
    int PendingShotCargoFlippers,
    int OrdinaryTankers,
    int ConvertedTankers,
    int ActiveSpikers,
    int ActiveOtherEnemies,
    int LatentTankerCargoFlippers,
    double? PendingSegmentSpikeRomComparisonValue)
{
    public int ActiveFlippers =>
        OrdinaryFlippers + ReleasedCargoFlippers + PendingShotCargoFlippers;

    public int ActiveEnemyCount =>
        ActiveFlippers + OrdinaryTankers + ConvertedTankers +
        ActiveSpikers + ActiveOtherEnemies;
}

public static class Level7WaveSelection
{
    public const int PhysicalEnemySlotCount = 7;
    public const int FlipperMinimum = 0;
    public const int FlipperMaximum = 5;
    public const int TankerMinimum = 1;
    public const int TankerMaximum = 2;
    public const int SpikerMinimum = 2;
    public const int SpikerMaximum = 4;
    public const int PendingSegmentSpikeRomThreshold = 0xCC;

    public static bool ShouldConvertTerminalSpiker(
        LevelConfiguration configuration,
        int pendingGenericOriginals)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentOutOfRangeException.ThrowIfNegative(pendingGenericOriginals);
        return ReferenceEquals(configuration, LevelConfiguration.Level7Production) &&
            pendingGenericOriginals == 0;
    }

    public static double GetPendingSegmentSpikeRomComparisonValue(Level level, int pendingLane)
    {
        ArgumentNullException.ThrowIfNull(level);
        if (!level.IsPlayableLane(pendingLane))
            throw new ArgumentOutOfRangeException(nameof(pendingLane));

        Spike? pendingSegmentSpike = level.Spikes.SingleOrDefault(
            spike => spike.EdgeIndex == pendingLane);
        // PROJECT TRANSLATION: ROM-along conversion is approximate; zero spike_ht maps to $ff.
        return pendingSegmentSpike is null || pendingSegmentSpike.IsDepleted
            ? 0xFF
            : Tanker.RomAlongPositionForHeight(pendingSegmentSpike.Height);
    }

    public static int GetOpenPhysicalSlots(Level7AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Max(0, PhysicalEnemySlotCount - snapshot.ActiveEnemyCount);
    }

    public static int GetTankerCargoChildCapacity(
        LevelConfiguration configuration,
        Level7AdmissionSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (!configuration.UsesLevel7RomWavePolicy)
            return Tanker.CarriedFlipperCount;

        return Math.Min(Tanker.CarriedFlipperCount, GetOpenPhysicalSlots(snapshot));
    }

    public static int GetFlipperAvailability(Level7AdmissionSnapshot snapshot)
    {
        Validate(snapshot);

        // The ROM saturates max-minus-live at zero, then applies cargo subtraction as a byte.
        byte availability = (byte)Math.Max(0, FlipperMaximum - snapshot.ActiveFlippers);
        availability = unchecked((byte)(
            availability - snapshot.LatentTankerCargoFlippers));
        return Math.Min(availability, GetOpenPhysicalSlots(snapshot));
    }

    public static int GetTankerAvailability(Level7AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        if (GetFlipperAvailability(snapshot) == 0)
            return 0;

        return Math.Min(
            Math.Max(0, TankerMaximum - snapshot.OrdinaryTankers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static int GetSpikerAvailability(Level7AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Min(
            Math.Max(0, SpikerMaximum - snapshot.ActiveSpikers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static IReadOnlyList<Level3OriginalType> GetOrdinaryEligibleTypes(
        Level7AdmissionSnapshot snapshot)
    {
        int tankerAvailability = GetTankerAvailability(snapshot);
        int spikerAvailability = GetSpikerAvailability(snapshot);

        // ROM checks positive minima in descending species order: Spiker before Tanker.
        if (snapshot.ActiveSpikers < SpikerMinimum && spikerAvailability > 0)
            return [Level3OriginalType.Spiker];
        if (snapshot.OrdinaryTankers < TankerMinimum && tankerAvailability > 0)
            return [Level3OriginalType.Tanker];

        List<Level3OriginalType> eligible = [];
        if (tankerAvailability > 0)
            eligible.Add(Level3OriginalType.Tanker);
        if (spikerAvailability > 0)
            eligible.Add(Level3OriginalType.Spiker);
        return eligible;
    }

    public static bool CanAdmitRetainedOriginal(
        Level7AdmissionSnapshot snapshot,
        Level3OriginalType retainedType,
        bool isConvertedTanker = false)
    {
        Validate(snapshot);
        return retainedType switch
        {
            Level3OriginalType.Tanker when isConvertedTanker =>
                GetOpenPhysicalSlots(snapshot) > 0,
            Level3OriginalType.Tanker => GetTankerAvailability(snapshot) > 0,
            Level3OriginalType.Spiker => GetSpikerAvailability(snapshot) > 0,
            _ => false
        };
    }

    public static Level3OriginalType? SelectOrdinaryType(
        Level7AdmissionSnapshot snapshot,
        Func<int, int, int> randomInclusive)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        IReadOnlyList<Level3OriginalType> eligible = GetOrdinaryEligibleTypes(snapshot);
        if (eligible.Count == 0)
            return null;
        if (eligible.Count == 1)
            return eligible[0];

        if (snapshot.PendingSegmentSpikeRomComparisonValue is double comparisonValue)
        {
            return comparisonValue >= PendingSegmentSpikeRomThreshold
                ? Level3OriginalType.Spiker
                : Level3OriginalType.Tanker;
        }

        return SelectRandomStartFallback(eligible, randomInclusive);
    }

    private static Level3OriginalType? SelectRandomStartFallback(
        IReadOnlyList<Level3OriginalType> eligible,
        Func<int, int, int> randomInclusive)
    {
        // ROM starts at a random non-Flipper type, then scans the four non-Flipper species once.
        int firstRomType = randomInclusive(0, 3) + 1;
        for (int offset = 0; offset < 4; offset++)
        {
            int romType = (firstRomType - 1 + offset) % 4 + 1;
            Level3OriginalType? candidate = romType switch
            {
                2 => Level3OriginalType.Tanker,
                3 => Level3OriginalType.Spiker,
                _ => null
            };
            if (candidate is Level3OriginalType selected && eligible.Contains(selected))
                return selected;
        }

        return null;
    }

    private static void Validate(Level7AdmissionSnapshot snapshot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ReleasedCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.PendingShotCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ConvertedTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveOtherEnemies);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.LatentTankerCargoFlippers);
        if (snapshot.PendingSegmentSpikeRomComparisonValue is double comparisonValue &&
            !double.IsFinite(comparisonValue))
            throw new ArgumentOutOfRangeException(nameof(snapshot));
    }
}
