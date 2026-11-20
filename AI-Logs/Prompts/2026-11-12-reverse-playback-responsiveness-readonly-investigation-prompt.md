FAIL-CLOSED TASK COMPLETENESS CHECK

Read this entire user-provided task before using tools, reading repository files, running commands, or taking repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's token and do not invalidate it.

This check detects an incomplete or truncated user task, not appended agent context.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.


TASK: READ-ONLY REVERSE PLAYBACK STEPPING RESPONSIVENESS INVESTIGATION

Level 9 is closed.

Begin the deferred reverse-playback responsiveness milestone.

This task is READ-ONLY / DIAGNOSTIC ONLY.

Do NOT implement a correction yet.

Do NOT change playback controls.

Do NOT change recordings.

Do NOT commit or push.


TEST RECORDING

Use exactly:

.\temp\GameplayRecordings\level9-spiker-speed-2.0-2.json

First verify that this exact recording exists.

Do not substitute another recording if it does not exist.

Do not modify, rewrite, normalize, rename, move, or regenerate this recording.


CURRENT MANUAL OBSERVATION

The existing reverse-playback functionality is mechanically correct and previously passed manual acceptance.

The current concern is ordinary held-Left responsiveness.

Neal reports:

- A single Left tap steps back exactly one frame/boundary as desired.
- Exact one-frame reverse stepping must be preserved.
- Holding ordinary Left is significantly too slow compared with holding Right.
- Forward held stepping is much faster and practically useful.
- Ordinary held reverse appears to advance backward only a fraction as quickly as desired.

Neal had temporarily forgotten that Shift+Left already requests five boundaries backward.

Neal considers the existing Shift+Left = -5 behavior an acceptable faster-reverse solution if ordinary held-Left cannot be improved through a small, clean, low-risk change.

Therefore this milestone must not assume a reverse-control redesign is required.

The goal is to determine WHY ordinary held Left is slow and whether there is a simple improvement.

Possible causes include, but are not limited to:

- replay-from-start reconstruction cost;
- repeated reconstruction allocation/setup;
- renderer or playback-session rebinding;
- audio-provider reattachment;
- native keyboard-repeat frequency;
- Raylib repeat-event behavior;
- input-loop sampling;
- the main loop being blocked during synchronous reconstruction;
- synchronization or frame pacing;
- interaction with --speed;
- another implementation bottleneck.

Do not assume the cause.


EXISTING ACCEPTED CONTROL MODEL

Preserve the existing conceptual distinction:

- Left tap = exact one-boundary reverse inspection.
- Shift+Left = five-boundary faster reverse navigation.
- Right = one-boundary forward stepping.
- Shift+Right = five-boundary forward stepping.

Shift+Left -5 is already an acceptable faster-reverse fallback.

Do not recommend replacing or degrading it merely because ordinary held Left is slow.


REQUIRED HISTORICAL CONTEXT

Read completely:

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-report.md

AI-Logs/Reports/2026-11-07-reverse-playback-scrubbing-phase3b-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-headless-playback-runner-phase3a-acceptance-closeout-report.md

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-closeout-report.md

Read the current relevant playback, input, rendering, screenshot, and test source.

The historical design is expected to include:

- reverse navigation through deterministic replay-from-start reconstruction;
- Left = one boundary backward;
- Shift+Left = five boundaries backward;
- native keyboard repeat driving held stepping;
- no application-controlled held-key polling or custom repeat timer;
- one-frame forward stepping through sequential advancement of the existing playback session;
- playback-session replacement only after successful backward reconstruction;
- historical audio suppression during reconstruction;
- renderer rebinding after successful reconstruction;
- screenshot capture of the selected paused boundary.

Verify the CURRENT implementation rather than assuming the historical description remains exact.


STARTING GIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Expected starting point:

- branch master tracking origin/master;
- local HEAD and origin/master both at the accepted Level 9 closeout commit:

  cd678572c009d27fa8e1e1f0059ec45f6db5db11

- clean tracked worktree and index;
- no nonignored untracked project paths.

If tracked/index state is not clean or HEAD/origin/master differ, STOP and report before investigation.

Do not reset, restore, clean, checkout, stage, commit, push, stash, remove, rename, or discard anything.


GOAL A: TRACE FORWARD VERSUS REVERSE CONTROL PATHS

Trace the complete current paused visible-playback paths for:

1. Right tap
2. Right held/native repeat
3. Shift+Right tap
4. Shift+Right held/native repeat
5. Left tap
6. Left held/native repeat
7. Shift+Left tap
8. Shift+Left held/native repeat

For each determine:

- input API used;
- pressed versus repeat flags;
- whether key-down polling is involved;
- whether modifier state changes behavior;
- number of playback boundaries requested per event;
- whether gameplay simulation is replayed;
- whether a new session is allocated;
- whether the renderer is rebound;
- whether audio providers are reattached;
- whether paused overlay state is updated;
- whether the main loop waits synchronously for reconstruction to finish;
- whether another repeat can be observed while reconstruction is running;
- whether rendering or presentation pacing affects event-processing opportunity.

Produce a concise comparative control-path table or diagram.


GOAL B: MEASURE RECONSTRUCTION COST

Using the specified Level 9 recording, determine its finite duration/input count.

Use existing headless/session APIs where safe.

Measure or otherwise obtain bounded timing evidence for reconstructing representative targets:

- early recording;
- approximately 25%;
- approximately 50%;
- approximately 75%;
- near the final boundary.

Also compare, where existing safe seams permit:

- fresh reconstruction from boundary zero to target N;
- ordinary sequential advancement of one boundary;
- reconstruction from N to N-1 as performed by visible reverse stepping;
- grouped reconstruction from N to N-5;
- whether a hypothetical N-to-N-10 target would cost materially more than N-to-N-1 when both reconstruct from boundary zero.

Use repeated bounded measurements only if necessary to avoid relying on one noisy sample.

Do not add permanent benchmark infrastructure.

Temporary measurement scripts or output must not modify project source or tests.

Prefer existing public APIs, existing executables, or bounded PowerShell timing.

Report:

- target boundary;
- recording percentage;
- elapsed time;
- execution route;
- whether startup/session creation is included;
- whether renderer or Raylib is involved.

The purpose is to answer:

Is replay-from-start reconstruction itself slow enough to explain Neal's ordinary held-Left experience?


GOAL C: EXAMINE KEY-REPEAT BEHAVIOR

Inspect the current Raylib, InputSystem, Program, and VisiblePlaybackController handling.

Determine exactly what event causes each held-Left reverse request.

Specifically establish:

- whether ordinary held Left depends solely on native IsKeyPressedRepeat or an equivalent repeat event;
- whether Shift+Left uses that same repeat source;
- whether Right and Left use the same native repeat source;
- whether repeat timing and frequency are controlled by Tempest;
- whether current code has separate key-down state that could support application-controlled repeat timing;
- whether the main loop can observe held state independently of native repeated press events;
- whether reconstruction blocks subsequent input polling;
- whether the apparent Right/Left difference can be explained by the different amount of work performed after each identical repeat event;
- whether Shift+Left appears faster simply because each repeat requests five boundaries rather than one.

Do not speculate about Windows keyboard-repeat timing without evidence.

If the native/external repeat frequency itself cannot be measured reliably in this read-only task, say so.

Do not change Windows keyboard settings.


GOAL D: TEST OR ANALYZE PRESENTATION-SPEED INTERACTION

The existing --speed diagnostic changes interactive presentation or host-loop pacing, not fixed simulation semantics.

Determine whether selecting:

--speed 0.5
--speed 1.0
--speed 2.0

could change perceived held reverse responsiveness because of:

- polling frequency;
- native repeat delivery;
- host-loop scheduling;
- reconstruction timing;
- rendering or frame-pacing interaction.

Do not alter accepted --speed semantics.

Do not launch the interactive game.

A bounded source-level conclusion is acceptable if visible execution would be required for direct measurement.


GOAL E: DETERMINE THE BOTTLENECK

End this portion with exactly one primary classification:

A. RECONSTRUCTION-LIMITED

B. KEY-REPEAT / INPUT-LIMITED

C. BOTH RECONSTRUCTION AND INPUT LIMITED

D. ANOTHER IDENTIFIED BOTTLENECK

E. INSUFFICIENT EVIDENCE

Support the classification with measured and source-backed evidence.

Do not force a conclusion.

If different parts dominate at early versus late recording boundaries, state that explicitly.


GOAL F: DETERMINE WHETHER ORDINARY HELD-LEFT CAN BE IMPROVED SIMPLY

The existing acceptable faster-navigation fallback is:

Shift+Left = five boundaries backward per press/repeat.

Prioritize these options in order.


