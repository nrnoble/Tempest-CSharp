IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, attachment metadata, tagged-file information, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

TASK

Perform a bounded READ-ONLY investigation of the Tempest whole-game presentation architecture and recommend a presentation calibration toward the supplied MAME reference.

This investigation must make NO production-code, test, durable-documentation, configuration, Git, or reference-file changes.

Level 9 is the primary calibration case.

VISUAL REFERENCE — EXPLICITLY AUTHORIZED

Exactly one visual asset is authorized for this task:

TEMP\Comparision of screen demisions.png

This image contains the presentation comparison supplied by Neal:

- LEFT = MAME/reference presentation.
- RIGHT = current C# presentation.
- Goal = calibrate the C# presentation toward the MAME presentation.

Analyze this specific image as deeply as useful for the investigation.

Do NOT search for, enumerate, open, inspect, extract, generate, or analyze any other images, screenshots, contact sheets, recordings, videos, MAME frames, C# frames, or other visual assets.

Do not use broad image/video-extension searches.

If the supplied image is insufficient to answer a particular visual question, identify the missing evidence in the report. Do not search for substitute images.

STARTING SAFETY

First inspect, with bounded commands:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files.

Expected accepted baseline:

master

385f603a94391756b2ca6cccd524dc17b281e782

"Close out ROM-backed enemy-shot rendering"

origin/master should match the same accepted commit.

The tracked worktree and index are expected to be clean.

TEMP/ and other intentionally ignored user assets are not reasons to stop merely because they exist.

Do not broadly inspect TEMP/. Access only the specifically authorized:

TEMP\Comparision of screen demisions.png

Do not read TEMP/myNotes.md.

If the tracked repository state materially differs from the expected accepted baseline, STOP before investigating and report the discrepancy.

Do not reset, restore, clean, checkout, stash, stage, commit, push, rebase, merge, amend, or otherwise alter repository state.

CONTEXT AND FIDELITY GOAL

This is a presentation milestone, not gameplay reconstruction.

Neal's high-level understanding is that the original arcade game uses a vector-monitor presentation and therefore the Atari game software should not be assumed to define a conventional modern pixel resolution or desktop/window aspect ratio.

Treat conventional C# logical pixel dimensions, host-window dimensions, fullscreen behavior, and modern viewport/aspect handling as presentation concerns requiring project translation unless authoritative project/reference evidence establishes otherwise.

Do not invent a ROM-defined pixel resolution or monitor aspect ratio.

The project-local ROM/reference material may still be useful for logical vector/projection relationships if necessary, but do not conduct another broad ROM investigation merely to search for a conventional screen resolution.

If determining the physical/reference display aspect ratio requires evidence not present in the project or supplied comparison image, state that limitation and make any recommendation explicitly as PROJECT RECOMMENDATION rather than arcade/ROM fact.

CURRENT ACCEPTED FOUNDATION

The existing 1280x960 host window was previously manually accepted as much better for Neal's 1080p setup.

Do not assume that the accepted host-window dimensions imply that the current internal presentation, viewport proportions, tube projection, tube size, vertical placement, or HUD layout are arcade-faithful.

The goal now is to refine the presentation inside the available display/window architecture.

Do not discard the accepted 1280x960 work merely because other presentation values may need refinement.

INVESTIGATION GOALS

Determine how the present C# architecture controls and relates:

1. Host/window dimensions.
2. Internal/logical game-screen dimensions.
3. Viewport or presentation transform.
4. Aspect ratio handling.
5. Tube projection and perspective.
6. Tube apparent width and height.
7. Tube depth compression/expansion.
8. Tube scale.
9. Tube horizontal centering.
10. Tube vertical placement.
11. HUD score placement.
12. HUD lives placement.
13. Other presentation offsets directly relevant to matching the supplied comparison.
14. Scaling behavior if the host window changes size.
15. Current feasibility of fullscreen and/or borderless fullscreen presentation.

Clearly distinguish values that control GAMEPLAY GEOMETRY from values that control PRESENTATION.

