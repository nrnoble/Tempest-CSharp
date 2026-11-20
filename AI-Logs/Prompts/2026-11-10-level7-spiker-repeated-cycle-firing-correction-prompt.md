FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

This check detects incomplete or truncated user tasks, not appended agent
context.

If exactly one complete TEMPEST_ token cannot be established, stop before any
project or repository action and explain why.


TASK

Implement one bounded correction to canonical Level 7 Spiker firing:

A continuing live Spiker must retain ongoing firing eligibility across repeated
ascent/retreat cycles, independently of the amount of new persistent-Spike
growth available during the current ascent.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted committed baseline beneath the current uncommitted Level 7 candidate:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

The complete current uncommitted Level 7 candidate is already present in the
worktree, including:

- the bounded Level 7 concurrency diagnostic;
- seven-place ordinary capacity;
- pending-lane Spike selection;
- retained live Spiker behavior while generic supply remains;
- existing generic-obligation death reconstruction;
- the later saturated-cargo allocation correction.

Preserve that candidate except for this specifically authorized Spiker-firing
correction.

Read first:

AI-Logs/Reports/2026-11-10-level7-spiker-repeated-cycle-firing-investigation-report.md

AI-Logs/Reports/2026-11-10-level7-concurrency-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-10-level7-saturated-cargo-allocation-correction-report.md

Use the project-local Atari listing as the internal arcade authority where
needed:

Reference/arcade/Tempest Rom Source code.md

Do not repeat the completed investigation.

Do not search the Internet for ROM behavior.

This is an implementation candidate requiring automated validation and Neal's
manual acceptance.

Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 8 work.


MANUAL ARCADE / MAME / C# EVIDENCE

Treat the following as ARCADE/MAME/C# OBSERVED evidence supplied by Neal.

EXPECTED ARCADE / MAME BEHAVIOR:

- A Level 7 Spiker retains its ability to fire during later ascents.
- Completing one ascent/retreat cycle does not permanently silence it.
- A continuing Spiker remains a firing threat during repeated cycles.

CURRENT C# BEHAVIOR:

- A Spiker in an individual lane fires only during its first ascent.
- The first ascent may extend the persistent Spike only partway toward the
  rim.
- The Spiker then retreats and later ascends again.
- It may require approximately three or four ascent/retreat cycles to build
  the persistent Spike fully to the rim.
- During those later ascents, it continues building the persistent Spike but
  never fires again.

PERSISTENT SPIKE CLARIFICATION:

- Persistent Spike growth is expected behavior.
- Spikes remain persistent across Spiker cycles.
- A Blaster shot can hammer/shorten an existing Spike.
- Do not remove, reset, or otherwise weaken persistent Spike behavior as a way
  to restore firing.
- The defect is that firing eligibility disappears during later ascents, not
  that the Spike remains persistent.


CONFIRMED INVESTIGATION RESULT

The read-only investigation concluded:

B. CONFIRMED DIFFERENT MECHANISM.

ROM:

- A Spiker has the shooter attribute/flag.
- The shared enemy firing routine evaluates eligible live enemies after enemy
  movement.
- Spiker firing is governed by:
  - shooter eligibility;
  - position eligibility;
  - a per-enemy shot_delay cooldown;
  - global shot_holdoff;
  - random firing threshold;
  - global active enemy-shot count;
  - global enemy-shot slot capacity.
- There is no per-Spiker ammunition field.
- There is no lifetime Spiker shot allowance.
- Firing is not conditioned on Spike growth exceeding a threshold.
- Firing is not restricted to the event that initiates retreat.
- A continuing Spiker retains its shooter flag through its hop/continuation
  lifecycle.
- The ROM does not perform a cycle-boundary ammunition reset because there is
  no ammunition to reset.

CURRENT C#:

- Spiker has no ammunition, shot-count, or independent firing cooldown.
- Its only firing opportunity occurs in the Growth > 100 branch that can also
  trigger retreat.
