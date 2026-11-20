IMPORTANT: READ THIS ENTIRE PROMPT BEFORE DOING ANYTHING.

Do not use tools, read files, inspect Git, run commands, run tests, edit files, or perform any repository work until you have read this entire prompt from beginning to end.

A unique completion token appears ONCE and only once, on the FINAL line of this prompt.

After reading the entire prompt:

- If you reached the unique completion token on the final line, proceed.
- If you did NOT reach a unique completion token on the final line, STOP immediately.
- If stopped, respond only:

STOPPED: The complete prompt was not received.

Do not reconstruct missing instructions from previous chats, reports, source code, memory, or inference.

The completion token is intentionally NOT shown here. You must read to the very end to obtain it.

BETWEEN-LEVEL APPROACH IMPLEMENTATION

CURRENT CLEAN CHECKPOINT

df0dc19 Complete natural Level 3 progression

PRIMARY AUTHORITY

AI-Logs/Reports/2026-11-01-between-level-approach-preimplementation-analysis-report.md

Supporting authority:

- current committed source and tests;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- README.md;
- AI-Logs/Reports/2026-11-01-c4-natural-level3-progression-implementation-report.md;
- current C3 architecture/lifecycle documentation;
- Reference/incitti read-only where needed.

Do not rely on an older planning document where current source or the between-level analysis supersedes it.

ACCEPTED PRODUCTION BASELINE

Natural production progression is accepted:

Level 1 -> Level 2 -> Level 3 -> terminal

At checkpoint df0dc19:

- final C4 focused tests: 494/494;
- full solution: 527/527;
- build: 0 warnings, 0 errors;
- git diff --check passed;
- natural Level 1 -> Level 2 -> Level 3 manually accepted;
- score/lives carry manually accepted;
- Level 3 board/HUD and fresh mixed wave manually accepted;
- two fresh Level 3 Super Zapper charges manually accepted;
- Level 3 terminal/no Level 4 manually accepted;
- restart to Level 1 manually accepted;
- no crashes observed.

C3 Level 3 graphical gameplay was previously manually accepted 5/5.

Do not reopen those milestones without a genuine defect.

ARCADE OBSERVATION / IMPLEMENTATION AUTHORITY

Neal's direct Atari observation establishes the visible target:

After the Blaster exits a completed tube:

- the incoming tube initially appears distant;
- the incoming tube grows larger during approach;
- the small Blaster is visible moving around the distant rim when the spinner is turned;
- normal enemy engagement begins only after arrival.

Do not claim exact arcade duration, scale curve, projection constants or hidden internal timing.

The analysis found that Incitti also animates an incoming distant tube, but permits gameplay activity during approach that conflicts with Neal's observed no-engagement behavior.

For this visible behavior, Neal's direct arcade observation has higher authority.

PURPOSE

Implement a bounded, interactive between-level approach for NATURAL production progression:

Level 1 exit -> Level 2 approach -> Level 2 Playing

Level 2 exit -> Level 3 approach -> Level 3 Playing

Do not change direct development-mode startup.

Do not implement Level 4.

1. EXPLICIT APPROACH STATE

Add an explicit transition/approach state rather than pretending the incoming level is ordinary Playing.

Use the naming that best fits existing LevelState conventions, such as an explicit approach state.

The approach must begin only after the existing successful natural progression commit.

The incoming level/configuration is current during approach.

Do not duplicate progression resolution.

Do not create another Level 2 or Level 3 configuration.

2. PRESERVE C4 PROGRESSION SEMANTICS

The existing successful progression commit remains authoritative.

Preserve:

- Level1 -> Level2Square;
- Level2Square -> Level3Mixed;
- Level3Mixed -> terminal;
- score carry;
- lives carry;
- fresh configuration;
- fresh Level 3 ledger/wave;
- fresh Super Zapper state;
- normal Game Over restart.

The new approach is presentation/interaction between successful commit and ordinary Playing.

Do not invent another campaign transition mechanism.

3. INCOMING LEVEL OWNERSHIP

At approach start:

- incoming Level is current;
- its configuration is the real production configuration;
- its fresh state may exist as required by the accepted initializer;
- Level 3's Level3WaveLedger may exist;
- initial incoming Eggs/state may exist.

However, the incoming wave must remain GAMEPLAY-FROZEN until arrival.

Do not allow the approach animation to consume or progress gameplay obligations.

