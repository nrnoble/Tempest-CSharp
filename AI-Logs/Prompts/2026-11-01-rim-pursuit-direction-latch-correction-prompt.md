IMPORTANT: READ THIS ENTIRE PROMPT BEFORE DOING ANYTHING.

Do not use tools, inspect files, run commands, modify the repository, or perform any other work until you have read this entire prompt.

A unique completion token appears exactly once on the FINAL line.

If the unique completion token is missing, duplicated, or is not the final line, STOP before any repository action and report:

STOPPED: The complete prompt was not received.

RIM ENEMY PERSISTENT CHASE-DIRECTION CORRECTION

STARTING CHECKPOINT

09a558e Implement natural Level 3 to Level 4 progression

PURPOSE

Correct an arcade-behavior discrepancy discovered after Level 4 progression was completed.

CURRENT C# BEHAVIOR

Once a Flipper or other rim-pursuing enemy reaches the player rim, its chase logic can reconsider the Blaster's location and change direction so that the enemy follows the currently shorter route.

This can cause an enemy already chasing clockwise/right to reverse and chase counterclockwise/left if the Blaster moves behind it, or vice versa.

EXPECTED ARCADE BEHAVIOR — ARCADE VERIFIED BY NEAL

Once a rim enemy chooses a direction in which to pursue the Blaster, that chase direction remains committed.

Example:

- Flipper reaches the rim;
- Flipper begins chasing the Blaster to the right;
- Blaster subsequently moves behind the Flipper and becomes only one lane to its left;
- left is now the geometrically shorter route;
- the Flipper must NOT reverse;
- it continues pursuing to the right.

Likewise for a Flipper committed left: it continues left even if right later becomes shorter.

This means a rim enemy can intentionally take what has become the long way around the rim.

Do not interpret this as "always choose clockwise" or "always choose counterclockwise."

The requirement is:

CHOOSE/ESTABLISH DIRECTION
-> LATCH THAT DIRECTION
-> CONTINUE USING THAT DIRECTION WHILE THAT RIM-PURSUIT LIFECYCLE REMAINS ACTIVE.

This observation is consistent with an already accepted broader Tempest principle: directed movement is not necessarily replaced by the geometric shortest route. The previously accepted 270-degree directed Flipper transitions likewise retain their required directed route rather than taking an opposite 90-degree shortcut.

1. VERIFY CLEAN BASELINE

After prompt-completeness verification:

- confirm HEAD is exactly 09a558e;
- confirm no tracked changes;
- confirm TEMP/ remains untracked;
- inspect recent Git history.

If tracked changes exist, STOP and report them.

Do not modify anything until the clean baseline is confirmed.

2. INVESTIGATE BEFORE IMPLEMENTING

Inspect the literal C# implementation governing rim pursuit for every currently implemented enemy type that can legitimately pursue the Blaster around the rim.

At minimum inspect:

- Flipper;
- normal production Flippers;
- Tanker-released cargo Flippers;
- Pulsar;
- Fuseball;
- Tanker;
- Spiker;

but change only enemy types whose actual lifecycle contains relevant rim-pursuit behavior.

Trace:

- rim arrival;
- any rim pause;
- initial chase-direction selection;
- destination-lane selection;
- directed pivot/transition construction;
- completion of a rim transition;
- selection of the next rim destination;
- any call to FindShortestDirection or equivalent;
- any recalculation based on the current Blaster lane;
- death/reconstruction;
- new enemy creation/re-hatch.

Do not assume all enemy types share identical rim behavior.

3. INSPECT INCITTI

Inspect the literal Reference/incitti source for the equivalent rim/chase behavior.

Determine for each relevant enemy type:

- whether a direction field/state exists;
- when it is selected;
- whether it is retained;
- whether it can reverse;
- whether it is recalculated based on player position;
- whether closed and open tubes differ;
- how endpoint behavior works on non-looping boards, if relevant;
- whether death/reconstruction/new enemy creation resets direction;
- whether Tanker cargo has distinct handling.

