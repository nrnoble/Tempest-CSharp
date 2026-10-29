using Raylib_cs;

namespace Tempest.Game.Systems;

public readonly record struct PlayerInput(bool Fire, float MouseDeltaX, bool Left, bool Right);

public sealed class InputSystem
{
	public bool RestartPressed() => Raylib.IsKeyPressed(KeyboardKey.R);

	public PlayerInput ReadPlayer()
	{
		bool fireButtonDown =
			Raylib.IsKeyDown(KeyboardKey.LeftControl) ||
			Raylib.IsKeyDown(KeyboardKey.RightControl) ||
			Raylib.IsMouseButtonDown(MouseButton.Left);

		float mouseDeltaX = Raylib.GetMouseDelta().X;
		bool left = Raylib.IsKeyDown(KeyboardKey.Left);
		bool right = Raylib.IsKeyDown(KeyboardKey.Right);
		return new PlayerInput(fireButtonDown, mouseDeltaX, left, right);
	}
}
