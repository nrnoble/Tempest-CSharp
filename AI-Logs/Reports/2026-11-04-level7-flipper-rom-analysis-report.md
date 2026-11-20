# Level 7 Flipper ROM movement investigation

Date: 2026-11-04. Read-only analysis; stop for Neal's review.

## 1. Verified starting HEAD and scope

Required and verified HEAD:
`79e33886772424a31973332f33f3781b55e4c2f8`.
Starting status was `## master`; unstaged and cached diff statistics were
empty. The tracked worktree and index passed the fail-closed baseline.
Exactly one complete standalone completion token was present in the task.

Read the complete
[preimplementation report](2026-11-04-level7-preimplementation-analysis-report.md),
project instructions, relevant behavior/development sections, and the ROM,
Incitti, configuration, Flipper, transition, geometry, projection, Tanker and
game-orchestration source paths cited below. Preserved the substantive task
in the corresponding [prompt record](../Prompts/2026-11-04-level7-flipper-rom-analysis-prompt.md),
excluding appended agent/VS Code context.

Evidence order for this investigation is the task's order: established arcade
observations, ROM source/tables, Incitti, accepted C#, new decisions.
ROM-BACKED below means a static trace of the checked-in annotated disassembly,
not observed arcade execution or independent verification against ROM binaries.
Some tables use `.chunk` with values only in comments; the coordinate/angle
tables omit their contents entirely. These limitations matter.

No evidence here reopens accepted Levels 1-6.

## 2. Exact ROM movement program

**Verified:** entry offset `$24` into the p-code at `$a0f7`, physical address
`$a11b`, implements:

1. Move along the tube once; halt this movement invocation.
2. Set remaining flips to 2.
3. Start an adjacent flip; halt.
4. On subsequent movement invocations, advance its angle and move along the
   tube; repeat until angular completion.
5. Halt on the completing invocation.
6. On the next invocation, decrement remaining flips. If nonzero, start the
   next flip immediately in that same invocation.
7. After the second completed flip, toggle direction, load 3, start the first
   opposite-direction flip in that same invocation.
8. Repeat the completion/decrement process for three flips.
9. After the third completed flip, toggle direction, branch back to `$24`,
   perform the cycle's one lead-in along-movement, and halt. On the following
   invocation, load 2 and start the next flip.

The route is therefore **2, reverse, 3, reverse, repeat**, not random
left/right choices, not player pursuit, and not a universal below-rim policy.
Entry offset `$24` is not opcode `$24` (the separate player-direction action).

Source:
[program bytes, ROM:4283](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4283);
[interpreter, ROM:3565](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3565);
[opcode handlers, ROM:3640](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3640).

For an ordinary Flipper starting at ROM segment S, the first ten completed
destinations, modulo 16, are:

```text
S-1, S-2, S-1, S, S+1, S, S-1, S, S+1, S+2
 \--2--/ \-----3-----/ \--2--/ \------3------/
```

Each five-flip cycle drifts one ROM segment in the direction opposite to the
initial two-flip direction. This describes completed lane destinations, not
the temporary internal segment assignment during a flip.

## 3. Exact displayed-Level-7 selection path

1. `curlevel=6` represents displayed Level 7. `$92c5-$92d4` copies it and
   increments the effective table level to 7.
2. `$92d6-$9326` resolves parameter-table source/destination pointers.
   The pair at `$965f/$9661` selects `9607_5b` and `flipper_move`.
3. `9607_5b` has table opcode `$06`, level range 1..99, and the repeating
   16-level entries:
   `07 0b 19 24 53 0b 24 19 53 87 24 19 53 07 87 24`.
4. Parameter opcode `$06` computes `((level-1)&15)+1`; at 7 it selects the
   seventh byte, `$24`. `$931b` stores the result into `flipper_move`.
5. Ordinary Flipper selection at `$9a9d-$9aa7` loads `flipper_move` into
   the creation parameters, with type/direction zero.
6. Allocation at `$9981-$998b` writes the selected PC to `enm_move_pc[enemy]`
   and type/direction to `$0283[enemy]`.
7. `move_enemies` loads each enemy's saved PC, executes through a halt, then
   saves the resulting PC separately for that enemy.

Sources:
[effective level, ROM:2547](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2547),
[selection table/pointers, ROM:2856](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2856),
[parameter opcode, ROM:2952](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2952),
[creation, ROM:3482](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3482),
[allocation, ROM:3293](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3293).

This same table selects entry `$24` on displayed Level 4. That is a source
comparison only, not permission to change accepted C# Level 4.

## 4. Initialization, first phase, and initial direction

### Ordinary originals

