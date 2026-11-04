using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tempest.Game.Levels;
using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public static class GameplayRecordingFile
{
	public const string FormatIdentifier = "tempest-gameplay-input";
	public const int CurrentFormatVersion = 1;
	public const string Level1Scenario = "level1";

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
		if (!ReferenceEquals(recording.StartingConfiguration, LevelConfiguration.Level1))
			throw new ArgumentException(
				"Gameplay recording files currently support a new Level 1 run only.",
				nameof(recording));

		string fullPath = PrepareOutputPath(path);
		RecordingDocument document = new()
		{
			Format = FormatIdentifier,
			Version = CurrentFormatVersion,
			RandomSeed = recording.RandomSeed,
			StartingScenario = Level1Scenario,
			DurationTicks = recording.DurationTicks,
			Frames = recording.Frames.Select(frame => new RecordedFrame
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
			}).ToArray()
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
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);
		string json = File.ReadAllText(path, Encoding.UTF8);
		RecordingDocument document = JsonSerializer.Deserialize<RecordingDocument>(
			json, SerializerOptions) ??
			throw new JsonException("The gameplay recording file is empty.");

		if (document.Format != FormatIdentifier)
			throw new InvalidDataException(
				$"Unsupported gameplay recording format '{document.Format}'.");
		if (document.Version != CurrentFormatVersion)
			throw new InvalidDataException(
				$"Unsupported gameplay recording version {document.Version}; supported version is {CurrentFormatVersion}.");
		if (document.RandomSeed < 0)
			throw new InvalidDataException("The gameplay recording random seed must be non-negative.");
		if (document.StartingScenario != Level1Scenario)
			throw new InvalidDataException(
				$"Unsupported gameplay recording starting scenario '{document.StartingScenario}'.");
		if (document.DurationTicks < 0)
			throw new InvalidDataException("The gameplay recording duration cannot be negative.");
		if (document.Frames is null || document.Frames.Length != document.DurationTicks)
			throw new InvalidDataException(
				"The gameplay recording duration must equal its exact input frame count.");

		PlayerInputFrame[] frames = new PlayerInputFrame[document.Frames.Length];
		for (int index = 0; index < document.Frames.Length; index++)
		{
			RecordedFrame? frame = document.Frames[index];
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

		return new PlayerInputReplay(document.RandomSeed, document.DurationTicks, frames,
			LevelConfiguration.Level1);
	}

	private sealed class RecordingDocument
	{
		public required string Format { get; init; }
		public required int Version { get; init; }
		public required int RandomSeed { get; init; }
		public required string StartingScenario { get; init; }
		public required int DurationTicks { get; init; }
		public required RecordedFrame?[] Frames { get; init; }
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
