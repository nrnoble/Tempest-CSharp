# Blaster Death Visual-Animation Read-Only Investigation

Date: 2026-11-22

## Disposition

**NEEDS MORE ROM EVIDENCE**

The authorized frames establish the visual lifecycle strongly enough to
identify a small correction direction, but the tracked text does not yet
identify the exact cause/type and exact update cadence for this specific
frame sequence. It would be premature to implement a screenshot-fitted
phase or replacement geometry. Preserve the accepted 150%-scale layered
effect as the reusable baseline and resolve the exact ROM death-picture
selection/timing before changing its production values.

This investigation made no source, test, configuration, documentation,
reference, gameplay, or Git-state changes. It created only this report and
its paired prompt under `AI-Logs/`.

## Starting repository state

- Branch: `master`
- HEAD: `aa6b711 Animate production Fuseballs`
- Tracking: `master...origin/master`
- Pre-existing tracked modifications:
  `AI-Logs/Reports/2026-11-20-atari-vector-hud-final-closeout-report.md`
  and `AI-Logs/Reports/2026-11-22-fuseball-visual-animation-closeout-report.md`
- Pre-existing untracked: `Reference/arcade/MBUCOD.V05.txt`
- Protected/untracked gameplay, `UserData`, `Temp`, and reference assets
  were not broadly enumerated or modified.

The task explicitly prohibited reading `MBUCOD.V05.txt`; it was not read.

## Evidence scope

All 45 numbered authorized PNG files exist. No missing frame numbers.
They were compared from `_5701` through `_5745`. Only representative frames
and exact duplicate-byte checks were needed. No contact sheet or temporary
image artifact was retained.

## ARCADE/FRAME OBSERVED

### Start and first breakup

- `_5701` is the intact/start condition: the Blaster/rim glyph is present
  near the lower-left rim and no large breakup is visible.
- `_5702`–`_5704` show the first visible angular breakup. The original
  centered location is retained while compact red, yellow, and white
  star-shaped fragments appear around it.
- `_5705`–`_5706` continue expansion. The fragments remain recognizable as
  separate vector arm/piece groups rather than a filled flash or a
  continuously deformed Blaster outline.

### Expansion and peak

- `_5708`–`_5710`, `_5711`–`_5712`, `_5715`–`_5716`, and `_5717`–`_5718`
  are exact consecutive raster duplicates. `_5707` is also held with
  `_5706`.
- `_5719` begins a visibly larger broad starburst.
- `_5720`–`_5724` show continued expansion and the strongest peak-like
  frame range. The most expanded representative is near `_5723`–`_5724`,
  where red, yellow, and white arms occupy separate angular sectors.
- The effect grows rapidly, but not at a visually uniform screenshot
  cadence. The held groups indicate discrete capture holds or stepped
  animation frames rather than evidence of smooth continuous movement.

Representative held/near-hold groups include:

| Frames | Observation |
|---|---|
| 5706-5707 | Compact/early expansion hold |
| 5708-5710 | Early arm hold |
| 5711-5712 | Small-step hold |
| 5715-5716 | Intermediate hold |
| 5717-5718 | Intermediate hold |
| 5724-5725 | Near peak hold |
| 5728-5729 | Contraction hold |
| 5730-5733 | Smaller-remnant hold with step changes |
| 5735-5738 | Shrinking remnant/hold groups |
| 5741-5744 | Final disappearance region |

### Contraction and final disappearance

- `_5727` onward is unmistakably contracting. `_5730`–`_5734` are a smaller
  red/white/yellow remnant centered near the original rim location.
- `_5739`–`_5740` show a compact final star or fragment cluster.
- `_5741`–`_5744` contain only progressively smaller final remnants.
- `_5745` has no visible breakup; the Blaster effect is gone. The nearby
  right-side enemy color pieces are not part of the Blaster explosion.
- The center is never displaced by tube perspective in the event itself.
  The fragment cluster remains around the original Blaster death location
  while its overall scale changes. Individual arms are visible in changing
  angular positions, but no evidence establishes independent procedural
  velocities or rotation.

### Visual phase model

There are approximately five distinguishable phases:

1. Intact/last visible Blaster: `_5701`
2. First breakup, compact angular fragments: `_5702`–`_5706`
3. Early-to-middle expansion: `_5707`–`_5718`
4. Broad peak and contraction: `_5719`–`_5727`
5. Shrinking/remnant/disappearance: `_5728`–`_5745`

