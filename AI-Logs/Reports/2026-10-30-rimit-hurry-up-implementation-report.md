# Incitti rimit / Hurry-Up Implementation Report

Starting checkpoint: `37535eb Implement Pulsar behavior and add behavior specification`.
The working tree was clean at inspection. Implementation is saved locally;
no commit or staging was performed. The agent performed automated validation
only; Neal's subsequent manual validation is recorded in section 9.

Scope: only the existing direct-hatched Fuseball and Pulsar development waves.
The behavior specification was deliberately unchanged during implementation.
Following Neal's manual validation and explicit approval to incorporate the
milestone, it was updated in the documentation-only follow-up on 2026-10-31.

## 1. Exact trigger and evaluation order

`TempestGame.HurryUp` is the sole game-owned latch. It activates only in
`--fuseball-test` or `--pulsar-test` when all these conditions hold:

1. `CurrentLevel.ReserveEggs == 0`.
2. `Eggs.Count == 0`.
3. At least one active enemy remains.
4. At least one active enemy has `Height < Level.Depth` (400).

The implementation's `Any` below-rim predicate over the two supported enemy
lists enforces both conditions 3 and 4; an empty list cannot qualify.
There is no separate global, random source, subsystem or command-line flag.

The current active simulation tick order is retained:

1. Advance the shared pulse clock.
2. Update player movement/firing and Fuseball animation.
3. Advance player shots and resolve shot collisions.
4. Advance enemy bullets.
5. Early completion check; a final shot kill can begin exit here and return
   without Egg or enemy updates.
6. Reserve Egg release.
7. Egg approach/roaming updates.
8. Hatch all ready Eggs, removing them from the active Egg list.
9. Flipper and Tanker updates, then activation of pending shot-released cargo.
10. Spiker, Fuseball and Pulsar updates. Hatchlings receive this same tick's
    enemy update.
11. Enemy/spike-to-player collision checks.
12. **Latch hurry-up from the resulting post-hatch, post-update state.**
13. Final completion check, using the existing kill-all policy.

A newly true latch is not visible to step 10 until the next active tick.
If the last approaching Egg started at height -10 with a fresh approach
counter, approach update 39 leaves it at height -2, counter 7, still an Egg.
Update 40 reaches zero, hatches/removes the Egg and updates its enemy before
latching. A Pulsar hatchling reaches height 1; a Fuseball remains at height 0
with lateral counter 1. Tests assert false on tick 39 and true on tick 40,
not merely eventual activation.

Additional timing tests establish:

- An existing climbing Fuseball receives only 3.3 on the trigger tick, then
  two 3.3 additions on the next tick.
- An existing top-end Pulsar with controlled roll zero still chooses the
  ordinary descending rim-pause branch on the trigger tick. Hurry-up does
  not retroactively interrupt that pause or descent.
- If the only active enemy was below the rim but reaches it in this update,
  the post-update trigger remains false.
- If a player collision starts PlayerDying on the trigger tick, the final
  latch evaluation still occurs in the already-entered active branch.
  Subsequent PlayerDying ticks do not evaluate or advance it.

### Literal evidence and discrepancies

The approved analysis and literal source agree on the rimit mechanic.
The report's suggested "after active enemy/Egg updates" evaluation point is
made precise here against the current C# order; no conflicting rimit rule
was found.

Literal reference locations in
[tempest.bmx](../../Reference/incitti/tempest.bmx):

- Lines 3746-3760: Egg/baddie updates precede the later main-loop decision.
- Lines 2183-2207: `onrim` is recomputed using post-update enemy heights,
  followed by player collision checks.
- Lines 3811-3824: exhausted reserve/active Eggs select exit or latch rimit.
- Lines 3391-3401: Fuseball boundary override, second climb addition and clamp.
- Lines 3064-3078: Pulsar top-end choice only.
- Lines 2224-2253: life conversion does not reset rimit.
- Line 4253: new-level enemy setup resets rimit.

Intentional current-C# differences are preserved rather than importing source
ordering wholesale: player shots precede enemy/player collision, all active
simulation freezes throughout death, and all enemy units must resolve before
exit. Incitti's `onrim` exit branch is specifically excluded. The analysis's
observation about older, stale Tanker bullet README text is unrelated and was
not repaired in this milestone.

## 2. Latch, reset and death lifecycle

- New game/level initialization sets `HurryUp = false` in `ResetLevel`.
- Once activated, no same-level gameplay path sets it false.
- Reaching the rim or otherwise making the immediate trigger false does not
  undo the latch.
