namespace Tempest.Game;

public sealed class ApplicationPaths
{
	public static ApplicationPaths Current { get; } = new(Directory.GetCurrentDirectory());

	public ApplicationPaths(string developmentRoot)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(developmentRoot);
		DevelopmentRoot = Path.GetFullPath(developmentRoot);
	}

	public string DevelopmentRoot { get; }
	public string UserDataRoot => Path.Combine(DevelopmentRoot, "UserData");
	public string GameplayRecordingsDirectory =>
		Path.Combine(UserDataRoot, "GameplayRecordings");
	public string ScreenshotsDirectory => Path.Combine(UserDataRoot, "Screenshots");

	public string GetRaylibRelativeScreenshotPath(string fullDestination)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fullDestination);
		string fullPath = Path.GetFullPath(fullDestination);
		string relativeToScreenshots = Path.GetRelativePath(ScreenshotsDirectory, fullPath);
		if (relativeToScreenshots == "." || IsOutsideRoot(relativeToScreenshots))
			throw new ArgumentException(
				"The screenshot destination must remain beneath UserData\\Screenshots.",
				nameof(fullDestination));

		// Raylib 6 prefixes the CWD to absolute Windows paths; pass its relative equivalent instead.
		string relativePath = Path.GetRelativePath(DevelopmentRoot, fullPath);
		if (Path.IsPathRooted(relativePath))
			throw new ArgumentException(
				"The Raylib screenshot path must be relative to the development root.",
				nameof(fullDestination));

		return relativePath
			.Replace(Path.DirectorySeparatorChar, '/')
			.Replace(Path.AltDirectorySeparatorChar, '/');
	}

	public string ResolveGameplayRecordingPath(string path)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(path);

		// Only bare filenames use development UserData; explicit relative paths retain CWD semantics.
		return !Path.IsPathRooted(path) &&
			string.IsNullOrEmpty(Path.GetDirectoryName(path))
			? Path.Combine(GameplayRecordingsDirectory, path)
			: path;
	}

	private static bool IsOutsideRoot(string relativePath) =>
		Path.IsPathRooted(relativePath) ||
		relativePath == ".." ||
		relativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal) ||
		relativePath.StartsWith($"..{Path.AltDirectorySeparatorChar}", StringComparison.Ordinal);
}
