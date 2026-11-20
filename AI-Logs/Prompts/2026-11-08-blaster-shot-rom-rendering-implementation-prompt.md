FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Implement a tightly bounded, manually testable candidate that replaces the
current C# Blaster-shot rendering with a ROM-derived translation of the arcade
player-shot vector picture.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

4ccb3cfb6cf3a1e72ac47bcfc79454ce257cb9de
Close out accepted Level 5 no-direct-Flipper correction

Confirm the actual Git state before modifying anything.


PRIMARY INVESTIGATION REPORT

Read this report fully before designing or modifying the implementation:

AI-Logs/Reports/2026-11-08-blaster-shot-rom-visual-investigation-report.md

Reuse its ROM and visual findings rather than repeating the complete
investigation.


LOCAL ROM AUTHORITY

Use the project-local ROM-derived annotated Tempest disassembly:

Reference/arcade/Tempest Rom Source code.md

This local source has been used repeatedly in previous Tempest milestones.

Do not search the internet for another Tempest disassembly.

Do not use Mark Incitti's implementation, another port, or a generic projectile
design to establish the shot shape or color.

Recheck the relevant ROM routines, tables, graphic pointers, and vector data
directly before implementing the translation.


VISUAL REFERENCE FRAMES

The arcade/MAME reference frames remain available at:

Videos/FramesBlasterShots/

Do not modify, rename, move, compress, or stage these files.

The prior investigation inspected 41 readable frames. Use the images only as
supporting visual evidence.

Do not derive exact ROM coordinates, update timing, vector scale, or RGB values
solely from captured pixels.

Do not treat the bright white radial impact effect visible in some frames as
part of the Blaster shot. The accepted common type-0 enemy-destruction effect
is a separate object and display pass.


ESTABLISHED ROM FINDINGS

The investigation traced these player-shot rendering facts:

- The player-shot graphic table entry is at $ced0.
- The selected player-shot vector picture begins at vector address $1594.
- The corresponding vector program begins at listing address $3594 and ends
  with vrts at $3620.
- draw_shots at $b75a selects graphic-table offset $08 for all player shots.
- graphic_at_mid at $bcfd places the shot at the projected lane midpoint and
  supplies the external vector scale.
- The static vector picture uses sparkle-enabled point-like marks.
- Visible connecting line segments are not drawn between point positions.
- The first color group uses ROM color index 8.
- The second color group uses ROM color index 1.
- The first group contains nine illuminated point instructions.
- The second group contains eight illuminated point instructions.
- The first group includes the origin and a compact point ring.
- The second group forms a larger point pattern.
- The picture does not vary according to shot slot, shot age, or depth.
- Depth changes the externally supplied scale, not the picture coordinates.
- The shot remains screen-oriented.
- The shot does not rotate with the lane.
- The shot does not rotate as it ages.
- The apparent dotted trail during rapid fire is primarily several independent
  active shots at different depths.
- Enemy destruction is drawn separately after collision.

Recheck these findings against the local ROM listing while implementing.

If the relevant local listing reveals a discrepancy, stop and document it
before building an implementation based on an uncertain interpretation.


IMPORTANT COORDINATE QUALIFICATION

The prior investigation determined that the first group’s readable relative
moves are:

- (0, 0)
- (+7, 0)
- (-2, +5)
- (-5, +2)
- (-5, -2)
- (-2, -5)
- (+2, -5)
- (+5, -2)
- (+5, +2)

The second group begins with readable relative movement including:

- (+10, +5)
- (-4, +11)
- (-11, +4)
- (-11, -4)

The annotated listing marks the final four outer movement coordinates with
question marks, and its decoded values and inline comments may disagree.

Before implementing the second group:

1. Re-read the raw vector words and the local disassembly’s vector decoding
   conventions.
2. Resolve the final four movement coordinates as confidently as the local ROM
   material permits.
3. Distinguish raw-word decoding from informal or uncertain comments.
4. Record the exact final coordinate derivation in the implementation report.
5. If the raw ROM words establish the coordinates, use the raw-word decoding.
6. If the local information genuinely cannot resolve them, implement only the
   narrowest defensible translation and label the uncertain portion explicitly.

Do not invent a symmetric outer ring merely because it looks plausible.

Do not ask image-generation or generic drawing tools to design the shot.


CURRENT C# RENDERING TO REPLACE

The current project draws each player shot as:

- eight full-diameter visible lines;
- 45 degrees apart;
- centered at the projected shot position;
- hard-coded white;
- rotated by the shot’s cosmetic Rotation value;
- scaled using the existing project-space base size of 4.5 and reciprocal
  perspective.

