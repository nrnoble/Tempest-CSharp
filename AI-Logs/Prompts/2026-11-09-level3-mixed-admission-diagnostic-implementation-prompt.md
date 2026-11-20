FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, automatically supplied workspace context, or other agent-generated
material may appear after the user's token and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Implement a bounded LEVEL 3 MIXED-ADMISSION DIAGNOSTIC based on the completed
ROM investigation.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected starting accepted baseline:

7acac60035383bfb6e0cb170c0adfa0dd88b6c4b

Before modifying anything, verify the actual repository state against the
project-wide Copilot instructions and this expected baseline.

Read and use the existing investigation report:

AI-Logs/Reports/2026-11-09-level3-concurrency-rom-investigation-report.md

Treat that report as the detailed investigation handoff for this task. Do not
repeat the entire ROM investigation unless a specific implementation question
requires a bounded re-check of evidence.

This is an IMPLEMENTATION CANDIDATE FOR MANUAL DIAGNOSTIC TESTING.

It is NOT authorization to promote the new behavior to accepted production
behavior.

Do not commit or push.
Do not document the candidate as manually accepted.
Do not modify Levels 1 or 2.
Do not launch the interactive game.

PURPOSE

Implement the smallest safe Level 3 admission correction recommended by the
ROM investigation so Neal can manually compare Level 3 gameplay against the
arcade.

The implementation should make ORDINARY Level 3 enemy admission materially
closer to the verified Atari ROM mechanism while preserving the accepted C#
Level 3 identity, Tanker-cargo, death/reconstruction, Super Zapper, scoring,
completion, and ledger semantics.

This is specifically NOT:

- a blanket four-Flipper cap;
- a strict global seven-object conversion;
- a Level 3 scheduler rewrite;
- a Tanker cargo lifecycle rewrite;
- a death/reconstruction rewrite;
- a Super Zapper fidelity rewrite;
- a change to Levels 1 or 2.


ROM-BACKED TARGET FOR THIS DIAGNOSTIC

Use the completed investigation report as the authority for exact details.

The verified Level 3 admission concepts include:

- 15 original generic pending entries in the arcade mechanism;
- ordinary Level 3 enemy admission uses Flippers and Tankers;
- Pulsars, Spikers, and Fuseballs are not ordinary Level 3 entry types;
- ordinary admission permits at most one active Tanker;
- the ROM has seven physical active enemy slots, indices 0..6;
- Flipper maximum data is four, but four is NOT a universal hard active
  Flipper ceiling on Level 3;
- active Tanker cargo affects Flipper species availability;
- the ROM performs two wrapping byte decrements for a Tanker's two latent
  Flippers;
- ordinary Tanker admission requires nonzero Flipper availability but does
  not require availability of two;
- rim Flippers and transitioning/live Flippers remain occupancy;
- pending-zap/live enemies remain occupancy until actual removal;
- pending original entries do not occupy active physical slots;
- bullets and firing state do not consume enemy capacity;
- released Tanker cargo bypasses the ordinary species picker.

Do not simplify these findings into "Level 3 has a four-Flipper cap."


IMPLEMENTATION SCOPE

Create a narrowly scoped Level 3 mixed-admission diagnostic.

Use existing production Level 3 simulation, wave, ledger, enemy construction,
rendering, firing, scoring, and lifecycle systems.

Prefer a small explicit admission-policy implementation over broad changes.

The diagnostic must be isolated to the canonical production Level 3
configuration and its equivalent direct/recorded startup paths as appropriate.

Do not incidentally apply the policy to other production-ledger levels.

Do not alter the accepted Level 1/2 four-Flipper behavior.


ADMISSION SNAPSHOT

At the point where a ready Level 3 Egg would otherwise hatch/commit into an
active enemy, compute admission from authoritative current runtime state.

For this diagnostic, correctly distinguish at least:

- active ordinary Flippers;
- active Tanker-released Flippers;
- active Tankers;
- latent Flipper cargo on active Tankers;
- pending shot-released cargo that already represents materialized live cargo;
- rim Flippers;
- Flippers/Tankers in active lifecycle/transition states;
- pending-zap enemies that remain live;
- ready/approaching Eggs;
- reserve identities;
- terminal/resolved ledger identities;
- bullets/projectiles.

