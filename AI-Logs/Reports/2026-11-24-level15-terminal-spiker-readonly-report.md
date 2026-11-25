# Level 15 terminal-Spiker read-only investigation

Date: 2026-11-24

## Disposition

**READY FOR BOUNDED CORRECTION.**

The direct-start Level 15 candidate does not wire its Level 11 parameter
development baseline into the existing terminal-Spiker lifecycle. At the
height-zero return boundary it removes the Spiker and returns that original to
reserve instead of restarting it while generic originals remain or converting
it when they are exhausted. The state transition re-creates pending supply,
and the Level 11-based selector can select Spiker again. This explains the
reported repeated up/down cycles without requiring a movement defect.

This is classification **B: an existing shared terminal-conversion mechanism
was not wired for the direct Level 15 candidate**. Repository history does not
support calling it a regression. The Atari behavior and a safe C# translation
are established; no correction was implemented.

## Starting state and scope

- Branch `master`; HEAD `045aeb240fce91a3eaacf9424da30cd7359927bf`;
  upstream `origin/master`; ahead/behind `0/0`.
- Index was empty.
- Tracked worktree modifications at start:
  - `AI-Logs/Reports/2026-11-24-background-opacity-closeout-report.md`
  - `TEMPEST-BEHAVIOR-SPEC.md`
  - `TEMPEST-DEVELOPMENT-NOTES.md`
  - `src/Tempest.Game/Levels/LevelConfiguration.cs`
  - `src/Tempest.Game/Levels/RomPresentationMetadata.cs`
  - `src/Tempest.Game/Program.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
  - `src/Tempest.Game/Rendering/AtariPresentationCalibration.cs`
  - `tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs`
  - `tests/Tempest.Game.Tests/Level15TubeCandidateTests.cs`
- Relevant untracked Level 15 records:
  - Prompts: `2026-11-24-level15-baseline-spike-reconciliation-prompt.md`,
    `2026-11-24-level15-spike-arcade-evidence-correction-prompt.md`,
    `2026-11-24-level15-spike-height-correction-prompt.md`.
  - Reports: `2026-11-24-level15-baseline-spike-reconciliation-report.md`,
    `2026-11-24-level15-spike-arcade-evidence-correction-report.md`,
    `2026-11-24-level15-spike-height-correction-report.md`.
- Other untracked entries included `Reference/arcade/MBUCOD.V05.txt` and
  `temo/`. Neither was opened or changed. All pre-existing work was preserved.

The candidate under investigation is
`LevelConfiguration.Level15DirectStartCandidate`: direct-only Level 15, with
`UsesLevel11ParameterDevelopmentBaseline = true` and
`UsesLevel11RomWavePolicy = false`. It uses the Level 11 selector as a
development parameter baseline; that is not a claim that Level 15 is a
production level or uses the complete Level 11 ROM policy. The campaign still
ends at Level 14.

Neal's current manual observation, supplied for this task: Level 15 gameplay
otherwise passes; the accepted presentation is Y gain `1.15`, scale `1`,
vertical offset `75`, depth-Y gain `1`; the corrected starting-Spike length
has passed and is frozen; near the wave end the last couple of Spikers
repeatedly travel up and down without converting into Tankers. This is a
manual observation of the C# candidate, not an Atari observation.

The older Level 15 Spike prompt/report and durable documents still describe
Spike acceptance as pending, and an earlier baseline report describes an
earlier worktree without the current candidate changes. They are historical
records, not the current acceptance authority. The current user task
supersedes those statuses; none was rewritten.

## Historical Level 11 comparison

- The 2026-11-21 read-only Level 11 report traces Atari's shared Spiker action:
  grow, reverse, return/hop, then test `enemies_pending`. Nonzero means repeat
  as the same Spiker; zero means rewrite that active enemy slot as a
  Flipper-carrying Tanker. The report classified this as shared ROM behavior
  and identified the then-missing Level 11 C# integration.
- The 2026-11-21 correction report added the bounded Level 11 translation:
  `PendingGenericOriginalCount > 0` restarts the live Spiker; zero converts the
  same original using `ConvertActiveSpikerToTanker` and the existing Tanker
  construction. Two latent cargo identities are owned by that original.
- The 2026-11-23 test-resolution report found an overbroad test assertion, not
  a production conversion failure. It requires the converting original to stop
  being a Spiker, while allowing unrelated Spikers to remain.

These reports distinguish ROM-backed behavior from the C# identity/ledger
translation and the approximate C# return boundary. Their similarity to the
Level 15 symptom was treated only as a hypothesis until verified below.

## Atari disassembly evidence

Primary implementation evidence was read from the project-local
`Reference/arcade/Tempest Rom Source code.md`; no web or raw-ROM search was
used.

**ROM-BACKED lifecycle and pending value**

- `set_enm_and_spikes` (`$9234-$9245`) copies `wave_enemies` into
  `enemies_pending` and copies `wave_spikeht` into the Spike-tip slots.
- A pending descriptor is decremented at `$993C` only after a new enemy is
  successfully initialized. Thus `enemies_pending` is generic supply still
  waiting to enter, not the count of active Spikers, active Tankers, or all
  enemies still alive. The death/recovery path can rebuild pending supply from
  in-tube/top enemies (`$A56A-$A57B`); the endpoint decision still directly
  tests the current generic pending value.
- The shared `eact_0e` action (`$9FC6-$A027`, selected by the common Spiker
  program at `$A0F7-$A0FD`) updates the Spike tip while the Spiker moves from
  `$F0` toward `$20`, clamps/reverses at `$20`, and returns. At the return
  endpoint it performs the lane hop and restores the same slot's along value
  to `$F0`, then checks `enemies_pending` at `$A009`.
- If pending is nonzero, the Spiker program continues its Spiker loop. If zero,
  `$A00E-$A024` changes the type/attribute bytes in that same enemy slot to a
  Tanker and sets its contents to Flippers. This is an in-place conversion,
  not destruction followed by a newly admitted Tanker.
- The ordinary picker separately applies per-type minima/maxima and accounts
  for Tanker payload as two units. None of those ordinary selection checks is
  called by the `$A009` terminal conversion. Nor does the endpoint test inspect
  other active enemies.
- Level 15's ordinary per-level data affects supply/selection (including a
  Spiker minimum of 2 and maximum of 3 in the applicable table ranges), and
  its `wave_spikeht` entry is `$A0`. It does not select a different terminal
  Spiker routine or change the shared zero-pending rule. The current C#
  candidate's supply/selection still uses its explicitly DEVELOPMENT ONLY
  Level 11 baseline; the Level 15 ROM row is not a reason to expand this task
  into replacing that accepted candidate configuration.

The ROM establishes a Flipper-carrying Tanker payload. It does not have the
C# ledger's stable cargo identity objects. The C# representation of the
payload as two owned latent cargo identities is a PROJECT TRANSLATION.

## Current C# terminal path

1. `Spiker.Update` builds, retreats, and finally marks the object
   `ReturnedToReserve`; it contains no Tanker conversion.
2. `TempestGame.UpdateSpikers` only obtains
   `PendingGenericOriginalCount` for Level 7/9 and the ROM-policy flags for
   Levels 11-14. It omits `UsesLevel11ParameterDevelopmentBaseline`, which is
   the flag set on this Level 15 candidate.
3. The same omission exists in the `pending > 0` restart eligibility and in
   the Level 11-14 terminal conversion predicate. Level 15 therefore does not
   enter an existing restart/conversion branch. Its local pending value
   defaults to zero, but that does not make it eligible for conversion.
4. On `ReturnedToReserve`, the fallback removes the Spiker and calls
   `CurrentLevel.ReturnOriginalToReserve`. `Level3WaveLedger.ReturnSpikerToReserve`
   changes that original from `Spiker/ActiveSpiker` to
   `Unselected/Reserve`. It does not destroy or replace the identity.
5. `PendingGenericOriginalCount` counts original entries in `Reserve` or
   `Egg`. Therefore, when it was genuinely zero at the terminal boundary,
   returning the original creates one new pending generic obligation.
6. On later hatch, `CreateLevel3MixedEnemy` selects through
   `Level11WaveSelection` for the parameter-baseline flag. Its minimum is two
   Spikers and maximum is three. If the returned Spiker left fewer than two
   active, Spiker is available and the unmet minimum deterministically selects
   Spiker (subject to normal active-capacity admission). At/above the minimum,
   Spiker remains eligible below its maximum; Tanker/flipper and latent-cargo
   limits can narrow the other choices. This can produce further Spiker
   reselection without implying a movement defect.

Thus the zero-pending return path does create fresh pending supply and can
explain endless cycles. When pending is positive, current Level 15 also
returns the Spiker rather than using the existing shared restart behavior.

`PendingGenericOriginalCount` is the established C# PROJECT TRANSLATION of
generic Atari pending supply: Reserve/Egg originals count; active originals
and cargo do not. At a proper zero-pending conversion, the existing ledger
method changes the same ID from `Spiker/ActiveSpiker` to
`Tanker/ActiveTanker`, marks it converted, and allocates two distinct latent
cargo IDs with that original as owner. The new `Tanker` is constructed with
the same original ID and lane. Because the original remains active and both
cargo obligations remain unresolved, generic pending count stays unchanged
(zero); conversion does not create another generic original.

The terminal path directly invokes ledger conversion and constructs the
Tanker. It does not pass through ordinary Tanker selection or its maximum,
and that is necessary: ROM terminal conversion bypasses ordinary selection
limits and occupies the existing Spiker slot. Other active enemies likewise
must not block it; the trigger is generic pending supply only. The existing
Level 11 identity-specific tests cover conversion despite ordinary Tanker
capacity and two latent cargo IDs.

Completion remains protected. Production completion requires no unresolved
originals/cargo and no active Tankers, Spikers, or other enemies. The all-rim
path validates active original/cargo representations against the ledger
before resolving them. A converted active Tanker plus its unresolved cargo
cannot silently satisfy completion.

## Regression classification and Spike-height causality

**Classification: unwired direct Level 15 candidate, not a regression.**
Repository history shows commit `064665e` (`Complete accepted Level 11`,
2026-11-22) as the introduction point for the Level 11 parameter-development
flag and the Level 11 terminal-conversion gate. At that introduction the
conversion gate used `UsesLevel11RomWavePolicy`; the Level 15 candidate's
separate parameter-baseline flag was not part of the terminal gate. The
Level 15 candidate and this omission were present from the same development
baseline; no history evidence shows that this candidate previously converted
and was later broken. The terminal implementation was subsequently extended
to production Levels 12-14, still without the separate Level 15 candidate
flag.

The accepted `$A0` correction is not the root cause. The present working-tree
diff for that correction changes Level 15's starting-height constant and
configuration assignment (plus its focused test); it does not change
`Spiker.Update`, `UpdateSpikers`, reserve/pending ledger methods, conversion,
Tanker construction, cargo ownership, or selection. There is a limited timing
relationship: `Spiker.Update` compares its climbing height to `Spike.Height`
before adding to `Growth`, and its growth threshold can start retreat. Changing
the initial Spike tip can therefore affect when that Spiker reaches the
growth/retreat portion of its cycle. It cannot change the omitted terminal
eligibility or the reserve/identity transition. Keep the accepted Spike
correction frozen and classify the terminal defect separately.

## Deterministic/headless investigation and validation

Test platform: .NET SDK `9.0.304`; VSTest with xUnit v2, target `net9.0`.

Command run, bounded with a 2-minute VSTest hang timeout:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --no-restore --filter "FullyQualifiedName~Level15TubeCandidateTests|FullyQualifiedName~Level11ProductionCorrectionTests" --blame-hang-timeout 2min
```

