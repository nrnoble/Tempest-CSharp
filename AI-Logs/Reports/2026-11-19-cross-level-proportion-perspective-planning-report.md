# Cross-level proportion-versus-perspective planning investigation

**Status: COMPLETE — read-only presentation-model investigation.** No implementation, production value, geometry change, or gameplay change is approved.

## Answers

1. **Do Levels 1 and 7 appear to miss the same class of correction?** Yes, for the near outer cross-section: each current C# envelope is approximately 1:1 while its reference is about 0.763 W:H. The Level 7 interior/depth relationship is a separate unresolved issue; Level 1's corresponding centers and rails compare more consistently.
2. **Is one common diagnostic target justified?** Yes. The four reference ratios cluster tightly around a mean of `0.76325` (median `0.763`, range `0.003`). This supports a shared **reference-envelope rule** as a diagnostic concept, not a final production constant or Atari rule.
3. **Which levels should the next outer-proportion experiment affect?** Levels 1 and 7 together. Both are currently isotropic and both show the same clear near-envelope ratio mismatch. Keep Levels 2 and 5 as controls; do not broaden the candidate to other levels.
4. **What gain should it expose?** A development-only range from identity `1.00` through about `1.32`, with checkpoints at `1.21`, `1.23`, and `1.31`. The ratio-derived gains are approximately `1.310` for Level 1 and `1.309` for Level 7. These are comparison points only; no value is selected for production.
5. **At what seam?** Scale the model cross-section's Y displacement about its source-space center **before depth projection**, inside the level-specific presentation adapter. Do not transform gameplay points or alter the depth equation.
6. **What anchor?** The existing source-space cross-section center, `(0,0)` for both Levels 1 and 7 in the source/math report. This is not a screenshot-pixel anchor, per-level placement, or `CenterY`/`YOffset`.
7. **Can outer proportion be tested independently of perspective?** Yes, most cleanly by scaling only the source cross-section Y term while leaving depth, centerline, `CenterY`, `YOffset`/Approach offset, and final placement unchanged. This tests outer proportion; it does not claim to resolve Level 7's far/core relationship.
8. **Is Level 1 adequately explained by one Y gain?** Its outer contour is well fit by the prior `1.230` diagnostic Y fit: residual falls from `4.66%` to `1.41%` of reference height, and affine fit is also about `1.41%`. The near/far center and rail directions broadly agree. Its approximate far/near rim sizes are not identical, so this is not a claim of exact perspective equivalence. The common-ratio gain (`1.310`) was not separately contour-fit.
9. **Is Level 7 outer shape adequately explained by one Y gain?** For the outer envelope, one Y gain materially helps: prior `1.210` fit reduces RMS from `5.43%` to `2.89%`; affine is `2.75%`, only about 4.9% lower than the Y-only residual. The common-ratio gain (`1.309`) yields the target bounding ratio by construction, but its contour residual was not measured. This supports an outer-only experiment, not a complete tube correction.
10. **What remains unresolved in Level 7?** The apparent far/core center displacement reverses sign between C# and reference, with a different apparent upper-notch-to-core relation and rail convergence. These measurements are low-confidence; gameplay vectors near the core may contaminate the landmarks.
11. **What evidence is needed before changing perspective?** A matched static pair in ordinary Playing, known Level 7, no transition, same presentation state, unobscured core/tube center, and enough clearly corresponding near-to-far rails. No video is presently justified.
12. **Is there enough evidence for a bounded development-only implementation prompt?** Yes, for an outer-proportion-only experiment on Levels 1 and 7. No perspective correction should be included until the Level 7 landmarks are better established.

## Ratio target and shared-rule comparison

The following values come from the two controlling investigation reports. The four reference ratios and current C# ratios are rounded to three decimals; derived gains are therefore approximate. A Y gain `g` maps a current cross-section ratio `r` to `r/g`. The mean reference ratio is used only as a **diagnostic target**.