Do not count reserve, Eggs, terminal identities, bullets, or destruction
presentation as active physical enemy occupancy.

Latent Tanker cargo is not itself a physical active slot, but must influence
Flipper species availability according to the investigated ROM arithmetic.


LEVEL 3 ADMISSION POLICY

Implement a small Level-3-specific admission decision corresponding to the
verified ROM availability mechanism.

The implementation must cover:

1. Ordinary Tanker availability:
   - ordinary admission must not create a second active Tanker;
   - the presence of one active Tanker makes ordinary Tanker availability zero.

2. Flipper availability:
   - begin from the verified Level 3 Flipper maximum of four;
   - account for current active Flippers;
   - apply the verified two-unit latent-cargo subtraction for each active
     Tanker using literal 8-bit wrapping semantics where applicable;
   - clamp the resulting selectable availability to remaining physical
     admission places as described by the investigation.

3. Seven physical admission places:
   - ordinary ready-Egg admission must respect the ROM-derived seven-place
     active-object concept;
   - rim and other still-live objects count;
   - do not allow a new ordinary hatch merely because an enemy is visually in
     transition or already at the rim.

4. Species selection:
   - replace the six-Tanker production eligibility behavior for this diagnostic;
   - implement the minimum/availability behavior needed for Level 3 based on
     the investigation;
   - do not introduce arbitrary retry/random-rejection loops;
   - do not consume random state merely because an otherwise-ready Egg is
     denied admission.

5. Waiting behavior:
   - if capacity/species availability denies admission, leave the Egg ready and
     unresolved;
   - do not destroy it;
   - do not mark it terminal;
   - do not silently return it to reserve;
   - do not reroll its retained species/identity when existing C# semantics
     require that identity/species to remain stable;
   - do not repeatedly consume randomness while it waits.

6. Multiple ready Eggs:
   - if more than one Egg can hatch in the same simulation update, recompute
     occupancy/admission after every successful hatch;
   - never use a stale population snapshot across several successful hatches.


IMPORTANT CARGO COMPATIBILITY DECISION

For THIS diagnostic, preserve the current accepted C# complete-pair Tanker
cargo behavior.

Do NOT reproduce the ROM's possible one-child full-slot Tanker split.

When a Tanker releases cargo:

- preserve both established cargo identities;
- preserve the accepted complete two-Flipper materialization behavior;
- preserve normal natural/shot/zap lifecycle behavior unless an unavoidable
  implementation dependency requires a narrowly justified adjustment;
- do not discard, merge, resolve, replace, or recreate a cargo identity merely
  to force the live-object count to seven.

Therefore this diagnostic does NOT claim strict global seven-object fidelity.

If Tanker cargo release temporarily drives the live population above the
ordinary seven-place admission capacity, allow the already-authorized cargo
lifecycle to complete and block further ordinary Egg admission until occupancy
again permits admission.

Clearly comment/document this as a deliberate PROJECT DECISION / diagnostic
approximation preserving accepted C# cargo semantics.

Do not hide this difference.


PRESERVE ACCEPTED LEVEL 3 SYSTEMS

Do not redesign these systems:

- Level3WaveLedger stable identities;
- original/cargo identity ownership;
- Tanker cargo pairs;
- latent cargo;
- shot-released pending activation;
- natural cargo release;
- accepted zap/stillborn cargo handling;
- death/reconstruction;
- reentry ordering;
- score behavior;
- enemy firing;
- rim behavior;
- collision behavior;
- Super Zapper lifecycle;
- completion obligations;
- all-rim behavior;
- level progression;
- overall Egg/reserve scheduling cadence.

The purpose is admission correction, not general Level 3 ROM conversion.


REENTRY / RETAINED IDENTITIES

Handle already-committed original or cargo identities carefully.

If an identity has an established species because of existing ledger/reentry
semantics, admission must not reroll its species merely because concurrency
temporarily blocks it.

