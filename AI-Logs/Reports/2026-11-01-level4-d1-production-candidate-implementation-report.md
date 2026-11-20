# Level 4 D1 isolated production-candidate implementation report

## Result

Implemented and corrected the isolated Level 4 production candidate from
checkpoint `2318ea2 Finalize player-death enemy retraction`. Neal subsequently
reported manual acceptance of the corrected Spike-exit behavior, late-wave
Spiker behavior, and overall Level 4 feel. This is acceptance of visible
behavior, not Atari's hidden identity accounting or general arcade fidelity.
The configured Avoid Spikes warning and exit-policy boundary are now covered
by focused tests. Only a short visual check of the new warning remains. No
commit or push was created.

## Manual acceptance failure and correction

The original implementation passed 10/10 focused Level 4 tests and 564/564
full-suite tests, with a clean build and `git diff --check`. Neal then reported
two manual failures during `dotnet run --project src/Tempest.Game -- --level4-test --lives 10`:

1. Player shots did not correctly damage persistent Spikes during the
   avoidance/exit sequence.
2. The final unresolved Spiker obligation could return, hatch as a Spiker
   again, and continue the cycle indefinitely.

### Incitti source findings (REFERENCE VERIFIED)

`Reference/incitti/tempest.bmx` was inspected directly:

- `Egg.PickEnemy` at lines 1982-1989 suppresses Spikers when
  `enemiesleft + egg_list.Count() < 4` or `spinner.numspinners > 3`.
  The boundary is strictly less than four; equality permits Spikers.
- `enemiesleft` tracks active enemy units. Tanker cargo count once
  materialized; a zapped enemy remains active until its delayed removal.
  Bullets and Spikes do not affect it.
- `egg_list.Count()` includes roaming, approaching, and the Egg currently
  hatching (`Egg.Hatch` selects before removing itself from the list). Reserve
  `eggsleft` is separate and is not included.
- A normal Spinner retreat at lines 3660-3669 removes the active enemy,
  decrements `enemiesleft`, increments `eggsleft`, awards no kill score, and
  leaves its Spike. A later hatch invokes `PickEnemy` again.
- `Player.Update` at lines 639-649 keeps firing eligible when not dying;
  `Shot.Update` at lines 1826-1838 advances shots by 6 and checks collisions.
  Spike collision at lines 3535-3545 requires the same lane and a shot below
  the tip, subtracts 10, awards 1, and removes the Spike at zero or below.
  Spike/player collision is lethal in the same lane when the Spike extends
  past the Blaster. During exit zoom/reverse zoom, baddie updates are restricted
  to Spike hazards.

### Root causes and corrections

- **Spike exit firing:** On Spike collision, C# discarded all player shots,
  and the reverse-zoom branch advanced no shots. Incitti continues advancing
  existing shots through reverse zoom, with Spike-only baddie updates. The C#
  path now retains and advances existing shots during reverse/retry and renders
  them there; new firing remains suppressed while the Blaster is dying. Avoid,
  delay, exit, and reverse shots use a dedicated Spike-only collision path over
  persistent Level Spikes. It applies exactly -10 height and +1 point, removes
  at zero or below, and does not touch enemy-ledger or enemy-destroyed
  accounting. Enemy simulation remains disabled.
- **Spiker recycling:** Retreat preserved `Level3OriginalType.Spiker` on the
  original ledger identity. Re-hatching only invokes the Level 4 picker for
  `Unselected` originals, so the returned Egg bypassed the late-wave policy.
  Retreat now returns the same original ID as unresolved reserve and clears
  only its selected runtime type. Its later Egg hatch reruns general selection;
  deterministic tests show the final one-unit Egg selects a non-Spiker at the
  source boundary without a new original allocation.

**ARCADE VERIFIED (visible behavior only):** Neal observed that endless
final-Spiker recycling does not match his Atari recollection. This does not
verify Atari's hidden Egg/identity mechanism.

### Subsequent manual acceptance and warning policy

Neal reports that manual revalidation accepted the corrected Spike-exit
behavior, the late-wave Spiker behavior, and the overall Level 4 feel. This
supersedes the previous pending status for those behaviors; it does not
establish Atari's hidden identity mechanism or broad arcade fidelity.

