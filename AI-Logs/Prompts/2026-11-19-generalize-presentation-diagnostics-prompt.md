Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the token do not invalidate the token.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Generalize the presentation calibration diagnostics to all implemented levels

Continue from the current uncommitted presentation-calibration candidate.

The diagnostic controls have proven extremely useful during manual calibration.

Current Level 1 manual evidence:

--atari-presentation --level 1 --tube-y-gain 1.4 --level1-tube-scale .8 --tube-vertical-offset +35

looks quite good.

This is a useful approximate diagnostic calibration. Do not spend time fine-tuning it and do not encode these values as production constants in this task.

The immediate problem is that the diagnostic controls are unnecessarily restricted by level.

Neal needs to be able to freely combine the same three presentation controls while testing Level 7 and, ideally, every other implemented Atari-presentation level:

--tube-y-gain

--tube-scale

--tube-vertical-offset

Unless investigation discovers a concrete technical reason why a particular transform cannot safely operate on a particular implemented level, generalize these presentation diagnostics so Neal can experiment with them across all implemented Atari-presentation levels.

This is a diagnostic-capability task. It is not a whole-game presentation-fixing milestone.

Neal may notice unrelated presentation problems while testing other levels and keep notes for later. Do not fix those problems as part of this task.

## 1. --tube-y-gain

Generalize the existing Y-gain diagnostic so it can be used on all implemented Atari-presentation levels unless a genuine technical restriction prevents that.

Purpose:

Change the tube's vertical proportion/shape.

Requirements:

- Default remains identity.
- Accept appropriate finite diagnostic values without arbitrary narrow aesthetic limits.
- Preserve the existing ability to deliberately test extreme finite diagnostic values.
- Reject malformed and non-finite input.
- Do not select or encode per-level production values.
- Do not automatically change tube scale or vertical position.

## 2. --tube-scale

Generalize the current Level-1-only --level1-tube-scale diagnostic into:

--tube-scale

Purpose:

Uniformly change the overall presented size of the tube while preserving its existing proportions.

Requirements:

- Default is identity.
- Uniformly scale X and Y.
- Preserve whatever proportion has already been established by --tube-y-gain.
- Allow use across all implemented Atari-presentation levels unless a genuine technical restriction prevents it.
- Accept positive finite values without arbitrary aesthetic upper limits.
- Reject malformed, non-finite, zero, and negative scale values.
- Do not automatically change vertical position.
- Do not automatically recenter the result.
- Do not select or encode production values.

Because this is a development-only diagnostic, avoid unnecessary compatibility complexity.

If there is no concrete reason to retain --level1-tube-scale, replace it with the clearer generalized name:

--tube-scale

Clearly report what was done.

## 3. --tube-vertical-offset

Generalize the existing vertical-position diagnostic so it can be used across all implemented Atari-presentation levels unless a genuine technical restriction prevents it.

Purpose:

Move the complete presented tube vertically without changing its shape or size.

Requirements:

- Default remains zero.
- Positive values move down and negative values move up, preserving the established screen-coordinate convention.
- Accept finite signed values without arbitrary aesthetic limits.
- Reject malformed and non-finite input.
- Do not automatically derive vertical position from Y gain or tube scale.
- Do not alter tube proportion or uniform scale.

## Keep the three controls independent

This separation is intentional:

- --tube-y-gain = vertical proportion/shape.
- --tube-scale = overall uniform size.
- --tube-vertical-offset = vertical position.

Do not derive one value from another.

Do not introduce automatic recentering.

Do not make scaling automatically reposition the tube.

Manual visual calibration is intentional.

A desired command should therefore be possible in this form:

dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35

The same three independent options should be usable on another implemented Atari-presentation level unless a genuine technical reason prevents it.

## Preserve the active milestone

Generalizing diagnostic availability does not broaden the development milestone.

Do not use this task to recalibrate or repair Levels 2-6 or 8-10.

If testing later reveals other presentation problems on those levels, Neal will record them for separate future work.

Do not select final production calibration constants.

Do not investigate or correct Level 7's remaining inner/core problem in this task.

Do not change:

- far/core-center relationships;
- notch-to-core relationships;
- rail convergence;
- depth projection;
- the accepted portrait-perspective formula;
- accepted viewport dimensions;
- HUD layout;
- gameplay/model geometry;
- enemy behavior or timing;
- collision behavior;
- deterministic simulation behavior.

Preserve accepted Approach and ExitZoom behavior.

## Technical check before implementation

Inspect the current diagnostic implementation and determine whether there is any actual architectural reason that Y gain, uniform tube scale, or vertical offset cannot safely operate on a particular implemented Atari-presentation level.

If a genuine technical restriction exists:

- do not work around it speculatively;
- preserve the affected behavior;
- clearly identify the exact technical restriction in the final report.

Do not preserve an existing level restriction merely because the diagnostic was originally created specifically for Level 1 or Levels 1/7.

## Presentation registration

The complete presented tube and attached presentation must remain coherent under the transforms.

Preserve appropriate registration of:

- tube geometry;
- Blaster;
- enemies;
- shots;
- Spikes;
- Flippers;
- other tube-attached presentation.

The diagnostics remain presentation-only.

Do not mutate gameplay/model geometry or simulation state.

Recorded gameplay and deterministic playback must remain simulation-equivalent regardless of diagnostic presentation values.

Do not store these presentation diagnostic settings in the gameplay recording format.

## Visual assets

Do not open, enumerate, search for, or analyze screenshots, videos, frames, comparison images, or other visual assets.

Neal is performing visual calibration manually.

## Screenshot behavior

Preserve the established screenshot workflow:

- F9 toggles pause.
- F10 performs the Tempest-owned screenshot capture.
- Tempest-owned screenshots are saved under UserData\Screenshots.
- Repeated captures use distinct collision-safe/incrementing names and do not overwrite previous captures.
- Neal will manually move screenshots into any desired subfolders.
- Do not automatically organize screenshots by level.
- F12 remains Raylib's native screenshot behavior and is not Tempest's screenshot command.

Do not redesign screenshot capture.

## Validation

Add or adjust focused automated tests for the generalized diagnostic scope.

Verify at minimum:

- omitted controls retain identity behavior;
- --tube-y-gain can operate across implemented Atari-presentation levels;
- --tube-scale can operate across implemented Atari-presentation levels;
- --tube-vertical-offset can operate across implemented Atari-presentation levels;
- the three controls can be used simultaneously;
- each control remains independently adjustable;
- uniform tube scale does not alter the proportion established by Y gain;
- vertical offset does not alter proportion or scale;
- tube geometry and attached presentation remain registered;
- gameplay/model state is not mutated;
- playback/deterministic state remains equivalent under different diagnostic presentation values;
- existing Approach behavior remains valid;
- existing ExitZoom behavior remains valid;
- existing screenshot regressions continue to pass.

Run bounded focused tests, appropriate targeted regressions, the complete solution test suite, Release build, and git diff --check.

Follow the established project instructions for Windows security blocks, locked artifacts, or stalled commands.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide concise example commands demonstrating independent combinations of:

--tube-y-gain

--tube-scale

--tube-vertical-offset

for:

1. Level 1.
2. Level 7.
3. At least one additional implemented Atari-presentation level as confirmation that the former diagnostic level restrictions have been removed.

For Level 1, include Neal's current useful approximate reference:

dotnet run --project src\Tempest.Game -- --atari-presentation --level 1 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35

For Level 7, provide an equivalent example such as:

dotnet run --project src\Tempest.Game -- --atari-presentation --level 7 --tube-y-gain 1.4 --tube-scale .8 --tube-vertical-offset +35

These example values are diagnostic starting points only.

They must not become parser limits or production constants.

Neal must remain free to deliberately test more extreme finite diagnostic values without another code change.

Save the substantial prompt and implementation report in the established ignored AI-Logs locations.

Preserve all unrelated worktree changes and user/reference assets.

Do not stage, commit, push, reset, restore, stash, clean, or rebase.

Stop after automated validation and provide the manual-test commands.

TEMPEST_GENERALIZE_PRESENTATION_DIAGNOSTICS