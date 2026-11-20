IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using any tools, reading project files, running commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

The completion token begins with "TEMPEST_" and appears on a line by itself.

Verify that exactly one complete standalone token beginning with "TEMPEST_" is present in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the user's token and do not invalidate it.

The purpose of this check is to detect incomplete or truncated user tasks, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project/repository action and explain why.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Required starting checkpoint:
79e33886772424a31973332f33f3781b55e4c2f8

MILESTONE

Implement production Level 7 clover and natural Level 6→7 progression, including the narrow ROM-backed Level 7 Flipper movement capability established by the completed source investigations and Neal's direct arcade observation.

Do not implement Level 8.

Do not launch the interactive game.

Do not commit or push.

After automated validation, stop and hand the build to Neal for manual gameplay acceptance.

STEP 1 - VERIFY BASELINE

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat
git diff --check

HEAD must exactly equal:

79e33886772424a31973332f33f3781b55e4c2f8

The tracked worktree and index must be clean.

If HEAD differs or unexpected tracked/staged changes exist, STOP.

Do not clean, reset, restore, stash, or otherwise alter unexpected work.

STEP 2 - READ REQUIRED AUTHORITY

Read:

.github/copilot-instructions.md

AI-Logs\Reports\2026-11-04-level7-preimplementation-analysis-report.md

AI-Logs\Reports\2026-11-04-level7-flipper-rom-analysis-report.md

Read the relevant current repository sections of:

TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
README.md

Inspect the accepted Level 6 implementation and tests as the immediate production/progression precedent.

Inspect the referenced Atari ROM source, Incitti source/data, Flipper, Tanker, geometry, progression, reconstruction, firing, Spike exit, direct diagnostic, and campaign-soak implementations as required.

Do not repeat the completed investigations unnecessarily.

SOURCE AUTHORITY

For Tempest gameplay fidelity use this authority order:

1. Neal's directly observed Atari arcade behavior.
2. Atari ROM behavior/data.
3. Incitti reference implementation.
4. Existing C# behavior.
5. New project decisions/approximations only where stronger evidence cannot provide a direct answer.

When ROM behavior/data and Incitti conflict, use the ROM unless Neal's directly observed arcade behavior establishes otherwise.

Preserve accepted Levels 1–6.

LEVEL 7 PRODUCTION CONFIGURATION

Implement one immutable production Level 7 configuration.

Required identity:

- zero-based progression identity: 6
- displayed level: 7
- board identity: Original Arcade clover / board index 6
- palette: 0
- closed tube
- 16 vertices
- 16 playable lanes
- center Y: 310
- Y offset: -10
- depth: 400
- ready position: 50
- player reference starting lane: 7
- retain the established C# fractional starting position 0.5

Use the exact source-ordered clover coordinates:

(-160,160)
(-179,44)
(-89,0)
(-179,-44)
(-160,-160)
(-44,-179)
(0,-89)
(44,-179)
(160,-160)
(179,-44)
(89,0)
(179,44)
(160,160)
(44,179)
(0,89)
(-44,179)

Preserve the full long inward directed sweep through the four reflex hinges.

Do not approximate the clover geometry.

LEVEL 7 SUPPLY AND ENEMY COMPOSITION

Use the Atari ROM-backed Level 7 supply:

- 20 original enemy obligations

Integrate this using the existing accepted C# production representation:

- four initial Eggs
- 16 reserve original obligations

This intentionally takes precedence over Incitti's conflicting 25-original calculation.

Eligible production enemies:

- Flippers
- Tankers carrying Flippers
- Spikers

Excluded:

- Pulsars
- Fuseballs
- Pulsar cargo
- Fuseball cargo

Preserve the existing accepted production species-selection mechanism and ledger/accounting unless Level 7 requires a narrow range extension.

Do not fabricate fixed Flipper/Tanker/Spiker ratios.

STARTING SPIKES

Level 7 starts with:

- 16 Spikes
- one per lane
- C# height 60

Preserve existing Spike damage persistence, shortening, scoring, reconstruction, collision, and exit mechanisms.

LEVEL 7 AVOID SPIKES

Configure:

AvoidSpikesWaitThreshold = 30

Based on the established existing comparison semantics this produces:

- 31 Avoid Spikes updates
- then the separate existing 21-update pretravel delay
- then existing exit travel

