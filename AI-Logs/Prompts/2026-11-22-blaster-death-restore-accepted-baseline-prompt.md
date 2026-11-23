Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Restore and freeze the last accepted Blaster-death presentation

The experimental Blaster-death refinement has completed its final evaluation.

Neal's manual result is:

**The ROM-derived version is visibly different, but it is not better than the previously accepted Blaster-death presentation.**

This is the hard stop previously established for this refinement effort.

Do not perform another investigation.

Do not attempt another animation, geometry, color, timing, scale, or cadence refinement.

Do not start Level 12 in this task.

## Objective

Restore only the Blaster-death presentation to its last manually accepted state from before the current refinement effort.

Remove the unaccepted experimental changes introduced by:

1. the stepped Blaster-death schedule implementation;
2. the ROM-derived 24-segment Blaster-death geometry implementation.

Preserve all unrelated accepted work.

After restoration:

- validate that the prior accepted Blaster-death implementation is restored;
- document that the ROM-derived refinement was investigated, implemented, manually evaluated, and rejected as not better;
- leave the repository ready for the next normal milestone;
- do not commit or push unless explicitly authorized elsewhere in this task.

## Accepted restoration target

Restore the Blaster-death visual presentation that existed at the clean accepted baseline before this refinement began.

The relevant accepted Git baseline is the production Fuseball visual-animation closeout commit:

aa6b7110fe845cf36173a80b7dc5d11c1f067806

Commit message:

Animate production Fuseballs

Use Git/diff/history surgically to identify the exact Blaster-death state at that baseline.

Do not reset the repository to that commit.

Do not restore unrelated files wholesale.

Do not discard later unrelated tracked changes or historical records.

The baseline is an evidence source for the exact accepted Blaster-death implementation, not authorization for a repository-wide rollback.

## Restore the previous accepted Blaster-death behavior

The restored effect should contain the previously accepted presentation characteristics, including:

- deterministic 60-tick death presentation;
- existing accepted explosion center;
- three fixed layered starburst outlines;
- yellow/orange-red/white presentation;
- existing layer-specific scale;
- existing fixed layer rotations;
- asymmetric 16-point radius pattern;
- accepted 150% peak-size presentation;
- previously accepted smooth expansion/contraction radius envelope;
- clean disappearance at the established death boundary;
- shared underlying local effect in legacy and Atari presentation.

Restore the exact previously accepted implementation from repository history rather than reconstructing it from this prose if there is any discrepancy.

## Remove the unaccepted stepped-schedule implementation

Remove/revert only the production/test code introduced by the unaccepted stepped-animation experiment, including as applicable:

- BlasterDeathStage;
- BlasterDeathStageSchedule;
- discrete nine-stage death schedule;
- translated durations 6,6,6,6,6,12,9,6,3;
- stage-scale lookup that replaced the accepted smooth envelope;
- tests whose sole purpose is asserting that unaccepted discrete schedule.

Do not remove historical AI-Logs documenting the experiment.

Those records should remain as historical evidence that the approach was tested and rejected.

## Remove the unaccepted ROM-geometry implementation

Remove/revert only the production/test code introduced by the unaccepted final ROM-geometry experiment, including as applicable:

- BlasterDeathSegment;
- the 24-segment local ROM table;
- the Blaster-death-specific 13/17 coordinate scale;
- bounds-center (10,4) transform used only by that experimental geometry;
- project-local code-9/10/11 color translation introduced solely for that geometry;
- tests whose sole purpose is asserting the unaccepted 24-segment topology/translation;
- obsolete helper code created only for that experiment.

Restore the accepted three-layer Blaster-death geometry from the baseline.

Do not remove shared utilities used by other renderers or Fuseball work.

## Preserve accepted Fuseball work

The production Fuseball visual-animation milestone is accepted, closed, committed, and pushed.

Preserve it exactly.

Do not alter:

- production four-phase Fuseball animation;
- modulo-16 visual counter;
- divide-by-four phase selection;
- Fuseball geometry;
- Fuseball colors;
- production animation activation;
- Fuseball gameplay;
- terminal-rimward behavior;
- Fuseball identity/lifecycle.

The restoration must not undo commit aa6b7110… or any accepted Fuseball behavior.

## Preserve Level 11 and other accepted game state

Do not alter accepted Level 11 or earlier gameplay/presentation.

Preserve at minimum:

- genuine Level 11 geometry;
- Level 11 calibration;
- spinner correction;
- Atari model-frame mapping;
- Spike-height correction;
- terminal Spiker-to-Tanker correction;
- Fuseball terminal behavior;
- Atari HUD;
- Levels 1-10;
- direct Level 15 candidate;
- playback;
- reverse playback;
- screenshot behavior;
- Approach;
- ExitZoom.