4. GAMEPLAY FREEZE DURING APPROACH

During approach do NOT advance:

- Egg release schedule;
- Egg roaming;
- Egg approach/hatching;
- enemy updates;
- Tanker firing;
- enemy bullets;
- player shots;
- Super Zapper target timers;
- Super Zapper activation;
- collisions;
- death logic from enemy hazards;
- Hurry-Up;
- all-rim;
- ordinary completion logic.

Do not secretly simulate the incoming wave while it is visually distant.

At arrival, ordinary Playing begins from the correct fresh incoming state.

5. PLAYER MOVEMENT DURING APPROACH

Player movement IS enabled during the approach.

Reuse the accepted Player/InputSystem movement path as narrowly as practical.

Requirements:

- spinner movement works;
- controller movement works;
- keyboard fallback works;
- spinner direction remains unchanged;
- spinner sensitivity remains unchanged;
- cursor capture remains unchanged;
- player lane/fraction movement remains consistent with the incoming tube;
- player position reached during approach persists into Playing at arrival unless current established behavior requires a documented reset.

Do not create separate approach-only control tuning.

6. FIRE / SUPER ZAPPER SUPPRESSION

During approach:

- player firing is disabled;
- no player Shot is created;
- Super Zapper activation is disabled;
- approach input must not consume gameplay state in a way that changes the fresh wave.

This suppression is a PROJECT DECISION implementing Neal's observed "enemy engagement begins after arrival" boundary.

Do not claim exact Atari internal input policy beyond the visible behavior.

7. APPROACH PROJECTION AND RENDERING

Render the incoming tube so that it:

- starts clearly distant/small;
- grows smoothly toward the player;
- remains finite and visually stable;
- reaches EXACTLY the existing accepted normal gameplay projection at arrival.

The Blaster must render on/around the incoming distant rim and move visibly with input.

Do not casually modify normal:

- Projection.K;
- Projection.ZOffset;
- tube geometry;
- normal-game player rendering;
- Level 2 geometry;
- Level 3 geometry.

Prefer a bounded transition-specific render/projection offset or equivalent small model whose terminal value is exactly the established gameplay projection.

Rendering must not become the authority for transition completion or gameplay state.

8. APPROACH TIMING / CURVE BOUNDARY

Exact arcade approach duration and growth curve are not established.

Implement a simple deterministic starting policy suitable for manual refinement.

Classify its exact timing/curve as PROJECT DECISION / provisional presentation tuning, not ARCADE VERIFIED.

Requirements:

- deterministic;
- smooth;
- monotonic toward arrival;
- finite;
- easy to adjust later without changing gameplay/progression architecture;
- same normalized mechanism for Level 1 -> 2 and Level 2 -> 3 unless existing geometry requires only natural scaling differences.

Do not overfit arbitrary constants to Incitti merely because they exist.

9. ARRIVAL BOUNDARY

At approach completion:

- transition exactly once into Playing;
- restore/use the established normal gameplay projection exactly;
- retain the approach-selected Blaster rim position;
- incoming fresh wave has not advanced prematurely;
- ordinary gameplay begins normally on the next appropriate simulation boundary;
- shooting/Super Zapper become available according to normal Playing rules;
- Eggs/enemies begin normal simulation;
- no duplicate initialization occurs.

Avoid an arrival frame that accidentally performs both frozen approach and multiple Playing updates.

10. SHARED NATURAL TRANSITION

Use the same implementation architecture for:

Level 1 -> Level 2
Level 2 -> Level 3

Do not copy/paste separate approach implementations.

Level-specific board shapes should naturally use their existing LevelConfiguration/LevelDefinition.

11. DEVELOPMENT MODES BYPASS APPROACH

Preserve direct launch behavior:

--level2-test
--level3-test
--level3-geometry-test
--tanker-test
--spiker-test
--fuseball-test
--pulsar-test

These scenarios should begin according to their existing direct-development semantics.

Do not force an artificial approach merely because the implementation now supports one.

No new CLI mode is required unless implementation reveals a concrete testability problem that cannot be solved with existing seams.

12. TERMINAL LEVEL 3

Natural Level 3 still ends terminally.

No approach follows terminal Level 3 because no Level 4 exists.

Preserve:

Level 1 -> approach -> Level 2
Level 2 -> approach -> Level 3
Level 3 -> terminal

Do not introduce Level 4 or a cycle.

