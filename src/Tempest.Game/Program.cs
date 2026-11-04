// Composition root and fixed-rate main loop. Program only wires command-line selection, the
// Raylib window, input and rendering together; all gameplay simulation belongs in TempestGame so
// it stays headless-testable. Keep it that way — logic added here is invisible to the test suite.
//
// Development command-line modes (see DebugController): --gameplay-playback runs the seeded Level 1
// input profile; --gameplay-playback-level4 runs the distinct profile on production Level 4;
// --level2-test, --level3-test, --level4-test, --level6-test and --level7-test select direct production configurations;
// --level3-geometry-test selects plus-board geometry only; --spiker-test, --tanker-test,
// --fuseball-test and --pulsar-test select isolated enemy waves. These scenarios are mutually
// exclusive; --enemy-bullets-test and --lives compose with existing scenarios. --record-gameplay
// and --playback-file are standalone Level 1 modes.
// Normal play progresses Level 1 through terminal Level 7.
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Tempest.Game.Debug;
using Tempest.Game.Levels;

if (!DebugController.TryParseGameplayFileCommand(args,
    out GameplayFileCommand? gameplayFileCommand, out string? commandLineError))
{
    Console.Error.WriteLine(commandLineError);
    return 2;
}

if (DebugController.HasConflictingScenarios(args))
{
    Console.Error.WriteLine("--gameplay-playback, --gameplay-playback-level4, --level2-test, --level3-test, --level4-test, --level6-test, --level7-test, --level3-geometry-test, --tanker-test, --spiker-test, --fuseball-test and --pulsar-test are mutually exclusive.");
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
bool level6Test = DebugController.UseLevel6Test(args);
bool level7Test = DebugController.UseLevel7Test(args);
bool level3GeometryTest = DebugController.UseLevel3GeometryTest(args);
bool level4Playback = DebugController.UseLevel4GameplayPlayback(args);
PlayerInputReplay? playback = null;
PlayerInputRecorder? recording = null;
string? recordingPath = null;
try
{
    if (gameplayFileCommand is { Mode: GameplayFileMode.Record })
    {
        recordingPath = GameplayRecordingFile.PrepareOutputPath(gameplayFileCommand.Path);
        int randomSeed = Random.Shared.Next();
        recording = new PlayerInputRecorder(randomSeed, LevelConfiguration.Level1);
        Console.WriteLine($"Gameplay recording active: {recordingPath} (seed {randomSeed}). Close the window normally to save.");
    }
    else if (gameplayFileCommand is { Mode: GameplayFileMode.Playback })
    {
        string playbackPath = Path.GetFullPath(gameplayFileCommand.Path);
        playback = GameplayRecordingFile.Load(playbackPath);
        Console.WriteLine($"Gameplay file playback active: {playbackPath}");
    }
    else
    {
        playback = DebugController.UseGameplayPlayback(args)
            ? GameplayPlayback.CreateLevel1()
            : level4Playback
                ? GameplayPlayback.CreateLevel4()
                : null;
    }
}
catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or
    ArgumentException or NotSupportedException or JsonException or System.Security.SecurityException)
{
    Console.Error.WriteLine($"Could not prepare gameplay file mode: {exception.Message}");
    return 2;
}

int? deterministicSeed = playback?.RandomSeed ?? recording?.RandomSeed;
Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
    recording is not null
        ? "Tempest C# - Recording Level 1" :
    playback is not null
        ? $"Tempest C# - Deterministic Gameplay Playback - Level {playback.StartingConfiguration.DisplayedLevel}" :
    level2Test ? "Tempest C# - Development Level 2 Square Test" :
    level3Test ? "Tempest C# - Development Level 3 Mixed Wave Test" :
    level4Test ? "Tempest C# - Development Level 4 Production Candidate" :
    level6Test ? "Tempest C# - Development Level 6 Production Test" :
    level7Test ? "Tempest C# - Development Level 7 Production Test" :
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
        random: deterministicSeed is int seed ? new Random(seed) : null,
        startingLives: DebugController.GetStartingLives(args),
        enemyBulletsTest: DebugController.UseEnemyBulletsTest(args),
        tankerTest: DebugController.UseTankerTest(args),
        fuseballTest: DebugController.UseFuseballTest(args),
        pulsarTest: DebugController.UsePulsarTest(args),
        level2Test: level2Test,
        level3Test: level3Test,
        level3GeometryTest: level3GeometryTest,
        level4Test: level4Test || level4Playback,
        audioEvents: audio,
        level6Test: level6Test,
        level7Test: level7Test);
    InputSystem input = new();
    VectorRenderer renderer = new(game);
    int activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
    int simulationTick = 0;
    bool playbackCompletionReported = false;

    while (!input.QuitRequested())
    {
        bool replaying = playback is not null && playback.IsActiveAt(simulationTick);
        if (playback is not null && !replaying && !playbackCompletionReported)
        {
            Console.WriteLine($"Gameplay playback completed after {simulationTick} simulation ticks; live controls resumed.");
            playbackCompletionReported = true;
        }

        // Death and ordinary exit delay stay frozen. Actual exit travel polls movement input;
        // the simulation retains the separate ordinary/Spike firing gates.
        PlayerInput effectiveInput = default;
        bool effectiveRestartPressed = false;
        if (game.GameOver || game.LevelComplete)
            effectiveRestartPressed = replaying
                ? playback!.GetFrame(simulationTick).RestartPressed
                : input.RestartPressed();
        else if (game.DeathTicks == 0 &&
            (!game.IsExiting || game.ExitMovementInputEnabled))
            effectiveInput = replaying
                ? playback!.GetFrame(simulationTick).Input
                : input.ReadPlayer();

        game.RunOneStep(effectiveInput, effectiveRestartPressed);
        recording?.CaptureTick(simulationTick, effectiveInput, effectiveRestartPressed);
        simulationTick++;

        if (!level2Test && !level3Test && !level4Test && !level6Test && !level7Test &&
            !level3GeometryTest &&
            game.CurrentLevel.Configuration.DisplayedLevel != activeDisplayedLevel)
        {
            activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
            Raylib.SetWindowTitle($"Tempest C# - Level {activeDisplayedLevel}");
        }

        renderer.Draw();
    }

    if (recording is not null)
    {
        string outputPath = recordingPath ??
            throw new InvalidOperationException("The recording destination was not initialized.");
        try
        {
            GameplayRecordingFile.SaveNew(outputPath, recording);
            Console.WriteLine($"Gameplay recording saved: {recording.DurationTicks} simulation ticks to {outputPath}");
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or
            ArgumentException or NotSupportedException or System.Security.SecurityException)
        {
            Console.Error.WriteLine($"Gameplay recording was not saved: {exception.Message}");
            return 1;
        }
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