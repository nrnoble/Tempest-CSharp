using System.Numerics;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level16GeometryTests
{
    private static readonly byte[] ExpectedX =
    [
        0x80, 0x70, 0x48, 0x20, 0x10, 0x20, 0x48, 0x70,
        0x80, 0x90, 0xB8, 0xE0, 0xF0, 0xE0, 0xB8, 0x90
    ];

    private static readonly byte[] ExpectedY =
    [
        0x80, 0x57, 0x48, 0x57, 0x80, 0xA9, 0xBA, 0xA9,
        0x80, 0x57, 0x48, 0x57, 0x80, 0xA9, 0xBA, 0xA9
    ];

    private static readonly byte[] ExpectedAngles =
    [
        0x0B, 0x09, 0x07, 0x05, 0x03, 0x01, 0x0F, 0x0D,
        0x0D, 0x0F, 0x01, 0x03, 0x05, 0x07, 0x09, 0x0B
    ];

    [Fact]
    public void RomRowElevenPreservesOrderedClosedZeroAreaGeometry()
    {
        LevelDefinition definition = LevelLoader.LoadLevel16ClosedTube();
        const float scale = 25f / 14f;

        Assert.Equal(ExpectedX, LevelLoader.Level16RomEvidence.X);
        Assert.Equal(ExpectedY, LevelLoader.Level16RomEvidence.Y);
        Assert.Equal(ExpectedAngles, LevelLoader.Level16RomEvidence.Angles);
        Assert.True(definition.Continuous);
        Assert.Equal(16, definition.PlayableLaneCount);
        Assert.Equal(definition.Points[0], definition.Points[8]);
        for (int index = 0; index < 16; index++)
        {
            Assert.Equal((ExpectedX[index] - 128) * scale, definition.Points[index].X);
            Assert.Equal(-(ExpectedY[index] - 128) * scale, definition.Points[index].Y);
        }

        double twiceArea = 0;
        for (int index = 0; index < definition.Points.Count; index++)
        {
            Vector2 current = definition.Points[index];
            Vector2 next = definition.Points[(index + 1) % definition.Points.Count];
            twiceArea += (double)current.X * next.Y - (double)current.Y * next.X;
        }

        Assert.Equal(0, twiceArea, precision: 3);
    }

    [Fact]
    public void SelfTouchingClosedOutlineSupportsEveryCyclicAdjacentPivot()
    {
        Level level = CreateGeometryLevel();

        Assert.Equal(16, level.PlayableLaneCount);
        Assert.True(level.Continuous);
        Assert.Equal(level.Definition.Points[0], level.Definition.Points[8]);
        Assert.True(level.TryGetAdjacentLane(15, 1, out int wrappedLane));
        Assert.Equal(0, wrappedLane);
        Assert.Equal(0, level.NextEdge(15));
        Assert.Equal(15, level.PreviousEdge(0));

        for (int lane = 0; lane < level.PlayableLaneCount; lane++)
        {
            foreach (int direction in new[] { -1, 1 })
            {
                int laneDelta = direction > 0 ? -1 : 1;
                Assert.True(level.TryGetAdjacentLane(lane, laneDelta, out int destination));
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(lane, destination, direction);

                foreach (float progress in new[] { 0f, 0.25f, 0.5f, 0.75f, 1f })
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(transition, progress);
                    Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
                    Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
                }
            }
        }
    }

    [Fact]
    public void SimpleClosedTubesKeepTheirAcceptedGlobalWindingAndTransitionDirection()
    {
        foreach (LevelConfiguration configuration in new[]
                 {
                     LevelConfiguration.Level1,
                     LevelConfiguration.Level2Square,
                     LevelConfiguration.Level13Production
                 })
        {
            Level level = new(configuration);
            int expectedWinding = configuration == LevelConfiguration.Level13Production ? -1 : 1;
            Assert.Equal(expectedWinding, level.ClockwiseLaneIndexDelta);

            Assert.True(level.TryGetAdjacentLane(
                0, expectedWinding, out int clockwiseDestination));
            DirectedLaneTransition clockwise =
                level.CalculateDirectedLaneTransition(
                    0, clockwiseDestination, expectedWinding > 0 ? -1 : 1);
            Assert.True(float.IsFinite(clockwise.SpatialSweepDegrees));
            Assert.True(clockwise.SweepMagnitudeDegrees > 0);
        }
    }

    private static Level CreateGeometryLevel()
    {
        LevelDefinition definition = LevelLoader.LoadLevel16ClosedTube();
        LevelConfiguration configuration = new(
            ProgressionIndex: 15,
            DisplayedLevel: 16,
            BoardIndex: 15,
            PaletteIndex: 0,
            Definition: definition,
            Depth: Level.Depth,
            Position: Level.Position,
            InitialPlayerLane: 0,
            InitialPlayerLanePosition: 0.5f,
            EnemySupply: 1,
            InitialActiveEggCount: 0,
            ReserveReleaseIntervalTicks: 50,
            MaximumActiveEggs: 4,
            MaximumApproachingEggs: 2,
            HatchSelectionIntervalTicks: 50,
            SpawnLaneSequence: Enumerable.Range(0, 16).ToArray(),
            EggDirectionSequence: new[] { 1, -1 },
            ExitPolicy: LevelExitPolicy.Standard,
            NormalEnemyComposition: LevelEnemyComposition.FlippersOnly)
        {
            FlipperPivotMode = FlipperPivotGeometry.EndpointHinge
        };
        return new Level(configuration);
    }

}
