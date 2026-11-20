# Enemy-shot ROM/vector-rendering investigation recovery

Date: 2026-11-12
Disposition: **READ-ONLY INVESTIGATION COMPLETE; IMPLEMENTATION AND MANUAL ACCEPTANCE PENDING**

## Recovery, scope, and starting state

The prior investigation was interrupted by `408 Timed out reading request
body` near synthesis. Its reported conclusion was treated as a lead, not
proof. The prior errored session was identifiable, but its short summary did
not expose the unfinished detailed analysis. This report independently
reconstructs the necessary conclusions from bounded local source ranges; it
does not claim to recover every previous tool result or visual observation.

The user prompt's one standalone completion token was verified before
repository actions. Starting state: `master`, HEAD
`cd678572c009d27fa8e1e1f0059ec45f6db5db11` ("Complete accepted Level 9
behavior"), tracking `origin/master` at that same locally recorded commit.
`git status --short --branch --untracked-files=normal` reported only
`## master...origin/master`: no staged, tracked-worktree, or non-ignored
untracked changes. There was no supplied contradictory baseline.

No production code, tests, durable project documentation, screenshots,
recordings, ROM copies, or Git index were modified. `TEMP/myNotes.md` was not
read. The paired prompt and this new report are ignored `AI-Logs/` records.
No commit, push, game launch, test, or build was performed.

## Evidence examined

- Local ROM disassembly, `Reference/arcade/Tempest Rom Source code.md`:
  vector programs `$3a40-$3b02` (listing lines 1477-1563); shot selection
  `$b75a-$b785` (lines 7182-7209); `graphic_at_mid` and Mathbox scale
  `$bcfd-$bd9f` (lines 7482-7567); graphic table `$cec8-$ceee`
  (lines 9927-9946); and timer increments `$c8e3` and self-test `$dae5`.
  Vector addresses `$1a40`, `$1a72`, `$1aa2`, `$1ad4` correspond to listing
  addresses `$3a40`, `$3a72`, `$3aa2`, `$3ad4`.
- `src/Tempest.Game/Gameplay/EnemyBullet.cs`,
  `src/Tempest.Game/Rendering/VectorRenderer.cs` and
  `PlayerShotVectorProgram.cs`; bounded playback and active-update ranges in
  `Program.cs`, `TempestGame.cs`, and `Debug/GameplayPlaybackSession.cs`.
- `tests/Tempest.Game.Tests/Level1ProductionEnemyBulletRenderingTests.cs`,
  and relevant existing Level 2 rendering assertions.
- The November 8 Blaster-shot investigation and implementation reports,
  `TEMP/2026-11-08-blaster-shot-rom-rendering-closeout.md`, and the accepted
  player-shot section of `TEMPEST-BEHAVIOR-SPEC.md`; the November 9
  enemy-bullet visibility report.
- Short, bounded calculations checked the listing's short- and long-vector
  signed coordinate annotations against the displayed raw words and
  accumulated the relative coordinates. No discrepancy was reported for the
  four enemy-shot programs. No raw ROM file was opened.

## ROM-BACKED: selection and timing

`draw_shots` scans shot slots 11 down to 0 and skips zero positions. Slots
below 8 use the player-shot graphic; enemy slots 8-11 use:

```
offset = $20 + ((timectr << 1) & $06)
       = $20 + 2 * (timectr & 3)
```

Offsets `$20`, `$22`, `$24`, `$26` select graphic-table entries `$cee8`,
`$ceea`, `$ceec`, `$ceee`: pictures 1, 2, 3, 4 respectively. Thus all
enemy shots drawn with the same `timectr` value share the same picture;
selection is not a function of individual shot age, lane, or order. The
sequence repeats every four timer increments. The normal control code
increments the byte at `$c8e3`; self-test has a separate increment at
`$dae5`. A byte wrap preserves the modulo-four sequence.

**UNRESOLVED:** This bounded check does not establish exact real-time
frequency, display-list-to-timer phase, startup/reset alignment, missed-frame
behavior, or a literal one-ROM-timer-increment-to-one-C#-tick mapping.
Consequently do not claim a precise arcade Hz or animation duration. If the
timer changes between separate display-list constructions, they can select
different pictures; the selector itself is global.

`graphic_at_mid` takes lane midpoint and along-tube position, invokes
projection `$c098`, appends a computed external vector scale through
`$bd3e`, then emits the selected table entry. The picture program contains
no per-shot or lane rotation operation. Logical vector geometry is
screen-oriented; the lane/depth placement and external scaling are separate.
The exact Atari Mathbox/analog display mapping is not identical to C# pixel
coordinates merely because both use a projected midpoint.

## ROM-BACKED: reconstructed vector geometry

Each picture starts with `vstat z=12 c=0 sparkle=1`, draws four nonzero lit
short segments separated by blank beam moves, changes to
`vstat z=12 c=3 sparkle=1`, and emits four zero-displacement lit
`vldraw` instructions at positions reached by blank moves. Each ends in
`vrts`. Blank travel is not a visible connecting line. The four index-3
instructions are point-like marks, not long rays. Color *indices*, intensity
instructions, and sparkle flags are ROM facts; actual RGB and perceived
sparkle are not established by these words.

Coordinates below are cumulative **relative logical vector units** from the
entry beam position, X right and Y up, before external VScale or projection.
They are not pixels or measured CRT dimensions. Segment start/end order
preserves the ROM's draw order. In each row the dot order is also the ROM
order. Coordinates are deliberately not symmetrized.

| Picture | Index 0 lit segments: start -> end | Index 3 point positions |
|---|---|---|
| 1 | `(-11,11)->(-17,17)`; `(-17,-11)->(-11,-17)`; `(17,-17)->(11,-11)`; `(11,17)->(17,11)` | `(6,6)`, `(-6,6)`, `(-6,-6)`, `(6,-6)` |
| 2 | `(-18,18)->(-18,4)`; `(-8,-14)->(-8,-22)`; `(18,-12)->(18,-4)`; `(8,14)->(8,22)` | `(-3,7)`, `(-7,3)`, `(3,-7)`, `(7,3)` |
| 3 | `(-17,3)->(-23,-3)`; `(-3,-23)->(3,-17)`; `(17,-3)->(23,3)`; `(3,23)->(-3,23)` | `(0,8)`, `(-8,0)`, `(0,-8)`, `(8,0)` |
| 4 | `(-22,-8)->(-14,-8)`; `(4,-18)->(12,-18)`; `(14,8)->(22,8)`; `(-4,18)->(-12,18)` | `(-7,3)`, `(-3,-7)`, `(7,3)`, `(3,7)` |

For example picture 3's last index-0 word `$40dd` decodes to `(-6,0)`,
not an inferred diagonal. Picture 2's last blank movement before the fourth
point is `(+4,+10)`, so its final point is `(7,3)`, not an invented
symmetric counterpart. The listed geometry supports four distinct images; a
smoothly rotated single glyph would be a different model.

## EXISTING C# BEHAVIOR

`EnemyBullet.Advance` adds 3 height units and advances its per-object
`Rotation` by 15 degrees each simulation step. Current rendering projects
the active bullet's lane midpoint with depth
`Level.Position + Level.Depth - bullet.Height`, then emits eight full
diameter white lines, 45 degrees apart, rotated by that bullet's own
`Rotation`. The radius is
`6 / ((projectedDepth + Projection.ZOffset) / Projection.K)`.
The production colored overload uses white and the active bullet collection;
normal rendering is gated by game state. `Level1ProductionEnemyBulletRenderingTests`
assert this exact white eight-line output, midpoint, depth scaling and
simulation rotation; related Level 2 tests also assume eight lines.

The geometric mismatch is clear from source: eight full, intersecting,
per-object rotating diameters versus four timer-selected pictures of four
short disconnected segments plus four points. Existing white is the project's
current presentation, not evidence that the two ROM color indices are
identical. The November 9 visibility report establishes why the active
collection must remain the rendering authority; it does not establish arcade
fidelity for the radial shape.

## ARCADE/MAME-OBSERVED and evidence limits

The user reports that the interrupted agent inspected C# playback screenshots
and MAME/reference frames, but its detailed visual notes were not available
in the recovered session summary. This recovery did not independently
inspect an identifiable enemy-shot frame and therefore does **not** assert
any enemy-shot color, exact pixel shape, motion, timing, or discrepancy as
visually observed. The November 8 report's MAME observations concern
Blaster shots and a separate destruction burst; they cannot establish
enemy-shot appearance. No direct original-arcade observation is claimed.
There is consequently no independently verifiable ROM-versus-enemy-shot
visual disagreement to resolve in this report.

## PROJECT TRANSLATION / RECOMMENDATION

The accepted Blaster-shot work is an appropriate *methodological* precedent:
decode actual relative vector instructions, respect blank versus lit
operations and color-index provenance, use a small testable vector program,
convert logical Y-up to screen Y-down, retain the established midpoint and
depth projection, explicitly label scale and pixel translations, protect
simulation and playback, then require Neal's manual visual acceptance.
The accepted Blaster's 17 static points, its 4.5 normalization and its amber
and red-orange RGB translations are specific to that shot and must not be
copied as enemy-shot ROM facts or treated as authorization to change color.

A later, separately authorized **presentation-only** milestone should:

1. Encode the four verified enemy programs as four short segments and four
   points each, preserving their coordinates, draw/blank distinctions and
   ROM color-index metadata. Keep the existing active-collection visibility,
   midpoint, depth and gameplay state. Use project-space normalization and
   point rasterization explicitly as translations, not literal ROM pixels;
   preserve existing white output under the present no-color-change
   constraint. Any distinct RGB mapping needs separate authorization.
2. Select a single shared deterministic frame phase, not per-bullet
   `Rotation` or wall-clock/render-call count. One phase advance per
   appropriate simulation step is a **candidate translation**, not a
   ROM-proven physical cadence; choose and document initial phase, step
   mapping and reset/freeze behavior after examining existing simulation and
   playback lifecycles. Preserve deterministic pause, forward stepping and
   reverse reconstruction; do not casually reuse another mechanic's clock
   or alter its behavior. Preserve `EnemyBullet.Rotation` simulation state
   and recording/playback schema unless separately authorized.
3. Replace geometry-specific tests that require the old eight lines; add
   focused checks for all four raw-derived frames, blank moves, four segments
   and four points, color preservation, global synchronization for bullets
   of different ages, depth/midpoint behavior, visibility, deterministic
   pause/step/reconstruction and unchanged projectile lifecycle. Run the
   smallest relevant projectile/rendering/playback regressions.
4. Do not change bullet creation, speed, trajectory, firing, collision,
   scoring, sound, expiration, enemy mechanics, level progression, difficulty
   or gameplay timing. This is not a gameplay rebalancing milestone.

Scale, screen rasterization, visual phase/cadence translation and CRT sparkle
remain candidates requiring evidence and manual review, not accepted
behavior. White-only rendering cannot reproduce potentially distinct ROM
index colors; this is an explicit current constraint rather than a claim of
equivalence. Do not silently relax the no-color-change instruction to solve it.

## Validation and handoff

**PASS:** bounded source review and raw-word coordinate consistency checks;
starting Git state checked. **NOT RUN / NOT APPLICABLE:** automated test,
build, and interactive play, since this investigation changes no production
implementation. Historical test results in the cited reports are not new
validation for this milestone. **MANUAL ACCEPTANCE PENDING:** no new
enemy-shot rendering candidate exists and no visual acceptance is claimed.

For a future candidate, a relevant manual diagnostic is:

```powershell
dotnet run --project src\Tempest.Game -- --enemy-bullets-test --lives 10
```

Neal should compare multiple simultaneous bullets, near/far scaling,
animation, unchanged white color and gameplay against appropriate reference
footage, plus pause/forward/reverse playback where a suitable recording
exists. Copilot must not launch the game for acceptance. Record any reference
disagreement or unresolved visual mapping rather than promoting inference to
ROM fact.
