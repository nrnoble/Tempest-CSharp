# Level 18 Shared Pulsar Movement Translation Investigation

Date: 2026-11-26

**Disposition: READY FOR BOUNDED LEVEL 18 IMPLEMENTATION.**

This is an implementation-readiness specification, not accepted Level 18
behavior. No production code, tests, or durable behavior documentation were
changed. Level 17's accepted behavior remains frozen. The only files created
are this report and its paired prompt.

## 1. Executive summary

The local Rev. 2 disassembly establishes that `$0157` is compared with the
integer byte of a Pulsar's along coordinate. It is not a timer despite the
older RAM comment calling `pulsar_fliprate` a count of movement ticks. The
coordinate normally ranges from `player_along=$10` at the rim to `$F2` at the
deep-end clamp. The threshold is therefore position-relative, not a fraction
of `$FF`.

For a Pulsar moving toward the rim, the ROM uses the Flipper speed when
`enemy_along >= threshold` (the deeper region), and the fixed Pulsar speed
when `enemy_along < threshold` (the rimward region). Moving away from the
player, the ROM uses the fixed Pulsar speed; after that movement the same
threshold determines when the Pulsar turns back toward the rim. The threshold
also gates the ROM pulse-lane kill check. Lane-turn direction does not select
the speed.

Using the known Atari endpoints and C# `Level.Depth=400`, the proposed
Level 18 threshold maps to host `Height=392.92035f`. Level 17's raw `$28`
would map to `357.52212f` for comparison only. The Level 18 threshold is about
1.77% of the Atari depth span inward from the rim, versus 10.62% for Level 17.

Recommended Level 18 speed translation:

- Preserve the accepted Level 17 fixed-speed behavior as the anchor:
  ROM `$FE.A0` (magnitude `1.375`) maps to the accepted host `1.0` unit/update.
- Preserve the Level 18 ROM ratio `2.625 / 1.375 = 21/11`; the deeper-region
  host climb speed is therefore `21/11` (`1.9090909...`) units/update.
- Use the alternate speed only on toward-rim movement in the deep region.
  Away/deep movement uses the fixed `1.0` speed.
- Keep Level 17 on the existing one-speed behavior with no active threshold.

This is an explicit **PROJECT TRANSLATION**, not literal equality between an
Atari tick and a C# simulation update. It introduces no screenshot fitting or
per-level actor. A nullable threshold and two speed values are enough to
express Level 17 compatibility and Level 18 ROM-derived movement in the same
actor. Neal's Level 18 gameplay acceptance remains required.

## 2. Starting Git/worktree state

Read-only baseline before investigation:

- Branch: `master`
- HEAD: `0ce7e538374c4e155b75cf536b4f3a67c8da3de0`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged changes: none
- Tracked worktree changes: none
- Pre-existing untracked paths:
  - `AI-Logs/Prompts/2026-11-25-level18-rom-configuration-readonly-investigation-prompt.md`
  - `AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`
  - `Reference/arcade/MBUCOD.V05.txt`

The prior untracked files were preserved. `MBUCOD.V05.txt` was not opened,
searched, or modified. The current HEAD includes the Level 17 production and
closeout commits described in the prior reports.

## 3. Prior blocker

The Level 18 ROM/configuration investigation identifies a missing shared
Pulsar movement translation: current `Pulsar.Update` uses a fixed host
increment and does not consume the ROM along threshold or its two speed
regions. Level 18 changes the threshold from Level 17's `$28` to `$14`, while
retaining fixed Pulsar speed `$FE.A0` and using the Level 18 Flipper speed in
the other region.

The prior report correctly prohibits silently treating Level 17's accepted
one-speed behavior as an already-established Level 18 translation.

## 4. Level 17 freeze boundary

The Level 17 final closeout and durable behavior records establish:

- Pulsars eligible 2-5; medium/default firing disabled.
- Current accepted C# vertical movement is one host unit per movement update.
- Existing Level 17 movement, lane/rim behavior, pulse presentation/timing,
  collision, identity, zap, completion, and rendering are manually accepted.
