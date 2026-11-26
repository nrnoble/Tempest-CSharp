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
public sealed class Level12ProductionTests
{
    [Fact]
    public void LevelTwelveUsesGenuineClosedShapeRow12()
    {
        LevelConfiguration configuration = LevelConfiguration.Level12Production;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(12);

        Assert.Equal(12, configuration.DisplayedLevel);
        Assert.Equal(11, configuration.ProgressionIndex);
        Assert.Equal(12, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(12));
        Assert.Equal(12, metadata.TubeTableIndex);
        Assert.Equal(0x18, metadata.ScaleRaw);
        Assert.Equal(0x20, metadata.CameraHeightRaw);
        Assert.Equal(0xA0, metadata.ScreenYOffsetLowRaw);
        Assert.Equal(0xFE, metadata.ScreenYOffsetHighRaw);
        Assert.Equal((short)-352, metadata.ScreenYOffsetSigned);
        Assert.False(metadata.IsOpen);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(Enumerable.Range(0, 16), configuration.SpawnLaneSequence);
        Assert.Equal(0, new Level(configuration).NextEdge(15));
    }

    [Fact]
    public void LevelTwelveOrderedGeometryMatchesRecoveredRomRow()
    {
        byte[] expectedX =
        [
            0xDA, 0xA4, 0x87, 0x80, 0x79, 0x5C, 0x26, 0x10,
            0x10, 0x20, 0x48, 0x80, 0xB8, 0xE0, 0xF0, 0xF0
        ];
        byte[] expectedY =
        [
            0xE4, 0xE8, 0xB7, 0x80, 0xB7, 0xE8, 0xE4, 0xB2,
            0x7A, 0x47, 0x20, 0x10, 0x20, 0x47, 0x7A, 0xB2
        ];
        byte[] expectedAngles =
        [
            0x08, 0x0B, 0x0C, 0x04, 0x05, 0x08, 0x0B, 0x0C,
            0x0D, 0x0E, 0x0F, 0x01, 0x02, 0x03, 0x04, 0x05
        ];

        Assert.Equal(expectedX, LevelLoader.Level12RomEvidence.X);
        Assert.Equal(expectedY, LevelLoader.Level12RomEvidence.Y);
        Assert.Equal(expectedAngles, LevelLoader.Level12RomEvidence.Angles);
    }

