READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Diagnose and correct the reproducible Level 7 Atari-presentation crash caused
by mapped Flipper transition rendering.

This is a narrowly bounded crash-correction milestone.

Do not work on the other remaining Level 7 visual problems during this task.

MANUAL RESULT

Neal manually ran:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

The latest render-registration and centering candidate is substantially
improved:

- the record-06 tube is correctly placed;
- gameplay presentation is aligned with the visible tube;
- gameplay itself appears to behave correctly.

However, the game crashes within a few seconds during ordinary Level 7 play.

The failure occurred on two consecutive launches with this exception:

System.InvalidOperationException:
A mapped Level 7 Flipper pivot ray did not intersect the tube polygon.

Stack:

AtariLevel7EntityPresentation.CalculateFirstPolygonBoundaryDistance(
    Vector2 origin,
    Vector2 rayDirection,
    float normalizedDepth)
in AtariLevel7EntityPresentation.cs, line 196

AtariLevel7EntityPresentation.MapFlipperTransition(
    DirectedLaneTransition transition,
    float progress,
    float normalizedDepth)
in AtariLevel7EntityPresentation.cs, line 144

VectorRenderer.ProjectFlipperEndpoints(
    Flipper flipper,
    IReadOnlyList<Vector2> points,
    float projectedDepth)
in VectorRenderer.cs, line 1428

VectorRenderer.DrawFlipper(...)
VectorRenderer.DrawFlippers(...)
VectorRenderer.Draw(...)
Program.Main(...)

The exact line numbers may shift as files change. Locate the failure by method
and exception text, not only by line number.

PRIMARY OBJECTIVE

Determine why a valid ordinary Level 7 Flipper transition produces a mapped
pivot ray that does not intersect the mapped record-06 tube polygon.

Implement the smallest mathematically justified render-only correction that:

- prevents this normal Level 7 transition from crashing;
- preserves the correct visual transition path;
- preserves directed transition behavior;
- preserves the newly aligned record-06 presentation;
- and does not alter gameplay or simulation state.

Do not merely suppress the exception.

Do not add a broad catch block.

Do not return a fabricated endpoint.

Do not silently skip drawing the Flipper.

Do not fall back to stale production-tube screen coordinates.

ROOT-CAUSE INVESTIGATION

Trace the exact failing transition state through:

1. gameplay source lane;
2. gameplay destination lane;
3. directed transition direction and intended sweep;
4. transition progress;
5. normalized entity depth;
6. mapped record-06 lane geometry at that depth;
7. transition origin;
8. pivot center or reference point;
9. ray direction;
10. mapped tube polygon used for intersection;
11. polygon winding;
12. ray/segment intersection math;
13. accepted/rejected intersection parameters;
14. selection of the first boundary distance;
15. the final mapped Flipper endpoints.

Determine whether the failure is caused by one or more of:

- a wrong polygon being intersected;
- polygon points in an unsuitable order;
- ray direction pointing away from the intended boundary;
- origin lying outside the polygon;
- origin lying on or numerically very near a boundary;
- concavity of the mapped record-06 cross-section;
- an assumption that the mapped tube cross-section is convex;
- a directed 180-degree or 270-degree transition;
- an invalid correspondence between gameplay transition direction and visible
  record-lane direction;
- near-parallel ray/edge geometry;
- endpoint/tolerance handling;
- normalized-depth extrapolation;
- or another evidenced error.

Do not assume the prior crash from several days ago had the same cause. Compare
only if the repository contains directly relevant evidence.

REPRODUCE WITHOUT INTERACTIVE GAMEPLAY

Create a deterministic automated reproduction of the exact failure or the
smallest equivalent normal production-reachable state.

Prefer capturing the actual source lane, destination lane, direction, progress,
and depth that make the intersection fail.

If the exact runtime tuple cannot be recovered from the exception alone,
derive all production-reachable Level 7 Flipper transition/depth combinations
and test them with bounded enumeration.

Any enumeration must have explicit bounds and must cover:

- all 16 source lanes;
- both directed transition directions where production permits them;
- relevant destination lanes;
- representative transition progress including endpoints and interior values;
- relevant ordinary and Tanker-cargo Flipper depths;
- and mapped geometry from near to far.

Do not launch the interactive game.

Do not require Neal to reproduce the failure again.

CORRECTION STANDARD

The correction must follow the actual directed Level 7 transition geometry.

Prefer deriving the visible transition directly from mapped record-06 lane
geometry and the existing simulation transition state rather than relying on
an unsupported ray-against-whole-polygon shortcut.

If ray/polygon intersection remains appropriate, make its geometric contract
valid and explicit.

If record-06 concavity makes the current whole-polygon ray method invalid,
replace only that presentation calculation with a construction that correctly
handles the mapped Level 7 shape.

Possible correction directions must be chosen from evidence, not from this
prompt. Examples to evaluate, not assume, include:

- intersecting the intended mapped lane boundaries rather than an entire
  concave polygon;
- deriving the pivot path from the mapped source/destination edge geometry;
- preserving the gameplay transition's directed angular sweep in the mapped
  local basis;
- or correcting the ray origin/direction and intersection-selection rule.

Do not replace directed rotation with shortest-path interpolation.

Preserve the established Tempest requirement that a directed 270-degree
transition remains the long directed route when gameplay specifies it.

FAILURE BEHAVIOR

Normal production-reachable Flipper transitions must not throw.

Retain fail-fast validation for genuinely malformed or impossible internal
state where useful.

Clearly distinguish:

- valid but previously unsupported mapped geometry;
- floating-point boundary cases;
- and genuinely invalid state.

