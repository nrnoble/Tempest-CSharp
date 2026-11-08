using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PlayerShotRenderingTests
{
	[Fact]
	public void RomPointGroupsAndProjectColorTranslationsAreExplicit()
	{
		IReadOnlyList<PlayerShotVectorPointGroup> groups = PlayerShotVectorProgram.Groups;

		Assert.Equal(2, groups.Count);
		Assert.Equal(8, groups[0].RomColorIndex);
		Assert.Equal(1, groups[1].RomColorIndex);
		Assert.Equal(9, groups[0].Points.Count);
		Assert.Equal(8, groups[1].Points.Count);
		Assert.Equal(new Color(255, 176, 0, 255), groups[0].ProjectColor);
		Assert.Equal(new Color(255, 64, 0, 255), groups[1].ProjectColor);
		Assert.Equal(PlayerShotVectorProgram.RomColor8ProjectTranslation, groups[0].ProjectColor);
		Assert.Equal(PlayerShotVectorProgram.RomColor1ProjectTranslation, groups[1].ProjectColor);

		Assert.Equal(
		[
			new Vector2(0, 0), new(7, 0), new(5, 5), new(0, 7), new(-5, 5),
			new(-7, 0), new(-5, -5), new(0, -7), new(5, -5)
		], groups[0].Points);
		Assert.Equal(
		[
			new Vector2(15, 0), new(11, 11), new(0, 15), new(-11, 11),
			new(-11, 0), new(-11, -11), new(0, -11), new(11, -11)
		], groups[1].Points);
	}

	[Fact]
	public void QualifiedRawWordsPreserveRepeatedOuterMovesInsteadOfInventingSymmetry()
	{
		IReadOnlyList<Vector2> qualifiedPoints = PlayerShotVectorProgram.Groups[1].Points;

		// $3600/$3608 decode as (0,-11); $3610/$3618 decode as (+11,0).
		Assert.Equal(new Vector2(-11, 0), qualifiedPoints[4]);
		Assert.Equal(new Vector2(-11, -11), qualifiedPoints[5]);
		Assert.Equal(new Vector2(0, -11), qualifiedPoints[6]);
		Assert.Equal(new Vector2(11, -11), qualifiedPoints[7]);
		Assert.NotEqual(new Vector2(11, 0), qualifiedPoints[6]);
	}

	[Fact]
	public void RendererSubmitsSeventeenColoredPointsAndNoRadialLines()
	{
		TempestGame game = CreateGameWithShot();
		Shot shot = Assert.Single(game.Shots);
		VectorRenderer renderer = new(game);
		List<(Vector2 Position, Color Color)> points = Capture(renderer);

		Assert.Equal(17, points.Count);
		Assert.Equal(9, points.Count(point =>
			point.Color == PlayerShotVectorProgram.RomColor8ProjectTranslation));
		Assert.Equal(8, points.Count(point =>
			point.Color == PlayerShotVectorProgram.RomColor1ProjectTranslation));
		Assert.DoesNotContain(points, point => point.Color == Color.White);

		Vector2 center = renderer.ProjectShot(shot);
		float scale = ExpectedScale(game, shot);
		int pointIndex = 0;
		foreach (PlayerShotVectorPointGroup group in PlayerShotVectorProgram.Groups)
		{
			foreach (Vector2 romPoint in group.Points)
			{
				Vector2 expected = center +
					new Vector2(romPoint.X, -romPoint.Y) *
					(scale / PlayerShotVectorProgram.MaximumPointRadius);
				Assert.Equal(expected, points[pointIndex].Position);
				Assert.Equal(group.ProjectColor, points[pointIndex].Color);
				pointIndex++;
			}
		}
	}

	[Fact]
	public void GeometryIsStaticAcrossShotAgeAndIndependentOfLaneAndShotOrder()
	{
		TempestGame game = CreateGameWithShot();
		Shot firstShot = Assert.Single(game.Shots);
		VectorRenderer renderer = new(game);
		List<(Vector2 Position, Color Color)> initial = Capture(renderer);
		float originalHeight = firstShot.Height;

		firstShot.Update();
		firstShot.Height = originalHeight;
		Assert.NotEqual(15, firstShot.Rotation);
		Assert.Equal(initial, Capture(renderer));

		TempestGame otherLaneGame = new();
		otherLaneGame.RunOneStep(new PlayerInput(false, 80, false, false));
		for (int tick = 0; tick < 3; tick++)
			otherLaneGame.RunOneStep(default);
		otherLaneGame.RunOneStep(new PlayerInput(true, 0, false, false));
		Shot otherLaneShot = Assert.Single(otherLaneGame.Shots);
		Assert.NotEqual(firstShot.EdgeIndex, otherLaneShot.EdgeIndex);

		otherLaneShot.Height = originalHeight;
		List<(Vector2 Position, Color Color)> otherLanePoints =
			Capture(new VectorRenderer(otherLaneGame));
		Assert.Equal(
			NormalizeAround(initial, renderer.ProjectShot(firstShot)),
			NormalizeAround(otherLanePoints,
				new VectorRenderer(otherLaneGame).ProjectShot(otherLaneShot)));
		Assert.Equal(otherLanePoints, Capture(new VectorRenderer(otherLaneGame)));
	}

	[Fact]
	public void NearMiddleAndFarShotsKeepFiniteShapeWithMonotonicDepthScale()
	{
		TempestGame game = CreateGameWithShot();
		Shot shot = Assert.Single(game.Shots);
		VectorRenderer renderer = new(game);
		float[] projectedRadii = new float[3];
		float[] heights = [shot.Height, shot.Height / 2, 0];

		for (int depthIndex = 0; depthIndex < heights.Length; depthIndex++)
		{
			shot.Height = heights[depthIndex];
			List<(Vector2 Position, Color Color)> points = Capture(renderer);
			Vector2 center = renderer.ProjectShot(shot);

			Assert.Equal(17, points.Count);
			Assert.All(points, point =>
			{
				Assert.True(float.IsFinite(point.Position.X));
				Assert.True(float.IsFinite(point.Position.Y));
			});
			projectedRadii[depthIndex] = points.Max(point =>
				Vector2.Distance(center, point.Position));
			Assert.Equal(ExpectedScale(game, shot), projectedRadii[depthIndex], precision: 3);
		}

		Assert.True(projectedRadii[0] > projectedRadii[1]);
		Assert.True(projectedRadii[1] > projectedRadii[2]);
		Assert.Equal(4.5f, VectorRenderer.PlayerShotBaseSize);
	}

	[Fact]
	public void MultipleActiveShotsRenderIndependentProgramsWithoutConnectingTrail()
	{
		TempestGame game = CreateGameWithShot();
		Shot firstShot = Assert.Single(game.Shots);
		game.RunOneStep(new PlayerInput(false, 80, false, false));
		for (int tick = 0; tick < 2; tick++)
			game.RunOneStep(default);
		game.RunOneStep(new PlayerInput(true, 0, false, false));

		Assert.Equal(2, game.Shots.Count);
		Shot secondShot = game.Shots[1];
		Assert.NotEqual(firstShot.EdgeIndex, secondShot.EdgeIndex);
		firstShot.Height = secondShot.Height = 394;
		VectorRenderer renderer = new(game);
		List<(Vector2 Position, Color Color)> points = Capture(renderer);

		Assert.Equal(34, points.Count);
		Assert.Equal(
			NormalizeAround(points.Take(17).ToList(), renderer.ProjectShot(firstShot)),
			NormalizeAround(points.Skip(17).ToList(), renderer.ProjectShot(secondShot)));
	}

	[Fact]
	public void VisiblePlaybackPauseForwardStepAndReverseReconstructShotRendering()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1, 1981);
		PlayerInput fire = new(true, 0, false, false);
		PlayerInputReplay replay = new(1981, 6,
		[
			new PlayerInputFrame(0, fire),
			new PlayerInputFrame(3, fire)
		]);
		VisiblePlaybackController controller = new(new GameplayPlaybackSession(startup, replay));
		Assert.Equal(0, controller.ProcessControls(new(
			PauseTogglePressed: true, ForwardNavigationPressed: false, ShiftDown: false)));
		Assert.Equal(1, controller.ProcessControls(new(
			PauseTogglePressed: false, ForwardNavigationPressed: true, ShiftDown: false)));
		Assert.Equal(1, controller.ProcessControls(new(
			PauseTogglePressed: false, ForwardNavigationPressed: true, ShiftDown: false)));

		VectorRenderer renderer = new(controller.Session.Game);
		List<(Vector2 Position, Color Color)> boundaryTwo = Capture(renderer);
		Assert.Equal(0, controller.ProcessControls(default));
		Assert.Equal(boundaryTwo, Capture(renderer));

		Assert.Equal(1, controller.ProcessControls(new(
			PauseTogglePressed: false, ForwardNavigationPressed: true, ShiftDown: false)));
		Assert.Equal(3, controller.Session.CurrentBoundary);
		Assert.Equal(-1, controller.ProcessControls(new(
			PauseTogglePressed: false, ForwardNavigationPressed: false, ShiftDown: false,
			BackwardNavigationPressed: true)));
		Assert.Equal(2, controller.Session.CurrentBoundary);

		renderer = new VectorRenderer(controller.Session.Game);
		Assert.Equal(boundaryTwo, Capture(renderer));
	}

	private static TempestGame CreateGameWithShot()
	{
		TempestGame game = new();
		game.RunOneStep(new PlayerInput(true, 0, false, false));
		return game;
	}

	private static List<(Vector2 Position, Color Color)> Capture(VectorRenderer renderer)
	{
		List<(Vector2 Position, Color Color)> points = [];
		renderer.DrawPlayerShots((position, color) => points.Add((position, color)));
		return points;
	}

	private static List<(Vector2 Offset, Color Color)> NormalizeAround(
		IReadOnlyList<(Vector2 Position, Color Color)> points, Vector2 center) =>
		points.Select(point => (point.Position - center, point.Color)).ToList();

	private static float ExpectedScale(TempestGame game, Shot shot)
	{
		float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
			game.CurrentLevel.Configuration.Position;
		return VectorRenderer.PlayerShotBaseSize /
			((projectedDepth + Projection.ZOffset) / Projection.K);
	}
}