Within the peak phase, yellow, white, and red remain the prominent
independent vector components. Color is not observed to pulse independently;
rather, the entire frame contains several fixed-color vector groups with
changing geometry and global scale. The final disappearance is primarily
shrinking/visibility loss, not a detached fragment escape. Exact timing is
**UNRESOLVED**: PNG numbering proves order and held captures, but it is not
established as one captured frame per Atari/ROM tick.

## ROM/DISASSEMBLY BACKED

### Shot-hit-player discrete size sequence

The tracked disassembly contains a direct shot-hit-player vector mechanism:

- Graphic-table entries at `cf0`–`cfa` point to `hit-by-shot explosion 1`
  through `6` and back through `9`.
- `3b04`–`3b82` implement six scaled shots-hit-player states:
  - size 6: `$3b04`
  - size 5: `$3b08`
  - size 4: `$3b0c`
  - size 3: `$3b10`
  - size 2: `$3b14`
  - size 1: `$3b18`
- Each larger-state entry applies a `vscale` and calls the shared vector
  fragment picture at `$1b1a`/`$3b1a`.
- `$3b1a`–`$3b82` is a fixed, multicolor vector fragment list using
  `vstat` colors 9, 10, and 11 with sparkle enabled and `z=14`.
- It is therefore **ROM/disassembly backed** as a stepped set of global
  sizes around a fixed vector fragment drawing, with a shared `vscale`
  rather than the frame-by-frame independent point objects the C# currently
  has.

This mechanism establishes that Atari has:

- discrete vector size stages;
- a fixed fragment/list geometry;
- a shared scale transform;
- per-fragment color assignment in the vector records;
- global disappearance through the size/selection lifecycle.

The disassembly does **not** establish independent POKEY/random velocities
for this sequence. No per-fragment velocity, persistent fragment object,
or separate fragment coordinate-update structure was identified in the
tracked text.

### Spiked/pulsared-apart player is a separate mechanism

The disassembly at `$3bae`–`$3c68` explicitly describes a
“spiked-apart or pulsared-apart player.” It calls `$0ffc` before piece
movements and states that the routine contains a changing `vscale` to move
the pieces. It contains reusable/player-derived-looking vector pieces
and repeated calls to `$1afe`.

This is **ROM/disassembly backed** for Spike/Pulsar-specific piece
separation, but it is not established to be the source of the authorized
sequence, which appears as a radially expanding multicolor starburst.
Cause/type, exact `$0ffc` update, and the exact routine that indexes this
sequence remain **UNRESOLVED** in the tracked text.

### General mechanism limits

The tracked listing does establish the player-death state area:

- `player_seg` and `player_along` represent location.
- The `player_seg` death-in-progress/high-bit comments describe $80 as
  death in progress and $81 for enemy-shot/spike death states.
- `graphic_table` routes vector pictures.

It does not expose a clearly named Blaster-effect countdown for this
specific frame sequence, and it does not give exact ROM frame cadence for
the `_5701`–`_5745` captures. A bounded Rev. 2 read is not justified yet:
the tracked text already establishes mechanism type, while the missing
essential value is the specific update selector/cause for the observed
sequence.

## CURRENT C# BEHAVIOR

### Accepted implementation

`PlayerDeathPresentation` is currently a 60-tick deterministic presentation
clock, with elapsed/duration/completion and enemy-retraction progress.
It does not own the Blaster geometry.

The renderer path is [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs):

- `Draw` invokes `DrawPlayerExplosion` while `DeathTicks > 0`.
- [DrawPlayerExplosion](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs:1613)
  derives the projected rim center from the player's edge endpoints.
- The effect is sampled from `TempestGame.DeathTicks`, not from
  `EnemyRetractionProgress`.
- It uses one shared `radius` envelope and three fixed 16-point starburst
  layers.

Layer data in [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs:96):

| Layer | Base scale | Fixed rotation |
|---|---:|---:|
| Yellow | 0.88 | -0.055 radians |
| Orange-red | 1.00 | 0.025 radians |
| White | 0.76 | 0.075 radians |

The scale envelope expands and contracts through 0.12, 0.28, 0.70, 1.00,
0.92, 0.62, 0.32, 0.14, and 0.00 at normalized ages 0 through 1.
The 52 logical-pixel base peak is multiplied 1.5, accepted as 78 logical
pixels during the 150%-scale milestone.

The geometry has:

- three concentric/starburst outlines;
- 16 radii and fixed asymmetric radius multipliers;
- fixed layer scales/rotations;
- no independent fragment velocity;
- no local shape animation;
- no explicit visibility state beyond zero radius;
- deterministic interpolation and disappearance at the countdown boundary.

