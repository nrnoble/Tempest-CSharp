namespace Tempest.Game.Levels;

public readonly record struct RomFlipperAnglePath
{
    private const int PhaseCount = 16;
    private const float DegreesPerPhase = 360f / PhaseCount;

    private RomFlipperAnglePath(
        int sourceLane,
        int destinationLane,
        bool romDirectionBitSet,
        int startPhase,
        int targetPhase,
        int phaseDirection,
        int phaseSteps)
    {
        SourceLane = sourceLane;
        DestinationLane = destinationLane;
        RomDirectionBitSet = romDirectionBitSet;
        StartPhase = startPhase;
        TargetPhase = targetPhase;
        PhaseDirection = phaseDirection;
        PhaseSteps = phaseSteps;
        HostSweepDegrees = -phaseDirection * phaseSteps * DegreesPerPhase;
    }

    public int SourceLane { get; }
    public int DestinationLane { get; }
    public bool RomDirectionBitSet { get; }
    public int StartPhase { get; }
    public int TargetPhase { get; }
    public int PhaseDirection { get; }
    public int PhaseSteps { get; }
    public float HostSweepDegrees { get; }

    internal static RomFlipperAnglePath Create(
        int sourceLane,
        int destinationLane,
        int direction,
        IReadOnlyList<byte> laneAngles)
    {
        ArgumentNullException.ThrowIfNull(laneAngles);
        if (direction is not (-1 or 1))
            throw new ArgumentOutOfRangeException(nameof(direction));
        if ((uint)sourceLane >= (uint)laneAngles.Count ||
            (uint)destinationLane >= (uint)laneAngles.Count)
            throw new ArgumentOutOfRangeException(nameof(sourceLane));

        bool romDirectionBitSet = direction < 0;
        int sourcePhase = laneAngles[sourceLane];
        int destinationPhase = laneAngles[destinationLane];
        if (sourcePhase >= PhaseCount || destinationPhase >= PhaseCount)
            throw new InvalidOperationException("ROM Flipper lane angles must be 4-bit phases.");

        int startPhase = romDirectionBitSet
            ? (sourcePhase + PhaseCount / 2) % PhaseCount
            : sourcePhase;
        int targetPhase = romDirectionBitSet
            ? destinationPhase
            : (destinationPhase + PhaseCount / 2) % PhaseCount;
        int phaseDirection = romDirectionBitSet ? -1 : 1;
        int phaseSteps = phaseDirection > 0
            ? (targetPhase - startPhase + PhaseCount) % PhaseCount
            : (startPhase - targetPhase + PhaseCount) % PhaseCount;
        if (phaseSteps == 0)
            throw new InvalidOperationException("A ROM Flipper transition must advance its angle phase.");

        return new RomFlipperAnglePath(
            sourceLane,
            destinationLane,
            romDirectionBitSet,
            startPhase,
            targetPhase,
            phaseDirection,
            phaseSteps);
    }
}
