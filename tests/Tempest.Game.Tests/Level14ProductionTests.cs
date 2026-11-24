using System.Numerics;
using System.Reflection;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level14ProductionTests
{
    [Fact]
    public void LevelFourteenUsesOrderedOpenShapeRowFifteen()
    {
        byte[] expectedX =
        [
            0x10, 0x1E, 0x21, 0x28, 0x3C, 0x55, 0x66, 0x73,
            0x8D, 0x9A, 0xAB, 0xC4, 0xD8, 0xDF, 0xE2, 0xF0
        ];
        byte[] expectedY =
        [
            0x7E, 0x6A, 0x51, 0x3A, 0x2C, 0x2C, 0x38, 0x4E,
            0x4E, 0x38, 0x2C, 0x2C, 0x3A, 0x51, 0x6A, 0x7E
        ];
        byte[] expectedAngles =
        [
            0x0E, 0x0C, 0x0D, 0x0E, 0x00, 0x02, 0x02, 0x00,
            0x0E, 0x0E, 0x00, 0x02, 0x03, 0x04, 0x02, 0x00
        ];

        LevelConfiguration configuration = LevelConfiguration.Level14Production;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(14);

        Assert.Equal(14, configuration.DisplayedLevel);
        Assert.Equal(13, configuration.ProgressionIndex);
        Assert.Equal(13, configuration.BoardIndex);
        Assert.Equal(13, LevelLoader.Level14RomEvidence.LookupIndex);
        Assert.Equal(15, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(14));
        Assert.Equal(15, metadata.TubeTableIndex);
        Assert.Equal(0x0F, LevelLoader.Level14RomEvidence.ShapeRemap);
        Assert.Equal(expectedX, LevelLoader.Level14RomEvidence.X);
        Assert.Equal(expectedY, LevelLoader.Level14RomEvidence.Y);
        Assert.Equal(expectedAngles, LevelLoader.Level14RomEvidence.Angles);
        Assert.Equal(expectedX, AtariTubeRecordCatalog.Level14Record15.RawXPoints);
        Assert.Equal(expectedY, AtariTubeRecordCatalog.Level14Record15.RawYPoints);
        Assert.Equal(15, AtariTubeRecordCatalog.Level14Record15.RecordIndex);
        Assert.Same(AtariTubeRecordCatalog.Level14Record15,
            AtariTubeRecordCatalog.ForRecordIndex(15));

        const float modelScale = 25f / 14f;
        for (int index = 0; index < expectedX.Length; index++)
        {
            Assert.Equal((expectedX[index] - 128) * modelScale,
                configuration.Definition.Points[index].X);
            Assert.Equal(-(expectedY[index] - 128) * modelScale,
                configuration.Definition.Points[index].Y);
        }
    }

    [Fact]
    public void ProductionTopologyAndSpinnerFollowTheActualOpenLevelFourteenInstance()
    {
        TempestGame game = new(startingProductionLevel: 14, random: new Random(1981));
        Level level = game.CurrentLevel;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(14);

        Assert.Same(LevelConfiguration.Level14Production, level.Configuration);
        Assert.Equal(metadata.IsOpen, !level.Definition.Continuous);
        AtariPresentationAdapter.ValidateTopology(metadata, level.Definition.Continuous);
        Assert.False(level.Continuous);
        Assert.Equal(15, level.PlayableLaneCount);
        Assert.Equal(Enumerable.Range(0, 15), level.Configuration.SpawnLaneSequence);
        Assert.False(level.TryGetAdjacentLane(0, -1, out _));
        Assert.False(level.TryGetAdjacentLane(14, 1, out _));
        Assert.Equal(-1, level.NextEdge(15));
        Assert.True(level.IsOpenEndpoint(0));
        Assert.True(level.IsOpenEndpoint(14));
        Assert.Throws<InvalidOperationException>(
            () => AtariPresentationAdapter.ValidateTopology(metadata, continuous: true));

        int lane = 7;
        Assert.True(level.TryGetAdjacentLane(
            lane, level.ClockwiseLaneIndexDelta, out int clockwiseLane));
        game.Player.ResetPosition(
            lane, edgePosition: level.ClockwiseLaneIndexDelta > 0 ? 1 : 0);
        game.RunOneStep(new PlayerInput(false, 1, false, false));
        Assert.Equal(clockwiseLane, game.Player.EdgeIndex);

        Assert.True(level.TryGetAdjacentLane(
            lane, -level.ClockwiseLaneIndexDelta, out int counterclockwiseLane));
        game.Player.ResetPosition(
            lane, edgePosition: -level.ClockwiseLaneIndexDelta > 0 ? 1 : 0);
        game.RunOneStep(new PlayerInput(false, -1, false, false));
        Assert.Equal(counterclockwiseLane, game.Player.EdgeIndex);
    }

    [Fact]
    public void LevelFourteenRawParametersAndPickerReachabilityStayDistinct()
    {
        Assert.Equal(0x1C, Level14WaveSelection.OrdinaryEnemySupply);
        Assert.Equal(0x20, Level14WaveSelection.HardDifficultyEnemySupply);
        Assert.Equal((6, 7),
            (Level14WaveSelection.MaximumActiveEnemies,
                Level14WaveSelection.MaximumPhysicalEnemySlots));
        Assert.Equal((0, 5),
            (Level14WaveSelection.MinimumFlippers, Level14WaveSelection.MaximumFlippers));
        Assert.Equal((1, 2),
            (Level14WaveSelection.MinimumTankers, Level14WaveSelection.MaximumTankers));
        Assert.Equal((2, 3),
            (Level14WaveSelection.MinimumSpikers, Level14WaveSelection.MaximumSpikers));
        Assert.Equal((1, 1),
            (Level14WaveSelection.MinimumFuseballs, Level14WaveSelection.MaximumFuseballs));
        Assert.Equal((0, 0),
            (Level14WaveSelection.MinimumPulsars, Level14WaveSelection.MaximumPulsars));
        Assert.Equal((byte)0, Level14WaveSelection.PulsarFireRaw);
        Assert.Equal((byte)0x07, Level14WaveSelection.FlipperMovementProgram);
        Assert.Equal((byte)0xA4, Level14WaveSelection.FlipperSpeedRaw);
        Assert.Equal((byte)0x29, Level14WaveSelection.ShotHoldoffRaw);
        Assert.Equal((byte)0x02, Level14WaveSelection.MaximumEnemyShotsRaw);
        Assert.Equal((byte)0xC0, Level14WaveSelection.EnemyShotSpeedRaw);
        Assert.Equal((byte)0, Level14WaveSelection.SpikerSpeedRaw);
        Assert.Equal((byte)0xDC, Level14WaveSelection.FuseballMovementProbabilityRaw);
        Assert.Equal(((byte)0x01, (byte)0x01),
            (Level14WaveSelection.TankerCargoTypeFirst,
                Level14WaveSelection.TankerCargoTypeSecond));
        Assert.Equal(((byte)0xA0, (byte)0xFE),
            (Level14WaveSelection.PulsarSpeedLowRaw,
                Level14WaveSelection.PulsarSpeedHighRaw));

        Assert.Equal(Level3OriginalType.Spiker,
            Level14WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 0, activeSpikers: 0, activeFuseballs: 0,
                activeEnemyUnits: 0, activeFlipperUnitsIncludingLatentCargo: 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level14WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 0, activeSpikers: 2, activeFuseballs: 1,
                activeEnemyUnits: 3, activeFlipperUnitsIncludingLatentCargo: 0));
        Assert.Equal(Level3OriginalType.Fuseball,
            Level14WaveSelection.SelectOriginalType((min, _) => min,
                activeTankers: 1, activeSpikers: 2, activeFuseballs: 0,
                activeEnemyUnits: 3, activeFlipperUnitsIncludingLatentCargo: 2));
        Assert.Null(Level14WaveSelection.SelectOriginalType((min, _) => min,
            activeTankers: 2, activeSpikers: 3, activeFuseballs: 1,
            activeEnemyUnits: 6, activeFlipperUnitsIncludingLatentCargo: 4));

        for (int choice = 0; choice <= 2; choice++)
        {
            Level3OriginalType? selected = Level14WaveSelection.SelectOriginalType(
                (_, _) => choice,
                activeTankers: 1, activeSpikers: 2, activeFuseballs: 1,
                activeEnemyUnits: 4, activeFlipperUnitsIncludingLatentCargo: 2);
            Assert.NotNull(selected);
            Assert.Contains(selected.Value,
                new[] { Level3OriginalType.Spiker, Level3OriginalType.Tanker });
            Assert.NotEqual(Level3OriginalType.Flipper, selected);
        }
    }

    [Fact]
    public void LevelFourteenUsesAtariModelFrameAcceptedCalibrationAndVectorHud()
    {
        TempestGame game = new(startingProductionLevel: 14, random: new Random(1981));
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(14);

        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(14));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(14));
        Assert.Equal(new AtariPresentationCalibration(1f, 1.1f, 200f, 1f),
            AtariPresentationCalibration.ForDisplayedLevel(14));
        Assert.Equal(new AtariPresentationCalibration(1f, 1.1f, 200f, .6f),
            AtariPresentationCalibration.Resolve(14, tubeDepthYGainOverride: .6f));

        AtariPresentationProjection projection = AtariPresentationAdapter.Project(
            displayedLevel: 14,
            x: game.CurrentLevel.Definition.Points[0].X,
            y: game.CurrentLevel.Definition.Points[0].Y,
            depth: 100,
            centerY: game.CurrentLevel.Definition.CenterY,
            yOffset: game.CurrentLevel.Definition.YOffset,
            position: LevelConfiguration.Level14Production.Position,
            levelDepth: LevelConfiguration.Level14Production.Depth,
            approachDepthOffset: LevelApproach.InitialDepthOffset,
            approachVerticalOffset: 0,
            k: Projection.K,
            zOffset: Projection.ZOffset,
            metadata: metadata);
        Assert.True(float.IsFinite(projection.Position.X));
        Assert.True(float.IsFinite(projection.Position.Y));
        Assert.Equal("14", new string(AtariHudVector.FormatFixedTwoPositionLevel(14)));

        VectorRenderer renderer = new(
            game, atariPresentationEnabled: true, calibrationGuidesEnabled: true);
        List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = [];
        renderer.DrawTube((start, end, color) => tubeLines.Add((start, end, color)));
        Assert.NotEmpty(tubeLines);
        List<(Vector2 Start, Vector2 End, Color Color)> depthAdjustedLines = [];
        new VectorRenderer(game, atariPresentationEnabled: true, tubeDepthYGain: .6f)
            .DrawTube((start, end, color) =>
                depthAdjustedLines.Add((start, end, color)));
        Assert.NotEqual(tubeLines, depthAdjustedLines);

        List<(Vector2 Start, Vector2 End, Color Color)> guides = [];
        renderer.DrawDevelopmentPortraitCalibrationGuides(
            (start, end, color) => guides.Add((start, end, color)));
        Assert.NotEmpty(guides);

        game.Score.Add(1234);
        game.Player.Lives = 4;
        IReadOnlyList<AtariHudLine> hud = AtariHudVector.Render(game);
        Assert.Contains(hud, line => line.Color == AtariHudVector.ScoreColor);
        Assert.Contains(hud, line => line.Color == AtariHudVector.HighScoreColor);
        Assert.Contains(hud, line => line.Color == AtariHudVector.LevelColor);
        Assert.Contains(hud, line => line.Color == AtariHudVector.ReserveLifeColor);
        Assert.Equal(3, AtariHudVector.GetReserveLifeCount(game.Player.Lives));
    }

    [Fact]
    public void LevelFourteenTankerCargoUsesTheExistingStraightProgramAtEveryReleasePosition()
    {
        LevelConfiguration configuration = LevelConfiguration.Level14Production;
        Level level = new(configuration);
        Tanker deepTanker = new(7, 0);
        Tanker nearRimTanker = new(7, 1);
        for (int tick = 0; tick < 207; tick++)
            nearRimTanker.UpdateForDevelopment();

        foreach (TankerReleaseKind releaseKind in Enum.GetValues<TankerReleaseKind>())
        {
            Assert.Equal(TankerCargoMovementProgram.Straight07,
                deepTanker.GetCargoMovementProgram(configuration, releaseKind));
            Assert.Equal(TankerCargoMovementProgram.Straight07,
                nearRimTanker.GetCargoMovementProgram(configuration, releaseKind));
        }

        TankerReleaseResult result = deepTanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);
        Assert.Equal(Tanker.CarriedFlipperCount, result.Cargo.Count);
        Assert.All(result.Cargo, cargo =>
        {
            Assert.True(level.IsPlayableLane(cargo.SourceLane));
            Assert.True(level.IsPlayableLane(cargo.DestinationLane));
        });

        Flipper reenteredCargo = Flipper.CreateFreshTankerCargo(
            edge: 7, cargoUnitId: 0, initialDirection: 1,
            tankerCargoMovementProgram: TankerCargoMovementProgram.Straight07);
        Assert.True(reenteredCargo.IsTankerCargo);
        Assert.Equal(TankerCargoMovementProgram.Straight07,
            reenteredCargo.CargoMovementProgram);
    }

    [Fact]
    public void LevelFourteenTranslatesSpikeAlongCoordinateAndUsesRecoveredShotLimits()
    {
        LevelConfiguration configuration = LevelConfiguration.Level14Production;

        Assert.Equal((byte)0xA0, Level14WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal((0xF0 - 0xA0) * Level.Depth / 224f,
            configuration.StartingSpikeHeight);
        Assert.Equal(41, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(2, configuration.MaximumActiveEnemyShots);
        Assert.Equal((int)Level14WaveSelection.MaximumEnemyShotsRaw,
            configuration.MaximumActiveEnemyShots);
        Assert.Equal(15, new Level(configuration).Spikes.Count);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.False(configuration.AllowsOrdinaryFlipperSelection);
    }

    [Fact]
    public void LevelFourteenTankerFiringHonorsTheRecoveredTwoShotCapacity()
    {
        TempestGame game = new(
            startingProductionLevel: 14, random: new AlwaysSuccessfulShotRandom());
        List<EnemyBullet> bullets = GetPrivateList<EnemyBullet>(game, "enemyBullets");
        List<Tanker> tankers = GetPrivateList<Tanker>(game, "tankers");
        bullets.Add(new EnemyBullet(7, 10));
        bullets.Add(new EnemyBullet(8, 10));
        tankers.Add(new Tanker(7, 0, bulletAmmunition: 2, bulletTimer: 0));

        typeof(TempestGame).GetMethod("UpdateTankers", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null);

        Assert.Equal(2, bullets.Count);
        Assert.Equal(2, tankers[0].BulletAmmunition);
    }

    [Fact]
    public void LevelFourteenSpikerFiringHonorsTheRecoveredTwoShotCapacity()
    {
        TempestGame game = new(
            startingProductionLevel: 14, random: new AlwaysSuccessfulShotRandom());
        List<EnemyBullet> bullets = GetPrivateList<EnemyBullet>(game, "enemyBullets");
        List<Spiker> spikers = GetPrivateList<Spiker>(game, "spikers");
        bullets.Add(new EnemyBullet(7, 10));
        bullets.Add(new EnemyBullet(8, 10));
        spikers.Add(new Spiker(7, speed: 4, originalSlotId: 10));

        MethodInfo updateSpikers = typeof(TempestGame).GetMethod(
            "UpdateSpikers", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("TempestGame.UpdateSpikers is missing.");
        for (int tick = 0; tick < 40; tick++)
            updateSpikers.Invoke(game, null);

        Assert.Equal(2, bullets.Count);
    }

    [Fact]
    public void LevelFourteenProductionSoakConservesReachableSpeciesAndIdentityDeterministically()
    {
        const int tickBound = 1200;
        TempestGame first = new(
            startingProductionLevel: 14, random: new Random(1981), startingLives: 99);
        TempestGame second = new(
            startingProductionLevel: 14, random: new Random(1981), startingLives: 99);
        RunSoak(first, tickBound);
        RunSoak(second, tickBound);

        Level3WaveLedger firstLedger = first.CurrentLevel.Level3Ledger!;
        Level3WaveLedger secondLedger = second.CurrentLevel.Level3Ledger!;
        Assert.Equal(first.CurrentLevel.EnemiesSpawned, second.CurrentLevel.EnemiesSpawned);
        Assert.Equal(first.CurrentLevel.EnemiesDestroyed, second.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(first.CurrentLevel.EnemiesRimCleared, second.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(firstLedger.UnresolvedOriginalIds.Order(),
            secondLedger.UnresolvedOriginalIds.Order());
        Assert.Equal(
            Enumerable.Range(0, firstLedger.OriginalCount)
                .Select(firstLedger.GetOriginalState),
            Enumerable.Range(0, secondLedger.OriginalCount)
                .Select(secondLedger.GetOriginalState));
        Assert.Equal(
            Enumerable.Range(0, firstLedger.OriginalCount)
                .Select(firstLedger.GetOriginalType),
            Enumerable.Range(0, secondLedger.OriginalCount)
                .Select(secondLedger.GetOriginalType));
    }

    [Fact]
    public void LevelFourteenSuperZapperAndDeathReconstructionPreserveLedgerIdentities()
    {
        TempestGame game = new(
            startingProductionLevel: 14, random: new Random(1981), startingLives: 99);
        for (int tick = 0; tick < 1000 &&
             (game.State != LevelState.Playing || game.Tankers.Count == 0); tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.NotEmpty(game.Tankers);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int chargesBeforeZap = game.SuperZapperChargesRemaining;
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(chargesBeforeZap - 1, game.SuperZapperChargesRemaining);
        for (int tick = 0; tick < 16; tick++)
            game.RunOneStep();
        ledger.ValidateState();

        typeof(TempestGame).GetMethod("KillPlayer", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Same(LevelConfiguration.Level14Production,
            game.CurrentLevel.Configuration);
        Assert.Equal(Level14WaveSelection.OrdinaryEnemySupply, ledger.OriginalCount);
        ledger.ValidateState();
        Assert.All(game.Flippers, flipper => Assert.True(flipper.IsTankerCargo));
        Assert.Empty(game.Pulsars);
    }

    private static void RunSoak(TempestGame game, int tickBound)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        int initialSpawned = level.EnemiesSpawned;
        bool observedEnemyLifecycle = false;
        HashSet<Level3OriginalType> selectedTypes = [];

        Assert.Same(LevelConfiguration.Level14Production, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level14WaveSelection.OrdinaryEnemySupply, ledger.OriginalCount);

        for (int tick = 0; tick < tickBound; tick++)
        {
            game.RunOneStep();

            Assert.Same(LevelConfiguration.Level14Production,
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
            for (int id = 0; id < ledger.OriginalCount; id++)
            {
                Level3OriginalType type = ledger.GetOriginalType(id);
                if (type != Level3OriginalType.Unselected)
                    selectedTypes.Add(type);
            }

            observedEnemyLifecycle |= game.Eggs.Any(egg => egg.State != EggState.Roaming) ||
                game.Flippers.Count != 0 || game.Tankers.Count != 0 ||
                game.Spikers.Count != 0 || game.Fuseballs.Count != 0;
        }

        Assert.True(level.EnemiesSpawned > initialSpawned,
            $"No Level 14 production admission occurred within {tickBound} ticks.");
        Assert.True(observedEnemyLifecycle,
            $"No Level 14 enemy lifecycle advanced within {tickBound} ticks.");
        Assert.Contains(Level3OriginalType.Tanker, selectedTypes);
        Assert.Contains(Level3OriginalType.Spiker, selectedTypes);
        Assert.Contains(Level3OriginalType.Fuseball, selectedTypes);
        Assert.DoesNotContain(Level3OriginalType.Flipper, selectedTypes);
        Assert.Empty(game.Pulsars);
    }

    private static List<T> GetPrivateList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Missing TempestGame field {fieldName}.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException($"TempestGame field {fieldName} is null."));
    }

    private sealed class AlwaysSuccessfulShotRandom : Random
    {
        public override int Next(int minValue, int maxValue) => maxValue - 1;
    }
}