Legacy and Atari presentation share this underlying
`DrawPlayerExplosion` renderer. It is a global, render-only effect and
does not require per-level data.

The 150%-scale milestone enlarged the existing three-layer geometry while
preserving the one-center, fixed-outline, deterministic envelope design.
It established manual acceptance, not ROM-backed exact constants.

### Why it appears less animated

The current C# effect has a smooth mathematically sampled global scale,
fixed outline geometry, and fixed layer offsets/rotations. The Atari
sequence appears in stepped, discrete visual pictures and has recognizable
separate arms and piece changes. It is less animated because C# has:

- one continuously interpolated shared scale instead of distinct vector
  picture stages;
- a fixed 16-point shape pattern;
- no explicit compact-to-breakup frame transition;
- no evidence-based arm/discrete-color picture schedule;
- no per-vector-list stage that is inserted, held, or removed.

The first meaningful divergence is not a gameplay or center mismatch. It is
the loss of discrete breakup/picture changes: C# begins with a radius 0.12
over three already-present outlines, while Atari `_5702`–`_5706` shows
distinct newly visible compact vector pieces. C# already samples the correct
growth/peak/contraction/zero progression more broadly, so geometry should
not be replaced wholesale.

## Historical evidence

The prior [150%-scale implementation report](/C:/Users/nrnob/source/repos/Tempest/AI-Logs/Reports/2026-11-02-blaster-death-visual-fidelity-implementation-report.md)
records:

- accepted layered yellow/orange/white starburst;
- three 16-point outlines;
- 52 base radius times 1.5;
- 60-tick death countdown;
- unchanged ordinary/life and Spike timing;
- manual acceptance as good enough at that milestone.

That manual acceptance is preserved. It is **PROJECT TRANSLATION**, not
ROM-backed geometry or timing. No type-0 enemy-destruction evidence should
be repurposed for player death; the ROM mechanisms are distinct.

The current C# 150% milestone remains useful and should not be rewritten.

## Smallest evidence-supported enhancement

If implementation is authorized later, the smallest correction should be:

1. Keep `DeathTicks`, `PlayerDying`, life/re-entry, collision, and
   reconstruction unchanged.
2. Keep the existing three-layer C# starburst drawing and 1.5 visual scale.
3. Replace the continuous symmetric radius envelope with an
   evidence-supported staged vector-picture schedule only if the ROM lookup
   sequence and cadence establish exact stages.
4. Until then, limit future tuning to:
   - earlier/clearer first breakup;
   - more clearly separated arm geometry;
   - explicit held peak/remnant stages;
   - final clean zero/disappearance.

Do not add random motion, independent fragment state, cause-specific player
deaths, or per-level animation data.

## Implementation readiness

**NEEDS MORE ROM EVIDENCE**

The visual frames establish the desired lifecycle and hold the compact,
peak, contraction, and disappearance structure. The tracked disassembly
establishes a shot-hit size sequence and a separate spiked/pulsared piece
mechanism, but it does not identify the exact observed cause and update
selector for the 45-frame event. The next safe step is a bounded
disassembly/Rev. 2 verification of the exact graphic-index/death-counter
path. Do not inspect `MBUCOD.V05.txt`.

## Recommended future implementation boundaries

- Deterministic phase/picture progression in the existing `DeathTicks`
  sample.
- Legacy and Atari presentation should submit the same underlying effect.
- No mutable persistent fragment objects.
- No gameplay-state coupling.
- No 60-tick duration change.
- No per-level constants.

## Suggested automated regression coverage

After ROM evidence is sufficient:

- deterministic phase progression at identical normalized ages;
- exact first/peak/contract/remnant/zero ordering;
- unchanged 60-tick ordinary death;
- unchanged life decrement and re-entry;
- unchanged final-life Game Over;
- unchanged Spike reverse-zoom projection/timing;
- unchanged enemy retraction and reconstruction;
- unchanged collision, score, shots, and Super Zapper state;
- unchanged recording/playback and reverse scrubbing determinism;
- shared geometry in legacy and Atari presentation;
- Levels 1–11 compatibility;
- clean disappearance at or before the countdown boundary.

## Validation status

**PASS**: bounded visual characterization and source/disassembly/code
investigation completed.

**PASS**: no protected assets outside the authorized frame range were
opened; no missing files were found.

**PASS**: no repository source or Git state was modified.

**UNRESOLVED**: exact Atari tick cadence, cause/type selector, and
`$0ffc`/death-counter update linkage.
