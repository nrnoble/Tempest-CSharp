FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: COMPLETE ACCEPTED LEVEL 9 CLOSEOUT, FINAL VALIDATION, COMMIT, AND PUSH

This is the final closeout of the accumulated Level 9 implementation candidate and Level 9 Spiker/projectile correction.

Neal explicitly authorizes:

- final Level 9 production cleanup needed to record the accepted candidate;
- durable documentation updates;
- final bounded automated validation;
- staging the intended complete accepted Level 9 milestone;
- creating one Git commit;
- pushing that commit to the configured upstream GitHub repository.

Do NOT begin Level 10.

Do NOT start the deferred whole-game difficulty/timing investigation.

Do NOT start any other deferred post-Level-9 milestone.


STARTING CHECKPOINT AND CANDIDATE

The accepted committed baseline before Level 9 work is:

037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9

Commit subject:

Complete accepted Level 7 behavior

The complete Level 9 candidate remains uncommitted on top of that checkpoint.

Before editing, run:

git status --short --branch
git diff --stat
git diff --name-status
git diff --cached --stat
git rev-parse HEAD
git rev-parse origin/master
git rev-parse --abbrev-ref HEAD
git rev-parse --abbrev-ref --symbolic-full-name '@{u}'
git ls-files --others --exclude-standard

Expected:

- branch master tracking origin/master;
- local HEAD and origin/master both at the checkpoint above;
- index empty;
- accumulated Level 9 candidate present;
- current candidate approximately matches the most recent Level 9 reports:
  - 22 modified tracked files;
  - three untracked candidate/test files;
  - plus ignored AI-Logs records.

Do not assume counts if the actual inventory differs slightly because of legitimate candidate integration.

Instead compare every path against the implementation/correction reports.

If an unrelated or unexplained tracked/untracked project file is present, STOP before staging and report it.

Do not reset, restore, clean, checkout over changes, or discard candidate work.


REQUIRED REPORTS

Read completely:

AI-Logs/Reports/2026-11-11-level9-bounded-production-implementation-report.md

AI-Logs/Reports/2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-report.md

AI-Logs/Reports/2026-11-11-level9-spiker-relative-speed-correction-calibration-report.md

Also inspect as needed:

AI-Logs/Reports/2026-11-11-level9-status-rom-preimplementation-investigation-report.md

and directly relevant Level 8/Level 9 historical reports necessary to distinguish accepted implementation from investigation history.

Read current:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

.github/copilot-instructions.md

Do not rewrite historical AI reports.

AI-Logs are ignored historical/working artifacts and must not be staged.


MANUAL ACCEPTANCE SUPPLIED BY NEAL

Record Neal's manual acceptance accurately.

LEVEL 9 OVERALL MECHANICS:

PASS.

Neal manually played the current Level 9 candidate and reports that the overall Level 9 mechanics pass manual testing.

The original implementation was already judged impressive and played well on its first attempt.

Accepted Level 9 mechanics include the candidate's:

- stepped open-tube gameplay;
- 15 playable lanes / open endpoints;
- starting Spike behavior/presentation at the current candidate level;
- Tanker behavior;
- Tanker cargo behavior;
- Level 9 $53 Spike-sensitive Flipper behavior;
- ordinary enemy composition;
- Spiker lifecycle;
- repeated-cycle Spiker firing;
- firing behavior;
- wave behavior;
- reconstruction architecture;
- gameplay progression/terminal behavior insofar as exercised and covered by automation.

Do not claim Neal manually inspected invisible ledger identities or every synthetic edge case. Those remain automated-test evidence.


SPIKER / PROJECTILE MANUAL ACCEPTANCE

The original Level 9 candidate had a blocking defect:

Spiker projectiles could lag behind their firing Spikers.

Neal manually rejected that behavior.

The correction replaced Level 9's accidental old randomized Spiker-speed path with an explicit normal Level 9 candidate speed of:

2.0

The shared enemy projectile remains:

3.0

Neal has now manually tested the corrected candidate and reports:

THE PROBLEM IS CORRECTED.

Spiker shots visibly travel faster than the firing Spiker.

