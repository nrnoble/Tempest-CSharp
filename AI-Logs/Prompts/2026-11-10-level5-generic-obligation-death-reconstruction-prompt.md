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

Replace the rejected Level 5 cargo-position restoration experiment with a
ROM-informed generic-obligation reconstruction candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath all current uncommitted Level 5 work:

62c66e0ab19436ea162935627021154be21e0a41

The validated Level 5 admission-and-terminal-Spiker candidate and the later
rejected cargo-position restoration experiment are both currently present in
the worktree.

Do not restart the Level 5 concurrency implementation.

Read these historical handoffs:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md

AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-post-death-restoration-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-restoration-release-validation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-restoration-compile-fix-validation-report.md

Preserve those reports as historical records.

Do not rewrite them to reflect the later arcade findings.


NEW DIRECT ARCADE EVIDENCE

Neal tested original arcade Level 5 directly and repeatedly.

Treat these findings as ARCADE-OBSERVED:

1. Under no circumstance does an individual Flipper enter independently from
   the deep/bottom end on Level 5.

2. This remains true after Blaster death and reentry.

3. Flippers that were active before death do not reappear as Flippers on the
   rim when gameplay resumes.

4. Unresolved enemy quantity is conserved across death.

5. Four unresolved live enemies at death become four unresolved enemy
   obligations after reconstruction.

6. If four Tankers are destroyed, producing eight cargo Flippers, and all eight
   Flippers remain unresolved when the Blaster dies, the resumed wave contains
   eight unresolved enemy obligations.

7. Those obligations do not have predetermined post-death species identities.

8. On Level 5, reconstructed generic obligations subsequently become Tankers
   or Spikers through the normal Level 5 selection machinery, never Flippers.

The eight-obligation case is rare in normal three-life gameplay, but Neal
successfully reproduced and verified it on the arcade machine.

Do not reinterpret eight cargo Flippers as four former carriers.

Eight unresolved cargo Flippers mean eight unresolved post-death obligations.


ROM-BACKED MODEL

The prior ROM investigation established:

- active enemies are returned to generic pending supply during death
  reconstruction;
- Level 5 ordinary selectable species are Tanker and Spiker;
- Flipper minimum is zero;
- positive calculated Flipper availability does not make Flipper ordinarily
  selectable;
- recreated species are dynamically selected rather than predetermined;
- Tanker and Spiker minima/maxima and shared physical headroom govern later
  creation.

Use those findings with Neal's arcade evidence.

The intended model is now:

unresolved active enemy at death
    ->
one generic unresolved Level 5 obligation
    ->
normal Level 5 Tanker/Spiker selection when admitted again

Do not preserve the pre-death Flipper species across reconstruction.

Do not restore the pre-death Flipper position.

Do not create a post-death Flipper Egg.

Do not collapse two Flipper obligations back into one Tanker obligation.


STATUS OF THE CURRENT EXPERIMENT

The current cargo-position restoration experiment is manually rejected.

It captures unresolved cargo Flipper lane/height/movement state and restores
the same Flipper identity in the tube after death.

Manual testing showed that this can make Flippers reappear on the rim when
gameplay resumes.

That is not arcade behavior.

Remove or supersede the rejected snapshot-position restoration design.

Do not continue tuning its placement.

Do not preserve snapshot machinery merely because its tests pass.

Its automated validation remains useful historical evidence, but it validates
a rejected project translation.


PRESERVE THE VALIDATED LEVEL 5 ADMISSION CANDIDATE

Keep the validated Level 5 concurrency work unless a tiny direct dependency
requires adjustment:

- ordinary Tanker/Spiker selection only;
- no ordinary Flipper selection;
- F minimum/maximum 0/5;
- T minimum/maximum 1/1;
- S minimum/maximum 2/3;
- Spiker-first unmet-minimum handling;
- shortest-Spike post-minimum project translation;
- seven-place ordinary physical headroom;
- latent-cargo availability reservation;
- blocked-Egg waiting without repeated randomness;
- terminal Spiker-to-Tanker conversion;
- converted-Tanker reentry;
- complete-pair cargo behavior during normal live play.

Do not reopen those rules merely because death reconstruction now feeds
additional generic obligations into the same selector.


PURPOSE