13. C3/C4 HARD PROTECTION BOUNDARY

Do NOT retune or redesign:

- Level3Mixed composition;
- Level3WaveSelection;
- Level3WaveLedger;
- Level 3 Egg scheduling;
- six-active-Tanker behavior;
- Tanker firing;
- Tanker cargo semantics;
- cargo route/pivot;
- Super Zapper gameplay semantics;
- death/reconstruction;
- completion/all-rim;
- directed lane-transition geometry;
- Flipper behavior;
- score/lives carry;
- spinner/controller tuning;
- default Flipper firing.

If the approach exposes a genuine defect in accepted C3/C4 behavior:

STOP at a clean recoverable point and report it.

Do not bury unrelated fixes inside this milestone.

14. MAINTAINABLE APPROACH MODEL

The analysis recommends a small supporting transition model.

Use the narrowest useful abstraction to own approach presentation state such as:

- progress/time;
- transition-specific depth/scale translation;
- completion state.

Do not move unrelated TempestGame responsibilities merely to make the architecture look cleaner.

Do not perform a broad refactor.

Add maintainability-focused comments only where they explain non-obvious state ownership or arcade/project-decision boundaries.

15. AUDIO LONG-TERM BOUNDARY

Do NOT implement sound in this milestone.

Sound effects are explicitly part of the long-term Tempest design.

Inspect/reuse the existing AudioSystem architecture only if a harmless semantic integration seam is already appropriate, but do not:

- select audio assets;
- copy arcade audio;
- add an audio dependency;
- add sound playback;
- couple the approach state to a specific audio library.

The transition architecture should not prevent a future semantic "level approach/arrival" sound event.

Record audio/sound fidelity as a future roadmap item in the implementation report.

16. AUTOMATED TESTS

Add focused deterministic tests for the approach.

At minimum cover:

A. ENTRY

- normal Level 1 successful exit commits Level 2 and enters approach rather than immediate Playing;
- normal Level 2 successful exit commits Level 3 and enters approach;
- terminal Level 3 does not enter approach.

B. CARRY / OWNERSHIP

- score carries;
- lives carry;
- incoming Level configuration is correct;
- Level 3 gets the expected fresh ledger/wave identity state;
- no duplicate progression/initialization.

C. FREEZE

Across approach updates:

- Egg schedule does not advance;
- Eggs do not roam/hatch;
- enemies do not update;
- Tankers do not fire;
- enemy bullets do not advance;
- no player shots are created;
- Super Zapper cannot activate/consume a charge;
- no gameplay collision/completion/Hurry-Up/all-rim processing occurs.

Use practical assertions based on current accessible state.

Do not use reflection or inaccessible-state mutation solely for tests.

D. MOVEMENT

- left/right movement changes player rim position during approach;
- existing movement logic/rules remain in effect;
- movement survives arrival.

Do not pretend headless tests validate physical spinner feel.

E. APPROACH PRESENTATION MODEL

- starts at the intended distant endpoint;
- progresses monotonically toward gameplay;
- remains finite;
- never overshoots;
- reaches exact accepted gameplay projection endpoint;
- completion occurs exactly once.

F. ARRIVAL

- approach transitions into Playing exactly once;
- normal projection is exact at arrival;
- incoming wave remains fresh until Playing;
- subsequent gameplay executes normally.

G. BOTH NATURAL EDGES

Exercise:

Level 1 -> Level 2
Level 2 -> Level 3

H. DEVELOPMENT-MODE REGRESSION

Protect:

- --level2-test direct start;
- --level3-test direct start;
- --level3-geometry-test;
- scenario isolation.

I. TERMINAL/RESTART

- Level 3 remains terminal;
- Game Over/restart remains correct.

17. ITERATIVE VALIDATION

While implementing, run focused approach/progression tests first.

Then run a broader focused group covering at minimum:

- new approach tests;
- LevelProgression;
- LevelConfiguration;
- Level2;
- Level3Geometry;
- Level3TestMode;
- Level3MixedWave;
- Level3WaveLedger;
- SuperZapper;
- InputSystem/InputDiagnostic if affected;
- relevant Simulation/exit/restart tests.

Use actual repository test names and valid filter syntax.

18. FULL AUTOMATED GATE

Before manual acceptance run:

dotnet test TempestCSharp.slnx --no-restore

dotnet build TempestCSharp.slnx --no-restore

git diff --check

