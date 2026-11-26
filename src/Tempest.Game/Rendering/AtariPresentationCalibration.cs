using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

/// <summary>
/// First-pass Atari-presentation defaults. Levels 1-16 are manually accepted
/// project decisions; repeated cycles reuse calibration by canonical shape.
/// Diagnostic values remain absolute overrides.
/// </summary>
public readonly record struct AtariPresentationCalibration(
	float TubeYGain,
	float TubeScale,
	float VerticalOffset,
	float TubeDepthYGain = 1f)
{
	public static AtariPresentationCalibration Resolve(
		int displayedLevel,
		float? tubeYGainOverride = null,
		float? tubeScaleOverride = null,
		float? verticalOffsetOverride = null,
		float? tubeDepthYGainOverride = null)
	{
		AtariPresentationCalibration selected = ForDisplayedLevel(displayedLevel);
		return new(
			tubeYGainOverride ?? selected.TubeYGain,
			tubeScaleOverride ?? selected.TubeScale,
			verticalOffsetOverride ?? selected.VerticalOffset,
			tubeDepthYGainOverride ?? selected.TubeDepthYGain);
	}

	public static AtariPresentationCalibration ForDisplayedLevel(int displayedLevel)
	{
		if (displayedLevel > 16)
		{
			int shapeRow = RomTubeMappingCatalog.ResolveShapeRow(displayedLevel);
			return ForDisplayedLevel(
				RomTubeMappingCatalog.FirstDisplayedLevelForShapeRow(shapeRow));
		}

		return displayedLevel switch
		{
			1 => new(1.4f, .8f, 35),
			2 => new(1f, 1.1f, 35),
			3 => new(1.2f, .97f, 35),
			4 => new(1f, 1f, 35),
			5 => new(1f, 1.2f, 15),
			6 => new(1f, 1.28f, 15),
			7 => new(1.3f, .9f, -25),
			8 => new(1f, 1f, -75),
			9 => new(1.5f, 1.05f, -150),
			10 => new(1.2f, .9f, 15),
			11 => new(1.5f, 1.1f, 0),
			12 => new(1.3f, .8f, 0),
			13 => new(1.2f, 1f, 0, .3f),
			14 => new(1f, 1.1f, 200),
			15 => new(1.15f, 1f, 75f, 1f),
			// MANUALLY ACCEPTED PROJECT DECISION: Level 16 Atari presentation defaults.
			16 => new(1.1f, 1f, 100f, 1f),
			_ => throw new ArgumentOutOfRangeException(
				nameof(displayedLevel), displayedLevel,
				"No first-pass Atari-presentation calibration exists for this level.")
		};
	}
}
