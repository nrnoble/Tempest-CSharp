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

Perform a tightly bounded, read-only investigation of the Tempest project's
current physical spinner/mouse input sensitivity and recommend the smallest
safe design for a future command-line sensitivity option.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

d5ccbe1d3ef8af21a02bd53c8c465ed0c4908fef
Close out accepted production Level 8

Confirm the actual branch, HEAD, remote tracking state, index, worktree, and
untracked state before beginning.

This task is investigation-only.

Do not implement a sensitivity option.

Do not modify production code, tests, durable documentation, configuration,
project files, recordings, screenshots, reference material, or Git state.

Do not stage, commit, push, clean, move, rename, or delete anything.

Do not launch the interactive game.


USER OBSERVATION AND GOAL

Neal uses a physical USB spinner that presents to the current game through the
mouse/input path.

Current play is functional, but the spinner is too sensitive.

A small physical spinner movement can move the Blaster across multiple lanes
very quickly.

This makes precise lane control harder than desired, especially as later levels
become more difficult.

Neal does not want to change Windows mouse settings merely to play Tempest.

The goal is to investigate adding a Tempest command-line option that changes
physical spinner/mouse sensitivity inside the game.

A possible option name is:

--spinner-sensitivity <multiplier>

Do not assume that this is necessarily the best final name.

Investigate the existing command-line conventions and recommend the clearest
name and semantics.

Possible intended examples include:

--spinner-sensitivity 1

Current accepted project sensitivity.

--spinner-sensitivity 0.5

Half the current response to physical spinner/mouse movement.

--spinner-sensitivity 0.75

Seventy-five percent of the current response.

The future option should initially be a diagnostic/manual-calibration feature.

Do not change the accepted default sensitivity during this investigation.


IMPORTANT CONTROL INVARIANT

The accepted Levels 1-8 spinner rule is based on physical rotational direction,
not universal screen-left/screen-right movement.

Physical clockwise spinner movement must produce clockwise Blaster movement
around the tube.

Physical counter-clockwise spinner movement must produce counter-clockwise
Blaster movement.

On closed tubes, the Blaster circulates around the continuous rim.

On open Level 8:

- clockwise physical spinner movement follows the clockwise open-rim path and
  appears visually toward the left endpoint;
- counter-clockwise physical spinner movement follows the counter-clockwise
  path and appears visually toward the right endpoint;
- movement stops at the corresponding open endpoint;
- no wrapping occurs across the opening.

A future sensitivity multiplier must change movement magnitude only.

It must not change:

- clockwise/counter-clockwise meaning;
- Level 8 winding-aware lane mapping;
- keyboard direction;
- controller direction;
- open-endpoint behavior;
- lane identity;
- recording/playback semantics.

The accepted Level 8 winding-aware correction must remain intact.


PRIMARY QUESTIONS

The investigation must answer:

1. Where does raw physical spinner/mouse movement enter the game?

2. What exact value does Raylib or the current input API provide?

3. Is that value:
   - a relative mouse delta;
   - an absolute cursor-position difference;
   - a wheel value;
   - an accumulated count;
   - or another representation?

4. What current constant or multiplier controls spinner response?

5. Where and how is sensitivity applied?

6. Does the project preserve fractional mouse/spinner movement between ticks,
   or does it discard fractional values?

7. How does input accumulate until the Blaster crosses from one lane to the
   next?

8. Can a single large mouse delta cross several lanes in one simulation tick?

9. Is multi-lane movement in one tick intentional, bounded, or unrestricted?

10. Does the current implementation apply any:
    - dead zone;
    - acceleration;
    - smoothing;
    - filtering;
    - clamping;
    - quantization;
    - fractional remainder;
    - maximum per-tick delta?

11. How do Windows mouse sensitivity and acceleration settings affect the input
    value received by the project?

12. Does Raylib use raw mouse input for the project, cursor movement after
    Windows processing, or a platform-dependent path?

13. Does the USB spinner appear to Windows and Raylib as an ordinary relative
    mouse device?

14. How do keyboard and controller movement bypass or share this path?

15. How does Level 8's winding-aware clockwise mapping interact with the
    sensitivity calculation?

