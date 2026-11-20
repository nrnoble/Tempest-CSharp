# Direct Atari Tube-Renderer Architecture Investigation

**Date:** 2026-11-16
**Status:** READ-ONLY INVESTIGATION COMPLETE
**Feasibility:** **FEASIBLE ONLY AS EXPLICIT PROJECT TRANSLATION**

## Executive conclusion

A direct Atari-record presentation path is technically supportable without
replacing or mutating C# gameplay geometry. The normal-game ROM pipeline
provides a concrete integer transform from raw record points and an explicit
runtime position to signed 16-bit coordinates, then a source-backed DVG packing
boundary. The old “mode `$0f` is unresolved” reports are obsolete: `$0f` is the
low quotient-count input, and `$c098` uses the `$14` write to start `DIVZ`.

This does **not** establish a fully arcade-faithful Level 7/gameplay renderer
from current evidence. Three material seams require explicit presentation
translations or later calibration:

1. Level 7's current closed C# clover and record 06 are topologically
   isomorphic, but not an exact indexed point conversion. The numerical `+5`
   cyclic fit is not evidence that Atari lane `i+5` is the accepted gameplay
   identity for C# lane `i`.
2. C# depth/height units are project units, not Atari `$57` positions. Near/far
   endpoints support an affine endpoint correspondence, but below-rim,
   Approach, and exit states leave the normal Atari byte domain or have
   different transition semantics.
3. The canonical signed DVG words are not C# portrait pixels. Monitor polarity,
   physical rotation, X/Y gain/aspect, and final viewport calibration remain a
   global display boundary.

Thus a **tube-only, opt-in Level 7 prototype** is meaningful if it states its
conditional Math Box state and uses clearly labeled lane/depth translations.
It must not claim exact gameplay correspondence, change accepted Levels 1-6,
alter Level 7 geometry, or become the default. No renderer was implemented and
no visual acceptance is claimed.

## Evidence reconciliation

### Obsolete conclusions

The following conclusions in earlier reports are superseded by the controlling
2026-11-15 `$c098` reconciliation:

- `$0f` is not a persistent projection mode. Atari's Math Box interface
  documents offset `$0c` as `NL`, the number of quotient bits; initialization
  writes `$0f` there as the low-byte input.
- `$c098` does not start its divide by writing `$0f`. The instruction accesses
  are `$6095` (`XPL`), `$6096` (`XPH`), `$608e` (`ZLH`), and `$6094`
  (`SZXD`). Earlier `$600e/$6014/$6015/$6016` notation is function-offset
  shorthand, not the actual CPU addresses. Offset `$14` starts `DIVZ`.
- The coordinate operation is not wholly unknown. The relevant mapping-PROM
  entries and bounded microcode path were reconstructed and match
  `MBUCOD.V05.txt`; local PROMs and CPU ROM were boundedly checked in the
  controlling handoff.
- The earlier absence-of-arithmetic/numeric-oracle conclusion is obsolete for
  explicitly supplied normal-game inputs. `DIVZ` is signed division; for the
  nonnegative magnitudes and positive divisor in the normal coordinate path,
  the quotient is the integer quotient with remainder discarded.

### Controlling cross-verified facts and limits

Original Atari `MBUDOC.DOC` calls `$0c` `NL`, `$0e` `ZLH`, `$14` `SZXD` and
states that a write to `$14` starts `Z/X'` division. `MBUCOD.V05.txt` identifies
the corresponding `DIVZ` and signed/unsigned division flow. The reconciliation
report cross-checks those functions against only the required local mapping
entries, microcode path, CPU ROM range, and table bytes; it does not claim
whole-PROM equivalence.

The normal setup writes the selected scale as the sign-extended negative
`$5b:$5f` pair. Given current runtime position `p=$57`, this makes the divisor
`p + lev_scale`. For unsigned axis magnitude `d`, the byte is placed in the
high byte of the 32-bit dividend, explicitly forming `d * 256`. With quotient
count low byte `$0f` and zero high-byte state, the recovered normal operation
is:

```text
q = floor((d * 256) / (p + lev_scale))
```

CPU code separately restores the sign, adds/subtracts the axis base offsets,
and saturates signed overflow to `$7fff` or `$8000`. The selected Y2D pair is
the Y-derived base; normal tube setup initializes the X base to zero.

This is a deterministic arithmetic contract for explicit normal-game inputs
**conditional on the quotient-count high byte being zero**. That high-byte/reset
state is not established by the available documentation or traced CPU writes.
Also unresolved are the physical read-port alias decode, arbitrary-call
retained state, and call-time runtime position/offsets when they are not
supplied. These are not silently treated as known. Controlled inputs make the
normal software transform usable for a bounded prototype, but not a claim of
bit-exact hardware reproduction.

### Evidence classes

- **ATARI-DOCUMENTED:** Math Box interface/function table and symbolic
  `DIVZ`/division description.
- **ROM/PROM-CROSS-VERIFIED:** Relevant mapping-PROM entries and microcode
  path, plus bounded CPU-ROM and tube-table ranges as recorded in the
  controlling reconciliation.
- **ROM-DERIVED-DISASSEMBLY-BACKED:** Actual CPU accesses, setup, runtime
  operands, signs, offsets, saturation, tube selection, and vector serialization.
- **INFERENCE:** `$57` is runtime along-tube position, supported by draw
  callers and the disassembly's function annotation, not by the Math Box
  interface definition alone.
- **PROJECT TRANSLATION / APPROXIMATION:** C# depth-to-`$57` conversion, Level 7
  lane phase, display-to-viewport map, and presentation of states outside the
  normal Atari position range.

## Atari tube pipeline

| Stage | Inputs and output | Scope, units, signedness/orientation | Evidence |
|---|---|---|---|
| Displayed level | Current level index enters `get_tube_no`; `lev_remap[level]` yields a tube-table row. Level 7 selects row `06`; Levels 8/9 select `07`/`0d`. | Per level; table indices are unsigned bytes. | Disassembly and recovered ROM table; C# progression association verified in the coordinate-frame report. |
| Record selection | The selected `lev_x/lev_y` row is copied into `tube_x/tube_y`, 16 points in stored order. | Per record and point; raw coordinates are unsigned bytes `0..255`, not host pixels. | Disassembly and ROM-backed table recovery. |
| Per-record parameters | Row 06 has `lev_scale=$18` (24), `lev_y3d=$68` (104), `lev_y2d/$y2db=$70/$ff` (signed `-144`), `lev_open=$00`. | Scale, Y reference, and signed Y2D are per record; `lev_open` is per record and controls closed/open traversal, not the quotient. | Disassembly and ROM-backed table recovery. |
| Runtime position | Normal tube precomputation calls the shared coordinate routine at `$57=$f0` and `$57=$10`. The register is a per-cross-section position used for all points at that pass. | Unsigned byte position; normal tube endpoints are `240` and `16`; not C# depth units. | Disassembly at `$c30d-$c333`, `$c473-$c4de`. |
| Axis input formation | `dX=abs(rawX-$80)`; `dY=abs(rawY-lev_y3d)`. The respective comparison determines sign. `$5e` is initialized to `$80`. | Raw-byte differences `0..255`; magnitudes unsigned; sign retained separately by CPU. X/Y are source components, not yet screen axes. | `$c098-$c0e5`, setup at `$c16e` and `$c235-$c25a`. |
| Divisor | With `$5b:$5f=$ff:(-lev_scale mod 256)`, `$c098` forms `X'=p+lev_scale`; a negative high-byte result is replaced by one. | 16-bit positive divisor in normal tube setup; runtime position plus per-record scale. | Disassembly, Math Box report, and bounded PROM reconciliation. |
| Dividend/division | Put `d` in `ZLH`, leaving the other initialized dividend bytes zero; `$6094` starts `DIVZ`. The result is `q=floor(d*256/X')` for normal positive operands. | 32-bit unsigned magnitude dividend, signed 16-bit quotient output; the factor 256 is byte placement, not an assumed camera constant. | Atari docs, ROM/PROM cross-check, `DIVZ` trace, and external micro-PC trace. |
| CPU result | Restore sign; add/subtract current X or Y base; saturate signed overflow to `+32767`/`-32768`. The usual tube setup uses X base zero and the record's signed Y2D pair. | Signed 16-bit source-coordinate results. Y2D affects the Y-derived result. | ROM-derived disassembly; bounded arithmetic in reconciliation. |
| DVG serialization | Source-Y-derived result is serialized as vector X; source-X-derived result as vector Y. The high three bits are dropped, yielding signed 13-bit words. | DVG signed coordinates `-4096..4095`; X/Y component names are swapped relative to source component names. 13-bit packing truncates; it is not clipping or another saturation. | `vapp_to(X)` at `$c765-$c79d` and DVG translation handoff. |
| Vector state/list | Tube generation uses the projected point arrays and emits an ordered DVG list; closed rows traverse the closing edge, open rows omit it. The list includes vector scale and `vcentre` state (`vscale b=1,l=0` / word `$7100`; center word `$8040` in the cited helper). | Shared display-list state plus per-tube ordered vectors/status. `vcentre` is the logical DVG center; the physical center is analog-adjusted. | `$c30d-$c3b9`, `$c473-$c4de`, `$c765-$c79d`; ROM-derived source. |
| Display/host | AVG analog outputs feed the X-Y display and must ultimately map to C#'s portrait viewport. | One global transform for all records; physical signs, rotation, gain, aspect, and monitor effects remain uncalibrated. | Atari monitor-orientation handoff; physical transform not established. |

