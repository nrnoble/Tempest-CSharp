using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

using Raylib_cs;
using System.Numerics;

namespace Tempest.Game.Tests;

public sealed class Level19ProductionTests
{
    [Fact]
    public void ProductionIdentityReusesShapeThreeAndCycleOnePalette()
    {
        LevelConfiguration level19 = LevelConfiguration.Level19Production;

        Assert.Same(level19, LevelProgression.GetProductionLevel(19));
        Assert.Same(level19, LevelProgression.GetDirectStartLevel(19));
        Assert.Same(LevelConfiguration.Level19Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level18Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(level19));
        Assert.Same(level19, LevelProgression.GetLastProductionLevel());
        Assert.Same(LevelConfiguration.Level3Mixed.Definition, level19.Definition);
        Assert.Equal((18, 19, 19, 2, 1, 2, 1),
            (level19.ProgressionIndex, level19.DisplayedLevel,
             level19.EffectiveGameplayParameterLevel, level19.BoardIndex,
             level19.PaletteIndex, RomTubeMappingCatalog.ResolveShapeRow(19),
             RomPaletteCatalog.PaletteIndexForDisplayedLevel(19)));
        Assert.Equal(16, level19.PlayableLaneCount);
        Assert.True(level19.Definition.Continuous);
        Assert.Null(level19.StartingSpikeHeight);
    }

    [Fact]
    public void MediumWaveAndEligibilityMatchTheRomEnvelope()
    {
        LevelConfiguration level19 = LevelConfiguration.Level19Production;

        Assert.Equal(22, level19.EnemySupply);
        Assert.Equal(7, Level19WaveSelection.MaximumPhysicalEnemySlots);
        Assert.Equal((0, 3, 1, 1, 0, 0, 2, 3, 0, 0),
            (Level19WaveSelection.MinimumFlippers, Level19WaveSelection.MaximumFlippers,
             Level19WaveSelection.MinimumTankers, Level19WaveSelection.MaximumTankers,
             Level19WaveSelection.MinimumSpikers, Level19WaveSelection.MaximumSpikers,
             Level19WaveSelection.MinimumPulsars, Level19WaveSelection.MaximumPulsars,
             Level19WaveSelection.MinimumFuseballs, Level19WaveSelection.MaximumFuseballs));
        Assert.True(level19.AllowsOrdinaryFlipperSelection);
        Assert.False(Level19WaveSelection.MediumPulsarFiringEnabled);
        Assert.Null(level19.StartingSpikeHeight);
        Assert.Empty(new Level(level19).Spikes);
        Assert.Equal((29, 3),
            (level19.FlipperShotHoldoffTicks, level19.MaximumActiveEnemyShots));
        Assert.Equal(2.71875f, level19.Flipper.ClimbSpeed);
    }

