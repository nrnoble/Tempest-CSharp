FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST LEVEL 1 STATIONARY RIM-COMBAT DETERMINISTIC COMPARISON

Perform the next bounded investigation milestone into the C# game's overly safe stationary rim-combat behavior.

This task is primarily TEST / HARNESS / ANALYSIS work.

Do NOT change production gameplay behavior.

The goal is to determine which established ROM-versus-C# differences actually account for Neal's arcade observation before authorizing a production correction.


REQUIRED BASELINE

Start from the accepted production Level 7 checkpoint:

3bebf42cbe9514846ab32bf4c8a12bcfdb048da7
Implement accepted production Level 7

Before doing anything else, run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat

Expected:

- branch master;
- HEAD exactly 3bebf42cbe9514846ab32bf4c8a12bcfdb048da7;
- tracked worktree and index clean.

A pre-existing untracked:

comment for AI.md

may be present. Preserve it untouched and do not stage or commit it.

Ignored AI-Logs are expected.

If HEAD differs or tracked/staged changes exist, STOP rather than resetting or discarding anything.


MASTER ROM REFERENCE

The sole authoritative disassembly reference is:

Reference/arcade/Tempest Rom Source code.md

Do NOT search the Internet for another Tempest disassembly.

Do NOT use:

- another Tempest disassembly;
- GitHub Tempest source;
- Atari 2600 Tempest source;
- another arcade ROM revision;
- MAME source;
- another C port;
- a console/computer port;
- an online source claiming to describe Tempest internals.

If the project-local master source does not establish something, mark it UNKNOWN.

Use the completed report:

AI-Logs/Reports/2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md

as the investigation foundation, but verify source-sensitive expectations against the project-local master ROM file where necessary.


NEAL'S ARCADE OBSERVATION AND FIDELITY TARGET

Treat the following as ARCADE OBSERVED.

The current C# implementation plays impressively close to Atari Tempest overall, but rim combat is too forgiving.

On Neal's original arcade machine:

- Camping in one rim lane is not a sustainable strategy.
- Continuously holding Fire does not allow the Blaster to safely kill practically every Flipper chasing it.
- Repeated single-tapping while remaining stationary is also not sustainable indefinitely.
- Flippers pursuing the Blaster around the rim are a significant source of player deaths.
- Effective play requires timing a shot so that a chasing enemy is destroyed at the appropriate moment as it begins or performs its lane change while simultaneously tracking other enemies already pursuing the Blaster.
- A skilled player may destroy one or two pursuing enemies from a stationary position but can still be overtaken by additional chasing enemies.
- In a crowded rim situation, Neal sometimes attempts to fire and has no player-shot capacity available at the needed moment. The chasing enemy then reaches the Blaster and destroys it.
- This is expected arcade behavior, not a defect.
- Even an excellent Tempest player cannot remain parked indefinitely using careful single shots to neutralize every pursuing enemy.
- Rim pressure requires movement and situational awareness.

The current C# game differs materially:

- holding Fire while stationary is too effective;
- single-tapping while stationary is also too effective;
- chasing enemies can often be destroyed as they flip toward the Blaster;
- remaining parked is therefore much safer than on the arcade machine.

This observable distinction is the ultimate gameplay-fidelity target.

Do NOT implement an arbitrary rule that kills a stationary player.

Do NOT artificially punish camping.

The correct result must emerge naturally from Atari-faithful shot availability, collision ordering, Flipper shootability, and rim pursuit behavior.


PRIOR ROM INVESTIGATION FINDINGS

The completed ROM investigation established the following relevant facts.

ROM-BACKED:

- Active player Fire is held-level driven, not single-press-only.
- The ROM has eight player-shot slots.
- While Fire remains held, player_fire can attempt one shot per gameplay pass when a slot is available.
- There is no equivalent ROM player-shot cooldown identified in that path.
- A normal Level 1 shot begins around $10 and advances +9 toward retirement at $f0, for 25 player-shot movement invocations when unobstructed.
- Normal player shots are not simply removed after the first ordinary enemy hit.
- Newly created shots receive an immediate birth-position collision check.
- Enemy movement/contact occurs before the later moving-player-shot hit pass.
- A pre-existing moving shot therefore does not automatically save the Blaster if an enemy has already made lethal contact during enemy movement.
- Flipper pivot shootability uses ROM state/position information and is not established as C#'s blanket source-lane-or-destination-lane acceptance throughout the pivot.
- Level 1 Flippers climb straight and hand off to the common rim pursuit program.
- Rim pursuit selects a direction toward the player, with a special branch involving another rim enemy.
- Rim direction persists after selection.
- Level 1 top movement uses the ROM four-count behavior and top factor 2.
- Exact ROM lane-to-lane physical timing remains UNKNOWN because the necessary angle data is not decoded in the master listing.

