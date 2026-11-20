FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Implement the bounded Level 6 Tanker-cargo Flipper movement correction
established by ROM analysis and Neal's direct arcade observation.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted baseline beneath the existing uncommitted Level 6 concurrency
candidate:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

The existing Level 6 concurrency candidate is already present in the worktree.

It has passed automated validation, its pending-lane Spike crash has been
corrected, and Neal's Level 6 manual concurrency tests pass.

Do not redo or broaden that work.

Read these handoffs:

AI-Logs/Reports/2026-11-10-level6-flipper-0b-provenance-check-report.md

AI-Logs/Reports/2026-11-10-level6-flipper-0b-bounded-comparison-report.md

AI-Logs/Reports/2026-11-10-level6-combined-concurrency-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-10-level6-pending-lane-spike-crash-correction-report.md

Treat the two Flipper reports as the direct movement evidence for this task.

Do not repeat broad ROM reverse engineering.

This is an IMPLEMENTATION CANDIDATE requiring Neal's subsequent manual
acceptance.

Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 7 concurrency.


MANUAL ARCADE EVIDENCE

Neal has directly observed the deep Level 6 Tanker-cargo movement pattern on
the original arcade game and confirms this pattern is familiar and important:

1. Eight straight along-tube movement advances.
2. Then one lane flip.
3. During the flip, the Flipper does NOT continue moving up the tube.
4. After completion of the flip, another eight-advance interval begins.
5. The Flipper retains its lane-travel direction through repeated cycles.
6. The two Flippers released from a Tanker begin traveling in opposite
   directions.

Treat this as ARCADE-OBSERVED at this stated level of specificity.

Neal considers tube-specific Flipper movement a significant part of Tempest
gameplay and experienced-player timing/muscle memory.


ROM-BACKED MOVEMENT

The local ROM disassembly establishes two distinct Level 6 Tanker-cargo
movement paths.

DEEP / PLAYER-SHOT RELEASE:

If the Tanker's preserved ROM enemy_along value is >= $20, its cargo receives
the level-selected Flipper movement program.

For displayed Level 6 that program is:

$0b

Program $0b:

- loads an eight-move counter;
- performs eight along-tube movement operations;
- initiates one lane flip;
- performs no along-tube advancement while the flip is in progress;
- on completion begins another eight-move interval;
- contains no scripted direction reversal;
- repeats below the rim until top/rim handling takes over.

The two Tanker cargo children begin with opposite direction bits.


NEAR-RIM / NATURAL SPLIT:

If preserved enemy_along is < $20, cargo uses default program:

$07

Program $07:

- performs straight along-tube movement;
- contains no below-rim lane-flip operation;
- continues straight until top/rim handling supersedes it.

Natural Tanker splitting occurs in this near-rim branch and therefore uses
$07.


CURRENT C# MISMATCH

Current canonical Level 6 uses BelowRimFlipperProgram.Existing.

Both Level 6 cargo cases currently use the same general behavior:

- cargo begins with a release pivot;
- cargo gains height during that pivot;
- a 23-tick pause follows;
- cargo continues gaining height during the pause;
- another same-direction pivot begins;
- pivot/pause cycles repeat below the rim.

That is neither the ROM $0b cadence nor the $07 straight-climb path.

The current C# code also lacks the Level 6 ROM distinction between deep-shot
cargo and near-rim/natural cargo.

Correct that specific mismatch.


STRICT SCOPE

This milestone changes only canonical Level 6 Tanker-released Flipper
below-rim movement required for the $0b/$07 behavior.

Do not modify:

- Level 6 admission minima/maxima;
- zero ordinary Flipper admission;
- pending-segment Spike selection;
- seven-place headroom;
- terminal Spiker-to-Tanker conversion;
- generic-obligation death reconstruction;
- Tanker selection frequency;
- Tanker firing;
- enemy-shot limits;
- Spiker/Spike behavior;
- Super Zapper;
- Blaster death/reentry;
- scoring;
- rim pursuit/combat;
- completion/progression;
- Level 1-5 behavior;
- Level 7-8 behavior.

Do not generalize all Flipper movement programs in this milestone.

Implement only what Level 6 cargo requires.


IMPLEMENTATION DESIGN

Prefer a small explicit Level 6 cargo movement mode/program integrated with
the existing Flipper architecture.

