# Close out, document, validate, commit, and push background opacity

The development-only background-transparency calibration feature has passed
Neal's manual acceptance. This is a CLOSEOUT task; do not add transparency
features, revisit whole-window opacity, or begin Level 15.

## Manual acceptance

Record Neal's final result: **`--background-opacity` works perfectly and
produces exactly the desired calibration behavior.** Neal verified that the
MAME/desktop reference is visible through Tempest's background; tube and HUD
vectors remain solid; Blaster, enemies, shots, and effects remain normally
visible; geometry and input are unchanged; and the feature is useful for
manual overlay calibration. Treat background-only transparency as manually
accepted and frozen.

## Accepted implementation

Preserve the implementation in
`AI-Logs/Reports/2026-11-24-background-opacity-correction-report.md`.
Freeze `--background-opacity <value>` with finite invariant-culture values
from 0 through 1, default 1, where 1 is the ordinary opaque black background.
Values below 1 request the transparent-framebuffer window flag before window
creation. Configured alpha affects only the established black background
clear; later vector submissions keep their current colors/alpha. Headless
playback rejects the graphical-only option. The value is not gameplay
recording data. Do not retune or redesign it.

The earlier `--window-opacity` experiment remains historical AI-Logs only. Do
not restore it, `SetWindowOpacity` startup application, or obsolete
whole-window-opacity source/tests/docs. Final diagnostic is background-only.

Keep the existing Raylib/Raylib-cs transparent-framebuffer mechanism. Do not
add layered-window P/Invoke, chroma-key transparency, desktop capture, custom
compositor, or native dependencies. Preserve normal focus/input; do not enable
mouse passthrough. Preserve the observed Windows/Raylib screenshot behavior:
opaque Tempest-only images rather than desktop composites. Do not change
screenshot architecture.

Keep this a generally available development diagnostic. It remains usable
with supported `--level`, `--start-level`, Atari and legacy presentation,
calibration guides, Y gain, tube scale, vertical offset, and depth-Y gain.
Default opacity remains 1; omitted option preserves normal window/render
behavior and does not request the transparent framebuffer.

## Documentation and preservation

Update README/help and suitable developer documentation to describe
`--background-opacity <0..1>` as a development-only whole-background
transparency control for manual visual comparison/alignment. State that it
changes background/framebuffer transparency, not vector graphics; it is for
manual overlay calibration, does not load or align a reference image, and
defaults to 1. Do not add it as arcade gameplay in the behavior specification
unless there is a natural development-diagnostics section.

Do not alter Levels 1-14 or direct Level 15, tube geometry, calibration,
depth-Y, HUD, Fuseball/Flipper/Spiker/Tanker behavior, death, playback,
reverse playback, screenshots, Approach, ExitZoom, scoring, lives, or input.
This is display/window tooling only.

## Repository audit and validation

Before staging, verify branch, HEAD, upstream, ahead/behind, index, tracked
worktree, and untracked material. Distinguish accepted implementation/tests/
README/help, AI-Logs, the pending post-push Level 14 closeout-report update,
and unrelated local material. Never stage MAME screenshots, UserData, ROMs,
`Reference/arcade/MBUCOD.V05.txt`, Temp/TEMP, Videos, recordings, personal
scratch material, or build output.

Run a fresh bounded closeout gate covering:

1. BackgroundOpacityOptions tests.
2. CLI/startup tests.
3. Window lifecycle/config-flag tests.
4. Display configuration tests.
5. Screenshot regressions.
6. Atari presentation regressions.
7. Tube calibration tests.
8. Depth-Y diagnostic tests.
9. Playback/headless tests.
10. Input/startup tests.
11. Levels 1-14 production regressions.
12. Direct Level 15 preservation.
13. Complete solution test suite.
14. Release solution build.
15. `git diff --check`.

Use explicit hang/time bounds and do not weaken Windows security or launch an
unnecessary interactive game session. If a genuine regression appears, stop
before staging. Visual acceptance has already passed.

## Stage and publish

After validation, stage only accepted background-opacity code, tests,
README/help, relevant AI-Logs records for whole-window history, correction,
and this closeout, plus the expected Level 14 closeout-report update if it
naturally belongs in this commit. Inspect staged inventory and source/text
diffs, run `git diff --cached --check`, and confirm no protected/local assets
are staged. If ownership is ambiguous, stop.

If and only if validation and staged review pass, commit on current `master`
with subject `Add background overlay calibration`, then push normally to
`origin/master`. Do not force-push, create a branch, amend, or rewrite
history. Verify local `master` equals `origin/master`, ahead/behind is `0/0`,
and the index is empty. Where available, independently verify remote master
with the GitHub connector and report connector failures honestly.

Save this prompt and a paired final report under tracked AI-Logs. Record
starting HEAD, final SHA, commit/push and GitHub verification, focused and
full validation, Release build and whitespace gates, exact committed
inventory, Raylib-cs version and framebuffer mechanism, whole-window option
removal/non-acceptance, manual acceptance, screenshot and default behavior,
excluded/local material, and final worktree/index/upstream state. Do not
create an amend/report/commit loop to place the resulting SHA in its own
commit; a final post-push report update may ride with the next normal commit.

Do not begin Level 15.

TEMPEST_BACKGROUND_OPACITY_CLOSEOUT
