namespace Tempest.Game.Debug;
using System.Globalization;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;

public enum GameplayFileMode
{
	Record,
	Playback
}

public sealed record GameplayFileCommand(
	GameplayFileMode Mode, string Path, int? StartingDisplayedLevel = null,
	int? StartingLives = null, double? RimSpeedMultiplier = null);

/// <summary>
/// Parses the development/testing command-line switches. These are controlled engineering tools
/// for exercising one mechanic in isolation; they are not the finished arcade level-selection or
/// progression system. No flags = normal Level 1. --level N starts a fresh production game at
/// that level and continues normal campaign progression. Legacy --levelN-test selectors remain
/// terminal diagnostics. Scenario flags are mutually exclusive; --enemy-bullets-test (opt-in
/// Flipper firing), --lives (starting-life override), --rim-speed and --game-over-density compose
/// with normal play. --gameplay-playback selects a seeded Level 1 profile;
/// --gameplay-playback-level4 selects a distinct profile on the production Level 4 configuration.
/// Both playback commands return to live input when their seeded profile ends.
/// --record-gameplay records production input with startup metadata; --playback-file restores that
/// metadata and rejects simulation overrides. The file transport pair precedes compatible options.
/// --input-diagnostic instead opens a standalone development-only input window with no gameplay.
/// </summary>
public sealed class DebugController
{
	public static bool TryParseStartingLevel(
		IReadOnlyList<string> arguments,
		out int? startingLevel,
		out string? error)
	{
		startingLevel = null;
		error = null;
		int optionIndex = -1;

		for (int index = 0; index < arguments.Count; index++)
		{
			if (arguments[index] != "--level")
				continue;
			if (optionIndex >= 0)
			{
				error = "--level may be specified only once.";
				return false;
			}
			optionIndex = index;
		}

		if (optionIndex < 0)
			return true;

		int highestLevel = LevelProgression.GetLastProductionLevel().DisplayedLevel;
		string requirement =
			$"--level requires an implemented production level from 1 through {highestLevel}.";
		if (optionIndex + 1 >= arguments.Count ||
			arguments[optionIndex + 1].StartsWith("--", StringComparison.Ordinal))
		{
			error = requirement;
			return false;
		}
		if (!int.TryParse(arguments[optionIndex + 1], NumberStyles.Integer,
			CultureInfo.InvariantCulture, out int parsed) ||
			LevelProgression.GetProductionLevel(parsed) is null)
		{
			error = requirement;
			return false;
		}

		startingLevel = parsed;
		return true;
	}

	public static bool TryParseGameOverDensityMultiplier(
		IReadOnlyList<string> arguments,
		out double multiplier,
		out string? error)
	{
		// This is Neal's accepted manual calibration for the current four-burst translation.
		multiplier = 2.0;
		error = null;
		int optionIndex = -1;

		for (int index = 0; index < arguments.Count; index++)
		{
			if (arguments[index] != "--game-over-density")
				continue;
			if (optionIndex >= 0)
			{
				error = "--game-over-density may be specified only once.";
				return false;
			}
			optionIndex = index;
		}

		if (optionIndex < 0)
			return true;
		if (optionIndex + 1 >= arguments.Count ||
			arguments[optionIndex + 1].StartsWith("--", StringComparison.Ordinal))
		{
			error = "--game-over-density requires a positive finite numeric value.";
			return false;
		}
		if (!double.TryParse(arguments[optionIndex + 1], NumberStyles.Float,
			CultureInfo.InvariantCulture, out double parsed) ||
			!double.IsFinite(parsed) || parsed <= 0)
		{
			error = "--game-over-density requires a positive finite numeric value.";
			return false;
		}

		multiplier = parsed;
		return true;
	}

	public static bool TryParseRimSpeedMultiplier(
		IReadOnlyList<string> arguments,
		out double multiplier,
		out string? error)
	{
		multiplier = 1.0;
		error = null;
		int optionIndex = -1;

		for (int index = 0; index < arguments.Count; index++)
		{
			if (arguments[index] != "--rim-speed")
				continue;
			if (optionIndex >= 0)
			{
				error = "--rim-speed may be specified only once.";
				return false;
			}
			optionIndex = index;
		}

		if (optionIndex < 0)
			return true;
		if (optionIndex + 1 >= arguments.Count ||
			arguments[optionIndex + 1].StartsWith("--", StringComparison.Ordinal))
		{
			error = "--rim-speed requires a positive finite numeric value.";
			return false;
		}
		if (!double.TryParse(arguments[optionIndex + 1], NumberStyles.Float,
			CultureInfo.InvariantCulture, out double parsed) ||
			!double.IsFinite(parsed) || parsed <= 0)
		{
			error = "--rim-speed requires a positive finite numeric value.";
			return false;
		}

		multiplier = parsed;
		return true;
	}

