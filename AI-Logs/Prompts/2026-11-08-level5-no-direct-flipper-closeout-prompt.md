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

Close out the manually accepted Level 5 no-direct-Flipper correction.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline before this candidate:

d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2
Close out accepted common type-0 destruction effect

Confirm the actual Git state before modifying anything.

Relevant implementation report:

AI-Logs/Reports/2026-11-08-level5-no-direct-flipper-implementation-report.md

Relevant investigation report:

AI-Logs/Reports/2026-11-08-level1-7-enemy-supply-rom-investigation-report.md

Use the project-local ROM-derived disassembly if evidence must be rechecked:

Reference/arcade/Tempest Rom Source code.md

Do not search the internet for another Tempest disassembly.


MANUAL ACCEPTANCE PROVIDED BY NEAL

Neal manually play-tested production Level 5 several times.

Manual acceptance result:

- Level 5 passes.
- Across several Level 5 playthroughs, Neal did not observe any ordinary
  Flipper entering directly from the bottom.
- This is the visible behavior the Level 5 no-direct-Flipper correction was
  intended to establish.
- Neal considers this correction accepted and wants to close out the
  milestone.

Treat this as final manual acceptance of the Level 5 no-direct-Flipper
candidate.

Do not require additional manual testing before closeout unless final automated
validation reveals a genuine regression.


ACCEPTED SCOPE

The accepted correction is narrowly:

- Production Level 5 ordinary/original enemies may not be selected as direct
  bottom-entry Flippers.
- Tankers remain valid Level 5 ordinary entrants.
- Spikers remain valid Level 5 ordinary entrants.
- Tanker-released cargo Flippers remain valid and are not blocked by this
  policy.
- The Level 5 original supply remains 20.
- The existing Level 3-6 identity/ledger model remains intact.
- Death/re-entry re-applies the Level 5 no-direct-Flipper policy to unresolved
  original supply as implemented.
- Existing cargo ownership and reconstruction remain intact.


DO NOT BROADEN THIS CLOSEOUT

Do not change:

- Level 6 selection behavior;
- Levels 1-4;
- Level 7;
- base enemy supply totals;
- Tanker concurrent limits;
- Spiker concurrent limits;
- global active-enemy limits;
- Egg/release cadence;
- ROM pending-entry timing;
- Tanker cargo rules;
- death/re-entry architecture beyond the accepted Level 5 correction;
- completion accounting;
- starting Spikes or Spike height;
- scoring;
- audio;
- movement;
- enemy firing;
- Super Zapper;
- destruction effects;
- progression;
- recording/playback functionality;
- screenshots;
- HUD behavior;
- difficulty/DIP-switch behavior;
- Levels 8 or 9;
- unrelated cleanup or refactoring.

The broader observation that C# levels can take substantially longer to clear
than the arcade remains unresolved and is not a blocker for this narrowly
accepted correction.

Do not claim this Level 5 change resolves the clear-time difference.


DOCUMENTATION CLOSEOUT

Update durable documentation appropriately:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

README.md only if genuinely necessary.

Record:

- the ROM-listing-backed Level 5 no-direct-Flipper rule;
- the implementation mechanism at an appropriate maintainability level;
- preserved Tanker cargo behavior;
- unchanged original supply of 20;
- Neal's manual acceptance after several Level 5 playthroughs;
- the fact that he observed no ordinary Flippers entering directly from the
  bottom;
- Level 6 remains unchanged and separate;
- the broader clear-time difference remains an observation for future
  investigation.

Preserve evidence classifications.

Do not relabel project ledger/reconstruction mechanics as literal ROM behavior.


FINAL VALIDATION

Run final validation against the complete manually accepted candidate.

At minimum:

1. Focused Level 5 production/no-direct-Flipper tests.

2. Expanded relevant regression coverage including:
   - Level 5 production and progression;
   - level configuration;
   - shared mixed-wave selection;
   - Level 3 mixed-wave/ledger behavior if affected by shared code;
   - Tankers and cargo;
   - Flippers;
   - Spikers;
   - death/re-entry;
   - completion;
   - Level 4;
   - Level 6;
   - Level 7 production/selection/re-entry;
   - Super Zapper and destruction-effect regressions if affected shared paths
     warrant them.

3. Full solution test suite.

4. Full solution build.

5. git diff --check.

6. After staging, git diff --cached --check.

Do not launch the interactive game. Neal has already completed manual
acceptance.


STAGING SAFETY

Inspect the full candidate carefully before staging.

Stage only intended tracked milestone files.

The implementation report identified these candidate files:

- src/Tempest.Game/Levels/LevelConfiguration.cs
- src/Tempest.Game/Levels/Level3WaveSelection.cs
- src/Tempest.Game/Levels/Level3WaveLedger.cs
- src/Tempest.Game/Levels/Level.cs
- src/Tempest.Game/TempestGame.cs
- tests/Tempest.Game.Tests/Level5ProductionTests.cs
- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Verify the actual final changed-file set rather than blindly staging this list.

Do not stage, modify, delete, rename, or move:

- screenshot000.png through screenshot013.png;
- UserData;
- gameplay recordings;
- ignored AI-Logs;
- generated build output;
- unrelated local files.


COMMIT AND PUSH

If final validation succeeds:

1. Inspect the final tracked diff.
2. Stage only intended milestone files.
3. Verify the staged allowlist.
4. Run git diff --cached --check.
5. Commit with a clear descriptive message reflecting the accepted Level 5
   no-direct-Flipper correction.
6. Push master to the configured GitHub origin.
7. Verify after push:
   - local HEAD;
   - local origin/master;
   - live remote master if that remains part of normal project workflow;
   - tracked worktree/index cleanliness;
   - no generated/user files were committed;
   - original generic screenshots remain untouched and untracked.


REPORT

Update the implementation report or create the appropriate paired closeout
report according to existing project AI-log policy.

Preserve the implementation-stage manual-pending status as historical context,
then record the subsequent manual acceptance and final closeout.

Record:

- starting Git state;
- manual acceptance supplied by Neal;
- durable documentation changes;
- final validation commands/results;
- final staged file list;
- commit hash and message;
- push result;
- final local HEAD;
- origin/master;
- final Git status;
- excluded generated/local files;
- any genuinely unresolved issue.

Explicitly retain as deferred:

- Level 6 no-direct-Flipper evaluation/correction;
- Level 3-6 active/type-cap differences;
- ROM six-active gate investigation/implementation;
- release/pending pacing differences;
- hard-difficulty supply adjustment;
- Spike-height mapping;
- the broader arcade-versus-C# level clear-time observation.


STOP POINT

After successful documentation, validation, commit, push, and final
verification, stop.

Do not begin Level 6 or any other enemy-supply/pacing correction.

Provide Neal a concise final closeout summary including:

- final commit hash and message;
- focused validation result;
- expanded validation result;
- full-suite result;
- build result;
- diff-check result;
- push status;
- final Git status;
- confirmation that Level 5 no-direct-Flipper behavior is manually accepted
  and closed;
- confirmation that Level 6 and the broader clear-time observation remain
  separate future work.

TEMPEST_LEVEL5_NO_DIRECT_FLIPPER_CLOSEOUT_20261108
