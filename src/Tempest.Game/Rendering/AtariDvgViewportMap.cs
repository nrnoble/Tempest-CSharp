using System.Numerics;

namespace Tempest.Game.Rendering;

public readonly struct AtariDvgViewportMap
{
    public const float ProvisionalGlobalScale = 0.4f;

    public AtariDvgViewportMap(
        Vector2 translation,
        float globalScale,
        float displayAspect,
        float rotationDegrees,
        bool swapAxes,
        int xSign,
        int ySign)
    {
        if (!float.IsFinite(translation.X) || !float.IsFinite(translation.Y))
            throw new ArgumentOutOfRangeException(nameof(translation));
        if (!float.IsFinite(globalScale) || globalScale <= 0)
            throw new ArgumentOutOfRangeException(nameof(globalScale));
        if (!float.IsFinite(displayAspect) || displayAspect <= 0)
            throw new ArgumentOutOfRangeException(nameof(displayAspect));
        if (!float.IsFinite(rotationDegrees))
            throw new ArgumentOutOfRangeException(nameof(rotationDegrees));
        if (xSign is not (-1 or 1))
            throw new ArgumentOutOfRangeException(nameof(xSign));
        if (ySign is not (-1 or 1))
            throw new ArgumentOutOfRangeException(nameof(ySign));

        Translation = translation;
        GlobalScale = globalScale;
        DisplayAspect = displayAspect;
        RotationDegrees = rotationDegrees;
        SwapAxes = swapAxes;
        XSign = xSign;
        YSign = ySign;
    }

    public Vector2 Translation { get; }
    public float GlobalScale { get; }
    public float DisplayAspect { get; }
    public float RotationDegrees { get; }
    public bool SwapAxes { get; }
    public int XSign { get; }
    public int YSign { get; }

    // PROJECT TRANSLATION / APPROXIMATION: Atari evidence leaves physical sign, rotation, gain,
    // and aspect unknown; these are shared portrait starting values, not tube calibration.
    public static AtariDvgViewportMap DevelopmentOnlyProvisional => new(
        new Vector2(
            DisplayConfiguration.PortraitLogicalWidth / 2f,
            DisplayConfiguration.PortraitLogicalHeight / 2f),
        globalScale: ProvisionalGlobalScale,
        displayAspect: 1.0f,
        rotationDegrees: 0,
        swapAxes: false,
        xSign: 1,
        ySign: -1);

    public Vector2 Map(AtariDvgPoint point)
    {
        if (!float.IsFinite(Translation.X) || !float.IsFinite(Translation.Y) ||
            !float.IsFinite(GlobalScale) || GlobalScale <= 0 ||
            !float.IsFinite(DisplayAspect) || DisplayAspect <= 0 ||
            !float.IsFinite(RotationDegrees) ||
            XSign is not (-1 or 1) || YSign is not (-1 or 1))
        {
            throw new InvalidOperationException("The Atari DVG viewport map is not initialized with valid values.");
        }

        float sourceX = (SwapAxes ? point.Y : point.X) * XSign *
            GlobalScale * DisplayAspect;
        float sourceY = (SwapAxes ? point.X : point.Y) * YSign * GlobalScale;
        float radians = RotationDegrees * (MathF.PI / 180f);
        float cosine = MathF.Cos(radians);
        float sine = MathF.Sin(radians);
        Vector2 mapped = Translation + new Vector2(
            sourceX * cosine - sourceY * sine,
            sourceX * sine + sourceY * cosine);
        if (!float.IsFinite(mapped.X) || !float.IsFinite(mapped.Y))
            throw new ArgumentOutOfRangeException(nameof(point), "The DVG viewport mapping is not finite.");
        return mapped;
    }
}
