using System.Numerics;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public readonly record struct AtariModelFrameMapping
{
	public float XUnitsPerAtariUnit { get; }
	public float YUnitsPerAtariUnit { get; }
	public float XReference { get; }
	public float YReference { get; }
	public int XOrientation { get; }
	public int YOrientation { get; }

	public AtariModelFrameMapping(
		float xUnitsPerAtariUnit,
		float yUnitsPerAtariUnit,
		float xReference,
		float yReference,
		int xOrientation,
		int yOrientation)
	{
		ValidateScale(xUnitsPerAtariUnit, nameof(xUnitsPerAtariUnit));
		ValidateScale(yUnitsPerAtariUnit, nameof(yUnitsPerAtariUnit));
		ValidateReference(xReference, nameof(xReference));
		ValidateReference(yReference, nameof(yReference));
		ValidateOrientation(xOrientation, nameof(xOrientation));
		ValidateOrientation(yOrientation, nameof(yOrientation));

		XUnitsPerAtariUnit = xUnitsPerAtariUnit;
		YUnitsPerAtariUnit = yUnitsPerAtariUnit;
		XReference = xReference;
		YReference = yReference;
		XOrientation = xOrientation;
		YOrientation = yOrientation;
	}

	internal void Validate()
	{
		ValidateScale(XUnitsPerAtariUnit, nameof(XUnitsPerAtariUnit));
		ValidateScale(YUnitsPerAtariUnit, nameof(YUnitsPerAtariUnit));
		ValidateReference(XReference, nameof(XReference));
		ValidateReference(YReference, nameof(YReference));
		ValidateOrientation(XOrientation, nameof(XOrientation));
		ValidateOrientation(YOrientation, nameof(YOrientation));
	}

	private static void ValidateScale(float scale, string parameterName)
	{
		if (!float.IsFinite(scale) || scale <= 0)
			throw new ArgumentOutOfRangeException(parameterName,
				"Model-frame scales must be positive and finite.");
	}

	private static void ValidateReference(float reference, string parameterName)
	{
		if (!float.IsFinite(reference))
			throw new ArgumentOutOfRangeException(parameterName,
				"Model-frame references must be finite.");
	}

	private static void ValidateOrientation(int orientation, string parameterName)
	{
		if (orientation is not (1 or -1))
			throw new ArgumentOutOfRangeException(parameterName,
				"Model-frame orientations must be +1 or -1.");
	}
}

public readonly record struct AtariPresentationProjection(Vector2 Position, float LocalScale)
{
	public float LocalScaleX { get; init; } = LocalScale;
	public float LocalScaleY { get; init; } = LocalScale;
}