Implement the smallest safe Level-5-specific translation of the ROM death
reconstruction model within the C# project's ledger architecture.

The implementation must conserve unresolved quantity while discarding
pre-death enemy species for reconstructed active enemies.

For each unresolved active enemy at death:

- preserve exactly one unresolved obligation;
- return that obligation to a generic pending/reconstruction state;
- clear its prior runtime species commitment for post-death reconstruction;
- later route it through the normal Level 5 Tanker/Spiker selector;
- never recreate it as an ordinary or retained Flipper.

This applies to unresolved active Tankers, Spikers, and released cargo
Flippers as required to reproduce the generic reset model.

Do not apply it to terminal/resolved enemies.

Do not count latent cargo as a separate active enemy obligation because latent
cargo was not an active enemy.

Do not count bullets, persistent Spikes, or visual effects as enemy
obligations.


ARCHITECTURE / IDENTITY TRANSLATION

The C# project uses stable original and cargo identities for accounting, while
the ROM uses generic reconstructed pending supply.

Implement the narrowest maintainable project translation.

The externally observable and accounting requirements are mandatory:

- one unresolved active enemy at death becomes exactly one generic obligation;
- no obligation is lost;
- no obligation is duplicated;
- cargo siblings remain separate obligations after materialization;
- previous Flipper species is cleared;
- reconstructed obligations can become only Tankers or Spikers on Level 5;
- completion remains blocked until every reconstructed obligation resolves;
- repeated deaths conserve the then-current unresolved obligation count.

The internal ID used to track an obligation may remain stable if that helps
ledger conservation, but its old cargo-Flipper identity must not force:

- Flipper species retention;
- cargo Egg presentation;
- rim/in-tube Flipper restoration;
- restoration through CreateFreshTankerCargo.

Do not preserve owner/ordinal semantics when those semantics would incorrectly
force the obligation to remain cargo after death.

If a genericized former cargo obligation becomes a Tanker, it must participate
in the normal Tanker lifecycle appropriate to a post-death generic Level 5
enemy.

Do not invent recursive or duplicate cargo identities.

Before implementing, inspect the ledger architecture and choose the smallest
safe representation for a generic reconstructed obligation.

Possible internal approaches include:

- a dedicated generic reconstruction-obligation identity/state;
- conversion of materialized cargo identities into generic pending identities;
- another narrowly justified representation.

Do not choose an approach merely from this list.

Use the existing architecture and invariants to determine the safest mapping.

If the current fixed original/cargo namespace cannot represent this behavior
without broad redesign or identity collision, STOP and report the precise
architectural conflict instead of forcing a fragile solution.


OBLIGATION ACCOUNTING EXAMPLES

Add explicit deterministic coverage for these required cases.

CASE A — FOUR ACTIVE FLIPPERS

Before death:

- four materialized active Flippers;
- all four unresolved.

After reconstruction:

- exactly four generic unresolved obligations;
- zero retained Flipper runtime objects;
- zero cargo Flipper Eggs;
- zero Flippers restored on the rim or in the tube.

As the wave resumes:

- those four obligations are processed under Level 5 Tanker/Spiker selection;
- no reconstructed obligation becomes a Flipper;
- the four obligations remain independently resolvable.


CASE B — FOUR ACTIVE ORDINARY ENEMIES

Before death:

- any four unresolved active Level 5 enemies.

After reconstruction:

- exactly four generic unresolved obligations attributable to those active
  enemies;
- no loss or duplication;
- no requirement to preserve their prior species.


CASE C — EIGHT MATERIALIZED CARGO FLIPPERS

Construct a deterministic development fixture representing:

- four Tanker owners already terminal through cargo release/destruction;
- eight materialized cargo Flippers;
- all eight cargo Flippers unresolved at death.

After reconstruction:

- exactly eight generic unresolved obligations;
- not four Tanker obligations;
- no cargo Flipper Eggs;
- no restored Flippers;
- no rim Flippers;
- no retained cargo species commitment.

During resumed play:

- all eight obligations are eventually admitted as dynamic Level 5 Tankers or
  Spikers subject to minima, maxima, physical headroom, and ordinary waiting;
- all eight remain completion obligations until legitimately resolved;
- new Tankers may create their normal subsequent cargo according to the
  accepted live Tanker lifecycle.