- PlayerDying preserves the flag through all 60 frozen updates.
- Ordinary death reconstruction clears runtime enemies and rebuilds unresolved
  units as fresh Eggs, but does not replenish/reset/recalculate hurry-up.
- Tests cover reconstruction with ten survivors (four active Eggs plus six
  reserve) and with two survivors after eight already-destroyed units.
  Score and destroyed count remain intact.
- Fresh Fuseballs receive the latch starting with their hatch-tick lateral
  update; their first climbing update after re-entry adds 6.6.
- Fresh Pulsars still move one unit on their hatch update. Their first eligible
  top-end decision uses the existing latch, even while reconstructed Egg
  sources remain nonempty.
- Game Over retains the latch until full restart. Restart uses `ResetGame`
  and `ResetLevel`, clears score/restores starting lives as before, and clears
  hurry-up.
- Level Complete replay uses `ResetLevel`, clears hurry-up and resets the
  wave while retaining the existing replay score/lives policy.

Focused comments document latch lifetime and why enemy reconstruction must
not erase same-attempt state.

## 3. Fuseball behavior

The existing update method accepts the game state as an optional `bool`
argument, defaulting to false for existing direct callers.

- Lateral-boundary decisions still consume the ordinary controlled random
  roll, preserving random-call order.
- Hurry-up overrides a descending result with climbing at that decision.
- Climbing adds `ClimbSpeed`, adds the same term again when hurried, then
  clamps to `Level.Depth`. It is not a replacement speed constant.
- Deterministic tests check terms 1, 3.3 and 7.25, including exact 6.6 for the
  development speed, plus a near-rim overshoot clamp.
- Already-descending behavior, rerolled duration thresholds, nine-update
  lateral cadence, direction/edge movement, shot and player collision windows,
  score bands and animation remain unchanged.
- Hurried render-submission tests verify the existing four frames, five colors,
  25 line segments and projected first-arm geometry.

## 4. Pulsar behavior

The existing update method also accepts the same game-owned boolean.

- Only the climbing state's already-at-top endpoint choice changes.
- The ordinary roll still runs; zero chooses descent normally, but chooses
  lane pause / flip when hurried.
- The lane pause stays 18 updates and transfers exactly one adjacent lane on
  its expiration update. Flip angular speed and subsequent 14-update rim
  pause remain intact.
- Climb, descent and climbing-flip movement stay one unit per movement update.
- Mid-tube decision timing, bottom decisions and existing pauses/descent are
  not redirected.
- The shared pulse clock is untouched. A full 181-update latched-state test
  checks phase, pulse window, amplitude, six-segment glyph geometry/colors,
  lane overlays and near-rim suppression/restoration.
- A latched-state collision test confirms the lane hazard begins on phase 71
  and costs a life without points or destroyed-count changes.

## 5. Completion and accounting preservation

No completion or accounting expression was changed.

Hurry-up adds no score, destroyed count or enemy resolution, and removes no
enemy/Egg. Trigger and lifecycle tests check the conserved ten-unit total.
Reserve and active-Egg counts change only through their existing release,
hatch, kill and reconstruction operations.

All-rim survivors still block exit, both before and after the latch activates.
Actual final shot kills still award the existing enemy score and increment the
existing destroyed count, permitting ExitDelay on that same tick. Existing
exit travel and replay remain unchanged. Normal Level 1, Spiker and Tanker
scenarios explicitly remain outside the hurry-up gate.

## 6. Files changed

| File | Change |
|---|---|
| [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs) | Read-only latch, end-of-active-tick evaluation, initialization reset, enemy argument wiring and lifecycle/completion comments. |
| [Fuseball.cs](../../src/Tempest.Game/Gameplay/Fuseball.cs) | Boundary override, duplicate source climb addition and focused explanation. |
| [Pulsar.cs](../../src/Tempest.Game/Gameplay/Pulsar.cs) | Top-end flip override, unchanged-speed explanation. |
| [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs) | 23 deterministic test methods / 39 executed hurry-up cases and local fixtures using existing updates. |
| [README.md](../../README.md) | Current development-scenario hurry-up behavior, lifecycle and evidence boundaries. |
| [TEMPEST-DEVELOPMENT-NOTES.md](../../TEMPEST-DEVELOPMENT-NOTES.md) | Exact order, lifecycle, enemy rules, validation and pending manual work. |
| [Implementation prompt](../Prompts/2026-10-30-rimit-hurry-up-implementation-prompt.md) | Exact requested implementation prompt. |
| This report | Implementation, evidence, validation and manual checklist. |

[TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md) was unchanged during
implementation; the approved documentation-only follow-up updated it and this
ignored report, without changing gameplay, tests or the archived prompt.
Reference, renderer, Level, player/input, dependency manifests and configuration
are unchanged. `AI-Logs/` is ignored by the existing `.gitignore`; prompt/report
are persistent local files, not staged changes. No ignore rule was changed.

## 7. Tests added

The 23 `HurryUp...` methods execute 39 cases covering:

- Reserve still present with no active Eggs; active Egg still present with no
  reserve; no enemies; all active enemies at rim.
- Last-Egg approach updates 39/40, hatch removal, first hatchling update,
  trigger-tick ordinary Fuseball/Pulsar decisions and next-tick effects.
- Post-update rather than pre-update rim height; latched all-rim state;
  activation after a same-tick lethal collision.
- Controlled descending/forced-climbing boundary results with identical rolls
  and lateral counters; exact doubled terms, rim clamp, unchanged descent,
  duration, collision/score boundaries and animation.
- Controlled Pulsar zero-roll top decisions, pause expiration/lane transfer,
  flip completion, one-unit climb/descent/climbing-flip movement, unchanged
  mid-tube choices, pulse oscillator, rendering and pulsing-lane collision.
- Full death freeze, unresolved reconstruction with/without prior kills,
  first eligible re-entry effects, full restart, real final kill and replay.
- Explicit absence of hurry-up in normal, Spiker and Tanker waves.

Fixtures reuse controlled random delegates and existing public update,
Egg-release/hatch and accounting operations, consistent with prior tests.
They do not use reflection, protected-property setters, production test-only
setters or native Raylib initialization. Bounded entity setup loops prepare
known state; exact activation tests assert particular ticks independently.
All pre-existing tests remain in place and pass.

## 8. Automated validation results

Environment: .NET SDK 9.0.304; net9.0; xUnit v2 / VSTest 17.14.1.
Commands run from the repository root. Existing dependencies were available;
no restore/install or dependency changes were needed.

| Validation | Passed | Failed | Skipped | Total / result |
|---|---:|---:|---:|---|
| Focused hurry-up / Fuseball / Pulsar | 121 | 0 | 0 | 121; exit 0 |
| Full solution test suite | 289 | 0 | 0 | 289; exit 0 |
| Additional hurry-up-only count check | 39 | 0 | 0 | 39; exit 0 |
| Solution build | n/a | n/a | n/a | Succeeded; 0 warnings, 0 errors; exit 0 |
| `git diff --check` | n/a | n/a | n/a | Passed; no whitespace errors; exit 0 |

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter 'FullyQualifiedName~HurryUp|FullyQualifiedName~Fuseball|FullyQualifiedName~Pulsar' --no-restore --verbosity minimal
dotnet test TempestCSharp.slnx --no-build --no-restore --verbosity minimal
dotnet build TempestCSharp.slnx --no-restore --verbosity minimal
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-build --no-restore --filter 'FullyQualifiedName~HurryUp' --verbosity minimal
git diff --check
```

The full run reused the binaries just built by the successful focused run;
the subsequent explicit solution build also passed.
Git printed existing LF-to-CRLF conversion notices for four edited tracked
files; these are Git line-ending notices, not build warnings or diff-check
errors. The SDK printed a workload-update availability notice, not a warning
or failure. Prompt/report whitespace was also checked separately because
ignored AI-Logs files are not included in ordinary Git diff.

Validation history is not hidden: the first focused invocation could not
compile because new test fixtures attempted to assign protected `Enemy.Height`
setters; no tests executed. Those fixtures were replaced with source-state
updates, not production API changes. The next invocation had 114 passed,
1 failed, 0 skipped (115 total): a rim Pulsar fixture occupied the player's
starting lane. The next expanded invocation had 119 passed, 1 failed,
0 skipped (120 total): a two-survivor reconstruction fixture incorrectly
expected another Egg still present after 72 real game updates. The safe-lane
setup and assertions were corrected; final focused/full results above are
green. Final explicit build warnings/errors are both zero.

## 9. Neal's manual validation and original checklist

**Manual validation completed by Neal, reported on 2026-10-31. Not performed
by the agent and separate from section 8's automated validation.**

Neal manually tested both existing development scenarios with these options:
`--fuseball-test --lives 10` and `--pulsar-test --lives 10`.

- Fuseball: a noticeable late-wave increase in upward speed was visible and
  consistent with Neal's Atari Tempest expectation. Existing movement,
  shooting, scoring, rim interaction, life-loss/re-entry and exit continued
  to appear normal. No new problem or regression was observed.
- Pulsar: late-wave behavior appeared correct. No vertical speed increase was
  expected or observed as a hurry-up effect. Existing movement, pulse visuals,
  disappearing affected near/top rim segment, lane hazard, shooting/scoring,
  life-loss/re-entry and exit continued to appear normal. No new problem or
  regression was observed.
- General: everything else tested appeared normal; Neal observed no new
  problems. This is not evidence that every original checklist item or every
  other scenario received a separate manual check.

Evidence classification:

- **ARCADE VERIFIED**: observable Fuseball late-wave acceleration, limited to
  Neal's direct observation and arcade comparison; no measured rate is claimed.
- **ARCADE VERIFIED**: Neal's observable Pulsar late-wave behavior appeared
  correct without an expected or observed vertical speed increase. This does
  not independently establish the precise top-end branch.
- **REFERENCE VERIFIED**: exact 3.3-to-6.6 climb terms, forced Fuseball boundary
  decision, Pulsar top-end redirect rather than speed increase, random
  boundaries and source timing/lifecycle remain source-derived.
- **PROJECT DECISION**: exact C# post-update trigger tick, latch implementation,
  full death freeze/reconstruction/reset policy and retained kill-all exit
  are not promoted to arcade evidence by these manual observations.
- **DEVELOPMENT ONLY**: the all-Fuseball/all-Pulsar waves and extra lives remain
  artificial test configurations, not authentic Atari wave composition.

The original commands/checklist below are retained as implementation history
and precise comparison guidance, not a claim Neal measured every item:

```powershell
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

