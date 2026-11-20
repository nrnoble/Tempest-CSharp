# Between-level approach pre-implementation analysis

## Disposition

**GO: one bounded implementation checkpoint.** Add a distinct incoming-level
approach after successful natural exit travel, with spinner movement and a
frozen incoming wave; arrive at the existing gameplay projection and `Playing`
state. The architecture is clear. Exact duration, depth curve, and some
presentation choices remain unmeasured and should be manually tuned without
changing accepted Level 1–3 gameplay.

This is a read-only analysis. No source, tests, tracked documentation, or
reference files were changed; no tests/build were run. HEAD is
`df0dc194a6948ff11af19f720ed92d4a4ff84617` (`master`). The pre-existing
untracked `TEMP/` directory remains untouched. `AI-Logs/` is ignored.

## Current C# progression boundary

`TempestGame.RunOneStep` routes `ExitDelay` and `ExitZoom` separately from
ordinary simulation. Wave completion calls `BeginExit`, which enters
`ExitDelay` and clears outgoing shots/bullets. Exit delay advances for 21
updates (`exitDelayTicks > 20`); `ExitZoom` then changes `Projection.ZOffset`
and `Projection.K` while reducing `Player.Depth`. The incoming configuration
is committed only when the successful exit reaches `Player.Depth < 10`:
natural progression resolves the successor and calls `InitializeFreshWave`.
Sources: `src/Tempest.Game/TempestGame.cs:183-276`,
`:1468-1509`, `:1584-1615`.

`InitializeFreshWave` immediately replaces `CurrentLevel`, sets its state to
`Playing`, restores projection, resets player position/depth from the
configuration, resets death/hurry-up/Super Zapper state, and clears runtime
entities and counters. The new `Level` constructor/reset creates its initial
Eggs and, for `Level3Mixed`, its `Level3WaveLedger`; neither is updated in the
commit step because `UpdateExitZoom` returns after initialization. The first
rendered incoming-level frame is therefore already the full-size normal board
with the player at the incoming rim and a fresh, not-yet-ticked wave. Shooting
and incoming Egg/enemy simulation are available on the following simulation
step. Score and lives carry because this initializer does not reset them.
Sources: `TempestGame.cs:291-322`, `:412-419`, `Levels/Level.cs:55-96`,
`:153-180`.

`LevelState.Begin` exists, but fresh levels are explicitly set to `Playing`;
there is no current approach state (`Levels/LevelState.cs:3-7`). Current
natural production resolution is Level 1 -> Level 2 -> Level 3 -> terminal
(`Levels/LevelProgression.cs:5-9`). The approved C3/C4 wave, ledger,
progression, exit, death, and terminal behavior should remain unchanged apart
from inserting presentation at the existing successful commit.

## Incitti reference findings

These are **REFERENCE VERIFIED** source findings, not claims that Incitti
perfectly matches Atari:

- The outgoing tube enters `LEVEL_START_ZOOM`, waits, then uses
  `LEVEL_ZOOMING`; when the player reaches the far end it becomes
  `LEVEL_COMPLETE` (`Reference/incitti/tempest.bmx:1354-1403`).
- The completion branch sets position to 1500, changes to `LEVEL_BEGIN`,
  advances `current_level`, sets up the next level and enemies, restores two
  Super Zapper charges, and puts the player on edge 7
  (`tempest.bmx:1403-1433`). Thus setup happens before the approach finishes.
- `LEVEL_BEGIN` moves `position` inward by 10 per update until 50, while
  adjusting `CCenterY`, then switches to `LEVEL_READY`
  (`tempest.bmx:1311-1322`). The level draw projects the tube using that
  position (`tempest.bmx:769-817`); this is how the distant tube grows.
- The player is drawn on the rim during `LEVEL_BEGIN`. `Player.Update` runs
  fire, Super Zapper and keyboard/mouse rim movement while not dying
  (`tempest.bmx:639-710`, `:3747-3755`).
