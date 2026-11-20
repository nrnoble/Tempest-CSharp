FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain that the supplied user task appears
incomplete.


TASK

Continue the focused Level 8 correction using Neal's newly supplied visual
orientation evidence.

Correct:

1. The display orientation of the already-correct Level 8 tube shape.
2. Player firing during active tube exit.
3. Player-shot movement and Spike erosion during active tube exit.
4. Blaster-versus-Spike collision during active tube exit.

Repository:

C:\Users\nrnob\source\repos\Tempest

Committed baseline beneath the current unstaged Level 8 candidate:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

The current working tree contains the existing unstaged Level 8 candidate.

Preserve the candidate.

Do not undo, discard, restore, restart, or reimplement Level 8.

Do not stage, commit, or push.

Leave the corrected candidate unstaged for Neal's continued manual acceptance.


REQUIRED REPORTS

Read these reports before editing:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

AI-Logs/Reports/2026-11-08-level8-implementation-recovery-report.md

AI-Logs/Reports/2026-11-08-level8-orientation-exit-spike-blocked-report.md

The blocked report established:

- the local ROM/code evidence did not independently determine physical screen
  orientation;
- active arcade exit processes player movement, firing, player shots,
  shot-versus-Spike interaction, and Blaster-versus-Spike collision;
- the C# standard ExitZoom path currently omits firing, shot movement, and
  Spike collision;
- the AVOID SPIKES interstitial and active Spike hazards are separate concepts.

Preserve those findings.


NEW AUTHORIZED VISUAL EVIDENCE

Neal has now supplied the visual evidence needed to resolve the remaining
orientation ambiguity:

UserData\Screenshots\Level8-orientation.png

This screenshot is authorized only for determining the displayed orientation
of the already-correct Level 8 geometry.

Read the screenshot directly from that path.

Do not move, rename, edit, resize, copy, stage, or commit it.


NEAL'S GEOMETRY CLARIFICATION

The current Level 8 tube shape is drawn correctly.

The recovered shape and relative geometry do NOT need to change.

This is a display-orientation issue only.

Hard constraints:

- preserve all 16 ROM-derived vertices;
- preserve every relative distance and angle;
- preserve point order unless an equivalent topology-preserving orientation
  mapping specifically requires a corresponding order interpretation;
- preserve 15 playable lanes;
- preserve open topology;
- preserve the two open endpoints;
- preserve the absence of a closing edge;
- do not edit individual vertices to resemble the screenshot;
- do not redraw the tube by eye;
- do not change scale or proportions as part of orientation correction;
- do not use the screenshot to tune unrelated presentation details.

The tube's current shape is accepted for purposes of this correction.


SCREENSHOT ORIENTATION EVIDENCE

The supplied screenshot shows the correct arcade display orientation:

- the two open endpoints are at the upper left and upper right;
- the two long sides extend downward and inward;
- the narrow short connecting segment is at the bottom;
- the tube remains open across the upper portion;
- the shape is presented upright in this orientation.

Use this evidence to choose among the transform possibilities left unresolved
by the prior investigation.

Do not assume the operation in advance.

Mathematically compare the current C# Level 8 coordinates/rendered orientation
with the screenshot and determine whether the minimum correct whole-shape
operation is:

- Y reflection;
- X reflection;
- 180-degree rotation;
- axis swap;
- quarter-turn;
- another fixed rigid transform;
- or an equivalent display-space mapping.

Reject transforms that do not match the supplied evidence.

Document the exact selected transform and why it is the minimum correct one.


ORIENTATION CORRECTION BOUNDARY

Correct orientation using one coherent whole-tube transformation.

Prefer:

- a Level 8 definition-space transform; or
- a narrowly scoped presentation transform associated with the Level 8
  definition.

Do not alter global rendering orientation if that would rotate or reflect
accepted Levels 1-7.

Do not manually replace all 16 coordinates with visually tuned values if one
mathematical transform expresses the correction.

The final Level 8 definition must remain traceable to the original ROM-derived
coordinates plus one documented orientation transform.


TOPOLOGY AFTER ORIENTATION

After applying the transform, verify:

- there are still 16 vertices;
- there are still exactly 15 playable lanes;
- lane 15 remains nonexistent;
- no vertex-15-to-vertex-0 edge is created;
- both endpoints remain open;
- Blaster movement still clamps at both endpoints;
- enemies do not cross the open gap;
- Tanker cargo endpoint correction remains valid;
- Spikes remain mapped to the intended playable lanes;
- collision lane identities remain internally consistent;
- directed transitions still use the transformed geometry correctly.