16. What does gameplay recording store:
    - raw physical spinner/mouse delta;
    - sensitivity-adjusted movement;
    - accumulated rim displacement;
    - lane transitions;
    - or final effective per-tick input?

17. Would changing sensitivity alter existing recorded playback?

18. Should sensitivity be serialized into new recordings?

19. Would visible playback, headless playback, pause, stepping, reverse
    reconstruction, and screenshots remain deterministic?

20. What future implementation would let Neal tune physical spinner response
    without modifying Windows settings?


CURRENT INPUT PIPELINE

Trace the complete physical-spinner/mouse path.

Inspect the exact files, methods, properties, constants, and state involved,
including as applicable:

- Program.cs;
- DebugController.cs;
- command-line/runtime options;
- InputSystem;
- GameplayInput;
- PlayerInput;
- RawPlayerInput;
- mouse-delta acquisition;
- spinner sensitivity constants;
- TempestGame.UpdatePlayerMovement;
- Player.MoveAlongTube;
- lane-position accumulation;
- open/closed topology handling;
- Level.ClockwiseLaneIndexDelta;
- gameplay recording;
- gameplay playback;
- input diagnostics;
- controller handling;
- keyboard handling;
- relevant tests.

Document the path from physical input acquisition through final Blaster
movement.

Include units and numeric conversions at every stage.

Do not describe the input pipeline only at a high level.


SIMULATION-TICK INTERACTION

The accepted game uses a nominal 60-Hz interactive simulation/host cadence.

Determine how spinner input is sampled and consumed per simulation tick.

Establish:

- whether a delta is consumed once per host iteration;
- whether a delta may remain pending across simulation ticks;
- whether sub-lane movement accumulates;
- how many project-space movement units correspond to one lane;
- how geometry affects the amount required to cross a lane;
- whether lanes of differing perimeter length change perceived sensitivity;
- whether open Level 8 behaves differently from closed levels;
- whether `--speed` changes the number of input samples per real-world second;
- whether the same physical movement at `--speed 0.5`, `1`, and `2` yields
  different effective Blaster travel.

Do not change `--speed`.

Report any interaction that a future sensitivity feature must document.


MULTI-LANE JUMPS

Neal reports that a small spinner movement can quickly jump a couple of lanes.

Determine the exact cause.

Possible causes to evaluate include:

- current sensitivity multiplier is too large;
- one USB spinner count maps to too much project movement;
- Windows pointer acceleration has already processed the delta;
- the project applies an additional multiplier;
- large deltas are consumed in one simulation step;
- lane transitions are processed repeatedly until the full delta is consumed;
- no per-tick clamp exists;
- no fractional accumulator exists;
- geometry makes some lanes quicker to cross;
- input sampling frequency interacts with accumulated device counts.

Do not select a cause without tracing the code.

Quantify the current mappings wherever the source permits it.

If a physical device count cannot be inferred statically, say so explicitly.


WINDOWS-SETTING DEPENDENCE

Investigate whether normal Windows pointer settings can materially affect the
input values used by Tempest.

This is a code/reference investigation, not a Windows configuration change.

Do not change:

- Windows pointer speed;
- Enhance Pointer Precision;
- registry settings;
- device software;
- USB spinner settings.

Determine whether the current project already uses or can use a raw-input path
independent of Windows pointer acceleration.

Do not implement raw input in this task.

If current evidence cannot prove how Raylib behaves on this Windows/device
combination, classify it as UNKNOWN / REQUIRES MEASUREMENT.

Do not guess.


MARK INCITTI ENGINEERING REFERENCE

Inspect the project-local Mark Incitti reference implementation as a secondary
engineering source if it is available.

Neal previously found Incitti's spinner response excellent with the same USB
controller and without changing Windows mouse settings.

Determine:

- how Incitti obtains mouse/spinner movement;
- any sensitivity constant;
- any scaling;
- any fractional accumulation;
- any clamping;
- any acceleration/filtering;
- how movement is applied to the Blaster;
- whether control magnitude depends on frame rate;
- whether the mechanism differs materially from current C# behavior.

Do not treat Incitti's implementation as Atari ROM authority.

Classify it as SECONDARY ENGINEERING REFERENCE.

