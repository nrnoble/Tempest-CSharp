using System.Numerics;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class VectorFontTests
{
    [Fact]
    public void AvoidSpikesTextUsesDeterministicSharedVectorGlyphsAtTheRequestedSizeAndCenter()
    {
        const string text = "AVOID SPIKES";
        Vector2 center = new(400, 288);
        const float scale = 8;
        List<(Vector2 Start, Vector2 End)> first = [];
        List<(Vector2 Start, Vector2 End)> second = [];

        VectorFont.DrawCenteredText(text, center, scale, (start, end) => first.Add((start, end)));
        VectorFont.DrawCenteredText(text, center, scale, (start, end) => second.Add((start, end)));

        Assert.Equal(59, first.Count);
        Assert.NotEmpty(first);
        Assert.Equal(first, second);
        Vector2[] points = first.SelectMany(line => new[] { line.Start, line.End }).ToArray();
        Assert.Equal(center, BoundsCenter(points));
        Assert.Equal(new Vector2(552, 56), BoundsSize(points));
        Assert.All(points, point =>
        {
            Assert.True(float.IsFinite(point.X));
            Assert.True(float.IsFinite(point.Y));
        });
    }

    [Fact]
    public void GameOverKeepsItsExistingGlyphGeometryScaleAndCenterThroughTheSharedPath()
    {
        Vector2 center = new(400, 300);
        List<(Vector2 Start, Vector2 End)> gameOver = [];
        List<(Vector2 Start, Vector2 End)> sharedPath = [];

        VectorFont.DrawGameOver(center, 4, (start, end) => gameOver.Add((start, end)));
        VectorFont.DrawCenteredText("GAME OVER", center, 4,
            (start, end) => sharedPath.Add((start, end)));

        Assert.Equal(44, gameOver.Count);
        Assert.Equal(gameOver, sharedPath);
        Vector2[] points = gameOver.SelectMany(line => new[] { line.Start, line.End }).ToArray();
        Assert.Equal(center, BoundsCenter(points));
        Assert.Equal(new Vector2(204, 28), BoundsSize(points));
    }

    private static Vector2 BoundsCenter(IEnumerable<Vector2> points)
    {
        Vector2[] values = points.ToArray();
        return new Vector2(
            (values.Min(point => point.X) + values.Max(point => point.X)) / 2,
            (values.Min(point => point.Y) + values.Max(point => point.Y)) / 2);
    }

    private static Vector2 BoundsSize(IEnumerable<Vector2> points)
    {
        Vector2[] values = points.ToArray();
        return new Vector2(
            values.Max(point => point.X) - values.Min(point => point.X),
            values.Max(point => point.Y) - values.Min(point => point.Y));
    }
}
