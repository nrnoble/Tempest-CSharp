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

Implement a tightly bounded live-spinner/mouse sensitivity command-line option:

--spinner-sensitivity <multiplier>

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef
Close out accepted production Level 8

Confirm the actual branch, HEAD, remote tracking state, index, worktree, and
untracked state before modifying anything.

Do not stage, commit, or push.

Leave the completed implementation candidate unstaged for Neal's manual
calibration and acceptance.

Do not begin Level 9.


PRIMARY INVESTIGATION REPORT

Read this report fully before designing or modifying code:

AI-Logs/Reports/2026-11-08-spinner-sensitivity-investigation-report.md

Reuse its established findings.

Do not repeat the investigation unless a specific implementation detail
requires verification.


USER'S PHYSICAL DEVICE

Neal uses a real arcade-style USB spinner mounted in a PC controller deck.

In Windows, this spinner behaves as a horizontal mouse device:

- clockwise and counter-clockwise spinner movement move the Windows mouse
  cursor horizontally;
- the spinner moves the cursor left and right;
- the spinner does not intentionally produce vertical mouse movement;
- the spinner does not use the mouse wheel for Tempest movement;
- the game receives the spinner through the existing horizontal mouse-delta
  path.

The implementation must therefore scale only the horizontal live mouse/spinner
delta:

Raylib.GetMouseDelta().X

Do not use or scale:

- mouse Y movement;
- mouse wheel movement;
- mouse buttons;
- keyboard movement;
- controller movement;
- recorded playback input after acquisition;
- any unrelated Windows cursor setting.

Do not add device identification or USB-specific code.

The same option should also apply when an ordinary mouse is used horizontally
as the game's spinner control.


USER PROBLEM

Current spinner response is too sensitive.

A small physical movement of Neal's spinner can move the Blaster across multiple
lanes too quickly.

Neal wants to tune this inside Tempest rather than changing Windows mouse
settings merely to play the game.

The future diagnostic/calibration option should let Neal reduce or increase
live horizontal spinner/mouse response while preserving all existing gameplay
and control-direction semantics.


ACCEPTED CURRENT INPUT PIPELINE

The investigation established the current live path:

Raylib.GetMouseDelta().X
    -> PlayerInput.MouseDeltaX
    -> TempestGame.UpdatePlayerMovement
    -> existing SpinnerSensitivity constant 0.0125f
    -> winding-aware clockwise/counter-clockwise sign
    -> Player.MoveAlongTube
    -> geometry-derived rim movement

The current gameplay scalar is:

TempestGame.SpinnerSensitivity = 0.0125f

Do not change that accepted gameplay constant in this milestone.

The new multiplier belongs at the live input-acquisition boundary before the
effective PlayerInput is passed to both:

- simulation;
- recording.

The intended new live path is:

Raylib.GetMouseDelta().X
    -> --spinner-sensitivity multiplier
    -> effective PlayerInput.MouseDeltaX
    -> existing TempestGame SpinnerSensitivity 0.0125f
    -> existing clockwise/counter-clockwise mapping
    -> existing Player.MoveAlongTube behavior


COMMAND-LINE OPTION

Implement:

--spinner-sensitivity <multiplier>

Examples:

--spinner-sensitivity 1

Current accepted response.

--spinner-sensitivity 0.75

Seventy-five percent of the current live horizontal spinner/mouse response.

--spinner-sensitivity 0.5

Half the current live horizontal spinner/mouse response.

--spinner-sensitivity 0.3

Thirty percent of the current live horizontal spinner/mouse response.

--spinner-sensitivity 1.25

Twenty-five percent more live horizontal spinner/mouse response.


DEFAULT

The default must remain:

1.0

No option and explicit:

--spinner-sensitivity 1

must produce exactly equivalent live-input behavior.

Do not change the project's normal default sensitivity yet.

This feature is initially a development/manual-calibration tool.

Neal will manually compare values before deciding whether another value should
become the normal default.


