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

        ledger.CommitOriginalAsSpiker(2);
        level.ReturnOriginalToReserve(2);
        Spike persistentSpike = level.Spikes[7];
        persistentSpike.Height = 45;
        ledger.ValidateState();

        level.ResetAfterDeath();

        Assert.Equal(20, ledger.OriginalCount);
        Assert.Equal(2, ledger.CargoCount);
        Assert.Equal(22, ledger.TotalIdentityCount);
        Assert.Equal(19, ledger.UnresolvedOriginalIds.Count);
        Assert.Single(ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(0));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoIds[0]));
        Assert.Contains(ledger.GetCargoState(cargoIds[1]),
            new[] { Level3CargoState.ReentryReserve, Level3CargoState.Egg });
        Assert.Equal(
            ledger.GetCargoState(cargoIds[1]) == Level3CargoState.ReentryReserve ? 1 : 0,
            level.Level3CargoReserveEggs);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(2));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(2));
        Assert.Equal(1, level.EnemiesDestroyed);
        Assert.Equal(19, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Same(persistentSpike, Assert.Single(level.Spikes, spike => spike.EdgeIndex == 7));
        Assert.Equal(45, persistentSpike.Height);
        ledger.ValidateState();
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
    public void FreshLevelFiveFlippersAfterDeathUseTheSpikePolicy()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        TempestGame game = new(configuration, random: new Level5FlipperSelectionRandom());
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Egg reentryEgg = game.Eggs[0];
        MakeReady(reentryEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper reentryFlipper = Assert.Single(game.Flippers);
        float initialHeight = reentryFlipper.Height;

        reentryFlipper.Update(game.CurrentLevel, playerEdge: 2, ThrowOnRandom);

        Assert.True(reentryFlipper.Height > initialHeight);
        Assert.Equal(FlipperState.Climbing, reentryFlipper.State);
    }

    [Fact]
    public void FreshLevelFiveNoInputDeathsAndReentriesRemainStable()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        TempestGame game = new(configuration, random: new Level5FlipperSelectionRandom(),
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
    public void ReconstructedLevelFiveTankerCargoHasDirectionBeforeSpikeGatedPivot()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production with
        {
            EnemySupply = 4,
            InitialActiveEggCount = 4
        };
        TempestGame game = new(configuration, random: new Level5TankerSelectionRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Tanker tanker = Assert.Single(game.Tankers);
        int tankerTicks = 0;
        while (tanker.Height < 250 && tankerTicks++ < 2000)
            game.RunOneStep();

        Assert.InRange(tanker.Height, 250, LevelConfiguration.Level7DeepCargoMaximumHeight);
        game.Player.ResetPosition(tanker.EdgeIndex, 0.5f);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot tankerShot = Assert.Single(game.Shots);
        tankerShot.Height = tanker.Height - 1;
        game.RunOneStep();
        Flipper releasedCargo = Assert.IsType<Flipper>(game.Flippers.First(
            flipper => flipper.IsTankerCargo));
        Assert.InRange(tanker.Height, 250, LevelConfiguration.Level7DeepCargoMaximumHeight);

        ForceFlipperIntoPlayer(game, releasedCargo);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Egg cargoEgg = Assert.Single(game.Eggs,
            egg => egg.CargoUnitId == releasedCargo.CargoUnitId);
        MakeReady(cargoEgg, game.CurrentLevel);
        game.RunOneStep();
        Flipper reconstructedCargo = Assert.Single(game.Flippers,
            flipper => flipper.CargoUnitId == releasedCargo.CargoUnitId);

        Assert.Contains(reconstructedCargo.Direction, new[] { -1, 1 });
        int reconstructedTicks = 0;
        while (reconstructedCargo.State != FlipperState.Flipping &&
            reconstructedTicks++ < 2000)
            game.RunOneStep();

        Assert.Equal(FlipperState.Flipping, reconstructedCargo.State);
        Assert.Contains(reconstructedCargo.Direction, new[] { -1, 1 });
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

    private static void ForceFlipperIntoPlayer(TempestGame game, Flipper flipper)
    {
        int ticks = 0;
        while (!flipper.CompletedIntoPlayer && ticks++ < 2000)
        {
            if (flipper.State == FlipperState.Flipping)
                game.Player.Move(flipper.DestinationEdge - game.Player.EdgeIndex,
                    game.CurrentLevel.PlayableLaneCount);
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex, ThrowOnRandom);
        }

        Assert.True(flipper.CompletedIntoPlayer);
    }

    private sealed class Level5FlipperSelectionRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue == 5 ? 1 : minValue;
    }

    private sealed class Level5TankerSelectionRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue == 5 ? 3 : minValue;
    }
}
