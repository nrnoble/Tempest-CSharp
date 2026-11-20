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

Resolve the four narrowly scoped C# accessibility/compile errors in the final
Level 5 cargo-specific post-death restoration candidate, then resume bounded
automated validation.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the uncommitted Level 5 work:

62c66e0ab19436ea162935627021154be21e0a41

The combined Level 5 concurrency and cargo-restoration candidate already
exists in the worktree.

Read these historical handoffs:

AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-post-death-restoration-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-cargo-restoration-release-validation-report.md

Do not redesign the restoration implementation.

Do not change gameplay behavior merely to make tests compile.

Do not launch the game.
Do not commit.
Do not push.


KNOWN COMPILE FAILURE

The authorized fresh Release route successfully compiled Tempest.Game.dll.

Compilation then failed in the test project before VSTest execution because
Level5ProductionTests.cs attempted to access four inaccessible production
members/types:

line 620:
Flipper.ReloadShotDelay is not accessible

line 622:
FlipperTankerCargoRestorationSnapshot is inaccessible

line 623:
Flipper.CaptureDeathRestorationSnapshot is not accessible

line 624:
Flipper.RestoreTankerCargo is not accessible

No tests executed.

This was a normal C# compilation failure, not a Windows Application Control
block and not a test assertion failure.


FIRST — VERIFY CURRENT STATE

Before modifying anything:

- verify branch/HEAD/upstream;
- verify index;
- inspect the existing candidate diff;
- verify the eight candidate paths recorded by the validation report;
- run git diff --check;
- inspect the exact declarations and call sites involved in all four compile
  errors.

Do not assume what their current access modifiers are. Read the source.


PURPOSE

Make the smallest maintainable change needed for the focused restoration tests
to verify the implemented behavior.

The preferred outcome is NOT automatically "make everything public."

First determine whether the tests are improperly reaching into implementation
details that can instead be verified through an existing accessible seam or
observable runtime state.

Choose the narrowest appropriate correction.


ACCESSIBILITY DECISION RULES

Use this order of preference:

1. If the same restoration behavior can be tested through an existing
   appropriately accessible production/test seam without weakening the
   regression, rewrite only the test setup/assertion to use that seam.

2. If these members genuinely constitute an intentional test seam already
   used by comparable project tests, expose only the minimum accessibility
   needed and follow the project's existing convention.

3. If the project already uses InternalsVisibleTo for Tempest.Game.Tests,
   prefer an appropriate internal test seam over making implementation
   details public.

4. Do not make fields, methods, snapshot types, or mutable state public merely
   for test convenience unless the project's existing architecture clearly
   establishes that as the intended API.

5. Do not use reflection, dynamic invocation, unsafe access, or other
   workarounds merely to bypass accessibility.

6. Do not weaken or delete the regression assertions simply to make the test
   compile.

7. Keep production encapsulation as strong as practical while preserving a
   meaningful deterministic test of the cargo-restoration behavior.


REQUIRED REGRESSION INTENT

Whatever compile correction is chosen, preserve the regression's substantive
purpose.

The tests must still be capable of establishing the relevant restoration
properties, including where applicable:

- same stable cargo identity;
- owner/ordinal conservation;
- unresolved materialized cargo is not reconstructed as a cargo Egg;
- restoration is in-tube rather than at deep-entry height zero;
- captured/restored movement state is preserved where intentionally part of
  the project translation;
- pending-zap information is preserved where applicable;
- restored cargo remains a completion obligation;
- latent/terminal/resolved cargo remains excluded from this path.

Do not retain an assertion merely because the original test happened to access
a private implementation member. Preserve behavioral coverage, not accidental
test structure.


NO GAMEPLAY REDESIGN

Do not alter:

- Level 5 ordinary Tanker/Spiker admission;
- zero ordinary Flipper entry;
- minima/maxima;
- shortest-Spike selection translation;
- terminal Spiker-to-Tanker conversion;
- converted-Tanker reentry;
- cargo identity ownership;
- restoration positioning policy;
- seven-place physical-headroom behavior;
- Blaster death/reentry timing;
- Super Zapper behavior;
- Tanker release behavior;
- scoring;
- persistent Spikes;
- rim/completion behavior;
- deterministic recording/playback;
- Levels 1-4 or 6-8.

If resolving the compile errors would require changing any of those behaviors,
STOP and report rather than broadening scope.


VALIDATION SEQUENCE

After making the smallest compile correction:

1. Run git diff --check.