CURRENT C# BEHAVIOR:

- C# has ten player-shot slots.
- Held Fire uses a three-simulation-tick global firing cooldown after the immediate first shot.
- A Level 1 C# projectile normally remains active for about 67 movement updates absent a hit.
- A C# shot is removed after its first successful target hit.
- C# tests player shots against Flippers before Flipper movement/contact.
- During a pivot C# permits a hit from either SourceEdge or DestinationEdge.
- C# Level 1 rim behavior uses its existing 24-tick pause and 8-degree pivot implementation.
- C# does not reproduce the ROM's other-rim-enemy direction-selection rule.

Do not assume every mismatch contributes equally.

This milestone exists to isolate them.


CENTRAL QUESTION

Determine which of these differences materially creates the excessive stationary safety in C#:

A. shot/contact ordering;
B. broad two-lane pivot shootability;
C. 10-versus-8 shot capacity;
D. shot occupancy/lifetime;
E. rim pursuit timing;
F. a combination of the above.

Do NOT retune production gameplay during this task.


DETERMINISTIC COMPARISON STRATEGY

Create the smallest maintainable deterministic test/harness capability needed to exercise controlled Level 1 rim-combat situations.

Prefer TEST-ONLY or INTERNAL TEST-SEAM facilities.

Do not add player-visible production debug UI unless absolutely necessary.

Do not alter normal game behavior.

Do not build a generalized ROM emulator or p-code VM.

The comparison should use the real C# gameplay objects/geometry/update path wherever practical rather than inventing a disconnected mathematical model.

When an Atari-specific comparison cannot literally execute in the production C# simulation, create a narrowly scoped test model/helper based directly on the master ROM trace and clearly label it as ROM-MODEL test behavior, not production behavior.


SCENARIO 1: FINAL-PIVOT CONTACT RACE

Construct a deterministic Level 1 state containing:

- stationary Blaster;
- Flipper on the rim;
- Flipper performing its final adjacent pivot into the Blaster's occupied lane;
- an already-existing player shot positioned so that the current C# collision pass can hit the Flipper on the same simulation step that the Flipper would otherwise complete its lethal pivot.

Establish current C# outcome.

Expected based on current source:

- shot collision is processed first;
- Flipper is destroyed;
- lethal Flipper/Blaster contact is never consumed.

Then construct the directly corresponding ROM-order model:

- birth-shot exception NOT applicable because the projectile already existed;
- enemy movement/contact occurs;
- Flipper completes lethal contact;
- later moving-shot hit processing cannot retroactively save the Blaster.

This test is crucial.

Determine whether this is a demonstrated concrete safety advantage in C#.


SCENARIO 2: NEW-SHOT BIRTH-POSITION EXCEPTION

Construct a separate case in which Fire creates a NEW shot during the relevant pass.

Model the ROM's immediate birth-position collision check separately from the later moving-shot collision pass.

Verify that we do NOT incorrectly conclude:

"all shot collisions must occur after Player death/contact."

The ROM gives newly created shots an earlier opportunity.

Determine the exact distinction between:

A. a newly spawned shot that can legitimately save the player before enemy movement; and

B. an already-moving projectile whose ROM hit check occurs after enemy movement/contact.

Any future production correction must preserve this distinction if practical.


SCENARIO 3: PIVOT-LANE SHOOTABILITY

Exercise a Flipper transitioning from lane A to adjacent lane B.

Test C# separately with a player shot in:

- source lane A;
- destination lane B.

Record the portions/states of the pivot in which current C# can hit the Flipper.

Compare this with what the master ROM source actually establishes from:

- enemy segment/state;
- in-progress state;
- $02c0 or equivalent shot-time state;
- hit tolerance.

Do not invent the missing geometric interpretation.

Produce a precise statement of:

- what ROM definitely permits;
- what ROM definitely rejects;
- what remains UNKNOWN;
- where C# is demonstrably broader.

Do not change production IsHitBy behavior yet.


SCENARIO 4: SHOT CAPACITY AND SLOT EXHAUSTION

This needs to reflect Neal's arcade observation that rim pressure can leave him attempting to Fire with no available Blaster shot slot.

Create deterministic models/tests for held Fire and controlled tapping.

ROM model:

- 8 slots;
- one accepted shot maximum per gameplay pass;
- no additional player cooldown;
- normal free-slot rules;
- normal player-shot retirement behavior.

Current C# model:

- 10 slots;
- three-tick cooldown;
- current shot movement;
- current hit retirement;
- current slot recycling.

Determine under controlled no-hit and controlled-hit streams:

- how rapidly each model fills all slots;
- how long it can remain slot-exhausted;
- when the next firing opportunity appears;
- whether the C# combination makes an immediately available defensive shot more likely during repeated rim engagement;
- whether removing C# shots after ordinary hits actually works in the opposite direction and frees capacity sooner.

Do not translate ROM gameplay passes into seconds unless the master source establishes the conversion.

Compare event sequences/invocations rather than pretending the two clocks are identical.


SCENARIO 5: STATIONARY HELD-FIRE RIM PRESSURE

Create a deterministic C# Level 1 test scenario with:

- stationary Blaster;
- Fire continuously held;
- multiple Flippers already at or reaching the rim;
- controlled initial lanes/directions;
- no player spinner movement;
- deterministic enemy timing.

The purpose is not to fabricate an arbitrary "player must die after N ticks" assertion.

Measure/report instead:

- shots attempted;
- shots accepted;
- maximum concurrent shots;
- unavailable-fire attempts due to capacity/cooldown;
- Flippers reaching the rim;
- rim pivots begun/completed;
- Flippers destroyed during pivots;
- Flippers destroyed on the would-be lethal pivot step;
- player deaths;
- enemy lane/contact events.

Use suitable internal counters/result objects for tests if needed.

Do not expose those counters as production gameplay features.


SCENARIO 6: CONTROLLED SINGLE-TAP RIM PRESSURE

Repeat an equivalent deterministic scenario using deliberately timed single Fire presses instead of held Fire.

This matters because Neal's arcade observation is NOT merely that auto-fire should be weaker.

Even an excellent player carefully single-tapping while parked in one lane is eventually vulnerable to accumulating rim pressure.

Determine whether current C# collision/order semantics allow carefully timed tapping to neutralize pursuing Flippers unrealistically consistently.


SCENARIO 7: MULTIPLE PURSUERS

Create a deterministic scenario with at least enough rim Flippers to exercise:

- first pursuing Flipper;
- second pursuing Flipper;
- continued pressure from another enemy rather than a single isolated duel.

This is important because Neal's arcade strategy involves tracking multiple enemies.

The goal is not to reproduce arcade RNG.

The goal is to determine whether the current C# mechanics provide an unrealistically strong same-lane defensive funnel once several Flippers pursue the stationary player.


RIM-PURSUIT TIMING

Do NOT guess a new rim-pause value.

The previous investigation found that:

- current C# Level 1 uses a 24-tick rim pause;
- ROM uses its separate four-count/top-factor mechanism;
- exact ROM lane-transition duration is not currently calculable from the master source because required angle data is unresolved.

Therefore:

- characterize current C# deterministic pursuit timing precisely;
- characterize the ROM source-level event/counter sequence precisely;
- identify what timing information is missing;
- determine whether the collision-order experiments already explain enough of the discrepancy without a rim-speed change.

If exact arcade timing remains necessary after the deterministic comparison, recommend a focused manual/video observation rather than inventing a constant.


DIFFICULTY

Do not implement difficulty switches.

The previous investigation established:

- 00 = Medium
- 01 = Easy
- 10 = Hard
- 11 = Medium

but did NOT establish the physical factory/default cabinet switch position.

Neal believes his cabinet is probably Medium.

Do not make implementation contingent on this unless one of the deterministic scenarios materially depends on a difficulty-adjusted parameter.

The stationary-fire comparison should initially use the ROM Medium parameter path where a model needs a difficulty value, while clearly labeling that as the intended project target pending physical cabinet confirmation.

Do not call Neal's physical cabinet Medium until verified.


TEST/HARNESS ARCHITECTURE

Prefer the least invasive approach.

Acceptable examples:

- new focused test class;
- internal deterministic scenario helper used only by tests;
- narrow observability snapshot/result type internal to tests/test-support;
- deterministic construction of Flipper/player/shot state.

Avoid:

- new runtime menus;
- new gameplay flags unless genuinely necessary;
- interactive test UI;
- broad debug controller expansion;
- generalized ROM simulation framework;
- changing normal random behavior;
- changing production timing just to make tests easier.

If a tiny production-internal seam is necessary for deterministic testing, it must have zero behavioral effect during ordinary gameplay and be justified in the report.


REQUIRED OUTCOME CLASSIFICATION

At the end classify each candidate:

SHOT / CONTACT ORDERING:
- demonstrated material contributor;
- possible contributor;
- not contributor;
- unresolved.

PIVOT-LANE SHOOTABILITY:
- demonstrated material contributor;
- possible contributor;
- not contributor;
- unresolved.

SHOT CAPACITY:
same categories.

SHOT LIFETIME / SLOT RECYCLING:
same categories.

RIM PURSUIT TIMING:
same categories.

DIFFICULTY:
same categories.

Then answer:

Can the excessive stationary safety be corrected confidently without changing:

- player-shot cadence;
- player-shot travel speed;
- global simulation speed;
- accepted below-rim enemy behavior?

If yes, identify the narrowest likely production correction.

If no, identify exactly what additional evidence is required.


DO NOT IMPLEMENT THE FINAL CORRECTION

Even if the deterministic tests identify an obvious fix:

DO NOT change production gameplay in this task.

Stop with:

- evidence;
- tests/harness;
- results;
- recommended implementation contract.

Neal will review before approving a gameplay change.


REGRESSION BOUNDARY

Any test-support changes must preserve the accepted Levels 1-7 production game.

Run appropriate targeted tests for the touched seams.

Run the complete solution test suite.

Run:

dotnet build TempestCSharp.slnx
git diff --check

If a test-only harness requires production-internal seams, inspect the diff carefully to verify there is no ordinary gameplay behavior change.


DOCUMENTATION BOUNDARY

Do not change:

- README.md
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md

during this investigation.

The durable authority documents should be changed only after Neal approves and manually validates an actual production behavior correction.

Save the prompt and report under ignored AI-Logs according to project instructions.


DELIVERABLE

Save paired records such as:

AI-Logs/Prompts/2026-11-04-level1-stationary-rim-combat-comparison-prompt.md

AI-Logs/Reports/2026-11-04-level1-stationary-rim-combat-comparison-report.md

The report should include:

1. Starting repository state.
2. Confirmation of accepted baseline.
3. Master-ROM-only source boundary.
4. Neal's sharpened arcade rim-combat observation.
5. Deterministic harness architecture.
6. Final-pivot existing-shot race.
7. New-shot birth-position exception.
8. Pivot lane shootability comparison.
9. Shot-capacity/slot-exhaustion comparison.
10. Held-fire stationary-pressure results.
11. Single-tap stationary-pressure results.
12. Multiple-pursuer results.
13. C# rim pursuit timing.
14. ROM source-level rim sequence and unresolved absolute timing.
15. Candidate-contributor classification.
16. Root-cause conclusion.
17. Narrow recommended production correction.
18. Explicit mechanics that should NOT be changed.
19. Automated validation results.
20. Remaining unknowns.
21. Suggested manual acceptance after future implementation.
22. Final Git/worktree state.


FUTURE MANUAL ACCEPTANCE TARGET

Do not perform this manually now.

Design the eventual acceptance around Neal's actual arcade experience:

PASS should mean that remaining stationary on the rim is no longer a reliably sustainable strategy.

Holding Fire while stationary should not allow the Blaster to destroy practically every pursuing Flipper indefinitely.

Careful single tapping while stationary may work temporarily and may kill one or several pursuing Flippers, but should not create indefinite safety.

Multiple chasing Flippers should create genuine pressure.

The player should sometimes encounter the arcade-like situation where a shot is needed but no suitable shot is available in time, allowing a pursuing enemy to destroy the Blaster.

Survival should again reward:

- spinner movement;
- awareness of multiple rim enemies;
- deliberate shot timing;
- lane choice;
- deciding when to abandon a stationary defense.

DO NOT turn this into an enforced "anti-camping" rule.

The correct threat must emerge from the underlying mechanics.

Do not require Neal to die every time he stops moving.

A skilled player should still be able to defend a lane temporarily.

The acceptance criterion is that permanent stationary defense is no longer disproportionately safe compared with Neal's arcade experience.


FINAL GIT STATE

Do not commit or push.

At completion run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --cached --stat
git diff --check

Report exactly which tracked files changed for test/harness purposes and confirm that no production gameplay behavior was intentionally changed.

Preserve the pre-existing untracked comment for AI.md.


FINAL RESPONSE

Return:

1. Whether stationary C# safety was reproduced deterministically.
2. Which candidate causes were demonstrated.
3. Whether existing-shot/contact ordering materially changes the outcome.
4. Whether the new-shot birth exception matters.
5. Whether two-lane pivot shootability materially changes the outcome.
6. Whether 10-versus-8 shot capacity materially changes the outcome.
7. Whether shot lifetime/slot recycling materially changes the outcome.
8. Whether rim pursuit timing still requires external/manual observation.
9. Whether difficulty is relevant to this specific discrepancy.
10. The narrowest recommended production correction.
11. Which mechanics should remain unchanged.
12. Focused test result with actual counts.
13. Full-suite result with actual counts.
14. Build warnings/errors.
15. git diff --check result.
16. Files changed.
17. Report filename.
18. Confirmation that no production gameplay correction, authority-document edit, game launch, commit, push, or Internet disassembly search occurred.

Stop for Neal's review.

TEMPEST_LEVEL1_STATIONARY_RIM_COMBAT_COMPARISON_COMPLETE