The implementation should make the runtime distinction when Tanker cargo is
created/released.

The movement mode must distinguish:

A. Level 6 deep/player-shot cargo -> $0b behavior.

B. Level 6 near-rim/natural cargo -> $07 behavior.

Do not infer the branch afterward from visual position if the Tanker release
path already knows the relevant depth/release condition.

Carry the minimum deterministic movement-program state needed by the Flipper.

Do not create a broad ROM p-code interpreter unless the existing architecture
already makes that substantially simpler than a narrow explicit translation.


$0b C# TRANSLATION

Implement a maintainable C# translation with these invariants:

- cargo begins the $0b movement cycle after release;
- perform the equivalent of eight straight movement updates/advances;
- no below-rim lane flip occurs during those eight advances;
- after the eighth advance, begin one lane flip;
- preserve depth throughout the flip;
- do not apply ordinary climb advancement while that flip is active;
- when the flip completes, immediately begin a fresh eight-advance cycle;
- preserve the child's travel direction;
- do not reverse direction merely because a cycle completed;
- allow normal geometry/open-end correction where the existing project needs
  it;
- stop the below-rim $0b program when established rim/top behavior takes over.

Do not describe the eight moves as eight pixels or eight C# height units unless
the current movement conversion explicitly establishes that equivalence.

Preserve existing accepted climb-speed/project timing translations where a
literal ROM speed conversion is not already established.


$07 C# TRANSLATION

For the near-rim/natural-split Level 6 cargo path:

- use straight below-rim climbing;
- do not initiate repeated below-rim lane pivots;
- do not add a 23-tick below-rim pivot pause;
- hand off to normal established rim behavior at the appropriate rim boundary.

Do not add an artificial flip merely because the common C# cargo path
previously began with one.


CARGO PAIR DIRECTION

Preserve/implement the ROM-backed pair behavior:

- the two released children receive opposite lane-travel directions;
- each child preserves its own direction through repeated $0b cycles;
- $0b itself does not reverse direction.

Do not invent screen-left/right or clockwise/counterclockwise labels where the
ROM-to-screen handedness has not been established.

Tests should assert relative/opposite direction rather than unsupported visual
handedness.


RELEASE-CAUSE / DEPTH DECISION

Preserve the ROM-backed distinction.

Deep Tanker destroyed by player shot:
-> Level 6 $0b cargo.

Near-rim natural Tanker split:
-> Level 6 $07 cargo.

Trace the current release paths before editing so this decision is made at the
smallest authoritative point.

Do not accidentally apply $0b to natural near-rim release.

Do not accidentally apply $07 to a validated deep-shot release.

If current C# Tanker depth coordinates do not map literally to ROM
enemy_along $20, use the narrowest existing ROM-coordinate/C# translation
supported by the project and classify it as PROJECT TRANSLATION /
APPROXIMATION.

Do not guess a threshold without evidence.

If the exact C# threshold cannot be safely established from existing mappings,
STOP and report that specific gap rather than inventing one.


TANKER CARGO CREATION

Preserve existing accepted complete-pair behavior:

- two cargo identities;
- existing ownership/accounting;
- release lane geometry;
- opposite initial travel directions;
- scoring;
- shot versus natural Tanker lifecycle;
- cargo completion obligations.

Do not reopen strict seven-slot ROM child-loss behavior.

Only the Level 6 below-rim movement program/release-mode selection changes.


DEATH RECONSTRUCTION

The current Level 6 generic-obligation death reconstruction candidate is
already accepted in manual testing direction and automated validation.

Do not make $0b/$07 movement state survive death as a retained Flipper.

If a materialized cargo Flipper becomes a generic obligation at Blaster death,
its former movement-program state disappears with its former species.

If that generic obligation later becomes a new Tanker whose cargo is released,
the new cargo receives movement according to that new release event.


TESTS

Add focused deterministic coverage.

At minimum:

1. DEEP SHOT RELEASE SELECTS $0b
   - canonical Level 6;
   - Tanker destroyed in the established deep condition;
   - two cargo children;
   - both receive Level 6 $0b mode;
   - pair directions are opposite.

2. $0b STRAIGHT INTERVAL
   - child performs eight straight movement advances;
   - no lane flip begins before the interval completes.

