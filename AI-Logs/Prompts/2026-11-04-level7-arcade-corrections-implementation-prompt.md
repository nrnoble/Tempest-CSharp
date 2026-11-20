FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain that the task may be incomplete or truncated.


TEMPEST LEVEL 7 ARCADE-FIDELITY CORRECTION IMPLEMENTATION

Implement the bounded Level 7 corrections established by Neal's direct arcade observations and the completed ROM investigations.

This is an IMPLEMENTATION task.

Do not broaden the milestone beyond the explicitly authorized Level 7 corrections below.

Do not modify accepted Levels 1-6 behavior.

Do not implement Level 8+.

Do not launch the interactive game for manual testing. Neal will do the final gameplay acceptance after automated validation.

Do not commit or push. Stop after implementation and automated validation so Neal can manually test the result.


AUTHORITATIVE EVIDENCE

Read these before making implementation decisions:

1. TEMPEST-DEVELOPMENT-NOTES.md
2. TEMPEST-BEHAVIOR-SPEC.md
3. AI-Logs/Reports/2026-11-04-level7-reentry-rom-investigation-report.md
4. AI-Logs/Reports/2026-11-04-level7-flipper-rom-analysis-report.md
5. The existing Level 7 implementation and its tests.
6. Reference/arcade/Tempest Rom Source code.md where needed to verify the cited ROM behavior.

Use evidence in this order:

1. Neal's direct arcade observations.
2. Atari ROM behavior/data.
3. Existing accepted project behavior where it does not conflict with 1-2.
4. Incitti only as an engineering/reference comparison.

When Incitti or existing C# disagrees with established arcade/ROM behavior for this milestone, arcade/ROM behavior wins.

Do not reinterpret accepted Levels 1-6 from Level 7 findings.


CURRENT PROJECT STATE / SCOPE

Levels 1-6 are the accepted production campaign foundation and must remain behaviorally frozen for this milestone.

Level 7 already exists as an uncommitted implementation and has been manually played successfully, but newly established arcade/ROM findings require specific corrections before Level 7 can be considered finally accepted.

Preserve existing Level 7 behavior that is not directly affected by the corrections authorized below.

Do not perform unrelated cleanup/refactoring.

Before editing, inspect git status and the current diff carefully.

The repository may contain the existing uncommitted Level 7 implementation. Do NOT discard, reset, overwrite, or revert those changes.

Work from the existing Level 7 implementation.

If the worktree contains unrelated unexpected changes that cannot be safely distinguished from the existing Level 7 work, STOP and report the problem.


ARCADE-VERIFIED LEVEL 7 FINDINGS

Treat these as established direct arcade observations from Neal:

1. Ordinary individual Flippers do NOT enter Level 7 directly from the bottom.

2. Tankers enter from the bottom and release Flippers.

3. After player death, even when one visible Flipper remained, that Flipper did not re-enter individually from the bottom. A Tanker subsequently appeared instead.

4. The visible Level 7 Flipper below-rim lane-change pattern is:

   2 - 3 - 2 - 2 - 2

This new observation supersedes the project's previous interpretation of the Level 7 route as a repeating 2/3 program.

Do not change accepted rim pursuit behavior unless directly required by these corrections.


ROM-BACKED LEVEL 7 SUPPLY / RE-ENTRY FINDINGS

The completed ROM investigation established:

- Level 7 Flipper minimum = 0.
- Ordinary Flipper bottom materialization exists in the ROM generally but is unreachable through the traced normal Level 7 generic picker because its selection paths require a nonzero species minimum.
- Level 7 Tanker minimum = 1 and maximum = 2.
- Level 7 Spiker minimum = 2 and maximum = 4.
- Level 7 Tankers carry Flippers.
- Player death combines unresolved active-below-rim, at-rim and already-pending quantities into generic pending supply.
- Previous species identity and per-enemy movement identity are not reconstructed.
- A Tanker consumes one generic pending wave unit.
- Its two Flipper children are created separately and do not consume two additional pending wave units.
- For a literal one-unit Level 7 reconstruction, the generic entry is initially selected as a Spiker.
- When pending supply reaches zero, the ROM contains a path that can convert that Spiker in place into a Flipper-carrying Tanker at the bottom.
- No odd-count or R==1 special-case branch was found.
- The current C# Level 7 differs because it can select ordinary bottom-entry Flippers and preserves unresolved original species through death.

Use the detailed report and ROM addresses it cites as the implementation authority rather than reconstructing these rules from this summary alone.


AUTHORIZED IMPLEMENTATION TARGETS

Implement exactly these six targets.


1. CORRECT LEVEL 7 FLIPPER MOVEMENT TO THE ARCADE-OBSERVED 2-3-2-2-2 PATTERN

