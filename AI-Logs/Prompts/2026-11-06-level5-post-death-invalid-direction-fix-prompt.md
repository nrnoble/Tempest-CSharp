# Level 5 post-death invalid-direction fix

## Source limitation

The follow-up task was supplied as a pasted attachment. In the resumed
conversation, only `#attachment:Pasted text #1` was available; its original
contents and completion token were not recoverable. This record reconstructs
the substantive requirements from the retained task summary and is not a
verbatim copy of the attachment.

## Recovered task

Neal supplied an exception trace from the Level 5 post-death unexpected exit:
an unhandled `ArgumentOutOfRangeException("direction")` from
`Flipper.BeginPivot()` into `Level.CalculateDirectedLaneTransition()`, followed
by cleanup and abnormal exit code `-532462766`.

Find and fix the root cause narrowly. Preserve the existing normal-exit
diagnostic and accepted gameplay behavior; do not make speculative Level 5,
Spike, enemy, or input changes. Determine whether pre-death active play is
necessary, identify the smallest production-realistic state sequence that
reproduces the failure, and retain the no-input repeated-death control.

Use deterministic headless production simulation for the regression. Verify
the exact pre-fix exception and its `direction` parameter, then verify the
fixed lifecycle completes with a valid direction. Cover Level 5 and relevant
neighboring movement, cargo, death/re-entry, progression, firing, Spike,
Super Zapper, rim-pursuit, recording/playback, and direct-level regressions.

Update the durable development and behavior records with the proven C# cause
and fix, without presenting the result as new arcade evidence. Save a paired
report with the changed files, root-cause evidence, validation results,
limitations, and manual acceptance still required.

Do not launch the game, stage, commit, or push. After automated validation,
provide Neal the launch command for manual acceptance of the no-input control,
the reproduced failure sequence, and Level 5-to-6 progression.
