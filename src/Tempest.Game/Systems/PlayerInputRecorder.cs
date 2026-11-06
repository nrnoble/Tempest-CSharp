using Tempest.Game.Debug;
using Tempest.Game.Levels;

namespace Tempest.Game.Systems;

public sealed class PlayerInputRecorder
{
	private readonly List<PlayerInputFrame> frames = [];

	public GameplayStartupConfiguration StartupConfiguration { get; }
	public int RandomSeed => StartupConfiguration.RandomSeed;
	public LevelConfiguration StartingConfiguration { get; }
	public int DurationTicks => frames.Count;
	public IReadOnlyList<PlayerInputFrame> Frames => frames.AsReadOnly();

	public PlayerInputRecorder(GameplayStartupConfiguration startupConfiguration)
	{
		StartupConfiguration = startupConfiguration ??
			throw new ArgumentNullException(nameof(startupConfiguration));
		StartingConfiguration = startupConfiguration.ResolveStartingConfiguration();
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