| Level | Current C# outer W:H | Reference outer W:H | Gain to mean `0.76325` | Prior individual contour-fit Y gain | Common minus prior |
|---|---:|---:|---:|---:|---:|
| 1 | 1.000 | 0.762 | 1.3102 | 1.230 | +0.0802 |
| 2 | 0.762 | 0.764 | 0.9984 | 0.998 | +0.0004 |
| 5 | 0.760 | 0.762 | 0.9957 | 0.983 | +0.0127 |
| 7 | 0.999 | 0.765 | 1.3089 | 1.210 | +0.0989 |

Reference statistics: **mean 0.76325; median 0.763; minimum 0.762; maximum 0.765; range 0.003; population standard deviation 0.00130.** The range is about 0.39% of the mean and the population standard deviation about 0.17% of it. The ratio-derived Level 1/7 gains are nearly equal, while Levels 2/5 need essentially no additional correction. This is strong evidence for a common target-ratio *concept* across these four references.

The prior individual values are least-squares contour-fit diagnostics, not ratios recovered from the bounding-box formula. Their differences from the ratio-derived gains are substantial for Levels 1 and 7. Do not collapse these distinct measurements into a selected constant: compare both the target-ratio checkpoints and each prior contour-fit checkpoint in a future experiment.

Model alternatives:

- **A. Common reference-envelope rule — supported for a diagnostic.** Normalize each of these four near envelopes toward the clustered reference W:H, using the level's current ratio to derive its gain. For Levels 1/7 this means about 1.31; Levels 2/5 are already close.
- **B. Individually fitted per-level gains — useful comparison points, not a common accepted rule.** Existing contour fits are `1.230` and `1.210` for Levels 1/7. They improve the outer-contour scores but do not reach the same bounding-ratio target.
- **C. Shape-family-specific rules — not established.** The sample includes a circular Level 1, square Level 2, cross Level 5, and clover Level 7. Their ratios support a shared target more directly than a distinct rule per shape family; four levels do not establish a general family law.
- **D. No justified common rule — too conservative for the outer-envelope question.** A common *diagnostic target* is justified. A common production value, all-level policy, or perspective rule is not.

## Per-level evidence

### Level 1

The visual diagnostic reports a C# outer ratio of `1.000` against reference `0.762`. A one-Y contour fit at `1.230` reduces the RMS residual from `4.66%` to `1.41%` of reference height; general affine is approximately `1.41%`, so additional affine freedom adds no material improvement. The far center and rail convergence are broadly consistent after normalization. Approximate far/near width ratios are `0.117` C# versus `0.143` reference, and height ratios `0.120` versus `0.148`; these estimates are not exact enough to claim full perspective identity.

Conclusion: Level 1's outer mismatch is well explained by a vertical proportion adjustment. The common-ratio gain of about `1.310` is a separate bounding-ratio result and has not been run through the prior contour fitting.

### Level 2 control

Current/reference near-envelope ratios are `0.762`/`0.764`; contour similarity residual is `0.16%`, with a `0.998` diagnostic fit. The source-established adapter applies `Level2VerticalAspectFactor = 1.314` to model Y while leaving the projected vertical center term and final placement outside that factor; `LocalScaleY` is adjusted too. The ratio matches, although the reference far opening is proportionally larger. Level 2 is a strong outer-contour control, not proof that its perspective is exact.

### Level 5 control

Current/reference near-envelope ratios are `0.760`/`0.762`; contour similarity residual is `1.28%`, with a `0.983` diagnostic fit and affine residual `1.09%`. Its adapter applies `.670` X and `.880` Y to projected displacement about a projected near-origin anchor, with corresponding local X/Y scales. This gives an effective X:Y ratio near `0.761`. Central geometry is busy, so the far/core estimate remains low confidence. Level 5 supports the outer-ratio target but is a less isolated precedent for depth because its post-projection displacement scaling can also affect near/far center offsets.