- The Level 17 wave data contains raw threshold `$28`, pulse beat `$04`, and
  medium firing disabled, but `Pulsar.Update` does not use a threshold or raw
  Pulsar speed.

Treat these as **EXISTING ACCEPTED PROJECT BEHAVIOR**. Do not apply a new
threshold, speed ratio, reversal rule, pulse window, or state-machine change
to Level 17 in this milestone. Its current output must remain identical.

## 5. Level 17 ROM Pulsar data

**ROM-BACKED:** threshold `$28`, fixed speed `$FE.A0` (magnitude `1.375`),
pulse beat `$04`, eligible 2-5, medium firing off and hard firing enabled by
`$40`.

**EXISTING ACCEPTED PROJECT BEHAVIOR:** one host unit per vertical movement
update. The shared actor uses the existing 51-update decision, 18-tick lane
pause, 14-tick rim pause, 12-degree flip increment, and shared pulse clock.
These host values are not a literal ROM 8.8 conversion.

## 6. Level 18 ROM Pulsar data

**ROM-BACKED, medium/default:**

- Pulsars eligible, minimum 2, maximum 5.
- Fixed Pulsar speed `$FE.A0`, signed 8.8 magnitude `1.375`.
- Alternate speed is the Level 18 Flipper speed `$FD.60`, magnitude `2.625`.
- `pulsar_fliprate` / `$0157` threshold `$14`.
- `pulse_beat=$04`.
- Pulsar firing disabled at medium/easy; hard firing uses `$40`.

Do not treat hard firing or the non-default difficulty speeds as active in the
medium/default Level 18 proposal.

## 7. `$9CB5-$9D04` trace

The relevant P-code action is `$22` (`eact_22`), entered from the Pulsar
program at `$A16B` and `$A172`. The enemy P-code engine dispatches the action
once per interpreter step. It is not a standalone wall-clock timer.

At `$9CB5` the action loads speed index 1 (Pulsar). It tests the movement
direction flag at `$028A,x`:

1. If bit 7 is clear (toward the player/rim), load the integer byte of
   `enemy_along`, compare it with `$0157`, and retain Pulsar speed for
   `enemy_along < threshold`. At equality or above, choose speed index 0
   (Flipper). It then calls `$9C63` (`eact_0c`) to add the signed 8.8 speed.
2. If bit 7 is set (away from the player/deeper), call `$9C99` to subtract the
   signed Pulsar speed. This makes the along coordinate increase. The action
   then compares the updated integer along coordinate with `$0157`. If
   enemies remain pending, it reverses direction when the updated coordinate
   is at or beyond the threshold. If no enemies remain pending, it substitutes
   `$FF`, forcing the reversal without waiting to reach the threshold.
3. Unless the pulsing value has its high bit set, the action checks whether
   along is below `$0157`. Only in that region does it continue to the player
   segment and lane-position checks that can call `pieces_death`.
4. The action returns. The P-code sequence separately manages its counter,
   pulse check, attract-to-player action, lane-flip start, and flip continuation.

`$9C63` performs the actual 8.8 arithmetic. With direction bit clear it adds
the signed speed; with bit 7 set it subtracts it. The top handler at
`$9D06` sets the enemy's integer along coordinate to `player_along`, normally
`$10`. For a Pulsar with pending enemies it toggles the movement flag and
returns. Otherwise it continues into the shared top-of-tube handling.

## 8. ROM pseudocode

Pseudocode equivalent for the threshold-sensitive action:

```text
speed = pulsar_speed

if moving_toward_player:
    if enemy_along_integer >= threshold:
        speed = flipper_speed
    move_toward_player(speed)
else:
    move_away_from_player(pulsar_speed)
    if enemies_pending == 0 or updated_enemy_along_integer >= threshold:
        reverse_vertical_direction()

if pulse_is_in_the_ROM_active_phase
   and enemy_along_integer < threshold
   and enemy_segment == player_segment
   and player_lane_position_matches:
    kill_player()
```

