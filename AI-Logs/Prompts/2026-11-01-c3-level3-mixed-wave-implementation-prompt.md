C3a — Complete headless Level 3 mixed-wave model

IMPLEMENTATION AND AUTOMATED VALIDATION

Starting clean checkpoint:
6d15165 Correct and validate isolated Tanker firing timing

Primary authority:
AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-report.md

Supporting authority:
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md
- AI-Logs/Reports/2026-10-31-level3-c1-geometry-implementation-report.md
- AI-Logs/Reports/2026-10-31-c2-tanker-firing-policy-analysis-report.md
- AI-Logs/Reports/2026-10-31-c2-tanker-firing-correction-implementation-report.md
- current C# source and tests
- literal Reference/incitti source

Purpose

Implement C3a only:

- complete headless production-shaped Level 3 configuration and gameplay model;
- Level-3-only source-shaped Egg scheduling;
- randomized Flipper/Tanker selection;
- stable original identities;
- Tanker-owned latent cargo identities;
- materialized cargo identities and lifecycle;
- zero, one and multiple Tankers;
- six simultaneously active Tanker cap;
- validated production Tanker firing;
- natural, shot and Super Zapper carrier outcomes;
- multi-carrier Super Zapper handling;
- death/re-entry identity conservation;
- all-rim and normal completion accounting;
- terminal Level 3 model behavior through a configuration-based headless entry.

Do not implement C3b.

Specifically, do not add:
- --level3-test;
- Program command-line routing;
- new window titles;
- production Level 3 HUD;
- production bullet rendering policy;
- manual graphical access.

Do not implement C4.

Specifically, do not add:
- natural Level 2 -> Level 3 progression;
- a LevelProgression resolver edge;
- changes to the Level 2 successful-exit commit condition;
- Level 4;
- campaign-cycle architecture.

Evidence hierarchy

Preserve:

1. Neal's directly observed Atari Tempest behavior.
2. Literal Mark Incitti source as the primary engineering reference.
3. Current tested C# behavior.
4. Explicit project decisions only where necessary.

Never upgrade Incitti-only values or hidden implementation details to ARCADE VERIFIED.

Accepted behavior that must not regress

- C1/C1.5 geometry and directed rotation are manually accepted.
- Full 90/180/270 directed sweeps determine transition duration.
- Enemy visible pivots swing inward through the web.
- Tanker cargo follows the accepted outward lane route while its visible pivot swings inward.
- Cargo does not dynamically track the Blaster below the rim.
- Existing isolated Tanker cargo identity, scoring and delayed-activation behavior remains correct.
- C2 Tanker firing is manually accepted.
- Tanker climb increment is 1.75.
- Tankers can naturally produce two separated shots before release, but two shots are not guaranteed.
- Tankers continue climbing while firing.
- There is no release-point firing burst.
- Existing Tanker bullets survive carrier release/destruction.
- Exact Atari ammunition, delay, probability, interval and per-carrier shot count remain unknown.
- Default Level 1 and Level 2 Flippers remain silent.
- Flipper bullets remain opt-in.
- Existing Level 1, Level 2 and every established development scenario must remain unchanged.

1. Baseline verification

Before editing, run:

git status --short
git log -3 --oneline
git diff --check
git status --short -- Reference/incitti
git ls-files --others --exclude-standard

Required baseline:
- HEAD is 6d15165.
- Worktree is clean.
- Reference/incitti is unchanged.
- No unexpected untracked files exist.

Save this exact prompt under AI-Logs/Prompts using the established naming convention.

Create a detailed C3a implementation report under AI-Logs/Reports.

AI-Logs must remain ignored.

Do not commit.
Do not push.

2. Reconfirm the analysis against current source

Read the complete C3 pre-implementation report and inspect the relevant current source before editing.

Reconfirm these assumptions:

- existing Level3Plus is the Flipper-only geometry diagnostic;
- production Level 3 needs a distinct mixed configuration;
- board point data must be shared rather than copied;
- normal Level 1/2 accounting has no stable original identities;
- isolated Tanker accounting contains fixed-slot-0 assumptions;
- production behavior must be selected by installed composition/configuration, not tankerTest or other CLI flags;
- production Tankers must be armed independently of --enemy-bullets-test;
- ordinary and cargo Flippers remain silent unless the existing optional bullet flag enables them;
- SuperZapperSystem already supports arbitrary target sequences and should not need a global redesign;
- LevelProgression remains untouched.

