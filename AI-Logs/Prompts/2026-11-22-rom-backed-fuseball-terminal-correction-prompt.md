Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the user's token do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Implement ROM-backed terminal Fuseball movement

Continue from the current uncommitted genuine Level 11 production candidate.

The read-only investigation is complete:

AI-Logs/Reports/2026-11-22-fuseball-terminal-behavior-readonly-report.md

Use that report as the factual implementation handoff.

Do not repeat the investigation.

Implement the smallest evidence-supported correction to the shared production Fuseball movement path.

## Confirmed defect

The Atari ROM does not count a fixed number of additional Fuseball traversals.

The ROM-backed terminal behavior is:

- normal Fuseball movement continues while generic pending enemy supply remains;
- the terminal condition is checked on the toward-player/rim movement leg;
- the check occurs after movement reaches or passes the player-side boundary;
- if generic pending supply is zero, Atari returns before ordinary endpoint/reversal handling;
- the Fuseball retains its toward-player/rim direction;
- it therefore continues toward the rim instead of beginning another ordinary traversal.

The number of visibly apparent additional traversals varies according to the Fuseball's direction and position when pending supply becomes zero.

Do not implement a one-cycle or two-cycle counter.

Do not infer a fixed traversal count from Neal's approximate arcade observation.

## Current C# defect

The shared C# Fuseball movement path has no equivalent terminal pending-supply decision.

Normal production Level 11 Fuseballs therefore continue through the ordinary randomized lateral/climbing/descending cycle after generic pending supply is exhausted.

The existing C# HurryUp behavior is not the Atari terminal mechanism:

- it is development-only;
- normal production Level 11 does not activate it;
- it accelerates or favors climbing but does not suppress the ROM-equivalent reversal at the correct boundary;
- it must remain separate from the new production terminal behavior.

## Objective

Add a shared Fuseball terminal decision that:

1. receives the production wave's generic pending-supply state;
2. preserves ordinary Fuseball cycling while pending supply is nonzero;
3. does not immediately redirect a Fuseball merely because pending supply becomes zero;
4. evaluates the terminal decision at the C# state boundary equivalent to Atari's toward-player endpoint;
5. when pending supply is zero at that boundary, suppresses the ordinary reversal or return to another traversal;
6. keeps the Fuseball moving toward the rim;
7. allows the existing completion system to begin exit only after the Fuseball actually reaches the rim and all other obligations are satisfied.

Do not modify the separate all-rim or ordinary completion rules merely to make the Fuseball disappear sooner.

## Terminology and coordinate translation

The Atari disassembly describes movement using along-tube coordinates and a toward-player direction bit.

The current C# Fuseball uses:

- Lateral
- Climbing
- Descending
- KillingPlayer

Translate the ROM behavior into the existing C# state machine at the closest defensible semantic boundary.

Establish in code comments and the report:

- which C# direction/state corresponds to Atari's toward-player/rim leg;
- which C# boundary corresponds to reaching or passing player_along;
- where the pending-supply predicate is evaluated;
- how the ordinary reversal is suppressed;
- how continued rimward movement is represented.

Classify this mapping as PROJECT TRANSLATION where the C# state machine has no literal ROM-equivalent state.

Do not redesign the complete Fuseball movement system if a narrow terminal state/flag or boundary decision can express the ROM behavior cleanly.

## Pending-supply source

Use the production wave's generic pending-obligation state.

For genuine Level 11, use the established production ledger/state that represents generic originals still waiting in Reserve/Egg admission states.

Do not use:

- total active enemy count;
- number of Flippers;
- number of rim enemies;
- whether the Fuseball is the final active enemy;
- all-rim state;
- Player.Lives;
- a fixed timer;
- an extra traversal counter;
- the development-only Hurry-Up latch.

Rim Flippers do not count as generic pending supply.

Other active enemies do not prevent the terminal Fuseball branch once generic pending supply is zero.

## Trigger timing

Pending supply reaching zero must not force an immediate turn from every Fuseball state.

Required behavior:

- If the Fuseball is on the away-from-rim leg when pending supply becomes zero, preserve its current ordinary movement until it naturally reaches the relevant toward-rim decision boundary.
- If pending supply becomes zero immediately after the Fuseball passed that boundary, another apparent traversal may occur before the decision is checked again.
- At the first qualifying toward-rim boundary with pending supply zero, suppress the ordinary reversal and retain rimward movement.
- Once terminal rimward movement has begun, ordinary random direction selection must not send the Fuseball away from the rim again.