- Pending-enemy materialization supplies along position `$f0` and its pending
  segment. Pending segments may be random; that does not randomize direction.
- `$9aa5/$9af6` supplies type/direction `$00` for an ordinary Flipper.
  Consequently direction bit `$40` is **clear**, fixed independently of
  player position, spawn lane, RNG, previous occupant's direction, or player
  identity on this path.
- `$028a` receives `$40`: shooting enabled, along motion toward the player,
  no Tanker split action.
- `enm_move_pc` starts at `$24`; not flipping.
- `$02cc` initially receives `(segment+1)&15`, the resting endpoint encoding.
- `shot_delay` starts at zero.
- The route counter `$0298[enemy]` is **not explicitly cleared by allocation**.
  This is safe for this program: its first invocation performs only the
  lead-in move; the next loads 2 before the counter is read.
- Likewise the allocation fragment does not explicitly initialize the
  fractional along accumulator. Do not invent a fresh-zero ROM guarantee.
  This does not affect the fixed initial phase/direction conclusion.

Thus the first phase is definitely **two flips toward decreasing ROM segment
indices**, not three and not an initial shortest-path choice. On closed
clover, the edge-reversal routine does nothing. Direction is unchanged within
each block until the explicit `$16` action toggles `$40`.

Sources:
[pending materialization, ROM:3273](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3273),
[allocation, ROM:3293](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3293),
[Flipper parameters, ROM:3482](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3482),
[reverse action, ROM:3699](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3699),
[open-edge guard, ROM:3974](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3974).

Cargo initialization differs; see section 10.

## 5. ROM-to-C# direction mapping

Do not confuse `$0283` direction bit `$40` with `$028a` firing bit `$40`.
They are separate arrays with separate meanings.

| Representation | Route meaning |
|---|---|
| ROM `$0283 & $40 == 0` | Completed destination is previous ROM segment, S-1 |
| ROM `$0283 & $40 != 0` | Completed destination is next ROM segment, S+1 |
| C# `Direction=+1` | `PreviousEdge`, decreasing C# lane index |
| C# `Direction=-1` | `NextEdge`, increasing C# lane index |

