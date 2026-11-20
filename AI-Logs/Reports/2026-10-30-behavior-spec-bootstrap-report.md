
# Behavior Specification Bootstrap Report

## Construction

The specification was assembled from current C# behavior, project documentation,
the Incitti source, automated test names/assertions, and milestone reports. Each
claim was classified separately from the evidence that supports it. Arcade
labels were limited to observations explicitly supplied by Neal or already
recorded as manual validation; source code and tests were not treated as proof
of arcade fidelity.

## Files and reports consulted

- Project documentation: `README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`.
- Tests: `tests/Tempest.Game.Tests/SimulationTests.cs`.
- Current simulation and gameplay: `src/Tempest.Game/TempestGame.cs`,
  `src/Tempest.Game/Gameplay/Player.cs`, `Flipper.cs`, `Egg.cs`,
  `EnemyBullet.cs`, `Spiker.cs`, `Spike.cs`, `Tanker.cs`, `Fuseball.cs`,
  `Pulsar.cs`, and `Shot.cs`.
- Current level, scenario, scoring, and rendering: `src/Tempest.Game/Levels/Level.cs`,
  `src/Tempest.Game/Debug/DebugController.cs`, `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Systems/ScoreSystem.cs`, and
  `src/Tempest.Game/Rendering/VectorRenderer.cs`.
- Reference: `Reference/incitti/Readme.txt` and
  `Reference/incitti/tempest.bmx`.
- Historical reports: `AI-Logs/Reports/2026-10-30-flipper-rim-arrival-bugfix-report.md`,
  `2026-10-30-fuseball-implementation-report.md`,
  `2026-10-30-pulsar-analysis-report.md`,
  `2026-10-30-pulsar-implementation-report.md`,
  `2026-10-30-spiker-retreat-bullet-implementation-report.md`, and
  `2026-10-30-tanker-enemy-bullets-implementation-report.md`.
- Additional milestone evidence: `AI-Logs/2026-10-30-enemy-bullets-implementation.md`,
  `AI-Logs/2026-10-30-spiker-implementation.md`,
  `AI-Logs/2026-10-30-spiker-source-analysis.md`,
  `AI-Logs/2026-10-30-tanker-analysis.md`,
  `AI-Logs/2026-10-30-next-gameplay-milestone-analysis.md`, and
  `AI-Logs/2026-10-30-fuseball-validation-recovery.txt`.

## Evidence gaps and conflicts

- **Fuseballs:** Neal's explicit manual validation recovered for this revision
  establishes lane travel up and down, shootability, a score display on a hit,
  same-lane rim lethality, and the exit after the final Fuseball is destroyed.
  Only those observations are marked **ARCADE VERIFIED**. Exact cadence,
  climb/descent duration, numeric collision boundaries, score formula and
  thresholds, animation timing, and source speed remain
  **REFERENCE VERIFIED**.
- **Pulsar timing:** The 51-update decision cadence, 181-update pulse cycle, and
  18/14 pause values are established by Incitti/source arithmetic and current
  tests. No historical evidence reviewed establishes that Neal validated those
  exact constants against Atari gameplay; they remain **REFERENCE VERIFIED**.
- **Pulsar rim rendering:** An earlier analysis assumed ordinary rim lines
  remained during pulses. Neal's arcade observation established that only the
  affected near/top connection disappears and returns after the pulse, while
  unaffected connections remain visible and radial boundaries continue
  flashing. Literal Incitti rendering has broader line-suppression behavior.
  The current renderer's narrower correction follows Neal's observation, which
  outranks the earlier analysis and source interpretation.
- **Fuseball cadence:** An earlier analysis described an eight-update lateral
  cadence. Checking Incitti's increment-before-test order and the later
  implementation report establishes nine updates. Exact arcade cadence remains
  unverified.
- **Flipper rim arrival:** The older development note emphasized death only
  after a completed pivot. Later source review and the correction report
  distinguish a direct climbing arrival in the occupied lane, which Incitti
  treats as lethal, from an in-progress pivot. Only the pivot rule is explicitly
  recorded as arcade-validated; direct arrival remains reference-derived.
- **Death timing:** Incitti advances Pulsar/global pulse state during much of
  its death presentation, while current C# freezes gameplay for the full death
  interval. The specification documents this as a recreation policy, not an
  equivalence claim.
- **Tanker bullets:** Older README text says Tankers do not fire in the
  milestone. Current C# and the later Tanker enemy-bullet report implement
  Tanker firing only when `--tanker-test` and `--enemy-bullets-test` are both
  enabled. The specification identifies the older text as stale.
- **Arcade wave composition and pacing:** Controlled Spiker, Tanker, Fuseball,
  Pulsar, and enemy-bullet modes are development scenarios. Incitti's source
  establishes its own progression/composition, but this evidence does not
  establish that those exact rules match Atari Tempest.

## Intentionally incomplete

No required section was omitted. The specification intentionally leaves
unverified arcade constants and behavior unresolved, including direct Flipper
rim-arrival validation, bullet frequency, Spiker/Tanker tuning, normal-wave
composition, and exact exit presentation. It does not specify deferred
progression, Super Zapper, audio, bonuses, or open-tube presentation as
implemented behavior.

The Fuseball manual observations were recovered from Neal's explicit
validation. The specification wording was corrected to state that player shots
travel from the near rim toward the far end, to scope Pulsar observations to
the isolated development scenario, and to keep evidence labels and Markdown
spacing consistent.

Only the specification, this report, and the requested prompt archive were
created. No gameplay code, tests, README, project/configuration files, or Git
state were changed; nothing was committed.