Classify literal findings as REFERENCE VERIFIED.

Neal's direct arcade observation has higher authority if Incitti differs.

Do not modify Reference/incitti.

4. DETERMINE ROOT CAUSE

Specifically determine whether current C# behavior is caused by:

- FindShortestDirection being called repeatedly;
- chase direction being recomputed after every completed rim transition;
- lack of a persistent rim-direction field;
- direction being overwritten as the Blaster crosses behind an enemy;
- or another mechanism.

Report the actual cause rather than assuming it.

5. IMPLEMENT THE GENERAL ARCADE RULE

For each applicable rim-pursuing enemy:

- establish its chase direction at the appropriate source-backed/arcade-consistent point;
- preserve that direction for the remainder of that continuous rim-pursuit lifecycle;
- use that direction when choosing subsequent adjacent rim destinations;
- do not reverse merely because the Blaster becomes geometrically closer in the opposite direction.

Example behavior to guarantee:

Enemy committed right:
enemy moves right
-> Blaster crosses behind enemy
-> left becomes shorter
-> enemy continues right.

Enemy committed left:
enemy moves left
-> Blaster crosses behind enemy
-> right becomes shorter
-> enemy continues left.

Do not introduce a one-off special case for a particular lane or board.

6. DO NOT BREAK DIRECTED GEOMETRY

Preserve all accepted directed transition behavior.

Especially:

- inward endpoint-hinge motion;
- 90-degree convex transitions;
- 270-degree reflex transitions;
- required long 270-degree route;
- sensible 180-degree transitions;
- correct landing lanes;
- transition duration based on the actual directed angular sweep;
- shoot/move-under opportunity during long pivots;
- no outward arcs;
- no snapping;
- no tube crossing.

Persistent rim pursuit direction and geometric directed pivot construction must cooperate.

Do not replace DirectedLaneTransition with simpler shortest-path interpolation.

7. CLOSED VERSUS OPEN TUBES

Current production Levels 1-4 are closed boards where applicable to this behavior.

Do not implement future open-tube support merely to solve this correction.

If Incitti reveals that open tubes require endpoint reversal or other behavior, record it as future/reference behavior only unless existing implemented development scenarios already require it.

Do not broaden this milestone into implementation of later open tube geometries.

8. LIFE-CYCLE RESET

Determine when a latched rim direction legitimately ceases to exist.

At minimum evaluate:

- enemy destruction;
- all-rim clearing;
- ordinary player death and enemy reconstruction;
- Spiker retreat;
- Tanker cargo creation;
- fresh Egg/re-hatch;
- new level;
- full game restart.

A newly constructed/re-hatched enemy may establish a fresh direction according to the appropriate selection rule.

Do not accidentally preserve stale runtime direction across reconstruction if the current/source lifecycle creates a fresh enemy.

9. FLIPPER COVERAGE

Flippers are the primary manual acceptance target.

Protect:

- normal below-rim climbing;
- Level 2 canflip;
- Level 3/4 geometry-aware transitions;
- rim pause;
- rim pursuit;
- pivot shootability;
- shot-before-player-collision ordering;
- direct rim-arrival lethality;
- completed-pivot lethality;
- score;
- death reconstruction.

Test both initial direction choices if deterministic seams permit.

10. TANKER CARGO FLIPPERS

Explicitly determine whether Tanker-released Flippers use the same rim pursuit rule once they reach the rim.

Preserve their already accepted special below-rim behavior:

- distinct non-Tanker cargo lanes;
- outward lane routing below the rim;
- geometry-aware inward visible pivots;
- stable below-rim direction;
- correct rim arrival.

Do not conflate cargo below-rim route direction with persistent rim-pursuit direction unless the source/current architecture explicitly uses the same state.

11. PULSAR / OTHER ENEMY COVERAGE

