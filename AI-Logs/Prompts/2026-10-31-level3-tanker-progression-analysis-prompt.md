Analyze the next Tempest milestone: natural Level 3 progression and production
Tanker integration.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Starting clean checkpoint:
f6ac233 Implement Level 1 to Level 2 progression

ANALYSIS ONLY.

Do not modify tracked gameplay code, tests, README,
TEMPEST-DEVELOPMENT-NOTES.md, TEMPEST-BEHAVIOR-SPEC.md, project files,
configuration files, or Git state.

Do not implement Level 3.
Do not commit or push.

WORKFLOW / MAINTENANCE

Restore the useful low-cost maintenance steps that were previously reduced for
credit conservation.

At the beginning, run and record:

git status --short
git log -1 --oneline
git diff --check

Verify:

- HEAD is f6ac233;
- working tree is clean;
- Reference/incitti has no tracked modifications.

Because this is analysis-only, do not run the full build/test suite unless
source inspection reveals a concrete reason that analysis depends on current
execution results.

At the end, run and record:

git status --short
git diff --check

Confirm tracked files remain unchanged.

The final commit, if any future implementation is approved, remains under
Neal's control.

FIRST READ

Current authority and architecture:

- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- README.md
- AI-Logs/Reports/2026-10-30-progression-architecture-analysis-report.md
- AI-Logs/Reports/2026-10-31-level1-to-level2-progression-analysis-report.md
- AI-Logs/Reports/2026-10-31-level1-to-level2-progression-implementation-report.md
- AI-Logs/Reports/2026-10-30-level1-configuration-groundwork-report.md
- AI-Logs/Reports/2026-10-30-level2-square-canflip-analysis-report.md
- AI-Logs/Reports/2026-10-30-level2-square-b1-implementation-report.md
- AI-Logs/Reports/2026-10-31-level2-square-b2-implementation-report.md

Tanker history:

- current Tanker.cs, Flipper.cs, Egg.cs, Enemy.cs and TempestGame.cs
- current simulation, Super Zapper and progression tests
- all relevant Tanker reports, including:
  - initial Tanker analysis and implementation;
  - Tanker enemy-bullet work;
  - Super Zapper Tanker/stillborn-cargo work;
  - cargo-direction correction;
  - all-rim accounting analysis;
  - Stage A accounting/configuration work.

Reference:

- Reference/incitti/tempest.bmx
- original Level 3 board row and relevant level-set data
- literal SetUpLevel / SetUpEnemies / Egg Hatch / Tanker creation and cargo
  paths
- relevant Incitti completion, death conversion, Super Zapper and transition
  code.

Treat checkpoint f6ac233 and the current TEMPEST-BEHAVIOR-SPEC.md as authority
over stale historical reports.

EVIDENCE MODEL

Maintain:

ARCADE VERIFIED
REFERENCE VERIFIED
PROJECT DECISION
DEVELOPMENT ONLY

Incitti describes Incitti, not necessarily Atari Tempest.

Neal's direct arcade observations take precedence.

Passing tests establish C# behavior, not arcade fidelity.

PURPOSE

Determine the smallest safe sequence for extending the normal campaign from:

Level 1 -> Level 2 -> terminal

to:

Level 1 -> Level 2 -> Level 3

where natural Level 3 introduces Tankers into a genuine mixed Flipper/Tanker
wave.

This analysis must answer whether natural Level 3 can be implemented in one
reviewable milestone or must be split into:

- Level 3 geometry/configuration isolation;
- production mixed-wave accounting;
- natural Level 2 -> Level 3 progression;
- Tanker firing promotion/tuning;
- or another evidence-derived sequence.

Do not accept that example sequence automatically. Derive the recommendation
from current code and reference behavior.

LEVEL 3 CONFIGURATION

Establish exact Incitti displayed-Level-3 reference values:

- internal level identity;
- displayed level;
- board index;
- palette index;
- exact ordered board coordinates;
- closed/open topology;
- vertex count;
- playable lane count;
- center Y;
- Y offset;
- depth/position;
- player start lane;
- enemy supply;
- initial Eggs;
- reserve supply;
- release behavior;
- hatch behavior;
- canflip;
- Flipper tuning;
- Tanker tuning;
- enemy bullets;
- Super Zapper;
- any other level-sensitive value that changes from Level 2.

Do not infer these values from memory.

Trace them from the literal source and board file.

Clearly distinguish:

- reference values;
- existing C# project policies;
- behavior requiring manual Atari comparison.

LEVEL 3 BOARD / GEOMETRY

