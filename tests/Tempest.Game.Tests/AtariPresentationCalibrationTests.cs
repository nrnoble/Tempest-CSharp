using System.Numerics;
using Raylib_cs;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class AtariPresentationCalibrationTests
{
	[Theory]
	[InlineData(1, 1.4f, .8f, 35f, 1f)]
	[InlineData(2, 1f, 1.1f, 35f, 1f)]
	[InlineData(3, 1.2f, .97f, 35f, 1f)]
	[InlineData(4, 1f, 1f, 35f, 1f)]
	[InlineData(5, 1f, 1.2f, 15f, 1f)]
	[InlineData(6, 1f, 1.28f, 15f, 1f)]
	[InlineData(7, 1.3f, .9f, -25f, 1f)]
	[InlineData(8, 1f, 1f, -75f, 1f)]
	[InlineData(9, 1.5f, 1.05f, -150f, 1f)]
	[InlineData(10, 1.2f, .9f, 15f, 1f)]
	[InlineData(11, 1.5f, 1.1f, 0f, 1f)]
	[InlineData(12, 1.3f, .8f, 0f, 1f)]
	[InlineData(13, 1.2f, 1f, 0f, .3f)]
	[InlineData(14, 1f, 1.1f, 200f, 1f)]
	[InlineData(15, 1f, 1f, 0f, 1f)]
	public void SelectedDefaultsMatchTheFirstPassTable(
		int displayedLevel, float yGain, float scale, float verticalOffset,
		float depthYGain)
	{
		Assert.Equal(new AtariPresentationCalibration(yGain, scale, verticalOffset, depthYGain),
			AtariPresentationCalibration.ForDisplayedLevel(displayedLevel));
		Assert.Equal(new AtariPresentationCalibration(yGain, scale, verticalOffset, depthYGain),
			AtariPresentationCalibration.Resolve(displayedLevel));

		TempestGame game = new(startingProductionLevel: displayedLevel);
		Assert.Equal(
			CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true)),
			CaptureTube(new VectorRenderer(
				game, atariPresentationEnabled: true,
				tubeYGain: yGain, tubeScale: scale, tubeVerticalOffset: verticalOffset)));
	}

	[Fact]
	public void LevelThirteenDepthGainIsAcceptedAndIndependentOfOtherOverrides()
	{
		AtariPresentationCalibration selected =
			AtariPresentationCalibration.ForDisplayedLevel(13);
		Assert.Equal(new AtariPresentationCalibration(1.2f, 1f, 0f, .3f), selected);
		Assert.Equal(new AtariPresentationCalibration(.8f, 1.3f, 12f, .6f),
			AtariPresentationCalibration.Resolve(
				13, tubeYGainOverride: .8f, tubeScaleOverride: 1.3f,
				verticalOffsetOverride: 12f, tubeDepthYGainOverride: .6f));

		TempestGame game = new(startingProductionLevel: 13);
		List<(Vector2 Start, Vector2 End, Color Color)> defaultTube =
			CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));
		Assert.Equal(defaultTube, CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true,
			tubeYGain: selected.TubeYGain, tubeScale: selected.TubeScale,
			tubeVerticalOffset: selected.VerticalOffset,
			tubeDepthYGain: selected.TubeDepthYGain)));
		Assert.NotEqual(defaultTube, CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeDepthYGain: 1f)));
	}

	[Fact]
	public void ExplicitDimensionsReplaceOnlyTheirPerLevelDefaults()
	{
		const int displayedLevel = 7;
		AtariPresentationCalibration level7 =
			AtariPresentationCalibration.ForDisplayedLevel(displayedLevel);
		Assert.Equal(new AtariPresentationCalibration(1.3f, 1.1f, -25),
			AtariPresentationCalibration.Resolve(displayedLevel, tubeScaleOverride: 1.1f));
		Assert.Equal(new AtariPresentationCalibration(1f, .9f, -25),
			AtariPresentationCalibration.Resolve(displayedLevel, tubeYGainOverride: 1f));
		Assert.Equal(new AtariPresentationCalibration(1.3f, .9f, 0),
			AtariPresentationCalibration.Resolve(displayedLevel, verticalOffsetOverride: 0));
		Assert.Equal(new AtariPresentationCalibration(1f, 1f, 0),
			AtariPresentationCalibration.Resolve(
				displayedLevel, tubeYGainOverride: 1, tubeScaleOverride: 1,
				verticalOffsetOverride: 0));

		TempestGame game = new(startingProductionLevel: displayedLevel);
		List<(Vector2 Start, Vector2 End, Color Color)> expected = CaptureTube(
			new VectorRenderer(game, atariPresentationEnabled: true,
				tubeYGain: level7.TubeYGain, tubeScale: 1.1f,
				tubeVerticalOffset: level7.VerticalOffset));
		Assert.Equal(expected, CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeScale: 1.1f)));

		expected = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true,
			tubeYGain: 1, tubeScale: level7.TubeScale,
			tubeVerticalOffset: level7.VerticalOffset));
		Assert.Equal(expected, CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeYGain: 1)));

		expected = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true,
			tubeYGain: level7.TubeYGain, tubeScale: level7.TubeScale,
			tubeVerticalOffset: 0));
		Assert.Equal(expected, CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeVerticalOffset: 0)));
	}

	[Fact]
	public void LevelFifteenHasAnUncalibratedDefaultAndRetainsTubeOverrides()
	{
		Assert.Equal(new AtariPresentationCalibration(1f, 1f, 0f),
			AtariPresentationCalibration.ForDisplayedLevel(15));
		Assert.Equal(new AtariPresentationCalibration(1.1f, .9f, 12f),
			AtariPresentationCalibration.Resolve(
				15, tubeYGainOverride: 1.1f, tubeScaleOverride: .9f,
				verticalOffsetOverride: 12f));

		TempestGame game = new(startingProductionLevel: 15);
		Assert.NotEmpty(CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true,
			tubeYGain: 1.1f, tubeScale: .9f, tubeVerticalOffset: 12f)));
	}

	[Fact]
	public void ExplicitIdentityOverridesMatchTheUncalibratedAtariProjector()
	{
		TempestGame game = new(startingProductionLevel: 7);
		VectorRenderer renderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: 1, tubeScale: 1, tubeVerticalOffset: 0);
		List<(Vector2 Start, Vector2 End, Color Color)> actual = CaptureTube(renderer);
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(7);
		List<(Vector2 Start, Vector2 End)> expected = [];
		foreach (Vector2 point in game.CurrentLevel.Definition.Points)
		{
			AtariPresentationProjection near = AtariPresentationAdapter.Project(
				7, point.X, point.Y, configuration.Position,
				game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset,
				configuration.Position, configuration.Depth, 0, 0,
				Projection.K, Projection.ZOffset, metadata, tubeYGain: 1);
			AtariPresentationProjection far = AtariPresentationAdapter.Project(
				7, point.X, point.Y, configuration.Position + configuration.Depth,
				game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset,
				configuration.Position, configuration.Depth, 0, 0,
				Projection.K, Projection.ZOffset, metadata, tubeYGain: 1);
			expected.Add((near.Position, far.Position));
		}

		Assert.Equal(expected, actual.Take(game.CurrentLevel.Definition.Points.Count)
			.Select(line => (line.Start, line.End)));
	}

	[Fact]
	public void DefaultCalibrationRegistersTubeAndAttachedPresentationWithoutMutatingModel()
	{
		TempestGame game = new(startingProductionLevel: 7, random: new Random(1981));
		Spike spike = game.CurrentLevel.CreateSpike(2, 80);
		AtariPresentationCalibration selected =
			AtariPresentationCalibration.ForDisplayedLevel(7);
		VectorRenderer calibrated = new(game, atariPresentationEnabled: true);
		VectorRenderer explicitCalibration = new(
			game, atariPresentationEnabled: true,
			tubeYGain: selected.TubeYGain, tubeScale: selected.TubeScale,
			tubeVerticalOffset: selected.VerticalOffset);
		Vector2[] pointsBefore = game.CurrentLevel.Definition.Points.ToArray();
		int playerLaneBefore = game.Player.EdgeIndex;
		float playerDepthBefore = game.Player.Depth;
		int scoreBefore = game.Score.Score;
		LevelState stateBefore = game.State;

		Assert.Equal(CaptureTube(explicitCalibration), CaptureTube(calibrated));
		Assert.Equal(calibrated.ProjectShot(new Shot { EdgeIndex = 3, Height = 100 }),
			explicitCalibration.ProjectShot(new Shot { EdgeIndex = 3, Height = 100 }));
		Assert.Equal(calibrated.ProjectEnemyBullet(new EnemyBullet(4, 100)),
			explicitCalibration.ProjectEnemyBullet(new EnemyBullet(4, 100)));
		Assert.Equal(calibrated.ProjectFlipperEndpoints(new Flipper(5, 120)),
			explicitCalibration.ProjectFlipperEndpoints(new Flipper(5, 120)));
		Assert.Equal(CaptureSpikes(calibrated), CaptureSpikes(explicitCalibration));

		Assert.Contains(spike, game.Spikes);
		Assert.Equal(pointsBefore, game.CurrentLevel.Definition.Points);
		Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
		Assert.Equal(playerDepthBefore, game.Player.Depth);
		Assert.Equal(scoreBefore, game.Score.Score);
		Assert.Equal(stateBefore, game.State);
	}

	[Fact]
	public void AtariCalibrationFollowsTheCurrentLevelDuringProgression()
	{
		TempestGame game = new(startingProductionLevel: 6, random: new Random(1981));
		foreach (Spike spike in game.CurrentLevel.Spikes.ToArray())
			game.CurrentLevel.RemoveSpike(spike);
		VectorRenderer renderer = new(game, atariPresentationEnabled: true);
		List<(Vector2 Start, Vector2 End, Color Color)> level6 = CaptureTube(renderer);

		AdvanceToLevel7(game);

		Assert.Equal(7, game.CurrentLevel.Configuration.DisplayedLevel);
		Assert.NotEqual(level6, CaptureTube(renderer));
		Assert.Equal(CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true,
			tubeYGain: 1.3f, tubeScale: .9f, tubeVerticalOffset: -25)),
			CaptureTube(renderer));
	}

	[Fact]
	public void VisiblePlaybackCalibrationFollowsTheDisplayedLevelAcrossABoundary()
	{
		GameplayStartupConfiguration startup = new(6, 3, 1.0, 1981);
		PlayerInputReplay replay = new(
			1981, 1, [], LevelConfiguration.Level6Production);
		GameplayPlaybackSession session = new(startup, replay);
		foreach (Spike spike in session.Game.CurrentLevel.Spikes.ToArray())
			session.Game.CurrentLevel.RemoveSpike(spike);
		session.Game.CurrentLevel.State = LevelState.ExitZoom;
		session.Game.Player.AdvanceExit(session.Game.CurrentLevel.Configuration.Depth - 12);
		VisiblePlaybackController controller = new(session);
		VectorRenderer renderer = new(session.Game, atariPresentationEnabled: true);

		Assert.Equal(1, controller.ProcessControls(default));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(7, controller.Session.Game.CurrentLevel.Configuration.DisplayedLevel);
		Assert.Equal(CaptureTube(new VectorRenderer(
			controller.Session.Game, atariPresentationEnabled: true,
			tubeYGain: 1.3f, tubeScale: .9f, tubeVerticalOffset: -25)),
			CaptureTube(renderer));
	}

	[Fact]
	public void LegacyPresentationIgnoresTheAtariCalibrationTable()
	{
		TempestGame game = new(startingProductionLevel: 7);
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		List<(Vector2 Start, Vector2 End, Color Color)> actual =
			CaptureTube(new VectorRenderer(game));
		List<(Vector2 Start, Vector2 End)> expected = [];
		foreach (Vector2 point in game.CurrentLevel.Definition.Points)
		{
			Vector2 near = Projection.Project(
				point.X, point.Y, configuration.Position,
				game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset,
				configuration.Position, configuration.Position + configuration.Depth);
			Vector2 far = Projection.Project(
				point.X, point.Y, configuration.Position + configuration.Depth,
				game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset,
				configuration.Position, configuration.Position + configuration.Depth);
			expected.Add((near, far));
		}

		Assert.Equal(expected, actual.Take(game.CurrentLevel.Definition.Points.Count)
			.Select(line => (line.Start, line.End)));
	}

	private static void AdvanceToLevel7(TempestGame game)
	{
		game.CurrentLevel.State = LevelState.ExitZoom;
		game.Player.AdvanceExit(389);
		game.RunOneStep();
	}

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		VectorRenderer renderer)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}

	private static List<(Vector2 Start, Vector2 End)> CaptureSpikes(VectorRenderer renderer)
	{
		List<(Vector2 Start, Vector2 End)> lines = [];
		renderer.DrawSpikes((start, end) => lines.Add((start, end)), _ => { });
		return lines;
	}
}
