# Level 5 Visual-Calibrated Presentation Candidate

Date: 2026-11-16

## Outcome

**READY FOR LEVEL 5 MANUAL CHECK**

The opt-in Level 5 presentation candidate is isolated to the rendering path.
Required automated validation passed. Neal's manual visual acceptance has not
been performed; no gameplay was launched.

## Starting repository state

- Branch: `master`; HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`.
- Upstream: `origin/master`; ahead/behind: `0/0`; index: empty.
- Before work, 16 tracked files were modified and 8 paths were untracked. This
  already-dirty state included the presentation adapter and candidate tests.
  Existing work was preserved and integrated; no unrelated file was changed.
- The candidate source/test paths were pre-existing dirty or untracked user
  work. The two authorized AI-Logs files are ignored working artifacts.

## Candidate implementation and evidence

`AtariPresentationAdapter.ProjectLevel5` uses the existing C# `Projection`
with its current depth denominator, obtains the projected near-rim origin,
and maps each projected displacement around that origin. The candidate then
adds the fixed render-time placement offset. Approach depth/vertical offsets
remain in the underlying projection, so the transform composes with the
existing Approach path.

| Value | Implementation | Provenance |
|---|---:|---|
| Net X extent scale | `0.670` | Rounded normalized MAME-to-C# X extent ratio; PROJECT VISUAL CALIBRATION |
| Y extent scale | `0.880` | Rounded normalized MAME-to-C# Y extent ratio; PROJECT VISUAL CALIBRATION |
| Additional X aspect factor | Not applied separately | The net X value is used directly; the report's `0.762` aspect ratio is calibration derivation, not another renderer gain |
| Vertical placement | `+136.4026` logical Y from the projected near-rim anchor | Renders the near-rim anchor at about `53.2%` of the 960px portrait viewport; includes the established common-center translation plus the calibrated `3.2%` downward exception |
| Horizontal placement | No additional X translation | Existing logical center remains X=`400` / host center |

These are rounded first-candidate visual values, not ROM-backed constants,
accepted behavior, or model coordinates. No Atari Level 5 correspondence scale,
Atari reciprocal-depth term, or depth-denominator change is used. Level 5
remains absent from `HasModelFrameMapping`.

At the established production camera values (`K=70`, `ZOffset=5`, portrait
playfield zoom `1.26`), calibrated near-rim bounds are approximately:

| Coordinate space | X bounds | Y bounds | Width x height |
|---|---:|---:|---:|
| Logical | `220.927..579.073` | `201.657..672.057` | `358.145 x 470.400` |
| Portrait host | `414.367..865.633` | `214.367..807.072` | `451.265 x 592.704` |

The near-rim anchor is host `(640, 510.720)`, or `53.2%` of viewport height.
The normal gameplay HUD lower bound is host Y=`165.6`; the near-rim host top
leaves approximately **48.767 pixels of positive vertical-envelope clearance**.
The bound is a mechanical check, not a visual acceptance judgment.

## Preservation and renderer treatment

- Level 5 points, order, lane identities, closed topology, center/configuration,
  and gameplay state are not changed. The mapping is used only while rendering
  with `--atari-presentation`; the disabled path continues to use the exact
  legacy `Projection.Project` result.
- Near/middle/far depth inputs and the C# `K/(depth + ZOffset)` progression
  remain unchanged. Near stays larger than far; their local-scale ratio is
  unchanged. Only projected displacements from the near-rim anchor receive
  the candidate extent transform.
- Tube vertices, near/far rims, rails, lane/world anchors, player, enemies,
  shots, Spikes, and computed pivot endpoints flow through the same candidate
  mapping. Tube-oriented local glyphs receive the X/Y gains; player/pulsar
  local offsets use the same per-axis transform.
- Intentionally screen-oriented shots, enemy-shot artwork, and existing
  effects retain scalar local scaling rather than the anisotropic gains. HUD,
  transition stars, Game Over, and gameplay simulation are not transformed.
- Levels 1-4 constants and mappings were not changed. Level 6 remains on the
  unchanged legacy fallback. Levels 7-9 retain their existing candidate
  behavior.

## Validation

All commands completed; none timed out, hung, or was canceled.

| Validation | Result |
|---|---|
| Focused Level 5 presentation tests (adapter, renderer, tube) | PASS — 13 tests |
| Adapter and renderer/world-anchor regressions | PASS — 66 tests |
| Level 5 gameplay, Approach, transition, recording/playback, screenshot, CLI and display regressions | PASS — 323 tests |
| Full test suite (`dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --blame-hang-timeout 5min`) | PASS — 1,497 tests |
| Release solution build (`dotnet build .\TempestCSharp.slnx -c Release --no-restore`) | PASS — 0 warnings, 0 errors |
| `git diff --check` | PASS — no whitespace errors; Git emitted only existing LF-to-CRLF notices for `LevelLoader.cs` and `InputSystem.cs` |

Focused tests cover opt-in/legacy output, unchanged stored points/order/lanes/
topology and gameplay state, anchor and per-axis finite differences, depth
relationship, no Level 5 Atari mapping, HUD clearance, consistent world
anchors, Approach determinism, invalid values, and undistorted screen-oriented
shot offsets. Existing regression tests cover accepted Levels 1-4, Level 6
fallback, Levels 7-9, Game Over, transitions, recording/playback, and
screenshot capture. No screenshot pixels were used as an automated oracle.

During implementation, a focused finite-difference check exposed float
precision at an overly small probe interval; the bounded probe was enlarged.
A later build caught remaining call sites after a helper rename; those were
updated. Final focused tests, the complete suite, and Release build all pass.

## Limitations and manual acceptance

- This is an experimental first candidate based on one authorized MAME
  reference image. It is not accepted arcade behavior or a durability/spec
  update.
- Neal's visual judgment remains required for placement, HUD separation, shape,
  size, and perspective. Manual acceptance: **PENDING**.
- Do not launch the game from this session. Neal's task-appropriate command:

  `dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 5 --atari-presentation`

  First question: "Is the Level 5 candidate now appropriately placed beneath
  the HUD and visually usable enough for normal evaluation of its shape, size,
  and perspective?"

## Ending repository state

- Branch, HEAD, upstream, and tracking divergence are unchanged (`master`,
  `385f603a94391756b2ca6cccd524dc17b281e782`, `origin/master`, `0/0`).
- The index remains empty. The same 16 tracked modifications and 8 pre-existing
  untracked paths remain; no additional source/test path was introduced.
- Only the two authorized ignored AI-Logs artifacts were created. Nothing was
  staged, committed, pushed, cleaned, reset, restored, or stashed.
