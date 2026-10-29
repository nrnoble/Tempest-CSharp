namespace Tempest.Game.Levels;
public sealed record LevelSet(string Description, IReadOnlyList<LevelDefinition> Levels);
