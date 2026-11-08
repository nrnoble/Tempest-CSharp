using System.Text.Json;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class HeadlessGameplayPlaybackTests
{
	[Theory]
	[InlineData(1, 1, 3, 1.0, 1981)]
	[InlineData(2, 5, 10, 2.5, 9127)]
	public void RunnerLoadsBothRecordingVersionsAtTheRestoredStartupBoundary(
		int version, int level, int lives, double rimSpeed, int seed)
	{
		WithRecordingFile(CreateRecordingJson(version, 1, level, lives, rimSpeed, seed),
			path =>
			{
				HeadlessGameplayPlaybackRunner runner = new(path);
				GameplayPlaybackRunResult result = runner.RunToBoundary(0);

				Assert.Equal(0, result.AchievedBoundary);
				Assert.Equal(0, result.StateSummary.NextRecordingFrame);
				Assert.Equal(1, result.RemainingFrames);
				Assert.Equal(level, result.StateSummary.StartingDisplayedLevel);
				Assert.Equal(lives, result.StateSummary.StartingLives);
				Assert.Equal(rimSpeed, result.StateSummary.StartingRimSpeedMultiplier);
				Assert.Equal(seed, result.StateSummary.RandomSeed);

				GameplayPlaybackRunResult afterFrameZero = runner.RunToBoundary(1);
				Assert.Equal(1, afterFrameZero.AchievedBoundary);
				Assert.Equal(1, afterFrameZero.StateSummary.NextRecordingFrame);
			});
	}

	[Theory]
	[InlineData(1, 1, 3, 1.0, 1981)]
	[InlineData(2, 5, 10, 2.5, 9127)]
	public void VisibleBackwardReconstructionPreservesVersionedStartupAndRecordingFile(
		int version, int level, int lives, double rimSpeed, int seed)
	{
		WithRecordingFile(CreateRecordingJson(version, 8, level, lives, rimSpeed, seed),
			path =>
			{
				byte[] originalFile = File.ReadAllBytes(path);
				GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
				VisiblePlaybackController controller = new(new GameplayPlaybackSession(
					recording.StartupConfiguration, recording.Replay));
				controller.Session.RunToBoundary(7);
				controller.ProcessControls(new PlaybackControlInput(true, false, false));

				Assert.Equal(-1, controller.ProcessControls(new PlaybackControlInput(
					PauseTogglePressed: false,
					ForwardNavigationPressed: false,
					ShiftDown: false,
					BackwardNavigationPressed: true)));
				GameplayPlaybackSession expected = CreateSession(recording);
				GameplayStateSummary expectedSummary = expected.RunToBoundary(6);
				GameplayStateSummary actualSummary = controller.Session.GetStateSummary();

				AssertEquivalentSummary(expectedSummary, actualSummary);
				Assert.Equal(level, actualSummary.StartingDisplayedLevel);
				Assert.Equal(lives, actualSummary.StartingLives);
				Assert.Equal(rimSpeed, actualSummary.StartingRimSpeedMultiplier);
				Assert.Equal(seed, actualSummary.RandomSeed);
				Assert.Equal(originalFile, File.ReadAllBytes(path));
			});
	}

	[Fact]
	public void BoundaryZeroConsumesNoFrameAndBoundaryOneConsumesOnlyFrameZero()
	{
		GameplayRecording recording = CreateRecording(3,
			new PlayerInputFrame(0, new PlayerInput(false, 0, false, true)),
			new PlayerInputFrame(1, new PlayerInput(false, 0, true, false)));
		HeadlessGameplayPlaybackRunner runner = new(recording);
		GameplayPlaybackSession expected = CreateSession(recording);

		GameplayPlaybackRunResult boundaryZero = runner.RunToBoundary(0);

		Assert.Equal(0, boundaryZero.AchievedBoundary);
		AssertEquivalentSummary(expected.GetStateSummary(), boundaryZero.StateSummary);

		expected.StepForward();
		GameplayPlaybackRunResult boundaryOne = runner.RunToBoundary(1);

		Assert.Equal(1, boundaryOne.AchievedBoundary);
		Assert.Equal(1, boundaryOne.StateSummary.NextRecordingFrame);
		Assert.Equal(2, boundaryOne.RemainingFrames);
		AssertEquivalentSummary(expected.GetStateSummary(), boundaryOne.StateSummary);
	}

	[Fact]
	public void RunToBoundaryIsSequentialAndObserverReceivesReadOnlySummaries()
	{
		GameplayRecording recording = CreateRecording(4,
			new PlayerInputFrame(0, new PlayerInput(false, 0, false, true)),
			new PlayerInputFrame(1, default),
			new PlayerInputFrame(2, new PlayerInput(false, 0, true, false)));
		HeadlessGameplayPlaybackRunner runner = new(recording);
		List<(int Boundary, GameplayStateSummary Summary)> observed = [];

		GameplayPlaybackRunResult result = runner.RunToBoundary(3,
			(boundary, summary) => observed.Add((boundary, summary)));

		Assert.Equal(new[] { 0, 1, 2, 3 }, observed.Select(item => item.Boundary));
		Assert.All(observed, item => Assert.Equal(item.Boundary, item.Summary.Boundary));
		Assert.Equal(3, result.AchievedBoundary);
		Assert.Equal(1, result.RemainingFrames);
		Assert.Equal(GameplayPlaybackCompletionReason.RequestedBoundaryReached,
			result.CompletionReason);
	}

	[Fact]
	public void RunToCompletionUsesEveryRecordedFrameAndReportsRecordingCompletion()
	{
		GameplayRecording recording = CreateRecording(3,
			new PlayerInputFrame(0, default),
			new PlayerInputFrame(1, new PlayerInput(false, 0, false, true)),
			new PlayerInputFrame(2, default, RestartPressed: true));
		HeadlessGameplayPlaybackRunner runner = new(recording);
		List<int> observedBoundaries = [];

		GameplayPlaybackRunResult result = runner.RunToCompletion(
			(boundary, _) => observedBoundaries.Add(boundary));

		Assert.Equal(new[] { 0, 1, 2, 3 }, observedBoundaries);
		Assert.Equal(3, result.AchievedBoundary);
		Assert.Equal(3, result.RecordingDurationTicks);
		Assert.Equal(0, result.RemainingFrames);
		Assert.True(result.IsRecordingComplete);
		Assert.True(result.StateSummary.IsRecordingComplete);
		Assert.Equal(GameplayPlaybackCompletionReason.RecordingCompleted,
			result.CompletionReason);

		GameplayPlaybackRunResult repeatedCompletion = runner.RunToCompletion();
		AssertEquivalentSummary(result.StateSummary, repeatedCompletion.StateSummary);
	}

	[Fact]
	public void RunnerRejectsOutOfRangeAndBackwardBoundaries()
	{
		HeadlessGameplayPlaybackRunner runner = new(CreateRecording(2));

		Assert.Throws<ArgumentOutOfRangeException>(() => runner.RunToBoundary(-1));
		Assert.Throws<ArgumentOutOfRangeException>(() => runner.RunToBoundary(3));
		Assert.Equal(1, runner.RunToBoundary(1).AchievedBoundary);
		Assert.Throws<InvalidOperationException>(() => runner.RunToBoundary(0));
	}

	[Fact]
	public void FreshRunnerMatchesTheSharedSessionAtTheSameBoundary()
	{
		GameplayRecording recording = CreateRecording(8,
			new PlayerInputFrame(0, new PlayerInput(true, 0, false, true)),
			new PlayerInputFrame(1, new PlayerInput(false, 0.5f, true, false)),
			new PlayerInputFrame(2, new PlayerInput(false, 0, false, false, ZapPressed: true)));
		GameplayPlaybackSession expected = CreateSession(recording);
		expected.RunToBoundary(6);

		GameplayPlaybackRunResult first =
			new HeadlessGameplayPlaybackRunner(recording).RunToBoundary(6);
		GameplayPlaybackRunResult second =
			new HeadlessGameplayPlaybackRunner(recording).RunToBoundary(6);

		AssertEquivalentSummary(expected.GetStateSummary(), first.StateSummary);
		AssertEquivalentSummary(first.StateSummary, second.StateSummary);
	}

	[Fact]
	public void ObserverExceptionsPropagateInsteadOfReturningPartialSuccess()
	{
		HeadlessGameplayPlaybackRunner runner = new(CreateRecording(2));
		InvalidOperationException expected = new("checkpoint observer failed");

		InvalidOperationException actual = Assert.Throws<InvalidOperationException>(() =>
			runner.RunToBoundary(1, (boundary, _) =>
			{
				if (boundary == 1)
					throw expected;
			}));

		Assert.Same(expected, actual);
		Assert.Throws<InvalidOperationException>(() => runner.RunToBoundary(0));
	}

	[Fact]
	public void CliRunsToCompletionAndRequestedBoundaryWithoutNativePresentation()
	{
		string json = CreateRecordingJson(2, durationTicks: 3, startingLevel: 5,
			startingLives: 10, rimSpeed: 2.5, seed: 9127);
		WithRecordingFile(json, path =>
		{
			(int completeExitCode, string completeOutput, string completeError) =
				RunCli("--headless-playback", path);
			(int boundaryExitCode, string boundaryOutput, string boundaryError) =
				RunCli("--headless-playback", path, "--boundary", "1");

			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, completeExitCode);
			Assert.Contains("achieved boundary: 3/3", completeOutput);
			Assert.Contains("recording completion", completeOutput);
			Assert.Empty(completeError);
			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, boundaryExitCode);
			Assert.Contains("Requested: 1", boundaryOutput);
			Assert.Contains("achieved boundary: 1/3", boundaryOutput);
			Assert.Empty(boundaryError);
		});
	}

	[Fact]
	public void CliResolvesBareRecordingNameUnderApplicationUserData()
	{
		string applicationRoot = Path.Combine(Path.GetTempPath(),
			$"tempest-headless-userdata-tests-{Guid.NewGuid():N}");
		ApplicationPaths paths = new(applicationRoot);

		try
		{
			Directory.CreateDirectory(paths.GameplayRecordingsDirectory);
			File.WriteAllText(
				Path.Combine(paths.GameplayRecordingsDirectory, "recording.json"),
				CreateRecordingJson(2, durationTicks: 1, startingLevel: 5,
					startingLives: 10, rimSpeed: 1.0, seed: 9127));

			using StringWriter standardOutput = new();
			using StringWriter standardError = new();
			int exitCode = HeadlessGameplayPlaybackCli.Run(
				["--headless-playback", "recording.json"],
				standardOutput, standardError, paths);

			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, exitCode);
			Assert.Contains("achieved boundary: 1/1", standardOutput.ToString());
			Assert.Empty(standardError.ToString());
		}
		finally
		{
			if (Directory.Exists(applicationRoot))
				Directory.Delete(applicationRoot, recursive: true);
		}
	}

	[Fact]
	public void CliJsonHasStableExplicitSchemaAndFlipperDiagnostics()
	{
		string json = CreateRecordingJson(2, durationTicks: 2500,
			startingLevel: 5, startingLives: 10, rimSpeed: 1.0, seed: 1981);
		WithRecordingFile(json, path =>
		{
			GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
			GameplayPlaybackSession session = CreateSession(recording);
			while (session.Game.Flippers.Count < 2 && session.StepForward())
			{
			}
			Assert.True(session.Game.Flippers.Count >= 2);
			int diagnosticBoundary = session.CurrentBoundary;

			(int firstExitCode, string firstOutput, string firstError) =
				RunCli("--headless-playback", path, "--boundary",
					diagnosticBoundary.ToString(), "--json");
			(int secondExitCode, string secondOutput, _) =
				RunCli("--headless-playback", path, "--boundary",
					diagnosticBoundary.ToString(), "--json");

			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, firstExitCode);
			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, secondExitCode);
			Assert.Empty(firstError);
			Assert.Equal(firstOutput, secondOutput);

			using JsonDocument document = JsonDocument.Parse(firstOutput);
			JsonElement root = document.RootElement;
			Assert.Equal(new[]
			{
				"requestedBoundary", "achievedBoundary", "recordingDurationTicks",
				"remainingFrames", "completionReason", "stateSummary"
			}, root.EnumerateObject().Select(property => property.Name));
			Assert.Equal(diagnosticBoundary,
				root.GetProperty("requestedBoundary").GetInt32());
			Assert.Equal(diagnosticBoundary,
				root.GetProperty("achievedBoundary").GetInt32());
			Assert.Equal(2500,
				root.GetProperty("recordingDurationTicks").GetInt32());
			Assert.Equal(2500 - diagnosticBoundary,
				root.GetProperty("remainingFrames").GetInt32());
			Assert.Equal("RequestedBoundaryReached",
				root.GetProperty("completionReason").GetString());

			JsonElement summary = root.GetProperty("stateSummary");
			Assert.Equal(diagnosticBoundary, summary.GetProperty("boundary").GetInt32());
			Assert.Equal(5, summary.GetProperty("currentDisplayedLevel").GetInt32());
			Assert.Equal(10, summary.GetProperty("startingLives").GetInt32());
			Assert.True(summary.TryGetProperty("score", out _));
			Assert.True(summary.TryGetProperty("remainingLives", out _));
			Assert.True(summary.TryGetProperty("playerLane", out _));
			Assert.True(summary.TryGetProperty("activeFlipperCount", out _));
			JsonElement flippers = summary.GetProperty("flippers");
			Assert.NotEmpty(flippers.EnumerateArray());
			Assert.All(flippers.EnumerateArray(), flipper =>
			{
				Assert.True(flipper.TryGetProperty("originalSlotId", out _));
				Assert.True(flipper.TryGetProperty("movementState", out _));
				Assert.InRange(flipper.GetProperty("pivotProgress").GetSingle(), 0, 1);
			});
		});
	}

	[Theory]
	[InlineData("--headless-playback")]
	[InlineData("--boundary", "1")]
	[InlineData("--json")]
	public void CliRejectsMissingFilenameOrOrphanedHeadlessOptions(params string[] arguments)
	{
		(int exitCode, _, string error) = RunCli(arguments);

		Assert.Equal(HeadlessGameplayPlaybackCli.InvalidCommandExitCode, exitCode);
		Assert.Contains("INVALID COMMAND", error);
	}

	[Theory]
	[InlineData("--headless-playback", "recording.json", "--boundary", "bad")]
	[InlineData("--headless-playback", "recording.json", "--boundary")]
	[InlineData("--headless-playback", "recording.json", "--level", "5")]
	[InlineData("--headless-playback", "recording.json", "--lives", "10")]
	[InlineData("--headless-playback", "recording.json", "--rim-speed", "2.5")]
	[InlineData("--headless-playback", "recording.json", "--speed", "0.5")]
	[InlineData("--headless-playback", "recording.json", "--level5-test")]
	[InlineData("--headless-playback", "recording.json", "--boundary", "1", "--boundary", "2")]
	public void CliRejectsMalformedAndIncompatibleArguments(params string[] arguments)
	{
		(int exitCode, _, string error) = RunCli(arguments);

		Assert.Equal(HeadlessGameplayPlaybackCli.InvalidCommandExitCode, exitCode);
		Assert.Contains("INVALID COMMAND", error);
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(2)]
	public void CliRejectsBoundaryOutsideDurationWithDistinctNonzeroStatus(int boundary)
	{
		WithRecordingFile(CreateRecordingJson(2, durationTicks: 1), path =>
		{
			(int exitCode, _, string error) =
				RunCli("--headless-playback", path, "--boundary",
					boundary.ToString());

			Assert.Equal(HeadlessGameplayPlaybackCli.OutOfRangeBoundaryExitCode, exitCode);
			Assert.Contains("OUT-OF-RANGE BOUNDARY", error);
		});
	}

	[Fact]
	public void CliJsonCompletionReportsNullRequestedBoundaryAndExactDuration()
	{
		WithRecordingFile(CreateRecordingJson(2, durationTicks: 1), path =>
		{
			(int exitCode, string output, string error) =
				RunCli("--headless-playback", path, "--json");

			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, exitCode);
			Assert.Empty(error);
			using JsonDocument document = JsonDocument.Parse(output);
			JsonElement root = document.RootElement;
			Assert.Equal(JsonValueKind.Null, root.GetProperty("requestedBoundary").ValueKind);
			Assert.Equal(1, root.GetProperty("achievedBoundary").GetInt32());
			Assert.Equal(1, root.GetProperty("recordingDurationTicks").GetInt32());
			Assert.Equal(0, root.GetProperty("remainingFrames").GetInt32());
			Assert.Equal("RecordingCompleted",
				root.GetProperty("completionReason").GetString());
			Assert.True(root.GetProperty("stateSummary")
				.GetProperty("isRecordingComplete").GetBoolean());
		});
	}

	[Theory]
	[InlineData("{")]
	[InlineData("""{"Format":"tempest-gameplay-input","Version":99}""")]
	public void CliReportsMalformedAndUnsupportedRecordingsAsFailures(string recordingJson)
	{
		WithRecordingFile(recordingJson, path =>
		{
			(int exitCode, string output, string error) =
				RunCli("--headless-playback", path);

			Assert.Equal(HeadlessGameplayPlaybackCli.InvalidRecordingExitCode, exitCode);
			Assert.Empty(output);
			Assert.Contains("INVALID RECORDING", error);
			Assert.NotEqual(HeadlessGameplayPlaybackCli.SuccessExitCode, exitCode);
		});
	}

	[Fact]
	public void CliDoesNotModifyRecordingFiles()
	{
		string original = CreateRecordingJson(2, durationTicks: 2);
		WithRecordingFile(original, path =>
		{
			byte[] before = File.ReadAllBytes(path);
			(int exitCode, _, string error) = RunCli("--headless-playback", path, "--json");

			Assert.Equal(HeadlessGameplayPlaybackCli.SuccessExitCode, exitCode);
			Assert.Empty(error);
			Assert.Equal(before, File.ReadAllBytes(path));
		});
	}

	private static GameplayRecording CreateRecording(
		int durationTicks,
		params PlayerInputFrame[] frames)
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		return new GameplayRecording(startup, new PlayerInputReplay(
			startup.RandomSeed, durationTicks, frames, startup.ResolveStartingConfiguration()));
	}

	private static GameplayPlaybackSession CreateSession(GameplayRecording recording) =>
		new(recording.StartupConfiguration, recording.Replay);

	private static string CreateRecordingJson(
		int version,
		int durationTicks,
		int startingLevel = 1,
		int startingLives = 3,
		double rimSpeed = 1.0,
		int seed = 1981)
	{
		object[] frames = Enumerable.Range(0, durationTicks)
			.Select(tick => (object)new
			{
				SimulationTick = tick,
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
			})
			.ToArray();
		Dictionary<string, object?> document = new()
		{
			["Format"] = GameplayRecordingFile.FormatIdentifier,
			["Version"] = version,
			["RandomSeed"] = seed,
			["StartingScenario"] = version == 1
				? GameplayRecordingFile.Level1Scenario
				: GameplayRecordingFile.ProductionScenario,
			["DurationTicks"] = durationTicks,
			["Frames"] = frames
		};
		if (version == 2)
		{
			document["StartingDisplayedLevel"] = startingLevel;
			document["StartingLives"] = startingLives;
			document["RimSpeedMultiplier"] = rimSpeed;
		}

		return JsonSerializer.Serialize(document);
	}

	private static (int ExitCode, string Output, string Error) RunCli(
		params string[] arguments)
	{
		using StringWriter standardOutput = new();
		using StringWriter standardError = new();
		int exitCode = HeadlessGameplayPlaybackCli.Run(
			arguments, standardOutput, standardError);
		return (exitCode, standardOutput.ToString(), standardError.ToString());
	}

	private static void WithRecordingFile(string json, Action<string> action)
	{
		string directory = Path.Combine(Path.GetTempPath(),
			$"tempest-headless-playback-tests-{Guid.NewGuid():N}");
		Directory.CreateDirectory(directory);
		string path = Path.Combine(directory, "recording.json");
		try
		{
			File.WriteAllText(path, json);
			action(path);
		}
		finally
		{
			Directory.Delete(directory, recursive: true);
		}
	}

	private static void AssertEquivalentSummary(
		GameplayStateSummary expected,
		GameplayStateSummary actual)
	{
		Assert.Equal(expected.Boundary, actual.Boundary);
		Assert.Equal(expected.RecordingDurationTicks, actual.RecordingDurationTicks);
		Assert.Equal(expected.NextRecordingFrame, actual.NextRecordingFrame);
		Assert.Equal(expected.RemainingRecordedFrames, actual.RemainingRecordedFrames);
		Assert.Equal(expected.IsRecordingComplete, actual.IsRecordingComplete);
		Assert.Equal(expected.StartingDisplayedLevel, actual.StartingDisplayedLevel);
		Assert.Equal(expected.StartingLives, actual.StartingLives);
		Assert.Equal(expected.StartingRimSpeedMultiplier, actual.StartingRimSpeedMultiplier);
		Assert.Equal(expected.RandomSeed, actual.RandomSeed);
		Assert.Equal(expected.CurrentDisplayedLevel, actual.CurrentDisplayedLevel);
		Assert.Equal(expected.LevelState, actual.LevelState);
		Assert.Equal(expected.Score, actual.Score);
		Assert.Equal(expected.RemainingLives, actual.RemainingLives);
		Assert.Equal(expected.PlayerLane, actual.PlayerLane);
		Assert.Equal(expected.PlayerLanePosition, actual.PlayerLanePosition);
		Assert.Equal(expected.PlayerDepth, actual.PlayerDepth);
		Assert.Equal(expected.SuperZapperChargesRemaining, actual.SuperZapperChargesRemaining);
		Assert.Equal(expected.PlayerShotCount, actual.PlayerShotCount);
		Assert.Equal(expected.EnemyBulletCount, actual.EnemyBulletCount);
		Assert.Equal(expected.ActiveEggCount, actual.ActiveEggCount);
		Assert.Equal(expected.ActiveFlipperCount, actual.ActiveFlipperCount);
		Assert.Equal(expected.ActiveTankerCount, actual.ActiveTankerCount);
		Assert.Equal(expected.ActiveSpikerCount, actual.ActiveSpikerCount);
		Assert.Equal(expected.ActiveSpikeCount, actual.ActiveSpikeCount);
		Assert.Equal(expected.ActiveFuseballCount, actual.ActiveFuseballCount);
		Assert.Equal(expected.ActivePulsarCount, actual.ActivePulsarCount);
		Assert.Equal(expected.Flippers, actual.Flippers);
	}
}
