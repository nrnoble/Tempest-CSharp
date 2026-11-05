using System.Globalization;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class RimSpeedDiagnosticTests
{
    [Fact]
    public void MissingOptionDefaultsToMultiplierOne()
    {
        bool success = DebugController.TryParseRimSpeedMultiplier(
            [], out double multiplier, out string? error);

        Assert.True(success);
        Assert.Equal(1.0, multiplier);
        Assert.Null(error);
    }

    [Theory]
    [InlineData("0.4", 0.4)]
    [InlineData("0.5", 0.5)]
    [InlineData("1", 1)]
    [InlineData("1.25", 1.25)]
    [InlineData("2", 2)]
    [InlineData("4", 4)]
    [InlineData("2.5", 2.5)]
    public void ParsesAndPropagatesPositiveFiniteMultipliers(string value, double expected)
    {
        bool success = DebugController.TryParseRimSpeedMultiplier(
            ["--rim-speed", value, "--lives", "10"],
            out double multiplier, out string? error);

        Assert.True(success);
        Assert.Equal(expected, multiplier);
        Assert.Null(error);

        TempestGame game = new(rimSpeedMultiplier: multiplier);
        Assert.Equal(expected, game.CurrentLevel.RimTurnSpeedMultiplier);
        Assert.Equal(8 * Level.RimTurnSpeedCalibrationMultiplier * expected,
            game.CurrentLevel.RimTurnSpeedDegrees);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    [InlineData("-Infinity")]
    [InlineData("not-a-number")]
    public void RejectsInvalidMultipliers(string value)
    {
        bool success = DebugController.TryParseRimSpeedMultiplier(
            ["--rim-speed", value], out _, out string? error);

        Assert.False(success);
        Assert.Contains("positive finite", error);
    }

    [Theory]
    [InlineData("--rim-speed")]
    [InlineData("--rim-speed", "--lives", "10")]
    public void RejectsMissingMultiplierValue(params string[] arguments)
    {
        bool success = DebugController.TryParseRimSpeedMultiplier(
            arguments, out _, out string? error);

        Assert.False(success);
        Assert.Contains("--rim-speed", error);
    }

    [Fact]
    public void RejectsRepeatedOption()
    {
        bool success = DebugController.TryParseRimSpeedMultiplier(
            ["--rim-speed", "2", "--rim-speed", "4"], out _, out string? error);

        Assert.False(success);
        Assert.Contains("only once", error);
    }

    [Fact]
    public void ParsesDecimalUsingInvariantCulture()
    {
        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            bool success = DebugController.TryParseRimSpeedMultiplier(
                ["--rim-speed", "2.5"], out double multiplier, out string? error);

            Assert.True(success);
            Assert.Equal(2.5, multiplier);
            Assert.Null(error);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void DefaultAndExplicitOnePreserveTheAcceptedConfigurationAndBehavior()
    {
        bool defaultParsed = DebugController.TryParseRimSpeedMultiplier(
            [], out double defaultMultiplier, out string? defaultError);
        bool explicitParsed = DebugController.TryParseRimSpeedMultiplier(
            ["--rim-speed", "1"], out double explicitMultiplier, out string? explicitError);
        Assert.True(defaultParsed);
        Assert.Null(defaultError);
        Assert.True(explicitParsed);
        Assert.Null(explicitError);

        TempestGame defaultGame = new(random: new Random(4225));
        TempestGame explicitBaseline = new(random: new Random(4225),
            rimSpeedMultiplier: explicitMultiplier);

        Assert.Same(LevelConfiguration.Level1, defaultGame.CurrentLevel.Configuration);
        Assert.Same(defaultGame.CurrentLevel.Configuration, explicitBaseline.CurrentLevel.Configuration);
        Assert.Equal(1, defaultMultiplier);
        Assert.Equal(1, explicitMultiplier);
        Assert.Equal(8 * Level.RimTurnSpeedCalibrationMultiplier,
            defaultGame.CurrentLevel.RimTurnSpeedDegrees);
        Assert.Equal(defaultGame.CurrentLevel.RimTurnSpeedDegrees,
            explicitBaseline.CurrentLevel.RimTurnSpeedDegrees);

        LevelConfiguration[] configurations =
        [
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production,
            LevelConfiguration.Level7Production
        ];
        foreach (LevelConfiguration configuration in configurations)
        {
            Level noArgument = new(configuration, rimTurnSpeedMultiplier: defaultMultiplier);
            Level explicitOne = new(configuration, rimTurnSpeedMultiplier: explicitMultiplier);
            Assert.Equal(noArgument.RimTurnSpeedDegrees, explicitOne.RimTurnSpeedDegrees);
        }

        for (int tick = 0; tick < 120; tick++)
        {
            defaultGame.RunOneStep();
            explicitBaseline.RunOneStep();
            Assert.Equal(defaultGame.State, explicitBaseline.State);
            Assert.Equal(defaultGame.Player.EdgeIndex, explicitBaseline.Player.EdgeIndex);
            Assert.Equal(defaultGame.Flippers.Select(flipper =>
                    (flipper.EdgeIndex, flipper.Height, flipper.State, flipper.TurnAngle)),
                explicitBaseline.Flippers.Select(flipper =>
                    (flipper.EdgeIndex, flipper.Height, flipper.State, flipper.TurnAngle)));
        }
    }

    [Fact]
    public void PointFourMultiplierReproducesFormerConfiguredRateAcrossLevels()
    {
        Assert.Equal(2.5, Level.RimTurnSpeedCalibrationMultiplier);
        LevelConfiguration[] configurations =
        [
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production,
            LevelConfiguration.Level7Production
        ];

        foreach (LevelConfiguration configuration in configurations)
        {
            Level calibrated = new(configuration);
            Level formerSpeed = new(configuration, rimTurnSpeedMultiplier: 0.4);
            double formerConfiguredRate = configuration.Flipper.TurnSpeedDegrees;

            Assert.Equal(
                formerConfiguredRate * Level.RimTurnSpeedCalibrationMultiplier,
                calibrated.RimTurnSpeedDegrees);
            Assert.InRange(
                Math.Abs(formerSpeed.RimTurnSpeedDegrees - formerConfiguredRate),
                0,
                1e-12);
        }
    }

    [Fact]
    public void MultiplierIsRetainedAcrossNaturalProgression()
    {
        TempestGame game = new(random: new Random(4225), rimSpeedMultiplier: 2.5);
        Level outgoing = game.CurrentLevel;
        for (int enemy = 0; enemy < outgoing.EnemySupply; enemy++)
            outgoing.RecordEnemyDestroyed();
        outgoing.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);

        for (int tick = 0; tick < 200 &&
             !ReferenceEquals(game.CurrentLevel.Configuration, LevelConfiguration.Level2Square); tick++)
            game.RunOneStep();

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(2.5, game.CurrentLevel.RimTurnSpeedMultiplier);
        Assert.Equal(8 * Level.RimTurnSpeedCalibrationMultiplier * 2.5,
            game.CurrentLevel.RimTurnSpeedDegrees);
    }

    [Fact]
    public void VeryLargeFiniteMultiplierCompletesTheCurrentPivotSafely()
    {
        string value = double.MaxValue.ToString("R", CultureInfo.InvariantCulture);
        bool parsed = DebugController.TryParseRimSpeedMultiplier(
            ["--rim-speed", value], out double multiplier, out string? error);
        Assert.True(parsed);
        Assert.Equal(double.MaxValue, multiplier);
        Assert.Null(error);

        Level level = new(LevelConfiguration.Level1,
            rimTurnSpeedMultiplier: multiplier);
        Flipper flipper = new(edge: 4, height: Level.Depth);
        int playerEdge = level.PreviousEdge(4);
        flipper.Update(level, playerEdge);
        for (int tick = 0; tick < level.Configuration.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge);

        flipper.Update(level, playerEdge);

        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(playerEdge, flipper.EdgeIndex);
        Assert.Equal(0, flipper.TurnAngle);
    }
}
