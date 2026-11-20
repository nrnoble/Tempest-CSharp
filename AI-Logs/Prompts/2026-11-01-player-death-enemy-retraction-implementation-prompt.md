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

PLAYER-DEATH ENEMY-RETRACTION IMPLEMENTATION

CURRENT CLEAN CHECKPOINT

d07e6d6 Finalize accepted Blaster-lane highlighting

PRIMARY AUTHORITY

AI-Logs/Reports/2026-11-01-player-death-presentation-preimplementation-analysis-report.md

Supporting authority:

- current committed source/tests;
- TEMPEST-BEHAVIOR-SPEC.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- README.md;
- recent accepted C3/C4/approach/lane-highlight reports;
- Reference/incitti read-only where necessary.

PURPOSE

Implement ONLY the first bounded player-death presentation checkpoint:

ENEMY RETRACTION DURING PLAYER DEATH

Do NOT implement the final Atari Blaster-location death animation in this checkpoint.

The architecture must leave a clean seam for that later refinement.

ATARI OBSERVATIONS

Treat the following as Neal's direct Atari observations:

1. ACTIVE ENEMY RETRACTION

- when the Blaster is destroyed, active enemies do NOT simply disappear;
- active enemies visibly retract backward/down the tube away from the player;
- this retraction is presentation, not enemy destruction;
- enemies maintain visually coherent lane placement while retracting.

2. RETRACTING ENEMIES ARE INACTIVE

During retraction, the enemies are inactive.

They must not:

- execute ordinary enemy AI;
- pursue the Blaster;
- change lanes because of normal gameplay behavior;
- fire;
- attack;
- collide with the Blaster;
- hatch or release gameplay entities through ordinary active behavior;
- trigger gameplay hazards through ordinary active simulation;
- score;
- resolve identities;
- trigger Hurry-Up;
- trigger all-rim logic;
- trigger completion logic.

Only their death-presentation rendering position should visibly change.

3. SPIKES REMAIN

On levels/scenarios containing Spikes:

- existing Spikes remain on the web during the death/retraction presentation;
- Spikes do NOT retract with active enemies;
- Spikes do NOT disappear merely because the Blaster died.

Do not apply the enemy-retraction presentation displacement to Spike rendering.

Preserve established Spike persistence semantics.

4. RETRACTION DURATION

Neal reports that the complete visible enemy-retraction process takes no more than approximately two seconds.

Treat this as an ARCADE VERIFIED qualitative upper-bound observation.

It is NOT:

- an exact measured duration;
- an exact frame count;
- an exact tick count;
- proof of a specific interpolation curve.

The implementation must complete visible enemy retraction within that observed approximate upper bound.

Use deterministic and easily adjustable timing.

Neal's later manual comparison remains authoritative for pacing.

5. BLASTER DEATH ANIMATION

A separate visible death animation occurs at the Blaster's location in Atari.

The exact Blaster animation is NOT part of this checkpoint.

Neal may later provide arcade footage for its detailed geometry/timing.

Do not invent that animation now.

REFERENCE FINDING

The pre-implementation analysis found an important distinction:

Incitti does NOT contain a dedicated enemy-specific retreat algorithm during ordinary death.

Incitti uses a shared projection/depth shift during its death/vortex presentation.

Therefore:

- use Incitti as a REFERENCE VERIFIED engineering clue for presentation/depth movement;
- do NOT claim Incitti contains an explicit per-enemy retreat routine;
- Neal's direct Atari observation remains the behavioral authority that enemies visibly retract;
- do not copy Incitti's differing Blaster/off-screen presentation and call it Atari behavior.

1. PRESERVE CURRENT DEATH GAMEPLAY SEMANTICS

The current C# death/re-entry behavior underneath presentation is accepted and extensively tested.

Do NOT change:

- identity ownership;
- unresolved/resolved identity sets;
- ordinary destroyed outcomes;
- rim-cleared outcomes;
- Tanker original identity;
- Tanker cargo ownership/ordinal;
- latent cargo;
- materialized cargo;
- pending cargo activation;
- death reconstruction priorities;
- score;
- life-consumption semantics;
- Super Zapper gameplay semantics;
- Hurry-Up;
- level progression;
- completion rules.

This is a PRESENTATION milestone.

2. PRESENTATION-ONLY RETRACTION

Prefer the pre-implementation analysis recommendation:

- keep logical gameplay enemies frozen in their existing death state;
- preserve their actual gameplay lane/height/state;
- apply a game-owned/read-only DEATH PRESENTATION displacement when rendering them.

Do not mutate actual Enemy.Height merely to make enemies look farther away unless current source proves that a render-only approach is technically impossible.

If a presentation offset can achieve the visible behavior safely, use it.

Renderer must not own or mutate gameplay lifecycle.

The architectural intent is:

LOGICAL ENEMY STATE:
frozen and unchanged

VISIBLE ENEMY POSITION:
moves backward/down the tube through presentation displacement

This separation is important for preserving existing reconstruction semantics.

3. SMALL DEATH-PRESENTATION MODEL

If justified by the analysis/current architecture, introduce a narrowly scoped model for death-presentation progress.

It may own only presentation concepts such as:

- elapsed presentation tick/progress;
- deterministic enemy retreat amount;
- presentation completion/read-only state.

Do not build a general animation engine.

Do not move existing gameplay responsibilities merely for architectural aesthetics.

Add maintainability-focused comments where necessary so a future developer understands that visible retreat and logical reconstruction are deliberately separate.

4. EXISTING DEATH WINDOW AND RETRACTION TIMING

Preserve the currently established logical player-death lifecycle unless implementation reveals a genuine contradiction.

Fit the visible enemy retraction into that established lifecycle where practical.

Neal's arcade observation establishes:

- visible enemy retraction completes within approximately two seconds.

This is an observed upper bound, not an exact duration.

Requirements:

- deterministic timing;
- visible retraction completes within the observed approximate two-second maximum;
- timing is easy to tune later;
- no exact Atari frame/tick claim;
- no unnecessary alteration of logical life-loss/reconstruction timing;
- no retraction lingering beyond the player's remembered arcade presentation;
- Neal's manual acceptance determines whether initial pacing is visually satisfactory.

If current C# update frequency means the existing 60-update death lifecycle already falls within this bound, prefer fitting the presentation into it rather than changing the lifecycle simply to create a new number.

Do not assume 60 updates equal any specific elapsed wall-clock duration without verifying current project timing.

5. RETRACTION DIRECTION

During PlayerDying, active visible enemies should visually move AWAY from the player and backward/down the tube.

Requirements:

- monotonic retreat away from player;
- finite projected positions;
- no projection inversion/explosion;
- no sudden teleport before presentation begins;
- no score/accounting effect;
- no gameplay movement/update underneath;
- no lane movement resulting from presentation;
- works for enemies at different starting depths;
- enemies near/on the rim visibly retreat rather than simply disappear;
- deeper enemies retreat coherently rather than overtaking/reordering visually for arbitrary implementation reasons.

Do not change lane position merely to create motion.

6. COMPLETE ENEMY INACTIVITY DURING RETRACTION

Neal's Atari observation establishes that enemies are inactive during retraction.

Therefore during the full visible retraction phase do NOT allow enemies to:

- run normal Update behavior;
- pursue;
- pivot;
- select a new lane;
- fire;
- create enemy bullets;
- damage the player;
- release cargo through normal active behavior;
- perform active top/rim behavior;
- trigger ordinary gameplay timers that are currently intended to freeze during PlayerDying;
- alter enemy resolution/accounting.

Do not implement "retreat behavior" by running enemy AI in reverse.

The retreat is a presentation effect applied to frozen gameplay entities.

7. ENEMY TYPES

Apply the presentation consistently to currently relevant active enemy rendering where the existing renderer supports it:

- Flipper;
- Tanker;
- Spiker;
- Fuseball;
- Pulsar;
- released Tanker cargo Flippers.

Use one generic presentation displacement where possible.

Do not duplicate a separate death algorithm per enemy merely because enemy draw routines differ.

Preserve each enemy's existing visual shape/color/state as much as practical while retracting it.

8. SPIKES ARE EXCLUDED FROM RETRACTION

ARCADE VERIFIED observation:

Spikes remain on the web when the Blaster is destroyed.

Therefore:

- do NOT apply enemy-retraction depth displacement to Spikes;
- do NOT remove Spikes merely because PlayerDying begins;
- do NOT reconstruct Spikes as though they were enemies;
- preserve established Spike persistence;
- keep Spike geometry fixed during enemy retraction;
- do not let persistent Spikes cause additional player collisions while the player is already in the inactive death presentation unless current accepted lifecycle explicitly requires such processing.

