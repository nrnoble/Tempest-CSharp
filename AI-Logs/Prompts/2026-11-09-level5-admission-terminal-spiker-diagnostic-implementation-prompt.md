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

Implement a bounded Level 5 ordinary-admission plus terminal-Spiker diagnostic
candidate in the Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

62c66e0ab19436ea162935627021154be21e0a41

Implement accepted Level 4 enemy concurrency

Use the completed investigation as the detailed evidence and implementation
handoff:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

Read that report before designing or changing production behavior.

Use the accepted Level 3 and Level 4 implementations as engineering
infrastructure references where genuinely useful, but do not reuse their
level-specific rules or constants.

Do not repeat the complete ROM investigation unless a specific implementation
question requires a narrowly bounded evidence check.

This is an IMPLEMENTATION CANDIDATE FOR MANUAL DIAGNOSTIC TESTING.

Do not document the candidate as accepted production behavior.
Do not commit.
Do not push.
Do not launch the interactive game.


PURPOSE

Implement the smallest safe Level-5-specific concurrency diagnostic combining:

1. ROM-informed Level 5 ordinary admission; and
2. terminal Spiker-to-Tanker conversion when mapped generic pending original
   supply is exhausted.

These behaviors are level-specific.

Do not model Level 5 as Level 3 or Level 4 with different tube geometry.

Do not apply this policy to any other level.


ROM-BACKED LEVEL 5 TARGET

Use the investigation report as the authority for detailed evidence.

For displayed Level 5 on the investigated default ROM behavior:

- generic pending original supply is 20 on Easy/Medium;
- Hard supply is 23, although this diagnostic does not need to introduce a new
  project-wide difficulty system merely to represent that table fact;
- ordinary selectable species are Tankers and Spikers;
- ordinary Flipper selection is excluded even though computed Flipper
  availability may be positive;
- Flipper minimum/maximum is 0/5;
- Tanker minimum/maximum is 1/1;
- Spiker minimum/maximum is 2/3;
- Pulsar minimum/maximum is 0/0;
- Fuseball minimum/maximum is 0/0;
- physical enemy capacity is seven shared places;
- one live Tanker contains two latent Flippers;
- latent cargo occupies no physical place;
- released cargo bypasses ordinary species selection;
- Spikers share the generic grow/retreat lifecycle;
- with no generic pending supply remaining, a retreating Spiker converts in
  its existing physical place into a two-Flipper Tanker.

Do not turn these species maxima into universal late-wave live-population caps.

Ordinary Tanker maximum 1 does not prohibit later converted Tankers.

Flipper maximum 5 does not prohibit cargo from producing more live Flippers.

The physical capacity and C# complete-pair cargo translation must be treated
separately as described below.


STRICT ISOLATION

Apply the diagnostic only to the canonical production Level 5 configuration
and its equivalent direct/campaign/recorded startup paths.

Do not alter accepted concurrency behavior for Levels 1-4.

Do not alter Levels 6-8.

Do not generalize all existing admission policies into a new framework merely
because several levels now share concepts.

Prefer a small explicit Level 5 policy over unnecessary architecture work.

The following may be reused as engineering concepts:

- authoritative admission snapshots;
- ready-Egg denial/wait;
- physical-headroom calculation;
- ledger identity preservation;
- same-tick occupancy recomputation;
- terminal same-ID Spiker-to-Tanker conversion support.

The following MUST remain Level-5-specific:

- species minima/maxima;
- zero-minimum Flipper exclusion;
- Tanker cargo reservation arithmetic;
- Tanker/Spiker priority and selection;
- terminal conversion applicability;
- retained converted-Tanker reentry behavior.


ORDINARY LEVEL 5 ADMISSION

Implement a Level-5-specific admission snapshot/policy at the ready-Egg hatch
boundary.

Count live physical occupancy consistently with the investigated ROM concept
and accepted C# representations.

Include, where applicable:

- ordinary Tankers;
- converted Tankers;
- released-cargo Flippers;
- active Spikers;
- rim enemies;
- enemies in live movement/transition states;
- pending-zap enemies until actual removal;
- shot-released cargo pending activation where it already represents committed
  live cargo;
- other genuine live enemy objects occupying runtime capacity.

Do not count:

- reserve identities;
- Eggs;
- latent Tanker cargo;
- terminal/resolved ledger identities;
- bullets;
- persistent Spike hazards;
- destruction presentation.

Latent Tanker cargo does not occupy a physical slot, but include its verified
two-Flipper reservation when computing Flipper species availability.

Use literal wrapping byte semantics only where the investigation established
that ROM arithmetic requires them.

Clamp selectable availability to remaining physical headroom as appropriate.


ZERO-MINIMUM FLIPPER RULE

This is important.

