using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level4AdmissionPolicyTests
{
    [Theory]
    [InlineData(0, 0, Level3OriginalType.Spiker)]
    [InlineData(0, 1, Level3OriginalType.Flipper)]
    [InlineData(1, 1, null)]
    [InlineData(4, 1, Level3OriginalType.Spiker)]
    [InlineData(1, 2, Level3OriginalType.Flipper)]
    [InlineData(4, 2, null)]
    public void OrdinaryEligibilityUsesLevelFourMinimaMaximaAndSharedHeadroom(
        int flippers,
        int spikers,
        Level3OriginalType? expectedOnlyType)
    {
        Level4AdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: flippers,
            activeSpikers: spikers);
        IReadOnlyList<Level3OriginalType> eligible =
            Level4AdmissionPolicy.GetOrdinaryEligibleTypes(snapshot);

        if (expectedOnlyType is Level3OriginalType expected)
            Assert.Equal(new[] { expected }, eligible);
        else if (flippers == 1 && spikers == 1)
            Assert.Equal(
                new[] { Level3OriginalType.Flipper, Level3OriginalType.Spiker },
                eligible);
        else
        {
            Assert.Empty(eligible);
            Assert.Equal(1, Level4AdmissionPolicy.GetOpenPhysicalSlots(snapshot));
        }

        Assert.DoesNotContain(Level3OriginalType.Tanker, eligible);
        Assert.All(eligible, type =>
            Assert.True(type is Level3OriginalType.Flipper or Level3OriginalType.Spiker));
    }

    [Fact]
    public void EligibleSpeciesSelectionIsBoundedAndDenialConsumesNoRandomness()
    {
        int calls = 0;
        Level4AdmissionSnapshot bothEligible = Snapshot(
            ordinaryFlippers: 1,
            activeSpikers: 1);
        Assert.Equal(Level3OriginalType.Spiker,
            Level4AdmissionPolicy.SelectOrdinaryType(
                bothEligible,
                (minimum, maximum) =>
                {
                    calls++;
                    Assert.Equal(0, minimum);
                    Assert.Equal(1, maximum);
                    return maximum;
                }));
        Assert.Equal(1, calls);

        Assert.Null(Level4AdmissionPolicy.SelectOrdinaryType(
            Snapshot(ordinaryFlippers: 4, activeSpikers: 2),
            (_, _) =>
            {
                calls++;
                return 0;
            }));
        Assert.Equal(1, calls);
    }

    [Fact]
    public void RepeatedOrdinaryAdmissionCannotExceedFourFlippersOrTwoSpikers()
    {
        int flippers = 0;
        int spikers = 0;

        for (int hatch = 0; hatch < 7; hatch++)
        {
            Level3OriginalType? selected = Level4AdmissionPolicy.SelectOrdinaryType(
                Snapshot(ordinaryFlippers: flippers, activeSpikers: spikers),
                (minimum, _) => minimum);
            if (selected is null)
                break;

            if (selected == Level3OriginalType.Flipper)
                flippers++;
            else
                spikers++;
        }

        Assert.Equal(4, flippers);
        Assert.Equal(2, spikers);
        Assert.Null(Level4AdmissionPolicy.SelectOrdinaryType(
            Snapshot(ordinaryFlippers: flippers, activeSpikers: spikers),
            (_, _) => throw new InvalidOperationException("A denied hatch must not draw randomness.")));
    }

    [Fact]
    public void AllLiveStatesAndPendingShotCargoOccupyPhysicalPlacesButEggsAndHazardsDoNot()
    {
        // Live-list membership includes rim, transition and pending-zap enemies; these states
        // remain represented by their enemy objects until the existing lifecycle removes them.
        Level4AdmissionSnapshot occupied = Snapshot(
            ordinaryFlippers: 2,
            releasedCargoFlippers: 1,
            pendingShotCargoFlippers: 1,
            activeTankers: 1,
            activeSpikers: 1,
            activeOtherEnemies: 1);

        Assert.Equal(7, occupied.ActiveEnemyCount);
        Assert.Equal(0, Level4AdmissionPolicy.GetOpenPhysicalSlots(occupied));
        Assert.Empty(Level4AdmissionPolicy.GetOrdinaryEligibleTypes(occupied));

        // Reserve/Egg identities, latent cargo, bullets and persistent Spikes are not fields
        // in the physical-occupancy snapshot and therefore do not consume enemy slots.
        Level4AdmissionSnapshot noLiveEnemies = Snapshot();
        Assert.Equal(7, Level4AdmissionPolicy.GetOpenPhysicalSlots(noLiveEnemies));
        Assert.Equal(1, Snapshot(pendingShotCargoFlippers: 1).ActiveEnemyCount);
    }

    [Fact]
    public void CompletePairCargoOverflowBlocksOrdinaryHatchesUntilRoomReturns()
    {
        Level4AdmissionSnapshot overflow = Snapshot(releasedCargoFlippers: 8);

        Assert.Equal(0, Level4AdmissionPolicy.GetOpenPhysicalSlots(overflow));
        Assert.Empty(Level4AdmissionPolicy.GetOrdinaryEligibleTypes(overflow));
        Assert.False(Level4AdmissionPolicy.CanAdmitCargoReentry(overflow));
        Assert.True(Level4AdmissionPolicy.CanAdmitCargoReentry(
            overflow with { ReleasedCargoFlippers = 6 }));
    }

    [Fact]
    public void BlockedReadyEggKeepsItsUnselectedIdentityAndCanHatchAfterCapacityReturns()
    {
        Level level = new(LevelConfiguration.Level4Candidate);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg egg = level.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);

        int randomCalls = 0;
        Level4AdmissionSnapshot full = Snapshot(
            ordinaryFlippers: 4,
            activeSpikers: 2,
            activeOtherEnemies: 1);
        List<Enemy> hatched = [];
        level.HatchReadyMixedEnemies(
            readyEgg =>
            {
                Level3OriginalType? type = Level4AdmissionPolicy.SelectOrdinaryType(
                    full,
                    (_, _) =>
                    {
                        randomCalls++;
                        return 0;
                    });
                if (type is null)
                    return null;

                ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                return new Spiker(readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
            },
            hatched.Add);

        Assert.Equal(EggState.ReadyToHatch, egg.State);
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(17, ledger.PendingGenericOriginalCount);
        Assert.Equal(0, randomCalls);

        Level4AdmissionSnapshot roomAvailable = Snapshot();
        level.HatchReadyMixedEnemies(
            readyEgg =>
            {
                Level3OriginalType type = Level4AdmissionPolicy.SelectOrdinaryType(
                    roomAvailable, (_, _) => 0)!.Value;
                if (type == Level3OriginalType.Spiker)
                    ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                else
                    ledger.CommitOriginalAsFlipper(readyEgg.OriginalSlotId);

                Assert.Equal(Level3OriginalType.Spiker, type);
                return new Spiker(readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
            },
            hatched.Add);

        Assert.DoesNotContain(level.Eggs,
            candidate => candidate.OriginalSlotId == egg.OriginalSlotId);
        Assert.Single(hatched);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(Level3OriginalState.ActiveSpiker, ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Equal(16, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void RetainedConvertedTankerAndCargoReentryUsePhysicalHeadroomWithoutReallocation()
    {
        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        ledger.ConvertActiveSpikerToTanker(0);
        int[] cargoIds = ledger.GetCargoIdsForOriginal(0).ToArray();

        ledger.RebuildAfterDeath();
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(0));
        Assert.Equal(cargoIds, ledger.GetCargoIdsForOriginal(0));
        Assert.False(Level4AdmissionPolicy.CanAdmitRetainedOriginal(
            Snapshot(activeOtherEnemies: 7), Level3OriginalType.Tanker));
        Assert.True(Level4AdmissionPolicy.CanAdmitRetainedOriginal(
            Snapshot(activeOtherEnemies: 6), Level3OriginalType.Tanker));

        ledger.MarkOriginalEgg(0);
        Assert.True(Level4AdmissionPolicy.CanAdmitCargoReentry(Snapshot()));
        ledger.CommitOriginalAsTanker(0);
        Assert.Equal(cargoIds, ledger.GetCargoIdsForOriginal(0));
        Assert.Equal(2, ledger.CargoCount);
        ledger.ValidateState();
    }

    [Fact]
    public void GenericPendingCountUsesOnlyOriginalReserveAndEggStates()
    {
        Level3WaveLedger ledger = new(originalCount: 3);
        Assert.Equal(3, ledger.PendingGenericOriginalCount);

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

    private static Level4AdmissionSnapshot Snapshot(
        int ordinaryFlippers = 0,
        int releasedCargoFlippers = 0,
        int pendingShotCargoFlippers = 0,
        int activeTankers = 0,
        int activeSpikers = 0,
        int activeOtherEnemies = 0) =>
        new(ordinaryFlippers, releasedCargoFlippers, pendingShotCargoFlippers,
            activeTankers, activeSpikers, activeOtherEnemies);

}
