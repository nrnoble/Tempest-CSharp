using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public sealed class PlaybackScreenshotCapture
{
	private readonly ApplicationPaths applicationPaths;
	private readonly Action<string> takeScreenshot;
	private readonly TextWriter output;
	private readonly TextWriter error;

	public string? LastRequestedPath { get; private set; }
	public string? LastRaylibPath { get; private set; }
	public long? LastFileLength { get; private set; }

	public PlaybackScreenshotCapture(
		ApplicationPaths applicationPaths,
		Action<string> takeScreenshot,
		TextWriter output,
		TextWriter error)
	{
		ArgumentNullException.ThrowIfNull(applicationPaths);
		ArgumentNullException.ThrowIfNull(takeScreenshot);
		ArgumentNullException.ThrowIfNull(output);
		ArgumentNullException.ThrowIfNull(error);

		this.applicationPaths = applicationPaths;
		this.takeScreenshot = takeScreenshot;
		this.output = output;
		this.error = error;
	}

	public static bool IsCaptureRequested(
		bool filePlaybackActive,
		bool playbackPaused,
		PlaybackScreenshotInput input) =>
		filePlaybackActive && playbackPaused && input.IsScreenshotCommand;

	public bool TryCapture(
		bool filePlaybackActive,
		bool playbackPaused,
		PlaybackScreenshotInput input,
		int displayedLevel,
		int boundary)
	{
		LastRequestedPath = null;
		LastRaylibPath = null;
		LastFileLength = null;
		if (!IsCaptureRequested(filePlaybackActive, playbackPaused, input))
			return false;

		if (displayedLevel < 1)
			throw new ArgumentOutOfRangeException(nameof(displayedLevel));
		if (boundary < 0)
			throw new ArgumentOutOfRangeException(nameof(boundary));

		string? fullDestination = null;
		string? raylibPath = null;
		try
		{
			string outputDirectory = applicationPaths.ScreenshotsDirectory;
			Directory.CreateDirectory(outputDirectory);
			fullDestination = ReserveUniquePath(outputDirectory, displayedLevel, boundary);
			LastRequestedPath = fullDestination;
			raylibPath = applicationPaths.GetRaylibRelativeScreenshotPath(fullDestination);
			LastRaylibPath = raylibPath;
			takeScreenshot(raylibPath);

			FileInfo capturedFile = new(fullDestination);
			if (!capturedFile.Exists)
				throw new IOException("Raylib did not write a non-empty PNG file.");
			LastFileLength = capturedFile.Length;
			if (LastFileLength == 0)
				throw new IOException("Raylib did not write a non-empty PNG file.");
		}
		catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or
			ArgumentException or NotSupportedException or System.Security.SecurityException)
		{
			string cleanupFailure = DeleteReservation(fullDestination);
			error.WriteLine(
				$"Could not capture playback screenshot. Raylib-relative path: " +
				$"{raylibPath ?? "(not generated)"}; expected full destination: " +
				$"{fullDestination ?? "(not reserved)"}. {exception.Message}{cleanupFailure}");
			return false;
		}

		output.WriteLine($"Screenshot saved: {fullDestination}");
		return true;
	}

	private static string ReserveUniquePath(string outputDirectory, int displayedLevel, int boundary)
	{
		string stem = FormattableString.Invariant(
			$"playback-level-{displayedLevel}-boundary-{boundary:D6}");
		for (int sequence = 1; ; sequence++)
		{
			string suffix = sequence == 1
				? string.Empty
				: FormattableString.Invariant($"-{sequence:D3}");
			string path = Path.Combine(outputDirectory, $"{stem}{suffix}.png");
			try
			{
				using FileStream reservation = new(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
				return path;
			}
			catch (IOException) when (File.Exists(path))
			{
				// CreateNew makes the name reservation atomic, including across game processes.
			}
		}
	}

	private static string DeleteReservation(string? path)
	{
		if (path is null)
			return string.Empty;

		try
		{
			File.Delete(path);
			return string.Empty;
		}
		catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
		{
			return $" The incomplete file could not be removed: {exception.Message}";
		}
	}
}