The common normal-game path is more specific than “draw raw points”: the
disassembly precomputes each raw point at the two runtime positions and uses
the resulting projected point arrays for tube rails and rims. A separate
raw-point/vector helper exists elsewhere, but it does not replace the `$c098`
normal gameplay projection path.

### Level 7 record 06

```text
lev_x[06]: ea e0 9c 80 64 20 16 50 16 20 64 80 9c e0 ea b0
lev_y[06]: a0 e0 ea b0 ea e0 a0 80 60 20 16 50 16 20 60 80
```

These are 16 ordered unsigned raw points. The record is closed. `lev_open` is
not part of coordinate arithmetic; it selects whether traversal includes the
closing edge.

## Arithmetic contract

For an immutable raw point `(x,y)`, explicit runtime position `p`, row scale
`s`, camera-height reference `r`, current X base `Bx`, and Y base `By`:

```text
dx = abs(x - 128)
dy = abs(y - r)
divisor = p + s
qx = floor((dx * 256) / divisor)
qy = floor((dy * 256) / divisor)
x16 = sat16(Bx + sign(x - 128) * qx)
y16 = sat16(By + sign(y - r) * qy)
vX13 = signed13(y16)    // source-Y result becomes DVG X
vY13 = signed13(x16)    // source-X result becomes DVG Y
```

For normal tube record setup, `Bx=0` and `By` is the selected signed
`lev_y2d/lev_y2db` word. For other callers, both bases and `p` are explicit
inputs; using stale global state would be incorrect. `sat16` is the Atari CPU
overflow saturation. `signed13` means the source's DVG packing drops the top
three bits after signed-16 saturation; it is not signed-13 saturation.

The formula uses integer division and must not be changed to floating point for
the coordinate words. The unknown Math Box `RN` high-byte/reset state remains
a qualification on representing actual retained hardware state; a prototype
must state its `NL=$0f`/zero-high-byte assumption. A future host glyph scale may
use a derivative of this formula, but it is a separate presentation
calculation, not a replacement for integer coordinate output.

## Objective test vectors

### External micro-PC trace

The existing external coordinate trace provides independent conditional
microcode vectors. Its cases match the recovered quotient when the listed
`depth` is used as both runtime position and scale:

| Magnitude `d` | `p` | `s` | Integer quotient | Trace result |
|---:|---:|---:|---:|---:|
| 0 | 13 | 13 | 0 | `$0000` |
| 1 | 13 | 13 | 9 | `$0009` |
| 64 | 32 | 32 | 256 | `$0100` |
| 255 | 13 | 13 | 2510 | `$09ce` |
| 64 | 283 | 283 | 28 | `$001c` |