Neal's later direct arcade observation corrects the earlier firing claim:
movement remains available during the Avoid Spikes warning/pretravel delay,
but new firing is disabled until down-tube exit travel begins. Incitti's
`Player.Update` permits firing in AvoidSpikes and StartZoom, while the
shot-update loop continues advancing existing shots through avoidance and
exit. The C# Level 4 path now gates new firing to updates that begin in
ExitZoom and preserves shots already in flight across the AvoidSpikes
transition. Standard exits and the separate Spiker diagnostic retain their
previous firing policy. Neal's final focused manual revalidation accepted the
visible Level 4 behavior; this supersedes the earlier firing-specific
acceptance statement.

The prominent **Avoid Spikes** warning is rendered only for the configured
exit policy, through the controllable AvoidSpikes and pretravel ExitDelay
states. It hides when ExitZoom begins and during death, retry, and terminal
states. The existing
`ExitPolicy` already separates this behavior from configured Spike presence
(`StartingSpikeHeight`): Levels 1-3 use `Standard`, the Level 4 candidate uses
`AvoidSpikes`, and a future Spike-bearing Level 8+ may use `Standard`. The test
uses a synthetic configuration only; it does not add later-level content or
natural progression. The warning does not change timing, movement, collision,
or identity behavior; the separate Level 4 firing gate follows Neal's visible
arcade observation and passed focused manual revalidation.

### Correction tests and identity/lifecycle evidence

Added focused coverage for disabled new firing during AvoidSpikes/ExitDelay,
enabled firing during ExitZoom, in-flight shot retention at AvoidSpikes entry,
shot travel and Spike intersection, exact repeated damage/scoring and depletion,
Spike-only exit target behavior, reverse-shot persistence without new firing,
prevention of a corresponding collision, unrelated enemy/Egg freeze, the
strict above/equal/below-four picker boundary, reserve exclusion, approaching
Eggs, and returned-ID re-selection.

Retreat keeps `OriginalCount`, `TotalIdentityCount`, unresolved original IDs,
score, and destroyed outcomes unchanged; the Spike is separate. The returned
ID re-enters as an eligible Flipper in a deterministic below-boundary case.
Production death reconstruction also verifies active Spiker retraction,
unchanged persistent Spike geometry, exactly one Egg for the same unresolved
ID, unchanged identity sets, and no score/destroyed outcome. Existing Super
Zapper/Spiker tests continue to verify that Spikes are not zap targets or
enemy-accounting units.

No natural Level 3 -> Level 4 progression, Level 5, enemy-composition redesign,
death-presentation retuning, window/fullscreen change, palette work, or other
deferred presentation work was introduced.

## Level 4 configuration and geometry

`LevelConfiguration.Level4Candidate` defines displayed Level 4/internal source
index 3, board 3, palette 0, closed 16-lane topology, center Y 380/offset -65,
17 original enemy obligations, four initial Eggs, and 13 reserve obligations.
The exact ordered binoculars point set is loaded through `LevelLoader`; the
shared projection, renderer, and endpoint-hinge transition system are reused.
Geometry tests verify the ordered points, finite rendering, near/far
connections, player projection/highlighting, and all 32 directed transitions,
including start/destination ownership and sampled pivot points remaining
inside/on the board boundary. Super Zapper rendering does not mutate points.

The candidate creates one height-60 persistent Spike per lane, independently
of enemy accounting. Spikes remain through life reconstruction and spike-exit
retry, and do not block enemy-supply resolution.

## Production wave and identities

The existing Level 3 ledger was narrowly extended for original Spiker
identities and ActiveSpiker state. The Level 4 picker follows the source
selection boundaries for eligible Spikers, Flippers, and Tankers; Spiker speed
uses the source-shaped `index / 48 + Rand(2.5, 4.5)` expression. The four-Spiker
suppression, six-Tanker cap, and near-end suppression are retained. Fuseballs
and Pulsars remain ineligible.

A Spiker's spike is not an enemy identity. A retreat returns the same
unresolved Spiker original to reserve without score; destruction and rim-clear
remain terminal outcomes. Death reconstruction preserves terminal outcomes
and recreates unresolved identities without duplication. Total identity count
is 17 originals plus two cargo identities for each selected Tanker.

