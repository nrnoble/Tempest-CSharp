IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, attachment metadata, tagged-file information, or other automatically appended context may appear after the user-provided completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.

TASK

Implement a narrowly bounded presentation-only candidate for the ROM-backed four-frame Tempest enemy-shot vector geometry and deterministic shared animation phase.

The read-only investigation is complete. Use this report as the immediate investigation authority:

AI-Logs/Reports/2026-11-12-enemy-shot-rom-vector-rendering-recovery.md

Read that report first after completing the required starting safety checks.

This milestone may modify production rendering code and focused tests, but must not change enemy-shot gameplay or unrelated presentation.

STARTING SAFETY AND GIT STATE

Before implementation:

1. Run bounded checks of:
   - current branch;
   - HEAD;
   - origin/master;
   - staged changes;
   - tracked worktree changes;
   - non-ignored untracked files.

2. Expected accepted baseline:
   - branch: master
   - HEAD: cd678572c009d27fa8e1e1f0059ec45f6db5db11
   - commit description: Complete accepted Level 9 behavior
   - origin/master should match that commit
   - no staged or tracked-worktree changes are expected.

3. Ignored AI-Logs content, TEMP/, UserData/, recordings, screenshots, local reference assets, and other intentionally ignored user files are not reasons to stop merely because they exist.

4. If the tracked repository state materially differs from the expected baseline, STOP before implementation and clearly report the discrepancy.

Do not reset, restore, clean, checkout, stash, rebase, merge, amend, commit, push, delete, or overwrite existing user work.

Do not read TEMP/myNotes.md.

Do not modify or replace the project-local ROM reference.

SCOPE

Replace the current generic C# enemy-shot visual geometry with a candidate based on the four verified arcade ROM enemy-shot vector programs.

The implementation must:

- encode all four verified ROM-backed enemy-shot pictures;
- preserve each picture's four disconnected lit segments;
- preserve each picture's four point-like marks;
- preserve the verified relative logical coordinates and ROM draw order;
- preserve blank movement as non-visible movement;
- use one shared deterministic animation phase for all enemy bullets;
- remove per-projectile Rotation from visual frame selection;
- retain existing active-enemy-bullet collection authority;
- retain existing lane-midpoint and depth projection;
- retain existing game-state visibility gating;
- retain the current white rendered color for this candidate;
- preserve deterministic playback, pause, stepping, and reverse reconstruction;
- remain presentation-only.

The accepted Blaster-shot reconstruction may be used as an engineering and testing precedent, but do not copy Blaster-specific geometry, normalization, point count, color translation, or scale as though those were enemy-shot ROM facts.

ROM-BACKED GEOMETRY

Use the verified geometry recorded in:

AI-Logs/Reports/2026-11-12-enemy-shot-rom-vector-rendering-recovery.md

Treat the report's coordinates as cumulative relative logical vector units with X right and Y up, before projection and external scale.

Implement all four pictures exactly as recorded there, including deliberate asymmetries.

Do not redesign, smooth, symmetrize, rotate, reinterpret, or invent missing geometry.

Each frame must contain:

- exactly four disconnected visible line segments;
- exactly four visible point-like marks;
- no visible lines corresponding to blank beam movements;
- no generic full-diameter radial star.

Represent the geometry in a small, maintainable, directly testable rendering structure or vector-program abstraction consistent with the project's existing rendering architecture.

Include concise maintainability comments explaining:

- which parts are ROM-backed;
- that logical Y-up must be translated to screen Y-down where applicable;
- that normalization and point rasterization are project translations;
- that animation cadence is a deterministic project translation rather than a proven arcade frequency.

Do not add excessive historical commentary to production code.

SHARED DETERMINISTIC PHASE

The ROM establishes a global modulo-four selection from timectr. It does not establish a literal one-ROM-increment-to-one-C#-simulation-tick mapping.

Implement one shared deterministic four-frame enemy-shot animation phase.

Requirements:

- All enemy bullets rendered during the same simulation state must use the same frame.
- Frame selection must not depend on:
  - individual bullet age;
  - individual bullet Rotation;
  - lane;
  - bullet collection order;
  - render call count;
  - wall-clock time;
  - nondeterministic state.

