using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public readonly record struct AtariTubeVectorSegment(AtariDvgPoint Start, AtariDvgPoint End);

public sealed class AtariTubeVectorList
{
    internal AtariTubeVectorList(
        int recordIndex,
        int nearRuntimePosition,
        int farRuntimePosition,
        AtariDvgPoint[] nearPoints,
        AtariDvgPoint[] farPoints,
        AtariTubeVectorSegment[] rails,
        AtariTubeVectorSegment[] nearRim,
        AtariTubeVectorSegment[] farRim)
    {
        RecordIndex = recordIndex;
        NearRuntimePosition = nearRuntimePosition;
        FarRuntimePosition = farRuntimePosition;
        NearPoints = Array.AsReadOnly(nearPoints);
        FarPoints = Array.AsReadOnly(farPoints);
        Rails = Array.AsReadOnly(rails);
        NearRim = Array.AsReadOnly(nearRim);
        FarRim = Array.AsReadOnly(farRim);
    }

    public int RecordIndex { get; }
    public int NearRuntimePosition { get; }
    public int FarRuntimePosition { get; }
    public AtariDvgPoint DvgCenter => new(0, 0);
    public ushort DvgCenterWord => 0x8040;
    public ushort DvgVectorScaleWord => 0x7100;
    public IReadOnlyList<AtariDvgPoint> NearPoints { get; }
    public IReadOnlyList<AtariDvgPoint> FarPoints { get; }
    public IReadOnlyList<AtariTubeVectorSegment> Rails { get; }
    public IReadOnlyList<AtariTubeVectorSegment> NearRim { get; }
    public IReadOnlyList<AtariTubeVectorSegment> FarRim { get; }
}

public static class AtariTubeVectorListBuilder
{
    public const int NormalNearRuntimePosition = 16;
    public const int NormalFarRuntimePosition = 240;

    public static AtariTubeVectorList Build(
        AtariTubeRecord record,
        bool useRn000fDiagnostic = false)
    {
        ArgumentNullException.ThrowIfNull(record);
        AtariDvgPoint[] near = ProjectSection(record, NormalNearRuntimePosition, useRn000fDiagnostic);
        AtariDvgPoint[] far = ProjectSection(record, NormalFarRuntimePosition, useRn000fDiagnostic);
        AtariTubeVectorSegment[] rails = new AtariTubeVectorSegment[record.PointCount];
        int rimEdgeCount = record.IsOpen ? record.PointCount - 1 : record.PointCount;
        AtariTubeVectorSegment[] nearRim = new AtariTubeVectorSegment[rimEdgeCount];
        AtariTubeVectorSegment[] farRim = new AtariTubeVectorSegment[rimEdgeCount];

        for (int index = 0; index < record.PointCount; index++)
        {
            rails[index] = new AtariTubeVectorSegment(near[index], far[index]);
            if (index >= rimEdgeCount)
                continue;

            int next = (index + 1) % record.PointCount;
            nearRim[index] = new AtariTubeVectorSegment(near[index], near[next]);
            farRim[index] = new AtariTubeVectorSegment(far[index], far[next]);
        }

        return new AtariTubeVectorList(
            record.RecordIndex,
            NormalNearRuntimePosition,
            NormalFarRuntimePosition,
            near,
            far,
            rails,
            nearRim,
            farRim);
    }

    private static AtariDvgPoint[] ProjectSection(
        AtariTubeRecord record,
        int runtimePosition,
        bool useRn000fDiagnostic)
    {
        AtariDvgPoint[] points = new AtariDvgPoint[record.PointCount];
        for (int index = 0; index < record.PointCount; index++)
        {
            AtariCoordinateProjection projection = AtariCoordinateProjector.Project(
                record,
                record.RawXPoints[index],
                record.RawYPoints[index],
                runtimePosition,
                xBase: 0,
                yBase: record.ScreenYOffset,
                AtariQuotientCountState.PrototypeAssumption,
                useRn000fDiagnostic);
            points[index] = projection.DvgPoint;
        }

        return points;
    }
}
