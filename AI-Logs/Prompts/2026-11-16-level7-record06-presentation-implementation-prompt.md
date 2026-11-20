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

Promote the successful Level 7 record-06 reference-shape presentation
diagnostic into a bounded Level 7 direct-Atari presentation implementation
candidate for Neal's manual acceptance.

This milestone is based on Neal's manual diagnostic result:

"The reference-shape diagnostic looks pretty good."

Neal has also explicitly decided:

- keep this candidate;
- stop investigating whether Atari record 06 itself is wrong;
- proceed with Level 7 implementation.

This is implementation of the Level 7 presentation candidate only.

It is NOT authorization to retune record-06 geometry, reopen its provenance, or
redesign the Atari projection pipeline.

CORE IMPLEMENTATION DECISION

Atari record 06 remains the authoritative Level 7 tube-data basis.

Preserve unchanged:

- record-06 X/Y bytes;
- record-06 metadata;
- record selection;
- point count;
- point order;
- closed-rim status;
- near/far point identity;
- cyclic near-rim connectivity;
- cyclic far-rim connectivity;
- same-index Near[i] / Far[i] rail pairing;
- AtariCoordinateProjector behavior;
- near position p=$10;
- far position p=$F0.

Do not investigate whether record 06 is wrong.

Do not generate replacement Level 7 geometry.

Do not derive new point coordinates from screenshots.

PRESENTATION DECISION

Use the already-implemented reference-shape diagnostic presentation family as
the Level 7 implementation candidate:

- shared anisotropic X/Y presentation gain;
- far-endpoint-set-specific translation;
- one consistent mapped Near[i] used by near-rim edges and corresponding rail;
- one consistent mapped Far[i] used by far-rim edges and corresponding rail;
- straight rails;
- unchanged point topology.

The current diagnostic uses:

- horizontal aspect approximately 0.7330435;
- presentation scale 0.345;
- near logical anchor (300,400);
- far logical anchor (333.75,446.25), equivalent to the existing
  DEVELOPMENT-ONLY far relative translation (+33.75,+46.25).

These values originated in bounded screenshot-based visual diagnosis and were
manually judged by Neal as producing a Level 7 tube that "looks pretty good."

Do not claim these values are ROM-BACKED or Atari hardware constants.

For this implementation milestone, treat them as a PROJECT TRANSLATION /
VISUALLY CALIBRATED PRESENTATION CANDIDATE pending final manual acceptance.

Do not retune the values during this task unless the current implementation
cannot faithfully preserve the manually tested candidate. If a technical
problem prevents exact preservation, STOP and report rather than choosing new
visual values autonomously.

PROMOTION, NOT REIMPLEMENTATION

Reuse the existing tested diagnostic implementation wherever practical.

Do not rewrite working diagnostic mathematics merely to make the code look
cleaner.

Refactor only as narrowly necessary to turn the successful diagnostic path into
the Level 7 implementation candidate cleanly and maintainably.

Preserve the existing control/diagnostic capabilities when useful for
regression or future investigation, but avoid unnecessary permanent duplicate
architecture.

Clearly separate:

- Atari record/projection data;
- Level 7 presentation translation;
- DEVELOPMENT-ONLY diagnostics.

Do not embed screenshot-derived presentation values into the record catalog or
projection mathematics.

LEVEL 7 SCOPE

Apply this implementation only to Level 7 in the direct-Atari / Atari
presentation architecture appropriate to the current candidate.

Do not generalize these calibrated presentation values to other tube records.

Do not assume Levels 8-16 require the same anisotropic gain or far-set
translation.

Future levels must be evaluated from their own evidence and manual presentation
results.

GAMEPLAY PROTECTION

Do not change any Level 7 gameplay behavior.

Specifically preserve:

- production Level 7 model geometry;
- lanes;
- topology;
- enemy spawning;
- Tankers;
- Flippers;
- Spikers;
- Spike behavior;
- enemy firing;
- movement;
- collisions;
- completion logic;
- Super Zapper behavior;
- scoring;
- lives;
- timing;
- RNG;
- recording/playback behavior.

This task is presentation-only.

ACCEPTED PRESENTATION PROTECTION

Do not reopen or alter accepted Levels 1-6 presentation.

Do not alter Levels 8-9 presentation candidates.

Do not alter accepted shared vertical placement or other previously frozen
presentation behavior outside the Level 7 candidate.

Do not investigate or modify the separate Blaster-exit rendering defect.

Do not alter Approach or transition behavior unless a compiler/test dependency
requires a strictly non-behavioral integration adjustment.

Do not retune HUD layout.

FACTOR-128 DIAGNOSTIC

The RN=$000F factor-128 diagnostic remains a historical DEVELOPMENT-ONLY
experiment.

Do not promote factor 128 as part of this Level 7 implementation.

Do not change quotient behavior.

Do not reopen RN.high.

Preserve the diagnostic if it remains useful and inexpensive to retain, but it
must not affect the production Level 7 candidate.

REFERENCE-SHAPE DIAGNOSTIC

The existing --atari-tube-reference-shape-diagnostic path was the manual test
vehicle for this candidate.

After promotion:

- avoid having two divergent copies of the same presentation transformation;
- if the diagnostic remains, it should either exercise the same underlying
  mapping or be clearly retained as a historical/control mechanism;