Spikers and Spikes are different concepts for this purpose:

- the active Spiker enemy may visually retract;
- the Spike structure remains on the web.

Protect that distinction in tests.

9. LEVELS 1-3

The death retraction must work on:

- Level 1 circular tube;
- Level 2 square;
- Level 3 plus board;
- mixed Level 3 Flipper/Tanker waves.

Use existing projection/topology.

Do not special-case screen coordinates per board.

Do not change geometry.

10. EGGS / INCOMING PRECURSORS

Follow the pre-implementation analysis evidence for Eggs.

Do not invent arcade behavior where evidence remains uncertain.

If Egg visibility during death/retraction remains unresolved:

- preserve current logical Egg state;
- use the lowest-risk current presentation policy;
- document the uncertainty for future arcade-footage comparison.

Do NOT resolve/remove Egg identity merely for presentation.

Do not make Eggs active merely because visible enemies are retracting.

11. PLAYER SHOTS / ENEMY BULLETS / HAZARDS

Preserve current gameplay semantics during PlayerDying.

Do not introduce collisions or active simulation during the death presentation.

For:

- player shots;
- enemy bullets;
- Pulsar visual hazards;
- Super Zapper flash;

follow current accepted lifecycle plus the pre-implementation analysis.

Spikes have an explicit arcade rule in this prompt:

- they remain visually fixed on the web.

Where treatment of other projectiles/hazards remains uncertain, preserve established C# behavior rather than inventing Atari behavior.

Do not bundle projectile/hazard fidelity redesign into enemy retraction.

12. BLASTER PRESENTATION

Do NOT implement the final Atari Blaster-location death animation in this checkpoint.

Preserve the safest current Blaster presentation necessary to avoid regression.

The future fidelity milestone will use Neal's arcade footage to determine:

- exact animation geometry;
- timing;
- shape transformation/fragment behavior if applicable;
- relationship to enemy retraction.

Do not copy Incitti's Blaster pull-back/off-screen behavior and claim it is Atari-accurate.

Ensure the new death-presentation architecture leaves a clean place for a later Blaster animation.

13. LANE HIGHLIGHTING

The accepted Blaster-lane highlighting checkpoint is d07e6d6.

Do not redesign it.

Use the death-analysis recommendation for PlayerDying visibility.

If the death presentation requires a narrow change to whether the lane cue remains visible during PlayerDying, classify that explicitly as PROJECT DECISION unless Atari/reference evidence establishes it.

Do not alter:

- lane selection;
- exact yellow color;
- topology;
- Pulsar precedence;
- Super Zapper precedence;
- approach behavior.

14. ORDINARY DEATH

After enemy retraction presentation completes:

- existing life-loss/reconstruction semantics proceed;
- exactly one life has been consumed;
- score remains;
- already resolved identities remain resolved;
- unresolved identities reconstruct exactly as current tests require;
- Tanker/cargo conservation remains exact;
- no duplicate entity allocation occurs;
- fresh re-entry remains perceptually consistent with the accepted game.

Do not replace current reconstruction code.

15. GAME OVER

Final-life death must remain correct.

Use the current established boundary so:

- enemy retraction presentation occurs coherently;
- lives do not decrement twice;
- no reconstruction happens if current semantics prohibit it on Game Over;
- Game Over appears at the established point;
- restart remains unchanged.

Do not redesign Game Over presentation.

End-of-game animation is a separate future milestone.

16. DIRECT DEVELOPMENT MODES

The same ordinary death presentation should work naturally where current development scenarios use the ordinary death lifecycle:

- --level2-test;
- --level3-test;
- --level3-geometry-test where applicable;
- --tanker-test;
- --spiker-test;
- --fuseball-test;
- --pulsar-test;
- enemy-bullet scenarios.

Do not add a new CLI mode unless current testability genuinely requires one.

17. AUTOMATED TESTS

Add focused deterministic tests for death-presentation retraction.

At minimum prove:

A. PRESENTATION START

- lethal collision begins PlayerDying exactly once;
- death presentation starts at the same boundary;
- no initial visual teleport.

B. RETRACTION

- enemy presentation depth moves monotonically away from player;
- render positions remain finite;
- actual Enemy.Height remains logically unchanged if using presentation offset;
- enemy gameplay state remains frozen;
- lane remains unchanged;
- different starting depths remain coherent;
- near-rim enemies visibly retreat.

