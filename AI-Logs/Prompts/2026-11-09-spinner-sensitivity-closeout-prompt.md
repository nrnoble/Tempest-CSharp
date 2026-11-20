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
project or repository action and explain why.


TASK

Close out the manually accepted --spinner-sensitivity diagnostic/manual-
calibration milestone.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected committed baseline beneath the current unstaged candidate:

d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef
Close out accepted production Level 8

Confirm the actual branch, HEAD, tracking state, index, worktree, and untracked
state before modifying anything.

Relevant report:

AI-Logs/Reports/2026-11-08-spinner-sensitivity-implementation-report.md

Read the implementation report and current candidate before closeout.

Do not begin Level 9.


MANUAL ACCEPTANCE PROVIDED BY NEAL

Neal manually tested the --spinner-sensitivity implementation with his physical
arcade-style USB spinner.

Final manual acceptance:

- The feature works correctly.
- All manual tests passed.
- Reducing sensitivity to approximately 0.8 was an improvement in physical
  spinner control.
- Neal nevertheless wants to KEEP THE DEFAULT AT 1.0 FOR NOW.
- Do not change the default to 0.8.
- 0.8 is a useful manually observed alternative, not the accepted normal
  default.
- The command-line option is useful as a diagnostic/manual-calibration tool.
- Clockwise/counter-clockwise behavior remains correct.
- Existing gameplay remains correct.
- No blocking regression was found.

Treat the feature as manually accepted.

Do not infer that Neal has selected 0.8 as a permanent or arcade-verified
sensitivity value.


ACCEPTED FEATURE

Preserve:

--spinner-sensitivity <multiplier>

Default:

1.0

Supported inclusive range:

0.1 through 4.0

The multiplier scales only live horizontal Raylib mouse/spinner movement before
effective PlayerInput is sent to simulation and recording.

Preserve:

- horizontal X-axis scaling only;
- fractional floating-point input;
- no clamp;
- no smoothing;
- no dead zone;
- no acceleration curve;
- no nonlinear filtering;
- no device-specific handling;
- existing TempestGame.SpinnerSensitivity = 0.0125f;
- existing clockwise/counter-clockwise mapping;
- Level 8 winding-aware input mapping;
- keyboard behavior;
- controller behavior;
- simulation timing.

Do not modify Windows settings or USB-device configuration.


DEFAULT DECISION

This is explicit:

DEFAULT REMAINS 1.0.

Record Neal's manual observation that:

0.8 improved spinner control

but classify that only as:

MANUALLY OBSERVED CALIBRATION ALTERNATIVE

Do not classify 0.8 as:

- arcade verified;
- ROM backed;
- accepted default;
- required hardware setting.

The option exists specifically because PC spinner/mouse devices can differ in
response.


RECORDING / PLAYBACK

Preserve the implemented boundary:

- live horizontal input is scaled first;
- effective scaled PlayerInput is used by simulation;
- the same effective input is recorded;
- the recording schema does not contain spinner sensitivity;
- playback does not rescale recorded input;
- historical recordings remain compatible;
- visible playback rejects this live-only option according to the implemented
  CLI policy;
- headless playback rejects it according to the implemented policy;
- reverse reconstruction remains independent of the live sensitivity setting.

Do not modify recording format.


`--SPEED`

Preserve independence between:

--speed

and:

--spinner-sensitivity

Do not multiply the two values together.

Do not change accepted --speed behavior.


DOCUMENTATION CLOSEOUT

Update durable documentation appropriately:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Record:

- final manual acceptance;
- default remains 1.0;
- 0.8 was manually tested and was an improvement for Neal's physical spinner;
- 0.8 remains optional rather than the new default;
- feature remains development/diagnostic/manual calibration;
- horizontal mouse/spinner input only;
- keyboard/controller unaffected;
- recordings contain effective scaled input;
- playback is not rescaled;
- --speed remains independent;
- no Windows settings need to be changed by Tempest;
- no further tuning is required in this milestone.

Do not claim a universal best spinner value.

Do not claim the option reproduces a literal Atari spinner-count conversion.


FINAL VALIDATION

Run final validation against the complete accepted candidate.

At minimum:

1. Focused spinner parser/scaling tests.

2. Focused live-input/recording tests.

3. Focused playback/headless incompatibility tests.

4. Focused movement/direction/topology tests.

5. Expanded regression including:
   - command-line parsing;
   - InputSystem;
   - player movement;
   - Levels 1-8;
   - closed-tube wrapping;
   - Level 8 open endpoints;
   - clockwise/counter-clockwise semantics;
   - firing;
   - Super Zapper;
   - exits;
   - death/re-entry;
   - progression;
   - recording;
   - visible playback;
   - headless playback;
   - pause/resume;
   - forward/reverse stepping;
   - screenshots;
   - --speed.

6. Full solution:

dotnet test TempestCSharp.slnx

7. Full build:

dotnet build TempestCSharp.slnx

8. Run:

git diff --check

9. After staging:

git diff --cached --check

Do not launch the interactive game.

Neal has completed manual acceptance.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated assemblies.

Do not weaken Windows security.

If final validation is blocked:

- record the exact command/error;
- distinguish a security block from an actual test failure;
- use --no-build only where it legitimately validates the current candidate;
- do not claim blocked validation passed;
- do not commit unless the final gate is genuinely complete.


DIFF REVIEW

Inspect the complete current candidate.

Expected implementation files include:

src/Tempest.Game/Debug/SpinnerSensitivityOptions.cs
src/Tempest.Game/Debug/DebugController.cs
src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs
src/Tempest.Game/Program.cs
src/Tempest.Game/Systems/InputSystem.cs
tests/Tempest.Game.Tests/SpinnerSensitivityOptionsTests.cs
tests/Tempest.Game.Tests/GameplayRecordingTests.cs
tests/Tempest.Game.Tests/HeadlessGameplayPlaybackTests.cs
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Verify the actual final diff rather than blindly trusting this historical list.

Exclude any unrelated change.


PROTECTED LOCAL FILES

Do not stage or commit:

Reference/arcade/roms.rev2/
UserData/
screenshots
gameplay recordings
Videos/
AI-Logs/
TEMP/
Temp/
build outputs
unrelated local files

Do not run git clean.

Verify the 21 local Rev 2 ROM files remain untouched and untracked.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete closeout prompt under:

AI-Logs/Prompts/

2. Create a NEW closeout report under:

AI-Logs/Reports/

3. Creating these ignored records is expected and authorized.

4. Existing investigation and implementation reports remain historical records.

5. Do not rewrite existing reports.

6. AI-Logs is Git-ignored.

7. Do not stage or commit AI-Logs.

8. Do not place the closeout report under TEMP/ or Temp/.

TEMP/ and Temp/ are not normal milestone-report destinations.


STAGING SAFETY

After validation:

1. Inspect git status.
2. Inspect the complete tracked diff.
3. Derive the intended tracked-file allowlist.
4. Stage only intended milestone files.
5. Print the staged file list.
6. Compare it to the reviewed allowlist.
7. Stop on any mismatch.
8. Run:

git diff --cached --check

9. Review the staged diff/stat.

Do not use:

git add .
git add -A


COMMIT AND PUSH

If and only if validation succeeds:

1. Commit the accepted spinner-sensitivity feature.

Suggested subject:

Close out accepted spinner sensitivity diagnostic

2. Include the normal Copilot co-author trailer if that remains established
   project policy.

3. Push master to the configured GitHub origin.

4. Verify:

- local HEAD;
- origin/master;
- live remote master;
- all three match;
- tracked worktree clean;
- index clean;
- protected local material excluded.


PUBLIC GITHUB VERIFICATION

The repository is publicly available at:

https://github.com/nrnoble/Tempest-CSharp

After pushing:

1. Verify the live remote master using Git.
2. Record the final commit hash.
3. Provide Neal the exact commit hash.

Do not spend substantial time waiting for GitHub's web UI or external caches to
refresh.

A successful Git push plus matching live remote ref is sufficient repository
proof.

Neal will separately ask Microsoft 365 Copilot to verify whether the new public
commit is externally readable.


CLOSEOUT REPORT

Create a new report under:

AI-Logs/Reports/

Record:

1. Starting Git state.
2. Accepted baseline.
3. Neal's manual acceptance.
4. 0.8 observation.
5. Explicit decision to keep default 1.0.
6. Documentation changes.
7. Final changed-file list.
8. Focused validation.
9. Expanded validation.
10. Full-suite result.
11. Build result.
12. Unstaged diff check.
13. Staged diff check.
14. Staged allowlist.
15. Commit hash.
16. Commit message.
17. Push result.
18. Local HEAD.
19. origin/master.
20. Live remote master.
21. Final Git status.
22. ROM/UserData/recording/Video exclusions.
23. AI-Logs exclusion.
24. Any genuine unresolved limitation.
25. Confirmation that the spinner-sensitivity milestone is accepted and closed.


STOP POINT

After documentation, final validation, commit, push, and repository
verification:

STOP.

Do not begin:

- the low-level difficulty comparison investigation;
- Level 9;
- another spinner model;
- input clamping;
- raw-input architecture;
- Windows-setting changes;
- unrelated cleanup.


FINAL RESPONSE TO NEAL

Provide a concise summary including:

- final commit hash and subject;
- focused validation;
- expanded validation;
- full-suite result;
- build result;
- diff-check results;
- push status;
- final Git status;
- confirmation default remains 1.0;
- confirmation 0.8 remains an optional manually useful value;
- confirmation protected local material was excluded;
- confirmation milestone is closed.

TEMPEST_SPINNER_SENSITIVITY_ACCEPTED_CLOSEOUT_20261108