/// <summary>
/// Presentation-only adapter for the opt-in candidate. Levels 1-3 retain the C# reciprocal
/// camera progression; Level 4 uses an approximate independent-axis frame with that same depth
/// progression; Level 6 fits the normalized near rim in its current C# projection; and Levels 7-9
/// use the recovered Atari reciprocal-depth equation. Diagnostic Level 10 projects its own
/// ROM-backed open geometry through the existing raw-shape mapping; the placement remains
/// accepted and frozen. Level 3's signed ROM Y2D is not a C# placement value. Projection never changes
/// model geometry or gameplay state.
/// </summary>
public static class AtariPresentationAdapter
{
	private const float AtariCenter = 128;
	private const float LogicalCenterX = 400;
	// PROJECT TRANSLATION: center Level 8's projected envelope in the portrait playfield below the HUD.
	public const float Level8VerticalPlacement = 212.28139f;
	// PROJECT TRANSLATION: diagnostic Level 10 final screen-space Y offset. Applied after
	// projection so the existing horizontal mapping, scale, and depth response stay unchanged.
	public const float Level10VerticalPlacement = 50;
	// PROJECT TRANSLATION: +122 logical pixels moves Level 1 down about 154 host pixels at 1.26x.
	private const float Level1VerticalPlacement = 122;
	// PROJECT TRANSLATION: +114 nearly centers the unchanged Level 2 tube in the portrait viewport.
	private const float Level2VerticalPlacement = 114;
	// PROJECT VISUAL CALIBRATION FROM AUTHORIZED MAME REFERENCE: first-pass Level 2 vertical aspect.
	public const float Level2VerticalAspectFactor = 1.314f;
	// PROJECT TRANSLATION FOR PRESENTATION COMPOSITION: derived from Level 3's 70/55 near-rim
	// envelope and the portrait camera, centering its complete tube envelope in the 960px viewport.
	private const float Level3VerticalPlacement = 114.29437f;
	// PROJECT TRANSLATION: align Level 4's near-rim model origin with the portrait viewport
	// center used by Levels 2-3; Level 1's accepted origin is approximately 10 host pixels lower.
	private const float Level4VerticalPlacement = 115.20346f;
	// REFERENCE-DERIVED APPROXIMATION: record-03 correspondence only; coefficients are not presentation gains.
	private static readonly AtariModelFrameMapping Level4ModelFrame = new(
		2.081778520f, 2.231150659f, 128f, 121.501111f, 1, 1);
	// PROJECT VISUAL CALIBRATION FROM THE AUTHORIZED MAME REFERENCE: rounded net X/Y
	// extent gains and the Level 5 near-rim placement exception.
	public const float Level5HorizontalPresentationScale = .670f;
	public const float Level5VerticalPresentationScale = .880f;
	public const float Level5VerticalPlacement = 136.4026f;
	// PROJECT TRANSLATION: fit the reconciled C# near rim to MAME's normalized .625 x .6141 extent;
	// these are presentation candidates, not Atari constants or coordinate-correspondence gains.
	public const float Level6HorizontalPresentationScale = .820504f;
	public const float Level6VerticalPresentationScale = 1.012678f;
	// PROJECT TRANSLATION: place the scaled near rim beneath the HUD about its near-origin anchor.
	public const float Level6VerticalPlacement = 177.21448f;

	public static bool HasModelFrameMapping(int displayedLevel) =>
		displayedLevel is 1 or 2 or 3 or 4 or 7 or 8 or 9 or 10;

	public static AtariPresentationProjection ProjectLevel5(
		float x,
		float y,
		float depth,
		float centerY,
		float yOffset,
		float position,
		float levelDepth,
		float approachDepthOffset,
		float approachVerticalOffset)
	{
		ValidateLevel5Scale(Level5HorizontalPresentationScale,
			nameof(Level5HorizontalPresentationScale));
		ValidateLevel5Scale(Level5VerticalPresentationScale,
			nameof(Level5VerticalPresentationScale));

		float projectedDepth = depth + approachDepthOffset;
		float nearDepth = position + approachDepthOffset;
		float farDepth = position + levelDepth + approachDepthOffset;
		float projectedYOffset = yOffset + approachVerticalOffset;
		Vector2 projected = Projection.Project(
			x, y, projectedDepth, centerY, projectedYOffset, nearDepth, farDepth);
		Vector2 nearAnchor = Projection.Project(
			0, 0, nearDepth, centerY, projectedYOffset, nearDepth, farDepth);
		Vector2 presentationAnchor = nearAnchor + new Vector2(0, Level5VerticalPlacement);
		Vector2 displacement = projected - nearAnchor;
		Vector2 positionOnScreen = presentationAnchor + new Vector2(
			displacement.X * Level5HorizontalPresentationScale,
			displacement.Y * Level5VerticalPresentationScale);
		float localScale = Projection.K / (projectedDepth + Projection.ZOffset);
		float localScaleX = localScale * Level5HorizontalPresentationScale;
		float localScaleY = localScale * Level5VerticalPresentationScale;
		EnsureFinite(positionOnScreen.X, "Level 5 screen X");
		EnsureFinite(positionOnScreen.Y, "Level 5 screen Y");
		EnsureFinite(localScale, "Level 5 local scale");
		EnsureFinite(localScaleX, "Level 5 horizontal local scale");
		EnsureFinite(localScaleY, "Level 5 vertical local scale");

		// Screen-aligned artwork uses a uniform scale; tube-oriented glyphs read both axes.
		return new AtariPresentationProjection(positionOnScreen, localScaleY)
		{
			LocalScaleX = localScaleX,
			LocalScaleY = localScaleY
		};
	}

