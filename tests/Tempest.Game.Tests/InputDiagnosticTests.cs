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
}
