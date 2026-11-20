namespace Tempest.Game.Rendering;

/// <summary>
/// Manually selected first-pass Atari-presentation calibrations. These are project decisions
/// based on Neal's manual visual acceptance, not ROM-derived gameplay geometry; later milestones
/// may fine-tune them. Diagnostic command-line values remain absolute per-dimension overrides.
/// </summary>
public readonly record struct AtariPresentationCalibration(
	float TubeYGain,
	float TubeScale,
	float VerticalOffset)
{
	public static AtariPresentationCalibration Resolve(
		int displayedLevel,
		float? tubeYGainOverride = null,
		float? tubeScaleOverride = null,
		float? verticalOffsetOverride = null)
	{
		AtariPresentationCalibration selected = ForDisplayedLevel(displayedLevel);
		return new(
			tubeYGainOverride ?? selected.TubeYGain,
			tubeScaleOverride ?? selected.TubeScale,
			verticalOffsetOverride ?? selected.VerticalOffset);
	}

	public static AtariPresentationCalibration ForDisplayedLevel(int displayedLevel) =>
		displayedLevel switch
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
			_ => throw new ArgumentOutOfRangeException(
				nameof(displayedLevel), displayedLevel,
				"No first-pass Atari-presentation calibration exists for this level.")
		};
}
