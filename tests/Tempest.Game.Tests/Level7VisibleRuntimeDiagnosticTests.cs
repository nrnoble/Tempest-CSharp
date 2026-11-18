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

public sealed class Level7VisibleRuntimeDiagnosticTests
{
	[Fact]
	public void CapturesPresentedGeometryMotionAndLifecycleAsStructuredJsonLines()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-runtime-{Guid.NewGuid():N}");
		string outputPath = Path.Combine(directory, "trace.jsonl");
		object entity = new();
		Camera2D camera = new()
		{
			Target = new(10, 20),
			Offset = new(100, 200),
			Rotation = 0,
			Zoom = 2
		};

		try
		{
			using (Level7VisibleRuntimeDiagnostic diagnostic = new(
				outputPath, "manualGameplay", null, "ExperimentalPortrait", 60,
				atariPresentationEnabled: true))
			{
				diagnostic.BeginFrame(
					0, 1, null, 7, LevelState.Playing, false, camera, 1_000);
				diagnostic.CaptureEntity(
					entity,
					"tanker",
					"Climbing",
					2,
					10,
					10,
					new(15, 25),
					.975f,
					new(15, 25),
					.2f,
					6.2f,
					[new(14, 24), new(16, 26)],
					1);
				diagnostic.EndFrame(2_000);

				diagnostic.BeginFrame(
					1, 2, null, 7, LevelState.Playing, false, camera, 2_000);
				diagnostic.CaptureEntity(
					entity,
					"tanker",
					"Climbing",
					2,
					11.75f,
					11,
					new(16, 25),
					.970625f,
					new(16, 25),
					.21f,
					6.46f,
					[new(15, 24), new(17, 26)],
					1);
				diagnostic.EndFrame(3_000);

				diagnostic.BeginFrame(
					2, 3, null, 7, LevelState.Playing, false, camera, 3_000,
					"recordedPlayback");
				diagnostic.CaptureEntity(
					entity,
					"tanker",
					"CargoReleasePending",
					3,
					13.5f,
					12,
					new(17, 25),
					.96625f,
					new(17, 25),
					.22f,
					6.72f,
					[new(16, 24), new(18, 26)],
					1);
				diagnostic.EndFrame(4_000);

				diagnostic.BeginFrame(
					3, 4, null, 7, LevelState.Playing, false, camera, 4_000);
				diagnostic.EndFrame(5_000);
			}

			string[] lines = File.ReadAllLines(outputPath);
			Assert.DoesNotContain(lines, line =>
				line.Contains("\"framebufferCapture\"", StringComparison.Ordinal));
			using (JsonDocument run = JsonDocument.Parse(lines[0]))
			{
				Assert.Equal(1, run.RootElement.GetProperty("schemaVersion").GetInt32());
				Assert.False(run.RootElement.TryGetProperty("atariPresentationEnabled", out _));
				Assert.Contains(
					"framebuffer contents, monitor scanout",
					run.RootElement.GetProperty("presentationTiming").GetString());
			}
			string[] entityLines = lines.Where(line =>
				line.Contains("\"recordType\":\"entity\"", StringComparison.Ordinal)).ToArray();
			using JsonDocument firstEntity =
				JsonDocument.Parse(entityLines[0]);
			using JsonDocument secondEntity =
				JsonDocument.Parse(entityLines[1]);
			using JsonDocument transitionEntity =
				JsonDocument.Parse(entityLines[2]);
			using JsonDocument removal =
				JsonDocument.Parse(lines.Single(line =>
					line.Contains("\"recordType\":\"entityRemoved\"", StringComparison.Ordinal)));

			JsonElement first = firstEntity.RootElement;
			Assert.Equal("spawn", first.GetProperty("events")[0].GetString());
			Assert.Equal(110, first.GetProperty("hostAnchor").GetProperty("x").GetSingle());
			Assert.Equal(210, first.GetProperty("hostAnchor").GetProperty("y").GetSingle());
			Assert.Equal(1, first.GetProperty("submittedPrimitiveCount").GetInt32());
			Assert.False(first.TryGetProperty("submittedCoordinates", out _));

			JsonElement second = secondEntity.RootElement;
			Assert.True(second.GetProperty("fixedLaneContinuous").GetBoolean());
			Assert.Equal(2, second.GetProperty("hostAnchorDelta").GetProperty("x").GetSingle());
			Assert.Equal(0, second.GetProperty("hostAnchorDelta").GetProperty("y").GetSingle());
			Assert.True(second.GetProperty("displacementRateHostPixelsPerSecond").GetDouble() > 0);

			JsonElement transition = transitionEntity.RootElement;
			Assert.False(transition.GetProperty("fixedLaneContinuous").GetBoolean());
			Assert.Contains(transition.GetProperty("events").EnumerateArray(),
				value => value.GetString() == "laneChanged");
			Assert.Contains(transition.GetProperty("events").EnumerateArray(),
				value => value.GetString() == "stateChanged");
			Assert.Contains(transition.GetProperty("events").EnumerateArray(),
				value => value.GetString() == "runtimeModeChanged");
			Assert.Equal(
				transition.GetProperty("entityId").GetInt64(),
				removal.RootElement.GetProperty("entityId").GetInt64());
			Assert.Contains(lines, line =>
				line.Contains("\"recordType\":\"frame\"", StringComparison.Ordinal));
			Assert.Contains(lines, line =>
				line.Contains("\"recordType\":\"runEnded\"", StringComparison.Ordinal));
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void RequiresOneNewJsonlFilenameAndAcceptsItWithFilePlayback()
	{
		Assert.True(DebugController.TryParseLevel7VisibleRuntimeDiagnostic(
			[], out string? disabledFileName, out string? disabledError));
		Assert.Null(disabledFileName);
		Assert.Null(disabledError);

		Assert.True(DebugController.TryParseLevel7VisibleRuntimeDiagnostic(
			["--level7-visible-runtime-diagnostic", "control.jsonl"],
			out string? fileName,
			out string? error));
		Assert.Equal("control.jsonl", fileName);
		Assert.Null(error);

		Assert.True(DebugController.TryParseGameplayFileCommand(
			[
				"--atari-presentation",
				"--playback-file", "old-level7.json",
				"--level7-visible-runtime-diagnostic", "control.jsonl"
			],
			out GameplayFileCommand? command,
			out string? fileError));
		Assert.Equal(GameplayFileMode.Playback, command?.Mode);
		Assert.Null(fileError);

		string[][] invalidArguments =
		[
			["--level7-visible-runtime-diagnostic"],
			["--level7-visible-runtime-diagnostic", "--level", "7"],
			["--level7-visible-runtime-diagnostic", "trace.json"],
			["--level7-visible-runtime-diagnostic", "folder\\trace.jsonl"],
			[
				"--level7-visible-runtime-diagnostic", "first.jsonl",
				"--level7-visible-runtime-diagnostic", "second.jsonl"
			]
		];
		foreach (string[] arguments in invalidArguments)
			Assert.False(DebugController.TryParseLevel7VisibleRuntimeDiagnostic(
				arguments, out _, out _));
	}

	[Fact]
	public void FramebufferDiagnosticRequiresOneNewJsonlFilename()
	{
		Assert.True(DebugController.TryParseLevel7FramebufferDiagnostic(
			[], out string? disabledFileName, out string? disabledError));
		Assert.Null(disabledFileName);
		Assert.Null(disabledError);

		Assert.True(DebugController.TryParseLevel7FramebufferDiagnostic(
			["--level7-framebuffer-diagnostic", "manual.jsonl"],
			out string? fileName,
			out string? error));
		Assert.Equal("manual.jsonl", fileName);
		Assert.Null(error);
		Assert.False(DebugController.TryValidateLevel7FramebufferDiagnosticContext(
			framebufferDiagnosticEnabled: true,
			visibleRuntimeDiagnosticEnabled: false,
			out string? missingRuntimeDiagnosticError));
		Assert.Contains("--level7-visible-runtime-diagnostic", missingRuntimeDiagnosticError);
		Assert.True(DebugController.TryValidateLevel7FramebufferDiagnosticContext(
			framebufferDiagnosticEnabled: true,
			visibleRuntimeDiagnosticEnabled: true,
			out string? compatibleError));
		Assert.Null(compatibleError);
		Assert.True(DebugController.TryParseGameplayFileCommand(
			["--playback-file", "old-level7.json"],
			out GameplayFileCommand? diagnosticsDisabledPlaybackCommand,
			out string? diagnosticsDisabledPlaybackError));
		Assert.Equal(
			new GameplayFileCommand(GameplayFileMode.Playback, "old-level7.json"),
			diagnosticsDisabledPlaybackCommand);
		Assert.Null(diagnosticsDisabledPlaybackError);

		Assert.True(DebugController.TryParseGameplayFileCommand(
			[
				"--playback-file", "old-level7.json",
				"--level7-visible-runtime-diagnostic", "runtime.jsonl",
				"--level7-framebuffer-diagnostic", "frames.jsonl"
			],
			out GameplayFileCommand? playbackCommand,
			out string? composedError));
		Assert.Equal(
			new GameplayFileCommand(GameplayFileMode.Playback, "old-level7.json"),
			playbackCommand);
		Assert.Null(composedError);

		Assert.False(DebugController.TryParseGameplayFileCommand(
			[
				"--playback-file", "old-level7.json",
				"--level7-visible-runtime-diagnostic", "runtime.jsonl",
				"--level7-framebuffer-diagnostic", "frames.jsonl",
				"--unrelated-gameplay-file-option"
			],
			out GameplayFileCommand? unknownOptionCommand,
			out string? unknownOptionError));
		Assert.Null(unknownOptionCommand);
		Assert.Contains(
			"Unknown gameplay file option or argument '--unrelated-gameplay-file-option'",
			unknownOptionError);

		string[] missingFramebufferValueArguments =
		[
			"--playback-file", "old-level7.json",
			"--level7-visible-runtime-diagnostic", "runtime.jsonl",
			"--level7-framebuffer-diagnostic"
		];
		Assert.False(DebugController.TryParseLevel7FramebufferDiagnostic(
			missingFramebufferValueArguments, out _, out string? dedicatedValidationError));
		Assert.False(DebugController.TryParseGameplayFileCommand(
			missingFramebufferValueArguments, out _, out string? composedValidationError));
		Assert.Equal(dedicatedValidationError, composedValidationError);
		Assert.Equal(
			"--level7-framebuffer-diagnostic requires a new .jsonl filename.",
			composedValidationError);

		string[][] invalidArguments =
		[
			["--level7-framebuffer-diagnostic"],
			["--level7-framebuffer-diagnostic", "--level", "7"],
			["--level7-framebuffer-diagnostic", "frames.json"],
			["--level7-framebuffer-diagnostic", "folder\\frames.jsonl"],
			[
				"--level7-framebuffer-diagnostic", "first.jsonl",
				"--level7-framebuffer-diagnostic", "second.jsonl"
			]
		];
		foreach (string[] arguments in invalidArguments)
			Assert.False(DebugController.TryParseLevel7FramebufferDiagnostic(
				arguments, out _, out _));
	}

	[Fact]
	public void AtariOffIsAllowedOnlyForInteractiveRecordedPlaybackFramebufferCapture()
	{
		Assert.True(DebugController.TryValidateLevel7VisibleRuntimeDiagnosticContext(
			visibleRuntimeDiagnosticEnabled: true,
			framebufferDiagnosticEnabled: true,
			atariPresentationEnabled: false,
			playbackFileSelected: true,
			headlessPlaybackAnalysis: false,
			inputDiagnostic: false,
			atariTubePrototypeEnabled: false,
			atariTubeRn000fDiagnosticEnabled: false,
			atariTubeReferenceShapeDiagnosticEnabled: false,
			out string? offError));
		Assert.Null(offError);

		Assert.True(DebugController.TryValidateLevel7VisibleRuntimeDiagnosticContext(
			visibleRuntimeDiagnosticEnabled: true,
			framebufferDiagnosticEnabled: true,
			atariPresentationEnabled: true,
			playbackFileSelected: true,
			headlessPlaybackAnalysis: false,
			inputDiagnostic: false,
			atariTubePrototypeEnabled: false,
			atariTubeRn000fDiagnosticEnabled: false,
			atariTubeReferenceShapeDiagnosticEnabled: false,
			out string? onError));
		Assert.Null(onError);

		Assert.False(DebugController.TryValidateLevel7VisibleRuntimeDiagnosticContext(
			visibleRuntimeDiagnosticEnabled: true,
			framebufferDiagnosticEnabled: false,
			atariPresentationEnabled: false,
			playbackFileSelected: true,
			headlessPlaybackAnalysis: false,
			inputDiagnostic: false,
			atariTubePrototypeEnabled: false,
			atariTubeRn000fDiagnosticEnabled: false,
			atariTubeReferenceShapeDiagnosticEnabled: false,
			out _));
		Assert.False(DebugController.TryValidateLevel7VisibleRuntimeDiagnosticContext(
			visibleRuntimeDiagnosticEnabled: true,
			framebufferDiagnosticEnabled: true,
			atariPresentationEnabled: false,
			playbackFileSelected: false,
			headlessPlaybackAnalysis: false,
			inputDiagnostic: false,
			atariTubePrototypeEnabled: false,
			atariTubeRn000fDiagnosticEnabled: false,
			atariTubeReferenceShapeDiagnosticEnabled: false,
			out _));
		Assert.False(DebugController.TryValidateLevel7VisibleRuntimeDiagnosticContext(
			visibleRuntimeDiagnosticEnabled: true,
			framebufferDiagnosticEnabled: true,
			atariPresentationEnabled: false,
			playbackFileSelected: true,
			headlessPlaybackAnalysis: true,
			inputDiagnostic: false,
			atariTubePrototypeEnabled: false,
			atariTubeRn000fDiagnosticEnabled: false,
			atariTubeReferenceShapeDiagnosticEnabled: false,
			out _));

		Assert.True(DebugController.TryParseAtariPresentation(
			["--playback-file", "Level-7-slowmo.json"],
			out bool offMode,
			out string? parseError));
		Assert.False(offMode);
		Assert.Null(parseError);
		Assert.True(DebugController.TryParseAtariPresentation(
			["--playback-file", "Level-7-slowmo.json", "--atari-presentation"],
			out bool onMode,
			out parseError));
		Assert.True(onMode);
		Assert.Null(parseError);
	}

	[Fact]
	public void RendererTraceUsesTheExactSubmittedPlayerShotPointGeometry()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-shot-trace-{Guid.NewGuid():N}");
		string outputPath = Path.Combine(directory, "shot.jsonl");
		try
		{
			TempestGame game = new(level7Test: true, random: new Random(1981));
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			Shot shot = Assert.Single(game.Shots);
			List<Vector2> submittedPoints = [];

			using (Level7VisibleRuntimeDiagnostic diagnostic = new(
				outputPath, "manualGameplay", null, "ExperimentalPortrait", 60,
				atariPresentationEnabled: true))
			{
				diagnostic.BeginFrame(
					0,
					1,
					null,
					7,
					LevelState.Playing,
					false,
					DisplayConfiguration.ExperimentalPortraitPlayfieldCamera,
					1_000);
				new VectorRenderer(
					game,
					atariPresentationEnabled: true,
					level7VisibleRuntimeDiagnostic: diagnostic).DrawPlayerShots(
						(position, _) => submittedPoints.Add(position));
				diagnostic.EndFrame(2_000);
			}

			using JsonDocument entity = JsonDocument.Parse(
				File.ReadLines(outputPath).Single(line =>
					line.Contains("\"recordType\":\"entity\"", StringComparison.Ordinal)));
			JsonElement record = entity.RootElement;
			Assert.Equal("playerShot", record.GetProperty("entityType").GetString());
			Assert.Equal("Traveling", record.GetProperty("state").GetString());
			Assert.Equal(shot.Height, record.GetProperty("height").GetSingle());
			Assert.Equal(submittedPoints.Count,
				record.GetProperty("submittedPrimitiveCount").GetInt32());
			Assert.True(record.GetProperty("logicalGeometryBounds")
				.GetProperty("maxX").GetSingle() >= submittedPoints.Max(point => point.X));
			Assert.True(record.GetProperty("logicalGeometryBounds")
				.GetProperty("minX").GetSingle() <= submittedPoints.Min(point => point.X));
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void CapturesLegacyLevel7GeometryWithoutAtariMappedValuesWhenPresentationIsOff()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-legacy-trace-{Guid.NewGuid():N}");
		string outputPath = Path.Combine(directory, "legacy.jsonl");
		try
		{
			TempestGame game = new(level7Test: true, random: new Random(1981));
			game.RunOneStep(new PlayerInput(true, 0, false, false));
			List<Vector2> submittedPoints = [];
			using (Level7VisibleRuntimeDiagnostic diagnostic = new(
				outputPath,
				"recordedPlayback",
				"recording.json",
				"ExperimentalPortrait",
				60,
				framebufferCaptureEnabled: true,
				atariPresentationEnabled: false))
			{
				diagnostic.BeginFrame(
					0,
					98,
					98,
					7,
					LevelState.Playing,
					false,
					DisplayConfiguration.ExperimentalPortraitPlayfieldCamera,
					1_000,
					"recordedPlayback");
				diagnostic.EndFrame(1_500);
				diagnostic.BeginFrame(
					1,
					99,
					99,
					7,
					LevelState.Playing,
					false,
					DisplayConfiguration.ExperimentalPortraitPlayfieldCamera,
					2_000,
					"recordedPlayback");
				new VectorRenderer(
					game,
					atariPresentationEnabled: false,
					level7VisibleRuntimeDiagnostic: diagnostic).DrawPlayerShots(
						(position, _) => submittedPoints.Add(position));
				diagnostic.EndFrame(3_000);
			}

			using JsonDocument entity = JsonDocument.Parse(
				File.ReadLines(outputPath).Single(line =>
					line.Contains("\"recordType\":\"entity\"", StringComparison.Ordinal)));
			JsonElement record = entity.RootElement;
			Assert.Equal("playerShot", record.GetProperty("entityType").GetString());
			Assert.False(record.GetProperty("atariPresentationEnabled").GetBoolean());
			Assert.Equal(JsonValueKind.Null,
				record.GetProperty("normalizedDepth").ValueKind);
			Assert.Equal(JsonValueKind.Null, record.GetProperty("mappedAnchor").ValueKind);
			Assert.True(record.GetProperty("rendererAnchor").ValueKind == JsonValueKind.Object);
			Assert.Equal(
				submittedPoints.Count,
				record.GetProperty("submittedCoordinates").GetArrayLength());
			Assert.Equal(
				submittedPoints.Count,
				record.GetProperty("cameraTransformedCoordinates").GetArrayLength());
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}

	[Fact]
	public void OmitsContinuousLevel7ApproachFramesButMarksTheTransitionIntoPlaying()
	{
		string directory = Path.Combine(
			Path.GetTempPath(), $"tempest-level7-state-trace-{Guid.NewGuid():N}");
		string outputPath = Path.Combine(directory, "state.jsonl");
		Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;

		try
		{
			using (Level7VisibleRuntimeDiagnostic diagnostic = new(
				outputPath, "manualGameplay", null, "ExperimentalPortrait", 60))
			{
				diagnostic.BeginFrame(
					0, 0, null, 7, LevelState.Approach, false, camera, 1_000);
				diagnostic.EndFrame(2_000);
				diagnostic.BeginFrame(
					1, 0, null, 7, LevelState.Approach, false, camera, 2_000);
				diagnostic.EndFrame(3_000);
				diagnostic.BeginFrame(
					2, 1, null, 7, LevelState.Playing, false, camera, 3_000);
				diagnostic.EndFrame(4_000);
			}

			string[] lines = File.ReadAllLines(outputPath);
			Assert.Single(lines, line =>
				line.Contains("\"recordType\":\"frame\"", StringComparison.Ordinal));
			using JsonDocument transition = JsonDocument.Parse(lines.Single(line =>
				line.Contains("\"recordType\":\"frame\"", StringComparison.Ordinal)));
			Assert.True(transition.RootElement.GetProperty("stateTransition").GetBoolean());
			Assert.Equal("Playing", transition.RootElement.GetProperty("levelState").GetString());
			Assert.Equal("Approach",
				transition.RootElement.GetProperty("previousLevelState").GetString());
		}
		finally
		{
			if (Directory.Exists(directory))
				Directory.Delete(directory, recursive: true);
		}
	}
}
