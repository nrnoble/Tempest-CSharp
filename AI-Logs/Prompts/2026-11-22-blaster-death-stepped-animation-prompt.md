Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, visual assets, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the user's token do not invalidate it.

This check is intended to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Implement Atari-informed stepped Blaster-death animation

Implement a tightly bounded rendering-only refinement of the shared Blaster death/explosion presentation.

Use these completed reports as the factual handoff:

AI-Logs/Reports/2026-11-22-blaster-death-visual-animation-readonly-report.md

AI-Logs/Reports/2026-11-22-blaster-death-rom-selector-verification-report.md

Do not repeat the visual investigation or ROM selector investigation.

Neal explicitly authorizes mapping the ROM’s relative stage holds onto the existing 60-tick C# death-presentation clock as a PROJECT TRANSLATION.

## Objective

Replace the current smoothly interpolated Blaster-death radius envelope with a deterministic stepped visual schedule informed by the verified Atari enemy-shot death sequence.

Preserve the accepted C# death effect as the visual foundation:

- current explosion center;
- existing three-layer yellow/orange-red/white starburst;
- existing asymmetric 16-point layer geometry;
- existing layer rotations;
- accepted 150% peak size;
- existing 60-tick death-presentation duration;
- shared legacy and Atari-presentation renderer;
- deterministic behavior;
- clean disappearance at the end.

Do not replace the effect with screenshot-traced geometry.

Do not add independent moving fragment objects.

Do not alter gameplay.

## Verified Atari sequence

The ROM-backed enemy-shot player-death sequence uses one shared multicolor fragment picture at $3b1a with discrete global scale stages.

The verified stage order is:

1, 2, 3, 4, 5, 6, 4, 2, 1

The verified relative hold counts are:

2, 2, 2, 2, 2, 4, 3, 2, 1

That is 20 relative display-call units.

The peak size-6 stage has the longest expansion hold: 4 units.

The descending size-4 stage has a 3-unit hold.

The final size-1 stage has a 1-unit hold.

Preserve this order and these relative hold proportions.

Do not smooth away held stages.

Do not require every C# render/simulation tick to produce a visibly different explosion frame.

## Evidence classification

Keep these distinctions explicit:

### ROM/DISASSEMBLY BACKED

- enemy-shot player-death effect family;
- shared multicolor fragment picture;
- stage order 1,2,3,4,5,6,4,2,1;
- relative holds 2,2,2,2,2,4,3,2,1;
- expansion, peak, contraction, and disappearance structure;
- fixed-color multicolor vector groups.

### PROJECT TRANSLATION

- mapping the 20 relative display-call units onto the existing 60 C# death ticks;
- using the existing C# three-layer starburst rather than translating the ROM’s exact $3b1a coordinates;
- mapping Atari scale labels 1–6 onto the accepted C# 150%-peak radius;
- any rounding needed to assign integer C# tick ranges.

Do not claim frame-exact Atari timing.

## Sixty-tick stage mapping

Keep the existing 60-tick PlayerDeathPresentation lifecycle unchanged.

Map the 20 verified relative hold units across the 60 C# death ticks.

The natural initial translation is:

- each relative hold unit receives 3 C# ticks;
- stage durations therefore become:
- size 1 expansion: 6 ticks;
- size 2 expansion: 6 ticks;
- size 3 expansion: 6 ticks;
- size 4 expansion: 6 ticks;
- size 5 expansion: 6 ticks;
- size 6 peak: 12 ticks;
- size 4 contraction: 9 ticks;
- size 2 contraction: 6 ticks;
- size 1 remnant: 3 ticks.

These durations total exactly 60 ticks.

Use half-open, deterministic tick ranges with no gaps or overlaps.

Document clearly whether DeathTicks counts down or elapsed age counts up, and map ranges accordingly without reversing the intended visual order.

At the completion boundary after the final size-1 remnant, the explosion must disappear cleanly.

Do not introduce an extra visible stage beyond the 60-tick lifecycle.

## Stage scales

Map Atari stages 1–6 onto centralized C# scale factors.

