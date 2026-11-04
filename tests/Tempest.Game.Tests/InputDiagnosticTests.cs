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
}
