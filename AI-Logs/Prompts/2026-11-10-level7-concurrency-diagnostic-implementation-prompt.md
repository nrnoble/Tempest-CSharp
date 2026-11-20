FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Implement a bounded canonical Level 7 concurrency diagnostic while explicitly
preserving all currently accepted Level 7 Flipper and Tanker-cargo movement.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

Implement accepted Level 6 concurrency and cargo movement

Read the completed investigation first:

AI-Logs/Reports/2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md

Also consult prior accepted Level 7 reports and durable documentation where
needed to preserve established behavior.

This is a Level 7 CONCURRENCY candidate only.

Do not modify Level 7 Flipper movement.

Do not modify Level 7 Tanker-cargo movement.

Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 8 concurrency.


MOVEMENT FREEZE

This boundary is mandatory.

The investigation found an unresolved evidence conflict:

- the Rev. 2 ROM listing decodes Level 7 $24 as a repeating
  2-flips / reverse / 3-flips / reverse route;
- Neal's direct arcade observation previously established and manually accepted
  the visible ordinary Level 7 pattern as 2-3-2-2-2;
- the current C# implementation intentionally reproduces Neal's accepted
  visible pattern.

Therefore this concurrency milestone MUST preserve the accepted current
movement.

Do not change:

- ordinary Level 7 Flipper 2-3-2-2-2 behavior;
- current movement handedness;
- current turn geometry/timing;
- existing C# height/update translation;
- existing deep player-shot Tanker cargo route;
- existing natural/near-rim Tanker cargo presentation;
- current cargo depth cutoff.

Do not implement ROM $24.

Do not implement ROM $07 cargo movement.

Do not reinterpret the accepted movement merely because static ROM evidence
differs.

Any movement reconciliation is a separate future milestone requiring targeted
arcade observation first.


PURPOSE

Correct only the bounded Level 7 concurrency differences established by the
ROM investigation while preserving previously accepted gameplay.

The important targets are:

1. seven physical enemy places instead of the current C# ordinary cap of six;
2. ROM-informed Level 7 species availability;
3. pending-entry lane Spike selection after minima are satisfied;
4. Tanker latent-cargo availability accounting;
5. correct Spiker behavior while generic pending supply remains;
6. preservation/verification of zero-pending same-slot Spiker-to-Tanker
   conversion;
7. verification/preservation of existing generic-obligation death
   reconstruction;
8. preservation of complete-pair C# Tanker cargo;
9. no Super Zapper redesign.


ROM-BACKED LEVEL 7 PARAMETERS

Use the investigation report as the detailed authority.

For displayed Level 7:

- Easy/Medium generic pending supply = 20;
- Hard ROM supply = 23, but do not introduce a new project-wide difficulty
  system;
- Flipper minimum/maximum = 0/5;
- Tanker minimum/maximum = 1/2;
- Spiker minimum/maximum = 2/4;
- Pulsar minimum/maximum = 0/0;
- Fuseball minimum/maximum = 0/0;
- Tanker cargo = two Flippers;
- physical enemy capacity = seven shared places;
- ordinary Flippers are not selectable because their minimum is zero;
- unmet Spiker minimum precedes Tanker minimum;
- species maxima constrain ordinary admission, not every later live
  population.


CANONICAL LEVEL 7 ONLY

Apply this diagnostic only to canonical production Level 7 and equivalent
direct/campaign/recorded production startup using the canonical configuration.

Do not change concurrency behavior for Levels 1-6 or Level 8.

Do not redesign all level admission policies into one generic framework.

Reuse previously validated engineering infrastructure where appropriate, while
keeping Level 7 parameters, selector behavior, and lifecycle rules explicit.


ORDINARY SPECIES

Fresh generic/unselected Level 7 obligations may ordinarily become:

- Tanker;
- Spiker.

They may NOT ordinarily become:

- Flipper;
- Pulsar;
- Fuseball.

Do not permit independent ordinary Flippers to enter from the bottom.

Preserve the already accepted visible rule that Level 7 Flippers arise through
Tanker cargo rather than ordinary bottom entry.


MINIMUM PRIORITY

Implement/retain:

Spiker minimum = 2
Tanker minimum = 1

Required behavior includes:

F=0,T=0,S=0
-> Spiker priority.

F=0,T=0,S=2
-> Tanker minimum becomes required.

F=0,T=1,S=2
-> minima satisfied; use ROM-informed post-minimum Level 7 selection.


SEVEN PHYSICAL PLACES

Correct the current ordinary Level 7 capacity from six to seven physical enemy
places.

Count genuine live enemy objects, including where applicable:

- materialized Flippers;
- ordinary Tankers;
- converted Tankers;
- Spikers;
- rim enemies;
- transition/live enemies;
- pending-zap enemies until removal;
- materialized cargo pending activation when it represents a committed live
  child.

Do not count:

- Eggs;
- generic pending/reserve obligations;
- latent cargo;
- bullets;
- persistent Spikes;
- resolved/terminal identities.

Rim transfer does not free a place.

A visual transition does not free a place.

Recompute occupancy after every successful same-tick hatch.


LEVEL 7 AVAILABILITY

Implement the investigated Level 7 ROM-informed availability:

- Flipper max 5;
- Tanker max 2;
- Spiker max 4;
- subtract two Flipper-availability units per live Flipper-carrying Tanker
  using the investigated byte arithmetic;
- clamp availability to shared physical headroom;
- Tanker selection requires positive availability for its Flipper cargo type.

Latent Tanker cargo does NOT occupy two physical enemy places.

A Tanker does not require two physical child slots before it can be selected.


OPEN-PLACE DEAD END

Protect this Level 7 state:

F=0,T=2,S=4

Six live Tanker/Spiker objects occupy six of seven physical places.

One physical place remains open.

Tanker availability is exhausted.
Spiker availability is exhausted.
Flipper availability may remain positive.

But Flipper minimum is zero, so ordinary Flipper selection is prohibited.

Result:

NO ordinary admission.

The pending generic obligation remains pending.

Do not fill the seventh place with an individual Flipper.

Do not treat this temporary selector dead end as zero generic pending supply.


POST-MINIMUM PENDING-LANE SPIKE SELECTION

After the relevant minima are satisfied, when Tanker and Spiker are both
available, implement the ROM-backed Level 7 pending-entry lane Spike decision.

Use the CURRENT PENDING ENTRY'S segment Spike.

Do NOT scan globally for the shortest Spike.

Use the already validated Level 6 engineering pattern only where it truly
represents the same shared ROM machinery.

Required semantics:

- obtain the pending Egg/entry lane's corresponding Spike state;
- a depleted/missing C# persistent Spike representing ROM spike_ht == 0 maps
  to ROM comparison sentinel $FF;
- comparison value >= $CC selects Spiker;
- comparison value < $CC selects Tanker.

Use the existing established ROM-along/C# height translation where needed.

Classify the coordinate mapping as PROJECT TRANSLATION / APPROXIMATION.

Do not claim literal coordinate equivalence.

Preserve a fail-closed response for genuinely invalid lane indices.


RANDOM FALLBACK

The ROM can use a random-start eligible fallback when the pending-lane
comparison path does not produce a selection.

Implement only the smallest necessary Level 7 translation established by the
investigation.

Do not introduce arbitrary retries.

Do not consume repeated randomness for an Egg that remains blocked.

Preserve deterministic recording/playback.


WAITING

If a ready Level 7 Egg cannot be admitted because of:

- physical headroom;
- species maxima;
- cargo availability;
- zero-minimum exclusion;

leave the generic obligation unresolved.

Do not:

- destroy it;
- terminalize it;
- silently lose it;
- reroll an already retained committed identity improperly;
- repeatedly consume selection/construction randomness while blocked.

When availability reopens, allow progress normally.


TANKER CARGO

Preserve the accepted C# complete-pair project translation.

Do not implement literal ROM one-child loss at saturation.

Keep:

- two stable cargo identities;
- normal release behavior;
- accepted cargo geometry;
- accepted Level 7 movement;
- scoring;
- deferred shot activation;
- Super Zapper/stillborn handling;
- completion obligations.

Cargo bypasses ordinary species selection.

If complete-pair release temporarily exceeds seven live C# enemy objects:

- preserve both children;
- block subsequent ordinary admission until physical headroom returns;
- document this as the existing PROJECT TRANSLATION.

Do not alter cargo movement in this milestone.


SPIKER WITH PENDING SUPPLY

This is a confirmed current C# lifecycle mismatch.

ROM behavior while generic pending supply > 0:

- the Spiker remains a live enemy occupying its physical place;
- it grows/retreats/hops as part of the shared Spiker lifecycle;
- it does NOT become a fresh generic pending identity merely because a retreat
  completes;
- it does NOT ordinary-reenter through a new Egg merely because of that
  retreat.

Current C# returns the Spiker's original identity to reserve at the retreat
endpoint.

Implement the smallest safe Level 7 translation needed to keep the unresolved
Spiker live/in its lifecycle while generic pending supply remains.

Do not redesign Spiker geometry, Spike growth, visual timing, or movement more
broadly than necessary.

Preserve accepted visible Spike/Spiker behavior.

If exact live-hop behavior cannot safely be represented without altering
accepted presentation, isolate the smallest engineering translation and
classify it explicitly rather than guessing.