Requirements:

- stage 1 is the smallest visible fragment stage;
- stages 2–5 increase monotonically;
- stage 6 uses the existing accepted peak radius;
- contraction stage 4 uses the same scale as expansion stage 4;
- contraction stage 2 uses the same scale as expansion stage 2;
- final stage 1 uses the same scale as initial stage 1;
- the peak remains at the accepted 150% size;
- the explosion center and layer-relative scales remain unchanged.

Prefer deriving normalized stage scales from the six Atari scale levels in a simple explicit table.

Do not reuse the old continuously interpolated radius envelope inside a stage.

Each stage must hold a constant visual scale over its assigned tick range.

If the exact Atari vscale values can be translated cleanly through existing vector-scale conventions without changing the accepted peak size, use their relative proportions.

Otherwise, define a clear monotonic normalized project-translation table anchored at:

- stage 6 = current accepted peak radius;
- stage 1 = compact visible breakup/remnant.

Do not tune values from individual screenshot pixels.

Keep all stage scale constants centralized and maintainable for one bounded manual refinement if Neal finds the first result clearly too compact or too broad.

## Preserve the existing layered effect

Keep the existing three layers:

- yellow;
- orange-red;
- white.

Preserve:

- each layer’s current base scale;
- fixed rotation;
- asymmetric radius multiplier pattern;
- vector-line topology;
- current center calculation;
- sparkle-like multicolor appearance insofar as the existing renderer provides it.

The primary change is the shared stage schedule and stepped global scale.

Do not redesign colors or layer geometry in this task.

## Compact breakup and held pictures

The first visible death stage should appear as a compact breakup, not as an intact Blaster and not as an almost invisible zero-radius shape.

Preserve the normal intact Blaster until the existing death-rendering path begins.

Once death rendering begins:

- stage 1 must be visibly present;
- stage 1 must hold for its translated duration;
- each subsequent stage must remain visually unchanged during its hold;
- stage transitions must be discrete;
- the peak must remain held longer than ordinary expansion stages;
- contraction must proceed through 4, 2, and 1 without smooth interpolation between them.

Do not blend between neighboring stage scales.

Held frames are expected behavior.

## Visual-only boundary

Do not change:

- death detection;
- death cause;
- PlayerDying;
- DeathTicks duration;
- lives decrement;
- collision;
- player lane;
- score;
- enemy state;
- enemy retraction timing;
- death reconstruction;
- re-entry timing;
- invulnerability;
- Super Zapper;
- wave state;
- Game Over;
- recording schema;
- deterministic simulation.

The renderer reads the accepted death clock and chooses a visual stage.

It must not own or mutate gameplay state.

## Death-cause scope

The verified sequence is the enemy-shot player-death family.

The current C# project uses a common accepted Blaster death presentation.

For this bounded milestone, apply the stepped schedule through the existing shared Blaster-death renderer rather than introducing cause-specific player-death effects.

Do not implement separate enemy-shot, Spike, Pulsar, Flipper, or Fuseball death presentations.

Clearly classify this as a project-wide visual translation using the verified enemy-shot sequence as the fidelity model.

Cause-specific death presentation may be investigated separately in the future if Neal requests it.

## Global level scope

The corrected Blaster-death effect is global and should remain constant across current and future levels.

Do not add per-level death-animation tables.

Verify compatibility with Levels 1–11 and the direct Level 15 development candidate.

Do not begin Level 12.

## Presentation compatibility

Legacy and Atari presentation must select the same death stage and submit the same underlying local explosion geometry.

Only the established projection path may differ.

Do not change:

- Atari model-frame mappings;
- tube calibration;
- portrait perspective;
- viewport;
- HUD;
- Approach;
- ExitZoom;
- Fuseball animation.

## Playback and reverse navigation

Preserve deterministic playback and reverse stepping.

The rendered death stage must be a pure deterministic function of existing death-presentation state.

Do not introduce wall-clock timing.

Do not retain mutable renderer-only history that cannot be reconstructed during reverse playback.

At the same DeathTicks or elapsed-death age, the same stage and geometry must always be selected.

