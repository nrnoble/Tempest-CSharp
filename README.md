# Tempest C# Recreation

Personal C# recreation guided first by observed Atari Tempest gameplay, with Mark Incitti's Tempest 3.1 BlitzMax project as an engineering reference.

## Initial goal
Get one tube playable with player movement, shooting, a Flipper, collision, scoring, and a fixed 60 Hz simulation.

## VS Code
Install the current .NET SDK and Microsoft's C# Dev Kit extension, then open this folder in VS Code.

```powershell
dotnet build
dotnet test
dotnet run --project src/Tempest.Game
```

## Code organization
- `Program.cs` composes the game, input and renderer, and owns the window and frame loop.
- `TempestGame.RunOneStep` advances one gameplay tick without calling Raylib.
- `Gameplay` owns player movement, shots and the Flipper state machine.
- `Levels` defines wave configuration and owns the current `Level` runtime, geometry, Egg sequences and accounting.
- `Systems/InputSystem.cs` samples Raylib input; `ScoreSystem.cs` owns the score.
- `Rendering/VectorRenderer.cs` draws the tube, entities, explosion and HUD.

During active play, the tick processes player/firing and zap input, shots and
bullets, an early completion check, Egg release/update/hatching, enemy updates
and collisions, all-rim completion, Hurry-Up, and the final completion check.
The frame loop retains one simulation tick per frame at the existing 60 FPS target.

## Level configuration and wave lifecycle

`LevelConfiguration.Level1` explicitly records internal/displayed identity
0/1, board/palette 0/0, the existing closed Level 1 geometry, depth/position,
player start, ten-unit supply, Egg pacing/sequences, standard exit policy and
Flipper-only normal composition. `LevelConfiguration.Level2Square` is the
single production Level 2 object and also powers the direct development
launch. `Level` remains the mutable per-wave runtime.

Normal play progresses from Level 1 to Level 2 after successful standard exit
travel, when the updated player depth falls below 10. Wave clear and the exit
delay do not advance the level. Score and current lives carry unchanged.
Manual play confirmed two usable Super Zapper charges at Level 2 start; the
exact internal recharge/flash-reset timing remains a project implementation
detail. The recreation adds no level bonus, bonus life, startbonus
presentation, entry animation or audio.
Natural Level 2 remains terminal because its production Level 3 wave is not
implemented. Pressing R there starts a new Level 1 campaign with reset score
and configured starting lives. Game Over in a normal campaign also restarts
at Level 1. The separate Level 3 geometry diagnostic below does not change
normal progression.

The closed definition has 16 ordered vertices and 16 playable lanes. The
topology metadata distinguishes these counts: an open definition with 16
vertices would expose 15 playable lanes, but open-tube gameplay is not
implemented. Four Eggs start active and six remain in reserve. The 50-tick
release cadence, four-active-Egg cap and deterministic spawn/direction
sequences are current project decisions, not claims about literal Incitti
behavior. Every 50 active ticks, the oldest roaming Egg is selected to
approach, with at most two approaching or ready to hatch.

Life reconstruction retains the same `Level` and configuration. Unresolved
enemies return through fresh Eggs while destroyed/rim-cleared outcomes, score,
charges, Hurry-Up state and persistent Spikes keep their established
lifetimes. Hatching converts an Egg without changing the remaining-enemy count.
The HUD's ACTIVE counter includes active Eggs and active enemies.

Tests run headlessly without initializing a Raylib window. Visual appearance,
spinner feel and cursor capture still require manual gameplay testing.

## Level 2 square/canflip and direct development access

```powershell
dotnet run --project src/Tempest.Game -- --level2-test --lives 10
```

