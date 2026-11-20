using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level9AtariPresentationTests
{
	private const float ProjectionK = 70;
	private const float ProjectionZOffset = 5;

	[Fact]
	public void LevelNineUsesRecoveredShapeThirteenAndCentersItsEnvelopeBelowTheHud()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 9, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			LevelDefinition definition = game.CurrentLevel.Definition;
			RomPresentationMetadata metadata =
				RomPresentationMetadataCatalog.ForDisplayedLevel(9);
			Vector2[] originalPoints = definition.Points.ToArray();
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			List<(Vector2 Start, Vector2 End)> lines = [];

			Assert.Equal(9, metadata.DisplayedLevel);
			Assert.Equal(13, metadata.TubeTableIndex);
			Assert.Equal((byte)0x0C, metadata.ScaleRaw);
			Assert.Equal((byte)0xB0, metadata.CameraHeightRaw);
			Assert.Equal((ushort)0x0140, metadata.ScreenYOffsetCombinedRaw);
			Assert.Equal((short)320, metadata.ScreenYOffsetSigned);
			Assert.Equal((byte)0xFF, metadata.OpenRaw);
			Assert.True(metadata.IsOpen);
			Assert.Equal((byte)0x0D, LevelLoader.Level9RomEvidence.ShapeRemap);
			Assert.Equal(16, definition.VertexCount);
			Assert.False(definition.Continuous);
			Assert.Equal(15, definition.PlayableLaneCount);
			Assert.Equal((25f / 14f, -1), AtariPresentationAdapter.GetModelFrameMapping(9));
			Assert.Equal(50, configuration.Position);
			Assert.Equal(400, configuration.Depth);

			for (int index = 0; index < originalPoints.Length; index++)
			{
				Assert.Equal(
					new Vector2(
						(LevelLoader.Level9RomEvidence.X[index] - 128) * (25f / 14f),
						-(LevelLoader.Level9RomEvidence.Y[index] - 128) * (25f / 14f)),
					originalPoints[index]);
			}

			renderer.DrawTube((start, end, _) => lines.Add((start, end)));

			Assert.Equal(46, lines.Count);
			for (int index = 0; index < originalPoints.Length; index++)
			{
				Assert.Equal(
					ProjectPoint(configuration, metadata, originalPoints[index],
						configuration.Position),
					lines[index].Start);
				Assert.Equal(
					ProjectPoint(configuration, metadata, originalPoints[index],
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
				.Select(point => point - new Vector2(0,
					AtariPresentationAdapter.Level9VerticalPlacement))
				.ToArray();
			(float MinX, float MaxX, float MinY, float MaxY) logicalBounds =
				GetBounds(prePlacementLogicalPoints);
			(float MinX, float MaxX, float MinY, float MaxY) prePlacementHostBounds =
				GetBounds(prePlacementLogicalPoints.Select(ToHost));

			Assert.InRange(MathF.Abs(logicalBounds.MinX - 145.45454f), 0, 0.01f);
			Assert.InRange(MathF.Abs(logicalBounds.MaxX - 654.54547f), 0, 0.01f);
			Assert.InRange(MathF.Abs(logicalBounds.MinY - 119.44444f), 0, 0.01f);
			Assert.InRange(MathF.Abs(logicalBounds.MaxY - 475f), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementHostBounds.MinX -
				PreviousPortraitHostTransform.ToCurrentHost(new(319.27274f, 0)).X), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementHostBounds.MaxX -
				PreviousPortraitHostTransform.ToCurrentHost(new(960.7273f, 0)).X), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementHostBounds.MinY -
				PreviousPortraitHostTransform.ToCurrentHost(new(0, 110.78f)).Y), 0, 0.01f);
			Assert.InRange(MathF.Abs(prePlacementHostBounds.MaxY -
				PreviousPortraitHostTransform.ToCurrentHost(new(0, 558.78f)).Y), 0, 0.01f);

			float hudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
			Vector2 gameplayCenter = new(
				DisplayConfiguration.WindowWidth / 2f,
				(hudBottom + DisplayConfiguration.PortraitViewportHeight) / 2f);
			Vector2 logicalGameplayCenter =
				(gameplayCenter - camera.Offset) / camera.Zoom + camera.Target;
			float calculatedTranslation = logicalGameplayCenter.Y -
				(logicalBounds.MinY + logicalBounds.MaxY) / 2;

			Assert.InRange(MathF.Abs(
				calculatedTranslation - AtariPresentationAdapter.Level9VerticalPlacement), 0, 0.001f);
			Assert.Equal(212.28139f, AtariPresentationAdapter.Level8VerticalPlacement);
			Assert.Equal(50f, AtariPresentationAdapter.Level10VerticalPlacement);

			(float MinX, float MaxX, float MinY, float MaxY) finalBounds =
				GetBounds(projectedPoints.Select(ToHost));
			Assert.InRange(MathF.Abs(finalBounds.MinX - prePlacementHostBounds.MinX), 0, 0.001f);
			Assert.InRange(MathF.Abs(finalBounds.MaxX - prePlacementHostBounds.MaxX), 0, 0.001f);
			Assert.InRange(MathF.Abs(finalBounds.MinX -
				PreviousPortraitHostTransform.ToCurrentHost(new(319.27274f, 0)).X), 0, 0.01f);
			Assert.InRange(MathF.Abs(finalBounds.MaxX -
				PreviousPortraitHostTransform.ToCurrentHost(new(960.7273f, 0)).X), 0, 0.01f);
			Assert.InRange(MathF.Abs(finalBounds.MinY -
				PreviousPortraitHostTransform.ToCurrentHost(new(0, 338.8f)).Y), 0, 0.01f);
			Assert.InRange(MathF.Abs(finalBounds.MaxY -
				PreviousPortraitHostTransform.ToCurrentHost(new(0, 786.8f)).Y), 0, 0.01f);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinX + finalBounds.MaxX) / 2 - gameplayCenter.X), 0, 0.001f);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinY + finalBounds.MaxY) / 2 - gameplayCenter.Y), 0, 0.001f);
			Assert.InRange(MathF.Abs(
				(finalBounds.MinY - hudBottom) -
					(DisplayConfiguration.PortraitViewportHeight - finalBounds.MaxY)), 0, 0.001f);
			Assert.Equal(originalPoints, definition.Points);
			Assert.Equal(ProjectionK, Projection.K);
			Assert.Equal(ProjectionZOffset, Projection.ZOffset);
			Assert.Equal(0, Projection.PortraitPerspective);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LevelNineEntitiesRemainRegisteredToTheSharedTubeProjection()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 9, random: new Random(1));
			Level level = game.CurrentLevel;
			LevelConfiguration configuration = level.Configuration;
			IReadOnlyList<Vector2> points = level.Definition.Points;
			RomPresentationMetadata metadata =
				RomPresentationMetadataCatalog.ForDisplayedLevel(9);
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			List<(Vector2 Start, Vector2 End)> tubeLines = [];
			renderer.DrawTube((start, end, _) => tubeLines.Add((start, end)));

			int lane = game.Player.EdgeIndex;
			int nextLane = level.NextEdge(lane);
			renderer.GetPlayerEdge(points, out Vector2 playerStart, out Vector2 playerEnd);
			Assert.Equal(tubeLines[lane].Start, playerStart);
			Assert.Equal(tubeLines[nextLane].Start, playerEnd);
			Assert.Equal(configuration.Position, renderer.PlayerProjectedDepth);

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
			Vector2 laneMidpoint = GetLaneMidpoint(points, level, lane);
			Vector2 expectedShot = ProjectPoint(configuration, metadata, laneMidpoint, shotDepth);
			Assert.Equal(expectedShot, renderer.ProjectShot(shot));

			EnemyBullet enemyBullet = new(lane, 137);
			Assert.Equal(expectedShot, renderer.ProjectEnemyBullet(enemyBullet));

			Spike spike = game.Spikes[0];
			Vector2 spikeMidpoint = GetLaneMidpoint(points, level, spike.EdgeIndex);
			List<(Vector2 Start, Vector2 End)> spikeLines = [];
			renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), _ => { });
			Assert.Equal(
				ProjectPoint(configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth),
				spikeLines[0].Start);
			Assert.Equal(
				ProjectPoint(configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth - spike.Height),
				spikeLines[0].End);

			float startingSpikeHeight = spike.Height;
			spike.Height += 50;
			spikeLines.Clear();
			renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), _ => { });
			Assert.Equal(startingSpikeHeight + 50, spike.Height);
			Assert.Equal(
				ProjectPoint(configuration, metadata, spikeMidpoint,
					configuration.Position + configuration.Depth - spike.Height),
				spikeLines[0].End);

			Egg egg = game.Eggs[0];
			float eggDepth = configuration.Position + configuration.Depth - egg.Height * 20;
			Assert.True(eggDepth > configuration.Position + configuration.Depth);
			Assert.Equal(
				ProjectPoint(configuration, metadata,
					GetLaneMidpoint(points, level, egg.EdgeIndex), eggDepth),
				renderer.ProjectEgg(egg));
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LevelNineAtariPresentationOptionOffRetainsLegacyProjection()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(startingProductionLevel: 9, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2[] points = game.CurrentLevel.Definition.Points.ToArray();
			List<(Vector2 Start, Vector2 End)> lines = [];

			new VectorRenderer(game).DrawTube((start, end, _) => lines.Add((start, end)));

			Assert.Equal(points.Length + game.CurrentLevel.PlayableLaneCount * 2, lines.Count);
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
	public void AtariPresentationRenderingDoesNotChangeLevelNineSimulation()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame control = new(startingProductionLevel: 9, random: new Random(1981));
			TempestGame rendered = new(startingProductionLevel: 9, random: new Random(1981));
			VectorRenderer renderer = CreateIdentityAtariRenderer(rendered);

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

	[Fact]
	public void LevelNineRecordingFormatAndPlaybackRemainIndependentOfPresentation()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");
		try
		{
			Projection.K = ProjectionK;
			Projection.ZOffset = ProjectionZOffset;
			Projection.PortraitPerspective = 0;

			GameplayStartupConfiguration startup = new(9, 4, 1.0, 1981);
			PlayerInputRecorder recorder = new(startup);
			for (int tick = 0; tick < 96; tick++)
			{
				PlayerInput input = new(tick % 16 == 0, tick % 2 == 0 ? 3 : -2, false, false);
				recorder.CaptureTick(tick, input);
			}

			GameplayRecordingFile.SaveNew(path, recorder);
			using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(path)))
			{
				Assert.Equal(GameplayRecordingFile.CurrentFormatVersion,
					document.RootElement.GetProperty("Version").GetInt32());
				Assert.False(document.RootElement.TryGetProperty(
					"AtariPresentationEnabled", out _));
			}

			GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
			GameplayPlaybackSession standardPlayback =
				new(recording.StartupConfiguration, recording.Replay);
			GameplayPlaybackSession atariPlayback =
				new(recording.StartupConfiguration, recording.Replay);
			VectorRenderer renderer = new(
				atariPlayback.Game, atariPresentationEnabled: true,
				tubeYGain: 1, tubeScale: 1, tubeVerticalOffset: 0);

			for (int tick = 0; tick < recording.Replay.DurationTicks; tick++)
			{
				Assert.True(standardPlayback.StepForward());
				Assert.True(atariPlayback.StepForward());
				renderer.DrawTube((_, _, _) => { });
				renderer.DrawSpikes((_, _) => { }, _ => { });
				renderer.DrawEggs((_, _) => { });
				renderer.DrawPlayerShots((_, _) => { });
				renderer.DrawEnemyBullets((_, _, _) => { }, (_, _) => { });
				foreach (Flipper flipper in atariPlayback.Game.Flippers)
					_ = renderer.ProjectFlipperEndpoints(flipper);
				AssertEquivalentGameplayState(standardPlayback.Game, atariPlayback.Game);
			}
		}
		finally
		{
			if (File.Exists(path))
				File.Delete(path);
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
			9,
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

	private static Vector2 GetLaneMidpoint(
		IReadOnlyList<Vector2> points,
		Level level,
		int lane)
	{
		int nextLane = level.NextEdge(lane);
		return (points[lane] + points[nextLane]) / 2;
	}

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

	private static VectorRenderer CreateIdentityAtariRenderer(TempestGame game) =>
		new(game, atariPresentationEnabled: true,
			tubeYGain: 1, tubeScale: 1, tubeVerticalOffset: 0);

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
