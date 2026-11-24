using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class StartLevelTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    public void ProductionStartsUseFreshProductionState(int level)
    {
        string[] args = ["--start-level", level.ToString()];
        Assert.True(DebugController.TryParseStartingLevel(args, out int? selected, out string? error));
        Assert.Null(error);
        Assert.Equal(level, selected);
        TempestGame game = new(startingProductionLevel: selected, random: new Random(1981),
            startingLives: DebugController.GetStartingLives(args));
        LevelConfiguration configuration = LevelProgression.GetProductionLevel(level)!;
        Assert.Same(configuration, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(Player.DefaultStartingLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(configuration.InitialActiveEggCount, game.Eggs.Count);
        Assert.Equal(configuration.EnemySupply - configuration.InitialActiveEggCount,
            game.CurrentLevel.ReserveEggs);
        Assert.Equal(level.ToString("D2"), new string(AtariHudVector.FormatFixedTwoPositionLevel(
            game.CurrentLevel.Configuration.DisplayedLevel)));
        Assert.True(game.NaturalProgressionEnabled);
    }

    [Fact]
    public void NaturalLevelThirteenToFourteenProgressionCarriesCampaignState()
    {
        TempestGame game = new(startingProductionLevel: 13, startingLives: 4,
            random: new Random(1981));
        game.Score.Add(1234);
        CompleteWaveAndExit(game);

        Assert.Same(LevelConfiguration.Level14Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level15Production, LevelProgression.GetNextProductionLevel(
            LevelConfiguration.Level14Production));
    }

    [Fact]
    public void NaturalLevelFourteenToFifteenRechargesZapperAndUsesOutgoingStarfield()
    {
        TempestGame game = new(startingProductionLevel: 14, startingLives: 4,
            random: new Random(1981));
        game.Score.Add(1234);
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        CompleteWaveAndExit(game);

        Assert.Same(LevelConfiguration.Level15Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(14, game.CurrentApproach!.ExitedLevel);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(27, game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(LevelConfiguration.Level15Production.InitialActiveEggCount, game.Eggs.Count);
        List<Color> colors = [];
        TransitionStarfield.Draw(game.CurrentApproach.ExitedLevel, 1, (_, color) => colors.Add(color));
        Assert.NotEmpty(colors);
    }

    [Fact]
    public void TerminalLevelFifteenCompletionRestartsAtLevelOne()
    {
        TempestGame game = new(startingProductionLevel: 15, startingLives: 5);
        game.Score.Add(1234);

        CompleteWaveAndExit(game);

        Assert.True(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level15Production, game.CurrentLevel.Configuration);
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    [Fact]
    public void ParsingRejectsInvalidAndConflictingStartSelectors()
    {
        string[][] invalid =
        [
            ["--start-level"], ["--start-level", "--lives", "5"],
            ["--start-level", "abc"], ["--start-level", "0"],
            ["--start-level", "-1"],
            ["--start-level", "16"],
            ["--start-level", "999999999999999"], ["--start-level=11"],
            ["--start-level", "11", "--start-level", "12"],
            ["--start-level", "11", "--level", "11"],
            ["--level", "11", "--start-level", "11"]
        ];
        foreach (string[] args in invalid)
        {
            Assert.False(DebugController.TryParseStartingLevel(args, out _, out string? error));
            Assert.NotNull(error);
        }
        Assert.True(DebugController.TryParseStartingLevel(["--level", "10"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--level", "13"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--level", "14"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--start-level", "13"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--start-level", "14"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--start-level", "15"], out _, out _));
        Assert.True(DebugController.TryParseStartingLevel(["--level", "15"], out _, out _));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--start-level", "11", "--gameplay-playback"]));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--start-level", "11", "--level2-test"]));
    }

    [Fact]
    public void FileModesAndPresentationOptionsRespectStartupOwnership()
    {
        string[] args =
        [
            "--record-gameplay", "session.json", "--start-level", "11", "--lives", "7",
            "--atari-presentation", "--calibration-guides", "--tube-y-gain", "1.2",
            "--tube-scale", "0.9", "--tube-vertical-offset", "-20"
        ];
        Assert.True(DebugController.TryParseGameplayFileCommand(args, out GameplayFileCommand? command,
            out string? error), error);
        Assert.Equal(11, command!.StartingDisplayedLevel);
        Assert.Equal(7, command.StartingLives);
        Assert.True(DebugController.TryParseTubeYGainOptions(args, out var gain, out _));
        Assert.True(DebugController.TryParseTubeScaleOptions(args, out var scale, out _));
        Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(args, out var offset, out _));
        Assert.Equal(1.2f, gain.Gain);
        Assert.Equal(.9f, scale.Scale);
        Assert.Equal(-20f, offset.Offset);
        Assert.True(DebugController.TryParseCalibrationGuides(args, out bool guides, out _));
        Assert.True(guides);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "session.json", "--start-level", "11"],
            out _, out string? playbackError));
        Assert.Contains("--start-level", playbackError);
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "session.json", "--start-level", "10"],
            out GameplayFileCommand? level10Command, out _));
        Assert.Equal(10, level10Command!.StartingDisplayedLevel);
        using StringWriter output = new();
        using StringWriter errors = new();
        Assert.Equal(HeadlessGameplayPlaybackCli.InvalidCommandExitCode,
            HeadlessGameplayPlaybackCli.Run(
                ["--headless-playback", "session.json", "--start-level", "11"], output, errors));
        Assert.Contains("--start-level", errors.ToString());
    }

    [Theory]
    [InlineData("--level")]
    [InlineData("--start-level")]
    public void LevelTenStartsTheProductionConfigurationAndContinuesToEleven(string option)
    {
        Assert.True(DebugController.TryParseStartingLevel([option, "10"], out int? level, out string? error), error);
        Assert.Same(LevelConfiguration.Level10Production, LevelProgression.GetProductionLevel(10));
        Assert.Same(LevelConfiguration.Level10Production, LevelProgression.GetDirectStartLevel(10));
        Assert.Same(LevelConfiguration.Level10Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level9Production));
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level10Production));

        TempestGame game = new(startingProductionLevel: level, random: new Random(1981));
        Assert.Same(LevelConfiguration.Level10Production, game.CurrentLevel.Configuration);
        Assert.Equal(9, game.CurrentLevel.Configuration.ProgressionIndex);
        Assert.Equal(9, game.CurrentLevel.Configuration.BoardIndex);
        game.Score.Add(1234);
        game.Player.Lives = 4;
        CompleteWaveAndExit(game);
        Assert.Same(LevelConfiguration.Level11Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(14)]
    [InlineData(15)]
    public void RecordingFromStartLevelRestoresTheSameSeededStartupInPlayback(int level)
    {
        string[] args = ["--record-gameplay", "session.json", "--start-level", level.ToString(),
            "--lives", "7", "--rim-speed", "1.5"];
        Assert.True(DebugController.TryParseGameplayFileCommand(args, out GameplayFileCommand? command,
            out string? error), error);
        GameplayStartupConfiguration startup = new(command!.StartingDisplayedLevel!.Value,
            command.StartingLives!.Value, command.RimSpeedMultiplier!.Value, 1981);
        TempestGame recordedGame = new(startup);
        PlayerInputRecorder recorder = new(startup);
        for (int tick = 0; tick < 30; tick++)
        {
            recordedGame.RunOneStep();
            recorder.CaptureTick(tick, default);
        }
        GameplayPlaybackSession playback = new(startup, recorder.CreateReplay());
        Assert.Same(LevelProgression.GetProductionLevel(level), playback.Game.CurrentLevel.Configuration);
        Assert.Equal(7, playback.Game.Player.Lives);
        playback.RunToBoundary(30);
        Assert.Equal(recordedGame.Score.Score, playback.Game.Score.Score);
        Assert.Equal(recordedGame.Player.Lives, playback.Game.Player.Lives);
        Assert.Equal(recordedGame.Eggs.Count, playback.Game.Eggs.Count);
        Assert.Equal(recordedGame.CurrentLevel.ReserveEggs, playback.Game.CurrentLevel.ReserveEggs);
        Assert.Equal(recordedGame.SuperZapperChargesRemaining,
            playback.Game.SuperZapperChargesRemaining);
    }

    [Theory]
    [InlineData("--level")]
    [InlineData("--start-level")]
    public void DirectProductionStartsContinueThroughFifteenAndStopThere(string option)
    {
        string[] args = [option, "11", "--lives", "7"];
        Assert.True(DebugController.TryParseStartingLevel(args, out int? level, out _));
        TempestGame game = new(startingProductionLevel: level,
            startingLives: DebugController.GetStartingLives(args), random: new Random(1981));
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);
        VectorRenderer overridden = new(game, atariPresentationEnabled: true, tubeScale: .95f);
        Assert.Equal(7, game.Player.Lives);
        Assert.Same(LevelConfiguration.Level11Production, game.CurrentLevel.Configuration);
        AtariHudLine[] hud11 = AtariHudVector.Render(game)
            .Where(line => line.Color == AtariHudVector.LevelColor).ToArray();
        game.Score.Add(1234);
        game.Player.Lives = 4;

        CompleteWaveAndExit(game);
        Assert.Same(LevelConfiguration.Level12Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(LevelConfiguration.Level12Production.InitialActiveEggCount, game.Eggs.Count);
        AtariHudLine[] hud12 = AtariHudVector.Render(game)
            .Where(line => line.Color == AtariHudVector.LevelColor).ToArray();
        Assert.NotEqual(hud11, hud12);
        TempestGame level12 = new(startingProductionLevel: 12, random: new Random(1981));
        Assert.Equal(AtariHudVector.Render(level12).Where(line => line.Color == AtariHudVector.LevelColor),
            hud12);
        VectorRenderer explicitLevel12 = new(game, atariPresentationEnabled: true,
            tubeYGain: 1.3f, tubeScale: .8f, tubeVerticalOffset: 0);
        Assert.Equal(CaptureTube(explicitLevel12), CaptureTube(renderer));
        VectorRenderer explicitOverride = new(game, atariPresentationEnabled: true,
            tubeYGain: 1.3f, tubeScale: .95f, tubeVerticalOffset: 0);
        Assert.Equal(CaptureTube(explicitOverride), CaptureTube(overridden));

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        CompleteWaveAndExit(game);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level13Production, game.CurrentLevel.Configuration);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        AtariHudLine[] hud13 = AtariHudVector.Render(game)
            .Where(line => line.Color == AtariHudVector.LevelColor).ToArray();
        Assert.NotEqual(hud12, hud13);
        TempestGame level13 = new(startingProductionLevel: 13, random: new Random(1981));
        Assert.Equal(AtariHudVector.Render(level13).Where(line => line.Color == AtariHudVector.LevelColor),
            hud13);
        AtariPresentationCalibration level13Calibration =
            AtariPresentationCalibration.ForDisplayedLevel(13);
        VectorRenderer explicitLevel13 = new(game, atariPresentationEnabled: true,
            tubeYGain: level13Calibration.TubeYGain,
            tubeScale: level13Calibration.TubeScale,
            tubeVerticalOffset: level13Calibration.VerticalOffset,
            tubeDepthYGain: level13Calibration.TubeDepthYGain);
        Assert.Equal(CaptureTube(explicitLevel13), CaptureTube(renderer));

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        CompleteWaveAndExit(game);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level14Production, game.CurrentLevel.Configuration);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        AtariHudLine[] hud14 = AtariHudVector.Render(game)
            .Where(line => line.Color == AtariHudVector.LevelColor).ToArray();
        Assert.NotEqual(hud13, hud14);
        TempestGame level14 = new(startingProductionLevel: 14, random: new Random(1981));
        Assert.Equal(AtariHudVector.Render(level14).Where(line => line.Color == AtariHudVector.LevelColor),
            hud14);
        VectorRenderer explicitLevel14 = new(game, atariPresentationEnabled: true,
            tubeYGain: 1, tubeScale: 1.1f, tubeVerticalOffset: 200, tubeDepthYGain: 1);
        Assert.Equal(CaptureTube(explicitLevel14), CaptureTube(renderer));

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        CompleteWaveAndExit(game);
        Assert.False(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level15Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1234, game.Score.Score);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(LevelConfiguration.Level15Production.InitialActiveEggCount, game.Eggs.Count);
        AtariHudLine[] hud15 = AtariHudVector.Render(game)
            .Where(line => line.Color == AtariHudVector.LevelColor).ToArray();
        Assert.NotEqual(hud14, hud15);
        TempestGame level15 = new(startingProductionLevel: 15, random: new Random(1981));
        Assert.Equal(AtariHudVector.Render(level15).Where(line => line.Color == AtariHudVector.LevelColor),
            hud15);
        Assert.Equal(new AtariPresentationCalibration(1.15f, 1, 75, 1),
            AtariPresentationCalibration.ForDisplayedLevel(15));
        VectorRenderer explicitLevel15 = new(game, atariPresentationEnabled: true,
            tubeYGain: 1.15f, tubeScale: 1, tubeVerticalOffset: 75, tubeDepthYGain: 1);
        Assert.Equal(CaptureTube(explicitLevel15), CaptureTube(renderer));

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        CompleteWaveAndExit(game);
        Assert.True(game.LevelComplete);
        Assert.Same(LevelConfiguration.Level15Production, game.CurrentLevel.Configuration);
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(7, game.Player.Lives);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    public void GameOverRestartRetainsExistingFreshLevelOneRule(int level)
    {
        TempestGame game = new(startingProductionLevel: level, startingLives: 8,
            random: new Random(1981));
        game.Score.Add(900);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(8, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
    }

    private static void CompleteWaveAndExit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        Level3WaveLedger ledger = outgoing.Level3Ledger!;
        foreach (int id in ledger.UnresolvedOriginalIds.Order())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }
        outgoing.ResetAfterDeath();
        game.RunOneStep();
        foreach (var spike in game.Spikes)
            spike.Height = 0;
        if (game.State == LevelState.AvoidSpikes)
        {
            for (int tick = 0; tick <= outgoing.Configuration.AvoidSpikesWaitThreshold; tick++)
                game.RunOneStep();
        }
        Assert.Equal(LevelState.ExitDelay, game.State);
        for (int tick = 0; tick < 300 && ReferenceEquals(outgoing, game.CurrentLevel) &&
            !game.LevelComplete; tick++)
            game.RunOneStep();
        Assert.True(game.LevelComplete || !ReferenceEquals(outgoing, game.CurrentLevel));
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(VectorRenderer renderer)
    {
        List<(Vector2, Vector2, Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
        return lines;
    }
}
