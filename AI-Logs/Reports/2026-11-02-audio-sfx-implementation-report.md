# Audio SFX implementation

## Final acceptance and freeze (2026-11-03)

This section supersedes the pending-acceptance statements in the historical
implementation/experiment record below. The accepted implementation was
preserved exactly during finalization; only documentation was updated.

**ARCADE VERIFIED / MANUALLY ACCEPTED:** Neal performed direct listening
comparison against original Atari Tempest behavior and Mark Incitti's
implementation. Isolated Blaster shot, sustained rapid fire, and the final
uninterrupted shot after releasing Fire all passed. Neal reports the Blaster
shot now sounds exactly like the arcade and slightly better than Incitti's
already-nearly-perfect version. Other implemented effects and sound timing
remain correct. No gameplay regression was observed. These are Neal's supplied
manual results, not automated-test claims.

**REFERENCE VERIFIED:** Incitti's WAV mappings and playback calls, especially
`PlaySound(shotsfx, schan)` at tempest.bmx:650 and the persistent AllocChannel
at line 983. Classic FreeAudio replaces/stops prior playback and starts from zero.

**PROJECT DECISION:** C# AudioSystem architecture; silent/recording hardware-free
event seam; bounded non-PlayerShot overlap; resource ownership, output-copy,
and application-base-relative loading strategy.

### Frozen PlayerShot behavior

- One persistent loaded Raylib Sound voice; no PlayerShot aliases or overlap pool.
- Each accepted shot stops that voice if currently playing, then restarts the
  same voice from sample zero.
- Authoritative shot.wav unchanged; no trimming, sample modification,
  substitution, pitch/rate/volume adjustment, or firing-cadence change.
- Shot creation, maximum active shots, gameplay input, and all other audio
  behavior unchanged. Do not further tune Blaster-shot playback.
- The full WAV can still finish after an isolated tap or Fire release; Neal
  tested both and accepted them. The earlier isolated-playback concern is
  resolved by that manual evidence, not by an invented fixed-duration cutoff.

### Accepted mappings

| Semantic event | Required root sfx asset | Accepted trigger |
|---|---|---|
| PlayerMove | tick.wav | Actual lane change, at most once per update |
| PlayerShot | shot.wav | Successfully created player projectile |
| EnemyBullet | bullet.wav | Created enemy bullet |
| SpikeHit | spikeshot.wav | Successful Spike shortening |
| PulsarPulse | pulse.wav | Shared pulse-window entry with active Pulsar |
| ZapResolution | zap.wav | Eligible enemy's delayed zap resolution |
| PlayerKilledByBullet | killedbybullet.wav | Bullet-caused Blaster death |
| PlayerKilledByEnemy | killedbyflipper.wav | Flipper/Spike/Pulsar/Fuseball contact death |
| EnemyDestroyed | flippershot.wav | Player-shot destruction of Flipper/Pulsar/Fuseball/Tanker/Spiker |
| ZoomOut | zoomout.wav | Accepted ExitZoom entry |
| ZoomIn | zoomin.wav | LevelApproach entry |
| BonusLife (deferred, not playback-accepted) | bonus.wav | Awaits actual extra-life awarding |

BonusLife has semantic support and bonus.wav remains loaded required content
intended for Git tracking. No score-threshold extra-life award exists in the
current game. Playback awaits that actual gameplay milestone; no extra-life
feature was added merely to exercise the WAV.

### Final validation

Final requested validation executed successfully:

- `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter FullyQualifiedName~AudioEventTests`:
  **11 executed, 11 passed, 0 failed, 0 skipped**.
- `dotnet test TempestCSharp.slnx`: **652 executed, 652 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **PASS, 0 warnings, 0 errors**.
- `git diff --check`: **PASS**.
- All 12 source WAV hashes unchanged from finalization baseline; all 12
  application build-output copies exist and match their respective sources.
- All 12 WAVs are not ignored and remain intended untracked Git additions.
- All 62 non-generated files under src/ and tests/ match their pre-finalization
  hashes: no implementation or test changes during finalization.
- Complete diff against a666ca2 reviewed, including new source/tests and the
  binary asset inventory: **audio milestone only, no unrelated changes**.