Do not copy its values blindly.

Use it to explain possible implementation differences.


ARCADE ROM / HARDWARE REFERENCE

Use the project-local Atari sources where useful:

Reference/arcade/Tempest Rom Source code.md

Reference/arcade/roms.rev2/

Investigate only enough to understand:

- how arcade spinner counts are sampled;
- whether the ROM applies scaling;
- whether the ROM accumulates fractional or integer movement;
- whether it clamps movement;
- whether physical spinner counts map directly to segment/lane movement;
- whether input cadence affects movement;
- how clockwise/counter-clockwise direction is represented.

Do not assume PC mouse counts equal Atari spinner counts.

Do not derive a literal PC sensitivity value from arcade hardware unless the
conversion is genuinely established.

Do not modify, stage, or commit the ROM binaries.


RECORDING AND PLAYBACK

Trace the accepted recording/playback path precisely.

Determine whether recordings store:

- raw device input;
- effective sensitivity-adjusted movement;
- final simulation input;
- resulting player position;
- or another representation.

The preferred future behavior is likely:

- physical sensitivity affects live input acquisition;
- recordings preserve effective per-tick input;
- playback reproduces the recorded result independent of the machine's current
  spinner sensitivity setting;
- `--spinner-sensitivity` does not need to be stored in the recording.

But do not assume this.

Confirm it from the current code and tests.

If existing recordings would change under a sensitivity option, identify exactly
why.

Do not change the recording schema.


KEYBOARD AND CONTROLLER SCOPE

Determine which controls a future spinner-sensitivity option should affect.

Preferred candidate scope:

- physical spinner presented as mouse movement;
- ordinary mouse movement used as spinner input.

It should probably not affect:

- keyboard movement;
- controller digital movement;
- controller quit behavior;
- playback input;
- replay reconstruction.

However, inspect the current architecture before recommending this policy.

If analog controller movement shares the same magnitude path, report whether it
should be included or given a separate option.

Do not implement separate options during this investigation.


OPTION NAME AND SEMANTICS

Evaluate command-line names such as:

--spinner-sensitivity
--mouse-sensitivity
--spinner-scale
--spinner

Recommend one canonical option name.

Prefer a name whose purpose is obvious.

Recommend precise semantics.

A likely model is:

effective spinner movement =
    raw live mouse/spinner delta * sensitivity multiplier

Candidate examples:

--spinner-sensitivity 1
    Current accepted response.

--spinner-sensitivity 0.5
    Half current response.

--spinner-sensitivity 0.75
    Three-quarters current response.

--spinner-sensitivity 1.25
    Twenty-five percent more response.

Do not implement the option.

Determine whether a multiplier is the best abstraction or whether another
project-space unit would be clearer.


DEFAULT VALUE

Do not change the default as part of this investigation.

Current behavior must remain the initial 1.0/default reference.

Neal will manually test candidate values later.

Only after manual calibration should the project consider whether another
default is more arcade-accurate.

Do not recommend a new default without evidence.


SUPPORTED RANGE AND VALIDATION

Recommend a practical supported range.

Evaluate a bounded range such as:

0.1 through 4.0 inclusive

or another evidence-based range.

The future parser should reject:

- missing value;
- malformed value;
- zero;
- negative values;
- NaN;
- positive infinity;
- negative infinity;
- out-of-range values;
- duplicate options according to existing CLI policy.

Do not silently clamp invalid input.

Use invariant-culture parsing.

Do not implement this now.


FRACTIONAL ACCUMULATION

Investigate whether simply multiplying the current delta by a fractional
sensitivity value is sufficient.

For example:

--spinner-sensitivity 0.5

must not lose every odd/small input delta because integer conversion truncates
the result to zero.

Determine whether a future implementation requires:

- a floating-point accumulator;
- persistent fractional remainder;
- fixed-point arithmetic;
- or existing float movement already preserves fractions.

Recommend deterministic behavior.

The same sequence of raw live input samples with the same multiplier should
produce the same simulation result.

Do not inject nondeterministic smoothing.


CLAMPING AND RESPONSIVENESS

Evaluate whether the sensitivity option alone is sufficient to address
multi-lane jumps.

Do not automatically add a per-tick movement clamp.

