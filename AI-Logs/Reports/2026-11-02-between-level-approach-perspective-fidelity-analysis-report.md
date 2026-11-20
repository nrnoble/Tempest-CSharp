# Between-level Approach Perspective-Fidelity Analysis

## CURRENT CHECKPOINT / STATUS

- HEAD is `15933a85f8ac7e9b1390a5436d092d96774c3213` (`15933a8 Refine accepted Blaster death animation`).
- The working tree has a pre-existing `.gitignore` modification. It was left untouched. No source, tests, tracked documentation, index, or commit was changed for this analysis.
- The existing between-level approach is already accepted as functionally good. Its architecture is not in question; this is a bounded visual-fidelity refinement.
- The previous implementation report records manual acceptance of the approach and its continuity properties. The current task's observation refines that acceptance: the remaining difference is the *impression of viewpoint travel*, not a broken transition.
- No tests were run. This analysis creates only this requested report.

## ARCADE OBSERVATION

Neal's direct arcade observation is authoritative: the approach feels as though the player/viewpoint travels through space toward the next tube from an elevated or above angle. It does not feel merely as though a distant tube is moving toward the player from above. Neal estimated the apparent angle qualitatively at roughly 30–45 degrees; this is not a measured camera angle or a numeric implementation target.

The previously accepted visible properties remain requirements: outgoing ExitZoom completes smoothly; the next tube starts distant and grows; the Blaster remains visibly attached to its rim and spinner-movable; gameplay does not start early; and arrival has no projection flash, wrong board, or Blaster jump. Preserve natural progression, input behavior, and the 1280x960 presentation.

## CURRENT C# PRESENTATION

### State and lifecycle

`LevelApproach` is a small deterministic model with a 120-tick duration and a 240-unit initial depth offset. Its offset follows a smoothstep curve to zero (`src/Tempest.Game/Levels/LevelApproach.cs`). These are provisional project presentation values, not arcade measurements.

Natural progression commits the incoming configuration only after successful ExitZoom travel reaches the existing completion boundary. `InitializeFreshWave(..., beginApproach: true)` installs the new level, restores the standard projection, and creates the approach model (`src/Tempest.Game/TempestGame.cs`, `InitializeFreshWave` and `UpdateExitZoom`). In `RunOneStep`, Approach is handled before normal simulation. `UpdateLevelApproach` updates only the existing player rim movement and advances the timer; the completing update changes state to Playing and returns, so ordinary gameplay begins on the following simulation step. Direct development starts do not use this natural-progression path.

### Projection and apparent motion

The shared projection is a 2D perspective transform: `x/z` and `(y + yOffset)/z`, scaled by `K`, with `ZOffset` keeping the near plane positive (`src/Tempest.Game/Rendering/Projection.cs`). Fresh gameplay uses `K = 70`, `ZOffset = 5`.

During Approach, `VectorRenderer.DrawTube` adds the same `ApproachDepthOffset` to the near and far tube depths. The offset therefore moves both rims along the fixed projection axis: it starts at 240 and eases to zero, making the same tube progressively larger. The level's `CenterY` and `YOffset` do not change. There is no approach-only pitch, vertical framing drift, or depth-dependent vertical parallax.

The Blaster edge is projected at `PlayerProjectedDepth + ApproachDepthOffset`; its claw height is scaled to stay visually consistent with that rim. The shared input movement path preserves spinner, keyboard, and controller movement around the incoming rim. Incoming wave simulation is frozen; ordinary enemy bodies and wave HUD details are hidden. The renderer's existing approach-visible geometry, including the board, Blaster, and any rendered spikes, remains under its current rules.

The render uses logical 800x600 coordinates and a uniform 1.6 presentation scale to 1280x960 (`Rendering/DisplayConfiguration.cs`). The transition does not change this scale or the gameplay projection constants.

### Existing automated coverage

`LevelApproachTests.cs` already protects natural entry, incoming-wave freeze, fire/Zapper suppression, spinner/keyboard/controller movement, finite monotonic transition progress, exact arrival, player-rim continuity, and the first gameplay update. Progression/configuration and simulation tests cover the broader transition and campaign behavior. Display tests protect the 800x600-to-1280x960 scale and standard projection.

## INCITTI REFERENCE

The read-only BlitzMax reference also uses the existing perspective projection rather than a general 3D camera. `LEVEL_BEGIN` starts with level position 1500 and moves it toward 50 in 10-unit steps. It also adjusts global `CCenterY` as position changes (`Reference/incitti/tempest.bmx`, `Level.Update`, around lines 1313–1322). Its `TForm` projection uses the same basic depth-scaling form (`Reference/incitti/transformfunctions.bmx`). The incoming tube therefore grows while its vertical framing also moves; the player glyph is drawn at the incoming rim and grows from its approach-specific small presentation.

This is useful evidence that a small projection/framing change can accompany approach depth animation without adding a 3D renderer. It does not establish the correct Atari angle or target curve. Incitti's approach is visually sharper, and its gameplay/update behavior is not authoritative where Neal's observation differs.

## WHY THE PERSPECTIVE DIFFERS

