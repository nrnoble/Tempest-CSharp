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
    {
        TempestGame game = new();
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