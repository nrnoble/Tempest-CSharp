FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: LEVEL 9 READ-ONLY STATUS AND ROM PREIMPLEMENTATION INVESTIGATION

This begins the Level 9 milestone.

This task is READ-ONLY with respect to production code, tests, durable project documentation, references, recordings, screenshots, and Git state.

Do NOT implement Level 9.

Do NOT modify accepted Levels 1-8.

Do NOT begin Level 10 work.


STARTING CHECKPOINT

The current accepted checkpoint should be:

037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9

Commit subject:

Complete accepted Level 7 behavior

The later Level 8 reconciliation established that Level 8 implementation and manual acceptance are already present in this checkpoint.

Neal has additionally manually verified the exact natural Level 7 -> Level 8 transition.

Before investigating Level 9, run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat

Expected:

- master tracking origin/master;
- local HEAD and origin/master equal to the checkpoint above;
- clean tracked worktree and index.

If that is not true, STOP and report the discrepancy.

Do not reset, restore, clean, checkout, stage, commit, push, or otherwise alter the repository.


GOAL 0: DETERMINE CURRENT LEVEL 9 STATUS FIRST

Do not assume Level 9 is unimplemented merely because an older report says so.

Before performing a preimplementation analysis, inspect the CURRENT repository and durable documentation.

Determine whether Level 9 currently exists in any form:

- production LevelConfiguration;
- LevelLoader / LevelDefinition;
- LevelProgression;
- direct --level 9 startup;
- natural Level 8 -> 9 progression;
- Level 9 wave selection;
- Level 9 tests;
- terminal/campaign handling;
- recording/playback configuration;
- durable behavior/development documentation;
- historical implementation/closeout reports.

Classify the status as exactly one of:

A. LEVEL 9 NOT IMPLEMENTED

B. LEVEL 9 PARTIALLY IMPLEMENTED / UNACCEPTED

C. LEVEL 9 ALREADY IMPLEMENTED AND DOCUMENTED

If B or C is found, STOP before treating this as a normal preimplementation investigation.

Instead:

- identify the implementation/history;
- read the relevant reports;
- reconcile the apparently stale premise;
- give Neal the exact current status;
- do not duplicate existing implementation work.

If A is confirmed, proceed with the Level 9 preimplementation investigation below.


PRIMARY EXISTING LEVEL 9 ROM ANALYSIS

Read completely:

AI-Logs/Reports/2026-11-06-level-specific-flipper-spike-rom-analysis-report.md

Use it as prior evidence, not as current implementation-status authority.

That report establishes important Level 9 movement behavior:

- displayed Level 9 selects Flipper program $53;
- initial ROM wave_spikeht is $c0;
- the secondary Incitti board name is "Steps";
- $53 is the same ROM movement program selected for Level 5;
- $53 reads the CURRENT lane's live Spike height;
- below the relevant Spike threshold, the Flipper continues moving along the same lane and does not start a lane change;
- at equality the predicate permits the flip;
- after passing the threshold, the normal flip can start;
- a missing/destroyed Spike is treated so flipping is allowed;
- a shortened Spike changes the threshold;
- the predicate is reevaluated from the current lane after a completed flip;
- this is not an explicit capture/lock state and is not a random movement decision;
- natural near-rim Tanker cargo uses $07;
- sufficiently deep player-shot Tanker cargo uses the level-selected $53 route.

Verify those findings against the local ROM source as needed.

Do not silently reinterpret $53 as a generic CanFlip behavior.


EVIDENCE AUTHORITY

Use evidence in this order:

1. Neal's directly observed arcade behavior where supplied.
2. Project-local Atari Tempest Rev. 2 ROM-derived evidence/disassembly and already documented local binary-backed extraction evidence.
3. Accepted C# Levels 1-8 as reusable architecture only, not as evidence of Level 9 arcade behavior.
4. Project-local Mark Incitti material as a secondary engineering reference when stronger evidence is incomplete.
5. MAME observation supplied by Neal.
6. Clearly labeled PROJECT TRANSLATION or PROJECT APPROXIMATION where unavoidable.

Do not search the internet for another disassembly.

Do not allow Incitti behavior to override clear ROM evidence.

Do not assume Level 8 rules automatically carry into Level 9.


GOAL 1: LEVEL 9 GEOMETRY / TOPOLOGY / PRESENTATION

The previous Level 8 work eventually used local Rev. 2 binary-backed table extraction to resolve geometry omitted from the annotated listing.