2. Run the smallest focused Release command covering the three restoration
   regressions from the failed validation attempt:

   - Level3WaveLedgerTests.
     ExplicitDeathRestorationKeepsOnlySelectedUnresolvedCargoOutOfEggReserve

   - Level5ProductionTests.
     DeathRestoresUnresolvedLevelFiveCargoInTubeWithoutCargoEggs

   - Level5ProductionTests.
     CargoRestorationSnapshotPreservesPivotAndPendingZapState

   Use:
   --configuration Release
   --blame-hang-timeout 5min

3. Explicitly report:
   - production compilation;
   - test-project compilation;
   - whether VSTest actually executed tests;
   - exact test count;
   - exact result.

4. If those tests PASS, run the complete focused Level 5 cargo-restoration,
   Level 5 admission, Level 5 production, and relevant ledger test groups.

5. If focused validation passes, run affected Release regressions covering:
   - Tanker cargo;
   - death/reentry;
   - ledger/identity conservation;
   - terminal Spiker conversion;
   - converted-Tanker reentry;
   - pending cargo activation;
   - Super Zapper;
   - persistent Spikes;
   - completion/all-rim;
   - direct/campaign/recorded startup;
   - Levels 1-4 concurrency.

6. If affected validation passes, run full solution Release tests with the
   established VSTest hang timeout.

7. If full tests pass, run the full solution Release build.

Stop immediately on the first:

- FAIL;
- BLOCKED;
- TIMEOUT;
- CANCELED;
- meaningful stall.


WINDOWS SECURITY

The previous Debug artifact was blocked by Windows Application Control, but
the most recent Release attempt failed at normal C# test compilation before
VSTest.

Do not conflate those results.

If Windows Application Control blocks a generated Release artifact during
this validation:

- report BLOCKED;
- record the artifact/error;
- stop;
- do not retry an equivalent route;
- do not change security;
- do not use stale binaries.

Do not weaken or bypass Windows security.


TEST FAILURE HANDLING

If test code executes and reveals a genuine restoration defect:

- preserve the exact failure evidence;
- diagnose the smallest cause;
- correct it only if the proper behavior is unambiguous from the authorized
  restoration design;
- do not weaken valid assertions;
- do not broaden scope;
- rerun the directly failing test once after a justified correction.

If expected behavior is ambiguous, stop and report for Neal instead of
guessing.


FINAL STATIC / REPOSITORY CHECK

At completion:

- run git diff --check;
- inspect the complete candidate diff;
- verify exact changed/untracked candidate files;
- verify no files are staged;
- verify HEAD remains
  62c66e0ab19436ea162935627021154be21e0a41;
- verify AI-Logs remain ignored/uncommitted;
- verify no unrelated file changed;
- verify no Windows security configuration changed.


MANUAL HANDOFF

If all required automated validation passes, leave the complete Level 5
candidate uncommitted and provide Neal:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

Primary manual scenario:

1. Allow a Tanker to release Flippers.
2. Leave at least one cargo Flipper unresolved.
3. Allow the Blaster to die.
4. Reenter.
5. Verify no individual Flipper subsequently appears from the deep/bottom end.
6. Verify the restored Flipper presentation looks natural relative to the
   arcade.
7. Repeat after another death if practical.
8. Confirm ordinary bottom entry remains Tankers and Spikers only.
9. Confirm normal Tanker cargo release.
10. Confirm death/reentry, Spikes, Super Zapper, rim combat, and completion
    remain normal.

Do not claim manual acceptance before Neal reports it.


REPORT

Do not modify historical reports.

Create a new paired follow-up such as:

AI-Logs/Prompts/2026-11-09-level5-cargo-restoration-compile-fix-validation-prompt.md

AI-Logs/Reports/2026-11-09-level5-cargo-restoration-compile-fix-validation-report.md

Record:

- starting candidate/repository state;
- four original compile errors;
- actual accessibility cause;
- selected minimal correction and why;
- files changed by the correction;
- production/test compilation;
- focused test results;
- affected/full test results;
- build result;
- any Windows block;
- final static validation;
- final repository state;
- exact manual handoff;
- manual acceptance status.


GIT AND STOP

Do not commit.
Do not push.

Even if everything passes, stop with the candidate uncommitted for Neal's
manual gameplay acceptance.

Do not begin Level 6 concurrency.
Do not perform unrelated cleanup.


TEMPEST_LEVEL5_CARGO_RESTORATION_COMPILE_FIX_VALIDATION_COMPLETE