Level 5 ordinary Flipper minimum is zero.

Do not ordinary-select a Flipper merely because its max-minus-count
availability is positive.

An unselected generic original Level 5 identity may ordinarily become:

- Tanker;
- Spiker.

It may NOT ordinarily become:

- Flipper;
- Pulsar;
- Fuseball.

Visible/live Flippers should arise through established cargo/lifecycle paths,
not ordinary Level 5 bottom admission.


MINIMUM PRIORITY

Implement the investigated Level 5 minimum behavior.

Before fallback selection:

- Spiker minimum is 2;
- Tanker minimum is 1;
- unmet minima are considered according to the verified ROM ordering;
- an unmet Spiker minimum takes priority over an unmet Tanker minimum;
- Flipper minimum zero does not create a Flipper minimum obligation.

Representative required behavior includes:

F=0,T=0,S=0:
Spiker minimum has priority.

F=0,T=0,S=2:
Spiker minimum is satisfied; Tanker minimum has priority.

F=0,T=1,S=2:
both relevant minima are satisfied; use the bounded Level-5-specific
post-minimum selection rule among currently eligible ordinary species.

F=0,T=1,S=3:
ordinary Tanker and Spiker maxima are exhausted; positive Flipper availability
must NOT cause ordinary Flipper selection.

This last state may leave physical places unused.

Do not fill available physical places merely because capacity exists.


POST-MINIMUM TANKER/SPIKER SELECTION

After the relevant Tanker and Spiker minima are satisfied and both remain
eligible, implement the smallest safe Level 5 translation of the investigated
ROM Tanker/Spiker selection behavior.

The investigation identified use of the shortest-Spike coordinate threshold
$CC when both Tanker and Spiker are available.

Inspect the existing C# Spike representation and Level 5 systems sufficiently
to determine the smallest faithful project translation.

Do not redesign Spike geometry or movement.

Do not invent a probability replacement if the existing project can provide
the relevant Spike-state information.

If literal ROM coordinate $CC cannot be mapped directly to C# geometry, use a
narrow, deterministic project translation justified by existing accepted
Level 5 Spike representation and clearly classify it as APPROXIMATION /
PROJECT TRANSLATION.

Do not claim literal ROM coordinate equivalence without evidence.

Preserve deterministic recording/playback.


SEVEN-PLACE ORDINARY HEADROOM

Ordinary admission must respect seven shared physical places.

Admission must be denied if physical headroom is exhausted.

However, species availability may deny admission before all seven places are
used.

Therefore:

- seven is physical capacity;
- it is not a requirement to fill seven enemies;
- Tanker/Spiker maxima may intentionally leave places unused;
- zero-minimum Flipper availability must not be used merely to fill the array.

Recompute the snapshot after each successful same-tick hatch.


WAITING / RNG / IDENTITY

Make the admission decision before consuming or committing work that should
not occur for a denied hatch.

If admission is denied:

- keep the Egg ready;
- keep its unresolved original identity;
- preserve any already-committed retained species where existing C# semantics
  require it;
- do not reroll it;
- do not destroy/resolve/escape it;
- do not silently return it to reserve;
- do not repeatedly consume selection/construction randomness while it waits.

When capacity or species availability reopens, the waiting Egg may proceed
normally.

Admission denial remains an unresolved completion obligation.


TERMINAL SPIKER-TO-TANKER CONVERSION

Enable the generic ROM-backed terminal Spiker consequence specifically for
canonical Level 5.

At the existing accepted C# Spiker retreat boundary:

- while mapped generic pending original supply remains, preserve the current
  accepted same-ID reserve/Egg translation for this first diagnostic;
- when mapped generic pending original supply reaches zero, convert the
  retreating Spiker into a Tanker using the same original identity.

Reuse existing conversion/ledger infrastructure where appropriate.

Requirements:

- preserve original identity;
- convert exactly once;
- allocate the normal owned two-Flipper cargo pair exactly once;
- preserve original-supply accounting;
- do not fabricate a new original;
- do not ordinary-admit the converted Tanker;
- do not duplicate cargo on later updates;
- do not enable broad generic ROM death reconstruction for Level 5;
- do not change the behavior of other levels.

The current C# retreat boundary remains a PROJECT TRANSLATION /
APPROXIMATION unless stronger evidence already establishes literal timing.

Do not implement the ROM's complete shortest-Spike live-hop lifecycle merely
to support this diagnostic.


MAPPED GENERIC PENDING SUPPLY

Use an authoritative ledger-based mapping of the ROM's enemies_pending concept.

Reuse/adapt the already established project approach where appropriate.

The condition must distinguish:

- generic original identities still pending through reserve/Egg representation;
- active original identities;
- terminal original identities;
- owned cargo identities.