- Reaching the upper-height limit can initiate retreat without a firing
  attempt.
- The persistent lane Spike reduces the amount of new Growth available during
  later ascents.
- Once the current ascent cannot accumulate Growth > 100 before the upper
  boundary, the only firing branch becomes unreachable.
- RestartForPendingSupply resets Growth and state but cannot restore firing
  eligibility because firing is structurally tied to that Growth > 100 branch.
- The growth-only firing rule predates the concurrency candidate.
- Keeping the same live Spiker active on the same lane exposes the limitation
  repeatedly.

Correct this mechanism.

Do not invent an ammunition reset.


PURPOSE

Implement the smallest maintainable canonical Level 7 translation of the ROM's
ongoing shared Spiker firing eligibility.

The correction must make firing independent of:

- Growth > 100;
- whether the Spiker is about to retreat;
- how much new persistent-Spike growth remains available;
- whether this is the first, second, third, or later ascent.

A continuing Level 7 Spiker must remain capable of firing whenever the
authorized firing conditions are satisfied.

Do not guarantee that a Spiker fires on every ascent.

Firing remains subject to the authorized cooldown, random, position, and global
projectile-capacity conditions.


CANONICAL LEVEL 7 SCOPE

Apply the new firing translation only to canonical production Level 7 and
equivalent direct/campaign/recorded paths that use the canonical Level 7
configuration.

Do not silently change Spiker firing on:

- Levels 1–6;
- Level 8;
- copied configurations;
- synthetic configurations;
- development scenarios;

unless a narrow shared implementation seam is required and tests prove that
other behavior remains unchanged.

Keep the Level 7 policy explicit.


IMPLEMENTATION DESIGN

Prefer a small independent Spiker firing-policy/state path rather than
continuing to overload Spike growth or retreat decisions.

Trace existing enemy-firing architecture before editing.

The design should represent only the minimum state supported by the ROM-backed
behavior and current project architecture.

Where practical, separate:

1. Spiker movement and persistent-Spike growth;
2. Spiker retreat decisions;
3. Spiker firing eligibility and cooldown;
4. global enemy-shot capacity/registration.

Do not create a broad universal enemy-firing refactor unless the existing
architecture makes a narrow shared helper clearly safer and smaller.

Do not implement a full ROM firing interpreter.


NO AMMUNITION

Do not add:

- per-Spiker ammunition;
- a lifetime shot count;
- a shot allowance per ascent;
- a forced shot quota;
- a firing reset at the bottom described as ammunition replenishment.

The local ROM listing supports none of those concepts for Spikers.

If a new C# field is necessary, it should represent a supported mechanism such
as a firing cooldown/delay, not invented ammunition.


FIRING ELIGIBILITY

Implement ongoing canonical Level 7 Spiker firing eligibility consistent with
the established ROM concepts.

A Spiker may be considered for firing only when all applicable translated
conditions are satisfied, including:

- it remains a live Spiker;
- it is in an eligible along-tube position;
- its shooter policy is enabled;
- its firing cooldown/delay permits an attempt;
- the applicable deterministic/random threshold permits the shot;
- global enemy-shot capacity has room.

Do not let persistent Spike height eliminate firing eligibility.

Do not require Growth > 100.

Do not require a growth-triggered retreat decision.

Do not require the Spiker to be on its first climb.

Preserve deterministic recording/playback and injectable/scripted randomness.


COOLDOWN / HOLDOFF

Translate the ROM-backed per-enemy shot_delay / global shot_holdoff concept
using the narrowest project-consistent design.

Before choosing constants:

- inspect existing Level 7 firing configuration;
- inspect current Flipper/Tanker firing delay conventions;
- inspect the local ROM Level 7 shot_holdoff loading;
- inspect the shared enemy-shot capacity and random-pressure rules;
- determine whether a suitable existing Level 7 project translation already
  exists.

