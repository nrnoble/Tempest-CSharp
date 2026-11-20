FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain that the supplied user task appears
incomplete.


TASK

Close out the complete manually accepted production Level 8 milestone.

This closeout includes the accumulated Level 8 implementation, recovery,
orientation, active-exit, spinner-direction, and Spike-retry corrections.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline beneath the unstaged Level 8 candidate:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

Confirm the actual Git branch, HEAD, index, worktree, and untracked state before
modifying anything.

The working tree contains the complete unstaged Level 8 candidate.

Preserve that candidate.

Do not restart or reimplement Level 8.

Do not begin Level 9.


REQUIRED PROJECT RECORDS

Read the relevant Level 8 reports, including:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

AI-Logs/Reports/2026-11-08-level8-implementation-recovery-report.md

AI-Logs/Reports/2026-11-08-level8-orientation-exit-spike-blocked-report.md

AI-Logs/Reports/2026-11-08-level8-orientation-exit-correction-report.md

AI-Logs/Reports/2026-11-08-level8-clockwise-control-retry-correction-report.md

Also inspect:

- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- README.md;
- the entire current tracked diff;
- all new tracked-candidate files;
- all existing Level 8 tests;
- all relevant Levels 1-7 regression tests.

Use the reports as historical evidence.

Do not rewrite existing historical reports.


FINAL MANUAL ACCEPTANCE PROVIDED BY NEAL

Neal has completed manual Level 8 play testing.

Final manual result:

- Level 8 passes.
- The ROM-derived Level 8 tube shape is accepted.
- The corrected tube orientation is accepted.
- The open topology and two endpoint presentation are accepted.
- Clockwise/counter-clockwise spinner behavior is correct.
- On Level 8, clockwise spinner rotation moves the Blaster clockwise around
  the open tube and therefore visually toward the left endpoint.
- On Level 8, counter-clockwise spinner rotation moves the Blaster
  counter-clockwise and therefore visually toward the right endpoint.
- Endpoint clamping and reversal are accepted.
- No incorrect wrap across the open gap was observed.
- Level 8 gameplay behaves as expected.
- Active-exit firing works.
- Exit player-shot movement and Spike interaction work.
- Exit Spike hazards and corrected retry behavior work.
- No remaining blocking Level 8 gameplay problem was found.

Neal also performed regression-oriented manual validation:

- An existing recording covering Levels 1-7 played correctly.
- No Levels 1-7 regression was observed in that recording.
- Playback pause works correctly.
- Playback resume works correctly.
- Forward/reverse stepping has no observed regression.
- The `--speed` diagnostic works as expected.
- No recording/playback regression was observed.

Treat this as final manual acceptance of production Level 8.

Do not require additional manual testing unless final automated validation
reveals a genuine candidate-caused regression.


ACCEPTED LEVEL 8 SCOPE

The accepted Level 8 milestone includes:

- Rev 2 ROM-derived shape row 7;
- 16 ordered tube vertices;
- open topology;
- exactly 15 playable lanes;
- no closing edge;
- correct Y-reflected display orientation;
- topology-aware clockwise/counter-clockwise spinner mapping;
- endpoint clamping;
- no wrap across the opening;
- Medium/default original supply of 24;
- six-active ordinary creation gate;
- Tanker and Spiker ordinary entry;
- no ordinary bottom-entry Flippers;
- no Pulsars;
- no Fuseballs/Fuzzballs;
- two Flipper cargo children per Tanker;
- topology-safe Tanker cargo near endpoints;
- ROM-derived `$07` versus `$19` cargo movement selection;
- Level 8 `$19` repeated move/flip program;
- starting Spikes on playable lanes only;
- the accepted project translation for Level 8 starting-Spike height;
- final-pending Spiker-to-Tanker conversion;
- Level 7-style generic Level 8 death/re-entry reconstruction;
- natural Level 7 to Level 8 progression;
- generic `--level 8` startup;
- active-exit firing;
- exit player-shot movement;
- exit shot-versus-Spike interaction;
- exit Blaster-versus-Spike collision;
- separation of active Spike hazards from the earlier AVOID SPIKES
  interstitial;
