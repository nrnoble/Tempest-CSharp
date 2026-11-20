IMPORTANT: READ THIS ENTIRE PROMPT BEFORE DOING ANYTHING.

Do not use tools, read files, inspect Git, run commands, run tests, edit files, or perform any repository work until you have read this entire prompt from beginning to end.

A unique completion token appears ONCE and only once, on the FINAL line of this prompt.

After reading the entire prompt:

- If you reached the unique completion token on the final line, proceed.
- If you did NOT reach a unique completion token on the final line, STOP immediately.
- If stopped, respond only:

STOPPED: The complete prompt was not received.

Do not reconstruct missing instructions from previous chats, reports, source code, memory, or inference.

The completion token is intentionally NOT shown here. You must read to the very end to obtain it.

BLASTER-LANE HIGHLIGHTING IMPLEMENTATION

CURRENT CLEAN CHECKPOINT

328c244 Add accepted between-level approach for natural progression

PRIMARY AUTHORITY

AI-Logs/Reports/2026-11-01-blaster-lane-highlighting-preimplementation-analysis-report.md

Supporting authority:

- current committed source/tests;
- TEMPEST-BEHAVIOR-SPEC.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- README.md;
- Reference/incitti read-only where necessary.

PURPOSE

Implement ONLY Blaster-lane highlighting.

The lane currently occupied by the Blaster must be identified by recoloring the two longitudinal tube/web rails bounding that lane.

For the currently implemented early levels, the highlight is yellow.

This is gameplay-relevant rendering because it lets the player focus deeper into the tube while retaining a clear visual indication of the Blaster's lane.

Do not combine this milestone with other fidelity work.

ARCADE AUTHORITY

Neal directly observes from Atari Tempest:

- the Blaster's occupied lane is highlighted by recoloring its two side/boundary rails;
- those highlighted rails run the full depth of the tube/web;
- the highlight follows the Blaster around the rim;
- the currently implemented early-level highlight color is yellow;
- later overall color schemes change, including across later 16-level groups.

Do not implement future palette cycling here.

REFERENCE FINDINGS

The pre-implementation analysis found Incitti behavior consistent with this feature:

- one indexed player lane is selected;
- the two depth-running rails bounding that lane are highlighted;
- Incitti palette 0 uses yellow RGB (255,255,0);
- pulse/Pulsar coloring takes precedence over player-lane coloring;
- player-lane coloring takes precedence over ordinary/Super-Zapper tube coloring.

Treat Incitti implementation details as REFERENCE VERIFIED, not automatically ARCADE VERIFIED.

1. LANE SOURCE OF TRUTH

Use the existing:

Player.EdgeIndex

as the Blaster's occupied lane.

Do not invent another lane tracker.

Player.EdgePosition remains the fractional position within the indexed lane.

Do not derive the highlighted lane from screen position.

Do not derive it from proximity.

Do not use shortest-path geometry.

Do not modify Player movement.

2. MOVEMENT / LANE-CHANGE RULE

Maintain exactly one highlighted lane.

While Player.EdgeIndex remains unchanged, keep that indexed lane highlighted.

When Player.EdgeIndex changes to the next lane, highlight the new indexed lane.

Do not highlight both source and destination lanes simultaneously.

Do not add another animated transition state.

Do not change existing directed lane-transition timing or geometry.

3. EXACT BOUNDARIES

For player lane index L, highlight exactly the two longitudinal rails corresponding to:

- boundary/edge vertex L;
- boundary/edge vertex NextEdge(L).

Derive both from existing Level topology.

The same topology-driven rule must work for:

- Level 1 circular tube;
- Level 2 square;
- Level 3 plus board.

Do not hard-code screen coordinates.

Do not create board-specific lane-highlight tables unless current topology demonstrably requires one and the analysis report explicitly supports it.

4. FULL DEPTH

Highlight the selected lane boundaries along their complete rendered tube depth.

Do not merely highlight:

- the outer rim;
- the Blaster itself;
- a short segment near the player.

The purpose is to preserve lane awareness while the player looks deeper into the tube.

Reuse existing tube-line geometry/endpoints.

Change color selection, not geometry.

5. EARLY-LEVEL COLOR

For the current Levels 1-3, render the Blaster-lane boundary rails in yellow.

Use the project's existing color conventions/types.

Do not scatter a universal hard-coded yellow rule throughout unrelated renderer code.

Provide the narrowest maintainable color-resolution seam so future palette work can supply a different player-lane color without changing the lane-selection algorithm.

Do NOT implement:

- 16-level palette cycling;
- Level 4;
- later color schemes.

6. COLOR PRECEDENCE

