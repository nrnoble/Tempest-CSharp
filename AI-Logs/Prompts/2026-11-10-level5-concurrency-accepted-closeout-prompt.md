FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Close out the accepted Level 5 concurrency and generic-obligation
death-reconstruction milestone.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the current uncommitted Level 5 candidate:

62c66e0ab19436ea162935627021154be21e0a41

The complete corrected Level 5 candidate is already present in the worktree.
Do not redo the implementation.

This task IS authorized to update durable accepted documentation, perform
final closeout validation, stage the exact accepted Level 5 candidate, commit
it, push master to origin, and verify local and remote repository state.
Do not begin Level 6 concurrency.


HISTORICAL HANDOFFS

Read the relevant existing Level 5 reports, especially:

- AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md
- AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md
- AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md
- AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md
- AI-Logs/Reports/2026-11-09-level5-cargo-post-death-restoration-implementation-report.md
- AI-Logs/Reports/2026-11-09-level5-cargo-restoration-compile-fix-validation-report.md
- AI-Logs/Reports/2026-11-10-level5-generic-obligation-death-reconstruction-report.md
- AI-Logs/Reports/2026-11-10-level5-final-generic-obligation-validation-report.md

These are historical records. Do not rewrite earlier reports to erase rejected
experiments, former blockers, or former pending states.


STARTING-STATE VERIFICATION

Before modifying durable documentation, verify branch, HEAD, fetch origin
normally, verify origin/master and direct remote refs/heads/master if
appropriate, upstream/tracking, ahead/behind, index, complete tracked
worktree, nonignored untracked state, and inspect the complete Level 5
candidate diff.

Confirm that the candidate matches the exact candidate validated in
2026-11-10-level5-final-generic-obligation-validation-report.md. It recorded
these six modified tracked files and two untracked candidate files:

- src/Tempest.Game/Levels/Level.cs
- src/Tempest.Game/Levels/Level3WaveLedger.cs
- src/Tempest.Game/Levels/LevelConfiguration.cs
- src/Tempest.Game/TempestGame.cs
- tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs
- tests/Tempest.Game.Tests/Level5ProductionTests.cs
- src/Tempest.Game/Levels/Level5AdmissionPolicy.cs
- tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs

Do not assume this list overrides actual Git state. If the candidate
materially differs from the validated/manual-tested version, stop and report
the discrepancy.


FINAL MANUAL ACCEPTANCE

Neal completed final manual gameplay acceptance of the exact corrected Level
5 candidate. Manual result: PASS.

Neal reports:

"The bug no longer is present in level 5. Level 5 plays as expected. No
regressions in levels 1-4."

This acceptance incorporates Neal's direct arcade observations:

- Level 5 never has individual Flippers independently entering from the
  deep/bottom end, including after Blaster death/reentry.
- Pre-death Flippers do not reappear on the rim after Blaster death.
- Unresolved active-enemy quantity is conserved through death reconstruction.
- Four unresolved enemies become four unresolved obligations.
- Eight unresolved materialized cargo Flippers become eight unresolved
  obligations, not four former-Tanker obligations.
- Reconstructed obligations have no predetermined species identities.
- Reconstructed generic obligations later use normal Level 5 Tanker/Spiker
  selection, not Flipper selection.

Treat these as ARCADE-OBSERVED only at the specificity Neal established; do
not extend them into unobserved internal claims.


AUTOMATED VALIDATION ALREADY ESTABLISHED

For the exact corrected candidate, historical fresh validation recorded:

- Smallest generic-obligation reconstruction group: 7 passed, 0 failed,
  0 skipped.
- Complete focused Level 5/ledger/cargo groups: 88 passed, 0 failed,
  0 skipped.
- Affected regressions: 767 passed, 0 failed, 0 skipped.
- Full solution Release tests: 1,171 passed, 0 failed, 0 skipped.
- Full solution Release build: 0 warnings, 0 errors.
- git diff --check: PASS.
- VSTest executed in every test stage.
- No Windows Application Control block or security error occurred during
  final validation.