`move_toward_player` includes the shared top handler. This pseudocode omits
unrelated enemy-type and P-code actions; it preserves the threshold branches
and arithmetic direction.

## 9. Threshold comparison semantics

The comparison uses `enemy_along,x` at `$02DF,x`, the integer/high byte of the
8.8 position. It does not compare the fractional byte at `$029F,x`.
`pulsar_fliprate` is loaded into `$0157`, and equality takes the Flipper-speed
branch because the ROM branches to Pulsar speed only on unsigned `<`.

Therefore:

- Toward rim: `along < threshold` selects fixed Pulsar speed; `along >=
  threshold` selects Flipper speed.
- Away/deep: fixed Pulsar speed is used; post-move `along >= threshold`
  triggers reversal when enemies remain.
- Pulse hazard: the same threshold gates entry to the ROM lane/position kill
  checks; it is not merely a speed selector.

The RAM comment “Number of movement ticks between pulsar flips” is contradicted
by these direct coordinate comparisons. `$14` is a position threshold.

## 10. Speed-region orientation

Atari coordinate increases toward the deep end. Thus:

- `along >= threshold`: deep-side region; toward-rim movement uses Flipper
  speed.
- `along < threshold`: rimward/shallow region; toward-rim movement uses fixed
  Pulsar speed.
- Away/deep movement uses fixed Pulsar speed on both sides.
- C# lane direction (`Direction`) is lateral and does not select the speed.

The Level 18 Flipper magnitude `2.625` is greater than the fixed Pulsar
magnitude `1.375`, so the deep-side climb is the faster region.

## 11. Atari along-coordinate domain

The listing documents `player_along` as the player position along the tube;
normal initialization sets it to `$10`. The shared away movement clamps the
integer coordinate at `$F2` after it reaches `$F0`. The normal active range is
therefore `$10` (rim) through `$F2` (deep end), a span of `226` integer units.

The position is 8.8 fixed point: one integer/high byte and one fractional/low
byte. Pulsar movement uses signed 8.8 speed; threshold decisions use only the
integer/high byte. Lane changes update segment/lane state separately; they do
not wrap or transform the along coordinate. Movement uses common top handling
at the rim and a `$F2` clamp at the far end.

## 12. Level 17 `$28` threshold interpretation

With rim `$10` and deep `$F2`:

- Raw threshold: `$28` = 40.
- Fraction from deep end toward rim: `(242 - 40) / 226 = 0.89380531`.
- Fraction inward from rim: `(40 - 16) / 226 = 0.10619469`.
- Linear C# reference height: `400 * (242 - 40) / 226 = 357.52212`.

This is a reference mapping only. Level 17's active C# compatibility value is
not this threshold; enabling it would change accepted behavior.

## 13. Level 18 `$14` threshold interpretation

- Raw threshold: `$14` = 20.
- Fraction from deep end toward rim: `(242 - 20) / 226 = 0.98230088`.
- Fraction inward from rim: `(20 - 16) / 226 = 0.01769912`.
- Linear C# threshold: `400 * (242 - 20) / 226 = 392.92035`.
- Compared with Level 17, the threshold moves `20/226 = 8.84956%` of the
  depth span closer to the rim.

The exact C# comparison is `Height <= 392.92035` for the ROM deep-side region
at the movement boundary, and `Height > 392.92035` for the ROM rimward region.
This follows the inverted host coordinate convention and preserves ROM
equality behavior.

## 14. Current C# Height domain

`Level.Depth` is `400`. C# height is `0` at the far end and increases toward
the rim; `Level.Depth` is the rim. `Pulsar` validates `0 <= Height <= Depth`.
Simulation position is not modified by projection, calibration, tube shape,
palette, or screen-space transforms.

The current 1-unit movement and endpoint clamps are direct host simulation
values. A linear spatial mapping for the ROM threshold is justified from the
known endpoints; it does not, by itself, establish a literal ROM-tick-to-host
speed conversion.