The existing Level 7 implementation was based on the earlier ROM interpretation of a repeating two/three movement program.

Neal has now directly observed the arcade behavior sufficiently clearly to establish the visible Level 7 sequence as:

2 - 3 - 2 - 2 - 2

Correct Level 7 ordinary/cargo Flipper routing as appropriate so the resulting visible Level 7 behavior matches this arcade-observed sequence.

First inspect how the current Level 7 implementation distinguishes ordinary Flippers and Tanker cargo.

Do not assume blindly that changing one shared counter is correct.

Preserve the already accepted Level 7 geometry, inward directed pivots, full long reflex sweeps, rim handoff and existing gameplay mechanics unless this route correction directly requires adjustment.

Implement the smallest Level-7-specific movement correction needed.

Do not change Levels 1-6 movement.

Document in maintainability-focused comments that the 2-3-2-2-2 visible sequence is an arcade-observed Level 7 requirement where appropriate. Do not over-comment obvious code.


2. ELIMINATE ORDINARY BOTTOM-ENTRY FLIPPERS ON LEVEL 7

The ROM investigation established that Level 7 ordinary bottom-entry Flippers are unreachable in the normal picker.

Modify Level 7 generation so ordinary/original individual Flippers are never selected as bottom entrants.

This does NOT mean removing Flippers from Level 7.

Flippers remain present as Tanker cargo.

Preserve the distinction between:

- ordinary bottom-entry enemy selection;
- Tanker-carried Flippers;
- already active/rim Flippers;
- death/re-entry supply.

Do not implement this by globally disabling the Flipper type, because Tanker cargo still requires Flippers.


3. CORRECT LEVEL 7 DEATH/RE-ENTRY ACCOUNTING

Current C# reconstruction preserves selected unresolved original species identities.

For Level 7, that differs from Atari.

Implement a Level-7-specific reconstruction policy in which unresolved Level 7 wave quantity survives death but prior unresolved original species selection does not.

After death, the unresolved Level 7 quantity must be reconstructed through the Level 7 selection policy rather than recreating an unresolved original specifically as its previous Flipper/Tanker/Spiker species.

Preserve all unrelated accepted death behavior:

- score;
- remaining lives semantics;
- Super Zapper lifecycle;
- damaged Spike persistence;
- resolved enemy state where appropriate;
- player death/re-entry presentation;
- other accepted Level 1-6 behavior.

Do not globally redesign or remove the existing identity ledger.

If the existing ledger must contain bookkeeping identities for C# invariants, adapt Level 7 narrowly so species selection is cleared/reselected while required accounting integrity remains intact.


4. IMPLEMENT LEVEL 7 ROM-SHAPED TANKER/SPIKER SELECTION

Implement the smallest Level-7-specific selection policy required to reproduce the established Atari behavior.

Required Level 7 constraints include:

- ordinary Flipper bottom selection: unavailable;
- Tanker minimum/maximum: 1/2;
- Spiker minimum/maximum: 2/4;
- maximum active-enemy behavior as established by the report/ROM;
- Tankers carry Flippers;
- Tanker consumes one pending wave unit;
- Tanker cargo does not consume two additional pending wave units.

Do not turn this into a generalized recreation of Atari's enemy-selection VM for all levels.

Implement only the policy required by Level 7.


FINAL-PENDING SPIKER-TO-TANKER BEHAVIOR

Implement the ROM-backed final-pending behavior for Level 7:

When the applicable Spiker reaches the established conversion condition with no generic pending wave supply remaining, it can be converted in place into a Flipper-carrying Tanker and returned to the bottom according to the traced ROM behavior.

This is a same-unit conversion.

It is NOT:

- two remaining Flippers becoming one Tanker;
- creation of an extra pending unit;
- an ordinary Flipper hatch;
- an additional decrement of pending supply.

Preserve correct accounting through this transformation.

Use the ROM investigation report and cited source to determine the narrowest faithful C# mapping.

If exact ROM coordinate/timing values cannot be mapped literally onto existing C# units, preserve the accepted C# timing/coordinate model and clearly label any necessary mapping as a project approximation rather than inventing ROM precision.


5. KEEP LEVELS 1-6 COMPLETELY FROZEN

Absolutely no intentional gameplay changes to Levels 1-6 in this milestone.

Do not apply Level 7's new picker, reconstruction rules, movement sequence or Spiker-to-Tanker transformation globally.

Level 1 is currently considered unlikely to require additional work.

Levels 2-6 MAY receive separate ROM/arcade-fidelity investigations in the future similar to the Level 7 investigation.

That future possibility does NOT authorize any Level 2-6 change now.