If visual left/right lane numbering changes because of the rigid transform,
preserve consistent behavior across player movement, enemies, Spikes,
collisions, recording, and playback.

Do not create ad hoc lane remapping unless mathematically required.


ROM FILE SAFETY

Local ROM authorities remain:

Reference/arcade/Tempest Rom Source code.md

Reference/arcade/roms.rev2/

The binary ROM files are read-only reference inputs.

Do not edit, move, rename, patch, stage, or commit them.


EXIT DEFECTS: ESTABLISHED ROM BEHAVIOR

The blocked investigation established from the local Rev 2 ROM and annotated
listing that the active arcade exit state:

1. moves the player;
2. executes the tube-exit movement/collision path;
3. executes ordinary player firing;
4. moves player shots;
5. processes shot-versus-Spike interaction;
6. checks the exiting Blaster against the Spike in the current segment;
7. enters the death path if the Blaster crosses a remaining Spike.

The Level 8 condition that skips the earlier AVOID SPIKES interstitial does not
disable firing, shots, Spike erosion, or active exit collision.

Treat this as ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED behavior.


CURRENT C# EXIT DEFECT

The blocked investigation established that:

- Level 8 correctly uses Standard exit policy;
- BeginExit clears existing shots;
- standard ExitZoom calls exit movement and player movement;
- standard ExitZoom does not call player firing;
- standard ExitZoom does not update player shots;
- standard ExitZoom does not check active Blaster-versus-Spike collision;
- the specialized Spike-exit path performs those operations, but it is coupled
  to the AVOID SPIKES policy;
- this coupling is incorrect because the interstitial policy and active Spike
  hazard are separate arcade behaviors.

Fix this shared architectural seam.

Do not change Level 8 to the AVOID SPIKES policy merely to gain access to the
specialized update path.


ACTIVE EXIT FIRING

During active tube exit:

- Fire input must remain operational;
- normal accepted shot-slot limits apply;
- normal accepted cooldown applies;
- normal shot sound behavior applies;
- player shots are created in the Blaster's current exit lane;
- shots advance during exit;
- holding Fire must not create more shots than normal rules allow;
- shots must remain deterministic;
- recording/playback must reproduce the same exit firing.

Do not alter ordinary Playing-state firing.

Do not add unlimited or exit-specific ammunition.


EXISTING SHOTS AT EXIT START

The current implementation clears existing shots when exit begins.

Trace the arcade and current project behavior carefully.

Change this only if the local ROM evidence establishes that pre-existing shots
must survive into active exit and the change is required for fidelity.

The primary confirmed requirement is that new shots can be fired and moved
during active exit.

Do not broaden the correction unnecessarily.


SHOT MOVEMENT DURING EXIT

During active ExitZoom:

- update active player shots through the established shot simulation;
- preserve normal shot speed rules unless the ROM/project already has an
  exit-specific rule;
- preserve shot-slot lifetime/expiry;
- preserve lane association;
- preserve Spike-slow/erosion behavior where applicable;
- preserve deterministic simulation boundaries.

Do not run ordinary enemy-wave simulation that should be inactive during exit.


SHOT-VERSUS-SPIKE EXIT INTERACTION

During exit, a player shot traveling in the Spike's lane must interact with the
Spike through the appropriate existing Spike erosion rules.

Requirements:

- a shot in the same lane can shorten/erode the Spike;
- a shot in another lane cannot affect it;
- exit lane changes affect which Spike the subsequent shot targets;
- lane 15 is never accessed on open Level 8;
- shot erosion remains bounded by the normal accepted shot rules;
- successful Spike removal/shortening is deterministic.

Do not rewrite all Spike erosion across Levels 1-7 unless the actual defect is
in a shared method and the correction is demonstrated to preserve accepted
behavior.


BLASTER-VERSUS-SPIKE EXIT COLLISION

During active tube exit:

- compare the Blaster's exit lane and along/depth position with the remaining
  Spike in that same lane;
- if the Blaster reaches/crosses a blocking Spike, trigger the existing player
  death path before successful tube exit;
- a Spike in another lane must not destroy the Blaster;
- a sufficiently shortened or cleared Spike must permit safe passage;
- changing lanes during exit changes the relevant Spike;
- collision must work on Level 8 endpoint lanes without invalid lane access;
- no lane 15 may be read;
- successful exit must occur only if the Blaster survives.

