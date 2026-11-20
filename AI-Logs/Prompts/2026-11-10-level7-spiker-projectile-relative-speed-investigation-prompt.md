FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

This check detects incomplete or truncated user tasks, not appended agent
context.

If exactly one complete TEMPEST_ token cannot be established, stop before any
project or repository action and explain why.


TASK

Perform a bounded READ-ONLY investigation of canonical Level 7 Spiker movement
speed relative to enemy-projectile movement speed.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted committed baseline beneath the current uncommitted Level 7 candidate:

8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b

The complete current Level 7 candidate is already present in the worktree,
including:

- the Level 7 concurrency candidate;
- seven-place admission and pending-lane Spike selection;
- retained live Spiker behavior while generic pending supply remains;
- saturated Tanker-cargo allocation correction;
- repeated-cycle Spiker firing correction.

Preserve the candidate exactly.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not stage.
Do not commit.
Do not push.
Do not launch the interactive game.
Do not begin Level 8 work.


READ FIRST

Read:

AI-Logs/Reports/2026-11-10-level7-spiker-repeated-cycle-firing-investigation-report.md

AI-Logs/Reports/2026-11-10-level7-spiker-repeated-cycle-firing-correction-report.md

AI-Logs/Reports/2026-11-10-level7-gameplay-pressure-readonly-diagnostic-report.md

AI-Logs/Reports/2026-11-10-level7-concurrency-and-movement-rom-investigation-report.md

Use the project-local Atari listing as the primary internal arcade source:

Reference/arcade/Tempest Rom Source code.md

Do not search the Internet for ROM/disassembly behavior.


CURRENT CANDIDATE STATUS

The repeated-cycle Spiker firing correction achieved its primary visible goal:

- Spikers can now fire during later ascent cycles;
- firing is no longer structurally dependent on Growth > 100;
- persistent Spikes continue growing across cycles;
- persistent Spike hammering/shortening remains intact.

Manual Level 7 acceptance is still pending because this correction exposed a
newly observable relative-speed mismatch.

Automated validation is also not yet complete:

- new Level 7 Spiker firing tests: 6 passed;
- focused Spiker / Level 7 / ledger regressions: 123 passed;
- affected Release regressions: 804 passed;
- full Release suite: 1,217 passed, 1 failed, 0 skipped;
- the one failure was
  GameOverDensityDiagnosticTests.OmittedAndExplicitTwoRenderIdenticalGameOverMarks;
- Release build was not run after that failure;
- final git diff --check was not run after that failure.

Do not investigate that Game Over test in this read-only speed task.

Do not rerun broad validation.

Preserve and report the outstanding validation state.


NEW MANUAL EVIDENCE

Treat the following as ARCADE/MAME/C# OBSERVED evidence supplied by Neal.

ARCADE / MAME:

- A Spiker projectile travels up the tube at a perceptibly faster constant
  rate than the Spiker that fired it.
- The projectile does not merely remain barely ahead.
- The distance between the projectile and the firing Spiker visibly increases.
- A Spiker never catches or overtakes its own shot.
- Projectile motion appears constant rather than accelerating.
- Neal's illustrative example is approximately:
  - Spiker at 10 mph;
  - projectile at 15 mph.
- That example communicates the qualitative relationship only.
- It is not a measured ratio and must not be used as a tuning constant.

CURRENT C#:

- Enemy projectiles appear to travel at a normal-looking speed.
- Enemy projectiles can overtake Tankers, which qualitatively agrees with
  MAME/arcade observation.
- A Level 7 Spiker can nevertheless catch or overtake its own shot.
- Therefore the C# Spiker/projectile relative travel rate is visibly wrong.

The required visible invariant is:

A Spiker projectile must travel at a perceptibly faster constant along-tube
rate than its firing Spiker, causing their separation to increase after the
shot is fired.

The Spiker must never catch or overtake its own projectile while both remain
in comparable along-tube travel.


PURPOSE

Determine which C# quantity or update rule causes the Level 7 Spiker to catch
its own projectile.

Determine whether a future correction should change:

- canonical Level 7 Spiker movement speed;
- enemy-projectile movement speed;
- movement cadence/update frequency;
- coordinate conversion;
- another narrow mechanism;
- or neither until additional evidence is available.

Do not implement the correction in this task.


IMPORTANT PRESUMPTION BOUNDARY

Neal's current perception is that C# projectile speed looks normal.

That makes Spiker speed the stronger initial suspect, but it is not yet a
conclusion.

Do not lower Spiker speed merely because it is the obvious candidate.

Trace both sides of the relative movement relationship before recommending a
change.


PRIMARY QUESTIONS

Answer:

1. What raw Level 7 ROM values feed Spiker along-tube speed?
2. What raw Level 7 ROM values feed enemy-projectile along-tube speed?
3. How does the ROM transform each raw value into movement state?
4. How does the ROM apply each movement value during gameplay?
5. How often does the Spiker receive an along-tube movement operation?
6. How often does an enemy projectile receive one?
7. Are both values expressed in the same fixed-point representation?
8. Do they use the same coordinate orientation and scale?
9. Does one path negate, complement, shift, carry, accumulate, or otherwise
   interpret its value differently?