The blocking projectile-lag defect is therefore manually closed.

Record this as:

ARCADE/MAME-OBSERVED FIDELITY REQUIREMENT:
Spiker projectiles must travel visibly faster than their firing Spiker.

MANUALLY ACCEPTED LEVEL 9 RESULT:
The corrected Level 9 candidate satisfies that relationship.

C# PROJECT CALIBRATION:
The current Level 9 normal Spiker authority is 2.0 and shared projectile movement is 3.0.

Do NOT describe the numerical 2.0 value as a ROM measurement.


ABSOLUTE LEVEL 9 SPIKER SPEED — IMPORTANT QUALIFICATION

Do NOT claim Neal separately verified that absolute Level 9 Spiker speed 2.0 exactly matches the arcade.

Neal reports that C# currently appears significantly more difficult/faster overall than the arcade game across the implemented levels.

Because that broader whole-game difference affects subjective speed judgment, Neal cannot reliably isolate whether Level 9's absolute Spiker speed itself precisely matches the arcade.

Therefore record:

- Level 9's blocking relative-speed defect is fixed and accepted.
- Level 9 mechanics pass.
- Absolute Level 9 Spiker-speed arcade equivalence is NOT independently established.
- No further Level-9-only speed tuning is wanted during closeout.
- Retuning Level 9 in isolation risks compensating for a broader cross-level difficulty/timing issue.


DEFERRED WHOLE-GAME DIFFICULTY / TIMING INVESTIGATION

Record a distinct future milestone:

WHOLE-GAME DIFFICULTY / TIMING INVESTIGATION

Neal observes that the C# recreation currently feels significantly more difficult/faster than the arcade across the implemented levels.

A moderately experienced Tempest player should not find Level 9 particularly challenging, while the current C# build remains challenging.

Do NOT assume the cause is one global movement-speed multiplier.

Potential contributors may include:

- enemy movement timing;
- rim pursuit;
- player/enemy shot timing;
- shot lifetime/capacity;
- collision ordering;
- pivot vulnerability/shootability;
- concurrency/admission;
- firing pressure;
- other simulation/presentation timing interactions.

This closeout does NOT investigate or change those systems.

The prior difficulty investigation remains historical evidence.

Do not reopen it now.

Classify this as a separate cross-level investigation after Level 9.


CROSS-LEVEL SPIKER SPEED FOLLOW-UP

The Level 9 speed investigation found that the inherited old randomized Spiker-speed path remains in:

- Level 4 candidate behavior where applicable;
- production Level 5;
- production Level 6;
- production Level 8.

Those configurations may permit a Spiker to exceed the shared projectile movement rate.

Do NOT modify Levels 4, 5, 6, or 8 during this closeout.

Levels 1-8 are accepted/frozen for this milestone.

Record this as a future bounded cross-level Spiker/projectile invariant investigation.

The durable intended fidelity relationship is:

For every firing Spiker, projectile effective rimward displacement must be greater than the firing Spiker's effective rimward displacement over equivalent active simulation intervals, unless a future compelling arcade observation explicitly establishes otherwise.

Absolute Spiker speed may remain level-specific.

The unresolved ROM/static-analysis contradiction must remain documented rather than hidden.


LEVEL 9 PRODUCTION SPEED POLICY CLEANUP

Review:

--level9-spiker-speed <value>

The normal production value 2.0 is now the accepted Level 9 project calibration for purposes of closing the mechanically accepted Level 9 candidate, subject to the global-difficulty qualification above.

Requirements:

1. Normal Level 9 must retain 2.0 production Spiker speed.

2. Projectile movement remains unchanged at 3.0.

3. The development-only Level 9 numeric override may remain because it is useful for future diagnostics/calibration and is already tested.

4. Clearly classify the override as DEVELOPMENT ONLY.

5. Diagnostic override metadata compatibility in recording/playback may remain.

6. Do not remove backward-compatible startup handling needlessly.

7. Do not broaden this into a command-line cleanup.

8. Do not modify Level 7's accepted policy.

9. Do not modify Levels 4/5/6/8.

10. Do not change random-number consumption on accepted earlier levels.