- TEMP untouched; recursive entry metadata fingerprint unchanged:
  `0486354931E5CD165BEDA383184F3FB263EDD8B4B03C65876430879EFAD1C7B8`.
- AI-Logs remains ignored. Index empty. HEAD remains
  `a666ca273be608d8e393d8bdd2cf8c0a9b2b9c17`; nothing staged or committed.

All twelve WAVs were already untracked when audio work began and remain intended
Git additions. No audio asset was edited by this milestone. Historical initial
source hashes were not captured before the first implementation; finalization
checks compare a fresh pre-finalization baseline with post-validation source
hashes and build-output hashes, not an unavailable pre-milestone manifest.

SHA256 manifest (source and application-output hashes agree):

| Asset | SHA256 |
|---|---|
| bonus.wav | 0687B393BA22871C855FEA7739AE73ED304E45721AB618EF95782D6221C8810A |
| bullet.wav | 4FDFF95DADFE0D0D01699C46105D88714A8B9BEFC186EEA4E3FC479C218A8B3F |
| flippershot.wav | D0F37F6802E5EFC6BE1CA318533CBFA534AF7B21EB3784EF10B5390ECED1EF9C |
| killedbybullet.wav | 68AB4319194D2E31E46CE49272407BF1B2B5A34BEDA210D2A08B36279B6CC0E1 |
| killedbyflipper.wav | 4DF7C93CD581C1BAEFDD93D4A8BC052B377B6CA89D2B3500B0A5094F23475606 |
| pulse.wav | 7E9474741C68B457C76264B0360F9AFBD39D5A7DD35B0A7EF290D8A294AF1D16 |
| shot.wav | 1375E27CE3F9B39C81EE2A65FD22D1BBE9BFE0F76BF8ABC1B1751205FBFEB744 |
| spikeshot.wav | 63D90790A65C1CDF91C9E43418A7AD0F1650FCB160964BB0FAF4E655C6549892 |
| tick.wav | 30F4DB193716AA81AA071E21606089369292377F6863E7B0B0A3F11E29BC11E4 |
| zap.wav | 090BA9CEE4FAD9EB7EA1E599807477534C7AFF6F8A833F590EDF783CADC7FC27 |
| zoomin.wav | 0CEEDF1C67AFAEF4593B4CA121A72BE340406725FB050DB9EC205F3996CFA20C |
| zoomout.wav | 8DB0E2A69A2F3DF3FDEE0AAFF5CC2FED596FC5F3DF03B1B8DEC0B5B18F140791 |

### Complete intended Git scope

Modified tracked files: README.md, TEMPEST-DEVELOPMENT-NOTES.md,
src/Tempest.Game/Program.cs, src/Tempest.Game/Systems/AudioSystem.cs,
src/Tempest.Game/Tempest.Game.csproj, src/Tempest.Game/TempestGame.cs.

New source/test files: src/Tempest.Game/Systems/AudioEvents.cs,
tests/Tempest.Game.Tests/AudioEventTests.cs.

New WAV assets: bonus.wav, bullet.wav, flippershot.wav, killedbybullet.wav,
killedbyflipper.wav, pulse.wav, shot.wav, spikeshot.wav, tick.wav, zap.wav,
zoomin.wav, zoomout.wav, all under repository-root sfx/.

Review against a666ca2 covers infrastructure, event attachment, tests,
documentation, asset configuration, PlayerShot correction, and WAV additions.
AI-Logs is ignored and excluded from the intended Git scope. TEMP is excluded.

Recommended staging command (provided only; NOT executed):

```powershell
git add -- README.md TEMPEST-DEVELOPMENT-NOTES.md src\Tempest.Game\Program.cs src\Tempest.Game\Systems\AudioSystem.cs src\Tempest.Game\Systems\AudioEvents.cs src\Tempest.Game\Tempest.Game.csproj src\Tempest.Game\TempestGame.cs tests\Tempest.Game.Tests\AudioEventTests.cs sfx\bonus.wav sfx\bullet.wav sfx\flippershot.wav sfx\killedbybullet.wav sfx\killedbyflipper.wav sfx\pulse.wav sfx\shot.wav sfx\spikeshot.wav sfx\tick.wav sfx\zap.wav sfx\zoomin.wav sfx\zoomout.wav
```

