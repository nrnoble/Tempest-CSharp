# Tanker-Cargo Flipper Direction Correction

## Summary

Corrected only Tanker-released Flipper movement. Ordinary non-cargo Flippers,
Super Zapper semantics, Hurry-Up, enemy composition, progression, exit rules,
and the behavior specification are unchanged. The all-rim completion finding
was not implemented.

The Super Zapper had already passed manual validation in its five existing
scenarios. Subsequent Tanker-mode validation exposed that cargo Flippers
tracked the Blaster below the rim. Neal confirmed the arcade cargo does not
dynamically track the Blaster while below the rim. This report documents the
separate correction; manual validation of the corrected cargo behavior remains
pending.

## Previous C# behavior

`Tanker.ReleaseFlipperCargo` placed two children on opposite adjacent lanes,
at Tanker height, and created each child in mandatory `FlipperState.Flipping`.
However, it assigned each initial destination farther away from the Tanker
lane. After each pivot completed, `Flipper.BeginPivot` unconditionally called
`Level.FindShortestDirection(EdgeIndex, playerEdge)`. Cargo therefore
reselected its direction toward the current Blaster after every pause, even
below the rim.

The `FlipperState.Pausing` branch also did not advance cargo height. The shared
pause-to-pivot selection path was the direct cause of player-responsive
mid-tube direction changes; it was not the mandatory initial pivot.

## Incitti source and arcade observation

Incitti `Tanker.ReleaseCargo` places each child on an adjacent lane, creates it
with `typ = 1` (already flipping) at the Tanker's height, and assigns the
opposite of the placement direction as the child direction. In the source lane
step convention, this makes the initial pivot return toward the Tanker lane.
After a pivot, the source's delaying state raises the child by one height unit
per update, then takes a lane step in its retained direction. It recalculates
toward the player only when height is at least `theLevel.depth`. The `canflip`
gate is in the ordinary `typ = 0` climb path; it does not enable mid-tube
player pursuit for cargo born in `typ = 1`.

The first Tanker appears at zero-based `lv = 2` (displayed Level 3) in the
reference's normal first-set progression, where `canflip` is enabled. The
development `--tanker-test` runs this later-level Tanker/cargo behavior on
Level 1 geometry and tuning. That scenario mismatch does not account for
mid-tube pursuit: Incitti cargo preserves its direction below the rim even
when `canflip` is true.

Neal verified:

- Ordinary Level 1 Flippers climb their hatch lane without reacting to Blaster
  movement, then pursue at the rim.
- Tanker cargo does not dynamically track Blaster movement below the rim.

The exact Atari intermediate lane sequence, timing, and initial-pivot geometry
remain unverified. Incitti source establishes reference behavior, not arcade
truth.

## Correction

- The deterministic first-side choice is retained as a development/project
  decision; it is not claimed to match arcade randomness.
- Both children remain on opposite adjacent source lanes and retain their
  distinct cargo IDs.
- Each child's initial destination is now the former Tanker lane, so its
  mandatory first pivot heads inward toward the carrier.
- Cargo preserves its assigned direction through below-rim pivot/pause cycles;
  changing `playerEdge` cannot change that route.
- Tanker cargo gains the source's one-unit climb during each below-rim pause.
  Existing pivot climbing remains unchanged.
- Once cargo reaches the rim, the next pivot selection uses the current player
  lane. Existing rim collision and completed-pivot behavior remain in place.
- Non-cargo Flippers keep the previous state machine: ordinary Level 1 climb,
  rim pause and pursuit are unchanged.

The current C# representation advances `EdgeIndex` when a pivot completes and
then pauses before starting the next pivot. Incitti applies its lane step at
the end of the delay. The corrected direction and endpoint sequence follow
the source; exact arcade pause presentation and timing remain an observation
item rather than a broader state-machine rewrite.

## Accounting and Super Zapper preservation

Movement changes do not modify Tanker release outcomes, score, original-slot
resolution, cargo ID assignment, or collection ownership. Natural release
still resolves the Tanker slot as escaped without carrier score; player-shot
release still awards 200 and resolves it as destroyed.