- All eight candidate files had matching start/end validation fingerprints.

Treat these as historical validation results for the exact candidate, but
perform the required final closeout validation after documentation changes.


ACCEPTED ROM-BACKED LEVEL 5 CONCURRENCY

Document supported ROM-backed behavior accurately:

- Level 5 Easy/Medium generic pending supply is 20.
- Ordinary selectable species are Tanker and Spiker.
- Ordinary Flipper minimum/maximum is 0/5; Tanker 1/1; Spiker 2/3.
- Pulsar and Fuseball ordinary availability is zero.
- Seven shared physical enemy places are used.
- Unmet Spiker minimum precedes unmet Tanker minimum.
- Positive Flipper availability does not make Flipper ordinarily selectable
  because its minimum is zero.
- Latent Tanker cargo reduces Flipper availability but occupies no physical
  enemy place.
- Released cargo bypasses ordinary species selection.
- Ordinary species availability may stop admission while physical places
  remain free.
- Terminal/no-pending Spiker behavior converts the existing live Spiker into
  a Flipper-carrying Tanker rather than ordinarily admitting a new Tanker.
- Species maxima govern ordinary admission and are not absolute late-wave
  live-population caps.


ACCEPTED LEVEL 5 ADMISSION IMPLEMENTATION

Preserve evidence classification while documenting:

- Ordinary selection is Tanker/Spiker only; individual Flippers are never
  ordinary Level 5 admissions.
- Level 5 uses its own minima/maxima rather than Level 3 or Level 4 rules.
- Spiker minimum 2 is fulfilled before Tanker minimum 1.
- Seven-place physical ordinary-admission headroom is respected.
- Latent cargo affects availability.
- Blocked Eggs wait without losing identity or repeatedly consuming selection
  randomness; same-tick admission recomputes current state.
- Terminal Spiker-to-Tanker conversion is enabled for canonical Level 5.
- Converted-Tanker handling remains integrated with project identity and
  cargo accounting.
- The shortest-Spike threshold mapping remains classified as PROJECT
  TRANSLATION / APPROXIMATION. Do not claim literal coordinate equivalence.


ACCEPTED DEATH-RECONSTRUCTION MODEL

ROM-backed concept: active enemies are reconstructed into generic pending
supply after death.

Arcade-observed visible consequences:

- Unresolved quantity survives death.
- Pre-death Flipper species/presentation does not.
- Unresolved Flippers do not independently return from the bottom or
  reappear on the rim.
- Reconstructed Level 5 obligations later use Tanker/Spiker selection.

Accepted C# PROJECT TRANSLATION: the stable ledger terminalizes historical
materialized cargo identities and creates one fresh unselected generic
original obligation for each unresolved materialized cargo child.

Document explicitly:

- One unresolved materialized cargo Flipper becomes one generic obligation.
- Four become four; eight become eight.
- Two children are not collapsed into their former Tanker owner.
- Terminal/resolved cargo remains terminal.
- Latent unreleased cargo does not become additional active obligations.
- Historical Flipper species, cargo-Egg presentation, lane, height, and rim
  placement are not retained.
- Reconstructed obligations subsequently use normal Level 5 Tanker/Spiker
  selection.
- Repeated deaths conserve the then-current unresolved quantity.
- Do not call stable ledger IDs or this mapping literal Atari identity
  behavior.


REJECTED HISTORICAL TRANSLATIONS

Briefly document the accepted conclusion without rewriting historical
experiments:

- Prior cargo-Egg reconstruction was rejected because it caused individual
  Flippers to enter from the bottom after death.
- Later captured-position/in-tube Flipper restoration was rejected because
  it preserved Flipper species/presentation and could make Flippers reappear
  on the rim.
- Generic-obligation reconstruction supersedes both.
- Detailed failed experiments remain preserved in historical AI-Logs reports.


SUPER ZAPPER BOUNDARY