GAMEPLAY GEOMETRY MUST REMAIN FROZEN

This future refinement must not alter actual gameplay geometry or mechanics.

The investigation must identify and protect, as applicable:

- logical tube lane structure;
- lane adjacency;
- collision geometry;
- enemy paths;
- player paths;
- lane-transition geometry used by gameplay;
- shot trajectories;
- projectile behavior;
- enemy movement;
- level configurations;
- spawn positions insofar as they represent gameplay state;
- score/gameplay rules;
- difficulty or timing.

A presentation transform may visually reposition or rescale rendered geometry without changing the underlying gameplay model.

If the existing architecture couples presentation coordinates to gameplay coordinates in a way that prevents safe separation, identify that explicitly rather than recommending silent gameplay changes.

LEVEL 9 PRIMARY CALIBRATION

Use Level 9 as the strongest primary calibration case.

Investigate why Level 9 makes the current presentation differences particularly visible and whether its geometry provides a strong basis for evaluating:

- overall viewport proportions;
- tube apparent dimensions;
- perspective/depth presentation;
- scale;
- vertical position;
- HUD relationship to the tube.

Do not introduce a Level-9-specific rendering correction merely because Level 9 is the calibration reference.

The desired outcome is a whole-game/global presentation model where practical.

REGRESSION REQUIREMENT

Any eventual candidate must be regression-checked across the previously accepted tube layouts.

This investigation should determine whether a global presentation transform can reasonably apply to Levels 1-9 without changing each tube's gameplay geometry.

Prefer shared presentation parameters over per-level visual patches unless evidence demonstrates that tube-specific presentation behavior is genuinely required.

Do not implement those changes in this task.

AUTHORIZED IMAGE ANALYSIS

Analyze only:

TEMP\Comparision of screen demisions.png

The analysis should compare the LEFT MAME/reference side with the RIGHT C# side and characterize, as objectively as the image permits:

- relative displayed aspect/proportions;
- tube apparent width;
- tube apparent height;
- tube depth/perspective appearance;
- tube center;
- vertical placement;
- surrounding margins/unused presentation space;
- relationship of tube to the visible game area;
- score placement;
- lives placement;
- HUD-to-tube relationship;
- any obvious global scaling or translation difference;
- any evidence suggesting that aspect, internal dimensions, projection, scale, vertical translation, or HUD coordinates are separate problems.

Measurements derived from the supplied image may be used.

Clearly label image-derived measurements as measurements of this supplied comparison image, not original arcade hardware specifications.

Do not infer exact physical arcade-monitor dimensions from screenshot pixels alone.

If useful, calculate normalized ratios instead of relying only on absolute screenshot pixels.

SOURCE/CODE ANALYSIS

Inspect only the repository code/reference material needed to understand the presentation pipeline.

Likely relevant areas may include, but are not limited to:

- display/window configuration;
- Program/startup window setup;
- Projection;
- VectorRenderer;
- presentation transforms;
- HUD rendering;
- level/tube rendering;
- Level 9 configuration;
- existing display/projection tests;
- historical durable documentation relevant to the accepted larger-window milestone.

Follow actual repository evidence rather than assuming filenames or architecture.

You may inspect the Mark Incitti reference implementation where directly useful because it has previously served as a strong engineering reference for presentation/sizing, but distinguish its choices from arcade/ROM facts and from Neal's manual arcade authority.

Do not use web searches. This should remain grounded in project-local evidence and the explicitly supplied comparison image.

Do not broadly reread unrelated gameplay systems.

HISTORICAL EVIDENCE

Identify relevant history around the accepted larger-window/display milestone and determine:

- what was intentionally changed;
- what was manually accepted;
- what remained deferred;
- which current presentation constants came from that work;
- which assumptions should remain protected.

Do not rewrite historical reports or documentation.

FULLSCREEN / BORDERLESS FULLSCREEN

Investigate, but do not implement, how fullscreen and/or borderless fullscreen-window presentation could fit the existing architecture.

The first objective remains correct internal presentation.

Recommend whether fullscreen should logically be:

- part of the eventual main presentation implementation;
- or a separate follow-on milestone after the internal presentation is calibrated.

The fullscreen recommendation must preserve aspect and presentation rather than simply stretching arbitrary content to fill a display.

Do not change window modes in this investigation.

EVIDENCE CLASSIFICATION

Use explicit classifications where appropriate:

- ARCADE/MAME-OBSERVED — only what is objectively supported by the specifically supplied MAME comparison.
- PROJECT CURRENT BEHAVIOR — what the current C# code does.
- REFERENCE-DERIVED — engineering information from another implementation such as Incitti.
- PROJECT TRANSLATION — existing intentional mapping from vector/logical concepts to modern rendering.
- PROJECT RECOMMENDATION — proposed future calibration.
- UNRESOLVED — evidence is currently insufficient.

Do not promote recommendations, screenshot measurements, or reference-implementation choices into ROM/arcade facts.

DELIVERABLE

Produce a read-only investigation report under:

AI-Logs/Reports/

Use a descriptive 2026-11-12 filename for the whole-game presentation/Level-9 calibration investigation.

Because AI-Logs are ignored working/historical artifacts, creating the investigation report is permitted.

Also preserve this substantial prompt under:

AI-Logs/Prompts/

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md during this read-only investigation.

The report should contain:

1. Starting Git state.
2. Investigation scope and evidence boundaries.
3. Exact visual asset analyzed.
4. Explicit confirmation that no other visual assets were searched for or analyzed.
5. Current C# presentation architecture.
6. Current host-window versus internal/logical presentation model.
7. Objective MAME-left versus C#-right comparison findings.
8. Level 9 calibration findings.
9. Tube projection/perspective findings.
10. Tube scale and placement findings.
11. HUD score/lives findings.
12. Gameplay-geometry versus presentation boundary.
13. Previous display/window milestone findings that remain accepted.
14. Recommended internal logical dimensions/aspect model, if evidence supports a recommendation.
15. Recommended global projection/presentation parameters or categories requiring adjustment.
16. Recommendation for HUD calibration.
17. Regression strategy across Levels 1-9.
18. Fullscreen/borderless-fullscreen architectural recommendation.
19. Explicit evidence classifications.
20. Unresolved questions.
21. A tightly bounded proposed implementation milestone.
22. Confirmation that no production code/tests/durable documentation were modified.

Do not implement the recommendations.

NO AUTOMATED GAMEPLAY VALIDATION REQUIRED

This is a read-only investigation.

Do not run the interactive game.

Do not run a full test suite merely for this investigation.

Small bounded diagnostic calculations or existing focused non-mutating checks may be used when necessary to understand projection/presentation mathematics.

Do not generate screenshots.

Do not modify files outside the ignored paired AI-Logs prompt/report.

SECURITY AND STALL POLICY

If Windows Application Control, Smart App Control, or similar security blocks an operation:

- do not weaken security;
- do not bypass security;
- do not repeatedly retry the same blocked artifact;
- do not substitute stale binaries.

If a required operation cannot complete, stop and report what remains.

All potentially long searches, analyses, or command loops must have explicit bounds.

If an operation makes no meaningful progress beyond its expected duration, stop rather than waiting indefinitely.

In particular, do not enter open-ended visual processing.

The one authorized comparison image may be analyzed as needed, but do not repeatedly reprocess it without a materially different and necessary question.

FINAL RESPONSE

At completion report:

1. whether the investigation completed;
2. principal differences between the supplied MAME and C# presentations;
3. current architectural cause(s) identified;
4. recommended internal screen/aspect approach;
5. recommended projection/scale/vertical-placement approach;
6. recommended HUD approach;
7. fullscreen/borderless recommendation;
8. what gameplay geometry will remain protected;
9. unresolved questions;
10. proposed next implementation milestone;
11. report path;
12. final Git/worktree status;
13. explicit confirmation that the only visual asset analyzed was TEMP\Comparision of screen demisions.png.

Do not commit or push.

TEMPEST_WHOLE_GAME_PRESENTATION_LEVEL9_READONLY_20261112