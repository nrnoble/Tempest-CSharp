using System.Numerics;
using System.Reflection;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level7ProductionTests
{
    private static readonly Vector2[] CloverPoints =
    [
        new(-160, 160), new(-179, 44), new(-89, 0), new(-179, -44),
        new(-160, -160), new(-44, -179), new(0, -89), new(44, -179),
        new(160, -160), new(179, -44), new(89, 0), new(179, 44),
        new(160, 160), new(44, 179), new(0, 89), new(-44, 179)
    ];

    [Fact]
    public void ProductionConfigurationUsesTheOriginalArcadeCloverAndApprovedWaveSystems()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production;
        Level level = new(configuration, randomInclusive: (_, _) => 0);

        Assert.Equal(6, configuration.ProgressionIndex);
        Assert.Equal(7, configuration.DisplayedLevel);
        Assert.Equal(6, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(310, configuration.Definition.CenterY);
        Assert.Equal(-10, configuration.Definition.YOffset);
        Assert.Equal(CloverPoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(Level.Depth, configuration.Depth);
        Assert.Equal(Level.Position, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(20, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Equal(20, level.Level3Ledger!.OriginalCount);
        Assert.Equal(20, level.Level3Ledger.TotalIdentityCount);
        Assert.Equal(20, level.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(0, level.Level3Ledger.CargoCount);
        Assert.Equal(16, level.Spikes.Count);
        Assert.All(level.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, configuration.ExitPolicy);
        Assert.Equal(30, configuration.AvoidSpikesWaitThreshold);
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.Equal(62, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(3, configuration.MaximumActiveEnemyShots);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.Equal(BelowRimFlipperProgram.Level7ArcadeObserved, configuration.BelowRimFlipperProgram);
        Assert.True(configuration.UsesLevel7RomWavePolicy);
        Assert.False((configuration with { }).UsesLevel7RomWavePolicy);
        Assert.True(configuration.UsesLevel7SpikerFiringPolicy);
        Assert.Equal(62, configuration.Level7SpikerShotHoldoffTicks);
        Assert.False((configuration with { }).UsesLevel7SpikerFiringPolicy);
        Assert.False((configuration with { }).UsesGenericRomDeathReconstruction);
        Assert.Equal(1, configuration.ProgrammedFlipperInitialDirection);
        Assert.Equal(new FlipperConfiguration(true, 2, 9, 0, 23), configuration.Flipper);
    }

    [Fact]
    public void OneUnitDeathReconstructionClearsSpeciesAndSelectsTheLevel7Spiker()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1
        };
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg originalEgg = Assert.Single(level.Eggs);
        ledger.CommitOriginalAsFlipper(originalEgg.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Single(ledger.UnresolvedOriginalIds);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Assert.Equal(1, level.PendingGenericEnemyUnits);
        Egg reentryEgg = Assert.Single(level.Eggs);
        MakeReady(reentryEgg, level);
        Spiker? reconstructed = null;
        level.HatchReadyMixedEnemies(egg =>
        {
            Level7AdmissionSnapshot snapshot = new(
                OrdinaryFlippers: 0, ReleasedCargoFlippers: 0,
                PendingShotCargoFlippers: 0, OrdinaryTankers: 0,
                ConvertedTankers: 0, ActiveSpikers: 0, ActiveOtherEnemies: 0,
                LatentTankerCargoFlippers: 0,
                PendingSegmentSpikeRomComparisonValue: 0xFF);
            Level3OriginalType type = Level7WaveSelection.SelectOrdinaryType(
                snapshot, (_, _) => throw new InvalidOperationException())
                ?? throw new InvalidOperationException("The Level 7 one-unit selection was unavailable.");
            Assert.Equal(Level3OriginalType.Spiker, type);
            ledger.CommitOriginalAsSpiker(egg.OriginalSlotId);
            reconstructed = new Spiker(egg.EdgeIndex, originalSlotId: egg.OriginalSlotId);
            return reconstructed;
        }, _ => { });

        Assert.NotNull(reconstructed);
        Assert.Empty(level.Eggs);
        Assert.Equal(0, level.PendingGenericEnemyUnits);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(0));
        Assert.Single(ledger.UnresolvedOriginalIds);
    }

    [Fact]
    public void ReleasedTankerCargoReturnsAsGenericPendingSupplyAfterLevel7Death()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            EnemySupply = 2,
            InitialActiveEggCount = 2,
            MaximumActiveEggs = 2
        };
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg tankerEgg = level.Eggs[0];
        ledger.CommitOriginalAsTanker(tankerEgg.OriginalSlotId);
        int[] cargoIds = ledger.MaterializeLiveCargo(tankerEgg.OriginalSlotId).ToArray();
        ledger.MarkOriginalEscaped(tankerEgg.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(3, level.PendingGenericEnemyUnits);
        Assert.Equal(3, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.All(cargoIds,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
    }

    [Fact]
    public void CanonicalLevel7NaturalReleaseMaterializesOnlyTheFirstChildAtSaturation()
    {
        (TempestGame game, Tanker tanker, int[] priorCargoIds) =
            CreateCapacityLimitedReleaseScenario(spikerCount: 4);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int pendingOriginalsBefore = ledger.PendingGenericOriginalCount;

        ReleaseTankerCargo(game, tanker, TankerReleaseKind.Natural);

        Assert.Equal(7, CountPhysicalEnemies(game));
        Assert.Equal(new[] { 0, 1, 2 }, game.Flippers
            .Where(flipper => flipper.IsTankerCargo)
            .Select(flipper => flipper.CargoUnitId).Order());
        Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(2));
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
        Assert.Equal(new[] { 0, 1, 2 }, ledger.UnresolvedCargoIds.Order());
        Assert.Equal(pendingOriginalsBefore, ledger.PendingGenericOriginalCount);
        Assert.DoesNotContain(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.Empty(ledger.CargoIdsInReentryReserve());

        foreach (int id in priorCargoIds)
            ledger.MarkCargoDestroyed(id);
        RemovePriorCargoFlippers(game, priorCargoIds);

        int originalCountBeforeDeath = ledger.OriginalCount;
        game.CurrentLevel.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(originalCountBeforeDeath + 1, ledger.OriginalCount);
        Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(2));
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
        Assert.Empty(ledger.UnresolvedCargoIds);

        game.CurrentLevel.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(originalCountBeforeDeath + 1, ledger.OriginalCount);
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
        Assert.Empty(ledger.UnresolvedCargoIds);
    }

    [Fact]
    public void CanonicalLevel7NaturalReleaseMaterializesBothChildrenWhenTheyFit()
    {
        (TempestGame game, Tanker tanker, _) =
            CreateCapacityLimitedReleaseScenario(spikerCount: 3);

        ReleaseTankerCargo(game, tanker, TankerReleaseKind.Natural);

        Flipper[] cargo = game.Flippers.Where(flipper =>
            flipper.IsTankerCargo && flipper.CargoUnitId >= 2)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(new[] { 2, 3 }, cargo.Select(flipper => flipper.CargoUnitId));
        Assert.Equal(new[] { 1, -1 }, cargo.Select(flipper => flipper.Direction));
        Assert.Equal(new[] { 0, 2 }, cargo.Select(flipper => flipper.EdgeIndex));
        Assert.Equal(7, CountPhysicalEnemies(game));
        Assert.All(new[] { 2, 3 },
            id => Assert.Equal(Level3CargoState.Active,
                game.CurrentLevel.Level3Ledger!.GetCargoState(id)));
    }

    [Fact]
    public void CanonicalLevel7ShotReleaseCountsDeferredActivationAsPhysicalOccupancy()
    {
        (TempestGame game, Tanker tanker, _) =
            CreateCapacityLimitedReleaseScenario(spikerCount: 4);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        ReleaseTankerCargo(game, tanker, TankerReleaseKind.PlayerShotDestruction);

        List<Flipper> pendingActivation =
            GetGameList<Flipper>(game, "shotReleasedCargoPendingActivation");
        Assert.Single(pendingActivation);
        Assert.Equal(7, CountPhysicalEnemies(game));
        Assert.Equal(2, pendingActivation[0].CargoUnitId);
        Assert.Equal(Level3CargoState.PendingShotActivation,
            ledger.GetCargoState(2));
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
        Assert.True(ledger.HasPendingShotActivation);
        Assert.DoesNotContain(3, ledger.UnresolvedCargoIds);
    }

    [Fact]
    public void ConvertedLevel7TankerUsesTheSameIndependentChildCapacity()
    {
        (TempestGame game, Tanker tanker, _) =
            CreateCapacityLimitedReleaseScenario(spikerCount: 4, convertedTanker: true);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.True(ledger.IsConvertedSpikerTanker(tanker.OriginalSlotId));

        ReleaseTankerCargo(game, tanker, TankerReleaseKind.Natural);

        Assert.Equal(7, CountPhysicalEnemies(game));
        Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(2));
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
    }

    [Fact]
    public void CanonicalLevel7SuperZapperReleaseKeepsAllocatedCargoStillborn()
    {
        (TempestGame game, Tanker tanker, _) =
            CreateCapacityLimitedReleaseScenario(spikerCount: 4);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        ReleaseTankerCargo(game, tanker, TankerReleaseKind.SuperZapperDestruction);

        Flipper stillbornChild = Assert.Single(game.Flippers,
            flipper => flipper.CargoUnitId == 2);
        Assert.Equal(8, stillbornChild.PendingZapTicks);
        Assert.Equal(7, CountPhysicalEnemies(game));
        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(3));
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
    }

    [Fact]
    public void UnallocatedCargoDoesNotBlockCompletionButMaterializedCargoDoes()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            random: new Level7FiringRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        for (int originalId = 0; originalId < ledger.OriginalCount; originalId++)
        {
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(originalId);
            if (originalId == 0)
            {
                ledger.CommitOriginalAsTanker(originalId);
                ledger.MaterializeCargoWithinCapacity(
                    originalId, Level3CargoState.Active, childCapacity: 1);
            }
            else
                ledger.CommitOriginalAsFlipper(originalId);
            ledger.MarkOriginalDestroyed(originalId);
        }
        GetGameList<Egg>(game.CurrentLevel, "eggs").Clear();

        Assert.Equal(Level3CargoState.NotAllocatedForPhysicalCapacity,
            ledger.GetCargoState(1));
        Assert.False(CheckForLevelComplete(game));
        Assert.Contains(0, ledger.UnresolvedCargoIds);

        ledger.MarkCargoDestroyed(0);

        Assert.True(CheckForLevelComplete(game));
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Empty(ledger.UnresolvedCargoIds);
    }

    [Fact]
    public void FinalPendingSpikerConversionPreservesTheOriginalAndAllocatesOneCargoPair()
    {
        Level3WaveLedger ledger = new(originalCount: 3);
        for (int originalId = 0; originalId < 3; originalId++)
        {
            ledger.MarkOriginalEgg(originalId);
            ledger.CommitOriginalAsSpiker(originalId);
        }

        Assert.False(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production, pendingGenericOriginals: 1));
        Assert.False(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production with { }, pendingGenericOriginals: 0));
        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.True(Level7WaveSelection.ShouldConvertTerminalSpiker(
            LevelConfiguration.Level7Production, ledger.PendingGenericOriginalCount));

        foreach (int originalId in new[] { 0, 1, 2 })
            ledger.ConvertActiveSpikerToTanker(originalId);

        Assert.Equal(3, ledger.UnresolvedOriginalIds.Count);
        Assert.All(new[] { 0, 1, 2 }, originalId =>
        {
            Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(originalId));
            Assert.True(ledger.IsConvertedSpikerTanker(originalId));
            Assert.Equal(2, ledger.GetCargoIdsForOriginal(originalId).Count);
        });
        Assert.Throws<InvalidOperationException>(() => ledger.ConvertActiveSpikerToTanker(0));
    }

    [Fact]
    public void Level7DeathPreservesResolvedStateScoreLivesZapperAndDamagedSpikes()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            startingLives: 5, random: new Level7FiringRandom());
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Spiker resolvedSpiker = game.Spikers[0];
        int resolvedId = resolvedSpiker.OriginalSlotId;
        Assert.True(resolvedSpiker.TryZap(1));
        for (int tick = 0; tick < 4 && game.Spikers.Contains(resolvedSpiker); tick++)
            game.RunOneStep();
        Assert.DoesNotContain(game.Spikers, spiker => spiker.OriginalSlotId == resolvedId);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(resolvedId));

        game.Score.Add(1234);
        game.Spikes[0].Height = 35;
        Assert.True(game.SuperZapper.TryActivate(
            game.Flippers.Cast<Enemy>().Concat(game.Tankers).Concat(game.Spikers),
            game.Player.EdgeIndex, game.CurrentLevel.PlayableLaneCount, continuous: true));
        int charges = game.SuperZapperChargesRemaining;
        int flashTicks = game.SuperZapper.FlashTicksRemaining;
        int lives = game.Player.Lives;
        int unresolved = game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count;
        int score = game.Score.Score;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Equal(score, game.Score.Score);
        Assert.Equal(lives - 1, game.Player.Lives);
        Assert.Equal(charges, game.SuperZapperChargesRemaining);
        Assert.Equal(flashTicks - 1, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(35, game.Spikes[0].Height);
        Assert.Equal(unresolved, game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger.GetOriginalState(resolvedId));
        Assert.All(game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected,
                game.CurrentLevel.Level3Ledger.GetOriginalType(id)));
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void Level7ProductionUsesThePendingLaneSpikeAfterItsMinima()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            random: new Level7FiringRandom());
        Egg[] eggs = game.Eggs.ToArray();
        foreach (Egg egg in eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(Tanker.DevelopmentClimbSpeed, tanker.ClimbSpeed);
        Assert.Equal(10 + Tanker.DevelopmentClimbSpeed, tanker.Height);
        Assert.Equal(3, game.Spikers.Count);
        Assert.Equal(2, game.CurrentLevel.Level3Ledger!.CargoCount);
    }

    [Fact]
    public void ProductionPostMinimumSelectorReadsOnlyThePendingEggsLaneSpike()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            random: new Level7FiringRandom());
        Egg[] initialEggs = game.Eggs.ToArray();
        foreach (Egg egg in initialEggs.Take(3))
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Equal(2, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Egg pendingEgg = Assert.Single(game.Eggs);
        for (int tick = 0;
             tick < game.CurrentLevel.PlayableLaneCount * 8 && pendingEgg.EdgeIndex != 10;
             tick++)
            pendingEgg.Update(game.CurrentLevel);
        Assert.Equal(10, pendingEgg.EdgeIndex);

        foreach (Spike spike in game.Spikes)
            spike.Height = 120;
        game.Spikes.Single(spike => spike.EdgeIndex == 2).Height = 60;
        MakeReady(pendingEgg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Equal(Level3OriginalType.Tanker,
            game.CurrentLevel.Level3Ledger!.GetOriginalType(pendingEgg.OriginalSlotId));
        Assert.Equal(2, game.Tankers.Count);
        Assert.Equal(2, game.Spikers.Count);
    }

    [Fact]
    public void SpikerRetainsItsLiveIdentityAcrossRetreatWhileGenericSupplyRemains()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            random: new Level7SpikerLifecycleRandom());
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Spiker spiker = game.Spikers[0];
        int originalId = spiker.OriginalSlotId;
        int pendingBeforeRetreat = game.CurrentLevel.PendingGenericEnemyUnits;
        Assert.True(pendingBeforeRetreat > 0);

        for (int tick = 0; tick < 260; tick++)
            game.RunOneStep();

        Assert.True(game.CurrentLevel.PendingGenericEnemyUnits > 0);
        Assert.Equal(Level3OriginalState.ActiveSpiker,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(originalId));
        Assert.Same(spiker, Assert.Single(
            game.Spikers, candidate => candidate.OriginalSlotId == originalId));
        Assert.DoesNotContain(game.Eggs, egg => egg.OriginalSlotId == originalId);
        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
    }

    [Fact]
    public void CloverSupportsEveryDirectedAdjacentTransitionAndPreservesReflexSweeps()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Vector2[] points = level.Definition.Points.ToArray();
        int[] reflexHinges = [2, 6, 10, 14];

        Assert.Equal(15, level.PreviousEdge(0));
        Assert.Equal(0, level.NextEdge(15));

        for (int source = 0; source < level.PlayableLaneCount; source++)
        {
            foreach (int direction in new[] { 1, -1 })
            {
                int destination = direction > 0
                    ? level.PreviousEdge(source)
                    : level.NextEdge(source);
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(source, destination, direction);

                Assert.Equal(source, transition.SourceLane);
                Assert.Equal(destination, transition.DestinationLane);
                Assert.Equal(direction, transition.Direction);
                Assert.True(float.IsFinite(transition.SweepMagnitudeDegrees));
                Assert.True(transition.SweepMagnitudeDegrees > 0);
                Assert.Equal(reflexHinges.Contains(transition.HingeVertex),
                    transition.SweepMagnitudeDegrees > 180);

                foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(transition, progress);
                    AssertFinite(start);
                    AssertFinite(end);
                    Assert.True(IsInsideOrOnPolygon(start, points),
                        $"Start endpoint escaped at lane {source}, direction {direction}, progress {progress}.");
                    Assert.True(IsInsideOrOnPolygon(end, points),
                        $"End endpoint escaped at lane {source}, direction {direction}, progress {progress}.");
                }
            }
        }
    }

    [Fact]
    public void ProductionProgressionContinuesAfterLevel7AndEarlierLevelsKeepTheirRoute()
    {
        Assert.Same(LevelConfiguration.Level2Square,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level1));
        Assert.Same(LevelConfiguration.Level3Mixed,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level2Square));
        Assert.Same(LevelConfiguration.Level4Candidate,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));
        Assert.Same(LevelConfiguration.Level5Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level4Candidate));
        Assert.Same(LevelConfiguration.Level6Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level5Production));
        Assert.Same(LevelConfiguration.Level7Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level6Production));
        Assert.Same(LevelConfiguration.Level8Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level7Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level8Production));

        foreach (LevelConfiguration configuration in new[]
                 {
                     LevelConfiguration.Level1,
                     LevelConfiguration.Level2Square,
                     LevelConfiguration.Level3Mixed,
                     LevelConfiguration.Level4Candidate,
                     LevelConfiguration.Level6Production
                 })
            Assert.Equal(BelowRimFlipperProgram.Existing, configuration.BelowRimFlipperProgram);
        Assert.Equal(BelowRimFlipperProgram.Level5SpikeSensitive,
            LevelConfiguration.Level5Production.BelowRimFlipperProgram);

        Assert.Equal(150, LevelConfiguration.Level4Candidate.AvoidSpikesWaitThreshold);
        Assert.Equal(150, LevelConfiguration.Level5Production.AvoidSpikesWaitThreshold);
        Assert.Equal(70, LevelConfiguration.Level6Production.AvoidSpikesWaitThreshold);
        Assert.Equal(30, LevelConfiguration.Level7Production.AvoidSpikesWaitThreshold);
        Assert.True(LevelConfiguration.Level7Production.UsesLevel7RomWavePolicy);
        Assert.All(new[]
        {
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production
        }, configuration => Assert.False(configuration.UsesLevel7RomWavePolicy));
    }

    [Fact]
    public void DirectLevel7DiagnosticUsesProductionConfigurationFiringAndFreshReplay()
    {
        string[] arguments = ["--level7-test", "--lives", "10"];
        TempestGame game = new(level7Test: DebugController.UseLevel7Test(arguments),
            startingLives: DebugController.GetStartingLives(arguments),
            random: new Level7FiringRandom());

        Assert.True(DebugController.UseLevel7Test(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--level7-test", "--level6-test"]));
        Assert.Throws<ArgumentException>(() =>
            new TempestGame(level6Test: true, level7Test: true));
        Assert.Equal(10, game.Player.Lives);
        Assert.True(game.Level7Test);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);

        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.Empty(game.Flippers);
        Assert.Equal(3, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(BelowRimFlipperProgram.Level7ArcadeObserved,
            game.CurrentLevel.Configuration.BelowRimFlipperProgram);

        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        AssertFreshLevel7(game);

        game.Score.Add(100);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        AssertFreshLevel7(game);
    }

    private static void AssertFreshLevel7(TempestGame game)
    {
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(20, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(20, game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, game.CurrentLevel.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
    }

    private static (TempestGame Game, Tanker Carrier, int[] PriorCargoIds)
        CreateCapacityLimitedReleaseScenario(int spikerCount, bool convertedTanker = false)
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            random: new Level7FiringRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        ledger.CommitOriginalAsTanker(0);
        Tanker previousCarrier = new(0, 0);
        TankerReleaseResult previousRelease = previousCarrier.ReleaseFlipperCargo(
            game.CurrentLevel, firstSideDirection: 1, TankerReleaseKind.Natural);
        int[] priorCargoIds = ledger.MaterializeLiveCargo(0).ToArray();
        ledger.MarkOriginalEscaped(0);
        List<Flipper> flippers = GetGameList<Flipper>(game, "flippers");
        for (int index = 0; index < priorCargoIds.Length; index++)
            flippers.Add(Flipper.CreateTankerCargo(
                game.CurrentLevel, previousRelease.Cargo[index], priorCargoIds[index]));

        List<Spiker> spikers = GetGameList<Spiker>(game, "spikers");
        for (int index = 0; index < spikerCount; index++)
        {
            int originalId = index + 2;
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(originalId);
            ledger.CommitOriginalAsSpiker(originalId);
            spikers.Add(new Spiker(originalId, speed: 3, originalSlotId: originalId));
        }

        if (convertedTanker)
        {
            ledger.CommitOriginalAsSpiker(1);
            ledger.ConvertActiveSpikerToTanker(1);
        }
        else
            ledger.CommitOriginalAsTanker(1);

        Tanker carrier = new(1, 1);
        GetGameList<Tanker>(game, "tankers").Add(carrier);
        return (game, carrier, priorCargoIds);
    }

    private static void ReleaseTankerCargo(
        TempestGame game,
        Tanker tanker,
        TankerReleaseKind releaseKind)
    {
        Assert.True(GetGameList<Tanker>(game, "tankers").Remove(tanker));
        MethodInfo release = typeof(TempestGame).GetMethod(
            "ReleaseTankerCargo", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Tanker cargo release method was not found.");
        release.Invoke(game, [tanker, releaseKind]);
    }

    private static List<T> GetGameList<T>(object instance, string fieldName)
    {
        FieldInfo field = instance.GetType().GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(instance)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' is null."));
    }

    private static bool CheckForLevelComplete(TempestGame game)
    {
        MethodInfo check = typeof(TempestGame).GetMethod(
            "CheckForLevelComplete", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The level completion check was not found.");
        return (bool)(check.Invoke(game, null)
            ?? throw new InvalidOperationException("The level completion check returned null."));
    }

    private static int CountPhysicalEnemies(TempestGame game) =>
        game.Flippers.Count + game.Tankers.Count + game.Spikers.Count +
        GetGameList<Flipper>(game, "shotReleasedCargoPendingActivation").Count;

    private static void RemovePriorCargoFlippers(TempestGame game, IReadOnlyCollection<int> cargoIds)
    {
        List<Flipper> flippers = GetGameList<Flipper>(game, "flippers");
        foreach (int cargoId in cargoIds)
        {
            Flipper flipper = Assert.Single(flippers,
                candidate => candidate.CargoUnitId == cargoId);
            Assert.True(flippers.Remove(flipper));
        }
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static bool IsInsideOrOnPolygon(Vector2 point, IReadOnlyList<Vector2> polygon)
    {
        bool inside = false;
        for (int index = 0, previous = polygon.Count - 1;
             index < polygon.Count;
             previous = index++)
        {
            Vector2 start = polygon[index];
            Vector2 end = polygon[previous];
            Vector2 edge = end - start;
            Vector2 offset = point - start;
            float cross = edge.X * offset.Y - edge.Y * offset.X;
            if (MathF.Abs(cross) < .001f &&
                point.X >= MathF.Min(start.X, end.X) - .001f &&
                point.X <= MathF.Max(start.X, end.X) + .001f &&
                point.Y >= MathF.Min(start.Y, end.Y) - .001f &&
                point.Y <= MathF.Max(start.Y, end.Y) + .001f)
                return true;

            if ((start.Y > point.Y) != (end.Y > point.Y) &&
                point.X < (end.X - start.X) * (point.Y - start.Y) /
                (end.Y - start.Y) + start.X)
                inside = !inside;
        }
        return inside;
    }

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }

    private sealed class Level7FiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue == 5 ? 1 :
            minValue == 0 && maxValue == 256 ? 255 :
            minValue;
    }

    private sealed class Level7SpikerLifecycleRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
        public override double NextDouble() => 1;
    }
}
