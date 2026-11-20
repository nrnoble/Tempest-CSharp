FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Implement a tightly bounded Level 5 enemy-selection correction:

Prevent ordinary original Flippers from entering directly from the bottom of
Level 5, while preserving Tanker-released cargo Flippers.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2
Close out accepted common type-0 destruction effect

Verify the actual Git state before modifying anything.


PRIMARY INVESTIGATION REPORT

Read this report before changing production code:

AI-Logs/Reports/2026-11-08-level1-7-enemy-supply-rom-investigation-report.md

Reuse its findings rather than repeating the full Levels 1-7 investigation.


LOCAL ROM AUTHORITY

The ROM-derived annotated Tempest disassembly is already stored in the project:

Reference/arcade/Tempest Rom Source code.md

Use this project-local reference directly.

Do not search the internet for another Tempest disassembly.

Do not substitute Mark Incitti's implementation or another Tempest port for
the local ROM evidence.

Recheck the relevant local ROM tables/control flow as necessary before
implementing the correction.


ESTABLISHED ROM FINDING

The previous investigation established that Level 5 is the first implemented
level where the ROM Flipper minimum becomes zero.

For Level 5 the ROM-derived relevant selection limits are:

- Flipper: min 0, max 5
- Tanker: min 1, max 1
- Spiker: min 2, max 3
- Pulsar: 0
- Fuzzball: 0

The ROM enemy-selection logic requires a nonzero type minimum for ordinary
bottom selection.

Therefore on Level 5:

- ordinary Flippers are not selected as normal bottom entrants;
- Flippers can still materialize as Tanker cargo;
- the nonzero Flipper maximum remains relevant to active/cargo accounting and
  must not be interpreted as permission for ordinary Flipper entry.

Recheck the exact relevant tables and selector path in the local ROM listing
before editing production code.


CURRENT C# DISCREPANCY

The current Level 5 C# selector allows ordinary original Flippers as part of
its random Flipper/Tanker/Spiker mix.

This differs from the established ROM Level 5 selection rule.

The purpose of this milestone is ONLY to correct that specific discrepancy.


IMPLEMENTATION SCOPE

For production Level 5:

1. Prevent an unresolved ordinary/original Level 5 wave unit from being
   selected/hatching as a directly entering Flipper.

2. Preserve normal Level 5 Tanker selection.

3. Preserve normal Level 5 Spiker selection.

4. Preserve Flippers released from Tankers.

5. Preserve cargo-Flipper creation, ownership, activation, movement, firing,
   destruction, death/re-entry handling, and completion obligations.

Implement the smallest maintainable policy change that expresses this
Level-5-specific rule.

Prefer extending an existing level-selection policy/seam over introducing a
parallel wave architecture.

Do not copy the entire Level 7 picker merely to obtain its no-direct-Flipper
property unless current architecture genuinely makes a small shared policy
appropriate.

Do not accidentally give Level 5 Level 7-specific behavior.


MUST REMAIN UNCHANGED

Keep Level 5 base original supply at:

20

Do not change:

- initial four-Egg arrangement;
- reserve count;
- 50-tick release cadence;
- Egg approach behavior;
- active-enemy global caps;
- Tanker concurrent cap;
- Spiker concurrent cap;
- Tanker cargo count;
- Tanker cargo identity ownership;
- wave ledger semantics;
- death/re-entry reconstruction;
- completion accounting;
- score values;
- sounds;
- enemy movement;
- Flipper movement;
- Tanker movement;
- Spiker movement;
- enemy firing;
- starting Spikes;
- Spike heights;
- Super Zapper;
- enemy-destruction effects;
- Level 5 geometry;
- Level 4→5 progression;
- Level 5→6 progression;
- HUD behavior;
- recording/playback format or controls;
- screenshot functionality.

This milestone intentionally does NOT yet correct Level 5's ROM Tanker max 1,
Spiker max 3, global six-active gate, or ROM release/pending timing.

Those are separate potential future milestones.


OTHER LEVELS

Do not change Level 6 in this prompt.

Although the prior ROM investigation found the same no-direct-Flipper rule for
Level 6, Level 6 will be considered separately after Neal manually evaluates
this Level 5 correction.

Do not alter Levels 1-4.

Do not alter Level 7.

In particular, preserve Level 7's accepted:

- dedicated selection logic;
- no-direct-Flipper behavior;
- Tanker/Spiker policy;
- generic re-entry behavior;
- final-Spiker/same-unit Tanker conversion;
- ordinary and cargo Flipper movement behavior.


LEVEL 3 / LEDGER PROTECTION

Preserve the accepted C# Level 3-6 identity model.

Do not redesign it to imitate the ROM's scalar pending counter.

A Level 5 Tanker remains one original identity whose commitment can allocate
two owned cargo-Flipper identities.

The Level 5 original supply must remain 20.

Cargo Flippers are not additional original supply slots.

Do not alter the established ledger conservation and completion semantics.


DEATH / RE-ENTRY

The Level 5 correction must remain correct across death and reconstruction.

Ordinary unresolved Level 5 original identities that are reconstituted must
remain subject to the Level 5 no-direct-Flipper policy.

Do not allow a death/re-entry path to bypass the new selection rule and create
an ordinary bottom Flipper.

At the same time, unresolved materialized Tanker cargo must retain its existing
accepted reconstruction behavior.

Do not alter that behavior merely to enforce the selection rule.


REQUIRED AUTOMATED TESTS

Add or refine focused tests proving at least:

1. LEVEL 5 ORDINARY SELECTION
   - Across deterministic selection cases/seeds sufficient to exercise the
     selector, no ordinary/original Level 5 Flipper can be selected for bottom
     entry.
   - Tankers remain selectable.
   - Spikers remain selectable.

