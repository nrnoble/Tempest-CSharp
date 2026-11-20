READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Implement a tightly bounded DEVELOPMENT-ONLY Level 7 direct-Atari presentation
diagnostic candidate based on the completed reference-shape transform
investigation.

The diagnostic must test this specific presentation hypothesis:

The recovered Atari record-06 geometry can reproduce the supplied Level 7
reference shape when the already-projected endpoint sets receive:

1. one shared anisotropic horizontal-versus-vertical presentation gain; and
2. a far-endpoint-set-specific presentation translation relative to the near
   endpoint set.

This is a visual diagnostic candidate.

It is NOT an Atari hardware reconstruction.
It is NOT a ROM-backed presentation correction.
It is NOT authorization to change record-06 geometry.
It is NOT authorization to change projection mathematics.
It is NOT manual acceptance of the result.

IMPLEMENTATION BASIS

The controlling investigation report is:

AI-Logs/Reports/2026-11-16-level7-record06-reference-shape-transform-investigation-report.md

That investigation established diagnostically:

- record 06 already contains the required ordered 16-point geometry;
- C# preserves Atari's near-rim and far-rim cyclic adjacency;
- C# preserves Atari's same-index near/far rail pairing;
- no per-point movement, reindexing, or topology change is indicated;
- translation alone fails;
- uniform scale and translation fail;
- one global anisotropic X/Y transform improves the outer shape but does not
  correctly align the far rim and rail convergence;
- general affine and projective maps do not materially improve that global fit;
- a shared anisotropic gain plus a separate far-set translation provides the
  smallest useful diagnostic family;
- permitting separate near/far gains offers only negligible additional
  improvement and is not justified for this candidate;
- the fitted values are visual diagnostic measurements, not Atari constants.

Preserve those qualifications.

DIAGNOSTIC GOAL

Determine through manual A/B comparison whether changing only the final
presentation relationship of the existing near and far endpoint sets is
sufficient to make the Level 7 record-06 tube visibly match the supplied
reference shape.

The diagnostic must preserve:

- every record-06 raw coordinate;
- all record metadata;
- the current near and far runtime positions;
- the current projector behavior;
- all point identities;
- point ordering;
- cyclic rim adjacency;
- same-index rail pairing;
- closed-rim topology;
- and gameplay behavior.

The experiment concerns only final presentation mapping.

REFERENCE IMAGE

The authorized correct-shape visual reference is:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Level7_frame_0090-MAME.png

The filename identifies the capture source only.

Do not investigate MAME source, settings, emulation, rendering, scaling, or
implementation.

Treat the image solely as the supplied reference for the desired Level 7 tube
shape.

Do not inspect, enumerate, open, or analyze any other image unless one current
C# comparison screenshot is explicitly necessary to verify the investigation's
existing diagnostic values. If needed, use only:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\Level-7\Prototype\Level-7 prototype-cs.png

Do not inspect other screenshots, recordings, videos, frames, contact sheets,
or visual assets.

EVIDENCE CLASSIFICATION

The candidate must be explicitly documented and reported as:

DEVELOPMENT-ONLY / VISUAL-DIAGNOSTIC

The following are not established:

- original Atari physical X/Y gain;
- original AVG analog-output calibration;
- original monitor calibration;
- historical depth-conditioned final translation;
- historical use of the fitted diagnostic values.

Do not label the diagnostic values as ROM-BACKED, Atari-documented, arcade
hardware constants, or accepted project translations.

The screenshot-derived transformation is permitted here only to test whether
the recovered Atari record geometry is capable of producing the supplied
reference shape.

PRESERVE THE CONTROL

Do not silently change the existing direct-Atari prototype.

The current command must remain available with its existing behavior as the
control:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype

Add a separate, clearly named, opt-in DEVELOPMENT-ONLY selector for this
reference-shape presentation experiment.

Choose a concise name consistent with existing diagnostic CLI conventions.

The selector must require:

- --level 7
- --atari-tube-prototype

It must reject unsupported or conflicting use explicitly.

Do not make the experiment the default.

Do not write the diagnostic option into gameplay recording metadata.

FACTOR-128 DIAGNOSTIC

The existing conditional RN=$000F factor-128 diagnostic must remain separate
and unchanged.

Do not combine the factor-128 experiment with this presentation diagnostic by
default.

