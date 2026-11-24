# Atari tube depth-origin Y gain diagnostic

**Status: IMPLEMENTED; AUTOMATED VALIDATION PASS; MANUAL PRESENTATION ACCEPTANCE PENDING.**

Completion token: `TEMPEST_TUBE_DEPTH_Y_GAIN_DIAGNOSTIC`.

## Scope and implementation

Added the general development-only `--tube-depth-y-gain <value>` option. The
neutral/default value is `1`; `0` removes the depth-dependent camera-origin
contribution; fractions reduce it; values above one exaggerate it; negative
values reverse it. Values are signed finite invariant-culture floats with no
aesthetic limit. Missing, malformed, non-finite, joined and duplicate forms
are rejected, as is use without Atari presentation or without an existing
level/start-level/playback-file context. This follows the other tube
diagnostics' existing CLI context convention. `--help` describes the option,
and explicit use prints its semantics before interactive setup.

The shared Atari reciprocal-depth adapter applies `(gain - 1)` times the
camera-origin displacement relative to its near-plane projection. It skips
the calculation entirely at `1`, preserving the existing floating-point
evaluation order and prior output. It does not change model Y, signed Y2D,
common screen placement, local Jacobians, or depth-size ratio. The renderer
uses the same adjusted projection for tube vertices, attached anchors and
local artwork placement. Tube scale continues to scale geometry about the
per-depth origin, while vertical offset remains a final common translation;
the existing Y gain and the new gain are independent. None of these settings
is stored as gameplay recording data.

The projection is a **DEVELOPMENT-ONLY PROJECT TRANSLATION**; this diagnostic
does not rewrite ROM evidence and does not establish an arcade-correct
calibration. The Level 13 geometry, frame, topology, lane state, camera-height
metadata and all existing calibration defaults remain unchanged.

The option is accepted for supported Atari levels 1-13 and direct Level 15.
The shared reciprocal camera-origin term is used by Levels 7-13 and 15.
Levels 1-6 use separate existing mappings without this term, so changing the
gain has no effect on those levels. The same shared world mapping is already
used in normal Playing, Approach and ExitZoom; the gain therefore affects
those states where the reciprocal mapping applies, without redesigning their
existing transition or focal-compensation logic. HUD remains on its separate
fixed path. Neutral behavior remains unchanged in all states.

All finite float values are accepted by the parser, including `float.MaxValue`
and `float.MinValue`. Some such values cannot produce a representable finite
float coordinate for every point; in that case the existing explicit
non-finite projection error is raised rather than silently clamping. Large
representable probes such as `1e30` produce finite deterministic results in
the tested Level 13 geometry.

## Changed files for this milestone

- `src/Tempest.Game/Debug/DebugController.cs` — option parsing, validation,
  playback-command recognition and diagnostic option record.
- `src/Tempest.Game/Program.cs` — pre-window validation, diagnostic notice,
  `--help`, and renderer wiring.
- `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs` — neutral-preserving
  depth-dependent camera-origin gain.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — per-renderer gain and
  shared projection wiring.
- `tests/Tempest.Game.Tests/TubeDepthYGainTests.cs` — parser, projection,
  registration, calibration composition, neutral regression and transition
  coverage.
- `README.md` — option semantics, supported projection paths and state scope.
- Paired prompt and this report under `AI-Logs/`.

These are additions to the pre-existing uncommitted Level 13 candidate, not a
replacement for that candidate. No accepted gameplay or visual behavior was
recalibrated.

## Validation

The repository uses .NET SDK 9.0.304, VSTest and xUnit v2. Commands were run
without dependency restore. Tests used the specified hang timeout.

- **PASS:** Focused diagnostic test run, 43 tests, before expanded coverage.
- **PASS:** Combined diagnostic/parser, calibration, registration, Level 13
  production, Level 15, Atari projection/HUD, Fuseball, recording/playback,
  Approach and ExitZoom regressions: **577 passed, 0 failed, 0 skipped**.
  The new tests also cover zero/negative/large values, bit-identical omitted
  versus explicit neutral for Levels 1-13 and 15, no-effect Levels 1-6,
  near/far geometry and scale invariants, tube/Flipper/Fuseball/projectile and
  player registration, independent offsets and unchanged HUD/state.
- **PASS:** Final complete Release solution suite, with
  `--blame-hang-timeout 5min`: **1,913 passed, 0 failed, 0 skipped**. This
  includes the existing Level 13 production soak, topology, spinner/lane,
  popup/vector/animation, open-rim, playback and transition regressions.
- **PASS:** Release solution build with `--no-restore`: **0 warnings,
  0 errors**.
- **PASS:** Real CLI `--help` and pre-window rejection checks for missing
  Atari mode, NaN, missing value, and duplicate option. Each invalid case
  returned exit code 2; no interactive window was opened.
- **PASS:** `git diff --check`; Git emitted only existing LF-to-CRLF
  conversion warnings for README and LevelLoader.
- No gameplay or manual visual acceptance was performed. No interactive game
  was launched.

## Manual Level 13 calibration handoff

Run each command manually from the repository root; the existing Level 13
defaults for all other controls are neutral:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --tube-depth-y-gain 1.0
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --tube-depth-y-gain 0.75
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --tube-depth-y-gain 0.5
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --tube-depth-y-gain 0.25
dotnet run --project src\Tempest.Game -- --start-level 13 --atari-presentation --tube-depth-y-gain 0.0
```

For Level 13, decreasing the value reduces the existing downward
depth-dependent drift; zero removes that contribution, while one preserves
current behavior. These are exploratory probes, not recommended defaults or
limits. The near/far perspective size ratio does not change. Neal's MAME
comparison and manual visual acceptance remain required.

## Repository protection

The initial branch was `master` at
`534cbf21150dc501300c548f98972a4c64198602`, tracking `origin/master` at the
same commit, with an empty index and the previously uncommitted Level 13
candidate and unrelated untracked material. No reset, restore, stash, clean,
rebase, stage, commit or push was performed. The required prompt and report
are new untracked files under the existing `AI-Logs` directories, which also
contain tracked historical records; staging is explicitly prohibited.
