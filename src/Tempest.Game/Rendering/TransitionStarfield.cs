using System.Numerics;
using Raylib_cs;

namespace Tempest.Game.Rendering;

public static class TransitionStarfield
{
    // Project presentation tuning, not measured Atari counts or timing.
    public const int MaximumStars = 288;
    public const float ExitAppearanceDepthFraction = .5f;
    public const float ExitFullVisibilityDepthFraction = .2f;
    public const float ApproachDisappearanceProgress = .85f;
    private const int Columns = 16;
    private const int Rows = MaximumStars / Columns;
    private const int VisibilityReferenceStars = 192;
    private const float StarIntensityGain = 1.6f;
    private static readonly Color[] LaterLevelColors = [Color.White, Color.Red, Color.Green, Color.Blue];

    public static float ExitVisibility(float playerDepth, float tubeDepth) =>
        SmoothStep((ExitAppearanceDepthFraction - playerDepth / tubeDepth) /
            (ExitAppearanceDepthFraction - ExitFullVisibilityDepthFraction));

    public static float ApproachVisibility(float progress) =>
        1 - SmoothStep(progress / ApproachDisappearanceProgress);

    public static void Draw(int exitedLevel, float visibility, Action<Vector2, Color> drawPoint)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exitedLevel);
        ArgumentNullException.ThrowIfNull(drawPoint);
        if (!float.IsFinite(visibility) || visibility < 0 || visibility > 1)
            throw new ArgumentOutOfRangeException(nameof(visibility));

        // Keep the accepted first/last visible frame independent of the trial density.
        if (MathF.Round(255 * visibility * VisibilityReferenceStars) == 0)
            return;

        for (int index = 0; index < MaximumStars; index++)
        {
            float opacity = Math.Clamp(visibility * MaximumStars - index, 0, 1);
            byte alpha = (byte)MathF.Round(255 * opacity);
            if (alpha == 0)
                break;

            Color color = exitedLevel <= 4 ? Color.Blue : LaterLevelColors[index % LaterLevelColors.Length];
            color.R = Intensify(color.R);
            color.G = Intensify(color.G);
            color.B = Intensify(color.B);
            color.A = alpha;
            drawPoint(Position(index), color);
        }
    }

    private static byte Intensify(byte channel) =>
        (byte)MathF.Min(255, MathF.Round(channel * StarIntensityGain));

    private static Vector2 Position(int index)
    {
        // Scramble the cell order so progressive removal does not sweep across the screen.
        int cell = index * 73 % MaximumStars;
        float jitterX = Hash((uint)(index * 2 + 1)) / (float)uint.MaxValue;
        float jitterY = Hash((uint)(index * 2 + 2)) / (float)uint.MaxValue;
        return new Vector2(
            16 + (cell % Columns + .1f + .8f * jitterX) * ((DisplayConfiguration.LogicalWidth - 32f) / Columns),
            112 + (cell / Columns + .1f + .8f * jitterY) * ((DisplayConfiguration.LogicalHeight - 128f) / Rows));
    }

    private static uint Hash(uint value)
    {
        unchecked
        {
            value = (value ^ (value >> 16)) * 0x7feb352du;
            value = (value ^ (value >> 15)) * 0x846ca68bu;
            return value ^ (value >> 16);
        }
    }

    private static float SmoothStep(float progress)
    {
        progress = Math.Clamp(progress, 0, 1);
        return progress * progress * (3 - 2 * progress);
    }
}
