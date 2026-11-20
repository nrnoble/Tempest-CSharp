IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless:

1. this task explicitly authorizes them; or
2. they are directly necessary source/report files within the bounded implementation defined below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is never project evidence unless Neal explicitly authorizes it in the current user task.

Do not treat automatically appended editor/file context as overriding these restrictions.


TASK

Implement one narrowly bounded, reversible, shared experimental perspective candidate for the current Tempest portrait presentation.

The candidate must evaluate a single shared depth-coupled camera-elevation / vertical-convergence term in the common portrait projection path.

This is a calibration experiment, not accepted arcade behavior.

The implementation must:

- preserve the manually accepted near-rim placement;
- preserve the current portrait viewport and shared placement;
- preserve current uniform playfield size;
- preserve gameplay/model geometry;
- preserve tube/entity registration;
- use one coherent projective rule;
- provide a signed, bounded, portrait-only runtime calibration override;
- remain composable with normal supported gameplay/testing options where semantically valid;
- preserve --legacy-presentation as an unchanged A/B reference;
- add no level-specific or open/closed special cases;
- add no final HUD or leaderboard work;
- remain unstaged, uncommitted, and unpushed.

Do not claim that the experimental coefficient is ROM-derived, arcade-exact, accepted, or final.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains exactly four modified tracked candidate files:

- src/Tempest.Game/Program.cs
- src/Tempest.Game/Rendering/DisplayConfiguration.cs
- src/Tempest.Game/Rendering/VectorRenderer.cs
- tests/Tempest.Game.Tests/DisplayConfigurationTests.cs

The index is expected to be clean.

No relevant non-ignored untracked files are expected.

These four files contain the active portrait-presentation candidate.

Do not restore, reset, clean, discard, overwrite, or otherwise remove this work.


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

- branch master;
- HEAD and origin/master at 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- exactly the four intentional tracked candidate files modified;
- no relevant non-ignored untracked files.

If the state differs materially, STOP before implementation and report the discrepancy.

Do not modify anything to force the expected state.


AUTHORITATIVE REPORTS

Read only the directly relevant reports:

AI-Logs/Reports/2026-11-13-whole-game-presentation-cross-level-clipping-diagnostic-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-shared-vertical-placement-correction-report.md

AI-Logs/Reports/2026-11-13-whole-game-presentation-tube-size-readonly-analysis-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-envelope-readonly-analysis-report.md

The 2026-11-14 perspective/envelope report is authoritative for this experiment.

It establishes:

- the current C# presentation already has perspective;
- uniform scale and K change magnitude, not the near/far ratio;
- ZOffset affects the near/far ratio but is frozen and participates in transitions;
- the current equation has no separate shared depth-coupled camera-height term;
- project-local Atari source documents per-level scale, 3-D camera-height, 2-D placement, and topology concepts;
- no direct numeric conversion from those ROM fields to current C# constants was established;
- the three authorized arcade frames support a stronger looking-down-the-tube presentation;
- one reversible shared perspective experiment is justified;
- the experiment should preserve accepted near-rim placement;
- the experiment should not choose a supposedly final coefficient from the screenshots;
- size should remain deferred through this experiment.


NO VISUAL-ASSET ANALYSIS

Do not open or analyze the three previously authorized MAME frames again.

Do not search for, enumerate, open, inspect, extract, generate, or analyze:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- gameplay recordings;
- playback files;
- contact sheets;
- comparison images;
- leaderboard or attract-screen images;
- any active-editor image.

The completed read-only report already contains the authorized visual findings needed for this implementation.


GEMINI MATERIAL IS PROHIBITED

Do not search for, locate, read, restore, or use:

- Gemini prompts;
- Gemini reports;
- Gemini chat exports;
- Gemini-generated code;
- formulas or constants from the discarded Gemini experiment.

Do not reproduce its implementation.


MANUAL FINDINGS AND PRESENTATION ENVELOPE

Preserve Neal's manual findings:

- the current shared vertical placement is correct;
- the current candidate is a strong step in the right direction;
- tube size still needs later improvement;
- looking-down-the-tube perspective is the immediate target;
- gameplay has a stable upper score/status/lives region above the tube;
- tube placement and margins are consistently managed across all 16 arcade layouts;
- visible perspective can differ among tube geometries;
- enclosed and non-enclosed tubes may appear to belong to different visual families;
- do not translate that observation into an invented open/closed rendering branch;
- final HUD layout is not part of this milestone;
- leaderboard/attract presentation remains deferred.

Reserve the established upper presentation region conceptually by preserving the current accepted tube placement and separate HUD camera.

Do not add or move final HUD text.


FROZEN VALUES

Do not change:

- host window: 1280 x 960;
- legacy logical projection space: 800 x 600;
- portrait logical presentation: 600 x 800;
- portrait viewport: 720 x 960;
- viewport placement: centered, left = 280;
- output scale: 1.2;
- playfield scale: 1.05;
- shared vertical translation: -33.1;
- horizontal center/placement;
- K = 70;
- ZOffset = 5;
- existing LevelDefinition points;
- existing CenterY values;
- existing YOffset values;
- level position/depth;
- gameplay geometry;
- mechanics;
- simulation timing;
- transition timing;
- HUD camera/layout;
- legacy presentation.

The experimental perspective term is the only new presentation variable authorized.


PRE-IMPLEMENTATION MATHEMATICAL DESIGN CHECK

Before modifying code, inspect the current projection and rendering paths and derive the smallest mathematically coherent form for the experiment.

The experimental mapping must:

1. be zero at the near rim so the accepted near-rim placement remains fixed;
2. vary continuously and monotonically with projection depth;
3. alter vertical convergence / apparent camera elevation;
4. preserve horizontal projection;
5. preserve finite values at all production gameplay depths;
6. use one shared equation for every production level;
7. use the same world-depth interpretation for tube and tube-relative entities;
8. preserve the common vanishing/presentation semantics;
9. avoid introducing per-level fitting constants;
10. remain inactive in legacy presentation.

Do not assume a specific formula merely because the report used the phrase “depth-coupled.”

Derive the formula from the current projection architecture.

PROJECTIVE-LINE COHERENCE IS REQUIRED

The tube rails are currently rendered as straight segments between projected near and far endpoints.

A depth-dependent mapping can cause intermediate gameplay anchors to leave those visible straight rails if the mapping is not projectively coherent.

This failure occurred in discarded work and must not recur.

Before implementation, prove analytically that the proposed vertical camera-height term preserves incidence for points that lie on a model-space tube rail:

- the projected near endpoint;
- the projected far endpoint;
- representative intermediate-depth points

must remain collinear in final portrait space within floating-point tolerance.

Sharing a helper is not sufficient.

If no bounded shared term can satisfy both:

- meaningful vertical-convergence control; and
- straight-rail/intermediate-anchor incidence,

STOP before source modification and classify the task as ARCHITECTURALLY BLOCKED.

Do not implement a nonlinear post-projection correction that bends the mathematical rail while DrawTube continues to render one straight endpoint-to-endpoint segment.

Do not compensate by individually moving entities.


ROM-EVIDENCE BOUNDARY

Do not reopen raw ROM files.

You may use the bounded findings already recorded in the perspective report:

- lev_scale exists;
- lev_y3d exists and is described as camera height;
- lev_y2d / lev_y2db exist;
- lev_open exists;
- setup loads these per-level values.

Do not claim that the new coefficient is lev_y3d.

Do not map a ROM byte to the experimental coefficient without a separately proven conversion.

Do not add current C# per-level camera-height data in this task.

The initial experiment must use one shared coefficient so Neal can evaluate the presentation model before a later bounded ROM-mapping milestone.


RUNTIME CALIBRATION OPTION

If the mathematical design check passes, add one clearly named portrait-perspective calibration option:

--portrait-perspective <signed-number>

This is a development/calibration option.

It must:

- accept one finite signed numeric value;
- reject missing, malformed, NaN, positive infinity, and negative infinity values;
- use invariant-culture parsing;
- enforce a conservative explicit finite range derived from bounded Levels 1-9 calculations;
- report an actionable error for values outside that range;
- affect only the experimental portrait perspective term;
- not change gameplay, simulation, timing, randomness, level selection, lives, speed, recording, playback, or legacy projection;
- not silently reinterpret another option;
- appear at most once unless the current parser already has an established duplicate-option rule that should be preserved.

