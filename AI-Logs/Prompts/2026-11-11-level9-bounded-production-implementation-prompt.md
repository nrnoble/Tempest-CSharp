FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: BOUNDED LEVEL 9 PRODUCTION IMPLEMENTATION AND AUTOMATED VALIDATION

Implement the first bounded production Level 9 candidate according to the completed Level 9 status and ROM preimplementation investigation.

This task includes:

- Level 9 production implementation;
- tightly scoped automated tests;
- bounded automated validation;
- exact manual launch and acceptance instructions for Neal.

This task does NOT include:

- manual arcade/MAME acceptance by the agent;
- Level 9 closeout;
- commit;
- push;
- Level 10 implementation;
- changes to accepted Levels 1-8.


STARTING CHECKPOINT

The required clean starting checkpoint is:

037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9

Commit subject:

Complete accepted Level 7 behavior

Before modifying anything, run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Expected:

- branch master tracking origin/master;
- local HEAD and origin/master both equal the checkpoint above;
- clean tracked worktree and index;
- no visible untracked project files.

If the checkpoint or worktree differs materially, STOP before editing and report the discrepancy.

Do not reset, restore, clean, checkout over changes, or discard anything.


REQUIRED IMPLEMENTATION SPECIFICATION

Read completely:

AI-Logs/Reports/2026-11-11-level9-status-rom-preimplementation-investigation-report.md

Treat that report as the authoritative implementation specification for this task.

Also read, only where directly relevant:

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

TEMPEST-BEHAVIOR-SPEC.md

TEMPEST-DEVELOPMENT-NOTES.md

Inspect the accepted current C# implementation and tests for Levels 7 and 8 as engineering architecture.

Do not copy Level 7 or Level 8 behavior into Level 9 merely because it is convenient.

Do not modify Levels 1-8.


EVIDENCE CLASSIFICATIONS

Maintain these evidence boundaries throughout code, tests, comments, and the implementation report:

REV-2-BINARY-CONFIRMED:
Raw Level 9 geometry, angle, topology, remap, and presentation-table bytes recovered from the protected local Rev. 2 source.

ROM-LISTING-BACKED:
Level parameters, enemy eligibility, movement programs, `$53` behavior, cargo boundary, reconstruction semantics, firing metadata, and progression behavior directly traced from the local ROM listing.

PROJECT TRANSLATION:
The approved coordinate conversion, Y reflection, candidate starting Spike-height conversion, C# representation of ROM concepts, and terminal Level 9 boundary.

PROJECT APPROXIMATION:
Any C# timing, speed, random-selection, visual, or lifecycle translation not established as literal ROM behavior.

MANUAL ACCEPTANCE NEEDED:
Tube presentation, starting Spike appearance, `$53` visible behavior, speeds, transition presentation, and overall gameplay fidelity pending Neal.

Do not promote a project translation or approximation into ROM-backed behavior.


SCOPE CONTROL

Implement only Level 9 and the smallest reusable seams needed to support it without affecting accepted earlier levels.

Do not:

- alter Level 5 because Level 5 also selects ROM `$53`;
- alter Level 7 movement, speed, firing, admission, reconstruction, or cargo;
- alter Level 8 geometry, `$19` movement, Spike presentation, wave policy, reconstruction, or accepted terminal behavior except the narrowly required progression link from Level 8 to Level 9;
- implement Level 10;
- introduce general difficulty selection;
- broadly refactor enemy classes;
- redesign recording formats;
- add unrelated diagnostics or UI;
- modify protected ROM/reference data;
- modify local recordings or TEMP material.


PART 1: LEVEL 9 GEOMETRY AND TOPOLOGY

Add the Level 9 production tube using ROM shape row 13.

Raw evidence:

lev_remap[8] = $0D

X:
10 10 30 30 50 50 70 70 90 90 B0 B0 D0 D0 F0 F0

Y:
90 70 70 50 50 30 30 10 10 30 30 50 50 70 70 90

Angle:
0C 00 0C 00 0C 00 0C 00 04 00 04 00 04 00 04 00

Presentation fields:

lev_scale = $0C
lev_y3d = $B0
lev_y2db:lev_y2d = $01:$40 = $0140
lev_open = $FF
lev_fscale2:lev_fscale = $04:$50 = $0250
tube-color raw index = $05

