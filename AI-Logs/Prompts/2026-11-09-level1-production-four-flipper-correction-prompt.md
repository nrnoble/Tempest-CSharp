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

Promote the manually validated Level 1 maximum-four-concurrent-Flippers rule
from the current development-only diagnostic into normal production Level 1.

Repository:

C:\Users\nrnob\source\repos\Tempest

Committed baseline beneath the current unstaged candidate:

c1a974a625eea2dfaecf30d1b8d729e022ef17e8
Close out accepted spinner sensitivity diagnostic

The working tree currently contains the unstaged Level 1 four-Flipper-cap
diagnostic and its enemy-bullet visibility correction.

Preserve those correct candidate changes while converting the accepted
experimental rule into production Level 1 behavior.

Do not discard, restore, or restart the candidate from the committed baseline.

Do not stage, commit, or push.

Leave the complete production candidate unstaged for Neal's final production
Level 1 manual acceptance.

Do not begin Level 2 concurrency work.

Do not begin Level 9.


REQUIRED REPORTS

Read:

AI-Logs/Reports/2026-11-09-low-level-difficulty-comparative-investigation-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-cap-diagnostic-report.md

AI-Logs/Reports/2026-11-09-level1-cap-enemy-bullet-visibility-fix-report.md

Also inspect:

- the complete current tracked and untracked candidate;
- current Level 1 configuration and lifecycle;
- Egg release, approach, readiness, and hatching;
- Flipper lifecycle;
- production Level 1 firing;
- enemy-bullet simulation and rendering;
- progression;
- death/re-entry;
- Super Zapper;
- recording/playback;
- existing Level 1 tests;
- Levels 2-8 regression tests;
- README.md;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md.

Existing reports are historical records.

Do not rewrite them.


MANUAL EVIDENCE FROM NEAL

Neal performed the controlled Level 1 A/B comparison using the physical
arcade-style spinner.

Initial capped-diagnostic observation:

- the capped version felt better;
- a defect was found in which enemy bullets were simulated and lethal but not
  rendered.

The renderer defect was then corrected.

Neal manually retested the corrected capped diagnostic with visible enemy
bullets.

Final experiment result:

- enemy bullets are now visible;
- bullet visibility is fixed;
- the capped Level 1 is much better;
- the capped Level 1 feels the same as or close to the original arcade Level 1;
- Neal visibly noticed fewer Flippers occupying the tube simultaneously;
- the improvement remains after normal enemy-shot visibility was restored.

Treat this as strong manual acceptance of the four-concurrent-Flipper rule for
production Level 1.

This does not establish that the current C# Egg waiting mechanism literally
matches the Atari pending-entry implementation.

This does not accept the same rule automatically for Level 2 or later levels.


PRODUCTION CORRECTION

Normal production Level 1 must now enforce:

Maximum concurrent active Flippers: 4

Apply this to all normal production Level 1 entry paths, including:

- no-option fresh campaign startup;
- generic direct startup using --level 1;
- fresh Level 1 restart after Game Over;
- campaign restart;
- Level 1 death/re-entry;
- any accepted production Level 1 initialization path;
- deterministic tests and headless production startup.

Do not require a diagnostic option.

Do not apply the rule only to a terminal development scenario.


ROM-BACKED VALUE

The cap value four is:

ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED

For Level 1:

- total original supply remains 10;
- ordinary species remain Flippers only;
- maximum concurrent Flippers becomes 4.

The C# method of keeping additional ready Eggs waiting is:

PROJECT TRANSLATION / MANUALLY ACCEPTED RESULT

Do not claim that waiting C# Eggs are the literal Atari pending-entry data
structure or timing mechanism.


WAITING-EGG SEMANTICS

Preserve the validated mechanism:

- Count active Flippers from the authoritative active-enemy collection.
- When four active Flippers exist, another ready Level 1 Egg cannot hatch.
- The ready Egg remains ReadyToHatch.
- It remains unresolved.
- It does not disappear.
- It does not return to reserve.
- It does not count as destroyed or rim-cleared.
- It does not reroll or consume new randomness while waiting.
- It does not retarget or change identity/lane/direction every tick.
- When the active Flipper count falls below four, an eligible waiting Egg may
  hatch through the existing normal hatch path.
- No update may transiently produce five active Flippers.
- All 10 Level 1 obligations must eventually resolve.

Use existing lifecycle state rather than adding a second queue or cached
parallel counter.


WHAT COUNTS AS ACTIVE

A Flipper begins counting when the ordinary hatch path adds it to the active
Flipper collection.

A Flipper stops counting only through the existing lifecycle when it is removed
after:

- normal destruction;
- completed Super Zapper resolution;
- rim clear;
- reconstruction/removal during the existing death lifecycle;
- another already-established terminal removal condition.

A Flipper with a pending delayed Zapper resolution remains active until the
existing lifecycle removes it.

Do not count:

- reserve supply;
- roaming Eggs;
- approaching Eggs;
- ready Eggs;
- bullets;
- shots;
- Spikes;
- presentation objects.

Do not introduce a count whose value can drift from the actual collection.


TOTAL SUPPLY AND ACCOUNTING

Preserve Level 1 total supply:

10

At all meaningful lifecycle boundaries, preserve exact conservation among:

- reserve originals;
- Eggs in all lifecycle states;
- active Flippers;
- destroyed outcomes;
- rim-cleared outcomes;
- any existing legitimate terminal resolution states.

Do not:

- discard a capped Egg;
- duplicate an obligation;
- manufacture a replacement;
- decrement unresolved accounting prematurely;
- complete the level while an unresolved waiting Egg remains;
- deadlock completion after an active slot becomes available.


EGG SCHEDULE

Keep the accepted C# Egg schedule unchanged for this production correction:

- four starting Eggs;
- existing reserve quantity;
- 50-tick reserve-release policy;
- existing approach-selection cadence;
- existing approach limits;
- existing Egg movement;
- existing ready-to-hatch timing;
- existing deterministic lane/direction behavior.

The only production behavior change is that Level 1 hatching waits at four
active Flippers.

Do not modify reserve release simply because ready Eggs are waiting.

Do not introduce the separate ROM six-total-active ordinary-enemy gate in this
milestone.


DO NOT IMPLEMENT LEVEL 2 YET

The comparative investigation found a related missing four-Flipper maximum on
Level 2.

Do not change Level 2 in this milestone.

Level 2 requires its own bounded implementation and manual acceptance because
it includes below-rim lane changes and a different geometry/play rhythm.

Preserve Levels 2-8 exactly except for shared implementation refactoring that
is demonstrably inactive for them.


DO NOT IMPLEMENT THE SIX-ACTIVE GATE

The ROM also has a six-total-active ordinary creation gate.

Do not implement it now.

Level 1's four-Flipper species maximum is stricter and already isolates the
accepted correction.

Do not combine this production change with:

- a six-active global gate;
- revised Egg release timing;
- revised approach timing;
- revised ROM pending-entry timing;
- species-policy changes beyond Level 1.


NO OTHER GAMEPLAY TUNING

Do not change:

- accepted 2.5 rim-speed calibration;
- below-rim climb speed;
- rim pause;
- rim targeting;
- directed pivot duration;
- player firing;
- player-shot capacity;
- player-shot cooldown;
- shot speed;
- shot lifetime;
- player-shot collision;
- enemy firing;
- enemy-shot capacity;
- enemy-shot holdoff;
- enemy-shot speed;
- enemy-bullet lifetime;
- collision ordering;
- collision tolerance;
- scoring;
- lives;
- Super Zapper;
- spinner sensitivity;
- --speed;
- geometry;
- rendering other than retaining the required bullet-visibility correction;
- audio;
- total Level 1 supply.

This milestone promotes one validated concurrency rule only.


ENEMY-BULLET VISIBILITY CORRECTION

Preserve the already validated correction.

The prior defect was:

- bullets were created;
- bullets moved;
- bullets could collide with and destroy the Blaster;
- the diagnostic renderer classification skipped drawing them.

The production candidate must preserve accepted enemy-bullet rendering for:

- normal campaign Level 1;
- generic --level 1;
- any retained direct/development Level 1 paths;
- Levels 2-8;
- the separate --enemy-bullets-test.

Do not couple ordinary production bullet visibility to:

- natural progression only;
- a production-ledger requirement;
- the development bullet-test selector.

Prefer a presentation rule based on actual supported bullet simulation/state
rather than repeatedly enumerating scenario names, if that is demonstrably safe.

However, do not broaden rendering behavior into scenarios where bullets are
intentionally unsupported.

Keep the existing white color, geometry, size, rotation, projection, and
collision-state identity.


DIAGNOSTIC OPTION DISPOSITION

The existing temporary option is:

--level1-four-flipper-cap-test

Now that the rule is being promoted into production Level 1, determine the
cleanest disposition.

Preferred outcome:

- remove the temporary diagnostic option;
- remove its terminal scenario identity;
- remove obsolete parser/conflict wiring;
- remove documentation that instructs users to select the diagnostic;
- convert useful diagnostic tests into production Level 1 fidelity tests;
- retain useful renderer regression coverage in a production-relevant form.

