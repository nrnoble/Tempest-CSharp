using System.Numerics;
using System.Reflection;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level17ProductionTests
{
    [Fact]
    public void ShapePaletteAndProgressionContinueIntoLevelEighteen()
    {
        LevelConfiguration level17 = LevelConfiguration.Level17Production;
        Assert.Same(level17, LevelProgression.GetProductionLevel(17));
        Assert.Same(level17, LevelProgression.GetDirectStartLevel(17));
        Assert.Same(LevelConfiguration.Level1.Definition, level17.Definition);
        Assert.Equal(0, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(17));
        Assert.Equal(1, level17.PaletteIndex);
        Assert.Equal(20, level17.EnemySupply);
        Assert.Same(LevelConfiguration.Level18Production,
            LevelProgression.GetNextProductionLevel(level17));
        Assert.Same(LevelConfiguration.Level18Production, LevelProgression.GetProductionLevel(18));
    }

    [Fact]
    public void RawPaletteAndNibbleDataAreExact()
    {
        Assert.Equal(new byte[] { 0x00, 0x04, 0x08, 0x0C, 0xC3, 0x07, 0x0B, 0x0B },
            RomPaletteCatalog.FirstCycleRaw);
        Assert.Equal(new byte[] { 0x00, 0x07, 0x0B, 0x08, 0x44, 0x03, 0x0C, 0x0C },
            RomPaletteCatalog.SecondCycleRaw);
        Assert.Equal(0, RomPaletteCatalog.PaletteIndexForDisplayedLevel(1));
        Assert.Equal(1, RomPaletteCatalog.PaletteIndexForDisplayedLevel(17));
    }

    [Fact]
    public void SpikeEligibilityUsesWaveSpikeHeightCyclePositions()
    {
        Assert.Equal(new byte[]
        {
            0x00, 0x00, 0x00, 0xE0,
            0xD8, 0xD4, 0xD0, 0xC8,
            0xC0, 0xB8, 0xB0, 0xA8,
            0xA0, 0xA0, 0xA0, 0xA8
        }, LevelConfiguration.RomSpikeHeightRawByCyclePosition);

        Assert.Null(LevelConfiguration.StartingSpikeHeightForDisplayedLevel(17));
        Assert.Null(LevelConfiguration.StartingSpikeHeightForDisplayedLevel(18));
        Assert.Null(LevelConfiguration.StartingSpikeHeightForDisplayedLevel(19));
        Assert.Equal((0xF0 - 0xE0) * Level.Depth / 224f,
            LevelConfiguration.StartingSpikeHeightForDisplayedLevel(20));
    }

    [Fact]
    public void ShapeOneProjectionIsSharedByLevelOneAndLevel17()
    {
        LevelConfiguration level1 = LevelConfiguration.Level1;
        LevelConfiguration level17 = LevelConfiguration.Level17Production;
        RomPresentationMetadata level1Metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(1);
        RomPresentationMetadata level17Metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(17);
        Vector2 point = level1.Definition.Points[0];
        const float depth = 250;
        const float approachDepth = 4;
        const float approachVertical = 9;

        AtariPresentationProjection level1Projection = AtariPresentationAdapter.Project(
            1, point.X, point.Y, depth,
            level1.Definition.CenterY, level1.Definition.YOffset,
            level1.Position, level1.Depth, approachDepth, approachVertical,
            70, 5, level1Metadata, tubeYGain: 1.4f, tubeDepthYGain: 1f);
        AtariPresentationProjection level17Projection = AtariPresentationAdapter.Project(
            17, point.X, point.Y, depth,
            level17.Definition.CenterY, level17.Definition.YOffset,
            level17.Position, level17.Depth, approachDepth, approachVertical,
            70, 5, level17Metadata, tubeYGain: 1.4f, tubeDepthYGain: 1f);

        Assert.Equal(level1Metadata.TubeTableIndex, level17Metadata.TubeTableIndex);
        Assert.Equal(level1Projection.Position, level17Projection.Position);
        Assert.Equal(level1Projection.LocalScale, level17Projection.LocalScale);
        Assert.Equal(level1Projection.LocalScaleY, level17Projection.LocalScaleY);
        Assert.Equal(17, level17.DisplayedLevel);
    }

    [Fact]
    public void DominantLevel17PaletteRolesUseEstablishedHostColors()
    {
        Assert.Equal(Raylib_cs.Color.Red,
            CyclePaletteResolver.ResolveTubeColor(17));
        Assert.Equal(Raylib_cs.Color.Blue,
            CyclePaletteResolver.ResolveTankerColor(17));
        Assert.Equal(Raylib_cs.Color.Yellow,
            CyclePaletteResolver.ResolveEggColor(17));
        Assert.Equal(Raylib_cs.Color.Magenta,
            CyclePaletteResolver.ResolveFlipperColor(17));
        Assert.Equal(new Raylib_cs.Color(0, 255, 255, 255),
            CyclePaletteResolver.ResolvePulsarColor(17));
        Assert.Equal(new Raylib_cs.Color(0, 255, 255, 255),
            AtariHudVector.ResolveScoreColor(17));
        Assert.Equal(Raylib_cs.Color.Green,
            AtariHudVector.ResolveReserveLifeColor(17));
        Assert.Equal(Raylib_cs.Color.Red,
            AtariHudVector.ResolveLevelColor(17));

        Assert.Equal(Raylib_cs.Color.Blue,
            CyclePaletteResolver.ResolveTubeColor(16));
        Assert.Equal(Raylib_cs.Color.Magenta,
            CyclePaletteResolver.ResolveTankerColor(16));
        Assert.Equal(Raylib_cs.Color.Red,
            CyclePaletteResolver.ResolveEggColor(16));
        Assert.Equal(Raylib_cs.Color.Red,
            CyclePaletteResolver.ResolveFlipperColor(16));
    }

    [Fact]
    public void Level17BaseRailAndEggsUseCyclePaletteWithoutChangingEggState()
    {
        TempestGame game = new(startingProductionLevel: 17, random: new Random(1981));
        VectorRenderer renderer = new(game);
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End,
            Raylib_cs.Color Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));

        int playerLane = game.Player.EdgeIndex;
        int nextPlayerBoundary = game.CurrentLevel.NextEdge(playerLane);
        for (int vertex = 0; vertex < game.CurrentLevel.Definition.Points.Count; vertex++)
        {
            Raylib_cs.Color expected = vertex == playerLane || vertex == nextPlayerBoundary
                ? CyclePaletteResolver.ResolveBlasterLaneColor(17)
                : Raylib_cs.Color.Red;
            Assert.Equal(expected, lines[vertex].Color);
        }

        Egg ordinary = new(edgeIndex: 2);
        Egg reconstructed = new(edgeIndex: 3, originalSlotId: 7, cargoUnitId: 11);
        List<Egg> eggs = GetMutableEggList(game.CurrentLevel);
        eggs.Clear();
        eggs.Add(ordinary);
        eggs.Add(reconstructed);
        List<(System.Numerics.Vector2 Point, System.Numerics.Vector2 Size,
            Raylib_cs.Color Color)> eggPoints = [];
        renderer.DrawEggs((point, size, color) => eggPoints.Add((point, size, color)));

        Assert.Equal(2, eggPoints.Count);
        Assert.All(eggPoints, egg => Assert.Equal(Raylib_cs.Color.Yellow, egg.Color));
        Assert.Equal(EggState.Roaming, ordinary.State);
        Assert.Equal(EggState.Roaming, reconstructed.State);
    }

    [Fact]
    public void Level17TransientRailRestoresRedAndCycleZeroStaysBlue()
    {
        TempestGame level17 = new(startingProductionLevel: 17, random: new Random(1981));
        Assert.True(level17.SuperZapper.TryActivate([], level17.Player.EdgeIndex,
            level17.CurrentLevel.PlayableLaneCount, continuous: true));
        for (int tick = 0;
             tick <= SuperZapperSystem.FlashDurationTicks;
             tick++)
        {
            level17.SuperZapper.AdvanceDisplayStep();
        }

        VectorRenderer level17Renderer = new(level17);
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End,
            Raylib_cs.Color Color)> level17Lines = [];
        level17Renderer.DrawTube((start, end, color) =>
            level17Lines.Add((start, end, color)));
        int lane = level17.Player.EdgeIndex;
        int next = level17.CurrentLevel.NextEdge(lane);
        for (int vertex = 0;
             vertex < level17.CurrentLevel.Definition.Points.Count;
             vertex++)
        {
            Assert.Equal(vertex == lane || vertex == next
                    ? CyclePaletteResolver.ResolveBlasterLaneColor(17)
                    : Raylib_cs.Color.Red,
                level17Lines[vertex].Color);
        }

        TempestGame level16 = new(startingProductionLevel: 16, random: new Random(1981));
        Assert.Equal(Raylib_cs.Color.Blue,
            CyclePaletteResolver.ResolveTubeColor(
                level16.CurrentLevel.Configuration.DisplayedLevel));
    }

    [Fact]
    public void MediumConfigurationUsesStraight07AndThreeShotCapacity()
    {
        LevelConfiguration level17 = LevelConfiguration.Level17Production;
        Assert.Equal(BelowRimFlipperProgram.Straight07, level17.BelowRimFlipperProgram);
        Tanker tanker = new(0, 0);
        Assert.Equal(TankerCargoMovementProgram.Level17Straight07,
            tanker.GetCargoMovementProgram(level17, TankerReleaseKind.Natural));
        Assert.Equal(2.53125f, level17.Flipper.ClimbSpeed);
        Assert.Equal(32, level17.FlipperShotHoldoffTicks);
        Assert.Equal(3, level17.MaximumActiveEnemyShots);
        Assert.Null(level17.StartingSpikeHeight);
        Assert.Equal(0, Level17WaveSelection.MinimumSpikers);
        Assert.Equal(0, Level17WaveSelection.MaximumSpikers);
        Assert.Equal((byte)0x07, Level17WaveSelection.FlipperMovementProgram);
        Assert.Equal((byte)0xAF, Level17WaveSelection.FlipperSpeedRaw);
        Assert.Equal((byte)0x28, Level17WaveSelection.PulsarFlipRateRaw);
        Assert.Equal((byte)0x04, Level17WaveSelection.PulseBeatRaw);
        Assert.False(Level17WaveSelection.MediumPulsarFiringEnabled);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(100)]
    public void WaveSelectionConservesCapacityAndIsDeterministic(int seed)
    {
        Random random = new(seed);
        Level3OriginalType?[] selections = new Level3OriginalType?[32];
        int tankers = 0;
        int spikers = 0;
        int pulsars = 0;
        int flippers = 0;
        int activeUnits = 0;

        for (int index = 0; index < selections.Length; index++)
        {
            selections[index] = Level17WaveSelection.SelectOriginalType(
                (min, max) => random.Next(min, max + 1),
                tankers, spikers, pulsars, activeUnits, flippers);
            if (selections[index] is null)
                break;
            switch (selections[index]!.Value)
            {
                case Level3OriginalType.Tanker:
                    tankers++;
                    activeUnits += 2;
                    flippers += 2;
                    break;
                case Level3OriginalType.Spiker:
                    spikers++;
                    activeUnits++;
                    break;
                case Level3OriginalType.Pulsar:
                    pulsars++;
                    activeUnits++;
                    break;
                case Level3OriginalType.Flipper:
                    flippers++;
                    activeUnits++;
                    break;
            }
        }

        Assert.InRange(pulsars, 2, 5);
        Assert.Equal(1, tankers);
        Assert.InRange(flippers, 0, 3);
        Assert.InRange(spikers, 0, 2);
    }

    [Fact]
    public void PulsarLedgerAndTankerCargoIdentityConserve()
    {
        Level3WaveLedger ledger = new(20);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsPulsar(0);
        Assert.Equal(Level3OriginalType.Pulsar, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.ActivePulsar, ledger.GetOriginalState(0));
        Assert.Contains(0, ledger.UnresolvedOriginalIds);

        ledger.MarkOriginalDestroyed(0);
        Assert.DoesNotContain(0, ledger.UnresolvedOriginalIds);

        ledger.MarkOriginalEgg(1);
        ledger.CommitOriginalAsTanker(1);
        Assert.Equal(2, ledger.GetCargoIdsForOriginal(1).Count);
        Assert.All(ledger.GetCargoIdsForOriginal(1),
            id => Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id)));
    }

    [Fact]
    public void DirectStartupAndCalibrationResolveShapeOneDefaults()
    {
        Tempest.Game.TempestGame game =
            new(startingProductionLevel: 17, random: new Random(1981));
        Assert.Same(LevelConfiguration.Level17Production, game.CurrentLevel.Configuration);
        Assert.Equal(17, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(AtariPresentationCalibration.ForDisplayedLevel(1),
            AtariPresentationCalibration.ForDisplayedLevel(17));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Straight07FlippersHandOffToRimAndResumePursuit(bool cargo)
    {
        Tempest.Game.TempestGame game =
            new(startingProductionLevel: 17, random: new Random(1982));
        Level level = game.CurrentLevel;
        Flipper flipper;

        if (cargo)
        {
            flipper = Flipper.CreateTankerCargo(
                level,
                new TankerCargoSpawn(7, 6, 1, Level.Depth - 50),
                cargoUnitId: 101,
                tankerCargoMovementProgram: TankerCargoMovementProgram.Level17Straight07);
        }
        else
        {
            flipper = new Flipper(0, Level.Depth - 50);
        }

        int startingLane = flipper.EdgeIndex;
        for (int tick = 0; tick < 100; tick++)
        {
            flipper.Update(level, playerEdge: 7, (min, _) => min);
            if (flipper.Height >= Level.Depth &&
                flipper.RimPursuitDirection.HasValue &&
                (flipper.State == FlipperState.Flipping ||
                 flipper.EdgeIndex != startingLane ||
                 flipper.ActiveTransition is not null))
            {
                break;
            }
        }

        Assert.Equal(Level.Depth, flipper.Height);
        Assert.NotNull(flipper.RimPursuitDirection);
        Assert.True(flipper.State == FlipperState.Flipping ||
                    flipper.EdgeIndex != startingLane ||
                    flipper.ActiveTransition is not null,
            "Level 17 straight-$07 Flipper remained frozen after rim arrival.");
    }

    private static List<Egg> GetMutableEggList(Level level)
    {
        FieldInfo field = typeof(Level).GetField("eggs",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Level Egg list was not found.");
        return (List<Egg>)(field.GetValue(level)
            ?? throw new InvalidOperationException("Level Egg list was not initialized."));
    }
}
