IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading files, running commands, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

Automatically appended VS Code/agent context after that token does not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

TASK

Perform a SHORT CORRECTIVE READ-ONLY continuation of the completed whole-game presentation / Level 9 calibration investigation.

The original investigation report is:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

A material user-provided labeling error has now been corrected.

CORRECT VISUAL PROVENANCE

For the already-authorized image:

TEMP\Comparision of screen demisions.png

the correct interpretation is:

LEFT = current C# presentation.

RIGHT = MAME/reference presentation.

GOAL = move the future C# presentation toward the RIGHT-side MAME reference.

The previous task incorrectly stated the opposite.

This correction is authoritative.

Do not continue using conclusions that depend on LEFT=MAME / RIGHT=C#.

SCOPE

This is correction and reconciliation only.

Do NOT:

- implement production changes;
- modify tests;
- modify durable project documentation;
- alter display configuration;
- alter projection;
- alter gameplay geometry;
- alter HUD;
- stage;
- commit;
- push;
- rerun the entire previous source investigation;
- repeat broad repository searches;
- inspect ROM files/disassembly;
- search the web;
- run the game;
- run the test suite.

Use the source/code findings already established by the prior investigation where they remain independent of the reversed image labels.

VISUAL-ASSET RULE

Exactly one visual asset remains authorized:

TEMP\Comparision of screen demisions.png

You may reanalyze that image as necessary because its left/right provenance was previously reversed.

Do not search for, enumerate, open, extract, generate, or analyze any other image, screenshot, video, recording, MAME frame, C# frame, contact sheet, or visual asset.

Do not perform broad searches for image/video extensions.

CORRECTIVE ANALYSIS

Reevaluate the prior report specifically in light of:

LEFT = C#
RIGHT = MAME target.

The previous report measured approximately:

LEFT panel:
~1278 x 959 black area
~1.33:1 / 4:3

RIGHT panel:
~773 x 1030 black area
~0.75:1 / 3:4

Recheck these measurements if necessary against the same authorized image.

If confirmed, correctly attribute them:

LEFT / ~4:3 = C#.

RIGHT / ~3:4 = MAME/reference target.

These remain measurements of the supplied comparison image, not arcade hardware specifications.

Reevaluate:

1. The aspect-ratio conclusion.
2. Whether retaining the current C# 800x600 4:3 logical canvas remains the best recommendation.
3. Whether a portrait-like internal presentation should instead be considered.
4. The relationship between the current 1280x960 C# host window and the desired MAME presentation.
5. Tube apparent width relative to each presentation area.
6. Tube apparent height.
7. Tube vertical placement.
8. Empty space above and below the tube.
9. Horizontal centering.
10. Tube perspective/depth appearance.
11. HUD score placement.
12. HUD lives placement.
13. Level placement.
14. Whether the earlier apparent HUD/source discrepancy disappears once the panels are correctly identified.
15. Which previous recommendations remain valid despite the reversal.
16. Which previous recommendations must be withdrawn or replaced.

LEVEL 9

Continue to treat Level 9 as the strongest calibration fixture.

Do NOT introduce Level-9-specific presentation constants.

The desired future model remains a whole-game presentation model that can regression-check Levels 1-9 while preserving gameplay geometry.

GAMEPLAY GEOMETRY REMAINS FROZEN

Do not recommend changing LevelDefinition.Points, lane topology, collisions, enemy/player paths, projectile behavior, level mechanics, timing, or other gameplay geometry merely to match the MAME presentation.

Strongly prefer presentation-layer solutions.

If moving from the current C# 4:3 presentation toward the portrait-like MAME presentation requires reconsidering the logical presentation canvas, explain how that could be separated safely from gameplay geometry.

Do not implement it.

IMPORTANT ASPECT-RATIO DISCIPLINE

Do not infer that the arcade ROM defines a conventional pixel resolution.

Do not treat the supplied screenshot dimensions as physical arcade-monitor measurements.

However, because the user explicitly identifies the RIGHT-side MAME presentation as the visual target, its observed viewport proportions are relevant evidence for choosing a future C# presentation translation.

Clearly distinguish:

- image-observed target proportions;
- current C# logical dimensions;
- physical arcade-monitor facts, if unknown;
- proposed C# presentation translation.

Do not preserve 800x600 merely because the current project and Incitti use it if the corrected visual evidence indicates that another presentation aspect better matches the user's target.

Likewise, do not abandon 800x600 automatically merely because the supplied MAME pane appears portrait-like.

Evaluate the evidence first.

FULLSCREEN

Reevaluate the earlier fullscreen/borderless recommendation only if the corrected aspect result materially changes it.

Fullscreen still should not be implemented in this task.

A future fullscreen implementation should preserve the ultimately accepted internal presentation aspect rather than stretching it arbitrarily.

REPORT CORRECTION

Do not silently rewrite history as though the original report had never used the reversed labels.

Create a new ignored corrective report under:

AI-Logs/Reports/

Use a descriptive 2026-11-12 filename such as:

2026-11-12-whole-game-presentation-level9-label-correction-report.md

Preserve this corrective prompt under:

AI-Logs/Prompts/

The corrective report should clearly state:

- the original user instruction accidentally reversed C# and MAME;
- Neal corrected the provenance;
- LEFT is C#;
- RIGHT is MAME/reference;
- MAME/right is the target;
- which original measurements remain usable;
- which original interpretations are invalid;
- which source/code findings remain valid;
- corrected aspect-ratio interpretation;
- corrected tube scale/placement interpretation;
- corrected HUD interpretation;
- corrected recommendation for internal presentation dimensions/aspect;
- corrected recommendation for the next implementation/calibration step;
- any remaining unresolved questions.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md yet.

NEXT-STEP DECISION

At the end, determine whether the corrected evidence is now sufficient to design a bounded first implementation candidate.

If sufficient, recommend the smallest safe first presentation candidate.

If insufficient, identify exactly what additional evidence Neal should provide.

Do not independently search for that evidence.

FINAL RESPONSE

Report:

1. Whether the correction completed successfully.
2. Corrected C#-versus-MAME aspect interpretation.
3. Corrected tube scale/placement interpretation.
4. Corrected HUD interpretation.
5. Whether 800x600 should still be retained, reconsidered, or remains unresolved.
6. What should happen to the accepted 1280x960 window.
7. What prior conclusions remain valid.
8. What prior conclusions are superseded.
9. Whether evidence is sufficient for implementation.
10. Recommended next step.
11. Corrective report path.
12. Final Git state.
13. Confirmation that only TEMP\Comparision of screen demisions.png was visually analyzed.

Do not make production changes.
Do not commit or push.

TEMPEST_PRESENTATION_LEVEL9_LABEL_CORRECTION_20261112