Determine whether that already documented local evidence contains the Level 9 geometry row as well.

Resolve as much as is safely available:

- displayed/internal Level 9 identity;
- lev_remap selection;
- geometry/shape row;
- raw X coordinates;
- raw Y coordinates;
- angle table;
- lev_open;
- open versus closed topology;
- number of playable lanes;
- edge ordering;
- endpoint behavior;
- directed turns;
- lev_scale;
- lev_y3d;
- lev_y2d;
- Flipper scale if applicable;
- tube/palette lookup;
- appropriate C# translation seam.

Do not reopen or directly inspect protected ROM binaries unless existing project instructions and prior accepted evidence explicitly permit it.

Prefer existing documented binary-backed evidence if sufficient.

If geometry remains unresolved, inspect Incitti's "Steps" geometry only as SECONDARY REFERENCE.

Clearly separate:

RAW-ROM / BINARY-CONFIRMED

ROM-LISTING-BACKED

SECONDARY REFERENCE

PROJECT TRANSLATION

MANUAL ACCEPTANCE NEEDED


GOAL 2: LEVEL 9 PARAMETER TABLE

Trace effective Level 9 Medium/default parameters.

Establish, where source allows:

- generic pending supply;
- starting Spike raw value;
- max generic active enemy gate;
- Flipper min/max;
- Tanker min/max;
- Spiker min/max;
- Pulsar min/max;
- Fuzzball/Fuseball min/max;
- Tanker cargo type;
- Flipper movement entry $53;
- enemy shot holdoff;
- enemy-shot-slot capacity;
- relevant raw speed inputs;
- flip-top acceleration;
- any other Level 9-specific table values required for implementation.

Keep Medium/default as the target unless current project policy explicitly requires otherwise.

Do not add general difficulty support to Level 9.


GOAL 3: EXACT $53 FLIPPER / SPIKE BEHAVIOR

Perform a fresh focused instruction trace of $53 sufficient for implementation.

Determine precisely:

1. movement before the Spike test;
2. which lane's Spike height is tested;
3. comparator orientation;
4. equality behavior;
5. behavior below the Spike threshold;
6. behavior after satisfying the predicate;
7. whether movement continues during active flip continuation;
8. when the new lane's Spike is first tested;
9. treatment of a missing/destroyed Spike;
10. treatment of a shortened Spike;
11. whether the condition is latched;
12. whether $53 itself changes direction;
13. any open-endpoint interaction;
14. Flipper firing eligibility during the route.

Translate this into a simple human-observable pattern for later Neal/MAME/arcade frame-step acceptance.

Use careful terminology.

Avoid calling it a "lock" without explaining that it is a conditional lane-stay behavior rather than a separate captured state.


GOAL 4: ORDINARY FLIPPERS VERSUS TANKER CARGO

Determine whether Level 9 ordinary bottom Flippers are selectable.

Separately trace Tanker cargo.

For Tanker cargo determine:

- cargo type;
- cargo pair size;
- latent accounting;
- adjacent child lanes;
- initial child directions;
- natural release route;
- player-shot release route;
- $07 versus $53 selection;
- exact ROM-coordinate threshold and equality behavior;
- effects of current lane Spike state after deep cargo materializes.

Do not assume Level 8's $07/$19 policy without tracing Level 9's actual level-selected route.

Clearly describe the expected difference between:

A. natural/near-rim Tanker cargo

and

B. sufficiently deep shot-released Tanker cargo.


GOAL 5: STARTING SPIKES

Establish Level 9 raw starting Spike behavior.

Known prior result:

wave_spikeht = $c0

Determine:

- how many ROM Spike slots are initialized;
- how topology maps those slots onto playable C# lanes;
- whether all playable lanes begin with a Spike;
- whether any ROM slot is non-playable because of open geometry;
- what existing accepted raw-to-C# mapping, if any, can be reused;
- resulting proposed C# initial Spike height;
- evidence classification of that translation.

Do not assume Level 8's numerical C# Spike height unless the accepted translation formula itself applies.

If the formula applies, calculate Level 9's candidate height explicitly and label it PROJECT TRANSLATION rather than a literal ROM coordinate.


GOAL 6: ENEMY SELECTION / CONCURRENCY

Trace Level 9 enemy-selection behavior.

Determine:

- required species minima;
- species maxima;
- six-unit or other generic-entry gate;
- ordinary eligible species;
- initial minimum-admission behavior;
- post-minimum selection behavior;
- whether Spike heights influence selection;
- Tanker cargo accounting against Flipper limits;
- whether cargo release can exceed generic entry capacity;
- behavior near wave exhaustion;
- final-pending special cases;
- any Spiker-to-Tanker conversion or other conversion behavior.

Compare with existing Level 8 and Level 7 architecture only to identify reusable seams.

Do not assume identical policies merely because types overlap.


GOAL 7: TANKER / SPIKER / FLIPPER / PROJECTILE SPEEDS

Trace Level 9 raw speed setup independently.

Determine:

- Flipper raw speed input;
- Tanker relationship to Flipper speed;
- Spiker raw speed input;
- projectile raw speed input;
- whether ROM relationships are the same or different;
- whether any accepted C# translation can defensibly be reused.

Do NOT copy the manually accepted Level 7 Tanker=Spiker relationship into Level 9 merely because it worked on Level 7.

Do NOT invent a numeric ROM-to-C# conversion.

If visual speed remains an implementation-time project calibration issue, specify exactly what Neal should compare in MAME/arcade.


GOAL 8: FIRING

Trace Level 9 firing:

- eligible enemy species;
- shot holdoff;
- simultaneous enemy-shot capacity;
- chance-table behavior where relevant;
- repeated-cycle Spiker firing;
- Flipper firing during $53 movement;
- Tanker firing;
- projectile update semantics.

Identify what current accepted infrastructure can be reused unchanged versus what is Level 9-specific.


GOAL 9: DEATH / RECONSTRUCTION / CARGO

Trace Level 9 death reconstruction.

Determine:

- active-below-rim treatment;
- rim treatment;
- generic pending supply;
- species identity retention or discard;
- latent Tanker cargo;
- released cargo;
- resolved/terminal outcomes;
- restoration of $53 Flipper movement state versus fresh generic reconstruction;
- whether new Flippers restart the Level 9 $53 program rather than resuming prior program counters;
- possible special conversion behavior after reconstruction.

Identify the closest accepted C# accounting model without modifying it.


GOAL 10: PROGRESSION AND CAMPAIGN

Determine the minimal requirements for:

- natural Level 8 -> Level 9 transition;
- direct --level 9 startup;
- score/lives carry;
- Super Zapper recharge;
- between-level approach;
- Level 9 initial Blaster lane;
- recording/playback;
- Level 9 completion;
- whether Level 9 should be terminal initially or naturally lead to another supported level;
- restart policy if implemented as current terminal boundary.

Do not implement Level 10.

If the ROM naturally progresses beyond Level 9, distinguish arcade behavior from the project's bounded implementation boundary.


GOAL 11: CURRENT C# ARCHITECTURE REUSE

Inspect current accepted architecture.

At minimum review relevant pieces of:

- LevelConfiguration;
- LevelLoader / LevelDefinition;
- LevelProgression;
- Level8WaveSelection;
- Level3WaveLedger/current accounting;
- Flipper;
- Tanker;
- Spiker;
- Spike;
- EnemyBullet;
- TempestGame;
- input/direct startup;
- recording/playback/headless playback;
- terminal campaign handling.

Produce a map:

Level 9 mechanic
-> reusable component
-> Level 9-specific extension
-> evidence classification
-> regression risk.

Protect Levels 1-8.


GOAL 12: IMPLEMENTATION READINESS

End with exactly one decision:

READY FOR BOUNDED LEVEL 9 IMPLEMENTATION

or

NOT READY FOR LEVEL 9 IMPLEMENTATION

READY requires enough evidence to avoid fabricating essential:

- geometry/topology;
- wave composition;
- $53 movement;
- Spike presentation;
- cargo behavior;
- concurrency;
- death/reconstruction;
- progression.

If READY:

Produce a precise bounded implementation specification but DO NOT implement it.

The specification must include:

- geometry/topology;
- presentation;
- configuration;
- pending supply;
- starting Eggs if project architecture uses them;
- starting Spikes;
- species eligibility/min/max;
- generic entry gate;
- Tanker cargo;
- $07/$53 movement distinction;
- ordinary $53 Flipper behavior;
- Spiker behavior;
- firing;
- speed evidence boundaries;
- death/reconstruction;
- direct startup;
- Level 8 -> 9 progression;
- completion/terminal boundary;
- recording/playback;
- automated tests;
- manual acceptance checklist.