That rendering is a previously accepted project decision, but it is now proven
not to match the ROM player-shot picture.

Replace only that visual representation.

Do not change gameplay merely because the Shot class currently advances a
cosmetic Rotation value.


IMPLEMENTATION SCOPE

Implement a rendering-only translation of the ROM player-shot vector program.

The corrected C# presentation should:

1. Draw the ROM-derived point positions rather than eight radial lines.

2. Preserve the division into:
   - nine point positions using the project translation of ROM color index 8;
   - eight point positions using the project translation of ROM color index 1.

3. Represent each illuminated zero-displacement vector instruction as a
   compact point-like/sparkle mark.

4. Preserve blank movement semantics:
   - blank moves reposition the virtual beam;
   - blank moves must not become visible connecting lines.

5. Preserve the static screen-oriented picture:
   - do not rotate it according to lane;
   - do not rotate it according to age;
   - do not vary coordinates according to shot slot.

6. Continue placing and scaling each shot through the existing accepted lane
   midpoint/depth projection initially.

7. Keep the existing project-space 4.5 value temporarily as a conversion or
   compatibility scale unless a smaller, equally bounded representation is
   necessary to translate the ROM coordinates.

8. Clearly separate ROM-derived coordinates from C# project-space scaling.

9. Make the two ROM color-index translations explicit and maintainable.

10. Preserve deterministic rendering and compatibility with paused playback,
    forward stepping, reverse reconstruction, headless playback, and screenshot
    capture.


POINT / SPARKLE TRANSLATION

The ROM uses sparkle-enabled zero-displacement illuminated vectors. The existing
C# renderer may not have a literal vector-hardware sparkle primitive.

Choose the smallest deterministic C# translation that visually represents these
as compact point-like marks without turning them into connected lines or large
radial stars.

Requirements:

- Do not use wall-clock randomness.
- Do not use nondeterministic random-number generation.
- Do not consume the gameplay RNG stream.
- Do not make point positions vary by shot age unless ROM evidence establishes
  such variation.
- Do not add a general-purpose particle system.
- Do not reuse the enemy-destruction effect pool.
- Do not emulate phosphor persistence or MAME post-processing.
- Keep the shot recognizable at the project’s normal window size.
- Keep the translation structurally faithful enough that each ROM point group
  and point location can be tested.

If a tiny deterministic cross, short micro-segment, or existing point primitive
is required because a zero-length submitted line is not visible in the current
renderer, classify that as PROJECT TRANSLATION and keep it visually compact.

Do not recreate the previous eight-spoke star in miniature.


COLOR TRANSLATION

The ROM establishes color indices 8 and 1, but the project does not yet have a
verified Atari RGB mapping for these player-shot indices.

Use the MAME frames only as visual guidance.

The examples support a small warm-colored presentation with visible
red/orange/yellow-like components, but captured pixels do not establish literal
ROM RGB values.

Before selecting project colors:

1. Search the local project for any existing use or translation of ROM color
   indices 8 and 1.
2. Determine whether any such translation is already arcade-accepted,
   ROM-derived, or merely a project choice.
3. Avoid misusing the type-0 destruction effect’s separate color-index-0
   translation.
4. Avoid treating unrelated Super Zapper or Game Over colors as proof.

If no authoritative mapping exists, choose explicit provisional C# colors that
best support manual comparison with the supplied MAME frames.

Document them as:

PROJECT TRANSLATION / MANUAL ACCEPTANCE PENDING

Do not describe provisional RGB values as literal Atari palette facts.


SIZE AND PROJECTION

The ROM uses an external Mathbox-derived vector scale. The local listing does
not directly specify a C# pixel radius.

For this candidate:

- preserve the existing lane midpoint/depth projection;
- preserve monotonic depth scaling;
- preserve the qualitative behavior that shots appear smaller deeper in the
  tube;
- retain 4.5 as a project-space conversion starting point where practical;
- reinterpret 4.5 as a compatibility scale for the ROM point coordinates,
  not as the radius of a circular or radial shot;
- do not change shot gameplay depth or movement.

If direct insertion of ROM coordinate magnitudes with the existing 4.5 scale
would produce obviously invalid or enormous output, derive a narrowly scoped
normalization from the ROM coordinate extent and document it.

Do not tune the size extensively in this prompt.

Neal will manually compare the result and request a later adjustment if needed.


COSMETIC ROTATION STATE

The ROM picture does not rotate by shot age.

Stop using Shot.Rotation to transform player-shot rendering.