    [Fact]
    public void LevelTwelveWavePolicyAndRawParameterValuesMatchRecoveredRows()
    {
        Assert.Equal(24, Level12WaveSelection.OrdinaryEnemySupply);
        Assert.Equal(27, Level12WaveSelection.HardDifficultyEnemySupply);
        Assert.Equal(6, Level12WaveSelection.MaximumActiveEnemies);
        Assert.Equal(7, Level12WaveSelection.MaximumPhysicalEnemySlots);
        Assert.Equal(1, Level12WaveSelection.MinimumTankers);
        Assert.Equal(1, Level12WaveSelection.MaximumTankers);
        Assert.Equal(1, Level12WaveSelection.MinimumSpikers);
        Assert.Equal(2, Level12WaveSelection.MaximumSpikers);
        Assert.Equal(0, Level12WaveSelection.MinimumFlippers);
        Assert.Equal(5, Level12WaveSelection.MaximumFlippers);
        Assert.Equal(2, Level12WaveSelection.MinimumPulsars);
        Assert.Equal(2, Level12WaveSelection.MaximumPulsars);
        Assert.Equal(1, Level12WaveSelection.MinimumFuseballs);
        Assert.Equal(1, Level12WaveSelection.MaximumFuseballs);
        Assert.Equal(0xA8, Level12WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal(0x19, Level12WaveSelection.FlipperMovementProgram);
        Assert.Equal(0x07, Level12WaveSelection.StraightTankerCargoProgram);
        Assert.Equal(0x20, Level12WaveSelection.DeepTankerCargoRomAlongThreshold);
        Assert.Equal(47, LevelConfiguration.Level12Production.FlipperShotHoldoffTicks);
        Assert.Equal(2, LevelConfiguration.Level12Production.MaximumActiveEnemyShots);
    }

    [Fact]
    public void LevelTwelveProductionSelectionIsLimitedToEligibleTypes()
    {
        Level3OriginalType[] eligibleTypes =
        [
            Level3OriginalType.Flipper,
            Level3OriginalType.Tanker,
            Level3OriginalType.Spiker,
            Level3OriginalType.Fuseball
        ];
        for (int choice = 0; choice <= 3; choice++)
        {
            Level3OriginalType? selected = Level12WaveSelection.SelectOriginalType(
                (_, _) => choice,
                activeTankers: 1,
                activeSpikers: 1,
                activeFuseballs: 1,
                activeEnemyUnits: 3,
                activeFlipperUnitsIncludingLatentCargo: 0);

            Assert.NotNull(selected);
            Assert.Contains(selected.Value, eligibleTypes);
        }
    }

    [Fact]
    public void NaturalElevenToTwelveProgressionContinuesThroughSixteen()
    {
        Assert.Same(LevelConfiguration.Level12Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level11Production));
        Assert.Same(LevelConfiguration.Level13Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level12Production));
        Assert.Same(LevelConfiguration.Level14Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level13Production));
        Assert.Same(LevelConfiguration.Level15Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level14Production));
        Assert.Same(LevelConfiguration.Level16Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level15Production));
        Assert.Same(LevelConfiguration.Level17Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level16Production));
        Assert.Same(LevelConfiguration.Level18Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level17Production));
        Assert.Same(LevelConfiguration.Level19Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level18Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level19Production));
        Assert.Same(LevelConfiguration.Level19Production,
            LevelProgression.GetLastProductionLevel());

        TempestGame game = new(startingProductionLevel: 12, startingLives: 4);
        game.Score.Add(1234);
        Assert.Same(LevelConfiguration.Level12Production, game.CurrentLevel.Configuration);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void AtariPresentationHasValidLevelTwelveModelFrameAndAcceptedCalibration()
    {
        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(12));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(12));
        Assert.Equal(new AtariPresentationCalibration(1.3f, .8f, 0f),
            AtariPresentationCalibration.ForDisplayedLevel(12));
    }

    [Fact]
    public void ProductionAtariRendererValidatesProjectsAndPreservesClosedTopology()
    {
        TempestGame game = new(startingProductionLevel: 12, random: new Random(1981));
        Level level = game.CurrentLevel;
        LevelDefinition definition = level.Definition;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(12);
        Vector2[] pointsBefore = definition.Points.ToArray();
        int playerLaneBefore = game.Player.EdgeIndex;
        LevelState stateBefore = game.State;
        int enemyCountBefore = game.Eggs.Count + game.Flippers.Count +
            game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count;

        Assert.True(definition.Continuous);
        Assert.False(metadata.IsOpen);
        AtariPresentationAdapter.ValidateTopology(metadata, definition.Continuous);
        Assert.Throws<InvalidOperationException>(
            () => AtariPresentationAdapter.ValidateTopology(metadata, continuous: false));
        Assert.Equal((25f / 14f, -1), AtariPresentationAdapter.GetModelFrameMapping(12));

        VectorRenderer renderer = new(
            game,
            atariPresentationEnabled: true,
            calibrationGuidesEnabled: true);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
        Vector2 projectedShot = renderer.ProjectShot(new Shot { EdgeIndex = 15, Height = 100 });
        Assert.True(float.IsFinite(projectedShot.X));
        Assert.True(float.IsFinite(projectedShot.Y));

        foreach (int pointIndex in new[] { 0, 7, 15 })
        {
            Assert.True(float.IsFinite(lines[pointIndex].Start.X));
            Assert.True(float.IsFinite(lines[pointIndex].Start.Y));
            Assert.True(float.IsFinite(lines[pointIndex].End.X));
            Assert.True(float.IsFinite(lines[pointIndex].End.Y));
        }

        (Vector2 Start, Vector2 End, Color Color) lastRail = lines[15];
        (Vector2 Start, Vector2 End, Color Color) firstRail = lines[0];
        Assert.Contains(lines.Skip(definition.VertexCount),
            line => line.Start == lastRail.End &&
                line.End == firstRail.End &&
                line.Color == Color.Blue);

        List<(Vector2 Start, Vector2 End, Color Color)> guides = [];
        renderer.DrawDevelopmentPortraitCalibrationGuides(
            (start, end, color) => guides.Add((start, end, color)));
        Assert.NotEmpty(guides);

        VectorRenderer diagnosticRenderer = new(
            game,
            atariPresentationEnabled: true,
            tubeYGain: 1.2f,
            tubeScale: 0.8f,
            tubeVerticalOffset: 4f,
            calibrationGuidesEnabled: true);
        List<(Vector2 Start, Vector2 End, Color Color)> diagnosticLines = [];
        diagnosticRenderer.DrawTube(
            (start, end, color) => diagnosticLines.Add((start, end, color)));
        Assert.NotEqual(lines, diagnosticLines);

        Assert.True(level.Continuous);
        Assert.Equal(16, level.PlayableLaneCount);
        Assert.Equal(pointsBefore, definition.Points);
        Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
        Assert.Equal(stateBefore, game.State);
        Assert.Equal(enemyCountBefore, game.Eggs.Count + game.Flippers.Count +
            game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count);
        Assert.Equal("12", new string(AtariHudVector.FormatFixedTwoPositionLevel(
            game.CurrentLevel.Configuration.DisplayedLevel)));
        Assert.Contains(AtariHudVector.Render(game),
            line => line.Color == AtariHudVector.LevelColor);

        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(16));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(16));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomPresentationMetadataCatalog.ForDisplayedLevel(20));
    }

    [Fact]
    public void ProductionLevelTwelveSpinnerCrossesTheClosedRimInBothDirections()
    {
        TempestGame game = new(startingProductionLevel: 12, random: new Random(1981));
        Level level = game.CurrentLevel;

        game.Player.ResetPosition(15, 0.99f);
        float clockwiseMovement =
            (0.01f * level.CalculateLaneTransitionAngle(15, 0) / 180 + 0.001f);
        game.RunOneStep(new PlayerInput(
            Fire: false,
            MouseDeltaX: clockwiseMovement /
                (0.0125f * level.ClockwiseLaneIndexDelta),
            Left: false,
            Right: false));
        Assert.Equal(0, game.Player.EdgeIndex);
        Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));

        game.Player.ResetPosition(0, 0.01f);
        float counterClockwiseMovement =
            (0.01f * level.CalculateLaneTransitionAngle(0, 15) / 180 + 0.001f);
        game.RunOneStep(new PlayerInput(
            Fire: false,
            MouseDeltaX: -counterClockwiseMovement /
                (0.0125f * level.ClockwiseLaneIndexDelta),
            Left: false,
            Right: false));
        Assert.Equal(15, game.Player.EdgeIndex);
        Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));
    }

    [Fact]
    public void LevelTwelveProductionSimulationAdvancesForABoundedSoak()
    {
        const int tickBound = 1200;
        TempestGame game = new(
            startingProductionLevel: 12,
            random: new Random(1981),
            startingLives: 99);
        Level level = game.CurrentLevel;
        int initialSpawned = level.EnemiesSpawned;
        Level3WaveLedger ledger = level.Level3Ledger!;
        bool observedEnemyLifecycle = false;

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(24, ledger.OriginalCount);

        for (int tick = 0; tick < tickBound; tick++)
        {
            game.RunOneStep();

            Assert.Same(LevelConfiguration.Level12Production, game.CurrentLevel.Configuration);
            int terminalOriginals = Enumerable.Range(0, ledger.OriginalCount)
                .Count(id => ledger.GetOriginalState(id) is
                    Level3OriginalState.Destroyed or
                    Level3OriginalState.Escaped or
                    Level3OriginalState.RimCleared);
            Assert.Equal(ledger.OriginalCount,
                ledger.UnresolvedOriginalIds.Count + terminalOriginals);
            Assert.InRange(level.RemainingEnemies, 0, level.EnemySupply);
            Assert.All(game.Eggs, egg => Assert.True(level.IsPlayableLane(egg.EdgeIndex)));
            Assert.All(game.Flippers, enemy => Assert.True(level.IsPlayableLane(enemy.EdgeIndex)));
            Assert.All(game.Tankers, enemy => Assert.True(level.IsPlayableLane(enemy.EdgeIndex)));
            Assert.All(game.Spikers, enemy => Assert.True(level.IsPlayableLane(enemy.EdgeIndex)));
            Assert.All(game.Fuseballs, enemy => Assert.True(level.IsPlayableLane(enemy.EdgeIndex)));
            Assert.Empty(game.Pulsars);

            observedEnemyLifecycle |= game.Eggs.Any(egg => egg.State != EggState.Roaming) ||
                game.Flippers.Count != 0 || game.Tankers.Count != 0 ||
                game.Spikers.Count != 0 || game.Fuseballs.Count != 0;
        }

        Assert.True(level.EnemiesSpawned > initialSpawned,
            $"No Level 12 production admission occurred within {tickBound} ticks.");
        Assert.True(observedEnemyLifecycle,
            $"No Level 12 enemy lifecycle advanced within {tickBound} ticks.");
    }
}