Do not expose K, ZOffset, scale, translation, or ROM fields through this option.

Do not make the calibration value persistent.

Do not add a settings file or UI.


DEFAULT BEHAVIOR

Normal portrait startup without --portrait-perspective must use a single conservative non-zero experimental default chosen only after the bounded mathematical safety analysis.

The default must:

- be clearly documented as a PROJECT CALIBRATION CANDIDATE;
- preserve near-rim placement;
- remain inside the complete Levels 1-9 portrait envelope;
- preserve the upper presentation/HUD region;
- produce a measurable but not extreme perspective change;
- remain reversible through an explicit neutral override.

The neutral calibration value must be:

--portrait-perspective 0

That value must reproduce the current portrait candidate's projection/presentation before this milestone, subject only to ordinary floating-point tolerance.

Do not describe the non-zero default as arcade-accurate.

If the evidence cannot justify a safe, measurable non-zero default without guessing, do not invent one.

Instead:

- keep the default at 0;
- implement the signed runtime calibration mechanism;
- report a small bounded set of mathematically safe manual probe values in both directions;
- classify the implementation as CALIBRATION MECHANISM READY, DEFAULT PENDING MANUAL CHOICE.

That outcome is acceptable and preferable to an unjustified default.


LEGACY INTERACTION

--legacy-presentation must remain the unchanged A/B reference.

The combination:

--legacy-presentation --portrait-perspective <value>

must fail clearly as a semantically conflicting combination.

Do not silently ignore the perspective option in legacy mode.

Do not apply the perspective term to legacy mode.


COMMAND-LINE COMPOSABILITY

The new option should be orthogonal and composable wherever the current parser and simulation semantics permit.

Inspect the actual parser and preserve exact existing option behavior.

The new perspective option should compose with:

- --level N;
- --lives N where currently valid;
- --speed N;
- --rim-speed N where currently valid;
- --record-gameplay <file>;
- --playback-file <file>;
- existing deterministic visible-playback modes;
- existing scenario selectors where they already permit ordinary portrait rendering.

If record/playback parsing uses an explicit allowlist or startup-option filter, update it narrowly so the presentation-only option is recognized without altering recorded simulation inputs or recording schema.

Do not store the perspective coefficient in gameplay recordings unless a source-level necessity is demonstrated.

The same recording should remain simulation-identical when viewed with different portrait-perspective calibration values.

If any combination is genuinely unsafe or ambiguous, reject only that combination and document why.

Do not hard-code Level 9 into the option.


SHARED IMPLEMENTATION PATH

Implement the perspective term through the smallest centralized shared presentation/projection path.

The term must apply coherently to all relevant tube-space world anchors, including:

- tube near/far vertices;
- lane rails;
- Blaster anchors;
- enemies;
- Spikers;
- Spikes;
- player shots;
- enemy shots;
- gameplay-space effects;
- score popups;
- transition/playfield elements that use the same world-space projection;
- Game Over vectors where those vectors currently participate in the shared playfield path.

Do not add independent entity offsets.

Do not add a tube-only transform.

Do not add per-entity perspective formulas.

If any relevant path bypasses the proposed shared perspective mapping, STOP and report the mismatch rather than broadening the milestone arbitrarily.


NO LEVEL OR TOPOLOGY SPECIAL CASES

Do not branch based on:

- level number;
- Level 9;
- open versus closed tube;
- enclosed versus non-enclosed tube;
- tube shape;
- point count;
- CenterY;
- YOffset;
- screenshot identity.

Different visual results should arise from the shared projection and existing geometry/data, not separate calibration rules.


TRANSITIONS AND NON-STANDARD PRESENTATION STATES

Inspect the directly affected transition/presentation paths before implementation.

The experiment must not change:

- simulation-side Projection.K animation;
- simulation-side Projection.ZOffset animation;
- exit timing;
- approach timing;
- reverse stepping;
- playback determinism;
- death behavior;
- collision geometry.

Apply the portrait perspective term consistently where the normal shared portrait mapping is intended.

If exit/approach/reverse states require a distinct interpretation of the depth reference, use the current established renderer context and one equivalent shared formula.

Do not redesign exit or approach behavior.