Preserve:

- Levels 4 and 5 at their accepted 151-update behavior
- Level 6 at its accepted 71-update behavior

Do not derive this using a broad level-number formula.

LEVEL 7 FLIPPER FIRING

Extend the production firing configuration explicitly for displayed Level 7:

- successful-shot holdoff: 62
- maximum active enemy-shot capacity: 3

Preserve existing Levels 1–6 firing values exactly.

Preserve the accepted production Flipper firing phase, initial-ready policy, successful-shot-only reload, probability rules, active-pivot emission gate, height cutoff approximation, bullet behavior, audio event behavior, and shared enemy-shot pressure.

Do not restore Incitti's movement-bound firing model.

LEVEL 7 TANKERS

TANKERS THEMSELVES MUST NOT CHANGE LANES.

Neal explicitly confirms from arcade behavior and the current accepted C# implementation:

- Tankers climb straight up their current lane.
- Tankers do not switch lanes while climbing.

Preserve this behavior.

Do not apply the Level 7 two/three movement program to Tankers.

LEVEL 7 ROM FLIPPER MOVEMENT

The Atari ROM's Level 7 ordinary-Flipper program $24 is the movement authority.

Implement the smallest explicit Level-7-opt-in movement capability required to express:

1. Lead-in movement.
2. Two completed adjacent lane flips in one direction.
3. Reverse direction.
4. Three completed adjacent lane flips.
5. Reverse direction.
6. Repeat.

The movement is:

2 flips -> reverse -> 3 flips -> reverse -> repeat

This is not:

- player pursuit below the rim;
- random left/right movement;
- one persistent below-rim direction;
- Incitti's generic canflip behavior;
- a universal movement policy for all levels.

Do NOT add Incitti's 20-tick below-rim lane pause to this policy.

The ROM program does not contain that universal pause.

LEVEL 7 ORDINARY-FLIPPER STATE

Implement route state per Flipper instance.

Use the narrowest maintainable representation necessary, conceptually including:

- Two versus Three phase
- remaining completed flips in the current phase
- movement direction
- route/boundary stage sufficient to distinguish:
  - lead-in
  - flip start
  - active turning
  - completion boundary

Do not:

- put route phase/count on Level;
- share it between enemies;
- implement a generalized Atari p-code VM;
- create a speculative script engine;
- redesign all Flipper movement;
- reinterpret CanFlip for Levels 1–6.

Fresh ordinary Level 7 Flippers start fresh in the two-flip phase.

Moving the Blaster must not retarget this below-rim route.

Unrelated RNG consumption must not change it.

After two completed flips:

- reverse once
- begin three-flip phase

After three completed flips:

- reverse once
- return to the two-flip cycle with the ROM-established lead-in boundary

Completed pivots, not attempted or partial pivots, count against the two/three phase.

RIM HANDOFF

Below-rim programmed movement must remain separate from the accepted C# rim-pursuit behavior.

If a Flipper reaches the rim:

- do not continue using the two/three route as rim pursuit;
- preserve the accepted persistent rim-pursuit direction/latch and existing rim mechanics.

If a Flipper is already performing a visible pivot when reaching the rim boundary:

- preserve a complete sensible visible pivot;
- do not truncate a long reflex transition;
- do not double-count the pivot;
- do not schedule an extra below-rim flip after rim handoff.

Do not import unrelated ROM top-program behavior that would regress the manually accepted C# rim implementation.

ORDINARY INITIAL PHYSICAL DIRECTION

The ROM establishes a fixed first direction for ordinary originals but static evidence did not fully establish Atari-private segment handedness versus the imported C# clover ordering.

Initial physical direction is therefore NOT a blocker.

Choose one explicit fixed C# initial direction as a PROJECT IMPLEMENTATION APPROXIMATION.

Requirements:

- do not use player position;
- do not use RNG;
- keep the choice extremely easy to reverse after Neal's manual testing;
- centralize/name/comment the choice rather than burying an unexplained sign in movement code;
- tests must pin whichever direction is selected so that reversing it later is a deliberate one-line/configuration-level behavioral change rather than accidental drift.

Do not claim the selected physical direction is ARCADE VERIFIED until Neal establishes it.

LEVEL 7 MOVEMENT CADENCE

The ROM establishes movement-call sequencing but not a cycle-exact mapping to the C# 60-Hz simulation.