Requirements:

1. Preserve all 16 vertices in ROM order.
2. Configure the tube as open.
3. Expose exactly 15 playable lanes, numbered 0-14.
4. Do not create a closing edge from vertex 15 to vertex 0.
5. Player traversal must clamp at both open endpoints.
6. Enemy endpoint behavior must reverse appropriately rather than wrap.
7. Tanker cargo allocation at an endpoint must use valid adjacent playable lanes and must not create a child on a nonexistent closing lane.
8. Tube rendering must omit the closing edge.
9. Retain raw geometry/presentation evidence in tests or maintainable comments where appropriate.
10. Use the accepted coordinate-normalization and Y-reflection seam as a PROJECT TRANSLATION candidate.
11. Do not claim raw presentation bytes define literal C# pixels or RGB colors.
12. Make the smallest reasonable initial C# palette, scale, center, and vertical-placement choices consistent with accepted project presentation conventions.
13. Leave those visual choices explicitly pending Neal's manual acceptance.

Do not alter Level 8 open-topology behavior while adding Level 9.


PART 2: LEVEL 9 PRODUCTION CONFIGURATION

Add canonical Level 9 Medium/default configuration.

Required values:

Displayed level:
9

Generic pending supply:
27

Initial C# Eggs:
4

Generic active-entry gate:
6

Species limits:

Flipper minimum 0
Flipper maximum 5

Tanker minimum 1
Tanker maximum 2

Spiker minimum 2
Spiker maximum 4

Pulsar minimum 0
Pulsar maximum 0

Fuzzball/Fuseball minimum 0
Fuzzball/Fuseball maximum 0

Tanker cargo:
two Flippers per Tanker

Flipper movement program:
Level 9 `$53` policy where eligible

Enemy shot holdoff metadata:
56

Enemy shot capacity:
4

Starting Spikes:
one Spike on each playable lane 0-14

Candidate starting Spike height:
85.7142857

The candidate Spike height is a PROJECT TRANSLATION based on:

(0xF0 - 0xC0) * 400 / (0xF0 - 0x10)

Do not place a Spike on nonexistent lane 15.

Do not claim 85.7142857 is a literal arcade pixel height.

Ordinary bottom entrants must be Tankers and Spikers only.

Ordinary Flippers, Pulsars, and Fuseballs/Fuzzballs must not enter.


PART 3: LEVEL 9 WAVE SELECTION AND CONCURRENCY

Add Level 9-specific production wave selection or a safely isolated policy based on the accepted Level 8 architecture.

Requirements:

1. Satisfy the ROM minima:
   - two Spikers;
   - one Tanker;
   subject to capacity and availability.

2. Enforce:
   - maximum four active/committed Spikers;
   - maximum two active/committed Tankers;
   - maximum five active/committed Flippers, including latent Tanker cargo and released cargo as appropriate.

3. Enforce the six-unit ordinary generic active-entry gate.

4. Do not apply the ordinary six-unit entry gate as an extra gate on Tanker cargo release.

5. Tanker cargo may bring total active materialized enemies above six.

6. One ordinary Tanker consumes one generic pending unit.

7. Its two latent Flipper children do not consume additional generic pending supply.

8. Preserve cargo reservations so excessive Tanker commitments do not exceed the Flipper maximum.

9. Preserve deterministic testing and recording behavior.

10. If exact ROM post-minimum Spike-height/random-start selection is not already represented safely, use a tightly scoped deterministic PROJECT TRANSLATION for Level 9.

11. Do not change accepted Level 8 selection behavior to make Level 9 fit.

12. Clearly document any post-minimum selection approximation.

13. Preserve the existing final-pending same-unit Spiker-to-Tanker conversion behavior where compatible with the Level 9 ROM trace:
    - retain the original generic identity;
    - add two latent Flipper cargo identities;
    - do not add generic pending supply;
    - obey type/cargo constraints.


PART 4: EXACT LEVEL 9 `$53` FLIPPER BEHAVIOR

Implement `$53` as a Level 9-specific below-rim Flipper program.

Do not implement it as generic `CanFlip`.

Do not change Level 5 in this task.

Required observable semantics:

1. The Flipper begins from the Level 9 `$53` program state.

2. On the relevant update:
   - move along the current lane;
   - then inspect the live Spike associated with the Flipper's current lane.

