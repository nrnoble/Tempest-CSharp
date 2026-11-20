# Tempest: Level 2 Placement-Only Correction Candidate

Implement one bounded, opt-in `--atari-presentation` Level 2 vertical-placement
candidate. Neal reports the current Level 2 candidate is too high to judge its
depth presentation. This milestone is placement-only: preserve all current
Level 2 shape, depth, coordinate mapping, near/far relationship, and gameplay.
Level 1 remains frozen at its accepted shape and placement. Do not investigate
ROM/disassembly, Math Box behavior, Level 2 mapping, other levels, or visual
assets.

Before editing, record the branch, HEAD, upstream, tracked changes, untracked
paths, and index state. Preserve all existing work and do not stage, commit, or
push. Read the Level 2 candidate and Level 1 placement handoff reports. Derive
the Level 2 translation from current source bounds and the established
portrait-host composition requirement. Do not copy Level 1's `+122`, use ROM
Y2D, or modify `CenterY`, `YOffset`, `-33.1`, zoom, or viewport dimensions.
Apply exactly one Y translation after Level 2's existing projection; X,
Jacobian, shape/depth, and all shared world anchors must otherwise remain
unchanged. Keep Levels 3-6 on legacy fallback, Levels 7-9 unchanged, and Game
Over on legacy projection.

Change only the existing candidate adapter and its relevant tests, plus these
two ignored artifacts. Do not alter gameplay, model geometry, topology, timing,
input, recording/playback, screenshot capture, HUD, portrait camera, or
`Projection.Project`. Do not inspect visual assets or launch the game.

Validate the focused Level 2 adapter and renderer tests; Level 1 and
Levels 3-9 regressions; relevant Approach, playback, recording, and screenshot
tests; the full VSTest suite with hang protection; a Release build with zero
warnings/errors; `git diff --check`; and final Git/index/worktree state. Report
incomplete validation as PARTIAL or BLOCKED, never as passed.

After successful automated validation, provide only:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation`

Ask Neal only whether Level 2's vertical placement now leaves sufficient room
above and useful lower margin while preserving its existing shape. Do not ask
for depth/perspective judgment until placement is accepted.

TEMPEST_LEVEL2_PLACEMENT_ONLY_CORRECTION_20261115