OPTION 1 — SIMPLE HELD-LEFT RESPONSIVENESS IMPROVEMENT

Determine whether ordinary held Left can be made substantially faster through a small, clean change while preserving:

- Left tap = exactly -1;
- Shift+Left tap/repeat = exactly -5;
- deterministic reconstruction;
- current playback and screenshot behavior.

Examples to evaluate only if supported by the current architecture:

- application-controlled repeat timing after the initial press;
- explicit held-state polling;
- avoiding unnecessary work surrounding each reconstruction;
- another narrowly scoped input/control-path correction.

Do not implement anything in this read-only task.


OPTION 2 — KEEP THE EXISTING CONTROL MODEL

If ordinary held-Left speed is mainly limited by native/external key repeat, or improving it cleanly would add disproportionate complexity, recommend retaining:

- Left tap/repeat for precise one-frame reverse navigation;
- Shift+Left for accepted faster five-frame reverse navigation.

This is an acceptable outcome.

Do not invent another acceleration mechanism merely because ordinary held Left is slower than held Right.


OPTION 3 — RECONSTRUCTION OPTIMIZATION

Evaluate reconstruction optimization only if measurements establish that replay-from-start reconstruction materially limits responsiveness.

Possible areas to identify, without implementing:

- session construction;
- repeated startup restoration;
- repeated replay work;
- allocation;
- session replacement;
- renderer rebinding;
- audio-provider reattachment;
- another measured cost.

Do not recommend snapshots or checkpoints just because snapshots could theoretically improve performance.


OPTION 4 — SNAPSHOTS / CHECKPOINTS

Snapshots remain deferred by default.

Recommend them only if:

- measured later-boundary reconstruction latency is clearly harmful;
- ordinary held-key or Shift+Left usability cannot reasonably mask that cost;
- a smaller optimization cannot address it.

If snapshots are not justified, say so directly.


OPTION 5 — LARGER OR PROGRESSIVE REVERSE GROUPS

Do NOT recommend -10, progressive acceleration, or another larger grouping unless the investigation demonstrates a concrete usability problem that existing Shift+Left -5 cannot reasonably solve.

The default faster-navigation fallback remains the existing Shift+Left -5 behavior.


ACCEPTANCE PRIORITIES

Any later implementation must preserve:

1. Left tap steps backward exactly one boundary.

2. Exact frame-by-frame reverse inspection remains reliable.

3. Shift+Left remains exactly five boundaries backward per accepted event unless separately authorized.

4. Shift+Left -5 is an existing acceptable faster-reverse solution and must not be degraded while investigating ordinary held-Left responsiveness.

5. Right and Shift+Right behavior remain unchanged unless separately justified.

6. Space pause/resume remains unchanged.

7. Reconstruction remains deterministic.

8. Recording bytes remain unchanged.

9. No historical audio is emitted during reconstruction.

10. Visible audio resumes correctly after reverse stepping.

11. Boundary zero clamps correctly and does not wrap.

12. The final paused boundary remains rewindable.

13. -1/+1 and -5/+5 visual round trips remain deterministic.

14. The paused boundary overlay remains correct.

15. Live-input handoff behavior remains unchanged.

16. Screenshots remain bound to the selected paused boundary.

17. Gameplay simulation and recording semantics remain completely unchanged.

18. No new reverse behavior applies while normal playback is running unless explicitly authorized later.


RECORDING-SPECIFIC AUDIT

For exactly:

.\temp\GameplayRecordings\level9-spiker-speed-2.0-2.json

Report:

- whether the exact path exists;
- file format and version;
- starting level;
- starting lives and other relevant startup metadata;
- Level 9 Spiker-speed metadata/default state;
- duration/input count;
- whether validation/loading succeeds;
- whether full bounded headless playback succeeds;
- completion reason;
- final boundary/state;
- whether all recorded input is consumed;
- representative reconstruction timing evidence;
- whether the recording is suitable as the responsiveness calibration recording.

Do not modify the recording.


READ-ONLY POLICY

Do not change:

- production source;
- tests;
- durable documentation;
- README;
- recordings;
- screenshots;
- UserData;
- Videos;
- TEMP/temp scratch material;
- ROM/reference data;
- Git configuration.

Do not create a permanent diagnostic feature.

A temporary external script or disposable timing result is permitted only if necessary.

Any temporary analysis artifact must be removed before completion and must not appear in final Git status.

