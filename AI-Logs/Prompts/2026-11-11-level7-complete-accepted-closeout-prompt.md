FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: CLOSE OUT THE COMPLETE ACCEPTED LEVEL 7 MILESTONE, COMMIT, AND PUSH

This is the final closeout of the accumulated accepted Level 7 candidate.

Neal explicitly authorizes:

- durable documentation updates;
- final bounded validation;
- staging the intended complete accepted Level 7 milestone;
- creating one Git commit;
- pushing that commit to the configured upstream GitHub repository.

Do not begin Level 8 work.

Do not reopen accepted Level 7 speed tuning.

Do not make unrelated gameplay changes.


STARTING CHECKPOINT AND REQUIRED REPORTS

Before editing, run:

git status --short --branch
git diff --stat
git diff --cached --stat
git rev-parse --abbrev-ref HEAD
git rev-parse HEAD
git rev-parse --abbrev-ref --symbolic-full-name '@{u}'

The expected branch is master tracking origin/master.

The accepted Level 6 baseline at the start of Level 7 work was:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

Read the relevant local AI reports needed to understand and close out the accumulated Level 7 candidate, especially:

AI-Logs/Reports/2026-11-11-level7-spiker-tanker-speed-match-diagnostic-report.md

AI-Logs/Reports/2026-11-11-level7-expanded-spiker-speed-and-slow-recording-diagnostic-report.md

AI-Logs/Reports/2026-11-11-level7-recorded-headless-behavioral-audit-report.md

Also inspect other directly relevant Level 7 reports already present under AI-Logs/Reports when necessary to accurately identify the full accumulated candidate, including the accepted concurrency, admission, reconstruction, movement, and repeated-cycle Spiker-firing work.

AI-Logs are historical/working artifacts. Do not stage or commit AI-Logs.

Do not modify historical reports merely to rewrite history. Create a new closeout report.


WORKTREE SAFETY

This worktree contains a large, intentional, accumulated Level 7 candidate.

Do not:

- reset;
- restore;
- clean;
- checkout over changes;
- discard files;
- replace the candidate with HEAD;
- stage before understanding the complete diff.

Before making closeout edits:

1. Inventory all modified and untracked files.
2. Classify each as:
   - accepted Level 7 implementation;
   - accepted Level 7 tests;
   - accepted recording/playback diagnostic support;
   - accepted project-instructions maintenance;
   - durable documentation requiring closeout update;
   - ignored AI log;
   - local recording or TEMP material;
   - unrelated or uncertain.
3. Stop and report any genuinely unrelated or uncertain tracked change before staging.

Do not stage:

- AI-Logs;
- TEMP;
- GameplayRecordings;
- generated recordings;
- local videos;
- ROM/reference-user assets;
- build output;
- unrelated personal files.


MANUAL ACCEPTANCE: LEVEL 7 SPEED FIDELITY

Neal has completed manual comparison against arcade Tempest and MAME.

Treat the following as accepted manual fidelity results:

1. Tanker movement speed and Spiker movement speed should be the same.

2. Tanker-shot speed and Spiker-shot speed should be the same.

3. Tanker and Spiker shots travel visibly faster than the enemies that fired them.

4. The accepted C# relationship is:
   - Tanker climb rate: the existing Tanker.DevelopmentClimbSpeed;
   - Level 7 Spiker movement: the same Tanker movement rate;
   - Tanker and Spiker shots: the existing shared EnemyBullet movement.

5. Repeated-cycle Spiker firing remains correct:
   - Spikers continue firing during later ascent cycles;
   - this behavior must remain intact.

6. Neal judges the resulting Level 7 speeds to play extremely close to the arcade game, with the overall speeds appearing effectively identical.

7. No further Level 7 speed changes are wanted.

This manual acceptance converts the Tanker-match relationship from a development-only hypothesis into the accepted normal Level 7 production behavior.

Do not describe the literal C# numeric value as ROM-measured unless the inspected evidence directly establishes that.

The acceptance is based on Neal's direct arcade/MAME comparison and manual gameplay judgment.


PROMOTE THE ACCEPTED RELATIONSHIP TO NORMAL LEVEL 7

Inspect the current implementation and make the smallest production change required so that normal no-option Level 7 uses the accepted relationship:

Level 7 Spiker movement uses the same effective movement rate as normal Level 7 Tanker movement.

Do not duplicate the numeric value if the existing Tanker movement authority can be referenced safely and clearly.

Prefer one maintainable shared authority for the accepted Tanker/Spiker movement relationship.

Do not broadly merge Tanker and Spiker classes.

Do not change Tanker movement.

Do not change EnemyBullet movement.

Do not change projectile cadence or update order.

Do not change Spiker firing cadence, ammunition, firing holdoff, repeated-cycle firing, retreat behavior, Spike growth, or lifecycle behavior.

Do not modify Levels 1-6.


DEVELOPMENT DIAGNOSTIC CLEANUP

Review the existing development-only controls:

--level7-spiker-speed <value>

--level7-spiker-match-tanker-speed

The accepted normal Level 7 production behavior now matches the Tanker relationship.