- The main loop updates Egg timers, Eggs, and Baddies during `LEVEL_BEGIN`;
  enemy simulation is suppressed only for `LEVEL_COMPLETE` (and certain
  unrelated death states) (`tempest.bmx:3747-3755`). Incitti therefore
  permits firing/Zapper and advances incoming enemies during approach. This
  conflicts with Neal's direct observation that combat engagement begins
  after arrival; the arcade observation takes precedence.
- Incitti uses global `K`/`ZOFFSET` projection and a changing level `position`
  (`Reference/incitti/transformfunctions.bmx:6-15`,
  `tempest.bmx:1313-1320`). It also calls `PlaySound(zoominsfx)` when
  `LEVEL_BEGIN` starts and `PlaySound(zoomoutsfx)` on zoom-out
  (`tempest.bmx:1313`, `:1354-1363`). These are source facts only, not a
  sound-asset recommendation.
- Its generic six-set progression/wrapping differs from this project's
  intentionally terminal Level 3 campaign. Do not copy that progression.

## Behavior matrix

| Behavior | Neal arcade observation | Incitti | Current C# | Evidence / implementation implication |
|---|---|---|---|---|
| Outgoing exit | Blaster exits the completed tube. | Start-zoom delay, then zoom travel to completion. | Simulation stops when exit begins; 21-update delay, then projection/depth travel. Success commits below depth 10. | Arcade observation + reference verified; preserve the C# exit and commit point. |
| Incoming tube | Next tube is first visible at a distance and grows during approach. | New level is set up at position 1500; position moves to 50. | Incoming tube appears immediately at normal projection. | Arcade observation + reference verified; add visual approach after commit. |
| Blaster | Small Blaster is on/around the distant rim. | Player is rendered at the rim during `LEVEL_BEGIN`. | Player resets to incoming start and is rendered at normal size. | Arcade observation + reference verified; project it on the translated incoming rim. |
| Spinner movement | Spinner moves Blaster around that rim. | Mouse/keyboard movement is active during `LEVEL_BEGIN`. | Input is ignored during standard exit, then movement resumes on normal `Playing`. | Arcade observation + reference verified; reuse existing lane movement during approach. |
| Firing / Zapper | Not established by the qualitative observation. | Both are allowed by `Player.Update`. | No approach exists; both are available in `Playing`. | Uncertain; recommend suppressing both during approach as a project gameplay-freeze policy, not as an arcade fact. |
| Enemies / Egg wave | Normal engagement begins on arrival. | Eggs and Baddies update during `LEVEL_BEGIN`. | Fresh Eggs/ledger are created at commit; all simulation begins next tick. | Direct arcade observation overrides Incitti; create but freeze the incoming wave. |
| Arrival / projection | Tube reaches normal gameplay size; exact curve/duration unknown. | `LEVEL_BEGIN` ends at position 50. | Existing gameplay uses `Level.Position=50`, `Depth=400`, `K=70`, `ZOffset=5`. | Reuse the current projection at offset zero; do not copy source timing or alter normal constants. |
| Progression | Both upcoming tubes are observed in the described flow. | Generic progression increments and wraps among sets. | Natural Level 1 -> 2 -> 3; Level 3 terminal. | Use the same natural successor path for both edges; no Level 4. |

Neal's qualitative observation is the authority for observed behavior; it
does not establish timing, projection values/curve, depth, firing policy,
enemy initialization details, HUD treatment, or exact input behavior.

## Recommended transition and gameplay design

Use an explicit `LevelState.Approach` (or equivalently named
`LevelApproach`) rather than overloading `ExitZoom`, `Begin`, or `Playing`.
At the existing successful natural exit commit, the **incoming** configuration
becomes `CurrentLevel` and owns the approach. This is the progression commit;
the outgoing level no longer simulates. Its successor resolver remains the
single shared route for Level 1 -> 2 and Level 2 -> 3.

