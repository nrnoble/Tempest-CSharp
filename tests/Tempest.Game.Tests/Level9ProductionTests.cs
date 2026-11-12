using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level9ProductionTests
{
    private static readonly byte[] RawX =
    [
        0x10, 0x10, 0x30, 0x30, 0x50, 0x50, 0x70, 0x70,
        0x90, 0x90, 0xB0, 0xB0, 0xD0, 0xD0, 0xF0, 0xF0
    ];

    private static readonly byte[] RawY =
    [
        0x90, 0x70, 0x70, 0x50, 0x50, 0x30, 0x30, 0x10,
        0x10, 0x30, 0x30, 0x50, 0x50, 0x70, 0x70, 0x90
    ];

    private static readonly byte[] RawAngles =
    [
        0x0C, 0x00, 0x0C, 0x00, 0x0C, 0x00, 0x0C, 0x00,
        0x04, 0x00, 0x04, 0x00, 0x04, 0x00, 0x04, 0x00
    ];

    [Fact]
    public void ProductionDefinitionPreservesRev2ShapeThirteenAndProjectTransform()
    {
        LevelConfiguration configuration = LevelConfiguration.Level9Production;
        LevelDefinition definition = configuration.Definition;
        const float scale = 25f / 14f;

        Assert.Equal(8, configuration.BoardIndex);
        Assert.False(definition.Continuous);
        Assert.Equal(16, definition.VertexCount);
        Assert.Equal(15, definition.PlayableLaneCount);
        Assert.Equal(300, definition.CenterY);
        Assert.Equal(0, definition.YOffset);
        Assert.Equal(RawX, LevelLoader.Level9RomEvidence.X);
        Assert.Equal(RawY, LevelLoader.Level9RomEvidence.Y);
        Assert.Equal(RawAngles, LevelLoader.Level9RomEvidence.Angles);
        Assert.Equal(0x0D, LevelLoader.Level9RomEvidence.ShapeRemap);
        Assert.Equal((0x0C, 0xB0, 0x0140, 0xFF, 0x0250, 0x05),
            (LevelLoader.Level9RomEvidence.Scale,
             LevelLoader.Level9RomEvidence.Y3d,
             LevelLoader.Level9RomEvidence.Y2d,
             LevelLoader.Level9RomEvidence.Open,
             LevelLoader.Level9RomEvidence.FlipperScale,
             LevelLoader.Level9RomEvidence.TubeColorIndex));
        Assert.Equal((0x01, 0x40),
            (LevelLoader.Level9RomEvidence.Y2dHigh, LevelLoader.Level9RomEvidence.Y2dLow));
        Assert.Equal((0x04, 0x50),
            (LevelLoader.Level9RomEvidence.FlipperScaleHigh,
                LevelLoader.Level9RomEvidence.FlipperScaleLow));

        for (int vertex = 0; vertex < RawX.Length; vertex++)
        {
            Assert.Equal((RawX[vertex] - 128) * scale, definition.Points[vertex].X);
            Assert.Equal(-(RawY[vertex] - 128) * scale, definition.Points[vertex].Y);
        }

        Assert.Equal(-200, definition.Points[0].X);
        Assert.Equal(-28.57143f, definition.Points[0].Y, precision: 4);
        Assert.Equal(200, definition.Points[15].X);
        Assert.Equal(-28.57143f, definition.Points[15].Y, precision: 4);
    }

    [Fact]
    public void ProductionConfigurationInitializesLevelNineCompositionAndSpikes()
    {
        LevelConfiguration configuration = LevelConfiguration.Level9Production;
        Level level = new(configuration);

        Assert.Equal(8, configuration.ProgressionIndex);
        Assert.Equal(9, configuration.DisplayedLevel);
        Assert.Equal(27, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(6, Level9WaveSelection.MaximumActiveEnemies);
        Assert.Equal((0, 5), (Level9WaveSelection.MinimumFlippers,
            Level9WaveSelection.MaximumFlippers));
        Assert.Equal((1, 2), (Level9WaveSelection.MinimumTankers,
            Level9WaveSelection.MaximumTankers));
        Assert.Equal((2, 4), (Level9WaveSelection.MinimumSpikers,
            Level9WaveSelection.MaximumSpikers));
        Assert.Equal((0, 0), (Level9WaveSelection.MinimumPulsars,
            Level9WaveSelection.MaximumPulsars));
        Assert.Equal((0, 0), (Level9WaveSelection.MinimumFuseballs,
            Level9WaveSelection.MaximumFuseballs));
        Assert.False(configuration.AllowsOrdinaryFlipperSelection);
        Assert.True(configuration.UsesLevel9RomWavePolicy);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.Equal(BelowRimFlipperProgram.Level9SpikeSensitive,
            configuration.BelowRimFlipperProgram);
        Assert.Equal(56, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(4, configuration.MaximumActiveEnemyShots);
        Assert.Equal(2, Tanker.CarriedFlipperCount);
        Assert.Equal(15, level.Spikes.Count);
        Assert.Equal(85.7143f, LevelConfiguration.Level9StartingSpikeHeight, precision: 3);
        Assert.All(level.Spikes, spike =>
        {
            Assert.InRange(spike.EdgeIndex, 0, 14);
            Assert.Equal(LevelConfiguration.Level9StartingSpikeHeight, spike.Height);
        });
        Assert.Throws<ArgumentOutOfRangeException>(() => level.CreateSpike(15, 20));
    }

    [Fact]
    public void OpenTubeClampsPlayerReversesEnemiesAndDoesNotRenderClosingChord()
    {
        Level level = new(LevelConfiguration.Level9Production);
        Player player = new();
        player.ResetPosition(14, 0.5f);
        player.MoveAlongTube(100, level);
        Assert.Equal(14, player.EdgeIndex);
        Assert.Equal(1, player.EdgePosition);
        player.MoveAlongTube(-100, level);
        Assert.Equal(0, player.EdgeIndex);
        Assert.Equal(0, player.EdgePosition);
        Assert.False(level.IsPlayableLane(15));
        Assert.False(level.TryGetAdjacentLane(0, -1, out _));
        Assert.False(level.TryGetAdjacentLane(14, 1, out _));

        ClearSpikes(level);
        level.CreateSpike(0, 20);
        Flipper endpointFlipper = new(0, 18);
        endpointFlipper.Update(level, playerEdge: 7);
        Assert.Equal(FlipperState.Flipping, endpointFlipper.State);
        Assert.Equal(1, endpointFlipper.DestinationEdge);
        Assert.Equal(-1, endpointFlipper.Direction);

        TempestGame game = new(startingProductionLevel: 9);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        new VectorRenderer(game).DrawTube((start, end, color) =>
            lines.Add((start, end, color)));
        Assert.Equal(46, lines.Count);
        Assert.DoesNotContain(lines.Skip(16), line =>
            line.Start == lines[15].Start && line.End == lines[0].Start);
    }

    [Fact]
    public void LevelNinePickerEnforcesMinimaMaximaGateAndCargoReservations()
    {
        Func<int, int, int> chooseFirst = (minimum, _) => minimum;

        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 0, 0, 0, 0));
        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 0, 1, 1, 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 0, 2, 2, 0));
        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 1, 2, 4, 4));
        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 1, 3, 5, 4));
        Assert.Null(Level9WaveSelection.SelectOriginalType(chooseFirst, 1, 4, 5, 4));
        Assert.Null(Level9WaveSelection.SelectOriginalType(chooseFirst, 2, 4, 5, 4));
        Assert.Null(Level9WaveSelection.SelectOriginalType(chooseFirst, 1, 2, 6, 2));
        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType(chooseFirst, 1, 2, 5, 5));

        Assert.Equal(Level3OriginalType.Spiker,
            Level9WaveSelection.SelectOriginalType((_, _) => 0, 1, 2, 3, 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level9WaveSelection.SelectOriginalType((_, maximum) => maximum, 1, 2, 3, 0));
        Assert.True(Level9WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level9Production, 0, 0, 3));
        Assert.False(Level9WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level9Production, 1, 0, 0));
        Assert.False(Level9WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level9Production, 0, 2, 0));
        Assert.False(Level9WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level9Production, 0, 0, 4));
        Assert.False(Level9WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level8Production, 0, 0, 0));
    }

    [Theory]
    [InlineData(10, 20, false)]
    [InlineData(18, 20, true)]
    [InlineData(20, 20, true)]
    [InlineData(18, 40, false)]
    public void LevelNineSpikePredicateUsesPostMoveLiveHeightAndEquality(
        float initialHeight, float spikeHeight, bool startsPivot)
    {
        Level level = LevelForFlipper(spikeLane: 5, spikeHeight: spikeHeight);
        Flipper flipper = new(5, initialHeight);

        flipper.Update(level, playerEdge: 0);

        Assert.Equal(initialHeight + 2, flipper.Height);
        Assert.Equal(startsPivot, flipper.State == FlipperState.Flipping);
        Assert.Equal(startsPivot, flipper.ActiveTransition is not null);
    }

    [Fact]
    public void LevelNineSpikePredicateAllowsMissingAndDestroyedSpikesAndRechecksLiveShortening()
    {
        Level absentLevel = new(LevelConfiguration.Level9Production);
        ClearSpikes(absentLevel);
        Flipper absent = new(5, 0);
        absent.Update(absentLevel, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, absent.State);

        Level destroyedLevel = new(LevelConfiguration.Level9Production);
        ClearSpikes(destroyedLevel);
        destroyedLevel.CreateSpike(5, 20).Height = 0;
        Flipper destroyed = new(5, 0);
        destroyed.Update(destroyedLevel, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, destroyed.State);

        Level shortenedLevel = LevelForFlipper(5, 60);
        Flipper shortened = new(5, 38);
        shortened.Update(shortenedLevel, playerEdge: 0);
        Assert.Equal(FlipperState.Climbing, shortened.State);
        shortenedLevel.Spikes.Single().Height = 40;
        shortened.Update(shortenedLevel, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, shortened.State);
    }

    [Fact]
    public void LevelNineFlipperMovesDuringPivotAndRetestsDestinationAfterNextAlongStep()
    {
        Level level = LevelForFlipper(spikeLane: 5, spikeHeight: 20);
        level.CreateSpike(4, 300);
        Flipper flipper = new(5, 18);
        flipper.Update(level, playerEdge: 0);
        float heightAtPivotStart = flipper.Height;

        Assert.Equal(FlipperState.Flipping, flipper.State);
        while (flipper.State == FlipperState.Flipping)
        {
            float previousHeight = flipper.Height;
            flipper.Update(level, playerEdge: 0);
            Assert.True(flipper.Height > previousHeight);
        }

        Assert.Equal(4, flipper.EdgeIndex);
        float heightAfterPivot = flipper.Height;
        flipper.Update(level, playerEdge: 0);

        Assert.Equal(heightAfterPivot + 2, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.True(heightAfterPivot > heightAtPivotStart);
    }

    [Theory]
    [InlineData(0x1F, false)]
    [InlineData(0x20, true)]
    [InlineData(0x21, true)]
    public void LevelNineTankerCargoUsesTheRomAlongBoundary(byte rawAlong, bool usesSpikeProgram)
    {
        float height = Tanker.HeightForRomAlongPosition(rawAlong);
        TankerCargoMovementProgram expected = usesSpikeProgram
            ? TankerCargoMovementProgram.Level9SpikeSensitive53
            : TankerCargoMovementProgram.Level9Straight07;

        Assert.Equal(usesSpikeProgram, Tanker.IsLevel9DeepCargoPosition(height));
        Assert.Equal(expected, Tanker.GetLevel9CargoMovementProgram(
            height, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Level9Straight07,
            Tanker.GetLevel9CargoMovementProgram(
                height, TankerReleaseKind.Natural));
        Assert.Equal(rawAlong >= 0x20,
            Tanker.RomAlongPositionForHeight(height) >= 0x20);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14)]
    public void OpenEndpointTankerCargoAllocatesDistinctValidOpposingRoutes(int tankerLane)
    {
        Level level = new(LevelConfiguration.Level9Production);
        Tanker tanker = new(tankerLane, 42);

        TankerReleaseResult result = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);

        Assert.Equal(2, result.Cargo.Count);
        Assert.Equal(2, result.Cargo.Select(cargo => cargo.SourceLane).Distinct().Count());
        Assert.Equal(2, result.Cargo.Select(cargo => cargo.Direction).Distinct().Count());
        Assert.All(result.Cargo, cargo =>
        {
            Assert.InRange(cargo.SourceLane, 0, 14);
            Assert.InRange(cargo.DestinationLane, 0, 14);
            Assert.True(level.IsPlayableLane(cargo.DestinationLane));
        });
        Flipper[] children =
        [
            Flipper.CreateTankerCargo(level, result.Cargo[0], cargoUnitId: 100),
            Flipper.CreateTankerCargo(level, result.Cargo[1], cargoUnitId: 101)
        ];
        Assert.Equal(2, children.Select(child => child.CargoUnitId).Distinct().Count());
        Assert.Equal(2, children.Select(child => child.Direction).Distinct().Count());
    }

    [Fact]
    public void LevelNineDeepCargoFlippersStartFreshAndUseTheirCurrentLaneSpike()
    {
        LevelConfiguration slowLevelConfiguration = LevelConfiguration.Level9Production with
        {
            Flipper = new(true, 0.1f, 180, 0, 23)
        };
        Level level = new(slowLevelConfiguration);
        ClearSpikes(level);
        level.CreateSpike(4, 100);
        Flipper child = Flipper.CreateTankerCargo(level,
            new TankerCargoSpawn(5, 4, 1, 18), cargoUnitId: 50,
            useLevel9SpikeSensitiveProgram: true);

        for (int tick = 0; tick < 10 && child.State == FlipperState.Flipping; tick++)
            child.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Climbing, child.State);
        Assert.Equal(4, child.EdgeIndex);
        child.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Climbing, child.State);

        Flipper fresh = Flipper.CreateFreshTankerCargo(
            edge: 4, cargoUnitId: 51, initialDirection: -1,
            useLevel9SpikeSensitiveProgram: true);
        fresh.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Climbing, fresh.State);
        Assert.Equal(slowLevelConfiguration.Flipper.ClimbSpeed, fresh.Height);
    }

    [Fact]
    public void LevelNineStraightCargoClimbsWithoutExecutingTheSpikeSensitiveRoute()
    {
        LevelConfiguration quickLevel = LevelConfiguration.Level9Production with
        {
            Flipper = new(true, 2, 180, 0, 23)
        };
        Level level = new(quickLevel);
        ClearSpikes(level);
        Flipper naturalCargo = Flipper.CreateTankerCargo(level,
            new TankerCargoSpawn(7, 8, -1, 18), cargoUnitId: 60,
            useLevel9StraightCargoProgram: true);

        for (int tick = 0; tick < 10 && naturalCargo.State == FlipperState.Flipping; tick++)
            naturalCargo.Update(level, playerEdge: 0);
        Assert.Equal(8, naturalCargo.EdgeIndex);
        Assert.Equal(FlipperState.Climbing, naturalCargo.State);
        float heightAfterReleasePivot = naturalCargo.Height;
        naturalCargo.Update(level, playerEdge: 0);

        Assert.Equal(heightAfterReleasePivot + quickLevel.Flipper.ClimbSpeed,
            naturalCargo.Height);
        Assert.Equal(8, naturalCargo.EdgeIndex);
        Assert.Equal(FlipperState.Climbing, naturalCargo.State);
    }

    [Fact]
    public void LevelNineFinalConversionKeepsOriginalIdentityAndAddsOnlyLatentCargo()
    {
        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        int originalCount = ledger.OriginalCount;

        ledger.ConvertActiveSpikerToTanker(0);

        Assert.Equal(originalCount, ledger.OriginalCount);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(0));
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(0).Count);
        Assert.All(ledger.GetCargoIdsForOriginal(0),
            cargoId => Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(cargoId)));
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void LevelNineDeathReconstructionDropsLatentCargoAndRebuildsReleasedCargoGenerically()
    {
        LevelConfiguration configuration = LevelConfiguration.Level9Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        Level level = new(configuration);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg latentCarrier = level.Eggs[0];
        Egg releasedCarrier = level.Eggs[1];
        Egg activeSpiker = level.Eggs[2];
        ledger.CommitOriginalAsTanker(latentCarrier.OriginalSlotId);
        ledger.CommitOriginalAsTanker(releasedCarrier.OriginalSlotId);
        int[] releasedCargo = ledger.MaterializeLiveCargo(releasedCarrier.OriginalSlotId).ToArray();
        ledger.MarkOriginalEscaped(releasedCarrier.OriginalSlotId);
        ledger.CommitOriginalAsSpiker(activeSpiker.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(6, ledger.OriginalCount);
        Assert.Equal(5, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.All(releasedCargo,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
        Assert.Equal(5, level.PendingGenericEnemyUnits);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);
        Assert.Equal(6, ledger.OriginalCount);
        Assert.Equal(5, level.PendingGenericEnemyUnits);
    }

    [Fact]
    public void LevelNineFinalConversionRemainsAvailableAfterReconstructedSupplyIsConsumed()
    {
        LevelConfiguration configuration = LevelConfiguration.Level9Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        Level level = new(configuration);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsTanker(0);
        int[] releasedCargo = ledger.MaterializeLiveCargo(0).ToArray();
        ledger.MarkOriginalEscaped(0);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.All(releasedCargo,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
        int reconstructedOriginalCount = ledger.OriginalCount;
        int finalSpikerId = ledger.UnresolvedOriginalIds.Last();
        foreach (int id in ledger.UnresolvedOriginalIds.Where(id => id != finalSpikerId).ToArray())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsSpiker(id);
            ledger.MarkOriginalDestroyed(id);
        }

        if (ledger.GetOriginalState(finalSpikerId) == Level3OriginalState.Reserve)
            ledger.MarkOriginalEgg(finalSpikerId);
        ledger.CommitOriginalAsSpiker(finalSpikerId);
        Assert.Equal(0, ledger.PendingGenericOriginalCount);

        ledger.ConvertActiveSpikerToTanker(finalSpikerId);

        Assert.Equal(reconstructedOriginalCount, ledger.OriginalCount);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(finalSpikerId));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(finalSpikerId));
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(finalSpikerId).Count);
        Assert.All(ledger.GetCargoIdsForOriginal(finalSpikerId),
            id => Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id)));
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void LevelNineProductionPickerKeepsOrdinaryFlippersAndAbsentSpeciesOut()
    {
        TempestGame game = new(startingProductionLevel: 9, random: new MinimumRandom());
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        for (int hatch = 0; hatch < 4; hatch++)
            game.RunOneStep();

        Assert.Equal(27, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.InRange(game.Spikers.Count, 2, 4);
        Assert.InRange(game.Tankers.Count, 1, 2);
        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.True(game.Tankers.Count + game.Spikers.Count + game.Flippers.Count <= 6);
    }

    [Fact]
    public void LevelNineSpikersFireRepeatedlyAndNewShotsAdvanceOnTheNextStep()
    {
        LevelConfiguration slowSupply = LevelConfiguration.Level9Production with
        {
            EnemySupply = 27,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1,
            MaximumApproachingEggs = 1,
            ReserveReleaseIntervalTicks = 1000
        };
        TempestGame game = new(slowSupply, random: new MinimumRandom());
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);

        game.RunOneStep();
        Spiker spiker = game.Spikers[0];
        game.RunOneStep();

        EnemyBullet firstShot = Assert.Single(game.EnemyBullets);
        Assert.Equal(56, spiker.ShotDelayTicks);
        float initialShotHeight = firstShot.Height;
        game.RunOneStep();
        Assert.Equal(initialShotHeight + 3, firstShot.Height);

        bool sawRetreat = false;
        bool sawRepeatedAscent = false;
        for (int tick = 0; tick < 1000 && !sawRepeatedAscent; tick++)
        {
            game.RunOneStep();
            sawRetreat |= spiker.State == SpikerState.Retreating;
            sawRepeatedAscent = sawRetreat &&
                spiker.State == SpikerState.Building &&
                spiker.Height > 0;
        }

        Assert.True(sawRepeatedAscent);
        Assert.Contains(spiker, game.Spikers);
    }

    [Fact]
    public void LevelNineTankerUsesTheSharedEnemyProjectilePath()
    {
        Tanker tanker = new(3, originalSlotId: 7,
            bulletAmmunition: 1, bulletTimer: 0);

        TankerUpdateResult result = tanker.UpdateForDevelopment(
            (_, maximum) => maximum, out EnemyBullet? bullet,
            LevelConfiguration.Level9Production.FlipperShotHoldoffTicks);

        Assert.Equal(TankerUpdateResult.Active, result);
        Assert.NotNull(bullet);
        Assert.Equal(3, bullet!.EdgeIndex);
        Assert.Equal(tanker.Height, bullet.Height);
        Assert.Equal(56, tanker.BulletTimer);
        Assert.Equal(4, LevelConfiguration.Level9Production.MaximumActiveEnemyShots);
    }

    [Fact]
    public void LevelNineSupportsNaturalProgressionDirectStartAndTerminalRestart()
    {
        TempestGame game = new(startingProductionLevel: 8, startingLives: 5);
        game.Score.Add(1234);
        game.Player.Lives = 3;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));

        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.Same(LevelConfiguration.Level9Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);

        TempestGame direct = new(startingProductionLevel: 9, startingLives: 10);
        Assert.Same(LevelConfiguration.Level9Production, direct.CurrentLevel.Configuration);
        Assert.Equal(7, direct.Player.EdgeIndex);
        Assert.True(direct.NaturalProgressionEnabled);
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level9Production));
    }

    [Fact]
    public void RecordingVisibleAndHeadlessPlaybackRestoreLevelNineWithoutSchemaChanges()
    {
        GameplayStartupConfiguration startup = new(9, 10, 1.0, 20261111);
        PlayerInputRecorder recorder = new(startup);
        recorder.CaptureTick(0, default);
        recorder.CaptureTick(1, new PlayerInput(false, 0.5f, false, false));

        string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "level9.json");
            GameplayRecordingFile.SaveNew(path, recorder);
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
            Assert.Equal(2, document.RootElement.GetProperty("Version").GetInt32());
            Assert.Equal(9, document.RootElement.GetProperty("StartingDisplayedLevel").GetInt32());
            Assert.False(document.RootElement.TryGetProperty("Level9DiagnosticMetadata", out _));
            Assert.False(document.RootElement.TryGetProperty("Speed", out _));

            GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
            GameplayPlaybackSession session = new(recording.StartupConfiguration, recording.Replay);
            Assert.Same(LevelConfiguration.Level9Production, session.Game.CurrentLevel.Configuration);
            VisiblePlaybackController visible = new(session);
            Assert.Same(LevelConfiguration.Level9Production,
                visible.Session.Game.CurrentLevel.Configuration);

            GameplayPlaybackRunResult result =
                new HeadlessGameplayPlaybackRunner(recording).RunToBoundary(1);
            Assert.Equal(9, result.StateSummary.StartingDisplayedLevel);
            Assert.Equal(1, result.StateSummary.NextRecordingFrame);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static Level LevelForFlipper(int spikeLane, float spikeHeight)
    {
        Level level = new(LevelConfiguration.Level9Production);
        ClearSpikes(level);
        level.CreateSpike(spikeLane, spikeHeight);
        return level;
    }

    private static void ClearSpikes(Level level)
    {
        foreach (Spike spike in level.Spikes.ToArray())
            level.RemoveSpike(spike);
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
