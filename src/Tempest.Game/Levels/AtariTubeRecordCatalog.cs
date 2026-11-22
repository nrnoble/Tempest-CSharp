using System.Collections.ObjectModel;

namespace Tempest.Game.Levels;

public sealed class AtariTubeRecord
{
    public const int RequiredPointCount = 16;

    private readonly ReadOnlyCollection<byte> rawXPoints;
    private readonly ReadOnlyCollection<byte> rawYPoints;

    public AtariTubeRecord(
        int recordIndex,
        IEnumerable<byte> rawXPoints,
        IEnumerable<byte> rawYPoints,
        byte scaleRaw,
        byte cameraHeightRaw,
        short screenYOffset,
        byte openRaw)
    {
        if (recordIndex is < 0 or > 15)
            throw new ArgumentOutOfRangeException(nameof(recordIndex));
        if (scaleRaw == 0)
            throw new ArgumentOutOfRangeException(nameof(scaleRaw), "Atari lev_scale must be nonzero.");
        if (openRaw is not (0x00 or 0xFF))
            throw new ArgumentOutOfRangeException(
                nameof(openRaw), openRaw, "Atari lev_open must be 0x00 (closed) or 0xFF (open).");

        ArgumentNullException.ThrowIfNull(rawXPoints);
        ArgumentNullException.ThrowIfNull(rawYPoints);

        byte[] xPoints = rawXPoints.ToArray();
        byte[] yPoints = rawYPoints.ToArray();
        if (xPoints.Length != RequiredPointCount)
            throw new ArgumentException(
                $"Atari tube records must contain exactly {RequiredPointCount} X points.",
                nameof(rawXPoints));
        if (yPoints.Length != RequiredPointCount)
            throw new ArgumentException(
                $"Atari tube records must contain exactly {RequiredPointCount} Y points.",
                nameof(rawYPoints));

        RecordIndex = recordIndex;
        this.rawXPoints = Array.AsReadOnly(xPoints);
        this.rawYPoints = Array.AsReadOnly(yPoints);
        ScaleRaw = scaleRaw;
        CameraHeightRaw = cameraHeightRaw;
        ScreenYOffset = screenYOffset;
        OpenRaw = openRaw;
    }

    public int RecordIndex { get; }
    public IReadOnlyList<byte> RawXPoints => rawXPoints;
    public IReadOnlyList<byte> RawYPoints => rawYPoints;
    public byte ScaleRaw { get; }
    public byte CameraHeightRaw { get; }
    public short ScreenYOffset { get; }
    public byte OpenRaw { get; }
    public bool IsOpen => OpenRaw == 0xFF;
    public int PointCount => rawXPoints.Count;
}

public static class AtariTubeRecordCatalog
{
    // ROM-BACKED Rev 2 K1 row 06; these bytes are independent of C# LevelDefinition geometry.
    private static readonly AtariTubeRecord Level7Record = CreateLevel7Record();

    public static AtariTubeRecord Level7Record06 => Level7Record;
    // ROM-BACKED Rev 2 shape row 08 for displayed Level 11.
    private static readonly AtariTubeRecord Level11Record =
        new(
            recordIndex: 8,
            rawXPoints:
            [
                0x10, 0x1E, 0x2D, 0x3C, 0x4B, 0x5A, 0x69, 0x78,
                0x87, 0x96, 0xA5, 0xB4, 0xC3, 0xD2, 0xE1, 0xF0
            ],
            rawYPoints:
            [
                0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40,
                0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40
            ],
            scaleRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(11).ScaleRaw,
            cameraHeightRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(11).CameraHeightRaw,
            screenYOffset: RomPresentationMetadataCatalog.ForDisplayedLevel(11).ScreenYOffsetSigned,
            openRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(11).OpenRaw);

    public static AtariTubeRecord Level11Record08 => Level11Record;

    // DEVELOPMENT ONLY: manually identified Level 15 tube, selected as Rev. 2 shape row 10.
    private static readonly AtariTubeRecord Level15Record =
        new(
            recordIndex: 10,
            rawXPoints:
            [
                0x10, 0x24, 0x30, 0x36, 0x3E, 0x49, 0x5A, 0x75,
                0x94, 0xA4, 0xAC, 0xBA, 0xDA, 0xE2, 0xEA, 0xF0
            ],
            rawYPoints:
            [
                0xC0, 0xA6, 0x8A, 0x6A, 0x4A, 0x2F, 0x14, 0x24,
                0x20, 0x39, 0x59, 0x75, 0x72, 0x90, 0xB0, 0xD0
            ],
            scaleRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(15).ScaleRaw,
            cameraHeightRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(15).CameraHeightRaw,
            screenYOffset: RomPresentationMetadataCatalog.ForDisplayedLevel(15).ScreenYOffsetSigned,
            openRaw: RomPresentationMetadataCatalog.ForDisplayedLevel(15).OpenRaw);

    public static AtariTubeRecord Level15Record10 => Level15Record;

    public static AtariTubeRecord ForRecordIndex(int recordIndex) =>
        recordIndex == Level7Record.RecordIndex
            ? Level7Record
            : recordIndex == Level11Record.RecordIndex
                ? Level11Record
                : recordIndex == Level15Record.RecordIndex
                    ? Level15Record
                    : throw new ArgumentOutOfRangeException(
                        nameof(recordIndex), recordIndex,
                        "Only Atari tube records 06, 08 and 10 are cataloged.");

    private static AtariTubeRecord CreateLevel7Record()
    {
        RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(7);
        if (metadata.TubeTableIndex != 6)
            throw new InvalidOperationException("Displayed Level 7 must select Atari tube record 06.");

        return new AtariTubeRecord(
            recordIndex: 6,
            rawXPoints:
            [
                0xEA, 0xE0, 0x9C, 0x80, 0x64, 0x20, 0x16, 0x50,
                0x16, 0x20, 0x64, 0x80, 0x9C, 0xE0, 0xEA, 0xB0
            ],
            rawYPoints:
            [
                0xA0, 0xE0, 0xEA, 0xB0, 0xEA, 0xE0, 0xA0, 0x80,
                0x60, 0x20, 0x16, 0x50, 0x16, 0x20, 0x60, 0x80
            ],
            scaleRaw: metadata.ScaleRaw,
            cameraHeightRaw: metadata.CameraHeightRaw,
            screenYOffset: metadata.ScreenYOffsetSigned,
            openRaw: metadata.OpenRaw);
    }
}
