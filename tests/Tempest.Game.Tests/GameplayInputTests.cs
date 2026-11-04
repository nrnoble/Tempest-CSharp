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