Do not keep a redundant option whose only behavior now matches production.

If a narrow reason requires temporarily retaining the option, it must not
create a second divergent Level 1 implementation, and the report must explain
why removal was unsafe.

Do not leave dead flags or misleading README commands.


PRODUCTION PROGRESSION

Normal Level 1 must still progress naturally into Level 2.

Verify:

- all 10 obligations resolve normally;
- waiting ready Eggs hatch as active slots open;
- no early completion occurs;
- no permanent waiting-Egg deadlock occurs;
- successful Level 1 exit remains unchanged;
- between-level approach remains unchanged;
- score and lives carry;
- Super Zapper recharges according to accepted behavior;
- Level 2 starts normally;
- no development terminal boundary appears in normal production play.

Generic --level 1 should follow its currently accepted direct-level policy.

Do not accidentally convert production Level 1 into the old terminal
diagnostic behavior.


DEATH / RE-ENTRY

Production Level 1 death/re-entry must preserve:

- the four-active-Flipper maximum;
- all unresolved obligations;
- Level 1 supply conservation;
- existing score/lives behavior;
- existing player re-entry timing;
- existing firing availability;
- existing enemy-shot behavior;
- existing Super Zapper state;
- existing random/deterministic behavior.

Repeated deaths must not:

- exceed the cap;
- lose waiting Eggs;
- duplicate Flippers;
- falsely complete the wave;
- permanently block further hatching.


SUPER ZAPPER

Preserve accepted Level 1 Super Zapper behavior.

When Zapper resolution reduces active Flippers:

- a waiting ready Egg may hatch through the normal path once the slot is
  available;
- the active count must not exceed four;
- delayed Zapper lifecycle remains unchanged;
- supply remains conserved;
- charges and audio remain unchanged.

Do not make Eggs or reserve supply new Zapper targets.


RECORDING AND PLAYBACK

Production Level 1 recordings must use the new production rule automatically.

Do not add a four-Flipper-cap field or scenario flag to the recording format.

Do not change recording schema unless an existing compatibility contract
demonstrably requires it.

Verify:

- live recording captures player input normally;
- visible playback starts production Level 1 with the new rule;
- headless playback uses the new production rule;
- pause/resume remains correct;
- forward stepping remains correct;
- reverse reconstruction remains exact under the current code;
- screenshots remain correct;
- live input handoff remains correct.

Historical recordings may exercise updated production simulation when replayed
under the new code.

Do not claim byte-for-byte historical trajectory compatibility unless tests
actually establish it.

Record the exact compatibility result.

Use a fresh production Level 1 recording for manual validation if older input
files diverge because production behavior intentionally changed.


RANDOMNESS

Cap blocking must not consume random draws repeatedly.

Verify that:

- a ready Egg waiting at the cap does not repeatedly invoke the hatch factory;
- lane/direction/type/firing state does not reroll while waiting;
- identical seeds and inputs produce identical production results;
- uncapped Levels 2-8 retain their current random sequences except where a
  shared code refactor is proven behavior-neutral.

Do not introduce global randomness changes.


REQUIRED TEST CONVERSION

Review diagnostic tests and classify them as:

1. Production fidelity tests to retain and rename/refocus.
2. Temporary CLI/terminal-diagnostic tests to remove.
3. Renderer regression tests to retain in production-relevant form.
4. Redundant tests to remove only if equal or stronger coverage remains.

Do not merely leave all tests tied to a removed diagnostic flag.

Maintain clear test names that describe production behavior.


REQUIRED PRODUCTION TESTS

Add or convert tests proving all of the following.


1. PRODUCTION STARTUP

- no-option Level 1 uses the four-Flipper maximum;
- --level 1 uses the same production maximum;
- displayed level remains 1;
- total supply remains 10;
- four starting Eggs remain;
- accepted geometry/player start remain unchanged.


2. FOUR-FLIPPER MAXIMUM

- production Level 1 never exceeds four active Flippers;
- the fifth ready Egg waits;
- the waiting Egg remains unresolved;
- no transient fifth Flipper appears;
- after a slot opens, a waiting Egg can hatch normally.


3. ACCOUNTING

At meaningful boundaries:

reserve + Eggs + active Flippers + destroyed + rim-cleared
    = 10

Use the actual accepted accounting model and include any other legitimate
terminal state if required.

No duplication or loss.


4. DETERMINISM

- fixed seed/input yields the same cap/wait sequence;
- waiting does not consume repeated randomness;
- repeated runs match.


5. RELEASE-TIMING ISOLATION

