# Level 7 Direct-Atari Wireframe Prototype

## Outcome

**READY FOR LEVEL 7 DIRECT-ATARI WIREFRAME CHECK**

The opt-in record-06 wireframe path, integer projection, vector list, and
shared provisional viewport map are implemented. Required focused and full
automated validation passed. This is ready for Neal's bounded visual check;
no visual or gameplay-fidelity acceptance is claimed.

## Starting repository state

- Branch: `master`
- HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: empty
- Tracked worktree modifications: 16 pre-existing files:
  `src/Tempest.Game/Debug/DebugController.cs`,
  `src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs`,
  `src/Tempest.Game/Levels/LevelLoader.cs`,
  `src/Tempest.Game/Program.cs`,
  `src/Tempest.Game/Rendering/DisplayConfiguration.cs`,
  `src/Tempest.Game/Rendering/Projection.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/Systems/InputSystem.cs`,
  `tests/Tempest.Game.Tests/DebugControllerTests.cs`,
  `DisplayConfigurationTests.cs`, `GameOverDensityDiagnosticTests.cs`,
  `GameplayRecordingTests.cs`, `InputSystemTests.cs`,
  `Level9ProductionTests.cs`, `PlaybackScreenshotCaptureTests.cs`, and
  `TransitionPresentationTests.cs`.
- Nonignored untracked paths: 8 pre-existing paths:
  `Reference/arcade/MBUCOD.V05.txt`, `Reference/arcade/MBUDOC.DOC`,
  `screenshot000.png`,
  `src/Tempest.Game/Levels/RomPresentationMetadata.cs`,
  `src/Tempest.Game/Rendering/AtariPresentationAdapter.cs`, and tests
  `AtariPresentationAdapterTests.cs`, `RomPresentationMetadataTests.cs`, and
  `VectorRendererAtariPresentationTests.cs`.
- The existing screenshot and candidate work were preserved. The screenshot
  was not opened.

## Atari record implementation

Added [AtariTubeRecordCatalog.cs](../../src/Tempest.Game/Levels/AtariTubeRecordCatalog.cs)
as a separate immutable record model and single-record catalog. Record index
06 uses the ROM-backed Rev 2 K1 point row recovered in
[the local projection-data investigation](./2026-11-15-atari-projection-data-mathbox-investigation-report.md);
scale, camera-height, signed Y2D, and open status are sourced from the existing
[RomPresentationMetadataCatalog](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs)
and local table-extraction handoff.

```text
X: EA E0 9C 80 64 20 16 50 16 20 64 80 9C E0 EA B0
Y: A0 E0 EA B0 EA E0 A0 80 60 20 16 50 16 20 60 80
scale: 0x18 (24)
Y3D: 0x68 (104)
signed Y2D: 0xFF70 (-144)
open: 0x00 (closed)
point count: 16
```

The catalog verifies Level 7 selects table row 06. Point sequences are copied
into read-only collections; malformed point counts, scale, open state, and
record index fail explicitly. The arrays are not derived from or connected to
the C# clover. No raw ROM/PROM copy was opened during implementation.

## Integer coordinate projector

Added [AtariCoordinateProjector.cs](../../src/Tempest.Game/Rendering/AtariCoordinateProjector.cs).
For byte-valued raw coordinates and explicit one-byte runtime position `p`,
the implementation uses integer truncation:

```text
dx = abs(rawX - 128)
dy = abs(rawY - lev_y3d)
divisor = p + lev_scale
qx = floor((dx * 256) / divisor)
qy = floor((dy * 256) / divisor)
```

It restores each source-axis sign, adds explicit signed X/Y bases, saturates
the results to signed 16-bit, maps source-Y to DVG X and source-X to DVG Y,
then drops the upper three bits and sign-extends the remaining 13-bit pattern.
`Project` rejects runtime positions outside `0..255` and rejects any quotient
count state other than low byte `$0f` with a zero high byte. That high-byte
value is an explicit prototype assumption, not an unconditional claim about
retained Math Box hardware state.

`CalculateControlledQuotient` supports the external trace's `p=283` arithmetic
tuple as a standalone controlled-input vector; it does not treat 283 as a
valid Atari runtime-position state. The record projector still enforces the
one-byte runtime limit.

## Objective vectors

The focused tests verify all five external quotient cases exactly:

| Magnitude | p | scale | Quotient |
|---:|---:|---:|---:|
| 0 | 13 | 13 | 0 |
| 1 | 13 | 13 | 9 |
| 64 | 32 | 32 | 256 |
| 255 | 13 | 13 | 2510 |
| 64 | 283 | 283 | 28 |

Record-06 point 0 `(EA,A0)`, X base 0, Y base `-144`:

| p | Divisor | qx | qy | Signed source `(X,Y)` | DVG `(X,Y)` |
|---:|---:|---:|---:|---:|---:|
| 16 | 40 | 678 | 358 | `(678,214)` | `(214,678)` |
| 240 | 264 | 102 | 54 | `(102,-90)` | `(-90,102)` |

The conditional record-00 vector also passes: `p=80`, scale 24, raw point
`(240,128)`, Y3D 80, Y base `-192` gives divisor 104, quotients `(275,118)`,
signed source `(275,-74)`, and DVG `(-74,275)`.

Additional tests cover floor rather than round, both signs around each source
axis, base composition, signed-16 saturation, DVG axis ordering, signed-13
packing boundaries, invalid divisors/runtime state, and the explicit
quotient-count assumption.

## Tube vector list

Added [AtariTubeVectorListBuilder.cs](../../src/Tempest.Game/Rendering/AtariTubeVectorListBuilder.cs).
It projects the immutable row at normal near `p=16` and far `p=240`, preserves
record order, emits 16 near points, 16 far points, 16 rails, and 16 edges for
each closed rim, including the final point-to-first-point edge. It retains
canonical DVG center word `$8040` (logical origin `(0,0)`) and vector-scale
word `$7100`. Repeated builds are deterministic. It accepts no gameplay
service or entity and reads no `LevelDefinition.Points`.

## Global viewport map

Added [AtariDvgViewportMap.cs](../../src/Tempest.Game/Rendering/AtariDvgViewportMap.cs)
as one shared map with axis permutation/sign, rotation, translation, scale,
and display aspect. Current development-only provisional values are:

- Translation: `(300,400)`, the center of the existing 600x800 portrait logical
  area.
- Axis permutation: none.
- Signs: DVG X positive to logical right; DVG Y positive to logical up
  (negative screen Y).
- Rotation: 0 degrees.
- Global scale: `0.4` logical units per canonical DVG unit.
- Global display aspect: `1.0`.

Atari sources establish named X/Y axes and adjustable X/Y center and size, but
do not establish physical signs, cabinet rotation, gain, or aspect. Thus the
chosen signs, rotation, global scale, and aspect are **PROJECT TRANSLATION /
APPROXIMATION**, not Atari monitor calibration. The constants are shared and
contain no Level-7 placement, size, point, lane, or gameplay tuning. Tests
verify finite deterministic mapping and that all record-06 near/far points
fall within the portrait logical bounds. This geometric test is not a
recognizability or visual-fidelity oracle.

## Renderer isolation

`--atari-tube-prototype` is parsed and validated separately from
`--atari-presentation`; it requires an explicit ordinary `--level 7` start,
rejects legacy/perspective/Atari-presentation conflicts, headless analysis,
input diagnostics, scenario/calibration modes, and recording/playback.
It is disabled by default, is not added to recording metadata, and prints an
`EXPERIMENTAL` diagnostic when enabled.

The renderer dispatches the direct vector list only when the option is active,
the displayed level is 7, and the game state is ordinary `Playing`. It draws
only the blue near rim, far rim, and rails through the provisional map, using
the existing portrait logical HUD camera. Gameplay objects are hidden. HUD
and text remain on the existing overlay path. Approach, exit, and any other
unsupported non-Game-Over state show an explicit development-only unavailable
message instead of a legacy-projected tube. Game Over render states bypass the
prototype and continue through the existing renderer path.

