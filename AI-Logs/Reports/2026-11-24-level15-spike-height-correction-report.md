# Level 15 starting Spike height: blocked baseline

**Status: BLOCKED — EVIDENCE CONFLICT — STOP.**

## Scope and baseline

The task requested verification and a possible correction of the current
uncommitted production Level 15 candidate, without changing its presentation
calibration or other gameplay. At inspection, `master` and `origin/master`
were at `045aeb240fce91a3eaacf9424da30cd7359927bf`. There were no staged
changes and no source or test worktree changes. A pre-existing modified
background-opacity closeout report and unrelated untracked material were
left untouched.

The available Level 15 configuration is explicitly a development-only,
direct-start candidate using a Level 11 gameplay parameter baseline:
`src/Tempest.Game/Levels/LevelConfiguration.cs` identifies it as such.
`src/Tempest.Game/Levels/LevelProgression.cs` ends the production chain at
Level 14 and routes direct Level 15 to that development candidate. The
Level 15 candidate test explicitly distinguishes its Level 11 evidence from
Level 15. This is materially different from the requested uncommitted
production candidate, so modifying it would not safely address the requested
baseline.

## Evidence and work

The existing development candidate uses `0xB0 * Level.Depth / 224f`.
The source associates the raw `0xB0` with Level 11; this does not establish
the genuine Level 15 starting-Spike byte. The accepted Level 11–14 source
expressions subtract their raw tip coordinate from `0xF0` before scaling by
`Level.Depth / 224f`. No conclusion is made about the missing production
candidate or its genuine Level 15 ROM value.

No gameplay, presentation, test, or durable behavior file was changed.
The supplied task prompt was copied to the paired prompt record; this report
is the only other task-created file. No protected reference or user material
was modified.

## Validation and handoff

**BLOCKED:** the requested verification, correction, regression tests,
deterministic soak, full solution tests, Release build and manual acceptance
cannot be completed against the specified missing candidate. No automated
test or interactive game was run. No files were staged, committed, or pushed.
**PASS:** `git diff --check`; the copied prompt matches the supplied attachment.

Restore or identify the uncommitted production Level 15 candidate and rerun
the task against it. Manual judgment of the proposed Spike correction is
pending implementation and completed automated validation; presentation
recalibration is not requested.
