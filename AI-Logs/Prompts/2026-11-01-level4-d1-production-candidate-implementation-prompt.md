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

LEVEL 4 D1 ISOLATED PRODUCTION-CANDIDATE IMPLEMENTATION

CURRENT CLEAN CHECKPOINT

2318ea2 Finalize player-death enemy retraction

PRIMARY ANALYSIS

AI-Logs/Reports/2026-11-01-post-level3-progression-roadmap-analysis-report.md

CURRENT ACCEPTED PRODUCTION STATE

Natural campaign:

Level 1
-> approach
-> Level 2
-> approach
-> Level 3
-> terminal

Level 3 natural progression remains terminal throughout D1.

Do NOT add natural Level 3 -> Level 4 progression in this checkpoint.

PURPOSE

Implement one isolated Level 4 PRODUCTION-CANDIDATE diagnostic that exercises the source-derived Level 4 board, wave composition, Spikers, persistent Spikes, Tankers/cargo, appropriate bullet policy, death/reconstruction, completion, and spike-avoid exit behavior.

D1 is intended to prove that the current game architecture can support authentic Level 4 mechanics safely before natural progression is extended.

This is NOT:

- natural Level 4 progression;
- Level 5;
- open-tube work;
- Fuseball progression;
- Pulsar progression;
- palette-cycle implementation.

1. SOURCE-DERIVED LEVEL 4 IDENTITY

REFERENCE VERIFIED findings from the approved analysis:

Displayed Level:
4

Internal source level index:
3

Board:
3

Reference board description:
“binoculars”

Topology:

- closed;
- 16 playable lanes.

Reference position:

Center Y:
380

Y offset:
-65

Palette group:
0

Enemy supply:
17 original units

Initial active Eggs:
4

Reserve:
13

Source-eligible production enemy categories:

- Flippers;
- Tankers;
- Spikers.

Not yet production-eligible:

- Fuseballs;
- Pulsars.

Persistent starting Spikes:
16

Tankers:

- remain eligible;
- carry two Flippers.

Do not silently substitute development-wave composition for this Level 4 production candidate.

2. LEVEL 4 GEOMETRY

Add the source-derived Level 4 binoculars ordered point set from Reference/incitti.

Do not invent coordinates.

Do not derive geometry from screenshots.

Use the exact reference point ordering.

The approved analysis found that Level 4 remains representable using the existing point-based closed-board renderer and directed transition system.

Therefore prefer:

- a new LevelConfiguration;
- exact LevelDefinition/point data;
- existing generic topology;
- existing projection;
- existing endpoint-hinge/directed-transition machinery.

Do NOT introduce a Level-4-specific renderer.

Do NOT hard-code visual screen coordinates outside the existing level data/configuration model.

Do NOT implement open-tube behavior.

Level 8 is the later open-tube boundary.

3. GEOMETRY VALIDATION

Add deterministic automated checks for the Level 4 binoculars board.

At minimum verify:

- exact source-derived ordered points;
- closed topology;
- 16 playable lanes;
- finite tube rendering;
- correct near/far connections;
- player projection on all lanes;
- Egg projection;
- Flipper projection;
- Tanker projection;
- cargo projection;
- Spiker projection;
- Spike projection;
- no geometry mutation during rendering;
- Super Zapper rendering does not damage geometry;
- Blaster-lane highlighting selects the correct two rails.

Exercise every directed adjacent player/enemy transition around the board.

Verify:

- correct source/destination endpoint ownership;
- finite intermediate geometry;
- no tube crossing;
- destination completion;
- direction is preserved;
- no shortest-path substitution where directed geometry requires otherwise.

Do not assume this board has square or plus-board angles merely because existing boards do.

4. ISOLATED LEVEL 4 DIAGNOSTIC

Add one direct DEVELOPMENT ONLY command-line entry for the Level 4 production candidate.

Preferred name:

--level4-test

unless an existing naming convention strongly supports a better name.

The diagnostic should:

- load the exact production-candidate Level 4 configuration;
- use the intended production wave semantics;
- isolate Level 4 for repeated manual testing;
- stay on Level 4 after ordinary life loss;
- allow Game Over restart back into fresh Level 4 diagnostic;
- permit existing --lives override where appropriate.

Do not make --level4-test advance to Level 5.

Do not change normal no-flag campaign progression.

Maintain existing flag-conflict conventions.

5. PRODUCTION-WAVE GENERALIZATION

This is the major D1 architecture task.

The approved analysis concluded that the existing Level3WaveLedger cannot simply be reused blindly because Level 4 introduces normal production Spikers and persistent Spikes.

