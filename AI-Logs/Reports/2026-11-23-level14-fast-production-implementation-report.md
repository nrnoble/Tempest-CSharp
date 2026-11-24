# Level 14 Fast Production Implementation Report

## Status

**Implementation complete; automated validation PASS; manual acceptance pending.**
The interactive game was not launched. No staging, commit, push, reset, restore,
stash, clean, or rebase was performed.

## Scope and baseline

Implemented displayed Level 14 as the next production level, using the
established Level 14 -> shape row 15 mapping. Production progression now
continues `9 -> 10 -> 11 -> 12 -> 13 -> 14`; Level 14 is terminal, and direct
Level 15 remains separate.

The accepted Level 13 baseline was `f2ef17211803e852809829ec0e23bcd4a26b4d4a`
(`Complete accepted Level 13`). Local Git state was authoritative; the
repository was on `master`, tracking `origin/master`, with no ahead/behind
commits at the verified starting point. GitHub retrieval confirmed the
baseline. The pre-existing Level 13 closeout-report modification was preserved.
The unrelated untracked `Reference/arcade/MBUCOD.V05.txt` and `temo/` were
preserved and not inspected. No ROM material was copied or modified.

## Evidence and implementation

Tracked disassembly and project evidence were used first. Bounded read-only
verification accessed only the authorized
`Reference/arcade/roms.rev2/136002-118.k1` row-15 geometry and associated
metadata. No ROM directory enumeration or unrelated ROM-region inspection was
performed; `Reference/arcade/MBUCOD.V05.txt` was not used.

ROM/DISASSEMBLY-BACKED row-15 evidence, in source point order:

- X: `10 1E 21 28 3C 55 66 73 8D 9A AB C4 D8 DF E2 F0`
- Y: `7E 6A 51 3A 2C 2C 38 4E 4E 38 2C 2C 3A 51 6A 7E`
- Angles: `0E 0C 0D 0E 00 02 02 00 0E 0E 00 02 03 04 02 00`
- Metadata: remap `0F`, scale `0C`, camera `B0`, signed Y placement `+320`
  (`40/01`), open flag `FF`, Flipper-scale bytes `50/04` (translated value
  `0250`).

The open topology produces 15 playable lanes with no wraparound and uses the
accepted open-end reversal/pursuit behavior. Centering, `25/14` scaling, Y
reflection, model frame `(25/14, -1)`, and geometry-derived spinner sign are
PROJECT TRANSLATIONS.

ROM/DISASSEMBLY-BACKED gameplay evidence:

- Ordinary/hard supply `1C`/`20`; active/physical limits 6/7.
- Raw species min/max: Flippers `0/5`, Tankers `1/2`, Spikers `2/3`,
  Fuseballs `1/1`, Pulsars `0/0`; Pulsar fire-enable `00`.
- The nonzero-minimum production picker admits Tankers, Spikers, and Fuseballs,
  not ordinary Flippers or Pulsars. The existing no-Pulsars-before-Level-17
  boundary remains unchanged.
- Flipper movement program `$07`, Flipper/Tanker speed `A4`, Spiker speed
  `00`, Fuseball movement probability `DC`, shared enemy-shot speed `C0`,
  shot holdoff `29`, and enemy-shot limit 2.
- Tanker cargo bytes `01/01`; the existing generic Tanker lifecycle and
  straight `$07` cargo route are reused.
- Spike start `A0` is an Atari along-coordinate. Its PROJECT TRANSLATION is
  `(F0 - A0) * 400 / 224`, approximately `142.857` C# world-height units;
  the raw value is not used as a C# length.
- Pulsar speed is globally initialized as `FE:A0`, but its Level 14 picker
  gates remain zero; no Pulsar production behavior was enabled.

The generic production ledger, identity conservation, admissions, enemy
lifecycle, bullets, collision, death/re-entry, Super Zapper, score/lives,
completion, deterministic playback, and existing enemy behavior are reused.
Only the Level 14 Spiker firing path received a Level-14-specific capacity
gate, so active shots cannot exceed the recovered two-shot limit; Levels 1–13
behavior is unchanged.

Atari presentation is registered with neutral Level 14 calibration
`(Y gain 1, scale 1, vertical offset 0, depth-Y gain 1)`. Existing generic
Approach, ExitZoom, vector HUD, calibration guides, and depth-Y diagnostic are
retained. Accepted Levels 1–13 calibration and direct Level 15 are unchanged.

## Changed files

Added:

- `src/Tempest.Game/Levels/Level14WaveSelection.cs`
- `tests/Tempest.Game.Tests/Level14ProductionTests.cs`
- `AI-Logs/Prompts/2026-11-23-level14-fast-production-implementation-prompt.md`

Updated production code, existing tests, README, and the behavior/development
records for Level 14. The pre-existing Level 13 report edit was not overwritten.

## Validation

- **PASS** — Focused `Level14ProductionTests`: 10 passed, 0 failed.
- **PASS** — Full Release solution test run:
  `dotnet test TempestCSharp.slnx --configuration Release --no-restore --blame-hang-timeout 3m`
  — 1,938 passed, 0 failed, 0 skipped.
- **PASS** — Release solution build:
  `dotnet build TempestCSharp.slnx --configuration Release --no-restore`
  — 0 warnings, 0 errors.
- **PASS** — `git diff --check` (only existing LF-to-CRLF working-copy warnings).
- **PASS** — Deterministic production soak in the Level 14 production tests:
  bounded 1,200-tick run completed.

Automated validation establishes tested mechanics and determinism, not arcade
fidelity or manual acceptance.

## Manual acceptance required

Neal should review the genuine Level 14 tube/topology, spinner direction,
production enemy mix and routes, absence of Pulsars, Spike starting height,
firing, Super Zapper, death/re-entry, completion, HUD, Level 13 -> 14 carry,
and first-pass Atari Playing presentation. Do not select final calibration
values until that review. The game has not been launched.

Suggested commands:

```powershell
dotnet run --project src\Tempest.Game -- --level 14
dotnet run --project src\Tempest.Game -- --start-level 14
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --calibration-guides
dotnet run --project src\Tempest.Game -- --level 14 --atari-presentation --tube-y-gain 1 --tube-scale 1 --tube-vertical-offset 0 --tube-depth-y-gain 1
dotnet run --project src\Tempest.Game -- --level 14 --lives 7
```
