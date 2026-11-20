# Common type-0 Blaster-hit destruction effect

## Starting state

The implementation began at `4604297920f90c96731abd727ca3c73660300e88`
(`master`). The tracked worktree was clean, the index had no staged changes,
and `screenshot000.png` through `screenshot013.png` were already untracked.
Those screenshots remain untouched and unstaged.

The complete user task is preserved in
`AI-Logs/Prompts/2026-11-08-common-type0-destruction-implementation-prompt.md`.
The prior investigation report was read and corrected where it misidentified
the type-0 vector program.

## ROM evidence and correction

The local annotated listing traces player-shot scans at `$a454`; enemy-shot
removal at `$a36f`; ordinary enemy hits at `$a38e`; the shared eight-record
allocator at `$a3d4/$a3d6`; age advancement at `$a416`; and drawing in
`draw_explosions` at `$b79a`. The allocator scans free slots from 7 down to 0.
When full, it replaces the greatest-age entry; the descending scan and
equal-age replacement resolve ties to the lowest slot index.

**Important correction to the earlier investigation report and the supplied
ROM-listing findings:** `$b7c3-$b7c9` uses `(age >> 1) & $fe` for types below
2, and type 0's offset in `$b7e5` is `$00`. At ages 0, 3, 6, and 9 this
selects graphic-table byte offsets 0, 0, 2, and 4: size-1, size-1, size-2,
and size-3. The corresponding graphic-table entries point to `$14c2`,
`$14f8`, and `$1522`; their vector programs are listed at `$34c2-$34f6`,
`$34f8-$3520`, and `$3522-$3558`. Each sets `vstat` color index 0. The
nine-entry `$1b18`…`$1b04` sequence and shared `$1b1a` color changes are not
the common type-0 path.

The user-reported MAME appearance is multicolor, but those exact frames were
not available to inspect. The code follows the local type-0 listing and maps
color index 0 to existing `Color.White` only as a C# project translation; it
does not claim an Atari RGB match or resolve the visual discrepancy.

## Implementation

- Added a fixed eight-slot, simulation-owned pool. It captures the final lane
  and height at each confirmed supported hit, uses the ROM's descending free
  slot selection and oldest-age replacement, and preserves deterministic
  lowest-index ties.
- The records are not enemies or projectiles and do not enter collision,
  score/audio, enemy counts, ledgers, cargo identity, or level completion.
- The pool advances at the beginning of each simulation step. A newly created
  age-zero record is therefore visible immediately; ages 0 and 3 both select
  size 1, followed by sizes 2 and 3 at ages 6 and 9, then inactive at 12.
  This adds an initial visible C# boundary compared with the ROM's
  post-hit-update order, without changing completion timing. Rendering does
  not advance or mutate the effect.
- The renderer translates the three ROM vector programs using the existing
  perspective projection and remains active during exit presentation. Effects
  naturally pause when no simulation steps run and replay deterministically;
  no recording or screenshot format changes were made.
- Confirmed normal Blaster-hit support: ordinary Flippers, Tankers,
  materialized Tanker-cargo Flippers, Spiker bodies, Pulsars, and enemy
  projectiles. Spike erosion, Fuseball/Fuzzball special effects, Super Zapper,
  player death, and Game Over remain excluded.

## Files changed

Tracked-file modifications and new untracked source/test files:

- `src/Tempest.Game/Gameplay/EnemyDestructionEffectPool.cs` (new)
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/EnemyDestructionEffectTests.cs` (new)
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`

Ignored project records:

- `AI-Logs/Prompts/2026-11-08-common-type0-destruction-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-08-common-type0-destruction-implementation-report.md`
- `AI-Logs/Reports/2026-11-07-enemy-destruction-rom-investigation-report.md`
  was corrected to remove the erroneous type-0/multicolor interpretation.

## Validation

- Focused destruction-effect tests: **12 passed, 0 failed**.
- Expanded gameplay/rendering/audio/ledger/completion/playback regression
  filter: **545 passed, 0 failed**.
- Full solution tests: **986 passed, 0 failed, 0 skipped**.
- Full solution build: **0 warnings, 0 errors**.
- `git diff --check`: passed.
- No interactive game was launched.

## Implementation-stage state before manual acceptance

This section preserves the state when the original implementation prompt
finished. Neal's subsequent acceptance and the completed closeout are recorded
after this historical snapshot.

All changes remain unstaged; no commit or push was made. The original
fourteen screenshots remain untracked and untouched. No gameplay recordings
or `UserData` files were changed. The AI prompt/report files are ignored.

Neal's manual launch command:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

Manual checklist:

1. Destroy an ordinary Flipper and compare the size-1/size-2/size-3,
   single-ROM-color candidate with the MAME sequence. The local ROM type-0
   listing does not support the supplied multicolor interpretation, so record
   that discrepancy rather than assuming a match.
2. Destroy an enemy projectile, Tanker, materialized cargo Flipper, Spiker
   body, and Pulsar; confirm the effect stays at the struck object's captured
   lane/depth and Tanker cargo behaves as before.
3. Shoot a Spike and confirm no common effect. Confirm Fuseball receives no
   guessed type-0 effect.
4. Kill the final enemy; confirm the effect appears during the existing exit
   delay and does not delay progression.
5. Pause, step forward, reverse-scrub, and capture a screenshot around an
   effect. Check scoring, sound, death/re-entry, and progression for regressions.

Exact MAME visual equivalence and the reported multicolor appearance remain
**UNKNOWN / UNRESOLVED** pending Neal's comparison. No arcade acceptance is
claimed at this implementation-stage checkpoint.

## Subsequent manual acceptance and closeout (2026-11-08)

Neal subsequently play-tested the common type-0 effect against the arcade
presentation and accepted it as identical. He confirmed the geometry/drawing
and timing, pausing recorded gameplay and stepping forward and backward to
inspect each phase. This establishes the resulting C# drawing and timing as
matching the arcade presentation to Neal's satisfaction; it does not convert
the C# color or perspective translations into literal ROM facts.

The durable behavior and development records now classify ROM-backed
mechanics/data, project translations, and manual arcade-fidelity acceptance
separately. The common type-0 effect is closed and frozen unless a future
regression or new arcade/ROM evidence warrants revisiting it. Fuseball/Fuzzball
special destruction remains deferred.

Closeout validation passed: focused destruction-effect tests **12/12**;
expanded gameplay/rendering/audio/ledger/completion/recording/playback filter
**679/679**; full solution tests **986/986**; full solution build **0 warnings,
0 errors**; staged and unstaged `git diff --check` passed. The interactive game
was not launched.

Commit `d2d8b1ef69b4a13be06ab59b41a1c9d38a8dd6e2`
(`Close out accepted common type-0 destruction effect`) was pushed to `origin`
`master`. Local HEAD, local `origin/master`, and live remote `master` all
verified at that commit. The tracked worktree and index are clean; only the
original `screenshot000.png` through `screenshot013.png` remain untracked and
unstaged. See
`AI-Logs/Reports/2026-11-08-common-type0-destruction-closeout-report.md` for
the complete closeout record.