Inspect the existing Level3WaveLedger and related production-wave lifecycle carefully.

Generalize only as much as Level 4 genuinely requires.

Do NOT perform broad renames/refactors merely because Level3WaveLedger has a Level-3-specific name.

Acceptable outcomes include:

- a small shared production-wave abstraction;
- a narrowly extended ledger;
- another similarly bounded design.

Choose the smallest maintainable solution that can represent both:

Level 3:
Flippers + Tankers + Tanker cargo

Level 4:
Flippers + Tankers + Spikers + Tanker cargo + persistent Spikes

Preserve all accepted Level 3 lifecycle semantics.

6. ORIGINAL IDENTITY CONSERVATION

Each of the 17 source enemy obligations must remain accounted for exactly.

Preserve distinctions between:

- original Flipper;
- original Tanker;
- original Spiker;
- Tanker cargo Flipper;
- unresolved;
- destroyed;
- escaped where applicable;
- rim-cleared;
- returned-to-reserve where applicable;
- pending/terminal Super Zapper outcomes.

The following remain mandatory:

- no duplicate identity allocation;
- no lost unresolved identity;
- no resurrected destroyed identity;
- no score for presentation-only transitions;
- no silent identity creation through hatching.

Hatching remains conversion of an existing obligation.

7. LEVEL 4 COMPOSITION

Derive the production candidate's Flipper/Tanker/Spiker selection from the approved Incitti analysis/reference behavior.

Do not fabricate a fixed artificial composition simply because deterministic tests are easier.

If deterministic randomness seams are necessary for tests, keep them DEVELOPMENT ONLY and prove they do not redefine production composition.

Document precisely which Level 4 composition properties are:

REFERENCE VERIFIED

versus

PROJECT DECISION.

8. SPIKER IDENTITY BEHAVIOR

Level 4 introduces Spikers into production progression.

Integrate the existing Spiker enemy behavior without copying the artificial --spiker-test wave.

Protect the reference-backed distinction:

- Spiker is an active enemy obligation;
- the Spike it builds is a separate persistent web hazard;
- a normal Spiker retreat can return the unresolved enemy obligation according to the source-backed lifecycle;
- destroying the Spiker follows the established kill/score path;
- persistent Spike is NOT itself an enemy identity.

Do not add Spike to the 17-unit enemy supply.

9. PERSISTENT LEVEL 4 SPIKES

REFERENCE VERIFIED Level 4 source data indicates 16 starting Spikes.

Represent them as persistent web hazards independently of enemy accounting.

Requirements:

- one source-defined initial Spike state per appropriate lane;
- Spike geometry is topology-driven;
- Spike is not an enemy;
- Spike does not consume enemy supply;
- Spike does not award ordinary enemy-kill score;
- Spike remains distinct from the Spiker that may extend it;
- Spike survives ordinary player death according to the established persistence rules;
- Spike remains fixed during the accepted player-death enemy-retraction presentation.

Do not regress the development Spiker scenario.

10. SPIKER / SPIKE DEATH INTERACTION

Protect the accepted death-presentation distinction:

ACTIVE SPIKER:
may visually retract with active enemies during PlayerDying

PERSISTENT SPIKE:
remains fixed on the web

The new Level 4 diagnostic must use the accepted presentation-only retraction architecture.

Do not create a second death system.

11. TANKERS AND CARGO

Reuse the accepted Level 3 Tanker/cargo lifecycle.

Protect:

- original Tanker identity;
- exact cargo pair ownership;
- cargo ordinal;
- latent cargo;
- materialized cargo;
- pending activation;
- ordinary Tanker shot destruction;
- natural carrier release;
- Super Zapper behavior;
- death reconstruction;
- terminal/resolved outcome distinctions.

If Level 4 requires parameter differences, make them explicit configuration/source decisions.

Do not redesign cargo merely because Level 4 introduces another original enemy type.

12. TANKER FIRING

The approved analysis found no new Level-4-specific Tanker firing blocker.

Preserve the accepted production Tanker firing path.

Do not retune Tanker firing in D1 unless exact Level 4 source configuration requires a parameter difference already supported by evidence.

The previously observed broader Atari timing/fidelity question remains separately deferred.

13. ENEMY BULLETS

The approved analysis found an important production-policy issue:

- Incitti ordinary Flippers are source-eligible for bullets from earlier levels;
- current normal C# Flippers remain silent by default;
- source Spikers can also fire under their defined retreat/firing path;
- C# enemy bullet behavior has historically been DEVELOPMENT ONLY.

Do NOT solve this by arbitrarily enabling bullets only on Level 4.

