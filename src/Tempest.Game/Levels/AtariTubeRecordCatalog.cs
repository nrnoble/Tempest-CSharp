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

    public static AtariTubeRecord ForRecordIndex(int recordIndex) =>
        recordIndex == Level7Record.RecordIndex
            ? Level7Record
            : throw new ArgumentOutOfRangeException(
                nameof(recordIndex), recordIndex, "Only Atari tube record 06 is cataloged.");

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
