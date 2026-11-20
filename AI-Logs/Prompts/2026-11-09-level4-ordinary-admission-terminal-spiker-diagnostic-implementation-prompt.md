FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive
user-provided task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Implement a bounded Level 4 ordinary-admission plus terminal-Spiker diagnostic
candidate in the Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

af560b100bebad672ec9231d4f88332848378594

Ignore local Tempest ROM reference copies

Use the completed investigation as the detailed evidence and implementation
handoff:

AI-Logs/Reports/2026-11-09-level4-concurrency-rom-investigation-report.md

Read that report before designing or changing production behavior.

Do not repeat the entire ROM investigation unless a specific implementation
question requires a narrowly bounded re-check.

This is an IMPLEMENTATION CANDIDATE FOR MANUAL DIAGNOSTIC TESTING.

It is not authorization to document the candidate as accepted production
behavior.

Do not commit or push.
Do not launch the interactive game.
Do not modify Levels 1-3.
Do not modify Level 5 or later levels.
Do not begin unrelated fidelity work.


PURPOSE

Implement the smallest safe Level 4 diagnostic candidate that combines:

1. ROM-informed ordinary Level 4 Flipper/Spiker admission; and
2. terminal Spiker-to-Tanker conversion after generic pending supply is
   exhausted.

These two behaviors must be implemented together.

A cap-only correction is not acceptable because replacing the existing
late-wave Spiker suppression without terminal conversion could reintroduce
endless final-Spiker retreat, reserve return, Egg reentry, and Spiker
reselection.

Preserve existing accepted Level 4 lifecycle, accounting, cargo, death,
Super Zapper, Spike, firing, scoring, completion, progression, and
deterministic-playback behavior except for the narrowly authorized admission
and terminal-conversion changes described below.


ROM-BACKED LEVEL 4 TARGET

Use the investigation report for detailed evidence.

For displayed Level 4 on the examined default ROM behavior:

- original supply is 17 generic pending entries;
- ordinary selectable species are Flippers and Spikers;
- ordinary Tanker admission is zero;
- Pulsars and Fuseballs are not ordinary Level 4 selections;
- Flipper minimum/maximum is 1/4;
- Spiker minimum/maximum is 1/2;
- seven physical enemy places are shared;
- ordinary Flipper/Spiker admission can occupy at most six places because
  four Flippers plus two Spikers exhaust species availability even though one
  physical place remains;
- unmet Spiker minimum has priority before unmet Flipper minimum;
- once both minima are satisfied, eligible species selection follows the
  bounded ROM-informed selection behavior;
- rim and other still-live enemies continue occupying capacity;
- released Tanker cargo bypasses ordinary species selection;
- Tankers arise late through Spiker conversion, not ordinary admission.

Do not simplify this into a blanket four-Flipper or six-enemy global cap.

Four is the ordinary Flipper admission maximum. Released Tanker cargo may
later raise the live Flipper population above four.


STRICT SCOPE

This diagnostic may change only behavior required for:

- canonical production Level 4 ordinary admission;
- Level 4 terminal Spiker-to-Tanker conversion;
- retained converted-Tanker/cargo reentry required for identity conservation;
- focused tests and narrowly necessary fixture adjustments.

Do not broadly rewrite:

- the reserve/Egg scheduler;
- approaches or Egg movement;
- Spiker growth/retreat geometry;
- shortest-Spike lane selection;
- Spike representation or collision;
- enemy firing;
- shared shot budgets;
- scoring;
- Super Zapper behavior;
- death reconstruction generally;
- campaign progression;
- level geometry;
- recording format;
- deterministic simulation order;
- Levels 1-3 or Levels 5-8.

Do not enable generic ROM death reconstruction for Level 4 merely to obtain
terminal conversion.


LEVEL 4 POLICY ISOLATION

Apply this diagnostic only to the canonical production Level 4 configuration
and equivalent direct/campaign/recorded startup paths that use that canonical
configuration.

Do not apply it to copied or synthetic configurations unless a test explicitly
uses a copied configuration to verify isolation.

Do not alter:

- accepted Level 1/2 four-Flipper behavior;
- accepted Level 3 mixed-admission behavior;
- policies for Level 5 or later levels.

Prefer a small explicit Level-4 policy over unnecessary generic abstraction.

Reuse existing Level 3 admission infrastructure concepts only where they are
truly shared:

- authoritative live-state snapshot;
- admission at the ready-Egg boundary;
- denial before identity/type commitment or construction randomness;
- blocked Egg waiting without loss;
- fresh occupancy after each successful same-tick hatch.