If shared code must be extended, preserve the exact existing Level 1-6 behavior through explicit opt-in/configuration and regression tests.


6. PRESERVE ALREADY WORKING LEVEL 7 FEATURES

Unless one of the authorized corrections above directly requires a change, preserve the existing Level 7 implementation, including:

- closed 16-lane clover geometry;
- existing accepted Level 7 rendering;
- 16 starting Spikes;
- established Avoid Spikes threshold behavior;
- existing Level 7 firing behavior;
- Tanker and Spiker behavior not superseded by this milestone;
- natural Level 6 -> Level 7 progression;
- existing between-level approach behavior;
- score/lives carry behavior;
- fresh Super Zapper recharge behavior;
- --level7-test;
- current Level 7 terminal campaign boundary;
- accepted rim pursuit;
- existing audio behavior;
- replay infrastructure.

Do not expand the campaign to Level 8.


IMPLEMENTATION DESIGN REQUIREMENTS

Prefer small explicit Level-7 opt-in policies over new global behavioral assumptions.

Keep accepted generic/shared paths unchanged wherever practical.

Do not build a generalized Atari p-code VM.

Do not rewrite the existing Level 3+ identity ledger globally.

Do not perform broad architectural refactoring.

Use maintainability-focused comments where the reason for Level 7's special behavior would otherwise be unclear.

Comments should explain WHY the Level 7 exception exists and distinguish:

- arcade-observed behavior;
- ROM-backed behavior;
- C# project approximations.

Do not fill the code with comments explaining obvious statements.


AUTOMATED TEST REQUIREMENTS

Add or update focused tests that prove the corrected Level 7 behavior.

At minimum test:


A. NO ORDINARY BOTTOM FLIPPERS

Across sufficient deterministic selection scenarios/seeds to exercise the Level 7 selection path:

- no original ordinary Flipper is selected as a bottom entrant;
- Tankers and Spikers remain selectable as appropriate;
- Tanker cargo is still Flipper cargo.


B. LEVEL 7 MOVEMENT

Test the corrected arcade-observed:

2 - 3 - 2 - 2 - 2

sequence across enough completed lane changes to prove all boundaries and reversals represented by the implementation.

Include wrap behavior where applicable.

Verify independent Flippers do not share route state.

Verify Level 1-6 movement tests remain unchanged/passing.


C. ONE-UNIT DEATH RECONSTRUCTION

Construct an appropriate deterministic Level 7 state corresponding to one unresolved generic wave unit.

After death/reconstruction verify:

- previous species identity is not restored;
- one generic unit remains;
- Level 7 selection produces the ROM-backed Spiker result;
- no bottom-entry ordinary Flipper is created.


D. FINAL-PENDING SPIKER CONVERSION

Verify that once the relevant final pending supply reaches zero:

- the applicable Spiker can perform the ROM-backed conversion;
- it becomes a Flipper-carrying Tanker;
- the transformation preserves the same logical unit rather than adding wave supply;
- pending supply is not decremented twice;
- it returns to the appropriate Level 7 bottom position/path;
- accounting remains internally consistent.


E. TANKER ACCOUNTING

Verify:

- normally selected Tanker consumes one wave/pending unit;
- two Flipper cargo children do not consume two additional pending wave units;
- cargo release remains functional;
- children remain distinct active cargo entities after release as required by the existing game model.


F. ORDINARY MULTI-UNIT SELECTION

Verify initial/re-entry selection respects the Level 7 Tanker and Spiker constraints derived from the ROM investigation.

Do not write brittle tests for random details not established by ROM evidence.

Test deterministic decision boundaries/invariants rather than fabricated exact random wave compositions.


G. DEATH/RE-ENTRY REGRESSION

Verify Level 7 preserves:

- generic unresolved quantity;
- resolved-state integrity;
- score;
- lives behavior;
- remaining Zapper state;
- damaged Spikes;
- required cargo/accounting invariants.

Verify stale species selection and stale movement route state do not survive improperly.


H. LEVELS 1-6 FREEZE

Run the existing focused movement, wave-selection, Tanker, death/re-entry, progression, firing, Spike, exit and campaign regression tests relevant to the touched shared seams.

If a shared seam changes, add regression coverage proving Levels 1-6 retain their previous behavior.


I. LEVEL 7 EXISTING FEATURES

Retain existing Level 7 tests for:

- exact clover topology;
- progression;
- firing;
- Spikes;
- exit;
- direct test mode;
- terminal boundary;
- Tanker cargo;
- Spiker behavior;
- campaign soak where applicable.

Update tests only where their previous Level 7 expectation has now been superseded by the newly established arcade/ROM evidence.


VALIDATION

After implementation, run focused tests covering all touched systems.