For each scenario:

- Keep a surviving enemy alive while reserve Eggs and active Eggs drain.
  Confirm late-wave behavior changes only after both sources are exhausted.
  For precise state observation, watch `HurryUp`, `ReserveEggs`, `Eggs.Count`,
  enemy height/state and game State in the debugger; no new HUD or flag exists.
- Fuseballs: compare ordinary climb/descent choices with later forced climbing
  at boundaries; inspect 3.3 versus 6.6 climbing updates and rim clamp. Check
  nine-update lateral travel, shot windows, lateral-rim lethality, hit labels
  and existing animation.
- Pulsars: observe later top-end lane pauses/flips instead of newly selected
  descents; verify unchanged one-unit movement, lane transfer and pauses.
  Check pulse glyph, radial flashing, affected near-rim disappearance/return
  and lane hazard timing.
- Lose a life after hurry-up; verify the death freeze, fresh-Egg return,
  retained score/prior kills and persistent later hurried behavior.
- Do not treat all-rim survivors as cleared. Destroy the final enemy and check
  the ordinary exit sequence and score; replay with R and check a fresh
  non-hurried start.
- Test final-life Game Over restart with R and check reset score/lives/supply.
- Smoke-check ordinary Level 1, Spiker and Tanker modes for unchanged play.
- Compare the relevant late-wave situations against Atari Tempest before
  asserting arcade equivalence. Record manual findings separately.

The behavior specification was updated in the approved documentation-only
follow-up using these conservative evidence boundaries. Gameplay code, tests
and the archived implementation prompt were not altered in that follow-up.
`git diff --check` passed (exit 0); ignored report whitespace was checked
separately. No build/test rerun was needed for documentation-only changes;
section 8 retains the agent's original automated results.

## 10. Explicit out-of-scope behavior

Not implemented: Super Zapper or any charge/zap/death timer, source onrim-based
exit, level progression, Level 2/canflip, new geometry/composition, Tanker-carried
Pulsars or Fuseballs, enemy-bullet promotion, new rendering or scoring, audio,
bonuses or unrelated fixes/refactors. No new milestone was started and no commit
was made.

## 11. Remaining arcade uncertainties

- Exact rimit trigger/update timing, latch-through-life-loss internals, forced
  boundary/top-end decisions and numeric effects remain Incitti-derived or
  C# project policy, not arcade-verified. Neal's observable Fuseball
  acceleration and Pulsar late-wave/no-speed-increase observations are now
  recorded separately above; they do not verify those exact internal rules.
- Fuseball 3.3/6.6 development tuning and random/cadence constants, and Pulsar
  51/181/18/14 timings and one-unit movement need direct arcade comparison.
- The artificial all-Fuseball/all-Pulsar ten-unit waves do not establish normal
  Atari eligibility, later-level scaling or composition.
- The kill-all versus source onrim exit difference remains an explicit project
  decision; this implementation supplies no new arcade evidence about exit.
- Existing full-death-freeze versus source death animation/timing divergence
  remains intentional and unverified as exact arcade timing.
- Previously recorded direct climbing-Flipper rim-arrival uncertainty is
  unaffected; no evidence label was upgraded.
- Headless geometry tests do not prove native framebuffer output, spinner
  feel or arcade visual fidelity.
