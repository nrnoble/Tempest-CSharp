using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class LevelProgressionTests
{
    [Fact]
    public void NormalLaunchWaitsForSuccessfulExitAndInstallsLevel2OnlyOnce()
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Level outgoing = game.CurrentLevel;
        game.Score.Add(1500);
        game.Player.Lives = 2;
        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Step(game, 20);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);

        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Step(game, 123);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);

        game.RunOneStep();

        AssertFreshLevel2(game, outgoing, expectedScore: 1500, expectedLives: 2);
        Level installed = game.CurrentLevel;
        Step(game, 50);
        Assert.Same(installed, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(1, game.CurrentLevel.Configuration.ProgressionIndex);
    }

    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 10)]
    [InlineData(8, 2)]
    public void KillAllRimAndMixedClearRoutesConvergeOnNaturalLevel2(int kills, int rimClears)
    {
        TempestGame game = CreateClearedLevel1(kills);
        game.Score.Add(kills * 150);
        if (rimClears > 0)
            CompleteRemainingUnitsAtRim(game, rimClears);
        else
            game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(kills, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(rimClears, game.CurrentLevel.EnemiesRimCleared);
        AssertFreshLevel2AfterExit(game, expectedScore: kills * 150, expectedLives: 3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void FreshLevel2AlwaysRechargesZapperAndClearsFlash(int chargesBeforeExit)
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        for (int use = SuperZapperSystem.InitialCharges; use > chargesBeforeExit; use--)
            Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));

        Assert.Equal(chargesBeforeExit, game.SuperZapperChargesRemaining);
        if (chargesBeforeExit < SuperZapperSystem.InitialCharges)
            Assert.True(game.SuperZapper.FlashTicksRemaining > 0);

        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void FreshTransitionDiscardsOldFlippersAndPendingZapTargets()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 40; tick++)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();
        Flipper outgoingFlipper = Assert.Single(game.Flippers);
        Assert.True(game.SuperZapper.TryActivate(game.Flippers, game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));
        Assert.True(outgoingFlipper.PendingZapTicks > 0);

        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Empty(game.Flippers);
        Assert.DoesNotContain(outgoingFlipper, game.Flippers);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void NaturalProgressionIsOrthogonalToLivesAndEnemyBulletOption(
        bool enemyBulletsTest, bool livesOverride)
    {
        TempestGame game = new(startingLives: livesOverride ? 10 : Player.DefaultStartingLives,
            enemyBulletsTest: enemyBulletsTest);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);

        game = CreateClearedLevel1(kills: 10, enemyBulletsTest: enemyBulletsTest,
            startingLives: livesOverride ? 10 : Player.DefaultStartingLives);
        game.Score.Add(91);
        game.Player.Lives--;
        AssertFreshLevel2AfterExit(game, expectedScore: 91,
            expectedLives: livesOverride ? 9 : Player.DefaultStartingLives - 1);
        Assert.True(game.EnemyBulletsTest == enemyBulletsTest);
    }

    [Fact]
    public void DevelopmentScenariosAreNotEligibleForProductionProgression()
    {
        TempestGame[] developmentGames =
        [
            new(level2Test: true),
            new(level3Test: true),
            new(level3GeometryTest: true),
            new(spikerDevelopmentWave: true),
            new(tankerTest: true),
            new(fuseballTest: true),
            new(pulsarTest: true)
        ];

        Assert.All(developmentGames, game => Assert.False(game.NaturalProgressionEnabled));
        Assert.Same(LevelConfiguration.Level2Square, developmentGames[0].CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level3Mixed, developmentGames[1].CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level3Plus, developmentGames[2].CurrentLevel.Configuration);
        Assert.All(developmentGames.Skip(3),
            game => Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration));
    }

    [Fact]
    public void LifeLossAndGameOverBeforeSuccessfulExitRemainOnLevel1()
    {
        TempestGame game = new(startingLives: 5);
        Level initialLevel = game.CurrentLevel;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(initialLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(4, game.Player.Lives);

        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep();
        Assert.True(game.GameOver);
        Assert.Same(initialLevel, game.CurrentLevel);

        game.Score.Add(125);
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    [Fact]
    public void NaturalLevel2DeathReconstructsTheSameConfigurationAndGameOverStartsLevel1()
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        game.Score.Add(400);
        AssertFreshLevel2AfterExit(game, expectedScore: 400, expectedLives: 3);
        Level level2 = game.CurrentLevel;
        game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true);
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(level2, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Score.Score);
        Assert.Equal(1, game.SuperZapperChargesRemaining);

        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void NaturalLevel2KillAndAllRimCompletionAdvanceToLevel3(bool allRim)
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        game.Score.Add(650);
        game.Player.Lives = 2;
        AssertFreshLevel2AfterExit(game, expectedScore: 650, expectedLives: 2);

        if (allRim)
            CompleteRemainingUnitsAtRim(game, game.CurrentLevel.EnemySupply);
        else
        {
            for (int enemy = 0; enemy < game.CurrentLevel.EnemySupply; enemy++)
                game.CurrentLevel.RecordEnemyDestroyed();
            game.CurrentLevel.ResetAfterDeath();
            game.RunOneStep();
        }

        Assert.Equal(LevelState.ExitDelay, game.State);
        Level outgoing = game.CurrentLevel;
        game.Score.Add(125);
        game.Player.Lives = 1;
        FinishLevel2Exit(game);
        AssertFreshLevel3(game, outgoing, expectedScore: 775, expectedLives: 1);
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));

        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();
        Assert.True(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);

        game.Score.Add(225);
        game.Player.Lives = 0;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(LevelState.Playing, game.State);
    }

    [Fact]
    public void NaturalLevel2SuccessfulExitCreatesFreshMixedWaveAndCarriesCampaignState()
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        game.Score.Add(1500);
        game.Player.Lives = 3;
        AssertFreshLevel2AfterExit(game, expectedScore: 1500, expectedLives: 3);

        Level outgoing = game.CurrentLevel;
        game.Score.Add(275);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.FlashTicksRemaining > 0);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.NotEmpty(game.Shots);

        for (int enemy = 0; enemy < outgoing.EnemySupply; enemy++)
            outgoing.RecordEnemyDestroyed();
        outgoing.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Shots);

        FinishLevel2Exit(game);

        AssertFreshLevel3(game, outgoing, expectedScore: 1775, expectedLives: 2);
    }

    [Fact]
    public void NaturalLevel3DeathReconstructsInPlaceAndGameOverRestartStartsLevel1()
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        AssertFreshLevel2AfterExit(game, expectedScore: 0, expectedLives: 5);
        Level level2 = game.CurrentLevel;
        for (int enemy = 0; enemy < level2.EnemySupply; enemy++)
            level2.RecordEnemyDestroyed();
        level2.ResetAfterDeath();
        game.RunOneStep();
        FinishLevel2Exit(game);

        Level level3 = game.CurrentLevel;
        Level3WaveLedger ledger = level3.Level3Ledger!;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(level3, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Same(ledger, game.CurrentLevel.Level3Ledger);
        Assert.Equal(LevelState.Playing, game.State);

        game.Score.Add(300);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);

        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    private static TempestGame CreateClearedLevel1(
        int kills, bool enemyBulletsTest = false, int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = new(startingLives: startingLives, enemyBulletsTest: enemyBulletsTest);
        for (int enemy = 0; enemy < kills; enemy++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        return game;
    }

    private static void CompleteRemainingUnitsAtRim(TempestGame game, int rimClears)
    {
        while (game.Eggs.Count > 0 ||
            game.CurrentLevel.EnemiesSpawned < game.CurrentLevel.EnemySupply)
        {
            while (game.Eggs.Count < game.CurrentLevel.Configuration.MaximumActiveEggs &&
                game.CurrentLevel.ReserveEggs > 0)
                for (int tick = 0; tick < game.CurrentLevel.Configuration.ReserveReleaseIntervalTicks; tick++)
                    game.CurrentLevel.UpdateEggRelease();

            foreach (Egg egg in game.Eggs)
            {
                egg.BeginApproach();
                for (int tick = 0; tick < 40; tick++)
                    egg.Update(game.CurrentLevel);
            }
            game.RunOneStep();
        }

        Assert.Equal(rimClears, game.Flippers.Count);
        foreach (Flipper flipper in game.Flippers)
            for (int tick = 0; tick < 2000 && flipper.Height < Level.Depth; tick++)
                flipper.Update(game.CurrentLevel, (flipper.EdgeIndex + 8) % 16);

        Assert.All(game.Flippers, flipper => Assert.Equal(Level.Depth, flipper.Height));
        int safeLane = Enumerable.Range(0, game.CurrentLevel.PlayableLaneCount)
            .First(lane => game.Flippers.All(flipper => flipper.EdgeIndex != lane));
        game.Player.Move(safeLane - game.Player.EdgeIndex, game.CurrentLevel.PlayableLaneCount);
        game.RunOneStep();
        Assert.Equal(rimClears, game.CurrentLevel.EnemiesRimCleared);
    }

    private static void AssertFreshLevel2AfterExit(TempestGame game, int expectedScore, int expectedLives)
    {
        Level outgoing = game.CurrentLevel;
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        FinishLevel1Exit(game);
        AssertFreshLevel2(game, outgoing, expectedScore: expectedScore, expectedLives: expectedLives);
    }

    private static void AssertFreshLevel2(TempestGame game, Level outgoing, int expectedScore, int expectedLives)
    {
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.CurrentLevel.Configuration.ProgressionIndex);
        Assert.Equal(2, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(1, game.CurrentLevel.Configuration.BoardIndex);
        Assert.Equal(0, game.CurrentLevel.Configuration.PaletteIndex);
        Assert.True(game.CurrentLevel.Configuration.Definition.Continuous);
        Assert.Equal(16, game.CurrentLevel.PlayableLaneCount);
        Assert.True(game.CurrentLevel.Configuration.Flipper.CanFlip);
        Assert.Equal(12, game.CurrentLevel.EnemySupply);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(12, game.CurrentLevel.RemainingEnemies);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikes);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(0, game.TankerAccountingTotal);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(expectedScore, game.Score.Score);
        Assert.Equal(expectedLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.All(game.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
    }

    private static void FinishLevel1Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 200 &&
             ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
    }

    private static void FinishLevel2Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 200 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
    }

    private static void AssertFreshLevel3(TempestGame game, Level outgoing,
        int expectedScore, int expectedLives)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = Assert.IsType<Level3WaveLedger>(level.Level3Ledger);

        Assert.NotSame(outgoing, level);
        Assert.Same(LevelConfiguration.Level3Mixed, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, level.Configuration.ProgressionIndex);
        Assert.Equal(3, level.Configuration.DisplayedLevel);
        Assert.Equal(2, level.Configuration.BoardIndex);
        Assert.Equal(0, level.Configuration.PaletteIndex);
        Assert.True(level.IsLevel3Mixed);
        Assert.Equal(15, level.EnemySupply);
        Assert.Equal(15, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(11, level.ReserveEggs);
        Assert.Equal(4, level.EnemiesSpawned);
        Assert.Equal(0, level.EnemiesDestroyed);
        Assert.Equal(0, level.EnemiesRimCleared);
        Assert.Equal(15, ledger.OriginalCount);
        Assert.Equal(0, ledger.CargoCount);
        Assert.Equal(15, ledger.TotalIdentityCount);
        Assert.Equal(15, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikes);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(0, game.TankerAccountingTotal);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(expectedScore, game.Score.Score);
        Assert.Equal(expectedLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.All(level.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }
}
