FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: READ-ONLY HEADLESS AUDIT OF A REAL LEVEL 7 GAMEPLAY RECORDING

This is the first experiment using a real Neal-created gameplay recording as a reusable deterministic headless diagnostic dataset.

The recording to analyze is exactly:

Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json

This task is primarily READ-ONLY.

Do not implement a gameplay correction.

Do not change production behavior.

Do not change the accepted Tanker-matched Spiker speed relationship.

Do not commit or push.


STARTING CONTEXT

Read:

AI-Logs/Reports/2026-11-11-level7-spiker-tanker-speed-match-diagnostic-report.md

Treat that report as the current implementation checkpoint.

The recording was made using the development-only Level 7 Tanker-match diagnostic.

The current diagnostic relationship is:

- Level 7 Tanker climb speed: 1.75 depth units per simulation step.
- Level 7 diagnostic Spiker movement: the same existing Tanker rate, 1.75.
- Tanker and Spiker shots: the shared EnemyBullet implementation at 3.0 depth units per simulation step.
- Repeated-cycle Spiker firing remains enabled.
- The diagnostic startup state is stored in recording metadata and restored by playback.
- Presentation speed is independent of deterministic recording state.

Neal has manually accepted the resulting speed relationships as playing extremely close to the arcade game.

The accepted speed relationship is now frozen for this investigation.

Do not reopen speed calibration.


MANUAL ACCEPTANCE ALREADY ESTABLISHED

Record these as the current manual results:

1. Tanker speed = Spiker speed.
2. Tanker-shot speed = Spiker-shot speed.
3. Shots visibly travel faster than the firing Tankers and Spikers.
4. Spikers continue firing during repeated ascent cycles.
5. Overall Level 7 speeds appear effectively identical to Neal's arcade experience.
6. No further speed changes are wanted.

Do not challenge or retune these accepted relationships in this task.


END-OF-WAVE TANKER-SURGE CONTEXT

Neal previously observed a rare late-Level-7 Tanker surge in C#.

After further testing, Neal reproduced the same type of Level 7 end-of-wave Tanker surge in MAME.

It required unusual timing:

- dying near the end of the wave;
- with approximately seven or fewer enemies remaining;
- at the right moment in the remaining-enemy lifecycle.

Neal no longer considers the Tanker surge a C# discrepancy or gameplay issue.

Treat it as a potentially legitimate rare behavior.

Do not approach this audit with the assumption that the surge is a defect.

If the recording contains a Tanker surge, explain its internal C# lifecycle and admission sequence.

Do not propose a correction merely because the behavior is unusual.


PRIMARY GOAL

Determine what objectively useful behavioral evidence can be extracted by replaying Neal's real Level 7 recording headlessly.

Use the existing deterministic playback path wherever practical.

The audit should answer:

1. Does the recording load and replay deterministically from its stored startup metadata?

2. Does it restore the Tanker-match Spiker diagnostic state?

3. What major Level 7 lifecycle events occur during the recording?

4. What are the maximum simultaneous populations observed for:
   - active Tankers;
   - active Spikers;
   - active Flippers;
   - enemy projectiles;
   - total active enemies where the implementation has a meaningful definition?

5. What player deaths and reconstruction events occur?

6. What Tanker cargo-release events occur?

7. What Spiker ascent, retreat, and repeated-ascent cycles occur?

8. Do Spikers continue firing during later ascent cycles?

9. Do Spiker projectiles consistently gain separation from their firing Spikers while both are moving toward the rim?

10. If a late-wave Tanker surge appears:
    - when does it begin;
    - how many Tankers coexist;
    - what game state preceded it;
    - what obligation/admission path created each Tanker;
    - whether player death/reconstruction immediately contributed;
    - how many enemies or obligations remained;
    - whether the sequence is internally consistent with the current Level 7 rules?

11. Does the recorded session complete Level 7 normally, stop early, end on player exit, or end for another recorded reason?

12. Does headless playback reveal any objectively inconsistent state transition, conservation violation, duplicate identity/allocation, impossible population, stale entity, or other internal invariant problem?


FIRST: VERIFY RECORDING AND WORKTREE

Before running playback:

1. Run:

   git status --short --branch

2. Preserve the entire uncommitted candidate.

3. Verify that the recording exists at exactly:

   Temp\GameplayRecordings\lvl7-SpikerTankerMatch.json

4. Inspect the recording only enough to establish:
   - format version;
   - startup level;
   - starting lives;
   - stored Tanker-match diagnostic state;
   - input/tick count;
   - any other existing startup metadata relevant to deterministic playback.

5. Do not manually edit the recording.

6. Do not replace or regenerate the recording.

If the recording is missing, malformed, incompatible, or does not contain the expected Tanker-match metadata, STOP and create a BLOCKED report.

Do not substitute another recording.


HEADLESS EXECUTION

Prefer the existing headless playback infrastructure.

Do not launch the visible interactive game for this audit.

If an existing command already supports headless playback of an arbitrary recording, use it.

If a minimal temporary or test-only harness is required to feed this recording through existing deterministic playback, make the smallest possible TEST/DIAGNOSTIC-ONLY addition.

Do not build a general telemetry framework.

Do not alter the recording format.

Do not alter production gameplay.

Do not alter visible playback.

Do not change the recording's inputs or startup state.

All playback runs must be bounded by:

- the recording's finite input/tick count;
- a reasonable explicit wall-clock timeout;
- no unbounded loops.

If playback ends before all recorded inputs are consumed, record the exact reason and state.


TELEMETRY DESIGN

Collect only enough telemetry to answer the audit questions.

Do not dump a full entity snapshot for every tick unless absolutely needed.

Prefer:

1. a compact aggregate summary;
2. an event timeline;
3. targeted detailed windows around important events.

Useful event categories include:

- level start;
- enemy admission;
- Tanker creation;
- Spiker creation;
- Flipper creation;
- Tanker cargo allocation;
- Tanker cargo release;
- enemy destruction;
- enemy rim arrival;
- Spiker shot creation;
- Spiker retreat;
- Spiker re-ascent;
- repeated-cycle Spiker firing;
- player death;
- reconstruction;
- Super Zapper use if present;
- transition into a late-wave state;
- maximum-population points;
- wave completion;
- recording termination.

For each important event, include the simulation tick and enough state to explain the transition.

Where useful, include:

- active Tankers;
- active Spikers;
- active Flippers;
- enemy projectiles;
- reserve/remaining supply;
- unresolved obligations;
- latent/pending cargo;
- reconstruction state;
- current level state.

Use existing project terminology and identity/obligation concepts accurately.


SPIKER/PROJECTILE MEASUREMENT

For firing Spikers that can be tracked unambiguously:

1. identify the firing tick;
2. identify the Spiker;
3. record Spiker depth at firing;
4. record projectile depth when created;
5. sample both over a small fixed number of subsequent active simulation steps;
6. report projectile-to-Spiker separation change;
7. note when the Spiker retreats or changes state;
8. do not compare their positions after the projectile or Spiker has been removed or transformed.

It is unnecessary to trace every shot.

Select a small representative sample including, if present:

- first-ascent firing;
- repeated-ascent firing;
- a shot near a Spiker reversal.

The purpose is to verify actual recorded-session behavior, not to recreate the already-completed synthetic calibration tests.


TANKER-SURGE ANALYSIS

If the recording contains three or more simultaneous Tankers, create a focused event window around the first such occurrence.

For that window, determine:

1. the first tick with three or more Tankers;
2. the maximum simultaneous Tanker count;
3. the tick at maximum count;
4. whether the Blaster recently died;
5. remaining/reserve/unresolved enemy state;
6. active Spiker and Flipper populations;
7. how each Tanker entered the active simulation;
8. whether each Tanker came from:
   - ordinary Level 7 admission;
   - reconstruction;
   - an existing unresolved obligation;
   - conversion/reclassification;
   - another actual code path.

Do not assume Spikers automatically became Tankers.

Report conversion only if the code and playback state directly establish it.

Then classify the result:

- internally consistent rare behavior;
- internally inconsistent behavior;
- insufficient telemetry to determine.

Since Neal reproduced the surge in MAME, unusual Tanker population alone is not evidence of a C# defect.


