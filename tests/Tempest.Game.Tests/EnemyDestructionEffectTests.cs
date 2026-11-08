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
public sealed class EnemyDestructionEffectTests
{
    [Fact]
    public void PoolUsesEightSlotsRomAgeStepsAndDeterministicReplacement()
    {
        EnemyDestructionEffectPool pool = new();
        for (int index = 0; index < EnemyDestructionEffectPool.Capacity; index++)
        {
            EnemyDestructionEffect effect = pool.Add(index, index);
            Assert.Equal(EnemyDestructionEffectPool.Capacity - index - 1, effect.Slot);
            Assert.Equal(0, effect.Age);
        }

        Assert.Equal(EnemyDestructionEffectPool.Capacity, pool.ActiveCount);
        pool.Advance();
        Assert.All(pool.Slots, effect => Assert.Equal(3, effect.Age));
        EnemyDestructionEffect tiedReplacement = pool.Add(20, 20);
        Assert.Equal(0, tiedReplacement.Slot);

        pool.Clear();
        EnemyDestructionEffect oldest = pool.Add(0, 0);
        pool.Advance();
        pool.Advance();
        for (int index = 1; index < EnemyDestructionEffectPool.Capacity; index++)
            pool.Add(index, index);

        Assert.Equal(6, oldest.Age);
        Assert.Same(oldest, pool.Add(30, 30));
        Assert.Equal(30, oldest.EdgeIndex);
        Assert.Equal(0, oldest.Age);
    }

    [Fact]
    public void EffectRemainsVisibleAtAgeZeroAndExpiresAtTwelve()
    {
        EnemyDestructionEffectPool pool = new();
        EnemyDestructionEffect effect = pool.Add(4, 25);

        Assert.True(effect.IsActive);
        Assert.Equal(0, effect.Age);
        pool.Advance();
        Assert.Equal(3, effect.Age);
        pool.Advance();
        Assert.Equal(6, effect.Age);
        pool.Advance();
        Assert.Equal(9, effect.Age);
        Assert.True(effect.IsActive);
        pool.Advance();
        Assert.Equal(12, effect.Age);
        Assert.False(effect.IsActive);
        Assert.Equal(4, effect.EdgeIndex);
        Assert.Equal(25, effect.Height);
    }