These verify the bounded arithmetic trace, not the Math Box register reset
state in a live cabinet.

### Record-backed coordinate examples

For record 06, point 0 `(x=$ea,y=$a0)`, `s=24`, `r=104`, and `By=-144`:

| `p` | Divisor | `dx,dy` | `qx,qy` | Signed source result `(x16,y16)` | DVG `(Vx,Vy)` |
|---:|---:|---:|---:|---:|---:|
| 16 | 40 | 106, 56 | 678, 358 | `(678,214)` | `(214,678)` |
| 240 | 264 | 106, 56 | 102, 54 | `(102,-90)` | `(-90,102)` |

Both DVG pairs fit the signed 13-bit range and are unchanged by packing.
Similarly, the reconciliation report's conditional record-00 example uses
`p=80`, `s=24`, point `(240,128)`, `r=80`, and `By=-192`: divisor 104,
`qx=275`, `qy=118`, source results `(275,-74)`, DVG `(Vx,Vy)=(-74,275)`.
The example's call-time inputs are explicit assumptions, not an observed
frame.

### Test classification

- **Atari-documented:** Math Box register/function behavior, `NL`, `DIVZ`
  symbolic divide operation, DVG vector-center/scale/vector instruction shape.
- **ROM/PROM-cross-verified:** `$c098` operation selection and required mapping
  entries/microcode path; selected CPU instruction/table behavior.
- **Arithmetic implementation tests:** integer quotient, sign restoration,
  offset application, signed-16 saturation, axis swap, and signed-13 packing
  for explicit input tuples.
- **Project-translation tests:** Level 7 cyclic lane bridge; C# depth endpoints
  to Atari `p`; host/global map. These cannot be called Atari-ROM tests.
- **Visual/manual only:** actual vector-to-host orientation/gain/aspect,
  portrait composition, monitor-specific appearance, and acceptance of the
  approximate Level 7 registration.

## Global display-map classification

**GLOBAL MAP PLUS ONE DISPLAY-ASPECT PARAMETER**

The record-specific shape and all per-level projection inputs are upstream of
the common DVG vector list: raw point rows, `lev_scale`, `lev_y3d`, signed
Y2D, open/closed traversal, and runtime `p`. The normal tube path applies
shared vector state and does not require independent per-record host
calibration. `vcentre` defines the logical center, while the actual center is
adjusted in analog hardware.

The exact canonical DVG-to-portrait matrix is **not** established: positive
digital X/Y signs through the AVG and monitor are unknown; so are upright
cabinet rotation, net axis permutation/reflection, vector-unit-to-pixel gain,
and physical extents. Atari's monitor material documents horizontal X and
vertical Y inputs and separately adjustable axis size/center; its different
X/Y electrical ranges mean equal DVG integers need not have equal physical
deflection. One global translation, orientation/axis map, overall scale, and
global aspect ratio are therefore the right boundary. Clipping and CRT
characteristics are display-level concerns; no evidence here requires
per-tube correction.

No screenshots or images were used to fit this mapping. The project's current
portrait camera/`-33.1` placement and any Level-specific calibration are
project presentation choices, not Atari vector units.

## Level 7 lane bridge

**Classification: STABLE TOPOLOGICAL CORRESPONDENCE WITH GEOMETRIC APPROXIMATION**

Both C# Level 7 and Atari record 06 contain 16 ordered vertices and form a
closed 16-edge cycle. The C# definition exposes 16 playable lanes and wraps
adjacency; Atari `lev_open=$00` includes the closing edge. A forward cyclic
mapping preserves vertex count, adjacency, the opposite-neighbor relation
(`i` to `i+8`), and direction of indexed transitions without changing C#
state. The inspected coordinate arrays also have the same winding sign before
any global monitor transform. Thus a stable **topological** bridge exists.

