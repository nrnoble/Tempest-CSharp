# Blaster-Death ROM Selector and Timing Verification

Date: 2026-11-22

## Result

**NEEDS MORE VISUAL EVIDENCE**

The ROM listing identifies the supplied burst as the enemy-shot player-death
family and provides its discrete picture order and display-call holds. It does
not yet support an exact mapping from the 45 MAME PNGs to C# simulation ticks:
the picture selector advances on vector-display calls, while the effect object
has a separate 15-update lifetime. The prior capture sequence has no
established one-PNG-per-Atari-update cadence. Do not fit tick ranges or change
the accepted C# presentation until that relationship is established.

“More visual evidence” here means capture-cadence or synchronized update
evidence, not another broad image review. The specified representative frames
5702, 5723, and 5744 were revisited only to compare the compact, broad, and
remnant shapes with the ROM family; no other visual assets were opened.

## Evidence and exact death-effect identity

**ROM-BACKED, from the tracked disassembly:** The expanding, contracting
multicolor fragment effect uses the “hit-by-shot explosion” entries and the
shared vector picture at `$3b1a`. The cause path is the enemy-shot collision:

1. `$a1e4` verifies the shot and player occupy the same segment and that the
   player is not already in the death state; it calls `$a34b` and sets
   `$0201 = $81`.
2. `$a34b` initializes selector `$013b = $ff`; shared setup creates a type-1
   mid-graphic at the player location and initializes `$013c = 1`.
3. Drawing routes that type-1 effect through `vapp_mid_graphic` at `$b7eb`.
   `$b7ff` decrements `$013c`; at zero it increments `$013b`, reloads the
   hold count from `$b82a,x`, and selects `graphic_table + $28 + 2*x`.
4. The first draw advances the initial `$ff` index to 0 and selects
   `$cef0`, the size-1 shot-hit picture. Subsequent indices select the
   expanding and contracting shot-hit pictures listed below.

This identifies the supplied broad burst as the **enemy-shot death** sequence:
the enemy-shot path starts at index `$ff` and enters the size-1-to-size-6
multicolor burst. Spike collision and Pulsar contact call `pieces_death`
(`$9893`, `$9d01`) and start at index 7; enemy contact at `$9e58` starts at
index 9. Those paths do not start the broad expansion. `draw_player` also
suppresses the ordinary player glyph for `$0201 == $81`.

The separate `$3bae` spiked/pulsared-apart list calls vector subroutine
`$0ffc` before piece motions; it is not the selected `$3b1a` shot-hit picture
path. No `$0ffc` implementation is needed to establish the selected event.
No local ROM bytes were read: the tracked disassembly includes the relevant
instructions, selector tables, and vector-picture entries. The prohibited
`MBUCOD.V05.txt` was not accessed.

## Picture order, scale, and color

All nine shot-hit entries call the same multicolor fragment list at `$3b1a`.
Its vector geometry is shared; these stages change global `vscale`, not to a
different fragment geometry:

| Index | Graphic-table entry | Vector program | Size label / scale command | Hold |
|---:|---|---|---|---:|
| 0 | `$cef0` | `$3b18` | size 1, `vscale b=2 l=64` | 2 display calls |
| 1 | `$cef2` | `$3b14` | size 2, `vscale b=2 l=0` | 2 |
| 2 | `$cef4` | `$3b10` | size 3, `vscale b=1 l=64` | 2 |
| 3 | `$cef6` | `$3b0c` | size 4, `vscale b=1 l=0` | 2 |
| 4 | `$cef8` | `$3b08` | size 5, `vscale b=0 l=64` | 2 |
| 5 | `$cefa` | `$3b04` | size 6, `vscale b=0 l=0` | 4 |
| 6 | `$cefc` | `$3b0c` | size 4, `vscale b=1 l=0` | 3 |
| 7 | `$cefe` | `$3b14` | size 2, `vscale b=2 l=0` | 2 |
| 8 | `$cf00` | `$3b18` | size 1, `vscale b=2 l=64` | 1 |

