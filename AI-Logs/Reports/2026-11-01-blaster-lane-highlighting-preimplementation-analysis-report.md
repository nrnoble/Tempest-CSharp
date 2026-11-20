# Blaster-Lane Highlighting: Pre-implementation Analysis

## Disposition

**GO.** Implement as one bounded rendering checkpoint. This is a read-only analysis;
no production source, tests, tracked documentation, or Git history were changed.
The accepted between-level approach and its tuning remain untouched.

## Arcade observation

Neal's supplied observation establishes that the Blaster's lane is identified by
recoloring its two side boundaries all the way down the tube; early levels use
yellow, and the highlight follows rim movement. This is gameplay-readable
orientation, not just decoration. Atari's palette changes every 16 levels, so
yellow is not an established universal color.

## Incitti reference — REFERENCE VERIFIED

In `Reference/incitti/tempest.bmx`:

- `Player.Update` maintains `e_Index` and fractional `scl`, changes the edge when
  the rim position crosses an endpoint, then sets
  `theLevel.edges[e_Index].hasplayer = True`. `Edge.Draw` uses that flag, not
  `scl`, to mark the lane.
- `Edge.Draw` colors the two depth-running rails at `p1` and `p2` for that edge.
  Those are the two boundaries of the occupied lane; the near/far rim
  connections are separate lines. `theLevel.Draw` renders every edge twice for
  its two layers.
- For palette 0 (levels 1–16), `Color(COL_CLAW)` is RGB `(255,255,0)`. Incitti
  computes `current_color` from the level and maps `COL_CLAW` by palette:
  palette indices 1–5 map to green, blue, blue, red, and yellow respectively.
  This is **Incitti behavior**, not evidence of Atari's later-level highlight
  colors.
- Ordinary movement highlights one `e_Index` at a time. The fractional `scl`
  does not interpolate the lane color. On a strict endpoint crossing the edge
  index changes; at the endpoint itself it remains the source edge. No
  simultaneous source/destination highlight or animated Blaster lane pivot is
  present.
- Open/non-continuous boards clamp movement at the first/last edge instead of
  wrapping. The active edge is still marked by `e_Index`; its `p1` and `p2`
  remain its two side boundaries.
- Rendering precedence is explicit: an edge in the Pulsar `pulsing` branch
  uses pulse colors rather than the normal/player color; otherwise
  `hasplayer` selects `COL_CLAW` before the Super Zapper/normal tube color.
  Thus pulse coloring supersedes the Blaster color, while the Blaster color
  supersedes the Zapper flash. The bonus-display `bcol` override can also alter
  layer-zero coloring; C# has no corresponding effect.
- Lane selection itself depends only on the current player edge. The displayed
  color additionally depends on the global palette, per-edge Pulsar state,
  Zapper flash, and the legacy bonus-display override.
- During `LEVEL_BEGIN`, the level and player are updated and drawn, so the
  marker follows the player during the incoming-board presentation. In
  `LEVEL_PLAYER_DYING`, player updates/drawing continue and the lane marker
  remains. In `LEVEL_INTO_VORTEX`, `Player.Update`/`Player.Draw` are skipped;
  per-frame edge flags are cleared, so no lane remains highlighted. Gameplay
  drawing is suppressed for `LEVEL_COMPLETE`.

## Current C# geometry and rendering

- `Gameplay/Player.cs` stores `EdgeIndex` and continuous `EdgePosition`.
  `MoveAlongTube` crosses lanes using the level's topology-derived transition
  angle and updates the index at a boundary. The player has no active
  `DirectedLaneTransition`, source/destination pair, or animated corner pivot.
  `DirectedLaneTransition` is used by Flippers and Pulsars, not the Blaster.
- `LevelDefinition.Points` is an ordered polygon. Levels 1, 2, and 3 each have
  16 vertices and are continuous: respectively the circle, square, and plus.
  `Level.NextEdge(i)` is `(i + 1) % pointCount`. The plus board includes
  reflex vertices; they do not change lane adjacency.
