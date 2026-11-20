# Direct-Hatched Pulsar Milestone Implementation

## 1. Behavior implemented

Added an isolated `--pulsar-test` scenario on the existing Level 1 tube. It
reuses the ten-unit Egg release/hatch ledger and hatches each ordinary unit as a
height-zero, climbing Pulsar. Pulsars do not fire enemy bullets, including when
`--enemy-bullets-test` is also present. The scenario conflicts with the Spiker,
Tanker and Fuseball scenarios; `--lives` remains composable.

Implemented source states 0, 1, 3, 4, 5 and 7. Height moves one unit per
vertical update. The strict `timetoflip > 50` decision therefore occurs on
update 51. The source's 18-update lane pause transfers exactly one adjacent
edge on its expiration update; its 14-update rim pause resumes vertical travel
on its expiration update. Direction follows the source's short-route rule,
including same-edge random ties; closed edges wrap and an open model reflects
at endpoints. State 2 is represented for source-compatible rendering and
collision rules but no normal transition enters it.

The first-eligible source configuration (`lv=16`) is used on the current
development tube: 12 degrees per flip update, an 18-tick lane pause and a
14-tick rim pause. This is a development setup, not Level 17 composition or
verified arcade tuning. `rimit`/hurry-up remains disabled.

## 2. Files changed

- `src/Tempest.Game/Gameplay/Pulsar.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `README.md`
- `AI-Logs/Prompts/2026-10-30-pulsar-implementation-prompt.md` (ignored)
- This report (ignored)

## 3. BlitzMax division conclusion

The official BlitzMax language reference describes `Int` as a 32-bit signed
integer and its type-balancing rule as returning `Int` when neither operand is
floating point or `Long`: <https://blitzmax.org/docs/en/language/data_types/>.
The Incitti declarations use integer-default names for `lv`, `pause`,
`pulsepausedelay` and `pulsepausedelayrim` (the floating-point fields have the
`#` suffix); see `Reference/incitti/tempest.bmx` around lines 137, 2653, 2949
and 4259-4260. Therefore `lv/6` is the integer quotient `16/6 = 2`, and the
assigned pause values are exactly **18 ticks** (`20 - 2`) and **14 ticks**
(`16 - 2`). The result does not depend on converting a fractional result into
an integer.

## 4. Pulse clock and death freeze

One oscillator is owned by `TempestGame` and advances once per active gameplay
update before shots and enemy updates. It resets only when phase exceeds 180,
so the default cycle is 181 calls. The pulsing window is strictly
`phase > 70 && phase < 110`, and amplitude is
`Abs((phase % 90) - 45) / 4`, then zeroed only below 2 and capped at 12.

The clock **freezes during the existing 60-update `PlayerDying` freeze**.
Incitti advances the global timer and Pulsars during much of its death
presentation, but this C# game freezes gameplay entities for the entire death
interval. Keeping the clock in the active-gameplay update block preserves that
established simulation policy without changing the death lifecycle. The same
active-play gate also means no clock advances during existing exit/frozen end
states.

## 5. State-machine decisions and source discrepancies

- State 2 (source numeric state 2) is modeled but is never assigned by the
  direct-hatch state machine.
- Near-rim hits enter the source terminal state 7; electrical lane collisions
  use the existing generic C# death lifecycle and do not invent a separate
  animation or death cause.
- Open-edge movement is supported in the Pulsar model for tests, but the
  renderer remains closed-tube-only.
- **Analysis-report correction:** the report says source state 7 has no draw
  geometry. Literal `Pulsar.Draw` in `Reference/incitti/tempest.bmx` includes
  `Case 3,5,7` in the endpoint-projection switch (around lines 2965-2982), so
  state 7 receives the same zigzag geometry. The implementation retains that
  source behavior.
- Incitti's palette changes by level band. The existing Level 1 tube uses the
  source Level 1-16 Pulsar cyan and bullet white for the glyph/overlay; no
  level-color progression was added.

## 6. Renderer changes

Added a Pulsar-specific callback renderer that projects the source/current
edge, applies the source pivot interpolation during a flip, and submits six
connected zigzag segments using the five source fractions and alternating
`pulse_zh` normal offsets. Pulsars are cyan outside the active pulse window and
white during it. The tube retains its normal geometry except that the near/top
rim connection of each visually pulsing lane is omitted until the pulse ends,
following Neal's manual Atari observation. The renderer overlays only
the two radial boundaries of each unique lane whose Pulsar is above height 16
while the global pulse is active. The source phase-mod-8 color alternation is
preserved.

## 7. Collision, scoring and accounting

Player collision requires the same edge and preserves the strict
`Depth - Height < 8` near-rim branch. Non-flipping rim contact is lethal;
flipping rim contact is lethal only when
`Abs(TotalTurnAngle - TurnAngle) < 30`. At distance 8 or greater, only
climbing/descending Pulsars are lethal during the global pulse window; this is
a lane-wide hazard, not a height overlap.

Shots must be on the current edge and satisfy the strict separation
`0 < Pulsar.Height - Shot.Height < 16`. A hit consumes the shot, removes only
that Pulsar, records one destroyed unit and awards 200 points. The existing
shot-before-enemy-update/player-collision order is unchanged.