The Level 5 ROM investigation found that ordinary entry is paused while
zap_running is active. The accepted C# Super Zapper lifecycle has not been
redesigned around this exact ROM gate. Preserve the existing
evidence/deferment classification. Do not broaden this closeout into Super
Zapper admission-pause work or imply manual acceptance establishes hidden
zap_running timing equivalence.


DURABLE DOCUMENTATION

Update TEMPEST-DEVELOPMENT-NOTES.md and TEMPEST-BEHAVIOR-SPEC.md. Record
concisely the Level 5 concurrency investigation; accepted Level-5-specific
ordinary admission; ROM-backed minima/maxima and physical-capacity findings;
zero ordinary Flipper entry; terminal Spiker-to-Tanker behavior;
shortest-Spike project translation; generic-obligation death reconstruction;
Neal's four- and eight-obligation observations; rejected cargo-Egg and
captured-Flipper presentations; final manual acceptance; final automated
validation; project translations and remaining fidelity boundaries; and no
observed Levels 1-4 regression. Do not rewrite unrelated history.


SCOPE

Level 5 closeout only. Do not begin Level 6 concurrency, modify Levels 1-4 or
6-8, perform unrelated refactoring or comment cleanup, redesign stable ledger
architecture, implement deferred Super Zapper admission pause, revisit
unrelated visual/audio fidelity, or change Windows security configuration.


FINAL CLOSEOUT VALIDATION

After durable documentation updates:

1. Run git diff --check.
2. Run the smallest critical Level 5 generic-reconstruction/admission Release
   tests sufficient to establish documented behavior matches the candidate.
3. Run affected Release regression groups covering Level 5, cargo,
   death/reentry, ledger/accounting, Super Zapper, persistent Spikes,
   completion/all-rim, startup/playback, and Levels 1-4 concurrency.
4. If those pass, run:
   dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min
5. If full tests pass, run:
   dotnet build TempestCSharp.slnx --configuration Release
6. Run final git diff --check, complete tracked diff review, intended file-list
   review, and staged git diff --check after staging.

Do not launch the game. Manual acceptance is complete. If Windows security or
another blocker prevents required validation, follow repository self-stop
policy and do not commit/push.


AI RECORDS

Preserve all existing Level 5 investigation, implementation, validation, and
rejected-experiment reports unchanged. Create a paired closeout record:

- AI-Logs/Prompts/2026-11-10-level5-concurrency-accepted-closeout-prompt.md
- AI-Logs/Reports/2026-11-10-level5-concurrency-accepted-closeout-report.md

The report records starting baseline/state; accepted implementation;
ROM-backed and ARCADE-OBSERVED evidence; PROJECT TRANSLATION /
APPROXIMATION decisions; rejected restoration approaches; manual acceptance;
final closeout validation; committed files; commit hash; push verification;
final repository state; and explicitly deferred fidelity boundaries.
AI-Logs remain ignored and must not be committed.


COMMIT AND PUSH

Only if the candidate matches the validated/manual-accepted candidate,
documentation accurately records behavior, required final validation passes,
and no blocking discrepancy exists: stage exactly the intended Level 5
production/test changes plus TEMPEST-DEVELOPMENT-NOTES.md and
TEMPEST-BEHAVIOR-SPEC.md. Review staged file list and complete staged diff;
run staged git diff --check. Do not stage AI-Logs or protected local material.

Commit with an appropriate concise message, for example:

Implement accepted Level 5 enemy concurrency

Push master to origin. Then verify local HEAD, local origin/master, direct
remote refs/heads/master, ahead/behind 0/0, clean index, clean tracked
worktree, AI-Logs ignored/uncommitted, local ROM/reference material
ignored/uncommitted, and no unrelated files included.


STOP

Stop after Level 5 is documented, final-validated, committed, pushed, and
remote-verified. Do not begin Level 6 concurrency. Provide Neal the final
commit hash, validation results, committed-file list, push status, and
repository cleanliness.


TEMPEST_LEVEL5_CONCURRENCY_ACCEPTED_CLOSEOUT_COMPLETE
