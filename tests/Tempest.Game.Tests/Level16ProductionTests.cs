using System.Reflection;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level16ProductionTests
{
    [Fact]
    public void ProductionConfigurationKeepsRomValuesSeparateFromProjectTranslations()
    {
        LevelConfiguration configuration = LevelConfiguration.Level16Production;

        Assert.Same(configuration, LevelProgression.GetProductionLevel(16));
        Assert.Equal(15, configuration.ProgressionIndex);
        Assert.Equal(16, configuration.DisplayedLevel);
        Assert.Equal(11, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(16));
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.True(configuration.UsesLevel16RomWavePolicy);
        Assert.True(configuration.UsesGenericRomDeathReconstruction);
        Assert.False(configuration.AllowsOrdinaryFlipperSelection);
        Assert.Equal(BelowRimFlipperProgram.Level16Repeating24,
            configuration.BelowRimFlipperProgram);
        Assert.Equal(27, configuration.EnemySupply);
        Assert.Equal(3, configuration.MaximumActiveEnemyShots);
        Assert.Equal(0x23, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(3, Level16WaveSelection.DefaultMaximumActiveEnemyShots);
        Assert.Equal(Level16WaveSelection.StartingSpikeHeightRaw, (byte)0xA0);
        Assert.Equal(142.85715f, LevelConfiguration.Level16StartingSpikeHeight, precision: 4);
        Assert.Equal(LevelConfiguration.Level16StartingSpikeHeight,
            configuration.StartingSpikeHeight);
        Assert.Equal(0x24, Level16WaveSelection.FlipperMovementProgram);
        Assert.Equal(0xA0, Level16WaveSelection.FlipperSpeedRaw);
        Assert.Equal(0x00, Level16WaveSelection.SpikerSpeedRaw);
        Assert.Equal(0xC0, Level16WaveSelection.FuseballMovementProbabilityRaw);
        Assert.Equal(0x00, Level16WaveSelection.FuseballMovementFlagRaw);
        Assert.Equal(0xC0, Level16WaveSelection.EnemyShotSpeedRaw);
    }

    [Fact]
    public void LevelSixteenSupplyAndShotSlotsRepresentDifficultyAndIndexSemantics()
    {
        Assert.Equal((27, 27, 30),
            (Level16WaveSelection.OrdinaryEnemySupply,
             Level16WaveSelection.EasyDifficultyEnemySupply,
             Level16WaveSelection.HardDifficultyEnemySupply));
        Assert.Equal(27,
            Level16WaveSelection.EnemySupplyForDifficulty(Level16Difficulty.Default));
        Assert.Equal(27,
            Level16WaveSelection.EnemySupplyForDifficulty(Level16Difficulty.Easy));
        Assert.Equal(30,
            Level16WaveSelection.EnemySupplyForDifficulty(Level16Difficulty.Hard));
        Assert.Equal((0x02, 3), (Level16WaveSelection.DefaultEnemyShotLastSlotIndex,
            Level16WaveSelection.DefaultMaximumActiveEnemyShots));
        Assert.Equal((0x01, 2), (Level16WaveSelection.EasyEnemyShotLastSlotIndex,
            Level16WaveSelection.EasyMaximumActiveEnemyShots));
        Assert.Equal((0x03, 4), (Level16WaveSelection.HardEnemyShotLastSlotIndex,
            Level16WaveSelection.HardMaximumActiveEnemyShots));
        Assert.Equal(2,
            Level16WaveSelection.MaximumActiveEnemyShotsForDifficulty(Level16Difficulty.Easy));
        Assert.Equal(3,
            Level16WaveSelection.MaximumActiveEnemyShotsForDifficulty(Level16Difficulty.Default));
        Assert.Equal(4,
            Level16WaveSelection.MaximumActiveEnemyShotsForDifficulty(Level16Difficulty.Hard));
        Assert.Equal(2, LevelConfiguration.Level14Production.MaximumActiveEnemyShots);
    }

    [Fact]
    public void SpeciesPickerEnforcesMinimaWithoutMakingOrdinaryFlippersEligible()
    {
        Assert.Equal((0, 5), (Level16WaveSelection.MinimumFlippers,
            Level16WaveSelection.MaximumFlippers));
        Assert.Equal((0, 0), (Level16WaveSelection.MinimumPulsars,
            Level16WaveSelection.MaximumPulsars));
        Assert.Equal((1, 2), (Level16WaveSelection.MinimumTankers,
            Level16WaveSelection.MaximumTankers));
        Assert.Equal((2, 3), (Level16WaveSelection.MinimumSpikers,
            Level16WaveSelection.MaximumSpikers));
        Assert.Equal((1, 1), (Level16WaveSelection.MinimumFuseballs,
            Level16WaveSelection.MaximumFuseballs));

        Level3OriginalType[] minimumTypes = Enumerable.Range(0, 3)
            .Select(choice => Level16WaveSelection.SelectOriginalType(
                (_, _) => choice, activeTankers: 0, activeSpikers: 0,
                activeFuseballs: 0, activeEnemyUnits: 0,
                activeFlipperUnitsIncludingLatentCargo: 0)!.Value)
            .ToArray();
        Assert.Equal(new[]
        {
            Level3OriginalType.Tanker,
            Level3OriginalType.Spiker,
            Level3OriginalType.Fuseball
        }, minimumTypes);
        Assert.DoesNotContain(Level3OriginalType.Flipper, minimumTypes);

        Level3OriginalType[] availableBeforeTankersAndSpikersMinimums = Enumerable.Range(0, 2)
            .Select(choice => Level16WaveSelection.SelectOriginalType(
                (_, _) => choice, activeTankers: 0, activeSpikers: 0,
                activeFuseballs: 1, activeEnemyUnits: 1,
                activeFlipperUnitsIncludingLatentCargo: 0)!.Value)
            .ToArray();
        Assert.Equal(new[] { Level3OriginalType.Tanker, Level3OriginalType.Spiker },
            availableBeforeTankersAndSpikersMinimums);
        Assert.Null(Level16WaveSelection.SelectOriginalType(
            (_, _) => 0, activeTankers: 2, activeSpikers: 3,
            activeFuseballs: 1, activeEnemyUnits: 6,
            activeFlipperUnitsIncludingLatentCargo: 4));
        Assert.Equal(Level3OriginalType.Spiker, Level16WaveSelection.SelectOriginalType(
            (_, _) => 0, activeTankers: 0, activeSpikers: 0,
            activeFuseballs: 0, activeEnemyUnits: 0,
            activeFlipperUnitsIncludingLatentCargo: 4));
    }

    [Fact]
    public void PhysicalAdmissionReservesSpaceForCarrierAndTerminalSpikerChildren()
    {
        Assert.Equal(7, Level16WaveSelection.MaximumPhysicalEnemySlots);
        Assert.True(Level16WaveSelection.CanAdmitOriginalType(
            Level3OriginalType.Tanker, activeEnemyUnits: 5));
        Assert.False(Level16WaveSelection.CanAdmitOriginalType(
            Level3OriginalType.Tanker, activeEnemyUnits: 6));
        Assert.False(Level16WaveSelection.CanAdmitOriginalType(
            Level3OriginalType.Spiker, activeEnemyUnits: 7));
        Assert.True(Level16WaveSelection.CanAdmitCargoFlipper(activeEnemyUnits: 6));
        Assert.False(Level16WaveSelection.CanAdmitCargoFlipper(activeEnemyUnits: 7));
    }

    [Fact]
    public void SharedRepeating24RoutePerformsTwoFlipsThenThreeInReverse()
    {
        Level level = new(LevelConfiguration.Level16Production);
        Flipper flipper = new(edge: 0);
        List<int> completedDirections = [];

        for (int tick = 0; tick < 300 && completedDirections.Count < 6; tick++)
        {
            int previousLane = flipper.EdgeIndex;
            flipper.Update(level, playerEdge: 15);
            if (flipper.EdgeIndex != previousLane)
                completedDirections.Add(flipper.Direction);
        }

        Assert.Equal(new[] { 1, 1, -1, -1, -1, 1 }, completedDirections);
    }

    [Fact]
    public void TankerReleasesTwoIndependentlyOwnedAdjacentFlippersOnOppositeDirections()
    {
        Level level = new(LevelConfiguration.Level16Production);
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 7);
        TankerReleaseResult result = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);

        Assert.Equal(2, result.Cargo.Count);
        Assert.Equal(new[] { 15, 1 }, result.Cargo.Select(child => child.SourceLane));
        Assert.Equal(new[] { 1, -1 }, result.Cargo.Select(child => child.Direction));
        Assert.Equal(14, result.Cargo[0].DestinationLane);
        Assert.Equal(2, result.Cargo[1].DestinationLane);
        Assert.Equal(TankerCargoMovementProgram.Repeating24,
            tanker.GetCargoMovementProgram(LevelConfiguration.Level16Production,
                TankerReleaseKind.Natural));

        Flipper first = Flipper.CreateTankerCargo(level, result.Cargo[0], cargoUnitId: 20,
            tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);
        Flipper second = Flipper.CreateTankerCargo(level, result.Cargo[1], cargoUnitId: 21,
            tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);
        Assert.Equal(20, first.CargoUnitId);
        Assert.Equal(21, second.CargoUnitId);
        Assert.Equal(TankerCargoMovementProgram.Repeating24, first.CargoMovementProgram);
        Assert.Equal(TankerCargoMovementProgram.Repeating24, second.CargoMovementProgram);
    }

    [Fact]
    public void ProductionTankerReleaseMaterializesBothLedgerCargoIdentities()
    {
        TempestGame game = new(startingProductionLevel: 16);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int originalId = 0;
        if (ledger.GetOriginalState(originalId) == Level3OriginalState.Reserve)
            ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsTanker(originalId);
        RemoveEggs(game.CurrentLevel);

        Tanker tanker = new(edgeIndex: 0, originalSlotId: originalId);
        GetGameList<Tanker>(game, "tankers").Add(tanker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(tanker);
        GetGameList<Tanker>(game, "tankers").Remove(tanker);
        GetGameList<object>(game, "developmentCollisionOrder").Remove(tanker);
        MethodInfo release = typeof(TempestGame).GetMethod(
            "ReleaseTankerCargo", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Tanker cargo release method was not found.");
        release.Invoke(game, [tanker, TankerReleaseKind.Natural]);

        Flipper[] children = game.Flippers.ToArray();
        int[] cargoIds = ledger.GetCargoIdsForOriginal(originalId).ToArray();
        Assert.Equal(2, children.Length);
        Assert.Equal(cargoIds, children.Select(child => child.CargoUnitId).Order());
        Assert.All(children, child =>
        {
            Assert.True(child.IsTankerCargo);
            Assert.Equal(TankerCargoMovementProgram.Repeating24, child.CargoMovementProgram);
            Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(child.CargoUnitId));
        });
        Assert.Equal(1, children[0].Direction);
        Assert.Equal(-1, children[1].Direction);
        Assert.Equal(Level3OriginalState.Escaped, ledger.GetOriginalState(originalId));
        ledger.ValidateState();
    }

    [Fact]
    public void LevelSixteenTankerFiringHasNoFiniteAmmunitionLimit()
    {
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 1,
            bulletAmmunition: 0, bulletTimer: 0, unlimitedBulletAmmunition: true);
        Func<int, int, int> maximumRoll = (_, maximum) => maximum;

        TankerUpdateResult firstUpdate = tanker.UpdateForDevelopment(
            maximumRoll, out EnemyBullet? first,
            shotHoldoffTicks: LevelConfiguration.Level16Production.FlipperShotHoldoffTicks);
        Assert.Equal(TankerUpdateResult.Active, firstUpdate);
        Assert.NotNull(first);
        Assert.Equal(0, tanker.BulletAmmunition);

        EnemyBullet? next = null;
        for (int tick = 0; tick <= LevelConfiguration.Level16Production.FlipperShotHoldoffTicks;
             tick++)
            tanker.UpdateForDevelopment(maximumRoll, out next,
                shotHoldoffTicks: LevelConfiguration.Level16Production.FlipperShotHoldoffTicks);

        Assert.NotNull(next);
        Assert.Equal(0, tanker.BulletAmmunition);
    }

    [Fact]
    public void LevelSixteenSpikersUseTheSharedShotSlotsAndHoldoff()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Spiker spiker = new(edgeIndex: 7, speed: 3);
        spiker.Update(game.CurrentLevel, () => false);
        spiker.Update(game.CurrentLevel, () => false);
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);

        MethodInfo updateFiring = typeof(TempestGame).GetMethod(
            "UpdateLevel16SpikerFiring", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Level 16 Spiker firing phase was not found.");
        updateFiring.Invoke(game, null);

        Assert.Single(game.EnemyBullets);
        Assert.True(game.EnemyBullets[0].Height > 0);
        Assert.Equal(Level16WaveSelection.ShotHoldoffRaw, spiker.ShotDelayTicks);
    }

    [Fact]
    public void TerminalLevelSixteenSpikerConvertsThroughTheProductionLedger()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int spikerId = ledger.OriginalIdsInReserve().First();
        ledger.MarkOriginalEgg(spikerId);
        ledger.CommitOriginalAsSpiker(spikerId);
        ResolveOtherOriginals(ledger, spikerId);
        RemoveEggs(game.CurrentLevel);

        Spiker spiker = new(edgeIndex: 7, speed: 100, originalSlotId: spikerId);
        GetGameList<Spiker>(game, "spikers").Add(spiker);
        GetGameList<object>(game, "developmentCollisionOrder").Add(spiker);

        for (int tick = 0; tick < 100 &&
             !game.Tankers.Any(candidate => candidate.OriginalSlotId == spikerId); tick++)
            game.RunOneStep();

        Tanker converted = Assert.Single(game.Tankers,
            candidate => candidate.OriginalSlotId == spikerId);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(spikerId));
        Assert.Equal(Level3OriginalState.ActiveTanker, ledger.GetOriginalState(spikerId));
        Assert.True(ledger.IsConvertedSpikerTanker(spikerId));
        Assert.True(converted.UnlimitedBulletAmmunition);
        int[] cargo = ledger.GetCargoIdsForOriginal(spikerId).ToArray();
        Assert.Equal(2, cargo.Length);
        Assert.Equal(2, cargo.Distinct().Count());
        Assert.All(cargo, id =>
        {
            Assert.Equal(spikerId, ledger.GetCargoOwner(id));
            Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id));
        });
        Assert.True(Level16WaveSelection.ShouldConvertTerminalSpiker(
            ledger.PendingGenericOriginalCount));
        ledger.ValidateState();
    }

    [Fact]
    public void PendingTankerCargoPreventsLevelSixteenCompletionUntilResolved()
    {
        TempestGame game = new(startingProductionLevel: 16);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int tankerId = 0;
        if (ledger.GetOriginalState(tankerId) == Level3OriginalState.Reserve)
            ledger.MarkOriginalEgg(tankerId);
        ledger.CommitOriginalAsTanker(tankerId);
        int[] cargo = ledger.QueueCargoForShotActivation(tankerId).ToArray();
        ledger.MarkOriginalEscaped(tankerId);
        ResolveOtherOriginals(ledger, tankerId);
        RemoveEggs(game.CurrentLevel);

        Assert.False(CheckForLevelComplete(game));
        Assert.True(ledger.HasPendingShotActivation);
        foreach (int cargoId in cargo)
        {
            ledger.ActivateShotCargo(cargoId);
            ledger.MarkCargoDestroyed(cargoId);
        }
        Assert.True(CheckForLevelComplete(game));
    }

    [Fact]
    public void LevelSixteenDeathReconstructionConservesCommittedCargoAndFuseballIdentity()
    {
        Assert.True(LevelConfiguration.Level16Production.UsesGenericRomDeathReconstruction);
        Level3WaveLedger ledger = new(originalCount: 4);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsTanker(0);
        int[] activeCargo = ledger.MaterializeLiveCargo(0).ToArray();
        ledger.MarkOriginalEscaped(0);

        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsTanker(1);
        int[] pendingCargo = ledger.QueueCargoForShotActivation(1).ToArray();
        ledger.ActivateShotCargo(pendingCargo[0]);
        ledger.MarkOriginalDestroyed(1);

        ledger.MarkOriginalEgg(2);
        ledger.CommitOriginalAsFuseball(2);
        ledger.MarkOriginalDestroyed(2);

        ledger.MarkOriginalEgg(3);
        ledger.CommitOriginalAsSpiker(3);
        ledger.ConvertActiveSpikerToTanker(3);
        ledger.RebuildAfterDeath(
            LevelConfiguration.Level16Production.UsesGenericRomDeathReconstruction);

        Assert.Equal(8, ledger.OriginalCount);
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalType.Tanker, ledger.GetOriginalType(1));
        Assert.Equal(Level3OriginalType.Fuseball, ledger.GetOriginalType(2));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(3));
        Assert.All(activeCargo.Concat(pendingCargo), cargoId =>
            Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(cargoId)));
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(id)));
        Assert.Equal(5, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.GetCargoIdsForOriginal(3));
        ledger.ValidateState();
    }

    private static void ResolveOtherOriginals(Level3WaveLedger ledger, int preservedId)
    {
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            if (id == preservedId)
                continue;
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            if (ledger.GetOriginalState(id) == Level3OriginalState.Egg)
            {
                ledger.CommitOriginalAsFlipper(id);
                ledger.MarkOriginalDestroyed(id);
            }
        }
    }

    private static void RemoveEggs(Level level)
    {
        FieldInfo field = typeof(Level).GetField(
            "eggs", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Level Egg collection was not found.");
        ((List<Egg>)(field.GetValue(level)
            ?? throw new InvalidOperationException("The Level Egg collection is null."))).Clear();
    }

    private static List<T> GetGameList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"The game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException($"The game collection '{fieldName}' is null."));
    }

    private static bool CheckForLevelComplete(TempestGame game)
    {
        MethodInfo method = typeof(TempestGame).GetMethod(
            "CheckForLevelComplete", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The level-completion check was not found.");
        return (bool)(method.Invoke(game, null)
            ?? throw new InvalidOperationException("The level-completion check returned null."));
    }

}
