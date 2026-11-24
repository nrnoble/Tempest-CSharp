using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class RomPresentationMetadataTests
{
    [Fact]
    public void ProductionLevelsPreserveRev2PresentationTableValues()
    {
        int[] tubeTableIndices = [0, 1, 2, 3, 4, 5, 6, 7, 13];
        byte[] scales = [0x18, 0x1C, 0x18, 0x0F, 0x18, 0x18, 0x18, 0x18, 0x0C];
        byte[] cameraHeights = [0x50, 0x50, 0x50, 0x68, 0x50, 0x50, 0x68, 0xB0, 0xB0];
        byte[] yOffsetLows = [0x40, 0x20, 0x40, 0x80, 0x40, 0x40, 0x70, 0x60, 0x40];
        byte[] yOffsetHighs = [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x01];
        ushort[] yOffsetCombined = [0xFF40, 0xFF20, 0xFF40, 0xFF80, 0xFF40, 0xFF40, 0xFF70, 0x0060, 0x0140];
        short[] yOffsetSigned = [-192, -224, -192, -128, -192, -192, -144, 96, 320];
        byte[] openValues = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF];

        Assert.Equal(9, RomPresentationMetadataCatalog.ProductionLevels1To9.Count);

        for (int index = 0; index < 9; index++)
        {
            int displayedLevel = index + 1;
            RomPresentationMetadata metadata =
                RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);

            Assert.Same(RomPresentationMetadataCatalog.ProductionLevels1To9[index], metadata);
            Assert.Equal(displayedLevel, metadata.DisplayedLevel);
            Assert.Equal(tubeTableIndices[index], metadata.TubeTableIndex);
            Assert.Equal(scales[index], metadata.ScaleRaw);
            Assert.Equal(cameraHeights[index], metadata.CameraHeightRaw);
            Assert.Equal(yOffsetLows[index], metadata.ScreenYOffsetLowRaw);
            Assert.Equal(yOffsetHighs[index], metadata.ScreenYOffsetHighRaw);
            Assert.Equal(yOffsetCombined[index], metadata.ScreenYOffsetCombinedRaw);
            Assert.Equal(yOffsetSigned[index], metadata.ScreenYOffsetSigned);
            Assert.Equal(openValues[index], metadata.OpenRaw);
            Assert.Equal(openValues[index] == 0xFF, metadata.IsOpen);
        }
    }

    [Fact]
    public void SignedYOffsetCombinesBytesAsLittleEndianTwosComplement()
    {
        RomPresentationMetadata negative = new(1, 0, 0, 0, 0x40, 0xFF, 0x00);
        RomPresentationMetadata positive = new(8, 7, 0, 0, 0x60, 0x00, 0xFF);

        Assert.Equal((ushort)0xFF40, negative.ScreenYOffsetCombinedRaw);
        Assert.Equal((short)-192, negative.ScreenYOffsetSigned);
        Assert.Equal((ushort)0x0060, positive.ScreenYOffsetCombinedRaw);
        Assert.Equal((short)96, positive.ScreenYOffsetSigned);
    }

    [Fact]
    public void UnsupportedDisplayedLevelsDoNotFallBackToMetadata()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomPresentationMetadataCatalog.ForDisplayedLevel(0));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomPresentationMetadataCatalog.ForDisplayedLevel(16));
    }

    [Fact]
    public void Rev2EncounterOrderMapsDisplayedLevelsElevenThroughSixteenToTheirShapeRows()
    {
        Assert.Equal(new[] { 8, 12, 14, 15, 10, 11 },
            RomTubeMappingCatalog.DisplayedLevels1To16.Skip(10));

        for (int displayedLevel = 11; displayedLevel <= 16; displayedLevel++)
        {
            Assert.Equal(RomTubeMappingCatalog.DisplayedLevels1To16[displayedLevel - 1],
                RomTubeMappingCatalog.ShapeRowForDisplayedLevel(displayedLevel));
        }

        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomTubeMappingCatalog.ShapeRowForDisplayedLevel(0));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => RomTubeMappingCatalog.ShapeRowForDisplayedLevel(17));
    }

    [Fact]
    public void LevelElevenAndFifteenMetadataFollowTheirRemappedShapeRows()
    {
        RomPresentationMetadata level11 = RomPresentationMetadataCatalog.ForDisplayedLevel(11);
        RomPresentationMetadata level15 = RomPresentationMetadataCatalog.ForDisplayedLevel(15);

        Assert.Equal((8, (byte)0x0A, (byte)0xA0, (ushort)0x0100, (byte)0xFF),
            (level11.TubeTableIndex, level11.ScaleRaw, level11.CameraHeightRaw,
             level11.ScreenYOffsetCombinedRaw, level11.OpenRaw));
        Assert.Equal((10, (byte)0x10, (byte)0x90, (ushort)0x0040, (byte)0xFF),
            (level15.TubeTableIndex, level15.ScaleRaw, level15.CameraHeightRaw,
             level15.ScreenYOffsetCombinedRaw, level15.OpenRaw));
    }

    [Fact]
    public void DiagnosticLevelTenUsesRecoveredShapeRowNineMetadataOutsideProductionCatalog()
    {
        RomPresentationMetadata level7 = RomPresentationMetadataCatalog.ForDisplayedLevel(7);
        RomPresentationMetadata level10 = RomPresentationMetadataCatalog.ForDisplayedLevel(10);

        Assert.Equal(9, RomPresentationMetadataCatalog.ProductionLevels1To9.Count);
        Assert.Equal(10, level10.DisplayedLevel);
        Assert.Equal(9, level10.TubeTableIndex);
        Assert.Equal((byte)0x18, level10.ScaleRaw);
        Assert.Equal((byte)0x50, level10.CameraHeightRaw);
        Assert.Equal((byte)0x20, level10.ScreenYOffsetLowRaw);
        Assert.Equal((byte)0xFF, level10.ScreenYOffsetHighRaw);
        Assert.Equal(unchecked((short)0xFF20), level10.ScreenYOffsetSigned);
        Assert.Equal((byte)0xFF, level10.OpenRaw);
        Assert.True(level10.IsOpen);
        Assert.NotEqual(level7.TubeTableIndex, level10.TubeTableIndex);
    }

    [Fact]
    public void InvalidOpenEncodingIsRejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new RomPresentationMetadata(1, 0, 0, 0, 0, 0, 0x01));
    }

    [Fact]
    public void RomOpenEvidenceAgreesWithCurrentProductionTopology()
    {
        for (int displayedLevel = 1; displayedLevel <= 14; displayedLevel++)
        {
            RomPresentationMetadata metadata =
                RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
            LevelConfiguration configuration =
                LevelProgression.GetProductionLevel(displayedLevel)
                ?? throw new InvalidOperationException(
                    $"No production configuration exists for displayed Level {displayedLevel}.");

            Assert.Equal(displayedLevel, configuration.DisplayedLevel);
            Assert.Equal(metadata.IsOpen, !configuration.Definition.Continuous);
        }

        RomPresentationMetadata level10Metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(10);
        LevelConfiguration level10 = LevelConfiguration.Level10Production;
        Assert.Equal(level10Metadata.IsOpen, !level10.Definition.Continuous);
        Assert.Equal(15, level10.PlayableLaneCount);

        RomPresentationMetadata level14Metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(14);
        LevelConfiguration level14 = LevelConfiguration.Level14Production;
        Assert.Equal((15, (byte)0x0C, (byte)0xB0, (ushort)0x0140,
            (short)320, (byte)0xFF, (byte)0x50, (byte)0x04),
            (level14Metadata.TubeTableIndex, level14Metadata.ScaleRaw,
             level14Metadata.CameraHeightRaw, level14Metadata.ScreenYOffsetCombinedRaw,
             level14Metadata.ScreenYOffsetSigned, level14Metadata.OpenRaw,
             level14Metadata.FlipperScaleLowRaw, level14Metadata.FlipperScaleHighRaw));
        Assert.Equal((ushort)0x0250, LevelLoader.Level14RomEvidence.FlipperScale);
        Assert.Equal(level14Metadata.IsOpen, !level14.Definition.Continuous);
        Assert.Equal(15, level14.PlayableLaneCount);
    }
}
