Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Read-only investigation of right-end rim-Flipper oscillation

Perform a tightly bounded READ-ONLY investigation.

Do not implement a correction.

Do not modify production source, tests, configuration, durable documentation, gameplay behavior, or Git state.

## Manual observation

Neal has identified a new Flipper rim-pursuit problem.

Observed behavior:

1. A Flipper is already on the rim of a tube with endpoints.
2. The Flipper travels toward the far-right endpoint.
3. At the right endpoint, the Flipper reverses direction as expected.
4. The Flipper then travels left by only one lane.
5. The Flipper immediately reverses again and travels right.
6. The cycle repeats between the right endpoint and its adjacent lane.

Neal has not observed the corresponding problem at the left endpoint.

The issue occurs on:

- Level 12;
- Level 10.

Do not assume the defect is Level-12-specific.

Do not assume the defect is global.

Do not assume the right-end reversal itself is wrong. The first reversal at the actual endpoint appears correct. The suspicious behavior is the **second reversal after only one lane of travel away from the endpoint**.

## Objective

Determine exactly why a rim Flipper that correctly reverses at the right endpoint reverses again after moving only one lane left.

Identify the first incorrect state transition.

Classify the defect as exactly one of:

- GLOBAL RIM-PURSUIT DEFECT
- OPEN-TUBE ENDPOINT DEFECT
- SHARED ENDPOINT-ORIENTATION DEFECT
- LEVEL-SPECIFIC DEFECT
- CURRENT BEHAVIOR IS EXPECTED
- INCONCLUSIVE

Do not change production behavior.

## Use current local source as primary evidence

The active worktree is ahead of GitHub because Level 12 remains uncommitted.

Inspect the current local implementation first.

GitHub nrnoble/Tempest-CSharp remains the master reference for the last committed baseline and may be used to compare the accepted implementation where useful.

If GitHub retrieval unexpectedly returns no content or an invalid response, do not silently substitute assumptions. Record that GitHub retrieval failed so Neal can reset/reconnect the connector.

## Relevant Flipper state

Trace all rim-pursuit state that can affect direction.

At minimum inspect and report:

- Direction;
- RimPursuitDirection;
- current rim lane/edge;
- movement state;
- source lane;
- destination lane;
- pivot/transition state;
- transition completion;
- endpoint detection;
- open/closed topology;
- lane count;
- geometry orientation/winding sign;
- any normalization/clamping/wrapping behavior.

Determine which state is authoritative for deciding the next rim move.

Do not assume Direction and RimPursuitDirection always agree.

## Deterministic Level 10 reproduction

Create or use the smallest bounded deterministic/headless reproduction of the Level 10 right-end behavior.

Do not launch the interactive game.

Drive one rim Flipper toward the right endpoint and capture state at each relevant completed lane transition.

Record a compact trace covering at least:

- two lanes before the right endpoint;
- arrival at the right endpoint;
- first correct reversal;
- move one lane left;
- second unexpected reversal;
- return toward the endpoint.

For each transition record:

- simulation tick or deterministic step;
- current lane;
- source/destination lane if transitioning;
- Direction;
- RimPursuitDirection;
- movement state;
- endpoint predicate results;
- orientation/winding value used;
- next chosen lane;
- reason/branch selecting that next lane.

Do not merely record screen coordinates.

We need the simulation-state cause.

## Left-end control

Using equivalent deterministic state, drive a rim Flipper to the left endpoint.

Capture the same state fields for:

- arrival;
- reversal;
- first lane away;
- second lane away.

Determine why the left end does not exhibit the same immediate second reversal, if that observation is reproduced.

The left endpoint is an important control.

## Level 12 comparison

After reproducing Level 10, perform the same minimal state trace on Level 12.

Determine whether:

- the exact same branch causes the second reversal;
- lane counts alter the condition;
- closed-versus-open topology changes the mechanism;
- orientation/winding differs;
- Level 12 merely exposes an already-existing shared issue.

If Level 12 is closed and therefore has no true open endpoint in the gameplay model, explain precisely what “far right” means in the state machine and why the same visible oscillation occurs.

Do not force Level 10 and Level 12 into one explanation unless the state traces support it.

## Previously accepted baseline comparison

Inspect the committed aa6b7110… baseline where useful.

Determine whether the right-end oscillation:

- already exists in committed Level 10 behavior;
- was introduced by current Level 12/shared work;
- or cannot be established from static diff/history alone.

