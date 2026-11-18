using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game.Levels;

namespace Tempest.Game.Debug;

public sealed class Level7VisibleRuntimeDiagnostic : IDisposable
{
	public const int MaximumLevel7RenderFrames = 60000;

	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	private readonly StreamWriter writer;
	private readonly Dictionary<object, long> entityIds =
		new(ReferenceEqualityComparer.Instance);
	private readonly Dictionary<object, PreviousEntity> previousEntities =
		new(ReferenceEqualityComparer.Instance);
	private readonly List<CapturedEntity> frameEntities = [];
	private readonly string? playbackFilePath;
	private readonly bool atariPresentationEnabled;
	private readonly bool framebufferCaptureEnabled;
	private readonly bool traceOnlyPlaybackBoundaryWindows;
	private readonly HashSet<long> recordedPlaybackBoundaries = [];
	private FrameContext? currentFrame;
	private Level7FramebufferCaptureRecord? pendingFramebufferCapture;
	private long nextEntityId = 1;
	private long previousPresentedTimestamp;
	private long writtenFrameCount;
	private long? previousSimulationTick;
	private long? previousPlaybackBoundary;
	private LevelState? previousLevelState;
	private bool sawLevel7;
	private bool scopeEnded;
	private bool limitReached;
	private bool disposed;

	public Level7VisibleRuntimeDiagnostic(
		string outputPath,
		string runtimeMode,
		string? playbackFilePath,
		string presentationMode,
		int targetFramesPerSecond,
		bool framebufferCaptureEnabled = false,
		bool atariPresentationEnabled = false)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
		ArgumentException.ThrowIfNullOrWhiteSpace(runtimeMode);
		ArgumentException.ThrowIfNullOrWhiteSpace(presentationMode);

