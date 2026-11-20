IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task expressly authorizes them or they are directly necessary within the bounded implementation below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is not project evidence unless Neal explicitly authorizes it in the current user task.


TASK

Convert the existing --portrait-perspective development/calibration option from an artificially narrow parser-limited range into a useful expert diagnostic control.

The endpoint-normalized baseline-referenced perspective formulation is now:

- automated-validation complete;
- manually gameplay/transition accepted by Neal;
- frozen for this milestone.

Do NOT change the endpoint-normalized perspective formula.

Do NOT change the game presentation architecture.

Do NOT perform additional perspective design.

This milestone has two goals:

1. remove the arbitrary [-5,+5] aesthetic/parser limitation while retaining meaningful numeric-input validation; and
2. establish and document a validated/recommended testing envelope with a small set of human-visible perspective probes.

The distinction is critical:

PARSER-ACCEPTED DOMAIN:
Inputs that the development diagnostic accepts syntactically/numerically.

VALIDATED/RECOMMENDED ENVELOPE:
Values that automated calculations/tests in this milestone establish as mechanically well-behaved within the current Levels 1-9 presentation and transition contexts.

These are NOT required to be the same.

Neal understands that deliberately testing outside the validated/recommended envelope may produce clipping, extreme rendering, malformed presentation, or runtime/rendering errors.

Such behavior outside the validated envelope is not automatically a project defect.

This is a development diagnostic, not an end-user graphics setting.

Do not protect Neal from intentionally experimenting with an extreme finite value merely because that value has not received visual or automated acceptance.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current endpoint-normalized portrait-presentation candidate remains uncommitted.

Before implementation, determine the exact current tracked/untracked state.

Expected from the previous milestone/checkpoint:

- master at HEAD/origin 385f603a94391756b2ca6cccd524dc17b281e782;
- clean index;
- current presentation/perspective candidate present only as unstaged worktree changes;
- no relevant non-ignored untracked files.

The prior checkpoint recorded approximately ten modified tracked files, but DO NOT fail merely because the exact count differs from historical prose.

Instead compare the actual current status with the endpoint-normalized implementation report named below.

If unexpected user work exists beyond that documented candidate, STOP and report it.

Do not reset, restore, clean, discard, stage, commit, or push anything.


AUTHORITATIVE REPORT

Read:

AI-Logs/Reports/2026-11-14-whole-game-presentation-endpoint-normalized-perspective-implementation-report.md

Use it as authority for:

- the implemented endpoint-normalized formula;
- files changed by the previous milestone;
- current validated projection architecture;
- stable-equivalence results;
- projective-line coherence;
- transition amplification protection;
- Approach handling;
- playback/reverse-step determinism;
- parser behavior;
- Git state.

Also read only if necessary for the derivation history:

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-transition-interaction-readonly-report.md

Do not reopen earlier visual-analysis reports unless a required numeric/presentation-envelope fact is unavailable from the endpoint-normalized report and current tests/source.


MANUAL ACCEPTANCE NOW RECORDED

Neal has manually tested the endpoint-normalized candidate and reports:

"The manual game play is as expected."

Treat the transition-composition acceptance boundary as PASSED unless a later specific defect is reported.

The purpose of this milestone is no longer to establish whether endpoint-normalized composition works.

It is to make the existing runtime diagnostic useful for human perspective calibration.


FROZEN IMPLEMENTATION

Do not change:

- endpoint-normalized perspective equation;
- baseline-reference semantics;
- active near/far context propagation;
- near-rim zero correction;
- X projection;
- projective-line interpolation;
- shared tube/entity projection path;
- Approach treatment;
- ExitZoom/ReverseZoom treatment;
- K=70 stable value;
- ZOffset=5 stable value;
- playfield scale 1.05;
- shared vertical placement -33.1;
- 600x800 internal portrait presentation;
- 720x960 portrait viewport;
- 1280x960 host;
- HUD camera/layout;
- gameplay/model geometry;
- mechanics;
- simulation timing;
- transition timing;
- recording schema;
- playback determinism;
- legacy presentation;
- default perspective value 0.

The mathematical perspective mechanism is frozen.

Do not redesign it to accommodate larger values.


NO VISUAL-ASSET ANALYSIS

Do not search for, enumerate, open, inspect, generate, or analyze:

- screenshots;
- images;
- videos;
- MAME frames;
- C# frames;
- recordings;
- contact sheets;
- comparison images;
- active-editor images.

Do not inspect:

- Gemini material;
- Incitti implementation;
- web sources;
- raw ROM files.

This milestone uses current source, existing reports, and bounded mathematical/test analysis only.


PARSER POLICY

Preserve:

--portrait-perspective <signed-number>

Remove the arbitrary [-5,+5] parser rejection.

The parser must continue to reject:

- missing value;
- malformed numeric value;
- NaN;
- positive infinity;
- negative infinity;
- duplicate use if duplicate rejection is current established behavior;
- use with --legacy-presentation if that conflict is current established behavior.

Use invariant-culture parsing.

The diagnostic should accept substantially wider finite signed values without imposing an arbitrary aesthetic upper/lower bound.

Before choosing the exact parser policy, inspect the numeric type and downstream calculation.

Preferred policy:

- accept any finite value representable by the existing runtime numeric type if doing so does not itself create immediate parser/runtime conversion hazards;
- clearly distinguish "accepted input" from "validated presentation range."

If accepting every finite float/double would create obvious arithmetic-overflow hazards before projection can fail naturally, establish only the broadest defensible implementation bound needed to protect numeric machinery.

Do NOT derive a hard parser bound from:
- viewport clipping;
- aesthetic quality;
- arcade fidelity;
- whether a value has been automated-tested;
- whether a value looks ridiculous.

Those belong to the validated/recommended envelope, not parser validity.


VALIDATED / RECOMMENDED ENVELOPE

Independently derive a practical validated envelope for the current Levels 1-9 candidate.

The validated envelope should identify values for which automated evidence establishes, as applicable:

- finite projection results;
- near-rim placement preserved;
- X unchanged;
- projective-line coherence preserved;
- tube/entity registration preserved;
- stable-play Level 1-9 geometry remains interpretable;
- transitions remain finite and compositionally coherent;
- Approach remains finite/coherent;
- no new formula-induced discontinuity;
- reserved upper gameplay-text region remains meaningfully available;
- viewport clipping status is explicitly known.

The validated envelope does NOT need to include every parser-accepted value.

Do not artificially narrow the validated envelope merely to avoid all visible difference.

We specifically need enough range for useful human calibration.


HUMAN-VISIBLE PROBES

The prior [-5,+5] range was inadequate:

- +5 moved the normal far region only about 7.05 host pixels;
- Neal could not practically distinguish the intended perspective difference;
- that was not a useful human acceptance experiment.

Now derive a small practical probe set from the endpoint-normalized formula.

At minimum evaluate candidate probes in territory such as:

- 0;
- +20;
- +40;
- +60;
- +80;
- +100;

and corresponding negative values where analytically informative.

These are ANALYSIS CANDIDATES, not mandated accepted probes.

Determine actual displacement and bounds from the current formula.

Select a small recommended manual set:

1. neutral 0;
2. clearly visible moderate positive;
3. unmistakably visible strong positive;
4. one useful negative value if desirable for directional confirmation.

Do not give Neal a large sweep.

The moderate/strong values must be visibly meaningful at normal game size without pixel counting, overlays, or prolonged inspection.


COEFFICIENT-TO-DISPLACEMENT ANALYSIS

For the endpoint-normalized formula, calculate and report the relationship between coefficient and final host-pixel vertical displacement at:

- near rim;
- representative intermediate depth;
- normal far rim;
- representative Approach context;
- representative ExitZoom/ReverseZoom contexts.

Because endpoint normalization is now implemented, verify that transient live K/ZOffset no longer amplify the endpoint displacement beyond the stable calibrated amount.

For each recommended probe report:

- near correction;
- intermediate correction;
- far correction;
- Level 1-9 final bounds;
- upper-region clearance;
- any viewport clipping;
- transition-context behavior.

Keep PROJECT CALIBRATION separate from arcade fact.


SAFE RANGE VERSUS EXPERIMENTAL RANGE

The final report and help/error text should clearly establish this philosophy:

- Values inside the validated/recommended envelope have automated evidence behind them.
- Values outside the validated envelope are experimental.
- Experimental values may produce extreme rendering, clipping, malformed presentation, or runtime/rendering failure.
- Such outcomes outside the validated envelope are not automatically project bugs.
- Neal may deliberately use those values to bracket the visually useful region.

Do not add intrusive runtime warnings every time Neal uses an out-of-envelope value unless the current CLI architecture already has a clean diagnostics/help mechanism for this.

Prefer documentation/reporting over adding noisy UI.


DEFAULT REMAINS ZERO

Do not select a final arcade perspective value.

Keep:

--portrait-perspective 0