- clean terminal Level 8 boundary;
- no Level 9;
- recording/playback/headless/scrubbing/screenshot compatibility;
- `--speed` compatibility.


EVIDENCE CLASSIFICATION

Preserve the distinctions among:

RAW-ROM-DATA / REV-2-BINARY-CONFIRMED:

- shape-row selector;
- raw tube coordinates;
- open topology;
- ROM level parameters;
- supply/type limits;
- Tanker cargo type;
- `$07`/`$19` movement boundary;
- `$19` movement program;
- starting-Spike raw value;
- active exit firing/shot/Spike behavior.

ROM-LISTING-BACKED:

- decoded routines and control flow;
- active-exit lifecycle;
- AVOID SPIKES presentation distinction;
- death/re-entry and pending conversion behavior where established.

PROJECT TRANSLATION:

- normalized C# tube coordinates;
- Y reflection required to match arcade presentation;
- C# tube color/placement;
- starting Blaster lane;
- starting-Spike C# height conversion;
- C# topology abstractions;
- clockwise lane-index translation derived from tube winding.

MANUALLY ARCADE-FIDELITY ACCEPTED:

- resulting Level 8 tube and orientation;
- open-endpoint behavior;
- spinner behavior;
- playing experience;
- active-exit behavior;
- overall Level 8 production result.

Do not relabel project coordinate, color, scale, or control translations as
literal Atari hardware constants merely because the resulting presentation is
accepted.


CLOCKWISE / COUNTER-CLOCKWISE CONTROL

Preserve the accepted rotational invariant:

- physical clockwise spinner movement means clockwise Blaster travel around the
  tube;
- physical counter-clockwise spinner movement means counter-clockwise travel.

For accepted Level 8 orientation:

- clockwise travel appears visually left;
- counter-clockwise travel appears visually right.

Do not replace this with a global rule that input right must always move the
Blaster visually right.

Do not globally reverse input.

Preserve accepted Levels 1-7 controls.


EXIT AND SPIKE BEHAVIOR

Preserve the accepted separation between:

1. AVOID SPIKES presentation/interstitial policy; and
2. active Spike hazards during tube exit.

Level 8 does not show the earlier AVOID SPIKES interstitial.

During active Level 8 exit:

- Fire remains available;
- normal cooldown and shot capacity apply;
- shots move down the active exit lane;
- shots interact with that lane's Spike;
- a remaining blocking Spike can destroy the Blaster;
- exit death uses the corrected retry path;
- a successfully cleared/sufficiently shortened path allows safe completion.

Do not undo the shared exit-state correction.


SPIKE RETRY CORRECTION

Preserve the correction established in the final focused report:

- the two previously failing tests exercised the Level 1 Spiker development
  scenario, not production Level 8;
- runtime Spike-exit mode, not configuration ExitPolicy alone, determines the
  correct post-death retry state for that diagnostic scenario;
- those tests correctly remain on AvoidSpikes;
- production Level 8 remains Standard exit;
- Level 8 active-exit hazards do not enable the AvoidSpikes interstitial.


LEVELS 1-7 ARE FROZEN

Do not intentionally modify accepted Levels 1-7 behavior during this closeout.

Preserve:

- geometry;
- closed-tube wrapping;
- spinner direction;
- movement;
- enemy behavior;
- Tanker cargo;
- Spikes;
- exit behavior;
- progression;
- recording/playback;
- presentation.

Only closeout documentation or a validation-proven defect directly caused by
the Level 8 candidate may be corrected.

Do not broaden this milestone into unrelated earlier-level work.


DOCUMENTATION CLOSEOUT

Update durable project documentation appropriately:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md
- README.md only if the current Level 8 campaign/direct-startup description
  needs final acceptance wording.

Record:

- final Level 8 manual acceptance;
- accepted ROM-derived open tube;
- accepted display orientation;
- accepted clockwise/counter-clockwise spinner behavior;
- accepted endpoint behavior;
- accepted supply/species/cargo behavior;
- accepted `$07`/`$19` cargo distinction;
- accepted `$19` movement result;
- accepted Spike presentation and gameplay;
- accepted active-exit firing/Spike behavior;
- accepted natural Level 7-to-8 progression if already manually covered;
- Level 8 terminal boundary;
- Levels 1-7 recorded-playback regression pass;
- pause/resume/stepping regression pass;
- `--speed` pass;
- no further Level 8 tuning currently required.

If any specific Level 8 manual scenario was not actually tested, do not invent
a result. Record only the acceptance Neal provided and the automated evidence.

Preserve evidence classifications.


FINAL VALIDATION

Run final validation against the complete accepted Level 8 candidate.

At minimum:

1. Focused Level 8 production tests.

2. Focused geometry/open-topology/spinner tests.

3. Focused active-exit/firing/Spike/retry tests.

4. Focused Levels 1-7 regression tests covering affected shared systems.

5. Expanded regression coverage including:
   - Level configurations;
   - Level loader;
   - progression;
   - player movement;
   - geometry and rendering;
   - Flippers;
   - Tankers and cargo;
   - Spikers and Spikes;
   - enemy selection;
   - wave ledger/accounting;
   - death/re-entry;
   - completion;
   - Super Zapper;
   - direct-level startup;
   - campaign soak;
   - gameplay recording;
   - visible playback;
   - headless playback;
   - pause/resume;
   - forward/reverse stepping;
   - screenshot capture;
   - `--speed`.

6. Full solution test suite:

dotnet test TempestCSharp.slnx

7. Full solution build:

dotnet build TempestCSharp.slnx

8. Whitespace validation:

git diff --check

9. After staging:

git diff --cached --check

Do not launch the interactive game.

Neal has already completed manual acceptance.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated .NET
assemblies during this milestone.

Do not weaken Windows security.

Do not repeatedly rebuild without purpose if a block occurs.

If App Control blocks a final validation command:

- record the exact command and error;
- determine whether an already-built `--no-build` validation can legitimately
  complete the required gate;
- retry only when useful;
- do not claim a blocked test run passed;
- do not commit unless final validation is genuinely complete.

The most recently completed focused report recorded:

- focused tests: 149 passed;
- full solution: 1,062 passed;
- build: 0 warnings and 0 errors;
- git diff --check: passed.

These are implementation-stage results, not a substitute for the requested
final gate if the candidate changes during closeout.


DIFF REVIEW

This milestone accumulated changes across implementation, recovery, orientation,
exit, and spinner corrections.

Inspect the entire final tracked diff carefully.

Verify that every changed tracked file belongs to the accepted Level 8
milestone or its required shared topology/exit corrections.

Identify and exclude any unrelated change.

Do not blindly stage based on a historical file list.


NEW FILES

Verify intended new tracked candidate files, including as applicable:

- src/Tempest.Game/Levels/Level8WaveSelection.cs
- tests/Tempest.Game.Tests/Level8ProductionTests.cs

Confirm their final content and necessity before staging.


ROM FILE SAFETY

The local Rev 2 ROM directory is:

Reference/arcade/roms.rev2/

It is reference material and must remain untracked/excluded.

Verify:

- all expected ROM files remain unchanged;
- filenames and sizes remain unchanged;
- hashes remain unchanged if existing reports provide the baseline;
- no ROM file is staged;
- no ROM file is committed;
- no generated output is written into that directory.

Do not modify `.gitignore` merely to hide an unexpected ROM staging error
without understanding the existing project policy.

If the accepted project policy is to keep the ROM directory local and
untracked, preserve that state.


SCREENSHOT AND USERDATA SAFETY

Do not stage or commit:

- UserData/
- UserData/Screenshots/Level8-orientation.png
- gameplay recordings;
- Videos;
- root screenshots;
- temporary output;
- generated build output;
- AI-Logs/.