SUPPORTED RANGE

Use this inclusive range:

0.1 through 4.0

Accept positive finite invariant-culture decimal values within that range.

Examples that should be accepted:

0.1
0.25
0.3
0.4
0.5
0.75
1
1.0
1.25
2
4
4.0


INVALID INPUT

Reject clearly:

- a missing value;
- malformed text;
- zero;
- negative values;
- NaN;
- positive infinity;
- negative infinity;
- values below 0.1;
- values above 4.0;
- duplicate --spinner-sensitivity options according to the existing strict
  command-line policy.

Do not silently clamp invalid input.

Use invariant-culture parsing.

Return a clear command-line diagnostic.


OPTION NAME

Use exactly:

--spinner-sensitivity

Do not add aliases such as:

--spinner
--mouse-sensitivity
--spinner-scale

The selected name clearly communicates the purpose while documenting that
ordinary horizontal mouse movement is also included because it is the game's
spinner input.


LIVE HORIZONTAL INPUT ONLY

Apply the multiplier only to live horizontal mouse/spinner acquisition.

Conceptually:

effectiveMouseDeltaX =
    Raylib.GetMouseDelta().X * spinnerSensitivityMultiplier

Then construct PlayerInput using that effective horizontal value.

Do not apply the multiplier inside TempestGame.UpdatePlayerMovement.

Do not apply it inside Player.MoveAlongTube.

Do not apply it after recording.

Do not apply it during playback.

Do not scale MouseDeltaY because MouseDeltaY is not part of Blaster movement.

Do not add MouseDeltaY to PlayerInput merely for symmetry.

Do not use the mouse wheel.


FRACTIONAL INPUT

Preserve floating-point values.

Do not cast the scaled horizontal delta to an integer.

Examples:

raw 1.0 at sensitivity 0.5 -> effective 0.5

raw 1.0 at sensitivity 0.3 -> effective 0.3

raw 2.5 at sensitivity 0.4 -> effective 1.0

The existing pipeline uses floats and Player.EdgePosition preserves fractional
sub-lane movement.

Do not add a separate fractional accumulator unless implementation evidence
shows the existing float path is insufficient.

Do not add smoothing or time-dependent state.


NO MOVEMENT CLAMP

Do not add a per-tick spinner/mouse movement clamp in this milestone.

The investigation established that Player.MoveAlongTube deliberately consumes
the complete movement amount and may cross several lanes for a sufficiently
large input sample.

This feature changes magnitude proportionally.

It does not guarantee that every possible input sample is limited to one lane.

Neal will first determine whether sensitivity scaling alone solves the control
problem.

A future movement clamp, acceleration curve, or alternate Incitti-style control
model would be a separate milestone requiring separate evidence and acceptance.


NO DEAD ZONE OR FILTERING

Do not add:

- a mouse dead zone;
- smoothing;
- averaging;
- acceleration;
- deceleration;
- input filtering;
- sample queues;
- nonlinear response curves;
- momentum;
- overshoot dropping;
- forced one-lane movement;
- device-specific calibration.

This milestone is a simple linear multiplier applied to live horizontal
spinner/mouse input.


CLOCKWISE / COUNTER-CLOCKWISE INVARIANT

Sensitivity changes magnitude only.

It must not alter rotational direction.

Preserve:

- physical clockwise spinner movement produces clockwise Blaster movement;
- physical counter-clockwise spinner movement produces counter-clockwise
  Blaster movement.

On closed Levels 1-7:

- preserve accepted continuous rim traversal and wrapping.

On open Level 8:

- clockwise physical spinner movement follows the clockwise rim path and appears
  visually toward the left endpoint;
- counter-clockwise movement follows the counter-clockwise path and appears
  visually toward the right endpoint;
- endpoint clamping remains correct;
- reversing direction from an endpoint moves inward;
- no wrap occurs across the opening.

Do not globally reverse input.