C. TIMING

- retraction presentation duration is deterministic;
- visible retraction completes within the implementation's configured bounded duration;
- implementation does not exceed Neal's approximate two-second observed maximum under the project's established simulation timing;
- tests do not falsely label that chosen duration arcade-exact;
- presentation does not advance outside PlayerDying.

D. ENEMY INACTIVITY

During retraction:

- no normal enemy movement;
- no pursuit;
- no lane-transition initiation;
- no Tanker firing;
- no normal cargo release;
- no enemy collision;
- no completion processing;
- no Hurry-Up/all-rim processing;
- no scoring/resolution from presentation.

E. IDENTITY / ACCOUNTING

Across death presentation:

- destroyed/resolved identities remain resolved;
- unresolved identities remain unresolved;
- counters are unchanged by presentation;
- no score is awarded;
- no identity is duplicated/lost.

F. LEVEL 3 / TANKER CARGO

Protect:

- Tanker original ownership;
- latent cargo pair;
- materialized cargo units;
- cargo owner/ordinal;
- pending activation;
- terminal cargo outcomes;
- repeated death/re-entry identity conservation.

Do not weaken existing C3 tests.

G. SPIKES

Prove:

- existing Spikes remain represented during PlayerDying;
- Spike geometry/height does not retract;
- Spike does not receive enemy-presentation displacement;
- active Spiker enemy may retract independently from its persistent Spike;
- Spike persistence after the ordinary death lifecycle remains exactly as established.

H. EGGS

Protect current accepted Egg identity/lifecycle.

If visual Egg death behavior is not changed, lock that decision down appropriately without claiming arcade verification.

I. PROJECTILES / HAZARDS

Protect existing death behavior for player shots, enemy bullets, Pulsars and Super Zapper state.

J. LEVELS

Exercise representative death presentation on:

- Level 1;
- Level 2;
- Level 3 mixed wave.

K. GAME OVER

- final-life presentation remains coherent;
- life accounting occurs exactly once;
- Game Over/restart remains correct.

L. LANE HIGHLIGHT

Protect accepted d07e6d6 rendering behavior outside any explicitly justified PlayerDying visibility change.

M. PRESENTATION MODEL

If a model is introduced:

- deterministic progress;
- bounded;
- monotonic;
- exact completion;
- no advancement outside PlayerDying;
- reset correctly on next life/restart.

18. REGRESSION GATE

Run new focused death-presentation tests first.

Then a broader regression group covering at minimum:

- new death-presentation tests;
- SimulationTests death/restart;
- Level3MixedWaveTests;
- Level3WaveLedgerTests;
- Tanker tests;
- SuperZapperTests;
- BlasterLaneHighlightTests;
- LevelApproachTests;
- LevelProgressionTests;
- enemy-bullet tests;
- Spiker/Fuseball/Pulsar tests where applicable.

Use actual test names and valid filter syntax.

19. FULL AUTOMATED GATE

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

- no identity/accounting redesign;
- no C3/C4 progression changes;
- no Tanker/cargo gameplay change;
- no Super Zapper gameplay change;
- no approach changes;
- no lane-highlight redesign;
- no shot-size change;
- no final Blaster animation invention;
- no Spike retraction/removal;
- no palette cycling;
- no audio;
- no end-game animation;
- no high-score implementation;
- no Level 4;
- no Reference/incitti modification;
- existing TEMP/ remains excluded.

20. TRACKED DOCUMENTATION

Update narrowly as needed:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Before manual acceptance:

ARCADE VERIFIED observations may include:

- active enemies visibly retract backward/down the tube during player death;
- retracting enemies are inactive;
- Spikes remain on the web and do not retract/disappear;
- complete visible enemy retraction takes no more than approximately two seconds.

Be precise with the timing classification:

"No more than approximately two seconds" is a qualitative observed upper bound, not an exact measured duration.

REFERENCE VERIFIED:

- Incitti's shared depth/projection death/vortex presentation findings.

PROJECT DECISION:

- render-only offset architecture;
- exact deterministic duration/curve chosen within the observed bound;
- uncertain Egg/projectile/state-visibility details.

Explicitly state:

- final Blaster-location animation remains pending arcade-footage refinement;
- future timing refinement remains possible after footage/manual comparison.

