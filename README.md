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

The Blaster's occupied lane is highlighted by its two full-depth tube
boundaries. Current Levels 1-3 use yellow; the highlight follows the player's
indexed lane through normal play and the incoming-level approach. Pulsar
overlays take visual precedence, while the lane highlight remains visible over
the ordinary/Super Zapper tube colors. Neal manually accepted this C# rendering
across the implemented boards and interactions as visually identical to Atari.
Later palette colors and cycling remain deferred and have not been validated.

## Level configuration and wave lifecycle

`LevelConfiguration.Level1` explicitly records internal/displayed identity
0/1, board/palette 0/0, the existing closed Level 1 geometry, depth/position,
player start, ten-unit supply, Egg pacing/sequences, standard exit policy and
Flipper-only normal composition. `LevelConfiguration.Level2Square` is the
single production Level 2 object and also powers the direct development
launch. `Level` remains the mutable per-wave runtime.

Normal play progresses from Level 1 to Level 2 and then to the existing
`LevelConfiguration.Level3Mixed` after each successful standard exit. Wave
clear and exit delay do not commit progression; score and current lives carry
unchanged across both transitions. Fresh-level initialization gives Level 3
the same accepted production mixed-wave setup used by direct development
access, including two Super Zapper charges and a cleared flash. The Level 3
configuration remains terminal; pressing R there starts a new Level 1
campaign with reset score and configured starting lives. Game Over in a
normal campaign also restarts at Level 1. Neal manually accepted the visible
natural Level 1 -> Level 2 -> Level 3 progression, Level 3 entry/gameplay, and
terminal/restart behavior in normal campaign play. This accepts the current
C# progression and makes no new arcade-behavior claim. Natural progression now
enters an interactive approach state for each incoming production level. Neal
manually accepted the current approach presentation in normal campaign play:
no ExitZoom handoff discontinuity, projection flash, wrong board, Blaster jump
or new problem was observed. He found it impressive and close to remembered
Atari behavior; this is qualitative acceptance, not a measured fidelity
percentage or verification of provisional timing/projection tuning. The more
elevated Atari approach perspective remains a deferred fidelity refinement.

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

## Controls

- **Fire:** left/right Ctrl or left mouse button (existing arcade Fire input).
- **Movement:** horizontal spinner/mouse movement or Left/Right arrow keys.
- **Super Zapper:** Space or right mouse button, during eligible active play.
- **Restart/Start/Replay:** R or P, only at Game Over or Level Complete.
  Both invoke the same existing restart/replay behavior; neither starts a
  new game during active play, death or exit travel. Hold does not repeat.
- **Quit:** Escape, V or window close; all use normal application shutdown.

P and V are additive controller-friendly keyboard aliases, not arcade-verified
gameplay rules. All previous controls remain available without the controller.
The physical controller diagnostic confirmed ordinary Raylib key presses,
including Space; no character-input workaround or new zap key was added.

## Development-only controller input diagnostic

From the repository root in PowerShell:

```powershell
dotnet run --project src\Tempest.Game -- --input-diagnostic |
    Tee-Object -FilePath "$env:TEMP\Tempest-controller-input.txt"
```

`--input-diagnostic` opens a standalone **DEVELOPMENT ONLY** Raylib window
at the game's size, cursor capture and 60 FPS target. It runs no simulation,
spends no charges and assigns no controller buttons. Launch it by itself;
gameplay options are not used in this mode (conflicting scenario flags are
still rejected). Normal launches and all existing controls are unchanged.

Only test button input: all keyboard input delivered to this diagnostic is
logged, so do not type private text. Click the diagnostic window and verify
`FOCUS focused=True`. The terminal/file holds the full log; the window shows
recent lines and a live frame counter.

1. First use the ordinary keyboard: tap Space, R and Enter; hold/release Ctrl.
2. Then test controller buttons in this order: P, Enter, Space, Ctrl, Alt,
   Shift, Z, X, C, V, 1, 5. Test the two unknown small red buttons afterward.
3. For each button, tap twice about a second apart; then hold for two seconds
   and release. Leave a three-second gap before the next button. Note the
   order and any button with no output. Do not move the spinner while testing.
4. Close with ordinary keyboard Escape or the window X. Send the captured
   log and the physical-button/order notes, including any focus loss and
   whether holding a button produced repeats or a sustained held key.