**PASS:** 18 passed, 0 failed, 0 skipped. This includes the existing seeded
Level 15 headless test (which advances 1,000 ticks and checks broad identity/
capacity invariants) and the Level 11 identity-specific terminal-conversion
tests. It compiles/runs the existing focused tests; no full suite or solution
build was needed.

The Level 15 seeded test does not assert pending count at a Spiker endpoint,
record converting IDs, or distinguish a same-ID Tanker from a returned/reused
Spiker. A targeted test search found no existing Level 15 terminal-conversion
test. Thus the automated run did not independently expose the terminal
transition. Static control-flow and ledger evidence above establish the cause;
no new diagnostic infrastructure or test was created.

No graphical game was launched. No complete solution test suite, separate
Release build, or `git diff --check` was run; those are not necessary for this
read-only investigation and are not claimed as passed.

## Smallest correction and focused regression plan (not implemented)

Reuse `TempestGame.UpdateSpikers`' existing Level 11-14 lifecycle and
`Level3WaveLedger.ConvertActiveSpikerToTanker`. Keep Level 15 marked as a
direct-start DEVELOPMENT ONLY parameter baseline; do not relabel it as
`UsesLevel11RomWavePolicy`.

The narrow eligibility correction is to include
`UsesLevel11ParameterDevelopmentBaseline` in the existing Level 11-equivalent
terminal lifecycle at all three relevant points: obtaining the ledger's
`PendingGenericOriginalCount`, restarting the same Spiker when it is positive,
and converting when it is zero. This reuses the already shared path for the
current direct Level 15 candidate; it does not authorize a broader policy
refactor or changes to Levels 1-14.

