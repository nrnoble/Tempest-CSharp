IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, active/current editor files, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


AUTOMATIC / ACTIVE-EDITOR CONTEXT IS NOT AUTHORIZATION

Files automatically surfaced by VS Code because they are open, active, recently viewed, suggested, or otherwise included as editor/chat context are NOT authorized merely because VS Code shows them to the agent.

Do not read or use automatically surfaced files unless this task expressly authorizes them or they are directly necessary source/report files within the bounded investigation below.

In particular:

DO NOT read TEMP/myNotes.md.

TEMP/myNotes.md is Neal's personal scratchpad and is not project evidence unless Neal explicitly authorizes it in the current user task.


TASK

Perform a tightly bounded READ-ONLY architectural investigation of the interaction between:

1. the experimental portrait-perspective calibration term; and
2. the existing ExitZoom and ReverseZoom projection/camera animation.

Do not implement anything.

The previous range-expansion milestone correctly stopped because the unchanged perspective formula is safe and useful during stable play but becomes severely amplified when ExitZoom/ReverseZoom animate Projection.K and Projection.ZOffset.

Determine the correct semantic relationship between the portrait-perspective calibration and those transient camera states.

The investigation must answer whether the experimental coefficient should:

A. remain coupled to the live animated K and ZOffset;

B. use stable baseline projection parameters only for calculating the added perspective correction while the existing exit/reverse projection continues normally;

C. fade or otherwise transition coherently between stable-play and transient-camera interpretations;

D. be neutralized during one or more transient states;

E. or be considered architecturally incompatible with the current exit/reverse camera.

Do not assume any answer in advance.

Do not modify source, tests, constants, parser behavior, recording formats, documentation, or Git state.

Do not stage, commit, or push.


CURRENT AUTHORITATIVE REPOSITORY STATE

Repository baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should remain at the same commit.

The current worktree intentionally contains nine modified tracked files from the portrait and perspective candidate:

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

Do not restore, reset, clean, discard, overwrite, stage, commit, or push this candidate.


STARTING SAFETY CHECK

Before analysis, perform bounded read-only checks of:

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

If the repository differs materially, STOP before analysis and report the discrepancy.

Do not modify anything to force the expected state.


AUTHORITATIVE REPORTS

Read:

AI-Logs/Reports/2026-11-14-whole-game-presentation-shared-perspective-experiment-report.md

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-range-expansion-report.md

These reports are authoritative for:

- the current perspective formula;
- runtime-option behavior;
- current range [-5,+5];
- neutral default 0;
- validated line coherence and entity registration;
- stable-gameplay coefficient-to-pixel relationship;
- the blocked range expansion;
- the documented ExitZoom/ReverseZoom amplification.

Do not reopen earlier image-analysis reports unless a directly cited architectural fact is unavailable from these two reports and current source.


KNOWN BLOCKING EVIDENCE

The current experimental term is:

q = 1 / (z + ZOffset)

qNear = 1 / (nearDepth + ZOffset)

xProjected = 400 + x * K * q

yProjected = CenterY + (y + YOffset) * K * q
             + a * K * (q - qNear)

where a is the signed portrait-perspective coefficient.

Stable gameplay:

- K = 70;
- ZOffset = 5;
- normal far-depth host displacement is approximately -1.40979 * a pixels;
- +40 therefore moves the normal far region approximately 56.39 pixels upward.

ExitZoom:

- decreases ZOffset from 5 to -41 while K remains 70;
- then holds ZOffset at -41 while increasing K;
- at K=70 and ZOffset=-41, the same +40 probe moves far-depth geometry approximately 383.4 host pixels upward;
- later rendered exit frames can reach approximately K=440, producing substantially greater amplification.

ReverseZoom returns these projection parameters toward stable values.

The previous task correctly retained no range expansion.

Do not reinterpret that blocked result as authorization to accept clipping.


STRICT READ-ONLY SCOPE

Do not modify:

- production source;
- tests;
- report history;
- durable documentation;
- parser behavior;
- command-line ranges;
- projection constants;
- transition behavior;
- gameplay behavior;
- recording/playback format;
- current candidate files.

The only files this task may create are:

- the ignored saved prompt under AI-Logs/Prompts;
- one ignored read-only report under AI-Logs/Reports.


NO VISUAL-ASSET ANALYSIS

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
- active-editor images.

Do not inspect Gemini material, Incitti source, web sources, or raw ROM files.

This is a source-and-mathematics architectural investigation.


PRIMARY SEMANTIC QUESTION

Determine what the experimental portrait-perspective coefficient conceptually represents in the current architecture.

Specifically distinguish among:

1. Base gameplay projection/camera geometry.
2. Portrait presentation calibration.
3. Transient exit/reverse camera animation.
4. Uniform post-projection viewport/presentation mapping.

Determine whether the perspective coefficient is intended to describe:

- a permanent property of the stable portrait viewing geometry;
- a property that should be multiplied by all live projection animation values;
- a presentation correction referenced to the stable camera;
- or another clearly demonstrated concept.

Use actual source behavior.

Do not decide this from naming alone.


TRACE THE COMPLETE CAMERA / PROJECTION LIFECYCLE

Trace, with source locations:

- normal Playing state;
- ExitDelay;
- AvoidSpikes where relevant;
- ExitZoom;
- fatal exit-Spike handling if currently relevant;
- ReverseZoom;
- restoration to normal play;
- between-level approach;
- deterministic playback and reverse stepping where projection state is reconstructed.

For every state, report:

- live K;
- live ZOffset;
- world-depth interpretation;
- near-depth reference;
- far-depth reference;
- which projection overload/path is used;
- where the portrait-perspective term is applied;
- where the portrait playfield camera is applied;
- whether tube geometry and tube-relative entities share the same projection context;
- whether projection parameters represent simulation state, rendering state, or both.

Do not broaden into an general gameplay audit.


BASELINE-REFERENCED CORRECTION ANALYSIS

Analyze this candidate architecture without implementing it:

Continue using the actual live K/ZOffset for the established ExitZoom/ReverseZoom base projection, but calculate only the added portrait-perspective correction from stable baseline parameters:

- baseline K = 70;
- baseline ZOffset = 5;
- the current level/configuration near and far depths;
- the same world depth used by the rendered anchor.

Determine whether this would:

- preserve the normal stable-gameplay result exactly;
- prevent exit/reverse amplification;
- preserve near-rim anchoring;
- preserve X coordinates;
- preserve projective-line incidence;
- preserve tube/entity registration;
- preserve continuity at transition entry and exit;
- preserve approach behavior;
- create any discontinuity when K/ZOffset begin or stop animating;
- leave existing exit/reverse zoom visually intact apart from preventing coefficient amplification.

Provide equations and bounded calculations.

Do not implement this candidate.


NEUTRALIZATION ANALYSIS

Analyze, without implementing:

- perspective coefficient forced to zero during ExitZoom only;
- coefficient forced to zero during ExitZoom and ReverseZoom;
- coefficient instantaneously gated at state boundaries;
- coefficient faded using an existing continuous transition quantity, if one already exists.

For each, determine:

- continuity at entry;
- continuity at exit;
- visual jump risk;
- line-incidence behavior;
- tube/entity registration;
- interaction with playback/reverse stepping;
- whether this introduces a second independently animated camera concept;
- whether it changes accepted transition presentation.

Do not invent a new animation merely to make gating smooth.

Do not recommend a fade unless the required continuous quantity already exists and has clear semantics.


LIVE-PARAMETER COUPLING ANALYSIS

Analyze whether retaining live K/ZOffset coupling has any defensible architectural meaning.

Determine:

- whether multiplication by animated K is expected for a camera-height term;
- whether the changing denominator naturally represents exit-camera motion or accidental double amplification;
- whether the formula is effectively combining two camera changes that should not be multiplied;
- whether a safely nonlinear coefficient clamp would preserve semantics or merely hide the coupling problem;
- whether meaningful stable-play probes are fundamentally incompatible with live transition coupling.

Do not propose arbitrary clamps or per-state coefficient limits unless analyzing why they would or would not be coherent.


PROJECTIVE-LINE COHERENCE

For every candidate interpretation, prove or disprove whether straight model-space tube rails remain straight after final portrait mapping.

Evaluate:

- near endpoint;
- far endpoint;
- representative intermediate depths;
- stable play;
- representative early ExitZoom;
- ZOffset=-41/K=70 boundary;
- representative high-K exit;
- ReverseZoom.

A candidate that bends mathematical rails while the renderer draws one endpoint-to-endpoint segment is not acceptable.

Sharing a helper is not sufficient proof.


TUBE/ENTITY REGISTRATION

For every candidate interpretation, determine whether tube-relative gameplay anchors use the same final mapping as the equivalent tube-space points.

Include:

- Blaster;
- enemies;
- Spikers;
- player shots;
- enemy shots;
- Spikes;
- gameplay-space effects.

Identify any path that uses a different depth or projection context.

Do not correct any mismatch in this task.


CONTINUITY AND REVERSIBILITY

This project has accepted deterministic playback and reverse stepping.

Determine whether each candidate approach is a pure function of:

- current simulation/render state;
- level configuration;
- current projection values;
- runtime perspective coefficient.

A viable approach must reproduce the same frame when stepping backward and forward through the same state.

Identify any proposal requiring hidden mutable interpolation state or transition history.

Prefer stateless derivations from existing state.

Do not implement snapshots or new recording fields.


TRANSITION AUTHORITY BOUNDARY

Do not assume the existing ExitZoom/ReverseZoom presentation is arcade-exact merely because it exists.

But do not change it within this investigation.

Classify existing transition behavior accurately as one of:

- manually accepted;
- project decision;
- reference-derived;
- ROM-backed;
- unresolved;