Cargo is not generic original pending supply.

A temporarily empty visible Egg list is not automatically zero pending supply
if generic originals remain in reserve/approach states.

Document the exact mapping in maintainability comments/tests.


CONVERTED TANKER REENTRY

A converted Tanker surviving death must not become permanently stranded because
ordinary Level 5 Tanker maximum is one.

Preserve the accepted C# stable-identity model.

For a previously committed converted Tanker:

- preserve original ID;
- preserve selected Tanker type;
- preserve owned cargo pair;
- do not allocate another pair;
- allow retained reentry subject to physical headroom rather than treating it
  as a new ordinary Tanker selection;
- if physically blocked, let it wait without rerolling identity/type.

Classify this as a PROJECT TRANSLATION, not literal ordinary ROM Tanker
admission.

Do not impose a blanket post-death maximum of one Tanker when that would strand
already committed identities.


COMPLETE-PAIR CARGO TRANSLATION

Preserve the project's accepted complete two-Flipper Tanker cargo principle.

Do not intentionally recreate the ROM case where a second cargo child may fail
physical allocation at saturation.

Preserve:

- both cargo identities;
- normal natural release;
- shot release;
- deferred activation;
- Super Zapper/stillborn lifecycle;
- scoring;
- established movement/routing.

If a complete pair temporarily causes C# live occupancy to exceed seven:

- preserve both children;
- classify this explicitly as PROJECT TRANSLATION / APPROXIMATION;
- block subsequent ordinary admission until physical headroom returns;
- do not claim strict seven-live-object fidelity for that edge condition.


SUPER ZAPPER BOUNDARY

The investigation found that the ROM pauses ordinary admission while
zap_running is active, while current C# Level 5 does not globally pause Egg
approach/hatching during its Super Zapper sequence.

For this first diagnostic:

PRESERVE the accepted C# Super Zapper lifecycle and behavior.

Do not redesign targeting, timing, score, cargo resolution, or zap delays.

Do not add a broad Super Zapper rewrite.

If a very narrow ordinary-admission pause can be implemented at the new Level 5
admission boundary without altering accepted Zapper behavior, inspect whether
that is safe and well-defined.

However, do NOT make the candidate depend on speculative mapping between ROM
zap_running duration and existing C# zap presentation/timers.

If exact mapping is not already established, leave the admission-pause mismatch
explicitly deferred and document it as such.

Neal can observe entry behavior around Super Zapper use during manual testing.


PRESERVE ACCEPTED LEVEL 5 SYSTEMS

Do not redesign or retune:

- cross-shaped Level 5 geometry;
- Level 4-to-5 progression;
- next-level/terminal progression behavior;
- starting Spikes;
- Spike persistence;
- Spike collision/shootability;
- Flipper movement;
- Tanker movement;
- Tanker cargo routing;
- Spiker growth/retreat presentation except the terminal conversion decision;
- enemy firing;
- shared enemy-shot behavior;
- scoring;
- Super Zapper charges and lifecycle;
- rim behavior;
- all-rim behavior;
- death/reentry;
- stable ledger identities;
- completion accounting;
- deterministic recording/playback;
- simulation ordering.

Do not modify Levels 1-4 or 6-8.


TESTS

Add narrowly focused Level 5 admission/lifecycle tests.

At minimum cover:

1. Species eligibility
   - unselected originals never ordinary-select Flipper;
   - no Pulsar ordinary selection;
   - no Fuseball ordinary selection;
   - only Tanker and Spiker are ordinary choices.

2. Minima and maxima
   - initial state prioritizes Spiker;
   - two-Spiker minimum is satisfied before Tanker minimum;
   - Tanker minimum then becomes required;
   - ordinary Tanker maximum is one;
   - ordinary Spiker maximum is three;
   - zero-minimum Flipper remains unavailable to ordinary selection even when
     Flipper availability is positive.

3. Representative states
   - F=0,T=0,S=0 -> Spiker priority;
   - F=0,T=0,S=2 -> Tanker priority;
   - F=0,T=1,S=2 -> bounded post-minimum Tanker/Spiker decision;
   - F=0,T=1,S=3 -> denied ordinary selection despite physical headroom;
   - saturation at seven blocks ordinary hatching.

4. Latent cargo
   - live Tanker reserves two Flipper-availability units;
   - latent cargo does not consume physical places;
   - cargo reservation arithmetic uses verified semantics;
   - released cargo bypasses ordinary species selection.

5. Waiting / deterministic behavior
   - blocked Egg remains ready/unresolved;
   - blocked Egg does not reroll;
   - repeated blocked updates do not consume new selection randomness;
   - reopened availability permits progress;
   - same-tick successful hatches recompute occupancy.

