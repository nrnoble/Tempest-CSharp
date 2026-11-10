using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level6AdmissionPolicyTests
{
    [Fact]
    public void OrdinarySpeciesAndMinimaAreLevelSixSpecific()
    {
        Assert.Equal(0, Level6AdmissionPolicy.FlipperMinimum);
        Assert.Equal(5, Level6AdmissionPolicy.FlipperMaximum);
        Assert.Equal(1, Level6AdmissionPolicy.TankerMinimum);
        Assert.Equal(2, Level6AdmissionPolicy.TankerMaximum);
        Assert.Equal(2, Level6AdmissionPolicy.SpikerMinimum);
        Assert.Equal(4, Level6AdmissionPolicy.SpikerMaximum);
        Assert.Equal(5, Level6AdmissionPolicy.GetFlipperAvailability(Snapshot()));
        Assert.Equal(0, Level6AdmissionPolicy.GetFlipperAvailability(
            Snapshot(ordinaryFlippers: Level6AdmissionPolicy.FlipperMaximum)));
        Assert.Equal(new[] { Level3OriginalType.Spiker },
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(Snapshot()));
        Assert.Equal(Level3OriginalType.Spiker,
            Level6AdmissionPolicy.SelectOrdinaryType(Snapshot()));

        Level6AdmissionSnapshot twoSpikers = Snapshot(activeSpikers: 2);
        Assert.Equal(new[] { Level3OriginalType.Tanker },
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(twoSpikers));
        Assert.Equal(Level3OriginalType.Tanker,
            Level6AdmissionPolicy.SelectOrdinaryType(twoSpikers));

        Level6AdmissionSnapshot satisfiedMinima = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2);
        Assert.Equal(new[]
        {
            Level3OriginalType.Tanker,
            Level3OriginalType.Spiker
        }, Level6AdmissionPolicy.GetOrdinaryEligibleTypes(satisfiedMinima));
    }

    [Fact]
    public void ZeroMinimumFlippersAndUnavailableSpeciesAreNeverOrdinaryCandidates()
    {
        Level6AdmissionSnapshot snapshot = Snapshot(
            ordinaryFlippers: 3, ordinaryTankers: 1, activeSpikers: 2);

        Assert.True(Level6AdmissionPolicy.GetFlipperAvailability(snapshot) > 0);
        Assert.DoesNotContain(Level3OriginalType.Flipper,
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(snapshot));
        Assert.Equal(new[]
        {
            Level3OriginalType.Tanker,
            Level3OriginalType.Spiker
        }, Level6AdmissionPolicy.GetOrdinaryEligibleTypes(Snapshot(
            ordinaryTankers: 1, activeSpikers: 2)));
    }

    [Fact]
    public void OrdinaryMaximaStopSelectionWithoutLimitingConvertedTankers()
    {
        Level6AdmissionSnapshot tankerMaximum = Snapshot(
            ordinaryTankers: 2, activeSpikers: 2);
        Assert.Equal(0, Level6AdmissionPolicy.GetTankerAvailability(tankerMaximum));
        Assert.DoesNotContain(Level3OriginalType.Tanker,
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(tankerMaximum));

        Level6AdmissionSnapshot spikerMaximum = Snapshot(
            ordinaryTankers: 1, activeSpikers: 4);
        Assert.Equal(0, Level6AdmissionPolicy.GetSpikerAvailability(spikerMaximum));
        Assert.DoesNotContain(Level3OriginalType.Spiker,
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(spikerMaximum));

        Level6AdmissionSnapshot convertedBeyondOrdinaryMaximum = Snapshot(
            ordinaryTankers: 1, convertedTankers: 4,
            latentTankerCargoFlippers: 10, activeSpikers: 1);
        Assert.Equal(1, Level6AdmissionPolicy.GetTankerAvailability(
            convertedBeyondOrdinaryMaximum));
    }

    [Fact]
    public void PostMinimumSelectionUsesThePendingEggsRomMappedSegmentRatherThanGlobalMinimum()
    {
        Level level = new(LevelConfiguration.Level6Production);
        level.Spikes.Single(spike => spike.EdgeIndex == 2).Height = 30;
        level.Spikes.Single(spike => spike.EdgeIndex == 10).Height = 350;

        double pendingTankerLaneValue =
            Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(
                level, pendingLane: 10);
        double pendingSpikerLaneValue =
            Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(
                level, pendingLane: 2);
        Level6AdmissionSnapshot tankerLane = Snapshot(
            ordinaryTankers: 1, activeSpikers: 2,
            pendingSegmentSpikeRomComparisonValue: pendingTankerLaneValue);
        Level6AdmissionSnapshot spikerLane = tankerLane with
        {
            PendingSegmentSpikeRomComparisonValue = pendingSpikerLaneValue
        };

        Assert.Equal(Level3OriginalType.Tanker,
            Level6AdmissionPolicy.SelectOrdinaryType(tankerLane));
        Assert.Equal(Level3OriginalType.Spiker,
            Level6AdmissionPolicy.SelectOrdinaryType(spikerLane));
        Assert.Equal(Level3OriginalType.Spiker,
            Level6AdmissionPolicy.SelectOrdinaryType(tankerLane with
            {
                PendingSegmentSpikeRomComparisonValue =
                    Level6AdmissionPolicy.PendingSegmentSpikeRomThreshold
            }));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(
                level, pendingLane: 16));
    }

    [Fact]
    public void EveryCanonicalLevelSixPendingLaneMapsToItsOwnSpikeAcrossClosedTubeBoundary()
    {
        Level level = new(LevelConfiguration.Level6Production);

        Assert.Equal(16, level.PlayableLaneCount);
        Assert.Equal(16, level.Spikes.Count);
        for (int lane = 0; lane < level.PlayableLaneCount; lane++)
        {
            Spike segmentSpike = Assert.Single(
                level.Spikes, spike => spike.EdgeIndex == lane);
            segmentSpike.Height = 10 + lane * 20;

            Assert.Equal(
                Tanker.RomAlongPositionForHeight(segmentSpike.Height),
                Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(level, lane),
                precision: 10);
        }

        Assert.True(level.TryGetAdjacentLane(0, -1, out int wrappedFromZero));
        Assert.True(level.TryGetAdjacentLane(15, 1, out int wrappedFromFifteen));
        Assert.Equal(15, wrappedFromZero);
        Assert.Equal(0, wrappedFromFifteen);
        Assert.Equal(Tanker.RomAlongPositionForHeight(
                level.Spikes.Single(spike => spike.EdgeIndex == 15).Height),
            Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(level, wrappedFromZero),
            precision: 10);
        Assert.Equal(Tanker.RomAlongPositionForHeight(
                level.Spikes.Single(spike => spike.EdgeIndex == 0).Height),
            Level6AdmissionPolicy.GetPendingSegmentSpikeRomComparisonValue(level, wrappedFromFifteen),
            precision: 10);
    }

    [Fact]
    public void CanonicalLevelSixPendingEggOnLaneEightUsesRomNoSpikeSentinelAfterMinima()
    {
        TempestGame game = new(startingProductionLevel: 6, random: new Random(1));
        Level level = game.CurrentLevel;
        game.CurrentLevel.State = LevelState.Playing;
        Egg[] initialEggs = game.Eggs.ToArray();
        foreach (Egg egg in initialEggs.Take(3))
            MakeReady(egg, level);

        game.RunOneStep();

        Assert.Equal(2, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Egg pendingEgg = Assert.Single(game.Eggs);
        for (int update = 0;
             update < level.PlayableLaneCount * 8 && pendingEgg.EdgeIndex != 8;
             update++)
            pendingEgg.Update(level);
        Assert.Equal(8, pendingEgg.EdgeIndex);
        MakeReady(pendingEgg, level);
        level.RemoveSpike(level.Spikes.Single(spike => spike.EdgeIndex == pendingEgg.EdgeIndex));

        game.RunOneStep();

        Assert.Equal(Level3OriginalType.Spiker,
            level.Level3Ledger!.GetOriginalType(pendingEgg.OriginalSlotId));
        Assert.Equal(3, game.Spikers.Count);
        Assert.Single(game.Tankers);
    }

    [Fact]
    public void CanonicalProductionHatchesUseThePendingEggLanesSpike()
    {
        TempestGame game = new(startingProductionLevel: 6, random: new Random(1));
        Level level = game.CurrentLevel;
        game.CurrentLevel.State = LevelState.Playing;
        Egg[] eggs = game.Eggs.ToArray();
        foreach (Egg egg in eggs)
            MakeReady(egg, level);

        foreach (Spike spike in level.Spikes)
            spike.Height = 350;
        Spike otherLaneMinimum = level.Spikes.First(
            spike => spike.EdgeIndex != eggs[3].EdgeIndex);
        otherLaneMinimum.Height = 30;

        game.RunOneStep();

        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        Assert.Equal(2, game.Spikers.Count);
        Assert.Equal(2, game.Tankers.Count);
        Assert.Empty(game.Flippers);
        Assert.Equal(Level3OriginalType.Tanker,
            level.Level3Ledger!.GetOriginalType(eggs[3].OriginalSlotId));
    }

    [Fact]
    public void OpenPlaceDeadEndLeavesFlipperAvailabilityButDeniesOrdinaryAdmission()
    {
        Level6AdmissionSnapshot deadEnd = Snapshot(
            ordinaryTankers: 2,
            activeSpikers: 4,
            latentTankerCargoFlippers: 4);

        Assert.Equal(6, deadEnd.ActiveEnemyCount);
        Assert.Equal(1, Level6AdmissionPolicy.GetOpenPhysicalSlots(deadEnd));
        Assert.Equal(1, Level6AdmissionPolicy.GetFlipperAvailability(deadEnd));
        Assert.Equal(0, Level6AdmissionPolicy.GetTankerAvailability(deadEnd));
        Assert.Equal(0, Level6AdmissionPolicy.GetSpikerAvailability(deadEnd));
        Assert.Null(Level6AdmissionPolicy.SelectOrdinaryType(deadEnd));
    }

    [Fact]
    public void SevenLivePlacesBlockAdmissionWhileLatentCargoAndHazardsUseNoPlaces()
    {
        Level6AdmissionSnapshot full = Snapshot(
            ordinaryFlippers: 1,
            releasedCargoFlippers: 1,
            pendingShotCargoFlippers: 1,
            ordinaryTankers: 1,
            convertedTankers: 1,
            activeSpikers: 1,
            activeOtherEnemies: 1,
            latentTankerCargoFlippers: 4);

        Assert.Equal(7, full.ActiveEnemyCount);
        Assert.Equal(0, Level6AdmissionPolicy.GetOpenPhysicalSlots(full));
        Assert.Null(Level6AdmissionPolicy.SelectOrdinaryType(full));
        Assert.Equal(7, Level6AdmissionPolicy.GetOpenPhysicalSlots(Snapshot()));
        Assert.Equal(7, Level6AdmissionPolicy.PhysicalEnemySlotCount);
    }

    [Fact]
    public void LatentCargoReducesFlipperAvailabilityWithoutOccupyingPlaces()
    {
        Level6AdmissionSnapshot oneTanker = Snapshot(
            ordinaryTankers: 1, latentTankerCargoFlippers: 2);

        Assert.Equal(1, oneTanker.ActiveEnemyCount);
        Assert.Equal(6, Level6AdmissionPolicy.GetOpenPhysicalSlots(oneTanker));
        Assert.Equal(3, Level6AdmissionPolicy.GetFlipperAvailability(oneTanker));
        Assert.Equal(1, Level6AdmissionPolicy.GetTankerAvailability(oneTanker));
    }

    [Fact]
    public void TankerUsesOneFreePlaceAndReleasedCargoMayExceedOrdinaryFlipperMaximum()
    {
        Level6AdmissionSnapshot oneFreePlace = Snapshot(
            ordinaryFlippers: 4, activeSpikers: 2);
        Assert.Equal(1, Level6AdmissionPolicy.GetOpenPhysicalSlots(oneFreePlace));
        Assert.Equal(Level3OriginalType.Tanker,
            Level6AdmissionPolicy.SelectOrdinaryType(oneFreePlace));

        Level6AdmissionSnapshot cargoAboveOrdinaryMaximum =
            Snapshot(releasedCargoFlippers: 6);
        Assert.Equal(6, cargoAboveOrdinaryMaximum.ActiveFlippers);
        Assert.Equal(1, Level6AdmissionPolicy.GetOpenPhysicalSlots(
            cargoAboveOrdinaryMaximum));
        Assert.DoesNotContain(Level3OriginalType.Flipper,
            Level6AdmissionPolicy.GetOrdinaryEligibleTypes(cargoAboveOrdinaryMaximum));
        Assert.Null(Level6AdmissionPolicy.SelectOrdinaryType(
            cargoAboveOrdinaryMaximum with { ReleasedCargoFlippers = 8 }));
    }

    [Fact]
    public void BlockedReadyEggKeepsItsIdentityAndDoesNotConstructOrConsumeRandomness()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg egg = level.Eggs[0];
        MakeReady(egg, level);

        int randomCalls = 0;
        int constructionCalls = 0;
        List<Enemy> hatched = [];
        Level6AdmissionSnapshot blocked = Snapshot(activeOtherEnemies: 7);
        for (int attempt = 0; attempt < 3; attempt++)
        {
            level.HatchReadyMixedEnemies(
                readyEgg =>
                {
                    if (Level6AdmissionPolicy.SelectOrdinaryType(blocked) is null)
                        return null;
                    randomCalls++;
                    constructionCalls++;
                    ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                    return new Spiker(readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
                },
                hatched.Add);
        }

        Assert.Equal(EggState.ReadyToHatch, egg.State);
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(egg.OriginalSlotId));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(22, ledger.PendingGenericOriginalCount);
        Assert.Empty(hatched);
        Assert.Equal(0, randomCalls);
        Assert.Equal(0, constructionCalls);

        Level6AdmissionSnapshot reopened = Snapshot();
        level.HatchReadyMixedEnemies(
            readyEgg =>
            {
                Level3OriginalType selected =
                    Level6AdmissionPolicy.SelectOrdinaryType(reopened)!.Value;
                Assert.Equal(Level3OriginalType.Spiker, selected);
                ledger.CommitOriginalAsSpiker(readyEgg.OriginalSlotId);
                return new Spiker(readyEgg.EdgeIndex, 2, readyEgg.OriginalSlotId);
            },
            hatched.Add);

        Assert.DoesNotContain(level.Eggs,
            candidate => candidate.OriginalSlotId == egg.OriginalSlotId);
        Assert.Single(hatched);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(egg.OriginalSlotId));
        Assert.Equal(21, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void SuccessfulSameTickHatchesRecomputeOrdinaryAvailability()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Level3WaveLedger ledger = level.Level3Ledger!;
        foreach (Egg egg in level.Eggs)
            MakeReady(egg, level);

        int ordinaryTankers = 0;
        int activeSpikers = 0;
        List<Enemy> hatched = [];
        level.HatchReadyMixedEnemies(
            egg =>
            {
                Level6AdmissionSnapshot snapshot = Snapshot(
                    ordinaryTankers: ordinaryTankers,
                    activeSpikers: activeSpikers,
                    latentTankerCargoFlippers: ordinaryTankers * 2);
                Level3OriginalType? selected =
                    Level6AdmissionPolicy.SelectOrdinaryType(snapshot);
                if (selected is null)
                    return null;

                if (selected == Level3OriginalType.Tanker)
                {
                    ledger.CommitOriginalAsTanker(egg.OriginalSlotId);
                    return new Tanker(egg.EdgeIndex, egg.OriginalSlotId);
                }

                ledger.CommitOriginalAsSpiker(egg.OriginalSlotId);
                return new Spiker(egg.EdgeIndex, 2, egg.OriginalSlotId);
            },
            enemy =>
            {
                hatched.Add(enemy);
                if (enemy is Tanker)
                    ordinaryTankers++;
                else if (enemy is Spiker)
                    activeSpikers++;
            });

        Assert.Equal(4, hatched.Count);
        Assert.Equal(1, ordinaryTankers);
        Assert.Equal(3, activeSpikers);
        Assert.Equal(18, ledger.PendingGenericOriginalCount);
    }

    [Fact]
    public void TerminalConversionRequiresZeroGenericPendingAndPreservesEachIdentityOnce()
    {
        Assert.True(LevelConfiguration.Level6Production.UsesGenericRomDeathReconstruction);
        Assert.False((LevelConfiguration.Level6Production with { })
            .UsesGenericRomDeathReconstruction);
        Assert.True(Level6AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level6Production, pendingGenericOriginals: 0));
        Assert.False(Level6AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level6Production, pendingGenericOriginals: 1));
        Assert.False(Level6AdmissionPolicy.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level6Production with { }, pendingGenericOriginals: 0));

        Level3WaveLedger ledger = new(originalCount: 6);
        for (int id = 0; id < 6; id++)
        {
            ledger.MarkOriginalEgg(id);
            if (id < 2)
                ledger.CommitOriginalAsTanker(id);
            else
                ledger.CommitOriginalAsSpiker(id);
        }

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        int[] terminalSpikers = [2, 3, 4, 5];
        foreach (int id in terminalSpikers)
        {
            Assert.True(Level6AdmissionPolicy.ShouldConvertTerminalSpiker(
                LevelConfiguration.Level6Production, ledger.PendingGenericOriginalCount));
            ledger.ConvertActiveSpikerToTanker(id);
        }

        Assert.Equal(6, Enumerable.Range(0, 6)
            .Count(id => ledger.GetOriginalType(id) == Level3OriginalType.Tanker));
        Assert.Equal(2, Enumerable.Range(0, 6)
            .Count(id => !ledger.IsConvertedSpikerTanker(id)));
        Assert.All(terminalSpikers, id =>
        {
            Assert.True(ledger.IsConvertedSpikerTanker(id));
            Assert.Equal(2, ledger.GetCargoIdsForOriginal(id).Count);
        });
        Assert.Throws<InvalidOperationException>(
            () => ledger.ConvertActiveSpikerToTanker(terminalSpikers[0]));
        ledger.ValidateState();
    }

    [Fact]
    public void LevelSixDeathReconstructionConservesPendingActiveAndMaterializedCargoObligations()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg[] initialEggs = level.Eggs.ToArray();

        ledger.CommitOriginalAsFlipper(initialEggs[0].OriginalSlotId);
        ledger.CommitOriginalAsTanker(initialEggs[1].OriginalSlotId);
        int materializedOwner = initialEggs[2].OriginalSlotId;
        ledger.CommitOriginalAsTanker(materializedOwner);
        int[] materializedCargo = ledger.MaterializeLiveCargo(materializedOwner).ToArray();
        ledger.MarkCargoDestroyed(materializedCargo[0]);
        ledger.MarkOriginalEscaped(materializedOwner);
        int fourthPendingId = initialEggs[3].OriginalSlotId;

        level.ResetAfterDeath();

        Assert.Equal(23, ledger.OriginalCount);
        Assert.Equal(22, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalType.Unselected,
            ledger.GetOriginalType(initialEggs[0].OriginalSlotId));
        Assert.Equal(Level3OriginalType.Unselected,
            ledger.GetOriginalType(initialEggs[1].OriginalSlotId));
        Assert.Equal(Level3OriginalType.Unselected,
            ledger.GetOriginalType(fourthPendingId));
        Assert.Equal(Level3OriginalState.Escaped,
            ledger.GetOriginalState(materializedOwner));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(materializedCargo[0]));
        Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(materializedCargo[1]));
        Assert.All(level.Eggs, egg => Assert.Equal(-1, egg.CargoUnitId));
        Assert.Equal(22, ledger.PendingGenericOriginalCount);

        int reconstructedActiveId = level.Eggs[0].OriginalSlotId;
        ledger.CommitOriginalAsTanker(reconstructedActiveId);
        level.ResetAfterDeath();

        Assert.Equal(22, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.All(ledger.UnresolvedOriginalIds, id =>
            Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(materializedCargo[0]));
        Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(materializedCargo[1]));
        ledger.ValidateState();
    }

    [Fact]
    public void WaitingGenericObligationsRemainLevelCompletionBlockers()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Assert.True(level.Level3Ledger!.PendingGenericOriginalCount > 0);
        Assert.False(level.IsComplete(activeFlippers: 0));
    }

    private static Level6AdmissionSnapshot Snapshot(
        int ordinaryFlippers = 0,
        int releasedCargoFlippers = 0,
        int pendingShotCargoFlippers = 0,
        int ordinaryTankers = 0,
        int convertedTankers = 0,
        int activeSpikers = 0,
        int activeOtherEnemies = 0,
        int latentTankerCargoFlippers = 0,
        double pendingSegmentSpikeRomComparisonValue = 0xD4) =>
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