Do not poll pending supply in a way that eliminates the variable delay inherent in the ROM behavior.

## Terminal state design

Use the smallest maintainable mechanism.

A narrowly scoped terminal-rimward state or flag is acceptable if necessary to preserve the C# state machine.

It must:

- be deterministic;
- belong to Fuseball movement state, not gameplay presentation;
- activate only at the qualified boundary;
- prevent another ordinary reversal;
- continue using normal movement cadence unless the ROM-backed translation requires otherwise;
- survive ordinary simulation updates until rim arrival;
- remain reconstructable or reset correctly through death/re-entry if applicable;
- not affect development Fuseballs unless their caller supplies the production pending-state semantics.

Do not use the existing Hurry-Up flag as a renamed substitute unless evidence proves it can express the exact ROM branch without changing its accepted development behavior.

Prefer keeping the two mechanisms separate.

## Rim arrival and completion

Preserve the existing separation between terminal Fuseball movement and wave completion.

A below-rim Fuseball remains an unresolved active identity and must block exit.

When terminal movement brings the Fuseball to the rim:

- preserve its production identity;
- preserve runtime representation validation;
- allow the existing all-rim/completion path to evaluate normally;
- do not directly begin exit from inside Fuseball movement;
- do not resolve or destroy the Fuseball merely because terminal movement begins.

If other unresolved obligations remain, exit must continue to wait.

If every other remaining Flipper is already on the rim and generic pending supply is zero, the Fuseball should still travel to the rim before completion can proceed.

## Preserve existing Fuseball identity integration

Do not regress the manually accepted direct-Fuseball lifecycle correction.

Preserve:

- stable original identity on production Fuseballs;
- inclusion in runtime representation collection;
- collision-order consistency;
- active-enemy counting;
- shot destruction;
- Super Zapper destruction;
- death/re-entry reconstruction;
- all-rim accounting;
- completion obligations.

Do not weaken or bypass runtime representation validation.

## Preserve development Hurry-Up

Keep the existing development-only Hurry-Up behavior for established test scenarios.

Do not change:

- its existing CLI/test activation;
- its existing acceleration behavior;
- its current purpose;
- Pulsar-test behavior;
- Fuseball-test behavior unless a narrow caller adaptation is required to keep old behavior intact.

The new ROM-backed production terminal decision must be independently testable from Hurry-Up.

A production Level 11 Fuseball must not require Hurry-Up to enter terminal rimward movement.

## Scope classification

The missing behavior is in the shared Fuseball movement implementation, but Level 11 is currently the first production level that exposes it.

Implement the correction in the most appropriate shared location, while ensuring no behavior change occurs on production Levels 1-10 where regular Fuseballs are not admitted.

Do not create an unnecessarily Level-11-specific duplicate Fuseball state machine.

Do not change Level 11's Fuseball minimum/maximum selection limits.

## Preserve accepted Level 11 work

Do not change:

- genuine Level 11 geometry;
- open topology;
- 15 playable lanes;
- spinner orientation;
- Atari model-frame mapping;
- starting Spike-height correction;
- terminal Spiker-to-Tanker correction;
- ordinary Flipper $24 behavior;
- Tanker-cargo $07/$24 behavior;
- enemy supply;
- selection limits;
- seven-slot capacity;
- Spike parameter evidence;
- Fuseball admission;
- Atari HUD;
- presentation diagnostics.

Preserve Neal's current visual calibration candidate:

--tube-y-gain 1.5 --tube-scale 1.1

Do not encode these presentation values as defaults during this task.

## Preserve Level 15

Do not modify:

- direct Level 15 startup;
- row-10 Level 15 geometry;
- Level 15's development gameplay baseline;
- displayed-level-to-shape-row mapping.

## Scope exclusions

Do not change:

- all-rim completion semantics;
- ordinary wave completion semantics;
- enemy supply;
- admission timing;
- Flipper/Tanker logic;
- Spiker behavior;
- Spike geometry;
- Super Zapper;
- scoring;
- lives;
- progression;
- playback schema;
- HUD;
- presentation transforms;
- Approach;
- ExitZoom;
- screenshot or pause behavior.

