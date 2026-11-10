using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level5AdmissionPolicyTests
{
    [Fact]
    public void SpikerMinimumPrecedesTankerMinimumAndBothAreThenSatisfied()
    {
        Assert.Equal(Level3OriginalType.Spiker,
            Level5AdmissionPolicy.SelectOrdinaryType(Snapshot()));
        Assert.Equal(Level3OriginalType.Spiker,
            Level5AdmissionPolicy.SelectOrdinaryType(Snapshot(activeSpikers: 1)));
        Assert.Equal(Level3OriginalType.Tanker,
            Level5AdmissionPolicy.SelectOrdinaryType(Snapshot(activeSpikers: 2)));
    }

    [Fact]
    public void PostMinimumSelectionUsesTheShortestSpikeThresholdTranslation()
    {
        Level5AdmissionSnapshot belowThreshold = Snapshot(
            convertedTankers: 1,
            activeSpikers: 2,
            shortestSpikeHeight: Level5AdmissionPolicy.ShortestSpikeTankerThreshold - 1);
        Level5AdmissionSnapshot atThreshold = belowThreshold with
        {
            ShortestSpikeHeight = Level5AdmissionPolicy.ShortestSpikeTankerThreshold
        };

        Assert.Equal(new[]
        {
            Level3OriginalType.Tanker,
            Level3OriginalType.Spiker
        }, Level5AdmissionPolicy.GetOrdinaryEligibleTypes(belowThreshold));
        Assert.Equal(Level3OriginalType.Tanker,
            Level5AdmissionPolicy.SelectOrdinaryType(belowThreshold));
        Assert.Equal(Level3OriginalType.Spiker,
            Level5AdmissionPolicy.SelectOrdinaryType(atThreshold));
    }

    [Fact]
    public void ExhaustedOrdinaryTankersAndSpikersDoNotSelectAvailableFlippers()
    {
        Level5AdmissionSnapshot exhausted = Snapshot(
            ordinaryTankers: 1,
            activeSpikers: 3,
            latentTankerCargoFlippers: 2);

        Assert.True(Level5AdmissionPolicy.GetFlipperAvailability(exhausted) > 0);
        Assert.Empty(Level5AdmissionPolicy.GetOrdinaryEligibleTypes(exhausted));
        Assert.Null(Level5AdmissionPolicy.SelectOrdinaryType(exhausted));
    }

    [Fact]
    public void TankerCargoReservesTwoFlipperAvailabilityUnitsButNoPhysicalSlots()
    {
        Level5AdmissionSnapshot oneTanker = Snapshot(
            ordinaryTankers: 1,
            latentTankerCargoFlippers: 2);

        Assert.Equal(1, oneTanker.ActiveEnemyCount);
        Assert.Equal(6, Level5AdmissionPolicy.GetOpenPhysicalSlots(oneTanker));
        Assert.Equal(3, Level5AdmissionPolicy.GetFlipperAvailability(oneTanker));

        Level5AdmissionSnapshot wrapped = oneTanker with
        {
            OrdinaryTankers = 0,
            ConvertedTankers = 3,
            LatentTankerCargoFlippers = 6
        };
        Assert.Equal(4, Level5AdmissionPolicy.GetOpenPhysicalSlots(wrapped));
        Assert.Equal(4, Level5AdmissionPolicy.GetFlipperAvailability(wrapped));
    }

    [Fact]
    public void PhysicalCapacityCountsAllLiveRepresentationsAndBlocksAtSeven()
    {
        Level5AdmissionSnapshot full = Snapshot(
            ordinaryFlippers: 1,
            releasedCargoFlippers: 1,
            pendingShotCargoFlippers: 1,
            ordinaryTankers: 1,
            convertedTankers: 1,
            activeSpikers: 1,
            activeOtherEnemies: 1);

        Assert.Equal(7, full.ActiveEnemyCount);
        Assert.Equal(0, Level5AdmissionPolicy.GetOpenPhysicalSlots(full));
        Assert.Null(Level5AdmissionPolicy.SelectOrdinaryType(full));
        Assert.False(Level5AdmissionPolicy.CanAdmitRetainedOriginal(
            full, Level3OriginalType.Flipper, convertedTanker: false));
    }

    [Fact]
    public void CargoPairOverflowBlocksFurtherAdmissionWithoutClampingCommittedChildren()
    {
        Level5AdmissionSnapshot overflow = Snapshot(releasedCargoFlippers: 8);

        Assert.Equal(0, Level5AdmissionPolicy.GetOpenPhysicalSlots(overflow));
        Assert.Null(Level5AdmissionPolicy.SelectOrdinaryType(overflow));
        Assert.Equal(1, Level5AdmissionPolicy.GetOpenPhysicalSlots(
            overflow with { ReleasedCargoFlippers = 6 }));
    }

    [Fact]
    public void BlockedReadyEggRetainsItsIdentityAndCanProceedWhenAdmissionReopens()
    {
        Level level = new(LevelConfiguration.Level5Production);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg egg = level.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);

        int constructionCalls = 0;
        List<Tempest.Game.Gameplay.Enemy> hatched = [];
        Level5AdmissionSnapshot full = Snapshot(activeOtherEnemies: 7);
        for (int tick = 0; tick < 3; tick++)
        {
            level.HatchReadyMixedEnemies(
                readyEgg =>
                {
                    if (Level5AdmissionPolicy.SelectOrdinaryType(full) is null)
                        return null;

                    constructionCalls++;
                    ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                    return new Tempest.Game.Gameplay.Spiker(
                        readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
                },
                hatched.Add);
        }

        Assert.Equal(EggState.ReadyToHatch, egg.State);
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(20, ledger.PendingGenericOriginalCount);
        Assert.Empty(hatched);
        Assert.Equal(0, constructionCalls);

        Level5AdmissionSnapshot reopened = Snapshot();
        level.HatchReadyMixedEnemies(
            readyEgg =>
            {
                Level3OriginalType selected =
                    Level5AdmissionPolicy.SelectOrdinaryType(reopened)!.Value;
                Assert.Equal(Level3OriginalType.Spiker, selected);
                ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                return new Tempest.Game.Gameplay.Spiker(
                    readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
            },
            hatched.Add);

        Assert.DoesNotContain(level.Eggs, candidate =>
            candidate.OriginalSlotId == egg.OriginalSlotId);
        Assert.Single(hatched);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(19, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void RetainedConvertedTankerReentryUsesHeadroomNotOrdinaryMaximum()
    {
        Level5AdmissionSnapshot atTankerMaximum = Snapshot(
            ordinaryTankers: 1,
            activeSpikers: 1);

        Assert.True(Level5AdmissionPolicy.CanAdmitRetainedOriginal(
            atTankerMaximum, Level3OriginalType.Tanker, convertedTanker: true));
        Assert.False(Level5AdmissionPolicy.CanAdmitRetainedOriginal(
            atTankerMaximum, Level3OriginalType.Tanker, convertedTanker: false));
        Assert.False(Level5AdmissionPolicy.CanAdmitRetainedOriginal(
            Snapshot(activeOtherEnemies: 7),
            Level3OriginalType.Tanker,
            convertedTanker: true));
    }

    [Fact]
    public void TerminalConversionIsCanonicalLevelFiveOnlyAndPreservesOneCargoPairAcrossReentry()
    {
        Assert.True(Level5AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level5Production, pendingGenericOriginals: 0));
        Assert.False(Level5AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level5Production, pendingGenericOriginals: 1));
        Assert.False(Level5AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level5Production with { },
            pendingGenericOriginals: 0));
        Assert.False(Level5AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level6Production,
            pendingGenericOriginals: 0));

        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        ledger.ConvertActiveSpikerToTanker(0);
        int[] cargoIds = ledger.GetCargoIdsForOriginal(0).ToArray();

        Assert.True(ledger.IsConvertedSpikerTanker(0));
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.Equal(2, cargoIds.Length);
        Assert.Throws<InvalidOperationException>(
            () => ledger.ConvertActiveSpikerToTanker(0));

        ledger.RebuildAfterDeath();
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        Assert.True(ledger.IsConvertedSpikerTanker(0));
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(cargoIds, ledger.GetCargoIdsForOriginal(0));
        Assert.Equal(2, ledger.CargoCount);
        ledger.ValidateState();
    }

    [Fact]
    public void GenericPendingSupplyCountsOnlyOriginalReserveAndEggStates()
    {
        Level3WaveLedger ledger = new(originalCount: 3);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        Assert.Equal(2, ledger.PendingGenericOriginalCount);

        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsTanker(1);
        Assert.Equal(1, ledger.PendingGenericOriginalCount);
        Assert.Equal(2, ledger.CargoCount);

        ledger.ReturnSpikerToReserve(0);
        Assert.Equal(2, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void EightReconstructedCargoObligationsUseOrdinaryTankerAndSpikerSelection()
    {
        Level3WaveLedger ledger = new(originalCount: 4);
        for (int ownerId = 0; ownerId < 4; ownerId++)
        {
            ledger.MarkOriginalEgg(ownerId);
            ledger.CommitOriginalAsTanker(ownerId);
            ledger.MaterializeLiveCargo(ownerId);
            ledger.MarkOriginalEscaped(ownerId);
        }

        ledger.RebuildAfterDeath(clearUnresolvedSpeciesSelections: true);
        int[] genericIds = ledger.UnresolvedOriginalIds.Order().ToArray();
        int nextId = 0;
        int activeTankers = 0;
        List<int> activeSpikers = [];
        HashSet<Level3OriginalType> selectedTypes = [];

        while (nextId < genericIds.Length)
        {
            Level5AdmissionSnapshot snapshot = Snapshot(
                ordinaryTankers: activeTankers,
                activeSpikers: activeSpikers.Count,
                shortestSpikeHeight: null);
            Level3OriginalType? selectedType =
                Level5AdmissionPolicy.SelectOrdinaryType(snapshot);
            if (selectedType is null)
            {
                int resolvedSpikerId = activeSpikers[0];
                ledger.MarkOriginalDestroyed(resolvedSpikerId);
                activeSpikers.RemoveAt(0);
                continue;
            }

            int genericId = genericIds[nextId++];
            ledger.MarkOriginalEgg(genericId);
            if (selectedType == Level3OriginalType.Tanker)
            {
                ledger.CommitOriginalAsTanker(genericId);
                activeTankers++;
            }
            else
            {
                Assert.Equal(Level3OriginalType.Spiker, selectedType.Value);
                ledger.CommitOriginalAsSpiker(genericId);
                activeSpikers.Add(genericId);
            }

            selectedTypes.Add(selectedType.Value);
            Assert.Contains(genericId, ledger.UnresolvedOriginalIds);
        }

        Assert.Contains(Level3OriginalType.Tanker, selectedTypes);
        Assert.Contains(Level3OriginalType.Spiker, selectedTypes);
        Assert.DoesNotContain(Level3OriginalType.Flipper, selectedTypes);
        Assert.Equal(1, activeTankers);
        Assert.Equal(3, activeSpikers.Count);
        Assert.Equal(2, ledger.UnresolvedCargoIds.Count);
        Assert.All(activeSpikers, id =>
            Assert.Contains(id, ledger.UnresolvedOriginalIds));
        ledger.ValidateState();
    }

    private static Level5AdmissionSnapshot Snapshot(
        int ordinaryFlippers = 0,
        int releasedCargoFlippers = 0,
        int pendingShotCargoFlippers = 0,
        int ordinaryTankers = 0,
        int convertedTankers = 0,
        int activeSpikers = 0,
        int activeOtherEnemies = 0,
        int latentTankerCargoFlippers = 0,
        float? shortestSpikeHeight = 60) =>
        new(ordinaryFlippers, releasedCargoFlippers, pendingShotCargoFlippers,
            ordinaryTankers, convertedTankers, activeSpikers, activeOtherEnemies,
            latentTankerCargoFlippers, shortestSpikeHeight);
}
