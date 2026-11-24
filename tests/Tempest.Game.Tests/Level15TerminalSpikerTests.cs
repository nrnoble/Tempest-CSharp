using System.Reflection;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level15TerminalSpikerTests
{
    [Fact]
    public void TerminalSpikerRestartsWithSameIdentityWhileGenericSupplyRemains()
    {
        (TempestGame game, Level3WaveLedger ledger, int originalId, Spiker spiker,
            _, _, _) = CreateScenario();
        int pendingBefore = ledger.PendingGenericOriginalCount;

        Assert.True(pendingBefore > 0);
        Assert.True(ReachPendingRestart(game, spiker));

        Assert.Contains(spiker, game.Spikers);
        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(originalId, spiker.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveSpiker, ledger.GetOriginalState(originalId));
        Assert.DoesNotContain(game.Tankers, tanker => tanker.OriginalSlotId == originalId);
        Assert.Equal(pendingBefore, ledger.PendingGenericOriginalCount);
        ValidateRuntimeRepresentation(game);
    }

    [Fact]
    public void TerminalSpikerConvertsSameIdentityWithTwoOwnedCargoWhenSupplyIsExhausted()
    {
        (TempestGame game, Level3WaveLedger ledger, int originalId, Spiker spiker,
            int originalCount, int cargoCount, _) =
            CreateScenario(resolveRemainingSupply: true);

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.True(ReachTankerConversion(game, originalId));

        AssertConvertedIdentity(game, ledger, originalId, spiker, originalCount, cargoCount);
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void TerminalSpikerConversionBypassesOrdinaryTankerCapacity()
    {
        (TempestGame game, Level3WaveLedger ledger, int originalId, Spiker spiker,
            int originalCount, int cargoCount, _) = CreateScenario(
                activeTankers: Level11WaveSelection.MaximumTankers,
                resolveRemainingSupply: true);

        Assert.Equal(Level11WaveSelection.MaximumTankers, game.Tankers.Count);
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.True(ReachTankerConversion(game, originalId));

        Assert.Equal(Level11WaveSelection.MaximumTankers + 1, game.Tankers.Count);
        AssertConvertedIdentity(game, ledger, originalId, spiker, originalCount, cargoCount);
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void UnrelatedActiveEnemiesDoNotBlockTerminalSpikerConversion()
    {
        (TempestGame game, Level3WaveLedger ledger, int originalId, Spiker spiker,
            int originalCount, int cargoCount, int unrelatedSpikerId) = CreateScenario(
                activeTankers: 1,
                unrelatedSpikers: 1,
                resolveRemainingSupply: true);

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.True(ReachTankerConversion(game, originalId));

        AssertConvertedIdentity(game, ledger, originalId, spiker, originalCount, cargoCount);
        Assert.Contains(game.Spikers, remaining => remaining.OriginalSlotId == unrelatedSpikerId);
        Assert.Single(game.Tankers, tanker => tanker.OriginalSlotId != originalId);
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void ConvertedTankerAndLatentCargoKeepCompletionAndAllRimBlocked()
    {
        (TempestGame game, Level3WaveLedger ledger, int originalId, Spiker spiker,
            int originalCount, int cargoCount, _) =
            CreateScenario(resolveRemainingSupply: true);

        Assert.True(ReachTankerConversion(game, originalId));
        AssertConvertedIdentity(game, ledger, originalId, spiker, originalCount, cargoCount);

        int[] cargoIds = ledger.GetCargoIdsForOriginal(originalId).ToArray();
        Assert.Contains(originalId, ledger.UnresolvedOriginalIds);
        Assert.All(cargoIds, cargoId => Assert.Contains(cargoId, ledger.UnresolvedCargoIds));
        Assert.False(game.CurrentLevel.IsComplete(0));
        Assert.False(TryCompleteAllRim(game));
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(originalId));
        Assert.All(cargoIds, cargoId =>
            Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(cargoId)));
    }

    private static (
        TempestGame Game,
        Level3WaveLedger Ledger,
        int OriginalId,
        Spiker Spiker,
        int OriginalCount,
        int CargoCount,
        int UnrelatedSpikerId) CreateScenario(
            int activeTankers = 0,
            int unrelatedSpikers = 0,
            bool resolveRemainingSupply = false)
    {
        TempestGame game = new(
            LevelConfiguration.Level15Production,
            random: new Random(1981));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        ResolveInitialEggSupply(game, ledger);

        int originalId = ledger.OriginalIdsInReserve().First();
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsSpiker(originalId);

        // DEVELOPMENT-ONLY: place the identified Spiker one update before the return boundary.
        Spiker spiker = new(edgeIndex: 7, speed: 3, originalSlotId: originalId);
        typeof(Enemy).GetProperty(nameof(Enemy.Height))!.SetValue(spiker, 1f);
        typeof(Spiker).GetProperty(nameof(Spiker.State))!
            .SetValue(spiker, SpikerState.Retreating);
        List<Spiker> spikers = GetGameList<Spiker>(game, "spikers");
        spikers.Add(spiker);
        List<object> collisionOrder = GetGameList<object>(game, "developmentCollisionOrder");
        collisionOrder.Add(spiker);

        int unrelatedSpikerId = -1;
        for (int index = 0; index < unrelatedSpikers; index++)
        {
            unrelatedSpikerId = ledger.OriginalIdsInReserve().First();
            ledger.MarkOriginalEgg(unrelatedSpikerId);
            ledger.CommitOriginalAsSpiker(unrelatedSpikerId);
            Spiker unrelated = new(edgeIndex: 8 + index, speed: 1,
                originalSlotId: unrelatedSpikerId);
            spikers.Add(unrelated);
            collisionOrder.Add(unrelated);
        }

        List<Tanker> tankers = GetGameList<Tanker>(game, "tankers");
        for (int index = 0; index < activeTankers; index++)
        {
            int tankerId = ledger.OriginalIdsInReserve().First();
            ledger.MarkOriginalEgg(tankerId);
            ledger.CommitOriginalAsTanker(tankerId);
            Tanker tanker = new(edgeIndex: index, originalSlotId: tankerId);
            tankers.Add(tanker);
            collisionOrder.Add(tanker);
        }

        if (resolveRemainingSupply)
            ResolveRemainingOriginalSupply(ledger);

        return (game, ledger, originalId, spiker, ledger.OriginalCount,
            ledger.CargoCount, unrelatedSpikerId);
    }

    private static void ResolveInitialEggSupply(TempestGame game, Level3WaveLedger ledger)
    {
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            if (ledger.GetOriginalState(id) != Level3OriginalState.Egg)
                continue;

            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        GetGameList<Egg>(game.CurrentLevel, "eggs").Clear();
    }

    private static void ResolveRemainingOriginalSupply(Level3WaveLedger ledger)
    {
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            if (ledger.GetOriginalState(id) != Level3OriginalState.Reserve)
                continue;

            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
    }

    private static bool ReachPendingRestart(TempestGame game, Spiker spiker)
    {
        for (int tick = 0; tick < 20; tick++)
        {
            game.RunOneStep();
            if (spiker.State == SpikerState.Building && spiker.Height == 0)
                return true;
        }

        return false;
    }

    private static bool ReachTankerConversion(TempestGame game, int originalId)
    {
        for (int tick = 0; tick < 20; tick++)
        {
            if (game.Tankers.Any(tanker => tanker.OriginalSlotId == originalId))
                return true;
            game.RunOneStep();
        }

        return game.Tankers.Any(tanker => tanker.OriginalSlotId == originalId);
    }

    private static void AssertConvertedIdentity(
        TempestGame game,
        Level3WaveLedger ledger,
        int originalId,
        Spiker spiker,
        int originalCount,
        int cargoCount)
    {
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(originalId));
        Assert.True(ledger.IsConvertedSpikerTanker(originalId));
        Assert.Single(game.Tankers, tanker => tanker.OriginalSlotId == originalId);
        Assert.DoesNotContain(spiker, game.Spikers);
        Assert.DoesNotContain(game.Spikers, candidate => candidate.OriginalSlotId == originalId);
        Assert.Equal(originalCount, ledger.OriginalCount);
        Assert.Equal(cargoCount + Tanker.CarriedFlipperCount, ledger.CargoCount);

        int[] cargoIds = ledger.GetCargoIdsForOriginal(originalId).ToArray();
        Assert.Equal(Tanker.CarriedFlipperCount, cargoIds.Length);
        Assert.Equal(Tanker.CarriedFlipperCount, cargoIds.Distinct().Count());
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.All(cargoIds, cargoId =>
        {
            Assert.Equal(originalId, ledger.GetCargoOwner(cargoId));
            Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(cargoId));
        });

        ValidateRuntimeRepresentation(game);
    }

    private static void ValidateRuntimeRepresentation(TempestGame game) =>
        typeof(TempestGame).GetMethod(
            "ValidateLevel3WaveRepresentation",
            BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null);

    private static bool TryCompleteAllRim(TempestGame game) =>
        (bool)typeof(TempestGame).GetMethod(
            "TryCompleteAllRim",
            BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(game, null)!;

    private static List<T> GetGameList<T>(object instance, string fieldName)
    {
        FieldInfo field = instance.GetType().GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(instance)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' is null."));
    }
}
