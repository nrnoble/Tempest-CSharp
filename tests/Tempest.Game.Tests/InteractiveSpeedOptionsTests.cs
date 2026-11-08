using System.Globalization;
using Tempest.Game.Debug;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class InteractiveSpeedOptionsTests
{
	[Fact]
	public void OmittedAndExplicitNormalSpeedHaveEquivalentOptionsAndRetainSixtyFps()
	{
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			[], out InteractiveSpeedOptions omitted, out string? omittedError));
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", "1"], out InteractiveSpeedOptions explicitNormal,
			out string? explicitError));

		Assert.Null(omittedError);
		Assert.Null(explicitError);
		Assert.Equal(InteractiveSpeedOptions.Normal, omitted);
		Assert.Equal(omitted, explicitNormal);
		Assert.Equal(1.0, omitted.Multiplier);
		Assert.Equal(60, omitted.TargetFramesPerSecond);
	}

	[Theory]
	[InlineData("0.5", 30)]
	[InlineData("0.9", 54)]
	[InlineData("0.95", 57)]
	[InlineData("1", 60)]
	[InlineData("1.0", 60)]
	[InlineData("2", 120)]
	[InlineData("4", 240)]
	public void ConvertsSpeedToNearestIntegerTargetFps(string text, int expectedFps)
	{
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", text], out InteractiveSpeedOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(expectedFps, options.TargetFramesPerSecond);
	}

	[Fact]
	public void TargetFpsMidpointsRoundAwayFromZero()
	{
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", "1.125"], out InteractiveSpeedOptions options, out _));

		Assert.Equal(68, options.TargetFramesPerSecond);
	}

	[Theory]
	[InlineData("0.25", 15)]
	[InlineData("4.0", 240)]
	public void AcceptsInclusiveSupportedBounds(string text, int expectedFps)
	{
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", text], out InteractiveSpeedOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(expectedFps, options.TargetFramesPerSecond);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("-1")]
	[InlineData("0.249")]
	[InlineData("4.001")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("not-a-number")]
	public void RejectsUnsupportedOrInvalidSpeedValues(string value)
	{
		Assert.False(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", value], out _, out string? error));

		Assert.Contains("0.25 through 4.0", error);
	}

	[Fact]
	public void ParsesSpeedUsingInvariantDecimalNotation()
	{
		CultureInfo originalCulture = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
			Assert.True(DebugController.TryParseInteractiveSpeedOptions(
				["--speed", "0.95"], out InteractiveSpeedOptions options,
				out string? error));

			Assert.Null(error);
			Assert.Equal(0.95, options.Multiplier);
			Assert.Equal(57, options.TargetFramesPerSecond);
		}
		finally
		{
			CultureInfo.CurrentCulture = originalCulture;
		}
	}

	[Fact]
	public void RejectsMissingOptionValueAndDuplicateSpeedOptions()
	{
		foreach (string[] arguments in new[]
		{
			new[] { "--speed" },
			new[] { "--speed", "--lives", "10" },
			new[] { "--speed", "1", "--speed", "2" }
		})
		{
			Assert.False(DebugController.TryParseInteractiveSpeedOptions(
				arguments, out _, out string? error));
			Assert.NotNull(error);
		}

		Assert.Contains("only once", GetError(["--speed", "1", "--speed", "2"]));
	}

	[Fact]
	public void FileModesAcceptSpeedWithoutAddingItToRecordingIdentity()
	{
		string[] recordingArguments =
			["--record-gameplay", "session.json", "--speed", "0.5", "--level", "5"];
		string[] playbackArguments =
			["--playback-file", "session.json", "--speed", "2"];

		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			recordingArguments, out InteractiveSpeedOptions recordingSpeed, out _));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			recordingArguments, out GameplayFileCommand? recordingCommand, out string? recordError));
		Assert.Null(recordError);
		Assert.Equal(30, recordingSpeed.TargetFramesPerSecond);
		Assert.Null(recordingCommand!.RimSpeedMultiplier);
		Assert.Equal(5, recordingCommand.StartingDisplayedLevel);

		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			playbackArguments, out InteractiveSpeedOptions playbackSpeed, out _));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			playbackArguments, out GameplayFileCommand? playbackCommand, out string? playbackError));
		Assert.Null(playbackError);
		Assert.Equal(120, playbackSpeed.TargetFramesPerSecond);
		Assert.Null(playbackCommand!.StartingDisplayedLevel);
		Assert.Null(playbackCommand.StartingLives);
		Assert.Null(playbackCommand.RimSpeedMultiplier);
	}

	[Fact]
	public void FileModesRejectInvalidOrRepeatedSpeedOptions()
	{
		foreach (string[] arguments in new[]
		{
			new[] { "--record-gameplay", "session.json", "--speed", "0" },
			new[] { "--playback-file", "session.json", "--speed", "5" },
			new[] { "--record-gameplay", "session.json", "--speed", "1",
				"--speed", "2" }
		})
		{
			Assert.False(DebugController.TryParseGameplayFileCommand(
				arguments, out GameplayFileCommand? command, out string? error));
			Assert.Null(command);
			Assert.NotNull(error);
		}
	}

	private static string? GetError(string[] arguments)
	{
		DebugController.TryParseInteractiveSpeedOptions(arguments, out _, out string? error);
		return error;
	}
}