Do not claim manual C# graphical acceptance until Neal provides it.

21. AI WORK RECORD

Save this exact prompt under:

AI-Logs/Prompts/2026-11-01-player-death-enemy-retraction-implementation-prompt.md

Create/update:

AI-Logs/Reports/2026-11-01-player-death-enemy-retraction-implementation-report.md

AI-Logs remains ignored.

Report:

- starting checkpoint;
- presentation architecture;
- Atari/reference distinction;
- enemy inactivity rule;
- Spike persistence rule;
- approximate two-second upper-bound handling;
- chosen presentation tuning;
- identity/accounting protections;
- tests/results;
- full validation;
- changed files;
- protected scope;
- manual acceptance pending;
- deferred Blaster animation/footage requirement;
- blockers.

Do not produce a reasoning transcript.

22. MANUAL ACCEPTANCE HANDOFF

If automated gates pass, STOP before commit.

Primary normal campaign run:

dotnet run --project src/Tempest.Game -- --lives 10

Manual checks:

LEVEL 1

- allow the Blaster to be destroyed while enemies are visibly active;
- enemies should visibly retract backward/down the tube;
- they should not simply vanish at death start;
- they should appear completely inactive while retracting;
- they should not fire, attack, change lanes through active behavior, or otherwise continue playing;
- lane positions should remain visually coherent while retreating;
- no enemy-kill score should appear because of retreat;
- complete retraction should be short and no longer than approximately two seconds;
- ordinary next-life reconstruction should look normal;
- exactly one life should be lost.

Try deaths with:

- enemy near rim;
- enemy deeper in tube;
- several enemies simultaneously if practical.

LEVEL 2

- square geometry retraction looks coherent;
- enemies move away rather than sideways/off geometry;
- retraction remains short;
- next life reconstructs normally.

LEVEL 3

- mixed wave retraction looks coherent;
- observe Flipper/Tanker if practical;
- cargo/re-entry remains normal;
- no duplicated or missing units apparent.

SPIKER/SPIKE

Using the existing Spiker development scenario if useful:

- active enemy units retract as appropriate;
- existing Spikes remain fixed on the web;
- Spikes do not retract;
- Spikes do not disappear because the Blaster died;
- normal established Spike persistence still works after the next life begins.

GAME OVER

If practical:

- final-life retraction remains coherent;
- Game Over appears normally;
- restart works.

REGRESSION

- lane highlighting remains normal where applicable;
- no projection flicker/explosion;
- no crash;
- no score anomaly;
- no altered spinner/control behavior.

Ask Neal specifically to assess:

- whether enemy retreat direction matches Atari;
- whether enemy inactivity looks correct;
- whether retreat distance/speed feels plausible;
- whether the entire retreat is comfortably within the remembered two-second maximum;
- whether enemies disappear too early or too late;
- whether Spikes remain exactly as expected;
- whether transition from death presentation to reconstructed next life looks natural.

Do NOT require approval of the final Blaster-location death animation because that remains explicitly deferred.

23. HARD DEFERRED ROADMAP

Do NOT implement:

- final arcade Blaster-location death animation;
- player-shot size refinement;
- elevated between-level approach perspective;
- broader entity rendering refinement;
- spinner refinement;
- later palettes;
- Level 4/later content;
- audio/sound;
- end-of-game animation;
- high-score tracking/persistence.

24. END-OF-RUN REPORT

At the end report only:

- files changed;
- death-presentation model;
- enemy render-retraction rule;
- Atari/reference distinction;
- enemy inactivity implementation;
- Spike persistence implementation;
- retraction timing policy and evidence classification;
- logical enemy freeze result;
- identity/accounting protection;
- Egg/projectile/hazard behavior;
- Level 3 Tanker/cargo result;
- lane-highlight interaction;
- ordinary-death result;
- Game Over result;
- tests added/changed;
- focused result;
- broader regression result;
- full solution result;
- build result;
- git diff --check;
- protected-scope result;
- Reference/incitti status;
- whether any existing gameplay defect was exposed;
- documentation status;
- deferred Blaster animation/footage note;
- exact manual command/checklist;
- blocker, if any.

Do not declare the enemy-retraction milestone complete until Neal performs manual graphical acceptance.

Do not commit.
Do not push.

TEMPEST-PLAYER-DEATH-ENEMY-RETRACTION-IMPLEMENTATION-COMPLETE-8C14F7