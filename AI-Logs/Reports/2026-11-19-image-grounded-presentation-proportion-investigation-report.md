# Matched image-grounded presentation-proportion investigation

**Status: PARTIAL — image comparison could not be performed because no authorized comparison images were supplied.**

## Direct answers

1. Whether Level 7 remains broader after translation and uniform-size differences are removed: **UNKNOWN**. No C#/reference image pair was available to measure.
2. Approximate residual width: **Not measurable**.
3. Whether any Level 7 difference is proportion, perspective, geometry, capture state, or a combination: **UNKNOWN** from this task's visual evidence.
4. Why Level 1 appears closer despite mathematical isotropy: **Unresolved**. The Level 1 and Level 7 source/transform math is isotropic, but neither visual pair was available for normalization or comparison.
5. Whether Levels 2 and 5 visually match in a way consistent with their existing presentation gains: **Unverified**. Prior source/math analysis finds effective projected W:H near `0.761` for both; that is not image evidence.
6. Whether one Level-7-only Y gain explains the mismatch: **Untested**. A fit requires matched landmarks from images.
7. Whether a general affine fit materially improves beyond one Y gain: **Untested** for the same reason.
8. Whether the global viewport remains frozen: **Yes**. Nothing in this image-incomplete task provides evidence to reopen the accepted viewport.
9. Whether Level 7 gameplay geometry remains frozen: **Yes**. No visual measurement establishes a geometry defect, and this task does not authorize changing it.
10. Is there enough evidence for a bounded Level-7-only presentation experiment? **No.** First provide the authorized matched comparison images; no correction or gain is selected here.

## Scope and available evidence

The supplied task attachment was the investigation prompt, not a comparison image. The available attachment metadata listed no image attachments. Accordingly:

- **Exact authorized comparison images inspected:** none.
- No workspace screenshots, videos, frames, or other visual assets were searched, enumerated, opened, or analyzed.
- `AI-Logs/Copilot.txt` was opened as a separately tagged text file; it was unrelated to this investigation and was not used as evidence.
- The prior source/math report, `AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md`, is controlling for source and transform findings. It explicitly states that no comparison pixels were measured in that earlier analysis.

This report therefore records a bounded evidence limitation, not a visual match result. It does not claim `ARCADE-OBSERVED` findings.

## Capture compatibility and image method

Capture-state compatibility is **UNKNOWN** for Levels 1, 2, 5, and 7. Without images, it is not possible to determine whether any pair represents ordinary Playing, exit zoom, Approach, different `K`/`ZOffset`, another perspective/depth state, or comparable HUD/composition.

The required rendered-line extraction, tube/background separation, landmark correspondence, orientation/reflection resolution, and translation-plus-uniform-scale similarity fits were **not performed**. No tube-line pixels or tube bounds were extracted. The prescribed method remains appropriate when the authorized pairs are available: identify unobstructed geometric tube vertices and rail endpoints, exclude gameplay/HUD pixels, align orientation, fit translation and a single uniform scale, and measure residuals before testing anisotropic, one-Y-gain, or affine fits.

## Normalized comparison table

All image-derived entries are unavailable. `N/A` means not measured; it is not a zero residual or an assumed match.

| Level | Matched landmarks | C# raw tube bounds | Reference raw tube bounds | Uniform scale / translation | Orientation / reflection | Similarity RMS / max residual, normalized by reference height | Diagnostic X/Y ratio | Anisotropic-fit RMS | Final visual diagnosis |
|---|---:|---|---|---|---|---|---|---|---|
| 1 | N/A | N/A | N/A | N/A | N/A | N/A / N/A | N/A | N/A | Unknown; visual “reasonably close” assessment not independently testable |
| 2 | N/A | N/A | N/A | N/A | N/A | N/A / N/A | N/A | N/A | Unknown; visual match versus the existing Y factor is unverified |
| 5 | N/A | N/A | N/A | N/A | N/A | N/A / N/A | N/A | N/A | Unknown; visual match versus existing X/Y gains is unverified |
| 7 | N/A | N/A | N/A | N/A | N/A | N/A / N/A | N/A | N/A | Unknown; broadness and its cause cannot be measured |

