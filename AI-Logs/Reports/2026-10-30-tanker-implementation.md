# Tanker Development Implementation Report

## 1. Summary

Implemented the opt-in `--tanker-test` scenario on existing Level 1 geometry. The
scenario deterministically assigns original Egg slot 0 to one Tanker; all other
original slots hatch as ordinary Flippers. Tankers carry exactly two latent
Flippers, climb on a fixed lane, then release the pair naturally or upon player-shot
destruction. Normal Level 1 and `--spiker-test` accounting/behavior remain on their
existing paths.

Tanker and Spiker scenario flags are mutually exclusive. The program reports the
conflict and exits before initializing Raylib. `--lives` remains orthogonal.

## 2. Tanker and Cargo Behavior

- Tankers start on their selected Egg's lane at height 10 with two latent cargo
  Flippers. They remain on their lane and use a documented development speed of
  1.85, approximating the earliest source tier's `1.75 + lv/20`.
- They climb every active update and release only after the updated height is
  strictly greater than 385. At exactly 385 they remain active.
- A Tanker has no player collision. A player shot hits only on the same lane when
  shot height is strictly less than Tanker height; there is no 16-unit band.
- A Tanker hit consumes the shot, resolves the original Tanker slot as destroyed,
  awards 200 points, and releases two live cargo Flippers. Natural release resolves
  the original slot as escaped and awards no Tanker points.
- Cargo side selection is deterministic in the development wave. For the
  continuous circle, the first child is placed on the previous lane and the second
  on the next lane. They start at Tanker release height, with the Tanker lane as
  destination, opposite pivot directions, computed pivot angles, and Flipping
  state. Their first update is deferred to the following simulation tick.
- Cargo Flippers continue through the existing Flipper state machine after the
  initial pivot; collision into the player lane is not lethal until the rim height
  is reached. Ordinary Flipper constructors and rim behavior are unchanged.
- Tankers use a fixed, nonrotating magenta line glyph projected at lane center with
  source-scale approximation `TFormSZ(26,z) + 1`; no cargo icon is drawn. Released
  children use the existing Flipper renderer.

## 3. Accounting Design and Invariants

Normal and Spiker modes retain the fixed ten-unit model and the existing Level
`EnemiesDestroyed` meaning. Tanker mode instead tracks original Egg slots as stable
IDs 0 through 9, with separate unresolved, player-destroyed, and naturally escaped
sets. Level's Tanker-specific Egg queue carries those IDs through hatching and
life resets. Slot 0 is the only Tanker candidate; once resolved it cannot hatch
again.

Before release, the Tanker's two latent cargo units are part of the Tanker slot,
not extra enemies. On either natural or shot release, the original slot resolves
and exactly two materialized cargo IDs are added. Cargo kills move one unit from
unresolved to resolved cargo. The Tanker development accounting total is original
slots (destroyed + escaped + unresolved) plus materialized cargo (resolved +
unresolved). Thus the total is 10 before cargo release and 12 after the one Tanker
releases its pair. The global `Level.EnemyTotal` remains 10.

Completion in Tanker mode requires all original slots resolved, all materialized
cargo resolved, zero original reserve Eggs, zero active Eggs, and no active Tankers,
Flippers, or Spikers. Residual player/enemy projectiles are not enemies. The HUD
uses separate ORIGINAL, CARGO (including latent cargo) and ACTIVE counts.

## 4. Death and Reset Behavior

Before Tanker release, death clears active objects and restores all unresolved
original slot IDs to the Egg supply. An unresolved slot 0 therefore re-hatches as
the Tanker with two latent cargo units; it is not expanded into two Eggs.

After release, the Tanker slot remains resolved. Each surviving cargo Flipper is
captured as a pending cargo unit, including its lane, pivot state, angles, height,
identity, and optional Flipper ammunition/timer. On life reset, unresolved original
slots are rebuilt as Eggs and pending cargo units rematerialize as cargo Flippers,
not Eggs or Tankers. Previously destroyed original slots/cargo and earned score
remain resolved/preserved. Game Over restart and Level Complete replay reset the
Tanker wave's slot/cargo ledger and return to one selected Tanker with two latent
cargo units, preserving established lives/score replay behavior.

## 5. Files Changed

