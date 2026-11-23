# Fuseball Visual-Animation Read-Only Investigation

Date: 2026-11-22
Completion token: `TEMPEST_FUSEBALL_VISUAL_ANIMATION_READONLY`

## 1. Scope and evidence

- Visual evidence: exactly the authorized 54-frame range `UserData/Video/Level-11-Fuseball/tempest-lvl9-12_5691.png` through `_5744.png`; all 54 files exist.
- ROM evidence: tracked textual disassembly `Reference/arcade/Tempest Rom Source code.md` only.
- Project evidence: existing Fuseball implementation/tests/reports and the current C# renderer.
- Protected user/Temp content was not inspected.
- The raw Rev. 2 ROM directory and `Reference/arcade/MBUCOD.V05.txt` were not inspected.
- No production source, tests, configuration, durable documentation, reference assets, gameplay behavior, or Git state was changed.

## 2. Authorized-frame visual observations

The sequence is 480x640. Consecutive-frame differences include Fuseball movement, growth from perspective, and the Fuseball's local shape changes. Some frames are exact pixel-identical:

- `_5697` matches `_5696`
- `_5699` matches `_5698`
- `_5701` matches `_5700`
- `_5703` matches `_5702`
- `_5705` matches `_5704`
- `_5707` matches `_5706`
- `_5709`, `_5710` match `_5708`
- `_5712` matches `_5711`
- `_5714` matches `_5713`
- `_5716` matches `_5715`
- `_5718` matches `_5717`
- `_5720` matches `_5719`
- `_5725` matches `_5724`
- `_5727` matches `_5726`
- `_5729` matches `_5728`
- `_5731` matches `_5730`
- `_5733` matches `_5732`
- `_5735`, `_5736` match `_5734`
- `_5738` matches `_5737`
- `_5740` matches `_5739`
- `_5742` matches `_5741`
- `_5744` matches `_5743`

Representative Fuseball frames show:

- A small, visible five-color star-like vector figure that moves through the tube.
- Approximately 25 submitted vector segments in the C# model: five arms with five segments each.
- A stable entity center with changing arm/endpoint relationships.
- Growth as the Fuseball approaches the rim is perspective/scale, not local shape animation.
- Some arms change endpoint shape independently while the overall figure remains centered.
- Exact holds occur between some phase changes, but the sequence cannot establish a one-frame Atari timing cadence because the capture file names do not establish arcade frame cadence.
- The local phase sequence appears visually cyclic but the exact four-phase order cannot be proved from raster screenshots alone.

## 3. ROM/disassembly mechanism

The tracked disassembly defines four separate Fuseball/Fuzzball picture records:

- Picture 1: `3c6a`, address/table target `1c6a`
- Picture 2: `3cb2`, address/table target `1cb2`
- Picture 3: `3cfc`, address/table target `1cfc`
- Picture 4: `3d44`, address/table target `1d44`

The dispatch table at `cf16-cf1c` lists the four target subroutines in picture 1 through picture 4 order. Each picture uses five `vstat` color groups and `vsdraw`/`vldraw` vector records, with sparkle-enabled state changes. The visible outer/inner arm endpoints differ between pictures; this is a four-picture vector sequence, not a single fixed shape translated through the tube.

Classified facts:

- **ROM/DISASSEMBLY BACKED:** four discrete Fuseball vector-picture records, five color groups, changing picture geometry, and dispatch table entries.
- **UNRESOLVED:** exact state byte/counter and the arcade simulation cadence that selects the four pictures. The textual evidence inspected so far does not establish a one-to-one frame or 4-tick mapping.
- **UNRESOLVED:** whether the five arm colors remain fixed across every picture or whether state/color selection changes independently; the pictures use the same disassembly pattern, with five color groups.

Relevant state declarations include `n_fuzzballs`, `spd_fuzzball_lsb/msb`, and `fuzz_move_flg/fuzz_move_prb`, but these describe composition/movement context rather than a proven animation-clock mechanism.