	public static bool TryParseGameplayFileCommand(
		IReadOnlyList<string> arguments,
		out GameplayFileCommand? command,
		out string? error)
	{
		command = null;
		error = null;
		int optionIndex = -1;
		GameplayFileMode mode = default;

		for (int index = 0; index < arguments.Count; index++)
		{
			GameplayFileMode? foundMode = arguments[index] switch
			{
				"--record-gameplay" => GameplayFileMode.Record,
				"--playback-file" => GameplayFileMode.Playback,
				_ => null
			};
			if (foundMode is null)
				continue;
			if (optionIndex >= 0)
			{
				error = "Recording and file playback are mutually exclusive and may be specified only once.";
				return false;
			}

			optionIndex = index;
			mode = foundMode.Value;
		}

		if (optionIndex < 0)
			return true;
		if (optionIndex != 0)
		{
			error = "The gameplay file mode and path must appear before compatible options.";
			return false;
		}
		if (arguments.Count < 2 || string.IsNullOrWhiteSpace(arguments[1]) ||
			arguments[1].StartsWith("--", StringComparison.Ordinal))
		{
			error = $"The {arguments[0]} option requires an explicit file path.";
			return false;
		}

		int? startingLevel = null;
		int? startingLives = null;
		double? rimSpeedMultiplier = null;
		bool densitySeen = false;
		for (int index = 2; index < arguments.Count; index++)
		{
			string option = arguments[index];
			if (IsGameplayScenarioOption(option))
			{
				error = $"Gameplay file modes cannot be combined with special scenario option '{option}'.";
				return false;
			}

			if (option is "--level" or "--lives" or "--rim-speed")
			{
				if (mode == GameplayFileMode.Playback)
				{
					error = $"--playback-file restores simulation startup values from the recording; '{option}' is not allowed.";
					return false;
				}
				if ((option == "--level" && startingLevel is not null) ||
					(option == "--lives" && startingLives is not null) ||
					(option == "--rim-speed" && rimSpeedMultiplier is not null))
				{
					error = $"{option} may be specified only once.";
					return false;
				}
				if (!TryReadOptionValue(arguments, ref index, option, out string? value, out error))
					return false;

				if (option == "--level")
				{
					int highestLevel = LevelProgression.GetLastProductionLevel().DisplayedLevel;
					if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture,
						out int level) || LevelProgression.GetProductionLevel(level) is null)
					{
						error = $"--level requires an implemented production level from 1 through {highestLevel}.";
						return false;
					}
					startingLevel = level;
				}
				else if (option == "--lives")
				{
					if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture,
						out int lives) || lives <= 0)
					{
						error = "--lives requires a positive integer value.";
						return false;
					}
					startingLives = lives;
				}
				else
				{
					if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture,
						out double multiplier) || !double.IsFinite(multiplier) || multiplier <= 0)
					{
						error = "--rim-speed requires a positive finite numeric value.";
						return false;
					}
					rimSpeedMultiplier = multiplier;
				}
				continue;
			}

			if (option == "--game-over-density")
			{
				if (densitySeen)
				{
					error = "--game-over-density may be specified only once.";
					return false;
				}
				if (!TryReadOptionValue(arguments, ref index, option, out string? density, out error))
					return false;
				if (!double.TryParse(density, NumberStyles.Float, CultureInfo.InvariantCulture,
					out double parsedDensity) || !double.IsFinite(parsedDensity) || parsedDensity <= 0)
				{
					error = "--game-over-density requires a positive finite numeric value.";
					return false;
				}
				densitySeen = true;
				continue;
			}

			error = $"Unknown gameplay file option or argument '{option}'.";
			return false;
		}

		command = new GameplayFileCommand(mode, arguments[1], startingLevel,
			startingLives, rimSpeedMultiplier);
		return true;
	}

	private static bool TryReadOptionValue(
		IReadOnlyList<string> arguments, ref int index, string option,
		out string? value, out string? error)
	{
		value = null;
		error = null;
		if (index + 1 >= arguments.Count ||
			arguments[index + 1].StartsWith("--", StringComparison.Ordinal))
		{
			error = $"{option} requires a value.";
			return false;
		}

		value = arguments[++index];
		return true;
	}

	private static bool IsGameplayScenarioOption(string option) =>
		option is "--level2-test" or "--level3-test" or "--level4-test" or
			"--level6-test" or "--level7-test" or "--level3-geometry-test" or
			"--spiker-test" or "--tanker-test" or "--fuseball-test" or
			"--pulsar-test" or "--enemy-bullets-test" or "--input-diagnostic" or
			"--gameplay-playback" or "--gameplay-playback-level4";

	public static bool UseInputDiagnostic(IEnumerable<string> arguments) =>
		arguments.Contains("--input-diagnostic", StringComparer.Ordinal);

	public static bool UseGameplayPlayback(IEnumerable<string> arguments) =>
		arguments.Contains("--gameplay-playback", StringComparer.Ordinal);

	public static bool UseDirectProductionLevelStart(IEnumerable<string> arguments) =>
		arguments.Contains("--level", StringComparer.Ordinal);

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

	// DEVELOPMENT ONLY: direct access to the production Level 6 wave and Flipper firing phase.
	public static bool UseLevel6Test(IEnumerable<string> arguments) =>
		arguments.Contains("--level6-test", StringComparer.Ordinal);

	// DEVELOPMENT ONLY: direct access to the production Level 7 wave and movement/firing policies.
	public static bool UseLevel7Test(IEnumerable<string> arguments) =>
		arguments.Contains("--level7-test", StringComparer.Ordinal);

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
		(UseDirectProductionLevelStart(arguments) ? 1 : 0) +
		(UseLevel2Test(arguments) ? 1 : 0) + (UseLevel3Test(arguments) ? 1 : 0) +
		(UseLevel4Test(arguments) ? 1 : 0) + (UseLevel6Test(arguments) ? 1 : 0) +
		(UseLevel7Test(arguments) ? 1 : 0) +
		(UseLevel3GeometryTest(arguments) ? 1 : 0) +
		(UseTankerTest(arguments) ? 1 : 0) +
		(UseSpikerWave(arguments) ? 1 : 0) + (UseFuseballTest(arguments) ? 1 : 0) +
		(UsePulsarTest(arguments) ? 1 : 0) > 1;

	// Ordinary non-file launches retain the legacy default fallback; file recording validates
	// --lives strictly so its effective startup value cannot be silently changed.
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
