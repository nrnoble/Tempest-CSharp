using System.Globalization;
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
		PlayerInputRecorder recorder = new(new GameplayStartupConfiguration(1, 3, 1.0, 4225));
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
		PlayerInputRecorder recorder = new(new GameplayStartupConfiguration(5, 10, 2.5, 9127));
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
			Assert.Equal(GameplayRecordingFile.ProductionScenario,
				root.GetProperty("StartingScenario").GetString());
			Assert.Equal(5, root.GetProperty("StartingDisplayedLevel").GetInt32());
			Assert.Equal(10, root.GetProperty("StartingLives").GetInt32());
			Assert.Equal(2.5, root.GetProperty("RimSpeedMultiplier").GetDouble());
			Assert.Equal(inputs.Length, root.GetProperty("DurationTicks").GetInt32());
			Assert.Equal(inputs.Length, root.GetProperty("Frames").GetArrayLength());

			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			PlayerInputReplay replay = file.Replay;
			Assert.Equal(new GameplayStartupConfiguration(5, 10, 2.5, 9127),
				file.StartupConfiguration);
			Assert.Equal(recorder.RandomSeed, replay.RandomSeed);
			Assert.Equal(recorder.DurationTicks, replay.DurationTicks);
			Assert.Same(LevelConfiguration.Level5Production, replay.StartingConfiguration);
			for (int tick = 0; tick < inputs.Length; tick++)
				Assert.Equal(new PlayerInputFrame(tick, inputs[tick], tick == 1),
					replay.GetFrame(tick));
			Assert.False(replay.IsActiveAt(inputs.Length));
			Assert.Throws<ArgumentOutOfRangeException>(() => replay.GetFrame(inputs.Length));
			Assert.Throws<IOException>(() => GameplayRecordingFile.SaveNew(path, recorder));
		});
	}

	[Fact]
	public void DefaultVersionTwoRecordingStoresEffectiveProductionDefaults()
	{
		PlayerInputRecorder recorder = new(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981));
		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "default.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
			JsonElement root = document.RootElement;

			Assert.Equal(2, root.GetProperty("Version").GetInt32());
			Assert.Equal(1981, root.GetProperty("RandomSeed").GetInt32());
			Assert.Equal(GameplayRecordingFile.ProductionScenario,
				root.GetProperty("StartingScenario").GetString());
			Assert.Equal(1, root.GetProperty("StartingDisplayedLevel").GetInt32());
			Assert.Equal(3, root.GetProperty("StartingLives").GetInt32());
			Assert.Equal(1.0, root.GetProperty("RimSpeedMultiplier").GetDouble());
			Assert.Equal(0, root.GetProperty("DurationTicks").GetInt32());
			Assert.Empty(root.GetProperty("Frames").EnumerateArray());
		});
	}

	[Fact]
	public void MalformedRecordingJsonFailsClearly()
	{
		WithRecording("{", exception =>
			Assert.IsAssignableFrom<JsonException>(exception));
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
		WithRecording(CreateJson(scenario: "unlisted"), exception =>
			Assert.IsType<InvalidDataException>(exception));
	}

	[Fact]
	public void VersionOneFixtureLoadsWithOriginalLevelOneDefaults()
	{
		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "version1.json");
			File.WriteAllText(path, CreateJson(version: 1));
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);

			Assert.Equal(new GameplayStartupConfiguration(1, 3, 1.0, 1981),
				file.StartupConfiguration);
			Assert.Same(LevelConfiguration.Level1, file.Replay.StartingConfiguration);
			Assert.Equal(new PlayerInputFrame(0,
				new PlayerInput(true, 1.25f, true, false, true, -0.75f), true),
				file.Replay.GetFrame(0));
		});
	}

	[Theory]
	[InlineData(1, 1, 3, 1.0, 1981)]
	[InlineData(2, 5, 10, 2.5, 9127)]
	public void SharedPlaybackSessionPreservesVersionedStartupAndFrameSemantics(
		int version, int startingLevel, int startingLives, double rimSpeed, int seed)
	{
		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, $"version{version}.json");
			File.WriteAllText(path, CreateJson(version: version, randomSeed: seed,
				startingDisplayedLevel: startingLevel, startingLives: startingLives,
				rimSpeedMultiplier: rimSpeed));
			GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
			GameplayPlaybackSession session = new(
				recording.StartupConfiguration, recording.Replay);
			TempestGame expected = new(recording.StartupConfiguration);
			PlayerInputFrame frame = recording.Replay.GetFrame(0);
			expected.RunOneStep(frame.Input, frame.RestartPressed);

			Assert.Equal(startingLevel, session.GetStateSummary().StartingDisplayedLevel);
			Assert.Equal(startingLives, session.GetStateSummary().StartingLives);
			Assert.Equal(rimSpeed, session.GetStateSummary().StartingRimSpeedMultiplier);
			Assert.Equal(seed, session.GetStateSummary().RandomSeed);
			Assert.Equal(new PlayerInputFrame(0,
				new PlayerInput(true, 1.25f, true, false, true, -0.75f), true), frame);
			Assert.True(session.StepForward());
			AssertEquivalentGameplayState(expected, session.Game);
			Assert.Equal(1, session.GetStateSummary().Boundary);
		});
	}

	[Fact]
	public void VisibleFilePlaybackAdvancesThroughTheSharedSessionWithoutSkippingFrames()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputRecorder recorder = new(startup);
		PlayerInput[] inputs =
		[
			new(false, 0, false, true),
			new(true, 0.5f, false, false, ZapPressed: true),
			default
		];
		for (int tick = 0; tick < inputs.Length; tick++)
			recorder.CaptureTick(tick, inputs[tick]);

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "visible-playback.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			VisiblePlaybackController controller = new(new GameplayPlaybackSession(
				file.StartupConfiguration, file.Replay));
			TempestGame expected = new(file.StartupConfiguration);

			for (int tick = 0; tick < file.Replay.DurationTicks; tick++)
			{
				PlayerInputFrame frame = file.Replay.GetFrame(tick);
				expected.RunOneStep(frame.Input, frame.RestartPressed);
				Assert.Equal(1, controller.ProcessControls(default));
				Assert.Equal(tick + 1, controller.Session.CurrentBoundary);
				AssertEquivalentGameplayState(expected, controller.Session.Game);
			}

			Assert.True(controller.Session.IsRecordingComplete);
			Assert.Equal(0, controller.ProcessControls(default));
			Assert.True(controller.TryMarkCompletionReported());
			Assert.False(controller.TryMarkCompletionReported());
			Assert.Equal(inputs.Length, controller.Session.CurrentBoundary);
			AssertEquivalentGameplayState(expected, controller.Session.Game);
		});
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	public void UnknownMembersRemainRejectedForBothRecordingVersions(int version)
	{
		WithRecording(CreateJson(version: version, includeUnknownMember: true),
			exception => Assert.IsType<JsonException>(exception));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(8)]
	[InlineData(-1)]
	public void VersionTwoRejectsUnsupportedStartingLevels(int level)
	{
		WithRecording(CreateJson(startingDisplayedLevel: level),
			exception => Assert.IsType<InvalidDataException>(exception));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void VersionTwoRejectsInvalidStartingLives(int lives)
	{
		WithRecording(CreateJson(startingLives: lives),
			exception => Assert.IsType<InvalidDataException>(exception));
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	[InlineData(double.NaN)]
	[InlineData(double.PositiveInfinity)]
	public void VersionTwoRejectsInvalidRimSpeed(double rimSpeed)
	{
		string value = double.IsNaN(rimSpeed) ? "\"NaN\"" :
			double.IsInfinity(rimSpeed) ? "\"Infinity\"" :
			rimSpeed.ToString("R", CultureInfo.InvariantCulture);
		string json = CreateJson(rimSpeedMultiplier: 1.0).Replace(
			"\"RimSpeedMultiplier\":1", $"\"RimSpeedMultiplier\":{value}",
			StringComparison.Ordinal);
		WithRecording(json, exception => Assert.True(
			exception is InvalidDataException or JsonException));
	}

	[Fact]
	public void VersionTwoRequiresItsCompleteMetadataShape()
	{
		WithRecording("""{"Format":"tempest-gameplay-input","Version":2}""",
			exception => Assert.IsType<JsonException>(exception));
	}

	[Fact]
	public void VersionTwoRejectsNonFiniteInputValues()
	{
		string json = CreateJson().Replace(
			"\"MouseDeltaX\":1.25", "\"MouseDeltaX\":1e100",
			StringComparison.Ordinal);
		WithRecording(json, exception => Assert.True(
			exception is InvalidDataException or JsonException));
	}

	[Fact]
	public void VersionTwoRequiresEveryInputField()
	{
		string json = CreateJson().Replace(
			"\"MouseDeltaX\":1.25,", "", StringComparison.Ordinal);
		WithRecording(json, exception => Assert.IsAssignableFrom<JsonException>(exception));
	}

	[Theory]
	[InlineData(1, -1, 1)]
	[InlineData(1, 1981, -1)]
	[InlineData(2, -1, 1)]
	[InlineData(2, 1981, -1)]
	public void BothVersionsRejectNegativeSeedOrDuration(
		int version, int randomSeed, int durationTicks)
	{
		WithRecording(CreateJson(version: version, randomSeed: randomSeed,
			durationTicks: durationTicks), exception =>
			Assert.IsType<InvalidDataException>(exception));
	}

	[Fact]
	public void InconsistentDurationAndTickMetadataFailClearly()
	{
		WithRecording(CreateJson(durationTicks: 2), exception =>
			Assert.IsType<InvalidDataException>(exception));
		WithRecording(CreateJson(simulationTick: 1), exception =>
			Assert.IsType<InvalidDataException>(exception));
		WithRecording("""{"Version":2}""", exception =>
			Assert.IsType<JsonException>(exception));
	}

	[Fact]
	public void SerializedRecordingReplaysTheCapturedSequenceFromTheSameSeed()
	{
		const int randomSeed = 1981;
		GameplayStartupConfiguration startup = new(1, 3, 1.0, randomSeed);
		PlayerInputRecorder recorder = new(startup);
		TempestGame recordedGame = new(startup);

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
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			PlayerInputReplay replay = file.Replay;
			TempestGame playbackGame = new(file.StartupConfiguration);

			for (int tick = 0; tick < replay.DurationTicks; tick++)
			{
				PlayerInputFrame frame = replay.GetFrame(tick);
				playbackGame.RunOneStep(frame.Input, frame.RestartPressed);
			}

			AssertEquivalentGameplayState(recordedGame, playbackGame);
		});
	}

	[Fact]
	public void DirectLevelFiveRecordingRestoresProductionStartupAndDeterministicState()
	{
		const int randomSeed = 1981;
		GameplayStartupConfiguration startup = new(5, 10, 2.5, randomSeed);
		PlayerInputRecorder recorder = new(startup);
		TempestGame recordedGame = new(startup);

		for (int tick = 0; tick < 180; tick++)
		{
			PlayerInput input = tick switch
			{
				< 60 => new PlayerInput(true, 0, false, false),
				< 120 => new PlayerInput(false, 0.5f, false, true),
				120 => new PlayerInput(false, 0, false, false, ZapPressed: true),
				_ => default
			};
			recordedGame.RunOneStep(input);
			recorder.CaptureTick(tick, input);
		}

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "level5.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			TempestGame playbackGame = new(file.StartupConfiguration);

			Assert.Equal(startup, file.StartupConfiguration);
			Assert.Same(LevelConfiguration.Level5Production,
				playbackGame.CurrentLevel.Configuration);
			Assert.True(playbackGame.NaturalProgressionEnabled);
			Assert.Equal(10, playbackGame.Player.Lives);
			Assert.Equal(2.5, playbackGame.CurrentLevel.RimTurnSpeedMultiplier);

			for (int tick = 0; tick < file.Replay.DurationTicks; tick++)
			{
				PlayerInputFrame frame = file.Replay.GetFrame(tick);
				playbackGame.RunOneStep(frame.Input, frame.RestartPressed);
			}

			AssertEquivalentGameplayState(recordedGame, playbackGame);
		});
	}

	[Fact]
	public void VersionTwoReplaysARecordedLifeLossAndReentry()
	{
		GameplayStartupConfiguration startup = new(1, 10, 1.0, 1981);
		PlayerInputRecorder recorder = new(startup);
		TempestGame recordedGame = new(startup);
		int tick = 0;
		while (recordedGame.Player.Lives == startup.StartingLives &&
			tick < 10_000 && !recordedGame.GameOver)
		{
			recordedGame.RunOneStep();
			recorder.CaptureTick(tick++, default);
		}

		Assert.Equal(startup.StartingLives - 1, recordedGame.Player.Lives);
		Assert.Equal(LevelState.Playing, recordedGame.State);
		Assert.Equal(0, recordedGame.DeathTicks);

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "life-loss.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			TempestGame playbackGame = new(file.StartupConfiguration);

			for (int replayTick = 0; replayTick < file.Replay.DurationTicks; replayTick++)
			{
				PlayerInputFrame frame = file.Replay.GetFrame(replayTick);
				playbackGame.RunOneStep(frame.Input, frame.RestartPressed);
			}

			AssertEquivalentGameplayState(recordedGame, playbackGame);
		});
	}

	[Fact]
	public void DirectLevelSevenIsAValidProductionRecordingStartup()
	{
		GameplayStartupConfiguration startup = new(7, 4, 1.25, 19);
		PlayerInputRecorder recorder = new(startup);
		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "level7.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			TempestGame game = new(file.StartupConfiguration);

			Assert.Equal(startup, file.StartupConfiguration);
			Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
			Assert.Equal(4, game.Player.Lives);
			Assert.Equal(1.25, game.CurrentLevel.RimTurnSpeedMultiplier);
			Assert.Equal(16, game.Spikes.Count);
		});
	}

	[Fact]
	public void ObservationDensityOptionDoesNotChangeRecordedSimulationIdentityOrState()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 4225);
		string[] lowDensityArguments =
			["--playback-file", "session.json", "--game-over-density", "0.5"];
		string[] highDensityArguments =
			["--playback-file", "session.json", "--game-over-density", "4"];
		Assert.True(DebugController.TryParseGameplayFileCommand(
			lowDensityArguments, out GameplayFileCommand? lowDensity, out _));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			highDensityArguments, out GameplayFileCommand? highDensity, out _));
		Assert.True(DebugController.TryParseGameOverDensityMultiplier(
			lowDensityArguments, out double lowValue, out _));
		Assert.True(DebugController.TryParseGameOverDensityMultiplier(
			highDensityArguments, out double highValue, out _));
		Assert.NotEqual(lowValue, highValue);
		Assert.Equal(lowDensity!.StartingDisplayedLevel, highDensity!.StartingDisplayedLevel);
		Assert.Equal(lowDensity.StartingLives, highDensity.StartingLives);
		Assert.Equal(lowDensity.RimSpeedMultiplier, highDensity.RimSpeedMultiplier);

		TempestGame lowDensityGame = new(startup);
		TempestGame highDensityGame = new(startup);
		for (int tick = 0; tick < 120; tick++)
		{
			PlayerInput input = tick % 3 == 0
				? new PlayerInput(true, 0.5f, false, true)
				: default;
			lowDensityGame.RunOneStep(input);
			highDensityGame.RunOneStep(input);
		}

		AssertEquivalentGameplayState(lowDensityGame, highDensityGame);
	}

	[Fact]
	public void RecordedFullGameRestartRetainsEffectiveLivesAndRimSpeedPolicy()
	{
		GameplayStartupConfiguration startup = new(5, 10, 2.5, 7721);
		PlayerInputRecorder recorder = new(startup);
		TempestGame recordedGame = new(startup);
		recordedGame.CurrentLevel.State = LevelState.GameOver;
		recordedGame.RunOneStep(restartPressed: true);
		recorder.CaptureTick(0, default, restartPressed: true);

		WithTemporaryDirectory(directory =>
		{
			string path = Path.Combine(directory, "restart.json");
			GameplayRecordingFile.SaveNew(path, recorder);
			GameplayRecording file = GameplayRecordingFile.LoadRecording(path);
			TempestGame playbackGame = new(file.StartupConfiguration);
			playbackGame.CurrentLevel.State = LevelState.GameOver;
			PlayerInputFrame frame = file.Replay.GetFrame(0);
			playbackGame.RunOneStep(frame.Input, frame.RestartPressed);

			AssertEquivalentGameplayState(recordedGame, playbackGame);
			// A full restart in a production campaign starts Level 1, while restoring
			// the recording's effective lives and rim-speed multiplier as before.
			Assert.Same(LevelConfiguration.Level1, playbackGame.CurrentLevel.Configuration);
			Assert.Equal(10, playbackGame.Player.Lives);
			Assert.Equal(2.5, playbackGame.CurrentLevel.RimTurnSpeedMultiplier);
		});
	}

	private static string CreateJson(
		string format = GameplayRecordingFile.FormatIdentifier,
		int version = GameplayRecordingFile.CurrentFormatVersion,
		string scenario = GameplayRecordingFile.ProductionScenario,
		int durationTicks = 1,
		int randomSeed = 1981,
		int simulationTick = 0,
		int startingDisplayedLevel = 1,
		int startingLives = 3,
		double rimSpeedMultiplier = 1.0,
		bool includeUnknownMember = false)
	{
		object frame = new
		{
			SimulationTick = simulationTick,
			RestartPressed = true,
			Input = new
			{
				Fire = true,
				MouseDeltaX = 1.25f,
				Left = true,
				Right = false,
				ZapPressed = true,
				ControllerAxis = -0.75f
			}
		};
		Dictionary<string, object?> document = version == 1
			? new Dictionary<string, object?>
			{
				["Format"] = format,
				["Version"] = version,
				["RandomSeed"] = randomSeed,
				["StartingScenario"] = GameplayRecordingFile.Level1Scenario,
				["DurationTicks"] = durationTicks,
				["Frames"] = new[] { frame }
			}
			: new Dictionary<string, object?>
			{
				["Format"] = format,
				["Version"] = version,
				["RandomSeed"] = randomSeed,
				["StartingScenario"] = scenario,
				["StartingDisplayedLevel"] = startingDisplayedLevel,
				["StartingLives"] = startingLives,
				["RimSpeedMultiplier"] = rimSpeedMultiplier,
				["DurationTicks"] = durationTicks,
				["Frames"] = new[] { frame }
			};
		if (includeUnknownMember)
			document["UnknownMember"] = true;
		return JsonSerializer.Serialize(document);
	}

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
		Assert.Equal(first.CurrentLevel.RimTurnSpeedMultiplier,
			second.CurrentLevel.RimTurnSpeedMultiplier);
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
