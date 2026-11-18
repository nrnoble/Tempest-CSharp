using System.Numerics;

namespace Tempest.Game.Rendering;

/// <summary>
/// Perspective projection shared by all rendering. Larger z shrinks x/y by K/z. ZOffset keeps z
/// positive at the near rim; K and ZOffset are animated by the exit/reverse zoom effects. The
/// portrait calibration interpolates its stable near/far displacement affinely in live reciprocal
/// depth. Global mutable state, so zoom code must restore it when finished.
/// </summary>
public static class Projection
{
    public const float Width = DisplayConfiguration.LogicalWidth;
    public const float Height = DisplayConfiguration.LogicalHeight;
    public static float K { get; set; } = 65;
    public static float ZOffset { get; set; } = 5;
    public const float PortraitPerspectiveDefault = 0;
    public const float PortraitPerspectiveBaselineK = 70;
    public const float PortraitPerspectiveBaselineZOffset = 5;

    private static float portraitPerspective;

    // PROJECT CALIBRATION CANDIDATE: positive values move farther points upward on screen; zero
    // preserves the pre-experiment projection. This is presentation state, not gameplay state.
    public static float PortraitPerspective
    {
        get => portraitPerspective;
        set
        {
            if (!float.IsFinite(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Portrait perspective must be finite.");
            }

            portraitPerspective = value;
        }
    }

    public static Vector2 Project(
        float x,
        float y,
        float z,
        float centerY,
        float yOffset,
        float? nearDepth = null,
        float? farDepth = null,
        float? projectionK = null,
        float? projectionZOffset = null)
    {
        ValidateFinite(x, nameof(x));
        ValidateFinite(y, nameof(y));
        ValidateFinite(z, nameof(z));
        ValidateFinite(centerY, nameof(centerY));
        ValidateFinite(yOffset, nameof(yOffset));
        float k = projectionK ?? K;
        float zOffset = projectionZOffset ?? ZOffset;
        if (!float.IsFinite(k) || k <= 0)
            throw new InvalidOperationException("Projection K must be positive and finite.");
        if (!float.IsFinite(zOffset))
            throw new InvalidOperationException("Projection ZOffset must be finite.");
        if (nearDepth.HasValue != farDepth.HasValue)
            throw new ArgumentException("Near and far projection depths must be provided together.");
        if (PortraitPerspective != 0 && !nearDepth.HasValue)
            throw new InvalidOperationException(
                "Portrait perspective projection requires active near and far depth context.");

        float liveDepth = z + zOffset;
        float depthScale = liveDepth / k;
        if (!float.IsFinite(depthScale) || depthScale == 0)
            throw new ArgumentOutOfRangeException(nameof(z),
                "The projected depth must have a finite, non-zero camera denominator.");
        float liveNearDepth = 0;
        float liveFarDepth = 0;
        if (nearDepth is float activeNearDepth && farDepth is float activeFarDepth)
        {
            ValidateFinite(activeNearDepth, nameof(nearDepth));
            ValidateFinite(activeFarDepth, nameof(farDepth));
            liveNearDepth = activeNearDepth + zOffset;
            liveFarDepth = activeFarDepth + zOffset;
            if (!float.IsFinite(liveNearDepth) || liveNearDepth == 0)
                throw new ArgumentOutOfRangeException(nameof(nearDepth),
                    "The near depth must have a finite, non-zero camera denominator.");
            if (!float.IsFinite(liveFarDepth) || liveFarDepth == 0)
                throw new ArgumentOutOfRangeException(nameof(farDepth),
                    "The far depth must have a finite, non-zero camera denominator.");
            if (liveNearDepth == liveFarDepth)
                throw new ArgumentException("The live near and far depths must define an interval.");
        }

        float projectedY = centerY + (y + yOffset) / depthScale;
        if (PortraitPerspective != 0)
        {
            // Share the live reciprocal-depth parameter so the corrected tube rail stays straight.
            float qLive = Reciprocal(liveDepth, nameof(z));
            float qLiveNear = Reciprocal(liveNearDepth, nameof(nearDepth));
            float qLiveFar = Reciprocal(liveFarDepth, nameof(farDepth));
            float liveInterval = qLiveNear - qLiveFar;
            if (!float.IsFinite(liveInterval) || liveInterval == 0)
                throw new ArgumentException("The live near and far depths must define a finite interval.");

            float qBaselineNear = Reciprocal(
                nearDepth!.Value + PortraitPerspectiveBaselineZOffset, nameof(nearDepth));
            float qBaselineFar = Reciprocal(
                farDepth!.Value + PortraitPerspectiveBaselineZOffset, nameof(farDepth));
            float baselineInterval = qBaselineFar - qBaselineNear;
            if (!float.IsFinite(baselineInterval) || baselineInterval == 0)
                throw new ArgumentException("The baseline near and far depths must define a finite interval.");

            float projectiveFraction = (qLiveNear - qLive) / liveInterval;
            float deltaY = PortraitPerspective * PortraitPerspectiveBaselineK *
                baselineInterval * projectiveFraction;
            projectedY += deltaY;
        }

        Vector2 projected = new(Width / 2f + x / depthScale, projectedY);
        if (!float.IsFinite(projected.X) || !float.IsFinite(projected.Y))
            throw new InvalidOperationException("Projection produced a non-finite coordinate.");
        return projected;
    }

    private static float Reciprocal(float value, string parameterName)
    {
        if (!float.IsFinite(value) || value == 0)
            throw new ArgumentOutOfRangeException(parameterName,
                "A projection depth denominator must be finite and non-zero.");
        float reciprocal = 1 / value;
        if (!float.IsFinite(reciprocal))
            throw new ArgumentOutOfRangeException(parameterName,
                "A projection depth reciprocal must be finite.");
        return reciprocal;
    }

    private static void ValidateFinite(float value, string parameterName)
    {
        if (!float.IsFinite(value))
            throw new ArgumentOutOfRangeException(parameterName, "Projection inputs must be finite.");
    }
}
