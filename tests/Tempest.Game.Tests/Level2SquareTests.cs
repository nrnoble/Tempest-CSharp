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
public sealed class Level2SquareTests
{
    private static readonly Vector2[] SquarePoints =
    [
        new(161, 161), new(80, 161), new(0, 161), new(-80, 161),
        new(-161, 161), new(-161, 80), new(-161, 0), new(-161, -80),
        new(-161, -161), new(-80, -161), new(0, -161), new(80, -161),
        new(161, -161), new(161, -80), new(161, 0), new(161, 80)
    ];

    [Fact]
    public void ProductionLevel2ConfigurationPreservesValidatedGeometryAndEnablesCanflip()
    {
        LevelConfiguration configuration = LevelConfiguration.Level2Square;
        Level level = new(configuration);

        Assert.Equal(1, configuration.ProgressionIndex);
        Assert.Equal(2, configuration.DisplayedLevel);
        Assert.Equal(1, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(SquarePoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(400, configuration.Depth);
        Assert.Equal(50, configuration.Position);
        Assert.Equal(400, configuration.Definition.CenterY);
        Assert.Equal(-80, configuration.Definition.YOffset);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(12, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(8, level.ReserveEggs);
        Assert.Equal(LevelEnemyComposition.FlippersOnly, configuration.NormalEnemyComposition);
        Assert.True(configuration.FlipperCanFlip);
        Assert.Equal(new FlipperConfiguration(true, 2, 8, 21, 24), configuration.Flipper);
        Assert.Equal(FlipperPivotGeometry.SquareEndpointHinge, configuration.FlipperPivotMode);
        Assert.All(configuration.SpawnLaneSequence, lane => Assert.InRange(lane, 0, 15));
        Assert.All(configuration.EggDirectionSequence, direction => Assert.Contains(direction, new[] { -1, 1 }));
    }

    [Fact]
    public void DefaultAndExistingDevelopmentScenariosRemainLevel1()
    {
        TempestGame[] games =
        [
            new(),
            new(spikerDevelopmentWave: true),
            new(tankerTest: true),
            new(fuseballTest: true),
            new(pulsarTest: true),
            new(enemyBulletsTest: true)
        ];

        Assert.All(games, game =>
        {
            Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
            Assert.False(game.Level2Test);
            Assert.False(game.CurrentLevel.Configuration.FlipperCanFlip);
        });
    }

    [Fact]
    public void Level2FlagComposesWithLivesAndOptionalBulletsButConflictsWithOtherScenarios()
    {
        string[] arguments = ["--level2-test", "--lives", "10"];
        Assert.True(DebugController.UseLevel2Test(arguments));
        Assert.False(DebugController.UseLevel2Test([]));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.Equal(10, DebugController.GetStartingLives(arguments));

        TempestGame game = new(level2Test: DebugController.UseLevel2Test(arguments),
            startingLives: DebugController.GetStartingLives(arguments));
        Assert.True(game.Level2Test);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
        Assert.False(game.EnemyBulletsTest);

        string[] bulletArguments = ["--level2-test", "--enemy-bullets-test", "--lives", "10"];
        Assert.False(DebugController.HasConflictingScenarios(bulletArguments));
        TempestGame withBullets = new(
            startingLives: DebugController.GetStartingLives(bulletArguments),
            enemyBulletsTest: DebugController.UseEnemyBulletsTest(bulletArguments),
            level2Test: DebugController.UseLevel2Test(bulletArguments));
        Assert.True(withBullets.EnemyBulletsTest);
        Assert.Same(LevelConfiguration.Level2Square, withBullets.CurrentLevel.Configuration);

        foreach (string other in new[]
        {
            "--spiker-test", "--tanker-test", "--fuseball-test", "--pulsar-test"
        })
        {
            string[] conflicting = ["--level2-test", other];
            Assert.True(DebugController.HasConflictingScenarios(conflicting));
            Assert.Throws<ArgumentException>(() => new TempestGame(
                spikerDevelopmentWave: DebugController.UseSpikerWave(conflicting),
                tankerTest: DebugController.UseTankerTest(conflicting),
                fuseballTest: DebugController.UseFuseballTest(conflicting),
                pulsarTest: DebugController.UsePulsarTest(conflicting),
                level2Test: DebugController.UseLevel2Test(conflicting)));
        }
    }

    [Fact]
    public void Level2ReplayAndGameOverRestartReuseTheIsolatedConfiguration()
    {
        TempestGame game = new(level2Test: true, startingLives: 10);
        Level completedLevel = game.CurrentLevel;
        game.Score.Add(501);
        game.Player.Lives = 4;
        game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true);
        game.CurrentLevel.State = LevelState.Complete;

        game.RunOneStep(restartPressed: true);

        Assert.NotSame(completedLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(501, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(2, game.SuperZapperChargesRemaining);

        Level gameOverLevel = game.CurrentLevel;
        game.Score.Add(500);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);

        Assert.NotSame(gameOverLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void LifeReconstructionRetainsLevel2RuntimeAndPersistentState()
    {
        TempestGame game = CreateSingleUnresolvedFlipperGame(startingLives: 10);
        Level level = game.CurrentLevel;
        Flipper flipper = Assert.Single(game.Flippers);
        game.Score.Add(321);
        game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true);

        PrepareLethalRimPivot(game, flipper);
        game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(9, game.Player.Lives);
        Assert.Equal(321, game.Score.Score);
        Assert.Equal(11, game.CurrentLevel.EnemiesDestroyed);
        Assert.Empty(game.Flippers);
        Assert.Single(game.Eggs);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.DoesNotContain(flipper, game.Flippers);
    }

    [Fact]
    public void Level2ConfiguredSupplySupportsKillAndNonScoringRimClear()
    {
        TempestGame killGame = CreateSingleUnresolvedFlipperGame();
        Flipper target = Assert.Single(killGame.Flippers);
        for (int tick = 0; tick < 50; tick++)
            target.Update(killGame.CurrentLevel, killGame.Player.EdgeIndex);

        killGame.Player.Move(target.EdgeIndex - killGame.Player.EdgeIndex, 16);
        killGame.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(killGame.Shots);
        shot.Height = target.Height - 6;
        killGame.RunOneStep();

        Assert.Empty(killGame.Flippers);
        Assert.Equal(12, killGame.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, killGame.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(150, killGame.Score.Score);
        Assert.Equal(LevelState.ExitDelay, killGame.State);

        TempestGame rimGame = CreateSingleUnresolvedFlipperGame(movePlayerToEggLane: false);
        Flipper survivor = Assert.Single(rimGame.Flippers);
        Assert.NotEqual(survivor.EdgeIndex, rimGame.Player.EdgeIndex);
        rimGame.RunOneStep();
        Assert.Equal(LevelState.Playing, rimGame.State);

        while (survivor.Height < Level.Depth)
            survivor.Update(rimGame.CurrentLevel, rimGame.Player.EdgeIndex);
        rimGame.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, rimGame.State);
        Assert.Empty(rimGame.Flippers);
        Assert.Equal(11, rimGame.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, rimGame.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, rimGame.CurrentLevel.RemainingEnemies);
        Assert.Equal(0, rimGame.Score.Score);
    }

    [Fact]
    public void LegacyTurnHelperRemainsUnchangedAndExposesSquareZeroAnglePairs()
    {
        Level square = new(LevelConfiguration.Level2Square);
        float[] expectedSquareTurns =
        [
            0, 0, 0, 90, 0, 0, 0, 90, 0, 0, 0, 90, 0, 0, 0, 90
        ];
        for (int edge = 0; edge < 16; edge++)
        {
            int next = square.NextEdge(edge);
            Assert.Equal(expectedSquareTurns[edge], square.CalculateEdgeTurnAngle(edge, next), 3);
        }

        Level circle = new(LevelConfiguration.Level1);
        for (int edge = 0; edge < 16; edge++)
        {
            int next = circle.NextEdge(edge);
            Assert.Equal(circle.CalculateEdgeTurnAngle(edge, next),
                circle.CalculateFlipperTurnAngle(edge, next));
        }
    }

    [Fact]
    public void SquareHingeCoversEveryPairAndBothDirectionsWithExactDestinationEndpoints()
    {
        Level level = new(LevelConfiguration.Level2Square);

        for (int source = 0; source < 16; source++)
        {
            int previous = level.PreviousEdge(source);
            int next = level.NextEdge(source);
            float previousAngle = level.CalculateSquareFlipperHingeRotationDegrees(source, previous);
            float nextAngle = level.CalculateSquareFlipperHingeRotationDegrees(source, next);

            Assert.Equal(ExpectedHingeAngle(source, previous), MathF.Abs(previousAngle), 3);
            Assert.Equal(ExpectedHingeAngle(source, next), MathF.Abs(nextAngle), 3);
            Assert.Equal(MathF.Abs(previousAngle), level.CalculateFlipperTurnAngle(source, previous));
            Assert.Equal(MathF.Abs(nextAngle), level.CalculateFlipperTurnAngle(source, next));

            AssertDestinationEndpoints(level, source, previous);
            AssertDestinationEndpoints(level, source, next);

            foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
            {
                (Vector2 start, Vector2 end) =
                    level.CalculateSquareFlipperPivotEndpoints(source, previous, progress);
                AssertFinite(start);
                AssertFinite(end);
                (start, end) = level.CalculateSquareFlipperPivotEndpoints(source, next, progress);
                AssertFinite(start);
                AssertFinite(end);
            }
        }

        Assert.Equal(180, level.CalculateSquareFlipperHingeRotationDegrees(1, 0));
        Assert.Equal(-180, level.CalculateSquareFlipperHingeRotationDegrees(0, 1));
        Assert.Equal(-90, level.CalculateSquareFlipperHingeRotationDegrees(3, 4));
        Assert.Equal(180, level.CalculateSquareFlipperHingeRotationDegrees(3, 2));
        Assert.Equal(90, level.CalculateSquareFlipperHingeRotationDegrees(0, 15));
        Assert.Equal(-90, level.CalculateSquareFlipperHingeRotationDegrees(15, 0));
    }

    [Fact]
    public void CompletedPivotIntoPlayerDoesNotRequestSquareHingeWithStaleLanePair()
    {
        TempestGame game = new(level2Test: true, enemyBulletsTest: true);
        const int source = 1;
        const int destination = 0;
        Flipper flipper = BeginRimPivot(game.CurrentLevel, source, destination);
        Assert.Equal(180, flipper.TotalTurnAngle);

        for (int tick = 0; tick < 23; tick++)
            flipper.Update(game.CurrentLevel, destination);

        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(destination, flipper.EdgeIndex);
        Assert.Equal(destination, flipper.SourceEdge);
        Assert.Equal(destination, flipper.DestinationEdge);

        VectorRenderer renderer = new(game);
        (Vector2 start, Vector2 end) = renderer.ProjectFlipperEndpoints(flipper);
        Assert.Equal(Projection.Project(SquarePoints[destination].X, SquarePoints[destination].Y,
            LevelConfiguration.Level2Square.Position, 400, -80), start);
        Assert.Equal(Projection.Project(SquarePoints[(destination + 1) % SquarePoints.Length].X,
            SquarePoints[(destination + 1) % SquarePoints.Length].Y,
            LevelConfiguration.Level2Square.Position, 400, -80), end);
    }

    [Fact]
    public void ClimbingAndPausedFlippersUseLaneProjectionInsteadOfSquareHingeGeometry()
    {
        TempestGame game = new(level2Test: true);
        VectorRenderer renderer = new(game);
        Flipper climbing = new(15, 100);
        AssertFinite(renderer.ProjectFlipperEndpoints(climbing).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(climbing).End);

        Flipper paused = new(15, Level.Depth);
        paused.Update(game.CurrentLevel, playerEdge: 7);
        Assert.Equal(FlipperState.Pausing, paused.State);
        Assert.Equal(paused.EdgeIndex, paused.SourceEdge);
        Assert.Equal(paused.EdgeIndex, paused.DestinationEdge);
        (Vector2 start, Vector2 end) = renderer.ProjectFlipperEndpoints(paused);
        Assert.Equal(Projection.Project(SquarePoints[15].X, SquarePoints[15].Y,
            LevelConfiguration.Level2Square.Position, 400, -80), start);
        Assert.Equal(Projection.Project(SquarePoints[0].X, SquarePoints[0].Y,
            LevelConfiguration.Level2Square.Position, 400, -80), end);
    }

    [Fact]
    public void SquareFlipperRenderingHandlesBothZapperUsesWithEnemyBulletsEnabled()
    {
        // Keep incoming supply so all-rim clearing cannot discard the bullet on its creation tick.
        TempestGame game = new(level2Test: true, enemyBulletsTest: true,
            random: new DeterministicEnemyBulletRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        while (egg.State != EggState.ReadyToHatch)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.InRange(flipper.BulletAmmunition, 1, 3);
        VectorRenderer renderer = new(game);

        for (int tick = 0; tick < 2000 &&
            !(flipper.BulletTimer == 0 && flipper.State == FlipperState.Pausing &&
              flipper.PauseTicks == 1); tick++)
            flipper.Update(game.CurrentLevel, (flipper.EdgeIndex + 8) % 16, (_, _) => 76);
        Assert.Equal(0, flipper.BulletTimer);
        Assert.Equal(1, flipper.PauseTicks);
        game.Player.Move((flipper.EdgeIndex + 8) % 16 - game.Player.EdgeIndex, 16);
        game.RunOneStep();

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        AssertFinite(renderer.ProjectEnemyBullet(bullet));
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).End);
        int bulletRenderLines = 0;
        renderer.DrawEnemyBullets((_, _) => bulletRenderLines++);
        Assert.Equal(8, bulletRenderLines);

        IEnumerable<Enemy> targets = game.Flippers.Cast<Enemy>().Concat(game.EnemyBullets);
        Assert.True(game.SuperZapper.TryActivate(targets,
            game.Player.EdgeIndex, game.CurrentLevel.PlayableLaneCount, continuous: true));
        Assert.Equal(4, flipper.PendingZapTicks);
        Assert.Equal(0, bullet.PendingZapTicks);
        Assert.Equal(1, game.SuperZapperChargesRemaining);

        targets = game.Flippers.Cast<Enemy>().Concat(game.EnemyBullets);
        Assert.True(game.SuperZapper.TryActivate(targets,
            game.Player.EdgeIndex, game.CurrentLevel.PlayableLaneCount, continuous: true));
        Assert.Equal(4, flipper.PendingZapTicks);
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).End);

        for (int tick = 0; tick < 4; tick++)
        {
            game.RunOneStep();
            foreach (Flipper active in game.Flippers)
            {
                AssertFinite(renderer.ProjectFlipperEndpoints(active).Start);
                AssertFinite(renderer.ProjectFlipperEndpoints(active).End);
            }
        }

        Assert.Empty(game.Flippers);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void LevelOneCompletedPivotIntoPlayerKeepsLegacyProjection()
    {
        TempestGame game = new();
        const int source = 1;
        const int destination = 0;
        Flipper flipper = BeginRimPivot(game.CurrentLevel, source, destination);
        int turnTicks = (int)MathF.Ceiling(flipper.TotalTurnAngle / 8);

        for (int tick = 0; tick < turnTicks; tick++)
            flipper.Update(game.CurrentLevel, destination);

        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(destination, flipper.EdgeIndex);
        VectorRenderer renderer = new(game);
        (Vector2 start, Vector2 end) = renderer.ProjectFlipperEndpoints(flipper);
        Assert.Equal(Projection.Project(game.CurrentLevel.Definition.Points[destination].X,
            game.CurrentLevel.Definition.Points[destination].Y, Level.Position,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset), start);
        int next = game.CurrentLevel.NextEdge(destination);
        Assert.Equal(Projection.Project(game.CurrentLevel.Definition.Points[next].X,
            game.CurrentLevel.Definition.Points[next].Y, Level.Position,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset), end);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [InlineData(3, 4)]
    [InlineData(3, 2)]
    public void RendererUsesTheSquareHingePathAndProjectsFiniteEndpoints(int source, int destination)
    {
        TempestGame game = new(level2Test: true);
        Flipper flipper = BeginRimPivot(game.CurrentLevel, source, destination);
        VectorRenderer renderer = new(game);
        int turnTicks = flipper.TotalTurnAngle >= 180 ? 11 : 10;

        for (int tick = 0; tick < turnTicks; tick++)
            flipper.Update(game.CurrentLevel, destination);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        (Vector2 start, Vector2 end) = renderer.ProjectFlipperEndpoints(flipper);
        AssertFinite(start);
        AssertFinite(end);

        float progress = flipper.TurnAngle / flipper.TotalTurnAngle;
        (Vector2 expectedStart, Vector2 expectedEnd) =
            game.CurrentLevel.CalculateSquareFlipperPivotEndpoints(source, destination, progress);
        float depth = Level.Position + Level.Depth - flipper.Height;
        expectedStart = Projection.Project(expectedStart.X, expectedStart.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        expectedEnd = Projection.Project(expectedEnd.X, expectedEnd.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        Assert.Equal(expectedStart, start);
        Assert.Equal(expectedEnd, end);
    }

    [Fact]
    public void SquareTubeAndEntityProjectionRemainFiniteAndFlashOnlyChangesColor()
    {
        TempestGame game = new(level2Test: true);
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> normal = [];

        renderer.DrawTube((start, end, color) => normal.Add((start, end, color)));

        Assert.Equal(48, normal.Count);
        Assert.All(normal, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
            Assert.Equal(Color.Blue, line.Color);
        });
        Assert.Equal(16, normal.Take(16).Count());
        Assert.Equal(16, normal.Skip(16).Take(16).Count());
        Assert.Equal(16, normal.Skip(32).Take(16).Count());

        renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points, out Vector2 playerStart, out Vector2 playerEnd);
        AssertFinite(playerStart);
        AssertFinite(playerEnd);
        Assert.All(game.Eggs, egg => AssertFinite(renderer.ProjectEgg(egg)));
        AssertFinite(renderer.ProjectShot(new Shot { EdgeIndex = 15, Height = 200 }));
        AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(4, 200)).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(4, 200)).End);

        game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true);
        List<(Vector2 Start, Vector2 End, Color Color)> flash = [];
        renderer.DrawTube((start, end, color) => flash.Add((start, end, color)));
        Assert.Equal(normal.Select(line => (line.Start, line.End)),
            flash.Select(line => (line.Start, line.End)));
        Assert.All(flash, line => Assert.Equal(new Color(255, 0, 255, 255), line.Color));
    }

    private static TempestGame CreateSingleUnresolvedFlipperGame(
        int startingLives = 3, bool movePlayerToEggLane = true,
        bool enemyBulletsTest = false, Random? random = null)
    {
        TempestGame game = new(level2Test: true, startingLives: startingLives,
            enemyBulletsTest: enemyBulletsTest, random: random);
        for (int index = 0; index < 11; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        Egg egg = Assert.Single(game.Eggs);
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        if (movePlayerToEggLane)
            game.Player.Move(egg.EdgeIndex - game.Player.EdgeIndex,
                game.CurrentLevel.PlayableLaneCount);
        game.RunOneStep();
        Assert.Single(game.Flippers);
        return game;
    }

    private static Flipper BeginRimPivot(Level level, int source, int destination)
    {
        Flipper flipper = new(source, Level.Depth);
        flipper.Update(level, destination);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        for (int tick = 0; tick < 24; tick++)
            flipper.Update(level, destination);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(destination, flipper.DestinationEdge);
        return flipper;
    }

    private static void PrepareLethalRimPivot(TempestGame game, Flipper flipper)
    {
        for (int tick = 0; tick < 1000 &&
            (flipper.Height < Level.Depth || flipper.State == FlipperState.Flipping); tick++)
            flipper.Update(game.CurrentLevel, (flipper.DestinationEdge + 8) % 16);
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        int destination = game.CurrentLevel.NextEdge(flipper.EdgeIndex);
        game.Player.Move(destination - game.Player.EdgeIndex, 16);
        while (flipper.State == FlipperState.Pausing)
            flipper.Update(game.CurrentLevel, destination);
        int turnTicks = (int)MathF.Ceiling(flipper.TotalTurnAngle / 8);
        for (int tick = 0; tick < turnTicks - 1; tick++)
            flipper.Update(game.CurrentLevel, destination);
        Assert.False(flipper.CompletedIntoPlayer);
    }

    private static float ExpectedHingeAngle(int source, int destination)
    {
        bool isCorner = (source == 3 && destination == 4) ||
            (source == 4 && destination == 3) ||
            (source == 7 && destination == 8) ||
            (source == 8 && destination == 7) ||
            (source == 11 && destination == 12) ||
            (source == 12 && destination == 11) ||
            (source == 15 && destination == 0) ||
            (source == 0 && destination == 15);
        return isCorner ? 90 : 180;
    }

    private static void AssertDestinationEndpoints(Level level, int source, int destination)
    {
        (Vector2 initialStart, Vector2 initialEnd) =
            level.CalculateSquareFlipperPivotEndpoints(source, destination, progress: 0);
        Assert.Equal(level.Definition.Points[source], initialStart);
        Assert.Equal(level.Definition.Points[level.NextEdge(source)], initialEnd);

        (Vector2 start, Vector2 end) =
            level.CalculateSquareFlipperPivotEndpoints(source, destination, progress: 1);
        Assert.Equal(level.Definition.Points[destination], start);
        Assert.Equal(level.Definition.Points[level.NextEdge(destination)], end);
    }

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private sealed class DeterministicEnemyBulletRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == 1 && maxValue == 4 ? 1 :
            minValue == 0 && maxValue == 101 ? 76 :
            throw new InvalidOperationException($"Unexpected random range [{minValue}, {maxValue}).");
    }
}
