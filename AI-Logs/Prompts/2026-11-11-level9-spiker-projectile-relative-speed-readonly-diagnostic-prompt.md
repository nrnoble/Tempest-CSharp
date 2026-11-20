FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: LEVEL 9 SPIKER / PROJECTILE RELATIVE-SPEED READ-ONLY DIAGNOSTIC

This is a tightly bounded READ-ONLY diagnostic of the current uncommitted Level 9 candidate.

Do NOT correct production speeds in this task.

Do NOT implement a new speed policy.

Do NOT alter Levels 1-8.

Do NOT begin Level 10.

Preserve the complete current Level 9 implementation candidate exactly as found.


STARTING CANDIDATE

Read completely:

AI-Logs/Reports/2026-11-11-level9-bounded-production-implementation-report.md

Also read the relevant speed sections of:

AI-Logs/Reports/2026-11-11-level9-status-rom-preimplementation-investigation-report.md

Inspect additional Level 7 speed reports only where necessary to understand the previously accepted Level 7 correction and avoid repeating already-completed analysis.

Before investigation, run:

git status --short --branch
git diff --stat
git diff --name-status
git diff --cached --stat
git rev-parse HEAD
git rev-parse origin/master

Expected:

- HEAD/origin/master remain at the accepted pre-Level-9 checkpoint:
  037b3f9877f38d0182a3eba4fb8a8f0279e4f1a9
- Level 9 candidate remains uncommitted;
- index remains empty;
- the production/test changes match the bounded Level 9 implementation report.

If the candidate materially differs, STOP and report the discrepancy.

Do not reset, restore, clean, checkout over changes, stage, commit, push, or discard anything.


NEW MANUAL LEVEL 9 OBSERVATION

Neal has manually played and recorded the current Level 9 C# candidate.

Most of Level 9 plays very well on the first implementation.

A specific speed-fidelity problem remains:

MANUAL C# OBSERVATION:

Level 9 Spiker projectiles lag behind the firing Spiker.

Neal has observed cases where a Spiker reaches its reversal, begins returning toward the deep/bottom end, and its previously fired projectile still has not reached the rim.

This is not accepted behavior.


CROSS-LEVEL ARCADE FIDELITY OBSERVATION

Neal's repeated direct arcade and MAME observations establish the following project fidelity requirement:

A Spiker projectile traveling toward the rim must move faster than the Spiker that fired it.

The firing Spiker must not:

- catch its own projectile;
- overtake its own projectile;
- move toward the rim faster than its projectile;
- leave the projectile visibly lagging behind during the same ascent.

While both Spiker and projectile are traveling toward the rim, projectile-to-Spiker separation should increase rather than decrease.

Neal reports this relationship consistently throughout observed arcade gameplay.

The absolute Spiker speed may change between levels.

This requirement therefore does NOT mean that every level must use one universal Spiker speed.


DEFAULT ENGINEERING DIRECTION

Unless compelling Level-specific evidence establishes a better model, prefer a maintainable relationship in which:

- Spiker movement follows the appropriate comparable enemy movement policy for that level;
- Spiker shots use the established enemy-projectile movement policy;
- projectile speed remains visibly greater than the firing Spiker's speed.

Level 7 is an accepted example of a successful implementation relationship, but do NOT automatically copy its numerical values or code merely because it solved Level 7.

Determine whether the Level 7 implementation represents:

- a reusable general Spiker-speed policy;
- a level-specific accepted project decision;
- or a relationship whose implementation can be generalized safely while allowing future per-level Spiker speed differences.


CONFLICT WITH STATIC ROM INTERPRETATION

Do not ignore apparent ROM conflicts.

For Level 9 the prior ROM investigation established raw inputs:

- Flipper: $AF
- Tanker: converted Flipper speed
- Spiker: $C0
- enemy projectile: $C0

Those raw bytes pass through ROM speed-processing mechanisms.

They do NOT directly establish C# depth units or visible frames-per-distance ratios.

