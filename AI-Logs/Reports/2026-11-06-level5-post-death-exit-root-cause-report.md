# Level 5 post-death unexpected exit root-cause investigation

Date: 2026-11-06

## 1. Starting Git state

- Branch: `master`, tracking `origin/master`.
- Starting `HEAD` and `origin/master`:
  `11ddd630a3c6e01c18795658c9625280ca24ca8e`
  (`Extend recorded gameplay startup metadata`).
- The tracked worktree and index were clean; there were no unexplained
  nonignored untracked files. The configured remote is the existing `origin`.
- No reset, cleanup, restore, staging, commit, or push was performed.

## 2–4. Manual evidence and reproduction status

**NEAL-OBSERVED / ROOT CAUSE UNKNOWN.** The supplied control case is a fresh
Level 5 with no gameplay input: the stationary Blaster is destroyed, death and
re-entry complete, and subsequent no-input play continues. This can be repeated
for multiple deaths.

The supplied failure case is fresh Level 5, active play, Blaster death, normal-
appearing death/re-entry, then an unexpected exit within seconds without
post-re-entry keyboard/controller input. Level 5 can proceed to Level 6 when
the Blaster survives; Levels 1–4 deaths, ordinary entry into Level 5, and other
tested levels have not reproduced it so far. The symptom occurs without
record/playback. The console ends with `INFO: Window closed successfully` and
Neal has not observed an exception.

Copilot did not launch the game or reproduce either manual case. The exact
minimum pre-death action is unknown; “active play before death” is the only
observed discriminator. No A–M action-reduction experiment was run. Existing
headless simulation tests do not represent Raylib's window-close signal or the
observed application process exit, and no failing recording or runtime trace
was supplied. Adding an action-specific test without such evidence would test
a guessed cause.

## 5. Production termination-path inventory

The ordinary production loop in `Program.cs` ends only when
`InputSystem` reports either:

1. Raylib `WindowShouldClose()` (which can represent Raylib's Escape/window
   close handling), or
2. an Enter press edge.

The loop checks Raylib first and short-circuits the Enter poll if Raylib
already requests closure. The terminal game states (`GameOver`,
`LevelComplete`, death, and level transitions) do not end the application
loop; the loop continues rendering and accepts the existing restart behavior.
Playback completion returns to live input and is not an exit condition.
Recording saves after a normal loop exit and does not request one.

Other explicit top-level returns are startup/parser failures (`2`), handled
recording-save failure (`1`), the standalone input diagnostic's return, and
the final normal return (`0`). The input diagnostic is a separate opt-in
window loop. The production source has no `Environment.Exit`/process-exit
call, cancellation token, application quit flag, shutdown callback, or
application-managed secondary `Task`/`Thread`. The `SpikeCreated` callback is
synchronous bookkeeping, not a shutdown path.

## 6. Exception-handling audit

- There is no catch around production simulation, input, rendering, or the main
  loop. A managed exception there propagates out of `Program`.
- `Program` has filtered catches for selected file/setup errors before the
  loop and selected recording-save errors after it. Those catches print short
  messages and return `2` or `1`; they do not surround gameplay.
- `AudioSystem` catches construction failures only to dispose partially loaded
  sounds, then rethrows. It does not suppress the exception.
- `Program`'s `finally` restores the cursor and closes the audio device/window.
  `InputDiagnostic` also closes its window in `finally`.
- There are no empty catches or catch blocks that convert gameplay/render/input
  exceptions to successful exits. Cleanup can therefore run after a gameplay
  exception and Raylib can print its close message. A cleanup exception could
  itself complicate the final exception report; no such failure was observed.
- For an unhandled managed exception, the normal .NET host path should report
  the exception and terminate nonzero. No fatal exception was reproduced, so
  its actual console output and exact process code were not measured here.
- No application-managed background task/thread was found whose unobserved
  failure could initiate this shutdown.

Thus, gameplay exceptions are not silently caught by current project code.
Whether a particular user's console/environment might obscure runtime output
is not established.

## 7. Raylib close message

The literal `Window closed successfully` is present in the installed native
Raylib DLL, not in project source. `Program` calls `Raylib.CloseWindow()` from
`finally`; the separate input diagnostic does likewise. Raylib emits this
cleanup message while closing the native window. It does not report why the
loop ended, and its emission is not conditional on clean gameplay completion.
It is therefore compatible with both ordinary loop exit and exception
unwinding when cleanup reaches `CloseWindow()`.

