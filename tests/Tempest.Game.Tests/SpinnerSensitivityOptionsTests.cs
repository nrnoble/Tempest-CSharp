using System.Globalization;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class SpinnerSensitivityOptionsTests
{
	[Fact]
	public void OmittedAndExplicitNormalSensitivityPreserveCurrentResponse()
	{
		Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
			[], out SpinnerSensitivityOptions omitted, out string? omittedError));
		Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
			["--spinner-sensitivity", "1"], out SpinnerSensitivityOptions explicitNormal,
			out string? explicitError));

		Assert.Null(omittedError);
		Assert.Null(explicitError);
		Assert.Equal(1.0f, omitted.Multiplier);
		Assert.False(omitted.WasExplicitlySpecified);
		Assert.True(explicitNormal.WasExplicitlySpecified);
		Assert.Equal(omitted.Multiplier, explicitNormal.Multiplier);
		Assert.Equal(
			InputSystem.ScaleMouseDeltaX(4.0f, omitted.Multiplier),
			InputSystem.ScaleMouseDeltaX(4.0f, explicitNormal.Multiplier));
	}

	[Theory]
	[InlineData("0.1")]
	[InlineData("0.25")]
	[InlineData("0.3")]
	[InlineData("0.4")]
	[InlineData("0.5")]
	[InlineData("0.75")]
	[InlineData("1")]
	[InlineData("1.0")]
	[InlineData("1.25")]
	[InlineData("2")]
	[InlineData("4")]
	[InlineData("4.0")]
	public void AcceptsInvariantMultipliersWithinInclusiveRange(string value)
	{
		Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
			["--spinner-sensitivity", value], out SpinnerSensitivityOptions options,
			out string? error));

		Assert.Null(error);
		Assert.True(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("0.75", 10.0f, 7.5f)]
	[InlineData("0.5", 10.0f, 5.0f)]
	[InlineData("0.4", 2.5f, 1.0f)]
	[InlineData("0.3", 1.0f, 0.3f)]
	[InlineData("1.25", 4.0f, 5.0f)]
	[InlineData("2.0", 4.0f, 8.0f)]
	[InlineData("4.0", 1.0f, 4.0f)]
	public void ScalesOnlyTheProvidedHorizontalDeltaAndRetainsFractions(
		string value, float rawDelta, float expectedDelta)
	{
		Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
			["--spinner-sensitivity", value], out SpinnerSensitivityOptions options, out _));

		Assert.Equal(expectedDelta,
			InputSystem.ScaleMouseDeltaX(rawDelta, options.Multiplier));
		Assert.Equal(-expectedDelta,
			InputSystem.ScaleMouseDeltaX(-rawDelta, options.Multiplier));
	}

	[Theory]
	[InlineData("0")]
	[InlineData("-1")]
	[InlineData("0.099")]
	[InlineData("4.001")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("not-a-number")]
	public void RejectsInvalidOrOutOfRangeMultipliers(string value)
	{
		Assert.False(DebugController.TryParseSpinnerSensitivityOptions(
			["--spinner-sensitivity", value], out _, out string? error));

		Assert.Contains("0.1 through 4.0", error);
	}

	[Fact]
	public void RejectsMissingAndDuplicateOptions()
	{
		foreach (string[] arguments in new[]
		{
			new[] { "--spinner-sensitivity" },
			new[] { "--spinner-sensitivity", "--lives", "10" },
			new[] { "--spinner-sensitivity", "1", "--spinner-sensitivity", "2" }
		})
		{
			Assert.False(DebugController.TryParseSpinnerSensitivityOptions(
				arguments, out _, out string? error));
			Assert.NotNull(error);
		}

		Assert.Contains("only once", GetError(
			["--spinner-sensitivity", "1", "--spinner-sensitivity", "2"]));
	}

	[Fact]
	public void ParsesUsingInvariantDecimalNotation()
	{
		CultureInfo originalCulture = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
			Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
				["--spinner-sensitivity", "0.75"],
				out SpinnerSensitivityOptions options, out string? error));

			Assert.Null(error);
			Assert.Equal(0.75f, options.Multiplier);
		}
		finally
		{
			CultureInfo.CurrentCulture = originalCulture;
		}
	}

	[Fact]
	public void RepeatedFractionalLiveSamplesRemainSubLaneMovement()
	{
		TempestGame scaledGame = new(random: new Random(4225));
		TempestGame directGame = new(random: new Random(4225));
		float startingPosition = scaledGame.Player.EdgePosition;

		for (int tick = 0; tick < 4; tick++)
		{
			float effectiveDelta = InputSystem.ScaleMouseDeltaX(1.0f, 0.5f);
			scaledGame.RunOneStep(new PlayerInput(false, effectiveDelta, false, false));
			directGame.RunOneStep(new PlayerInput(false, 0.5f, false, false));

			Assert.Equal(directGame.Player.EdgeIndex, scaledGame.Player.EdgeIndex);
			Assert.Equal(directGame.Player.EdgePosition, scaledGame.Player.EdgePosition);
		}

		Assert.Equal(7, scaledGame.Player.EdgeIndex);
		Assert.NotEqual(startingPosition, scaledGame.Player.EdgePosition);
	}

	[Theory]
	[InlineData("0.5", "0.5")]
	[InlineData("2", "0.5")]
	public void SpinnerSensitivityAndInteractiveSpeedParseIndependently(
		string speedValue, string sensitivityValue)
	{
		string[] arguments =
		[
			"--speed", speedValue,
			"--spinner-sensitivity", sensitivityValue
		];

		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			arguments, out InteractiveSpeedOptions speed, out string? speedError));
		Assert.True(DebugController.TryParseSpinnerSensitivityOptions(
			arguments, out SpinnerSensitivityOptions sensitivity,
			out string? sensitivityError));

		Assert.Null(speedError);
		Assert.Null(sensitivityError);
		Assert.Equal(double.Parse(speedValue, CultureInfo.InvariantCulture), speed.Multiplier);
		Assert.Equal(float.Parse(sensitivityValue, CultureInfo.InvariantCulture),
			sensitivity.Multiplier);
	}

	[Fact]
	public void LiveRecordingAllowsSensitivityButVisiblePlaybackRejectsIt()
	{
		string[] recordingArguments =
			["--record-gameplay", "session.json", "--spinner-sensitivity", "0.5"];
		Assert.True(DebugController.TryParseGameplayFileCommand(
			recordingArguments, out GameplayFileCommand? recordingCommand,
			out string? recordingError));
		Assert.Null(recordingError);
		Assert.Equal(GameplayFileMode.Record, recordingCommand!.Mode);

		string[] playbackArguments =
			["--playback-file", "session.json", "--spinner-sensitivity", "0.5"];
		Assert.False(DebugController.TryParseGameplayFileCommand(
			playbackArguments, out GameplayFileCommand? playbackCommand,
			out string? playbackError));
		Assert.Null(playbackCommand);
		Assert.Contains("live horizontal mouse/spinner input", playbackError);
	}

	private static string? GetError(string[] arguments)
	{
		DebugController.TryParseSpinnerSensitivityOptions(
			arguments, out _, out string? error);
		return error;
	}
}