GENERAL ANOMALY AUDIT

Report only objectively supported anomalies.

Examples include:

- an identity allocated twice;
- a terminal identity returning;
- a negative reserve;
- unresolved obligation count changing without a valid lifecycle event;
- cargo appearing without an owning allocation;
- an enemy becoming active through no recognized admission path;
- a Spiker failing to fire on a later ascent despite eligible state and ammunition;
- wave completion while unresolved obligations remain;
- playback divergence or premature termination;
- state inconsistent with recorded startup metadata.

Do not report these as anomalies merely because behavior is rare:

- several Tankers appearing late;
- death near wave completion;
- high but valid enemy pressure;
- a Spiker retreating and re-ascending;
- delayed cargo release;
- legitimate reconstruction.

Do not make arcade-fidelity claims from headless C# state alone.


DELIVERABLES

Produce a concise but useful report containing:

A. RECORDING IDENTITY

- path;
- recording format/version;
- startup level;
- starting lives;
- diagnostic startup state;
- recorded input/tick count;
- termination condition.

B. HEADLESS PLAYBACK RESULT

- whether playback completed;
- whether all recorded input was consumed;
- whether deterministic startup state was restored;
- final level/state;
- final score/lives if available;
- any playback or validation error.

C. POPULATION SUMMARY

- maximum simultaneous Tankers;
- maximum simultaneous Spikers;
- maximum simultaneous Flippers;
- maximum enemy-projectile count;
- maximum total active-enemy count if meaningful;
- ticks at which maxima occurred.

D. EVENT TIMELINE

A compact timeline of important admissions, deaths, reconstruction, repeated Spiker firing, cargo releases, surge behavior, and completion.

E. SPIKER FIRING AND RELATIVE-MOVEMENT SAMPLE

A small representative sample showing repeated-cycle firing and projectile separation.

F. TANKER-SURGE ANALYSIS

Only if the recording contains the relevant event.

G. OBJECTIVE ANOMALIES

- confirmed anomalies;
- suspected but unproven anomalies;
- or an explicit statement that none were found.

H. ASSESSMENT OF THE METHOD

State what the headless recording audit could determine reliably and what still requires visible playback or Neal's manual arcade/MAME comparison.


READ-ONLY AND CHANGE POLICY

Start read-only.

If current production/test APIs already expose enough state, do not modify tracked files.

If a minimal test-only diagnostic is required:

- keep it narrowly scoped;
- do not change production behavior;
- do not change the recording;
- do not change normal tests;
- do not commit;
- do not push;
- identify every changed file.

Do not add permanent telemetry infrastructure unless the current audit cannot be completed without it.

Prefer a one-time bounded diagnostic test over a new production feature.


VALIDATION

At minimum:

1. Run the bounded headless audit.
2. Verify deterministic startup metadata was restored.
3. Verify the run terminates within the recording bounds.
4. Run directly affected playback tests if tracked test code changes.
5. Run git diff --check if any tracked file changes.

Do not run the full solution suite unless tracked implementation changes unexpectedly make it necessary.

Do not launch interactive Tempest.


GIT SAFETY

Do not:

- stage;
- commit;
- push;
- reset;
- restore;
- clean;
- checkout over changes;
- discard candidate work.

Show final:

git status --short --branch

Make clear whether the audit changed any tracked or untracked project files.


AI LOGS

This is a substantive diagnostic investigation.

Save the complete task under:

AI-Logs/Prompts/2026-11-11-level7-recorded-headless-behavioral-audit-prompt.md

Save the report under:

AI-Logs/Reports/2026-11-11-level7-recorded-headless-behavioral-audit-report.md

AI-Logs remain ignored historical/working artifacts and must not be staged or committed.

The task is not complete until both files exist and the report has been verified.


STOP CONDITION

After the bounded headless playback audit, report, final Git status, and method assessment are complete, STOP.

Do not implement gameplay corrections.

Do not alter accepted Level 7 speeds.

Do not launch visible Tempest.

Do not proceed into Level 7 closeout.

Wait for Neal to review the headless audit results.

TEMPEST_LEVEL7_RECORDED_HEADLESS_BEHAVIORAL_AUDIT_20261111