	public static AtariPresentationProjection ProjectLevel6(
		float x,
		float y,
		float depth,
		float centerY,
		float yOffset,
		float position,
		float levelDepth,
		float approachDepthOffset,
		float approachVerticalOffset)
	{
		ValidateLevel6Scale(Level6HorizontalPresentationScale,
			nameof(Level6HorizontalPresentationScale));
		ValidateLevel6Scale(Level6VerticalPresentationScale,
			nameof(Level6VerticalPresentationScale));

		float projectedDepth = depth + approachDepthOffset;
		float nearDepth = position + approachDepthOffset;
		float farDepth = position + levelDepth + approachDepthOffset;
		float projectedYOffset = yOffset + approachVerticalOffset;
		Vector2 projected = Projection.Project(
			x, y, projectedDepth, centerY, projectedYOffset, nearDepth, farDepth);
		Vector2 nearAnchor = Projection.Project(
			0, 0, nearDepth, centerY, projectedYOffset, nearDepth, farDepth);
		Vector2 presentationAnchor = nearAnchor + new Vector2(0, Level6VerticalPlacement);
		Vector2 displacement = projected - nearAnchor;
		Vector2 positionOnScreen = presentationAnchor + new Vector2(
			displacement.X * Level6HorizontalPresentationScale,
			displacement.Y * Level6VerticalPresentationScale);
		// Keep screen-oriented artwork uniform; tube-oriented artwork consumes both local axes.
		float localScale = Projection.K / (projectedDepth + Projection.ZOffset);
		float localScaleX = localScale * Level6HorizontalPresentationScale;
		float localScaleY = localScale * Level6VerticalPresentationScale;
		EnsureFinite(positionOnScreen.X, "Level 6 screen X");
		EnsureFinite(positionOnScreen.Y, "Level 6 screen Y");
		EnsureFinite(localScale, "Level 6 local scale");
		EnsureFinite(localScaleX, "Level 6 horizontal local scale");
		EnsureFinite(localScaleY, "Level 6 vertical local scale");

		return new AtariPresentationProjection(positionOnScreen, localScale)
		{
			LocalScaleX = localScaleX,
			LocalScaleY = localScaleY
		};
	}