Do not alter Level.ClockwiseLaneIndexDelta or tube winding behavior.


KEYBOARD

Do not apply spinner sensitivity to keyboard input.

Preserve the current keyboard movement amount and semantics exactly.

Keyboard Left and Right remain screen/lane-index controls as currently accepted.

Do not reinterpret keyboard controls as physical spinner rotation.


CONTROLLER

Do not apply spinner sensitivity to controller input.

Preserve:

- controller-axis scaling;
- controller dead zone;
- controller direction;
- controller quit behavior;
- all accepted controller tests.

Do not add a controller sensitivity option.


WINDOWS SETTINGS

Do not change or attempt to override:

- Windows pointer speed;
- Enhance Pointer Precision;
- registry settings;
- USB spinner configuration;
- device-driver settings;
- cursor acceleration.

The exact relationship among the spinner hardware, Windows pointer processing,
and Raylib remains unknown and requires measurement.

This feature operates only on the horizontal float delta supplied to the
project by Raylib.


RECORDING BOUNDARY

Apply the multiplier before the effective PlayerInput is sent to:

1. TempestGame.RunOneStep;
2. PlayerInputRecorder.

The exact same PlayerInput instance/value for a live tick should be used by the
simulation and recording.

Example:

Raw live Raylib X delta:

10

Selected sensitivity:

0.5

Effective PlayerInput.MouseDeltaX:

5

The simulation receives 5.

The recording stores 5.

Do not store raw 10 and then rely on replay-time sensitivity.


RECORDING FORMAT

Do not add a spinner-sensitivity field to the recording format.

Do not change:

- recording schema;
- recording version;
- startup identity;
- level;
- lives;
- rim-speed metadata;
- seed;
- playback boundaries.

New recordings made with reduced sensitivity should contain the effective
scaled per-tick MouseDeltaX values.

Historical recordings remain compatible.

Do not rewrite existing recordings.


VISIBLE PLAYBACK

Visible playback must not apply live spinner sensitivity to recorded frames.

Recorded PlayerInput.MouseDeltaX is already effective input.

Do not scale it again.

The current live machine's spinner-sensitivity setting must not alter visible
playback state or trajectory.

Use the existing strict CLI policy for inapplicable option combinations.

Recommended policy:

Reject --spinner-sensitivity when used with --playback-file.

Provide a clear diagnostic explaining that spinner sensitivity affects live
mouse/spinner acquisition only and recorded playback already contains effective
input.

Do not silently accept a no-op.


HEADLESS PLAYBACK

Do not apply sensitivity to headless playback.

Headless playback contains no live physical spinner acquisition.

Recommended policy:

Reject --spinner-sensitivity for headless playback with a clear diagnostic,
consistent with the strict current headless CLI.

Do not throttle or otherwise change headless playback.


LIVE RECORDING

Allow:

--record-gameplay <path> --spinner-sensitivity <value>

The live input should be scaled first and then recorded.

Playback of that file should reproduce the effective movement without requiring
the original sensitivity option.


INPUT DIAGNOSTIC MODE

Investigate the existing interaction with input-diagnostic mode during
implementation.

The investigation recommended not silently claiming sensitivity applies when
the current diagnostic does not perform gameplay movement or report mouse
delta.

Prefer one of these narrow policies:

- reject --spinner-sensitivity with input-diagnostic mode as inapplicable; or
- if the implementation makes the diagnostic explicitly report raw and scaled
  horizontal delta, document and test that narrowly.

Do not broaden this milestone into a full input telemetry system.

Prefer rejection unless a minimal existing runtime-options architecture makes
diagnostic reporting clearly beneficial.


INTERACTION WITH `--SPEED`

Spinner sensitivity and speed are independent multipliers.

Do not compute:

spinner sensitivity * speed multiplier

The accepted meanings remain:

--speed

changes host-loop/simulation pacing.

--spinner-sensitivity

scales each live horizontal Raylib mouse-delta sample.