Perform only narrow, safe cleanup.

Requirements:

1. Normal no-option Level 7 must use the accepted Tanker-matched relationship.

2. Do not retain a diagnostic flag whose only effect is now identical to normal production behavior unless retaining it is necessary for backward compatibility with the accepted recording format or existing local recordings.

3. Preserve existing version-1 and version-2 recording compatibility.

4. Do not invalidate the existing local recording:

   Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json

5. If removing the command-line Tanker-match option while continuing to load the corresponding optional recording metadata is the safest compatibility design, do that and explain it.

6. If retaining the option is simpler and harmless, it may remain DEVELOPMENT ONLY, but document that it is now equivalent to normal accepted Level 7 behavior.

7. The numeric --level7-spiker-speed override may remain as a DEVELOPMENT-ONLY calibration/diagnostic seam if it is already well-tested and does not affect normal play.

8. Do not broaden this into a command-line-system cleanup.

9. Preserve --speed and the accepted recording/playback improvements.

10. Do not remove useful recording compatibility merely because manual calibration is complete.


TANKER-SURGE CONCLUSION

Record this conclusion accurately:

Neal initially considered the late-Level-7 Tanker surge a possible C#/arcade discrepancy because C# could show several Tankers entering near the end of the wave.

After further manual testing, Neal reproduced the same type of Level 7 end-of-wave Tanker surge in MAME.

The reproduction required unusual timing:

- the Blaster died near the end of Level 7;
- approximately seven or fewer enemies remained;
- death occurred at a narrow point in the remaining-enemy lifecycle.

Neal no longer considers the Tanker surge a C# discrepancy or gameplay issue.

It is a rare scenario in ordinary gameplay.

Do not implement a Tanker cap.

Do not change admission, reconstruction, concurrency, enemy supply, or Tanker creation because of this behavior.

Do not preserve the earlier speculative Spiker-to-Tanker explanation as established fact.

If mentioned historically, classify that explanation only as a discarded/unproven hypothesis.


RECORDED HEADLESS AUDIT CONCLUSION

Record the useful result without overstating it:

The real recording:

Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json

successfully completed bounded headless playback using all 2,808 recorded input frames.

It progressed from Level 7 Playing into Level 8 Approach.

The existing headless CLI established recording compatibility, startup restoration, finite completion, and final state.

Detailed per-tick telemetry was not completed because Windows Application Control blocked:

tests\Tempest.Game.Tests\bin\Debug\net9.0\Tempest.Game.Tests.dll

with error:

0x800711C7

The blocked result was handled correctly:

- security was not weakened;
- the same blocked artifact was not repeatedly retried;
- the temporary diagnostic source was removed;
- no telemetry success was claimed.

Treat this audit as:

- successful proof that real gameplay recordings can drive bounded headless playback;
- partial for detailed behavioral telemetry;
- not a Level 7 gameplay blocker;
- useful future testing direction.

Do not implement a permanent telemetry framework during this closeout.


DURABLE DOCUMENTATION

Update the appropriate durable project documents, especially:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Inspect their current contents and make focused updates that accurately record the accepted Level 7 milestone.

Document at minimum:

1. Level 7 is manually accepted.

2. Level 7 Spiker movement uses the same accepted movement relationship as Tankers.

3. Tanker and Spiker shots share the same accepted shot behavior and visibly travel faster than their firing enemies.

4. Repeated-cycle Spiker firing is accepted.

5. Level 7 speed fidelity is closed; no further speed tuning is wanted.

6. The rare end-of-wave Tanker surge was reproduced in MAME and is not considered a C# discrepancy.

7. The accumulated accepted Level 7 concurrency, admission, reconstruction, cargo, movement, and firing work included in this candidate is closed, based on the actual inspected diff and preceding reports.

8. The real Level 7 recording successfully completed bounded headless playback into Level 8 Approach.

9. Fine-grained playback telemetry remains a future optional diagnostic direction and is not required for Level 7 acceptance.

Use the project's established evidence classifications carefully:

ARCADE VERIFIED / ARCADE OBSERVED:
Only Neal's directly observed and manually accepted gameplay relationships.

MAME OBSERVED:
Only behavior Neal reproduced in MAME.

C# MEASURED:
Exact code values and automated/headless results.

PROJECT DECISION:
Implementation policy not literally established by arcade internals.

DEVELOPMENT ONLY:
Diagnostic overrides, half-speed observation, recording seams, and temporary calibration controls.

Do not claim the C# numeric Tanker speed is ROM-derived if it is only Incitti-derived/current-C# implementation evidence.


TEST AND CODE CLEANUP

Before final validation:

1. Review all newly added and modified tests.

2. Keep tests that protect accepted production behavior, recording compatibility, repeated-cycle firing, admission/reconstruction, cargo allocation, and Level 7 movement.

3. Keep useful deterministic diagnostic tests where they provide durable regression value.

4. Remove only clearly obsolete calibration-only assertions that conflict with the accepted normal production relationship.

5. Do not delete broad test coverage merely to simplify the diff.

6. Ensure comments describe accepted behavior accurately and distinguish arcade observation from implementation mechanism.