3. The Spike predicate is true when:

   spike height >= Flipper along position

   including equality.

4. If the current lane has no live Spike or the Spike is destroyed:
   - treat the predicate as allowed, corresponding to the ROM's `$FF` substitution.

5. If the predicate is false:
   - do not begin a lane change;
   - continue climbing the same lane;
   - reevaluate after the next applicable along movement;
   - do not pause;
   - do not enter a separate captured/locked state.

6. If the predicate is true:
   - begin a normal directed lane transition.

7. During active transition continuation:
   - continue along-tube movement as required by `$53`;
   - do not retest the destination lane's Spike in the middle of the pivot.

8. After the pivot completes:
   - settle on the new lane;
   - perform the next along movement;
   - then inspect the new current lane's live Spike.

9. A shortened Spike must change the threshold at which the Flipper may begin changing lanes.

10. The condition is live and reevaluated, not latched at spawn.

11. `$53` itself contains:
    - no random route decision;
    - no explicit direction-toggle instruction.

12. Open-endpoint reversal remains a separate topology behavior.

13. Preserve common Flipper firing rules:
    - `$53` does not independently grant or suppress firing;
    - active-pivot firing suppression should follow the existing common/project policy.

Implement a maintainable level-selected seam so Level 9 can select `$53` without changing accepted movement on other levels.


PART 5: ORDINARY FLIPPERS AND TANKER CARGO

Ordinary bottom Flippers are ineligible on Level 9.

All Level 9 Flippers originate from Tanker cargo.

Implement cargo routing based on release depth.

ROM boundary:

- current Tanker ROM along position below `$20` selects `$07`;
- current Tanker ROM along position at or above `$20`, including equality, selects Level 9 `$53`.

Requirements:

1. Natural near-rim Tanker release uses `$07`.
2. Shallow player-shot release below `$20` uses `$07`.
3. Player-shot release at `$20` uses `$53`.
4. Player-shot release above `$20` uses `$53`.
5. Translate the ROM boundary through the existing accepted C# ROM-along/depth seam rather than inventing an unrelated threshold.
6. Add exact tests at `$1F`, `$20`, and `$21`.
7. The two children use distinct identities.
8. Children begin on opposing adjacent routes with opposing direction bits.
9. Open-end correction must keep both children on valid lanes.
10. `$07` children climb straight and do not execute `$53`.
11. `$53` children use their own current lane's live Spike state after materialization.
12. Do not modify accepted Level 7 or Level 8 cargo behavior.


PART 6: STARTING SPIKES

Initialize one Spike on each playable Level 9 lane 0-14.

Use candidate C# height:

85.7142857

Requirements:

1. No Spike exists on nonexistent lane 15.
2. Preserve distinct Spike objects per lane.
3. Player shots must continue changing the stored live Spike height used by `$53`.
4. Shortened/destroyed Spikes must affect later Level 9 Flipper decisions.
5. Preserve existing Spike destruction, erosion, collision, and rendering behavior unless a Level 9-specific seam is strictly necessary.
6. Do not retune accepted Spike presentation on Levels 4-8.
7. Treat the Level 9 height and visual presentation as pending Neal's manual acceptance.


PART 7: TANKER, SPIKER, FLIPPER, AND PROJECTILE SPEEDS

Do not claim a literal ROM-to-C# speed conversion.

ROM raw inputs are:

Flipper: $AF
Tanker: converted Flipper speed
Spiker: $C0
Enemy projectile: $C0
flip_top_accel: 2

Requirements:

1. Do not copy Level 7's accepted Tanker=Spiker speed relationship into Level 9.
2. Preserve accepted Levels 1-8 speed behavior.
3. Use the smallest existing C# speed policies needed for a playable Level 9 candidate.
4. Clearly classify these as PROJECT TRANSLATION or PROJECT APPROXIMATION.
5. Avoid creating a broad Level 9 speed-calibration subsystem in this implementation.
6. Keep future manual calibration possible through existing development/playback tools.
7. Do not alter global projectile speed based solely on raw `$C0`.

Neal will manually compare Level 9 speeds after automated validation.


PART 8: LEVEL 9 FIRING

Configure Level 9 firing using the existing firing architecture with Level 9-specific parameters where possible.

Required behavior/evidence:

- shot holdoff metadata: 56;
- simultaneous enemy-shot capacity: four;
- eligible species: Flippers, Tankers, and Spikers;
- Pulsars and Fuseballs/Fuzzballs absent;
- active-pivot Flippers do not fire under the common firing rule;
- new projectiles first move on the later applicable simulation step;
- Spikers can fire during later eligible ascent cycles rather than being permanently exhausted after one ascent.

Requirements:

1. Add a Level 9-specific repeated-cycle Spiker firing policy without altering accepted Level 7 behavior.
2. Reuse shared project projectile accounting, capacity, and chance infrastructure where safe.
3. Do not claim C# timing is a literal ROM conversion.
4. Add deterministic tests for Level 9 holdoff/capacity/configuration and repeated-cycle firing.
5. Do not change firing on Levels 1-8.


PART 9: DEATH AND GENERIC RECONSTRUCTION

Level 9 must use generic post-death reconstruction.

Requirements:

1. Combine unresolved:
   - active-below-rim obligations;
   - at-rim obligations;
   - pending generic obligations;
   subject to existing safe accounting.

2. Discard unresolved species/movement identity during reconstruction.

3. Rebuild fresh generic entries.

4. Preserve already resolved/terminal outcomes.

5. Drop latent cargo when a Tanker carrying it is lost.

6. Treat already released unresolved cargo as active obligations that return through generic reconstruction.

7. Do not restore an old `$53` program counter.

8. Any newly selected Level 9 Flipper must start fresh from the Level 9 `$53` program.

9. Preserve identity conservation internally:
   - no duplicate generic identity;
   - no duplicate cargo identity;
   - no terminal identity returning;
   - no negative supply;
   - idempotent repeated reset behavior.

10. Allow final-pending Spiker-to-Tanker conversion to occur again after reconstructed supply is consumed.

11. Use the accepted ledger architecture without altering reconstruction semantics for Levels 1-8.


PART 10: PROGRESSION, DIRECT START, AND TERMINAL BOUNDARY

Add Level 9 to the canonical production chain.

Requirements:

1. Natural Level 8 -> Level 9 progression.
2. Between-level approach using the accepted common presentation.
3. Carry score and lives.
4. Recharge/reset Super Zapper using accepted new-level behavior.
5. Direct:

   --level 9

6. Provisional Level 9 starting Blaster lane 7 as a PROJECT TRANSLATION.
7. Level 9 startup through generic recording metadata.
8. Visible playback support.
9. Headless playback support.
10. Level 9 becomes the current terminal supported level.
11. Completing Level 9 uses the existing terminal completion/restart policy.
12. Restart begins a fresh Level 1 campaign.
13. Level 10 remains unsupported.
14. Level 10 direct startup must be rejected.
15. Do not implement or prepare Level 10 production data.


PART 11: RECORDING AND PLAYBACK

Use existing recording version 2 and generic starting-level metadata.

No recording schema change is expected.

Add tests proving:

1. Level 9 direct recording startup is accepted.
2. Startup metadata restores Level 9.
3. Deterministic visible playback can start Level 9.
4. Headless playback can start Level 9.
5. Existing recordings for Levels 1-8 remain compatible.
6. Existing presentation-only `--speed` behavior remains independent of deterministic simulation state.
7. No Level 9-specific diagnostic metadata is added unless strictly necessary.

Do not create a permanent gameplay recording during this implementation task.


PART 12: AUTOMATED TESTS

Add focused Level 9 coverage.

At minimum test:

A. GEOMETRY

- exact raw X array;
- exact raw Y array;
- exact angle array;
- remap row `$0D`;
- open flag `$FF`;
- raw presentation bytes;
- 16 vertices;
- 15 playable lanes;
- no closing edge;
- no lane 15;
- player endpoint clamp;
- enemy endpoint reversal;
- renderer does not draw a closing chord.

B. CONFIGURATION

- displayed Level 9;
- generic supply 27;
- four initial Eggs;
- 15 starting Spikes;
- starting Spike height approximately 85.7143;
- Level 9 species limits;
- generic gate six;
- shot holdoff metadata 56;
- shot capacity four;
- Level 9 generic reconstruction enabled.

C. ADMISSION / CONCURRENCY

