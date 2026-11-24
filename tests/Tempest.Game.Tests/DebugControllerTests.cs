using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
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
    [InlineData("-100", -100f)]
    [InlineData("-50", -50f)]
    [InlineData("-20", -20f)]
    [InlineData("0", 0f)]
    [InlineData("+20", 20f)]
    [InlineData("+50", 50f)]
    [InlineData("+100", 100f)]
    public void PortraitPerspectiveAcceptsFiniteInvariantSignedValues(
        string value, float expected)
    {
        Assert.True(DebugController.TryParsePortraitPerspective(
            ["--portrait-perspective", value], out float? actual, out string? error));

        Assert.Equal(expected, actual);
        Assert.Null(error);
    }

    [Fact]
    public void PortraitPerspectiveUsesInvariantCultureAndRejectsInvalidOrRepeatedValues()
    {
        System.Globalization.CultureInfo originalCulture =
            System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture =
                System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
            Assert.True(DebugController.TryParsePortraitPerspective(
                ["--portrait-perspective", "1.25"], out float? parsed, out _));
            Assert.Equal(1.25f, parsed);
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = originalCulture;
        }

        string[][] invalidArguments =
        [
            ["--portrait-perspective"],
            ["--portrait-perspective", "--level", "9"],
            ["--portrait-perspective", ""],
            ["--portrait-perspective", "not-a-number"],
            ["--portrait-perspective", "1,25"],
            ["--portrait-perspective", "NaN"],
            ["--portrait-perspective", "Infinity"],
            ["--portrait-perspective", "-Infinity"],
            ["--portrait-perspective", "+Infinity"],
            ["--portrait-perspective", "1e100"],
            ["--portrait-perspective", "-1e100"],
            ["--portrait-perspective", "1", "--portrait-perspective", "2"]
        ];

        foreach (string[] arguments in invalidArguments)
        {
            Assert.False(DebugController.TryParsePortraitPerspective(
                arguments, out float? parsed, out string? error));
            Assert.Null(parsed);
            Assert.NotNull(error);
        }
    }

    [Fact]
    public void PortraitPerspectiveAcceptsTheFiniteSinglePrecisionLimits()
    {
        float originalPerspective = Tempest.Game.Rendering.Projection.PortraitPerspective;
        try
        {
            foreach (float expected in new[] { float.MinValue, float.MaxValue })
            {
                string value = expected.ToString("R",
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.True(DebugController.TryParsePortraitPerspective(
                    ["--portrait-perspective", value], out float? parsed, out string? error));
                Assert.Equal(expected, parsed);
                Assert.Null(error);
                Tempest.Game.Rendering.Projection.PortraitPerspective = parsed!.Value;
            }
        }
        finally
        {
            Tempest.Game.Rendering.Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void PortraitPerspectiveComposesWithRecordingAndPlaybackWithoutBecomingStartupMetadata()
    {
        string[] recordingArguments =
        [
            "--record-gameplay", "session.json", "--level", "9", "--lives", "8",
            "--speed", "0.5", "--rim-speed", "1.5", "--portrait-perspective", "+50"
        ];
        Assert.True(DebugController.TryParseGameplayFileCommand(
            recordingArguments, out GameplayFileCommand? recording, out string? recordingError));
        Assert.Null(recordingError);
        Assert.Equal(9, recording!.StartingDisplayedLevel);
        Assert.Equal(8, recording.StartingLives);
        Assert.Equal(1.5, recording.RimSpeedMultiplier);
        Assert.True(DebugController.TryParsePortraitPerspective(
            recordingArguments, out float? recordingPerspective, out string? recordingPerspectiveError));
        Assert.Equal(50f, recordingPerspective);
        Assert.Null(recordingPerspectiveError);

        Assert.True(DebugController.TryParseStartingLevel(
            ["--level", "9", "--portrait-perspective", "+50"],
            out int? startingLevel, out string? levelError));
        Assert.Equal(9, startingLevel);
        Assert.Null(levelError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "session.json", "--speed", "1.5",
                "--portrait-perspective", "+50"],
            out GameplayFileCommand? playback, out string? playbackError));
        Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"), playback);
        Assert.Null(playbackError);
        Assert.True(DebugController.TryParsePortraitPerspective(
            ["--playback-file", "session.json", "--speed", "1.5",
                "--portrait-perspective", "+50"],
            out float? playbackPerspective, out string? playbackPerspectiveError));
        Assert.Equal(50f, playbackPerspective);
        Assert.Null(playbackPerspectiveError);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "session.json", "--legacy-presentation",
                "--portrait-perspective", "0"],
            out _, out string? conflictError));
        Assert.Contains("cannot be combined with --legacy-presentation", conflictError);
    }

    [Fact]
    public void AtariPresentationIsExplicitOptInAndRejectsDuplicateMalformedOrIncompatibleOptions()
    {
        Assert.True(DebugController.TryParseAtariPresentation(
            [], out bool omitted, out string? omittedError));
        Assert.False(omitted);
        Assert.Null(omittedError);

        Assert.True(DebugController.TryParseAtariPresentation(
            ["--atari-presentation"], out bool enabled, out string? enabledError));
        Assert.True(enabled);
        Assert.Null(enabledError);

        string[][] invalidArguments =
        [
            ["--atari-presentation", "--atari-presentation"],
            ["--atari-presentation", "unexpected-value"],
            ["--atari-presentation", "--legacy-presentation"],
            ["--atari-presentation", "--portrait-perspective", "0"]
        ];
        foreach (string[] arguments in invalidArguments)
        {
            Assert.False(DebugController.TryParseAtariPresentation(
                arguments, out bool parsed, out string? error));
            Assert.False(parsed);
            Assert.NotNull(error);
        }

    }

    [Theory]
    [InlineData("--atari-tube-prototype")]
    [InlineData("--atari-tube-rn000f-diagnostic")]
    [InlineData("--atari-tube-reference-shape-diagnostic")]
    [InlineData("--level7-visible-runtime-diagnostic")]
    [InlineData("--level7-framebuffer-diagnostic")]
    public void RemovedLevel7PresentationOptionsAreRejectedInsteadOfIgnored(string option)
    {
        Assert.False(DebugController.TryParseAtariPresentation(
            [option], out bool enabled, out string? error));
        Assert.False(enabled);
        Assert.Contains(option, error);
        Assert.Contains("was removed", error);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "recording.json", option],
            out _, out string? playbackError));
        Assert.Contains(option, playbackError);
    }

    [Fact]
    public void AtariPresentationComposesWithInteractiveFileRecordingAndPlaybackOnly()
    {
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "session.json", "--level", "7", "--atari-presentation"],
            out GameplayFileCommand? recording, out string? recordingError));
        Assert.Equal(
            new GameplayFileCommand(GameplayFileMode.Record, "session.json", 7),
            recording);
        Assert.Null(recordingError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--atari-presentation", "--playback-file", "session.json"],
            out GameplayFileCommand? playback, out string? playbackError));
        Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"), playback);
        Assert.Null(playbackError);
    }

    [Fact]
    public void AtariPresentationIsRejectedForNonRenderingModes()
    {
        Assert.True(DebugController.TryValidateAtariPresentationContext(
            false, headlessPlaybackAnalysis: true, inputDiagnostic: true, out _));
        Assert.False(DebugController.TryValidateAtariPresentationContext(
            true, headlessPlaybackAnalysis: true, inputDiagnostic: false, out string? headlessError));
        Assert.Contains("headless playback analysis", headlessError);
        Assert.False(DebugController.TryValidateAtariPresentationContext(
            true, headlessPlaybackAnalysis: false, inputDiagnostic: true, out string? diagnosticError));
        Assert.Contains("--input-diagnostic", diagnosticError);
        Assert.True(DebugController.TryValidateAtariPresentationContext(
            true, headlessPlaybackAnalysis: false, inputDiagnostic: false, out string? interactiveError));
        Assert.Null(interactiveError);
    }

    [Fact]
    public void RemovedBlasterBrightnessOptionUsesNormalUnknownGameplayFileOptionBehavior()
    {
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "session.json", "--blaster-brightness", "1.5"],
            out GameplayFileCommand? command, out string? error));

        Assert.Null(command);
        Assert.Equal(
            "Unknown gameplay file option or argument '--blaster-brightness'.",
            error);
    }

    [Fact]
    public void PortraitPerspectiveDoesNotConflictWithSupportedVisiblePlaybackOrScenarios()
    {
        string[] compatibleModes =
        [
            "--gameplay-playback",
            "--gameplay-playback-level4",
            "--level2-test",
            "--level6-test",
            "--spiker-test"
        ];

        foreach (string mode in compatibleModes)
        {
            Assert.True(DebugController.TryParsePortraitPerspective(
                [mode, "--portrait-perspective", "2"], out float? perspective,
                out string? error));
            Assert.Equal(2f, perspective);
            Assert.Null(error);
            Assert.False(DebugController.HasConflictingScenarios(
                [mode, "--portrait-perspective", "2"]));
        }
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("999")]
    [InlineData("not-a-number")]
    public void RejectsUnsupportedOrMalformedStartingLevels(string value)
    {
        Assert.False(DebugController.TryParseStartingLevel(
            ["--level", value], out int? selected, out string? error));
        Assert.Null(selected);
        Assert.Contains("implemented production campaign level (currently ending at Level 12)", error);
        Assert.Contains("direct-only Level 15", error);
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
    public void LevelFifteenIsAcceptedOnlyAsADirectTubeCandidate()
    {
        Assert.True(DebugController.TryParseStartingLevel(
            ["--level", "15"], out int? selected, out string? error));
        Assert.Equal(15, selected);
        Assert.Null(error);
        Assert.Null(LevelProgression.GetProductionLevel(15));
        Assert.Same(LevelConfiguration.Level15DirectStartCandidate,
            LevelProgression.GetDirectStartLevel(15));
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

    [Fact]
    public void AtariHudCalibrationOmittedPreservesDefaults()
    {
        Assert.True(DebugController.TryParseAtariHudCalibrationOptions(
            ["--atari-presentation"],
            out AtariHudCalibrationOptions options,
            out string? error));
        Assert.Null(error);
        Assert.False(options.HasExplicitOverrides);
        Assert.Equal(AtariHudVector.ScoreRight, options.ResolvedScoreRight);
        Assert.Equal(AtariHudVector.HighScoreStartX, options.ResolvedHighScoreStartX);
        Assert.Equal(AtariHudVector.LevelStartX, options.ResolvedLevelStartX);
        Assert.Equal(AtariHudVector.ReserveLifeAdvance, options.ResolvedReserveLifeAdvance);
    }

    [Theory]
    [InlineData("--hud-score-x")]
    [InlineData("--hud-score-y")]
    [InlineData("--hud-high-x")]
    [InlineData("--hud-high-y")]
    [InlineData("--hud-level-x")]
    [InlineData("--hud-level-y")]
    [InlineData("--hud-lives-x")]
    [InlineData("--hud-lives-y")]
    [InlineData("--hud-lives-advance")]
    public void AtariHudCoordinateOptionsAcceptSignedFiniteValues(string option)
    {
        foreach (string value in new[] { "-1000", "-1.25", "0", "+1.25", "1000" })
        {
            Assert.True(DebugController.TryParseAtariHudCalibrationOptions(
                ["--atari-presentation", option, value],
                out _, out string? error));
            Assert.Null(error);
        }
    }

    [Theory]
    [InlineData("--hud-score-scale")]
    [InlineData("--hud-high-scale")]
    [InlineData("--hud-level-scale")]
    [InlineData("--hud-lives-scale")]
    public void AtariHudScaleOptionsAcceptPositiveFiniteValues(string option)
    {
        foreach (string value in new[] { "0.0001", "1", "1.25", "100000" })
        {
            Assert.True(DebugController.TryParseAtariHudCalibrationOptions(
                ["--atari-presentation", option, value],
                out _, out string? error));
            Assert.Null(error);
        }
    }

    [Fact]
    public void AtariHudCalibrationRejectsMalformedNonFiniteZeroAndDuplicateValues()
    {
        foreach (string[] arguments in new[]
        {
            new[] { "--atari-presentation", "--hud-score-x" },
            new[] { "--atari-presentation", "--hud-score-x", "bad" },
            new[] { "--atari-presentation", "--hud-score-y", "NaN" },
            new[] { "--atari-presentation", "--hud-score-scale", "0" },
            new[] { "--atari-presentation", "--hud-high-scale", "-1" },
            new[] { "--atari-presentation", "--hud-level-scale", "Infinity" },
            new[] { "--atari-presentation", "--hud-lives-advance", "1e100" },
            new[] { "--atari-presentation", "--hud-score-x", "1", "--hud-score-x", "2" },
            new[] { "--atari-presentation", "--hud-level-y=12" }
        })
        {
            Assert.False(DebugController.TryParseAtariHudCalibrationOptions(
                arguments, out AtariHudCalibrationOptions options, out string? error));
            Assert.False(options.HasExplicitOverrides);
            Assert.NotNull(error);
        }
    }

    [Fact]
    public void AtariHudCalibrationRequiresAtariPresentationAndPartialOverridesPreserveDefaults()
    {
        Assert.False(DebugController.TryParseAtariHudCalibrationOptions(
            ["--hud-score-x", "250"],
            out _, out string? contextError));
        Assert.Contains("--atari-presentation", contextError);

        Assert.True(DebugController.TryParseAtariHudCalibrationOptions(
            ["--atari-presentation", "--hud-score-x", "250", "--hud-lives-scale", "0.8"],
            out AtariHudCalibrationOptions options,
            out string? error));
        Assert.Null(error);
        Assert.Equal(250f, options.ResolvedScoreRight);
        Assert.Equal(AtariHudVector.ScoreBaselineY, options.ResolvedScoreBaselineY);
        Assert.Equal(AtariHudVector.HighScoreStartX, options.ResolvedHighScoreStartX);
        Assert.Equal(0.8f, options.ResolvedReserveLifeScale);
        Assert.Equal(AtariHudVector.ReserveLifeAdvance, options.ResolvedReserveLifeAdvance);
    }
}
