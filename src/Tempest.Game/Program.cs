// Composition root and fixed-rate main loop. Program only wires command-line selection, the
// Raylib window, input and rendering together; all gameplay simulation belongs in TempestGame so
// it stays headless-testable. Keep it that way — logic added here is invisible to the test suite.
//
// Development command-line modes (see DebugController): --level N starts a fresh supported
// production game and continues progression; --start-level N limits this to campaign levels.
// --gameplay-playback runs the seeded Level 1 input
// profile; --gameplay-playback-level4 runs the distinct profile on production Level 4;
// --level2-test, --level3-test, --level4-test, --level6-test and --level7-test
// select direct development configurations;
// --level3-geometry-test selects plus-board geometry only; --spiker-test, --tanker-test,
// --fuseball-test and --pulsar-test select isolated scenarios.
// These scenario selectors are mutually
// exclusive; --enemy-bullets-test, --lives, --speed and --game-over-density compose with existing
// scenarios. --game-over-density only changes the procedural visible speck count, not mark size;
// --level7-spiker-speed and --level9-spiker-speed fix their level's Spiker movement only for calibration runs;
// --spinner-sensitivity scales only live horizontal mouse/spinner input;
// File recordings use --record-gameplay <file> with optional production startup;
// file playback restores startup identity, pauses with Space, steps with Right, and reconstructs
// backward with Left while paused. F9 pauses live play and F10 requests a screenshot; these
// process controls stay outside PlayerInput and the recording.
// --legacy-presentation selects the established 4:3 rendering instead of the experimental portrait
// presentation; --portrait-perspective calibrates only the shared portrait projection;
// --atari-presentation opts into the shared Atari presentation adapter; Level 7 uses its
// production gameplay geometry for both the tube and gameplay entities;
// --calibration-guides enables fixed DEVELOPMENT-ONLY viewport guides;
// --background-opacity changes only the cleared background alpha for visual comparison;
// --tube-y-gain changes DEVELOPMENT-ONLY vertical tube proportion;
// --tube-depth-y-gain scales the depth-dependent Atari camera-origin Y displacement;
// --tube-scale uniformly scales the DEVELOPMENT-ONLY presented tube and attached artwork;
// --tube-vertical-offset translates the complete DEVELOPMENT-ONLY presentation in screen space;
// Level 10 uses its accepted +50 placement; Level 15 has its own accepted presentation defaults.
// Normal play progresses Level 1 through terminal Level 15.
using System.Globalization;
using System.Text.Json;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Gameplay;

if (args.Contains("--help", StringComparer.Ordinal))
{
    Console.WriteLine(
        "Development-only Atari presentation diagnostic:\n" +
        "  --tube-depth-y-gain <value> scales the depth-dependent vertical camera-origin\n" +
        "  contribution. 1 preserves it, 0 removes it, negative values reverse it.\n" +
        "  Default is 0.3 for Level 13 and 1 for other supported levels, including Level 14.\n" +
        "  Any finite signed invariant-culture value is accepted. Requires\n" +
        "  --atari-presentation and --level, --start-level, or --playback-file.\n" +
        "  It does not change model Y, common placement, HUD, or depth-size ratio.\n" +
        "  Levels 1-6 use separate projections and have no camera-origin term.\n" +
        "See README.md for other development options.\n" +
        "  --background-opacity <0..1> changes only the black background transparency for manual overlay calibration; it does not fade vectors or load/align a reference image (default 1).");
    return 0;
}

if (args.Contains("--level1-four-flipper-cap-test", StringComparer.Ordinal))
{
    Console.Error.WriteLine(
        "--level1-four-flipper-cap-test was removed; production Level 1 now applies the four-Flipper limit automatically.");
    return 2;
}

if (args.Contains("--level2-four-flipper-cap-test", StringComparer.Ordinal))
{
    Console.Error.WriteLine(
        "--level2-four-flipper-cap-test was removed; " +
        "production Level 2 now applies the four-Flipper limit automatically.");
    return 2;
}

