# Start-level continuing campaign: partial investigation

Status: BLOCKED pending a decision on conflicting requirements.

## Scope and starting state

The task requests a production-only `--start-level` option, preserving existing
`--level` behavior, with distinct isolated and continuing semantics. The
starting branch was master at d1544654e780c677c852f38dc6bf1e2045900f6f,
tracking origin/master. The pre-existing modified Level 12 closeout report and
untracked materials were left untouched. The paired prompt is
`../Prompts/2026-11-23-start-level-continuing-campaign-prompt.md`.

## Findings

- Production `--level N` already starts a fresh game and enables natural
  progression. `TempestGame` sets `naturalProgressionEnabled` for
  `startingProductionLevel`, and `CompleteSuccessfulExit` advances using
  `LevelProgression.GetNextProductionLevel`.
- The existing README and behavior specification explicitly document onward
  progression and identify the direct-start feature as manually accepted.
  The terminal isolated modes are the separate `--levelN-test` scenarios.
- The production chain currently skips diagnostic Level 10, advances Level 11
  to Level 12, and ends at Level 12; Level 15 is direct-only.
- `ResetGame` currently returns normal production starts to Level 1, including
  after Game Over or a terminal production level. Diagnostic Level 10 is an
  exception.
- Gameplay startup metadata uses the same direct production start and seeded
  initialization for recording and playback. Changing `--level` semantics
  would therefore also require careful compatibility decisions for recordings.

The requested proof that `--level 11` is terminal cannot coexist with
preserving its existing progression behavior. The user must decide whether to
preserve the accepted behavior or authorize changing it and clarify recording
compatibility.

## Changes and validation

Only the paired task prompt and this partial report were created. No project
source, tests, configuration, or durable documentation were changed. No tests
or build were run; implementation and required validation are BLOCKED pending
the decision. Interactive gameplay was not launched. Manual acceptance remains
outstanding.