		OutputPath = Path.GetFullPath(outputPath);
		this.playbackFilePath = playbackFilePath;
		this.atariPresentationEnabled = atariPresentationEnabled;
		this.framebufferCaptureEnabled = framebufferCaptureEnabled;
		traceOnlyPlaybackBoundaryWindows =
			framebufferCaptureEnabled && playbackFilePath is not null;
		Directory.CreateDirectory(Path.GetDirectoryName(OutputPath)!);
		writer = new StreamWriter(
			new FileStream(OutputPath, FileMode.CreateNew, FileAccess.Write, FileShare.Read),
			new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
			bufferSize: 64 * 1024);
		Dictionary<string, object?> runRecord = new()
		{
			["recordType"] = "run",
			["schemaVersion"] = framebufferCaptureEnabled ? 2 : 1,
			["startedUtc"] = DateTimeOffset.UtcNow,
			["outputPath"] = OutputPath,
			["runtimeMode"] = runtimeMode,
			["playbackFilePath"] = playbackFilePath,
			["presentationMode"] = presentationMode,
			["targetFramesPerSecond"] = targetFramesPerSecond,
			["stopwatchFrequency"] = Stopwatch.Frequency,
			["renderStage"] = framebufferCaptureEnabled && playbackFilePath is not null
				? "VectorRenderer arguments immediately before drawing; submitted coordinates and camera-transformed Raylib window-pixel coordinates are recorded"
				: "VectorRenderer arguments immediately before drawing; logical draw coordinates and camera-transformed Raylib window-pixel bounds are recorded",
			["presentationTiming"] = framebufferCaptureEnabled
				? "Monotonic Stopwatch timestamp immediately after EndDrawing returns; selected actual screen-buffer " +
					"pixels are read before EndDrawing swaps the production frame, with readback and PNG-write " +
					"durations recorded; monitor scanout and physical display presentation are not observable"
				: "Monotonic Stopwatch timestamp immediately after EndDrawing returns; framebuffer contents, monitor scanout, and physical display presentation are not observable"
		};
		if (framebufferCaptureEnabled)
		{
			runRecord["atariPresentationEnabled"] = atariPresentationEnabled;
			if (playbackFilePath is not null)
			{
				runRecord["framebufferCaptureSelection"] = new
				{
					windowCenters = Level7FramebufferDiagnostic.PlaybackCaptureWindowCenters,
					playbackBoundaries = Level7FramebufferDiagnostic.PlaybackCaptureBoundaries,
					consecutiveFramesPerWindow = Level7FramebufferDiagnostic.PlaybackFramesPerBoundaryWindow,
					boundaryWindows = "N-1, N, N+1",
					maximumImages = Level7FramebufferDiagnostic.MaximumPlaybackFrames,
					maximumBytesPerImage = Level7FramebufferDiagnostic.MaximumPngFileBytes,
					maximumTotalPngBytes = Level7FramebufferDiagnostic.MaximumPlaybackBytes
				};
			}
		}
		WriteRecord(runRecord);
		writer.Flush();
	}

	public string OutputPath { get; }
	public bool IsLimitReached => limitReached;
	public bool ShouldCaptureEntityGeometry => currentFrame?.RecordFrame == true;

	public void BeginFrame(
		long renderFrameSequence,
		long simulationTick,
		long? playbackBoundary,
		int displayedLevel,
		LevelState levelState,
		bool paused,
		Camera2D playfieldCamera,
		long renderStartTimestamp,
		string runtimeInputMode = "unknown")
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		if (limitReached)
			return;
		if (currentFrame is not null)
			throw new InvalidOperationException("The prior diagnostic render frame was not completed.");

		if (displayedLevel != 7)
		{
			pendingFramebufferCapture = null;
			if (sawLevel7 && !scopeEnded)
			{
				WriteRecord(new
				{
					recordType = "levelScopeEnded",
					renderFrameSequence,
					displayedLevel,
					levelState = levelState.ToString()
				});
				scopeEnded = true;
			}
			previousSimulationTick = null;
			previousPlaybackBoundary = null;
			previousLevelState = null;
			return;
		}

		sawLevel7 = true;
		scopeEnded = false;
		pendingFramebufferCapture = null;
		bool stateTransition = previousLevelState != levelState;
		long? simulationTickDelta = previousSimulationTick is long priorSimulationTick
			? simulationTick - priorSimulationTick
			: null;
		long? playbackBoundaryDelta = playbackBoundary is long currentBoundary &&
			previousPlaybackBoundary is long priorBoundary
				? currentBoundary - priorBoundary
				: null;
		bool recordFrame = levelState == LevelState.Playing;
		if (traceOnlyPlaybackBoundaryWindows)
		{
			recordFrame = recordFrame && !paused && !stateTransition &&
				playbackBoundary is long boundary &&
				Level7FramebufferDiagnostic.PlaybackCaptureBoundaries.Contains((int)boundary) &&
				!recordedPlaybackBoundaries.Contains(boundary);
		}
		bool captureSubmittedGeometry =
			traceOnlyPlaybackBoundaryWindows && recordFrame;
		currentFrame = new(
			renderFrameSequence,
			simulationTick,
			simulationTickDelta,
			playbackBoundary,
			playbackBoundaryDelta,
			displayedLevel,
			levelState,
			paused,
			playfieldCamera,
			renderStartTimestamp,
			runtimeInputMode,
			recordFrame,
			previousLevelState,
			stateTransition,
			captureSubmittedGeometry);
		frameEntities.Clear();
	}

	public void CaptureEntity(
		object identity,
		string entityType,
		string entityState,
		int lane,
		float height,
		float projectedDepth,
		Vector2 rendererAnchor,
		float? normalizedDepth,
		Vector2? mappedAnchor,
		float? localPresentationScale,
		float? glyphScale,
		IReadOnlyList<Vector2> submittedCoordinates,
		int submittedPrimitiveCount,
		float? rotationDegrees = null)
	{
		ArgumentNullException.ThrowIfNull(identity);
		ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
		ArgumentException.ThrowIfNullOrWhiteSpace(entityState);
		ArgumentNullException.ThrowIfNull(submittedCoordinates);
		FrameContext frame = currentFrame ??
			throw new InvalidOperationException("A diagnostic entity was captured outside a frame.");
		if (!frame.RecordFrame)
			throw new InvalidOperationException(
				"Entity geometry can only be captured in a Level 7 Playing frame.");
		if (submittedCoordinates.Count == 0)
			throw new ArgumentException("Rendered entity geometry must contain submitted coordinates.",
				nameof(submittedCoordinates));
		if (submittedPrimitiveCount <= 0)
			throw new ArgumentOutOfRangeException(nameof(submittedPrimitiveCount));

		ValidateFinite(height, nameof(height));
		ValidateFinite(projectedDepth, nameof(projectedDepth));
		ValidateFinite(rendererAnchor, nameof(rendererAnchor));
		if (normalizedDepth is float depth)
			ValidateFinite(depth, nameof(normalizedDepth));
		if (mappedAnchor is Vector2 mapped)
			ValidateFinite(mapped, nameof(mappedAnchor));
		if (localPresentationScale is float localScale)
			ValidateFinite(localScale, nameof(localPresentationScale));
		if (glyphScale is float glyph)
			ValidateFinite(glyph, nameof(glyphScale));
		if (rotationDegrees is float rotation)
			ValidateFinite(rotation, nameof(rotationDegrees));

		long entityId = GetEntityId(identity);
		Bounds logicalBounds = Bounds.From(submittedCoordinates);
		Vector2[] hostCoordinates = new Vector2[submittedCoordinates.Count];
		for (int index = 0; index < submittedCoordinates.Count; index++)
		{
			ValidateFinite(submittedCoordinates[index], nameof(submittedCoordinates));
			hostCoordinates[index] = TransformToHost(submittedCoordinates[index], frame.Camera);
		}

		frameEntities.Add(new(
			identity,
			entityId,
			entityType,
			entityState,
			lane,
			height,
			projectedDepth,
			rendererAnchor,
			normalizedDepth,
			mappedAnchor,
			localPresentationScale,
			glyphScale,
			rotationDegrees,
			logicalBounds,
			Bounds.From(hostCoordinates),
			frame.CaptureSubmittedGeometry ? submittedCoordinates.ToArray() : null,
			frame.CaptureSubmittedGeometry ? hostCoordinates : null,
			submittedPrimitiveCount,
			frame));
	}

	public void CaptureFramebufferFrame(Level7FramebufferDiagnostic framebufferDiagnostic)
	{
		ArgumentNullException.ThrowIfNull(framebufferDiagnostic);
		ObjectDisposedException.ThrowIf(disposed, this);
		if (pendingFramebufferCapture is not null)
			throw new InvalidOperationException(
				"A framebuffer sample was already associated with this render frame.");

		FrameContext? frame = currentFrame;
		if (frame is null)
		{
			framebufferDiagnostic.CaptureFrame(new(
				false, false, false, false, 0, 0, 0, [],
				null, 0, "Unknown", playbackFilePath, atariPresentationEnabled));
			return;
		}

		Level7FramebufferTankerObservation[] tankers = frame.RecordFrame &&
			playbackFilePath is null
			? frameEntities
				.Where(entity => IsEligibleFramebufferTanker(entity, frame))
				.Select(CreateFramebufferTankerObservation)
				.ToArray()
			: [];
		pendingFramebufferCapture = framebufferDiagnostic.CaptureFrame(new(
			true,
			frame.LevelState == LevelState.Playing,
			frame.Paused,
			frame.StateTransition,
			frame.RenderFrameSequence,
			frame.SimulationTick,
			frame.RenderStartTimestamp,
			tankers,
			frame.PlaybackBoundary,
			frame.DisplayedLevel,
			frame.LevelState.ToString(),
			playbackFilePath,
			atariPresentationEnabled));
		if (pendingFramebufferCapture is not null &&
			frame.PlaybackBoundary is long capturedBoundary)
		{
			recordedPlaybackBoundaries.Add(capturedBoundary);
		}
	}

	public void EndFrame(long presentedCallReturnTimestamp)
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		FrameContext? frame = currentFrame;
		if (frame is null)
			return;

		double? elapsedSincePreviousPresentedCall =
			previousPresentedTimestamp == 0
				? null
				: ToSeconds(presentedCallReturnTimestamp - previousPresentedTimestamp);
		if (!frame.RecordFrame)
		{
			if (frame.StateTransition)
			{
				WriteRecord(new
				{
					recordType = "levelStateTransition",
					frame.RenderFrameSequence,
					frame.SimulationTick,
					frame.SimulationTickDeltaSincePreviousLevel7Frame,
					frame.PlaybackBoundary,
					frame.PlaybackBoundaryDeltaSincePreviousLevel7Frame,
					previousLevelState = frame.PreviousLevelState?.ToString(),
					levelState = frame.LevelState.ToString(),
					frame.RuntimeInputMode,
					frame.Paused,
					frame.RenderStartTimestamp,
					presentedCallReturnTimestamp,
					elapsedSincePreviousPresentedCall
				});
			}
			previousPresentedTimestamp = presentedCallReturnTimestamp;
			previousSimulationTick = frame.SimulationTick;
			previousPlaybackBoundary = frame.PlaybackBoundary;
			previousLevelState = frame.LevelState;
			currentFrame = null;
			pendingFramebufferCapture = null;
			return;
		}

		if (writtenFrameCount >= MaximumLevel7RenderFrames)
		{
			WriteRecord(new
			{
				recordType = "diagnosticLimitReached",
				maximumLevel7RenderFrames = MaximumLevel7RenderFrames,
				renderFrameSequence = frame.RenderFrameSequence
			});
			previousPresentedTimestamp = presentedCallReturnTimestamp;
			previousSimulationTick = frame.SimulationTick;
			previousPlaybackBoundary = frame.PlaybackBoundary;
			previousLevelState = frame.LevelState;
			currentFrame = null;
			pendingFramebufferCapture = null;
			limitReached = true;
			writer.Flush();
			return;
		}

		Dictionary<string, object?> frameRecord = new()
		{
			["recordType"] = "frame",
			["renderFrameSequence"] = frame.RenderFrameSequence,
			["simulationTick"] = frame.SimulationTick,
			["simulationTickDeltaSincePreviousLevel7Frame"] =
				frame.SimulationTickDeltaSincePreviousLevel7Frame,
			["playbackBoundary"] = frame.PlaybackBoundary,
			["playbackBoundaryDeltaSincePreviousLevel7Frame"] =
				frame.PlaybackBoundaryDeltaSincePreviousLevel7Frame,
			["displayedLevel"] = frame.DisplayedLevel,
			["levelState"] = frame.LevelState.ToString(),
			["previousLevelState"] = frame.PreviousLevelState?.ToString(),
			["stateTransition"] = frame.StateTransition,
			["runtimeInputMode"] = frame.RuntimeInputMode,
			["paused"] = frame.Paused,
			["renderStartTimestamp"] = frame.RenderStartTimestamp,
			["presentedCallReturnTimestamp"] = presentedCallReturnTimestamp,
			["elapsedSincePreviousPresentedCall"] = elapsedSincePreviousPresentedCall,
			["renderCallDurationSeconds"] = ToSeconds(
				presentedCallReturnTimestamp - frame.RenderStartTimestamp),
			["entityCount"] = frameEntities.Count
		};
		if (framebufferCaptureEnabled)
			frameRecord["atariPresentationEnabled"] = atariPresentationEnabled;
		if (pendingFramebufferCapture is not null)
			frameRecord.Add("framebufferCapture", pendingFramebufferCapture);
		WriteRecord(frameRecord);

		HashSet<object> observedIdentities = new(ReferenceEqualityComparer.Instance);
		foreach (CapturedEntity entity in frameEntities)
		{
			observedIdentities.Add(entity.Identity);
			previousEntities.TryGetValue(entity.Identity, out PreviousEntity? previous);
			WriteEntityRecord(entity, previous, presentedCallReturnTimestamp);
		}

		if (frame.LevelState == LevelState.Playing)
		{
			foreach ((object identity, PreviousEntity previous) in previousEntities.ToArray())
			{
				if (observedIdentities.Contains(identity))
					continue;

				WriteRecord(new
				{
					recordType = "entityRemoved",
					frame.RenderFrameSequence,
					frame.SimulationTick,
					entityId = previous.EntityId,
					entityType = previous.EntityType,
					previous.State,
					previous.Lane
				});
				previousEntities.Remove(identity);
				entityIds.Remove(identity);
			}
		}

		previousPresentedTimestamp = presentedCallReturnTimestamp;
		previousSimulationTick = frame.SimulationTick;
		previousPlaybackBoundary = frame.PlaybackBoundary;
		previousLevelState = frame.LevelState;
		writtenFrameCount++;
		currentFrame = null;
		pendingFramebufferCapture = null;
	}

	public void Dispose()
	{
		if (disposed)
			return;

		if (currentFrame is FrameContext incompleteFrame)
		{
			WriteRecord(new
			{
				recordType = "incompleteFrame",
				incompleteFrame.RenderFrameSequence,
				levelState = incompleteFrame.LevelState.ToString()
			});
			currentFrame = null;
		}

		WriteRecord(new
		{
			recordType = "runEnded",
			endedUtc = DateTimeOffset.UtcNow,
			level7RenderFrames = writtenFrameCount
		});
		writer.Flush();
		writer.Dispose();
		disposed = true;
	}

	private long GetEntityId(object identity)
	{
		if (entityIds.TryGetValue(identity, out long entityId))
			return entityId;

		entityId = nextEntityId++;
		entityIds.Add(identity, entityId);
		return entityId;
	}

	private void WriteEntityRecord(
		CapturedEntity entity,
		PreviousEntity? previous,
		long presentedCallReturnTimestamp)
	{
		FrameContext frame = entity.Frame;
		Vector2 hostAnchor = TransformToHost(entity.RendererAnchor, frame.Camera);
		long renderFrameDelta = previous is null
			? 0
			: frame.RenderFrameSequence - previous.RenderFrameSequence;
		long simulationTickDelta = previous is null
			? 0
			: frame.SimulationTick - previous.SimulationTick;
		double? elapsedSeconds = previous is null
			? null
			: ToSeconds(presentedCallReturnTimestamp - previous.PresentedTimestamp);
		Vector2? logicalAnchorDelta = previous is null
			? null
			: entity.RendererAnchor - previous.RendererAnchor;
		Vector2? hostAnchorDelta = previous is null
			? null
			: hostAnchor - previous.HostAnchor;
		bool fixedLaneContinuous =
			previous is not null &&
			renderFrameDelta == 1 &&
			previous.LevelState == LevelState.Playing &&
			frame.LevelState == LevelState.Playing &&
			!previous.Paused &&
			!frame.Paused &&
			previous.Lane == entity.Lane &&
			previous.State == entity.State &&
			previous.RuntimeInputMode == frame.RuntimeInputMode &&
			simulationTickDelta > 0 &&
			elapsedSeconds is > 0;
		List<string> events = [];
		if (previous is null)
			events.Add("spawn");
		else
		{
			if (renderFrameDelta != 1)
				events.Add("observationGap");
			if (previous.Lane != entity.Lane)
				events.Add("laneChanged");
			if (previous.State != entity.State)
				events.Add("stateChanged");
			if (previous.RuntimeInputMode != frame.RuntimeInputMode)
				events.Add("runtimeModeChanged");
		}
		if (events.Count == 0)
			events.Add("sample");

		double? displacementRate =
			hostAnchorDelta is Vector2 delta && elapsedSeconds is > 0
				? delta.Length() / elapsedSeconds.Value
				: null;
		Vector2? hostVelocity =
			hostAnchorDelta is Vector2 velocity && elapsedSeconds is > 0
				? velocity / (float)elapsedSeconds.Value
				: null;
		double? pixelsPerSimulationTick =
			hostAnchorDelta is Vector2 perTick && simulationTickDelta != 0
				? perTick.Length() / Math.Abs(simulationTickDelta)
				: null;

		Dictionary<string, object?> entityRecord = new()
		{
			["recordType"] = "entity",
			["frameSequence"] = frame.RenderFrameSequence,
			["simulationTick"] = frame.SimulationTick,
			["playbackBoundary"] = frame.PlaybackBoundary,
			["entityId"] = entity.EntityId,
			["entityType"] = entity.EntityType,
			["state"] = entity.State,
			["lane"] = entity.Lane,
			["height"] = entity.Height,
			["normalizedDepth"] = entity.NormalizedDepth,
			["mappedAnchor"] = entity.MappedAnchor is Vector2 mappedAnchor
				? ToPoint(mappedAnchor)
				: null,
			["hostAnchor"] = ToPoint(hostAnchor),
			["localPresentationScale"] = entity.LocalPresentationScale,
			["glyphScale"] = entity.GlyphScale,
			["rotationDegrees"] = entity.RotationDegrees,
			["logicalGeometryBounds"] = entity.LogicalGeometryBounds,
			["hostGeometryBounds"] = entity.HostGeometryBounds,
			["submittedPrimitiveCount"] = entity.SubmittedPrimitiveCount,
			["events"] = events,
			["previousRenderFrameDelta"] = previous is null ? (long?)null : renderFrameDelta,
			["previousSimulationTickDelta"] = previous is null ? (long?)null : simulationTickDelta,
			["elapsedSeconds"] = elapsedSeconds,
			["logicalAnchorDelta"] = logicalAnchorDelta is Vector2 logicalDelta
				? ToPoint(logicalDelta)
				: null,
			["hostAnchorDelta"] = hostAnchorDelta is Vector2 hostDelta
				? ToPoint(hostDelta)
				: null,
			["hostVelocityPixelsPerSecond"] = hostVelocity is Vector2 hostVelocityValue
				? ToPoint(hostVelocityValue)
				: null,
			["displacementRateHostPixelsPerSecond"] = displacementRate,
			["pixelsPerSimulationTick"] = pixelsPerSimulationTick,
			["fixedLaneContinuous"] = fixedLaneContinuous
		};
		if (framebufferCaptureEnabled)
		{
			entityRecord["projectedDepth"] = entity.ProjectedDepth;
			entityRecord["atariPresentationEnabled"] = atariPresentationEnabled;
			entityRecord["rendererAnchor"] = ToPoint(entity.RendererAnchor);
		}
		if (entity.SubmittedCoordinates is not null &&
			entity.CameraTransformedCoordinates is not null)
		{
			entityRecord["submittedCoordinates"] =
				entity.SubmittedCoordinates.Select(ToPoint);
			entityRecord["cameraTransformedCoordinates"] =
				entity.CameraTransformedCoordinates.Select(ToPoint);
		}
		WriteRecord(entityRecord);

		previousEntities[entity.Identity] = new(
			entity.EntityId,
			entity.EntityType,
			entity.State,
			entity.Lane,
			entity.Height,
			frame.RuntimeInputMode,
			frame.LevelState,
			frame.Paused,
			frame.RenderFrameSequence,
			frame.SimulationTick,
			hostAnchor,
			entity.RendererAnchor,
			presentedCallReturnTimestamp);
	}

	private bool IsEligibleFramebufferTanker(CapturedEntity entity, FrameContext frame)
	{
		if (entity.EntityType != "tanker" || entity.State != "Climbing" ||
			frame.LevelState != LevelState.Playing || frame.Paused || frame.StateTransition ||
			!previousEntities.TryGetValue(entity.Identity, out PreviousEntity? previous))
		{
			return false;
		}

		return frame.RenderFrameSequence - previous.RenderFrameSequence == 1 &&
			previous.LevelState == LevelState.Playing &&
			!previous.Paused &&
			previous.Lane == entity.Lane &&
			previous.State == entity.State &&
			previous.RuntimeInputMode == frame.RuntimeInputMode &&
			frame.SimulationTick - previous.SimulationTick > 0 &&
			frame.RenderStartTimestamp > previous.PresentedTimestamp &&
			entity.Height > previous.Height;
	}

	private Level7FramebufferTankerObservation CreateFramebufferTankerObservation(
		CapturedEntity entity)
	{
		Vector2 hostAnchor = TransformToHost(entity.RendererAnchor, entity.Frame.Camera);
		float normalizedDepth = entity.NormalizedDepth ??
			throw new InvalidOperationException(
				"Manual Atari framebuffer capture requires normalized Atari depth metadata.");
		return new(
			entity.EntityId,
			entity.Lane,
			entity.State,
			entity.Height,
			normalizedDepth,
			new(entity.RendererAnchor.X, entity.RendererAnchor.Y),
			new(hostAnchor.X, hostAnchor.Y),
			ToFramebufferBounds(entity.LogicalGeometryBounds),
			ToFramebufferBounds(entity.HostGeometryBounds),
			true);
	}

	private static Level7FramebufferBounds ToFramebufferBounds(Bounds bounds) =>
		new(bounds.MinX, bounds.MinY, bounds.MaxX, bounds.MaxY);

	private void WriteRecord<T>(T record)
	{
		writer.WriteLine(JsonSerializer.Serialize(record, JsonOptions));
	}

	private static Vector2 TransformToHost(Vector2 point, Camera2D camera)
	{
		// Mirror Raylib's camera mapping; this cannot observe rasterization or monitor scanout.
		float radians = camera.Rotation * (MathF.PI / 180);
		float cosine = MathF.Cos(radians);
		float sine = MathF.Sin(radians);
		Vector2 relative = point - camera.Target;
		Vector2 rotated = new(
			relative.X * cosine - relative.Y * sine,
			relative.X * sine + relative.Y * cosine);
		return camera.Offset + rotated * camera.Zoom;
	}

	private static double ToSeconds(long stopwatchTicks) =>
		stopwatchTicks / (double)Stopwatch.Frequency;

	private static PointValue ToPoint(Vector2 point) => new(point.X, point.Y);

	private static void ValidateFinite(float value, string parameterName)
	{
		if (!float.IsFinite(value))
			throw new ArgumentOutOfRangeException(parameterName, "Diagnostic values must be finite.");
	}

	private static void ValidateFinite(Vector2 value, string parameterName)
	{
		ValidateFinite(value.X, parameterName);
		ValidateFinite(value.Y, parameterName);
	}

	private sealed record FrameContext(
		long RenderFrameSequence,
		long SimulationTick,
		long? SimulationTickDeltaSincePreviousLevel7Frame,
		long? PlaybackBoundary,
		long? PlaybackBoundaryDeltaSincePreviousLevel7Frame,
		int DisplayedLevel,
		LevelState LevelState,
		bool Paused,
		Camera2D Camera,
		long RenderStartTimestamp,
		string RuntimeInputMode,
		bool RecordFrame,
		LevelState? PreviousLevelState,
		bool StateTransition,
		bool CaptureSubmittedGeometry);

	private sealed record CapturedEntity(
		object Identity,
		long EntityId,
		string EntityType,
		string State,
		int Lane,
		float Height,
		float ProjectedDepth,
		Vector2 RendererAnchor,
		float? NormalizedDepth,
		Vector2? MappedAnchor,
		float? LocalPresentationScale,
		float? GlyphScale,
		float? RotationDegrees,
		Bounds LogicalGeometryBounds,
		Bounds HostGeometryBounds,
		IReadOnlyList<Vector2>? SubmittedCoordinates,
		IReadOnlyList<Vector2>? CameraTransformedCoordinates,
		int SubmittedPrimitiveCount,
		FrameContext Frame);

	private sealed record PreviousEntity(
		long EntityId,
		string EntityType,
		string State,
		int Lane,
		float Height,
		string RuntimeInputMode,
		LevelState LevelState,
		bool Paused,
		long RenderFrameSequence,
		long SimulationTick,
		Vector2 HostAnchor,
		Vector2 RendererAnchor,
		long PresentedTimestamp);

	private sealed record Bounds(float MinX, float MinY, float MaxX, float MaxY)
	{
		public static Bounds From(IReadOnlyList<Vector2> points)
		{
			Vector2 first = points[0];
			float minX = first.X;
			float minY = first.Y;
			float maxX = first.X;
			float maxY = first.Y;
			for (int index = 1; index < points.Count; index++)
			{
				Vector2 point = points[index];
				minX = MathF.Min(minX, point.X);
				minY = MathF.Min(minY, point.Y);
				maxX = MathF.Max(maxX, point.X);
				maxY = MathF.Max(maxY, point.Y);
			}
			return new(minX, minY, maxX, maxY);
		}
	}

	private sealed record PointValue(float X, float Y);
}
