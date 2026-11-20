# Whole-Game Presentation / Level 9: Label Correction

**Date:** 2026-11-12
**Status:** COMPLETED — corrective, read-only investigation
**Completion token:** `TEMPEST_PRESENTATION_LEVEL9_LABEL_CORRECTION_20261112`

## Provenance and scope

The original user instruction accidentally reversed the panels. Neal has corrected the provenance authoritatively: **LEFT = current C#**, **RIGHT = MAME/reference**, and **RIGHT is the target**. This report supersedes the image-dependent conclusions of the [original report](2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md); that historical report remains unchanged.

The only visual asset analyzed was `TEMP/Comparision of screen demisions.png`. No other image, screenshot, recording, video, frame, contact sheet, or visual asset was searched for, enumerated, opened, generated, extracted, or analyzed. Source analysis was limited to the prior report's established findings; there was no new broad source investigation, ROM inspection, web search, game launch, or test run.

## Corrected observations

**ARCADE/MAME-OBSERVED / image measurements only:** The original panel measurements are still useful after correcting their attribution:

| Panel | Approximate black area | Approximate displayed aspect |
|---|---:|---:|
| LEFT — C# | 1278 × 959 image pixels | 4:3 landscape |
| RIGHT — MAME target | 773 × 1030 image pixels | 3:4 portrait |

These approximate bounds are measurements of the supplied comparison composition, not native resolutions or physical arcade-monitor specifications. The image does not establish whether either pane has been cropped or rescaled before composition. The corrected identification makes the left pane's aspect and HUD broadly consistent with the C# source; it does not prove that the image was captured from the exact current commit or identify each scene's phase.

**Approximate tube-only visual bounds:** On reinspection, excluding the right-hand Blaster and the overlaid C# recharge/paused text, the visible C# tube spans about **61% of its panel width and 46–49% of its height**; the MAME tube spans about **90% of panel width and 61% of panel height**. The C# bottom edge is partly obscured by text, so these are visual estimates, not precise pixel-derived measurements. The original report's broader 95% × 66% right-hand silhouette appears to include adjacent colored detail; it is not a clean tube-only measurement. Either way, the corrected direction is clear: **the reference tube occupies substantially more of its portrait presentation area**, not less.

- **Placement and margins:** The C# tube begins around 44% of its panel height and extends to around 91%; the MAME tube begins around 24% and extends to around 85%. Thus C# has much more black space above the tube and less below it; the MAME tube's bounding center is higher. These ratios describe this image only and the left bottom is partly obscured.
- **Horizontal center:** Both tubes are roughly centered in their respective panels. Small differences are uncertain because the selected colored lane and right-hand Blaster are not symmetric tube bounds.
- **Perspective:** The C# upper outer shoulders sit above the corresponding small far-rim shoulders, whereas the MAME upper outer shoulders sit below the far-rim shoulders. This visible difference suggests more than global translation and positive uniform scaling: those alone cannot reverse the relative vertical ordering of corresponding points. A depth-dependent presentation offset or a projection adjustment may be needed. **UNRESOLVED:** This one image does not establish matched gameplay/transition phases, exact correspondence, or which projection parameter is correct. Do not select exact values from it.
- **HUD:** C# shows yellow score and `LIVES` at upper left and `LEVEL 9` at upper right. MAME shows green score/initials near the upper center, blue `9` below that area, and yellow life icons toward upper left. The two panels differ in arrangement, content representation, and color. The earlier claimed C# HUD/source discrepancy disappears at the level of placement and color once the left panel is correctly identified; exact-version provenance is not independently established.

## Source findings that remain valid

**PROJECT CURRENT BEHAVIOR:** The prior investigation established an **800 × 600 logical canvas**, fixed **1280 × 960 window**, and uniform **1.6** output camera; the same camera applies to scene and HUD. The normal projection uses `K = 70`, `ZOffset = 5`, and the per-level projection center/offset; exit/reverse zoom animates projection state. The fixed camera does not provide automatic arbitrary-window aspect fitting. Existing display tests cover established presentation for Levels 1–4, not all nine.

**Correction independent of label reversal:** The prior report's claim that Level 9 has a current near-rim bounding height of about **85%** of the logical canvas was an arithmetic mistake. Its cited Level 9 points span approximately `-28.6` to `200` in Y, not ±200; with near scale `70/55`, their near-rim span is approximately `291` logical units, or **49% of 600**. The approximately 64% projected width remains a reasonable source-derived estimate. Neither is an arcade measurement.