7. Do not perform broad refactoring.


FINAL VALIDATION

Run bounded validation appropriate to the complete accepted milestone.

At minimum run:

1. Focused Level 7 validation covering:
   - Level 7 production;
   - admission/concurrency;
   - wave selection;
   - reconstruction/ledger behavior;
   - ordinary and cargo Flipper movement;
   - Tanker behavior;
   - Spiker movement;
   - repeated-cycle Spiker firing;
   - projectile behavior;
   - recording/playback startup metadata;
   - relevant command-line parsing.

2. Full Level 7 regression subset.

3. Recording/playback regression subset.

4. Full solution test suite:

   dotnet test TempestCSharp.slnx --no-restore

5. Solution build:

   dotnet build TempestCSharp.slnx --no-restore

6. Whitespace validation:

   git diff --check
   git diff --cached --check

Use explicit reasonable bounds.

Do not launch interactive Tempest.

Neal has already supplied final manual Level 7 acceptance.

If Windows Application Control blocks a generated artifact:

- do not weaken security;
- do not bypass it;
- do not repeatedly retry the same blocked artifact;
- do not substitute stale binaries;
- do not claim the blocked test passed.

Because commit and push are authorized only for a known-good milestone, if required final validation is blocked or fails and cannot be resolved safely, STOP before commit and push.

Create a BLOCKED closeout report describing the exact artifact, command, error, completed work, and remaining validation.


STAGING REVIEW

Before staging:

1. Show:

   git status --short --branch
   git diff --stat
   git diff --name-status

2. Confirm every intended tracked and untracked source/test/documentation file belongs to this accepted Level 7 milestone.

3. Confirm ignored AI-Logs, TEMP, recordings, videos, build output, and local assets are excluded.

4. Stage only the intended accepted project files.

5. Show:

   git status --short --branch
   git diff --cached --stat
   git diff --cached --name-status

6. Run:

   git diff --cached --check

7. Inspect the staged diff sufficiently to confirm:
   - accepted Level 7 production behavior is included;
   - required tests are included;
   - durable documentation is included;
   - no AI logs or local recordings are staged;
   - no unrelated files are staged.


COMMIT AND PUSH AUTHORIZATION

If and only if all required validation passes and the staged inventory is correct:

1. Commit the complete accepted Level 7 milestone.

Use a concise descriptive commit message, preferably:

Complete accepted Level 7 behavior

A slightly more specific message is acceptable if the inspected final scope warrants it.

2. Capture the new commit hash.

3. Show the commit summary.

4. Verify the worktree/index state after commit.

5. Push the current master branch to its configured upstream origin/master.

6. Verify the push completed successfully.

7. Verify local HEAD and origin/master refer to the new commit.

Do not force-push.

Do not amend an earlier commit.

Do not create multiple commits unless an unavoidable technical reason is discovered and clearly reported before committing.


AI LOGS

Save this substantive prompt under:

AI-Logs/Prompts/2026-11-11-level7-complete-accepted-closeout-prompt.md

Create the closeout report under:

AI-Logs/Reports/2026-11-11-level7-complete-accepted-closeout-report.md

AI-Logs remain ignored and must not be staged or committed.

The report must exist and be verified before claiming completion.

The report should include:

- starting branch, HEAD, upstream, and worktree;
- complete final scope;
- files changed;
- documentation updates;
- manual acceptance recorded;
- Tanker-surge conclusion;
- headless playback audit conclusion;
- diagnostic cleanup decisions;
- exact focused validation results;
- exact Level 7 regression result;
- exact recording/playback regression result;
- exact full-suite result;
- exact build result;
- diff-check results;
- staged file inventory;
- confirmation that AI-Logs, TEMP, recordings, videos, ROM/reference-user assets, and build output were not staged;
- commit message;
- new commit hash;
- push result;
- verification that local HEAD and origin/master match;
- final Git status;
- unresolved issues, if any;
- explicit statement that Level 7 is closed and Level 8 work has not begun.


FINAL SUCCESS CRITERIA

The task is complete only if:

- accepted Tanker-matched Spiker behavior is normal Level 7 production behavior;
- repeated-cycle Spiker firing remains correct;
- projectile behavior remains correct;
- the rare late-wave Tanker surge is documented as reproduced in MAME and not a discrepancy;
- durable documentation is updated;
- focused tests pass;
- Level 7 regression passes;
- recording/playback regression passes;
- full solution tests pass;
- build passes with zero errors;
- diff checks pass;
- intended files only are staged;
- one commit is created;
- the commit is pushed successfully to origin/master;
- local HEAD and origin/master are verified equal;
- the required prompt and closeout report exist;
- Level 8 work has not begun.

If any required condition is not met, report PARTIAL or BLOCKED and do not claim success.


STOP CONDITION

After successful documentation, final validation, staging review, commit, push, verification, closeout report, and final status, STOP.

Do not begin Level 8.

Do not start another fidelity investigation.

Do not launch interactive Tempest.

TEMPEST_LEVEL7_COMPLETE_ACCEPTED_CLOSEOUT_COMMIT_PUSH_20261111