## 15. Current C# Pulsar state machine

`Pulsar.Update` increments `TimeToFlip`, then:

- `Climbing`: adds 1 up to `Level.Depth`; after strict `>50` (the 51st update)
  it chooses a lane pause or rim pause. At the rim it makes the existing
  shared pursuit/descend decision.
- `Descending`: subtracts 1 to zero; after strict `>50` it chooses a lane or
  rim pause. At zero it chooses a lane pause or resumes climbing.
- `LanePause`: no height movement; after 18 ticks it moves one adjacent lane
  and enters `Flipping`.
- `Flipping`: advances turn angle by 12 degrees and adds 1 height when below
  the rim. `FlippingWithoutClimb` does not add height.
- `RimPause`: no height movement; after 14 ticks it resumes the stored state.
- `Terminal`: no movement.

The movement decision timer and pause periods remain accepted host behavior
for Level 17. The actor's lateral `Direction` is not Atari's vertical
toward/away flag.

## 16. ROM-to-C# threshold mapping

Use the linear endpoint mapping:

```text
hostHeight = Level.Depth * (deepAlong - along) /
             (deepAlong - rimAlong)
```

where `deepAlong=$F2`, `rimAlong=$10`, and `Level.Depth=400`. This gives the
thresholds in sections 12-13 without a Level 18 magic height or presentation
fitting.

The formula is a **PROJECT TRANSLATION** from ROM coordinate endpoints to the
C# simulation domain. It is independent of geometry projection and of the
speed translation.

## 17. Level 17 mapped threshold

`$28` maps to `357.52212f`, but must not be activated for Level 17. Keep the
current no-threshold, one-host-unit policy. The comparison value is useful for
tests/documentation only and is not an accepted Level 17 change.

## 18. Level 18 mapped threshold

`$14` maps to `392.92035f`. The host comparison is pre-move for speed
selection and post-move for the ROM descending reversal condition. At exact
boundary equality, use the deep-side branch for toward-rim movement, matching
the ROM's `BCC` behavior.

The ROM pulse hazard also uses `along < threshold`, corresponding to host
`Height > 392.92035f`. Current C# rim collision has priority within 8 host
units of the rim, so the mapped Level 18 pulse region lies inside that existing
rim band. Preserve existing Level 17 collision behavior; test and manually
judge Level 18 collision/pulse interaction rather than claiming the two
collision systems are identical.

## 19. ROM speed arithmetic

The fixed Pulsar word is set directly to `$FE.A0`, a signed 8.8 value whose
magnitude is `0x0160 / 256 = 1.375`.

The Flipper speed path uses `crack_speed`. The current Level 17 medium raw
Flipper input `$AF` resolves to `$FD.78`, magnitude `0x0288 / 256 =
2.53125`. Level 18 medium raw input `$AC` resolves to `$FD.60`, magnitude
`0x02A0 / 256 = 2.625`. These are ROM movement-unit magnitudes; applying them
per C# update remains a host translation.

## 20. Level 17 host movement baseline

The shared actor and `SimulationTests.PulsarMovesExactlyOneUnitPerClimbAndClampsAtTheTop`
establish the current `+1` climb and depth clamp. Descending uses `-1`.
Existing tests also establish the current pause, flip, bottom-reversal, and
181-call pulse behavior. The final closeout records manual acceptance of
Level 17 Pulsar behavior.

The exact host baseline is `1.0f` per active vertical movement update; no
movement occurs during lane pause, rim pause, or terminal state. The accepted
pulse phase is global to `TempestGame`, freezes during the death freeze, and is
not reset when fresh Pulsar actors reconstruct.

## 21. Level 18 speed-ratio analysis

| Comparison | Calculation | Result |
|---|---:|---:|
| Level 18 fixed / Level 17 fixed | `1.375 / 1.375` | `1.0` |
| Level 18 alternate / Level 17 Flipper | `2.625 / 2.53125` | `28/27 = 1.037037...` |
| Level 18 alternate / fixed Pulsar | `2.625 / 1.375` | `21/11 = 1.909090...` |
| Level 18 threshold from deep end | `(242-20)/226` | `0.98230088` |
| Level 17 threshold from deep end | `(242-40)/226` | `0.89380531` |

