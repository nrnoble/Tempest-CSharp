using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level15ProductionSoakTests
{
    [Theory]
    [InlineData(1981)]
    [InlineData(1982)]
    public void SeededProductionSoakConservesIdentitiesAcross1200Ticks(int seed)
    {
        TempestGame first = new(startingProductionLevel: 15, startingLives: 99, random: new Random(seed));
        TempestGame second = new(startingProductionLevel: 15, startingLives: 99, random: new Random(seed));
        RunSoak(first);
        RunSoak(second);

        Level3WaveLedger firstLedger = first.CurrentLevel.Level3Ledger!;
        Level3WaveLedger secondLedger = second.CurrentLevel.Level3Ledger!;
        Assert.Equal(first.CurrentLevel.EnemiesSpawned, second.CurrentLevel.EnemiesSpawned);
        Assert.Equal(first.CurrentLevel.EnemiesDestroyed, second.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(first.CurrentLevel.EnemiesRimCleared, second.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(Enumerable.Range(0, firstLedger.OriginalCount).Select(firstLedger.GetOriginalState),
            Enumerable.Range(0, secondLedger.OriginalCount).Select(secondLedger.GetOriginalState));
        Assert.Equal(Enumerable.Range(0, firstLedger.OriginalCount).Select(firstLedger.GetOriginalType),
            Enumerable.Range(0, secondLedger.OriginalCount).Select(secondLedger.GetOriginalType));
        Assert.Equal(firstLedger.UnresolvedCargoIds.Order(), secondLedger.UnresolvedCargoIds.Order());
        Assert.Equal(first.Score.Score, second.Score.Score);
        Assert.Equal(first.Player.Lives, second.Player.Lives);
    }

    private static void RunSoak(TempestGame game)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        HashSet<Level3OriginalType> selectedTypes = [];
        Dictionary<int, float> tankerHeights = [];
        bool observedMovement = false;
        bool observedCargo = false;
        bool observedPlayerShot = false;
        bool observedEnemyShot = false;
        int initialSpawned = level.EnemiesSpawned;

        Assert.Same(LevelConfiguration.Level15Production, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Equal(27, ledger.OriginalCount);
        Assert.Equal(level.Configuration.InitialActiveEggCount, game.Eggs.Count);

        for (int tick = 0; tick < 1200; tick++)
        {
            game.RunOneStep(new PlayerInput(tick % 30 == 0, 0, false, false));
            Assert.Same(level, game.CurrentLevel);
            ledger.ValidateState();
            int terminalOriginals = Enumerable.Range(0, ledger.OriginalCount)
                .Count(id => ledger.GetOriginalState(id) is Level3OriginalState.Destroyed or
                    Level3OriginalState.Escaped or Level3OriginalState.RimCleared);
            Assert.Equal(ledger.OriginalCount, ledger.UnresolvedOriginalIds.Count + terminalOriginals);
            Assert.InRange(level.RemainingEnemies, 0, level.EnemySupply);
            Assert.True(level.IsPlayableLane(game.Player.EdgeIndex));
            Assert.All(game.Eggs, egg => Assert.True(level.IsPlayableLane(egg.EdgeIndex)));
            Assert.All(game.Flippers, flipper =>
            {
                Assert.True(level.IsPlayableLane(flipper.EdgeIndex));
                if (!flipper.IsTankerCargo)
                    Assert.Equal(Level3OriginalType.Flipper,
                        ledger.GetOriginalType(flipper.OriginalSlotId));
            });
            Assert.All(game.Spikers, spiker => Assert.True(level.IsPlayableLane(spiker.EdgeIndex)));
            Assert.All(game.Fuseballs, fuseball => Assert.True(level.IsPlayableLane(fuseball.EdgeIndex)));
            Assert.Empty(game.Pulsars);
            foreach (Tanker tanker in game.Tankers)
            {
                Assert.True(level.IsPlayableLane(tanker.EdgeIndex));
                int[] cargo = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
                Assert.Equal(2, cargo.Length);
                Assert.Equal(2, cargo.Distinct().Count());
                observedCargo = true;
                if (tankerHeights.TryGetValue(tanker.OriginalSlotId, out float previousHeight) &&
                    previousHeight != tanker.Height)
                    observedMovement = true;
                tankerHeights[tanker.OriginalSlotId] = tanker.Height;
            }
            for (int id = 0; id < ledger.OriginalCount; id++)
            {
                Level3OriginalType type = ledger.GetOriginalType(id);
                if (type != Level3OriginalType.Unselected)
                    selectedTypes.Add(type);
            }
            observedPlayerShot |= game.Shots.Count > 0;
            observedEnemyShot |= game.EnemyBullets.Count > 0;
        }

        Assert.True(level.EnemiesSpawned > initialSpawned);
        Assert.True(observedMovement);
        Assert.True(observedCargo);
        Assert.True(observedPlayerShot);
        Assert.True(observedEnemyShot);
        Assert.Contains(Level3OriginalType.Tanker, selectedTypes);
        Assert.Contains(Level3OriginalType.Spiker, selectedTypes);
        Assert.Contains(Level3OriginalType.Fuseball, selectedTypes);
        Assert.Contains(Level3OriginalType.Flipper, selectedTypes);
    }
}
