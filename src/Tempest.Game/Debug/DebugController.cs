namespace Tempest.Game.Debug;
using System.Globalization;
using Tempest.Game.Gameplay;

/// <summary>
/// Parses the development/testing command-line switches. These are controlled engineering tools
/// for exercising one mechanic in isolation; they are not the finished arcade level-selection or
/// progression system. No flags = normal Level 1. --level2-test selects an isolated square Level 2
/// square/canflip run; --level3-geometry-test selects a non-production plus-board geometry
/// diagnostic. Both are terminal and do not enable progression. Scenario flags are mutually
/// exclusive; --enemy-bullets-test (opt-in Flipper firing) and --lives (starting-life override)
/// compose with any scenario or normal play. --gameplay-playback selects a seeded Level 1 script;
/// --gameplay-playback-level4 selects the same script on the production Level 4 configuration.
/// --level3-test directly selects the production-shaped
/// mixed Level 3 wave and remains terminal; --level3-geometry-test remains Flipper-only.
/// Both playback commands return to live input when their short seeded sequence ends.
/// --input-diagnostic instead opens a standalone development-only input window with no gameplay.
/// </summary>
public sealed class DebugController
{
	public static bool UseInputDiagnostic(IEnumerable<string> arguments) =>
		arguments.Contains("--input-diagnostic", StringComparer.Ordinal);

	public static bool UseGameplayPlayback(IEnumerable<string> arguments) =>
		arguments.Contains("--gameplay-playback", StringComparer.Ordinal);

	public static bool UseLevel4GameplayPlayback(IEnumerable<string> arguments) =>
		arguments.Contains("--gameplay-playback-level4", StringComparer.Ordinal);

	public static bool UseSpikerWave(IEnumerable<string> arguments) =>
		arguments.Contains("--spiker-test", StringComparer.Ordinal);

	// DEVELOPMENT ONLY: this fixed launch configuration is terminal and never enables progression.
	public static bool UseLevel2Test(IEnumerable<string> arguments) =>
		arguments.Contains("--level2-test", StringComparer.Ordinal);

	// DEVELOPMENT ONLY: direct access to the production-shaped mixed Level 3 wave.
	public static bool UseLevel3Test(IEnumerable<string> arguments) =>
		arguments.Contains("--level3-test", StringComparer.Ordinal);

	// DEVELOPMENT ONLY: direct access to the isolated Level 4 production candidate.
	public static bool UseLevel4Test(IEnumerable<string> arguments) =>
		arguments.Contains("--level4-test", StringComparer.Ordinal);

	// DEVELOPMENT ONLY: validates the source-derived plus board, not a production mixed wave.
	public static bool UseLevel3GeometryTest(IEnumerable<string> arguments) =>
		arguments.Contains("--level3-geometry-test", StringComparer.Ordinal);

	public static bool UseEnemyBulletsTest(IEnumerable<string> arguments) =>
		arguments.Contains("--enemy-bullets-test", StringComparer.Ordinal);

	public static bool UseTankerTest(IEnumerable<string> arguments) =>
		arguments.Contains("--tanker-test", StringComparer.Ordinal);

	public static bool UseFuseballTest(IEnumerable<string> arguments) =>
		arguments.Contains("--fuseball-test", StringComparer.Ordinal);

	public static bool UsePulsarTest(IEnumerable<string> arguments) =>
		arguments.Contains("--pulsar-test", StringComparer.Ordinal);

	public static bool HasConflictingScenarios(IEnumerable<string> arguments) =>
		(UseGameplayPlayback(arguments) ? 1 : 0) +
		(UseLevel4GameplayPlayback(arguments) ? 1 : 0) +
		(UseLevel2Test(arguments) ? 1 : 0) + (UseLevel3Test(arguments) ? 1 : 0) +
		(UseLevel4Test(arguments) ? 1 : 0) +
		(UseLevel3GeometryTest(arguments) ? 1 : 0) +
		(UseTankerTest(arguments) ? 1 : 0) +
		(UseSpikerWave(arguments) ? 1 : 0) + (UseFuseballTest(arguments) ? 1 : 0) +
		(UsePulsarTest(arguments) ? 1 : 0) > 1;

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
