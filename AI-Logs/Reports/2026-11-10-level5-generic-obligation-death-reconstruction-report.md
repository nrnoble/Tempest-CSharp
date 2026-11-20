# Level 5 generic-obligation death reconstruction

Date: 2026-11-10

Status: **IMPLEMENTATION CANDIDATE; FINAL VALIDATION INCOMPLETE.** The rejected
captured-position restoration path was removed and replaced with generic
obligation reconstruction. A later focused rerun exposed a compile error; the
syntax issue was corrected, but validation was not restarted under the task's
stop-on-first-failure rule. Manual acceptance remains pending.

## Evidence and model

- **ARCADE-OBSERVED (Neal):** Level 5 Flippers do not enter independently from
  the deep end, including after death/reentry. Active Flippers do not return
  on the rim. Four unresolved enemies reconstruct as four obligations; eight
  unresolved cargo Flippers reconstruct as eight obligations with no
  predetermined species. Reconstructed Level 5 obligations later become
  Tankers or Spikers, never Flippers.
- **ROM-BACKED (prior investigation):** death reconstruction returns active
  enemies to generic pending supply; Level 5 ordinary species are Tanker and
  Spiker; Flipper minimum is zero; later species are dynamically selected.
- **PROJECT TRANSLATION:** stable historical cargo IDs enter terminal
  `Reconstructed` state, and each unresolved materialized child receives a
  new unselected original identity. This preserves one obligation per child
  without keeping its Flipper species or routing it through cargo Egg
  presentation. Unreleased latent cargo is removed when its unresolved Tanker
  is genericized; terminal owners and resolved children remain terminal.
  Canonical production Level 5 opts in by configuration identity. Copies and
  Levels 1-4/6 remain outside this Level 5 policy; existing Level 7/8 generic
  reconstruction is unchanged.

## Implementation and tests

- Removed the Flipper position/state snapshot, capture and restore methods,
  Level 5 snapshot queue, capacity retry path, in-tube ledger states, and
  snapshot-only `InternalsVisibleTo` file.
- Removed canonical Level 5 cargo-Egg reentry and retained-Flipper admission.
  An unexpected cargo Egg now raises an explicit invariant error. Ordinary
  and retained Level 5 admissions use Tanker/Spiker only.
- Preserved the existing Level 5 minima/maxima, ordinary selector, headroom,
  latent-cargo reservation, waiting, terminal Spiker conversion, and normal
  live Tanker cargo lifecycle.
- Added ledger coverage for four active cargo Flippers, four mixed active
  species with latent cargo, eight cargo obligations, a terminal/unresolved
  sibling pair, pending-shot cargo, post-reconstruction Tanker cargo
  allocation, and repeated death. Added selector coverage admitting eight
  generic obligations only as Tankers/Spikers. Replaced the in-tube
  restoration integration expectation with checks for no post-death
  Flippers/cargo Eggs, conserved unresolved quantity, and normal Level 5
  species admission.
- Historical handoffs were read and left unchanged. Durable behavior
  documentation was not updated because this remains a candidate.

## Validation

On the version before the final cargo-Egg/retained-Flipper guard and direct
Level-reset scope refinement:

- New reconstruction test filter: **PASS — 7 passed**.
- Focused Level 5 admission/production and ledger groups: **PASS — 88 passed**.
- Affected gameplay/startup/regression groups: **PASS — 814 passed**.
- Full Release solution tests: **PASS — 1,171 passed, 0 failed, 0 skipped**.
- Full Release solution build: **PASS — 0 warnings, 0 errors**.

After the final selector guard and Level 5-only direct-reset refinement,
`git diff --check` passed, but the focused test command stopped during
compilation with `CS1513` in `Level5AdmissionPolicy.cs` (an extra opening
brace). The extra brace was removed. Per the task's stop-on-first-failure
instruction, tests/build were not rerun afterward. Therefore the final
candidate is **not fully validated**, despite the preceding version's passes.

## Final state, handoff, and limits

- HEAD remains `62c66e0ab19436ea162935627021154be21e0a41`. Candidate changes
  are uncommitted and unstaged; no commit or push was made.
- The game was not launched. Manual arcade acceptance has not been performed.
- After final automated validation succeeds, Neal's manual launch command is:

  `dotnet run --project src\Tempest.Game -- --level 5 --lives 10`

  Verify that unresolved released cargo does not return as Flippers from the
  bottom or rim, and that its quantity re-enters through ordinary Tanker/
  Spiker selection. Also recheck normal Level 5 cargo, Spikes, Super Zapper,
  rim combat, death/reentry, and completion.
- Remaining fidelity boundaries: this maps ROM generic supply into the
  project's stable identity ledger; it does not emulate Atari hardware or
  claim exact ROM admission cadence. Normal selection remains the existing
  C# Level 5 translation. Neal's manual acceptance is still required.
