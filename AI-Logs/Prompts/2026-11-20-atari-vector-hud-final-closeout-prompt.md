# Atari Vector Gameplay HUD Final Closeout Prompt

Date: 2026-11-20
Scope: Final closeout, commit, and push of the accepted Atari vector gameplay HUD
milestone.
Completion token: `TEMPEST_ATARI_VECTOR_HUD_FINAL_CLOSEOUT`

Neal provided manual acceptance across all currently implemented/tested levels.
The accepted HUD is frozen for this milestone:

- ROM/disassembly-informed vector digits and uppercase A.
- ROM/disassembly-informed reserve-life Blaster geometry.
- Live current score and live level.
- Fixed high score 38930 and fixed initials AAA.
- Green score/high/initials, blue level, and yellow reserve Blasters.
- Right-justified six-position score; zero displays as `0`; digit 1 has one
  visible vertical stroke.
- Reserve lives exclude the active Blaster, cap at 25 symbols, and do not mutate
  Player.Lives.
- Accepted defaults: score right edge 250, score baseline 75, score scale 1.50,
  level baseline 75, reserve baseline 100.
- Development-only `--hud-*` absolute calibration controls remain preserved.

Constraints:

- Do not redesign, fine-tune, or reopen presentation calibration.
- Do not change gameplay, timing, scoring, lives, progression, playback,
  recording, F9/F10 behavior, or legacy presentation.
- Do not add monitor junction-brightness behavior.
- Stage only HUD source/tests, appropriate durable documentation, and tracked
  AI-Logs records for this milestone.
- Preserve protected/local and unrelated material, including ROMs, UserData,
  TEMP, Videos, root screenshots, and untracked assets.
- Do not inspect protected ROM contents.
- Run bounded focused validation, relevant presentation/score/playback/pause
  regressions, the complete solution suite, Release build, and whitespace check.
- Do not launch the interactive game.
- Commit to master and push normally to origin/master without force, amend,
  reset, or rebase.