Determine whether displayed Level 3 uses a new closed geometry and whether the
current B1/B2 renderer and Flipper hinge logic can represent it correctly.

For the exact Level 3 board:

- list all ordered points;
- identify lane topology;
- calculate adjacent-lane turn behavior;
- identify straight, corner, acute, obtuse, collinear, repeated-coordinate or
  potentially degenerate transitions;
- determine whether B1's square-specific endpoint-hinge path applies;
- determine whether the legacy Level 1 pivot path applies;
- determine whether a new generalized or board-specific transition helper is
  required.

Do not globally replace already validated Level 1 or Level 2 geometry.

Check:

- Player rim movement;
- Eggs/shots;
- ordinary canflip Flippers;
- rim pivots;
- Tanker glyph;
- Tanker cargo pivots;
- Super Zapper flash;
- finite projection;
- potential zero-length or zero-angle cases.

If geometry itself is a risk, recommend an isolated Level 3 geometry checkpoint
before mixed-wave work.

LITERAL LEVEL 3 COMPOSITION

Trace the exact Egg hatch-selection decision for internal level 2 / displayed
Level 3.

Determine:

- which enemy types are eligible;
- whether only Flippers and Tankers are eligible;
- random selection ranges and ordering;
- active-count restrictions;
- near-end-of-wave restrictions;
- maximum active Tankers, if any;
- whether each Egg independently selects a type;
- how many Tankers can appear in one source wave;
- whether selection depends on existing Tankers, active enemies, Eggs, reserve
  supply or cargo;
- whether Tanker creation changes source counters immediately;
- whether Tanker cargo expands active enemy count in the source;
- whether Level 3 source composition can produce zero, one or multiple
  Tankers.

Do not substitute the current controlled slot-0 `--tanker-test` composition for
natural Level 3.

Do not invent a deterministic production composition without clearly labeling
it a PROJECT DECISION.

PRODUCTION MIXED-WAVE ACCOUNTING

This is the central architecture question.

Map current accounting for:

1. ordinary configured units;
2. destroyed units;
3. rim-cleared units;
4. active/reserve Eggs;
5. Flippers;
6. the controlled Tanker original-slot ledger;
7. latent Tanker cargo;
8. materialized cargo IDs;
9. shot-destroyed Tankers;
10. naturally released Tankers;
11. Super-Zapped Tankers;
12. stillborn cargo;
13. death/re-entry;
14. all-rim clearing;
15. wave completion.

Determine why the current development Tanker ledger is scenario-specific.

Determine what minimal generalization is required for:

- multiple possible Tankers;
- Tanker identities selected during normal hatching;
- Flippers and Tankers coexisting naturally;
- each Tanker carrying two latent Flippers;
- live cargo increasing the unresolved population after carrier resolution;
- life loss before release;
- life loss after release;
- kills, escapes and rim-clears remaining distinct;
- no duplicated or lost cargo identity;
- completion blocking until every unresolved production obligation is resolved.

Do not collapse everything into a simple count if stable identity is required
for correct reconstruction and Super Zapper behavior.

Do not build a universal entity-component accounting system unless the current
requirements genuinely justify it.

IDENTITY MODEL

Evaluate whether natural Level 3 should use:

A. configured original wave-slot IDs for every Egg;
B. dynamic enemy-unit IDs assigned at hatch;
C. the current Tanker original-slot model generalized to all production units;
D. another minimal model supported by current architecture.

Explain:

- identity creation;
- Egg identity carriage;
- Flipper/Tanker hatch conversion;
- latent cargo ownership;
- materialized child identity;
- resolved outcomes;
- death reconstruction;
- fresh-level reset.

Determine whether ordinary Level 1/2 need migration to the new identity model
or whether Level 3 can use a mixed-wave accounting policy without disturbing
the validated earlier levels.

Prefer the narrowest design that remains correct.

TANKER CREATION / TUNING

Compare current development Tanker behavior with literal Level 3 source behavior:

- start height;
- climb speed;
- release threshold;
- lane behavior;
- shot collision;
- score;
- natural release;
- live cargo release;
- cargo placement;
- initial pivot;
- cargo movement below rim;
- rim pursuit;
- death/re-entry;
- rendering.

Identify which current development approximations are suitable for natural
Level 3 and which must become configuration-driven.

Do not alter the arcade-validated observable cargo result:

- shooting a Tanker destroys only the Tanker;
- two live cargo Flippers survive;
- children appear in distinct non-Tanker lanes;
- cargo does not dynamically track the Blaster below the rim.

