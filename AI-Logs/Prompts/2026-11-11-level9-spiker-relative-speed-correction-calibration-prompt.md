FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: BOUNDED LEVEL 9 SPIKER RELATIVE-SPEED CORRECTION AND MANUAL CALIBRATION CANDIDATE

This task corrects the identified Level 9 Spiker/projectile relative-speed defect while preserving the remainder of the current Level 9 implementation candidate.

This is a bounded candidate implementation followed by automated validation and Neal's manual calibration.

Do NOT close Level 9.

Do NOT commit or push.

Do NOT change Levels 1-8.

Do NOT begin Level 10.


STARTING CANDIDATE

Read completely:

AI-Logs/Reports/2026-11-11-level9-bounded-production-implementation-report.md

AI-Logs/Reports/2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-report.md

Before changing anything, verify:

git status --short --branch
git diff --stat
git diff --name-status
git diff --cached --stat
git rev-parse HEAD
git rev-parse origin/master

Expected:

- HEAD and origin/master remain:
  037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9
- the complete uncommitted Level 9 candidate is present;
- index is empty;
- candidate inventory matches the Level 9 implementation and relative-speed reports.

If it differs materially, STOP before editing.

Do not reset, restore, clean, checkout, discard, stage, commit, or push.


CONFIRMED LEVEL 9 DEFECT

The read-only diagnostic established the current C# cause.

Normal Level 9 falls through to the old general Spiker speed expression:

ProgressionIndex / 48f + 2.5f + (float)(random.NextDouble() * 2)

At Level 9 this produces approximately:

2.6666667 through 4.666667

per Spiker.

Enemy projectiles move at:

3.0

per active simulation step.

Therefore current Level 9 production permits:

Spiker < 3:
projectile pulls ahead.

Spiker == 3:
projectile and Spiker remain equal.

Spiker > 3:
Spiker gains on its own projectile.

The latter two outcomes violate Neal's observed arcade behavior.

The randomized Level 9 range was not an accepted Level 9 fidelity decision. It is an inherited fallback implementation path.


ARCADE/MAME FIDELITY REQUIREMENT

Neal's repeated manual arcade/MAME observations establish the project fidelity requirement:

While a firing Spiker and its projectile are both traveling toward the rim:

THE PROJECTILE MUST TRAVEL VISIBLY FASTER THAN THE SPIKER.

The Spiker must never:

- catch its own projectile;
- overtake its projectile;
- visibly outpace it;
- leave the projectile lagging behind during the same ascent.

Projectile-to-Spiker separation must increase.

This requirement applies across observed Tempest levels.

The ABSOLUTE Spiker speed may vary by level.

Do not interpret this as requiring one universal Spiker speed.


ROM CONTRADICTION REMAINS UNRESOLVED

The immediately preceding read-only investigation traced Level 9 ROM speed processing.

It found:

Spiker raw input:
$C0

Enemy projectile raw input:
$C0

Both crack_speed paths produce:

$FE00

The bounded static trace found equal effective raw displacement in the normal active path.

That does NOT agree with Neal's repeated arcade/MAME observation of visibly faster projectiles.

The report classified this contradiction:

UNRESOLVED

Preserve that classification.

Do NOT:

- claim the ROM is wrong;
- claim Neal's repeated arcade observation is wrong;
- claim equal raw $C0 inputs establish visible equal arcade speed;
- claim this C# correction is a literal ROM speed translation.

The C# correction is a PROJECT FIDELITY / MANUAL CALIBRATION decision pending Neal.


PRIMARY IMPLEMENTATION GOAL

Remove Level 9's accidental dependency on the old randomized general Spiker speed formula.

Add an explicit Level 9 production Spiker-speed policy.

The policy must guarantee:

maximum effective Level 9 Spiker ascent rate
<
Level 9 effective projectile ascent rate

over equivalent active simulation intervals.

The current projectile rate remains 3.0.

Do NOT change EnemyBullet speed.

Do NOT change Tanker speed.

Do NOT change Flipper speed.

Do NOT change firing cadence.

Do NOT change repeated-cycle Spiker firing.

Do NOT change Spiker reversal/retreat behavior.

Do NOT change Level 9 geometry, $53 behavior, Tanker cargo, wave selection, reconstruction, Spikes, progression, recording semantics, or any accepted Level 1-8 behavior.


PROVISIONAL LEVEL 9 PRODUCTION CALIBRATION

The current C# comparable Level 9 movement authorities are:

Tanker normal climb:
1.75

Flipper normal along climb:
2.0

