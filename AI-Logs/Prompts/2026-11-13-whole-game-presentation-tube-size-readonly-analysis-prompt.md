IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


TASK

Perform a tightly bounded READ-ONLY investigation of whole-game tube SIZE / PROMINENCE in the current experimental 3:4 portrait presentation.

Do not implement any change.

Neal has manually evaluated the current Level 9 candidate and considers the new shared vertical placement a very good step in the right direction and positioned where it should be.

For this milestone:

FREEZE the current vertical placement.

Investigate tube size/prominence only.

Perspective/depth appearance looking down the tube is known to need later work but is explicitly DEFERRED from this investigation.

The goal is to determine the smallest safe shared presentation-layer size adjustment that can make the tube appropriately larger/more prominent without changing the accepted vertical placement, altering perspective, changing gameplay geometry, or introducing per-level calibration.


CURRENT AUTHORITATIVE STATE

Repository:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains the four tracked experimental presentation files:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

Do not restore, reset, clean, discard, stage, commit, or push the current candidate.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-experiment-reconstruction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md

For image-dependent conclusions, the label-correction report supersedes the original investigation report.

The final shared-vertical-placement correction report is authoritative for the current candidate state.


CURRENT MANUAL FINDING

Neal manually compared the current Level 9 experimental presentation against the previous presentation.

Manual finding:

- the current vertical placement is a very good step in the right direction;
- the vertical placement is where it should be;
- freeze that placement for now;
- the actual tube size still needs improvement;
- the perspective looking down the tube also needs improvement, but perspective is NOT part of this milestone.

Do not reinterpret "tube size" as permission to change perspective.


CURRENT PRESENTATION VALUES TO PRESERVE

Unless this read-only investigation proves that a value participates mathematically in size measurement, treat the current presentation as frozen:

- host window: 1280 x 960;
- portrait logical presentation: 600 x 800;
- centered portrait viewport: 720 x 960;
- output scale: 1.2;
- playfield scale: 1.05;
- shared vertical translation: -33.1;
- K = 70;
- ZOffset = 5;
- horizontal center/placement unchanged;
- shared playfield camera unchanged;
- HUD/screen-space camera unchanged;
- --legacy-presentation unchanged.

Do not alter any of these during this read-only task.


STARTING SAFETY CHECK

Before analysis, run bounded read-only checks of:

- branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected:

- branch master;
- HEAD/origin at 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- exactly the four intentional tracked candidate files modified;
- no relevant untracked files.

If the state differs materially, STOP and report the discrepancy.

Do not modify the repository to make it match expectations.


STRICT READ-ONLY SCOPE

Do not modify source, tests, tracked documentation, project configuration, or current candidate files.

The only files this task may create are:

- the preserved prompt under AI-Logs/Prompts;
- one new ignored analysis report under AI-Logs/Reports.

Do not run the interactive game.

Do not implement a size adjustment.

Do not change tests.

Do not tune presentation constants.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, extract, generate, or analyze screenshots, images, videos, gameplay recordings, MAME frames, C# frames, contact sheets, or other visual assets.

The prior authorized visual investigation and Neal's manual judgment provide the calibration context for this task.

Do not use Gemini prompts, reports, chat exports, formulas, constants, or discarded implementation work.


PRIMARY QUESTION

Determine how tube SIZE / PROMINENCE is controlled in the current experimental portrait presentation and recommend the smallest shared presentation-layer adjustment suitable for a subsequent implementation experiment.

Specifically determine:

1. Which existing value or combination of values controls the apparent size of the complete playfield.

2. Whether changing the current shared playfield scale alone would:
   - preserve tube shape;
   - preserve the currently approved shared vertical placement semantically;
   - preserve perspective/depth relationships;
   - preserve tube/entity registration;
   - preserve horizontal centering;
   - avoid gameplay/model changes.

3. Whether separate horizontal/vertical scaling would inherently alter tube proportions and should therefore be deferred until perspective analysis.

4. Whether a uniform scale around the current shared playfield anchor is the cleanest size-only experiment.

Do not assume the answer in advance.

Trace the actual source.


LEVEL 1-9 SIZE AND HEADROOM ANALYSIS

For production Levels 1 through 9, calculate the current tube bounds under the current candidate.

Report for every level:

- host X min/max;
- host Y min/max;
- tube width;
- tube height;
- width as percentage of the 720-pixel portrait viewport;
- height as percentage of the 960-pixel portrait viewport;
- left clearance;
- right clearance;
- top clearance;
- bottom clearance.

Then analytically evaluate the maximum uniform shared scale that could be applied about the CURRENT shared playfield anchor before any Level 1-9 tube first intersects a portrait viewport boundary.

Identify:

- which level and edge become limiting horizontally;
- which level and edge become limiting vertically;
- the exact mathematical scale ceiling;
- whether the practical size increase is limited primarily by width or height.

Do not change the vertical translation to make a larger scale fit.

That is deliberately prohibited in this milestone because Neal has manually accepted the current vertical placement.


LEVEL 9 CALIBRATION CONTEXT

The current report states that Level 9 occupies approximately:

641.46 host pixels of the 720-pixel portrait viewport width,
or approximately 89.09%.

Use the current source calculations to verify that value.

Do not assume that 89.09% is automatically too small or prescribe a target percentage without evidence.

Instead, calculate several small uniform-scale hypotheses analytically, such as approximately:

- +2%;
- +4%;
- +6%;
- and the maximum safe shared uniform scale supported by Levels 1-9.

These are analytical probes only.

For each candidate, report:

- resulting Level 9 width and viewport percentage;
- resulting Level 9 height and viewport percentage;
- worst Levels 1-9 top/bottom/left/right clearance;
- first clipping level/edge, if any.

Do not select a value merely because it fills the viewport.


PRESERVE VERTICAL PLACEMENT

Neal's manual finding that vertical placement is correct is authoritative for this stage.

A size-only experiment must scale around an anchor/reference that does not arbitrarily translate the whole tube upward or downward.

Analyze the current playfield camera anchor/target/offset and explain:

- what point uniform scaling is centered around;
- whether increasing scale would make the Level 9 pit/convergence point move vertically;
- whether the approved apparent placement would remain stable;
- if not, whether another existing shared anchor can preserve placement without introducing a second calibration variable.

This is analysis only.

Do not invent an anchor.

Do not implement anchor changes.


PERSPECTIVE IS DEFERRED

Do not change or recommend implementation of:

- K;
- ZOffset;
- projection depth formula;
- depth-dependent translation;
- per-level camera values;
- Y-axis normalization;
- CenterY;
- YOffset;
- tube vertex data;
- different scaling for near and far geometry.

You may explain whether uniform size scaling leaves current perspective unchanged.

Do not attempt to improve perspective in this task.

Perspective looking down the tube is the next separate visual-calibration problem after size.


GAMEPLAY AND MODEL GEOMETRY ARE FROZEN

Do not modify or recommend modifying:

- LevelDefinition.Points;
- lanes;
- tube connectivity;
- collision geometry;
- enemy paths;
- Blaster paths;
- projectile paths;
- Spike geometry;
- mechanics;
- timing;
- simulation;
- score;
- progression.

The desired size correction must remain purely presentational.


TUBE / ENTITY REGISTRATION

Inspect the current shared-camera path only enough to determine whether a uniform playfield scale would continue to affect:

- tube;
- Blaster;
- enemies;
- Spikers;
- player shots;
- enemy shots;
- Spikes;
- gameplay-space effects;
- score popups;
- transitions/world-space elements;
- applicable Game Over vectors

as one coherent playfield.

State whether uniform size scaling at the existing shared camera can preserve their relative registration automatically.

No new implementation is authorized.


COMMAND-LINE COMPOSABILITY REQUIREMENT

This investigation must analyze the existing command-line option architecture because any subsequent experimental size option should, if practical, compose normally with the project's existing development/test options.

Do NOT add a command-line option in this read-only task.

Identify the actual currently supported relevant options from Program.cs and related argument handling.

For a future size experiment, the desired behavior is:

- a presentation-size experimental option should not unnecessarily force a particular level;
- it should be combinable with --level N;
- it should be combinable with the existing lives option, if one exists;
- it should be combinable with the existing speed option, if one exists;
- it should be combinable with recording options, if supported;
- it should be combinable with playback options / --playback-file, if supported;
- it should continue to respect --legacy-presentation or explicitly reject an ambiguous combination.

Do not invent option names.

Inspect the actual parser and report the exact existing switches relevant to these workflows.

For each combination, classify:

- SAFE TO COMPOSE;
- SEMANTICALLY CONFLICTING;
- UNSUPPORTED BY CURRENT PARSER;
- REQUIRES EXPLICIT PRECEDENCE RULE.

If any combination would be unsafe or ambiguous, explain why.

The goal is that Neal should retain normal flexibility while testing presentation changes rather than being locked into one hard-coded launch scenario.


LEGACY PRESENTATION

The legacy presentation must remain a valid A/B reference.

Analyze whether a future size experimental option should:

A. affect portrait mode only while --legacy-presentation always remains unchanged;

or

B. be rejected when combined with --legacy-presentation.

Recommend the cleaner behavior based on current argument architecture.

Do not implement it.


NO SPECIAL TEST BUILD UNLESS NEEDED

Prefer avoiding a special compiled test build if the existing normal executable plus composable runtime arguments can provide the experiment safely.

If a special command-line presentation-size override would materially improve manual calibration, recommend its minimal semantics.

If the existing presentation configuration can be changed as an uncommitted candidate and tested with ordinary commands, say so instead.

Do not implement either approach during this analysis.


REFERENCE AUTHORITY

Do not search the web.

Do not consult external Tempest implementations.

Do not use Mark Incitti to obtain a scale constant.

Do not inspect Gemini material.

Do not inspect raw ROM files.

Use:

- current source;
- current candidate diff;
- authoritative presentation reports;
- Neal's current manual finding.

The size value remains a PROJECT CALIBRATION subject to Neal's manual acceptance.


BOUNDED ANALYSIS

Use only bounded source reads/searches and bounded numerical calculations.

Do not run the full test suite.

Do not run the interactive game.

A targeted pre-existing test may be run only if needed to establish an otherwise unavailable factual property, and must use an explicit hang timeout.

Prefer calculations and source inspection.


DIAGNOSTIC CLASSIFICATION

Finish with one of:

READY FOR UNIFORM SIZE EXPERIMENT

Use if one shared size control can be adjusted safely without changing vertical placement, perspective, or gameplay geometry.

SIZE REQUIRES ANCHOR DECISION

Use if uniform scaling necessarily moves the manually accepted vertical placement and an explicit shared-anchor decision is needed first.

SIZE AND PERSPECTIVE ARE COUPLED

Use only if the current architecture makes a meaningful size-only adjustment impossible without altering perspective.

INSUFFICIENT EVIDENCE

Use if source/calculation evidence cannot establish a bounded next experiment.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts according to project instructions.

Create:

AI-Logs/Reports/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md

Include:

- starting Git state;
- files inspected;
- confirmation that current candidate was unchanged;
- current presentation constants;
- current Level 1-9 bounds/size matrix;
- current Level 9 89.09% verification;
- shared scale control analysis;
- anchor behavior;
- +2%, +4%, +6% analytical probes;
- maximum safe shared uniform scale;
- limiting level/edge;
- effect on current perspective;
- tube/entity registration consequences;
- exact existing command-line options relevant to manual testing;
- command-line composability matrix;
- recommendation on size option versus ordinary uncommitted candidate;
- recommendation for --legacy-presentation interaction;
- final classification;
- smallest recommended next implementation experiment;
- final Git state;
- confirmation that nothing was staged/committed/pushed;
- confirmation that no images/videos/Gemini material were analyzed.


FINAL RESPONSE

Report:

1. Whether read-only analysis completed.
2. Starting/final Git state.
3. Confirmation that candidate files were unchanged.
4. Exact current size-control path.
5. Level 1-9 size/headroom summary.
6. Verified Level 9 width and percentage.
7. +2%, +4%, +6% analytical results.
8. Maximum safe shared uniform size increase.
9. Limiting level and viewport edge.
10. Whether scaling moves the manually approved vertical placement.
11. Whether uniform scaling changes perspective.
12. Whether tube/entity registration remains coherent.
13. Current relevant command-line switches found in source.
14. Which switches can compose with a future presentation-size experiment.
15. Recommended --legacy-presentation interaction.
16. Whether a special runtime size option is actually warranted.
17. Final diagnostic classification.
18. Smallest recommended next implementation step.
19. Report path.
20. Confirmation that nothing was staged, committed, or pushed.
21. Confirmation that no visual assets or Gemini material were analyzed.

Do not implement the recommendation.

TEMPEST_PRESENTATION_TUBE_SIZE_READONLY_ANALYSIS_20261113