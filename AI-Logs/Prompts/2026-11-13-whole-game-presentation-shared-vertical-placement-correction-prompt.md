IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


TASK

Implement exactly one narrowly bounded correction to the current PARTIAL whole-game portrait-presentation candidate:

Replace the Level-9-derived fixed vertical placement with one shared all-level-safe vertical placement derived from the union of production Levels 1-9 tube bounds.

Also correct the two invalid or overprecise test assumptions identified by the completed read-only diagnostic:

1. Levels 8 and 9 are open 16-vertex tubes with 15 playable lanes and therefore produce 46 tube lines, not 48.
2. The HUD bottom-corner result of approximately 960.00006 versus 960 is ordinary floating-point rounding and should be tested with an appropriately narrow tolerance rather than exact equality.

Do not perform any additional visual calibration.

Do not change tube scale, horizontal placement, viewport dimensions, projection constants, gameplay geometry, HUD design, or level-specific presentation behavior.

This remains an EXPERIMENTAL PRESENTATION CANDIDATE.

Do not stage, commit, or push.


CURRENT AUTHORITATIVE STATE

The accepted repository baseline remains:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The worktree intentionally contains exactly four tracked modified files from the current portrait candidate:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

These four modified files are current authorized candidate work.

Do not restore, reset, discard, overwrite, clean, or otherwise remove them.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-experiment-reconstruction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md

For image-dependent conclusions, the label-correction report supersedes the original 2026-11-12 investigation report.

The cross-level clipping diagnostic is authoritative for this correction.

It established:

- the current shared camera architecture is sound for the measured ordinary playfield;
- the portrait transform preserves orientation and point order;
- no horizontal or bottom overflow exists across Levels 1-9;
- top overflow exists on Levels 1, 3, 5, 6, 7, and 8;
- the maximum measured top overflow is approximately 53.74 host pixels on Level 6;
- this is approximately 44.78 portrait-internal units at the current 1.2 output scale;
- the union of all current portrait-host tube bounds is approximately:
  x = [288.80, 991.20]
  y = [-53.74, 621.74];
- a single downward shared translation can remove all measured top clipping while leaving ample bottom space;
- changing scale is not required;
- K and ZOffset are not implicated as needing correction;
- per-level offsets are unsupported;
- Levels 8 and 9 correctly draw 46 tube lines because their open topology has 15 playable lanes;
- the HUD 960.00006 result is harmless floating-point rounding.

Treat the numerical shift as a feasibility bound, not an arcade-derived constant or accepted final visual calibration.


STARTING SAFETY CHECK

Before implementation, run bounded checks of:

- current branch;
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
- exactly the four intentional tracked candidate files modified;
- no relevant non-ignored untracked files.

If the repository differs materially from that state, STOP before implementation and report the discrepancy.

Do not modify anything to force the expected state.


GEMINI MATERIAL IS NOT PROJECT AUTHORITY

Gemini prompts, reports, chat exports, and discarded Gemini-generated implementation work were removed from the active project.

Do not search for, locate, read, restore, reconstruct, or use any Gemini material.

Do not use formulas, constants, architectural decisions, or implementation ideas from the discarded Gemini experiment.

This implementation must be based only on the current candidate, current project source, and the four authoritative reports named above.


STRICT IMPLEMENTATION SCOPE

Modify only what is necessary in the existing four-file candidate.

Expected primary targets:

- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

Modify Program.cs or VectorRenderer.cs only if a direct and unavoidable correction is required to preserve the existing centralized presentation configuration.

Do not broaden the candidate.

Do not create a second presentation-configuration system.

Do not add unrelated rendering abstractions.


ONE SHARED ALL-LEVEL-SAFE VERTICAL PLACEMENT

Replace the current Level-9-derived fixed portrait vertical translation with one shared vertical placement derived from the complete production Levels 1-9 tube envelope.

Requirements:

- Use one global portrait playfield vertical placement.
- Preserve the current shared playfield camera.
- Preserve the current portrait viewport and output fitting.
- Preserve existing gameplay/model coordinates.
- Preserve every level's production CenterY and YOffset.
- Preserve point order and topology.
- Preserve Level 9's horizontal prominence.
- Do not shrink the playfield.
- Do not add per-level offsets.
- Do not add level-group offsets.
- Do not branch on level number, open/closed topology, shape, CenterY, or YOffset.
- Do not normalize or overwrite per-level projected Y origins.
- Do not change projection formulas.
- Do not change K or ZOffset.

The implementation should express clearly that the vertical placement is one experimental project translation chosen to keep the complete existing Levels 1-9 tube envelope inside the portrait viewport.

The diagnostic demonstrated that the minimum mathematical downward correction is approximately:

- 53.74 host pixels; or
- 44.78 portrait-internal units at the current 1.2 output scale.

The current internal translation is -78.

Therefore, an exact minimum-envelope correction would place the internal translation at approximately:

-78 + 44.78 = -33.22

Do not blindly copy rounded prose values if the source/test calculations support a more accurate value.

