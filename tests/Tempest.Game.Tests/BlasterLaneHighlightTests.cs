using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class BlasterLaneHighlightTests
{
    private static readonly Color BlasterLaneColor = new(255, 255, 0, 255);
    private static readonly Color[] ZapRailColors =
    [
        Color.White, BlasterLaneColor, new(0, 255, 0, 255), Color.Red,
        new(0, 255, 255, 255), new(255, 0, 255, 255), Color.Blue
    ];

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void TubeColorsExactlyTheTwoTopologyBoundariesForEachBoard(int board)
    {
        TempestGame game = board switch
        {
            0 => new TempestGame(),
            1 => new TempestGame(level2Test: true),
            2 => new TempestGame(level3GeometryTest: true),
            _ => throw new ArgumentOutOfRangeException(nameof(board))
        };
        VectorRenderer renderer = new(game);
        IReadOnlyList<Vector2> originalPoints = game.CurrentLevel.Definition.Points.ToArray();
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        for (int lane = 0; lane < points.Count; lane++)
        {
            game.Player.ResetPosition(lane, 0.37f);
            List<(Vector2 Start, Vector2 End, Color Color)> lines = CaptureTube(renderer);
            int nextBoundary = game.CurrentLevel.NextEdge(lane);

            Assert.Equal(points.Count * 3, lines.Count);
            for (int vertex = 0; vertex < points.Count; vertex++)
            {
                Color expected = vertex == lane || vertex == nextBoundary
                    ? BlasterLaneColor
                    : Color.Blue;
                Assert.Equal(expected, lines[vertex].Color);
                Assert.Equal(ProjectVertex(game, vertex, Level.Position), lines[vertex].Start);
                Assert.Equal(ProjectVertex(game, vertex, Level.Position + Level.Depth), lines[vertex].End);
                Assert.True(float.IsFinite(lines[vertex].Start.X));
                Assert.True(float.IsFinite(lines[vertex].Start.Y));
                Assert.True(float.IsFinite(lines[vertex].End.X));
                Assert.True(float.IsFinite(lines[vertex].End.Y));
            }

            Assert.All(lines.Skip(points.Count), line => Assert.Equal(Color.Blue, line.Color));
        }

        Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points);
    }

    [Fact]
    public void FractionalMovementKeepsOneLaneAndLaneChangeSwitchesBoundariesOnce()
    {
        TempestGame game = new(level2Test: true);
        VectorRenderer renderer = new(game);
        int initialLane = game.Player.EdgeIndex;
        int laneCount = game.CurrentLevel.PlayableLaneCount;

        game.Player.Move(0.25f, laneCount);
        Assert.Equal(initialLane, game.Player.EdgeIndex);
        AssertHighlightedBoundaries(game, renderer, initialLane);

        game.Player.Move(0.25f, laneCount);
        Assert.Equal(1, game.Player.EdgePosition);
        Assert.Equal(initialLane, game.Player.EdgeIndex);
        AssertHighlightedBoundaries(game, renderer, initialLane);

        game.Player.Move(0.01f, laneCount);
        int destinationLane = game.CurrentLevel.NextEdge(initialLane);
        Assert.Equal(destinationLane, game.Player.EdgeIndex);
        AssertHighlightedBoundaries(game, renderer, destinationLane);
    }

    [Fact]
    public void ApproachHighlightTracksPlayerAndKeepsApproachProjection()
    {
        TempestGame game = EnterNextLevelApproach();
        VectorRenderer renderer = new(game);
        Assert.Equal(LevelState.Approach, game.State);
        LevelApproach approach = game.CurrentApproach ??
            throw new InvalidOperationException("The level approach was not initialized.");
        int approachTicks = approach.ElapsedTicks;
        float approachOffset = approach.DepthOffset;

        AssertHighlightedBoundaries(game, renderer, game.Player.EdgeIndex);
        Assert.Equal(approachTicks, approach.ElapsedTicks);
        Assert.Equal(approachOffset, approach.DepthOffset);
        int initialLane = game.Player.EdgeIndex;
        for (int tick = 0; tick < 40 && game.Player.EdgeIndex == initialLane; tick++)
            game.RunOneStep(new PlayerInput(false, 0, false, true));

        Assert.NotEqual(initialLane, game.Player.EdgeIndex);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = CaptureTube(renderer);
        AssertLaneRails(game, lines, game.Player.EdgeIndex, BlasterLaneColor);
        approachOffset = approach.DepthOffset;
        Vector2 point = game.CurrentLevel.Definition.Points[game.Player.EdgeIndex];
        Vector2 expectedNear = Projection.Project(point.X, point.Y,
            Level.Position + approachOffset, game.CurrentLevel.Definition.CenterY,
            game.CurrentLevel.Definition.YOffset + renderer.ApproachVerticalOffset);
        Assert.Equal(expectedNear, lines[game.Player.EdgeIndex].Start);
        Assert.All(lines, line =>
        {
            Assert.True(float.IsFinite(line.Start.X));
            Assert.True(float.IsFinite(line.Start.Y));
            Assert.True(float.IsFinite(line.End.X));
            Assert.True(float.IsFinite(line.End.Y));
        });
    }

    [Theory]
    [InlineData(LevelState.PlayerDying, true)]
    [InlineData(LevelState.AvoidSpikes, true)]
    [InlineData(LevelState.ExitDelay, false)]
    [InlineData(LevelState.ExitZoom, false)]
    [InlineData(LevelState.GameOver, false)]
    [InlineData(LevelState.Complete, false)]
    public void HighlightVisibilityFollowsGameplayPresentation(LevelState state, bool visible)
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        game.CurrentLevel.State = state;

        List<(Vector2 Start, Vector2 End, Color Color)> lines = CaptureTube(renderer);
        int lane = game.Player.EdgeIndex;
        int next = game.CurrentLevel.NextEdge(lane);
        for (int vertex = 0; vertex < game.CurrentLevel.Definition.Points.Count; vertex++)
        {
            bool highlighted = vertex == lane || vertex == next;
            Assert.Equal(visible && highlighted ? BlasterLaneColor : Color.Blue, lines[vertex].Color);
        }
    }

    [Fact]
    public void BlasterHighlightWinsOverSuperZapperButPulsarOverlayWinsOverHighlight()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        int lane = game.Player.EdgeIndex;
        Assert.True(game.SuperZapper.TryActivate([], lane, 16, continuous: true));

        for (int phase = 0; phase < 8; phase++)
        {
            List<(Vector2 Start, Vector2 End, Color Color)> flashLines = CaptureTube(renderer);
            AssertLaneRails(game, flashLines, lane, BlasterLaneColor);
            Assert.Equal(ZapRailColors[phase == 7 ? 1 : phase], flashLines[0].Color);
            Assert.All(flashLines.Skip(16), line => Assert.Equal(Color.Blue, line.Color));
            game.SuperZapper.AdvanceDisplayStep();
        }

        TempestGame pulsarGame = HatchPulsarAndStartPulse();
        VectorRenderer pulsarRenderer = new(pulsarGame);
        Pulsar pulsar = Assert.Single(pulsarGame.Pulsars, candidate =>
            Pulsar.IsLanePulsing(candidate.Height, pulsarGame.Pulsing));
        pulsarGame.Player.ResetPosition(pulsar.EdgeIndex, 0.5f);
        List<(Vector2 Start, Vector2 End, Color Color)> normalPulsars = [];
        pulsarRenderer.DrawPulsars(
            (start, end, color) => normalPulsars.Add((start, end, color)));
        Assert.NotEmpty(normalPulsars);
        Assert.True(pulsarGame.SuperZapper.TryActivate([], pulsar.EdgeIndex, 16, continuous: true));
        for (int phase = 0; phase < 8; phase++)
        {
            List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = CaptureTube(pulsarRenderer);
            AssertLaneRails(pulsarGame, tubeLines, pulsar.EdgeIndex, BlasterLaneColor);

            List<(Vector2 Start, Vector2 End, Color Color)> overlays = [];
            pulsarRenderer.DrawPulsingLaneBoundaries(
                (start, end, color) => overlays.Add((start, end, color)));
            Assert.Equal(2, overlays.Count);
            Assert.All(overlays, line => Assert.Equal(new Color(0, 255, 255, 255), line.Color));
            Assert.Contains((tubeLines[pulsar.EdgeIndex].Start, tubeLines[pulsar.EdgeIndex].End,
                new Color(0, 255, 255, 255)), overlays);
            int nextBoundary = pulsarGame.CurrentLevel.NextEdge(pulsar.EdgeIndex);
            Assert.Contains((tubeLines[nextBoundary].Start, tubeLines[nextBoundary].End,
                new Color(0, 255, 255, 255)), overlays);
            List<(Vector2 Start, Vector2 End, Color Color)> activePulsars = [];
            pulsarRenderer.DrawPulsars(
                (start, end, color) => activePulsars.Add((start, end, color)));
            Assert.Equal(normalPulsars, activePulsars);
            pulsarGame.SuperZapper.AdvanceDisplayStep();
        }
    }

    private static TempestGame EnterNextLevelApproach()
    {
        TempestGame game = new();
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();
        return game;
    }

    private static TempestGame HatchPulsarAndStartPulse()
    {
        TempestGame game = new(pulsarTest: true, random: new MinimumRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        game.RunOneStep();
        for (int tick = 0; tick < 70; tick++)
            game.RunOneStep();
        Assert.True(game.Pulsing);
        return game;
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
        return lines;
    }

    private static void AssertHighlightedBoundaries(TempestGame game, VectorRenderer renderer, int lane)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = CaptureTube(renderer);
        AssertLaneRailsForPoints(lines, game, lane);
    }

    private static void AssertLaneRails(
        TempestGame game,
        IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> lines,
        int lane,
        Color color)
    {
        int nextBoundary = game.CurrentLevel.NextEdge(lane);
        Assert.Equal(color, lines[lane].Color);
        Assert.Equal(color, lines[nextBoundary].Color);
        for (int vertex = 0; vertex < game.CurrentLevel.Definition.Points.Count; vertex++)
            if (vertex != lane && vertex != nextBoundary)
                Assert.Equal(ZapRailColors[game.SuperZapper.TubeColorIndex], lines[vertex].Color);
    }

    private static void AssertLaneRailsForPoints(
        IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> lines,
        TempestGame game,
        int lane)
    {
        int count = game.CurrentLevel.Definition.Points.Count;
        int nextBoundary = game.CurrentLevel.NextEdge(lane);
        for (int vertex = 0; vertex < count; vertex++)
            Assert.Equal(vertex == lane || vertex == nextBoundary ? BlasterLaneColor : Color.Blue,
                lines[vertex].Color);
    }

    private static Vector2 ProjectVertex(TempestGame game, int vertex, float depth)
    {
        Vector2 point = game.CurrentLevel.Definition.Points[vertex];
        return Projection.Project(point.X, point.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
