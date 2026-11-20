# Level 5 concurrency accepted closeout

Date: 2026-11-10

Status: **CLOSED — documented, validated, committed, pushed, and
remote-verified.** No Level 6 concurrency work was started.

## Starting state and candidate verification

- Branch: `master`; expected baseline and starting HEAD:
  `62c66e0ab19436ea162935627021154be21e0a41`.
- Fetched `origin` before edits. Local `origin/master`, the direct remote
  `refs/heads/master`, and upstream tracking resolved to the expected
  baseline; starting divergence was 0/0.
- The index was clean. The complete candidate consisted of the six modified
  tracked files and two untracked policy files recorded in the final generic
  obligation validation handoff. No additional nonignored untracked changes
  were present.
- Reviewed the complete production/test candidate and confirmed it matched
  the validated candidate's reported scope. All eight candidate SHA-256
  fingerprints were unchanged across final validation.
- Historical Level 5 reports were left unchanged.

## Accepted behavior and evidence

**ROM-BACKED (local Rev. 2 listing):** Easy/Medium Level 5 has 20 generic
pending originals (Hard has 23). Ordinary limits are Flipper 0/5, Tanker
1/1, Spiker 2/3, Pulsar 0/0, and Fuseball 0/0. Seven shared physical enemy
places are used. Unmet Spiker minimum precedes Tanker minimum. Flipper's
positive maximum-derived availability cannot make it ordinarily selectable
because its minimum is zero. Latent Tanker cargo reduces Flipper availability
without occupying a physical place, and released cargo bypasses ordinary
selection. Species availability can stop admission with places unused.
At zero generic pending supply, the existing live Spiker converts in place
to a Flipper-carrying Tanker; this is not ordinary Tanker admission.
Species maxima govern ordinary admission, not absolute late-wave live
populations.

**ACCEPTED PROJECT TRANSLATION:** Canonical production Level 5 now uses its
own Tanker/Spiker admission policy, minima/maxima, seven-place ordinary
headroom, latent cargo accounting, and terminal Spiker conversion. Blocked
ready Eggs retain identity and do not repeatedly consume selection
randomness; admission recomputes current state. Converted Tankers preserve
project identity/cargo accounting and reenter based on physical headroom
rather than ordinary Tanker maximum.

**PROJECT TRANSLATION / APPROXIMATION:** The shortest-Spike `$CC` threshold
uses the existing ROM-along/world-height mapping (approximately 64.29 C#
world-height units), not literal ROM coordinates. Stable ledger identities,
waiting Eggs, admission cadence, the C# retreat boundary, and reconstructed
obligation IDs are project mechanisms. Full cargo pairs retain their
identities and can temporarily exceed seven live C# objects; ordinary
admission waits for headroom.

**ROM-BACKED CONCEPT / ARCADE-OBSERVED CONSEQUENCE:** ROM death
reconstruction returns active enemies to generic pending supply. Neal
observed quantity conservation without retaining Flipper species or
presentation: individual Flippers do not enter from the bottom after
death/reentry or reappear on the rim, and reconstructed Level 5 obligations
later use ordinary Tanker/Spiker selection. He observed four unresolved
enemies becoming four obligations and eight unresolved materialized cargo
Flippers becoming eight obligations rather than four former-Tanker
obligations.

**ACCEPTED C# RECONSTRUCTION TRANSLATION:** Each unresolved materialized
cargo child maps to one fresh, unselected generic original obligation; the
two children are not collapsed into their former Tanker. Historical cargo
identities become terminal, resolved cargo remains terminal, and latent
unreleased cargo does not add obligations. Historical Flipper species,
cargo-Egg presentation, lane, height, and rim placement are not retained.
Repeated deaths preserve the then-current unresolved quantity. Stable IDs
and this mapping are not literal Atari identity behavior.

**REJECTED TRANSLATIONS:** Cargo-Egg reconstruction was rejected because it
sent individual Flippers from the bottom after death. Captured-position
Flipper restoration was rejected because it retained Flipper presentation
and could return Flippers to the rim. The historical reports retain the
experiments.

## Manual acceptance

Neal's final result for this candidate was **PASS**:

> The bug no longer is present in level 5. Level 5 plays as expected. No
> regressions in levels 1-4.

This is visible gameplay acceptance only; it does not establish hidden ROM
scheduling, coordinate, or `zap_running` timing equivalence.

## Final validation

After both durable documents were updated:

- `git diff --check`: **PASS**.
- Critical generic-obligation reconstruction/admission Release group:
  **7 passed, 0 failed, 0 skipped**; VSTest executed.
- Focused Level 5 admission/production and ledger groups:
  **88 passed, 0 failed, 0 skipped**; VSTest executed.
- Affected Release regressions covering Level 5, cargo, death/reentry,
  ledger/accounting, Super Zapper, persistent Spikes, completion/all-rim,
  startup/playback, and Levels 1-4 concurrency:
  **767 passed, 0 failed, 0 skipped**; VSTest executed.
- Full solution Release tests:
  **1,171 passed, 0 failed, 0 skipped**; VSTest executed.
- Full solution Release build: **PASS, 0 warnings, 0 errors**.
- Staged `git diff --cached --check`: **PASS**. Complete staged diff and
  exact file list reviewed.
- Candidate fingerprints matched before and after the validation stages.
- No Windows Application Control block or other security error occurred.
  The interactive game was not launched.

## Commit and push

- Commit: `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`
- Message: `Implement accepted Level 5 enemy concurrency`
- Pushed `master` to `origin` successfully.
- Post-push local HEAD, local `origin/master`, and direct remote
  `refs/heads/master` all resolve to the commit above; ahead/behind is 0/0.

Committed files:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs`
- `tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs`

## Final repository state and boundaries

- `master` tracks `origin/master`; index and tracked worktree are clean.
- Prompt and report are under ignored `AI-Logs/` and are not committed.
- No local ROM/reference material was changed or included. The existing
  tracked reference listing remains unchanged.
- No unrelated files were included.
- ROM ordinary entry pauses during `zap_running`; the accepted C# Super
  Zapper lifecycle was not changed to match this gate. Manual acceptance
  does not establish hidden zap timing. Exact Atari admission cadence,
  coordinate/timing equivalence, C# hard-mode supply variation, and strict
  seven-live-object fidelity during complete cargo-pair release remain
  deferred.
- Level 6 concurrency was not started.
