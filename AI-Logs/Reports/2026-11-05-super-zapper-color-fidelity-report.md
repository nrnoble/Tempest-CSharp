# Super Zapper color fidelity - pending manual acceptance

## 1. Starting checkpoint

- Completion check: exactly one complete standalone completion token found in
  the user task before any tools or repository action.
- Branch: `master`.
- HEAD: `23ceeeef3535505d2096f552f914d74082ec8eae`.
- Subject: `Accept calibrated Flipper rim pursuit`.
- All seven requested baseline commands ran first (in the Tempest directory).
  Tracked worktree/index were clean; no non-ignored untracked files; diff check
  succeeded. Nothing was reset or discarded.

## 2. ROM routines/data inspected

Only [the project-local Atari source](../../Reference/arcade/Tempest%20Rom%20Source%20code.md)
was used as disassembly authority. No Internet, alternate ROM/disassembly,
MAME, other port, or other game was consulted.

| ROM address / symbol | Evidence relevant to this correction |
|---|---|
| `$0003 timectr` | Free-running time-based counter, not a zap countdown. |
| `$0125 zap_running`, `$03aa zap_uses` | Presentation gate and use count. |
| `$a831 reset_sz` | Clears uses and running state. |
| `$a83a check_zap` through `$a882` | Edge-triggered activation, two-use limit; increments running state; uses `zap_length`; clears state on expiry. |
| `$a883 zap_length` | Bytes `00,13,05,00,00`; first/second ROM use lengths differ. |
| `$a888-$a8ac` | Even running states >= 3 process a target; no remaining enemy can clear running state early. Not changed in C#. |
| `$b367-$b3d4` | Builds tube/Pulsar lane state before selecting ordinary/zap rail colors. |
| `$b3d6-$b3e9` | Inactive/non-positive running state selects index 6; active state loads `timectr & 7`, replacing only 7 with 1. |
| `$b414-$b44f` | Pulsar lane handling wins first, occupied Blaster boundaries select 1 next, existing separate transition presentation is considered next, otherwise the zap/base slot from `$29` is used. |
| `$b476-$b485` | Offsets used by `$b44c` to patch the longitudinal vector color instructions. Not a palette RGB table. |
| `$c30d-$c420`, especially `$c33e`, `$c358`, `$c3ee` | Tube construction; longitudinal rail vector instructions are separately patchable; near/far circumferential rim connections remain under index 6. |
| `$b586 draw_player`, `$b5ad draw_enemies` and type dispatch | Player explicitly selects 1; Flipper explicitly selects 3; no zap-wide player/enemy recoloring is established. |
| `$b60f-$b633`, `$b69b-$b6f7`, `$b71b-$b754` | Tanker, Spiker, Fuseball and Pulsar rendering have their own graphic/color choices; Pulsar uses 4/0 according to pulsing. |
| `$b75a draw_shots` through `$b799` | Own shot presentation/color RAM handling, not a zap recoloring path. Preserved. |
| `$b79a draw_explosions` through `$b895` | Separate explosion palette rotations/restoration; not evidence for a zap-wide palette rewrite. Preserved. |
| `$c196 setcolours` through `$c1c2`, `$c1fd-$c225` | Installs per-level colors by splitting packed nibbles into color RAM and shadows; actual palette blocks appear only as `.chunk 8`. |
| `$c8e3` | Increments global `timectr`; confirms the zap color phase does not originate from remaining flash time. |

## 3. Exact ROM-backed behavior

When `zap_running` is positive, the ordinary longitudinal rail color index is:

```
phase = timectr & 7
index = phase == 7 ? 1 : phase
```

In increasing counter order the repeating slots are:
`00,01,02,03,04,05,06,01`. Activation does not restart this global sequence.
The first visible slot depends on the counter phase at activation.

Important register detail: `$b3df` replaces A with the time counter, so the
branch at `$b3e5` preserves slots 0 through 6; it does NOT keep default slot 6
for those phases. An early provisional interpretation was corrected by
checking this register flow before final validation.

This changes rail vector color selections, not the level's whole palette RAM.
Occupied Blaster rails remain slot 1. Pulsar lane selections precede both
occupied-rail and zap selections. Circumferential near/far rim connections
remain slot 6; ordinary player and enemy colors are not globally overridden.
The next inactive update selects slot 6 again without a special zap palette
reload.

