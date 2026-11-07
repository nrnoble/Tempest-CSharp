using System.Globalization;
using System.Security;
using System.Text.Json;

namespace Tempest.Game.Debug;

public sealed record HeadlessGameplayPlaybackDiagnostic(
	int? RequestedBoundary,
	int AchievedBoundary,
	int RecordingDurationTicks,
	int RemainingFrames,
	string CompletionReason,
	HeadlessGameplayStateDiagnostic StateSummary);

public sealed record HeadlessGameplayStateDiagnostic(
	int Boundary,
	int NextRecordingFrame,
	int RemainingRecordedFrames,
	bool IsRecordingComplete,
	int StartingDisplayedLevel,
	int StartingLives,
	double StartingRimSpeedMultiplier,
	int RandomSeed,
	int CurrentDisplayedLevel,
	string LevelState,
	int Score,
	int RemainingLives,
	int PlayerLane,
	float PlayerLanePosition,
	float PlayerDepth,
	int SuperZapperChargesRemaining,
	int PlayerShotCount,
	int EnemyBulletCount,
	int ActiveEggCount,
	int ActiveFlipperCount,
	int ActiveTankerCount,
	int ActiveSpikerCount,
	int ActiveSpikeCount,
	int ActiveFuseballCount,
	int ActivePulsarCount,
	IReadOnlyList<HeadlessFlipperDiagnostic> Flippers);

public sealed record HeadlessFlipperDiagnostic(
	int OriginalSlotId,
	int CargoUnitId,
	bool IsTankerCargo,
	int Lane,
	float Height,
	string MovementState,
	int Direction,
	int? RimPursuitDirection,
	int PivotSourceLane,
	int PivotDestinationLane,
	float PivotAngleDegrees,
	float PivotTotalAngleDegrees,
	float PivotProgress);

/// <summary>
/// Development-only command wrapper for deterministic playback without the Raylib game loop.
/// </summary>
public static class HeadlessGameplayPlaybackCli
{
	public const int SuccessExitCode = 0;
	public const int InvalidCommandExitCode = 2;
	public const int InvalidRecordingExitCode = 3;
	public const int SimulationFailureExitCode = 4;
	public const int OutOfRangeBoundaryExitCode = 5;

	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	public static bool IsRequested(IReadOnlyList<string> arguments) =>
		arguments.Contains("--headless-playback", StringComparer.Ordinal) ||
		arguments.Contains("--boundary", StringComparer.Ordinal) ||
		arguments.Contains("--json", StringComparer.Ordinal);

	public static int Run(
		IReadOnlyList<string> arguments,
		TextWriter standardOutput,
		TextWriter standardError,
		ApplicationPaths? applicationPaths = null)
	{
		ArgumentNullException.ThrowIfNull(arguments);
		ArgumentNullException.ThrowIfNull(standardOutput);
		ArgumentNullException.ThrowIfNull(standardError);

		if (!TryParse(arguments, out string? recordingPath, out int? requestedBoundary,
			out bool writeJson, out string? parseError))
		{
			standardError.WriteLine($"INVALID COMMAND ({InvalidCommandExitCode}): {parseError}");
			return InvalidCommandExitCode;
		}

		GameplayRecording recording;
		try
		{
			string resolvedPath = (applicationPaths ?? ApplicationPaths.Current)
				.ResolveGameplayRecordingPath(recordingPath!);
			recording = GameplayRecordingFile.LoadRecording(resolvedPath);
		}
		catch (Exception exception) when (IsRecordingLoadFailure(exception))
		{
			standardError.WriteLine(
				$"INVALID RECORDING ({InvalidRecordingExitCode}): {exception}");
			return InvalidRecordingExitCode;
		}

		if (requestedBoundary is int boundary &&
			(boundary < 0 || boundary > recording.Replay.DurationTicks))
		{
			standardError.WriteLine(
				$"OUT-OF-RANGE BOUNDARY ({OutOfRangeBoundaryExitCode}): Requested boundary {boundary} is outside 0 through {recording.Replay.DurationTicks}.");
			return OutOfRangeBoundaryExitCode;
		}

		GameplayPlaybackRunResult result;
		try
		{
			HeadlessGameplayPlaybackRunner runner = new(recording);
			result = requestedBoundary is int target
				? runner.RunToBoundary(target)
				: runner.RunToCompletion();
		}
		catch (Exception exception)
		{
			standardError.WriteLine(
				$"SIMULATION FAILURE ({SimulationFailureExitCode}):{Environment.NewLine}{exception}");
			return SimulationFailureExitCode;
		}

		if (writeJson)
			standardOutput.WriteLine(JsonSerializer.Serialize(ToDiagnostic(result), JsonOptions));
		else
			WriteHumanReadable(result, standardOutput);

		return SuccessExitCode;
	}

