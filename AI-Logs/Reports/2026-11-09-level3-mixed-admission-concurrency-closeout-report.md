# Level 3 mixed-admission concurrency closeout

Date: 2026-11-09

Disposition: **MANUALLY ACCEPTED, VALIDATED, COMMITTED, AND PUSHED.**

## Starting state and candidate confirmation

- Expected starting baseline: `7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`.
- Actual starting HEAD and `origin/master`: `7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`.
- Branch: `master`, tracking `origin/master`; index and tracked worktree were
  clean before closeout documentation was added.
- The candidate's production policy, tests, and changed lifecycle fixtures
  matched the implementation handoff
  ([implementation report](./2026-11-09-level3-mixed-admission-diagnostic-implementation-report.md))
  and the completed ROM investigation
  ([investigation report](./2026-11-09-level3-concurrency-rom-investigation-report.md)).
- The only pre-existing nonignored untracked material was the 21-file local
  `Reference/arcade/roms.rev2/` set. It remained unmodified and uncommitted.
- The historical investigation and implementation reports were left unchanged.

## Closeout documentation

Updated `TEMPEST-DEVELOPMENT-NOTES.md` and `TEMPEST-BEHAVIOR-SPEC.md` with the
accepted milestone, the Rev. 2 evidence boundary, the project translations,
and Neal's manual acceptance. The closeout prompt is preserved in
`AI-Logs/Prompts/2026-11-09-level3-mixed-admission-concurrency-closeout-prompt.md`.
Both AI records are ignored and were not committed.

The documentation records these ROM-backed findings:

- Level 3 ordinary Flipper/Tanker entry is availability- and minimum-driven,
  with an ordinary-entry maximum of one Tanker.
- There are seven physical enemy places; rim/live enemies continue to occupy
  them.
- The Flipper table maximum of four is not a hard four-Flipper population cap.
  Latent Tanker cargo affects Flipper availability through wrapping byte
  subtraction, followed by a physical-place clamp.
- Tanker selection requires nonzero Flipper availability, not two available
  Flipper units; released cargo bypasses ordinary species selection.
- Level 3's 15 original entries are generic pending entries, not a fixed
  Flipper/Tanker mixture.

The documentation separately classifies the accepted C# behavior as project
translation: stable ledger identities are retained; blocked Eggs wait ready
without losing selected type or identity; and Tanker release preserves the
complete two-Flipper cargo pair. At saturation, this complete pair can exceed
seven live C# enemy objects, unlike literal ROM slot allocation that may fail
to allocate the second child. The C# implementation is not claimed to be a
strict seven-live-object reproduction at every release edge case.

## Manual acceptance

Neal manually tested the candidate using the Level 3 diagnostic and passed all
eight checks:

1. Level 3 felt less overcrowded and matched arcade gameplay.
2. More than one Tanker was not observed simultaneously through normal entry.
3. Tanker destruction still released both Flippers normally.
4. Entry cadence looked natural, with no obvious freezing or unnatural gaps.
5. Flipper population varied naturally rather than appearing rigidly capped
   at four.
6. Death and reentry behaved normally.
7. Super Zapper behavior remained normal.
8. Rim combat and eventual level completion remained normal.

Neal's overall judgment: “As far as I can determine, it is perfect. Exact game
play as the arcade game.” This records acceptance of visible gameplay, not
manual observation of hidden admission internals or the saturation edge case.
The interactive game was not launched during closeout.

## Validation

- Focused Level 3 admission, mixed-wave, ledger, Tanker, death/reentry, and
  Super Zapper test filter: **PASS**, 219 passed, 0 failed.
- Full solution tests:
  `dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min`:
  **PASS**, 1,139 passed, 0 failed, 0 skipped.
- Full solution build:
  `dotnet build TempestCSharp.slnx --no-restore`: **PASS**, 0 warnings,
  0 errors.
- `git diff --check` and staged `git diff --cached --check`: **PASS**.
- Complete candidate/documentation diff and exact staged file list reviewed.
- The nine committed tracked files were:
  - `TEMPEST-BEHAVIOR-SPEC.md`
  - `TEMPEST-DEVELOPMENT-NOTES.md`
  - `src/Tempest.Game/Levels/Level3MixedAdmissionPolicy.cs`
  - `src/Tempest.Game/Levels/Level3WaveSelection.cs`
  - `src/Tempest.Game/TempestGame.cs`
  - `tests/Tempest.Game.Tests/Level3MixedAdmissionPolicyTests.cs`
  - `tests/Tempest.Game.Tests/Level3MixedWaveTests.cs`
  - `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
  - `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
- No AI-Logs, ROM files, TEMP, UserData, recordings, screenshots, videos, or
  other local material was staged.

## Commit, push, and final state

- Commit: `1d5f37f6c309dee7fface6f99d09abd85d8a7f4c`
  (`Implement accepted Level 3 enemy concurrency`).
- Push: **PASS**, `master` pushed to `origin`.
- Final local HEAD, local `origin/master`, and remote `refs/heads/master`:
  `1d5f37f6c309dee7fface6f99d09abd85d8a7f4c`.
- Final branch/tracking: `master...origin/master`, ahead 0 / behind 0.
- Final tracked index and working tree: clean.
- Protected local material remains uncommitted: 21 files under
  `Reference/arcade/roms.rev2/`.

## Deliberately unresolved fidelity boundary

The complete-pair Tanker release at saturation remains a deliberate C# project
translation. Ordinary admission follows the investigated ROM-informed
availability rules, but released cargo can temporarily exceed the ROM's seven
physical slots. Manual gameplay acceptance did not directly observe this
internal edge case, measure its frequency, or establish exact hidden timing.
No Level 4 concurrency work was started.