Do not equate a raw ROM byte automatically with a C# tick count unless the
project already has a justified translation.

If an exact literal translation is not established, use the smallest existing
project-consistent Level 7 timing translation and classify it clearly as a
PROJECT TRANSLATION / APPROXIMATION.

Do not tune by eye in this implementation step.


RANDOM FIRING

Preserve deterministic/random behavior appropriate to the project.

Do not guarantee one shot per climb.

Do not make every eligible update fire.

Do not consume uncontrolled randomness while firing is impossible because:

- global shot capacity is full;
- the Spiker is outside the eligible position;
- the Spiker is no longer active;
- another explicit gate prevents an attempt.

Use scripted randomness in tests to establish both firing and non-firing paths.


FIRST-CYCLE BEHAVIOR

Existing first-cycle firing behavior is currently tested through the
Growth > 100 retreat branch.

Update that design carefully.

Required outcome:

- first-ascent Spikers remain capable of firing;
- later-ascent Spikers become equally capable under equivalent eligibility
  conditions;
- firing is no longer coupled to crossing Growth > 100;
- Spike-growth and retreat decisions remain correct;
- do not accidentally cause duplicate firing attempts during one update.

Existing tests that specifically encode "fire only when successful
growth-triggered retreat occurs" represent the superseded implementation
mechanism and may be updated narrowly.

Do not weaken unrelated first-cycle guarantees.


PERSISTENT SPIKE BEHAVIOR

Preserve all accepted Spike behavior:

- existing Spike height persists across cycles;
- a continuing Spiker can add more height on later ascents;
- several cycles may be needed to build a Spike fully toward the rim;
- Blaster shots can hammer/shorten the Spike;
- Spike collision and hazard behavior remain unchanged;
- persistent Spike state remains separate from Spiker firing state.

Add tests proving that a high retained Spike can prevent Growth > 100 while the
Spiker still retains a valid firing opportunity.


REPEATED CYCLES

The corrected same Spiker must remain firing-capable across:

- first ascent;
- first retreat;
- pending-supply restart;
- second ascent;
- second retreat;
- third ascent where tested.

Do not create a new enemy identity at the cycle boundary.

Do not create a new generic obligation.

Do not release or reacquire a physical enemy place.

Do not return the Spiker through an Egg.

Preserve identity and occupancy throughout the continuing lifecycle.


RETREAT FIRING BOUNDARY

The ROM shared routine may continue evaluating a live enemy when position and
flags qualify, without making firing a growth-triggered action.

Determine the narrow C# phase policy required for faithful visible behavior.

At minimum, later ascents must fire.

Do not unnecessarily broaden firing into phases that current evidence does not
require.

If firing while Retreating is directly established and safely translatable,
implement and test it.

If the existing C# state model or visible evidence supports firing only while
Building/ascending as the narrower translation, document that as a PROJECT
TRANSLATION rather than claiming literal ROM phase equivalence.

Do not guess silently.


ZERO-PENDING CONVERSION

Preserve the existing zero-pending boundary.

At actual generic pending supply zero:

- the retreating Spiker converts in place to a Flipper-carrying Tanker;
- no additional Spiker cycle begins;
- no new generic obligation is created;
- Tanker cargo remains allocated exactly once under the existing accepted
  Level 7 logic.

Do not reset or invoke a new Spiker firing cycle before conversion.

Do not modify the converted Tanker's firing/cargo behavior.


GLOBAL ENEMY-SHOT CAPACITY

Preserve the existing canonical Level 7 enemy-shot capacity unless a direct
source-backed defect within this narrow milestone requires otherwise.

A full projectile collection must prevent a new Spiker shot.

When capacity later becomes available, the live Spiker must remain capable of
a future attempt according to cooldown/random rules.

Do not change EnemyBullet speed.

Do not change projectile movement, collision, lifetime, score, rendering, or
audio.


RELATIONSHIP TO CURRENT CONCURRENCY CANDIDATE

