# Task: Determine and resolve the single failing Level 11 terminal-Spiker regression test

Continue from the current uncommitted Level 12 candidate after restoration of the rejected Level 12 ExitZoom camera. The restoration itself passed focused validation, but the complete solution suite reported one failure:

`Level11ProductionCorrectionTests.LevelElevenTerminalSpikerConvertsInPlaceWithTwoCargoAndNoPendingLoss`

The reported failure was at `Level11ProductionCorrectionTests.cs:85`: `Assert.DoesNotContain` matched an existing Spiker. Do not assume this is a production gameplay regression or merely a stale test. Determine which it is.

## Objective

Answer and resolve why the Level 11 terminal-Spiker conversion regression test finds a Spiker when its assertion expects none. Classify the result as exactly one of:

1. REAL PRODUCTION REGRESSION
2. STALE OR OVERBROAD TEST ASSERTION
3. INVALID / NON-DETERMINISTIC TEST SETUP
4. SHARED-BEHAVIOR CHANGE REQUIRING TEST UPDATE
5. OTHER — explain precisely

Apply the smallest justified correction; do not broaden the task.

## Preserve accepted Level 11 behavior

Level 11 was manually accepted and committed before the current Level 12 candidate. Preserve terminal Spiker-to-Tanker conversion at the qualifying terminal boundary when generic pending original supply is zero; preserve the converting original's identity as the Tanker; attach two distinct latent cargo identities; lose no pending original supply; and do not subject conversion to ordinary Tanker selection caps.

Inspect the complete failing test first. Identify the converting Spiker identity, how pending supply is set to zero, whether other Spikers are present, what `Assert.DoesNotContain` checks, whether it checks the converting identity or every Spiker, whether Level 11 permits another legitimate Spiker, and whether Level 12/shared changes affected setup or deterministic selection.

Trace production behavior and verify that the intended Spiker reaches the terminal boundary; pending original count is zero; its identity is preserved and changes from Spiker to Tanker; exactly two distinct latent cargo identities belong to it; pending supply is unchanged; runtime-representation validation holds; and any other Spiker has a different legitimate identity.

If these invariants hold, correct a collection-wide “no Spiker anywhere” assertion to the intended identity-specific invariant. If the intended original remains a Spiker, correct the production bug narrowly.

Determine whether the exact test fails alone or only with another test; investigate shared-state leakage, random-state isolation, dynamic Level 11 selection, and test ordering. Tests must not depend on execution order.

## Scope protections

Do not alter accepted Level 11 or Level 12 gameplay parameters or behaviors. Preserve supply, selection limits, Spiker/Fuseball eligibility, Tanker behavior, Flipper programs, seven-slot capacity, Spike behavior, progression, spinner, presentation, HUD, Fuseball animation, and terminal Fuseball behavior. Do not change Level 12 gameplay or begin the Fuseball bonus-vector correction.

Preserve the genuine Level 12 candidate and topology/headless fixes, including shape row 12, closed topology, progression, wave policy, model-frame mapping, headless production soak, topology validation, calibration candidate 1.3 / .8 / 0, and deferred Level 12 ExitZoom status.

## Validation

After the smallest correction, run with explicit hang timeouts:

1. The exact failing test alone.
2. `Level11ProductionCorrectionTests`.
3. Relevant terminal Spiker/Tanker/cargo tests.
4. Level 11 identity/ledger tests.
5. Level 12 headless production soak.
6. Level 12 production/topology tests.
7. Complete solution test suite.
8. Release solution build.
9. `git diff --check`.

Do not launch the interactive game. If the isolated test passes but the full suite fails, resolve any test isolation/order dependence before reporting success.

## Report and repository handling

Save a paired report under AI-Logs documenting the exact reason, classification, production correctness, change, isolated and surrounding test results, full-suite result, Level 12 soak, Release build, whitespace check, and any unresolved issues. Do not stage, commit, or push. Preserve unrelated worktree changes. Stop after resolving this single automated regression.

TEMPEST_LEVEL11_TERMINAL_SPIKER_TEST_RESOLUTION