The recommended shared scale anchors the Level 18 fixed speed to the accepted
Level 17 host fixed speed: `hostScale=1/1.375=8/11`. Applying the same ratio to
the alternate speed gives `2.625 * 8/11 = 21/11 = 1.9090909...`.

## 22. Strategy A: literal host scaling from ROM 8.8 magnitudes

Mapping the ROM magnitudes directly would propose host speeds `1.375` and
`2.625`. The existing Flipper/Tanker translation demonstrates a precedent for
using an 8.8 magnitude as a fixed-step host increment, but it does not establish
an exact cadence conversion for Pulsars. The resulting Level 18 fixed speed is
37.5% faster than the accepted Level 17 host speed despite identical ROM fixed
speed data. This can be an intentional future project translation, but is not
the recommended cross-level anchor.

Assessment: structurally simple and ROM-number-literal; medium regression
risk at the Level 17-to-18 transition; no new actor; no arbitrary constant,
but does not preserve the accepted host anchor. Manual acceptance required.

## 23. Strategy B: ratio preservation against accepted Level 17 behavior

Keep Level 17 fixed movement at `1.0`. Since its ROM fixed Pulsar word matches
Level 18's, preserve that as Level 18's fixed host rate. Preserve the Level 18
ROM alternate/fixed ratio `21/11` to derive `1.9090909...` for the deep climb.

Assessment: preserves the Level 17 freeze, establishes explicit Level 18
values without visual fitting, and adds only one derived ratio. It is a
**PROJECT TRANSLATION** whose perceived rate still needs manual acceptance.
Recommended when combined with Strategy C's normalized threshold.

## 24. Strategy C: normalized threshold and configurable host speeds

Use one nullable host threshold plus the fixed and deep-climb host speeds in
shared configuration. A missing threshold means the existing one-speed
compatibility behavior. A present threshold enables thresholded movement
semantics. The threshold must be resolved from ROM endpoints, not hard-coded
per displayed level inside `Pulsar.Update`.

Assessment: best data-driven architecture; same actor and lifecycle; focused
tests can assert boundary and speed selection; Level 17 compatibility is
explicit. The optional threshold is the only mode distinction needed.
Recommended, paired with Strategy B's speed values.

## 25. Strategy D: retain the existing one-speed approximation

Using `1.0` everywhere for Level 18 ignores the documented `$14` region,
Level 18 Flipper speed, threshold movement from Level 17, and the threshold's
ROM reversal/pulse uses. It is not defensible as an implementation of this
blocker. It would only be acceptable as a separately named, explicitly
temporary project approximation if Neal authorizes that narrower behavior.

## 26. Strategy E: endpoint-scaled speed

Scaling ROM speed by the spatial endpoint ratio `400/226` would yield host
fixed speed about `2.4336` and alternate speed about `4.6460` per update.
This assumes one ROM movement call equals one C# update and does not preserve
Level 17's accepted `1.0` speed. The endpoint ratio justifies position mapping,
not the timing conversion. Reject for this milestone.

## 27. Recommended translation

Use shared configuration with:

- no active threshold for Level 17; fixed and fallback host speed both
  `1.0f`;
- Level 18 threshold `392.92035f`;
- Level 18 fixed host speed `1.0f`;
- Level 18 deep-side toward-rim host speed `21/11` (`1.9090909...`);
- away/deep movement at fixed host speed;
- the same threshold for the ROM-derived descending turn boundary and pulse
  region, subject to the explicitly stated host collision approximation;
- existing shared state machine, pulse clock, rim lifecycle, identity, and
  reconstruction mechanisms.