as the default behavior when the option is omitted.

Manual calibration has not yet selected an accepted non-zero value.

The whole purpose of this milestone is to allow Neal to find that useful region.


COMMAND-LINE COMPOSABILITY

Preserve the already validated composability of --portrait-perspective with normal supported development/testing options where applicable, including:

- --level N;
- --lives N;
- --speed N;
- --rim-speed N;
- --record-gameplay <file>;
- --playback-file <file>;
- deterministic visible gameplay playback;
- existing compatible scenario selectors.

Preserve the current explicit conflict with:

--legacy-presentation

Do not put the perspective value in recording metadata or change recording schema.

The same recorded simulation must remain viewable at different perspective values without changing gameplay state.


PROJECTIVE-LINE AND ENTITY REGISTRATION VALIDATION

Do not assume larger coefficients remain numerically benign merely because the formula is mathematically affine.

Test recommended probes and validated boundaries for representative:

- Level 1 closed geometry;
- Level 6 closed geometry;
- Level 9 open geometry.

Verify:

- near/far/intermediate rail incidence;
- tube/entity anchor registration;
- Blaster anchor registration;
- representative shot anchor;
- Spike/lane center;
- enemy center where practical.

Use appropriate tolerances for larger coordinate magnitudes.

Do not loosen tolerances simply until failures disappear.

If numeric precision becomes unacceptable at extreme values, use that as evidence for the validated envelope, not as permission to alter the formula.


LEVELS 1-9 PRESENTATION ENVELOPE

At recommended probes and candidate validated boundaries calculate:

- min/max X;
- min/max Y;
- left/right clearance;
- top/bottom clearance;
- far-region position;
- any clipping;
- reserved upper-region relationship.

Do not move:
- -33.1;
- 1.05 scale;
- viewport;
- HUD;
- any tube.

A coefficient that looks extreme is allowed to be classified as outside the validated envelope.

Do not "fix" an extreme diagnostic value by moving the presentation.


TRANSITION / APPROACH REGRESSION

At the recommended strong positive probe and validated boundaries, test:

- stable play;
- first ExitZoom update;
- ZOffset=-41/K=70;
- representative high-K ExitZoom;
- ReverseZoom;
- restoration;
- LevelApproach;
- Approach completion.

Verify:

- no old live-K/live-Z perspective amplification returns;
- no discontinuity introduced;
- straight rails remain straight;
- equivalent anchors remain registered;
- endpoint-normalized displacement remains bounded as designed.

Do not redesign transition behavior.


SIMULATION NEUTRALITY

At minimum compare neutral and the recommended strong positive probe across a bounded deterministic simulation.

Verify unchanged:

- gameplay/model coordinates;
- level state;
- score;
- lives;
- RNG progression where current tests expose it;
- recording/playback input interpretation;
- simulation timing.

Presentation coefficient must remain presentation-only.


IMPLEMENTATION SCOPE

Expected production changes should be very small.

Likely change:
- parser/range validation/help/error handling related to --portrait-perspective.

Tests may change where directly required for:
- parser acceptance;
- invalid finite/non-finite handling;
- validated-envelope probes;
- presentation bounds;
- transitions;
- entity registration;
- recording/playback composability.

Do not change Projection.cs or the endpoint-normalized formula unless a genuine existing defect is discovered.

If changing the formula seems necessary, STOP and report rather than expanding scope.


FAIL-CLOSED CONDITIONS

STOP instead of broadening the milestone if:

- human-visible coefficients reveal a defect in the endpoint-normalized formula itself;
- rails bend;
- entity anchors detach;
- near-rim placement moves;
- X changes;
- transition amplification returns;
- gameplay state changes;
- per-level coefficients appear necessary;
- open/closed special cases appear necessary;
- scale or -33.1 would need changing;
- K/ZOffset would need changing;
- recording schema would need changing;
- final HUD work would be required.

Do not force a broad validated range if evidence does not support it.

A narrow VALIDATED envelope plus a broad parser-accepted experimental domain is acceptable.


NO BUILD/TEST LOOPS WITHOUT BOUNDS

Run a bounded focused validation covering:

- parser/option behavior;
- recommended probes;
- validated-envelope boundaries;
- projection/presentation invariants;
- Levels 1-9 bounds;
- intermediate-depth incidence;
- tube/entity registration;
- ExitZoom/ReverseZoom;
- Approach;
- recording/playback composability;
- simulation neutrality.

Use explicit VSTest hang timeout of no more than 3 minutes.

