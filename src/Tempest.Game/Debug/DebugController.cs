namespace Tempest.Game.Debug;
using System.Globalization;
using Tempest.Game.Gameplay;

public sealed class DebugController
{
	public static bool UseSpikerWave(IEnumerable<string> arguments) =>
		arguments.Contains("--spiker-test", StringComparer.Ordinal);

	public static int GetStartingLives(IReadOnlyList<string> arguments)
	{
		for (int index = 0; index < arguments.Count; index++)
		{
			if (arguments[index] != "--lives")
				continue;

			if (index + 1 < arguments.Count &&
				int.TryParse(arguments[index + 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int lives) &&
				lives > 0)
				return lives;

			return Player.DefaultStartingLives;
		}

		return Player.DefaultStartingLives;
	}
}