6. Terminal Spiker conversion
   - Spiker still follows accepted intermediate reserve/Egg translation while
     generic originals remain pending;
   - final/no-pending retreat converts same original to Tanker;
   - conversion happens exactly once;
   - cargo pair is allocated exactly once;
   - original supply count remains conserved;
   - multiple eligible late Spikers preserve distinct identities/cargo;
   - final Spiker cannot recycle indefinitely.

7. Converted Tanker death/reentry
   - retained converted Tanker preserves ID/type/cargo;
   - retained reentry waits when physical headroom is unavailable;
   - reentry does not ordinary-select or reroll the Tanker;
   - cargo pair is not duplicated.

8. Cargo saturation
   - complete pair is preserved;
   - temporary occupancy overflow does not lose identity;
   - subsequent ordinary admission waits until capacity reopens.

9. Existing lifecycle preservation
   - Tanker shot/natural cargo release remains correct;
   - Super Zapper cargo behavior remains correct;
   - death/reentry ledger conservation remains correct;
   - persistent Spike behavior remains correct;
   - completion/all-rim cannot bypass unresolved waiting identities/cargo.

10. Isolation
   - Levels 1-4 policies are unchanged;
   - Levels 6-8 are unchanged;
   - only canonical Level 5 receives this diagnostic policy;
   - campaign/direct/recorded Level 5 startup use the correct canonical policy;
   - copied/synthetic configurations do not accidentally acquire it unless
     specifically intended for a test.

Update old Level 5 picker tests only when they encode superseded Incitti-shaped
admission behavior rather than independent accepted lifecycle guarantees.

Do not weaken valid lifecycle tests merely to make the new policy pass.


VALIDATION

Use the repository-wide bounded validation and security rules.

Because Release configuration was the successful safe validation route for the
recent Level 4 work, prefer Release for this candidate's test validation rather
than unnecessarily reproducing the previously blocked Debug DLL route.

Start with the smallest focused Level 5 admission/lifecycle tests.

Then run bounded affected Release tests covering:

- Level 5 candidate/admission;
- Spiker lifecycle/conversion;
- Tanker/cargo;
- ledger/identity accounting;
- death/reentry;
- Super Zapper;
- persistent Spikes;
- completion/all-rim;
- campaign/direct startup;
- recording/playback;
- Levels 1-4 concurrency regressions.

If focused/affected validation succeeds, run:

- full solution Release tests with established VSTest hang protection;
- full solution Release build;
- git diff --check;
- complete tracked-diff review;
- final repository-state verification.

If Windows Application Control blocks a generated artifact, follow the
repository-wide self-stop rules.

Do not weaken or bypass security.
Do not repeat equivalent blocked routes.
Do not report blocked tests as passed.


MANUAL HANDOFF

If automated validation passes, stop with the candidate uncommitted.

Do not launch the game.

Provide Neal the exact supported direct Level 5 command with ten lives.

During manual play, ask Neal to compare against the arcade specifically for:

- overall crowding/concurrency;
- initial ordinary enemy composition;
- absence of ordinary bottom-entry Flippers;
- ordinary Tanker population;
- ordinary Spiker population;
- natural entry cadence without freezes or artificial gaps;
- Spiker growth/retreat presentation;
- late Spiker-to-Tanker behavior;
- Tanker cargo release;
- behavior when live population is crowded;
- absence of endless final-Spiker recycling;
- death/reentry;
- Super Zapper behavior and entry cadence around its use;
- persistent Spike behavior;
- rim combat;
- Level 5 completion/progression.

Do not describe a fixed final species recipe as the expected result.

Neal's manual arcade comparison is the final gameplay-fidelity authority.


REPORT

Preserve this substantial prompt and create a matching ignored implementation
report according to the repository instructions.

Use a descriptive report name such as:

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

The report should record:

- starting baseline/state;
- files changed;
- exact Level 5 admission policy;
- minima/maxima behavior;
- zero-minimum Flipper exclusion;
- Tanker cargo availability translation;
- post-minimum Tanker/Spiker decision;
- mapped generic pending supply;
- terminal Spiker conversion;
- converted-Tanker reentry;
- complete-pair saturation translation;
- Super Zapper entry-pause decision/deferment;
- tests added/updated;
- focused/affected/full validation;
- build result;
- diff/repository state;
- unresolved fidelity questions;
- exact manual launch command;
- explicit status that manual acceptance remains pending.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to
claim acceptance during this implementation-candidate task.


STOP

Stop after automated validation, report creation, and manual handoff.

Do not launch the game.
Do not commit.
Do not push.
Do not begin Level 6 concurrency.


TEMPEST_LEVEL5_ADMISSION_TERMINAL_SPIKER_DIAGNOSTIC_IMPLEMENTATION_COMPLETE