Do not claim a regression date unless evidence establishes it.

The user's observation that the issue is newly noticed does not by itself prove the code changed recently.

## Endpoint logic

Trace the exact predicates that decide that a Flipper:

- is at the left endpoint;
- is at the right endpoint;
- must reverse;
- may continue in the current direction.

Look for:

- <= versus <;
- >= versus >;
- last-lane versus last-edge confusion;
- vertex count versus playable-lane count;
- one-based/zero-based translation;
- endpoint predicate evaluated against source rather than destination;
- stale transition source/destination after pivot completion;
- direction being reversed once by endpoint handling and again by another pursuit rule;
- Direction and RimPursuitDirection being independently reversed;
- closed-tube wrapping accidentally treated as endpoint reversal;
- topology/orientation sign causing asymmetric endpoint normalization.

Do not choose a cause until the deterministic trace identifies the branch.

## Geometry versus gameplay direction

Separate:

- physical screen-left/screen-right;
- increasing/decreasing lane index;
- clockwise/counterclockwise rim movement;
- Direction;
- RimPursuitDirection.

Report the mapping for Level 10 and Level 12.

The bug may present visually on the right while being caused by index/orientation semantics elsewhere.

## Spinner independence

Do not modify or investigate player spinner behavior except where needed to clarify coordinate terminology.

This task concerns enemy rim Flippers.

The previously accepted physical spinner convention remains frozen.

## ROM/reference boundary

This appears to be a C# rim-pursuit state-machine problem.

Do not inspect local ROM binaries unless static C# and historical project evidence unexpectedly prove insufficient to determine the intended endpoint rule.

Do not inspect screenshots, videos, UserData, or Temp.

Neal's manual observation is sufficient visual evidence.

## Historical project evidence

Review tracked historical records relevant to:

- rim Flipper pursuit;
- open-tube endpoint behavior;
- Level 7/10 rim movement where relevant;
- directed lane transitions;
- accepted rim-speed work;
- previous endpoint/manual acceptance.

Distinguish:

- accepted manual behavior;
- project translation;
- ROM-backed behavior;
- current implementation.

Do not reopen unrelated Flipper below-rim movement programs.

## No production modifications

Do not change:

- Flipper.cs;
- TempestGame.cs;
- level geometry;
- orientation;
- admissions;
- enemy selection;
- rim speed;
- collisions;
- presentation;
- HUD;
- Level 12;
- Level 10.

Temporary instrumentation is permitted only if static inspection/headless state summaries cannot expose the branch.

Remove temporary instrumentation before completion.

Do not leave new tests in this read-only investigation unless the project's established investigation convention explicitly permits a test-only deterministic diagnostic fixture. Prefer no persistent changes.

## Report requirements

Produce a focused report containing:

### 1. Level 10 right-end trace

Show the state sequence from approach through the second reversal.

### 2. Level 10 left-end control

Show the corresponding correct sequence.

### 3. Level 12 comparison

State whether the same state transition causes the visible behavior.

### 4. First incorrect transition

Identify the exact method/branch/predicate causing the second reversal.

### 5. Why it is asymmetric

Explain why the right side is affected while the left side is not, if established.

### 6. Scope classification

Choose exactly one:

- GLOBAL RIM-PURSUIT DEFECT
- OPEN-TUBE ENDPOINT DEFECT
- SHARED ENDPOINT-ORIENTATION DEFECT
- LEVEL-SPECIFIC DEFECT
- CURRENT BEHAVIOR IS EXPECTED
- INCONCLUSIVE

### 7. Implementation readiness

Choose exactly one:

- READY FOR BOUNDED CORRECTION
- NEEDS MORE RUNTIME EVIDENCE
- NO CORRECTION JUSTIFIED
- STILL UNRESOLVED

If correction-ready, specify:

- smallest production change;
- exact invariant to preserve;
- focused regression tests;
- levels that should and should not change.

Do not implement the correction.

## Repository protection

This task is read-only except for its paired AI-Logs prompt/report records.

Do not stage, commit, or push.

Do not reset, restore, stash, clean, or rebase.

Preserve the current Level 12 candidate, accepted Fuseball bonus-vector work, deferred ExitZoom state, Level 11, and all unrelated work.

Stop after the investigation report.

TEMPEST_RIM_FLIPPER_RIGHT_ENDPOINT_OSCILLATION_READONLY