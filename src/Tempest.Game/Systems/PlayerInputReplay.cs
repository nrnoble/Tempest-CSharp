namespace Tempest.Game.Systems;

public readonly record struct PlayerInputFrame(
	int SimulationTick, PlayerInput Input, bool RestartPressed = false);

public sealed class PlayerInputReplay
{
	private readonly Dictionary<int, PlayerInputFrame> frames;

	public int RandomSeed { get; }
	public int DurationTicks { get; }

	public PlayerInputReplay(
		int randomSeed, int durationTicks, IEnumerable<PlayerInputFrame> frames)
	{
		ArgumentNullException.ThrowIfNull(frames);
		if (randomSeed < 0)
			throw new ArgumentOutOfRangeException(nameof(randomSeed));
		if (durationTicks < 0)
			throw new ArgumentOutOfRangeException(nameof(durationTicks));

		Dictionary<int, PlayerInputFrame> indexedFrames = [];
		foreach (PlayerInputFrame frame in frames)
		{
			if (frame.SimulationTick < 0 || frame.SimulationTick >= durationTicks)
				throw new ArgumentOutOfRangeException(nameof(frames),
					"Every replay frame must be within the replay duration.");
			if (!indexedFrames.TryAdd(frame.SimulationTick, frame))
				throw new ArgumentException("Replay frames must have unique simulation ticks.",
					nameof(frames));
		}

		RandomSeed = randomSeed;
		DurationTicks = durationTicks;
		this.frames = indexedFrames;
	}

	public PlayerInputFrame GetFrame(int simulationTick)
	{
		if (simulationTick < 0 || simulationTick >= DurationTicks)
			throw new ArgumentOutOfRangeException(nameof(simulationTick));

		return frames.TryGetValue(simulationTick, out PlayerInputFrame frame)
			? frame
			: new PlayerInputFrame(simulationTick, default);
	}
}