Preserve:

- seven-place ordinary admission;
- Tanker/Spiker minima and maxima;
- zero ordinary Flippers;
- pending-lane Spike selector;
- depleted-Spike $FF handling;
- open-place dead end;
- retained same-identity Spiker behavior while generic supply remains;
- zero-pending conversion;
- generic-obligation death reconstruction;
- saturated Level 7 cargo allocation correction.

This task changes only continuing Spiker firing capability.


MOVEMENT FREEZE

Do not modify:

- Level 7 Flipper movement;
- Level 7 Tanker-cargo movement;
- Spiker climb/retreat geometry except mechanically necessary firing wiring;
- lane-hop behavior;
- persistent-Spike growth;
- rim behavior.

Do not implement the ROM Spiker lane hop in this milestone.

The same-lane continuing Spiker remains the accepted current PROJECT
TRANSLATION unless separately authorized.


OUT-OF-SCOPE BOUNDARIES

Do not change or investigate further:

- Level 7 Tanker natural-vs-player-shot cargo allocation distinction;
- enemy projectile speed;
- saturated cargo accounting beyond preserving the current correction;
- Super Zapper admission pause;
- Flipper $24 movement conflict;
- broader Level 7 difficulty;
- Levels 1–6;
- Level 8.


TESTS

Add/update narrowly focused deterministic tests.

At minimum cover:

1. FIRST ASCENT CAN FIRE
   - canonical Level 7 Spiker;
   - eligible position;
   - available global enemy-shot slot;
   - scripted cooldown/random conditions permit firing;
   - exactly one bullet is created;
   - no duplicated firing attempt occurs in the same update.

2. SECOND ASCENT CAN FIRE
   - same Spiker identity;
   - first ascent/retreat completed;
   - generic pending supply remains;
   - pending-supply restart occurs;
   - second ascent reaches an eligible position;
   - scripted conditions permit firing;
   - the same Spiker produces another bullet.

3. THIRD ASCENT CAN FIRE
   - optionally or explicitly repeat another lifecycle boundary;
   - prove firing capability is not limited to one restart.

4. HIGH PERSISTENT SPIKE
   - persistent Spike is already high enough that the current ascent cannot
     accumulate Growth > 100;
   - growth-triggered firing branch would be unreachable in the old code;
   - the Spiker can nevertheless fire through the corrected independent path.

5. PERSISTENT SPIKE UNCHANGED
   - firing does not reset Spike height;
   - restart preserves Spike height;
   - later ascent continues adding growth;
   - Blaster hammer/shortening behavior remains covered.

6. COOLDOWN
   - shot attempt is blocked before cooldown expiry;
   - successful shot reloads cooldown;
   - later attempt can occur after cooldown permits it.

7. RANDOM PATH
   - failing random threshold does not create a bullet;
   - later eligible attempt can still succeed;
   - no permanent silence results from a failed roll.

8. GLOBAL CAPACITY
   - full enemy-shot capacity prevents a new shot;
   - Spiker remains live and firing-capable;
   - freeing a slot allows a future eligible shot.

9. RETREAT / PHASE POLICY
   - assert only the phases explicitly authorized by the chosen translation;
   - prove firing is not coupled to retreat initiation.

10. IDENTITY / OCCUPANCY
    - repeated cycles retain the same Spiker identity;
    - the same physical place remains occupied;
    - no reserve identity or Egg is created;
    - no generic obligation is added.

11. ZERO-PENDING CONVERSION
    - at true generic pending zero, Spiker converts to Tanker as before;
    - no extra Spiker firing cycle is introduced;
    - cargo allocation occurs once.

12. ISOLATION
    - Levels 1–6 unchanged;
    - Level 8 unchanged;
    - copied/noncanonical configurations unchanged;
    - existing Tanker/Flipper firing unchanged.

