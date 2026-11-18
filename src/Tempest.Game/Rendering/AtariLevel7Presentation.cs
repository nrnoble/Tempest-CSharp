using System.Numerics;
using Raylib_cs;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public static class AtariLevel7Presentation
{
    private const float ReferenceRasterWidth = 480f;
    private const float ReferenceRasterHeight = 640f;
    private const float ReferenceToPortraitScaleX =
        DisplayConfiguration.PortraitViewportWidth / ReferenceRasterWidth;
    private const float ReferenceToPortraitScaleY =
        DisplayConfiguration.PortraitViewportHeight / ReferenceRasterHeight;

    // PROJECT TRANSLATION / visually calibrated Level 7 presentation candidate;
    // these gains and translations are not ROM geometry or proven Atari hardware behavior.
    public const float ReferenceHorizontalGain = 0.4215f;
    public const float ReferenceVerticalGain = 0.575f;
    public const float FarReferenceOffsetX = 27f;
    public const float FarReferenceOffsetY = 37f;

    public const float SharedHorizontalAspect =
        ReferenceHorizontalGain / ReferenceVerticalGain;

    public const float SharedGlobalScale =
        AtariDvgViewportMap.ProvisionalGlobalScale *
        ReferenceVerticalGain *
        ReferenceToPortraitScaleY;

    public const float FarTranslationLogicalX =
        FarReferenceOffsetX * ReferenceToPortraitScaleX /
        DisplayConfiguration.PortraitOutputScale;

    public const float FarTranslationLogicalY =
        FarReferenceOffsetY * ReferenceToPortraitScaleY /
        DisplayConfiguration.PortraitOutputScale;

    private static readonly AtariDvgViewportMap NearMap = CreateMap(Vector2.Zero);
    private static readonly AtariDvgViewportMap FarMap = CreateMap(
        new Vector2(FarTranslationLogicalX, FarTranslationLogicalY));
    private static readonly AtariTubeVectorList TubeVectors =
        AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
    private static readonly Vector2[] UncenteredNearPoints = TubeVectors.NearPoints
        .Select(NearMap.Map)
        .ToArray();
    private static readonly Vector2[] UncenteredFarPoints = TubeVectors.FarPoints
        .Select(FarMap.Map)
        .ToArray();
    private static readonly Vector2 CenteringTranslation = CalculateCenteringTranslation();
    private static readonly IReadOnlyList<Vector2> MappedNearPoints = Array.AsReadOnly(
        UncenteredNearPoints.Select(point => point + CenteringTranslation).ToArray());
    private static readonly IReadOnlyList<Vector2> MappedFarPoints = Array.AsReadOnly(
        UncenteredFarPoints.Select(point => point + CenteringTranslation).ToArray());

    public static Vector2 CenteringTranslationLogical => CenteringTranslation;

    public static IReadOnlyList<Vector2> NearPoints => MappedNearPoints;

    public static IReadOnlyList<Vector2> FarPoints => MappedFarPoints;

    public static Vector2 MapNear(AtariDvgPoint point) =>
        NearMap.Map(point);

    public static Vector2 MapFar(AtariDvgPoint point) =>
        FarMap.Map(point);

    public static Vector2 MapNearCentered(AtariDvgPoint point) =>
        MapNear(point) + CenteringTranslation;

    public static Vector2 MapFarCentered(AtariDvgPoint point) =>
        MapFar(point) + CenteringTranslation;

    private static Vector2 CalculateCenteringTranslation()
    {
        Vector2 minimum = new(float.PositiveInfinity);
        Vector2 maximum = new(float.NegativeInfinity);
        foreach (Vector2 point in UncenteredNearPoints.Concat(UncenteredFarPoints))
        {
            minimum = Vector2.Min(minimum, point);
            maximum = Vector2.Max(maximum, point);
        }

        Vector2 currentBoundsCenter = (minimum + maximum) / 2;
        Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
        float persistentHudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
        Vector2 gameplayCompositionCenter = new(
            DisplayConfiguration.WindowWidth / 2f,
            (persistentHudBottom + DisplayConfiguration.PortraitViewportHeight) / 2f);
        Vector2 logicalCompositionCenter =
            (gameplayCompositionCenter - camera.Offset) / camera.Zoom + camera.Target;

        // PROJECT TRANSLATION candidate; Neal's manual Level 7 presentation acceptance is pending.
        return logicalCompositionCenter - currentBoundsCenter;
    }

    private static AtariDvgViewportMap CreateMap(Vector2 relativeTranslation)
    {
        AtariDvgViewportMap provisional = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        return new AtariDvgViewportMap(
            provisional.Translation + relativeTranslation,
            SharedGlobalScale,
            SharedHorizontalAspect,
            provisional.RotationDegrees,
            provisional.SwapAxes,
            provisional.XSign,
            provisional.YSign);
    }
}