The strongest numerical fit cited by the existing coordinate-frame report is
C# lane `i` to record point `(i+5) mod 16`, with uniform scale about `1.679`.
Its maximum coordinate residuals are about 8.41 and 9.73 units and RMS
distance is about 6.70; this is far beyond integer rounding. The prior `+5`
was a comparison alignment, not an established lane-numbering rule. No
historical or Atari evidence makes it an exact gameplay correspondence.

Disposition: reject `+5` as a proven or accepted gameplay identity; it may be
used only as an explicitly named candidate approximation for render-only
markers, then manually evaluated. Do not rotate or edit C# points, lane
indices, adjacency, pivots, collision rules, or gameplay transitions.

## C# depth bridge

**Classification: ENDPOINT-NORMALIZED PROJECT TRANSLATION** for the ordinary
rim-to-web interval; **state-dependent/approximate outside it**.

Current C# Level 7 uses `Level.Position=50`, `Level.Depth=400`; the near rim is
`z=50`, far rim `z=450`. Normal Atari tube preprocessing uses `p=$10` and
`p=$f0` for the two projected endpoint sections. A deterministic bridge for a
prototype can normalize:

```text
p = 16 + 224 * (z - 50) / 400
```

then explicitly quantize to the Atari byte domain for integer projection.
`z=50,250,450` maps to `p=16,128,240`. The precise rounding rule is a project
decision; no ROM evidence maps C# units to these byte positions.

| C# state | Existing render depth | Bridge implication |
|---|---|---|
| Near rim | `z=Position=50` | `p=16` |
| Far rim | `z=Position+Depth=450` | `p=240` |
| Player, normal play | `Player.Depth=400`; projected at near rim | `p=16` |
| Player, exit | `Player.Depth` decreases toward 0; `z=Position+Depth-Player.Depth` moves 50 to 450 | endpoint-mapped `p=16..240`; exit's zoom/state remains a separate translation |
| Enemy / Tanker / Spiker / Flipper | `z=Position+Depth-Height` | ordinary height `0..400` maps from far to near; enemy retraction adds a render-only offset |
| Shot | `z=Position+Depth-shot.Height`; shot height runs from player's depth toward 0 | ordinary shot path spans near to far endpoints |
| Spike | line starts at far rim and tip uses `Position+Depth-spike.Height` | far endpoint and interior tip can use same map |
| Below-rim Egg | renderer uses `Position+Depth-20*Height`; roaming height is `-10..-20` | produces `z=650..850`, or `p=352..464`, beyond both normal tube range and `$57`'s byte range; requires an explicit extension/approximation, not silent byte wrap |
| Approach | presentation adds a 960-unit depth offset and vertical translation | endpoint-normalizing after adding the offset would cancel the approach travel; it needs a separate global transition transform |
| Tube exit | C# changes player depth and animates `Projection.ZOffset`/`K` | the player coordinate can be endpoint-mapped, but the Atari-to-host zoom/exit presentation is not thereby recovered |

Thus ordinary entity depth has a coherent render-only endpoint bridge, but it
is not an exact affine correspondence between Atari and C# world units.
Out-of-range states and transitions cannot be claimed ROM-faithful from this
map. A Level 7 prototype should initially avoid gameplay mutation and limit
its registration overlay to normal in-tube depths.

## Tube-attached object strategy

**Select Strategy C: hybrid.**

Use the direct Atari-rendered rails and record topology as the geometric
reference; map C# lane/edge and depth state to a location on those rails using
the explicit cyclic and endpoint translations. Interpolate anchors along the
corresponding rendered edge. This preserves current C# lane identity and
continuous edge position without pretending the Level 7 C# point array is an
exact ROM conversion. For complex in-progress pivots, derive the visual anchor
from the C# transition state in the render-only bridge, not by mutating the
gameplay transition or its endpoints.

- Strategy A (rendered-lane interpolation) is the safest anchor for rim lanes,
  but by itself does not define per-object scale/orientation or special
  transitions.
- Strategy B (per-object Atari projection) is suitable only when a raw Atari
  coordinate is actually defined for the object. Deriving one from every
  current C# pivot/glyph would invent a coordinate conversion and exacerbate
  the Level 7 residual.