if (!DebugController.TryParseBackgroundOpacityOptions(args,
    out BackgroundOpacityOptions backgroundOpacityOptions, out string? backgroundOpacityError))
{
    Console.Error.WriteLine(backgroundOpacityError);
    return 2;
}

if (!DebugController.TryParsePortraitPerspective(args,
    out float? portraitPerspective, out string? portraitPerspectiveError))
{
    Console.Error.WriteLine(portraitPerspectiveError);
    return 2;
}

if (!DebugController.TryParseAtariPresentation(args,
    out bool atariPresentationEnabled, out string? atariPresentationError))
{
    Console.Error.WriteLine(atariPresentationError);
    return 2;
}

if (!DebugController.TryParseCalibrationGuides(args,
    out bool calibrationGuidesEnabled, out string? calibrationGuidesError))
{
    Console.Error.WriteLine(calibrationGuidesError);
    return 2;
}

if (!DebugController.TryParseTubeYGainOptions(args,
    out TubeYGainOptions tubeYGainOptions, out string? tubeYGainError))
{
    Console.Error.WriteLine(tubeYGainError);
    return 2;
}

if (!DebugController.TryParseTubeDepthYGainOptions(args,
    out TubeDepthYGainOptions tubeDepthYGainOptions, out string? tubeDepthYGainError))
{
    Console.Error.WriteLine(tubeDepthYGainError);
    return 2;
}

if (!DebugController.TryParseTubeScaleOptions(args,
    out TubeScaleOptions tubeScaleOptions, out string? tubeScaleError))
{
    Console.Error.WriteLine(tubeScaleError);
    return 2;
}

if (!DebugController.TryParseTubeVerticalOffsetOptions(args,
    out TubeVerticalOffsetOptions tubeVerticalOffsetOptions,
    out string? tubeVerticalOffsetError))
{
    Console.Error.WriteLine(tubeVerticalOffsetError);
    return 2;
}

if (!DebugController.TryParseAtariHudCalibrationOptions(args,
    out AtariHudCalibrationOptions hudCalibrationOptions,
    out string? hudCalibrationError))
{
    Console.Error.WriteLine(hudCalibrationError);
    return 2;
}

bool legacyPresentationRequested = args.Contains("--legacy-presentation", StringComparer.Ordinal);
bool headlessPlaybackAnalysis = HeadlessGameplayPlaybackCli.IsRequested(args);
if (!DebugController.TryValidateAtariPresentationContext(
    atariPresentationEnabled,
    headlessPlaybackAnalysis,
    DebugController.UseInputDiagnostic(args),
    out string? atariPresentationContextError))
{
    Console.Error.WriteLine(atariPresentationContextError);
    return 2;
}

// Keep deterministic file analysis ahead of every Raylib window, input, audio and pacing setup.
if (headlessPlaybackAnalysis)
{
    if (portraitPerspective is not null)
    {
        Console.Error.WriteLine(
            "--portrait-perspective applies only to interactive portrait rendering, not headless playback analysis.");
        return 2;
    }
    return HeadlessGameplayPlaybackCli.Run(args, Console.Out, Console.Error);
}

if (!DebugController.TryParseInteractiveSpeedOptions(args,
    out InteractiveSpeedOptions speedOptions, out string? speedError))
{
    Console.Error.WriteLine(speedError);
    return 2;
}

if (!DebugController.TryParseSpinnerSensitivityOptions(args,
    out SpinnerSensitivityOptions spinnerSensitivityOptions, out string? spinnerSensitivityError))
{
    Console.Error.WriteLine(spinnerSensitivityError);
    return 2;
}

if (DebugController.UseInputDiagnostic(args) &&
    spinnerSensitivityOptions.WasExplicitlySpecified)
{
    Console.Error.WriteLine("--spinner-sensitivity applies only to live gameplay input, not --input-diagnostic.");
    return 2;
}