Super Zapper stillborn cargo uses the corrected `CreateTankerCargo` path:
the two children retain their IDs and corrected initial geometry, receive
eight-update timers, and remain deferred until the next eligible update.
Expiry still skips ordinary movement on that visit and resolves each child
once for 150. Player movement cannot redirect a stillborn child's below-rim
direction before expiry. The remaining-charge replacement behavior remains
covered. Death/re-entry still reconstructs only unresolved cargo identities;
completion continues to wait for unresolved cargo. No Super Zapper code or
semantics were changed for this correction.

## Files changed for this correction

- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Reports/2026-10-30-super-zapper-implementation-report.md`
- `AI-Logs/Reports/2026-10-30-tanker-cargo-direction-correction-report.md`

The Super Zapper milestone's pre-existing uncommitted changes remain in the
working tree. `TEMPEST-BEHAVIOR-SPEC.md` was not changed. No commit was made.

## Automated validation

Final validation:

- Focused Tanker/cargo filter: **57 passed, 0 failed, 0 skipped**.
- Focused Super Zapper tests: **30 passed, 0 failed, 0 skipped**.
- Focused Flipper tests: **28 passed, 0 failed, 0 skipped**.
- Full solution test suite: **322 passed, 0 failed, 0 skipped**.
- Solution build: **0 warnings, 0 errors**.
- `git diff --check`: **passed**, exit code 0. Git printed non-fatal
  working-copy LF-to-CRLF normalization notices.

An early focused Tanker/cargo run had **4 failed, 51 passed (55 total)** while
tests still expected outward destinations, expected stillborn timers to have
one fewer update after an added movement assertion, and used a near-rim fixture
for a below-rim route test. Those expectations/fixtures were corrected. The
later focused runs and final full suite passed.

Automated tests do not establish arcade fidelity. The five prior Super Zapper
scenario validations passed before this separate correction; the correction
itself has not yet been manually validated.

## Manual validation checklist

- Run the ordinary Level 1 scenario and confirm normal Flippers still climb
  in one lane, ignore Blaster movement below the rim, and pursue at the rim.
- Run `--tanker-test --lives 10`; test both natural and shot release.
- Move the Blaster repeatedly during the initial cargo pivot and later
  below-rim cycles. Confirm the cargo route does not retarget, continues
  climbing during pivots/pauses, and does not cause premature collision.
- Observe whether cargo's initial pivot passes toward the former Tanker lane;
  record intermediate lane sequence and pause timing without assuming the
  reference recreation is arcade-correct.
- At the rim, move the Blaster and verify cargo resumes pursuit and retains
  the established completed-pivot collision rule.
- Use Super Zapper on a Tanker. Confirm both stillborn children start with
  corrected geometry, have the existing delayed eight-update resolution, and
  preserve their identities and score behavior.
- Cause death/re-entry before and after cargo materialization; confirm no
  unresolved ID is duplicated or lost. Verify completion remains blocked
  until the cargo resolves.
- Recheck all five existing Super Zapper scenarios only if manual testing
  exposes a real interaction regression; no broader revalidation is required
  by the current automated results.

## Remaining uncertainties

- Whether Atari cargo uses the source's exact inward initial pivot.
- Exact arcade pause duration, lane-step presentation, and intermediate lane
  sequence after release.
- Whether Atari cargo continues the same one-unit pause climb and pivot climb
  used by Incitti.
- Whether Tanker unlock level and `canflip` progression match the arcade.
- Whether the controlled Level 1-geometry development scenario visually
  represents Tanker behavior from its actual later-level progression.

## Follow-up manual-validation correction (2026-10-31)

### Arcade observation

Neal confirmed the first movement correction achieved its goal: Tanker cargo no
longer appeared to follow Blaster movement while climbing below the rim.
Follow-up manual testing exposed a separate problem: when the Tanker was shot,
one or sometimes both released cargo Flippers disappeared. Neal clarified that
the shot should destroy only the Tanker; both Flippers must survive and emerge
in different lanes other than the Tanker's lane. The children then climb
without below-rim player tracking.

### Exact cause in C#

`RunOneStep` processes `UpdatePlayerShots` before `UpdateFlippers` and
`UpdateTankers`. `UpdatePlayerShots` updates each shot and scans
`developmentCollisionOrder`; a Tanker hit removes the Tanker, awards 200,
resolves its original slot, releases cargo, and returns `true`. That consumes
the Tanker-killing projectile. Shot-released cargo is first placed in
`shotReleasedCargoPendingActivation` and is added to the Flipper and collision
lists only after the tick's shots and enemy updates have completed. The
killing shot therefore cannot hit a child on the release tick, and children
receive no shot collision or movement update on that tick.

The deterministic pre-correction regression test established the actual
failure: with two projectiles already in flight in the Tanker's lane, the
newer projectile killed the Tanker and was consumed. The other projectile
survived that tick because the children were not yet collision-eligible. On
the next tick it hit and destroyed one child.

The cause was the previous correction's inward initial geometry. The first
child started on the previous adjacent lane and pivoted to the Tanker's lane;
the second started on the next lane and also pivoted to the Tanker's lane.
While `FlipperState.Flipping`, `Flipper.IsHitBy` accepts a shot in either the
source or destination lane. A remaining Tanker-lane shot whose height was
strictly between 0 and 16 units below the child therefore matched either
child's pivot collision path. The ordinary Flipper shot path removed the
first matching child, resolved its cargo ID, and awarded 150. Additional
leftover shots could hit the sibling on later ticks. This is a real entity
destruction/accounting event, not only a rendering overlap, although inward
pivot animation could also visually converge on the former Tanker lane.

Natural release and Super Zapper expiry also insert cargo after that tick's
player-shot pass, so a pre-existing Tanker-lane shot could reach inward-pivot
cargo on the following tick regardless of release kind. There is no cleanup
path silently deleting cargo. The creator shot is consumed; a *different*
already-existing projectile causes the observed removal.

### Reference, arcade, and project behavior

- **REFERENCE VERIFIED (Incitti):** `Tanker.ReleaseCargo` places children on
  opposite adjacent lanes, creates each in the mandatory `typ = 1` Flipping
  state, and sets direction opposite to the side used to place it. This makes
  both initial pivots head toward the Tanker's former lane.
- **ARCADE VERIFIED (Neal's manual observation):** both children survive the
  Tanker shot and visibly emerge in two distinct lanes other than the
  Tanker's. Cargo does not dynamically track Blaster movement below the rim.
- **PROJECT/DEVELOPMENT DECISION:** C# keeps its deterministic side assignment
  and now sends the first child from its adjacent lane to the next lane
  outward, and the second child likewise outward. This keeps both source and
  initial destination lanes off the Tanker's lane and prevents the reproduced
  lingering shot from intersecting either initial pivot. It satisfies the
  observed release result without claiming that the exact intermediate Atari
  route is known.

Below-rim fixed direction and climb, rim player pursuit, rim collision,
ordinary Flipper behavior, cargo score/accounting, and natural/shot release
outcomes are unchanged. `Tanker.ReleaseFlipperCargo` is shared by live,
natural-release, and stillborn Super Zapper cargo, so the geometry correction
applies to all three paths. It does not change cargo IDs, the eight-update
stillborn timers, deferred activation, 150-point child resolution, death and
re-entry conservation, or completion blocking. The existing Super Zapper
tests cover the shared geometry and timer/accounting behavior.

### Automated validation

Final validation:

- Focused Tanker/cargo filter: **60 passed, 0 failed, 0 skipped**.
- Focused Super Zapper tests: **30 passed, 0 failed, 0 skipped**.
- Focused Flipper tests: **28 passed, 0 failed, 0 skipped**.
- Full solution test suite: **325 passed, 0 failed, 0 skipped**.
- Solution build: **0 warnings, 0 errors**.
- `git diff --check`: **passed**, exit code 0.

Before the production geometry change, the deterministic reproduction passed
against the old code by confirming the extra projectile destroyed a child.
The first post-change Tanker run had **1 failed, 57 passed**: its test assumed
the leftover shot would expire after a fixed number of updates, but the
Tanker's height had advanced while staging the shots. The test was corrected
to assert cargo survival until the shot naturally expires; the final focused
and full runs passed. The manual pursuit result was observed before this
geometry correction; Neal has not yet manually validated the final outward
route.

The behavior specification remains unchanged. No all-rim completion behavior
was implemented, and no commit was made.

### Follow-up files

- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Reports/2026-10-30-super-zapper-implementation-report.md`
- `AI-Logs/Reports/2026-10-30-tanker-cargo-direction-correction-report.md`