10. Does the ROM produce a projectile-vs-Spiker relationship consistent with
    Neal's verified MAME observation?
11. What are the corresponding C# per-update Spiker and projectile movements?
12. Are both C# objects updated once per active simulation step?
13. Does the C# Spiker move during states or phases where the projectile
    continues independently?
14. Why can C# Spiker-to-projectile separation decrease after firing?
15. Which narrow future correction best restores the arcade/MAME invariant?


ROM TRACE

Trace the actual arithmetic and call paths.

At minimum inspect:

- Level 7 spd_spiker_lsb / spd_spiker_msb loading;
- Level 7 enm_shotspd_lsb / enm_shotspd_msb loading;
- crack_speed or any equivalent speed conversion;
- difficulty adjustments;
- enemy initialization that installs Spiker speed;
- the Spiker p-code movement operation;
- along_lsb and enemy along-position updates;
- carry/high-byte behavior;
- enemy projectile creation;
- projectile movement routine;
- projectile fixed-point position state;
- projectile expiry/rim endpoint;
- gameplay-loop call frequency for enemy movement;
- gameplay-loop call frequency for projectile movement.

Do not compare only the final numeric values without proving that they use
equivalent representation and cadence.

The prior static diagnostic calculated approximately:

enemy projectile / Tanker = 0.865

That interpretation conflicts with Neal's reproduced MAME observation that an
enemy shot can start behind a Tanker, catch it, and pass it.

Treat that conflict as evidence that the earlier static ratio interpretation
may be incomplete.

Identify exactly whether the earlier calculation:

- compared unlike representations;
- omitted a sign/complement;
- omitted fixed-point accumulation;
- omitted call frequency;
- used the wrong loaded value;
- used the wrong difficulty result;
- or remains unresolved.

Do not repeat the 0.865 value as an arcade fact unless the complete trace
supports it.


MAME / ROM RELATIONSHIP

MAME is executing the arcade program, and Neal's observation is now concrete:

- a projectile originating behind a Tanker can catch and pass it;
- a Spiker projectile pulls visibly ahead of the firing Spiker.

Use these as observable reality checks on the static ROM interpretation.

Do not dismiss the local listing.

Instead identify which source interpretation produces behavior consistent with
the verified execution.

If the annotated listing remains ambiguous or internally incomplete, state that
clearly.


CURRENT C# TRACE

Inspect at minimum:

src/Tempest.Game/Gameplay/Spiker.cs

src/Tempest.Game/Gameplay/EnemyBullet.cs

src/Tempest.Game/TempestGame.cs

src/Tempest.Game/Levels/LevelConfiguration.cs

the current Level 7 Spiker firing tests

enemy-bullet and movement tests

the current uncommitted diff

Establish:

- canonical Level 7 Spiker climb/retreat speed;
- whether Spiker speed varies by phase;
- whether persistent-Spike growth affects physical Spiker movement;
- enemy-projectile height advance per update;
- order of projectile and Spiker updates in one simulation step;
- whether a newly created shot moves in its creation step or the next step;
- whether both objects update every eligible simulation step;
- whether the projectile begins at the pre-move or post-move Spiker position;
- whether collision/removal ordering affects apparent separation;
- whether rendering uses the same position state just updated by simulation.

Calculate the exact C# relative movement per comparable active update.

If C# uses, for example:

projectile advance - Spiker advance

show the supported value, but do not infer arcade timing from it.


FIRST-CYCLE VERSUS LATER-CYCLE SPEED

Determine whether the relative-speed mismatch occurs:

- on the first Spiker ascent;
- only on later ascents;
- only after the new repeated-cycle firing correction;
- or whenever a Spiker fires.

The firing correction added ongoing firing eligibility and cooldown but was not
intended to change movement or projectile speed.

Verify from the diff whether it changed:

- Spiker movement;
- update ordering;
- shot origin;
- projectile update timing;
- or only whether/when a shot is created.

If the mismatch predates the correction but was previously difficult to
observe, say so explicitly.


PHASE COMPARISON

Compare projectile speed against the Spiker in each relevant C# phase:

- Building/ascending;
- retreat transition;
- Retreating/descending, if the projectile still travels toward the rim;
- pending-supply restart.

The important manual defect is the ascending case.

Do not recommend changing retreat speed unless directly relevant.


TANKER SANITY CHECK

Neal has reproduced in MAME that an enemy shot originating behind a Tanker can
catch and pass that Tanker.

C# also shows enemy projectiles overtaking Tankers.

Use the Tanker only as a sanity check:

- trace current C# Tanker movement rate;
- compare it with the unchanged projectile rate;
- determine whether a proposed Spiker-only correction would preserve the
  already-correct qualitative projectile-vs-Tanker relationship.

Do not broaden this task into Tanker tuning.


FIRING FREQUENCY IS OUT OF SCOPE