The prototype does not call
[AtariPresentationAdapter](../../src/Tempest.Game/Rendering/AtariPresentationAdapter.cs),
[Projection.Project](../../src/Tempest.Game/Rendering/Projection.cs#L40),
or the C# tube-point renderer. No screenshot or MAME fitting was performed.

## Gameplay/model protection

No gameplay system, Level 7 model point, point order, lane, topology, movement,
transition, entity, collision, timer, score, life, RNG, recording format, or
playback state was changed. The builder and projector operate only on immutable
record data and explicit scalar inputs. The renderer reads current level/state
only to select the isolated presentation or its unsupported-state diagnostic;
it does not pass gameplay entities or services into the prototype.

No optional lane/depth marker bridge was implemented.

## Unsupported states

The prototype omits Approach, tube exit, below-rim Eggs, out-of-byte runtime
positions, reverse transition zoom, gameplay entities, and Levels 8-9.
Unsupported runtime presentation states show a diagnostic without changing
simulation. The prototype makes no recorded-playback presentation-equivalence
claim.

## Accepted Levels 1-6 protection

All new renderer behavior is gated by the separate prototype flag. Option-off
and existing `--atari-presentation` behavior remain on their prior paths.
Tests compare default and explicitly disabled rendering, verify the existing
Atari presentation candidate remains on its path, and verify Levels 1-6 and
8-9 do not select record 06. No pre-existing candidate or protected gameplay
geometry was rewritten.

## Tests and validation

All commands completed; VSTest hang-timeout protection was used.

Focused Atari record/projector/vector-list/viewport/option/renderer tests:

```powershell
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariTubeRecordPrototypeTests" --blame-hang-timeout 3m --verbosity minimal
```

**PASS** — 33 passed, 0 failed.

Presentation, Level 1-9, Approach/transition, playback/recording, Game Over,
CLI, input, and display regressions:

```powershell
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariTubeRecordPrototypeTests|FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level1|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~Level6|FullyQualifiedName~Level7|FullyQualifiedName~Level8|FullyQualifiedName~Level9|FullyQualifiedName~LevelApproachTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~GameplayPlayback|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~PlaybackScreenshotCaptureTests|FullyQualifiedName~VisiblePlaybackControllerTests|FullyQualifiedName~GameOver|FullyQualifiedName~DebugControllerTests|FullyQualifiedName~DisplayConfigurationTests|FullyQualifiedName~InputSystemTests" --blame-hang-timeout 5m --verbosity minimal
```

**PASS** — 939 passed, 0 failed.

Full solution tests:

```powershell
dotnet test .\TempestCSharp.slnx --no-restore --blame-hang-timeout 5m --verbosity minimal
```

**PASS** — 1,538 passed, 0 failed.

Release solution build:

```powershell
dotnet build .\TempestCSharp.slnx --configuration Release --no-restore --verbosity minimal
```

**PASS** — 0 warnings, 0 errors.

`git diff --check`: **PASS**; Git reported only existing LF-to-CRLF
working-copy advisories for `LevelLoader.cs` and `InputSystem.cs`.

An earlier focused iteration passed 31 tests, the expanded final focused test
passed 33, and an earlier regression filter passed 903 tests before adding
the final input-system selector. The final counts above are from the final
code. No test was weakened. No interactive game was launched.

## Deviations

- No optional lane/depth marker overlay was added; the requested milestone is
  wireframe-first.
- Physical viewport orientation and calibration remain provisional as
  permitted by the controlling evidence; Neal's visual review is still
  required.

## Limitations

This is not exact physical cabinet output, bit-exact retained Math Box state,
an exact C# lane identity, an exact C# depth conversion, a complete entity
registration, or acceptance of Levels 8-9. The quotient-count high-byte
assumption and global display map remain explicit qualifications.

## Manual command

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype

## First manual questions

1. Does the direct Atari record-06 wireframe appear as a recognizable Level 7 tube?
2. Does its perspective look plausibly like Atari Tempest?
3. Is the tube fully contained and usable in the portrait viewport?
4. Does the result support continuing with lane/depth registration work?

## Final repository state

- Branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, upstream
  `origin/master`, ahead/behind `0/0`.
- The same 16 tracked paths remain modified; this work only extended the
  already-modified `DebugController.cs`, `Program.cs`, and `VectorRenderer.cs`.
  Existing modifications in `LevelLoader.cs`, `DisplayConfiguration.cs`,
  `Projection.cs`, and the other candidate files were preserved.
- The 8 starting untracked paths remain; the 5 new source/test paths are the
  only additional nonignored untracked paths.
- Index remains empty. The prompt and this report are ignored by Git.
- Nothing was staged, committed, pushed, reset, restored, stashed, rebased,
  branch-switched, or cleaned.