Both use counts pass through the same color selection: no use-count branch
exists in `$b3d6-$b3e9`. ROM lifecycle length differs (`$13` versus `$05`) and
may end early; changing that lifecycle is explicitly out of scope.

## 4. Remaining UNKNOWN information

- Exact packed palette bytes for levels 1-16 at `$c1fd` are UNKNOWN in this
  document: `.chunk 8` does not expose their values.
- Exact Atari hardware nibble-to-RGB/CRT output mapping, phosphor response,
  brightness and calibrated RGB values are UNKNOWN.
- Consequently the exact ROM-backed hue of every slot cannot be proved from
  the supplied source alone. Named hues below are PROJECT TRANSLATIONS, not
  recovered palette bytes.
- Hardware wall-clock cadence, display pipeline latency and exact starting
  phase correspondence to a C# frame are not measured here. C# uses its
  existing display-step hook (including death freezes, excluding approach).
  The established C# lifecycle and approach/exit hooks remain unchanged.

## 5. Previous C# mismatch / audit

`SuperZapperSystem.UsesAlternateFlashColor` used remaining flash time with
`(FlashTicksRemaining / 2) % 4 > 1`: a two-color, activation-relative pattern.
`VectorRenderer.DrawTube` applied blue/Tanker-magenta to both longitudinal
rails and circumferential rim connections. Occupied boundaries stayed yellow.

The existing narrow resolver only knows palette-0 Blaster yellow. Player
drawing uses its existing yellow; Flippers red; Spikers/Spikes green; Tankers
magenta; Fuseballs independent arm colors; Pulsars cyan/white; bullets/shots
their independent colors. Pulsar rail overlays are submitted after the tube
and supersede its rail colors. These existing paths were audited and left
unchanged. No new player/Pulsar override outside zap presentation was added.

## 6. Exact implementation

- Add a modulo-eight presentation phase in `SuperZapperSystem`, advanced on
  every existing `AdvanceDisplayStep`, even while inactive. Neither activation
  nor `Reset` restarts it.
- Replace the old boolean with `TubeColorIndex`, exposing 0..6,1 while active
  and 6 while inactive.
- Add one narrow renderer switch translating those seven slots to existing
  project colors. No generalized palette subsystem or additional palette
  groups were introduced.
- Separate ordinary rim color from rail color in `DrawTube`; only unoccupied
  rails receive the zap cycle. Occupied rails retain their existing yellow;
  Pulsar overlays retain precedence and accepted rim suppression.
- Keep all activation, charges, target selection, accepted-target delays,
  countdown decrements, 60-step lifetime, scoring, sound and gameplay logic
  unchanged.

## 7. ROM-backed versus project-translated decisions

| Slot | C# translation | Authority |
|---|---|---|
| 0 | Existing `Color.White` | PROJECT TRANSLATION |
| 1 | Existing palette-0 Blaster `(255,255,0,255)` | PROJECT TRANSLATION; accepted occupied-lane yellow reused |
| 2 | Existing Spiker `(0,255,0,255)` | PROJECT TRANSLATION |
| 3 | Existing `Color.Red` | PROJECT TRANSLATION |
| 4 | Existing Pulsar `(0,255,255,255)` | PROJECT TRANSLATION |
| 5 | Existing Tanker `(255,0,255,255)` | PROJECT TRANSLATION |
| 6 | Existing `Color.Blue` | PROJECT TRANSLATION; accepted normal tube preserved |

`Color.White`, `Color.Red` and `Color.Blue` refer to the unchanged Raylib
constants already used by this project, not newly calibrated RGB triplets.
Slot sequence, active gate, rail-only application and precedence are
ROM-BACKED. Hue/RGB translations and the retained C# 60-step envelope are
PROJECT choices, not ROM-exact color calibration/lifecycle.

Both successful C# presentations use the same rule and existing 60-step
envelope. The second use still starts a fresh presentation only if an eligible
target accepts the zap; a targetless second use does not start a new one.
This intentionally preserves existing gameplay/presentation lifecycle policy.

## 8. Files changed

Production:

- `src/Tempest.Game/Systems/SuperZapperSystem.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`

Tests:

- `tests/Tempest.Game.Tests/SuperZapperTests.cs`
- `tests/Tempest.Game.Tests/BlasterLaneHighlightTests.cs`
- `tests/Tempest.Game.Tests/Level2SquareTests.cs`
- `tests/Tempest.Game.Tests/Level3GeometryTests.cs`

