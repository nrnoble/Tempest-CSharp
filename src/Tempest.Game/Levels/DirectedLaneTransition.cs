namespace Tempest.Game.Levels;

public readonly record struct DirectedLaneTransition
{
    internal DirectedLaneTransition(
        int sourceLane,
        int destinationLane,
        int direction,
        int hingeVertex,
        int sourceFreeVertex,
        int destinationFreeVertex,
        bool hingeIsStart,
        float signedSweepDegrees,
        float spatialSweepDegrees)
    {
        SourceLane = sourceLane;
        DestinationLane = destinationLane;
        Direction = direction;
        HingeVertex = hingeVertex;
        SourceFreeVertex = sourceFreeVertex;
        DestinationFreeVertex = destinationFreeVertex;
        HingeIsStart = hingeIsStart;
        SignedSweepDegrees = signedSweepDegrees;
        SpatialSweepDegrees = spatialSweepDegrees;
    }

    public int SourceLane { get; }
    public int DestinationLane { get; }
    public int Direction { get; }
    public int HingeVertex { get; }
    public int SourceFreeVertex { get; }
    public int DestinationFreeVertex { get; }
    public bool HingeIsStart { get; }
    // Keep C1.5's directed sweep separate from the spatial arc selected by board geometry.
    public float SignedSweepDegrees { get; }
    public float SpatialSweepDegrees { get; }
    public float SweepMagnitudeDegrees => MathF.Abs(SignedSweepDegrees);
}
