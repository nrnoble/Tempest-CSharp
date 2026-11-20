# Accepted Level 4 concurrency closeout

Date: 2026-11-09

Status: **ACCEPTED, RELEASE-VALIDATED, COMMITTED, AND PUSHED.**

## Starting state and candidate review

- Expected baseline, `HEAD`, and `origin/master` were all
  `af560b100bebad672ec9231d4f88332848378594` on `master`.
- The index was empty. The worktree contained exactly four modified tracked
  candidate files and two untracked candidate source/test files. There was no
  unrelated nonignored user work.
- The candidate matched the three historical ROM investigation,
  implementation, and Release-validation handoffs. Those reports were
  preserved unchanged. Local Tempest ROM reference copies were not opened,
  hashed, or enumerated.
- The complete substantive closeout task was copied to the matching ignored
  prompt record in `AI-Logs/Prompts/`.

## Accepted behavior and evidence

**ROM-BACKED (local Rev. 2 listing):** The investigated default displayed
Level 4 has 17 generic original pending entries (Hard has 19). Ordinary
selectable species are Flippers (minimum/maximum 1/4) and Spikers (1/2);
ordinary Tanker, Pulsar, and Fuseball availability is zero. Seven physical
enemy places are shared. The unmet Spiker minimum precedes the unmet Flipper
minimum, so four Flippers plus two Spikers can exhaust ordinary availability
with one physical place unused. Rim and other still-live enemies retain
physical occupancy. Late Tankers result from terminal Spiker conversion after
generic pending supply is exhausted, not ordinary Tanker admission; released
cargo bypasses ordinary species selection.

**ACCEPTED PROJECT TRANSLATIONS / APPROXIMATIONS:** C# preserves stable
original/cargo identities and exact accounting without claiming to reproduce
the ROM's generic reconstruction mechanism. Intermediate Spiker retreat
returns the same identity through reserve/Egg while generic original supply
remains, rather than implementing live shortest-Spike hopping. The C# mapping
of reserve/Egg originals to generic pending supply controls terminal
conversion. At zero mapped pending supply, the same original identity becomes
a Tanker at the existing C# retreat boundary; its timing/height is an
approximation, not a literal ROM coordinate/timing claim. This prevents
endless final-Spiker recycling. A retained converted Tanker may reenter under
physical-headroom rules with identity/type/cargo preserved. Complete cargo
pairs are preserved on release; this can temporarily exceed seven live C#
objects, after which ordinary admission waits for capacity. No strict
seven-live-object fidelity is claimed at that saturation edge.

**MANUAL ARCADE-FIDELITY ACCEPTANCE:** Neal tested the candidate and judged,
“As far as I can determine, Level 4 is identical to the arcade game.” This is
strong acceptance of visible Level 4 gameplay he tested, not a claim about
hidden ROM internals. Manual acceptance does not promote the C# translations
to ROM-backed facts.

## Final validation

- Focused Level 4 Release tests with VSTest hang protection: **39 passed, 0
  failed, 0 skipped**.
- Full solution Release tests with VSTest hang protection: **1,154 passed, 0
  failed, 0 skipped**.
- Full solution Release build: **0 warnings, 0 errors**.
- Staged `git diff --check`: **PASS**; the final tracked worktree is clean.
- The historical Debug VSTest attempt remains **BLOCKED** before test
  execution by Windows Application Control `0x800711C7`. It was not a test
  pass or a gameplay defect. Release was the separate successful validation
  route; no security policy was weakened or bypassed.
- The interactive game was not launched. Manual acceptance was already
  complete.

## Committed files

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level4AdmissionPolicy.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level4AdmissionPolicyTests.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`

Commit: `62c66e0ab19436ea162935627021154be21e0a41`

Push: **PASS** — `master` was pushed to `origin`.

## Final repository state

- Local `master`, local `origin/master`, and remote `refs/heads/master` resolve
  to `62c66e0ab19436ea162935627021154be21e0a41`.
- Ahead/behind: `0 / 0`.
- Index and tracked worktree: clean.
- AI-Logs prompt/report records and the three historical handoffs remain
  ignored and uncommitted.
- No protected local material was committed or changed.

## Deliberately unresolved fidelity boundaries

This closeout does not claim literal ROM generic-identity reconstruction,
shortest-Spike hopping during intermediate retreat, exact C# retreat
coordinates/timing, or strict seven-live-object behavior during complete-pair
cargo saturation. It does not begin Level 5 concurrency or change other
levels.
