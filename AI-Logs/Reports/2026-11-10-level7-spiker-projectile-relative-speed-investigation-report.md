# Level 7 Spiker/projectile relative-speed investigation

Date: 2026-11-10

Disposition: **READ-ONLY INVESTIGATION COMPLETE.** Relative-speed mismatch is
confirmed in C#; the exact arcade-to-C# speed translation remains unresolved.
The current Level 7 candidate was preserved.

## Result and supplied observation

**ARCADE/MAME OBSERVED (Neal):** an enemy shot can catch and pass a Tanker;
a Spiker projectile moves at a perceptibly faster, constant along-tube rate
than its firing Spiker, and the gap grows. A Spiker does not catch its own
shot. The illustrative 10/15 mph example is qualitative only and is not used
as a ratio or tuning value.

**PROJECT-CURRENT:** C# enemy bullets advance at a constant `+3` height units
per active update. A canonical Level 7 Spiker's fixed per-instance speed is
`6/48 + 2.5 + random*2`, or `[2.625, 4.625)` height units per active update.
Consequently, during ascent:

```text
change in (projectile height - Spiker height) = 3 - Spiker.Speed
```

For speeds below 3 the projectile pulls ahead; at 3 the gap is unchanged;
above 3 the signed gap shrinks and the Spiker catches/overtakes its shot.
This confirms the visible C# defect for the faster part of the production
Spiker speed range. The speed is constant for an individual Spiker and does
not vary with its growth.

**ROM/MAME CONFLICT:** the literal fixed-point arithmetic in the checked-in
Rev. 2 listing predicts that a Level 7 Spiker moves four times as fast as an
enemy shot in the same direction and at the same per-pass cadence. That
prediction is opposite Neal's reproduced MAME observation. The source does
not provide a defensible exact C# speed target, so this report does not select
or recommend a production speed value.

## ROM trace

### Level 7 inputs and conversion

`curlevel=6` represents displayed Level 7; the parameter loader selects
effective table level 7. The local parameter tables supply:

| Quantity | Level 7 raw table byte | After `crack_speed` |
|---|---:|---:|
| Spiker (`spd_spiker_lsb`) | `$00` | `$F800` = signed 8.8 `-8.0` |
| Enemy projectile (`enm_shotspd_lsb`) | `$C0` | `$FE00` = signed 8.8 `-2.0` |

The table entries are `$00` at `9607_5f` and `$C0` at `9607_63`. The Level
parameter loader applies `crack_speed` to both and stores the returned A byte
in the named LSB and Y in the named MSB. `crack_speed` initializes its
high-byte accumulator to `$FF`, shifts the input/high-byte pair left three
times, and returns the pair used by the movement additions. Thus its effective
conversion is `(0xFF00 | raw) << 3`, modulo 16 bits.

The listing's prose comment immediately above `crack_speed` reverses which
return register it calls MSB versus LSB. The actual call-site stores and
subsequent low-then-high additions establish the byte order used in the
table above; following that comment literally would misread the values.

The speed adjustment branches at `$9328` change `spd_flipper_lsb` and other
difficulty-dependent parameters, but do not modify the Spiker or
enemy-projectile input before their conversions at `$9382` and `$9391`.
Their Level 7 values therefore do not change between the difficulty branches.
The Tanker comparison does change with difficulty because Tanker shares the
Flipper speed; even those variants do not make the listed projectile faster
than the listed Tanker.

### Representation, movement, and cadence

- **Spiker:** its `$00` p-code entry at `$A0F7` executes `move per speed`
  (`eact_0c`), followed by spike growth/reversal/conversion (`eact_0e`).
  `move_enemies` walks each occupied enemy slot and runs its p-code through
  the halt operation on every `state_04` gameplay pass. The Spiker's type
  selects its speed from the contiguous speed table. During ascent the
  16-bit along position adds `$F800` (decreasing by 8.0); on retreat the
  reverse-direction branch subtracts that negative value. The growth check
  changes the Spike and lifecycle, not the along-tube step size.
- **Enemy projectile:** `state_04` calls `move_shots` once after
  `move_enemies`. Each active enemy-shot slot adds `enm_shotspd_lsb` to its
  low position byte and adds `enm_shotspd_msb` to its high position byte,
  carrying between bytes. At Level 7 this is `$FE00` per pass (decreasing
  along by 2.0).
- Both paths use signed 8.8 along coordinates, low-byte fractional state,
  high-byte along position, the same coordinate orientation, and one
  movement operation per active gameplay pass. The speed fractions are zero
  for these two Level 7 values, so no fractional accumulation can reverse the
  relative rate. The projectile slot's low position byte is separate state;
  its creation routine does not explicitly clear that byte, but with a zero
  fractional speed it does not change the per-pass rate.
