# Final Blaster-Death Geometry Investigation Report

Date: 2026-11-22

## Question and disposition

**Can the Atari enemy-shot player-death vector picture at `$3b1a–$3b82` be
translated directly enough to create a justified visible improvement over the
current C# three-layer starburst?**

**Yes.** The complete tracked listing decodes into a fixed, substantially
different 24-stroke, multicolor angular path. It is not three concentric
outlines and does not need inferred fragment motion. The geometry is
implementation-ready. Neal's report that applying only the stepped schedule
looked effectively identical remains authoritative: the schedule changes
size/hold presentation, not the C# outline topology, segment count, or
color-sector arrangement.

## Starting state and scope

- Branch `master`, HEAD `aa6b7110fe845cf36173a80b7dc5d11c1f067806`,
  tracking `origin/master`.
- Starting worktree already contained modifications to
  `PlayerDeathPresentation.cs`, `VectorRenderer.cs`,
  `PlayerDeathPresentationTests.cs`, and two existing AI-Logs reports, plus
  several untracked AI-Logs prompt/report files and
  `Reference/arcade/MBUCOD.V05.txt`.
- Those changes and files were pre-existing and preserved. The MBUCOD file
  was not opened. No ROM file was accessed. No source, tests, configuration,
  durable documentation, reference files, or Git index were changed.
- Evidence was limited to the tracked disassembly, current renderer/tests,
  accepted behavior notes, and the task-provided manual result. No screenshots
  were revisited and no timing investigation was performed.

## Disassembly decode

The full tracked vector program is present with no omitted or ambiguous
instruction: [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md),
lines 1574–1612. Local origin `(0,0)` is the current vector point on entry.
`VSTAT` changes color/stat state without moving the point. Coordinates below
are decoded raw DVG relative deltas; cumulative endpoints define the local
picture. The initial `VLDRAW z=off` moves to `(48,-16)` without drawing.
Thereafter every listed `VSDRAW`/`VLDRAW z=14` is visible. The final
`VLDRAW z=off` is a non-drawing return to `(0,0)`.

All twelve `VSTAT` groups set `z=12` and `sparkle=1`; each visible stroke
instruction has `z=14`. The color state persists until the next `VSTAT`.
There are no pen-up moves between the visible strokes, so all strokes connect
end-to-start.

| Draw order | Address | Start `(x,y)` | End `(x,y)` | Stat group | Color code | Stroke |
|---:|---:|---:|---:|---:|---:|---|
| 1 | `$3b20` | `(48,-16)` | `(112,16)` | 1 | 9 | visible, `z=14` |
| 2 | `$3b24` | `(112,16)` | `(64,24)` | 1 | 9 | visible, `z=14` |
| 3 | `$3b2a` | `(64,24)` | `(72,40)` | 2 | 11 | visible, `z=14` |
| 4 | `$3b2c` | `(72,40)` | `(56,42)` | 2 | 11 | visible, `z=14` |
| 5 | `$3b30` | `(56,42)` | `(68,64)` | 3 | 10 | visible, `z=14` |
| 6 | `$3b32` | `(68,64)` | `(32,44)` | 3 | 10 | visible, `z=14` |
| 7 | `$3b38` | `(32,44)` | `(36,96)` | 4 | 9 | visible, `z=14` |
| 8 | `$3b3c` | `(36,96)` | `(8,80)` | 4 | 9 | visible, `z=14` |
| 9 | `$3b40` | `(8,80)` | `(-20,90)` | 5 | 11 | visible, `z=14` |
| 10 | `$3b42` | `(-20,90)` | `(-24,36)` | 5 | 11 | visible, `z=14` |
| 11 | `$3b48` | `(-24,36)` | `(-92,48)` | 6 | 10 | visible, `z=14` |
| 12 | `$3b4c` | `(-92,48)` | `(-56,0)` | 6 | 10 | visible, `z=14` |
| 13 | `$3b52` | `(-56,0)` | `(-76,-16)` | 7 | 9 | visible, `z=14` |
| 14 | `$3b54` | `(-76,-16)` | `(-64,-20)` | 7 | 9 | visible, `z=14` |
| 15 | `$3b58` | `(-64,-20)` | `(-76,-46)` | 8 | 11 | visible, `z=14` |
| 16 | `$3b5a` | `(-76,-46)` | `(-26,-28)` | 8 | 11 | visible, `z=14` |
| 17 | `$3b60` | `(-26,-28)` | `(-32,-68)` | 9 | 9 | visible, `z=14` |
| 18 | `$3b64` | `(-32,-68)` | `(-24,-64)` | 9 | 9 | visible, `z=14` |
| 19 | `$3b68` | `(-24,-64)` | `(-16,-88)` | 10 | 10 | visible, `z=14` |
| 20 | `$3b6a` | `(-16,-88)` | `(8,-64)` | 10 | 10 | visible, `z=14` |
| 21 | `$3b6e` | `(8,-64)` | `(32,-88)` | 11 | 11 | visible, `z=14` |
| 22 | `$3b70` | `(32,-88)` | `(36,-48)` | 11 | 11 | visible, `z=14` |
| 23 | `$3b76` | `(36,-48)` | `(68,-60)` | 12 | 10 | visible, `z=14` |
| 24 | `$3b7a` | `(68,-60)` | `(48,-16)` | 12 | 10 | visible, `z=14` |

Non-drawing operations:

| Address | Operation | Start | End | Result |
|---|---|---|---|---|
| `$3b1c` | `VLDRAW z=off`, delta `(+48,-16)` | `(0,0)` | `(48,-16)` | Initial pen-up positioning |
| `$3b7e` | `VLDRAW z=off`, delta `(-48,+16)` | `(48,-16)` | `(0,0)` | Returns to local origin; not a visible stroke |