The repeated-cycle firing correction concerns whether a Spiker remains capable
of firing.

This task concerns projectile-versus-Spiker travel speed after a projectile
exists.

Do not retune:

- firing cooldown;
- random firing threshold;
- projectile capacity;
- firing position eligibility;
- amount of firing pressure.

Do not confuse projectile creation cadence with projectile travel speed.


NO TUNING FROM VISUAL ESTIMATES

Do not use:

- 50 percent faster;
- 10 mph versus 15 mph;
- or any other rough visual estimate

as an implementation constant.

Those examples establish only that the projectile's speed advantage is readily
perceptible.

If the ROM trace cannot establish a defensible exact C# translation, recommend
the smallest bounded manual-calibration diagnostic rather than inventing a
value.


PRESERVE CURRENT LEVEL 7 CANDIDATE

Do not change or recommend reopening in this task:

- seven-place admission;
- Tanker/Spiker minima and maxima;
- pending-lane Spike selection;
- Spiker persistence;
- saturated cargo allocation;
- generic death reconstruction;
- ongoing repeated-cycle firing eligibility;
- persistent Spike growth and hammering;
- zero-pending Tanker conversion;
- Level 7 Flipper movement;
- Tanker-cargo movement;
- Super Zapper behavior.

This investigation is relative speed only.


TEST-GAP ANALYSIS

Identify existing coverage for:

- Spiker position after one update;
- enemy projectile position after one update;
- shot origin;
- shot first-movement timing;
- Spiker/projectile relative separation after firing;
- multiple-step separation;
- later-cycle firing and relative speed;
- Tanker/projectile relative speed;
- deterministic playback.

Do not modify tests.

Describe the smallest future regression that should fail on the current
candidate, such as:

1. canonical Level 7 Spiker in an eligible ascent;
2. deterministic shot creation;
3. capture Spiker and projectile positions immediately after creation;
4. advance a bounded number of comparable active steps;
5. assert projectile-to-Spiker separation increases monotonically or by the
   translated expected delta;
6. assert the Spiker never catches the shot;
7. repeat after a pending-supply restart/later ascent;
8. ensure Tanker/projectile behavior remains unchanged.

Adapt the test design to the mechanism actually established.


OUTPUT CLASSIFICATION

Conclude with one of:

A. CONFIRMED SPIKER SPEED DEFECT

The projectile path is consistent with the arcade/MAME invariant, but canonical
Level 7 Spiker movement is too fast relative to it.

Recommend one Spiker-only implementation milestone.

B. CONFIRMED PROJECTILE SPEED DEFECT

The Spiker path is supported, but projectile travel is too slow.

Recommend one projectile-speed milestone and explain how Tanker behavior will
remain correct.

C. CONFIRMED UPDATE/COORDINATE DEFECT

The nominal speed values are not the root cause; update cadence, shot origin,
fixed-point arithmetic, or coordinate handling causes the mismatch.

Recommend one narrow correction.

D. RELATIVE MISMATCH CONFIRMED, EXACT TRANSLATION UNRESOLVED

The C# mismatch is clear, but the local evidence does not establish a safe exact
value or mechanism.

Recommend the smallest bounded diagnostic/manual-calibration milestone.

E. NO SOURCE-BACKED DEFECT FOUND

Only use this if the trace genuinely fails to support a code-level mismatch.

Do not dismiss Neal's reproduced MAME/arcade observation.


RECOMMENDED NEXT STEP

Recommend exactly ONE next milestone.

Do not combine it with:

- outstanding Game Over test validation recovery;
- Tanker natural-vs-shot cargo behavior;
- Level 7 movement reconciliation;
- Super Zapper;
- broader concurrency;
- Level 8.


VALIDATION

This is READ-ONLY.

Do not:

- modify production files;
- modify tests;
- modify durable documentation;
- run broad tests/builds;
- launch the interactive game;
- stage;
- commit;
- push.

Small read-only calculations and source searches are allowed.

At completion verify:

- HEAD remains
  8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b;
- the complete current uncommitted Level 7 candidate is unchanged;
- index remains unchanged;
- git diff --check passes;
- protected local material remains untouched.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level7-spiker-projectile-relative-speed-investigation-report.md

Include:

- Neal's precise MAME/arcade/C# observation;
- ROM Spiker speed trace;
- ROM projectile speed trace;
- update cadence and fixed-point arithmetic;
- explanation of the earlier 0.865 conflict;
- current C# Spiker speed;
- current C# projectile speed;
- first/later-cycle comparison;
- Tanker sanity check;
- existing-test gap;
- future regression design;
- A/B/C/D/E conclusion;
- exactly one recommended next milestone;
- confirmation that nothing was modified.


STOP

Stop after the read-only investigation/report.

Do not implement a speed correction.
Do not investigate the Game Over test failure.
Do not begin Tanker natural-vs-shot cargo work.
Do not begin Level 8 work.
Do not commit or push.


TEMPEST_LEVEL7_SPIKER_PROJECTILE_RELATIVE_SPEED_INVESTIGATION_COMPLETE