- reserve release remains 50 ticks;
- four starting Eggs remain;
- approach policy remains unchanged;
- only hatch eligibility changes at four active Flippers;
- no six-total-active gate was added.


6. MOVEMENT AND RIM BEHAVIOR

- below-rim movement remains unchanged;
- accepted rim pause remains unchanged;
- accepted 2.5 rim-speed calibration remains unchanged;
- targeting and pivot behavior remain unchanged.


7. PLAYER FIRING

- player firing cadence remains unchanged;
- shot capacity remains unchanged;
- shots and collision ordering remain unchanged.


8. ENEMY FIRING

- Level 1 Flippers still fire according to accepted production rules;
- enemy-shot capacity remains two;
- holdoff and firing eligibility remain unchanged;
- bullets remain visible;
- rendering and collision use the same active bullet state;
- production Level 1 does not require --enemy-bullets-test.


9. DEATH / RE-ENTRY

- reconstruction conserves unresolved supply;
- the cap remains active;
- repeated deaths cannot exceed four or lose obligations;
- completion remains reachable.


10. SUPER ZAPPER

- accepted Zapper behavior remains;
- delayed resolution frees active slots normally;
- waiting Eggs resume;
- no update exceeds four active Flippers.


11. COMPLETION AND PROGRESSION

- all 10 obligations resolve;
- no early completion;
- no deadlock;
- production Level 1 exits normally;
- natural Level 1-to-2 progression remains correct.


12. LEVELS 2-8 REGRESSION

- Level 2 remains unchanged and uncapped by this Level 1 rule;
- Levels 3-8 remain unchanged;
- Level 8 open topology/spinner behavior remains unchanged;
- no shared cap affects later levels.


13. CLI CLEANUP

If the old diagnostic option is removed:

- parser no longer accepts it;
- help/README no longer advertises it;
- obsolete conflict logic is removed;
- production --level 1 remains valid;
- no dead diagnostic selection remains.

Avoid testing removed private implementation details when normal parser
rejection coverage is sufficient.


14. RECORDING / PLAYBACK

- production Level 1 recording works;
- visible playback works;
- headless playback works;
- pause/resume works;
- forward/reverse reconstruction works;
- no cap metadata is added;
- historic recording compatibility is characterized accurately.


BASELINE-VERSUS-PRODUCTION CHANGE TEST

Retain a test that documents the intentional behavioral difference from the
previous accepted baseline:

Before this correction, the deterministic Level 1 no-kill schedule reached five
active Flippers at boundary 289.

After this correction, production Level 1 remains at four while a ready Egg
waits.

Do not create a test that requires checking out or executing the old commit.

Encode the old result only as historical context in the test name/comment or
report, while asserting the new production behavior from current code.

Avoid brittle dependence on boundary 289 unless that boundary remains a useful
isolation check and the existing unchanged schedule makes it stable.


VALIDATION

During implementation:

1. Run narrow production Level 1 cap/accounting tests.

2. Run enemy-firing/rendering regressions.

3. Run death/re-entry and Super Zapper tests.

4. Run completion and natural Level 1-to-2 progression tests.

5. Run recording/playback/pause/step/reverse tests.

6. Run Levels 2-8 regression coverage.

7. Run expanded affected coverage including:
   - CLI/startup;
   - level configuration;
   - Egg release/approach/hatching;
   - Flipper lifecycle;
   - enemy firing;
   - bullet rendering/collision;
   - rim pursuit;
   - player firing/shots;
   - death/re-entry;
   - Super Zapper;
   - completion;
   - progression;
   - campaign soak;
   - recording;
   - visible/headless playback;
   - screenshots;
   - --speed;
   - --spinner-sensitivity.

8. Run the full solution:

dotnet test TempestCSharp.slnx

9. Run the full build:

dotnet build TempestCSharp.slnx

10. Run:

git diff --check

Do not launch the interactive game.

Neal will perform final production Level 1 manual acceptance.


WINDOWS APPLICATION CONTROL

Windows Application Control has intermittently blocked generated assemblies.

Do not weaken Windows security.

If a validation command is blocked:

- record the exact command and error;
- distinguish the security block from a test failure;
- retry only when useful;
- use --no-build only when it genuinely tests the current candidate;
- do not claim blocked validation passed.


DOCUMENTATION

Update:

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record:

- production Level 1 now has a maximum of four concurrent active Flippers;
- cap value four is ROM-backed;
- C# ready-Egg waiting is a project translation;
- total supply remains 10;
- Egg release/approach scheduling remains the accepted C# policy;
- the six-total-active ROM gate is not yet implemented;
- Level 2 remains unchanged and pending separate analysis/implementation;
- accepted rim speed, movement, firing, bullets, collisions, and spinner
  behavior remain unchanged;
