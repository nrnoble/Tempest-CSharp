using System.Reflection;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level11ProductionCorrectionTests
{
    [Fact]
    public void LevelElevenInitialSpikeLengthUsesRawB0TipCoordinateTranslation()
    {
        Assert.Equal(0xB0, Level11WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal(64, 0xF0 - Level11WaveSelection.StartingSpikeHeightRaw);
        Assert.Equal(224, 0xF0 - 0x10);
        Assert.Equal(114.285714f,
            (0xF0 - Level11WaveSelection.StartingSpikeHeightRaw) *
            Level.Depth / (0xF0 - 0x10),
            precision: 5);
        Assert.Equal(114.285714f, LevelConfiguration.Level11StartingSpikeHeight, precision: 5);
        Assert.Equal(LevelConfiguration.Level11StartingSpikeHeight,
            LevelConfiguration.Level11Production.StartingSpikeHeight);

        Level level = new(LevelConfiguration.Level11Production);
        Assert.Equal(LevelConfiguration.Level11Production.PlayableLaneCount, level.Spikes.Count);
        Assert.All(level.Spikes,
            spike => Assert.Equal(LevelConfiguration.Level11StartingSpikeHeight, spike.Height,
                precision: 5));
        Assert.DoesNotContain(level.Spikes,
            spike => !level.IsPlayableLane(spike.EdgeIndex));

        Assert.Equal(71.42857f, LevelConfiguration.Level8StartingSpikeHeight, precision: 4);
        Assert.Equal(85.7143f, LevelConfiguration.Level9StartingSpikeHeight, precision: 3);
    }

    [Fact]
    public void LevelElevenSpikerRepeatsWhileGenericPendingSupplyRemains()
    {
        TempestGame game = new(startingProductionLevel: 11);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.True(ledger.PendingGenericOriginalCount > 0);

        int originalId = ledger.OriginalIdsInReserve().First();
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsSpiker(originalId);
        Spiker spiker = new(edgeIndex: 7, speed: 3, originalSlotId: originalId);
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);
        ResolveInitialEggSupply(ledger);
        RemoveAllEggs(game);

        int pendingBefore = ledger.PendingGenericOriginalCount;
        for (int tick = 0; tick < 20; tick++)
            game.RunOneStep();

        Assert.DoesNotContain(game.Tankers, _ => true);
        Assert.Single(game.Spikers);
        Assert.Equal(originalId, game.Spikers[0].OriginalSlotId);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(originalId));
        Assert.True(pendingBefore > 0);
        Assert.True(ledger.PendingGenericOriginalCount > 0 ||
            game.Spikers.Count == 1);
    }

    [Fact]
    public void LevelElevenTerminalSpikerConvertsInPlaceWithTwoCargoAndNoPendingLoss()
    {
        TempestGame game = new(startingProductionLevel: 11);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        int originalId = ledger.OriginalIdsInReserve().First();
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsSpiker(originalId);
        ResolveOtherOriginalSupply(ledger, originalId);

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Spiker spiker = new(edgeIndex: 7, speed: 3, originalSlotId: originalId);
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);
        RemoveAllEggs(game);

        for (int tick = 0; tick < 500 && game.Tankers.Count == 0; tick++)
            game.RunOneStep();

        Assert.DoesNotContain(game.Spikers, _ => true);
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(originalId, tanker.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(originalId));
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(originalId).Count);
        Assert.All(ledger.GetCargoIdsForOriginal(originalId),
            cargoId => Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(cargoId)));
    }

    [Fact]
    public void LevelElevenTerminalConversionIgnoresOrdinaryTankerCapacityCaps()
    {
        TempestGame game = new(startingProductionLevel: 11);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int originalId = ledger.OriginalIdsInReserve().First();
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsSpiker(originalId);
        ResolveOtherOriginalSupply(ledger, originalId);

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        ledger.ConvertActiveSpikerToTanker(originalId);

        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(originalId));
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(originalId).Count);
    }

    private static void ResolveOtherOriginalSupply(Level3WaveLedger ledger, int keptSpikerId)
    {
        for (int otherId = 0; otherId < ledger.OriginalCount; otherId++)
        {
            if (otherId == keptSpikerId)
                continue;
            if (ledger.GetOriginalState(otherId) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(otherId);
            else if (ledger.GetOriginalState(otherId) != Level3OriginalState.Egg)
                continue;
            ledger.CommitOriginalAsFlipper(otherId);
            ledger.MarkOriginalDestroyed(otherId);
        }
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    private static void ResolveInitialEggSupply(Level3WaveLedger ledger)
    {
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            if (ledger.GetOriginalState(id) != Level3OriginalState.Egg)
                continue;
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }
    }

    private static void RemoveAllEggs(TempestGame game)
    {
        FieldInfo field = typeof(Level).GetField(
            "eggs", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Level Egg collection was not found.");
        List<Egg> eggs = (List<Egg>)(field.GetValue(game.CurrentLevel)
            ?? throw new InvalidOperationException("The Level Egg collection is null."));
        eggs.Clear();
    }

    private static List<T> GetGameList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' is null."));
    }
}
