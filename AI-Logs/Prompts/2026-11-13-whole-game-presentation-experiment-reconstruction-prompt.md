IMPORTANT COMPLETENESS CHECK

Read this entire user-provided task before taking any action.

Before using tools, reading project files, running commands, modifying files, or performing repository actions, locate the unique completion token near the end of this task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code or agent-generated reminders, tagged-file information, attachment metadata, or other automatically appended context may appear after the user's completion token and do not invalidate it.

This check exists to detect an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before taking any project or repository action and explain why.


TASK

Reconstruct a narrowly bounded, reversible, uncommitted experimental candidate for the Tempest whole-game presentation calibration.

Start from the clean accepted baseline:

branch:
master

HEAD:
385f603a94391756b2ca6cccd524dc17b281e782

commit:
Close out ROM-backed enemy-shot rendering

origin/master should match the same commit.

The user deliberately rolled the repository back to this clean baseline after unrelated experimental work performed while Copilot was unavailable.

Do NOT attempt to recover, reconstruct, inspect, or reason from discarded experimental work, deleted temporary files, Gemini-generated changes, or any previous uncommitted implementation remnants.

Reconstruct the candidate cleanly from the accepted project and the authoritative investigation reports described below.

This remains an EXPERIMENTAL PRESENTATION CANDIDATE.

Do not commit or push.


STARTING SAFETY AND GIT STATE

Before implementation, run bounded checks of:

- current branch;
- HEAD;
- origin/master;
- staged changes;
- tracked worktree changes;
- non-ignored untracked files;
- diff summary.

Expected state:

- master
- HEAD = 385f603a94391756b2ca6cccd524dc17b281e782
- origin/master matches HEAD
- tracked worktree clean
- index clean
- no relevant untracked files

If the repository does not match that clean baseline, STOP before implementation and report the discrepancy.

Do not reset, clean, restore, delete, overwrite, or otherwise modify unexpected user work.


AUTHORITATIVE INVESTIGATION REPORTS

After the starting Git checks, read:

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-readonly-investigation-report.md

AI-Logs/Reports/2026-11-12-whole-game-presentation-level9-label-correction-report.md

For every image-dependent conclusion, the label-correction report supersedes the original investigation report.

Authoritative visual provenance:

LEFT = current C# presentation.
RIGHT = MAME/reference presentation.
RIGHT/MAME = target.

Do not reverse those labels.

If the prior experimental implementation report still exists, you may read:

AI-Logs/Reports/2026-11-12-whole-game-presentation-experiment-implementation-report.md

Treat that report only as historical documentation of the previously attempted candidate. Reimplement against the current 385f603 source rather than blindly recreating code structure from historical descriptions.


VISUAL-ASSET RULE

Do not search for, enumerate, open, inspect, extract, batch-process, or analyze repository screenshots, images, video files, recordings, MAME frames, C# frames, contact sheets, or other visual assets during this implementation task.

The visual investigation has already been performed.

Use the authoritative investigation reports.

No new image/frame/video analysis is authorized by this task.


PRIMARY GOAL

Use Level 9 as the strongest calibration fixture for a shared whole-game presentation model.

The desired direction established by the investigation is a portrait-like Tempest presentation resembling the RIGHT-side MAME reference.

The original experimental starting hypothesis was:

- accepted host window remains 1280 x 960;
- internal presentation starting hypothesis = 600 x 800 / 3:4;
- centered portrait viewport starting hypothesis = 720 x 960;
- approximately 280 pixels of host-window space on each side;
- uniform output scale starting hypothesis = 1.2x;
- additional shared playfield scale starting hypothesis = 1.05x;
- initial vertical playfield translation starting hypothesis = -78;
- existing projection K=70 remains unchanged initially;
- existing ZOffset=5 remains unchanged initially.

These are PROJECT TRANSLATION / EXPERIMENTAL values.

They are not ROM-defined values.
They are not arcade hardware specifications.
They are not accepted final calibration values.

Reconstruct the conservative candidate around these starting values unless current source architecture requires a materially safer equivalent representation.


CRITICAL SIZE CONSTRAINT

Do NOT treat 720 x 960 as a predetermined box into which the existing game must simply be shrunk.

The user's explicit objective is not to sacrifice the apparent size of the game/tube merely to satisfy a chosen application viewport.

The MAME/reference presentation's relative tube prominence and placement are the target.

The earlier investigation observed approximately:

- current C# tube = about 61% of panel width and 46-49% of panel height;
- MAME reference tube = about 90% of panel width and 61% of panel height.

Those values are observations of the supplied comparison image, not arcade hardware specifications.

The previous experimental candidate reportedly maintained Level 9 at at least approximately 88% of the portrait viewport width.

Use that only as a regression/invariant target for this experimental translation, not as proof of an arcade constant.

If satisfying an arbitrary viewport dimension would require making the tube conspicuously smaller than the target presentation, do not shrink the game merely to satisfy the viewport hypothesis.


GAMEPLAY GEOMETRY IS FROZEN

Preserve every gameplay/model coordinate and all accepted mechanics.

Do NOT change presentation by modifying:

- LevelDefinition.Points;
- lane topology;
- tube connectivity;
- collision geometry;
- enemy paths;
- player paths;
- projectile mechanics;
- enemy mechanics;
- movement timing;
- firing timing;
- wave behavior;
- scoring;
- level progression;
- level-specific mechanics.

Presentation must remain a rendering/presentation concern.

Level 9 is a calibration fixture, not authorization for Level-9-specific gameplay or geometry changes.


ONE SHARED PLAYFIELD PRESENTATION TRANSFORM

Apply one coherent shared presentation transformation to the complete rendered playfield.

The transform must maintain alignment among presentation elements that occupy the same game space, including as applicable:

- tube;
- Blaster;
- enemies;
- player shots;
- enemy shots;
- Spikes;
- explosions/effects;
- score popups or other gameplay-space effects;
- transition presentation elements associated with playfield space;
- Game Over vector effects or other presentation elements that must remain spatially aligned.

Do not independently "fix" individual gameplay entities with unrelated offsets merely to make screenshots look correct.


HUD IS INDEPENDENT

Treat HUD/screen-message layout separately from the playfield transform where appropriate.

Re-layout score, lives, level information, and screen-space messages for the portrait presentation without changing gameplay coordinates.

Do not distort gameplay geometry to solve HUD placement.


LEVEL 9 IS NOT A SPECIAL CASE

Level 9 is the strongest calibration reference, but the resulting presentation architecture must be shared across the game.

Do NOT introduce Level-9-only scale values, coordinate inversions, projection signs, translations, camera rules, or presentation constants merely to make Level 9 match.

The same presentation model must support Levels 1-9.


IMPORTANT CROSS-LEVEL FAIL-CLOSED RULE

The user performed unrelated discarded experimentation while Copilot was unavailable and observed a significant failure mode:

Level 9 could be made visually correct while earlier levels became inverted. Attempts to compensate then led to additional rendering problems.

Do not reproduce that failure mode.

If the shared presentation candidate causes any previously correct tube to become:

- inverted;
- mirrored;
- orientation-reversed;
- topologically malformed;
- rendered through the wrong side of the tube;
- grossly displaced;
- clipped because of an incorrect shared transform;
- or otherwise fundamentally misrendered,

STOP further corrective implementation.

Do NOT introduce per-level sign changes, inversion fixes, compensating offsets, point rewrites, projection hacks, or special-case transformations to patch individual tubes.

Instead:

1. preserve the worktree at the first diagnosable state;
2. identify the common presentation/projection assumption responsible if possible;
3. state which levels/tests expose the problem;
4. report whether the candidate is PARTIAL or BLOCKED;
5. leave the issue for Neal's review.

A cross-level failure is evidence that the shared presentation model needs correction, not permission to patch individual level geometry.


PROJECTION / PERSPECTIVE DISCIPLINE

The earlier investigation found evidence that scale and translation alone might not ultimately reproduce the Level 9 MAME/reference shoulder/far-rim relationship.

Do NOT try to solve that unresolved perspective question aggressively in this first candidate.

Keep the established K=70 and ZOffset=5 unchanged initially.

Do not invent projection constants from the comparison image.

The purpose of this candidate is to establish the shared portrait presentation, playfield prominence, placement, and HUD layout first.

Projection/depth calibration can be a later bounded pass after Neal manually evaluates this candidate.


LEGACY A/B COMPARISON

Provide a simple development comparison mechanism:

Normal startup should use the experimental presentation candidate.

The command-line option:

--legacy-presentation

should select the previously accepted 385f603 presentation.

The switch must affect presentation only.

It must not select different gameplay geometry, mechanics, level definitions, timing, randomness, enemy behavior, or simulation behavior.

The intended manual Level 9 comparison is:

dotnet run --project src\Tempest.Game -- --level 9

