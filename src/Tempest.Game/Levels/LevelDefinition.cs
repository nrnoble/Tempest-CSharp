using System.Numerics;
namespace Tempest.Game.Levels;

public sealed record LevelDefinition(bool Continuous, float CenterY, float YOffset, IReadOnlyList<Vector2> Points, string Comment)
{
    public int VertexCount => Points.Count;

    // Open definitions retain their terminal vertex, but do not expose a closing lane.
    public int PlayableLaneCount => Continuous ? VertexCount : Math.Max(0, VertexCount - 1);
}
