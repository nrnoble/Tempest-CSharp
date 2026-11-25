using System.Numerics;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level16RomFlipperTransitionTests
{
    private static readonly float[] RepresentativeProgress =
        [0, 0.125f, 0.25f, 0.5f, 0.75f, 0.875f, 1];

    [Theory]
    [InlineData(0, 15, 1, 0x0B, 0x03, 1, 8, -180)]
    [InlineData(15, 0, -1, 0x03, 0x0B, -1, 8, 180)]
    [InlineData(7, 8, -1, 0x05, 0x0D, -1, 8, 180)]
    [InlineData(8, 7, 1, 0x0D, 0x05, 1, 8, -180)]
    public void SharedCenterPathUsesRomLaneAnglesAndDirectedPhaseOrder(
        int sourceLane,
        int destinationLane,
        int direction,
        int startPhase,
        int targetPhase,
        int phaseDirection,
        int phaseSteps,
        float hostSweepDegrees)
    {
        Level level = new(LevelConfiguration.Level16Production);
        DirectedLaneTransition transition =
            level.CalculateDirectedLaneTransition(sourceLane, destinationLane, direction);

        RomFlipperAnglePath path = Assert.IsType<RomFlipperAnglePath>(
            transition.RomAnglePath);
        Assert.Equal(sourceLane, path.SourceLane);
        Assert.Equal(destinationLane, path.DestinationLane);
        Assert.Equal(direction < 0, path.RomDirectionBitSet);
        Assert.Equal(startPhase, path.StartPhase);
        Assert.Equal(targetPhase, path.TargetPhase);
        Assert.Equal(phaseDirection, path.PhaseDirection);
        Assert.Equal(phaseSteps, path.PhaseSteps);
        Assert.Equal(hostSweepDegrees, path.HostSweepDegrees);

        Vector2 pivot = level.Definition.Points[transition.HingeVertex];
        Vector2 initial = level.Definition.Points[transition.SourceFreeVertex] - pivot;
        (Vector2 start, Vector2 end) =
            level.CalculateFlipperPivotEndpoints(transition, 0.5f);
        Vector2 moved = transition.HingeIsStart ? end : start;
        Vector2 halfTurn = moved - pivot;
        float measuredDegrees = MathF.Atan2(
            initial.X * halfTurn.Y - initial.Y * halfTurn.X,
            Vector2.Dot(initial, halfTurn)) * 180 / MathF.PI;
        Assert.Equal(hostSweepDegrees / 2, measuredDegrees, precision: 3);
        Assert.True(float.IsFinite(halfTurn.X) && float.IsFinite(halfTurn.Y));
        Assert.True(halfTurn.LengthSquared() > 0);
    }

    [Fact]
    public void EveryDirectedLevelSixteenTransitionProjectsFiniteEndpointsAtBoundedProgress()
    {
        Tempest.Game.TempestGame game = new(
            LevelConfiguration.Level16Production, random: new Random(1981));
        Level level = game.CurrentLevel;
        VectorRenderer renderer = new(game);
        HashSet<(int Source, int Destination)> romAlignedTransitions = [];

        for (int sourceLane = 0; sourceLane < level.PlayableLaneCount; sourceLane++)
        {
            foreach (int direction in new[] { -1, 1 })
            {
                int laneDelta = direction > 0 ? -1 : 1;
                Assert.True(level.TryGetAdjacentLane(
                    sourceLane, laneDelta, out int destinationLane));
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(
                        sourceLane, destinationLane, direction);
                bool sharedCenterTransition = IsRepeatedVertex(
                    level.Definition.Points, transition.HingeVertex);
                Flipper projectedFlipper = Flipper.CreateTankerCargo(
                    level,
                    new TankerCargoSpawn(sourceLane, destinationLane, direction, 10),
                    cargoUnitId: sourceLane * 2 + (direction > 0 ? 1 : 0),
                    tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);

                if (sharedCenterTransition)
                {
                    Assert.NotNull(transition.RomAnglePath);
                    romAlignedTransitions.Add((sourceLane, destinationLane));
                }
                else
                {
                    Assert.Null(transition.RomAnglePath);
                }

                foreach (float progress in RepresentativeProgress)
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(transition, progress);
                    AssertFinite(start);
                    AssertFinite(end);
                    if (progress is > 0 and < 1)
                        Assert.True(Vector2.DistanceSquared(start, end) > 0);

                    if (progress == 1)
                        continue;

                    while (projectedFlipper.State == FlipperState.Flipping &&
                        projectedFlipper.TurnAngle / projectedFlipper.TotalTurnAngle < progress)
                        projectedFlipper.Update(level, playerEdge: 0);

                    Assert.Equal(FlipperState.Flipping, projectedFlipper.State);
                    (Vector2 projectedStart, Vector2 projectedEnd) =
                        renderer.ProjectFlipperEndpoints(projectedFlipper);
                    AssertFinite(projectedStart);
                    AssertFinite(projectedEnd);
                }
            }
        }

        Assert.Equal(4, romAlignedTransitions.Count);
        Assert.Contains((0, 15), romAlignedTransitions);
        Assert.Contains((15, 0), romAlignedTransitions);
        Assert.Contains((7, 8), romAlignedTransitions);
        Assert.Contains((8, 7), romAlignedTransitions);
    }

    [Fact]
    public void TankerCargoRepeating24LaneZeroToFifteenProjectsWithoutBoundaryIntersection()
    {
        Tempest.Game.TempestGame game = new(
            LevelConfiguration.Level16Production, random: new Random(1981));
        Level level = game.CurrentLevel;
        Tanker tanker = new(edgeIndex: 1, originalSlotId: 41);
        TankerReleaseResult release = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);
        TankerCargoSpawn cargo = release.Cargo[0];

        Assert.Equal(0, cargo.SourceLane);
        Assert.Equal(15, cargo.DestinationLane);
        Assert.Equal(1, cargo.Direction);

        TankerCargoMovementProgram movementProgram =
            tanker.GetCargoMovementProgram(
                LevelConfiguration.Level16Production, TankerReleaseKind.Natural);
        Assert.Equal(TankerCargoMovementProgram.Repeating24, movementProgram);
        Flipper flipper = Flipper.CreateTankerCargo(
            level, cargo, cargoUnitId: 82,
            tankerCargoMovementProgram: movementProgram);

        for (int tick = 0; tick < 18; tick++)
            flipper.Update(level, playerEdge: 0);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(0, flipper.SourceEdge);
        Assert.Equal(15, flipper.DestinationEdge);
        Assert.Equal(144, flipper.TurnAngle);
        Assert.Equal(180, flipper.TotalTurnAngle);

        DirectedLaneTransition transition = Assert.IsType<DirectedLaneTransition>(
            flipper.ActiveTransition);
        Assert.Equal(0x0B, transition.RomAnglePath!.Value.StartPhase);
        Assert.Equal(0x03, transition.RomAnglePath.Value.TargetPhase);
        Assert.Equal(1, transition.RomAnglePath.Value.PhaseDirection);
        Assert.Equal(8, transition.RomAnglePath.Value.PhaseSteps);
        Assert.Equal(-180, transition.RomAnglePath.Value.HostSweepDegrees);

        (Vector2 modelStart, Vector2 modelEnd) =
            level.CalculateFlipperPivotEndpoints(transition, 0.8f);
        AssertFinite(modelStart);
        AssertFinite(modelEnd);
        Assert.True(Vector2.DistanceSquared(modelStart, modelEnd) > 0);

        VectorRenderer renderer = new(game);
        (Vector2 projectedStart, Vector2 projectedEnd) =
            renderer.ProjectFlipperEndpoints(flipper);
        AssertFinite(projectedStart);
        AssertFinite(projectedEnd);
        Assert.True(Vector2.DistanceSquared(projectedStart, projectedEnd) > 0);
    }

    private static bool IsRepeatedVertex(IReadOnlyList<Vector2> points, int vertex)
    {
        for (int index = 0; index < points.Count; index++)
            if (index != vertex && points[index] == points[vertex])
                return true;
        return false;
    }

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }
}
