using System.Numerics;
using Raylib_cs;

namespace Tempest.Game.Rendering;

public static class DisplayConfiguration
{
	public const int LogicalWidth = 800;
	public const int LogicalHeight = 600;
	public const float WindowScale = 1.6f;
	public const int WindowWidth = (int)(LogicalWidth * WindowScale);
	public const int WindowHeight = (int)(LogicalHeight * WindowScale);

	// All drawing stays in the logical 4:3 canvas; one camera scale grows the full presentation.
	public static Camera2D PresentationCamera => new()
	{
		Target = Vector2.Zero,
		Offset = Vector2.Zero,
		Rotation = 0,
		Zoom = WindowScale
	};
}