Inspect the current bullet implementation and the source.

Choose an explicit, evidence-classified D1 policy.

Preferred outcome:

- support the source-appropriate Level 4 production bullet behavior needed by the production candidate;
- avoid retroactively altering accepted Levels 1–3 unless necessary;
- document any temporary discrepancy as PROJECT DECISION rather than silently claiming arcade fidelity.

If faithfully adding normal bullets across Levels 1–4 is too broad for this checkpoint, keep the discrepancy narrowly documented and implement only the minimum Level 4 production behavior justified by the reference.

Do not let bullet cleanup alter enemy identity supply.

14. HURRY-UP / RIMIT

Do NOT add Spiker-specific rimit behavior.

The approved analysis corrected this point:

Incitti's rimit affects later Fuseball/Pulsar behavior, not Level 4 Spikers.

Level 4 contains neither Fuseballs nor Pulsars.

Therefore:

- do not promote development Fuseball/Pulsar Hurry-Up behavior into Level 4;
- do not invent Spiker rimit effects;
- preserve current Level 4 completion semantics independently;
- leave production rimit integration for the later pre-Level-11 milestone.

15. SUPER ZAPPER

Level 4 must use existing production Super Zapper lifecycle unless exact reference configuration establishes a supported difference.

Protect:

- fresh Level start charge policy;
- delayed target behavior;
- Tanker/cargo obligations;
- original identity accounting;
- Spiker resolution;
- score;
- death/re-entry;
- completion blocking.

Add tests proving Spiker targets do not corrupt the production ledger.

Do not simplify Super Zapper selection merely to support a third original enemy category.

16. COMPLETION

Reuse the accepted completion framework while extending qualifying Level 4 obligations correctly.

Protect distinctions between:

- destroyed;
- escaped/returned;
- rim-cleared;
- unresolved;
- Tanker cargo;
- pending Super Zapper obligations.

Persistent Spikes:

- are not enemy obligations;
- do not count toward the 17 enemy supply;
- do not by themselves prevent ordinary enemy-wave resolution unless the established spike-avoid exit phase requires them as hazards.

Do not require every Spike to be destroyed to complete Level 4.

17. LEVEL 4 SPIKE-AVOID EXIT

The approved analysis identifies Level 4–7 as using source spike-avoid exit behavior.

Integrate the smallest production-capable version needed by Level 4.

Reuse existing development Spiker exit/reverse/retry behavior where it already matches the reference architecture.

Do not blindly promote development-only assumptions.

Protect:

- Blaster exit down the tube;
- collision with persistent Spike;
- reverse/retry where source-backed;
- lives/score;
- enemy-wave accounting;
- Spike persistence;
- no duplicate wave regeneration caused by an exit retry.

Level 4 diagnostic remains terminal after successful Level 4 completion.

Do not construct Level 5.

18. PLAYER DEATH / ENEMY RETRACTION

Reuse checkpoint 2318ea2 exactly.

Level 4 active enemies should follow the accepted presentation policy:

- active Flipper retracts visually;
- active Tanker retracts visually;
- active Spiker retracts visually;
- gameplay entities are logically frozen;
- persistent Spikes remain fixed;
- no retreat score;
- one life consumed at established boundary;
- unresolved obligations reconstruct correctly.

Do not retune presentation timing.

Do not refine the existing Blaster death animation in D1.

19. BLASTER-LANE HIGHLIGHTING

Level 4 remains in palette group 0 according to the approved reference analysis.

Reuse the currently accepted yellow lane highlighting.

Requirements:

- exactly two bounding rails;
- full tube depth;
- follows Player.EdgeIndex;
- correct on irregular binoculars geometry;
- Pulsar precedence remains irrelevant because Pulsars are not Level 4 eligible;
- Super Zapper precedence remains as accepted.

Do not implement later palette cycling.

20. LEVEL 4 APPROACH IS OUT OF D1 NATURAL PROGRESSION

The generic approach architecture appears ready for Level 4, but D1 does NOT wire natural Level 3 -> 4.

Do not modify normal Level 3 terminal presentation except where test seams require strictly non-production access.

Do not retune:

- perspective;
- approach angle;
- duration;
- depth;
- smoothstep;
- movement.

Natural Level 3 -> Level 4 approach belongs to D2 after D1 manual acceptance.

21. LEVEL 3 REGRESSION BOUNDARY

Level 3 is already accepted.

D1 must preserve:

- geometry;
- Flipper/Tanker composition;
- ledger semantics;
- cargo;
- Tanker firing;
- Zapper;
- death/reconstruction;
- enemy retraction;
- all-rim completion;
- approach into Level 3;
- terminal Level 3 behavior.

