using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level5ProductionTests
{
    private static readonly Vector2[] CrossPoints =
    [
        new(-30, 210), new(-60, 120), new(-120, 60), new(-210, 30),
        new(-210, -30), new(-120, -60), new(-60, -120), new(-30, -210),
        new(30, -210), new(60, -120), new(120, -60), new(210, -30),
        new(210, 30), new(120, 60), new(60, 120), new(30, 210)
    ];

    [Fact]
    public void ProductionConfigurationUsesTheOriginalArcadeCrossAndAcceptedWaveSystems()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production;
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        TempestGame game = new(configuration);

        Assert.Equal(4, configuration.ProgressionIndex);
        Assert.Equal(5, configuration.DisplayedLevel);
        Assert.Equal(4, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(415, configuration.Definition.CenterY);
        Assert.Equal(-90, configuration.Definition.YOffset);
        Assert.Equal(CrossPoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(Level.Depth, configuration.Depth);
        Assert.Equal(Level.Position, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(20, configuration.EnemySupply);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.All(new[]
        {
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate
        }, otherConfiguration =>
            Assert.False(otherConfiguration.UsesGenericRomDeathReconstruction));
        Assert.True(LevelConfiguration.Level6Production.UsesGenericRomDeathReconstruction);
        Assert.True(LevelConfiguration.Level7Production.UsesGenericRomDeathReconstruction);
        Assert.True(LevelConfiguration.Level8Production.UsesGenericRomDeathReconstruction);
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
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.False(configuration.AllowsOrdinaryFlipperSelection);
        Assert.True(LevelConfiguration.Level4Candidate.AllowsOrdinaryFlipperSelection);
        Assert.True(LevelConfiguration.Level6Production.AllowsOrdinaryFlipperSelection);
        Assert.True(configuration.FlipperCanFlip);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.Equal(BelowRimFlipperProgram.Level5SpikeSensitive,
            configuration.BelowRimFlipperProgram);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.Same(LevelConfiguration.Level6Production,
            LevelProgression.GetNextProductionLevel(configuration));
    }

    [Fact]
    public void LegacyMixedPickerCanExcludeOrdinaryFlippers()
    {
        HashSet<Level3OriginalType> selectedTypes = [];

        for (int seed = 0; seed < 32; seed++)
        {
            Random random = new(seed);
            Level3OriginalType selected = Level3WaveSelection.SelectLevel4OriginalType(
                (minimum, maximum) => random.Next(minimum, checked(maximum + 1)),
                activeTankers: 0,
                activeSpikers: 0,
                activeEnemyUnits: 0,
                activeEggs: 4,
                allowFlippers: false);

            Assert.NotEqual(Level3OriginalType.Flipper, selected);
            selectedTypes.Add(selected);
        }

        Assert.Contains(Level3OriginalType.Tanker, selectedTypes);
        Assert.Contains(Level3OriginalType.Spiker, selectedTypes);
    }

    [Fact]
    public void LevelFiveProductionUsesOnlyEligibleOrdinaryTypesAndPreservesCargoPairs()
    {
        TempestGame game = new(LevelConfiguration.Level5Production,
            random: new Random(1));
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.InRange(game.Tankers.Count, 0, Level5AdmissionPolicy.TankerMaximum);
        Assert.InRange(game.Spikers.Count, 0, Level5AdmissionPolicy.SpikerMaximum);
        Assert.Equal(4, game.Tankers.Count + game.Spikers.Count);
        Assert.Equal(20, ledger.OriginalCount);
        Assert.Equal(game.Tankers.Count * 2, ledger.CargoCount);
        Assert.Equal(20 + game.Tankers.Count * 2, ledger.TotalIdentityCount);
        Assert.Equal(20, ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(game.Tankers.Count * 2, ledger.UnresolvedCargoIds.Count);
        Assert.Equal(20, game.CurrentLevel.RemainingEnemies);

        foreach (Tanker tanker in game.Tankers)
        {
            IReadOnlyList<int> cargoIds = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId);
            Assert.Equal(2, cargoIds.Count);
            Assert.All(cargoIds, cargoId =>
                ledger.ValidateCargoOwner(cargoId, tanker.OriginalSlotId));
        }
    }

    [Fact]
    public void CopiedLevelFiveConfigurationDoesNotAcquireCanonicalDiagnosticPolicy()
    {
        LevelConfiguration copiedConfiguration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        Assert.False(copiedConfiguration.UsesGenericRomDeathReconstruction);
        TempestGame game = new(copiedConfiguration,
            random: new Level5TankerSelectionRandom());
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Equal(4, game.Tankers.Count);
        Assert.Empty(game.Spikers);
        Assert.Same(copiedConfiguration, game.CurrentLevel.Configuration);
    }

    [Fact]
    public void LevelFiveLedgerRebuildPreservesDestroyedAndCargoIdentitiesWithoutDuplication()
    {
        Level level = new(LevelConfiguration.Level5Production, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;

        ledger.CommitOriginalAsFlipper(0);
        ledger.MarkOriginalDestroyed(0);
        level.RecordEnemyDestroyed();

        ledger.CommitOriginalAsTanker(1);
        IReadOnlyList<int> cargoIds = ledger.MaterializeLiveCargo(1);
        ledger.MarkCargoDestroyed(cargoIds[0]);
        ledger.MarkOriginalEscaped(1);

        ledger.CommitOriginalAsSpiker(2);
        level.ReturnOriginalToReserve(2);
        Spike persistentSpike = level.Spikes[7];
        persistentSpike.Height = 45;
        ledger.ValidateState();

        level.ResetAfterDeath();

        Assert.Equal(21, ledger.OriginalCount);
        Assert.Equal(2, ledger.CargoCount);
        Assert.Equal(23, ledger.TotalIdentityCount);
        Assert.Equal(19, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(0));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoIds[0]));
        Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(cargoIds[1]));
        Assert.Equal(0, level.Level3CargoReserveEggs);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(2));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(2));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(20));
        Assert.Equal(1, level.EnemiesDestroyed);
        Assert.Equal(19, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(15, level.ReserveEggs);
        Assert.Same(persistentSpike, Assert.Single(level.Spikes, spike => spike.EdgeIndex == 7));
        Assert.Equal(45, persistentSpike.Height);
        ledger.ValidateState();
    }

    [Fact]
    public void LevelFiveDeathReentryClearsAllUnresolvedSpeciesAndLatentCargo()
    {
        Level level = new(LevelConfiguration.Level5Production, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        int flipperOriginalId = level.Eggs[0].OriginalSlotId;
        int tankerOriginalId = level.Eggs[1].OriginalSlotId;

        ledger.CommitOriginalAsFlipper(flipperOriginalId);
        ledger.CommitOriginalAsTanker(tankerOriginalId);

        level.ResetAfterDeath();

        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(flipperOriginalId));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(tankerOriginalId));
        Assert.Equal(20, ledger.OriginalCount);
        Assert.Equal(0, ledger.CargoCount);
        Assert.Equal(20, ledger.TotalIdentityCount);
        Assert.Equal(20, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Empty(ledger.GetCargoIdsForOriginal(tankerOriginalId));
        ledger.ValidateState();
    }

    [Fact]
    public void LevelFiveCompletionWaitsForEveryOriginalAndTankerCargoObligation()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production with
        {
            InitialActiveEggCount = 0
        };
        Level level = new(configuration);
        Level3WaveLedger ledger = level.Level3Ledger!;
        IReadOnlyList<int> cargoIds = [];

        for (int originalId = 0; originalId < ledger.OriginalCount; originalId++)
        {
            ledger.MarkOriginalEgg(originalId);
            if (originalId == 0)
            {
                ledger.CommitOriginalAsTanker(originalId);
                cargoIds = ledger.MaterializeLiveCargo(originalId);
            }
            else
            {
                ledger.CommitOriginalAsSpiker(originalId);
            }

            ledger.MarkOriginalDestroyed(originalId);
        }

        Assert.False(level.IsComplete(activeFlippers: 0));
        ledger.MarkCargoDestroyed(cargoIds[0]);
        Assert.False(level.IsComplete(activeFlippers: 0));
        ledger.MarkCargoDestroyed(cargoIds[1]);
        Assert.True(level.IsComplete(activeFlippers: 0));
    }

    [Fact]
    public void LevelFiveLifeLossPreservesScoreLivesSpikeAndRemainingZapperCharge()
    {
        TempestGame game = new(LevelConfiguration.Level5Production, startingLives: 5);
        Level level = game.CurrentLevel;
        Spike[] spikes = game.Spikes.ToArray();
        spikes[game.Player.EdgeIndex].Height = 45;
        game.Score.Add(325);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        level.State = LevelState.PlayerDying;

        game.RunOneStep();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level5Production, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.Player.Lives);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(20, level.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Equal(spikes, game.Spikes);
        Assert.Equal(45, game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height);
    }

    [Fact]
    public void OrdinaryFlipperKeepsClimbingBelowTheLiveSpikeTipWithoutRandomLockDecisions()
    {
        Level level = CreateSpikePolicyLevel();
        Flipper flipper = new(edge: 0, height: 20);

        for (int tick = 0; tick < 5; tick++)
            flipper.Update(level, playerEdge: 2, ThrowOnRandom);

        Assert.Equal(30, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Equal(0, flipper.EdgeIndex);
    }

    [Theory]
    [InlineData(58, 60)]
    [InlineData(60, 62)]
    public void EqualityAndAboveTheSpikeTipPermitTheNextLaneChange(
        float initialHeight, float expectedHeight)
    {
        Level level = CreateSpikePolicyLevel();
        Flipper flipper = new(edge: 0, height: initialHeight);

        flipper.Update(level, playerEdge: 2, (_, _) => 0);

        Assert.Equal(expectedHeight, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(0, flipper.PauseTicks);
    }

    [Fact]
    public void ShortenedDestroyedAndLiveSpikesUseTheirCurrentTipHeight()
    {
        Level shortenedLevel = CreateSpikePolicyLevel();
        shortenedLevel.Spikes.Single(spike => spike.EdgeIndex == 0).Height = 30;
        Flipper shortened = new(edge: 0, height: 28);
        shortened.Update(shortenedLevel, playerEdge: 2, (_, _) => 0);
        Assert.Equal(FlipperState.Pausing, shortened.State);

        Level liveLevel = CreateSpikePolicyLevel();
        Spike liveSpike = liveLevel.Spikes.Single(spike => spike.EdgeIndex == 0);
        Flipper live = new(edge: 0, height: 28);
        live.Update(liveLevel, playerEdge: 2, ThrowOnRandom);
        Assert.Equal(FlipperState.Climbing, live.State);
        liveSpike.Height = 30;
        live.Update(liveLevel, playerEdge: 2, (_, _) => 0);
        Assert.Equal(FlipperState.Pausing, live.State);

        Level destroyedLevel = CreateSpikePolicyLevel();
        Spike destroyedSpike = destroyedLevel.Spikes.Single(spike => spike.EdgeIndex == 0);
        destroyedLevel.RemoveSpike(destroyedSpike);
        Flipper destroyed = new(edge: 0, height: 20);
        destroyed.Update(destroyedLevel, playerEdge: 2, (_, _) => 0);
        Assert.Equal(FlipperState.Pausing, destroyed.State);

        Level depletedLevel = CreateSpikePolicyLevel();
        depletedLevel.Spikes.Single(spike => spike.EdgeIndex == 0).Height = 0;
        Flipper depleted = new(edge: 0, height: 20);
        depleted.Update(depletedLevel, playerEdge: 2, (_, _) => 0);
        Assert.Equal(FlipperState.Pausing, depleted.State);
    }

    [Fact]
    public void EnteringASpikeLaneResumesClimbingBeforeTestingThatLanesLiveTip()
    {
        Level level = CreateSpikePolicyLevel();
        foreach (Spike spike in level.Spikes.ToArray())
        {
            if (spike.EdgeIndex != 15)
                level.RemoveSpike(spike);
        }

        Flipper flipper = new(edge: 0, height: 18);
        flipper.Update(level, playerEdge: 14, (_, _) => 0);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        flipper.Update(level, playerEdge: 14, (_, _) => 0);
        Assert.Equal(15, flipper.DestinationEdge);
        Assert.Equal(FlipperState.Flipping, flipper.State);

        flipper.Update(level, playerEdge: 14, (_, _) => 0);
        Assert.Equal(15, flipper.EdgeIndex);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        float heightAfterFlip = flipper.Height;

        flipper.Update(level, playerEdge: 14, (_, _) => 0);

        Assert.True(flipper.Height > heightAfterFlip);
        Assert.True(flipper.Height < level.Spikes.Single().Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);

        for (int tick = 0; tick < 30 && flipper.State == FlipperState.Climbing; tick++)
            flipper.Update(level, playerEdge: 14, (_, _) => 0);

        Assert.True(flipper.Height >= level.Spikes.Single().Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void LevelsFourAndSixKeepBelowRimFlippingAndLevelSevenKeepsItsAcceptedRoute()
    {
        foreach (LevelConfiguration configuration in new[]
        {
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level6Production
        })
        {
            Level level = CreateSpikePolicyLevel(configuration);
            Flipper flipper = new(edge: 0, height: 20);

            flipper.Update(level, playerEdge: 2, (_, _) => 0);

            Assert.Equal(FlipperState.Pausing, flipper.State);
        }

        Level levelSeven = CreateSpikePolicyLevel(LevelConfiguration.Level7Production);
        Flipper levelSevenFlipper = new(edge: 0);
        levelSevenFlipper.Update(levelSeven, playerEdge: 2, ThrowOnRandom);
        levelSevenFlipper.Update(levelSeven, playerEdge: 2, ThrowOnRandom);

        Assert.Equal(FlipperState.Flipping, levelSevenFlipper.State);
        Assert.Equal(levelSeven.PreviousEdge(0), levelSevenFlipper.DestinationEdge);
    }

    [Fact]
    public void NaturalTankerCargoStaysStraightButDeepPlayerShotCargoUsesTheSpikeGate()
    {
        Level level = CreateSpikePolicyLevel();
        Tanker naturalTanker = new(edgeIndex: 8, originalSlotId: 0);
        while (naturalTanker.UpdateForDevelopment() == TankerUpdateResult.Active)
        {
        }
        TankerReleaseResult naturalRelease = naturalTanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);
        Assert.False(naturalTanker.UsesLevel5SpikeSensitiveCargo(
            level.Configuration, naturalRelease.Kind));

        Tanker deepTanker = new(edgeIndex: 8, originalSlotId: 1);
        while (deepTanker.Height < 250)
            deepTanker.UpdateForDevelopment();
        TankerReleaseResult deepRelease = deepTanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.PlayerShotDestruction);
        Assert.True(deepTanker.UsesLevel5SpikeSensitiveCargo(
            level.Configuration, deepRelease.Kind));

        Tanker deepBoundary = new(edgeIndex: 8, originalSlotId: 2, climbSpeed: 360);
        deepBoundary.UpdateForDevelopment();
        Assert.Equal(LevelConfiguration.Level7DeepCargoMaximumHeight, deepBoundary.Height);
        Assert.True(deepBoundary.UsesLevel5SpikeSensitiveCargo(
            level.Configuration, TankerReleaseKind.PlayerShotDestruction));

        Tanker nearRimShot = new(edgeIndex: 8, originalSlotId: 3, climbSpeed: 361);
        nearRimShot.UpdateForDevelopment();
        Assert.False(nearRimShot.UsesLevel5SpikeSensitiveCargo(
            level.Configuration, TankerReleaseKind.PlayerShotDestruction));

        TankerCargoSpawn deepSpawn = deepRelease.Cargo[0];
        level.RemoveSpike(level.Spikes.Single(spike => spike.EdgeIndex == deepSpawn.DestinationLane));
        Flipper deepCargo = Flipper.CreateTankerCargo(
            level, deepSpawn, cargoUnitId: 0,
            useLevel5SpikeSensitiveProgram: true);
        deepCargo.Update(level, playerEdge: 2, ThrowOnRandom);
        Assert.Equal(FlipperState.Pausing, deepCargo.State);
        deepCargo.Update(level, playerEdge: 2, ThrowOnRandom);

        Assert.Equal(FlipperState.Flipping, deepCargo.State);

        Flipper naturalCargo = Flipper.CreateTankerCargo(
            level, naturalRelease.Cargo[0], cargoUnitId: 1);
        naturalCargo.Update(level, playerEdge: 2, ThrowOnRandom);
        naturalCargo.Update(level, playerEdge: 2, ThrowOnRandom);
        Assert.Equal(FlipperState.Pausing, naturalCargo.State);
        Assert.Equal(naturalRelease.Cargo[0].DestinationLane, naturalCargo.EdgeIndex);

        LevelConfiguration tankerConfiguration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4,
            Flipper = new(true, 2, 360, 0, 24)
        };
        TempestGame game = new(tankerConfiguration, random: new Level5TankerSelectionRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Tanker gameTanker = Assert.Single(game.Tankers);
        while (gameTanker.Height < 250)
            game.RunOneStep();

        game.Player.ResetPosition(gameTanker.EdgeIndex, 0.5f);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot tankerShot = Assert.Single(game.Shots);
        tankerShot.Height = gameTanker.Height - 1;
        game.RunOneStep();
        Flipper[] integratedCargo = game.Flippers
            .Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId)
            .ToArray();
        Assert.Equal(2, integratedCargo.Length);
        Assert.Equal(new[] { 1, -1 }, integratedCargo.Select(flipper => flipper.Direction));
        game.CurrentLevel.RemoveSpike(game.Spikes.Single(
            spike => spike.EdgeIndex == integratedCargo[0].DestinationEdge));

        integratedCargo[0].Update(game.CurrentLevel, playerEdge: 2, ThrowOnRandom);
        integratedCargo[0].Update(game.CurrentLevel, playerEdge: 2, ThrowOnRandom);

        Assert.Equal(FlipperState.Flipping, integratedCargo[0].State);
    }

    [Fact]
    public void LevelFiveDeathReentryReappliesTheNoDirectFlipperPolicy()
    {
        TempestGame game = new(LevelConfiguration.Level5Production,
            random: new Level5ScriptedSelectionRandom(1, 0, 1, 3, 1, 4, 1, 0),
            startingLives: 10);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        for (int death = 0; death < 2; death++)
        {
            game.CurrentLevel.State = LevelState.PlayerDying;
            game.RunOneStep();

            Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
            Assert.Equal(20, ledger.OriginalCount);
            Assert.Equal(20, ledger.UnresolvedOriginalIds.Count);
            Assert.Equal(0, ledger.CargoCount);
            Assert.Equal(4, game.Eggs.Count);
            Assert.Equal(16, game.CurrentLevel.ReserveEggs);
            Assert.Equal(20, game.CurrentLevel.RemainingEnemies);
        }

        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.InRange(game.Tankers.Count, 0, Level5AdmissionPolicy.TankerMaximum);
        Assert.InRange(game.Spikers.Count, 0, Level5AdmissionPolicy.SpikerMaximum);
        Assert.Equal(4, game.Tankers.Count + game.Spikers.Count);
        Assert.Equal(20, ledger.OriginalCount);
        Assert.Equal(20, ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(game.Tankers.Count * 2, ledger.CargoCount);
        Assert.Equal(20 + game.Tankers.Count * 2, ledger.TotalIdentityCount);
        Assert.Equal(20, game.CurrentLevel.RemainingEnemies);
    }

    [Fact]
    public void FreshLevelFiveNoInputDeathsAndReentriesRemainStable()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        TempestGame game = new(configuration, random: new Level5TankerSelectionRandom(),
            startingLives: 10);

        for (int death = 0; death < 2; death++)
        {
            game.CurrentLevel.State = LevelState.PlayerDying;
            game.RunOneStep();
            Assert.Equal(LevelState.Playing, game.State);

            for (int tick = 0; tick < 200; tick++)
                game.RunOneStep();

            Assert.NotEqual(LevelState.GameOver, game.State);
        }
    }

    [Fact]
    public void DeathReconstructsReleasedLevelFiveCargoAsGenericObligations()
    {
        TempestGame game = new(LevelConfiguration.Level5Production,
            random: new Level5CargoReconstructionRandom(), startingLives: 10);
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();
        Tanker tanker = Assert.Single(game.Tankers);
        int tankerTicks = 0;
        while (!tanker.CargoReleased && tankerTicks++ < 2000)
            game.RunOneStep();

        Assert.True(tanker.CargoReleased);
        Assert.NotEqual(0, tanker.EdgeIndex);
        Flipper[] releasedCargo = game.Flippers
            .Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId)
            .ToArray();
        Assert.Equal(2, releasedCargo.Length);
        Assert.All(releasedCargo, flipper => Assert.NotEqual(0, flipper.EdgeIndex));
        int[] cargoIds = releasedCargo.Select(flipper => flipper.CargoUnitId).ToArray();
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int unresolvedBeforeDeath =
            ledger.UnresolvedOriginalIds.Count + ledger.UnresolvedCargoIds.Count;
        Assert.DoesNotContain(game.Eggs, egg => cargoIds.Contains(egg.CargoUnitId));

        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Empty(game.Flippers);
        Assert.DoesNotContain(game.Eggs, egg => cargoIds.Contains(egg.CargoUnitId));
        Assert.All(game.Eggs, egg =>
        {
            Assert.True(egg.OriginalSlotId >= 0);
            Assert.Equal(-1, egg.CargoUnitId);
        });
        Assert.All(cargoIds, id =>
            Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(unresolvedBeforeDeath,
            ledger.UnresolvedOriginalIds.Count + ledger.UnresolvedCargoIds.Count);
        Assert.False(game.CurrentLevel.IsComplete(activeFlippers: game.Flippers.Count));

        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.Empty(game.Flippers);
        Assert.Equal(4, game.Tankers.Count + game.Spikers.Count);
        Assert.All(game.Tankers, tanker =>
            Assert.Contains(ledger.GetOriginalType(tanker.OriginalSlotId),
                new[] { Level3OriginalType.Tanker, Level3OriginalType.Spiker }));
        Assert.All(game.Spikers, spiker =>
            Assert.Equal(Level3OriginalType.Spiker,
                ledger.GetOriginalType(spiker.OriginalSlotId)));
    }

    private static Level CreateSpikePolicyLevel(
        LevelConfiguration? configuration = null)
    {
        LevelConfiguration selected = configuration ?? LevelConfiguration.Level5Production;
        return new Level(selected with
        {
            Flipper = new(true, 2, 360, 0, 24)
        });
    }

    private static int ThrowOnRandom(int minimum, int maximum) =>
        throw new InvalidOperationException($"Unexpected random decision [{minimum}, {maximum}].");

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private sealed class Level5ScriptedSelectionRandom : Random
    {
        private readonly Queue<int> selection = new();

        public Level5ScriptedSelectionRandom(params int[] candidates)
        {
            foreach (int candidate in candidates)
                selection.Enqueue(candidate);
        }

        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue is 4 or 5
                ? selection.Dequeue()
                : minValue;
    }

    private sealed class Level5TankerSelectionRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 || minValue == 1 && maxValue >= 4
                ? Math.Min(3, maxValue - 1)
                : minValue;
    }

    private sealed class Level5CargoReconstructionRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == 0 && maxValue == 16 ? 3 : minValue;
    }
}
