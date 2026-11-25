using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PortraitCalibrationGuideTests
{
	[Fact]
	public void GuidesAreOffByDefaultWithTheAcceptedAtariDefault()
	{
		Assert.True(DebugController.TryParseCalibrationGuides(
			["--level", "1"], out bool enabled, out string? error));
		Assert.False(enabled);
		Assert.Null(error);
		TempestGame game = new(startingProductionLevel: 1);
		foreach (VectorRenderer renderer in new[]
		{
			new VectorRenderer(game),
			new VectorRenderer(game, atariPresentationEnabled: true)
		})
		{
			List<(Vector2, Vector2, Color)> lines = [];
			renderer.DrawDevelopmentPortraitCalibrationGuides(
				(start, end, color) => lines.Add((start, end, color)));
			Assert.Empty(lines);
		}
	}

	[Fact]
	public void GuideOptionComposesWithDiagnosticsAndFileModes()
	{
		string[] arguments =
		[
			"--atari-presentation", "--level", "1", "--calibration-guides",
			"--tube-y-gain", "1.32", "--tube-scale", ".8",
			"--tube-vertical-offset", "-42.25"
		];
		Assert.True(DebugController.TryParseAtariPresentation(arguments, out bool atari, out _));
		Assert.True(DebugController.TryParseCalibrationGuides(arguments, out bool guides, out _));
		Assert.True(atari && guides);
		Assert.True(DebugController.TryParseTubeYGainOptions(arguments, out var gain, out _));
		Assert.True(DebugController.TryParseTubeScaleOptions(arguments, out var scale, out _));
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(arguments, out var offset, out _));
		Assert.Equal(1.32f, gain.Gain);
		Assert.Equal(.8f, scale.Scale);
		Assert.Equal(-42.25f, offset.Offset);

		foreach (string mode in new[] { "--record-gameplay", "--playback-file" })
		{
			string[] fileArguments =
				[mode, "session.json", "--atari-presentation", "--calibration-guides"];
			Assert.True(DebugController.TryParseCalibrationGuides(fileArguments, out guides, out _));
			Assert.True(guides);
			Assert.True(DebugController.TryParseGameplayFileCommand(
				fileArguments, out GameplayFileCommand? command, out string? error), error);
			Assert.NotNull(command);
		}
	}

	[Theory]
	[InlineData("--atari-presentation --calibration-guides --calibration-guides")]
	[InlineData("--atari-presentation --calibration-guides=value")]
	[InlineData("--atari-presentation --calibration-guides value")]
	[InlineData("--baseline-portrait-presentation --calibration-guides")]
	[InlineData("--legacy-presentation --calibration-guides")]
	public void InvalidGuideOptionsAreRejected(string command)
	{
		Assert.False(DebugController.TryParseCalibrationGuides(
			command.Split(' '), out _, out string? error));
		Assert.NotNull(error);
	}

	[Fact]
	public void RendererRejectsGuidesWithoutAtariPresentation()
	{
		Assert.Throws<ArgumentException>(() =>
			new VectorRenderer(new TempestGame(), calibrationGuidesEnabled: true));
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void GuideSubmissionIsUnchangedByLivePauseAndScreenshotRequest(bool enabled)
	{
		TempestGame game = new(startingProductionLevel: 1);
		VectorRenderer renderer = new(
			game, atariPresentationEnabled: true, calibrationGuidesEnabled: enabled);
		List<(Vector2, Vector2, Color)> before = [];
		renderer.DrawDevelopmentPortraitCalibrationGuides(
			(start, end, color) => before.Add((start, end, color)));
		LiveControlInput pause = InputSystem.CreateLiveControlInput(true, false);
		bool paused = InputSystem.ToggleLivePause(false, pause.PauseTogglePressed);
		LiveControlInput screenshot = InputSystem.CreateLiveControlInput(false, true);
		Assert.True(paused);
		Assert.True(screenshot.ScreenshotPressed);
		Assert.True(InputSystem.ToggleLivePause(paused, screenshot.PauseTogglePressed));
		List<(Vector2, Vector2, Color)> captured = [];
		renderer.DrawDevelopmentPortraitCalibrationGuides(
			(start, end, color) => captured.Add((start, end, color)));
		Assert.Equal(before, captured);
		Assert.Equal(enabled ? 2 : 0, captured.Count);
	}

	[Fact]
	public void DevelopmentGuidesStayAtViewportCoordinatesIndependentOfTubeDiagnostics()
	{
		TempestGame game = new(startingProductionLevel: 2, random: new Random(1));
		Level level = game.CurrentLevel;
		Vector2[] originalPoints = level.Definition.Points.ToArray();
		float originalPosition = level.Configuration.Position;
		float originalDepth = level.Configuration.Depth;
		LevelState originalState = game.State;
		int originalPlayerEdge = game.Player.EdgeIndex;
		float originalPlayerEdgePosition = game.Player.EdgePosition;
		float originalPlayerDepth = game.Player.Depth;
		int originalScore = game.Score.Score;
		int originalLives = game.Player.Lives;
		int originalShots = game.Shots.Count;
		int originalEnemies = game.Eggs.Count + game.Flippers.Count +
			game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count +
			game.Pulsars.Count + game.EnemyBullets.Count;

		VectorRenderer[] renderers =
		[
			new(game, atariPresentationEnabled: true, calibrationGuidesEnabled: true),
			new(game, atariPresentationEnabled: true, tubeYGain: 1.2f, calibrationGuidesEnabled: true),
			new(game, atariPresentationEnabled: true, tubeScale: .8f, calibrationGuidesEnabled: true),
			new(game, atariPresentationEnabled: true, tubeVerticalOffset: 42, calibrationGuidesEnabled: true),
			new(game, atariPresentationEnabled: true, tubeYGain: 1.32f,
				tubeScale: .8f, tubeVerticalOffset: -42.25f, calibrationGuidesEnabled: true)
		];
		float left = DisplayConfiguration.PortraitViewportLeft;
		float right = left + DisplayConfiguration.PortraitViewportWidth - 1;
		(Vector2 Start, Vector2 End, Color Color)[] expected =
		[
			(new Vector2(left, 180), new Vector2(right, 180), Color.Yellow),
			(new Vector2(left, 1210), new Vector2(right, 1210), Color.Yellow)
		];

		List<(Vector2 Start, Vector2 End, Color Color)>? baseline = null;
		foreach (VectorRenderer renderer in renderers)
		{
			List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
			renderer.DrawDevelopmentPortraitCalibrationGuides(
				(start, end, color) => lines.Add((start, end, color)));

			Assert.Equal(expected, lines);
			Assert.Equal(DisplayConfiguration.WindowWidth,
				DisplayConfiguration.PortraitViewportLeft * 2 +
				DisplayConfiguration.PortraitViewportWidth);
			if (baseline is null)
				baseline = lines;
			else
				Assert.Equal(baseline, lines);
		}

		Assert.Same(level, game.CurrentLevel);
		Assert.Equal(originalPoints, level.Definition.Points);
		Assert.Equal(originalPosition, level.Configuration.Position);
		Assert.Equal(originalDepth, level.Configuration.Depth);
		Assert.Equal(originalState, game.State);
		Assert.Equal(originalPlayerEdge, game.Player.EdgeIndex);
		Assert.Equal(originalPlayerEdgePosition, game.Player.EdgePosition);
		Assert.Equal(originalPlayerDepth, game.Player.Depth);
		Assert.Equal(originalScore, game.Score.Score);
		Assert.Equal(originalLives, game.Player.Lives);
		Assert.Equal(originalShots, game.Shots.Count);
		Assert.Equal(originalEnemies, game.Eggs.Count + game.Flippers.Count +
			game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count +
			game.Pulsars.Count + game.EnemyBullets.Count);
	}
}
