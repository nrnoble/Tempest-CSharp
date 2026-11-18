using System.Diagnostics;
using Raylib_cs;
using Tempest.Game.Rendering;

namespace Tempest.Game.Debug;

internal sealed record Level7FramebufferPoint(float X, float Y);

internal sealed record Level7FramebufferBounds(float MinX, float MinY, float MaxX, float MaxY);

internal sealed record Level7FramebufferTankerObservation(
	long EntityId,
	int Lane,
	string State,
	float Height,
	float NormalizedDepth,
	Level7FramebufferPoint MappedAnchor,
	Level7FramebufferPoint HostAnchor,
	Level7FramebufferBounds LogicalGeometryBounds,
	Level7FramebufferBounds HostGeometryBounds,
	bool FixedLaneContinuous);

internal sealed record Level7FramebufferFrameObservation(
	bool IsLevel7,
	bool IsPlaying,
	bool Paused,
	bool StateTransition,
	long RenderFrameSequence,
	long SimulationTick,
	long RenderStartTimestamp,
	IReadOnlyList<Level7FramebufferTankerObservation> Tankers,
	long? PlaybackBoundary,
	int DisplayedLevel,
	string LevelState,
	string? PlaybackFilePath,
	bool AtariPresentationEnabled);

internal sealed record Level7FramebufferCaptureRecord(
	int? BurstNumber,
	string? DepthBand,
	int? FrameInBurst,
	string FileName,
	string OutputDirectory,
	string PresentationMode,
	string? PlaybackFilePath,
	long? PlaybackBoundary,
	int DisplayedLevel,
	string LevelState,
	long RenderFrameSequence,
	long SimulationTick,
	long RenderStartTimestamp,
	Level7FramebufferTankerObservation? Tanker,
	int ImageWidth,
	int ImageHeight,
	string ImageFormat,
	long PngLengthBytes,
	double ReadbackDurationMilliseconds,
	double PngWriteDurationMilliseconds,
	double CaptureDurationMilliseconds);

public sealed class Level7FramebufferDiagnostic : IDisposable
{
	public const int MaximumBursts = 3;
	public const int MaximumFramesPerBurst = 8;
	public const int MaximumTotalFrames = MaximumBursts * MaximumFramesPerBurst;
	public const long MaximumPngFileBytes = 8 * 1024 * 1024;
	public const long MaximumTotalBytes = 128 * 1024 * 1024;
	public const int PlaybackFramesPerBoundaryWindow = 3;
	public const int MaximumPlaybackFrames = 15;
	public const long MaximumPlaybackBytes = MaximumPlaybackFrames * MaximumPngFileBytes;

	private static readonly string[] DepthBands = ["deep", "middle", "nearRim"];
	private static readonly int[] PlaybackWindowCenters = [100, 150, 200, 250, 300];
	private static readonly int[] PlaybackBoundaries =
		[99, 100, 101, 149, 150, 151, 199, 200, 201, 249, 250, 251, 299, 300, 301];

	private readonly string outputDirectory;
	private readonly string? playbackFilePath;
	private readonly bool atariPresentationEnabled;
	private readonly bool captureByPlaybackBoundary;
	private readonly Func<Image> readFramebuffer;
	private readonly Func<Image, string, long> writePng;
	private readonly Action<Image> unloadImage;
	private readonly TextWriter output;
	private readonly TextWriter error;
	private readonly HashSet<string> attemptedDepthBands = new(StringComparer.Ordinal);
	private readonly HashSet<int> capturedPlaybackBoundaries = [];
	private readonly HashSet<int> missedPlaybackBoundaries = [];
	private Burst? activeBurst;
	private int completedBurstCount;
	private int nextPlaybackBoundaryIndex;
	private int totalFrameCount;
	private long totalBytes;
	private bool stopped;
	private bool statusReported;
	private bool disposed;