Recommended commit subject: `Implement accepted arcade sound effects`.
No staging or commit is authorized during finalization.

TEMPEST_AUDIO_SFX_FINALIZED

## Historical implementation and experiment record

## 2026-11-03 player-shot experiment update

The original milestone below is historical. PlayerShot now owns exactly one
persistent loaded Raylib Sound (no aliases), not the four-voice pool described
below. On every accepted event, AudioSystem stops that voice if playing and
calls PlaySound on the same voice, restarting from sample zero. All other
effects retain their existing four-voice idle-first overlap and busy-pool skip.
The semantic seam and existing headless tests are unchanged.

This is a **PROJECT DECISION** based on **REFERENCE VERIFIED** Incitti playback:
`PlaySound(shotsfx, schan)` at `Reference/incitti/tempest.bmx:650`, with schan
allocated once at line 983. Classic FreeAudio stops/replaces the prior channel
output and starts the next playback from zero. See the
[analysis report](2026-11-03-player-shot-audio-discrepancy-analysis-report.md)
for the complete source trace, upstream backend evidence, and waveform timing.

Only AudioSystem playback/allocation and directly related documentation changed
in this experiment. No WAV edits, substitution, pitch/rate/volume changes,
cadence/shot-creation/shot-limit/input changes, or other sound-policy changes.
Existing gameplay/composition/event tests were preserved.

Manual acceptance is **pending**; this is not an arcade-equivalence claim.
An isolated shot and the final shot after release can still play the complete
168.707 ms WAV. Its later motif may remain audible, and isolated playback may
still differ from Incitti. Single-channel restart primarily tests the sustained
fire overlap explanation.

### Experiment validation

Validation completed for this experiment:

- `dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter
  "FullyQualifiedName~AudioEventTests" --no-restore`: **11 passed**.
- `dotnet test TempestCSharp.slnx --no-restore`: **652 passed**.
- `dotnet build TempestCSharp.slnx --no-restore`: **0 warnings, 0 errors**.
- `git diff --check`: **PASS**; complete tracked diff reviewed, including
  pre-existing implementation changes. Untracked semantic seam/tests also read.
- `git status --short --untracked-files=all`: reviewed; existing unstaged
  implementation files and twelve untracked WAVs remain visible.
- shot.wav SHA256 unchanged:
  `1375E27CE3F9B39C81EE2A65FD22D1BBE9BFE0F76BF8ABC1B1751205FBFEB744`.
- Hashes of Program.cs, TempestGame.cs, the project file, AudioEvents.cs, and
  AudioEventTests.cs match their pre-experiment values.
- TEMP was not touched; its recursive entry metadata fingerprint remains
  `0486354931E5CD165BEDA383184F3FB263EDD8B4B03C65876430879EFAD1C7B8`
  (one entry).
- Index empty; HEAD remains `a666ca2 Refine accepted Blaster shot size`.
  Nothing staged or committed. This report remains ignored under AI-Logs.

No new AudioSystem unit-test abstraction was introduced: native allocation and
stop/play calls require an audio device with the current direct Raylib binding.
Source review verifies one PlayerShot voice, no aliases, conditional stop then
same-voice play, and unchanged non-PlayerShot pooling. Existing semantic tests
remain hardware-free; they do not prove native playback or perceptual parity.

### Manual comparison checklist

Normal launch from the repository root:

```powershell
dotnet run --project src\Tempest.Game
```

All listening results are **not yet recorded**:

- **A - Isolated tap:** allow the complete single shot to finish, compare C#
  with Incitti, and record whether a difference remains.
- **B - Sustained rapid firing:** compare with Incitti's single-shot character
  and record whether the previous multiple-shot/overlap effect is gone.
- **C - Release:** after sustained fire, let the last shot finish and record
  whether its later motif returns on that uninterrupted playback.
- **D - Regression:** confirm unchanged cadence, projectile count, controls,
  collisions, and all other sounds.

TEMPEST_PLAYER_SHOT_SINGLE_CHANNEL_EXPERIMENT_COMPLETE

## Checkpoint and scope

- Starting checkpoint: `a666ca2 Refine accepted Blaster shot size`.
- The 12 root `sfx/*.wav` files were already present and untracked. They were
  not edited, renamed, transformed, staged, or moved.