Use the narrowest reasonable C# approximation necessary to produce the ROM route.

Requirements:

- no 20-tick lane pause;
- preserve the ROM route boundaries conceptually;
- no along-tube increment on a flip-start boundary;
- active pivot continuation should advance both angular progress and along-tube progress;
- pivot completion must remain visible/collision-safe;
- next route boundary handles count/reversal semantics;
- geometry must continue affecting pivot duration;
- reflex clover pivots must retain the complete long directed sweep.

You may reuse existing C# scalar movement/turn infrastructure where that produces a clean bounded implementation, but document numeric timing as a PROJECT APPROXIMATION rather than literal ROM cycle timing.

Prefer a simple tunable Level-7-specific cadence parameter over new architecture.

Neal will judge visible speed/rhythm during manual gameplay.

Do not over-engineer exact Atari CPU-cycle emulation.

LEVEL 7 TANKER CARGO FLIPPERS

Neal has now directly observed relevant Level 7 arcade behavior:

When a Blaster shot destroys a Tanker sufficiently deep below the rim:

- the two released Flippers move away in opposite directions;
- each travels for two lane changes;
- each reverses direction;
- each then travels for at least three lane changes;
- afterward it either reaches the rim or appears to reverse again.

This directly observed behavior agrees with the ROM finding that deep released Tanker cargo may enter the Level 7 $24 movement program, with children initialized in opposite directions.

Implement this behavior narrowly for Level 7 deep player-shot Tanker cargo.

IMPORTANT:

- Tanker itself remains a straight-lane climber.
- Do not modify Levels 1–6 cargo movement.
- Do not apply the ROM route blindly to every Level 7 cargo release.

The ROM distinguishes release depth:

- near-rim natural release uses the straight/default entry
- sufficiently deep player-shot release uses the Level 7 movement program

The exact ROM along-coordinate threshold does not directly map to C# height.

Determine the narrowest safe C# threshold/integration seam using the current Tanker release lifecycle and the ROM/reference analysis.

If an exact coordinate conversion is unavailable, use a clearly named/documented Level-7-specific PROJECT APPROXIMATION that preserves the functional distinction:

- natural near-rim release does NOT incorrectly begin a long two/three program;
- clearly deep player-shot release DOES use the two/three program.

Do not describe the numeric C# threshold as an exact ROM coordinate conversion.

For deep Level 7 cargo:

- create independent fresh route state for each child;
- initialize children in opposite outward directions;
- start each in the two-flip phase;
- do not inherit Tanker movement state;
- do not share counters between siblings;
- after two completed flips reverse;
- then three completed flips;
- reverse again if still below the rim;
- rim handoff uses accepted rim behavior.

Preserve existing identity ownership, latent cargo, reconstruction and accounting semantics.

A re-created cargo Egg should not resurrect stale runtime phase state.

NATURAL LEVEL 6→7 PROGRESSION

Extend natural campaign progression so Level 7 is installed only after successful Level 6 completion and safe Spike-exit travel reaches the existing progression commit point.

Successful Level 6→7 progression must:

- carry score unchanged;
- carry remaining lives unchanged;
- create a fresh Level 7 wave/ledger;
- install the 16 height-60 starting Spikes;
- restore two fresh Super Zapper charges;
- clear stale Zapper flash/targets;
- clear outgoing runtime entities using existing fresh-level paths;
- start the accepted LevelApproach presentation;
- use the existing mixed-color star policy for exited Level 6;
- allow established player rim movement during approach;
- suppress Fire and Super Zapper during approach;
- arrive seamlessly at Level 7 Playing.

A failed Level 6 Spike exit must remain on Level 6.

Do not install Level 7 prematurely.

DIRECT LEVEL 7 DIAGNOSTIC

Add:

--level7-test

It must:

- use the exact production Level 7 configuration;
- use production Level 7 firing;
- use the same ordinary Level 7 Flipper two/three movement policy;
- use the same Level 7 deep-cargo policy;
- use normal renderer/audio/collision/accounting/reconstruction/Zapper/Spike systems;
- start with fresh Level 7 state;
- remain isolated from natural progression;
- remain terminal;
- replay/restart Level 7 after completion or Game Over according to established direct-level diagnostic policy;
- compose appropriately with --lives;
- not create a diagnostic-only enemy composition.