The recreation's deterministic outward cargo routing is an existing PROJECT
DECISION matching Neal's observed final result. Do not silently revert to the
reference's conflicting inward path.

TANKER BULLETS

Neal's direct arcade observation is:

- Atari Tankers fire individual shots at intervals;
- at least two shots can occur before the Tanker reaches the rim/release area.

Current C# policy:

- `--tanker-test` alone leaves the Tanker silent;
- combined `--tanker-test --enemy-bullets-test` arms it;
- one observed C# Tanker fired one shot before release;
- exact current ammunition, delay, chance and height cutoff are
  Incitti-derived and not arcade-validated.

Analyze Level 3 options:

A. natural Level 3 Tankers remain silent pending a separate firing milestone;
B. natural Level 3 enables the existing Incitti-derived Tanker firing;
C. natural Level 3 uses a revised arcade-informed Tanker firing policy;
D. split Tanker firing validation before Level 3 integration.

Choose the safest sequence.

Do not invent exact Atari ammunition or cadence from the qualitative
observation.

Determine whether natural Level 3 without firing would be an intentionally
incomplete but honest milestone, or whether Tanker firing is essential enough
that it should precede production integration.

FLIPPER BULLETS

Natural enemy bullets are currently opt-in.

Determine whether natural Level 3 should retain that policy.

Do not automatically enable Flipper bullets merely because Incitti does.

Preserve `--enemy-bullets-test` as an orthogonal development option unless
there is strong project evidence to change it.

SUPER ZAPPER

Natural Level 3 must support:

- fresh two-charge setup;
- first all-target use;
- second single-target use;
- delayed Tanker resolution;
- 200-point Tanker score;
- stillborn cargo;
- child IDs;
- delayed child resolution;
- exact-once scoring/accounting;
- pending target pre-emption;
- ordinary life loss before/after materialization;
- all-rim behavior.

Determine whether the current controlled single-Tanker Super Zapper ledger can
support multiple production Tankers without identity collision.

Explicitly analyze:

- multiple simultaneously pending Tankers;
- staggered all-zap timers;
- cargo spawned from more than one carrier;
- cargo ID uniqueness;
- death during pending timers;
- completion blocking;
- score/order correctness;
- all-rim clearing of pending carriers or children.

ALL-RIM COMPLETION

Natural Level 3 must preserve the validated broad all-rim behavior.

Analyze:

- unreleased/active Eggs gate;
- below-rim Tanker blocks;
- latent cargo represented through its carrier;
- released below-rim cargo blocks;
- pending zap targets;
- all active qualifying Flippers/Tankers/cargo at rim;
- no false kill score;
- distinct Tanker destroyed, escaped and rim-cleared outcomes;
- exact-once cargo resolution;
- standard exit/progression commit.

Level 3 normal progression will have no Spikes unless source evidence says
otherwise. Confirm this.

DEATH / RECONSTRUCTION

Design the required Level 3 death behavior.

Before carrier release:

- unresolved Tanker obligation returns correctly;
- latent cargo does not become duplicate Eggs.

After natural/shot release:

- resolved carrier remains resolved;
- unresolved live cargo returns correctly;
- destroyed children remain resolved.

After Super Zapper carrier resolution:

- carrier score/outcome remains resolved;
- stillborn or unresolved materialized children are reconstructed or resolved
  according to the established project policy;
- no identity duplicates occur.

Also analyze repeated deaths and a mixture of multiple carriers/cargo.

LEVEL 2 -> LEVEL 3 PROGRESSION

Assuming natural Level 3 becomes implementable, define how the existing
successful-travel resolver should extend:

Level 1 -> Level 2 -> Level 3.

Preserve:

- score;
- current lives;
- fresh two-charge Super Zapper;
- projection reset;
- fresh accounting;
- first-frame geometry;
- no stale Level 2 entities;
- development scenarios remaining terminal.

Do not implement this during analysis.

Determine terminal policy after Level 3 while Level 4 is unavailable.

Prefer the same explicit no-next-level approach unless another policy is
better justified.

DEVELOPMENT ACCESS

Determine whether Level 3 needs a direct:

--level3-test

If recommended, define:

- DEVELOPMENT ONLY status;
- flag conflicts;
- replay/Game Over semantics;
- use of the exact same production Level 3 configuration;
- whether it should exercise full production mixed selection;
- deterministic/manual-debug aids without changing production behavior.

Do not create a duplicate Level 3 configuration.

TEST STRATEGY