- Implemented audio ownership, output asset linking, and semantic gameplay
  events. No gameplay rules or simulation ordering were changed.

## Implementation

- `AudioSystem` loads the 12 WAVs once from `AppContext.BaseDirectory/sfx`,
  owns the Raylib sounds, and unloads aliases and source sounds before the
  audio device is closed.
- Each clip has four alias playback voices sharing its loaded sample. A sound
  event uses a free voice; if all four are busy, it is skipped instead of
  restarting an active sound. This overlap policy is a **PROJECT DECISION**.
- `TempestGame` accepts an `IAudioEvents` seam and defaults to
  `SilentAudioEvents`. Tests inject a recording sink; they do not initialize
  Raylib audio or load WAV assets.
- `Tempest.Game.csproj` links root `sfx/*.wav` into build output `sfx/`.
  Runtime asset lookup is independent of the shell working directory.

## Event mappings and evidence

The sound mappings below are **REFERENCE VERIFIED** against Incitti's
`sfx.bmx` and `tempest.bmx`. C# event timing and the bounded playback pool are
**PROJECT DECISIONS** where they adapt to the existing simulation model.
Nothing in this milestone is **ARCADE VERIFIED**; manual audio acceptance
remains pending.

- `tick.wav`: actual Blaster lane change, at most once per simulation update.
- `shot.wav`: player shot creation only; blocked firing is silent.
- `bullet.wav`: enemy bullet creation only.
- `spikeshot.wav`: successful Spike shortening.
- `pulse.wav`: start of the shared pulse window while a Pulsar is active.
- `zap.wav`: each eligible enemy's delayed zap resolution; canceled or
  pre-empted resolutions are silent.
- `killedbybullet.wav`: Blaster killed by an enemy bullet.
- `killedbyflipper.wav`: Blaster killed by a Flipper, Spike, Pulsar, or
  Fuseball.
- `flippershot.wav`: player-shot destruction of Flipper, Pulsar, Fuseball,
  Tanker, or Spiker.
- `zoomout.wav`: accepted transition into ExitZoom.
- `zoomin.wav`: entry into LevelApproach.
- `bonus.wav`: loaded, but no event is emitted. The current C# game has no
  score-threshold extra-life award, so this awaits that future gameplay event.

## Changed and added files

- Updated `README.md`, `TEMPEST-DEVELOPMENT-NOTES.md`,
  `src/Tempest.Game/Program.cs`, `src/Tempest.Game/Tempest.Game.csproj`,
  `src/Tempest.Game/TempestGame.cs`, and
  `src/Tempest.Game/Systems/AudioSystem.cs`.
- Added `src/Tempest.Game/Systems/AudioEvents.cs` and
  `tests/Tempest.Game.Tests/AudioEventTests.cs`.
- Added the requested prompt at
  `AI-Logs/Prompts/2026-11-02-audio-sfx-implementation-prompt.md`.
- The 12 authoritative WAVs remain untracked and visible as intended additions.
  `AI-Logs/` remains ignored. `TEMP/` was not modified.

## Automated validation

- Focused audio tests: **PASS**, 11 passed, 0 failed.
- Full suite, `dotnet test TempestCSharp.slnx`: **PASS**, 652 passed, 0 failed.
- Build, `dotnet build TempestCSharp.slnx`: **PASS**, 0 warnings, 0 errors.
- `git diff --check`: **PASS**.
- All 12 WAVs copied to `src/Tempest.Game/bin/Debug/net9.0/sfx/`.
- All 12 WAVs are not ignored; `AI-Logs/` is ignored.
- No stage or commit was made.

## Manual acceptance

Run from the repository root:

```powershell
dotnet run --project src/Tempest.Game
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --lives 10
dotnet run --project src/Tempest.Game -- --level4-test --lives 10
```

Check spinner lane ticks, player firing and blocked firing, enemy bullet
creation, ordinary enemy destruction, Spike shortening, Pulsar pulse, staggered
Super Zapper resolutions, bullet-caused and enemy/contact-caused death,
ExitZoom and LevelApproach sounds, absence of restart loops, acceptable
overlap/clipping, and unchanged gameplay. `bonus.wav` cannot currently be
exercised because extra-life awarding is not implemented. Manual playback and
arcade comparison have not been performed in this implementation session.