    [Fact]
    public void FlipperShotCapturesPositionRendersRomTypeZeroAndLeavesAccountingAlone()
    {
        TempestGame game = new(startingLives: 100);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        int lane = flipper.EdgeIndex;
        float height = ShootAt(game, lane, () => flipper.Height);

        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        Assert.Equal(lane, effect.EdgeIndex);
        Assert.Equal(height, effect.Height);
        Assert.Equal(EnemyDestructionEffectProgram.BlasterHitType0, effect.Program);
        Assert.Equal(0, effect.Age);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);

        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawEnemyDestructionEffects(
            (start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(8, lines.Count);
        Assert.All(lines, line => Assert.Equal(Color.White, line.Color));
        Vector2 expectedCenter = ProjectLane(game, lane, height);
        float scale = 1 / ((game.CurrentLevel.Configuration.Position +
            game.CurrentLevel.Configuration.Depth - height + Projection.ZOffset) / Projection.K);
        Assert.Equal(expectedCenter + new Vector2(7, 3) * scale, lines[0].Start);
        Assert.Equal(expectedCenter + new Vector2(-7, -3) * scale, lines[0].End);

        game.CurrentLevel.State = LevelState.ExitDelay;
        lines.Clear();
        renderer.DrawEnemyDestructionEffects(
            (start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(8, lines.Count);

        game.RunOneStep();
        Assert.Equal(3, effect.Age);
        game.RunOneStep();
        Assert.Equal(6, effect.Age);
        game.RunOneStep();
        Assert.Equal(9, effect.Age);
        game.RunOneStep();
        Assert.False(effect.IsActive);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void RendererUsesTypeZeroAgeIndexingWithoutMutatingTheEffect()
    {
        TempestGame game = new(startingLives: 100);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        ShootAt(game, flipper.EdgeIndex, () => flipper.Height);
        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        VectorRenderer renderer = new(game);
        Vector2 center = ProjectLane(game, effect.EdgeIndex, effect.Height);
        float scale = 1 / ((game.CurrentLevel.Configuration.Position +
            game.CurrentLevel.Configuration.Depth - effect.Height + Projection.ZOffset) / Projection.K);
        Vector2[] expectedFirstPoints =
        [
            new(7, 3), new(7, 3), new(14, 6), new(28, 12)
        ];

        for (int index = 0; index < expectedFirstPoints.Length; index++)
        {
            int age = index * 3;
            List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
            renderer.DrawEnemyDestructionEffects(
                (start, end, color) => lines.Add((start, end, color)));

            Assert.Equal(age, effect.Age);
            Assert.Equal(8, lines.Count);
            Assert.Equal(center + expectedFirstPoints[index] * scale, lines[0].Start);
            Assert.All(lines, line => Assert.Equal(Color.White, line.Color));
            if (index < expectedFirstPoints.Length - 1)
                game.RunOneStep();
        }
    }

    [Fact]
    public void TankerShotAndMaterializedCargoFlipperShotsEachCreateTypeZeroEffects()
    {
        TempestGame game = CreateTankerWave();
        Tanker tanker = Assert.Single(game.Tankers);
        int tankerLane = tanker.EdgeIndex;
        float tankerHeight = ShootAt(game, tankerLane, () => tanker.Height);

        EnemyDestructionEffect tankerEffect = Assert.Single(game.EnemyDestructionEffects,
            effect => effect.IsActive);
        Assert.Equal(tankerLane, tankerEffect.EdgeIndex);
        Assert.Equal(tankerHeight, tankerEffect.Height);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Equal(2, game.TankerCargoUnitsRemaining);

        Flipper cargo = game.Flippers.First(flipper => flipper.IsTankerCargo);
        int activeEffectsBeforeCargoHit = game.EnemyDestructionEffects.Count(effect => effect.IsActive);
        float cargoHeight = ShootAt(game, cargo.EdgeIndex, () => cargo.Height);

        Assert.Equal(activeEffectsBeforeCargoHit + 1,
            game.EnemyDestructionEffects.Count(effect => effect.IsActive));
        EnemyDestructionEffect cargoEffect = Assert.Single(game.EnemyDestructionEffects,
            effect => effect.IsActive && effect.EdgeIndex == cargo.EdgeIndex);
        Assert.Equal(cargoHeight, cargoEffect.Height);
        Assert.Equal(350, game.Score.Score);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
    }

    [Fact]
    public void FinalEnemyHitKeepsEffectVisibleWithoutChangingExitDelay()
    {
        LevelConfiguration oneEnemy = LevelConfiguration.Level1 with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1,
            MaximumApproachingEggs = 1,
            SpawnLaneSequence = Array.AsReadOnly(new[] { 7 }),
            EggDirectionSequence = Array.AsReadOnly(new[] { 1 })
        };
        TempestGame game = new(oneEnemy, startingLives: 100);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);

        ShootAt(game, flipper.EdgeIndex, () => flipper.Height);

        Assert.Equal(LevelState.ExitDelay, game.State);
        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        Assert.Equal(0, effect.Age);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        new VectorRenderer(game).DrawEnemyDestructionEffects(
            (start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(8, lines.Count);

        for (int tick = 0; tick < 20; tick++)
        {
            game.RunOneStep();
            Assert.Equal(LevelState.ExitDelay, game.State);
        }

        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.False(effect.IsActive);
    }

    [Fact]
    public void SpikerBodyShotCreatesEffectButSpikeHitDoesNot()
    {
        TempestGame game = new(spikerDevelopmentWave: true, startingLives: 100,
            random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        while (spiker.Height < 80)
            game.RunOneStep();

        float spikerHeight = ShootAt(game, spiker.EdgeIndex, () => spiker.Height);

        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        Assert.Equal(spiker.EdgeIndex, effect.EdgeIndex);
        Assert.Equal(spikerHeight, effect.Height);
        Assert.Equal(50, game.Score.Score);

        TempestGame spikeGame = new(spikerDevelopmentWave: true);
        int spikeLane = 2;
        Spike spike = Assert.Single(spikeGame.Spikes, candidate => candidate.EdgeIndex == spikeLane);
        ShootAt(spikeGame, spikeLane, () => spike.Height);
        Assert.DoesNotContain(spikeGame.EnemyDestructionEffects, candidate => candidate.IsActive);
        Assert.Equal(1, spikeGame.Score.Score);
    }

    [Fact]
    public void FuseballShotDoesNotReceiveTheCommonTypeZeroEffect()
    {
        TempestGame game = new(fuseballTest: true, startingLives: 100,
            random: new MinimumRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        for (int tick = 0; tick < 10000 &&
             !(fuseball.Height == Level.Depth &&
               fuseball.State == FuseballState.Lateral &&
               fuseball.W is > 1 and < 7); tick++)
            game.RunOneStep();

        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.InRange(fuseball.W, 2, 6);
        ShootAt(game, fuseball.EdgeIndex, () => fuseball.Height);

        Assert.DoesNotContain(fuseball, game.Fuseballs);
        Assert.DoesNotContain(game.EnemyDestructionEffects, effect => effect.IsActive);
    }

    [Fact]
    public void PulsarShotCreatesTypeZeroEffect()
    {
        TempestGame game = new(pulsarTest: true, startingLives: 100);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Pulsar pulsar = Assert.Single(game.Pulsars);
        int lane = pulsar.EdgeIndex;
        float height = ShootAt(game, lane, () => pulsar.Height);

        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        Assert.Equal(lane, effect.EdgeIndex);
        Assert.Equal(height, effect.Height);
        Assert.Equal(200, game.Score.Score);
    }

    [Fact]
    public void SuperZapperDoesNotCreateNormalBlasterDestructionEffects()
    {
        TempestGame game = new(startingLives: 100);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        for (int tick = 0; tick < 25; tick++)
            game.RunOneStep();

        Assert.DoesNotContain(game.EnemyDestructionEffects, effect => effect.IsActive);
    }

    [Fact]
    public void EnemyBulletShotCreatesEffectWithoutEnemyKillAccounting()
    {
        TempestGame game = CreateTankerWave(enemyBulletsTest: true,
            random: new MaximumRandom());
        Tanker tanker = Assert.Single(game.Tankers);
        for (int tick = 0; tick < 110 && game.EnemyBullets.Count == 0; tick++)
            game.RunOneStep();
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);

        Assert.True(tanker.TryZap(1));
        game.RunOneStep();
        Assert.Empty(game.Tankers);
        Assert.Contains(bullet, game.EnemyBullets);
        int scoreBeforeBullet = game.Score.Score;
        int killsBeforeBullet = game.CurrentLevel.EnemiesDestroyed;
        int lane = bullet.EdgeIndex;
        float height = ShootAt(game, lane, () => bullet.Height);

        EnemyDestructionEffect effect = Assert.Single(game.EnemyDestructionEffects,
            candidate => candidate.IsActive);
        Assert.Equal(lane, effect.EdgeIndex);
        Assert.Equal(height, effect.Height);
        Assert.Equal(scoreBeforeBullet + 25, game.Score.Score);
        Assert.Equal(killsBeforeBullet, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void PlaybackReconstructionRestoresDestructionEffectSlotsAndAges()
    {
        GameplayStartupConfiguration startup = new(1, 100, 1, 1981);
        const int duration = 1800;
        PlayerInputFrame[] frames = Enumerable.Range(0, duration)
            .Select(tick => new PlayerInputFrame(tick,
                new PlayerInput(true, 0, false, false)))
            .ToArray();
        PlayerInputReplay replay = new(startup.RandomSeed, duration, frames,
            startup.ResolveStartingConfiguration());
        GameplayPlaybackSession session = new(startup, replay);
        int effectBoundary = -1;

        while (session.StepForward())
        {
            if (session.Game.EnemyDestructionEffects.Any(effect => effect.IsActive))
            {
                effectBoundary = session.CurrentBoundary;
                break;
            }
        }

        Assert.True(effectBoundary > 0, "The deterministic fire recording should hit an enemy.");
        (int Slot, int Age, int Lane, float Height, EnemyDestructionEffectProgram Program)[] expected =
            GetEffectSnapshot(session.Game);

        GameplayPlaybackSession reconstructed =
            session.ReconstructToBoundary(effectBoundary);

        Assert.Equal(expected, GetEffectSnapshot(reconstructed.Game));
        reconstructed.RunToBoundary(effectBoundary + 5);
        GameplayPlaybackSession scrubbedBack =
            reconstructed.ReconstructToBoundary(effectBoundary);
        Assert.Equal(expected, GetEffectSnapshot(scrubbedBack.Game));
    }

    private static TempestGame CreateTankerWave(
        bool enemyBulletsTest = false, Random? random = null)
    {
        TempestGame game = new(startingLives: 100, random: random,
            enemyBulletsTest: enemyBulletsTest, tankerTest: true);
        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        Assert.Single(game.Tankers);
        return game;
    }

    private static float ShootAt(TempestGame game, int lane, Func<float> targetHeight)
    {
        game.Player.Move(lane - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 6 && game.Shots.Count == 0; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        float height = targetHeight();
        shot.Height = height + 5;
        game.RunOneStep();
        return height;
    }

    private static void MakeReady(Egg egg, Tempest.Game.Levels.Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static Vector2 ProjectLane(TempestGame game, int lane, float height)
    {
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        Vector2 midpoint = (points[lane] + points[(lane + 1) % points.Count]) / 2;
        float depth = game.CurrentLevel.Configuration.Position +
            game.CurrentLevel.Configuration.Depth - height;
        return Projection.Project(midpoint.X, midpoint.Y, depth,
            game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
    }

    private static (int Slot, int Age, int Lane, float Height,
        EnemyDestructionEffectProgram Program)[] GetEffectSnapshot(TempestGame game) =>
        game.EnemyDestructionEffects.Select(effect =>
            (effect.Slot, effect.Age, effect.EdgeIndex, effect.Height, effect.Program)).ToArray();

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
        public override double NextDouble() => 0;
    }

    private sealed class MaximumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => maxValue - 1;
        public override double NextDouble() => .999999;
    }
}