The speed resolver uses the coordinate before a toward-rim move. The ROM
deep-side branch is `Height <= threshold`; after the movement crosses the
boundary, the next call uses fixed speed. On an away/deep move, use the fixed
speed and evaluate the threshold against the post-move position. The
pending-enemy condition used to force the ROM return when no enemies remain
must use the shared Level pending-supply representation and be covered by
tests; it must not become a level-number check.

The existing C# pause/turn cadence remains a host translation. Do not assert
that its 51-update decision or 18/14-tick pauses are ROM timing. The ROM's
threshold-dependent return and pulse effects are not silently reclassified as
speed-only behavior.

## 28. Level 17 compatibility design

No separate actor or Level 17 subclass is needed. A null threshold and the
existing `1.0f` movement value select the compatibility path. The path must
retain the current state/order and produce identical height, lane, pause,
pulse, collision, and rim outputs for the same deterministic inputs.

Do not use the Level 17 raw `$28` as an active host threshold. Do not add a
`if (displayedLevel == 17/18)` branch in `Pulsar.Update`.

## 29. Current Pulsar constant classifications

| Existing value/state | Current meaning | Classification / handling |
|---|---|---|
| `+1` climb, `-1` descend | Current fixed-step movement | Existing accepted Level 17 PROJECT TRANSLATION; preserve |
| `TimeToFlip > 50` | Decision on update 51 | REFERENCE-DERIVED / PROJECT TRANSLATION; preserve |
| Lane pause 18 | Host pause before lane transition | REFERENCE-DERIVED / PROJECT TRANSLATION; preserve |
| Rim pause 14 | Host rim/turn pause | REFERENCE-DERIVED / PROJECT TRANSLATION; preserve |
| Flip angle step 12 degrees | Host directed-turn presentation | REFERENCE-DERIVED / PROJECT TRANSLATION; preserve |
| Pulse phase 181 calls | Shared host pulse cycle | PROJECT TRANSLATION; manually accepted for Level 17 |
| Pulse active phase 71-109 | Host hazard/audio window | PROJECT TRANSLATION; preserve |
| `PulseBeatRaw=$04` | ROM oscillator input | ROM-BACKED raw data; not currently consumed by host clock |
| `PulsarFlipRateRaw=$28` | Level 17 ROM threshold | ROM-BACKED raw data; not currently consumed by actor |
| Pulse-white/base palette state | Transient vector presentation | Existing accepted PROJECT TRANSLATION; unaffected |
| Rim proximity `<8` | Host rim-collision region | Existing C# behavior; not the ROM threshold |
| `IsLanePulsing(height)` uses `height>16` | Helper threshold | Existing host/test helper, not the ROM `$0157` translation |
| Medium firing disabled | Level 17 policy | ROM-BACKED and existing accepted configuration |
| `Terminal=7`, state identifiers | Actor state values | Existing implementation detail; not a new ROM claim |

No unrelated accepted timing, firing, pulse color, or rendering constants need
to become Level 18 movement fields.

## 30. Proposed configuration surface

Add a small semantic movement value to the shared level configuration, not a
new actor lifecycle. Exact type/placement can follow existing project naming;
the minimal fields are:

| Field | Source and host value | Level 17 | Level 18 | Owner |
|---|---|---|---|---|
| Optional threshold height | ROM `$0157`; endpoint mapping above | `null` for compatibility; raw `$28` remains documented | `392.92035f` from raw `$14` | `LevelConfiguration` via a small Pulsar movement record |
| Fixed movement speed | ROM `$FE.A0` -> `1.0f` host anchor | `1.0f` | `1.0f` | Same shared movement record |
| Deep-side climb speed | Level Flipper 8.8 speed times shared `8/11` scale | ignored by null-threshold path; compatibility remains 1 | `21/11` (`1.9090909...`) from `$FD.60` | Same shared movement record |

Store Level 18 raw `$14`, `$04`, `$AC`, and eligibility/firing values in its
wave-selection/configuration source as ROM data. `pulse_beat=$04` and firing
policy are not movement-record fields. Do not add configurable pause durations,
turn cadence, pulse timing, geometry, palette, display-level branching, or a
general 8.8 converter for this bounded change.

