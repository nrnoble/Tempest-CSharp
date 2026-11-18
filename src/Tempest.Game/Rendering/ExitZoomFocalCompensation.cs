using System.Numerics;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

// DEVELOPMENT ONLY: latch the mapped opening when ExitZoom travel begins, after adapter and
// accepted placement but before the shared host camera.
internal sealed class ExitZoomFocalCompensation
{
	private const double FloatMachineEpsilon = 1.1920928955078125e-7;
	private const double RailIntersectionToleranceFactor = 32;

	private Vector2? focalAnchor;
	private LevelDefinition? cachedDefinition;
	private LevelConfiguration? cachedConfiguration;
	private float cachedK;
	private float cachedZOffset;
	private float cachedPerspective;
	private bool hasCachedOpening;

	public bool IsActive => focalAnchor.HasValue;
	public Vector2? FocalAnchor => focalAnchor;
	public Vector2 Translation { get; private set; }

	public Vector2 GetTranslation(
		LevelState state,
		LevelDefinition definition,
		LevelConfiguration configuration,
		float k,
		float zOffset,
		float perspective,
		Func<Vector2, float, Vector2> mapPoint)
	{
		if (state != LevelState.ExitZoom)
		{
			Reset();
			return Vector2.Zero;
		}

		ArgumentNullException.ThrowIfNull(definition);
		ArgumentNullException.ThrowIfNull(configuration);
		ArgumentNullException.ThrowIfNull(mapPoint);

		if (focalAnchor.HasValue &&
			(!ReferenceEquals(definition, cachedDefinition) ||
			 !ReferenceEquals(configuration, cachedConfiguration)))
			Reset();

		if (!focalAnchor.HasValue)
		{
			focalAnchor = MapOpening(definition, configuration, mapPoint);
			CacheOpening(definition, configuration, k, zOffset, perspective);
			Translation = Vector2.Zero;
			return Translation;
		}

		if (!HasCachedOpening(definition, configuration, k, zOffset, perspective))
		{
			Vector2 currentOpening = MapOpening(definition, configuration, mapPoint);
			// This shared logical-space translation fixes the opening while leaving ordinary Playing placement,
			// source geometry, projection, and simulation state untouched.
			Translation = focalAnchor.Value - currentOpening;
			EnsureFinite(Translation, "ExitZoom focal compensation");
			CacheOpening(definition, configuration, k, zOffset, perspective);
		}

		return Translation;
	}

	public void Reset()
	{
		focalAnchor = null;
		cachedDefinition = null;
		cachedConfiguration = null;
		hasCachedOpening = false;
		Translation = Vector2.Zero;
	}

	private static Vector2 MapOpening(
		LevelDefinition definition,
		LevelConfiguration configuration,
		Func<Vector2, float, Vector2> mapPoint)
	{
		IReadOnlyList<Vector2> points = definition.Points;
		if (points.Count == 0)
			throw new InvalidOperationException("The ExitZoom tube has no opening geometry.");

		float farDepth = configuration.Position + configuration.Depth;
		if (!float.IsFinite(farDepth))
			throw new InvalidOperationException("The ExitZoom far-rim depth must be finite.");

		// PROJECT TRANSLATION: closed tubes retain their mapped rim center; open tubes use
		// their mapped rail convergence. The terminal midpoint belongs to an omitted chord.
		if (!definition.Continuous)
			return CalculateOpenRailConvergence(
				points,
				configuration.Position,
				farDepth,
				mapPoint);

		return CalculateClosedFarRimCenter(points, farDepth, mapPoint);
	}

	private static Vector2 CalculateClosedFarRimCenter(
		IReadOnlyList<Vector2> points,
		float farDepth,
		Func<Vector2, float, Vector2> mapPoint)
	{
		Vector2 sum = Vector2.Zero;
		foreach (Vector2 point in points)
		{
			Vector2 mappedPoint = mapPoint(point, farDepth);
			EnsureFinite(mappedPoint, "ExitZoom far-rim vertex");
			sum += mappedPoint;
		}

		Vector2 center = sum / points.Count;
		EnsureFinite(center, "ExitZoom far-rim center");
		return center;
	}