## 8. Exit-code findings

- Invalid `--level 8` startup was run without opening the game window: it
  printed the supported range and exited **2**.
- Normal Enter and Raylib-window-close paths both leave the loop, complete
  cleanup, and reach `return 0` by source inspection. They were not interactively
  exercised; recording-save failure remains the explicit normal-shutdown
  exception that returns `1`.
- A fatal managed exception has no project catch and is expected to produce a
  nonzero .NET process result, but neither an exact fatal code nor a Level 5
  unexpected-exit code was captured.
- The new diagnostic reports the selected normal loop signal, not the process
  exit status. Neal's reproduction still needs the console/host exit status
  recorded.

## 9. Level 5 mutable-state inventory

Active gameplay can change player lane/position; shot cooldown, shots and score;
Super Zapper charges/effects; random-stream position; enemy identities and
species selections; Egg timing; Flipper movement/route phase; Tankers and
their latent/released cargo; Spikers; enemy bullets; destruction/rim-clear
bookkeeping; Spike heights/removal; Hurry-Up and pulse state. Which of these
changed in Neal's failing run is unknown. The Level 5-specific `$53`
Flipper/Spike policy can affect Flipper routing, but no evidence connects it
to shutdown.

After a life loss, active runtime entities are discarded, so their current
movement timers/phases do not resume. The same Level/configuration, score,
decremented lives, damaged persistent Spikes, Super Zapper state, random
stream, Hurry-Up/pulse state, and resolved wave outcomes are not generally
reset by the production death reconstruction. The ledger retains unresolved
original/cargo identity and committed species selection on Levels 4–6.

## 10. Death/re-entry trace

1. `KillPlayer()` is guarded by `Playing`, starts the 60-update death interval,
   changes state to `PlayerDying`, and clears player shots.
2. While dying, gameplay-entity updates freeze; the death presentation and
   display-only timers advance.
3. At the end of the interval, one life is deducted. Final-life loss enters
   Game Over; otherwise `ResetAfterDeath()` runs.
4. For Level 5, `UsesProductionLedger` selects the shared production path:
   validate ledger representation; reset player position and shot timer; clear
   shots, bullets, Flippers, pending cargo activations, Tankers, Spikers,
   Fuseballs, Pulsars, popups, and collision-order runtime entries; retain the
   existing Spike objects; rebuild unresolved identities and fresh Eggs on
   the same `Level`; set state to `Playing`; validate again.
5. Ledger terminal outcomes, unresolved IDs, cargo ownership, and selected
   unresolved species are retained. The next wave work uses newly constructed
   enemies and the existing Level 5 configuration/policy, not old Flipper
   program counters or object timers.

The before/after ledger validation can throw on an invariant violation, but no
Level 5 violation was reproduced. The player, wave, and level lifecycle do not
set an application quit flag.

## 11. Level 4/5/6 comparison

Levels 4–6 use the same `FlippersTankersAndSpikers` production composition and
production-ledger death/re-entry branch; no Level 5-specific reconstruction or
application-shutdown branch exists. Each starts with four Eggs and height-60
Spikes. Their supplies are 17, 20, and 22 respectively. Level 5 alone has the
cross board and `Level5SpikeSensitive` Flipper policy (`$53` project
translation); Level 4 is the binocular board and Level 6 the triangle. Level 6
also has different Flipper timing and an Avoid-Spikes wait threshold.

The Level 5 and Level 6 production tests cover life-loss state preservation and
ledger reconstruction; the Level 5 test also checks fresh Flippers use the
Spike policy. These tests are state-level checks, not an application-exit
reproduction. No Level 4/5/6 differential test established a failing-state
delta.

## 12. Active-play reduction and automated reproduction

No minimum trigger among movement, firing, Zapper use, enemy/Spike destruction,
score, cargo, enemy bullets, or rim arrival was established. No deterministic
automated application reproduction was achieved. The headless tests can advance
`TempestGame`, but the actual normal exit decision lives at the Raylib
`Program` boundary, and there is no automated Raylib-window test harness in the
repository. There is also no known failing seed/recording or demonstrated
simulation exception to reduce against.

The current Level 5/Level 6 and existing death/re-entry tests passed. Their
passing result does not prove that every active-play state is safe or explain
Neal's process exit.

