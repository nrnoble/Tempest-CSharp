using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level13ProductionTests
{
    [Fact]
    public void LevelThirteenUsesGenuineClosedShapeRowFourteen()
    {
        LevelConfiguration configuration = LevelConfiguration.Level13Production;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(13);

        Assert.Equal(13, configuration.DisplayedLevel);
        Assert.Equal(12, configuration.ProgressionIndex);
        Assert.Equal(12, LevelLoader.Level13RomEvidence.LookupIndex);
        Assert.Equal(14, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(13));
        Assert.Equal(14, metadata.TubeTableIndex);
        Assert.Equal(0x18, metadata.ScaleRaw);
        Assert.Equal(0x20, metadata.CameraHeightRaw);
        Assert.Equal(0xA0, metadata.ScreenYOffsetLowRaw);
        Assert.Equal(0xFE, metadata.ScreenYOffsetHighRaw);
        Assert.Equal((short)-352, metadata.ScreenYOffsetSigned);
        Assert.False(metadata.IsOpen);
        Assert.Equal(0x00, LevelLoader.Level13RomEvidence.Open);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(Enumerable.Range(0, 16), configuration.SpawnLaneSequence);
        Assert.Equal(0, new Level(configuration).NextEdge(15));
    }

    [Fact]
    public void LevelThirteenOrderedGeometryAndAnglesMatchBoundedRomEvidence()
    {
        byte[] expectedX =
        [
            0xB0, 0x80, 0x50, 0x47, 0x18, 0x30, 0x18, 0x47,
            0x50, 0x80, 0xB0, 0xB9, 0xE8, 0xD4, 0xE8, 0xB9
        ];
        byte[] expectedY =
        [
            0xE6, 0xD0, 0xE6, 0xB9, 0xAE, 0x80, 0x52, 0x47,
            0x14, 0x30, 0x14, 0x47, 0x52, 0x80, 0xAE, 0xB9
        ];
        byte[] expectedAngles =
        [
            0x0A, 0x06, 0x0C, 0x08, 0x0E, 0x0A, 0x00, 0x0C,
            0x02, 0x0E, 0x04, 0x00, 0x06, 0x02, 0x08, 0x04
        ];

        Assert.Equal(expectedX, LevelLoader.Level13RomEvidence.X);
        Assert.Equal(expectedY, LevelLoader.Level13RomEvidence.Y);
        Assert.Equal(expectedAngles, LevelLoader.Level13RomEvidence.Angles);
        Assert.Equal(expectedX, AtariTubeRecordCatalog.Level13Record14.RawXPoints);
        Assert.Equal(expectedY, AtariTubeRecordCatalog.Level13Record14.RawYPoints);
        Assert.Equal(14, AtariTubeRecordCatalog.Level13Record14.RecordIndex);

        const float scale = 25f / 14f;
        LevelDefinition definition = LevelConfiguration.Level13Production.Definition;
        for (int index = 0; index < expectedX.Length; index++)
        {
            Assert.Equal((expectedX[index] - 128) * scale, definition.Points[index].X);
            Assert.Equal(-(expectedY[index] - 128) * scale, definition.Points[index].Y);
        }
    }

    [Fact]
    public void LevelThirteenWaveParametersKeepRawValuesSeparateFromReachability()
    {
        Assert.Equal(26, Level13WaveSelection.OrdinaryEnemySupply);
        Assert.Equal(29, Level13WaveSelection.HardDifficultyEnemySupply);
        Assert.Equal(6, Level13WaveSelection.MaximumActiveEnemies);
        Assert.Equal(7, Level13WaveSelection.MaximumPhysicalEnemySlots);
        Assert.Equal((1, 2),
            (Level13WaveSelection.MinimumTankers, Level13WaveSelection.MaximumTankers));
        Assert.Equal((2, 3),
            (Level13WaveSelection.MinimumSpikers, Level13WaveSelection.MaximumSpikers));
        Assert.Equal((0, 5),
            (Level13WaveSelection.MinimumFlippers, Level13WaveSelection.MaximumFlippers));
        Assert.Equal((0, 0),
            (Level13WaveSelection.MinimumPulsars, Level13WaveSelection.MaximumPulsars));
        Assert.Equal((1, 1),
            (Level13WaveSelection.MinimumFuseballs, Level13WaveSelection.MaximumFuseballs));
        Assert.Equal(0x00, Level13WaveSelection.PulsarFireRaw);
        Assert.Equal(0xA0, Level13WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal(0x53, Level13WaveSelection.FlipperMovementProgram);
        Assert.Equal(0xA8, Level13WaveSelection.FlipperSpeedRaw);
        Assert.Equal(0x00, Level13WaveSelection.SpikerSpeedRaw);
        Assert.Equal(((byte)0x01, (byte)0x01),
            (Level13WaveSelection.TankerCargoTypeFirst,
                Level13WaveSelection.TankerCargoTypeSecond));
        Assert.Equal(0x07, Level13WaveSelection.StraightTankerCargoProgram);
        Assert.Equal(0x20, Level13WaveSelection.DeepTankerCargoRomAlongThreshold);

        LevelConfiguration configuration = LevelConfiguration.Level13Production;
        Assert.True(configuration.UsesLevel13RomWavePolicy);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.False(configuration.AllowsOrdinaryFlipperSelection);
        Assert.Equal(13, configuration.EffectiveGameplayParameterLevel);
        Assert.Equal(44, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(2, configuration.MaximumActiveEnemyShots);
        Assert.Equal((0xF0 - 0xA0) * Level.Depth / 224f,
            configuration.StartingSpikeHeight);
        Assert.Equal(16, new Level(configuration).Spikes.Count);
    }

    [Fact]
    public void LevelThirteenPickerReachesTankerSpikerAndFuseballButNotFlipperOrPulsar()
    {
        Assert.Equal(Level3OriginalType.Spiker,
            Level13WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 0, activeSpikers: 0, activeFuseballs: 0,
                activeEnemyUnits: 0, activeFlipperUnitsIncludingLatentCargo: 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level13WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 0, activeSpikers: 2, activeFuseballs: 1,
                activeEnemyUnits: 3, activeFlipperUnitsIncludingLatentCargo: 0));
        Assert.Equal(Level3OriginalType.Fuseball,
            Level13WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 1, activeSpikers: 2, activeFuseballs: 0,
                activeEnemyUnits: 3, activeFlipperUnitsIncludingLatentCargo: 2));
        Assert.Null(Level13WaveSelection.SelectOriginalType((min, _) => min,
            activeTankers: 2, activeSpikers: 3, activeFuseballs: 1,
            activeEnemyUnits: 6, activeFlipperUnitsIncludingLatentCargo: 4));

        Level3OriginalType[] fallbackTypes = [Level3OriginalType.Tanker, Level3OriginalType.Spiker];
        for (int choice = 0; choice <= 2; choice++)
        {
            Level3OriginalType? selected = Level13WaveSelection.SelectOriginalType(
                (_, _) => choice,
                activeTankers: 1,
                activeSpikers: 2,
                activeFuseballs: 1,
                activeEnemyUnits: 4,
                activeFlipperUnitsIncludingLatentCargo: 2);

            Assert.NotNull(selected);
            Assert.Contains(selected.Value, fallbackTypes);
            Assert.NotEqual(Level3OriginalType.Flipper, selected);
        }
    }

    [Fact]
    public void LevelThirteenTankerCargoUsesExistingRomProgramsAndThreshold()
    {
        Level level = new(LevelConfiguration.Level13Production);
        Tanker tanker = new(0, 0);
        Tanker deepTanker = new(0, 1);
        Tanker nearRimTanker = new(0, 2);
        for (int tick = 0; tick < 207; tick++)
            nearRimTanker.UpdateForDevelopment();

        Assert.Equal(TankerCargoMovementProgram.Level9SpikeSensitive53,
            deepTanker.GetCargoMovementProgram(
                level.Configuration, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Level9Straight07,
            nearRimTanker.GetCargoMovementProgram(
                level.Configuration, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Level9Straight07,
            deepTanker.GetCargoMovementProgram(
                level.Configuration, TankerReleaseKind.Natural));

        TankerReleaseResult release = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);
        Assert.Equal(Tanker.CarriedFlipperCount, release.Cargo.Count);
        Assert.All(release.Cargo, cargo =>
        {
            Assert.True(level.IsPlayableLane(cargo.SourceLane));
            Assert.True(level.IsPlayableLane(cargo.DestinationLane));
        });
    }

    [Fact]
    public void LevelThirteenProductionMapsSpinnerDirectionAndUsesAtariPresentation()
    {
        TempestGame game = new(startingProductionLevel: 13, random: new Random(1981));
        Level level = game.CurrentLevel;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(13);

        Assert.Equal(-1, level.ClockwiseLaneIndexDelta);
        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(13));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(13));
        Assert.Equal(new AtariPresentationCalibration(1.2f, 1f, 0f, .3f),
            AtariPresentationCalibration.ForDisplayedLevel(13));
        AtariPresentationAdapter.ValidateTopology(metadata, level.Definition.Continuous);
        Assert.Throws<InvalidOperationException>(
            () => AtariPresentationAdapter.ValidateTopology(metadata, continuous: false));

        game.Player.ResetPosition(0, 0);
        game.RunOneStep(new PlayerInput(
            Fire: false,
            MouseDeltaX: 1,
            Left: false,
            Right: false));
        Assert.Equal(15, game.Player.EdgeIndex);
        Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));

        game.Player.ResetPosition(15, 0.99f);
        game.RunOneStep(new PlayerInput(
            Fire: false,
            MouseDeltaX: -1,
            Left: false,
            Right: false));
        Assert.Equal(0, game.Player.EdgeIndex);
        Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));

        VectorRenderer renderer = new(
            game, atariPresentationEnabled: true, calibrationGuidesEnabled: true);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
        (Vector2 Start, Vector2 End, Color Color) lastRail = lines[15];
        (Vector2 Start, Vector2 End, Color Color) firstRail = lines[0];
        Assert.Contains(lines.Skip(level.Definition.VertexCount),
            line => line.Start == lastRail.End && line.End == firstRail.End);

        List<(Vector2 Start, Vector2 End, Color Color)> guides = [];
        renderer.DrawDevelopmentPortraitCalibrationGuides(
            (start, end, color) => guides.Add((start, end, color)));
        Assert.NotEmpty(guides);
        Assert.Equal("13", new string(AtariHudVector.FormatFixedTwoPositionLevel(13)));
        game.Score.Add(1234);
        game.Player.Lives = 3;
        IReadOnlyList<AtariHudLine> hud = AtariHudVector.Render(game);
        Assert.Contains(hud,
            line => line.Color == AtariHudVector.LevelColor);
        Assert.Contains(hud,
            line => line.Color == AtariHudVector.ScoreColor);
        Assert.Contains(hud,
            line => line.Color == AtariHudVector.HighScoreColor);
        Assert.Contains(hud,
            line => line.Color == AtariHudVector.ReserveLifeColor);
        Assert.Equal(2, AtariHudVector.GetReserveLifeCount(game.Player.Lives));
        Assert.Equal(0x0400, LevelLoader.Level13RomEvidence.FlipperScale);

        AtariPresentationProjection approachProjection = AtariPresentationAdapter.Project(
            displayedLevel: 13,
            x: level.Definition.Points[3].X,
            y: level.Definition.Points[3].Y,
            depth: 100,
            centerY: level.Definition.CenterY,
            yOffset: level.Definition.YOffset,
            position: LevelConfiguration.Level13Production.Position,
            levelDepth: level.Configuration.Depth,
            approachDepthOffset: LevelApproach.InitialDepthOffset,
            approachVerticalOffset: 0,
            k: Projection.K,
            zOffset: Projection.ZOffset,
            metadata: metadata);
        Assert.True(float.IsFinite(approachProjection.Position.X));
        Assert.True(float.IsFinite(approachProjection.Position.Y));
    }

    [Fact]
    public void LevelTwelveProgressesIntoThirteenAndThirteenIsTerminal()
    {
        Assert.Same(LevelConfiguration.Level13Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level12Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level13Production));
        Assert.Same(LevelConfiguration.Level13Production,
            LevelProgression.GetLastProductionLevel());
        Assert.Null(LevelProgression.GetProductionLevel(14));
        Assert.Same(LevelConfiguration.Level15DirectStartCandidate,
            LevelProgression.GetDirectStartLevel(15));

        TempestGame levelTwelve = new(startingProductionLevel: 12, startingLives: 4);
        levelTwelve.Score.Add(1234);
        Assert.Equal(1234, levelTwelve.Score.Score);
        Assert.Equal(4, levelTwelve.Player.Lives);
        Assert.Equal(2, levelTwelve.SuperZapperChargesRemaining);
        Assert.True(levelTwelve.NaturalProgressionEnabled);

        TempestGame levelThirteen = new(startingProductionLevel: 13, startingLives: 4);
        Assert.Same(LevelConfiguration.Level13Production,
            levelThirteen.CurrentLevel.Configuration);
        Assert.Equal(13, levelThirteen.CurrentLevel.Configuration.DisplayedLevel);
    }

    [Fact]
    public void LevelThirteenProductionSimulationPreservesLedgerAndLanesForBoundedSoak()
    {
        const int tickBound = 1200;
        TempestGame game = new(
            startingProductionLevel: 13, random: new Random(1981), startingLives: 99);
        Level level = game.CurrentLevel;
        int initialSpawned = level.EnemiesSpawned;
        Level3WaveLedger ledger = level.Level3Ledger!;
        bool observedEnemyLifecycle = false;

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level13WaveSelection.OrdinaryEnemySupply, ledger.OriginalCount);

        for (int tick = 0; tick < tickBound; tick++)
        {
            game.RunOneStep();

            Assert.Same(LevelConfiguration.Level13Production,
                game.CurrentLevel.Configuration);
            int terminalOriginals = Enumerable.Range(0, ledger.OriginalCount)
                .Count(id => ledger.GetOriginalState(id) is
                    Level3OriginalState.Destroyed or
                    Level3OriginalState.Escaped or
                    Level3OriginalState.RimCleared);
            Assert.Equal(ledger.OriginalCount,
                ledger.UnresolvedOriginalIds.Count + terminalOriginals);
            Assert.InRange(level.RemainingEnemies, 0, level.EnemySupply);
            Assert.All(game.Eggs, egg => Assert.True(level.IsPlayableLane(egg.EdgeIndex)));
            Assert.All(game.Flippers, flipper =>
            {
                Assert.True(level.IsPlayableLane(flipper.EdgeIndex));
                Assert.True(flipper.IsTankerCargo);
            });
            Assert.All(game.Tankers, tanker => Assert.True(level.IsPlayableLane(tanker.EdgeIndex)));
            Assert.All(game.Spikers, spiker => Assert.True(level.IsPlayableLane(spiker.EdgeIndex)));
            Assert.All(game.Fuseballs, fuseball => Assert.True(level.IsPlayableLane(fuseball.EdgeIndex)));
            Assert.Empty(game.Pulsars);
            Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));

            observedEnemyLifecycle |= game.Eggs.Any(egg => egg.State != EggState.Roaming) ||
                game.Flippers.Count != 0 || game.Tankers.Count != 0 ||
                game.Spikers.Count != 0 || game.Fuseballs.Count != 0;
        }

        Assert.True(level.EnemiesSpawned > initialSpawned,
            $"No Level 13 production admission occurred within {tickBound} ticks.");
        Assert.True(observedEnemyLifecycle,
            $"No Level 13 enemy lifecycle advanced within {tickBound} ticks.");
    }
}