Future focused deterministic coverage should:

1. With Level 15 pending generic originals present, advance one identity to
   its terminal return and assert that the same identity remains
   `Spiker/ActiveSpiker`, restarts, and does not become Reserve/Egg or Tanker.
2. With the same seeded setup and all other generic originals resolved, assert
   pending count is zero before the boundary; then wait a bounded number of
   ticks for a Tanker with the exact original ID. Assert
   `Tanker/ActiveTanker`, no Spiker with that ID, and unchanged zero pending
   count afterward.
3. Assert exactly two distinct cargo IDs, ordinals 0 and 1, owned by that
   original and still `Latent`. Do not assert that every unrelated Spiker
   disappears.
4. Verify conversion proceeds despite unrelated active non-Spiker enemies,
   another legal active Spiker, and ordinary Tanker-selection capacity being
   full. The conversion is in-place and bypasses ordinary Tanker maxima.
5. Assert no false level completion/all-rim resolution while the converted
   Tanker and its cargo obligations remain unresolved; retain ledger
   representation checks.
6. Keep the accepted Level 15 `$A0` height/count/lane/growth regression
   unchanged; do not couple calibration or height to terminal conversion.

Leave `Level11ProductionCorrectionTests` unchanged, especially
`LevelElevenSpikerRepeatsWhileGenericPendingSupplyRemains`,
`LevelElevenTerminalSpikerConvertsInPlaceWithTwoCargoAndNoPendingLoss`, and
`LevelElevenTerminalConversionIgnoresOrdinaryTankerCapacityCaps`. Preserve
the existing Level 4, 5, 6, 7, 8, 9, and production Level 12-14 conversion,
selection, ledger, cargo, and all-rim regressions unchanged.

## Files and preservation

Created only this paired report and
`AI-Logs/Prompts/2026-11-24-level15-terminal-spiker-readonly-prompt.md`.
No production source, tests, configuration, durable documentation, reference
material, protected user material, or prior report was modified. Pre-existing
worktree changes remain untouched. Nothing was staged, committed, or pushed;
no branch or Git state was changed.

**Remaining manual acceptance:** after a separately authorized bounded
correction and its automated validation, Neal must manually verify that
terminal Spikers convert as intended and that Level 15 gameplay remains
accepted. This read-only task makes no new gameplay acceptance claim.