Do not convert all errors into silent rendering fallbacks.

FROZEN PRESENTATION AND GAMEPLAY

Do not change or reopen:

- record-06 bytes or metadata;
- record selection;
- point order;
- rim connectivity;
- near/far pairing;
- AtariCoordinateProjector;
- near p=$10 or far p=$F0;
- accepted Level 7 anisotropic gain;
- accepted relative far-set translation;
- latest shared centering translation;
- lane/depth registration for non-Flipper objects;
- gameplay Level 7 geometry;
- gameplay lanes;
- Flipper movement state;
- transition timing;
- directed transition choice;
- ordinary/Tanker-cargo behavior;
- collision;
- firing;
- spawning;
- scoring;
- lives;
- RNG;
- recording/playback data or simulation.

Preserve the manual result that the tube is now correctly placed and gameplay
presentation is aligned with it.

Do not retune other entities.

Do not address remaining Level 7 visual issues in this milestone.

FLIPPER CLASSES

Verify the correction for both:

- ordinary Flippers;
- Tanker-released cargo Flippers.

Both currently share DrawFlipper and may reach the same presentation mapping.

Do not assume one test covers both lifecycle paths if their transition states
can differ.

Verify below-rim transitions at relevant depths and any production-reachable
rim transition handled by this path.

PRESENTATION-ONLY REQUIREMENT

The correction may read transition and gameplay state but must not write or
normalize simulation state.

The same recording or deterministic input sequence must produce identical
underlying gameplay before and after this fix.

Only rendered Flipper placement/orientation may change.

TESTING REQUIREMENTS

Add regression coverage proving at minimum:

1. The exact reproduced failing transition no longer throws.
2. The corrected endpoints are finite.
3. The corrected endpoints lie on the intended visible mapped transition
   geometry.
4. The apparent Flipper anchor remains registered to the visible record-06
   tube.
5. The mapped tangent/normal and artwork orientation remain coherent.
6. Directed clockwise and counterclockwise transitions remain distinct.
7. A directed 270-degree transition retains its long directed sweep.
8. Transition progress is continuous across representative interior samples.
9. Progress endpoints agree with mapped source and destination lane geometry.
10. Near, middle, and far ordinary depths are supported.
11. Production-reachable depth extremes do not throw.
12. Ordinary Flippers are covered.
13. Tanker-released Flippers are covered.
14. All 16 lanes are covered with bounded exhaustive or equivalent invariant
    tests.
15. No valid production-reachable transition triggers the former exception.
16. Genuinely malformed state still fails explicitly where appropriate.
17. Record-06 mapping is unchanged.
18. Tube centering is unchanged.
19. Non-Flipper registration is unchanged.
20. Gameplay and transition state are unchanged by rendering.
21. Deterministic recording/playback behavior is unchanged.
22. Legacy and option-off rendering remain unchanged.
23. Levels 1-6 and 8-9 remain unchanged.
24. Approach, exit, transition-between-levels, and Game Over remain unchanged.
25. The separate Blaster-exit defect remains out of scope.

Use geometric assertions, not screenshot pixels.

VALIDATION

Run bounded focused tests for:

- AtariLevel7EntityPresentation;
- mapped Flipper transition geometry;
- all-lane/all-direction production-reachable coverage;
- ordinary and Tanker-cargo Flippers;
- render-only state preservation.

Run relevant targeted regressions covering:

- Level 7 gameplay;
- Level 7 Atari presentation;
- Flipper rendering and transitions;
- directed transitions and pivots;
- Tanker cargo;
- record-06 mapping;
- centering/registration;
- recording/playback;
- Levels 1-9 presentation selection;
- legacy/option-off behavior.

Then run:

- the full solution test suite with explicit VSTest hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Canceled, timed-out, stalled, blocked, or incomplete commands are not passes.

Do not launch the interactive game.

REPOSITORY SAFETY

Before changes, record:

- branch;
- HEAD;
- upstream;
- index;
- tracked worktree;
- relevant untracked state.

Preserve all pre-existing candidate work.

Do not reset, restore, clean, stash, rebase, switch branches, stage, commit,
or push.

Do not absorb unrelated changes.

IMAGE SCOPE

No image analysis is required or authorized for this correction.

Do not search for, enumerate, open, inspect, or analyze images, screenshots,
videos, frames, contact sheets, or unknown visual assets.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired crash-correction report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide Neal the exact manual retest command:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --atari-presentation --level 7

State clearly that this retest is limited to confirming:

- the crash no longer occurs during ordinary Flipper transitions;
- ordinary and Tanker-released Flippers remain visually registered to the
  visible tube;
- gameplay remains unchanged;
- tube centering and the alignment improvements remain intact.

Do not ask Neal questions during execution.

Do not pause for input.

Manual acceptance remains PENDING.

EXPECTED REPORT

The paired report must include:

- exact root cause;
- exact failing transition/state reproduced;
- whether the failure was production-reachable;
- files changed;
- geometric correction;
- why the correction is valid for the concave record-06 presentation;
- handling of directed long-path transitions;
- ordinary and Tanker-cargo coverage;
- before/after representative geometry;
- state-preservation evidence;
- focused validation results;
- targeted regression results;
- full-suite result;
- Release-build result;
- git diff --check result;
- starting and ending Git state;
- exact manual command;
- manual acceptance status PENDING;
- exact prompt/report paths.

Do not stage, commit, or push.

The task ends when the reproducible Level 7 mapped-Flipper rendering crash has
been corrected and the candidate is ready for Neal's manual retest.

TEMPEST_LEVEL7_MAPPED_FLIPPER_TRANSITION_CRASH_FIX