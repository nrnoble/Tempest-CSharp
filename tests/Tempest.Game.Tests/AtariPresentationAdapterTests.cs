using System.Numerics;
using Raylib_cs;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class AtariPresentationAdapterTests
{
	private const float TestK = 70;
	private const float TestZOffset = 5;
	private const float Level1Placement = 122;
	private const float Level2Placement = 114;
	private const float Level3Placement = 114.29437f;
	private const float Level4Placement = 115.20346f;

	[Fact]
	public void AcceptedNearRimOriginsEstablishPortraitCenterAndLevel4UsesIt()
	{
		var camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
		Vector2 ToHost(Vector2 point) =>
			camera.Offset + (point - camera.Target) * camera.Zoom;
		Vector2 viewportCenter = new(
			DisplayConfiguration.WindowWidth / 2f,
			DisplayConfiguration.PortraitViewportHeight / 2f);

		for (int level = 1; level <= 4; level++)
		{
			LevelConfiguration configuration = GetConfiguration(level);
			AtariPresentationProjection origin = Project(configuration,
				RomPresentationMetadataCatalog.ForDisplayedLevel(level),
				0, 0, configuration.Position);
			Vector2 host = ToHost(origin.Position);
			Assert.InRange(MathF.Abs(host.X - viewportCenter.X), 0, .001f);
			float expectedOffset = level == 1 ? 9.7091f : level == 2 ? -.3709f : 0;
			Assert.InRange(MathF.Abs(host.Y - viewportCenter.Y - expectedOffset), 0, .001f);
		}

		LevelConfiguration level4 = GetConfiguration(4);
		var metadata4 = RomPresentationMetadataCatalog.ForDisplayedLevel(4);
		float minHostY = new[] { level4.Position, level4.Position + level4.Depth }
			.SelectMany(depth => level4.Definition.Points.Select(point =>
				ToHost(Project(level4, metadata4, point.X, point.Y, depth).Position).Y))
			.Min();
		// Normal score, lives and wave text end above host Y=165.6 in portrait mode.
		Assert.True(minHostY > (120 + 18) * DisplayConfiguration.PortraitOutputScale);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void NearMidAndFarProjectionsMatchTheContinuousContract(int displayedLevel)
	{
		LevelConfiguration configuration = GetConfiguration(displayedLevel);
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		float[] depths =
		[
			configuration.Position,
			configuration.Position + configuration.Depth / 2,
			configuration.Position + configuration.Depth
		];

		foreach (float depth in depths)
		{
			AtariPresentationProjection actual = Project(configuration, metadata, 13, -9, depth);
			(double x, double y, double scale) = ReferenceProjection(
				displayedLevel, configuration, metadata, 13, -9, depth, 0, 0);

			Assert.InRange(MathF.Abs(actual.Position.X - (float)x), 0, .001f);
			Assert.InRange(MathF.Abs(actual.Position.Y - (float)y), 0, .001f);
			Assert.InRange(MathF.Abs(actual.LocalScale - (float)scale), 0, .001f);
		}

		AtariPresentationProjection near = Project(configuration, metadata, 13, -9, depths[0]);
		Assert.InRange(MathF.Abs(near.LocalScale -
			TestK / (configuration.Position + TestZOffset)), 0, .0001f);
	}

	[Fact]
	public void Level1PreservesLegacyNearScaleAndRecedesContinuouslyToFarRim()
	{
		LevelConfiguration configuration = GetConfiguration(1);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(1);
		float nearDepth = configuration.Position;
		float farDepth = configuration.Position + configuration.Depth;
		AtariPresentationProjection near = Project(configuration, metadata, 0, 0, nearDepth);
		AtariPresentationProjection far = Project(configuration, metadata, 0, 0, farDepth);

		Assert.InRange(MathF.Abs(near.LocalScale - TestK / (nearDepth + TestZOffset)), 0, .0001f);
		Assert.InRange(MathF.Abs(far.LocalScale - TestK / (farDepth + TestZOffset)), 0, .0001f);
		Assert.InRange(MathF.Abs(far.LocalScale / near.LocalScale -
			(nearDepth + TestZOffset) / (farDepth + TestZOffset)), 0, .0001f);
		Assert.True(far.Position.Y > near.Position.Y);
		Assert.InRange(MathF.Abs(near.Position.Y -
			(configuration.Definition.CenterY +
				configuration.Definition.YOffset * TestK / (nearDepth + TestZOffset) +
				Level1Placement)), 0, .001f);

		float previousY = near.Position.Y;
		float previousScale = near.LocalScale;
		for (int step = 1; step <= 8; step++)
		{
			float depth = nearDepth + configuration.Depth * step / 8;
			AtariPresentationProjection current = Project(configuration, metadata, 0, 0, depth);
			Assert.True(float.IsFinite(current.Position.X));
			Assert.True(float.IsFinite(current.Position.Y));
			Assert.True(float.IsFinite(current.LocalScale));
			Assert.True(current.Position.Y > previousY);
			Assert.True(current.LocalScale < previousScale);
			previousY = current.Position.Y;
			previousScale = current.LocalScale;
		}
	}

	[Fact]
	public void Level5VisualCandidatePreservesItsAnchorDepthAndAxisSpecificJacobian()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = TestK;
			Projection.ZOffset = TestZOffset;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(5);
			Assert.False(AtariPresentationAdapter.HasModelFrameMapping(5));
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				AtariPresentationAdapter.GetModelFrameMapping(5));
			Assert.Equal(.670f, AtariPresentationAdapter.Level5HorizontalPresentationScale);
			Assert.Equal(.880f, AtariPresentationAdapter.Level5VerticalPresentationScale);

			AtariPresentationProjection near = ProjectLevel5(
				configuration, 0, 0, configuration.Position);
			AtariPresentationProjection far = ProjectLevel5(
				configuration, 0, 0, configuration.Position + configuration.Depth);
			Vector2 legacyNear = Projection.Project(
				0, 0, configuration.Position, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Position + configuration.Depth);
			Vector2 legacyFar = Projection.Project(
				0, 0, configuration.Position + configuration.Depth,
				configuration.Definition.CenterY, configuration.Definition.YOffset,
				configuration.Position, configuration.Position + configuration.Depth);

			Assert.InRange(MathF.Abs(near.Position.X - 400), 0, .001f);
			Assert.InRange(MathF.Abs(near.Position.Y - legacyNear.Y -
				AtariPresentationAdapter.Level5VerticalPlacement), 0, .001f);
			Assert.InRange(MathF.Abs(
				(far.Position.Y - near.Position.Y) -
				(legacyFar.Y - legacyNear.Y) *
				AtariPresentationAdapter.Level5VerticalPresentationScale), 0, .001f);
			Assert.InRange(MathF.Abs(
				far.LocalScale / near.LocalScale -
				(configuration.Position + TestZOffset) /
				(configuration.Position + configuration.Depth + TestZOffset)), 0, .001f);
			Assert.True(near.LocalScaleX < near.LocalScaleY);

			foreach (Vector2 point in configuration.Definition.Points)
			{
				Vector2 legacyPoint = Projection.Project(
					point.X, point.Y, configuration.Position,
					configuration.Definition.CenterY, configuration.Definition.YOffset,
					configuration.Position, configuration.Position + configuration.Depth);
				AtariPresentationProjection candidate = ProjectLevel5(
					configuration, point.X, point.Y, configuration.Position);
				Vector2 expected = near.Position + new Vector2(
					(legacyPoint.X - legacyNear.X) *
						AtariPresentationAdapter.Level5HorizontalPresentationScale,
					(legacyPoint.Y - legacyNear.Y) *
						AtariPresentationAdapter.Level5VerticalPresentationScale);
				Assert.InRange(Vector2.Distance(candidate.Position, expected), 0, .001f);
			}

			Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
			Vector2 ToHost(Vector2 point) =>
				camera.Offset + (point - camera.Target) * camera.Zoom;
			Vector2[] hostNear = configuration.Definition.Points
				.Select(point => ToHost(ProjectLevel5(
					configuration, point.X, point.Y, configuration.Position).Position))
				.ToArray();
			float hudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
			float hostTop = hostNear.Min(point => point.Y);
			Assert.True(hostTop > hudBottom,
				$"Level 5 near-rim top {hostTop} must clear HUD bottom {hudBottom}.");
			Assert.InRange(MathF.Abs(ToHost(near.Position).X -
				DisplayConfiguration.WindowWidth / 2f), 0, .01f);
			Assert.InRange(MathF.Abs(ToHost(near.Position).Y -
				DisplayConfiguration.PortraitViewportHeight * .532f), 0, .01f);

			const float epsilon = .1f;
			AtariPresentationProjection center = ProjectLevel5(
				configuration, 0, 0, configuration.Position + configuration.Depth / 2);
			AtariPresentationProjection xStep = ProjectLevel5(
				configuration, epsilon, 0,
				configuration.Position + configuration.Depth / 2);
			AtariPresentationProjection yStep = ProjectLevel5(
				configuration, 0, epsilon,
				configuration.Position + configuration.Depth / 2);
			Assert.InRange(MathF.Abs(
				(xStep.Position.X - center.Position.X) / epsilon - center.LocalScaleX),
				0, .001f);
			Assert.InRange(MathF.Abs(
				(yStep.Position.Y - center.Position.Y) / epsilon - center.LocalScaleY),
				0, .001f);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level5ApproachOffsetsComposeDeterministicallyWithoutModelMutation()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = TestK;
			Projection.ZOffset = TestZOffset;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(5);
			Vector2 point = configuration.Definition.Points[0];
			Vector2[] storedPoints = configuration.Definition.Points.ToArray();
			const float approachDepth = 4;
			const float approachVertical = 9;
			float depth = configuration.Position + configuration.Depth / 2;
			AtariPresentationProjection actual = AtariPresentationAdapter.ProjectLevel5(
				point.X, point.Y, depth, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Depth, approachDepth, approachVertical);
			AtariPresentationProjection repeated = AtariPresentationAdapter.ProjectLevel5(
				point.X, point.Y, depth, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Depth, approachDepth, approachVertical);
			Vector2 legacy = Projection.Project(
				point.X, point.Y, depth + approachDepth,
				configuration.Definition.CenterY,
				configuration.Definition.YOffset + approachVertical,
				configuration.Position + approachDepth,
				configuration.Position + configuration.Depth + approachDepth);
			Vector2 legacyAnchor = Projection.Project(
				0, 0, configuration.Position + approachDepth,
				configuration.Definition.CenterY,
				configuration.Definition.YOffset + approachVertical,
				configuration.Position + approachDepth,
				configuration.Position + configuration.Depth + approachDepth);
			Vector2 expected = legacyAnchor +
				new Vector2(0, AtariPresentationAdapter.Level5VerticalPlacement) +
				new Vector2(
					(legacy.X - legacyAnchor.X) *
						AtariPresentationAdapter.Level5HorizontalPresentationScale,
					(legacy.Y - legacyAnchor.Y) *
						AtariPresentationAdapter.Level5VerticalPresentationScale);

			Assert.Equal(actual, repeated);
			Assert.InRange(Vector2.Distance(actual.Position, expected), 0, .001f);
			Assert.Equal(storedPoints, configuration.Definition.Points.ToArray());
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void InvalidLevel5CandidateInputsFailExplicitly()
	{
		LevelConfiguration configuration = GetConfiguration(5);
		AtariPresentationProjection Project(float x = 0, float y = 0, float? depth = null) =>
			AtariPresentationAdapter.ProjectLevel5(
				x, y, depth ?? configuration.Position,
				configuration.Definition.CenterY, configuration.Definition.YOffset,
				configuration.Position, configuration.Depth, 0, 0);

		Assert.Throws<ArgumentOutOfRangeException>(() => Project(x: float.NaN));
		Assert.Throws<ArgumentOutOfRangeException>(() => Project(y: float.PositiveInfinity));
		Assert.Throws<ArgumentOutOfRangeException>(() => Project(depth: float.NegativeInfinity));

		float originalK = Projection.K;
		try
		{
			Projection.K = 0;
			Assert.Throws<InvalidOperationException>(() => Project());
		}
		finally
		{
			Projection.K = originalK;
		}
	}

	[Fact]
	public void Level6VisualCandidateMatchesNormalizedMameNearRimAndKeepsDepthProgression()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = TestK;
			Projection.ZOffset = TestZOffset;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(6);
			LevelDefinition definition = configuration.Definition;
			float nearDepth = configuration.Position;
			float farDepth = nearDepth + configuration.Depth;
			Vector2 legacyNearAnchor = Projection.Project(
				0, 0, nearDepth, definition.CenterY, definition.YOffset,
				nearDepth, farDepth);
			AtariPresentationProjection candidateNearAnchor = ProjectLevel6(
				configuration, 0, 0, nearDepth);

			Assert.Equal(.820504f, AtariPresentationAdapter.Level6HorizontalPresentationScale);
			Assert.Equal(1.012678f, AtariPresentationAdapter.Level6VerticalPresentationScale);
			Assert.InRange(MathF.Abs(candidateNearAnchor.Position.X - legacyNearAnchor.X), 0, .001f);
			Assert.InRange(MathF.Abs(candidateNearAnchor.Position.Y - legacyNearAnchor.Y -
				AtariPresentationAdapter.Level6VerticalPlacement), 0, .001f);
			Assert.InRange(MathF.Abs(
				candidateNearAnchor.LocalScale -
				TestK / (nearDepth + TestZOffset)), 0, .001f);
			Assert.InRange(MathF.Abs(
				candidateNearAnchor.LocalScaleX -
				candidateNearAnchor.LocalScale *
					AtariPresentationAdapter.Level6HorizontalPresentationScale), 0, .001f);
			Assert.InRange(MathF.Abs(
				candidateNearAnchor.LocalScaleY -
				candidateNearAnchor.LocalScale *
					AtariPresentationAdapter.Level6VerticalPresentationScale), 0, .001f);

			Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
			Vector2 ToHost(Vector2 point) =>
				camera.Offset + (point - camera.Target) * camera.Zoom;
			Vector2[] legacyHostNear = definition.Points
				.Select(point => ToHost(Projection.Project(
					point.X, point.Y, nearDepth, definition.CenterY, definition.YOffset,
					nearDepth, farDepth)))
				.ToArray();
			Assert.InRange(MathF.Abs(legacyHostNear.Min(point => point.X) - 365.778f), 0, .01f);
			Assert.InRange(MathF.Abs(legacyHostNear.Max(point => point.X) - 914.222f), 0, .01f);
			Assert.InRange(MathF.Abs(legacyHostNear.Min(point => point.Y) - .142f), 0, .01f);
			Assert.InRange(MathF.Abs(legacyHostNear.Max(point => point.Y) - 582.262f), 0, .01f);
			Vector2[] hostNear = definition.Points
				.Select(point => ToHost(ProjectLevel6(
					configuration, point.X, point.Y, nearDepth).Position))
				.ToArray();
			float minX = hostNear.Min(point => point.X);
			float maxX = hostNear.Max(point => point.X);
			float minY = hostNear.Min(point => point.Y);
			float maxY = hostNear.Max(point => point.Y);
			float normalizedWidth =
				(maxX - minX) / DisplayConfiguration.PortraitViewportWidth;
			float normalizedHeight =
				(maxY - minY) / DisplayConfiguration.PortraitViewportHeight;
			float normalizedLeftMargin =
				(minX - DisplayConfiguration.PortraitViewportLeft) /
				DisplayConfiguration.PortraitViewportWidth;
			float normalizedRightMargin =
				(DisplayConfiguration.PortraitViewportLeft +
				 DisplayConfiguration.PortraitViewportWidth - maxX) /
				DisplayConfiguration.PortraitViewportWidth;
			float normalizedTopMargin =
				minY / DisplayConfiguration.PortraitViewportHeight;
			float normalizedBottomMargin =
				(DisplayConfiguration.PortraitViewportHeight - maxY) /
				DisplayConfiguration.PortraitViewportHeight;
			float normalizedCenterX =
				((minX + maxX) / 2 - DisplayConfiguration.PortraitViewportLeft) /
				DisplayConfiguration.PortraitViewportWidth;
			float normalizedCenterY =
				(minY + maxY) / (2 * DisplayConfiguration.PortraitViewportHeight);

			Assert.InRange(MathF.Abs(normalizedWidth - 300f / 480), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedHeight - 393f / 640), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedLeftMargin - 90f / 480), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedRightMargin - 90f / 480), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedTopMargin - 146f / 640), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedBottomMargin - 101f / 640), 0, .002f);
			Assert.InRange(MathF.Abs(normalizedCenterX - .5f), 0, .002f);
			Assert.InRange(MathF.Abs(
				normalizedCenterY - (146f + 539f) / (2 * 640)), 0, .002f);
			Assert.True(minY > 122f / 640 * DisplayConfiguration.PortraitViewportHeight);

			float depth = nearDepth + configuration.Depth / 2;
			const float epsilon = .1f;
			AtariPresentationProjection center = ProjectLevel6(configuration, 0, 0, depth);
			AtariPresentationProjection xStep = ProjectLevel6(configuration, epsilon, 0, depth);
			AtariPresentationProjection yStep = ProjectLevel6(configuration, 0, epsilon, depth);
			Assert.InRange(MathF.Abs(
				(xStep.Position.X - center.Position.X) / epsilon - center.LocalScaleX),
				0, .001f);
			Assert.InRange(MathF.Abs(
				(yStep.Position.Y - center.Position.Y) / epsilon - center.LocalScaleY),
				0, .001f);

			AtariPresentationProjection far = ProjectLevel6(configuration, 0, 0, farDepth);
			Assert.InRange(MathF.Abs(
				far.LocalScale / candidateNearAnchor.LocalScale -
				(nearDepth + TestZOffset) / (farDepth + TestZOffset)), 0, .001f);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level6ApproachOffsetsComposeDeterministicallyWithoutModelMutation()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = TestK;
			Projection.ZOffset = TestZOffset;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(6);
			Vector2 point = configuration.Definition.Points[0];
			Vector2[] storedPoints = configuration.Definition.Points.ToArray();
			const float approachDepth = 4;
			const float approachVertical = 9;
			float depth = configuration.Position + configuration.Depth / 2;
			AtariPresentationProjection actual = AtariPresentationAdapter.ProjectLevel6(
				point.X, point.Y, depth, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Depth, approachDepth, approachVertical);
			AtariPresentationProjection repeated = AtariPresentationAdapter.ProjectLevel6(
				point.X, point.Y, depth, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Depth, approachDepth, approachVertical);
			Vector2 legacy = Projection.Project(
				point.X, point.Y, depth + approachDepth,
				configuration.Definition.CenterY,
				configuration.Definition.YOffset + approachVertical,
				configuration.Position + approachDepth,
				configuration.Position + configuration.Depth + approachDepth);
			Vector2 legacyAnchor = Projection.Project(
				0, 0, configuration.Position + approachDepth,
				configuration.Definition.CenterY,
				configuration.Definition.YOffset + approachVertical,
				configuration.Position + approachDepth,
				configuration.Position + configuration.Depth + approachDepth);
			Vector2 expected = legacyAnchor +
				new Vector2(0, AtariPresentationAdapter.Level6VerticalPlacement) +
				new Vector2(
					(legacy.X - legacyAnchor.X) *
						AtariPresentationAdapter.Level6HorizontalPresentationScale,
					(legacy.Y - legacyAnchor.Y) *
						AtariPresentationAdapter.Level6VerticalPresentationScale);

			Assert.Equal(actual, repeated);
			Assert.InRange(Vector2.Distance(actual.Position, expected), 0, .001f);
			Assert.Equal(storedPoints, configuration.Definition.Points.ToArray());
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void InvalidLevel6CandidateInputsFailExplicitly()
	{
		LevelConfiguration configuration = GetConfiguration(6);
		AtariPresentationProjection Project(float x = 0, float y = 0, float? depth = null) =>
			ProjectLevel6(configuration, x, y, depth ?? configuration.Position);

		Assert.Throws<ArgumentOutOfRangeException>(() => Project(x: float.NaN));
		Assert.Throws<ArgumentOutOfRangeException>(() => Project(y: float.PositiveInfinity));
		Assert.Throws<ArgumentOutOfRangeException>(() => Project(depth: float.NegativeInfinity));

		float originalK = Projection.K;
		try
		{
			Projection.K = 0;
			Assert.Throws<InvalidOperationException>(() => Project());
		}
		finally
		{
			Projection.K = originalK;
		}
	}

	[Fact]
	public void Level2PreservesCSharpDepthAndUsesUniformPlacementAcrossTheTube()
	{
		LevelConfiguration configuration = GetConfiguration(2);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(2);
		float nearDepth = configuration.Position;
		float farDepth = configuration.Position + configuration.Depth;
		AtariPresentationProjection near = Project(configuration, metadata, 0, 0, nearDepth);
		AtariPresentationProjection far = Project(configuration, metadata, 0, 0, farDepth);

		Assert.Equal(-224, metadata.ScreenYOffsetSigned);
		Assert.InRange(MathF.Abs(near.LocalScale -
			TestK / (nearDepth + TestZOffset)), 0, .0001f);
		Assert.InRange(MathF.Abs(far.LocalScale -
			TestK / (farDepth + TestZOffset)), 0, .0001f);
		Assert.True(far.LocalScale < near.LocalScale * .25f);
		Assert.True(far.Position.Y > near.Position.Y);
		Assert.InRange(MathF.Abs(near.Position.Y -
			(configuration.Definition.CenterY +
				configuration.Definition.YOffset * TestK / (nearDepth + TestZOffset) +
				Level2Placement)), 0, .001f);

		float previousY = near.Position.Y;
		float previousScale = near.LocalScale;
		for (int step = 1; step <= 8; step++)
		{
			float depth = nearDepth + configuration.Depth * step / 8;
			AtariPresentationProjection current = Project(configuration, metadata, 0, 0, depth);
			Assert.True(float.IsFinite(current.Position.X));
			Assert.True(float.IsFinite(current.Position.Y));
			Assert.True(float.IsFinite(current.LocalScale));
			Assert.True(current.Position.Y > previousY);
			Assert.True(current.LocalScale < previousScale);
			previousY = current.Position.Y;
			previousScale = current.LocalScale;
		}
	}

	[Fact]
	public void Level2PlacementPreservesPrePlacementProjectionAndCentersTheTubeEnvelope()
	{
		LevelConfiguration configuration = GetConfiguration(2);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(2);
		float nearDepth = configuration.Position;
		float farDepth = configuration.Position + configuration.Depth;
		float[] depths = [nearDepth, (nearDepth + farDepth) / 2, farDepth];
		List<Vector2> originalNear = [];
		List<Vector2> placedNear = [];
		List<Vector2> originalFar = [];
		List<Vector2> placedFar = [];

		foreach (Vector2 point in configuration.Definition.Points)
		{
			foreach (float depth in depths)
			{
				AtariPresentationProjection original = ProjectLevel2BeforePlacement(
					configuration, point, depth);
				AtariPresentationProjection originalCenter = ProjectLevel2BeforePlacement(
					configuration, Vector2.Zero, depth);
				AtariPresentationProjection placed = AtariPresentationAdapter.Project(
					2,
					point.X,
					point.Y,
					depth,
					configuration.Definition.CenterY,
					configuration.Definition.YOffset,
					configuration.Position,
					configuration.Depth,
					0,
					0,
					TestK,
					TestZOffset,
					metadata);

				Assert.InRange(MathF.Abs(placed.Position.X - original.Position.X), 0, .001f);
				Assert.InRange(MathF.Abs(
					placed.Position.Y -
					(originalCenter.Position.Y +
						(original.Position.Y - originalCenter.Position.Y) *
						AtariPresentationAdapter.Level2VerticalAspectFactor +
						Level2Placement)), 0, .001f);
				Assert.InRange(MathF.Abs(placed.LocalScale - original.LocalScale), 0, .001f);
				Assert.InRange(MathF.Abs(placed.LocalScaleY -
					original.LocalScale * AtariPresentationAdapter.Level2VerticalAspectFactor),
					0, .001f);

				if (depth == nearDepth)
				{
					originalNear.Add(original.Position);
					placedNear.Add(placed.Position);
				}
				else if (depth == farDepth)
				{
					originalFar.Add(original.Position);
					placedFar.Add(placed.Position);
				}
			}
		}

		AssertAspectBounds(originalNear, placedNear);
		AssertAspectBounds(originalFar, placedFar);
		(float originalNearMinX, float originalNearMinY, float originalNearMaxX, float originalNearMaxY) =
			Bounds(originalNear);
		(float originalFarMinX, float originalFarMinY, float originalFarMaxX, float originalFarMaxY) =
			Bounds(originalFar);
		(float placedNearMinX, float placedNearMinY, float placedNearMaxX, float placedNearMaxY) =
			Bounds(placedNear);
		(float placedFarMinX, float placedFarMinY, float placedFarMaxX, float placedFarMaxY) =
			Bounds(placedFar);
		float originalHorizontalRatio =
			(originalNearMaxX - originalNearMinX) / (originalFarMaxX - originalFarMinX);
		float originalVerticalRatio =
			(originalNearMaxY - originalNearMinY) / (originalFarMaxY - originalFarMinY);
		Assert.InRange(MathF.Abs(
			((placedNearMaxX - placedNearMinX) / (placedFarMaxX - placedFarMinX)) -
				originalHorizontalRatio), 0, .001f);
		Assert.InRange(MathF.Abs(
			((placedNearMaxY - placedNearMinY) / (placedFarMaxY - placedFarMinY)) -
				originalVerticalRatio), 0, .001f);
		Assert.InRange(MathF.Abs(
			(CenterY(placedFar) - CenterY(placedNear)) -
			(CenterY(originalFar) - CenterY(originalNear))), 0, .001f);

		(float _, float originalNearTop, float _, float originalNearBottom) =
			Bounds(originalNear);
		(float _, float originalFarTop, float _, float originalFarBottom) =
			Bounds(originalFar);
		(float _, float placedNearTop, float _, float placedNearBottom) = Bounds(placedNear);
		(float _, float placedFarTop, float _, float placedFarBottom) = Bounds(placedFar);
		float originalTop = MathF.Min(originalNearTop, originalFarTop);
		float originalBottom = MathF.Max(originalNearBottom, originalFarBottom);
		float placedTop = MathF.Min(placedNearTop, placedFarTop);
		float placedBottom = MathF.Max(placedNearBottom, placedFarBottom);
		var camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
		float ToHostY(float logicalY) =>
			camera.Offset.Y + (logicalY - camera.Target.Y) * camera.Zoom;
		float hostTop = ToHostY(placedTop);
		float hostBottom = ToHostY(placedBottom);

		Assert.InRange(MathF.Abs(ToHostY(originalTop) - 77.8f), 0, .1f);
		Assert.InRange(MathF.Abs(ToHostY(originalBottom) - 594.2f), 0, .1f);
		Assert.InRange(MathF.Abs(hostTop - 140.4f), 0, .2f);
		Assert.InRange(MathF.Abs(hostBottom - 818.9f), 0, .2f);
		Assert.True(hostTop > 100);
		Assert.True(DisplayConfiguration.PortraitViewportHeight - hostBottom > 100);
		Assert.True(hostTop >= 0);
		Assert.True(hostBottom <= DisplayConfiguration.PortraitViewportHeight);
	}

	[Fact]
	public void Level3PreservesCSharpDepthAndRecedesContinuouslyToFarRim()
	{
		LevelConfiguration configuration = GetConfiguration(3);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(3);
		float nearDepth = configuration.Position;
		float middleDepth = nearDepth + configuration.Depth / 2;
		float farDepth = nearDepth + configuration.Depth;
		AtariPresentationProjection near = Project(configuration, metadata, 0, 0, nearDepth);
		AtariPresentationProjection middle = Project(configuration, metadata, 0, 0, middleDepth);
		AtariPresentationProjection far = Project(configuration, metadata, 0, 0, farDepth);

		Assert.Equal(-192, metadata.ScreenYOffsetSigned);
		Assert.Equal((1.756696429f, 1),
			AtariPresentationAdapter.GetModelFrameMapping(3));
		Assert.InRange(MathF.Abs(near.LocalScale - TestK / (nearDepth + TestZOffset)), 0, .0001f);
		Assert.InRange(MathF.Abs(middle.LocalScale -
			TestK / (middleDepth + TestZOffset)), 0, .0001f);
		Assert.InRange(MathF.Abs(far.LocalScale - TestK / (farDepth + TestZOffset)), 0, .0001f);
		Assert.True(near.LocalScale > middle.LocalScale && middle.LocalScale > far.LocalScale);
		Assert.True(near.Position.Y < middle.Position.Y && middle.Position.Y < far.Position.Y);
		Assert.True(far.LocalScale < near.LocalScale * .2f);

		float expectedCenterTravel = TestK * -configuration.Definition.YOffset *
			(1 / (nearDepth + TestZOffset) - 1 / (farDepth + TestZOffset));
		Assert.InRange(MathF.Abs(far.Position.Y - near.Position.Y - expectedCenterTravel),
			0, .001f);
	}

	[Fact]
	public void Level4UsesPositiveIndependentAxisFrameAndUniformPlacement()
	{
		LevelConfiguration configuration = GetConfiguration(4);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(4);
		AtariModelFrameMapping frame =
			AtariPresentationAdapter.GetAffineModelFrameMapping(4);
		Assert.Equal(2.081778520f, frame.XUnitsPerAtariUnit);
		Assert.Equal(2.231150659f, frame.YUnitsPerAtariUnit);
		Assert.Equal(128f, frame.XReference);
		Assert.Equal(121.501111f, frame.YReference);
		Assert.Equal(1, frame.XOrientation);
		Assert.Equal(1, frame.YOrientation);
		Assert.True(AtariPresentationAdapter.HasModelFrameMapping(4));
		Assert.False(metadata.IsOpen);
		AtariPresentationAdapter.ValidateTopology(metadata, configuration.Definition.Continuous);
		Assert.Equal(380, configuration.Definition.CenterY);
		Assert.Equal(-65, configuration.Definition.YOffset);

		Vector2[] storedPoints = configuration.Definition.Points.ToArray();
		int laneCount = configuration.Definition.Points.Count;
		float nearDepth = configuration.Position;
		float middleDepth = nearDepth + configuration.Depth / 2;
		float farDepth = nearDepth + configuration.Depth;
		AtariPresentationProjection nearCenter = Project(
			configuration, metadata, 0, 0, nearDepth);
		AtariPresentationProjection farCenter = Project(
			configuration, metadata, 0, 0, farDepth);
		Assert.Equal(nearCenter, Project(
			configuration, metadata, 0, 0, nearDepth));
		Assert.True(nearCenter.Position.Y < farCenter.Position.Y);
		Assert.True(nearCenter.LocalScale > farCenter.LocalScale);
		Assert.True(nearCenter.LocalScaleY > farCenter.LocalScaleY);

		float previousY = nearCenter.Position.Y;
		float previousXScale = nearCenter.LocalScaleX;
		float previousYScale = nearCenter.LocalScaleY;
		for (int step = 1; step <= 8; step++)
		{
			float depth = nearDepth + configuration.Depth * step / 8;
			AtariPresentationProjection center = Project(
				configuration, metadata, 0, 0, depth);
			Assert.True(float.IsFinite(center.Position.X));
			Assert.True(float.IsFinite(center.Position.Y));
			Assert.True(float.IsFinite(center.LocalScaleX));
			Assert.True(float.IsFinite(center.LocalScaleY));
			Assert.True(center.Position.Y > previousY);
			Assert.True(center.LocalScaleX < previousXScale);
			Assert.True(center.LocalScaleY < previousYScale);
			previousY = center.Position.Y;
			previousXScale = center.LocalScaleX;
			previousYScale = center.LocalScaleY;
		}

		foreach (float depth in new[] { nearDepth, middleDepth, farDepth })
		{
			float depthScale = TestK / (depth + TestZOffset);
			AtariPresentationProjection center = Project(
				configuration, metadata, 0, 0, depth);
			Assert.InRange(MathF.Abs(center.Position.X - 400), 0, .001f);
			Assert.InRange(MathF.Abs(center.Position.Y -
				(configuration.Definition.CenterY +
					configuration.Definition.YOffset * depthScale + Level4Placement)), 0, .001f);

			Vector2 point = storedPoints[0];
			AtariPresentationProjection projected = Project(
				configuration, metadata, point.X, point.Y, depth);
			Assert.InRange(MathF.Abs(projected.Position.X -
				(400 + point.X * depthScale)), 0, .001f);
			Assert.InRange(MathF.Abs(projected.Position.Y -
				(configuration.Definition.CenterY +
					(point.Y + configuration.Definition.YOffset) * depthScale +
					Level4Placement)), 0, .001f);
			Assert.InRange(MathF.Abs(projected.LocalScale -
				depthScale), 0, .001f);
			Assert.InRange(MathF.Abs(projected.LocalScaleY -
				depthScale), 0, .001f);
			Assert.True(float.IsFinite(projected.Position.X));
			Assert.True(float.IsFinite(projected.Position.Y));
			Assert.True(float.IsFinite(projected.LocalScale));
			Assert.True(float.IsFinite(projected.LocalScaleY));
		}

		Assert.Equal(storedPoints, configuration.Definition.Points.ToArray());
		Assert.Equal(laneCount, configuration.Definition.Points.Count);
		Assert.True(configuration.Definition.Continuous);
	}

	[Fact]
	public void Level4PlacementPreservesLegacyShapeAtNearMiddleAndFarDepths()
	{
		const float k = 65;
		const float zOffset = 5;
		const float tolerance = .0001f;
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = k;
			Projection.ZOffset = zOffset;
			Projection.PortraitPerspective = 0;

			LevelConfiguration configuration = GetConfiguration(4);
			RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(4);
			Vector2[] storedPoints = configuration.Definition.Points.ToArray();
			bool continuous = configuration.Definition.Continuous;
			int laneCount = configuration.Definition.Points.Count;
			float[] depths = [50, 250, 450];
			List<Vector2[]> candidateFrames = [];
			List<Vector2[]> legacyFrames = [];
			float[] candidateWidths = new float[depths.Length];
			float[] legacyWidths = new float[depths.Length];
			float[] candidateHeights = new float[depths.Length];
			float[] legacyHeights = new float[depths.Length];

			void AssertClose(float expected, float actual) =>
				Assert.InRange(MathF.Abs(expected - actual), 0, tolerance);

			AtariPresentationProjection ProjectCandidate(float x, float y, float depth) =>
				AtariPresentationAdapter.Project(
					4, x, y, depth, configuration.Definition.CenterY,
					configuration.Definition.YOffset, configuration.Position,
					configuration.Depth, 0, 0, k, zOffset, metadata);

			Vector2 ProjectLegacy(float x, float y, float depth) =>
				Projection.Project(
					x, y, depth, configuration.Definition.CenterY,
					configuration.Definition.YOffset, configuration.Position,
					configuration.Position + configuration.Depth);

			Assert.Equal(50, configuration.Position);
			Assert.Equal(400, configuration.Depth);
			Assert.Equal(16, storedPoints.Length);
			Assert.Equal(380, configuration.Definition.CenterY);
			Assert.Equal(-65, configuration.Definition.YOffset);

			for (int depthIndex = 0; depthIndex < depths.Length; depthIndex++)
			{
				float depth = depths[depthIndex];
				float depthScale = k / (depth + zOffset);
				Vector2[] candidatePoints = new Vector2[storedPoints.Length];
				Vector2[] legacyPoints = new Vector2[storedPoints.Length];

				for (int pointIndex = 0; pointIndex < storedPoints.Length; pointIndex++)
				{
					Vector2 point = storedPoints[pointIndex];
					AtariPresentationProjection candidate =
						ProjectCandidate(point.X, point.Y, depth);
					Vector2 legacy = ProjectLegacy(point.X, point.Y, depth);
					Vector2 expected = new(
						400 + point.X * depthScale,
						380 + (point.Y - 65) * depthScale);

					AssertClose(legacy.X, candidate.Position.X);
					AssertClose(legacy.Y + Level4Placement, candidate.Position.Y);
					AssertClose(expected.X, candidate.Position.X);
					AssertClose(expected.Y + Level4Placement, candidate.Position.Y);
					candidatePoints[pointIndex] = candidate.Position;
					legacyPoints[pointIndex] = legacy;
				}

				(float candidateMinX, float candidateMinY,
					float candidateMaxX, float candidateMaxY) = Bounds(candidatePoints);
				(float legacyMinX, float legacyMinY,
					float legacyMaxX, float legacyMaxY) = Bounds(legacyPoints);
				AssertClose(legacyMinX, candidateMinX);
				AssertClose(legacyMinY + Level4Placement, candidateMinY);
				AssertClose(legacyMaxX, candidateMaxX);
				AssertClose(legacyMaxY + Level4Placement, candidateMaxY);

				candidateWidths[depthIndex] = candidateMaxX - candidateMinX;
				legacyWidths[depthIndex] = legacyMaxX - legacyMinX;
				candidateHeights[depthIndex] = candidateMaxY - candidateMinY;
				legacyHeights[depthIndex] = legacyMaxY - legacyMinY;
				AssertClose(legacyWidths[depthIndex], candidateWidths[depthIndex]);
				AssertClose(legacyHeights[depthIndex], candidateHeights[depthIndex]);
				AssertClose((legacyMinX + legacyMaxX) / 2,
					(candidateMinX + candidateMaxX) / 2);
				AssertClose(CenterY(legacyPoints) + Level4Placement, CenterY(candidatePoints));

				Vector2 sample = storedPoints[0];
				AtariPresentationProjection sampleProjection =
					ProjectCandidate(sample.X, sample.Y, depth);
				Vector2 legacySample = ProjectLegacy(sample.X, sample.Y, depth);
				float candidateXDerivative =
					(ProjectCandidate(sample.X + 1, sample.Y, depth).Position.X -
						sampleProjection.Position.X);
				float legacyXDerivative =
					(ProjectLegacy(sample.X + 1, sample.Y, depth).X - legacySample.X);
				float candidateYDerivative =
					(ProjectCandidate(sample.X, sample.Y + 1, depth).Position.Y -
						sampleProjection.Position.Y);
				float legacyYDerivative =
					(ProjectLegacy(sample.X, sample.Y + 1, depth).Y - legacySample.Y);
				AssertClose(legacyXDerivative, candidateXDerivative);
				AssertClose(legacyYDerivative, candidateYDerivative);
				AssertClose(depthScale, sampleProjection.LocalScaleX);
				AssertClose(depthScale, sampleProjection.LocalScaleY);
				AssertClose(legacyXDerivative, sampleProjection.LocalScaleX);
				AssertClose(legacyYDerivative, sampleProjection.LocalScaleY);

				AtariPresentationProjection candidateOrigin = ProjectCandidate(0, 0, depth);
				Vector2 legacyOrigin = ProjectLegacy(0, 0, depth);
				AssertClose(legacyOrigin.X, candidateOrigin.Position.X);
				AssertClose(legacyOrigin.Y + Level4Placement, candidateOrigin.Position.Y);
				candidateFrames.Add(candidatePoints);
				legacyFrames.Add(legacyPoints);
			}

			AssertClose(legacyWidths[0] / legacyWidths[2],
				candidateWidths[0] / candidateWidths[2]);
			AssertClose(legacyHeights[0] / legacyHeights[2],
				candidateHeights[0] / candidateHeights[2]);
			AssertClose(
				CenterY(legacyFrames[2]) - CenterY(legacyFrames[0]),
				CenterY(candidateFrames[2]) - CenterY(candidateFrames[0]));
			Assert.Equal(storedPoints, configuration.Definition.Points.ToArray());
			Assert.Equal(continuous, configuration.Definition.Continuous);
			Assert.Equal(laneCount, configuration.Definition.Points.Count);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level3PlacementUniformlyCentersItsOwnCompletePortraitEnvelope()
	{
		LevelConfiguration configuration = GetConfiguration(3);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(3);
		float nearDepth = configuration.Position;
		float middleDepth = nearDepth + configuration.Depth / 2;
		float farDepth = nearDepth + configuration.Depth;
		float[] depths = [nearDepth, middleDepth, farDepth];
		List<Vector2> originalEnvelope = [];
		List<Vector2> placedEnvelope = [];
		List<Vector2> originalNear = [];
		List<Vector2> placedNear = [];
		List<Vector2> originalMiddle = [];
		List<Vector2> placedMiddle = [];
		List<Vector2> originalFar = [];
		List<Vector2> placedFar = [];

		foreach (float depth in depths)
		{
			foreach (Vector2 point in configuration.Definition.Points)
			{
				AtariPresentationProjection original = ProjectLevel3BeforePlacement(
					configuration, point, depth);
				AtariPresentationProjection placed = Project(
					configuration, metadata, point.X, point.Y, depth);

				Assert.InRange(MathF.Abs(placed.Position.X - original.Position.X), 0, .001f);
				Assert.InRange(MathF.Abs(
					placed.Position.Y - original.Position.Y - Level3Placement), 0, .001f);
				Assert.InRange(MathF.Abs(placed.LocalScale - original.LocalScale), 0, .001f);
				Assert.InRange(MathF.Abs(placed.LocalScaleY - original.LocalScaleY), 0, .001f);
				originalEnvelope.Add(original.Position);
				placedEnvelope.Add(placed.Position);

				if (depth == nearDepth)
				{
					originalNear.Add(original.Position);
					placedNear.Add(placed.Position);
				}
				else if (depth == middleDepth)
				{
					originalMiddle.Add(original.Position);
					placedMiddle.Add(placed.Position);
				}
				else
				{
					originalFar.Add(original.Position);
					placedFar.Add(placed.Position);
				}
			}
		}

		AssertBoundsUnchanged(originalNear, placedNear);
		AssertBoundsUnchanged(originalMiddle, placedMiddle);
		AssertBoundsUnchanged(originalFar, placedFar);
		AssertBoundsUnchanged(originalEnvelope, placedEnvelope);
		Assert.InRange(MathF.Abs(
			(CenterY(placedFar) - CenterY(placedNear)) -
			(CenterY(originalFar) - CenterY(originalNear))), 0, .001f);
		Assert.InRange(MathF.Abs(
			(CenterY(placedMiddle) - CenterY(placedNear)) -
			(CenterY(originalMiddle) - CenterY(originalNear))), 0, .001f);

		var camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
		float ToHostY(float logicalY) =>
			camera.Offset.Y + (logicalY - camera.Target.Y) * camera.Zoom;
		float originalTop = originalEnvelope.Min(point => ToHostY(point.Y));
		float originalBottom = originalEnvelope.Max(point => ToHostY(point.Y));
		float originalCenter = (originalTop + originalBottom) / 2;
		float derivedPlacement =
			(DisplayConfiguration.PortraitViewportHeight / 2f - originalCenter) / camera.Zoom;
		Assert.InRange(MathF.Abs(derivedPlacement - Level3Placement), 0, .001f);

		float placedTop = placedEnvelope.Min(point => ToHostY(point.Y));
		float placedBottom = placedEnvelope.Max(point => ToHostY(point.Y));
		Assert.InRange(MathF.Abs(placedTop - 164.08f), 0, .1f);
		Assert.InRange(MathF.Abs(placedBottom - 795.92f), 0, .1f);
		Assert.True(placedTop > 150);
		Assert.True(DisplayConfiguration.PortraitViewportHeight - placedBottom > 150);

		foreach (float depth in depths)
		{
			AtariPresentationProjection before = ProjectLevel3BeforePlacement(
				configuration, Vector2.Zero, depth);
			AtariPresentationProjection beforeX = ProjectLevel3BeforePlacement(
				configuration, new Vector2(.01f, 0), depth);
			AtariPresentationProjection beforeY = ProjectLevel3BeforePlacement(
				configuration, new Vector2(0, .01f), depth);
			AtariPresentationProjection after = Project(
				configuration, metadata, 0, 0, depth);
			AtariPresentationProjection afterX = Project(
				configuration, metadata, .01f, 0, depth);
			AtariPresentationProjection afterY = Project(
				configuration, metadata, 0, .01f, depth);

			Assert.InRange(MathF.Abs(
				(afterX.Position.X - after.Position.X) / .01f - after.LocalScale), 0, .01f);
			Assert.InRange(MathF.Abs(
				(afterY.Position.Y - after.Position.Y) / .01f - after.LocalScaleY), 0, .01f);
			Assert.InRange(MathF.Abs(
				(beforeX.Position.X - before.Position.X) / .01f -
				(afterX.Position.X - after.Position.X) / .01f), 0, .01f);
			Assert.InRange(MathF.Abs(
				(beforeY.Position.Y - before.Position.Y) / .01f -
				(afterY.Position.Y - after.Position.Y) / .01f), 0, .01f);
		}
	}

	[Fact]
	public void Level1PlacementIsUniformAcrossDepthAndPreservesShapeAndJacobian()
	{
		LevelConfiguration configuration = GetConfiguration(1);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(1);
		const float approachDepth = 4;
		const float approachVertical = 9;
		float nearDepth = configuration.Position;
		float farDepth = configuration.Position + configuration.Depth;
		float[] depths = [nearDepth, (nearDepth + farDepth) / 2, farDepth];
		List<Vector2> originalNear = [];
		List<Vector2> placedNear = [];
		List<Vector2> originalFar = [];
		List<Vector2> placedFar = [];

		foreach (Vector2 point in configuration.Definition.Points)
		{
			foreach (float depth in depths)
			{
				AtariPresentationProjection original = ProjectLevel1BeforePlacement(
					configuration, point, depth, approachDepth, approachVertical);
				AtariPresentationProjection placed = AtariPresentationAdapter.Project(
					1,
					point.X,
					point.Y,
					depth,
					configuration.Definition.CenterY,
					configuration.Definition.YOffset,
					configuration.Position,
					configuration.Depth,
					approachDepth,
					approachVertical,
					TestK,
					TestZOffset,
					metadata);

				Assert.InRange(MathF.Abs(placed.Position.X - original.Position.X), 0, .001f);
				Assert.InRange(MathF.Abs(
					placed.Position.Y - (original.Position.Y + Level1Placement)), 0, .001f);
				Assert.InRange(MathF.Abs(placed.LocalScale - original.LocalScale), 0, .001f);

				if (depth == nearDepth)
				{
					originalNear.Add(original.Position);
					placedNear.Add(placed.Position);
				}
				else if (depth == farDepth)
				{
					originalFar.Add(original.Position);
					placedFar.Add(placed.Position);
				}
			}
		}

		AssertBoundsUnchanged(originalNear, placedNear);
		AssertBoundsUnchanged(originalFar, placedFar);
		Assert.InRange(MathF.Abs(
			(CenterY(placedFar) - CenterY(placedNear)) -
			(CenterY(originalFar) - CenterY(originalNear))), 0, .001f);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(4)]
	public void TransformHasExpectedSymmetryAndDiagonalLocalJacobian(int displayedLevel)
	{
		LevelConfiguration configuration = GetConfiguration(displayedLevel);
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		float depth = configuration.Position + configuration.Depth * .37f;
		AtariPresentationProjection center = Project(configuration, metadata, 0, 11, depth);
		AtariPresentationProjection right = Project(configuration, metadata, 10, 11, depth);
		AtariPresentationProjection left = Project(configuration, metadata, -10, 11, depth);

		Assert.InRange(MathF.Abs(right.Position.X + left.Position.X - 2 * center.Position.X),
			0, .001f);
		Assert.InRange(MathF.Abs(right.Position.Y - left.Position.Y), 0, .001f);

		const float epsilon = .01f;
		AtariPresentationProjection xStep = Project(
			configuration, metadata, epsilon, 11, depth);
		AtariPresentationProjection yStep = Project(
			configuration, metadata, 0, 11 + epsilon, depth);
		Assert.InRange(MathF.Abs(
			(xStep.Position.X - center.Position.X) / epsilon - center.LocalScale), 0, .01f);
		Assert.InRange(MathF.Abs(
			MathF.Abs((yStep.Position.Y - center.Position.Y) / epsilon) -
			center.LocalScaleY), 0, .01f);
		Assert.InRange(MathF.Abs(
			center.LocalScaleY - center.LocalScale * (displayedLevel switch
			{
				2 => AtariPresentationAdapter.Level2VerticalAspectFactor,
				_ => 1
			})),
			0, .001f);
	}

	[Fact]
	public void Level2AspectFactorIsFinitePositiveAndKeepsDepthCentersAndPlacement()
	{
		float factor = AtariPresentationAdapter.Level2VerticalAspectFactor;
		Assert.True(float.IsFinite(factor));
		Assert.True(factor > 1);

		LevelConfiguration configuration = GetConfiguration(2);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(2);
		foreach (float depth in new[]
		{
			configuration.Position,
			configuration.Position + configuration.Depth / 2,
			configuration.Position + configuration.Depth
		})
		{
			AtariPresentationProjection beforePlacement = ProjectLevel2BeforePlacement(
				configuration, Vector2.Zero, depth);
			AtariPresentationProjection actual = Project(configuration, metadata, 0, 0, depth);
			Assert.InRange(MathF.Abs(actual.Position.X - beforePlacement.Position.X), 0, .001f);
			Assert.InRange(MathF.Abs(
				actual.Position.Y - beforePlacement.Position.Y - Level2Placement), 0, .001f);
			Assert.InRange(MathF.Abs(actual.LocalScaleY / actual.LocalScale - factor), 0, .001f);
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void RepeatedInputsAreDeterministicAndSupportContinuousNegativeExtrapolation(
		int displayedLevel)
	{
		LevelConfiguration configuration = GetConfiguration(displayedLevel);
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		AtariPresentationProjection first = Project(
			configuration, metadata, 8, 14, configuration.Position + configuration.Depth * .5f);
		AtariPresentationProjection second = Project(
			configuration, metadata, 8, 14, configuration.Position + configuration.Depth * .5f);
		Assert.Equal(first, second);

		// Finite negative reciprocal denominators remain algebraic extrapolation, not a hardware claim.
		AtariPresentationProjection extrapolated = Project(
			configuration, metadata, 8, 14, configuration.Position - configuration.Depth);
		Assert.True(float.IsFinite(extrapolated.Position.X));
		Assert.True(float.IsFinite(extrapolated.Position.Y));
		Assert.True(float.IsFinite(extrapolated.LocalScale));
		Assert.True(extrapolated.LocalScale < 0);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void ApproachOffsetsComposeIntoTheRenderFrameWithoutModelMutation(int displayedLevel)
	{
		LevelConfiguration configuration = GetConfiguration(displayedLevel);
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		Vector2 point = configuration.Definition.Points[0];
		Vector2[] storedPoints = configuration.Definition.Points.ToArray();
		const float approachDepth = 4;
		const float approachVertical = 9;
		float depth = configuration.Position + configuration.Depth / 2;

		AtariPresentationProjection actual = AtariPresentationAdapter.Project(
			displayedLevel,
			point.X,
			point.Y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			approachDepth,
			approachVertical,
			TestK,
			TestZOffset,
			metadata);
		(double x, double y, _) = ReferenceProjection(
			displayedLevel, configuration, metadata, point.X, point.Y, depth,
			approachDepth, approachVertical);

		Assert.InRange(MathF.Abs(actual.Position.X - (float)x), 0, .001f);
		Assert.InRange(MathF.Abs(actual.Position.Y - (float)y), 0, .001f);
		Assert.Equal(storedPoints, configuration.Definition.Points.ToArray());
		AtariPresentationProjection repeated = AtariPresentationAdapter.Project(
			displayedLevel,
			point.X,
			point.Y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			approachDepth,
			approachVertical,
			TestK,
			TestZOffset,
			metadata);
		Assert.Equal(actual, repeated);
	}

	[Fact]
	public void ModelFrameMappingsAreLimitedToEstablishedCandidateLevels()
	{
		foreach (int level in new[] { 1, 2, 3, 4, 7, 8, 9 })
			Assert.True(AtariPresentationAdapter.HasModelFrameMapping(level));
		foreach (int level in new[] { 5, 6 })
			Assert.False(AtariPresentationAdapter.HasModelFrameMapping(level));

		Assert.Equal((1.436688312f, 1),
			AtariPresentationAdapter.GetModelFrameMapping(2));
		Assert.Equal((1.756696429f, 1),
			AtariPresentationAdapter.GetModelFrameMapping(3));
		Assert.Equal(
			new AtariModelFrameMapping(2.081778520f, 2.231150659f, 128f, 121.501111f, 1, 1),
			AtariPresentationAdapter.GetAffineModelFrameMapping(4));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			AtariPresentationAdapter.GetModelFrameMapping(4));
		RomPresentationMetadata level3Metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(3);
		Assert.Equal(2, level3Metadata.TubeTableIndex);
		Assert.Equal(0x18, level3Metadata.ScaleRaw);
		Assert.Equal(0x50, level3Metadata.CameraHeightRaw);
		Assert.Equal((short)-192, level3Metadata.ScreenYOffsetSigned);
		Assert.False(level3Metadata.IsOpen);
		foreach (int level in new[] { 5, 6 })
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				AtariPresentationAdapter.GetModelFrameMapping(level));
		foreach (int level in new[] { 1, 2, 3, 5, 6, 7, 8, 9 })
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				AtariPresentationAdapter.GetAffineModelFrameMapping(level));
	}

	[Fact]
	public void InvalidInputsAndInconsistentSupportedMetadataFailExplicitly()
	{
		LevelConfiguration configuration = GetConfiguration(1);
		RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(1);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(configuration, metadata, float.NaN, 0, configuration.Position));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(configuration, metadata, 0, 0, float.PositiveInfinity));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(configuration, metadata, 0, 0, configuration.Position,
				zOffset: float.NaN));
		Assert.Throws<InvalidOperationException>(() =>
			Project(configuration, metadata, 0, 0, configuration.Position, k: 0));
		Assert.Throws<InvalidOperationException>(() =>
			Project(configuration, metadata, float.MaxValue, 0, configuration.Position,
				k: float.MaxValue));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(configuration, metadata, 0, 0, configuration.Position,
				approachVerticalOffset: float.PositiveInfinity));
		Assert.Throws<InvalidOperationException>(() =>
			Project(configuration, metadata, 0, 0, configuration.Position,
				levelDepth: 0));
		Assert.Throws<InvalidOperationException>(() =>
			Project(configuration, metadata, 0, 0, configuration.Position,
				position: -TestZOffset));
		LevelConfiguration mappedConfiguration = GetConfiguration(7);
		RomPresentationMetadata mappedMetadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(7);
		Assert.Throws<InvalidOperationException>(() =>
			Project(mappedConfiguration, mappedMetadata, 0, 0, 5,
				position: 10, levelDepth: 28, zOffset: TestZOffset));
		Assert.Throws<InvalidOperationException>(() =>
			Project(configuration, metadata, 0, 0, -TestZOffset));

		LevelConfiguration level2 = GetConfiguration(2);
		RomPresentationMetadata level2Metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(2);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level2, level2Metadata, float.NaN, 0, level2.Position));
		Assert.Throws<InvalidOperationException>(() =>
			Project(level2, level2Metadata, 0, 0, level2.Position, k: 0));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level2, level2Metadata, 0, 0, level2.Position,
				level2VerticalAspectFactor: 0));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level2, level2Metadata, 0, 0, level2.Position,
				level2VerticalAspectFactor: float.NaN));

		LevelConfiguration level3 = GetConfiguration(3);
		RomPresentationMetadata level3Metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(3);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level3, level3Metadata, float.NaN, 0, level3.Position));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level3, level3Metadata, 0, 0, float.PositiveInfinity));
		Assert.Throws<InvalidOperationException>(() =>
			Project(level3, level3Metadata, 0, 0, level3.Position, k: 0));
		Assert.Throws<InvalidOperationException>(() =>
			Project(level3, level3Metadata, 0, 0, level3.Position,
				position: -TestZOffset));

		AtariModelFrameMapping validFrame =
			AtariPresentationAdapter.GetAffineModelFrameMapping(4);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AtariModelFrameMapping(0, validFrame.YUnitsPerAtariUnit,
				validFrame.XReference, validFrame.YReference, 1, 1));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AtariModelFrameMapping(float.NaN, validFrame.YUnitsPerAtariUnit,
				validFrame.XReference, validFrame.YReference, 1, 1));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AtariModelFrameMapping(validFrame.XUnitsPerAtariUnit,
				validFrame.YUnitsPerAtariUnit, validFrame.XReference,
				float.PositiveInfinity, 1, 1));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new AtariModelFrameMapping(validFrame.XUnitsPerAtariUnit,
				validFrame.YUnitsPerAtariUnit, validFrame.XReference,
				validFrame.YReference, 0, 1));

		LevelConfiguration level4 = GetConfiguration(4);
		RomPresentationMetadata level4Metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(4);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			Project(level4, level4Metadata, float.NaN, 0, level4.Position));
		Assert.Throws<InvalidOperationException>(() =>
			Project(level4, level4Metadata, 0, 0, level4.Position, k: 0));
		Assert.Throws<InvalidOperationException>(() =>
			Project(level4, level4Metadata, 0, 0, level4.Position,
				zOffset: -level4.Position));

		RomPresentationMetadata inconsistent = new(
			metadata.DisplayedLevel,
			metadata.TubeTableIndex,
			(byte)(metadata.ScaleRaw + 1),
			metadata.CameraHeightRaw,
			metadata.ScreenYOffsetLowRaw,
			metadata.ScreenYOffsetHighRaw,
			metadata.OpenRaw);
		Assert.Throws<InvalidOperationException>(() =>
			AtariPresentationAdapter.Project(
				1, 0, 0, configuration.Position, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position, configuration.Depth,
				0, 0, TestK, TestZOffset, inconsistent));
	}

	private static AtariPresentationProjection Project(
		LevelConfiguration configuration,
		RomPresentationMetadata metadata,
		float x,
		float y,
		float depth,
		float? position = null,
		float? levelDepth = null,
		float? approachDepthOffset = null,
		float approachVerticalOffset = 0,
		float k = TestK,
		float zOffset = TestZOffset,
		float level2VerticalAspectFactor = AtariPresentationAdapter.Level2VerticalAspectFactor) =>
		AtariPresentationAdapter.Project(
			configuration.DisplayedLevel,
			x,
			y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			position ?? configuration.Position,
			levelDepth ?? configuration.Depth,
			approachDepthOffset ?? 0,
			approachVerticalOffset,
			k,
			zOffset,
			metadata,
			level2VerticalAspectFactor);

	private static AtariPresentationProjection ProjectLevel5(
		LevelConfiguration configuration,
		float x,
		float y,
		float depth) =>
		AtariPresentationAdapter.ProjectLevel5(
			x,
			y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			0,
			0);

	private static AtariPresentationProjection ProjectLevel6(
		LevelConfiguration configuration,
		float x,
		float y,
		float depth) =>
		AtariPresentationAdapter.ProjectLevel6(
			x,
			y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			0,
			0);

	private static (double X, double Y, double Scale) ReferenceProjection(
		int displayedLevel,
		LevelConfiguration configuration,
		RomPresentationMetadata metadata,
		float x,
		float y,
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset)
	{
		if (displayedLevel is 1 or 2 or 3)
		{
			double scale = TestK /
				(depth + approachDepthOffset + TestZOffset);
			float placement = displayedLevel == 1
				? Level1Placement
				: displayedLevel == 2 ? Level2Placement : Level3Placement;
			return (
				400 + x * scale,
				configuration.Definition.CenterY +
					(configuration.Definition.YOffset + approachVerticalOffset) * scale +
					y * scale * (displayedLevel == 2
						? AtariPresentationAdapter.Level2VerticalAspectFactor
						: 1) +
					placement,
				scale);
		}

		if (displayedLevel == 4)
		{
			double scale = TestK /
				(depth + approachDepthOffset + TestZOffset);
			return (
				400 + x * scale,
				configuration.Definition.CenterY +
					(y + configuration.Definition.YOffset + approachVerticalOffset) * scale +
					Level4Placement,
				scale);
		}

		(double unitsPerRawUnit, int orientation) = displayedLevel switch
		{
			7 => (1.678958593, 1),
			8 or 9 => (25d / 14, -1),
			_ => throw new ArgumentOutOfRangeException(nameof(displayedLevel))
		};
		double verticalOffset = configuration.Definition.YOffset + approachVerticalOffset;
		double near = configuration.Position + approachDepthOffset;
		double far = configuration.Position + configuration.Depth + approachDepthOffset;
		double normalizedDepth = (depth + approachDepthOffset - near) / (far - near);
		double reciprocalDepth = 16 + 224 * normalizedDepth;
		double scaleRaw = metadata.ScaleRaw;
		double lambda = unitsPerRawUnit * TestK * (16 + scaleRaw) /
			(256 * (near + TestZOffset));
		double qOrigin = 256 * (orientation * verticalOffset / unitsPerRawUnit +
			128 - metadata.CameraHeightRaw) / (16 + scaleRaw);
		double baseline = configuration.Definition.CenterY +
			verticalOffset * TestK / (near + TestZOffset) -
			orientation * lambda * qOrigin;
		double xDelta = x / unitsPerRawUnit;
		double yDelta = 128 + orientation * (y + verticalOffset) / unitsPerRawUnit -
			metadata.CameraHeightRaw;
		double projectedX = 400 + lambda * 256 * xDelta /
			(reciprocalDepth + scaleRaw);
		double projectedY = baseline + orientation * lambda *
			(metadata.ScreenYOffsetSigned +
				256 * yDelta / (reciprocalDepth + scaleRaw));
		if (displayedLevel == 8)
			projectedY += AtariPresentationAdapter.Level8VerticalPlacement;
		if (displayedLevel == 9)
			projectedY += AtariPresentationAdapter.Level9VerticalPlacement;
		if (displayedLevel == 7)
			projectedY += AtariPresentationAdapter.Level7VerticalPlacement;
		double localScale = lambda * 256 /
			(unitsPerRawUnit * (reciprocalDepth + scaleRaw));
		return (projectedX, projectedY, localScale);
	}

	private static LevelConfiguration GetConfiguration(int displayedLevel) =>
		LevelProgression.GetProductionLevel(displayedLevel) ??
		throw new ArgumentOutOfRangeException(nameof(displayedLevel));

	private static AtariPresentationProjection ProjectLevel1BeforePlacement(
		LevelConfiguration configuration,
		Vector2 point,
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset)
	{
		float z = depth + approachDepthOffset;
		float depthScale = (z + TestZOffset) / TestK;
		return new AtariPresentationProjection(
			new Vector2(
				400 + point.X / depthScale,
				configuration.Definition.CenterY +
					(point.Y + configuration.Definition.YOffset + approachVerticalOffset) /
					depthScale),
			1 / depthScale);
	}

	private static AtariPresentationProjection ProjectLevel2BeforePlacement(
		LevelConfiguration configuration,
		Vector2 point,
		float depth)
	{
		float depthScale = (depth + TestZOffset) / TestK;
		return new AtariPresentationProjection(
			new Vector2(
				400 + point.X / depthScale,
				configuration.Definition.CenterY +
					(point.Y + configuration.Definition.YOffset) / depthScale),
			1 / depthScale);
	}

	private static AtariPresentationProjection ProjectLevel3BeforePlacement(
		LevelConfiguration configuration,
		Vector2 point,
		float depth)
	{
		float depthScale = (depth + TestZOffset) / TestK;
		return new AtariPresentationProjection(
					new Vector2(
						400 + point.X / depthScale,
						configuration.Definition.CenterY +
							(point.Y + configuration.Definition.YOffset) / depthScale),
					1 / depthScale);
	}

	private static (float MinX, float MinY, float MaxX, float MaxY) Bounds(
		IReadOnlyList<Vector2> points) =>
		(points.Min(point => point.X), points.Min(point => point.Y),
			points.Max(point => point.X), points.Max(point => point.Y));

	private static float CenterY(IReadOnlyList<Vector2> points)
	{
		(float _, float minY, float _, float maxY) = Bounds(points);
		return (minY + maxY) / 2;
	}

	private static void AssertBoundsUnchanged(
		IReadOnlyList<Vector2> original,
		IReadOnlyList<Vector2> placed)
	{
		(float originalMinX, float originalMinY, float originalMaxX, float originalMaxY) =
			Bounds(original);
		(float placedMinX, float placedMinY, float placedMaxX, float placedMaxY) =
			Bounds(placed);
		Assert.InRange(MathF.Abs(
			(placedMaxX - placedMinX) - (originalMaxX - originalMinX)), 0, .001f);
		Assert.InRange(MathF.Abs(
			(placedMaxY - placedMinY) - (originalMaxY - originalMinY)), 0, .001f);
	}

	private static void AssertAspectBounds(
		IReadOnlyList<Vector2> original,
		IReadOnlyList<Vector2> corrected)
	{
		(float originalMinX, float originalMinY, float originalMaxX, float originalMaxY) =
			Bounds(original);
		(float correctedMinX, float correctedMinY, float correctedMaxX, float correctedMaxY) =
			Bounds(corrected);
		float factor = AtariPresentationAdapter.Level2VerticalAspectFactor;
		Assert.InRange(MathF.Abs(
			(correctedMaxX - correctedMinX) - (originalMaxX - originalMinX)), 0, .001f);
		Assert.InRange(MathF.Abs(
			(correctedMaxY - correctedMinY) -
				(originalMaxY - originalMinY) * factor), 0, .001f);
		Assert.InRange(MathF.Abs(
			(correctedMaxX - correctedMinX) / (correctedMaxY - correctedMinY) -
				((originalMaxX - originalMinX) /
					(originalMaxY - originalMinY) / factor)), 0, .001f);
	}
}