- The hybrid supports Player, Flippers, Tankers, Spikers, Fuseballs, shots,
  enemy bullets, and Spikes at ordinary mapped depths. Fuseball lateral `W`
  and Flipper pivots need render-only interpolation consistent with their
  existing simulation state. Eggs beyond the byte position domain need a
  separately labeled extension. Approach and exit remain separate transition
  translations and are out of scope for the first prototype.

### Local scale and orientation

Do not use the current C# `K/(z+ZOffset)` glyph scale in a direct Atari path.
At a supplied runtime position, derive local screen basis/scale from the
recovered projection: either project neighboring Atari points on the same
rail, or evaluate the derivative of `256*d/(p+s)` and then apply the same
global DVG-to-host map. For artwork represented as Atari-local vectors, the
most coherent option is to transform its anchor and local basis through this
same projection, accounting for integer quantization where exact vector words
are required. Orient lane-attached shapes using the projected rail tangent and
its normal. The integer coordinate operation remains unchanged; scale is a
separate presentation quantity. This avoids correct anchors paired with an
unrelated perspective size.

## Blaster-exit implications

**Root cause: UNKNOWN; direct-path effect: UNKNOWN.**

The source confirms the existing adapter begins with C# model coordinates and
C# render depth. During player exit, `PlayerProjectedDepth` changes as
`Player.Depth` falls, and the current adapter also has depth-responsive claw
scale and Approach offsets. These are plausible coordinate/depth/local-scale
assumptions relevant to an exit presentation, but the authorized evidence
contains no inspection of the defect's frames or a candidate reproduction.
No evidence selects wrong depth, coordinate domain, lane interpolation, glyph
scale, or state mismatch as its cause.

A direct Level 7 renderer would remove dependence on the existing model-point
adapter for the tube itself, but that does not prove it removes the Level 1-3
Blaster defect. A future direct path needs a separately designed exit
translation. Do not fix or infer the defect from this read-only investigation.

## Accepted Levels 1-6 coexistence

Use a separate development-only Level 7 prototype option (proposed name:
`--atari-tube-prototype`) that is distinct from `--atari-presentation`.
Dispatch only the explicit Level 7 experiment. When disabled, preserve the
current renderer/default path byte-for-byte; do not make the prototype default
or apply it to accepted Levels 1-6. Keep the Level 7 C# geometry and all
simulation state untouched. The current `--atari-presentation` adapter and its
uncommitted source/tests are protected existing work, not inputs to rewrite.

## Smallest prototype architecture

One future milestone, and no implementation in this investigation:

1. Add immutable ROM-derived record data for row 06, including the 16 raw X/Y
   bytes, scale, Y3D, signed Y2D, remap identity, and open flag. Keep the record
   independent of `LevelDefinition.Points`.
2. Add a pure integer `AtariCoordinateProjector` accepting record metadata,
   raw point, explicit `p`, and current X/Y bases. Return saturated signed-16
   components plus their packed signed-13 DVG pair. State the zero-high-byte
   quotient-count assumption in its API/docs; reject invalid divisor/state
   rather than silently substitute a result.
3. Add a pure `AtariTubeVectorListBuilder` that projects all points at the
   Atari normal endpoint positions `p=$10` and `$f0`, preserves row order and
   closed topology, and returns a canonical ordered vector list plus vector
   center/scale state. No C# host pixels in this layer.
4. Add one isolated `AtariDvgViewportMap` boundary for the shared global
   orientation, translation, scale, and aspect. Leave its calibration explicit
   and provisional; do not add per-level fit constants.
5. Wire a separate development-only Level 7 rendering option. The tube
   wireframe comes only from row 06. An optional marker overlay may map lane
   `i` to record edge `(i+5) mod 16` as a project approximation and C# depth
   `50..450` to `p=16..240`; exclude eggs, Approach, and exit.
6. Keep all C# gameplay lanes, geometry, pivots, collisions, entity state,
   timers, and deterministic recordings unchanged.

