IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task explicitly authorizes them or they are directly necessary source/report files within the bounded task below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is not project evidence unless Neal explicitly authorizes it in the current user task.


TASK

Correct only the practical manual-calibration range of the existing shared portrait-perspective experiment.

The mathematical perspective mechanism itself has already been implemented and validated.

Do NOT redesign, replace, or extend the projection model.

The existing permitted range of -5 through +5 is mechanically safe but practically inadequate for human visual calibration:

- +2 moves the normal far anchor only approximately 2.82 host pixels;
- +5 moves the normal far anchor only approximately 7.05 host pixels;
- Neal found the visual comparison effectively indistinguishable and unsuitable for deciding whether this is the correct direction of perspective correction.

The purpose of this continuation is to:

1. analytically derive a substantially larger but still safe signed runtime range;
2. identify a very small set of unmistakably different manual probe values;
3. update the parser range and directly affected tests;
4. preserve every already validated invariant;
5. leave the default at neutral 0;
6. make no other presentation or gameplay change.

This remains an experimental calibration mechanism.

Do not stage, commit, or push.


CURRENT AUTHORITATIVE STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains nine modified tracked files from the active portrait and perspective candidate:

- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/Projection.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DebugControllerTests.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs
- tests/Tempest.Game.Tests/GameplayRecordingTests.cs
- tests/Tempest.Game.Tests/TransitionPresentationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

Do not restore, reset, clean, discard, overwrite, stage, commit, or push the candidate.


STARTING SAFETY CHECK

Before implementation, run bounded checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected:

- master;
- HEAD and origin/master at 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- exactly the nine intentional tracked files modified;
- no relevant non-ignored untracked files.

If the repository differs materially, STOP before implementation and report the discrepancy.

Do not alter anything to force the expected state.


AUTHORITATIVE REPORT

Read:

AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md

That report is authoritative for the existing formula, implementation, validation, argument semantics, and current Git state.

The existing formula is:

q = 1 / (z + ZOffset)

qNear = 1 / (nearDepth + ZOffset)

xProjected = 400 + x * K * q

yProjected = CenterY + (y + YOffset) * K * q
             + a * K * (q - qNear)

where a is the signed runtime coefficient.

Preserve this formula exactly unless a source-level defect is discovered.

Do not alter the established sign convention:

- positive moves farther points upward and strengthens the candidate looking-down presentation;
- negative moves farther points downward and weakens it;
- zero reproduces the pre-perspective portrait candidate.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, generate, or analyze:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- gameplay recordings;
- contact sheets;
- comparison images;
- active-editor images.

Do not inspect Gemini material, Incitti source, web sources, or raw ROM files.

This task is based on the completed report, current source, and bounded mathematical analysis.


PRESERVE THE VALIDATED MECHANISM

Do not alter:

- the formula;
- the near-rim-zero anchoring rule;
- horizontal projection;
- reciprocal-depth affine structure;
- projective-line coherence;
- shared tube/entity projection path;
- runtime-only calibration semantics;
- recording schema;
- gameplay state;
- simulation timing;
- randomness;
- transition timing;
- K;
- ZOffset;
- LevelDefinition points;
- CenterY;
- YOffset;
- playfield scale;
- shared vertical translation;
- viewport;
- host-window dimensions;
- HUD camera;
- legacy presentation.

The only authorized production behavior change is expanding the accepted numeric range of --portrait-perspective if the bounded analysis proves that expansion safe.


HUMAN-VISIBLE CALIBRATION REQUIREMENT

The next manual comparison must be visually meaningful to a human observer at normal game size.

Do not repeat a calibration handoff where the strongest probe changes the relevant far-depth geometry by only a few pixels.

The manual probe set must include:

- neutral 0;
- one clearly visible moderate positive probe;
- one unmistakably visible strong positive probe;
- optionally one clearly visible negative probe if safe and diagnostically useful.

The moderate and strong probes should differ enough from neutral that Neal can determine within seconds whether the mechanism moves the presentation in the correct direction.

Do not require:

- pixel counting;
- transparent overlays;
- screenshot subtraction;
- zoomed inspection;
- measuring tools;
- prolonged staring at the small far region.

The purpose is first to judge the kind and direction of the perspective change, not to fine-tune a final coefficient.


PRE-IMPLEMENTATION RANGE ANALYSIS

Before changing source or tests, analytically determine the safe coefficient range using the current formula and actual production geometry.

Evaluate production Levels 1-9 across all directly relevant depths and rendering contexts, including:

- near rim;
- far rim;
- representative intermediate depths;
- ordinary enemy/player/projectile depths;
- enemy retraction or other directly relevant extended production depths;
- approach depths;
- established exit/reverse rendering contexts;
- any other existing depth range that currently reaches the shared projection.

For candidate signed coefficients, calculate:

- maximum far/intermediate host-space vertical displacement;
- minimum and maximum final Y;
- preserved near-rim positions;
- reserved upper-region intrusion;
- viewport clipping;
- finite denominator behavior;
- line-incidence preservation;
- implications for approach and exit contexts.

Do not use the existing -5 through +5 range as an assumed safety ceiling.

It was selected conservatively and is now known to be operationally inadequate.

Derive the real practical boundary from the current source and geometry.


VISUAL DISPLACEMENT TARGETS

Use host-pixel displacement only as a human-calibration design measure, not as an arcade specification.

Analyze several bounded target magnitudes for the normal far depth, such as approximately:

- a clearly visible moderate displacement;
- an unmistakable strong displacement;
- a stronger stress-boundary displacement used only for safety analysis.

Do not blindly hard-code predetermined coefficients from this prompt.

Calculate the coefficient needed to produce each displacement under the existing formula.

The final manual probes should be based on safe, rounded, understandable coefficient values near the analytically useful targets.

Do not describe the pixel displacement or coefficient as arcade-derived.


RESERVED UPPER PRESENTATION REGION

The perspective-envelope investigation established that the arcade gameplay composition reserves a stable upper score/status/lives region.

The current implementation report also notes that Level 6 has approximately 0.14 host pixels of top clearance, although the current perspective term leaves the near-rim extrema unchanged.

For the expanded range:

- verify that the shifted far/intermediate geometry does not intrude into the protected upper presentation region in a way that invalidates the current composition;
- distinguish tube-wide near-rim extrema from far/convergence-region movement;
- report the minimum Y reached at every proposed probe;
- reject any probe that produces clipping or clearly consumes the intended upper presentation territory.

Do not move the entire playfield down to make a larger perspective coefficient fit.

Do not alter the accepted -33.1 shared vertical placement.


PROJECTIVE-LINE COHERENCE

The existing reciprocal-depth-affine formula was proven to preserve straight tube rails and intermediate anchor incidence.

Confirm that enlarging the coefficient range does not invalidate that proof numerically.

Tests must continue to verify representative intermediate points on:

- closed Level 1;
- closed Level 6;
- open Level 9;

for:

- neutral;
- moderate positive probe;
- strong positive probe;
- negative probe if retained;
- accepted range boundaries.

Sharing a projection helper is not enough.

The measured final points must remain collinear within a narrow tolerance appropriate for the enlarged numeric range.


TUBE/ENTITY REGISTRATION

Retain and extend the current intermediate-depth alignment coverage.

At the enlarged probes and accepted boundaries, representative tube-relative gameplay anchors must remain registered with equivalent lane/tube-space anchors.

Do not independently move:

- Blaster;
- enemies;
- Spikers;
- shots;
- Spikes;
- effects;
- score popups.

If the enlarged range exposes meaningful floating-point divergence, reduce the safe range rather than adding entity-specific corrections.


PARSER AND OPTION SEMANTICS

Preserve:

--portrait-perspective <signed-number>

Preserve:

- invariant-culture parsing;
- finite-value requirement;
- missing/malformed/NaN/infinity rejection;
- duplicate-option rejection;
- conflict with --legacy-presentation;
- composability with existing supported level/lives/speed/rim-speed/recording/playback/visible-playback modes;
- rejection in headless analysis and standalone input diagnostic where no portrait playfield is rendered;
- runtime-only behavior;
- no recording metadata/schema change.

Update only:

- the conservative allowed range;
- range error text;
- directly affected parser tests;
- probe/boundary tests.

Keep default at 0.

Do not select a non-zero default in this task.


FAIL-CLOSED CONDITIONS

STOP rather than broaden the milestone if a human-visible range requires:

- changing the projection formula;
- moving the near rim;
- moving the full playfield;
- changing scale;
- changing -33.1;
- changing K or ZOffset;
- per-level coefficients;
- open/closed special cases;
- separate tube/entity transforms;
- bending rails;
- entity-specific correction;
- changing gameplay/model geometry;
- changing transitions;
- redesigning the presentation envelope;
- final HUD work.

If the existing mechanism cannot produce an unmistakable safe comparison before violating the upper presentation region or another invariant, report that finding explicitly and classify the mechanism as visually inadequate rather than modifying unrelated systems.


