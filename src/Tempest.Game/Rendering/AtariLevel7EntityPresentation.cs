using System.Numerics;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public readonly record struct AtariLevel7LaneGeometry(
    int GameplayLane,
    int PresentationLane,
    float NormalizedDepth,
    Vector2 Start,
    Vector2 End,
    Vector2 Center,
    Vector2 Tangent,
    Vector2 Normal,
    float LocalScale);

/// <summary>
/// Render-only registration from existing Level 7 state to record-06 geometry used only for
/// visible presentation. Gameplay geometry remains authoritative for simulation and collision.
/// </summary>
public static class AtariLevel7EntityPresentation
{
    private const float GeometryEpsilon = 0.000001f;

    public static int MapGameplayLaneToPresentationLane(int gameplayLane)
    {
        // PROJECT TRANSLATION candidate: identity lane correspondence is explicit, not verified.
        if ((uint)gameplayLane >= (uint)AtariLevel7Presentation.NearPoints.Count)
            throw new ArgumentOutOfRangeException(nameof(gameplayLane));

        return gameplayLane;
    }

    public static float NormalizeDepth(
        float projectedDepth,
        float nearDepth,
        float depthRange)
    {
        if (!float.IsFinite(projectedDepth))
            throw new ArgumentOutOfRangeException(nameof(projectedDepth));
        if (!float.IsFinite(nearDepth))
            throw new ArgumentOutOfRangeException(nameof(nearDepth));
        if (!float.IsFinite(depthRange) || depthRange <= 0)
            throw new ArgumentOutOfRangeException(nameof(depthRange));

        return (projectedDepth - nearDepth) / depthRange;
    }

    public static AtariLevel7LaneGeometry MapLaneAtDepth(int gameplayLane, float normalizedDepth)
    {
        if (!float.IsFinite(normalizedDepth))
            throw new ArgumentOutOfRangeException(nameof(normalizedDepth));

        int presentationLane = MapGameplayLaneToPresentationLane(gameplayLane);
        int nextLane = (presentationLane + 1) % AtariLevel7Presentation.NearPoints.Count;
        Vector2 start = InterpolateVertex(presentationLane, normalizedDepth);
        Vector2 end = InterpolateVertex(nextLane, normalizedDepth);
        Vector2 edge = end - start;
        float edgeLength = edge.Length();
        if (!float.IsFinite(edgeLength) || edgeLength <= GeometryEpsilon)
            throw new InvalidOperationException("The mapped Level 7 lane has degenerate presentation geometry.");

        float nearEdgeLength = Vector2.Distance(
            AtariLevel7Presentation.NearPoints[presentationLane],
            AtariLevel7Presentation.NearPoints[nextLane]);
        float localScale = edgeLength / nearEdgeLength;
        if (!float.IsFinite(localScale))
            throw new InvalidOperationException("The mapped Level 7 lane scale is not finite.");

        Vector2 tangent = edge / edgeLength;
        return new AtariLevel7LaneGeometry(
            gameplayLane,
            presentationLane,
            normalizedDepth,
            start,
            end,
            (start + end) / 2,
            tangent,
            new Vector2(-tangent.Y, tangent.X),
            localScale);
    }

    public static AtariLevel7LaneGeometry MapLaneAtProjectedDepth(
        int gameplayLane,
        float projectedDepth,
        float nearDepth,
        float depthRange) =>
        MapLaneAtDepth(
            gameplayLane,
            NormalizeDepth(projectedDepth, nearDepth, depthRange));

    public static Vector2 MapVertexAtDepth(int presentationVertex, float normalizedDepth)
    {
        if ((uint)presentationVertex >= (uint)AtariLevel7Presentation.NearPoints.Count)
            throw new ArgumentOutOfRangeException(nameof(presentationVertex));
        if (!float.IsFinite(normalizedDepth))
            throw new ArgumentOutOfRangeException(nameof(normalizedDepth));

        return InterpolateVertex(presentationVertex, normalizedDepth);
    }

