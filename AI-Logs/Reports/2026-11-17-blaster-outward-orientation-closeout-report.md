# Blaster Outward Orientation Closeout

## Status

**COMPLETE — committed and pushed.** Neal's manual orientation acceptance is
recorded below. No interactive game was launched for this closeout.

## Manual acceptance and decisions

- **Blaster outward orientation: PASS.** Neal accepted the Blaster travelling
  around the outside of the visible rim during ordinary Atari-presentation
  `Playing`.
- The correction remains correct through ordinary rim movement and direction
  changes as manually tested. No further orientation adjustment is wanted.
- No brightness increase or other production brightness change will be
  pursued. The experiment showed that values below 1.0 visibly reduce
  brightness, while values above 1.0 provide no useful visible improvement.
- The existing yellow/current production Blaster color and intensity are
  unchanged and accepted as-is. The experiment served its diagnostic purpose;
  it was not a gameplay or presentation failure.

## Changes

### Removed brightness diagnostic

Removed the temporary `--blaster-brightness <multiplier>` surface:

- `DebugController`: removed multiplier parsing, duplicate/value validation,
  Atari-presentation context validation, visible-playback acceptance, and the
  option's generic file-mode handling.
- `Program`: removed command-line parsing, diagnostic output, and renderer
  wiring. After cleanup this file is back at its starting baseline.
- `VectorRenderer`: removed the multiplier field and constructor parameter,
  validation, conditional color selection, RGB multiplication, and
  multiplication/clamping helpers. Blaster artwork uses the original
  `Color.Yellow` production color, identical to multiplier 1.0.
- `README.md`: removed the option documentation. The file is back at its
  starting baseline.
- Tests: removed brightness parser/context/recording tests and RGB-scaling
  tests/helper. Added one regression that verifies the removed option is
  rejected by the existing generic unknown-option path for gameplay file
  modes.

The renderer's line-callback seam remains because the orientation tests use it
to capture exact geometry without a graphics window.

### Preserved orientation correction

The shared, render-only correction remains in `VectorRenderer.DrawPlayer`.
Only Atari-presentation `Playing` selects an outward normal. The implementation
uses the corresponding projected near-to-far rails to determine the tube
interior and selects the opposite side for the Blaster body. It supports
closed/open tubes, reflected presentation geometry, and direction changes
without per-level hardcoded normal signs.

The rim anchor, lane identity, movement, spinner response, highlighted lane,
artwork geometry/topology/dimensions, firing, collision, and gameplay state
remain unchanged. The geometry test covers all playable lanes for Levels 1-10,
both projected winding orientations, movement direction changes, the six-line
artwork, the rim anchor, yellow color, and render-state read-only behavior.
Approach, transition, death, exit, and legacy presentation remain outside the
correction.

### Durable documentation

Updated:

- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

Both record manual acceptance, the geometry-derived project translation,
unchanged gameplay position/movement, unchanged yellow color/intensity, and
the decision to conclude and remove the brightness diagnostic. The accepted
visual behavior is classified as a **PROJECT DECISION** and the geometry
mapping as a **PROJECT TRANSLATION**; neither is described as ROM-backed.

Accepted Levels 8 (+212.28139), 9 (+180.96825), and diagnostic Level 10 (+50)
placements remain frozen. No Level 7 architecture was changed. The separate
Blaster-exit presentation defect remains separate and was not investigated.
Recording/playback simulation and formats were not changed.

## Validation

- Focused Blaster orientation and removed-option tests:
  **PASS — 17 passed, 0 failed, 0 skipped.**
- Targeted Atari presentation, Blaster/player rendering, open/closed geometry,
  Levels 8/9/10, approach/transition/exit, display, and recording/playback
  regressions:
  **PASS — 463 passed, 0 failed, 0 skipped.**
- Full solution tests with `--blame-hang-timeout 5min`:
  **PASS — 1,621 passed, 0 failed, 0 skipped.**
- Release solution build:
  **PASS — 0 warnings, 0 errors.**
- `git diff --check`: **PASS.**
- Staged `git diff --cached --check`: **PASS.**

No canceled, stalled, blocked, or timed-out validation was counted as a pass.

## Staging, commit, and push

Exact staged files:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/BlasterPresentationTests.cs`
- `tests/Tempest.Game.Tests/DebugControllerTests.cs`

The complete staged diff was reviewed; all five files and their hunks belong
to this milestone. `.github/copilot-instructions.md`, local/reference files,
and `AI-Logs/` were excluded. The staged whitespace check passed.

- Commit: `34eedf0cf996cfe660494e34bdff2a1499696068`
- Message: `Close out Blaster rim orientation`
- Push: **PASS**, pushed `master` to its existing `origin/master` upstream.

Final Git state: `master` remains active, HEAD is the closeout commit,
upstream is `origin/master`, ahead/behind is 0/0, and the index is clean. No
milestone changes remain unstaged. The pre-existing `.github/copilot-instructions.md`
modification and untracked `Reference/arcade/MBUCOD.V05.txt`,
`Reference/arcade/MBUDOC.DOC`, and `screenshot000.png` remain present and
untouched.

## AI-Logs

- Prompt: `AI-Logs/Prompts/2026-11-17-blaster-outward-orientation-closeout-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-blaster-outward-orientation-closeout-report.md`