Candidate files/classes: `src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs`,
`src/Tempest.Game/Rendering/AtariCoordinateProjector.cs`,
`AtariTubeVectorListBuilder.cs`, `AtariDvgViewportMap.cs`, and a separate
`AtariTubeRecordPrototypeTests.cs`. Minimal option wiring may touch
`DebugController.cs`, `Program.cs`, and `VectorRenderer.cs` only in a future
authorized implementation. The existing model adapter, production data,
accepted point arrays, and gameplay systems are not to be edited for this
milestone.

Automated checks should include the record-06 table bytes and row selection;
the two point-0 vectors above; Level 1 point-0 conditional vector; quotient
flooring, signs, offsets, saturation, DVG axis ordering and signed-13 packing;
ordered edges and closed closure; deterministic endpoint/depth inputs; and
proof that the option-disabled renderer does not select the prototype. Any
lane/depth marker checks are explicitly project-translation tests, not ROM
acceptance.

After focused tests and `git diff --check`, stop for Neal. The future launch
command should be the exact Level 7-only option command, for example:

```powershell
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype
```

The command is proposed and will not work until a separately authorized
implementation. The agent must not launch it. No commit before manual
evaluation. Rollback is to disable the new option; the option-disabled
production path and accepted Levels 1-6 remain unchanged.

## Decision-relevant unknowns

| Unknown | Feasibility? | Atari arithmetic? | Lane mapping? | Depth mapping? | Global display map? | Gameplay/model changes? | Misleading Level 7 prototype? |
|---|---|---|---|---|---|---|---|
| `RN` high-byte/reset state | Not for a clearly labeled project candidate; it blocks an unconditional bit-exact claim. | Yes; could alter quotient loop state/results. | No. | No. | No. | No. | Yes, if the zero-high-byte assumption is hidden. |
| Physical result-read alias decode | Not for software arithmetic on verified expected results; relevant to exact bus emulation. | No for the recovered equation; yes only to literal hardware bus behavior. | No. | No. | No. | No. | Only if called hardware-exact. |
| Call-time `p`, X/Y bases, and retained state | No when explicitly supplied from controlled normal setup. | Yes; they are arithmetic inputs. | No. | `p` is the depth seam. | No. | No. | Yes if stale or guessed state is used. |
| Record-06 to C# lane phase/shape residual | No for tube-only rendering; object registration needs the stated translation. | No. | Yes. | No. | No. | No; never change gameplay lanes. | Yes if `+5` is called exact or accepted. |
| C# depth correspondence outside near/far endpoints, plus Approach/exit | No for wireframe and normal in-tube marker; matters for full object coverage. | Changes supplied runtime position, not the quotient formula. | No. | Yes. | No. | No. | Yes outside the demonstrated range. |
| DVG sign/rotation/physical gain/aspect | No; isolate one global calibration. | No. | No. | No. | Yes. | No. | Yes visually until global calibration/manual review. |
| Local glyph scale/orientation under integer projection | No for wireframe; matters for full entity artwork. | No to anchor arithmetic; basis can use the same formula. | No. | Coupled to runtime depth. | Uses the same global map. | No. | Yes for object presentation if borrowed from C# perspective. |
| Blaster-exit defect cause | No for the Level 7 wireframe milestone. | Unknown relationship. | Unknown. | Could be relevant. | Could be relevant. | No evidence authorizes gameplay changes. | Yes if assumed fixed by the new tube path. |
| CRT clipping/nonlinear characteristics | No for canonical coordinates or one shared host map. | No. | No. | No. | May affect final display appearance, not per-tube mapping. | No. | Only if the prototype claims cabinet/CRT emulation. |

No additional investigation is recommended for the read-alias, CRT, glyph, or
Blaster unknowns before a tube-only candidate: they do not change the stated
architecture decision. The quotient-count state must remain an explicit
conditional in tests and reporting; a future claim of exact hardware
reproduction would require resolving it.

## Smallest next milestone