Derive or encode the smallest clear deterministic all-level-safe value supported by the diagnostic and current source calculations.

It is acceptable to use a simple, maintainable experimental constant when documented as a project translation.

Do not introduce runtime per-level fitting or dynamic scaling.

Do not introduce arbitrary extra visual margin beyond what is needed for a stable numerical boundary unless required to prevent ordinary floating-point boundary failure.

If a tiny safety allowance is necessary, keep it minimal, explicit, and documented as numerical protection, not visual calibration.


PRESERVE CURRENT PRESENTATION MODEL

Do not change:

- host window: 1280 x 960;
- portrait internal presentation: 600 x 800;
- centered host viewport: 720 x 960;
- side margins: approximately 280 pixels;
- output scale: 1.2;
- shared playfield scale: 1.05;
- horizontal center/placement;
- K=70;
- ZOffset=5;
- legacy 800 x 600 projection space;
- shared playfield camera architecture;
- separate HUD/screen-space camera;
- --legacy-presentation behavior;
- viewport scissor behavior;
- gameplay/model coordinates;
- lane topology;
- mechanics;
- timing;
- simulation behavior.

The only presentation calibration authorized in this pass is the one global vertical placement correction.


OPEN-TUBE TEST CORRECTION

Correct the newly expanded legacy tube-line assertion so it respects actual production topology.

The test must derive or otherwise correctly express the expected line count from playable-lane topology:

- closed 16-lane tube:
  16 rails + 16 near-rim segments + 16 far-rim segments = 48;

- open 16-vertex / 15-playable-lane tube:
  16 rails + 15 near-rim segments + 15 far-rim segments = 46.

Do not special-case Levels 8 and 9 by number if the existing topology/model API can express the correct rule.

Do not change DrawTube merely to make the old 48-line expectation pass.

The current 46-line rendering for open Levels 8 and 9 is correct.


HUD FLOATING-POINT TEST CORRECTION

Replace the exact HUD-corner equality that compares 960 with approximately 960.00006 with a narrow floating-point tolerance appropriate for the calculation.

Do not alter production HUD mapping to force exact binary floating-point equality.

Do not use a broad tolerance that could conceal meaningful layout drift.

Do not otherwise re-layout the HUD in this task.


INTERMEDIATE-DEPTH ALIGNMENT REGRESSION

Add a bounded regression test that protects the current candidate from a previously observed class of presentation failure:

The displayed tube may move to a new presentation mapping while tube-relative gameplay elements remain on a different mapping and appear to cross visible lane boundaries.

Do not import any implementation or formula from discarded experiments.

Base the test only on the current shared-camera architecture.

The test should use representative production geometry and one or more intermediate depths between the near and far rims.

At minimum, verify that:

- a representative point constructed on a tube rail or lane centerline at an intermediate depth;
- and a representative tube-relative gameplay anchor using the same model-space lane/depth interpretation;

receive the same final shared presentation mapping within a narrow numerical tolerance.

Prefer a focused architectural/invariant test over broad renderer reconstruction.

Do not require every local glyph vertex to lie on a lane; Flipper pivots, local enemy shapes, explosion geometry, and deliberate cross-lane animation may legitimately extend away from a centerline.

The purpose is to verify common presentation registration of world anchors, not to impose false gameplay constraints.

If the current affine shared camera already makes this invariant structurally automatic, test the shared mapping at representative intermediate depths without adding new production code.

Do not introduce nonlinear depth-dependent translation or a new projection model in this task.


CROSS-LEVEL BOUNDS VALIDATION

Update the current Levels 1-9 bounds coverage to verify the corrected one-shared-placement candidate.

For every production Level 1 through Level 9, verify:

- no left-edge overflow;
- no right-edge overflow;
- no top-edge overflow;
- no bottom-edge overflow;
- orientation remains preserved;
- point/lane ordering remains preserved where existing tests already support that assertion;
- Level 9 still occupies at least approximately 88% of portrait viewport width;
- source model points remain unchanged;
- no level-specific presentation value is used.

Use narrow floating-point tolerances where boundary calculations require them.

Do not alter tests merely to accept clipping.

If any production level still exceeds the viewport after the one shared correction, stop rather than adding another local fix.


TUBE AND ENTITY PRESENTATION PATH

Confirm through focused source inspection that the current candidate still applies the same playfield camera to:

- tube;
- Blaster;
- enemies;
- player shots;
- enemy shots;
- Spikes;
- explosions/effects;
- score popups or gameplay-space effects;
- transition/playfield elements included in the current shared pass;
- Game Over vectors where currently applicable.

Do not redesign those paths in this task.

If inspection reveals a path bypassing the shared camera, STOP and report an architectural mismatch rather than expanding implementation scope.


FAIL-CLOSED RULE

If the one shared correction produces:

- inversion;
- mirroring;
- orientation reversal;
- malformed topology;
- gameplay/model-coordinate changes;
- tube/entity camera divergence;
- horizontal clipping;
- bottom clipping;
- loss of Level 9 prominence;
- a requirement for per-level offsets;
- or any new rendering architecture beyond the bounded shared translation,

STOP.

Do not compensate with additional constants or local corrections.

Report the candidate as PARTIAL or BLOCKED and preserve a diagnosable worktree.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, extract, generate, or analyze any image, screenshot, video, gameplay recording, MAME frame, C# frame, contact sheet, or other visual asset.

Do not perform broad image/video extension searches.

This pass is based on source and the completed numerical diagnostic only.


REFERENCE AUTHORITY

Do not search the web.

Do not consult external Tempest implementations.

Do not use Mark Incitti as an implementation authority for this correction.

Do not inspect raw ROM material.

Do not alter existing accepted ROM-derived behavior.

Use only:

- the current candidate;
- current project source;
- the four authoritative reports named above;
- directly relevant durable project documentation if required;
- the bounded correction authorized by this task.


BOUNDED VALIDATION

Run a bounded focused test pass covering:

- DisplayConfigurationTests;
- the changed presentation/bounds tests;
- directly affected display/transition tests if required.

Use a VSTest hang timeout, such as 3 minutes.

If the focused tests fail:

- diagnose only failures directly caused by this bounded correction;
- correct only errors within the authorized scope;
- stop if the required fix would broaden the milestone.

If focused validation passes, run:

1. the full test suite with an explicit hang timeout;
2. a build with zero warnings/errors expected;
3. git diff --check.

Do not run unbounded command loops.

Do not repeatedly rerun unchanged failing commands.

If Windows security, a locked executable, timeout, hang, or stall blocks validation, follow the project's established self-stop policy.

Do not report canceled, timed-out, stale-binary, blocked, or incomplete validation as passed.


MANUAL ACCEPTANCE BOUNDARY

Do not launch the interactive game.

After automated validation passes, stop and provide Neal with manual launch commands.

The required first comparison is Level 9:

Experimental:

dotnet run --project .\src\Tempest.Game -- --level 9

Legacy:

dotnet run --project .\src\Tempest.Game -- --level 9 --legacy-presentation

After Level 9 is evaluated, provide commands for Levels 1-8 or a concise command pattern Neal can use to regression-check them individually.

Manual checks should include:

- overall 3:4 portrait composition;
- tube prominence;
- vertical placement;
- no clipping;
- no inversion or mirroring;
- tube and gameplay elements remain registered to the same lanes;
- Blaster, enemies, Spikers, shots, Spikes, and effects remain visually coherent;
- HUD remains legible and contained;
- legacy presentation remains unchanged.

Do not describe the candidate as accepted until Neal completes manual evaluation.


DOCUMENTATION AND AI LOGGING

This is a substantial Tempest implementation milestone.

Preserve the substantive prompt under AI-Logs/Prompts according to project instructions.

Create one new ignored implementation report under AI-Logs/Reports with a descriptive 2026-11-13 filename, such as:

2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md

The report must include:

- starting Git state;
- authoritative diagnostic used;
- exact files read;
- exact files changed;
- old and new shared vertical placement;
- derivation or justification of the new placement;
- confirmation that no per-level values were introduced;
- open-tube test correction;
- HUD floating-point test correction;
- intermediate-depth alignment regression;
- Levels 1-9 final bounds;
- Level 9 width/prominence result;
- focused validation results;
- full-suite result, if run;
- build result;
- git diff --check result;
- unresolved manual visual questions;
- final Git state;
- manual commands;
- confirmation that candidate remains unstaged/uncommitted;
- confirmation that no visual assets were analyzed.

Do not rewrite historical reports.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md yet.

Durable documentation waits for manual acceptance.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.

Leave the corrected candidate inspectable and reversible for Neal's manual acceptance.


FINAL RESPONSE

Report:

1. Whether the shared correction completed successfully.
2. Starting and final branch/HEAD/origin state.
3. Files changed.
4. Old and new vertical-placement values.
5. How the new shared placement was derived.
6. Confirmation that no per-level presentation constants were added.
7. Final Levels 1-9 bounds and any remaining overflow.
8. Level 9 width/prominence result.
9. Open-tube topology test correction.
10. HUD floating-point test correction and tolerance.
11. Intermediate-depth alignment regression added.
12. Confirmation that tube-relative gameplay elements still use the shared presentation path.
13. Focused test result.
14. Full-suite result.
15. Build result.
16. git diff --check result.
17. Any blocked or partial work.
18. Exact Level 9 experimental and legacy commands.
19. Concise Levels 1-8 regression command guidance.
20. Recommended manual checklist.
21. Implementation report path.
22. Final Git state.
23. Confirmation that nothing was staged, committed, or pushed.
24. Confirmation that no visual assets or Gemini materials were analyzed.

Stop after automated validation and hand the candidate to Neal for manual acceptance.

TEMPEST_PRESENTATION_SHARED_VERTICAL_PLACEMENT_CORRECTION_20261113