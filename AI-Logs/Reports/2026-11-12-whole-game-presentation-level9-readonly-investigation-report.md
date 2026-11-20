# Whole-Game Presentation / Level 9 Calibration Investigation

**Date:** 2026-11-12
**Status:** COMPLETED — READ-ONLY INVESTIGATION
**Completion token:** `TEMPEST_WHOLE_GAME_PRESENTATION_LEVEL9_READONLY_20261112`

## 1. Starting Git state

- Branch: `master`
- `HEAD`: `385f603a94391756b2ca6cccd524dc17b281e782` — “Close out ROM-backed enemy-shot rendering”
- `origin/master`: same commit
- Index and tracked worktree: clean
- Non-ignored untracked files: none
- No Git operation changed repository state; nothing was staged, committed, or pushed.

## 2. Scope and evidence boundaries

This investigation followed the supplied prompt and used repository source, tests, current durable notes, the local Incitti reference where relevant, and exactly the authorized comparison image. It did not inspect the ROM disassembly or ROM files, search the web, run the game, generate screenshots, or run tests.

The only visual asset analyzed was `TEMP/Comparision of screen demisions.png` (PNG header dimensions **2258 × 1230**). No other visual assets were searched for, enumerated, opened, extracted, or analyzed.

Evidence labels in this report:

- **ARCADE/MAME-OBSERVED:** appearance directly visible in that supplied comparison only; not a hardware measurement.
- **PROJECT CURRENT BEHAVIOR:** behavior established from the checked-out C# source/tests.
- **REFERENCE-DERIVED:** engineering choices in Incitti, not arcade authority.
- **PROJECT TRANSLATION:** existing mapping from vector/game coordinates into the modern renderer.
- **PROJECT RECOMMENDATION:** proposed future implementation direction.
- **UNRESOLVED:** the allowed evidence does not settle the point.

## 3. Current presentation architecture

### Host and logical canvas