Implement the analyzed precedence:

Highest:
Pulsar/pulse lane overlay

then:
Blaster-lane highlight

then:
Super Zapper tube flash / ordinary tube color

This means:

- Pulsar overlay may visually override the Blaster-lane color where they compete;
- otherwise the Blaster-lane highlight should remain visible even when ordinary/Super-Zapper tube coloring is active.

Do not modify Super Zapper GAMEPLAY.

Do not modify Pulsar GAMEPLAY.

Only rendering color precedence is in scope.

7. BETWEEN-LEVEL APPROACH

The Blaster-lane highlight should also render during the accepted natural between-level approach.

The incoming board is already current during approach.

Use the same Player.EdgeIndex/topology rule.

As the Blaster moves around the distant incoming rim:

- the correct lane's two rails remain highlighted;
- highlighting follows the player naturally;
- existing approach projection/depth behavior remains unchanged.

Do not modify:

- LevelApproach timing;
- LevelApproach initial depth;
- smoothstep curve;
- elevated perspective;
- movement/control tuning;
- gameplay freeze rules.

8. STATE VISIBILITY

Follow the pre-implementation analysis state policy unless current code reveals a concrete conflict:

Show highlighting during:

- Playing;
- Approach;
- PlayerDying where the normal tube remains part of the accepted current presentation;
- active AvoidSpikes/development travel where appropriate under existing renderer conventions.

Hide highlighting during:

- ExitDelay;
- ExitZoom;
- terminal GameOver;
- terminal Complete presentation,

where the player-lane gameplay cue is no longer appropriate.

Do not change state transitions to achieve this.

If the exact current renderer/state mapping differs from these names, use the current equivalent and document it.

9. RENDERER OWNERSHIP

Keep this primarily a rendering concern.

Preferred design:

- VectorRenderer selects the effective color of each longitudinal tube rail;
- Player/Level remain authoritative for lane/topology state;
- gameplay does not know that the renderer is highlighting a lane.

A small pure/read-only rendering helper is acceptable if it materially improves testability or keeps color precedence easy to understand.

Do not move gameplay state into VectorRenderer.

Do not create a broad rendering framework for this feature.

10. AUTOMATED TESTS

Add focused headless tests using existing renderer callback/submission seams where practical.

At minimum prove:

A. LEVEL 1

- exactly the expected two longitudinal boundaries are yellow;
- adjacent unrelated boundaries retain normal color;
- projected endpoints are finite;
- geometry is unchanged.

B. LEVEL 2

- square-board occupied lane selects correct two boundaries;
- movement to a new lane changes the selected pair correctly;
- corner geometry does not cause wrong-boundary selection.

C. LEVEL 3

- plus-board occupied lane selects correct two boundaries;
- convex/reflex geometry does not change which topology boundaries belong to the lane;
- 90/180/270-degree movement semantics remain untouched.

D. FRACTIONAL MOVEMENT

- changing EdgePosition without changing EdgeIndex does NOT change the selected lane;
- when EdgeIndex changes, the selected highlighted pair changes exactly once.

E. APPROACH

- lane highlighting is submitted during Approach;
- movement during approach updates highlighted lane through the same EdgeIndex rule;
- approach projection remains finite;
- approach model/state is not modified.

F. COLOR

- early-level selected boundaries use yellow;
- normal unrelated tube boundaries keep existing colors.

G. PRECEDENCE

Where accessible through current test seams:

- Pulsar overlay overrides player highlight;
- player highlight overrides ordinary/Super-Zapper tube coloring.

Do not duplicate Pulsar/Super-Zapper gameplay tests.

H. STATE VISIBILITY

Protect the selected show/hide state policy without changing lifecycle behavior.

11. REGRESSION PROTECTION

Do not change:

- tube geometry;
- Projection.K;
- Projection.ZOffset;
- Player movement;
- spinner sensitivity/direction;
- controller mapping/tuning;
- directed lane transitions;
- Flipper movement;
- Tanker/cargo behavior;
- Super Zapper semantics;
- Pulsar semantics;
- score/lives;
- C3/C4 progression;
- LevelApproach behavior.

12. TRACKED DOCUMENTATION

After implementation, update narrowly as needed:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Record:

ARCADE VERIFIED:
- Neal's visible observation that occupied Blaster lane is marked by its two full-depth boundary rails;
- early implemented levels visibly use yellow, subject to Neal's later manual acceptance of the C# rendering.

REFERENCE VERIFIED:
- relevant Incitti implementation details and precedence.

PROJECT DECISION:
- any implementation/test seam and future color-resolution abstraction;
- exact state-visibility policy where Atari behavior has not been directly established.

