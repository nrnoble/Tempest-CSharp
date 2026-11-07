using Raylib_cs;

namespace Tempest.Game.Systems;

public readonly record struct PlayerInput(
	bool Fire, float MouseDeltaX, bool Left, bool Right, bool ZapPressed = false,
	float ControllerAxis = 0);

public readonly record struct PlaybackControlInput(
	bool PauseTogglePressed, bool ForwardNavigationPressed, bool ShiftDown,
	bool BackwardNavigationPressed = false);

public enum QuitReason
{
	WindowShouldClose,
	Enter
}

public sealed class InputSystem
{
	// Avoid drift from a centered left stick; keyboard and spinner movement are unchanged.
	private const float ControllerDeadZone = 0.15f;

	public bool RestartPressed() => IsRestartPressed(
		Raylib.IsKeyPressed(KeyboardKey.R), Raylib.IsKeyPressed(KeyboardKey.P));

	public PlaybackControlInput ReadPlaybackControls()
	{
		// Native repeat events are one navigation request, not held-key polling.
		return CreatePlaybackControlInput(
			Raylib.IsKeyPressed(KeyboardKey.Space),
			Raylib.IsKeyPressed(KeyboardKey.Right),
			Raylib.IsKeyPressedRepeat(KeyboardKey.Right),
			Raylib.IsKeyDown(KeyboardKey.LeftShift) ||
				Raylib.IsKeyDown(KeyboardKey.RightShift),
			Raylib.IsKeyPressed(KeyboardKey.Left),
			Raylib.IsKeyPressedRepeat(KeyboardKey.Left));
	}

	public static PlaybackControlInput CreatePlaybackControlInput(
		bool spacePressed, bool rightArrowPressed, bool rightArrowRepeated, bool shiftDown,
		bool leftArrowPressed = false, bool leftArrowRepeated = false) =>
		new(spacePressed, rightArrowPressed || rightArrowRepeated, shiftDown,
			leftArrowPressed || leftArrowRepeated);

	public QuitReason? ReadQuitReason()
	{
		// Preserve the original short-circuit order: don't poll Enter after Raylib requests close.
		if (Raylib.WindowShouldClose())
			return QuitReason.WindowShouldClose;

		return Raylib.IsKeyPressed(KeyboardKey.Enter) ? QuitReason.Enter : null;
	}

	public static bool IsRestartPressed(bool rPressed, bool pPressed) =>
		rPressed || pPressed;

	public static QuitReason? GetQuitReason(bool windowShouldClose, bool enterPressed) =>
		windowShouldClose ? QuitReason.WindowShouldClose :
		enterPressed ? QuitReason.Enter : null;

	public static bool IsQuitRequested(bool windowShouldClose, bool enterPressed) =>
		GetQuitReason(windowShouldClose, enterPressed) is not null;

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
