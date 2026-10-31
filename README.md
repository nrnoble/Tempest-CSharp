# Tempest C# Recreation

Personal C# recreation based on the gameplay behavior of Mark Incitti's Tempest 3.1 BlitzMax project.

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
- `Levels` owns the current tube geometry, deterministic spawn sequence and wave counters.
- `Systems/InputSystem.cs` samples Raylib input; `ScoreSystem.cs` owns the score.
- `Rendering/VectorRenderer.cs` draws the tube, entities, explosion and HUD.

The active tick order is player/firing, shots and shot collisions, reserve Egg
release, Egg updates, hatching, Flipper updates, completed-pivot player
collisions, then level completion.
The frame loop retains one simulation tick per frame at the existing 60 FPS target.

Level 1 starts with four roaming Eggs and six in reserve. Reserve releases use
a 50-tick interval and a four-active-Egg limit. Every 50 active ticks the oldest
roaming Egg is selected to approach, with at most two approaching or ready to
hatch. Release lanes and alternating roaming directions remain deterministic.
Eggs hatch into Flippers without changing the remaining-enemy count. After a
life loss, surviving enemies return to the Egg supply, with up to four released
immediately; defeated enemies and score remain preserved. The HUD's ACTIVE
counter includes both active Eggs and active Flippers.

Tests run headlessly without initializing a Raylib window. Visual appearance,
spinner feel and cursor capture still require manual gameplay testing.

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

After the last kill, this scenario permits movement and firing during a
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

## Flipper bullet development

Flipper-fired enemy bullets are disabled in normal Level 1. To test them:

```powershell
dotnet run --project src/Tempest.Game -- --enemy-bullets-test
dotnet run --project src/Tempest.Game -- --enemy-bullets-test --lives 10
dotnet run --project src/Tempest.Game -- --spiker-test --enemy-bullets-test --lives 10
```

The flag is independent of `--spiker-test`; Spiker test runs do not fire Flipper
bullets unless both flags are supplied. Enabled Flippers receive 1-3 rounds,
fire from their lane during climbs or after pause transitions, and bullets travel
toward the rim. Shoot bullets for 25 points or move off their lane near the rim.

## Tanker development wave

Tankers are not included in normal Level 1. Run the controlled one-Tanker wave:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

The selected Tanker carries two Flippers. It climbs in one lane, then releases
them on opposite adjacent lanes; shooting it awards 200 points and releases the
same live cargo. `--tanker-test` cannot be combined with `--spiker-test`. Add
`--enemy-bullets-test` only to enable the existing Flipper bullets; Tankers do not
fire in this milestone.

## Tanker development wave

Tankers are not included in normal Level 1. To run the controlled single-Tanker
wave with an extended life count:

```powershell
dotnet run --project src/Tempest.Game -- --tanker-test --lives 10
```

The scenario starts one Tanker carrying two Flippers; other original wave slots
remain Flippers. The Tanker climbs on one lane and releases two live Flippers on
opposite adjacent lanes near the rim. Shoot the Tanker for 200 points, or let it
release naturally for no Tanker score. Children remain independently shootable.
Tankers fire no bullets in this milestone; existing Flipper bullets are enabled
only by additionally passing `--enemy-bullets-test`. `--tanker-test` cannot be
combined with `--spiker-test`.

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
Normal Level 1 remains unchanged. Tanker cargo, Zapper, audio and
progression are not part of this milestone.

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
Tanker cargo, Zapper, audio, open-tube rendering or progression.

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
score, kills or accounting changes. All enemies must still be destroyed before
exit; Incitti's separate onrim-based exit rule is deliberately not copied.
Normal Level 1, Spiker and Tanker scenarios do not enable hurry-up.

This is source-derived behavior in artificial development compositions, not
arcade-verified hurry-up. Neal's manual validation is pending; the behavior
specification remains unchanged until review, automated validation and that
manual validation are complete.

## Reference mapping
- `tempest.bmx` -> gameplay and state behavior
- `transformfunctions.bmx` -> `Rendering/Projection.cs`
- `vectorfont.bmx` -> `Rendering/VectorFont.cs`
- levelset files -> `Levels/LevelLoader.cs` / data files later

The repository-specific Copilot guidance is in `.github/copilot-instructions.md`.