If current source contradicts a central assumption, stop and report the contradiction instead of broadening the task.

3. Production Level 3 configuration

Add one immutable production-shaped Level 3 mixed configuration.

Use a clear name consistent with current conventions, such as Level3Mixed.

It must share the exact existing Level 3 plus-board definition. Do not copy the board coordinates.

Effective configuration:

- internal identity: 2;
- displayed identity: 3;
- board: 2;
- palette: 0;
- closed topology;
- 16 vertices;
- 16 playable lanes;
- CenterY: 400;
- YOffset: -80;
- depth: 400;
- playing position: 50;
- player lane: 7;
- player fractional position: 0.5;
- 15 original units;
- four initial entrants;
- 11 reserve originals;
- eligible original enemies: Flipper and Tanker only;
- Tanker cargo: two Flippers only;
- Tanker climb: 1.75;
- Tanker firing: accepted C2 policy;
- Flipper canflip: true;
- Flipper climb: 2;
- turn increment: 8;
- below-rim pause: 21;
- rim pause: 24;
- existing endpoint/inward geometry and full directed sweep duration;
- two fresh Super Zapper charges;
- no Spikes;
- existing standard exit;
- terminal Level 3 model behavior when exercised directly through the configuration-based headless entry.

Classify values correctly in comments/report:
- source values are REFERENCE VERIFIED;
- C# lifecycle/entry choices are PROJECT DECISIONS;
- direct headless access is DEVELOPMENT ONLY;
- accepted visible geometry/cargo/firing observations retain their existing evidence labels.

Do not modify Level3Plus diagnostic behavior.

4. Level-3-only production scheduling

Implement the analysis-approved Level-3-only source-shaped scheduling policy.

Preserve every existing Level 1, Level 2 and development-scenario scheduling rule.

Production mixed Level 3 must use:

- four initial original Eggs;
- each fresh Egg receives a random inclusive lane 0..15;
- direction comes from the source-shaped 0..1 choice mapped to the existing signed direction representation;
- every 50 active Playing updates, release up to six queued entrants in queue order;
- release is limited by available unresolved queued obligations;
- do not apply the existing four-active-Egg cap to production mixed Level 3;
- preserve the existing eight-update Egg movement cadence;
- approach requires fewer than nine active enemy units;
- approach requires fewer than two approaching/hatching Eggs;
- at internal level 2, hatchrate is zero;
- approach is selected by inclusive Rand(0,100) > 95 or the near-end condition where active enemy units plus active Eggs is less than four;
- preserve the source-shaped roaming height boundary and direction-reversal behavior for this Level-3-only path.

For the active-enemy gate count:
- include active original Flippers;
- include active Tankers;
- include active cargo Flippers;
- include pending-activation cargo obligations where the approved analysis requires them to count as active;
- exclude bullets;
- exclude reserve originals;
- exclude latent children;
- exclude terminal identities.

Preserve current C# pipeline conventions:
- ready Eggs hatch in the normal hatch pass;
- newly hatched originals update later in the same game step;
- newly released cargo first updates on the next game step;
- death freezes the production scheduling clock;
- reconstruction resets the local schedule phase;
- reconstruction initially releases up to four surviving obligations;
- materialized cargo re-entry is prioritized;
- later six-at-50 releases drain the remaining reconstructed queue.

Treat active-play clock semantics, hatch ordering, reconstruction ordering and death freeze as PROJECT DECISIONS.

Do not claim global seeded equivalence to Incitti.

5. Mixed Flipper/Tanker selection

Implement the literal Level 3 source-shaped selection behavior through the existing injected randomness seam.

For a ready, previously unselected original:

- upper is 4 when active Tanker count is below six;
- upper is 3 when active Tanker count equals six;
- draw inclusive Rand(-6, upper);
- clamp a negative candidate to zero;
- if active enemy units plus active Eggs is less than four, perform the source near-end overwrite draw inclusive Rand(1, upper);
- retain the first discarded draw in random-consumption order;
- reject Spinner candidate 0;
- accept Flipper candidate 1;
- reject Pulsar candidate 2;
- reject Fuseball candidate 3;
- accept Tanker candidate 4;
- retry rejected candidates using the literal selection flow.

