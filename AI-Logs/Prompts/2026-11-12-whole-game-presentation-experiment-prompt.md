IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, attachment metadata, tagged-file information, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.

TASK

Implement a narrowly bounded, reversible, uncommitted experimental candidate for the Tempest whole-game presentation calibration.

Use Level 9 as the primary calibration fixture.

The target visual direction is the MAME presentation shown on the RIGHT side of the specifically authorized comparison image.

This is an EXPERIMENTAL PRESENTATION CANDIDATE.

Do not treat the first implementation values as accepted, final, ROM-defined, hardware-defined, or arcade-exact.

Do not commit or push.

AUTHORITATIVE INVESTIGATION REPORTS

Read these reports after completing the starting Git checks:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

For all image-dependent findings, the label-correction report supersedes the original report.

The authoritative visual provenance is:

LEFT = current C# presentation.

RIGHT = MAME/reference presentation.

RIGHT/MAME is the target.

Do not reverse these labels.

STARTING SAFETY AND GIT STATE

Before implementation, run bounded checks of:

- current branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected accepted baseline:

branch: master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match the same commit.

The tracked worktree and index are expected to be clean.

TEMP/ and other intentionally ignored local assets are not reasons to stop merely because they exist.

Do not broadly inspect TEMP/.

Do not read TEMP/myNotes.md.

If the tracked repository state materially differs from the expected baseline, STOP before implementation and clearly report the discrepancy.

Do not reset, restore, clean, checkout, stash, rebase, merge, amend, delete, overwrite, or otherwise discard user work.

Do not modify project-local ROM/reference files or user assets.

PRIMARY USER GOAL

Do not reduce the apparent on-screen game or tube merely to force the existing presentation into a predetermined application-window box.

The 3:4 portrait viewport and the previously proposed 720×960 fit are starting hypotheses, not immutable constraints.

The visual goal is:

- move the C# presentation toward the MAME target;
- preserve or increase the tube/playfield’s visual prominence as appropriate;
- reproduce the target’s portrait-style composition;
- improve tube scale, vertical placement, margins, and HUD relationship;
- preserve all gameplay geometry and behavior.

The supplied comparison indicates that the MAME tube occupies substantially more of its portrait presentation area than the current C# tube occupies in its landscape area.

Therefore:

- do not implement a portrait viewport by merely shrinking the current game until it fits;
- do not accept a candidate that makes the tube smaller or less prominent merely to satisfy 720×960;
- do not treat side margins as more important than the game presentation;
- revise the working viewport or transform hypothesis if necessary to preserve the intended tube prominence;
- do not crop essential gameplay content or permit clipping merely to enlarge the tube.

Keep the accepted 1280×960 host window during this experiment.

The accepted host window is a container for testing the internal presentation. It must not become an arbitrary visual-size ceiling that dictates an undersized game.

FUTURE SCALING GOAL

Design the experimental internal presentation so that a future milestone can uniformly scale the entire accepted presentation as one unit into fullscreen or borderless fullscreen.

The conceptual future pipeline should be:

accepted internal Tempest presentation
→ uniform whole-presentation scaling
→ centered fullscreen or borderless-fullscreen output

Preserve the accepted aspect and composition during that future scaling.

Do not implement fullscreen or borderless fullscreen in this task.

Do not implement interactive user-dragged resizing.

There is no requirement to dynamically rescale the presentation while the user manually drags window borders after launch.

Fullscreen scaling and arbitrary resizable-window behavior are separate concerns.

SCOPE

Implement the smallest coherent experimental candidate that allows Neal to judge:

1. Portrait-style presentation within the existing 1280×960 host window.
2. Tube/playfield prominence relative to the presentation area.
3. Global playfield scale.
4. Horizontal centering.
5. Vertical placement.
6. Space above and below the playfield.
7. Consistent alignment of the tube and all gameplay visuals.
8. Independent HUD layout.
9. Level 9 composition.
10. Regression behavior across Levels 1–8.

The first candidate should be global/whole-game, not Level-9-specific.

Do not broaden this milestone into:

- fullscreen;
- borderless fullscreen;
- arbitrary window resizing;
- gameplay timing;
- difficulty adjustment;
- new levels;
- tube geometry reconstruction;
- enemy rendering refinement;
- audio work;
- unrelated HUD redesign;
- general renderer replacement;
- ROM investigation.

CURRENT ARCHITECTURE TO PRESERVE WHERE PRACTICAL