- The phase must advance from deterministic simulation state, not from rendering activity.
- Rendering the same unchanged state more than once must not advance or alter the phase.
- Pausing must not allow wall-clock or render calls to advance the phase.
- Deterministic playback and reconstruction of the same simulation point must reproduce the same enemy-shot frame.
- Avoid changing recording/playback file schemas unless absolutely necessary. A schema change is not expected or authorized by this task.
- Do not reuse another gameplay mechanic's mutable clock if doing so could alter or couple that mechanic's behavior.

Use the smallest maintainable implementation that satisfies these requirements.

Because exact arcade cadence is unresolved, choose a conservative initial candidate mapping and document it explicitly as PROJECT TRANSLATION.

Prefer a simple mapping that is easy to adjust following Neal's manual testing.

Do not claim that the selected candidate cadence is ROM-proven or manually accepted.

Do not broaden this milestone into a general animation-clock redesign.

PRESERVE GAMEPLAY AND EXISTING STATE

Do not change:

- enemy-bullet creation;
- projectile speed;
- projectile trajectory;
- height advancement;
- firing behavior or cadence;
- ammunition behavior;
- collision behavior;
- collision dimensions;
- scoring;
- sound;
- expiration or lifecycle;
- enemy mechanics;
- level configurations;
- level progression;
- difficulty or gameplay timing;
- projection geometry;
- tube geometry;
- player shots;
- Blaster-shot rendering;
- destruction effects;
- HUD or whole-game presentation;
- recording/playback input behavior.

EnemyBullet.Rotation currently advances as simulation state.

Do not remove, repurpose, reset, reinterpret, or change the advancement of EnemyBullet.Rotation merely because rendering will no longer use it for enemy-shot frame selection. Preserve it unless a compelling technical requirement is discovered.

If preserving it creates a genuine conflict, STOP and explain the conflict rather than silently changing simulation or serialization behavior.

COLOR CONSTRAINT

Preserve current white enemy-shot rendering for this implementation candidate.

The ROM report identifies distinct color-index metadata, but it does not establish a final C# RGB translation, and this milestone does not authorize an enemy-shot color change.

Do not infer RGB values from the Blaster-shot translation.

Do not change brightness, blending, glow, sparkle, or color behavior outside what is minimally required to render the new geometry visibly in white.

POINT RASTERIZATION AND SCALE

The ROM's zero-displacement lit instructions require a visible point representation in C#.

Use a small, deterministic, testable point-rasterization translation appropriate to the current renderer.

Do not represent the four points as invented long rays or connected lines.

The ROM's logical coordinates are not literal screen pixels.

Use a single maintainable normalization/scale translation integrated with the existing midpoint/depth projection.

Keep the initial candidate conservative and easy to tune.

Do not alter projection constants or whole-game presentation to make the enemy-shot candidate fit.

Clearly classify point rasterization and normalization as PROJECT TRANSLATION.

IMAGE AND FRAME ANALYSIS RULE

Image/frame analysis is opt-in and explicitly scoped.

No images, screenshots, video frames, contact sheets, recordings, or other visual assets are provided or identified for analysis in this task.

Therefore, do not:

- search for image or video files;
- enumerate repository visual assets;
- open or inspect screenshots;
- inspect MAME frames;
- inspect C# playback frames;
- inspect contact sheets;
- extract frames from videos;
- batch-process images;
- run pixel, component, color, bounding-box, or computer-vision analysis;
- use TEMP/, Videos/, UserData/Screenshots/, or similar visual material as evidence.

Do not use broad file searches for image or video extensions.

If implementation cannot responsibly continue without particular visual evidence, STOP and report exactly what image or frame evidence is needed. Do not independently search for substitute images.

This rule does not prohibit implementation from the verified ROM geometry and source evidence.

TESTS

Update or replace only the focused tests that encode the obsolete generic eight-line enemy-shot presentation.

Add focused automated coverage for at least:

1. Each of the four ROM-backed enemy-shot frames.
2. Exactly four disconnected visible segments per frame.
3. Exactly four point-like marks per frame.
4. Verified relative geometry, including deliberate asymmetries.
5. Blank movements producing no visible connecting lines.
6. Preserved white output.
7. Preserved lane-midpoint projection.
8. Preserved depth scaling behavior.
9. Preserved active-bullet collection and visibility gating.
10. Multiple enemy bullets of different ages and Rotation values sharing the same frame at the same simulation point.
11. Deterministic phase progression.
12. Repeated rendering of an unchanged state not advancing the phase.
13. Pause behavior not advancing phase from wall-clock or rendering activity.
14. Deterministic playback/reconstruction producing the same frame for the same simulation point.
15. EnemyBullet.Rotation no longer selecting visual geometry while its existing simulation state remains preserved.
16. Projectile lifecycle and gameplay behavior remaining unchanged.