If generalizing production-wave code requires touching Level 3 internals, add direct equivalence/regression tests.

Do not change accepted observable Level 3 behavior.

22. AUTOMATED TESTS — LEVEL 4 CONFIGURATION

At minimum prove:

- displayed identity 4;
- source internal index relationship documented;
- exact binoculars points;
- closed 16-lane topology;
- source center/offset;
- palette group 0;
- supply 17;
- four initial Eggs;
- 13 reserve obligations;
- Flipper/Tanker/Spiker eligibility;
- Fuseball/Pulsar exclusion;
- 16 starting persistent Spikes.

23. AUTOMATED TESTS — GEOMETRY/RENDERING

Prove:

- every lane renders finite;
- every directed transition is finite;
- source/destination completion correct;
- player movement works around full board;
- Blaster lane highlighting correct for every lane;
- Eggs finite;
- shots finite;
- Flippers finite;
- Tankers/cargo finite;
- Spikers finite;
- Spikes finite;
- Super Zapper rendering finite;
- death-retraction rendering finite;
- renderer does not mutate geometry.

24. AUTOMATED TESTS — WAVE / IDENTITY

Exercise:

- original Flipper lifecycle;
- original Tanker lifecycle;
- original Spiker lifecycle;
- Tanker latent cargo;
- Tanker active cargo;
- Spiker retreat/return path;
- Spiker destruction;
- multiple mixed identities;
- ordinary death;
- consecutive deaths;
- destroyed identities remain terminal;
- unresolved identities reconstructed exactly once;
- no duplicate allocation;
- exact TotalIdentityCount conservation or equivalent generalized invariant.

25. AUTOMATED TESTS — SPIKES

Prove:

- 16 starting Spikes;
- Spike independent of original enemy identity;
- Spiker can interact with its Spike without creating/removing enemy identity incorrectly;
- Spike persists across ordinary death;
- Spike remains stationary during enemy-retraction presentation;
- Spike can participate in source-backed spike-avoid exit behavior;
- Spike is not required to be destroyed as part of enemy supply completion.

26. AUTOMATED TESTS — BULLETS

Protect whatever explicit D1 production bullet policy is chosen.

At minimum:

- no duplicate bullets from death freeze;
- no firing during PlayerDying;
- cleanup/reconstruction correct;
- bullet does not count as enemy identity;
- bullet cannot incorrectly block completion;
- Spiker/Flipper source boundary is explicit;
- Levels 1–3 remain unchanged unless a deliberately justified cross-level policy is implemented.

27. AUTOMATED TESTS — SUPER ZAPPER

Cover:

- Spiker targeting/resolution;
- Flipper;
- Tanker;
- Tanker cargo;
- pending target across relevant lifecycle;
- no identity loss/duplication;
- no stale score;
- proper completion blocking.

28. AUTOMATED TESTS — EXIT/REPLAY

Cover:

- Level 4 diagnostic completion;
- spike-avoid exit;
- retry/reverse where applicable;
- successful terminal diagnostic result;
- R replay returns to fresh Level 4 diagnostic;
- Game Over restart returns appropriately;
- no Level 5;
- normal no-flag campaign still ends at Level 3.

29. DEVELOPMENT FLAG

Add focused tests proving:

--level4-test

is DEVELOPMENT ONLY and composes correctly with:

--lives

and any intentionally supported diagnostic option.

Preserve existing mutually-exclusive scenario policy.

Do not accidentally allow ambiguous combinations that create artificial production claims.

30. BROADER REGRESSION GATE

After new Level 4 focused tests pass, run a broad group covering at minimum:

- LevelConfigurationTests;
- LevelProgressionTests;
- LevelApproachTests;
- Level2SquareTests;
- Level3GeometryTests;
- Level3TestModeTests;
- Level3MixedWaveTests;
- Level3WaveLedgerTests;
- new Level4 tests;
- SimulationTests;
- Tanker firing/cargo tests;
- Spiker tests;
- enemy bullet tests;
- SuperZapperTests;
- BlasterLaneHighlightTests;
- PlayerDeathPresentationTests;
- Fuseball/Pulsar regression tests where existing shared systems are touched.

Use actual test names and valid filters.

31. FULL AUTOMATED GATE

Before requesting manual acceptance, run:

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

- no natural Level 4 progression;
- no Level 5;
- no open tubes;
- no Fuseball/Pulsar production activation;
- no palette cycling;
- no approach retuning;
- no death-presentation retuning;
- no Blaster animation refinement;
- no shot-size work;
- no spinner changes;
- no audio;
- no end-of-game animation;
- no high-score implementation;
- no Reference/incitti modification;
- existing TEMP/ remains excluded.

