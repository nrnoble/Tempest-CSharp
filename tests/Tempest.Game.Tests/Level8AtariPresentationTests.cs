using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level8AtariPresentationTests
{
	private const float ProjectionK = 70;
	private const float ProjectionZOffset = 5;

	[Fact]
	public void LevelEightUsesShapeSevenAndCentersItsEnvelopeBelowTheHud()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 8, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			RomPresentationMetadata metadata =
				RomPresentationMetadataCatalog.ForDisplayedLevel(8);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
			List<(Vector2 Start, Vector2 End)> lines = [];

			Assert.Equal(7, metadata.TubeTableIndex);
			Assert.Equal((byte)0x18, metadata.ScaleRaw);
			Assert.Equal((byte)0xB0, metadata.CameraHeightRaw);
			Assert.Equal((short)0x0060, metadata.ScreenYOffsetSigned);
			Assert.Equal((byte)0xFF, metadata.OpenRaw);
			Assert.Equal((25f / 14f, -1), AtariPresentationAdapter.GetModelFrameMapping(8));

			renderer.DrawTube((start, end, _) => lines.Add((start, end)));

			Assert.Equal(46, lines.Count);
			for (int index = 0; index < originalPoints.Length; index++)
			{
				Vector2 point = originalPoints[index];
				Assert.Equal(
					ProjectPoint(configuration, metadata, point, configuration.Position),
					lines[index].Start);
				Assert.Equal(
					ProjectPoint(
						configuration, metadata, point,
						configuration.Position + configuration.Depth),
					lines[index].End);
			}

			Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
			Vector2 ToHost(Vector2 point) =>
				camera.Offset + (point - camera.Target) * camera.Zoom;
			Vector2[] projectedPoints = lines
				.SelectMany(line => new[] { line.Start, line.End })
				.ToArray();
			Vector2[] prePlacementLogicalPoints = projectedPoints
				.Select(point => point - new Vector2(0, AtariPresentationAdapter.Level8VerticalPlacement))
				.ToArray();
			(float MinX, float MaxX, float MinY, float MaxY) prePlacementBounds =
				GetBounds(prePlacementLogicalPoints.Select(ToHost));

			Assert.InRange(MathF.Abs(prePlacementBounds.MinX - 319.27274f), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementBounds.MaxX - 960.7273f), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementBounds.MinY - -25.40182f), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementBounds.MaxY - 616.05273f), 0, 0.01f);

			float hudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
			Vector2 gameplayCenter = new(
				DisplayConfiguration.WindowWidth / 2f,
				(hudBottom + DisplayConfiguration.PortraitViewportHeight) / 2f);
			Vector2 logicalGameplayCenter =
				(gameplayCenter - camera.Offset) / camera.Zoom + camera.Target;
			(float MinX, float MaxX, float MinY, float MaxY) prePlacementLogicalBounds =
				GetBounds(prePlacementLogicalPoints);
			float calculatedTranslation = logicalGameplayCenter.Y -
				(prePlacementLogicalBounds.MinY + prePlacementLogicalBounds.MaxY) / 2;

			Assert.InRange(MathF.Abs(
				calculatedTranslation - AtariPresentationAdapter.Level8VerticalPlacement), 0, 0.001f);

			(float MinX, float MaxX, float MinY, float MaxY) finalBounds =
				GetBounds(projectedPoints.Select(ToHost));
			Assert.InRange(MathF.Abs(finalBounds.MinX - prePlacementBounds.MinX), 0, 0.001f);
			Assert.InRange(MathF.Abs(finalBounds.MaxX - prePlacementBounds.MaxX), 0, 0.001f);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinX + finalBounds.MaxX) / 2 - gameplayCenter.X), 0, 0.001f);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinY + finalBounds.MaxY) / 2 - gameplayCenter.Y), 0, 0.001f);
			Assert.True(finalBounds.MinY > hudBottom);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinY - hudBottom) -
					(DisplayConfiguration.PortraitViewportHeight - finalBounds.MaxY)), 0, 0.001f);
			Assert.True(DisplayConfiguration.PortraitViewportHeight - finalBounds.MaxY > 70);
			Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LevelEightEntityAnchorsRemainRegisteredAcrossTubeDepths()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 8, random: new Random(1));
			Level level = game.CurrentLevel;
			LevelConfiguration configuration = level.Configuration;
			IReadOnlyList<Vector2> points = level.Definition.Points;
			RomPresentationMetadata metadata =
				RomPresentationMetadataCatalog.ForDisplayedLevel(8);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			List<(Vector2 Start, Vector2 End)> tubeLines = [];
			renderer.DrawTube((start, end, _) => tubeLines.Add((start, end)));

			int lane = game.Player.EdgeIndex;
			int nextLane = level.NextEdge(lane);
			int playerLane = game.Player.EdgeIndex;
			float playerPosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			Egg egg = game.Eggs[0];
			(int EdgeIndex, float Height, EggState State) originalEgg =
				(egg.EdgeIndex, egg.Height, egg.State);
			Spike spike = game.Spikes[0];
			float spikeHeight = spike.Height;
			int originalScore = game.Score.Score;
			Vector2[] originalPoints = points.ToArray();
			LevelState originalState = game.State;

			renderer.GetPlayerEdge(points, out Vector2 playerStart, out Vector2 playerEnd);
			Assert.Equal(tubeLines[lane].Start, playerStart);
			Assert.Equal(tubeLines[nextLane].Start, playerEnd);

			foreach (float depth in new[]
			{
				configuration.Position,
				configuration.Position + configuration.Depth / 2,
				configuration.Position + configuration.Depth
			})
			{
				float height = configuration.Position + configuration.Depth - depth;
				(Vector2 Start, Vector2 End) flipperEndpoints =
					renderer.ProjectFlipperEndpoints(new Flipper(lane, height));
				Assert.Equal(ProjectPoint(configuration, metadata, points[lane], depth),
					flipperEndpoints.Start);
				Assert.Equal(ProjectPoint(configuration, metadata, points[nextLane], depth),
					flipperEndpoints.End);
			}

			Shot shot = new() { EdgeIndex = lane, Height = 137 };
			float shotDepth = configuration.Position + configuration.Depth - shot.Height;
			Vector2 laneMidpoint = (points[lane] + points[nextLane]) / 2;
			Assert.Equal(
				ProjectPoint(configuration, metadata, laneMidpoint, shotDepth),
				renderer.ProjectShot(shot));

			float eggDepth = configuration.Position + configuration.Depth - egg.Height * 20;
			Assert.True(eggDepth > configuration.Position + configuration.Depth);
			Assert.Equal(
				ProjectPoint(
					configuration, metadata,
					(points[egg.EdgeIndex] + points[level.NextEdge(egg.EdgeIndex)]) / 2,
					eggDepth),
				renderer.ProjectEgg(egg));

			List<(Vector2 Start, Vector2 End)> spikeLines = [];
			renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), _ => { });
			Vector2 spikeMidpoint =
				(points[spike.EdgeIndex] + points[level.NextEdge(spike.EdgeIndex)]) / 2;
			Assert.Equal(
				ProjectPoint(
					configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth),
				spikeLines[0].Start);
			Assert.Equal(
				ProjectPoint(
					configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth - spike.Height),
				spikeLines[0].End);

			spike.Height += 50;
			spikeLines.Clear();
			renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), _ => { });
			Assert.Equal(
				ProjectPoint(
					configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth - spike.Height),
				spikeLines[0].End);
			Assert.Equal(spikeHeight + 50, spike.Height);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerPosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(originalEgg, (egg.EdgeIndex, egg.Height, egg.State));
			Assert.Equal(originalState, game.State);
			Assert.Equal(originalScore, game.Score.Score);
			Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LevelEightPresentationOptionOffRetainsLegacyProjection()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 8, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2[] points = game.CurrentLevel.Definition.Points.ToArray();
			List<(Vector2 Start, Vector2 End)> lines = [];

			new VectorRenderer(game).DrawTube((start, end, _) => lines.Add((start, end)));

			for (int index = 0; index < points.Length; index++)
			{
				Assert.Equal(
					Projection.Project(
						points[index].X, points[index].Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					lines[index].Start);
				Assert.Equal(
					Projection.Project(
						points[index].X, points[index].Y,
						configuration.Position + configuration.Depth,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					lines[index].End);
			}
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void AtariPresentationRenderingDoesNotChangeLevelEightMotionOrTiming()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame control = new(startingProductionLevel: 8, random: new Random(1981));
			TempestGame rendered = new(startingProductionLevel: 8, random: new Random(1981));
			VectorRenderer renderer = new(rendered, atariPresentationEnabled: true);

			for (int tick = 0; tick < 96; tick++)
			{
				PlayerInput input = new(tick % 16 == 0, tick % 2 == 0 ? 3 : -2, false, false);
				control.RunOneStep(input);
				rendered.RunOneStep(input);

				renderer.DrawTube((_, _, _) => { });
				renderer.DrawSpikes((_, _) => { }, _ => { });
				renderer.DrawEggs((_, _) => { });
				renderer.DrawPlayerShots((_, _) => { });
				renderer.DrawEnemyBullets((_, _, _) => { }, (_, _) => { });
				renderer.DrawTankers((_, _) => { });
				renderer.DrawSpikers((_, _) => { });
				foreach (Flipper flipper in rendered.Flippers)
					_ = renderer.ProjectFlipperEndpoints(flipper);

				AssertEquivalentGameplayState(control, rendered);
			}
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	private static Vector2 ProjectPoint(
		LevelConfiguration configuration,
		RomPresentationMetadata metadata,
		Vector2 point,
		float depth) =>
		AtariPresentationAdapter.Project(
			8,
			point.X,
			point.Y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			0,
			0,
			Projection.K,
			Projection.ZOffset,
			metadata).Position;

	private static void AssertEquivalentGameplayState(TempestGame expected, TempestGame actual)
	{
		Assert.Equal(expected.State, actual.State);
		Assert.Equal(expected.Score.Score, actual.Score.Score);
		Assert.Equal(expected.Player.Lives, actual.Player.Lives);
		Assert.Equal(
			(expected.Player.EdgeIndex, expected.Player.EdgePosition, expected.Player.Depth),
			(actual.Player.EdgeIndex, actual.Player.EdgePosition, actual.Player.Depth));
		Assert.Equal(expected.SuperZapperChargesRemaining, actual.SuperZapperChargesRemaining);
		Assert.Equal(expected.CurrentLevel.EnemiesSpawned, actual.CurrentLevel.EnemiesSpawned);
		Assert.Equal(expected.CurrentLevel.EnemiesDestroyed, actual.CurrentLevel.EnemiesDestroyed);
		Assert.Equal(expected.CurrentLevel.EnemiesRimCleared, actual.CurrentLevel.EnemiesRimCleared);
		Assert.Equal(expected.CurrentLevel.RemainingEnemies, actual.CurrentLevel.RemainingEnemies);
		Assert.Equal(expected.CurrentLevel.Definition.Points, actual.CurrentLevel.Definition.Points);
		Assert.Equal(
			expected.Eggs.Select(egg =>
				(egg.EdgeIndex, egg.Height, egg.UpdateCounter, egg.MovementDirection,
					egg.VerticalDirection, egg.State)),
			actual.Eggs.Select(egg =>
				(egg.EdgeIndex, egg.Height, egg.UpdateCounter, egg.MovementDirection,
					egg.VerticalDirection, egg.State)));
		Assert.Equal(
			expected.Flippers.Select(flipper =>
				(flipper.EdgeIndex, flipper.Height, flipper.State, flipper.Direction,
					flipper.SourceEdge, flipper.DestinationEdge, flipper.TurnAngle,
					flipper.TotalTurnAngle, flipper.ShotDelayTicks)),
			actual.Flippers.Select(flipper =>
				(flipper.EdgeIndex, flipper.Height, flipper.State, flipper.Direction,
					flipper.SourceEdge, flipper.DestinationEdge, flipper.TurnAngle,
					flipper.TotalTurnAngle, flipper.ShotDelayTicks)));
		Assert.Equal(
			expected.Tankers.Select(tanker =>
				(tanker.EdgeIndex, tanker.Height, tanker.State, tanker.BulletAmmunition,
					tanker.BulletTimer, tanker.LatentCargoCount, tanker.CargoReleased)),
			actual.Tankers.Select(tanker =>
				(tanker.EdgeIndex, tanker.Height, tanker.State, tanker.BulletAmmunition,
					tanker.BulletTimer, tanker.LatentCargoCount, tanker.CargoReleased)));
		Assert.Equal(
			expected.Spikers.Select(spiker =>
				(spiker.EdgeIndex, spiker.Height, spiker.State, spiker.Growth,
					spiker.Rotation, spiker.ShotDelayTicks)),
			actual.Spikers.Select(spiker =>
				(spiker.EdgeIndex, spiker.Height, spiker.State, spiker.Growth,
					spiker.Rotation, spiker.ShotDelayTicks)));
		Assert.Equal(
			expected.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
			actual.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
		Assert.Equal(
			expected.EnemyBullets.Select(bullet => (bullet.EdgeIndex, bullet.Height)),
			actual.EnemyBullets.Select(bullet => (bullet.EdgeIndex, bullet.Height)));
		Assert.Equal(
			expected.Spikes.Select(spike => (spike.EdgeIndex, spike.Height)),
			actual.Spikes.Select(spike => (spike.EdgeIndex, spike.Height)));
	}

	private static (float MinX, float MaxX, float MinY, float MaxY) GetBounds(
		IEnumerable<Vector2> points)
	{
		Vector2[] values = points.ToArray();
		Assert.NotEmpty(values);
		return (
			values.Min(point => point.X),
			values.Max(point => point.X),
			values.Min(point => point.Y),
			values.Max(point => point.Y));
	}
}
