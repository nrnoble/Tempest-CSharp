using Tempest.Game.Levels;

namespace Tempest.Game.Debug;

public sealed record GameplayStartupConfiguration
{
	public const float MinimumLevel7SpikerSpeedOverride = 1.0f;
	public const float MaximumLevel7SpikerSpeedOverride = 3.0f;

	public int StartingDisplayedLevel { get; }
	public int StartingLives { get; }
	public double RimSpeedMultiplier { get; }
	public int RandomSeed { get; }
	public float? Level7SpikerSpeedOverride { get; }
	// Legacy version-2 recording metadata; true now matches normal Level 7 behavior.
	public bool Level7SpikerMatchesTankerSpeed { get; }

	public GameplayStartupConfiguration(
		int startingDisplayedLevel, int startingLives, double rimSpeedMultiplier, int randomSeed,
		float? level7SpikerSpeedOverride = null,
		bool level7SpikerMatchesTankerSpeed = false)
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
		if (level7SpikerSpeedOverride is float fixedSpikerSpeed &&
			(!float.IsFinite(fixedSpikerSpeed) ||
			 fixedSpikerSpeed < MinimumLevel7SpikerSpeedOverride ||
			 fixedSpikerSpeed > MaximumLevel7SpikerSpeedOverride))
			throw new ArgumentOutOfRangeException(nameof(level7SpikerSpeedOverride),
				$"The Level 7 Spiker speed override must be between " +
				$"{MinimumLevel7SpikerSpeedOverride:0.000} and {MaximumLevel7SpikerSpeedOverride:0.000}.");
		if (level7SpikerSpeedOverride is not null && level7SpikerMatchesTankerSpeed)
			throw new ArgumentException(
				"The Level 7 Spiker numeric override and Tanker-speed match are mutually exclusive.",
				nameof(level7SpikerMatchesTankerSpeed));

		StartingDisplayedLevel = startingDisplayedLevel;
		StartingLives = startingLives;
		RimSpeedMultiplier = rimSpeedMultiplier;
		RandomSeed = randomSeed;
		Level7SpikerSpeedOverride = level7SpikerSpeedOverride;
		Level7SpikerMatchesTankerSpeed = level7SpikerMatchesTankerSpeed;
	}

	public LevelConfiguration ResolveStartingConfiguration() =>
		LevelProgression.GetProductionLevel(StartingDisplayedLevel) ??
		throw new InvalidOperationException(
			$"Production Level {StartingDisplayedLevel} is no longer available.");
}
