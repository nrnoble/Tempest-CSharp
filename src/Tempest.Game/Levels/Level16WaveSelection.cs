using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public enum Level16Difficulty
{
    Easy,
    Default,
    Hard
}

public static class Level16WaveSelection
{
    public const int OrdinaryEnemySupply = 27;
    public const int EasyDifficultyEnemySupply = 27;
    public const int HardDifficultyEnemySupply = 30;

    // ROM max_enm is the last indexed physical record, so indices 0 through 6 exist.
    // PROJECT TRANSLATION: admission reserves this seven-slot envelope for actors and split cargo;
    // it is not asserted as a direct ROM actor-concurrency rule.
    public const int MaximumPhysicalEnemySlots = 7;

    public const int MinimumFlippers = 0;
    public const int MaximumFlippers = 5;
    public const int MinimumPulsars = 0;
    public const int MaximumPulsars = 0;
    public const int MinimumTankers = 1;
    public const int MaximumTankers = 2;
    public const int MinimumSpikers = 2;
    public const int MaximumSpikers = 3;
    public const int MinimumFuseballs = 1;
    public const int MaximumFuseballs = 1;

    public const byte StartingSpikeHeightRaw = 0xA0;
    public const byte FlipperMovementProgram = 0x24;
    public const byte FlipperSpeedRaw = 0xA0;
    public const byte SpikerSpeedRaw = 0x00;
    public const byte TankerCargoTypeFirst = 0x01;
    public const byte TankerCargoTypeSecond = 0x01;
    public const byte FuseballMovementProbabilityRaw = 0xC0;
    public const byte FuseballMovementFlagRaw = 0x00;
    public const byte ShotHoldoffRaw = 0x23;
    public const byte EnemyShotSpeedRaw = 0xC0;
    public const byte DefaultEnemyShotLastSlotIndex = 0x02;
    public const byte EasyEnemyShotLastSlotIndex = 0x01;
    public const byte HardEnemyShotLastSlotIndex = 0x03;
    public const int DefaultMaximumActiveEnemyShots = DefaultEnemyShotLastSlotIndex + 1;
    public const int EasyMaximumActiveEnemyShots = EasyEnemyShotLastSlotIndex + 1;
    public const int HardMaximumActiveEnemyShots = HardEnemyShotLastSlotIndex + 1;

    public static int EnemySupplyForDifficulty(Level16Difficulty difficulty) =>
        difficulty switch
        {
            Level16Difficulty.Easy => EasyDifficultyEnemySupply,
            Level16Difficulty.Default => OrdinaryEnemySupply,
            Level16Difficulty.Hard => HardDifficultyEnemySupply,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
        };

    public static int MaximumActiveEnemyShotsForDifficulty(Level16Difficulty difficulty) =>
        difficulty switch
        {
            Level16Difficulty.Easy => EasyMaximumActiveEnemyShots,
            Level16Difficulty.Default => DefaultMaximumActiveEnemyShots,
            Level16Difficulty.Hard => HardMaximumActiveEnemyShots,
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
        };

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

        List<Level3OriginalType> available = [];
        if (activeTankers < MaximumTankers &&
            activeFlipperUnitsIncludingLatentCargo + Tanker.CarriedFlipperCount <= MaximumFlippers &&
            CanAdmitOriginalType(Level3OriginalType.Tanker,
                activeEnemyUnits))
            available.Add(Level3OriginalType.Tanker);
        if (activeSpikers < MaximumSpikers &&
            CanAdmitOriginalType(Level3OriginalType.Spiker,
                activeEnemyUnits))
            available.Add(Level3OriginalType.Spiker);
        if (activeFuseballs < MaximumFuseballs &&
            CanAdmitOriginalType(Level3OriginalType.Fuseball,
                activeEnemyUnits))
            available.Add(Level3OriginalType.Fuseball);

        if (available.Count == 0)
            return null;

        List<Level3OriginalType> required = available.Where(type => type switch
        {
            Level3OriginalType.Tanker => activeTankers < MinimumTankers,
            Level3OriginalType.Spiker => activeSpikers < MinimumSpikers,
            Level3OriginalType.Fuseball => activeFuseballs < MinimumFuseballs,
            _ => false
        }).ToList();
        IReadOnlyList<Level3OriginalType> choices = required.Count > 0 ? required : available;
        int selected = randomInclusive(0, choices.Count - 1);
        if ((uint)selected >= (uint)choices.Count)
            throw new InvalidOperationException("The Level 16 selector received an out-of-range choice.");

        return choices[selected];
    }

    public static bool CanAdmitOriginalType(
        Level3OriginalType type,
        int activeEnemyUnits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);

        // Existing actors are already included in activeEnemyUnits. A Tanker occupies one
        // record now but needs room for two children after release; other entries need one.
        int additionalSlots = type == Level3OriginalType.Tanker ? 2 : 1;
        return activeEnemyUnits + additionalSlots <= MaximumPhysicalEnemySlots;
    }

    public static bool CanAdmitCargoFlipper(int activeEnemyUnits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        // The source Tanker has already been removed before its two cargo actors are added.
        return activeEnemyUnits + 1 <= MaximumPhysicalEnemySlots;
    }

    public static bool ShouldConvertTerminalSpiker(int pendingOriginalSupply)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pendingOriginalSupply);
        return pendingOriginalSupply == 0;
    }
}
