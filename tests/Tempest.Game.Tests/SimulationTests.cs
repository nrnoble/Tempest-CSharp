using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class SimulationTests
{
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
        int[] expected = [12, 4, 9, 1, 14, 6, 10, 2, 15, 7];
        foreach (int lane in expected)
        {
            for (int tick = 0; tick < 49; tick++)
                Assert.Null(level.UpdateSpawning(0));
            Assert.Equal(lane, level.UpdateSpawning(0));
        }
        Assert.Equal(10, level.EnemiesSpawned);
        Assert.Null(level.UpdateSpawning(0));
    }

    [Fact]
    public void ActiveLimitFreezesSpawnTimerAndDeathRestoresPendingPool()
    {
        Level level = new();
        for (int tick = 0; tick < 49; tick++)
            level.UpdateSpawning(0);
        for (int tick = 0; tick < 100; tick++)
            Assert.Null(level.UpdateSpawning(4));
        Assert.Equal(12, level.UpdateSpawning(0));
        level.RecordEnemyDestroyed();
        level.RecordEnemyDestroyed();
        level.ResetAfterDeath();
        Assert.Equal(2, level.EnemiesSpawned);
        for (int tick = 0; tick < 49; tick++)
            Assert.Null(level.UpdateSpawning(0));
        Assert.Equal(9, level.UpdateSpawning(0));
    }

    [Fact]
    public void SpawnedFlipperClimbsOnItsSpawnTick()
    {
        TempestGame game = new();
        Step(game, 49);
        Assert.Empty(game.Flippers);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(12, flipper.EdgeIndex);
        Assert.Equal(2, flipper.Height);
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
        Assert.Equal(0, game.CurrentLevel.EnemiesSpawned);
        Step(game, 50);
        Assert.Equal(12, Assert.Single(game.Flippers).EdgeIndex);
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
    public void DestroyingLastEnemyCompletesLevelOnTheSameTick()
    {
        TempestGame game = new();
        for (int enemyIndex = 0; enemyIndex < 9; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        Step(game, 50);
        Assert.Equal(7, Assert.Single(game.Flippers).EdgeIndex);

        int ticks = 0;
        while (!game.LevelComplete && ticks < 100)
        {
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            ticks++;
            Assert.Equal(game.CurrentLevel.EnemiesDestroyed == 10, game.LevelComplete);
        }

        Assert.True(game.LevelComplete);
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
        game.Score.Add(1500);
        game.Player.Lives = 2;
        game.RunOneStep();
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

    private static TempestGame PreparePivot()
    {
        TempestGame game = new();
        Step(game, 249);
        Assert.Equal(FlipperState.Pausing, game.Flippers[0].State);
        game.Player.Move(4, 16);
        Step(game, 24);
        Assert.Equal(FlipperState.Flipping, game.Flippers[0].State);
        Assert.Equal(11, game.Flippers[0].DestinationEdge);
        return game;
    }

    private static void Step(TempestGame game, int ticks, PlayerInput input = default)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep(input);
    }
}