ZERO-PENDING SPIKER CONVERSION

Preserve the existing Level 7 behavior that is already broadly aligned.

At actual generic pending supply == 0:

- a retreating Spiker converts to a Flipper-carrying Tanker;
- conversion uses the same live physical place;
- it is not ordinary Tanker admission;
- it can exceed ordinary Tanker maximum 2;
- allocate the normal accepted cargo pair exactly once;
- preserve project identity/accounting rules.

Temporary admission dead-end with pending > 0 must NOT trigger conversion.


GENERIC PENDING SUPPLY

Use authoritative ledger state.

Do not infer zero pending merely because:

- no Egg is visible;
- species are temporarily at maxima;
- no ordinary type can currently be selected;
- physical headroom is blocked.

Maintain the distinction between:

- generic pending obligations;
- active enemies;
- latent cargo;
- materialized cargo;
- terminal outcomes.


DEATH RECONSTRUCTION

Current canonical Level 7 already uses project generic-obligation
reconstruction that is broadly aligned with the ROM.

VERIFY and PRESERVE it rather than rewriting it.

Required invariants:

- unresolved active physical enemies contribute one generic obligation each;
- a Tanker carrying latent cargo contributes one active obligation, not three;
- latent cargo does not add extra generic obligations;
- each unresolved materialized cargo child contributes one obligation;
- rim enemies still unresolved at death contribute one;
- prior active species commitment is cleared;
- historical cargo linkage does not force reconstructed Flipper species;
- reconstructed obligations use Level 7 Tanker/Spiker selection;
- terminal outcomes remain terminal;
- repeated death conserves current unresolved quantity.

Add focused tests where existing Level 7 coverage does not adequately establish
these invariants.

Do not redesign the shared accepted Level 5/6 reconstruction infrastructure.


SUPER ZAPPER

The ROM pauses ordinary entry while zap_running != 0.

The C# project does not implement an exact equivalent Level 7 hatch pause.

DEFER this mismatch.

Do not redesign or retime Super Zapper.

Do not use this concurrency task to change accepted Zapper behavior.


MOVEMENT MUST REMAIN UNCHANGED

This section is intentionally repeated because it is a hard scope boundary.

Do not modify:

- the accepted ordinary Level 7 2-3-2-2-2 route;
- deep Tanker-cargo accepted movement;
- natural/near-rim cargo movement;
- C# movement handedness;
- route reversal counters;
- cargo depth cutoff;
- pivot speed;
- climb speed;
- rim movement.

The ROM $24 versus accepted arcade-observed movement conflict remains
unresolved and belongs to a separate targeted observation milestone.

No concurrency implementation change may silently "fix" movement.


TESTS

Add/update narrowly focused deterministic tests.

At minimum cover:

1. Species eligibility
   - no ordinary Flipper;
   - Tanker only where available;
   - Spiker only where available;
   - no Pulsar/Fuseball.

2. Minima/maxima
   - F 0/5;
   - T 1/2;
   - S 2/4;
   - initial Spiker priority;
   - Tanker minimum after two Spikers;
   - Tanker max 2;
   - Spiker max 4.

3. Seven-place headroom
   - seven physical places accepted as capacity;
   - eighth ordinary hatch blocked;
   - rim/live transition/pending-zap objects count;
   - latent cargo does not;
   - Eggs/Spikes/bullets do not.

4. Open-place dead end
   - F=0,T=2,S=4;
   - one place physically free;
   - no ordinary admission;
   - no Flipper selected;
   - pending obligation survives.

5. Pending-lane Spike selector
   - minima satisfied;
   - below $CC selects Tanker;
   - $CC-or-greater selects Spiker;
   - depleted/missing Spike maps to $FF and chooses accordingly;
   - selector uses the pending Egg lane, not a global scan;
   - invalid lane fails closed.

6. Tanker cargo availability
   - latent pair subtracts Flipper availability;
   - Tanker eligibility requires positive cargo availability;
   - latent pair consumes no physical child slots;
   - complete-pair release remains preserved.

7. Waiting/RNG
   - blocked ready Egg waits;
   - no repeated random consumption while blocked;
   - availability/headroom reopening permits progress;
   - same-tick hatches recompute occupancy.

8. Spiker with pending supply
   - retreat with pending > 0 does not return original to ordinary reserve as a
     new selectable obligation;
   - Spiker remains represented as the same unresolved live obligation;
   - physical place remains occupied;
   - no duplicate identity/allocation;
   - no premature terminal Tanker conversion.

9. Zero-pending conversion
   - actual pending == 0 permits same-obligation Tanker conversion;
   - cargo allocated exactly once;
   - converted Tanker may exceed ordinary max;
   - selector dead-end with pending > 0 does not convert.