- minimum two Spikers and one Tanker;
- type maxima;
- ordinary Flipper/Pulsar/Fuzzball exclusion;
- six-unit ordinary gate;
- latent cargo reservation against Flipper maximum;
- released cargo permitted above ordinary gate;
- deterministic post-minimum selection;
- final-pending same-unit Spiker-to-Tanker conversion;
- no added generic supply during conversion.

D. `$53` MOVEMENT

- live Spike below-threshold case;
- equality case;
- above-threshold case;
- destroyed/absent Spike;
- shortened Spike;
- continued same-lane movement while predicate false;
- movement during active transition;
- no mid-pivot destination-lane test;
- destination-lane retest after completed transition and next along movement;
- no latched lock state;
- open-end reversal;
- no invalid wrap.

E. TANKER CARGO

- natural release uses `$07`;
- shallow shot release uses `$07`;
- raw `$1F` uses `$07`;
- raw `$20` uses `$53`;
- raw `$21` uses `$53`;
- two distinct cargo identities;
- opposite directions;
- valid open-end child lanes;
- `$53` children use current-lane Spike state.

F. FIRING

- Level 9 holdoff/capacity configuration;
- Flipper pivot firing suppression;
- Tanker firing;
- repeated-cycle Spiker firing;
- new projectile next-step advancement;
- no change to accepted Level 7 firing.

G. RECONSTRUCTION

- active-below-rim;
- at-rim;
- pending supply;
- lost latent cargo;
- released cargo;
- resolved outcomes;
- fresh generic reconstruction;
- fresh `$53` movement state;
- no duplicates;
- repeated reset idempotence;
- post-reconstruction final conversion.

H. PROGRESSION

- natural Level 8 -> 9;
- score/lives carry;
- approach;
- fresh Super Zapper;
- direct `--level 9`;
- recording/playback startup;
- terminal Level 9 completion;
- restart to Level 1;
- Level 10 rejection.

I. REGRESSION

Add targeted regression coverage demonstrating Levels 1-8 remain unchanged.


PART 13: AUTOMATED VALIDATION

Use bounded commands.

At minimum run:

1. New Level 9-focused tests.

2. Directly affected shared tests for:
   - geometry/topology;
   - Flipper;
   - Tanker;
   - Spiker;
   - Spike;
   - ledger/reconstruction;
   - progression;
   - command-line parsing;
   - recording/playback.

3. Complete Level 9 test subset.

4. Targeted regression for Levels 1-8.

5. Recording/playback regression subset.

6. Full solution tests:

   dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal

7. Solution build:

   dotnet build TempestCSharp.slnx --no-restore --verbosity minimal

8. Whitespace validation:

   git diff --check

Do not launch interactive Tempest.

Do not perform Neal's manual gameplay acceptance.

If Windows Application Control blocks an artifact:

- do not weaken security;
- do not bypass it;
- do not repeatedly retry the same artifact;
- do not substitute stale binaries;
- do not claim success.

Retry only through a materially different safe route.

If required validation remains blocked, preserve the worktree and create a PARTIAL or BLOCKED report.

All potentially long operations must be bounded.

A timeout, canceled test, blocked command, stalled operation, or hung process is not a pass.


PART 14: MANUAL ACCEPTANCE HANDOFF

After automated validation, provide Neal exact task-appropriate commands.

At minimum provide:

DIRECT LEVEL 9 PLAY

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1

HALF-SPEED LEVEL 9 OBSERVATION

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5

LEVEL 9 RECORDING

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --record-gameplay Temp\GameplayRecordings\level9-manual-acceptance.json

LEVEL 9 PLAYBACK AT HALF PRESENTATION SPEED

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level9-manual-acceptance.json --speed 0.5

If actual accepted command syntax differs, provide the exact real commands.

Manual acceptance remains PENDING NEAL.

Ask Neal to validate in practical groups:

GROUP 1: TUBE AND STARTUP

- stepped shape and orientation;
- open endpoints;
- 15 playable lanes;
- no false closing line;
- no endpoint wrap;
- Blaster direction and lane movement;
- starting Spikes on all playable lanes;
- starting Spike presentation;
- correct Level 9 HUD/identity;
- no ordinary Flippers/Pulsars/Fuseballs.

GROUP 2: TANKER AND `$53` FLIPPERS

