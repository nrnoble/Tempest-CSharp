# Background-only opacity correction

Continue from the uncommitted window-opacity candidate, using
`AI-Logs/Reports/2026-11-24-window-opacity-diagnostic-report.md` as factual
handoff. Neal manually verified that whole-window opacity fades the tube, HUD,
Blaster, enemies, shots, effects, and background; that is not acceptable for
reference overlay calibration.

Replace the whole-window experiment with `--background-opacity <value>`.
Values must be finite invariant-culture numbers in `[0,1]`, defaulting to 1.
Reject missing, malformed, non-finite, out-of-range, duplicate, and headless
uses. Preserve the normal opaque-black startup exactly at the default. For
values below 1, set Raylib-cs 8.1.0's transparent-framebuffer flag before
`InitWindow`, then change only the background clear alpha; retain black RGB,
all vector colors/alpha, rendering order, geometry, gameplay, input, and
calibration behavior. Do not apply window opacity, alter blending without need,
or enable mouse passthrough. Keep the control in startup/display diagnostics,
independent of tube calibration and recordings. It must compose with level
selection, Atari presentation, lives, tube controls, and calibration guides;
Atari presentation is not required.

Verify the installed binding/API and the exact clear and window-creation paths.
If transparent framebuffer support is unavailable, stop without platform
specific APIs or new native packages. Replace the unaccepted whole-window
option (Choice A) unless evidence justifies keeping both. Preserve existing
screenshot architecture: report whether captures are framebuffer-only,
whether PNG alpha is preserved, and ensure no desktop/MAME compositing.
Do not redesign fullscreen, borderless, or interaction behavior.

Add focused parser, lifecycle, and rendering-semantics tests; run the
requested display, CLI, screenshot, Atari presentation, calibration,
headless/playback, Levels 1-14, full solution, Release build, and diff checks
with bounded commands. Run a short graphical smoke check for initialization,
responsiveness, exceptions, and continued rendering, but do not claim visual
compositor acceptance from process health.

Save a paired implementation report and this prompt under `AI-Logs/`. Do not
stage, commit, push, reset, restore, stash, clean, or rebase. Preserve Level 14
and unrelated local material. Do not begin Level 15. End with Level 14
baseline, half-transparent, and fully transparent command examples, plus
Neal's manual checks for background visibility, solid vectors, unchanged
geometry, normal input, calibration utility, and screenshot behavior. Manual
acceptance remains pending.

TEMPEST_BACKGROUND_TRANSPARENCY_DIAGNOSTIC
