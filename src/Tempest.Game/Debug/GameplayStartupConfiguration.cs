using Tempest.Game.Levels;

namespace Tempest.Game.Debug;

public sealed record GameplayStartupConfiguration
{
	public int StartingDisplayedLevel { get; }
	public int StartingLives { get; }
	public double RimSpeedMultiplier { get; }
	public int RandomSeed { get; }

	public GameplayStartupConfiguration(
		int startingDisplayedLevel, int startingLives, double rimSpeedMultiplier, int randomSeed)
	{
		if (LevelProgression.GetProductionLevel(startingDisplayedLevel) is null)
			throw new ArgumentOutOfRangeException(nameof(startingDisplayedLevel),
				"The starting level must be an implemented production level.");
		if (startingLives <= 0)
			throw new ArgumentOutOfRangeException(nameof(startingLives),
				"Starting lives must be positive.");
		if (!double.IsFinite(rimSpeedMultiplier) || rimSpeedMultiplier <= 0)
			throw new ArgumentOutOfRangeException(nameof(rimSpeedMultiplier),
				"The rim-speed multiplier must be positive and finite.");
		if (randomSeed < 0)
			throw new ArgumentOutOfRangeException(nameof(randomSeed),
				"The random seed must be non-negative.");

		StartingDisplayedLevel = startingDisplayedLevel;
		StartingLives = startingLives;
		RimSpeedMultiplier = rimSpeedMultiplier;
		RandomSeed = randomSeed;
	}

	public LevelConfiguration ResolveStartingConfiguration() =>
		LevelProgression.GetProductionLevel(StartingDisplayedLevel) ??
		throw new InvalidOperationException(
			$"Production Level {StartingDisplayedLevel} is no longer available.");
}