One permitted stale-fact correction only:

- `TEMPEST-DEVELOPMENT-NOTES.md`: replace the misleading claim that the old
  blue/magenta whole-web flash is source-derived with the scoped ROM rule,
  UNKNOWN palette bytes and pending manual-acceptance status. No acceptance
  history or unrelated durable documentation was updated.

Ignored paired records:

- `AI-Logs/Prompts/2026-11-05-super-zapper-color-fidelity-prompt.md`
- `AI-Logs/Reports/2026-11-05-super-zapper-color-fidelity-report.md`

Reference files were not edited.

## 9. Focused validation

.NET SDK `9.0.304`; VSTest `17.14.1`, xUnit v2. No dependency changes/install.

Final focused command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter 'FullyQualifiedName~SuperZapperTests|FullyQualifiedName~BlasterLaneHighlightTests|FullyQualifiedName~PlayerShotRenderingTests|FullyQualifiedName~SquareTubeAndEntityProjectionRemainFiniteAndFlashOnlyChangesColor|FullyQualifiedName~PlusBoardRendererProjectsAllRimsPlayerLanesEntitiesAndFlashWithFiniteGeometry'
```

**54 passed, 0 failed, 0 skipped.**

Protects full slot sequence, all eight activation phases, equivalent first and
second color rules, no activation/reset phase restart, inactive colors,
60-step expiry and restoration, rail-only colors with identical geometry,
occupied rails, Pulsar overlay precedence and unchanged Pulsar glyphs.
Existing gameplay tests continue to protect charges, targets, delayed kills,
score, death and progression. New presentation-phase tests also verify that
display advances do not mutate a target's timer, height or lane.

Neighboring command:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --filter 'FullyQualifiedName~SuperZapperTests|FullyQualifiedName~BlasterLaneHighlightTests|FullyQualifiedName~SimulationTests|FullyQualifiedName~Flipper|FullyQualifiedName~Rim|FullyQualifiedName~Input|FullyQualifiedName~Player|FullyQualifiedName~Level|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~CampaignSoakTests'
```

**720 passed, 0 failed, 0 skipped.** Includes Super Zapper, rendering state,
Flippers, Pulsars (SimulationTests), spinner/input/player, production Levels
1-7 and progression/campaign tests.

Iteration record: preliminary focused runs passed 45 and then 52 cases as
coverage grew. The initial neighboring run was 718 passed / 2 failed / 0
skipped (720 total); both failures asserted the obsolete whole-web magenta
color in square/plus geometry tests. Only those expected color values were
updated; final focused and neighboring commands above then passed completely.

## 10. Full suite / build / whitespace validation

```powershell
dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check
```

- Full suite: **786 passed, 0 failed, 0 skipped**.
- Build: succeeded, **0 warnings, 0 errors**.
- `git diff --check`: exit 0, no whitespace errors.
- Git emits its existing LF-to-CRLF conversion notices for the two production
  files; these are Git notices, not compiler warnings or whitespace failures.

## 11. Manual acceptance checklist

The interactive game was NOT launched. Pending Neal's judgment:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

- First activation resembles the original arcade; evaluate the repeating
  multicolor rail cycle and its speed, not an activation-fixed starting hue.
- Depth-running unoccupied rails cycle; near/far rim connections remain blue;
  player/enemies retain their ordinary colors.
- Occupied-lane boundaries stay yellow; Pulsar overlays remain sensible and
  accepted near-rim suppression is unchanged.
- Normal colors return cleanly after presentation.
- Second use still selects/kills normally and uses the same visual color rule.
- No unrelated rendering, spinner, shooting, movement, scoring or progression
  regression.

Do not stage, commit or push until requested after manual acceptance.

## 12. Final Git state

- Branch: `master`.
- HEAD remains `23ceeeef3535505d2096f552f914d74082ec8eae`.
- Seven tracked modifications listed above, all unstaged; index unchanged.
- No non-ignored untracked files. Both requested records are ignored under
  the existing `AI-Logs/` rule.
- Final required status, HEAD, diff-stat, name-status, full diff, diff-check
  and untracked-list commands are run after saving this report.
- No gameplay semantics, unrelated rendering or reference files changed.
- No staging, commit, push, Internet research, or interactive launch occurred.
