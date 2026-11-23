Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Replace the Atari-presentation Fuseball bonus font with accepted vector digits

Continue from the current uncommitted Level 12 candidate after:

- genuine Level 12 implementation;
- closed-topology correction;
- Level 12 headless production gate;
- rejection/restoration of the Level 12 ExitZoom experiment;
- correction of the overbroad Level 11 terminal-Spiker regression test.

This is a tightly bounded **global rendering correction**.

Neal has identified an existing presentation inconsistency:

**When a Fuseball is destroyed, its temporary score/bonus amount is currently rendered using a conventional font. In Atari presentation, that amount should be drawn using the same Atari-style vector digit system already used by the accepted gameplay HUD.**

Do not change the Fuseball score award itself unless inspection proves a separate scoring defect.

The reported problem is rendering.

## First inspect the current local implementation

Inspect the current local worktree, especially:

- the Fuseball score-popup state/data type;
- where the popup is created when a Fuseball is destroyed;
- how the amount is stored;
- how long it remains visible;
- where VectorRenderer submits it;
- which presentation modes use it;
- whether it currently uses Raylib/font rendering;
- the accepted Atari HUD vector-digit implementation;
- existing reusable digit geometry/helpers.

Determine the narrowest way to reuse the already accepted Atari HUD vector digits.

Do not create another font or another independent vector-digit alphabet.

Do not change source merely because a helper could theoretically be cleaner.

## Preserve score semantics

Before changing rendering, verify the current popup value comes from the established Fuseball score-award logic.

Preserve:

- awarded score amount;
- score addition timing;
- popup numeric value;
- popup lifetime;
- popup world/lane/depth association;
- destruction timing;
- Fuseball identity/lifecycle;
- shot destruction;
- Super Zapper behavior;
- wave completion.

Do not change gameplay scoring.

If the numeric award is already correct, leave it exactly unchanged.

If local inspection reveals an unrelated numeric scoring defect, report it separately rather than folding it into this rendering task.

## Atari-presentation rendering

When Atari presentation is active, render the Fuseball bonus amount using the accepted Atari vector digits.

Requirements:

- no conventional font rendering for the Fuseball bonus amount in Atari presentation;
- use the same accepted digit geometry used by the Atari gameplay HUD;
- preserve digit 1 as the accepted single vertical stroke;
- decimal digits only unless current score-popup behavior requires another established character;
- render the actual existing bonus value dynamically rather than hard-coding a particular amount;
- preserve deterministic rendering;
- preserve the popup's intended association with the destroyed Fuseball position;
- preserve the existing popup lifetime.

Do not alter HUD score rendering itself.

## Reuse accepted vector digit infrastructure

Prefer a small reusable drawing/geometry helper shared with the existing AtariHudVector implementation if the current architecture permits that cleanly.

The objective is:

**one accepted Atari digit definition, reused by both HUD numbers and the Fuseball bonus.**

Do not duplicate the digit segment tables into a second popup-specific implementation.

Do not alter the accepted digit shapes merely to accommodate the popup.

If the current HUD implementation makes direct reuse awkward, extract only the minimum immutable digit geometry/submission helper needed to allow safe reuse.

Preserve existing HUD output exactly.

## Popup scale, placement, and color

Do not assume HUD field scale or HUD screen coordinates should be reused for the popup.

The HUD and Fuseball bonus are different presentation contexts.

Preserve the popup's current gameplay-associated position and presentation intent unless Atari evidence already represented in the project establishes a more specific placement rule.

Use the existing popup color if it is already an accepted/established project translation.

If current font metrics implicitly control centering, replace that centering with deterministic vector-geometry bounds/advance so the numeric vector popup remains centered or anchored consistently relative to its existing popup position.

Keep popup scale and spacing centralized and easy to adjust after Neal's manual test.

Do not conduct a screenshot-fitting exercise in this task.

Do not search for visual references.

## Legacy presentation

Inspect existing legacy behavior before changing it.

Unless there is already a project-wide requirement that this popup should be vector-drawn in legacy presentation too, preserve legacy behavior.

Preferred scope:

- Atari presentation -> accepted Atari vector digits;
- legacy/non-Atari presentation -> existing popup rendering unchanged.

Do not change legacy presentation solely for consistency.

If the popup is currently shared in a way that makes this boundary nontrivial, implement the smallest explicit presentation-mode distinction.

## Presentation coordinate correctness

