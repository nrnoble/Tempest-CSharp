using System.Numerics;
using Tempest.Game;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class ExitZoomFocalCompensationTests
{
	private const float ExitZoomK = 70;
	private const float ExitZoomZOffset = 5;

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
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	public void MappedOpeningStaysFixedThroughoutExitZoom(int displayedLevel)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;

			TempestGame game = new(
				startingProductionLevel: displayedLevel,
				startingLives: 5,
				random: new Random(1));
			float tubeYGain = displayedLevel is 1 or 7 ? 1.31f : 1f;
			VectorRenderer renderer = new(
				game, atariPresentationEnabled: true, tubeYGain: tubeYGain);
			TubeLine[] playingTube = CaptureTube(renderer);
			game.CurrentLevel.State = LevelState.ExitZoom;
			TubeLine[] onsetTube = CaptureTube(renderer);
			Vector2 openingAtOnset = GetOpening(game.CurrentLevel.Definition, onsetTube);
			AssertTubeCoordinatesEqual(playingTube, onsetTube);

			foreach ((float k, float zOffset) in new[]
			{
				(70f, 3f),
				(70f, -41f),
				(72f, -41f),
				(240f, -41f),
				(440f, -41f)
			})
			{
				Projection.K = k;
				Projection.ZOffset = zOffset;
				TubeLine[] tube = CaptureTube(renderer);
				Vector2 opening = GetOpening(game.CurrentLevel.Definition, tube);
				AssertVectorNear(openingAtOnset, opening);
				if (!game.CurrentLevel.Definition.Continuous)
				{
					float expansion = (k / (game.CurrentLevel.Configuration.Position + zOffset)) /
						(ExitZoomK / (game.CurrentLevel.Configuration.Position + ExitZoomZOffset));
					AssertTubeExpandedAroundPivot(
						onsetTube, tube, openingAtOnset, expansion);
				}
				Assert.All(tube, line =>
				{
					Assert.True(float.IsFinite(line.Start.X));
					Assert.True(float.IsFinite(line.Start.Y));
					Assert.True(float.IsFinite(line.End.X));
					Assert.True(float.IsFinite(line.End.Y));
				});
			}

			Assert.Equal(440, Projection.K);
			Assert.Equal(-41, Projection.ZOffset);

			LevelState stateBeforeRender = game.State;
			float playerDepthBeforeRender = game.Player.Depth;
			int playerLaneBeforeRender = game.Player.EdgeIndex;
			float playerPositionBeforeRender = game.Player.EdgePosition;
			float kBeforeRender = Projection.K;
			float zOffsetBeforeRender = Projection.ZOffset;
			_ = CaptureTube(renderer);
			Assert.Equal(stateBeforeRender, game.State);
			Assert.Equal(playerDepthBeforeRender, game.Player.Depth);
			Assert.Equal(playerLaneBeforeRender, game.Player.EdgeIndex);
			Assert.Equal(playerPositionBeforeRender, game.Player.EdgePosition);
			Assert.Equal(kBeforeRender, Projection.K);
			Assert.Equal(zOffsetBeforeRender, Projection.ZOffset);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Theory]
	[InlineData(8, 369.100f)]
	[InlineData(9, 292.332f)]
	[InlineData(10, 538.636f)]
	public void OpenMappedRailsConvergeAtExpectedOnset(int displayedLevel, float expectedY)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;

			TempestGame game = new(
				startingProductionLevel: displayedLevel,
				random: new Random(1));
			LevelDefinition definition = game.CurrentLevel.Definition;
			Assert.False(definition.Continuous);
			Assert.Equal(16, definition.Points.Count);

			TubeLine[] tube = CaptureTube(
				new VectorRenderer(game, atariPresentationEnabled: true,
					tubeYGain: 1, tubeScale: 1, tubeVerticalOffset: 0));
			RailFit fit = FitRails(tube, definition.Points.Count);

			Assert.Equal(definition.Points.Count, fit.ValidRailCount);
			Assert.InRange(fit.Convergence.X, 399.99f, 400.01f);
			Assert.InRange(fit.Convergence.Y, expectedY - 0.01f, expectedY + 0.01f);
			Assert.True(fit.MaximumResidual <= fit.Tolerance);

			Vector2 terminalMidpoint =
				(tube[0].End + tube[definition.Points.Count - 1].End) / 2;
			Assert.True(Vector2.Distance(terminalMidpoint, fit.Convergence) > 1);
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
	[InlineData(5)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	public void OpeningSelectionUsesMappedFarGeometryAndTubeTopology(int displayedLevel)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;

			TempestGame game = new(
				startingProductionLevel: displayedLevel,
				random: new Random(1));
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			LevelDefinition definition = game.CurrentLevel.Definition;
			TubeLine[] tube = CaptureTube(renderer);
			Vector2[] mappedFarVertices = GetMappedFarVertices(definition, tube);
			Vector2 displayedOpening = GetOpening(definition, tube);
			Vector2 sourceOpening = definition.Continuous
				? Average(definition.Points)
				: (definition.Points[0] + definition.Points[^1]) / 2;

			Assert.NotEqual(sourceOpening, displayedOpening);
			Assert.Equal(
				definition.Continuous
					? Average(mappedFarVertices)
					: FitRails(tube, definition.Points.Count).Convergence,
				displayedOpening);

			if (!definition.Continuous)
			{
				Assert.NotEqual(
					(mappedFarVertices[0] + mappedFarVertices[^1]) / 2,
					displayedOpening);
				Assert.Equal(definition.Points.Count, FitRails(
					tube, definition.Points.Count).ValidRailCount);
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
	[InlineData(7)]
	[InlineData(8)]
	public void TubeBlasterAndSpikesShareCompensationWithoutChangingBlasterScale(
		int displayedLevel)
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;

			TempestGame game = new(
				startingProductionLevel: displayedLevel, random: new Random(1));
			game.CurrentLevel.State = LevelState.ExitZoom;
			VectorRenderer latchedRenderer = new(game, atariPresentationEnabled: true);
			Vector2 openingAtOnset = GetOpening(
				game.CurrentLevel.Definition,
				CaptureTube(latchedRenderer));

			Projection.K = 240;
			Projection.ZOffset = -41;
			game.Player.AdvanceExit(229);
			TubeLine[] uncompensatedTube = CaptureTube(
				new VectorRenderer(game, atariPresentationEnabled: true));
			Vector2 currentUncompensatedOpening =
				GetOpening(game.CurrentLevel.Definition, uncompensatedTube);
			Vector2 compensation = openingAtOnset - currentUncompensatedOpening;
			TubeLine[] compensatedTube = CaptureTube(latchedRenderer);
			AssertTubeTranslated(uncompensatedTube, compensatedTube, compensation);

			VectorRenderer uncompensatedRenderer = new(game, atariPresentationEnabled: true);
			Vector2 uncompensatedPlayerSize = new(uncompensatedRenderer.PlayerClawHeight);
			List<Line> uncompensatedBlaster =
				CaptureBlaster(game.CurrentLevel.Definition.Points, uncompensatedRenderer);
			float compensatedPlayerSize = latchedRenderer.PlayerClawHeight;
			List<Line> compensatedBlaster =
				CaptureBlaster(game.CurrentLevel.Definition.Points, latchedRenderer);
			AssertVectorNear(uncompensatedPlayerSize, new Vector2(compensatedPlayerSize));
			AssertLinesTranslated(uncompensatedBlaster, compensatedBlaster, compensation);

			List<Line> uncompensatedSpikes = CaptureSpikes(
				uncompensatedRenderer, out Vector2[] rawTips);
			List<Line> compensatedSpikes = CaptureSpikes(
				latchedRenderer, out Vector2[] compensatedTips);
			Assert.NotEmpty(uncompensatedSpikes);
			AssertLinesTranslated(uncompensatedSpikes, compensatedSpikes, compensation);
			Assert.Equal(rawTips.Length, compensatedTips.Length);
			for (int index = 0; index < rawTips.Length; index++)
				AssertVectorNear(rawTips[index] + compensation, compensatedTips[index]);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	[Fact]
	public void LatchBeginsAtExitTravelAndResetsForRetryAndSuccessfulHandoff()
	{
		float originalK = Projection.K;
		float originalZOffset = Projection.ZOffset;
		float originalPerspective = Projection.PortraitPerspective;
		try
		{
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;

			foreach (int displayedLevel in new[] { 4, 10 })
			{
				TempestGame gateGame = new(
					startingProductionLevel: displayedLevel,
					random: new Random(1));
				Assert.Equal(
					LevelExitPolicy.AvoidSpikes,
					gateGame.CurrentLevel.Configuration.ExitPolicy);
				VectorRenderer gateRenderer = new(gateGame, atariPresentationEnabled: true);
				gateGame.CurrentLevel.State = LevelState.AvoidSpikes;
				for (int tick = 0;
					tick <= gateGame.CurrentLevel.Configuration.AvoidSpikesWaitThreshold;
					tick++)
				{
					gateGame.RunOneStep();
					Assert.Equal(
						tick < gateGame.CurrentLevel.Configuration.AvoidSpikesWaitThreshold
							? LevelState.AvoidSpikes
							: LevelState.ExitDelay,
						gateGame.State);
					_ = CaptureTube(gateRenderer);
				}

				for (int tick = 0; tick < 20; tick++)
				{
					gateGame.RunOneStep();
					Assert.Equal(LevelState.ExitDelay, gateGame.State);
					_ = CaptureTube(gateRenderer);
				}
				Projection.K = 90;
				gateGame.RunOneStep();
				Assert.Equal(LevelState.ExitZoom, gateGame.State);
				TubeLine[] gateOnset = CaptureTube(gateRenderer);
				TubeLine[] gateOnsetUncompensated = CaptureTube(
					new VectorRenderer(gateGame, atariPresentationEnabled: true));
				AssertTubeCoordinatesEqual(gateOnsetUncompensated, gateOnset);
			}

			TempestGame retryGame = new(
				startingProductionLevel: 5,
				random: new Random(1));
			VectorRenderer retryRenderer = new(retryGame, atariPresentationEnabled: true);
			retryGame.CurrentLevel.State = LevelState.AvoidSpikes;
			Projection.K = 90;
			_ = CaptureTube(retryRenderer);
			retryGame.CurrentLevel.State = LevelState.ExitDelay;
			Projection.K = 110;
			_ = CaptureTube(retryRenderer);
			retryGame.CurrentLevel.State = LevelState.ExitZoom;
			Projection.K = 240;
			Projection.ZOffset = -41;
			TubeLine[] retryOnset = CaptureTube(retryRenderer);
			TubeLine[] retryOnsetUncompensated = CaptureTube(
				new VectorRenderer(retryGame, atariPresentationEnabled: true));
			AssertTubeCoordinatesEqual(retryOnsetUncompensated, retryOnset);

			retryGame.CurrentLevel.State = LevelState.ReverseZoom;
			TubeLine[] reverseZoom = CaptureTube(retryRenderer);
			TubeLine[] reverseZoomUncompensated = CaptureTube(
				new VectorRenderer(retryGame, atariPresentationEnabled: true));
			AssertTubeCoordinatesEqual(reverseZoomUncompensated, reverseZoom);

			retryGame.CurrentLevel.State = LevelState.ExitDelay;
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			_ = CaptureTube(retryRenderer);
			retryGame.CurrentLevel.State = LevelState.ExitZoom;
			Projection.K = 100;
			TubeLine[] secondAttemptOnset = CaptureTube(retryRenderer);
			TubeLine[] secondAttemptRaw = CaptureTube(
				new VectorRenderer(retryGame, atariPresentationEnabled: true));
			AssertTubeCoordinatesEqual(secondAttemptRaw, secondAttemptOnset);
			Vector2 secondAttemptOpening =
				GetOpening(retryGame.CurrentLevel.Definition, secondAttemptOnset);

			Projection.K = 240;
			Projection.ZOffset = -41;
			TubeLine[] secondAttemptLate = CaptureTube(retryRenderer);
			AssertVectorNear(
				secondAttemptOpening,
				GetOpening(retryGame.CurrentLevel.Definition, secondAttemptLate));

			TempestGame handoffGame = new(startingProductionLevel: 1, random: new Random(1));
			VectorRenderer handoffRenderer = new(handoffGame, atariPresentationEnabled: true);
			handoffGame.CurrentLevel.State = LevelState.ExitZoom;
			Projection.K = ExitZoomK;
			Projection.ZOffset = ExitZoomZOffset;
			_ = CaptureTube(handoffRenderer);
			Projection.K = 440;
			Projection.ZOffset = -41;
			handoffGame.Player.AdvanceExit(389);
			_ = CaptureTube(handoffRenderer);

			handoffGame.RunOneStep();
			Assert.Equal(LevelState.Approach, handoffGame.State);
			TubeLine[] handedOffTube = CaptureTube(handoffRenderer);
			TubeLine[] freshRendererTube = CaptureTube(
				new VectorRenderer(handoffGame, atariPresentationEnabled: true));
			AssertTubeCoordinatesEqual(freshRendererTube, handedOffTube);
		}
		finally
		{
			Projection.K = originalK;
			Projection.ZOffset = originalZOffset;
			Projection.PortraitPerspective = originalPerspective;
		}
	}

	private static TubeLine[] CaptureTube(VectorRenderer renderer)
	{
		List<TubeLine> lines = [];
		renderer.DrawTube((start, end, _) => lines.Add(new TubeLine(start, end)));
		return lines.ToArray();
	}

	private static List<Line> CaptureBlaster(
		IReadOnlyList<Vector2> points,
		VectorRenderer renderer)
	{
		List<Line> lines = [];
		renderer.DrawPlayer(points,
			(start, end, _) => lines.Add(new Line(start, end)));
		return lines;
	}

	private static List<Line> CaptureSpikes(VectorRenderer renderer, out Vector2[] tips)
	{
		List<Line> lines = [];
		List<Vector2> capturedTips = [];
		renderer.DrawSpikes(
			(start, end) => lines.Add(new Line(start, end)),
			capturedTips.Add);
		tips = capturedTips.ToArray();
		return lines;
	}

	private static Vector2[] GetMappedFarVertices(
		LevelDefinition definition,
		IReadOnlyList<TubeLine> tube) =>
		tube.Take(definition.Points.Count).Select(line => line.End).ToArray();

	private static Vector2 GetOpening(
		LevelDefinition definition,
		IReadOnlyList<TubeLine> tube)
	{
		Vector2[] farVertices = GetMappedFarVertices(definition, tube);
		Assert.Equal(definition.Points.Count, farVertices.Length);
		return definition.Continuous
			? Average(farVertices)
			: FitRails(tube, definition.Points.Count).Convergence;
	}

	private static RailFit FitRails(IReadOnlyList<TubeLine> tube, int railCount)
	{
		Assert.True(railCount >= 2);
		Assert.True(tube.Count >= railCount);

		double coordinateScale = 1;
		for (int index = 0; index < railCount; index++)
		{
			coordinateScale = Math.Max(coordinateScale,
				Math.Max(Math.Max(Math.Abs(tube[index].Start.X),
					Math.Abs(tube[index].Start.Y)),
					Math.Max(Math.Abs(tube[index].End.X),
						Math.Abs(tube[index].End.Y))));
		}

		double tolerance = Math.Max(1e-5, coordinateScale * 32 * 1.1920928955078125e-7);
		double a00 = 0;
		double a01 = 0;
		double a11 = 0;
		double b0 = 0;
		double b1 = 0;
		List<(double NormalX, double NormalY, double SignedDistance)> rails = [];
		for (int index = 0; index < railCount; index++)
		{
			double dx = (double)tube[index].End.X - tube[index].Start.X;
			double dy = (double)tube[index].End.Y - tube[index].Start.Y;
			double length = Math.Sqrt(dx * dx + dy * dy);
			Assert.True(length > tolerance);
			double normalX = -dy / length;
			double normalY = dx / length;
			double signedDistance = normalX * tube[index].Start.X +
				normalY * tube[index].Start.Y;
			rails.Add((normalX, normalY, signedDistance));
			a00 += normalX * normalX;
			a01 += normalX * normalY;
			a11 += normalY * normalY;
			b0 += normalX * signedDistance;
			b1 += normalY * signedDistance;
		}

		double determinant = a00 * a11 - a01 * a01;
		Assert.True(determinant > (a00 + a11) * (a00 + a11) * 1e-12);
		double x = (b0 * a11 - b1 * a01) / determinant;
		double y = (a00 * b1 - a01 * b0) / determinant;
		double maximumResidual = rails.Max(rail => Math.Abs(
			rail.NormalX * x + rail.NormalY * y - rail.SignedDistance));

		return new RailFit(
			new Vector2((float)x, (float)y),
			maximumResidual,
			tolerance,
			rails.Count);
	}

	private static void AssertTubeExpandedAroundPivot(
		IReadOnlyList<TubeLine> onset,
		IReadOnlyList<TubeLine> current,
		Vector2 pivot,
		float scale)
	{
		Assert.Equal(onset.Count, current.Count);
		for (int index = 0; index < onset.Count; index++)
		{
			AssertVectorNear(
				pivot + scale * (onset[index].Start - pivot),
				current[index].Start,
				0.05f);
			AssertVectorNear(
				pivot + scale * (onset[index].End - pivot),
				current[index].End,
				0.05f);
		}
	}

	private static Vector2 Average(IReadOnlyList<Vector2> points)
	{
		Assert.NotEmpty(points);
		Vector2 sum = Vector2.Zero;
		foreach (Vector2 point in points)
			sum += point;
		return sum / points.Count;
	}

	private static void AssertTubeTranslated(
		IReadOnlyList<TubeLine> expected,
		IReadOnlyList<TubeLine> actual,
		Vector2 translation)
	{
		Assert.Equal(expected.Count, actual.Count);
		for (int index = 0; index < expected.Count; index++)
		{
			AssertVectorNear(expected[index].Start + translation, actual[index].Start);
			AssertVectorNear(expected[index].End + translation, actual[index].End);
		}
	}

	private static void AssertTubeCoordinatesEqual(
		IReadOnlyList<TubeLine> expected,
		IReadOnlyList<TubeLine> actual)
	{
		Assert.Equal(expected.Count, actual.Count);
		for (int index = 0; index < expected.Count; index++)
		{
			AssertVectorNear(expected[index].Start, actual[index].Start);
			AssertVectorNear(expected[index].End, actual[index].End);
		}
	}

	private static void AssertLinesTranslated(
		IReadOnlyList<Line> expected,
		IReadOnlyList<Line> actual,
		Vector2 translation)
	{
		Assert.Equal(expected.Count, actual.Count);
		for (int index = 0; index < expected.Count; index++)
		{
			AssertVectorNear(expected[index].Start + translation, actual[index].Start);
			AssertVectorNear(expected[index].End + translation, actual[index].End);
		}
	}

	private static void AssertVectorNear(Vector2 expected, Vector2 actual) =>
		AssertVectorNear(expected, actual, 0.001f);

	private static void AssertVectorNear(
		Vector2 expected,
		Vector2 actual,
		float tolerance) =>
		Assert.InRange(Vector2.Distance(expected, actual), 0, tolerance);

	private readonly record struct RailFit(
		Vector2 Convergence,
		double MaximumResidual,
		double Tolerance,
		int ValidRailCount);
	private readonly record struct TubeLine(Vector2 Start, Vector2 End);
	private readonly record struct Line(Vector2 Start, Vector2 End);
}