    [Fact]
    public void SelectorSatisfiesMinimaAndKeepsCargoOutsideOriginalSupply()
    {
        Assert.Equal(Level3OriginalType.Pulsar,
            Level19WaveSelection.SelectOriginalType((_, _) => 0, 0, 0, 0, 0, 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level19WaveSelection.SelectOriginalType((_, _) => 0, 0, 0, 2, 2, 2));
        Assert.Null(Level19WaveSelection.SelectOriginalType(
            (_, _) => 0, 1, 0, 3, 7, 3));

        for (int seed = 0; seed < 24; seed++)
        {
            Random random = new(seed);
            int tankers = 0;
            int pulsars = 0;
            int flipperUnits = 0;
            int occupiedUnits = 0;

            for (int admission = 0;
                 admission < Level19WaveSelection.MaximumPhysicalEnemySlots;
                 admission++)
            {
                Level3OriginalType? selected = Level19WaveSelection.SelectOriginalType(
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
            Assert.InRange(pulsars, 2, 3);
            Assert.InRange(flipperUnits - Tanker.CarriedFlipperCount, 0, 3);
        }
    }

    [Fact]
    public void CargoAndSharedRouteReuseTankerAndFlipperArchitecture()
    {
        LevelConfiguration level19 = LevelConfiguration.Level19Production;
        Tanker tanker = new(0, originalSlotId: 5,
            climbSpeed: level19.Flipper.ClimbSpeed);

        Assert.Equal(TankerCargoMovementProgram.EightAdvanceTurn,
            tanker.GetCargoMovementProgram(level19, TankerReleaseKind.Natural));
        Assert.Equal(BelowRimFlipperProgram.EightAdvanceTurn,
            level19.BelowRimFlipperProgram);
        Assert.Equal(0x19, Level19WaveSelection.FlipperMovementProgram);
        Assert.Equal(0xA9, Level19WaveSelection.FlipperSpeedRaw);
        Assert.Equal(2.71875f,
            Level19WaveSelection.MediumFlipperTankerSpeed);
    }

    [Fact]
    public void PulsarThresholdSpeedAndRatioUseTheSharedPolicy()
    {
        LevelConfiguration level19 = LevelConfiguration.Level19Production;
        float threshold = Level19WaveSelection.MapPulsarThresholdToHostHeight(
            Level19WaveSelection.PulsarFlipRateRaw, level19.Depth);

        Assert.Equal(392.92035f, threshold, 3);
        Assert.Equal(threshold,
            level19.PulsarMovement.ThresholdHeight);
        Assert.Equal(1f, level19.PulsarMovement.FixedSpeed);
        Assert.Equal(87f / 55f,
            level19.PulsarMovement.DeepSideTowardRimSpeed);
        Assert.False(Level19WaveSelection.MediumPulsarFiringEnabled);
    }

    [Fact]
    public void PresentationAndPaletteResolveThroughCanonicalShapeThree()
    {
        Assert.Same(LevelConfiguration.Level3Mixed.Definition,
            LevelConfiguration.Level19Production.Definition);
        Assert.Equal(2, RomTubeMappingCatalog.ResolveShapeRow(19));
        Assert.Equal(1, RomPaletteCatalog.PaletteIndexForDisplayedLevel(19));
        Assert.Equal(
            AtariPresentationCalibration.ForDisplayedLevel(3),
            AtariPresentationCalibration.ForDisplayedLevel(19));
        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(19));
        Assert.False(Level19WaveSelection.MediumPulsarFiringEnabled);
    }

    [Theory]
    [InlineData(1, 17)]
    [InlineData(2, 18)]
    [InlineData(3, 19)]
    public void RepeatedShapesProduceEquivalentEffectivePresentation(int ownerLevel, int repeatedLevel)
    {
        LevelConfiguration owner = LevelProgression.GetProductionLevel(ownerLevel)!;
        LevelConfiguration repeated = LevelProgression.GetProductionLevel(repeatedLevel)!;
        RomPresentationMetadata ownerMetadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(ownerLevel);
        RomPresentationMetadata repeatedMetadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(repeatedLevel);
        Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;

        Assert.Equal(
            RomTubeMappingCatalog.ResolveShapeRow(ownerLevel),
            RomTubeMappingCatalog.ResolveShapeRow(repeatedLevel));
        Assert.Equal(ownerMetadata.TubeTableIndex, repeatedMetadata.TubeTableIndex);
        Assert.Equal(
            AtariPresentationCalibration.ForDisplayedLevel(ownerLevel),
            AtariPresentationCalibration.ForDisplayedLevel(repeatedLevel));
        Assert.Equal(owner.Definition.Points, repeated.Definition.Points);

        float[] depths =
        [
            owner.Position,
            owner.Position + owner.Depth / 2,
            owner.Position + owner.Depth
        ];

        foreach (float depth in depths)
        foreach (Vector2 point in owner.Definition.Points)
        {
            AtariPresentationProjection ownerProjection =
                AtariPresentationAdapter.Project(
                    ownerLevel, point.X, point.Y, depth,
                    owner.Definition.CenterY, owner.Definition.YOffset,
                    owner.Position, owner.Depth, 0, 0, 70, 5, ownerMetadata);
            AtariPresentationProjection repeatedProjection =
                AtariPresentationAdapter.Project(
                    repeatedLevel, point.X, point.Y, depth,
                    repeated.Definition.CenterY, repeated.Definition.YOffset,
                    repeated.Position, repeated.Depth, 0, 0, 70, 5, repeatedMetadata);

            Assert.InRange(Vector2.Distance(ownerProjection.Position,
                repeatedProjection.Position), 0, .001f);
            Assert.InRange(MathF.Abs(ownerProjection.LocalScale -
                repeatedProjection.LocalScale), 0, .001f);

            Vector2 ownerHost = camera.Offset +
                (ownerProjection.Position - camera.Target) * camera.Zoom;
            Vector2 repeatedHost = camera.Offset +
                (repeatedProjection.Position - camera.Target) * camera.Zoom;
            Assert.InRange(Vector2.Distance(ownerHost, repeatedHost), 0, .01f);
        }

        for (int lane = 0; lane < owner.PlayableLaneCount; lane++)
        {
            Vector2 ownerNear = (owner.Definition.Points[lane] +
                owner.Definition.Points[(lane + 1) % owner.PlayableLaneCount]) / 2;
            Vector2 repeatedNear = (repeated.Definition.Points[lane] +
                repeated.Definition.Points[(lane + 1) % repeated.PlayableLaneCount]) / 2;

            for (float entityDepthOffset = 0; entityDepthOffset <= owner.Depth; entityDepthOffset += owner.Depth / 2)
            {
                float depth = owner.Position + owner.Depth - entityDepthOffset;
                AtariPresentationProjection ownerEntity =
                    AtariPresentationAdapter.Project(
                        ownerLevel, ownerNear.X, ownerNear.Y, depth,
                        owner.Definition.CenterY, owner.Definition.YOffset,
                        owner.Position, owner.Depth, 0, 0, 70, 5, ownerMetadata);
                AtariPresentationProjection repeatedEntity =
                    AtariPresentationAdapter.Project(
                        repeatedLevel, repeatedNear.X, repeatedNear.Y, depth,
                        repeated.Definition.CenterY, repeated.Definition.YOffset,
                        repeated.Position, repeated.Depth, 0, 0, 70, 5, repeatedMetadata);
                Assert.InRange(Vector2.Distance(ownerEntity.Position,
                    repeatedEntity.Position), 0, .001f);
            }
        }
    }
}