The Fuseball bonus is associated with gameplay space, unlike the fixed HUD.

Do not draw it through the fixed portrait HUD layout merely because HUD glyph geometry is being reused.

Reuse **glyph geometry**, not necessarily the HUD camera.

The popup should remain registered with the destroyed Fuseball/tube location through the established gameplay presentation path.

Verify compatibility with:

- normal/legacy projection;
- Atari presentation;
- Level 11;
- Level 12;
- tube calibration;
- model-frame projection.

Do not alter tube calibration or projection to make the popup fit.

## Level scope

This is a global Fuseball presentation correction.

Do not hard-code it to Level 12.

Level 11 and every future production level containing Fuseballs should naturally receive the same Atari-presentation vector bonus rendering.

Do not alter levels that do not display a Fuseball score popup.

## Preserve accepted global work

Do not change:

- Fuseball four-phase animation;
- Fuseball terminal-rimward behavior;
- Fuseball identity/lifecycle;
- Level 11;
- genuine Level 12 geometry/gameplay;
- Level 12 closed topology;
- current Level 12 calibration candidate;
- deferred Level 12 ExitZoom behavior;
- Atari HUD layout/calibration;
- tube projection;
- spinner;
- Blaster-death accepted baseline;
- Approach;
- ExitZoom;
- playback;
- screenshot behavior.

Do not reintroduce the rejected Level 12 ExitZoom camera.

## Automated tests

Add focused coverage proving at minimum:

1. Atari-presentation Fuseball bonus rendering uses vector submissions rather than a conventional font call.
2. The popup uses the same accepted digit geometry as the Atari HUD.
3. Digit 1 remains one vertical stroke.
4. Multi-digit bonus amounts render in correct digit order.
5. Digit geometry is deterministic and finite.
6. Popup scale is independent from the fixed HUD field scales.
7. Popup position remains associated with its existing gameplay position.
8. Atari tube calibration does not detach the popup from the destroyed Fuseball position.
9. Popup lifetime is unchanged.
10. Awarded score amount is unchanged.
11. Score addition timing is unchanged.
12. Rendering does not mutate score or gameplay state.
13. Legacy popup rendering remains unchanged if legacy is intentionally preserved.
14. Atari HUD geometry/output remains unchanged.
15. Level 11 Fuseball behavior remains unchanged.
16. Level 12 Fuseball behavior remains unchanged.
17. Fuseball animation remains unchanged.
18. Fuseball terminal behavior remains unchanged.
19. Playback remains deterministic.
20. Reverse playback reconstructs the same popup presentation for equivalent state where the existing architecture supports it.

Avoid screenshot/golden-image tests.

Test vector submissions, bounds/advance, presentation selection, and state preservation directly.

## Validation

Run bounded:

1. New Fuseball bonus-vector rendering tests.
2. Existing Atari HUD/vector-digit tests.
3. Fuseball scoring tests.
4. Fuseball lifecycle/identity tests.
5. Fuseball visual-animation tests.
6. Fuseball terminal-behavior tests.
7. Level 11 regressions.
8. Level 12 production/topology/headless-soak regressions.
9. Presentation/model-frame/calibration regressions.
10. Playback/determinism regressions.
11. Complete solution test suite.
12. Release solution build.
13. git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide the simplest verified command for Neal to destroy Fuseballs and visually judge the bonus popup.

Prefer a currently existing direct Level 11 or Level 12 command.

If an existing development Fuseball scenario provides faster repeatable visual testing without changing production behavior, provide it as an optional second command.

Neal will judge:

- the bonus amount is visibly composed from Atari-style vector digits;
- it no longer looks like conventional font text in Atari presentation;
- digit shape matches the accepted HUD style;
- size is sensible;
- spacing is sensible;
- amount remains readable;
- popup remains registered to the destroyed Fuseball location;
- popup appears/disappears normally;
- gameplay scoring is unchanged.

Do not require pixel-perfect arcade placement for first acceptance.

## Records and Git

Save this substantial prompt and implementation report under the tracked AI-Logs convention.

Do not update durable documentation before Neal's manual acceptance.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked material, including the active Level 12 candidate and historical rejected ExitZoom/Blaster-death reports.

Do not reset, restore, stash, clean, or rebase.

Stop after the bounded Fuseball bonus-vector correction, automated validation, and manual handoff.

TEMPEST_FUSEBALL_BONUS_VECTOR_DIGITS