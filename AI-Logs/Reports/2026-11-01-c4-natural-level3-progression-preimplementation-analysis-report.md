# C4 natural Level 3 progression pre-implementation analysis

## Disposition and checkpoint

**GO for one bounded C4 implementation checkpoint.** C3 is complete at
`e76d0b0 Complete direct Level 3 mixed-wave access (C3b)`: the existing
`LevelConfiguration.Level3Mixed` has automated lifecycle coverage and direct
graphical access, and Neal accepted the visible C# C3 behavior. C4 can now
wire natural Level 2 completion into that same model without reopening C3.

The required prompt completion marker was present. This analysis changed no
production source, tests, tracked documentation, reference material, or Git
history; no tests/build were run. HEAD is `e76d0b0`. The worktree is not
literally clean: `git status --short` reports an existing untracked `TEMP/`
directory. It was left untouched. The requested report is under ignored
`AI-Logs/Reports/`.

## Current progression architecture and exact gap

Normal construction selects `LevelConfiguration.Level1` and enables
`naturalProgressionEnabled`; explicit development flags and
configuration-based construction disable it. `LevelProgression` currently
maps only the exact Level 1 singleton to `Level2Square`; every other input,
including Level 2, returns `null`.

On a cleared wave, `CheckForLevelComplete` begins the configured exit. The
game does not replace the level at wave clear or during `ExitDelay`.
`UpdateExitZoom` is the successful-exit commit point: after standard exit
travel reaches `Player.Depth < 10`, it installs a next level only when
natural progression is enabled, the state is `ExitZoom`, the active
configuration is exactly Level 1, and the resolver returns a next level.
Otherwise it marks the active level `Complete`. Thus natural Level 2 is
terminal for **two coupled reasons**: the resolver has no Level 2 successor,
and the commit path independently hard-codes Level 1 as the only eligible
source. `RunOneStep` and `VectorRenderer.DrawHud` also encode the current
natural Level 2 terminal/restart behavior.

`InitializeFreshWave` already supplies the required level boundary: it
constructs a new `Level`, makes it active, restores projection, resets
death/exit/hurry-up state and Super Zapper, resets the player to the selected
configuration's start, clears shots/bullets/enemies/pending cargo and
level-specific runtime accounting, and initializes the fresh wave. It does
not reset score or lives. Ordinary death instead reconstructs the current
Level in place; that existing C3 behavior must remain unchanged.

`Program` already updates the normal window title when the displayed level
changes. Its no-flag route is the normal campaign; `--level2-test` and
`--level3-test` select direct configurations with natural progression
disabled. The exact model is already present: C4 must use
`LevelConfiguration.Level3Mixed`, not add another Level 3 configuration or
copy C3 behavior.

## Narrowest C4 design

1. Extend `LevelProgression.GetNextProductionLevel` by one explicit
   identity-based edge: `Level2Square` to `Level3Mixed`. Keep Level 3 terminal
   (`null` successor); do not infer progression from an index or create a
   generic cycle.
2. At successful standard exit commit, ask the resolver for the successor
   under the existing natural-progression and successful-`ExitZoom` guards.
   Remove the separate Level-1-only eligibility test. This gives both
   campaign edges the same commit point and fresh-wave path; direct
   development scenarios remain ineligible because their natural-progression
   flag is false.
3. Update natural-campaign terminal/restart handling from Level 2 to terminal
   Level 3. Successful natural Level 3 completion remains `Complete`; R then
   starts a fresh Level 1 campaign with reset score and configured starting
   lives. Keep direct Level 3 replay distinct: `--level3-test` replays its
   launch configuration under its current development semantics.
4. Change the normal terminal HUD from the obsolete Level 2 / “NEXT LEVEL NOT
   IMPLEMENTED” message to a Level 3 completion/new-campaign message. Preserve
   direct test completion labels. Program's runtime title logic already
   follows displayed identity; its introductory comment saying normal play
   ends at Level 2 should be corrected, but no CLI/runtime routing change is
   needed.

No Level 4, repeating cycle, new bonus, new entry/approach sequence, or C3
gameplay adjustment is warranted.

## Level 3 fresh-entry state