### Remaining uncertainty

The exact Atari intermediate lane sequence after the two observed release lanes,
initial turn direction, and timing remain unknown. The outward first-pivot
choice is an implementation decision to preserve the observed lane separation
and prevent the proven Tanker-lane projectile collision; it is not established
as the complete arcade route.

## Final manual validation (2026-10-31)

Neal completed manual validation of the final Super Zapper and Tanker-cargo
behavior. This section supersedes earlier statements in this report that
manual validation of the final outward cargo route was pending; those
statements are retained as the status at the time of the earlier report.

### Arcade observations

- **ARCADE VERIFIED** — Ordinary Level 1 Flippers continue directly up their
  lanes without reacting to Blaster left/right movement below the rim, then
  pursue the Blaster after reaching rim behavior.
- **ARCADE VERIFIED** — Tanker-released cargo Flippers do not dynamically
  follow Blaster movement while traveling below the rim.
- **ARCADE VERIFIED** — Shooting a Tanker destroys the Tanker while both
  released cargo Flippers survive and appear in two distinct lanes other than
  the Tanker's former lane.
- **ARCADE VERIFIED** — Moving the Blaster during below-rim cargo travel does
  not redirect the cargo route toward the Blaster.
- **ARCADE VERIFIED** — A player shot already traveling through the tube when
  the Tanker was shot produced no unusual cargo behavior in Neal's manual
  test.