Wait until its retained species can be admitted.

Treat this as a PROJECT TRANSLATION where the stable C# ledger differs from the
ROM's generic reconstruction model.

Do not weaken ledger conservation to imitate the ROM.


TESTS

Add or adjust narrowly focused automated tests for the new diagnostic.

At minimum cover the important investigated admission states, including:

- ordinary Tanker maximum of one;
- no Level 3 Pulsar admission;
- Flipper admission with F=0,1,2 while T=1;
- Tanker eligibility around F=3,T=0;
- wrapping Flipper-availability cases around F=3,4,5 with T=1;
- total active occupancy around six and seven;
- rim Flipper occupancy;
- transition/live occupancy;
- pending-zap occupancy where applicable;
- blocked ready Egg remains ready and unresolved;
- no repeated random consumption while blocked;
- capacity reopening allows the waiting Egg to continue;
- multiple same-tick ready Eggs recompute capacity after each success;
- already-selected Tanker/reentry waits rather than rerolling identity/species;
- cargo reentry preserves identity;
- complete two-child Tanker release remains preserved;
- pending shot activation remains conserved;
- death/reconstruction retains exact unresolved identities;
- completion cannot bypass a concurrency-blocked unresolved Egg.

Include an explicit test for the chosen diagnostic approximation:

A Tanker cargo split may temporarily cause live population to exceed the
ordinary seven-place admission capacity while both cargo identities remain
preserved; further ordinary Egg admission remains blocked until capacity
reopens.

Existing artificial multi-Tanker lifecycle fixtures may remain where they are
useful for lifecycle stress coverage. Do not preserve six active Tankers as a
production admission expectation merely because an older test encoded Incitti
behavior.


VALIDATION

Follow the repository-wide command/validation/recovery rules.

Start with the smallest affected focused tests.

Then run the appropriately bounded Level 3 mixed-wave, ledger, Tanker,
concurrency, death/reconstruction, Super Zapper, completion, and relevant
Level 1/2 regression tests needed to establish that the candidate did not
damage accepted behavior.

Run the full solution tests and build only after focused validation succeeds,
if that remains proportionate and safe under the project-wide instructions.

Run git diff --check and inspect the final diff.

Do not launch the interactive Tempest game.

Do not treat automated tests as Neal's manual acceptance.


MANUAL HANDOFF

If automated validation succeeds, stop with the candidate uncommitted and
provide Neal an exact command for manually testing the Level 3 diagnostic.

Prefer a direct Level 3 launch with ten lives using the project's actual
supported command-line syntax.

If the implementation adds a dedicated diagnostic option, provide that exact
command instead.

Explain briefly what Neal should observe during manual play, especially:

- whether Level 3 now feels less overcrowded;
- whether multiple simultaneous Tankers have disappeared;
- whether Tanker cargo release still looks normal;
- whether waiting/entry cadence looks natural;
- whether Flipper population still varies naturally rather than appearing
  artificially locked to four;
- whether deaths/reentry, Super Zapper, rim behavior, and completion remain
  normal.

Do not commit or push.

Neal will decide after manual gameplay whether this candidate should be
accepted, revised, or rejected.


REPORT

Preserve the substantive prompt under AI-Logs/Prompts and create a matching
ignored implementation report under AI-Logs/Reports according to the project
instructions.

Use a descriptive report filename similar to:

2026-11-09-level3-mixed-admission-diagnostic-implementation-report.md

The report should record:

- starting baseline and Git state;
- files changed;
- admission-policy implementation;
- the deliberate complete-pair cargo approximation;
- tests added/updated;
- focused/full automated validation results;
- any blocked/partial validation;
- unresolved fidelity questions;
- exact manual test command;
- explicit status that manual gameplay acceptance is still pending.

Do not modify TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to
claim acceptance during this candidate implementation.

STOP after automated validation and manual handoff.

Do not launch the game.
Do not commit.
Do not push.


TEMPEST_LEVEL3_MIXED_ADMISSION_DIAGNOSTIC_IMPLEMENTATION_COMPLETE