	public static AtariPresentationProjection Project(
		int displayedLevel,
		float x,
		float y,
		float depth,
		float centerY,
		float yOffset,
		float position,
		float levelDepth,
		float approachDepthOffset,
		float approachVerticalOffset,
		float k,
		float zOffset,
		RomPresentationMetadata metadata,
		float level2VerticalAspectFactor = Level2VerticalAspectFactor)
	{
		ArgumentNullException.ThrowIfNull(metadata);
		if (!float.IsFinite(level2VerticalAspectFactor) || level2VerticalAspectFactor <= 0)
			throw new ArgumentOutOfRangeException(nameof(level2VerticalAspectFactor),
				"The Level 2 vertical aspect factor must be positive and finite.");
		(float unitsPerRawUnit, int yOrientation) = displayedLevel == 4
			? (1, 1)
			: GetModelFrameMapping(displayedLevel);
		ValidateMetadata(displayedLevel, metadata);
		ValidateFinite(x, nameof(x));
		ValidateFinite(y, nameof(y));
		ValidateFinite(depth, nameof(depth));
		ValidateFinite(centerY, nameof(centerY));
		ValidateFinite(yOffset, nameof(yOffset));
		ValidateFinite(position, nameof(position));
		ValidateFinite(levelDepth, nameof(levelDepth));
		ValidateFinite(approachDepthOffset, nameof(approachDepthOffset));
		ValidateFinite(approachVerticalOffset, nameof(approachVerticalOffset));
		if (!float.IsFinite(unitsPerRawUnit) || unitsPerRawUnit <= 0)
			throw new InvalidOperationException("The Atari model-frame scale must be positive and finite.");
		if (yOrientation is not (1 or -1))
			throw new InvalidOperationException("The Atari model-frame Y orientation must be +1 or -1.");
		if (!float.IsFinite(k) || k <= 0)
			throw new InvalidOperationException("Projection K must be positive and finite.");
		ValidateFinite(zOffset, nameof(zOffset));

		float verticalOffset = yOffset + approachVerticalOffset;
		float zNear = position + approachDepthOffset;
		float zFar = position + levelDepth + approachDepthOffset;
		float z = depth + approachDepthOffset;
		EnsureFinite(verticalOffset, "combined vertical offset");
		EnsureFinite(zNear, "near depth");
		EnsureFinite(zFar, "far depth");
		EnsureFinite(z, "point depth");

		float depthInterval = zFar - zNear;
		EnsureFinite(depthInterval, "depth interval");
		if (depthInterval == 0)
			throw new InvalidOperationException("The Atari presentation depth interval must be non-zero.");
		float nearDenominator = zNear + zOffset;
		EnsureFinite(nearDenominator, "near camera denominator");
		if (nearDenominator == 0)
			throw new InvalidOperationException("The Atari near camera denominator must be non-zero.");

		if (displayedLevel == 4)
		{
			float denominator = z + zOffset;
			EnsureFinite(denominator, "Level 4 camera denominator");
			if (denominator == 0)
				throw new InvalidOperationException(
					"The Level 4 camera denominator must be non-zero.");
			float depthScale = k / denominator;
			EnsureFinite(depthScale, "Level 4 depth scale");
			if (depthScale == 0)
				throw new InvalidOperationException("The Level 4 depth scale must be non-zero.");

			// The affine fit relates C# coordinates to Atari comparison coordinates. The renderer
			// consumes C# model units, so correspondence coefficients must not become screen gains.
			float level4ScreenX = LogicalCenterX + x * depthScale;
			float level4ScreenY = centerY + (y + verticalOffset) * depthScale +
				Level4VerticalPlacement;
			EnsureFinite(level4ScreenX, "Level 4 screen X");
			EnsureFinite(level4ScreenY, "Level 4 screen Y");
			return new AtariPresentationProjection(
				new Vector2(level4ScreenX, level4ScreenY), depthScale);
		}

		if (displayedLevel == 1)
		{
			// Level 1 retains the established C# depth progression; its ROM reciprocal and Y2D
			// terms do not preserve the known-good near/far relationship in this presentation pipeline.
			float denominator = z + zOffset;
			EnsureFinite(denominator, "Level 1 camera denominator");
			if (denominator == 0)
				throw new InvalidOperationException(
					"The Level 1 camera denominator must be non-zero.");
			float depthScale = denominator / k;
			EnsureFinite(depthScale, "Level 1 depth scale");
			if (depthScale == 0)
				throw new InvalidOperationException("The Level 1 depth scale must be non-zero.");
			float level1LocalScale = 1 / depthScale;
			float level1ScreenX = LogicalCenterX + x / depthScale;
			float level1ScreenY = centerY + (y + verticalOffset) / depthScale +
				Level1VerticalPlacement;
			EnsureFinite(level1LocalScale, "Level 1 local Jacobian");
			EnsureFinite(level1ScreenX, "Level 1 screen X");
			EnsureFinite(level1ScreenY, "Level 1 screen Y");
			return new AtariPresentationProjection(
				new Vector2(level1ScreenX, level1ScreenY), level1LocalScale);
		}

		if (displayedLevel == 2)
		{
			// Preserve Level 2's C# depth and shape; ROM reciprocal depth and signed Y2D would
			// alter the established presentation frame.
			float denominator = z + zOffset;
			EnsureFinite(denominator, "Level 2 camera denominator");
			if (denominator == 0)
				throw new InvalidOperationException(
					"The Level 2 camera denominator must be non-zero.");
			float depthScale = denominator / k;
			EnsureFinite(depthScale, "Level 2 depth scale");
			if (depthScale == 0)
				throw new InvalidOperationException("The Level 2 depth scale must be non-zero.");
			float level2LocalScale = 1 / depthScale;
			float level2ScreenX = LogicalCenterX + x / depthScale;
			float level2ProjectedCenterY = centerY + verticalOffset / depthScale;
			float level2ProjectedY = level2ProjectedCenterY +
				level2VerticalAspectFactor * y / depthScale;
			// PROJECT TRANSLATION: move the complete Level 2 candidate after projection.
			float level2ScreenY = level2ProjectedY + Level2VerticalPlacement;
			float level2LocalScaleY = level2LocalScale * level2VerticalAspectFactor;
			EnsureFinite(level2LocalScale, "Level 2 local Jacobian");
			EnsureFinite(level2LocalScaleY, "Level 2 vertical local Jacobian");
			EnsureFinite(level2ProjectedCenterY, "Level 2 projected tube center");
			EnsureFinite(level2ScreenX, "Level 2 screen X");
			EnsureFinite(level2ScreenY, "Level 2 screen Y");
			return new AtariPresentationProjection(
				new Vector2(level2ScreenX, level2ScreenY), level2LocalScale)
			{
				LocalScaleY = level2LocalScaleY
			};
		}

		if (displayedLevel == 3)
		{
			// Keep Level 3's established C# depth progression; the Atari reciprocal and signed
			// Y2D terms would clip the near rim and reverse its near-to-far center progression.
			float denominator = z + zOffset;
			EnsureFinite(denominator, "Level 3 camera denominator");
			if (denominator == 0)
				throw new InvalidOperationException(
					"The Level 3 camera denominator must be non-zero.");
			float depthScale = denominator / k;
			EnsureFinite(depthScale, "Level 3 depth scale");
			if (depthScale == 0)
				throw new InvalidOperationException("The Level 3 depth scale must be non-zero.");
			float level3LocalScale = 1 / depthScale;
			float level3ScreenX = LogicalCenterX + x / depthScale;
			float level3ScreenY = centerY + (y + verticalOffset) / depthScale +
				Level3VerticalPlacement;
			EnsureFinite(level3LocalScale, "Level 3 local Jacobian");
			EnsureFinite(level3ScreenX, "Level 3 screen X");
			EnsureFinite(level3ScreenY, "Level 3 screen Y");
			return new AtariPresentationProjection(
				new Vector2(level3ScreenX, level3ScreenY), level3LocalScale);
		}

		float normalizedDepth = (z - zNear) / depthInterval;
		EnsureFinite(normalizedDepth, "normalized depth");
		float reciprocalDepth = 16 + 224 * normalizedDepth;
		EnsureFinite(reciprocalDepth, "reciprocal depth");

		float scaleRaw = metadata.ScaleRaw;
		float pointDenominator = reciprocalDepth + scaleRaw;
		EnsureFinite(pointDenominator, "point reciprocal denominator");
		if (pointDenominator == 0)
			throw new InvalidOperationException("The Atari point reciprocal denominator must be non-zero.");
		float logicalScaleNumerator = unitsPerRawUnit * k * (16 + scaleRaw);
		float logicalScaleDenominator = 256 * nearDenominator;
		EnsureFinite(logicalScaleNumerator, "logical scale numerator");
		EnsureFinite(logicalScaleDenominator, "logical scale denominator");
		if (logicalScaleDenominator == 0)
			throw new InvalidOperationException("The Atari logical scale denominator must be non-zero.");
		float logicalScale = logicalScaleNumerator / logicalScaleDenominator;
		EnsureFinite(logicalScale, "logical scale");

		float xRawDelta = x / unitsPerRawUnit;
		float yRawDelta = yOrientation * (y + verticalOffset) / unitsPerRawUnit +
			AtariCenter - metadata.CameraHeightRaw;
		EnsureFinite(xRawDelta, "raw X displacement");
		EnsureFinite(yRawDelta, "raw Y displacement");

		float yOriginDelta = yOrientation * verticalOffset / unitsPerRawUnit +
			AtariCenter - metadata.CameraHeightRaw;
		EnsureFinite(yOriginDelta, "near-origin Y displacement");
		float yOriginProjection = ReciprocalProjection(yOriginDelta, 16, scaleRaw);
		float baseline = centerY + verticalOffset * k / nearDenominator -
			yOrientation * logicalScale * yOriginProjection;
		EnsureFinite(baseline, "host vertical baseline");

		float screenX = LogicalCenterX +
			logicalScale * ReciprocalProjection(xRawDelta, reciprocalDepth, scaleRaw);
		float screenY = baseline + yOrientation * logicalScale *
			(metadata.ScreenYOffsetSigned +
				ReciprocalProjection(yRawDelta, reciprocalDepth, scaleRaw));
		if (displayedLevel == 8)
			screenY += Level8VerticalPlacement;
		if (displayedLevel == 10)
			screenY += Level10VerticalPlacement;

		float localScaleDenominator = unitsPerRawUnit * pointDenominator;
		EnsureFinite(localScaleDenominator, "local Jacobian denominator");
		if (localScaleDenominator == 0)
			throw new InvalidOperationException("The Atari local Jacobian denominator must be non-zero.");
		float localScale = logicalScale * 256 / localScaleDenominator;
		EnsureFinite(screenX, "screen X");
		EnsureFinite(screenY, "screen Y");
		EnsureFinite(localScale, "local Jacobian");
		return new AtariPresentationProjection(new Vector2(screenX, screenY), localScale);
	}

