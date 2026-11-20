FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: LEVEL 8 READ-ONLY PREIMPLEMENTATION RECONCILIATION INVESTIGATION

This begins Level 8 work after the complete accepted Level 7 closeout.

This task is READ-ONLY with respect to production code, tests, durable project documentation, reference files, recordings, and Git state.

Do NOT implement Level 8 in this task.

Do NOT modify accepted Levels 1-7.

Do NOT begin Level 9 work.


STARTING CHECKPOINT

The accepted Level 7 production checkpoint is:

037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9

Commit subject:

Complete accepted Level 7 behavior

Before investigation, verify:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat

Expected:

- branch master tracking origin/master;
- local HEAD and origin/master at the accepted Level 7 commit;
- clean tracked worktree and index.

If that is not true, STOP before investigation and report the discrepancy.

Do not reset, restore, clean, checkout, stage, commit, push, or otherwise alter the repository.


PRIMARY PRIOR REPORT

Read completely:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

Do not redo that investigation from scratch.

Treat its demonstrated ROM findings as prior evidence to verify/reconcile where necessary.

Its status was:

PARTIAL / BLOCKED — do not implement Level 8 from that investigation.

The primary blockers included unavailable raw geometry/remap/open data, unresolved Level 8 geometry/topology/palette, raw Spike-height translation, and the need to keep Level 8's specific Flipper/cargo movement distinct from Level 7.

This investigation should determine whether those blockers can now be resolved from the project-local evidence and accepted project knowledge sufficiently to produce a bounded Level 8 implementation specification.


EVIDENCE AUTHORITY

Use evidence in this order:

1. Neal's directly observed arcade behavior where supplied.
2. Project-local Atari Tempest Rev. 2 ROM-derived evidence/disassembly.
3. Accepted Levels 1-7 C# behavior only as reusable engineering architecture, not as evidence of Level 8 arcade behavior.
4. Mark Incitti's project-local Tempest implementation as a secondary engineering reference where primary ROM evidence is unavailable or incomplete.
5. MAME observation supplied by Neal to resolve visual/behavioral questions.
6. Clearly labeled PROJECT APPROXIMATION only when no stronger source exists and a future implementation would require one.

Do not search the internet for another Tempest disassembly.

The project-local ROM-derived reference remains the primary ROM authority.

Do not use Mark Incitti to override clear ROM evidence.

Do not assume Level 7 mechanics automatically apply to Level 8.

Do not infer arcade facts merely because existing C# code behaves a certain way.


PRIOR ROM FINDINGS TO RECONCILE

The previous Level 8 report established, among other things:

- displayed Level 8 is internal zero-based level index 7;
- effective parameter-table level is $08;
- Medium/default generic pending supply is 24;
- generic active-entry gate is six;
- ordinary bottom selection permits Tankers and Spikers;
- ordinary Flippers are not selected;
- Pulsars and Fuseballs/Fuzzballs are not selected;
- Tankers carry two Flippers;
- Tanker cargo does not consume extra generic pending supply;
- when no generic units remain, the ROM can convert a Spiker in place into a Flipper-carrying Tanker without adding supply;
- Tanker minimum/maximum is 1/2;
- Spiker minimum/maximum is 2/4;
- Level 8 Flipper movement program byte is $19;
- $19 is not Level 7's $24 movement program;
- near-rim/natural Tanker cargo uses the $07 straight entry;
- sufficiently deep shot-released cargo uses Level 8's selected $19 movement program;
- generic death reconstruction discards species/movement identity;
- latent Tanker cargo is not reconstructed;
- released cargo contributes as active units;
- no Level 8 implementation should begin until geometry/topology and other unresolved translation questions are sufficiently settled.

Verify these conclusions against the local source where necessary.

If any prior conclusion is incorrect, distinguish:
- prior report statement;
- new evidence;
- corrected conclusion.

Do not silently rewrite history.


INVESTIGATION GOAL A: LEVEL 8 GEOMETRY AND TOPOLOGY

The previous report could not recover Level 8 geometry because the annotated ROM listing contained unexpanded .chunk declarations for:

lev_x
lev_y
lev_angle
lev_remap
lev_open