Uniform size, placement, X:Y shape, perspective, and residual geometry differences cannot be separated for any pair without the image landmarks.

## Near/far perspective table

| Level | C# near/far width ratio | Reference near/far width ratio | C# near/far height ratio | Reference near/far height ratio | Far-center displacement | Rail convergence | Classification |
|---|---|---|---|---|---|---|---|
| 1 | N/A | N/A | N/A | N/A | N/A | N/A | Unknown |
| 2 | N/A | N/A | N/A | N/A | N/A | N/A | Unknown |
| 5 | N/A | N/A | N/A | N/A | N/A | N/A | Unknown |
| 7 | N/A | N/A | N/A | N/A | N/A | N/A | Unknown |

No evidence distinguishes a near/far perspective difference from a cross-section proportion difference or an incompatible capture state.

## Level 7 diagnostic fits

| Fit | Result |
|---|---|
| Translation + uniform-scale similarity | Not computable; no paired landmarks |
| Single Level-7 Y gain about a fixed anchor | Not computable; diagnostic gain, anchor residual, near/far improvement, and rail-angle effect remain unknown |
| General affine upper bound | Not computable; no residual available to compare against the one-Y-gain fit |

No diagnostic value is proposed as an implementation constant. No implementation prompt or correction is authorized by this report.

## Source/math reconciliation and frozen decisions

The prior source/math analysis establishes, independently of these missing images:

- Production geometry W:H and centered RMS X:Y are `1.000` for Levels 1, 2, 5, and 7.
- Level 7 adapter X/Y gain is isotropic; Level 1 is also isotropic.
- Level 2's Y presentation factor is `1.314`; Level 5 uses `.670` X and `.880` Y gains, yielding projected W:H near `0.761` for each.
- Shared host camera and viewport gains are equal on X and Y; ordinary translations do not change shape ratios.

Those facts do not explain why Level 1 may look closer than Level 7, establish that a reference tube has non-1:1 proportion after perspective normalization, or determine whether the reference uses different geometry. The missing matched pixels are the evidence needed to resolve the contradiction; neither the prior math nor the supplied qualitative question substitutes for them.

The accepted viewport remains frozen at host `1600x1280`, viewport `960x1280` at `(320,0)`, aspect `3:4`, with uniform host mapping. Level 7 gameplay geometry remains frozen. Both decisions are consistent with the task constraints and the absence of new visual evidence.

The smallest useful next evidence is the expressly authorized, matched C#/reference image pair for each available level among 1, 2, 5, and 7. To support capture-state classification, each pair should show the same tube phase/state or include enough surrounding frame context to identify it. The Level 2 prototype, if provided, remains context-only and must not enter quantitative matching.

**Visual evidence classification:** `UNKNOWN` — no image pixels supplied or examined.

**Implementation evidence classification:** prior source/math findings are implementation measurements and existing `PROJECT DECISION` / `PROJECT TRANSLATION` behavior as documented by the controlling report; they are not arcade observations and do not establish an image correction.

## Validation and repository preservation

No source, tests, configuration, durable documentation, user/reference material, or Git state was changed. No game was launched and no build or tests were run; this report performs no executable-code change and the required visual analysis was blocked by missing inputs.

Starting repository state:

| Field | Start |
|---|---|
| Branch | `master` |
| HEAD | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` |
| Upstream | `origin/master` |
| Ahead / behind | `0 / 0` |
| Index | Clean |
| Tracked worktree | Pre-existing modification to `.github/copilot-instructions.md` |
| Relevant untracked state | `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC` |

Finish-state verification confirmed the same branch, HEAD, upstream, ahead/behind, index, pre-existing tracked modification, and untracked files. The only task-created files are the required ignored records:

- Prompt: `AI-Logs/Prompts/2026-11-19-image-grounded-presentation-proportion-investigation-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-image-grounded-presentation-proportion-investigation-report.md`

No staging, commit, push, branch switch, stash, reset, restore, cleanup, or manual gameplay acceptance was performed.
