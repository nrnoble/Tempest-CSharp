using System.Numerics;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class DirectedRotationInvestigationTests
{
    [Fact]
    public void DirectedEnemyPivotArcsStayOnTheInteriorSideOfTheirBoardGeometry()
    {
        Level[] levels =
        [
            new(LevelConfiguration.Level1),
            new(LevelConfiguration.Level2Square),
            new(LevelConfiguration.Level3Plus),
            NonRightAngleLevel(),
            ArbitraryConcaveLevel(),
            ReversedPlusBoard()
        ];

        foreach (Level level in levels)
        {
            IReadOnlyList<Vector2> points = level.Definition.Points;
            for (int source = 0; source < points.Count; source++)
            {
                foreach ((int destination, int direction) in new[]
                {
                    (level.PreviousEdge(source), 1),
                    (level.NextEdge(source), -1)
                })
                {
                    DirectedLaneTransition transition =
                        level.CalculateDirectedLaneTransition(source, destination, direction);
                    foreach (float progress in new[] { 0.125f, 0.25f, 0.5f, 0.75f, 0.875f })
                    {
                        (Vector2 start, Vector2 end) =
                            level.CalculateFlipperPivotEndpoints(transition, progress);
                        Vector2 movingEndpoint = direction > 0 ? start : end;

                        Assert.True(IsInsideOrOnPolygon(movingEndpoint, points),
                            $"{level.Definition.Comment}: lane {source}->{destination}, " +
                            $"direction {direction}, hinge {transition.HingeVertex}, " +
                            $"sweep {transition.SweepMagnitudeDegrees}, progress {progress}, " +
                            $"endpoint {movingEndpoint} is outside the tube/web polygon.");
                    }
                }
            }
        }
    }

    [Fact]
    public void SquareTransitionsMatchTheReferenceMagnitudesAndDirectedPaths()
    {
        Level level = new(LevelConfiguration.Level2Square);
        IReadOnlyList<Vector2> points = level.Definition.Points;

        for (int source = 0; source < points.Count; source++)
        {
            foreach ((int destination, int direction) in new[]
            {
                (level.PreviousEdge(source), 1),
                (level.NextEdge(source), -1)
            })
            {
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(source, destination, direction);
                float referenceAngle = ReferenceInteriorAngle(points, transition.HingeVertex);
                Assert.Equal(referenceAngle, transition.SweepMagnitudeDegrees, 3);
                Assert.Equal(direction > 0 ? -referenceAngle : referenceAngle,
                    transition.SignedSweepDegrees, 3);
                Assert.Equal(referenceAngle, MathF.Abs(transition.SpatialSweepDegrees), 3);
                AssertTransitionPath(level, transition);
            }
        }
    }

    [Fact]
    public void PlusBoardAllDirectedTransitionsRetainTheirReferenceSweepAndPath()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        IReadOnlyList<Vector2> points = level.Definition.Points;
        int reflexTransitions = 0;
        int transitionCount = 0;

        for (int source = 0; source < points.Count; source++)
        {
            foreach ((int destination, int direction) in new[]
            {
                (level.PreviousEdge(source), 1),
                (level.NextEdge(source), -1)
            })
            {
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(source, destination, direction);
                transitionCount++;
                float referenceAngle = ReferenceInteriorAngle(points, transition.HingeVertex);
                Assert.Equal(referenceAngle, transition.SweepMagnitudeDegrees, 3);
                Assert.Equal(direction > 0 ? -referenceAngle : referenceAngle,
                    transition.SignedSweepDegrees, 3);
                Assert.Equal(referenceAngle, MathF.Abs(transition.SpatialSweepDegrees), 3);
                AssertTransitionPath(level, transition);

                if (referenceAngle > 180)
                {
                    Assert.Equal(270, referenceAngle, 3);
                    reflexTransitions++;
                }
            }
        }

        Assert.Equal(8, reflexTransitions);
        Assert.Equal(32, transitionCount);
    }

    [Fact]
    public void ArbitraryConcaveGeometryKeepsComplementaryShortAndLongRoutesDistinct()
    {
        Level level = ArbitraryConcaveLevel();
        IReadOnlyList<Vector2> points = level.Definition.Points;

        DirectedLaneTransition shortRoute = level.CalculateDirectedLaneTransition(14, 13, 1);
        DirectedLaneTransition longRoute = level.CalculateDirectedLaneTransition(15, 14, 1);

        Assert.Equal(101.31f, shortRoute.SweepMagnitudeDegrees, 2);
        Assert.Equal(258.69f, longRoute.SweepMagnitudeDegrees, 2);
        Assert.Equal(360, shortRoute.SweepMagnitudeDegrees + longRoute.SweepMagnitudeDegrees, 2);
        Assert.Equal(ReferenceInteriorAngle(points, shortRoute.HingeVertex),
            shortRoute.SweepMagnitudeDegrees, 3);
        Assert.Equal(ReferenceInteriorAngle(points, longRoute.HingeVertex),
            longRoute.SweepMagnitudeDegrees, 3);
        AssertTransitionPath(level, shortRoute);
        AssertTransitionPath(level, longRoute);
    }

    [Fact]
    public void InwardRadiusLimitRemainsContinuousAtTransitionEndpoints()
    {
        Level level = ArbitraryConcaveLevel();
        DirectedLaneTransition transition =
            level.CalculateDirectedLaneTransition(3, 4, direction: -1);
        IReadOnlyList<Vector2> points = level.Definition.Points;

        (Vector2 startNearSource, Vector2 endNearSource) =
            level.CalculateFlipperPivotEndpoints(transition, 0.001f);
        (Vector2 startNearDestination, Vector2 endNearDestination) =
            level.CalculateFlipperPivotEndpoints(transition, 0.999f);

        Assert.True(Vector2.Distance(points[transition.SourceFreeVertex], endNearSource) < 0.1f);
        Assert.True(Vector2.Distance(points[transition.DestinationFreeVertex], endNearDestination) < 0.1f);
        Assert.Equal(points[transition.HingeVertex], startNearSource);
        Assert.Equal(points[transition.HingeVertex], startNearDestination);
    }

    [Fact]
    public void NonRightAngleConvexFixtureUsesItsActualPolygonInteriorSweep()
    {
        Level level = NonRightAngleLevel();
        DirectedLaneTransition transition =
            level.CalculateDirectedLaneTransition(0, 4, direction: 1);

        Assert.InRange(transition.SweepMagnitudeDegrees, 107.5f, 108.5f);
        Assert.Equal(transition.SweepMagnitudeDegrees, MathF.Abs(transition.SpatialSweepDegrees), 3);
        Assert.Equal(-1, Math.Sign(transition.SpatialSweepDegrees));
    }

    [Fact]
    public void FlippersPulsarsAndCargoUseTheFullReflexSweepAndItsDuration()
    {
        Level level = new(LevelConfiguration.Level3Plus);
        Flipper flipper = new(edge: 0, height: 100);
        flipper.Update(level, playerEdge: 15, UnexpectedRandom);
        flipper.Update(level, playerEdge: 15, UnexpectedRandom);

        Assert.Equal(1, flipper.Direction);
        Assert.Equal(15, flipper.DestinationEdge);
        Assert.Equal(270, flipper.TotalTurnAngle);
        Assert.Equal(-270, flipper.ActiveTransition!.Value.SignedSweepDegrees);
        Assert.Equal(270, flipper.ActiveTransition.Value.SpatialSweepDegrees);
        Assert.True(IsInsideOrOnPolygon(
            level.CalculateFlipperPivotEndpoints(flipper.ActiveTransition.Value, 0.5f).Start,
            level.Definition.Points));
        int flipperTicks = 0;
        while (flipper.State == FlipperState.Flipping)
        {
            flipper.Update(level, playerEdge: 15, UnexpectedRandom);
            flipperTicks++;
        }
        Assert.Equal(34, flipperTicks);

        Flipper cargo = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(0, 15, 1, 200), cargoUnitId: 1);
        Assert.Equal(270, cargo.TotalTurnAngle);
        Assert.Equal(-270, cargo.ActiveTransition!.Value.SignedSweepDegrees);
        Assert.Equal(270, cargo.ActiveTransition.Value.SpatialSweepDegrees);
        Assert.True(IsInsideOrOnPolygon(
            level.CalculateFlipperPivotEndpoints(cargo.ActiveTransition.Value, 0.5f).Start,
            level.Definition.Points));

        Pulsar pulsar = new(0, Level.Depth);
        pulsar.Update(level, playerEdge: 15, (_, _) => 1);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        Assert.Equal(1, pulsar.Direction);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            pulsar.Update(level, playerEdge: 15, UnexpectedRandom);

        Assert.Equal(PulsarState.Flipping, pulsar.State);
        Assert.Equal(15, pulsar.EdgeIndex);
        Assert.Equal(270, pulsar.TotalTurnAngle);
        Assert.Equal(-270, pulsar.ActiveTransition!.Value.SignedSweepDegrees);
        Assert.Equal(270, pulsar.ActiveTransition.Value.SpatialSweepDegrees);
        Assert.True(IsInsideOrOnPolygon(
            level.CalculateFlipperPivotEndpoints(pulsar.ActiveTransition.Value, 0.5f).Start,
            level.Definition.Points));
        int pulsarTicks = 0;
        while (pulsar.State == PulsarState.Flipping)
        {
            pulsar.Update(level, playerEdge: 15, UnexpectedRandom);
            pulsarTicks++;
        }
        Assert.Equal(23, pulsarTicks);
        Assert.Equal(PulsarState.RimPause, pulsar.State);
    }

    [Fact]
    public void LevelOneFlippersUseTheReferenceSweepWithoutChangingTheLegacyHelper()
    {
        Level level = new(LevelConfiguration.Level1);
        float headingAngle = level.CalculateEdgeTurnAngle(sourceEdge: 0, destinationEdge: 15);
        float referenceAngle = ReferenceInteriorAngle(level.Definition.Points, hinge: 0);

        Assert.Equal(23.777, headingAngle, 3);
        Assert.Equal(156.223, referenceAngle, 3);
        Assert.Equal(referenceAngle, level.CalculateFlipperTurnAngle(0, 15), 3);
        Assert.Equal(-referenceAngle, level.CalculateEndpointHingeRotationDegrees(0, 15), 3);

        Flipper flipper = new(edge: 0, height: Level.Depth);
        flipper.Update(level, playerEdge: 14);
        for (int tick = 0; tick < 24; tick++)
            flipper.Update(level, playerEdge: 14);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(1, flipper.Direction);
        Assert.Equal(15, flipper.DestinationEdge);
        Assert.Equal(referenceAngle, flipper.TotalTurnAngle, 3);
        int turnTicks = (int)Math.Ceiling(
            flipper.TotalTurnAngle / level.RimTurnSpeedDegrees);
        for (int tick = 0; tick < turnTicks - 1; tick++)
            flipper.Update(level, playerEdge: 14);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        flipper.Update(level, playerEdge: 14);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(15, flipper.EdgeIndex);
    }

    [Theory]
    [InlineData("square", true, 3, 4, 90, 10)]
    [InlineData("square", true, 0, 1, 180, 20)]
    [InlineData("plus", true, 15, 0, 270, 30)]
    [InlineData("square", false, 4, 3, 90, 10)]
    [InlineData("square", false, 1, 0, 180, 20)]
    [InlineData("plus", false, 0, 15, 270, 30)]
    public void BlasterCrossingTimeScalesWithDirectedHingeAngle(
        string board, bool positive, int sourceLane, int destinationLane, float angle, int ticks)
    {
        Level level = new(board == "plus"
            ? LevelConfiguration.Level3Plus
            : LevelConfiguration.Level2Square);
        Player player = new();
        player.ResetPosition(sourceLane, 0);

        for (int tick = 0; tick < ticks; tick++)
            player.MoveAlongTube(positive ? 0.05f : -0.05f, level);

        if (positive)
        {
            Assert.Equal(sourceLane, player.EdgeIndex);
            Assert.Equal(1, player.EdgePosition, 4);
        }
        else
        {
            Assert.Equal(destinationLane, player.EdgeIndex);
            Assert.Equal(0, player.EdgePosition, 4);
        }
        Assert.Equal(angle, level.CalculateLaneTransitionAngle(sourceLane, destinationLane), 3);
    }

    [Fact]
    public void ArbitraryAngleBlasterTraversalUsesProportionalShortAndLongDurations()
    {
        Level level = ArbitraryConcaveLevel();
        Player player = new();
        player.ResetPosition(13, 0);
        int shortTicks = TraverseUntilLaneChanges(player, level, 0.05f, 14);
        Assert.Equal(12, shortTicks);
        Assert.Equal(14, player.EdgeIndex);

        player.ResetPosition(14, 0);
        int longTicks = TraverseUntilLaneChanges(player, level, 0.05f, 15);
        Assert.Equal(29, longTicks);
        Assert.Equal(15, player.EdgeIndex);
    }

    private static Level ArbitraryConcaveLevel()
    {
        Vector2[] points =
        [
            new(0, 0), new(-10, 10), new(-20, 20), new(-30, 30),
            new(-40, 40), new(-50, 50), new(-50, -50), new(-20, -50),
            new(20, -50), new(50, -50), new(50, -20), new(50, 20),
            new(50, 50), new(30, 50), new(20, 50), new(10, 0)
        ];
        return new Level(LevelConfiguration.Level3Plus with
        {
            Definition = new LevelDefinition(true, 400, -80, points, "arbitrary concave hinge")
        });
    }

    private static Level NonRightAngleLevel()
    {
        Vector2[] points =
        [
            new(0, 100), new(95, 31), new(59, -81), new(-59, -81), new(-95, 31)
        ];
        return new Level(LevelConfiguration.Level3Plus with
        {
            Definition = new LevelDefinition(true, 400, -80, points, "non-right-angle tube"),
            EnemySupply = 0,
            InitialActiveEggCount = 0,
            SpawnLaneSequence = Array.AsReadOnly(new[] { 0 }),
            EggDirectionSequence = Array.AsReadOnly(new[] { 1 })
        });
    }

    private static Level ReversedPlusBoard()
    {
        Vector2[] points = LevelConfiguration.Level3Plus.Definition.Points.Reverse().ToArray();
        return new Level(LevelConfiguration.Level3Plus with
        {
            Definition = LevelConfiguration.Level3Plus.Definition with { Points = points }
        });
    }

    private static int TraverseUntilLaneChanges(Player player, Level level, float movement, int destination)
    {
        int ticks = 0;
        while (player.EdgeIndex != destination && ticks < 100)
        {
            player.MoveAlongTube(movement, level);
            ticks++;
        }
        Assert.Equal(destination, player.EdgeIndex);
        return ticks;
    }

    private static void AssertTransitionPath(Level level, DirectedLaneTransition transition)
    {
        IReadOnlyList<Vector2> points = level.Definition.Points;
        Vector2 hinge = points[transition.HingeVertex];
        Vector2 initial = points[transition.SourceFreeVertex] - hinge;
        Vector2 destination = points[transition.DestinationFreeVertex] - hinge;

        foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
        {
            (Vector2 actualStart, Vector2 actualEnd) =
                level.CalculateFlipperPivotEndpoints(transition, progress);
            Vector2 expectedMoving;
            if (progress == 0)
                expectedMoving = points[transition.SourceFreeVertex];
            else if (progress == 1)
                expectedMoving = points[transition.DestinationFreeVertex];
            else
            {
                float angle = transition.SpatialSweepDegrees * progress * MathF.PI / 180;
                Vector2 rotated = new(
                    initial.X * MathF.Cos(angle) - initial.Y * MathF.Sin(angle),
                    initial.X * MathF.Sin(angle) + initial.Y * MathF.Cos(angle));
                float radius = initial.Length() + (destination.Length() - initial.Length()) * progress;
                expectedMoving = hinge + Vector2.Normalize(rotated) * radius;
            }

            Vector2 expectedStart;
            Vector2 expectedEnd;
            if (progress == 0)
            {
                expectedStart = points[transition.SourceLane];
                expectedEnd = points[level.NextEdge(transition.SourceLane)];
            }
            else
            {
                expectedStart = transition.HingeIsStart ? hinge : expectedMoving;
                expectedEnd = transition.HingeIsStart ? expectedMoving : hinge;
            }
            Assert.True(Vector2.Distance(expectedStart, actualStart) < 0.001f,
                $"{transition.SourceLane}->{transition.DestinationLane} direction {transition.Direction} at {progress}: {expectedStart} != {actualStart}");
            Assert.True(Vector2.Distance(expectedEnd, actualEnd) < 0.001f,
                $"{transition.SourceLane}->{transition.DestinationLane} direction {transition.Direction} at {progress}: {expectedEnd} != {actualEnd}");
        }

        (Vector2 finalStart, Vector2 finalEnd) =
            level.CalculateFlipperPivotEndpoints(transition, 1);
        Assert.Equal(points[transition.DestinationLane], finalStart);
        Assert.Equal(points[level.NextEdge(transition.DestinationLane)], finalEnd);
    }

    private static float ReferenceInteriorAngle(IReadOnlyList<Vector2> points, int hinge)
    {
        Vector2 previousRay = points[(hinge - 1 + points.Count) % points.Count] - points[hinge];
        Vector2 nextRay = points[(hinge + 1) % points.Count] - points[hinge];
        float angle = ReferenceAtan2Degrees(Cross(previousRay, nextRay), Vector2.Dot(previousRay, nextRay));
        if (angle == 0)
            angle = 180;
        if (angle != 180)
        {
            float rotation = angle < 90 ? -(179 - angle) : -1;
            float radians = rotation * MathF.PI / 180;
            Vector2 rotated = new(
                previousRay.X * MathF.Cos(radians) - previousRay.Y * MathF.Sin(radians),
                previousRay.X * MathF.Sin(radians) + previousRay.Y * MathF.Cos(radians));
            if (ReferenceAtan2Degrees(Cross(rotated, nextRay), Vector2.Dot(rotated, nextRay)) > 90)
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

    private static float Cross(Vector2 left, Vector2 right) => left.X * right.Y - left.Y * right.X;

    private static bool IsInsideOrOnPolygon(Vector2 point, IReadOnlyList<Vector2> polygon)
    {
        bool inside = false;
        for (int current = 0, previous = polygon.Count - 1;
             current < polygon.Count;
             previous = current++)
        {
            Vector2 start = polygon[previous];
            Vector2 end = polygon[current];
            Vector2 segment = end - start;
            Vector2 offset = point - start;
            if (MathF.Abs(Cross(segment, offset)) < 0.001f &&
                Vector2.Dot(offset, point - end) <= 0)
                return true;

            if ((start.Y > point.Y) != (end.Y > point.Y) &&
                point.X < (end.X - start.X) * (point.Y - start.Y) /
                (end.Y - start.Y) + start.X)
                inside = !inside;
        }

        return inside;
    }

    private static int UnexpectedRandom(int minimum, int maximum) =>
        throw new InvalidOperationException($"Unexpected random request [{minimum}, {maximum}].");
}