`STATE` records Raylib down/pressed/released/repeat flags; `KEY_QUEUE` records
queued key codes even when no sampled press edge survives; `CHAR_QUEUE`
records Unicode text (`U+0020` is Space). `MOUSE` records mouse-button flags.
Matching frame numbers show signals observed together. `HEARTBEAT` confirms
focus and held keys during quiet/held intervals. Escape is still Raylib's
default close key, so its press can close before a diagnostic event line.
This log is evidence of what Raylib exposes, not raw USB/Windows messages or
proof of the controller's input mechanism. Neal's physical test confirmed
ordinary key events and led to the production P/V aliases above. This
standalone diagnostic retains its original controls: V is logged, not used
to quit, so its input can still be inspected. The two volcano Player 1/2
buttons produced no observable Raylib event; further investigation is deferred.

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
travel. Natural Level 2 now progresses to Level 3 after its successful exit;
`--level2-test` itself remains terminal and replayable with R and does not
enable campaign progression. Natural Level 3 now progresses through the
accepted Level 4 configuration after its successful exit. Level 4 is
temporarily terminal because Level 5 is not implemented; R starts a fresh
natural campaign at Level 1.

Score and current lives carry unchanged into each natural level, including
Level 4. The Super Zapper is restored to two charges on fresh-level setup.
There is no level-completion bonus, bonus
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
distant Blaster before combat engagement. The C# approach translates only the
incoming tube/player projection, freezes the incoming wave, and permits
spinner, left-stick and keyboard rim movement while suppressing fire and
Super Zapper. Its 120-tick duration and 240-unit starting depth offset are
provisional project presentation tuning, not arcade measurements. Neal
manually accepted the visible natural Level 1 -> Level 2 -> Level 3 -> Level 4
campaign approach; this does not verify exact timing, scale, or projection
parameters. Audio and sound fidelity remain long-term roadmap items; no audio
playback is implemented.

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
progresses through Level 1, Level 2, Level 3, and terminal Level 4. Production
Tanker mixed-wave access is provided separately below; the diagnostic remains
Flipper-only. Level 1/2 wave policy and
progression remain unchanged, but C1.5 changed directed enemy pivot
geometry/timing. Neal manually validated and passed the inward-pivot,
directed-sweep and geometry behavior for Level 1, Level 2, the Level 3
diagnostic, Pulsars and Tanker cargo. See
`TEMPEST-BEHAVIOR-SPEC.md` for the precise **ARCADE VERIFIED** observations;
automated geometry and source-derived values do not extend that evidence.

## Direct Level 3 mixed-wave access

```powershell
dotnet run --project src/Tempest.Game -- --level3-test --lives 10
```

`--level3-test` is **DEVELOPMENT ONLY** direct graphical access to the existing
`LevelConfiguration.Level3Mixed` model. It displays Level 3, uses the production
randomized mixed Flipper/Tanker wave and remains terminal when launched
directly. Natural campaign progression now enters this same model from Level
2; direct access remains isolated and does not enable progression. The window
title and HUD label direct access as a Level 3 mixed-wave test. The HUD uses
the Level 3 identity ledger's unresolved
original and cargo counts and includes Eggs, Flippers and Tankers in its active
count.

Production Tanker bullets use the existing renderer path and are visible
without `--enemy-bullets-test`. Ordinary and cargo Flippers remain silent by
default; the optional flag may enable their development firing behavior. On
successful completion, R replays a fresh Level 3 mixed wave while retaining
score and lives. Game Over restart follows the existing full new-game behavior.
Neal manually accepted the visible graphical behavior across multiple runs,
including production Tanker bullets, both Tanker cargo release paths,
Super Zapper behavior, death/re-entry, and completion/replay. No crashes or
other observed problems were reported; the overall rating was **5/5 —
"Impressive."** This accepts the visible C# behavior, not unknown Atari
internals. Existing **REFERENCE VERIFIED** and **PROJECT DECISIONS** evidence
classifications are unchanged.

## Isolated Level 4 production candidate

```powershell
dotnet run --project src/Tempest.Game -- --level4-test --lives 10
```

`--level4-test` is **DEVELOPMENT ONLY** direct access to Level 4; it does not
enable or replace natural campaign progression. Natural Level 3 -> Level 4
and the diagnostic use the same accepted production configuration and
gameplay. The configuration uses the source-derived closed 16-lane binoculars
board, 17 original enemy obligations, four initial Eggs, and 16 persistent
height-60 Spikes. Its randomized eligible originals are Flippers, Tankers, and
Spikers; Tankers carry two Flippers. Fuseballs and Pulsars are excluded. Level 4 uses
the spike-avoid exit and remains terminal; natural-campaign R starts a fresh
Level 1 campaign, while diagnostic replay/Game Over restart return to Level 4.
A prominent **Avoid Spikes** warning remains through the configured avoidance
interval and controllable pretravel delay, then hides when exit travel begins.
Exit policy is configured
independently of Spike presence: Levels 1-3 use the standard exit; Incitti's
Levels 4-7 use the spike-avoid policy, and the Level 4 configuration opts into
it. Later Spike-bearing
levels may use a standard exit without this warning. Levels 5-8 are not
implemented here.

