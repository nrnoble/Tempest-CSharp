# Level 11 final closeout prompt - 2026-11-22

Scope: close out the already manually accepted genuine production Level 11
milestone, update durable Level 11 documentation and calibration, run bounded
focused and complete validation, stage only the accepted candidate, and commit
and push it to the current master/origin/master.

Constraints:
- Do not investigate or tune Level 11 gameplay, ROM behavior, or presentation
  beyond documenting the accepted calibration and final closeout.
- Record Neal's final manual acceptance accurately, including spinner
  clockwise -> left and counterclockwise -> right.
- Encode Level 11 presentation defaults as Y gain 1.5, tube scale 1.1, and
  vertical offset 0. Preserve Levels 1-10 and CLI absolute overrides.
- Preserve genuine row-8 geometry, open topology, the 16-point/15-lane model,
  generic Fuseball and Tanker-cargo corrections, terminal movement state
  behavior, and direct Level 15 as unaccepted development baseline.
- Keep Level 12 unimplemented and Level 11 as the terminal production level.
- Protect ROM, MBUCOD, UserData, Temp, Videos, screenshots, local assets, and
  build outputs from staging or commit.
- Run fresh focused validation, complete solution tests, Release build,
  git diff checks, staged inspection, commit/push, and post-push verification.
- Save the paired closeout report. Manual acceptance is the final gameplay
  authority and the interactive game must not be launched.

Completion token: TEMPEST_LEVEL11_FINAL_CLOSEOUT