Allow live gameplay combinations such as:

--speed 0.5 --spinner-sensitivity 0.5

--speed 2 --spinner-sensitivity 0.5

Do not change either option's meaning.

Do not serialize either option into recording metadata unless the existing
accepted design already does so.

The recording should store effective per-tick input.


STARTUP FEEDBACK

When --spinner-sensitivity is explicitly supplied, print one concise startup
console line showing:

- selected multiplier;
- that it scales live horizontal spinner/mouse response relative to current
  behavior.

Do not print additional noise when the option is omitted unless existing
command-line options follow a consistent always-print policy.

Do not add an in-game HUD message.

Do not add a settings menu.


RUNTIME OPTIONS STRUCTURE

Keep the implementation maintainable.

Prefer a small validated options value/property associated with existing
command-line/runtime parsing.

Do not scatter raw string parsing or multiplier checks through Program.cs.

Define:

- default multiplier;
- minimum;
- maximum;
- invariant parsing;
- validation;
- option-presence state if needed for incompatibility checks.

Use maintainability-focused comments only where the input/recording boundary is
not self-evident.

Do not over-comment obvious code.


EXPECTED CODE SURFACE

The investigation recommends limiting changes primarily to:

- DebugController.cs or the existing command-line parser;
- Program.cs;
- InputSystem.cs;
- focused parser/input/recording/playback tests;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md.

Modify additional files only when an actual architecture dependency requires
it.

Do not change:

- TempestGame.SpinnerSensitivity;
- Player.MoveAlongTube;
- Level.ClockwiseLaneIndexDelta;
- geometry;
- enemy behavior;
- rendering;
- simulation timing;
- audio;
- recording schema.


REQUIRED AUTOMATED TESTS

Add focused tests proving all of the following.


1. DEFAULT

- no --spinner-sensitivity option => 1.0;
- live behavior is unchanged;
- existing default input samples remain unchanged before the existing gameplay
  scalar.


2. EXPLICIT NORMAL

- --spinner-sensitivity 1 => 1.0;
- explicit 1.0 equals no-option behavior.


3. FRACTIONAL VALUES

- 0.75 scales a live horizontal delta to 75%;
- 0.5 scales a live horizontal delta to 50%;
- 0.4 scales correctly;
- 0.3 scales correctly;
- small values remain fractional and are not truncated.


4. INCREASED VALUES

- 1.25 increases live response by 25%;
- 2.0 doubles live response;
- 4.0 is accepted as the maximum.


5. RANGE

- 0.1 is accepted;
- 4.0 is accepted;
- below 0.1 is rejected;
- above 4.0 is rejected.


6. INVALID INPUT

Reject:

- missing value;
- malformed value;
- zero;
- negative value;
- NaN;
- positive infinity;
- negative infinity;
- duplicates according to current CLI policy.

Do not silently clamp.


7. HORIZONTAL-ONLY BEHAVIOR

- only MouseDeltaX is scaled;
- vertical mouse movement is not introduced;
- mouse wheel is not introduced;
- mouse buttons remain unchanged;
- Fire and Super Zapper remain unchanged.


8. DIRECTION

- sensitivity does not change the sign of MouseDeltaX;
- clockwise/counter-clockwise semantics remain unchanged;
- Levels 1-7 retain accepted closed-tube direction/wrapping;
- Level 8 retains winding-aware direction and endpoint clamping.


9. KEYBOARD

- keyboard movement is unchanged at every spinner-sensitivity value.


10. CONTROLLER

- controller movement is unchanged;
- controller dead zone is unchanged;
- controller direction is unchanged.


11. FRACTIONAL MOVEMENT

- a raw delta of 1 at sensitivity 0.5 produces effective 0.5;
- resulting sub-lane movement is retained;
- repeated fractional samples accumulate through existing player position;
- no integer truncation occurs.


12. MULTI-LANE MOVEMENT

