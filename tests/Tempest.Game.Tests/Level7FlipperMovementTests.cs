using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level7FlipperMovementTests
{
    [Fact]
    public void OrdinaryFlippersFollowArcadeObservedTwoThreeTwoTwoTwoRoute()
    {
        int[] expected = [6, 5, 6, 7, 8, 7, 6, 7, 8, 7, 6, 7, 8, 7, 6];
        Assert.Equal(expected, RunDestinations(
            initialLane: 7, playerLanes: [0, 15, 4, 11], initialDirection: 1));

        Assert.Equal(new[] { 15, 14, 15, 0, 1, 0, 15, 0, 1, 0, 15, 0, 1, 0, 15 },
            RunDestinations(initialLane: 0, playerLanes: [8], initialDirection: 1));
    }

    [Fact]
    public void FixedInitialDirectionDoesNotDependOnPlayerLaneOrRandomConsumption()
    {
        int[] first = RunDestinations(
            initialLane: 4, playerLanes: [4, 12, 0, 8], initialDirection: 1);
        int[] second = RunDestinations(
            initialLane: 4, playerLanes: [15, 1, 7, 13], initialDirection: 1);

        Assert.Equal(first, second);
        Assert.Equal(new[] { 3, 2, 3, 4, 5, 4, 3, 4, 5, 4, 3, 4, 5, 4, 3 }, first);
    }

    [Fact]
    public void OrdinaryFlippersKeepIndependentObservedRouteCounters()
    {
        Level level = CreateRouteLevel(climbSpeed: 0, turnSpeed: 360);
        Flipper first = new(edge: 7);
        Flipper second = new(edge: 7);

        Assert.Equal(new[] { 6, 5 }, RunDestinations(first, level, playerEdge: 0, count: 2));
        Assert.Equal(1, first.Direction);
        Assert.Equal(0, second.Direction);

        first.Update(level, playerEdge: 0, ThrowOnRandom);
        Assert.Equal(-1, first.Direction);
        Assert.Equal(0, second.Direction);

        Assert.Equal(new[] { 6, 5 }, RunDestinations(second, level, playerEdge: 15, count: 2));
        Assert.Equal(1, second.Direction);
        second.Update(level, playerEdge: 15, ThrowOnRandom);
        Assert.Equal(-1, second.Direction);
    }

    [Fact]
    public void StartedAndPartialPivotsDoNotReverseAndTheNextBoundaryStartsImmediately()
    {
        Level level = CreateRouteLevel(climbSpeed: 0, turnSpeed: 9);
        Flipper flipper = new(edge: 7, height: 0);
        flipper.Update(level, playerEdge: 0, ThrowOnRandom);
        flipper.Update(level, playerEdge: 0, ThrowOnRandom);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(1, flipper.Direction);
        Assert.Equal(7, flipper.EdgeIndex);
        float firstSweep = flipper.TotalTurnAngle;
        int partialUpdates = (int)MathF.Ceiling(firstSweep / 9) - 1;
        for (int tick = 0; tick < partialUpdates; tick++)
            flipper.Update(level, playerEdge: 0, ThrowOnRandom);

        Assert.Equal(7, flipper.EdgeIndex);
        Assert.Equal(1, flipper.Direction);
        Assert.Equal(FlipperState.Flipping, flipper.State);

        flipper.Update(level, playerEdge: 0, ThrowOnRandom);
        Assert.Equal(6, flipper.EdgeIndex);
        Assert.Equal(1, flipper.Direction);
        Assert.Equal(FlipperState.Climbing, flipper.State);

        flipper.Update(level, playerEdge: 0, ThrowOnRandom);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(1, flipper.Direction);
        FinishPivot(flipper, level);
        Assert.Equal(5, flipper.EdgeIndex);
        Assert.Equal(1, flipper.Direction);

        flipper.Update(level, playerEdge: 0, ThrowOnRandom);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(-1, flipper.Direction);
    }

    [Fact]
    public void RouteUsesLeadInAndAlongMotionOnlyOnLeadInAndPivotContinuation()
    {
        Level level = CreateRouteLevel(climbSpeed: 2, turnSpeed: 9);
        Flipper flipper = new(edge: 7, height: 0);

        flipper.Update(level, playerEdge: 0);
        Assert.Equal(2, flipper.Height);
        Assert.Equal(FlipperState.Climbing, flipper.State);

        flipper.Update(level, playerEdge: 0);
        Assert.Equal(2, flipper.Height);
        Assert.Equal(FlipperState.Flipping, flipper.State);

        flipper.Update(level, playerEdge: 0);
        Assert.Equal(4, flipper.Height);
        Assert.Equal(FlipperState.Flipping, flipper.State);
    }

    [Fact]
    public void GeometryStillControlsPivotDurationAndPreservesLongReflexTurnAtRim()
    {
        Level level = CreateRouteLevel(climbSpeed: 0, turnSpeed: 9);
        for (int lane = 0; lane < level.PlayableLaneCount; lane++)
        {
            Flipper flipper = new(edge: lane, height: 0);
            flipper.Update(level, playerEdge: 8);
            flipper.Update(level, playerEdge: 8);
            float sweep = flipper.TotalTurnAngle;
            int updates = 0;
            while (flipper.State == FlipperState.Flipping)
            {
                flipper.Update(level, playerEdge: 8);
                updates++;
            }
            Assert.Equal((int)MathF.Ceiling(sweep / 9), updates);
        }

        Level rimLevel = CreateRouteLevel(climbSpeed: 2, turnSpeed: 9);
        Flipper reflex = new(edge: 2, height: Level.Depth - 4);
        reflex.Update(rimLevel, playerEdge: 1);
        reflex.Update(rimLevel, playerEdge: 1);
        Assert.Equal(307.893f, reflex.TotalTurnAngle, precision: 2);

        int pivotUpdates = 0;
        while (reflex.State == FlipperState.Flipping && pivotUpdates < 40)
        {
            reflex.Update(rimLevel, playerEdge: 1);
            pivotUpdates++;
        }

        Assert.Equal(35, pivotUpdates);
        Assert.Equal(1, reflex.EdgeIndex);
        Assert.Equal(Level.Depth, reflex.Height);
        Assert.Equal(FlipperState.Pausing, reflex.State);
        Assert.True(reflex.CompletedIntoPlayer);

        for (int tick = 0; tick < 23; tick++)
            reflex.Update(rimLevel, playerEdge: 8);
        Assert.Equal(-1, reflex.RimPursuitDirection);
        Assert.Equal(-1, reflex.Direction);
    }

    [Fact]
    public void DeepCargoGetsIndependentOppositeRoutesAndNearRimCargoKeepsExistingMovement()
    {
        Level level = CreateRouteLevel(climbSpeed: 0, turnSpeed: 360);
        Tanker carrier = new(edgeIndex: 8, originalSlotId: 0);
        TankerReleaseResult release = carrier.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.PlayerShotDestruction);
        Assert.Equal(new[] { 1, -1 }, release.Cargo.Select(cargo => cargo.Direction));

        Flipper first = Flipper.CreateTankerCargo(
            level, release.Cargo[0], cargoUnitId: 10, useLevel7ProgrammedRoute: true);
        Flipper second = Flipper.CreateTankerCargo(
            level, release.Cargo[1], cargoUnitId: 11, useLevel7ProgrammedRoute: true);
        Assert.Equal(1, first.Direction);
        Assert.Equal(-1, second.Direction);

        int[] firstDestinations = RunCargoDestinations(first, level, playerEdge: 0, count: 5);
        int[] secondDestinations = RunCargoDestinations(second, level, playerEdge: 15, count: 5);
        Assert.Equal(new[] { 6, 5, 6, 7, 8 }, firstDestinations);
        Assert.Equal(new[] { 10, 11, 10, 9, 8 }, secondDestinations);

        Tanker nearRimCarrier = new(edgeIndex: 8, originalSlotId: 1);
        TankerCargoSpawn naturalSpawn = nearRimCarrier.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural).Cargo[0];
        Flipper nearRim = Flipper.CreateTankerCargo(level, naturalSpawn, cargoUnitId: 12);
        nearRim.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Pausing, nearRim.State);
        Assert.Equal(naturalSpawn.Direction, nearRim.Direction);

        Flipper reconstructed = Flipper.CreateFreshTankerCargo(
            edge: 4, cargoUnitId: 10, programmedDirection: 1);
        reconstructed.Update(level, playerEdge: 0);
        Assert.Equal(0, reconstructed.Height);
        Assert.Equal(1, reconstructed.Direction);
        Assert.Equal(FlipperState.Climbing, reconstructed.State);
    }

    [Fact]
    public void TankersRemainStraightLaneClimbersAndDeepCargoSelectionIsNarrow()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Tanker tanker = new(edgeIndex: 5, originalSlotId: 0);
        for (int tick = 0; tick < 20; tick++)
        {
            tanker.UpdateForDevelopment();
            Assert.Equal(5, tanker.EdgeIndex);
        }

        Tanker deep = new(edgeIndex: 5, originalSlotId: 1);
        while (deep.Height + Tanker.DevelopmentClimbSpeed <=
               LevelConfiguration.Level7DeepCargoMaximumHeight)
            deep.UpdateForDevelopment();
        Assert.True(deep.UsesLevel7ProgrammedCargo(
            LevelConfiguration.Level7Production, TankerReleaseKind.PlayerShotDestruction));
        Assert.False(deep.UsesLevel7ProgrammedCargo(
            LevelConfiguration.Level7Production, TankerReleaseKind.Natural));
        Assert.False(deep.UsesLevel7ProgrammedCargo(
            LevelConfiguration.Level7Production, TankerReleaseKind.SuperZapperDestruction));
        Assert.False(deep.UsesLevel7ProgrammedCargo(
            LevelConfiguration.Level6Production, TankerReleaseKind.PlayerShotDestruction));

        Tanker nearRim = new(edgeIndex: 5, originalSlotId: 2);
        TankerUpdateResult result = TankerUpdateResult.Active;
        while (result == TankerUpdateResult.Active)
            result = nearRim.UpdateForDevelopment();
        Assert.True(nearRim.Height > Tanker.CargoReleaseThreshold);
        Assert.False(nearRim.UsesLevel7ProgrammedCargo(
            LevelConfiguration.Level7Production, TankerReleaseKind.PlayerShotDestruction));
    }

    [Fact]
    public void DeepPlayerShotCargoUsesProgramAndDeathReconstructsItAsGenericSupplyWithoutOldRouteState()
    {
        TempestGame game = new(level7Test: true,
            random: new Level7TankerSelectionRandom());
        Egg[] firstEntries = game.Eggs.Take(3).ToArray();
        foreach (Egg original in firstEntries.Take(2))
            MakeReady(original, game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(2, game.Spikers.Count);
        MakeReady(firstEntries[2], game.CurrentLevel);
        game.RunOneStep();
        Tanker tanker = Assert.Single(game.Tankers);

        while (tanker.Height < 250)
            game.RunOneStep();
        Assert.InRange(tanker.Height, 250, 252);
        game.Player.ResetPosition(tanker.EdgeIndex, 0.5f);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height - 1;
        game.RunOneStep();

        Flipper[] releasedCargo = game.Flippers
            .Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId)
            .ToArray();
        Assert.Equal(2, releasedCargo.Length);
        Assert.Equal(2, releasedCargo.Select(flipper => flipper.CargoUnitId).Distinct().Count());
        Assert.Equal(19, game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(2, game.CurrentLevel.Level3Ledger.UnresolvedCargoIds.Count);
        Assert.Equal(new[] { 1, -1 }, releasedCargo.Select(flipper => flipper.Direction));
        Assert.All(releasedCargo, flipper =>
            Assert.Equal(FlipperState.Flipping, flipper.State));
        int[] cargoIds = releasedCargo.Select(flipper => flipper.CargoUnitId).ToArray();
        int[] releaseDestinations = releasedCargo.Select(flipper => flipper.DestinationEdge).ToArray();
        HashSet<int> firstPivotsCompleted = [];

        for (int tick = 0; tick < 40 && firstPivotsCompleted.Count < releasedCargo.Length; tick++)
        {
            game.RunOneStep();
            for (int index = 0; index < releasedCargo.Length; index++)
            {
                Flipper flipper = releasedCargo[index];
                if (flipper.EdgeIndex != releaseDestinations[index] ||
                    !firstPivotsCompleted.Add(flipper.CargoUnitId))
                    continue;

                Assert.Equal(FlipperState.Climbing, flipper.State);
            }
        }
        Assert.Equal(releasedCargo.Length, firstPivotsCompleted.Count);
        Assert.All(releasedCargo, flipper => Assert.InRange(flipper.ShotDelayTicks, 0, 62));
        if (releasedCargo.Any(flipper => flipper.ShotDelayTicks == 0))
            Assert.Equal(game.CurrentLevel.Configuration.MaximumActiveEnemyShots,
                game.EnemyBullets.Count);

        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();
        Assert.Empty(game.Flippers);
        Assert.Empty(game.CurrentLevel.Level3Ledger.UnresolvedCargoIds);
        Assert.All(cargoIds,
            cargoId => Assert.Equal(Level3CargoState.Reconstructed,
                game.CurrentLevel.Level3Ledger.GetCargoState(cargoId)));
        Assert.Equal(21, game.CurrentLevel.PendingGenericEnemyUnits);
        Assert.All(game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected,
                game.CurrentLevel.Level3Ledger.GetOriginalType(id)));
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
    }

    private static int[] RunDestinations(
        int initialLane, int[] playerLanes, int initialDirection)
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            ProgrammedFlipperInitialDirection = initialDirection
        };
        Level level = CreateRouteLevel(
            climbSpeed: 0, turnSpeed: 360, configuration: configuration);
        Flipper flipper = new(initialLane, 0);
        List<int> destinations = [];
        for (int tick = 0; destinations.Count < 15 && tick < 300; tick++)
        {
            int previousLane = flipper.EdgeIndex;
            flipper.Update(level, playerLanes[tick % playerLanes.Length], ThrowOnRandom);
            if (flipper.EdgeIndex != previousLane)
                destinations.Add(flipper.EdgeIndex);
        }
        Assert.Equal(15, destinations.Count);
        return destinations.ToArray();
    }

    private static int[] RunDestinations(
        Flipper flipper, Level level, int playerEdge, int count)
    {
        List<int> destinations = [];
        for (int tick = 0; destinations.Count < count && tick < 100; tick++)
        {
            int previousLane = flipper.EdgeIndex;
            flipper.Update(level, playerEdge, ThrowOnRandom);
            if (flipper.EdgeIndex != previousLane)
                destinations.Add(flipper.EdgeIndex);
        }
        Assert.Equal(count, destinations.Count);
        return destinations.ToArray();
    }

    private static int[] RunCargoDestinations(
        Flipper flipper, Level level, int playerEdge, int count)
    {
        List<int> destinations = [];
        for (int tick = 0; destinations.Count < count && tick < 100; tick++)
        {
            int previousLane = flipper.EdgeIndex;
            flipper.Update(level, playerEdge, ThrowOnRandom);
            if (flipper.EdgeIndex != previousLane)
                destinations.Add(flipper.EdgeIndex);
        }
        Assert.Equal(count, destinations.Count);
        return destinations.ToArray();
    }

    private static Level CreateRouteLevel(
        float climbSpeed, float turnSpeed, LevelConfiguration? configuration = null)
    {
        LevelConfiguration routeConfiguration = (configuration ?? LevelConfiguration.Level7Production) with
        {
            Flipper = new(true, climbSpeed, turnSpeed, 0, 23)
        };
        return new Level(routeConfiguration);
    }

    private static void FinishPivot(Flipper flipper, Level level)
    {
        for (int tick = 0; flipper.State == FlipperState.Flipping && tick < 100; tick++)
            flipper.Update(level, playerEdge: 0, ThrowOnRandom);
        Assert.NotEqual(FlipperState.Flipping, flipper.State);
    }

    private static int ThrowOnRandom(int minimum, int maximum) =>
        throw new InvalidOperationException($"Unexpected route RNG request [{minimum}, {maximum}].");

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private sealed class Level7TankerSelectionRandom : Random
    {
        private int nextLane;

        public override int Next(int minValue, int maxValue)
        {
            if (minValue == 0 && maxValue == 16)
                return nextLane++ % 16;
            if (minValue == 0 && maxValue == 256)
                return 255;
            return minValue;
        }
    }
}
