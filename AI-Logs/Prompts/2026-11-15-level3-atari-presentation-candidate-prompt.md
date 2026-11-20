# Tempest Level 3 Atari-Presentation Candidate

Date: 2026-11-15
Scope: one opt-in Level 3-only presentation candidate and placement check.

Enable the displayed Level 3 approximate model-frame mapping from the Levels
2-6 coordinate-mapping handoff (`u=1.756696429`, X/Y orientation `+1`), without
production point rotation, reordering, or gameplay geometry/state changes.
Verify Level 3's existing metadata: record `02`, scale `0x18`, camera height
`0x50`, signed Y2D `-192`, and closed topology. Keep the mapping explicitly
approximate.

Before editing, reconstruct legacy and literal generic-candidate bounds and
depth behavior at near, middle, and far depths. Do not reproduce Level 1/2
failure classes: preserve the established C# reciprocal-depth relationship,
avoid the Atari reciprocal/Y2D terms if they clip or reverse the tube, and
derive a single Level 3-only vertical translation from the full envelope in
the 960-pixel portrait viewport. Apply it after projection. Do not add an
aspect factor or tune perspective.

Preserve accepted Levels 1 and 2 exactly, including Level 2's `1.314` aspect
and `+114` placement; preserve the shared `-33.1` portrait placement, K,
ZOffset, camera, viewport, gameplay, deterministic recording/playback,
Approach timing, Game Over, and Levels 4-9 behavior. Level 3 tube-attached
anchors must use the same candidate projection and placement; keep screen-
oriented artwork separate. Do not redesign geometry or artwork.

Preferred code scope:

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`
- Change `VectorRenderer.cs` only if narrow integration requires it.

Do not modify Projection, display configuration, level data/metadata, gameplay,
input, recording/playback, screenshots, HUD/text, Game Over, or durable
documentation. Do not inspect visual assets, launch the game, stage, commit,
push, reset, clean, restore, or stash. Preserve all pre-existing work.

Prove mapping specificity, unmodified point/lane/topology/game state, legacy
rendering when disabled, continuous finite near/middle/far projection,
meaningful far recession, coherent center progression, uniform Y-only
placement, unchanged dimensions/near-far separation/Jacobian, shared world
anchor projection, and unchanged behavior for Levels 1-2 and 4-9, Game Over,
Approach, playback/recording, and repeated inputs. Invalid/non-finite mapping
inputs must fail explicitly. Do not use screenshot pixel tests.

Run bounded VSTest commands with hang-timeout protection for Level 3 adapter,
candidate renderer, accepted Levels 1-2, Levels 4-9, related Level 3/Approach/
playback/recording/screenshot regressions, and the full test suite. Also run a
zero-warning/zero-error Release build, `git diff --check`, and final Git
worktree/index verification. Do not report incomplete validation as passed.

Create only this prompt artifact and the paired report
`AI-Logs/Reports/2026-11-15-level3-atari-presentation-candidate-report.md`;
leave both unstaged and uncommitted.

After successful automated validation, do not launch the game. Provide only:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 3 --atari-presentation`

Ask only whether Level 3 placement is reasonable, there is stable visual room
above and useful lower margin, and there is no obvious tube-shape failure.
Do not ask for perspective/depth judgment before placement is accepted, and do
not ask about aspect correction or another recording.

Required outcome: exactly one of `READY FOR LEVEL 3 PLACEMENT CHECK`,
`PARTIAL`, or `BLOCKED`. Report Git baseline, mapping/evidence classification,
reconstruction, depth treatment, placement derivation and seam, geometry/
gameplay protection, world-anchor consistency, Levels 1-2 and other-level
preservation, tests/validation, manual command/question, and final Git state.

TEMPEST_LEVEL3_ATARI_PRESENTATION_CANDIDATE_20261115