- the diagnostic A/B comparison was manually successful;
- visible-bullet correction was manually verified;
- production manual acceptance remains pending;
- no claim is made that all Level 1 timing now literally matches the ROM.

Remove obsolete development-diagnostic instructions if the option is removed.


EVIDENCE CLASSIFICATION

Use:

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

MANUALLY ARCADE-FIDELITY SUPPORTED

PROJECT TRANSLATION

CURRENT PRODUCTION CANDIDATE

AUTOMATED TEST EVIDENCE

FINAL PRODUCTION MANUAL ACCEPTANCE PENDING

Do not label the waiting-Egg mechanism as literal arcade implementation.


PROTECTED FILES

Do not modify, stage, or commit:

Reference/arcade/roms.rev2/
UserData/
screenshots
gameplay recordings
Videos/
TEMP/
historical AI reports
unrelated local files

Do not run git clean.

Do not read TEMP files unless explicitly required.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW production implementation report under:

AI-Logs/Reports/

3. Existing investigation, diagnostic, and visibility-correction reports remain
   historical records.

4. Do not rewrite existing reports.

5. AI-Logs is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place the report under TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Committed baseline.
3. Existing unstaged candidate inherited.
4. Reports reviewed.
5. Neal's A/B manual evidence.
6. Production policy adopted.
7. Diagnostic flag disposition.
8. Exact cap semantics.
9. Active-count boundary.
10. Waiting-Egg mechanism.
11. Randomness preservation.
12. Supply/accounting conservation.
13. Egg timing preservation.
14. Rim/movement preservation.
15. Player/enemy firing preservation.
16. Enemy-bullet visibility preservation.
17. Death/re-entry.
18. Super Zapper.
19. Completion and natural progression.
20. Recording/playback compatibility.
21. Levels 2-8 isolation.
22. Files changed.
23. Tests converted/added/removed.
24. Focused results.
25. Expanded results.
26. Full-suite result.
27. Build result.
28. `git diff --check`.
29. Protected files unchanged.
30. Final Git state.
31. Manual commands.
32. Final production acceptance checklist.
33. Remaining Level 1 unknowns.
34. Deferred Level 2 and six-active work.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the production candidate unstaged for Neal's final manual acceptance.

Do not begin Level 2 concurrency work.

Do not implement the six-total-active gate.

Do not change rim speed.

Do not begin Level 9.


MANUAL TEST COMMANDS

Provide exact commands supported by the finished implementation.

At minimum:


Normal production campaign Level 1:

dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1


Generic direct production Level 1:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1


Natural campaign for Level 1-to-2 progression:

dotnet run --project src\Tempest.Game -- --lives 10 --spinner-sensitivity 1


Fresh production recording:

dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level1-four-flipper-production.json --level 1 --lives 10 --spinner-sensitivity 1


Visible playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level1-four-flipper-production.json


Do not provide the removed diagnostic command if the option no longer exists.


FINAL MANUAL ACCEPTANCE CHECKLIST

Ask Neal to verify:

1. Normal no-option Level 1 now has arcade-like threat density.
2. No more than four Flippers appear active simultaneously.
3. Additional Eggs wait and later hatch as active slots open.
4. All 10 enemies eventually appear and resolve.
5. The wave does not feel unnaturally short or sparse.
6. Enemy bullets remain visible.
7. Enemy bullets move, sound, and collide normally.
8. The Blaster is not destroyed by invisible threats.
9. Rim pursuit remains threatening.
10. Stationary defense has not become trivially safe.
11. The game retains useful breathing room and target manageability.
12. Spinner control remains correct.
13. Player firing and Super Zapper remain normal.
14. Death/re-entry remains coherent.
15. Level 1 completes normally.
16. Natural Level 1-to-2 progression works.
17. Level 2 remains behaviorally unchanged.
18. Fresh recording/playback reproduces the production run.
19. Pause/resume/forward/reverse remain correct.
20. No obvious Levels 2-8 regression appears.

Do not require an exact death count.

The authority is Neal's judgment of threat timing, spacing, and arcade feel.


MANUAL ACCEPTANCE STATUS

The production candidate remains FINAL MANUAL ACCEPTANCE PENDING.

Do not claim the production correction is accepted until Neal completes this
test.

STOP after automated validation and manual handoff.

TEMPEST_LEVEL1_PRODUCTION_FOUR_FLIPPER_CORRECTION_20261109