| State | Expected on natural Level 3 entry | Boundary |
|---|---|---|
| Identity/rendering | Exact `Level3Mixed`; displayed level 3, progression index 2, plus board 2 / palette 0; normal HUD/window identity, not a development-test label | Existing configuration and renderer |
| Score/lives | Carry their current Level 2 values unchanged | Existing fresh initializer leaves both intact |
| Player | Alive/playing at configured lane 7, fraction 0.5, depth 400 | Fresh initialization resets position and death ticks |
| Shots and enemies | No old shots, enemy bullets, Flippers, Tankers, or pending cargo activation; no stale Level 2 entities | Existing fresh initializer clears runtime collections |
| Eggs/wave | Four configured initial Eggs; 15 original units total, 11 in reserve; new `Level3WaveLedger`, zero initial cargo; C3's existing randomized selection/scheduling continues unchanged | Existing `Level3Mixed` and C3 lifecycle |
| Level counters/timers | Fresh Level counters, spawn/hatch timing, exit delay, shot timer, death ticks, hurry-up state and C3 identity accounting | Existing `Level` construction and fresh initializer |
| Super Zapper | Two charges and cleared flash state | Existing fresh-wave reset; no new charge display |
| Projection | Restored baseline for the first Level 3 frame | Existing fresh initializer |

This is a new Level, not a life reconstruction. C4 should assert fresh ledger
identity/state and configured initial wave, not retune the randomized C3
selection or require manually observable RNG internals.

## Super Zapper evidence and policy

- **ARCADE VERIFIED:** Neal confirmed two usable charges at natural Level 2
  entry. C3b manually accepted visible Super Zapper behavior in the direct
  Level 3 C# scenario, not a natural Level 2-to-Level 3 entry.
- **REFERENCE VERIFIED:** the earlier Incitti/source analysis records two
  charges on new-level setup (`superzapper = 2`).
- **PROJECT DECISION:** natural Level 3 uses the same fresh-wave boundary as
  Level 2 and therefore starts with two usable charges and no active flash.
  This is consistent with existing C# behavior and source evidence, but is
  not a new claim of arcade-observed Level 3 transition behavior.

No charge HUD or change to C3 target/timer semantics is needed.

## Terminal Level 3, restart, and development isolation

Natural Level 3's existing C3 completion path should finish its accepted
ordinary/all-rim exit travel, then remain terminal because its resolver
successor is null. Do not implement Level 4 or repeat levels. This terminal
policy is a **PROJECT DECISION**, not a claim that Atari Level 3 is terminal.

- Ordinary Level 3 death retains the same Level/configuration and invokes the
  existing C3 reconstruction policy; score and remaining lives follow
  established behavior.
- R after successful natural Level 3 completion starts a fresh normal
  campaign at Level 1 and resets score/lives, matching current normal
  terminal-Level-2 conventions.
- Game Over restart in a normal campaign continues to use the original launch
  configuration (Level 1), resetting score/lives.
- `--level2-test`, `--level3-test`, and the Level 3 geometry diagnostic remain
  isolated development paths. Direct Level 2 remains terminal/replayable;
  direct Level 3 remains terminal/replayable with its existing score/life
  behavior; geometry diagnostic remains on `Level3Plus`. None enables
  natural progression.

## Between-level approach presentation

**Recommendation A:** keep C4 on the project's existing immediate
successful-exit transition. Do not gate C4 on the Atari between-level
approach. It is an independently documented, gameplay-relevant fidelity and
presentation milestone: Atari shows a distant tube that grows during
approach, with spinner-controlled Blaster movement before combat. Its timing,
projection, and engagement details remain unmeasured. Keep it explicitly
deferred as a future milestone; do not silently drop it or bundle it with
this narrow progression edge.

## C3 protection and regression tests

C4 consumes the accepted C3 model. Do not change Level 3 composition,
selection/ledger semantics, Egg scheduling, six-active-Tanker policy, firing,
cargo ownership/routes, Super Zapper target/timer rules, death/reconstruction,
completion/all-rim behavior, HUD visibility, geometry, controller behavior,
or default Flipper firing.

Required deterministic coverage:

- Keep normal Level 1 -> Level 2 successful-exit behavior and its no-early-
  commit cases; assert resolver sequence Level 1 -> Level 2 -> `Level3Mixed`,
  then terminal `null`.
- Complete the natural Level 2 exit and assert the fresh Level 3 config,
  playing state, displayed identity, score/lives carry through both edges,
  player start, fresh Level3 ledger, 4 Eggs / 11 original reserve, fresh
  counters, and absence of stale shots/bullets/enemies/pending cargo.
- Consume Zapper charges and create flash state before Level 2 exit; assert
  two charges and zero flash at Level 3 entry.
- Verify natural Level 3 completion remains terminal and restart begins a
  fresh normal Level 1 campaign with reset score and starting lives. Reuse
  the public C3 completion path; do not use reflection or inaccessible-state
  mutation.
- Retain direct `--level2-test` terminal/replay behavior, direct
  `--level3-test` selection/replay/Game Over behavior, and geometry diagnostic
  isolation. Confirm normal Game Over restart from a later campaign level
  starts Level 1.