Do not inspect screenshots, images, videos, UserData, Temp, or ROM files.

All required evidence is already recorded in the read-only report.

## Automated tests: ordinary cycling

Add deterministic coverage proving:

1. Pending generic supply above zero preserves ordinary Fuseball cycling.
2. A Fuseball does not terminal-switch merely because it is currently the only below-rim enemy.
3. Rim Flippers do not independently activate terminal Fuseball movement.
4. Other active enemies do not alter the pending-supply predicate.
5. The ordinary randomized lateral/climbing/descending behavior remains available before terminal activation.

## Automated tests: trigger timing

Add deterministic coverage proving:

1. Pending supply reaching zero during the away-from-rim leg does not immediately reverse the Fuseball.
2. The Fuseball continues until the ROM-equivalent toward-rim decision boundary.
3. Zero pending supply is evaluated at that boundary.
4. The first qualifying boundary suppresses ordinary reversal.
5. No explicit cycle counter is involved.
6. Different positions/directions when pending becomes zero can result in different apparent delays.
7. The terminal decision remains deterministic for a fixed input/random sequence.

## Automated tests: terminal movement

Verify:

1. Once activated, terminal Fuseball movement remains directed toward the rim.
2. Ordinary random direction selection cannot send the terminal Fuseball away again.
3. The Fuseball reaches the rim through normal simulation updates.
4. The Fuseball retains the same original production identity.
5. Runtime representation validation remains correct.
6. A below-rim terminal Fuseball still blocks completion.
7. Reaching the rim does not itself bypass unresolved ledger obligations.
8. With all remaining enemies at the rim and all obligations otherwise resolved, the existing completion path can proceed.
9. Other rim Flippers do not block the terminal movement trigger when pending supply is zero.
10. Death/re-entry during an unresolved terminal Fuseball state remains identity-conserving and reconstructable.

## Hurry-Up separation tests

Verify:

1. Normal production Level 11 terminal behavior works with Hurry-Up false.
2. The new terminal decision does not require development flags.
3. Existing Fuseball-test Hurry-Up behavior remains unchanged.
4. Existing Pulsar-test Hurry-Up behavior remains unchanged.
5. Hurry-Up and terminal movement cannot cause duplicate movement increments or contradictory direction changes if both are present in a development scenario.
6. The mechanisms remain separately observable in tests.

## Existing regression protection

Also verify:

- direct Fuseball identity soak;
- Level 11 runtime-representation regression;
- Level 11 Spike-height correction;
- Level 11 terminal Spiker conversion;
- Flipper/Tanker cargo movement;
- spinner behavior;
- Level 11 Atari projection;
- HUD;
- Level 15 preservation;
- playback determinism;
- Levels 1-10 behavior.

## Validation

Run bounded:

1. New Fuseball terminal-trigger tests.
2. New timing/boundary tests.
3. New terminal-rimward movement tests.
4. Existing Fuseball and Hurry-Up tests.
5. Level 11 focused tests.
6. Runtime representation/ledger/reconstruction tests.
7. Spiker/Tanker/Flipper regressions.
8. Relevant completion/all-rim tests.
9. Presentation/HUD/playback regressions.
10. Complete solution test suite.
11. Release solution build.
12. git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation provide:

dotnet run --project src\Tempest.Game -- --level 11

Neal should intentionally preserve a Fuseball late in the wave and observe:

- while generic pending supply remains, the Fuseball can continue ordinary cycling;
- pending supply reaching zero does not cause an unnatural immediate reversal from every state;
- at the next qualifying toward-rim boundary, the Fuseball stops beginning new ordinary traversals;
- the Fuseball continues to the rim;
- the number of apparent additional traversals can vary with its current state;
- all-rim exit waits until the Fuseball reaches the rim;
- the wave completes normally.

Also provide:

dotnet run --project src\Tempest.Game -- --atari-presentation --level 11 --tube-y-gain 1.5 --tube-scale 1.1

This is for continued gameplay observation only.

Do not encode a fixed extra-cycle count.

## Records and Git

Save this prompt and the correction report under the tracked AI-Logs convention.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked work and historical records.

Do not reset, restore, stash, clean, or rebase.

Stop after the bounded Fuseball correction, automated validation, and manual handoff.

TEMPEST_ROM_BACKED_FUSEBALL_TERMINAL_CORRECTION