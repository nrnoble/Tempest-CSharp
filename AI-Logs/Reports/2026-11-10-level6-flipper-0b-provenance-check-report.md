# Level 6 Flipper $0b provenance check - 2026-11-10

## Conclusion: C. CONFIRMED MISMATCH

Canonical C# Level 6 neither consumes `$0b` nor establishes an intentional
translation of that program. More than missing provenance is established:
deep-shot Tanker cargo uses a different below-rim movement algorithm.
This is a static source mismatch, not a newly reproduced gameplay failure.
Neal's current manual comparison and Level 6 acceptance remain intact.
No movement or concurrency change is authorized or made.

## ROM-BACKED

Evidence: [local text disassembly](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).

- Lines 2856-2859, 2904-2905, 2934-2941: the level-block table
  `07 0b 19 24 53 0b ...` loads `flipper_move`; displayed Level 6 selects
  `$0b`. This is a p-code entry offset into the byte region at `$a0f7`,
  not a numeric speed or a count of eleven flips.
- Lines 3482-3486 (`$9a9d-$9aa7`): ordinary Flipper initialization loads
  `flipper_move`. Lines 3556-3578 (`move_enemies`, `$9b34-$9b4f`): the
  per-enemy saved PC is interpreted and saved each update.
- Lines 4257-4270 (`$a102-$a10f`): `$0b` loads an eight-move counter,
  advances along the tube eight times, starts one flip, completes it
  **without along-motion during the turn**, then repeats. No Spike predicate,
  player-targeting opcode, or scripted direction reversal occurs in this loop.
  Flip handlers use direction bit `$40`; open-end correction is separate.
  No screen-left/right handedness equivalence is asserted here.
- Lines 3542-3555 (`$9b07-$9b1d`): cargo with saved `enemy_along >= $20`
  goes through the level-selected initialization, hence Level 6 `$0b`;
  cargo nearer the rim (`< $20`) instead takes default `$07` straight motion.
  `$a06f-$a0f6` (lines 4179-4240) preserves the carrier's along coordinate,
  allocates opposite adjacent sides, and sets opposite direction bits.
  `$9981-$9983` (lines 3315-3316) stores the selected PC in each child.
  Player-shot destruction calls this setup at `$a32c` (line 4609).
  Natural splitting is triggered below `$20` at `$9c83-$9c91`
  (lines 3724-3732), so it uses `$07`, not `$0b`.
- Thus ordinary Flipper *capability* and deep-shot cargo share `$0b`,
  but not every cargo release does. Ordinary Level 6 Flippers are excluded
  by zero-minimum admission: table lines 2765-2768 and selector
  `$9a15-$9a18`, `$9a6a-$9a6d` (lines 3402-3403, 3451-3452).
  Cargo bypasses ordinary selection; its movement capability is reachable.

## PROJECT-CURRENT C# path

- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
  lines 22-29, 90-91, 257-283: `Level6Production` inherits
  `BelowRimFlipperProgram.Existing`; no Level 6 `$0b` policy exists.
  Tuning is `(CanFlip: true, ClimbSpeed: 2, TurnSpeedDegrees: 9,
  LanePauseTicks: 20, RimPauseTicks: 23)`, with endpoint-hinge geometry.
  Those constants do not implement the ROM eight-move/no-motion-turn loop.
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs), lines 1499-1553:
  `ReleaseTankerCargo` delegates to `Tanker.ReleaseFlipperCargo`, then calls
  `Flipper.CreateTankerCargo`. Specialized Level 5/7/8 route flags are all
  false for Level 6, regardless of shot depth.
- [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs), lines 130-157,
  180-210: the specialized predicates recognize only Levels 5/7/8 policies;
  Level 6 uses the common opposite-side/direction cargo pair.
- [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs), lines 82-146,
  151-180, 530-594: cargo starts immediately in its release pivot.
  `canFlip = tuning.CanFlip && (!IsTankerCargo || useLevel5SpikeSensitiveProgram)`
  is false for Level 6 cargo. While turning it nevertheless gains height
  by climb speed 2 each update, then `CompletePivot` gives it the
  **23-tick rim-pause value even below the rim**. During that pause it gains
  height by 1 each update, then starts another same-direction pivot.
  It repeats pivots/pauses until rim pursuit takes over. It does NOT switch
  to a straight climb after the release pivot.
