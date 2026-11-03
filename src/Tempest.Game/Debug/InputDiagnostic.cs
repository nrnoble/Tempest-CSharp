using System.Diagnostics;
using Raylib_cs;
using Tempest.Game.Rendering;

namespace Tempest.Game.Debug;

// DEVELOPMENT ONLY: queues are consumed here, never in the gameplay input path.
public static class InputDiagnostic
{
    public static int Run()
    {
        Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
            "Tempest C# - DEVELOPMENT ONLY Input Diagnostic");
        Raylib.DisableCursor();
        Raylib.SetTargetFPS(60);
        try
        {
            KeyboardKey[] keys = Enum.GetValues<KeyboardKey>().Where(key => (int)key > 0).Distinct().ToArray();
            MouseButton[] buttons = Enum.GetValues<MouseButton>().Distinct().ToArray();
            HashSet<KeyboardKey> heldKeys = [];
            HashSet<MouseButton> heldButtons = [];
            Queue<string> recent = new();
            Stopwatch clock = Stopwatch.StartNew();
            int frame = 0;
            bool? previousFocus = null;
            double nextHeartbeat = 0;

            void Log(string message)
            {
                string line = $"[input frame={frame} t={clock.Elapsed.TotalSeconds:F3}] {message}";
                Console.WriteLine(line);
                recent.Enqueue(line);
                while (recent.Count > 16)
                    recent.Dequeue();
            }

            Log("DEVELOPMENT ONLY; no gameplay/actions/mappings. Logs all focused keyboard input; do not type private text.");
            Log("Test physical keyboard first, then controller: P Enter Space Ctrl Alt Shift Z X C V 1 5.");
            Log("For each: tap twice with a pause, then hold 2 seconds and release. Escape/window X closes.");
            while (!Raylib.WindowShouldClose())
            {
                frame++;
                bool focused = Raylib.IsWindowFocused();
                if (focused != previousFocus)
                {
                    Log($"FOCUS focused={focused}");
                    previousFocus = focused;
                }

                foreach (KeyboardKey key in keys)
                {
                    bool down = Raylib.IsKeyDown(key);
                    bool pressed = Raylib.IsKeyPressed(key);
                    bool released = Raylib.IsKeyReleased(key);
                    bool repeat = Raylib.IsKeyPressedRepeat(key);
                    if (down != heldKeys.Contains(key) || pressed || released || repeat)
                        Log($"STATE key={key}({(int)key}) down={down} pressed={pressed} released={released} repeat={repeat}");
                    if (down)
                        heldKeys.Add(key);
                    else
                        heldKeys.Remove(key);
                }

                int queuedKey;
                while ((queuedKey = Raylib.GetKeyPressed()) != 0)
                {
                    KeyboardKey key = (KeyboardKey)queuedKey;
                    Log($"KEY_QUEUE key={key}({queuedKey}) down={Raylib.IsKeyDown(key)} pressed={Raylib.IsKeyPressed(key)}");
                }

                int codepoint;
                while ((codepoint = Raylib.GetCharPressed()) != 0)
                    Log($"CHAR_QUEUE U+{codepoint:X4} decimal={codepoint}");

                foreach (MouseButton button in buttons)
                {
                    bool down = Raylib.IsMouseButtonDown(button);
                    bool pressed = Raylib.IsMouseButtonPressed(button);
                    bool released = Raylib.IsMouseButtonReleased(button);
                    if (down != heldButtons.Contains(button) || pressed || released)
                        Log($"MOUSE button={button}({(int)button}) down={down} pressed={pressed} released={released}");
                    if (down)
                        heldButtons.Add(button);
                    else
                        heldButtons.Remove(button);
                }

                if (clock.Elapsed.TotalSeconds >= nextHeartbeat)
                {
                    Log($"HEARTBEAT focused={focused} heldKeys=[{string.Join(",", heldKeys.OrderBy(key => (int)key))}]");
                    nextHeartbeat = clock.Elapsed.TotalSeconds + 2;
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.BeginMode2D(DisplayConfiguration.PresentationCamera);
                Raylib.DrawText("DEVELOPMENT ONLY - INPUT DIAGNOSTIC (no gameplay)", 12, 12, 20, Color.Yellow);
                Raylib.DrawText("Tap twice, pause; hold 2 seconds, release. Test one button at a time.", 12, 42, 16, Color.White);
                Raylib.DrawText("P Enter Space Ctrl Alt Shift Z X C V 1 5 | Escape/window X closes", 12, 66, 16, Color.White);
                Raylib.DrawText($"Focused: {focused} | Frame: {frame} | Full output is in the terminal.", 12, 90, 16, Color.White);
                int y = 124;
                foreach (string line in recent)
                {
                    Raylib.DrawText(line, 12, y, 12, Color.White);
                    y += 22;
                }
                Raylib.EndMode2D();
                Raylib.EndDrawing();
            }
            Log("CLOSED (Escape or window close); no gameplay was run.");
            return 0;
        }
        finally
        {
            Raylib.EnableCursor();
            Raylib.CloseWindow();
        }
    }
}