- sensitivity scales the movement amount proportionally;
- sufficiently large input may still cross multiple lanes;
- no new clamp exists;
- endpoint clamping remains correct on Level 8.


13. LIVE RECORDING

- the effective scaled PlayerInput is passed to simulation;
- the identical effective value is recorded;
- the original raw live value is not stored when a multiplier changes it;
- recording schema does not gain a sensitivity field.


14. HISTORICAL RECORDINGS

- existing recording files remain readable;
- existing recordings at default semantics replay unchanged.


15. VISIBLE PLAYBACK

- recorded MouseDeltaX is not rescaled;
- visible playback state is independent of current live sensitivity;
- --spinner-sensitivity with --playback-file follows the chosen strict
  inapplicable-option policy.


16. REVERSE RECONSTRUCTION

- pause/forward/reverse behavior remains exact;
- reconstructed state does not depend on current spinner sensitivity.


17. HEADLESS

- headless playback remains deterministic and unthrottled;
- the sensitivity option is rejected clearly as inapplicable.


18. `--SPEED`

- speed and spinner sensitivity parse independently;
- live combinations are allowed;
- neither multiplier modifies the other;
- recording stores effective input under the selected live combination.


19. GAMEPLAY REGRESSION

- accepted Levels 1-8 movement and topology tests pass;
- player firing, Super Zapper, exits, death/re-entry, progression, recording,
  playback, screenshots, and campaign behavior remain unchanged.


TESTING CAUTION

Do not create flaky physical-device or wall-clock tests.

Do not attempt automated tests that depend on Neal's USB spinner.

Test the deterministic scaling boundary by injecting or directly exercising
known horizontal delta samples.

Do not use Thread.Sleep to test sensitivity.

Do not infer Windows pointer behavior in tests.


VALIDATION

During implementation:

1. Run narrow parser/sensitivity scaling tests first.

2. Run focused input/player movement tests.

3. Run focused recording/playback tests.

4. Run Level 8 winding/endpoint tests.

5. Run expanded regression covering:
   - command-line parsing;
   - InputSystem;
   - PlayerInput;
   - player movement;
   - closed Levels 1-7;
   - open Level 8;
   - recording;
   - visible playback;
   - headless playback;
   - pause/resume;
   - forward/reverse stepping;
   - screenshots;
   - `--speed`;
   - firing;
   - exits;
   - campaign progression.

6. Run the full solution test suite:

dotnet test TempestCSharp.slnx

7. Run the full solution build:

dotnet build TempestCSharp.slnx

8. Run:

git diff --check

Do not launch the interactive game.

Neal will perform physical-spinner calibration manually.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated assemblies.

Do not weaken Windows security.

If a test/build run is blocked:

- record the exact command and error;
- distinguish a blocked run from a test failure;
- retry only when useful;
- use `--no-build` only when it legitimately tests the current built candidate;
- do not claim blocked tests passed.

Do not change security settings in this task.


DOCUMENTATION

Update appropriately:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Document:

- `--spinner-sensitivity <multiplier>`;
- default 1.0;
- supported range 0.1 through 4.0;
- live horizontal spinner/mouse input only;
- the physical spinner appears as horizontal mouse movement;
- ordinary horizontal mouse movement is also affected;
- vertical movement is irrelevant;
- keyboard/controller input is unchanged;
- clockwise/counter-clockwise direction is unchanged;
- no clamp/smoothing/dead zone is added;
- recording stores effective scaled input;
- playback is not rescaled;
- inapplicable playback/headless policy;
- independence from `--speed`;
- Windows settings are not modified;
- manual calibration is pending;
- default remains 1.0 until accepted otherwise.

Classify the option as:

DEVELOPMENT / DIAGNOSTIC / MANUAL-CALIBRATION PROJECT FEATURE

It is not:

- Atari ROM behavior;
- an arcade difficulty setting;
- a change to tube geometry;
- an automatic hardware-calibration system.


ROM / REFERENCE SAFETY