Prefer the smallest safe change:

- rendering may simply ignore Rotation;
- remove Rotation production state only if doing so is clearly safe, bounded,
  and improves maintainability;
- do not alter gameplay state, recording format, collision, or deterministic
  input processing merely to remove an obsolete cosmetic value.

If Rotation remains in Shot temporarily, document that it no longer affects
rendering and avoid expanding this milestone into unrelated cleanup.


MUST REMAIN UNCHANGED

Do not change:

- player firing input;
- firing cadence;
- shot cooldown;
- shot capacity;
- shot-slot allocation;
- shot lane assignment;
- shot starting depth;
- shot movement speed;
- Spike-slowed shot behavior;
- collision detection;
- collision radius;
- enemy-hit behavior;
- enemy-shot-hit behavior;
- Spike erosion;
- scoring;
- sounds;
- enemy destruction presentation;
- shot removal;
- death clearing;
- exit clearing;
- Spike-exit shot simulation;
- recording format;
- playback input format;
- pause/resume;
- forward stepping;
- reverse scrubbing;
- screenshot capture;
- tube rendering;
- enemy/projectile rendering;
- Blaster rendering;
- level behavior;
- supply or progression;
- Super Zapper;
- HUD;
- Levels 8 or 9.

Do not change the C# ten-shot capacity, three-tick cooldown, or movement of six
height units in this rendering milestone, even though these are project
translations rather than ROM values.


RENDERING ARCHITECTURE

Prefer a small data-driven representation of the translated ROM point program.

Potential structure:

- immutable relative point/move definitions;
- explicit color-group identity;
- deterministic accumulation of relative coordinates;
- renderer conversion from logical ROM point coordinates to projected
  project-space positions.

Do not draw the program by copying and pasting 17 ad hoc renderer calls if a
small static data representation is clearer and easier to verify.

Do not introduce an elaborate vector-program interpreter unless the existing
project architecture already provides a suitable small abstraction.

The renderer must not mutate shot simulation state.


REQUIRED AUTOMATED TESTS

Add or update focused rendering tests proving at least:

1. SHAPE
   - One player shot produces the expected ROM-derived point groups.
   - The first group contains nine point positions.
   - The second group contains eight point positions.
   - Blank movement vectors do not create visible connecting lines.
   - The old eight full-diameter radial lines are no longer submitted.

2. COORDINATES
   - The translated logical coordinates match the resolved ROM point program.
   - Relative movement accumulation is correct.
   - The report/test names identify any qualified uncertain coordinates.
   - No invented symmetry is hidden in the implementation.

3. COLORS
   - Group 1 uses the explicit project translation of ROM color index 8.
   - Group 2 uses the explicit project translation of ROM color index 1.
   - Both groups are present for one shot.
   - Rendering does not collapse the shot to hard-coded white.
   - Tests classify RGB selections as project translations, not ROM facts.

4. STATIC PRESENTATION
   - Shot age does not rotate or otherwise alter point coordinates.
   - Shot slot does not alter geometry or color groups.
   - Lane selection changes placement but not picture orientation.
   - Repeated rendering of equal shot state is deterministic.

5. DEPTH SCALING
   - Near, middle, and far shots preserve the same logical picture.
   - Scale remains finite and monotonic.
   - Deeper shots appear smaller according to the existing projection.
   - No coordinate produces NaN or infinity.

6. MULTIPLE SHOTS
   - Two active shots produce two independently projected point programs.
   - The apparent trail is represented by independent active shots, not a
     renderer-generated line or trail between them.

7. IMPACT SEPARATION
   - Shot rendering does not emit the enemy-destruction radial effect.
   - Enemy-destruction rendering remains separate and unchanged.

8. GAMEPLAY ISOLATION
   - Existing firing-limit, cooldown, movement, expiry, collision, scoring,
     Spike interaction, exit, and death tests remain unchanged and passing.
   - No gameplay-recording schema change occurs.

9. PLAYBACK
   - Equal playback boundaries produce equal rendered shot submissions.
   - Pause does not advance shot state.
   - Forward stepping advances normal gameplay state.
   - Reverse reconstruction reproduces prior shot positions/rendering.
   - Headless playback remains valid.

Prefer geometry/submission tests over brittle pixel comparisons.


VALIDATION

During implementation, run the narrowest player-shot rendering tests first.

Before stopping, run expanded focused regression coverage including:

- PlayerShotRenderingTests;
- GameplayInputTests;
- SimulationTests relevant to player shots;
- collision tests;
- Spike interaction tests;
- enemy-destruction effect tests;
- rendering tests;
- gameplay recording/playback tests;
- headless playback;
- visible playback;
- pause/step/scrubbing tests;
- screenshot capture tests where relevant.