LEVEL 9 ACCEPTED PRODUCTION SCOPE

Close the accumulated Level 9 production candidate including:

- Level 9 geometry/topology;
- shape row 13;
- open 16-vertex / 15-playable-lane tube;
- endpoint behavior;
- Level 9 configuration;
- 27-unit generic pending wave;
- Tanker/Spiker ordinary selection policy;
- ordinary Flipper/Pulsar/Fuzzball exclusion;
- six-unit ordinary active gate;
- Tanker latent cargo accounting;
- cargo release above ordinary gate;
- final-pending Spiker-to-Tanker conversion;
- 15 starting Spikes;
- current accepted/project-translated starting Spike height;
- Level 9 $53 live-Spike-sensitive Flipper policy;
- natural/shallow $07 Tanker cargo;
- deep shot-released $53 Tanker cargo;
- firing configuration;
- repeated-cycle Spiker firing;
- generic post-death reconstruction;
- corrected Level 9 Spiker/projectile relative-speed relationship;
- natural Level 8 -> Level 9 progression;
- direct --level 9;
- Level 9 recording/playback/headless-startup support;
- current terminal Level 9 boundary;
- Level 10 rejection.

Preserve evidence classifications.

Do not convert project translations into ROM-backed facts.


LEVEL 8 -> LEVEL 9 PROGRESSION

If Neal has not explicitly supplied a separate manual acceptance statement for the exact natural Level 8 -> Level 9 transition, do NOT fabricate one.

Record automated progression coverage accurately.

Manual Level 9 gameplay acceptance is sufficient for this milestone's implemented behavior, but distinguish any transition behavior that remains automation-only.

Do not block closeout merely because a separately itemized transition observation was not supplied unless existing project policy requires it.


DURABLE DOCUMENTATION

Update:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record the Level 9 accepted candidate and evidence accurately.

At minimum document:

1. Level 9 implementation is manually accepted for overall mechanics.

2. Level 9 uses the ROM/binary-backed stepped open topology with 15 playable lanes.

3. Level 9 $53 Spike-sensitive Flipper behavior and $07/$53 Tanker-cargo distinction are part of the accepted implementation.

4. The Level 9 blocking Spiker/projectile lag defect was corrected.

5. Normal Level 9 production Spiker speed is the C# project calibration 2.0.

6. Shared projectile movement remains 3.0.

7. Neal manually accepted the relative relationship:
   projectile faster than firing Spiker.

8. Do not claim 2.0 is an exact arcade/ROM numerical speed.

9. Absolute Level 9 speed fidelity remains entangled with a deferred whole-game difficulty/timing discrepancy.

10. Whole-game difficulty/timing investigation is deferred and not a Level 9 blocker.

11. Levels 4/5/6/8 inherited Spiker-speed relationship risk remains a separate future investigation.

12. Level 10 remains unimplemented.

13. Accepted Levels 1-8 remain frozen.

14. Level 9 recording/playback calibration support remains development tooling where appropriate.

15. The ROM/static Level 9 Spiker/projectile equal-speed contradiction remains unresolved.

Preserve existing evidence labels such as:

ROM-LISTING-BACKED
REV-2-BINARY-CONFIRMED
ARCADE/MAME OBSERVED
MANUALLY ACCEPTED
C# MEASURED
PROJECT TRANSLATION
PROJECT CALIBRATION / PROJECT DECISION
DEVELOPMENT ONLY
UNRESOLVED


DEFERRED POST-LEVEL-9 ROADMAP

Where the development notes maintain future work, preserve the already established deferred milestones without implementing them:

1. REVERSE PLAYBACK STEPPING PERFORMANCE

Improve held reverse-stepping responsiveness while preserving exact single-frame reverse taps.

If external key-repeat limits prevent sufficiently fast single-frame held reversal, investigate accelerated multi-frame reverse stepping while held.

2. ENEMY-SHOT RENDERING FIDELITY

Replace the generic enemy-shot appearance with much closer arcade/ROM-faithful vector rendering.

Use the accepted Blaster-shot ROM reconstruction methodology as the engineering precedent.