Do not reuse the unchanged Level 3 species policy, Tanker-first rule, or
ordinary-carrier cargo-wrap explanation.


ORDINARY LEVEL 4 ADMISSION

Implement a small pure Level-4-specific admission/selection policy.

At the ready-Egg hatch boundary, derive an authoritative current snapshot.

Count as physical live occupancy where applicable:

- ordinary Flippers;
- Tanker-released Flippers;
- live Spikers;
- live converted Tankers;
- pending shot-released cargo already representing materialized children;
- rim enemies;
- enemies in live movement/transition states;
- pending-zap enemies until actual removal;
- any other live enemy object occupying runtime capacity.

Do not count as physical live occupancy:

- reserve identities;
- roaming, approaching, or ready Eggs;
- terminal/resolved ledger identities;
- latent Tanker cargo;
- bullets;
- persistent Spike hazards;
- destruction visuals.

Latent cargo does not occupy a physical place.

Ordinary unselected Level 4 original entries may select only:

- Flipper;
- Spiker.

Ordinary selection must not produce:

- Tanker;
- Pulsar;
- Fuseball.

Apply:

- Flipper minimum 1, maximum 4;
- Spiker minimum 1, maximum 2;
- shared physical headroom based on seven places;
- descending unmet-minimum preference, with Spiker considered before Flipper;
- bounded eligible-species selection after both minima are satisfied.

Do not preserve the old six-Tanker or four-Spiker production expectations.

Do not preserve the old near-end Spiker suppression as a hidden substitute for
the new policy. The terminal-conversion rule below replaces its essential
anti-recycling purpose for this diagnostic.

A state with four live Flippers and two live Spikers must deny another ordinary
hatch even though one of seven physical places remains, because no ordinary
Level 4 species is then available.


WAITING, IDENTITY, AND RANDOMNESS

Admission must be decided before:

- removing or consuming a ready Egg;
- committing an unselected original identity to a species;
- allocating Tanker cargo;
- constructing an enemy;
- consuming construction randomness that should not occur for a denied hatch.

If admission is denied:

- leave the Egg ready;
- preserve its unresolved identity;
- preserve a retained committed species when existing ledger semantics require
  it;
- do not reroll it;
- do not return it silently to reserve;
- do not mark it terminal;
- do not consume repeated randomness while it waits.

If several Eggs are ready during one update, recompute current occupancy and
species availability after every successful hatch. Do not reuse a stale
snapshot across multiple hatches.


TERMINAL SPIKER-TO-TANKER CONVERSION

Explicitly authorize this bounded exception to the currently accepted Level 4
retreat/reroll translation:

At the existing C# Spiker retreat/return boundary, when no generic pending
original supply remains, convert the retreating active Spiker into a Tanker
instead of returning that original identity to reserve as an unselected Egg.

Use the existing same-original-identity conversion infrastructure where
appropriate, including the existing ConvertActiveSpikerToTanker helper, rather
than replacing the original identity.

Requirements:

- preserve the same original ledger identity;
- convert exactly once;
- allocate the normal owned two-Flipper cargo pair exactly once;
- reuse existing Tanker and cargo systems;
- preserve original supply accounting;
- do not increment or fabricate another original identity;
- do not duplicate cargo allocation on later updates;
- do not perform ordinary Tanker admission;
- do not enable UsesGenericRomDeathReconstruction for Level 4;
- do not change generic death behavior for unrelated Level 4 enemies;
- do not change Levels 1-3 or Levels 5-8.

Use the current C# retreat boundary.

Classify the exact C# height/timing of that boundary as an APPROXIMATION /
PROJECT TRANSLATION, not as literal ROM coordinate/timing fidelity.

While generic pending original supply remains, preserve the existing same-ID
Spiker retreat/reserve/Egg translation for this first diagnostic unless the
implementation reveals a direct unavoidable conflict.

Do not port the entire ROM live-hop/shortest-Spike behavior in this milestone.


DEFINING "NO GENERIC PENDING SUPPLY"

Determine this from the authoritative Level 4 ledger/runtime state, not from a
visual guess.

The testable condition must correctly distinguish unresolved generic original
entries still represented in reserve/Egg/approach or otherwise pending from
terminal outcomes and separately owned cargo identities.

Do not count unresolved cargo as generic original pending supply.

Do not allow conversion merely because the visible Egg list happens to be
temporarily empty while unreleased original reserve still exists.

