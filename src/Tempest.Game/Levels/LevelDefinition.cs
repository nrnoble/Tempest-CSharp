using System.Numerics;
namespace Tempest.Game.Levels;

public sealed record LevelDefinition(bool Continuous, float CenterY, float YOffset, IReadOnlyList<Vector2> Points, string Comment);
