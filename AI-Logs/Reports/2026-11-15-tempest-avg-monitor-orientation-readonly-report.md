# Tempest AVG-to-Monitor Orientation — Read-Only Investigation

**Date:** 2026-11-15
**Status:** PARTIAL — PRIMARY DOCUMENTS ACQUIRED; PHYSICAL TRANSFORM NOT ESTABLISHED
**Primary classification:** **AVG OUTPUT POLARITY GAP**

## Result

Original Atari material was located, including the Tempest drawing-package
sheets for the Analog Vector-Generator PCB and the Wells-Gardner color X-Y
monitor manual. It confirms the Tempest AVG's named X/Y output domain, the
monitor's horizontal-X/vertical-Y input labels, and separately adjustable
X/Y center and size calibration.

The available text and legible net information did **not** establish the
sign from a positive digital VCTR count through the AVG DAC/analog stages to
the board output. The matching monitor input polarity and the upright
cabinet's physical rotation were also not established. The result therefore
stops before a player-visible matrix. No orientation is inferred from MAME,
screenshots, or guessed schematic topology.

No C# projection or other project implementation was made.

## Starting Git state

The bounded pre-research check found:

- Branch: `master`.
- `HEAD` and `origin/master`:
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index: clean.
- Tracked worktree: exactly the expected 16 modified candidate files.
- Non-ignored untracked files: exactly the expected
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs` and
  `tests/Tempest.Game.Tests/RomPresentationMetadataTests.cs`.
- Diff summary: 16 files, 2,021 insertions and 127 deletions.
- No unrelated work was observed.

The substantive prompt was copied byte-for-byte to
`AI-Logs/Prompts/2026-11-15-tempest-avg-monitor-orientation-readonly-prompt.md`.
Both requested log paths are ignored by `.gitignore`.

## Primary Atari documents obtained

1. **Atari, *Drawing Package Supplement to Tempest Operation, Maintenance,
   and Service Manual*, © 1981, 1st printing.**
   - [Sheet 2, Side B, DP-190-02](https://archive.org/download/tempest_dp-190-1st-02b_ptm/Tempest_DP-190-1st-02B.pdf):
     schematic sections of Tempest Analog Vector-Generator PCB
     `037383-01 & -02B`, including the named **Analog Vector-Generator X-Y
     Outputs** and adjustment-pot information.
   - [Sheet 3, Side A, DP-190-03](https://archive.org/download/tempest_dp-190-1st-03a_ptm/Tempest_DP-190-1st-03A.pdf):
     Vector-Generator Data Buffer, Address Selector, Data Shifter, Timer,
     and state-machine sections.
   - Sheet 2 Side B was available as an actual one-page schematic scan. Its
     OCR identifies circuit blocks and pot names, but did not provide a
     sufficiently traceable digital-to-analog net/polarity path. Sheet 3
     Side A likewise does not, by itself, close the analog output path. No
     Sheet 3 Side B was obtained.

2. **Atari, *Tempest Operation, Maintenance and Service Manual*, TM-190,
   1981, 2nd printing.**
   [Internet Archive scan](https://archive.org/details/arcademanual_Tempest).
   The introduction identifies Tempest as a color X-Y game; Figure 13
   concerns monitor removal, and Figure 18 is titled “Cabinet-Mounted
   Assemblies” (printed p. 32). Neither the searchable text nor the
   consulted material established a named portrait rotation direction,
   physical coil polarity, or a connector pin-to-axis/sign map.

3. **Atari, *Wells-Gardner Color X-Y Monitor Service Manual*, TM-183,
   Atari part no. 92-053, 1981, 1st printing; monitor manufactured by
   Wells-Gardner Electronics Corp.**
   [Internet Archive scan](https://archive.org/details/wells-gardner-color-x-y-monitor-service-manual).
   Chapter 2, “Specifications,” section A.4 names **X horizontal** and
   **Y vertical** signal inputs (16 V peak-to-peak and 12 V peak-to-peak,
   respectively, at maximum deflection). The manual's Chapter 7 lists
   monitor circuit schematics, including a Y-amplifier schematic and the
   monitor schematic. The acquired text does not state positive input
   voltage's player-visible direction. No polarity conclusion was taken
   from the OCR. The separate Wells-Gardner 19V2000 monochrome manual was
   not used as the Tempest color-monitor authority.

## Bounded hardware findings

### AVG and board output

- **Established from the Atari drawing-package titles and labels:** this is
  Tempest's Analog Vector-Generator PCB; its documentation names analog
  vector-generator X-Y outputs.
- **Not established:** the complete digital AVG coordinate/current-position
  output to DAC, DAC polarity, any intervening inversion, integrator/output
  polarity, and the precise board connector pins carrying those outputs.
- The retrieved sheet 2 Side B did not support an end-to-end symbol/net
  trace with polarity. Therefore no claim is made that any op-amp or
  integrator stage is inverting or non-inverting.
- The Atari manual's self-test instructions refer to adjusting X-center
  and Y-center and X-size and Y-size pots. The drawing-package supplement
  also documents X/Y output adjustments. These are calibration controls,
  not evidence of a fixed sign or exact voltage-per-vector-unit.

### Monitor, harness, and cabinet

- The Atari monitor manual identifies the two signal axes as X-horizontal
  and Y-vertical. Its peak-to-peak specifications show that a common exact
  physical gain cannot be assumed from the word format alone.
- The monitor input connector pin assignment, board-to-monitor harness
  mapping, deflection-amplifier polarity, and yoke lead polarity were not
  established from the retrieved evidence.
- TM-190 confirms the upright cabinet and includes a cabinet-mounted
  assemblies figure, but the inspected references did not establish
  whether the monitor is rotated clockwise or counterclockwise relative to
  the screen axes. Physical player-visible signs therefore remain unknown.

### Center, scale, and logical units

- The preceding source-side investigation establishes that `vcentre`
  precedes the relevant VCTR and resets the logical beam position to the
  vector-generator center.
- Atari self-test documentation says to use the X/Y center adjustments to
  center the displayed crosshatch. Thus logical `(0,0)` is the intended
  physical display center **after analog calibration**; no fixed,
  post-`vcentre` offset was established. Real-unit calibration can shift
  the center.
- X and Y size controls and the monitor's distinct X/Y input ranges mean
  equal VCTR integers are not proven to produce equal physical deflection.
  Keep the canonical logical unit one signed VCTR integer unit; leave
  physical gain/aspect correction to a later presentation-fit stage.
- No clean hardware-visible VCTR range or exact physical gain was
  established. No millimeter or pixel conversion is justified.

## Orientation, matrix, and vectors

The verified input to any future conversion remains the centered signed
VCTR displacement `(Vx, Vy)`, with each encoded component in
`[-4096, +4095]` under the preceding `vscale b=1, l=0` path. The required
player-visible convention would be `+Px = right`, `+Py = down`, but the
primary sources obtained here do not establish its signs or rotation.

**No canonical matrix is asserted.** In particular, none of axis swap,
X inversion, Y inversion, or 90/270-degree rotation is selected. There is
no supported center translation beyond the calibrated logical origin.

The Atari self-test vector-list examples remain a word-level oracle, not a
physical-orientation oracle:

| VCTR example | What is established | Player-visible direction |
|---|---|---|
| `(+60, 0)` | X-only signed VCTR displacement | Unknown |
| `(0, +256)` | Y-only displacement; Atari source labels repeated such strokes as vertical self-test lines | Unknown sign/orientation |
| `(+16, -256)` | Mixed signed X/Y displacement | Unknown |

The prior verified representative examples are retained at the VCTR stage;
they cannot be carried into canonical portrait coordinates without
guessing:

| Level | Centered VCTR `(Vx, Vy)` | Canonical portrait result |
|---:|---:|---|
| 1 | `(-96, +64)` | Not established |
| 4 | `(-73, +74)` | Not established |
| 7 | `(-96, +64)` | Not established |
| 8 | `(0, +64)` | Not established |
| 9 | `(+202, +78)` | Not established |

## Cross-checks and project relationship

- **MAME:** the prior read-only report records MAME `AVG_TEMPEST`'s
  pre-render direction pair `(-Vy, +Vx)`, Tempest `ROT270`, and vector-buffer
  orientation handling. These are reference-derived emulator transforms.
  Since the Atari physical sign/rotation matrix remains unknown, equivalence
  to the physical monitor result cannot be tested. `ROT270` is not used as
  Atari wiring proof.
- **Hardware-level FPGA:** no vector-output orientation cross-check was
  performed in this investigation. The prior MiSTer work concerned the
  Math Box and does not close this physical path.
- **Current C#:** the existing 600x800 portrait logical area remains a
  suitable *modern final-fit target*, not an Atari-native coordinate
  system. The conceptual pipeline remains ROM metadata → verified Math Box
  transfer → signed VCTR → future Atari canonical conversion → modern
  presentation fit → 600x800 portrait logical area → 720x960 viewport →
  1280x960 host.
- Preserve the frozen `-33.1` placement, `1.05` scale,
  `--portrait-perspective` mechanism, and manually observed Level 9
  approximately `+125` as project calibration/translations. Reevaluate
  them only after ROM-backed Atari projection is active; do not fold them
  into the Atari matrix.
- No change to lanes, `LevelDefinition.Points`, collision geometry, or
  player/enemy paths is supported. Any later raw-Atari-model-to-gameplay
  bridge remains a separate future translation seam.

## Future objective oracle

The smallest independent automated oracle, once the matrix is established,
should test:

1. Known raw VCTR instruction words, including signed-13 boundaries and the
   three Atari self-test-list examples above.
2. `vcentre` producing exact centered origin `(0,0)`.
3. The five representative Math Box results listed above.
4. The primary-source-derived sign/permutation matrix applied to basis
   vectors `(1,0)` and `(0,1)` and to mixed-sign inputs.
5. Agreement with the MAME sign/permutation only as an independent
   behavioral cross-check, not as the matrix's source.

Items 1–3 can be tested now at the native VCTR-word layer. Items 4–5 are
blocked until the Atari hardware transform is resolved; screenshots are
not a validation oracle.

## Classification and next milestone

**AVG OUTPUT POLARITY GAP**

The narrowest unresolved hardware link is the positive/negative direction
through the Tempest AVG output stages and into the monitor's analog
deflection channels. The monitor input labels establish axis names but not
input polarity. The cabinet's physical monitor rotation is also still
unproved; it must be resolved before claiming a player-visible matrix.

The smallest next milestone is a read-only acquisition and legible,
symbol-level trace of the remaining Atari drawing-package AVG output sheets
(including Sheet 3 Side B) together with the TM-183 monitor deflection
schematic and the upright cabinet mounting drawing. Trace the two
coordinates through DAC/output stages, board connector and harness, monitor
amplifiers/yoke, and physical cabinet mounting. Stop if any sign or rotation
still depends on inference.

## Final state and protections

- Branch remains `master`; `HEAD` and `origin/master` remain
  `385f603a94391756b2ca6cccd524dc17b281e782`.
- Index remains clean.
- The same 16 tracked candidate files remain modified; the same two
  metadata/test files remain the only non-ignored untracked files.
- Only the ignored prompt copy and this ignored report were created.
- No C# source, tests, metadata, durable project documentation, candidate,
  ROM/PROM/reference files were modified.
- Nothing was staged, committed, or pushed.
- No gameplay screenshots or gameplay recordings/videos, Gemini output, or
  Incitti material was inspected. Browser imagery was limited to the cited
  technical-manual scans and was not used to infer missing signal topology
  or orientation. `TEMP/myNotes.md` and the protected raw ROM/PROM contents
  were not read or accessed.
- No game, build, or test suite was run.