Do not launch the interactive game.

Do not stage, commit, or push.


VALIDATION AND SAFETY

All commands must be explicitly bounded.

If Windows Application Control or another security policy blocks a generated artifact:

- do not weaken security;
- do not bypass it;
- do not repeatedly retry the same artifact;
- do not substitute stale binaries;
- use an existing safe execution path if available;
- otherwise report BLOCKED.

Do not terminate user-owned Tempest processes merely to obtain measurements.

If existing build output is locked, prefer source analysis or a safe isolated-output route only when execution is genuinely necessary.

A failed, blocked, timed-out, canceled, or stalled command is not a pass.


REQUIRED AI LOGS

Save the complete substantive task under:

AI-Logs/Prompts/2026-11-12-reverse-playback-responsiveness-readonly-investigation-prompt.md

Create:

AI-Logs/Reports/2026-11-12-reverse-playback-responsiveness-readonly-investigation-report.md

AI-Logs remain ignored and must not be staged or committed.

The task is complete only after both records exist and the report has been read back or otherwise explicitly verified.


REQUIRED REPORT STRUCTURE

Include:

1. Starting Git state.

2. Test-recording identity and validity.

3. Current reverse-scrubbing architecture.

4. Forward versus reverse control-path comparison.

5. Current Left and Shift+Left behavior.

6. Native repeat and held-state findings.

7. Reconstruction timing methodology.

8. Early/middle/late timing measurements.

9. -1 versus -5 target-cost comparison.

10. Whether a hypothetical -10 target materially changes reconstruction cost.

11. Presentation-speed interaction.

12. Primary bottleneck classification.

13. Simple held-Left improvement evaluated.

14. Existing-control-model fallback evaluation.

15. Reconstruction optimization evaluation.

16. Snapshot/checkpoint justification or rejection.

17. Larger/progressive grouping justification or rejection.

18. Recommended next outcome.

19. Exact behaviors that must remain frozen.

20. Files or temporary artifacts changed by this investigation.

21. Confirmation all temporary artifacts were removed.

22. Final Git status.

23. Explicit confirmation no gameplay, playback, input, screenshot, or recording behavior changed.

24. Explicit confirmation no commit or push occurred.


FINAL RECOMMENDATION

Recommend exactly one outcome:


A. IMPLEMENT A SMALL HELD-LEFT RESPONSIVENESS IMPROVEMENT

Choose this only if evidence identifies a simple, low-risk improvement that preserves:

- exact Left taps;
- existing Shift+Left -5;
- deterministic reconstruction;
- all other accepted playback behavior.


B. KEEP THE EXISTING CONTROL MODEL

Choose this if:

- ordinary held Left is primarily limited by native/external repeat behavior;
- Shift+Left -5 already provides acceptable faster reverse navigation;
- or improving held Left would introduce unnecessary complexity or risk.

Under this outcome:

- Left remains precise single-frame reverse;
- Shift+Left remains accepted five-frame faster reverse.


C. INVESTIGATE RECONSTRUCTION PERFORMANCE FURTHER

Choose this only if measured reconstruction cost is genuinely the dominant bottleneck and a specific additional bounded investigation is necessary before implementation.


D. IMPLEMENT A SMALL RECONSTRUCTION OPTIMIZATION

Choose this only if evidence identifies a specific low-risk reconstruction cost that can be removed without snapshots or behavioral change.


Do not implement the recommendation in this task.


PREFERRED DECISION PRINCIPLE

Prefer the simplest outcome supported by evidence.

Do not redesign already acceptable controls merely to make ordinary held Left match held Right.

If reconstruction is already fast and native repeat/input delivery is the main limitation, either recommend a very small application-controlled held-repeat improvement or retain the existing Left/Shift+Left model.

If Shift+Left -5 already solves practical fast reverse navigation, that is a valid reason to avoid unnecessary complexity.


STOP CONDITION

After the read-only investigation, measurements, bottleneck classification, required AI logs, exact recommendation, and final Git status are complete, STOP.

Do not implement a responsiveness correction.

Do not launch interactive Tempest.

Do not stage, commit, or push.

Do not begin enemy-shot rendering.

Do not begin global screen/tube/HUD presentation work.

Do not begin whole-game difficulty work.

Do not begin cross-level Spiker remediation.

Do not begin Level 10.

TEMPEST_REVERSE_PLAYBACK_RESPONSIVENESS_READONLY_20261112