Use a small transition presentation model (for example `LevelApproach`) to
hold deterministic elapsed/progress state and a nonnegative projection depth
offset that monotonically reaches zero. The incoming `Level` can be
constructed through the existing fresh-wave path: its configured initial
Eggs and, for Level 3, its ledger exist during approach, but are **not yet
advanced**. Enemy collections are empty. This avoids creating a second
wave-initialization path and retains C3's established fresh-level semantics.
On the arrival step, clear the approach and set the incoming level to
`Playing` once; return without simulating gameplay that same step, so the
first ordinary simulation tick follows the normal boundary.

During approach, run only the existing player movement calculation and
`Player.MoveAlongTube` against the incoming `Level`. Keep player depth at its
incoming rim; do not use exit travel or change spinner sensitivity. Ignore
Fire and Zap input (a provisional project decision pending any more precise
arcade evidence). The regular movement keys and mouse continue through the
existing `InputSystem`/`PlayerInput`; cursor capture stays unchanged. Current
input code has keyboard/mouse, not controller/gamepad bindings; any future
controller mapping can feed the same `PlayerInput` rather than adding an
approach-specific path.

Make approach a hard simulation boundary: do not advance Egg release clock,
Egg roaming/approach, hatch selection, enemy actions, Tanker firing, bullets,
player shots, Super Zapper activation, collisions, pulse/hurry-up clocks,
completion/all-rim checks, or score/combat timers. Existing initial Eggs and
Level 3 ledger remain static/frozen and are hidden during the approach render;
no enemy entities exist to collide. At arrival, existing counters/timers are
still fresh and ordinary `Playing` updates begin. Do not alter C3's Egg,
enemy, cargo, zapper, reconstruction, or completion policies.

## Projection and rendering

Keep `Projection.K`, `Projection.ZOffset`, board geometry, `Level.Position`,
and ordinary gameplay formulas unchanged. The cleanest seam is a
transition-owned depth translation applied only when projecting the approach
tube and Blaster, using existing `Projection.Project` math. At zero offset,
the exact same coordinates as the accepted normal board are submitted.
`K`/`ZOffset` remain at the restored gameplay values throughout the approach;
this avoids global mutable-camera state and protects other renderers. Bound
the offset to finite, nonnegative values so projected denominators remain
positive. Scale the Blaster/claw relative to the projected rim so it shrinks
with the distant board; do not invent a numeric curve or scale constant before
visual acceptance.

Render the incoming tube and Blaster, not Eggs, enemies, shots, bullets, or
wave-status details. Keep score/lives stable; whether to show destination
level text during approach is a presentation choice to settle during manual
acceptance. On arrival restore the established complete gameplay board/HUD.
Rendering consumes transition progress but does not decide when gameplay
starts. Relevant seams: `Rendering/Projection.cs:13-18`,
`Rendering/VectorRenderer.cs:79-118`, `:388-408`, `:544-582`.

## Shared progression, diagnostics, audio, and maintainability

- One implementation serves both natural edges through existing
  `LevelProgression`; carry score/lives unchanged and retain fresh Zapper,
  player, entity, Egg, and Level 3 ledger semantics.
- Direct `--level2-test`, `--level3-test`, `--level3-geometry-test`, and
  Tanker/Spiker/Fuseball/Pulsar scenarios start directly in `Playing` and
  bypass approach. `NaturalProgressionEnabled` remains the gate. No new CLI
  transition diagnostic is needed because both natural edges are manually
  reachable. No approach follows terminal Level 3; Game Over/restart remains
  unchanged.
- `Systems/AudioSystem.cs` is an empty scaffold. Future sound work should be
  an explicit audio/sound-fidelity roadmap item, not an accidental omission.
  The transition can expose semantic start/arrival notifications for a future
  AudioSystem consumer without coupling game state to a library. Do not add
  audio now, choose/copy assets, or infer sounds from Incitti.