## 31. Movement-state applicability

- **Climbing:** choose fixed versus deep-side speed from the pre-move height
  and the mapped threshold.
- **Descending:** fixed Pulsar speed regardless of region; evaluate the mapped
  threshold after movement for the return condition. If no generic enemies
  remain pending, translate the ROM immediate-return condition through the
  shared pending-supply abstraction.
- **Flipping:** where the current host state actually changes height, use the
  toward-rim resolver; retain its existing decision/turn order. `FlippingWithoutClimb`
  does not move vertically.
- **LanePause / RimPause:** no movement resolver call and no height change.
- **Rim arrival:** clamp at `Level.Depth` and retain shared rim behavior. ROM
  top handling uses `player_along=$10` and is separate from `$0157`; do not
  create a Level 18 rim branch.
- **Pulse hazard:** the ROM threshold gates its pulse collision check. Reuse
  the resolved threshold for the Level 18 profile while keeping Level 17's
  existing collision path untouched. The C# 8-unit rim result takes priority;
  verify the overlap at Level 18.
- **Terminal:** no movement or threshold evaluation.

The threshold is an along/depth parameter only. It must not alter lateral
lane-change direction, lane topology, or movement during pauses.

## 32. Rendering separation

This is simulation-only. `Height` remains the actor's gameplay depth; the
renderer consumes the resulting position. Do not couple the threshold or
speeds to projection, calibration, tube geometry, palette, pulse-white
rendering, or displayed level. No presentation change is required.

## 33. Identity, ledger, and reconstruction impact

Movement policy is level runtime configuration, not identity architecture.
Keep the existing Pulsar original identity and ledger states. New or
reconstructed Pulsars resolve the current `LevelConfiguration` movement value
when created; they do not carry a stale level-specific movement mode in their
identity.

On life loss, the existing code discards active Pulsar runtime objects and
rebuilds unresolved identities as Eggs. A newly hatched Pulsar receives the
same level configuration and fresh movement timers. The shared pulse clock
remains on its current lifecycle; do not reset it as part of this movement
change. Super Zapper, collision, completion, and ledger transitions are
unchanged.

## 34. Deterministic-test specification

Future focused tests should include:

1. **Level 17 compatibility:** deterministic movement/state fingerprint
   unchanged; exact `+1/-1` increments; existing 51-update decision, lane and
   rim pauses, bottom reversal, pulse phase, and collision boundaries remain.
2. **Threshold mapping:** `$28 -> 357.52212f` reference and `$14 ->
   392.92035f` active value; verify endpoints and inverse orientation.
3. **Boundary:** pre-move heights on deep side, exact equality, and rimward
   side select the expected increment; crossing changes the next climb's
   selected speed.
4. **Direction:** fast speed applies only on toward-rim motion in the deep
   region; descending remains fixed speed on both sides; no lane-direction
   inversion of the speed choice.
5. **Threshold reversal:** pending supply returns at the post-move boundary;
   no pending supply follows the explicit immediate-return translation.
6. **Pulse interaction:** mapped threshold bounds the ROM-derived pulse
   region; existing Level 17 hazard/collision behavior is unchanged; Level 18
   rim-priority behavior is explicit.
7. **Pauses/transitions:** no movement during lane/rim pauses or terminal;
   turn state changes height only on existing movement updates; lane transition
   remains deterministic.
8. **Lifecycle:** reconstruction uses the same level configuration with a
   fresh Pulsar; Super Zapper, collision, identity conservation, and completion
   remain unchanged.

Do not run these tests during this read-only investigation. Do not weaken
existing tests to make the Level 17 fingerprint pass.

## 35. Manual-acceptance specification

After implementation and focused automated validation, Neal should judge:

- whether Level 18 Pulsars move naturally relative to other Level 18 enemies;
- whether the deep-side climb is observably faster and the rimward transition
  feels natural;
