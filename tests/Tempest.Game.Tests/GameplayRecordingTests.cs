using System.Text.Json;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class GameplayRecordingTests
{
	[Fact]
	public void RecorderCapturesEveryEffectiveTickInOrder()
	{
		PlayerInputRecorder recorder = new(4225, LevelConfiguration.Level1);
		PlayerInput[] inputs =
		[
			new(true, 1.25f, true, false, ZapPressed: true, ControllerAxis: -0.75f),
			new(false, -2.5f, false, true, ControllerAxis: 0.5f),
			default
		];

		for (int tick = 0; tick < inputs.Length; tick++)
			recorder.CaptureTick(tick, inputs[tick], restartPressed: tick == 1);

		Assert.Equal(inputs.Length, recorder.DurationTicks);
		Assert.Equal(inputs.Select((input, tick) =>
			new PlayerInputFrame(tick, input, RestartPressed: tick == 1)), recorder.Frames);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			recorder.CaptureTick(4, default));
		Assert.Throws<ArgumentException>(() =>
			recorder.CaptureTick(3, new PlayerInput(false, float.NaN, false, false)));
	}

	[Fact]
	public void RecordingFileRoundTripsFormatMetadataAndEveryInputField()
	{
		PlayerInputRecorder recorder = new(9127, LevelConfiguration.Level1);
		PlayerInput[] inputs =
		[
			new(true, 1.25f, true, false, ZapPressed: true, ControllerAxis: -0.75f),
			new(false, -2.5f, false, true, ControllerAxis: 0.5f),
			default
		];
		for (int tick = 0; tick < inputs.Length; tick++)
			recorder.CaptureTick(tick, inputs[tick], restartPressed: tick == 1);

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "roundtrip.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
			JsonElement root = document.RootElement;

			Assert.Equal(GameplayRecordingFile.FormatIdentifier,
				root.GetProperty("Format").GetString());
			Assert.Equal(GameplayRecordingFile.CurrentFormatVersion,
				root.GetProperty("Version").GetInt32());
			Assert.Equal(recorder.RandomSeed, root.GetProperty("RandomSeed").GetInt32());
			Assert.Equal(GameplayRecordingFile.Level1Scenario,
				root.GetProperty("StartingScenario").GetString());
			Assert.Equal(inputs.Length, root.GetProperty("DurationTicks").GetInt32());
			Assert.Equal(inputs.Length, root.GetProperty("Frames").GetArrayLength());

			PlayerInputReplay replay = GameplayRecordingFile.Load(path);
			Assert.Equal(recorder.RandomSeed, replay.RandomSeed);
			Assert.Equal(recorder.DurationTicks, replay.DurationTicks);
			Assert.Same(LevelConfiguration.Level1, replay.StartingConfiguration);
			for (int tick = 0; tick < inputs.Length; tick++)
				Assert.Equal(new PlayerInputFrame(tick, inputs[tick], tick == 1),
					replay.GetFrame(tick));
			Assert.False(replay.IsActiveAt(inputs.Length));
			Assert.Throws<ArgumentOutOfRangeException>(() => replay.GetFrame(inputs.Length));
			Assert.Throws<IOException>(() => GameplayRecordingFile.SaveNew(path, recorder));
		});
	}

	[Fact]
	public void MalformedRecordingJsonFailsClearly()
	{
		WithRecording("{", exception =>
			Assert.IsType<JsonException>(exception));
	}

	[Fact]
	public void MissingPlaybackFileFailsClearly()
	{
		WithTemporaryDirectory(directory =>
			Assert.Throws<FileNotFoundException>(() =>
				GameplayRecordingFile.Load(Path.Combine(directory, "missing.json"))));
	}

	[Fact]
	public void UnsupportedFormatVersionAndScenarioFailClearly()
	{
		WithRecording(CreateJson(format: "other-format"), exception =>
			Assert.IsType<InvalidDataException>(exception));
		WithRecording(CreateJson(version: GameplayRecordingFile.CurrentFormatVersion + 1),
			exception => Assert.IsType<InvalidDataException>(exception));
		WithRecording(CreateJson(scenario: "level4"), exception =>
			Assert.IsType<InvalidDataException>(exception));
	}

	[Fact]
	public void InconsistentDurationAndTickMetadataFailClearly()
	{
		WithRecording(CreateJson(durationTicks: 2), exception =>
			Assert.IsType<InvalidDataException>(exception));
		WithRecording(CreateJson(simulationTick: 1), exception =>
			Assert.IsType<InvalidDataException>(exception));
		WithRecording("""{"Format":"tempest-gameplay-input"}""", exception =>
			Assert.IsType<JsonException>(exception));
	}

	[Fact]
	public void SerializedRecordingReplaysTheCapturedSequenceFromTheSameSeed()
	{
		const int randomSeed = 1981;
		PlayerInputRecorder recorder = new(randomSeed, LevelConfiguration.Level1);
		TempestGame recordedGame = new(random: new Random(randomSeed));

		for (int tick = 0; tick < 180; tick++)
		{
			PlayerInput input = tick switch
			{
				< 30 => new PlayerInput(false, 0.5f, false, true, ControllerAxis: 0.25f),
				< 90 => new PlayerInput(true, -0.25f, false, false),
				90 => new PlayerInput(false, 0, false, false, ZapPressed: true),
				< 140 => new PlayerInput(true, 0, true, false, ControllerAxis: -0.5f),
				_ => default
			};
			recordedGame.RunOneStep(input);
			recorder.CaptureTick(tick, input);
		}

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "headless-roundtrip.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			PlayerInputReplay replay = GameplayRecordingFile.Load(path);
			TempestGame playbackGame = new(random: new Random(replay.RandomSeed));

			for (int tick = 0; tick < replay.DurationTicks; tick++)
			{
				PlayerInputFrame frame = replay.GetFrame(tick);
				playbackGame.RunOneStep(frame.Input, frame.RestartPressed);
			}

			AssertEquivalentGameplayState(recordedGame, playbackGame);
		});
	}

	private static string CreateJson(
		string format = GameplayRecordingFile.FormatIdentifier,
		int version = GameplayRecordingFile.CurrentFormatVersion,
		string scenario = GameplayRecordingFile.Level1Scenario,
		int durationTicks = 1,
		int simulationTick = 0) =>
		JsonSerializer.Serialize(new
		{
			Format = format,
			Version = version,
			RandomSeed = 1981,
			StartingScenario = scenario,
			DurationTicks = durationTicks,
			Frames = new[]
			{
				new
				{
					SimulationTick = simulationTick,
					RestartPressed = false,
					Input = new
					{
						Fire = false,
						MouseDeltaX = 0f,
						Left = false,
						Right = false,
						ZapPressed = false,
						ControllerAxis = 0f
					}
				}
			}
		});

	private static void WithRecording(string json, Action<Exception> assertException)
	{
		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "invalid.json");
			File.WriteAllText(path, json);
			Exception exception = Assert.ThrowsAny<Exception>(() =>
				GameplayRecordingFile.Load(path));
			assertException(exception);
		});
	}

	private static void WithTemporaryDirectory(Action<string> action)
	{
		string directory = Path.Combine(Path.GetTempPath(), $"tempest-recording-tests-{Guid.NewGuid():N}");
		Directory.CreateDirectory(directory);
		try
		{
			action(directory);
		}
		finally
		{
			Directory.Delete(directory, recursive: true);
		}
	}

	private static void AssertEquivalentGameplayState(TempestGame first, TempestGame second)
	{
		Assert.Same(first.CurrentLevel.Configuration, second.CurrentLevel.Configuration);
		Assert.Equal(first.State, second.State);
		Assert.Equal(first.Player.EdgeIndex, second.Player.EdgeIndex);
		Assert.Equal(first.Player.EdgePosition, second.Player.EdgePosition);
		Assert.Equal(first.Player.Depth, second.Player.Depth);
		Assert.Equal(first.Player.Lives, second.Player.Lives);
		Assert.Equal(first.DeathTicks, second.DeathTicks);
		Assert.Equal(first.Score.Score, second.Score.Score);
		Assert.Equal(first.SuperZapperChargesRemaining, second.SuperZapperChargesRemaining);
		Assert.Equal(first.CurrentLevel.EnemiesSpawned, second.CurrentLevel.EnemiesSpawned);
		Assert.Equal(first.CurrentLevel.EnemiesDestroyed, second.CurrentLevel.EnemiesDestroyed);
		Assert.Equal(first.Eggs.Select(egg =>
				(egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)),
			second.Eggs.Select(egg =>
				(egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)));
		Assert.Equal(first.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
			second.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
		Assert.Equal(first.Flippers.Select(enemy =>
				(enemy.EdgeIndex, enemy.Height, enemy.State, enemy.PauseTicks)),
			second.Flippers.Select(enemy =>
				(enemy.EdgeIndex, enemy.Height, enemy.State, enemy.PauseTicks)));
	}
}