Enemy projectile:
3.0

The ROM does not currently establish which C# number should be used for the Level 9 Spiker.

Therefore use the current Level 9 Flipper normal climb rate:

2.0

as the PROVISIONAL LEVEL 9 SPIKER PRODUCTION CALIBRATION CANDIDATE.

This is NOT ROM-BACKED.

Classify it explicitly as:

PROJECT CALIBRATION CANDIDATE — MANUAL ACCEPTANCE PENDING NEAL

Reasons for this provisional candidate:

- it is an existing Level 9 enemy movement authority rather than another arbitrary high randomized value;
- it guarantees the 3.0 projectile moves visibly faster;
- it preserves a meaningful speed margin;
- it does not reuse the exact Level 7 1.75 value merely because Level 7 was accepted;
- it gives Neal a concrete normal-production Level 9 candidate to judge.

Do not claim these reasons establish arcade correctness.

Neal remains the final acceptance authority.


DEVELOPMENT-ONLY LEVEL 9 CALIBRATION OVERRIDE

Also provide a tightly scoped DEVELOPMENT-ONLY Level 9 Spiker-speed override so Neal can compare a small useful range without editing production code repeatedly.

Prefer:

--level9-spiker-speed <value>

Accepted diagnostic range:

1.0 through 2.75 inclusive.

Why stop below 3.0:

The purpose of this diagnostic is manual calibration within the already-established strict projectile-faster requirement.

Do not permit 3.0 or above here.

Required useful values include:

1.75
2.0
2.25
2.5
2.75

Do not hard-code separate modes.

Requirements:

- invariant numeric parsing;
- finite values only;
- reject malformed input;
- reject repeated/conflicting input;
- reject below 1.0;
- reject 3.0 or above;
- Level-9-only;
- normal no-option Level 9 uses provisional 2.0;
- Levels 1-8 ignore/remain unaffected.

Reuse the existing diagnostic/recording architecture where practical rather than inventing a new subsystem.


RECORDING AND PLAYBACK

The Level 9 speed override changes deterministic startup/gameplay state.

If Neal records using:

--level9-spiker-speed

the selected value must be faithfully restored during playback.

Use the existing recording-v2 optional-startup-metadata pattern where appropriate.

Preserve existing recording compatibility.

Requirements:

1. Old recordings without Level 9 override metadata continue to load normally.
2. Normal new Level 9 recordings with no override use the production candidate 2.0.
3. Diagnostic recordings preserve the selected Level 9 override.
4. Playback restores the recorded diagnostic value automatically.
5. Direct conflicting --level9-spiker-speed with --playback-file should be rejected where appropriate.
6. --speed remains presentation-only and independent.
7. Do not redesign the recording format broadly.

If existing generic infrastructure can represent this safely without a new recording-format version, prefer that approach.


CROSS-LEVEL SPEED POLICY SEAM

The read-only investigation recommended a future common per-level Spiker movement policy with documented per-level authorities and a relative-speed invariant.

Implement only the MINIMUM architecture needed to prevent Level 9 from silently falling through to the old randomized formula.

If a small maintainable per-level Spiker-speed selection seam can be introduced safely:

- Level 7 remains its accepted Tanker-derived authority;
- Level 9 uses its explicit provisional 2.0 authority;
- other existing levels retain their existing behavior for now.

Do NOT fix Levels 4, 5, 6, or 8 in this task.

Do NOT make the cross-level invariant part of production runtime behavior.

Do NOT silently alter random-number consumption on accepted Levels 1-8.

Do NOT generalize farther than needed for Level 9.


LEVEL 9 REGRESSION INVARIANT

Add automated Level 9 coverage proving:

For normal production Level 9:

effective Spiker rimward displacement
<
effective projectile rimward displacement

over equivalent active simulation steps.

At provisional normal Level 9:

Spiker = 2.0
Projectile = 3.0

Over 10 matched active ascent steps:

Spiker travel should be approximately:
20

Projectile travel:
30

Separation growth:
approximately +10

Projectile/Spiker travel:
approximately 1.5

Use real movement paths/methods where practical rather than merely asserting constants.

Also test shot-origin/update ordering.

A newly fired projectile may first advance on the following appropriate step; that one-step age difference must not be confused with the steady-state relative-rate invariant.


DIAGNOSTIC OVERRIDE INVARIANT

For every accepted --level9-spiker-speed value:

Spiker effective movement must remain strictly below projectile effective movement.

Test at least:

1.0
1.75
2.0
2.25
2.5
2.75

Also test rejection of:

3.0
greater than 3.0
zero
negative
NaN
Infinity
malformed input
repeated option

Do not accept an epsilon-below-3 value unless it falls inside the intentionally supported range.

The range ceiling is deliberately 2.75 to retain a visibly meaningful margin for testing.


BROADER CROSS-LEVEL FINDING

The preceding diagnostic found that the old general randomized formula also feeds firing Spikers on:

- the Level 4 candidate;
- production Level 5;
- production Level 6;
- production Level 8.

Those levels may be able to select Spiker rates above the shared 3.0 projectile rate.

This is a FOLLOW-UP FINDING.

Do NOT fix those levels now.

Do NOT alter their tests merely to satisfy a new global invariant.

Do NOT hide this finding.

Record it in the implementation report as a separately scoped future cross-level remediation milestone.

Level 7 already satisfies the accepted relationship and must remain frozen.


PRESERVE THE SUCCESSFUL LEVEL 9 CANDIDATE

The current bounded Level 9 candidate otherwise plays well according to Neal's initial manual testing.

Freeze all unrelated Level 9 behavior during this correction.

Do not change:

- tube geometry;
- orientation;
- open endpoints;
- lane count;
- starting Spikes;
- $53 behavior;
- $07/$53 cargo boundary;
- Tanker behavior;
- Flipper behavior;
- wave composition;
- admission/concurrency;
- final Spiker-to-Tanker conversion;
- generic reconstruction;
- firing eligibility;
- repeated-cycle firing;
- Super Zapper;
- Level 8 -> 9 progression;
- Level 9 terminal boundary;
- HUD;
- renderer;
- playback controls.

This milestone changes only Spiker SPEED POLICY and whatever diagnostic/recording plumbing is strictly required for manual calibration.


AUTOMATED TESTS

At minimum add/update focused tests for:

1. Normal Level 9 production Spiker speed is exactly the explicit provisional 2.0 policy.

2. Normal Level 9 no longer consumes the old randomized speed selection.

3. Accepted Level 7 remains on its existing Tanker-derived policy.

4. Existing Levels 1-8 other than Level 7 retain their current speed-selection behavior.

5. Level 9 production Spiker/projectile 10-step separation increases.

6. Level 9 projectile remains 3.0.

7. Level 9 Tanker and Flipper existing speed behavior is unchanged.

8. Diagnostic override accepts 1.0 through 2.75 according to the intended range.

9. Diagnostic override rejects invalid/equal-or-faster cases.

10. Level 9 repeated-cycle Spiker firing remains intact under:
    - normal production speed;
    - at least one diagnostic override.

11. Recording round-trip preserves a Level 9 diagnostic override.

12. Existing recording files without this metadata remain compatible.

13. Level 9 visible/headless playback startup remains deterministic.

14. $53, cargo, reconstruction, progression, and geometry tests remain unchanged and passing.


AUTOMATED VALIDATION

Run bounded validation.

At minimum:

A. Focused Level 9 speed/firing/recording tests.

B. Complete Level 9 test subset.

C. Directly affected CLI/recording/playback tests.

D. Levels 1-8 targeted regression proving this correction did not alter accepted prior behavior.

E. Full solution:

dotnet test TempestCSharp.slnx --no-restore --blame-hang-timeout 5min --verbosity minimal

F. Build:

dotnet build TempestCSharp.slnx --no-restore --verbosity minimal

G. Whitespace:

git diff --check

Do not launch interactive Tempest.

Use the project's security/stall policy.

If Windows Application Control blocks an artifact:

- do not weaken security;
- do not bypass it;
- do not repeatedly retry the same artifact;
- try only materially different safe validation routes;
- report BLOCKED if required validation cannot complete.

A timeout, canceled run, blocked command, hung process, or stalled command is not a pass.


MANUAL CALIBRATION HANDOFF

After automated validation, STOP for Neal.

Give exact commands for:

NORMAL LEVEL 9 PRODUCTION CANDIDATE — SPIKER 2.0

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1

LEVEL 9 — SPIKER 1.75

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 1.75

LEVEL 9 — SPIKER 2.25

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.25

LEVEL 9 — SPIKER 2.5

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.5

LEVEL 9 — SPIKER 2.75

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --level9-spiker-speed 2.75

HALF-SPEED RECORDING OF NORMAL 2.0 CANDIDATE

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5 --record-gameplay Temp\GameplayRecordings\level9-spiker-speed-2.0.json

HALF-SPEED RECORDING WITH AN OVERRIDE, EXAMPLE 2.5