TEST REQUIREMENTS

At minimum, update or add focused coverage for:

1. New parser minimum/maximum.
2. Rejection just outside the new range.
3. Neutral 0 equivalence.
4. Near-rim invariance at probes and boundaries.
5. X-coordinate invariance.
6. Meaningful far-depth displacement at moderate and strong probes.
7. Level 1-9 viewport bounds at:
   - 0;
   - moderate positive;
   - strong positive;
   - retained negative probe;
   - minimum allowed value;
   - maximum allowed value.
8. Protected upper-region behavior.
9. Projective-line incidence at intermediate depths.
10. Tube/entity registration at intermediate depths.
11. Approach/transition finite behavior.
12. Legacy conflict.
13. Command-line composability.
14. Simulation/recording neutrality.

Do not assert arcade visual acceptance.

Do not make a chosen probe a gameplay constant or ROM fact.


IMPLEMENTATION SCOPE

Expected files changed should be limited to the files already participating in:

- parser/range definition;
- debug/perspective option propagation;
- presentation/projection tests;
- parser/options tests;
- recording/playback composability tests;
- transition tests directly required by the enlarged range.

Do not add a second formula or alternate calibration subsystem.

Do not modify durable behavior documentation.


BOUNDED VALIDATION

Run focused tests covering the changed range and directly affected presentation/parser/transition/recording behavior with an explicit VSTest hang timeout of no more than 3 minutes.

If focused validation passes, run:

1. full test suite with explicit 3-minute hang timeout;
2. Release build;
3. git diff --check.

Do not repeatedly rerun unchanged failing commands.

Do not use stale binaries after a failed build.

If Windows security, locked executable, timeout, hang, or stall blocks validation, follow the project's established self-stop policy.

Do not report incomplete validation as passed.


MANUAL ACCEPTANCE HANDOFF

Do not launch the interactive game.

After validation, provide a minimal Level 9 comparison set:

1. neutral 0;
2. moderate positive probe;
3. unmistakable strong positive probe;
4. optional negative probe if retained;
5. legacy presentation.

Use the actual safe values derived during this task.

Also provide:

- one enclosed representative level command;
- one open representative level command;
- one command combining the perspective probe with lives and speed;
- one playback-file example;
- the reusable Levels 1-8 command pattern.

Do not provide a long parameter sweep.

The strong probe must be visually unmistakable under normal viewing if the mechanism is capable of producing such a safe change.


REPORT

Preserve the substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md

The report must include:

- starting Git state;
- files inspected;
- files changed;
- confirmation the existing formula was preserved;
- analytical coefficient-to-host-pixel relationship;
- all depth ranges evaluated;
- actual limiting geometry/context;
- old allowed range;
- new allowed range;
- reason for the new boundaries;
- moderate and strong probe values;
- expected normal-far-depth displacement for each probe;
- negative probe if retained;
- upper-region results;
- Levels 1-9 bounds;
- approach/exit finite behavior;
- line-incidence results;
- tube/entity registration results;
- parser/composability results;
- simulation-neutrality results;
- focused tests;
- full suite;
- Release build;
- git diff --check;
- manual commands;
- unresolved visual questions;
- final Git state;
- confirmation nothing was staged, committed, or pushed;
- confirmation no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.


FINAL RESPONSE

Report:

1. Whether range expansion completed.
2. Starting/final Git state.
3. Files changed.
4. Confirmation the perspective formula was unchanged.
5. Old range.
6. New range.
7. Actual limiting safety condition.
8. Coefficient-to-pixel relationship.
9. Neutral, moderate, strong, and optional negative probes.
10. Expected visible displacement for each.
11. Levels 1-9 bounds.
12. Upper-region results.
13. Projective-line coherence results.
14. Tube/entity registration results.
15. Transition/approach behavior.
16. Parser and composability results.
17. Simulation neutrality.
18. Focused test result.
19. Full-suite result.
20. Release build result.
21. git diff --check result.
22. Exact Level 9 commands.
23. Representative enclosed/open commands.
24. Lives/speed composition command.
25. Playback example.
26. Whether the strong probe should be unmistakable to a human observer.
27. Any blocked or partial work.
28. Report path.
29. Confirmation nothing was staged, committed, or pushed.
30. Confirmation no visual assets or prohibited reference material were inspected.

Stop after automated validation and hand the expanded calibration range to Neal.

TEMPEST_PRESENTATION_PERSPECTIVE_RANGE_EXPANSION_20261114