using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class TubeYGainTests
{
	private const float TestK = 70;
	private const float TestZOffset = 5;

	[Fact]
	public void OmittedGainDefaultsToIdentity()
	{
		Assert.True(DebugController.TryParseTubeYGainOptions(
			[], out TubeYGainOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(1f, options.Gain);
		Assert.False(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("1", 1f)]
	[InlineData("1.00", 1f)]
	[InlineData("0.5", .5f)]
	[InlineData("1.21", 1.21f)]
	[InlineData("1.23", 1.23f)]
	[InlineData("1.31", 1.31f)]
	[InlineData("1.32", 1.32f)]
	[InlineData("1.33", 1.33f)]
	[InlineData("2.5", 2.5f)]
	[InlineData("1e30", 1e30f)]
	public void ParsesFiniteInvariantGainWithoutAnAestheticUpperLimit(
		string value, float expected)
	{
		Assert.True(DebugController.TryParseTubeYGainOptions(
			ArgsForLevel(1, "--tube-y-gain", value),
			out TubeYGainOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(expected, options.Gain);
		Assert.True(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("-0.5")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("invalid")]
	[InlineData("1,21")]
	public void RejectsMalformedNonFiniteOrOutOfRangeValues(string value)
	{
		Assert.False(DebugController.TryParseTubeYGainOptions(
			ArgsForLevel(1, "--tube-y-gain", value),
			out TubeYGainOptions _, out string? error));

		Assert.Contains("positive finite", error);
	}

	[Fact]
	public void OmittedUniformScaleDefaultsToIdentity()
	{
		Assert.True(DebugController.TryParseTubeScaleOptions(
			[], out TubeScaleOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(1f, options.Scale);
		Assert.False(options.WasExplicitlySpecified);
	}

	[Fact]
	public void UniformScaleCoexistsWithTheYGainDiagnosticOnEveryLevel()
	{
		for (int level = 1; level <= 10; level++)
		{
			string[] arguments =
			[
				"--atari-presentation", "--level",
				level.ToString(CultureInfo.InvariantCulture),
				"--tube-y-gain", "1.32", "--tube-scale", ".65"
			];

			Assert.True(DebugController.TryParseTubeYGainOptions(
				arguments, out TubeYGainOptions gain, out string? gainError));
			Assert.True(DebugController.TryParseTubeScaleOptions(
				arguments, out TubeScaleOptions scale, out string? scaleError));

			Assert.Null(gainError);
			Assert.Null(scaleError);
			Assert.Equal(1.32f, gain.Gain);
			Assert.Equal(.65f, scale.Scale);
		}
	}

	[Theory]
	[InlineData("0.25", .25f)]
	[InlineData("0.5", .5f)]
	[InlineData("1", 1f)]
	[InlineData("1.5", 1.5f)]
	[InlineData("10", 10f)]
	[InlineData("1000000", 1000000f)]
	[InlineData("1e30", 1e30f)]
	public void ParsesPositiveFiniteUniformScaleWithoutAnAestheticLimit(
		string value, float expected)
	{
		Assert.True(DebugController.TryParseTubeScaleOptions(
			ArgsForLevel(7, "--tube-scale", value),
			out TubeScaleOptions options, out string? error));

		Assert.Null(error);
		Assert.Equal(expected, options.Scale);
		Assert.True(options.WasExplicitlySpecified);
	}

	[Theory]
	[InlineData("0")]
	[InlineData("-0.5")]
	[InlineData("NaN")]
	[InlineData("Infinity")]
	[InlineData("-Infinity")]
	[InlineData("invalid")]
	[InlineData("1,5")]
	public void RejectsMalformedNonFiniteOrNonPositiveScale(string value)
	{
		Assert.False(DebugController.TryParseTubeScaleOptions(
			ArgsForLevel(1, "--tube-scale", value),
			out _, out string? error));

		Assert.Contains("positive finite", error);
	}

	[Fact]
	public void TubeScaleRejectsMissingDuplicateAndIncompatibleOptions()
	{
		string[][] invalidArguments =
		[
			["--atari-presentation", "--level", "1", "--tube-scale"],
			["--atari-presentation", "--level", "1",
				"--tube-scale", "0.5", "--tube-scale", "0.75"],
			["--atari-presentation", "--level", "1", "--tube-scale=0.5"]
		];

		foreach (string[] arguments in invalidArguments)
			Assert.False(DebugController.TryParseTubeScaleOptions(arguments, out _, out _));

		Assert.True(DebugController.TryParseTubeScaleOptions(
			["--level", "1", "--tube-scale", "0.5"], out TubeScaleOptions defaultAtariScale,
			out string? presentationError));
		Assert.Null(presentationError);
		Assert.Equal(.5f, defaultAtariScale.Scale);

		for (int level = 1; level <= 10; level++)
			Assert.True(DebugController.TryParseTubeScaleOptions(
				ArgsForLevel(level, "--tube-scale", "0.5"), out _, out _));
	}

	[Fact]
	public void RecordedPlaybackAcceptsUniformScaleWithoutRecordingIt()
	{
		string[] arguments =
		[
			"--atari-presentation", "--playback-file", "session.json",
			"--tube-scale", "0.65"
		];

		Assert.True(DebugController.TryParseTubeScaleOptions(
			arguments, out TubeScaleOptions options, out string? error));
		Assert.Null(error);
		Assert.True(options.WasExplicitlySpecified);

		Assert.True(DebugController.TryParseGameplayFileCommand(
			arguments, out GameplayFileCommand? command, out string? commandError));
		Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"),
			command);
		Assert.Null(commandError);
	}

	[Fact]
	public void RejectsMissingDuplicateAndIncompatibleOptions()
	{
		string[][] invalidArguments =
		[
			["--atari-presentation", "--level", "1", "--tube-y-gain"],
			["--atari-presentation", "--level", "1", "--tube-y-gain", "--lives", "3"],
			["--atari-presentation", "--level", "1",
				"--tube-y-gain", "1.21", "--tube-y-gain", "1.23"]
		];

		foreach (string[] arguments in invalidArguments)
			Assert.False(DebugController.TryParseTubeYGainOptions(
				arguments, out _, out string? error));

		Assert.True(DebugController.TryParseTubeYGainOptions(
			["--level", "1", "--tube-y-gain", "1.21"], out TubeYGainOptions defaultAtariGain,
			out string? presentationError));
		Assert.Null(presentationError);
		Assert.Equal(1.21f, defaultAtariGain.Gain);
		Assert.False(DebugController.TryParseTubeYGainOptions(
			["--atari-presentation", "--level", "1", "--tube-y-gain=1.21"],
			out _, out string? joinedValueError));
		Assert.Contains("separate value", joinedValueError);

		for (int level = 1; level <= 10; level++)
			Assert.True(DebugController.TryParseTubeYGainOptions(
				ArgsForLevel(level, "--tube-y-gain", "1.21"), out _, out _));

		Assert.False(DebugController.TryParseTubeYGainOptions(
			["--atari-presentation", "--tube-y-gain", "1.21"],
			out _, out string? missingLevelError));
		Assert.Equal("--tube-y-gain requires --level, --start-level, or --playback-file.",
			missingLevelError);
	}

	[Fact]
	public void ParsesInvariantDecimalNotationRegardlessOfCurrentCulture()
	{
		CultureInfo originalCulture = CultureInfo.CurrentCulture;
		try
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
			Assert.True(DebugController.TryParseTubeYGainOptions(
				ArgsForLevel(7, "--tube-y-gain", "1.21"),
				out TubeYGainOptions options, out string? error));

			Assert.Null(error);
			Assert.Equal(1.21f, options.Gain);
		}
		finally
		{
			CultureInfo.CurrentCulture = originalCulture;
		}
	}

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
	public void UniformScalePreservesGainAndTubeAnchorsAcrossLevels(int displayedLevel)
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: displayedLevel);
		const float gain = 1.32f;
		const float scale = .65f;
		VectorRenderer unscaledRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: gain, tubeScale: 1, tubeVerticalOffset: 0);
		VectorRenderer scaledRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: gain, tubeScale: scale, tubeVerticalOffset: 0);
		VectorRenderer changedGainRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: gain * 1.5f, tubeScale: 1, tubeVerticalOffset: 0);
		List<(Vector2 Start, Vector2 End, Color Color)> unscaled = CaptureTube(unscaledRenderer);
		List<(Vector2 Start, Vector2 End, Color Color)> scaled = CaptureTube(scaledRenderer);
		List<(Vector2 Start, Vector2 End, Color Color)> changedGain =
			CaptureTube(changedGainRenderer);
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		Vector2 nearAnchor = ProjectAnchor(
			game, configuration.Position, gain).Position;
		Vector2 farAnchor = ProjectAnchor(
			game, configuration.Position + configuration.Depth, gain).Position;

		Assert.Equal(unscaled.Count, scaled.Count);
		for (int index = 0; index < game.CurrentLevel.Definition.Points.Count; index++)
		{
			AssertScaledPoint(unscaled[index].Start, scaled[index].Start, nearAnchor, scale);
			AssertScaledPoint(unscaled[index].End, scaled[index].End, farAnchor, scale);
			Assert.Equal(unscaled[index].Color, scaled[index].Color);
		}
		Assert.Contains(unscaled.Zip(changedGain), pair =>
			pair.First.Start.Y != pair.Second.Start.Y ||
			pair.First.End.Y != pair.Second.End.Y);
	}

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
	public void OmittedControlsUseTheSelectedPerLevelCalibration(int displayedLevel)
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: displayedLevel);
		AtariPresentationCalibration selected =
			AtariPresentationCalibration.ForDisplayedLevel(displayedLevel);
		List<(Vector2 Start, Vector2 End, Color Color)> selectedDefaults = CaptureTube(
			new VectorRenderer(game, atariPresentationEnabled: true));
		List<(Vector2 Start, Vector2 End, Color Color)> explicitSelection = CaptureTube(
			new VectorRenderer(game, atariPresentationEnabled: true,
				tubeYGain: selected.TubeYGain, tubeScale: selected.TubeScale,
				tubeVerticalOffset: selected.VerticalOffset));

		Assert.Equal(selectedDefaults, explicitSelection);
	}

	[Fact]
	public void UniformScaleKeepsLevel1TubeBlasterAndEnemyBulletRegistered()
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: 1);
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		const float gain = 1.32f;
		const float scale = .6f;
		VectorRenderer unscaledRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: gain, tubeScale: 1, tubeVerticalOffset: 0);
		VectorRenderer scaledRenderer = new(
			game, atariPresentationEnabled: true,
			tubeYGain: gain, tubeScale: scale, tubeVerticalOffset: 0);
		int playerLaneBefore = game.Player.EdgeIndex;
		float playerDepthBefore = game.Player.Depth;
		int scoreBefore = game.Score.Score;
		List<(Vector2 Start, Vector2 End, Color Color)> unscaledTube = CaptureTube(unscaledRenderer);
		List<(Vector2 Start, Vector2 End, Color Color)> scaledTube = CaptureTube(scaledRenderer);
		float nearDepth = configuration.Position;
		float farDepth = configuration.Position + configuration.Depth;
		Vector2 nearAnchor = ProjectAnchor(game, nearDepth, gain).Position;
		Vector2 farAnchor = ProjectAnchor(game, farDepth, gain).Position;

		Assert.Equal(unscaledTube.Count, scaledTube.Count);
		for (int index = 0; index < game.CurrentLevel.Definition.Points.Count; index++)
		{
			AssertScaledPoint(unscaledTube[index].Start, scaledTube[index].Start, nearAnchor, scale);
			AssertScaledPoint(unscaledTube[index].End, scaledTube[index].End, farAnchor, scale);
		}

		List<(Vector2 Start, Vector2 End, Color Color)> unscaledBlaster = [];
		List<(Vector2 Start, Vector2 End, Color Color)> scaledBlaster = [];
		unscaledRenderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			(start, end, color) => unscaledBlaster.Add((start, end, color)));
		scaledRenderer.DrawPlayer(game.CurrentLevel.Definition.Points,
			(start, end, color) => scaledBlaster.Add((start, end, color)));
		Assert.Equal(unscaledBlaster.Count, scaledBlaster.Count);
		for (int index = 0; index < unscaledBlaster.Count; index++)
		{
			AssertScaledPoint(unscaledBlaster[index].Start, scaledBlaster[index].Start,
				nearAnchor, scale);
			AssertScaledPoint(unscaledBlaster[index].End, scaledBlaster[index].End,
				nearAnchor, scale);
		}

		EnemyBullet bullet = new(0, 12);
		Vector2 unscaledBullet = unscaledRenderer.ProjectEnemyBullet(bullet);
		Vector2 scaledBullet = scaledRenderer.ProjectEnemyBullet(bullet);
		Vector2 bulletAnchor = ProjectAnchor(
			game, configuration.Position + configuration.Depth - bullet.Height, gain).Position;
		AssertScaledPoint(unscaledBullet, scaledBullet, bulletAnchor, scale);
		Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
		Assert.Equal(playerDepthBefore, game.Player.Depth);
		Assert.Equal(scoreBefore, game.Score.Score);
	}

	[Fact]
	public void RecordedPlaybackAcceptsYGainWithoutRecordingIt()
	{
		string[] arguments =
		[
			"--atari-presentation", "--playback-file", "session.json",
			"--tube-y-gain", "1.31"
		];

		Assert.True(DebugController.TryParseTubeYGainOptions(
			arguments, out TubeYGainOptions options, out string? error));
		Assert.Null(error);
		Assert.True(options.WasExplicitlySpecified);

		Assert.True(DebugController.TryParseGameplayFileCommand(
			arguments, out GameplayFileCommand? command, out string? commandError));
		Assert.Equal(new GameplayFileCommand(GameplayFileMode.Playback, "session.json"),
			command);
		Assert.Null(commandError);
	}

	[Fact]
	public void RecordingOptionDoesNotBecomeGameplayStartupMetadata()
	{
		string[] arguments =
		[
			"--record-gameplay", "session.json", "--level", "7",
			"--atari-presentation", "--tube-y-gain", "1.31"
		];

		Assert.True(DebugController.TryParseTubeYGainOptions(
			arguments, out TubeYGainOptions options, out string? gainError));
		Assert.True(DebugController.TryParseGameplayFileCommand(
			arguments, out GameplayFileCommand? command, out string? commandError));

		Assert.Equal(1.31f, options.Gain);
		Assert.Equal(new GameplayFileCommand(GameplayFileMode.Record, "session.json", 7),
			command);
		Assert.Null(gainError);
		Assert.Null(commandError);
		Assert.DoesNotContain("TubeYGain", JsonSerializer.Serialize(command));

		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-tube-y-gain-{Guid.NewGuid():N}");
		Directory.CreateDirectory(directory);
		try
		{
			string path = Path.Combine(directory, "recording.json");
			PlayerInputRecorder recorder = new(
				new GameplayStartupConfiguration(7, 3, 1.0, 1981));
			GameplayRecordingFile.SaveNew(path, recorder);
			using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));

			Assert.Equal(GameplayRecordingFile.CurrentFormatVersion,
				document.RootElement.GetProperty("Version").GetInt32());
			Assert.False(document.RootElement.TryGetProperty("TubeYGain", out _));
		}
		finally
		{
			Directory.Delete(directory, recursive: true);
		}
	}

	[Theory]
	[InlineData(1, 1.21f)]
	[InlineData(1, 1.23f)]
	[InlineData(1, 1.31f)]
	[InlineData(7, 1.21f)]
	[InlineData(7, 1.23f)]
	[InlineData(7, 1.31f)]
	public void ScalesOnlySourceCrossSectionYAndItsLocalJacobianAtNearAndFarDepth(
		int displayedLevel, float gain)
	{
		LevelConfiguration configuration = GetConfiguration(displayedLevel);
		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		float[] depths =
		[
			configuration.Position,
			configuration.Position + configuration.Depth
		];

		foreach (float depth in depths)
		{
			AtariPresentationProjection identity = Project(
				configuration, metadata, 13, -9, depth, 4, 17);
			AtariPresentationProjection explicitIdentity = Project(
				configuration, metadata, 13, -9, depth, 4, 17, 1);
			AtariPresentationProjection changed = Project(
				configuration, metadata, 13, -9, depth, 4, 17, gain);
			AtariPresentationProjection identityCenter = Project(
				configuration, metadata, 13, 0, depth, 4, 17);
			AtariPresentationProjection changedCenter = Project(
				configuration, metadata, 13, 0, depth, 4, 17, gain);
			AtariPresentationProjection identityOrigin = Project(
				configuration, metadata, 0, 0, depth, 4, 17);
			AtariPresentationProjection changedOrigin = Project(
				configuration, metadata, 0, 0, depth, 4, 17, gain);

			Assert.Equal(identity, explicitIdentity);
			Assert.Equal(identityCenter.Position, changedCenter.Position);
			Assert.Equal(identityOrigin.Position, changedOrigin.Position);
			Assert.Equal(identity.Position.X, changed.Position.X);
			Assert.Equal(identity.LocalScaleX, changed.LocalScaleX);
			Assert.Equal(identity.LocalScale, changed.LocalScale);
			Assert.Equal(identity.LocalScaleY * gain, changed.LocalScaleY);
			Assert.InRange(
				MathF.Abs(
					(changed.Position.Y - changedCenter.Position.Y) -
					(identity.Position.Y - identityCenter.Position.Y) * gain),
				0,
				.0001f);
		}
	}

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
	public void TubeRenderingUsesGainOnEveryImplementedLevel(int displayedLevel)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> identity = CaptureTube(
			displayedLevel, AtariPresentationAdapter.DefaultTubeYGain);
		List<(Vector2 Start, Vector2 End, Color Color)> diagnostic = CaptureTube(
			displayedLevel, 1.31f);

		Assert.Equal(identity.Count, diagnostic.Count);
		Assert.Contains(identity.Zip(diagnostic), pair =>
			pair.First.Start.Y != pair.Second.Start.Y ||
			pair.First.End.Y != pair.Second.End.Y);
	}

	[Fact]
	public void FiniteGainBeyondPreviousLimitRequiresAtariPresentationInRenderer()
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: 1);

		Assert.Throws<ArgumentException>(() =>
			new VectorRenderer(game, tubeYGain: 1.21f));
		Assert.NotNull(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeYGain: 100f));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true, tubeYGain: float.NaN));
		Assert.NotNull(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeYGain: .5f));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new VectorRenderer(game, atariPresentationEnabled: true, tubeYGain: 0));
		Assert.Throws<ArgumentException>(() =>
			new VectorRenderer(game, tubeScale: .5f));
		Assert.NotNull(new VectorRenderer(
			game, atariPresentationEnabled: true, tubeScale: float.MaxValue));
	}

	[Theory]
	[InlineData(1)]
	[InlineData(7)]
	public void TubeGeometryUsesGainWithoutChangingXOrGameplayState(int displayedLevel)
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: displayedLevel);
		VectorRenderer identityRenderer = new(
			game, atariPresentationEnabled: true, tubeYGain: 1f);
		VectorRenderer diagnosticRenderer = new(
			game, atariPresentationEnabled: true, tubeYGain: 1.31f);
		List<(Vector2 Start, Vector2 End, Color Color)> identity = CaptureTube(identityRenderer);
		int playerLaneBefore = game.Player.EdgeIndex;
		float playerDepthBefore = game.Player.Depth;
		int scoreBefore = game.Score.Score;
		List<(Vector2 Start, Vector2 End, Color Color)> diagnostic =
			CaptureTube(diagnosticRenderer);

		Assert.Equal(identity.Count, diagnostic.Count);
		Assert.Contains(identity.Zip(diagnostic), pair =>
			pair.First.Start.Y != pair.Second.Start.Y ||
			pair.First.End.Y != pair.Second.End.Y);
		for (int index = 0; index < identity.Count; index++)
		{
			Assert.Equal(identity[index].Start.X, diagnostic[index].Start.X);
			Assert.Equal(identity[index].End.X, diagnostic[index].End.X);
		}
		Assert.Equal(playerLaneBefore, game.Player.EdgeIndex);
		Assert.Equal(playerDepthBefore, game.Player.Depth);
		Assert.Equal(scoreBefore, game.Score.Score);
	}

	[Fact]
	public void PlaybackBoundaryIsIdenticalAcrossDifferentPresentationDiagnostics()
	{
		PlayerInputReplay replay = GameplayPlayback.CreateLevel1();
		GameplayStartupConfiguration startup = new(1, 3, 1.0, replay.RandomSeed);
		GameplayPlaybackSession identity = new(startup, replay);
		GameplayPlaybackSession diagnostic = new(startup, replay);
		VectorRenderer identityRenderer = new(
			identity.Game, atariPresentationEnabled: true, tubeYGain: 1f);
		VectorRenderer diagnosticRenderer = new(
			diagnostic.Game, atariPresentationEnabled: true,
			tubeYGain: 2.5f, tubeScale: .65f, tubeVerticalOffset: 35);
		_ = CaptureTube(identityRenderer);
		_ = CaptureTube(diagnosticRenderer);

		GameplayStateSummary identitySummary = identity.RunToBoundary(120);
		GameplayStateSummary diagnosticSummary = diagnostic.RunToBoundary(120);

		Assert.Equal(identity.CurrentBoundary, diagnostic.CurrentBoundary);
		Assert.Equal(identity.RecordingDurationTicks, diagnostic.RecordingDurationTicks);
		Assert.Equal(JsonSerializer.Serialize(identitySummary),
			JsonSerializer.Serialize(diagnosticSummary));
	}

	private static string[] ArgsForLevel(int level, string option, string value) =>
		["--atari-presentation", "--level", level.ToString(CultureInfo.InvariantCulture),
			option, value];

	private static LevelConfiguration GetConfiguration(int displayedLevel) =>
		LevelProgression.GetDirectStartLevel(displayedLevel) ??
		throw new ArgumentOutOfRangeException(nameof(displayedLevel));

	private static AtariPresentationProjection Project(
		LevelConfiguration configuration,
		RomPresentationMetadata metadata,
		float x,
		float y,
		float depth,
		float approachDepthOffset = 0,
		float approachVerticalOffset = 0,
		float tubeYGain = 1f) =>
		AtariPresentationAdapter.Project(
			configuration.DisplayedLevel,
			x,
			y,
			depth,
			configuration.Definition.CenterY,
			configuration.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			approachDepthOffset,
			approachVerticalOffset,
			TestK,
			TestZOffset,
			metadata,
			tubeYGain: tubeYGain);

	private static AtariPresentationProjection ProjectAnchor(
		Tempest.Game.TempestGame game, float depth, float tubeYGain)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		if (configuration.DisplayedLevel == 5)
			return AtariPresentationAdapter.ProjectLevel5(
				0, 0, depth, game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset, configuration.Position,
				configuration.Depth, 0, 0, Projection.K, Projection.ZOffset);
		if (configuration.DisplayedLevel == 6)
			return AtariPresentationAdapter.ProjectLevel6(
				0, 0, depth, game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset, configuration.Position,
				configuration.Depth, 0, 0, Projection.K, Projection.ZOffset);

		return Project(
			configuration,
			RomPresentationMetadataCatalog.ForDisplayedLevel(configuration.DisplayedLevel),
			0, 0, depth, tubeYGain: tubeYGain);
	}

	private static void AssertScaledPoint(
		Vector2 original,
		Vector2 scaled,
		Vector2 anchor,
		float scale)
	{
		Vector2 expected = anchor + (original - anchor) * scale;
		Assert.InRange(Vector2.Distance(expected, scaled), 0, .001f);
	}

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		int displayedLevel,
		float tubeYGain)
	{
		Tempest.Game.TempestGame game = new(startingProductionLevel: displayedLevel);
		return CaptureTube(new VectorRenderer(
			game, atariPresentationEnabled: true,
			tubeYGain: tubeYGain, tubeScale: 1, tubeVerticalOffset: 0));
	}

	private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
		VectorRenderer renderer)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
		renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
		return lines;
	}
}
