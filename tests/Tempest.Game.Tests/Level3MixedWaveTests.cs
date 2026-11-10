using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level3MixedWaveTests
{
    [Fact]
    public void ProductionConfigurationSharesThePlusBoardAndDefinesTheLevelThreeWave()
    {
        LevelConfiguration configuration = LevelConfiguration.Level3Mixed;
        TempestGame game = new(configuration,
            random: new InclusiveRangeScript(
                Enumerable.Range(0, 4)
                    .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) })));

        Assert.Same(LevelConfiguration.Level3Plus.Definition, configuration.Definition);
        Assert.Equal(2, configuration.ProgressionIndex);
        Assert.Equal(3, configuration.DisplayedLevel);
        Assert.Equal(2, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(400, configuration.Definition.CenterY);
        Assert.Equal(-80, configuration.Definition.YOffset);
        Assert.Equal(400, configuration.Depth);
        Assert.Equal(50, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(15, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(11, game.CurrentLevel.ReserveEggs);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Empty(game.Spikes);
        Assert.Equal(new FlipperConfiguration(true, 2, 8, 21, 24),
            configuration.Flipper);
        Assert.Equal(LevelEnemyComposition.FlippersAndTankers,
            configuration.NormalEnemyComposition);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(15, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(0, game.CurrentLevel.Level3Ledger.CargoCount);
        Assert.Equal(15, game.CurrentLevel.Level3Ledger.TotalIdentityCount);

        Assert.Equal(LevelEnemyComposition.FlippersOnly,
            LevelConfiguration.Level3Plus.NormalEnemyComposition);
    }

    [Fact]
    public void ProductionEggsUseRandomLanesAndReleaseSixWithoutTheDiagnosticCap()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 10)
                .SelectMany(_ => new[] { (0, 15, 15), (0, 1, 1) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);

        Assert.All(level.Eggs, egg =>
        {
            Assert.Equal(15, egg.EdgeIndex);
            Assert.Equal(1, egg.MovementDirection);
        });
        Assert.Equal(11, level.ReserveEggs);

        for (int tick = 0; tick < 49; tick++)
            level.UpdateEggRelease();

        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(11, level.ReserveEggs);
        level.UpdateEggRelease();

        Assert.Equal(10, level.Eggs.Count);
        Assert.Equal(5, level.ReserveEggs);
        Assert.Equal(4, level.Configuration.MaximumActiveEggs);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ProductionApproachGateUsesStrictActiveAndRollBoundaries()
    {
        InclusiveRangeScript initial = new(
            Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: initial.NextInclusive);
        int randomCalls = 0;

        level.UpdateLevel3MixedEggs(9, (_, _) =>
        {
            randomCalls++;
            throw new InvalidOperationException("Nine active enemies must close the approach gate.");
        });
        Assert.Equal(0, randomCalls);

        level.UpdateLevel3MixedEggs(8, (_, _) =>
        {
            randomCalls++;
            return 95;
        });
        Assert.Equal(0, level.Eggs.Count(egg => egg.State != EggState.Roaming));

        level.UpdateLevel3MixedEggs(8, (_, _) =>
        {
            randomCalls++;
            return 96;
        });
        level.UpdateLevel3MixedEggs(8, (_, _) =>
        {
            randomCalls++;
            return 96;
        });
        Assert.Equal(2, level.Eggs.Count(egg => egg.State == EggState.Approaching));

        level.UpdateLevel3MixedEggs(8, (_, _) =>
        {
            randomCalls++;
            throw new InvalidOperationException("Two approaching Eggs must close the approach gate.");
        });

        Assert.Equal(2, level.Eggs.Count(egg => egg.State == EggState.Approaching));
        Assert.Equal(3, randomCalls);
    }

    [Fact]
    public void MixedSelectionPreservesRejectedDrawsNegativeClampAndNearEndOverwrite()
    {
        InclusiveRangeScript ordinary = new(
        [
            (-6, 4, -6),
            (-6, 4, 0),
            (-6, 4, 2),
            (-6, 4, 3),
            (-6, 4, 1)
        ]);
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectOriginalType(
                ordinary.NextInclusive, activeTankers: 0,
                activeEnemyUnits: 4, activeEggs: 0));
        Assert.Equal(0, ordinary.Remaining);

        InclusiveRangeScript nearEnd = new(
        [
            (-6, 4, -6),
            (1, 4, 4)
        ]);
        Assert.Equal(Level3OriginalType.Tanker,
            Level3WaveSelection.SelectOriginalType(
                nearEnd.NextInclusive, activeTankers: 5,
                activeEnemyUnits: 1, activeEggs: 2));
        Assert.Equal(0, nearEnd.Remaining);
    }

    [Fact]
    public void SixTankerGateAndCargoSelectionUseExactInclusiveRanges()
    {
        InclusiveRangeScript atCap = new([(-6, 3, 1)]);
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectOriginalType(
                atCap.NextInclusive, activeTankers: 6,
                activeEnemyUnits: 6, activeEggs: 1));
        Assert.Equal(0, atCap.Remaining);

        InclusiveRangeScript cargo = new(
        [
            (0, 2, 1),
            (0, 2, 2),
            (0, 2, 0)
        ]);
        Level3WaveSelection.SelectTwoFlipperCargo(cargo.NextInclusive);
        Assert.Equal(0, cargo.Remaining);
    }

    [Fact]
    public void TankerSelectionBecomesAvailableAgainWhenCapacityFrees()
    {
        InclusiveRangeScript random = new([(-6, 4, 4)]);

        Assert.Equal(Level3OriginalType.Tanker,
            Level3WaveSelection.SelectOriginalType(
                random.NextInclusive, activeTankers: 5,
                activeEnemyUnits: 10, activeEggs: 1));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void CanonicalProductionAdmissionNeverCreatesASecondTanker()
    {
        BoundedCompositionRandom random = new(maximumCalls: 2000);
        TempestGame game = new(LevelConfiguration.Level3Mixed, random: random);

        for (int tick = 0; tick < 200; tick++)
        {
            game.RunOneStep();
            Assert.InRange(game.Tankers.Count, 0, 1);
            Assert.Empty(game.Pulsars);
        }

        Assert.Contains(game.Flippers, flipper => !flipper.IsTankerCargo);
        Assert.True(random.Consumed > 0);
    }

    [Fact]
    public void FirstSuperZapperResolvesSixMixedWaveCarriersAndTheirOwnedPairsExactlyOnce()
    {
        TempestGame game = CreateArtificialMultiCarrierGame();

        Tanker[] carriers = game.Tankers.ToArray();
        Assert.Equal(6, carriers.Length);
        Assert.Contains(game.Flippers, flipper => !flipper.IsTankerCargo);
        int originalFlippers = game.Flippers.Count(flipper => !flipper.IsTankerCargo);
        int acceptedTargets = carriers.Length + originalFlippers;
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int[] carrierIds = carriers.Select(carrier => carrier.OriginalSlotId).ToArray();
        Assert.Equal(carrierIds.Length, carrierIds.Distinct().Count());

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(
            Enumerable.Range(0, acceptedTargets).Select(index => 3 + 2 * index),
            game.Tankers.Select(carrier => carrier.PendingZapTicks)
                .Concat(game.Flippers.Where(flipper => !flipper.IsTankerCargo)
                    .Select(flipper => flipper.PendingZapTicks))
                .Order());

        for (int tick = 0; tick < 50; tick++)
            game.RunOneStep();

        Assert.Equal(500 * carriers.Length + 150 * originalFlippers, game.Score.Score);
        foreach (Tanker carrier in carriers)
        {
            Assert.Equal(Level3OriginalState.Destroyed,
                ledger.GetOriginalState(carrier.OriginalSlotId));
            Assert.Equal(2, ledger.GetCargoIdsForOriginal(carrier.OriginalSlotId).Count);
            Assert.All(ledger.GetCargoIdsForOriginal(carrier.OriginalSlotId),
                cargoId => Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoId)));
        }
        Assert.True(ledger.CargoCount > 12);
        Assert.Equal(0, ledger.CargoCount % Tanker.CarriedFlipperCount);
        Assert.InRange(game.Tankers.Count, 0, 6);
    }

    [Fact]
    public void NaturalShotAndZapCarrierOutcomesOverlapWithoutSharingCargoOrScore()
    {
        TempestGame game = CreateArtificialMultiCarrierGame(includeEnemyBullets: true);
        Tanker[] carriers = game.Tankers
            .GroupBy(tanker => tanker.EdgeIndex)
            .Select(group => group.First())
            .Take(3)
            .ToArray();
        Tanker shotCarrier = FindTankerSafeForShot(game);
        Assert.Contains(shotCarrier, carriers);
        Tanker naturalCarrier = carriers.First(tanker =>
            !ReferenceEquals(tanker, shotCarrier));
        Tanker zapCarrier = carriers.First(tanker =>
            !ReferenceEquals(tanker, shotCarrier) &&
            !ReferenceEquals(tanker, naturalCarrier));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Tanker[] initialTankers = game.Tankers.ToArray();
        HashSet<int> unresolvedOriginalsBefore = ledger.UnresolvedOriginalIds.ToHashSet();
        HashSet<int> unresolvedCargoBefore = ledger.UnresolvedCargoIds.ToHashSet();
        HashSet<int> destroyedOriginalsBefore = Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Destroyed)
            .ToHashSet();
        HashSet<int> escapedOriginalsBefore = Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Escaped)
            .ToHashSet();
        HashSet<int> rimClearedOriginalsBefore = Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.RimCleared)
            .ToHashSet();
        HashSet<int> destroyedCargoBefore = Enumerable.Range(0, ledger.CargoCount)
            .Where(id => ledger.GetCargoState(id) == Level3CargoState.Destroyed)
            .ToHashSet();
        HashSet<int> rimClearedCargoBefore = Enumerable.Range(0, ledger.CargoCount)
            .Where(id => ledger.GetCargoState(id) == Level3CargoState.RimCleared)
            .ToHashSet();
        EnemyBullet[] existingBullets = game.EnemyBullets.ToArray();
        EnemyBullet independentBullet = existingBullets.FirstOrDefault(
            bullet => bullet.Height <= 394)
            ?? throw new InvalidOperationException("The overlap fixture must contain a live Tanker bullet.");
        float bulletHeightBefore = independentBullet.Height;

        Assert.Equal(LevelState.Playing, game.State);
        Assert.NotEmpty(existingBullets);
        game.Player.Move(shotCarrier.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.True(game.Shots.Count == 1,
            $"Shot was consumed: score={game.Score.Score}, target={shotCarrier.OriginalSlotId}, " +
            $"targetHeight={shotCarrier.Height}, state={game.State}.");
        Shot shot = Assert.Single(game.Shots);
        shot.Height = shotCarrier.Height + 5;
        Assert.True(shotCarrier.TryZap(1));

        while (naturalCarrier.State == TankerState.Climbing)
            naturalCarrier.UpdateForDevelopment();
        Assert.True(zapCarrier.TryZap(1));

        MovePlayerToSafeLane(game);
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(400, game.Score.Score);
        Assert.DoesNotContain(shotCarrier, game.Tankers);
        Assert.DoesNotContain(naturalCarrier, game.Tankers);
        Assert.DoesNotContain(zapCarrier, game.Tankers);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(shotCarrier.OriginalSlotId));
        Assert.Equal(Level3OriginalState.Escaped,
            ledger.GetOriginalState(naturalCarrier.OriginalSlotId));
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(zapCarrier.OriginalSlotId));
        int[] targetOriginalIds =
        [
            shotCarrier.OriginalSlotId,
            naturalCarrier.OriginalSlotId,
            zapCarrier.OriginalSlotId
        ];
        HashSet<int> otherEscapedCarriers = initialTankers
            .Where(tanker => !targetOriginalIds.Contains(tanker.OriginalSlotId) &&
                !game.Tankers.Contains(tanker))
            .Select(tanker => tanker.OriginalSlotId)
            .ToHashSet();
        Assert.All(otherEscapedCarriers, id =>
            Assert.Equal(Level3OriginalState.Escaped, ledger.GetOriginalState(id)));
        Assert.Equal(destroyedOriginalsBefore
                .Concat([shotCarrier.OriginalSlotId, zapCarrier.OriginalSlotId]).ToHashSet(),
            Enumerable.Range(0, ledger.OriginalCount)
                .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Destroyed)
                .ToHashSet());
        Assert.Equal(escapedOriginalsBefore
                .Concat([naturalCarrier.OriginalSlotId])
                .Concat(otherEscapedCarriers)
                .ToHashSet(),
            Enumerable.Range(0, ledger.OriginalCount)
                .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Escaped)
                .ToHashSet());
        Assert.Equal(rimClearedOriginalsBefore,
            Enumerable.Range(0, ledger.OriginalCount)
                .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.RimCleared)
                .ToHashSet());
        Assert.Equal(unresolvedOriginalsBefore
                .Except(targetOriginalIds)
                .Except(otherEscapedCarriers)
                .ToHashSet(),
            ledger.UnresolvedOriginalIds);

        IReadOnlyList<int> shotCargo = ledger.GetCargoIdsForOriginal(shotCarrier.OriginalSlotId);
        IReadOnlyList<int> naturalCargo = ledger.GetCargoIdsForOriginal(naturalCarrier.OriginalSlotId);
        IReadOnlyList<int> zapCargo = ledger.GetCargoIdsForOriginal(zapCarrier.OriginalSlotId);
        int[] allReleasedCargo = shotCargo.Concat(naturalCargo).Concat(zapCargo).ToArray();
        Assert.Equal(6, allReleasedCargo.Distinct().Count());
        Assert.All(shotCargo, id => ledger.ValidateCargoOwner(id, shotCarrier.OriginalSlotId));
        Assert.All(naturalCargo, id => ledger.ValidateCargoOwner(id, naturalCarrier.OriginalSlotId));
        Assert.All(zapCargo, id => ledger.ValidateCargoOwner(id, zapCarrier.OriginalSlotId));
        Assert.All(new[] { shotCargo, naturalCargo, zapCargo }, cargoIds =>
        {
            Assert.Equal(2, cargoIds.Count);
            Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        });
        Assert.All(allReleasedCargo,
            id => Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(id)));
        Assert.All(game.Flippers.Where(flipper =>
                shotCargo.Concat(naturalCargo).Contains(flipper.CargoUnitId)),
            flipper => Assert.Equal(0, flipper.PendingZapTicks));
        Assert.All(game.Flippers.Where(flipper => zapCargo.Contains(flipper.CargoUnitId)),
            flipper => Assert.Equal(8, flipper.PendingZapTicks));
        Assert.Equal(unresolvedCargoBefore, ledger.UnresolvedCargoIds);
        Assert.Equal(destroyedCargoBefore,
            Enumerable.Range(0, ledger.CargoCount)
                .Where(id => ledger.GetCargoState(id) == Level3CargoState.Destroyed)
                .ToHashSet());
        Assert.Equal(rimClearedCargoBefore,
            Enumerable.Range(0, ledger.CargoCount)
                .Where(id => ledger.GetCargoState(id) == Level3CargoState.RimCleared)
                .ToHashSet());
        Assert.False(ledger.HasPendingShotActivation);
        Assert.Equal(bulletHeightBefore + 6, independentBullet.Height);
        Assert.Contains(independentBullet, game.EnemyBullets);
        Assert.Empty(game.Shots);

        Flipper[] shotChildren = game.Flippers
            .Where(flipper => shotCargo.Contains(flipper.CargoUnitId)).ToArray();
        Flipper[] naturalChildren = game.Flippers
            .Where(flipper => naturalCargo.Contains(flipper.CargoUnitId)).ToArray();
        Flipper[] zapChildren = game.Flippers
            .Where(flipper => zapCargo.Contains(flipper.CargoUnitId)).ToArray();
        Assert.Equal(2, shotChildren.Length);
        Assert.Equal(2, naturalChildren.Length);
        Assert.Equal(2, zapChildren.Length);
        Assert.All(shotChildren,
            child => Assert.Equal(shotCarrier.Height, child.Height));
        Assert.All(naturalChildren,
            child => Assert.Equal(naturalCarrier.Height, child.Height));
        Assert.All(zapChildren, child =>
        {
            Assert.Equal(zapCarrier.Height, child.Height);
            Assert.Equal(8, child.PendingZapTicks);
        });
        Assert.Equal(1, shotCarrier.PendingZapTicks);

        MovePlayerToSafeLane(game);
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(400, game.Score.Score);
        Assert.Equal(1, shotCarrier.PendingZapTicks);
        Assert.All(game.Flippers.Where(flipper => zapCargo.Contains(flipper.CargoUnitId)),
            flipper => Assert.Equal(7, flipper.PendingZapTicks));
        Assert.DoesNotContain(shotCarrier, game.Tankers);

        for (int tick = 0; tick < 7; tick++)
        {
            MovePlayerToSafeLane(game);
            game.RunOneStep();
        }

        Assert.Equal(700, game.Score.Score);
        Assert.Equal(1, shotCarrier.PendingZapTicks);
        Assert.DoesNotContain(shotCarrier, game.Tankers);
        Assert.All(zapCargo,
            id => Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(id)));
        Assert.All(shotCargo.Concat(naturalCargo),
            id => Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(id)));
        Assert.Equal(unresolvedCargoBefore.Except(zapCargo).ToHashSet(),
            ledger.UnresolvedCargoIds);
        Assert.Equal(destroyedCargoBefore.Concat(zapCargo).ToHashSet(),
            Enumerable.Range(0, ledger.CargoCount)
                .Where(id => ledger.GetCargoState(id) == Level3CargoState.Destroyed)
                .ToHashSet());
        Assert.Equal(rimClearedCargoBefore,
            Enumerable.Range(0, ledger.CargoCount)
                .Where(id => ledger.GetCargoState(id) == Level3CargoState.RimCleared)
                .ToHashSet());
        Assert.DoesNotContain(game.Flippers,
            flipper => zapCargo.Contains(flipper.CargoUnitId));
    }

    [Fact]
    public void SecondSuperZapperReplacesTheSelectedTargetTimerAndIgnoresBulletsForDelaySlots()
    {
        TempestGame game = CreateArtificialMultiCarrierGame(includeEnemyBullets: true);
        Tanker[] carriers = game.Tankers.ToArray();
        Flipper[] flippers = game.Flippers.ToArray();
        Enemy[] acceptedTargets = carriers.Cast<Enemy>().Concat(flippers).ToArray();
        EnemyBullet[] bullets = game.EnemyBullets.ToArray();
        Assert.NotEmpty(bullets);

        MovePlayerToSafeLane(game);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(
            Enumerable.Range(0, acceptedTargets.Length).Select(index => 3 + 2 * index),
            carriers.Select(carrier => carrier.PendingZapTicks)
                .Concat(flippers.Select(flipper => flipper.PendingZapTicks))
                .Order());
        Assert.All(bullets, bullet => Assert.Equal(0, bullet.PendingZapTicks));

        MovePlayerToSafeLane(game);
        acceptedTargets = game.Tankers.Cast<Enemy>().Concat(game.Flippers).ToArray();
        Enemy selected = Assert.IsAssignableFrom<Enemy>(SuperZapperSystem.SelectTarget(
            acceptedTargets,
            game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount,
            game.CurrentLevel.Continuous));
        int oldTimer = selected.PendingZapTicks;
        Assert.True(oldTimer > 1);
        Dictionary<Enemy, int> timersBeforeSecondCharge = acceptedTargets
            .ToDictionary(enemy => enemy, enemy => enemy.PendingZapTicks);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(0, game.SuperZapperChargesRemaining);
        Assert.Equal(3, selected.PendingZapTicks);
        Assert.NotEqual(oldTimer - 1, selected.PendingZapTicks);
        foreach ((Enemy enemy, int timer) in timersBeforeSecondCharge)
            Assert.Equal(ReferenceEquals(enemy, selected) ? 3 : timer - 1,
                enemy.PendingZapTicks);
        Assert.All(bullets, bullet => Assert.Equal(0, bullet.PendingZapTicks));
    }

    [Fact]
    public void ShotPreemptsFirstChargeCarrierZapAndItsRemovedTimerCannotScoreLater()
    {
        TempestGame game = CreateArtificialMultiCarrierGame();
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        Assert.Equal(LevelState.Playing, game.State);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        int firstChargeScore = game.Score.Score;
        Tanker target = FindTankerSafeForShot(game);
        Assert.True(target.PendingZapTicks > 0);

        game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.True(game.Shots.Count == 1,
            $"Shot was consumed: score={game.Score.Score}, target={target.OriginalSlotId}, " +
            $"targetHeight={target.Height}, state={game.State}.");
        Shot shot = Assert.Single(game.Shots);
        shot.Height = target.Height + 5;
        game.RunOneStep();

        Assert.DoesNotContain(target, game.Tankers);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(target.OriginalSlotId));
        Assert.Equal(200, game.Score.Score - firstChargeScore);
        int detachedZapTimer = target.PendingZapTicks;

        for (int tick = 0; tick < 30; tick++)
            game.RunOneStep();

        Assert.Equal(detachedZapTimer, target.PendingZapTicks);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(target.OriginalSlotId));
        Assert.DoesNotContain(target, game.Tankers);
    }

    [Fact]
    public void ProductionEggMovementKeepsEightStepCadenceAndRoamingBounds()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);
        Egg egg = level.Eggs[0];

        for (int tick = 0; tick < 7; tick++)
            egg.Update(level);

        Assert.Equal(-10, egg.Height);
        Assert.Equal(0, egg.EdgeIndex);

        egg.Update(level);
        Assert.Equal(-9, egg.Height);
        Assert.Equal(15, egg.EdgeIndex);

        for (int movement = 0; movement < 8; movement++)
            for (int tick = 0; tick < 8; tick++)
                egg.Update(level);

        Assert.Equal(-1, egg.Height);
        Assert.Equal(-1, egg.VerticalDirection);

        for (int movement = 0; movement < 19; movement++)
            for (int tick = 0; tick < 8; tick++)
                egg.Update(level);

        Assert.Equal(-20, egg.Height);
        Assert.Equal(1, egg.VerticalDirection);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ReconstructedProductionQueueResetsTheSixAtFiftyReleasePhase()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 14)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);

        level.ResetAfterDeath();
        for (int tick = 0; tick < 49; tick++)
            level.UpdateEggRelease();

        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(11, level.ReserveEggs);
        level.UpdateEggRelease();

        Assert.Equal(10, level.Eggs.Count);
        Assert.Equal(5, level.ReserveEggs);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ActualGameHatchesASelectedTankerAndFlipperThroughProductionOrchestration()
    {
        InclusiveRangeScript random = new(
        [
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            (0, 100, 96),
            (0, 100, 96),
            (0, 2, 1),
            (0, 2, 2),
            (0, 2, 0),
            (1, 2, 1),
            (1, 64, 64),
            (0, 100, 95)
        ]);
        TempestGame game = new(LevelConfiguration.Level3Mixed, random: random);

        for (int tick = 0; tick < 41; tick++)
            game.RunOneStep();

        Assert.Single(game.Tankers);
        Assert.Single(game.Flippers);
        Assert.Equal(0, game.Tankers[0].OriginalSlotId);
        Assert.Equal(1, game.Flippers[0].OriginalSlotId);
        Assert.Equal(1, game.Tankers[0].BulletAmmunition);
        Assert.Equal(0, game.Flippers[0].ShotDelayTicks);
        Assert.Equal(Level3OriginalType.Tanker,
            game.CurrentLevel.Level3Ledger!.GetOriginalType(0));
        Assert.Equal(new[] { 0, 1 },
            game.CurrentLevel.Level3Ledger.GetCargoIdsForOriginal(0));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ProductionTankerFiresWithoutOptionalFlagWhileOrdinaryFlippersStaySilent()
    {
        InclusiveRangeScript random = new(
        [
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            (0, 100, 96),
            (0, 100, 95),
            .. Enumerable.Repeat((0, 100, 95), 38),
            (0, 2, 0),
            (1, 2, 1),
            (1, 64, 1),
            .. Enumerable.Repeat((0, 100, 95), 9),
            .. Enumerable.Range(0, 6)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            .. Enumerable.Repeat((0, 100, 95), 8),
            (0, 100, 100)
        ]);
        TempestGame game = new(LevelConfiguration.Level3Mixed, random: random);

        for (int tick = 0; tick < 40; tick++)
            game.RunOneStep();

        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(1, tanker.BulletAmmunition);
        Assert.False(game.EnemyBulletsTest);

        for (int tick = 0; tick < 17; tick++)
            game.RunOneStep();

        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(tanker.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(tanker.Height, bullet.Height);
        Assert.Equal(0, tanker.BulletAmmunition);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void NaturalTankerReleaseEscapesWithoutScoreAndMaterializesItsOriginalPair()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            (0, 100, 95), (-6, 4, 1));
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.State == TankerState.Climbing)
            tanker.UpdateForDevelopment();

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(Level3OriginalState.Escaped,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(tanker.OriginalSlotId));
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(new[] { 0, 1 }, cargo.Select(flipper => flipper.CargoUnitId));
        Assert.All(cargo, child =>
        {
            Assert.Equal(0, child.PendingZapTicks);
            Assert.Equal(tanker.Height, child.Height);
        });
        Assert.Equal(Level3CargoState.Active,
            game.CurrentLevel.Level3Ledger.GetCargoState(0));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void PlayerShotDestroysCarrierOnceAndActivatesItsExistingCargoPair()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            (0, 100, 95), (-6, 4, 1), (0, 1, 0),
            (0, 100, 95), (0, 100, 95),
            (0, 100, 95), (0, 100, 95));
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.True(game.SuperZapper.TryActivate(game.Tankers, game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(4, tanker.PendingZapTicks);

        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(tanker.OriginalSlotId));
        Assert.Equal(new[] { Level3CargoState.Active, Level3CargoState.Active },
            new[] { 0, 1 }.Select(game.CurrentLevel.Level3Ledger.GetCargoState));
        Assert.Equal(new[] { 0, 1 }, game.Flippers
            .Where(flipper => flipper.IsTankerCargo)
            .Select(flipper => flipper.CargoUnitId).Order());
        Assert.False(game.EnemyBulletsTest);
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            child => Assert.Equal(0, child.ShotDelayTicks));
        Assert.False(game.CurrentLevel.Level3Ledger.HasPendingShotActivation);
        Assert.Equal(3, tanker.PendingZapTicks);
        game.RunOneStep();
        Assert.Equal(200, game.Score.Score);
        game.RunOneStep();
        Assert.Equal(200, game.Score.Score);
        game.RunOneStep();
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(3, tanker.PendingZapTicks);
        Assert.All(new[] { 0, 1 },
            cargoId => Assert.Equal(Level3CargoState.Active,
                game.CurrentLevel.Level3Ledger.GetCargoState(cargoId)));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void TankerZapExpiryScoresOnceAndCreatesItsExistingStillbornChildren()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            new[] { (0, 100, 95), (-6, 4, 1) }
                .Concat(Enumerable.Repeat((0, 100, 95), 8)).ToArray());
        Tanker tanker = Assert.Single(game.Tankers);
        tanker.TryZap(1);

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(200, game.Score.Score);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(new[] { 0, 1 }, cargo.Select(flipper => flipper.CargoUnitId));
        Assert.All(cargo, child => Assert.Equal(8, child.PendingZapTicks));
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(tanker.OriginalSlotId));

        for (int tick = 0; tick < 8; tick++)
            game.RunOneStep();

        Assert.Equal(500, game.Score.Score);
        Assert.Equal(new[] { Level3CargoState.Destroyed, Level3CargoState.Destroyed },
            new[] { 0, 1 }.Select(game.CurrentLevel.Level3Ledger.GetCargoState));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void DeathReentryPrioritizesMaterializedCargoAndPreservesIdentityAcrossRepeatedResets()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 12)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 1) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsTanker(0);
        IReadOnlyList<int> cargo = ledger.MaterializeLiveCargo(0);
        ledger.MarkOriginalEscaped(0);
        LedgerSnapshot beforeDeath = CaptureLedgerSnapshot(ledger);

        level.ResetAfterDeath();

        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        Level3Identity[] firstReentry = GetEggIdentities(level.Eggs);
        Assert.Equal(new[]
        {
            new Level3Identity(Level3IdentityKind.Cargo, cargo[0]),
            new Level3Identity(Level3IdentityKind.Cargo, cargo[1]),
            new Level3Identity(Level3IdentityKind.Original, 1),
            new Level3Identity(Level3IdentityKind.Original, 2)
        }, firstReentry);
        Assert.Equal(12, level.ReserveEggs);
        Assert.Equal(0, level.Level3CargoReserveEggs);
        Assert.All(cargo, id =>
            Assert.Equal(Level3CargoState.Egg, ledger.GetCargoState(id)));
        ledger.ValidateRepresentation(firstReentry.Concat(ledger.OriginalIdsInReserve()
            .Select(id => new Level3Identity(Level3IdentityKind.Original, id))));

        level.ResetAfterDeath();

        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        Assert.Equal(firstReentry, GetEggIdentities(level.Eggs));
        Assert.Equal(12, level.ReserveEggs);
        Assert.Equal(0, level.Level3CargoReserveEggs);
        ledger.ValidateRepresentation(GetEggIdentities(level.Eggs)
            .Concat(ledger.OriginalIdsInReserve()
                .Select(id => new Level3Identity(Level3IdentityKind.Original, id))));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ConsecutiveOrdinaryDeathsConserveSeveralCarriersCargoEggsAndReentryReserve()
    {
        TempestGame game = CreateArtificialMultiCarrierGame();
        Assert.False(game.EnemyBulletsTest);
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        Tanker[] carriers = game.Tankers.OrderBy(tanker => tanker.OriginalSlotId).ToArray();
        Tanker[] releasedCarriers = carriers.Take(3).ToArray();
        Tanker[] latentCarriers = carriers.Skip(3).ToArray();
        int[] materializedCargo = releasedCarriers
            .SelectMany(tanker => ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId))
            .Order().ToArray();
        int[] latentCargo = latentCarriers
            .SelectMany(tanker => ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId))
            .Order().ToArray();

        foreach (Tanker carrier in releasedCarriers)
            while (carrier.State == TankerState.Climbing)
                carrier.UpdateForDevelopment();
        MovePlayerToSafeLane(game);
        game.RunOneStep();

        Assert.Equal(3, game.Tankers.Count);
        Assert.Equal(6, materializedCargo.Length);
        Assert.Equal(6, latentCargo.Length);
        Assert.All(releasedCarriers, carrier =>
            Assert.Equal(Level3OriginalState.Escaped,
                ledger.GetOriginalState(carrier.OriginalSlotId)));
        Assert.All(materializedCargo, id =>
            Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(id)));
        Flipper terminalOriginal = game.Flippers.First(flipper => !flipper.IsTankerCargo);
        Assert.True(terminalOriginal.TryZap(1));
        MovePlayerToSafeLane(game);
        game.RunOneStep();
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(terminalOriginal.OriginalSlotId));

        foreach (Tanker carrier in latentCarriers)
            Assert.True(carrier.TryZap(64));
        MovePlayerToSafeLane(game);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot oldShot = Assert.Single(game.Shots);
        Flipper lethalOriginal = game.Flippers.First(flipper => !flipper.IsTankerCargo);
        ForceFlipperIntoPlayer(game, lethalOriginal);
        MovePlayerToSafeLane(game);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.NotEmpty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.DoesNotContain(oldShot, game.Shots);
        Assert.All(latentCarriers, carrier => Assert.True(carrier.PendingZapTicks > 0));
        LedgerSnapshot beforeDeath = CaptureLedgerSnapshot(ledger);
        AssertRuntimeIdentitiesAreConserved(game);
        Flipper[] oldFlippers = game.Flippers.ToArray();
        EnemyBullet[] oldBullets = game.EnemyBullets.ToArray();
        Dictionary<int, int> oldFiringTimers = latentCarriers
            .ToDictionary(carrier => carrier.OriginalSlotId, carrier => carrier.BulletTimer);

        for (int death = 1; death <= 2; death++)
        {
            LedgerSnapshot immediatelyBeforeRebuild = CaptureLedgerSnapshot(ledger);
            AssertLedgerSnapshotConserved(ledger, beforeDeath);
            for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
                game.RunOneStep();

            Assert.Equal(LevelState.Playing, game.State);
            Assert.Equal(3 - death, game.Player.Lives);
            Assert.Same(level, game.CurrentLevel);
            Assert.Same(ledger, level.Level3Ledger);
            AssertLedgerSnapshotConserved(ledger, immediatelyBeforeRebuild);
            AssertLedgerSnapshotConserved(ledger, beforeDeath);
            AssertRuntimeIdentitiesAreConserved(game);
            Assert.Equal(materializedCargo.Take(4), game.Eggs.Select(egg => egg.CargoUnitId));
            Assert.Equal(materializedCargo.Skip(4), ledger.CargoIdsInReentryReserve());
            Assert.Equal(2, level.Level3CargoReserveEggs);
            Assert.All(game.Eggs, egg => Assert.Equal(-1, egg.OriginalSlotId));
            Assert.All(latentCargo, id =>
                Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id)));
            Assert.All(latentCarriers, carrier =>
                Assert.Equal(Level3OriginalState.Reserve,
                    ledger.GetOriginalState(carrier.OriginalSlotId)));
            Assert.Empty(game.Flippers);
            Assert.Empty(game.Tankers);
            Assert.Empty(game.Shots);
            Assert.Empty(game.EnemyBullets);
            Assert.False(ledger.HasPendingShotActivation);
            Assert.All(oldFlippers, flipper => Assert.DoesNotContain(flipper, game.Flippers));
            Assert.All(carriers, carrier => Assert.DoesNotContain(carrier, game.Tankers));
            Assert.All(oldBullets, bullet => Assert.DoesNotContain(bullet, game.EnemyBullets));

            if (death == 2)
                break;

            Egg lethalEgg = game.Eggs[0];
            lethalEgg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                lethalEgg.Update(level);
            MovePlayerToSafeLane(game);
            game.RunOneStep();
            Flipper reenteredCargo = Assert.Single(game.Flippers);
            Assert.Equal(lethalEgg.CargoUnitId, reenteredCargo.CargoUnitId);
            Assert.Equal(0, reenteredCargo.PendingZapTicks);
            Assert.Equal(0, reenteredCargo.ShotDelayTicks);
            ForceFlipperIntoPlayer(game, reenteredCargo);
            game.RunOneStep();

            Assert.Equal(LevelState.PlayerDying, game.State);
            Assert.Equal(materializedCargo.Skip(1).Take(3),
                game.Eggs.Select(egg => egg.CargoUnitId));
            Assert.All(game.Eggs, egg =>
                Assert.Equal(Level3CargoState.Egg, ledger.GetCargoState(egg.CargoUnitId)));
            Assert.Equal(materializedCargo.Skip(4), ledger.CargoIdsInReentryReserve());
            AssertRuntimeIdentitiesAreConserved(game);
            AssertLedgerSnapshotConserved(ledger, beforeDeath);
        }

        for (int tick = 0; tick < 50; tick++)
            level.UpdateEggRelease();
        Assert.Equal(materializedCargo,
            game.Eggs.Where(egg => egg.CargoUnitId >= 0).Select(egg => egg.CargoUnitId));
        Assert.Equal(0, level.Level3CargoReserveEggs);
        foreach (Tanker oldCarrier in latentCarriers)
        {
            Egg carrierEgg = Assert.Single(game.Eggs,
                egg => egg.OriginalSlotId == oldCarrier.OriginalSlotId);
            carrierEgg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                carrierEgg.Update(level);
        }
        MovePlayerToSafeLane(game);
        game.RunOneStep(new PlayerInput(true, 0, false, false));

        Assert.Equal(latentCarriers.Select(carrier => carrier.OriginalSlotId),
            game.Tankers.Select(carrier => carrier.OriginalSlotId));
        foreach (Tanker carrier in game.Tankers)
        {
            Assert.DoesNotContain(carrier, carriers);
            Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(carrier.OriginalSlotId));
            Assert.Equal(1, carrier.BulletAmmunition);
            Assert.Equal(79, carrier.BulletTimer);
            Assert.NotEqual(oldFiringTimers[carrier.OriginalSlotId], carrier.BulletTimer);
            Assert.Equal(0, carrier.PendingZapTicks);
        }
        Assert.NotSame(oldShot, Assert.Single(game.Shots));
        Assert.Empty(game.EnemyBullets);
        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        AssertRuntimeIdentitiesAreConserved(game);
    }

    [Fact]
    public void ActualGameDeathRebuildsUnreleasedTankerWithTheSameLatentPair()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            (0, 100, 95), (-6, 4, 1),
            (0, 100, 95),
            (0, 15, 1), (0, 1, 0),
            (0, 15, 2), (0, 1, 1),
            (0, 15, 3), (0, 1, 0),
            (0, 15, 4), (0, 1, 1),
            (0, 100, 96), (0, 100, 96),
            (1, 2, 2), (1, 64, 64));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Tanker original = Assert.Single(game.Tankers);
        int[] cargoIds = ledger.GetCargoIdsForOriginal(original.OriginalSlotId).ToArray();
        int initialCargoCount = ledger.CargoCount;
        int initialIdentityCount = ledger.TotalIdentityCount;

        Assert.False(game.EnemyBulletsTest);
        Assert.Equal(0, original.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(original.OriginalSlotId));
        Assert.Equal(1, original.BulletAmmunition);
        Assert.Equal(79, original.BulletTimer);
        Assert.Equal(new[] { 0, 1 }, cargoIds);
        Assert.Equal(new[] { 0, 0 }, cargoIds.Select(ledger.GetCargoOwner));
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));

        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers,
            candidate => !candidate.IsTankerCargo);
        ForceFlipperIntoPlayer(game, flipper);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Same(original, Assert.Single(game.Tankers));
        int oldAmmunition = original.BulletAmmunition;
        int oldTimer = original.BulletTimer;

        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Flippers);
        Assert.Equal(new[] { 0, 1, 2, 3 }, game.Eggs
            .Select(egg => egg.OriginalSlotId).Order());
        Assert.DoesNotContain(game.Eggs, egg => egg.CargoUnitId >= 0);
        Assert.Equal(initialCargoCount, ledger.CargoCount);
        Assert.Equal(initialIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(new[] { Level3CargoState.Latent, Level3CargoState.Latent },
            cargoIds.Select(ledger.GetCargoState));
        Assert.Equal(cargoIds, ledger.GetCargoIdsForOriginal(0));
        Assert.Equal(new[] { 0, 0 }, cargoIds.Select(ledger.GetCargoOwner));
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));

        for (int tick = 0; tick < 50 && game.Tankers.Count == 0; tick++)
            game.RunOneStep();

        Tanker reentered = Assert.Single(game.Tankers);
        Assert.NotSame(original, reentered);
        Assert.Equal(0, reentered.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(reentered.OriginalSlotId));
        Assert.Equal(Level3OriginalState.ActiveTanker,
            ledger.GetOriginalState(reentered.OriginalSlotId));
        Assert.Equal(initialCargoCount, ledger.CargoCount);
        Assert.Equal(initialIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(cargoIds, ledger.GetCargoIdsForOriginal(reentered.OriginalSlotId));
        Assert.Equal(new[] { 0, 0 }, cargoIds.Select(ledger.GetCargoOwner));
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.Equal(2, reentered.BulletAmmunition);
        Assert.Equal(79, reentered.BulletTimer);
        Assert.NotEqual(oldAmmunition, reentered.BulletAmmunition);
        Assert.NotEqual(oldTimer, reentered.BulletTimer);

        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void DeathAfterShotReleaseReentersOnlyUnresolvedOwnedCargoAndOriginals()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            (0, 100, 95), (-6, 4, 1), (0, 1, 0),
            (0, 100, 95),
            (0, 15, 5), (0, 1, 0),
            (0, 15, 6), (0, 1, 1),
            (0, 15, 7), (0, 1, 0),
            (0, 15, 8), (0, 1, 1));
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        Flipper original = Assert.Single(game.Flippers,
            candidate => !candidate.IsTankerCargo);
        ForceFlipperIntoPlayer(game, original);
        game.RunOneStep();
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(LevelState.PlayerDying, game.State);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(0));
        Assert.Equal(Level3OriginalType.Flipper,
            game.CurrentLevel.Level3Ledger.GetOriginalType(1));
        Assert.Equal(new[]
        {
            new Level3Identity(Level3IdentityKind.Cargo, 0),
            new Level3Identity(Level3IdentityKind.Cargo, 1),
            new Level3Identity(Level3IdentityKind.Original, 1),
            new Level3Identity(Level3IdentityKind.Original, 2)
        }, GetEggIdentities(game.Eggs));
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void DeathDuringZapChildTimersReentersOnlyTheUnresolvedSiblingCargo()
    {
        BoundedCompositionRandom random = new(maximumCalls: 5000, variedLanes: true);
        TempestGame game = new(MultiCarrierFixtureConfiguration(), random: random);
        for (int tick = 0; tick < 200; tick++)
            game.RunOneStep();

        Tanker tanker = Assert.Single(game.Tankers, candidate =>
            candidate.OriginalSlotId == 0);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int[] cargoIds = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
        int initialCargoCount = ledger.CargoCount;
        int initialIdentityCount = ledger.TotalIdentityCount;
        Flipper[] children = ExpireTankerBySuperZapper(game, tanker);
        Assert.Equal(2, children.Length);

        Assert.DoesNotContain(tanker, game.Tankers);
        Flipper firstChild = Assert.Single(game.Flippers,
            flipper => flipper.CargoUnitId == cargoIds[0]);
        Flipper sibling = Assert.Single(game.Flippers,
            flipper => flipper.CargoUnitId == cargoIds[1]);
        Assert.Equal(8, firstChild.PendingZapTicks);
        Assert.Equal(8, sibling.PendingZapTicks);

        Assert.True(firstChild.TryZap(1));
        game.RunOneStep();

        Assert.DoesNotContain(firstChild, game.Flippers);
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoIds[0]));
        Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(cargoIds[1]));
        Assert.Equal(7, sibling.PendingZapTicks);

        Flipper original = game.Flippers.First(flipper => !flipper.IsTankerCargo);
        ForceFlipperIntoPlayer(game, original);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(6, sibling.PendingZapTicks);
        int scoreBeforeDeath = game.Score.Score;
        LedgerSnapshot beforeDeath = CaptureLedgerSnapshot(ledger);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoIds[0]));
        Assert.Equal(Level3CargoState.Egg, ledger.GetCargoState(cargoIds[1]));
        Assert.Contains(game.Eggs, egg => egg.CargoUnitId == cargoIds[1]);
        Assert.DoesNotContain(game.Eggs,
            egg => egg.CargoUnitId == cargoIds[0]);
        Assert.Equal(1, game.Eggs.Count(egg => egg.CargoUnitId == cargoIds[1]));
        Assert.Equal(initialCargoCount, ledger.CargoCount);
        Assert.Equal(initialIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(new[] { 0, 0 }, cargoIds.Select(ledger.GetCargoOwner));
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.Equal(6, sibling.PendingZapTicks);
        Assert.DoesNotContain(firstChild, game.Flippers);
        Assert.DoesNotContain(sibling, game.Flippers);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        AssertRuntimeIdentitiesAreConserved(game);
        Assert.Equal(scoreBeforeDeath, game.Score.Score);
    }

    [Fact]
    public void DeathAfterZapExpiryKeepsCarrierAndResolvedCargoTerminal()
    {
        BoundedCompositionRandom random = new(maximumCalls: 5000, variedLanes: true);
        TempestGame game = new(MultiCarrierFixtureConfiguration(), random: random);
        for (int tick = 0; tick < 200; tick++)
            game.RunOneStep();

        Tanker tanker = Assert.Single(game.Tankers, candidate =>
            candidate.OriginalSlotId == 0);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int[] cargoIds = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
        int initialCargoCount = ledger.CargoCount;
        int initialIdentityCount = ledger.TotalIdentityCount;
        Flipper[] children = ExpireTankerBySuperZapper(game, tanker);

        Assert.Equal(8, children[0].PendingZapTicks);
        Assert.Equal(8, children[1].PendingZapTicks);
        for (int tick = 0; tick < 8; tick++)
        {
            MovePlayerToSafeLane(game);
            game.RunOneStep();
        }

        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.All(cargoIds,
            id => Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(id)));
        Assert.DoesNotContain(game.Flippers,
            flipper => cargoIds.Contains(flipper.CargoUnitId));

        Flipper original = game.Flippers.First(flipper => !flipper.IsTankerCargo);
        ForceFlipperIntoPlayer(game, original);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        int scoreBeforeDeath = game.Score.Score;
        LedgerSnapshot beforeDeath = CaptureLedgerSnapshot(ledger);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.All(cargoIds,
            id => Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(id)));
        Assert.DoesNotContain(game.Eggs,
            egg => egg.OriginalSlotId == tanker.OriginalSlotId ||
                cargoIds.Contains(egg.CargoUnitId));
        Assert.Equal(initialCargoCount, ledger.CargoCount);
        Assert.Equal(initialIdentityCount, ledger.TotalIdentityCount);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.All(cargoIds, id => Assert.Equal(0, ledger.GetCargoOwner(id)));
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.All(children, child => Assert.DoesNotContain(child, game.Flippers));
        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        AssertRuntimeIdentitiesAreConserved(game);
        Assert.Equal(scoreBeforeDeath, game.Score.Score);
    }

    [Fact]
    public void DeathWithBothZapReleasedCargoTimersReentersFreshUnzappedCargo()
    {
        BoundedCompositionRandom random = new(maximumCalls: 5000, variedLanes: true);
        TempestGame game = new(MultiCarrierFixtureConfiguration(), random: random);
        for (int tick = 0; tick < 200; tick++)
            game.RunOneStep();

        Tanker tanker = Assert.Single(game.Tankers, candidate =>
            candidate.OriginalSlotId == 0);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int[] cargoIds = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
        int initialCargoCount = ledger.CargoCount;
        int initialIdentityCount = ledger.TotalIdentityCount;
        Flipper[] oldChildren = ExpireTankerBySuperZapper(game, tanker);

        Assert.All(oldChildren, child => Assert.Equal(8, child.PendingZapTicks));
        Flipper original = game.Flippers.First(flipper => !flipper.IsTankerCargo);
        ForceFlipperIntoPlayer(game, original);
        game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.All(oldChildren, child => Assert.Equal(7, child.PendingZapTicks));
        int scoreBeforeDeath = game.Score.Score;
        LedgerSnapshot beforeDeath = CaptureLedgerSnapshot(ledger);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.All(cargoIds,
            id => Assert.Equal(Level3CargoState.Egg, ledger.GetCargoState(id)));
        Assert.Equal(cargoIds.Order(), game.Eggs
            .Where(egg => egg.CargoUnitId >= 0)
            .Select(egg => egg.CargoUnitId).Order());
        Assert.All(cargoIds, id =>
        {
            Assert.Equal(1, game.Eggs.Count(egg => egg.CargoUnitId == id));
            Assert.Equal(tanker.OriginalSlotId, ledger.GetCargoOwner(id));
        });
        Assert.Equal(new[] { 0, 1 }, cargoIds.Select(ledger.GetCargoOrdinal));
        Assert.All(oldChildren, child =>
        {
            Assert.Equal(7, child.PendingZapTicks);
            Assert.DoesNotContain(child, game.Flippers);
        });
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.Equal(initialCargoCount, ledger.CargoCount);
        Assert.Equal(initialIdentityCount, ledger.TotalIdentityCount);
        AssertLedgerSnapshotConserved(ledger, beforeDeath);
        AssertRuntimeIdentitiesAreConserved(game);
        Assert.Equal(scoreBeforeDeath, game.Score.Score);
    }

    [Fact]
    public void BlockedReadyEggDoesNotPreventOtherReadyEggsFromHatching()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }));
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);
        Egg blocked = level.Eggs[0];
        Egg available = level.Eggs[1];
        blocked.BeginApproach();
        available.BeginApproach();
        for (int tick = 0; tick < 40; tick++)
        {
            blocked.Update(level);
            available.Update(level);
        }

        List<Enemy> active = [];
        level.HatchReadyMixedEnemies(egg =>
        {
            if (egg.OriginalSlotId == blocked.OriginalSlotId)
                return null;

            level.Level3Ledger!.CommitOriginalAsFlipper(egg.OriginalSlotId);
            return new Flipper(egg.EdgeIndex, 0);
        }, active.Add);

        Assert.Single(active);
        Assert.Contains(blocked, level.Eggs);
        Assert.Equal(EggState.ReadyToHatch, blocked.State);
        Assert.DoesNotContain(available, level.Eggs);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void DiagnosticKeepsBlockedEggsReadyAndRecomputesCapacityForEachSameTickHatch()
    {
        InclusiveRangeScript random = new(
            Enumerable.Range(0, 2)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }));
        LevelConfiguration twoIdentityConfiguration = LevelConfiguration.Level3Mixed with
        {
            EnemySupply = 2,
            InitialActiveEggCount = 2
        };
        Level level = new(twoIdentityConfiguration,
            randomInclusive: random.NextInclusive);
        Egg firstEgg = level.Eggs[0];
        Egg secondEgg = level.Eggs[1];
        foreach (Egg egg in new[] { firstEgg, secondEgg })
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
        }

        Level3WaveLedger ledger = level.Level3Ledger!;
        List<Enemy> active = [];
        int ordinaryFlippersBeforeBatch = 6;

        Enemy? CreateAdmittedFlipper(Egg egg)
        {
            Level3MixedAdmissionSnapshot snapshot = new(
                OrdinaryFlippers: ordinaryFlippersBeforeBatch +
                    active.OfType<Flipper>().Count(),
                ReleasedCargoFlippers: 0,
                PendingShotCargoFlippers: 0,
                ActiveTankers: 1,
                ActiveOtherEnemies: 0,
                LatentTankerCargoFlippers: Tanker.CarriedFlipperCount);
            Level3OriginalType? type = Level3MixedAdmissionPolicy
                .SelectAdmissibleOriginalType(snapshot, ledger.GetOriginalType(egg.OriginalSlotId));
            if (type != Level3OriginalType.Flipper)
                return null;

            ledger.CommitOriginalAsFlipper(egg.OriginalSlotId);
            return new Flipper(egg.EdgeIndex, 0);
        }

        level.HatchReadyMixedEnemies(CreateAdmittedFlipper, active.Add);
        Assert.Empty(active);
        Assert.Equal(EggState.ReadyToHatch, firstEgg.State);
        Assert.Equal(EggState.ReadyToHatch, secondEgg.State);
        Assert.Equal(Level3OriginalState.Egg,
            ledger.GetOriginalState(firstEgg.OriginalSlotId));
        Assert.Equal(Level3OriginalState.Egg,
            ledger.GetOriginalState(secondEgg.OriginalSlotId));
        Assert.False(level.IsComplete(active.Count));

        ordinaryFlippersBeforeBatch = 5;
        level.HatchReadyMixedEnemies(CreateAdmittedFlipper, active.Add);
        Assert.Single(active);
        Assert.DoesNotContain(firstEgg, level.Eggs);
        Assert.Contains(secondEgg, level.Eggs);
        Assert.Equal(EggState.ReadyToHatch, secondEgg.State);
        Assert.Equal(Level3OriginalState.ActiveFlipper,
            ledger.GetOriginalState(firstEgg.OriginalSlotId));
        Assert.Equal(Level3OriginalState.Egg,
            ledger.GetOriginalState(secondEgg.OriginalSlotId));

        ordinaryFlippersBeforeBatch = 4;
        level.HatchReadyMixedEnemies(CreateAdmittedFlipper, active.Add);
        Assert.Equal(2, active.Count);
        Assert.DoesNotContain(level.Eggs, egg => egg.State == EggState.ReadyToHatch);
        Assert.Equal(Level3OriginalState.ActiveFlipper,
            ledger.GetOriginalState(secondEgg.OriginalSlotId));
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void SyntheticSixTankerHatchFixturePreservesSelectedTypeWhileAtCapacity()
    {
        InclusiveRangeScript random = new(
        [
            .. Enumerable.Range(0, 8)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            .. Enumerable.Repeat((-6, 3, 1), 3)
        ]);
        Level level = new(LevelConfiguration.Level3Mixed,
            randomInclusive: random.NextInclusive);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsTanker(0);
        ledger.RebuildAfterDeath();
        level.ResetAfterDeath();

        foreach (Egg egg in level.Eggs)
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
        }

        List<Enemy> active = [];
        // This is a custom hatch-factory stress state, not the canonical production policy.
        int activeTankers = 6;
        Enemy? CreateEnemy(Egg egg)
        {
            int originalId = egg.OriginalSlotId;
            Level3OriginalType selectedType = ledger.GetOriginalType(originalId);
            if (selectedType == Level3OriginalType.Unselected)
            {
                selectedType = Level3WaveSelection.SelectOriginalType(
                    random.NextInclusive, activeTankers,
                    activeTankers + active.Count, level.Eggs.Count);
            }

            if (selectedType == Level3OriginalType.Tanker)
            {
                if (activeTankers == 6)
                    return null;

                ledger.CommitOriginalAsTanker(originalId);
                activeTankers++;
                return new Tanker(egg.EdgeIndex, originalId);
            }

            ledger.CommitOriginalAsFlipper(originalId);
            return new Flipper(egg.EdgeIndex, 0);
        }

        level.HatchReadyMixedEnemies(CreateEnemy, active.Add);

        Assert.Equal(3, active.Count);
        Assert.Equal(3, active.OfType<Flipper>().Count());
        Egg retainedEgg = Assert.Single(level.Eggs);
        Assert.Equal(0, retainedEgg.OriginalSlotId);
        Assert.Equal(EggState.ReadyToHatch, retainedEgg.State);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(new[] { Level3CargoState.Latent, Level3CargoState.Latent },
            new[] { 0, 1 }.Select(ledger.GetCargoState));
        Assert.Equal(0, random.Remaining);

        activeTankers--;
        level.HatchReadyMixedEnemies(CreateEnemy, active.Add);

        Assert.Equal(4, active.Count);
        Assert.Single(active.OfType<Tanker>());
        Assert.Equal(6, activeTankers);
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(0));
        Assert.Equal(0, random.Remaining);
    }

    [Theory]
    [InlineData("OriginalReserve")]
    [InlineData("OriginalEgg")]
    [InlineData("OriginalFlipper")]
    [InlineData("ActiveTanker")]
    [InlineData("LatentCargo")]
    [InlineData("CargoReserve")]
    [InlineData("CargoEgg")]
    [InlineData("CargoFlipper")]
    public void OrdinaryCompletionWaitsForTheFinalBlocker(string blocker)
    {
        bool cargoBlocker = blocker.StartsWith("Cargo", StringComparison.Ordinal);
        bool selectedTanker = cargoBlocker || blocker is "ActiveTanker" or "LatentCargo";
        InclusiveRangeScript random = new(
        [
            (0, 15, 0), (0, 1, 0),
            .. selectedTanker && !cargoBlocker
                ? new[] { (1, 2, 1), (1, 64, 64) }
                : Array.Empty<(int, int, int)>(),
            .. blocker == "OriginalReserve"
                ? new[] { (-6, 4, 0), (1, 4, 1) }
                : Array.Empty<(int, int, int)>()
        ]);
        TempestGame game = new(LevelConfiguration.Level3Mixed with
        {
            InitialActiveEggCount = 0
        }, random: random);
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        for (int id = 1; id < ledger.OriginalCount; id++)
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        if (blocker != "OriginalReserve")
        {
            ledger.MarkOriginalEgg(0);
            if (selectedTanker)
                ledger.CommitOriginalAsTanker(0);
            else
                ledger.CommitOriginalAsFlipper(0);
            if (cargoBlocker)
            {
                ledger.MaterializeLiveCargo(0);
                ledger.MarkOriginalDestroyed(0);
                ledger.MarkCargoDestroyed(1);
            }
            ledger.RebuildAfterDeath();
        }

        if (blocker is not ("OriginalReserve" or "LatentCargo" or "CargoReserve"))
            for (int tick = 0; tick < 50; tick++)
                level.UpdateEggRelease();
        if (blocker is "OriginalFlipper" or "ActiveTanker" or "CargoFlipper")
        {
            Egg egg = Assert.Single(game.Eggs);
            egg.BeginApproach();
            for (int tick = 0; tick < 40; tick++)
                egg.Update(level);
            game.RunOneStep();
        }

        Assert.Equal(cargoBlocker ? Array.Empty<int>() : new[] { 0 },
            ledger.UnresolvedOriginalIds.Order());
        Assert.Equal(cargoBlocker ? new[] { 0 } :
            selectedTanker ? new[] { 0, 1 } : Array.Empty<int>(),
            ledger.UnresolvedCargoIds.Order());
        switch (blocker)
        {
            case "OriginalReserve":
                Assert.Equal(new[] { 0 }, ledger.OriginalIdsInReserve());
                Assert.Empty(game.Eggs);
                break;
            case "OriginalEgg":
                Assert.Equal(0, Assert.Single(game.Eggs).OriginalSlotId);
                Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(0));
                break;
            case "OriginalFlipper":
                Assert.False(Assert.Single(game.Flippers).IsTankerCargo);
                Assert.InRange(game.Flippers[0].Height, 0, Level.Depth - 10);
                break;
            case "ActiveTanker":
                Assert.InRange(Assert.Single(game.Tankers).Height, 0, Level.Depth - 10);
                Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(0));
                break;
            case "LatentCargo":
                // Latent cargo necessarily retains its unresolved selected owner, not an active enemy.
                Assert.Empty(game.Tankers);
                Assert.Empty(game.Eggs);
                Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
                Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(0));
                Assert.All(new[] { 0, 1 }, id =>
                    Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id)));
                break;
            case "CargoReserve":
                Assert.Equal(new[] { 0 }, ledger.CargoIdsInReentryReserve());
                Assert.Empty(game.Eggs);
                break;
            case "CargoEgg":
                Assert.Equal(0, Assert.Single(game.Eggs).CargoUnitId);
                Assert.Equal(-1, game.Eggs[0].OriginalSlotId);
                Assert.Equal(Level3CargoState.Egg, ledger.GetCargoState(0));
                break;
            case "CargoFlipper":
                Assert.True(Assert.Single(game.Flippers).IsTankerCargo);
                Assert.Equal(0, game.Flippers[0].CargoUnitId);
                Assert.InRange(game.Flippers[0].Height, 0, Level.Depth - 10);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(blocker), blocker,
                    "Unknown ordinary completion blocker.");
        }
        AssertRuntimeIdentitiesAreConserved(game);
        LedgerSnapshot before = CaptureLedgerSnapshot(ledger);

        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.IsExiting);
        Assert.False(game.LevelComplete);
        AssertLedgerSnapshotConserved(ledger, before);
        AssertRuntimeIdentitiesAreConserved(game);

        for (int tick = 0; tick < 80 && game.State == LevelState.Playing; tick++)
        {
            foreach (Egg egg in game.Eggs)
            {
                egg.BeginApproach();
                for (int approachTick = 0; approachTick < 40; approachTick++)
                    egg.Update(level);
            }
            foreach (Enemy enemy in game.Flippers.Cast<Enemy>().Concat(game.Tankers))
                if (enemy.PendingZapTicks == 0)
                    Assert.True(enemy.TryZap(1));
            game.RunOneStep();
        }

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Equal(0, level.ReserveEggs);
        Assert.Equal(0, level.Level3CargoReserveEggs);
        Assert.False(ledger.HasLatentCargo);
        Assert.False(ledger.HasPendingShotActivation);
        Assert.Equal(before.TotalIdentityCount, ledger.TotalIdentityCount);
        AssertRuntimeIdentitiesAreConserved(game);
        Assert.Equal(0, random.Remaining);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void OrdinaryCompletionRejectsTheFinalMissingRepresentation(bool cargo)
    {
        TempestGame game = new(LevelConfiguration.Level3Mixed,
            random: new InclusiveRangeScript(Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) })));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int id = 1; id < ledger.OriginalCount; id++)
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }
        if (cargo)
        {
            ledger.CommitOriginalAsTanker(0);
            ledger.MaterializeLiveCargo(0);
            ledger.MarkOriginalDestroyed(0);
            ledger.MarkCargoDestroyed(1);
        }
        game.CurrentLevel.ReturnSurvivorsToReserve();
        Assert.Equal(cargo ? Level3OriginalState.Destroyed : Level3OriginalState.Egg,
            ledger.GetOriginalState(0));
        Assert.Equal(cargo ? Array.Empty<int>() : new[] { 0 }, ledger.UnresolvedOriginalIds);
        Assert.Equal(cargo ? new[] { 0 } : Array.Empty<int>(), ledger.UnresolvedCargoIds);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.CurrentLevel.Level3CargoReserveEggs);

        InvalidOperationException error = Assert.Throws<InvalidOperationException>(
            () => game.RunOneStep());

        Assert.Contains("runtime representation mismatch", error.Message);
        Assert.Contains(cargo ? "Cargo, Id = 0" : "Original, Id = 0", error.Message);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.IsExiting);
        Assert.False(game.LevelComplete);
    }

    [Fact]
    public void OrdinaryCompletionWaitsForShotPendingCargoThenFinishesLevelThreeWithBullets()
    {
        (TempestGame game, InclusiveRangeScript random) = CreateTankerGame(
            canonicalLevel3: true, (0, 100, 100));
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        Tanker tanker = Assert.Single(game.Tankers);
        for (int id = 1; id < ledger.OriginalCount; id++)
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }
        level.ReturnSurvivorsToReserve();
        Assert.Empty(game.Eggs);
        Assert.Equal(0, level.ReserveEggs);
        Assert.Equal(new[] { tanker.OriginalSlotId }, ledger.UnresolvedOriginalIds);
        Assert.True(ledger.HasLatentCargo);
        AssertRuntimeIdentitiesAreConserved(game);
        int totalIdentityCount = ledger.TotalIdentityCount;

        for (int tick = 0; tick < 80; tick++)
            game.RunOneStep();
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(ledger.HasLatentCargo);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);

        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex,
            level.PlayableLaneCount);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;

        // The ordinary completion check runs after this shot removes the last original,
        // but before its cargo leaves pending activation and joins the active Flipper list.
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.IsExiting);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(game.Tankers);
        Assert.Equal(new[] { 0, 1 }, ledger.UnresolvedCargoIds.Order());
        Assert.Equal(new[] { 0, 1 }, game.Flippers.Select(child => child.CargoUnitId).Order());
        Assert.All(game.Flippers, child =>
        {
            Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(child.CargoUnitId));
            Assert.Equal(tanker.Height, child.Height);
            Assert.Equal(-1, child.OriginalSlotId);
            Assert.Equal(0, child.PendingZapTicks);
        });
        Assert.False(ledger.HasLatentCargo);
        Assert.False(ledger.HasPendingShotActivation);
        Assert.Equal(200, game.Score.Score);
        Assert.Contains(bullet, game.EnemyBullets);
        AssertRuntimeIdentitiesAreConserved(game);

        Flipper[] children = game.Flippers.ToArray();
        Assert.True(children[0].TryZap(1));
        MovePlayerToSafeLane(game);
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(new[] { children[1].CargoUnitId }, ledger.UnresolvedCargoIds);
        Assert.Contains(bullet, game.EnemyBullets);
        AssertRuntimeIdentitiesAreConserved(game);

        Assert.True(children[1].TryZap(1));
        MovePlayerToSafeLane(game);
        Assert.Contains(bullet, game.EnemyBullets);
        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Equal(0, level.ReserveEggs);
        Assert.Equal(0, level.Level3CargoReserveEggs);
        Assert.False(ledger.HasLatentCargo);
        Assert.False(ledger.HasPendingShotActivation);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.All(Enumerable.Range(0, ledger.OriginalCount), id =>
            Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(id)));
        Assert.All(new[] { 0, 1 }, id =>
            Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(id)));
        Assert.Equal(500, game.Score.Score);
        AssertRuntimeIdentitiesAreConserved(game);

        int lives = game.Player.Lives;
        for (int tick = 0; tick < 200 && game.State != LevelState.Complete; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Complete, game.State);
        Assert.True(game.LevelComplete);
        Assert.Same(level, game.CurrentLevel);
        Assert.Same(ledger, level.Level3Ledger);
        Assert.Same(LevelConfiguration.Level3Mixed, level.Configuration);
        Assert.Equal(500, game.Score.Score);
        Assert.Equal(lives, game.Player.Lives);
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Equal(LevelState.Complete, game.State);
        Assert.Same(level, game.CurrentLevel);
        Assert.Empty(game.Shots);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void ExactAllRimClearTravelsToTerminalLevelThreeAndReplayCreatesAFreshLedger()
    {
        (TempestGame game, Level previous, Level3WaveLedger ledger,
            Flipper survivor, InclusiveRangeScript random) = CreateSingleRimFlipperGame();

        while (survivor.Height < Level.Depth)
        {
            MovePlayerToSafeLane(game, survivor);
            survivor.Update(game.CurrentLevel, game.Player.EdgeIndex);
        }
        Assert.Equal(Level.Depth, survivor.Height);
        Assert.True(game.SuperZapper.TryActivate([survivor], survivor.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));

        game.RunOneStep(new PlayerInput(true, 0, false, false));

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(0, game.Score.Score);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Eggs);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(Level3OriginalState.RimCleared, ledger.GetOriginalState(0));
        Assert.Equal(new[] { 0 }, Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.RimCleared));
        Assert.Equal(new[] { 1 }, Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Escaped));
        Assert.Equal(Enumerable.Range(2, ledger.OriginalCount - 2),
            Enumerable.Range(0, ledger.OriginalCount)
                .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Destroyed));
        int[] escapedCarrierCargo = ledger.GetCargoIdsForOriginal(1).ToArray();
        Assert.Equal(new[] { 2, 3 }, escapedCarrierCargo);
        Assert.Equal(escapedCarrierCargo, ledger.GetCargoIdsForOriginal(1)
            .Where(id => ledger.GetCargoState(id) == Level3CargoState.Destroyed));
        Assert.Equal(Array.Empty<int>(), escapedCarrierCargo
            .Where(id => ledger.GetCargoState(id) == Level3CargoState.RimCleared));
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(3, survivor.PendingZapTicks);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        AssertRuntimeIdentitiesAreConserved(game);

        for (int tick = 0; tick < 5; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(Level3OriginalState.RimCleared, ledger.GetOriginalState(0));
        Assert.Equal(new[] { 1 }, Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Escaped));
        Assert.Equal(Enumerable.Range(2, ledger.OriginalCount - 2),
            Enumerable.Range(0, ledger.OriginalCount)
                .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Destroyed));

        for (int tick = 0; tick < 200 && game.State != LevelState.Complete; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Complete, game.State);
        Assert.Same(previous, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level4Candidate,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));

        game.RunOneStep(restartPressed: true);

        Assert.NotSame(previous, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(15, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(0, game.CurrentLevel.Level3Ledger.CargoCount);
        Assert.Equal(0, random.Remaining);
    }

    [Fact]
    public void PlayerCollisionAtTheRimPrecedesLevelThreeAllRimClear()
    {
        (TempestGame game, _, Level3WaveLedger ledger,
            Flipper survivor, _) = CreateSingleRimFlipperGame();
        for (int tick = 0; survivor.Height < Level.Depth - 2 && tick < 1000; tick++)
            survivor.Update(game.CurrentLevel, playerEdge: -1);

        Assert.Equal(Level.Depth - 2, survivor.Height);
        game.Player.Move(survivor.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount);

        for (int tick = 0; tick < 10 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(Level3OriginalState.ActiveFlipper,
            ledger.GetOriginalState(survivor.OriginalSlotId));
        Assert.Equal(Array.Empty<int>(), Enumerable.Range(0, ledger.OriginalCount)
            .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.RimCleared));
        Assert.Contains(survivor, game.Flippers);
    }

    private static (
        TempestGame Game,
        Level Previous,
        Level3WaveLedger Ledger,
        Flipper Survivor,
        InclusiveRangeScript Random) CreateSingleRimFlipperGame()
    {
        InclusiveRangeScript random = new(
        [
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            (0, 15, 0),
            (0, 1, 0),
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 1), (0, 1, 1) })
        ]);
        TempestGame game = new(LevelConfiguration.Level3Mixed, random: random);
        Level previous = game.CurrentLevel;
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        ledger.CommitOriginalAsFlipper(0);
        for (int originalId = 1; originalId < ledger.OriginalCount; originalId++)
        {
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(originalId);
            if (originalId == 1)
            {
                ledger.CommitOriginalAsTanker(originalId);
                IReadOnlyList<int> cargoIds = ledger.MaterializeLiveCargo(originalId);
                foreach (int cargoId in cargoIds)
                    ledger.MarkCargoDestroyed(cargoId);
                ledger.MarkOriginalEscaped(originalId);
            }
            else
            {
                ledger.CommitOriginalAsFlipper(originalId);
                ledger.MarkOriginalDestroyed(originalId);
            }
        }
        game.CurrentLevel.ResetAfterDeath();

        Flipper? survivor = null;
        for (int tick = 0; tick < 1000 && game.State == LevelState.Playing && survivor is null; tick++)
        {
            game.RunOneStep();
            survivor = game.Flippers.SingleOrDefault();
        }

        return (game, previous, ledger,
            survivor ?? throw new InvalidOperationException("The Level 3 rim-clear fixture did not hatch its Flipper."),
            random);
    }

    private static Flipper[] ExpireTankerBySuperZapper(TempestGame game, Tanker tanker)
    {
        Assert.True(game.SuperZapper.TryActivate([tanker], tanker.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));
        Assert.Equal(4, tanker.PendingZapTicks);
        for (int tick = 0; tick < 4; tick++)
        {
            MovePlayerToSafeLane(game);
            game.RunOneStep();
        }

        Assert.DoesNotContain(tanker, game.Tankers);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Flipper[] children = game.Flippers
            .Where(flipper => flipper.IsTankerCargo &&
                ledger.GetCargoOwner(flipper.CargoUnitId) == tanker.OriginalSlotId)
            .OrderBy(flipper => flipper.CargoUnitId)
            .ToArray();
        Assert.Equal(Tanker.CarriedFlipperCount, children.Length);
        Assert.All(children, child => Assert.Equal(8, child.PendingZapTicks));
        return children;
    }

    private static LedgerSnapshot CaptureLedgerSnapshot(Level3WaveLedger ledger)
    {
        int[] cargoIds = Enumerable.Range(0, ledger.OriginalCount)
            .SelectMany(ledger.GetCargoIdsForOriginal)
            .Order()
            .ToArray();
        return new LedgerSnapshot(
            ledger.CargoCount,
            ledger.TotalIdentityCount,
            cargoIds,
            cargoIds.Select(id => (Id: id, Owner: ledger.GetCargoOwner(id),
                Ordinal: ledger.GetCargoOrdinal(id))).ToArray(),
            ledger.UnresolvedOriginalIds.Order().ToArray(),
            Enumerable.Range(0, ledger.OriginalCount)
                .Select(id => new KeyValuePair<int, Level3OriginalType>(
                    id, ledger.GetOriginalType(id)))
                .Where(pair => pair.Value != Level3OriginalType.Unselected)
                .ToArray(),
            cargoIds.Where(id => ledger.GetCargoState(id) == Level3CargoState.Latent)
                .Select(id => (Owner: ledger.GetCargoOwner(id),
                    Ordinal: ledger.GetCargoOrdinal(id)))
                .OrderBy(pair => pair.Owner)
                .ThenBy(pair => pair.Ordinal)
                .ToArray(),
            ledger.UnresolvedCargoIds
                .Where(id => ledger.GetCargoState(id) != Level3CargoState.Latent)
                .Order()
                .ToArray(),
            Enumerable.Range(0, ledger.OriginalCount)
                .Select(id => new KeyValuePair<int, Level3OriginalState>(
                    id, ledger.GetOriginalState(id)))
                .Where(pair => pair.Value is Level3OriginalState.Destroyed or
                    Level3OriginalState.Escaped or Level3OriginalState.RimCleared)
                .ToArray(),
            cargoIds.Select(id => new KeyValuePair<int, Level3CargoState>(
                    id, ledger.GetCargoState(id)))
                .Where(pair => pair.Value is Level3CargoState.Destroyed or
                    Level3CargoState.RimCleared)
                .ToArray());
    }

    private static void AssertLedgerSnapshotConserved(
        Level3WaveLedger ledger,
        LedgerSnapshot expected)
    {
        LedgerSnapshot actual = CaptureLedgerSnapshot(ledger);
        Assert.Equal(expected.CargoCount, actual.CargoCount);
        Assert.Equal(expected.TotalIdentityCount, actual.TotalIdentityCount);
        Assert.Equal(expected.CargoIds, actual.CargoIds);
        Assert.Equal(expected.CargoOwnership, actual.CargoOwnership);
        Assert.Equal(expected.UnresolvedOriginalIds, actual.UnresolvedOriginalIds);
        Assert.Equal(expected.SelectedTypes, actual.SelectedTypes);
        Assert.Equal(expected.LatentCargo, actual.LatentCargo);
        Assert.Equal(expected.UnresolvedMaterializedCargoIds,
            actual.UnresolvedMaterializedCargoIds);
        Assert.Equal(expected.TerminalOriginals, actual.TerminalOriginals);
        Assert.Equal(expected.TerminalCargo, actual.TerminalCargo);
    }

    private static void AssertRuntimeIdentitiesAreConserved(TempestGame game)
    {
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        List<Level3WaveRepresentation> represented = [];
        represented.AddRange(ledger.OriginalIdsInReserve()
            .Select(id => new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Original, id),
                Level3RepresentationLocation.Reserve)));
        represented.AddRange(ledger.CargoIdsInReentryReserve()
            .Select(id => new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Cargo, id),
                Level3RepresentationLocation.Reserve)));

        foreach (Egg egg in game.Eggs)
        {
            if (egg.CargoUnitId >= 0)
            {
                Assert.NotEqual(Level3CargoState.Latent,
                    ledger.GetCargoState(egg.CargoUnitId));
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Cargo, egg.CargoUnitId),
                    Level3RepresentationLocation.Egg));
            }
            else
            {
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Original, egg.OriginalSlotId),
                    Level3RepresentationLocation.Egg));
            }
        }

        represented.AddRange(game.Flippers.Select(flipper =>
            new Level3WaveRepresentation(
                flipper.IsTankerCargo
                    ? new Level3Identity(Level3IdentityKind.Cargo, flipper.CargoUnitId)
                    : new Level3Identity(Level3IdentityKind.Original, flipper.OriginalSlotId),
                Level3RepresentationLocation.ActiveEnemy)));
        represented.AddRange(game.Tankers.Select(tanker =>
            new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Original, tanker.OriginalSlotId),
                Level3RepresentationLocation.ActiveEnemy)));
        ledger.ValidateRuntimeRepresentations(represented);
    }

    private sealed record LedgerSnapshot(
        int CargoCount,
        int TotalIdentityCount,
        int[] CargoIds,
        (int Id, int Owner, int Ordinal)[] CargoOwnership,
        int[] UnresolvedOriginalIds,
        KeyValuePair<int, Level3OriginalType>[] SelectedTypes,
        (int Owner, int Ordinal)[] LatentCargo,
        int[] UnresolvedMaterializedCargoIds,
        KeyValuePair<int, Level3OriginalState>[] TerminalOriginals,
        KeyValuePair<int, Level3CargoState>[] TerminalCargo);

    private static void MovePlayerToSafeLane(TempestGame game, Flipper flipper)
    {
        int laneCount = game.CurrentLevel.PlayableLaneCount;
        int[] unsafeLanes = flipper.State == FlipperState.Flipping
            ? [flipper.EdgeIndex, flipper.DestinationEdge,
                (flipper.EdgeIndex + laneCount / 2) % laneCount]
            : [flipper.EdgeIndex, (flipper.EdgeIndex + laneCount / 2) % laneCount];
        int safeLane = Enumerable.Range(0, laneCount)
            .First(lane => !unsafeLanes.Contains(lane));
        int movement = (safeLane - game.Player.EdgeIndex + laneCount) % laneCount;
        if (movement > laneCount / 2)
            movement -= laneCount;
        game.Player.Move(movement, laneCount);
    }

    private static TempestGame CreateArtificialMultiCarrierGame(bool includeEnemyBullets = false)
    {
        BoundedCompositionRandom random = new(maximumCalls: 5000, variedLanes: true);
        // This copied configuration is an artificial multi-carrier lifecycle fixture, not the
        // canonical production Level 3 admission diagnostic.
        LevelConfiguration fixtureConfiguration = LevelConfiguration.Level3Mixed with { };
        TempestGame game = new(
            spikerDevelopmentWave: false,
            random: random,
            enemyBulletsTest: includeEnemyBullets,
            startingConfiguration: fixtureConfiguration);
        for (int tick = 0; tick < 200; tick++)
        {
            if (includeEnemyBullets)
                MovePlayerToSafeLane(game);
            game.RunOneStep();
        }

        Assert.Equal(6, game.Tankers.Count);
        Assert.True(random.Consumed > 0);
        return game;
    }

    private static void MovePlayerToSafeLane(TempestGame game)
    {
        int laneCount = game.CurrentLevel.PlayableLaneCount;
        int safeLane = Enumerable.Range(0, laneCount)
            .Where(lane =>
                !game.Eggs.Any(egg => egg.EdgeIndex == lane) &&
                !game.Tankers.Any(tanker => tanker.EdgeIndex == lane) &&
                !game.Flippers.Any(flipper => flipper.EdgeIndex == lane ||
                    flipper.State == FlipperState.Flipping &&
                    flipper.DestinationEdge == lane) &&
                !game.EnemyBullets.Any(bullet => bullet.EdgeIndex == lane))
            .DefaultIfEmpty(-1)
            .First();
        Assert.True(safeLane >= 0, "The Level 3 setup must have a lane clear of active hazards.");

        int movement = safeLane - game.Player.EdgeIndex;
        if (movement > laneCount / 2)
            movement -= laneCount;
        else if (movement < -laneCount / 2)
            movement += laneCount;
        game.Player.Move(movement, laneCount);
    }

    private static Tanker FindTankerSafeForShot(TempestGame game)
    {
        float shotHeight = game.Player.Depth - 6;
        return game.Tankers.First(tanker =>
        {
            Shot probe = new() { EdgeIndex = tanker.EdgeIndex, Height = shotHeight };
        return game.Tankers.Count(candidate => candidate.EdgeIndex == tanker.EdgeIndex) == 1 &&
            !game.Tankers.Any(candidate => candidate.IsHitBy(probe)) &&
                !game.Flippers.Any(candidate => candidate.IsHitBy(probe)) &&
                !game.EnemyBullets.Any(candidate => candidate.IsHitBy(probe)) &&
                !game.Flippers.Any(candidate => candidate.EdgeIndex == tanker.EdgeIndex ||
                    candidate.State == FlipperState.Flipping &&
                    candidate.DestinationEdge == tanker.EdgeIndex) &&
                !game.EnemyBullets.Any(bullet =>
                    bullet.EdgeIndex == tanker.EdgeIndex &&
                    game.Player.Depth - (bullet.Height + 3) < 8);
        });
    }

    private static Level3Identity[] GetEggIdentities(IEnumerable<Egg> eggs) =>
        eggs.Select(egg => egg.CargoUnitId >= 0
                ? new Level3Identity(Level3IdentityKind.Cargo, egg.CargoUnitId)
                : new Level3Identity(Level3IdentityKind.Original, egg.OriginalSlotId))
            .ToArray();

    private static void ForceFlipperIntoPlayer(TempestGame game, Flipper flipper)
    {
        int ticks = 0;
        while (!flipper.CompletedIntoPlayer && ticks++ < 2000)
        {
            if (flipper.State == FlipperState.Flipping)
                game.Player.Move(flipper.DestinationEdge - game.Player.EdgeIndex, 16);
            flipper.Update(game.CurrentLevel, game.Player.EdgeIndex, (_, _) => 0);
        }

        Assert.True(flipper.CompletedIntoPlayer);
    }

    private static (TempestGame Game, InclusiveRangeScript Random) CreateTankerGame(
        params (int Minimum, int Maximum, int Value)[] followupDecisions) =>
        CreateTankerGame(canonicalLevel3: false, followupDecisions);

    private static (TempestGame Game, InclusiveRangeScript Random) CreateTankerGame(
        bool canonicalLevel3,
        params (int Minimum, int Maximum, int Value)[] followupDecisions)
    {
        (int Minimum, int Maximum, int Value)[] initialDecisions =
        [
            .. Enumerable.Range(0, 4)
                .SelectMany(_ => new[] { (0, 15, 0), (0, 1, 0) }),
            (0, 100, 96),
            (0, 100, 96),
            .. (canonicalLevel3
                ? Array.Empty<(int Minimum, int Maximum, int Value)>()
                : new[] { (-6, 4, 4) }),
            (0, 2, 0),
            (1, 2, 1),
            (1, 64, 64),
            .. followupDecisions
        ];
        InclusiveRangeScript random = new(initialDecisions);
        TempestGame game = new(
            canonicalLevel3 ? LevelConfiguration.Level3Mixed : MultiCarrierFixtureConfiguration(),
            random: random);
        for (int tick = 0; tick < 40; tick++)
            game.RunOneStep();

        Assert.Single(game.Tankers);
        return (game, random);
    }

    private static LevelConfiguration MultiCarrierFixtureConfiguration() =>
        LevelConfiguration.Level3Mixed with { };

    private sealed class InclusiveRangeScript(
        IEnumerable<(int Minimum, int Maximum, int Value)> decisions) : Random
    {
        private readonly Queue<(int Minimum, int Maximum, int Value)> remaining = new(decisions);

        public int Remaining => remaining.Count;

        public int NextInclusive(int minimum, int maximum)
        {
            if (remaining.Count == 0)
                throw new InvalidOperationException(
                    $"Unexpected random request [{minimum}, {maximum}].");

            (int expectedMinimum, int expectedMaximum, int value) = remaining.Dequeue();
            Assert.Equal(expectedMinimum, minimum);
            Assert.Equal(expectedMaximum, maximum);
            Assert.InRange(value, minimum, maximum);
            return value;
        }

        public override int Next(int minValue, int maxValue)
        {
            if (remaining.Count == 0)
                throw new InvalidOperationException(
                    $"Unexpected random request [{minValue}, {maxValue}).");

            (int expectedMinimum, int expectedMaximum, int value) = remaining.Dequeue();
            Assert.Equal(expectedMinimum, minValue);
            Assert.Equal(expectedMaximum + 1, maxValue);
            Assert.InRange(value, minValue, maxValue - 1);
            return value;
        }
    }

    private sealed class BoundedCompositionRandom(int maximumCalls, bool variedLanes = false) : Random
    {
        private int remaining = maximumCalls;
        private int nextLane;

        public int Consumed { get; private set; }

        public override int Next(int minValue, int maxValue)
        {
            if (remaining-- == 0)
                throw new InvalidOperationException("The finite Level 3 composition script was exhausted.");
            Consumed++;

            return (minValue, maxValue) switch
            {
                (0, 16) => variedLanes ? nextLane++ % 16 : 0,
                (0, 2) => 0,
                (0, 101) => 96,
                (-6, 5) => 4,
                (-6, 4) => 1,
                (0, 3) => 0,
                (1, 3) => 1,
                (1, 65) => 64,
                (1, 5) => 4,
                (1, 4) => 1,
                _ => throw new InvalidOperationException(
                    $"Unexpected production Level 3 random range [{minValue}, {maxValue}).")
            };
        }
    }
}
