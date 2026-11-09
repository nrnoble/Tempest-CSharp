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
    [InlineData("9")]
    [InlineData("999")]
    [InlineData("not-a-number")]
    public void RejectsUnsupportedOrMalformedStartingLevels(string value)
    {
        Assert.False(DebugController.TryParseStartingLevel(
            ["--level", value], out int? selected, out string? error));
        Assert.Null(selected);
        Assert.Contains("implemented production level from 1 through 8", error);
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
