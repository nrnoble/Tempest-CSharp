# Blaster Death Visual-Fidelity Analysis

## Disposition

**Proceed with a bounded rendering refinement; do not redesign player death.**
The supplied arcade sequence establishes a qualitative visual target that the
current C# effect does not meet. Reuse the existing death countdown and
render-only presentation boundary. Do not change the 60-tick life-loss timing,
enemy retraction, reconstruction, accounting, or other gameplay behavior.

This is analysis only. No production code, tests, README, behavior
specification, development notes, or Git state were changed. The repository
was at `7ece019598cc57676ab096156c2cec413ff86df5` (`Implement accepted larger
game window`) at investigation start. `git status --short` showed only the
intentionally untracked `TEMP/`; it was not inspected or disturbed. This report
records the supplied frame observations because those image files were not
present in the repository paths inspected.

## ARCADE EVIDENCE

The supplied sequential frames `frame_0041.png` through `frame_0065.png` are the
behavioral authority for this work. The observations provided with them
establish that:

- The effect develops at the Blaster's death location and remains an
  in-playfield effect: the tube is visible behind it.
- It is a layered, angular/vector starburst-like presentation, not merely a
  handful of detached Blaster fragments.
- Multiple related outlines/colors participate. White, red/orange, and yellow
  are prominent; the outlines are not one static shape.
- Its size changes substantially: a compact beginning grows into a much larger
  layered presentation, contracts, persists as a smaller remnant, then
  diminishes until effectively gone.
- The exact geometry, outline count, timing, color schedule, and scaling curve
  are not measured by the observations. No numerical values or precise
  transitions should be inferred from this qualitative evidence.
- Small white radial effects elsewhere in some frames are not automatically
  part of the Blaster effect.

