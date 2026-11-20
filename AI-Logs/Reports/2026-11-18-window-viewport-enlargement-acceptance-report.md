# 1600x1280 Window / 960x1280 Viewport Acceptance Closeout

## Status

**NEAL MANUAL ACCEPTANCE — PASS.** Neal responded **"Accepted"** to the
enlarged-window and portrait-viewport candidate after receiving the requested
manual checklist. The implementation report remains unchanged as the historical
record of the candidate before manual acceptance.

## Accepted behavior

- Host window client area: `1600 x 1280`.
- Centered portrait viewport: `960 x 1280`, origin `(320,0)`.
- Exact `3:4` viewport aspect; `320` pixels of margin on each side and none
  above or below.
- The prior `720 x 960` viewport is enlarged uniformly by `4/3`.
- Logical presentation and accepted Level 7/8/9/10 placements remain
  unchanged.
- The blue viewport border remains one host pixel.
- This acceptance does not include C# versus arcade/MAME screenshot-proportion
  matching.

The host dimensions and viewport layout are recorded as a **PROJECT DECISION**.
The uniform host-space mapping is recorded as a **PROJECT TRANSLATION**.
Neither is asserted to be ROM-backed or arcade-observed behavior.

## Durable documentation updated

- `TEMPEST-BEHAVIOR-SPEC.md` now records the accepted dimensions, mapping,
  preservation boundaries, option/legacy behavior, and separate deferred
  screenshot-proportion investigation.
- `TEMPEST-DEVELOPMENT-NOTES.md` now identifies the accepted current window
  and viewport contract and corrects the former pending `1280 x 960` display
  candidate.

No implementation source or test files changed during this acceptance
closeout. The original implementation report was not rewritten.

## Validation and Git

- **PASS** `git diff --check`.
- Documentation-only closeout; no additional build or test run was needed.
- No game, screenshot, image, or video was opened or inspected.
- No files were staged, committed, or pushed.
- Pre-existing local changes and untracked reference files remain preserved.

## Paired AI-Logs records

- Prompt: `AI-Logs/Prompts/2026-11-18-window-viewport-enlargement-acceptance-closeout-prompt.md`
- Report: `AI-Logs/Reports/2026-11-18-window-viewport-enlargement-acceptance-report.md`