At Level 3:
- only Flipper or Tanker may be committed;
- Tanker cargo selection must retry inclusive Rand(0,2) until candidate 0, because Fuseball/Pulsar cargo is disabled;
- commit two-Flipper cargo.

The six-Tanker cap is simultaneous, not per-wave:
- count actual live Tanker objects, including pending-zap carriers;
- when one leaves through natural release, shot destruction or zap resolution, later Tanker selection again becomes possible;
- more than six sequential Tankers in one wave must remain possible;
- no more than six may be active simultaneously.

Do not hard-code original slot 0 as a Tanker.

Do not force a production Tanker count or ratio.

Do not add a production seed or manual forced-composition flag.

Deterministic tests may script selection outcomes through strict finite random seams.

Selected-type retention:
- commit an original's selected type on first successful hatch;
- retain that type across ordinary death/reconstruction;
- never reroll a selected Tanker into a Flipper;
- never discard that Tanker's latent children;
- if a reconstructed selected Tanker is ready while six Tankers are active, leave that Egg ready and skip it;
- process other hatchable Eggs where possible;
- do not spin indefinitely on the blocked Egg;
- do not consume additional selection randomness for an already-selected blocked Tanker;
- once capacity is available, hatch the retained Tanker.

Unselected originals may still select Flipper while the Tanker cap is full.

6. Add a narrow Level-3-only wave ledger

Add one small ledger model in the existing Levels area.

Do not create a general ECS.
Do not migrate Level 1 or Level 2.
Do not replace the isolated Tanker diagnostic's externally established accounting behavior.

The production mixed-wave ledger must own:

Original identities:
- exact namespace 0..14;
- selected type: unselected, Flipper or Tanker;
- state/location;
- terminal outcome.

Cargo identities:
- parent original ID;
- ordinal 0 or 1;
- distinct namespace from originals;
- state/location;
- terminal outcome.

Recommended cargo integer adapter if needed:
CargoUnitId = 2 * ownerOriginalId + ordinal

But keep namespace/type information explicit:
- Original(7) and Cargo(7) are not interchangeable;
- an Egg or Flipper holds exactly one identity namespace;
- a Tanker holds one original ID;
- cargo records retain owner and ordinal.

Create a Tanker's two latent cargo records exactly once when that original first commits to Tanker.

Never recreate them on:
- hatch;
- death;
- re-hatch;
- natural release;
- shot destruction;
- zap expiry.

Original legal states:

- reserve;
- Egg;
- active Flipper;
- active Tanker;
- destroyed;
- escaped;
- rim-cleared.

Cargo legal states:

- latent under unresolved selected Tanker;
- pending shot activation;
- re-entry reserve;
- Egg;
- active child;
- destroyed;
- rim-cleared.

Use enums/types consistent with project style.

Reject impossible transitions explicitly.

Do not hide accounting failures with:
- clamps;
- silent returns;
- reconstructed replacement identities;
- count-only repairs;
- success-shaped completion.

7. Required invariants and validation helpers

At stable game/lifecycle boundaries, validate:

Originals:
- the disjoint union of all original states equals exactly IDs 0..14;
- every unresolved original has exactly one runtime/queue representation;
- selected type agrees with the runtime object;
- terminal originals have no runtime/queue representation.

Tanker ownership:
- each committed Tanker original owns exactly two cargo records;
- unselected or Flipper originals own none;
- each latent child belongs to one selected unresolved Tanker;
- a Tanker carrier represented in reserve/Egg form may still own latent children;
- a terminal carrier has no latent children.

Cargo:
- every unresolved cargo identity has exactly one representation:
  - latent;
  - pending activation;
  - re-entry reserve;
  - Egg;
  - active child;
- terminal cargo has no runtime representation;
- no child pair is allocated twice;
- no child identity can belong to two owners.

Cap:
- no more than six active Tankers.

Scoring:
- carrier destroyed transition scores 200 once;
- original Flipper/child destroyed transition scores 150 once;
- natural escape scores zero;
- rim-clear scores zero;
- an already-terminal identity cannot score again.

Runtime ordering:
- runtime enemy lists and insertion-ordered shot/Zapper targets agree;
- shot-released children remain only in the activation queue until the established activation point;
- queued children cannot also appear as active or Egg identities.