Before Neal manually tests the implementation, do NOT claim the C# result has passed graphical acceptance.

13. FUTURE WORK — DO NOT IMPLEMENT

Keep explicitly deferred:

- death presentation fidelity;
- enemy retraction during death;
- Blaster-location death animation;
- slightly larger player shots;
- elevated between-level approach perspective;
- broader entity/rendering fidelity;
- spinner characterization/refinement;
- later levels;
- 16-level palette cycling;
- audio/sound fidelity;
- end-of-game animation/presentation;
- high-score tracking/persistence.

These are roadmap items only.

Do not change them in this milestone.

14. ITERATIVE VALIDATION

Run new/focused lane-highlighting tests first.

Then run a broader rendering/gameplay regression group covering at minimum:

- new lane-highlight tests;
- Level 1 simulation/rendering where available;
- Level2Square;
- Level3Geometry;
- LevelApproach;
- LevelProgression;
- SuperZapper;
- Pulsar;
- InputSystem/InputDiagnostic if rendering/input integrations are touched;
- relevant Simulation tests.

Use actual test-class names and valid filter syntax.

15. FULL AUTOMATED GATE

Before requesting manual graphical acceptance, run:

dotnet test TempestCSharp.slnx --no-restore

dotnet build TempestCSharp.slnx --no-restore

git diff --check

Then inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git ls-files --others --exclude-standard
git log -1 --oneline

Confirm:

- no movement changes;
- no geometry changes;
- no projection changes;
- no approach retuning;
- no death implementation;
- no shot-size change;
- no palette cycling;
- no audio;
- no end-game animation;
- no high-score implementation;
- no Level 4;
- no Reference/incitti modification;
- existing TEMP/ remains excluded.

16. AI WORK RECORD

Save this exact prompt under:

AI-Logs/Prompts/

Suggested filename:

2026-11-01-blaster-lane-highlighting-implementation-prompt.md

Create/update:

AI-Logs/Reports/2026-11-01-blaster-lane-highlighting-implementation-report.md

AI-Logs remains ignored.

Report:

- starting checkpoint;
- exact rendering rule;
- Incitti/source relationship;
- color precedence;
- approach/state behavior;
- tests/results;
- full validation;
- changed files;
- protected-scope inspection;
- manual acceptance still required;
- future palette boundary;
- deferred fidelity roadmap.

Do not create a reasoning transcript.

17. MANUAL ACCEPTANCE HANDOFF

If all automated gates pass, STOP before commit and provide Neal with a concise manual checklist.

Primary normal campaign command:

dotnet run --project src/Tempest.Game -- --lives 10

Manual checks:

LEVEL 1
- Blaster lane has two yellow full-depth rails;
- exactly the occupied lane is obvious;
- spinner movement makes highlighting follow naturally;
- fractional motion does not produce strange flicker;
- rail geometry itself is unchanged.

LEVEL 1 -> LEVEL 2 APPROACH
- distant square board shows the Blaster lane highlight;
- highlighting follows Blaster movement during approach;
- approach remains smooth and otherwise unchanged.

LEVEL 2
- correct square lane remains highlighted;
- corners select the correct lane pair;
- movement remains normal.

LEVEL 2 -> LEVEL 3 APPROACH
- distant plus board shows correct highlighted lane;
- highlight follows movement.

LEVEL 3
- correct lane pair is highlighted on the plus board;
- convex/reflex geometry does not highlight an unrelated adjacent lane;
- movement timing remains unchanged.

INTERACTION
- Super Zapper visually coexists sensibly;
- Pulsar overlay takes visual precedence where it intersects the selected lane;
- no color flicker;
- no rendering crash;
- no gameplay regression.

Ask Neal whether the highlighted rails provide the remembered arcade benefit of allowing attention deeper in the tube while maintaining immediate lane awareness.

Do not require later palette-cycle testing.

18. END-OF-RUN REPORT

At the end report only:

- files changed;
- exact lane/boundary selection rule;
- early-level color policy;
- precedence implementation;
- approach behavior;
- state-visibility behavior;
- automated tests added/changed;
- focused results;
- broader regression results;
- full solution result;
- build result;
- git diff --check;
- protected-scope result;
- Reference/incitti status;
- whether any existing gameplay defect was exposed;
- tracked documentation status;
- future palette boundary;
- deferred-roadmap confirmation;
- exact manual command/checklist;
- blocker, if any.

Do not declare this milestone complete until Neal performs manual graphical acceptance.

Do not commit.
Do not push.

TEMPEST-BLASTER-LANE-HIGHLIGHT-IMPLEMENTATION-COMPLETE-E4186A