Do not extend deterministic visible playback or recorded-input infrastructure for this milestone.

LEVEL 7 TERMINAL POLICY

For this bounded milestone Level 7 is terminal.

Do not implement Level 8.

Natural successful Level 7 completion must stop at the established terminal boundary.

Natural restart from terminal Level 7 starts a fresh Level 1 campaign under the established policy.

Direct Level 7 restart/replay remains Level 7.

Document terminal Level 7 as a PROJECT DECISION.

FRESH STATE / DEATH / RECONSTRUCTION

Fresh Level 7:

- two Super Zapper charges
- zero stale flash
- no stale pending targets
- fresh production ledger
- fresh enemies
- fresh Spikes
- no outgoing bullets/entities
- no stale death/Hurry-Up state

Ordinary death/re-entry:

- preserve accepted score/lives policy;
- preserve remaining Super Zapper charges;
- preserve resolved identities;
- preserve damaged Spikes;
- preserve existing ledger/accounting rules;
- reconstruct unresolved obligations as established.

New Level 7 runtime Flipper route state must NOT survive through an Egg/reconstruction.

Fresh ordinary Flippers restart with fresh two/three route state.

Fresh cargo instances use the appropriate accepted/Level-7-specific initialization, never stale per-instance route counters from a prior destroyed entity.

PRESERVE LEVELS 1–6

This is critical.

Do not retrofit the new ROM movement policy into accepted Levels 1–6.

The ROM investigation found potential level-specific fidelity work for earlier levels, but those are separate future milestones.

Specifically:

- Level 1 straight climb remains untouched.
- Levels 2–6 retain their currently accepted below-rim behavior.
- Level 4 is known from ROM source to select the same $24 program, but that does NOT authorize changing accepted C# Level 4 in this milestone.
- Existing direct-level scenarios remain functionally unchanged.
- Existing natural Levels 1–6 remain regression protected.

AUTOMATED VALIDATION

Add focused deterministic tests covering at minimum:

LEVEL 7 CONFIGURATION

1. Exact identity 6/7/6/0.
2. Exact 16 clover vertices in source order.
3. Closed 16-lane topology including 15↔0 wrap.
4. Center Y 310 and offset -10.
5. Established depth/ready position.
6. Player start lane/position policy.
7. ROM-backed 20 original obligations.
8. Four initial Eggs / 16 reserve.
9. Flipper/Tanker/Spiker eligibility.
10. Pulsars/Fuseballs excluded.
11. 16 starting height-60 Spikes.
12. AvoidSpikesWaitThreshold 30.

GEOMETRY

13. Every clover adjacent directed transition works in both directions.
14. Full long reflex sweeps are preserved at the four reflex hinges.
15. Intermediate transition geometry remains inward/finite.
16. No shortest-arc substitution for long directed pivots.
17. Wrap transitions remain correct.

ORDINARY FLIPPER ROM ROUTE

18. Level 7 opts into the new route; Levels 1–6 do not.
19. Fresh route begins in two-flip phase.
20. Initial direction is fixed and unaffected by player/RNG.
21. Exact lane-destination sequence across at least two full cycles.
22. Two completed flips before first reversal.
23. Three completed flips before second reversal.
24. Partial/started pivots do not consume phase count.
25. Completed pivot is counted only at the correct subsequent route boundary.
26. Different Flippers maintain independent state.
27. Player movement does not retarget below-rim sequence.
28. No 20-tick generic lane pause is introduced.
29. Along-motion/start/continuation semantics match the approved approximation.
30. Rim handoff terminates below-rim sequence safely.
31. Long-pivot rim arrival is not truncated.
32. Accepted rim pursuit remains unchanged.

CARGO

33. Tanker itself never switches lanes.
34. Levels 1–6 cargo behavior unchanged.
35. Level 7 near-rim/natural cargo preserves the approved non-$24 path.
36. Clearly deep Level 7 player-shot cargo opts into the two/three route.
37. Deep cargo siblings initialize in opposite outward directions.
38. Each sibling has independent phase/counters.
39. Two flips then reverse then three.
40. Destroyed/reconstructed cargo does not restore stale route state.
41. Identity ownership/accounting remains exact.

FIRING

