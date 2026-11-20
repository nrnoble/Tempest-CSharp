# C3b Level 3 mixed-wave access: pre-implementation analysis

Baseline: `233bf25 Document Level 3 mixed-wave architecture`
(`2b2e50c Implement headless Level 3 mixed-wave lifecycle`).
Scope: read-only planning for direct graphical access and manual acceptance.
No production source or tests were changed.

## Findings

C3a already provides the production-shaped `LevelConfiguration.Level3Mixed`,
sharing the exact plus-board definition with `Level3Plus` while selecting the
mixed Flipper/Tanker composition. The existing
`TempestGame(LevelConfiguration)` path initializes and runs that real model:
randomized production Egg scheduling/selection, Tanker firing and cargo,
identity ledger, Zapper, death reconstruction, all-rim, completion and terminal
replay. A direct configuration launch disables natural progression and records
the configuration for replay. `Level3Plus` remains the Flipper-only geometry
diagnostic.

The existing graphical path is already suitable. `Program.cs` owns one
`TempestGame`, one `InputSystem`, and one `VectorRenderer`. `VectorRenderer`
already draws the current configuration's tube, Eggs, Flippers and Tankers,
and already has the production enemy-bullet glyph/projection. No alternate
Level 3 renderer, game loop, or model is needed. Keyboard, mouse, firing,
Super Zapper input, death freeze, and end-state R/P replay routing are generic
and reusable.

The direct launch should pass `LevelConfiguration.Level3Mixed` through the
existing configuration-based construction path, not `Level3Plus` and not a
test-only configuration. It remains terminal: no Level 2 exit/progression
change and no `LevelProgression` change.

### Visibility and HUD

- Production Tankers already receive ammo/timers and create real
  `EnemyBullet` instances without `--enemy-bullets-test`. However,
  `VectorRenderer.Draw()` currently draws those bullets only when that flag is
  set. The narrow fix is to retain the existing draw helper and show bullets
  for the mixed production configuration as well as for the existing opt-in
  flag, while preserving the existing exit-hidden policy. Do not change any
  firing mechanics.
- Eggs, Flippers, and Tankers already have production rendering. No new enemy
  glyphs or projection/geometry work is indicated.
- The generic HUD already displays configured level number, score and lives.
  The mixed level must not be presented as “FLIPPERS 15”: `RemainingEnemies`
  counts original supply, not the extra cargo obligations, and the generic
  active count omits Tankers. Reuse the ledger's unresolved original/cargo
  counts for accurate `ORIGINAL`, `CARGO`, and `ACTIVE` rows, counting Eggs,
  Flippers and Tankers as active. Do not expose identity IDs.
- `SuperZapperSystem` already implements two charges, the flash, and the
  established death/replay reset policy. Existing development notes explicitly
  say the UI has no charge counter or icons. Preserve that convention: show the
  existing flash, but do not add a charge HUD. Validate the charge lifecycle
  through model tests and observable use instead.
- Add a Level 3 mixed development title/label, analogous to the Level 2 and
  geometry diagnostic names. Keep the Level 3 display identity at 3 and mark
  direct access **DEVELOPMENT ONLY**. The Level 3 geometry diagnostic keeps its
  existing name and Flipper-only behavior.

### CLI and terminal behavior

Add `--level3-test` as one mutually exclusive scenario alongside
`--level2-test`, `--level3-geometry-test`, `--tanker-test`, `--spiker-test`,
`--fuseball-test`, and `--pulsar-test`. `--lives` and
`--enemy-bullets-test` remain composable; the latter may add the already
supported ordinary/cargo Flipper firing, but must not be needed to arm or show
production Tanker bullets. Preserve the existing standalone
`--input-diagnostic` precedence/compatibility rather than silently changing
that unrelated mode.

At normal Level 3 completion, R replays a fresh `Level3Mixed` wave while
retaining score and lives; after Game Over, R performs the established full
restart with score reset and configured starting lives. Death reconstruction
continues within the same Level 3 model. No terminal path may progress to
Level 4 or a campaign cycle.

## Likely changes

| File | Expected C3b work |
|---|---|
| `src/Tempest.Game/Debug/DebugController.cs` | Parse `--level3-test`; include it in scenario conflicts and messages. |
| `src/Tempest.Game/Program.cs` | Route the flag to `LevelConfiguration.Level3Mixed`; set the development title and preserve existing shared input/render loop. |
| `src/Tempest.Game/TempestGame.cs` | Only if needed to identify the direct mixed launch for presentation and enforce consistent scenario validation; retain the existing model, launch configuration and terminal semantics. |
| `src/Tempest.Game/Rendering/VectorRenderer.cs` | Make production mixed-wave bullets visible; correct mixed-wave HUD counts/labels and development identity without replacing existing entity drawing or Zapper visuals. |
| `tests/Tempest.Game.Tests/Level3TestModeTests.cs` (new focused file) | CLI/config selection, conflict/composable flags, rendering policy, HUD data policy, and direct terminal replay/restart regressions. Reuse existing Level 3 model tests rather than duplicating C3a lifecycle coverage. |
| `README.md` | Document the new direct command and distinguish it from the geometry-only diagnostic and natural progression. |
| `TEMPEST-DEVELOPMENT-NOTES.md` | Record implemented development access, actual render/HUD behavior, and manual acceptance results. |
| `TEMPEST-BEHAVIOR-SPEC.md` | Add only verified development-only access and genuinely observed manual outcomes, preserving the existing evidence classifications. |

`AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-report.md`
and the C3a implementation report are historical evidence; do not rewrite them.