Design deterministic tests for the recommended next implementation milestone.

Include as applicable:

BASELINE / MAINTENANCE:
- clean checkpoint verification;
- focused tests during development;
- full 409-test-or-later suite at completion;
- solution build;
- git diff --check;
- git status --short;
- git diff --stat;
- Reference/incitti unchanged;
- unexpected/untracked-file review.

CONFIGURATION:
- Level 3 identity;
- exact board coordinates/topology;
- supply and initial Eggs;
- tuning;
- eligible composition;
- fresh setup.

GEOMETRY:
- all adjacent lane transitions;
- canflip;
- rim pivot;
- Tanker/cargo;
- finite projections;
- no Level 1/2 regression.

COMPOSITION:
- deterministic selection boundaries;
- zero/one/multiple Tankers if source permits;
- end-of-wave restrictions;
- no unrelated enemy classes.

ACCOUNTING:
- original units;
- kills;
- natural escapes;
- rim clears;
- latent cargo;
- materialized cargo;
- multiple carrier identities;
- exact conservation invariants;
- completion blocking.

DEATH:
- before and after cargo release;
- multiple carriers;
- repeated reconstruction;
- no duplicates/losses.

SUPER ZAPPER:
- multiple carriers;
- staggered expiry;
- stillborn cargo IDs;
- score/order;
- death and all-rim interaction.

PROGRESSION:
- Level 2 successful exit installs Level 3 only after successful travel;
- score/lives carry;
- fresh Zapper;
- no stale entities;
- Level 3 terminal policy;
- development scenarios stay terminal.

REGRESSION:
- Levels 1 and 2 remain exact;
- direct `--tanker-test` remains available;
- Spiker/Fuseball/Pulsar scenarios unchanged;
- Tanker firing observation remains documented if not implemented.

MANUAL VALIDATION PLAN

Define what Neal should observe for the recommended milestone.

Separate:

- geometry validation;
- mixed Flipper/Tanker composition;
- live cargo;
- death/re-entry;
- Super Zapper;
- all-rim completion;
- Tanker firing if included;
- Level 2 -> Level 3 progression if included.

Do not require exact timer measurements.

STAGED ROADMAP

Recommend a bounded Level 3 roadmap.

Potential stages might be:

C1. isolated Level 3 geometry/configuration;
C2. generalized production Tanker accounting and mixed wave;
C3. Tanker firing policy/validation;
C4. natural Level 2 -> Level 3 progression.

But do not accept that sequence automatically.

Choose stages based on actual dependencies and rollback value.

For each stage provide:

- purpose;
- prerequisites;
- expected files;
- automated checks;
- manual checks;
- documentation/spec impact;
- checkpoint boundary.

EXACTLY ONE NEXT IMPLEMENTATION MILESTONE

Choose exactly one next implementation milestone.

It must:

- start from clean `f6ac233`;
- be small enough to review;
- have a clear manual validation target;
- not intentionally corrupt or fake production composition;
- not depend on unimplemented future behavior;
- preserve current Levels 1 and 2;
- include proportionate maintenance/Git validation.

If the correct next step is analysis-derived groundwork rather than visible
Level 3, say so plainly.

BEHAVIOR SPECIFICATION

Identify future changes only after implementation/manual validation.

Do not edit TEMPEST-BEHAVIOR-SPEC.md during analysis.

OUTPUT

Save this exact prompt to:

AI-Logs/Prompts/2026-10-31-level3-tanker-progression-analysis-prompt.md

Save the analysis to:

AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md

The report must include:

1. baseline Git/maintenance result;
2. executive recommendation;
3. exact Level 3 reference configuration;
4. Level 3 geometry analysis;
5. literal composition selection;
6. current C# Tanker/accounting map;
7. production mixed-wave accounting requirements;
8. identity model recommendation;
9. Tanker tuning/configuration;
10. Tanker bullet policy;
11. Flipper bullet policy;
12. Super Zapper/multiple-carrier analysis;
13. all-rim behavior;
14. death/reconstruction;
15. Level 2 -> Level 3 transition;
16. development-access recommendation;
17. staged roadmap;
18. exactly one next implementation milestone;
19. deterministic test strategy;
20. manual-validation strategy;
21. expected files;
22. documentation/Behavior Spec impact;
23. explicit out-of-scope items;
24. remaining arcade uncertainties;
25. final Git/maintenance result;
26. GO / NO-GO recommendation.

STOP after analysis/report generation and final maintenance checks.

Do not implement.
Do not modify tracked files.
Do not commit or push.