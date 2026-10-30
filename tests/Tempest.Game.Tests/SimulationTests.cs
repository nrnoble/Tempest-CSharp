using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Tempest.Game.Rendering;
using System.Numerics;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class SimulationTests
{
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
        game.RunOneStep(new PlayerInput(true, 80, false, false), restartPressed: true);
        Assert.Equal(lane + 1, game.Player.EdgeIndex);
        Assert.Equal(zoom ? depth - 3 : depth, Assert.Single(game.Shots).Height);
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

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

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
        Step(game, 199);
        game.Player.Move(-1, 16);
        Step(game, 24 + 3);
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
        Step(game, 199);
        game.Player.Move(-1, 16);
        Step(game, 24);
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