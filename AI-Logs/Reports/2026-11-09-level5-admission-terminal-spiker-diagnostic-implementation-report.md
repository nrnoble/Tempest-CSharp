# Level 5 admission and terminal Spiker diagnostic implementation

Date: 2026-11-09

Status: **PARTIAL / BLOCKED.** The candidate is implemented but final automated
validation did not execute because Windows Application Control blocked the
generated Release game assembly. Manual gameplay acceptance remains pending.

## Starting state

- Expected baseline: `62c66e0ab19436ea162935627021154be21e0a41`.
- Starting HEAD matched the baseline. The tracked worktree was clean.
- Read the required
  [Level 5 concurrency ROM investigation](2026-11-09-level5-concurrency-rom-investigation-report.md)
  before implementation.
- Preserved the complete substantive user task in the matching ignored
  [implementation prompt](../Prompts/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-prompt.md).
- No accepted behavior documentation was changed.

## Implementation

- Added a Level 5-specific admission snapshot and policy in
  `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs`. It is enabled only for
  the canonical `LevelConfiguration.Level5Production` instance, shared by
  direct, campaign, and recorded production startup. Copied configurations
  retain their previous picker behavior.
- Ordinary Level 5 admission counts live Flippers (including released and
  shot-pending cargo), ordinary and converted Tankers, Spikers, and other live
  enemy types against seven physical places. Eggs, reserves, latent cargo,
  bullets, persistent Spikes, and resolved identities do not occupy places.
  The snapshot is rebuilt at each ready-Egg hatch, so same-tick occupancy
  changes apply immediately. A blocked hatch leaves its ready Egg and ledger
  identity unchanged.
- Ordinary choices are restricted to Tanker and Spiker. The policy enforces
  Spiker 2/3 and ordinary Tanker 1/1 minima/maxima, with unmet Spiker minimum
  taking priority over Tanker. Flipper minimum remains zero and no positive
  Flipper availability can make it an ordinary choice. Maxima constrain
  ordinary admissions, not converted Tankers or released cargo.
- Flipper availability subtracts latent Tanker cargo units with the
  investigation's byte-wrapping behavior, then clamps to physical headroom.
  Each live Tanker reserves its two latent units but consumes only its own
  physical place. Cargo reentry bypasses species choice and waits only for
  physical headroom.
- Once both minima are satisfied and Tanker and Spiker are eligible, selection
  is deterministic: the shortest live Spike below the translated `$CC`
  threshold selects Tanker; otherwise it selects Spiker. This is an
  **APPROXIMATION / PROJECT TRANSLATION**, using the established
  `$F0`/`$10` to C# height mapping (`$CC` maps to about 64.29 world-height
  units); C# Spike height is not a literal ROM coordinate. With no live Spike,
  the bounded fallback is Spiker.
- The generic pending-original mapping is the ledger count in Reserve or Egg
  states. Active and terminal originals and separately owned cargo are
  excluded. On canonical Level 5 only, a Spiker reaching the existing C#
  retreat boundary converts in place to a Tanker when that count reaches zero.
  It keeps its original ID and receives one normal two-Flipper cargo pair.
  That height-zero boundary remains a **PROJECT TRANSLATION / APPROXIMATION**,
  not literal ROM timing.
- The ledger now retains a converted-Spiker-Tanker marker across Level 5 death
  reconstruction. A retained converted Tanker reenters with its original ID,
  type, and cargo pair, subject to physical headroom rather than the ordinary
  Tanker maximum. Generic reconstruction that clears species selection also
  clears this marker; Level 7/8 policy behavior remains unchanged.
- Complete two-Flipper cargo allocation and all existing release, activation,
  scoring, routing, zap, and lifecycle behavior remain unchanged. If a
  complete pair exceeds seven live objects, both identities remain present
  and subsequent ordinary admissions wait for headroom.
- The ROM Super Zapper admission pause remains deferred. No established
  mapping exists between `zap_running` and the accepted C# zap timers, so this
  candidate does not change Egg approach/hatching during Super Zapper use.

## Files changed

- `src/Tempest.Game/Levels/Level5AdmissionPolicy.cs` — new Level 5 policy and
  snapshot.
- `src/Tempest.Game/Levels/Level3WaveLedger.cs` — converted-Tanker identity
  retention and reconstruction invariant.
- `src/Tempest.Game/TempestGame.cs` — canonical Level 5 admission and terminal
  conversion wiring.
- `tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs` — minima, threshold,
  cargo arithmetic, physical capacity, waiting, reentry, terminal conversion,
  and pending-supply tests.
- `tests/Tempest.Game.Tests/Level5ProductionTests.cs` — updated superseded
  random-picker expectations and added copied-configuration isolation.
- `AI-Logs/Prompts/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-prompt.md`
  — complete substantive prompt.
- This report.

## Validation

- An earlier focused Release run passed 24 tests, before the final waiting,
  terminal-conversion, and copied-configuration test additions; it does not
  validate the final candidate.
- Final focused command:
  `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests" --blame-hang-timeout 5min`
  - **Build phase: PASS** — game and test assemblies compiled.
  - **Test execution: BLOCKED** — VSTest could not load
    `tests\Tempest.Game.Tests\bin\Release\net9.0\Tempest.Game.dll`.
    Windows Application Control reported “An Application Control policy has
    blocked this file” (`0x800711C7`). All 28 selected tests were prevented
    from running; this is not a test pass or a gameplay failure.
- Per repository security guidance, no equivalent test route, rebuild, full
  solution test run, or separate full solution build was attempted after the
  block.
- `git diff --check`: **PASS**.
- Static review covered the changed production paths, the complete tracked
  diff, and the new policy and test files.

## Repository state and remaining work

- HEAD remains at the expected baseline; no files are staged. No commit or push
  was made, and the interactive game was not launched.
- This diagnostic is not accepted production behavior. Automated validation
  remains blocked, and arcade gameplay comparison remains pending.
- Fidelity questions for later review include the shortest-Spike threshold
  translation, real arcade admission cadence/composition, live crowding and
  complete-pair saturation behavior. Neal should also compare terminal
  Spiker-to-Tanker behavior, death/reentry, Super Zapper entry cadence,
  persistent Spikes, rim combat, and Level 5 completion/progression.
- Once an approved validation route is available, the supported direct
  Level 5 launch command for Neal's manual comparison is:

  `dotnet run --project src\Tempest.Game -- --level 5 --lives 10`

Manual acceptance remains **PENDING**. Do not treat this candidate as accepted
until validation and Neal's arcade comparison are complete.