`--level2-test` is **DEVELOPMENT ONLY** direct access to the same closed
16-lane square, 12-unit Flipper-only configuration used by natural progression.
B2 enables the validated below-rim canflip behavior using B1's square endpoint
hinges. Flippers choose direction after their first climb update, retain it
across below-rim pivots and pauses, and resample the Blaster lane only when a
rim pause expires. Climb/pivot increments of 2/8 and lane/rim pauses of 21/24
are **REFERENCE VERIFIED**, not arcade-measured tuning. The recreation
preserves two-lane shot acceptance during pivots and completed-pivot player
safety; Incitti's destination-only shot ownership is not silently imported.
Normal Level 1 progresses automatically to this Level 2 after successful
travel. `--level2-test` itself remains terminal and replayable with R; it does
not enable progression. Level 2 completes with an explicit "NEXT LEVEL NOT
IMPLEMENTED" message; R starts a new normal campaign at Level 1.

Score and current lives carry from Level 1. The Super Zapper is restored to
two charges on fresh Level 2 setup. There is no level-completion bonus, bonus
life, source `startbonus`, Begin/Ready entry animation, entry-position shift
or audio. These are incomplete presentation/economy features, not claims of
arcade-equivalent behavior. Level 1 remains the default with no flags.
`--level2-test` is mutually exclusive with the Spiker, Tanker, Fuseball and
Pulsar scenarios; it may be combined with `--lives` or the orthogonal
`--enemy-bullets-test` option. Those other development scenarios remain
terminal and replay their selected scenario.

Neal completed B2 manual validation on 2026-10-31. He found the broad below-rim
lane changes consistent with Atari Level 2: straight-side and corner flips
looked mechanically sensible, with no snapping or wrong-lane landing. Active
turns did not visibly retarget halfway through; later transitions may respond
to the then-current Blaster position. Below-rim visual crossing was not lethal,
rim pursuit and completed-pivot behavior remained normal, and no crash occurred
or B1 renderer-crash recurrence was observed. These qualitative observations
are **ARCADE VERIFIED** where applicable, not measurements of exact tuning,
geometry math, shot bands or internal state.

Neal subsequently completed end-to-end manual validation of the natural
Level 1-to-2 transition. He confirmed automatic square Level 2 entry without a
Level Complete/R pause, updated identity and immediate square rendering,
unchanged score and remaining lives, four visible Eggs, the 12-unit wave, two
usable Super Zapper charges, normal validated canflip behavior, and no
observed regression. In his run, 10 starting lives became 8 after two Level 1
deaths and Level 2 began with 8; this is an example, not a fixed life rule.
These broad observable results are **ARCADE VERIFIED** within that manual
comparison; exact transition ticks and implementation internals are not.

Atari has a brief between-level approach through space with an interactive
distant Blaster before combat engagement. This presentation is not implemented
and is deferred; its duration and timing details have not been measured.

## Level 3 plus-board geometry diagnostic

```powershell
dotnet run --project src/Tempest.Game -- --level3-geometry-test --lives 10
```

`--level3-geometry-test` is **DEVELOPMENT ONLY**. It exposes the single
source-derived, closed 16-lane Level 3 plus-board configuration for rendering,
rim movement, Eggs, shots, Flipper canflip pivots, Super Zapper flash and
projection checks. Its controlled Flipper-only setup uses the project's
deterministic Egg schedule; it is not a production Level 3 mixed wave and does
not claim the eventual Level 3 composition or Egg schedule. Tanker-cargo
geometry is covered by headless tests only in this checkpoint; it is not
graphically included in this diagnostic.

The diagnostic is terminal and R replays it; Game Over restart stays on the
same board. It conflicts with other scenario-selection flags and composes
with `--lives` and the existing optional `--enemy-bullets-test`. Normal play
still progresses only from Level 1 to Level 2, then stops. Production Tanker
mixed accounting and Tanker firing remain deferred. Level 1/2 wave policy and
progression remain unchanged, but C1.5 changed directed enemy pivot
geometry/timing. Neal manually validated and passed the inward-pivot,
directed-sweep and geometry behavior for Level 1, Level 2, the Level 3
diagnostic, Pulsars and Tanker cargo. See
`TEMPEST-BEHAVIOR-SPEC.md` for the precise **ARCADE VERIFIED** observations;
automated geometry and source-derived values do not extend that evidence.

