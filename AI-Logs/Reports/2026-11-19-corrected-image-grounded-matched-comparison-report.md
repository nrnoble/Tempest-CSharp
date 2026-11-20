# Corrected matched image-grounded presentation comparison

**Status: BLOCKED — the four authorized images were not attached.**

## Direct answers

1. Whether Level 7 remains measurably broader after translation and uniform-size normalization: **UNKNOWN**; no image landmarks could be measured.
2. Approximate residual proportion difference: **Not measurable**.
3. Cause (X:Y proportion, perspective, geometry, capture state, placement, or a combination): **UNKNOWN**.
4. Whether Level 1 compares better under the same normalization: **UNKNOWN**.
5. Whether Levels 2 and 5 visually recover their known proportion tendencies: **Unverified**.
6. Best Level-7-only Y gain: **Not fit or selected**.
7. Improvement over uniform similarity: **Not measurable**.
8. Whether a general affine fit materially outperforms one Y gain: **Untested**.
9. Global viewport: **Not reopened or modified**; this task did not independently revalidate the accepted display contract.
10. Level 7 gameplay geometry: **Not reopened or modified**.
11. Evidence for a bounded Level-7-only presentation experiment: **Insufficient** until the authorized images are provided.

## Scope and evidence availability

The task authorized exactly these four images:

- `Level-1 SideBySide.png`
- `Level-2 sidebyside.png`
- `level-5 Sidebyside.png`
- `Level-7 SidebySide.png`

The task attachment directory contained only `Pasted text #1.txt`. The attachment metadata supplied no image files. None of the four authorized images was available or inspected. Earlier mistaken comparison images and all workspace screenshots, images, videos, frames, and contact sheets were ignored; no workspace visual assets were searched, enumerated, opened, or analyzed.

This is an evidence-blocked result, not a visual comparison. The exact image extraction method, tube-line pixel isolation, landmark lists, correspondences, capture-state compatibility, and normalized fits were **not performed**. For the same reason, no raw bounds, near/far envelopes, rail angles, residual directions, diagnostic gain, or affine upper bound can be reported. Player/enemy state is not a basis for a tube comparison here because no tube pixels were available.

## Cross-level result table

`N/A` means not measured; it is not a zero residual or an assumed match.

| Level | Matched landmarks | Capture compatibility | C# raw W:H | Reference raw W:H | Similarity scale | Similarity RMS | Similarity max | Diagnostic Y gain | Y-gain RMS | Affine RMS | Near/far classification | Final diagnosis | Confidence |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | N/A | Unknown | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Unknown | No visual conclusion | None |
| 2 | N/A | Unknown | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Unknown | No visual conclusion | None |
| 5 | N/A | Unknown | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Unknown | No visual conclusion | None |
| 7 | N/A | Unknown | N/A | N/A | N/A | N/A | N/A | N/A | N/A | N/A | Unknown | No visual conclusion | None |

No near/far perspective table values are available for any level. Near/far width and height ratios, far-center displacement, and rail convergence all require visible corresponding tube landmarks.

## Level 7 fit comparison

| Diagnostic fit | Result |
|---|---|
| Translation + uniform scale + permitted discrete orientation/reflection | Not computable |
| One Y gain about a fixed anchor | Not computable; no gain or anchor selected |
| General affine upper bound | Not computable |

The relative improvement between these fits is unknown. A one-gain experiment is not justified by this evidence.

## Source/image reconciliation

The controlling prior source/math report, `AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md`, records implementation measurements of isotropic production geometry and Level 1/7 adapter gains; it also records Level 2's `1.314` vertical presentation factor and Level 5's `.670` X / `.880` Y gains (effective W:H near `.761`). Those prior calculations are not repeated here and are **not visual validation**. No source was changed or remeasured for this blocked comparison.

Accordingly:

- **IMPLEMENTATION-ESTABLISHED (prior report):** the source/transform relationships listed above.
- **VISUAL DIAGNOSTIC:** none; no image pixels were available.
- **MANUAL OBSERVATION:** the task's qualitative question is not a substitute for measurable matched landmarks.
- **PROJECT TRANSLATION / PROJECT DECISION:** the accepted display and gameplay boundaries remain untouched.
- **UNRESOLVED:** Level 1 versus Level 7 visual difference, all four capture states, normalized proportions, perspective differences, and any Level 7 gain.

The accepted display contract remains frozen at host `1600x1280`, viewport `960x1280` at `(320,0)`, aspect `3:4`, with uniform host mapping. Level 7 gameplay geometry remains frozen. These are preservation statements, not new image-derived findings.

## Repository state, validation, and handoff

| Field | Start | Finish |
|---|---|---|
| Branch | `master` | `master` |
| HEAD | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` | `c8bc40416eff4ffaac17862f7c894e5a34d133f9` |
| Upstream | `origin/master` | `origin/master` |
| Ahead / behind | `0 / 0` | `0 / 0` |
| Index | Clean | Clean |
| Tracked worktree | Pre-existing modification to `.github/copilot-instructions.md` | Preserved unchanged |
| Relevant untracked state | `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC` | Preserved unchanged |

The only task-created files are the required ignored records:

- Prompt: `AI-Logs/Prompts/2026-11-19-corrected-image-grounded-matched-comparison-prompt.md`
- Report: `AI-Logs/Reports/2026-11-19-corrected-image-grounded-matched-comparison-report.md`

No source, tests, configuration, durable documentation, visual assets, or Git index state changed. No game was launched; no build or tests were run because no executable code was changed and the required visual evidence was unavailable. No stage, commit, push, branch switch, stash, reset, restore, or cleanup was performed.

**Next step:** provide the four authorized image attachments. Then perform the specified matched-landmark analysis; do not infer a production correction from this blocked report.

**Completion token:** `TEMPEST_CORRECTED_MATCHED_IMAGE_PROPORTION_READONLY`