Pulsars are included in active counts, ordered development shot targets,
completion checks, HUD, death clearing and fresh-Egg reconstruction. Destroyed
units and score persist through death; unresolved Pulsars return to the
ordinary ten-unit Egg/reserve supply. Completion uses the existing exit path.

## 8. Focused test result

Command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Pulsar|FullyQualifiedName~PulsingLane"
```

Latest renderer-correction run: **passed:** 44; **failed:** 0; **skipped:** 0.
The filter also includes the established pulse-lane tests whose names do not
contain "Pulsar". The original milestone's Pulsar-only run passed 39 tests.

## 9. Full regression test result

Command:

```powershell
dotnet test --no-restore
```

Latest renderer-correction run: **passed:** 250; **failed:** 0; **skipped:** 0.
The original milestone suite passed 246 tests.

## 10. Build result and warnings

Command:

```powershell
dotnet build --no-restore
```

**Build succeeded:** 0 errors, 0 warnings.

## 11. Manual validation for Neal

Run:

```powershell
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
```

Compare against Atari Tempest gameplay (and use Incitti as an implementation
reference):

- Check the six-segment zigzag, pulse amplitude and pulse/non-pulse colors.
- Confirm only the active lane's two radial boundaries flash, starting above
  height 16. Its near/top rim connection must disappear throughout the pulse
  and return afterward; far-rim and unaffected tube lines remain visible.
- Compare climb/descent speed, update-51 decisions, 18/14 pauses, turn direction
  and lane association while turning.
- Check rim and lane-wide pulse collisions, movement off the hazardous lane,
  and the shot window/200-point award.
- Verify life loss, fresh-Egg re-entry, and that shot-destroyed Pulsars stay
  destroyed.
- Confirm no Tankers or other enemy kinds hatch, and the scenario does not
  advance to another level.
- Smoke-test normal Level 1 and the Spiker, Tanker and Fuseball scenarios.

No manual or visual validation was performed by the assistant. Neal subsequently
reported that gameplay/state/collision behavior passed manual validation and
identified the near/top rim rendering bug described in section 14. The visual
correction has been tested headlessly; no new manual validation is claimed.

## 12. Explicitly deferred behavior

Normal progression and Level 17 composition; Tanker-carried Pulsars; `rimit` /
hurry-up; Super Zapper, `ZapIt` and delayed kills; audio; open-tube rendering;
new death animations; level bonuses; and broad enemy, animation or renderer
abstractions remain out of scope.

## 13. Remaining arcade comparison

Automated tests establish the Incitti-derived boundaries and C# accounting,
not Atari accuracy. Neal should compare movement cadence and pauses, turn
presentation, glyph size/offset and palette, lane-wide electrical collision,
shot scoring/window, and life-loss/re-entry timing against the arcade. Open
endpoint behavior is model-tested only and has no corresponding board rendering.

## 14. Manual arcade finding and renderer-only correction (2026-10-30)

Neal observed that the near/top rim connection spanning an affected Pulsar lane
disappears for the duration of an Atari pulse and returns when the pulse ends.
The C# lane-wide player collision was already correct and manually validated;
the flashing radial boundaries were correct, but the underlying top rim was
incorrectly left visible.

Re-inspected literal `Edge.Draw` and `Pulsar.Draw/Update` before this correction.
In `Reference/incitti/tempest.bmx:1209-1251`, the pulsing branch draws only the
two radial boundaries. The bottom and top connections are both inside its
non-pulsing `Else`; additionally, the top connection there is gated by
`haspulser = False`. `Pulsar.Update` sets `haspulser` unconditionally and marks
the lane pulsing only above height 16. Thus the earlier analysis's assertion
that Incitti preserves ordinary rim lines during a pulse was incorrect.
Literal Incitti also suppresses the top connection outside the pulse whenever
a Pulsar is present, and suppresses the bottom connection during a pulse.
Those broader omissions are not copied: Neal's arcade observation and the
requested scope establish that only the near/top segment should disappear,
only while the lane is visually pulsing.

Changed only `src/Tempest.Game/Rendering/VectorRenderer.cs`,
`tests/Tempest.Game.Tests/SimulationTests.cs`, and this ignored report in this
follow-up. Exposed the existing tube line submissions through a callback for
headless testing, and used the same render-only lane eligibility predicate for
rim suppression and radial overlays: a Pulsar on that edge, height strictly
greater than 16, and the existing active global pulse flag. Each near segment
is submitted at most once; an affected lane is omitted regardless of how many
Pulsars occupy it. Overlay lane deduplication remains intact.

Four new deterministic test cases verify ordinary tube geometry, omission and
phase-110 restoration, exact multiple-lane omissions and radial overlays, and
height 16 versus 17 eligibility. Geometry assertions check every near segment,
every far segment and every normal radial boundary, plus counts and uniqueness.
All movement, state, collision, scoring, accounting, reconstruction, scenario
composition and pulse-clock code remains unchanged in this correction.

Validation: focused Pulsar/pulse-lane tests **44 passed, 0 failed, 0 skipped**;
full suite **250 passed, 0 failed, 0 skipped**; build **0 errors, 0 warnings**.
The SDK also printed a workload-update availability notice; it is not a build
warning. Manual confirmation of the corrected rendered appearance remains
Neal's next check.

No commit was created.