### Level 7

The C# near-envelope ratio is `0.999` versus reference `0.765`. A contour-fit Y gain `1.210` reduces outer RMS from `5.43%` to `2.89%`; affine reaches `2.75%` (about 4.9% lower than Y-only). Thus a single Y gain is a useful low-complexity outer-envelope model. The ratio-derived gain `1.309` instead targets the common bounding ratio; its contour residual was not measured and must not be inferred from the `1.210` fit.

The reported far/deep bounds are about `118x122` C# versus `92x115` reference, and the estimated far-center displacement from the near-envelope center is `(0,-50)` versus `(-12,+74)` pixels. These are explicitly low-confidence: the reference core is crossed by gameplay vectors, while the near/far rail angle and correspondence cannot be robustly inferred from this single static pair. The apparent sign reversal is suggestive, not reliable enough to fit a perspective formula or decide whether the difference is geometry, projection, or capture alignment.

## Proportion and perspective are separate dimensions

1. **Cross-section proportion** is the X:Y shape of a tube cross-section at a given depth. The controlled candidate is a Y gain on cross-section displacement around its source center.
2. **Depth perspective** is how near and far cross-sections differ in size, center, and displacement, and how rails converge between them. The screenshots indicate that Level 7 may need this analyzed separately.

The current renderer routes tube points and entity anchors through `VectorRenderer.ProjectWorldPoint` and the selected per-level adapter. The shared `Projection` controls the ordinary reciprocal depth mapping and optional portrait-perspective adjustment. Level-specific adapter branches then control these distinct behaviors:

- **Per-level X/Y presentation:** Level 2's Y-only factor (`AtariPresentationAdapter.cs:103,352-367`); Level 5's `.670/.880` projected displacement and local scales (`:128-177`). Level 1 has a C# reciprocal projection with equal X/Y scale (`:337-347`). Level 7 uses the Atari reciprocal-depth mapping and currently equal-axis local scale (`:395-450`).
- **Near/far scale and center displacement:** per-level projection equations and depth terms in `AtariPresentationAdapter.Project`; the shared `Projection.Project` implementation for the legacy route and portrait perspective. A Y factor applied to a projected displacement can change a far center's offset from the near center even when it does not change depth itself.
- **Final per-level placement:** explicit post-projection translations, including `Level7VerticalPlacement = 228.99702f` (`AtariPresentationAdapter.cs:86-105,438`). Translation changes neither an individual cross-section ratio nor its source center.
- **Local entity scale/basis:** `AtariPresentationProjection.LocalScaleX/LocalScaleY` and `VectorRenderer.GetAtariLocalScaleAxes` (`VectorRenderer.cs:398-404`), plus `ApplyAtariLocalPresentationToOffset` (`:406-415`). Existing Level 2/5 corrections explicitly propagate to these consumers.
- **Shared host camera:** the 1600x1280 host, centered 960x1280 viewport, and 1.68 uniform playfield camera are common and do not create a per-level ratio change (`DisplayConfiguration.cs:16-28`). Do not change them.

### Transform-order assessment

| Seam | Assessment for this experiment |
|---|---|
| **A. Model Y before depth projection** | **Recommended.** Scale only cross-section `y` about its model-space center before the adapter's depth-dependent mapping. Leave `CenterY`, `YOffset`/Approach offset, camera/depth inputs, X, and final translation unchanged. This preserves the depth equation and centerline while changing cross-section proportion at both near and far depths. The nonlinear Level 7 projection still needs focused near/far regression tests; this seam does not establish that its perspective matches the reference. |
| **B. Projected logical Y about projected near-envelope center** | Not preferred. A single projected-space anchor preserves the near center but scales far-center displacement from that anchor; this mixes cross-section proportion with depth/perspective. |
| **C. Final logical playfield Y before host camera** | Not preferred. It is farther downstream and can affect every playfield object without retaining per-depth/source-center semantics. It risks inconsistent local bases and makes the correction harder to isolate from other playfield presentation. |
| **D. Existing Level 2/5 adapter patterns** | Reuse the per-level adapter seam and local-scale propagation, but follow Level 2's isolated model-Y term rather than copying Level 5's projected-displacement scaling. Level 2 is the closest precedent for changing section shape without scaling the projected centerline. |