Do not change projectile gameplay behavior merely for presentation.

3. WHOLE-GAME SCREEN / TUBE / HUD PRESENTATION

Refine internal screen presentation, tube perspective/projection/scale/vertical placement, and HUD score/lives layout without changing gameplay geometry/mechanics.

Use Level 9 as the strongest initial calibration reference because its C#/arcade perspective mismatch is visually pronounced.

Regression-check earlier accepted tubes.

All sixteen distinct tube layouts should ultimately be defined once and recycle from Level 17 onward.

Internal gameplay screen dimensions should remain constant across the game.

Fullscreen and/or borderless fullscreen-window presentation is desirable.

4. WHOLE-GAME DIFFICULTY / TIMING

Investigate why C# currently feels significantly more difficult/faster than arcade across implemented levels.

Do not presume a single global speed multiplier.

5. CROSS-LEVEL SPIKER / PROJECTILE INVARIANT

Investigate/remediate Levels 4/5/6/8 separately so future implemented levels do not rediscover the projectile-lag issue.

Do not implement any of these deferred milestones during Level 9 closeout.


FINAL CODE / TEST REVIEW

Before validation:

1. Inspect the entire accumulated Level 9 diff.

2. Verify all production edits belong to the accepted Level 9 milestone or its Spiker-speed correction.

3. Verify all new/untracked test/source files belong to Level 9.

4. Retain useful speed diagnostic tests and recording compatibility.

5. Remove only clearly obsolete candidate assertions that conflict with accepted normal Level 9 production behavior.

6. Do not remove diagnostic coverage merely to reduce diff size.

7. Ensure comments distinguish PROJECT CALIBRATION 2.0 from ROM evidence.

8. Ensure no comment claims the unresolved ROM/arcade speed contradiction was solved.

9. Ensure no accepted Level 1-8 behavior was changed during closeout.


FINAL VALIDATION

Run bounded final validation.

If an already-running Tempest.Game process still locks the default bin output:

- do not terminate a user process;
- do not enter same-command retry loops;
- use the previously validated safe isolated-output technique if needed;
- do not weaken Windows security;
- clearly report the validation route.

At minimum run:

A. Focused Level 9 tests covering:
- production configuration;
- geometry/topology;
- $53;
- Tanker cargo;
- Spikes;
- wave selection;
- concurrency;
- firing;
- repeated Spiker firing;
- reconstruction;
- relative Spiker/projectile speed;
- progression;
- direct startup;
- recording/playback;
- terminal boundary.

B. Complete Level 9 test subset.

C. Levels 1-8 targeted regression.

D. CLI/recording/playback/startup regression.

E. Relevant speed-selection regression.

F. Full solution tests:

dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal

If default outputs are locked, use the known safe isolated-output route while retaining the hang timeout.

G. Full solution build:

dotnet build TempestCSharp.slnx --no-restore --verbosity minimal

Use isolated output if required by an existing process lock.

H. Whitespace validation:

git diff --check

Before commit also run:

git diff --cached --check

Do not launch interactive Tempest.

Neal has already supplied the Level 9 manual acceptance required for this closeout.

A blocked, canceled, timed-out, stalled, or hung command is NOT a pass.


STAGING SAFETY REVIEW

Before staging run:

git status --short --branch
git diff --stat
git diff --name-status
git ls-files --others --exclude-standard

Classify all candidate files.

Stage ONLY intended accepted project files for Level 9 and its closeout documentation.

Do NOT stage:

- AI-Logs;
- TEMP;
- gameplay recordings;
- screenshots;
- UserData;
- Videos;
- protected ROM/reference-user assets;
- bin/obj;
- isolated validation output;
- session-workspace output;
- local scratchpad files;
- unrelated material.

After staging run:

git status --short --branch
git diff --cached --stat
git diff --cached --name-status
git diff --cached --check

Inspect the staged diff enough to establish:

- complete accepted Level 9 implementation is staged;
- Level 9 speed correction is staged;
- required new Level 9 source/tests are staged;
- durable documentation is staged;
- no ignored/local/user material is staged;
- no Level 10 work is staged;
- no unrelated Levels 1-8 production changes are staged.