Investigate what project-local evidence is NOW available.

Search appropriate project-local reference material, excluding protected/unrelated personal scratchpad material.

Determine whether the repository now contains any trustworthy source for:

- Level 8 tube selector;
- raw vertex coordinates;
- lane ordering;
- open/closed topology;
- lane count;
- directed edge relationships;
- convex/reflex/shared vertices;
- player traversal;
- projection scale/centering;
- tube palette/color.

Do not fabricate missing ROM bytes.

If the ROM-derived evidence remains incomplete, inspect project-local Incitti material strictly as a secondary engineering reference and determine:

1. what Level 8 geometry Incitti uses;
2. whether its geometry can be mapped cleanly into current C# LevelDefinition architecture;
3. which aspects would remain SECONDARY-REFERENCE or PROJECT TRANSLATION rather than ROM-BACKED.

If MAME/arcade observation from Neal would still be required to select or manually validate the geometry, state exactly what Neal needs to observe.


INVESTIGATION GOAL B: LEVEL 8 $19 FLIPPER MOVEMENT

Perform a focused instruction-level reconciliation of Level 8's $19 Flipper program.

Do not reuse Level 7's accepted $24 / 2-3-2-2-2 behavior.

Determine as precisely as local ROM evidence permits:

1. exact movement/flip opcode sequence;
2. along-tube movement between flips;
3. whether movement continues during active flip transition steps;
4. direction handling;
5. whether $19 itself ever reverses direction;
6. interaction with open-tube endpoint reversal if applicable;
7. firing eligibility during this route;
8. expected observable pattern for a Tanker-released Flipper.

Translate the ROM sequence into a human-observable description Neal can later verify using arcade/MAME recording and frame stepping.

Do not invent an arbitrary repeating visual pattern if the ROM does not establish one.


INVESTIGATION GOAL C: $07 VERSUS $19 TANKER CARGO BOUNDARY

Resolve the previous report's cargo-boundary question with a fresh, targeted instruction trace.

Trace the relevant ROM control flow around the $20 comparison carefully.

Determine:

- exact comparison;
- branch condition;
- equality behavior;
- natural Tanker release path;
- sufficiently deep player-shot destruction path;
- which path selects $07;
- which path selects Level 8's $19;
- whether the previous report's interpretation is correct.

State the result in ROM coordinate terms first.

Do not invent a C# depth threshold unless a conversion is actually established.

Then determine what additional translation evidence a future implementation would need.


INVESTIGATION GOAL D: LEVEL 8 SPIKER/TANKER SPEED RELATIONSHIP

Levels 7 and 8 must remain distinct unless evidence establishes shared behavior.

The accepted Level 7 production relationship now has Tankers and Spikers using the same visible movement rate based on Neal's manual arcade/MAME acceptance.

For Level 8, investigate independently.

Trace the Level 8 ROM speed setup for:

- Flippers;
- Tankers;
- Spikers;
- enemy projectiles.

Determine:

1. which raw ROM inputs apply;
2. whether Tanker speed derives from Flipper speed;
3. whether Spiker raw speed equals or differs from Tanker/Flipper raw speed;
4. whether there is sufficient evidence to derive a C# movement relationship;
5. whether Neal should perform a focused arcade/MAME Level 8 speed comparison before implementation.

Do not copy the Level 7 Tanker=Spiker project relationship to Level 8 without Level 8 evidence.

Do not invent a raw-ROM-to-C# numeric conversion.


INVESTIGATION GOAL E: LEVEL 8 SPIKES

The prior report found raw starting Spike height $c8 copied into all 16 ROM Spike slots but did not establish a C# conversion.

Investigate whether accepted Levels 4-7 work, current ROM understanding, or other project-local evidence now supports a defensible translation.

Determine:

- raw Level 8 Spike initialization behavior;
- how many physical/playable Spike locations Level 8 should have once topology is known;
- whether every playable lane should begin with a Spike;
- whether unused ROM slots exist for open tubes;
- whether a raw-to-C# depth/height mapping can now be established.

If not, identify a minimal future PROJECT APPROXIMATION or manual calibration seam rather than pretending a ROM conversion exists.


