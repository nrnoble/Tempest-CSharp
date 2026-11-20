FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Close out the manually accepted common type-0 Blaster-hit destruction-effect
milestone.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline before this candidate:

4604297920f90c96731abd727ca3c73660300e88
Add manually accepted paused-playback screenshot capture

Confirm the actual Git state before making any changes.

Relevant implementation report:

AI-Logs/Reports/2026-11-08-common-type0-destruction-implementation-report.md

Relevant investigation report:

AI-Logs/Reports/2026-11-07-enemy-destruction-rom-investigation-report.md

Use the project-local ROM-derived disassembly if ROM evidence must be rechecked:

Reference/arcade/Tempest Rom Source code.md

Do not search the internet for another Tempest disassembly.


MANUAL ACCEPTANCE PROVIDED BY NEAL

Neal has now manually play-tested the common type-0 destruction-effect
candidate against the arcade presentation.

Final manual acceptance:

- The enemy destruction explosion looks identical to the arcade game.
- The explosion geometry/drawing is correct.
- The animation timing is perfect.
- Neal used the accepted recorded-gameplay playback controls to pause at the
  destruction event and inspect the explosion frame by frame.
- Forward and reverse frame stepping/scrubbing allowed direct examination of
  the individual explosion phases.
- This frame-by-frame inspection confirmed the visual sequence and timing to
  Neal's satisfaction.

Treat this as final manual arcade-fidelity acceptance of the implemented common
type-0 Blaster-hit destruction effect.

Do not continue visual tuning merely because the earlier implementation report
listed MAME visual equivalence as pending. That pending status is superseded by
Neal's manual acceptance above.


ACCEPTED IMPLEMENTATION SCOPE

The accepted candidate implements the common type-0 normal-Blaster-hit
destruction effect for:

- ordinary Flippers;
- Tankers;
- materialized Tanker-cargo Flippers;
- Spiker bodies;
- Pulsars;
- enemy projectiles.

The implementation uses the corrected ROM-derived type-0 program documented
during implementation:

- fixed eight-record effect pool;
- deterministic ROM-derived slot selection/replacement behavior;
- captured lane/depth;
- simulation-owned lifecycle;
- ages 0, 3, 6, and 9 with deactivation on reaching 12;
- ROM-derived size-1, size-1, size-2, size-3 picture progression;
- project translation of ROM color index 0 using the existing C# color mapping;
- rendering through the project's normal perspective/vector conventions.

Preserve the correction made during implementation: the earlier investigation
report's $1b18...$1b04/multicolor interpretation was incorrect for common
type 0. The implementation report identifies the corrected type-0 vector
program at $14c2, $14f8, and $1522 and the associated vector-program ranges.

Do not revert that correction simply because the earlier report or prompt used
the older interpretation.


STILL EXCLUDED / DEFERRED

This closeout must not implement or change:

- Spike erosion effects;
- Fuseball/Fuzzball special destruction effects or ROM effect types 2-4;
- Super Zapper destruction presentation;
- player death presentation;
- Game Over;
- scoring;
- audio selection or volume;
- enemy movement or firing;
- Tanker cargo behavior;
- wave-ledger behavior;
- level completion timing;
- recording format;
- playback controls;
- screenshot controls;
- level geometry or supply;
- Levels 8 or 9;
- unrelated cleanup or refactoring.

Fuseball/Fuzzball destruction remains a separate deferred investigation and
implementation milestone.


CLOSEOUT WORK

1. Inspect the complete current Git state.

2. Read the implementation report and enough of the changed candidate to verify
   that the worktree still represents the manually accepted implementation.

3. Record Neal's manual acceptance in the durable documentation, particularly:

   - TEMPEST-DEVELOPMENT-NOTES.md
   - TEMPEST-BEHAVIOR-SPEC.md

   Update README.md only if existing user-facing documentation genuinely
   requires a closeout change.

4. Update the implementation report so its historical final section records the
   subsequent manual acceptance rather than leaving the candidate described as
   pending.

   Preserve the implementation report as a historical record:
   - do not erase the fact that manual acceptance was pending when the original
     implementation prompt finished;
   - add the subsequent manual acceptance and closeout result clearly.