	public static void ValidateTopology(RomPresentationMetadata metadata, bool continuous)
	{
		ArgumentNullException.ThrowIfNull(metadata);
		if (metadata.IsOpen == continuous)
			throw new InvalidOperationException(
				$"ROM topology metadata for Level {metadata.DisplayedLevel} conflicts with its C# level definition.");
	}

	public static AtariModelFrameMapping GetAffineModelFrameMapping(int displayedLevel) =>
	displayedLevel == 4
		? Level4ModelFrame
		: throw new ArgumentOutOfRangeException(nameof(displayedLevel), displayedLevel,
			"No independent-axis Atari model-frame mapping is established for this level.");

	public static (float UnitsPerRawUnit, int YOrientation) GetModelFrameMapping(
	int displayedLevel) =>
		displayedLevel switch
		{
			1 => (25f / 14f, 1),
			// Approximate Level 2 fit; comparison index alignment is not production reindexing.
			2 => (1.436688312f, 1),
			// Approximate Level 3 fit; the comparison-only +2 index alignment is not production reindexing.
			3 => (1.756696429f, 1),
			// PROJECT TRANSLATION: Level 7's established model-frame mapping.
			7 => (1.678958593f, 1),
			// Reuse the established raw-shape normalization for the open Level 8-10 definitions;
			// Level 10's +50 final screen-space placement was manually accepted and is frozen.
			8 or 9 or 10 => (25f / 14f, -1),
			_ => throw new ArgumentOutOfRangeException(nameof(displayedLevel), displayedLevel,
				"No Atari presentation model-frame mapping is established for this level.")
		};