**PROJECT CURRENT BEHAVIOR / PROJECT TRANSLATION:** [`DisplayConfiguration.cs`](../../src/Tempest.Game/Rendering/DisplayConfiguration.cs#L8) defines an **800 × 600 logical canvas**, a fixed `WindowScale` of **1.6**, and a **1280 × 960** window. Its `PresentationCamera` has zero target/offset/rotation and a uniform 1.6 zoom. [`Program.cs`](../../src/Tempest.Game/Program.cs#L240) initializes the window from the renderer's fixed dimensions. The inspected startup path has no resizable-window or fullscreen toggle setup. The configured dimensions therefore describe the fixed windowed mode, not a responsive viewport system.

[`VectorRenderer.Draw`](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L189) draws the scene under that camera. The tube, game objects, messages, and HUD are currently drawn in the same logical canvas/camera pass. The 1.6 output scale applies uniformly to all of them. A fixed camera does not, by itself, calculate a fit/letterbox transform for a changed host-window size.

### Projection and tube parameters

**PROJECT CURRENT BEHAVIOR:** [`Projection.Project`](../../src/Tempest.Game/Rendering/Projection.cs#L16) maps each model point to:

`xScreen = 400 + x * K / (z + ZOffset)`
`yScreen = CenterY + (y + YOffset) * K / (z + ZOffset)`

Normal production startup sets `K = 70` and `ZOffset = 5` ([`Program.cs`](../../src/Tempest.Game/Program.cs#L178)). The shared perspective is then rendered by `DrawTube` and the object renderers. `K` controls the overall projected magnitude; the near/far scale ratio is governed by depth and `ZOffset` (for the normal tube depths, `455/55 ≈ 8.27`). The exit/reverse-zoom presentation animates `K` and `ZOffset` in `TempestGame`; any future baseline change must preserve that established visual transition and exact restoration.

The screen-space level parameters `CenterY` and `YOffset` vary by layout in [`LevelLoader.cs`](../../src/Tempest.Game/Levels/LevelLoader.cs#L40). Level 9 uses `CenterY = 300`, `YOffset = 0`. Its ROM row and points are translated into centered/scaled C# coordinates, with Y reflected for the screen-down convention ([`LevelLoader.cs`](../../src/Tempest.Game/Levels/LevelLoader.cs#L132)). These are project presentation translations, not literal physical display dimensions.

For Level 9, the current 16-point open polyline spans approximately ±200 in both model axes. At the near rim (`z = Level.Position = 50`) the current projection scale is `70/55 ≈ 1.273`; at the far rim (`z = 450`) it is `70/455 ≈ 0.154`. Thus the current near-rim projected bounding box is about **64% of logical width and 85% of logical height**, before the fixed 1.6 host scale. These are consequences of current C# geometry and projection, not MAME pixel specifications.

## 4. Supplied comparison: MAME left versus C# right

**ARCADE/MAME-OBSERVED — supplied image only.** Approximate measurements from the two black game areas in the supplied composition:

| Supplied panel | Approx. black-area dimensions | Approx. ratio | Tube silhouette relative to that area |
|---|---:|---:|---|
| Left, MAME/reference | 1278 × 959 px | 1.33:1 (4:3) | about 61% wide × 49% tall |
| Right, labeled C# | 773 × 1030 px | 0.75:1 (3:4) | about 95% wide × 66% tall |

Measurements are approximate screenshot-pixel observations, not cabinet/display specifications. Silhouette bounds include the visible rails/rims and may include nearby colored details.

- The left/MAME tube is visually broad across its upper shoulders, narrows toward its lower stem, and occupies the lower part of its black area. Its horizontal center is close to the panel center; its visible tube bounds sit roughly 19% of panel height below the panel center.
- The right/C# tube is much wider relative to its shown black area, reaches close to both side margins, and appears taller than the MAME tube relative to its area. It is nearly vertically centered and its apparent center is slightly right of the area center (roughly 2% of panel width).
- There is substantial empty black space above the MAME tube. The C# panel is itself much taller/narrower, so its margins and tube-to-screen relationship cannot be directly read as a same-viewport comparison.
- MAME-side HUD is visibly arranged as score and lives at upper left, with level at upper right. The right-side panel instead shows a green score/initials around the upper middle, a blue level below it, and yellow icons toward upper left; this is a visibly different HUD placement/color treatment.

### Important screenshot/source discrepancy

**UNRESOLVED:** The right-side black area in this image is approximately **3:4**, but the checked-out C# build is configured as a fixed **4:3, 1280 × 960** window with no resizing/fullscreen path found in the inspected startup. Also, the right-side image's centered green score/blue level do not match the current C# HUD source, which draws yellow score/lives at logical top-left and yellow level at logical top-right. The image's right pane therefore cannot be assumed to be a direct, unmodified capture of this exact baseline's full client area. Cropping, composition, capture version, or another presentation layer may explain it; the supplied evidence does not identify which.

This discrepancy is material: the screenshot supports a qualitative direction (the shown C# tube is too large relative to its pane and its visible placement differs), but it does **not** justify changing the accepted logical aspect to 3:4 or selecting exact projection/HUD coordinates. A current-build capture and confirmation of the compared pane bounds are needed before tuning numeric values. No alternate visual asset was sought.

## 5. Level 9 as the primary calibration case

**PROJECT CURRENT BEHAVIOR:** Level 9 is a 16-vertex, open Level 9 shape with 15 playable lanes; its production configuration uses the Level 9 definition and does not change model points for rendering. The shape's broad horizontal shoulders, narrow inward/central sections, and open ends make global horizontal scale, aspect, centering, and apparent vertical placement easy to see. Long rails from distinct vertices to near/far rims make depth convergence/compression visible. Its wide model bounds also make clipping and safe margins easier to detect than a compact layout.

**PROJECT RECOMMENDATION:** Level 9 is a strong *measurement fixture*, not a reason to add Level-9-only rendering constants. Apply shared presentation parameters and evaluate Levels 1–9 before accepting any change.

## 6. Gameplay-geometry boundary

**PROJECT CURRENT BEHAVIOR:** The `LevelDefinition.Points` are not disposable screen coordinates: `Level` uses them to establish polygon winding, lane adjacency, edge transitions, and gameplay geometry. Player/enemy paths and collisions depend on this model. Do not edit points, lane topology, playable-lane counts, or per-level configurations to make the picture fit.

By contrast, `CenterY`/`YOffset` are consumed by the renderer's projection calls; `Projection.K`/`ZOffset` are consumed by rendering and presentation transition code. A render-only transformation applied **after projecting** can reposition/rescale all displayed playfield geometry without changing model coordinates, lane transitions, collisions, paths, shots, spawns, scoring, or timing. It must include the tube and all aligned playfield objects/effects; transforming just the tube would misalign gameplay visuals.

**PROJECT RECOMMENDATION:** Keep gameplay geometry frozen. Separate a shared playfield presentation transform from the logical model and from HUD placement. Avoid baking global calibration into `LevelDefinition.Points` or introducing per-level visual patches. Treat `K`/`ZOffset` calibration as a separate, higher-risk projection choice because they participate in existing exit/reverse-zoom visuals.

## 7. HUD and other offsets

**PROJECT CURRENT BEHAVIOR:** The current HUD draws score at `(10,10)`, lives at `(10,44)`, and level at `(640,10)` on the 800 × 600 logical canvas ([`VectorRenderer.cs`](../../src/Tempest.Game/Rendering/VectorRenderer.cs#L1026)). Current code therefore already broadly shares the MAME-side top-left score/lives and top-right level arrangement. The supplied right-side C# presentation does not match these coordinates or colors.

**PROJECT RECOMMENDATION:** Calibrate HUD independently in logical-canvas coordinates, with stable edge margins and a clear relationship to the tube. Do not let a playfield-only scale/translation accidentally move or enlarge text. First resolve the capture/source discrepancy; only then use the MAME-side screenshot arrangement as the target. Avoid asserting exact hardware HUD pixel positions from the comparison.

## 8. Previously accepted display milestone and historical constraints

Commit `7ece019` (“Implement accepted larger game window”, 2026-11-02) added the 800 × 600 logical canvas, uniform 1.6 camera scale, 1280 × 960 window sizing, and focused coordinate/presentation tests. The checked-out [`DisplayConfigurationTests.cs`](../../tests/Tempest.Game.Tests/DisplayConfigurationTests.cs#L13) pins those dimensions and scale and verifies existing tube coordinates are preserved for Levels 1–4. The present task explicitly confirms that **1280 × 960 was manually accepted as much better for Neal's 1080p setup**; that accepted host-window choice remains protected.

There is a documentation mismatch: current [`TEMPEST-DEVELOPMENT-NOTES.md`](../../TEMPEST-DEVELOPMENT-NOTES.md#L95) still calls manual acceptance pending. This read-only task did not update durable documentation; the task's explicit acceptance statement is recorded here, and the notes should be reconciled only in an authorized acceptance closeout.

The same notes say not to casually tune projection, preserve K=70/ZOffset=5, and avoid switching to portrait based only on the historical vertical-monitor fact. The deferred Level 9 handoff explicitly calls for whole-game screen/tube/HUD refinement, regression on earlier tubes, constant internal gameplay dimensions, and fullscreen/borderless consideration ([`TEMPEST-DEVELOPMENT-NOTES.md`](../../TEMPEST-DEVELOPMENT-NOTES.md#L3957)).

**REFERENCE-DERIVED:** The checked-in Incitti implementation uses an 800 × 600 logical canvas, opens windowed, and has an F12 window/fullscreen toggle path. This is engineering precedent only, not evidence for the original cabinet's pixel resolution or aspect ratio. The local development notes also describe Incitti's fullscreen mode as requesting an available 800 × 600 display mode.

## 9. Recommendations

### Internal dimensions and aspect

**PROJECT RECOMMENDATION:** Preserve the accepted **800 × 600 logical 4:3 canvas** and **1280 × 960 windowed client dimensions** as the initial implementation baseline. The MAME-side black area in the supplied image is itself approximately 4:3; the right pane's apparent 3:4 shape conflicts with source and is not sufficient evidence to rotate or redefine the logical canvas. Do not claim that 4:3, 800 × 600, or 1280 × 960 is ROM-defined or a physical monitor specification.

If future host sizes vary, fit the logical 4:3 canvas uniformly and center it, using unused letterbox/pillarbox space rather than stretching. The current fixed camera is not yet a dynamic aspect-fit implementation.

### Global projection / placement calibration

**PROJECT RECOMMENDATION:** Once a current-baseline capture is confirmed, calibrate in this order:

1. Compare tube and playfield bounds as fractions of the confirmed 4:3 client area.
2. Tune one global **uniform playfield scale about a defined logical anchor** and one global translation (especially vertical placement). Apply the same transform to tube, player, enemies, shots, and related effects.
3. Tune the global perspective/depth ratio only if a verified comparison still requires it. Distinguish overall scale (`K` or a post-projection scale) from depth compression (`ZOffset`/the projection's depth relationship); preserve exit/reverse-zoom behavior and restoration.
4. Calibrate HUD separately.

Do not adjust individual tube points or add a Level 9-only correction. Do not infer the physical display aspect from screenshot pixels.

### Regression across layouts

The existing display-specific test covers Levels 1–4, while Level 9 is not currently included in that presentation test. An eventual change should add projected bounds/centers and aspect-preserving transform checks for **all production layouts, Levels 1–9**; verify the model point arrays/topology remain unchanged and that transform application is shared across playfield objects. Existing gameplay/collision/path tests remain the authority for preserved model behavior. Automated geometric invariants cannot establish arcade visual acceptance: after focused tests, Neal should manually compare Level 9 and check previously accepted Levels 1–8.

### Fullscreen / borderless fullscreen

**PROJECT CURRENT BEHAVIOR:** The inspected app initializes the fixed window and has no current fullscreen toggle or dynamic resize handling.

**PROJECT RECOMMENDATION:** Make fullscreen/borderless fullscreen a **separate follow-on milestone after internal presentation calibration**. It introduces host-display dimensions, DPI/window behavior, and aspect-fit/unused-margin decisions that should not obscure whether the logical scene is calibrated. When implemented, preserve the calibrated logical aspect with uniform scaling and centered letterbox/pillarbox margins; never stretch to an arbitrary display aspect. Do not discard the accepted 1280 × 960 window.

## 10. Unresolved questions

1. Is the right-hand comparison pane a direct capture of the exact current baseline, or is it cropped/rescaled/from another C# build? Why is its apparent black area 3:4 while current code is fixed at 4:3?
2. What are the true client-area bounds in each side of the comparison, distinct from surrounding application/composition margins?
3. What numeric scale/vertical offset does Neal want after a current-build, same-aspect comparison?
4. Does the supplied right-hand HUD represent the desired C# HUD, or is it from an earlier/alternate display implementation?
5. The physical/reference display aspect and dimensions are not established by this source or one screenshot. They remain **UNRESOLVED**; no ROM-defined desktop resolution is claimed.
6. Should variable-sized window support be included in the eventual internal-presentation milestone, or remain separate from fullscreen? No such requirement is established by the current fixed-window architecture.

## 11. Bounded next implementation milestone

**PROJECT RECOMMENDATION — not authorized/implemented here:** After confirming a same-baseline Level 9 capture and comparable client bounds, implement one whole-game presentation calibration: retain the 800 × 600 logical model and accepted 1280 × 960 host; add or tune a shared, uniform post-projection playfield scale/translation; keep HUD anchoring independently calibrated; leave all Level 1–9 model geometry and gameplay behavior unchanged. Add focused projected-bounds/transform regression for Levels 1–9, run those tests, then provide Neal the Level 9 and earlier-level manual checks. Leave fullscreen/borderless to a later milestone.

## 12. Changes and validation

- No production code, tests, durable documentation, configuration, Git state, or reference files were modified.
- The paired prompt was copied into ignored `AI-Logs/Prompts/`; this report was created under ignored `AI-Logs/Reports/`.
- No interactive game, automated gameplay validation, test suite, or screenshot generation was run; none was required for this read-only investigation.
- Final validation is limited to the read-only source/image inspection and final Git-state check.
