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
    public FlipperConfiguration Flipper { get; init; } = new(false, 2, 8, 21, 24);
    public bool FlipperCanFlip => Flipper.CanFlip;
    public FlipperPivotGeometry FlipperPivotMode { get; init; } = FlipperPivotGeometry.LegacyLaneTurn;
    public float? StartingSpikeHeight { get; init; }

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

    // REFERENCE VERIFIED: source Level 4 board, supply, enemy eligibility, and starting Spikes.
    // Natural campaign progression and --level4-test share this accepted configuration.
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
}
