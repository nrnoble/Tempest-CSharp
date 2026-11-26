namespace Tempest.Game.Levels;

/// <summary>
/// Raw Atari Tempest Rev 2 per-tube presentation metadata. Displayed levels select
/// zero-based tube-table rows through <c>lev_remap</c>. The source describes
/// <see cref="ScaleRaw"/> as reciprocal level scale and <see cref="CameraHeightRaw"/>
/// as a 3-D Y offset / camera height. The source's <c>lev_y2d</c> low byte and
/// <c>lev_y2db</c> high byte combine little-endian as signed 16-bit post-projection
/// placement; <see cref="OpenRaw"/> is 0x00 closed or 0xFF open. Any use by the C# presentation
/// adapter is a project translation, not a direct mathbox/DVG translation or a claim of final
/// visual fidelity.
/// </summary>
public sealed class RomPresentationMetadata
{
    public RomPresentationMetadata(
        int displayedLevel,
        int tubeTableIndex,
        byte scaleRaw,
        byte cameraHeightRaw,
        byte screenYOffsetLowRaw,
        byte screenYOffsetHighRaw,
        byte openRaw,
        byte flipperScaleLowRaw = 0,
        byte flipperScaleHighRaw = 0)
    {
        if (openRaw != 0x00 && openRaw != 0xFF)
            throw new ArgumentOutOfRangeException(
                nameof(openRaw), openRaw, "ROM lev_open must be 0x00 (closed) or 0xFF (open).");

        DisplayedLevel = displayedLevel;
        TubeTableIndex = tubeTableIndex;
        ScaleRaw = scaleRaw;
        CameraHeightRaw = cameraHeightRaw;
        ScreenYOffsetLowRaw = screenYOffsetLowRaw;
        ScreenYOffsetHighRaw = screenYOffsetHighRaw;
        OpenRaw = openRaw;
        FlipperScaleLowRaw = flipperScaleLowRaw;
        FlipperScaleHighRaw = flipperScaleHighRaw;
    }

    public int DisplayedLevel { get; }
    public int TubeTableIndex { get; }
    public byte ScaleRaw { get; }
    public byte CameraHeightRaw { get; }
    public byte ScreenYOffsetLowRaw { get; }
    public byte ScreenYOffsetHighRaw { get; }
    public ushort ScreenYOffsetCombinedRaw =>
        (ushort)(ScreenYOffsetLowRaw | (ScreenYOffsetHighRaw << 8));
    public short ScreenYOffsetSigned => unchecked((short)ScreenYOffsetCombinedRaw);
    public byte OpenRaw { get; }
    public byte FlipperScaleLowRaw { get; }
    public byte FlipperScaleHighRaw { get; }
    public ushort FlipperScaleCombinedRaw =>
        (ushort)(FlipperScaleLowRaw | (FlipperScaleHighRaw << 8));
    public bool IsOpen => OpenRaw == 0xFF;
}

public static class RomTubeMappingCatalog
{
    private static readonly IReadOnlyList<int> ShapeRowsByDisplayedLevel =
        Array.AsReadOnly(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 13, 9, 8, 12, 14, 15, 10, 11 });

    public static IReadOnlyList<int> DisplayedLevels1To16 => ShapeRowsByDisplayedLevel;
    public static IReadOnlyList<int> FirstCycleShapeRows => ShapeRowsByDisplayedLevel;

    // ROM-BACKED get_tube_no: subtract 16 repeatedly, then remap the retained 16-level index.
    // Level 17 therefore resolves to shape row 0 / canonical Level 1 geometry.
    public static int ResolveShapeRow(int displayedLevel)
    {
        if (displayedLevel < 1)
            throw new ArgumentOutOfRangeException(nameof(displayedLevel), displayedLevel,
                "Tempest displayed levels start at 1.");
        if (displayedLevel > 19)
            throw new ArgumentOutOfRangeException(nameof(displayedLevel), displayedLevel,
                "Only bounded Level 19 cycle reuse is supported.");
        int cycleIndex = ((displayedLevel - 1) & 15);
        return ShapeRowsByDisplayedLevel[cycleIndex];
    }

    public static int FirstDisplayedLevelForShapeRow(int shapeRow)
    {
        int displayedLevelIndex = Array.IndexOf(ShapeRowsByDisplayedLevel.ToArray(), shapeRow);
        if (displayedLevelIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(shapeRow), shapeRow,
                "The requested ROM shape row is not mapped in the first cycle.");
        return displayedLevelIndex + 1;
    }

    public static int ShapeRowForDisplayedLevel(int displayedLevel)
    {
        if (displayedLevel is < 1)
            throw new ArgumentOutOfRangeException(nameof(displayedLevel), displayedLevel,
                "Rev. 2 tube remapping starts at displayed Level 1.");
        if (displayedLevel > 16)
            return ResolveShapeRow(displayedLevel);

        return ShapeRowsByDisplayedLevel[displayedLevel - 1];
    }
}

public static class RomPaletteCatalog
{
    // ROM-BACKED raw blocks. Host entity-role RGB remains a bounded project translation.
    public static IReadOnlyList<byte> FirstCycleRaw =>
        Array.AsReadOnly(new byte[] { 0x00, 0x04, 0x08, 0x0C, 0xC3, 0x07, 0x0B, 0x0B });
    public static IReadOnlyList<byte> SecondCycleRaw =>
        Array.AsReadOnly(new byte[] { 0x00, 0x07, 0x0B, 0x08, 0x44, 0x03, 0x0C, 0x0C });

