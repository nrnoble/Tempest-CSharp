using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level1ProductionFlipperCapTests
{
    private const int OriginalSupply = 10;
    private const int ActiveFlipperLimit = 4;

    [Fact]
    public void ProductionLevelOneUsesTheLimitFromBothStartupPathsDeterministically()
    {
        TempestGame normal = new(random: new Random(1981), startingLives: 1000);
        TempestGame direct = new(random: new Random(1981), startingLives: 1000,
            startingProductionLevel: 1);

        AssertProductionLevelOneStart(normal);
        AssertProductionLevelOneStart(direct);
        Assert.Same(normal.CurrentLevel.Configuration, direct.CurrentLevel.Configuration);
        Assert.Same(normal.CurrentLevel.Definition, direct.CurrentLevel.Definition);
        Assert.Equal(normal.Player.EdgeIndex, direct.Player.EdgeIndex);
        Assert.Equal(normal.Player.Depth, direct.Player.Depth);

        bool sawWaitingEgg = false;
        int maximumActiveFlippers = 0;
        for (int tick = 0; tick < 3000; tick++)
        {
            normal.RunOneStep();
            direct.RunOneStep();
            AssertLevelOneConservation(normal);
            AssertLevelOneConservation(direct);

            maximumActiveFlippers = Math.Max(maximumActiveFlippers, normal.Flippers.Count);
            Assert.Equal(normal.Eggs.Select(GetEggSnapshot),
                direct.Eggs.Select(GetEggSnapshot));
            Assert.Equal(normal.Flippers.Select(GetFlipperSnapshot),
                direct.Flippers.Select(GetFlipperSnapshot));
            Assert.Equal(normal.EnemyBullets.Select(GetBulletSnapshot),
                direct.EnemyBullets.Select(GetBulletSnapshot));
            Assert.Equal(normal.CurrentLevel.ReserveEggs, direct.CurrentLevel.ReserveEggs);

            if (normal.Flippers.Count == ActiveFlipperLimit &&
                normal.Eggs.Any(egg => egg.State == EggState.ReadyToHatch))
            {
                sawWaitingEgg = true;
                break;
            }
        }

        // The pre-promotion production schedule reached five active Flippers at boundary 289.
        Assert.Equal(ActiveFlipperLimit, maximumActiveFlippers);
        Assert.True(sawWaitingEgg);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void CampaignAndGameOverRestartsReturnToCappedProductionLevelOne(int startingLevel)
    {
        TempestGame game = startingLevel == 1
            ? new TempestGame(random: new Random(1981), startingLives: 1000)
            : new TempestGame(startingProductionLevel: startingLevel, startingLives: 1000,
                random: new Random(1981));
        game.Score.Add(325);
        game.CurrentLevel.State = LevelState.GameOver;

        game.RunOneStep(restartPressed: true);

        AssertProductionLevelOneStart(game);
        Assert.Equal(0, game.Score.Score);
        bool sawWaitingEgg = false;
        for (int tick = 0; tick < 3000 && !sawWaitingEgg; tick++)
        {
            game.RunOneStep();
            AssertLevelOneConservation(game);
            sawWaitingEgg = game.Flippers.Count == ActiveFlipperLimit &&
                game.Eggs.Any(egg => egg.State == EggState.ReadyToHatch);
        }

        Assert.True(sawWaitingEgg);
    }

    [Fact]
    public void ReadyEggWaitsUnresolvedAndHatchesWhenSuperZapperFreesASlot()
    {
        TempestGame game = new(random: new Random(1981), startingLives: 100);
        Egg? waitingEgg = null;
        for (int tick = 0; tick < 3000; tick++)
        {
            game.RunOneStep();
            AssertLevelOneConservation(game);
            if (game.Flippers.Count == ActiveFlipperLimit &&
                game.State == LevelState.Playing)
                waitingEgg = game.Eggs.FirstOrDefault(egg =>
                    egg.State == EggState.ReadyToHatch);
            if (waitingEgg is not null)
                break;
        }

        Assert.NotNull(waitingEgg);
        (int Edge, int Direction, float Height, int UpdateCounter, EggState State) snapshot =
            GetEggSnapshot(waitingEgg!);
        int unresolvedBeforeZap = game.CurrentLevel.RemainingEnemies;

        for (int tick = 0; tick < 3; tick++)
        {
            game.RunOneStep();
            AssertLevelOneConservation(game);
            Assert.Contains(waitingEgg!, game.Eggs);
            Assert.Equal(snapshot, GetEggSnapshot(waitingEgg!));
            Assert.Equal(unresolvedBeforeZap, game.CurrentLevel.RemainingEnemies);
        }

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(SuperZapperSystem.InitialCharges - 1, game.SuperZapperChargesRemaining);
        Assert.Contains(waitingEgg!, game.Eggs);

        bool waitingEggHatched = false;
        for (int tick = 0; tick < 120; tick++)
        {
            game.RunOneStep();
            AssertLevelOneConservation(game);
            if (!game.Eggs.Contains(waitingEgg!))
            {
                waitingEggHatched = true;
                break;
            }
        }

        Assert.True(waitingEggHatched,
            "A ready Egg must hatch through the normal path after an active Flipper is resolved.");
        Assert.Equal(ActiveFlipperLimit, game.Flippers.Count);
        Assert.True(game.CurrentLevel.RemainingEnemies < unresolvedBeforeZap);
        Assert.True(game.Score.Score > 0);
    }

    [Fact]
    public void DeathReentryKeepsTheLimitAndConservesEveryLevelOneObligation()
    {
        TempestGame game = new(random: new Random(1981), startingLives: 100);
        int livesAtStart = game.Player.Lives;
        int deathReentries = 0;
        bool sawDeathFreeze = false;

        for (int tick = 0; tick < 20000 && deathReentries < 2; tick++)
        {
            game.RunOneStep();
            AssertLevelOneConservation(game);
            if (game.State == LevelState.PlayerDying)
                sawDeathFreeze = true;
            if (game.Player.Lives < livesAtStart)
            {
                deathReentries++;
                livesAtStart = game.Player.Lives;
                Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
                Assert.Equal(LevelState.Playing, game.State);
                AssertLevelOneConservation(game);
            }
        }

        Assert.True(sawDeathFreeze);
        Assert.Equal(2, deathReentries);
    }

    [Fact]
    public void ProductionLevelOneResolvesAllTenObligationsAndNaturallyStartsLevelTwo()
    {
        TempestGame game = new(random: new Random(1981), startingLives: 1000);
        Level levelOne = game.CurrentLevel;
        int initialScore = game.Score.Score;
        int resolvedLevelOneSupply = -1;

        for (int tick = 0; tick < 60000 &&
             ReferenceEquals(game.CurrentLevel, levelOne) &&
             !game.GameOver; tick++)
        {
            bool zap = game.SuperZapperChargesRemaining > 0 &&
                game.Flippers.Count == ActiveFlipperLimit && tick % 300 == 0;
            bool moveRight = tick / 120 % 2 == 0;
            game.RunOneStep(new PlayerInput(true, 0, !moveRight, moveRight, zap),
                restartPressed: game.LevelComplete);
            if (ReferenceEquals(game.CurrentLevel, levelOne))
            {
                AssertLevelOneConservation(game);
                if (game.LevelComplete)
                    resolvedLevelOneSupply =
                        levelOne.EnemiesDestroyed + levelOne.EnemiesRimCleared;
            }
            else
                resolvedLevelOneSupply =
                    levelOne.EnemiesDestroyed + levelOne.EnemiesRimCleared;
        }

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(OriginalSupply, resolvedLevelOneSupply);
        Assert.Equal(0, levelOne.ReserveEggs);
        Assert.Empty(levelOne.Eggs);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.NotEqual(LevelState.Complete, game.State);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.True(game.Score.Score > initialScore);
    }

    [Fact]
    public void ProductionLevelTwoRemainsUncappedAndLevelsThreeThroughEightKeepTheirConfigurations()
    {
        TempestGame levelTwo = new(startingProductionLevel: 2, startingLives: 1000,
            random: new Random(1981));
        bool exceededFourFlippers = false;
        for (int tick = 0; tick < 1000 && !exceededFourFlippers; tick++)
        {
            levelTwo.RunOneStep();
            exceededFourFlippers = levelTwo.Flippers.Count > ActiveFlipperLimit;
        }
        Assert.True(exceededFourFlippers);

        for (int level = 3; level <= 8; level++)
        {
            TempestGame game = new(startingProductionLevel: level);
            Assert.Equal(level, game.CurrentLevel.Configuration.DisplayedLevel);
            Assert.Same(LevelProgression.GetProductionLevel(level),
                game.CurrentLevel.Configuration);
        }
    }

    [Fact]
    public void VisibleAndHeadlessLevelOnePlaybackUseTheProductionLimit()
    {
        GameplayStartupConfiguration startup = new(1, 1000, 1.0, 1981);
        PlayerInputRecorder recorder = new(startup);
        for (int tick = 0; tick < 300; tick++)
            recorder.CaptureTick(tick, default);
        GameplayRecording recording = new(startup, recorder.CreateReplay());
        GameplayPlaybackSession visible = new(startup, recording.Replay);
        HeadlessGameplayPlaybackRunner headless = new(recording);

        visible.RunToBoundary(300);
        GameplayPlaybackRunResult result = headless.RunToBoundary(300);

        Assert.Equal(ActiveFlipperLimit, visible.Game.Flippers.Count);
        Assert.Contains(visible.Game.Eggs, egg => egg.State == EggState.ReadyToHatch);
        Assert.Equal(visible.Game.Flippers.Count, result.StateSummary.ActiveFlipperCount);
        Assert.Equal(visible.Game.Eggs.Count, result.StateSummary.ActiveEggCount);
    }

    private static void AssertProductionLevelOneStart(TempestGame game)
    {
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Equal(1, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(OriginalSupply, game.CurrentLevel.EnemySupply);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.Equal(LevelConfiguration.Level1.Definition.Points,
            game.CurrentLevel.Definition.Points);
        Assert.Equal(2, game.CurrentLevel.Configuration.MaximumActiveEnemyShots);
    }

    private static void AssertLevelOneConservation(TempestGame game)
    {
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.True(game.Flippers.Count <= ActiveFlipperLimit);
        Assert.True(game.EnemyBullets.Count <=
            game.CurrentLevel.Configuration.MaximumActiveEnemyShots);
        Assert.Equal(OriginalSupply,
            game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Flippers.Count +
            game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.EnemiesRimCleared);
    }

    private static (int Edge, int Direction, float Height, int UpdateCounter, EggState State)
        GetEggSnapshot(Egg egg) =>
        (egg.EdgeIndex, egg.MovementDirection, egg.Height, egg.UpdateCounter, egg.State);

    private static (int Edge, float Height, int Direction, FlipperState State, int ShotDelay)
        GetFlipperSnapshot(Flipper flipper) =>
        (flipper.EdgeIndex, flipper.Height, flipper.Direction, flipper.State,
            flipper.ShotDelayTicks);

    private static (int Lane, float Height, float Rotation) GetBulletSnapshot(EnemyBullet bullet) =>
        (bullet.EdgeIndex, bullet.Height, bullet.Rotation);
}
