using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level3GeometryTests
{
    private static readonly Vector2[] PlusPoints =
    [
        new(98, 98), new(98, 197), new(0, 197), new(-98, 197),
        new(-98, 98), new(-197, 98), new(-197, 0), new(-197, -98),
        new(-98, -98), new(-98, -197), new(0, -197), new(98, -197),
        new(98, -98), new(197, -98), new(197, 0), new(197, 98)
    ];

    [Fact]
    public void Level3PlusConfigurationContainsTheExactClosedSourceBoardAndDiagnosticSupply()
    {
        LevelConfiguration configuration = LevelConfiguration.Level3Plus;
        Level level = new(configuration);

        Assert.Equal(2, configuration.ProgressionIndex);
        Assert.Equal(3, configuration.DisplayedLevel);
        Assert.Equal(2, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(400, configuration.Definition.CenterY);
        Assert.Equal(-80, configuration.Definition.YOffset);
        Assert.Equal(400, configuration.Depth);
        Assert.Equal(50, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(15, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(11, level.ReserveEggs);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(PlusPoints, configuration.Definition.Points);
        Assert.Equal(new FlipperConfiguration(true, 2, 8, 21, 24), configuration.Flipper);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.All(configuration.SpawnLaneSequence, lane => Assert.InRange(lane, 0, 15));
        Assert.Equal(15, configuration.SpawnLaneSequence.Count);

        Assert.Same(LevelConfiguration.Level2Square,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level1));
        Assert.Same(LevelConfiguration.Level3Mixed,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level2Square));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));
        Assert.Null(LevelProgression.GetNextProductionLevel(configuration));
    }

    [Fact]
    public void GeometryDiagnosticFlagSelectsLevel3AndComposesOnlyWithApprovedOptions()
    {
        string[] arguments = ["--level3-geometry-test", "--lives", "10"];
        Assert.True(DebugController.UseLevel3GeometryTest(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.Equal(10, DebugController.GetStartingLives(arguments));

        TempestGame game = new(level3GeometryTest: DebugController.UseLevel3GeometryTest(arguments),
            startingLives: DebugController.GetStartingLives(arguments));
        Assert.True(game.Level3GeometryTest);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level3Plus, game.CurrentLevel.Configuration);
        Assert.Equal(15, game.CurrentLevel.EnemySupply);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(11, game.CurrentLevel.ReserveEggs);

        string[] bulletArguments = ["--level3-geometry-test", "--enemy-bullets-test"];
        Assert.False(DebugController.HasConflictingScenarios(bulletArguments));
        TempestGame withBullets = new(
            enemyBulletsTest: DebugController.UseEnemyBulletsTest(bulletArguments),
            level3GeometryTest: DebugController.UseLevel3GeometryTest(bulletArguments));
        Assert.True(withBullets.EnemyBulletsTest);
        Assert.Same(LevelConfiguration.Level3Plus, withBullets.CurrentLevel.Configuration);

        string[] scenarioFlags =
        [
            "--level2-test", "--level3-test", "--spiker-test", "--tanker-test",
            "--fuseball-test", "--pulsar-test"
        ];
        foreach (string scenario in scenarioFlags)
        {
            string[] conflicting = ["--level3-geometry-test", scenario];
            Assert.True(DebugController.HasConflictingScenarios(conflicting));
            Assert.Throws<ArgumentException>(() => new TempestGame(
                spikerDevelopmentWave: DebugController.UseSpikerWave(conflicting),
                tankerTest: DebugController.UseTankerTest(conflicting),
                fuseballTest: DebugController.UseFuseballTest(conflicting),
                pulsarTest: DebugController.UsePulsarTest(conflicting),
                level2Test: DebugController.UseLevel2Test(conflicting),
                level3Test: DebugController.UseLevel3Test(conflicting),
                level3GeometryTest: DebugController.UseLevel3GeometryTest(conflicting)));
        }

        Assert.False(DebugController.UseLevel3GeometryTest([]));
        Assert.Same(LevelConfiguration.Level1, new TempestGame().CurrentLevel.Configuration);
    }

    [Fact]
    public void GeometryDiagnosticReplayGameOverAndLifeReconstructionKeepThePlusConfiguration()
    {
        TempestGame game = new(level3GeometryTest: true, startingLives: 5);
        Level completedLevel = game.CurrentLevel;
        game.Score.Add(325);
        game.Player.Lives = 3;
        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);

        Assert.NotSame(completedLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Plus, game.CurrentLevel.Configuration);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(2, game.SuperZapperChargesRemaining);

        Level gameOverLevel = game.CurrentLevel;
        game.Score.Add(100);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);

        Assert.NotSame(gameOverLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Plus, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);

        Level runtime = game.CurrentLevel;
        runtime.RecordEnemyDestroyed();
        runtime.ResetAfterDeath();
        Assert.Same(runtime, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Plus, runtime.Configuration);
        Assert.Equal(14, runtime.RemainingEnemies);
        Assert.Equal(4, runtime.Eggs.Count);
        Assert.Equal(10, runtime.ReserveEggs);
    }

    [Fact]
    public void EndpointHingesCoverAllThirtyTwoDirectedPlusTransitions()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        IReadOnlyList<Vector2> points = level.Definition.Points;

        for (int source = 0; source < points.Count; source++)
        {
            foreach (int destination in new[] { level.PreviousEdge(source), level.NextEdge(source) })
            {
                float angle = level.CalculateEndpointHingeRotationDegrees(source, destination);
                Assert.Equal(ExpectedSignedHingeAngle(points, source, destination), angle, 3);
                Assert.Equal(MathF.Abs(angle), level.CalculateFlipperTurnAngle(source, destination));
                Assert.Equal(angle, level.CalculateEndpointHingeRotationDegrees(source, destination));
                Assert.Equal(ReferenceInteriorAngle(points,
                        destination == level.PreviousEdge(source) ? source : level.NextEdge(source)),
                    MathF.Abs(angle), 3);

                (Vector2 initialStart, Vector2 initialEnd) =
                    level.CalculateFlipperPivotEndpoints(source, destination, 0);
                Assert.Equal(points[source], initialStart);
                Assert.Equal(points[level.NextEdge(source)], initialEnd);

                foreach (float progress in new[] { 0.125f, 0.25f, 0.5f, 0.75f, 0.875f })
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(source, destination, progress);
                    AssertFinite(start);
                    AssertFinite(end);
                    Assert.True(Vector2.DistanceSquared(start, end) > 0);

                    int hingeIndex = destination == level.PreviousEdge(source)
                        ? source
                        : level.NextEdge(source);
                    Assert.Contains(points[hingeIndex], new[] { start, end });
                }

                (Vector2 finalStart, Vector2 finalEnd) =
                    level.CalculateFlipperPivotEndpoints(source, destination, 1);
                Assert.Equal(points[destination], finalStart);
                Assert.Equal(points[level.NextEdge(destination)], finalEnd);
            }
        }

        Assert.Equal(180, level.CalculateEndpointHingeRotationDegrees(1, 2));
        Assert.Equal(-180, level.CalculateEndpointHingeRotationDegrees(2, 1));
        Assert.Equal(180, level.CalculateEndpointHingeRotationDegrees(5, 6));
        Assert.Equal(-180, level.CalculateEndpointHingeRotationDegrees(6, 5));
        Assert.Equal(180, level.CalculateEndpointHingeRotationDegrees(9, 10));
        Assert.Equal(-180, level.CalculateEndpointHingeRotationDegrees(10, 9));
        Assert.Equal(180, level.CalculateEndpointHingeRotationDegrees(13, 14));
        Assert.Equal(-180, level.CalculateEndpointHingeRotationDegrees(14, 13));
    }

    [Fact]
    public void PlusBoardOrdinaryCanflipKeepsItsDestinationAndCompletesStraightAndCornerTurns()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        Flipper straight = BeginMidTubePivot(level, source: 1, destination: 2);
        Assert.Equal(180, straight.TotalTurnAngle);

        Shot sourceShot = new() { EdgeIndex = 1, Height = straight.Height - 8 };
        Shot destinationShot = new() { EdgeIndex = 2, Height = straight.Height - 8 };
        Assert.True(straight.IsHitBy(sourceShot));
        Assert.True(straight.IsHitBy(destinationShot));

        straight.Update(level, playerEdge: 8);
        Assert.Equal(2, straight.DestinationEdge);
        for (int tick = 1; tick < 23; tick++)
            straight.Update(level, playerEdge: 8);
        Assert.Equal(FlipperState.Pausing, straight.State);
        Assert.Equal(2, straight.EdgeIndex);
        Assert.Equal(2, straight.SourceEdge);
        Assert.Equal(2, straight.DestinationEdge);
        Assert.Equal(0, straight.TotalTurnAngle);
        Assert.False(straight.CompletedIntoPlayer);

        Flipper corner = BeginMidTubePivot(level, source: 0, destination: 1);
        Assert.Equal(90, corner.TotalTurnAngle);
        for (int tick = 0; tick < 12; tick++)
            corner.Update(level, playerEdge: 8);
        Assert.Equal(FlipperState.Pausing, corner.State);
        Assert.Equal(1, corner.EdgeIndex);
        Assert.False(corner.CompletedIntoPlayer);
    }

    [Fact]
    public void PlusBoardRimWrapPivotUsesEndpointHingeAndCompletesIntoCanonicalLaneState()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        Flipper flipper = new(edge: 15, height: Level.Depth);
        flipper.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Pausing, flipper.State);

        for (int tick = 0; tick < 24; tick++)
            flipper.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(0, flipper.DestinationEdge);
        Assert.Equal(270, flipper.TotalTurnAngle);

        for (int tick = 0; tick < 34; tick++)
            flipper.Update(level, playerEdge: 7);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(0, flipper.EdgeIndex);
        Assert.Equal(0, flipper.SourceEdge);
        Assert.Equal(0, flipper.DestinationEdge);
    }

    [Fact]
    public void TankerCargoUsesInstalledGeometryAndRetainsItsOutwardRouteAndDirection()
    {
        Level plusLevel = new(LevelConfiguration.Level3Plus);
        Tanker tanker = new(edgeIndex: 3, originalSlotId: 0);
        TankerReleaseResult release = tanker.ReleaseFlipperCargo(
            plusLevel, firstSideDirection: 1, TankerReleaseKind.PlayerShotDestruction);
        Assert.Equal(2, release.Cargo.Count);
        Assert.Equal(new[] { 2, 4 }, release.Cargo.Select(cargo => cargo.SourceLane));
        Assert.Equal(new[] { 1, 5 }, release.Cargo.Select(cargo => cargo.DestinationLane));
        Assert.DoesNotContain(3, release.Cargo.Select(cargo => cargo.SourceLane));

        Flipper straightCargo = Flipper.CreateTankerCargo(plusLevel, release.Cargo[0], 77);
        Flipper cornerCargo = Flipper.CreateTankerCargo(plusLevel, release.Cargo[1], 78);
        Assert.Equal(180, straightCargo.TotalTurnAngle);
        Assert.Equal(90, cornerCargo.TotalTurnAngle);
        Assert.Equal(77, straightCargo.CargoUnitId);
        Assert.Equal(78, cornerCargo.CargoUnitId);
        Assert.Equal(1, straightCargo.Direction);
        Assert.Equal(-1, cornerCargo.Direction);

        for (int tick = 0; tick < 23; tick++)
            straightCargo.Update(plusLevel, playerEdge: 10);
        Assert.Equal(FlipperState.Pausing, straightCargo.State);
        Assert.Equal(1, straightCargo.EdgeIndex);
        for (int tick = 0; tick < 24; tick++)
            straightCargo.Update(plusLevel, playerEdge: 10);
        Assert.Equal(1, straightCargo.Direction);
        Assert.Equal(0, straightCargo.DestinationEdge);

        Tanker wrapTanker = new(edgeIndex: 1, originalSlotId: 1);
        TankerCargoSpawn wrapSpawn = wrapTanker.ReleaseFlipperCargo(
            plusLevel, firstSideDirection: 1, TankerReleaseKind.Natural).Cargo[0];
        Assert.Equal(0, wrapSpawn.SourceLane);
        Assert.Equal(15, wrapSpawn.DestinationLane);
        Assert.Equal(270, Flipper.CreateTankerCargo(plusLevel, wrapSpawn, 79).TotalTurnAngle);

        Level squareLevel = new(LevelConfiguration.Level2Square);
        Tanker squareTanker = new(edgeIndex: 3, originalSlotId: 0);
        Flipper squareCargo = Flipper.CreateTankerCargo(squareLevel,
            squareTanker.ReleaseFlipperCargo(squareLevel, 1, TankerReleaseKind.Natural).Cargo[0], 80);
        Assert.Equal(squareLevel.CalculateFlipperTurnAngle(
            squareCargo.SourceEdge, squareCargo.DestinationEdge), squareCargo.TotalTurnAngle);
        Assert.Equal(180, squareCargo.TotalTurnAngle);

        Level legacyLevel = new(LevelConfiguration.Level1);
        Tanker legacyTanker = new(edgeIndex: 3, originalSlotId: 0);
        Flipper legacyCargo = Flipper.CreateTankerCargo(legacyLevel,
            legacyTanker.ReleaseFlipperCargo(legacyLevel, 1, TankerReleaseKind.Natural).Cargo[0], 81);
        Assert.Equal(legacyCargo.ActiveTransition!.Value.SweepMagnitudeDegrees, legacyCargo.TotalTurnAngle);
    }

    [Fact]
    public void PlusBoardRendererProjectsAllRimsPlayerLanesEntitiesAndFlashWithFiniteGeometry()
    {
        TempestGame game = new(level3GeometryTest: true);
        VectorRenderer renderer = new(game);
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        List<(Vector2 Start, Vector2 End, Color Color)> normal = [];

        renderer.DrawTube((start, end, color) => normal.Add((start, end, color)));
        Assert.Equal(48, normal.Count);
        Assert.Equal(16, normal.Take(16).Count());
        Assert.Equal(16, normal.Skip(16).Take(16).Count());
        Assert.Equal(16, normal.Skip(32).Take(16).Count());
        for (int lane = 0; lane < 16; lane++)
        {
            AssertFinite(normal[lane].Start);
            AssertFinite(normal[lane].End);
            Assert.Equal(Projection.Project(points[lane].X, points[lane].Y, 50, 400, -80),
                normal[lane].Start);
            Assert.Equal(Projection.Project(points[lane].X, points[lane].Y, 450, 400, -80),
                normal[lane].End);
            AssertFinite(normal[16 + lane].Start);
            AssertFinite(normal[16 + lane].End);
            AssertFinite(normal[32 + lane].Start);
            AssertFinite(normal[32 + lane].End);
        }

        foreach (int lane in Enumerable.Range(0, 16))
        {
            game.Player.ResetPosition(lane, 0.5f);
            renderer.GetPlayerEdge(points, out Vector2 start, out Vector2 end);
            AssertFinite(start);
            AssertFinite(end);
        }

        Assert.All(game.Eggs, egg => AssertFinite(renderer.ProjectEgg(egg)));
        foreach (int lane in Enumerable.Range(0, 16))
            AssertFinite(renderer.ProjectShot(new Shot { EdgeIndex = lane, Height = 200 }));
        AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(7, 200)).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(7, 200)).End);
        AssertFinite(renderer.ProjectTankerCenter(new Tanker(7, 0)));

        Flipper ordinary = BeginMidTubePivot(game.CurrentLevel, source: 1, destination: 2);
        ordinary.Update(game.CurrentLevel, playerEdge: 8, (_, _) => 1);
        AssertRendererUsesPivotGeometry(game, renderer, ordinary);

        Tanker cargoTanker = new(edgeIndex: 3, originalSlotId: 0);
        foreach (TankerCargoSpawn spawn in cargoTanker.ReleaseFlipperCargo(
                     game.CurrentLevel, firstSideDirection: 1, TankerReleaseKind.Natural).Cargo)
        {
            Flipper cargo = Flipper.CreateTankerCargo(game.CurrentLevel, spawn, spawn.SourceLane);
            cargo.Update(game.CurrentLevel, playerEdge: 10);
            AssertRendererUsesPivotGeometry(game, renderer, cargo);
        }

        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        List<(Vector2 Start, Vector2 End, Color Color)> flash = [];
        renderer.DrawTube((start, end, color) => flash.Add((start, end, color)));
        Assert.Equal(normal.Select(line => (line.Start, line.End)),
            flash.Select(line => (line.Start, line.End)));
        int playerLane = game.Player.EdgeIndex;
        int nextPlayerLane = game.CurrentLevel.NextEdge(playerLane);
        for (int index = 0; index < flash.Count; index++)
            Assert.Equal(index < 16 && (index == playerLane || index == nextPlayerLane)
                ? new Color(255, 255, 0, 255)
                : new Color(255, 0, 255, 255), flash[index].Color);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void PlusBoardConfiguredSupplyAndRimClearRemainIndependentOfCargoLedger()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        Assert.False(level.TankerDevelopmentWave);
        for (int enemy = 0; enemy < level.EnemySupply; enemy++)
            level.RecordEnemyRimCleared();
        level.ReturnSurvivorsToReserve();

        Assert.Equal(15, level.EnemiesRimCleared);
        Assert.Equal(0, level.RemainingEnemies);
        Assert.Equal(0, level.ReserveEggs);
        Assert.Empty(level.Eggs);
        Assert.True(level.IsComplete(activeFlippers: 0));
        Assert.False(level.IsComplete(activeFlippers: 1));
    }

    private static Flipper BeginMidTubePivot(Level level, int source, int destination)
    {
        Flipper flipper = new(source, height: 100);
        Func<int, int, int> deterministicRandom = (_, _) => 1;
        flipper.Update(level, destination, deterministicRandom);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        flipper.Update(level, destination, deterministicRandom);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(destination, flipper.DestinationEdge);
        return flipper;
    }

    private static float ExpectedSignedHingeAngle(
        IReadOnlyList<Vector2> points, int source, int destination)
    {
        int next = (source + 1) % points.Count;
        int previous = (source - 1 + points.Count) % points.Count;
        int hinge = destination == previous ? source : next;
        Assert.True(destination == previous || destination == next);
        float degrees = ReferenceInteriorAngle(points, hinge);
        return destination == previous ? -degrees : degrees;
    }

    private static float ReferenceInteriorAngle(IReadOnlyList<Vector2> points, int hinge)
    {
        Vector2 previous = points[(hinge - 1 + points.Count) % points.Count] - points[hinge];
        Vector2 next = points[(hinge + 1) % points.Count] - points[hinge];
        float angle = ReferenceAtan2Degrees(previous.X * next.Y - previous.Y * next.X,
            Vector2.Dot(previous, next));
        if (angle == 0)
            angle = 180;
        if (angle != 180)
        {
            float rotation = angle < 90 ? -(179 - angle) : -1;
            float radians = rotation * MathF.PI / 180;
            Vector2 rotated = new(
                previous.X * MathF.Cos(radians) - previous.Y * MathF.Sin(radians),
                previous.X * MathF.Sin(radians) + previous.Y * MathF.Cos(radians));
            if (ReferenceAtan2Degrees(rotated.X * next.Y - rotated.Y * next.X,
                    Vector2.Dot(rotated, next)) > 90)
                angle = 360 - angle;
        }
        return MathF.Abs(angle % 360);
    }

    private static float ReferenceAtan2Degrees(float opposite, float adjacent)
    {
        float angle = MathF.Abs(adjacent) < 0.0001f
            ? 90
            : MathF.Abs(MathF.Atan(opposite / adjacent)) * 180 / MathF.PI;
        if (adjacent < 0)
            angle = 180 - angle;
        if (opposite < 0)
            angle = -angle;
        return angle;
    }

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private static void AssertRendererUsesPivotGeometry(
        TempestGame game, VectorRenderer renderer, Flipper flipper)
    {
        (Vector2 actualStart, Vector2 actualEnd) = renderer.ProjectFlipperEndpoints(flipper);
        float progress = flipper.TurnAngle / flipper.TotalTurnAngle;
        (Vector2 expectedStart, Vector2 expectedEnd) =
            game.CurrentLevel.CalculateFlipperPivotEndpoints(flipper.ActiveTransition!.Value, progress);
        float depth = game.CurrentLevel.Configuration.Position +
            game.CurrentLevel.Configuration.Depth - flipper.Height;
        expectedStart = Projection.Project(expectedStart.X, expectedStart.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        expectedEnd = Projection.Project(expectedEnd.X, expectedEnd.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
        AssertFinite(actualStart);
        AssertFinite(actualEnd);
        Assert.Equal(expectedStart, actualStart);
        Assert.Equal(expectedEnd, actualEnd);
    }
}
