using Raylib_cs;

namespace Tempest.Game.Systems;

public readonly record struct PlayerInput(
	bool Fire, float MouseDeltaX, bool Left, bool Right, bool ZapPressed = false);

public sealed class InputSystem
{
	public bool RestartPressed() => Raylib.IsKeyPressed(KeyboardKey.R);

	public static bool IsSuperZapperPressed(bool spacePressed, bool rightMousePressed) =>
		spacePressed || rightMousePressed;

	public PlayerInput ReadPlayer()
	{
		bool fireButtonDown =
			Raylib.IsKeyDown(KeyboardKey.LeftControl) ||
			Raylib.IsKeyDown(KeyboardKey.RightControl) ||
			Raylib.IsMouseButtonDown(MouseButton.Left);

		float mouseDeltaX = Raylib.GetMouseDelta().X;
		bool left = Raylib.IsKeyDown(KeyboardKey.Left);
		bool right = Raylib.IsKeyDown(KeyboardKey.Right);
		bool zapPressed = IsSuperZapperPressed(
			Raylib.IsKeyPressed(KeyboardKey.Space),
			Raylib.IsMouseButtonPressed(MouseButton.Right));
		return new PlayerInput(fireButtonDown, mouseDeltaX, left, right, zapPressed);
	}
}