Thus the order is **1, 2, 3, 4, 5, 6, 4, 2, 1**, for a total of 20
selector/display calls. The labels are the disassembly's named sizes; they
are not asserted as C# pixel scales. The fixed fragment list uses ROM color
codes 9, 11, and 10 with sparkle enabled; the listing does not establish an
RGB mapping. It does not expose a separate sparkle/intensity animation.

The shared selector table continues after index 8: index 9 selects
`$cf02`/spiked-player and has a `$20` hold; later entries select fuzzballed-
player graphics. Those are not additional stages in the enemy-shot
size-1-to-size-6 expansion/contraction schedule.

## Holds, culling, and capture cadence

**ROM-BACKED:** `$013c` is the stage hold counter. `$b82a` explicitly reloads
2, 2, 2, 2, 2, 4, 3, 2, and 1 for indices 0–8. These are intentional repeated
picture selections over consecutive calls to `vapp_mid_graphic`; the peak
size-6 picture is held for four such calls.

A separate age counter in `$0312,x` is advanced by `$a416`. For a type-1
effect, `$a448` gives a limit of 15 and `$a44e` increments age by 1; at the
limit `$a43b` clears the active effect position. The tracked listing therefore
shows a 15-simulation-update object lifetime alongside a 20-display-call
stage schedule. Their precise relative cadence in the supplied capture is
**UNKNOWN**; the listing and PNG numbering do not establish that one display
call equals one captured PNG or one simulation update.

The visual investigation independently found duplicate/held MAME frames and
Neal confirmed these are expected. ROM holds are real, so duplicates must not
be treated as noise or “fixed” with interpolation. Which specific duplicate
groups correspond to the explicit ROM holds versus capture duplication (or
both) remains **UNKNOWN**. The final zero-visibility point in the MAME capture
cannot be assigned an exact Atari update number from available cadence
evidence.

## C# translation readiness

**NEEDS MORE VISUAL EVIDENCE**

The existing 60-tick `DeathTicks` clock can mechanically host a deterministic
render-only stage lookup, without changing collision, lives, re-entry, or
gameplay timing. But assigning C# tick ranges now would conflate the ROM
display-call hold counter with the separate 15-update effect lifetime and
would not be justified by the 45-frame capture. Preserve the accepted center,
three-layer 16-point geometry, 150% peak scale, shared legacy/Atari renderer,
determinism, and gameplay lifecycle. Do not characterize the accepted effect
as broken. The smallest future correction, once cadence is reconciled, is a
read-only discrete stage lookup on existing `DeathTicks`/normalized age, with
explicit holds and no mutable fragment objects.

No C# implementation table is supplied because a defensible simulation-tick
mapping is not yet established. The ROM picture order and display-call holds
above are the exact inputs for that later mapping.

## Suggested focused regression tests

- Deterministic stage selection and exact order 1, 2, 3, 4, 5, 6, 4, 2, 1.
- Intentional repeated stages and verified hold counts after cadence mapping.
- Expansion-to-peak-to-contraction order and final clean disappearance.
- Unchanged 60-tick death lifecycle, lives decrement, collision, and re-entry.
- Unchanged deterministic reverse playback/scrubbing.
- Same underlying death effect in legacy and Atari presentation.
- Compatibility on Levels 1–11 and future levels, without level-specific data.

## Repository state, changes, and validation

- Starting branch/HEAD: `master`, `aa6b7110fe845cf36173a80b7dc5d11c1f067806`,
  tracking `origin/master`.
- Pre-existing tracked changes: two AI-Logs reports. Pre-existing untracked
  files included the prior visual-investigation prompt/report and
  `Reference/arcade/MBUCOD.V05.txt`; none were modified or accessed.
- This task created only this report and its paired prompt in ignored
  `AI-Logs/`. No source, tests, configuration, durable documentation,
  reference files, gameplay assets, ROMs, or Git index/state were changed.
- **PASS** — tracked-disassembly selector, death-cause, picture-table, hold,
  and effect-lifetime trace completed.
- **NOT RUN** — tests/build; this was a read-only investigation with no code
  changes.
- Manual gameplay acceptance is not requested because no behavior changed.
- Remaining evidence needed: correlate the authorized MAME capture cadence
  with Atari display calls and simulation updates before assigning exact
  C# death-tick ranges. Stop here; do not launch the game or implement.