**PROJECT TRANSLATION:** The current Level 9 model has 16 vertices and 15 playable lanes. Its model points also participate in gameplay geometry and cannot safely be changed merely to calibrate the image. Rendering dimensions and a render-only transform can be separated from those points, topology, collisions, player/enemy paths, shot/projectile behavior, spawns, rules, timing, and level configurations.

The previous recommendations to use Level 9 as a fixture rather than a special case, calibrate HUD independently, regression-check Levels 1–9, protect accepted transition behavior, and obtain Neal's manual visual acceptance remain valid. Incitti's 800 × 600 choice remains secondary engineering precedent, not arcade authority.

## Superseded conclusions and corrected recommendation

Withdraw the original report's assertions that the MAME area is 4:3, C# area is 3:4, C# tube is too large relative to the reference, current C# HUD broadly matches the MAME arrangement, or the right-hand 3:4 panel conflicts with the current C# implementation. Also withdraw the recommendation to preserve 4:3 specifically because the supplied MAME image was allegedly 4:3, and the suggestion that a post-projection scale/translation alone is likely sufficient.

**PROJECT RECOMMENDATION:** Reconsider the **800 × 600 4:3 presentation canvas**. The correctly identified target gives a concrete reason to try a **3:4 portrait presentation viewport**, without claiming that the ROM defines pixels or that the screenshot establishes physical monitor dimensions. Do not automatically delete or replace the 800 × 600 coordinate system: a portrait presentation coordinate space/viewport can be added around or separate from legacy projected coordinates, provided every playfield object and effect shares the intended mapping and HUD is placed separately. The eventual exact logical units and aspect remain provisional until Neal's manual acceptance.

Retain the manually accepted **1280 × 960 host window**. A first **PROJECT RECOMMENDATION** is to test a centered 3:4 viewport fitted uniformly inside it: **720 × 960**, with **280-pixel side margins**. For example, an internal **600 × 800** presentation coordinate space at 1.2 scale fits that viewport; these are convenient project translation values, not hardware facts. A different logical unit convention could yield the same viewport. This option reduces usable horizontal playfield area and requires moving/resizing and potentially reprojecting scene content; merely cropping the existing 4:3 drawing or stretching it would not be a valid calibration.

Use a global playfield anchor, scale, and translation; assess a global depth-dependent projection/vertical adjustment where required by the observed shoulder ordering. Apply it consistently to tube, player, enemies, shots, spikes, explosions, popups and transition presentation, with independent HUD layout and no Level-9-specific correction. Preserve model coordinates and simulation; do not casually change `K`/`ZOffset` because current exit/reverse animation depends on them.

Fullscreen/borderless remains a separate follow-on after internal calibration. It should uniformly fit and center whichever internal aspect is ultimately accepted, without arbitrary stretching. The fixed window remains available.

## Next-step decision

**Sufficient to design a bounded first experimental candidate**, not to assert an accepted or numerically exact match. The smallest safe candidate is an opt-in, shared portrait viewport/presentation calibration within the existing accepted window, with a coherent playfield transform and independently laid out HUD. Start from the 3:4 / 720 × 960 fitting hypothesis, test whether simple scaling/translation is enough, and allow a shared render-only depth/vertical adjustment only if needed. No gameplay geometry changes and no Level-9-only constants.

Focused automated checks should cover viewport fitting, margins, clipping/centering, aligned objects, unchanged gameplay model and projection restoration, plus Levels 1–9; Neal then manually checks Level 9 against the reference and the previously accepted layouts. This is a proposed future milestone, not an instruction to run it here.

## Remaining uncertainties

1. Whether the shown black bounds are the complete, uncropped, undistorted intended display regions, and whether both panes show comparable gameplay phases.
2. Exact logical aspect/units, optimal global projection parameters and HUD layout after manual calibration.
3. Whether a shared depth-dependent shift can preserve accepted transitions and all nine layout compositions without level-specific patches.
4. Physical arcade-monitor aspect and dimensions; the screenshot does not establish them.
5. Whether Neal prefers the portrait viewport pillarboxed inside the accepted window or, in a later explicitly authorized milestone, a different host/window mode.

## Git, changes, and validation

Starting and final branch: `master`; `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`. The index, tracked worktree, and non-ignored untracked state were clean at the initial and final checks.

Only this ignored corrective report and its paired ignored prompt were created. No production code, tests, durable documentation, display configuration, projection, gameplay geometry, HUD, Git state, or reference files were modified. No automated gameplay validation or test suite was required or run; no interactive game was launched. No commit or push was made.