## 4. Current C# mechanism

The current renderer already contains:

- A five-color palette in `src/Tempest.Game/Rendering/VectorRenderer.cs:86-90`.
- Four 20-arm-row records and 25 line submissions in `VectorRenderer.cs:120-159` and `VectorRenderer.cs:854-876`.
- Shared `game.FuseballAnimationFrame` selection in `VectorRenderer.cs:865-873`.
- A four-frame shared game clock exposed as `FuseballAnimationFrame => fuseballAnimationTicks / 4` in `src/Tempest.Game/TempestGame.cs:147-148`.
- A modulo-16 counter increment in `TempestGame.cs:475-476`.

The frame counter advances only inside `if (FuseballTest)`. Therefore normal production Fuseballs do not advance it: they receive frame 0 repeatedly, producing the static visible result. In isolated Fuseball-test/hurry-up scenarios, existing tests confirm four shared frames, five arms/colors, and 25 line submissions.

These C# points are mostly a PROJECT/REFERENCE-derived geometry model: the existing report says the arm data came from Incitti's `ReadFuseballData`, and that report explicitly identified arcade appearance/cadence as not independently established. The data is visually compatible with a five-arm four-phase figure, but exact ROM coordinate equivalence and cadence remain unproven.

## 5. First semantic/visual divergence

The earliest relevant divergence is the `FuseballTest`-only counter gate in `TempestGame.cs:475-476`, not projection or lane geometry.

The C# entity already:

1. has four geometry frames,
2. submits distinct vector endpoints by frame,
3. preserves five colors,
4. and scales/projected geometry with depth.

Normal production selects the same frame 0 for every render. This is the direct reason the visible entity is static while arcade reference/ROM evidence indicate four alternating vector pictures.

## 6. Smallest evidence-supported implementation

**READY FOR BOUNDED VISUAL CORRECTION**

The smallest candidate correction is rendering-only:

- Advance the existing shared visual phase counter during normal active production play, not only during `FuseballTest`.
- Keep the existing modulo-16 and divide-by-4 phase mapping as a **PROJECT TRANSLATION**, not a ROM-backed cadence claim.
- Keep all phase state outside `Fuseball`, collision, height, lane, lifecycle, and movement logic.
- Preserve the existing 25 submitted segments and five fixed arm colors unless separate ROM-backed evidence requires geometry changes.
- Do not change presentation/projection or Level 11 behavior.

The exact geometry table should remain unchanged for this bounded correction: it already provides four phases and does not require guessing ROM coordinates. A later ROM-backed closeout may replace the table/cadence only after the exact state/counter mechanism is established.

## 7. Suggested regression tests

No tests were implemented in this read-only task. Focused additions should prove:

- In normal production play, a Fuseball advances through exactly four shared animation phases and wraps deterministically.
- In isolated and hurry-up modes, the existing four-frame/25-segment behavior is preserved.
- Pause/death/freeze behavior has an explicitly chosen rule and does not mutate Fuseball movement or collision state.
- Restart/replay resets the visual phase consistently.
- The animation does not alter `Fuseball.Height`, `EdgeIndex`, `W`, direction, state, collision bands, score, or lifecycle.
- Level 11 completion and wave accounting remain unchanged.
- Legacy and Atari presentation registration both submit the same four selected geometry phases and only vary projection/scale.
- The test must be deterministic with fixed random seed and fixed simulation-step count.

## 8. Remaining uncertainties and manual acceptance

The exact arcade phase cadence and four-picture phase order are not established by the inspected evidence. The 54 raster frames show holds and repeated local shapes but cannot prove one-to-one Atari simulation ticks. Final acceptance requires Neal to compare the corrected C# playback under the authorized visual criteria.

TEMPEST_FUSEBALL_VISUAL_ANIMATION_READONLY