Level 4 reuses the accepted Tanker/cargo paths: original carrier ownership,
two cargo ordinals, latent/active cargo, natural escape/release, and Tanker
firing. Tests exercise natural release and confirm bullets do not change
identity accounting. The Super Zapper can resolve an original Spiker through
the production ledger. Level 3 lifecycle/regression tests remain unchanged and
pass.

## Bullets, Hurry-Up, presentation, and exit

Production Tanker bullets remain enabled. The Level 4 candidate enables the
reference-backed Spiker growth-retreat bullet path. Ordinary/cargo Flippers
remain silent across production Levels 1-4 to preserve accepted Level 1-3
behavior; Incitti makes them eligible from Level 1. This is an explicit
**PROJECT DECISION** and known source discrepancy, not an asserted Level-4-only
unlock. `--enemy-bullets-test` remains available.

No Spiker-specific rimit behavior, Fuseball/Pulsar production eligibility, or
palette cycling was added. Level 4 reuses the accepted enemy-retraction
presentation and yellow palette-0 lane highlighting. The source spike-avoid
exit reuses the established collision/reverse/retry behavior; a successful
candidate exit remains terminal and never constructs Level 5.

Natural Level 3 -> Level 4 progression remains unchanged and deferred to D2.
Normal no-flag play remains terminal at Level 3.

## Diagnostic and documentation

Added `--level4-test`, mutually exclusive with other scenario flags, compatible
with `--lives` and `--enemy-bullets-test`, replaying Level 4 on R and restarting
Level 4 after Game Over. Saved the exact supplied prompt under the requested
`AI-Logs/Prompts/` path; source and saved SHA-256 hashes matched.

Updated README, behavior specification, and development notes with the source
evidence boundary, D1 project decisions, known Flipper-bullet discrepancy,
terminal progression boundary, manual acceptance scope, warning policy, and
long-term progression constraints. Neal's manual report accepts visible C#
behavior only; source-backed identity details do not establish Atari's hidden
identity mechanism or general arcade fidelity.

## Final validation

- Focused `Level4CandidateTests` and `LevelConfigurationTests`:
  **29/29 passed**.
- Relevant groups (`Level4CandidateTests`, `SimulationTests`,
  `Level3MixedWaveTests`, `Level3WaveLedgerTests`, `SuperZapperTests`,
  `PlayerDeathPresentationTests`, `LevelProgressionTests`): **442/442 passed**.
- `dotnet test TempestCSharp.slnx --no-restore`: **573/573 passed**, no skips.
- `dotnet build TempestCSharp.slnx --no-restore`: passed, **0 warnings/errors**.
- `git diff --check`: passed; Git emitted only its working-copy LF-to-CRLF
  advisory for edited text files.
- `git status --short -- Reference/incitti`: clean. Reference source was not
  modified.
- `git log -1 --oneline`: remains `2318ea2 Finalize player-death enemy
  retraction`.
- Existing untracked `TEMP/` remains untouched. No Level 5, natural Level 4
  progression, open topology, rimit, Fuseball/Pulsar activation, palette-cycle,
  approach/death-presentation retuning, Blaster death-animation refinement,
  shot-size work, spinner changes, or audio work was introduced.

## Files changed

- `src/Tempest.Game/Levels/LevelLoader.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/Level3WaveLedger.cs`
- `src/Tempest.Game/Levels/Level3WaveSelection.cs`
- `src/Tempest.Game/Gameplay/Spiker.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
- `tests/Tempest.Game.Tests/LevelConfigurationTests.cs`
- `README.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Prompts/2026-11-01-level4-d1-production-candidate-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-01-level4-d1-production-candidate-implementation-report.md`

## Focused visual revalidation passed

Neal confirmed movement but no new firing during the warning/pretravel delay,
firing and Spike damage once down-tube travel begins, in-flight shot
continuation, warning disappearance at exit travel, retry behavior, and no
other obvious visual regression. Neal also manually accepted the natural
Level 1 -> Level 2 -> Level 3 -> Level 4 campaign. These manual results
establish visible behavior only, not hidden Atari identity/accounting
mechanisms or broad arcade fidelity. Natural progression is documented
separately in the D2 report.