The older wording in the behavior specification and development notes that
Blaster footage is still awaited (the behavior specification's death section;
the development notes' player-death and visual-fidelity sections) is now
superseded by this supplied sequence. Those documents were expressly out of
scope for this analysis run and were not edited.

## EXISTING C# BEHAVIOR

### Architecture and trigger

- `TempestGame` owns the fixed 60 Hz simulation and death/life transitions;
  `VectorRenderer` reads state and draws the scene. The simulation does not
  call the renderer.
- `PlayerDeathPresentation` is a deterministic clock with `Start`, `Advance`,
  and `Reset`; it currently exposes elapsed/duration, completion, and linear
  `EnemyRetractionProgress`. It contains no Blaster-effect geometry or visual
  phase state.
- The ordinary `KillPlayer` entry point accepts lethal events only while
  `Playing`, sets `DeathTicks` to `PlayerDeathTicks` (60), starts
  `DeathPresentation`, switches to `PlayerDying`, and clears player shots.
  Pulsar, Fuseball, enemy-bullet, and Flipper collisions use this path.
- Spike collision during the spike-exit route is a distinct path:
  `CheckSpikesAgainstPlayer` sets the same 60-tick `DeathTicks` and enters
  `ReverseZoom`, but does not start `PlayerDeathPresentation`. Reverse zoom
  advances the life timer and maintains its existing projection/retry rules.
  This distinction matters if the rendering refinement shares the Blaster
  effect across both death routes.

### Current Blaster drawing

- `VectorRenderer.Draw` draws the tube first, then spikes and gameplay
  entities, then draws the player or death effect, followed by the HUD. The
  death effect is drawn when `DeathTicks > 0` and the game is not over, so the
  tube remains visible behind it.
- `DrawPlayerExplosion` obtains the projected player-edge endpoints, computes
  the center from the player's edge position, and draws eight equally spaced
  radial line segments in `Color.Yellow`. Each ray extends from one-quarter of
  its offset to the full offset.
- Its radius is `8 + progress * 48`, where
  `progress = 1 - DeathTicks / PlayerDeathTicks`. It therefore starts compact
  and expands monotonically; it does not contract, form multiple outlines, or
  leave a shrinking remnant. The formula is a C# approximation, not an
  arcade-derived curve.
- During ordinary `PlayerDying`, player input/gameplay entities are frozen,
  so the projected player location and projection remain stable while the
  effect grows. During spike `ReverseZoom`, projection changes as part of the
  existing route; avoid changing that projection or its gameplay timing as
  part of this visual refinement.
- The effect's visibility follows `DeathTicks`, not
  `PlayerDeathPresentation.ElapsedTicks`. This lets the current drawing appear
  in both death routes even though only ordinary `PlayerDying` advances the
  presentation clock. Preserve that existing distinction.

### Relationship to gameplay and retraction

- Ordinary `PlayerDying` lasts 60 simulation updates. At the end, one life is
  deducted. With remaining lives, `ResetAfterDeath` reconstructs unresolved
  enemies, retains destroyed outcomes/score and persistent Spikes, resets the
  player, and resets `DeathPresentation`. With no lives remaining, the game
  enters `GameOver`, clears runtime enemies, and does not reconstruct. Restart
  uses the existing fresh-game path.
- `EnemyRetractionProgress` is exposed only while `State == PlayerDying`.
  The renderer applies that progress as a render-only enemy-depth offset;
  logical enemy state/accounting remains frozen. Spikes are rendered
  independently and remain on the web.
- Keep the Blaster visual sample separate from `EnemyRetractionProgress`.
  Neither its shape nor its envelope should affect enemies, collision, score,
  lives, reconstruction, or state transitions.

### Existing automated coverage

`PlayerDeathPresentationTests` verifies ordinary death timing and retraction,
logical enemy immobility, Spike persistence, final-life Game Over, restart,
and shared projection behavior across Levels 1–3. Existing simulation and
Level 4 tests cover reverse-zoom life loss and related exit behavior. These
tests protect useful invariants, but currently do not inspect the Blaster
effect's colors, geometry, scale envelope, or disappearance.

## INCITTI REFERENCE

`Reference/incitti/tempest.bmx` is useful for understanding source
architecture, not for overriding the supplied arcade frames:

- `Player` stores `deathcount` and `deathtype`; its update decrements the
  countdown and deducts a life when it reaches zero. `Player.Draw` selects
  cause-dependent geometry while in `LEVEL_PLAYER_DYING`.
- For a Flipper death, the source scales the Blaster claw height with
  `deathcount`, which contracts the claw as the count falls. Bullet/Pulsar
  branches change its projected depth. These are source-specific
  cause-dependent Blaster treatments, not evidence for the supplied
  multicolor starburst.
- The Fuseball-specific branch draws randomized small rectangles around the
  Blaster location, with radius derived from the countdown, and intermittently
  switches to white. It is not a deterministic set of related vector
  outlines, and it should not be copied as the arcade effect.
- Incitti's countdown ownership and separation of update timing from
  `Player.Draw` are useful architectural context. Its cause-specific geometry,
  color behavior, random fragment positions, and global death/vortex
  transitions are not authoritative when they conflict with the observed
  frames.

## DIFFERENCES/GAPS

| Aspect | Current C# | Supplied arcade evidence |
|---|---|---|
| Overall structure | Eight simple, symmetric radial rays | Layered angular/vector starburst-like outlines |
| Color | One yellow color | Multiple outlines/colors; white, red/orange, and yellow prominent |
| Shape changes | One fixed radial pattern | Related outlines are not visually static |
| Size over time | Small-to-large monotonic expansion | Compact development, substantial expansion, contraction, small remnant, disappearance |
| Position and scene | Player-derived center; tube drawn behind | Develops at death location; tube remains visible |
| Fidelity status | Approximate recreation choice | Qualitative visual behavior is directly supplied; exact measurements remain unresolved |

The existing effect already has two useful properties: it starts at the death
location and it remains over the visible tube. Its main shortcomings are
missing layers/colors and the missing second half of the observed size
progression. Exact shape, color assignment/order, timing, maximum size, and
inter-outline motion remain undetermined.

## RECOMMENDED IMPLEMENTATION

1. Keep the existing death and life-transition semantics unchanged. Do not
   extend `PlayerDeathTicks`, alter the reverse-zoom route, or modify enemy
   retraction/reconstruction.
2. Replace the single expanding-ray drawing with a deterministic,
   render-only starburst sample: one stable death origin plus multiple related
   angular vector outlines, with white, red/orange, and yellow represented.
   The exact outline count and geometry should be treated as implementation
   choices until compared manually; do not claim them as measured arcade facts.
3. Use a single normalized death-age input and a small ordered envelope of
   scale/outline parameters to express growth, peak, contraction, remnant, and
   zero-size/disappearance. Interpolate between parameters rather than adding
   a hard-coded state machine for each conceptual phase. This keeps the
   progression deterministic and testable while leaving timing/curve values
   visibly tunable.
4. Prefer sampling from the existing `DeathTicks` countdown for the Blaster
   effect, because it is already the draw gate for both ordinary death and
   spike reverse zoom. Keep it independent from the enemy-retraction progress
   clock. If presentation data is added to a model, make it a pure sample of
   the countdown rather than introducing a second mutable timer. The effect
   must reach zero when the current 60-tick countdown ends.
5. Keep the effect at one death origin while its outlines change size/shape.
   For ordinary `PlayerDying`, the current player-derived projected center is
   stable because the player and projection are frozen. Any special handling
   for the changing reverse-zoom projection should preserve that route's
   established camera behavior and be decided by manual comparison, not by
   changing gameplay state.
6. Keep vector submission separate from simulation. A deterministic
   presentation sample or callback-based draw helper (consistent with
   existing headless renderer helpers) allows tests to inspect layer data
   without depending on framebuffer pixels.

The envelope's key values and exact transitions are unresolved visual
parameters. Select and refine them by comparing captured C# output with the
supplied sequential arcade frames; do not infer precise values solely from
frame descriptions or Incitti.

## AUTOMATED VALIDATION

Tests should assert lifecycle and deterministic presentation data, not
subjective pixel fidelity:

- The effect activates on both existing 60-tick death routes and begins from
  the recorded player death location; repeated reads at the same death age
  produce identical samples.
- Ordered samples show compact development, a larger maximum, later
  contraction, a nonzero smaller remnant, then zero/disappearance. Assert
  ordering and relative sizes, not arbitrary screen-coordinate thresholds.
- The represented layers include white, red/orange, and yellow in the
  intended portions of the envelope. If testable geometry data is exposed,
  verify the outlines are distinct/related and not all one static contour.
- The effect stops at the current countdown boundary. Ordinary death still
  deducts one life and reconstructs at the same update; final life still
  enters Game Over at the same update. Spike reverse zoom retains its current
  life-loss, projection-restoration, and retry timing.
- Existing enemy-retraction tests continue to verify render-only retreat,
  unchanged logical enemy state, persistent Spikes, reconstruction/accounting,
  and Game Over/restart. Confirm the new effect sample does not drive or
  otherwise couple to `EnemyRetractionProgress`.
- Avoid brittle assertions for arbitrary pixel coordinates or exact
  pixel-level likeness; those are manual visual acceptance criteria.

## MANUAL VALIDATION

Compare a C# capture of a representative Blaster death directly against the
supplied `frame_0041.png`–`frame_0065.png` sequence. Review:

- Overall angular/vector shape and layered appearance.
- White, red/orange, and yellow color impression and their relative presence.
- Expansion magnitude from compact beginning to broad presentation.
- Visible contraction after the large presentation.
- A smaller remnant that diminishes cleanly to disappearance.
- Stable location at the Blaster's death point while the outlines animate.
- Tube geometry remains visible behind the effect.
- Concurrent enemy retraction remains visible and independent; Spikes remain
  fixed on the web.
- Ordinary life reconstruction and final-life Game Over occur at their
  existing boundaries.
- No regression to score, lives, Super Zapper, level progression, firing,
  spinner movement, collision rules, rim pursuit, display sizing, or
  between-level approach behavior.

Do not treat unrelated white radial effects elsewhere in the arcade frames as
part of the Blaster effect during comparison.

## RISKS / UNRESOLVED VISUAL DETAILS

- Exact outline count, vector coordinates, topology, relative scale/rotation,
  color order, color transition timing, total phase durations, peak magnitude,
  and mathematical interpolation curve have not been established.
- Three color families are required by the observations, but mapping them to
  particular outline identities/phases is still a visual implementation
  choice.
- Existing C# life loss is fixed at 60 ticks (one second). The supplied frames
  establish progression but not a measured animation duration. Keep the
  gameplay boundary; if the visual timing cannot fit convincingly within it,
  obtain additional timing evidence before proposing a gameplay-duration
  change.
- The standard `PlayerDying` and spike `ReverseZoom` routes use different
  presentation clocks/projection behavior. Reuse the countdown for effect
  sampling without broadening enemy retraction or altering reverse-zoom
  semantics.
- Incitti's cause-specific claw movement and randomized Fuseball fragments
  visibly differ from the supplied target and must remain secondary reference
  material.
- A successful automated lifecycle/geometry test does not establish
  arcade-like visual fidelity. Manual sequential comparison is required.