Unless current option architecture strongly establishes otherwise, reject
simultaneous use of the factor-128 diagnostic and this new presentation
diagnostic so the manual comparison changes only one hypothesis at a time.

Do not reopen RN.high or quotient behavior in this task.

IMPLEMENTATION BOUNDARY

Apply the candidate at the direct-Atari prototype's final presentation mapping
boundary, after AtariCoordinateProjector and AtariTubeVectorListBuilder have
produced the near and far endpoint sets.

Do not modify AtariCoordinateProjector.

Do not modify record-06 coordinates or metadata.

Do not modify the near/far builder topology.

Do not modify the existing point or segment model merely to encode
screenshot-derived geometry.

Prefer a small, explicit diagnostic mapping arrangement using the existing
AtariDvgViewportMap or a minimal diagnostic wrapper at the same presentation
boundary.

Do not create a second general projection architecture.

NEAR AND FAR MAPPING

The experiment must implement:

- one shared horizontal-versus-vertical gain relationship for both endpoint
  sets;
- one base translation for the near endpoint set;
- one additional translation applied only to the far endpoint set.

The same mapped far endpoint must be used consistently for:

- the far rim;
- and the far end of every corresponding rail.

The same mapped near endpoint must be used consistently for:

- the near rim;
- and the near end of every corresponding rail.

Do not bend, subdivide, curve, or independently transform rails.

Rails must remain straight segments connecting mapped Near[i] to mapped Far[i].

Do not move individual points independently.

VISUAL-DIAGNOSTIC STARTING VALUES

Use the completed investigation's smallest supported diagnostic family as the
basis for one bounded initial candidate.

The investigation reported:

- shared horizontal compression relative to vertical;
- diagnostic relative X/Y gain approximately 0.73 to 0.77;
- a far-set-specific translation equivalent to approximately (+27,+37) in the
  480x640 reference-raster fitting frame after common gain and near alignment.

Do not copy the reference-raster pixel values blindly into the C# logical
coordinate system.

Instead:

1. Reconstruct the investigation's transformation-space relationship from the
   report and current C# mapping.
2. Convert the far-set relative translation into the correct coordinate space
   for the presentation boundary where it will be applied.
3. Document the conversion.
4. Add tests proving the converted values correspond to the intended diagnostic
   mapping.
5. Fail closed if the conversion cannot be justified from the report and
   current mapping.

Choose one initial shared aspect value within the diagnostically supported
range. Prefer the investigation's fitted common-gain result rather than an
arbitrary rounded value when it can be translated cleanly into the current map.

The chosen values remain visual diagnostic calibration values only.

Do not expose broad free-form tuning controls in this milestone unless one
small explicit diagnostic value is technically necessary for manual testing.

The principal objective is to produce one reproducible candidate, not an
unbounded calibration interface.

STRICT PROHIBITIONS

Do not change:

- record-06 X/Y bytes;
- record-06 metadata;
- record selection;
- near position p=$10;
- far position p=$F0;
- quotient behavior;
- sign restoration;
- base handling;
- coordinate width or packing;
- point count;
- point order;
- rim adjacency;
- rail pairing;
- open/closed state;
- gameplay geometry;
- lanes;
- movement;
- collision;
- timing;
- entities;
- score;
- lives;
- RNG;
- recording/playback semantics;
- accepted Levels 1-6 presentation;
- Levels 8-9 presentation;
- the existing --atari-presentation path;
- Approach;
- exit presentation;
- or the separate Blaster-exit defect.

Do not add per-point offsets.

Do not derive replacement geometry from screenshot pixels.

Do not change the general portrait viewport or HUD layout.

Do not apply the diagnostic outside the Level 7 direct-Atari prototype.

TESTING REQUIREMENTS

Add focused automated coverage proving at minimum:

1. The existing direct-Atari prototype control is unchanged when the new option
   is absent.
2. The new selector requires --level 7 and --atari-tube-prototype.
3. Invalid contexts fail explicitly.
4. The new selector remains disabled by default.
5. The diagnostic uses one shared anisotropic gain for near and far endpoint
   sets.