Then run:

- the full solution test suite;
- the full solution build;
- git diff --check.

Inspect the final diff carefully.

Confirm that no gameplay constants or behavior changed accidentally.

Verify all files under:

Videos/FramesBlasterShots/

remain unchanged.

Verify screenshot000.png through screenshot013.png remain untouched and
untracked.

Verify UserData, gameplay recordings, AI-Logs, and other generated/local files
are not staged or modified.


DOCUMENTATION

Update durable project documentation appropriately:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

README.md only if genuinely necessary.

Document:

- ROM-derived `$1594` static sparkling two-color multi-point shot;
- the nine-point index-8 group;
- the eight-point index-1 group;
- no visible connecting segments;
- no age-based or lane-based rotation;
- external depth scaling;
- explicit C# RGB translations;
- any coordinate qualification still unresolved;
- existing gameplay behavior preserved;
- manual arcade/MAME acceptance pending.

Preserve evidence classifications:

- ROM-LISTING-BACKED;
- ARCADE/MAME-OBSERVED;
- PROJECT TRANSLATION;
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR;
- MANUAL ACCEPTANCE PENDING;
- UNKNOWN / UNRESOLVED.

Do not mark the visual result as accepted before Neal tests it.


GIT / STOP POINT

Do not stage, commit, or push.

Leave the complete candidate unstaged for Neal's manual visual acceptance.

Do not delete, rename, move, or stage unrelated/generated files.

Preserve this complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired date-prefixed implementation report under ignored
AI-Logs/Reports.

The implementation report must include:

1. Starting Git state.
2. Investigation report reused.
3. Local ROM regions and raw vector words rechecked.
4. Final resolved point coordinates and derivation.
5. Any remaining coordinate uncertainty.
6. Current C# renderer removed/replaced.
7. New rendering representation.
8. Project translations for ROM colors 8 and 1.
9. Scale/normalization translation.
10. Rotation handling.
11. Files changed.
12. Automated tests added/updated.
13. Focused test results.
14. Expanded regression results.
15. Full suite result.
16. Build result.
17. git diff --check result.
18. Proof that gameplay behavior/constants were preserved.
19. Proof that reference frames and other local files were untouched.
20. Final Git state.
21. Direct launch/record/playback commands.
22. Manual acceptance checklist.
23. Remaining unresolved issues.


MANUAL ACCEPTANCE HANDOFF

Do not launch the interactive game.

Provide Neal practical commands using existing project options for:

- direct normal play;
- creating a short new gameplay recording;
- playing that recording back for pause/forward/reverse inspection.

Do not rely on an older recording if this rendering-only change does not alter
simulation state, but note that a fresh short recording may be easier for
isolating shots visually.


MANUAL ACCEPTANCE CHECKLIST

Prepare a concise checklist covering:

1. Fire isolated single shots.
   - Each shot should appear as a tiny compact multi-point sparkle.
   - The old large white rotating radial star should be gone.
   - No visible connecting lines should appear between point locations.

2. Hold Fire.
   - Several independent shot pictures should create a dotted trail down the
     lane.
   - The renderer must not draw one continuous trail.

3. Compare colors with MAME.
   - Two warm color groups should be visible where scale permits.
   - Record whether either provisional project color needs adjustment.
   - Do not treat monitor/MAME phosphor effects as literal RGB evidence.

4. Inspect near, middle, and far positions.
   - Shots should shrink smoothly as they travel inward.
   - The shape should remain compact and recognizable.
   - Record whether the 4.5 compatibility scale appears too large or too small.

5. Move across different lanes.
   - The shot picture should remain screen-oriented.
   - The picture should not rotate with lane direction.

6. Pause and step frame by frame.
   - The shot geometry should not rotate or animate by age.
   - Forward and reverse stepping should show deterministic positions.
   - Multiple shots should remain distinguishable where spacing permits.

7. Destroy an enemy.
   - The player shot should disappear on impact.
   - The accepted white type-0 enemy-destruction effect should remain visibly
     separate from the shot.

8. Check gameplay.
   - Firing cadence, shot count, collisions, score, sound, Spike erosion,
     death, exit, and progression should feel unchanged.

Stop after automated validation and manual-test handoff.

Do not begin color/size retuning, firing changes, enemy-shot rendering, or
broader renderer work.

TEMPEST_BLASTER_SHOT_ROM_RENDERING_IMPLEMENTATION_20261108