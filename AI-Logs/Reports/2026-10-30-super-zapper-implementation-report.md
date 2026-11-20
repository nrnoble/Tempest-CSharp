# Super Zapper Implementation Report

Starting checkpoint: `587b9e3 Implement Fuseball and Pulsar hurry-up behavior`

## 1. Source behavior implemented

Implemented the Incitti reference's two-charge Super Zapper lifecycle, both
targeting strengths, accepted-target delays, per-enemy expiry scoring, and
60-step tube/web flash. This is **REFERENCE VERIFIED** against
`Reference/incitti/tempest.bmx`; it is not evidence of Atari arcade behavior.
No behavior-specification claims were upgraded.

The approved analysis and literal source agreed on the selection comparisons,
timer ordering, scoring, and Tanker stillborn cargo path. One existing C#
project decision differs from the literal source: Incitti randomizes the
Tanker cargo side, while C# already uses a deterministic side. That deterministic
orientation was retained; children still occupy opposite adjacent lanes and
directions. Same-pass child updates could not be established by static source
inspection, so the existing C# deferred-activation policy was retained.

## 2. Architecture and shared contract

`Enemy` now has a default-rejecting `TryZap` method, a shared pending countdown,
and countdown advancement. Flipper, Tanker, Spiker, Fuseball and Pulsar accept
and replace the timer. EnemyBullet remains rejected by default; Spike is not an
Enemy and is not passed to the target system. Scoring, active collection
changes, Tanker identities, and Fuseball popups remain game-owned.

`SuperZapperSystem` owns charge count, dispatch, selection, and flash state.
`TempestGame` creates a stable target view from development insertion order
(including rejected bullets), or Flipper creation order in ordinary Level 1.
The ordinary reverse Flipper shot scan is unchanged.

## 3. Input and charge lifecycle

Space and right mouse are sampled with Raylib press-edge APIs and added as the
optional trailing `PlayerInput.ZapPressed` value; existing four-argument input
construction remains valid. Activation is handled only in active gameplay and
is separate from held fire.

The first accepted press dispatches strength 2; the second dispatches strength
1. A charge is consumed after dispatch even when strength 1 finds no target.
Strength 2 always starts the flash. Initial setup, Game Over restart and Level
Complete replay restore two charges. Ordinary life loss preserves the
remaining charge.

## 4. All-target behavior

Strength 2 visits enemies in source/insertion order, beginning at delay 4 and
adding 2 only after an entity accepts. Rejected bullets do not advance the
delay; spikes are outside the Enemy view. There is no lane, height, type or
proximity sort. Ordinary Level 1 uses stable Flipper creation order without
changing shot-collision order.

## 5. Single-target behavior

Strength 1 uses the literal source comparisons: initial best distance 16,
height 0 and type 0; circular shortest distance on the current 16-lane
continuous tube, with opposite-lane distance 8 unchanged; direct distance on a
non-continuous model; and the strict `distance < bestDistance + 2` gate.
Within that gate, strictly greater height wins, otherwise strictly smaller
distance wins, otherwise strictly greater source type wins. Equal comparisons
retain the incumbent. The executable type comparison is preserved even though
the source comment says “same d.”

Source type values are Spiker 2, Flipper 3, Tanker 4, Pulsar 5 and Fuseball 6.
A selected target receives delay 4 and starts the flash. No target consumes
the charge without starting a new flash.

## 6. Timer, update and pre-emption semantics

Activation occurs before player-shot processing and enemy updates. The
activation tick is timer visit one. Positive timers decrement before normal
behavior; earlier visits run ordinary movement/behavior, while expiry resolves
the target and skips its normal update. The existing player-shot-before-enemy
update ordering is preserved.

A shot can resolve a pending target before zap expiry. Spiker retreat and
Tanker natural release likewise retain their ordinary accounting and do not
earn zap score. PlayerDying freezes enemy timers and movement; reconstruction
discards runtime timers but preserves unresolved accounting and earned score.

## 7. Per-enemy score and resolution

- Flipper: remove, award 150, resolve once.
- Tanker: remove, resolve its original slot as destroyed, award 200, then
  materialize stillborn cargo.
- Spiker: remove, award 50, resolve through existing accounting, retain its
  spike.
- Pulsar: remove and award 200.
- Fuseball: calculate the existing `KillScore` at expiry height, resolve and
  award once, and create one existing score popup using that height and score.

EnemyBullets continue their existing movement, shooting and collision behavior.
Spikes remain visible and hazardous; neither strength affects either entity.

## 8. Tanker ledger and stillborn cargo

