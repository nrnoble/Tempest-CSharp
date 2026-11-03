using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PlayerShotRenderingTests
{
    [Fact]
    public void PlayerShotStarIsSlightlyLargerAndShrinksWithProjectedDepth()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End)> lines = [];

        renderer.DrawPlayerShots((start, end) => lines.Add((start, end)));

        Assert.Equal(8, lines.Count);
        float nearRadius = AssertLineLength(lines, expectedRadius: ExpectedRadius(game, shot));
        float previousRadius = 4 /
            ((game.CurrentLevel.Configuration.Depth - shot.Height +
              game.CurrentLevel.Configuration.Position + Projection.ZOffset) / Projection.K);
        Assert.True(nearRadius > previousRadius);
        Assert.Equal(4.5f, VectorRenderer.PlayerShotBaseSize);

        shot.Height = 94;
        lines.Clear();
        renderer.DrawPlayerShots((start, end) => lines.Add((start, end)));

        Assert.Equal(8, lines.Count);
        float farRadius = AssertLineLength(lines, expectedRadius: ExpectedRadius(game, shot));
        Assert.True(farRadius < nearRadius);
    }

    private static float ExpectedRadius(TempestGame game, Shot shot)
    {
        float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
            game.CurrentLevel.Configuration.Position;
        return VectorRenderer.PlayerShotBaseSize /
            ((projectedDepth + Projection.ZOffset) / Projection.K);
    }

    private static float AssertLineLength(
        IReadOnlyList<(Vector2 Start, Vector2 End)> lines,
        float expectedRadius)
    {
        float expectedLength = expectedRadius * 2;
        float actualRadius = Vector2.Distance(lines[0].Start, lines[0].End) / 2;
        foreach ((Vector2 start, Vector2 end) in lines)
        {
            Assert.True(float.IsFinite(start.X) && float.IsFinite(start.Y));
            Assert.True(float.IsFinite(end.X) && float.IsFinite(end.Y));
            Assert.Equal(expectedLength, Vector2.Distance(start, end), precision: 3);
        }

        return actualRadius;
    }
}
