using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public readonly record struct Level6AdmissionSnapshot(
    int OrdinaryFlippers,
    int ReleasedCargoFlippers,
    int PendingShotCargoFlippers,
    int OrdinaryTankers,
    int ConvertedTankers,
    int ActiveSpikers,
    int ActiveOtherEnemies,
    int LatentTankerCargoFlippers,
    double PendingSegmentSpikeRomComparisonValue)
{
    public int ActiveFlippers =>
        OrdinaryFlippers + ReleasedCargoFlippers + PendingShotCargoFlippers;

    public int ActiveEnemyCount =>
        ActiveFlippers + OrdinaryTankers + ConvertedTankers +
        ActiveSpikers + ActiveOtherEnemies;
}

public static class Level6AdmissionPolicy
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
        return ReferenceEquals(configuration, LevelConfiguration.Level6Production) &&
            pendingGenericOriginals == 0;
    }

    public static double GetPendingSegmentSpikeRomComparisonValue(Level level, int pendingLane)
    {
        ArgumentNullException.ThrowIfNull(level);
        if (!level.IsPlayableLane(pendingLane))
            throw new ArgumentOutOfRangeException(nameof(pendingLane));

        Spike? pendingSegmentSpike = level.Spikes.SingleOrDefault(
            spike => spike.EdgeIndex == pendingLane);
        // ROM selection substitutes $ff for a zero spike_ht; C# removes depleted Spikes.
        return pendingSegmentSpike is null
            ? 0xFF
            : Tanker.RomAlongPositionForHeight(pendingSegmentSpike.Height);
    }

    public static int GetOpenPhysicalSlots(Level6AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Max(0, PhysicalEnemySlotCount - snapshot.ActiveEnemyCount);
    }

    public static int GetFlipperAvailability(Level6AdmissionSnapshot snapshot)
    {
        Validate(snapshot);

        // ROM cargo reservation uses byte arithmetic before physical-headroom clamping.
        byte availability = unchecked((byte)(FlipperMaximum - snapshot.ActiveFlippers));
        availability = unchecked((byte)(
            availability - snapshot.LatentTankerCargoFlippers));
        return Math.Min(availability, GetOpenPhysicalSlots(snapshot));
    }

    public static int GetTankerAvailability(Level6AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        if (GetFlipperAvailability(snapshot) == 0)
            return 0;

        return Math.Min(
            Math.Max(0, TankerMaximum - snapshot.OrdinaryTankers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static int GetSpikerAvailability(Level6AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Min(
            Math.Max(0, SpikerMaximum - snapshot.ActiveSpikers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static IReadOnlyList<Level3OriginalType> GetOrdinaryEligibleTypes(
        Level6AdmissionSnapshot snapshot)
    {
        int tankerAvailability = GetTankerAvailability(snapshot);
        int spikerAvailability = GetSpikerAvailability(snapshot);

        // ROM minimum checks prioritize Spikers before Tankers; zero-minimum Flippers
        // are never ordinary candidates, even when their availability is positive.
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

    public static Level3OriginalType? SelectOrdinaryType(Level6AdmissionSnapshot snapshot)
    {
        IReadOnlyList<Level3OriginalType> eligible = GetOrdinaryEligibleTypes(snapshot);
        if (eligible.Count == 0)
            return null;
        if (eligible.Count == 1)
            return eligible[0];

        return snapshot.PendingSegmentSpikeRomComparisonValue >=
                PendingSegmentSpikeRomThreshold
            ? Level3OriginalType.Spiker
            : Level3OriginalType.Tanker;
    }

    private static void Validate(Level6AdmissionSnapshot snapshot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ReleasedCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.PendingShotCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ConvertedTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveOtherEnemies);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.LatentTankerCargoFlippers);
        if (!double.IsFinite(snapshot.PendingSegmentSpikeRomComparisonValue))
            throw new ArgumentOutOfRangeException(nameof(snapshot));
    }
}