### Anchor assessment

- **Selected: source-space tube center `(0,0)`.** The source/math report establishes zero point-cloud centroid for Levels 1 and 7. Scale the model Y coordinate relative to that source center (currently just `y`), without repurposing `LevelDefinition.CenterY`; this preserves horizontal position and the projection of the source origin, avoids arbitrary screen-space vertical drift, and gives tube vertices and entity world coordinates the same mapping at every depth.
- **Projected near-envelope center:** a plausible screen-space anchor for a different transform, but applying it globally makes far-center displacement scale with the near-to-far offset.
- **Complete near/far envelope center:** balances the whole projected envelope but moves both rims and changes their relationship; it is not the fixed model cross-section center.
- **Per-level presentation placement:** a translation applied after projection, not a geometric anchor. Scaling around it couples proportion to placement.
- **`LevelDefinition.CenterY`/`YOffset`:** projection baseline and vertical offset inputs, not the source cross-section centroid. Scaling them would also scale camera/Approach terms and cause drift.

This model-space anchor remains the same in ordinary Playing, Approach, and ExitZoom. Approach's additional depth/vertical offset must remain separate from the scaled cross-section coordinate. The adapter's final per-level placement remains after the projection and unchanged.

## Smallest justified DEVELOPMENT-ONLY experiment

1. **Scope:** Levels 1 and 7 only, with Levels 2 and 5 unchanged as controls. Do not infer applicability to other levels or all 99 levels.
2. **Parameter:** a render-only gain defaulting to `1.00`, bounded for this experiment to `1.00–1.32`; compare `1.00`, `1.21`, `1.23`, and `1.31`. The exact ratio-derived checks are about `1.310` and `1.309`. Label all non-identity values `DEVELOPMENT-ONLY`; select no production value.
3. **Seam/anchor:** multiply only model cross-section Y about source center `(0,0)` before depth projection in the existing adapter route. Do not alter source points, collision, lane topology, simulation, Z/depth, X, `CenterY`, `YOffset`/Approach offset, Y2D metadata, or final placement.
4. **Runtime option:** a narrow interactive diagnostic option is justified to make A/B captures comparable without source edits between runs. If added, require `--atari-presentation`, accept only Levels 1/7, reject non-finite/out-of-range/duplicate values and incompatible diagnostic modes, and do not serialize it into gameplay state or recording data. Keep it out of headless playback analysis; do not change the recording format.
5. **Registration/local basis:** route all tube vertices and tube-space entity positions through the same mapping. Propagate the vertical Jacobian to local-scale consumers and local offsets; keep screen-oriented shot glyphs screen-oriented. Verify the Blaster remains outward-facing from projected near/far rail correspondence and that shot/Spike/entity centers remain registered to their lanes.
6. **Focused automated tests:** target gain validation and defaults; Level 1/7 near and far W:H response; unchanged X coordinates, source geometry, depth progression, centerline and per-level placement; Level 2/5 control outputs; entity-to-tube projection and local Y scale; Blaster outward direction; and ordinary Playing, Approach, and ExitZoom behavior. Verify recording/playback simulation data and RNG/timing are unchanged. If a runtime gain can change during ExitZoom, freeze it for the transition or include it in focal-cache invalidation; do not change the accepted shared focal rule.
7. **Manual A/B after that future implementation only:** use the same static ordinary-Playing state and compare `1.00`, `1.21`, `1.23`, and `1.31` on each level. Illustrative commands, requiring the proposed option to be implemented first:

   ```text
   dotnet run --project src/Tempest.Game -- --level 1 --atari-presentation --tube-y-gain 1.00
   dotnet run --project src/Tempest.Game -- --level 1 --atari-presentation --tube-y-gain 1.31
   dotnet run --project src/Tempest.Game -- --level 7 --atari-presentation --tube-y-gain 1.00
   dotnet run --project src/Tempest.Game -- --level 7 --atari-presentation --tube-y-gain 1.31
   ```