The stat groups, in stroke order, are `9,11,10,9,11,10,9,11,9,10,11,10`.
Each group contains two consecutive visible strokes; all use `z=12`,
`sparkle=1`, and the strokes use `z=14`.

## Reconstructed local picture

- Visible segment count: **24**.
- Stat/color groups: **12** (three distinct codes: 9, 10, 11).
- Raw local bounds: `x=[-92,112]`, `y=[-88,96]`.
- Width: **204** raw coordinate units; height: **184**.
- The coordinate bounds center is `(10,4)`, not the entry origin `(0,0)`.
  The picture is asymmetric and slightly offset relative to its starting
  point; its path is not a set of origin-centered spokes.
- Topology is one connected polyline, entered by a pen-up move and ended by a
  pen-up return. Color changes divide that line into twelve two-stroke
  sections. There are no separated pen-up fragment objects inside the picture.
- The path has angular turns and broad outward/returning arms around the
  local area. It is reasonably described as an angular fragment/star picture,
  but not as detached pieces or twelve disconnected radial arms.

## Comparison with current C# geometry

The current renderer submits three 16-edge closed outlines: **48 visible
segments**, in yellow, orange-red, and white. Their radii share one asymmetric
16-point sequence; the layers are small fixed rotations and scales of that
sequence. Each color therefore traces a complete overlapping contour.

The Atari picture has half as many strokes, no closed three-contour structure,
and color distributed along successive angular sections of one connected
path. It is asymmetric both in its raw bounds and in its turns. Color codes
alternate across the path's sectors rather than coloring three full outlines.
The C# rotations reproduce neither this path topology nor its color
distribution; they provide a layered starburst impression, but are
fundamentally different geometry.

This angular, grouped path plausibly explains the compact angular breakup and
the broader arms described in the already-authorized MAME observation. The
important qualification is that the ROM path itself is connected, not
literally separated fragments. No claim is made here about screenshot
pixel-level correspondence.

Changing only the schedule scales the same three closed C# contours in and
out. It cannot change their 48-segment topology, complete-color-ring
distribution, or angular relationships. Thus the reported lack of meaningful
visual difference is expected and supports addressing geometry rather than
revisiting the schedule.

## Implementation-ready translation

Use one immutable local segment table in original draw order. Each entry
contains the local start/end coordinates above and its color code/group.
Represent only the 24 visible lines; omit the two pen-up moves from submitted
draw calls. Keep the table shared by legacy and Atari presentation; after the
existing `DrawPlayerExplosion` center is computed, pass transformed endpoints
to the existing draw callback. No fragment objects or gameplay state are
needed.

Use conventional DVG-local axes with positive X right and positive Y up.
Convert into Raylib's screen coordinates with a Y inversion:

```text
screenX = centerX + (rawX - 10) * coordinateScale * stageScale
screenY = centerY - (rawY - 4) * coordinateScale * stageScale
```

Subtracting `(10,4)` centers the raw bounding box on the already accepted
explosion center. The largest raw dimension is 204. The existing accepted
150%-scale C# peak has a 156-pixel maximum diameter (`52 * 1.5 * 2`), so one
centralized coordinate scale of `156 / 204 = 13/17`, approximately
`0.7647059` screen pixels per raw unit, gives the new picture the same
maximum width without screenshot fitting. Its corresponding peak height is
approximately 140.7 pixels because the Atari picture is not square. Apply
the existing stage scale as a single global scale to every endpoint; do not
scale segments independently.

The project already has yellow, orange-red, and white RGB choices. A bounded
**PROJECT TRANSLATION** can map code 9 to yellow, code 11 to orange-red, and
code 10 to white, retaining the current three arcade-observed color families
while attaching them to the ROM-derived path groups. This mapping is not
established as the literal Atari palette mapping: the tracked vector listing
identifies the codes but does not state their RGB values, and this
investigation did not inspect the omitted palette data. Keep numeric color
codes in the geometry data so this translation remains explicit and
replaceable without changing path topology.

Retain the existing center, stepped stage order/holds, 60-tick project
translation, deterministic output, and shared legacy/Atari local geometry.
Do not alter death detection, lives, collision, re-entry, duration, enemy
retraction, score, recording, gameplay state, Level 11, or Fuseball behavior.

## Focused regression tests for that bounded implementation

1. Assert exactly 24 ordered visible segments and all exact local endpoints
   and group/color-code assignments in the table above.
2. Assert twelve contiguous groups in the listed order, two segments per
   group, and only color codes 9/10/11.
3. Assert the initial pen-up and terminal return are not submitted as lines;
   the last visible endpoint is `(48,-16)`, and the terminal move returns to
   `(0,0)`.
4. Assert raw bounds `[-92,112] x [-88,96]`, then assert the chosen transform
   centers the bounds on the supplied death center and gives a 156-pixel
   maximum width at size 6. Verify positive DVG Y maps upward on screen.
5. Assert every stage uses the same local geometry, only globally scaled by
   its existing stage size, including deterministic identical submissions
   for repeated reads at the same tick.
6. Preserve tests that prove the effect stays render-only and that both
   legacy and Atari presentation paths use the shared local geometry without
   changing gameplay or the existing death center logic.

No tests or builds were run: this was a read-only evidence investigation and
no production/test files changed. The required implementation would need its
own focused regression run and Neal's manual visual acceptance.

## Final recommendation

READY FOR FINAL BOUNDED IMPLEMENTATION