- `Rendering/VectorRenderer.cs:DrawTube` projects each vertex at the near and
  far depths and submits one longitudinal rail per vertex, followed by near
  and far rim connections. Therefore lane `i` is bounded by the longitudinal
  rails at vertices `i` and `NextEdge(i)`. This is a topology/index rule, not
  a shortest-path, screen-coordinate, or corner-angle rule.
- The current renderer already uses `DrawTube(Action<Vector2, Vector2, Color>)`
  as a headless capture seam. Its rail endpoints are the projected near/far
  vertices; recoloring those submissions can preserve geometry, projection,
  and endpoints exactly. Rendering does not mutate the level points.
- `DrawTube` uses the approach depth offset for both tube ends. The approach
  update continues to accept player movement, and `DrawPlayer` projects from
  the same player index/position and offset. A highlight in the tube draw path
  will therefore track the distant incoming Blaster without changing approach
  state or perspective.
- The C# Pulsar lane overlay is rendered after `DrawTube` and submits its two
  longitudinal boundaries in pulse color (cyan/white). It naturally wins
  over an underlying lane color on an overlapping boundary. The Super Zapper
  currently changes the tube line color in `DrawTube`; the Blaster's two
  selected rails can retain their highlight while all other tube lines flash.
- Current `LevelConfiguration` entries for Levels 1–3 have `PaletteIndex: 0`.
  `PaletteIndex` is the available palette seam, but there is not yet a C#
  level-color table.

**Open-board caveat:** `LevelDefinition.PlayableLaneCount` distinguishes open
boards, as does Incitti's movement. However, the current C# player movement and
renderer use cyclic `NextEdge`/point-count wrapping. All three target boards
are continuous, so this does not block this feature; generic open-board
movement/rendering should be handled separately rather than silently expanded
as part of this checkpoint.

## Recommended lane and boundary rule

Use the Blaster's current `Player.EdgeIndex` as the sole lane selector. Color
exactly the two longitudinal rails at `EdgeIndex` and
`CurrentLevel.NextEdge(EdgeIndex)`. Keep the current index through the
fractional travel along that lane, including the exact endpoint; when
`MoveAlongTube` changes the index, highlight the new lane. Do not derive a lane
from `EdgePosition`, interpolate colors, highlight both edges, or inspect a
Flipper/Pulsar transition.

This agrees with Incitti's edge flag and the existing C# movement representation.
Level 2 corners and Level 3 convex/reflex corners require no special cases.

## Color and precedence policy

- Palette 0 / Levels 1–3: yellow. Incitti's early-palette value is exactly
  `(255,255,0)`; implementation should choose one centralized yellow value and
  test that exact value consistently.
- Normal tube: blue. Super Zapper: preserve the alternate flash on ordinary
  rails, but keep the Blaster boundaries yellow, matching Incitti's
  player-over-Zapper ordering.
- Pulsar overlay: pulse color wins on any overlapping boundary because the
  overlay is drawn after the tube; this matches Incitti's pulse-first branch.
  The other Blaster boundary remains yellow unless it too overlaps an active
  pulse boundary.
- Do not add a development-mode color override. Existing diagnostics and entity
  colors remain as they are; only the two tube rail submissions change.
- Keep palette selection independent of lane selection. A small centralized
  renderer-side color resolver keyed by the existing `PaletteIndex` is
  sufficient: palette 0 returns yellow now, and later palette entries can be
  supplied when their rules are established. Do not add palette cycling or
  hard-code yellow across multiple rendering branches.

## State visibility and approach

Show the marker during normal `Playing`, incoming `Approach`, and
`PlayerDying` (the reference retains the indexed lane while the death
presentation runs). Also retain it in the development-only active
`AvoidSpikes` state if the Blaster is still being controlled. Hide it from
`ExitDelay`/`ExitZoom` onward and in `GameOver`/`Complete` terminal
presentations. Incitti's closest exit analogue, `LEVEL_INTO_VORTEX`, clears
the marker and suppresses player drawing; its `LEVEL_BEGIN` does the reverse.
No C# gameplay-state change is needed.