    public static int PaletteIndexForDisplayedLevel(int displayedLevel)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(displayedLevel, 1);
        return (displayedLevel - 1) / 16;
    }

    public static IReadOnlyList<byte> RawBlockForPaletteIndex(int paletteIndex) =>
        paletteIndex switch
        {
            0 => FirstCycleRaw,
            1 => SecondCycleRaw,
            _ => throw new ArgumentOutOfRangeException(nameof(paletteIndex), paletteIndex,
                "Only the bounded Levels 1-32 palette cycles are cataloged.")
        };
}

public static class RomPresentationMetadataCatalog
{
    // ROM-BACKED raw Rev 2 metadata for displayed Level 10 / shape row 9.
    // The C# projection translation and final placement were manually accepted on 2026-11-17.
    private static readonly RomPresentationMetadata Level10 =
        new(10, 9, 0x18, 0x50, 0x20, 0xFF, 0xFF);
    // ROM-BACKED Level 11 metadata is shape row 8; it is evidence only, not a production definition.
    private static readonly RomPresentationMetadata Level11 =
        new(11, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(11),
            0x0A, 0xA0, 0x00, 0x01, 0xFF);
    // ROM-BACKED Level 12 metadata is shape row 12.
    private static readonly RomPresentationMetadata Level12 =
        new(12, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(12),
            0x18, 0x20, 0xA0, 0xFE, 0x00, 0x00, 0x04);
    // ROM-BACKED Level 13 metadata is indexed by displayed level 13; its shape row is 14.
    private static readonly RomPresentationMetadata Level13 =
        new(13, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(13),
            0x18, 0x20, 0xA0, 0xFE, 0x00, 0x00, 0x04);
    // ROM-BACKED Level 14 metadata is shape row 15; Y2D is signed post-projection placement.
    private static readonly RomPresentationMetadata Level14 =
        new(14, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(14),
            0x0C, 0xB0, 0x40, 0x01, 0xFF, 0x50, 0x04);
    // Row 10 identifies the Level 15 tube; host-space placement is calibrated separately.
    private static readonly RomPresentationMetadata Level15 =
        new(15, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(15),
            0x10, 0x90, 0x40, 0x00, 0xFF);
    // ROM-BACKED raw Level 16 metadata; host-space calibration is intentionally unset.
    private static readonly RomPresentationMetadata Level16 =
        new(16, RomTubeMappingCatalog.ShapeRowForDisplayedLevel(16),
            0x0F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x04);

    private static readonly IReadOnlyList<RomPresentationMetadata> ProductionLevels =
        Array.AsReadOnly(new[]
        {
            new RomPresentationMetadata(1, 0, 0x18, 0x50, 0x40, 0xFF, 0x00),
            new RomPresentationMetadata(2, 1, 0x1C, 0x50, 0x20, 0xFF, 0x00),
            new RomPresentationMetadata(3, 2, 0x18, 0x50, 0x40, 0xFF, 0x00),
            new RomPresentationMetadata(4, 3, 0x0F, 0x68, 0x80, 0xFF, 0x00),
            new RomPresentationMetadata(5, 4, 0x18, 0x50, 0x40, 0xFF, 0x00),
            new RomPresentationMetadata(6, 5, 0x18, 0x50, 0x40, 0xFF, 0x00),
            new RomPresentationMetadata(7, 6, 0x18, 0x68, 0x70, 0xFF, 0x00),
            new RomPresentationMetadata(8, 7, 0x18, 0xB0, 0x60, 0x00, 0xFF),
            new RomPresentationMetadata(9, 13, 0x0C, 0xB0, 0x40, 0x01, 0xFF)
        });

    public static IReadOnlyList<RomPresentationMetadata> ProductionLevels1To9 => ProductionLevels;

    public static RomPresentationMetadata ForDisplayedLevel(int displayedLevel)
    {
        if (displayedLevel == Level10.DisplayedLevel)
            return Level10;
        if (displayedLevel == Level11.DisplayedLevel)
            return Level11;
        if (displayedLevel == Level12.DisplayedLevel)
            return Level12;
        if (displayedLevel == Level13.DisplayedLevel)
            return Level13;
        if (displayedLevel == Level14.DisplayedLevel)
            return Level14;
        if (displayedLevel == Level15.DisplayedLevel)
            return Level15;
        if (displayedLevel == Level16.DisplayedLevel)
            return Level16;
        if (displayedLevel < 1)
            throw new ArgumentOutOfRangeException(
                nameof(displayedLevel),
                displayedLevel,
                "ROM presentation metadata starts at displayed Level 1.");
        if (displayedLevel > 16)
        {
            int shapeRow = RomTubeMappingCatalog.ResolveShapeRow(displayedLevel);
            RomPresentationMetadata canonicalShape = ProductionLevels.SingleOrDefault(
                metadata => metadata.TubeTableIndex == shapeRow)
                ?? throw new ArgumentOutOfRangeException(
                    nameof(displayedLevel), displayedLevel,
                    $"No canonical production metadata is cataloged for shape row {shapeRow}.");
            return new RomPresentationMetadata(
                displayedLevel,
                shapeRow,
                canonicalShape.ScaleRaw,
                canonicalShape.CameraHeightRaw,
                canonicalShape.ScreenYOffsetLowRaw,
                canonicalShape.ScreenYOffsetHighRaw,
                canonicalShape.OpenRaw,
                canonicalShape.FlipperScaleLowRaw,
                canonicalShape.FlipperScaleHighRaw);
        }

        return ProductionLevels[displayedLevel - 1];
    }
}
