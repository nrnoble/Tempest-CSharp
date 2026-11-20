# Level 9 Spiker/projectile relative-speed read-only diagnostic

Date: 2026-11-11

**Disposition: READ-ONLY DIAGNOSTIC COMPLETE.** The current Level 9 candidate
was preserved. The production Spiker-speed relationship violates Neal's
cross-level arcade-fidelity requirement for most possible Level 9 Spiker
speeds. The ROM-listing comparison remains **UNRESOLVED** against that
observation.

## 1. Starting Git/candidate state

- Branch/status: `## master...origin/master`.
- `HEAD` and `origin/master` both equal
  `037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9`.
- The index was empty. The candidate had 17 modified tracked files and two
  untracked files; `git diff --stat` reported 512 insertions and 51 deletions.
- The production/test path inventory matched the changed-file inventory in
  [the Level 9 bounded implementation report](2026-11-11-level9-bounded-production-implementation-report.md):
  `Flipper.cs`, `Tanker.cs`, `LevelConfiguration.cs`, `LevelLoader.cs`,
  `LevelProgression.cs`, `Level9WaveSelection.cs` (new), `Program.cs`,
  `TempestGame.cs`, and the 11 listed Level 9/related test files.
- The branch, checkpoint, index, and candidate changes matched the task's
  expected state. Investigation proceeded without staging or altering them.

## 2. Manual Level 9 observation

**ARCADE/MAME MANUAL OBSERVATION (Neal, supplied in the task):** Level 9
otherwise plays well, but a Spiker can reverse toward the deep end while its
previously fired projectile has not yet reached the rim. The projectile
visibly lags its shooter during ascent. Neal does not accept this behavior.
This session did not independently play the arcade or MAME.

## 3. Cross-level arcade/MAME fidelity mandate

**ARCADE/MAME MANUAL OBSERVATION (Neal, repeated and supplied in the task):**
while both objects travel toward the rim, a Spiker projectile must move
faster than its firing Spiker. The Spiker must not catch, overtake, or
visibly outpace its shot; projectile-to-Spiker separation must increase.
Absolute Spiker speed may vary by level. This is a relative-speed requirement,
not a mandate for one universal Spiker speed.

## 4. Files and evidence inspected

- Complete [Level 9 bounded implementation report](2026-11-11-level9-bounded-production-implementation-report.md)
  and relevant speed/fire sections of the
  [Level 9 status and ROM preimplementation report](2026-11-11-level9-status-rom-preimplementation-investigation-report.md).