Then run the complete solution test suite.

Then run:

dotnet build TempestCSharp.slnx

and:

git diff --check

Inspect:

git status --short --branch

git diff --stat

git diff

Do not treat a command that reports zero discovered tests as a successful test run.

Record the actual nonzero passing test counts.

If automated tests or build fail, investigate and correct defects that are within this milestone.

Do not hide failures by weakening valid regression tests.

If correcting a failure would require changing accepted Levels 1-6 behavior or expanding scope materially, STOP and report the conflict rather than making the change.


MANUAL ACCEPTANCE

DO NOT launch the interactive game.

After every automated validation gate passes, stop and provide Neal with the appropriate Level 7 launch command.

The expected direct manual-test command should remain equivalent to:

dotnet run --project src\Tempest.Game -- --level7-test --lives 10

unless inspection shows the existing command syntax is different.

Neal will perform final manual gameplay acceptance.

Do not claim that Level 7 is finally accepted before Neal reports the manual result.


MANUAL ACCEPTANCE CHECKLIST FOR NEAL

At completion, provide a concise checklist covering:

1. No individual Flippers appear directly from the bottom.
2. Tankers appear from the bottom and release Flippers normally.
3. Follow Flippers below the rim and verify the visible 2-3-2-2-2 lane-change behavior.
4. Lose a life with Flippers/Tankers/Spikers in different states and verify individual previous species are not obviously reconstructed from the bottom.
5. If practical, reproduce the one-visible-Flipper death scenario and verify that the remaining Flipper does not return individually.
6. Watch late-wave Spiker/Tanker behavior for a sensible final-pending conversion.
7. Verify Tanker-released Flippers behave normally.
8. Verify Spikers, Spikes, firing, Super Zapper, audio and player controls remain normal.
9. Verify Level 7 completion/terminal behavior remains correct.
10. Note any obvious regression or behavior inconsistent with the arcade.

Do not require Neal to infer hidden internal counters from visual gameplay.


DOCUMENTATION / AI LOGS

Follow the repository's established AI logging instructions.

Save the substantive user prompt under AI-Logs/Prompts/ with a descriptive date-prefixed filename.

Save the corresponding implementation report under AI-Logs/Reports/ with a matching descriptive date-prefixed filename.

The implementation report should include:

- starting repository state;
- files changed;
- exact Level 7 behavior implemented;
- arcade-observed versus ROM-backed versus project-approximation distinctions;
- movement implementation;
- selection implementation;
- death/re-entry implementation;
- Spiker-to-Tanker implementation;
- Tanker cargo/accounting handling;
- tests added/updated;
- focused test results with nonzero counts;
- complete test-suite result with nonzero count;
- build result;
- git diff --check result;
- final git status;
- unresolved issues;
- manual acceptance status explicitly marked PENDING.


SPECIFICATION / DEVELOPMENT NOTES

Because this milestone incorporates newly established arcade and ROM evidence, update TEMPEST-BEHAVIOR-SPEC.md and TEMPEST-DEVELOPMENT-NOTES.md only as necessary to accurately record the new Level 7 behavior.

Preserve evidence classifications.

Clearly distinguish:

- ARCADE VERIFIED: Neal's observed no-individual-bottom-Flipper behavior and visible 2-3-2-2-2 movement sequence;
- ROM-BACKED: Level 7 picker/reconstruction/Tanker/Spiker accounting behavior established by the ROM investigation;
- PROJECT DECISION / APPROXIMATION: any necessary C# mapping not literally established by the ROM.

Do not mark the corrected Level 7 implementation as gameplay accepted until Neal completes the manual acceptance pass.


GIT BOUNDARY

Do not commit.
Do not push.
Do not reset or discard the existing Level 7 uncommitted implementation.

At completion, leave the corrected Level 7 implementation and documentation changes available for Neal's review and manual testing.

After Neal manually accepts the corrected Level 7 behavior, commit/closeout can be handled as a separate step.


FINAL RESPONSE

Return:

1. Concise implementation summary.
2. Files changed.
3. Important behavioral decisions.
4. Focused automated test result and actual passing count.
5. Full-suite result and actual passing count.
6. Build warnings/errors.
7. git diff --check result.
8. Final git status summary.
9. Any remaining unresolved issues.
10. Explicit statement that Levels 1-6 were not intentionally changed.
11. Explicit statement that Level 7 manual acceptance remains PENDING.
12. The Level 7 launch command.
13. The concise manual acceptance checklist.
14. Implementation-report filename.
15. Confirmation that no commit or push occurred.

Stop there and await Neal's manual test results.

TEMPEST_LEVEL7_ARCADE_CORRECTIONS_IMPLEMENTATION_COMPLETE