5. Preserve evidence classifications.

   Distinguish:
   - ROM-LISTING-BACKED mechanics/data;
   - PROJECT TRANSLATION choices;
   - Neal's MANUAL ARCADE-FIDELITY ACCEPTANCE.

   Neal's visual acceptance establishes that the resulting C# explosion drawing
   and timing match the arcade presentation to his satisfaction. Do not relabel
   project translation choices as literal ROM facts when they are not.

6. Do not modify the accepted production implementation unless a genuine
   closeout defect is discovered.

7. Preserve all generated/local user data.

   Do not stage, commit, delete, rename, or modify:
   - screenshot000.png through screenshot013.png;
   - UserData screenshots;
   - gameplay recordings;
   - ignored AI-Logs records;
   - unrelated local/generated files.


FINAL VALIDATION

Run final validation against the complete accepted candidate.

At minimum:

1. Focused common type-0 destruction-effect tests.

2. Expanded relevant regression coverage including:
   - player-shot collision;
   - Flippers;
   - Tankers/cargo;
   - Spikers/Spikes;
   - Pulsars;
   - enemy projectiles;
   - scoring;
   - relevant audio behavior;
   - wave ledger;
   - level completion;
   - rendering;
   - gameplay recording/playback;
   - visible playback and scrubbing.

3. Full solution test suite.

4. Full solution build.

5. git diff --check.

6. git diff --cached --check after staging.

7. Verify generated UserData, screenshots, recordings, and AI-Logs are excluded
   from the commit.

Do not launch the interactive game. Neal has already completed manual gameplay
and frame-by-frame visual acceptance.

If validation fails, investigate only failures plausibly caused by this
milestone or closeout documentation changes.

Do not commit or push a failing candidate.


COMMIT AND PUSH

If all validation succeeds:

1. Inspect the complete final diff.

2. Stage only the intended tracked files belonging to this milestone.

Expected implementation files include:

- src/Tempest.Game/Gameplay/EnemyDestructionEffectPool.cs
- src/Tempest.Game/TempestGame.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/EnemyDestructionEffectTests.cs
- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Include any additional tracked documentation file only if it was legitimately
modified as part of this accepted milestone.

3. Verify the staged allowlist before committing.

4. Commit with a clear descriptive message reflecting the accepted enemy
destruction effect.

5. Push master to the configured GitHub origin.

6. After pushing, verify:
   - local HEAD;
   - local origin/master;
   - live remote master if the normal project workflow checks it;
   - tracked worktree/index cleanliness;
   - generated screenshots/UserData/recordings were not committed;
   - AI-Logs were not committed;
   - the original generic screenshots remain untouched.


REPORT

Update/create the paired ignored AI closeout record according to project
policy.

Record:

- starting Git state;
- manual acceptance supplied by Neal;
- documentation changes;
- final validation commands/results;
- final staged file list;
- commit hash;
- commit message;
- push result;
- final local HEAD;
- origin/master state;
- final Git status;
- generated/local files deliberately excluded;
- any genuinely unresolved issue.

The durable documentation should make clear that the common type-0 destruction
effect is now accepted and frozen unless a future regression or new arcade/ROM
evidence justifies revisiting it.

Fuseball/Fuzzball special destruction remains deferred and must not be silently
treated as resolved by this milestone.


STOP POINT

After successful documentation, validation, commit, push, and final
verification, stop.

Do not begin the Fuseball/Fuzzball destruction milestone or any other gameplay
work.

Provide Neal a concise final closeout summary containing:

- final commit hash and message;
- focused validation result;
- expanded validation result;
- full-suite result;
- build result;
- diff-check result;
- push status;
- final Git status;
- confirmation that the common type-0 destruction effect is now manually
  accepted and closed;
- deferred Fuseball/Fuzzball status.

TEMPEST_COMMON_TYPE0_DESTRUCTION_CLOSEOUT_20261108