CASE D — MIXED TERMINAL/UNRESOLVED CARGO

If one sibling is already terminal and the other is unresolved:

- only the unresolved child contributes one generic obligation;
- the terminal child remains terminal;
- no pair is recreated merely because the former owner once carried two.


CASE E — LATENT CARGO

If a Tanker remains unresolved with an unreleased latent pair at death:

- determine the correct one-obligation reconstruction from the ROM generic
  active-enemy model;
- do not count the two latent children as two additional active obligations;
- do not produce individual latent-cargo Eggs.

The active Tanker itself contributes one generic unresolved obligation.


CASE F — REPEATED DEATH

After reconstructed obligations have reentered as Tankers/Spikers, trigger
another death.

Verify that:

- the currently unresolved active enemy count is conserved again;
- prior historical cargo ancestry does not revive;
- no Flipper species survives merely because an obligation originated as cargo;
- no count grows or shrinks without a legitimate gameplay event.


REMOVE THE REJECTED SNAPSHOT RESTORATION

Identify every production/test change belonging specifically to the rejected
captured-position restoration experiment.

Remove or replace as appropriate:

- Flipper cargo death-restoration snapshot type and methods;
- captured lane/height/pivot restoration logic;
- restoration-reserve state used only for preserved Flipper restoration;
- queued in-tube Flipper restoration;
- physical-headroom retry logic used only by that rejected restoration;
- all-rim guards specific to waiting Flipper snapshots;
- tests that require the same Flipper to return in-tube;
- the `InternalsVisibleTo` friend-assembly file if it is no longer required
  after those tests and internal seams are removed.

Do not remove shared code still required by the validated admission candidate
or another accepted project system.

Review each removal against the complete diff rather than deleting files
blindly.


TANKER CARGO AFTER RECONSTRUCTION

A generic reconstructed obligation that later becomes a Tanker may receive
normal new latent cargo under the Level 5 Tanker lifecycle.

This is not restoration of the former cargo identity.

It is a new species selection and subsequent Tanker lifecycle for a conserved
generic obligation.

Ensure that:

- cargo pair allocation remains deterministic and collision-free;
- no former cargo pair is duplicated;
- no old terminal owner is resurrected;
- new Tanker cargo creates the correct number of additional unresolved
  obligations when materialized;
- completion accounting remains exact.


PENDING SHOT / ZAPPER STATES

At the death boundary, determine which runtime objects count as unresolved
active enemies under existing ordering.

For cargo already materialized into an unresolved Flipper representation,
preserve one generic obligation.

Do not preserve Flipper species merely because it was:

- pending shot activation;
- pending zap completion;
- in a transition state;
- at the rim.

Terminal objects remain terminal.

Do not redesign Super Zapper timing, targeting, or scoring.


LEVEL 5 ONLY

Apply this reconstruction policy only to canonical production Level 5 and its
equivalent direct/campaign/recorded startup paths.

Do not alter death reconstruction for Levels 1-4 or 6-8.

Do not turn this into a universal ROM reconstruction conversion.

Copied/synthetic configurations remain unaffected unless a focused fixture
explicitly invokes the canonical Level 5 policy.


PRESERVE OTHER SYSTEMS

Do not redesign or retune:

- Level 5 ordinary admission;
- terminal Spiker conversion;
- converted-Tanker live behavior;
- live Tanker cargo release;
- Level 5 geometry;
- Flipper movement during normal live play;
- Spiker/Spike behavior;
- firing;
- scoring;
- Super Zapper;
- player death timing;
- persistent Spikes;
- rim combat;
- all-rim rules;
- level completion/progression;
- deterministic recording/playback;
- Levels 1-4;
- Levels 6-8.


TESTS

Replace tests for the rejected in-tube Flipper restoration with tests of
generic obligation reconstruction.

At minimum verify:

1. No post-death Flipper presentation
   - no restored cargo Flipper runtime object;
   - no cargo Flipper Egg;
   - no Flipper at height zero;
   - no Flipper restored at captured lane/height;
   - no Flipper reappearing on the rim.