10. Generic death reconstruction
   - active ordinary enemy -> one generic obligation;
   - latent-cargo Tanker -> one generic obligation;
   - each materialized child -> one generic obligation;
   - rim enemy -> one generic obligation;
   - species cleared;
   - no reconstructed individual Flipper entry;
   - repeated death remains quantity-conserving.

11. Completion
   - blocked pending obligation prevents completion;
   - retained live Spiker prevents completion;
   - latent/materialized cargo obligations remain correctly represented;
   - terminal outcomes remain terminal.

12. Isolation
   - Levels 1-6 unchanged;
   - Level 8 unchanged;
   - copied/noncanonical configurations do not acquire the canonical Level 7
     policy;
   - direct/campaign/recorded Level 7 paths use it.

13. MOVEMENT FREEZE REGRESSION
   - existing accepted Level 7 movement tests remain unchanged and passing;
   - ordinary 2-3-2-2-2 behavior remains unchanged;
   - accepted Tanker-cargo movement remains unchanged.


VALIDATION

Use repository-wide bounded validation/security rules.

Prefer Release configuration.

Start with the smallest new Level 7 concurrency policy/lifecycle tests.

Then run focused:

- Level 7 admission/policy;
- Level 7 production;
- Tanker/cargo;
- Spiker/Spike lifecycle;
- generic death reconstruction;
- movement-freeze regression tests.

If focused validation passes, run affected Release regressions covering:

- Levels 1-6;
- Level 8;
- Tanker cargo;
- Flipper movement;
- death/reentry;
- Super Zapper;
- rim/completion;
- progression;
- recording/playback;
- campaign/direct startup.

If affected validation passes:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete candidate diff review;
- final repository-state verification.

Explicitly establish that movement files/behavior were not unintentionally
changed outside any mechanically necessary shared wiring.

Stop on FAIL, BLOCKED, TIMEOUT, CANCELED, or meaningful stall.

Do not weaken or bypass Windows security.


MANUAL HANDOFF

If automated validation passes, leave the candidate uncommitted.

Do not launch the game.

Provide Neal the exact supported Level 7 command, expected to be:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10

Verify actual supported syntax before reporting it.

Manual concurrency acceptance should focus on:

1. No individual Flippers enter ordinarily from the bottom.
2. Ordinary deep-end arrivals are Tankers and Spikers.
3. Overall density/crowding feels like the arcade.
4. Ordinary Tanker population respects the expected Level 7 behavior.
5. Ordinary Spiker population respects the expected Level 7 behavior.
6. A temporarily blocked species state does not create an individual Flipper
   merely because one physical place is free.
7. Tanker complete-pair cargo release remains normal.
8. Spikers remain in their live lifecycle while pending obligations remain,
   without visibly recycling through the bottom.
9. Final true-zero-pending Spiker conversion behaves normally.
10. After Blaster death:
    - unresolved quantity appears conserved;
    - prior Flipper/cargo species does not improperly persist;
    - latent cargo does not create extra obligations;
    - materialized cargo children contribute individually.
11. Repeated death/reentry remains normal.
12. Super Zapper remains normal; do not evaluate the deferred exact
    zap_running admission gate.
13. Persistent Spikes, rim combat, completion, and Level 7 progression remain
    normal.
14. Levels 1-6 remain free of regression.

MOVEMENT MANUAL CHECK FOR THIS MILESTONE:

Only verify that the already accepted Level 7 movement has NOT changed.

Do not use this concurrency candidate to decide the unresolved ROM $24 versus
arcade-observed 2-3-2-2-2 conflict.

Movement reconciliation remains separate.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-concurrency-diagnostic-implementation-report.md

Record:

- starting baseline/Git state;
- files changed;
- seven-place capacity;
- Level 7 availability/minima/maxima;
- pending-lane Spike selector;
- depleted-Spike behavior;
- cargo availability;
- Spiker pending-supply lifecycle;
- zero-pending conversion;
- death reconstruction verification;
- complete-pair cargo preservation;
- Super Zapper deferment;
- explicit movement freeze;
- tests added/updated;
- focused/affected/full validation;
- build;
- repository state;
- exact manual launch command;
- manual acceptance pending.


DOCUMENTATION

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to
claim acceptance during this implementation candidate.

Historical Level 7 movement acceptance remains unchanged.


GIT AND STOP

Do not commit.
Do not push.

Stop after automated validation and manual handoff.

Do not begin Level 8 concurrency.
Do not begin Level 7 movement reconciliation.


TEMPEST_LEVEL7_CONCURRENCY_DIAGNOSTIC_IMPLEMENTATION_COMPLETE