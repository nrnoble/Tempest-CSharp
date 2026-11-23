# Task: Reject and restore the Level 12 ExitZoom candidate

Neal manually evaluated the geometry-derived Level-12-only ExitZoom camera and
rejected it: it did not improve the exit, severely distorted the tube
composition, and was worse overall. Do not tune this strategy or begin another
ExitZoom investigation.

Restore Level 12 ExitZoom to the pre-candidate generic ExitZoom presentation.
Remove only the candidate camera, Level-12-specific dispatch/cache/glyph
handling, camera tests, and candidate-only generic-path preservation cases.
Preserve the generic implementation and its original validation.

Preserve all independent Level 12 work: the genuine displayed level and shape
row 12, recovered geometry and closed topology (`Continuous = true`, 16 lanes),
ROM presentation metadata, model-frame mapping, wave policy, Level 11 to 12
progression and terminal Level-13-not-implemented boundary, HUD and Atari
presentation support, neutral/current calibration infrastructure, topology
regressions, bounded production soak, renderer/model-frame regression, and
corrected startup message.

Keep Neal's 1.3 tube Y gain, 0.8 scale, and zero vertical offset as manual
evidence; do not encode or recalibrate these values. Do not change Levels 1-11,
direct Level 15, Atari vector HUD, accepted Fuseball and Blaster-death behavior,
spinner behavior, screenshots, pause/capture, playback, reverse playback,
Approach, or calibration guides. Do not inspect screenshots, other images,
UserData, TEMP, or private notes. Do not launch the game.

Record the deferred issue narrowly: existing generic ExitZoom is visually
imperfect on Level 12; the geometry-derived Level-12-only camera was manually
rejected as worse; no further correction was attempted; gameplay is not
blocked; future work may revisit ExitZoom only if a materially better
architectural approach emerges. Do not alter the durable behavior spec or
document the rejected algorithm as project behavior.

Run bounded generic ExitZoom, Level 12 topology/production/model-frame/soak/
spinner/HUD/progression, Levels 1-11 presentation, Level 15, Fuseball,
Blaster-death, and playback/reverse-playback tests; run the complete solution
test suite, Release build, and `git diff --check`. Report failures without
changing unrelated work.

Preserve unrelated tracked and untracked work. Do not stage, commit, push,
reset, restore, stash, clean, or rebase. Stop after restoration and automated
validation. Do not begin the Fuseball bonus-number correction.

Required final disposition:

**LEVEL 12 EXITZOOM CANDIDATE REJECTED — PRE-CANDIDATE EXITZOOM RESTORED; DISCREPANCY DEFERRED**

TEMPEST_LEVEL12_EXITZOOM_RESTORE_AND_DEFER