2. Quantity conservation
   - one unresolved active enemy becomes one generic obligation;
   - four unresolved Flippers become four obligations;
   - eight unresolved cargo Flippers become eight obligations;
   - mixed terminal/unresolved siblings count exactly;
   - latent cargo does not add active obligations;
   - repeated death preserves the current unresolved count.

3. Species reset
   - genericized cargo does not retain Flipper species;
   - reconstructed obligations use Tanker/Spiker selection only;
   - no ordinary or retained Level 5 path selects Flipper.

4. Ledger invariants
   - no duplicate IDs;
   - no missing obligations;
   - no stale owner/ordinal relationships forcing cargo behavior;
   - terminal identities remain terminal;
   - completion waits for every generic obligation.

5. Tanker lifecycle after selection
   - a generic obligation selected as Tanker receives one valid cargo pair;
   - no historical cargo pair is reused or duplicated;
   - released new cargo behaves under the normal live Tanker system.

6. Existing Level 5 behavior
   - minima/maxima remain correct;
   - seven-place headroom remains correct;
   - terminal Spiker conversion remains correct;
   - normal no-death Tanker release remains correct;
   - Super Zapper and persistent Spike behavior remain correct.

7. Isolation
   - Levels 1-4 unchanged;
   - Levels 6-8 unchanged;
   - copied/noncanonical configurations unchanged;
   - direct/campaign/recorded canonical Level 5 share the intended policy.


VALIDATION

Use the repository-wide bounded validation and security rules.

Prefer Release configuration because it most recently completed the complete
suite successfully.

Begin with the smallest new generic-obligation reconstruction tests.

Then run focused Level 5 admission, production, ledger, cargo, and
death/reconstruction groups.

If those pass, run affected Release regressions covering:

- Tanker cargo;
- identity/accounting;
- death/reentry;
- terminal Spiker conversion;
- Super Zapper;
- persistent Spikes;
- completion/all-rim;
- direct/campaign/recorded startup;
- Levels 1-4 concurrency.

If affected validation passes, run:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

Then:

dotnet build TempestCSharp.slnx --configuration Release

Also run:

- git diff --check;
- complete tracked-diff review;
- final repository-state verification.

Stop on the first FAIL, BLOCKED, TIMEOUT, CANCELED, or meaningful stall.

If Windows Application Control blocks an artifact, do not retry an equivalent
route or weaken security.


MANUAL HANDOFF

If automated validation succeeds, leave the entire Level 5 candidate
uncommitted.

Do not launch the game.

Provide Neal:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

Primary manual acceptance:

1. Allow one or more Tankers to release Flippers.
2. Leave released Flippers unresolved.
3. Let the Blaster die.
4. Reenter.
5. Verify no Flippers enter independently from the bottom.
6. Verify no Flippers reappear on the rim or elsewhere when play resumes.
7. Verify the corresponding unresolved quantity returns through Tanker/Spiker
   obligations.
8. Test four unresolved enemies where practical.
9. The rare eight-obligation case is desirable but is not required for routine
   manual acceptance if automated coverage establishes it.
10. Verify ordinary Level 5 Tanker/Spiker admission, cargo release, Spikes,
    Super Zapper, rim combat, death/reentry, and completion remain normal.

Do not claim manual acceptance until Neal reports the result.


DOCUMENTATION AND REPORT

This remains an implementation candidate.

Do not update durable documentation to claim Level 5 acceptance.

Preserve every prior report as historical evidence, including the rejected
captured-position restoration reports.

Create a new paired record such as:

AI-Logs/Prompts/2026-11-09-level5-generic-obligation-death-reconstruction-prompt.md

AI-Logs/Reports/2026-11-09-level5-generic-obligation-death-reconstruction-report.md

Record:

- new arcade evidence;
- ROM-backed reconstruction model;
- rejected snapshot-restoration code removed;
- chosen ledger representation;
- quantity-conservation behavior;
- four- and eight-obligation cases;
- species-reset behavior;
- tests replaced/added;
- focused/affected/full validation;
- build result;
- final diff/repository state;
- exact manual handoff;
- remaining fidelity boundaries.


GIT AND STOP

Do not commit.
Do not push.

Stop after automated validation and manual handoff.

Do not begin Level 6 concurrency.
Do not perform unrelated cleanup.


TEMPEST_LEVEL5_GENERIC_OBLIGATION_RECONSTRUCTION_COMPLETE