	private static Vector2 CalculateOpenRailConvergence(
		IReadOnlyList<Vector2> points,
		float nearDepth,
		float farDepth,
		Func<Vector2, float, Vector2> mapPoint)
	{
		if (points.Count < 2)
			throw new InvalidOperationException(
				"An open ExitZoom tube requires at least two rail vertices.");

		Vector2[] nearPoints = new Vector2[points.Count];
		Vector2[] farPoints = new Vector2[points.Count];
		double coordinateScale = 1;
		for (int index = 0; index < points.Count; index++)
		{
			nearPoints[index] = mapPoint(points[index], nearDepth);
			farPoints[index] = mapPoint(points[index], farDepth);
			EnsureFinite(nearPoints[index], "ExitZoom near rail vertex");
			EnsureFinite(farPoints[index], "ExitZoom far rail vertex");
			coordinateScale = Math.Max(coordinateScale,
				Math.Max(Math.Max(Math.Abs(nearPoints[index].X),
					Math.Abs(nearPoints[index].Y)),
					Math.Max(Math.Abs(farPoints[index].X),
						Math.Abs(farPoints[index].Y))));
		}

		// Scale the tolerance by float-coordinate precision, not by visual preference.
		double tolerance = Math.Max(1e-5,
			coordinateScale * RailIntersectionToleranceFactor * FloatMachineEpsilon);
		double a00 = 0;
		double a01 = 0;
		double a11 = 0;
		double b0 = 0;
		double b1 = 0;
		int validRailCount = 0;

		for (int index = 0; index < points.Count; index++)
		{
			double dx = (double)farPoints[index].X - nearPoints[index].X;
			double dy = (double)farPoints[index].Y - nearPoints[index].Y;
			double length = Math.Sqrt(dx * dx + dy * dy);
			if (length <= tolerance)
				continue;

			double normalX = -dy / length;
			double normalY = dx / length;
			double signedDistance = normalX * nearPoints[index].X +
				normalY * nearPoints[index].Y;
			a00 += normalX * normalX;
			a01 += normalX * normalY;
			a11 += normalY * normalY;
			b0 += normalX * signedDistance;
			b1 += normalY * signedDistance;
			validRailCount++;
		}

		if (validRailCount < 2)
			throw new InvalidOperationException(
				"An open ExitZoom tube requires at least two non-degenerate rails.");

		double determinant = a00 * a11 - a01 * a01;
		double trace = a00 + a11;
		if (!double.IsFinite(determinant) ||
			determinant <= trace * trace * 1e-12)
			throw new InvalidOperationException(
				"Open ExitZoom rails do not contain enough independent directions.");

		double intersectionX = (b0 * a11 - b1 * a01) / determinant;
		double intersectionY = (a00 * b1 - a01 * b0) / determinant;
		if (!double.IsFinite(intersectionX) || !double.IsFinite(intersectionY) ||
			Math.Abs(intersectionX) > float.MaxValue ||
			Math.Abs(intersectionY) > float.MaxValue)
			throw new InvalidOperationException(
				"Open ExitZoom rail convergence must be finite.");

		double maximumResidual = 0;
		for (int index = 0; index < points.Count; index++)
		{
			double dx = (double)farPoints[index].X - nearPoints[index].X;
			double dy = (double)farPoints[index].Y - nearPoints[index].Y;
			double length = Math.Sqrt(dx * dx + dy * dy);
			if (length <= tolerance)
				continue;

			double residual = Math.Abs(
				-dy / length * (intersectionX - nearPoints[index].X) +
				dx / length * (intersectionY - nearPoints[index].Y));
			maximumResidual = Math.Max(maximumResidual, residual);
		}

		if (maximumResidual > tolerance)
			throw new InvalidOperationException(
				$"Open ExitZoom rails are not concurrent: maximum residual " +
				$"{maximumResidual:G6} exceeds the numerical tolerance {tolerance:G6}.");

		Vector2 convergence = new((float)intersectionX, (float)intersectionY);
		EnsureFinite(convergence, "ExitZoom open rail convergence");
		return convergence;
	}

	private bool HasCachedOpening(
		LevelDefinition definition,
		LevelConfiguration configuration,
		float k,
		float zOffset,
		float perspective) =>
		hasCachedOpening &&
		ReferenceEquals(definition, cachedDefinition) &&
		ReferenceEquals(configuration, cachedConfiguration) &&
		k == cachedK &&
		zOffset == cachedZOffset &&
		perspective == cachedPerspective;

	private void CacheOpening(
		LevelDefinition definition,
		LevelConfiguration configuration,
		float k,
		float zOffset,
		float perspective)
	{
		cachedDefinition = definition;
		cachedConfiguration = configuration;
		cachedK = k;
		cachedZOffset = zOffset;
		cachedPerspective = perspective;
		hasCachedOpening = true;
	}

	private static void EnsureFinite(Vector2 point, string description)
	{
		if (!float.IsFinite(point.X) || !float.IsFinite(point.Y))
			throw new InvalidOperationException($"{description} must be finite.");
	}
}
