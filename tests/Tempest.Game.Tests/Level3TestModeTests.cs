using System.Numerics;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level3TestModeTests
{
    [Fact]
    public void DirectLevel3FlagSelectsMixedProductionWaveAndComposesWithGeneralOptions()
    {
        string[] arguments = ["--level3-test", "--lives", "10", "--enemy-bullets-test"];

        Assert.True(DebugController.UseLevel3Test(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.Equal(10, DebugController.GetStartingLives(arguments));

        TempestGame game = new(
            startingLives: DebugController.GetStartingLives(arguments),
            enemyBulletsTest: DebugController.UseEnemyBulletsTest(arguments),
            level3Test: DebugController.UseLevel3Test(arguments));

        Assert.True(game.Level3Test);
        Assert.True(game.EnemyBulletsTest);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(3, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(15, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(10, game.Player.Lives);
    }

    [Fact]
    public void DirectLevel3FlagConflictsWithEveryExclusiveGameplayScenario()
    {
        string[] scenarios =
        [
            "--level2-test", "--level3-geometry-test", "--tanker-test",
            "--spiker-test", "--fuseball-test", "--pulsar-test"
        ];

        foreach (string scenario in scenarios)
        {
            string[] arguments = ["--level3-test", scenario];
            Assert.True(DebugController.HasConflictingScenarios(arguments));
            Assert.Throws<ArgumentException>(() => new TempestGame(
                spikerDevelopmentWave: DebugController.UseSpikerWave(arguments),
                tankerTest: DebugController.UseTankerTest(arguments),
                fuseballTest: DebugController.UseFuseballTest(arguments),
                pulsarTest: DebugController.UsePulsarTest(arguments),
                level2Test: DebugController.UseLevel2Test(arguments),
                level3Test: DebugController.UseLevel3Test(arguments),
                level3GeometryTest: DebugController.UseLevel3GeometryTest(arguments)));
        }

        Assert.False(DebugController.UseLevel3Test([]));
        Assert.False(DebugController.HasConflictingScenarios(
            ["--level3-test", "--lives", "10", "--enemy-bullets-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(
            level3Test: true, startingConfiguration: LevelConfiguration.Level3Mixed));
    }

    [Fact]
    public void MixedWaveTankerBulletsReuseTheExistingProjectionAndDrawCallback()
    {
        InclusiveRangeScript random = new(
        [
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            (0, 100, 96),
            (0, 100, 95),
            .. Enumerable.Repeat((0, 100, 95), 38),
            (-6, 4, 4),
            (0, 2, 0),
            (1, 2, 1),
            (1, 64, 1),
            .. Enumerable.Repeat((0, 100, 95), 9),
            .. Enumerable.Range(0, 6)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            .. Enumerable.Repeat((0, 100, 95), 8),
            (0, 100, 100)
        ]);
        TempestGame game = new(level3Test: true, random: random);

        for (int tick = 0; tick < 57; tick++)
            game.RunOneStep();

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.False(game.EnemyBulletsTest);
        VectorRenderer renderer = new(game);
        Assert.True(renderer.ShouldDrawEnemyBullets);
        AssertFinite(renderer.ProjectEnemyBullet(bullet));

        int lines = 0;
        renderer.DrawEnemyBullets((start, end) =>
        {
            AssertFinite(start);
            AssertFinite(end);
            lines++;
        });
        Assert.Equal(8, lines);

        IReadOnlyList<string> waveLines = renderer.GetWaveHudLines();
        Assert.Contains("ORIGINAL 15", waveLines);
        Assert.Contains("CARGO 2", waveLines);
        Assert.NotEmpty(game.Tankers);
        Assert.Equal($"ACTIVE {game.Eggs.Count + game.Flippers.Count + game.Tankers.Count}",
            waveLines[2]);
        Assert.NotEqual($"ACTIVE {game.Eggs.Count + game.Flippers.Count}", waveLines[2]);
        Assert.DoesNotContain("FLIPPERS 15", waveLines);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void BulletVisibilityAndHudRemainCorrectForExistingModes()
    {
        Assert.False(new VectorRenderer(new TempestGame()).ShouldDrawEnemyBullets);
        Assert.False(new VectorRenderer(new TempestGame(level2Test: true)).ShouldDrawEnemyBullets);
        Assert.False(new VectorRenderer(new TempestGame(level3GeometryTest: true)).ShouldDrawEnemyBullets);
        Assert.False(new VectorRenderer(new TempestGame(tankerTest: true)).ShouldDrawEnemyBullets);
        Assert.True(new VectorRenderer(new TempestGame(enemyBulletsTest: true)).ShouldDrawEnemyBullets);
        Assert.True(new VectorRenderer(new TempestGame(tankerTest: true,
            enemyBulletsTest: true)).ShouldDrawEnemyBullets);

        TempestGame game = new(level3Test: true, startingLives: 8, random: new Random(4));
        game.Score.Add(325);
        VectorRenderer renderer = new(game);
        Assert.Equal(3, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(8, game.Player.Lives);
        Assert.Equal(
            ["ORIGINAL 15", "CARGO 0", "ACTIVE 4"],
            renderer.GetWaveHudLines());
    }

    [Fact]
    public void DirectLevel3ReplayAndGameOverRestartUseTheEstablishedLifecycle()
    {
        TempestGame game = new(level3Test: true, startingLives: 5, random: new Random(5));
        Level completedLevel = game.CurrentLevel;
        game.Score.Add(325);
        game.Player.Lives = 3;
        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);

        Assert.NotSame(completedLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(15, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(0, game.CurrentLevel.Level3Ledger.CargoCount);

        Level gameOverLevel = game.CurrentLevel;
        game.Score.Add(100);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);

        Assert.NotSame(gameOverLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }

    private sealed class InclusiveRangeScript(
        IEnumerable<(int Minimum, int Maximum, int Value)> decisions) : Random
    {
        private readonly Queue<(int Minimum, int Maximum, int Value)> remaining = new(decisions);

        public int Remaining => remaining.Count;

        public override int Next(int minValue, int maxValue)
        {
            if (remaining.Count == 0)
                throw new InvalidOperationException(
                    $"Unexpected random request [{minValue}, {maxValue}).");

            (int expectedMinimum, int expectedMaximum, int value) = remaining.Dequeue();
            Assert.Equal(expectedMinimum, minValue);
            Assert.Equal(expectedMaximum + 1, maxValue);
            Assert.InRange(value, minValue, maxValue - 1);
            return value;
        }
    }
}
