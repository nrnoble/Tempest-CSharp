using System.Numerics;
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
public sealed class Level8ProductionTests
{
    private static readonly byte[] Level8RawX =
    [
        0x10, 0x1E, 0x2C, 0x3A, 0x48, 0x56, 0x64, 0x70,
        0x90, 0x9E, 0xAC, 0xBA, 0xC8, 0xD6, 0xE4, 0xF0
    ];

    private static readonly byte[] Level8RawY =
    [
        0xF0, 0xD0, 0xB0, 0x90, 0x70, 0x50, 0x30, 0x10,
        0x10, 0x30, 0x50, 0x70, 0x90, 0xB0, 0xD0, 0xF0
    ];

    [Fact]
    public void ProductionDefinitionUsesRev2ShapeSevenAndNormalizedOpenGeometry()
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production;
        LevelDefinition definition = configuration.Definition;

        Assert.Equal(7, configuration.BoardIndex);
        Assert.False(definition.Continuous);
        Assert.Equal(16, definition.VertexCount);
        Assert.Equal(15, definition.PlayableLaneCount);
        Assert.Equal(300, definition.CenterY);
        Assert.Equal(0, definition.YOffset);
        const float scale = 25f / 14f;
        for (int index = 0; index < Level8RawX.Length; index++)
        {
            Assert.Equal((Level8RawX[index] - 128) * scale,
                definition.Points[index].X);
            Assert.Equal(-(Level8RawY[index] - 128) * scale,
                definition.Points[index].Y);
        }

        Assert.Equal(new Vector2(-200, -200), definition.Points[0]);
        Assert.Equal(new Vector2(200, -200), definition.Points[15]);
        Assert.True(definition.Points[0].Y < definition.Points[7].Y);
        Assert.True(definition.Points[15].Y < definition.Points[8].Y);