The investigation established that the current project uses:

- an 800×600 logical canvas;
- a fixed 1280×960 host window;
- uniform 1.6 output scaling;
- shared scene/HUD camera behavior;
- Projection.K = 70;
- Projection.ZOffset = 5;
- per-level CenterY/YOffset presentation values;
- existing exit and reverse-zoom behavior that temporarily changes and restores projection state.

Do not discard this architecture wholesale if a focused presentation layer can produce the experiment safely.

The corrected investigation recommends reconsidering the 800×600 4:3 presentation, but not automatically deleting the legacy coordinate system.

Prefer adding a distinct presentation/viewport mapping around the established logical and projected coordinates where practical.

Any new abstraction should be small, maintainable, testable, and clearly separate:

- model/gameplay coordinates;
- projected playfield coordinates;
- internal presentation coordinates;
- host-window output coordinates;
- HUD coordinates.

EXPERIMENTAL PORTRAIT HYPOTHESIS

Use a 3:4 portrait presentation as the initial experimental direction.

A centered 720×960 viewport inside the existing 1280×960 host window was recommended as a first hypothesis, with 280-pixel side margins.

A corresponding 600×800 internal presentation space at 1.2 output scale was given only as one convenient example.

These are PROJECT TRANSLATION starting values, not requirements and not arcade facts.

Implementation requirements:

- begin from the 3:4 portrait hypothesis;
- keep the 1280×960 host window;
- center the internal portrait presentation;
- preserve aspect without horizontal or vertical stretching;
- do not distort circles, text, vectors, or tube geometry through nonuniform scaling;
- keep the implementation easy to tune;
- avoid scattering calibration values throughout rendering code;
- centralize experimental presentation parameters.

If 720×960 necessarily makes the tube less prominent than the current C# presentation or prevents approaching the MAME composition, do not blindly preserve it.

Use the smallest coherent alternative consistent with:

- a portrait internal presentation;
- the accepted 1280×960 host;
- no clipping of required playfield/HUD content;
- substantial tube prominence;
- global whole-game behavior;
- future uniform fullscreen scaling.

Document any deviation from the 720×960 starting hypothesis as PROJECT TRANSLATION.

PLAYFIELD PRESENTATION TRANSFORM

Implement a shared presentation transform for the rendered playfield.

The transform may include, as needed:

- uniform scale;
- horizontal translation;
- vertical translation;
- a defined presentation anchor;
- portrait viewport mapping.

It must be applied consistently to every visual that must remain aligned with the playfield, including as applicable:

- tube rails and rims;
- Blaster;
- enemies;
- enemy shots;
- player shots;
- Spikes;
- explosions and destruction effects;
- transition presentation;
- Super Zapper presentation;
- playfield-relative popups/effects;
- any other object rendered in projected playfield space.

Do not transform only the tube.

Do not leave gameplay objects using the old coordinate space while the tube uses the new one.

Do not alter the underlying model coordinates to accomplish visual placement.

PERSPECTIVE AND DEPTH

The corrected comparison suggests that simple uniform scaling and translation might not fully reproduce the MAME Level 9 shoulder/far-rim relationship.

However, one comparison image does not establish exact projection values or matched gameplay/transition phases.

For the first candidate:

- implement the portrait viewport and coherent global presentation transform first;
- preserve Projection.K = 70 and ZOffset = 5 initially if the candidate can be made coherent without changing them;
- do not casually tune K/ZOffset;
- preserve exit/reverse-zoom behavior and exact projection restoration;
- do not introduce a Level-9-only depth correction.

If the implementation evidence demonstrates that a simple scale/translation candidate cannot reasonably express the target’s composition, a small shared render-only depth-dependent vertical adjustment may be implemented only if:

- it is global rather than Level-9-specific;
- it applies consistently to all aligned playfield visuals;
- it does not change model/gameplay state;
- it is centralized and easy to disable/tune;
- it preserves deterministic rendering;
- it does not alter projection state used by gameplay or transition logic;
- it is explicitly documented as PROJECT TRANSLATION.

Do not choose projection values solely from screenshot pixels.

If introducing such an adjustment would require broad or risky rendering changes, STOP after implementing the simpler candidate and report the remaining visual limitation rather than expanding scope.

HUD

Lay out the HUD independently from the playfield transform.

The MAME target on the RIGHT side of the supplied image shows a visibly different HUD arrangement from current C#.

For this experimental candidate:

- use stable portrait-presentation anchors;
- keep score, lives, and level readable;
- do not let the playfield scale/translation unintentionally move or resize HUD text;
- avoid overlap with the enlarged/repositioned playfield;
- preserve score, lives, level values, and gameplay behavior;
- treat colors/content styling conservatively unless a change is directly necessary for the layout experiment;
- do not expand this task into a complete HUD art/fidelity redesign.

Centralize HUD layout values so Neal can request a bounded follow-up adjustment.

GAMEPLAY GEOMETRY MUST REMAIN FROZEN

Do not change:

- LevelDefinition.Points;
- lane topology;
- playable-lane count;
- lane adjacency;
- polygon winding;
- player movement geometry;
- enemy paths;
- enemy movement;
- collisions;
- projectile trajectories;
- shot behavior;
- spawn behavior;
- level configuration;
- score rules;
- lives rules;
- game speed;
- enemy speed;
- difficulty;
- progression;
- simulation timing;
- recording/playback input behavior.

Presentation changes must not change game-state coordinates or mechanics.

If the existing architecture makes a requested visual adjustment unsafe without changing gameplay coordinates, STOP and report that coupling instead of silently changing gameplay.

LEVEL 9 AND LEVELS 1–8

Use Level 9 as the primary calibration fixture because its broad shoulders, open layout, long rails, and narrow inner sections expose scale, clipping, centering, vertical placement, and perspective clearly.

Do not introduce Level-9-specific constants.

The presentation transform must be global and apply to all production Levels 1–9.

Automated checks must address all nine production layouts.

Manual acceptance remains Neal’s responsibility, with Level 9 checked first and Levels 1–8 checked for regression.

AUTHORIZED IMAGE ANALYSIS

Exactly one visual asset is authorized:

TEMP\Comparision of screen demisions.png

Correct provenance:

LEFT = C#.

RIGHT = MAME/reference.

RIGHT is the target.

You may inspect this specific image only as needed to guide the experimental candidate.

Do not:

- search for other images or videos;
- enumerate repository visual assets;
- inspect other screenshots;
- inspect other MAME or C# frames;
- inspect contact sheets;
- inspect recordings;
- extract video frames;
- generate screenshots;
- run pixel/color/component/bounding-box analysis on other assets;
- use broad image/video extension searches.

If another visual reference would materially help, report exactly what is needed and leave that for Neal to provide or authorize.

Do not independently locate a substitute.

Do not repeatedly reprocess the authorized image without a materially different necessary question.

TESTABILITY AND EXPERIMENTAL CONTROLS

Centralize the presentation parameters in an appropriate configuration or small presentation abstraction.

The experimental candidate should make these concepts independently testable and reasonably easy to tune:

- host dimensions;
- presentation aspect;
- viewport dimensions;
- margins;
- playfield uniform scale;
- playfield translation;
- playfield anchor;
- optional depth-dependent adjustment if implemented;
- HUD anchors.

Avoid magic values scattered across VectorRenderer or LevelLoader.

Do not add an elaborate runtime calibration UI.

A small development-only command-line option or configuration toggle for comparing the experimental presentation against the accepted legacy presentation is allowed if it substantially improves safety and manual evaluation.

If adding such an opt-in mode:

- keep normal startup behavior clearly defined;
- document the exact launch command;
- do not create multiple overlapping debug systems;
- ensure automated tests explicitly select the intended mode;
- keep it development-only or clearly provisional;
- do not widen the task into settings persistence or user-facing menus.

Because this work is experimental and uncommitted, prefer an easy way to compare or revert the presentation without Git surgery.

TESTS

Add or update focused tests sufficient to verify:

1. The accepted 1280×960 host-window dimensions remain available.
2. The experimental presentation uses the intended portrait aspect.
3. The viewport is centered in the host.
4. Uniform scaling is used.
5. No arbitrary nonuniform stretching occurs.
6. Presentation margins are calculated as designed.
7. The playfield transform is shared consistently across tube and aligned objects.
8. HUD anchoring is independent from the playfield transform.
9. Level 9 projected/presented bounds are centered and not clipped.
10. Levels 1–8 remain centered and not clipped.
11. All Levels 1–9 use the same global presentation model.
12. Level model points remain unchanged.
13. Lane topology/playable-lane counts remain unchanged.
14. Gameplay simulation coordinates are unaffected.
15. Projection state and exit/reverse-zoom restoration remain correct.
16. Playback/deterministic behavior remains unaffected.
17. Legacy presentation remains protected if an experimental opt-in mode is added.
18. Future host scaling can use a uniform whole-presentation fit without relying on user-dragged resize behavior.

