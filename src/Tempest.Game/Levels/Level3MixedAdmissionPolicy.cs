namespace Tempest.Game.Levels;

public readonly record struct Level3MixedAdmissionSnapshot(
    int OrdinaryFlippers,
    int ReleasedCargoFlippers,
    int PendingShotCargoFlippers,
    int ActiveTankers,
    int ActiveOtherEnemies,
    int LatentTankerCargoFlippers)
{
    public int ActiveFlippers =>
        OrdinaryFlippers + ReleasedCargoFlippers + PendingShotCargoFlippers;

    public int ActiveEnemyCount =>
        ActiveFlippers + ActiveTankers + ActiveOtherEnemies;
}

public static class Level3MixedAdmissionPolicy
{
    public const int PhysicalEnemySlotCount = 7;
    public const int FlipperMaximum = 4;
    public const int FlipperMinimum = 1;
    public const int TankerMaximum = 1;
    public const int TankerMinimum = 1;

    public static int GetOpenPhysicalSlots(Level3MixedAdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        return Math.Max(0, PhysicalEnemySlotCount - snapshot.ActiveEnemyCount);
    }

    public static int GetFlipperAvailability(Level3MixedAdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        byte availability = (byte)Math.Max(0, FlipperMaximum - snapshot.ActiveFlippers);

        // This is equivalent to one 8-bit DEC per latent cargo Flipper: underflow can
        // re-enable Flippers, while the physical-slot clamp prevents an ordinary eighth entrant.
        availability = unchecked((byte)(
            availability - snapshot.LatentTankerCargoFlippers));

        return Math.Min(availability, GetOpenPhysicalSlots(snapshot));
    }

    public static int GetTankerAvailability(Level3MixedAdmissionSnapshot snapshot)
    {
        Validate(snapshot);
        int speciesAvailability = Math.Max(0, TankerMaximum - snapshot.ActiveTankers);
        return Math.Min(speciesAvailability, GetOpenPhysicalSlots(snapshot));
    }

    public static Level3OriginalType? SelectAdmissibleOriginalType(
        Level3MixedAdmissionSnapshot snapshot,
        Level3OriginalType retainedType = Level3OriginalType.Unselected)
    {
        int flipperAvailability = GetFlipperAvailability(snapshot);
        int tankerAvailability = GetTankerAvailability(snapshot);

        if (retainedType != Level3OriginalType.Unselected)
        {
            return retainedType switch
            {
                Level3OriginalType.Flipper when flipperAvailability > 0 =>
                    Level3OriginalType.Flipper,
                Level3OriginalType.Tanker when tankerAvailability > 0 &&
                    flipperAvailability > 0 => Level3OriginalType.Tanker,
                _ => null
            };
        }

        // Tankers precede Flippers when both ROM minima are unmet, and require at least
        // one selectable Flipper for their cargo. No random draw is needed for denial.
        if (snapshot.ActiveTankers < TankerMinimum &&
            tankerAvailability > 0 && flipperAvailability > 0)
            return Level3OriginalType.Tanker;
        if (snapshot.ActiveFlippers < FlipperMinimum && flipperAvailability > 0)
            return Level3OriginalType.Flipper;
        if (flipperAvailability > 0)
            return Level3OriginalType.Flipper;
        return null;
    }

    public static bool CanAdmitCargoReentry(Level3MixedAdmissionSnapshot snapshot) =>
        GetOpenPhysicalSlots(snapshot) > 0;

    private static void Validate(Level3MixedAdmissionSnapshot snapshot)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.OrdinaryFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ReleasedCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.PendingShotCargoFlippers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.ActiveOtherEnemies);
        ArgumentOutOfRangeException.ThrowIfNegative(snapshot.LatentTankerCargoFlippers);
    }
}