COMMIT AND PUSH AUTHORIZATION

If and only if:

- final required validation passes;
- candidate inventory is understood;
- staged inventory is correct;
- no unrelated changes are included;

then create ONE commit.

Preferred subject:

Complete accepted Level 9 behavior

A slightly more descriptive subject is acceptable if the final reviewed scope warrants it.

After commit:

1. capture full commit hash;
2. show commit summary;
3. run git status;
4. verify index/worktree tracked state;
5. push current master to configured origin/master;
6. do not force-push;
7. fetch/update remote-tracking state if needed;
8. verify local HEAD and origin/master are the same full hash.

If required validation cannot complete safely, STOP BEFORE COMMIT/PUSH and create a BLOCKED closeout report.


AI LOGS

Save the complete substantive task under:

AI-Logs/Prompts/2026-11-12-level9-complete-accepted-closeout-prompt.md

Create a NEW closeout report under:

AI-Logs/Reports/2026-11-12-level9-complete-accepted-closeout-report.md

Do not rewrite historical implementation/investigation reports.

AI-Logs remain ignored and must not be staged.

The closeout report must exist and be read back or explicitly verified before final success is claimed.


REQUIRED CLOSEOUT REPORT

Include:

1. Starting branch/HEAD/origin checkpoint.
2. Starting candidate inventory.
3. Level 9 final production scope.
4. Neal's manual mechanics acceptance.
5. Corrected Spiker/projectile manual acceptance.
6. Exact qualification on absolute Spiker-speed fidelity.
7. Level 9 normal 2.0 project calibration.
8. Shared projectile 3.0 behavior.
9. Unresolved ROM/static speed contradiction.
10. $53 and cargo behavior.
11. Geometry/topology/presentation evidence.
12. Reconstruction and wave-selection state.
13. Progression/direct-start/terminal behavior.
14. Recording/playback status.
15. Whole-game difficulty/timing deferred milestone.
16. Levels 4/5/6/8 Spiker/projectile follow-up.
17. Other deferred post-Level-9 presentation/tooling milestones.
18. Durable documentation updates.
19. Files included in final milestone.
20. Focused validation results.
21. Level 9 regression result.
22. Levels 1-8 regression result.
23. Recording/playback/startup regression result.
24. Full solution result.
25. Build result.
26. diff-check results.
27. Validation output route and any process lock encountered.
28. Exact staged inventory.
29. Confirmation no AI-Logs/TEMP/recordings/UserData/videos/ROM/local validation output was staged.
30. Commit subject.
31. Full new commit hash.
32. Push result.
33. Local HEAD/origin-master equality verification.
34. Final Git status.
35. Explicit statement LEVEL 9 IS CLOSED.
36. Explicit statement Level 10 has not begun.
37. Remaining deferred milestones only, with no implication they block Level 9.


FINAL SUCCESS CRITERIA

Claim success only if:

- Level 9 accepted implementation is complete;
- Spiker/projectile lag defect is closed;
- normal Level 9 retains 2.0 production calibration;
- shared projectile speed remains unchanged;
- Level 9 mechanics are documented as manually accepted;
- absolute speed qualification is documented correctly;
- global difficulty issue is deferred rather than incorrectly "fixed";
- Levels 1-8 are preserved;
- full validation passes;
- durable docs are updated;
- correct candidate only is staged;
- commit succeeds;
- push succeeds;
- HEAD and origin/master match afterward;
- required closeout report exists;
- Level 10 has not begun.

If any required condition fails, report PARTIAL or BLOCKED and do not claim successful closeout.


STOP CONDITION

After successful Level 9 documentation, validation, staged-inventory review, commit, push, post-push verification, closeout report, and final Git status, STOP.

Do not begin Level 10.

Do not begin whole-game difficulty investigation.

Do not begin cross-level Spiker remediation.

Do not begin reverse-playback work.

Do not begin enemy-shot rendering.

Do not begin global presentation work.

TEMPEST_LEVEL9_COMPLETE_ACCEPTED_CLOSEOUT_COMMIT_PUSH_20261112