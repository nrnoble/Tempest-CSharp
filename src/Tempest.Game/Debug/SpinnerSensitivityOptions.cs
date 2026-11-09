using System.Diagnostics.CodeAnalysis;

namespace Tempest.Game.Debug;

public sealed record SpinnerSensitivityOptions
{
	public const float DefaultMultiplier = 1.0f;
	public const float MinimumMultiplier = 0.1f;
	public const float MaximumMultiplier = 4.0f;

	public static SpinnerSensitivityOptions Default { get; } =
		new(DefaultMultiplier, wasExplicitlySpecified: false);

	public float Multiplier { get; }
	public bool WasExplicitlySpecified { get; }

	private SpinnerSensitivityOptions(float multiplier, bool wasExplicitlySpecified)
	{
		Multiplier = multiplier;
		WasExplicitlySpecified = wasExplicitlySpecified;
	}

	public static bool TryCreate(
		float multiplier,
		[NotNullWhen(true)] out SpinnerSensitivityOptions? options)
	{
		options = null;
		if (!float.IsFinite(multiplier) ||
			multiplier < MinimumMultiplier ||
			multiplier > MaximumMultiplier)
			return false;

		options = new SpinnerSensitivityOptions(multiplier, wasExplicitlySpecified: true);
		return true;
	}
}