        for (int first = 0; first < Level8RawX.Length; first++)
        for (int second = first + 1; second < Level8RawX.Length; second++)
        {
            Vector2 originalFirst = new(
                (Level8RawX[first] - 128) * scale,
                (Level8RawY[first] - 128) * scale);
            Vector2 originalSecond = new(
                (Level8RawX[second] - 128) * scale,
                (Level8RawY[second] - 128) * scale);
            Assert.Equal(Vector2.Distance(originalFirst, originalSecond),
                Vector2.Distance(definition.Points[first], definition.Points[second]),
                precision: 4);
        }
    }

    [Fact]
    public void OpenTopologyExposesFifteenLanesAndNeverWrapsEitherEndpoint()
    {
        Level open = new(LevelConfiguration.Level8Production);

        Assert.All(Enumerable.Range(0, 15), lane => Assert.True(open.IsPlayableLane(lane)));
        Assert.False(open.IsPlayableLane(15));
        Assert.True(open.IsOpenEndpoint(0));
        Assert.True(open.IsOpenEndpoint(14));
        Assert.False(open.TryGetAdjacentLane(0, -1, out _));
        Assert.False(open.TryGetAdjacentLane(14, 1, out _));
        Assert.True(open.TryGetAdjacentLane(0, 1, out int lane1));
        Assert.Equal(1, lane1);
        Assert.True(open.TryGetAdjacentLane(14, -1, out int lane13));
        Assert.Equal(13, lane13);
        Assert.Equal(-1, open.NextEdge(15));
        Assert.Equal(-1, open.PreviousEdge(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => open.CreateSpike(15, 20));

        Level closed = new(LevelConfiguration.Level7Production);
        Assert.Equal(16, closed.PlayableLaneCount);
        Assert.True(closed.TryGetAdjacentLane(0, -1, out int wrappedPrevious));
        Assert.Equal(15, wrappedPrevious);
        Assert.Equal(0, closed.NextEdge(15));
        Assert.Equal(15, closed.PreviousEdge(0));
    }

    [Fact]
    public void BlasterClampsAtBothOpenEndpointsButStillWrapsOnClosedTube()
    {
        Level open = new(LevelConfiguration.Level8Production);
        Player player = new();

        player.ResetPosition(14, 0.5f);
        player.MoveAlongTube(100, open);
        Assert.Equal(14, player.EdgeIndex);
        Assert.Equal(1, player.EdgePosition);

        player.MoveAlongTube(-100, open);
        Assert.Equal(0, player.EdgeIndex);
        Assert.Equal(0, player.EdgePosition);

        Level closed = new(LevelConfiguration.Level7Production);
        player.ResetPosition(15, 1);
        player.MoveAlongTube(0.001f, closed);
        Assert.Equal(0, player.EdgeIndex);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void PositiveSpinnerRotationMovesClockwiseByTubeGeometry(int displayedLevel)
    {
        TempestGame game = CreatePlayerControlGame(displayedLevel);
        Vector2 start = GetBlasterGeometryPosition(game);

        game.RunOneStep(new PlayerInput(false, 1, false, false));

        Vector2 end = GetBlasterGeometryPosition(game);
        Assert.True(GetScreenClockwiseAngleDelta(start, end) > 0);
        if (displayedLevel == 8)
            Assert.True(end.X < start.X);
        else
            Assert.Equal(1, game.CurrentLevel.ClockwiseLaneIndexDelta);
    }

    [Fact]
    public void NegativeSpinnerRotationMovesCounterClockwiseAndVisuallyRightOnLevelEight()
    {
        TempestGame game = CreatePlayerControlGame(8);
        Vector2 start = GetBlasterGeometryPosition(game);

        game.RunOneStep(new PlayerInput(false, -1, false, false));

        Vector2 end = GetBlasterGeometryPosition(game);
        Assert.True(GetScreenClockwiseAngleDelta(start, end) < 0);
        Assert.True(end.X > start.X);
    }

    [Fact]
    public void SpinnerMovementClampsAtBothOpenEndpointsAndReversalMovesInward()
    {
        TempestGame game = CreatePlayerControlGame(8);

        MoveWithSpinnerUntilLane(game, spinnerDelta: 80, endpointLane: 0);
        Assert.Equal(0, game.Player.EdgePosition);
        for (int tick = 0; tick < 3; tick++)
        {
            game.RunOneStep(new PlayerInput(false, 80, false, false));
            Assert.Equal(0, game.Player.EdgeIndex);
            Assert.Equal(0, game.Player.EdgePosition);
        }
        Assert.InRange(game.Player.EdgeIndex, 0, 14);
        game.RunOneStep(new PlayerInput(false, -1, false, false));
        Assert.InRange(game.Player.EdgeIndex, 0, 14);
        Assert.True(game.Player.EdgeIndex > 0 || game.Player.EdgePosition > 0);

        MoveWithSpinnerUntilLane(game, spinnerDelta: -80, endpointLane: 14);
        Assert.Equal(1, game.Player.EdgePosition);
        for (int tick = 0; tick < 3; tick++)
        {
            game.RunOneStep(new PlayerInput(false, -80, false, false));
            Assert.Equal(14, game.Player.EdgeIndex);
            Assert.Equal(1, game.Player.EdgePosition);
            Assert.InRange(game.Player.EdgeIndex, 0, 14);
        }
        game.RunOneStep(new PlayerInput(false, 1, false, false));
        Assert.InRange(game.Player.EdgeIndex, 0, 14);
        Assert.True(game.Player.EdgeIndex < 14 || game.Player.EdgePosition < 1);
    }

    [Fact]
    public void OpenTubeRenderingDrawsFifteenRimEdgesAndSixteenDepthRails()
    {
        TempestGame game = new(startingProductionLevel: 8);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];

        new VectorRenderer(game).DrawTube((start, end, color) =>
            lines.Add((start, end, color)));

        Assert.Equal(46, lines.Count);
        for (int lane = 0; lane < 15; lane++)
        {
            Assert.Equal(lines[lane].Start, lines[16 + lane * 2].Start);
            Assert.Equal(lines[lane + 1].Start, lines[16 + lane * 2].End);
            Assert.Equal(lines[lane].End, lines[17 + lane * 2].Start);
            Assert.Equal(lines[lane + 1].End, lines[17 + lane * 2].End);
        }

        Assert.DoesNotContain(lines.Skip(16), line =>
            line.Start == lines[15].Start && line.End == lines[0].Start);

        TempestGame closedGame = new(startingProductionLevel: 7);
        List<(Vector2 Start, Vector2 End, Color Color)> closedLines = [];
        new VectorRenderer(closedGame).DrawTube((start, end, color) =>
            closedLines.Add((start, end, color)));

        Assert.Equal(48, closedLines.Count);
        Assert.Equal(closedLines[15].Start, closedLines[46].Start);
        Assert.Equal(closedLines[0].Start, closedLines[46].End);
        Assert.Equal(closedLines[15].End, closedLines[47].Start);
        Assert.Equal(closedLines[0].End, closedLines[47].End);
    }

    [Fact]
    public void EggsReverseAtOpenEndpointsWithoutEnteringLaneFifteen()
    {
        Level level = new(LevelConfiguration.Level8Production);
        Egg rightEndpoint = new(14, 1);
        Egg leftEndpoint = new(0, -1);

        for (int tick = 0; tick < 8; tick++)
        {
            rightEndpoint.Update(level);
            leftEndpoint.Update(level);
        }

        Assert.Equal(13, rightEndpoint.EdgeIndex);
        Assert.Equal(-1, rightEndpoint.MovementDirection);
        Assert.Equal(1, leftEndpoint.EdgeIndex);
        Assert.Equal(1, leftEndpoint.MovementDirection);
    }

    [Fact]
    public void TankerCargoKeepsTwoIdentitiesAndValidOpposingRoutesAtBothEndpoints()
    {
        Level level = new(LevelConfiguration.Level8Production);
        foreach (int endpointLane in new[] { 0, 14 })
        {
            Tanker tanker = new(endpointLane, endpointLane);
            TankerReleaseResult result = tanker.ReleaseFlipperCargo(
                level, firstSideDirection: 1, TankerReleaseKind.Natural);

            Assert.Equal(Tanker.CarriedFlipperCount, result.Cargo.Count);
            Assert.Equal(-result.Cargo[0].Direction, result.Cargo[1].Direction);
            Assert.All(result.Cargo, spawn =>
            {
                Assert.True(level.IsPlayableLane(spawn.SourceLane));
                Assert.True(level.IsPlayableLane(spawn.DestinationLane));
            });

            Flipper[] children = result.Cargo.Select((spawn, ordinal) =>
                Flipper.CreateTankerCargo(level, spawn, endpointLane * 2 + ordinal)).ToArray();
            Assert.Equal(2, children.Select(child => child.CargoUnitId).Distinct().Count());
            Assert.All(children, child => Assert.InRange(child.EdgeIndex, 0, 14));
            Assert.Empty(tanker.ReleaseFlipperCargo(
                level, firstSideDirection: 1, TankerReleaseKind.Natural).Cargo);
        }
    }

    [Fact]
    public void CargoBoundaryKeepsUnsignedEqualityOnLevelEightProgram()
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production;
        foreach ((byte rawAlong, bool expected) in new[]
                 {
                     ((byte)0x1F, false),
                     ((byte)0x20, true),
                     ((byte)0x21, true)
                 })
        {
            float height = Tanker.HeightForRomAlongPosition(rawAlong);
            Assert.Equal(rawAlong,
                Tanker.RomAlongPositionForHeight(height), precision: 4);
            Assert.Equal(expected, Tanker.IsLevel8DeepCargoPosition(height));
        }

        Tanker tanker = new(0, 0);
        Assert.False(tanker.UsesLevel8ProgrammedCargo(
            configuration, TankerReleaseKind.Natural));
        Assert.False(tanker.UsesLevel8ProgrammedCargo(
            configuration, TankerReleaseKind.SuperZapperDestruction));
    }

    [Fact]
    public void LevelEightDeepCargoRepeatsOneAlongIntervalAndOneFlipWithoutInteriorReversal()
    {
        Level level = new(LevelConfiguration.Level8Production);
        Flipper flipper = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(7, 8, -1, 200), cargoUnitId: 20,
            useLevel8ConstantFlipProgram: true);

        AdvanceUntil(flipper, level, () => flipper.State != FlipperState.Flipping);
        int firstLane = flipper.EdgeIndex;
        float heightAfterFlip = flipper.Height;
        int direction = flipper.Direction;

        flipper.Update(level, playerEdge: 0);
        Assert.Equal(firstLane, flipper.EdgeIndex);
        Assert.Equal(heightAfterFlip + level.Configuration.Flipper.ClimbSpeed,
            flipper.Height);

        flipper.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        AdvanceUntil(flipper, level, () => flipper.State != FlipperState.Flipping);

        Assert.Equal(9, flipper.EdgeIndex);
        Assert.Equal(direction, flipper.Direction);
        Assert.InRange(flipper.EdgeIndex, 0, 14);
    }

    [Fact]
    public void LevelEightConstantFlipRouteReversesOnlyWhenAnOpenEndpointRequiresIt()
    {
        Level level = new(LevelConfiguration.Level8Production);
        Flipper left = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(1, 0, 1, 200), cargoUnitId: 30,
            useLevel8ConstantFlipProgram: true);
        Flipper right = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(13, 14, -1, 200), cargoUnitId: 31,
            useLevel8ConstantFlipProgram: true);

        AdvanceToNextProgrammedFlip(left, level);
        AdvanceToNextProgrammedFlip(right, level);

        Assert.Equal(FlipperState.Flipping, left.State);
        Assert.Equal(1, left.DestinationEdge);
        Assert.Equal(-1, left.Direction);
        Assert.Equal(FlipperState.Flipping, right.State);
        Assert.Equal(13, right.DestinationEdge);
        Assert.Equal(1, right.Direction);
    }

    [Fact]
    public void LevelEightNaturalCargoClimbsStraightBelowRimAfterItsReleasePivot()
    {
        Level level = new(LevelConfiguration.Level8Production);
        Flipper flipper = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(7, 8, -1, 200), cargoUnitId: 40,
            useLevel8StraightCargoProgram: true);
        AdvanceUntil(flipper, level, () => flipper.State != FlipperState.Flipping);
        int laneAfterRelease = flipper.EdgeIndex;

        for (int tick = 0; tick < 20; tick++)
            flipper.Update(level, playerEdge: 0);

        Assert.Equal(laneAfterRelease, flipper.EdgeIndex);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.True(flipper.Height > 200);
        Assert.InRange(flipper.EdgeIndex, 0, 14);
    }

    [Fact]
    public void LevelEightProductionUsesMediumSupplySpikesAndRestrictedOrdinarySelection()
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production;
        Level level = new(configuration);
        TempestGame game = new(configuration, random: new MinimumRandom());

        Assert.Equal(24, configuration.EnemySupply);
        Assert.Equal(6, Level8WaveSelection.MaximumActiveEnemies);
        Assert.Equal(15, level.Spikes.Count);
        Assert.Equal(71.42857f, LevelConfiguration.Level8StartingSpikeHeight, precision: 4);
        Assert.All(level.Spikes, spike =>
        {
            Assert.InRange(spike.EdgeIndex, 0, 14);
            Assert.Equal(LevelConfiguration.Level8StartingSpikeHeight, spike.Height);
        });
        Assert.Equal(4, configuration.MaximumActiveEnemyShots);
        Assert.Equal(62, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(LevelExitPolicy.Standard, configuration.ExitPolicy);

        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.InRange(game.Tankers.Count, 1, 2);
        Assert.InRange(game.Spikers.Count, 2, 4);
        Assert.True(game.Tankers.Count + game.Spikers.Count + game.Flippers.Count <= 6);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(24, game.CurrentLevel.Level3Ledger!.OriginalCount);
    }

    [Fact]
    public void LevelEightSelectionEnforcesMinimumsMaximumsAndTheSixUnitGate()
    {
        Func<int, int, int> chooseFirst = (minimum, _) => minimum;

        Assert.Equal(Level3OriginalType.Spiker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 0, 0, 0, 0));
        Assert.Equal(Level3OriginalType.Spiker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 0, 1, 1, 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 0, 2, 2, 0));
        Assert.Equal(Level3OriginalType.Spiker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 2, 2, 4, 4));
        Assert.Equal(Level3OriginalType.Tanker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 1, 4, 5, 3));
        Assert.Equal(Level3OriginalType.Spiker,
            Level8WaveSelection.SelectOriginalType(chooseFirst, 2, 3, 5, 4));
        Assert.Null(Level8WaveSelection.SelectOriginalType(
            chooseFirst, 2, 4, 5, 4));
        Assert.Null(Level8WaveSelection.SelectOriginalType(
            chooseFirst, 2, 4, Level8WaveSelection.MaximumActiveEnemies, 4));
        Assert.Null(Level8WaveSelection.SelectOriginalType(
            chooseFirst, 1, 4, 5, 4));
    }

    [Fact]
    public void LevelEightDeathReconstructionClearsSpeciesAndConvertsReleasedCargoToGenericSupply()
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        Level level = new(configuration);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg latentCarrierEgg = level.Eggs[0];
        Egg releasedCarrierEgg = level.Eggs[1];
        ledger.CommitOriginalAsTanker(latentCarrierEgg.OriginalSlotId);
        ledger.CommitOriginalAsTanker(releasedCarrierEgg.OriginalSlotId);
        int[] releasedCargo = ledger.MaterializeLiveCargo(releasedCarrierEgg.OriginalSlotId).ToArray();
        ledger.MarkOriginalEscaped(releasedCarrierEgg.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(5, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(2, ledger.CargoCount);
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.All(releasedCargo,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
        Assert.Equal(5, level.PendingGenericEnemyUnits);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);
        Assert.Equal(5, ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(5, level.PendingGenericEnemyUnits);
    }

    [Fact]
    public void FinalPendingSpikerConvertsInPlaceWithoutAddingGenericSupply()
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1
        };
        TempestGame game = new(configuration, random: new MinimumRandom());
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        int originalId = spiker.OriginalSlotId;

        for (int tick = 0; tick < 1000 && game.Tankers.Count == 0; tick++)
            game.RunOneStep();

        Tanker converted = Assert.Single(game.Tankers);
        Assert.Equal(originalId, converted.OriginalSlotId);
        Assert.Equal(1, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(Level3OriginalType.Tanker,
            game.CurrentLevel.Level3Ledger.GetOriginalType(originalId));
        Assert.Equal(2,
            game.CurrentLevel.Level3Ledger.GetCargoIdsForOriginal(originalId).Count);
        Assert.Equal(0, game.CurrentLevel.PendingGenericEnemyUnits);
    }

    [Fact]
    public void DirectLevelNineUsesProductionWaveAndTerminalRestartBeginsLevelOne()
    {
        TempestGame game = new(startingProductionLevel: 9, startingLives: 5);
        game.Score.Add(88);

        Assert.Same(LevelConfiguration.Level9Production, game.CurrentLevel.Configuration);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(27, game.CurrentLevel.EnemySupply);
        Assert.Equal(15, game.Spikes.Count);
        Assert.Same(LevelConfiguration.Level9Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level8Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level9Production));

        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);

        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    [Fact]
    public void StandardExitFiresMovesAndRecordsShotsWithNormalLimits()
    {
        GameplayStartupConfiguration startup = new(8, 5, 1.0, 1981);
        RecordingAudioEvents audio = new();
        TempestGame game = new(startup, audio);
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(100);
        PlayerInputRecorder recorder = new(startup);
        PlayerInput[] inputs = Enumerable.Range(0, 36)
            .Select(tick => new PlayerInput(tick != 0, tick == 0 ? 80 : 0, false, false))
            .ToArray();
        for (int tick = 0; tick < inputs.Length; tick++)
            recorder.CaptureTick(tick, inputs[tick]);
        GameplayPlaybackSession playback = new(startup, recorder.CreateReplay());
        playback.Game.CurrentLevel.State = LevelState.ExitZoom;
        playback.Game.Player.AdvanceExit(100);
        Projection.K = 70;
        Projection.ZOffset = 5;
        float directK = 70;
        float directZOffset = 5;
        float playbackK = 70;
        float playbackZOffset = 5;

        for (int tick = 0; tick < inputs.Length; tick++)
        {
            PlayerInput input = inputs[tick];
            Projection.K = directK;
            Projection.ZOffset = directZOffset;
            game.RunOneStep(input);
            directK = Projection.K;
            directZOffset = Projection.ZOffset;

            Projection.K = playbackK;
            Projection.ZOffset = playbackZOffset;
            Assert.True(playback.StepForward());
            playbackK = Projection.K;
            playbackZOffset = Projection.ZOffset;

            Assert.Equal(game.State, playback.Game.State);
            Assert.Equal(game.Player.EdgeIndex, playback.Game.Player.EdgeIndex);
            Assert.Equal(game.Player.EdgePosition, playback.Game.Player.EdgePosition, precision: 4);
            Assert.Equal(game.Player.Depth, playback.Game.Player.Depth, precision: 4);
            Assert.Equal(
                game.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
                playback.Game.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
        }

        Projection.K = 70;
        Projection.ZOffset = 5;
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.True(game.ExitShotsVisible);
        Assert.Equal(10, game.Shots.Count);
        Assert.All(game.Shots, shot => Assert.Equal(game.Player.EdgeIndex, shot.EdgeIndex));
        Assert.Equal(10, audio.Events.Count(e => e == AudioEvent.PlayerShot));
        Assert.Equal(inputs.Length, recorder.DurationTicks);
    }

    [Fact]
    public void StandardExitShotErodesOnlyTheSpikeInItsLane()
    {
        TempestGame game = new(startingProductionLevel: 8);
        game.CurrentLevel.State = LevelState.ExitZoom;
        Spike targetedSpike = game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex);
        Spike otherLaneSpike = game.Spikes.Single(spike => spike.EdgeIndex == 8);
        float initialTargetHeight = targetedSpike.Height;
        float initialOtherHeight = otherLaneSpike.Height;

        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Single(game.Shots);
        Assert.Equal(game.Player.EdgeIndex, game.Shots[0].EdgeIndex);

        for (int tick = 0; tick < 80 && targetedSpike.Height == initialTargetHeight; tick++)
            game.RunOneStep();

        Assert.Equal(initialTargetHeight - 10, targetedSpike.Height, precision: 4);
        Assert.Equal(initialOtherHeight, otherLaneSpike.Height);
        Assert.Equal(1, game.Score.Score);
        Assert.Empty(game.Shots);
        Assert.Equal(LevelState.ExitZoom, game.State);

        TempestGame otherLaneGame = new(startingProductionLevel: 8);
        otherLaneGame.CurrentLevel.State = LevelState.ExitZoom;
        otherLaneGame.Player.AdvanceExit(150);
        otherLaneGame.Spikes.Single(spike =>
            spike.EdgeIndex == otherLaneGame.Player.EdgeIndex).Height = 0;
        Spike unrelatedSpike = otherLaneGame.Spikes.Single(spike => spike.EdgeIndex == 8);
        unrelatedSpike.Height = 200;
        otherLaneGame.RunOneStep(new PlayerInput(true, 0, false, false));
        for (int tick = 0; tick < 12; tick++)
            otherLaneGame.RunOneStep();

        Assert.Equal(200, unrelatedSpike.Height);
        Assert.Equal(0, otherLaneGame.Score.Score);
    }

    [Fact]
    public void StandardExitSpikeCollisionFollowsTheBlasterLaneAndLaneChanges()
    {
        TempestGame game = new(startingProductionLevel: 8);
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.ResetPosition(7, 0.99f);
        game.Player.AdvanceExit(298);
        game.Spikes.Single(spike => spike.EdgeIndex == 7).Height = 0;
        Spike adjacentLaneSpike = game.Spikes.Single(spike => spike.EdgeIndex == 8);
        adjacentLaneSpike.Height = 100;

        game.RunOneStep(new PlayerInput(false, 0, false, true));

        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(8, game.Player.EdgeIndex);

        game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(8, game.Player.EdgeIndex);
    }

    [Fact]
    public void StandardExitDeathAfterWaveClearRetriesWithoutAvoidSpikesInterstitial()
    {
        TempestGame game = new(startingProductionLevel: 8, startingLives: 5);
        CompleteProductionWave(game);
        Assert.Equal(LevelState.ExitDelay, game.State);
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(298);
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 100;

        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);

        for (int tick = 0; tick < 100 && game.State == LevelState.ReverseZoom; tick++)
            game.RunOneStep();

        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(LevelExitPolicy.Standard, game.CurrentLevel.Configuration.ExitPolicy);

        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.ShouldShowAvoidSpikesWarning);
        Assert.Same(LevelConfiguration.Level8Production, game.CurrentLevel.Configuration);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14)]
    public void StandardExitSpikeCollisionAtOpenEndpointsReentersWithoutCompleting(int lane)
    {
        LevelConfiguration configuration = LevelConfiguration.Level8Production with
        {
            InitialActiveEggCount = 0
        };
        TempestGame game = new(configuration, startingLives: 5);
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.ResetPosition(lane, 0.5f);
        game.Player.AdvanceExit(298);
        game.Spikes.Single(spike => spike.EdgeIndex == lane).Height = 100;

        game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(lane, game.Player.EdgeIndex);
        Assert.InRange(lane, 0, 14);
        Assert.Equal(5, game.Player.Lives);
        Assert.False(game.LevelComplete);

        for (int tick = 0; tick < 100 && game.State == LevelState.ReverseZoom; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(configuration, game.CurrentLevel.Configuration);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(LevelConfiguration.Level8Production.InitialPlayerLane, game.Player.EdgeIndex);
        Assert.False(game.LevelComplete);
        Assert.InRange(game.Player.EdgeIndex, 0, 14);
    }

    [Fact]
    public void HoldingFireClearsTheExitLaneSpikeBeforeLevelNineTerminalCompletion()
    {
        TempestGame game = new(startingProductionLevel: 9);
        game.CurrentLevel.State = LevelState.ExitZoom;
        Spike exitLaneSpike = game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex);

        for (int tick = 0; tick < 140 && game.State == LevelState.ExitZoom; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));

        Assert.True(exitLaneSpike.IsDepleted);
        Assert.DoesNotContain(game.Spikes, spike => spike.EdgeIndex == game.Player.EdgeIndex);
        Assert.Equal(LevelState.Complete, game.State);
        Assert.Null(LevelProgression.GetNextProductionLevel(game.CurrentLevel.Configuration));
    }

    [Fact]
    public void LevelEightSuperZapperUsesNormalChargesAndRechargesOnFreshLevel()
    {
        TempestGame game = new(startingProductionLevel: 8);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(SuperZapperSystem.InitialCharges - 1,
            game.SuperZapperChargesRemaining);
        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
    }

    private static void AdvanceToNextProgrammedFlip(Flipper flipper, Level level)
    {
        AdvanceUntil(flipper, level, () => flipper.State != FlipperState.Flipping);
        flipper.Update(level, playerEdge: 7);
        flipper.Update(level, playerEdge: 7);
    }

    private static void AdvanceUntil(Flipper flipper, Level level, Func<bool> condition)
    {
        for (int tick = 0; tick < 100 && !condition(); tick++)
            flipper.Update(level, playerEdge: 0);
        Assert.True(condition(), "The Level 8 Flipper route did not reach its expected state.");
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static TempestGame CreatePlayerControlGame(int displayedLevel)
    {
        LevelConfiguration configuration = LevelProgression.GetProductionLevel(displayedLevel)! with
        {
            InitialActiveEggCount = 0
        };
        TempestGame game = new(configuration);
        game.CurrentLevel.State = LevelState.Playing;
        return game;
    }

    private static void MoveWithSpinnerUntilLane(
        TempestGame game, float spinnerDelta, int endpointLane)
    {
        for (int tick = 0; tick < 20 && game.Player.EdgeIndex != endpointLane; tick++)
        {
            game.RunOneStep(new PlayerInput(false, spinnerDelta, false, false));
            Assert.InRange(game.Player.EdgeIndex, 0, 14);
        }

        Assert.Equal(endpointLane, game.Player.EdgeIndex);
    }

    private static Vector2 GetBlasterGeometryPosition(TempestGame game)
    {
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        int nextPoint = (game.Player.EdgeIndex + 1) % points.Count;
        return Vector2.Lerp(points[game.Player.EdgeIndex], points[nextPoint],
            game.Player.EdgePosition);
    }

    private static float GetScreenClockwiseAngleDelta(Vector2 start, Vector2 end)
    {
        float startAngle = MathF.Atan2(start.Y, start.X);
        float endAngle = MathF.Atan2(end.Y, end.X);
        float delta = endAngle - startAngle;
        while (delta <= -MathF.PI)
            delta += 2 * MathF.PI;
        while (delta > MathF.PI)
            delta -= 2 * MathF.PI;
        return delta;
    }

    private static void CompleteProductionWave(TempestGame game)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        foreach (int id in ledger.UnresolvedOriginalIds.Order())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        level.ResetAfterDeath();
        game.RunOneStep();
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        public List<AudioEvent> Events { get; } = [];
        public void Play(AudioEvent audioEvent) => Events.Add(audioEvent);
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