versus:

dotnet run --project src\Tempest.Game -- --level 9 --legacy-presentation

Keep this mechanism narrowly scoped and maintainable.


ARCHITECTURE AND MAINTAINABILITY

Inspect the current 385f603 rendering architecture before implementing.

Prefer a centralized, explicit presentation configuration/transformation rather than scattering portrait constants or coordinate corrections throughout unrelated rendering code.

Comments should explain:

- why the presentation abstraction exists;
- distinction between host window, internal presentation, projection/game coordinates, playfield transform, and HUD layout;
- which values are experimental project translations;
- why gameplay/model geometry must remain untouched.

Do not over-engineer a generalized resolution framework.

We need the smallest maintainable abstraction that supports this experiment safely.


REFERENCE AUTHORITY

Do not search the web.

Do not consult external Tempest implementations.

Do not independently introduce Mark Incitti implementation values into this milestone.

If existing project code contains historically derived Incitti-related choices, do not change them merely because of this instruction, but do not use Incitti as authority for new portrait-presentation constants.

For this milestone, use:

1. the accepted current project;
2. the authoritative presentation investigation reports;
3. established project behavior/specification documents where directly relevant;
4. the experimental values explicitly authorized in this task.

Do not allow a reference implementation to override Neal's arcade/MAME observations or accepted gameplay.


BOUNDED AUTOMATED VALIDATION

Add or update focused automated tests only where they provide useful protection for the presentation architecture.

At minimum, validate where practical:

- legacy presentation remains the established presentation;
- experimental presentation has the intended portrait relationship;
- Level 9 retains strong tube prominence rather than being merely shrunk to fit;
- presentation selection does not alter gameplay/model coordinates;
- shared transformation behavior does not introduce level-specific presentation rules;
- representative Levels 1-9 remain correctly oriented under the shared transformation;
- display/transition behavior affected by the presentation change remains coherent.

Use existing appropriate regression tests as well.

All test/analysis/command loops must be explicitly bounded.

If a command stalls, hangs, times out, or encounters a security block, follow the project's established self-stop policy.

Do not report canceled, timed-out, blocked, or incomplete commands as passed.


MANUAL ACCEPTANCE BOUNDARY

Do NOT launch the interactive game yourself for manual acceptance.

After automated validation, stop and provide Neal with the commands required for manual evaluation.

Manual visual acceptance belongs to Neal.

The primary first manual test is Level 9.

After Level 9 appears directionally correct, Neal will regression-check earlier tubes before this candidate is accepted.

Do not describe the experiment as accepted merely because automated tests pass.


DOCUMENTATION / AI LOGGING

This is a substantial Tempest task.

Preserve the substantive prompt under AI-Logs/Prompts according to the established project instructions.

Create a new ignored reconstruction/implementation report under AI-Logs/Reports with a descriptive 2026-11-13 filename.

The report should record:

- starting baseline;
- why the presentation candidate was reconstructed;
- implementation performed;
- experimental presentation values;
- gameplay-geometry preservation;
- HUD treatment;
- legacy A/B mechanism;
- cross-level orientation/regression protection;
- automated validation actually completed;
- anything blocked or unresolved;
- manual validation still required;
- final Git state.

Do not rewrite historical reports.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md with accepted-presentation claims yet.

This candidate has not received manual acceptance.


NO COMMIT / NO PUSH

Do not stage.
Do not commit.
Do not push.

Leave the candidate inspectable and reversible in the worktree for Neal's manual testing.


FINAL RESPONSE

Report:

1. Whether reconstruction completed successfully.
2. Starting branch/HEAD/origin state.
3. Files changed/created.
4. Exact experimental presentation model implemented.
5. Confirmation that gameplay/model coordinates were preserved.
6. HUD treatment.
7. Legacy A/B mechanism.
8. Cross-level orientation/regression validation performed.
9. Focused test results.
10. Full-suite result, if completed.
11. Build result.
12. git diff --check result.
13. Any unresolved perspective/projection question.
14. Any blocked or partial work.
15. Exact commands Neal should use for Level 9 experimental and legacy comparison.
16. Recommended manual checks.
17. Report path.
18. Final Git state.
19. Explicit confirmation that nothing was staged, committed, or pushed.
20. Explicit confirmation that no visual assets were analyzed.

Stop after automated validation and hand off to Neal for manual gameplay/presentation acceptance.

TEMPEST_PRESENTATION_RECONSTRUCTION_EXPERIMENT_20261113