	public static string GetOutputDirectory(string outputRootDirectory, string outputFileName)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(outputRootDirectory);
		ArgumentException.ThrowIfNullOrWhiteSpace(outputFileName);
		ValidateOutputFileName(outputFileName);
		return Path.Combine(
			Path.GetFullPath(outputRootDirectory),
			Path.GetFileNameWithoutExtension(outputFileName));
	}

	public Level7FramebufferDiagnostic(
		string outputRootDirectory,
		string outputFileName,
		TextWriter output,
		TextWriter error,
		Func<Image>? readFramebuffer = null,
		Func<Image, string, long>? writePng = null,
		Action<Image>? unloadImage = null,
		string? playbackFilePath = null,
		bool atariPresentationEnabled = false)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(outputRootDirectory);
		ArgumentException.ThrowIfNullOrWhiteSpace(outputFileName);
		ArgumentNullException.ThrowIfNull(output);
		ArgumentNullException.ThrowIfNull(error);
		outputDirectory = GetOutputDirectory(outputRootDirectory, outputFileName);
		if (Directory.Exists(outputDirectory) || File.Exists(outputDirectory))
			throw new IOException($"Framebuffer diagnostic output already exists: {outputDirectory}");

		Directory.CreateDirectory(Path.GetDirectoryName(outputDirectory)!);
		Directory.CreateDirectory(outputDirectory);
		OutputDirectory = outputDirectory;
		this.playbackFilePath = playbackFilePath;
		this.atariPresentationEnabled = atariPresentationEnabled;
		captureByPlaybackBoundary = playbackFilePath is not null;
		this.output = output;
		this.error = error;
		this.readFramebuffer = readFramebuffer ?? Raylib.LoadImageFromScreen;
		this.writePng = writePng ?? ExportPng;
		this.unloadImage = unloadImage ?? Raylib.UnloadImage;
	}

	public string OutputDirectory { get; }
	public bool IsStopped => stopped;
	public int CapturedFrameCount => totalFrameCount;
	public int CompletedBurstCount => completedBurstCount;
	public long CapturedBytes => totalBytes;
	public static IReadOnlyList<int> PlaybackCaptureWindowCenters { get; } =
		Array.AsReadOnly(PlaybackWindowCenters);
	public static IReadOnlyList<int> PlaybackCaptureBoundaries { get; } =
		Array.AsReadOnly(PlaybackBoundaries);

	internal Level7FramebufferCaptureRecord? CaptureFrame(
		Level7FramebufferFrameObservation frame)
	{
		ObjectDisposedException.ThrowIf(disposed, this);
		ArgumentNullException.ThrowIfNull(frame);
		if (stopped)
			return null;
		if (frame.AtariPresentationEnabled != atariPresentationEnabled ||
			!string.Equals(frame.PlaybackFilePath, playbackFilePath, StringComparison.Ordinal))
		{
			throw new InvalidOperationException(
				"The framebuffer capture configuration does not match the rendered presentation or playback file.");
		}

		if (captureByPlaybackBoundary)
			return CapturePlaybackBoundaryFrame(frame);

		return CaptureDepthBandFrame(frame);
	}

	private Level7FramebufferCaptureRecord? CapturePlaybackBoundaryFrame(
		Level7FramebufferFrameObservation frame)
	{
		if (frame.PlaybackBoundary is not long playbackBoundary)
			return null;

		while (nextPlaybackBoundaryIndex < PlaybackBoundaries.Length &&
			PlaybackBoundaries[nextPlaybackBoundaryIndex] < playbackBoundary)
		{
			missedPlaybackBoundaries.Add(PlaybackBoundaries[nextPlaybackBoundaryIndex]);
			nextPlaybackBoundaryIndex++;
		}

		if (nextPlaybackBoundaryIndex == PlaybackBoundaries.Length)
		{
			FinishPlaybackCapture();
			return null;
		}

		if (PlaybackBoundaries[nextPlaybackBoundaryIndex] != playbackBoundary ||
			!frame.IsLevel7 || !frame.IsPlaying || frame.Paused || frame.StateTransition)
		{
			return null;
		}

		Level7FramebufferCaptureRecord? capture = Capture(frame, null, null);
		if (capture is null)
			return null;

		capturedPlaybackBoundaries.Add((int)playbackBoundary);
		nextPlaybackBoundaryIndex++;
		if (nextPlaybackBoundaryIndex == PlaybackBoundaries.Length)
			FinishPlaybackCapture();
		return capture;
	}

	private void FinishPlaybackCapture()
	{
		bool complete = missedPlaybackBoundaries.Count == 0 &&
			capturedPlaybackBoundaries.Count == MaximumPlaybackFrames;
		Finish(
			complete
				? "all five selected playback-boundary windows were captured"
				: $"playback-boundary capture ended with {missedPlaybackBoundaries.Count} missed boundary frames",
			complete);
	}

	private Level7FramebufferCaptureRecord? CaptureDepthBandFrame(
		Level7FramebufferFrameObservation frame)
	{
		if (!frame.IsLevel7 || !frame.IsPlaying || frame.Paused || frame.StateTransition)
		{
			if (activeBurst is not null)
			{
				activeBurst = null;
				if (attemptedDepthBands.Count == MaximumBursts)
					Finish("the final burst was interrupted", completed: false);
			}
			else if (attemptedDepthBands.Count == MaximumBursts)
			{
				Finish("all depth-band bursts have been attempted", completed: false);
			}
			return null;
		}

		string? selectedBand = null;
		Level7FramebufferTankerObservation? selectedTanker = null;
		if (activeBurst is Burst active)
		{
			selectedTanker = frame.Tankers.FirstOrDefault(tanker =>
				tanker.EntityId == active.EntityId &&
				tanker.FixedLaneContinuous &&
				tanker.State == "Climbing" &&
				IsInDepthBand(tanker.NormalizedDepth, active.DepthBand));
			if (selectedTanker is null)
			{
				activeBurst = null;
				if (attemptedDepthBands.Count == MaximumBursts)
					Finish("the final burst lost its continuous Tanker trajectory", completed: false);
				return null;
			}
			selectedBand = active.DepthBand;
		}
		else
		{
			selectedBand = DepthBands.FirstOrDefault(band => !attemptedDepthBands.Contains(band));
			if (selectedBand is null)
			{
				Finish("all depth-band bursts have been attempted", completed: false);
				return null;
			}

			selectedTanker = frame.Tankers.FirstOrDefault(tanker =>
				tanker.FixedLaneContinuous &&
				tanker.State == "Climbing" &&
				IsInDepthBand(tanker.NormalizedDepth, selectedBand));
			if (selectedTanker is null)
				return null;

			attemptedDepthBands.Add(selectedBand);
			activeBurst = new(attemptedDepthBands.Count, selectedBand, selectedTanker.EntityId);
		}

		return Capture(frame, selectedTanker, selectedBand);
	}

	public void Dispose()
	{
		if (disposed)
			return;

		if (!statusReported)
		{
			if (captureByPlaybackBoundary)
			{
				for (int index = nextPlaybackBoundaryIndex;
					index < PlaybackBoundaries.Length;
					index++)
				{
					missedPlaybackBoundaries.Add(PlaybackBoundaries[index]);
				}
			}
			ReportStatus(
				captureByPlaybackBoundary
					? "run ended before all selected playback boundaries were captured"
					: "run ended before all depth-band bursts were captured",
				completed: false);
		}
		disposed = true;
	}

	private Level7FramebufferCaptureRecord? Capture(
		Level7FramebufferFrameObservation frame,
		Level7FramebufferTankerObservation? tanker,
		string? depthBand)
	{
		Burst? burst = activeBurst;
		if (!captureByPlaybackBoundary && burst is null)
			throw new InvalidOperationException("A framebuffer sample has no active burst.");
		string fileName;
		if (captureByPlaybackBoundary)
		{
			if (frame.PlaybackBoundary is not long playbackBoundary)
				throw new InvalidOperationException("A playback capture has no playback boundary.");
			fileName = FormattableString.Invariant(
				$"boundary-{playbackBoundary:D8}-frame-{frame.RenderFrameSequence:D8}.png");
		}
		else
		{
			string manualCaptureBand = depthBand ??
				throw new InvalidOperationException("A manual capture has no depth band.");
			fileName = FormattableString.Invariant(
				$"{manualCaptureBand}-burst-{burst!.Number:D2}-frame-{frame.RenderFrameSequence:D8}.png");
		}
		string? destinationPath = null;
		Image image = default;
		bool imageLoaded = false;
		long captureStart = Stopwatch.GetTimestamp();
		long readbackStart = captureStart;
		long readbackEnd = captureStart;
		long writeStart = captureStart;
		long writeEnd = captureStart;

		try
		{
			string destination = ReserveDestination(fileName);
			destinationPath = destination;
			image = readFramebuffer();
			imageLoaded = true;
			readbackEnd = Stopwatch.GetTimestamp();
			if (image.Width <= 0 || image.Height <= 0)
				throw new IOException("Raylib returned an invalid framebuffer image size.");

			writeStart = Stopwatch.GetTimestamp();
			long pngLength = writePng(image, destination);
			writeEnd = Stopwatch.GetTimestamp();
			if (pngLength <= 0 || !File.Exists(destination))
				throw new IOException("Raylib did not write a non-empty framebuffer PNG.");
			long maximumTotalBytes = captureByPlaybackBoundary
				? MaximumPlaybackBytes
				: MaximumTotalBytes;
			if (pngLength > MaximumPngFileBytes ||
				totalBytes > maximumTotalBytes - pngLength)
			{
				DeleteIncompleteFile(destinationPath);
				StopForFailure(
					$"The framebuffer PNG disk limit was reached; no further frames will be captured. " +
					$"File limit: {MaximumPngFileBytes} bytes; run limit: {maximumTotalBytes} bytes.");
				return null;
			}

			totalBytes += pngLength;
			totalFrameCount++;
			if (burst is not null)
				burst.FrameCount++;
			Level7FramebufferCaptureRecord captureRecord = new(
				burst?.Number,
				depthBand,
				burst?.FrameCount,
				fileName,
				outputDirectory,
				frame.AtariPresentationEnabled ? "Atari ON" : "Atari OFF",
				frame.PlaybackFilePath ?? playbackFilePath,
				frame.PlaybackBoundary,
				frame.DisplayedLevel,
				frame.LevelState,
				frame.RenderFrameSequence,
				frame.SimulationTick,
				frame.RenderStartTimestamp,
				tanker,
				image.Width,
				image.Height,
				"PNG",
				pngLength,
				ToMilliseconds(readbackEnd - readbackStart),
				ToMilliseconds(writeEnd - writeStart),
				ToMilliseconds(writeEnd - captureStart));

			if (burst is not null && burst.FrameCount == MaximumFramesPerBurst)
			{
				completedBurstCount++;
				activeBurst = null;
				if (completedBurstCount == MaximumBursts ||
					totalFrameCount == MaximumTotalFrames)
					Finish("all three depth-band bursts were captured", completed: true);
			}
			return captureRecord;
		}
		catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or
			ArgumentException or NotSupportedException or System.Security.SecurityException or
			InvalidOperationException)
		{
			DeleteIncompleteFile(destinationPath);
			StopForFailure($"Framebuffer capture failed: {exception.Message}");
			return null;
		}
		finally
		{
			if (imageLoaded)
				unloadImage(image);
		}
	}

	private string ReserveDestination(string fileName)
	{
		string destination = Path.Combine(outputDirectory, fileName);
		using FileStream reservation = new(
			destination, FileMode.CreateNew, FileAccess.Write, FileShare.None);
		return destination;
	}

	private static long ExportPng(Image image, string destination)
	{
		if (!Raylib.ExportImage(image, destination))
			throw new IOException("Raylib could not export the framebuffer image as PNG.");
		return new FileInfo(destination).Length;
	}

	private void DeleteIncompleteFile(string? destination)
	{
		if (destination is null)
			return;

		try
		{
			File.Delete(destination);
		}
		catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
		{
			error.WriteLine(
				$"Could not remove incomplete framebuffer capture {destination}: {exception.Message}");
		}
	}

	private void StopForFailure(string message)
	{
		error.WriteLine(message);
		stopped = true;
		activeBurst = null;
		ReportStatus("capture stopped after an explicit error", completed: false);
	}

	private void Finish(string reason, bool completed)
	{
		stopped = true;
		ReportStatus(reason, completed);
	}

	private void ReportStatus(string reason, bool completed)
	{
		if (statusReported)
			return;

		string result = completed
			? "capture complete"
			: "capture stopped or incomplete";
		string progress = captureByPlaybackBoundary
			? $"{capturedPlaybackBoundaries.Count}/{MaximumPlaybackFrames} frames, " +
				$"{totalBytes}/{MaximumPlaybackBytes} bytes; " +
				$"missed playback boundaries: " +
				$"{(missedPlaybackBoundaries.Count == 0 ? "none" : string.Join(", ", missedPlaybackBoundaries.Order()))}"
			: $"{completedBurstCount}/{MaximumBursts} bursts completed, " +
				$"{totalFrameCount}/{MaximumTotalFrames} frames, {totalBytes} bytes";
		output.WriteLine(
			$"Level 7 framebuffer diagnostic {result}: {progress}. {reason}. " +
			$"No further framebuffer readback will occur; gameplay may continue. " +
			$"Samples: {OutputDirectory}");
		statusReported = true;
	}

	private static bool IsInDepthBand(float depth, string band) =>
		band switch
		{
			"deep" => depth >= 0.8f && depth <= 1f,
			"middle" => depth > 0.2f && depth < 0.8f,
			"nearRim" => depth >= 0f && depth <= 0.2f,
			_ => false
		};

	private static double ToMilliseconds(long stopwatchTicks) =>
		stopwatchTicks * 1000d / Stopwatch.Frequency;

	private static void ValidateOutputFileName(string outputFileName)
	{
		if (!string.Equals(Path.GetFileName(outputFileName), outputFileName,
				StringComparison.Ordinal) ||
			!string.Equals(Path.GetExtension(outputFileName), ".jsonl",
				StringComparison.OrdinalIgnoreCase) ||
			outputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
		{
			throw new ArgumentException(
				"Framebuffer diagnostic output must be a .jsonl filename without a directory path.",
				nameof(outputFileName));
		}
	}

	private sealed class Burst(int number, string depthBand, long entityId)
	{
		public int Number { get; } = number;
		public string DepthBand { get; } = depthBand;
		public long EntityId { get; } = entityId;
		public int FrameCount { get; set; }
	}
}