Then inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git ls-files --others --exclude-standard
git log -1 --oneline

Confirm:

- no Level 4;
- no campaign-cycle change;
- no Reference/incitti modification;
- no controller/spinner retuning;
- no C3/C4 gameplay redesign;
- no Tanker/cargo change;
- no Super Zapper gameplay change;
- no audio implementation;
- no accidentally tracked TEMP/debug artifacts;
- existing TEMP/ remains excluded.

19. TRACKED DOCUMENTATION

Update tracked documentation only where the implemented approach requires accurate current status.

Likely review:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Before Neal's manual acceptance:

- describe implementation truth;
- keep timing/curve/project specifics classified appropriately;
- do NOT claim visual/arcade manual acceptance.

Preserve the distinction between:

ARCADE VERIFIED visible observation
REFERENCE VERIFIED Incitti behavior
PROJECT DECISION implementation details
DEVELOPMENT ONLY diagnostics

Record audio/sound fidelity as a long-term roadmap item without implementing it.

20. AI WORK RECORD

Save this exact prompt under:

AI-Logs/Prompts/

Suggested filename:

2026-11-01-between-level-approach-implementation-prompt.md

Create/update:

AI-Logs/Reports/2026-11-01-between-level-approach-implementation-report.md

AI-Logs remains ignored by Git.

The report should preserve:

- starting checkpoint;
- scope;
- chosen approach architecture;
- arcade/reference distinction;
- projection/timing policy;
- input/freeze/arrival behavior;
- tests;
- full validation results;
- changed files;
- protected-scope inspection;
- manual acceptance still required;
- audio long-term note;
- blockers.

Do not turn the report into a reasoning transcript.

Agent Debug and the VS Code chat remain the detailed execution history.

21. MANUAL ACCEPTANCE HANDOFF

If automated gates pass, STOP before commit and provide Neal with a concise manual test sequence.

Primary run:

dotnet run --project src/Tempest.Game -- --lives 10

Manual objective:

Validate BOTH natural transitions:

Level 1 -> Level 2
Level 2 -> Level 3

CHECK EACH APPROACH:

- outgoing exit still looks correct;
- incoming tube appears clearly distant;
- incoming tube grows smoothly toward normal size;
- Blaster is visible on/around the distant rim;
- physical spinner moves the Blaster during approach;
- controller movement remains correct;
- keyboard fallback remains correct;
- spinner direction/sensitivity still feel correct;
- no player shots occur during approach;
- no Super Zapper activation occurs during approach;
- enemies/Eggs do not visibly engage before arrival;
- arrival is visually smooth;
- accepted normal tube projection is restored at arrival;
- Blaster rim position feels continuous across arrival;
- correct board/HUD appears;
- score/lives carry remains correct.

LEVEL 3:

- fresh mixed Level 3 wave still looks correct;
- Tanker/Flipper gameplay remains normal when observed;
- two fresh Super Zapper charges remain available after arrival;
- Level 3 remains terminal.

REGRESSION:

- no crash;
- no projection explosion/flicker;
- no control discontinuity;
- direct --level2-test and --level3-test may be spot-checked if warranted.

Do not require an exact approach duration unless Neal can confidently identify a mismatch.

Ask Neal to assess:

- approach pacing;
- distant starting size;
- growth feel;
- Blaster visibility;
- spinner movement feel;
- arrival feel;
- resemblance to remembered Atari behavior.

If presentation needs tuning, treat timing/curve adjustment separately from progression/gameplay correctness.

22. END-OF-RUN REPORT

At the end report only:

- files changed;
- approach state/model design;
- Level 1 -> 2 behavior;
- Level 2 -> 3 behavior;
- frozen gameplay boundary;
- player movement behavior;
- firing/Zapper suppression;
- projection/timing policy and evidence classification;
- arrival behavior;
- development-mode regression;
- terminal Level 3 result;
- automated tests added/changed;
- focused results;
- full solution result;
- build result;
- git diff --check;
- protected-scope result;
- Reference/incitti status;
- whether any C3/C4 defect was exposed;
- tracked documentation status;
- audio long-term note;
- exact manual command/checklist;
- blocker, if any.

Do not declare the between-level approach milestone complete until Neal performs manual graphical acceptance.

Do not commit.
Do not push.

TEMPEST-BETWEEN-LEVEL-APPROACH-IMPLEMENTATION-COMPLETE-83F2C6