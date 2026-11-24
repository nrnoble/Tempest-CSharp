using System.Globalization;
using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class BackgroundOpacityOptionsTests
{
	[Fact]
	public void OmittedOptionDefaultsToOpaqueBlackWithoutRequestingFramebufferTransparency()
	{
		Assert.True(DebugController.TryParseBackgroundOpacityOptions(
			[], out var options, out var error));
		Assert.Null(error);
		Assert.Equal(1f, options.BackgroundOpacity);
		Assert.False(options.WasExplicitlySpecified);
		Assert.False(options.RequiresTransparentFramebuffer);
		Assert.Equal(Color.Black, options.BackgroundClearColor);
		options.ConfigureBeforeWindowInitialization(
			_ => throw new InvalidOperationException("Unexpected window configuration."));
	}

	[Theory]
	[InlineData("1", 1f, 255)]
	[InlineData("0.5", .5f, 127)]
	[InlineData("0", 0f, 0)]
	public void AcceptedValuesSetOnlyTheBackgroundAlpha(
		string value, float expected, byte expectedAlpha)
	{
		Assert.True(DebugController.TryParseBackgroundOpacityOptions(
			["--background-opacity", value], out var options, out var error), error);
		Assert.True(options.WasExplicitlySpecified);
		Assert.Equal(expected, options.BackgroundOpacity);
		Assert.Equal(new Color((byte)0, (byte)0, (byte)0, expectedAlpha),
			options.BackgroundClearColor);
		Assert.Equal(expected < 1f, options.RequiresTransparentFramebuffer);
	}

	[Fact]
	public void TransparentFramebufferFlagIsRequestedBeforeWindowInitializationOnlyWhenNeeded()
	{
		List<string> calls = [];
		BackgroundOpacityOptions options = new(.5f, true);
		options.ConfigureBeforeWindowInitialization(flags =>
		{
			Assert.Equal(ConfigFlags.TransparentWindow, flags);
			Assert.False(flags.HasFlag(ConfigFlags.MousePassthroughWindow));
			calls.Add("config");
		});
		calls.Add("init");
		Assert.Equal(["config", "init"], calls);

		calls.Clear();
		new BackgroundOpacityOptions(1f, true)
			.ConfigureBeforeWindowInitialization(_ => calls.Add("unexpected-config"));
		calls.Add("init");
		Assert.Equal(["init"], calls);
	}

	[Theory]
	[InlineData("-0.01")]
	[InlineData("1.00000001")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("1e1000")]
	[InlineData("invalid")]
	[InlineData("0,5")]
	[InlineData("")]
	public void RejectsInvalidValues(string value)
	{
		Assert.False(DebugController.TryParseBackgroundOpacityOptions(
			["--background-opacity", value], out _, out var error));
		Assert.Contains("between 0 and 1", error);
	}

	[Fact]
	public void RejectsMissingDuplicateAndJoinedOptions()
	{
		string[][] invalid =
		[
			["--background-opacity"],
			["--background-opacity", "--level", "14"],
			["--background-opacity", ".5", "--background-opacity", "1"],
			["--background-opacity=.5"]
		];
		foreach (string[] args in invalid)
			Assert.False(DebugController.TryParseBackgroundOpacityOptions(
				args, out _, out var error), error);
	}

	[Fact]
	public void UsesInvariantCulture()
	{
		CultureInfo previous = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
			Assert.True(DebugController.TryParseBackgroundOpacityOptions(
				["--background-opacity", "0.5"], out var options, out _));
			Assert.Equal(.5f, options.BackgroundOpacity);
			Assert.False(DebugController.TryParseBackgroundOpacityOptions(
				["--background-opacity", "0,5"], out _, out _));
		}
		finally { CultureInfo.CurrentCulture = previous; }
	}

	[Theory]
	[InlineData("--level")]
	[InlineData("--start-level")]
	public void ComposesWithStartupAndTubeCalibrationOptions(string selector)
	{
		string[] args =
		[
			selector, "14", "--lives", "7", "--atari-presentation", "--calibration-guides",
			"--tube-y-gain", "1.2", "--tube-scale", ".9",
			"--tube-vertical-offset", "7", "--tube-depth-y-gain", ".6",
			"--background-opacity", ".5"
		];
		Assert.True(DebugController.TryParseBackgroundOpacityOptions(args, out var opacity, out _));
		Assert.True(DebugController.TryParseStartingLevel(args, out var level, out _));
		Assert.Equal(14, level);
		Assert.True(DebugController.TryParseAtariPresentation(args, out var atari, out _));
		Assert.True(atari);
		Assert.True(DebugController.TryParseCalibrationGuides(args, out var guides, out _));
		Assert.True(guides);
		Assert.True(DebugController.TryParseTubeYGainOptions(args, out var y, out _));
		Assert.True(DebugController.TryParseTubeScaleOptions(args, out var scale, out _));
		Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(args, out var offset, out _));
		Assert.True(DebugController.TryParseTubeDepthYGainOptions(args, out var depth, out _));
		Assert.Equal((.5f, 1.2f, .9f, 7f, .6f),
			(opacity.BackgroundOpacity, y.Gain, scale.Scale, offset.Offset, depth.Gain));

		string[] withoutAtari = ["--level", "14", "--background-opacity", ".5"];
		Assert.True(DebugController.TryParseBackgroundOpacityOptions(withoutAtari, out _, out _));
	}

	[Fact]
	public void PlaybackAndRecordingAcceptBackgroundOpacityWithoutRecordingIt()
	{
		foreach (string option in new[] { "--record-gameplay", "--playback-file" })
		{
			string[] baseline = [option, "session.json"];
			string[] withOpacity = [option, "session.json", "--background-opacity", ".5"];
			Assert.True(DebugController.TryParseGameplayFileCommand(baseline, out var expected, out _));
			Assert.True(DebugController.TryParseGameplayFileCommand(
				withOpacity, out var actual, out var error), error);
			Assert.Equal(expected, actual);
		}
		Assert.False(DebugController.TryParseGameplayFileCommand(
			["--playback-file", "session.json", "--background-opacity", "2"], out _, out _));
	}

	[Fact]
	public void HeadlessPlaybackExplicitlyRejectsBackgroundOpacity()
	{
		string[] args =
			["--headless-playback", "session.json", "--background-opacity", ".5"];
		Assert.False(DebugController.TryParseBackgroundOpacityOptions(args, out _, out var error));
		Assert.Contains("headless", error);
		using StringWriter output = new();
		using StringWriter errors = new();
		Assert.Equal(HeadlessGameplayPlaybackCli.InvalidCommandExitCode,
			HeadlessGameplayPlaybackCli.Run(args, output, errors));
		Assert.Contains("background-opacity", errors.ToString());
	}

	[Fact]
	public void BackgroundOpacityDoesNotChangeTubeGeometryColorsOrGameplayState()
	{
		TempestGame game = new(startingProductionLevel: 14, random: new Random(1981));
		VectorRenderer opaque = new(game, atariPresentationEnabled: true);
		Assert.True(DebugController.TryParseBackgroundOpacityOptions(
			["--background-opacity", ".5"], out var options, out _));
		VectorRenderer translucent = new(
			game, atariPresentationEnabled: true,
			backgroundClearColor: options.BackgroundClearColor);
		var before = CaptureTube(opaque);
		var configuration = game.CurrentLevel.Configuration;
		var calibration = AtariPresentationCalibration.Resolve(14);
		int score = game.Score.Score;

		Assert.Equal(before, CaptureTube(translucent));
		Assert.All(CaptureTube(translucent), line => Assert.Equal(255, line.Color.A));
		Assert.Same(configuration, game.CurrentLevel.Configuration);
		Assert.Equal(calibration, AtariPresentationCalibration.Resolve(14));
		Assert.Equal(score, game.Score.Score);
	}

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		VectorRenderer renderer)
	{
		List<(Vector2, Vector2, Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}
}