- Keep all existing C3, Level 1/2, renderer, input, and configuration tests
  green. Update existing assertions that explicitly expect Level 2 to have no
  successor or natural Level 2 to be terminal.

## Practical manual acceptance

Use the normal no-flag campaign, for example
`dotnet run --project src/Tempest.Game -- --lives 10`. The manual target is
the natural Level 1 -> Level 2 -> Level 3 route, not a repeat of C3 gameplay:

1. Start on Level 1; earn score and optionally lose a life.
2. Complete Level 1 normally. Confirm immediate square Level 2 entry, correct
   displayed identity, and unchanged score/lives.
3. Complete Level 2 normally. Confirm the same immediate transition (no
   terminal pause), plus-board Level 3 identity/HUD and normal window title.
   The deferred Atari approach is not a C4 failure.
4. Confirm visible score/lives carry, fresh player-at-rim entry, fresh mixed
   wave (including both Flippers and Tankers over play), and two usable
   Super Zapper charges. Do not require hidden identity or RNG checks.
5. Spot-check a Tanker and ordinary Flipper encounter only for obvious
   progression regressions; C3's detailed acceptance is already complete.
6. If practical, complete Level 3 and confirm terminal Level 3/new-campaign
   restart, then check Game Over restart. Separately smoke-test the existing
   direct Level 2 and Level 3 development commands if concerned about
   isolation.

Classify this as manual acceptance of visible C# behavior, not arcade
verification unless Neal separately validates it in the arcade.

## Prior analysis reconciliation

`2026-10-31-level3-tanker-progression-analysis-report.md` predates C1/C1.5,
C2, and C3. Its C4 direction remains useful, but its readiness conclusions
must be updated:

- **Still valid:** progression belongs after successful exit travel; carry
  score/lives; use a fresh Level; keep direct test modes terminal; keep
  Level 3 terminal without Level 4; defer bonus/presentation unless separately
  authorized.
- **Superseded:** its “not a safe single milestone” and C1/C2/C3 prerequisite
  blockers. The required geometry, firing policy, mixed-wave accounting,
  direct production access, and C3 manual acceptance now exist.
- **Now directly implementable:** the existing `Level3Mixed`, fresh-wave
  initializer, successful-exit boundary, and C3 test coverage provide the
  required integration points.
- **Do not carry forward literally:** old expected-file references to a
  generic `Level3`/future configuration are obsolete; use `Level3Mixed`.
  Program's displayed-level update already works dynamically, so only its
  stale terminal comment needs correction. Also reconcile the final
  `TEMPEST-DEVELOPMENT-NOTES.md` statement that C3b graphical acceptance is
  “pending”; the C3b report and behavior spec record that acceptance as
  complete.

## Likely files and checkpoint

Likely implementation/test files:

- `src/Tempest.Game/Levels/LevelProgression.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Program.cs` (comment only)
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- Existing configuration/geometry tests whose assertions encode terminal
  Level 2
- Potentially focused renderer/restart tests if an established test seam
  already covers the terminal presentation

Related documentation to reconcile after implementation/manual acceptance:
`README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`, and
`TEMPEST-BEHAVIOR-SPEC.md`. No Level 3 configuration, wave ledger/selection,
enemy, input, reference, or geometry production file should require changes.

**One checkpoint is recommended.** The authorized behavior is one progression
edge, common fresh-level initialization, terminal/restart updates, focused
regression tests, and directly related documentation. Splitting it would
create no independent architectural boundary. Keep the approach presentation
separate.

**Implementation sequence:** add the explicit resolver edge; generalize only
the successful natural exit commit; update natural terminal restart and HUD
semantics; extend progression/restart tests; reconcile docs; run focused
progression/configuration/Level 3/render tests, then the full solution build
and test suite; perform Neal's natural-transition manual checklist; report
the exact evidence classification. Do not retune C3.

## Risks and GO / NO-GO

Main risks are forgetting the independent Level 1-only exit guard, leaving
natural Level 2 terminal checks in restart/HUD/tests/docs, or accidentally
making direct Level 3 access drive normal behavior. Cover all three with
focused tests. No C3 incompatibility is apparent: `InitializeFreshWave`
already initializes the exact production-shaped mixed configuration and
resets its ledger, entities, position, projection, and Zapper state.

**GO:** implement C4 as the single bounded checkpoint above.
**NO-GO:** adding Level 4/cycling or coupling the deferred approach sequence.
No implementation blocker is known. The only baseline discrepancy is the
pre-existing untracked `TEMP/` directory, preserved untouched.