- `move_enemies` precedes `move_shots`, which precedes `enm_shoot`. A shot is
  created at the Spiker's already-moved along position and first moves on the
  next gameplay pass. It then moves independently of the firing Spiker.
  `move_shots` removes an enemy shot when it reaches the player's along
  endpoint; this is the ROM endpoint, not a basis for equating the C# rim
  coordinate.

The literal source relationship is therefore:

```text
projectile / ascending-Spiker along-rate = 2 / 8 = 0.25
```

It says the Spiker catches its shot, contrary to the MAME observation. The
source's arithmetic contains no identified missed complement, sign reversal,
fixed-point carry, update-frequency difference, wrong Level 7 table byte, or
Spiker/projectile difficulty adjustment that resolves this contradiction.
The annotated listing itself remains authoritative evidence for its code, but
not proof that this static interpretation matches observed execution.

The prior `0.865` calculation was projectile versus Tanker
(`2 / 2.3125`), not projectile versus Spiker. It is arithmetically correct for
the literal Medium per-pass values in the listing; both are the same fixed
point representation and move once per pass. It is not an arcade fact and
does not match the observed projectile overtaking a Tanker. The difficulty
variants of Tanker speed do not resolve that observation either. The
disagreement is an unresolved source-versus-execution conflict, not evidence
that the ratio omitted a sign, conversion, or cadence.

Primary local source references: [parameter tables](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2717),
[parameter conversion and difficulty handling](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2628),
[crack_speed](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2677),
[active gameplay call order](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3016),
[enemy movement interpreter](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3554),
[eact_0c](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3706),
[Spiker p-code](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4243),
and [enemy-shot update/expiry](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4411).

## C# trace

### Movement and shot timing

- Canonical Level 7 Spikers are constructed with speed
  `ProgressionIndex / 48f + 2.5f + random.NextDouble() * 2` in
  `TempestGame`; Level 7's progression index is 6. The chosen value is held
  in `Spiker.Speed`. In `Building`, the Spiker adds that speed to `Height`
  once per update. In `Retreating`, it subtracts `Speed * .90f`. Spike
  persistence affects whether `Growth` increases and when retreat logic may
  run; it does not alter physical ascent speed.
- Each active `RunOneStep` advances existing enemy bullets first (`+3`),
  then later updates Spikers once. The Level 7 firing phase runs after
  Spiker movement and creates the bullet at the Spiker's post-movement
  `Height`. Because bullet advancement already ran, a newly created bullet
  first advances on the next active step. Existing bullets continue
  regardless of Spiker retreat/removal; C# expires them only after their
  height exceeds `Level.Depth`.
- The two objects use the same increasing C# `Height` coordinate and active
  simulation-step cadence. Rendering projects the bullet and Spiker from
  their respective updated heights on the same lane geometry. No coordinate
  conversion explains the changing relative gap. The Spiker renderer does
  apply a render-only retraction offset during `PlayerDying`; that offset is
  zero during the reported active ascending gameplay and does not change
  either object's simulation height.
- During ascent, the exact signed relative advance is `3 - Speed` per
  comparable active update. During retreat, the Spiker moves downward while
  the bullet continues upward, so separation increases by `3 + 0.9*Speed`
  per update while both remain comparable. Pending-supply restart begins a
  new Building phase at height zero with the same Spiker speed.

### Candidate attribution and cycle comparison

The current uncommitted repeated-cycle firing correction did **not** change
Spiker movement speed, projectile speed, update ordering, shot origin, or
first-movement timing. Its diff adds a Spiker firing cooldown and a separate
canonical-Level-7 firing eligibility phase, and disables the old
growth-retreat shot branch for that canonical policy. Both old and new
creation paths produce a bullet at the Spiker's post-movement height after
the shared bullet-update pass; a created bullet first moves next step.

The relative-speed defect is therefore present whenever a Level 7 Spiker
fires while ascending with `Speed > 3`, not a speed change introduced by the
repeated-cycle correction. The correction made shots available during more
observable ascent opportunities, including later cycles, exposing the
pre-existing mismatch. The same per-instance speed and `3 - Speed` relation
apply on first and later ascents. Persistent Spike height and the pending
restart do not change that rate. The firing correction only gates newly
created shots to Building; already-created shots continue during retreat.

### Tanker sanity check