Do not use restoration as an opportunity to clean up unrelated code.

## Historical records

Keep all AI-Logs from this Blaster-death refinement.

They are historical records and should document:

- the visual-frame investigation;
- ROM selector/timing investigation;
- stepped-animation implementation;
- final ROM-geometry investigation;
- ROM-geometry implementation;
- Neal's final manual rejection.

Do not rewrite old reports to pretend the experiments never occurred.

Instead, create this restoration/closeout report stating clearly:

- the previous C# presentation was already manually accepted;
- additional fidelity experiments were conducted;
- the scale-only stepped candidate produced no meaningful visible improvement;
- the ROM-derived geometry candidate was visibly different but not better;
- Neal therefore rejected the refinement;
- the last accepted presentation was restored;
- no further Blaster-death refinement is wanted at this time.

## Durable documentation

Review durable documentation only for statements accidentally introduced by the unaccepted candidate.

If durable documentation has not yet been updated to claim the experimental Blaster-death behavior is accepted, do not add new behavioral documentation merely for this rejected experiment.

If documentation does contain unaccepted stepped/ROM-geometry claims, restore those statements to the accepted pre-refinement description.

A short durable note that the prior accepted Blaster-death presentation remains authoritative is acceptable only if necessary to avoid ambiguity.

Do not clutter durable documentation with the full failed experiment history; AI-Logs already preserve it.

## Repository protection

Before restoration:

1. Verify branch, HEAD, upstream, ahead/behind, index, tracked worktree changes, and untracked files.
2. Identify precisely which current modifications belong to the two Blaster-death experiments.
3. Identify unrelated pending tracked changes and preserve them.
4. Do not assume every change after aa6b7110 should be reverted.

Do not use:

- repository-wide git reset --hard;
- broad git restore .;
- stash;
- clean;
- rebase.

Use surgical file/hunk restoration or equivalent bounded edits.

Do not alter ignored/local assets.

Do not inspect protected ROM files, screenshots, UserData, Temp, Videos, or unrelated references.

## Validation

After restoration, prove that the accepted pre-refinement Blaster-death implementation is restored.

Run bounded:

1. Existing accepted Blaster-death presentation tests from the clean baseline.
2. Death/re-entry/lives regressions.
3. Final-life/Game Over regressions.
4. Playback/reverse-playback determinism tests.
5. Legacy/Atari presentation regressions.
6. Fuseball visual-animation regressions.
7. Level 11 regressions relevant to player death.
8. Relevant Levels 1-10 regressions.
9. Direct Level 15 preservation where appropriate.
10. Complete solution test suite.
11. Release solution build.
12. git diff --check.

Do not retain tests that assert only the rejected stepped schedule or rejected ROM geometry.

Verify specifically:

- the accepted smooth death radius behavior is back;
- accepted three-layer geometry is back;
- accepted 150% size remains;
- 60-tick lifecycle remains;
- gameplay state remains unchanged;
- Fuseball animation remains accepted and unchanged.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Git comparison

After restoration, compare the Blaster-death production/test code against the corresponding state at:

aa6b7110fe845cf36173a80b7dc5d11c1f067806

The Blaster-death-specific implementation should match the accepted baseline except for any clearly unrelated accepted change that occurred independently.

Do not require the entire repository to match that commit.

Report any remaining Blaster-death code difference and justify it explicitly.

## Commit/push boundary

Do not create a production commit or push in this task unless the only changes requiring persistence are documentation/AI-Logs specifically authorized below.

Preferred outcome:

- production/test Blaster-death code returns to the already committed accepted baseline;
- rejected experimental source/test changes disappear from the worktree;
- historical AI-Logs remain;
- the new restoration/closeout prompt/report remain as tracked-history candidates for the next normal commit.

Do not create a standalone commit solely for failed-experiment bookkeeping.

Do not amend the Fuseball commit.

## Final report

Save this prompt and a paired restoration report under the tracked AI-Logs convention.

Report:

- starting HEAD;
- files/hunks restored;
- rejected stepped-schedule code removed;
- rejected ROM-geometry code removed;
- accepted Blaster-death presentation restored;
- comparison against aa6b7110…;
- validation results;
- Fuseball animation preservation;
- Level 11 preservation;
- Neal's rejection rationale: “different, but not better”;
- final index/worktree state;
- historical AI-Logs left intact.

End with a clear disposition:

**BLASTER-DEATH REFINEMENT CLOSED — PREVIOUS ACCEPTED PRESENTATION RESTORED**

Do not recommend another Blaster-death investigation.

Do not begin Level 12.

TEMPEST_BLASTER_DEATH_RESTORE_ACCEPTED_BASELINE