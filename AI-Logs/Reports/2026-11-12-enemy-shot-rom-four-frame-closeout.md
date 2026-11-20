# ROM-backed four-frame enemy-shot rendering closeout

Date: 2026-11-12
Disposition: **CLOSED — MANUALLY ACCEPTED, COMMITTED, AND PUSHED**

## Starting Git state

- Branch: `master`
- HEAD: `cd678572c009d27fa8e1e1f0059ec45f6db5db11`
  (`Complete accepted Level 9 behavior`)
- `origin/master`: same commit
- Staged changes: none
- Expected candidate worktree changes: `TEMPEST-DEVELOPMENT-NOTES.md`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs`,
  `src/Tempest.Game/TempestGame.cs`, seven focused test files, and the new
  untracked `src/Tempest.Game/Rendering/EnemyShotVectorProgram.cs`.
- No unexpected non-ignored untracked files were present.

The candidate changes matched the task's expected state. The diff was
presentation and rendering-test work: the four-frame vector program, renderer
and deterministic shared phase, tests for the ROM geometry and playback phase,
plus updates to existing tests whose old eight-radial-line assumptions no
longer applied. No unrelated gameplay changes were found.

## Relationship to prior reports

- `2026-11-12-enemy-shot-rom-vector-rendering-recovery.md` records the
  read-only investigation and ROM-backed geometry, global modulo-four
  selection, evidence limitations, and candidate translations.
- `2026-11-12-enemy-shot-rom-four-frame-implementation.md` records the
  implementation candidate and its then-current validation status. Its report
  that the final test invocation at that time was blocked remains historically
  accurate and was not rewritten.
- This report records Neal's later manual acceptance and final successful
  user-run test evidence. The paired substantive prompt is
  `AI-Logs/Prompts/2026-11-12-enemy-shot-rom-four-frame-closeout-prompt.md`.

## Manual acceptance and fidelity boundaries

Neal manually tested the candidate and reported that the enemy shots look
pretty good, nothing about them is distracting, and their presentation does
not make the game more difficult to play. This is recorded as manual visual
and playability acceptance of the candidate exactly as tested.

Neal noted the arcade shots may possibly appear somewhat larger, but the
comparison is unclear because the overall arcade game presentation is also
larger on screen. This is not recorded as an established arcade/C# discrepancy
and was not used to change enemy-shot size. Any broader presentation-scale
comparison belongs to the separately deferred whole-game presentation
refinement. No claim is made of pixel-identical or proven exact-size arcade
fidelity. No further enemy-shot size or rendering change is currently
requested.

**ROM-BACKED:** The four distinct pictures, global timer-based/modulo-four
picture selection, four disconnected visible segments and four point-like
marks per picture, non-visible blank beam moves, and verified logical vector
geometry/draw order/asymmetries.

**PROJECT TRANSLATION:** The C# shared deterministic phase advances one
picture per four `RunOneStep` calls; exact arcade real-time animation frequency
remains unresolved. Logical Y-up maps to screen Y-down, current scale and
normalization are project translations, zero-displacement points use
one-pixel rasterization, and the accepted current presentation remains white.
These are not ROM-proven cadence, pixel dimensions, or RGB facts.
`EnemyBullet.Rotation` and projectile gameplay/state remain unchanged.

Enemy-shot creation, speed, trajectory, firing, collision, lifecycle, scoring,
sound, enemy/level behavior, difficulty/timing, gameplay geometry, player
shots, Blaster-shot rendering, and recording/playback schema or input behavior
were not changed.

## Automated and closeout validation

**PASS — Neal-supplied final full test run on the candidate:**

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 2m
```

Neal reported both `Tempest.Game` and `Tempest.Game.Tests` succeeded, xUnit
discovery completed, and all tests ran to completion: 1341 total, 1341
succeeded, 0 failed, 0 skipped. Test duration was 2.9 seconds and overall
command/build duration was 3.4 seconds. The Blame collector warned, "All tests
finished running, Sequence file will not be generated."; this was not a test
failure.

An earlier test attempt could not load
`tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll` because
Windows Application Control reported `An Application Control policy has
blocked this file. (0x800711C7)`. The later user-run final test loaded the
assembly and completed successfully, so the earlier block did not prevent
final validation. No root cause for the different outcomes is claimed, and no
security controls were weakened or bypassed.

**PASS — `git diff --check`** before staging and `git diff --check HEAD^ HEAD`
after commit: no whitespace errors. Git emitted the existing line-ending
normalization warning that `VectorRenderer.cs` has LF line endings that may be
replaced by CRLF on a future Git touch.

No solution build or additional test run was performed during closeout. The
closeout edits were documentation-only; the candidate's final full-suite
validation was already supplied by Neal, and no production code or tests were
changed in this closeout.

No interactive game was launched. No images, screenshots, recordings, videos,
MAME/C# frames, contact sheets, or other visual assets were searched for,
opened, extracted, or analyzed.

## Durable documentation

- `TEMPEST-DEVELOPMENT-NOTES.md` now records the accepted four-picture
  presentation, ROM-BACKED and PROJECT TRANSLATION details, manual
  visual/playability acceptance, unchanged gameplay, final test result, and
  uncertain size observation. The size comparison is deferred to broader
  whole-game presentation refinement, not treated as an established
  discrepancy or reason for retuning.
- `TEMPEST-BEHAVIOR-SPEC.md` now records the same accepted rendering behavior
  with explicit evidence classifications and the pixel/size fidelity limits.

## Commit and push

Only the following 12 project files were staged and committed; no `AI-Logs/`,
`TEMP/`, ROM/reference copies, user assets, recordings, screenshots, videos,
or unrelated local files were staged:

- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Rendering/EnemyShotVectorProgram.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/GameplayPlaybackSessionTests.cs`
- `tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs`
- `tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs`
- `tests/Tempest.Game.Tests/Level2SquareTests.cs`
- `tests/Tempest.Game.Tests/Level3TestModeTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/VisiblePlaybackControllerTests.cs`

Commit:

- Hash: `385f603a94391756b2ca6cccd524dc17b281e782`
- Message: `Close out ROM-backed enemy-shot rendering`
- Push: **PASS** — `master` pushed to configured `origin`; remote reported
  `cd67857..385f603 master -> master`.

Final Git verification:

- Branch: `master`
- Local HEAD: `385f603a94391756b2ca6cccd524dc17b281e782`
- `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Tracked worktree and index: clean
- Non-ignored untracked files: none

## Remaining non-blocking items

- Exact real-time arcade enemy-shot animation frequency and its mapping to the
  C# simulation phase remain unresolved.
- A broader game-screen/presentation-scale comparison remains deferred. The
  uncertain apparent enemy-shot size observation is not a request or
  authorization to retune this accepted rendering.
- Other existing project roadmap items are outside this closeout.

The ROM-backed four-frame enemy-shot rendering milestone is **CLOSED**. No
further enemy-shot size or rendering change is currently requested.
