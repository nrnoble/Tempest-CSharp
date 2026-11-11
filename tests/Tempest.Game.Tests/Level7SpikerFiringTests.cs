using System.Reflection;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level7SpikerFiringTests
{
    [Fact]
    public void FirstAscentCanFireWithoutGrowthOrRetreat()
    {
        SpikerFiringRandom random = new([255]);
        TempestGame game = CreateGame(random, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 390;

        game.RunOneStep();
        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Empty(game.EnemyBullets);

        game.RunOneStep();

        Assert.Same(spiker, Assert.Single(game.Spikers));
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(spiker.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(spiker.Height, bullet.Height);
        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(0, spiker.Growth);
        Assert.Equal(390, spike.Height);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(1, random.FiringRolls);
    }

    [Fact]
    public void CalibratedSpikerRetainsTheExistingFirstAscentFiringHoldoff()
    {
        SpikerFiringRandom random = new([255]);
        TempestGame game = CreateCalibratedGame(random, 2.8125f, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 390;

        game.RunOneStep();
        game.RunOneStep();

        Assert.Equal(2.8125f, spiker.Speed);
        Assert.Equal(SpikerState.Building, spiker.State);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(spiker.Height, bullet.Height);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(1, random.FiringRolls);
    }

    [Fact]
    public void NormalLevel7SpikerFiresAgainDuringItsSecondAscent()
    {
        SpikerFiringRandom random = new([255], defaultFiringRoll: 0);
        TempestGame game = CreateDefaultProductionGame(random, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        int pendingBefore = game.CurrentLevel.PendingGenericEnemyUnits;

        bool crossedGrowthThreshold = false;
        for (int tick = 0; tick < 300 && spiker.State != SpikerState.ReturnedToReserve; tick++)
        {
            spiker.Update(game.CurrentLevel, () => true, allowGrowthRetreatShot: false);
            crossedGrowthThreshold |= spiker.Growth > 100;
        }

        Assert.Equal(Tanker.DevelopmentClimbSpeed, spiker.Speed);
        Assert.Equal(SpikerState.ReturnedToReserve, spiker.State);
        Assert.True(crossedGrowthThreshold);
        spike.Height = 390;

        game.RunOneStep();
        game.RunOneStep();

        Assert.Same(spiker, Assert.Single(game.Spikers));
        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(Tanker.DevelopmentClimbSpeed, spiker.Height);
        Assert.Single(game.EnemyBullets);
        Assert.Equal(1, random.FiringRolls);
        Assert.Equal(pendingBefore, game.CurrentLevel.PendingGenericEnemyUnits);

        for (int tick = 0; tick < 300 && spiker.State == SpikerState.Building; tick++)
            game.RunOneStep();

        Assert.Equal(SpikerState.Retreating, spiker.State);
        Assert.Equal(pendingBefore, game.CurrentLevel.PendingGenericEnemyUnits);
    }

    [Fact]
    public void SpikerAndTankerShotsShareEnemyBulletMovement()
    {
        SpikerFiringRandom random = new([255]);
        TempestGame game = CreateGame(random, out Spiker spiker);
        game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex).Height = 390;
        game.RunOneStep();
        game.RunOneStep();
        EnemyBullet spikerShot = Assert.Single(game.EnemyBullets);

        Tanker tanker = new(edgeIndex: spiker.EdgeIndex, originalSlotId: 0,
            bulletAmmunition: 1, bulletTimer: 0);
        Assert.Equal(TankerUpdateResult.Active,
            tanker.UpdateForDevelopment(static (_, _) => 100, out EnemyBullet? tankerShot));
        Assert.NotNull(tankerShot);
        Assert.IsType<EnemyBullet>(spikerShot);
        Assert.IsType<EnemyBullet>(tankerShot);

        float initialSpikerShotHeight = spikerShot.Height;
        float initialTankerShotHeight = tankerShot.Height;
        for (int tick = 0; tick < 10; tick++)
        {
            spikerShot.Advance();
            tankerShot.Advance();
        }

        Assert.Equal(30, spikerShot.Height - initialSpikerShotHeight);
        Assert.Equal(30, tankerShot.Height - initialTankerShotHeight);
    }

    [Fact]
    public void SameSpikerCanFireOnSecondAscentWithHighPersistentSpike()
    {
        SpikerFiringRandom random = new([255], defaultFiringRoll: 0);
        TempestGame game = CreateGame(random, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        int pendingBefore = game.CurrentLevel.PendingGenericEnemyUnits;

        bool crossedGrowthThreshold = false;
        for (int tick = 0; tick < 300 && spiker.State != SpikerState.ReturnedToReserve; tick++)
        {
            spiker.Update(game.CurrentLevel, () => true, allowGrowthRetreatShot: false);
            crossedGrowthThreshold |= spiker.Growth > 100;
        }

        Assert.Equal(SpikerState.ReturnedToReserve, spiker.State);
        Assert.True(crossedGrowthThreshold);
        spike.Height = 390;

        game.RunOneStep();

        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(0, spiker.Height);
        Assert.Same(spiker, Assert.Single(game.Spikers));
        Assert.Equal(pendingBefore, game.CurrentLevel.PendingGenericEnemyUnits);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, random.FiringRolls);

        game.RunOneStep();

        Assert.Same(spiker, Assert.Single(game.Spikers));
        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(3, spiker.Height);
        Assert.Equal(0, spiker.Growth);
        Assert.Equal(390, spike.Height);
        Assert.Single(game.EnemyBullets);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(1, random.FiringRolls);
        Assert.Equal(pendingBefore, game.CurrentLevel.PendingGenericEnemyUnits);

        for (int tick = 0; tick < 200 && spiker.State == SpikerState.Building; tick++)
            game.RunOneStep();

        Assert.Equal(SpikerState.Retreating, spiker.State);
        Assert.Equal(6, spiker.Growth);
        Assert.Equal(392, spike.Height);
        Assert.Equal(pendingBefore, game.CurrentLevel.PendingGenericEnemyUnits);
    }

    [Fact]
    public void RandomRejectionDoesNotSilenceSpikerAndCooldownReloadsOnlyAfterShot()
    {
        SpikerFiringRandom random = new([0, 255, 255]);
        TempestGame game = CreateGame(random, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 390;
        AddEnemyBullet(game, new EnemyBullet(10, 0));

        game.RunOneStep();
        game.RunOneStep();

        Assert.Single(game.EnemyBullets);
        Assert.Equal(0, spiker.ShotDelayTicks);
        Assert.Equal(1, random.FiringRolls);

        game.RunOneStep();

        Assert.Equal(2, game.EnemyBullets.Count);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(2, random.FiringRolls);

        for (int tick = 0; tick < 62; tick++)
            game.RunOneStep();

        Assert.Equal(0, spiker.ShotDelayTicks);
        Assert.Equal(2, random.FiringRolls);
        Assert.Equal(2, game.EnemyBullets.Count);

        game.RunOneStep();

        Assert.Equal(3, game.EnemyBullets.Count);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(3, random.FiringRolls);
    }

    [Fact]
    public void FullShotCapacityConsumesNoRandomnessAndReadySpikerFiresAfterSlotOpens()
    {
        SpikerFiringRandom random = new([255]);
        TempestGame game = CreateGame(random, out Spiker spiker);
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 390;
        EnemyBullet[] pressureBullets =
        [
            new EnemyBullet(10, 0),
            new EnemyBullet(11, 0),
            new EnemyBullet(12, 0)
        ];
        foreach (EnemyBullet bullet in pressureBullets)
            AddEnemyBullet(game, bullet);

        game.RunOneStep();
        game.RunOneStep();

        Assert.Equal(3, game.EnemyBullets.Count);
        Assert.Equal(0, spiker.ShotDelayTicks);
        Assert.Equal(0, random.FiringRolls);

        RemoveEnemyBullet(game, pressureBullets[0]);
        game.RunOneStep();

        Assert.Equal(3, game.EnemyBullets.Count);
        Assert.Equal(62, spiker.ShotDelayTicks);
        Assert.Equal(1, random.FiringRolls);
    }

    [Fact]
    public void RetreatingSpikerDoesNotUseTheAscendingPhaseFiringTranslation()
    {
        SpikerFiringRandom random = new([]);
        TempestGame game = CreateGame(random, out Spiker spiker);

        while (spiker.State == SpikerState.Initializing || spiker.State == SpikerState.Building)
            spiker.Update(game.CurrentLevel, () => true, allowGrowthRetreatShot: false);

        Assert.Equal(SpikerState.Retreating, spiker.State);
        game.RunOneStep();

        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, random.FiringRolls);
    }

    [Fact]
    public void SpikerFiringPolicyIsLimitedToCanonicalLevel7()
    {
        Assert.True(LevelConfiguration.Level7Production.UsesLevel7SpikerFiringPolicy);
        Assert.False((LevelConfiguration.Level7Production with { }).UsesLevel7SpikerFiringPolicy);
        Assert.False(LevelConfiguration.Level6Production.UsesLevel7SpikerFiringPolicy);
        Assert.False(LevelConfiguration.Level8Production.UsesLevel7SpikerFiringPolicy);
    }

    private static TempestGame CreateGame(SpikerFiringRandom random, out Spiker spiker)
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: random);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int originalId = ledger.OriginalIdsInReserve().Last();
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsSpiker(originalId);
        spiker = new Spiker(edgeIndex: 1, speed: 3, originalSlotId: originalId);
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);
        return game;
    }

    private static TempestGame CreateCalibratedGame(
        SpikerFiringRandom random, float speed, out Spiker spiker)
    {
        TempestGame game = new(startingProductionLevel: 7, random: random,
            level7SpikerSpeedOverride: speed);
        Egg egg = game.Eggs[0];
        game.CurrentLevel.Level3Ledger!.CommitOriginalAsSpiker(egg.OriginalSlotId);
        MethodInfo factory = typeof(TempestGame).GetMethod(
            "CreateLevel3MixedEnemy", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The mixed-wave enemy factory was not found.");
        spiker = Assert.IsType<Spiker>(factory.Invoke(game, [egg]));
        List<Egg> eggs = (List<Egg>)(typeof(Level).GetField(
            "eggs", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(game.CurrentLevel)
            ?? throw new InvalidOperationException("The Level Egg collection was not found."));
        Assert.True(eggs.Remove(egg));
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);
        return game;
    }

    private static TempestGame CreateDefaultProductionGame(
        SpikerFiringRandom random, out Spiker spiker)
    {
        TempestGame game = new(startingProductionLevel: 7, random: random);
        Egg egg = game.Eggs[0];
        game.CurrentLevel.Level3Ledger!.CommitOriginalAsSpiker(egg.OriginalSlotId);
        MethodInfo factory = typeof(TempestGame).GetMethod(
            "CreateLevel3MixedEnemy", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The mixed-wave enemy factory was not found.");
        spiker = Assert.IsType<Spiker>(factory.Invoke(game, [egg]));
        List<Egg> eggs = (List<Egg>)(typeof(Level).GetField(
            "eggs", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(game.CurrentLevel)
            ?? throw new InvalidOperationException("The Level Egg collection was not found."));
        Assert.True(eggs.Remove(egg));
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);
        return game;
    }

    private static void AddEnemyBullet(TempestGame game, EnemyBullet bullet)
    {
        GetGameList<EnemyBullet>(game, "enemyBullets").Add(bullet);
        GetGameList<object>(game, "developmentCollisionOrder").Add(bullet);
    }

    private static void RemoveEnemyBullet(TempestGame game, EnemyBullet bullet)
    {
        Assert.True(GetGameList<EnemyBullet>(game, "enemyBullets").Remove(bullet));
        Assert.True(GetGameList<object>(game, "developmentCollisionOrder").Remove(bullet));
    }

    private static List<T> GetGameList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' is null."));
    }

    private sealed class SpikerFiringRandom(
        IEnumerable<int> firingRolls, int defaultFiringRoll = 255) : Random
    {
        private readonly Queue<int> scriptedRolls = new(firingRolls);

        public int FiringRolls { get; private set; }

        public override int Next(int minValue, int maxValue)
        {
            if (minValue != 0 || maxValue != 256)
                return minValue;

            FiringRolls++;
            return scriptedRolls.TryDequeue(out int roll) ? roll : defaultFiringRoll;
        }

        public override double NextDouble() => 1;
    }
}
