using System.Numerics;
using Raylib_cs;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class DisplayConfigurationTests
{
    [Fact]
    public void WindowPresentationUniformlyScalesTheLogicalFourByThreeCanvas()
    {
        Assert.Equal(800, DisplayConfiguration.LogicalWidth);
        Assert.Equal(600, DisplayConfiguration.LogicalHeight);
        Assert.Equal(1.6f, DisplayConfiguration.WindowScale);
        Assert.Equal(1280, DisplayConfiguration.WindowWidth);
        Assert.Equal(960, DisplayConfiguration.WindowHeight);
        Assert.Equal(DisplayConfiguration.WindowWidth, VectorRenderer.ScreenWidth);
        Assert.Equal(DisplayConfiguration.WindowHeight, VectorRenderer.ScreenHeight);
        Assert.Equal(800f, Projection.Width);
        Assert.Equal(600f, Projection.Height);
        Assert.Equal(
            DisplayConfiguration.WindowWidth / (float)DisplayConfiguration.LogicalWidth,
            DisplayConfiguration.WindowHeight / (float)DisplayConfiguration.LogicalHeight);
        Assert.Equal(
            DisplayConfiguration.LogicalWidth * DisplayConfiguration.WindowHeight,
            DisplayConfiguration.LogicalHeight * DisplayConfiguration.WindowWidth);
        var camera = DisplayConfiguration.PresentationCamera;
        Assert.Equal(DisplayConfiguration.WindowScale, camera.Zoom);
        Assert.Equal(Vector2.Zero, camera.Target);
        Assert.Equal(Vector2.Zero, camera.Offset);
        Assert.Equal(0, camera.Rotation);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(800, 0, 1280, 0)]
    [InlineData(0, 600, 0, 960)]
    [InlineData(800, 600, 1280, 960)]
    [InlineData(400, 300, 640, 480)]
    [InlineData(640, 10, 1024, 16)]
    [InlineData(12.5f, 22.5f, 20, 36)]
    public void NativeCameraMapsLogicalCoordinatesToPhysicalCoordinates(
        float x, float y, float physicalX, float physicalY)
    {
        Vector2 logical = new(x, y);
        Vector2 physical = Raylib.GetWorldToScreen2D(logical, DisplayConfiguration.PresentationCamera);

        Assert.InRange(Vector2.Distance(new(physicalX, physicalY), physical), 0, .001f);
        Assert.InRange(Vector2.Distance(logical,
            Raylib.GetScreenToWorld2D(physical, DisplayConfiguration.PresentationCamera)), 0, .001f);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void PresentationPreservesEstablishedTubeCoordinatesAndProportions(int level)
    {
        LevelConfiguration configuration = level switch
        {
            1 => LevelConfiguration.Level1,
            2 => LevelConfiguration.Level2Square,
            3 => LevelConfiguration.Level3Mixed,
            4 => LevelConfiguration.Level4Candidate,
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
        TempestGame game = new(configuration, random: new Random(1));
        VectorRenderer renderer = new(game);
        Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTube((start, end, _) => lines.Add((start, end)));

        Assert.Equal(70f, Projection.K);
        Assert.Equal(5f, Projection.ZOffset);
        Assert.Equal(originalPoints.Length * 3, lines.Count);
        for (int vertex = 0; vertex < originalPoints.Length; vertex++)
        {
            Vector2 point = originalPoints[vertex];
            Vector2 LegacyProjection(float depth) => new(
                400 + point.X / ((depth + 5) / 70),
                configuration.Definition.CenterY +
                    (point.Y + configuration.Definition.YOffset) / ((depth + 5) / 70));
            Assert.Equal(LegacyProjection(Level.Position), lines[vertex].Start);
            Assert.Equal(LegacyProjection(Level.Position + Level.Depth), lines[vertex].End);
        }

        foreach ((Vector2 start, Vector2 end) in lines)
        {
            Vector2 physicalStart = Raylib.GetWorldToScreen2D(start, DisplayConfiguration.PresentationCamera);
            Vector2 physicalEnd = Raylib.GetWorldToScreen2D(end, DisplayConfiguration.PresentationCamera);
            Assert.InRange(Vector2.Distance(start * 1.6f, physicalStart), 0, .001f);
            Assert.InRange(Vector2.Distance(end * 1.6f, physicalEnd), 0, .001f);
            Assert.InRange(Vector2.Distance((end - start) * 1.6f, physicalEnd - physicalStart), 0, .001f);
            Assert.InRange(physicalStart.X, 0, DisplayConfiguration.WindowWidth);
            Assert.InRange(physicalStart.Y, 0, DisplayConfiguration.WindowHeight);
            Assert.InRange(physicalEnd.X, 0, DisplayConfiguration.WindowWidth);
            Assert.InRange(physicalEnd.Y, 0, DisplayConfiguration.WindowHeight);
        }
        Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(70f, Projection.K);
        Assert.Equal(5f, Projection.ZOffset);
    }
}
