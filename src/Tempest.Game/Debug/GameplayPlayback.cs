using Tempest.Game.Levels;
using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public sealed record GameplayPlaybackProfile(
	LevelConfiguration StartingConfiguration,
	int RandomSeed,
	int DurationTicks,
	IReadOnlyList<PlayerInputFrame> Frames)
{
	public PlayerInputReplay CreateReplay() =>
		new(RandomSeed, DurationTicks, Frames, StartingConfiguration);
}

public static class GameplayPlayback
{
	private const int DemoRandomSeed = 1981;
	private const int DemoDurationTicks = 240;

	public static GameplayPlaybackProfile Level1Profile { get; } = new(
		LevelConfiguration.Level1, DemoRandomSeed, DemoDurationTicks,
		Array.AsReadOnly(CreateLevel1Frames()));

	public static GameplayPlaybackProfile Level4Profile { get; } = new(
		LevelConfiguration.Level4Candidate, DemoRandomSeed, DemoDurationTicks,
		Array.AsReadOnly(CreateLevel4Frames()));

	public static PlayerInputReplay CreateLevel1() => Level1Profile.CreateReplay();
	public static PlayerInputReplay CreateLevel4() => Level4Profile.CreateReplay();

	private static PlayerInputFrame[] CreateLevel1Frames()
	{
		List<PlayerInputFrame> frames = [];
		for (int tick = 0; tick < DemoDurationTicks; tick++)
		{
			PlayerInput playerInput = tick switch
			{
				< 30 => new PlayerInput(false, 0, false, true),
				< 90 => new PlayerInput(true, 0, false, false),
				90 => new PlayerInput(false, 0, false, false, ZapPressed: true),
				< 150 => new PlayerInput(true, 0, true, false),
				_ => default
			};

			if (playerInput != default)
				frames.Add(new PlayerInputFrame(tick, playerInput));
		}

		return frames.ToArray();
	}

	private static PlayerInputFrame[] CreateLevel4Frames()
	{
		List<PlayerInputFrame> frames = [];
		for (int tick = 0; tick < DemoDurationTicks; tick++)
		{
			PlayerInput playerInput = tick switch
			{
				< 60 => new PlayerInput(true, 0, false, false),
				< 100 => new PlayerInput(true, 0, false, true),
				100 => new PlayerInput(false, 0, false, false, ZapPressed: true),
				< 140 => new PlayerInput(true, 0, true, false),
				140 => new PlayerInput(false, 0, false, false, ZapPressed: true),
				< 180 => new PlayerInput(true, 0, false, true),
				_ => default
			};

			if (playerInput != default)
				frames.Add(new PlayerInputFrame(tick, playerInput));
		}

		return frames.ToArray();
	}
}
