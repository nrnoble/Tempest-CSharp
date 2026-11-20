# Blaster-shot ROM rendering implementation report

## Starting state and evidence

- Started on `master` at expected baseline
  `4ccb3cfb6cf3a1e72ac47bcfc79454ce257cb9de`.
- The only initial Git status entries were the 14 pre-existing, untracked
  `screenshot000.png` through `screenshot013.png`; no tracked or staged changes
  were present.
- Reused `2026-11-08-blaster-shot-rom-visual-investigation-report.md`.
- Read the complete user prompt, verified its single standalone completion
  token, and copied all 633 lines into the ignored paired prompt record.
- Rechecked `Reference/arcade/Tempest Rom Source code.md`: `$ced0` points to
  `$1594`; `draw_shots` at `$b75a` selects the player-shot graphic; and
  `graphic_at_mid` supplies the lane/depth placement and external scale.
  The vector program is `$3594-$3620`, with the two sparkle color groups.

## ROM coordinates and translation

The raw `vldraw` words use the listing's signed coordinate decoding: the
upper coordinate field is Y and the lower field is X. At `$3600` and `$3608`,
`1ff50000` decodes to `(x=0,y=-11)`; at `$3610` and `$3618`, `0000000b`
decodes to `(x=+11,y=0)`. These raw values resolve the conflicting inline
guesses; no symmetric replacement was inferred. The first four index-1 moves
leave the beam at `(-11,11)`. Applying the four raw moves in order gives
`(-11,11)+(0,-11)=(-11,0)`, then `(-11,-11)`, then `(0,-11)`, then
`(11,-11)`.

Accumulating the relative blank moves gives the ROM point coordinates:

- Color index 8: `(0,0)`, `(7,0)`, `(5,5)`, `(0,7)`, `(-5,5)`, `(-7,0)`,
  `(-5,-5)`, `(0,-7)`, `(5,-5)`.
- Color index 1: `(15,0)`, `(11,11)`, `(0,15)`, `(-11,11)`, `(-11,0)`,
  `(-11,-11)`, `(0,-11)`, `(11,-11)`.

The C# renderer now emits one compact pixel mark for each zero-displacement
point instruction. It does not submit blank movement as a line, and no longer
submits the former eight full-diameter radial lines. Logical ROM Y-up is
converted to screen Y-down. The picture is independent of shot age, lane
orientation, and shot order; each shot is still placed at the accepted lane
midpoint and uses the existing depth projection.

`PlayerShotBaseSize` remains `4.5`; it now scales the maximum logical radius
`sqrt(11^2+11^2)`, followed by the existing reciprocal depth scaling. This is
a project-space normalization, not an arcade pixel radius.

The local project had no authoritative RGB translation for shot indices 8 or
1. The candidate uses amber `#FFB000` for index 8 and red-orange `#FF4000` for
index 1. Both are explicitly **PROJECT TRANSLATION / MANUAL ACCEPTANCE
PENDING**, not literal Atari palette facts. MAME frames support small warm
lane marks and a separate white target-destruction burst, but do not establish
literal RGB or point coordinates.

`Shot.Rotation` remains unchanged simulation state and continues to advance;
the renderer no longer reads it. No firing, capacity, cooldown, shot movement,
collision, scoring, Spike, sound, removal/clearing, recording schema, or
playback-input behavior was changed.

## Files changed

- `src/Tempest.Game/Rendering/PlayerShotVectorProgram.cs` — immutable point
  groups, relative-move accumulation, explicit provisional colors, and
  normalized ROM point radius.
- `src/Tempest.Game/Rendering/VectorRenderer.cs` — point rendering replaced
  the radial star while retaining the existing placement and perspective.
- `src/Tempest.Game/Gameplay/Shot.cs` — comment corrected to clarify that
  legacy rotation state no longer affects the static rendering.
- `tests/Tempest.Game.Tests/PlayerShotRenderingTests.cs` — rendering and
  playback-boundary coverage.
- `TEMPEST-DEVELOPMENT-NOTES.md` and `TEMPEST-BEHAVIOR-SPEC.md` — ROM,
  MAME-observed, project-translation, unresolved, and acceptance statuses.
- `AI-Logs/Prompts/2026-11-08-blaster-shot-rom-rendering-implementation-prompt.md`
  and this report are ignored project records.

## Automated validation

Added focused tests for the two point groups and their accumulated coordinates;
the qualified raw-word movements; the explicit project color translations;
17 point-only submissions; no white radial star; age/lane/order invariance;
finite monotonic near/middle/far scaling; independently projected active shots;
and pause, forward-step, and reverse-reconstruction rendering equality.

- Player-shot rendering tests: **7 passed**.
- Expanded rendering, input, simulation, collision/Spike-name, destruction,
  visible/headless playback, recording, and screenshot-capture coverage:
  **524 passed**.
- Full solution test suite: **996 passed, 0 failed**.
- Full solution build: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**.

The existing gameplay tests remain unchanged and passing, including checks for
shot cooldown/capacity, movement/rotation state, expiry, collisions, Spike
interactions, and recording/playback reconstruction. No simulation or recording
implementation file was changed.

## Local-file and Git safety checks

SHA-256 fingerprints were unchanged for all **118** checked local files:
41 MAME reference frames, 49 `UserData` files, 14 gameplay recordings, and the
14 root screenshots. The root screenshots remain untracked and untouched.
No reference frame, recording, `UserData`, or screenshot was staged or
modified. The prompt and report are ignored by Git as intended.

Final Git state remains on `master` at the expected baseline commit, with only
the intended documentation/source/test edits and new rendering/test files,
plus the same pre-existing untracked screenshots. Nothing was staged,
committed, or pushed.

## Manual acceptance handoff

Copilot did not launch the interactive game. Neal can test with:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\blaster-shot-candidate-20261108.json --level 5 --lives 12
dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\blaster-shot-candidate-20261108.json
```

For playback, Space pauses/resumes, Right steps forward, and Left reconstructs
one boundary backward while paused. A fresh short recording is recommended to
isolate shots; close the recording window normally to save it.

Manual comparison checklist:

1. Fire isolated shots: confirm a compact two-color point picture, without the
   old white rotating star or visible connecting segments.
2. Hold Fire: confirm separated shot pictures, not a renderer-generated trail.
3. Compare warm colors with MAME; report either provisional color adjustment
   needed without treating display/phosphor effects as literal RGB evidence.
4. Inspect near, middle, and far shots for smooth shrinkage and recognizable
   shape; report whether the retained 4.5 scale is too large or small.
5. Fire while changing lanes; confirm screen-oriented, non-rotating geometry.
6. Pause and step forward/backward; confirm static shape and deterministic
   reconstructed positions.
7. Destroy an enemy; confirm the shot disappears and the white type-0 effect
   remains separate.
8. Confirm firing cadence/count, collision, score, sound, Spike erosion,
   death, exit, and progression feel unchanged.

RGB selection, apparent point/sparkle size, and arcade/MAME visual acceptance
remain pending Neal's manual review. No further color/size tuning or broader
renderer work was performed.