42. Level 7 holdoff 62.
43. Level 7 maximum active shot capacity 3.
44. Exact existing Levels 1–6 firing values unchanged.
45. Firing delay continues during movement as established.
46. Active pivot emission gate remains.
47. Post-pivot firing eligibility remains correct.
48. Tanker/Spiker firing regressions remain protected.

SPIKE EXIT

49. Exactly 31 Level 7 Avoid Spikes updates before ExitDelay.
50. Existing 21-update pretravel preserved.
51. Level 4/5 151-update behavior unchanged.
52. Level 6 71-update behavior unchanged.
53. Spike shooting/scoring preserved.
54. Swept collision and reverse/retry preserved.
55. Final-life Game Over preserved.

PROGRESSION

56. Level 6 natural completion resolves to Level 7.
57. Failed Level 6 Spike exit remains Level 6.
58. Successful safe Level 6 exit installs Level 7 only at the established boundary.
59. Score/lives carry.
60. Two fresh Super Zapper charges.
61. No stale flash/targets/bullets/entities.
62. Fresh Level 7 ledger/wave/Spikes.
63. Mixed stars for Level 6→7.
64. Approach input/firing-freeze policy preserved.
65. Seamless arrival at Level 7 Playing.

DIRECT MODE / TERMINAL

66. --level7-test selects exact production configuration.
67. Direct mode has production movement and firing parity.
68. Direct mode is isolated from natural campaign progression.
69. Direct completion/restart returns to fresh Level 7.
70. Natural Level 7 does not install Level 8.
71. Natural terminal restart returns to fresh Level 1.
72. Direct Level 6 remains isolated/terminal as previously accepted.

RECONSTRUCTION / REGRESSION

73. Ordinary death/re-entry resets new runtime route state while preserving accepted persistent state.
74. Cargo reconstruction remains identity-correct.
75. Levels 1–6 focused movement/progression tests remain unchanged and passing.
76. Campaign soak extends through successful arrival at Level 7 Playing.

Use existing test patterns/helpers where practical.

Do not overfit tests to arbitrary implementation details when observable state transitions are sufficient.

Run focused tests while implementing.

FINAL AUTOMATED GATE

Before reporting run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check
git status --short --branch
git diff --stat

Review the complete diff.

Confirm:

- only intended Level 7 implementation/tests/documentation changed;
- Levels 1–6 accepted behavior remains protected;
- no Level 8 implementation exists;
- no reference file changed;
- no generated gameplay recording is included;
- no extracted video frames are added to tracked repository paths;
- no audio implementation changed;
- no unrelated refactoring occurred;
- no accidental line-ending churn exists;
- no interactive game was launched;
- no commit or push occurred.

DOCUMENTATION

Update the appropriate current sections of:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Record:

- Level 7 production clover configuration;
- ROM-backed 20-original supply;
- natural Level 6→7 progression;
- Level 7 Flipper/Tanker/Spiker eligibility;
- no Pulsars/Fuseballs;
- Level 7 firing 62/3;
- Level 7 Avoid Spikes threshold 30 / 31-update behavior;
- ROM-backed $24 two/reverse/three/reverse ordinary-Flipper route;
- no generic 20-tick lane pause in that route;
- initial physical direction as PROJECT APPROXIMATION pending Neal validation;
- numeric C# cadence as PROJECT APPROXIMATION pending Neal validation;
- Neal's direct arcade observation that deep player-shot Level 7 Tanker cargo siblings separate in opposite directions for two lane changes, reverse, then travel for at least three lane changes;
- Tankers themselves remain straight-lane climbers;
- Level-7-specific deep-cargo integration and any approximate coordinate threshold;
- --level7-test as DEVELOPMENT ONLY;
- Level 7 terminal boundary as PROJECT DECISION;
- Level 8 remains unimplemented;
- Levels 1–6 remain accepted/unchanged;
- automated validation results;
- manual acceptance pending.

Preserve evidence classifications.

Do not label C# implementation/testing as ARCADE VERIFIED.

Neal's directly observed Tanker-cargo movement may be recorded as arcade observation, carefully scoped to what Neal actually observed.

Do not claim ordinary initial handedness or numeric cadence was arcade verified.

DEFERRED AUDIO

Do not address the previously documented enemy-destruction volume-balancing issue.

It remains deferred and non-blocking.

PROMPT / REPORT LOGGING

Follow:

.github/copilot-instructions.md

Use appropriately paired Level 7 implementation prompt/report names consistent with the existing date-prefixed convention.

