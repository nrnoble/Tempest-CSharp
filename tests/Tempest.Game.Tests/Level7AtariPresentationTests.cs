using System.Numerics;
using System.Reflection;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit.Abstractions;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level7AtariPresentationTests
{
	private const float TestK = 70;
	private const float TestZOffset = 5;
	private readonly ITestOutputHelper output;

	private static readonly Vector2[] ProductionPoints =
	[
		new(-160, 160), new(-179, 44), new(-89, 0), new(-179, -44),
		new(-160, -160), new(-44, -179), new(0, -89), new(44, -179),
		new(160, -160), new(179, -44), new(89, 0), new(179, 44),
		new(160, 160), new(44, 179), new(0, 89), new(-44, 179)
	];

	public Level7AtariPresentationTests(ITestOutputHelper output) =>
		this.output = output;

	[Fact]
	public void ProductionGeometryAndRawRecord06EvidenceRemainUnchanged()
	{
		LevelConfiguration configuration = LevelConfiguration.Level7Production;
		TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
		AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(7);

		Assert.Same(configuration, game.CurrentLevel.Configuration);
		Assert.Equal(ProductionPoints, configuration.Definition.Points);
		Assert.True(configuration.Definition.Continuous);
		Assert.Equal(16, configuration.PlayableLaneCount);
		Assert.Equal(310, configuration.Definition.CenterY);
		Assert.Equal(-10, configuration.Definition.YOffset);
		Assert.Equal(Level.Position, configuration.Position);
		Assert.Equal(Level.Depth, configuration.Depth);
		Assert.Equal(7, configuration.InitialPlayerLane);
		Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
		Assert.Equal((1.678958593f, 1),
			AtariPresentationAdapter.GetModelFrameMapping(7));

		Assert.Equal(6, record.RecordIndex);
		Assert.Equal(16, record.PointCount);
		Assert.Equal(
			new byte[]
			{
				0xEA, 0xE0, 0x9C, 0x80, 0x64, 0x20, 0x16, 0x50,
				0x16, 0x20, 0x64, 0x80, 0x9C, 0xE0, 0xEA, 0xB0
			},
			record.RawXPoints);
		Assert.Equal(
			new byte[]
			{
				0xA0, 0xE0, 0xEA, 0xB0, 0xEA, 0xE0, 0xA0, 0x80,
				0x60, 0x20, 0x16, 0x50, 0x16, 0x20, 0x60, 0x80
			},
			record.RawYPoints);
		Assert.Equal((6, (byte)0x18, (byte)0x68, (short)-144, (byte)0x00),
			(metadata.TubeTableIndex, metadata.ScaleRaw, metadata.CameraHeightRaw,
				metadata.ScreenYOffsetSigned, metadata.OpenRaw));
		Assert.Same(record, AtariTubeRecordCatalog.ForRecordIndex(6));
		Assert.False(metadata.IsOpen);
	}

	[Fact]
	public void ObsoleteRecord06PresentationAndPrototypeTypesAreRemoved()
	{
		Assembly assembly = typeof(VectorRenderer).Assembly;
		string[] removedTypes =
		[
			"Tempest.Game.Rendering.AtariLevel7Presentation",
			"Tempest.Game.Rendering.AtariLevel7EntityPresentation",
			"Tempest.Game.Rendering.AtariTubeVectorListBuilder",
			"Tempest.Game.Rendering.AtariCoordinateProjector",
			"Tempest.Game.Rendering.AtariDvgViewportMap",
			"Tempest.Game.Rendering.AtariTubeReferenceShapeDiagnostic"
		];

		foreach (string typeName in removedTypes)
			Assert.Null(assembly.GetType(typeName));
		Assert.NotNull(assembly.GetType(
			"Tempest.Game.Levels.AtariTubeRecordCatalog"));
	}

	[Fact]
	public void Record06KeepsValidatedDefensiveCopiesOfItsRawPointData()
	{
		byte[] rawX = Enumerable.Repeat((byte)0x80, 16).ToArray();
		byte[] rawY = Enumerable.Repeat((byte)0x68, 16).ToArray();
		AtariTubeRecord record = new(6, rawX, rawY, 0x18, 0x68, -144, 0x00);
		rawX[0] = 0;
		rawY[0] = 0;

		Assert.Equal(0x80, record.RawXPoints[0]);
		Assert.Equal(0x68, record.RawYPoints[0]);
		Assert.Throws<NotSupportedException>(() =>
			Assert.IsAssignableFrom<IList<byte>>(record.RawXPoints)[0] = 0);
		Assert.Throws<NotSupportedException>(() =>
			Assert.IsAssignableFrom<IList<byte>>(record.RawYPoints)[0] = 0);
		Assert.Throws<ArgumentException>(() =>
			new AtariTubeRecord(6, rawX[..15], rawY, 0x18, 0x68, -144, 0x00));
		Assert.Throws<ArgumentException>(() =>
			new AtariTubeRecord(6, rawX, rawY[..15], 0x18, 0x68, -144, 0x00));
	}

	[Fact]
	public void TubeRailsAndClosedRimsUseTheSharedAdapterOnProductionPoints()
	{
		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			List<(Vector2 Start, Vector2 End, Color Color)> lines = [];

			renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));

			Assert.Equal(ProductionPoints.Length * 3, lines.Count);
			for (int lane = 0; lane < ProductionPoints.Length; lane++)
			{
				int next = (lane + 1) % ProductionPoints.Length;
				Vector2 expectedNear = Project(game, ProductionPoints[lane],
					configuration.Position);
				Vector2 expectedFar = Project(game, ProductionPoints[lane],
					configuration.Position + configuration.Depth);

				Assert.Equal(expectedNear, lines[lane].Start);
				Assert.Equal(expectedFar, lines[lane].End);
				Assert.Equal(expectedNear, lines[ProductionPoints.Length + lane * 2].Start);
				Assert.Equal(Project(game, ProductionPoints[next],
					configuration.Position),
					lines[ProductionPoints.Length + lane * 2].End);
				Assert.Equal(expectedFar, lines[ProductionPoints.Length + lane * 2 + 1].Start);
				Assert.Equal(Project(game, ProductionPoints[next],
					configuration.Position + configuration.Depth),
					lines[ProductionPoints.Length + lane * 2 + 1].End);
			}
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	[Fact]
	public void PlacementCentersTheCompleteSharedEnvelopeWithoutHorizontalTranslation()
	{
		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			List<Vector2> envelope = [];
			foreach (Vector2 point in configuration.Definition.Points)
			{
				envelope.Add(Project(game, point, configuration.Position));
				envelope.Add(Project(game, point,
					configuration.Position + configuration.Depth));
			}

			Vector2 minimum = new(
				envelope.Min(point => point.X),
				envelope.Min(point => point.Y));
			Vector2 maximum = new(
				envelope.Max(point => point.X),
				envelope.Max(point => point.Y));
			float placement = AtariPresentationAdapter.Level7VerticalPlacement;
			Vector2 unplacedCenter = (minimum + maximum) / 2 -
				new Vector2(0, placement);
			float expectedLogicalCenterY = GetHudClearedLogicalCenterY();
			Assert.InRange(MathF.Abs(
				placement - (expectedLogicalCenterY - unplacedCenter.Y)), 0, .001f);

			Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
			Vector2 hostCenter = camera.Offset +
				(((minimum + maximum) / 2) - camera.Target) * camera.Zoom;
			float expectedHostY =
				((120 + 18) * DisplayConfiguration.PortraitOutputScale +
					DisplayConfiguration.PortraitViewportHeight) / 2;
			Assert.InRange(MathF.Abs(hostCenter.X - DisplayConfiguration.WindowWidth / 2f),
				0, .001f);
			Assert.InRange(MathF.Abs(hostCenter.Y - expectedHostY), 0, .001f);
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	[Fact]
	public void GameplayEntityAnchorsUseProductionLanePointsAndTheSharedAdapter()
	{
		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			int lane = 3;
			int next = game.CurrentLevel.NextEdge(lane);
			Vector2 midpoint = (ProductionPoints[lane] + ProductionPoints[next]) / 2;

			Tanker tanker = new(lane, 0);
			Assert.Equal(Project(game, midpoint,
					configuration.Position + configuration.Depth - tanker.Height),
				renderer.ProjectTankerCenter(tanker));

			Egg egg = new(lane);
			Assert.Equal(Project(game, midpoint,
					configuration.Position + configuration.Depth - egg.Height * 20),
				renderer.ProjectEgg(egg));

			Shot shot = new() { EdgeIndex = lane, Height = 175 };
			Assert.Equal(Project(game, midpoint,
					configuration.Position + configuration.Depth - shot.Height),
				renderer.ProjectShot(shot));

			EnemyBullet enemyBullet = new(lane, 220);
			Assert.Equal(Project(game, midpoint,
					configuration.Position + configuration.Depth - enemyBullet.Height),
				renderer.ProjectEnemyBullet(enemyBullet));

			Flipper flipper = new(lane, height: 190);
			(Vector2 flipperStart, Vector2 flipperEnd) =
				renderer.ProjectFlipperEndpoints(flipper);
			Assert.Equal(Project(game, ProductionPoints[lane],
					configuration.Position + configuration.Depth - flipper.Height),
				flipperStart);
			Assert.Equal(Project(game, ProductionPoints[next],
					configuration.Position + configuration.Depth - flipper.Height),
				flipperEnd);

			renderer.GetPlayerEdge(ProductionPoints, out Vector2 blasterStart,
				out Vector2 blasterEnd);
			int playerNext = game.CurrentLevel.NextEdge(game.Player.EdgeIndex);
			Assert.Equal(Project(game, ProductionPoints[game.Player.EdgeIndex],
					renderer.PlayerProjectedDepth),
				blasterStart);
			Assert.Equal(Project(game, ProductionPoints[playerNext],
					renderer.PlayerProjectedDepth),
				blasterEnd);

			List<(Vector2 Start, Vector2 End)> spikeLines = [];
			List<Vector2> spikeTips = [];
			renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)),
				spikeTips.Add);
			Assert.Equal(game.CurrentLevel.Spikes.Count, spikeLines.Count);
			for (int index = 0; index < game.CurrentLevel.Spikes.Count; index++)
			{
				Spike spike = game.CurrentLevel.Spikes[index];
				int spikeNext = game.CurrentLevel.NextEdge(spike.EdgeIndex);
				Vector2 spikeMidpoint =
					(ProductionPoints[spike.EdgeIndex] + ProductionPoints[spikeNext]) / 2;
				Assert.Equal(Project(game, spikeMidpoint,
						configuration.Position + configuration.Depth),
					spikeLines[index].Start);
				Assert.Equal(Project(game, spikeMidpoint,
						configuration.Position + configuration.Depth - spike.Height),
					spikeLines[index].End);
				Assert.Equal(spikeLines[index].End, spikeTips[index]);
			}
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	[Fact]
	public void FlipperTransitionsProjectProductionDirectedPivotsForEveryLaneAndDirection()
	{
		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
			Level level = game.CurrentLevel;
			LevelConfiguration configuration = level.Configuration;
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			PropertyInfo directionProperty = typeof(Flipper).GetProperty(
				nameof(Flipper.Direction),
				BindingFlags.Instance | BindingFlags.Public)
				?? throw new InvalidOperationException("The Flipper direction property is missing.");
			PropertyInfo turnAngleProperty = typeof(Flipper).GetProperty(
				nameof(Flipper.TurnAngle),
				BindingFlags.Instance | BindingFlags.Public)
				?? throw new InvalidOperationException("The Flipper turn-angle property is missing.");
			MethodInfo beginPivot = typeof(Flipper).GetMethod(
				"BeginPivot", BindingFlags.Instance | BindingFlags.NonPublic)
				?? throw new InvalidOperationException("The Flipper pivot initializer is missing.");
			bool foundProductionReflexRoute = false;

			for (int lane = 0; lane < level.PlayableLaneCount; lane++)
			{
				foreach (int direction in new[] { -1, 1 })
				{
					int laneDelta = direction > 0 ? -1 : 1;
					Assert.True(level.TryGetAdjacentLane(lane, laneDelta,
						out int destination));
					Flipper ordinaryFlipper = new(lane, height: 190);
					directionProperty.SetValue(ordinaryFlipper, direction);
					beginPivot.Invoke(ordinaryFlipper,
						[level, game.Player.EdgeIndex, null]);
					Flipper cargoFlipper = Flipper.CreateTankerCargo(
						level,
						new TankerCargoSpawn(lane, destination, direction, 190),
						cargoUnitId: lane);

					foreach (Flipper flipper in new[] { ordinaryFlipper, cargoFlipper })
					{
						Assert.Equal(ReferenceEquals(flipper, cargoFlipper),
							flipper.IsTankerCargo);
						DirectedLaneTransition transition =
							flipper.ActiveTransition
							?? throw new InvalidOperationException(
								"The production Flipper pivot was not initialized.");
						if (transition.SweepMagnitudeDegrees > 180)
						{
							foundProductionReflexRoute = true;
							Assert.Equal(307.893f,
								transition.SweepMagnitudeDegrees, precision: 2);
						}
						Assert.Equal(direction, transition.Direction);

						foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
						{
							turnAngleProperty.SetValue(
								flipper, flipper.TotalTurnAngle * progress);
							var before = (
								flipper.State,
								flipper.Height,
								flipper.EdgeIndex,
								flipper.SourceEdge,
								flipper.DestinationEdge,
								flipper.Direction,
								flipper.TurnAngle,
								flipper.ActiveTransition);
							(Vector2 expectedStart, Vector2 expectedEnd) =
								level.CalculateFlipperPivotEndpoints(transition, progress);
							float depth = configuration.Position + configuration.Depth -
								flipper.Height;

							Assert.Equal(
								(Project(game, expectedStart, depth),
									Project(game, expectedEnd, depth)),
								renderer.ProjectFlipperEndpoints(flipper));
							Assert.Equal(before, (
								flipper.State,
								flipper.Height,
								flipper.EdgeIndex,
								flipper.SourceEdge,
								flipper.DestinationEdge,
								flipper.Direction,
								flipper.TurnAngle,
								flipper.ActiveTransition));
						}
					}
				}
			}

			Assert.True(foundProductionReflexRoute);
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	[Fact]
	public void AdapterJacobianIsSharedAcrossAxesAndRenderingDoesNotMutateGameplay()
	{
		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			TempestGame game = new(startingProductionLevel: 7, random: new Random(1));
			VectorRenderer renderer = CreateIdentityAtariRenderer(game);
			LevelDefinition definition = game.CurrentLevel.Definition;
			Vector2[] pointsBefore = definition.Points.ToArray();
			int playerLaneBefore = game.Player.EdgeIndex;
			float playerDepthBefore = game.Player.Depth;
			int spikeCountBefore = game.CurrentLevel.Spikes.Count;
			AtariPresentationProjection projection = ProjectResult(
				game, definition.Points[0], game.CurrentLevel.Configuration.Position);
			List<(Vector2 Start, Vector2 End, Color Color)> lines = [];

			renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
			renderer.DrawPlayer(definition.Points, (start, end, color) =>
				lines.Add((start, end, color)));
			renderer.DrawSpikes((start, end) => lines.Add((start, end, Color.Green)),
				_ => { });

			Assert.Equal(projection.LocalScale, projection.LocalScaleX);
			Assert.Equal(projection.LocalScale, projection.LocalScaleY);
			Assert.Equal(pointsBefore, definition.Points);
			Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
			Assert.Equal(playerDepthBefore, game.Player.Depth);
			Assert.Equal(spikeCountBefore, game.CurrentLevel.Spikes.Count);
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	[Fact]
	public void GenuineLevel7PlaybackHasEquivalentBoundariesWithPresentationOnAndOff()
	{
		string recordingPath = Path.GetFullPath(Path.Combine(
			AppContext.BaseDirectory, "..", "..", "..", "..", "..",
			"TEMP", "GameplayRecordings", "Level-7-slowmo.json"));
		if (!File.Exists(recordingPath))
		{
			output.WriteLine(
				"Optional local recording comparison skipped: TEMP\\GameplayRecordings\\Level-7-slowmo.json is not present.");
			return;
		}

		(float k, float zOffset, float perspective) = SetProjectionForTest();
		try
		{
			GameplayRecording recording =
				GameplayRecordingFile.LoadRecording(recordingPath);
			Assert.Equal(7, recording.StartupConfiguration.StartingDisplayedLevel);
			GameplayPlaybackSession presentationOff = new(
				recording.StartupConfiguration, recording.Replay);
			GameplayStateSummary initialOffBoundary =
				presentationOff.GetStateSummary();
			List<GameplayStateSummary> presentationOffBoundaries = [];
			while (presentationOff.StepForward())
				presentationOffBoundaries.Add(presentationOff.GetStateSummary());

			Projection.K = TestK;
			Projection.ZOffset = TestZOffset;
			Projection.PortraitPerspective = 0;
			GameplayPlaybackSession presentationOn = new(
				recording.StartupConfiguration, recording.Replay);
			VectorRenderer renderer = CreateIdentityAtariRenderer(presentationOn.Game);
			AssertEquivalentSummary(
				initialOffBoundary,
				presentationOn.GetStateSummary());
			int boundaryIndex = 0;
			while (presentationOn.StepForward())
			{
				GameplayStateSummary expected =
					presentationOffBoundaries[boundaryIndex++];
				AssertEquivalentSummary(expected, presentationOn.GetStateSummary());

				if (presentationOn.Game.CurrentLevel.Configuration.DisplayedLevel == 7 &&
					presentationOn.Game.State == LevelState.Playing)
				{
					RenderSharedLevel7Geometry(renderer, presentationOn.Game);
					AssertEquivalentSummary(expected,
						presentationOn.GetStateSummary());
				}
			}

			Assert.Equal(presentationOffBoundaries.Count, boundaryIndex);
			output.WriteLine(
				$"Compared all {recording.Replay.DurationTicks} boundaries from the unmodified local Level 7 recording.");
		}
		finally
		{
			RestoreProjection(k, zOffset, perspective);
		}
	}

	private static Vector2 Project(TempestGame game, Vector2 point, float depth) =>
		ProjectResult(game, point, depth).Position;

	private static void RenderSharedLevel7Geometry(
		VectorRenderer renderer, TempestGame game)
	{
		renderer.DrawTube(static (_, _, _) => { });
		renderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			static (_, _, _) => { });
		renderer.DrawPlayerShots(static (_, _) => { });
		renderer.DrawSpikes(static (_, _) => { }, static _ => { });
		renderer.DrawEggs(static (_, _) => { });
		renderer.DrawTankers(static (_, _) => { });
		renderer.DrawSpikers(static (_, _) => { });
		renderer.DrawFuseballs(static (_, _, _) => { });
		renderer.DrawPulsars(static (_, _, _) => { });
		renderer.DrawEnemyBullets(
			static (_, _, _) => { }, static (_, _) => { });
		renderer.DrawEnemyDestructionEffects(static (_, _, _) => { });
		renderer.DrawPulsingLaneBoundaries(static (_, _, _) => { });

		foreach (Flipper flipper in game.Flippers)
			_ = renderer.ProjectFlipperEndpoints(flipper);
		foreach (Tanker tanker in game.Tankers)
			_ = renderer.ProjectTankerCenter(tanker);
		foreach (Egg egg in game.Eggs)
			_ = renderer.ProjectEgg(egg);
		foreach (Shot shot in game.Shots)
			_ = renderer.ProjectShot(shot);
		foreach (EnemyBullet bullet in game.EnemyBullets)
			_ = renderer.ProjectEnemyBullet(bullet);
	}

	private static void AssertEquivalentSummary(
		GameplayStateSummary expected, GameplayStateSummary actual)
	{
		Assert.Equal(JsonSerializer.Serialize(expected),
			JsonSerializer.Serialize(actual));
	}

	private static AtariPresentationProjection ProjectResult(
		TempestGame game, Vector2 point, float depth)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		return AtariPresentationAdapter.Project(
			7,
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
			RomPresentationMetadataCatalog.ForDisplayedLevel(7));
	}

	private static float GetHudClearedLogicalCenterY()
	{
		Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
		float hudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
		float gameplayCenterY =
			(hudBottom + DisplayConfiguration.PortraitViewportHeight) / 2;
		return (gameplayCenterY - camera.Offset.Y) / camera.Zoom + camera.Target.Y;
	}

	private static (float K, float ZOffset, float Perspective) SetProjectionForTest()
	{
		(float k, float zOffset, float perspective) =
			(Projection.K, Projection.ZOffset, Projection.PortraitPerspective);
		Projection.K = TestK;
		Projection.ZOffset = TestZOffset;
		Projection.PortraitPerspective = 0;
		return (k, zOffset, perspective);
	}

	private static void RestoreProjection(float k, float zOffset, float perspective)
	{
		Projection.K = k;
		Projection.ZOffset = zOffset;
		Projection.PortraitPerspective = perspective;
	}

	private static VectorRenderer CreateIdentityAtariRenderer(TempestGame game) =>
		new(game, atariPresentationEnabled: true,
			tubeYGain: 1, tubeScale: 1, tubeVerticalOffset: 0);
}