Prefer geometric and structural tests.

Do not add screenshot-based tests.

Do not add brittle tests that assert incidental implementation details rather than presentation invariants.

VALIDATION

Use bounded commands with explicit timeouts or existing VSTest hang-timeout protection.

After implementation:

1. Run focused display/presentation/projection tests.
2. Run focused Level 9 rendering/presentation tests.
3. Run focused Levels 1–9 geometry/presentation regression.
4. Run relevant transition and playback tests if affected.
5. Run a broader test suite if the change reaches shared rendering paths.
6. Run a solution build.
7. Run git diff --check.
8. Review the final diff.
9. Review final Git status.

Smart App Control has been intentionally disabled by Neal because it repeatedly interfered with generated development assemblies.

Do not attempt to change or re-enable Smart App Control.

This does not authorize weakening other security controls.

If another security mechanism blocks validation:

- do not bypass it;
- do not disable it;
- do not retry the same artifact indefinitely;
- do not substitute stale binaries;
- stop and report the exact blocker.

Canceled, failed, timed-out, stalled, or blocked validation must not be reported as passed.

Do not launch the interactive game for Neal.

Provide the exact manual launch command instead.

DOCUMENTATION AND REPORTING

This is an experimental candidate pending Neal’s manual review.

Do not describe the presentation as accepted, complete, arcade-identical, or closed.

Update durable documentation only as narrowly necessary to record:

- an uncommitted experimental presentation candidate exists;
- the corrected visual provenance;
- portrait presentation is a PROJECT TRANSLATION hypothesis;
- gameplay geometry remains frozen;
- exact presentation values remain pending manual acceptance;
- fullscreen/borderless and arbitrary resizing remain deferred.

Do not record provisional calibration choices as accepted behavior.

Preserve this substantial prompt under:

AI-Logs/Prompts/

Create a paired ignored implementation report under:

AI-Logs/Reports/

Use descriptive 2026-11-12 filenames.

The report must include:

- starting Git state;
- files read;
- files changed;
- visual evidence used;
- confirmation that only the authorized image was analyzed;
- implementation architecture;
- exact experimental viewport/aspect values;
- whether 720×960 was retained or revised and why;
- playfield scale/anchor/translation;
- whether any depth-dependent adjustment was implemented;
- HUD layout;
- gameplay geometry explicitly preserved;
- test changes;
- exact validation commands/results;
- manual launch commands;
- manual acceptance checklist;
- unresolved visual questions;
- final Git status.

NO COMMIT OR PUSH

Do not stage files.

Do not commit.

Do not push.

Leave the experimental candidate in the worktree for Neal’s manual judgment.

Do not alter Git history.

If Neal rejects the candidate later, do not automatically revert it in this task. Simply leave the worktree state clearly documented.

MANUAL ACCEPTANCE CHECKLIST

Provide Neal a focused checklist covering:

- Level 9 tube prominence relative to the portrait presentation;
- comparison with the RIGHT-side MAME target;
- whether the tube was accidentally made too small;
- tube width and height;
- tube vertical placement;
- black space above and below;
- horizontal centering;
- clipping;
- perspective/shoulder/far-rim relationship;
- Blaster alignment;
- enemy alignment;
- player-shot and enemy-shot alignment;
- Spike and effect alignment;
- HUD score/lives/level placement;
- transition and reverse-zoom presentation;
- Levels 1–8 visual regression;
- unchanged movement, collisions, shot behavior, timing, and gameplay;
- whether the internal presentation appears suitable for future uniform fullscreen scaling.

FINAL RESPONSE

At completion, report:

1. implementation result;
2. experimental presentation architecture;
3. exact viewport/aspect values;
4. whether 720×960 was retained or revised;
5. playfield scale and placement;
6. HUD placement;
7. whether any depth-dependent adjustment was used;
8. files changed;
9. tests and validation;
10. gameplay geometry preservation;
11. manual launch commands;
12. manual acceptance checklist;
13. unresolved questions;
14. report path;
15. final Git/worktree status;
16. explicit confirmation that no commit or push occurred;
17. explicit confirmation that only TEMP\Comparision of screen demisions.png was visually analyzed.

TEMPEST_WHOLE_GAME_PORTRAIT_PRESENTATION_EXPERIMENT_20261112