## Spiker development wave

Normal startup still runs the validated Flipper-only Level 1. To explicitly run
the development scenario on the same tube:

```powershell
dotnet run --project src/Tempest.Game -- --spiker-test
```

This is not arcade Level 1 progression. It retains the ten-enemy supply, starts
height-60 green spikes on every lane, and enables Spikers alongside Flippers.
Spikers use an assigned development speed of 3; hatch/retreat decisions vary
between runs. Green spirals build persistent green spikes with white tips.
Spikers award 50 points; spike hits award 1 point and shorten the spike by 10.
The final hatch selections exclude Spikers when active enemies plus Eggs fall
below four, following the source's end-of-wave restriction.

After the wave resolves, this scenario permits movement and firing during a
151-update avoidance interval, the 21-update exit delay, and zoom. Avoid or
shoot remaining spikes. Hitting one during travel costs one life and reverses
the zoom before an exit retry; spikes persist. R replays after completion and
restarts after Game Over. Omit the flag to return to normal Level 1. No other
enemy types, next level, bonus, or audio are implemented.

## Flipper bullet development

Flipper-fired enemy bullets are disabled in normal Level 1. To test them:

```powershell
dotnet run --project src/Tempest.Game -- --enemy-bullets-test
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
```

The flag is independent of `--spiker-test`; Spiker test runs do not fire Flipper
bullets unless both flags are supplied. Enabled Flippers receive 1-3 rounds,
fire while climbing or after pause transitions, and send bullets outward toward
the rim. Shoot bullets for 25 points or move off their lane near the rim.

## Tanker development wave

Tankers are not included in normal Level 1. To run the controlled single-Tanker
wave with an extended life count:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

The scenario starts one Tanker carrying two Flippers; other original wave slots
remain Flippers. The Tanker climbs on one lane and releases two live Flippers on
opposite adjacent lanes near the rim. Each child follows the project-selected
outward lane route away from the Tanker's former lane, while its spatial pivot
swings inward into the web. It keeps its direction through below-rim
pivot/pause cycles without tracking the Blaster; rim pivots pursue the current
Blaster lane. Neal verified the cargo survival and no-mid-tube-pursuit behavior;
Neal has since manually confirmed the inward arc, stable below-rim direction,
correct rim behavior and preserved outward lane route. The exact intermediate
arcade lane sequence and timing remain unknown.
Shoot the Tanker for 200 points, or let it release naturally for no Tanker
score. Children remain independently shootable. Tankers and Flippers fire no
bullets unless `--enemy-bullets-test` is combined with this scenario; Tankers
are silent with `--tanker-test` alone.
`--tanker-test` cannot be combined with `--spiker-test`.

Historical cargo-direction checkpoint validation: 56 focused Tanker/cargo
tests, 29 Super Zapper tests, 28 Flipper tests, and 321 full-suite tests passed.
Those counts and the checkpoint-era pending status are historical; later
manual results and current validation are recorded in the behavior
specification and milestone reports.

## Fuseball development wave

```powershell
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
```

This development-only wave hatches all ten ordinary enemy units directly as
Fuseballs on the existing tube. It keeps the established Egg release schedule
and uses Incitti's unscaled 3.3 climb speed; it is not arcade level composition.
The flag is mutually exclusive with `--spiker-test` and `--tanker-test`.
`--enemy-bullets-test` adds no firing to this Fuseball-only wave.

Compare lateral boundary travel, climb/descent phases, five-color vector
animation, and the 250–1250 height-based score labels. Shots hit only inside
`1 < w < 7` and strictly within 16 depth units below a Fuseball. Only lateral
travel at the rim is lethal. Source `cnt > 8` means lateral steps are actually
nine updates apart (the earlier analysis called this eight); vertical duration
is rerolled each update. Surviving units return as fresh Eggs after life loss.
Normal Level 1 remains unchanged. This scenario does not add Tanker cargo,
audio or progression.

