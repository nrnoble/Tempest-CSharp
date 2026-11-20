# Controller Input Compatibility Investigation

## Current outcome - manual PASS; permanent P/V aliases accepted

Neal completed the physical USB controller diagnostic. Ordinary buttons
reached Raylib as genuine keyboard press events; text-format incompatibility
is not established as the cause of the earlier gameplay Space failure.
Space remains exactly the existing zap binding (with right mouse also
preserved). P and V are additive keyboard aliases, not ARCADE VERIFIED
gameplay mechanics. Subsequent manual gameplay validation passed on both
the ordinary keyboard and physical USB controller. No C2 work, commit or
push occurred.

### Physical evidence and corrected uncertainty

Neal reported `STATE down=True pressed=True` and matching `KEY_QUEUE`
events for P(80), Enter(257), LeftControl(341), LeftAlt(342), Space(32),
LeftShift(340), Z(90), X(88), C(67) and V(86). Printable buttons additionally
produced their ordinary Unicode characters, including `U+0020` for Space.
The related user-created `Tempest-controller-input.txt` capture was already
untracked when this implementation began. Reading it as UTF-16 confirmed
those keys' actual down/press and release lines; it was left untouched.
No new mapping for Z/X/C/Enter/Ctrl/Alt/Shift/1/5 was added.

The two small red Atari-style Player 1/Player 2 volcano buttons under Turbo
Mode produced no observable Raylib diagnostic event, per Neal's test.
Future Player 1 Start binding/investigation is deferred, not implemented
through lower-level input. No Raw Input, Win32 keyboard hook, production
character/key-queue consumption, USB/HID inference or remapping software
was introduced.

These results supersede the historical uncertainty below about whether
ordinary controller buttons expose usable Raylib key edges. They do not
establish why Space failed in the earlier isolated gameplay session.
Subsequent physical-controller and keyboard gameplay tests passed for Space
when available, resolving that uncertainty for practical validation. No cause
is attributed to the earlier failure. No alternate Super Zapper key or
controller-specific zap path was justified or added.

### Permanent changes and state gates

- [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs) now samples
  `IsKeyPressed(R)` OR `IsKeyPressed(P)` into its one existing restart request.
  The pure `IsRestartPressed` combiner follows the existing zap-combiner
  pattern and collapses simultaneous aliases to one boolean.
- [Program.cs](../../src/Tempest.Game/Program.cs) still supplies restart only
  at Game Over/Level Complete. The existing simulation owns all replay/
  reset/score/lives/progression semantics, unchanged. There is no P-specific
  action path or restart while Playing, dying or traveling out.
- `InputSystem.QuitRequested` combines Raylib's existing `WindowShouldClose`
  condition with `IsKeyPressed(V)`. The normal gameplay loop checks that
  combined condition before any next simulation step. V, Escape and window
  X therefore reach the existing `EnableCursor`/`CloseWindow` cleanup and
  return 0. Raylib's default Escape close behavior was not overridden.
- Both aliases use press-edge APIs, not held or repeat APIs. Headless tests
  verify combining supplied edges and absence of another request without a
  new edge. Neal has now reported manual acceptance of the implemented
  controls on keyboard and controller.
- Space/right mouse zap, held left/right Ctrl or left mouse Fire, horizontal
  spinner/mouse delta and Left/Right arrow movement are unchanged.
- The existing DEVELOPMENT ONLY standalone input diagnostic and its tests
  were preserved. Production aliases do not run there: P/V remain observable
  diagnostic buttons; Escape/window X still close it. No input logging or
  queue consumption runs in normal play.
- [README.md](../../README.md) documents permanent alternative controls.
  [Development notes](../../TEMPEST-DEVELOPMENT-NOTES.md) record evidence,
  aliases and deferred volcano investigation. The behavior specification's
  scope is gameplay/evidence, not controller implementation; it was left
  unchanged rather than adding an implementation narrative.
- Diagnostic font readability and Neal's reported Tee-Object file-location/
  capture difficulty remain non-blocking usability concerns. No diagnostic
  polish or PowerShell investigation was undertaken.

### Implementation validation