dotnet run --project src\Tempest.Game -- --level 9 --lives 10 --spinner-sensitivity 1 --speed 0.5 --record-gameplay Temp\GameplayRecordings\level9-spiker-speed-2.5.json --level9-spiker-speed 2.5

Provide exact playback commands using the actual implementation.

If the syntax differs, provide the real tested syntax rather than these conceptual examples.


NEAL'S MANUAL ACCEPTANCE QUESTIONS

For each relevant setting judge:

A. ABSOLUTE SPIKER SPEED

Compared with arcade/MAME:

- clearly too slow;
- somewhat too slow;
- approximately correct;
- somewhat too fast;
- clearly too fast.

B. PROJECTILE RELATIONSHIP

Does a fired projectile:

- pull away strongly;
- pull away moderately;
- pull away slightly;
- appear nearly equal;
- lag behind?

Any lagging result is unacceptable.

C. REVERSAL

Before the Spiker reverses, does the projectile gain a natural visible lead?

The prior bug where the Spiker reverses while an obviously lagging projectile remains behind must be gone.

D. LEVEL 9 GAMEPLAY PRESSURE

Does ordinary Level 9 remain close to arcade/MAME gameplay?

E. REGRESSION

Confirm:

- $53 still looks correct;
- repeated firing still works;
- Tankers/cargo still behave normally;
- geometry and starting Spikes remain correct;
- no unrelated Level 9 regression is visible.


NO ACCEPTANCE CLAIM YET

Do not declare 2.0 correct merely because automated tests pass.

Manual acceptance remains PENDING NEAL.

If Neal later prefers 1.75, 2.25, 2.5, 2.75, or another supported value, that future decision becomes the production calibration only after explicit authorization.


DURABLE DOCUMENTATION

Do not yet update TEMPEST-BEHAVIOR-SPEC.md to claim the Level 9 speed is accepted.

Do not broadly update TEMPEST-DEVELOPMENT-NOTES.md during this correction unless needed for an implementation-status note.

The final accepted value and the cross-level fidelity invariant should be durably documented during Level 9 acceptance/closeout after Neal's testing.


GIT SAFETY

Preserve the complete uncommitted Level 9 candidate.

Do NOT:

- stage;
- commit;
- push;
- reset;
- restore;
- clean;
- checkout over candidate files;
- discard unrelated candidate changes.

At completion show:

git status --short --branch
git diff --stat
git diff --name-status

Clearly distinguish files changed specifically by this speed correction from the pre-existing Level 9 candidate.


AI LOGS

Save the complete substantive task under:

AI-Logs/Prompts/2026-11-11-level9-spiker-relative-speed-correction-calibration-prompt.md

Create the implementation report under:

AI-Logs/Reports/2026-11-11-level9-spiker-relative-speed-correction-calibration-report.md

AI-Logs remain ignored and must not be staged.

The task is not complete until both files exist and the report has been read back or explicitly verified.


REQUIRED REPORT

Include:

1. Starting candidate/Git state.
2. Manual defect being corrected.
3. Arcade/MAME relative-speed mandate.
4. Unresolved ROM contradiction.
5. Old Level 9 randomized speed path.
6. New explicit Level 9 production policy.
7. Evidence classification of provisional 2.0.
8. Diagnostic override implementation/range.
9. Recording/playback metadata behavior.
10. Level 7 preservation.
11. Levels 4/5/6/8 follow-up finding without changes.
12. Relative-speed automated measurements.
13. Focused test results.
14. Level 9 regression result.
15. Levels 1-8 regression result.
16. Recording/playback regression result.
17. Full-suite result.
18. Build result.
19. git diff --check result.
20. Exact manual calibration commands.
21. Manual acceptance checklist.
22. Files changed specifically by this correction.
23. Final Git status.
24. Explicit confirmation shared projectile speed is unchanged.
25. Explicit confirmation Levels 1-8 were not intentionally changed.
26. Explicit confirmation no commit/push occurred.
27. Manual acceptance status: PENDING NEAL.


STOP CONDITION

After the Level 9 speed-policy correction, diagnostic override, automated validation, recording/playback support, required AI logs, and exact manual calibration handoff are complete, STOP.

Do not launch Tempest.

Do not choose a final accepted Level 9 speed for Neal.

Do not modify Levels 4/5/6/8.

Do not stage, commit, or push.

Do not begin Level 10.

Wait for Neal's manual arcade/MAME calibration result.

TEMPEST_LEVEL9_SPIKER_RELATIVE_SPEED_CORRECTION_CALIBRATION_20261111