- do not allow a stale diagnostic copy to drift from the implemented Level 7
  candidate.

Choose the smallest maintainable arrangement consistent with the current
architecture.

EVIDENCE CLASSIFICATION

Record 06 and its established structure retain their existing ROM/Atari
evidence classifications.

The anisotropic gain and far-set translation are not established Atari hardware
behavior.

For the implementation candidate classify them accurately as a deliberate
PROJECT TRANSLATION / visually calibrated presentation choice, with Neal's
manual diagnostic result recorded as manual evidence that the candidate is
promising.

Do not call Level 7 finally accepted yet.

Final visual acceptance remains Neal's decision after implementation and
manual testing.

IMAGE SCOPE

No image analysis is required for this implementation task.

Do not search for, enumerate, open, inspect, or analyze repository screenshots,
images, video frames, contact sheets, or unknown visual assets.

The candidate values have already been established by the completed diagnostic
investigation and Neal's manual test.

Do not repeat screenshot fitting.

CODE QUALITY

Keep the implementation small and maintainable.

Add concise comments where necessary explaining:

- record 06 remains unchanged;
- the Level 7 presentation mapping occurs after Atari projection;
- the calibrated anisotropic gain/far translation are a PROJECT TRANSLATION,
  not ROM geometry or proven Atari hardware behavior.

Do not narrate obvious code.

Do not create a general per-level arbitrary fitting system unless the existing
architecture already requires one.

TESTING REQUIREMENTS

Add or update focused tests proving at minimum:

1. Level 7 uses record 06 unchanged.
2. Level 7's record metadata remains unchanged.
3. Projector output remains unchanged.
4. Near p=$10 and far p=$F0 remain unchanged.
5. Point ordering remains unchanged.
6. Near/far same-index pairing remains unchanged.
7. Both cyclic rims remain unchanged.
8. Level 7 presentation candidate uses the accepted candidate anisotropic gain.
9. Level 7 presentation candidate uses the accepted candidate far-set
   translation.
10. Near-rim and rail near endpoints use the same mapped points.
11. Far-rim and rail far endpoints use the same mapped points.
12. Rails remain straight Near[i]-to-Far[i].
13. No per-point corrections exist.
14. Levels 1-6 presentation selection remains unchanged.
15. Levels 8-9 selection remains unchanged.
16. Existing --atari-presentation regressions remain protected.
17. Existing recording/playback behavior remains protected.
18. Factor-128 remains opt-in DEVELOPMENT-ONLY behavior and does not affect
   the Level 7 candidate.
19. Ordinary option-off/legacy behavior remains unchanged.
20. Existing gameplay Level 7 tests remain unchanged and passing.

Do not use screenshot pixels as automated acceptance or production test data.

VALIDATION

Run bounded focused tests for the Level 7 record/projection/presentation
candidate.

Run relevant targeted regressions covering:

- Level 7;
- Levels 1-9 presentation selection;
- direct-Atari prototype;
- Atari-presentation;
- CLI diagnostics;
- factor-128 isolation;
- recording/playback;
- display configuration;
- rendering selection.

Then run:

- the full solution test suite with explicit hang-timeout protection;
- Release solution build;
- git diff --check.

Do not weaken valid tests.

Do not treat canceled, blocked, timed-out, stalled, or incomplete commands as
passes.

Follow the project's Windows-security and bounded-command policies.

Do not launch the interactive game.

DOCUMENTATION

Do not mark Level 7 presentation as finally accepted in durable behavior
documentation yet.

This implementation candidate still requires Neal's manual acceptance.

Record the work and pending manual status in the required AI report.

Only update durable project documentation if an existing candidate-status entry
must be kept technically accurate, and do not convert pending manual acceptance
into acceptance.

REPOSITORY SAFETY

Record starting branch, HEAD, upstream, index, tracked worktree, and relevant
untracked state.

Preserve all pre-existing work.

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

This milestone stops before Git closeout.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired implementation report under AI-Logs/Reports/.

The report file is required even if the same information is returned in chat.

Do not stage or commit AI-Logs.

MANUAL HANDOFF

After automated validation, STOP.

Do not launch the interactive game.

Provide the exact command Neal should run to test the implemented Level 7
candidate.

Prefer testing through the intended Level 7 implementation path rather than a
special diagnostic selector if promotion makes that appropriate.

State clearly:

- which command is the implementation candidate;
- whether --atari-tube-reference-shape-diagnostic remains and what purpose it
  now serves;
- whether the old prototype control remains available;
- that final Level 7 visual acceptance is PENDING.

Do not ask Neal questions during execution.

Do not pause for manual input.

EXPECTED REPORT

The paired implementation report must include:

- exact implementation outcome;
- files changed;
- how the diagnostic candidate was promoted/reused;
- provenance classification;
- confirmation that record 06 remains unchanged;
- confirmation that projector mathematics remains unchanged;
- exact presentation mapping used;
- whether diagnostic/control options remain;
- focused validation;
- targeted regression validation;
- full-suite validation;
- Release build;
- git diff --check;
- starting and ending Git state;
- protected accepted-level status;
- exact manual test command;
- manual acceptance status PENDING.

Do not stage, commit, or push.

The task ends with a validated Level 7 presentation implementation candidate
ready for Neal's manual acceptance.

TEMPEST_LEVEL7_RECORD06_PRESENTATION_IMPLEMENTATION