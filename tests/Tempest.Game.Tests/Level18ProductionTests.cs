using System.Numerics;
using System.Reflection;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level18ProductionTests
{
    [Fact]
    public void ProductionIdentityReusesLevelTwoShapeAndCycleOnePalette()
    {
        LevelConfiguration level18 = LevelConfiguration.Level18Production;

        Assert.Same(level18, LevelProgression.GetProductionLevel(18));
        Assert.Same(level18, LevelProgression.GetDirectStartLevel(18));
        Assert.Same(level18, LevelProgression.GetNextProductionLevel(
            LevelConfiguration.Level17Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(level18));
        Assert.Null(LevelProgression.GetProductionLevel(19));
        Assert.Same(LevelConfiguration.Level2Square.Definition, level18.Definition);
        Assert.Equal((17, 18, 18, 1, 1, 1, 1),
            (level18.ProgressionIndex, level18.DisplayedLevel,
             level18.EffectiveGameplayParameterLevel, level18.BoardIndex,
             level18.PaletteIndex, RomTubeMappingCatalog.ResolveShapeRow(18),
             RomPaletteCatalog.PaletteIndexForDisplayedLevel(18)));
        Assert.True(level18.Definition.Continuous);
        Assert.Equal(16, level18.PlayableLaneCount);
        Assert.Null(level18.StartingSpikeHeight);
        Assert.Equal(0x0B, Level18WaveSelection.FlipperMovementProgram);
    }

    [Fact]
    public void MediumConfigurationEnablesOnlyEligibleSpeciesAndPreservesWaveEnvelope()
    {
        LevelConfiguration configuration = LevelConfiguration.Level18Production;
        Level level = new(configuration);
        Level3WaveLedger ledger = level.Level3Ledger!;

        Assert.Equal(21, configuration.EnemySupply);
        Assert.Equal(21, ledger.OriginalCount);
        Assert.Equal(21, ledger.TotalIdentityCount);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(4, configuration.MaximumActiveEggs);
        Assert.Equal(2, configuration.MaximumApproachingEggs);
        Assert.Equal(7, Level18WaveSelection.MaximumPhysicalEnemySlots);
        Assert.Equal((0, 3, 1, 1, 0, 0, 2, 5, 0, 0),
            (Level18WaveSelection.MinimumFlippers, Level18WaveSelection.MaximumFlippers,
             Level18WaveSelection.MinimumTankers, Level18WaveSelection.MaximumTankers,
             Level18WaveSelection.MinimumSpikers, Level18WaveSelection.MaximumSpikers,
             Level18WaveSelection.MinimumPulsars, Level18WaveSelection.MaximumPulsars,
             Level18WaveSelection.MinimumFuseballs, Level18WaveSelection.MaximumFuseballs));
        Assert.True(configuration.AllowsOrdinaryFlipperSelection);
        Assert.Null(configuration.StartingSpikeHeight);
        Assert.Empty(level.Spikes);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.False(Level18WaveSelection.MediumPulsarFiringEnabled);
        Assert.Equal((29, 3), (configuration.FlipperShotHoldoffTicks,
            configuration.MaximumActiveEnemyShots));
        Assert.Equal(2.625f, configuration.Flipper.ClimbSpeed);
        Assert.Equal(configuration.Flipper.ClimbSpeed,
            new Tanker(0, originalSlotId: 0,
                climbSpeed: configuration.Flipper.ClimbSpeed).ClimbSpeed);
        Assert.Equal((byte)0xAC, Level18WaveSelection.FlipperSpeedRaw);
        Assert.Equal((byte)0x1D, Level18WaveSelection.ShotHoldoffRaw);
        Assert.Equal((byte)0x02, Level18WaveSelection.DefaultEnemyShotLastSlotIndex);
        Assert.Equal((byte)0x04, Level18WaveSelection.PulseBeatRaw);
        Assert.Equal((byte)0x01, Level18WaveSelection.TankerCargoType);
        EnemyBullet sharedBullet = new(edgeIndex: 0, height: 10);
        sharedBullet.Advance();
        Assert.Equal(13, sharedBullet.Height);
    }

    [Fact]
    public void SelectorSatisfiesMinimaAndHonorsCargoAndSevenRecordCapacity()
    {
        Assert.Equal(Level3OriginalType.Pulsar,
            Level18WaveSelection.SelectOriginalType((_, _) => 0, 0, 0, 0, 0, 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level18WaveSelection.SelectOriginalType((_, _) => 0, 0, 0, 2, 2, 2));
        Assert.Equal(Level3OriginalType.Flipper,
            Level18WaveSelection.SelectOriginalType((_, maximum) => maximum, 1, 0, 2, 3, 2));
        Assert.Null(Level18WaveSelection.SelectOriginalType(
            (_, _) => 0, 1, 0, 5, 7, 3));
        Assert.False(Level18WaveSelection.CanAdmitCargoFlipper(7));
        Assert.True(Level18WaveSelection.CanAdmitCargoFlipper(6));
        Assert.False(Level18WaveSelection.CanAdmitOriginalType(
            Level3OriginalType.Tanker, 6));

        for (int seed = 0; seed < 24; seed++)
        {
            Random random = new(seed);
            int tankers = 0;
            int pulsars = 0;
            int flipperUnits = 0;
            int occupiedUnits = 0;

            for (int admission = 0; admission < Level18WaveSelection.MaximumPhysicalEnemySlots; admission++)
            {
                Level3OriginalType? selected = Level18WaveSelection.SelectOriginalType(
                    (minimum, maximum) => random.Next(minimum, maximum + 1),
                    tankers, activeSpikers: 0, pulsars, occupiedUnits, flipperUnits);
                if (selected is null)
                    break;

                switch (selected)
                {
                    case Level3OriginalType.Tanker:
                        tankers++;
                        flipperUnits += Tanker.CarriedFlipperCount;
                        occupiedUnits += 2;
                        break;
                    case Level3OriginalType.Pulsar:
                        pulsars++;
                        occupiedUnits++;
                        break;
                    case Level3OriginalType.Flipper:
                        flipperUnits++;
                        occupiedUnits++;
                        break;
                    default:
                        Assert.Fail($"Ineligible species selected: {selected}.");
                        break;
                }
            }

            Assert.Equal(1, tankers);
            Assert.InRange(pulsars, 2, 5);
            Assert.InRange(flipperUnits - Tanker.CarriedFlipperCount, 0, 3);
        }
    }

    [Fact]
    public void MixedEnemyFactorySelectsOnlyEligibleLevelEighteenActors()
    {
        TempestGame game = new(startingProductionLevel: 18, random: new MaximumRandom());
        Level level = game.CurrentLevel;
        List<Egg> eggs = GetPrivateList<Egg>(level, "eggs");
        Egg[] initialEggs = eggs.ToArray();
        List<Type> selectedTypes = [];

        foreach (Egg egg in initialEggs)
        {
            Enemy enemy = Assert.IsAssignableFrom<Enemy>(
                InvokePrivate(game, "CreateLevel3MixedEnemy", egg));
            eggs.Remove(egg);
            InvokePrivate(game, "AddHatchedEnemy", enemy);
            selectedTypes.Add(enemy.GetType());
        }

        Assert.Contains(typeof(Tanker), selectedTypes);
        Assert.Equal(2, selectedTypes.Count(type => type == typeof(Pulsar)));
        Assert.Contains(typeof(Flipper), selectedTypes);
        Assert.DoesNotContain(typeof(Spiker), selectedTypes);
        Assert.DoesNotContain(typeof(Fuseball), selectedTypes);
        Assert.Single(game.Tankers);
        Assert.Collection(game.Pulsars,
            pulsar => Assert.Equal(level.Configuration.PulsarMovement,
                pulsar.MovementConfiguration),
            pulsar => Assert.Equal(level.Configuration.PulsarMovement,
                pulsar.MovementConfiguration));
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Spikes);
        Assert.Equal(21, level.Level3Ledger!.OriginalCount);
        Assert.Collection(level.Level3Ledger.UnresolvedCargoIds,
            cargoId => Assert.Equal(Level3CargoState.Latent,
                level.Level3Ledger.GetCargoState(cargoId)),
            cargoId => Assert.Equal(Level3CargoState.Latent,
                level.Level3Ledger.GetCargoState(cargoId)));
        InvokePrivate(game, "ValidateLevel3WaveRepresentation");
    }

    [Fact]
    public void TankerCargoKeepsSeparateOwnerAndOrdinalIdentitiesAndSelectsSharedRoute()
    {
        LevelConfiguration configuration = LevelConfiguration.Level18Production;
        Tanker tanker = new(0, originalSlotId: 5, climbSpeed: configuration.Flipper.ClimbSpeed);

        Assert.Equal(TankerCargoMovementProgram.EightAdvanceTurn,
            tanker.GetCargoMovementProgram(configuration, TankerReleaseKind.Natural));
        Assert.Equal(TankerCargoMovementProgram.EightAdvanceTurn,
            tanker.GetCargoMovementProgram(configuration, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.EightAdvanceTurn,
            tanker.GetCargoMovementProgram(configuration, TankerReleaseKind.SuperZapperDestruction));

        Level3WaveLedger ledger = new(configuration.EnemySupply);
        ledger.MarkOriginalEgg(5);
        ledger.CommitOriginalAsTanker(5);
        IReadOnlyList<int> cargoIds = ledger.GetCargoIdsForOriginal(5);
        Assert.Equal(2, cargoIds.Count);
        Assert.All(cargoIds, cargoId => ledger.ValidateCargoOwner(cargoId, 5));
        Assert.Equal(new[] { 0, 1 },
            cargoIds.Select(ledger.GetCargoOrdinal).ToArray());
    }

    [Fact]
    public void FlipperFiringUsesLevelEighteenHoldoffAndSharedShotCapacity()
    {
        TempestGame game = new(startingProductionLevel: 18, random: new MaximumRandom());
        List<Flipper> flippers = GetPrivateList<Flipper>(game, "flippers");
        List<EnemyBullet> bullets = GetPrivateList<EnemyBullet>(game, "enemyBullets");
        Flipper flipper = new(edge: 0, height: 100);
        flippers.Add(flipper);
        for (int index = 0; index < Level18WaveSelection.DefaultMaximumActiveEnemyShots; index++)
            bullets.Add(new EnemyBullet(index, 20));

        InvokePrivate(game, "UpdateFlipperFiring");
        Assert.Collection(bullets,
            bullet => Assert.Equal(20, bullet.Height),
            bullet => Assert.Equal(20, bullet.Height),
            bullet => Assert.Equal(20, bullet.Height));
        Assert.Equal(0, flipper.ShotDelayTicks);

        bullets.Clear();
        InvokePrivate(game, "UpdateFlipperFiring");
        Assert.Single(bullets);
        Assert.Equal(29, flipper.ShotDelayTicks);
        Assert.Equal(flipper.Height, bullets[0].Height);

        for (int tick = 0; tick < 29; tick++)
            InvokePrivate(game, "UpdateFlipperFiring");
        Assert.Single(bullets);
        Assert.Equal(0, flipper.ShotDelayTicks);
        InvokePrivate(game, "UpdateFlipperFiring");
        Assert.Collection(bullets,
            bullet => Assert.Equal(100, bullet.Height),
            bullet => Assert.Equal(100, bullet.Height));
        Assert.Equal(29, flipper.ShotDelayTicks);
        bullets[0].Advance();
        Assert.Equal(flipper.Height + 3, bullets[0].Height);
    }

    [Fact]
    public void TankerFiringUsesLevelEighteenHoldoffAndSharedShotCapacity()
    {
        TempestGame game = new(startingProductionLevel: 18, random: new MaximumRandom());
        List<Tanker> tankers = GetPrivateList<Tanker>(game, "tankers");
        List<EnemyBullet> bullets = GetPrivateList<EnemyBullet>(game, "enemyBullets");
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 10,
            climbSpeed: Level18WaveSelection.MediumFlipperTankerSpeed,
            bulletAmmunition: 0, bulletTimer: 0, unlimitedBulletAmmunition: true);
        tankers.Add(tanker);
        for (int index = 0; index < Level18WaveSelection.DefaultMaximumActiveEnemyShots; index++)
            bullets.Add(new EnemyBullet(index, 20));

        InvokePrivate(game, "UpdateTankers");
        Assert.Collection(bullets,
            bullet => Assert.Equal(20, bullet.Height),
            bullet => Assert.Equal(20, bullet.Height),
            bullet => Assert.Equal(20, bullet.Height));
        Assert.Equal(0, tanker.BulletTimer);

        bullets.Clear();
        InvokePrivate(game, "UpdateTankers");
        Assert.Single(bullets);
        Assert.Equal(29, tanker.BulletTimer);
        Assert.Equal(tanker.Height, bullets[0].Height);
        float firstShotHeight = bullets[0].Height;

        for (int tick = 0; tick < 29; tick++)
            InvokePrivate(game, "UpdateTankers");
        Assert.Single(bullets);
        Assert.Equal(0, tanker.BulletTimer);
        InvokePrivate(game, "UpdateTankers");
        Assert.Collection(bullets,
            bullet => Assert.Equal(firstShotHeight, bullet.Height),
            bullet => Assert.Equal(tanker.Height, bullet.Height));
        Assert.Equal(29, tanker.BulletTimer);
    }

    [Theory]
    [InlineData(TankerReleaseKind.Natural, Level3OriginalState.Escaped, Level3CargoState.Active)]
    [InlineData(TankerReleaseKind.PlayerShotDestruction, Level3OriginalState.Destroyed,
        Level3CargoState.PendingShotActivation)]
    [InlineData(TankerReleaseKind.SuperZapperDestruction, Level3OriginalState.Destroyed,
        Level3CargoState.Active)]
    public void ProductionTankerReleasePreservesBothCargoIdentitiesAndSharedRoute(
        TankerReleaseKind releaseKind,
        Level3OriginalState expectedOriginalState,
        Level3CargoState expectedCargoState)
    {
        TempestGame game = new(startingProductionLevel: 18, random: new MaximumRandom());
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        List<Egg> eggs = GetPrivateList<Egg>(level, "eggs");
        eggs.Remove(eggs.Single(egg => egg.OriginalSlotId == 0));
        ledger.CommitOriginalAsTanker(0);

        Tanker tanker = new(0, originalSlotId: 0,
            climbSpeed: Level18WaveSelection.MediumFlipperTankerSpeed);
        InvokePrivate(game, "ReleaseTankerCargo", tanker, releaseKind);

        IReadOnlyList<int> cargoIds = ledger.GetCargoIdsForOriginal(0);
        Assert.Equal(2, cargoIds.Count);
        Assert.Equal(expectedOriginalState, ledger.GetOriginalState(0));
        Assert.All(cargoIds, cargoId =>
        {
            ledger.ValidateCargoOwner(cargoId, 0);
            Assert.Equal(expectedCargoState, ledger.GetCargoState(cargoId));
        });
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.Equal(releaseKind == TankerReleaseKind.Natural ? 0 : 200,
            game.Score.Score);

        if (releaseKind == TankerReleaseKind.PlayerShotDestruction)
            InvokePrivate(game, "ActivateShotReleasedCargo");

        Flipper[] children = game.Flippers.OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(cargoIds.Order(), children.Select(child => child.CargoUnitId));
        Assert.All(children, child =>
        {
            Assert.True(child.IsTankerCargo);
            Assert.Equal(TankerCargoMovementProgram.EightAdvanceTurn,
                child.CargoMovementProgram);
            Assert.Equal(Level3CargoState.Active,
                ledger.GetCargoState(child.CargoUnitId));
        });
        ledger.ValidateState();
        InvokePrivate(game, "ValidateLevel3WaveRepresentation");

        if (releaseKind != TankerReleaseKind.Natural)
            return;

        InvokePrivate(game, "KillPlayer");
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Empty(game.Flippers);
        Assert.All(cargoIds, cargoId =>
            Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(cargoId)));
        Assert.Equal(23, ledger.OriginalCount);
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(21));
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(22));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(21));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(22));
        InvokePrivate(game, "ValidateLevel3WaveRepresentation");
    }

    [Fact]
    public void OrdinaryAndCargoFlippersRepeatEightAdvancesThenHoldDepthDuringTurn()
    {
        Level level = new(LevelConfiguration.Level18Production);
        Flipper ordinary = new(edge: 7, height: 100);
        Flipper cargo = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(7, 6, 1, 100), cargoUnitId: 101,
            tankerCargoMovementProgram: TankerCargoMovementProgram.EightAdvanceTurn);

        AssertEightAdvanceCycle(ordinary, level);
        AssertEightAdvanceCycle(cargo, level);

        Flipper reconstructed = Flipper.CreateFreshTankerCargo(
            edge: 7, cargoUnitId: 101, initialDirection: 1,
            tankerCargoMovementProgram: TankerCargoMovementProgram.EightAdvanceTurn);
        AssertEightAdvanceCycle(reconstructed, level);

        static void AssertEightAdvanceCycle(Flipper flipper, Level routeLevel)
        {
            for (int cycle = 0; cycle < 2; cycle++)
            {
                float startHeight = flipper.Height;
                for (int advance = 1; advance <= 8; advance++)
                {
                    flipper.Update(routeLevel, playerEdge: 0, (_, _) => 0);
                    Assert.Equal(startHeight + advance *
                        routeLevel.Configuration.Flipper.ClimbSpeed, flipper.Height);
                }

                float heightAtTurn = flipper.Height;
                int startingLane = flipper.EdgeIndex;
                int turnTicks = 0;
                while (flipper.EdgeIndex == startingLane && turnTicks < 32)
                {
                    flipper.Update(routeLevel, playerEdge: 0, (_, _) => 0);
                    Assert.Equal(heightAtTurn, flipper.Height);
                    turnTicks++;
                }

                Assert.InRange(turnTicks, 1, 31);
                Assert.NotEqual(startingLane, flipper.EdgeIndex);
                Assert.Equal(heightAtTurn, flipper.Height);
            }
        }
    }

    [Fact]
    public void EightAdvanceRouteHandsOffAtRimToSharedPursuit()
    {
        Level level = new(LevelConfiguration.Level18Production);
        Flipper flipper = new(edge: 4, height: Level.Depth - 5);

        flipper.Update(level, playerEdge: 7, (_, _) => 0);
        flipper.Update(level, playerEdge: 7, (_, _) => 0);

        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Null(flipper.RimPursuitDirection);

        for (int tick = 0; tick < level.Configuration.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge: 7, (_, _) => 0);

        Assert.NotNull(flipper.RimPursuitDirection);
        Assert.Equal(FlipperState.Flipping, flipper.State);
    }

    [Fact]
    public void PulsarThresholdMapsEndpointsAndSelectsSpeedsByPreMoveHeight()
    {
        LevelConfiguration level18 = LevelConfiguration.Level18Production;
        PulsarMovementConfiguration movement = level18.PulsarMovement;
        float threshold = Level18WaveSelection.MapPulsarThresholdToHostHeight(
            Level18WaveSelection.PulsarFlipRateRaw, level18.Depth);

        Assert.Equal(392.92035f, threshold);
        Assert.Equal(threshold, movement.ThresholdHeight);
        Assert.Equal(1f, movement.FixedSpeed);
        Assert.Equal(21f / 11f, movement.DeepSideTowardRimSpeed);
        Assert.Equal(Level.Depth,
            Level18WaveSelection.MapPulsarThresholdToHostHeight(0x10, Level.Depth));
        Assert.Equal(0f,
            Level18WaveSelection.MapPulsarThresholdToHostHeight(0xF2, Level.Depth));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Level18WaveSelection.MapPulsarThresholdToHostHeight(0x0F, Level.Depth));

        Pulsar deepEquality = new(4, threshold, movementConfiguration: movement);
        deepEquality.Update(new Level(level18), 4, (_, _) => 0);
        Assert.Equal(threshold + 21f / 11f, deepEquality.Height);

        Pulsar rimward = new(4, threshold + .1f, movementConfiguration: movement);
        rimward.Update(new Level(level18), 4, (_, _) => 0);
        Assert.Equal(threshold + 1.1f, rimward.Height);

        Pulsar crossing = new(4, threshold - .5f, movementConfiguration: movement);
        crossing.Update(new Level(level18), 4, (_, _) => 0);
        float crossedHeight = threshold - .5f + 21f / 11f;
        Assert.Equal(crossedHeight, crossing.Height);
        crossing.Update(new Level(level18), 4, (_, _) => 0);
        Assert.Equal(crossedHeight + 1f, crossing.Height);
    }

    [Fact]
    public void PulsarDescendingUsesFixedSpeedAndReturnsAtThresholdOrWithoutPendingSupply()
    {
        PulsarMovementConfiguration movement =
            LevelConfiguration.Level18Production.PulsarMovement;
        float threshold = movement.ThresholdHeight!.Value;

        Pulsar atRim = CreateDescendingPulsar(movement);
        atRim.Update(new Level(LevelConfiguration.Level18Production), 4,
            (_, _) => 0,
            pendingGenericOriginalCount: 3);
        Assert.Equal(Level.Depth - 1, atRim.Height);
        Assert.Equal(PulsarState.Descending, atRim.State);

        while (atRim.Height > threshold)
            atRim.Update(new Level(LevelConfiguration.Level18Production), 4,
                (_, _) => 0,
                pendingGenericOriginalCount: 3);
        Assert.True(atRim.Height <= threshold);
        Assert.Equal(PulsarState.Climbing, atRim.State);

        Pulsar noSupply = CreateDescendingPulsar(movement);
        noSupply.Update(new Level(LevelConfiguration.Level18Production), 4,
            (_, _) => 0,
            pendingGenericOriginalCount: 0);
        Assert.Equal(Level.Depth - 1, noSupply.Height);
        Assert.Equal(PulsarState.Climbing, noSupply.State);

        static Pulsar CreateDescendingPulsar(PulsarMovementConfiguration movementPolicy)
        {
            Level level = new(LevelConfiguration.Level18Production);
            Pulsar pulsar = new(4, Level.Depth, movementConfiguration: movementPolicy);
            pulsar.Update(level, 4, (_, _) => 0);
            Assert.Equal(PulsarState.RimPause, pulsar.State);
            for (int tick = 0; tick < Pulsar.RimPauseDuration; tick++)
                pulsar.Update(level, 4, (_, _) => 0, pendingGenericOriginalCount: 5);
            Assert.Equal(PulsarState.Descending, pulsar.State);
            return pulsar;
        }
    }

    [Fact]
    public void PulsarPausesHoldDepthAndFlipUsesTheCurrentThresholdRegionSpeed()
    {
        PulsarMovementConfiguration movement =
            LevelConfiguration.Level18Production.PulsarMovement;
        float threshold = movement.ThresholdHeight!.Value;
        Level level = new(LevelConfiguration.Level18Production);

        Pulsar rimPause = new(4, Level.Depth, movementConfiguration: movement);
        rimPause.Update(level, 4, (_, _) => 0);
        Assert.Equal(PulsarState.RimPause, rimPause.State);
        for (int tick = 0; tick < Pulsar.RimPauseDuration; tick++)
        {
            rimPause.Update(level, 4, (_, _) => 0);
            Assert.Equal(Level.Depth, rimPause.Height);
        }
        Assert.Equal(PulsarState.Descending, rimPause.State);

        Pulsar lanePause = new(4, Level.Depth, movementConfiguration: movement);
        lanePause.Update(level, 4, (_, _) => 1);
        Assert.Equal(PulsarState.LanePause, lanePause.State);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
        {
            lanePause.Update(level, 4, (_, _) => 1);
            Assert.Equal(Level.Depth, lanePause.Height);
        }
        Assert.Equal(PulsarState.Flipping, lanePause.State);
        lanePause.Update(level, 4, (_, _) => 1);
        Assert.Equal(Level.Depth, lanePause.Height);

        Pulsar flipping = new(4, threshold - 100, movementConfiguration: movement);
        for (int tick = 0; tick < 51; tick++)
            flipping.Update(level, 4, (_, _) => 1);
        Assert.Equal(PulsarState.LanePause, flipping.State);
        float turnHeight = flipping.Height;
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
        {
            flipping.Update(level, 4, (_, _) => 1);
            Assert.Equal(turnHeight, flipping.Height);
        }
        Assert.Equal(PulsarState.Flipping, flipping.State);
        Assert.True(flipping.Height <= threshold);
        flipping.Update(level, 4, (_, _) => 1);
        Assert.Equal(turnHeight + 21f / 11f, flipping.Height);
    }

    [Fact]
    public void PulsarReconstructionUsesTheCurrentLevelPolicyAndKeepsIdentityAccounting()
    {
        TempestGame game = new(startingProductionLevel: 18, random: new MinimumRandom());
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        List<Egg> eggs = GetPrivateList<Egg>(level, "eggs");
        Egg originalEgg = eggs.Single(egg => egg.OriginalSlotId == 0);

        Pulsar initial = Assert.IsType<Pulsar>(
            InvokePrivate(game, "CreateLevel3MixedEnemy", originalEgg));
        Assert.Equal(LevelConfiguration.Level18Production.PulsarMovement,
            initial.MovementConfiguration);
        eggs.Remove(originalEgg);
        InvokePrivate(game, "AddHatchedEnemy", initial);
        InvokePrivate(game, "ValidateLevel3WaveRepresentation");

        InvokePrivate(game, "KillPlayer");
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Empty(game.Pulsars);
        Egg reconstructedEgg = game.Eggs.Single(egg => egg.OriginalSlotId == 0);
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(0));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Pulsar reconstructed = Assert.IsType<Pulsar>(
            InvokePrivate(game, "CreateLevel3MixedEnemy", reconstructedEgg));
        Assert.Equal(initial.OriginalSlotId, reconstructed.OriginalSlotId);
        Assert.Equal(LevelConfiguration.Level18Production.PulsarMovement,
            reconstructed.MovementConfiguration);

        eggs.Remove(reconstructedEgg);
        InvokePrivate(game, "AddHatchedEnemy", reconstructed);
        InvokePrivate(game, "ValidateLevel3WaveRepresentation");
        Assert.Equal(Level3OriginalState.ActivePulsar, ledger.GetOriginalState(0));
    }

    [Fact]
    public void PulsarPulseThresholdOverlapsAndYieldsToExistingRimCollisionPriority()
    {
        PulsarMovementConfiguration movement =
            LevelConfiguration.Level18Production.PulsarMovement;
        float threshold = movement.ThresholdHeight!.Value;
        Player player = new();
        player.Move(4 - player.EdgeIndex, 16);

        Pulsar pulseRegion = new(4, threshold + .1f, movementConfiguration: movement);
        Assert.Equal(PulsarCollision.Rim,
            pulseRegion.CheckPlayerCollision(player, pulsing: true));

        Pulsar outsidePulseRegion = new(4, threshold - 1, movementConfiguration: movement);
        Assert.Equal(PulsarCollision.None,
            outsidePulseRegion.CheckPlayerCollision(player, pulsing: true));

        Pulsar level17 = new(4, Level.Depth - 8,
            movementConfiguration: LevelConfiguration.Level17Production.PulsarMovement);
        Assert.Equal(PulsarCollision.PulsingLane,
            level17.CheckPlayerCollision(player, pulsing: true));
    }

    [Fact]
    public void LevelEighteenCompletionRemainsBlockedUntilItsPulsarIdentityResolves()
    {
        TempestGame game = new(startingProductionLevel: 18);
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        GetPrivateList<Egg>(level, "eggs").Clear();

        for (int originalId = 0; originalId < ledger.OriginalCount; originalId++)
        {
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(originalId);

            if (originalId == 0)
            {
                ledger.CommitOriginalAsPulsar(originalId);
                Pulsar pulsar = new(0, 0, originalId, level.Configuration.PulsarMovement);
                GetPrivateList<Pulsar>(game, "pulsars").Add(pulsar);
                GetPrivateList<object>(game, "developmentCollisionOrder").Add(pulsar);
                continue;
            }

            ledger.CommitOriginalAsFlipper(originalId);
            ledger.MarkOriginalDestroyed(originalId);
        }

        Assert.False((bool)InvokePrivate(game, "CheckForLevelComplete")!);
        Assert.Equal(LevelState.Playing, game.State);
        ledger.MarkOriginalDestroyed(0);
        GetPrivateList<Pulsar>(game, "pulsars").Clear();
        GetPrivateList<object>(game, "developmentCollisionOrder").Clear();

        Assert.True((bool)InvokePrivate(game, "CheckForLevelComplete")!);
        Assert.Equal(LevelState.ExitDelay, game.State);
        ledger.ValidateState();
    }

    [Fact]
    public void Level17PulsarPolicyKeepsOneUnitMovementAndNoThreshold()
    {
        PulsarMovementConfiguration movement =
            LevelConfiguration.Level17Production.PulsarMovement;
        Assert.Null(movement.ThresholdHeight);
        Assert.Equal(1f, movement.FixedSpeed);
        Assert.Equal(1f, movement.DeepSideTowardRimSpeed);

        Level level17 = new(LevelConfiguration.Level17Production);
        Pulsar pulsar = new(3, 10, movementConfiguration: movement);
        pulsar.Update(level17, 8, (_, _) => throw new InvalidOperationException());
        Assert.Equal(11, pulsar.Height);
        Assert.Equal(PulsarState.Climbing, pulsar.State);
        Assert.Equal(1, pulsar.TimeToFlip);

        Pulsar cadence = new(3, 10, movementConfiguration: movement);
        Level17Cadence(level17, cadence, expectedDecisionTicks: 51);

        static void Level17Cadence(Level level, Pulsar actor, int expectedDecisionTicks)
        {
            for (int tick = 0; tick < expectedDecisionTicks - 1; tick++)
                actor.Update(level, 8, (_, _) => 1);
            Assert.Equal(PulsarState.Climbing, actor.State);
            Assert.Equal(50, actor.TimeToFlip);
            Assert.Equal(60, actor.Height);

            actor.Update(level, 8, (_, _) => 1);
            Assert.Equal(PulsarState.LanePause, actor.State);
            Assert.Equal(Pulsar.LanePauseDuration, actor.PauseTicks);
            Assert.Equal(0, actor.TimeToFlip);
            float pauseHeight = actor.Height;
            int pauseEdge = actor.EdgeIndex;
            for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            {
                actor.Update(level, 8, (_, _) => 1);
                Assert.Equal(pauseHeight, actor.Height);
            }
            Assert.Equal(PulsarState.Flipping, actor.State);
            Assert.NotEqual(pauseEdge, actor.EdgeIndex);

            int flipTicks = 0;
            while (actor.State == PulsarState.Flipping && flipTicks < 32)
            {
                float heightBeforeFlip = actor.Height;
                actor.Update(level, 8, (_, _) => 1);
                Assert.Equal(heightBeforeFlip + 1, actor.Height);
                flipTicks++;
            }
            Assert.InRange(flipTicks, 1, 31);
            Assert.Equal(PulsarState.RimPause, actor.State);

            float rimPauseHeight = actor.Height;
            for (int tick = 0; tick < Pulsar.RimPauseDuration; tick++)
            {
                actor.Update(level, 8, (_, _) => 1);
                Assert.Equal(rimPauseHeight, actor.Height);
            }
            Assert.Equal(PulsarState.Climbing, actor.State);
        }
    }

    [Fact]
    public void Level18PresentationRoutesThroughShapeTwoWithoutChangingLevelTwoOutput()
    {
        LevelConfiguration level2 = LevelConfiguration.Level2Square;
        LevelConfiguration level18 = LevelConfiguration.Level18Production;
        RomPresentationMetadata metadata2 = RomPresentationMetadataCatalog.ForDisplayedLevel(2);
        RomPresentationMetadata metadata18 = RomPresentationMetadataCatalog.ForDisplayedLevel(18);

        Assert.Equal((1, 0x1C, 0x50, (short)-224, (byte)0x00),
            (metadata18.TubeTableIndex, metadata18.ScaleRaw,
             metadata18.CameraHeightRaw, metadata18.ScreenYOffsetSigned,
             metadata18.OpenRaw));
        Assert.Equal(AtariPresentationCalibration.ForDisplayedLevel(2),
            AtariPresentationCalibration.ForDisplayedLevel(18));
        Assert.Equal((1f, 1.1f, 35f, 1f),
            (AtariPresentationCalibration.ForDisplayedLevel(18).TubeYGain,
             AtariPresentationCalibration.ForDisplayedLevel(18).TubeScale,
             AtariPresentationCalibration.ForDisplayedLevel(18).VerticalOffset,
             AtariPresentationCalibration.ForDisplayedLevel(18).TubeDepthYGain));
        Assert.Equal((1.436688312f, 1),
            AtariPresentationAdapter.GetModelFrameMapping(18));
        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(18));

        Vector2 point = level2.Definition.Points[0];
        const float depth = 250;
        AtariPresentationProjection projection2 = Project(level2, metadata2, 2, point, depth);
        AtariPresentationProjection projection18 = Project(level18, metadata18, 18, point, depth);
        Assert.Equal(projection2.Position, projection18.Position);
        Assert.Equal(projection2.LocalScale, projection18.LocalScale);
        Assert.Equal(projection2.LocalScaleY, projection18.LocalScaleY);

        Assert.Equal(new byte[] { 0x00, 0x07, 0x0B, 0x08, 0x44, 0x03, 0x0C, 0x0C },
            RomPaletteCatalog.RawBlockForPaletteIndex(level18.PaletteIndex));
        Assert.Equal(Raylib_cs.Color.Red, CyclePaletteResolver.ResolveTubeColor(18));
        Assert.Equal(Raylib_cs.Color.Magenta, CyclePaletteResolver.ResolveFlipperColor(18));
        Assert.Equal(Raylib_cs.Color.Blue, CyclePaletteResolver.ResolveTankerColor(18));
        Assert.Equal(Raylib_cs.Color.Yellow, CyclePaletteResolver.ResolveEggColor(18));
        Assert.Equal(new Raylib_cs.Color(0, 255, 255, 255),
            CyclePaletteResolver.ResolvePulsarColor(18));
        Assert.Equal(new Raylib_cs.Color(0, 255, 255, 255),
            AtariHudVector.ResolveScoreColor(18));
        Assert.Equal(Raylib_cs.Color.Green, AtariHudVector.ResolveReserveLifeColor(18));
        Assert.Equal(Raylib_cs.Color.Red, AtariHudVector.ResolveLevelColor(18));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomPresentationMetadataCatalog.ForDisplayedLevel(19));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => AtariPresentationCalibration.ForDisplayedLevel(19));

        static AtariPresentationProjection Project(
            LevelConfiguration configuration,
            RomPresentationMetadata metadata,
            int displayedLevel,
            Vector2 point,
            float depth) =>
            AtariPresentationAdapter.Project(
                displayedLevel, point.X, point.Y, depth,
                configuration.Definition.CenterY, configuration.Definition.YOffset,
                configuration.Position, configuration.Depth,
                4, 9, 70, 5, metadata,
                tubeYGain: 1, tubeDepthYGain: 1);
    }

    [Theory]
    [InlineData("--start-level")]
    [InlineData("--level")]
    public void BothDirectStartupFormsResolveLevelEighteen(string option)
    {
        Assert.True(Debug.DebugController.TryParseStartingLevel(
            [option, "18"], out int? displayedLevel, out string? error));
        Assert.Equal(18, displayedLevel);
        Assert.Null(error);
        Assert.False(Debug.DebugController.TryParseStartingLevel(
            [option, "19"], out _, out _));

        TempestGame game = new(startingProductionLevel: displayedLevel, startingLives: 9);
        Assert.Same(LevelConfiguration.Level18Production, game.CurrentLevel.Configuration);
        Assert.Equal(18, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(9, game.Player.Lives);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(17, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Spikes);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
    }

    private static List<T> GetPrivateList<T>(object instance, string fieldName)
    {
        FieldInfo field = instance.GetType().GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Private list '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(instance)
            ?? throw new InvalidOperationException($"Private list '{fieldName}' was not initialized."));
    }

    private static object? InvokePrivate(
        object instance,
        string methodName,
        params object?[] arguments)
    {
        MethodInfo method = instance.GetType().GetMethod(
            methodName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Private method '{methodName}' was not found.");
        return method.Invoke(instance, arguments);
    }

    private sealed class MaximumRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            maxValue <= minValue ? minValue : maxValue - 1;
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
