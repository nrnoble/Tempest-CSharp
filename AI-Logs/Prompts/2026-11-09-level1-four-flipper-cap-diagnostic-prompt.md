FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any
project or repository action and explain why.


TASK

Implement a tightly bounded, development-only Level 1 diagnostic that applies
the Atari ROM's maximum of four concurrent active Flippers.

The purpose is to test whether excessive concurrent enemy density is the
primary reason the accepted C# game's lower levels feel harder than Neal's
original Atari arcade machine.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

c1a974a625eea2dfaecf30d1b8d729e022ef17e8
Close out accepted spinner sensitivity diagnostic

Confirm the actual branch, HEAD, remote tracking state, index, worktree, and
untracked state before modifying anything.

Do not stage, commit, or push.

Leave the completed diagnostic candidate unstaged for Neal's manual A/B
comparison.

Do not begin Level 9.


PRIMARY INVESTIGATION REPORT

Read this report fully before designing or modifying code:

AI-Logs/Reports/2026-11-09-low-level-difficulty-comparative-investigation-report.md

Reuse its established findings.

Do not repeat the broad difficulty investigation.

Investigate only implementation details necessary to build the recommended
four-concurrent-Flipper Level 1 diagnostic safely.


EXPERIMENTAL HYPOTHESIS

The strongest current explanation for excessive low-level C# difficulty is:

- accepted production Level 1 can eventually activate all 10 original
  Flippers;
- accepted production Level 2 can eventually activate all 12 original
  Flippers;
- the Rev 2 ROM limits Levels 1 and 2 to four concurrent Flippers;
- the ROM also has a six-total-active ordinary creation gate;
- the current C# Egg/release model continues feeding active enemies beyond
  those ROM limits;
- the accepted 2.5 rim-speed calibration may amplify the resulting crowding
  once several Flippers approach or reach the rim.

This milestone must isolate only the Level 1 four-Flipper concurrency limit.

Do not change production Level 1.

Do not implement a global difficulty correction.

Do not apply the diagnostic to Level 2 or later levels yet.


USER OBSERVATION

Neal is an experienced original Atari Tempest player and owns an original
arcade machine.

Neal currently observes that the accepted C# game feels materially harder than
the arcade machine on the lower levels.

The cabinet is believed to use Medium difficulty, but that setting is not yet
hardware-verified.

The accepted spinner-sensitivity option showed that 0.8 improved physical
control, but spinner response did not explain the broader survival-pressure
difference.

Default spinner sensitivity remains 1.0.

For this controlled experiment, all comparison runs must use the same spinner
sensitivity.

Use 1.0 as the recommended comparison setting unless Neal explicitly chooses
another value for both arms.


ROM-BACKED LIMIT

The local Rev 2 ROM evidence establishes for Level 1:

- base original supply: 10;
- ordinary species: Flippers;
- maximum concurrent Flippers: 4;
- maximum total active ordinary enemies: 6.

Because Level 1 is Flipper-only, the four-Flipper species maximum is the
effective creation limit for this experiment.

The cap value 4 is ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED.

The C# mechanism used to make ready Eggs wait is an experimental project
translation.

Do not claim that waiting C# Eggs reproduce the ROM pending-entry mechanism
literally.


DIAGNOSTIC NAME

Implement one explicit development-only command-line option.

Preferred name:

--level1-four-flipper-cap-test

Use this exact name unless the existing command-line architecture has a strong,
documented naming constraint that makes a closely related name materially
clearer.

Do not use a vague production-sounding option such as:

--easy
--difficulty
--enemy-limit
--flipper-cap

The option must clearly communicate that it is:

- Level 1 only;
- a four-Flipper-cap experiment;
- development-only;
- not a production difficulty mode.


DIAGNOSTIC STARTUP

The diagnostic must start directly in the accepted production Level 1
configuration.

It should use:

- the accepted Level 1 geometry;
- displayed Level 1 identity;
- normal Level 1 supply of 10;
- normal four starting Eggs;
- normal reserve quantity;
- normal Level 1 movement;
- normal Level 1 firing;
- normal scoring;
- normal Super Zapper behavior;
- normal death/re-entry architecture;
- normal recording and playback systems where compatible;
- the normal accepted 2.5 rim-speed calibration;
- normal spinner sensitivity behavior.

The only experimental gameplay difference must be:

No more than four active Level 1 Flippers may coexist.

Do not create a simplified synthetic wave if the normal production Level 1
configuration can be reused safely.


PRODUCTION DEFAULT MUST REMAIN UNCHANGED

Without the diagnostic option:

- production Level 1 must behave exactly as it does at baseline;
- all 10 Flippers may become active under the accepted current scheduling;
- normal campaign progression remains unchanged;
- Level 2 and later levels remain unchanged.

Do not silently make the four-Flipper cap the production default.

Do not alter LevelConfiguration.Level1 globally unless the cap is represented
as an explicit diagnostic-only override that is provably inactive otherwise.


CAP SEMANTICS

For the diagnostic:

- Count currently active Level 1 Flipper enemy instances.
- Do not allow another ordinary Flipper Egg to hatch while four Flippers are
  active.
- A ready-to-hatch fifth Flipper must wait.
- The waiting Egg remains unresolved.
- The waiting Egg is not killed.
- The waiting Egg is not rim-cleared.
- The waiting Egg is not returned to reserve merely because the cap is full.
- The waiting Egg does not consume a fifth active Flipper slot.
- Once the active Flipper count falls below four, an eligible waiting Egg may
  hatch through the existing normal hatch path.
- Supply/accounting must remain conserved.
- The diagnostic must still resolve all 10 original Level 1 obligations
  eventually.

Do not discard or duplicate an Egg or Flipper identity.

Do not decrement remaining-enemy accounting until the normal lifecycle says to
do so.

Do not manufacture replacement enemies.


WHAT COUNTS AS ACTIVE

Use the project's existing authoritative active enemy collection/state.

Do not count:

- reserve supply;
- roaming Eggs;
- approaching Eggs;
- ready Eggs;
- player shots;
- enemy bullets;
- destroyed/zap-resolving entities after the existing lifecycle no longer
  treats them as active;
- presentation-only objects.

Document precisely when a Flipper begins and stops counting toward the cap.

Prefer existing lifecycle semantics over a new parallel counter.

Avoid a cached count that can drift from the actual active-enemy collection.


EGG WAITING BEHAVIOR

Use the smallest maintainable waiting behavior compatible with the existing Egg
state machine.

A cap-blocked ready Egg should remain in a valid existing state whenever
possible.

Do not add a large new queue architecture.

Do not repeatedly reroll lane, direction, identity, type, firing state, or
randomness while waiting.

Do not consume new randomness every tick while blocked.

Once a slot becomes available, hatch through the ordinary conversion path.

Preserve deterministic behavior.


RELEASE SCHEDULE MUST REMAIN UNCHANGED

Do not alter:

- four starting Eggs;
- 50-tick reserve release interval;
- approach-selection cadence;
- maximum approaching-Egg policy;
- Egg movement cadence;
- reserve release quantity;
- deterministic Level 1 lane/direction sequences;
- total supply.

This experiment isolates active Flipper concurrency.

Eggs may continue to be released and approach according to accepted behavior,
but ready Eggs must wait when the four-Flipper cap is full.

Do not stop reserve release merely to keep total visible Eggs plus Flippers at
four.

Do not implement the ROM six-total-active gate in this milestone.


NO SIX-TOTAL-ACTIVE GATE YET

Although the ROM also has a six-total-active ordinary creation gate, do not
implement that separate rule now.

This Level 1 experiment isolates the stricter four-Flipper species maximum.

Do not combine:

- four-Flipper cap;
- six-total enemy cap;
- ROM pending delays;
- revised Egg timing.

