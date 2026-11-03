using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class AudioEventTests
{
    [Fact]
    public void PlayerShotAndLaneMovementEventsFollowSuccessfulActions()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(audioEvents: audio);

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Assert.Equal(1, audio.Count(AudioEvent.PlayerShot));

        for (int tick = 0; tick < 30; tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Equal(TempestGameShotLimit, game.Shots.Count);
        Assert.Equal(TempestGameShotLimit, audio.Count(AudioEvent.PlayerShot));

        game.RunOneStep(new PlayerInput(false, MouseDeltaX: 80, Left: false, Right: false));
        Assert.Equal(1, audio.Count(AudioEvent.PlayerMove));
        game.RunOneStep(new PlayerInput(false, MouseDeltaX: 0, Left: false, Right: false));
        Assert.Equal(1, audio.Count(AudioEvent.PlayerMove));
    }

    [Fact]
    public void SpikeShorteningAndSpikeDeathEmitTheirSemanticEvents()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(level4Test: true, audioEvents: audio);
        Spike spike = Assert.Single(game.Spikes, spike => spike.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 395;

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Equal(385, spike.Height);
        Assert.Equal(1, audio.Count(AudioEvent.SpikeHit));

        game.Player.AdvanceExit(20);
        game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.PlayerKilledByEnemy));
    }

    [Fact]
    public void PulsarPulseEventOccursOncePerEntryIntoPulseWindow()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(pulsarTest: true, audioEvents: audio);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();

        Step(game, 70);
        Assert.True(game.Pulsing);
        Assert.Equal(1, audio.Count(AudioEvent.PulsarPulse));

        Step(game, 39);
        Assert.False(game.Pulsing);
        Assert.Equal(1, audio.Count(AudioEvent.PulsarPulse));
    }

    [Fact]
    public void CreatedEnemyBulletAndBulletCausedPlayerDeathEmitOneEventEach()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(
            random: new MaximumRandom(),
            audioEvents: audio);
        Assert.False(game.EnemyBulletsTest);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Step(game, 17);

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(1, audio.Count(AudioEvent.EnemyBullet));

        game.Player.Move(bullet.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        for (int tick = 0; tick < 150 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.PlayerKilledByBullet));
    }

    [Fact]
    public void ZapResolutionPlaysOnceAndOrdinaryDestructionPreemptsPendingZapSound()
    {
        RecordingAudioEvents zapAudio = new();
        TempestGame zapGame = HatchOneFlipper(zapAudio);
        zapGame.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Step(zapGame, 4);

        Assert.Empty(zapGame.Flippers);
        Assert.Equal(1, zapAudio.Count(AudioEvent.ZapResolution));

        RecordingAudioEvents shotAudio = new();
        TempestGame shotGame = HatchOneFlipper(shotAudio);
        Flipper flipper = Assert.Single(shotGame.Flippers);
        shotGame.Player.Move(flipper.EdgeIndex - shotGame.Player.EdgeIndex,
            shotGame.CurrentLevel.Definition.Points.Count);
        shotGame.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(shotGame.Shots);
        shot.Height = flipper.Height + 5;
        flipper.TryZap(1);

        shotGame.RunOneStep();
        Step(shotGame, 4);

        Assert.Empty(shotGame.Flippers);
        Assert.Equal(1, shotAudio.Count(AudioEvent.EnemyDestroyed));
        Assert.Equal(0, shotAudio.Count(AudioEvent.ZapResolution));
    }

    [Fact]
    public void FlipperContactDeathUsesSharedEnemyDeathEvent()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = HatchOneFlipper(audio);
        Flipper flipper = Assert.Single(game.Flippers);
        while (flipper.State == FlipperState.Climbing)
            flipper.Update(game.CurrentLevel, playerEdge: -1);

        int destination = game.CurrentLevel.PreviousEdge(flipper.EdgeIndex);
        game.Player.Move(destination - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        for (int tick = 0; tick < 100 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.PlayerKilledByEnemy));
    }

    [Fact]
    public void PulsarContactDeathUsesSharedEnemyDeathEvent()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(pulsarTest: true, audioEvents: audio);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Pulsar pulsar = Assert.Single(game.Pulsars);

        for (int tick = 0; tick < 5000 && pulsar.Height < Level.Depth; tick++)
            pulsar.Update(game.CurrentLevel, pulsar.EdgeIndex, static (_, _) => 1);
        Assert.Equal(Level.Depth, pulsar.Height);
        game.Player.Move(pulsar.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        pulsar.BeginRimPlayerKill();
        game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.PlayerKilledByEnemy));
    }

    [Fact]
    public void FuseballContactDeathUsesSharedEnemyDeathEvent()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(
            fuseballTest: true,
            random: new MinimumRandom(),
            audioEvents: audio);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Fuseball fuseball = Assert.Single(game.Fuseballs);

        for (int tick = 0; tick < 1000 &&
            !(fuseball.State == FuseballState.Lateral && fuseball.Height == Level.Depth); tick++)
            fuseball.Update(game.CurrentLevel, 0, (minimum, maximum) => maximum == 3 ? 3 : 1);
        Assert.Equal(Level.Depth, fuseball.Height);
        AdvanceFuseball(fuseball, game.CurrentLevel, 18);
        game.Player.Move(fuseball.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.PlayerKilledByEnemy));
    }

    [Fact]
    public void NaturalTankerReleaseDoesNotEmitEnemyDestructionOrZapSound()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(tankerTest: true, startingLives: 10, audioEvents: audio);
        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        Tanker tanker = Assert.Single(game.Tankers);

        while (tanker.Height + Tanker.DevelopmentClimbSpeed <= Tanker.CargoReleaseThreshold)
            tanker.UpdateForDevelopment();
        tanker.TryZap(2);
        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(0, audio.Count(AudioEvent.EnemyDestroyed));
        Assert.Equal(0, audio.Count(AudioEvent.ZapResolution));
    }

    [Fact]
    public void ExitAndApproachTransitionsPlayOnceAndBonusLifeRemainsDeferred()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = new(audioEvents: audio);
        for (int enemy = 0; enemy < game.CurrentLevel.EnemySupply; enemy++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Step(game, 21);

        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.ZoomOut));
        Step(game, 2);
        Assert.Equal(1, audio.Count(AudioEvent.ZoomOut));

        for (int tick = 0; tick < 200 && game.State == LevelState.ExitZoom; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1, audio.Count(AudioEvent.ZoomIn));
        int livesBeforeBonusThreshold = game.Player.Lives;
        game.Score.Add(100_000);
        Assert.Equal(livesBeforeBonusThreshold, game.Player.Lives);
        Assert.Equal(0, audio.Count(AudioEvent.BonusLife));
    }

    [Fact]
    public void DefaultGameUsesSilentAudioWithoutLoadingRaylibResources()
    {
        TempestGame game = new();
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }

    private const int TempestGameShotLimit = 10;

    private static TempestGame HatchOneFlipper(RecordingAudioEvents audio)
    {
        TempestGame game = new(audioEvents: audio);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }

    private static void AdvanceFuseball(Fuseball fuseball, Level level, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            fuseball.Update(level, 0, static (minimum, _) => minimum);
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        private readonly List<AudioEvent> events = [];

        public int Count(AudioEvent audioEvent) => events.Count(item => item == audioEvent);

        public void Play(AudioEvent audioEvent) => events.Add(audioEvent);
    }

    private sealed class MaximumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => maxValue - 1;
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
