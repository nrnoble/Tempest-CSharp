using System.Globalization;
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
public sealed class TubeDepthYGainTests : IDisposable
{
	private readonly float originalK = Projection.K;
	private readonly float originalZOffset = Projection.ZOffset;
	private readonly float originalPerspective = Projection.PortraitPerspective;

	public TubeDepthYGainTests()
	{
		Projection.K = 70;
		Projection.ZOffset = 5;
		Projection.PortraitPerspective = 0;
	}

	public void Dispose()
	{
		Projection.K = originalK;
		Projection.ZOffset = originalZOffset;
		Projection.PortraitPerspective = originalPerspective;
	}

	[Fact]
	public void OmittedGainIsNeutral()
	{
		Assert.True(DebugController.TryParseTubeDepthYGainOptions([], out var options, out var error));
		Assert.Null(error);
		Assert.Equal(1, options.Gain);
		Assert.False(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("1", 1f)]
	[InlineData("0", 0f)]
	[InlineData(".75", .75f)]
	[InlineData("-2.5", -2.5f)]
	[InlineData("1e30", 1e30f)]
	[InlineData("-3.4028235e38", float.MinValue)]
	[InlineData("3.4028235e38", float.MaxValue)]
	public void ParsesAllFiniteSignedValues(string value, float expected)
	{
		Assert.True(DebugController.TryParseTubeDepthYGainOptions(
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain", value],
			out var options, out var error));
		Assert.Null(error);
		Assert.True(options.WasExplicitlySpecified);
		Assert.Equal(expected, options.Gain);
	}

	[Theory]
	[InlineData("")]
	[InlineData("1,5")]
	[InlineData("invalid")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("1e1000")]
	public void RejectsInvalidValues(string value)
	{
		Assert.False(DebugController.TryParseTubeDepthYGainOptions(
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain", value],
			out _, out var error));
		Assert.Contains("finite signed", error);
	}

	[Fact]
	public void RejectsMissingDuplicateJoinedAndIncompatibleArguments()
	{
		string[][] invalid =
		[
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain"],
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain", "--lives", "3"],
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain", "1", "--tube-depth-y-gain", "0"],
			["--atari-presentation", "--level", "13", "--tube-depth-y-gain=1"],
			["--level", "13", "--tube-depth-y-gain", "0"],
			["--atari-presentation", "--tube-depth-y-gain", "0"]
		];
		foreach (string[] args in invalid)
			Assert.False(DebugController.TryParseTubeDepthYGainOptions(args, out _, out _));
		Assert.False(DebugController.TryParseTubeDepthYGainOptions(
			["--level", "13", "--tube-depth-y-gain", "0"], out _, out var error));
		Assert.Contains("--atari-presentation", error);
	}

	[Fact]
	public void ParsesIndependentlyForEverySupportedLevelAndPlayback()
	{
		foreach (int level in Enumerable.Range(1, 13).Append(15))
		{
			string[] args = ["--atari-presentation", "--level",
				level.ToString(CultureInfo.InvariantCulture), "--tube-depth-y-gain", "-0.5",
				"--tube-y-gain", "1.2", "--tube-scale", ".8", "--tube-vertical-offset", "7"];
			Assert.True(DebugController.TryParseTubeDepthYGainOptions(args, out var depth, out _));
			Assert.True(DebugController.TryParseTubeYGainOptions(args, out var y, out _));
			Assert.True(DebugController.TryParseTubeScaleOptions(args, out var scale, out _));
			Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(args, out var offset, out _));
			Assert.Equal(-.5f, depth.Gain);
			Assert.Equal(1.2f, y.Gain);
			Assert.Equal(.8f, scale.Scale);
			Assert.Equal(7, offset.Offset);
		}
		string[] playback = ["--atari-presentation", "--playback-file", "session.json",
			"--tube-depth-y-gain", "0"];
		Assert.True(DebugController.TryParseTubeDepthYGainOptions(playback, out _, out _));
		Assert.True(DebugController.TryParseGameplayFileCommand(playback, out var command, out _));
		Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"), command);
	}