## Automated tests: stage schedule

Add focused tests proving:

1. Exactly nine ordered visual stage entries exist.
2. Stage order is 1,2,3,4,5,6,4,2,1.
3. Hold proportions are 2,2,2,2,2,4,3,2,1.
4. The 60-tick mapping uses durations 6,6,6,6,6,12,9,6,3.
5. Durations total exactly 60.
6. Tick ranges contain no gaps.
7. Tick ranges contain no overlaps.
8. Every active death tick maps to exactly one visible stage.
9. The final completion boundary maps to no visible explosion.
10. Stage transitions are discrete rather than interpolated.
11. Peak size 6 is held for 12 C# ticks.
12. Contracting size 4 is held for 9 ticks.
13. Final size 1 is held for 3 ticks.

Test boundary ticks explicitly.

## Automated tests: scale and geometry

Verify:

1. Stage scales increase monotonically from 1 through 6.
2. Stage 6 uses the current accepted peak radius.
3. Expansion and contraction instances of stage 4 use identical scale.
4. Expansion and contraction instances of stage 2 use identical scale.
5. Initial and final stage 1 use identical scale.
6. Existing three-layer relative scales remain unchanged.
7. Existing layer rotations remain unchanged.
8. Existing color assignments remain unchanged.
9. Existing line topology remains unchanged.
10. Explosion center remains unchanged.
11. No independent fragment state/velocity is introduced.
12. Geometry is finite at every stage.
13. Final disappearance emits no death-effect lines after completion.

## Automated tests: gameplay preservation

Verify:

1. Total death lifecycle remains 60 ticks.
2. Lives decrement timing remains unchanged.
3. Re-entry timing remains unchanged.
4. Final-life Game Over behavior remains unchanged.
5. Collision behavior remains unchanged.
6. Player lane/state remains unchanged by rendering.
7. Enemy retraction/reconstruction remains unchanged.
8. Score and Super Zapper states remain unchanged.
9. Spike-related reverse-zoom/death behavior remains unchanged.
10. Rendering does not mutate gameplay/model state.

## Automated tests: determinism and presentation

Verify:

1. Identical death state selects identical stage.
2. Playback reproduces identical stage progression.
3. Reverse playback/scrubbing restores the correct stage for each death tick.
4. Restart initializes death presentation normally.
5. Legacy and Atari presentation use the same local stage.
6. Levels 1–11 use the same global death schedule.
7. Direct Level 15 uses the same global death schedule.
8. Fuseball visual animation remains unchanged.

Avoid screenshot/golden-image tests.

Test stage selection and submitted geometry directly.

## Validation

Run bounded:

1. New Blaster-death stage-schedule tests.
2. Existing Blaster death-presentation tests.
3. Death/re-entry/lives/Game Over regressions.
4. Playback/reverse-playback determinism tests.
5. Legacy/Atari presentation regressions.
6. Level 1–11 regressions relevant to player death.
7. Direct Level 15 preservation tests.
8. Fuseball visual-animation regressions.
9. Complete solution test suite.
10. Release solution build.
11. git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide concise commands for observing a normal Blaster death in:

1. Legacy presentation.
2. Atari presentation.
3. Playback/reverse stepping if an established recording path contains a suitable death event.

Use only verified existing commands and paths.

Neal should judge:

- visible compact initial breakup;
- deliberate held frames;
- stepped expansion;
- held broad peak;
- stepped contraction;
- compact final remnant;
- clean disappearance;
- existing accepted overall size;
- existing center;
- existing colors;
- no gameplay or re-entry timing regression;
- same underlying effect in legacy and Atari presentation.

The acceptance target is improved animation fidelity, not frame-perfect MAME timing.

## Records and Git

Save this prompt and implementation report under the tracked AI-Logs convention.

Do not update durable behavior documentation before Neal’s manual acceptance.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked work and historical records.

Do not reset, restore, stash, clean, or rebase.

Stop after implementation, automated validation, and manual-test handoff.

TEMPEST_BLASTER_DEATH_STEPPED_ANIMATION