If the four-Flipper experiment improves fidelity, later work can consider
whether and how to implement a broader ROM-shaped creation policy.


NO OTHER GAMEPLAY CHANGE

Do not change:

- Level 1 total supply;
- Level 1 geometry;
- Level 1 player start;
- Egg release timing;
- Egg approach timing;
- Flipper climb speed;
- Flipper route;
- rim arrival behavior;
- rim targeting;
- rim pause;
- accepted 2.5 rim-speed calibration;
- enemy firing eligibility;
- enemy-shot capacity;
- enemy-shot holdoff;
- enemy-shot speed;
- player firing;
- player-shot slots;
- player-shot cooldown;
- player-shot lifetime;
- collision ordering;
- collision tolerance;
- scoring;
- lives;
- Super Zapper;
- death presentation;
- recording schema;
- playback behavior;
- `--speed`;
- `--spinner-sensitivity`;
- Levels 2-8.

This is a single-variable experiment.


DEATH / RE-ENTRY

The diagnostic must remain valid across player death.

After death and reconstruction:

- unresolved quantity remains conserved;
- the diagnostic cap remains enabled;
- no more than four active Flippers may coexist;
- waiting Eggs remain valid or are reconstructed through the existing accepted
  Level 1 lifecycle, depending on the current architecture;
- no duplicate or missing original obligation appears;
- enemy bullets and shots follow the current accepted death policy;
- score/lives/Super Zapper state remain normal.

Do not redesign Level 1 reconstruction.

Add focused tests that prove repeated death/re-entry cannot violate the cap or
lose wave obligations.


SUPER ZAPPER

Preserve accepted Super Zapper behavior.

A Super Zapper may reduce active Flippers below four.

After normal zap resolution:

- waiting Eggs may hatch when a slot is genuinely available;
- unresolved supply remains conserved;
- no burst may create more than four active Flippers;
- delayed zap lifecycle remains unchanged.

Do not make Eggs or reserve supply new zap targets.

Do not change Super Zapper charge behavior.


COMPLETION

The diagnostic must remain completable.

All 10 original Level 1 obligations must eventually resolve through normal
destruction or rim-clear paths.

The cap must not cause:

- a permanent ready-Egg deadlock;
- an unreleasable reserve;
- a false early completion;
- an infinite unresolved count;
- a terminal state with ready Eggs waiting despite free active slots.

For direct diagnostic startup, follow existing project convention for whether
the scenario is terminal/replayable or progresses naturally.

Prefer a terminal development diagnostic if that avoids contaminating normal
campaign progression.

Document the chosen behavior clearly.


DIRECT DIAGNOSTIC VERSUS PRODUCTION CAMPAIGN

Preferred policy:

- `--level1-four-flipper-cap-test` starts the accepted production Level 1
  configuration with only the diagnostic cap enabled;
- successful completion reaches the existing development terminal/replay
  boundary rather than progressing into production Level 2;
- normal no-option campaign progression remains unchanged.

If the existing generic direct-level architecture provides a safer reuse path,
use it while preserving the same principle.

Do not enable the cap in a normal campaign.


COMMAND-LINE COMPOSITION

Allow sensible orthogonal options:

- `--lives`;
- `--speed`;
- `--spinner-sensitivity`;
- live `--record-gameplay` if current CLI architecture supports recording a
  development scenario.

Reject incompatible scenario selectors according to existing strict CLI policy.

The diagnostic must conflict with:

- other direct-level/development wave selectors;
- visible playback startup selectors that already restore their own recorded
  scenario;
- headless options where the current CLI requires a recorded startup identity;
- input-diagnostic mode.

Do not invent inconsistent conflict behavior.


RECORDING AND PLAYBACK

Determine whether the existing recording startup identity can represent the new
diagnostic safely.

Preferred behavior:

- a recording made in the diagnostic preserves enough startup identity to
  replay the diagnostic with the cap enabled;
