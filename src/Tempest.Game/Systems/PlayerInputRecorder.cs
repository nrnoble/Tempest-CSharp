using Tempest.Game.Levels;

namespace Tempest.Game.Systems;

public sealed class PlayerInputRecorder
{
	private readonly List<PlayerInputFrame> frames = [];

	public int RandomSeed { get; }
	public LevelConfiguration StartingConfiguration { get; }
	public int DurationTicks => frames.Count;
	public IReadOnlyList<PlayerInputFrame> Frames => frames.AsReadOnly();

	public PlayerInputRecorder(int randomSeed, LevelConfiguration startingConfiguration)
	{
		ArgumentNullException.ThrowIfNull(startingConfiguration);
		if (randomSeed < 0)
			throw new ArgumentOutOfRangeException(nameof(randomSeed));
		if (!ReferenceEquals(startingConfiguration, LevelConfiguration.Level1))
			throw new ArgumentException(
				"Gameplay recording currently supports a new Level 1 run only.",
				nameof(startingConfiguration));

		RandomSeed = randomSeed;
		StartingConfiguration = startingConfiguration;
	}

	public void CaptureTick(int simulationTick, PlayerInput input, bool restartPressed = false)
	{
		if (simulationTick != frames.Count)
			throw new ArgumentOutOfRangeException(nameof(simulationTick),
				"Recorded inputs must be captured once, in simulation-tick order.");
		if (!float.IsFinite(input.MouseDeltaX) || !float.IsFinite(input.ControllerAxis))
			throw new ArgumentException("Player input values must be finite.", nameof(input));

		frames.Add(new PlayerInputFrame(simulationTick, input, restartPressed));
	}

	public PlayerInputReplay CreateReplay() =>
		new(RandomSeed, DurationTicks, frames, StartingConfiguration);
}
