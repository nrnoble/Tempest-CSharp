# Blaster Outward Orientation and Brightness Diagnostic

## Status

Implementation candidate complete. Automated validation passed. **Blaster
orientation acceptance: PENDING. Final brightness multiplier: PENDING.** No
interactive game was launched, and no files were staged, committed, or pushed.

## Scope and findings

The user-provided manual observation described the Atari-presentation Blaster
body pointing into the tube. The exact authorized image,
`playback-level-1-boundary-000033.png`, was not found in the workspace; no other
image or video was inspected.

`VectorRenderer.DrawPlayer` formed a perpendicular from the projected edge
tangent and always used its left-hand sign. It never determined which side of
that edge faced into the tube. The focused Level 1 geometry test confirms that
this fixed left normal points toward the actual near-to-far rails (the tube
interior) for the starting lane.

The correction is shared and render-only. During Atari-presentation
`Playing`, it uses the projected player edge and the corresponding far-rail
endpoints. The averaged near-to-far rail vector identifies the interior;
the perpendicular is selected on the opposite side. This uses the visible
geometry rather than polygon winding or a closed-polygon centroid, so the same
rule covers open and closed tubes, reflected projections, and reversed player
travel. Level 7 uses its existing Atari presentation near/far points without
changing Level-7-specific architecture. The original anchor computation,
offset magnitude, six line segments, and lane geometry are preserved.

The Blaster brightness option is a rendering-only development diagnostic. The
multiplier applies only to the six active Blaster artwork lines during Atari
presentation `Playing`. Each RGB byte is multiplied and clamped to 255 before
conversion back to a byte; alpha is copied unchanged. `1.0` reproduces the
existing `Color.Yellow` exactly, and `2.0` doubles each channel before
saturation. The multiplier does not enter simulation, recording metadata, or
recorded input.

Unaffected rendering includes the tube and highlighted lane, HUD and life
glyphs, shots, enemies, explosions/effects, Super Zapper, transition stars,
diagnostic overlays, and background. Approach, transition, death, and exit
render states retain their existing Blaster drawing behavior.

## Changed files

- `src/Tempest.Game/Rendering/VectorRenderer.cs` — geometry-derived outward
  normal, color scaling, and callback seam for headless artwork tests.
- `src/Tempest.Game/Debug/DebugController.cs` — invariant-culture option
  parser and visible-playback option handling.
- `src/Tempest.Game/Program.cs` — option validation and renderer wiring.
- `tests/Tempest.Game.Tests/BlasterPresentationTests.cs` — geometry, color,
  topology, and render-state preservation coverage.
- `tests/Tempest.Game.Tests/DebugControllerTests.cs` — parsing, context, and
  recording-metadata coverage.
- `README.md` — development option documentation.

No gameplay, tube geometry, accepted Level 8/9/10 placement, recording format,
or Level-7-specific presentation code was changed.

## Diagnostic syntax and validation

```text
--blaster-brightness <multiplier>
```

The option defaults to `1.0` and requires `--atari-presentation`. It accepts
positive finite invariant-culture `double` values, including `1`, `1.0`,
`1.25`, `1.5`, `1.75`, `2`, and larger finite values. Missing, malformed,
duplicate, NaN, infinite, zero, and negative values are rejected. There is no
arbitrary upper aesthetic bound; final RGB channels saturate safely at 255.
The option is rejected outside the Atari-presentation context and is accepted
with visible recorded playback without becoming recording metadata.

## Validation

- Focused Blaster geometry/CLI tests: **PASS**, 17 passed.
- Targeted Atari presentation, Level 7 shared-renderer, Level 8/9/10,
  camera/display, Blaster/lane, approach/transition, and recording/playback
  regressions: **PASS**, 411 passed.
- Full solution tests using VSTest hang-timeout protection:
  **PASS**, 1,623 passed, 0 failed, 0 skipped
  (`--blame-hang-timeout 5min`).
- Release solution build: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**. Git emitted only its README line-ending
  normalization notice; no whitespace errors were reported.

The first focused test attempt using the repository's normal build output
failed with MSB3027/MSB3021 because
`src\Tempest.Game\bin\Debug\net9.0\Tempest.Game.exe` was locked by an already
running `Tempest.Game` process (PID 113328). The process was not stopped. All
successful focused, targeted, full-suite, and Release validation was rerun
using an isolated `--artifacts-path` under the Copilot session folder.

## Git state

Starting state: branch `master`, HEAD
`bcd79da63021821c4e31a2a646152a70ccc443e5`, tracking `origin/master`,
0 ahead / 0 behind, and a clean index. The pre-existing tracked modification
to `.github/copilot-instructions.md` and untracked local/reference files
`Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`, and
`screenshot000.png` were preserved.

Ending state: same branch, HEAD, upstream, and ahead/behind count; no staged
changes. The pre-existing modification and untracked files remain untouched.
The authorized implementation changes are listed above, with the new test
file untracked. `AI-Logs/` remains ignored; nothing was staged or committed.

## Manual handoff

Run each command from the repository root. Every command starts Level 1 with
`--atari-presentation`, so the outward-orientation correction is active.

```powershell
# Omitted multiplier: current-brightness default control
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation

# Explicit 1.0: current-brightness control
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --blaster-brightness 1.0

# Higher values are visual diagnostic experiments, not selected defaults
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --blaster-brightness 1.25
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --blaster-brightness 1.5
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --blaster-brightness 1.75
dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 1 --atari-presentation --blaster-brightness 2.0
```

Manual judgment remains required for:

- The Blaster body staying outside the rim throughout movement, including
  direction changes.
- The anchor staying on the rim and the highlighted lane staying correct.
- Preferred brightness, color clipping, or unwanted color shift.
- Any gameplay regression.

The existing yellow color can reach channel saturation at multipliers above
1; automated tests establish byte scaling and clipping behavior, not perceived
visual brightness. No final value was selected. Orientation and brightness
acceptance both remain **PENDING**.

## Logs

- Prompt: `AI-Logs/Prompts/2026-11-17-blaster-outward-orientation-and-brightness-prompt.md`
- Report: `AI-Logs/Reports/2026-11-17-blaster-outward-orientation-and-brightness-report.md`
