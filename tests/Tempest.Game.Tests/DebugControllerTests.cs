using Tempest.Game.Debug;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class DebugControllerTests
{
    [Fact]
    public void StartingLevelIsOptionalAndIndependentOfLivesArgumentOrder()
    {
        Assert.True(DebugController.TryParseStartingLevel([], out int? omitted, out string? omittedError));
        Assert.Null(omitted);
        Assert.Null(omittedError);

        foreach (string[] arguments in new[]
        {
            new[] { "--level", "5", "--lives", "10" },
            new[] { "--lives", "10", "--level", "5" }
        })
        {
            Assert.True(DebugController.TryParseStartingLevel(
                arguments, out int? selected, out string? error));
            Assert.Equal(5, selected);
            Assert.Null(error);
            Assert.Equal(10, DebugController.GetStartingLives(arguments));
        }
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("10")]
    [InlineData("999")]
    [InlineData("not-a-number")]
    public void RejectsUnsupportedOrMalformedStartingLevels(string value)
    {
        Assert.False(DebugController.TryParseStartingLevel(
            ["--level", value], out int? selected, out string? error));
        Assert.Null(selected);
        Assert.Contains("implemented production level from 1 through 9", error);
    }

    [Fact]
    public void LevelEightIsAnImplementedProductionStart()
    {
        Assert.True(DebugController.TryParseStartingLevel(
            ["--level", "8"], out int? selected, out string? error));
        Assert.Equal(8, selected);
        Assert.Null(error);
    }

    [Fact]
    public void LevelNineIsAnImplementedProductionStart()
    {
        Assert.True(DebugController.TryParseStartingLevel(
            ["--level", "9"], out int? selected, out string? error));
        Assert.Equal(9, selected);
        Assert.Null(error);
    }

    [Fact]
    public void Level7SpikerSpeedOverrideIsOptionalAndAcceptsTheCalibrationValues()
    {
        Assert.True(DebugController.TryParseLevel7SpikerSpeedOverride(
            [], out float? omitted, out string? omittedError));
        Assert.Null(omitted);
        Assert.Null(omittedError);

        foreach (string value in new[] { "1.000", "1.500", "2.000", "2.500", "2.625", "3.000" })
        {
            Assert.True(DebugController.TryParseLevel7SpikerSpeedOverride(
                ["--level7-spiker-speed", value], out float? speed, out string? error));
            Assert.Equal(float.Parse(value, System.Globalization.CultureInfo.InvariantCulture), speed);
            Assert.Null(error);
        }
    }

    [Theory]
    [InlineData("--level7-spiker-speed")]
    [InlineData("--level7-spiker-speed", "--lives", "10")]
    [InlineData("--level7-spiker-speed", "0.999")]
    [InlineData("--level7-spiker-speed", "3.001")]
    [InlineData("--level7-spiker-speed", "NaN")]
    [InlineData("--level7-spiker-speed", "Infinity")]
    [InlineData("--level7-spiker-speed", "-Infinity")]
    [InlineData("--level7-spiker-speed", "not-a-number")]
    public void RejectsMalformedOrOutOfRangeLevel7SpikerSpeedOverrides(params string[] arguments)
    {
        Assert.False(DebugController.TryParseLevel7SpikerSpeedOverride(
            arguments, out float? speed, out string? error));
        Assert.Null(speed);
        Assert.Contains("finite value from 1.000 through 3.000", error);
    }

    [Fact]
    public void RejectsRepeatedLevel7SpikerSpeedOverrides()
    {
        Assert.False(DebugController.TryParseLevel7SpikerSpeedOverride(
            ["--level7-spiker-speed", "2.8", "--level7-spiker-speed", "2.9"],
            out float? speed, out string? error));
        Assert.Null(speed);
        Assert.Contains("may be specified only once", error);
    }

    [Fact]
    public void Level9SpikerSpeedOverrideIsOptionalAndAcceptsTheCalibrationValues()
    {
        Assert.True(DebugController.TryParseLevel9SpikerSpeedOverride(
            [], out float? omitted, out string? omittedError));
        Assert.Null(omitted);
        Assert.Null(omittedError);

        foreach (string value in new[] { "1.0", "1.75", "2.0", "2.25", "2.5", "2.75" })
        {
            Assert.True(DebugController.TryParseLevel9SpikerSpeedOverride(
                ["--level9-spiker-speed", value], out float? speed, out string? error));
            Assert.Equal(float.Parse(value, System.Globalization.CultureInfo.InvariantCulture), speed);
            Assert.Null(error);
        }
    }

    [Theory]
    [InlineData("--level9-spiker-speed")]
    [InlineData("--level9-spiker-speed", "--level", "9")]
    [InlineData("--level9-spiker-speed", "0")]
    [InlineData("--level9-spiker-speed", "-1")]
    [InlineData("--level9-spiker-speed", "0.999")]
    [InlineData("--level9-spiker-speed", "2.750001")]
    [InlineData("--level9-spiker-speed", "2.9999998")]
    [InlineData("--level9-spiker-speed", "3.0")]
    [InlineData("--level9-spiker-speed", "4")]
    [InlineData("--level9-spiker-speed", "NaN")]
    [InlineData("--level9-spiker-speed", "Infinity")]
    [InlineData("--level9-spiker-speed", "-Infinity")]
    [InlineData("--level9-spiker-speed", "not-a-number")]
    [InlineData("--level9-spiker-speed", "2,5")]
    public void RejectsMalformedOrOutOfRangeLevel9SpikerSpeedOverrides(params string[] arguments)
    {
        Assert.False(DebugController.TryParseLevel9SpikerSpeedOverride(
            arguments, out float? speed, out string? error));
        Assert.Null(speed);
        Assert.Contains("finite value from 1.000 through 2.750", error);
    }

    [Fact]
    public void RejectsRepeatedLevel9SpikerSpeedOverrides()
    {
        Assert.False(DebugController.TryParseLevel9SpikerSpeedOverride(
            ["--level9-spiker-speed", "2.5", "--level9-spiker-speed", "2.75"],
            out float? speed, out string? error));
        Assert.Null(speed);
        Assert.Contains("may be specified only once", error);
    }

    [Fact]
    public void Level9SpikerOverrideIsLevel9OnlyAndRestoredFromRecordingMetadata()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level9.json", "--level", "9", "--speed", "0.5",
                "--level9-spiker-speed", "2.5"],
            out GameplayFileCommand? recordCommand, out string? recordError));
        Assert.Null(recordError);
        Assert.Equal(9, recordCommand!.StartingDisplayedLevel);
        Assert.Equal(2.5f, recordCommand.Level9SpikerSpeedOverride);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level8.json", "--level", "8",
                "--level9-spiker-speed", "2.5"],
            out _, out string? wrongLevelError));
        Assert.Contains("only valid with --level 9", wrongLevelError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level1.json", "--level9-spiker-speed", "2.5"],
            out _, out string? missingLevelError));
        Assert.Contains("only valid with --level 9", missingLevelError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "level9.json", "--level9-spiker-speed", "2.5"],
            out _, out string? playbackError));
        Assert.Contains("restores Level 9 Spiker calibration", playbackError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level9.json", "--level", "9",
                "--level7-spiker-speed", "2.5", "--level9-spiker-speed", "2.5"],
            out _, out string? conflictingOverridesError));
        Assert.Contains("mutually exclusive", conflictingOverridesError);

        Assert.Throws<ArgumentException>(() => new GameplayStartupConfiguration(
            8, 10, 1.0, 1981, level9SpikerSpeedOverride: 2.5f));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameplayStartupConfiguration(
            9, 10, 1.0, 1981, level9SpikerSpeedOverride: 3.0f));
        Assert.Throws<ArgumentException>(() => new GameplayStartupConfiguration(
            9, 10, 1.0, 1981, level7SpikerSpeedOverride: 2.5f,
            level9SpikerSpeedOverride: 2.5f));
    }

    [Fact]
    public void RemovedLevel7SpikerTankerSpeedMatchOptionExplainsItIsNowTheDefault()
    {
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--level7-spiker-match-tanker-speed"],
            out GameplayFileCommand? command, out string? error));

        Assert.Null(command);
        Assert.Contains("no longer needed", error);
    }

    [Fact]
    public void StartupConfigurationRejectsConflictingLegacyMatchAndSpeedMetadata()
    {
        Assert.Throws<ArgumentException>(() => new GameplayStartupConfiguration(
            7, 10, 1.0, 1981, level7SpikerSpeedOverride: 2.0f,
            level7SpikerMatchesTankerSpeed: true));
    }

    [Fact]
    public void GameplayRecordingCapturesTheLevel7SpeedAndPlaybackRestoresIt()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "recording.json", "--level", "7",
                "--level7-spiker-speed", "2.0"],
            out GameplayFileCommand? recordCommand, out string? recordError));
        Assert.Null(recordError);
        Assert.Equal(7, recordCommand!.StartingDisplayedLevel);
        Assert.Equal(2f, recordCommand.Level7SpikerSpeedOverride);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "recording.json", "--level7-spiker-speed", "2.0"],
            out GameplayFileCommand? playbackCommand, out string? playbackError));
        Assert.Null(playbackCommand);
        Assert.Contains("restores Level 7 Spiker calibration", playbackError);
    }

    [Fact]
    public void RejectsMissingOrRepeatedStartingLevelValues()
    {
        foreach (string[] arguments in new[]
        {
            new[] { "--level" },
            new[] { "--level", "--lives", "10" },
            new[] { "--level", "4", "--level", "5" }
        })
        {
            Assert.False(DebugController.TryParseStartingLevel(
                arguments, out int? selected, out string? error));
            Assert.Null(selected);
            Assert.NotNull(error);
        }
    }

    [Fact]
    public void GenericDirectStartIsExclusiveWithLegacyDirectScenarios()
    {
        Assert.False(DebugController.HasConflictingScenarios(["--level", "5"]));
        Assert.False(DebugController.HasConflictingScenarios(["--level7-test"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--level", "5", "--level7-test"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--level", "5", "--gameplay-playback"]));
    }
}
