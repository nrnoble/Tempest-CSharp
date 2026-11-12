using Tempest.Game.Debug;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class InputDiagnosticTests
{
    [Fact]
    public void InputDiagnosticIsExplicitlyOptInAndCaseSensitive()
    {
        Assert.False(DebugController.UseInputDiagnostic([]));
        Assert.False(DebugController.UseInputDiagnostic(["--level3-geometry-test"]));
        Assert.False(DebugController.UseInputDiagnostic(["--INPUT-DIAGNOSTIC"]));
        Assert.False(DebugController.UseInputDiagnostic(["--input-diagnostic-extra"]));
        Assert.True(DebugController.UseInputDiagnostic(["--input-diagnostic"]));
    }

    [Fact]
    public void InputDiagnosticDoesNotChangeGameplayScenarioValidation()
    {
        Assert.False(DebugController.HasConflictingScenarios(["--input-diagnostic"]));
        Assert.False(DebugController.HasConflictingScenarios(["--input-diagnostic", "--level2-test"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--input-diagnostic", "--level2-test", "--level3-geometry-test"]));
    }

    [Fact]
    public void GameplayPlaybackIsOptInAndExclusiveWithOtherScenarios()
    {
        Assert.False(DebugController.UseGameplayPlayback([]));
        Assert.True(DebugController.UseGameplayPlayback(["--gameplay-playback"]));
        Assert.False(DebugController.HasConflictingScenarios(["--gameplay-playback"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--gameplay-playback", "--level2-test"]));
    }

    [Fact]
    public void LevelFourGameplayPlaybackSelectsAnExclusiveSeededScenario()
    {
        Assert.False(DebugController.UseLevel4GameplayPlayback([]));
        Assert.False(DebugController.UseLevel4GameplayPlayback(["--GAMEPLAY-PLAYBACK-LEVEL4"]));
        Assert.True(DebugController.UseLevel4GameplayPlayback(["--gameplay-playback-level4"]));
        Assert.False(DebugController.HasConflictingScenarios(["--gameplay-playback-level4"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--gameplay-playback-level4", "--gameplay-playback"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--gameplay-playback-level4", "--level4-test"]));
        Assert.False(DebugController.HasConflictingScenarios(
            ["--gameplay-playback-level4", "--lives", "10", "--enemy-bullets-test"]));
    }

    [Fact]
    public void GameplayFileCommandsRequireOneExplicitPathAndAreMutuallyExclusive()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "Temp\\GameplayRecordings\\session.json"],
            out GameplayFileCommand? recordCommand, out string? recordError));
        Assert.Equal(new GameplayFileCommand(GameplayFileMode.Record,
            "Temp\\GameplayRecordings\\session.json"), recordCommand);
        Assert.Null(recordError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "Temp\\GameplayRecordings\\session.json"],
            out GameplayFileCommand? playbackCommand, out string? playbackError));
        Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback,
            "Temp\\GameplayRecordings\\session.json"), playbackCommand);
        Assert.Null(playbackError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "Temp\\GameplayRecordings\\session.json",
                "--screenshot-key-diagnostic"],
            out GameplayFileCommand? diagnosticCommand, out string? diagnosticError));
        Assert.True(diagnosticCommand!.ScreenshotKeyDiagnostic);
        Assert.Null(diagnosticError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "session.json", "--screenshot-key-diagnostic",
                "--screenshot-key-diagnostic"], out _, out _));
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "session.json", "--screenshot-key-diagnostic"],
            out _, out string? recordDiagnosticError));
        Assert.Contains("only with --playback-file", recordDiagnosticError);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay"], out _, out string? missingPathError));
        Assert.Contains("requires an explicit file path", missingPathError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "--gameplay-playback"], out _, out _));
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "recording.json", "--playback-file", "playback.json"],
            out _, out string? exclusiveModeError));
        Assert.Contains("mutually exclusive", exclusiveModeError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "missing.json", "--lives", "10"], out _, out string? livesOverrideError));
        Assert.Contains("--lives", livesOverrideError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--gameplay-playback"], out GameplayFileCommand? noFileCommand,
            out string? noFileError));
        Assert.Null(noFileCommand);
        Assert.Null(noFileError);
        Assert.True(DebugController.UseGameplayPlayback(["--gameplay-playback"]));
        Assert.True(DebugController.UseLevel4GameplayPlayback(["--gameplay-playback-level4"]));
    }

    [Fact]
    public void RecordingAcceptsValidatedProductionOptionsInAnyTailOrder()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level5.json", "--lives", "10", "--rim-speed", "2.5",
                "--level", "5", "--game-over-density", "2"],
            out GameplayFileCommand? command, out string? error));

        Assert.Null(error);
        Assert.Equal(5, command!.StartingDisplayedLevel);
        Assert.Equal(10, command.StartingLives);
        Assert.Equal(2.5, command.RimSpeedMultiplier);
        Assert.Null(command.Level7SpikerSpeedOverride);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level7.json", "--level", "7", "--lives", "10",
                "--level7-spiker-speed", "2.0"],
            out GameplayFileCommand? level7Command, out string? level7Error));
        Assert.Null(level7Error);
        Assert.Equal(2f, level7Command!.Level7SpikerSpeedOverride);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--level", "7", "--lives", "10", "--speed", "0.5",
                "--record-gameplay", "Temp\\GameplayRecordings\\lvl7-SpikerTest.json",
                "--level7-spiker-speed", "2.0"],
            out GameplayFileCommand? requestedOrderCommand, out string? requestedOrderError));
        Assert.Null(requestedOrderError);
        Assert.Equal("Temp\\GameplayRecordings\\lvl7-SpikerTest.json",
            requestedOrderCommand!.Path);
        Assert.Equal(7, requestedOrderCommand.StartingDisplayedLevel);
        Assert.Equal(10, requestedOrderCommand.StartingLives);
        Assert.Equal(2f, requestedOrderCommand.Level7SpikerSpeedOverride);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "level1.json"], out GameplayFileCommand? defaults,
            out string? defaultsError));
        Assert.Null(defaultsError);
        Assert.Null(defaults!.StartingDisplayedLevel);
        Assert.Null(defaults.StartingLives);
        Assert.Null(defaults.RimSpeedMultiplier);
        Assert.Null(defaults.Level7SpikerSpeedOverride);
        GameplayStartupConfiguration defaultStartup = new(
            defaults.StartingDisplayedLevel ?? 1,
            defaults.StartingLives ?? 3,
            defaults.RimSpeedMultiplier ?? 1.0,
            1981,
            defaults.Level7SpikerSpeedOverride);
        Assert.Equal(new GameplayStartupConfiguration(1, 3, 1.0, 1981), defaultStartup);
    }

    [Fact]
    public void FileCommandsRejectDuplicateMissingMalformedAndUnknownOptions()
    {
        string[][] invalidCommands =
        [
            ["--record-gameplay", "session.json", "--level", "5", "--level", "6"],
            ["--record-gameplay", "session.json", "--lives", "10", "--lives", "8"],
            ["--record-gameplay", "session.json", "--rim-speed", "2", "--rim-speed", "3"],
            ["--record-gameplay", "session.json", "--level"],
            ["--record-gameplay", "session.json", "--lives", "0"],
            ["--record-gameplay", "session.json", "--rim-speed", "NaN"],
            ["--record-gameplay", "session.json", "--level", "10"],
            ["--record-gameplay", "session.json", "unexpected"],
            ["--record-gameplay", "session.json", "--unknown-option", "1"],
            ["unexpected", "--record-gameplay", "session.json"]
        ];

        foreach (string[] arguments in invalidCommands)
        {
            Assert.False(DebugController.TryParseGameplayFileCommand(
                arguments, out GameplayFileCommand? command, out string? error));
            Assert.Null(command);
            Assert.NotNull(error);
        }
    }

    [Fact]
    public void RecordingAndPlaybackRejectSpecialScenarioModes()
    {
        string[] scenarios =
        [
            "--level2-test", "--level3-test", "--level4-test", "--level6-test",
            "--level7-test", "--level3-geometry-test", "--spiker-test", "--tanker-test",
            "--fuseball-test", "--pulsar-test", "--enemy-bullets-test",
            "--input-diagnostic", "--gameplay-playback", "--gameplay-playback-level4"
        ];

        foreach (string scenario in scenarios)
        {
            Assert.False(DebugController.TryParseGameplayFileCommand(
                ["--record-gameplay", "session.json", scenario], out _, out _));
            Assert.False(DebugController.TryParseGameplayFileCommand(
                ["--playback-file", "session.json", scenario], out _, out _));
        }
    }

    [Fact]
    public void PlaybackRestoresSimulationIdentityAndAllowsOnlyObservationOptions()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "session.json", "--game-over-density", "0.5"],
            out GameplayFileCommand? command, out string? error));
        Assert.Null(error);
        Assert.Null(command!.StartingDisplayedLevel);
        Assert.Null(command.StartingLives);
        Assert.Null(command.RimSpeedMultiplier);

        foreach (string[] overrideArguments in new[]
        {
            new[] { "--level", "5" },
            new[] { "--lives", "10" },
            new[] { "--rim-speed", "2.5" }
        })
        {
            Assert.False(DebugController.TryParseGameplayFileCommand(
                ["--playback-file", "session.json", .. overrideArguments], out _, out _));
        }
    }
}