Neal manually accepted the natural Level 1 -> Level 2 -> Level 3 -> Level 4
campaign, including the corrected Spike-exit behavior, late-wave Spiker
behavior, and overall Level 4 feel. This accepts visible behavior only; it
does not establish general arcade fidelity or Atari's hidden identity
accounting. The direct diagnostic remains development-only.
Source findings are **REFERENCE VERIFIED**; C# implementation choices remain
project decisions.

Incitti calls the Spiker a Spinner. On retreat it leaves the persistent Spike,
removes the unresolved enemy from `enemiesleft`, and returns one unit to
`eggsleft`; it does not score a kill. Each later Egg hatch runs the general
picker again. That picker excludes Spikers when
`enemiesleft + egg_list.Count() < 4` or more than three Spikers are active.
Equality at four still permits Spikers. The listed Egg count includes
approaching/hatching Eggs; unreleased `eggsleft` reserve does not participate.
Active Tanker cargo count as enemies after materialization, while bullets and
Spikes do not; a zapped enemy remains counted until its delayed removal.
The C# ledger therefore preserves the original ID and unresolved status on
retreat, clears the runtime type selection, then applies the same general
picker when that ID hatches again. This is a project representation of the
reference lifecycle, not a claim about Atari's hidden identity mechanism.

During the configured Avoid Spikes interval and pretravel delay, rim movement
remains available but new firing is disabled. Firing becomes available on the
first update after down-tube exit travel begins. Shots already in flight when
Avoid Spikes starts are retained and continue moving, including through
reverse/retry; firing is also suppressed during the life-loss interval. This
visible firing boundary follows Neal's direct arcade observation, which
differs from Incitti's general firing eligibility in these states. A shot in
a Spike's lane and below its tip shortens it by 10 and scores 1; zero-or-less
removes the Spike. Exit shots use a Spike-only collision path, so they cannot
resolve enemy identities or run enemy simulation. Neal's visible Atari
observation is that endless final-Spiker recycling is wrong; the hidden
mechanism that prevents it is not arcade-verified. Neal manually verified the
visible firing boundary, Spike damage/scoring, reverse/retry, and preservation
of shots already in flight when avoidance begins. This does not verify the
hidden C# identity/accounting mechanisms.

Production Tanker bullets remain enabled, and source-backed Spiker retreat
bullets are supported in this candidate. Ordinary and cargo Flippers remain
silent in production, including Level 4; this preserves the accepted Level 1-3
policy rather than introducing a Level-4-only Flipper bullet unlock. Incitti
allows ordinary Flipper bullets from earlier levels, so this is a documented
**PROJECT DECISION** and known source discrepancy, not an arcade-fidelity
claim. `--enemy-bullets-test` remains an orthogonal development option.

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

In this separate Spiker diagnostic, after the wave resolves movement and
firing remain enabled during a 151-update avoidance interval, the 21-update
exit delay, and zoom. Avoid or shoot remaining spikes. Hitting one during
travel costs one life and reverses the zoom before an exit retry; spikes
persist. R replays after completion and restarts after Game Over. Omit the flag
to return to normal Level 1. No other enemy types, next level, bonus, or audio
are implemented.

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

## Player-death enemy retraction

**ARCADE VERIFIED:** Neal observed active enemies retracting backward/down the
tube when the Blaster is destroyed. They are inactive during that presentation,
and existing Spikes remain fixed on the web. The full retraction takes no more
than approximately two seconds; this is a qualitative upper bound, not an
exact measurement or frame count.

The C# presentation keeps the existing 60-update death/re-entry lifecycle.
At the fixed 60 Hz simulation rate, a linear render-only depth offset spans one
tube depth during those updates. This deterministic one-second tuning is a
**PROJECT DECISION**, not an arcade-exact curve or duration. Enemy gameplay
positions, lanes, identities, accounting and score remain frozen/unchanged;
Flippers, Tankers, Spikers, Fuseballs and Pulsars share the offset. Spikes,
Eggs, projectiles and hazards keep their existing separate presentation and
lifecycle policies.

Incitti provides a **REFERENCE VERIFIED** shared projection/depth-shift clue,
not a dedicated enemy-specific retreat routine. Neal manually accepted the
visible C# retraction on Levels 1-3 and the Spiker/Spike and Game Over/restart
scenarios. The separate Atari Blaster-location death animation remains deferred
until arcade footage establishes its geometry and timing.

## Super Zapper

The source-derived Super Zapper works in normal Level 1, natural Levels 2
through 4, and every existing development wave. Space or the right mouse button
activates it on a press edge. Each fresh Level setup provides two charges; ordinary life
loss preserves the remaining charge. Natural Level 1-to-2 progression restores
two charges and clears flash state through fresh-wave initialization; Level
2-to-3 and 3-to-4 use the same boundary. Normal Game Over and terminal Level 4
R start a new campaign. Fresh Level 3 and Level 4 charge states are project
decisions, not a new manual arcade observation. Development replay keeps its
established fresh-charge behavior. The first use assigns
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