32. TRACKED DOCUMENTATION

Update narrowly as necessary:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Before Neal's manual validation:

- record Level 4 source findings as REFERENCE VERIFIED;
- record --level4-test as DEVELOPMENT ONLY;
- record internal architecture/generalization as PROJECT DECISION;
- do NOT label C# Level 4 geometry/gameplay ARCADE VERIFIED merely because tests pass;
- do NOT claim authentic Level 4 wave composition has been manually arcade-validated;
- clearly state natural Level 3 -> 4 progression remains pending D2.

33. AI WORK RECORD

Save this exact prompt under:

AI-Logs/Prompts/2026-11-01-level4-d1-production-candidate-implementation-prompt.md

Create/update:

AI-Logs/Reports/2026-11-01-level4-d1-production-candidate-implementation-report.md

AI-Logs remains ignored.

Report:

- starting checkpoint;
- Level 4 configuration;
- geometry implementation;
- production-wave generalization;
- identity model;
- Spiker/Spike integration;
- Tanker/cargo result;
- bullet policy;
- rimit boundary;
- Super Zapper;
- death/retraction;
- lane highlighting;
- spike-avoid exit;
- tests/results;
- full validation;
- files changed;
- protected scope;
- manual acceptance pending.

Do not create a reasoning transcript.

34. MANUAL ACCEPTANCE HANDOFF

If all automated gates pass, STOP before commit.

Provide Neal a concise manual command/checklist.

Expected diagnostic command:

dotnet run --project src/Tempest.Game -- --level4-test --lives 10

Manual checks should include:

BOARD

- binoculars board visually correct and sensibly proportioned;
- all lanes connect correctly;
- no crossed tube lines;
- Blaster movement around entire rim feels correct;
- lane highlighting follows correctly.

FLIPPERS

- below-rim movement and pivots look geometrically correct;
- no outward/wrong-web pivots;
- rim behavior remains normal.

TANKERS

- Tankers climb normally;
- cargo release remains coherent;
- cargo uses correct geometry;
- no obvious duplicate/missing cargo;
- firing behavior follows chosen D1 policy.

SPIKERS / SPIKES

- Spikers appear as normal production enemies rather than artificial test-wave behavior;
- persistent Spikes appear correctly;
- Spiker/Spike relationship looks sensible;
- Spike persists when expected.

DEATH

- enemies retract normally;
- Spiker retracts as an active enemy;
- persistent Spikes remain fixed;
- next-life wave reconstruction looks coherent;
- exactly one life lost.

SUPER ZAPPER

- behavior remains sensible with mixed Flipper/Tanker/Spiker population.

EXIT

- wave completion leads to source-backed spike-avoid exit;
- Spike collision/retry behavior is coherent if encountered;
- successful Level 4 diagnostic remains terminal;
- no Level 5.

REGRESSION

- no crash;
- no projection explosion;
- no lane-highlight regression;
- no score/lives anomaly.

Do not ask Neal to validate natural Level 3 -> 4 progression in D1.

That is D2.

35. DEFERRED ROADMAP

Do NOT implement:

- D2 natural Level 3 -> 4 progression;
- Levels 5–7;
- Level 8 open topology;
- Level 11 Fuseball production;
- production rimit;
- Level 17 Pulsar/palette boundary;
- later cargo types;
- existing Blaster death-animation refinement;
- player-shot size refinement;
- elevated approach perspective;
- broader rendering fidelity;
- spinner refinement;
- audio/sound;
- end-of-game animation;
- high-score tracking/persistence.

36. END-OF-RUN REPORT

At the end report only:

- files changed;
- Level 4 configuration;
- geometry status;
- production-wave generalization;
- identity/accounting result;
- Spiker result;
- Spike result;
- Tanker/cargo result;
- bullet policy;
- rimit boundary;
- Super Zapper result;
- death/retraction result;
- lane-highlight result;
- spike-avoid exit result;
- tests added/changed;
- focused result;
- broader regression result;
- full solution result;
- build result;
- git diff --check result;
- protected-scope result;
- Reference/incitti status;
- existing defect found, if any;
- documentation status;
- exact manual command/checklist;
- blocker, if any.

Do not declare D1 complete until Neal performs manual graphical/gameplay acceptance.

Do not commit.
Do not push.

TEMPEST-LEVEL4-D1-PRODUCTION-CANDIDATE-IMPLEMENTATION-COMPLETE-91A6E4