**Implement one isolated, development-only Level 7 record-06 wireframe
prototype** as specified above: integer `$c098` arithmetic with its
zero-high-byte assumption visible, canonical DVG vector output, one shared
provisional viewport map, unchanged gameplay, and optional explicitly
approximate lane/depth markers. Run focused nonvisual tests, then stop for
Neal's manual review. Do not modify accepted Levels 1-6 or commit before
acceptance.

## Scope, evidence, and completion

### Evidence inspected

- Original local Atari sources: `Reference/arcade/MBUDOC.DOC` (Math Box
  interface/function excerpts), `Reference/arcade/MBUCOD.V05.txt` (`DIVZ`
  path), and `Reference/arcade/Tempest Rom Source code.md` (table declarations;
  `$c098`; initialization/setup; `lev_remap`; normal tube endpoint projection
  and display-list generation; DVG packing/centering).
- Controlling and historical reports: the full 2026-11-15 `$c098`
  reconciliation, external Math Box micro-PC trace, PROM coordinate-operation
  decode, DVG coordinate translation, projection-data/Math Box, earlier
  mode-`$0f`, AVG/monitor orientation, ROM metadata foundation, Atari
  presentation-adapter plan and implementation, Level 7 geometry provenance,
  and Atari-to-C# coordinate-frame mapping reports.
- Verified current C# source: `Level.cs`, `LevelConfiguration.cs`,
  `LevelDefinition.cs`, `LevelLoader.cs`, `LevelApproach.cs`,
  `RomPresentationMetadata.cs`, `Player.cs`, `Shot.cs`, `Egg.cs`,
  `EnemyBullet.cs`, `Spike.cs`, `Flipper.cs`, `Tanker.cs`, `Spiker.cs`,
  `Fuseball.cs`, `Projection.cs`, `AtariPresentationAdapter.cs`,
  `VectorRenderer.cs`, `DisplayConfiguration.cs`, and relevant exit/Approach
  state code in `TempestGame.cs`.
- No raw ROM/PROM copies were reopened. Prior bounded local byte/PROM
  verification was taken from the controlling reconciliation. No screenshot,
  image, video, recording, contact-sheet, `TEMP/myNotes.md`, Internet source,
  build, test suite, or game was used.

### Bounded calculations

- Summarized the existing external micro-PC trace by its five input cases and
  final quotient.
- Recomputed the record-06 point-0 `p=16` and `p=240` integer projection
  vectors and the conditional record-00 example.
- Checked the 16-edge cyclic successor and opposite-neighbor invariants for a
  `+5` mapping; checked winding sign from the two ordered coordinate arrays.
- Evaluated the proposed endpoint depth translation at C# depths 50, 250, 450,
  650, and 850. No calculation file or generated output was created.

### Repository state

Starting state:

- Branch `master`; HEAD `385f603a94391756b2ca6cccd524dc17b281e782`;
  upstream `origin/master`; ahead/behind `0/0`.
- Index empty.
- 16 pre-existing tracked modifications: `src/Tempest.Game/Debug/DebugController.cs`,
  `PlaybackScreenshotCapture.cs`; `Levels/LevelLoader.cs`; `Program.cs`;
  `Rendering/DisplayConfiguration.cs`, `Projection.cs`, `VectorRenderer.cs`;
  `Systems/InputSystem.cs`; and tests `DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`, `Level9ProductionTests.cs`,
  `PlaybackScreenshotCaptureTests.cs`, and `TransitionPresentationTests.cs`.
- 8 pre-existing nonignored untracked paths: `Reference/arcade/MBUCOD.V05.txt`,
  `Reference/arcade/MBUDOC.DOC`, `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`, and tests
  `AtariPresentationAdapterTests.cs`, `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`. The screenshot was not opened.

Ending state is unchanged: same branch, HEAD, upstream and ahead/behind; same
16 tracked modifications, same 8 untracked paths, and empty index. Exactly the
two authorized AI-Logs artifacts were created; both are ignored. No source,
test, durable documentation, reference, or visual file was changed. No index,
commit, branch, or remote was changed; nothing was staged, committed, or
pushed. No game was launched. No automated or manual visual validation was
required or performed for this read-only investigation.
