using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Raylib_cs;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PlaybackScreenshotCaptureTests
{
	[Theory]
	[InlineData(true, false, true, false)]
	[InlineData(false, true, true, false)]
	[InlineData(true, true, false, false)]
	[InlineData(true, true, true, true)]
	public void CtrlShiftSPressEdgeIsTheOnlyScreenshotCommand(
		bool controlDown, bool shiftDown, bool sPressed, bool expected)
	{
		PlaybackScreenshotInput input = new(
			LeftControlDown: controlDown,
			RightControlDown: false,
			LeftShiftDown: shiftDown,
			RightShiftDown: false,
			SPressed: sPressed);

		Assert.Equal(expected, input.IsScreenshotCommand);
	}

	[Fact]
	public void EitherControlAndEitherShiftCanCompleteTheScreenshotChord()
	{
		PlaybackScreenshotInput[] validChords =
		[
			new(true, false, true, false, true),
			new(true, false, false, true, true),
			new(false, true, true, false, true),
			new(false, true, false, true, true),
			new(true, true, true, true, true)
		];

		Assert.All(validChords, input => Assert.True(input.IsScreenshotCommand));
	}

	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	[InlineData(false, false)]
	public void SWithoutBothModifiersDoesNotRequestScreenshot(
		bool controlDown, bool shiftDown)
	{
		PlaybackScreenshotInput input = new(controlDown, false, shiftDown, false, true);

		Assert.False(input.IsScreenshotCommand);
	}

	[Fact]
	public void ScreenshotCommandRequiresPausedVisibleFilePlayback()
	{
		PlaybackScreenshotInput chord = new(true, false, false, true, true);

		Assert.True(PlaybackScreenshotCapture.IsCaptureRequested(true, true, chord));
		Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(true, false, chord));
		Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(false, true, chord));
		Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(false, false, chord));
	}

	[Fact]
	public void ScreenshotCommandRequiresAnSPressEdge()
	{
		PlaybackScreenshotInput withoutSPress = new(true, false, true, false, false);

		Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(
			filePlaybackActive: true, playbackPaused: true, withoutSPress));
	}

	[Fact]
	public void ScreenshotChordDiagnosticReportsInputAndNewNativeFilesOnce()
	{
		List<NativeScreenshotSnapshot> nativeFiles = [];
		StringWriter output = new();
		PlaybackScreenshotKeyDiagnostic diagnostic = new(() => nativeFiles, output);

		diagnostic.Observe(controlDown: true, shiftDown: true,
			sPressed: true, f12Pressed: false);
		diagnostic.RecordProjectCapture(
			"UserData/Screenshots/playback-level-7-boundary-004820.png",
			"C:\\Tempest\\UserData\\Screenshots\\playback-level-7-boundary-004820.png",
			fileExists: true,
			fileLength: 4);
		nativeFiles.Add(new NativeScreenshotSnapshot(
			"screenshot005.png", 1200, DateTime.UtcNow));
		diagnostic.Observe(controlDown: true, shiftDown: true,
			sPressed: false, f12Pressed: true);
		for (int frame = 0; frame < 17; frame++)
			diagnostic.Observe(false, false, false, false);

		Assert.True(diagnostic.HasReported);
		Assert.Contains("Control observed=True", output.ToString());
		Assert.Contains("Shift observed=True", output.ToString());
		Assert.Contains("S press edge observed=True (1)", output.ToString());
		Assert.Contains("F12 press edge observed=True (1)", output.ToString());
		Assert.Contains("screenshot005.png", output.ToString());
		Assert.Contains("Project capture request created: True", output.ToString());
		Assert.Contains(
			"Raylib-relative path passed to TakeScreenshot: " +
			"UserData/Screenshots/playback-level-7-boundary-004820.png",
			output.ToString());
		Assert.Contains("Project screenshot file length: 4 bytes", output.ToString());
		diagnostic.Observe(true, true, true, true);
		Assert.Equal(1, output.ToString().Split(
			"Screenshot chord diagnostic", StringSplitOptions.None).Length - 1);
	}

	[Fact]
	public void ScreenshotKeyDiagnosticRecordsProjectOutputEvidence()
	{
		StringWriter output = new();
		PlaybackScreenshotKeyDiagnostic diagnostic = new(
			() => Array.Empty<NativeScreenshotSnapshot>(), output);
		diagnostic.Observe(true, true, true, false);
		string destination = Path.Combine("UserData", "Screenshots",
			"playback-level-7-boundary-004820.png");
		diagnostic.RecordProjectCapture(
			destination,
			Path.GetFullPath(destination),
			fileExists: true,
			fileLength: 4);
		for (int frame = 0; frame < 17; frame++)
			diagnostic.Observe(false, false, false, false);

		Assert.Contains("Project capture request created: True", output.ToString());
		Assert.Contains(destination, output.ToString());
		Assert.Contains("Project screenshot file exists: True", output.ToString());
		Assert.Contains("Project screenshot file length: 4 bytes", output.ToString());
		Assert.Contains("New or overwritten native screenshotNNN.png files since diagnostic start: none observed.",
			output.ToString());
	}

	[Fact]
	public void ScreenshotKeyDiagnosticDetectsNativeScreenshotOverwrite()
	{
		DateTime before = DateTime.UtcNow.AddSeconds(-1);
		List<NativeScreenshotSnapshot> nativeFiles =
			[new("screenshot000.png", 1024, before)];
		StringWriter output = new();
		PlaybackScreenshotKeyDiagnostic diagnostic = new(() => nativeFiles, output);

		nativeFiles[0] = new("screenshot000.png", 2048, DateTime.UtcNow);
		diagnostic.Observe(false, false, false, true);
		for (int frame = 0; frame < 17; frame++)
			diagnostic.Observe(false, false, false, false);

		Assert.Contains("screenshot000.png", output.ToString());
		Assert.Contains("New or overwritten native screenshotNNN.png files",
			output.ToString());
	}

	[Fact]
	public void CaptureWritesBoundaryFilenameUnderUserDataAndNeverOverwrites()
	{
		string applicationRoot = CreateTemporaryDirectory();
		try
		{
			StringWriter output = new();
			StringWriter error = new();
			List<string> requestedPaths = [];
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(applicationRoot),
				path =>
				{
					requestedPaths.Add(path);
					File.WriteAllBytes(Path.Combine(applicationRoot, path), [137, 80, 78, 71]);
				},
				output,
				error);

			Assert.False(Directory.Exists(Path.Combine(applicationRoot, "UserData")));
			Assert.True(capture.TryCapture(true, true,
				new PlaybackScreenshotInput(true, false, true, false, true), 7, 4820));
			Assert.True(capture.TryCapture(true, true,
				new PlaybackScreenshotInput(true, false, true, false, true), 7, 4820));

			string outputDirectory = Path.Combine(applicationRoot, "UserData", "Screenshots");
			string first = Path.Combine(outputDirectory,
				"playback-level-7-boundary-004820.png");
			string second = Path.Combine(outputDirectory,
				"playback-level-7-boundary-004820-002.png");
			string[] files = Directory.GetFiles(outputDirectory);
			Assert.Equal(2, files.Length);
			Assert.Contains(first, files);
			Assert.Contains(second, files);
			Assert.Equal(
				[
					"UserData/Screenshots/playback-level-7-boundary-004820.png",
					"UserData/Screenshots/playback-level-7-boundary-004820-002.png"
				],
				requestedPaths);
			Assert.All(requestedPaths, path => Assert.False(Path.IsPathRooted(path)));
			Assert.Equal(second, capture.LastRequestedPath);
			Assert.Equal(requestedPaths[1], capture.LastRaylibPath);
			Assert.Equal(4, capture.LastFileLength);
			Assert.Equal([137, 80, 78, 71], File.ReadAllBytes(first));
			Assert.Equal([137, 80, 78, 71], File.ReadAllBytes(second));
			Assert.Contains(first, output.ToString());
			Assert.Contains($"Screenshot saved: {first}", output.ToString());
			Assert.Contains("playback-level-7-boundary-004820-002.png", output.ToString());
			Assert.Empty(error.ToString());
			Assert.False(Directory.Exists(Path.Combine(
				applicationRoot, "UserData", "GameplayRecordings")));
			Assert.False(capture.TryCapture(true, true,
				new PlaybackScreenshotInput(true, false, true, false, false), 7, 4820));
			Assert.Equal(2, requestedPaths.Count);
		}
		finally
		{
			Directory.Delete(applicationRoot, recursive: true);
		}
	}

	[Fact]
	public void SuccessfulCaptureDoesNotAdvancePausedPlaybackBoundaryOrCursor()
	{
		string applicationRoot = CreateTemporaryDirectory();
		try
		{
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(applicationRoot),
				path => File.WriteAllBytes(Path.Combine(applicationRoot, path), [137, 80, 78, 71]),
				TextWriter.Null,
				TextWriter.Null);
			GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
			PlayerInputReplay replay = new(1981, 3, [], LevelConfiguration.Level1);
			VisiblePlaybackController controller =
				new(new GameplayPlaybackSession(startup, replay));
			controller.ProcessControls(new PlaybackControlInput(true, false, false));
			GameplayStateSummary before = controller.Session.GetStateSummary();

			Assert.True(capture.TryCapture(
				controller.IsActive,
				controller.IsPaused,
				input: new PlaybackScreenshotInput(true, false, true, false, true),
				before.CurrentDisplayedLevel,
				before.Boundary));

			GameplayStateSummary after = controller.Session.GetStateSummary();
			Assert.True(controller.IsPaused);
			Assert.Equal(before.Boundary, after.Boundary);
			Assert.Equal(before.NextRecordingFrame, after.NextRecordingFrame);
			Assert.Equal(before.RemainingRecordedFrames, after.RemainingRecordedFrames);
			Assert.Equal(before.PlayerLane, after.PlayerLane);
			Assert.Equal(before.Score, after.Score);
		}
		finally
		{
			Directory.Delete(applicationRoot, recursive: true);
		}
	}

	[Fact]
	public void HeldOrRepeatedKeyWithoutAnotherPressEdgeDoesNotRequestAnotherCapture()
	{
		string applicationRoot = CreateTemporaryDirectory();
		try
		{
			int requestedCount = 0;
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(applicationRoot),
				path =>
				{
					requestedCount++;
					File.WriteAllBytes(Path.Combine(applicationRoot, path), [137, 80, 78, 71]);
				},
				TextWriter.Null, TextWriter.Null);

			Assert.True(capture.TryCapture(true, true,
				new PlaybackScreenshotInput(true, false, true, false, true), 7, 4820));
			Assert.False(capture.TryCapture(true, true,
				new PlaybackScreenshotInput(true, false, true, false, false), 7, 4820));
			Assert.Equal(1, requestedCount);
		}
		finally
		{
			Directory.Delete(applicationRoot, recursive: true);
		}
	}

	[Fact]
	public void CaptureFailureIsReportedAndDoesNotAdvancePausedPlayback()
	{
		string applicationRoot = CreateTemporaryDirectory();
		try
		{
			StringWriter output = new();
			StringWriter error = new();
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(applicationRoot),
				_ => throw new IOException("test capture failure"),
				output,
				error);
			GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
			PlayerInputReplay replay = new(1981, 3, [], LevelConfiguration.Level1);
			VisiblePlaybackController controller =
				new(new GameplayPlaybackSession(startup, replay));
			controller.ProcessControls(new PlaybackControlInput(true, false, false));
			GameplayStateSummary before = controller.Session.GetStateSummary();

			Assert.False(capture.TryCapture(
				controller.IsActive,
				controller.IsPaused,
				input: new PlaybackScreenshotInput(true, false, true, false, true),
				before.CurrentDisplayedLevel,
				before.Boundary));

			GameplayStateSummary after = controller.Session.GetStateSummary();
			Assert.True(controller.IsPaused);
			Assert.Equal(before.Boundary, after.Boundary);
			Assert.Equal(before.NextRecordingFrame, after.NextRecordingFrame);
			Assert.Equal(before.RemainingRecordedFrames, after.RemainingRecordedFrames);
			Assert.Equal(before.PlayerLane, after.PlayerLane);
			Assert.Contains(
				"Raylib-relative path: UserData/Screenshots/playback-level-1-boundary-000000.png",
				error.ToString());
			Assert.Contains(
				Path.Combine(applicationRoot, "UserData", "Screenshots",
					"playback-level-1-boundary-000000.png"),
				error.ToString());
			Assert.Contains("test capture failure", error.ToString());
			Assert.Empty(output.ToString());
			Assert.Empty(Directory.GetFiles(Path.Combine(
				applicationRoot, "UserData", "Screenshots")));
		}
		finally
		{
			Directory.Delete(applicationRoot, recursive: true);
		}
	}

	[Fact]
	public void EmptyRaylibOutputReportsBothPathsAndIsNotReportedAsSuccess()
	{
		string developmentRoot = CreateTemporaryDirectory();
		try
		{
			StringWriter error = new();
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(developmentRoot),
				_ => { },
				TextWriter.Null,
				error);

			Assert.False(capture.TryCapture(
				filePlaybackActive: true,
				playbackPaused: true,
				input: new PlaybackScreenshotInput(true, false, false, true, true),
				displayedLevel: 2,
				boundary: 988));

			string expectedFullDestination = Path.Combine(
				developmentRoot, "UserData", "Screenshots",
				"playback-level-2-boundary-000988.png");
			const string expectedRaylibPath =
				"UserData/Screenshots/playback-level-2-boundary-000988.png";
			Assert.Equal(expectedRaylibPath, capture.LastRaylibPath);
			Assert.Equal(expectedFullDestination, capture.LastRequestedPath);
			Assert.Equal(0, capture.LastFileLength);
			Assert.Contains(expectedRaylibPath, error.ToString());
			Assert.Contains(expectedFullDestination, error.ToString());
			Assert.Contains("non-empty PNG file", error.ToString());
			Assert.False(File.Exists(expectedFullDestination));
		}
		finally
		{
			Directory.Delete(developmentRoot, recursive: true);
		}
	}

	[Fact]
	public void RunningPlaybackAndLiveGameplayDoNotCreateUserDataScreenshotDirectory()
	{
		string applicationRoot = CreateTemporaryDirectory();
		try
		{
			PlaybackScreenshotCapture capture = new(
				new ApplicationPaths(applicationRoot),
				_ => throw new InvalidOperationException(),
				TextWriter.Null, TextWriter.Null);

			PlaybackScreenshotInput chord = new(true, false, false, true, true);
			Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(true, false, chord));
			Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(false, false, chord));
			Assert.False(PlaybackScreenshotCapture.IsCaptureRequested(false, true, chord));
			Assert.False(Directory.Exists(Path.Combine(applicationRoot, "UserData")));
		}
		finally
		{
			Directory.Delete(applicationRoot, recursive: true);
		}
	}

	private static string CreateTemporaryDirectory()
	{
		string path = Path.Combine(Path.GetTempPath(),
			"TempestScreenshotTests", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(path);
		return path;
	}
}