Verify the orientation screenshot remains unchanged.


AI-LOG POLICY

These instructions are explicit and non-conflicting:

1. Save this complete substantive closeout prompt under:

   AI-Logs/Prompts/

2. Create a NEW closeout report under:

   AI-Logs/Reports/

3. Creating the new ignored prompt/report files is authorized and expected.

4. Existing investigation, implementation, recovery, blocked, and correction
   reports are historical records and must remain unchanged.

5. Do not rewrite prior reports.

6. AI-Logs/ is Git-ignored.

7. Do not stage or commit any file under AI-Logs/.

8. Do not place the closeout report under TEMP/, Temp/, or another working
   directory.

9. TEMP/ and Temp/ are not the normal destination for milestone reports.


STAGING SAFETY

After final validation:

1. Review `git status --short`.
2. Review the complete diff.
3. Derive the intended tracked-file allowlist from the actual accepted
   candidate.
4. Stage only intended Level 8 milestone files.
5. Print the staged file list.
6. Compare the staged list with the reviewed allowlist.
7. Stop if there is any mismatch.
8. Run:

git diff --cached --check

9. Inspect the staged diff/stat before committing.

Do not use:

git add .
git add -A

Do not stage the Rev 2 ROM directory, UserData, recordings, screenshots,
AI-Logs, TEMP, or unrelated files.


COMMIT AND PUSH

If and only if final validation succeeds:

1. Commit the accepted complete Level 8 milestone with a clear descriptive
   message.

Suggested subject:

Implement accepted ROM-derived production Level 8

2. Include the normal Copilot co-author trailer if that remains established
   project policy.

3. Push `master` to the configured GitHub origin.

4. Verify after push:
   - local HEAD;
   - local `origin/master`;
   - live remote `master`;
   - all three references match;
   - tracked worktree is clean;
   - index is clean;
   - only intentional local/untracked material remains;
   - no protected local files were committed.


CLOSEOUT REPORT

Create a new descriptive closeout report under:

AI-Logs/Reports/

The report must record:

1. Starting Git state.
2. Committed baseline.
3. Full accumulated Level 8 candidate context.
4. Neal's final manual acceptance.
5. Manual Levels 1-7 recorded-playback regression result.
6. Pause/resume/stepping result.
7. `--speed` result.
8. Final durable documentation changes.
9. Final tracked changed-file list.
10. Final focused validation commands/results.
11. Final expanded validation commands/results.
12. Full-suite result.
13. Build result.
14. Unstaged diff-check result.
15. Staged diff-check result.
16. Final staged allowlist.
17. Commit hash.
18. Commit subject/body.
19. Push result.
20. Local HEAD.
21. `origin/master`.
22. Live remote `master`.
23. Final Git status.
24. ROM files deliberately excluded.
25. UserData/screenshot/recordings deliberately excluded.
26. AI-Logs deliberately excluded.
27. Any genuinely unresolved Level 8 issue.
28. Confirmation that Level 8 is manually accepted and closed.

If no issue remains in the accepted Level 8 scope, state that explicitly.


STOP POINT

After successful documentation, final validation, staging, commit, push, and
repository verification:

STOP.

Do not begin:

- Level 9 analysis;
- Level 9 geometry;
- Level 9 implementation;
- broader difficulty work;
- global timing changes;
- unrelated cleanup;
- Windows security policy changes.


FINAL RESPONSE TO NEAL

Provide a concise final summary containing:

- commit hash and message;
- focused validation result;
- expanded validation result;
- full-suite result;
- build result;
- diff-check results;
- push status;
- final Git status;
- confirmation that ROM/UserData/screenshots/recordings/AI-Logs were excluded;
- confirmation that production Level 8 is manually accepted and closed;
- confirmation that Levels 1-8 now form the accepted production campaign
  foundation.

TEMPEST_LEVEL8_ACCEPTED_PRODUCTION_CLOSEOUT_20261108