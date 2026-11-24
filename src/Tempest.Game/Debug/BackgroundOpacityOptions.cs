using Raylib_cs;

namespace Tempest.Game.Debug;

public readonly record struct BackgroundOpacityOptions(
	float BackgroundOpacity, bool WasExplicitlySpecified)
{
	public const float DefaultOpacity = 1f;

	public bool RequiresTransparentFramebuffer => BackgroundOpacity < DefaultOpacity;

	public Color BackgroundClearColor => new(0f, 0f, 0f, BackgroundOpacity);

	public void ConfigureBeforeWindowInitialization(Action<ConfigFlags> setConfigFlags)
	{
		ArgumentNullException.ThrowIfNull(setConfigFlags);
		if (RequiresTransparentFramebuffer)
			setConfigFlags(ConfigFlags.TransparentWindow);
	}
}