Do not add brittle pixel-perfect screenshot tests.

Prefer structural rendering tests consistent with the existing PlayerShot and enemy-bullet test architecture.

Do not weaken valid non-geometric tests merely to obtain a passing suite.

VALIDATION

Use bounded commands with explicit timeouts or existing VSTest hang-timeout protection.

After implementation:

1. Run the smallest focused enemy-shot rendering test group.
2. Run relevant projectile/rendering tests.
3. Run relevant deterministic playback, pause, stepping, and reconstruction tests affected by the phase implementation.
4. Run broader regression tests only to the extent reasonably necessary for this bounded candidate.
5. Run a solution build.
6. Run git diff --check.
7. Inspect the final diff and Git status.

Do not launch the interactive game.

Do not conduct manual visual acceptance on Neal's behalf.

Do not create or analyze screenshots as automated validation.

Canceled, timed-out, hung, stalled, blocked, or security-blocked commands must not be reported as passed.

If Windows Application Control, Smart App Control, or similar security blocks a generated DLL, EXE, test host, or command:

- do not weaken security;
- do not attempt a bypass;
- do not retry the same blocked artifact repeatedly;
- do not substitute stale binaries;
- do not claim validation passed;
- retry only through a materially different safe route when one is clearly available.

If required validation cannot complete, STOP voluntarily, preserve the worktree, and report:

- completed implementation work;
- completed validation;
- blocked or incomplete validation;
- the exact blocked artifact;
- the exact command;
- the exact error;
- Neal's required next step.

All potentially long commands or loops must be bounded. If an operation makes no meaningful progress beyond its expected duration, stop it and isolate the issue with smaller bounded runs rather than waiting indefinitely.

DOCUMENTATION AND REPORTING

This is an implementation candidate pending Neal's manual acceptance.

Update durable documentation only as narrowly necessary to record:

- that a ROM-backed enemy-shot rendering candidate now exists;
- the evidence classification;
- the selected shared-phase cadence as PROJECT TRANSLATION;
- the fact that visual/manual acceptance remains pending;
- preservation of existing gameplay and white color.

Do not describe the candidate as accepted, arcade-identical, complete, or closed.

Do not revise unrelated historical sections.

Preserve the substantial implementation prompt under:

AI-Logs/Prompts/

Create a paired ignored implementation report under:

AI-Logs/Reports/

Use descriptive date-prefixed 2026-11-12 filenames.

The report must include:

- starting Git state;
- files read;
- files changed;
- implementation summary;
- exact four-frame representation;
- shared deterministic phase design;
- chosen candidate cadence and its PROJECT TRANSLATION classification;
- point-rasterization and normalization translations;
- gameplay/state intentionally preserved;
- tests added or updated;
- exact validation commands and results;
- unresolved visual questions;
- manual-validation status;
- final Git status;
- confirmation that no unauthorized image/frame analysis occurred.

COMMIT AND PUSH

Do not commit or push.

Do not stage files.

Leave the bounded candidate in the worktree for Neal's manual review.

FINAL RESPONSE

At completion, provide:

1. concise implementation summary;
2. files changed;
3. shared-phase design and candidate cadence;
4. point and normalization translations;
5. tests and validation results;
6. unresolved visual questions;
7. manual launch command for Neal;
8. exact manual acceptance checklist;
9. report path;
10. final Git/worktree status;
11. explicit confirmation that no commit or push occurred;
12. explicit confirmation that no unspecified images or frames were searched for or analyzed.

The manual checklist should focus on:

- four-frame animation rather than the old rotating radial star;
- global synchronization across simultaneous enemy bullets;
- near/far scaling;
- point visibility;
- disconnected segment appearance;
- unchanged white color;
- unchanged movement, speed, firing, collisions, sound, and gameplay;
- pause, forward stepping, and reverse playback behavior when Neal uses a suitable existing recording.

Do not report manual acceptance until Neal performs it.

TEMPEST_ENEMY_SHOT_ROM_FOUR_FRAME_IMPLEMENTATION_20261112