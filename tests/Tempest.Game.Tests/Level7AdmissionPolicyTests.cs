using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level7AdmissionPolicyTests
{
    [Fact]
    public void SpeciesMinimaMaximaAndZeroMinimumExclusionsAreLevelSpecific()
    {
        Assert.Equal(0, Level7WaveSelection.FlipperMinimum);
        Assert.Equal(5, Level7WaveSelection.FlipperMaximum);
        Assert.Equal(1, Level7WaveSelection.TankerMinimum);
        Assert.Equal(2, Level7WaveSelection.TankerMaximum);
        Assert.Equal(2, Level7WaveSelection.SpikerMinimum);
        Assert.Equal(4, Level7WaveSelection.SpikerMaximum);

        Assert.Equal(new[] { Level3OriginalType.Spiker },
            Level7WaveSelection.GetOrdinaryEligibleTypes(Snapshot()));
        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOrdinaryType(
                Snapshot(), (_, _) => throw new InvalidOperationException()));

        Level7AdmissionSnapshot twoSpikers = Snapshot(activeSpikers: 2);
        Assert.Equal(new[] { Level3OriginalType.Tanker },
            Level7WaveSelection.GetOrdinaryEligibleTypes(twoSpikers));
        Assert.Equal(Level3OriginalType.Tanker,
            Level7WaveSelection.SelectOrdinaryType(
                twoSpikers, (_, _) => throw new InvalidOperationException()));

        Level7AdmissionSnapshot satisfiedMinima = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2);
        Assert.Equal(
            new[] { Level3OriginalType.Tanker, Level3OriginalType.Spiker },
            Level7WaveSelection.GetOrdinaryEligibleTypes(satisfiedMinima));
        Assert.DoesNotContain(Level3OriginalType.Flipper,
            Level7WaveSelection.GetOrdinaryEligibleTypes(satisfiedMinima));
        Assert.All(Level7WaveSelection.GetOrdinaryEligibleTypes(satisfiedMinima),
            type => Assert.Contains(type,
                new[] { Level3OriginalType.Tanker, Level3OriginalType.Spiker }));

        Assert.Equal(0, Level7WaveSelection.GetTankerAvailability(
            Snapshot(ordinaryTankers: 2, activeSpikers: 2)));
        Assert.Equal(0, Level7WaveSelection.GetSpikerAvailability(
            Snapshot(ordinaryTankers: 1, activeSpikers: 4)));
    }

    [Fact]
    public void PhysicalHeadroomCountsLiveObjectsButNotLatentCargo()
    {
        Level7AdmissionSnapshot sevenLiveObjects = Snapshot(
            ordinaryFlippers: 1,
            releasedCargoFlippers: 1,
            pendingShotCargoFlippers: 1,
            ordinaryTankers: 1,
            convertedTankers: 1,
            activeSpikers: 1,
            activeOtherEnemies: 1,
            latentTankerCargoFlippers: 4);

        Assert.Equal(7, Level7WaveSelection.PhysicalEnemySlotCount);
        Assert.Equal(7, sevenLiveObjects.ActiveEnemyCount);
        Assert.Equal(0, Level7WaveSelection.GetOpenPhysicalSlots(sevenLiveObjects));
        Assert.Null(Level7WaveSelection.SelectOrdinaryType(
            sevenLiveObjects, (_, _) => throw new InvalidOperationException()));
        Assert.Equal(7, Level7WaveSelection.GetOpenPhysicalSlots(Snapshot()));
    }

    [Fact]
    public void TankerCargoCapacityUsesCanonicalLevel7PhysicalHeadroomOnly()
    {
        Level7AdmissionSnapshot twoOpenSlots = Snapshot(
            ordinaryTankers: 1, activeSpikers: 4);
        Level7AdmissionSnapshot oneOpenSlot = Snapshot(
            ordinaryTankers: 1, activeSpikers: 4, releasedCargoFlippers: 1);

        Assert.Equal(2, Level7WaveSelection.GetTankerCargoChildCapacity(
            LevelConfiguration.Level7Production, twoOpenSlots));
        Assert.Equal(1, Level7WaveSelection.GetTankerCargoChildCapacity(
            LevelConfiguration.Level7Production, oneOpenSlot));

        foreach (LevelConfiguration configuration in new[]
                 {
                     LevelConfiguration.Level1,
                     LevelConfiguration.Level2Square,
                     LevelConfiguration.Level3Mixed,
                     LevelConfiguration.Level4Candidate,
                     LevelConfiguration.Level5Production,
                     LevelConfiguration.Level6Production,
                     LevelConfiguration.Level8Production,
                     LevelConfiguration.Level7Production with { }
                 })
            Assert.Equal(2, Level7WaveSelection.GetTankerCargoChildCapacity(
                configuration, oneOpenSlot));
    }

    [Fact]
    public void LatentTankerCargoReducesFlipperAndTankerAvailabilityWithoutExtraPlaces()
    {
        Level7AdmissionSnapshot oneTanker = Snapshot(
            ordinaryTankers: 1, latentTankerCargoFlippers: 2);

        Assert.Equal(1, oneTanker.ActiveEnemyCount);
        Assert.Equal(6, Level7WaveSelection.GetOpenPhysicalSlots(oneTanker));
        Assert.Equal(3, Level7WaveSelection.GetFlipperAvailability(oneTanker));
        Assert.Equal(1, Level7WaveSelection.GetTankerAvailability(oneTanker));

        Level7AdmissionSnapshot noCargoAvailability = Snapshot(
            ordinaryTankers: 0, latentTankerCargoFlippers: 5);
        Assert.Equal(0, Level7WaveSelection.GetFlipperAvailability(noCargoAvailability));
        Assert.Equal(0, Level7WaveSelection.GetTankerAvailability(noCargoAvailability));

        Level7AdmissionSnapshot convertedBeyondOrdinaryMaximum = Snapshot(
            ordinaryTankers: 1, convertedTankers: 3, activeSpikers: 2,
            latentTankerCargoFlippers: 6);
        Assert.Equal(1, Level7WaveSelection.GetTankerAvailability(
            convertedBeyondOrdinaryMaximum));
    }

    [Fact]
    public void OpenPlaceSelectorDeadEndKeepsPositiveZeroMinimumFlipperAvailabilityBlocked()
    {
        Level7AdmissionSnapshot deadEnd = Snapshot(
            ordinaryTankers: 2,
            activeSpikers: 4,
            latentTankerCargoFlippers: 4,
            pendingSegmentSpikeRomComparisonValue: null);

        Assert.Equal(6, deadEnd.ActiveEnemyCount);
        Assert.Equal(1, Level7WaveSelection.GetOpenPhysicalSlots(deadEnd));
        Assert.Equal(1, Level7WaveSelection.GetFlipperAvailability(deadEnd));
        Assert.Equal(0, Level7WaveSelection.GetTankerAvailability(deadEnd));
        Assert.Equal(0, Level7WaveSelection.GetSpikerAvailability(deadEnd));
        Assert.Empty(Level7WaveSelection.GetOrdinaryEligibleTypes(deadEnd));

        int randomCalls = 0;
        Assert.Null(Level7WaveSelection.SelectOrdinaryType(deadEnd, (_, _) =>
        {
            randomCalls++;
            return 0;
        }));
        Assert.Equal(0, randomCalls);

        Level7AdmissionSnapshot reopened = Snapshot(
            ordinaryTankers: 1, activeSpikers: 4,
            latentTankerCargoFlippers: 2);
        Assert.Equal(Level3OriginalType.Tanker,
            Level7WaveSelection.SelectOrdinaryType(
                reopened, (_, _) => throw new InvalidOperationException()));
    }

    [Fact]
    public void PendingEntryLaneSpikeAndRomThresholdSelectTheOrdinarySpecies()
    {
        Level level = new(LevelConfiguration.Level7Production);
        level.Spikes.Single(spike => spike.EdgeIndex == 2).Height = 60;
        level.Spikes.Single(spike => spike.EdgeIndex == 10).Height = 120;
        double tankerLaneValue =
            Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 10);
        double spikerLaneValue =
            Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 2);
        Level7AdmissionSnapshot pendingTankerLane = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2,
            pendingSegmentSpikeRomComparisonValue: tankerLaneValue);

        Assert.True(tankerLaneValue < Level7WaveSelection.PendingSegmentSpikeRomThreshold);
        Assert.True(spikerLaneValue >= Level7WaveSelection.PendingSegmentSpikeRomThreshold);
        Assert.Equal(Level3OriginalType.Tanker,
            Level7WaveSelection.SelectOrdinaryType(
                pendingTankerLane, (_, _) => throw new InvalidOperationException()));
        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOrdinaryType(
                pendingTankerLane with
                {
                    PendingSegmentSpikeRomComparisonValue = spikerLaneValue
                },
                (_, _) => throw new InvalidOperationException()));
        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOrdinaryType(
                pendingTankerLane with
                {
                    PendingSegmentSpikeRomComparisonValue =
                        Level7WaveSelection.PendingSegmentSpikeRomThreshold
                },
                (_, _) => throw new InvalidOperationException()));
    }

    [Fact]
    public void MissingOrDepletedPendingLaneSpikeUsesTheRomZeroHeightSentinel()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Spike depleted = level.Spikes.Single(spike => spike.EdgeIndex == 8);
        depleted.Height = 0;
        Assert.Equal(0xFF,
            Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 8));

        Spike missing = level.Spikes.Single(spike => spike.EdgeIndex == 9);
        level.RemoveSpike(missing);
        Assert.Equal(0xFF,
            Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 9));

        Level7AdmissionSnapshot sentinel = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2,
            pendingSegmentSpikeRomComparisonValue:
                Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 9));
        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOrdinaryType(
                sentinel, (_, _) => throw new InvalidOperationException()));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Level7WaveSelection.GetPendingSegmentSpikeRomComparisonValue(level, 16));
    }

    [Fact]
    public void RandomStartFallbackScansEligibleTypesOnceAndBlockedStatesUseNoRandomness()
    {
        Level7AdmissionSnapshot fallback = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2,
            pendingSegmentSpikeRomComparisonValue: null);
        int randomCalls = 0;

        Assert.Equal(Level3OriginalType.Tanker,
            Level7WaveSelection.SelectOrdinaryType(fallback, (_, _) =>
            {
                randomCalls++;
                return 0;
            }));
        Assert.Equal(1, randomCalls);

        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOrdinaryType(fallback, (_, _) => 2));
    }

    [Fact]
    public void BlockedReadyEggRetainsItsIdentityAndResumesWhenAdmissionReopens()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg egg = level.Eggs[0];
        MakeReady(egg, level);

        Level7AdmissionSnapshot blocked = Snapshot(
            ordinaryTankers: 2, activeSpikers: 4,
            latentTankerCargoFlippers: 4,
            pendingSegmentSpikeRomComparisonValue: null);
        int randomCalls = 0;
        int constructionCalls = 0;
        List<Enemy> hatched = [];

        for (int attempt = 0; attempt < 3; attempt++)
        {
            level.HatchReadyMixedEnemies(readyEgg =>
            {
                Level3OriginalType? type = Level7WaveSelection.SelectOrdinaryType(
                    blocked, (_, _) =>
                    {
                        randomCalls++;
                        return 0;
                    });
                if (type is null)
                    return null;

                constructionCalls++;
                throw new InvalidOperationException("The blocked selector unexpectedly admitted an Egg.");
            }, hatched.Add);
        }

        Assert.Contains(egg, level.Eggs);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
        Assert.Equal(Level3OriginalState.Egg,
            ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Equal(0, randomCalls);
        Assert.Equal(0, constructionCalls);
        Assert.Empty(hatched);

        Level7AdmissionSnapshot reopened = Snapshot(
            ordinaryTankers: 1, activeSpikers: 4,
            latentTankerCargoFlippers: 2);
        level.HatchReadyMixedEnemies(readyEgg =>
        {
            Level3OriginalType type = Level7WaveSelection.SelectOrdinaryType(
                reopened, (_, _) => throw new InvalidOperationException())
                ?? throw new InvalidOperationException("Reopened Level 7 admission stayed blocked.");
            Assert.Equal(Level3OriginalType.Tanker, type);
            ledger.CommitOriginalAsTanker(readyEgg.OriginalSlotId);
            return new Tanker(readyEgg.EdgeIndex, readyEgg.OriginalSlotId);
        }, hatched.Add);

        Assert.Single(hatched);
        Assert.DoesNotContain(egg, level.Eggs);
    }

    [Fact]
    public void CanonicalLevel7ConversionRequiresTrueZeroPendingAndKeepsSameLedgerIdentity()
    {
        Assert.True(LevelConfiguration.Level7Production.UsesLevel7RomWavePolicy);
        Assert.False((LevelConfiguration.Level7Production with { }).UsesLevel7RomWavePolicy);
        Assert.False(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production with { }, pendingGenericOriginals: 0));
        Assert.False(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production, pendingGenericOriginals: 1));
        Assert.True(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production, pendingGenericOriginals: 0));

        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        Assert.True(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production, pendingGenericOriginals: 0));

        ledger.ConvertActiveSpikerToTanker(0);

        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(0));
        Assert.True(ledger.IsConvertedSpikerTanker(0));
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(0).Count);
        Assert.Throws<InvalidOperationException>(() => ledger.ConvertActiveSpikerToTanker(0));
    }

    [Fact]
    public void GenericDeathReconstructionConservesActiveLatentMaterializedAndTerminalOutcomes()
    {
        Level3WaveLedger ledger = new(originalCount: 5);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);

        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsSpiker(1);

        ledger.MarkOriginalEgg(2);
        ledger.CommitOriginalAsTanker(2);
        int[] materializedChildren = ledger.MaterializeLiveCargo(2).ToArray();
        ledger.MarkOriginalEscaped(2);

        ledger.MarkOriginalEgg(3);
        ledger.CommitOriginalAsSpiker(3);
        ledger.MarkOriginalDestroyed(3);

        int[] unresolvedBeforeDeath = ledger.UnresolvedOriginalIds
            .Concat(ledger.UnresolvedCargoIds)
            .Order()
            .ToArray();
        Assert.Equal(7, unresolvedBeforeDeath.Length);

        ledger.RebuildAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(5, ledger.PendingGenericOriginalCount);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(3));
        Assert.All(materializedChildren,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));

        int originalCountAfterFirstDeath = ledger.OriginalCount;
        ledger.RebuildAfterDeath(clearUnresolvedSpeciesSelections: true);
        Assert.Equal(originalCountAfterFirstDeath, ledger.OriginalCount);
        Assert.Equal(5, ledger.PendingGenericOriginalCount);
        Assert.Empty(ledger.UnresolvedCargoIds);
    }

    [Fact]
    public void RetainedOrdinaryTypeMustWaitForItsOwnAvailability()
    {
        Level7AdmissionSnapshot tankerBlocked = Snapshot(
            ordinaryTankers: 2, activeSpikers: 2);
        Assert.False(Level7WaveSelection.CanAdmitRetainedOriginal(
            tankerBlocked, Level3OriginalType.Tanker));
        Assert.True(Level7WaveSelection.CanAdmitRetainedOriginal(
            tankerBlocked, Level3OriginalType.Tanker, isConvertedTanker: true));
        Assert.False(Level7WaveSelection.CanAdmitRetainedOriginal(
            Snapshot(activeSpikers: 4), Level3OriginalType.Spiker));
        Assert.False(Level7WaveSelection.CanAdmitRetainedOriginal(
            Snapshot(), Level3OriginalType.Flipper));
    }

    [Fact]
    public void SpikerCanRestartItsLivePendingLifecycleWithoutChangingIdentity()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Spiker spiker = new(edgeIndex: 0, speed: 400, originalSlotId: 17);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);
        spiker.Update(level, () => false);

        Assert.Equal(SpikerState.ReturnedToReserve, spiker.State);
        Assert.Equal(0, spiker.Height);

        spiker.RestartForPendingSupply();

        Assert.Equal(SpikerState.Building, spiker.State);
        Assert.Equal(0, spiker.Height);
        Assert.Equal(17, spiker.OriginalSlotId);
    }

    private static Level7AdmissionSnapshot Snapshot(
        int ordinaryFlippers = 0,
        int releasedCargoFlippers = 0,
        int pendingShotCargoFlippers = 0,
        int ordinaryTankers = 0,
        int convertedTankers = 0,
        int activeSpikers = 0,
        int activeOtherEnemies = 0,
        int latentTankerCargoFlippers = 0,
        double? pendingSegmentSpikeRomComparisonValue = 0xD0) =>
        new(ordinaryFlippers, releasedCargoFlippers, pendingShotCargoFlippers,
            ordinaryTankers, convertedTankers, activeSpikers, activeOtherEnemies,
            latentTankerCargoFlippers, pendingSegmentSpikeRomComparisonValue);

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }
}
