using System.Numerics;
namespace Tempest.Game.Rendering;

/// <summary>
/// Perspective projection shared by all rendering. Larger z shrinks x/y by K/z. ZOffset keeps z
/// positive at the near rim; K and ZOffset are animated by the exit/reverse zoom effects.
/// Global mutable state, so zoom code must restore it when finished.
/// </summary>
public static class Projection
{
    public const float Width = 800;
    public const float Height = 600;
    public static float K { get; set; } = 65;
    public static float ZOffset { get; set; } = 5;

    public static Vector2 Project(float x, float y, float z, float centerY, float yOffset)
    {
        z += ZOffset;
        return new Vector2(Width / 2f + x / (z / K), centerY + (y + yOffset) / (z / K));
    }
}