- visible/headless playback reproduces the capped run deterministically;
- playback does not require the user to repeat the diagnostic command;
- historic recordings remain compatible;
- no production recording is silently replayed with the cap.

If the existing recording schema cannot identify the scenario without a schema
change, do not casually broaden the recording format.

In that case, either:

- make recording incompatible with this diagnostic and explain why; or
- implement the smallest backward-compatible generic scenario identity only if
  the architecture already supports it naturally.

Do not change schema version unless genuinely necessary.

The core diagnostic does not depend on recording support if that would expand
scope excessively.


OPTIONAL COMPARATIVE MEASUREMENT

If it can be implemented without production complexity, add a development-only
headless/test seam that exposes boundary counts needed for automated comparison:

- active Flippers;
- Eggs by lifecycle state;
- reserve;
- unresolved total;
- enemy bullets;
- rim Flippers;
- player contact/death events.

Do not add persistent gameplay telemetry, a HUD overlay, file logging, or a
general analytics framework.

Prefer tests inspecting existing game state.

The visible manual comparison remains the primary fidelity judgment.


REQUIRED AUTOMATED TESTS

Add focused tests proving all of the following.


1. DEFAULT UNCHANGED

- normal production Level 1 has no new cap;
- accepted baseline scheduling remains unchanged;
- normal production Level 1 can exceed four active Flippers under the existing
  deterministic no-kill schedule;
- normal Level 1-to-2 progression remains unchanged.


2. DIAGNOSTIC STARTUP

- the command selects accepted production Level 1 geometry/configuration;
- displayed level is 1;
- original supply is 10;
- four starting Eggs remain;
- only the diagnostic cap differs;
- the scenario is correctly identified as development-only.


3. FOUR-FLIPPER CAP

- diagnostic active Flippers never exceed four;
- a fifth ready Egg waits;
- the waiting Egg remains unresolved;
- the waiting Egg does not disappear;
- the waiting Egg does not reroll every tick;
- when one Flipper resolves, an eligible waiting Egg can hatch;
- active count never transiently becomes five during the hatch update.


4. ACCOUNTING

At every meaningful boundary verify conservation among:

- reserve originals;
- Eggs;
- active Flippers;
- destroyed outcomes;
- rim-cleared outcomes;
- other existing terminal/resolution states.

All 10 original obligations must be represented exactly once.

No duplication or loss.


5. DETERMINISM

- the same seed/input produces the same hatch/wait sequence;
- blocking at the cap does not consume repeated randomness;
- release and lane/direction sequences remain deterministic.


6. RELEASE TIMING PRESERVATION

- four starting Eggs remain;
- reserve release remains on the accepted 50-tick policy;
- approach selection remains unchanged;
- the cap affects hatch eligibility only;
- no new six-total-active gate exists.


7. MOVEMENT AND FIRING PRESERVATION

- Flipper movement remains unchanged;
- below-rim climb remains unchanged;
- rim speed and pause remain unchanged;
- accepted 2.5 rim calibration remains unchanged;
- enemy-shot capacity remains unchanged;
- Level 1 holdoff/probability remains unchanged;
- player firing and shots remain unchanged.


8. DEATH / RE-ENTRY

- post-death diagnostic reconstruction conserves all unresolved obligations;
- active Flippers remain capped at four;
- repeated deaths do not duplicate or lose units;
- completion remains reachable.


9. SUPER ZAPPER

- accepted zap behavior remains unchanged;
- zap resolution can free active slots;
- waiting Eggs hatch normally afterward;
- no post-zap update exceeds four active Flippers.


10. COMPLETION

- all 10 originals can resolve;
- no false early completion occurs;
- no ready-Egg deadlock remains after active count drops;
- terminal/replay behavior matches the chosen diagnostic policy.


11. LEVELS 2-8 REGRESSION

- no Level 2-8 configuration or behavior receives the cap;
- Level 8 open topology and spinner direction remain unchanged;
- campaign progression remains unchanged.


12. CLI

Test:

- option alone;
- option plus `--lives`;
- option plus `--speed`;
- option plus `--spinner-sensitivity`;
- duplicates;
- incompatible scenario selectors;
- input-diagnostic conflict;
- playback/headless policy;
- live-recording policy if supported.

Use existing strict parser conventions.


COMPARISON TEST

Add a bounded deterministic test or analysis fixture comparing baseline Level 1
and the diagnostic under identical C# seed/input conditions.

Do not call this an arcade test.

At minimum compare:

- maximum active Flippers;
- tick at which the fifth Flipper would activate in baseline;
- waiting-Egg state in diagnostic;
- unresolved conservation;
- active/rim Flipper counts at selected boundaries;
- enemy-shot capacity remains identical;
- movement and rim settings remain identical.

The test should prove isolation of the cap, not assert a preferred difficulty or
survival result.


VALIDATION

During implementation:

1. Run narrow CLI and cap tests.

2. Run focused Level 1 Egg/Flipper/accounting tests.

3. Run focused death/re-entry/Super Zapper/completion tests.

4. Run focused recording/playback tests if the diagnostic supports recording.

5. Run Levels 1-8 movement/topology/difficulty regressions.

6. Run expanded coverage including:
   - Level configuration;
   - Level loader;
   - Egg release/approach/hatching;
   - Flipper lifecycle;
   - enemy firing;
   - rim pursuit;
   - player shots;
   - collisions;
   - death/re-entry;
   - Super Zapper;
   - completion;
   - progression;
   - campaign soak;
   - recording/playback;
   - pause/resume;
   - forward/reverse stepping;
   - screenshots;
   - `--speed`;
   - `--spinner-sensitivity`.

7. Run the full suite:

dotnet test TempestCSharp.slnx

8. Run the full build:

dotnet build TempestCSharp.slnx

9. Run:

git diff --check

Do not launch the interactive game.

Neal will perform the manual A/B comparison.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated assemblies.

Do not weaken Windows security.

If a command is blocked:

- record the exact command/error;
- distinguish a security block from a test failure;
- retry only when useful;
- use `--no-build` only when it genuinely tests the current built candidate;
- do not claim a blocked run passed.


DOCUMENTATION

Update:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Document:

- development-only diagnostic name;
- Level 1 only;
- production default remains unchanged;
- cap of four is ROM-backed;
- waiting C# Eggs are an experimental project translation;
- total supply remains 10;
- accepted Egg scheduling intentionally remains unchanged for isolation;
- six-total-active gate is not implemented;
- rim speed, firing, shots, collisions, and movement remain unchanged;
- purpose is comparative difficulty calibration;
- manual acceptance pending;
- no conclusion yet that the cap belongs in production.


EVIDENCE CLASSIFICATION

Use:

ARCADE OBSERVED

CABINET SETTING BELIEVED MEDIUM / UNVERIFIED

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

CURRENT ACCEPTED C# BEHAVIOR

DEVELOPMENT-ONLY CONTROLLED EXPERIMENT

PROJECT TRANSLATION

AUTOMATED TEST EVIDENCE

MANUAL ACCEPTANCE PENDING

Do not call the diagnostic arcade-accurate before Neal compares it.


PROTECTED FILES

Do not modify, stage, or commit:

Reference/arcade/roms.rev2/
UserData/
screenshots
recordings
Videos/
historical AI reports
unrelated local files

Do not run git clean.


TEMP DIRECTORY POLICY

TEMP/ is disposable local working material.

Do not read files under TEMP/ unless a particular file is explicitly required
for this task.

Do not use TEMP/myNotes.md as authoritative evidence for this experiment.

If temporary analysis output is needed, keep it minimal and remove it before
final status.

Do not place milestone reports in TEMP/ or Temp/.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW implementation report under:

AI-Logs/Reports/

3. Existing reports remain historical records and must not be rewritten.

4. AI-Logs is Git-ignored.

5. Do not stage or commit AI-Logs.