Modified:
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Gameplay/Egg.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`

Created:
- `AI-Logs/2026-10-30-tanker-implementation.md` (this report)

No Incitti reference files were modified. No other files were created.

## 6. Command-Line Activation

Run the controlled development scenario with extra lives:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

`--tanker-test` and `--spiker-test` are incompatible; passing both prints a clear
error and exits before Raylib startup. `--enemy-bullets-test` may be combined; it
enables the existing Flipper-fired bullets only. Tankers never fire bullets in
this milestone. Normal launch and standalone Spiker launch remain unchanged.

Expected scenario: Egg slot 0 hatches into a fixed-lane Tanker; other Eggs produce
ordinary Flippers. The Tanker climbs until just beyond the rim threshold, then
releases two Flippers on adjacent sides. Shooting it scores 200 and releases the
same live cargo. Cargo Flippers can then be shot for the existing Flipper score.
After all original and materialized cargo units resolve, the existing Level 1 exit
sequence runs; no next level is created.

## 7. Build and Test Results

- Baseline working tree: clean; baseline build passed.
- Baseline tests: 111 passed, 0 failed, 0 skipped.
- Final build: passed.
- Final tests: 142 passed, 0 failed, 0 skipped.
- `git diff --check`: passed.
- Editor diagnostics: no errors.
- Final reference-directory status check: empty; `Reference/incitti` unchanged.
- No graphical game window was launched for automated validation.

Automated coverage includes scenario conflict/independence, stable Tanker slot,
starting height/speed/release threshold, lane wrapping, cargo count/height/pivot
state/direction, Tanker and Flipper shot scores, natural release, dynamic
accounting, death before and after release, cargo and original-slot restoration,
Game Over/replay, full-wave completion, and finite render geometry. The previous
111 tests continue to pass.

## 8. Manual Validation Checklist

- Launch with `--tanker-test --lives 10`; confirm no Tanker appears without the flag.
- Compare the magenta line glyph, perspective scale, lane centering, and apparent
  cargo presentation with arcade Tempest.
- Check the fixed-lane climb and release timing near the rim.
- Observe the two adjacent cargo Flippers, their height, opposing initial pivots,
  continued climb to the rim, and established collision behavior.
- Compare natural release (no Tanker award) with player-shot release (200 points),
  then verify the two children can score independently.
- Validate death before release restores one Tanker Egg, while death after release
  preserves resolved cargo and restores only surviving cargo units.
- Confirm original/cargo HUD counts, no premature clear, final exit and replay.
- Recheck normal Level 1, Spiker mode, Flipper bullets, spinner direction and
  sensitivity, cursor capture, player shot, Flipper pivot safety, lives and exit.

## 9. Collision and Update Ordering

Global order remains player input/firing, player shots and ordered shot collision,
enemy-bullet movement, pre-enemy completion check, Egg update/hatching, Flipper
update, Tanker update, Spiker update, existing Flipper/bullet/spike player
collision checks, then final completion check.

Tanker and Flippers share the development insertion-order shot list. A Tanker is
inserted when its Egg hatches; natural cargo is appended during Tanker update,
after Flipper updates; shot-released cargo is appended while processing the Tanker
hit. The hit consumes and returns immediately, so that same shot cannot hit newly
created cargo. Cargo objects are marked to skip updates until the next simulation
tick. There is no proximity sort. Normal Level 1 keeps its reverse Flipper-only
collision path and existing update order.

## 10. Known Limitations and Deliberate Omissions

The Tanker scenario always chooses stable original Egg slot 0 and fixes cargo-side
direction for repeatability; this is a development setup, not general source
random selection or arcade progression. The 1.85 speed is a practical development
approximation; BlitzMax integer/floating division semantics and arcade pacing have
not been independently confirmed. Open-tube cargo placement is not exercised by
the current C# Level 1 geometry.

Tanker-fired bullets are omitted. `--enemy-bullets-test` only enables the existing
Flipper bullet feature, including on released cargo Flippers. Super Zapper input,
Tanker ZapIt, zap delay, 200-point zap scoring, stillborn cargo, Fuseball/Pulsar
cargo, other enemy types, audio, bonuses, full progression, and changes to normal
Level 1 accounting or exit are intentionally out of scope.

Manual arcade comparison remains pending. Automated render tests verify projected
line submissions and finiteness, not native framebuffer appearance, smoothness,
or arcade visual fidelity.

## 11. Todo Reconciliation

- Complete: baseline status/build/test recording.
- Complete: exclusive `--tanker-test` selection, stable slot-0 hatch, Tanker lifecycle,
  natural and shot cargo release, deterministic cargo placement and rendering.
- Complete: Tanker-only original/cargo accounting, life reset, Game Over/replay,
  exit eligibility and HUD.
- Complete: focused tests, existing regression suite, diff/reference checks,
  README/development notes, and this report.
- Pending manual validation: Tanker visual appearance, pace, cargo pivot/danger,
  score feel, spinner/cursor behavior, and original arcade comparison listed above.
- Intentionally deferred: Tanker bullets, Super Zapper/ZapIt/stillborn cargo,
  other enemy types, and full level progression. No implementation work remains
  in progress.

## 12. Manual Validation Defects and Corrections

Manual testing exposed two Tanker-only defects after the original implementation:

1. **Life loss resumed surviving enemies in place.** Root cause: Tanker mode saved
  and restored exact active cargo-Flipper runtime snapshots, while original active
  enemies returned through Egg reset. Correction: all active Tankers, ordinary
  Flippers, cargo Flippers, Eggs, shots and bullets are cleared from the tube.
  Unresolved original slot IDs and surviving cargo IDs are rebuilt as new Egg
  entrants at height -10, counter zero and Roaming state. Destroyed original
  slots, escaped Tanker slots, resolved cargo and score remain resolved. An
  unresolved Tanker returns as exactly one Tanker-designated Egg carrying two
  latent cargo units. Released cargo returns as separate cargo-identity Eggs and
  hatches into fresh ordinary Climbing Flippers, never into another Tanker.
  Original-slot identity and cargo-unit identity remain separate so this reset
  cannot duplicate or lose units.
2. **Natural Tanker release showed one apparent Flipper.** Root cause: both
  children were initially assigned the former Tanker lane as their destination,
  so both pivots converged onto that lane and overlapped. Re-reading Incitti's
  `ReleaseCargo` lane stepping and Flipper creation showed that each child starts
  on an adjacent lane and its destination is the next outward lane; direction
  and side alternate for the second child. Correction: the two cargo paths now
  pivot outward to distinct lanes, including continuous wrap at lanes 0 and 15.
  The pair stays distinct and independently shootable after the pivot. This
  source reread corrects the earlier report's mistaken interpretation that both
  children pivot back onto the carrier lane. A concise gameplay comment records
  why the children use outward destinations: to follow the lane-step behavior
  and avoid the observed overlap.

Focused regressions now verify a death with exactly three unresolved original
slots creates exactly three fresh Eggs, preserves score and resolved slots, and
does not reuse pre-death objects or runtime positions. A released-cargo death test
verifies a surviving cargo ID returns as a fresh cargo Egg and hatches to a new
Climbing Flipper. Natural and shot release tests assert an explicit count of two,
two distinct object references and cargo IDs, the corrected source/destination
lanes, completion of both pivots, distinct occupied lanes and independent shot
eligibility. Shot release still consumes the original shot and awards exactly
200 points; natural release awards none.

Reconciliation (post-interruption): the working tree had been left with the
rejected converging cargo implementation in `Tanker.cs`/`Flipper.cs` (both children
targeted the carrier lane, then used a separate pause-and-step transition) plus a
duplicate, contradictory second block of Tanker tests. Production now matches the
corrected behavior: `Tanker.ReleaseFlipperCargo` gives each child an adjacent
source lane and the next outward destination lane (directions `side` / `-side`),
and the temporary `tankerCargoInitialPivotPending` path in `Flipper` was removed so
cargo uses the ordinary pivot. Same-tick policy: for both natural and shot release
the cargo is created after that tick's Flipper update, so its first update occurs
on the following tick (cargo height equals the Tanker height after the release
tick). Twelve obsolete/duplicate test cases (converging-destination assertions,
immediate-update expectation and exact duplicates) were removed; their unique
assertions (materialized-cargo count, no `EnemiesDestroyed` credit) were merged
into the surviving tests, which now also assert outward destinations and distinct
lanes for both release kinds. Exact-two, distinct-ID, scoring, accounting and
fresh-Egg-reset assertions are unchanged.

Final post-correction validation: `dotnet build` passed (0 warnings, 0 errors);
`dotnet test` reported 142 passed, 0 failed, 0 skipped (142 total);
`git diff --check` reported no whitespace errors. No Incitti reference file was
modified. Visual confirmation of both cargo Flippers after their pivot and
comparison of the source interpretation with arcade gameplay remain manual
validation items. The separate direct Flipper rim-arrival collision bug is not
addressed here.