The prior investigation explicitly warned against deriving visible equality merely because Spiker and projectile share raw input $C0.

If static analysis appears to imply that the projectile is slower than, equal to, or otherwise inconsistent with the repeatedly observed arcade relationship:

1. document the exact ROM evidence;
2. document the exact calculation/interpretation;
3. identify every assumption made in translating it;
4. determine whether movement/update cadence differs between enemy and projectile paths;
5. determine whether crack_speed output interpretation differs;
6. determine whether coordinate/update domains differ;
7. determine whether display/update scheduling could explain the observed behavior;
8. classify the contradiction as unresolved if it cannot be reconciled.

Do NOT reproduce an arcade-observation contradiction merely because a static derivation appears to say so.

Do NOT claim the ROM is wrong.

The objective is to identify where the interpretation remains incomplete.


PRIMARY GOAL A: MEASURE CURRENT LEVEL 9 C# BEHAVIOR

Determine the actual current Level 9 C# rates and policies for:

- Tanker movement;
- Flipper movement;
- Spiker movement;
- Spiker speed selection;
- Tanker projectile movement;
- Spiker projectile movement;
- update cadence of each;
- firing/update ordering relevant to first projectile movement.

Explicitly determine whether Level 9 Spiker speed:

- is fixed;
- randomized;
- inherited from a shared formula;
- level-derived;
- progression-derived;
- or otherwise selected.

If randomized, report the complete possible production range and the source formula.

Do not rely on comments alone.

Trace the actual production creation/update path.


PRIMARY GOAL B: DETERMINISTIC RELATIVE-SPEED MATRIX

Using existing movement methods and the smallest read-only/test-only mechanism available, calculate the C# projectile/Spiker relationship across every materially distinct Level 9 Spiker speed.

At minimum report:

Spiker speed
Projectile speed
Spiker travel over fixed active steps
Projectile travel over the same active steps
Separation change
Projectile/Spiker travel ratio
Result:
- projectile pulls ahead;
- equal;
- Spiker gains.

If Level 9 uses a continuous/random range, measure:

- minimum;
- boundary where Spiker equals projectile;
- representative midpoint(s);
- maximum.

Do not hide raw measurements behind only ratios.

Clearly define direction/sign conventions.

Do not claim these C# measurements are arcade timing measurements.


PRIMARY GOAL C: COMPARE LEVEL 7 AND LEVEL 9 IMPLEMENTATIONS

Inspect accepted Level 7 and current-candidate Level 9.

Determine precisely:

1. how Level 7 Spiker speed is selected now;
2. why Level 7 cannot outrun its projectile under accepted production behavior;
3. how Level 9 differs;
4. whether Level 9 accidentally reused an older pre-Level-7 speed-selection formula;
5. whether the difference is intentional and evidence-backed;
6. whether a maintainable common seam could express:
   "Spiker speed may vary by level, but projectile must outrun the firing Spiker."

Do NOT modify either level.


PRIMARY GOAL D: COMPARABLE ENEMY RELATIONSHIPS

Measure current Level 9:

- Tanker movement;
- Flipper along movement where comparable;
- Spiker movement;
- Tanker shot movement;
- Spiker shot movement.

Determine whether Tanker and Spiker shots already use the same EnemyBullet path and rate.

Determine whether Tanker and Spiker movement should be compared directly in the current C# candidate.

Separate:

C# MEASURED

ROM-LISTING-BACKED

ARCADE/MAME MANUAL OBSERVATION

PROJECT TEST HYPOTHESIS

Do not infer arcade equality merely from a shared C# value.


PRIMARY GOAL E: USE NEAL'S LEVEL 9 RECORDING WHERE USEFUL

The expected recording path is:

Temp\GameplayRecordings\level9-manual-acceptance.json

First verify whether that exact file exists.

Do not substitute another recording without explicit authorization.

If present:

1. inspect startup metadata and finite input count;
2. use the existing headless playback path if it provides useful evidence;
3. verify the full recording replays successfully within a bounded wall-clock timeout;
4. record useful endpoint information.

If existing headless CLI output exposes enough information to verify Spiker/projectile relative movement, use it.

If it does NOT expose enough per-step telemetry, say so.

Do NOT repeat the prior failed strategy of creating a temporary compiled xUnit telemetry harness merely to obtain per-tick playback detail.

Windows Application Control previously blocked such a generated test assembly with 0x800711C7.

Do not weaken security or retry an equivalent blocked-artifact strategy.

The deterministic synthetic movement diagnostic is sufficient for exact C# relative-speed calculation if playback telemetry is unavailable.


PRIMARY GOAL F: INVESTIGATE THE ROM/ARCADE CONTRADICTION

Perform a narrowly scoped read-only trace of the local ROM source sufficient to understand:

- Level 9 Spiker speed setup;
- Level 9 projectile speed setup;
- crack_speed processing for each;
- resulting movement fields;
- Spiker along-position update;
- enemy-shot along-position update;
- when each update runs;
- how often each executes;
- any fixed-point carry/fraction behavior;
- whether identical raw inputs necessarily produce identical effective displacement per equivalent gameplay interval.

Do not broaden into a complete ROM timing reconstruction unless necessary.

The desired outcome is one of:

A. RECONCILED:
The ROM actually supports projectile faster-than-Spiker runtime motion, and the earlier static interpretation missed a specific mechanism.

B. PARTIALLY RECONCILED:
A plausible source-backed mechanism is identified but a complete quantitative mapping remains unavailable.

C. UNRESOLVED:
The listing/static calculation still appears inconsistent with repeated arcade observation, and the missing translation/timing factor cannot yet be established.

An UNRESOLVED result is acceptable.

Do not force reconciliation.


PRIMARY GOAL G: RECOMMEND A DURABLE FUTURE SPEED POLICY

Do not implement it.

Based on the evidence, recommend the smallest maintainable project policy that prevents this issue from recurring on every future level.

Evaluate at least these possible policy shapes:

POLICY 1:
Each level independently chooses Spiker speed, but validation requires:
Spiker speed < projectile speed.

POLICY 2:
Spiker movement derives from that level's comparable enemy movement authority where arcade observation supports it; projectiles use the shared enemy-shot authority.

POLICY 3:
A common Spiker movement authority exists with optional documented per-level overrides, while a regression invariant guarantees shots always outrun their firing Spiker.

POLICY 4:
Another evidence-backed model discovered during investigation.

Discuss advantages and regression risks.

The recommendation must preserve the ability for higher levels to use faster Spikers.

Do not cap all Spikers permanently at one universal speed unless compelling evidence supports that.


CROSS-LEVEL REGRESSION INVARIANT

Evaluate whether a durable automated invariant can safely assert:

For every implemented level containing firing Spikers, every production Spiker movement rate must be strictly less than the corresponding production Spiker projectile movement rate over equivalent active simulation steps.

If production speeds are randomized:

- test the maximum possible Spiker rate against projectile rate.

If update cadence differs:

- compare effective displacement over a common simulation interval rather than comparing raw numeric constants.

If a future level legitimately violates the invariant according to compelling arcade evidence:

- tests should require an explicit documented exception rather than silently allowing it.

Do NOT add this test in the read-only diagnostic unless a temporary non-production calculation can be performed without tracked changes.

Recommend its eventual form.


RECORDING OBSERVATION

If the Level 9 recording is present and successfully replays, report:

- recording frame/input count;
- startup Level;
- final state;
- whether all inputs were consumed;
- whether any useful speed evidence was available from existing output.

Do not claim per-tick results if the existing CLI does not expose them.


OUT OF SCOPE

Do NOT:

- change Level 9 speed;
- change projectile speed;
- change Level 7;
- change global enemy movement;
- change global projectile movement;
- add permanent telemetry;
- change firing cadence;
- change $53 behavior;
- change Tanker cargo;
- change concurrency;
- change reconstruction;
- change geometry;
- change starting Spikes;
- change progression;
- modify Levels 1-8;
- implement Level 10;
- stage;
- commit;
- push.


READ-ONLY / TEMPORARY ANALYSIS POLICY

Prefer source inspection and existing tests.

If exact deterministic calculation can be obtained with an existing test seam, use it.

Do not leave tracked source/test changes.

Do not create a permanent diagnostic feature.

Do not create a temporary compiled telemetry harness that risks repeating the known Application Control block.

A simple bounded existing test invocation or read-only calculation is acceptable.

Any temporary non-project analysis file must be removed before completion and must not affect final Git status.


VALIDATION

This is diagnostic-only.

At minimum:

1. verify starting candidate state;
2. perform exact current-C# speed-path analysis;
3. perform deterministic relative-speed calculation;
4. inspect the Level 7 comparison;
5. perform bounded ROM trace;
6. replay Neal's recording headlessly if present and useful;
7. verify final Git status matches starting candidate state;
8. run git diff --check only if investigation unexpectedly changes a tracked file, then restore only the investigation's own accidental change without disturbing the candidate.

No full test suite is required unless a specific diagnostic need justifies it.

Do not launch interactive Tempest.


REQUIRED REPORT

Save the complete substantive prompt under:

AI-Logs/Prompts/2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-prompt.md

Create the diagnostic report under:

AI-Logs/Reports/2026-11-11-level9-spiker-projectile-relative-speed-readonly-diagnostic-report.md

AI-Logs remain ignored and must not be staged or committed.

The task is not complete until both exist and the report has been read back or otherwise verified.


REPORT STRUCTURE

Include:

1. Starting Git/candidate state.
2. Manual Level 9 observation.
3. Cross-level arcade/MAME fidelity mandate.
4. Files and evidence inspected.
5. Current Level 9 Spiker speed-selection path.
6. Current projectile movement path.
7. Current Level 9 Tanker/Flipper speed paths.
8. Deterministic relative-speed matrix.
9. Exact conditions under which Level 9 Spiker outruns/equalizes/lags its projectile.
10. Comparison with accepted Level 7.
11. Recording identity and bounded headless-playback result, if available.
12. ROM Level 9 Spiker speed trace.
13. ROM projectile speed trace.
14. crack_speed/update-cadence findings.
15. Reconciliation outcome:
    RECONCILED / PARTIALLY RECONCILED / UNRESOLVED.
16. Evidence classification.
17. Recommended durable cross-level Spiker/projectile speed policy.
18. Recommended automated regression invariant.
19. Smallest proposed Level 9 correction, WITHOUT IMPLEMENTING IT.
20. Risks to Levels 1-8.
21. Exact next implementation milestone if Neal approves.
22. Files changed by this investigation.
23. Final Git status.
24. Explicit confirmation no production behavior changed.
25. Explicit confirmation no commit/push occurred.


IMPORTANT DECISION RULE

Do not recommend reproducing a slower-than-Spiker projectile merely because an incomplete static ROM interpretation appears to imply it.

Conversely, do not suppress ROM evidence that conflicts with Neal's arcade observation.

Report both.

The project fidelity target is the repeatedly observed arcade behavior unless stronger evidence explains that the observation is being interpreted incorrectly.

Any contradiction remains explicit until reconciled.


STOP CONDITION

After the bounded read-only diagnostic, recording check where useful, ROM/C# reconciliation attempt, durable-policy recommendation, required AI logs, and final Git status are complete, STOP.

Do not implement the Level 9 speed correction.

Do not stage, commit, or push.

Do not begin Level 10.

Wait for Neal to review the evidence and explicitly authorize the implementation milestone.

TEMPEST_LEVEL9_SPIKER_PROJECTILE_RELATIVE_SPEED_READONLY_20261111