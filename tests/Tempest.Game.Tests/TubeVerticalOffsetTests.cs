using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class TubeVerticalOffsetTests
{
	[Fact]
	public void OmittedOffsetDefaultsToIdentity()
	{
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			[], out TubeVerticalOffsetOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(0f, options.Offset);
		Assert.False(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("0", 0f)]
	[InlineData("38.5", 38.5f)]
	[InlineData("-42.25", -42.25f)]
	[InlineData("1e30", 1e30f)]
	[InlineData("-1e30", -1e30f)]
	[InlineData("3.4028235e38", float.MaxValue)]
	[InlineData("-3.4028235e38", float.MinValue)]
	public void ParsesFiniteSignedOffsetsWithoutAnAestheticLimit(
		string value, float expected)
	{
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			ArgsForLevel(1, "--tube-vertical-offset", value),
			out TubeVerticalOffsetOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(expected, options.Offset);
		Assert.True(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("")]
	[InlineData("invalid")]
	[InlineData("1,5")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("1e1000")]
	public void RejectsMalformedAndNonFiniteValues(string value)
	{
		Assert.False(DebugController.TryParseTubeVerticalOffsetOptions(
			ArgsForLevel(1, "--tube-vertical-offset", value),
			out _, out string? error));

		Assert.Contains("finite signed", error);
	}

	[Fact]
	public void RejectsMissingDuplicateAndJoinedOptions()
	{
		Assert.False(DebugController.TryParseTubeVerticalOffsetOptions(
			["--atari-presentation", "--level", "1", "--tube-vertical-offset"],
			out _, out _));
		Assert.False(DebugController.TryParseTubeVerticalOffsetOptions(
			["--atari-presentation", "--level", "1",
				"--tube-vertical-offset", "10", "--tube-vertical-offset", "-10"],
			out _, out _));
		Assert.False(DebugController.TryParseTubeVerticalOffsetOptions(
			["--atari-presentation", "--level", "1", "--tube-vertical-offset=10"],
			out _, out string? joinedValueError));
		Assert.Contains("separate value", joinedValueError);
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			["--level", "1", "--tube-vertical-offset", "10"],
			out TubeVerticalOffsetOptions defaultAtariOffset,
			out string? presentationError));
		Assert.Null(presentationError);
		Assert.Equal(10f, defaultAtariOffset.Offset);
		for (int level = 1; level <= 10; level++)
			Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
				ArgsForLevel(level, "--tube-vertical-offset", "10"), out _, out _));
	}

	[Fact]
	public void AllControlsCombineIndependentlyOnAnAdditionalLevel()
	{
		string[] arguments =
		[
			"--atari-presentation", "--level", "4",
			"--tube-y-gain", "1.32", "--tube-scale", ".8",
			"--tube-vertical-offset", "-42.25"
		];

		Assert.True(DebugController.TryParseTubeYGainOptions(
			arguments, out TubeYGainOptions gain, out string? gainError));
		Assert.True(DebugController.TryParseTubeScaleOptions(
			arguments, out TubeScaleOptions scale, out string? scaleError));
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			arguments, out TubeVerticalOffsetOptions offset, out string? offsetError));

		Assert.Null(gainError);
		Assert.Null(scaleError);
		Assert.Null(offsetError);
		Assert.Equal(1.32f, gain.Gain);
		Assert.Equal(.8f, scale.Scale);
		Assert.Equal(-42.25f, offset.Offset);
	}

	[Fact]
	public void AllControlsCanBeCombinedOnLevel7()
	{
		string[] arguments =
		[
			"--atari-presentation", "--level", "7",
			"--tube-y-gain", "1.45", "--tube-scale", ".8",
			"--tube-vertical-offset", "68"
		];

		Assert.True(DebugController.TryParseTubeYGainOptions(
			arguments, out TubeYGainOptions gain, out string? gainError));
		Assert.True(DebugController.TryParseTubeScaleOptions(
			arguments, out TubeScaleOptions scale, out string? scaleError));
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			arguments, out TubeVerticalOffsetOptions offset, out string? offsetError));

		Assert.Null(gainError);
		Assert.Null(scaleError);
		Assert.Null(offsetError);
		Assert.Equal(1.45f, gain.Gain);
		Assert.Equal(.8f, scale.Scale);
		Assert.Equal(68f, offset.Offset);
	}

	[Fact]
	public void PlaybackAcceptsOffsetAndDoesNotStoreIt()
	{
		string[] playbackArguments =
		[
			"--atari-presentation", "--playback-file", "session.json",
			"--tube-vertical-offset", "-55"
		];
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			playbackArguments, out TubeVerticalOffsetOptions playbackOffset,
			out string? playbackOffsetError));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			playbackArguments, out GameplayFileCommand? playbackCommand,
			out string? playbackCommandError));
		Assert.Equal(new GameplayFileCommand(
			GameplayFileMode.Playback, "session.json"), playbackCommand);
		Assert.Null(playbackOffsetError);
		Assert.Null(playbackCommandError);
		Assert.DoesNotContain("VerticalOffset", JsonSerializer.Serialize(playbackCommand));

		string[] recordingArguments =
		[
			"--record-gameplay", "session.json", "--level", "7",
			"--atari-presentation", "--tube-vertical-offset", "55"
		];
		Assert.True(DebugController.TryParseGameplayFileCommand(
			recordingArguments, out GameplayFileCommand? recordingCommand,
			out string? recordingError));
		Assert.Equal(new GameplayFileCommand(
			GameplayFileMode.Record, "session.json", 7), recordingCommand);
		Assert.Null(recordingError);

		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-tube-vertical-offset-{Guid.NewGuid():N}");
		Directory.CreateDirectory(directory);
		try
		{
			string path = Path.Combine(directory, "recording.json");
			PlayerInputRecorder recorder = new(
				new GameplayStartupConfiguration(7, 3, 1.0, 1981));
			GameplayRecordingFile.SaveNew(path, recorder);
			using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));

			Assert.Equal(GameplayRecordingFile.CurrentFormatVersion,
				document.RootElement.GetProperty("Version").GetInt32());
			Assert.False(document.RootElement.TryGetProperty("TubeVerticalOffset", out _));
		}
		finally
		{
			Directory.Delete(directory, recursive: true);
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(7)]
	public void OmittedAndExplicitSelectedOffsetRenderIdentically(int displayedLevel)
	{
		TempestGame game = new(startingProductionLevel: displayedLevel);
		float selectedOffset =
			AtariPresentationCalibration.ForDisplayedLevel(displayedLevel).VerticalOffset;
		List<(Vector2 Start, Vector2 End, Color Color)> omitted = CaptureTube(
			new VectorRenderer(game, atariPresentationEnabled: true));
		List<(Vector2 Start, Vector2 End, Color Color)> selected = CaptureTube(
			new VectorRenderer(game, atariPresentationEnabled: true,
				tubeVerticalOffset: selectedOffset));

		Assert.Equal(omitted, selected);
	}

	[Theory]
	[InlineData(1, 1.32f, .8f, 38.5f)]
	[InlineData(2, 1.32f, .8f, 38.5f)]
	[InlineData(3, 1.32f, .8f, 38.5f)]
	[InlineData(4, 1.32f, .8f, 38.5f)]
	[InlineData(5, 1.32f, .8f, 38.5f)]
	[InlineData(6, 1.32f, .8f, 38.5f)]
	[InlineData(7, 1.45f, 1f, -42.25f)]
	[InlineData(8, 1.32f, .8f, 38.5f)]
	[InlineData(9, 1.32f, .8f, 38.5f)]
	[InlineData(10, 1.32f, .8f, 38.5f)]
	public void TranslatesTubeAndAttachedEntitiesWithoutChangingShapeOrModel(
		int displayedLevel, float tubeYGain, float tubeScale, float offset)
	{
		TempestGame game = new(startingProductionLevel: displayedLevel);
		Spike spike = game.CurrentLevel.CreateSpike(2, 80);
		Vector2[] pointsBefore = game.CurrentLevel.Definition.Points.ToArray();
		int playerLaneBefore = game.Player.EdgeIndex;
		float playerDepthBefore = game.Player.Depth;
		int scoreBefore = game.Score.Score;
		LevelState stateBefore = game.State;
		VectorRenderer baselineRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: tubeYGain, tubeScale: tubeScale, tubeVerticalOffset: 0);
		VectorRenderer offsetRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: tubeYGain, tubeScale: tubeScale,
			tubeVerticalOffset: offset);

		AssertTranslatedTube(
			CaptureTube(baselineRenderer), CaptureTube(offsetRenderer), offset);

		List<(Vector2 Start, Vector2 End, Color Color)> baselinePlayer = [];
		List<(Vector2 Start, Vector2 End, Color Color)> offsetPlayer = [];
		baselineRenderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			(start, end, color) => baselinePlayer.Add((start, end, color)));
		offsetRenderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			(start, end, color) => offsetPlayer.Add((start, end, color)));
		AssertTranslatedLines(baselinePlayer, offsetPlayer, offset);

		AssertTranslated(baselineRenderer.ProjectShot(
			new Shot { EdgeIndex = 3, Height = 100 }),
			offsetRenderer.ProjectShot(new Shot { EdgeIndex = 3, Height = 100 }),
			offset);
		AssertTranslated(
			baselineRenderer.ProjectEnemyBullet(new EnemyBullet(4, 100)),
			offsetRenderer.ProjectEnemyBullet(new EnemyBullet(4, 100)),
			offset);

		Flipper flipper = new(5, 120);
		(Vector2 baselineFlipperStart, Vector2 baselineFlipperEnd) =
			baselineRenderer.ProjectFlipperEndpoints(flipper);
		(Vector2 offsetFlipperStart, Vector2 offsetFlipperEnd) =
			offsetRenderer.ProjectFlipperEndpoints(flipper);
		AssertTranslated(baselineFlipperStart, offsetFlipperStart, offset);
		AssertTranslated(baselineFlipperEnd, offsetFlipperEnd, offset);

		List<(Vector2 Start, Vector2 End)> baselineSpikeLines = [];
		List<Vector2> baselineSpikeTips = [];
		List<(Vector2 Start, Vector2 End)> offsetSpikeLines = [];
		List<Vector2> offsetSpikeTips = [];
		baselineRenderer.DrawSpikes((start, end) =>
			baselineSpikeLines.Add((start, end)), baselineSpikeTips.Add);
		offsetRenderer.DrawSpikes((start, end) =>
			offsetSpikeLines.Add((start, end)), offsetSpikeTips.Add);
		Assert.Equal(baselineSpikeLines.Count, offsetSpikeLines.Count);
		for (int index = 0; index < baselineSpikeLines.Count; index++)
		{
			AssertTranslated(baselineSpikeLines[index].Start,
				offsetSpikeLines[index].Start, offset);
			AssertTranslated(baselineSpikeLines[index].End,
				offsetSpikeLines[index].End, offset);
		}
		Assert.Equal(baselineSpikeTips.Count, offsetSpikeTips.Count);
		for (int index = 0; index < baselineSpikeTips.Count; index++)
			AssertTranslated(baselineSpikeTips[index], offsetSpikeTips[index], offset);

		Assert.Contains(spike, game.Spikes);
		Assert.Equal(80, spike.Height);
		Assert.Equal(pointsBefore, game.CurrentLevel.Definition.Points);
		Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
		Assert.Equal(playerDepthBefore, game.Player.Depth);
		Assert.Equal(scoreBefore, game.Score.Score);
		Assert.Equal(stateBefore, game.State);
	}

	[Fact]
	public void OffsetTranslatesAfterExitZoomCompensationAndDuringLevel7Approach()
	{
		TempestGame exitGame = new(startingProductionLevel: 7);
		exitGame.CurrentLevel.State = LevelState.ExitZoom;
		AssertTranslatedTube(
			CaptureTube(new VectorRenderer(exitGame, atariPresentationEnabled: true,
				tubeVerticalOffset: 0)),
			CaptureTube(new VectorRenderer(exitGame, atariPresentationEnabled: true,
				tubeVerticalOffset: 73)),
			73);

		TempestGame approachGame = new(
			startingProductionLevel: 6, random: new Random(1981));
		foreach (Spike existingSpike in approachGame.CurrentLevel.Spikes.ToArray())
			approachGame.CurrentLevel.RemoveSpike(existingSpike);
		approachGame.CurrentLevel.State = LevelState.ExitZoom;
		approachGame.Player.AdvanceExit(389);
		approachGame.RunOneStep();
		Assert.Equal(7, approachGame.CurrentLevel.Configuration.DisplayedLevel);
		Assert.Equal(LevelState.Approach, approachGame.State);
		Assert.NotNull(approachGame.CurrentApproach);
		AssertTranslatedTube(
			CaptureTube(new VectorRenderer(approachGame,
				atariPresentationEnabled: true, tubeVerticalOffset: 0)),
			CaptureTube(new VectorRenderer(approachGame,
				atariPresentationEnabled: true, tubeVerticalOffset: -31)),
			-31);
	}

	[Fact]
	public void PlaybackSimulationIsEquivalentAcrossVerticalOffsets()
	{
		PlayerInputReplay replay = GameplayPlayback.CreateLevel1();
		GameplayStartupConfiguration startup = new(1, 3, 1.0, replay.RandomSeed);
		GameplayPlaybackSession baseline = new(startup, replay);
		GameplayPlaybackSession diagnostic = new(startup, replay);
		_ = CaptureTube(new VectorRenderer(
			baseline.Game, atariPresentationEnabled: true));
		_ = CaptureTube(new VectorRenderer(
			diagnostic.Game, atariPresentationEnabled: true,
			tubeVerticalOffset: 1e30f));

		GameplayStateSummary baselineSummary = baseline.RunToBoundary(120);
		GameplayStateSummary diagnosticSummary = diagnostic.RunToBoundary(120);

		Assert.Equal(baseline.CurrentBoundary, diagnostic.CurrentBoundary);
		Assert.Equal(baseline.RecordingDurationTicks, diagnostic.RecordingDurationTicks);
		Assert.Equal(JsonSerializer.Serialize(baselineSummary),
			JsonSerializer.Serialize(diagnosticSummary));
	}

	[Fact]
	public void RendererRejectsNonFiniteAndRequiresAtariPresentationForNonzeroOffset()
	{
		TempestGame game = new(startingProductionLevel: 1);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true,
				tubeVerticalOffset: float.NaN));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true,
				tubeVerticalOffset: float.PositiveInfinity));
		Assert.Throws<ArgumentException>(() =>
			new VectorRenderer(game, tubeVerticalOffset: 1));
		Assert.NotNull(new VectorRenderer(game, atariPresentationEnabled: true,
			tubeVerticalOffset: float.MaxValue));
	}

	private static string[] ArgsForLevel(int level, params string[] options) =>
	[
		"--atari-presentation",
		"--level",
		level.ToString(CultureInfo.InvariantCulture),
		.. options
	];

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		VectorRenderer renderer)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}

	private static void AssertTranslatedTube(
		IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> baseline,
		IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> translated,
		float offset)
	{
		Assert.Equal(baseline.Count, translated.Count);
		for (int index = 0; index < baseline.Count; index++)
		{
			Assert.Equal(baseline[index].Color, translated[index].Color);
			AssertTranslated(baseline[index].Start, translated[index].Start, offset);
			AssertTranslated(baseline[index].End, translated[index].End, offset);
			AssertVectorNear(
				baseline[index].End - baseline[index].Start,
				translated[index].End - translated[index].Start);
		}
	}

	private static void AssertTranslatedLines(
		IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> baseline,
		IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> translated,
		float offset)
	{
		Assert.Equal(baseline.Count, translated.Count);
		for (int index = 0; index < baseline.Count; index++)
		{
			Assert.Equal(baseline[index].Color, translated[index].Color);
			AssertTranslated(baseline[index].Start, translated[index].Start, offset);
			AssertTranslated(baseline[index].End, translated[index].End, offset);
		}
	}

	private static void AssertTranslated(Vector2 baseline, Vector2 translated, float offset)
	{
		Assert.InRange(MathF.Abs(translated.X - baseline.X), 0, .001f);
		Assert.InRange(MathF.Abs(translated.Y - baseline.Y - offset), 0, .001f);
	}

	private static void AssertVectorNear(Vector2 expected, Vector2 actual) =>
		Assert.InRange(Vector2.Distance(expected, actual), 0, .001f);
}