if (DebugController.UseInputDiagnostic(args) &&
    args.Contains("--speed", StringComparer.Ordinal))
{
    Console.Error.WriteLine("--speed applies to interactive Tempest gameplay, not --input-diagnostic.");
    return 2;
}

if (DebugController.UseInputDiagnostic(args) && portraitPerspective is not null)
{
    Console.Error.WriteLine(
        "--portrait-perspective applies only to interactive Tempest rendering, not --input-diagnostic.");
    return 2;
}

if (!DebugController.TryParseRimSpeedMultiplier(args,
    out double rimSpeedMultiplier, out string? rimSpeedError))
{
    Console.Error.WriteLine(rimSpeedError);
    return 2;
}

if (!DebugController.TryParseLevel7SpikerSpeedOverride(args,
    out float? level7SpikerSpeedOverride, out string? level7SpikerSpeedError))
{
    Console.Error.WriteLine(level7SpikerSpeedError);
    return 2;
}

if (!DebugController.TryParseLevel9SpikerSpeedOverride(args,
    out float? level9SpikerSpeedOverride, out string? level9SpikerSpeedError))
{
    Console.Error.WriteLine(level9SpikerSpeedError);
    return 2;
}

if (level7SpikerSpeedOverride is not null &&
    level9SpikerSpeedOverride is not null)
{
    Console.Error.WriteLine("Level 7 and Level 9 Spiker speed overrides are mutually exclusive.");
    return 2;
}

if (DebugController.UseInputDiagnostic(args) && level7SpikerSpeedOverride is not null)
{
    Console.Error.WriteLine("--level7-spiker-speed applies only to Level 7 gameplay, not --input-diagnostic.");
    return 2;
}

if (DebugController.UseInputDiagnostic(args) && level9SpikerSpeedOverride is not null)
{
    Console.Error.WriteLine("--level9-spiker-speed applies only to Level 9 gameplay, not --input-diagnostic.");
    return 2;
}

if (!DebugController.TryParseGameOverDensityMultiplier(args,
    out double gameOverDensityMultiplier, out string? gameOverDensityError))
{
    Console.Error.WriteLine(gameOverDensityError);
    return 2;
}

if (!DebugController.TryParseGameplayFileCommand(args,
    out GameplayFileCommand? gameplayFileCommand, out string? commandLineError))
{
    Console.Error.WriteLine(commandLineError);
    return 2;
}

if (!DebugController.TryParseStartingLevel(args,
    out int? startingProductionLevel, out string? startingLevelError))
{
    Console.Error.WriteLine(startingLevelError);
    return 2;
}

if (DebugController.UseInputDiagnostic(args) &&
    args.Contains("--start-level", StringComparer.Ordinal))
{
    Console.Error.WriteLine("--start-level applies to gameplay, not --input-diagnostic.");
    return 2;
}

PresentationMode presentationMode = legacyPresentationRequested
    ? PresentationMode.Legacy
    : PresentationMode.ExperimentalPortrait;

if (level9SpikerSpeedOverride is not null && startingProductionLevel != 9)
{
    Console.Error.WriteLine("--level9-spiker-speed is only valid with --level 9.");
    return 2;
}

if (DebugController.HasConflictingScenarios(args))
{
    Console.Error.WriteLine("--level N, --start-level N, --gameplay-playback, --gameplay-playback-level4, --level2-test, --level3-test, --level4-test, --level6-test, --level7-test, --level3-geometry-test, --tanker-test, --spiker-test, --fuseball-test and --pulsar-test are mutually exclusive.");
    return 2;
}

if (spinnerSensitivityOptions.WasExplicitlySpecified)
{
    string multiplier = spinnerSensitivityOptions.Multiplier.ToString(
        "G", CultureInfo.InvariantCulture);
    Console.WriteLine(
        $"Spinner sensitivity: {multiplier}x live horizontal spinner/mouse response relative to current behavior.");
}

