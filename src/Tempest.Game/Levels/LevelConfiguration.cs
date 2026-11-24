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
    // ROM Level 9 $53, translated using post-move live C# Spike and Flipper heights.
    Level9SpikeSensitive,
    Level7ArcadeObserved,
    Level8ConstantFlip,
    Level11Repeating24,
    // ROM Level 12 $19: flip constantly, moving on ticks between flips.
    Level12ConstantFlip
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
    // Gameplay parameter rows are independent of displayed identity and tube-table selection.
    public int? EffectiveGameplayParameterLevelOverride { get; init; }
    public int EffectiveGameplayParameterLevel =>
        EffectiveGameplayParameterLevelOverride ?? DisplayedLevel;
    // Medium/default ROM values for ordinary effective gameplay Levels 1-6.
    public int FlipperShotHoldoffTicks => EffectiveGameplayParameterLevel switch
    {
        1 => 80,
        2 => 77,
        3 => 74,
        4 => 71,
        5 => 68,
        6 => 65,
        7 or 10 => 62,
        // The Level 8 ROM holdoff is not translated directly to C# ticks; retain the
        // established C# cadence until an independently validated conversion exists.
        8 => 62,
        // PROJECT APPROXIMATION: the ROM's 56 holdoff parameter is used as the C# cadence.
        9 => 56,
        11 => 56,
        // PROJECT APPROXIMATION: the ROM's 47 holdoff parameter is used as C# ticks.
        12 => 47,
        // PROJECT APPROXIMATION: the ROM's 44 holdoff parameter is used as C# ticks.
        13 => 44,
        _ => throw new InvalidOperationException(
            $"No arcade Flipper shot holdoff is defined for effective gameplay Level {EffectiveGameplayParameterLevel}.")
    };
    public bool UsesLevel7SpikerFiringPolicy =>
        UsesLevel7RomWavePolicy;
    // PROJECT TRANSLATION / APPROXIMATION: reuse the existing Level 7 C# firing cadence;
    // the ROM's raw shot_holdoff value has no established conversion to simulation ticks.
    public int Level7SpikerShotHoldoffTicks => FlipperShotHoldoffTicks;
    public bool UsesLevel9SpikerFiringPolicy =>
        UsesLevel9RomWavePolicy;
    public int Level9SpikerShotHoldoffTicks => FlipperShotHoldoffTicks;
    public int MaximumActiveEnemyShots => EffectiveGameplayParameterLevel switch
    {
        1 or 2 or 3 => 2,
        4 => 3,
        5 => 4,
        6 => 3,
        7 or 10 => 3,
        8 => 4,
        9 => 4,
        11 => 4,
        12 => 2,
        13 => 2,
        _ => throw new InvalidOperationException(
            $"No arcade enemy-shot capacity is defined for effective gameplay Level {EffectiveGameplayParameterLevel}.")
    };
    public FlipperConfiguration Flipper { get; init; } = new(false, 2, 8, 21, 24);
    public bool FlipperCanFlip => Flipper.CanFlip;
    public FlipperPivotGeometry FlipperPivotMode { get; init; } = FlipperPivotGeometry.LegacyLaneTurn;
    public BelowRimFlipperProgram BelowRimFlipperProgram { get; init; } =
        global::Tempest.Game.Levels.BelowRimFlipperProgram.Existing;
    public bool UsesLevel7RomWavePolicy =>
        ReferenceEquals(this, Level7Production) ||
        ReferenceEquals(this, Level10Production);
    public bool UsesLevel8RomWavePolicy { get; init; }
    public bool UsesLevel9RomWavePolicy { get; init; }
    public bool UsesLevel11RomWavePolicy { get; init; }
    public bool UsesLevel12RomWavePolicy { get; init; }
    public bool UsesLevel13RomWavePolicy { get; init; }
    // DEVELOPMENT ONLY: the preserved Level 15 candidate may retain the separately
    // recovered Level 11 parameter baseline; genuine Level 11 uses UsesLevel11RomWavePolicy.
    public bool UsesLevel11ParameterDevelopmentBaseline { get; init; }
    // Canonical ROM-policy levels share generic reconstruction; copied configurations stay isolated.
    public bool UsesGenericRomDeathReconstruction =>
        UsesLevel7RomWavePolicy || UsesLevel8RomWavePolicy ||
        UsesLevel9RomWavePolicy || UsesLevel11RomWavePolicy ||
        UsesLevel12RomWavePolicy || UsesLevel13RomWavePolicy ||
        UsesLevel11ParameterDevelopmentBaseline ||
        ReferenceEquals(this, Level5Production) ||
        ReferenceEquals(this, Level6Production);
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
    public const float Level9StartingSpikeHeight = 48f * Level.Depth / 224f;
    public const float Level11StartingSpikeHeight =
        (0xF0 - Level11WaveSelection.StartingSpikeHeightRaw) * Level.Depth / 224f;
    public const float Level12StartingSpikeHeight =
        (0xF0 - Level12WaveSelection.StartingSpikeHeightRaw) * Level.Depth / 224f;
    public const float Level13StartingSpikeHeight =
        (0xF0 - Level13WaveSelection.StartingSpikeHeightRaw) * Level.Depth / 224f;
    // PROJECT CALIBRATION: manually accepted relative projectile lead; not a ROM speed measurement.
    public const float Level9ProductionSpikerSpeed = 2.0f;

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

    // ROM-backed Level 6 source data; canonical admission uses its Level-6-specific diagnostic policy.
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
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = 60,
        AvoidSpikesWaitThreshold = 30
    };

    // PROJECT TRANSLATION: preserve accepted Level-7-like gameplay, not verified arcade Level 10 gameplay.
    // The open tube has 15 lanes, so the Level 7 spawn sequence omits its unplayable lane 15.
    public static LevelConfiguration Level10Production { get; } =
        Level7Production with
        {
            ProgressionIndex = 9,
            DisplayedLevel = 10,
            BoardIndex = 9,
            Definition = LevelLoader.LoadLevel10OpenTube(),
            SpawnLaneSequence = Array.AsReadOnly(Enumerable.Range(0, 15).ToArray()),
            EffectiveGameplayParameterLevelOverride = 7
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

    // ROM row 13 and Level 9 composition are source-backed; visual scale, placement,
    // starting lane and simulation-tick mappings remain project translations.
    public static LevelConfiguration Level9Production { get; } = new(
        ProgressionIndex: 8,
        DisplayedLevel: 9,
        BoardIndex: 8,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel9OpenTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 27,
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
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level9SpikeSensitive,
        UsesLevel9RomWavePolicy = true,
        AllowsOrdinaryFlipperSelection = false,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = Level9StartingSpikeHeight
    };

    // ROM-backed shape row 8 and recovered Level 11 gameplay parameter policy.
    public static LevelConfiguration Level11Production { get; } = new(
        ProgressionIndex: 10,
        DisplayedLevel: 11,
        BoardIndex: 10,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel11OpenTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: Level11WaveSelection.OrdinaryEnemySupply,
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
        EffectiveGameplayParameterLevelOverride = 11,
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level11Repeating24,
        UsesLevel11RomWavePolicy = true,
        AllowsOrdinaryFlipperSelection = false,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = Level11StartingSpikeHeight
    };

    // ROM-backed shape row 12 and recovered Level 12 gameplay parameter policy.
    public static LevelConfiguration Level12Production { get; } = new(
        ProgressionIndex: 11,
        DisplayedLevel: 12,
        BoardIndex: 11,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel12ClosedTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: Level12WaveSelection.OrdinaryEnemySupply,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        EffectiveGameplayParameterLevelOverride = 12,
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level12ConstantFlip,
        UsesLevel12RomWavePolicy = true,
        AllowsOrdinaryFlipperSelection = true,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = Level12StartingSpikeHeight
    };

    // ROM-backed shape row 14 and Level 13 wave parameters. The closed topology
    // is checked against both the production definition and ROM presentation metadata.
    public static LevelConfiguration Level13Production { get; } = new(
        ProgressionIndex: 12,
        DisplayedLevel: 13,
        BoardIndex: 12,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel13ClosedTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: Level13WaveSelection.OrdinaryEnemySupply,
        InitialActiveEggCount: 4,
        ReserveReleaseIntervalTicks: 50,
        MaximumActiveEggs: 4,
        MaximumApproachingEggs: 2,
        HatchSelectionIntervalTicks: 50,
        SpawnLaneSequence: Array.AsReadOnly(Enumerable.Range(0, 16).ToArray()),
        EggDirectionSequence: Array.AsReadOnly(new[] { 1, -1 }),
        ExitPolicy: LevelExitPolicy.Standard,
        NormalEnemyComposition: LevelEnemyComposition.FlippersTankersAndSpikers)
    {
        EffectiveGameplayParameterLevelOverride = 13,
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        // PROJECT TRANSLATION: Level 13's ROM program $53 reuses the already implemented
        // spike-sensitive route; it is not selected for ordinary entries because min=0.
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level9SpikeSensitive,
        UsesLevel13RomWavePolicy = true,
        AllowsOrdinaryFlipperSelection = false,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = Level13StartingSpikeHeight
    };

    // DEVELOPMENT ONLY: the manually identified Level 15 tube is direct-start only.
    // It does not inherit genuine Level 11 production identity or gameplay policy.
    public static LevelConfiguration Level15DirectStartCandidate { get; } = new(
        ProgressionIndex: 14,
        DisplayedLevel: 15,
        BoardIndex: 14,
        PaletteIndex: 0,
        Definition: LevelLoader.LoadLevel15OpenTube(),
        Depth: Level.Depth,
        Position: Level.Position,
        InitialPlayerLane: 7,
        InitialPlayerLanePosition: 0.5f,
        EnemySupply: 27,
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
        EffectiveGameplayParameterLevelOverride = 11,
        Flipper = new(true, 2, 9, 0, 23),
        FlipperPivotMode = FlipperPivotGeometry.EndpointHinge,
        BelowRimFlipperProgram = BelowRimFlipperProgram.Level11Repeating24,
        UsesLevel11ParameterDevelopmentBaseline = true,
        AllowsOrdinaryFlipperSelection = false,
        ProgrammedFlipperInitialDirection = 1,
        StartingSpikeHeight = 0xB0 * Level.Depth / 224f
    };
}