A clamp could improve control but may also discard legitimate high-speed
spinner motion and reduce arcade fidelity.

If a clamp appears necessary, treat it as a separate design decision and
explain:

- why sensitivity scaling alone is insufficient;
- what raw/effective unit would be clamped;
- how remainder is preserved;
- how rapid spinner motion remains responsive.

Prefer the smallest solution that addresses Neal's observation.


RUNTIME FEEDBACK

Assess whether a future implementation should display or log the selected
sensitivity.

Possibilities include:

- startup console line;
- debug-only message;
- command-line help/README only;
- no in-game HUD display.

Do not add an in-game settings menu.

Do not broaden this milestone into runtime configuration UI.


HEADLESS BEHAVIOR

A spinner sensitivity setting should not affect headless playback or tests that
supply already-effective simulation input.

Determine the correct policy if `--spinner-sensitivity` is supplied with a
headless playback command:

- accept but ignore for headless playback;
- reject as inapplicable;
- or another policy consistent with existing CLI design.

Prefer preserving unthrottled deterministic headless playback.


INTERACTION WITH `--SPEED`

Investigate how `--spinner-sensitivity` should interact with the accepted
`--speed` option.

The sensitivity multiplier should not be automatically multiplied by the speed
multiplier.

Determine whether live input sampling at 30, 60, or 120 host iterations per
second changes effective physical spinner response.

Recommend manual tests combining:

- normal speed and sensitivity 1.0;
- normal speed and reduced sensitivity;
- `--speed 0.5` with reduced sensitivity;
- `--speed 2` with reduced sensitivity.

Do not change `--speed` semantics.


EVIDENCE CLASSIFICATION

Classify findings as appropriate:

- ROM-LISTING-BACKED
- REV-2-BINARY-CONFIRMED
- EXISTING C# IMPLEMENTATION
- AUTOMATED-TEST EVIDENCE
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR
- MANUALLY OBSERVED
- SECONDARY ENGINEERING REFERENCE
- PROJECT TRANSLATION CANDIDATE
- DESIGN RECOMMENDATION
- UNKNOWN / REQUIRES MEASUREMENT

Do not present PC mouse scaling as Atari hardware fact.


OPTIONAL READ-ONLY TESTS

Run existing focused tests only when they answer a specific investigation
question.

Do not add or modify tests.

Do not run the full suite or build merely as ritual when no production code is
changed.

Do not launch the interactive game.

Do not benchmark Neal's hardware.

Do not change Windows settings.


NO IMPLEMENTATION

Do not:

- add the command-line option;
- modify input scaling;
- change spinner sensitivity;
- add a clamp;
- change keyboard/controller behavior;
- change recording/playback;
- change `--speed`;
- modify tests;
- update durable documentation;
- stage;
- commit;
- push.

Stop after investigation and recommendation.


PROPOSED FUTURE IMPLEMENTATION

End the report with a bounded future implementation recommendation.

The preferred candidate should preserve:

- default/no-option behavior exactly;
- multiplier 1.0 equals current behavior;
- only live mouse/spinner magnitude changes;
- physical clockwise/counter-clockwise semantics remain unchanged;
- Levels 1-8 topology behavior remains unchanged;
- keyboard/controller behavior remains unchanged unless evidence says otherwise;
- recordings remain compatible;
- playback remains independent of current sensitivity;
- headless playback remains deterministic;
- `--speed` remains independent;
- fractional movement is preserved;
- invalid values are rejected clearly.

Identify the smallest files and abstractions likely to change.

Do not implement them now.


PROPOSED AUTOMATED TESTS

Recommend future tests covering:

1. no option preserves the current default;
2. explicit 1.0 equals the default;
3. 0.5 halves effective live spinner movement;
4. 0.75 scales movement correctly;
5. supported maximum scales movement correctly;
6. small fractional input is accumulated rather than discarded;
7. the same samples produce deterministic results;
8. clockwise/counter-clockwise direction does not change;
9. closed Levels 1-7 preserve wrapping/control behavior;
10. open Level 8 preserves endpoint clamping and winding-aware direction;
11. keyboard movement is unchanged;
12. controller movement is unchanged under the recommended scope;
13. recordings do not gain a sensitivity field unless evidence requires one;
14. visible playback state is unaffected by the current sensitivity setting;
15. reverse reconstruction remains exact;
16. headless playback remains unaffected;
17. `--speed` and sensitivity remain independent;
18. parsing accepts valid invariant decimal values;
19. parsing rejects zero, negative, non-finite, malformed, missing, duplicate,
    and out-of-range values;