Document the exact project translation used to map the ROM's
enemies_pending == 0 condition onto the C# stable-identity ledger.


CONVERTED TANKER REENTRY

A terminally converted Tanker may survive a death and retain its established
type and owned cargo under the accepted C# identity model.

Ordinary Tanker admission remains zero, but a retained converted Tanker must
not be blocked forever by that rule.

Implement a narrowly documented reentry exception:

- a previously committed converted Tanker retains its identity, selected type,
  ownership, and cargo state;
- reentry is gated by physical headroom rather than ordinary Tanker species
  availability;
- delayed reentry waits without rerolling or losing identity;
- cargo is not allocated a second time;
- this is a PROJECT TRANSLATION, not ordinary ROM Level 4 Tanker admission.

Do not add a blanket two-Tanker maximum to all reconstructed C# states solely
from the fresh-ROM-life inference.

Do not allow new unselected originals to enter as Tankers.


CARGO AND SATURATION

Preserve the accepted C# complete-pair Tanker cargo behavior.

When a converted Tanker releases cargo:

- preserve both owned cargo identities;
- preserve natural, shot, and Super Zapper cargo lifecycle behavior;
- preserve existing score attribution;
- preserve pending shot activation;
- preserve zap-delayed/stillborn handling;
- do not discard or silently resolve a second child merely to maintain seven
  live objects.

This means complete-pair release may temporarily raise live C# occupancy above
seven at saturation.

Treat that as an explicit PROJECT TRANSLATION / diagnostic approximation.

While occupancy is at or above ordinary capacity, block further ordinary Egg
hatches until physical headroom returns.

Do not claim strict seven-live-object ROM fidelity for this cargo-release edge.


PRESERVE ACCEPTED LEVEL 4 SYSTEMS

Preserve unless a narrowly necessary dependency is proven and reported:

- stable original and cargo identity namespaces;
- exact identity accounting and representation checks;
- destroyed, escaped, and rim-cleared outcomes;
- Tanker cargo geometry and pivots;
- deferred shot activation;
- independent enemy bullets;
- enemy firing behavior and shared shot budget;
- scoring;
- death interval, freeze, retraction, and reentry;
- persistent shortened Spike objects and shot-collision restoration;
- Super Zapper charges, targeting, ordering, delays, shot preemption, and
  stillborn lifecycle;
- Spike growth, shooting, collision, and exit behavior;
- all-rim and ordinary completion obligations;
- Level 3-to-4-to-5 progression and transition presentation;
- fixed simulation order;
- seeded randomness;
- recording/playback format and determinism.

Admission denial must not be treated as destruction, escape, rim clearing, or
completion.

A blocked unresolved Egg remains a completion obligation.


TESTS

Add narrowly focused tests for the new diagnostic.

At minimum cover:

1. Ordinary Level 4 species and arithmetic
   - F=0,S=0: unmet Spiker minimum has priority.
   - F=0,S=1: unmet Flipper minimum has priority.
   - F=1,S=1: both eligible under bounded selection.
   - F=4,S=1: Spiker only.
   - F=1,S=2: Flipper only.
   - F=4,S=2: no ordinary admission despite one physical place.
   - zero ordinary Tanker admission.
   - no Pulsar or Fuseball ordinary admission.
   - no more than two ordinarily admitted Spikers.
   - no more than four ordinarily admitted Flippers.

2. Occupancy
   - rim enemies count;
   - transitioning/live enemies count;
   - pending-zap enemies count until removal;
   - shot-released pending activation counts;
   - Eggs/reserve/bullets/Spike hazards do not consume physical places;
   - occupancy at seven blocks ordinary hatching;
   - complete-pair cargo overflow blocks later ordinary hatches until room
     returns.

3. Waiting and randomness
   - blocked Egg remains ready and unresolved;
   - blocked admission does not consume repeated randomness;
   - retained species/identity is not rerolled;
   - reopening capacity permits later hatch;
   - multiple same-tick hatches use fresh occupancy after each success.

4. Terminal conversion
   - Spiker retreat while generic original supply remains uses the existing
     approved intermediate same-ID reserve/Egg translation;
   - Spiker retreat when generic original pending supply is exhausted converts
     the same original identity to Tanker;
   - conversion occurs exactly once;
   - cargo pair is allocated exactly once;
   - conversion does not change total original supply;
   - two independently eligible Spikers can convert without identity or cargo
     duplication;
   - no endless final-Spiker recycling;
   - no ordinary Tanker is introduced before the terminal conversion path.