- Neal judged the final corrected Tanker cargo behavior to appear correct.
  The outward initial geometry is the recreation behavior chosen to match the
  visible result; it is not evidence that the full intermediate Atari route
  is known.

### Super Zapper validation and evidence limits

Neal manually validated Super Zapper use successfully in all five existing
scenarios: ordinary Level 1, Spiker, Tanker, Fuseball, and Pulsar. No
Super-Zapper-specific regression was observed. This validates the observable
gameplay exercised in those scenarios, not exact target-selection
comparisons, 4/6/8... delays, timer ordering, the eight-update stillborn
countdown, the 60-step flash lifetime, or internal accounting. Those remain
**REFERENCE VERIFIED** against Incitti or **PROJECT DECISION** as appropriate;
Neal did not measure exact timing constants.

Ordinary shooting of a Tanker releases two live cargo Flippers. Super Zapper
delayed Tanker resolution instead uses the shared cargo entity path with
stillborn children. Visual scenario validation is distinct from reference/test
evidence for the exact timer, score, identity, and accounting rules. The
stillborn cargo geometry follows the same final outward recreation route.

### Newly established all-rim completion behavior

Neal also observed that after reserve/incoming Eggs are exhausted, if all
remaining enemies reach the top rim, those rim enemies disappear and the
Blaster proceeds into tube exit without requiring the player to kill every
one. This broad observable behavior is **ARCADE VERIFIED**. The C# recreation
still uses its kill-all completion rule; implementing all-rim completion is
the next standalone deferred milestone. Exact qualifying height and state
rules are not claimed as arcade observations; Incitti's literal predicate
remains **REFERENCE VERIFIED** only.

The behavior specification was updated to record these classifications. No
gameplay code, tests, configuration, or project files were changed for this
documentation update, and no commit was made.
