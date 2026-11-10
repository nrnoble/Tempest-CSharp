namespace Tempest.Game.Levels;

public enum LevelExitPolicy
{
    Standard,
    AvoidSpikes
}

public enum LevelEnemyComposition
{
    FlippersOnly,
    FlippersAndTankers,
    FlippersTankersAndSpikers
}

public enum FlipperPivotGeometry
{
    LegacyLaneTurn,
    EndpointHinge
}

public enum BelowRimFlipperProgram
{
    Existing,
    // ROM Level 5 $53, translated using the live C# Spike and Flipper heights.
    Level5SpikeSensitive,
    Level7ArcadeObserved,
    Level8ConstantFlip
}

// REFERENCE VERIFIED: Incitti lv=0/1 values; exact gameplay fidelity remains unverified.
public sealed record FlipperConfiguration(
    bool CanFlip,
    float ClimbSpeed,
    float TurnSpeedDegrees,
    int LanePauseTicks,
    int RimPauseTicks);

/// <summary>
/// Immutable wave data. A Level owns the mutable runtime state created from this configuration.
/// </summary>
public sealed record LevelConfiguration(
    int ProgressionIndex,
    int DisplayedLevel,
    int BoardIndex,
    int PaletteIndex,
    LevelDefinition Definition,
    float Depth,
    float Position,
    int InitialPlayerLane,
    float InitialPlayerLanePosition,
    int EnemySupply,
    int InitialActiveEggCount,
    int ReserveReleaseIntervalTicks,
    int MaximumActiveEggs,
    int MaximumApproachingEggs,
    int HatchSelectionIntervalTicks,
    IReadOnlyList<int> SpawnLaneSequence,
    IReadOnlyList<int> EggDirectionSequence,
    LevelExitPolicy ExitPolicy,
    LevelEnemyComposition NormalEnemyComposition)
{
    public int VertexCount => Definition.VertexCount;
    public int PlayableLaneCount => Definition.PlayableLaneCount;
    // Medium/default ROM values for ordinary displayed production Levels 1-6.
    public int FlipperShotHoldoffTicks => DisplayedLevel switch
    {
        1 => 80,
        2 => 77,
        3 => 74,
        4 => 71,
        5 => 68,
        6 => 65,
        7 => 62,
        // The Level 8 ROM holdoff is not translated directly to C# ticks; retain the
        // established C# cadence until an independently validated conversion exists.
        8 => 62,
        _ => throw new InvalidOperationException(
            $"No arcade Flipper shot holdoff is defined for displayed Level {DisplayedLevel}.")
    };
    public int MaximumActiveEnemyShots => DisplayedLevel switch
    {
        1 or 2 or 3 => 2,
        4 => 3,
        5 => 4,
        6 => 3,
        7 => 3,
        8 => 4,
        _ => throw new InvalidOperationException(
            $"No arcade enemy-shot capacity is defined for displayed Level {DisplayedLevel}.")
    };
    public FlipperConfiguration Flipper { get; init; } = new(false, 2, 8, 21, 24);
    public bool FlipperCanFlip => Flipper.CanFlip;
    public FlipperPivotGeometry FlipperPivotMode { get; init; } = FlipperPivotGeometry.LegacyLaneTurn;
    public BelowRimFlipperProgram BelowRimFlipperProgram { get; init; } =
        global::Tempest.Game.Levels.BelowRimFlipperProgram.Existing;
    public bool UsesLevel7RomWavePolicy { get; init; }
    public bool UsesLevel8RomWavePolicy { get; init; }
    public bool UsesGenericRomDeathReconstruction =>
        UsesLevel7RomWavePolicy || UsesLevel8RomWavePolicy;
    // The Level 5 ROM's zero Flipper minimum excludes ordinary bottom entries, not Tanker cargo.
    public bool AllowsOrdinaryFlipperSelection { get; init; } = true;
    // Imported ROM handedness is not mapped conclusively; +1 is a reversible C# project choice.
    public int ProgrammedFlipperInitialDirection { get; init; } = 1;
    public float? StartingSpikeHeight { get; init; }
    // The exit state transitions after the counter exceeds this value.
    public int AvoidSpikesWaitThreshold { get; init; } = 150;
    // Conservative C# height cutoff; the ROM's along coordinate does not map exactly to this scale.
    public const float Level7DeepCargoMaximumHeight = 370;
    public const float Level8StartingSpikeHeight = 40f * Level.Depth / 224f;

    public static LevelConfiguration Level1 { get; } = new(
        ProgressionIndex: 0,
        DisplayedLevel: 1,
        BoardIndex: 0,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel1(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 10,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(new[] { 12, 4, 9, 1, 14, 6, 10, 2, 15, 7 }),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersOnly)
    {
        Flipper = new(false, 2, 8, 21, 24),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge
    };

    public static LevelConfiguration Level2Square { get; } = new(
        ProgressionIndex: 1,
        DisplayedLevel: 2,
        BoardIndex: 1,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel2Square(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 12,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(new[] { 12, 4, 9, 1, 14, 6, 10, 2, 15, 7, 3, 11 }),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersOnly)
    {
        Flipper = new(true, 2, 8, 21, 24),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge
    };

    // DEVELOPMENT ONLY: this source-derived board configuration validates plus-board geometry;
    // its Flipper-only diagnostic setup is not the eventual production Level 3 composition.
    public static LevelConfiguration Level3Plus { get; } = new(
        ProgressionIndex: 2,
        DisplayedLevel: 3,
        BoardIndex: 2,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel3Plus(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 15,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(new[] { 12, 4, 9, 1, 14, 6, 10, 2, 15, 7, 3, 11, 0, 8, 5 }),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersOnly)
    {
        Flipper = new(true, 2, 8, 21, 24),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge
    };

    // Composition is REFERENCE VERIFIED; direct --level3-test access remains DEVELOPMENT ONLY.
    public static LevelConfiguration Level3Mixed { get; } = Level3Plus with
    {
        NormalEnemyComposition = LevelEnemyComposition.FlippersAndTankers
    };

    // REFERENCE VERIFIED: source Level 4 board, supply, and starting Spikes. Admission and
    // terminal conversion are accepted project translations documented in the behavior spec.
    // Natural campaign progression and --level4-test share this configuration.
    public static LevelConfiguration Level4Candidate { get; } = new(
        ProgressionIndex: 3,
        DisplayedLevel: 4,
        BoardIndex: 3,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel4Binoculars(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 17,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.AvoidSpikes,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        Flipper = new(true, 2, 8, 21, 24),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        StartingSpikeHeight = 60
    };

    // REFERENCE VERIFIED: Original Arcade Level 5 cross, supply, enemy eligibility, and Spikes.
    // Natural campaign progression reuses the accepted Level 4 production systems.
    public static LevelConfiguration Level5Production { get; } = new(
        ProgressionIndex: 4,
        DisplayedLevel: 5,
        BoardIndex: 4,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel5Cross(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 20,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.AvoidSpikes,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        Flipper = new(true, 2, 8, 21, 24),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level5SpikeSensitive,
        AllowsOrdinaryFlipperSelection = false,
        StartingSpikeHeight = 60
    };

    // REFERENCE VERIFIED: Original Arcade Level 6 source data; the shared picker remains accepted C# behavior.
    public static LevelConfiguration Level6Production { get; } = new(
        ProgressionIndex: 5,
        DisplayedLevel: 6,
        BoardIndex: 5,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel6Triangle(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 22,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.AvoidSpikes,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        Flipper = new(true, 2, 9, 20, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        StartingSpikeHeight = 60,
        AvoidSpikesWaitThreshold = 70
    };

    // ROM-backed supply/composition and $24 routing; direction and 60-Hz cadence remain
    // project approximations despite completed perceptual gameplay acceptance.
    public static LevelConfiguration Level7Production { get; } = new(
        ProgressionIndex: 6,
        DisplayedLevel: 7,
        BoardIndex: 6,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel7Clover(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 20,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.AvoidSpikes,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level7ArcadeObserved,
        UsesLevel7RomWavePolicy = true,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = 60,
        AvoidSpikesWaitThreshold = 30
    };

    // Rev 2 shape row 7 is open; the resulting geometry, placement, color, player lane,
    // and Spike presentation are manually accepted project translations.
    public static LevelConfiguration Level8Production { get; } = new(
        ProgressionIndex: 7,
        DisplayedLevel: 8,
        BoardIndex: 7,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel8OpenTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 24,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 15).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level8ConstantFlip,
        UsesLevel8RomWavePolicy = true,
        AllowsOrdinaryFlipperSelection = false,
        StartingSpikeHeight = Level8StartingSpikeHeight
    };
}
