# Level 7 Spiker repeated-cycle firing correction

Date: 2026-11-10

Disposition: **IMPLEMENTED. AUTOMATED VALIDATION BLOCKED by one full-suite
failure. MANUAL LEVEL 7 ACCEPTANCE IS PENDING. CANDIDATE REMAINS UNCOMMITTED.**

## Starting state

- `HEAD` remains `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
- The existing Level 7 concurrency and saturated-cargo candidate was already
  uncommitted. Those changes were preserved.
- The complete substantive task is preserved in
  [the matching prompt record](../Prompts/2026-11-10-level7-spiker-repeated-cycle-firing-correction-prompt.md).
- No commit, push, or interactive game launch was performed.

## Evidence and design

- Neal's supplied arcade/MAME observation is that a continuing Level 7 Spiker
  remains able to fire on later ascents. The checked-in ROM listing describes
  firing through shooter eligibility, along-tube position, per-enemy
  `shot_delay`, global `shot_holdoff`, random pressure, and shared projectile
  capacity; it has no Spiker ammunition or lifetime shot allowance.
- The existing C# path coupled its only Spiker shot opportunity to
  `Growth > 100` and the growth-driven retreat decision. A retained high Spike
  can leave too little ascent for that branch to execute.
- Added an independent firing phase for the exact canonical Level 7 production
  configuration. Copied and noncanonical configurations, Levels 1–6, Level 8,
  and development Spiker scenarios retain their existing policy.
- The new phase evaluates live Spikers only while `Building` and within the
  existing `$30`-to-project-height approximation. This ascent-only phase gate
  is explicitly a **PROJECT TRANSLATION**; the ROM shared routine does not use
  this C# phase distinction. Retreat firing is not enabled.
- Per-Spiker cooldown starts ready, reloads after a created shot, and remains
  independent of Spike growth and retreat. The cooldown uses the existing
  Level 7 C# 62-tick Flipper cadence as a **PROJECT TRANSLATION /
  APPROXIMATION**; the ROM's raw `shot_holdoff` value is not converted directly.
- The shared random-pressure thresholds and Level 7 three-projectile capacity
  are reused. To preserve scripted randomness when firing is impossible, the
  Spiker path checks full capacity before drawing randomness; this is a
  project-side consumption guard.
- Existing persistent Spike growth, shortening, collision, and lifecycle
  behavior is unchanged. Pending-supply restart retains the same Spiker
  identity/place and Spike height. Zero-pending conversion and Tanker cargo
  logic are unchanged.

## Changes and tests

- Added Level 7-only policy and cadence properties to
  `LevelConfiguration`.
- Added Spiker cooldown state and the independent canonical Level 7 firing
  phase in `Spiker` / `TempestGame`; disabled the superseded growth-retreat
  shot only for canonical Level 7.
- Added deterministic tests for first-ascent firing, second-ascent firing
  after same-identity pending restart with a high retained Spike, growth
  remaining at six or less through the upper boundary, random rejection,
  cooldown reload/expiry, full-capacity no-random behavior and recovery,
  retreat-phase exclusion, and canonical configuration isolation.
- Renamed the legacy growth-retreat firing test to identify its noncanonical
  scope. Narrowly adjusted a Level 7 cargo movement assertion to allow a
  ready Flipper shot only when the shared projectile capacity is full.
- No gameplay behavior specification or development notes were changed.

## Automated validation

VSTest executed. Compilation succeeded as part of the test runs. The full
solution build was **not run** because validation stopped at the first
full-suite failure, as requested.

| Validation | Result |
|---|---|
| New Level 7 Spiker firing tests | **PASS** — 6 passed, 0 failed, 0 skipped |
| Focused Spiker / Level 7 / ledger regressions | **PASS** — 123 passed, 0 failed, 0 skipped |
| Affected Release regressions across Levels 1–8, Spiker/Spike, firing, cargo, Super Zapper, rim/completion, progression, recording/playback, campaign, and startup | **PASS** — 804 passed, 0 failed, 0 skipped |
| `dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min` | **FAIL / STOPPED** — VSTest ran; 1,217 passed, 1 failed, 0 skipped. The failure was `GameOverDensityDiagnosticTests.OmittedAndExplicitTwoRenderIdenticalGameOverMarks`: the two rendering paths produced different mark coordinates. Blame reported all tests finished; no sequence file was generated. The failure was not investigated further. |
| `dotnet build TempestCSharp.slnx --configuration Release` | **NOT RUN** — stopped after the full-suite failure |
| `git diff --check` | **NOT RUN** — stopped after the full-suite failure |

No security error, timeout, or cancellation was reported.

## Final state and manual handoff

- The accepted baseline commit is unchanged; the full Level 7 candidate and
  this correction remain uncommitted.
- No push or interactive game launch was performed.
- Full-suite validation is blocked by the rendering-diagnostic failure above;
  build and `git diff --check` remain outstanding.
- Manual Level 7 acceptance is pending. After automated validation is
  completed, Neal can launch:

  ```powershell
  dotnet run --project src\Tempest.Game -- --level 7 --lives 10
  ```

- Verify the same Spiker can fire on later ascents while persistent Spikes
  continue growing and Blaster shots continue shortening them. Also verify
  projectile pressure, unchanged movement, zero-pending Tanker conversion,
  saturated cargo, Levels 1–6, and recording/playback. Do not claim manual
  acceptance until Neal reports it.
