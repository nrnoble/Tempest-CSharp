using System.Numerics;
using Tempest.Game;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class BlasterScaleRegressionTests
{
	private const float BaselineK = 70;
	private const float BaselineZOffset = 5;

	[Fact]
	public void AtariExitScaleTracksTheStableBaselineAcrossBothZoomPhases()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = BaselineK;
			Projection.ZOffset = BaselineZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(random: new Random(1));
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			Assert.Equal(20, renderer.PlayerClawHeight);
			game.CurrentLevel.State = LevelState.ExitDelay;
			Assert.Equal(20, renderer.PlayerClawHeight);
			game.CurrentLevel.State = LevelState.ExitZoom;

			int[] sampleTicks = [0, 1, 23, 24, 60, 100, 123];
			float previousHeight = 20;
			for (int tick = 0; tick <= 123; tick++)
			{
				float height = renderer.PlayerClawHeight;
				Assert.True(float.IsFinite(height));
				Assert.InRange(MathF.Abs(height - previousHeight), 0, 1);
				previousHeight = height;
				if (sampleTicks.Contains(tick))
				{
					(float expectedHeight, float edgeLength) = ExpectedBlasterHeightAndEdgeLength(
						game, renderer);
					Assert.InRange(MathF.Abs(height - expectedHeight), 0, .001f);
					Assert.InRange(height / edgeLength, .18f, .23f);

					if (tick == 23)
					{
						float oldReversedHeight = 20 *
							(renderer.PlayerProjectedDepth + Projection.ZOffset) /
							(game.CurrentLevel.Configuration.Position + Projection.ZOffset);
						Assert.InRange(MathF.Abs(oldReversedHeight - 173.33333f), 0, .001f);
						Assert.InRange(height, 14.09f, 14.12f);
						Assert.Equal(70, Projection.K);
						Assert.Equal(-41, Projection.ZOffset);
					}
					else if (tick == 24)
					{
						Assert.Equal(72, Projection.K);
						Assert.Equal(-41, Projection.ZOffset);
					}
					else if (tick == 123)
					{
						float oldReversedHeight = 20 *
							(renderer.PlayerProjectedDepth + Projection.ZOffset) /
							(game.CurrentLevel.Configuration.Position + Projection.ZOffset);
						Assert.InRange(MathF.Abs(oldReversedHeight - 884.44446f), 0, .001f);
					}

					AssertPlayerRenderDoesNotMutateState(game, renderer);
				}

				if (tick != 123)
					game.RunOneStep();
			}

			Assert.Equal(LevelState.ExitZoom, game.State);
			Assert.InRange(game.Player.Depth, 10.99f, 11.01f);
			Assert.InRange(renderer.PlayerClawHeight, 17.36f, 17.39f);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void AtariApproachBlasterGrowsFromDistantScaleToTheFixedPlayingSize()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = BaselineK;
			Projection.ZOffset = BaselineZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = new(random: new Random(1));
			game.CurrentLevel.State = LevelState.ExitZoom;
			game.Player.AdvanceExit(game.Player.Depth - 12);
			game.RunOneStep();

			Assert.Equal(LevelState.Approach, game.State);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			float initialHeight = renderer.PlayerClawHeight;
			Assert.InRange(initialHeight, 1.07f, 1.11f);
			Assert.Equal(0, game.CurrentApproach!.ElapsedTicks);

			for (int tick = 0; tick < LevelApproach.DurationTicks / 2; tick++)
				game.RunOneStep();
			float middleHeight = renderer.PlayerClawHeight;
			Assert.True(middleHeight > initialHeight);
			Assert.Equal(96 * renderer.ApproachDepthOffset /
				LevelApproach.InitialDepthOffset, renderer.ApproachVerticalOffset);

			for (int tick = LevelApproach.DurationTicks / 2;
				tick < LevelApproach.DurationTicks; tick++)
				game.RunOneStep();

			Assert.Equal(LevelState.Playing, game.State);
			Assert.Null(game.CurrentApproach);
			Assert.Equal(20, renderer.PlayerClawHeight);
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
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	public void AtariExitUsesTheActiveAdapterScaleAgainstItsStableRimBaseline(int displayedLevel)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = BaselineK;
			Projection.ZOffset = BaselineZOffset;
			Projection.PortraitPerspective = 0;

			TempestGame game = CreateGame(displayedLevel);
			if (displayedLevel == 10)
				AdvanceDiagnosticLevelTenThroughAvoidSpikesGate(game);
			else
				game.CurrentLevel.State = LevelState.ExitZoom;

			game.Player.AdvanceExit(69);
			Projection.K = 70;
			Projection.ZOffset = -41;
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);

			(float expectedHeight, float edgeLength) = ExpectedBlasterHeightAndEdgeLength(
				game, renderer);
			Assert.InRange(MathF.Abs(renderer.PlayerClawHeight - expectedHeight), 0, .001f);
			Assert.True(float.IsFinite(renderer.PlayerClawHeight));
			Assert.True(renderer.PlayerClawHeight > 0);
			Assert.InRange(renderer.PlayerClawHeight / edgeLength, .1f, .5f);
			Assert.Equal(displayedLevel is 8 or 9 or 10,
				!game.CurrentLevel.Definition.Continuous);
			AssertPlayerRenderDoesNotMutateState(game, renderer);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LevelSevenExitUsesTheSharedStableBaseline()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = BaselineK;
			Projection.ZOffset = -41;
			Projection.PortraitPerspective = 0;
			TempestGame game = CreateGame(7);
			game.CurrentLevel.State = LevelState.ExitZoom;
			game.Player.AdvanceExit(69);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);

			float expected = ExpectedStableBaselineHeight(game, renderer);
			Assert.InRange(MathF.Abs(renderer.PlayerClawHeight - expected), 0, .001f);
			Assert.True(float.IsFinite(renderer.PlayerClawHeight));
			Assert.InRange(renderer.PlayerClawHeight, 0, 20);
			AssertPlayerRenderDoesNotMutateState(game, renderer);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void ReverseZoomBlasterScaleRemainsFiniteAndReturnsToThePlayingBaseline()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.PortraitPerspective = 0;
			TempestGame game = CreateGame(4);
			game.CurrentLevel.State = LevelState.ReverseZoom;
			game.Player.AdvanceExit(3);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);

			Projection.K = BaselineK;
			for (int step = 0; step <= 3; step++)
			{
				Projection.ZOffset = 3 + step;
				float height = renderer.PlayerClawHeight;
				Assert.True(float.IsFinite(height));
				Assert.InRange(height, 18, 20);
			}

			game.Player.ResetDepth();
			Projection.ZOffset = BaselineZOffset;
			Assert.Equal(20, renderer.PlayerClawHeight);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	private static void AdvanceDiagnosticLevelTenThroughAvoidSpikesGate(TempestGame game)
	{
		Assert.Equal(10, game.CurrentLevel.Configuration.DisplayedLevel);
		Assert.Equal(LevelExitPolicy.AvoidSpikes, game.CurrentLevel.Configuration.ExitPolicy);
		game.CurrentLevel.State = LevelState.AvoidSpikes;
		for (int tick = 0; tick <= game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold; tick++)
			game.RunOneStep();
		Assert.Equal(LevelState.ExitDelay, game.State);

		for (int tick = 0; tick <= 20; tick++)
			game.RunOneStep();
		Assert.Equal(LevelState.ExitZoom, game.State);
	}

	private static (float Height, float EdgeLength) ExpectedBlasterHeightAndEdgeLength(
		TempestGame game,
		VectorRenderer renderer)
	{
		float expectedHeight = ExpectedStableBaselineHeight(game, renderer);
		renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points, out Vector2 start, out Vector2 end);
		return (expectedHeight, Vector2.Distance(start, end));
	}

	private static float ExpectedStableBaselineHeight(TempestGame game, VectorRenderer renderer)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		float currentScale = GetAdapterLocalScale(
			game, configuration, renderer.PlayerProjectedDepth,
			renderer.ApproachDepthOffset, renderer.ApproachVerticalOffset,
			Projection.K, Projection.ZOffset);
		float baselineScale = GetAdapterLocalScale(
			game, configuration, configuration.Position, 0, 0,
			BaselineK, BaselineZOffset);
		return 20 * currentScale / baselineScale;
	}

	private static float GetAdapterLocalScale(
		TempestGame game,
		LevelConfiguration configuration,
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset,
		float k,
		float zOffset)
	{
		AtariPresentationProjection projection;
		if (configuration.DisplayedLevel == 5)
		{
			projection = AtariPresentationAdapter.ProjectLevel5(
				0, 0, depth,
				game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset,
				configuration.Position,
				configuration.Depth,
				approachDepthOffset,
				approachVerticalOffset,
				k,
				zOffset);
		}
		else if (configuration.DisplayedLevel == 6)
		{
			projection = AtariPresentationAdapter.ProjectLevel6(
				0, 0, depth,
				game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset,
				configuration.Position,
				configuration.Depth,
				approachDepthOffset,
				approachVerticalOffset,
				k,
				zOffset);
		}
		else
		{
			RomPresentationMetadata metadata =
				RomPresentationMetadataCatalog.ForDisplayedLevel(configuration.DisplayedLevel);
			projection = AtariPresentationAdapter.Project(
				configuration.DisplayedLevel,
				0, 0, depth,
				game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset,
				configuration.Position,
				configuration.Depth,
				approachDepthOffset,
				approachVerticalOffset,
				k,
				zOffset,
				metadata);
		}
		return projection.LocalScale;
	}

	private static void AssertPlayerRenderDoesNotMutateState(
		TempestGame game,
		VectorRenderer renderer)
	{
		LevelState state = game.State;
		int lane = game.Player.EdgeIndex;
		float edgePosition = game.Player.EdgePosition;
		float depth = game.Player.Depth;
		int score = game.Score.Score;
		int lives = game.Player.Lives;
		float k = Projection.K;
		float zOffset = Projection.ZOffset;
		renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points,
			out Vector2 start, out Vector2 end);
		Vector2 tangent = Vector2.Normalize(end - start);
		Vector2 normal = new(-tangent.Y, tangent.X);
		List<(Vector2 Start, Vector2 End, Raylib_cs.Color Color)> lines = [];
		renderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			(lineStart, lineEnd, color) => lines.Add((lineStart, lineEnd, color)));
		renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points,
			out Vector2 afterStart, out Vector2 afterEnd);

		Assert.Equal(6, lines.Count);
		Assert.Equal(start, afterStart);
		Assert.Equal(end, afterEnd);
		Assert.InRange(MathF.Abs(tangent.Length() - 1), 0, .001f);
		Assert.InRange(MathF.Abs(normal.Length() - 1), 0, .001f);
		Assert.All(lines, line => Assert.Equal(Raylib_cs.Color.Yellow, line.Color));
		Assert.Equal(state, game.State);
		Assert.Equal(lane, game.Player.EdgeIndex);
		Assert.Equal(edgePosition, game.Player.EdgePosition);
		Assert.Equal(depth, game.Player.Depth);
		Assert.Equal(score, game.Score.Score);
		Assert.Equal(lives, game.Player.Lives);
		Assert.Equal(k, Projection.K);
		Assert.Equal(zOffset, Projection.ZOffset);
	}

	private static TempestGame CreateGame(int displayedLevel) =>
		new(startingProductionLevel: displayedLevel, random: new Random(1));
}