20. existing input/playback regression tests remain passing.

Do not create these tests now.


PROPOSED MANUAL CALIBRATION PLAN

Recommend a bounded manual calibration sequence.

Include candidate values such as:

1.0
0.75
0.5
0.4
0.3

Adjust the candidate list based on the traced current multiplier if another set
would be more informative.

The manual plan should ask Neal to evaluate:

- small precise one-lane corrections;
- slow controlled spinner motion;
- rapid emergency movement;
- closed-tube traversal;
- Level 8 open-endpoint control;
- ability to stop on a target lane;
- whether multi-lane jumps still occur;
- clockwise/counter-clockwise consistency;
- Level 1 and Level 8 at minimum;
- higher-level combat when practical;
- interaction with `--speed`;
- recording/playback independence.

The future feature should initially be accepted as a diagnostic/calibration
tool.

Do not recommend changing the default until Neal identifies a clearly preferred
value.


AI-LOG POLICY

These instructions are explicit and non-conflicting:

1. Save this complete substantive prompt under:

   AI-Logs/Prompts/

2. Create a NEW investigation report under:

   AI-Logs/Reports/

3. Creating these new ignored files is authorized and expected.

4. Existing reports remain historical records and must not be rewritten.

5. AI-Logs/ is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place the investigation report under TEMP/ or Temp/.

8. TEMP/ and Temp/ are not the normal destination for milestone reports.


REPORT REQUIREMENTS

Create a descriptive date-prefixed report under AI-Logs/Reports/.

The report must contain:

1. Starting Git state.
2. Accepted baseline.
3. Neal's sensitivity observation and goal.
4. Files, tests, references, and ROM regions inspected.
5. Complete current input pipeline.
6. Raw input units and acquisition behavior.
7. Current sensitivity constants and multipliers.
8. Fractional accumulation behavior.
9. Multi-lane movement behavior.
10. Windows-setting dependency findings.
11. Keyboard scope.
12. Controller scope.
13. Level 8 winding-aware interaction.
14. `--speed` interaction.
15. Recording format and effective-input findings.
16. Visible playback behavior.
17. Headless playback behavior.
18. Pause/step/reverse implications.
19. Mark Incitti comparison.
20. Arcade ROM/hardware findings.
21. Exact unresolved questions.
22. Recommended option name.
23. Recommended semantics.
24. Recommended supported range.
25. Recommended invalid-input behavior.
26. Recommended fractional/remainder strategy.
27. Whether a clamp is needed.
28. Smallest future implementation architecture.
29. Expected files to change.
30. Proposed automated tests.
31. Proposed manual calibration plan.
32. Final Git state proving no tracked change.


FINAL REPOSITORY VERIFICATION

Before stopping, verify:

- HEAD remains at the accepted Level 8 baseline;
- no tracked files changed;
- index remains clean;
- no reference files changed;
- no ROM files changed;
- no recording or screenshot changed;
- only the authorized ignored AI prompt/report files were added.


STOP POINT

Stop after the read-only investigation and recommendation.

Do not implement the option.

Do not begin Level 9.

At completion, provide Neal a concise summary answering:

- Why is the current spinner response so sensitive?
- What value currently controls it?
- Does Windows mouse configuration affect it?
- How does the USB spinner reach the game?
- Are fractional movements preserved?
- Why can one movement cross multiple lanes?
- Would a multiplier alone solve the problem?
- What should the option be called?
- What should 1.0, 0.5, and 0.75 mean?
- Should keyboard/controller input be affected?
- Would recordings remain compatible?
- Would playback remain independent of sensitivity?
- How does the option interact with `--speed`?
- What is the smallest safe future implementation?
- What values should Neal manually test?

TEMPEST_SPINNER_SENSITIVITY_INVESTIGATION_20261108