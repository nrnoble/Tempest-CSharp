using System.Numerics;

namespace Tempest.Game.Rendering;

// DEVELOPMENT-ONLY control for the former reference-shape diagnostic. It deliberately
// shares the production Level 7 translation so the diagnostic cannot drift.
public static class AtariTubeReferenceShapeDiagnostic
{
    public const float ReferenceHorizontalGain = AtariLevel7Presentation.ReferenceHorizontalGain;
    public const float ReferenceVerticalGain = AtariLevel7Presentation.ReferenceVerticalGain;
    public const float FarReferenceOffsetX = AtariLevel7Presentation.FarReferenceOffsetX;
    public const float FarReferenceOffsetY = AtariLevel7Presentation.FarReferenceOffsetY;
    public const float SharedHorizontalAspect = AtariLevel7Presentation.SharedHorizontalAspect;
    public const float SharedGlobalScale = AtariLevel7Presentation.SharedGlobalScale;
    public const float FarTranslationLogicalX = AtariLevel7Presentation.FarTranslationLogicalX;
    public const float FarTranslationLogicalY = AtariLevel7Presentation.FarTranslationLogicalY;

    public static Vector2 MapNear(AtariDvgPoint point) =>
        AtariLevel7Presentation.MapNear(point);

    public static Vector2 MapFar(AtariDvgPoint point) =>
        AtariLevel7Presentation.MapFar(point);
}