Use existing depth/height conventions consistently.

Do not compare incompatible ROM-along and C#-height values without a documented
translation.


EXIT DEATH AND RE-ENTRY

If the Blaster is destroyed by a Spike during exit:

- use the appropriate existing death presentation;
- preserve lives decrement;
- preserve score;
- preserve valid Level 8 reconstruction;
- preserve generic Level 8 death/re-entry behavior;
- return to gameplay without stale exit state;
- avoid duplicated or missing enemy/cargo obligations;
- avoid invalid lane state;
- do not advance to Level 9;
- do not incorrectly complete Level 8.

Preserve accepted earlier-level behavior.


AVOID SPIKES INTERSTITIAL

Do not enable the pre-Level-8 AVOID SPIKES interstitial on Level 8.

Level 8 remains Standard exit policy for presentation.

The correction must separate:

- whether the interstitial is shown; from
- whether active exit firing, shots, Spike erosion, and Spike collision run.

Document this architecture clearly.


SCOPE ACROSS LEVELS

Determine whether active exit firing and Spike hazards are general arcade exit
behavior for all Spike-bearing levels.

If the source-backed correction belongs in the general active exit path,
implement it generally and protect accepted Levels 1-7 with regression tests.

Do not add Level-8-only branching when the defect is a shared exit-state seam.

Do not alter an accepted earlier-level presentation policy without evidence.


REQUIRED ORIENTATION TESTS

Add or update tests proving:

1. Original 16 ROM-derived vertices remain represented through the documented
   transform.
2. The selected rigid transform is explicit and deterministic.
3. Relative geometry and proportions are unchanged.
4. Point ordering/topology remains coherent.
5. Level 8 remains open.
6. Level 8 still has exactly 15 playable lanes.
7. No closing edge appears.
8. The endpoints occupy the intended upper display positions under the
   screenshot-supported orientation.
9. The short connecting segment occupies the intended lower display position.
10. Blaster traversal and spinner direction remain coherent.
11. Enemy and Spike lane identities remain coherent.
12. Existing Levels 1-7 geometry is unchanged.
13. Recording/playback reconstructs the corrected Level 8 geometry
    deterministically.

Prefer geometry assertions over brittle pixel screenshots.


REQUIRED EXIT-FIRING TESTS

Add or update tests proving:

1. Fire input is accepted during active ExitZoom.
2. A player shot can be created during exit.
3. Normal shot-slot limits remain enforced.
4. Normal cooldown remains enforced.
5. Shot sound/event behavior remains correct.
6. Shots advance during exit.
7. Shots use the current exit lane.
8. Lane switching affects subsequent shot lane appropriately.
9. Holding Fire does not bypass normal constraints.
10. Ordinary Playing-state firing remains unchanged.
11. Recording/playback reproduces exit firing deterministically.


REQUIRED EXIT-SPIKE TESTS

Add or update tests proving:

1. Exit shots interact with a Spike in the same lane.
2. Exit shots do not affect Spikes in other lanes.
3. Exit shots can shorten/clear the relevant Spike.
4. A Blaster reaching a blocking Spike is destroyed.
5. A Spike in another lane does not destroy the Blaster.
6. A sufficiently shortened/cleared Spike permits safe exit.
7. Exit lane switching changes the relevant collision lane.
8. Open endpoint lanes are valid.
9. Lane 15 is never accessed.
10. Exit Spike death re-enters Level 8 correctly.
11. Exit Spike death does not complete the level.
12. Successful exit still reaches terminal Level 8 behavior.
13. Level 8 does not show the earlier AVOID SPIKES interstitial.
14. Accepted earlier Spike-bearing level behavior remains correct.


REQUIRED REGRESSION COVERAGE

Run focused and expanded tests covering:

- Level 8 geometry;
- orientation transform;
- open topology;
- endpoint clamping;
- spinner movement;
- tube rendering;
- lane validation;
- player input;
- firing;
- shot capacity;
- shot cooldown;
- shot movement;
- Spike erosion;
- exit movement;
- exit lane control;
- exit collision;
- player death;
- Level 8 re-entry;
- Level 8 completion;
- Level 7-to-8 progression;
- Super Zapper;
- direct startup;
- recording/playback;
- headless playback;
- pause/forward/reverse;
- screenshot capture;
- `--speed`;
- accepted Levels 1-7.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Diagnose and correct candidate-caused failures.