6. Do not place the implementation report under TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Comparative investigation reused.
4. Hypothesis being isolated.
5. Diagnostic option and CLI policy.
6. Production/default preservation.
7. Exact active-Flipper counting rule.
8. Egg waiting mechanism.
9. Randomness preservation.
10. Supply/accounting conservation.
11. Release timing preservation.
12. Death/re-entry behavior.
13. Super Zapper behavior.
14. Completion behavior.
15. Recording/playback policy.
16. Files changed.
17. Tests added/updated.
18. Deterministic baseline/diagnostic comparison.
19. Focused validation.
20. Expanded validation.
21. Full-suite result.
22. Build result.
23. `git diff --check`.
24. Levels 2-8 regression evidence.
25. Protected local files unchanged.
26. Final Git state.
27. Manual commands.
28. Manual A/B checklist.
29. Known limitations.
30. Manual acceptance status.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the completed experiment unstaged for Neal's manual comparison.

Do not begin Level 9.

Do not implement broader ROM active limits.


MANUAL COMMANDS

Provide exact commands based on the implemented CLI.

At minimum provide:


Accepted production Level 1 baseline:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1


Four-Flipper-cap diagnostic:

dotnet run --project src\Tempest.Game -- --level1-four-flipper-cap-test --lives 10 --spinner-sensitivity 1


If normal startup is the accepted baseline command instead of `--level 1`, also
provide:

dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1


If diagnostic recording is supported, provide commands to:

- record a diagnostic run;
- replay it without repeating live-only options.

Do not provide unsupported commands.


MANUAL A/B CHECKLIST

Ask Neal to compare production Level 1 and the diagnostic using the same:

- physical spinner;
- spinner-sensitivity value 1.0;
- lives setting;
- movement/fire strategy;
- general Super Zapper strategy;
- display/window setup.

Observe:

1. Maximum visibly active Flippers.
2. Whether a fifth Flipper appears while four remain active.
3. Egg/hatch spacing.
4. Number of simultaneous climbers.
5. Number reaching the rim close together.
6. Number of simultaneous rim pursuers.
7. Enemy-bullet overlap.
8. Whether the player has meaningful recovery time.
9. How often threats converge from multiple directions.
10. Whether target selection feels manageable.
11. Whether stationary defense becomes too easy again.
12. Whether active pressure now resembles the original arcade machine.
13. Whether the cap feels too restrictive.
14. Whether scoring/wave length feels abnormal.
15. Whether queued enemies continue to appear as slots open.
16. Whether the wave completes normally.
17. Whether death/re-entry remains coherent.
18. Whether Super Zapper use leaves sensible follow-up pressure.

Do not judge only by number of deaths.

The most important comparison is the timing and density of simultaneous
threats.


HYPOTHESIS OUTCOMES

The manual comparison supports the hypothesis if:

- the diagnostic never exceeds four active Flippers;
- queued enemies continue later rather than disappearing;
- combat gains arcade-like spacing and recovery time;
- excessive multi-direction rim crowding is materially reduced;
- the game remains threatening rather than becoming trivially easy.

The comparison weakens or refutes the hypothesis if:

- Level 1 still feels materially harder with the cap;
- the diagnostic feels too sparse compared with the arcade;
- arcade observation shows more than four active Flippers;
- the primary difficulty remains rim cadence, collision, or another mechanism.

Do not promote the cap into production during this milestone.


MANUAL ACCEPTANCE STATUS

The diagnostic remains MANUAL ACCEPTANCE PENDING.

Do not claim that production Level 1 should change.

Stop after automated validation and manual-test handoff.


STOP

After automated validation and manual-test handoff:

STOP.

Do not launch the game.

Do not stage.

Do not commit.

Do not push.

Do not implement Level 2 caps.

Do not implement the six-total-active gate.

Do not alter rim speed.

Do not begin Level 9.

TEMPEST_LEVEL1_FOUR_FLIPPER_CAP_DIAGNOSTIC_20261109