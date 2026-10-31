namespace Tempest.Game.Debug;
using System.Globalization;
using Tempest.Game.Gameplay;

/// <summary>
/// Parses the development/testing command-line switches. These are controlled engineering tools
/// for exercising one mechanic in isolation; they are not the finished arcade level-selection or
/// progression system. No flags = normal Level 1. --spiker-test, --tanker-test,
/// --fuseball-test and --pulsar-test replace the normal wave with a development wave and are mutually exclusive;
/// --enemy-bullets-test (opt-in Flipper firing) and --lives (starting-life override) compose with
/// any scenario or with normal play.
/// </summary>
public sealed class DebugController
{
	public static bool UseSpikerWave(IEnumerable<string> arguments) =>
		arguments.Contains("--spiker-test", StringComparer.Ordinal);

	public static bool UseEnemyBulletsTest(IEnumerable<string> arguments) =>
		arguments.Contains("--enemy-bullets-test", StringComparer.Ordinal);

	public static bool UseTankerTest(IEnumerable<string> arguments) =>
		arguments.Contains("--tanker-test", StringComparer.Ordinal);

	public static bool UseFuseballTest(IEnumerable<string> arguments) =>
		arguments.Contains("--fuseball-test", StringComparer.Ordinal);

	public static bool UsePulsarTest(IEnumerable<string> arguments) =>
		arguments.Contains("--pulsar-test", StringComparer.Ordinal);

	public static bool HasConflictingScenarios(IEnumerable<string> arguments) =>
		(UseTankerTest(arguments) ? 1 : 0) + (UseSpikerWave(arguments) ? 1 : 0) +
		(UseFuseballTest(arguments) ? 1 : 0) + (UsePulsarTest(arguments) ? 1 : 0) > 1;

	// --lives <n> overrides the arcade-default starting lives for long manual test sessions.
	// Any missing, non-numeric or non-positive value silently falls back to the default so a
	// mistyped development flag can never produce an unplayable configuration.
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