6. The diagnostic applies the additional translation only to far endpoints.
7. The same transformed Near[i] is used by its rim edges and rail.
8. The same transformed Far[i] is used by its rim edges and rail.
9. Every rail remains Near[i] to Far[i].
10. Both rims preserve their existing cyclic adjacency and closure.
11. Record-06 source values and metadata remain unchanged.
12. The projector outputs remain unchanged.
13. The builder's logical point/segment structure remains unchanged.
14. The existing factor-128 diagnostic remains unchanged.
15. Conflicting simultaneous diagnostics are rejected if required by the final
    option design.
16. Existing --atari-presentation behavior remains unchanged.
17. Ordinary option-off behavior remains unchanged.
18. Genuine recording/playback prototype conflicts remain protected.
19. No gameplay state is read or changed by the diagnostic mapping beyond the
    existing Level 7/state gate.
20. The diagnostic conversion from reference-fit space to C# presentation
    space is exact and documented by tests.

Use exact expected point/vector values for representative near and far points.

Do not use screenshot pixel matching as an automated production test oracle.

Tests may verify the explicit diagnostic mapping selected for this candidate,
but must classify those expected values as DEVELOPMENT-ONLY visual-calibration
values.

VALIDATION

Run bounded focused tests for:

- option parsing and conflicts;
- direct-Atari prototype mapping;
- near/far endpoint transformation;
- vector topology preservation;
- record/projector/builder invariants.

Run relevant targeted regressions for:

- Level 7;
- direct-Atari prototype;
- existing factor-128 diagnostic;
- --atari-presentation;
- display configuration;
- CLI diagnostics;
- recording/playback;
- rendering selection;
- and option-off behavior.

Then run:

- the full test suite with an explicit hang timeout;
- a Release solution build;
- git diff --check.

Do not weaken tests.

Canceled, timed-out, blocked, or incomplete commands are not passes.

If Windows security blocks a required artifact, follow the standing project
security policy and stop with an explicit BLOCKED or PARTIAL report rather
than weakening security or retrying the same artifact indefinitely.

Do not launch the interactive game.

REPOSITORY SAFETY

Before project changes, record:

- branch;
- HEAD;
- upstream/tracking state;
- index state;
- tracked worktree changes;
- relevant untracked state.

Preserve all pre-existing candidate work and protected material.

Do not:

- reset;
- restore;
- clean;
- stash;
- rebase;
- switch branches;
- stage;
- commit;
- or push.

Do not absorb unrelated changes.

AI-LOGS

Follow the current .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired implementation/diagnostic report under AI-Logs/Reports/.

Use descriptive date-prefixed paired filenames.

A chat response is not a substitute for the report file.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide:

1. The exact control command.
2. The exact new reference-shape diagnostic command.
3. The exact factor-128 command for identification only, clearly stating that
   it is a separate experiment and should not be combined with this A/B test.
4. A brief explanation of what differs between the control and new experiment.
5. The specific manual comparison target:
   - outer lobe and notch proportions;
   - near-rim shape;
   - far-rim relative position;
   - rail direction and convergence;
   - overall normalized correspondence to the supplied reference shape.
6. Any known limitation of the initial candidate.

Do not ask Neal questions during execution.

Do not pause for manual input or visual acceptance.

Manual acceptance remains PENDING when the task finishes.

EXPECTED REPORT

The paired report must include:

- outcome and acceptance status;
- files changed;
- exact new CLI selector;
- exact option constraints and conflicts;
- exact presentation mapping added;
- chosen shared X/Y gain;
- converted far-set relative translation;
- coordinate space in which each transform is applied;
- derivation of the conversion from the investigation's reference-raster fit;
- evidence classification;
- confirmation that fitted values remain DEVELOPMENT-ONLY visual diagnostics;
- representative transformed near/far coordinates;
- focused test results;
- targeted regression results;
- full-suite result;
- Release-build result;
- git diff --check result;
- confirmation that record data, projection, builder topology, gameplay,
  existing --atari-presentation, factor-128 diagnostic, accepted Levels 1-6,
  and Levels 8-9 were not modified;
- starting and ending Git/worktree state;
- exact manual A/B commands;
- and manual acceptance status as PENDING.

Do not stage, commit, or push.

The task ends when the validated, opt-in DEVELOPMENT-ONLY reference-shape
presentation candidate is ready for Neal's manual comparison.

TEMPEST_LEVEL7_REFERENCE_SHAPE_PRESENTATION_DIAGNOSTIC