- A small `LevelApproach` progress/presentation model is justified by its
  transition-only timer/offset and gives `TempestGame` one clear state
  boundary. Keep player simulation and start/arrival policy in
  `TempestGame`; do not introduce a general transition framework or refactor
  unrelated gameplay.

## Future automated test plan

Use public configuration/simulation seams (as current progression tests do);
avoid reflection and direct mutation of inaccessible internals.

1. Assert natural approach starts only after successful Level 1 and Level 2
   exits, with incoming config/current level correct and no early commit.
   Assert both edges carry score/lives and Level 3 remains terminal.
2. Assert direct development launch modes bypass approach and existing
   restart/Game Over paths remain unchanged.
3. During approach, pass movement plus Fire/Zap input: player lane/fraction
   changes through normal movement; no shots/Zap consumption. Snapshot Eggs,
   their heights/states, Level counters/reserve, and Level 3 ledger identity
   sets to prove no simulation or release occurs.
4. Assert no outgoing entities or pending cargo survive; incoming Level 2
   fresh wave and Level 3 fresh ledger/wave are correct. Verify no enemy
   simulation before arrival and the first regular Playing update advances
   the wave only once.
5. Capture approach tube/Blaster geometry at deterministic steps: all
   coordinates finite, depth offset monotonically approaches zero, and final
   rendered points equal existing normal-projection points. Check arrival
   changes to `Playing` exactly once.
6. Preserve current C3/C4 progression, mixed-wave, restart, renderer, input,
   and configuration regressions. Avoid assertions on unverified arcade
   timing or RNG internals.

## Future manual acceptance checklist

Run the normal campaign and repeat the same checks for **Level 1 -> 2** and
**Level 2 -> 3**:

- outgoing exit remains correct; destination tube first appears distant;
- approach grows smoothly and Blaster remains visible around the rim;
- spinner/left-right movement is responsive; controller behavior is checked
  only if a controller mapping exists; cursor behavior is unchanged;
- firing/Zapper do not activate under the provisional freeze policy and
  enemies do not visibly engage before arrival;
- arrival is smooth and returns to accepted normal tube/HUD/projection;
- score/lives carry; Level 3 arrival still has its fresh accepted mixed wave;
- no visual artifact/crash; compare the overall feel to Neal's arcade memory.

Do not demand exact timing without new reliable evidence. Record any arcade
correction to firing/HUD behavior separately from source-derived facts.

## Documentation and implementation checkpoint

After implementation/manual acceptance, update the current tracked
documentation that calls the approach deferred:

- `README.md:180-182` (deferred feature summary);
- `TEMPEST-BEHAVIOR-SPEC.md:58-60`, `:656-670`, and `:776-778` (decision,
  known behavior/uncertainty, and historical deferred note);
- `TEMPEST-DEVELOPMENT-NOTES.md:884`, `:972-982` (development status and
  deferred approach detail).

Do not rewrite historical reports. Preserve the qualitative arcade
classification and replace present-tense “not implemented/deferred” status
with accepted implementation evidence; keep unmeasured details explicitly
uncertain.

One checkpoint should include: explicit state/model; shared natural entry;
input-only movement and frozen gameplay; transition projection/render;
focused deterministic tests; manual acceptance for both edges; and direct
documentation updates. No split is needed: timing/curve refinement is a
bounded manual tuning task, not a separate architecture prerequisite.

Likely implementation surfaces: `Levels/LevelState.cs`; a narrowly scoped
`Levels/LevelApproach.cs` (or similarly located model);
`TempestGame.cs`; `Rendering/VectorRenderer.cs`; focused progression and
renderer tests; and the three tracked docs above. `Player`, `Projection`,
`InputSystem`, `Program`, `LevelConfiguration`, and C3 systems should need no
behavioral redesign; touch only if a focused seam is required. No blocker is
known.

**Conclusion: GO.** Preserve C3/C4 and terminal Level 3; implement one shared
natural approach with incoming-level ownership, movement-only input, a frozen
fresh wave, and a finite translated projection that reaches the exact normal
gameplay board.
