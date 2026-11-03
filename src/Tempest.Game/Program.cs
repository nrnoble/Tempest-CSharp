// Composition root and fixed-rate main loop. Program only wires command-line selection, the
// Raylib window, input and rendering together; all gameplay simulation belongs in TempestGame so
// it stays headless-testable. Keep it that way — logic added here is invisible to the test suite.
//
// Development command-line modes (see DebugController): --level2-test, --level3-test and --level4-test select
// direct production configurations; --level3-geometry-test selects plus-board geometry only;
// --spiker-test, --tanker-test, --fuseball-test and --pulsar-test select isolated enemy waves.
// These scenarios are mutually exclusive; --enemy-bullets-test and --lives compose with any.
// Normal play progresses Level 1 through terminal Level 5.
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Tempest.Game.Debug;

if (DebugController.HasConflictingScenarios(args))
{
    Console.Error.WriteLine("--level2-test, --level3-test, --level4-test, --level3-geometry-test, --tanker-test, --spiker-test, --fuseball-test and --pulsar-test are mutually exclusive.");
    return 2;
}

// DEVELOPMENT ONLY: observe the same Raylib input framework without running game actions.
if (DebugController.UseInputDiagnostic(args))
    return InputDiagnostic.Run();

Projection.K = 70;
Projection.ZOffset = 5;

bool level2Test = DebugController.UseLevel2Test(args);
bool level3Test = DebugController.UseLevel3Test(args);
bool level4Test = DebugController.UseLevel4Test(args);
bool level3GeometryTest = DebugController.UseLevel3GeometryTest(args);
Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
    level2Test ? "Tempest C# - Development Level 2 Square Test" :
    level3Test ? "Tempest C# - Development Level 3 Mixed Wave Test" :
    level4Test ? "Tempest C# - Development Level 4 Production Candidate" :
    level3GeometryTest ? "Tempest C# - Development Level 3 Geometry Diagnostic" :
    "Tempest C# - Level 1");
Raylib.DisableCursor();
Raylib.SetTargetFPS(60);

try
{
    Raylib.InitAudioDevice();
    if (!Raylib.IsAudioDeviceReady())
        throw new InvalidOperationException("Raylib could not initialize the audio device.");

    using AudioSystem audio = new();
    TempestGame game = new(DebugController.UseSpikerWave(args),
        startingLives: DebugController.GetStartingLives(args),
        enemyBulletsTest: DebugController.UseEnemyBulletsTest(args),
        tankerTest: DebugController.UseTankerTest(args),
        fuseballTest: DebugController.UseFuseballTest(args),
        pulsarTest: DebugController.UsePulsarTest(args),
        level2Test: level2Test,
        level3Test: level3Test,
        level3GeometryTest: level3GeometryTest,
        level4Test: level4Test,
        audioEvents: audio);
    InputSystem input = new();
    VectorRenderer renderer = new(game);
    int activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;

    while (!input.QuitRequested())
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

        if (!level2Test && !level3Test && !level4Test && !level3GeometryTest &&
            game.CurrentLevel.Configuration.DisplayedLevel != activeDisplayedLevel)
        {
            activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
            Raylib.SetWindowTitle($"Tempest C# - Level {activeDisplayedLevel}");
        }

        renderer.Draw();
    }
}
finally
{
    Raylib.EnableCursor();
    if (Raylib.IsAudioDeviceReady())
        Raylib.CloseAudioDevice();
    Raylib.CloseWindow();
}
return 0;