2. LEVEL 5 CARGO
   - Destroying or naturally resolving/releasing a Level 5 Tanker can still
     materialize its two Flipper cargo children as appropriate.
   - Cargo Flippers are not rejected by the new no-direct-Flipper rule.

3. SUPPLY
   - Level 5 original supply remains exactly 20.
   - Initial/reserve arrangement remains unchanged.
   - No original obligations are lost merely because ordinary Flipper
     selection is disabled.

4. IDENTITY / LEDGER
   - A Tanker still consumes one original identity.
   - Its two cargo identities remain separately owned/represented according to
     the current accepted C# ledger model.
   - No duplicate identity allocation occurs.

5. DEATH / RE-ENTRY
   - Reconstructed unresolved Level 5 original supply cannot hatch as ordinary
     bottom Flippers.
   - Existing cargo reconstruction behavior remains unchanged.
   - Repeated death/re-entry conserves unresolved obligations.

6. COMPLETION
   - Completion still waits for all existing original and cargo obligations.
   - The new Level 5 selection policy neither creates nor removes completion
     obligations.

7. REGRESSION ISOLATION
   - Level 4 ordinary Flipper selection remains unchanged.
   - Level 6 behavior remains unchanged in this candidate.
   - Level 7 remains unchanged and continues to prohibit ordinary bottom
     Flippers through its existing dedicated logic.

8. GAMEPLAY SYSTEMS
   - Tanker release behavior remains unchanged.
   - Spiker behavior remains unchanged.
   - Existing scoring, audio, destruction effects, Super Zapper behavior, and
     progression remain unchanged where directly affected tests can establish
     this economically.

Prefer deterministic state tests over brittle timing or rendering assertions.


VALIDATION

During implementation, run the narrowest affected tests first.

Before stopping, run an expanded focused regression covering at least:

- new Level 5 selection tests;
- Level 5 production/progression tests;
- Level configuration;
- Level 3 mixed-wave/ledger behavior where shared selector/ledger code was
  touched;
- Tankers and cargo;
- Flippers;
- Spikers;
- death/re-entry;
- completion;
- Super Zapper behavior if shared wave state was touched;
- Level 4 regressions;
- Level 6 regressions;
- Level 7 production/selection/re-entry regressions.

Then run:

- full solution tests;
- full solution build;
- git diff --check.

Inspect the final diff carefully.

Verify that screenshot000.png through screenshot013.png remain untouched and
untracked.

Verify UserData, recordings, screenshots, AI-Logs, and other generated/local
files are not staged or modified.


MANUAL ACCEPTANCE STATUS

Do not claim manual or arcade acceptance.

The candidate remains pending Neal's manual play test.

Do not launch the interactive game.

After automated validation, provide Neal the appropriate command to launch
Level 5 directly using the project's existing direct-level functionality.

Do not invent a new launch option if one already exists.


MANUAL ACCEPTANCE CHECKLIST

Prepare a short checklist for Neal emphasizing observation rather than
requiring exhaustive counting.

At minimum:

1. Start directly on Level 5.

2. Watch several ordinary bottom entries:
   - ordinary Flippers should not enter directly from the bottom;
   - Tankers should;
   - Spikers should.

3. Destroy or allow Tankers to release cargo:
   - cargo Flippers should still appear normally;
   - cargo Flippers should behave normally.

4. Confirm the distinction visually:
   - no solitary/original bottom-entry Flippers;
   - Flippers appearing from Tanker cargo are expected and correct.

5. Take at least one death/re-entry if practical and confirm ordinary bottom
   Flippers do not begin appearing after reconstruction.

6. Complete Level 5 if practical:
   - completion/progression should remain normal;
   - no apparent missing enemy obligations or stuck wave.

7. Observe whether the Level 5 pacing or clear-time impression changes.
   Do not treat this as a pass/fail requirement for this narrowly scoped
   correction.

Neal has separately observed that current C# levels can take substantially
longer to clear than the arcade game.

Do not assume this narrow Level 5 picker correction will explain or resolve
that observation.

The configured base Level 5 supply already matches the ROM at 20.

Any continuing clear-time difference should remain an observation for future
investigation rather than an excuse to broaden this implementation.


DOCUMENTATION

Update durable documentation appropriately for this implementation candidate:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

README.md only if genuinely needed.

Clearly distinguish:

- ROM-LISTING-BACKED Level 5 no-direct-Flipper selection;
- EXISTING PROJECT BEHAVIOR preserved;
- PROJECT TRANSLATION where applicable;
- MANUAL ACCEPTANCE PENDING.

Do not rewrite history suggesting previous Level 5 manual acceptance had
established this specific hidden selection rule. This is a newly identified
ROM/C# discrepancy.


GIT / STOP POINT

Do not stage, commit, or push.

Leave the complete candidate unstaged for Neal's manual acceptance.

Do not delete, rename, move, or stage unrelated/generated files.

Preserve the complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired date-prefixed implementation report under ignored
AI-Logs/Reports.

The report must include:

- starting Git state;
- ROM regions/tables rechecked;
- previous supply investigation reused;
- exact C# cause of ordinary Level 5 Flipper selection;
- implementation change;
- files changed;
- proof that Tanker cargo remains allowed;
- proof that supply remains 20;
- death/re-entry evidence;
- completion evidence;
- Level 4/6/7 regression isolation;
- focused validation results;
- full test result;
- build result;
- git diff --check result;
- final Git state;
- manual acceptance status;
- direct Level 5 launch command;
- manual acceptance checklist;
- unresolved issues.

STOP after automated validation and the manual-test handoff.

Do not begin Level 6 or any active-cap/release-pacing work.

TEMPEST_LEVEL5_NO_DIRECT_FLIPPER_IMPLEMENTATION_20261108