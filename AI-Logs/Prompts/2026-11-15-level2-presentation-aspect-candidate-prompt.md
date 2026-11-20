# Tempest: Level 2-Only Presentation-Aspect Correction Candidate

Implement one opt-in, Level 2-only vertical presentation-aspect correction in
the `--atari-presentation` candidate. The accepted Level 2 placement (`+114`),
depth/perspective, near/far center relationship, shared portrait placement
(`-33.1`), Level 1, gameplay, deterministic recording/playback, geometry,
point order, lanes, and topology are frozen.

## Evidence and calibration

Analyze only these exact files:

- `UserData/Screenshots/Level-2/Level-2 atari presentation Square.png`
- `UserData/Screenshots/Level-2/Level-2 MAME Rectangle.png`

Do not inspect any other image, video, recording, or visual asset, and do not
modify either image. Treat MAME as a practical visual reference, not a
pixel-perfect arcade specification. Measure both rims and margins, distinguish
image measurements from source-derived bounds, and derive one stable factor.
Classify it as **PROJECT VISUAL CALIBRATION FROM AUTHORIZED MAME REFERENCE**.
Stop with PARTIAL if near/far ratios do not support one factor.

Apply the factor only to Level 2 points after depth projection, about the
projected tube center at that depth, before the existing `+114` placement.
Preserve screen X, depth denominator, horizontal scale, centers, center
separation, placement, and geometry. Use an anisotropic local Jacobian for
Level 2 where tube-attached artwork is world-oriented; do not distort
screen-oriented artwork, HUD, or unrelated levels.

## Scope and protection

Prefer only:

- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs` for narrow anisotropic local
  presentation integration
- `tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs`
- `tests/Tempest.Game.Tests/VectorRendererAtariPresentationTests.cs`

Do not modify other source, tests, durable documentation, images, or gameplay,
input, recording/playback, Approach timing, Level 1, Levels 3-9, Game Over,
projection/camera configuration, or transition stars. Preserve all existing
work. Do not stage, commit, push, clean, reset, restore, or stash.

Create exactly this prompt and its paired report under ignored `AI-Logs/`; do
not create any other `AI-Logs` file. Final outcome must be exactly one of
**READY FOR LEVEL 2 ASPECT CHECK**, **PARTIAL**, or **BLOCKED**.

## Validation and manual acceptance

Add mathematical tests for finite positive factor validation, unchanged X,
depth denominator/progression, center and placement invariants, near/far
ratios, anisotropic derivatives, world-anchor alignment, screen-oriented art,
and preservation of legacy Level 2, Level 1, Levels 3-9, Game Over,
determinism, Approach composition, and recording/playback state.

Run bounded tests with VSTest hang-timeout protection: focused adapter tests,
renderer/world-anchor tests, Level 2 placement/depth regressions, Level 1
regressions, Levels 3-9 candidate/fallback regressions, relevant Approach and
playback/recording/screenshot regressions, then the full suite. Run a Release
build with zero warnings/errors, `git diff --check`, and final Git/worktree/
index verification. Use isolated output if a running game holds normal output
files; never terminate it.

Do not launch the game. After successful validation, provide only this manual
command:

`dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 2 --atari-presentation`

Ask only whether the Level 2 shape is now vertically rectangular and
reasonably similar to MAME while accepted placement and depth/perspective
remain intact. Do not retune until the manual result is recorded.

TEMPEST_LEVEL2_PRESENTATION_ASPECT_CANDIDATE_20261115