SDK 9.0.304; xUnit v2/VSTest. No global.json, Directory.Build.props or
Directory.Packages.props was found.

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter 'FullyQualifiedName~InputSystemTests|FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~SuperZapperTests|FullyQualifiedName~LevelProgressionTests'
dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check
```

- Focused tests: **66 passed**, zero failed/skipped. Sixteen new cases cover
  R/P and close/V truth tables, shared restart/replay outcomes, absent new
  press edges, and existing inactive-state restart rejection. Existing zap,
  progression and diagnostic tests were included. No physical USB behavior
  was simulated or claimed as automated evidence.
- Full solution tests: **452 passed**, zero failed/skipped.
- Solution build: passed, **zero warnings/errors**.
- `git diff --check`: passed; only Git's LF-to-CRLF informational conversion
  warnings appeared for tracked text edits.
- `git status --short -- Reference/incitti` and the behavior specification
  returned no changes. No gameplay simulation, C1/C1.5 geometry/directed
  rotation, progression, Tanker/C2, renderer or established action mechanics
  were edited. Full tests include their existing regressions.
- `git check-ignore` confirmed both prompt/report paths under AI-Logs and
  the existing local video path remain ignored; .gitignore is unchanged.

Implementation-step visible worktree (including the preceding diagnostic):
five modified tracked files (README, development notes, DebugController,
Program, InputSystem); three untracked diagnostic/input test source files
(InputDiagnostic.cs, InputDiagnosticTests.cs, InputSystemTests.cs); and the
pre-existing untracked controller capture. Tracked diff: 106 insertions,
2 deletions across five files. AI-Logs are saved locally and remain ignored.
The [exact implementation prompt](../Prompts/2026-10-31-controller-input-compatibility-implementation-prompt.md)
was copied from Neal's attachment with matching SHA256 hashes.

### Completed manual gameplay acceptance (2026-10-31)

Neal reported controller/input validation complete and PASS on both the normal
keyboard and physical USB arcade controller:

- Arcade spinner and existing Fire work normally.
- Controller Space activates Super Zapper normally when available; keyboard
  Space also works normally.
- Controller P works as the additive Restart/Replay/Start alternative in
  applicable existing states; keyboard R continues working.
- Controller V quits normally; keyboard Escape quits normally.
- Existing controls were not replaced; no obvious gameplay regression was
  observed.

Permanent P/V aliases are manually accepted. Practical controller-Space
uncertainty is resolved by the subsequent PASS, without speculation about the
earlier isolated test. No manual-validation blocker remains. Window-close
code remains unchanged; this acceptance records only the results Neal
explicitly reported, not an additional claimed window-X observation.

### Deferred fidelity observations

Current C# spinner movement is good, very playable and not a blocker. On the
same physical spinner, Neal found Incitti's response effectively perfect
without Windows mouse-setting adjustments. Future work should compare actual
spinner/input/movement processing rather than blindly changing sensitivity.

Neal's qualitative assessment, based on extensive Atari arcade experience,
is that Incitti's timing feels extremely close to the arcade. Timing
differences are useful future engineering comparisons. Direct arcade
observations remain highest authority; Incitti does not automatically become
ARCADE VERIFIED.

Neal assesses C# Blaster/enemy visuals as impressive and reasonably faithful,
roughly 3.5/5, versus approximately 5/5 for Incitti's arcade resemblance.
These subjective ratings are not measurements or automated acceptance
criteria. Defer visual refinement until actual vector/object definitions,
transforms, projection, scale, animation and rendering behavior are compared.
No spinner, timing, rendering, volcano-button or C2 implementation occurred.

### Finalization scope

This finalization changes only development notes and this ignored report.
README's controls are already accurate and need no status correction.
The exact finalization prompt is saved under
[Prompts](../Prompts/2026-10-31-controller-input-manual-validation-acceptance-prompt.md).
The useful diagnostic and its tests remain intended checkpoint source.
The user-created `Tempest-controller-input.txt` remains untouched, untracked,
unstaged diagnostic evidence and is excluded from the recommended commit.
AI-Logs and Videos remain ignored. Nothing was staged, committed or pushed.

Finalization rerun: the same focused command above passed **66 tests**;
`dotnet test TempestCSharp.slnx --no-restore` passed **452 tests**, with no
failures/skips. `dotnet build TempestCSharp.slnx --no-restore` passed with
zero warnings/errors. `git diff --check` passed. Complete tracked diff and
all three untracked source/test files were inspected; they remain useful and
correct for this checkpoint. Protected reference/spec paths have no changes.

Recommended commit scope (eight files):

- Modified tracked: `README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`,
  `src/Tempest.Game/Debug/DebugController.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`.
- New source/tests: `src/Tempest.Game/Debug/InputDiagnostic.cs`,
  `tests/Tempest.Game.Tests/InputDiagnosticTests.cs`,
  `tests/Tempest.Game.Tests/InputSystemTests.cs`.

Tracked diff totals 132 insertions and 2 deletions across five files;
untracked source/tests are additional. Exclude the untracked controller
capture and ignored AI-Logs/Videos. The checkpoint is ready for a selective
commit, but the worktree is intentionally not clean or staged. No blocker
remains. Suggested commit message:

```text
Add validated controller restart and quit aliases

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
```

## Historical diagnostic investigation (superseded pending-results status)

The following sections preserve the initial investigation and its tests.
Their statements that physical results or mappings are pending describe
the earlier diagnostic-only checkpoint, not the current implementation.

### Original outcome: B - diagnostic ready; physical controller results required

The code and framework establish possible failure paths, not this USB
controller's actual signal. No permanent compatibility mapping or Windows
workaround was implemented. Neal must run the diagnostic below before choosing
additional Super Zapper, restart and quit buttons. C2 was not started.

## Baseline

Before editing, `git status --short` was empty and `git diff --check` passed.
`git log -2 --oneline` returned exactly:

```text
a922172 Ignore local video files
11773c0 Implement Level 3 geometry and directed rotation
```

The exact task prompt is saved in
[the prompt log](../Prompts/2026-10-31-controller-input-compatibility-prompt.md).
The existing [.gitignore](../../.gitignore) ignores `AI-Logs/`; this prompt and
report are saved locally but will not appear in ordinary `git status`.
No ignore rules were changed, and nothing was staged, committed or pushed.

## Historical input architecture (before permanent aliases)

- [Program.cs](../../src/Tempest.Game/Program.cs) creates the Raylib window,
  disables/captures the cursor, targets 60 FPS, and routes one simulation
  step per frame. Raylib manages window events through the draw/frame loop.
- [InputSystem.cs](../../src/Tempest.Game/Systems/InputSystem.cs) uses
  `IsKeyDown(LeftControl/RightControl)` OR `IsMouseButtonDown(Left)` for held
  Fire. There is no separate controller API. The user's working Fire button
  therefore reaches an accepted path, but that observation alone does not
  distinguish which path.
- Spinner movement is `GetMouseDelta().X`; held Left/Right arrow keys are
  additional movement controls. [TempestGame.UpdatePlayer](../../src/Tempest.Game/TempestGame.cs)
  applies the existing spinner sensitivity and arrow movement speed.
- Super Zapper is `IsKeyPressed(Space)` OR
  `IsMouseButtonPressed(Right)`, translated into `PlayerInput.ZapPressed`.
  The OR already coalesces those two sources into one boolean per frame.
  `RunOneStep` retains its existing active-Playing-state activation gate
  and calls the existing `SuperZapper.TryActivate` implementation.
- Restart is `IsKeyPressed(R)`. The main loop only supplies it in Game Over
  or Level Complete. Existing reset/replay/campaign behavior remains owned
  by `TempestGame.RunOneStep`; no action semantics were edited.
- Quit is Raylib's default Escape exit key or window close, through
  `WindowShouldClose()`. Tempest does not translate Escape into a simulation
  command or override the exit key.
- Death freezes and non-interactive exit travel ignore ordinary player input;
  the existing spike-exit exception is unchanged. A Space press outside the
  accepted gameplay phase is not a zap request to the active simulation.
- Production code does not consume `GetKeyPressed()` or `GetCharPressed()`
  and contains no character-to-action mapping, Raw Input hook, custom Win32
  keyboard handler or gamepad routing.

## Established framework evidence and limits

The project uses Raylib-cs 8.1.0. Its version-matched bindings declare Raylib
6.0 and package native Raylib 6.0. The actual window smoke test also printed
`Initializing raylib 6.0`, `Platform backend: DESKTOP (GLFW)` and
`PLATFORM: DESKTOP (GLFW - Win32): Initialized successfully`.

Public version-matched sources inspected:

- [Raylib-cs v8.1.0 bindings](https://github.com/raylib-cs/raylib-cs/blob/v8.1.0/Raylib-cs/interop/Raylib.cs)
  and [native package version](https://github.com/raylib-cs/raylib-cs/blob/v8.1.0/Raylib-cs/Raylib-cs.csproj).
- [Raylib 6.0 rcore.c](https://github.com/raysan5/raylib/blob/6.0/src/rcore.c#L3806):
  `IsKeyPressed` compares previous-frame state 0 to current-frame state 1.
  `GetKeyPressed` and `GetCharPressed` consume separate key-code and Unicode
  queues, respectively (lines 3871 and 3893).
- [Raylib 6.0 GLFW backend](https://github.com/raysan5/raylib/blob/6.0/src/platforms/rcore_desktop_glfw.c#L2056):
  `KeyCallback` updates down/release state and queues GLFW press events.
  Repeat is a separate per-frame flag. `CharCallback` independently queues
  Unicode codepoints and does not define action/release semantics.
  `PollInputEvents` resets queues and copies previous key states before
  processing the next events (line 1258). Escape press marks window close
  in the key callback.

Ordinary keyboard Space is intended to work when Raylib exposes a Space
down edge and the game accepts active player input. The code explicitly
passes that edge to the existing zap action.

Two distinct framework-supported possibilities explain why text production
alone cannot establish action compatibility:

1. Text and key events are separate signals. A text-producing input need
   not provide the particular Space key-state transition Tempest samples.
   Even a Notepad Space character does not prove the Raylib window will
   receive a `CHAR_QUEUE U+0020`, let alone a Space press edge.
2. If a key's press and release are both processed in the same Raylib event
   poll, final current state can be up with previous state also up. Then
   `IsKeyPressed` is false even though `GetKeyPressed` queued the press.
   This follows from the inspected code, but whether the controller emits
   such short pulses is unknown.

Focus and the game's input phase are also conditions to check, not established
causes. Nothing here identifies how the controller creates Windows input.
Notepad output and On-Screen Keyboard reactions do not establish USB HID,
scan codes, Unicode injection, posted messages or driver implementation.
No inference about those mechanisms is used as a proposed fix.

## Diagnostic-only changes

- [InputDiagnostic.cs](../../src/Tempest.Game/Debug/InputDiagnostic.cs) adds
  a standalone opt-in `--input-diagnostic` window. It uses the same installed
  Raylib framework, 800x600 dimensions, cursor capture and 60 FPS target.
  It never constructs a game or runs simulation/actions.
- [DebugController.cs](../../src/Tempest.Game/Debug/DebugController.cs) parses
  the exact, case-sensitive flag. Program branches to it before ordinary
  game/window construction, after existing scenario-conflict validation.
  Launch the diagnostic alone; other valid gameplay options are unused.
- Every frame observes all declared positive Raylib keyboard keys:
  `STATE` records down, pressed, released and repeat flags when they change
  or have an edge/repeat. Both key and Unicode queues are drained and logged
  independently. Key queue lines include the same sampled down/pressed flags
  to expose a queued press that the current gameplay path would miss.
- Mouse-button state/edges are also recorded to identify alternate Fire
  or action signals. Focus changes and two-second held-key heartbeats help
  distinguish idle/no observable button event from focus loss.
- Console output and a recent-line window display show frame IDs and elapsed
  time. The live frame counter confirms ongoing polling. Unicode is logged
  as numeric codepoints (`U+0020` = Space), making invisible text reportable.
- Logs intentionally observe multiple paths without activating anything.
  There is no new production routing, character repeat handling or duplicate
  activation opportunity. Existing production Space/R/Escape/Fire/mouse/
  spinner controls and press-edge semantics are untouched.
- [README.md](../../README.md) documents this as DEVELOPMENT ONLY.
  The behavior specification, Reference/incitti, gameplay implementation,
  progression, Level 3/directed geometry and Tanker/C2 behavior were not edited.

This is the smallest diagnostic boundary that can compare sampled keys,
queued key presses and text without changing action routing or draining
production queues. It observes what the framework exposes, not every raw
Windows event, USB signal or every within-frame release transition.

## Validation

SDK: 9.0.304; tests: xUnit v2 via Microsoft.NET.Test.Sdk/VSTest.
No global.json, Directory.Build.props or Directory.Packages.props was found.

```powershell
dotnet build TempestCSharp.slnx --no-restore
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~InputDiagnosticTests|FullyQualifiedName~SuperZapperTests|FullyQualifiedName~LevelProgressionTests'
dotnet build src\Tempest.Game\Tempest.Game.csproj --configuration Release --no-restore
git diff --check
```

- Debug solution build: passed, zero warnings/errors.
- Focused tests: 50 passed, zero failed/skipped, including two new flag/
  scenario-validation tests and existing zap/restart/progression regressions.
- Release game build: passed, zero warnings/errors. DEVELOPMENT ONLY means
  explicit diagnostic launch, not a change to normal Release gameplay.
- Actual native window smoke: correct diagnostic title, responsive window,
  focused heartbeat at frame 1 and about two seconds, advancing frames,
  graceful close through WM_CLOSE to this test's own window, final CLOSED
  log and native "Window closed successfully", process exit code 0.
- The first smoke wrapper using PowerShell Start-Process reported a null
  ExitCode despite successful shutdown output. This was a harness result,
  not an observed app failure. Repeating with a direct
  System.Diagnostics.Process handle verified exit code 0 and empty stderr.
- `git diff --check`: passed. Git emitted only existing LF/CRLF conversion
  warnings for modified tracked text files, not whitespace errors.

No physical USB input was simulated or claimed tested. The automated tests
cover only deterministic flag parsing and existing headless actions; the
native smoke verifies diagnostic lifecycle, not controller compatibility.

## Historical diagnostic procedure (physical test now completed)

From the repository root in PowerShell:

```powershell
dotnet run --project src\Tempest.Game -- --input-diagnostic |
    Tee-Object -FilePath "$env:TEMP\Tempest-controller-input.txt"
