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
            ["--playback-file", "missing.json", "--lives", "10"], out _, out _));

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--gameplay-playback"], out GameplayFileCommand? noFileCommand,
            out string? noFileError));
        Assert.Null(noFileCommand);
        Assert.Null(noFileError);
        Assert.True(DebugController.UseGameplayPlayback(["--gameplay-playback"]));
        Assert.True(DebugController.UseLevel4GameplayPlayback(["--gameplay-playback-level4"]));
    }
}