If NOT READY:

List only true blockers.

For every blocker state:

- missing evidence;
- why implementation would require guessing;
- whether existing project-local secondary reference can resolve it;
- whether Neal can resolve it through arcade/MAME observation;
- exact observation required.


MANUAL ACCEPTANCE PLANNING

Where Neal will eventually need to compare arcade/MAME, make each request concrete.

Likely subtle Level 9 observations may include:

- exact tube orientation and open/closed topology;
- Flipper behavior while below a live Spike tip;
- behavior at/equality with the Spike tip;
- lane changes above the tip;
- behavior when the current Spike is shortened or destroyed;
- natural Tanker cargo behavior;
- deep shot-released cargo $53 behavior;
- relative Tanker/Spiker/shot speeds;
- death/re-entry;
- Level 8 -> 9 transition.

Prefer recorded gameplay and frame stepping for subtle movement.

Do not ask vague "does Level 9 look right?" questions.


REGRESSION PROTECTION

Levels 1-8 are accepted.

Do not recommend altering earlier production behavior as part of Level 9 unless a separately authorized defect is discovered.

Especially:

- do not alter Level 5 merely because Level 9 also uses $53;
- do not alter Level 7's accepted movement/speed policy;
- do not alter Level 8's accepted $19 policy;
- do not generalize Level 9 Spike-sensitive Flipper behavior into levels that do not select $53.


READ-ONLY POLICY

Do not:

- edit production code;
- edit tests;
- edit durable documentation;
- edit references;
- edit recordings;
- launch Tempest;
- create a gameplay recording;
- stage;
- commit;
- push.

Do not create temporary compiled diagnostic harnesses.

Use bounded source/reference/Git inspection only.

No build or test suite is required unless inspection reveals a specific read-only question that cannot be resolved statically. If execution is unexpectedly required, explain why before running it.


AI LOG REQUIREMENTS

Save the complete substantive task under:

AI-Logs/Prompts/2026-11-11-level9-status-rom-preimplementation-investigation-prompt.md

Create the report under:

AI-Logs/Reports/2026-11-11-level9-status-rom-preimplementation-investigation-report.md

AI-Logs are ignored historical/working artifacts and must not be staged or committed.

The task is not complete until both files exist and the report has been read back or explicitly verified.


REQUIRED REPORT STRUCTURE

1. Starting Git checkpoint and cleanliness.
2. Current Level 9 implementation-status determination.
3. Historical Level 9 reports/files found.
4. Evidence sources and authority.
5. Level identity and parameter indexing.
6. Geometry/topology.
7. Palette/presentation evidence.
8. Level 9 parameter table.
9. Exact $53 Flipper/Spike trace.
10. Human-observable $53 behavior.
11. Ordinary Flippers versus Tanker cargo.
12. $07/$53 cargo boundary.
13. Starting Spike behavior and proposed translation.
14. Enemy selection/concurrency.
15. Tanker cargo accounting/lifecycle.
16. Tanker/Spiker/Flipper/projectile speed evidence.
17. Enemy firing.
18. Death/reconstruction.
19. Progression/direct startup/completion boundary.
20. Accepted Levels 1-8 regression constraints.
21. C# architecture reuse map.
22. Explicit READY / NOT READY decision.
23. If READY, bounded implementation specification.
24. If NOT READY, exact blockers and next evidence.
25. Proposed automated implementation tests.
26. Proposed manual arcade/MAME acceptance checklist.
27. Files changed by this investigation.
28. Final Git status.
29. Confirmation no production/tests/durable docs/references/recordings changed.
30. Confirmation no commit or push occurred.


SAFETY AND STALL POLICY

Follow the established project policy.

If Windows Application Control or similar security blocks a command or generated artifact:

- do not weaken security;
- do not bypass it;
- do not repeatedly retry the same artifact;
- do not substitute stale binaries;
- report the blocker.

Bound all potentially long operations.

If an operation makes no meaningful progress, stop and report rather than waiting indefinitely.


STOP CONDITION

After the read-only Level 9 status/ROM investigation, implementation-readiness decision, required prompt/report records, and final Git status are complete, STOP.

Do not implement Level 9.

Do not launch Tempest.

Do not commit or push.

Do not begin Level 10.

Wait for Neal to review the evidence and explicitly authorize the next Level 9 milestone.

TEMPEST_LEVEL9_STATUS_ROM_PREIMPLEMENTATION_20261111