## Pulsar development wave

```powershell
dotnet run --project src/Tempest.Game -- --pulsar-test --lives 10
```

This isolated wave hatches all ten ordinary Egg units directly as Pulsars on
the existing closed tube. Pulsars use the source's 51-update decision check,
18-tick lane pause, 14-tick rim pause, and shared 181-update pulse clock.
Pulse hazards are lane-wide; shots must pass the strict 0-to-16 height band
and award 200 points. The clock and enemies freeze during the existing
60-update player-death interval. `--pulsar-test` is mutually exclusive with
the Spiker, Tanker and Fuseball scenarios; `--lives` composes, and
`--enemy-bullets-test` does not arm Pulsars.

Compare the glyph, lane flashing, movement, collision and re-entry with Atari
Tempest manually. This is not normal Level 17 composition and does not add
Tanker cargo, audio, open-tube rendering or progression.

## Source-derived hurry-up in the direct development waves

`--fuseball-test` and `--pulsar-test` now use Incitti's latched `rimit` rule.
At the end of an active simulation tick, after Egg approach/hatching, enemy
updates and player-collision checks, hurry-up activates only when reserve Eggs
and active Eggs are exhausted and an active enemy remains below depth 400.
The last hatchling receives its ordinary update on that trigger tick; enemy
updates begin using the latch on the next tick.

At a lateral-boundary decision, hurried Fuseballs climb even if the ordinary
random result selected descent. Climbing applies the existing 3.3 term twice
(6.6 per climbing update), clamped at the rim. Existing descent and nine-update
lateral cadence are unchanged. Hurried Pulsars choose the top-end lane-flip
branch instead of descent, keeping one-unit movement, existing pauses, lane
transfer and the shared pulse timing/rendering.

The latch survives the full player-death freeze and fresh-Egg reconstruction;
R after Game Over or Level Complete clears it on restart/replay. It adds no
score, kills or accounting changes. Below-rim survivors retain this Hurry-Up
behavior; after Egg sources are exhausted, the separate all-rim completion
path takes precedence when every active enemy reaches the rim.
Normal Level 1, Spiker and Tanker scenarios do not enable hurry-up.

This is source-derived behavior in artificial development compositions.
Neal later manually tested both scenarios: observed Fuseball late-wave
acceleration matched his Atari expectation, while the exact Pulsar redirect,
trigger ticks and latch timing remain reference/project decisions rather than
arcade-verified details. See the behavior specification for the evidence
boundary.

## Super Zapper

The source-derived Super Zapper works in normal Level 1, natural Level 2 and
every existing development wave. Space or the right mouse button activates it
on a press edge. Each fresh Level setup provides two charges; ordinary life
loss preserves the remaining charge. Natural Level 1-to-2 progression restores
two charges and clears flash state through fresh-wave initialization; normal
Game Over and natural Level 2 terminal R start a new campaign. Development
replay keeps its established fresh-charge behavior. The first use assigns
delays 4, 6, 8, ... to accepted enemies in source insertion order. The second
use selects one eligible enemy using Incitti's literal lane-distance, height
and type comparisons, then assigns delay 4.

Eligible enemies keep moving during their countdown. A player shot, Spiker
retreat, or Tanker natural release can resolve a pending target first. Zap
expiry awards the normal enemy-specific score once; EnemyBullets and spikes are
unaffected. Fuseball expiry uses its current height for both score and popup.

In `--tanker-test`, carrier zap expiry awards 200 and materializes two distinct
unresolved cargo IDs as adjacent Flippers with eight-update zap timers. Their
mandatory initial pivot follows the project-selected outward lane route away
from the Tanker's former lane, but the spatial arc is inward into the web. Their
direction stays fixed below the rim, then resumes player pursuit at the rim.
Their timers begin on the next simulation tick.
Neal manually confirmed that their visible pivots swing inward through the
web while their established outward lane route remains correct.
Cargo remains unresolved until a normal outcome or timer expiry; life loss
reconstructs each unresolved ID as a fresh Egg and discards its pending timer.
Completion remains blocked until all original slots and materialized cargo
resolve.