- Ordinary non-cargo `Existing` Flippers instead use `CanFlip`, the initial
  player-relative direction choice in `AdvanceClimb` (lines 274-301),
  and the 20-tick lane pause below the rim. Thus ordinary and cargo C#
  paths are not identical either; ordinary canonical Level 6 admission
  does not expose that hypothetical ordinary path.

The decisive mismatch for deep-shot cargo is **repeated climbing
pivots/pauses versus eight straight advances followed by a stationary-depth
flip**. Shared direction persistence does not establish program equivalence.

## Existing tests, comments, and documentation inspected

- [Level6ProductionTests.cs](../../tests/Tempest.Game.Tests/Level6ProductionTests.cs),
  lines 20-60: configuration/tuning assertions, not a `$0b` trace test.
- [Level6AdmissionPolicyTests.cs](../../tests/Tempest.Game.Tests/Level6AdmissionPolicyTests.cs),
  lines 11-56: zero-minimum ordinary Flippers excluded despite availability.
- [Level7ProductionTests.cs](../../tests/Tempest.Game.Tests/Level7ProductionTests.cs),
  lines 338-346: explicitly asserts Level 6 retains `Existing`.
- [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs),
  lines 2688-2768: common cargo immediate pivot, climb during pivot,
  pause climb, and retained direction. These are shared-path tests,
  not Level 6 ROM-program fidelity tests.
- [Behavior spec](../../TEMPEST-BEHAVIOR-SPEC.md), lines 117-150:
  source-derived 9/20/23 tuning and manual acceptance, not `$0b` equivalence.
  [Development notes](../../TEMPEST-DEVELOPMENT-NOTES.md), lines 2445-2495:
  accepted reused cargo/movement and source-derived timing.
- Historical corroboration, not primary authority:
  [Level 7 preimplementation report](2026-11-04-level7-preimplementation-analysis-report.md),
  lines 291-305, already identifies Level 6 generic motion versus `$0b`
  cadence; [level-specific ROM analysis](2026-11-06-level-specific-flipper-spike-rom-analysis-report.md),
  lines 30-78 and 112-127, decodes `$0b` and depth-dependent cargo selection.
  [Level 6 concurrency investigation](2026-11-10-level6-concurrency-rom-investigation-report.md),
  lines 45-64, distinguishes ordinary admission from cargo.

**PROJECT TRANSLATION:** no established `$0b` translation in this path.
**REFERENCE-DERIVED:** durable records attribute the movement tuning/reuse
to the existing secondary-reference implementation.
**ARCADE-OBSERVED:** Neal currently reports Tanker-released Level 6 movement
looks correct compared with the arcade; this is preserved, not reclassified
as ROM equivalence. **UNKNOWN:** perceptual significance of the source mismatch;
this task performed no new gameplay comparison.

## Preservation and next action

Only the expressly requested ignored prompt and this ignored report were
created. Production files, tests, durable documentation, and the complete
pre-existing candidate were left unchanged.

Read-only Git checks: HEAD remains
`94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`; index empty;
`git diff --check` passes. Before/after tracked binary-diff fingerprint:
`752a31388d8f186636614ae7d872794871576c4a`.
The two untracked candidate files retain their SHA-256 values:

- `src/Tempest.Game/Levels/Level6AdmissionPolicy.cs`:
  `0E47C7620FD21799D969FA6710A24D00C8F93EEECE2CECC8D1A2C33D79A1C07A`
- `tests/Tempest.Game.Tests/Level6AdmissionPolicyTests.cs`:
  `96E7D0CF5ECBCA75E92F52809B483D54FE5AEF1F0E27B3A2B1410F502147D354`

No tests, builds, game launch, Internet search, raw-ROM inspection,
staging, commit, push, or Level 7 work occurred.

A future **separately authorized bounded comparison** of deep-shot Level 6
cargo is worthwhile given the explicit cadence mismatch and Neal's stated
importance of level-specific movement. No further broad reverse engineering
is needed to establish this mismatch. No retuning is recommended without that
comparison and Neal's decision. Stop here for Neal; do not silently reopen
acceptance or proceed with closeout planning.

**Final decision: C. CONFIRMED MISMATCH.**