ROM `$12` temporarily increments the segment at flip start for set-bit
direction; `$14` decrements it at completion for clear-bit direction.
Neither early internal assignment changes the completed-destination mapping.
Source:
[continue/complete, ROM:3842](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3842),
[start, ROM:3940](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3940),
[C# routing, Flipper.cs:198](../../src/Tempest.Game/Gameplay/Flipper.cs#L198),
[topology, Level.cs:399](../../src/Tempest.Game/Levels/Level.cs#L399).

**Confirmed for the imported Incitti clover coordinates:** indices increase
clockwise on the C# screen, since its source-ordered polygon has positive
signed double area 204128 in screen coordinates and projection retains the
sign of Y. Therefore C# `+1` is counterclockwise and `-1` is clockwise on
this board. Do not apply a generic helper's variable names to every board.
Sources:
[clover coordinates, levelset1.txt:15](../../Reference/incitti/levelsets/levelset1.txt#L15),
[projection, Projection.cs:16](../../src/Tempest.Game/Rendering/Projection.cs#L16).

**Not established by the available listing:** whether increasing ROM clover
indices have the same physical screen handedness as increasing imported C#
indices, or the exact rotational lane-number offset. `lev_x`, `lev_y`,
`lev_angle` and `lev_remap` are unexpanded `.chunk` declarations. The loader
copies their order but does not reveal the missing values.
Sources:
[omitted tables, ROM:7461](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L7461),
[copy/remap, ROM:8252](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L8252).

If lane order is preserved (possibly with a cyclic offset), clear maps to C#
`+1` and set maps to `-1`. If lane order is reversed, those physical mappings
swap. **Do not certify initial C# `+1` as Atari-counterclockwise without
establishing this correspondence.** Arcade footage following ordinary
Flippers from their first flip can settle physical handedness without needing
to recover Atari's private lane numbering.

## 6. Flip-count and reversal semantics

There is no incrementing two/three counter. `$02` loads a **remaining count**
into `$0298[enemy]`; `$08` pre-decrements it and branches when nonzero.

For phase two:

```text
26: load 2
28: start
29: halt
2a: continue/end
2b: move along
2c: if complete, skip branch at 2d/2e
2d: branch to 29 while still flipping
2f: halt after completion
30: decrement remaining; branch to 28 if nonzero
32: reverse
33: load 3
35: start opposite-direction flip
36: halt
```

The three-flip block is analogous at `$37-$3f`; `$40` branches to `$24`
using operand `$23`, because the interpreter increments PC after every action.
Likewise branch operand `$28` targets the halt at `$29`, not start at `$28`.

The counter is charged **once per completed angular flip**, on the following
movement invocation. Starting a flip or advancing it through many angles does
not charge the counter. Destroying an enemy mid-flip discards that enemy; it
does not charge a shared phase or carry a partial flip into a fresh entity.
Death freezes execution until reconstruction; an incomplete flip is not
resumed as a counted fresh flip.

Reversal occurs on the next invocation after the phase's last completion,
before starting the next phase. It does not happen mid-pivot or at the moment
of attempting a turn. A rim handoff can replace the below-rim continuation
before its pending counter decrement; the discarded count then has no effect.

Sources:
[counter and branch handlers, ROM:3643](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3643),
[program, ROM:4286](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4286),
[completion flag, ROM:3890](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3890).

## 7. Flip trigger, cadence, and along movement

**Static cadence is known in movement invocations.** A flip starts when the
program reaches `$12`, not after a random decision, height interval, player
retarget, Spike-tip crossing, or arbitrary between-lane timer.

One invocation of `move_enemies` visits each live enemy and runs its program
until opcode `$00` halts that enemy. Halting is not a permanent pause: the
saved PC resumes on that enemy's next movement invocation.

Away from rim handoff:

| Invocation kind | Angular action | Along action |
|---|---|---|
| Initial/cycle lead-in | None | One `$0c` move |
| Flip start | `$12` establishes active hinge/angle | None |
| Each continuation, including completion | One `$14` angular step | One `$0c` move |
| Following completion, remaining flips nonzero | Decrement and start next flip | None |
| Following final two-phase completion | Decrement, reverse, load 3, start flip | None |
| Following final three-phase completion | Decrement, reverse, cycle lead-in | One `$0c` move |

There is **one lead-in movement invocation at entry and between five-flip
cycles**, not between every pair of flips. The completing invocation halts;
the next usually starts another flip. There is no intervening block of idle
movement calls.

`$14` advances the angular code by one modulo 16 (increment for clear
direction, decrement for set direction). It completes when the code equals
the geometry-derived target. This is a 16-orientation representation, not a
fixed 16-invocation turn or C#'s nine degrees per tick. The number of
continuation calls varies with the angle pair; equality is checked after
advancing, so a modulo-zero distance would take a full 16 steps.
Actual clover angle codes are omitted from this listing, so their exact
per-hinge counts cannot be recovered here.

The program changes **both lane and climb progress**. `$0c` adds the selected
signed fixed-point speed to `enemy_along`; closer to the rim is a smaller ROM
along value. Medium/default Level 7 has packed speed
`$d4 + 6*$fb = $b6`, expanded by `crack_speed` into `$fd.b0`, or **-2.3125 ROM
along units per `$0c` call**. Easy/hard adjustments differ. This is not a
claim that the C# `ClimbSpeed` should be 2.3125: coordinates and scheduler
rates differ.
Sources:
[speed table, ROM:2707](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2707),
[difficulty/expansion, ROM:2598](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2598),
[fixed-point move, ROM:3705](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3705).

Normal gameplay calls enemy movement once per `state_04`, before shot movement,
enemy firing and player-shot hit checks. The main loop waits for `$53>=9`,
clears `$53`, then dispatches; the interrupt handler increments `$53`.
That establishes scheduling structure, not a measured seconds-to-C#-ticks
conversion or universal uninterrupted wall-clock period. The source copy does
not provide the needed hardware interrupt-frequency specification here.
Sources:
[gameplay order, ROM:3016](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3016),
[main loop, ROM:8927](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L8927),
[interrupt increment, ROM:10647](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L10647).

## 8. Is a 20-tick lane pause supported?

**No.** Program `$24` has neither an immediate 20 nor a delay loop requesting
20 movement invocations between flips. Its immediate route counts are 2 and
3. `$00` terminates the current invocation; it does not mean 20 C# ticks.
Even changing the current C# pause to zero would not reproduce all boundaries:
the existing pause state adds height and the old flip-start path has different
along-motion semantics.

Incitti computes `flippausedelay = 21-lv/5`, hence 20 at `lv=6`;
the current C# Level 6 tuning also has 20. Neither establishes that pause for
the ROM's Level 7 program.
Sources:
[Incitti tuning, tempest.bmx:4255](../../Reference/incitti/tempest.bmx#L4255),
[current tuning, LevelConfiguration.cs:246](../../src/Tempest.Game/Levels/LevelConfiguration.cs#L246).

The ROM call-level cadence is not unknown. Its **numeric conversion into the
C# 60 Hz simulation and the imported clover's angular timing remains
unresolved**. Do not fill that gap with 20, zero-pause-plus-old-climb, or
nine-degree pivots labeled as literal ROM timing.

## 9. Per-Flipper state

Every active ROM enemy has independent saved movement PC, direction/active
flags, remaining count, angular state, along fraction and shot delay.
The interpreter's current PC/run/condition scratch is shared only while
processing one enemy; its persistent state is indexed by enemy number.
Different spawn times and hinges naturally desynchronize enemies.

Required C# equivalents for this program are per-instance phase, remaining
flips, route step/boundary state and direction, alongside existing pivot and
height state. Do not put phase/count/direction on `Level`, or use a shared
oscillator.
Sources:
[RAM arrays, ROM:315](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L315),
[save/restore PC, ROM:3554](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3554).

## 10. Tanker-cargo implications

**Cargo does not universally use or universally bypass `$24`.**

`$a06f` removes the Tanker, determines child type and passes its current along
position into the child parameter selector `$9b07`. For Flipper cargo:

| Release along value | Child movement entry |
|---|---|
| `< $20` (near rim) | Generic per-type default `$07`, straight along movement until rim |
| `>= $20` (deeper; includes exactly `$20`) | Level-selected `flipper_move`, hence `$24` on Level 7 |

The threshold is a **ROM along coordinate**, not 20 C# ticks or C# height 20.
Natural Tanker splitting occurs after crossing below `$20`, so it takes the
near-rim/default branch. A player-shot destruction deeper in the tube can
produce children running `$24`.

On closed clover, the first child is allocated at Tanker segment T-1 with
direction bit clear, and the second at T+1 with direction bit set. Thus the
two children's initial routes point outward from the former Tanker lane.
For deep-release program `$24`, each independently starts the two-flip phase;
the second child has the **opposite initial direction from an ordinary
original**. They do not inherit the Tanker's route phase/counter.

Allocation has ordinary slot/capacity constraints; the trace describes
successful child creation, not a guarantee of two successful allocations
under every resource condition. Slot traversal also means this code alone
should not be turned into a universal C# same-tick activation rule.

Sources:
[depth-dependent selector, ROM:3542](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3542),
[split/children, ROM:4178](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4178),
[type default `$07`, ROM:3528](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3528).

Accepted C# cargo always starts in a mandatory outward release pivot, retains
a separate below-rim route, and reconstructs through fresh cargo Eggs.
It is **not** this ROM depth-selected entry mechanism:
[Tanker.cs:134](../../src/Tempest.Game/Gameplay/Tanker.cs#L134),
[Flipper.cs:109](../../src/Tempest.Game/Gameplay/Flipper.cs#L109).

Do not automatically apply an ordinary-original policy to cargo. Keeping
accepted cargo unchanged is the narrow compatibility proposal below, but must
be labeled an explicit C# exception, not complete Level 7 ROM cargo fidelity.
If Neal wants ROM-depth-dependent Level 7 cargo, approve it separately with a
coordinate mapping and observations; do not graft `$24` onto the end of the
accepted mandatory pivot and call that hybrid the ROM path.

## 11. Rim, death, re-entry and fresh-level reset

| Event | ROM/static conclusion | Proposed C# integration |
|---|---|---|
| Ordinary materialization/hatch analogue | Fresh PC `$24`, fixed clear direction; load 2 before first flip | Fresh route instance; no inherited Egg direction or saved route count |
| Reaches rim between flips | `$0c` installs the separate top program at `$42` | Discard below-rim phase; invoke accepted rim policy |
| Reaches rim while flipping | Active flag causes `$9d23` path to retain a just-below-rim position and finish the active flip first | Finish current pivot; do not count a partial pivot as complete or select a new route mid-pivot |
| Rim handoff after pivot completes | Top program replaces route PC/count; initial top direction can be player-relative or opposite an existing top enemy | Preserve accepted C# separate `RimPursuitDirection` latch; do not import new ROM top-direction rules |
| Player dies | `$0201` negative makes enemy movement return; retraction/re-entry processing follows | Preserve current freeze/render-only death behavior |
| Unresolved enemies re-enter | Active/top/pending counts become pending; reset clears active enemies and reinitializes pending entries; no per-enemy route restoration is carried | Existing ledger/Egg reconstruction creates fresh ordinary route state |
| Tanker children activate | Fresh allocation, direction by side, entry by depth as section 10 | Explicit compatibility exception unless separately approved |
| New level/restart | New parameter selection and active/pending initialization; no prior route instance survives | Existing fresh-wave initializer; independent new route state |

The ROM retraction code sums unresolved counts; it does **not** prove the
C# identity ledger or literal Incitti Egg object model is Atari's internal
representation. RAM counter bytes need not all be zeroed for logical fresh
route state: program initialization overwrites the count before using it.

Sources:
[top handling, ROM:3786](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3786),
[top p-code, ROM:4322](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4322),
[death/re-entry counts, ROM:4840](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4840),
[player reload/reset, ROM:9166](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L9166),
[active/pending reset, ROM:2201](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L2201),
[C# reconstruction, TempestGame.cs:440](../../src/Tempest.Game/TempestGame.cs#L440),
[fresh hatches, TempestGame.cs:719](../../src/Tempest.Game/TempestGame.cs#L719),
[fresh wave, TempestGame.cs:361](../../src/Tempest.Game/TempestGame.cs#L361).

Preserving the C# rim latch, identity ledger, Spike persistence, score/lives
and Zapper lifecycle is required compatibility, not a new claim of exact ROM
equivalence. In particular, the program's pending below-rim decrement must not
overwrite the rim handoff or reverse an already selected rim pursuit.

## 12. Geometry interaction and renderer adequacy

On closed clover, the movement route depends on direction and two/three
counts, **not** which lobe/notch is nearby. Geometry affects angular start and
target, duration and therefore accumulated climb, not route choice. This
program does not test Spike tips; do not import Incitti's Spike-riding pauses
into it.

Static inspection says the existing C# endpoint-hinge model is sufficient to
express every required route edge: all are adjacent in either direction,
including 15/0 wrap. It already separates destination/direction, full sweep
and spatial inward arc, and clips inward rays against the closed boundary.
Reversal only changes the next adjacent transition; it does not require a
different polygon, a nonadjacent jump, a new hinge type or renderer rewrite.
Sources:
[transition representation, DirectedLaneTransition.cs:3](../../src/Tempest.Game/Levels/DirectedLaneTransition.cs#L3),
[transition construction, Level.cs:443](../../src/Tempest.Game/Levels/Level.cs#L443),
[endpoint/clipping calculation, Level.cs:544](../../src/Tempest.Game/Levels/Level.cs#L544).

Read-only arithmetic on the actual Incitti row independently confirmed:

- 35 fields, 16 vertices, closed 16-lane clover.
- Positive signed double area 204128.
- Repeating hinge angles:
  `108.604109, 106.751441, 307.893009, 106.751441` degrees.
- Full reflex hinges at vertices 2, 6, 10, 14 must remain long sweeps.
- **Hypothetically retaining Incitti step 9**, angular advancement takes
  13/12/35/12 updates. These are geometry calculations, **not ROM timings**
  and not an approved Level 7 cadence.

This is structural adequacy, not an executed clover renderer certification.
All 32 directed transitions, intermediate endpoints, inward clipping and
reflex completion still need automated tests and later visual acceptance.
The absence of implementation here means no Level 7 route was rendered.

## 13. Firing and collision during boundaries and pivots

**Systems continue; emission is not unconditional.**

- ROM normal gameplay calls shot movement, enemy firing and player-shot hit
  processing separately from the enemy's p-code. An enemy `$00` halt does not
  halt those global phases.
- An otherwise eligible enemy's shot delay advances before the
  between-segments flag is checked. An active flip blocks new emission, not
  delay progression. Completion clears the flag, making the later same-call
  firing scan eligible subject to distance, delay, pressure and shot slots.
- `$24` does not disable player shots or make the Flipper invulnerable.
  ROM player-shot collision explicitly checks active angular/endpoint state;
  its ownership rules are not identical to accepted C# two-lane acceptance.
- Below-rim pivots do not become player collision just because of projected
  overlap. Top-program `$18` performs the grab check when not flipping.
- ROM top-program grab processing can occur inside enemy movement, whereas
  accepted C# deliberately checks player shots before consuming completed
  Flipper/player collision. Do not silently replace that accepted order.

Sources:
[normal order, ROM:3016](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3016),
[firing gates, ROM:4547](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4547),
[hit processing, ROM:4763](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4763),
[grab flag check, ROM:3921](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3921),
[C# two-lane hit acceptance, Flipper.cs:160](../../src/Tempest.Game/Gameplay/Flipper.cs#L160),
[C# firing scan, TempestGame.cs:1286](../../src/Tempest.Game/TempestGame.cs#L1286),
[C# update order, TempestGame.cs:298](../../src/Tempest.Game/TempestGame.cs#L298).

For future integration, keep firing and collision phases independent of new
route stages. A start stage must expose an active pivot; a completed stage
must expose completion consistently for shooting/collision and rim handoff.
Do not use a route-stage early return to bypass global firing or collisions.

## 14. Precise differences from Incitti

| Question | ROM Level 7 ordinary `$24` | Incitti at `lv=6` |
|---|---|---|
| Route selection | Per-level program entry | Generic `canflip=true` |
| Initial direction | Fixed clear bit, decreasing ROM segments | `FindShortDir` relative to player; same-lane tie uses RNG |
| Below-rim reversals | Explicit after 2, then after 3 completed turns | No two/three phase counter; generally retains sampled direction |
| Trigger | Program reaches start opcode on eligible movement call | Sliding enters delay; expiry starts pivot |
| Between-flip delay | Completion boundary and next program action; extra lead-in after each five flips | Below-rim pause 20 |
| Angular continuation | One modulo-16 angular-code step per movement call | 9 degrees per update |
| Along motion | Selected fixed-point speed on lead-in/continuation, not start calls | Normal +2, delay +1, Spike slide +3 |
| Spike interaction | No Spike query in this entry | Slide under tip; eight-update pause on clearing; entering uncleared Spike lane can return to sliding |
| Rim | Separate top program; separate direction selection | Re-evaluates `FindShortDir` at rim pause expiry |
| Cargo | Depth-selected `$07` or level program; opposite side directions | Separate mandatory release-pivot/type behavior |

Sources:
[Incitti movement, tempest.bmx:2747](../../Reference/incitti/tempest.bmx#L2747),
[Incitti tuning, tempest.bmx:4255](../../Reference/incitti/tempest.bmx#L4255),
[FindShortDir, tempest.bmx:3987](../../Reference/incitti/tempest.bmx#L3987).

Accepted C# already intentionally differs from Incitti in rim persistence,
cargo outward routing, source-anchored transitions, two-lane shootability and
collision order. The new investigation does not authorize changing those.

## 15. Narrowest proposed C# contract (not implemented or approved)

**Readiness:** sufficient evidence for route/state semantics, but not yet for
certifying physical initial sign, C# numeric cadence or full ROM cargo parity.

### Configuration and compatibility boundary

- Add one explicit below-rim policy to immutable Flipper tuning, for example
  `Existing` versus `RomTwoThree`. Preserve the exact existing path as the
  default for all accepted Levels 1-6; do not reinterpret `CanFlip`.
- Only a future Level 7 configuration opts ordinary originals into
  `RomTwoThree`. No behavior inference from board shape, displayed-level
  switches buried in the entity, or universal below-rim direction policy.
- Configure/resolve one approved initial C# direction for this imported
  clover after handedness is established. Do not call shortest-path/RNG for
  this route.
- Reuse existing endpoint geometry and independent rim/firing policies.
- Keep C# cargo on its accepted explicit exception for the narrow milestone,
  subject to Neal's approval and the fidelity limitation in section 10.
  A request for complete ROM cargo fidelity requires an additional bounded
  decision, not an undocumented extension of this ordinary-original policy.

### Per-instance state

- Two-valued phase: `Two` or `Three`.
- Remaining completed flips to charge: initially 2, then 3 or 2 by phase.
- Reuse existing `Direction` (+1 previous / -1 next), pivot/height state and
  separately latched `RimPursuitDirection`.
- A small route-step enum, conceptually:
  `LeadIn`, `Start`, `Turning`, `AfterCompletion`.
  This represents observable movement-call boundaries, not arbitrary scripts.
- No list of opcodes, generalized VM, script parser, public variable phase
  lengths, shared level counter, or speculative future programs.

### Transition rules

1. Fresh ordinary hatch: phase Two, remaining 2, approved fixed direction,
   route step LeadIn, no active transition or rim latch.
2. LeadIn: perform one approved along step; next stage Start; no pivot.
3. Start: create the adjacent endpoint-hinge transition and mark flipping;
   no along increment on this route call. Next stage Turning.
4. Turning: advance approved angular progress and along position. On angular
   completion, commit the adjacent destination, expose normal collision/fire
   state, and enter AfterCompletion; do not decrement early.
5. AfterCompletion on the next route call: decrement remaining once.
   - If nonzero, start the next flip in the same call, without along motion.
   - If phase Two expired, reverse Direction, phase Three/remaining 3, and
     start that flip in the same call.
   - If phase Three expired, reverse Direction, phase Two/remaining 2, and
     execute LeadIn in the same call; next call starts a flip.
6. Rim arrival/handoff has priority over scheduling another below-rim flip.
   If a turn is active, preserve the complete visible pivot and accepted
   pivot collision safety. Once safe to hand off, discard route-step/count
   relevance and use the accepted independent rim latch/pause path.
7. Ordinary death/reconstruction discards all runtime route state. A fresh
   ordinary hatch restarts Two/2/fixed direction. Fresh levels and restarts do
   the same. Do not retain route state in Egg identities or the wave ledger.
8. Accepted cargo creation/re-entry stays separate; do not inherit the
   carrier's phase, consume original counts, or retarget cargo toward player
   below the rim.

**Cadence policy still requiring approval:** a ROM movement invocation is not
automatically one C# 60 Hz update. A one-call-per-update realization would be
an explicit project approximation. Choose the mapping after arcade
measurement; reuse existing scalar tuning only if approved as an
approximation. In no case append `LanePauseTicks=20` to this route.

### Deterministic seams and maintainability comments

- Drive real entity updates with an explicit policy/configuration, spawn lane,
  initial direction and controllable progress rate; use no route RNG.
- Inspect internal phase/remaining/step snapshots in tests, or infer them from
  exact destination/update traces. Avoid new production debug UI or replay
  infrastructure.
- Test independence with staggered hatches and different hinge lengths.
- Comments should cite program `$24`/`$a11b-$a138`, explain entry-offset versus
  opcode `$24`, completed-then-next-call decrement, the extra five-flip-cycle
  lead-in, no climb on start calls, direction convention/mapping evidence,
  rim-state precedence, and the explicit cargo compatibility exception.
- Label numeric C# timing as approved approximation until measured; explain
  why old generic canflip remains unchanged. Do not annotate obvious enum
  assignments or duplicate the source trace throughout the code.

## 16. Automated tests required later

No tests or production code were changed or executed during this analysis.
Eventual focused tests should verify the exact contract, not just that
Flippers sometimes reverse:

1. Opt-in configuration; all existing Level 1-6 policy/tuning snapshots and
   accepted movement regressions remain unchanged.
2. Ordinary initial Two/2 phase and fixed approved direction across player
   lanes, spawn lanes, RNG seeds and staggered creation. No route RNG calls.
3. Exact first ten destinations from section 2, both wrap boundaries,
   two full cycles and net one-lane cycle drift; mirror direction for any
   explicitly supported initial-direction seam.
4. No counter decrement on attempt/start/partial continuation; exactly one
   decrement on the invocation after completion; reversal before the next
   start, never during a pivot.
5. Every movement boundary in section 7, including no along motion on starts,
   along motion on completing continuation, and extra lead-in only after the
   three-flip block. Assert selected numeric cadence only after approval.
6. Moving the player or consuming unrelated RNG does not retarget this route.
   Independent Flippers do not share counters or phase.
7. Rim reached in lead-in, continuation, completion and pending-count states:
   no truncated reflex pivot, erroneous extra below-rim start, double
   decrement, spurious reverse, lost collision or overwritten rim latch.
8. All 16 clover hinges both directions: start/mid/end finite endpoints,
   correct adjacency/hinge and wrap, inward containment, complete reflex
   sweep, and no shortest-arc substitution. Additional intermediate samples
   are needed around concave clipping; endpoint equality alone is insufficient.
9. Destruction mid-flip, death in each phase/stage, re-entry through fresh
   Eggs, new level/replay: fresh route state, preserved accepted ledger,
   score/lives/Zapper/Spike invariants, no restored stale route direction.
10. Cargo remains explicit: accepted release/re-entry compatibility tests,
    no accidental ordinary-policy adoption. If later approved, separate tests
    for ROM `<$20`, `==$20`, `>$20` release selection, opposite side directions
    and independent fresh counters, using an approved coordinate conversion.
11. Player shots remain accepted through pauses/turns per C# rules; shot delay
    continues, active pivots suppress emission, completion exposes ready
    firing, and shot-before-player-collision precedence survives.

Existing focused test patterns include
[Level2CanFlipTests.cs](../../tests/Tempest.Game.Tests/Level2CanFlipTests.cs),
[Level3GeometryTests.cs](../../tests/Tempest.Game.Tests/Level3GeometryTests.cs)
and [ArcadeFlipperFiringTests.cs](../../tests/Tempest.Game.Tests/ArcadeFlipperFiringTests.cs).
No earlier passing test count is claimed as new validation.

## 17. Remaining unknowns and decisions

**Resolved statically:** program selection, first phase, ordinary raw initial
direction, per-instance persistence, flip-count/reversal boundaries, call-level
trigger/cadence, no 20-call pause, along-motion scheduling, rim program
replacement and depth-dependent cargo entry.

**Unresolved:**

- Physical ROM clover index orientation and rotational offset versus the
  imported source order. Consequently, the physically correct initial C#
  sign is not yet certified.
- Actual clover ROM `tube_angle` values and quantized continuation counts;
  their arrays have no values in this listing.
- Measured arcade seconds/frame durations and the intended conversion to C#
  updates, climb units and continuous-degree geometry.
- Whether a bounded ordinary-only Level 7 milestone should intentionally keep
  accepted cargo compatibility or also pursue ROM deep-release routing.
- Exact visible Level 7 cargo sequence/activation timing and its relation to
  accepted routing, especially after shot release deeper in the tube.
- Executed clover renderer acceptance; static topology/geometry adequacy is
  not a substitute for tests and Neal's comparison.

No fresh footage was measured here. Existing observation records do not
establish Level 7 first-flip handedness or numeric cadence.

## 18. Concise arcade/video observation plan for Neal

Use unmirrored, normal-speed Atari footage with known orientation and frame
rate, preferably recording a fresh displayed Level 7. C# recorded playback
can inspect C# behavior only; it cannot resolve any Atari uncertainty.

| Target | What to record | Minimum useful sample / distinguishing evidence |
|---|---|---|
| Ordinary initial handedness and phase | Follow an original from materialization, not cargo or a rim enemy; mark its start lane on the imported clover diagram and each completed destination | Prefer 10 consecutive flips / four block-boundary reversals if it survives below rim long enough; otherwise capture a complete five-flip cycle and combine multiple clearly fresh originals |
| Fixed program versus player-following/random | Record lane sequence, Blaster lane and every reversal; move Blaster behind when possible | At least three ordinary Flippers from different lanes across at least two fresh Level 7 entries/playthroughs; repeatable 2/3 counts and same initial handedness, independent of player, support the fixed program |
| Cadence and geometry | Frame-stamp first visible departure, angular landing, next departure and approximate depth; distinguish convex/lobe hinges from reflex notches | Several turns of each hinge type and at least one full five-flip cycle; look for the extra lead-in after the three-flip block, rather than a 20-update dwell after every turn |
| Along-motion schedule | Track depth during visible turn, completion and between-turn frames | Clear high-frame-rate footage/slow playback of the original recording; verify no evidence of a universal 20-tick pause. Ordinary 30/60-fps footage may not resolve a single ROM movement-call boundary |
| Cargo entry | Follow both children of a deep player-shot Tanker and compare with natural near-rim release | Multiple deep and near-rim releases, at least two examples each if possible; record initial child lanes, first two flips, reversal and three more where survival permits |
| Rim/death handoff | Follow an ordinary during a long reflex turn reaching rim; separately lose a life mid-pattern and follow fresh entrants | At least one clear rim crossing and one reconstruction, preferably repeated; no continuing partial phase should be assumed from a different re-entering enemy |
| C# compatibility after later implementation | Deterministic C# playback of identical marked route/state cases | Useful for observing approved C# output and regression, never proof of Atari direction, cadence or hidden program selection |

To settle direction mapping visually, number lanes using the imported clover
coordinates (lane 0 joins its first two listed vertices), overlay that diagram
on upright Atari footage, and record whether a fresh original first moves
toward increasing or decreasing *that overlay's* lane number. This gives the
needed C# sign without assuming ROM's private numbering.

To establish timing, preserve the video's true frame rate and speed, count
departure-to-landing and landing-to-next-departure frames separately, and
compare hinge types at similar heights. Report ranges/uncertainty when frames
are dropped, interpolated, repeated or occluded. Video alone may not resolve
the exact interrupt-to-movement-call relationship; verified hardware timing
or an instrumented authorized arcade-emulator trace would be needed for that.
No arbitrary 20-tick fallback is acceptable.

## 19. Final Git state and ignored records

Final required checks:

```powershell
git status --short --branch
git diff --stat
git diff --cached --stat
git rev-parse HEAD
```

Final verified result: `## master`, both diff statistics empty, HEAD unchanged
at `79e33886772424a31973332f33f3781b55e4c2f8`.
No tracked or staged changes.

Only these new analysis records were written:

- [Prompt](../Prompts/2026-11-04-level7-flipper-rom-analysis-prompt.md)
- [Report](2026-11-04-level7-flipper-rom-analysis-report.md)

Both are ignored by `.gitignore:6`, `AI-Logs/`. They are intentional durable
prompt/report records, not production/project-authority/reference edits.
The preserved prompt contains exactly one standalone completion token.
Read-only arithmetic checked clover winding/hinge angles and the medium ROM
speed conversion. No build/test tooling or game execution was needed for
this analysis-only task; no temporary helper files were created.

## 20. No implementation; stop for review

No Level 7 implementation, production edit, test edit, project-document edit,
reference edit, game launch, new recording, commit or push occurred.
Accepted Levels 1-6 remain untouched.

The two/three program is established. Physical initial-direction mapping and
numeric C# cadence remain prerequisites to claiming Atari-faithful ordinary
Level 7 motion; cargo compatibility is an explicit additional scope decision.
Stop here and await Neal's review.