	private static bool TryParse(
		IReadOnlyList<string> arguments,
		out string? recordingPath,
		out int? requestedBoundary,
		out bool writeJson,
		out string? error)
	{
		recordingPath = null;
		requestedBoundary = null;
		writeJson = false;
		error = null;

		int playbackIndex = -1;
		for (int index = 0; index < arguments.Count; index++)
		{
			if (arguments[index] != "--headless-playback")
				continue;
			if (playbackIndex >= 0)
			{
				error = "--headless-playback may be specified only once.";
				return false;
			}
			playbackIndex = index;
		}

		if (playbackIndex != 0)
		{
			error = "--headless-playback <file> must be the first command-line option.";
			return false;
		}
		if (arguments.Count < 2 || string.IsNullOrWhiteSpace(arguments[1]) ||
			arguments[1].StartsWith("--", StringComparison.Ordinal))
		{
			error = "--headless-playback requires an explicit recording file.";
			return false;
		}

		recordingPath = arguments[1];
		bool boundarySeen = false;
		for (int index = 2; index < arguments.Count; index++)
		{
			string option = arguments[index];
			if (option == "--json")
			{
				if (writeJson)
				{
					error = "--json may be specified only once.";
					return false;
				}
				writeJson = true;
				continue;
			}

			if (option != "--boundary")
			{
				error = $"Unsupported or incompatible headless playback option '{option}'. Only --boundary and --json may follow the recording path.";
				return false;
			}
			if (boundarySeen)
			{
				error = "--boundary may be specified only once.";
				return false;
			}
			if (index + 1 >= arguments.Count ||
				arguments[index + 1].StartsWith("--", StringComparison.Ordinal))
			{
				error = "--boundary requires an integer value.";
				return false;
			}
			if (!int.TryParse(arguments[++index], NumberStyles.Integer,
				CultureInfo.InvariantCulture, out int parsedBoundary))
			{
				error = "--boundary requires an integer value.";
				return false;
			}

			requestedBoundary = parsedBoundary;
			boundarySeen = true;
		}

		return true;
	}

	private static bool IsRecordingLoadFailure(Exception exception) =>
		exception is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException or
			NotSupportedException or JsonException or SecurityException;

	private static HeadlessGameplayPlaybackDiagnostic ToDiagnostic(
		GameplayPlaybackRunResult result)
	{
		GameplayStateSummary state = result.StateSummary;
		HeadlessGameplayStateDiagnostic stateDiagnostic = new(
			state.Boundary,
			state.NextRecordingFrame,
			state.RemainingRecordedFrames,
			state.IsRecordingComplete,
			state.StartingDisplayedLevel,
			state.StartingLives,
			state.StartingRimSpeedMultiplier,
			state.RandomSeed,
			state.CurrentDisplayedLevel,
			state.LevelState.ToString(),
			state.Score,
			state.RemainingLives,
			state.PlayerLane,
			state.PlayerLanePosition,
			state.PlayerDepth,
			state.SuperZapperChargesRemaining,
			state.PlayerShotCount,
			state.EnemyBulletCount,
			state.ActiveEggCount,
			state.ActiveFlipperCount,
			state.ActiveTankerCount,
			state.ActiveSpikerCount,
			state.ActiveSpikeCount,
			state.ActiveFuseballCount,
			state.ActivePulsarCount,
			Array.AsReadOnly(state.Flippers.Select(ToDiagnostic).ToArray()));

		return new HeadlessGameplayPlaybackDiagnostic(
			result.RequestedBoundary,
			result.AchievedBoundary,
			result.RecordingDurationTicks,
			result.RemainingFrames,
			result.CompletionReason.ToString(),
			stateDiagnostic);
	}

	private static HeadlessFlipperDiagnostic ToDiagnostic(GameplayFlipperSummary flipper) =>
		new(
			flipper.OriginalSlotId,
			flipper.CargoUnitId,
			flipper.IsTankerCargo,
			flipper.Lane,
			flipper.Height,
			flipper.MovementState.ToString(),
			flipper.Direction,
			flipper.RimPursuitDirection,
			flipper.PivotSourceLane,
			flipper.PivotDestinationLane,
			flipper.PivotAngleDegrees,
			flipper.PivotTotalAngleDegrees,
			flipper.PivotProgress);

	private static void WriteHumanReadable(
		GameplayPlaybackRunResult result,
		TextWriter output)
	{
		GameplayStateSummary state = result.StateSummary;
		string requestedBoundary = result.RequestedBoundary?.ToString(
			CultureInfo.InvariantCulture) ?? "recording completion";
		output.WriteLine(
			$"Requested: {requestedBoundary}; achieved boundary: {result.AchievedBoundary}/{result.RecordingDurationTicks}; remaining frames: {result.RemainingFrames}; completion: {result.CompletionReason}.");
		output.WriteLine(
			FormattableString.Invariant(
				$"Level {state.CurrentDisplayedLevel} ({state.LevelState}); score {state.Score}; lives {state.RemainingLives}; player lane {state.PlayerLane} + {state.PlayerLanePosition:0.###}, depth {state.PlayerDepth:0.###}; Super Zapper charges {state.SuperZapperChargesRemaining}."));
		output.WriteLine(
			$"Entities: shots {state.PlayerShotCount}, enemy bullets {state.EnemyBulletCount}, Eggs {state.ActiveEggCount}, Flippers {state.ActiveFlipperCount}, Tankers {state.ActiveTankerCount}, Spikers {state.ActiveSpikerCount}, Spikes {state.ActiveSpikeCount}, Fuseballs {state.ActiveFuseballCount}, Pulsars {state.ActivePulsarCount}.");
		foreach (GameplayFlipperSummary flipper in state.Flippers)
		{
			output.WriteLine(FormattableString.Invariant(
				$"Flipper slot {flipper.OriginalSlotId}, cargo {flipper.CargoUnitId}, lane {flipper.Lane}, height {flipper.Height:0.###}, state {flipper.MovementState}, direction {flipper.Direction}, pivot {flipper.PivotSourceLane}->{flipper.PivotDestinationLane} ({flipper.PivotProgress:0.###})."));
		}
	}
}
