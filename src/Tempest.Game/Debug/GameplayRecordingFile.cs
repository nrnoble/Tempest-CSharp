using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public sealed record GameplayRecording(
	GameplayStartupConfiguration StartupConfiguration, PlayerInputReplay Replay);

public static class GameplayRecordingFile
{
	public const string FormatIdentifier = "tempest-gameplay-input";
	public const int CurrentFormatVersion = 2;
	public const string Level1Scenario = "level1";
	public const string ProductionScenario = "production";

	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		WriteIndented = true,
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
	};

	public static string PrepareOutputPath(string path)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);
		string fullPath = Path.GetFullPath(path);
		if (File.Exists(fullPath) || Directory.Exists(fullPath))
			throw new IOException($"The gameplay recording destination already exists: {fullPath}");

		string? directory = Path.GetDirectoryName(fullPath);
		if (directory is null)
			throw new ArgumentException("The gameplay recording path must include a directory.", nameof(path));
		Directory.CreateDirectory(directory);
		if (File.Exists(fullPath) || Directory.Exists(fullPath))
			throw new IOException($"The gameplay recording destination already exists: {fullPath}");

		return fullPath;
	}

	public static void SaveNew(string path, PlayerInputRecorder recording)
	{
		ArgumentNullException.ThrowIfNull(recording);

		string fullPath = PrepareOutputPath(path);
		RecordingDocumentV2 document = new()
		{
			Format = FormatIdentifier,
			Version = CurrentFormatVersion,
			RandomSeed = recording.RandomSeed,
			StartingScenario = ProductionScenario,
			StartingDisplayedLevel = recording.StartupConfiguration.StartingDisplayedLevel,
			StartingLives = recording.StartupConfiguration.StartingLives,
			RimSpeedMultiplier = recording.StartupConfiguration.RimSpeedMultiplier,
			DurationTicks = recording.DurationTicks,
			Frames = ToRecordedFrames(recording.Frames)
		};
		byte[] contents = JsonSerializer.SerializeToUtf8Bytes(document, SerializerOptions);
		string temporaryPath = $"{fullPath}.{Guid.NewGuid():N}.tmp";
		bool temporaryFileCreated = false;
		try
		{
			using (FileStream stream = new(temporaryPath, FileMode.CreateNew,
				FileAccess.Write, FileShare.None))
			{
				temporaryFileCreated = true;
				stream.Write(contents);
				stream.Flush(flushToDisk: true);
			}

			File.Move(temporaryPath, fullPath);
			temporaryFileCreated = false;
		}
		catch
		{
			if (temporaryFileCreated)
				File.Delete(temporaryPath);
			throw;
		}
	}

	public static PlayerInputReplay Load(string path)
		=> LoadRecording(path).Replay;

	public static GameplayRecording LoadRecording(string path)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);
		string json = File.ReadAllText(path, Encoding.UTF8);
		using JsonDocument envelope = JsonDocument.Parse(json);
		JsonElement root = envelope.RootElement;
		if (root.ValueKind != JsonValueKind.Object ||
			!root.TryGetProperty("Version", out JsonElement versionElement) ||
			!versionElement.TryGetInt32(out int version))
			throw new InvalidDataException("The gameplay recording must declare an integer format version.");

		return version switch
		{
			1 => LoadVersion1(json),
			CurrentFormatVersion => LoadVersion2(json),
			_ => throw new InvalidDataException(
				$"Unsupported gameplay recording version {version}; supported versions are 1 and {CurrentFormatVersion}.")
		};
	}

	private static GameplayRecording LoadVersion1(string json)
	{
		RecordingDocumentV1 document = JsonSerializer.Deserialize<RecordingDocumentV1>(
			json, SerializerOptions) ??
			throw new JsonException("The gameplay recording file is empty.");
		ValidateCommonFields(document.Format, document.Version, document.RandomSeed,
			document.DurationTicks, document.Frames);
		if (document.StartingScenario != Level1Scenario)
			throw new InvalidDataException(
				$"Unsupported version-1 gameplay recording starting scenario '{document.StartingScenario}'.");

		GameplayStartupConfiguration startup = new(1, Player.DefaultStartingLives, 1.0,
			document.RandomSeed);
		PlayerInputFrame[] frames = ParseFrames(document.Frames, document.DurationTicks);
		return new GameplayRecording(startup, new PlayerInputReplay(document.RandomSeed,
			document.DurationTicks, frames, startup.ResolveStartingConfiguration()));
	}

	private static GameplayRecording LoadVersion2(string json)
	{
		RecordingDocumentV2 document = JsonSerializer.Deserialize<RecordingDocumentV2>(
			json, SerializerOptions) ??
			throw new JsonException("The gameplay recording file is empty.");
		ValidateCommonFields(document.Format, document.Version, document.RandomSeed,
			document.DurationTicks, document.Frames);
		if (document.StartingScenario != ProductionScenario)
			throw new InvalidDataException(
				$"Unsupported version-2 gameplay recording starting scenario '{document.StartingScenario}'.");

		GameplayStartupConfiguration startup;
		try
		{
			startup = new GameplayStartupConfiguration(document.StartingDisplayedLevel,
				document.StartingLives, document.RimSpeedMultiplier, document.RandomSeed);
		}
		catch (ArgumentOutOfRangeException exception)
		{
			throw new InvalidDataException(
				$"The version-2 gameplay recording has invalid startup metadata: {exception.Message}",
				exception);
		}

		PlayerInputFrame[] frames = ParseFrames(document.Frames, document.DurationTicks);
		return new GameplayRecording(startup, new PlayerInputReplay(document.RandomSeed,
			document.DurationTicks, frames, startup.ResolveStartingConfiguration()));
	}

	private static void ValidateCommonFields(
		string? format, int version, int randomSeed, int durationTicks,
		RecordedFrame?[]? frames)
	{
		if (format != FormatIdentifier)
			throw new InvalidDataException($"Unsupported gameplay recording format '{format}'.");
		if (randomSeed < 0)
			throw new InvalidDataException("The gameplay recording random seed must be non-negative.");
		if (durationTicks < 0)
			throw new InvalidDataException("The gameplay recording duration cannot be negative.");
		if (frames is null || frames.Length != durationTicks)
			throw new InvalidDataException(
				"The gameplay recording duration must equal its exact input frame count.");
		if (version is not 1 and not CurrentFormatVersion)
			throw new InvalidDataException($"Unsupported gameplay recording version {version}.");
	}

	private static PlayerInputFrame[] ParseFrames(
		RecordedFrame?[]? recordedFrames, int durationTicks)
	{
		if (recordedFrames is null || recordedFrames.Length != durationTicks)
			throw new InvalidDataException(
				"The gameplay recording duration must equal its exact input frame count.");

		PlayerInputFrame[] frames = new PlayerInputFrame[recordedFrames.Length];
		for (int index = 0; index < recordedFrames.Length; index++)
		{
			RecordedFrame? frame = recordedFrames[index];
			if (frame is null || frame.Input is null)
				throw new InvalidDataException($"Gameplay recording frame {index} is incomplete.");
			if (frame.SimulationTick != index)
				throw new InvalidDataException(
					$"Gameplay recording frames must be ordered and contiguous; expected tick {index}.");
			if (!float.IsFinite(frame.Input.MouseDeltaX) ||
				!float.IsFinite(frame.Input.ControllerAxis))
				throw new InvalidDataException(
					$"Gameplay recording frame {index} contains a non-finite input value.");

			frames[index] = new PlayerInputFrame(index,
				new PlayerInput(frame.Input.Fire, frame.Input.MouseDeltaX, frame.Input.Left,
					frame.Input.Right, frame.Input.ZapPressed, frame.Input.ControllerAxis),
				frame.RestartPressed);
		}

		return frames;
	}

	private static RecordedFrame[] ToRecordedFrames(
		IReadOnlyList<PlayerInputFrame> frames) =>
		frames.Select(frame => new RecordedFrame
		{
			SimulationTick = frame.SimulationTick,
			RestartPressed = frame.RestartPressed,
			Input = new RecordedInput
			{
				Fire = frame.Input.Fire,
				MouseDeltaX = frame.Input.MouseDeltaX,
				Left = frame.Input.Left,
				Right = frame.Input.Right,
				ZapPressed = frame.Input.ZapPressed,
				ControllerAxis = frame.Input.ControllerAxis
			}
		}).ToArray();

	private sealed class RecordingDocumentV1
	{
		public required string? Format { get; init; }
		public required int Version { get; init; }
		public required int RandomSeed { get; init; }
		public required string? StartingScenario { get; init; }
		public required int DurationTicks { get; init; }
		public required RecordedFrame?[]? Frames { get; init; }
	}

	private sealed class RecordingDocumentV2
	{
		public required string? Format { get; init; }
		public required int Version { get; init; }
		public required int RandomSeed { get; init; }
		public required string? StartingScenario { get; init; }
		public required int StartingDisplayedLevel { get; init; }
		public required int StartingLives { get; init; }
		public required double RimSpeedMultiplier { get; init; }
		public required int DurationTicks { get; init; }
		public required RecordedFrame?[]? Frames { get; init; }
	}

	private sealed class RecordedFrame
	{
		public required int SimulationTick { get; init; }
		public required bool RestartPressed { get; init; }
		public required RecordedInput? Input { get; init; }
	}

	private sealed class RecordedInput
	{
		public required bool Fire { get; init; }
		public required float MouseDeltaX { get; init; }
		public required bool Left { get; init; }
		public required bool Right { get; init; }
		public required bool ZapPressed { get; init; }
		public required float ControllerAxis { get; init; }
	}
}