The present C# transform changes only depth. At every tick it applies the same fixed projection center and vertical offset to a tube whose near and far rims retain their existing relationship. This reliably communicates “the tube is getting larger as depth closes,” but supplies no separate cue that the viewing position is elevated or traveling through space. Consequently, the current result can read as the tube itself approaching the player, even though its state/lifecycle and rim-attached Blaster are correct.

An elevated impression does not require literal 3D. A small, time-varying vertical framing/parallax term in the existing projection can make the near and far parts of the tube move differently on screen. That differential cue can imply a changing viewpoint more effectively than uniform axial scaling alone. A uniform screen-space shift of the whole board is less promising: it may simply look like the tube is sliding vertically.

## MINIMAL REFINEMENT OPTIONS

1. **Keep the accepted presentation.** This remains a valid choice if Neal judges the current difference minor or if a small render-only experiment does not improve the Atari resemblance. No architecture work is warranted merely to chase an unperceived distinction.
2. **Try one approach-only depth-dependent vertical bias (recommended experiment).** Add a small render-only vertical/parallax term to the existing projection of all approach-visible geometry, including tube and rim-attached Blaster. Tie its strength to the existing eased approach progress, and make it exactly zero at arrival. Tune the sign and amplitude visually with Neal; do not convert the qualitative 30–45-degree estimate into a numeric camera parameter.
3. **Try a simple transition-only center-Y shift.** This is mechanically smaller and resembles Incitti's moving `CCenterY`, but shifts geometry uniformly and may reinforce the undesired “tube moving” interpretation. Use only if a quick visual comparison shows it helps.

None of these options needs changes to progression, logical coordinates, `Projection.K`, `Projection.ZOffset`, the display scale, or the accepted LevelApproach lifecycle. Do not add a general 3D camera, a new rendering system, or a broad projection rewrite.

## RECOMMENDED APPROACH

If Neal wants a refinement trial, keep the current depth curve, duration, and lifecycle, and introduce only a subtle, render-only, depth-dependent vertical perspective bias that fades to zero with the existing approach offset. Prefer a shared projection helper or the existing projection inputs over new global mutable camera state. Apply the same rule to the entire incoming presentation so the Blaster remains registered to the rim; include any other geometry still visible during approach.

The aim is only to make the view feel as if it is moving toward the tube from above. The implementation should be rejected if the result looks like the board is sliding, if it weakens the accepted tube growth, or if Neal prefers the existing presentation. Keep the current approach as the fallback; this is a visual experiment, not a requirement to alter an already accepted feature.

## AUTOMATED VALIDATION

Automated tests can protect determinism and continuity, not subjective arcade fidelity. If the experiment is implemented:

- Test that the new vertical term is finite, bounded, and deterministic at start, intermediate progress, and arrival; it must be exactly zero at arrival.
- Capture tube and Blaster projection at those points. Verify that both use the same transition transform, that rim attachment is preserved, and that the ordinary gameplay coordinates exactly match the existing projection at arrival.
- Assert the approach's depth offset remains monotonic and the new term does not change the existing depth/size progression.
- Retain existing lifecycle/progression tests: natural entry only after successful exit; incoming wave frozen; no early gameplay; movement behavior preserved; current level and board stay correct; arrival starts ordinary simulation on the next step.
- Retain display assertions for 800x600 logical rendering, 1.6 scale, and 1280x960 output. Do not assert an “arcade angle” or claim a geometry test proves visual fidelity.

No test changes or implementation were made in this analysis.

## MANUAL VALIDATION

Compare the currently accepted build with the candidate using the same display/window settings and natural progression. Neal should judge the result against actual arcade Tempest/MAME and his direct observation, not a numeric angle:

1. Observe the existing approach, then the candidate, preferably across both Level 1→2 and Level 2→3 (and another natural edge if convenient). Focus on the *impression of movement*: does it feel more like the viewpoint/player is traveling toward the tube from above, rather than the tube moving toward the player?
2. Confirm the tube still begins distant and grows smoothly; its shape and vertical motion should not look distorted or like a board sliding on screen.
3. Turn the spinner throughout the approach. Confirm the Blaster remains visibly attached to the approaching rim and responds normally.
4. Confirm the incoming board is correct, no gameplay begins prematurely, and normal play begins only after arrival.
5. Watch the ExitZoom handoff, first distant frame, final arrival, and first gameplay frame for a projection flash, wrong board, Blaster jump, discontinuity, or changed display scaling. Recheck accepted score/lives and progression behavior if anything in gameplay appears affected.
6. Neal decides whether the resemblance is convincingly better. If not, retain the accepted presentation.

## RISKS / COMPROMISES

- The arcade angle is qualitative; an overlarge or wrong-sign bias could make the tube look distorted or moving independently rather than improve viewpoint travel.
- A projection adjustment could detach the Blaster visually if it is not applied identically to the rim and player geometry.
- Any nonzero residual at arrival could produce a visible snap. Exact zero at the normal gameplay boundary is mandatory.
- Tests can establish geometry and lifecycle invariants but cannot decide whether the result resembles Atari.
- Incitti offers an engineering precedent, not a target. Do not copy its sharper presentation or infer a measured Atari angle from it.
- If the one-parameter visual experiment is not clearly beneficial, stop and retain the current accepted approach rather than adding complexity.