```

1. Do not type private text while the diagnostic is open. Click its window
   and confirm `FOCUS focused=True` and an advancing frame counter.
2. Baseline with the ordinary keyboard: tap Space, R and Enter; hold Ctrl
   for two seconds and release. These do not invoke game actions here.
3. Test controller P, Enter, Space, Ctrl, Alt, Shift, Z, X, C, V, 1, 5 in
   that order. For each, tap twice a second apart, then hold two seconds
   and release. Wait three seconds before the next button. Do not move
   the spinner or press other buttons simultaneously.
4. Optionally test small red left, then small red right using the same
   procedure; identify them by physical position, not an assumed mapping.
5. Close with ordinary keyboard Escape or window X. Escape can terminate
   before its own key event is logged because the default exit key is kept.
6. Report the captured file with the physical-button/order notes. Include
   all STATE/KEY_QUEUE/CHAR_QUEUE/MOUSE lines, frame/time values, focus/
   heartbeat context, buttons with no observable output, and hold/repeat
   behavior. For an early close, identify which button caused it.

If no button event appears, retain the surrounding focused heartbeats and
note the physical button tested during that interval. Text-only signals,
key-queue-only pulses, ordinary held edges, repeated characters, multiple
paths or a different key code imply different next steps; none is assumed.

Once results establish usable signals and their press/release/repeat
behavior, choose additive mappings with Neal and coalesce overlapping
signals into the existing action booleans while preserving edge semantics.
If this window exposes no usable signal, a narrower framework/Windows
investigation would be justified then. Third-party remapping software is
not required for this diagnostic and has not been recommended as a fix.