Do not launch the interactive game.


DOCUMENTATION

Update durable documentation after the correction is established:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

README.md only if user-facing documentation genuinely requires a change.

Record:

- Neal's first blind Level 8 result;
- ROM-derived tube shape judged substantially correct;
- geometry frozen;
- orientation corrected using the authorized screenshot;
- exact rigid transform used;
- exit firing behavior;
- exit shot/Spike behavior;
- exit Blaster/Spike collision;
- separation of Avoid Spikes presentation from active exit hazards;
- automated validation;
- continued MANUAL ACCEPTANCE PENDING status.

Do not claim complete Level 8 acceptance.


SCREENSHOT SAFETY

The authorized screenshot is:

UserData\Screenshots\Level8-orientation.png

It is local evidence only.

Do not:

- edit it;
- overwrite it;
- rename it;
- move it;
- stage it;
- commit it;
- copy it into tracked documentation;
- use it as a pixel template.

Verify it remains unchanged.


OTHER LOCAL-FILE SAFETY

Do not modify, stage, commit, move, or delete:

- Reference/arcade/roms.rev2/
- UserData/
- gameplay recordings;
- Videos;
- screenshots;
- unrelated local files;
- existing AI reports.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW orientation/exit correction report under:

AI-Logs/Reports/

3. Creating these new ignored records is authorized and expected.

4. Existing investigation, blocked, implementation, and recovery reports remain
   historical records and must not be rewritten.

5. AI-Logs/ is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place the correction report under TEMP/ or Temp/.


CORRECTION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Prior reports reviewed.
3. Neal's first blind-test observations.
4. Authorized screenshot path and safety verification.
5. Frozen ROM-derived geometry.
6. Current orientation compared with visual evidence.
7. Transform candidates considered.
8. Exact selected rigid transform.
9. Why competing transforms were rejected.
10. Topology/lane consistency after transform.
11. ROM active-exit trace.
12. C# active-exit defect.
13. Firing correction.
14. Shot movement correction.
15. Shot-versus-Spike correction.
16. Blaster-versus-Spike correction.
17. Exit death/re-entry behavior.
18. Avoid Spikes policy separation.
19. Files changed.
20. Tests added/updated.
21. Focused validation results.
22. Expanded regression results.
23. Full-suite result.
24. Build result.
25. `git diff --check`.
26. Levels 1-7 regression evidence.
27. ROM/screenshot/local-file safety.
28. Final Git state.
29. Manual retest command.
30. Focused manual checklist.
31. Remaining Level 8 acceptance work.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the corrected Level 8 candidate unstaged.

Stop after automated validation and manual-test handoff.

Do not begin unrelated tuning.

Do not begin Level 9.


MANUAL RETEST COMMAND

Provide Neal:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10


FOCUSED MANUAL RETEST CHECKLIST

Ask Neal to verify:

1. The tube shape remains unchanged.
2. The tube orientation now matches the supplied arcade screenshot.
3. Both open endpoints appear in the correct upper positions.
4. The short connecting segment appears in the correct lower position.
5. No false closing edge appears.
6. Spinner direction remains intuitive.
7. Blaster movement still clamps at both endpoints.
8. Ordinary Level 8 gameplay remains stable.
9. Fire works during active tube exit.
10. Exit shots travel down the selected lane.
11. Exit shots shorten/clear the selected lane's Spike.
12. Shots do not affect another lane's Spike.
13. The Blaster is destroyed when reaching a remaining blocking Spike.
14. A sufficiently shortened or cleared Spike allows successful exit.
15. Lane switching during exit changes the relevant Spike lane.
16. Exit death/re-entry behaves normally.
17. Level 8 still reaches its clean terminal boundary after a safe exit.
18. Level 8 does not show the earlier AVOID SPIKES interstitial.
19. No obvious regression appears in Levels 1-7 if naturally encountered.


MANUAL ACCEPTANCE STATUS

Level 8 remains partially tested.

Do not claim full manual acceptance after this correction.

This focused retest is only the next acceptance gate.


STOP

After automated validation and manual-test handoff:

STOP.

Do not launch the game.

Do not stage.

Do not commit.

Do not push.

Do not implement Level 9.

TEMPEST_LEVEL8_ORIENTATION_EXIT_CORRECTION_20261108