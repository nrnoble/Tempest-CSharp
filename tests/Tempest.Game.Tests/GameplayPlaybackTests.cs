using Tempest.Game.Debug;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class GameplayPlaybackTests
{
    [Fact]
    public void ReplayFramesAreSelectedBySimulationTick()
    {
        PlayerInput movement = new(false, 0, false, true);
        PlayerInputReplay replay = new(4225, 4,
        [
            new PlayerInputFrame(1, movement),
            new PlayerInputFrame(3, default, RestartPressed: true)
        ]);

        Assert.Equal(4225, replay.RandomSeed);
        Assert.Equal(4, replay.DurationTicks);
        Assert.Equal(new PlayerInputFrame(0, default), replay.GetFrame(0));
        Assert.Equal(new PlayerInputFrame(1, movement), replay.GetFrame(1));
        Assert.Equal(new PlayerInputFrame(2, default), replay.GetFrame(2));
        Assert.Equal(new PlayerInputFrame(3, default, RestartPressed: true), replay.GetFrame(3));
        Assert.Throws<ArgumentOutOfRangeException>(() => replay.GetFrame(4));
    }

    [Fact]
    public void SeededVisibleDemoProducesMatchingGameplayAtTickCheckpoints()
    {
        PlayerInputReplay replay = GameplayPlayback.CreateDemo();
        TempestGame first = new(random: new Random(replay.RandomSeed));
        TempestGame second = new(random: new Random(replay.RandomSeed));

        for (int tick = 0; tick < replay.DurationTicks; tick++)
        {
            PlayerInputFrame frame = replay.GetFrame(tick);
            Assert.Equal(tick, frame.SimulationTick);
            first.RunOneStep(frame.Input, frame.RestartPressed);
            second.RunOneStep(frame.Input, frame.RestartPressed);

            if ((tick + 1) % 30 == 0)
                AssertEquivalentGameplayState(first, second);
        }
    }

    private static void AssertEquivalentGameplayState(TempestGame first, TempestGame second)
    {
        Assert.Same(first.CurrentLevel.Configuration, second.CurrentLevel.Configuration);
        Assert.Equal(first.State, second.State);
        Assert.Equal(first.Player.EdgeIndex, second.Player.EdgeIndex);
        Assert.Equal(first.Player.EdgePosition, second.Player.EdgePosition);
        Assert.Equal(first.Player.Depth, second.Player.Depth);
        Assert.Equal(first.Player.Lives, second.Player.Lives);
        Assert.Equal(first.DeathTicks, second.DeathTicks);
        Assert.Equal(first.Score.Score, second.Score.Score);
        Assert.Equal(first.HurryUp, second.HurryUp);
        Assert.Equal(first.SuperZapperChargesRemaining, second.SuperZapperChargesRemaining);
        Assert.Equal(first.SuperZapper.FlashTicksRemaining, second.SuperZapper.FlashTicksRemaining);
        Assert.Equal(first.CurrentLevel.EnemiesSpawned, second.CurrentLevel.EnemiesSpawned);
        Assert.Equal(first.CurrentLevel.EnemiesDestroyed, second.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(first.CurrentLevel.EnemiesRimCleared, second.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(first.CurrentLevel.ReserveEggs, second.CurrentLevel.ReserveEggs);
        Assert.Equal(first.Eggs.Select(egg =>
                (egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)),
            second.Eggs.Select(egg =>
                (egg.EdgeIndex, egg.Height, egg.State, egg.UpdateCounter)));
        Assert.Equal(first.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
            second.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
        Assert.Equal(first.Flippers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.PauseTicks, enemy.PendingZapTicks)),
            second.Flippers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.PauseTicks, enemy.PendingZapTicks)));
        Assert.Equal(first.Tankers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.BulletAmmunition, enemy.BulletTimer)),
            second.Tankers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.BulletAmmunition, enemy.BulletTimer)));
        Assert.Equal(first.Spikers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.Growth, enemy.Rotation)),
            second.Spikers.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.State, enemy.Growth, enemy.Rotation)));
        Assert.Equal(first.EnemyBullets.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.PendingZapTicks)),
            second.EnemyBullets.Select(enemy =>
                (enemy.EdgeIndex, enemy.Height, enemy.PendingZapTicks)));
    }
}