if (atariPresentationEnabled)
{
    Console.WriteLine(
        "DEVELOPMENT ONLY — Atari presentation candidate enabled for production Levels 1-15.");
}

if (tubeYGainOptions.WasExplicitlySpecified)
{
    Console.WriteLine(
        $"DEVELOPMENT ONLY — tube source cross-section Y gain {tubeYGainOptions.Gain.ToString("G", CultureInfo.InvariantCulture)}; " +
        "applies across the active Atari-presentation level; perspective is unchanged.");
}

if (tubeDepthYGainOptions.WasExplicitlySpecified)
{
    Console.WriteLine(
        $"DEVELOPMENT ONLY — Atari depth-origin Y gain {tubeDepthYGainOptions.Gain.ToString("G", CultureInfo.InvariantCulture)}; " +
        "1 preserves the current camera-origin drift, 0 removes it; depth-size ratio is unchanged.");
}

if (tubeScaleOptions.WasExplicitlySpecified)
{
    Console.WriteLine(
        $"DEVELOPMENT ONLY — presented tube uniform scale {tubeScaleOptions.Scale.ToString("G", CultureInfo.InvariantCulture)}; " +
        "applies across the active Atari-presentation level.");
}

if (tubeVerticalOffsetOptions.WasExplicitlySpecified)
{
    Console.WriteLine(
        $"DEVELOPMENT ONLY — presented tube vertical offset {tubeVerticalOffsetOptions.Offset.ToString("G", CultureInfo.InvariantCulture)}; " +
        "positive values move down and negative values move up.");
}

if (hudCalibrationOptions.HasExplicitOverrides)
{
    Console.WriteLine(
        "DEVELOPMENT ONLY — Atari HUD calibration overrides active; " +
        "HUD X/Y values use 600x800 portrait logical coordinates, positive X moves right, and positive Y moves down.");
}

if (level7SpikerSpeedOverride is float fixedSpikerSpeed)
{
    Console.WriteLine(
        $"Level 7 Spiker calibration active: fixed {fixedSpikerSpeed.ToString("0.000", CultureInfo.InvariantCulture)} " +
        "depth units per active step; production speed variation is suppressed on Level 7 only.");
}

if (level9SpikerSpeedOverride is float fixedLevel9SpikerSpeed)
{
    Console.WriteLine(
        $"DEVELOPMENT ONLY — Level 9 Spiker calibration override active: " +
        $"fixed {fixedLevel9SpikerSpeed.ToString("0.000", CultureInfo.InvariantCulture)} " +
        "depth units per active step; production speed variation is suppressed on Level 9 only.");
}

// DEVELOPMENT ONLY: observe the same Raylib input framework without running game actions.
if (DebugController.UseInputDiagnostic(args))
    return InputDiagnostic.Run(backgroundOpacityOptions);

Projection.K = 70;
Projection.ZOffset = 5;
Projection.PortraitPerspective = presentationMode == PresentationMode.ExperimentalPortrait
    ? portraitPerspective ?? Projection.PortraitPerspectiveDefault
    : Projection.PortraitPerspectiveDefault;

