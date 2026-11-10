namespace Tempest.Game.Levels;

public readonly record struct Level5AdmissionSnapshot(
    int OrdinaryFlippers,
    int ReleasedCargoFlippers,
    int PendingShotCargoFlippers,
    int OrdinaryTankers,
    int ConvertedTankers,
    int ActiveSpikers,
    int ActiveOtherEnemies,
    int LatentTankerCargoFlippers,
    float? ShortestSpikeHeight)
{
    public int ActiveFlippers =>
        OrdinaryFlippers + ReleasedCargoFlippers + PendingShotCargoFlippers;

    public int ActiveTankers => OrdinaryTankers + ConvertedTankers;

    public int ActiveEnemyCount =>
        ActiveFlippers + ActiveTankers + ActiveSpikers + ActiveOtherEnemies;
}

public static class Level5AdmissionPolicy
{
    public const int PhysicalEnemySlotCount = 7;
    public const int FlipperMinimum = 0;
    public const int FlipperMaximum = 5;
    public const int TankerMinimum = 1;
    public const int TankerMaximum = 1;
    public const int SpikerMinimum = 2;
    public const int SpikerMaximum = 3;

    public static bool ShouldConvertTerminalSpiker(
        LevelConfiguration configuration,
        int pendingGenericOriginals)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentOutOfRangeException.ThrowIfNegative(pendingGenericOriginals);
        return ReferenceEquals(configuration, LevelConfiguration.Level5Production) &&
            pendingGenericOriginals == 0;
    }

    // PROJECT TRANSLATION: reuse the established ROM-along/world-height mapping.
    // The C# Spike height is not a literal ROM coordinate.
    public static float ShortestSpikeTankerThreshold =>
        (0xF0 - 0xCC) * Level.Depth / (float)(0xF0 - 0x10);

    public static int GetOpenPhysicalSlots(Level5AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Max(0, PhysicalEnemySlotCount - snapshot.ActiveEnemyCount);
    }

    public static int GetFlipperAvailability(Level5AdmissionSnapshot snapshot)
    {
        Validate(snapshot);

        // ROM cargo reservation uses byte arithmetic; preserve its wrap before physical clamping.
        byte availability = unchecked((byte)(FlipperMaximum - snapshot.ActiveFlippers));
        availability = unchecked((byte)(
            availability - snapshot.LatentTankerCargoFlippers));
        return Math.Min(availability, GetOpenPhysicalSlots(snapshot));
    }

    public static int GetTankerAvailability(Level5AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        // Converted Tankers bypass ordinary selection and do not consume its one-Tanker allowance.
        return Math.Min(
            Math.Max(0, TankerMaximum - snapshot.OrdinaryTankers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static int GetSpikerAvailability(Level5AdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Min(
            Math.Max(0, SpikerMaximum - snapshot.ActiveSpikers),
            GetOpenPhysicalSlots(snapshot));
    }

    public static IReadOnlyList<Level3OriginalType> GetOrdinaryEligibleTypes(
        Level5AdmissionSnapshot snapshot)
    {
        int tankerAvailability = GetTankerAvailability(snapshot);
        int spikerAvailability = GetSpikerAvailability(snapshot);

        // The ROM checks unmet minima in descending species order: Spiker before Tanker.
        if (snapshot.ActiveSpikers < SpikerMinimum && spikerAvailability > 0)
            return [Level3OriginalType.Spiker];
        if (snapshot.ActiveTankers < TankerMinimum && tankerAvailability > 0)
            return [Level3OriginalType.Tanker];

        List<Level3OriginalType> eligible = [];
        if (tankerAvailability > 0)
            eligible.Add(Level3OriginalType.Tanker);
        if (spikerAvailability > 0)
            eligible.Add(Level3OriginalType.Spiker);
        return eligible;
    }

    public static Level3OriginalType? SelectOrdinaryType(Level5AdmissionSnapshot snapshot)
    {
        IReadOnlyList<Level3OriginalType> eligible = GetOrdinaryEligibleTypes(snapshot);
        if (eligible.Count == 0)
            return null;
        if (eligible.Count == 1)
            return eligible[0];

        // Approximate the ROM's $CC shortest-Spike test through the accepted C# height mapping.
        return snapshot.ShortestSpikeHeight is float height &&
            height < ShortestSpikeTankerThreshold
            ? Level3OriginalType.Tanker
            : Level3OriginalType.Spiker;
    }

    public static bool CanAdmitRetainedOriginal(
        Level5AdmissionSnapshot snapshot,
        Level3OriginalType retainedType,
        bool convertedTanker)
    {
        Validate(snapshot);
        if (convertedTanker)
            return retainedType == Level3OriginalType.Tanker &&
                GetOpenPhysicalSlots(snapshot) > 0;
        return retainedType switch
        {
            Level3OriginalType.Tanker => GetTankerAvailability(snapshot) > 0,
            Level3OriginalType.Spiker => GetSpikerAvailability(snapshot) > 0,
            _ => false
        };
    }

    private static void Validate(Level5AdmissionSnapshot snapshot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ReleasedCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.PendingShotCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ConvertedTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveSpikers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveOtherEnemies);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.LatentTankerCargoFlippers);
        if (snapshot.ShortestSpikeHeight is float height && !float.IsFinite(height))
            throw new ArgumentOutOfRangeException(nameof(snapshot));
    }
}