If applying the term safely to these paths would require broader architecture, stop and report PARTIAL/BLOCKED rather than inventing a second camera system.


TEST REQUIREMENTS

Add or update focused tests for the new experiment.

At minimum test:

1. Neutral equivalence
   - --portrait-perspective 0 reproduces the current portrait projection before this milestone within narrow tolerance.

2. Near-rim anchor
   - positive and negative allowed values leave representative Levels 1-9 near-rim anchors unchanged within narrow tolerance.

3. Perspective effect
   - non-zero values measurably change far-rim vertical convergence or another directly intended vertical perspective relationship.

4. Horizontal preservation
   - X coordinates remain unchanged by the perspective coefficient.

5. Projective-line coherence
   - representative intermediate-depth points on production rails remain collinear with the projected near/far endpoints.
   - cover representative closed and open production geometries.
   - include at least Levels 1, 6, and 9 unless source evidence supports a better representative set.

6. Tube/entity registration
   - representative tube-relative entity anchors at intermediate depths receive the same final mapping as equivalent tube-space anchors.

7. Bounds
   - all production Levels 1-9 remain inside the portrait viewport at:
     - neutral 0;
     - the selected default, if non-zero;
     - each documented manual probe value;
     - the accepted minimum/maximum runtime bounds, if doing so remains numerically meaningful.

8. Reserved upper region
   - no tested candidate allows principal tube geometry to intrude into the currently protected upper presentation region established by the current candidate/report architecture.

9. Parser validation
   - valid signed values;
   - missing value;
   - malformed value;
   - NaN;
   - infinities;
   - out-of-range values;
   - conflict with --legacy-presentation;
   - combination with --level;
   - combination with speed/lives where valid;
   - combination with recording and playback parsing.

10. Simulation neutrality
    - changing only the perspective coefficient does not change bounded simulation state, RNG progression, gameplay coordinates, score, lives, level state, or recording/playback input interpretation.

11. Legacy preservation
    - legacy output remains unchanged;
    - conflicting legacy + portrait-perspective input is rejected.

Do not make tests assert visual arcade acceptance.

Do not encode a guessed final coefficient as an arcade fact.


BOUNDED PARAMETER RANGE AND MANUAL PROBES

Derive the runtime range from Levels 1-9 geometry, finite-denominator behavior, reserved upper-region protection, and viewport bounds.

Keep the range deliberately conservative.

Choose a small set of manual probe values:

- neutral;
- at least two values in the perspective-strengthening direction;
- at least one small value in the opposite direction, if safe and diagnostically useful.

Do not provide a large sweep.

Do not ask Neal to make dozens of comparisons.

The final report must explain:

- sign convention;
- what positive values do;
- what negative values do;
- which direction is expected to strengthen the looking-down presentation;
- why each probe is safe;
- whether the default remains 0 or uses one conservative non-zero candidate.


FAIL-CLOSED CONDITIONS

STOP rather than expanding implementation if:

- the proposed term cannot preserve projective-line coherence;
- intermediate-depth anchors leave rendered rails;
- tube and entities require separate transforms;
- near-rim placement moves;
- horizontal placement changes;
- a per-level or open/closed correction appears necessary;
- gameplay/model coordinates would need to change;
- K or ZOffset would need to change;
- the accepted -33.1 placement would need to change;
- the 1.05 playfield scale would need to change;
- final HUD work would be required;
- the upper presentation region cannot be preserved;
- transition behavior requires redesign;
- legacy presentation cannot remain unchanged;
- validation demands Gemini or external implementation material.

Report PARTIAL or BLOCKED and preserve the diagnosable worktree.


REFERENCE AUTHORITY

Do not search the web.

Do not inspect Mark Incitti's implementation.

Do not inspect Gemini material.

Do not inspect raw ROM files or directories.

Use only:

- current source;
- current candidate diff;
- the four authoritative reports listed above;
- directly relevant durable project documents where necessary;
- bounded calculations from production Levels 1-9.

Do not update durable project behavior documentation before manual acceptance.


BOUNDED VALIDATION

Run a bounded focused validation covering:

- projection/presentation tests;
- parser/options tests;
- Levels 1-9 bounds/orientation tests;
- approach/transition tests directly affected by the shared mapping;
- player/enemy/shot/Spike rendering-alignment tests;
- recording/playback parsing and deterministic-state tests directly affected by option composability.