Total ledger identity count:
- 15 originals plus two cargo identities for each original ever committed as Tanker;
- the count grows only on first Tanker commitment;
- death/release does not change it.

Use exact namespace-tagged set equality and duplicate detection.

Include deliberate invalid-state tests:
- equal counts with one missing and one extraneous original;
- duplicate runtime original representation;
- duplicate cargo representation;
- wrong owner;
- terminal identity still active;
- latent and materialized representation of the same child.

8. Production Tanker firing integration

Arm production mixed-wave Tankers from installed production composition, not from --enemy-bullets-test.

Production Tanker initialization:
- inclusive ammo 1..2;
- timer 16 + inclusive 1..64;
- accepted 1.75 climb;
- accepted C2 firing algorithm.

An armed production Tanker must receive the inclusive random delegate even if the optional enemy-bullet flag is false.

Do not arm default ordinary or cargo Flippers in production Level 3.

Existing modes remain:

- --tanker-test alone: silent Tanker;
- --tanker-test --enemy-bullets-test: armed Tanker and opt-in Flipper bullets;
- normal Levels 1 and 2: silent Flippers;
- optional bullet scenarios retain their established behavior.

Do not modify:
- ammunition;
- timer;
- probability;
- ceiling;
- release threshold;
- bullet speed;
- bullet collision;
- carrier collision;
- C2 test semantics.

9. Natural Tanker release

Production natural release must:

- transition carrier original from active Tanker to escaped;
- award zero carrier points;
- transition that carrier's existing two children from latent to active live cargo;
- preserve the child IDs and owner/ordinal;
- apply the accepted distinct non-carrier lanes;
- apply the accepted outward lane route;
- use inward visible pivot geometry;
- preserve fixed below-rim direction;
- add no child zap timers;
- remove the carrier runtime object and pending timer;
- retain independent existing carrier bullets.

The two children first update on the next established game step.

No second child pair may be allocated.

10. Player-shot Tanker destruction

Shot destruction must:

- transition the carrier to destroyed exactly once;
- award 200 exactly once;
- consume the killing shot;
- transition the existing latent pair to pending shot activation;
- create two live children with the correct existing identities;
- activate them at the current established activation point;
- prevent the killing shot from hitting those children;
- preserve interaction with other already-traveling shots according to the currently accepted ordering;
- cancel any pending carrier zap with no later zap carrier score;
- preserve independent existing bullets.

The activation queue is a real cargo representation and a completion blocker.

11. Super Zapper with multiple carriers

Do not redesign SuperZapperSystem globally.

Wire production mixed enemies into the existing insertion-ordered target sequence.

Preserve:
- first charge accepted-target delays 4,6,8,...;
- rejected bullets do not consume delay slots;
- second charge's current target-selection semantics;
- timer replacement behavior;
- player-shot processing before enemy timer advancement;
- pending non-expiring Tankers continue normal climb/firing;
- expiry steps skip normal carrier movement/firing.

On carrier zap expiry:

- transition that specific carrier original to destroyed;
- award 200 exactly once;
- transition only its owned latent pair to stillborn active children;
- assign each child its established timer 8;
- do not allocate a new pair;
- do not affect another carrier's cargo;
- children begin countdown at the accepted next-update boundary;
- child expiry scores 150 exactly once unless preempted;
- shot destruction/preemption of a carrier prevents later carrier zap score;
- natural release before zap expiry produces live cargo and no carrier score;
- zap expiry that wins the ordering produces stillborn cargo.

Multiple carriers must support:
- independent staggered timers;
- unique child pairs;
- overlapping natural, shot and zap outcomes;
- interleaved target lists with Flippers and rejected bullets;
- no stale timer resolving a removed identity.

An all-target Zapper fixture with n unpreempted Tankers may assert 500*n carrier/child subtotal. Do not treat that as a natural-game guarantee.

12. Death and reconstruction transaction

Before discarding runtime state, validate complete representation.

Snapshot:

- unresolved original IDs;
- selected original types;
- terminal original sets;
- latent cargo owner/ordinal sets;
- unresolved materialized cargo IDs;
- terminal cargo sets;
- reserve queues;
- Eggs;
- active originals;
- active children;
- pending shot-activation children;
- relevant scheduling state.

Then preserve established project policy:

- discard runtime shots;
- discard bullets;
- discard enemies;
- discard pending activation runtime objects;
- discard enemy zap timers;
- discard firing ammo/timer;
- retain score;
- retain Super Zapper charges;
- retain established flash policy;
- retain the same Level/configuration;
- retain terminal outcomes;
- retain selected original types;
- retain child ownership/identity.

Rebuild exactly once:

- every unresolved original;
- every unresolved materialized child;
- no terminal identity;
- no latent child as an independent Egg.

Prioritize materialized cargo for the first up-to-four reconstructed Eggs.

Order remaining re-entry deterministically by the selected project policy.

Fresh Egg lane/direction choices still use production random choices.

Before-release death:
- same selected Tanker returns;
- same two latent children remain owned;
- no cargo Eggs;
- carrier gets new firing ammo/timer when rehatched.

After-release death:
- terminal carrier remains terminal;
- only unresolved materialized children return.

Partial child resolution:
- terminal child stays terminal;
- only unresolved sibling returns.

Stillborn child:
- unresolved child returns as a fresh unzapped cargo Flipper Egg;
- old zap timer does not return.

Repeated deaths:
- identical unresolved identity sets return;
- no new cargo allocation;
- no shrink/growth from reconstruction.

After rebuilding, validate exact equality of:
- unresolved-original set;
- selected-type mapping;
- latent owner/ordinal set;
- unresolved materialized-child set;
- all terminal sets.

Do not evaluate playing conservation after intentional Game Over teardown.

13. Normal completion

Production Level 3 ordinary completion requires:

- every original terminal;
- every committed cargo identity terminal;
- no original reserve;
- no cargo re-entry reserve;
- no Eggs;
- no active Flippers;
- no active Tankers;
- no active cargo;
- no pending shot activation;
- no latent cargo;
- no unrepresented unresolved identity.

Bullets are hazards, not wave obligations.

Existing exit cleanup remains responsible for residual bullets.

The headless production configuration must terminate at Level 3 after successful exit travel.

Do not advance to another level.

Do not add natural campaign access.

14. All-rim completion

Production all-rim requires:

- Playing state;
- no original reserve;
- no cargo re-entry reserve;
- no Eggs;
- no latent cargo;
- no pending activation cargo;
- all remaining active qualifying enemies have post-update height >= 400;
- namespace-tagged represented unresolved originals exactly equal unresolved-original ledger set;
- namespace-tagged represented unresolved cargo exactly equal unresolved-cargo ledger set;
- no duplicate representations;
- no missing or extraneous identity.

A normal unreleased Tanker with latent cargo blocks all-rim completion.

Do not erase latent children through carrier rim clearing.

For valid qualifying enemies:
- record rim-cleared terminal outcome;
- award zero kill score;
- remove runtime/order targets;
- cancel pending zap score by removing the target;
- do not later resolve stale timers;
- begin existing standard exit.

Maintain:
- player collision before all-rim;
- no score for natural escape;
- no score for rim-clear;
- destroyed, escaped and rim-cleared as distinct outcomes.

15. Configuration-based headless production entry

Add the narrowest internal/public construction path consistent with current repository conventions so tests can run the real production mixed model.

It must:

- install the production Level3Mixed configuration;
- use the real production scheduling policy;
- use the real randomized selection path;
- use the real production ledger;
- use the real TempestGame orchestration;
- use the real Tanker firing and Super Zapper paths;
- preserve terminal Level 3 behavior;
- preserve replay/new-game lifecycle in headless tests where currently supported;
- remain unavailable through Program CLI in C3a.

Do not create:
- a separate test simulation;
- a fixed Tanker production variant;
- a test-only composition path;
- a duplicate Level 3 board;
- `--level3-test`.

Tests may inject strict random scripts through established seams.

16. Required deterministic C3a tests

Add focused test classes with names that clearly identify production mixed Level 3 and its ledger.

Configuration:
- exact identity/display/board/palette;
- shared board-definition object or clearly proven no coordinate duplication;
- topology and points;
- supply 15;
- initial four and reserve 11;
- tuning;
- two charges;
- no Spikes;
- diagnostic Level3Plus remains Flipper-only.

Scheduling:
- four initial;
- up to six reserve at active step 50;
- no inherited four-active-Egg cap;
- remaining queue behavior;
- lane/direction random bounds;
- eight-step cadence;
- strict active-enemy <9 gate;
- strict approaching <2 gate;
- roll 95 fails and 96 succeeds;
- near-end condition;
- scheduling freezes during death;
- reconstruction resets local phase;
- old Level1/2/diagnostic schedules unchanged.