Pending carrier zaps do not change original-slot identity, latent cargo or
accounting. On zap expiry, the carrier slot is destroyed once for 200, and its
two latent units become distinct unresolved cargo IDs. Two Flippers are
created at carrier height on opposite adjacent lanes with ordinary Flipping
geometry and an eight-update timer. They are registered once and start their
timer on the next simulation tick.

The Tanker ledger total becomes 12 without changing standard
`Level.EnemiesDestroyed`. Each child resolves its own ID for 150 on zap expiry
or ordinary shot destruction. Completion remains blocked while any cargo ID is
unresolved. Shot destruction and natural release preempt carrier zap expiry
and continue to produce ordinary live cargo.

## 9. Death and re-entry

Death before carrier expiry restores the still-unresolved original slot as one
Tanker Egg and retains its two latent cargo units. Death after carrier expiry
preserves the destroyed slot and carrier score, then restores only unresolved
materialized cargo IDs as fresh cargo Eggs; resolved children are not restored.
Repeated death/re-entry tests verify identity conservation and prevent duplicate
or lost cargo IDs.

Ordinary non-Tanker survivors continue to reconstruct through the existing
fresh-Egg policy. Charges and score are preserved during ordinary life loss.

## 10. Completion

Pending zap targets remain active/accounted and block completion. Tanker
carrier expiry does not permit completion while stillborn cargo IDs remain
unresolved. Final ordinary enemy expiry resolves score/accounting before the
existing same-tick completion check. Kill-all completion, Hurry-Up and exit
timing were not changed.

## 11. Flash and rendering

Strength 2 starts a 60-display-step flash unconditionally; strength 1 starts
one only when it selects a target. The color phase uses
`(displayCounter / 2) % 4 > 1` and alternates the existing blue tube/web color
with the Tanker/magenta approximation. Tube geometry is unchanged. Pulsar lane
overlays and near-rim suppression remain in their existing paths. The display
counter advances while PlayerDying presentation continues, independently of
frozen enemy timers. No charge counter/icons or audio were added.

## 12. Files changed