C# Tankers climb at `+1.75` height units per update while enemy bullets
advance `+3`, so a bullet gains `1.25` units per comparable update and can
overtake a Tanker. This agrees qualitatively with Neal's MAME observation.
A future Spiker-only change that leaves projectile speed and Tanker movement
alone would preserve this existing C# relationship; the current evidence
does not justify choosing the Spiker's new value.

Relevant current source: [Spiker](../../src/Tempest.Game/Gameplay/Spiker.cs#L19),
[EnemyBullet](../../src/Tempest.Game/Gameplay/EnemyBullet.cs#L17),
[active update ordering](../../src/Tempest.Game/TempestGame.cs#L308),
[Level 7 Spiker construction](../../src/Tempest.Game/TempestGame.cs#L932),
[Level 7 firing phase](../../src/Tempest.Game/TempestGame.cs#L1795),
[Tanker movement](../../src/Tempest.Game/Gameplay/Tanker.cs#L54),
[Level 7 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L293),
and [vector projection](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L599).

## Existing test coverage and gap

| Behavior | Existing coverage |
|---|---|
| Spiker initial/first movement | `SpikerHatchesOnEggLaneWithNoMovementOnFirstUpdate` checks 0 then 3 in the development-wave setup; `SpikerInitialUpdateDoesNotCreateSpikeAndNextUpdateCreatesAtOwnHeight` checks explicit-speed updates. |
| Enemy-bullet movement | `EnemyBulletAdvancesThreeAndExpiresOnlyAfterPassingDepth` checks one and later advances; production rendering tests check the same advancing state over multiple steps. |
| Shot origin | `FirstAscentCanFireWithoutGrowthOrRetreat` asserts the created Level 7 bullet shares the Spiker lane and height. |
| First-movement timing | The shared update order implies next-step movement; existing bullet rendering tests cover a created Flipper bullet, but no Level 7 Spiker test explicitly asserts its first post-creation step. |
| Relative separation over multiple steps | **No coverage found.** |
| Later-cycle firing | `SameSpikerCanFireOnSecondAscentWithHighPersistentSpike` checks the second-ascent shot and retained Spike, but not its relative travel. |
| Tanker/projectile relative speed | **No direct relative-position assertion found.** |
| Deterministic playback | Playback tests compare Spiker and enemy-bullet state snapshots across runs, but do not assert the expected separation or arcade relationship. |

The smallest future regression, after calibration, should create a
deterministic canonical Level 7 Spiker and shot on the same lane, capture both
post-creation positions, advance bounded active steps, and assert the
projectile-to-Spiker gap increases without a catch during ascent. Repeat after
a pending-supply restart/later ascent, and retain a Tanker/bullet relative
check so the existing C# overtaking relationship remains unchanged. Test the
translated target only after a speed value is established; do not encode the
illustrative 10/15 mph example.

## Conclusion and recommended next milestone

**D. RELATIVE MISMATCH CONFIRMED, EXACT TRANSLATION UNRESOLVED.** C#'s
per-update values explain why some Spikers catch their own shots. The ROM
trace, however, predicts the opposite arcade relationship, and the verified
MAME observations conflict with that literal interpretation. Neither
lowering Spiker speed nor raising projectile speed has a source-backed exact
target here.

**Exactly one recommended milestone:** add a bounded, development-only Level
7 relative-speed calibration diagnostic. It should record Spiker and shot
positions at fixed active-step intervals for a deterministic same-lane
ascent, while Neal records equivalent MAME frame intervals and the
qualitative gap change. Use that comparison to select and then test the
smallest calibrated Spiker-only correction, if supported; keep production
movement, projectile speed, firing cadence/pressure, Tankers, and unrelated
Level 7 systems unchanged until then. This is a diagnostic milestone, not an
authorization to tune now.

## Validation and integrity

- `HEAD` remains `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`.
- The current candidate's tracked and untracked source/test files were
  SHA-256 recorded before report creation and rechecked afterward; they are
  unchanged. The Git index remains empty/unchanged.
- `git diff --check` passes. No tests or builds were rerun, as instructed.
- Outstanding prior validation remains: the new Level 7 Spiker firing tests
  passed (6); focused regressions passed (123); affected Release regressions
  passed (804); the previous full Release suite had 1,217 passed and one
  failure, `GameOverDensityDiagnosticTests.OmittedAndExplicitTwoRenderIdenticalGameOverMarks`.
  That failure was not investigated. The Release build was not run after it.
- Manual Level 7 acceptance remains pending. No interactive game was
  launched. No production code, tests, durable behavior documents, or
  protected local material were modified. Only the required prompt copy and
  this investigation report were created; no staging, commit, or push.