    public static (Vector2 Start, Vector2 End) MapFlipperTransition(
        DirectedLaneTransition transition,
        float progress,
        float normalizedDepth)
    {
        if (!float.IsFinite(progress))
            throw new ArgumentOutOfRangeException(nameof(progress));
        if (!float.IsFinite(normalizedDepth))
            throw new ArgumentOutOfRangeException(nameof(normalizedDepth));

        ValidateTransition(transition);
        progress = Math.Clamp(progress, 0, 1);
        if (progress == 0)
        {
            AtariLevel7LaneGeometry source = MapLaneAtDepth(
                transition.SourceLane, normalizedDepth);
            return (source.Start, source.End);
        }

        Vector2 pivot = InterpolateVertex(transition.HingeVertex, normalizedDepth);
        Vector2 moving = InterpolateVertex(transition.SourceFreeVertex, normalizedDepth);
        Vector2 destination = InterpolateVertex(transition.DestinationFreeVertex, normalizedDepth);
        if (progress == 1)
            return transition.HingeIsStart
                ? (pivot, destination)
                : (destination, pivot);

        Vector2 initialVector = moving - pivot;
        Vector2 destinationVector = destination - pivot;
        // PROJECT TRANSLATION candidate: record-06 can reverse a gameplay hinge's convexity.
        // Map the sweep between visible lane rays instead of clipping it to the whole outline.
        float mappedSweepDegrees = CalculateMappedSweepDegrees(
            initialVector, destinationVector, transition.SpatialSweepDegrees);
        float angle = mappedSweepDegrees * progress * MathF.PI / 180;
        float sin = MathF.Sin(angle);
        float cos = MathF.Cos(angle);
        Vector2 rotated = new(
            initialVector.X * cos - initialVector.Y * sin,
            initialVector.X * sin + initialVector.Y * cos);
        float rotatedLength = rotated.Length();
        if (rotatedLength <= GeometryEpsilon)
            throw new InvalidOperationException("The mapped Level 7 Flipper pivot is degenerate.");

        float radius = initialVector.Length() +
            (destinationVector.Length() - initialVector.Length()) * progress;
        Vector2 movedEndpoint = pivot + rotated / rotatedLength * radius;
        return transition.HingeIsStart
            ? (pivot, movedEndpoint)
            : (movedEndpoint, pivot);
    }

    private static void ValidateTransition(DirectedLaneTransition transition)
    {
        int pointCount = AtariLevel7Presentation.NearPoints.Count;
        MapGameplayLaneToPresentationLane(transition.SourceLane);
        MapGameplayLaneToPresentationLane(transition.DestinationLane);
        if (transition.Direction is not (-1 or 1))
            throw new ArgumentException("The transition direction is invalid.", nameof(transition));

        int expectedDestination = (transition.SourceLane +
            (transition.Direction > 0 ? pointCount - 1 : 1)) % pointCount;
        int expectedHinge = transition.Direction > 0
            ? transition.SourceLane
            : (transition.SourceLane + 1) % pointCount;
        int expectedSourceFree = transition.Direction > 0
            ? (transition.SourceLane + 1) % pointCount
            : transition.SourceLane;
        int expectedDestinationFree = transition.Direction > 0
            ? (transition.SourceLane + pointCount - 1) % pointCount
            : (expectedHinge + 1) % pointCount;
        bool expectedHingeIsStart = transition.Direction < 0;
        if (transition.DestinationLane != expectedDestination ||
            transition.HingeVertex != expectedHinge ||
            transition.SourceFreeVertex != expectedSourceFree ||
            transition.DestinationFreeVertex != expectedDestinationFree ||
            transition.HingeIsStart != expectedHingeIsStart ||
            !float.IsFinite(transition.SignedSweepDegrees) ||
            !float.IsFinite(transition.SpatialSweepDegrees) ||
            MathF.Abs(transition.SpatialSweepDegrees) <= GeometryEpsilon)
        {
            throw new ArgumentException(
                "The transition does not match Level 7 directed lane geometry.", nameof(transition));
        }
    }

    private static float CalculateMappedSweepDegrees(
        Vector2 initialVector,
        Vector2 destinationVector,
        float directedSpatialSweepDegrees)
    {
        if (initialVector.LengthSquared() <= GeometryEpsilon * GeometryEpsilon ||
            destinationVector.LengthSquared() <= GeometryEpsilon * GeometryEpsilon)
            throw new InvalidOperationException("The mapped Level 7 Flipper pivot is degenerate.");

        float shortestSweep = MathF.Atan2(
            Cross(initialVector, destinationVector),
            Vector2.Dot(initialVector, destinationVector)) * 180 / MathF.PI;
        if (!float.IsFinite(shortestSweep))
            throw new InvalidOperationException("The mapped Level 7 Flipper sweep is not finite.");

        // Preserve the simulation's long/short route choice while landing on the mapped ray.
        if (MathF.Abs(MathF.Abs(shortestSweep) - 180) <= 0.0001f)
            return MathF.CopySign(180, directedSpatialSweepDegrees);

        if (MathF.Abs(directedSpatialSweepDegrees) > 180)
        {
            if (MathF.Abs(shortestSweep) <= GeometryEpsilon)
                return MathF.CopySign(360, directedSpatialSweepDegrees);

            return shortestSweep - MathF.CopySign(360, shortestSweep);
        }

        return shortestSweep;
    }

    private static Vector2 InterpolateVertex(int vertex, float normalizedDepth) =>
        Vector2.Lerp(
            AtariLevel7Presentation.NearPoints[vertex],
            AtariLevel7Presentation.FarPoints[vertex],
            normalizedDepth);

    private static float Cross(Vector2 first, Vector2 second) =>
        first.X * second.Y - first.Y * second.X;
}