- whether reversal and lane changes remain natural;
- whether pulse/hazard behavior remains normal near the rim;
- whether rim pursuit and survival behavior remain normal;
- whether overall gameplay pressure is comparable to arcade/MAME;
- whether Level 17 remains unchanged.

These are experiential questions, not requests to judge ROM ticks or exact
decimal speeds. No screenshots are required absent a later visual issue.
Copilot must not launch the game for acceptance.

## 36. Evidence-classification table

| Finding | Classification |
|---|---|
| `$0157` comparisons, direction branches, signed movement arithmetic, top/deep handling, pulse checks | ROM-BACKED |
| Level 17 accepted one-unit movement and accepted pulse/lane/rim behavior | EXISTING ACCEPTED PROJECT BEHAVIOR |
| Linear `$10..$F2` to `0..400` threshold mapping | PROJECT TRANSLATION |
| Level 18 fixed host rate `1.0` anchored to accepted Level 17 | PROJECT TRANSLATION |
| Level 18 alternate host rate `21/11` preserving ROM speed ratio | PROJECT TRANSLATION |
| Host pause/flip/pulse timing retained from existing actor | REFERENCE-DERIVED / PROJECT TRANSLATION |
| C# thresholded pulse collision overlap with rim band | INFERENCE requiring tests/manual acceptance |
| Exact host perceptual fidelity for Level 18 | UNKNOWN until Neal's manual acceptance |
| Visual projection/calibration effects on movement | None; simulation/rendering are separate |

## 37. Readiness result

## READY FOR BOUNDED LEVEL 18 IMPLEMENTATION

ROM branch semantics, threshold domain/orientation, host threshold mapping,
host speed ratio, Level 17 compatibility, shared actor/configuration shape,
state applicability, and focused validation requirements are established.
The proposed values are explicit PROJECT TRANSLATIONS; readiness does not
claim Level 18 manual acceptance.

## 38. Exact blockers

None for the bounded shared Pulsar movement implementation. Manual gameplay
acceptance remains a required post-implementation gate, not a research
blocker. Do not broaden this milestone into Level 17 fidelity reconsideration
or redesign of the Pulsar lifecycle.

## 39. Files/resources inspected

- `AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`
- `AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`
- `Reference/arcade/Tempest Rom Source code.md`
- `src/Tempest.Game/Gameplay/Pulsar.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Levels/Level.cs`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level17WaveSelection.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`
- User-provided task attachment, copied unchanged to the paired prompt.

Protected videos, screenshots, recordings, `TEMP/` content, the private
scratchpad, raw ROM images, and the untracked `MBUCOD.V05.txt` were not
inspected.

## 40. Commands/calculations run

- Read-only Git status, upstream/HEAD, staged/worktree summary, and recent
  commit history.
- Bounded `rg` and source-range reads for the ROM routine, parameters,
  existing C# actor, configuration, and tests.
- Python exact-fraction calculations for `$10..$F2` threshold mapping and
  speed ratios:
  - Level 17: `40400/113 = 357.52212389...`
  - Level 18: `44400/113 = 392.92035398...`
  - fixed speed ratio `1`
  - Level 18 alternate/fixed ratio `21/11`
  - Level 18 alternate/Level 17 Flipper ratio `28/27`
- No tests, builds, game launch, recording playback, screenshot/video
  inspection, ROM-image access, or Internet search.

## 41. Production code changed: no

No production code was changed.

## 42. Tests changed: no

No tests were changed or run.

## 43. Durable docs changed: no

No durable behavior or development documentation was changed.

## 44. Stage/commit/push: none

Nothing was staged, committed, or pushed. The only new files are the required
paired prompt and report.

## 45. Level 18 implementation: none

No Level 18 configuration or actor behavior was implemented.

## 46. Recommended next step

Proceed with a bounded Level 18 production implementation using the shared
Pulsar and the configuration/translation above. Preserve Level 17's current
one-speed behavior exactly. Run the new focused deterministic tests and
relevant Level 17 regressions, then stop for Neal's Level 18 manual gameplay
acceptance.
