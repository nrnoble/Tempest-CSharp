# Tempest: Level 2-Only Atari-Presentation Candidate

This prompt authorizes one bounded Level 2-only extension of the existing
opt-in `--atari-presentation` candidate. Level 1 is manually accepted and must
remain numerically and visually frozen. Do not work on Levels 3-9 except for
automated regression protection. Do not revisit ROM/disassembly investigation.

## Controlling evidence

Read `AI-Logs/Reports/2026-11-15-levels2-6-atari-csharp-coordinate-mapping-report.md`
and the Level 1 correction handoff as needed. Current source is authoritative
for the implemented candidate. Do not reopen Math Box analysis, ROM-table
recovery, projection archaeology, Level 1 investigation, Level 7 provenance, or
Levels 4/6 provenance.

The Level 2 mapping is displayed Level 2 to Atari record `01`, closed topology,
16 ordered points on each side, approximate centered uniform fit
`u=1.436688312`, X/Y orientation `+1/+1`, with `j=(i+2) mod 16` used only as
comparison correspondence. The comparison rotation must not change production
points, point order, lanes, entity lanes, or traversal.

Verify existing `RomPresentationMetadata`: record/index 01, `lev_scale=0x1c`,
`lev_y3d=0x50`, signed Y2D `-224`, closed topology. Do not duplicate metadata
or change it unless it conflicts with the handoff; stop and report any
conflict. C# topology remains authoritative and must agree.

## Protected behavior and boundaries

Preserve Level 1 exactly: shape, depth progression, placement, no clipping,
upper visual space, point order/topology, gameplay/playback, and shared
portrait placement `-33.1`. Do not modify its presentation arithmetic or
constants. The candidate-specific Blaster-exit drawing defect is out of scope.

Do not inspect, search for, enumerate, open, compare, or analyze any visual
asset. Neal performs visual evaluation. Do not launch the game.

Level 2 must preserve existing C# points/order/lanes/closed topology, gameplay
and model geometry, movement, collisions, playback determinism, and legacy
rendering when the option is disabled. Approach simulation, transition stars,
Game Over legacy exemption, recording formats, pause/step/input/screenshot
behavior remain unchanged. Candidate projection may compose through the
existing render-only Approach seam and should be tested if applicable.

Before editing, calculate candidate near/middle/far behavior using the current
adapter and Level 2 frame mapping and compare it numerically with legacy
projection. Keep coherent looking-down-the-tube progression, finite continuous
projection, shared tube/object projection, symmetry, and model/renderer
consistency. Preserve C# `depth + ZOffset`; do not use endpoint-normalized
reciprocal depth, change global `Projection.K`/`Projection.ZOffset`, or
pre-tune Level 2 placement. Do not copy Level 1 `+122`, change `CenterY`,
`YOffset`, `-33.1`, or alter the portrait camera.

If direct signed-Y2D application causes the harmful global displacement
diagnosed for Level 1, preserve and validate the metadata but omit that literal
displacement from this first visual candidate. Do not add arbitrary tuning. If
no evidence-based translation avoids diagnosed failures, stop with `PARTIAL`.
Explain exactly which Atari metadata is validated/used and which terms are
deliberately omitted.

All Level 2 world anchors using candidate presentation must share its
transform: tube, rails, both rims, player/Blaster, enemies, shots, Spikes,
rim/below-rim positions, and any local depth-responsive sizing. Do not redesign
objects/effects.

## Scope and implementation

Prefer changes only to:

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

Modify `VectorRenderer.cs` only for a narrowly scoped supported-level change
not already data-driven. Do not modify projection/camera, level/gameplay,
input, recording/playback, screenshot, HUD, Game Over, or durable documentation
files. Stop with `PARTIAL` if broader production changes are necessary.

Add Level 2 mapping only (`u=1.436688312`, `e=+1`), explicitly approximate.
Do not encode the comparison `+2` rotation or modify Level 2 points. With the
option off, Level 2 remains exactly legacy. With it on, Level 1 numerics stay
unchanged; Levels 3-6 retain explicit legacy fallback; Levels 7-9 retain
existing candidate behavior and numerics.

## Automated coverage

Prove Level 2 candidate-only support, exact mapping, point/order/lane/topology
immutability, no comparison rotation, unchanged legacy output, square X
symmetry, finite continuous near/middle/far depth, meaningfully recessed far
rim, local Jacobian/anchor consistency, consistent world anchors, and no
gameplay/model mutation. Also protect Level 1 numerics/`+122`, Levels 3-6
fallback, Levels 7-9 numerics, Game Over legacy path, deterministic Approach
composition, repeated-input determinism, explicit invalid-input failures, and
presentation-independent playback/recording. Do not use screenshot pixels as
an automated oracle.

## Validation and repository protection

Before substantive work record branch, HEAD, upstream, tracked modifications,
untracked paths, and index state. Preserve all existing work. Do not clean,
reset, restore, stash, stage, commit, push, or update durable documentation.
If a running game locks standard build outputs, do not stop it; use an isolated
temporary output path.

Use bounded VSTest commands with hang-timeout protection. Run adapter-focused
tests, candidate renderer tests, Level 1 regressions, Levels 3-9 candidate/
fallback regressions, relevant Level 2/Approach/playback/recording/screenshot
regressions, the full suite, a Release build with zero warnings/errors,
`git diff --check`, and final Git/worktree/index verification. Incomplete,
failed, blocked, or timed-out validation is not a pass.

## Outcome, report, and manual boundary

Select exactly one: `READY FOR LEVEL 2 MANUAL CHECK`, `PARTIAL`, or `BLOCKED`.
Create only this prompt and
`AI-Logs/Reports/2026-11-15-level2-atari-presentation-candidate-report.md`
under `AI-Logs/`; do not stage or commit either.

The report must include outcome; starting/final Git state; mapping and
approximation; depth and vertical-placement treatment; Level 1 and other-level
preservation; geometry/gameplay/playback protection; bounded tests and exact
results/counts; Release warnings/errors; diff check; limitations and pending
manual acceptance; the manual command below; and the first manual question.

After all required automated validation passes, provide only this launch
command and ask only whether Level 2's tube shape, looking-down-the-tube
presentation, and overall placement look reasonably similar to arcade/MAME
expectation without an obvious new problem:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation`

Do not ask Neal to test Level 3, provide a recording, or revisit Level 1.
If shape is clearly wrong, stop broader testing and diagnose before placement
tuning; if only placement is wrong, record it separately rather than
immediately changing projection.

TEMPEST_LEVEL2_ATARI_PRESENTATION_CANDIDATE_20261115
