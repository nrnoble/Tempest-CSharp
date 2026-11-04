using Tempest.Game.Levels;
using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public static class GameplayPlayback
{
	private const int DemoRandomSeed = 1981;
	private const int DemoDurationTicks = 240;

	public static PlayerInputReplay CreateDemo(
		LevelConfiguration? startingConfiguration = null)
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

		return new PlayerInputReplay(DemoRandomSeed, DemoDurationTicks, frames,
			startingConfiguration);
	}
}