	[Fact]
	public void ParsingIsInvariantAcrossCurrentCultures()
	{
		CultureInfo previous = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
			Assert.True(DebugController.TryParseTubeDepthYGainOptions(
				["--atari-presentation", "--level", "13", "--tube-depth-y-gain", "-0.5"],
				out var options, out _));
			Assert.Equal(-.5f, options.Gain);
		}
		finally { CultureInfo.CurrentCulture = previous; }
	}

	[Theory]
	[InlineData(0f)]
	[InlineData(.25f)]
	[InlineData(.5f)]
	[InlineData(.75f)]
	[InlineData(1f)]
	[InlineData(-1f)]
	[InlineData(2f)]
	[InlineData(1e30f)]
	public void GainChangesOnlyDepthOriginAndPreservesLocalScale(float gain)
	{
		Vector2 point = LevelConfiguration.Level13Production.Definition.Points[0];
		var originalNear = Project(point, 50, 1);
		var originalFar = Project(point, 450, 1);
		var originalOriginNear = Project(Vector2.Zero, 50, 1);
		var originalOriginFar = Project(Vector2.Zero, 450, 1);
		var near = Project(point, 50, gain);
		var far = Project(point, 450, gain);
		var originNear = Project(Vector2.Zero, 50, gain);
		var originFar = Project(Vector2.Zero, 450, gain);
		Assert.Equal(originalNear, near);
		Assert.Equal(originalOriginNear, originNear);
		Assert.Equal(originalFar.Position.X, far.Position.X);
		Assert.Equal(originalFar.LocalScale, far.LocalScale);
		Assert.Equal(originalFar.LocalScaleX, far.LocalScaleX);
		Assert.Equal(originalFar.LocalScaleY, far.LocalScaleY);
		Assert.Equal(originalNear.LocalScale / originalFar.LocalScale, near.LocalScale / far.LocalScale);
		Assert.True(float.IsFinite(far.Position.Y));
		Assert.Equal(far, Project(point, 450, gain));
		if (MathF.Abs(gain) < 10)
		{
			float expectedDrift = 185.12396f * gain;
			Assert.InRange(MathF.Abs(originFar.Position.Y - originNear.Position.Y - expectedDrift), 0, .001f);
			Assert.InRange(MathF.Abs((far.Position.Y - originFar.Position.Y) -
				(originalFar.Position.Y - originalOriginFar.Position.Y)), 0, .001f);
		}
	}

	[Fact]
	public void NeutralMatchesPriorProjectionAndPlacement()
	{
		Assert.Equal(425f, Project(Vector2.Zero, 50, 1).Position.Y, 3);
		Assert.Equal(610.12396f, Project(Vector2.Zero, 450, 1).Position.Y, 3);
		Assert.Equal(425f, Project(Vector2.Zero, 450, 0).Position.Y, 3);
	}

	[Theory]
	[InlineData(1)] [InlineData(2)] [InlineData(3)] [InlineData(4)] [InlineData(5)]
	[InlineData(6)] [InlineData(7)] [InlineData(8)] [InlineData(9)] [InlineData(10)]
	[InlineData(11)] [InlineData(12)] [InlineData(13)] [InlineData(15)]
	public void OmittedAndExplicitSelectedDefaultAreBitIdentical(int level)
	{
		TempestGame game = CreateGame(level);
		game.CurrentLevel.State = LevelState.Playing;
		float depthYGain =
			AtariPresentationCalibration.ForDisplayedLevel(level).TubeDepthYGain;
		Assert.Equal(CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true)),
			CaptureTube(new VectorRenderer(
				game, atariPresentationEnabled: true, tubeDepthYGain: depthYGain)));
	}

	[Theory]
	[InlineData(1)] [InlineData(2)] [InlineData(3)]
	[InlineData(4)] [InlineData(5)] [InlineData(6)]
	public void SeparateProjectionPathsHaveNoCameraOriginTerm(int level)
	{
		TempestGame game = CreateGame(level);
		game.CurrentLevel.State = LevelState.Playing;
		Assert.Equal(CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true)),
			CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true, tubeDepthYGain: -2)));
	}

	[Fact]
	public void SharedRendererPreservesRegistrationAndIndependentControls()
	{
		TempestGame game = CreateGame(13);
		game.CurrentLevel.State = LevelState.Playing;
		Vector2[] geometry = game.CurrentLevel.Definition.Points.ToArray();
		int lane = game.Player.EdgeIndex;
		var hud = AtariHudVector.Render(game).ToArray();
		VectorRenderer baseline = new(game, atariPresentationEnabled: true,
			tubeYGain: 1.2f, tubeScale: .8f, tubeVerticalOffset: 17, tubeDepthYGain: 1);
		VectorRenderer zero = new(game, atariPresentationEnabled: true,
			tubeYGain: 1.2f, tubeScale: .8f, tubeVerticalOffset: 17, tubeDepthYGain: 0);
		var a = CaptureTube(baseline);
		var b = CaptureTube(zero);
		for (int i = 0; i < geometry.Length; i++)
		{
			AssertNear(a[i].Start, b[i].Start);
			AssertShift(a[i].End, b[i].End, -185.12396f);
		}
		Flipper flipper = new(4, 0);
		var flipperA = baseline.ProjectFlipperEndpoints(flipper);
		var flipperB = zero.ProjectFlipperEndpoints(flipper);
		AssertNear(b[4].End, flipperB.Start);
		AssertNear(b[5].End, flipperB.End);
		AssertShift(flipperA.Start, flipperB.Start, -185.12396f);
		Fuseball fuseball = new(4);
		AssertNear(b[4].End, zero.ProjectFuseballCenter(fuseball));
		AssertShift(baseline.ProjectFuseballCenter(fuseball), zero.ProjectFuseballCenter(fuseball), -185.12396f);
		Shot shot = new() { EdgeIndex = 4, Height = 0 };
		AssertNear((b[4].End + b[5].End) / 2, zero.ProjectShot(shot));
		AssertShift(baseline.ProjectShot(shot), zero.ProjectShot(shot), -185.12396f);
		Shot middleShot = new() { EdgeIndex = 4, Height = 200 };
		float middleDrift = Project(Vector2.Zero, 250, 1).Position.Y -
			Project(Vector2.Zero, 250, 0).Position.Y;
		AssertShift(baseline.ProjectShot(middleShot), zero.ProjectShot(middleShot), -middleDrift);
		List<(Vector2, Vector2, Color)> playerA = [], playerB = [];
		baseline.DrawPlayer(geometry, (s, e, c) => playerA.Add((s, e, c)));
		zero.DrawPlayer(geometry, (s, e, c) => playerB.Add((s, e, c)));
		Assert.Equal(playerA, playerB);
		Assert.Equal(geometry, game.CurrentLevel.Definition.Points);
		Assert.Equal(lane, game.Player.EdgeIndex);
		Assert.Equal(hud, AtariHudVector.Render(game));
		VectorRenderer translated = new(game, atariPresentationEnabled: true,
			tubeYGain: 1.2f, tubeScale: .8f, tubeVerticalOffset: 37, tubeDepthYGain: 0);
		AssertShift(b[4].End, CaptureTube(translated)[4].End, 20);
		AtariPresentationCalibration selected =
			AtariPresentationCalibration.ForDisplayedLevel(13);
		VectorRenderer defaultsWithGain = new(game, atariPresentationEnabled: true, tubeDepthYGain: 0);
		VectorRenderer explicitDefaultsWithGain = new(game, atariPresentationEnabled: true,
			tubeYGain: selected.TubeYGain, tubeScale: selected.TubeScale,
			tubeVerticalOffset: selected.VerticalOffset, tubeDepthYGain: 0);
		Assert.Equal(CaptureTube(defaultsWithGain), CaptureTube(explicitDefaultsWithGain));
	}

	[Fact]
	public void SharedMappingAppliesDuringApproachAndExitWithoutChangingSimulation()
	{
		TempestGame exitGame = CreateGame(13);
		exitGame.CurrentLevel.State = LevelState.ExitZoom;
		float acceptedDepthGain =
			AtariPresentationCalibration.ForDisplayedLevel(13).TubeDepthYGain;
		var acceptedExit = CaptureTube(new VectorRenderer(exitGame, atariPresentationEnabled: true));
		var explicitExit = CaptureTube(new VectorRenderer(
			exitGame, atariPresentationEnabled: true, tubeDepthYGain: acceptedDepthGain));
		var adjustedExit = CaptureTube(new VectorRenderer(exitGame, atariPresentationEnabled: true, tubeDepthYGain: 0));
		Assert.Equal(acceptedExit, explicitExit);
		Assert.NotEqual(acceptedExit, adjustedExit);

		TempestGame approachGame = new(startingProductionLevel: 6, random: new Random(1981));
		foreach (Spike spike in approachGame.CurrentLevel.Spikes.ToArray())
			approachGame.CurrentLevel.RemoveSpike(spike);
		approachGame.CurrentLevel.State = LevelState.ExitZoom;
		approachGame.Player.AdvanceExit(389);
		approachGame.RunOneStep();
		Assert.Equal(LevelState.Approach, approachGame.State);
		var acceptedApproach = CaptureTube(new VectorRenderer(approachGame, atariPresentationEnabled: true));
		var explicitApproach = CaptureTube(new VectorRenderer(
			approachGame, atariPresentationEnabled: true, tubeDepthYGain: 1f));
		var adjustedApproach = CaptureTube(new VectorRenderer(approachGame, atariPresentationEnabled: true, tubeDepthYGain: 0));
		Assert.Equal(acceptedApproach, explicitApproach);
		Assert.NotEqual(acceptedApproach, adjustedApproach);
		Assert.Equal(LevelState.Approach, approachGame.State);
	}

	[Fact]
	public void PlaybackSimulationDoesNotDependOnDiagnostic()
	{
		PlayerInputReplay replay = GameplayPlayback.CreateLevel1();
		GameplayStartupConfiguration startup = new(1, 3, 1.0, replay.RandomSeed);
		GameplayPlaybackSession baseline = new(startup, replay);
		GameplayPlaybackSession diagnostic = new(startup, replay);
		_ = CaptureTube(new VectorRenderer(baseline.Game, atariPresentationEnabled: true));
		_ = CaptureTube(new VectorRenderer(diagnostic.Game, atariPresentationEnabled: true, tubeDepthYGain: -1));
		Assert.Equal(JsonSerializer.Serialize(baseline.RunToBoundary(120)),
			JsonSerializer.Serialize(diagnostic.RunToBoundary(120)));
	}

	[Fact]
	public void RendererRejectsInvalidOrUnenabledGain()
	{
		TempestGame game = CreateGame(13);
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true, tubeDepthYGain: float.NaN));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true, tubeDepthYGain: float.PositiveInfinity));
		Assert.Throws<ArgumentException>(() => new VectorRenderer(game, tubeDepthYGain: 0));
		Assert.Throws<InvalidOperationException>(() => Project(Vector2.Zero, 450, float.MaxValue));
	}

	private static TempestGame CreateGame(int level) => level == 15
		? new TempestGame(LevelConfiguration.Level15Production)
		: new TempestGame(startingProductionLevel: level);

	private static AtariPresentationProjection Project(Vector2 point, float depth, float gain) =>
		AtariPresentationAdapter.Project(13, point.X, point.Y, depth, 300, 0, 50, 400,
			0, 0, 70, 5, RomPresentationMetadataCatalog.ForDisplayedLevel(13),
			tubeDepthYGain: gain);

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(VectorRenderer renderer)
	{
		List<(Vector2, Vector2, Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}

	private static void AssertNear(Vector2 expected, Vector2 actual) =>
		Assert.InRange(Vector2.Distance(expected, actual), 0, .001f);

	private static void AssertShift(Vector2 original, Vector2 changed, float delta)
	{
		Assert.InRange(MathF.Abs(original.X - changed.X), 0, .001f);
		Assert.InRange(MathF.Abs(changed.Y - original.Y - delta), 0, .001f);
	}
}