Do not modify, stage, or commit:

Reference/arcade/roms.rev2/

Do not modify other arcade or Incitti references.

No reference update is required.


LOCAL FILE SAFETY

Do not modify, stage, move, or delete:

- UserData/;
- screenshots;
- gameplay recordings;
- Videos;
- AI-Logs historical reports;
- unrelated local files.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting:

1. Save this complete substantive implementation prompt under:

AI-Logs/Prompts/

2. Create a NEW implementation report under:

AI-Logs/Reports/

3. Creating these ignored files is authorized and expected.

4. Existing investigation and historical reports remain unchanged.

5. Do not rewrite the spinner investigation report.

6. AI-Logs/ is Git-ignored.

7. Do not stage or commit AI-Logs.

8. Do not put milestone reports under TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Investigation report reused.
4. Physical spinner horizontal-mouse clarification.
5. Option name and parsing.
6. Default/range/validation.
7. Live horizontal-only scaling boundary.
8. Fractional behavior.
9. Direction preservation.
10. Keyboard/controller preservation.
11. Recording boundary.
12. Visible playback behavior.
13. Headless policy.
14. `--speed` interaction.
15. Startup feedback.
16. Files changed.
17. Tests added/updated.
18. Focused validation.
19. Expanded validation.
20. Full-suite result.
21. Build result.
22. `git diff --check`.
23. Levels 1-8 regression evidence.
24. Recording/playback regression evidence.
25. Protected local/reference files unchanged.
26. Final Git state.
27. Manual commands.
28. Manual calibration checklist.
29. Known limitations.
30. Manual acceptance status.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the completed candidate unstaged for Neal's physical-spinner manual tests.

Do not begin Level 9.


MANUAL CALIBRATION COMMANDS

Provide Neal commands for at least:


Current/default response:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1


75% response:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.75


Half response:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.5


40% response:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.4


30% response:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 0.3


Open Level 8 at half response:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --spinner-sensitivity 0.5


Half response at slower game speed:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 0.5 --spinner-sensitivity 0.5


Half response at faster game speed:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 2 --spinner-sensitivity 0.5


Fresh recording with half response:

dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\spinner-half.json --level 1 --lives 10 --spinner-sensitivity 0.5


Playback without sensitivity option:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\spinner-half.json


MANUAL CALIBRATION CHECKLIST

Ask Neal to test with the actual physical spinner without changing Windows
settings.

For each candidate value:

1. Make very small physical movements.
2. Check whether one-lane corrections are precise.
3. Check whether small movement still jumps multiple lanes.
4. Try stopping deliberately on a target lane.
5. Try slow movement across several lanes.
6. Try a rapid emergency spin.
7. Confirm rapid movement is not made impractically slow.
8. Confirm clockwise/counter-clockwise direction remains correct.
9. Test closed Level 1 traversal and wrap.
10. Test open Level 8 endpoints and reversal.
11. Test actual combat and target tracking.
12. Compare normal speed with `--speed 0.5` and `--speed 2`.
13. Make a fresh recording with the preferred candidate value.
14. Replay it without supplying spinner sensitivity.
15. Verify playback reproduces the recorded movement.
16. Verify pause/resume/forward/reverse remain correct.
17. Record the preferred multiplier and why it was chosen.

Recommended test order:

1.0
0.75
0.5
0.4
0.3

Do not change the default during implementation.

Manual calibration must determine whether another value should become the
accepted default.


MANUAL ACCEPTANCE STATUS

The feature remains MANUAL ACCEPTANCE PENDING.

Do not claim a preferred or accepted sensitivity.

Do not claim the default should change.

Stop after automated validation and manual-test handoff.


STOP

After automated validation and manual-test handoff:

STOP.

Do not launch the game.

Do not stage.

Do not commit.

Do not push.

Do not begin Level 9.

TEMPEST_SPINNER_SENSITIVITY_IMPLEMENTATION_20261108