- [Spiker.cs](../../src/Tempest.Game/Gameplay/Spiker.cs),
  [EnemyBullet.cs](../../src/Tempest.Game/Gameplay/EnemyBullet.cs),
  [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs),
  [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs),
  [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
  and relevant creation, update, and firing phases in
  [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs).
- The relevant Level 7 comparison was checked against the
  [Level 7 relative-speed investigation](2026-11-10-level7-spiker-projectile-relative-speed-investigation-report.md),
  [Level 7 calibration diagnostic](2026-11-11-level7-spiker-projectile-relative-speed-calibration-diagnostic-report.md),
  and [accepted Level 7 closeout](2026-11-11-level7-complete-accepted-closeout-report.md).
  Those reports already establish Level 7's accepted relationship; their
  numerical values were not copied as a Level 9 target.
- The local [Atari Tempest ROM listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md):
  speed parameter processing and `crack_speed` (`$9328-$93FF`), parameter
  tables (`$9607`), the normal active-state call order (`$970E-$9726`),
  enemy movement interpreter (`$9B1E-$9BB0`), `$0C` movement handler
  (`$9C58-$9CB5`), Spiker entry (`$A0F7`), enemy shot movement
  (`$A18F-$A1E1`), and firing (`$A2A6-$A303`).
- No ROM binary was opened. No recording other than the exact path requested
  was inspected.

## 5. Current Level 9 Spiker speed-selection path

`TempestGame` selects a Spiker when a production Egg resolves to the Spiker
type. The normal Level 7 branch uses its accepted, explicit Tanker movement
authority. Every other level using this general path uses:

```text
ProgressionIndex / 48f + 2.5f + (float)(random.NextDouble() * 2)
```

Level 9's `ProgressionIndex` is 8. Its base in C# single precision is
`2.6666667461395264`; the random term is sampled once when each Spiker is
created, and the resulting `Spiker.Speed` stays fixed for that instance.
Thus Level 9 is **randomized per Spiker**, with progression-derived base
speed, not a fixed Level 9 speed and not a speed that changes as the Spiker
grows.

The nominal real-number range is `[2.6666667, 4.6666667)`. The expression
casts the random contribution to `float` before adding it. Values near the
upper end therefore round the contribution to `2.0f`; the actual maximum
production float is `4.6666669845581055`. The speed is not selected by
`Level9WaveSelection` or by a Level 9 movement field in
`LevelConfiguration`.

## 6. Current projectile movement path

`EnemyBullet.Advance()` increments `Height` by exactly `3` depth units and
updates its visual rotation. All registered enemy bullets use this same
method and rate, including bullets fired by Tankers and Level 9 Spikers.
There is no source-specific projectile speed.

During each active `RunOneStep`, `TempestGame` advances already-existing
enemy bullets before updating Flippers, Tankers, and Spikers, then runs the
enemy firing phases. A Tanker fires after its own movement; Level 9's Spiker
firing phase runs after `UpdateSpikers`. Both create a bullet at the enemy's
post-movement height. Since the bullet phase has already run, a newly
created bullet first advances on the next active simulation step. This
one-step creation/update ordering is shared and does not change the ongoing
per-step rate comparison.

## 7. Current Level 9 Tanker/Flipper speed paths

- **Tanker:** Level 9 creates the standard `Tanker`; its default
  `ClimbSpeed` is `Tanker.DevelopmentClimbSpeed = 1.75` depth units per
  active update. `Tanker.UpdateForDevelopment` adds that amount once while
  climbing. It then may create the shared `EnemyBullet`.
- **Flipper:** Level 9 configuration specifies `Flipper.ClimbSpeed = 2`.
  The ordinary bottom Flipper type is disabled on Level 9, but Tanker cargo
  Flippers can be active. A climbing Flipper adds the configured amount on
  its along movement; pivot/pausing states have their separately specified
  movement behavior, so 2 is the comparable normal climbing rate, not every
  state’s displacement.
- Tankers, Flippers, and Spikers are advanced once per active C# simulation
  step when their update phase runs. These are comparable C# depth units,
  but the types have distinct policies and state transitions. Shared C#
  units do not establish arcade equality.

## 8. Deterministic relative-speed matrix

Direction convention: C# `Height` increases toward the rim. Both objects
start at depth 100 immediately after a hypothetical Level 9 shot is created.
Each row advances both for 10 subsequent active steps, while the Spiker
remains in its Building state. Spiker travel is the repeated single-precision
`Height += Speed` operation; projectile travel is repeated `Height += 3`.
Separation is `projectile depth - Spiker depth`: positive means the
projectile is farther rimward; negative means the Spiker is. The ratio is
`projectile travel / Spiker travel`.

The figures are deterministic C#-float calculations of the exact production
formula and movement increments, with float32 rounding on each addition. They
are not arcade timing measurements and do not claim a compiled harness or
per-tick playback telemetry.

| Spiker speed | Projectile speed | Steps | Spiker travel | Projectile travel | Separation change | Travel ratio | Result |
|---:|---:|---:|---:|---:|---:|---:|---|
| 2.666666746 | 3.000000000 | 10 | 26.666641235 | 30.000000000 | +3.333358765 | 1.1250011 | Projectile pulls ahead |
| 3.000000000 | 3.000000000 | 10 | 30.000000000 | 30.000000000 | 0.000000000 | 1.0000000 | Equal |
| 3.666666746 | 3.000000000 | 10 | 36.666656494 | 30.000000000 | -6.666656494 | 0.8181820 | Spiker gains |
| 4.666666985 | 3.000000000 | 10 | 46.666671753 | 30.000000000 | -16.666671753 | 0.6428571 | Spiker gains |

The equality is reachable: with base `2.666666746`, float random-term values
from approximately `0.3333331347` through `0.3333333731` round the selected
Spiker speed to exactly `3.0`. This corresponds approximately to
`NextDouble()` values from `0.1666665673` through `0.1666666865`, subject to
the float conversion. The immediate neighboring output speeds are
`2.999999762` and `3.000000238`.

## 9. Exact conditions under which Level 9 Spiker outruns/equalizes/lags its projectile

For matched active ascent updates:

- `Spiker.Speed < 3`: the projectile gains separation.
- `Spiker.Speed == 3`: separation is unchanged; this does not satisfy the
  required strict projectile-faster relationship.
- `Spiker.Speed > 3`: the Spiker gains on its own projectile, so separation
  shrinks and the projectile lags.

The production range includes all three outcomes. Every outcome above 3
violates the observed requirement; equality also violates the strict
requirement. The return/retreat phase subtracts `0.90 * Spiker.Speed` from
Spiker height while the bullet continues rimward, so the bullet then gains
separation. That later retreat does not repair the during-ascent defect Neal
observed.

## 10. Comparison with accepted Level 7

Accepted production Level 7 selects normal Spiker speed from
`Tanker.DevelopmentClimbSpeed`, currently `1.75`, rather than the old
randomized progression formula. The value is fixed for normal Level 7
production; the numeric override is DEVELOPMENT-ONLY. At `1.75` versus the
shared `3` bullet rate, a matched 10-step interval gives 17.5 Spiker units,
30 projectile units, `+12.5` separation change, and a travel ratio of about
`1.7143`.

The Level 7 accepted closeout classifies the Tanker/Spiker visible-rate match
and projectile lead as **ARCADE OBSERVED / MANUALLY ACCEPTED**. It also says
the 1.75 C# value is a project movement authority, not a ROM measurement.
Level 9 instead falls through to the older general randomized formula. The
Level 9 implementation report calls speed mapping a project candidate; it
does not establish that the random range is an intentional or evidence-backed
Level 9 Spiker policy. The difference is therefore an inherited implementation
path, not an accepted Level 9 speed decision. Do not copy Level 7's number or
code wholesale.

A common per-level speed-policy seam could allow each level's Spiker speed
to differ while testing its maximum effective movement against that level's
projectile movement. Level 7 establishes a Tanker-derived relationship for
Level 7 only; the Level 9 ROM records Tanker speed as Flipper-derived, but
does not establish that a Level 9 Spiker should match either one.

## 11. Recording identity and bounded headless-playback result

The exact requested path
`Temp\GameplayRecordings\level9-manual-acceptance.json` does **not** exist.
No substitute recording was used. Startup metadata, finite input count,
headless playback, final state, and input consumption could not be inspected.
No playback was run, and no per-step speed telemetry is claimed.

## 12. ROM Level 9 Spiker speed trace

The prior Level 9 ROM report identifies the Level 9 raw Spiker input as
`$C0`; the local listing's effective-level parameter tables and speed
processing path were checked for this diagnostic. The speed setup calls
`crack_speed` for `spd_spiker_lsb`, stores returned A in that low field and Y
in `spd_spiker_msb`. Three paired shifts of the input and `$FF` high-byte
accumulator give:

```text
(($FF00 | $C0) << 3) mod $10000 = $FE00
```

Interpreted as signed 8.8 this is `-2.0` along units per movement operation.
The speed fields have low/fraction byte `$00` and high/integer byte `$FE`.

The Spiker p-code at `$A0F7` begins with `$0C` (move per type speed), followed
by its growth/reversal/conversion operation. The enemy interpreter runs each
occupied enemy's p-code through its halt on the active `state_04` pass.
`eact_0c` obtains the enemy type's speed-table index and updates the Spiker's
16-bit along position using the type's low/high speed fields. On the ascent
branch the `$FE00` value moves it two raw ROM along units in the rimward
direction per pass. Spike growth and retreat selection do not change that
per-operation speed.

## 13. ROM projectile speed trace

The prior Level 9 report identifies the Level 9 enemy-projectile input as
`$C0`. Its separate `crack_speed` call stores the same outputs:
`enm_shotspd_lsb=$00`, `enm_shotspd_msb=$FE`, or signed 8.8 `-2.0`.
`move_shots` adds the projectile low speed to the shot's fractional position
byte and carries into the high along-position byte. With zero fractional
speed, the enemy projectile likewise moves two raw ROM along units per
`move_shots` call.

For comparison, Level 9's Flipper raw input `$AF` converts to `$FD78`
(signed 8.8 `-2.53125`); the setup copies those converted fields to the
Tanker fields. This is ROM-listing evidence about the respective ROM
movement fields, not a C# depth-unit or visible-frame conversion.

## 14. crack_speed/update-cadence findings

- **Processing:** `crack_speed` initializes the high accumulator to `$FF`,
  shifts the input/high pair left three times, and the call sites store the
  returned A byte as the fractional/low field and Y as the integer/high
  field. Its explanatory register comment reverses those names; the call-site
  stores and subsequent low-then-high additions establish the field order.
- **Difficulty:** the adjacent easy/hard branches adjust shot capacity and
  Flipper speed, among other parameters. They do not adjust Level 9's Spiker
  or projectile speed inputs before their conversions.
- **Cadence:** normal `state_04` calls `move_enemies`, then `move_shots`,
  then `enm_shoot`. The Spiker's `$0C` executes once in its p-code on that
  enemy pass; each existing projectile is advanced once in the shot pass.
  The shot is created from the Spiker's already-moved position after
  `move_shots`, and first moves on a later pass.
- **Fixed point/carry:** both `$C0` values produce `$FE00`, so both have zero
  fractional movement and the same two-unit displacement per corresponding
  active pass in the listing's coordinate domain. No fractional carry
  difference or per-entity scheduling difference in `state_04` was found.
- **Other state/display scheduling:** the separate `state_20` code path also
  calls `move_shots` without the normal `move_enemies` call. Its relevance to
  the observed same-ascent Level 9 behavior was not established. It could
  cause projectile-only movement if that state is entered with a live shot,
  but it does not explain a persistent relative-speed difference during
  ordinary `state_04` passes. The listing does not establish a mapping from
  those passes to MAME video frames or C# simulation steps.

## 15. Reconciliation outcome

**UNRESOLVED.**

The static listing yields equal `$FE00` along displacement for an ascending
Level 9 Spiker and its enemy projectile over corresponding normal active
passes. It does not statically yield projectile-faster motion. Conversely,
the repeated arcade/MAME observation supplied by Neal requires the projectile
to move faster. The listing's same inputs, same conversion, same coordinate
domain, zero fractional parts, and same `state_04` cadence do not reconcile
that conflict. No source-backed mechanism found in this bounded trace turns
the equal static rate into a faster projectile rate. The distinct `state_20`
path is only a possible state-specific scheduling caveat, not an established
explanation. Do not claim the ROM is wrong or treat its raw `$C0` bytes as
C# units.

## 16. Evidence classification

- **C# MEASURED:** Level 9 production speed formula and selected float range
  are established by the current creation path. The matrix is a deterministic
  float32 calculation of the source operations, not an arcade measurement
  and not a compiled telemetry run.
- **ROM-LISTING-BACKED:** Level 9 raw speed inputs, `crack_speed` conversion,
  field storage, movement additions, and normal active-state order described
  above.
- **ARCADE/MAME MANUAL OBSERVATION:** cross-level projectile-faster-than-
  firing-Spiker requirement and Level 9 lagging observation, supplied by Neal;
  not independently replayed in this session.
- **PROJECT TEST HYPOTHESIS:** a production invariant comparing effective
  Spiker and projectile displacement over equal active intervals is
  recommended below. It is not yet a test or accepted speed policy.
- **UNKNOWN:** a numeric mapping from ROM fixed-point units or arcade/MAME
  frames to C# depth units and simulation steps; whether `state_20` explains
  any part of the manual observation.

## 17. Recommended durable cross-level Spiker/projectile speed policy

| Policy | Assessment |
|---|---|
| **1. Each level chooses Spiker speed; require it to be strictly below projectile speed.** | Simple and flexible, but independent choices can drift; randomized policies must validate their maximum, not a sample. |
| **2. Derive Spiker movement from that level's comparable enemy authority where observed; use the shared enemy-shot authority.** | Matches accepted Level 7's Tanker/Spiker relationship. Do not assume Level 9 Spiker equals its Tanker or Flipper: its evidence does not establish that relationship. |
| **3. Common Spiker movement authority with documented per-level overrides and a relative-speed invariant.** | **Recommended.** It makes the relationship visible and reviewable, retains per-level speed differences, and can raise projectile authority when a higher level legitimately needs faster Spikers. It avoids a universal Spiker cap. |
| **4. Derive numeric speed ratios from shared ROM bytes or `crack_speed`.** | Not supported: Level 9's equal raw inputs and fixed-point calculation conflict with the manual arcade relationship, and no arcade-to-C# conversion is established. Do not use this as a policy. |

Implement a shared, testable per-level policy seam only in a separately
authorized milestone. Record whether each level has a fixed or bounded
randomized rate, its evidence classification, and the corresponding projectile
authority. Do not silently inherit the old progression formula for a new
level.

## 18. Recommended automated regression invariant

Eventually test every implemented level that can produce firing Spikers:
for each production speed choice, compare effective Spiker displacement to
the corresponding projectile displacement over the same number of active
simulation updates and assert **Spiker displacement is strictly less**.
For randomized rates, exercise the maximum representable production value.
If movement cadence differs, compare effective displacement over an
equivalent common interval, not numeric constants alone. Require an explicit
documented evidence-based exception and an explicit test assertion for any
authorized arcade exception.

The invariant should use the real production speed selector and movement
methods (or a small shared test seam), include a shot-origin/update-order
case, and assert positive separation growth. It was not added in this
read-only diagnostic.

Important baseline risk: the existing general randomized formula also feeds
Spikers on the Level 4 candidate and production Levels 5, 6, and 8; these
configurations can select speeds above the shared `3` projectile rate, and
their generic Spiker growth/retreat path can fire shared enemy bullets.
Accepted Level 7 instead uses `1.75`. The proposed invariant would therefore
surface pre-existing Level 4/5/6/8 issues as well as Level 9. No Level 1-8
change is authorized here; do not mask those failures or change the invariant
to make it pass. Handle that cross-level remediation in a separately
authorized milestone.

## 19. Smallest proposed Level 9 correction, WITHOUT IMPLEMENTING IT

Replace Level 9's accidental fallback to the old randomized selection
formula with an explicit Level 9 movement policy whose **maximum effective
ascent rate is below the current `3` projectile rate**. Preserve per-Spiker
variation if desired, but validate its maximum. Keep the shared projectile
path and rate, firing cadence, Level 7 policy, and Levels 1-8 untouched.

No evidence examined establishes the right absolute Level 9 speed or a
perceptually sufficient margin. A cap infinitesimally below 3 would satisfy
only the mechanical inequality, not Neal's "visibly faster" acceptance.
Select the actual Level 9 range/authority during the authorized
implementation-and-manual-calibration milestone; classify any provisional
value as a project translation, not ROM-backed.

## 20. Risks to Levels 1-8

- No behavior in Levels 1-8 was changed.
- Level 7's accepted Tanker-derived `1.75` relationship remains intact; do
  not retune it.
- The new cross-level invariant is expected to expose the existing random
  Spiker rates in the Level 4 candidate and Levels 5, 6, and 8. That is a
  follow-up finding, not authorization to fix those levels in this task.
- Any future shared speed abstraction must preserve existing Level 1-8
  creation, recording, and deterministic random-consumption behavior unless
  separately authorized.

## 21. Exact next implementation milestone if Neal approves

Authorize a bounded **Level 9 Spiker/projectile relative-speed correction**:

1. Add a Level 9-specific production speed policy at the existing Spiker
   creation seam; do not change shared bullet movement or Levels 1-8.
2. Ensure its worst-case effective rate is strictly below the Level 9
   projectile rate, with a meaningful margin selected for manual acceptance.
3. Add focused deterministic tests for minimum, equality boundary, maximum,
   10-step travel, increasing separation, and firing/update order.
4. Run the focused Level 9 tests and then give Neal this manual acceptance
   command; stop for his gameplay assessment:

   ```powershell
   dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1
   ```

5. Keep the broader Level 4/5/6/8 invariant failures as a separately scoped
   follow-up. Do not begin Level 10.

## 22. Recording and playback result

The exact Level 9 recording was absent, so no headless playback or endpoint
result exists. There are no frame/input counts, startup metadata, final
state, or input-consumption claims for this candidate.

## 23. Files changed by this investigation

- Added the verbatim substantive task prompt:
  [2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-prompt.md](../Prompts/2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-prompt.md).
- Added this report.
- No production source, tests, durable behavior/development documentation,
  recordings, ROMs, or other user material was changed.
- Both AI-Logs files are ignored and were not staged.

## 24. Final Git status and production-change confirmation

After saving this report, Git status was rechecked against the starting
candidate. Branch and `HEAD`/`origin/master` remain unchanged; the 17 tracked
modifications, two untracked candidate files, and empty index are the same
as at task start. The only investigation outputs are the two ignored
AI-Logs files named above.

**No production behavior changed. No tracked or test file was changed by this
investigation.**

## 25. Commit/push confirmation

No files were staged. No commit or push occurred. Level 10 was not started.
Stop here for Neal to review the evidence and explicitly authorize any
implementation milestone.