using current durable project documentation and source comments where available.

Read only directly relevant sections.

The purpose is to identify how strongly transition preservation constrains the next experiment.


APPROACH AND NON-STANDARD DEPTHS

Analyze the baseline-referenced and gating candidates for:

- LevelApproach depth offset;
- behind-pit or retraction depths that currently use the shared projection;
- Game Over vectors if applicable;
- ordinary stable play.

Determine whether a proposed semantic separation would remain coherent beyond ExitZoom/ReverseZoom.

Do not redesign these states.


COMMAND-LINE AND MANUAL TESTING IMPLICATIONS

No parser changes are authorized.

If one candidate architecture is supported, explain whether it would permit the existing:

--portrait-perspective <signed-number>

to expand to human-visible stable-play probes while preserving:

- --level N;
- lives;
- speed;
- rim speed;
- recording;
- playback;
- deterministic playback;
- legacy conflict behavior.

Do not invent a new option.


DECISION MATRIX

Provide a compact comparison of at least:

1. Current live K/ZOffset coupling.
2. Stable-baseline-referenced correction.
3. Instant neutralization during ExitZoom/ReverseZoom.
4. Existing-state-derived fade, only if such a semantically suitable fade exists.
5. Architectural incompatibility / abandon this mechanism.

For each evaluate:

- stable-play usefulness;
- transition clipping;
- continuity;
- line coherence;
- tube/entity registration;
- deterministic reversibility;
- implementation scope;
- effect on existing transition presentation;
- evidence strength.


DIAGNOSTIC CLASSIFICATION

Finish with one primary classification:

READY FOR BASELINE-REFERENCED PERSPECTIVE CORRECTION

Use only if computing the added term from stable baseline parameters is mathematically coherent, continuous, reversible, and bounded without redesigning transitions.

READY FOR STATE-DERIVED PERSPECTIVE GATING

Use only if an existing state variable provides a semantically justified, continuous, reversible gating mechanism.

PERSPECTIVE MUST BE DISABLED DURING TRANSITIONS

Use only if direct neutralization is the only coherent bounded choice and any boundary discontinuity is demonstrated to be absent or acceptably constrained.

CURRENT PERSPECTIVE MECHANISM IS ARCHITECTURALLY INCOMPATIBLE

Use if no safe shared interpretation preserves stable calibration and transition correctness.

INSUFFICIENT EVIDENCE

Use if source and calculations cannot establish a responsible next step.

Do not choose a classification merely to unblock implementation.


BOUNDED ANALYSIS

Use bounded source reads and numerical calculations.

A small temporary external script may be used outside tracked project files and removed afterward.

Do not run the interactive game.

Do not run the full test suite.

Do not build unless an essential source-level fact cannot otherwise be established.

Do not modify tracked or untracked candidate files.


REPORT

Preserve this substantive prompt under AI-Logs/Prompts.

Create:

AI-Logs/Reports/2026-11-14-whole-game-presentation-perspective-transition-interaction-readonly-report.md

The report must include:

- starting Git state;
- files inspected;
- confirmation candidate files were unchanged;
- current formula and current range;
- full projection/camera lifecycle;
- state-by-state K/ZOffset/depth/context table;
- stable-baseline-referenced candidate equation;
- bounded calculations for stable, exit, reverse, and approach contexts;
- neutralization analysis;
- fade analysis if applicable;
- current live-coupling analysis;
- line-incidence analysis;
- tube/entity registration analysis;
- continuity analysis;
- deterministic/reverse-step analysis;
- transition evidence classification;
- decision matrix;
- primary diagnostic classification;
- smallest recommended next implementation milestone, if supported;
- remaining uncertainties;
- final Git state;
- confirmation nothing was staged, committed, or pushed;
- confirmation no visual assets or prohibited references were inspected.


FINAL RESPONSE

Report:

1. Whether the investigation completed.
2. Starting/final Git state.
3. Confirmation candidate files were unchanged.
4. Source files inspected.
5. Current formula/range.
6. Stable-to-exit-to-reverse projection lifecycle.
7. Why current amplification occurs.
8. Semantic meaning of the perspective coefficient.
9. Stable-baseline-referenced correction result.
10. Neutralization result.
11. Fade result, if applicable.
12. Line-coherence result for each candidate.
13. Tube/entity-registration result.
14. Continuity result.
15. Playback/reverse-step determinism result.
16. Transition evidence classification.
17. Decision matrix summary.
18. Primary diagnostic classification.
19. Smallest recommended next implementation milestone.
20. Whether a human-visible range should become feasible.
21. Report path.
22. Confirmation nothing was staged, committed, or pushed.
23. Confirmation no visual assets, Gemini material, Incitti source, web sources, or raw ROM files were inspected.

Do not implement the recommendation.

TEMPEST_PRESENTATION_PERSPECTIVE_TRANSITION_INTERACTION_READONLY_20261114