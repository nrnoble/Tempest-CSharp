# Absolute-path matched-image proportion investigation

**Status: COMPLETE — visual measurements recorded; Level 7 full-tube diagnosis remains partly unresolved.**

## Direct answers

1. **Did all four authorized images open?** Yes.
2. **Does Level 7 remain broader after translation and uniform-size normalization?** Yes. Its outer-envelope W:H is `0.999`, versus `0.765` in the reference. Its W:H ratio is about **31% broader** than the reference ratio.
3. **Is the mismatch proportion, perspective, geometry, or capture state?** A clear outer-envelope proportion mismatch is accompanied by a low-confidence but substantial near/far alignment discrepancy. Both captures look like steady play, not a tube transition. The complete-tube difference is therefore a **combination of proportion and unresolved perspective/local-geometry differences**, not evidence of a capture-state transition.
4. **Does Level 1 objectively compare more closely?** Slightly on the measured outer contour: similarity RMS is `4.66%` of reference height for Level 1 and `5.43%` for Level 7. Both have nearly the same large outer W:H mismatch, so the result only weakly supports the supplied observation that Level 1 looks closer.
5. **Do Levels 2 and 5 validate the method?** Yes. Their current and reference outer W:H ratios agree closely, and their similarity residuals are low. They provide useful sanity checks for detecting the known vertical emphasis.
6. **What Level 7 Y gain best fits?** `1.210`, **visual diagnostic only**, fitted to the outer envelope about its projected envelope center.
7. **How much does that help?** Outer-envelope RMS falls from `40.62 px` (`5.43%` of reference height) to `21.64 px` (`2.89%`), a `46.7%` reduction.
8. **Does a general affine fit materially outperform it?** Not on the measured outer contour: affine RMS is `20.59 px`, only `4.9%` below the one-Y model. This does not resolve the interior rail/core discrepancy.
9. **Should the viewport remain frozen?** Yes. The references share a near-identical outer ratio (`0.762–0.765`), while Levels 2 and 5 already match it. The images do not show one common C# transform error across all four levels.
10. **Should gameplay geometry remain frozen?** Yes. No source geometry or gameplay change is justified.
11. **Is a Level-7-only presentation experiment justified now?** No. A Y gain improves the outer silhouette, but the apparent far/core displacement and rail convergence are not explained by that fit. Resolve the matching perspective/landmarks before proposing an experiment.

## Scope and evidence boundaries

This is a read-only, image-grounded follow-up to the source/math analysis in `AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md`. Only the four exact authorized image paths below were opened. No other visual files, workspace image searches, MAME internals, game processes, source files, tests, configuration, or durable project documentation were inspected or changed.

### Authorized images opened

- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-1 SideBySide.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-2 sidebyside.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\level-5 Sidebyside.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-7 SidebySide.png`

All four opened successfully as 1920×1280 PNGs. The divider is at x=960; each left half is C# and each right half is the visual reference.

### Extraction and fitting method

- Used a bounded Python standard-library PNG decoder; Pillow and OpenCV were unavailable, so no packages were installed. The decoder read only the four authorized paths.
- Masked the C# cyan tube vectors and reference blue tube vectors separately. The masks excluded red, green, yellow, HUD, and non-blue vectors; the reference mask also excluded the top HUD band. The image halves were never combined.
- Derived the outer near-envelope from the convex hull of each tube mask. Similarity fits use 32 equally spaced samples along that hull, aligned by cyclic/reversed perimeter correspondence. These are **resampled contour points**, not 32 independent hand-selected arcade landmarks.
- Fit translation, rotation, and one uniform scale first. Then fit a one-Y-gain model, allowing translation, rotation, and uniform comparison scale as well. A least-squares general affine fit is the upper-bound comparison. All fit residuals below are Euclidean pixel residuals normalized by the reference outer-envelope height.
- Far/deep rim bounds and centers were estimated from the visible blue geometry and scanline inspection. They are lower confidence than outer-hull measurements: Level 5's center is especially busy, and Level 7's center contains nearby gameplay vectors. They are not included in the outer-hull fit scores.

## Outer-envelope fit table

The path column gives the exact image used for each row. “Similarity” and “Y gain” residuals show RMS / maximum in pixels, followed by RMS as a percentage of reference near-envelope height. Affine RMS is likewise in pixels and percent.

| Level / exact image path | Matched contour samples | Capture-state confidence | Raw C# near W×H; W:H | Raw reference near W×H; W:H | Similarity scale | Similarity RMS / max | Diagnostic Y gain | Y-gain RMS / max | Affine RMS | Near/far classification and diagnosis | Confidence |
|---|---:|---|---|---|---:|---|---:|---|---|---|---|
| 1 — `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-1 SideBySide.png` | 32 | High | 855×855; 1.000 | 603×791; 0.762 | 0.807 | 36.87 / 50.12 px; 4.66% H | 1.230 | 11.16 / 14.57 px; 1.41% H | Outer proportion differs; far center and rail convergence are broadly consistent after normalization. | Medium-high |
| 2 — `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-2 sidebyside.png` | 32 | High | 689×904; 0.762 | 549×719; 0.764 | 0.795 | 1.13 / 1.96 px; 0.16% H | 0.998 | 1.09 / 1.89 px; 0.15% H | Outer proportion closely matches; far opening is proportionally larger in the reference, so perspective is not an exact match. | High for outer contour |
| 5 — `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\level-5 Sidebyside.png` | 32 | Medium | 601×791; 0.760 | 602×790; 0.762 | 1.026 | 10.11 / 17.03 px; 1.28% H | 0.983 | 9.72 / 17.59 px; 1.23% H | Outer proportion closely matches; central/deep geometry is too busy for a reliable matched far-rim fit. | Medium |
| 7 — `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-7 SidebySide.png` | 32 | Medium-high | 765×766; 0.999 | 572×748; 0.765 | 0.863 | 40.62 / 51.03 px; 5.43% H | **1.210** | **21.64 / 41.19 px; 2.89% H** | Outer proportion differs; apparent far-center displacement reverses sign, with distinct interior rail convergence. | Medium |

Affine RMS / maximum residuals are: Level 1 `11.13 / 14.55 px` (`1.41% H`), Level 2 `0.98 / 1.90 px` (`0.14% H`), Level 5 `8.59 / 13.93 px` (`1.09% H`), and Level 7 `20.59 / 35.41 px` (`2.75% H`). Thus, for the Level 7 outer contour, affine improves the one-Y RMS by only about `4.9%`; it does not materially outperform it.

## Near/far, rails, and capture state

Near measurements above are raster-derived outer-envelope bounds. The following far/deep measurements are approximate visual bounds in image pixels, not independently accepted project geometry. Coordinates are within each side's own half. Far/near ratios use the approximate far width or height divided by the raster-derived near width or height.

| Level | Approx. far/deep bounds, C# vs reference | Approx. far/near width ratios, C# vs reference | Approx. far/near height ratios, C# vs reference | Far-center displacement from near-envelope center, C# vs reference | Rail/convergence assessment |
|---|---|---|---|---|---|
| 1 | About 100×103 vs 86×117 px | 0.117 vs 0.143 | 0.120 vs 0.148 | About (+1,+151) vs (+2,+144) px | Both show radial rails converging below the near-envelope center by similar normalized distances. This supports a mostly proportion-driven outer mismatch, although the near/far ratios are not identical. |
| 2 | About 83×110 vs 90×118 px | 0.120 vs 0.164 | 0.122 vs 0.164 | About (0,+151) vs (+1,+130) px | The rectangular rails converge in the same general direction, but the reference far opening is larger relative to its near rim. |
| 5 | N/A — central cross/rim intersections and effects do not support a reliable far-envelope box | N/A | N/A | Approximately (+2,+154) vs (-3,+154) px; low confidence | General convergence direction is similar; no reliable local angle or far-size comparison. |
| 7 | About 118×122 vs 92×115 px | 0.154 vs 0.161 | 0.159 vs 0.154 | Approximately (0,-50) vs (-12,+74) px; low confidence | The inner/core center appears above the C# near-envelope center but below the reference near-envelope center. A positive Y gain about the envelope center cannot reverse this displacement. The rail-angle change is not robustly quantifiable from a single static pair. |

All four pairs show a steady tube, not an obvious Approach/ExitZoom transition. Differences in score, Blaster position, and enemies were ignored. Level 7 reference gameplay vectors obscure some center lines, so its state confidence is lower than Levels 1 and 2; there is no visible evidence that it is a different tube-transition state.

For Level 7, a representative upper-notch-to-core vertical run appears roughly `140 px` in C# versus `240 px` in the reference before normalization; relative to near-envelope height this is about `0.18` versus `0.32`. This is a low-confidence visual measurement, not a rail-angle fit. It indicates that the `1.210` outer-envelope gain does not establish a matching near/far projection. No reliable numerical rail-angle improvement is claimed.

## Level-specific findings

### Level 1

After uniform size and translation are removed, the outer shape remains substantially broader in X than the reference (`1.000` versus `0.762` W:H). Its similarity residual is slightly lower than Level 7's, supporting “Level 1 compares closer” only modestly. The same outer ratio mismatch remains, so the Level 1 result does not show that it is proportionally matched. The approximate far-center displacement is similar on both sides.

### Level 2

The outer W:H ratios (`0.762` and `0.764`) and similarity residual (`0.16%` of reference height) agree very closely. The image measurement detects the direction and approximate magnitude of vertical emphasis already present in the C# presentation: the final rendered shape is vertically emphasized relative to an isotropic 1:1 shape. The measured residual Y gain is approximately `1.0` because the screenshots already contain that presentation adjustment; it is not expected to recover the source factor `1.314`. Far/near size differs and remains a separate perspective caveat.

### Level 5

The outer ratios (`0.760` and `0.762`) and similarity residual (`1.28%` of reference height) are close. This is consistent with the known final presentation's `.670 X / .880 Y` relative gains producing a vertically emphasized comparison, but the image fit is not a recovery of those constants. Effects and complex crossing geometry reduce confidence in the deep-rim assessment.

### Level 7

The near outer W:H mismatch is clear and similar in magnitude to Level 1. The outer-envelope Y-only gain of `1.210` substantially reduces the hull residual; a general affine fit improves it only marginally. However, the low-confidence inner/core estimate suggests opposite vertical displacement around the envelope-center anchor, and the upper-notch-to-core run is visibly different. Therefore the measured outer gain cannot be treated as a full-tube correction or accepted presentation value.

## Source/image reconciliation

- **IMPLEMENTATION-ESTABLISHED:** The controlling source/math report establishes isotropic Level 1 and Level 7 source bounds and current adapter gains; Level 2 applies a Y presentation factor of `1.314`; Level 5 uses `.670 X / .880 Y`; the preserved Level 7 record 06 is approximately isotropic; and the shared camera/viewport scaling is uniform. These facts are not re-derived here, and record 06 is distinct from the production geometry under visual comparison.
- **VISUAL DIAGNOSTIC:** The four images show reference outer W:H in the narrow range `0.762–0.765`. C# Level 2 and 5 match that proportion; C# Level 1 and 7 remain near `1.0`. The Level 7 diagnostic gain and residuals are fits to these images only.
- **MANUAL OBSERVATION:** The supplied judgment that Level 7 looks broader is supported by the outer envelope. The judgment that Level 1 compares closer is only weakly supported by its slightly lower outer-contour similarity residual; Level 1 also has a large outer ratio mismatch.
- **PROJECT DECISION / PROJECT TRANSLATION:** The host/viewport contract remains frozen at 1600×1280, with a centered 960×1280 viewport at (320,0), 3:4 aspect, and uniform mapping. Accepted geometry and per-level presentation remain unchanged.
- **UNRESOLVED:** The images alone do not establish whether Level 7's apparent far-center/rail difference is an intentional geometry distinction, a projection/perspective difference, or a more specific capture mismatch. The low-confidence center landmarks need a better matched comparison before an implementation candidate is selected.

Image-derived ratios and fits are **VISUAL DIAGNOSTIC / INFERENCE**, not `ARCADE-OBSERVED`, `ROM-BACKED`, or Atari presentation constants. No value is being proposed as a source change.

## Decision and next step

Keep the global viewport, Level 7 gameplay geometry, shared presentation architecture, placement, and all gameplay systems frozen. The common reference outer ratio does not imply a common C# global transform error because Levels 2 and 5 already match that ratio while Levels 1 and 7 do not.

Do **not** run the optional Level-7-only Y-gain experiment yet. First obtain or identify stable, corresponding Level 7 near/far rail and core landmarks in a compatible ordinary-play capture. If those establish a proportion-only residual, a future **DEVELOPMENT-ONLY** test could apply a presentation-space Y gain about the projected envelope center consistently to tube and entity projection, without changing geometry, registration, shared projection, viewport, or gameplay. That experiment is not authorized by or implemented in this investigation.

## Validation and repository preservation

**Image decode and outer-contour fitting: PASS.** All four PNGs decoded locally and the bounded fits completed. Python image packages were not installed. No project build or tests were run because no executable project files changed. No game was launched and no manual gameplay acceptance was performed.

The expected baseline matched exactly. Repository start and finish:

| Field | Start | Finish |
|---|---|---|
| Branch | `master` | `master` |
| HEAD | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` | unchanged |
| Upstream | `origin/master` | `origin/master` |
| Ahead / behind | `0 / 0` | `0 / 0` |
| Index | Empty | Empty |
| Tracked worktree | Pre-existing modification to `.github/copilot-instructions.md` | Same; not modified by this task |
| Relevant untracked prompt/report state | No untracked files under the inspected `AI-Logs/Prompts` and `AI-Logs/Reports` paths | The two required ignored log files created; no Git state change |

No untracked files under protected `UserData` were enumerated. The only task writes are:

- Prompt: `AI-Logs/Prompts/2026-11-19-absolute-path-matched-image-proportion-investigation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-absolute-path-matched-image-proportion-investigation-report.md`

No project source, tests, configuration, durable documentation, visual assets, or Git state changed. No staging, commit, push, branch switch, stash, reset, restore, cleanup, game launch, or manual acceptance occurred.