- `src/Tempest.Game/Gameplay/Enemy.cs`
- `src/Tempest.Game/Gameplay/Flipper.cs`
- `src/Tempest.Game/Gameplay/Fuseball.cs`
- `src/Tempest.Game/Gameplay/Pulsar.cs`
- `src/Tempest.Game/Gameplay/Spiker.cs`
- `src/Tempest.Game/Gameplay/Tanker.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Systems/InputSystem.cs`
- `src/Tempest.Game/Systems/SuperZapperSystem.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `README.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Prompts/2026-10-30-super-zapper-implementation-prompt.md`
- `AI-Logs/Reports/2026-10-30-super-zapper-implementation-report.md`

`TEMPEST-BEHAVIOR-SPEC.md` was not changed. The saved prompt is an exact copy
of the supplied attachment; source and destination SHA-256 both equal
`A699BE134F85A0792F07515D70CE54534FDB45AA8203C000FE187D7E3EA1A612`.

## 13. Tests added

`SuperZapperTests.cs` adds 29 focused tests covering the shared contract,
replacement timers, source-order delays, lane-selection boundaries, input
injection, charge/recharge lifecycle, countdown timing and movement,
shot/retreat/natural-release pre-emption, all enemy score paths, Tanker
stillborn identities and repeated death/re-entry, completion blocking, and
flash phase/duration/geometry.

## 14. Automated validation

- Focused Super Zapper tests: **29 passed, 0 failed, 0 skipped**.
- Focused Tanker/accounting filter: **55 passed, 0 failed, 0 skipped**.
- Focused Flipper/Spiker/Fuseball/Pulsar/bullet/Hurry-Up/exit/spinner
  regressions: **204 passed, 0 failed, 0 skipped**.
- Full solution tests: **318 passed, 0 failed, 0 skipped**.
- Solution build: **0 warnings, 0 errors**.
- `git diff --check`: **passed** with exit code 0. Git emitted non-fatal
  working-copy LF-to-CRLF normalization notices; no whitespace errors were
  reported.

An intermediate focused run had one failed test because the new Spiker test
incorrectly assumed the development wave had only one spike; the test was
corrected to check that the Spiker's existing lane spike remained unchanged.
An xUnit analyzer warning in the test was also corrected. Final focused/full
test runs and build are clean.

## 15. Manual-validation commands and checklist

Manual validation was **not performed**. Use:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Check Space/right-mouse press edges and held-input behavior; first/all and
second/single use; delayed movement, scoring and pre-emption; bullet/spike
immunity; charge persistence/recharge; Tanker stillborn appearance, timing and
re-entry; flash color/phase/duration during play and death; Pulsar overlays;
and existing spinner, cursor capture, Hurry-Up and exit behavior.

## 16. Explicitly out of scope

No audio, charge HUD, new enemy types, progression, changes to Hurry-Up,
ordinary shot order, death duration/freeze, kill-all completion, spike exit
behavior, or behavior-spec update. No commit was created.

## 17. Remaining arcade uncertainties

All implemented Super Zapper details remain source-derived, not
**ARCADE VERIFIED**. Manual Atari comparison remains needed for activation
mapping, charge lifecycle, exact target selection and delays, per-enemy scores,
movement/pre-emption, Tanker stillborn cargo, death/re-entry, and flash
presentation. Tanker cargo-side randomness and same-pass appended-child update
timing are represented by current C# project decisions, not established arcade
behavior.

## 18. Post-validation Tanker-cargo movement correction

The Super Zapper passed manual validation in all five existing scenarios:
ordinary Flipper, Spiker, Tanker, Fuseball, and Pulsar. Separate manual
Tanker-mode testing exposed a Tanker-cargo Flipper pursuit bug: the shared
Flipper pause-to-pivot path recalculated direction toward the Blaster while
cargo was still below the rim.

The follow-up correction changes only shared Tanker-cargo movement. Cargo now
uses the source-derived initial pivot toward the Tanker lane, retains its
prescribed direction through below-rim pivot/pause cycles, and resumes
player-directed pivot selection at the rim. It does not change Super Zapper
targeting, charges, timers, scoring, flash, or target-expiry behavior. See
`2026-10-30-tanker-cargo-direction-correction-report.md` for implementation,
regression results, and remaining arcade uncertainties.

Post-correction automated validation: 29 Super Zapper tests passed; the
321-test full suite passed; the solution build completed with zero warnings
and errors. Manual validation of this follow-up cargo correction remains
pending; the earlier five-scenario Super Zapper manual pass is unchanged.

## 19. Follow-up Tanker-cargo release geometry correction

Neal's follow-up Tanker-mode validation confirmed that below-rim cargo pursuit
was corrected, then found that an already-existing second player shot could
destroy a released child on the next tick. The first correction's
Incitti-derived inward pivot made both children collision-eligible in the
former Tanker lane. The C# release geometry now pivots both children outward
from their adjacent source lanes. This is a project choice based on Neal's
observation that both children survive and emerge in two distinct non-Tanker
lanes; it is not a claim that the full intermediate arcade route is known.

The correction is in the shared cargo-creation path, so Super Zapper stillborn
children use the same release geometry. It changes no Super Zapper targeting,
charge, timer, score, flash, delayed activation, or cargo-identity semantics.
See `2026-10-30-tanker-cargo-direction-correction-report.md` for the
deterministic collision reproduction and full validation results.

Post-geometry validation: 30 Super Zapper tests passed and the full 325-test
suite passed; the build completed with zero warnings and errors. The final
outward route still awaits Neal's manual validation.

## 20. Final manual validation (2026-10-31)

This final validation supersedes the earlier "manual validation not
performed"/pending statements in Sections 15, 17, 18, and 19; those sections
preserve the status at their original reporting time.

Neal completed manual validation of Super Zapper in all five existing
scenarios:

```powershell
dotnet run --project src/Tempest.Game -- --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
dotnet run --project src/Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
```

Super Zapper use was successful in all five scenario contexts, with no
Super-Zapper-specific regression observed. This is **ARCADE VERIFIED** for the
observable behavior exercised in those play-throughs; it does not establish
exact target-selection comparisons, accepted-target delays, countdown/update
ordering, stillborn timer duration, flash lifetime, or hidden accounting.
Those details remain **REFERENCE VERIFIED** against Incitti or
**PROJECT DECISION** as appropriate. Neal did not measure the source-derived
timing constants.

Neal also completed final manual validation of the Tanker-cargo correction:
ordinary Flippers retain their observed lane-climb/rim-pursuit behavior;
Tanker cargo does not track the Blaster below the rim; a shot destroys the
Tanker while both cargo Flippers survive in distinct lanes other than the
former Tanker lane; and another in-flight shot produced no unusual behavior
in the manual test. The final outward release geometry is the recreation
behavior selected to match those visible observations. The exact intermediate
arcade lane sequence, pause/climb timing, and initial route remain unknown.
Super Zapper stillborn cargo shares this entity path, but manual validation of
visible gameplay is distinct from reference/test-derived timer and accounting
details.

During this validation Neal additionally established the all-rim completion
observation: after reserve/incoming Eggs are exhausted, if all remaining
enemies reach the top rim, they disappear and tube exit begins without
requiring the player to kill every one. This is **ARCADE VERIFIED** at the
observable level. Current C# kill-all completion remains a **PROJECT
DECISION**; all-rim completion is deferred to a separate standalone milestone.
No gameplay implementation was made as part of this documentation update.
