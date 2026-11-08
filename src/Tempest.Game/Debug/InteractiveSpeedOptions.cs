using System.Diagnostics.CodeAnalysis;

namespace Tempest.Game.Debug;

public sealed record InteractiveSpeedOptions
{
	public const int NormalTargetFramesPerSecond = 60;
	public const double MinimumMultiplier = 0.25;
	public const double MaximumMultiplier = 4.0;

	public static InteractiveSpeedOptions Normal { get; } =
		new(1.0, NormalTargetFramesPerSecond);

	public double Multiplier { get; }
	public int TargetFramesPerSecond { get; }

	private InteractiveSpeedOptions(double multiplier, int targetFramesPerSecond)
	{
		Multiplier = multiplier;
		TargetFramesPerSecond = targetFramesPerSecond;
	}

	public static bool TryCreate(
		double multiplier,
		[NotNullWhen(true)] out InteractiveSpeedOptions? options)
	{
		options = null;
		if (!double.IsFinite(multiplier) ||
			multiplier < MinimumMultiplier ||
			multiplier > MaximumMultiplier)
			return false;

		// Raylib requires integer FPS; the explicit tie rule makes conversion predictable.
		int targetFramesPerSecond = (int)Math.Round(
			NormalTargetFramesPerSecond * multiplier,
			MidpointRounding.AwayFromZero);
		options = new InteractiveSpeedOptions(multiplier, targetFramesPerSecond);
		return true;
	}
}