The tube/web alternates its normal blue with the existing Tanker/magenta color
for 60 display steps. It has no charge counter or icons. The reference-derived
details are **REFERENCE VERIFIED** against Incitti's source only; they are not
proof of Atari arcade behavior. The C# simulation uses the project's existing
60-update death freeze, stable source-order collections, and deferred
same-tick Tanker-child activation policy.

Historical Super Zapper checkpoint validation: 29 focused tests and 318 full
suite tests passed; the build had no warnings or errors. Neal later manually
validated Super Zapper use in all five existing scenarios. Exact selection
comparisons, timer values, accounting and timing remain source/project details,
not arcade-verified. Audio, charge HUD, new enemy types, Level 3 and broader
campaign progression remain out of scope.

## Tanker-cargo direction correction

The Tanker-cargo correction preserves normal Flippers and Super Zapper
semantics. Manual validation confirmed that cargo no longer tracks the Blaster
below the rim, then exposed a separate shot-collision problem: the source-
derived inward lane route put both children on the Tanker's lane and allowed a
second in-flight shot to destroy them. Cargo keeps the project-selected
outward lane route and fixed below-rim direction; this route is independent of
the spatial pivot side, which now swings inward into the web. Neal confirmed
both released Flippers should survive and appear in distinct non-Tanker lanes.
Neal has since manually validated the corrected inward arc, the distinct
non-Tanker lanes and the preserved outward lane route. The exact arcade
intermediate lane sequence and timing remain unknown. See
`AI-Logs/Reports/2026-10-30-tanker-cargo-direction-correction-report.md`.

## All-rim completion

**ARCADE VERIFIED:** Neal observed that, after reserve and incoming Eggs are
exhausted, the Blaster may exit when every remaining enemy reaches the rim;
those enemies disappear without needing individual kills. The recreation keeps
kill-scored destruction separate from non-kill rim clears. A rim clear awards
no score, creates no Fuseball kill popup, and does not increment
`EnemiesDestroyed`; the remaining-enemy HUD uses the combined unresolved count.

The implementation checks post-update enemy heights against `Level.Depth`
(400) after player/enemy, bullet and spike collisions. This exact threshold,
state-agnostic enemy list, update ordering and pending-zap handling are
**REFERENCE VERIFIED / PROJECT DECISION**, not arcade-verified details.
Flippers, Tankers, Spikers, Fuseballs and Pulsars qualify; bullets and Spike
hazards do not. A below-rim Fuseball or Pulsar keeps the existing Hurry-Up
behavior. Tanker latent or queued cargo blocks completion until represented;
each live cargo identity is resolved separately. Spiker exit keeps persistent
Spike hazards, and reverse/retry does not resurrect rim-cleared enemies.

For manual validation, run:

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
dotnet run --project src\Tempest.Game -- --pulsar-test --lives 10
dotnet run --project src\Tempest.Game -- --tanker-test --lives 10
dotnet run --project src\Tempest.Game -- --spiker-test --lives 10
```

Let all incoming Eggs clear and observe whether rim enemies disappear without
kill score; verify a below-rim survivor delays exit, Hurry-Up yields to later
rim completion, Tanker cargo below the rim blocks, pending Super Zapper targets
do not score after rim clear, and Spike hazards remain during the Spiker exit.
Same-tick player-collision precedence and exact arcade timing still need
hands-on comparison.

## Reference mapping
- `tempest.bmx` -> gameplay and state behavior
- `transformfunctions.bmx` -> `Rendering/Projection.cs`
- `vectorfont.bmx` -> `Rendering/VectorFont.cs`
- levelset files -> `Levels/LevelLoader.cs` / data files later

The repository-specific Copilot guidance is in `.github/copilot-instructions.md`.