## Should not require changes

Do not change `LevelConfiguration.cs`, Level 3 selection/scheduling/ledger or
`Level.cs`; `Level3Mixed` is already the requested model. Do not change
`LevelProgression.cs`, the Level 2 successful-exit condition, gameplay entity
firing/release/pivot code, `SuperZapperSystem`, `InputSystem`, projection
geometry, or `Reference/incitti`. `Level3Plus` stays a geometry-only
diagnostic. Do not add a natural Level 2-to-Level 3 edge, Level 4, or
campaign-cycle architecture.

## Automated gate before manual play

1. Cover parsing and every exclusive-scenario conflict; verify `--lives` and
   `--enemy-bullets-test` still compose.
2. Verify the launch selects the exact `LevelConfiguration.Level3Mixed`
   object, not `Level3Plus`; assert the model starts with its production supply
   and is non-progressing. Preserve existing geometry-diagnostic assertions.
3. Verify the bullet draw policy includes production Level 3 with the optional
   flag off, keeps ordinary default modes unchanged, and still respects exit
   hiding. Exercise the existing bullet projector/glyph with a real production
   Tanker bullet; do not infer visibility from firing-only tests.
4. Verify HUD inputs distinguish unresolved originals/cargo and count active
   Tankers as well as Flippers/Eggs, while score, lives, and displayed level
   remain correct. Preserve the absence of a Zapper counter.
5. Verify Level 3 completion R replay retains score/lives and rebuilds the
   mixed configuration; Game Over R resets score/lives; the direct mode does
   not enable progression. Retain the C3a death/reconstruction and Zapper
   lifecycle coverage.
6. Run focused C3b tests and build, then the repository's full regression gate
   before Neal's manual pass. These are future implementation gates, not tests
   run for this read-only analysis.

## Manual acceptance checklist for Neal

Launch `dotnet run --project .\src\Tempest.Game -- --level3-test --lives 10`.
Use fresh runs/replays as needed; the composition is randomized, so do not
require a particular Tanker count or exact random sequence.

- Confirm the window/HUD identify direct Level 3 development access, show
  level 3, score and lives, and draw the 16-lane plus board; move and fire
  normally.
- Observe incoming Eggs on the board, then production Flippers and Tankers.
  Observe multiple Tankers only when naturally selected; do not demand a
  deterministic composition.
- When a production Tanker fires, confirm its white bullet is visible and
  continues independently of its Tanker. In the default command, ordinary and
  cargo Flippers should remain silent.
- Observe a Tanker's natural release and, in another suitable encounter, its
  player-shot destruction release. Confirm two live cargo Flippers appear on
  distinct lanes and their established movement/pivots look correct.
- Use both Super Zapper charges. Confirm the visible flash and expected broad
  clearing/delayed effects; do not expect a charge counter or judge hidden
  timer/ledger details.
- If death occurs, confirm the Level 3 board returns to play without a crash
  or obvious duplicated/missing wave presentation; score/lives and visible
  Zapper behavior remain consistent with the established lifecycle.
- Where naturally reached, observe all-rim resolution and ordinary completion
  leading to exit; completion is terminal and does not enter another level.
- Press R after completion: confirm a fresh Level 3 wave, with score/lives
  retained. If Game Over is reached, R should return to a fresh Level 3 with
  score reset and configured starting lives.
- Note crashes, missing/invisible entities or bullets, clipped/overlapping
  presentation, incorrect title/HUD identity, or regressions in movement,
  scoring, firing, Zapper or exit.

Do not ask Neal to inspect hidden identity IDs, exact random-call order, ledger
conservation, or deterministic scheduling; those remain automated C3a concerns.
Optional Flipper-bullet behavior may be separately smoke-tested with
`--level3-test --enemy-bullets-test`; it is not the default acceptance run.

## Risks, checkpoint, and sequence

Primary risks are routing to the Flipper-only diagnostic by mistake; leaving
production bullets invisible despite C3a firing; an inaccurate original-only
HUD count or missing active Tankers; weakening CLI conflict checks; accidentally
changing the geometry diagnostic, Zapper lifecycle, or normal Level 1/2
silence; and introducing progression while making direct access terminal.
Random selection may require more than one run to observe Tanker-specific
events. Manual results must not upgrade reference-derived composition/firing
inputs to arcade-verified facts; C# identity/lifecycle rules remain project
decisions and the CLI remains development-only.

Recommend **one bounded C3b checkpoint**: implementation, focused automated
coverage/build/regression gate, then manual acceptance and narrowly related
corrections. The integration spans only CLI, existing renderer/HUD and terminal
launch wiring; splitting it would add handoff/context without materially
isolating risk. If manual play reveals a model/lifecycle defect, explicitly
reopen the C3a gate rather than hiding a model change in C3b UI work.

Sequence: (1) add parser/conflict coverage and direct configuration routing;
(2) wire title/HUD and production bullet visibility with focused tests;
(3) verify replay/Game Over and no-progression behavior; (4) build and run
automated regression gates; (5) Neal performs the checklist; (6) update
documentation to actual implementation and accepted evidence, then checkpoint
separately. No implementation ambiguity blocks this sequence. The Zapper HUD
question is resolved by the existing no-counter convention; any request to
add charge icons would be a separate presentation decision.

## Checkpoint note

HEAD was `233bf25`, matching the requested baseline. `git status --short`
also showed an existing untracked `TEMP/` directory; it was not inspected,
modified, or removed. `AI-Logs/` is ignored by `.gitignore`, consistent with
the established analysis-report convention. No tests, commit, or push were
performed.
