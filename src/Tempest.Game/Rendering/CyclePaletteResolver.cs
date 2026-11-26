using Raylib_cs;

namespace Tempest.Game.Rendering;

public static class CyclePaletteResolver
{
	public static readonly Color Cyan = new(0, 255, 255, 255);

	public static int ResolvePaletteIndex(int displayedLevel)
	{
		if (displayedLevel < 1)
			throw new ArgumentOutOfRangeException(nameof(displayedLevel),
				"Tempest displayed levels start at 1.");
		if (displayedLevel > 32)
			throw new ArgumentOutOfRangeException(nameof(displayedLevel),
				"Only bounded Levels 1-32 cycle palettes are supported.");
		return (displayedLevel - 1) / 16;
	}

	public static Color ResolveTubeColor(int displayedLevel) =>
		ResolvePaletteIndex(displayedLevel) switch
		{
			0 => Color.Blue,
			1 => Color.Red,
			_ => throw new InvalidOperationException("Unsupported Tempest palette.")
		};

	public static Color ResolveBlasterLaneColor(int displayedLevel) =>
		new(255, 255, 0, 255);

	public static Color ResolveFlipperColor(int displayedLevel) =>
		ResolvePaletteIndex(displayedLevel) switch
		{
			0 => Color.Red,
			1 => Color.Magenta,
			_ => throw new InvalidOperationException("Unsupported Tempest palette.")
		};

	public static Color ResolvePulsarColor(int displayedLevel) =>
		new(0, 255, 255, 255);

	public static Color ResolveTankerColor(int displayedLevel) =>
		ResolvePaletteIndex(displayedLevel) switch
		{
			0 => Color.Magenta,
			1 => Color.Blue,
			_ => throw new InvalidOperationException("Unsupported Tempest palette.")
		};

	public static Color ResolveEggColor(int displayedLevel) =>
		ResolvePaletteIndex(displayedLevel) switch
		{
			0 => Color.Red,
			1 => Color.Yellow,
			_ => throw new InvalidOperationException("Unsupported Tempest palette.")
		};
}