These are not current commands: `--tube-y-gain` does not exist. No game was launched. Neal's manual acceptance would still be required before any behavior is accepted or documented as a project translation.

## Evidence, scope, and remaining work

**Evidence classifications**

- **IMPLEMENTATION-ESTABLISHED:** source/model point-cloud proportions, current Level 1/7 isotropic presentation, Level 2 factor `1.314`, Level 5 `.670/.880` gains, common camera uniformity, and adapter/local-scale routing. These are facts about the C# implementation, not Atari hardware.
- **VISUAL DIAGNOSTIC:** the four matched screenshot outer ratios, fit residuals, and approximate near/far measurements from the controlling image report. These are raster comparison results, not accepted arcade behavior.
- **INFERENCE:** shared `0.76325` reference-envelope target, applicability to Levels 1/7, seam recommendation, and experiment design.
- **PROJECT TRANSLATION / PROJECT DECISION:** existing adapter factors, placements, and accepted viewport; frozen during this investigation.
- **DEVELOPMENT-ONLY:** proposed gain option and experiment; not implemented or accepted.
- **UNKNOWN:** Level 7's exact far/core geometry-to-perspective explanation and whether the apparent center-displacement reversal is genuine.

The exact four opened paths were:

- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-1 SideBySide.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-2 sidebyside.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\level-5 Sidebyside.png`
- `C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-7 SidebySide.png`

All four opened successfully. No other image, screenshot, video, frame, contact sheet, or visual asset was inspected; no neighboring `UserData` files were enumerated. Existing quantitative measurements and source/transform mathematics were taken from the two controlling reports; this investigation did not rerun their extraction/fitting procedure. The Level 7 authorized image was visually re-examined only to assess core/rail confidence and possible gameplay-vector contamination.

Sources additionally checked: `.github/copilot-instructions.md`; `AtariPresentationAdapter.cs`; `VectorRenderer.cs`; `Projection.cs`; `DisplayConfiguration.cs`; `ExitZoomFocalCompensation.cs`; `LevelDefinition.cs`; and the accepted Level 7 / ExitZoom / viewport sections of `TEMPEST-BEHAVIOR-SPEC.md`. No raw ROM, web source, test, game process, or unrelated visual material was used.

## Validation and repository preservation

**Bounded arithmetic check: PASS.** Mean, median, spread, common-target gains, and differences from the supplied prior gains were calculated from the rounded report values. No source/tests/configuration changed, so no build or test run was required. No manual acceptance was performed.

Start state matched the expected baseline `c8bc40416eff4ffaac17862f7c894e5a34d133f9`:

| Field | Start | Finish |
|---|---|---|
| Branch | `master` | `master` |
| HEAD | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` | unchanged |
| Upstream | `origin/master` | `origin/master` |
| Ahead / behind | `0 / 0` | `0 / 0` |
| Index | Empty | Empty |
| Tracked worktree | Pre-existing modification to `.github/copilot-instructions.md` | Same; not modified by this task |
| Relevant untracked state | `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC` | Preserved; no other untracked paths enumerated |

Only the authorized ignored AI-Logs files were written:

- Prompt: `AI-Logs/Prompts/2026-11-19-cross-level-proportion-perspective-planning-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-cross-level-proportion-perspective-planning-report.md`

No source, tests, configuration, durable documentation, protected images, gameplay/recording data, or Git state was changed. Nothing was staged or committed; no branch, stash, reset, restore, cleanup, or game launch occurred.
