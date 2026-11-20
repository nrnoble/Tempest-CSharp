# Level 4 ordinary admission and terminal Spiker diagnostic

Date: 2026-11-09

Status: **IMPLEMENTATION CANDIDATE. Automated tests are blocked before test
execution; manual acceptance is pending.**

## Starting state

- Expected baseline: `af560b100bebad672ec9231d4f88332848378594`.
- Starting HEAD matched the baseline on `master`, tracking `origin/master`.
- Starting index, tracked worktree, and nonignored untracked state were clean.
- The required prior handoff was read:
  [Level 4 concurrency and ordinary admission investigation](2026-11-09-level4-concurrency-rom-investigation-report.md).
- The substantive user prompt is preserved at
  [implementation prompt](../Prompts/2026-11-09-level4-ordinary-admission-terminal-spiker-diagnostic-implementation-prompt.md).

## Implementation

- Added a Level 4-specific admission snapshot and policy with seven physical
  places, Flipper 1/4 and Spiker 1/2 minimum/maximum, Spiker-first unmet
  minimum handling, and bounded selection among currently eligible ordinary
  types. Unselected originals cannot be ordinarily admitted as Tankers.
- The policy is enabled only when the runtime configuration is the canonical
  `LevelConfiguration.Level4Candidate` instance. Copied/synthetic
  configurations keep their prior behavior; Level 3 and Levels 5-8 policy
  branches are unchanged.
- Each ready Egg uses a fresh snapshot of live Flippers, Tankers, Spikers,
  Pulsars/Fuseballs, and pending shot cargo. Rim, transition, and pending-zap
  enemies remain counted through their live runtime collections. Eggs,
  reserves, latent cargo, bullets, and persistent Spikes are not physical
  occupants. Denial occurs before type commitment and enemy construction, so
  a blocked Egg remains ready and waiting.
- Added `PendingGenericOriginalCount` as the Level 4 mapping of ROM
  `enemies_pending == 0`: only originals in ledger Reserve or Egg states
  count. Active and terminal originals and separately owned cargo do not.
  The existing Spiker retreat boundary converts the same original identity
  when that count reaches zero, then creates the ordinary owned cargo pair
  through the existing ledger helper. The C# height-zero retreat boundary is
  an **APPROXIMATION / PROJECT TRANSLATION**, not literal ROM coordinate or
  timing fidelity. Intermediate retreat/reserve/Egg behavior remains in
  place while generic original supply remains.
- Retained committed originals, including converted Tankers, and cargo
  reentry use physical headroom rather than ordinary type selection. This
  preserves identity and cargo ownership without allocating a second cargo
  pair. Ordinary unselected originals remain restricted to Flipper/Spiker.
- Existing complete-pair Tanker release remains unchanged. It can temporarily
  exceed seven live objects; the policy blocks further ordinary hatches until
  physical headroom returns.
- Updated the Level 4 configuration comment to label admission and terminal
  conversion as a candidate, not accepted gameplay behavior. Durable accepted
  behavior documents were not changed.

## Files changed

- `src/Tempest.Game/Levels/Level4AdmissionPolicy.cs` — new policy and snapshot.
- `src/Tempest.Game/Levels/Level3WaveLedger.cs` — pending original supply count.
- `src/Tempest.Game/Levels/LevelConfiguration.cs` — candidate-status comment.
- `src/Tempest.Game/TempestGame.cs` — canonical policy wiring and gated
  terminal conversion.
- `tests/Tempest.Game.Tests/Level4AdmissionPolicyTests.cs` — new arithmetic,
  occupancy, waiting, randomness, cargo overflow, reentry, and pending-supply
  tests.
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs` — terminal conversion
  tests for one/two Spikers, same-tick ordinary hatching, canonical isolation,
  and replacement of superseded Level 4 picker expectations.
- `AI-Logs/Prompts/2026-11-09-level4-ordinary-admission-terminal-spiker-diagnostic-implementation-prompt.md`
  — complete substantive user prompt.
- This report.

## Validation

- **PASS — build phase of focused command:** the invocation compiled
  `Tempest.Game` and `Tempest.Game.Tests`.
- **BLOCKED — focused test execution:**
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter "FullyQualifiedName~Level4AdmissionPolicyTests|FullyQualifiedName~Level4CandidateTests" --blame-hang-timeout 5min`
  built the test assembly, but VSTest could not load
  `tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.dll`.
  Windows Application Control reported: “An Application Control policy has
  blocked this file” (`0x800711C7`) before test code ran. This is an
  infrastructure/security block, not a test pass or a gameplay failure.
- Full solution tests and a separate full solution build were not run after
  the focused test execution was blocked. The repository instructions prohibit
  bypassing the control or repeatedly rebuilding/retrying the same blocked
  artifact.
- **PASS — `git diff --check`.** Final static review covered the complete
  tracked diff and both new source/test files.
- **PASS — repository state:** HEAD remains the expected baseline on
  `master` tracking `origin/master`; only the four intended tracked files and
  two new source/test files are changed. The prompt and this report are
  ignored by `AI-Logs/`. No files are staged.
- No game was launched; no commit or push was made.

## Unresolved questions and handoff

- Test behavior remains unverified on this machine because Application
  Control prevented loading the built game assembly. Do not weaken or bypass
  that policy; a later validation attempt must use a materially different,
  approved route.
- The bounded eligible-species selection distribution and the current C#
  retreat boundary still need Neal's manual arcade comparison. The report is
  historical implementation evidence, not acceptance of ROM fidelity.
- Manual gameplay acceptance remains pending. After automated validation is
  available, Neal can launch the Level 4 diagnostic with:

  `dotnet run --project src\Tempest.Game -- --level4-test --lives 10`

  Observe ordinary admission density and cadence, Spiker growth/retreat and
  terminal conversion, converted Tanker/cargo behavior, death/reentry, Super
  Zapper, Spike/rim combat, completion, and Level 4 exit. In particular,
  verify there are no ordinary Tanker arrivals, ordinary Spikers do not exceed
  two, and the final wave does not recycle the last Spiker indefinitely.

Manual acceptance is **PENDING**. Neal determines whether this candidate is
accepted, revised, or rejected.