If focused validation passes:

1. run the full test suite with explicit 3-minute hang timeout;
2. run Release build;
3. run git diff --check.

Do not repeatedly rerun unchanged failures.

Do not use stale binaries after failed builds.

Follow the established Windows-security/stall self-stop policy.


MANUAL ACCEPTANCE HANDOFF

Do not launch the interactive game.

Provide Neal a very small practical Level 9 calibration set using actual values justified by this milestone:

Neutral:
--portrait-perspective 0

Moderate:
--portrait-perspective <moderate>

Strong:
--portrait-perspective <strong>

Optional opposite-direction:
--portrait-perspective <negative>

Do not include +5 as the "strong" comparison merely because it was previously validated.

The useful probes must be visibly different.

Also show that the option remains composable, for example:

--level 9 --lives 8 --speed 0.5 --portrait-perspective <strong>

and one playback-file example.

Provide concise command patterns for Levels 1-8.

Neal may also deliberately enter finite values outside the validated envelope.

Do not characterize resulting extreme behavior outside the validated envelope as a defect unless it demonstrates violation of an invariant that should remain valid for all finite parser-accepted values.


MANUAL CALIBRATION GOAL

The next manual question is finally:

Does the endpoint-normalized perspective mechanism move the game toward the arcade-looking-down-the-tube presentation when the effect is large enough for a human to actually see?

Neal should be able to determine this within seconds between neutral and strong probes.

If the direction is correct:

- bracket the visually useful region manually;
- do not fine-tune final value in code yet.

If the direction is wrong:

- stop further coefficient calibration;
- report the visual finding;
- investigate the perspective model rather than compensating with unrelated presentation changes.


DOCUMENTATION AND AI LOGGING

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-report.md

The report must include:

- starting Git state;
- files inspected;
- files changed;
- confirmation endpoint-normalized formula remained unchanged;
- former parser range [-5,+5];
- new parser-accepted numeric policy/domain;
- invalid inputs still rejected;
- validated/recommended envelope;
- distinction between parser-accepted and validated values;
- coefficient-to-host-pixel relationship;
- recommended neutral/moderate/strong/negative probes;
- Levels 1-9 bounds at those probes;
- upper-region results;
- transition/Approach results;
- line-incidence results;
- tube/entity registration results;
- simulation-neutrality result;
- command-line composability;
- focused test result;
- full-suite result;
- Release build result;
- git diff --check result;
- manual commands;
- explicitly unvalidated experimental behavior outside the recommended envelope;
- final Git state;
- confirmation nothing staged/committed/pushed;
- confirmation no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.


NO DURABLE BEHAVIOR DOCUMENTATION YET

Do not update:

TEMPEST-DEVELOPMENT-NOTES.md

or:

TEMPEST-BEHAVIOR-SPEC.md

with a final perspective value.

No non-zero perspective strength has received manual arcade-fidelity acceptance yet.


NO COMMIT OR PUSH

Do not stage.
Do not commit.
Do not push.

Leave the candidate reversible for Neal's manual calibration.


FINAL RESPONSE

Report:

1. Whether the calibration-control milestone completed.
2. Starting/final Git state.
3. Files changed.
4. Confirmation endpoint-normalized formula remained unchanged.
5. Old parser-enforced range.
6. New parser-accepted numeric domain/policy.
7. Inputs still rejected.
8. Validated/recommended envelope.
9. Why parser domain and validated envelope differ.
10. Coefficient-to-host-pixel relationship.
11. Neutral probe.
12. Moderate positive probe.
13. Strong positive probe.
14. Optional negative probe.
15. Expected visible displacement of each.
16. Levels 1-9 bounds.
17. Upper-region results.
18. Transition/Approach results.
19. Line-coherence results.
20. Tube/entity-registration results.
21. Simulation-neutrality result.
22. Command-line composability result.
23. Focused tests.
24. Full suite.
25. Release build.
26. git diff --check.
27. Exact Level 9 manual commands.
28. Lives/speed composition command.
29. Playback-file example.
30. Levels 1-8 reusable pattern.
31. What behavior outside the validated envelope remains explicitly unvalidated.
32. Manual calibration goal.
33. Report path.
34. Final Git state.
35. Confirmation nothing staged, committed, or pushed.
36. Confirmation no visual assets or prohibited references were inspected.

Stop after automated validation and hand the control to Neal for manual visual calibration.

TEMPEST_PRESENTATION_HUMAN_VISIBLE_PERSPECTIVE_CALIBRATION_20261114