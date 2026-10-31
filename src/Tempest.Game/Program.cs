// Composition root and fixed-rate main loop. Program only wires command-line selection, the
// Raylib window, input and rendering together; all gameplay simulation belongs in TempestGame so
// it stays headless-testable. Keep it that way — logic added here is invisible to the test suite.
//
// Development command-line modes (see DebugController): --level2-test selects direct access to
// the production Level 2 configuration; --spiker-test, --tanker-test, --fuseball-test and
// --pulsar-test select isolated enemy-development waves. These scenarios are mutually exclusive;
// --enemy-bullets-test and --lives compose with any. Normal play progresses Level 1 to Level 2.
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Tempest.Game.Debug;

if (DebugController.HasConflictingScenarios(args))
{
    Console.Error.WriteLine("--level2-test, --tanker-test, --spiker-test, --fuseball-test and --pulsar-test are mutually exclusive.");
    return 2;
}

Projection.K = 70;
Projection.ZOffset = 5;

bool level2Test = DebugController.UseLevel2Test(args);
Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
    level2Test ? "Tempest C# - Development Level 2 Square Test" : "Tempest C# - Level 1");
Raylib.DisableCursor();
Raylib.SetTargetFPS(60);

TempestGame game = new(DebugController.UseSpikerWave(args),
    startingLives: DebugController.GetStartingLives(args),
    enemyBulletsTest: DebugController.UseEnemyBulletsTest(args),
	tankerTest: DebugController.UseTankerTest(args),
    fuseballTest: DebugController.UseFuseballTest(args),
    pulsarTest: DebugController.UsePulsarTest(args),
    level2Test: level2Test);
InputSystem input = new();
VectorRenderer renderer = new(game);
int activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;

while (!Raylib.WindowShouldClose())
{
    // Input routing mirrors the simulation phases: frozen end states only accept restart; the
    // death freeze and automated exit travel ignore the player entirely (except the spike-exit
    // scenario, where movement/firing stay live); otherwise full player input applies.
    if (game.GameOver || game.LevelComplete)
        game.RunOneStep(restartPressed: input.RestartPressed());
    else if (game.DeathTicks > 0 || (game.IsExiting && !game.SpikeExitControlsEnabled))
        game.RunOneStep();
    else
        game.RunOneStep(input.ReadPlayer());

    if (!level2Test && game.CurrentLevel.Configuration.DisplayedLevel != activeDisplayedLevel)
    {
        activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
        Raylib.SetWindowTitle($"Tempest C# - Level {activeDisplayedLevel}");
    }

    renderer.Draw();
}

Raylib.EnableCursor();
Raylib.CloseWindow();
return 0;