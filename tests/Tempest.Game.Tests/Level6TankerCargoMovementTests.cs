using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level6TankerCargoMovementTests
{
    [Fact]
    public void DeepPlayerShotReleaseCreatesOppositelyDirectedLevel6ZeroBChildren()
    {
        TempestGame game = CreateProductionGameWithTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        for (int tick = 0; tanker.Height < 200 && tick < 300; tick++)
            tanker.UpdateForDevelopment();
        Assert.True(tanker.Height >= 200);

        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex, 16);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height + 5;
        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.Equal(2, cargo.Length);
        Assert.All(cargo, child =>
        {
            Assert.Equal(TankerCargoMovementProgram.Level6Deep0B, child.CargoMovementProgram);
            Assert.Equal(FlipperState.Climbing, child.State);
            Assert.Null(child.ActiveTransition);
            Assert.Equal(tanker.Height, child.Height);
        });
        Assert.Equal(-cargo[0].Direction, cargo[1].Direction);
        Assert.Equal(2, game.CurrentLevel.Level3Ledger!.UnresolvedCargoIds.Count);
    }

    [Fact]
    public void Level6ZeroBUsesEightStraightAdvancesAndRepeatsStationaryDepthFlips()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Flipper flipper = CreateDeepPlayerShotCargo(level, out TankerCargoSpawn[] pair);
        float initialHeight = flipper.Height;
        int initialDirection = flipper.Direction;

        for (int advance = 1; advance <= 7; advance++)
        {
            flipper.Update(level, playerEdge: 8);
            Assert.Equal(FlipperState.Climbing, flipper.State);
            Assert.Null(flipper.ActiveTransition);
            Assert.Equal(initialHeight + 2 * advance, flipper.Height);
        }

        flipper.Update(level, playerEdge: 8);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(initialHeight + 16, flipper.Height);
        Assert.Equal(initialDirection, flipper.Direction);

        float depthDuringFlip = flipper.Height;
        for (int tick = 0; flipper.State == FlipperState.Flipping && tick < 20; tick++)
            flipper.Update(level, playerEdge: 8);

        Assert.Equal(FlipperState.Climbing, flipper.State);
        Assert.Equal(depthDuringFlip, flipper.Height);
        Assert.Equal(0, flipper.PauseTicks);
        Assert.Equal(initialDirection, flipper.Direction);
        Assert.Equal(pair[0].Direction, initialDirection);

        for (int advance = 1; advance <= 7; advance++)
        {
            flipper.Update(level, playerEdge: 8);
            Assert.Equal(FlipperState.Climbing, flipper.State);
            Assert.Equal(depthDuringFlip + 2 * advance, flipper.Height);
        }

        flipper.Update(level, playerEdge: 8);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(depthDuringFlip + 16, flipper.Height);
        Assert.Equal(initialDirection, flipper.Direction);
    }

    [Fact]
    public void NaturalNearRimReleaseUsesStraightLevel6ZeroSevenAndHandsOffAtRim()
    {
        TempestGame game = CreateProductionGameWithTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        AdvanceTankerToNaturalRelease(tanker);
        float releaseHeight = tanker.Height;

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Flipper[] cargo = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.Equal(2, cargo.Length);
        Assert.All(cargo, child =>
        {
            Assert.Equal(TankerCargoMovementProgram.Level6Straight07, child.CargoMovementProgram);
            Assert.Equal(FlipperState.Climbing, child.State);
            Assert.Null(child.ActiveTransition);
            Assert.Equal(releaseHeight, child.Height);
        });
        Assert.Equal(-cargo[0].Direction, cargo[1].Direction);

        Flipper flipper = cargo[0];
        Level level = game.CurrentLevel;
        for (int tick = 0; flipper.Height < Level.Depth && tick < 200; tick++)
        {
            flipper.Update(level, playerEdge: 8);
            if (flipper.Height < Level.Depth)
            {
                Assert.Equal(FlipperState.Climbing, flipper.State);
                Assert.Null(flipper.ActiveTransition);
            }
        }

        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(LevelConfiguration.Level6Production.Flipper.RimPauseTicks,
            flipper.PauseTicks);
        for (int tick = 0; tick < LevelConfiguration.Level6Production.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge: 8);
        Assert.Equal(FlipperState.Flipping, flipper.State);

        for (int tick = 0; flipper.State == FlipperState.Flipping && tick < 20; tick++)
            flipper.Update(level, playerEdge: 8);
        Assert.NotNull(flipper.RimPursuitDirection);
        Assert.InRange(flipper.EdgeIndex, 0, level.PlayableLaneCount - 1);
    }

    [Fact]
    public void Level6DeepProgramHandsOffToExistingRimMovementWithoutSticking()
    {
        Level level = new(LevelConfiguration.Level6Production);
        Tanker tanker = CreateDeepTanker(370);
        TankerCargoSpawn spawn = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.PlayerShotDestruction).Cargo[0];
        Flipper flipper = Flipper.CreateTankerCargo(
            level, spawn, cargoUnitId: 1,
            tankerCargoMovementProgram: tanker.GetCargoMovementProgram(
                level.Configuration, TankerReleaseKind.PlayerShotDestruction));

        for (int tick = 0;
             flipper.Height < Level.Depth && tick < 200;
             tick++)
            flipper.Update(level, playerEdge: 8);

        Assert.Equal(Level.Depth, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(level.Configuration.Flipper.RimPauseTicks, flipper.PauseTicks);
        for (int tick = 0; tick < level.Configuration.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge: 8);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        for (int tick = 0; flipper.State == FlipperState.Flipping && tick < 20; tick++)
            flipper.Update(level, playerEdge: 8);

        Assert.NotNull(flipper.RimPursuitDirection);
        Assert.InRange(flipper.EdgeIndex, 0, level.PlayableLaneCount - 1);
    }

    [Fact]
    public void Level6MovementSelectionIsDepthBasedAndIsolatedToCanonicalNonZapReleases()
    {
        Tanker deep = CreateDeepTanker(200);
        Tanker nearRim = CreateNaturalReleaseTanker();

        Assert.Equal(TankerCargoMovementProgram.Level6Deep0B,
            deep.GetCargoMovementProgram(
                LevelConfiguration.Level6Production, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Level6Straight07,
            nearRim.GetCargoMovementProgram(
                LevelConfiguration.Level6Production, TankerReleaseKind.Natural));
        Assert.Equal(TankerCargoMovementProgram.Existing,
            deep.GetCargoMovementProgram(
                LevelConfiguration.Level6Production, TankerReleaseKind.SuperZapperDestruction));

        LevelConfiguration copiedLevel6 = LevelConfiguration.Level6Production with { };
        Assert.Equal(TankerCargoMovementProgram.Existing,
            deep.GetCargoMovementProgram(copiedLevel6, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Existing,
            deep.GetCargoMovementProgram(
                LevelConfiguration.Level5Production, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Existing,
            deep.GetCargoMovementProgram(
                LevelConfiguration.Level7Production, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Existing,
            deep.GetCargoMovementProgram(
                LevelConfiguration.Level8Production, TankerReleaseKind.PlayerShotDestruction));

        float romThresholdHeight = Tanker.HeightForRomAlongPosition(0x20);
        Tanker justDeep = CreateTankerAtHeight(romThresholdHeight - 0.5f);
        Tanker justNearRim = CreateTankerAtHeight(romThresholdHeight + 0.5f);
        Assert.True(Tanker.RomAlongPositionForHeight(justDeep.Height) >= 0x20);
        Assert.True(Tanker.RomAlongPositionForHeight(justNearRim.Height) < 0x20);
        Assert.Equal(TankerCargoMovementProgram.Level6Deep0B,
            justDeep.GetCargoMovementProgram(
                LevelConfiguration.Level6Production, TankerReleaseKind.PlayerShotDestruction));
        Assert.Equal(TankerCargoMovementProgram.Level6Straight07,
            justNearRim.GetCargoMovementProgram(
                LevelConfiguration.Level6Production, TankerReleaseKind.PlayerShotDestruction));
    }

    private static TempestGame CreateProductionGameWithTanker()
    {
        TempestGame game = new(startingProductionLevel: 6, random: new Random(1));
        game.CurrentLevel.State = LevelState.Playing;
        foreach (Egg egg in game.Eggs.Take(3))
        {
            egg.BeginApproach();
            while (egg.State != EggState.ReadyToHatch)
                egg.Update(game.CurrentLevel);
        }

        game.RunOneStep();
        Assert.Equal(2, game.Spikers.Count);
        return game;
    }

    private static Flipper CreateDeepPlayerShotCargo(
        Level level, out TankerCargoSpawn[] pair)
    {
        Tanker tanker = CreateDeepTanker(200);
        TankerReleaseKind releaseKind = TankerReleaseKind.PlayerShotDestruction;
        TankerCargoMovementProgram program =
            tanker.GetCargoMovementProgram(level.Configuration, releaseKind);
        pair = tanker.ReleaseFlipperCargo(level, 1, releaseKind).Cargo.ToArray();
        Assert.Equal(2, pair.Length);
        return Flipper.CreateTankerCargo(
            level, pair[0], cargoUnitId: 1, tankerCargoMovementProgram: program);
    }

    private static Tanker CreateDeepTanker(float minimumHeight)
    {
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 0);
        for (int tick = 0; tanker.Height < minimumHeight && tick < 300; tick++)
            tanker.UpdateForDevelopment();
        Assert.True(tanker.Height >= minimumHeight);
        return tanker;
    }

    private static Tanker CreateTankerAtHeight(float height)
    {
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 0, climbSpeed: height - 10);
        Assert.Equal(TankerUpdateResult.Active, tanker.UpdateForDevelopment());
        Assert.Equal(height, tanker.Height);
        return tanker;
    }

    private static Tanker CreateNaturalReleaseTanker()
    {
        Tanker tanker = new(edgeIndex: 0, originalSlotId: 0);
        AdvanceTankerToNaturalRelease(tanker);
        return tanker;
    }

    private static void AdvanceTankerToNaturalRelease(Tanker tanker)
    {
        TankerUpdateResult result = TankerUpdateResult.Active;
        for (int tick = 0; tick < 300 && result == TankerUpdateResult.Active; tick++)
            result = tanker.UpdateForDevelopment();
        Assert.Equal(TankerUpdateResult.NaturalCargoRelease, result);
    }
}
