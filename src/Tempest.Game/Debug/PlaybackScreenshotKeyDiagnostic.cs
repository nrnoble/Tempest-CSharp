namespace Tempest.Game.Debug;

public readonly record struct NativeScreenshotSnapshot(
	string Path, long Length, DateTime LastWriteTimeUtc);

public sealed class PlaybackScreenshotKeyDiagnostic
{
	// A short observation window distinguishes chord input from Raylib's native F12 capture.
	private const int ObservationFrameCount = 18;

	private readonly Func<IReadOnlyCollection<NativeScreenshotSnapshot>> enumerateNativeScreenshots;
	private readonly Dictionary<string, NativeScreenshotSnapshot> initialNativeScreenshots;
	private readonly TextWriter output;
	private int observationFramesRemaining;
	private int sPressCount;
	private int f12PressCount;
	private bool controlObserved;
	private bool shiftObserved;
	private bool sPressObserved;
	private bool f12PressObserved;
	private bool projectCaptureRequested;
	private string? projectRaylibDestination;
	private string? projectDestination;
	private bool? projectFileExists;
	private long? projectFileLength;
	private bool observationStarted;
	private bool hasReported;

	public PlaybackScreenshotKeyDiagnostic(
		string workingDirectory,
		TextWriter output)
		: this(() => EnumerateNativeScreenshots(workingDirectory), output)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(workingDirectory);
	}

	public PlaybackScreenshotKeyDiagnostic(
		Func<IReadOnlyCollection<NativeScreenshotSnapshot>> enumerateNativeScreenshots,
		TextWriter output)
	{
		ArgumentNullException.ThrowIfNull(enumerateNativeScreenshots);
		ArgumentNullException.ThrowIfNull(output);

		this.enumerateNativeScreenshots = enumerateNativeScreenshots;
		this.output = output;
		initialNativeScreenshots = enumerateNativeScreenshots()
			.ToDictionary(file => file.Path, StringComparer.OrdinalIgnoreCase);
	}

	public bool HasReported => hasReported;

	public void Observe(
		bool controlDown,
		bool shiftDown,
		bool sPressed,
		bool f12Pressed)
	{
		if (hasReported)
			return;

		if (!observationStarted)
		{
			if (!(sPressed || f12Pressed))
				return;

			observationStarted = true;
			observationFramesRemaining = ObservationFrameCount;
		}

		controlObserved |= controlDown;
		shiftObserved |= shiftDown;
		sPressObserved |= sPressed;
		f12PressObserved |= f12Pressed;
		if (sPressed)
			sPressCount++;
		if (f12Pressed)
			f12PressCount++;
		observationFramesRemaining--;
		if (observationFramesRemaining == 0)
			WriteReport();
	}

	public void RecordProjectCapture(
		string? raylibDestination,
		string? expectedFullDestination,
		bool fileExists,
		long? fileLength)
	{
		projectCaptureRequested = true;
		projectRaylibDestination = raylibDestination;
		projectDestination = expectedFullDestination;
		projectFileExists = fileExists;
		projectFileLength = fileLength;
	}

	private void WriteReport()
	{
		NativeScreenshotSnapshot[] changedNativeScreenshots = enumerateNativeScreenshots()
			.Where(file => !initialNativeScreenshots.TryGetValue(file.Path, out NativeScreenshotSnapshot initial) ||
				initial.Length != file.Length || initial.LastWriteTimeUtc != file.LastWriteTimeUtc)
			.OrderBy(file => file.Path, StringComparer.OrdinalIgnoreCase).ToArray();
		output.WriteLine(
			$"Screenshot chord diagnostic ({ObservationFrameCount} frames): " +
			$"Control observed={controlObserved}, Shift observed={shiftObserved}, " +
			$"S press edge observed={sPressObserved} ({sPressCount}), " +
			$"F12 press edge observed={f12PressObserved} ({f12PressCount}).");
		output.WriteLine($"Project capture request created: {projectCaptureRequested}.");
		output.WriteLine(
			$"Raylib-relative path passed to TakeScreenshot: {projectRaylibDestination ?? "(none)"}.");
		output.WriteLine($"Expected full destination: {projectDestination ?? "(none)"}.");
		output.WriteLine(
			$"Project screenshot file exists: {projectFileExists?.ToString() ?? "not requested"}.");
		output.WriteLine(
			$"Project screenshot file length: {projectFileLength?.ToString() ?? "not available"} bytes.");
		output.WriteLine(changedNativeScreenshots.Length == 0
			? "New or overwritten native screenshotNNN.png files since diagnostic start: none observed."
			: $"New or overwritten native screenshotNNN.png files since diagnostic start: " +
				$"{string.Join(", ", changedNativeScreenshots.Select(file => file.Path))}.");

		hasReported = true;
	}

	private static IReadOnlyCollection<NativeScreenshotSnapshot> EnumerateNativeScreenshots(
		string workingDirectory)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(workingDirectory);
		string fullDirectory = Path.GetFullPath(workingDirectory);
		return Directory.EnumerateFiles(fullDirectory, "screenshot*.png")
			.Where(path => IsNativeScreenshotName(Path.GetFileName(path)))
			.Select(path =>
			{
				FileInfo file = new(path);
				return new NativeScreenshotSnapshot(
					Path.GetFullPath(path), file.Length, file.LastWriteTimeUtc);
			})
			.ToArray();
	}

	private static bool IsNativeScreenshotName(string fileName)
	{
		const string prefix = "screenshot";
		const string extension = ".png";
		if (!fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
			!fileName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
			return false;

		ReadOnlySpan<char> digits = fileName.AsSpan(prefix.Length,
			fileName.Length - prefix.Length - extension.Length);
		return digits.Length > 0 && digits.IndexOfAnyExceptInRange('0', '9') < 0;
	}
}