13. CURRENT LEVEL 7 CANDIDATE
    - saturated cargo tests pass;
    - admission tests pass;
    - death reconstruction tests pass;
    - movement-freeze tests pass.


EXISTING TESTS

Inspect and update the narrow first-cycle test currently named similarly to:

SpikerFiresAtMostOnceOnlyOnSuccessfulGrowthRetreatDecision

That test encodes the superseded coupling between firing and growth-triggered
retreat.

Replace or refine only the obsolete coupling assertion.

Preserve useful guarantees such as:

- deterministic shot creation;
- at most one shot per authorized update;
- no shot when gates fail;
- bullet registration and capacity.

Do not weaken unrelated tests.


VALIDATION

Use repository-wide bounded validation and security rules.

Prefer Release configuration.

Start with the exact high-persistent-Spike second-cycle firing regression.

Then run focused tests covering:

- Spiker firing;
- Spiker/Spike lifecycle;
- Level 7 admission;
- Level 7 production;
- Level 7 saturated cargo;
- generic death reconstruction;
- zero-pending conversion;
- movement freeze.

If focused validation passes, run affected Release regressions covering:

- Levels 1–6;
- Level 8;
- Tanker/Flipper firing;
- enemy bullets;
- Spiker/Spike behavior;
- cargo;
- death/reentry;
- Super Zapper;
- rim/completion;
- progression;
- recording/playback;
- direct/campaign startup.

If affected validation passes, run:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then run:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete tracked and nonignored-untracked candidate review;
- final repository-state verification.

Explicitly report:

- compilation status;
- whether VSTest executed;
- exact passed/failed/skipped counts;
- PASS / FAIL / BLOCKED / TIMEOUT / CANCELED / PARTIAL.

Stop at the first failure, block, timeout, cancellation, security error, or
meaningful stall.

Do not weaken or bypass Windows security.


MANUAL HANDOFF

If automated validation passes:

- leave the entire Level 7 candidate uncommitted;
- do not launch the game;
- provide Neal:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10

Primary manual reproduction:

1. Keep approximately three Flippers alive on the rim as a practical setup.
2. Continue moving so the Flippers do not kill the Blaster.
3. Hold the wave in the state where Spikers undergo repeated ascent/retreat
   cycles.
4. Observe the first ascent and subsequent ascents.
5. Confirm that the same continuing Spiker can fire during later ascents.
6. Confirm that later-cycle firing does not depend on creating more than 100
   units of new persistent-Spike growth.
7. Confirm persistent Spikes continue growing normally across cycles.
8. Confirm Blaster shots still hammer/shorten Spikes normally.

Also confirm:

- no obvious projectile flood;
- global enemy-shot pressure remains sensible;
- Spiker movement is unchanged;
- zero-pending Tanker conversion remains normal;
- saturated Tanker cargo remains correct;
- Level 7 movement remains frozen;
- Levels 1–6 remain normal;
- recording/playback/frame stepping still work.

Do not claim manual acceptance until Neal reports it.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-spiker-repeated-cycle-firing-correction-report.md

Record:

- starting candidate/Git state;
- Neal's clarification;
- ROM-backed firing mechanism;
- confirmed current C# defect;
- implementation design;
- scope of canonical Level 7 activation;
- cooldown/random/capacity translation;
- persistent-Spike preservation;
- repeated-cycle behavior;
- zero-pending boundary;
- tests added/updated;
- focused/affected/full validation;
- build result;
- repository state;
- exact manual handoff;
- manual acceptance pending.


DOCUMENTATION

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to
claim acceptance during this candidate correction.

Historical reports remain unchanged.


GIT AND STOP

Do not commit.
Do not push.
Do not begin the Tanker natural-vs-shot cargo investigation.
Do not begin Level 8 concurrency.
Do not begin Level 7 movement reconciliation.

Stop after automated validation and manual handoff.


TEMPEST_LEVEL7_SPIKER_REPEAT_FIRING_CORRECTION_COMPLETE