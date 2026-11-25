using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PresentationSelectionTests
{
	[Theory]
	[InlineData(PresentationSelectionKind.DefaultAtariPortrait)]
	[InlineData(PresentationSelectionKind.AtariCompatibilityAlias)]
	public void InteractiveDefaultAndCompatibilityAliasUseTheSameAtariProjection(
		PresentationSelectionKind selectedKind)
	{
		string[] arguments = selectedKind == PresentationSelectionKind.DefaultAtariPortrait
			? []
			: ["--atari-presentation"];
		Assert.True(DebugController.TryParsePresentationSelection(
			arguments, out PresentationSelection selected, out string? error));
		Assert.Null(error);
		Assert.Equal(selectedKind, selected.Kind);
		Assert.True(selected.UsesAtariPresentation);
		Assert.Equal(PresentationMode.ExperimentalPortrait, selected.RendererMode);

		AtariPresentationCalibration expected = AtariPresentationCalibration.ForDisplayedLevel(1);
		Assert.Equal(expected, AtariPresentationCalibration.Resolve(1));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(9)]
	[InlineData(13)]
	[InlineData(16)]
	public void DirectProductionStartsKeepAcceptedCalibrationAndProjection(
		int displayedLevel)
	{
		Assert.True(DebugController.TryParsePresentationSelection(
			[], out PresentationSelection defaultSelection, out _));
		Assert.True(DebugController.TryParsePresentationSelection(
			["--atari-presentation"], out PresentationSelection aliasSelection, out _));
		Assert.Equal(
			AtariPresentationCalibration.ForDisplayedLevel(displayedLevel),
			AtariPresentationCalibration.Resolve(displayedLevel));

		TempestGame game = new(startingProductionLevel: displayedLevel);
		Assert.Equal(
			CaptureTube(new VectorRenderer(
				game, atariPresentationEnabled: defaultSelection.UsesAtariPresentation)),
			CaptureTube(new VectorRenderer(
				game, atariPresentationEnabled: aliasSelection.UsesAtariPresentation)));
	}

	[Fact]
	public void BaselinePortraitSelectsTheOldProjectorAndKeepsPortraitViewport()
	{
		Assert.True(DebugController.TryParsePresentationSelection(
			["--baseline-portrait-presentation"],
			out PresentationSelection baseline, out string? error));
		Assert.Null(error);
		Assert.Equal(PresentationSelectionKind.BaselinePortraitDiagnostic, baseline.Kind);
		Assert.False(baseline.UsesAtariPresentation);
		Assert.Equal(PresentationMode.ExperimentalPortrait, baseline.RendererMode);
		Assert.Equal(960, DisplayConfiguration.PortraitViewportWidth);
		Assert.Equal(1280, DisplayConfiguration.PortraitViewportHeight);

		TempestGame game = new(startingProductionLevel: 1);
		List<(Vector2 Start, Vector2 End, Color Color)> baselineLines =
			CaptureTube(new VectorRenderer(game,
				atariPresentationEnabled: baseline.UsesAtariPresentation));
		Assert.True(DebugController.TryParsePresentationSelection(
			[], out PresentationSelection production, out _));
		List<(Vector2 Start, Vector2 End, Color Color)> productionLines =
			CaptureTube(new VectorRenderer(game,
				atariPresentationEnabled: production.UsesAtariPresentation));
		Assert.NotEqual(productionLines, baselineLines);
	}

	[Fact]
	public void LegacyPresentationRemainsTheDistinctFourThreeMode()
	{
		Assert.True(DebugController.TryParsePresentationSelection(
			["--legacy-presentation"],
			out PresentationSelection legacy, out string? error));
		Assert.Null(error);
		Assert.Equal(PresentationSelectionKind.LegacyFourThree, legacy.Kind);
		Assert.False(legacy.UsesAtariPresentation);
		Assert.Equal(PresentationMode.Legacy, legacy.RendererMode);
		Assert.Equal(DisplayConfiguration.WindowScale,
			DisplayConfiguration.GetPlayfieldCamera(legacy.RendererMode).Zoom);
		Assert.NotEqual(DisplayConfiguration.PortraitPlayfieldZoom,
			DisplayConfiguration.GetPlayfieldCamera(legacy.RendererMode).Zoom);
	}

	[Fact]
	public void PerspectiveIsRestrictedToTheExplicitBaselinePortraitDiagnostic()
	{
		Assert.False(DebugController.TryParsePresentationSelection(
			["--portrait-perspective", "2"], out _, out string? missingBaselineError));
		Assert.Contains("--baseline-portrait-presentation", missingBaselineError);

		string[] baselineArguments =
			["--baseline-portrait-presentation", "--portrait-perspective", "2"];
		Assert.True(DebugController.TryParsePresentationSelection(
			baselineArguments, out PresentationSelection baseline, out string? selectionError));
		Assert.Null(selectionError);
		Assert.Equal(PresentationSelectionKind.BaselinePortraitDiagnostic, baseline.Kind);
		Assert.True(DebugController.TryParsePortraitPerspective(
			baselineArguments, out float? perspective, out string? perspectiveError));
		Assert.Equal(2f, perspective);
		Assert.Null(perspectiveError);
	}

	[Fact]
	public void PresentationOptionsRejectMalformedDuplicatesAndConflicts()
	{
		string[][] invalid =
		[
			["--baseline-portrait-presentation", "--baseline-portrait-presentation"],
			["--baseline-portrait-presentation", "unexpected-value"],
			["--baseline-portrait-presentation=value"],
			["--atari-presentation", "--baseline-portrait-presentation"],
			["--baseline-portrait-presentation", "--legacy-presentation"],
			["--atari-presentation=value"]
		];

		foreach (string[] arguments in invalid)
			Assert.False(DebugController.TryParsePresentationSelection(
				arguments, out _, out _), string.Join(' ', arguments));
	}

	[Fact]
	public void NonRenderingRoutesDoNotAcquireTheInteractiveAtariDefault()
	{
		Assert.True(DebugController.TryParsePresentationSelection(
			["--headless-playback", "session.json"],
			out PresentationSelection headless, out string? headlessError));
		Assert.Null(headlessError);
		Assert.Equal(PresentationSelectionKind.HeadlessPlayback, headless.Kind);
		Assert.False(headless.UsesAtariPresentation);

		Assert.True(DebugController.TryParsePresentationSelection(
			["--input-diagnostic"],
			out PresentationSelection inputDiagnostic, out string? inputError));
		Assert.Null(inputError);
		Assert.Equal(PresentationSelectionKind.InputDiagnostic, inputDiagnostic.Kind);
		Assert.False(inputDiagnostic.UsesAtariPresentation);

		foreach (string[] arguments in new[]
		{
			new[] { "--headless-playback", "session.json", "--atari-presentation" },
			new[] { "--headless-playback", "session.json",
				"--baseline-portrait-presentation" },
			new[] { "--input-diagnostic", "--atari-presentation" },
			new[] { "--input-diagnostic", "--baseline-portrait-presentation" }
		})
			Assert.False(DebugController.TryParsePresentationSelection(
				arguments, out _, out _));
	}

	[Fact]
	public void CalibrationControlsUseDefaultAtariButRejectOtherPresentations()
	{
		string[] defaultCalibration =
			["--level", "13", "--tube-y-gain", "1.1"];
		Assert.True(DebugController.TryParseTubeYGainOptions(
			defaultCalibration, out TubeYGainOptions tubeGain, out string? tubeError));
		Assert.Null(tubeError);
		Assert.Equal(1.1f, tubeGain.Gain);
		Assert.True(DebugController.TryParseTubeDepthYGainOptions(
			["--level", "13", "--tube-depth-y-gain", ".5"], out _, out _));
		Assert.True(DebugController.TryParseTubeScaleOptions(
			["--level", "13", "--tube-scale", "1.1"], out _, out _));
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
			["--level", "13", "--tube-vertical-offset", "10"], out _, out _));

		Assert.True(DebugController.TryParseCalibrationGuides(
			["--calibration-guides"], out bool guides, out string? guideError));
		Assert.True(guides);
		Assert.Null(guideError);

		Assert.True(DebugController.TryParseAtariHudCalibrationOptions(
			["--hud-score-x", "250"], out AtariHudCalibrationOptions hud,
			out string? hudError));
		Assert.Equal(250f, hud.ResolvedScoreRight);
		Assert.Null(hudError);

		foreach (string[] incompatible in new[]
		{
			new[] { "--baseline-portrait-presentation", "--level", "13",
				"--tube-y-gain", "1.1" },
			new[] { "--legacy-presentation", "--level", "13",
				"--tube-y-gain", "1.1" },
			new[] { "--baseline-portrait-presentation", "--calibration-guides" },
			new[] { "--legacy-presentation", "--hud-score-x", "250" }
		})
		{
			if (incompatible.Contains("--tube-y-gain", StringComparer.Ordinal))
				Assert.False(DebugController.TryParseTubeYGainOptions(
					incompatible, out _, out _));
			if (incompatible.Contains("--calibration-guides", StringComparer.Ordinal))
				Assert.False(DebugController.TryParseCalibrationGuides(
					incompatible, out _, out _));
			if (incompatible.Contains("--hud-score-x", StringComparer.Ordinal))
				Assert.False(DebugController.TryParseAtariHudCalibrationOptions(
					incompatible, out _, out _));
		}

		foreach (string[] mode in new[]
		{
			new[] { "--baseline-portrait-presentation" },
			new[] { "--legacy-presentation" }
		})
		{
			Assert.False(DebugController.TryParseTubeYGainOptions(
				[.. mode, "--level", "13", "--tube-y-gain", "1.1"], out _, out _));
			Assert.False(DebugController.TryParseTubeDepthYGainOptions(
				[.. mode, "--level", "13", "--tube-depth-y-gain", ".5"], out _, out _));
			Assert.False(DebugController.TryParseTubeScaleOptions(
				[.. mode, "--level", "13", "--tube-scale", "1.1"], out _, out _));
			Assert.False(DebugController.TryParseTubeVerticalOffsetOptions(
				[.. mode, "--level", "13", "--tube-vertical-offset", "10"], out _, out _));
		}
	}

	[Fact]
	public void VisibleFileModesUseDefaultAtariAndKeepPresentationOutOfRecordingMetadata()
	{
		string[] defaultPlaybackArguments = ["--playback-file", "session.json"];
		string[] aliasPlaybackArguments =
			["--playback-file", "session.json", "--atari-presentation"];
		Assert.True(DebugController.TryParsePresentationSelection(
			defaultPlaybackArguments, out PresentationSelection defaultSelection, out _));
		Assert.True(DebugController.TryParsePresentationSelection(
			aliasPlaybackArguments, out PresentationSelection aliasSelection, out _));
		Assert.True(defaultSelection.UsesAtariPresentation);
		Assert.True(aliasSelection.UsesAtariPresentation);
		Assert.Equal(PresentationMode.ExperimentalPortrait, defaultSelection.RendererMode);

		Assert.True(DebugController.TryParseGameplayFileCommand(
			defaultPlaybackArguments, out GameplayFileCommand? defaultCommand, out _));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			aliasPlaybackArguments, out GameplayFileCommand? aliasCommand, out _));
		Assert.Equal(defaultCommand, aliasCommand);
		Assert.DoesNotContain("Presentation", JsonSerializer.Serialize(defaultCommand));

		string[] playbackArguments =
		[
			"--playback-file", "session.json", "--baseline-portrait-presentation",
			"--portrait-perspective", "2"
		];
		Assert.True(DebugController.TryParseGameplayFileCommand(
			playbackArguments, out GameplayFileCommand? playback, out string? playbackError));
		Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"), playback);
		Assert.Null(playbackError);
		Assert.DoesNotContain("Presentation", System.Text.Json.JsonSerializer.Serialize(playback));

		Assert.False(DebugController.TryParseGameplayFileCommand(
			["--playback-file", "session.json", "--legacy-presentation"],
			out _, out _));
		Assert.False(DebugController.TryParseGameplayFileCommand(
			["--headless-playback", "session.json", "--baseline-portrait-presentation"],
			out _, out _));
	}

	[Fact]
	public void PresentationChoiceDoesNotChangeRecordedPlaybackSimulation()
	{
		GameplayStartupConfiguration startup = new(5, 3, 1, 9127);
		PlayerInputRecorder recorder = new(startup);
		for (int tick = 0; tick < 30; tick++)
			recorder.CaptureTick(tick,
				new PlayerInput(tick % 5 == 0, tick % 3 == 0 ? .25f : 0,
					false, tick == 10, ZapPressed: tick == 20));
		PlayerInputReplay replay = recorder.CreateReplay();

		Assert.True(DebugController.TryParsePresentationSelection(
			[], out PresentationSelection defaultSelection, out _));
		Assert.True(DebugController.TryParsePresentationSelection(
			["--atari-presentation"], out PresentationSelection aliasSelection, out _));
		Assert.True(defaultSelection.UsesAtariPresentation);
		Assert.True(aliasSelection.UsesAtariPresentation);

		GameplayPlaybackSession defaultPlayback = new(startup, replay);
		GameplayPlaybackSession aliasPlayback = new(startup, replay);
		defaultPlayback.RunToBoundary(30);
		aliasPlayback.RunToBoundary(30);
		Assert.Equal(JsonSerializer.Serialize(defaultPlayback.GetStateSummary()),
			JsonSerializer.Serialize(aliasPlayback.GetStateSummary()));
	}

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		VectorRenderer renderer)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}
}