INVESTIGATION GOAL F: ENEMY SELECTION, CONCURRENCY, AND FINAL-PENDING CONVERSION

Reconcile Level 8 enemy selection using the actual accepted lessons from Level 7 without merely copying Level 7 implementation.

Determine:

1. initial minimum admission behavior;
2. ordinary Tanker/Spiker selection after minima;
3. six-unit generic gate;
4. type-specific maxima;
5. how Tanker latent cargo affects type availability;
6. how cargo release can exceed the generic gate;
7. final-pending Spiker-to-Tanker in-place conversion;
8. whether conversion preserves the same generic unit/obligation;
9. whether conversion gives the Tanker two latent Flippers;
10. whether any ordinary Flipper can ever enter from the bottom;
11. potential saturated-capacity behavior when cargo children cannot both materialize.

Compare this with accepted Level 7 architecture and identify exactly what can safely be reused versus what requires Level 8-specific policy.


INVESTIGATION GOAL G: DEATH AND RECONSTRUCTION

Trace Level 8-compatible death reconstruction.

Determine:

- what counts are combined after death;
- what identity/species information is discarded;
- treatment of active below-rim enemies;
- treatment of rim enemies;
- treatment of generic reserve/pending supply;
- treatment of latent Tanker cargo;
- treatment of already released cargo Flippers;
- whether reconstructed obligations resume as generic units rather than their pre-death species;
- whether the final-pending Spiker-to-Tanker conversion can occur again after reconstruction.

Recommend the smallest C# reuse of the accepted Level 7 generic-reentry architecture while explicitly identifying Level 8-specific differences.


INVESTIGATION GOAL H: FIRING

Reconcile Level 8 enemy firing:

- shot holdoff;
- simultaneous shot capacity;
- eligible species;
- per-enemy firing lifecycle;
- repeated-cycle Spiker firing;
- projectile update cadence;
- projectile raw ROM speed input.

Determine which accepted firing infrastructure from Level 7 can be reused unchanged and which values/policies are Level 8-specific.

Do not retune Levels 1-7.


INVESTIGATION GOAL I: PROGRESSION AND CAMPAIGN BOUNDARY

Determine the exact minimal Level 8 integration requirements for:

- natural Level 7 -> Level 8 progression;
- direct --level 8 startup;
- score/lives carry;
- fresh Super Zapper charges;
- between-level approach;
- Blaster starting placement;
- recording/playback startup;
- completion of Level 8;
- terminal campaign behavior;
- restart to Level 1;
- rejection/absence of Level 9.

Do not implement any of this yet.


CURRENT C# ARCHITECTURE REVIEW

Review the accepted post-Level-7 codebase as it actually exists at the clean checkpoint.

Identify reusable components and seams for:

- LevelConfiguration;
- LevelLoader / LevelDefinition;
- LevelProgression;
- Level7WaveSelection;
- Level3WaveLedger or current ledger/accounting structures;
- Flipper;
- Tanker;
- Spiker;
- Spike;
- enemy bullets;
- TempestGame;
- direct startup;
- recording/playback;
- headless playback;
- terminal campaign behavior.

Do not modify them.

Produce a concrete mapping:

ROM / arcade mechanic
-> existing reusable C# component
-> Level 8-specific extension required
-> evidence classification
-> implementation risk.


INVESTIGATION GOAL J: DETERMINE IMPLEMENTATION READINESS

The final report must explicitly decide one of:

READY FOR BOUNDED LEVEL 8 IMPLEMENTATION

or

NOT READY FOR LEVEL 8 IMPLEMENTATION

READY means that enough evidence is available to implement a bounded Level 8 candidate without fabricating essential geometry/topology or core behavior.

If READY:

Provide a precise implementation specification including:

- tube geometry/topology source;
- configuration values;
- supply;
- enemy eligibility;
- concurrency/admission;
- Tanker cargo;
- $07/$19 cargo movement rules;
- Spiker behavior;
- Spike initialization;
- firing;
- death/reconstruction;
- progression;
- completion/terminal behavior;
- automated tests;
- manual acceptance checklist.

Still DO NOT implement in this task.

If NOT READY:

List only the genuinely blocking unresolved questions.

For each blocker state:

- what is missing;
- why implementation would require guessing;
- whether local secondary reference can resolve it;
- whether Neal can resolve it through an arcade/MAME observation;
- exact observation or evidence needed next.

Avoid repeating already-resolved questions.


MANUAL ARCADE/MAME OBSERVATION REQUESTS

If Neal's observation is needed, make requests concrete and practical.

For example:

- identify the exact Level 8 tube shape/topology Neal should confirm;
- identify whether the tube is open or closed;
- identify a specific Tanker cargo case to record;
- identify the visual flipping cadence to frame-step;
- identify whether Tanker/Spiker speeds visibly match;
- identify starting Spike presentation.

Do not ask Neal vague questions such as "Does Level 8 look right?"

Use recording/frame stepping where a subtle movement pattern is involved.


REGRESSION PROTECTION

Levels 1-7 are accepted and frozen.

Any future implementation plan must explicitly preserve them.

Do not recommend sharing or generalizing Level 8 behavior into earlier levels unless the existing accepted architecture already safely supports it.

Where new generalized code might be useful, identify the risk and prefer a Level 8-specific seam unless generalization is strongly justified.


READ-ONLY VALIDATION

This is primarily source analysis.

Do not run the interactive game.

Do not modify code.

Do not modify tests.

Do not modify durable documentation.

Do not modify reference files.

Do not create or change gameplay recordings.

Do not stage, commit, or push.

Run only bounded read-only commands needed to inspect:

- Git state;
- local source/reference content;
- existing tests;
- current architecture.

No test suite or build is required unless inspection discovers a concrete reason that execution is necessary to answer a read-only question.

Do not create temporary compiled telemetry harnesses for this task.


AI LOGS

This is substantial Tempest work.

Save the complete substantive prompt under:

AI-Logs/Prompts/2026-11-11-level8-preimplementation-reconciliation-investigation-prompt.md

Create the report under:

AI-Logs/Reports/2026-11-11-level8-preimplementation-reconciliation-investigation-report.md

AI-Logs are ignored historical/working artifacts.

Do not stage or commit them.

The task is not complete until both files exist and the report has been read back or otherwise explicitly verified.


REQUIRED REPORT STRUCTURE

Include:

1. Starting Git checkpoint and cleanliness.
2. Prior Level 8 report reconciliation.
3. Sources inspected and evidence authority.
4. Geometry/topology findings.
5. Tube palette/presentation findings.
6. Level 8 ROM parameter reconciliation.
7. $19 Flipper movement trace and human-observable pattern.
8. $07/$19 cargo boundary trace.
9. Tanker/Spiker speed relationship evidence.
10. Starting Spike findings and translation status.
11. Enemy selection/concurrency/final-pending conversion.
12. Tanker cargo lifecycle.
13. Death/reconstruction semantics.
14. Enemy firing.
15. Progression/direct startup/completion/terminal behavior.
16. Accepted Levels 1-7 regression constraints.
17. Current C# architecture reuse map.
18. Explicit READY / NOT READY implementation decision.
19. If READY, bounded implementation specification.
20. If NOT READY, exact blockers and exact next evidence needed from Neal.
21. Proposed automated implementation tests.
22. Proposed manual acceptance checklist.
23. Files changed by this task.
24. Final Git status.
25. Explicit confirmation no production code/tests/durable documentation/reference files were changed.
26. Explicit confirmation no commit or push occurred.


SAFETY / STALL POLICY

Follow the established project policy.

If Windows Application Control or another security mechanism blocks a command or generated artifact:

- do not weaken security;
- do not bypass it;
- do not enter a same-artifact retry loop;
- do not substitute stale binaries;
- report the block.

All potentially long operations must be explicitly bounded.

If an operation stops making meaningful progress, stop it and report the partial result rather than waiting indefinitely.


STOP CONDITION

After the read-only Level 8 reconciliation investigation, required prompt/report records, implementation-readiness decision, and final Git status are complete, STOP.

Do not implement Level 8.

Do not launch Tempest.

Do not commit.

Do not push.

Wait for Neal to review the evidence and authorize the next bounded milestone.

TEMPEST_LEVEL8_PREIMPLEMENTATION_RECONCILIATION_20261111