## 13. Recording/playback diagnostic usefulness

Version 2 records the initial production level, lives, rim-speed multiplier,
seed, tick-indexed effective player inputs, and restart bits. A recording that
includes active Level 5 play through death could provide a deterministic
simulation path if the run reaches a normal loop exit and saves. Playback does
not record the Enter/window-close event or an application exit reason, and
playback hands back to live input after its recorded duration.

The save occurs only after normal loop termination. If a fatal exception
unwinds out of the loop, execution skips that save; no recording-save behavior
was changed. Recording/playback is not implicated by the live observation.

## 14. Root-cause assessment and hypotheses

**ROOT CAUSE NOT PROVEN.** The runtime termination mechanism in the failure
case is not yet known. Ranked remaining hypotheses:

1. **A normal Raylib `WindowShouldClose()` signal.** This is the most direct
   normal loop-exit path and could be caused by Raylib/window-system input.
   Against: the failure needs active Level 5 play before death and no
   post-re-entry controls are required; no event was logged before this
   diagnostic. The user's observation does not identify Escape, an OS close
   event, or any other cause.
2. **An unhandled exception during post-re-entry simulation or rendering.**
   `finally` would still close the window and can produce the same Raylib
   message. Active-play-mutated state and the Level 5 ledger are plausible
   investigation areas. Against: no exception was observed, project code does
   not catch gameplay exceptions, and the shared Level 4–6 reconstruction
   invariants pass existing tests. No exception/stack trace supports this
   hypothesis yet.
3. **Another external/native process termination.** No project source path
   establishes one; there is no supporting evidence. It remains possible only
   until exit status and console diagnostics are captured.

The `$53` policy, recording/playback, controller, Enter, and ordinary
death/re-entry are not established causes. No gameplay fix is recommended
without a captured exit reason or failing state.

## 15. Diagnostic change and recommended next step

Added a small development diagnostic: normal loop exit now prints
`Exit reason: WindowShouldClose` or `Exit reason: Enter`. It preserves the
original Raylib-first short-circuit and does not change the selected exit
signal, gameplay input, simulation, or exception handling. Unhandled managed
exceptions remain uncaught and visible through the normal .NET host path.

No root-cause correction is proposed. Neal should reproduce the failure with
the diagnostic and retain the full console output and process exit status.
If it reports `WindowShouldClose`, investigate the window/Escape event path. If
an exception appears, use its full type/message/stack/inner exception to trace
the precise failing state before proposing a correction.

For a later fix, regression tests should include the no-input control, the
smallest reproducible pre-death action sequence, post-re-entry simulation
without input, Level 4 and 6 neighbors, and the exact signal/exception path
that failed. Manual acceptance should repeat the control and failure cases on
Level 5, verify normal progression/death/re-entry, and check Levels 4 and 6
for regressions. Any gameplay change remains subject to Neal's review and
manual acceptance.

## 16. Changed files

**Production diagnostic only**

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`

**Test**

- `tests/Tempest.Game.Tests/InputSystemTests.cs`

**Project records**

- `AI-Logs/Prompts/2026-11-06-level5-post-death-exit-root-cause-prompt.md`
  (copied from the supplied attachment; source/archive SHA-256 both
  `DF3839E302B5531412577DE1D8DE55AC54507ED2F42A7C3C8E2807EE686B9937`)
- `AI-Logs/Reports/2026-11-06-level5-post-death-exit-root-cause-report.md`

No durable behavior specification was changed.

## 17. Validation

- `dotnet test TempestCSharp.slnx`: **872 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **succeeded, 0 warnings, 0 errors**.
- Invalid startup smoke test (`--level 8`): **exit code 2**; Raylib window was
  not initialized.
- Installed native Raylib DLL contains the exact close-message literal.
- `git diff --check`: passed.
- No interactive game launch or manual acceptance was performed.

## 18. Final Git state and scope confirmation

- `HEAD` remains `11ddd630a3c6e01c18795658c9625280ca24ca8e`;
  `origin/master` remains the same commit.
- Expected tracked worktree changes are the two production diagnostic files
  and one test file above; index remains clean. `AI-Logs/` is ignored.
- No root-cause gameplay fix, staging, commit, push, Level 8/9 work, or
  unrelated gameplay change occurred.
- Stop for Neal's review and diagnostic reproduction before implementing any
  root-cause fix.