For Pulsars and any other implemented enemy that can chase on the rim:

- inspect actual behavior;
- apply the same persistent-direction rule only where arcade/source behavior supports it;
- preserve Pulsar pulse timing/hazard behavior;
- preserve Fuseball movement;
- preserve Spiker retreat;
- preserve Tanker behavior.

Remember that Pulsars are DEVELOPMENT ONLY before natural Level 17.

Do not activate Pulsars in natural Levels 1-4.

12. AUTOMATED ADVERSARIAL TESTS

Add deterministic tests specifically designed to defeat shortest-path recomputation.

At minimum for Flippers:

RIGHT-COMMITTED CASE

- establish Flipper on rim;
- establish rightward pursuit;
- allow at least one relevant rim movement/pivot;
- move Blaster behind the Flipper so left becomes strictly shorter;
- prove the Flipper continues right;
- repeat the player-side crossover if practical;
- prove no reversal.

LEFT-COMMITTED CASE

Mirror the test:

- establish leftward pursuit;
- make right strictly shorter;
- prove Flipper continues left.

Do not merely assert a direction property. Verify actual destination/movement behavior.

13. GEOMETRY TESTS

Include non-circular geometry where meaningful.

At minimum protect Level 2/3/4 geometry integration.

A direction latch must not result in:

- wrong destination lane;
- incorrect directed-angle choice;
- short-cut 270-degree transition;
- outward pivot;
- transition duration regression.

Reuse existing geometry tests rather than duplicating large suites.

14. PLAYER CROSSOVER STRESS CASE

Add a deterministic test in which the Blaster changes sides relative to a rim enemy multiple times.

The enemy must not oscillate:

right -> left -> right -> left

in response to shortest-path changes.

Once committed, it stays committed until the legitimate lifecycle reset.

15. DEATH / RECONSTRUCTION

Protect:

- active rim enemy retraction during player death;
- unresolved identity conservation;
- fresh Egg reconstruction;
- correct new enemy creation;
- no stale rim-direction state leaking into the reconstructed enemy unless literal established behavior specifically requires it;
- destroyed identities remain terminal;
- Tanker cargo identity remains exact.

16. ALL-RIM / COMPLETION

Persistent chase direction must not alter:

- all-rim qualification;
- all-rim clearing;
- kill-scored destruction;
- enemy accounting;
- Level 3 ledger;
- Level 4 ledger;
- completion blockers;
- progression.

This is a movement/pursuit correction, not an accounting change.

17. NATURAL LEVEL 1-4 REGRESSION

Protect the accepted natural campaign:

Level 1
-> Level 2
-> Level 3
-> Level 4
-> terminal

Do not alter:

- between-level approach;
- score/lives carry;
- Level 4 Spikes;
- Avoid Spikes behavior;
- corrected Avoid Spikes firing boundary;
- Super Zapper;
- death presentation;
- restart semantics.

18. DIRECT DIAGNOSTIC REGRESSION

Protect existing development modes where affected:

--level2-test
--level3-test
--level4-test
--tanker-test
--spiker-test
--fuseball-test
--pulsar-test

Do not redesign their wave composition.

19. DOCUMENTATION

Update narrowly:

README.md
TEMPEST-BEHAVIOR-SPEC.md
TEMPEST-DEVELOPMENT-NOTES.md

Record as ARCADE VERIFIED:

- once a rim-pursuing enemy establishes a chase direction, it does not reverse merely because the opposite route later becomes shorter;
- a rightward pursuer stays rightward;
- a leftward pursuer stays leftward;
- therefore pursuit can become the long route after the Blaster crosses behind the enemy.

Do not claim the hidden direction-selection implementation is arcade-verified unless directly observed.

Record literal Incitti behavior separately as REFERENCE VERIFIED.

Record C# state representation as PROJECT DECISION where appropriate.

20. LONG-TERM PLAN

Preserve existing long-term notes:

- larger 1080p-friendly window milestone;
- visual/entity fidelity;
- vector-style text;
- 16 reusable tube geometries;
- Level 17 normal Pulsars;
- palette cycles;
- post-99 random geometry;
- 999,999 displayed score / rollover at 1,000,000 observation;
- deterministic replay/attract-mode investigation.

Do not implement any of these during this correction.

21. AI WORK RECORD

Archive this exact prompt under:

AI-Logs/Prompts/2026-11-01-rim-pursuit-direction-latch-correction-prompt.md

Create:

AI-Logs/Reports/2026-11-01-rim-pursuit-direction-latch-correction-report.md

Record:

- starting checkpoint;
- C# root cause;
- Incitti findings by relevant enemy type;
- affected enemy types;
- unaffected enemy types;
- implementation approach;
- lifecycle/reset policy;
- adversarial tests;
- geometry regression;
- death/reconstruction;
- natural campaign regression;
- full validation;
- manual acceptance pending.

Do not create a reasoning transcript.

22. FOCUSED VALIDATION

Run the directly relevant tests first.

Then run the actual existing test classes covering:

- Flipper rim behavior;
- SimulationTests;
- Level 2 geometry/canflip;
- Level 3 geometry;
- Tanker cargo;
- Pulsar if affected;
- Level3MixedWave;
- Level3WaveLedger;
- Level4Candidate;
- PlayerDeathPresentation;
- LevelProgression.

Use actual test names and valid filter syntax.

23. FULL VALIDATION

Then run:

dotnet test TempestCSharp.slnx --no-restore

dotnet build TempestCSharp.slnx --no-restore

git diff --check

Inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git ls-files --others --exclude-standard
git log -1 --oneline

Confirm:

- HEAD began at 09a558e;
- no Level 5;
- no new geometry;
- no open-tube implementation;
- no Pulsar production activation;
- no window/fullscreen work;
- no vector-font work;
- no attract-mode implementation;
- no score-rollover implementation;
- no unrelated gameplay redesign;
- Reference/incitti unchanged;
- TEMP/ remains untouched.

24. MANUAL ACCEPTANCE HANDOFF

If automated validation passes, STOP before commit.

Provide the smallest practical manual scenarios needed to observe the correction.

Primary acceptance goal:

- get a Flipper onto the rim;
- observe which direction it begins pursuing;
- move the Blaster behind it so the opposite direction becomes obviously shorter;
- confirm the Flipper DOES NOT reverse;
- repeat if necessary;
- verify the Flipper continues the originally established direction even when this becomes the long way around.

Test both rightward and leftward pursuit when practical.

Also confirm:

- visible pivots still swing inward;
- no snapping/outward arc;
- Flipper remains shootable during pivots;
- completed pursuit into the Blaster remains lethal;
- no crash.

If an existing diagnostic is ideal, use it.

If no existing diagnostic reliably exposes both directions, add only the smallest DEVELOPMENT ONLY deterministic/manual seam necessary.

Do not require a complete Level 1 -> 4 replay unless automated testing finds a progression-sensitive change.

25. END REPORT

Report only:

- C# root cause;
- Incitti findings;
- affected enemy types;
- implementation correction;
- direction-selection/reset policy;
- adversarial right-case result;
- adversarial left-case result;
- crossover stress result;
- geometry result;
- Tanker cargo result;
- other enemy result;
- death/reconstruction result;
- progression regression result;
- tests added/changed;
- focused result;
- full-suite result;
- build result;
- git diff --check result;
- changed-file scope;
- protected-scope result;
- Reference/incitti status;
- report/prompt paths;
- exact manual acceptance command/checklist;
- blocker, if any.

Do not commit.
Do not push.
Do not declare the correction complete until Neal manually validates visible rim pursuit.

TEMPEST-RIM-PURSUIT-DIRECTION-LATCH-CORRECTION-COMPLETE-4D91B7