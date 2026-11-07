using Tempest.Game;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class ApplicationPathsTests
{
	[Fact]
	public void DevelopmentRootAndUserDataChildrenAreBasedOnTheInjectedRoot()
	{
		string temporaryDirectory = Path.Combine(Path.GetTempPath(),
			"TempestApplicationPathsTests", Guid.NewGuid().ToString("N"));
		string requestedRoot = Path.Combine(temporaryDirectory, "app", ".", "bin");
		ApplicationPaths paths = new(requestedRoot);
		string expectedRoot = Path.GetFullPath(requestedRoot);

		Assert.Equal(expectedRoot, paths.DevelopmentRoot);
		Assert.Equal(Path.Combine(expectedRoot, "UserData"), paths.UserDataRoot);
		Assert.Equal(Path.Combine(expectedRoot, "UserData", "GameplayRecordings"),
			paths.GameplayRecordingsDirectory);
		Assert.Equal(Path.Combine(expectedRoot, "UserData", "Screenshots"),
			paths.ScreenshotsDirectory);
		Assert.False(Directory.Exists(temporaryDirectory));
	}

	[Fact]
	public void CurrentDevelopmentRootUsesTheProcessWorkingDirectory()
	{
		Assert.Equal(Path.GetFullPath(Directory.GetCurrentDirectory()),
			ApplicationPaths.Current.DevelopmentRoot);
	}

	[Fact]
	public void RaylibScreenshotPathIsRelativeToDevelopmentRootAndUsesForwardSlashes()
	{
		string developmentRoot = Path.Combine(Path.GetTempPath(),
			"TempestApplicationPathsTests", Guid.NewGuid().ToString("N"));
		ApplicationPaths paths = new(developmentRoot);
		string fullDestination = Path.Combine(paths.ScreenshotsDirectory, "capture.png");

		Assert.Equal("UserData/Screenshots/capture.png",
			paths.GetRaylibRelativeScreenshotPath(fullDestination));
		Assert.False(Path.IsPathRooted(paths.GetRaylibRelativeScreenshotPath(fullDestination)));
		Assert.NotEqual(Path.GetFullPath(AppContext.BaseDirectory), paths.DevelopmentRoot);
		Assert.False(Directory.Exists(developmentRoot));
	}

	[Fact]
	public void RaylibScreenshotPathCannotEscapeTheScreenshotsDirectory()
	{
		ApplicationPaths paths = new(Path.GetTempPath());
		string outsidePath = Path.Combine(paths.UserDataRoot, "other.png");

		Assert.Throws<ArgumentException>(() =>
			paths.GetRaylibRelativeScreenshotPath(outsidePath));
	}

	[Fact]
	public void BareRecordingNamesResolveUnderDevelopmentUserData()
	{
		string developmentRoot = Path.Combine(Path.GetTempPath(),
			"TempestApplicationPathsTests", Guid.NewGuid().ToString("N"));
		ApplicationPaths paths = new(developmentRoot);

		Assert.Equal(
			Path.Combine(developmentRoot, "UserData", "GameplayRecordings", "levels1-7.json"),
			paths.ResolveGameplayRecordingPath("levels1-7.json"));
	}

	[Theory]
	[InlineData("Temp\\GameplayRecordings\\levels1-7.json")]
	[InlineData("subdirectory/levels1-7.json")]
	public void ExplicitRelativeRecordingPathsRetainTheirOriginalMeaning(string path)
	{
		ApplicationPaths paths = new(Path.GetTempPath());

		Assert.Equal(path, paths.ResolveGameplayRecordingPath(path));
	}

	[Fact]
	public void AbsoluteRecordingPathsRemainAbsoluteAndUnchanged()
	{
		string path = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "recording.json"));
		ApplicationPaths paths = new(Path.GetTempPath());

		Assert.Equal(path, paths.ResolveGameplayRecordingPath(path));
	}
}