The approach decision is supported: Incitti marks the player lane during
`LEVEL_BEGIN`, while C# allows movement in `Approach` and applies
`LevelApproach.DepthOffset` to the projected tube. The lane lines should use
the existing projected rails without a separate approach rule.

## Automated-test strategy

Use the existing `DrawTube` callback, without Raylib or reflection. A focused
renderer test group should:

1. Capture the tube submissions for Level 1, Level 2 square, and Level 3 plus;
   verify that only the rails at `i` and `NextEdge(i)` are yellow for a
   selected player lane and that all other rails/rim lines keep their colors.
2. Verify exact projected endpoints, finite geometry, stable line count/order,
   and unchanged level points.
3. Move the Blaster within a lane, to its exact boundary, and across it; verify
   source-at-endpoint then destination-after-index-change. Include ordinary
   movement around a Level 2 corner and Level 3 reflex/convex vertices.
4. Capture while `Approach` is active, move the player through public
   simulation input, and verify the highlighted rails follow at the
   approach-projected depths.
5. Verify the Zapper precedence (two yellow rails, remaining rails flashed)
   and Pulsar precedence (pulse color on overlapping boundaries, unchanged
   unrelated geometry).
6. Confirm development diagnostic modes do not change lane selection or tube
   geometry.

Existing focused patterns are in `Level2SquareTests.cs`,
`Level3GeometryTests.cs`, `LevelApproachTests.cs`, `SuperZapperTests.cs`, and
`SimulationTests.cs`. A dedicated `BlasterLaneHighlightTests.cs` would keep
the cross-board renderer behavior cohesive.

## Manual acceptance checklist

- **Normal play / Level 1:** both boundaries are yellow from rim to far end;
  spinner movement updates them; deep-tube targets remain legible.
- **Level 2:** side and corner lanes select the adjacent indexed boundaries;
  no premature or lingering source lane after a crossing.
- **Level 3:** verify plus-board convex/reflex lanes and 90/180/270-degree
  routes; no geometrically-shortest-path boundary selection.
- **Approach:** distant incoming tube shows both boundaries and tracks
  Blaster movement without changing approach timing, offset, or curve.
- **Interactions:** Zapper flash remains sensible; Pulsar lines visibly
  supersede the highlight only where they overlap; no flicker, adjacent-lane
  coloring, endpoint drift, or render crash.
- Check `PlayerDying`, exit, and terminal visibility against the policy above.
  Do not test future palette cycling.

## Uncertainties, files, and checkpoint

- The transition-frame rule is not an uncertainty: Incitti's `e_Index` flag
  establishes source through the endpoint and destination after the edge
  changes; C# exposes the same distinction through `EdgeIndex`.
- Neal's supplied observation and Incitti source support the feature without
  future footage. Manual visual acceptance is still needed for readability
  on the C# square/plus projections and simultaneous Pulsar/Zapper effects.
- Incitti's bonus `bcol` flash can temporarily override its lane color; C# has
  no equivalent, and no new bonus effect is proposed.
- Likely production change: `src/Tempest.Game/Rendering/VectorRenderer.cs`
  only, with lane/color selection kept in that rendering path or a small
  private helper. Do not change `Player`, `Level`, `Projection`,
  `LevelApproach`, or `TempestGame`.
- Likely tests: a focused `tests/Tempest.Game.Tests/BlasterLaneHighlightTests.cs`
  using the existing callback seam; no Raylib window required.

**Checkpoint recommendation:** one bounded rendering-and-tests checkpoint,
followed by automated tests and manual acceptance. No movement, transition,
approach, or palette-cycle milestone is a prerequisite.

**Conclusion: GO.**
