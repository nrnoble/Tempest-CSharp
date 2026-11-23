# Level 11 terminal-Spiker test resolution

## Result

**Classification: 2. STALE OR OVERBROAD TEST ASSERTION**

The failing assertion used `Assert.DoesNotContain(game.Spikers, _ => true)`, which requires the entire Spiker collection to be empty. The behavior under test is narrower: the specific original that reached the terminal boundary must stop being represented as a Spiker and become the Tanker with the same identity. Level 11 permits up to three active Spikers, so conversion of one identity does not imply that every unrelated Spiker must be absent.

The reported suite failure did not include the matched Spiker's identity. I could not reproduce that original failure: the exact test passed alone, the surrounding class passed, and the complete suite passed on the current worktree before and after the correction. Therefore, I cannot claim whether the single earlier match was the converting identity or a different identity. The assertion was nevertheless broader than the intended invariant and was corrected to check the converting identity explicitly.

No evidence of static/shared-state leakage or test-order dependence was found. The test now supplies a fixed random seed to isolate its setup. Its polling condition also waits for the Tanker with the intended original identity rather than any Tanker, so an unrelated Tanker cannot end the test early.

## Production trace and change

Production code was not changed. With the corrected deterministic test:

- The selected original is confirmed to be a Spiker before simulation, with generic pending supply at zero.
- The test waits for a Tanker bearing that exact original ID.
- The ledger reports that original as `Tanker` / `ActiveTanker`; no Spiker with that original ID remains.
- The Tanker owns exactly two distinct cargo IDs, both owned by that original and still `Latent`.
- Generic pending original count is unchanged by conversion.
- `RunOneStep` executes the existing runtime-representation validation during the tested simulation.

The test now asserts these identity-specific invariants and retains exact cargo identity/ownership coverage.

Changed file: `tests/Tempest.Game.Tests/Level11ProductionCorrectionTests.cs`.

## Validation

All required validation completed successfully with the explicit 5-minute hang timeout on test commands:

| Validation | Result |
|---|---|
| Exact failing test alone | **PASS** — 1 passed |
| Surrounding `Level11ProductionCorrectionTests` | **PASS** — 4 passed |
| Related terminal Spiker/Tanker/cargo regressions | **PASS** — 84 passed |
| Level 11 identity/ledger coverage (`Level3WaveLedgerTests`, `Level3MixedWaveTests`) | **PASS** — 62 passed |
| Level 12 production/topology, including bounded headless soak | **PASS** — 8 passed |
| Complete solution suite | **PASS** — 1,822 passed, 0 failed |
| Release solution build | **PASS** — 0 warnings, 0 errors |
| `git diff --check` | **PASS** |

The diff check emitted only the existing LF-to-CRLF notice for `src/Tempest.Game/Levels/LevelLoader.cs`.

## Scope and remaining work

No Level 11 production behavior, Level 12 candidate files, or accepted gameplay parameters were changed. No interactive game was launched; this test-only correction does not require renewed manual gameplay acceptance. No files were staged, committed, or pushed. All unrelated pre-existing worktree changes remain untouched.

The identity involved in the historical one-off assertion failure remains unknown because its original failure output did not report that identity and the failure did not reproduce. The corrected test now encodes the intended identity-specific regression contract directly.

**LEVEL 11 TERMINAL-SPIKER REGRESSION ASSERTION CORRECTED — TEST-ONLY**