	private static void ValidateMetadata(int displayedLevel, RomPresentationMetadata metadata)
	{
		RomPresentationMetadata expected = RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		if (metadata.DisplayedLevel != displayedLevel ||
			metadata.TubeTableIndex != expected.TubeTableIndex ||
			metadata.ScaleRaw != expected.ScaleRaw ||
			metadata.CameraHeightRaw != expected.CameraHeightRaw ||
			metadata.ScreenYOffsetLowRaw != expected.ScreenYOffsetLowRaw ||
			metadata.ScreenYOffsetHighRaw != expected.ScreenYOffsetHighRaw ||
			metadata.OpenRaw != expected.OpenRaw)
		{
			throw new InvalidOperationException(
				$"ROM presentation metadata for Level {displayedLevel} is missing, inconsistent, or malformed.");
		}
	}

	private static float ReciprocalProjection(float displacement, float depth, float scaleRaw)
	{
		float denominator = depth + scaleRaw;
		EnsureFinite(denominator, "reciprocal denominator");
		if (denominator == 0)
			throw new InvalidOperationException("An Atari reciprocal denominator must be non-zero.");
		float result = 256 * displacement / denominator;
		EnsureFinite(result, "reciprocal projection");
		return result;
	}

	private static void ValidateFinite(float value, string parameterName)
	{
		if (!float.IsFinite(value))
			throw new ArgumentOutOfRangeException(parameterName, "Atari presentation inputs must be finite.");
	}

	private static void ValidateLevel5Scale(float scale, string scaleName)
	{
		if (!float.IsFinite(scale) || scale <= 0)
			throw new InvalidOperationException(
				$"The Level 5 {scaleName} must be positive and finite.");
	}

	private static void ValidateLevel6Scale(float scale, string scaleName)
	{
		if (!float.IsFinite(scale) || scale <= 0)
			throw new InvalidOperationException(
				$"The Level 6 {scaleName} must be positive and finite.");
	}

	private static void EnsureFinite(float value, string valueName)
	{
		if (!float.IsFinite(value))
			throw new InvalidOperationException($"Atari presentation produced a non-finite {valueName}.");
	}
}