Selection:
- negative clamp;
- rejected 0,2,3;
- accepted 1 and 4;
- exact random draw ranges;
- near-end discarded first draw and overwrite;
- Tanker cargo retry of 1/2 until 0;
- only Flipper/Tanker produced;
- production randomness not fixed.

Cap:
- zero Tankers;
- one Tanker;
- multiple Tankers;
- six simultaneous Tankers;
- unselected original produces Flipper at cap;
- selected reconstructed Tanker waits at cap without reroll;
- other Eggs can continue;
- capacity restoration;
- more than six sequential Tankers possible;
- no hatch-loop hang.

Identity:
- all 15 originals represented exactly once;
- exact selected-type mapping;
- cargo owner/ordinal uniqueness;
- separate namespaces;
- latent ownership under active/reserve/Egg carrier;
- pending activation;
- cargo re-entry reserve/Egg;
- active and terminal children;
- pair allocation exactly once;
- invalid missing/extraneous/duplicate/wrong-owner states rejected.

Natural release:
- escaped carrier;
- no carrier score;
- existing owned pair becomes live cargo;
- route/pivot/direction;
- next-step child movement;
- bullet independence.

Shot release:
- one 200 score;
- existing pair queued;
- activation boundary;
- killing shot cannot hit its new children;
- pending zap canceled;
- exact ID outcomes.

Multi-carrier Zapper:
- interleaved targets;
- exact 4/6/8... accepted delays;
- bullets reject without consuming delay;
- each carrier resolves its own original and pair;
- timer-8 child behavior;
- exact once 200/150 scores;
- second-charge selection and timer replacement;
- player-shot preemption;
- natural release vs expiry ordering;
- no stale timer.

Overlap:
- one natural carrier;
- one shot carrier;
- one zap carrier;
- independent children;
- independent bullets;
- correct terminal sets and score.

Death:
- before release;
- after natural release;
- after shot release;
- after carrier zap expiry;
- during child timers;
- one resolved sibling;
- pending activation;
- cargo reserve;
- cargo Egg;
- several carriers;
- two consecutive deaths;
- exact before/after identity-set equality.

Completion blockers:
- original reserve;
- original Egg;
- below-rim original Flipper;
- below-rim Tanker;
- latent cargo;
- activation cargo;
- cargo re-entry reserve;
- cargo Egg;
- below-rim active cargo;
- deliberately missing representation.

All-rim:
- exact represented-set match;
- kill/rim mixtures;
- pending at-rim target produces no later score;
- latent carrier blocks;
- no score for rim-clear;
- exit starts correctly;
- cleanup preserves established policies.

Firing:
- production Tanker armed without optional flag;
- ordinary and cargo Flippers silent without optional flag;
- accepted C2 constants and boundaries;
- zero/one/two outcome capability;
- fresh firing draws after carrier re-entry;
- isolated silent and combined modes unchanged.

Fresh lifecycle:
- fresh production wave resets ledger;
- ordinary death retains the same ledger;
- terminal Level 3 model does not advance;
- normal Level 1 -> Level 2 still ends at Level 2;
- old development modes remain terminal.

Regression:
- Level 1;
- Level 2;
- isolated Tanker;
- C1/C1.5 geometry;
- C2 firing;
- Super Zapper;
- Spiker;
- Fuseball;
- Pulsar;
- all-rim;
- controller/input tests.

Use strict finite range-aware random scripts.

Test scripts must:
- assert requested ranges;
- fail on unexpected draws;
- fail on exhausted queues;
- fail on unused required decisions where appropriate;
- never silently clamp broken values.

Do not use statistical assertions on production Random.Shared.

17. Implementation quality

Keep the change maintainable:

- focused comments explaining identity ownership and non-obvious lifecycle transitions;
- no comments that merely narrate syntax;
- no duplicate source of ledger truth;
- no giant catch-all helper when small transition methods are clearer;
- no reflection/test-only mutation of inaccessible gameplay internals;
- no new package or framework;
- no change to project SDK/configuration unless absolutely required, in which case stop and explain first.