5. Death and reentry
   - converted Tanker retains established identity/type/cargo after death;
   - converted Tanker can reenter under physical-headroom-only policy;
   - reentry waits when physically blocked;
   - reentry does not reroll type or allocate another cargo pair;
   - materialized cargo identities remain conserved through death/reentry.

6. Existing lifecycle
   - ordinary and shot Tanker release still produce the complete pair;
   - Super Zapper cargo lifecycle remains intact;
   - scoring remains intact;
   - persistent Spike/death restoration remains intact;
   - completion cannot bypass a waiting Egg, converted Tanker, latent cargo, or
     pending cargo activation;
   - all-rim exact representation checks remain intact.

7. Isolation and determinism
   - Levels 1-3 remain unchanged;
   - Levels 5-8 remain unchanged;
   - campaign, direct Level 4, and recorded startup use the intended canonical
     Level 4 diagnostic policy;
   - copied/synthetic configurations do not accidentally acquire the policy;
   - finite-seed/tick deterministic behavior remains reproducible.

Existing tests that encode six ordinary Tankers, four ordinary Spikers, or the
old late-Spiker suppression as production Level 4 expectations should be
updated only after confirming that they represent the superseded selection
behavior rather than independent accepted lifecycle guarantees.

Keep artificial multi-Tanker stress fixtures where still useful for lifecycle
testing, but isolate them from canonical production Level 4 admission.


VALIDATION

Follow the repository-wide bounded command, security, stall, recovery, and Git
rules.

Begin with the smallest new policy/conversion tests.

Then run bounded affected tests covering:

- Level 4 admission and candidate behavior;
- Spiker retreat/conversion;
- Tanker cargo;
- ledger and identity accounting;
- death/reentry;
- Super Zapper;
- persistent Spikes;
- completion/all-rim;
- campaign/direct startup;
- recording/playback;
- Levels 1-3 concurrency regressions.

If focused validation passes, run the full solution tests with established
VSTest hang protection and run the full solution build.

Also run:

- git diff --check;
- complete tracked-diff review;
- final repository-state verification.

Do not launch the interactive game.

Do not treat automated validation as Neal's manual arcade-fidelity acceptance.


MANUAL HANDOFF

If automated validation succeeds, leave the candidate uncommitted and provide
the exact task-appropriate command for Neal to launch Level 4 directly with ten
lives.

Prefer the project's actual supported Level 4 diagnostic command.

Tell Neal to observe:

- whether Level 4 feels less overcrowded and closer to the arcade;
- whether no Tankers enter ordinarily during the main pending wave;
- whether no more than two Spikers appear through ordinary admission;
- whether Flipper/Spiker entry cadence looks natural;
- whether Spikers still grow and retreat normally;
- whether late retreating Spikers convert into Tankers after original pending
  supply is exhausted;
- whether converted Tankers and their two-Flipper cargo behave naturally;
- whether the final wave resolves without endless Spiker recycling;
- whether death/reentry remains normal;
- whether Super Zapper behavior remains normal;
- whether Spike hazards, rim combat, completion, and Level 4 exit remain normal.

Do not commit or push.

Neal will determine whether the candidate is accepted, needs revision, or
should be rejected.


DOCUMENTATION AND REPORT

Preserve this substantial prompt and create a matching ignored implementation
report according to the repository instructions.

Use a descriptive report name such as:

AI-Logs/Reports/2026-11-09-level4-ordinary-admission-terminal-spiker-diagnostic-implementation-report.md

The report should include:

- starting baseline and Git state;
- files changed;
- Level 4 admission policy;
- terminal Spiker-conversion implementation;
- exact C# mapping of generic pending supply exhaustion;
- retained converted-Tanker reentry policy;
- complete-pair cargo saturation translation;
- tests added and updated;
- focused/full validation;
- build and diff results;
- unresolved fidelity questions;
- exact manual launch command;
- explicit status that manual acceptance remains pending.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to
claim acceptance during this candidate implementation.

If limited non-acceptance documentation is genuinely required to explain a
test seam or code invariant, keep it narrowly classified and do not promote
the candidate to accepted behavior.


STOP

Stop after automated validation, report creation, and manual handoff.

Do not launch the game.
Do not commit.
Do not push.
Do not begin another milestone.


TEMPEST_LEVEL4_ADMISSION_TERMINAL_SPIKER_DIAGNOSTIC_IMPLEMENTATION_COMPLETE