using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Tempest.Game.Rendering;
using System.Numerics;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class SimulationTests
{
    // --- Tanker development scenario (--tanker-test): flags, slot identity, climb and release ---

    [Fact]
    public void TankerTestFlagKeepsNormalAndSpikerModesSeparateAndRejectsConflict()
    {
        string[] tankerArgs = ["--tanker-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseTankerTest(tankerArgs));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(tankerArgs));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(tankerArgs));
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(
            ["--tanker-test", "--enemy-bullets-test"]));
        Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(
            ["--tanker-test", "--spiker-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, tankerTest: true));

        TempestGame normal = new();
        Assert.False(normal.TankerTest);
        Assert.False(normal.CurrentLevel.TankerDevelopmentWave);
        Assert.Empty(normal.Tankers);
        Assert.Empty(normal.Spikes);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.TankerTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);
        TempestGame tankerGame = new(startingLives: 10, tankerTest: true);
        Assert.True(tankerGame.TankerTest);
        Assert.Equal(10, tankerGame.Player.Lives);
        Assert.Equal(4, tankerGame.Eggs.Count);
        Assert.Equal(6, tankerGame.CurrentLevel.ReserveEggs);
        Assert.Empty(tankerGame.Tankers);
        Assert.Equal(10, tankerGame.TankerOriginalSlotsRemaining);
    }

    [Fact]
    public void TankerScenarioEnemyBulletOptionAppliesToFlippersOnly()
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(2, []));
        Tanker tanker = Assert.Single(game.Tankers);
        Egg ordinaryEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 1);
        MakeReady(ordinaryEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper ordinary = Assert.Single(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.Equal(2, ordinary.BulletAmmunition);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(2, tanker.LatentCargoCount);
    }

    [Fact]
    public void TankerWaveHatchesStableOriginalSlotZeroExactlyOnce()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(0, tanker.OriginalSlotId);
        Assert.InRange(tanker.EdgeIndex, 0, 15);
        Assert.InRange(tanker.Height, 11.84f, 11.86f);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        Assert.Equal(1, game.ActiveTankerOriginalEnemies);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Empty(game.Spikers);
    }

    [Fact]
    public void NonTankerOriginalEggSlotsContinueToHatchOrdinaryFlippers()
    {
        TempestGame game = CreateTankerWave();
        Egg ordinaryEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 1);
        MakeReady(ordinaryEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers, candidate => !candidate.IsTankerCargo);
        Assert.Equal(1, flipper.OriginalSlotId);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Single(game.Tankers);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(10, game.TankerAccountingTotal);
    }

    [Fact]
    public void TankerFixedSpeedKeepsLaneAndReleasesStrictlyAbove385()
    {
        Tanker tanker = new(7, 0, climbSpeed: 1);
        Assert.Equal(10, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        for (int tick = 0; tick < 375; tick++)
            Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(385, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(7, tanker.EdgeIndex);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease, tanker.UpdateForDevelopment());
        Assert.Equal(386, tanker.Height);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
    }

    [Fact]
    public void TankerUsesDocumentedDevelopmentSpeedAndNeverDirectlyHitsPlayer()
    {
        Tanker tanker = new(7, 0);
        Assert.Equal(1.85f, tanker.ClimbSpeed, 5);
        Assert.False(tanker.HitsPlayer(new Player()));
        Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.InRange(tanker.Height, 11.849f, 11.851f);
        Assert.Equal(7, tanker.EdgeIndex);
    }

    [Theory]
    [InlineData(0, 15, 1, 14, 2)]
    [InlineData(15, 14, 0, 13, 1)]
    [InlineData(6, 5, 7, 4, 8)]
    public void TankerCargoUsesOppositeAdjacentLanesAndDirectionsOnContinuousTube(
        int tankerLane, int expectedFirstLane, int expectedSecondLane,
        int expectedFirstDestination, int expectedSecondDestination)
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(tankerLane);
        TankerReleaseResult result = tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.Natural);
        Assert.Equal(TankerReleaseKind.Natural, result.Kind);
        Assert.Equal(2, result.Cargo.Count);
        Assert.Equal(expectedFirstLane, result.Cargo[0].SourceLane);
        Assert.Equal(expectedSecondLane, result.Cargo[1].SourceLane);
        Assert.Equal(expectedFirstDestination, result.Cargo[0].DestinationLane);
        Assert.Equal(expectedSecondDestination, result.Cargo[1].DestinationLane);
        Assert.Equal(1, result.Cargo[0].Direction);
        Assert.Equal(-1, result.Cargo[1].Direction);
        Assert.Equal(tanker.Height, result.Cargo[0].Height);
        Assert.Equal(tanker.Height, result.Cargo[1].Height);
        Assert.Equal(TankerState.CargoReleased, tanker.State);
        Assert.Equal(0, tanker.LatentCargoCount);
        Assert.Empty(tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.PlayerShotDestruction).Cargo);
    }

    [Fact]
    public void TankerCargoFlippersStartInCalculatedPivotAndPreserveOrdinaryConstructor()
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(0);
        TankerCargoSpawn[] cargo = tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.Natural).Cargo.ToArray();
        Flipper first = Flipper.CreateTankerCargo(level, cargo[0], 10);
        Flipper second = Flipper.CreateTankerCargo(level, cargo[1], 11);
        Assert.Equal(FlipperState.Flipping, first.State);
        Assert.Equal(FlipperState.Flipping, second.State);
        Assert.Equal(cargo[0].SourceLane, first.EdgeIndex);
        Assert.Equal(cargo[0].SourceLane, first.SourceEdge);
        Assert.Equal(cargo[0].DestinationLane, first.DestinationEdge);
        Assert.Equal(cargo[0].Direction, first.Direction);
        Assert.Equal(level.CalculateEdgeTurnAngle(first.SourceEdge, first.DestinationEdge), first.TotalTurnAngle);
        Assert.Equal(0, first.TurnAngle);
        Assert.Equal(0, second.TurnAngle);
        Assert.Equal(-first.Direction, second.Direction);
        Assert.Equal(tanker.Height, first.Height);
        Assert.Equal(tanker.Height, second.Height);
        Assert.Equal(10, first.CargoUnitId);
        Assert.Equal(11, second.CargoUnitId);
        Assert.Equal(15, first.SourceEdge);
        Assert.Equal(1, second.SourceEdge);
        Assert.Equal(14, first.DestinationEdge);
        Assert.Equal(2, second.DestinationEdge);
        Assert.Equal(1, first.Direction);
        Assert.Equal(-1, second.Direction);
        Assert.Equal(FlipperState.Climbing, new Flipper(4, 0).State);

        float height = first.Height;
        first.Update(level, 8);
        Assert.Equal(height + 2, first.Height);
        Assert.Equal(8, first.TurnAngle);
    }

    [Fact]
    public void CargoPivotDoesNotKillPlayerBeforeCargoReachesTheRim()
    {
        Level level = new();
        Tanker tanker = CreatePendingTanker(0);
        TankerCargoSpawn cargo = tanker.ReleaseFlipperCargo(level, 1, TankerReleaseKind.Natural).Cargo[0];
        Flipper flipper = Flipper.CreateTankerCargo(level, cargo, 0);
        int ticks = 0;
        while (flipper.State == FlipperState.Flipping && ticks++ < 10)
            flipper.Update(level, cargo.DestinationLane);
        Assert.True(flipper.Height < Level.Depth);
        Assert.False(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    // --- Flipper rim-arrival collision/timing regressions (direct climb into the player lane) ---

    [Fact]
    public void ClimbingFlipperReachingRimInPlayerLaneSignalsLethalCollision()
    {
        Level level = new();
        Flipper flipper = new(5, Level.Depth - 1);
        flipper.Update(level, 5);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.NotEqual(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void ClimbingFlipperReachingRimInAnotherLaneStillPauses()
    {
        Level level = new();
        Flipper flipper = new(5, Level.Depth - 1);
        flipper.Update(level, 6);
        Assert.False(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void DirectRimArrivalKillsPlayerOnceAndShotOnFinalTickWinsInstead()
    {
        TempestGame game = CreateTankerWave();
        Flipper flipper = HatchOneOrdinaryTankerWaveFlipper(game);
        int lives = game.Player.Lives;
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        ForceClimbToJustBelowRim(flipper, game);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);

        game = CreateTankerWave();
        flipper = HatchOneOrdinaryTankerWaveFlipper(game);
        lives = game.Player.Lives;
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        ForceClimbToJustBelowRim(flipper, game);
        shot.Height = flipper.Height - 5;
        game.RunOneStep();
        Assert.DoesNotContain(flipper, game.Flippers);
        Assert.NotEqual(LevelState.PlayerDying, game.State);
        Assert.Equal(lives, game.Player.Lives);
    }

    private static void ForceClimbToJustBelowRim(Flipper flipper, TempestGame game)
    {
        for (int tick = 0; flipper.Height < Level.Depth - 2 && tick < 1000; tick++)
            flipper.Update(game.CurrentLevel, -1);
    }

    [Fact]
    public void DirectRimArrivalRunsFullDeathLifecycleAndRestartsUnresolvedEnemiesAsFreshEggs()
    {
        TempestGame game = CreateTankerWave();
        DestroyOriginalFlipperSlot(game, 1);
        Egg egg = FindOrReleaseOriginalEgg(game, 2)!;
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers, candidate => candidate.OriginalSlotId == 2);
        Egg[] oldEggs = game.Eggs.ToArray();
        Flipper[] oldFlippers = game.Flippers.ToArray();
        int score = game.Score.Score;
        int lives = game.Player.Lives;
        Assert.True(score > 0);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);

        ForceClimbToJustBelowRim(flipper, game);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathTicks);
        Assert.Equal(lives, game.Player.Lives);

        Step(game, 59);
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(lives, game.Player.Lives);
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(lives - 1, game.Player.Lives);

        Assert.Equal(score, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.DoesNotContain(game.Eggs, fresh => oldEggs.Contains(fresh));
        Assert.Equal(game.Eggs.Count, game.Eggs.Select(fresh => fresh.OriginalSlotId).Distinct().Count());
        Assert.DoesNotContain(game.Eggs, fresh => fresh.OriginalSlotId == 1);
        Assert.Contains(game.Eggs, fresh => fresh.OriginalSlotId == 0);
        Assert.All(game.Eggs, fresh =>
        {
            Assert.Equal(-10, fresh.Height);
            Assert.Equal(EggState.Roaming, fresh.State);
        });
        Assert.DoesNotContain(oldFlippers, game.Flippers.Contains);
        Assert.Equal(9, game.Eggs.Count + game.CurrentLevel.ReserveEggs);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        AssertTankerAccounting(game);
    }

    // --- Tanker shot/natural release, cargo ledger, death/restart and rendering regressions ---

    [Theory]
    [InlineData(7, 385, true)]
    [InlineData(7, 386, false)]
    [InlineData(6, 385, false)]
    public void TankerShotRequiresSameLaneAndStrictShotDepthBelowTanker(int shotLane, float shotHeight, bool expected)
    {
        Tanker tanker = CreatePendingTanker(7);
        Assert.Equal(expected, tanker.IsHitBy(new Shot { EdgeIndex = shotLane, Height = shotHeight }));
    }

    [Fact]
    public void TankerPlayerShotScores200AndReleasesExactlyTwoLiveCargoFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(2, game.TankerMaterializedCargoUnits);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo), flipper =>
        {
            Assert.Equal(FlipperState.Flipping, flipper.State);
            Assert.Equal(tanker.Height, flipper.Height);
        });
        Level shotLevel = game.CurrentLevel;
        Assert.Equal(new[] { shotLevel.PreviousEdge(tanker.EdgeIndex), shotLevel.NextEdge(tanker.EdgeIndex) },
            game.Flippers.Where(flipper => flipper.IsTankerCargo).Select(flipper => flipper.EdgeIndex));
        Assert.Equal(new[] { shotLevel.PreviousEdge(shotLevel.PreviousEdge(tanker.EdgeIndex)),
                shotLevel.NextEdge(shotLevel.NextEdge(tanker.EdgeIndex)) },
            game.Flippers.Where(flipper => flipper.IsTankerCargo).Select(flipper => flipper.DestinationEdge));
        Assert.Empty(game.Spikers);
    }

    [Fact]
    public void TankerNaturalReleaseEscapesWithoutCarrierScoreAndCreatesTwoCargoFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        float releaseHeight = tanker.Height;
        game.RunOneStep();
        Assert.Empty(game.Tankers);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(releaseHeight, flipper.Height));

        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.NotSame(cargo[0], cargo[1]);
        Assert.NotEqual(cargo[0].CargoUnitId, cargo[1].CargoUnitId);
        Level level = game.CurrentLevel;
        int lane = tanker.EdgeIndex;
        Assert.Equal(new[] { level.PreviousEdge(lane), level.NextEdge(lane) },
            cargo.Select(child => child.EdgeIndex));
        Assert.Equal(new[] { level.PreviousEdge(level.PreviousEdge(lane)), level.NextEdge(level.NextEdge(lane)) },
            cargo.Select(child => child.DestinationEdge));
        foreach (Flipper child in cargo)
            for (int tick = 0; child.State == FlipperState.Flipping && tick < 10; tick++)
                child.Update(level, 0);

        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(new[] { level.PreviousEdge(level.PreviousEdge(lane)), level.NextEdge(level.NextEdge(lane)) },
            cargo.Select(child => child.EdgeIndex));
        Assert.NotEqual(cargo[0].EdgeIndex, cargo[1].EdgeIndex);
        Assert.DoesNotContain(cargo, child => child.EdgeIndex == lane);
        Assert.True(cargo[0].IsHitBy(new Shot { EdgeIndex = cargo[0].EdgeIndex, Height = cargo[0].Height - 1 }));
        Assert.True(cargo[1].IsHitBy(new Shot { EdgeIndex = cargo[1].EdgeIndex, Height = cargo[1].Height - 1 }));
        Assert.False(cargo[0].IsHitBy(new Shot { EdgeIndex = cargo[1].EdgeIndex, Height = cargo[0].Height - 1 }));
    }

    [Fact]
    public void TankerHitConsumesShotBeforeCargoCanBeHitByThatShot()
    {
        TempestGame game = CreateTankerWave();
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Assert.Empty(game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(2, game.ActiveTankerCargoFlippers);
        Assert.Equal(0, game.TankerCargoUnitsResolved);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(tanker.Height, flipper.Height));
    }

    [Fact]
    public void TankerAndCargoCanBothScoreWhenShotThenChildrenAreDestroyed()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Flipper cargo = game.Flippers.First(flipper => flipper.IsTankerCargo);
        Step(game, 1);
        game.Player.Move(cargo.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        shot = Assert.Single(game.Shots);
        shot.Height = cargo.Height - 5;
        game.RunOneStep();
        Assert.Equal(350, game.Score.Score);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
    }

    [Fact]
    public void TankerDeathBeforeReleaseRestoresOneStableOriginalSlotEggAndLatentCargo()
    {
        TempestGame game = CreateTankerWave();
        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Empty(game.Tankers);
        Assert.Contains(game.Eggs, egg => egg.OriginalSlotId == 0);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(0, game.TankerMaterializedCargoUnits);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void DeathWithThreeUnresolvedOriginalSlotsRebuildsOnlyFreshEggEntrants()
    {
        TempestGame game = CreateTankerWave(startingLives: 100);
        Tanker tanker = Assert.Single(game.Tankers);
        ShootTanker(game, tanker);
        foreach (Flipper cargo in game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray())
            ShootDevelopmentFlipper(game, cargo);

        for (int slotId = 1; slotId <= 6; slotId++)
            DestroyOriginalFlipperSlot(game, slotId);

        Egg survivingEgg = FindOrReleaseOriginalEgg(game, 7)!;
        MakeReady(survivingEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper survivor = Assert.Single(game.Flippers, flipper => flipper.OriginalSlotId == 7);
        Step(game, 15);
        Assert.InRange(survivor.Height, 2, Level.Depth - 1);
        Egg[] oldEggs = game.Eggs.ToArray();
        Flipper[] oldFlippers = game.Flippers.ToArray();
        int score = game.Score.Score;
        Assert.Equal(3, game.TankerOriginalSlotsRemaining);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(100, game.Player.Lives);

        ForceFlipperPivotIntoPlayer(game, survivor);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);

        Assert.Equal(99, game.Player.Lives);
        Assert.Equal(score, game.Score.Score);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Flippers);
        Assert.Equal(3, game.Eggs.Count);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(new[] { 7, 8, 9 }, game.Eggs.Select(egg => egg.OriginalSlotId).Order());
        Assert.Equal(7, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.All(game.Eggs, egg =>
        {
            Assert.DoesNotContain(egg, oldEggs);
            Assert.DoesNotContain(oldFlippers, flipper => flipper.EdgeIndex == egg.EdgeIndex &&
                Math.Abs(flipper.Height - egg.Height) < 1);
            Assert.Equal(-10, egg.Height);
            Assert.Equal(0, egg.UpdateCounter);
            Assert.Equal(EggState.Roaming, egg.State);
        });
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CargoEggIdentitySurvivesASecondDeathBeforeHatching()
    {
        TempestGame game = CreateTankerWave(startingLives: 5);
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        game.RunOneStep();
        Flipper cargo = game.Flippers.First(flipper => flipper.IsTankerCargo);
        ShootDevelopmentFlipper(game, cargo);
        Flipper survivor = game.Flippers.Single(flipper => flipper.IsTankerCargo);
        ForceFlipperPivotIntoPlayer(game, survivor);
        game.RunOneStep();
        Step(game, 60);

        Egg firstLifeCargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        int cargoId = firstLifeCargoEgg.CargoUnitId;
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        AssertTankerAccounting(game);

        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);

        Egg secondLifeCargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.NotSame(firstLifeCargoEgg, secondLifeCargoEgg);
        Assert.Equal(cargoId, secondLifeCargoEgg.CargoUnitId);
        Assert.Equal(-10, secondLifeCargoEgg.Height);
        Assert.Equal(EggState.Roaming, secondLifeCargoEgg.State);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void CargoLifeResetRestoresSurvivingCargoWithoutReturningResolvedUnits()
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new BulletRandom(2, [3, 2]));
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        game.RunOneStep();
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Empty(game.Tankers);

        Flipper cargoToDestroy = game.Flippers.First(flipper => flipper.IsTankerCargo);
        ShootDevelopmentFlipper(game, cargoToDestroy);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Flipper survivingCargo = Assert.Single(game.Flippers, flipper => flipper.IsTankerCargo);
        float formerHeight = survivingCargo.Height;
        int formerLane = survivingCargo.EdgeIndex;
        ForceFlipperPivotIntoPlayer(game, survivingCargo);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Empty(game.Flippers);
        Assert.Equal(0, game.ActiveTankerCargoFlippers);
        Assert.Equal(1, game.PendingTankerCargoUnits);
        Egg cargoEgg = Assert.Single(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.Equal(-10, cargoEgg.Height);
        Assert.Equal(0, cargoEgg.UpdateCounter);
        Assert.Equal(EggState.Roaming, cargoEgg.State);
        Assert.NotEqual(formerLane, cargoEgg.EdgeIndex);
        Assert.NotEqual((int)formerHeight, (int)cargoEgg.Height);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.DoesNotContain(game.Eggs, egg => egg.OriginalSlotId == 0);
        Assert.Empty(game.Tankers);
        Assert.Equal(150, game.Score.Score);

        MakeReady(cargoEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper freshCargo = Assert.Single(game.Flippers, flipper => flipper.IsTankerCargo);
        Assert.NotSame(survivingCargo, freshCargo);
        Assert.Equal(FlipperState.Climbing, freshCargo.State);
        Assert.Equal(2, freshCargo.Height);
        Assert.Equal(survivingCargo.CargoUnitId, freshCargo.CargoUnitId);
        Assert.InRange(freshCargo.BulletAmmunition, 1, 3);
        Assert.Equal(15, freshCargo.BulletTimer);
        Assert.NotEqual((int)formerHeight, (int)freshCargo.Height);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerDevelopmentOriginalEggAndCargoAccountingRemainCoherent()
    {
        TempestGame game = CreateTankerWave();
        AssertTankerAccounting(game);
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        AssertTankerAccounting(game);
        Assert.False(game.LevelComplete);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
    }

    [Fact]
    public void CompleteTankerScenarioResolvesDynamicOriginalAndCargoLedgerThenReplays()
    {
        TempestGame game = new(startingLives: 100, tankerTest: true);
        int previousResolvedCargo = 0;
        for (int tick = 0; tick < 30000 && !game.LevelComplete && !game.GameOver; tick++)
        {
            Enemy? target = game.Tankers.Cast<Enemy>()
                .Concat(game.Flippers)
                .OrderByDescending(enemy => enemy.Height)
                .FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);

            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertTankerAccounting(game);
            Assert.True(game.TankerCargoUnitsResolved >= previousResolvedCargo);
            previousResolvedCargo = game.TankerCargoUnitsResolved;
        }

        Assert.True(game.LevelComplete,
            $"state={game.State}, lives={game.Player.Lives}, original={game.TankerOriginalSlotsRemaining}, " +
            $"cargo={game.TankerCargoUnitsRemaining}, activeTankers={game.Tankers.Count}, " +
            $"activeFlippers={game.Flippers.Count}, eggs={game.Eggs.Count}, reserve={game.CurrentLevel.ReserveEggs}");
        Assert.Equal(10, game.TankerOriginalSlotsResolved);
        Assert.Equal(10, game.TankerOriginalSlotsDestroyed + game.TankerOriginalSlotsEscaped);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);

        int score = game.Score.Score;
        int lives = game.Player.Lives;
        game.RunOneStep(restartPressed: true);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(score, game.Score.Score);
        Assert.Equal(lives, game.Player.Lives);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(0, game.TankerMaterializedCargoUnits);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertTankerAccounting(game);
    }

    [Fact]
    public void TankerScenarioGameOverRestartRestoresInitialLedger()
    {
        TempestGame game = CreateTankerWave(startingLives: 1);
        Flipper ordinary = HatchOneOrdinaryTankerWaveFlipper(game);
        ForceFlipperPivotIntoPlayer(game, ordinary);
        game.RunOneStep();
        Step(game, 60);
        Assert.True(game.GameOver);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(1, game.Player.Lives);
    }

    [Fact]
    public void TankerProjectionDrawsStableFiniteMagentaGlyphAndCargoUsesFlipperRendering()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        VectorRenderer renderer = new(game);
        Vector2 expected = ProjectTanker(game, tanker);
        Assert.Equal(expected, renderer.ProjectTankerCenter(tanker));
        List<(Vector2 Start, Vector2 End)> first = [];
        List<(Vector2 Start, Vector2 End)> second = [];
        renderer.DrawTankers((start, end) => first.Add((start, end)));
        renderer.DrawTankers((start, end) => second.Add((start, end)));
        Assert.Equal(16, first.Count);
        Assert.Equal(first, second);
        foreach ((Vector2 start, Vector2 end) in first)
        {
            Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
            Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
        }

        TankerCargoSpawn[] cargo = tanker.ReleaseFlipperCargo(game.CurrentLevel, 1, TankerReleaseKind.Natural).Cargo.ToArray();
        Flipper released = Flipper.CreateTankerCargo(game.CurrentLevel, cargo[0], 0);
        Assert.Equal(FlipperState.Flipping, released.State);
    }

    [Fact]
    public void TankerScenarioFlagsAreIndependentAndMutuallyExclusiveWithSpikerMode()
    {
        string[] tankerArgs = ["--tanker-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseTankerTest(tankerArgs));
        Assert.False(Tempest.Game.Debug.DebugController.HasConflictingScenarios(tankerArgs));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(tankerArgs));
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(
            ["--tanker-test", "--enemy-bullets-test"]));
        Assert.True(Tempest.Game.Debug.DebugController.HasConflictingScenarios(
            ["--tanker-test", "--spiker-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(true, tankerTest: true));

        TempestGame normal = new();
        Assert.False(normal.TankerTest);
        Assert.False(normal.CurrentLevel.TankerDevelopmentWave);
        Assert.Empty(normal.Tankers);
        Assert.Empty(normal.CurrentLevel.Spikes);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.TankerTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);
        TempestGame tanker = new(startingLives: 10, tankerTest: true);
        Assert.True(tanker.TankerTest);
        Assert.False(tanker.CurrentLevel.SpikerDevelopmentWave);
        Assert.Equal(10, tanker.Player.Lives);
        Assert.Equal(4, tanker.Eggs.Count);
        Assert.Equal(6, tanker.CurrentLevel.ReserveEggs);
        Assert.Empty(tanker.Tankers);
        Assert.Equal(10, tanker.TankerOriginalSlotsRemaining);
    }

    [Fact]
    public void TankerStartsAtHeightTenOnStableOriginalEggLaneAndCarriesTwoLatentFlippers()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(0, tanker.OriginalSlotId);
        Assert.InRange(tanker.EdgeIndex, 0, 15);
        Assert.InRange(tanker.Height, 11.84f, 11.86f);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(2, tanker.LatentCargoCount);
        Assert.Empty(game.Spikers);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(1, game.ActiveTankerOriginalEnemies);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(0, game.TankerOriginalSlotsEscaped);
        Assert.False(tanker.HitsPlayer(game.Player));
    }

    [Fact]
    public void TankerMovesAtFixedSpeedOnItsLaneAndDoesNotReleaseAt385()
    {
        Tanker tanker = new(4, 0, climbSpeed: 1);
        Assert.Equal(10, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        for (int tick = 0; tick < 375; tick++)
            Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(385, tanker.Height);
        Assert.Equal(TankerState.Climbing, tanker.State);
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease, tanker.UpdateForDevelopment());
        Assert.Equal(386, tanker.Height);
        Assert.Equal(TankerState.CargoReleasePending, tanker.State);
        Assert.Equal(4, tanker.EdgeIndex);
    }

    [Fact]
    public void TankerUsesDevelopmentSpeedApproximationAndCannotDirectlyHitPlayer()
    {
        Tanker tanker = new(7, 0);
        Assert.Equal(1.85f, tanker.ClimbSpeed, 5);
        Assert.False(tanker.HitsPlayer(new Player()));
        Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.InRange(tanker.Height, 11.849f, 11.851f);
        Assert.Equal(7, tanker.EdgeIndex);
    }


    [Fact]
    public void TankerRendererUsesFiniteMagentaGlyphAtProjectedLaneDepth()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        VectorRenderer renderer = new(game);
        Vector2 expected = Projection.Project(
            (game.CurrentLevel.Definition.Points[tanker.EdgeIndex].X +
             game.CurrentLevel.Definition.Points[(tanker.EdgeIndex + 1) % 16].X) / 2,
            (game.CurrentLevel.Definition.Points[tanker.EdgeIndex].Y +
             game.CurrentLevel.Definition.Points[(tanker.EdgeIndex + 1) % 16].Y) / 2,
            Level.Position + Level.Depth - tanker.Height, 400, -80);
        Assert.Equal(expected, renderer.ProjectTankerCenter(tanker));
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTankers((start, end) => lines.Add((start, end)));
        Assert.Equal(16, lines.Count);
        Assert.All(lines, line =>
        {
            Assert.True(float.IsFinite(line.Start.X) && float.IsFinite(line.Start.Y));
            Assert.True(float.IsFinite(line.End.X) && float.IsFinite(line.End.Y));
        });
    }

    // --- Enemy-bullet development scenario (--enemy-bullets-test) ---

    [Fact]
    public void EnemyBulletFlagIsIndependentAndNormalAndSpikerLaunchesDoNotFire()
    {
        Assert.False(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest([]));
        string[] bulletArguments = ["--enemy-bullets-test", "--lives", "10"];
        Assert.True(Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(bulletArguments));
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(bulletArguments));
        string[] combinedArguments = ["--spiker-test", "--enemy-bullets-test", "--lives", "10"];
        TempestGame combined = new(
            Tempest.Game.Debug.DebugController.UseSpikerWave(combinedArguments),
            startingLives: Tempest.Game.Debug.DebugController.GetStartingLives(combinedArguments),
            enemyBulletsTest: Tempest.Game.Debug.DebugController.UseEnemyBulletsTest(combinedArguments));
        Assert.True(combined.CurrentLevel.SpikerDevelopmentWave);
        Assert.True(combined.EnemyBulletsTest);
        Assert.Equal(10, combined.Player.Lives);

        TempestGame normal = new();
        Assert.False(normal.EnemyBulletsTest);
        TempestGame spiker = new(true, new SpikerRandom());
        Assert.False(spiker.EnemyBulletsTest);
        Assert.True(spiker.CurrentLevel.SpikerDevelopmentWave);

        Flipper ordinary = new(2, 0);
        Assert.Equal(0, ordinary.BulletAmmunition);
        Assert.Equal(16, ordinary.BulletTimer);
    }

    [Fact]
    public void FlipperBulletAmmunitionAndTimerPreserveSourceRanges()
    {
        Assert.InRange(new Flipper(0, 0, 1).BulletAmmunition, 1, 3);
        Assert.InRange(new Flipper(0, 0, 2).BulletAmmunition, 1, 3);
        Assert.InRange(new Flipper(0, 0, 3).BulletAmmunition, 1, 3);
        Flipper flipper = new(4, 0, 1);
        Assert.Equal(16, flipper.BulletTimer);

        int decision = 75;
        for (int tick = 0; tick < 16; tick++)
            Assert.Null(flipper.Update(new Level(), 7, (_, _) => decision));
        Assert.Equal(0, flipper.BulletTimer);
        Assert.Null(flipper.Update(new Level(), 7, (_, _) => decision));
        Assert.Equal(16, flipper.BulletTimer);
        Assert.Equal(1, flipper.BulletAmmunition);

        decision = 76;
        for (int tick = 0; tick < 16; tick++)
            Assert.Null(flipper.Update(new Level(), 7, (_, _) => decision));
        EnemyBullet bullet = Assert.IsType<EnemyBullet>(flipper.Update(new Level(), 7, (_, _) => decision));
        Assert.Equal(4, bullet.EdgeIndex);
        Assert.Equal(flipper.Height, bullet.Height);
        Assert.Equal(0, flipper.BulletAmmunition);
        Assert.Equal(16, flipper.BulletTimer);
    }

    [Fact]
    public void FlipperDoesNotSpendTimerOrFireWithoutAmmunition()
    {
        Flipper flipper = new(4, 0);
        for (int tick = 0; tick < 100; tick++)
            Assert.Null(flipper.Update(new Level(), 7, (_, _) => throw new InvalidOperationException()));
        Assert.Equal(0, flipper.BulletAmmunition);
        Assert.Equal(16, flipper.BulletTimer);
    }

    [Fact]
    public void FlipperCanFireAtPauseEdgeTransitionWithoutChangingTransitionTiming()
    {
        Flipper flipper = new(4, 28, 1);
        for (int tick = 0; tick < 186; tick++)
            Assert.Null(flipper.Update(new Level(), 7, (_, _) => 75));
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(0, flipper.BulletTimer);
        for (int tick = 0; tick < 23; tick++)
            Assert.Null(flipper.Update(new Level(), 7, (_, _) => 76));
        Assert.Equal(FlipperState.Pausing, flipper.State);
        EnemyBullet bullet = Assert.IsType<EnemyBullet>(flipper.Update(new Level(), 7, (_, _) => 76));
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(flipper.DestinationEdge, bullet.EdgeIndex);
        Assert.Equal(400, bullet.Height);
    }

    [Fact]
    public void EnemyBulletAdvancesThreeAndExpiresOnlyAfterPassingDepth()
    {
        EnemyBullet bullet = new(3, 394);
        bullet.Advance();
        Assert.Equal(397, bullet.Height);
        Assert.False(bullet.IsExpired);
        Assert.Equal(15, bullet.Rotation);
        bullet = new EnemyBullet(3, 397);
        bullet.Advance();
        Assert.Equal(400, bullet.Height);
        Assert.False(bullet.IsExpired);
        bullet.Advance();
        Assert.Equal(403, bullet.Height);
        Assert.True(bullet.IsExpired);
    }

    [Theory]
    [InlineData(392, false)]
    [InlineData(393, true)]
    [InlineData(400, true)]
    public void EnemyBulletPlayerHitUsesSameLaneAndStrictEightUnitBoundary(float height, bool expected)
    {
        Player player = new();
        Assert.Equal(expected, new EnemyBullet(7, height).HitsPlayer(player));
        Assert.False(new EnemyBullet(6, height).HitsPlayer(player));
    }

    [Theory]
    [InlineData(99, 100, true)]
    [InlineData(100, 100, false)]
    [InlineData(101, 100, false)]
    public void PlayerShotHitsEnemyBulletOnlyBelowItOnTheSameLane(float shotDepth, float bulletDepth, bool expected)
    {
        EnemyBullet bullet = new(8, bulletDepth);
        Assert.Equal(expected, bullet.IsHitBy(new Shot { EdgeIndex = 8, Height = shotDepth }));
        Assert.False(bullet.IsHitBy(new Shot { EdgeIndex = 9, Height = shotDepth }));
    }

    [Fact]
    public void BulletDevelopmentFlagHatchesFlipperWithAmmoAndCreatesBulletAtLaneAndHeight()
    {
        TempestGame game = new(enemyBulletsTest: true, random: new BulletRandom(1, [76]));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(1, flipper.BulletAmmunition);
        Assert.Equal(15, flipper.BulletTimer);
        Step(game, 15);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, flipper.BulletTimer);
        game.RunOneStep();
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(flipper.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(flipper.Height, bullet.Height);
        Assert.Equal(34, bullet.Height);
        Assert.Equal(0, flipper.BulletAmmunition);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void BulletDevelopmentAmmoUsesControlledInclusiveOneToThreeRange(int ammunition)
    {
        TempestGame game = new(enemyBulletsTest: true,
            random: new BulletRandom(ammunition, []));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(ammunition, Assert.Single(game.Flippers).BulletAmmunition);
        Assert.Equal(15, game.Flippers[0].BulletTimer);
    }

    [Fact]
    public void PlayerCanShootEnemyBulletForTwentyFiveWithoutEnemyAccountingChange()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 80 && game.EnemyBullets.Count > 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(25, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void FlipperAndBulletCollisionPriorityUsesDevelopmentInsertionOrder()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        Flipper flipper = Assert.Single(game.Flippers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height - 5;
        game.RunOneStep();
        Assert.Empty(game.Flippers);
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void EnemyBulletPlayerHitUsesNormalDeathAndSingleLifeLoss()
    {
        TempestGame game = PrepareBulletGame(3, 76, 76, 76);
        EnemyBullet firstBullet = game.EnemyBullets[0];
        game.Player.Move(firstBullet.EdgeIndex - game.Player.EdgeIndex, 16);
        int ticks = 0;
        while (game.State == LevelState.Playing && ticks++ < 150)
            game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.NotEmpty(game.EnemyBullets);
        game.Score.Add(125);
        Step(game, 59);
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);

        Assert.Equal(125, game.Score.Score);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);

        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void EnemyBulletsDoNotBlockCompletionAndAreClearedBeforeExit()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        Flipper flipper = Assert.Single(game.Flippers);

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height - 5;
        game.RunOneStep();
        Assert.Contains(bullet, game.EnemyBullets);
        for (int index = 0; index < 9; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ReturnSurvivorsToReserve();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.EnemyBullets);

        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]

    public void BulletDevelopmentDoesNotChangeNormalOrSpikerFlipperAmmunition()
    {
        TempestGame normal = new();
        MakeReady(normal.Eggs[0], normal.CurrentLevel);
        normal.RunOneStep();
        Assert.Equal(0, Assert.Single(normal.Flippers).BulletAmmunition);
        Step(normal, 100);
        Assert.Empty(normal.EnemyBullets);

        TempestGame spikerOnly = new(true, new SpikerRandom { ChooseSpikers = false });
        MakeReady(spikerOnly.Eggs[0], spikerOnly.CurrentLevel);
        spikerOnly.RunOneStep();
        Assert.Equal(0, Assert.Single(spikerOnly.Flippers).BulletAmmunition);
        Assert.False(spikerOnly.EnemyBulletsTest);
    }

    [Fact]
    public void EnemyBulletRenderUsesFiniteLaneDepthAndEightDistinctRadials()
    {
        TempestGame game = PrepareBulletGame(1, 76);
        VectorRenderer renderer = new(game);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        List<(Vector2 Start, Vector2 End)> lines = [];

        renderer.DrawEnemyBullets((start, end) => lines.Add((start, end)));
        Assert.Equal(8, lines.Count);
        foreach ((Vector2 start, Vector2 end) in lines)
        {
            Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
            Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
        }
        float projectedDepth = Level.Position + Level.Depth - bullet.Height;
        float expectedDiameter = 12 / ((projectedDepth + Projection.ZOffset) / Projection.K);
        Assert.InRange(Vector2.Distance(lines[0].Start, lines[0].End),
            expectedDiameter - 0.001f, expectedDiameter + 0.001f);
        Assert.Equal(34, bullet.Height);
        Assert.InRange(renderer.ProjectEnemyBullet(bullet).X, 0, 800);
        Assert.InRange(renderer.ProjectEnemyBullet(bullet).Y, 0, 600);
    }

    // --- Configurable starting lives (--lives) ---

    [Fact]
    public void StartingLivesDefaultRemainsThree()
    {
        Assert.Equal(3, Player.DefaultStartingLives);
        Assert.Equal(3, new Player().Lives);
        Assert.Equal(3, new TempestGame().Player.Lives);
        Assert.Equal(3, Tempest.Game.Debug.DebugController.GetStartingLives([]));
    }

    [Fact]
    public void LivesCommandLineOptionWorksAloneAndWithSpikerWave()
    {
        string[] livesArguments = ["--lives", "10"];
        Assert.Equal(10, Tempest.Game.Debug.DebugController.GetStartingLives(livesArguments));
        Assert.Equal(10, new TempestGame(startingLives:
            Tempest.Game.Debug.DebugController.GetStartingLives(livesArguments)).Player.Lives);

        string[] combinedArguments = ["--spiker-test", "--lives", "20"];
        TempestGame game = new(
            Tempest.Game.Debug.DebugController.UseSpikerWave(combinedArguments),
            startingLives: Tempest.Game.Debug.DebugController.GetStartingLives(combinedArguments));
        Assert.Equal(20, game.Player.Lives);
        Assert.True(game.CurrentLevel.SpikerDevelopmentWave);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("0")]
    [InlineData("-3")]
    [InlineData("999999999999999999")]
    public void InvalidLivesCommandLineValuesFallBackToThree(string value)
    {
        string[] arguments = ["--lives", value];
        Assert.Equal(Player.DefaultStartingLives,
            Tempest.Game.Debug.DebugController.GetStartingLives(arguments));
        Assert.Equal(Player.DefaultStartingLives,
            new TempestGame(startingLives:
                Tempest.Game.Debug.DebugController.GetStartingLives(arguments)).Player.Lives);
    }

    [Fact]
    public void MissingLivesValueFallsBackToThree()
    {
        string[] arguments = ["--lives"];
        Assert.Equal(Player.DefaultStartingLives,
            Tempest.Game.Debug.DebugController.GetStartingLives(arguments));
        Assert.Equal(Player.DefaultStartingLives,
            new TempestGame(startingLives:
                Tempest.Game.Debug.DebugController.GetStartingLives(arguments)).Player.Lives);
    }

    [Fact]
    public void GameOverRestartRestoresSelectedStartingLives()
    {
        TempestGame game = PreparePivotWithStartingLives(10);
        game.Player.Lives = 1;
        Step(game, 63);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
    }

    [Fact]
    public void LifeLossStillSubtractsExactlyOneFromSelectedStartingLives()
    {
        TempestGame game = PreparePivotWithStartingLives(10);
        game.Score.Add(125);
        Step(game, 63);
        Assert.False(game.GameOver);

        Assert.Equal(9, game.Player.Lives);
        Assert.Equal(125, game.Score.Score);
        Assert.Equal(LevelState.Playing, game.State);
    }

    // --- Spiker/spike development scenario (--spiker-test) and spike-exit regressions ---

    [Fact]
    public void FullDevelopmentWaveCanBeDefeatedAndExitedWithEnemyAccountingIntact()
    {
        SpikerRandom random = new();
        TempestGame game = new(true, random);
        bool sawSpiker = false;
        bool sawFlipper = false;

        for (int tick = 0; tick < 12000 && !game.GameOver && !game.LevelComplete; tick++)
        {
            sawSpiker |= game.Spikers.Count > 0;
            sawFlipper |= game.Flippers.Count > 0;
            if (sawSpiker)
                random.ChooseSpikers = false;
            Enemy? target = game.Flippers.Cast<Enemy>().Concat(game.Spikers)
                .OrderByDescending(enemy => enemy.Height).FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);
            else if (game.IsExiting)
            {
                int safeLane = Enumerable.Range(0, 16)
                    .OrderBy(lane => game.Spikes.FirstOrDefault(spike => spike.EdgeIndex == lane)?.Height ?? 0)
                    .First();
                game.Player.Move(safeLane - game.Player.EdgeIndex, 16);
            }
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertSpikerAccounting(game);
            Assert.True(game.Score.Score >= game.CurrentLevel.EnemiesDestroyed * 50);
        }
        Assert.True(sawSpiker);
        Assert.True(sawFlipper);
        Assert.True(game.LevelComplete,
            $"State={game.State}, lives={game.Player.Lives}, destroyed={game.CurrentLevel.EnemiesDestroyed}, " +
            $"reserve={game.CurrentLevel.ReserveEggs}, eggs={game.Eggs.Count}, flippers={game.Flippers.Count}, spikers={game.Spikers.Count}");
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void SpikerDevelopmentSelectionIsExplicitAndDefaultLevelOneIsUnchanged()
    {
        Assert.False(Tempest.Game.Debug.DebugController.UseSpikerWave([]));
        Assert.True(Tempest.Game.Debug.DebugController.UseSpikerWave(["--spiker-test"]));
        TempestGame normal = new();
        Assert.Empty(normal.Spikes);
        Assert.Empty(normal.Spikers);
        TempestGame development = new(true, new SpikerRandom());
        Assert.Equal(16, development.Spikes.Count);
        Assert.All(development.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(4, development.Eggs.Count);
        Assert.Equal(6, development.CurrentLevel.ReserveEggs);
        AssertSpikerAccounting(development);
    }

    [Fact]
    public void SpikerHatchesOnEggLaneWithNoMovementOnFirstUpdate()
    {
        TempestGame game = new(true, new SpikerRandom());
        Egg egg = game.Eggs[0];
        int lane = egg.EdgeIndex;
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        Assert.Equal(lane, spiker.EdgeIndex);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(SpikerState.Building, spiker.State);
        game.RunOneStep();
        Assert.Equal(3, spiker.Height);
        Assert.Equal(lane, spiker.EdgeIndex);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void SpikerInitialUpdateDoesNotCreateSpikeAndNextUpdateCreatesAtOwnHeight()
    {
        Level level = new();
        Spiker spiker = new(3, 5);
        Assert.Equal(SpikerState.Initializing, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(0, spiker.Height);
        Assert.Empty(level.Spikes);
        spiker.Update(level, () => false);
        Spike spike = Assert.Single(level.Spikes);
        Assert.Equal(3, spike.EdgeIndex);
        Assert.Equal(5, spike.Height);
        spiker.Update(level, () => false);
        Assert.Same(spike, Assert.Single(level.Spikes));
        Assert.Equal(10, spiker.Height);
        Assert.Equal(6, spike.Height);
    }

    [Fact]
    public void SpikerExistingSpikeAssignmentCanShortenAndDoesNotAlwaysGrow()
    {
        Level level = new();
        Spike spike = level.CreateSpike(2, 60);
        Spiker spiker = new(2, 3);
        for (int tick = 0; tick < 21; tick++)
            spiker.Update(level, () => false);
        Assert.Equal(60, spiker.Height);
        Assert.Equal(60, spike.Height);
        spiker.Update(level, () => false);
        Assert.Same(spike, Assert.Single(level.Spikes));
        Assert.Equal(63, spiker.Height);
        Assert.Equal(59, spike.Height);
        Assert.Equal(3, spiker.Growth);
    }

    [Theory]
    [InlineData(true, SpikerState.Retreating)]
    [InlineData(false, SpikerState.Building)]
    public void SpikerRetreatDecisionOccursOnlyAfterMoreThanOneHundredGrowth(bool retreat, SpikerState expected)
    {
        Level level = new();
        level.CreateSpike(0, 0);
        Spiker spiker = new(0, 5);
        spiker.Update(level, () => throw new InvalidOperationException());
        for (int tick = 0; tick < 20; tick++)
            spiker.Update(level, () => throw new InvalidOperationException());
        Assert.Equal(100, spiker.Growth);
        int decisions = 0;
        spiker.Update(level, () => { decisions++; return retreat; });
        Assert.Equal(1, decisions);
        Assert.Equal(105, spiker.Growth);
        Assert.Equal(expected, spiker.State);
    }

    [Fact]
    public void SpikerUpperThresholdRetreatsOnFollowingUpdateAndRemovesAfterCrossingZero()
    {
        Level level = new();
        Spiker spiker = new(1, 3);
        spiker.Update(level, () => false);
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(level, () => false);
        Assert.Equal(396, spiker.Height);
        Assert.Equal(SpikerState.Building, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(396, spiker.Height);
        Assert.Equal(SpikerState.Retreating, spiker.State);
        Spike spike = Assert.Single(level.Spikes);
        float spikeHeight = spike.Height;
        spiker.Update(level, () => false);
        Assert.InRange(spiker.Height, 393.299f, 393.301f);
        while (spiker.Height > 0)
            spiker.Update(level, () => false);
        Assert.Equal(SpikerState.Retreating, spiker.State);
        spiker.Update(level, () => false);
        Assert.Equal(SpikerState.ReturnedToReserve, spiker.State);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(spikeHeight, spike.Height);
    }

    [Fact]
    public void SpikerRuntimeRetreatReturnsExactlyOneReserveEnemyAndPreservesSpike()
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = Assert.Single(game.Spikers);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        for (int tick = 0; tick < 133; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        while (spiker.Height > 0)
            spiker.Update(game.CurrentLevel, () => false);
        int reserve = game.CurrentLevel.ReserveEggs;
        game.RunOneStep();
        Assert.Empty(game.Spikers);
        Assert.Equal(reserve + 1, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Contains(spike, game.Spikes);
        AssertSpikerAccounting(game);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, true)]
    [InlineData(1, false)]
    public void SpikerAndSpikeUseSameLaneStrictDepthComparisons(float difference, bool expected)
    {
        Level level = new();
        Spiker spiker = new(4, 3);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        Spike spike = Assert.Single(level.Spikes);
        Shot shot = new() { EdgeIndex = 4, Height = spiker.Height + difference };
        Assert.Equal(expected, spiker.IsHitBy(shot));
        Assert.Equal(expected, spike.IsHitBy(shot));
        Shot wrongLane = new() { EdgeIndex = 5, Height = 0 };
        Assert.False(spiker.IsHitBy(wrongLane));
        Assert.False(spike.IsHitBy(wrongLane));
    }

    [Fact]
    public void SpikerShotConsumesShotAwardsFiftyAndKeepsSpike()
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = game.Spikers[0];
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 60;
        game.Player.Move(spiker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikers);
        Assert.Equal(50, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(60, spike.Height);
        AssertSpikerAccounting(game);
    }

    [Theory]
    [InlineData(400, 390, false)]
    [InlineData(10, 0, true)]
    [InlineData(5, -5, true)]
    public void SpikeShotShortensExactlyTenAwardsOneAndDeletesWhenDepleted(float height, float expected, bool deleted)
    {
        TempestGame game = new(true, new SpikerRandom());
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = height;
        float initialDepth = height > 394 ? 400 : height + 5;
        game.Player.AdvanceExit(400 - initialDepth);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(expected, spike.Height);
        Assert.Equal(1, game.Score.Score);
        Assert.Empty(game.Shots);
        Assert.Equal(deleted, !game.Spikes.Contains(spike));
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SpikerVersusSpikePriorityFollowsInsertionNotProximity(bool replacement)
    {
        TempestGame game = HatchDevelopmentSpiker();
        Spiker spiker = game.Spikers[0];
        for (int tick = 0; tick < 132; tick++)
            spiker.Update(game.CurrentLevel, () => false);
        Spike initial = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        initial.Height = replacement ? 1 : 395;
        game.Player.Move(spiker.EdgeIndex - game.Player.EdgeIndex, 16);
        if (replacement)
        {
            // Deplete the initial spike through production shots, then recreate after the Spiker.
            game.Player.AdvanceExit(394);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            game.Player.ResetDepth();
            game.CurrentLevel.CreateSpike(spiker.EdgeIndex, 395);
            Step(game, 2);
            game.Player.AdvanceExit(400 - (spiker.Height + 5));
        }
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Empty(game.Shots);
        if (replacement)
        {
            Assert.Empty(game.Spikers);
            Assert.Equal(51, game.Score.Score);
        }
        else
        {
            Assert.Single(game.Spikers);
            Assert.Equal(1, game.Score.Score);
            Assert.Equal(385, initial.Height);
        }
    }

    [Fact]
    public void DevelopmentWaveCanContainBothTypesAndKeepsAccountingAcrossDeath()
    {
        SpikerRandom random = new();
        TempestGame game = new(true, random);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        random.ChooseSpikers = false;
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Spikers);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
        Spike spike = game.Spikes[0];
        spike.Height = 100;
        game.Player.Move(spike.EdgeIndex - game.Player.EdgeIndex, 16);
        game.Player.AdvanceExit(350);
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
        AssertSpikerAccounting(game);
        Step(game, 60);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(100, spike.Height);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void DevelopmentLastEnemiesAreFlippersSoRetreatCannotPreventCompletion()
    {
        TempestGame game = new(true, new SpikerRandom());
        for (int index = 0; index < 8; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Empty(game.Spikers);
        Assert.Single(game.Flippers);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void ResidualSpikesAllowClearAndAvoidanceLastsOneHundredFiftyOneUpdates()
    {
        TempestGame game = BeginSpikerExit();
        Assert.Equal(16, game.Spikes.Count);
        Step(game, 150);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Step(game, 20);
        Assert.Equal(LevelState.ExitDelay, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SpikeExitAllowsMovementAndShotsFromTravelDepth(bool zoom)
    {
        TempestGame game = BeginSpikerExit();
        if (zoom)
            Step(game, 151 + 21 + 10);
        float depth = game.Player.Depth;
        int lane = game.Player.EdgeIndex;
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.True(game.SpikeExitControlsEnabled);
    }

    [Fact]
    public void SpikeTravelCollisionUsesStrictDepthAndLaneAndCostsOneLifeBeforeRetry()
    {
        TempestGame game = BeginSpikerExit();
        Step(game, 151 + 21);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 397;
        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(397, game.Player.Depth);
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(60, game.DeathTicks);
        Step(game, 59);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(1, game.DeathTicks);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(400, game.Player.Depth);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(397, spike.Height);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        AssertSpikerAccounting(game);
        game.Player.Move(1, 16);
        foreach (Spike other in game.Spikes.Where(other => other != spike))
            other.Height = 0;
        for (int tick = 0; tick < 400 && !game.LevelComplete; tick++)
            game.RunOneStep();
        Assert.True(game.LevelComplete);
        Assert.Equal(2, game.Player.Lives);
    }

    [Fact]
    public void SpikeReverseZoomKeepsDeathDepthUntilOffsetRestoresAndHandlesGameOver()
    {
        TempestGame game = BeginSpikerExit();
        game.Player.Lives = 1;
        Step(game, 151 + 21 + 100);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = game.Player.Depth;
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        float depth = game.Player.Depth;
        Step(game, 46);
        Assert.Equal(depth, game.Player.Depth);
        game.RunOneStep();
        Assert.Equal(400, game.Player.Depth);
        Step(game, 13);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Step(game, 100);
        Assert.Equal(0, game.Player.Lives);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(3, game.Player.Lives);
        Assert.All(game.Spikes, candidate => Assert.Equal(60, candidate.Height));
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void DevelopmentReplayRestoresInitialSpikesAndEggsButPreservesScoreAndLives()
    {
        TempestGame game = BeginSpikerExit();
        foreach (Spike spike in game.Spikes)
            spike.Height = 0;
        game.Score.Add(501);
        game.Player.Lives = 2;
        Step(game, 400);
        Assert.True(game.LevelComplete);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(501, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Player.Depth);
        AssertSpikerAccounting(game);
    }

    [Fact]
    public void SpikerAndSpikeRenderSubmissionsAreFiniteRotateAndPersistDuringExit()
    {
        TempestGame game = HatchDevelopmentSpiker();
        VectorRenderer renderer = new(game);
        List<(Vector2, Vector2)> first = [];
        renderer.DrawSpikers((start, end) => first.Add((start, end)));
        Assert.Equal(14, first.Count);
        game.RunOneStep();
        List<(Vector2, Vector2)> second = [];
        renderer.DrawSpikers((start, end) => second.Add((start, end)));
        Assert.NotEqual(first[1], second[1]);
        foreach ((Vector2 start, Vector2 end) in first.Concat(second))
        {
            AssertFinite(start);
            AssertFinite(end);
        }
        game = BeginSpikerExit();
        renderer = new VectorRenderer(game);
        for (int tick = 0; tick < 400 && !game.GameOver; tick++)
        {
            int lines = 0;
            int tips = 0;
            renderer.DrawSpikes((start, end) => { AssertFinite(start); AssertFinite(end); lines++; },
                tip => { AssertFinite(tip); tips++; });
            Assert.Equal(16, lines);
            Assert.Equal(16, tips);
            game.RunOneStep();
        }
    }

    // --- Shared helpers and deterministic Random test doubles ---

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private static TempestGame CreateTankerWave(
        int startingLives = 3, bool enemyBulletsTest = false, Random? random = null)
    {
        TempestGame game = new(startingLives: startingLives, random: random,
            enemyBulletsTest: enemyBulletsTest, tankerTest: true);
        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Tankers);
        return game;
    }

    private static Tanker CreatePendingTanker(int lane)
    {
        Tanker tanker = new(lane, 0, climbSpeed: 1);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();
        Assert.Equal(386, tanker.Height);
        return tanker;
    }

    private static Egg? FindOrReleaseOriginalEgg(TempestGame game, int slotId)
    {
        for (int tick = 0; tick < 5000; tick++)
        {
            Egg? egg = game.Eggs.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            if (egg is not null)
                return egg;
            game.RunOneStep();
        }
        return null;
    }

    private static void DestroyOriginalFlipperSlot(TempestGame game, int slotId)
    {
        int remainingBefore = game.TankerOriginalSlotsRemaining;
        for (int attempt = 0; attempt < 4 && game.TankerOriginalSlotsRemaining == remainingBefore; attempt++)
        {
            Flipper? flipper = game.Flippers.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            if (flipper is null)
            {
                Egg? egg = FindOrReleaseOriginalEgg(game, slotId);
                Assert.NotNull(egg);
                MakeReady(egg!, game.CurrentLevel);
                game.RunOneStep();
                flipper = game.Flippers.FirstOrDefault(candidate => candidate.OriginalSlotId == slotId);
            }
            if (flipper is not null)
                ShootDevelopmentFlipper(game, flipper);
        }
        Assert.Equal(remainingBefore - 1, game.TankerOriginalSlotsRemaining);
    }

    private static void ShootTanker(TempestGame game, Tanker tanker)
    {
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();
        Assert.DoesNotContain(tanker, game.Tankers);
    }

    private static Flipper HatchOneOrdinaryTankerWaveFlipper(TempestGame game)
    {
        Egg egg = Assert.Single(game.Eggs, candidate => candidate.OriginalSlotId == 1);
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        return Assert.Single(game.Flippers, flipper => !flipper.IsTankerCargo);
    }

    private static void ForceFlipperPivotIntoPlayer(TempestGame game, Flipper flipper)
    {
        int ticks = 0;
        while (!flipper.CompletedIntoPlayer && ticks++ < 2000)
        {
            if (flipper.State == FlipperState.Flipping)
                game.Player.Move(flipper.DestinationEdge - game.Player.EdgeIndex, 16);
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex);
        }
        Assert.True(flipper.CompletedIntoPlayer);
    }

    private static void ShootDevelopmentFlipper(TempestGame game, Flipper flipper)
    {
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height + 5;
        game.RunOneStep();
        Assert.DoesNotContain(flipper, game.Flippers);
    }

    private static void AssertTankerAccounting(TempestGame game)
    {
        Assert.Equal(Level.EnemyTotal, game.TankerOriginalSlotsDestroyed + game.TankerOriginalSlotsEscaped +
            game.TankerOriginalSlotsRemaining);
        Assert.Equal(Level.EnemyTotal + game.TankerMaterializedCargoUnits, game.TankerAccountingTotal);
        Assert.Equal(game.TankerCargoUnitsResolved + game.TankerCargoUnitsRemaining,
            game.TankerMaterializedCargoUnits + game.TankerLatentCargoUnits);
        Assert.InRange(game.CurrentLevel.ReserveEggs, 0, Level.EnemyTotal);
        Assert.InRange(game.TankerOriginalSlotsRemaining, 0, Level.EnemyTotal);
        Assert.InRange(game.TankerCargoUnitsRemaining, 0, 2);
    }

    private static Vector2 ProjectTanker(TempestGame game, Tanker tanker)
    {
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        Vector2 center = (points[tanker.EdgeIndex] + points[(tanker.EdgeIndex + 1) % points.Count]) / 2;
        return Projection.Project(center.X, center.Y, Level.Position + Level.Depth - tanker.Height, 400, -80);
    }

    private static TempestGame PrepareBulletGame(int ammunition, params int[] firingDecisions)
    {
        TempestGame game = new(enemyBulletsTest: true,
            random: new BulletRandom(ammunition, firingDecisions));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Flippers);
        Step(game, 15);
        Assert.Empty(game.EnemyBullets);
        game.RunOneStep();
        Assert.Single(game.EnemyBullets);
        return game;
    }

    private static void AssertSpikerAccounting(TempestGame game) =>
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.ReserveEggs +
            game.Eggs.Count + game.Flippers.Count + game.Spikers.Count);

    private static TempestGame HatchDevelopmentSpiker()
    {
        TempestGame game = new(true, new SpikerRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Spikers);
        return game;
    }

    private static TempestGame BeginSpikerExit()
    {
        TempestGame game = new(true, new SpikerRandom());
        for (int index = 0; index < 10; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        return game;
    }

    private sealed class SpikerRandom : Random
    {
        public bool ChooseSpikers { get; set; } = true;
        public override int Next(int minValue, int maxValue) => ChooseSpikers ? minValue : maxValue - 1;
        public override double NextDouble() => 0;
    }

    private sealed class BulletRandom : Random
    {
        private readonly Queue<int> values;

        public BulletRandom(int ammunition, IEnumerable<int> firingDecisions)
        {
            values = new Queue<int>(new[] { ammunition }.Concat(firingDecisions));
        }

        public override int Next(int minValue, int maxValue)
        {
            if (values.Count == 0)
                return minValue;

            return Math.Clamp(values.Dequeue(), minValue, maxValue - 1);
        }
    }

    // --- Normal Level 1 exit/zoom, projection and production rendering regressions ---

    [Fact]
    public void NormalPlayUsesEstablishedProjectionAndNearRimDepth()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(50, renderer.PlayerProjectedDepth);
        Assert.Equal(20, renderer.PlayerClawHeight);
        Step(game, 100);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(400, game.Player.Depth);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ExitCannotStartWithReserveOrActiveEggs(bool reserveOnly)
    {
        TempestGame game = new();
        if (reserveOnly)
            game.CurrentLevel.ReturnSurvivorsToReserve();
        Step(game, 49);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.LevelComplete);
        Assert.Equal(400, game.Player.Depth);
        if (reserveOnly)
            Assert.Equal(10, game.CurrentLevel.ReserveEggs);
        else
            Assert.Equal(4, game.Eggs.Count);
    }

    [Fact]
    public void ExitCannotStartWithTheFinalActiveFlipper()
    {
        TempestGame game = PrepareFinalEnemy();
        Assert.Single(game.Flippers);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Step(game, 50);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void ExitPhaseOneChangesOffsetAndDepthForExactlyTwentyThreeTicks()
    {
        TempestGame game = BeginExit();
        Step(game, 21);
        Assert.Equal(LevelState.ExitZoom, game.State);
        for (int tick = 1; tick <= 23; tick++)
        {
            game.RunOneStep();
            Assert.Equal(5 - tick * 2, Projection.ZOffset);
            Assert.Equal(400 - tick * 3, game.Player.Depth);
            Assert.Equal(70, Projection.K);
        }
        Assert.Equal(-41, Projection.ZOffset);
        game.RunOneStep();
        Assert.Equal(-41, Projection.ZOffset);
        Assert.Equal(72, Projection.K);
        Assert.InRange(game.Player.Depth, 327.799f, 327.801f);
    }

    [Fact]
    public void ExitPhaseTwoAcceleratesKAfterOneHundred()
    {
        TempestGame game = BeginExit();
        Step(game, 21 + 23);
        for (int tick = 1; tick <= 15; tick++)
        {
            game.RunOneStep();
            Assert.Equal(70 + tick * 2, Projection.K);
            Assert.InRange(game.Player.Depth, 331 - tick * 3.2f - .001f, 331 - tick * 3.2f + .001f);
        }
        Assert.Equal(100, Projection.K);
        game.RunOneStep();
        Assert.Equal(104, Projection.K);
        game.RunOneStep();
        Assert.Equal(108, Projection.K);
        Assert.Equal(-41, Projection.ZOffset);
    }

    [Fact]
    public void ExitZoomIgnoresInputRestartAndFurtherClearTriggers()
    {
        TempestGame game = BeginExit();
        int edge = game.Player.EdgeIndex;
        float position = game.Player.EdgePosition;
        PlayerInput input = new(true, 800, true, true);
        int ticks = 0;
        while (!game.LevelComplete && ticks++ < 200)
        {
            game.RunOneStep(input, restartPressed: true);
            Assert.Equal(edge, game.Player.EdgeIndex);
            Assert.Equal(position, game.Player.EdgePosition);
            Assert.Empty(game.Shots);
            Assert.Empty(game.Eggs);
            Assert.Empty(game.Flippers);
            Assert.Equal(0, game.CurrentLevel.ReserveEggs);
            Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
            Assert.Equal(0, game.DeathTicks);
            Assert.False(game.GameOver);
            Assert.Equal(3, game.Player.Lives);
        }
        Assert.True(game.LevelComplete);
        Assert.Equal(145, ticks);
    }

    [Fact]
    public void ZoomCompletesOnlyBelowTenThenRestoresProjectionAndFreezes()
    {
        TempestGame game = BeginExit();
        Step(game, 21 + 23 + 100);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);
        game.RunOneStep();
        Assert.Equal(LevelState.Complete, game.State);
        Assert.InRange(game.Player.Depth, 7.79f, 7.81f);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        float depth = game.Player.Depth;
        Step(game, 50, new PlayerInput(true, 80, false, false));
        Assert.Equal(depth, game.Player.Depth);
        Assert.True(game.LevelComplete);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void ExitReplayRestoresProjectionDepthEggsAndCountersButKeepsScoreLives()
    {
        TempestGame game = BeginExit();
        game.Score.Add(1500);
        game.Player.Lives = 2;
        FinishExit(game);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        AssertAccounting(game);
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Step(game, 20);
        Assert.Equal(LevelState.ExitDelay, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(400, game.Player.Depth);
    }

    [Fact]
    public void UndestroyedFinalFlipperStillKillsPlayerAtCompletedPivot()
    {
        TempestGame game = PrepareFinalEnemy();
        Step(game, 190);
        game.Player.Move(-1, 16);
        Step(game, 9 + 24 + 3);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(60, game.DeathTicks);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.False(game.IsExiting);
        AssertAccounting(game);
        Step(game, 60);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Player.Depth);
        Assert.Single(game.Eggs);
        AssertAccounting(game);
    }

    [Fact]
    public void FinalPivotTickKillStartsExitInsteadOfKillingPlayer()
    {
        TempestGame game = PrepareFinalEnemy();
        Step(game, 190);
        game.Player.Move(-1, 16);
        Step(game, 9 + 24);
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(game.Player.EdgeIndex, flipper.DestinationEdge);
        Step(game, 2);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void ExitRendererTracksPlayerDepthAndAllProjectedVerticesStayFinite()
    {
        TempestGame game = BeginExit();
        VectorRenderer renderer = new(game);
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        for (int tick = 0; tick < 146; tick++)
        {
            float expectedDepth = Level.Position + Level.Depth - game.Player.Depth;
            Assert.Equal(expectedDepth, renderer.PlayerProjectedDepth);
            renderer.GetPlayerEdge(points, out Vector2 first, out Vector2 second);
            Assert.Equal(Projection.Project(points[game.Player.EdgeIndex].X,
                points[game.Player.EdgeIndex].Y, expectedDepth, 400, -80), first);
            Assert.True(float.IsFinite(first.X) && float.IsFinite(first.Y));
            Assert.True(float.IsFinite(second.X) && float.IsFinite(second.Y));
            Assert.True(float.IsFinite(renderer.PlayerClawHeight));
            Assert.True(renderer.PlayerClawHeight > 0);
            foreach (Vector2 point in points)
            {
                Vector2 near = Projection.Project(point.X, point.Y, Level.Position, 400, -80);
                Vector2 far = Projection.Project(point.X, point.Y, Level.Position + Level.Depth, 400, -80);
                Assert.True(float.IsFinite(near.X) && float.IsFinite(near.Y));
                Assert.True(float.IsFinite(far.X) && float.IsFinite(far.Y));
            }
            game.RunOneStep();
        }
        Assert.True(game.LevelComplete);
        Assert.True(renderer.PlayerClawHeight < 20);
    }

    [Fact]
    public void ExitDelayWaitsTwentyOneUpdatesAndIgnoresInput()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.LevelComplete);
        PlayerInput input = new(true, 80, true, true);
        for (int tick = 0; tick < 20; tick++)
        {
            game.RunOneStep(input, restartPressed: true);
            Assert.Equal(LevelState.ExitDelay, game.State);
            Assert.Equal(7, game.Player.EdgeIndex);
            Assert.Equal(0.5f, game.Player.EdgePosition);
            Assert.Equal(400, game.Player.Depth);
            Assert.Empty(game.Shots);
        }
        game.RunOneStep(input, restartPressed: true);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(400, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        game.RunOneStep(input, restartPressed: true);
        Assert.Equal(397, game.Player.Depth);
        Assert.Equal(3, Projection.ZOffset);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void ProductionInitializationAndFirstTickRetainRenderFacingEggs()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.Same(game.CurrentLevel.Eggs, game.Eggs);
        Assert.Same(game.Eggs, renderer.Eggs);
        Egg[] initial = game.Eggs.ToArray();
        game.RunOneStep();
        Assert.Equal(initial, renderer.Eggs);
        Assert.All(renderer.Eggs, egg =>
        {
            Assert.Equal(EggState.Roaming, egg.State);
            Assert.Equal(1, egg.UpdateCounter);
            Assert.Equal(-10, egg.Height);
        });
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void ProductionEggProjectionAndDrawSubmissionAreInsideWindow()
    {
        float previousK = Projection.K;
        float previousZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            TempestGame game = new();
            VectorRenderer renderer = new(game);
            List<Vector2> submitted = [];
            renderer.DrawEggs((point, size) =>
            {
                submitted.Add(point);
                Assert.Equal(new Vector2(3, 3), size);
                Assert.InRange(point.X + size.X, 0, 800);
                Assert.InRange(point.Y + size.Y, 0, 600);
            });
            Assert.Equal(4, submitted.Count);
            for (int eggIndex = 0; eggIndex < game.Eggs.Count; eggIndex++)
            {
                Egg egg = game.Eggs[eggIndex];
                IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
                Vector2 midpoint = (points[egg.EdgeIndex] + points[(egg.EdgeIndex + 1) % points.Count]) / 2;
                Vector2 expected = Projection.Project(midpoint.X, midpoint.Y, 650, 400, -80);
                Assert.Equal(expected, renderer.ProjectEgg(egg));
                Assert.Equal(expected - Vector2.One, submitted[eggIndex]);
                Assert.True(float.IsFinite(expected.X) && float.IsFinite(expected.Y));
                Assert.InRange(expected.X, 0, 799);
                Assert.InRange(expected.Y, 0, 599);
            }
        }
        finally
        {
            Projection.K = previousK;
            Projection.ZOffset = previousZOffset;
        }
    }

    [Fact]
    public void ProductionTimerCannotSpawnFlipperBeforeReadyEggConversion()
    {
        TempestGame game = new();
        for (int tick = 1; tick <= 64; tick++)
        {
            game.RunOneStep();
            Assert.Empty(game.Flippers);
            Assert.Equal(4, game.Eggs.Count);
            Assert.DoesNotContain(game.Eggs, egg => egg.State == EggState.ReadyToHatch);
        }
        Egg approaching = Assert.Single(game.Eggs, egg => egg.State == EggState.Approaching);
        Assert.Equal(-2, approaching.Height);
        Assert.Equal(7, approaching.UpdateCounter);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.DoesNotContain(approaching, game.Eggs);
        Assert.Equal(EggState.ReadyToHatch, approaching.State);
        Assert.Equal(0, approaching.Height);
        Assert.Equal(approaching.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(2, flipper.Height);
    }

    // --- Player movement, input and player-shot basics ---

    [Fact]
    public void PlayerPreservesExactBoundaryAndWrapsBothDirections()
    {
        Player player = new();
        player.Move(0.5f, 16);
        Assert.Equal(7, player.EdgeIndex);
        Assert.Equal(1, player.EdgePosition);
        player.Move(0.25f, 16);
        Assert.Equal(8, player.EdgeIndex);
        Assert.Equal(0.25f, player.EdgePosition);
        player.Move(-9, 16);
        Assert.Equal(15, player.EdgeIndex);
        Assert.Equal(0.25f, player.EdgePosition);
    }

    [Fact]
    public void FiringPrecedesMovementAndNewShotMovesImmediately()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Shot shot = Assert.Single(game.Shots);
        Assert.Equal(7, shot.EdgeIndex);
        Assert.Equal(394, shot.Height);
        Assert.Equal(15, shot.Rotation);
        Assert.Equal(8, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
    }

    [Fact]
    public void KeyboardAndSpinnerAccumulateWithOriginalSigns()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(false, -8, true, false));
        Assert.Equal(0.5f + -8 * 0.0125f - 0.05f, game.Player.EdgePosition);
        game.RunOneStep(new PlayerInput(false, 8, false, true));
        Assert.Equal(0.5f, game.Player.EdgePosition);
    }

    [Fact]
    public void FiringUsesThreeTickCooldownAndTenShotLimit()
    {
        TempestGame game = new();
        PlayerInput firing = new(true, 0, false, false);
        game.RunOneStep(firing);
        Step(game, 2, firing);
        Assert.Single(game.Shots);
        game.RunOneStep(firing);
        Assert.Equal(2, game.Shots.Count);
        Step(game, 30, firing);
        Assert.Equal(10, game.Shots.Count);
    }

    [Fact]
    public void ShotsRotateAndExpireOnlyBelowZero()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Step(game, 65);
        Assert.Equal(4, Assert.Single(game.Shots).Height);
        game.RunOneStep();
        Assert.Empty(game.Shots);
    }

    // --- Normal Level 1 spawn sequence, collision, death and replay regressions ---

    [Fact]
    public void SpawnSequenceAndIntervalAreDeterministic()
    {
        Level level = new();
        Assert.Equal(new[] { 12, 4, 9, 1 }, level.Eggs.Select(egg => egg.EdgeIndex));
        foreach (Egg egg in level.Eggs)
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
        }
        while (level.HatchReadyEgg() is not null) { }
        int[] expected = [14, 6, 10, 2, 15, 7];
        foreach (int lane in expected)
        {
            int released = level.EnemiesSpawned;
            for (int tick = 0; tick < 49; tick++)
                level.UpdateEggRelease();
            Assert.Equal(released, level.EnemiesSpawned);
            level.UpdateEggRelease();
            Egg egg = Assert.Single(level.Eggs);
            Assert.Equal(lane, egg.EdgeIndex);
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
            Assert.NotNull(level.HatchReadyEgg());
        }
        Assert.Equal(10, level.EnemiesSpawned);
        Assert.Equal(0, level.ReserveEggs);
    }

    [Fact]
    public void EggReleaseRespectsActiveLimitAndDeathRestoresPendingPool()
    {
        Level level = new();
        for (int tick = 0; tick < 100; tick++)
            level.UpdateEggRelease();
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(6, level.ReserveEggs);
        level.RecordEnemyDestroyed();
        level.RecordEnemyDestroyed();
        level.ResetAfterDeath();
        Assert.Equal(6, level.EnemiesSpawned);
        Assert.Equal(4, level.ReserveEggs);
        Assert.Equal(new[] { 9, 1, 14, 6 }, level.Eggs.Select(egg => egg.EdgeIndex));
    }

    [Fact]
    public void HatchedFlipperClimbsOnItsHatchTick()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        Assert.Empty(game.Flippers);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(egg.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(2, flipper.Height);
        Assert.DoesNotContain(egg, game.Eggs);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(15, true)]
    [InlineData(16, false)]
    public void ShotCollisionPreservesStrictDepthBoundaries(float separation, bool hit)
    {
        Flipper flipper = new(6, 400);
        Assert.Equal(hit, flipper.IsHitBy(new Shot { EdgeIndex = 6, Height = 400 - separation }));
        Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 5, Height = 400 - separation }));
    }

    [Fact]
    public void FlipperRemainsShootableOnBothLanesThroughoutVisiblePivot()
    {
        Level level = new();
        Flipper flipper = new(6, 400);
        flipper.Update(level, 7);
        Assert.Equal(24, flipper.PauseTicks);
        for (int tick = 0; tick < 23; tick++)
            flipper.Update(level, 7);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        flipper.Update(level, 7);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(7, flipper.DestinationEdge);

        int visibleTicks = 0;
        while (!flipper.CompletedIntoPlayer && visibleTicks < 10)
        {
            Assert.True(flipper.IsHitBy(new Shot { EdgeIndex = 6, Height = 394 }));
            Assert.True(flipper.IsHitBy(new Shot { EdgeIndex = 7, Height = 394 }));
            Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 8, Height = 394 }));
            flipper.Update(level, 7);
            visibleTicks++;
        }
        Assert.Equal(3, visibleTicks);
        Assert.True(flipper.CompletedIntoPlayer);
    }

    [Fact]
    public void PlayerCanMoveUnderPivotAndLeaveBeforeCompletion()
    {
        TempestGame game = PreparePivot();
        Flipper flipper = game.Flippers[0];
        game.Player.Move(1, 16);
        game.RunOneStep(new PlayerInput(false, -80, false, false));
        Assert.Equal(flipper.DestinationEdge, game.Player.EdgeIndex);
        Assert.Equal(0, game.DeathTicks);
        game.RunOneStep(new PlayerInput(false, 80, false, false));
        game.RunOneStep();
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void CollisionUsesPlayerLaneOnCompletionTick()
    {
        TempestGame game = PreparePivot();
        Step(game, 2);
        Assert.Equal(0, game.DeathTicks);
        game.RunOneStep(new PlayerInput(false, 80, false, false));
        Assert.Equal(0, game.DeathTicks);

        game = PreparePivot();
        game.Player.Move(1, 16);
        Step(game, 2);
        game.RunOneStep(new PlayerInput(false, -80, false, false));
        Assert.Equal(60, game.DeathTicks);
    }

    [Fact]
    public void FinalPivotTickShotWinsBeforePlayerCollisionAndScoresOnce()
    {
        TempestGame game = PreparePivot();
        Flipper flipper = game.Flippers[0];
        Step(game, 2);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.DoesNotContain(flipper, game.Flippers);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(game.Shots);
    }

    [Fact]
    public void DeathFreezesSimulationForSixtyTicksThenResetsPendingEnemies()
    {
        TempestGame game = PreparePivot();
        Step(game, 3);
        Assert.Equal(60, game.DeathTicks);
        float height = game.Flippers[1].Height;
        Step(game, 59, new PlayerInput(true, 80, false, false));
        Assert.Equal(1, game.DeathTicks);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(height, game.Flippers[1].Height);
        Assert.Empty(game.Shots);
        game.RunOneStep();
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Empty(game.Flippers);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Step(game, 50);
        Assert.Empty(game.Flippers);
        Assert.Equal(EggState.Approaching, game.Eggs[0].State);
    }

    [Fact]
    public void GameOverFreezesUntilRestartAndResetsScoreAndLives()
    {
        TempestGame game = PreparePivot();
        game.Player.Lives = 1;
        game.Score.Add(150);
        Step(game, 63);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Assert.Empty(game.Flippers);
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Empty(game.Shots);
        Assert.Equal(150, game.Score.Score);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.GameOver);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(7, game.Player.EdgeIndex);
    }

    [Fact]
    public void DestroyingLastEnemyStartsExitDelayOnTheSameTick()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 9; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        Assert.Equal(7, Assert.Single(game.Eggs).EdgeIndex);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(7, Assert.Single(game.Flippers).EdgeIndex);

        int ticks = 0;
        while (!game.IsExiting && ticks < 100)
        {
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            ticks++;
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed == 10, game.State == LevelState.ExitDelay);
        }

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.LevelComplete);
        Assert.Empty(game.Flippers);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(0, game.DeathTicks);
    }

    [Fact]
    public void LevelCompletionFreezesAndReplayRetainsScoreAndLives()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.Score.Add(1500);
        game.Player.Lives = 2;
        game.RunOneStep();
        FinishExit(game);
        Assert.True(game.LevelComplete);
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Empty(game.Shots);
        Assert.Equal(7, game.Player.EdgeIndex);
        game.RunOneStep(restartPressed: true);
        Assert.False(game.LevelComplete);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Theory]
    [InlineData(0, 8, 1)]
    [InlineData(0, 0, 1)]
    [InlineData(15, 0, -1)]
    [InlineData(0, 15, 1)]
    public void ShortestDirectionPreservesTieAndWrapRules(int source, int target, int expected)
    {
        Assert.Equal(expected, new Level().FindShortestDirection(source, target));
    }

    // --- Egg lifecycle and normal ten-unit accounting regressions ---

    [Fact]
    public void NewEggStartsRoamingBehindTheWeb()
    {
        Egg egg = new(12, -1);
        Assert.Equal(12, egg.EdgeIndex);
        Assert.Equal(-10, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
        Assert.Equal(-1, egg.MovementDirection);
        Assert.Equal(1, egg.VerticalDirection);
        Assert.Equal(EggState.Roaming, egg.State);
    }

    [Theory]
    [InlineData(15, 1, 0)]
    [InlineData(0, -1, 15)]
    [InlineData(4, 1, 5)]
    public void EggRoamsAndWrapsOnlyOnTheEighthUpdate(int lane, int direction, int expected)
    {
        Level level = new();
        Egg egg = new(lane, direction);
        for (int tick = 0; tick < 7; tick++)
            egg.Update(level);
        Assert.Equal(lane, egg.EdgeIndex);
        Assert.Equal(-10, egg.Height);
        Assert.Equal(7, egg.UpdateCounter);
        egg.Update(level);
        Assert.Equal(expected, egg.EdgeIndex);
        Assert.Equal(-9, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
    }

    [Fact]
    public void RoamingEggReversesAtMinusOneAndMinusTwenty()
    {
        Level level = new();
        Egg egg = new();
        for (int tick = 0; tick < 9 * 8; tick++)
            egg.Update(level);
        Assert.Equal(-1, egg.Height);
        Assert.Equal(-1, egg.VerticalDirection);
        for (int tick = 0; tick < 19 * 8; tick++)
            egg.Update(level);
        Assert.Equal(-20, egg.Height);
        Assert.Equal(1, egg.VerticalDirection);
        for (int tick = 0; tick < 1000; tick++)
        {
            egg.Update(level);
            Assert.InRange(egg.Height, -20, -1);
        }
    }

    [Fact]
    public void ApproachingEggStepsByTwoEveryEightUpdatesThenClampsAtZero()
    {
        Level level = new();
        Egg egg = new(4);
        for (int tick = 0; tick < 8; tick++)
            egg.Update(level);
        Assert.Equal(-9, egg.Height);
        egg.BeginApproach();
        for (int tick = 0; tick < 7; tick++)
            egg.Update(level);
        Assert.Equal(-9, egg.Height);
        egg.Update(level);
        Assert.Equal(-7, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
        Assert.Equal(5, egg.EdgeIndex);
        for (int tick = 0; tick < 32; tick++)
            egg.Update(level);
        Assert.Equal(0, egg.Height);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        egg.Update(level);
        Assert.Equal(0, egg.Height);
        Assert.Equal(0, egg.UpdateCounter);
    }

    [Fact]
    public void LevelStartsWithTenEnemiesFourActiveEggsAndSixReserveEggs()
    {
        TempestGame game = new();
        Assert.Equal(10, Level.EnemyTotal);
        Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.All(game.Eggs, egg =>
        {
            Assert.Equal(-10, egg.Height);
            Assert.Equal(0, egg.UpdateCounter);
            Assert.Equal(EggState.Roaming, egg.State);
            Assert.InRange(egg.EdgeIndex, 0, 15);
        });
        Assert.Equal(new[] { 1, -1, 1, -1 }, game.Eggs.Select(egg => egg.MovementDirection));
        AssertAccounting(game);
    }

    [Fact]
    public void HatchSelectionIsStaggeredAndNeverExceedsTwoEggsInTransit()
    {
        Level level = new();
        foreach (Egg egg in level.Eggs)
            for (int tick = 0; tick < 28 * 8; tick++)
                egg.Update(level);

        for (int tick = 0; tick < 49; tick++)
            level.UpdateEggs();
        Assert.All(level.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
        level.UpdateEggs();
        Assert.Single(level.Eggs, egg => egg.State != EggState.Roaming);
        for (int tick = 0; tick < 50; tick++)
            level.UpdateEggs();
        Assert.Equal(2, level.Eggs.Count(egg => egg.State != EggState.Roaming));
        for (int tick = 0; tick < 300; tick++)
        {
            level.UpdateEggs();
            Assert.InRange(level.Eggs.Count(egg => egg.State != EggState.Roaming), 0, 2);
        }
    }

    [Fact]
    public void ReadyEggConvertsToOneFlipperAtZeroWithoutChangingRemainingCount()
    {
        Level level = new();
        Egg egg = level.Eggs[0];
        MakeReady(egg, level);
        int remaining = level.RemainingEnemies;
        Flipper flipper = Assert.IsType<Flipper>(level.HatchReadyEgg());
        Assert.Equal(egg.EdgeIndex, flipper.EdgeIndex);
        Assert.Equal(0, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Equal(3, level.Eggs.Count);
        Assert.DoesNotContain(egg, level.Eggs);
        Assert.Null(level.HatchReadyEgg());
        Assert.Equal(remaining, level.RemainingEnemies);
        Assert.Equal(10, level.ReserveEggs + level.Eggs.Count + 1);
    }

    [Fact]
    public void ReserveReleaseOccursOnFiftiethTickAndCreatesAnEggNotAFlipper()
    {
        TempestGame game = new();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(3, game.Eggs.Count);
        Step(game, 48);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Equal(3, game.Eggs.Count);
        game.RunOneStep();
        Assert.Equal(5, game.CurrentLevel.ReserveEggs);
        Assert.Equal(4, game.Eggs.Count);
        Egg released = game.Eggs[^1];
        Assert.Equal(14, released.EdgeIndex);
        Assert.Equal(-10, released.Height);
        Assert.Equal(EggState.Roaming, released.State);
        Assert.Single(game.Flippers);
        AssertAccounting(game);
    }

    [Fact]
    public void LevelCannotCompleteWithReserveEggsActiveEggsOrFlippers()
    {
        Level level = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            level.RecordEnemyDestroyed();
        Assert.False(level.IsComplete(0));
        level.ReturnSurvivorsToReserve();
        Assert.True(level.IsComplete(0));
        Assert.False(level.IsComplete(1));

        level.Reset();
        level.ReturnSurvivorsToReserve();
        Assert.Empty(level.Eggs);
        Assert.Equal(10, level.ReserveEggs);
        Assert.False(level.IsComplete(0));

        level.Reset();
        while (level.ReserveEggs > 0)
        {
            foreach (Egg egg in level.Eggs)
                MakeReady(egg, level);
            while (level.HatchReadyEgg() is not null) { }
            for (int tick = 0; tick < 50; tick++)
                level.UpdateEggRelease();
        }
        Assert.Equal(0, level.ReserveEggs);
        Assert.NotEmpty(level.Eggs);
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            level.RecordEnemyDestroyed();
        Assert.False(level.IsComplete(0));

        TempestGame game = new();
        Step(game, 49);
        Assert.Empty(game.Flippers);
        Assert.False(game.LevelComplete);
    }

    [Fact]
    public void AccountingSurvivesNaturalHatchesKillsDeathsAndRestart()
    {
        TempestGame game = new();
        bool sawHatch = false;
        bool sawDeath = false;
        for (int tick = 0; tick < 4000 && !game.GameOver; tick++)
        {
            game.RunOneStep();
            sawHatch |= game.Flippers.Count > 0;
            sawDeath |= game.DeathTicks > 0;
            AssertAccounting(game);
            Assert.Equal(10, game.CurrentLevel.RemainingEnemies);
            Assert.False(game.LevelComplete);
        }
        Assert.True(sawHatch);
        Assert.True(sawDeath);
        Assert.True(game.GameOver);
        game.RunOneStep(restartPressed: true);
        AssertAccounting(game);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void DefeatedEnemiesAndScoreStayDefeatedAfterLosingALife()
    {
        TempestGame game = PreparePivot();
        Step(game, 2);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        AssertAccounting(game);
        int ticks = 0;
        while (game.Player.Lives == 3 && ticks++ < 2000)
        {
            game.RunOneStep();
            AssertAccounting(game);
        }
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(9, game.CurrentLevel.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(5, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void EntireEggWaveCanBeDestroyedWithSameTickExitThenCompletion()
    {
        TempestGame game = new();
        int previousDestroyed = 0;
        for (int tick = 0; tick < 5000 && !game.IsExiting && !game.GameOver; tick++)
        {
            Flipper? target = game.Flippers.OrderByDescending(flipper => flipper.Height).FirstOrDefault();
            if (target is not null)
                game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex, 16);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            AssertAccounting(game);
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed * 150, game.Score.Score);
            Assert.Equal(10 - game.CurrentLevel.EnemiesDestroyed,
                game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Flippers.Count);
            Assert.True(game.CurrentLevel.EnemiesDestroyed >= previousDestroyed);
            previousDestroyed = game.CurrentLevel.EnemiesDestroyed;
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed == 10, game.State == LevelState.ExitDelay);
        }
        Assert.Equal(LevelState.ExitDelay, game.State);
        FinishExit(game);
        Assert.True(game.LevelComplete);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
    }

    private static TempestGame PrepareFinalEnemy()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 9; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame BeginExit()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 10; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        return game;
    }

    private static void FinishExit(TempestGame game)
    {
        for (int tick = 0; tick < 200 && !game.LevelComplete; tick++)
            game.RunOneStep();
        Assert.True(game.LevelComplete);
    }

    private static void AssertAccounting(TempestGame game)
    {
        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.ReserveEggs
            + game.Eggs.Count + game.Flippers.Count);
        Assert.InRange(game.CurrentLevel.ReserveEggs, 0, 10);
        Assert.InRange(game.Eggs.Count, 0, 4);
        Assert.InRange(game.Eggs.Count(egg => egg.State != EggState.Roaming), 0, 2);
    }

    private static TempestGame PreparePivot()
        => PreparePivotWithStartingLives(Player.DefaultStartingLives);

    private static TempestGame PreparePivotWithStartingLives(int startingLives)
    {
        TempestGame game = new(startingLives: startingLives);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        game.Player.Move(egg.EdgeIndex - 1 - game.Player.EdgeIndex, 16);
        Step(game, 199);
        Assert.Equal(FlipperState.Pausing, game.Flippers[0].State);
        Step(game, 24);
        Assert.Equal(FlipperState.Flipping, game.Flippers[0].State);
        Assert.Equal(11, game.Flippers[0].DestinationEdge);
        return game;
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void Step(TempestGame game, int ticks, PlayerInput input = default)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep(input);
    }
}