Use an explicit VSTest hang timeout of no more than 3 minutes.

If focused validation passes, run:

1. full test suite with an explicit hang timeout;
2. Release build;
3. git diff --check.

Do not run repeated unchanged failing commands.

Do not use stale binaries after a failed build.

If Windows security, a locked executable, timeout, hang, or stall blocks validation, stop according to the established project policy.

Do not report blocked, timed-out, canceled, partial, or stale-binary validation as passed.


MANUAL ACCEPTANCE BOUNDARY

Do not launch the interactive game.

After automated validation, stop and provide Neal with a small, clear manual comparison set.

At minimum include Level 9 commands for:

- neutral portrait;
- conservative stronger-perspective probe 1;
- conservative stronger-perspective probe 2;
- legacy presentation.

Use the actual safe values derived during implementation.

Also provide one example showing composition with an existing option, such as a direct level plus lives or speed, and one example using an existing playback file argument placeholder.

Do not restrict Neal to Level 9.

Provide a concise reusable command pattern for Levels 1-8.

Manual checks:

- accepted near-rim/overall placement remains stable;
- stable upper gameplay-text region remains available;
- looking-down-the-tube perspective becomes stronger or weaker as documented;
- no clipping;
- no inversion or mirroring;
- tube rails remain straight;
- enemies, Spikers, shots, Spikes, effects, and Blaster remain registered to lanes throughout depth;
- different tube geometries respond coherently without identical-looking perspective;
- legacy presentation remains unchanged;
- gameplay feel and timing remain unchanged.

Do not describe the candidate as accepted before Neal's manual judgment.


DOCUMENTATION AND AI LOGGING

This is a substantial Tempest milestone.

Preserve the substantive prompt under AI-Logs/Prompts according to project instructions.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md

The report must include:

- starting Git state;
- source/reports inspected;
- files changed;
- mathematical form of the shared term;
- proof/argument for projective-line coherence;
- near-rim anchoring behavior;
- horizontal preservation;
- runtime option semantics;
- sign convention;
- accepted runtime range;
- neutral value;
- default value and why;
- manual probe values and why;
- command-line composability;
- legacy conflict behavior;
- transition-path treatment;
- Level 1-9 bounds at neutral/default/probes/range boundaries;
- reserved upper-region results;
- intermediate-depth alignment results;
- simulation-neutrality results;
- focused test result;
- full-suite result;
- Release build result;
- git diff --check result;
- unresolved visual/ROM questions;
- manual commands;
- final Git state;
- confirmation that nothing was staged, committed, or pushed;
- confirmation that no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.

Do not modify historical reports.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md yet.

Durable documentation waits for manual acceptance.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.

Leave the experimental candidate fully inspectable and reversible.


FINAL RESPONSE

Report:

1. Whether the shared perspective experiment completed.
2. Starting/final branch, HEAD, origin, index, and worktree state.
3. Files changed.
4. Mathematical form of the depth-coupled term.
5. Why the term preserves straight-rail/intermediate-anchor incidence.
6. How near-rim placement is preserved.
7. Confirmation X coordinates remain unchanged.
8. Runtime option name and syntax.
9. Sign convention.
10. Allowed range.
11. Neutral value.
12. Default value.
13. Manual probe values.
14. Whether normal startup changes by default.
15. Command-line composability results.
16. Legacy conflict behavior.
17. Transition/approach handling.
18. Levels 1-9 bound results.
19. Reserved upper-region results.
20. Intermediate-depth tube/entity alignment results.
21. Simulation-neutrality result.
22. Focused tests.
23. Full suite.
24. Release build.
25. git diff --check.
26. Any blocked or partial work.
27. Exact Level 9 manual commands.
28. Reusable Levels 1-8 command pattern.
29. Recording/playback command example.
30. Manual checklist.
31. Report path.
32. Final Git state.
33. Confirmation nothing was staged, committed, or pushed.
34. Confirmation no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.

Stop after automated validation and hand the candidate to Neal for manual calibration.

TEMPEST_PRESENTATION_SHARED_PERSPECTIVE_EXPERIMENT_20261114