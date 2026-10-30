using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Tempest.Game.Debug;

Projection.K = 70;
Projection.ZOffset = 5;

Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
    "Tempest C# - Level 1 Multi-Flipper Test");
Raylib.DisableCursor();
Raylib.SetTargetFPS(60);

TempestGame game = new(DebugController.UseSpikerWave(args));
InputSystem input = new();
VectorRenderer renderer = new(game);

while (!Raylib.WindowShouldClose())
{
    if (game.GameOver || game.LevelComplete)
        game.RunOneStep(restartPressed: input.RestartPressed());
    else if (game.DeathTicks > 0 || (game.IsExiting && !game.SpikeExitControlsEnabled))
        game.RunOneStep();
    else
        game.RunOneStep(input.ReadPlayer());

    renderer.Draw();
}

Raylib.EnableCursor();
Raylib.CloseWindow();