bool level2Test = DebugController.UseLevel2Test(args);
bool level3Test = DebugController.UseLevel3Test(args);
bool level4Test = DebugController.UseLevel4Test(args);
bool level6Test = DebugController.UseLevel6Test(args);
bool level7Test = DebugController.UseLevel7Test(args);
bool level3GeometryTest = DebugController.UseLevel3GeometryTest(args);
bool level4Playback = DebugController.UseLevel4GameplayPlayback(args);
PlayerInputReplay? playback = null;
PlayerInputRecorder? recording = null;
GameplayStartupConfiguration? gameplayStartupConfiguration = null;
VisiblePlaybackController? visiblePlaybackController = null;
string? recordingPath = null;
string? loadedPlaybackPath = null;
try
{
    if (gameplayFileCommand is { Mode: GameplayFileMode.Record })
    {
        recordingPath = GameplayRecordingFile.PrepareOutputPath(
            ApplicationPaths.Current.ResolveGameplayRecordingPath(gameplayFileCommand.Path));
        int randomSeed = Random.Shared.Next();
        gameplayStartupConfiguration = new GameplayStartupConfiguration(
            gameplayFileCommand.StartingDisplayedLevel ?? LevelConfiguration.Level1.DisplayedLevel,
            gameplayFileCommand.StartingLives ?? Player.DefaultStartingLives,
            gameplayFileCommand.RimSpeedMultiplier ?? 1.0,
            randomSeed,
            gameplayFileCommand.Level7SpikerSpeedOverride,
            level9SpikerSpeedOverride: gameplayFileCommand.Level9SpikerSpeedOverride);
        recording = new PlayerInputRecorder(gameplayStartupConfiguration);
        Console.WriteLine(
            $"Gameplay recording active: {recordingPath} (Level {gameplayStartupConfiguration.StartingDisplayedLevel}, " +
            $"{gameplayStartupConfiguration.StartingLives} lives, rim speed {gameplayStartupConfiguration.RimSpeedMultiplier}, " +
            $"seed {randomSeed}). Close the window normally to save.");
    }
    else if (gameplayFileCommand is { Mode: GameplayFileMode.Playback })
    {
        string playbackPath = Path.GetFullPath(
            ApplicationPaths.Current.ResolveGameplayRecordingPath(gameplayFileCommand.Path));
        GameplayRecording file = GameplayRecordingFile.LoadRecording(playbackPath);
        gameplayStartupConfiguration = file.StartupConfiguration;
        playback = file.Replay;
        loadedPlaybackPath = playbackPath;
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

int? deterministicSeed = gameplayStartupConfiguration?.RandomSeed ??
    playback?.RandomSeed ?? recording?.RandomSeed;
backgroundOpacityOptions.ConfigureBeforeWindowInitialization(Raylib.SetConfigFlags);
Raylib.InitWindow(VectorRenderer.ScreenWidth, VectorRenderer.ScreenHeight,
    recording is not null
        ? $"Tempest C# - Recording Level {recording.StartupConfiguration.StartingDisplayedLevel}" :
    playback is not null
        ? $"Tempest C# - Deterministic Gameplay Playback - Level {playback.StartingConfiguration.DisplayedLevel}" :
    startingProductionLevel is int selectedLevel
        ? $"Tempest C# - Development Level {selectedLevel}" :
    level2Test ? "Tempest C# - Development Level 2 Square Test" :
    level3Test ? "Tempest C# - Development Level 3 Mixed Wave Test" :
    level4Test ? "Tempest C# - Development Level 4 Production Candidate" :
    level6Test ? "Tempest C# - Development Level 6 Production Test" :
    level7Test ? "Tempest C# - Development Level 7 Production Test" :
    level3GeometryTest ? "Tempest C# - Development Level 3 Geometry Diagnostic" :
    "Tempest C# - Level 1");
Raylib.DisableCursor();
Raylib.SetTargetFPS(speedOptions.TargetFramesPerSecond);

try
{
    Raylib.InitAudioDevice();
    if (!Raylib.IsAudioDeviceReady())
        throw new InvalidOperationException("Raylib could not initialize the audio device.");

    using AudioSystem audio = new();
    if (gameplayFileCommand is { Mode: GameplayFileMode.Playback })
    {
        GameplayStartupConfiguration startup = gameplayStartupConfiguration ??
            throw new InvalidOperationException("File playback has no restored startup configuration.");
        PlayerInputReplay replay = playback ??
            throw new InvalidOperationException("File playback has no validated replay.");
        visiblePlaybackController = new(new GameplayPlaybackSession(startup, replay, audio));
    }
    else if (DebugController.UseGameplayPlayback(args))
    {
        PlayerInputReplay replay = playback ??
            throw new InvalidOperationException("The Level 1 playback profile was not initialized.");
        GameplayStartupConfiguration startup = new(1, Player.DefaultStartingLives,
            1.0, replay.RandomSeed);
        visiblePlaybackController = new(new GameplayPlaybackSession(startup, replay, audio));
    }

    TempestGame game = visiblePlaybackController?.Session.Game ??
        (gameplayStartupConfiguration is not null
            ? new TempestGame(gameplayStartupConfiguration, audioEvents: audio)
            : new TempestGame(DebugController.UseSpikerWave(args),
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
                level7Test: level7Test,
                rimSpeedMultiplier: rimSpeedMultiplier,
                startingProductionLevel: startingProductionLevel,
                level7SpikerSpeedOverride: level7SpikerSpeedOverride,
                level9SpikerSpeedOverride: level9SpikerSpeedOverride));
    InputSystem input = new();
    VectorRenderer renderer = new(
        game, gameOverDensityMultiplier, atariPresentationEnabled,
        tubeYGainOptions.WasExplicitlySpecified ? tubeYGainOptions.Gain : null,
        tubeScaleOptions.WasExplicitlySpecified ? tubeScaleOptions.Scale : null,
        tubeVerticalOffsetOptions.WasExplicitlySpecified
            ? tubeVerticalOffsetOptions.Offset
            : null,
        calibrationGuidesEnabled,
        hudCalibrationOptions,
        tubeDepthYGainOptions.WasExplicitlySpecified
            ? tubeDepthYGainOptions.Gain
            : null,
        backgroundClearColor: backgroundOpacityOptions.BackgroundClearColor);
    PlaybackScreenshotCapture screenshotCapture = new(
        ApplicationPaths.Current, Raylib.TakeScreenshot, Console.Out, Console.Error);
    PlaybackScreenshotKeyDiagnostic? screenshotKeyDiagnostic =
        gameplayFileCommand is { ScreenshotKeyDiagnostic: true }
            ? new PlaybackScreenshotKeyDiagnostic(
                Directory.GetCurrentDirectory(), Console.Out)
            : null;
    int activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
    int simulationTick = 0;
    bool livePaused = false;
    bool playbackCompletionReported = false;

    while (true)
    {
        QuitReason? quitReason = input.ReadQuitReason();
        if (quitReason is not null)
        {
            Console.WriteLine($"Exit reason: {quitReason}");
            break;
        }

        bool liveScreenshotRequested = false;
        bool playbackControlsHandled = false;
        if (visiblePlaybackController is { IsActive: true } playbackController)
        {
            simulationTick += playbackController.ProcessControls(input.ReadPlaybackControls());
            if (!ReferenceEquals(game, playbackController.Session.Game))
            {
                game = playbackController.Session.Game;
                renderer.SetGame(game);
            }
            playbackControlsHandled = true;
        }

        if (visiblePlaybackController?.TryMarkCompletionReported() == true)
        {
            int duration = visiblePlaybackController.Session.RecordingDurationTicks;
            string completionState = visiblePlaybackController.IsPaused
                ? "paused at final boundary."
                : "live controls resumed.";
            Console.WriteLine(
                $"Gameplay playback completed after {duration} simulation ticks; {completionState}");
            playbackCompletionReported = true;
        }

        if (!playbackControlsHandled)
        {
            LiveControlInput liveControls = input.ReadLiveControls();
            livePaused = InputSystem.ToggleLivePause(livePaused, liveControls.PauseTogglePressed);
            liveScreenshotRequested = liveControls.ScreenshotPressed;

            if (livePaused)
            {
                input.DiscardMouseDelta();
            }
            else
            {
                bool replaying = playback is not null && playback.IsActiveAt(simulationTick);
                if (playback is not null && visiblePlaybackController is null &&
                    !replaying && !playbackCompletionReported)
                {
                    Console.WriteLine($"Gameplay playback completed after {simulationTick} simulation ticks; live controls resumed.");
                    playbackCompletionReported = true;
                }

                PlayerInput effectiveInput = default;
                bool effectiveRestartPressed = false;
                if (replaying)
                {
                    // The built-in Level 4 demo keeps its terminal diagnostic startup, which the
                    // production-only playback session intentionally does not model.
                    (effectiveInput, effectiveRestartPressed) =
                        GameplayPlaybackSession.ApplyPlaybackInputGates(
                            game, playback!.GetFrame(simulationTick));
                }
                else if (game.GameOver || game.LevelComplete)
                    effectiveRestartPressed = input.RestartPressed();
                else if (game.DeathTicks == 0 &&
                    (!game.IsExiting || game.ExitMovementInputEnabled))
                    effectiveInput = input.ReadPlayer(spinnerSensitivityOptions.Multiplier);

                game.RunOneStep(effectiveInput, effectiveRestartPressed);
                recording?.CaptureTick(simulationTick, effectiveInput, effectiveRestartPressed);
                simulationTick++;
            }
        }

        if (!level2Test && !level3Test && !level4Test && !level6Test && !level7Test &&
            !level3GeometryTest &&
            game.CurrentLevel.Configuration.DisplayedLevel != activeDisplayedLevel)
        {
            activeDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
            Raylib.SetWindowTitle($"Tempest C# - Level {activeDisplayedLevel}");
        }

        PlaybackOverlayStatus? overlayStatus =
            visiblePlaybackController is { IsPaused: true } pausedPlayback
                ? new PlaybackOverlayStatus(
                    pausedPlayback.Session.CurrentBoundary,
                    pausedPlayback.Session.RecordingDurationTicks)
                : null;
        Action? screenshotCaptureAfterRender = null;
        if (gameplayFileCommand is { Mode: GameplayFileMode.Playback } &&
            visiblePlaybackController is { IsActive: true, IsPaused: true } pausedFilePlayback)
        {
            PlaybackScreenshotInput screenshotInput = input.ReadPlaybackScreenshotInput();
            bool f12Pressed = screenshotKeyDiagnostic is not null &&
                (bool)Raylib.IsKeyPressed(KeyboardKey.F12);
            screenshotKeyDiagnostic?.Observe(
                screenshotInput.ControlDown,
                screenshotInput.ShiftDown,
                screenshotInput.SPressed,
                f12Pressed);
            bool captureRequested = PlaybackScreenshotCapture.IsCaptureRequested(
                filePlaybackActive: true, playbackPaused: true, screenshotInput);
            if (captureRequested)
            {
                screenshotCaptureAfterRender = () =>
                {
                    screenshotCapture.TryCapture(
                        filePlaybackActive: true,
                        playbackPaused: true,
                        input: screenshotInput,
                        game.CurrentLevel.Configuration.DisplayedLevel,
                        pausedFilePlayback.Session.CurrentBoundary);
                    if (screenshotKeyDiagnostic is not null)
                    {
                        string? requestedPath = screenshotCapture.LastRequestedPath;
                        screenshotKeyDiagnostic.RecordProjectCapture(
                            screenshotCapture.LastRaylibPath,
                            requestedPath,
                            requestedPath is not null && File.Exists(requestedPath),
                            screenshotCapture.LastFileLength);
                    }
                };
            }
        }

        if (liveScreenshotRequested)
        {
            screenshotCaptureAfterRender = () =>
                screenshotCapture.TryCaptureLive(
                    game.CurrentLevel.Configuration.DisplayedLevel, simulationTick);
        }

        renderer.Draw(overlayStatus, screenshotCaptureAfterRender, presentationMode);
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