Do not duplicate global instructions unnecessarily.

Do not stage or commit ignored AI logs.

MANUAL ACCEPTANCE HANDOFF

Do not launch the game.

After successful automated validation give Neal:

Direct Level 7:

dotnet run --project src\Tempest.Game -- --level7-test --lives 10

Natural campaign:

dotnet run --project src\Tempest.Game -- --lives 10

Manual checklist should emphasize perceptual gameplay validation rather than asking Neal to reconstruct internal source mechanics.

DIRECT LEVEL 7

Ask Neal to verify:

- correct clover board, HUD and visual geometry;
- complete closed rim;
- four inward notches;
- player/spinner movement feels normal;
- Flippers, Tankers and Spikers only;
- no Pulsars/Fuseballs;
- Tankers climb straight without lane switching;
- ordinary Flippers visibly show characteristic two/reverse/three/reverse movement below the rim;
- movement speed/rhythm looks and feels like arcade Tempest;
- no suspicious long dwell between ordinary below-rim Flipper turns;
- long reflex pivots look correct rather than taking a shortcut;
- deep player-shot Tanker cargo siblings separate in opposite directions, travel two lanes, reverse, then travel approximately three lanes before the next reversal/rim handoff;
- natural near-rim cargo still looks sensible;
- ordinary/cargo firing pressure looks reasonable;
- Spiker behavior remains normal;
- starting Spikes and shooting/collision behavior remain normal;
- two fresh Super Zapper charges;
- death/re-entry looks and behaves correctly;
- shorter Level 7 Avoid Spikes timing;
- exit collision/retry behavior;
- terminal Level 7 and direct replay/restart;
- no crash or obvious regression.

IMPORTANT MANUAL-ACCEPTANCE PRINCIPLE

Neal has hundreds of hours of experience playing Atari Tempest.

Much of Neal's fidelity judgment is learned gameplay interaction/muscle memory rather than consciously measured timing.

Treat Neal's overall "this feels right / this does not feel right" judgment as meaningful manual acceptance evidence.

Do not demand that Neal identify an exact constant before reporting a perceptual discrepancy.

If Level 7 does not feel right, stop and let Neal narrow the behavior through focused comparison before retuning.

NATURAL LEVEL 6→7

Ask Neal to verify:

- successful Level 6 Spike exit;
- mixed-color star approach;
- correct distant clover;
- SUPERZAPPER RECHARGE;
- spinner-controlled Blaster during approach;
- seamless arrival;
- score/lives carry;
- fresh Level 7 wave/Spikes/Zapper;
- normal Level 7 gameplay;
- terminal Level 7 with no Level 8;
- fresh Level 1 natural restart;
- no malformed transition, accounting issue, crash or obvious Levels 1–6 regression.

FINAL REPORT

Report:

1. Verified starting HEAD.
2. Files changed.
3. Exact Level 7 configuration.
4. ROM-backed supply decision.
5. Ordinary Level 7 $24 movement implementation.
6. Initial direction approximation selected and how easily it can be reversed.
7. Numeric cadence approximation selected and why.
8. Tanker straight-lane preservation.
9. Level 7 deep-cargo two/three integration and threshold approximation.
10. Near-rim cargo behavior.
11. Level 7 firing values.
12. Avoid Spikes behavior.
13. Natural Level 6→7 progression.
14. Direct --level7-test behavior.
15. Terminal Level 7 behavior.
16. Reconstruction/death behavior.
17. Focused test results.
18. Full solution test count.
19. Build warnings/errors.
20. git diff --check result.
21. Complete final Git status/diff summary.
22. Confirmation Levels 1–6 were not intentionally retuned.
23. Confirmation no Level 8 implementation exists.
24. Confirmation no audio implementation changed.
25. Confirmation no video/frame/reference files were committed.
26. Confirmation no game was launched.
27. Confirmation no commit or push occurred.
28. Exact manual launch commands.
29. Concise manual acceptance checklist.
30. Any remaining perceptual uncertainty for Neal to judge during actual gameplay.

STOP after reporting.

Do not commit.
Do not begin Level 8.
Do not begin earlier-level ROM movement refinements.
Do not begin audio work.

Wait for Neal's manual Level 7 acceptance.

TEMPEST_LEVEL7_IMPLEMENTATION_79E3388