3. $0b FLIP
   - after the eight advances, one flip begins;
   - depth does not advance during the flip;
   - direction is preserved.

4. $0b REPEAT
   - after the flip completes, a new eight-advance interval begins;
   - no extra 23-tick common cargo pause is inserted;
   - subsequent flip uses the same travel direction unless geometry correction
     explicitly requires otherwise.

5. NATURAL / NEAR-RIM RELEASE SELECTS $07
   - canonical Level 6;
   - natural near-rim split;
   - children follow straight below-rim movement;
   - no repeated below-rim pivot/pause cycle occurs.

6. RELEASE MODE IS DISTINCT
   - deep-shot and natural release of otherwise comparable Tankers select
     different Level 6 cargo programs.

7. RIM HANDOFF
   - both programs yield to existing rim behavior correctly;
   - no malformed transition or stuck movement.

8. ISOLATION
   - Level 5 cargo behavior unchanged;
   - Level 7/8 specialized movement unchanged;
   - common copied/noncanonical configurations unchanged;
   - ordinary Level 6 Flipper admission remains disabled.

9. CONCURRENCY CANDIDATE PRESERVATION
   - Level 6 admission;
   - pending-lane Spike selector;
   - terminal Spiker conversion;
   - generic-obligation death reconstruction;
   - complete-pair cargo accounting
   remain unchanged.


VALIDATION

Use repository-wide bounded validation/security rules.

Prefer Release configuration.

Start with the exact new Level 6 $0b/$07 movement tests.

Then run focused:

- Level 6 admission/policy;
- Level 6 production;
- Tanker cargo;
- Flipper movement;
- Spiker/Spike;
- death reconstruction.

If focused validation passes, run affected Release regressions covering:

- Levels 1-5;
- Levels 7-8 specialized Flipper behavior;
- Tanker cargo;
- recording/playback;
- progression;
- Super Zapper;
- rim/completion.

If affected validation passes:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete candidate diff review;
- final repository-state verification.

Stop on FAIL, BLOCKED, TIMEOUT, CANCELED, or meaningful stall.

Do not weaken or bypass Windows security.


MANUAL HANDOFF

If automated validation passes, leave the entire Level 6 candidate uncommitted.

Do not launch the game.

Provide Neal:

dotnet run --project src\Tempest.Game -- --level 6 --lives 10

Manual comparison should be short and specific.

DEEP-SHOT TEST:

1. Let a Tanker remain clearly deep in the tube.
2. Destroy it with a player shot.
3. Observe the two released Flippers.
4. Verify a straight-climb interval before each flip.
5. Verify the Flipper appears to hold tube depth during the visible flip.
6. Verify the straight-climb interval starts again after the flip.
7. Verify each child retains its travel direction and the pair travel in
   opposite directions.

NATURAL-SPLIT TEST:

1. Let a Tanker survive until it splits naturally near the rim.
2. Observe the released cargo below the rim.
3. Verify those Flippers climb straight without the repeated $0b below-rim
   flipping cadence.
4. Verify normal rim handoff.

Also verify:

- Tanker cargo still releases as a pair;
- Level 6 concurrency still looks correct;
- no recurrence of the pending-lane Spike crash;
- death/reentry remains correct;
- Levels 1-5 show no regression.

Do not claim manual acceptance until Neal reports it.


DOCUMENTATION AND REPORT

This remains an implementation candidate.

Do not update durable documentation to claim Level 6 acceptance.

Preserve the provenance and bounded-comparison reports as historical evidence.

Create a new paired record:

AI-Logs/Prompts/2026-11-10-level6-flipper-0b-07-implementation-prompt.md

AI-Logs/Reports/2026-11-10-level6-flipper-0b-07-implementation-report.md

Include:

- arcade evidence supplied by Neal;
- ROM $0b/$07 distinction;
- C# movement design;
- release/depth mapping;
- direction handling;
- tests;
- focused/affected/full validation;
- build;
- repository state;
- exact manual handoff;
- acceptance pending.


GIT AND STOP

Do not commit.
Do not push.

Stop after automated validation and manual handoff.

Do not begin Level 7 concurrency.
Do not broaden movement work to other levels.


TEMPEST_LEVEL6_FLIPPER_0B_07_IMPLEMENTATION_COMPLETE