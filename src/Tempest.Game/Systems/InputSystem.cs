using Raylib_cs;

namespace Tempest.Game.Systems;

public readonly record struct PlayerInput(
	bool Fire, float MouseDeltaX, bool Left, bool Right, bool ZapPressed = false,
	float ControllerAxis = 0);

public sealed class InputSystem
{
	// Avoid drift from a centered left stick; keyboard and spinner movement are unchanged.
	private const float ControllerDeadZone = 0.15f;

	public bool RestartPressed() => IsRestartPressed(
		Raylib.IsKeyPressed(KeyboardKey.R), Raylib.IsKeyPressed(KeyboardKey.P));

	public bool QuitRequested() => IsQuitRequested(
		Raylib.WindowShouldClose(), Raylib.IsKeyPressed(KeyboardKey.Enter));

	public static bool IsRestartPressed(bool rPressed, bool pPressed) =>
		rPressed || pPressed;

	public static bool IsQuitRequested(bool windowShouldClose, bool enterPressed) =>
		windowShouldClose || enterPressed;

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
		float controllerAxis = Raylib.IsGamepadAvailable(0)
			? Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftX)
			: 0;
		if (MathF.Abs(controllerAxis) < ControllerDeadZone)
			controllerAxis = 0;
		bool zapPressed = IsSuperZapperPressed(
			Raylib.IsKeyPressed(KeyboardKey.Space),
			Raylib.IsMouseButtonPressed(MouseButton.Right));
		return new PlayerInput(fireButtonDown, mouseDeltaX, left, right, zapPressed, controllerAxis);
	}
}
