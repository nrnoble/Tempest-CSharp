using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class GameplayInputTests
{
    private static readonly PlayerInput[] InputsBeforeZapper =
    [
        default,
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(false, 0, false, true),
        new(true, 0, false, false),
        new(true, 0, false, false),
        new(true, 0, false, false),
        new(true, 0, false, false),
        new(true, 0, false, false),
        new(true, 0, false, false),
        new(true, 0, false, false)
    ];

    [Fact]
    public void SeededTickIndexedGameplayInputsProduceTheSameLevelOneResult()
    {
        RecordingAudioEvents firstAudio = new();
        RecordingAudioEvents secondAudio = new();
        TempestGame first = new(random: new Random(4225), audioEvents: firstAudio);
        TempestGame second = new(random: new Random(4225), audioEvents: secondAudio);

        RunInputsBeforeZapper(first);
        RunInputsBeforeZapper(second);

        PlayerInput zapPress = new(false, 0, false, false, ZapPressed: true);
        first.RunOneStep(zapPress);
        second.RunOneStep(zapPress);
        Assert.Equal(1, first.SuperZapperChargesRemaining);
        Assert.Equal(60, first.SuperZapper.FlashTicksRemaining);
        Assert.Equal(1, second.SuperZapperChargesRemaining);
        Assert.Equal(60, second.SuperZapper.FlashTicksRemaining);

        first.RunOneStep();
        second.RunOneStep();
        Assert.Equal(1, first.SuperZapperChargesRemaining);
        Assert.Equal(59, first.SuperZapper.FlashTicksRemaining);
        Assert.Equal(1, second.SuperZapperChargesRemaining);
        Assert.Equal(59, second.SuperZapper.FlashTicksRemaining);

        AssertExpectedOutcome(first, firstAudio);
        AssertExpectedOutcome(second, secondAudio);

        Assert.Equal(first.CurrentLevel.Configuration.DisplayedLevel,
            second.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(first.State, second.State);
        Assert.Equal(first.Player.EdgeIndex, second.Player.EdgeIndex);
        Assert.Equal(first.Player.EdgePosition, second.Player.EdgePosition);
        Assert.Equal(first.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
            second.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
        Assert.Equal(first.SuperZapperChargesRemaining, second.SuperZapperChargesRemaining);
        Assert.Equal(first.SuperZapper.FlashTicksRemaining, second.SuperZapper.FlashTicksRemaining);
        Assert.Equal(first.Score.Score, second.Score.Score);
        Assert.Equal(first.Player.Lives, second.Player.Lives);
        Assert.Equal(firstAudio.Events, secondAudio.Events);
    }

    [Fact]
    public void SeededExitMovementAndApproachReachTheSameProductionLevelTwo()
    {
        var first = RunLevelOneExitAndApproach();
        var second = RunLevelOneExitAndApproach();

        Assert.NotSame(first.Game, second.Game);
        Assert.NotSame(first.Game.CurrentLevel, second.Game.CurrentLevel);
        Assert.Equal(first.ExitLane, second.ExitLane);
        Assert.Equal(first.ExitPosition, second.ExitPosition);
        Assert.Equal(first.ExitDepth, second.ExitDepth);
        Assert.Same(first.Game.CurrentLevel.Configuration, second.Game.CurrentLevel.Configuration);
        Assert.Equal(first.Game.State, second.Game.State);
        Assert.Equal(first.Game.Player.EdgeIndex, second.Game.Player.EdgeIndex);
        Assert.Equal(first.Game.Player.EdgePosition, second.Game.Player.EdgePosition);
        Assert.Equal(first.Game.Player.Depth, second.Game.Player.Depth);
        Assert.Equal(first.Game.Score.Score, second.Game.Score.Score);
        Assert.Equal(first.Game.Player.Lives, second.Game.Player.Lives);
        Assert.Equal(first.Game.DeathTicks, second.Game.DeathTicks);
        Assert.Equal(first.Game.SuperZapperChargesRemaining, second.Game.SuperZapperChargesRemaining);
        Assert.Equal(first.Game.SuperZapper.FlashTicksRemaining, second.Game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(first.Game.CurrentLevel.EnemiesSpawned, second.Game.CurrentLevel.EnemiesSpawned);
        Assert.Equal(first.Game.CurrentLevel.ReserveEggs, second.Game.CurrentLevel.ReserveEggs);
        Assert.Equal(
            first.Game.Eggs.Select(egg => (egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)),
            second.Game.Eggs.Select(egg => (egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)));
    }

    private static (TempestGame Game, int ExitLane, float ExitPosition, float ExitDepth)
        RunLevelOneExitAndApproach()
    {
        TempestGame game = new(random: new Random(4225));
        Level outgoing = game.CurrentLevel;
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level1, outgoing.Configuration);
        Assert.Equal(1, outgoing.Configuration.DisplayedLevel);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Equal(10, outgoing.EnemySupply);
        Assert.Equal(10, outgoing.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, outgoing.ReserveEggs);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(Player.DefaultStartingLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);

        // TEST SETUP: resolve obligations directly, not through player combat. The existing
        // reconstruction seam removes their Egg/reserve representations without a simulated death.
        for (int enemy = 0; enemy < outgoing.EnemySupply; enemy++)
            outgoing.RecordEnemyDestroyed();
        outgoing.ResetAfterDeath();
        game.Score.Add(1500);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex,
            outgoing.PlayableLaneCount, outgoing.Continuous));
        Assert.Equal(10, outgoing.EnemiesDestroyed);
        Assert.Equal(0, outgoing.EnemiesRimCleared);
        Assert.Equal(0, outgoing.RemainingEnemies);
        Assert.Equal(0, outgoing.ReserveEggs);
        Assert.Empty(game.Eggs);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(60, game.SuperZapper.FlashTicksRemaining);

        // AUTOMATED GAMEPLAY INPUT: all state transitions and travel below use RunOneStep.
        // Fixed counts and tick-indexed inputs make both runs execute the same 327 steps.
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        for (int tick = 0; tick < 21; tick++)
        {
            game.RunOneStep();
            Assert.Equal(tick < 20 ? LevelState.ExitDelay : LevelState.ExitZoom, game.State);
            Assert.Same(outgoing, game.CurrentLevel);
            Assert.Equal(Level.Depth, game.Player.Depth);
            Assert.Equal(7, game.Player.EdgeIndex);
            Assert.Equal(2, game.Player.Lives);
            Assert.Equal(0, game.DeathTicks);
        }

        int exitLane = game.Player.EdgeIndex;
        float exitPosition = game.Player.EdgePosition;
        float exitDepth = game.Player.Depth;
        for (int tick = 0; tick < 123; tick++)
        {
            PlayerInput input = tick < 9 ? new(false, 0, false, true) : default;
            float previousDepth = game.Player.Depth;
            Assert.True(game.ExitMovementInputEnabled);
            game.RunOneStep(input);
            Assert.Equal(LevelState.ExitZoom, game.State);
            Assert.Same(outgoing, game.CurrentLevel);
            Assert.True(game.Player.Depth < previousDepth);
            Assert.Equal(1500, game.Score.Score);
            Assert.Equal(2, game.Player.Lives);
            Assert.Equal(0, game.DeathTicks);
            Assert.Equal(1, game.SuperZapperChargesRemaining);
            Assert.Empty(game.Shots);
            if (tick == 8)
            {
                exitLane = game.Player.EdgeIndex;
                exitPosition = game.Player.EdgePosition;
                exitDepth = game.Player.Depth;
                Assert.Equal(outgoing.NextEdge(7), exitLane);
                Assert.InRange(exitPosition, 0, .5f);
                Assert.Equal(373, exitDepth);
            }
            else if (tick > 8)
            {
                Assert.Equal(exitLane, game.Player.EdgeIndex);
                Assert.Equal(exitPosition, game.Player.EdgePosition);
            }
        }
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);
        game.RunOneStep();

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.NotNull(game.CurrentApproach);
        Assert.Equal(0, game.CurrentApproach!.ElapsedTicks);
        Assert.Equal(LevelApproach.InitialDepthOffset, game.CurrentApproach.DepthOffset);
        Assert.Same(LevelConfiguration.Level2Square, incoming.Configuration);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
        {
            Assert.Equal(LevelState.Approach, game.State);
            Assert.Equal(tick, game.CurrentApproach!.ElapsedTicks);
            game.RunOneStep();
            Assert.Same(incoming, game.CurrentLevel);
            Assert.Equal(1500, game.Score.Score);
            Assert.Equal(2, game.Player.Lives);
            Assert.Equal(0, game.DeathTicks);
            Assert.All(game.Eggs, egg => Assert.Equal(0, egg.UpdateCounter));
        }

        Assert.True(game.NaturalProgressionEnabled);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.False(game.GameOver);
        Assert.False(game.HurryUp);
        Assert.Same(LevelConfiguration.Level2Square, incoming.Configuration);
        Assert.Equal(2, incoming.Configuration.DisplayedLevel);
        Assert.Equal(1, incoming.Configuration.ProgressionIndex);
        Assert.Equal(1, incoming.Configuration.BoardIndex);
        Assert.Equal(0, incoming.Configuration.PaletteIndex);
        Assert.True(incoming.Continuous);
        Assert.Equal(16, incoming.PlayableLaneCount);
        Assert.True(incoming.Configuration.Flipper.CanFlip);
        Assert.Equal(12, incoming.EnemySupply);
        Assert.Equal(12, incoming.RemainingEnemies);
        Assert.Equal(0, incoming.EnemiesDestroyed);
        Assert.Equal(0, incoming.EnemiesRimCleared);
        Assert.Equal(4, incoming.EnemiesSpawned);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, incoming.ReserveEggs);
        Assert.All(game.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
        Assert.Empty(game.Flippers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikes);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        return (game, exitLane, exitPosition, exitDepth);
    }

    private static void RunInputsBeforeZapper(TempestGame game)
    {
        foreach (PlayerInput input in InputsBeforeZapper)
            game.RunOneStep(input);
    }

    private static void AssertExpectedOutcome(TempestGame game, RecordingAudioEvents audio)
    {
        Assert.Equal(1, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(game.CurrentLevel.NextEdge(7), game.Player.EdgeIndex);
        Assert.InRange(game.Player.EdgePosition, 0, 0.5f);
        Assert.Equal(3, game.Shots.Count);
        Assert.Collection(
            game.Shots,
            shot =>
            {
                Assert.Equal(game.Player.EdgeIndex, shot.EdgeIndex);
                Assert.Equal(346, shot.Height);
            },
            shot =>
            {
                Assert.Equal(game.Player.EdgeIndex, shot.EdgeIndex);
                Assert.Equal(364, shot.Height);
            },
            shot =>
            {
                Assert.Equal(game.Player.EdgeIndex, shot.EdgeIndex);
                Assert.Equal(382, shot.Height);
            });
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(59, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(Player.DefaultStartingLives, game.Player.Lives);
        Assert.Equal(
            new[]
            {
                AudioEvent.PlayerMove,
                AudioEvent.PlayerShot,
                AudioEvent.PlayerShot,
                AudioEvent.PlayerShot
            },
            audio.Events);
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        public List<AudioEvent> Events { get; } = [];

        public void Play(AudioEvent audioEvent) => Events.Add(audioEvent);
    }
}
