# User task: enlarged portrait viewport Git closeout

Complete the Git closeout for the manually accepted 1600x1280 host window and
centered 960x1280 portrait viewport. Manual acceptance is already recorded in
`AI-Logs/Reports/2026-11-18-window-viewport-enlargement-acceptance-report.md`.
Preserve the accepted origin `(320,0)`, 3:4 aspect, 320-pixel side margins,
one-host-pixel border, and exact uniform 4/3 enlargement from 720x960. Keep
logical presentation and accepted Level 7/8/9/10 placements unchanged.

Do not implement new display behavior, alter accepted dimensions, investigate
or correct screenshot proportions, inspect screenshots, or modify protected
references, user data, recordings, videos, TEMP, or UserData. Determine the
provenance of each current changed hunk against the implementation and
acceptance reports and Git baseline. Stage only accepted viewport code, tests,
and durable documentation; preserve unrelated changes and leave AI-Logs
unstaged. Do not reset, restore, clean, stash, rebase, or switch branches.

Run bounded focused viewport/display validation, targeted presentation and
gameplay/determinism regressions, full solution tests with VSTest hang-timeout
protection, a Release solution build, and `git diff --check`. Do not launch the
interactive game; manual acceptance is complete. Review the exact staged file
list and complete staged diff, run `git diff --cached --check`, and commit
only if every required validation passes and the staged diff contains only
this milestone. Push `master` to its existing `origin/master` upstream and
verify branch, HEAD, tracking, ahead/behind, index, worktree, and preservation
of unrelated local changes. Stop with a precise BLOCKED/PARTIAL report if any
safety or validation condition fails.

The paired report must record prior acceptance, exact dimensions and
transform, included and excluded files/hunks, validation commands/counts,
staging review, commit/push result, final Git state, and remaining
acknowledgements. Do not ask for input.

TEMPEST_ENLARGED_PORTRAIT_VIEWPORT_GIT_CLOSEOUT
