# Final Level 15 closeout

## Scope and accepted candidate

Close out the existing direct-start/development-only Level 15 candidate after
Neal's final manual acceptance. Do not do additional gameplay, presentation,
ROM, lifecycle, or feature work unless fresh validation identifies a genuine
regression introduced by this candidate. Do not begin Level 16.

Level 15 gameplay passes manual testing, subject to the two corrected defects.
Freeze presentation at Y gain `1.15`, tube scale `1`, vertical offset `75`, and
depth-Y gain `1`. The starting-Spike correction uses genuine Level 15 raw
`$A0` and the established ROM-coordinate-to-C# height translation; Neal
confirms the Spikes are fixed. The terminal-Spiker issue was an unwired
Level 15 lifecycle eligibility, not a demonstrated regression. Neal confirms
the endless terminal-Spiker cycling is fixed: the same identity restarts while
generic pending supply remains, and at zero pending the same identity converts
in place to a Tanker with exactly two ledger-owned latent cargo identities.
Tankers at ordinary selection capacity and unrelated active enemies do not
block conversion. Tanker/cargo obligations continue to block completion.

Keep Level 15 direct-start/development-only and outside normal campaign
progression, which currently ends at Level 14. Do not claim broader arcade
perfection or Level 16 acceptance/implementation.

## Repository safety and ownership

Before staging, record branch, HEAD, upstream, ahead/behind, index, tracked
changes, untracked material, Level 15 ownership, the prior background-opacity
post-push report update, and unrelated/protected local material. Independently
check remote `master` with the GitHub connector where available. If that
connector fails, is empty, or invalid, stop relying on it and report the need
to reconnect/reset it.

Read the Level 15 Spike and terminal-Spiker handoffs. Preserve earlier reports
as historical records. Update durable behavior/development documentation with
final acceptance and evidence classifications. Never stage ROM/reference
binaries, recordings, screenshots, UserData, Temp/TEMP, Videos, personal
scratch files, unknown assets, or build output.

## Fresh validation before staging

Run bounded Release validation for Level 15 geometry/topology/direct startup,
configuration, supply/selection/identity accounting, raw `$A0` and converted
Spike height/count/lanes/growth/collision, terminal-Spiker positive-pending
restart and zero-pending same-ID Tanker conversion, two owned cargo identities,
capacity/unrelated-enemy cases, completion/all-rim obligations, ledger and
reconstruction/death/re-entry behavior, accepted presentation, Levels 1-14
shared-path regressions, Level 11 and other terminal-Spiker/Tanker/cargo
regressions, direct-start, playback/headless, and the complete Release test
suite. Then run a fresh Release solution build and `git diff --check`.
Identity-specific terminal tests, not a broad soak, establish the endpoint
lifecycle. Do not weaken valid tests or launch the graphical game.

Stop before staging on a genuine new production regression or incomplete
required validation. Treat security blocks as blocked validation; do not
bypass or retry the same blocked artifact.

## Review, commit, and remote verification

Only after all validation passes, stage the audited Level 15 milestone files
and any clearly owned prior background-opacity post-push update. Review the
complete staged inventory and source/test/documentation diff; run
`git diff --cached --check`; confirm accepted values and `$A0`, terminal
correction, direct-only status, and absence of protected/unrelated files or
Level 16 work.

On the current `master`, commit as `Complete accepted Level 15`, then push
normally to `origin/master` without force, amend, or history rewriting. Verify
local HEAD, `origin/master`, `0/0` ahead/behind, empty index, subject/SHA, and
independent GitHub remote `master` when available. Create a paired final report
recording the full audit, manual acceptance, exact validation, inventory,
excluded material, commit/push, and post-push state. Leave any SHA-dependent
post-push report update unstaged rather than making another commit.

After successful commit, push, and remote verification, stop and report
Level 15 closeout. No Level 16 work.

TEMPEST_LEVEL15_FINAL_CLOSEOUT
