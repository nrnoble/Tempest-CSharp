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
public sealed class VectorRendererAtariPresentationTests
{
	private const float Level1Placement = 122;
	private const float Level2Placement = 114;
	private const float Level3Placement = 114.29437f;
	private const float Level4Placement = 115.20346f;

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	public void CandidateTubeKeepsPointOrderTopologyAndUsesOnlyEstablishedMappings(int level)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(level);
			TempestGame game = new(configuration, random: new Random(1));
			Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
			bool originalTopology = game.CurrentLevel.Definition.Continuous;
			int originalLaneCount = game.CurrentLevel.PlayableLaneCount;
			float originalPosition = configuration.Position;
			float originalDepth = configuration.Depth;
			int originalPlayerLane = game.Player.EdgeIndex;
			float originalPlayerEdgePosition = game.Player.EdgePosition;
			float originalPlayerDepth = game.Player.Depth;
			LevelState originalState = game.CurrentLevel.State;
			int originalShotCount = game.Shots.Count;
			int originalEnemyCount = game.Eggs.Count + game.Flippers.Count +
				game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count +
				game.Pulsars.Count + game.EnemyBullets.Count;
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);
			List<(Vector2 Start, Vector2 End, Color Color)> lines = [];

			candidate.DrawTube((start, end, color) => lines.Add((start, end, color)));
			List<(Vector2 Start, Vector2 End, Color Color)> firstFrame = [.. lines];
			lines.Clear();
			candidate.DrawTube((start, end, color) => lines.Add((start, end, color)));

			Assert.Equal(firstFrame, lines);
			Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(originalTopology, game.CurrentLevel.Definition.Continuous);
			Assert.Equal(originalLaneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(originalPosition, configuration.Position);
			Assert.Equal(originalDepth, configuration.Depth);
			Assert.Equal(originalPlayerLane, game.Player.EdgeIndex);
			Assert.Equal(originalPlayerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(originalPlayerDepth, game.Player.Depth);
			Assert.Equal(originalState, game.CurrentLevel.State);
			Assert.Equal(originalShotCount, game.Shots.Count);
			Assert.Equal(originalEnemyCount, game.Eggs.Count + game.Flippers.Count +
				game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count +
				game.Pulsars.Count + game.EnemyBullets.Count);
			Assert.Equal(originalPoints.Length + originalLaneCount * 2, lines.Count);
			AtariTubeVectorList? level7Vectors = level == 7
				? AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06)
				: null;

			if (level == 1)
			{
				List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
				new VectorRenderer(game).DrawTube((start, end, color) =>
					legacyLines.Add((start, end, color)));
				Assert.Equal(
					legacyLines.Select(line =>
						(line.Start + new Vector2(0, Level1Placement),
							line.End + new Vector2(0, Level1Placement), line.Color)),
					lines);
			}
			for (int index = 0; index < originalPoints.Length; index++)
			{
				Vector2 point = originalPoints[index];
				Vector2 expectedNear;
				Vector2 expectedFar;
				if (level == 1)
				{
					expectedNear = Projection.Project(
						point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth) +
						new Vector2(0, Level1Placement);
					expectedFar = Projection.Project(
						point.X, point.Y, configuration.Position + configuration.Depth,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth) +
						new Vector2(0, Level1Placement);
				}
				else if (level == 5)
				{
					expectedNear = ProjectLevel5CandidatePoint(
						configuration, point, configuration.Position).Position;
					expectedFar = ProjectLevel5CandidatePoint(
						configuration, point, configuration.Position + configuration.Depth).Position;
				}
				else if (level == 6)
				{
					expectedNear = ProjectLevel6CandidatePoint(
						configuration, point, configuration.Position).Position;
					expectedFar = ProjectLevel6CandidatePoint(
						configuration, point, configuration.Position + configuration.Depth).Position;
				}
				else if (level7Vectors is not null)
				{
					expectedNear = AtariLevel7Presentation.MapNearCentered(
						level7Vectors.NearPoints[index]);
					expectedFar = AtariLevel7Presentation.MapFarCentered(
						level7Vectors.FarPoints[index]);
				}
				else if (AtariPresentationAdapter.HasModelFrameMapping(level))
				{
					RomPresentationMetadata metadata =
						RomPresentationMetadataCatalog.ForDisplayedLevel(level);
					AtariPresentationAdapter.ValidateTopology(metadata, originalTopology);
					expectedNear = AtariPresentationAdapter.Project(
						level, point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Depth, 0, 0, 70, 5, metadata).Position;
					expectedFar = AtariPresentationAdapter.Project(
						level, point.X, point.Y, configuration.Position + configuration.Depth,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Depth, 0, 0, 70, 5, metadata).Position;
				}
				else
				{
					expectedNear = Projection.Project(
						point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth);
					expectedFar = Projection.Project(
						point.X, point.Y, configuration.Position + configuration.Depth,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth);
				}

				Assert.Equal(expectedNear, lines[index].Start);
				Assert.Equal(expectedFar, lines[index].End);
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
	public void Level6CandidateIsOptInAndTransformsWorldAnchorsWithoutMutatingGameplay()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(6), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			Vector2[] storedPoints = points.ToArray();
			bool topology = game.CurrentLevel.Definition.Continuous;
			int laneCount = game.CurrentLevel.PlayableLaneCount;
			int playerLane = game.Player.EdgeIndex;
			float playerEdgePosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			float configurationPosition = game.CurrentLevel.Configuration.Position;
			float configurationDepth = game.CurrentLevel.Configuration.Depth;
			LevelState levelState = game.CurrentLevel.State;
			int shotCount = game.Shots.Count;
			int enemyCount = game.Eggs.Count + game.Flippers.Count + game.Spikers.Count +
				game.Tankers.Count + game.Fuseballs.Count + game.Pulsars.Count +
				game.EnemyBullets.Count;

			List<(Vector2 Start, Vector2 End, Color Color)> legacyTube = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateTube = [];
			legacy.DrawTube((start, end, color) => legacyTube.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateTube.Add((start, end, color)));
			List<(Vector2 Start, Vector2 End, Color Color)> repeatedTube = [];
			candidate.DrawTube((start, end, color) => repeatedTube.Add((start, end, color)));
			Assert.Equal(candidateTube, repeatedTube);
			Assert.Equal(points.Count * 2 + laneCount, candidateTube.Count);

			for (int index = 0; index < points.Count; index++)
			{
				Vector2 point = points[index];
				Assert.Equal(
					Projection.Project(point.X, point.Y, configurationPosition,
						game.CurrentLevel.Definition.CenterY,
						game.CurrentLevel.Definition.YOffset,
						configurationPosition, configurationPosition + configurationDepth),
					legacyTube[index].Start);
				Assert.Equal(
					Projection.Project(point.X, point.Y,
						configurationPosition + configurationDepth,
						game.CurrentLevel.Definition.CenterY,
						game.CurrentLevel.Definition.YOffset,
						configurationPosition, configurationPosition + configurationDepth),
					legacyTube[index].End);
				Assert.Equal(
					ProjectLevel6CandidatePoint(
						game.CurrentLevel.Configuration, point, configurationPosition).Position,
					candidateTube[index].Start);
				Assert.Equal(
					ProjectLevel6CandidatePoint(
						game.CurrentLevel.Configuration, point,
						configurationPosition + configurationDepth).Position,
					candidateTube[index].End);
			}

			candidate.GetPlayerEdge(points, out Vector2 playerStart, out Vector2 playerEnd);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					points[playerLane], candidate.PlayerProjectedDepth).Position,
				playerStart);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					points[(playerLane + 1) % points.Count],
					candidate.PlayerProjectedDepth).Position,
				playerEnd);

			Tanker tanker = new(7, 0);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					(points[tanker.EdgeIndex] +
					 points[(tanker.EdgeIndex + 1) % points.Count]) / 2,
					configurationPosition + configurationDepth - tanker.Height).Position,
				candidate.ProjectTankerCenter(tanker));
			EnemyBullet enemyBullet = new(5, 200);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					(points[enemyBullet.EdgeIndex] +
					 points[(enemyBullet.EdgeIndex + 1) % points.Count]) / 2,
					configurationPosition + configurationDepth - enemyBullet.Height).Position,
				candidate.ProjectEnemyBullet(enemyBullet));
			Fuseball fuseball = new(6);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					Vector2.Lerp(points[fuseball.EdgeIndex],
						points[(fuseball.EdgeIndex + 1) % points.Count], fuseball.W / 8f),
					configurationPosition + configurationDepth - fuseball.Height).Position,
				candidate.ProjectFuseballCenter(fuseball));
			Flipper flipper = new(4, 200);
			(Vector2 flipperStart, Vector2 flipperEnd) =
				candidate.ProjectFlipperEndpoints(flipper);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					points[flipper.EdgeIndex],
					configurationPosition + configurationDepth - flipper.Height).Position,
				flipperStart);
			Assert.Equal(
				ProjectLevel6CandidatePoint(game.CurrentLevel.Configuration,
					points[(flipper.EdgeIndex + 1) % points.Count],
					configurationPosition + configurationDepth - flipper.Height).Position,
				flipperEnd);

			Vector2 legacyShotCenter = legacy.ProjectShot(shot);
			Vector2 candidateShotCenter = candidate.ProjectShot(shot);
			Vector2 shotMidpoint = (points[shot.EdgeIndex] +
				points[(shot.EdgeIndex + 1) % points.Count]) / 2;
			float shotDepth = configurationPosition + configurationDepth - shot.Height;
			Assert.Equal(
				ProjectLevel6CandidatePoint(
					game.CurrentLevel.Configuration, shotMidpoint, shotDepth).Position,
				candidateShotCenter);
			List<Vector2> legacyShotPoints = [];
			List<Vector2> candidateShotPoints = [];
			legacy.DrawPlayerShots((position, _) => legacyShotPoints.Add(position));
			candidate.DrawPlayerShots((position, _) => candidateShotPoints.Add(position));
			Assert.Equal(legacyShotPoints.Count, candidateShotPoints.Count);
			for (int index = 0; index < legacyShotPoints.Count; index++)
			{
				Assert.InRange(Vector2.Distance(
					legacyShotPoints[index] - legacyShotCenter,
					candidateShotPoints[index] - candidateShotCenter), 0, .001f);
			}

			Assert.NotEqual(legacyTube[0].Start, candidateTube[0].Start);
			Assert.Equal(storedPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(topology, game.CurrentLevel.Definition.Continuous);
			Assert.Equal(laneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(configurationPosition, game.CurrentLevel.Configuration.Position);
			Assert.Equal(configurationDepth, game.CurrentLevel.Configuration.Depth);
			Assert.Equal(levelState, game.CurrentLevel.State);
			Assert.Equal(shotCount, game.Shots.Count);
			Assert.Equal(enemyCount, game.Eggs.Count + game.Flippers.Count + game.Spikers.Count +
				game.Tankers.Count + game.Fuseballs.Count + game.Pulsars.Count +
				game.EnemyBullets.Count);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level5CandidateIsOptInAndTransformsWorldAnchorsWithoutMutatingGameplay()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(5), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			Vector2[] storedPoints = points.ToArray();
			bool topology = game.CurrentLevel.Definition.Continuous;
			int laneCount = game.CurrentLevel.PlayableLaneCount;
			int playerLane = game.Player.EdgeIndex;
			float playerEdgePosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			float configurationPosition = game.CurrentLevel.Configuration.Position;
			float configurationDepth = game.CurrentLevel.Configuration.Depth;
			LevelState levelState = game.CurrentLevel.State;
			int shotCount = game.Shots.Count;
			int enemyCount = game.Eggs.Count + game.Flippers.Count + game.Spikers.Count +
				game.Tankers.Count + game.Fuseballs.Count + game.Pulsars.Count +
				game.EnemyBullets.Count;

			List<(Vector2 Start, Vector2 End, Color Color)> legacyTube = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateTube = [];
			legacy.DrawTube((start, end, color) => legacyTube.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateTube.Add((start, end, color)));
			for (int index = 0; index < points.Count; index++)
			{
				Vector2 point = points[index];
				Assert.Equal(
					Projection.Project(point.X, point.Y, configurationPosition,
						game.CurrentLevel.Definition.CenterY,
						game.CurrentLevel.Definition.YOffset,
						configurationPosition, configurationPosition + configurationDepth),
					legacyTube[index].Start);
				Assert.Equal(
					ProjectLevel5CandidatePoint(
						game.CurrentLevel.Configuration, point, configurationPosition).Position,
					candidateTube[index].Start);
				Assert.Equal(
					ProjectLevel5CandidatePoint(
						game.CurrentLevel.Configuration, point,
						configurationPosition + configurationDepth).Position,
					candidateTube[index].End);
			}

			legacy.GetPlayerEdge(points, out Vector2 legacyPlayerStart,
				out _);
			candidate.GetPlayerEdge(points, out Vector2 candidatePlayerStart,
				out Vector2 candidatePlayerEnd);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					points[playerLane], candidate.PlayerProjectedDepth).Position,
				candidatePlayerStart);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					points[(playerLane + 1) % points.Count],
					candidate.PlayerProjectedDepth).Position,
				candidatePlayerEnd);

			Tanker tanker = new(7, 0);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					(points[tanker.EdgeIndex] +
					 points[(tanker.EdgeIndex + 1) % points.Count]) / 2,
					configurationPosition + configurationDepth - tanker.Height).Position,
				candidate.ProjectTankerCenter(tanker));
			EnemyBullet enemyBullet = new(5, 200);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					(points[enemyBullet.EdgeIndex] +
					 points[(enemyBullet.EdgeIndex + 1) % points.Count]) / 2,
					configurationPosition + configurationDepth - enemyBullet.Height).Position,
				candidate.ProjectEnemyBullet(enemyBullet));
			Fuseball fuseball = new(6);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					Vector2.Lerp(points[fuseball.EdgeIndex],
						points[(fuseball.EdgeIndex + 1) % points.Count], fuseball.W / 8f),
					configurationPosition + configurationDepth - fuseball.Height).Position,
				candidate.ProjectFuseballCenter(fuseball));
			Flipper flipper = new(4, 200);
			(Vector2 flipperStart, Vector2 flipperEnd) =
				candidate.ProjectFlipperEndpoints(flipper);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					points[flipper.EdgeIndex],
					configurationPosition + configurationDepth - flipper.Height).Position,
				flipperStart);
			Assert.Equal(
				ProjectLevel5CandidatePoint(game.CurrentLevel.Configuration,
					points[(flipper.EdgeIndex + 1) % points.Count],
					configurationPosition + configurationDepth - flipper.Height).Position,
				flipperEnd);

			List<Vector2> legacyShotPoints = [];
			List<Vector2> candidateShotPoints = [];
			legacy.DrawPlayerShots((position, _) => legacyShotPoints.Add(position));
			candidate.DrawPlayerShots((position, _) => candidateShotPoints.Add(position));
			Vector2 legacyShotCenter = legacy.ProjectShot(shot);
			Vector2 candidateShotCenter = candidate.ProjectShot(shot);
			Vector2 shotMidpoint = (points[shot.EdgeIndex] +
				points[(shot.EdgeIndex + 1) % points.Count]) / 2;
			float shotDepth = configurationPosition + configurationDepth - shot.Height;
			Assert.Equal(
				ProjectLevel5CandidatePoint(
					game.CurrentLevel.Configuration, shotMidpoint, shotDepth).Position,
				candidateShotCenter);
			Assert.Equal(legacyShotPoints.Count, candidateShotPoints.Count);
			for (int index = 0; index < legacyShotPoints.Count; index++)
			{
				Vector2 expected = candidateShotCenter +
					(legacyShotPoints[index] - legacyShotCenter) *
					AtariPresentationAdapter.Level5VerticalPresentationScale;
				Assert.InRange(Vector2.Distance(candidateShotPoints[index], expected), 0, .001f);
			}
			Assert.NotEqual(legacyPlayerStart, candidatePlayerStart);
			Assert.Equal(storedPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(topology, game.CurrentLevel.Definition.Continuous);
			Assert.Equal(laneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(configurationPosition, game.CurrentLevel.Configuration.Position);
			Assert.Equal(configurationDepth, game.CurrentLevel.Configuration.Depth);
			Assert.Equal(levelState, game.CurrentLevel.State);
			Assert.Equal(shotCount, game.Shots.Count);
			Assert.Equal(enemyCount, game.Eggs.Count + game.Flippers.Count +
				game.Spikers.Count + game.Tankers.Count + game.Fuseballs.Count +
				game.Pulsars.Count + game.EnemyBullets.Count);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level2LegacyRenderingRemainsExactWhenCandidateIsDisabled()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(2), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);
			List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];

			legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));

			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			for (int index = 0; index < points.Count; index++)
			{
				Vector2 point = points[index];
				Assert.Equal(
					ProjectCandidatePoint(game, point, configuration.Position).Position,
					candidateLines[index].Start);
				Assert.Equal(
					ProjectCandidatePoint(
						game, point, configuration.Position + configuration.Depth).Position,
					candidateLines[index].End);
				Assert.Equal(
					Projection.Project(
						point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					legacyLines[index].Start);
				Assert.Equal(
					Projection.Project(
						point.X, point.Y, configuration.Position + configuration.Depth,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					legacyLines[index].End);
			}
			Assert.Equal((1.436688312f, 1),
				AtariPresentationAdapter.GetModelFrameMapping(2));
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level3LegacyRenderingRemainsExactWhenCandidateIsDisabled()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(3), random: new Random(1));
			VectorRenderer legacy = new(game);
			List<(Vector2 Start, Vector2 End, Color Color)> lines = [];

			legacy.DrawTube((start, end, color) => lines.Add((start, end, color)));

			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			for (int index = 0; index < points.Count; index++)
			{
				Vector2 point = points[index];
				Assert.Equal(
					Projection.Project(
						point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					lines[index].Start);
				Assert.Equal(
					Projection.Project(
						point.X, point.Y, configuration.Position + configuration.Depth,
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
	public void Level4CandidateTranslatesLegacyTubeAndPreservesLegacyOutput()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			LevelConfiguration configuration = GetConfiguration(4);
			TempestGame game = new(configuration, random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);
			List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];

			legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));

			Assert.Equal(legacyLines.Count, candidateLines.Count);
			for (int index = 0; index < legacyLines.Count; index++)
			{
				Assert.Equal(legacyLines[index].Color, candidateLines[index].Color);
				Assert.InRange(MathF.Abs(
					legacyLines[index].Start.X - candidateLines[index].Start.X), 0, .0001f);
				Assert.InRange(MathF.Abs(
					legacyLines[index].Start.Y + Level4Placement - candidateLines[index].Start.Y), 0, .0001f);
				Assert.InRange(MathF.Abs(
					legacyLines[index].End.X - candidateLines[index].End.X), 0, .0001f);
				Assert.InRange(MathF.Abs(
					legacyLines[index].End.Y + Level4Placement - candidateLines[index].End.Y), 0, .0001f);
			}
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			RomPresentationMetadata metadata = RomPresentationMetadataCatalog.ForDisplayedLevel(4);
			for (int index = 0; index < points.Count; index++)
			{
				Vector2 point = points[index];
				Assert.Equal(
					Projection.Project(
						point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Position + configuration.Depth),
					legacyLines[index].Start);
				Assert.Equal(
					AtariPresentationAdapter.Project(
						4, point.X, point.Y, configuration.Position,
						configuration.Definition.CenterY, configuration.Definition.YOffset,
						configuration.Position, configuration.Depth, 0, 0, 70, 5, metadata).Position,
					candidateLines[index].Start);
			}

			AtariPresentationProjection nearCenter = AtariPresentationAdapter.Project(
				4, 0, 0, configuration.Position, configuration.Definition.CenterY,
				configuration.Definition.YOffset, configuration.Position,
				configuration.Depth, 0, 0, 70, 5, metadata);
			Assert.InRange(MathF.Abs(nearCenter.Position.X - 400), 0, .001f);
			Assert.InRange(MathF.Abs(nearCenter.Position.Y -
				(configuration.Definition.CenterY +
					configuration.Definition.YOffset * 70 /
					(configuration.Position + 5) + Level4Placement)), 0, .001f);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level4WorldAnchorsSharePlacementWithoutMutatingGameplay()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(4), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);
			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			Egg egg = game.Eggs[0];
			Vector2[] points = game.CurrentLevel.Definition.Points.ToArray();
			int laneCount = game.CurrentLevel.PlayableLaneCount;
			int playerLane = game.Player.EdgeIndex;
			float playerPosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			LevelState state = game.CurrentLevel.State;
			int shotCount = game.Shots.Count;

			legacy.GetPlayerEdge(points, out Vector2 legacyStart, out Vector2 legacyEnd);
			candidate.GetPlayerEdge(points, out Vector2 candidateStart, out Vector2 candidateEnd);
			AssertPlacementDelta(legacyStart, candidateStart, Level4Placement);
			AssertPlacementDelta(legacyEnd, candidateEnd, Level4Placement);
			AssertPlacementDelta(legacy.ProjectShot(shot), candidate.ProjectShot(shot), Level4Placement);
			AssertPlacementDelta(legacy.ProjectEgg(egg), candidate.ProjectEgg(egg), Level4Placement);
			Tanker tanker = new(7, 0);
			AssertPlacementDelta(legacy.ProjectTankerCenter(tanker),
				candidate.ProjectTankerCenter(tanker), Level4Placement);
			Flipper flipper = new(4, 200);
			var oldFlipper = legacy.ProjectFlipperEndpoints(flipper);
			var newFlipper = candidate.ProjectFlipperEndpoints(flipper);
			AssertPlacementDelta(oldFlipper.Start, newFlipper.Start, Level4Placement);
			AssertPlacementDelta(oldFlipper.End, newFlipper.End, Level4Placement);
			EnemyBullet bullet = new(5, 200);
			AssertPlacementDelta(legacy.ProjectEnemyBullet(bullet),
				candidate.ProjectEnemyBullet(bullet), Level4Placement);
			Fuseball fuseball = new(6);
			AssertPlacementDelta(legacy.ProjectFuseballCenter(fuseball),
				candidate.ProjectFuseballCenter(fuseball), Level4Placement);

			List<(Vector2 Start, Vector2 End)> oldSpikes = [];
			List<(Vector2 Start, Vector2 End)> newSpikes = [];
			List<Vector2> oldTips = [];
			List<Vector2> newTips = [];
			legacy.DrawSpikes((start, end) => oldSpikes.Add((start, end)), oldTips.Add);
			candidate.DrawSpikes((start, end) => newSpikes.Add((start, end)), newTips.Add);
			Assert.NotEmpty(oldSpikes);
			Assert.Equal(oldSpikes.Count, newSpikes.Count);
			Assert.Equal(oldTips.Count, newTips.Count);
			for (int i = 0; i < oldSpikes.Count; i++)
			{
				AssertPlacementDelta(oldSpikes[i].Start, newSpikes[i].Start, Level4Placement);
				AssertPlacementDelta(oldSpikes[i].End, newSpikes[i].End, Level4Placement);
				AssertPlacementDelta(oldTips[i], newTips[i], Level4Placement);
			}

			Assert.Equal(points, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(laneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerPosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(state, game.CurrentLevel.State);
			Assert.Equal(shotCount, game.Shots.Count);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level1CandidateMovesTubePlayerAndShotAnchorsWithoutChangingGameplayState()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(1), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);

			Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
			int playerLane = game.Player.EdgeIndex;
			float playerEdgePosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			int shotCount = game.Shots.Count;
			LevelState levelState = game.CurrentLevel.State;

			List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];
			legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));
			Assert.Equal(
				legacyLines.Select(line =>
					(line.Start + new Vector2(0, Level1Placement),
						line.End + new Vector2(0, Level1Placement), line.Color)),
				candidateLines);

			legacy.GetPlayerEdge(modelPoints, out Vector2 legacyPlayerStart,
				out Vector2 legacyPlayerEnd);
			candidate.GetPlayerEdge(modelPoints, out Vector2 candidatePlayerStart,
				out Vector2 candidatePlayerEnd);
			AssertPlacementDelta(legacyPlayerStart, candidatePlayerStart);
			AssertPlacementDelta(legacyPlayerEnd, candidatePlayerEnd);
			Assert.InRange(Vector2.Distance(
				candidatePlayerEnd - candidatePlayerStart,
				legacyPlayerEnd - legacyPlayerStart), 0, .001f);

			AssertPlacementDelta(legacy.ProjectShot(shot), candidate.ProjectShot(shot));
			Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(shotCount, game.Shots.Count);
			Assert.Equal(levelState, game.CurrentLevel.State);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level2CandidateUsesOneRenderTransformForTubeAndWorldAnchors()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(2), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			Egg egg = game.Eggs[0];

			Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
			int laneCount = game.CurrentLevel.PlayableLaneCount;
			int playerLane = game.Player.EdgeIndex;
			float playerEdgePosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			int shotCount = game.Shots.Count;
			LevelState levelState = game.CurrentLevel.State;

			List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];
			legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));

			legacy.GetPlayerEdge(modelPoints, out Vector2 legacyPlayerStart,
				out Vector2 legacyPlayerEnd);
			candidate.GetPlayerEdge(modelPoints, out Vector2 candidatePlayerStart,
				out Vector2 candidatePlayerEnd);
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			AssertLevel2Aspect(legacyPlayerStart, candidatePlayerStart,
				candidate.PlayerProjectedDepth, configuration);
			AssertLevel2Aspect(legacyPlayerEnd, candidatePlayerEnd,
				candidate.PlayerProjectedDepth, configuration);
			float shotDepth = configuration.Position + configuration.Depth - shot.Height;
			AssertLevel2Aspect(legacy.ProjectShot(shot), candidate.ProjectShot(shot),
				shotDepth, configuration);
			float eggDepth = configuration.Position + configuration.Depth - egg.Height * 20;
			AssertLevel2Aspect(legacy.ProjectEgg(egg), candidate.ProjectEgg(egg),
				eggDepth, configuration);
			float anchoredDepth = configuration.Position + configuration.Depth - 200;
			Tanker tanker = new(7, 0);
			AssertLevel2Aspect(
				legacy.ProjectTankerCenter(tanker),
				candidate.ProjectTankerCenter(tanker),
				configuration.Position + configuration.Depth - tanker.Height,
				configuration);
			AssertLevel2Aspect(
				legacy.ProjectFlipperEndpoints(new Flipper(4, 200)).Start,
				candidate.ProjectFlipperEndpoints(new Flipper(4, 200)).Start,
				anchoredDepth, configuration);
			AssertLevel2Aspect(
				legacy.ProjectEnemyBullet(new EnemyBullet(5, 200)),
				candidate.ProjectEnemyBullet(new EnemyBullet(5, 200)),
				anchoredDepth, configuration);
			Fuseball fuseball = new(6);
			AssertLevel2Aspect(
				legacy.ProjectFuseballCenter(fuseball),
				candidate.ProjectFuseballCenter(fuseball),
				configuration.Position + configuration.Depth - fuseball.Height,
				configuration);

			Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(laneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(shotCount, game.Shots.Count);
			Assert.Equal(levelState, game.CurrentLevel.State);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void Level3CandidateUsesOnePlacementForTubeAndWorldAnchorsWithoutMutatingGameplay()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			TempestGame game = new(GetConfiguration(3), random: new Random(1));
			VectorRenderer legacy = new(game);
			VectorRenderer candidate = new(game, atariPresentationEnabled: true);

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			Egg egg = game.Eggs[0];

			Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
			int laneCount = game.CurrentLevel.PlayableLaneCount;
			int playerLane = game.Player.EdgeIndex;
			float playerEdgePosition = game.Player.EdgePosition;
			float playerDepth = game.Player.Depth;
			int shotCount = game.Shots.Count;
			LevelState levelState = game.CurrentLevel.State;

			List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
			List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];
			legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
			candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));
			Assert.Equal(legacyLines.Count, candidateLines.Count);
			for (int index = 0; index < legacyLines.Count; index++)
			{
				AssertPlacementDelta(legacyLines[index].Start, candidateLines[index].Start,
					Level3Placement);
				AssertPlacementDelta(legacyLines[index].End, candidateLines[index].End,
					Level3Placement);
				Assert.Equal(legacyLines[index].Color, candidateLines[index].Color);
			}

			legacy.GetPlayerEdge(modelPoints, out Vector2 legacyPlayerStart,
				out Vector2 legacyPlayerEnd);
			candidate.GetPlayerEdge(modelPoints, out Vector2 candidatePlayerStart,
				out Vector2 candidatePlayerEnd);
			AssertPlacementDelta(legacyPlayerStart, candidatePlayerStart, Level3Placement);
			AssertPlacementDelta(legacyPlayerEnd, candidatePlayerEnd, Level3Placement);

			AssertPlacementDelta(legacy.ProjectShot(shot), candidate.ProjectShot(shot),
				Level3Placement);
			AssertPlacementDelta(legacy.ProjectEgg(egg), candidate.ProjectEgg(egg),
				Level3Placement);
			Tanker tanker = new(7, 0);
			AssertPlacementDelta(legacy.ProjectTankerCenter(tanker),
				candidate.ProjectTankerCenter(tanker), Level3Placement);
			Flipper flipper = new(4, 200);
			AssertPlacementDelta(legacy.ProjectFlipperEndpoints(flipper).Start,
				candidate.ProjectFlipperEndpoints(flipper).Start, Level3Placement);
			EnemyBullet enemyBullet = new(5, 200);
			AssertPlacementDelta(legacy.ProjectEnemyBullet(enemyBullet),
				candidate.ProjectEnemyBullet(enemyBullet), Level3Placement);
			Fuseball fuseball = new(6);
			AssertPlacementDelta(legacy.ProjectFuseballCenter(fuseball),
				candidate.ProjectFuseballCenter(fuseball), Level3Placement);

			Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
			Assert.Equal(laneCount, game.CurrentLevel.PlayableLaneCount);
			Assert.Equal(playerLane, game.Player.EdgeIndex);
			Assert.Equal(playerEdgePosition, game.Player.EdgePosition);
			Assert.Equal(playerDepth, game.Player.Depth);
			Assert.Equal(shotCount, game.Shots.Count);
			Assert.Equal(levelState, game.CurrentLevel.State);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	public void SameDepthLaneInterpolationsStayRegisteredAndCandidateShotsUseTheAdapterScale(
		int level)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = 70;
			Projection.ZOffset = 5;
			Projection.PortraitPerspective = 0;
			void AssertCandidatePosition(Vector2 expected, Vector2 actual)
			{
				if (level == 4)
					Assert.InRange(Vector2.Distance(expected, actual), 0, .0001f);
				else
					Assert.Equal(expected, actual);
			}

			TempestGame game = new(GetConfiguration(level), random: new Random(1));
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			Egg egg = game.Eggs[0];
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			int next = (egg.EdgeIndex + 1) % points.Count;
			float eggDepth = game.CurrentLevel.Configuration.Position +
				game.CurrentLevel.Configuration.Depth - egg.Height * 20;
			Vector2 first = ProjectCandidatePoint(game, points[egg.EdgeIndex], eggDepth).Position;
			Vector2 second = ProjectCandidatePoint(game, points[next], eggDepth).Position;
			Vector2 interpolated = renderer.ProjectEgg(egg);
			Assert.InRange(Vector2.Distance(interpolated, (first + second) / 2), 0, .001f);

			renderer.GetPlayerEdge(points, out Vector2 playerStart, out Vector2 playerEnd);
			int playerNext = (game.Player.EdgeIndex + 1) % points.Count;
			AssertCandidatePosition(
				ProjectCandidatePoint(game, points[game.Player.EdgeIndex],
					renderer.PlayerProjectedDepth).Position,
				playerStart);
			AssertCandidatePosition(
				ProjectCandidatePoint(game, points[playerNext],
					renderer.PlayerProjectedDepth).Position,
				playerEnd);

			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Tanker tanker = new(7, 0);
			Vector2 tankerMidpoint = (points[tanker.EdgeIndex] +
				points[(tanker.EdgeIndex + 1) % points.Count]) / 2;
			AssertCandidatePosition(
				ProjectCandidatePoint(game, tankerMidpoint,
					configuration.Position + configuration.Depth - tanker.Height).Position,
				renderer.ProjectTankerCenter(tanker));

			Flipper flipper = new(4, 200);
			float projectedFlipperDepth =
				configuration.Position + configuration.Depth - flipper.Height;
			var flipperEndpoints = renderer.ProjectFlipperEndpoints(flipper);
			AssertCandidatePosition(
				ProjectCandidatePoint(game, points[flipper.EdgeIndex], projectedFlipperDepth).Position,
				flipperEndpoints.Start);
			AssertCandidatePosition(
				ProjectCandidatePoint(game,
					points[(flipper.EdgeIndex + 1) % points.Count],
					projectedFlipperDepth).Position,
				flipperEndpoints.End);

			EnemyBullet enemyBullet = new(5, 200);
			Vector2 bulletMidpoint = (points[enemyBullet.EdgeIndex] +
				points[(enemyBullet.EdgeIndex + 1) % points.Count]) / 2;
			AssertCandidatePosition(
				ProjectCandidatePoint(game, bulletMidpoint,
					configuration.Position + configuration.Depth - enemyBullet.Height).Position,
				renderer.ProjectEnemyBullet(enemyBullet));

			Fuseball fuseball = new(6);
			Vector2 fuseballPosition = Vector2.Lerp(points[fuseball.EdgeIndex],
				points[(fuseball.EdgeIndex + 1) % points.Count], fuseball.W / 8f);
			AssertCandidatePosition(
				ProjectCandidatePoint(game, fuseballPosition,
					configuration.Position + configuration.Depth - fuseball.Height).Position,
				renderer.ProjectFuseballCenter(fuseball));

			if (level == 4)
			{
				Assert.NotEmpty(game.Spikes);
				List<(Vector2 Start, Vector2 End)> spikeLines = [];
				List<Vector2> spikeTips = [];
				renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)),
					tip => spikeTips.Add(tip));
				Assert.Equal(game.Spikes.Count, spikeLines.Count);
				Assert.Equal(game.Spikes.Count, spikeTips.Count);
				for (int index = 0; index < game.Spikes.Count; index++)
				{
					Spike spike = game.Spikes[index];
					int nextSpikeLane = (spike.EdgeIndex + 1) % points.Count;
					Vector2 spikeMidpoint = (points[spike.EdgeIndex] + points[nextSpikeLane]) / 2;
					Vector2 expectedStart = ProjectCandidatePoint(game, spikeMidpoint,
						configuration.Position + configuration.Depth).Position;
					Vector2 expectedTip = ProjectCandidatePoint(game, spikeMidpoint,
						configuration.Position + configuration.Depth - spike.Height).Position;
					AssertCandidatePosition(expectedStart, spikeLines[index].Start);
					AssertCandidatePosition(expectedTip, spikeLines[index].End);
					AssertCandidatePosition(expectedTip, spikeTips[index]);
				}
			}

			game.RunOneStep(new PlayerInput(false, 80, false, false));
			for (int tick = 0; tick < 3; tick++)
				game.RunOneStep(default);
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			List<(Vector2 Position, Color Color)> renderedPoints = [];
			renderer.DrawPlayerShots((position, color) =>
				renderedPoints.Add((position, color)));

			Vector2 center = renderer.ProjectShot(shot);
			float depth = game.CurrentLevel.Configuration.Position +
				game.CurrentLevel.Configuration.Depth - shot.Height;
			float localScale = ProjectCandidatePoint(game,
				(points[shot.EdgeIndex] + points[(shot.EdgeIndex + 1) % points.Count]) / 2,
				depth).LocalScale;
			float coordinateScale = VectorRenderer.PlayerShotBaseSize * localScale /
				PlayerShotVectorProgram.MaximumPointRadius;
			int pointIndex = 0;
			foreach (PlayerShotVectorPointGroup group in PlayerShotVectorProgram.Groups)
			{
				foreach (Vector2 point in group.Points)
				{
					Vector2 expected = center +
						new Vector2(point.X, -point.Y) * coordinateScale;
					AssertCandidatePosition(expected, renderedPoints[pointIndex].Position);
					Assert.Equal(group.ProjectColor, renderedPoints[pointIndex].Color);
					pointIndex++;
				}
			}
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Theory]
	[InlineData(1)]
	[InlineData(6)]
	public void GameOverVectorRecordsRemainOnTheExactLegacyProjectionPath(int level)
	{
		TempestGame game = new(GetConfiguration(level), random: new Random(1));
		game.CurrentLevel.State = LevelState.GameOverAnimation;
		game.GameOverEffect.Start();
		for (int tick = 0; tick <
			GameOverVectorBurstSequence.BurstStartIntervalTicks *
			(GameOverVectorBurstSequence.BurstCount - 1) + 1; tick++)
		{
			game.RunOneStep();
		}
		VectorRenderer legacy = new(game);
		VectorRenderer candidate = new(game, atariPresentationEnabled: true);
		List<(Vector2 Start, Vector2 End, Color Color)> legacyMarks = [];
		List<(Vector2 Start, Vector2 End, Color Color)> candidateMarks = [];

		legacy.DrawGameOverAnimation((start, end, color) =>
			legacyMarks.Add((start, end, color)));
		candidate.DrawGameOverAnimation((start, end, color) =>
			candidateMarks.Add((start, end, color)));

		Assert.NotEmpty(legacyMarks);
		Assert.Equal(legacyMarks, candidateMarks);
	}

	[Theory]
	[InlineData(LevelState.GameOver)]
	[InlineData(LevelState.GameOverPresentation)]
	[InlineData(LevelState.GameOverAnimation)]
	public void Level6GameOverTubeKeepsItsLegacyProjection(LevelState state)
	{
		TempestGame game = new(GetConfiguration(6), random: new Random(1));
		game.CurrentLevel.State = state;
		VectorRenderer legacy = new(game);
		VectorRenderer candidate = new(game, atariPresentationEnabled: true);
		List<(Vector2 Start, Vector2 End, Color Color)> legacyLines = [];
		List<(Vector2 Start, Vector2 End, Color Color)> candidateLines = [];

		legacy.DrawTube((start, end, color) => legacyLines.Add((start, end, color)));
		candidate.DrawTube((start, end, color) => candidateLines.Add((start, end, color)));

		Assert.Equal(legacyLines, candidateLines);
	}

	[Fact]
	public void Level6PresentationCandidateDoesNotUseLegacyFallback()
	{
		TempestGame game = new(GetConfiguration(6), random: new Random(1));
		VectorRenderer candidate = new(game, atariPresentationEnabled: true);
		using StringWriter diagnostics = new();
		TextWriter previousError = Console.Error;
		try
		{
			Console.SetError(diagnostics);
			candidate.DrawTube((_, _, _) => { });
			candidate.DrawTube((_, _, _) => { });
		}
		finally
		{
			Console.SetError(previousError);
		}

		Assert.Empty(diagnostics.ToString());
	}

	[Fact]
	public void CandidateRendererRejectsLegacyPresentationBeforeDrawing()
	{
		VectorRenderer renderer = new(new TempestGame(random: new Random(1)),
			atariPresentationEnabled: true);

		Assert.Throws<InvalidOperationException>(() =>
			renderer.Draw(presentationMode: PresentationMode.Legacy));
	}

	private static AtariPresentationProjection ProjectCandidatePoint(
		TempestGame game,
		Vector2 point,
		float depth)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		if (configuration.DisplayedLevel == 4)
		{
			// Level 4's affine fit is correspondence metadata, not a render-space gain.
			return new AtariPresentationProjection(
				Projection.Project(
					point.X, point.Y, depth, configuration.Definition.CenterY,
					configuration.Definition.YOffset, configuration.Position,
					configuration.Position + configuration.Depth) +
					new Vector2(0, Level4Placement),
				Projection.K / (depth + Projection.ZOffset));
		}

		return AtariPresentationAdapter.Project(
			configuration.DisplayedLevel,
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
			RomPresentationMetadataCatalog.ForDisplayedLevel(configuration.DisplayedLevel));
	}

	private static AtariPresentationProjection ProjectLevel5CandidatePoint(
		LevelConfiguration configuration,
		Vector2 point,
		float depth) =>
		AtariPresentationAdapter.ProjectLevel5(
			point.X,
			point.Y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			0,
			0);

	private static AtariPresentationProjection ProjectLevel6CandidatePoint(
		LevelConfiguration configuration,
		Vector2 point,
		float depth) =>
		AtariPresentationAdapter.ProjectLevel6(
			point.X,
			point.Y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			0,
			0);

	private static LevelConfiguration GetConfiguration(int displayedLevel) =>
		LevelProgression.GetProductionLevel(displayedLevel) ??
		throw new ArgumentOutOfRangeException(nameof(displayedLevel));

	private static void AssertPlacementDelta(
		Vector2 original,
		Vector2 placed,
		float expectedY = Level1Placement)
	{
		Assert.InRange(MathF.Abs(placed.X - original.X), 0, .001f);
		Assert.InRange(MathF.Abs(placed.Y - original.Y - expectedY), 0, .001f);
	}

	private static void AssertLevel2Aspect(
		Vector2 original,
		Vector2 corrected,
		float depth,
		LevelConfiguration configuration)
	{
		float scale = Projection.K / (depth + Projection.ZOffset);
		float tubeCenterY = configuration.Definition.CenterY +
			configuration.Definition.YOffset * scale;
		float expectedY = tubeCenterY +
			(original.Y - tubeCenterY) *
			AtariPresentationAdapter.Level2VerticalAspectFactor +
			Level2Placement;
		Assert.InRange(MathF.Abs(corrected.X - original.X), 0, .001f);
		Assert.InRange(MathF.Abs(corrected.Y - expectedY), 0, .001f);
	}
}