Prefer small explicit transition operations such as:
- CommitOriginalAsFlipper;
- CommitOriginalAsTanker;
- MarkOriginalDestroyed;
- MarkOriginalEscaped;
- MarkOriginalRimCleared;
- MaterializeLiveCargo;
- MaterializeStillbornCargo;
- MarkCargoDestroyed;
- MarkCargoRimCleared;
- ValidateRepresentation.

Use names consistent with existing code style rather than these exact names if better.

Avoid duplicating independently mutable state across Level, ledger and TempestGame.

18. Documentation boundaries

C3a is headless groundwork, not completed C3 and not a playable Level 3 feature.

Update TEMPEST-DEVELOPMENT-NOTES.md only as needed to record:
- C3a scope;
- production mixed model exists headlessly;
- identity/accounting policy;
- evidence classifications;
- C3b/manual access still pending;
- C4 not started.

Update README.md only if current architecture/development documentation requires a brief accurate note. Do not advertise `--level3-test`, because it does not exist yet.

Update TEMPEST-BEHAVIOR-SPEC.md only for behavior actually implemented and properly classified. Do not claim manual validation of production Level 3.

Reference-derived composition/scheduling remains REFERENCE VERIFIED.

Identity retention, ledger representation, reconstruction ordering and selected-type retention remain PROJECT DECISIONS.

Do not modify Reference/incitti.

19. Validation sequence

Run focused tests iteratively.

At final validation run appropriate focused groups, adjusted to actual class names:

dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level3MixedWave|FullyQualifiedName~Level3WaveLedger|FullyQualifiedName~Tanker|FullyQualifiedName~Cargo|FullyQualifiedName~SuperZapper|FullyQualifiedName~AllRim|FullyQualifiedName~Level3Geometry|FullyQualifiedName~DirectedRotation|FullyQualifiedName~LevelProgression"

dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~LevelConfiguration|FullyQualifiedName~Level2|FullyQualifiedName~InputSystem|FullyQualifiedName~InputDiagnostic"

Then:

dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check

If a running game locks normal output files, use the established isolated BaseOutputPath approach and document it. Do not terminate the user's process without authorization.

20. Final inspection

Run:

git status --short
git diff --stat
git diff
git diff --check
git status --short -- Reference/incitti
git status --short -- TEMPEST-BEHAVIOR-SPEC.md
git ls-files --others --exclude-standard
git log -1 --oneline

Inspect every changed/untracked file.

Confirm:
- no CLI `--level3-test`;
- no Program launch wiring;
- no renderer/HUD production integration;
- no progression changes;
- no Reference/incitti changes;
- no C2 constant changes;
- no Flipper bullet promotion;
- no cargo route/pivot changes;
- no unrelated input/rendering work;
- no commit or push.

21. C3a report

Update the C3a implementation report with:

- baseline;
- exact production configuration;
- scheduling implementation;
- literal selection implementation;
- ledger structure;
- identity namespaces;
- invariants;
- each carrier/cargo path;
- multi-carrier Zapper behavior;
- death/re-entry transaction;
- all-rim/completion logic;
- production Tanker firing integration;
- headless entry design;
- tests added and updated;
- intermediate failures and corrections;
- focused results;
- full test result;
- build result;
- git diff --check;
- exact changed/untracked files;
- Reference/incitti status;
- behavior-spec status and evidence classifications;
- confirmation that C3b/manual access is not implemented;
- confirmation C4/natural progression is not started;
- any remaining blocker;
- recommended selective commit scope/message if C3a is ready.

Do not commit.
Do not push.

22. Stop condition

C3a is complete only if:

- the full production mixed-wave lifecycle exists headlessly;
- actual TempestGame orchestration is used;
- every original and cargo identity is conserved;
- invalid equal-count/missing-ID states are rejected;
- six simultaneous Tankers are supported safely;
- later Tankers can appear after capacity is freed;
- carrier natural/shot/zap outcomes work with multiple carriers;
- repeated death preserves exact unresolved sets;
- Super Zapper scores and resolves each identity exactly once;
- all-rim and completion require full representation;
- production Tankers are armed without arming Flippers;
- all old scenarios remain green;
- the full suite and build pass;
- there is no CLI/manual access or progression work.

If the implementation cannot meet this complete gate without expanding into C3b or C4, stop and report the exact blocker instead of exposing a partial production model.

No manual Level 3 gameplay acceptance occurs in C3a.
Manual acceptance belongs to C3b.