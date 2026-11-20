Proceed with the recommended bounded perspective-fidelity experiment from:

AI-Logs/Reports/2026-11-02-between-level-approach-perspective-fidelity-analysis-report.md

Current accepted checkpoint:
15933a8 Refine accepted Blaster death animation

IMPORTANT SCOPE

The existing between-level approach is already accepted and good.

This is a small visual experiment intended to determine whether the approach can more convincingly resemble Neal's observation of Atari Tempest.

Do not redesign LevelApproach or the projection system.

The pre-existing .gitignore modification adding Temp/ is intentional but unrelated to this milestone. Leave .gitignore untouched and do not include it in this implementation.

IMPLEMENTATION

Implement the analysis report's recommended approach:

Add a subtle, deterministic, render-only vertical/parallax bias during LevelApproach.

Requirements:

1. The vertical bias must be active only during the between-level approach presentation.

2. The bias should be strongest when the next tube is distant and should smoothly diminish as the player/viewpoint approaches the tube.

3. The bias must reach exactly zero at arrival so normal gameplay uses the established normal projection with no residual offset.

4. Preserve the existing accepted depth approach:
   - 120-tick lifecycle;
   - existing smoothstep depth progression;
   - existing 240-unit starting depth offset;
   - existing tube growth behavior.

5. Keep the Blaster visually attached to the approaching rim.

6. Preserve spinner-controlled Blaster movement during approach.

7. Preserve existing gameplay freeze/input restrictions during approach.

8. Preserve natural level progression and arrival behavior.

9. Do not alter the accepted 1280x960 display configuration or general presentation scaling.

10. Do not change Projection.K, Projection.ZOffset, or normal gameplay projection solely to accomplish this effect.

11. Do not introduce a 3D camera system, generalized camera architecture, or unrelated rendering abstraction.

12. The purpose is perceptual:
    make the presentation feel somewhat more like the player/viewpoint is traveling toward the tube from an elevated approach rather than simply watching the tube grow along one fixed projection axis.

The exact vertical-bias magnitude is a PROJECT DECISION for this visual trial. Do not describe it as a measured Atari camera angle.

IMPLEMENTATION QUALITY

Prefer:
- one small centralized approach-only presentation calculation;
- deterministic behavior;
- smooth interpolation;
- no duplication of projection logic;
- no effect on normal gameplay rendering.

Avoid:
- magic adjustments scattered throughout VectorRenderer;
- special cases for individual levels unless geometry proves that absolutely necessary;
- changes to gameplay coordinates merely to move rendered objects.

TESTS

Add or adjust focused tests protecting objective invariants:

- bias exists during the appropriate part of LevelApproach;
- bias changes deterministically with approach progress/depth;
- bias smoothly trends toward normal presentation;
- bias is exactly zero at arrival;
- projected geometry remains finite;
- Blaster remains attached to the corresponding rim geometry;
- spinner movement remains valid;
- LevelApproach lifecycle remains 120 ticks;
- normal projection is completely restored at transition to gameplay;
- natural progression remains unchanged.

Do not add tests claiming that a particular bias value is "arcade accurate."

VALIDATION

Run:
1. focused LevelApproach tests;
2. affected progression/rendering tests;
3. full solution tests;
4. solution build;
5. git diff --check.

Review the tracked diff carefully.

Do not stage.
Do not commit.
Do not modify .gitignore.
Do not investigate Temp/.

MANUAL COMPARISON

After automated validation passes, stop for manual testing.

Provide the normal run command.

Neal will judge whether the refined presentation is actually better than the previously accepted approach.

Manual acceptance should specifically consider:

1. Does the distant tube now appear to be approached from somewhat above?
2. Does the presentation give a stronger impression that the viewpoint/player is traveling toward the tube?
3. Does the tube still grow smoothly and naturally?
4. Does the Blaster remain convincingly attached to the rim?
5. Does spinner movement still look natural during approach?
6. Is the transition into normal gameplay completely smooth?
7. Is there any arrival snap or vertical jump?
8. Any projection flash?
9. Any momentary wrong board?
10. Any new Blaster jump?
11. Does the refinement look clearly better than the previously accepted version?

If the answer to #11 is no, do not attempt increasingly complicated corrections. Report that retaining the existing accepted presentation is preferable.

Do not update the durable arcade-behavior documentation as though the experimental bias has been accepted until Neal manually evaluates it.

Save the implementation prompt/report under AI-Logs using the established project convention.

At completion report:
- exact implementation;
- chosen bias magnitude/curve;
- files changed;
- focused-test results;
- full-suite result;
- build result;
- git diff --check;
- worktree status;
- manual run command;
- comparison checklist.

COMPLETION TOKEN:
TEMPEST_APPROACH_PERSPECTIVE_FIDELITY_EXPERIMENT_COMPLETE