- natural Tanker cargo travels straight;
- shallow shot cargo uses straight route;
- deep shot cargo uses Spike-sensitive `$53`;
- below-tip same-lane movement;
- equality/above-tip lane-change permission;
- shortened/destroyed Spike changes the lane-stay threshold;
- movement continues during pivot;
- new lane is checked only after pivot completion and subsequent movement;
- no invalid endpoint wrap.

GROUP 3: SPIKER / FIRING / PRESSURE

- Spiker behavior;
- repeated-cycle Spiker firing;
- Tanker firing;
- Flipper firing when eligible;
- projectile behavior and visible speed;
- Tanker, Spiker, Flipper, and projectile speed comparison;
- overall arcade/MAME pressure.

GROUP 4: DEATH / PROGRESSION / TERMINAL

- death with latent Tanker cargo;
- death with released cargo;
- generic re-entry presentation;
- fresh `$53` behavior after re-entry;
- natural Level 8 -> 9 transition;
- score/lives carry;
- Super Zapper recharge;
- Level 9 completion;
- terminal restart to Level 1;
- recording/playback fidelity.

Do not require Neal to test every invisible accounting invariant manually. Automated tests are authoritative for identity/conservation bookkeeping.


PART 15: NO COMMIT OR PUSH

This is an implementation candidate awaiting Neal's manual acceptance.

Do NOT:

- stage;
- commit;
- push;
- update origin/master;
- close Level 9;
- begin Level 10.

Show final:

git status --short --branch
git diff --stat
git diff --name-status

Clearly identify:

- production files changed;
- test files added or changed;
- documentation files changed, if any;
- ignored AI log records;
- unresolved/manual items.


DURABLE DOCUMENTATION POLICY

This implementation is not yet manually accepted.

Do not update durable documentation to claim Level 9 acceptance.

A narrow implementation-status note may be added to TEMPEST-DEVELOPMENT-NOTES.md only if current project conventions require it, but it must say:

- Level 9 candidate implemented;
- automated validation status;
- manual acceptance pending;
- project translations/approximations remain pending Neal.

Do not update TEMPEST-BEHAVIOR-SPEC.md as though Level 9 behavior is accepted.

Prefer deferring durable acceptance documentation until manual testing succeeds.


AI LOGS

Save the complete substantive task under:

AI-Logs/Prompts/2026-11-11-level9-bounded-production-implementation-prompt.md

Create the implementation report under:

AI-Logs/Reports/2026-11-11-level9-bounded-production-implementation-report.md

AI-Logs remain ignored and must not be staged or committed.

The task is not complete until both files exist and the report has been read back or explicitly verified.


REQUIRED IMPLEMENTATION REPORT

Include:

1. Starting Git checkpoint and cleanliness.
2. Files and reports inspected.
3. Final implementation scope.
4. Geometry/topology implementation.
5. Presentation choices and evidence classification.
6. Level 9 configuration.
7. Enemy-selection/concurrency policy.
8. `$53` implementation.
9. `$07`/`$53` cargo implementation.
10. Starting Spike implementation.
11. Speed-policy choices and unresolved manual calibration.
12. Firing implementation.
13. Reconstruction implementation.
14. Progression/direct-start/terminal behavior.
15. Recording/playback support.
16. Tests added or modified.
17. Focused validation results.
18. Level 9 test result.
19. Levels 1-8 regression result.
20. Recording/playback regression result.
21. Full solution result.
22. Build result.
23. `git diff --check` result.
24. Exact manual launch/record/playback commands.
25. Manual acceptance checklist.
26. Files changed.
27. Final Git status.
28. Explicit confirmation Levels 1-8 were not intentionally changed.
29. Explicit confirmation Level 10 was not implemented.
30. Explicit confirmation no commit or push occurred.
31. Manual acceptance status: PENDING NEAL.
32. Any known implementation approximation or unresolved fidelity question.


STOP CONDITION

After bounded Level 9 implementation, automated validation, prompt/report records, exact manual acceptance handoff, and final Git status are complete, STOP.

Do not launch Tempest.

Do not conduct manual acceptance.

Do not stage, commit, or push.

Do not begin Level 10.

Wait for Neal's arcade/MAME comparison and explicit acceptance or correction requests.

TEMPEST_LEVEL9_BOUNDED_PRODUCTION_IMPLEMENTATION_20261111