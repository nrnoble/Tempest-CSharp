# Headless Recorded-Playback Runner — Phase 3A Acceptance Closeout

Date: 2026-11-07

## Starting Git state

- Repository: `C:\Users\nrnob\source\repos\Tempest`.
- Branch: `master`.
- Starting `HEAD` and `origin/master` were both
  `358fca9229153e4f0fefe51fbedd711696095955`
  (`Add visible recorded playback controls`).
- The index was clean. The only worktree changes were the expected four
  modified and three added Phase 3A files.
- The complete supplied task was archived verbatim as
  `AI-Logs/Prompts/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-prompt.md`.
  Source and archived SHA-256:
  `A2E712C4435536BEEEDE342B893CF8FB436F021329352D7867BC35F6EF88C5A1`.

## Manual acceptance and results

**HEADLESS RECORDED-PLAYBACK RUNNER PHASE 3A: MANUALLY ACCEPTED.**

Neal reported testing real recordings under ignored
`Temp\GameplayRecordings\`; those disposable recordings were not staged,
copied, or committed.

- `testlevel5-3.json`, requested boundary 4820: achieved exactly 4820 of
  7260, leaving 2440 frames; `RequestedBoundaryReached`. The reported state
  was Level 7 / Playing, score 11894, 10 lives, player lane 3, and two Super
  Zapper charges. Recorded startup was Level 5, 12 lives, rim-speed multiplier
  1.
- The same recording to completion: 7260/7260, zero frames remaining,
  `RecordingCompleted`; Level 7 Complete, score 21805, 8 lives.
- Boundary 999999: rejected with exit code 5 and the valid range 0 through
  7260.
- `level1.json`: 797/797, `RecordingCompleted`; Level 2 Approach, score
  1725, 20 lives.
- `levels1-7.json`: 12761/12761, `RecordingCompleted`; Level 7 Complete,
  score 39545, 15 lives.
- Neal observed no perceptible delay for the 12761-tick recording in ordinary
  use. No timing instrumentation or formal benchmark was used; none was
  required.

No interactive game was launched and no additional recording or manual test
was performed.

## Runner and CLI contract

- `HeadlessGameplayPlaybackRunner` accepts a validated recording or path and
  delegates to the same `GameplayPlaybackSession` used for visible playback.
- Boundary 0 is restored startup before frame 0. Consuming frame N produces
  boundary N+1. `RunToBoundary(N)` executes sequentially to exactly N and does
  not consume frame N. Negative, over-duration, and earlier-than-current
  targets are rejected, never clamped.
- `RunToCompletion` consumes every recorded frame, including after non-Playing
  states, and reports zero remaining frames plus `RecordingCompleted`. It
  does not accept live input or hand off to live controls.
- CLI syntax:

  ```powershell
  dotnet run --project src\Tempest.Game -- --headless-playback <recording-file>
  dotnet run --project src\Tempest.Game -- --headless-playback <recording-file> --boundary N --json
  ```

- Human-readable output is the default. `--json` serializes an explicit
  camel-cased diagnostic schema with requested/achieved boundary, duration,
  remaining frames, completion reason, bounded gameplay state and stably
  ordered Flipper diagnostics; it does not serialize internal object graphs.
- Exit codes: 0 success; 2 invalid command/incompatible option; 3 recording
  load or validation failure; 4 simulation failure; 5 out-of-range boundary.
- The API does not suppress simulation/observer exceptions. The CLI formats
  simulation errors with full exception details and returns nonzero.
- The runner uses `SilentAudioEvents`, starts no Raylib window, audio device,
  renderer, input poll, or frame pacing, and reads recordings without
  modifying them.
- The existing strict recording loader and v1/v2 schemas are unchanged. V1
  retains Level 1 / three lives / speed 1.0 defaults; v2 retains its explicit
  startup metadata.

The CLI gives VS Code Copilot a project-owned boundary/state interface for the
same recording Neal can scrub visibly. Separate runs at N-1/N/N+1 can be
compared to identify a behavior and motivate a focused typed regression test.
This is not arbitrary process-memory inspection.

Phase 2 remains unchanged: Space pause/resume; Right +1 tick; native held-Right
repeat; Shift+Right +5 and repeated groups; inert Left/Shift+Left; paused
boundary overlay; and normal live-input handoff.

Phase 3B backward visual scrubbing is next, using fresh startup followed by
deterministic replay to the earlier boundary. Do not reverse simulation
mutations. Snapshots remain deferred unless practical Phase 3B use proves
replay-from-start latency unacceptable. The deferred enemy-destruction visual
gap is unchanged: a C# Blaster-hit enemy disappears immediately, while Neal
observes the arcade briefly showing a small explosion.

## Files and validation

Production files:

- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
- `src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs`

Test file:

- `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`

Documentation:

- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`

Automated validation:

- Focused command, `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~HeadlessGameplayPlaybackTests"`:
  **26 passed, 0 failed, 0 skipped**.
- Neighboring selected regressions covering playback/session, recording,
  visible controls, input, direct startup, death/re-entry, progression,
  campaign soak, Level 5/7, ledgers/simulation, Super Zapper, and audio:
  **568 passed, 0 failed, 0 skipped**. This selection includes the focused
  tests.
- `dotnet test TempestCSharp.slnx`: **931 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check` and `git diff --cached --check`: passed. Git printed
  only its README LF-to-CRLF advisory.

The observer-exception test verifies API exceptions propagate rather than
returning partial success. There is no dedicated safe fault-injection seam
for forcing the CLI's simulation-failure exit-code-4 branch; that branch was
reviewed in source for full exception formatting and nonzero return, but was
not dynamically triggered. The headless API has no live-input provider or
handoff path; completion and exact-frame behavior were exercised by tests.

## Staging, commit, and push

The complete staged list contained only:

1. `README.md`
2. `TEMPEST-BEHAVIOR-SPEC.md`
3. `TEMPEST-DEVELOPMENT-NOTES.md`
4. `src/Tempest.Game/Program.cs`
5. `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
6. `src/Tempest.Game/Debug/HeadlessGameplayPlaybackRunner.cs`
7. `tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs`

The staged patch was 7 files changed, 1049 insertions, 8 deletions. No
unrelated file, recording, `Temp`/`TEMP`, video, `bin`/`obj`, reference,
AI-Logs, or Git configuration content was staged.

- Commit message: `Add headless recorded playback runner`
- Commit: `a028cdc4156ea5d0fbccd660e4fc427226daa390`
- Normal push succeeded: `358fca9..a028cdc master -> master` on the existing
  `origin` remote. No force-push, branch creation, or remote reconfiguration.
- Final `git status --short --branch`: `## master...origin/master`.
- Local `HEAD` and `origin/master` both equal
  `a028cdc4156ea5d0fbccd660e4fc427226daa390`.

No Phase 3B or backward navigation, rewind, snapshots, recording-format
change, Phase 2 visible-control change, enemy-explosion implementation,
Level 8/9 work, benchmark, force-push, remote change, or unrelated gameplay
work occurred.
