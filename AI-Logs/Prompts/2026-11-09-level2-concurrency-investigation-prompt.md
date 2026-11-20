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

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Perform a tightly bounded, read-only Level 2 concurrency investigation.

Determine the precise Atari ROM concurrency policy for production Level 2 and
compare it with the currently accepted C# Level 2 implementation.

The immediate question is whether Level 2 has the same excessive simultaneous
Flipper-density problem that was just identified and corrected on production
Level 1.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

f362af7f87c23a9b983937f8733f5a4ac71d0162
Implement accepted Level 1 four-Flipper maximum

Confirm the actual:

- branch;
- HEAD;
- origin/master;
- live remote master if appropriate;
- tracked worktree;
- index;
- untracked state.

before beginning.

This task is investigation-only.

Do not modify production code.

Do not modify tests.

Do not update durable documentation.

Do not stage, commit, or push.

Do not launch the interactive game.

Do not begin Level 9.


PRIMARY CONTEXT

Production Level 1 has just been corrected and manually accepted.

The accepted Level 1 result is:

- total original supply remains 10 Flippers;
- maximum concurrently active Flippers is 4;
- additional ready Eggs wait while four Flippers are active;
- all 10 obligations eventually resolve;
- accepted Egg release timing remains unchanged;
- accepted rim-speed calibration remains unchanged;
- firing, shots, collisions, spinner behavior, and global speed remain
  unchanged;
- Neal judged the resulting production Level 1 substantially improved and the
  same as or close to the original arcade Level 1;
- Neal visibly observed fewer Flippers simultaneously occupying the tube;
- full Levels 1-8 manual play/record regression found no blocking problem.

Freeze that production Level 1 behavior.

Do not modify or reopen Level 1.


REQUIRED HISTORICAL REPORTS

Read, as relevant:

AI-Logs/Reports/2026-11-09-low-level-difficulty-comparative-investigation-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-cap-diagnostic-report.md

AI-Logs/Reports/2026-11-09-level1-cap-enemy-bullet-visibility-fix-report.md

AI-Logs/Reports/2026-11-09-level1-production-four-flipper-correction-report.md

AI-Logs/Reports/2026-11-09-level1-four-flipper-production-closeout-report.md

Also review earlier Level 2 reports and tests, including as applicable:

- Level 2 square geometry analysis;
- Level 2 canflip analysis/implementation;
- Level 2 production/progression work;
- enemy firing investigation;
- rim-pursuit work;
- campaign progression;
- gameplay recording/playback tests;
- current LevelConfiguration.Level2Square;
- current Egg/hatch implementation.

Reuse established findings.

Do not repeat unrelated Level 2 geometry or canflip investigations.


PRIMARY ROM AUTHORITIES

Use:

Reference/arcade/Tempest Rom Source code.md

and when raw table bytes are needed:

Reference/arcade/roms.rev2/

These are the primary local Atari sources.

Do not search the internet for another disassembly.

Do not modify the ROM files.

Use the current C# implementation for comparison.

Use the project-local Mark Incitti implementation only as a secondary
engineering reference when useful.

Do not let Incitti override explicit ROM evidence.


USER'S MAIN QUESTION

Determine whether accepted production Level 2 currently presents too many
simultaneous Flippers compared with the Atari ROM.

The recently corrected Level 1 problem was:

matching total enemy supply
but
too many enemies active at the same time.

Do not assume Level 2 has exactly the same cause merely because the ROM appears
to contain a similar four-Flipper maximum.

Trace Level 2 independently.


LEVEL 2 CURRENT ACCEPTED IDENTITY

Establish the current production Level 2 configuration, including:

- displayed/internal level identity;
- square geometry;
- total original supply;
- initial Eggs;
- reserve quantity;
- ordinary eligible species;
- canflip behavior;
- Flipper firing;
- active limit behavior;
- Egg release schedule;
- approach schedule;
- ready/hatch policy;
- death/re-entry;
- Super Zapper;
- completion;
- natural Level 1 -> 2 entry;
- natural Level 2 -> 3 progression.

Do not alter any of these.


ROM LEVEL 2 SUPPLY

Recover and confirm Level 2 Medium/default values for:

- wave_enemies;
- ordinary Flipper eligibility;
- min_flippers;
- max_flippers;
- max_enm or equivalent global active gate;
- enemy-shot capacity;
- Flipper speed;
- top-rim acceleration;
- shot holdoff;
- any other concurrency-related table affecting how many threats can coexist.

Where Easy and Hard materially clarify a table, record them separately.

Focus primarily on Medium/default behavior.

Neal's cabinet is believed Medium but remains hardware-setting unverified.


FOUR-FLIPPER LIMIT

Determine definitively whether the Level 2 ROM maximum concurrent-Flipper value
is four.

Trace:

- the table value;
- how the table is loaded;
- the enemy-selection routine;
- whether the comparison is inclusive or exclusive;
- when a Flipper begins counting;
- when it stops counting;
- whether Eggs/pending units contribute to the species count;
- whether delayed destruction or Super Zapper targets remain active for the
  count;
- whether rim enemies count;
- whether below-rim and rim Flippers use the same count.

Do not infer runtime semantics from the table value alone.

Trace the actual creation/eligibility control flow.


SIX-TOTAL-ACTIVE GATE

The ROM also has a general active-enemy gate believed to be six.

For Level 2, determine precisely:

- whether the six-total-active gate applies;
- what objects count toward it;
- whether it is evaluated before or after species limits;
- whether Eggs/pending units count;
- whether projectiles count;
- whether Level 2's four-Flipper limit always becomes the stricter effective
  gate because Level 2 is Flipper-only;
- whether the six-active gate can have any observable effect on Level 2 beyond
  the four-Flipper maximum.

This distinction is important.

Do not implement the six-active gate.

The purpose is to determine whether it matters independently on Level 2.


CURRENT C# LEVEL 2 CONCURRENCY

Trace production C# Level 2.

Determine:

- total original supply;
- currently possible maximum active Flippers;
- whether the accepted Level 1 cap infrastructure affects Level 2;
- whether Level 2 has any independent species cap;
- whether Level 2 has a global active cap;
- whether ready Eggs are currently allowed to hatch without limit;
- when the fifth active Flipper can appear;
- whether all 12 original Flippers can eventually be active simultaneously
  under a no-kill schedule.

Do not infer this solely from configuration.

Use existing code and, if useful, bounded read-only test/headless execution.


BOUNDED DETERMINISTIC MEASUREMENT

If existing test/headless APIs make this practical, run a bounded deterministic
Level 2 no-input/no-kill measurement.

Do not add tests or diagnostics.

Use an existing seed where practical.

Measure, as available:

- simulation boundary when active Flipper count first reaches 1;
- 2;
- 3;
- 4;
- 5;
- peak active Flipper count;
- reserve quantity;
- Egg lifecycle states;
- unresolved total;
- rim count;
- enemy-bullet count.

Use an explicit tick bound.

Do not run an unbounded loop.

Remember the recent Level 1 test-hang lesson.

Every analysis loop must have an explicit maximum boundary and fail/report
cleanly if the requested state is never reached.


COMPARE WITH ACCEPTED LEVEL 1

Compare Level 2 against the newly accepted Level 1 production rule.

Determine which parts of the Level 1 implementation infrastructure could be
safely reused if Level 2 later receives the same ROM-backed cap.

Do not implement anything.

Identify whether the production code currently contains:

- a Level-1-specific condition;
- a configuration-driven cap;
- a reusable property;
- or another mechanism.

Recommend the smallest architecture for a later Level 2 experiment.

Do not generalize production behavior prematurely.


LEVEL 2 CANFLIP INTERACTION

Level 2 differs materially from Level 1 because Flippers can change lanes below
the rim.

Investigate whether limiting simultaneous Flippers to four could interact with:

- canflip direction selection;
- lane clustering;
- pivot concurrency;
- collision opportunity;
- player-shot opportunity;
- enemy firing during pivots;
- simultaneous rim arrival;
- deterministic random sequence.

Do not change canflip behavior.

Determine whether the same four-enemy cap could feel materially different from
Level 1 because four Level 2 Flippers are more spatially dynamic.


RELEASE / HATCH DENSITY

Compare current C# Level 2 threat delivery with ROM Level 2.

Trace:

- four starting Eggs;
- reserve release timing;
- approach timing;
- ready-to-hatch timing;
- active creation:
- how waiting would work if a four-Flipper cap were imposed;
- whether several Eggs can become ready while four Flippers are active;
- how quickly queued Eggs could hatch after several Flippers are destroyed;
- whether a burst of delayed hatches could create a pressure spike.

Determine whether simple waiting is sufficient for a clean controlled
experiment or whether Level 2 exposes a problem not seen on Level 1.

Do not alter release timing.


ENEMY FIRING

Confirm Level 2 firing pressure remains a separate variable.

Trace or reuse established findings for:

- Level 2 Flipper firing;
- global projectile capacity;
- holdoff;
- firing during below-rim canflip transitions;
- bullet visibility;
- projectile lifetime;
- collision.

Determine whether a four-Flipper cap would naturally reduce the number of
eligible firing sources while leaving the global shot capacity unchanged.

Do not change firing.


RIM PRESSURE

Preserve current accepted rim behavior as a separate constant.

Investigate only enough to determine how reducing simultaneous Flippers from
the current C# peak to four would change:

- simultaneous rim arrivals;
- number of pursuers;
- multi-direction pressure;
- time between threats.

Do not alter the accepted 2.5 rim-speed calibration.

Do not combine rim-speed tuning with a concurrency experiment.


DEATH / RE-ENTRY

Determine whether a future Level 2 cap would remain valid across current
production Level 2 death/re-entry.

Trace:

- unresolved supply reconstruction;
- active Flipper reconstruction;
- Eggs;
- reserve;
- ready state;
- canflip state;
- random state;
- projectiles;
- Super Zapper.

Identify any Level 2-specific re-entry complication that was absent on Level 1.

Do not modify death/re-entry.


SUPER ZAPPER

Determine whether existing Level 2 Super Zapper lifecycle would naturally free
Flipper slots in the same way Level 1 does.

Confirm whether delayed Zapper targets remain active for cap purposes until
normal removal.

Do not change Zapper behavior.


COMPLETION / PROGRESSION

Determine whether a future Level 2 waiting mechanism could interfere with:

- wave completion;
- terminal accounting;
- natural Level 2 -> Level 3 progression;
- direct Level 2 terminal behavior;
- replay/restart;
- recording/playback.

Identify any tests that currently assume all Level 2 Eggs hatch without active
slot pressure.

This is particularly important after the Level 1 stale-helper hang.

List every test/helper pattern that would need a bound or semantic update in a
future implementation.

Do not modify those tests now.


RECORDING / PLAYBACK

Determine whether promoting a Level 2 four-Flipper maximum later would require
any recording-format change.

Preferred expectation:

- no metadata;
- ordinary production simulation changes;
- new recordings naturally use the new production policy;
- existing playback uses whatever production simulation the current version
  defines;
- pause/step/reverse remain unchanged.

Verify this from architecture.

Do not modify recording code.


DIAGNOSTIC EXPERIMENT RECOMMENDATION

If the evidence confirms that:

- Level 2 ROM max Flippers = 4;
- current C# Level 2 can exceed four simultaneously;
- simple ready-Egg waiting can isolate the difference without altering other
  mechanics;

recommend ONE development-only Level 2 diagnostic.

A likely name is:

--level2-four-flipper-cap-test

Do not implement it now.

Recommended diagnostic should preserve:

- total supply 12;
- accepted square geometry;
- accepted canflip;
- four starting Eggs;
- reserve timing;
- enemy firing;
- rim speed;
- player firing;
- collisions;
- Super Zapper;
- death/re-entry;
- direct-scenario terminal behavior;
- spinner sensitivity;
- global speed.

Only hatch eligibility at four active Flippers should differ.


DO NOT PROMOTE DIRECTLY TO PRODUCTION

Even if Level 2 ROM evidence clearly shows maximum four Flippers, do not
recommend skipping the manual diagnostic unless there is a compelling reason.

Level 2's canflip behavior makes its threat pattern sufficiently different that
Neal should compare it independently.

The successful Level 1 result is evidence for the method, not automatic manual
acceptance of Level 2.


LEVEL 1 IS FROZEN

Do not modify:

- the accepted production Level 1 four-Flipper maximum;
- waiting-Egg behavior;
- total supply;
- bullet rendering;
- progression;
- tests;
- documentation.

Use Level 1 only as an architectural and experimental reference.


LEVELS 3-8 ARE FROZEN

Do not modify or reopen accepted Levels 3-8.

Do not use this investigation to broaden concurrency work into those levels.

Record later-level observations only when necessary to understand a shared
architecture.


NO IMPLEMENTATION

Do not:

- add a Level 2 cap;
- add a diagnostic flag;
- add the six-active gate;
- change Egg timing;
- change canflip;
- change rim speed;
- change firing;
- change bullets;
- change collisions;
- change death/re-entry;
- modify tests;
- update durable documentation;
- stage;
- commit;
- push.

This is investigation only.


OPTIONAL READ-ONLY COMMANDS

Use existing tests/headless tooling only where they answer a specific question.

Do not run the full suite or build merely as ritual.

Do not launch the interactive game.

Do not run unbounded state loops.

Any diagnostic shell/script loop must have:

- explicit maximum ticks/iterations;
- clear timeout or stop condition;
- useful failure output.


EVIDENCE CLASSIFICATION

Use:

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

CURRENT ACCEPTED C# BEHAVIOR

ACCEPTED LEVEL 1 ARCHITECTURAL REFERENCE

AUTOMATED TEST EVIDENCE

BOUNDED HEADLESS MEASUREMENT

PROJECT TRANSLATION CANDIDATE

MANUAL ACCEPTANCE REQUIRED

UNKNOWN

Do not label Level 2 behavior manually accepted merely because Level 1 was.


TEMP DIRECTORY POLICY

TEMP/ is disposable local working material.

Do not read:

TEMP/myNotes.md

or other TEMP files unless a particular file is explicitly required by this
task.

Do not use TEMP files as evidence.

If temporary analysis output is created under TEMP, remove only the files
created by this task before final status.

Do not alter unrelated TEMP material.


PROTECTED FILES

Do not modify:

Reference/arcade/roms.rev2/

UserData/

screenshots

recordings

Videos/

existing AI reports

unrelated local files.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW Level 2 concurrency investigation report under:

AI-Logs/Reports/

3. Existing reports remain historical records.

4. Do not rewrite existing reports.

5. AI-Logs is Git-ignored.

6. Do not stage or commit AI-Logs.

7. Do not place the report under TEMP/ or Temp/.


REPORT REQUIREMENTS

Create a descriptive date-prefixed report under AI-Logs/Reports/.

Include:

1. Starting Git state.
2. Accepted baseline.
3. Relevant Level 1 findings reused.
4. Level 2 current production configuration.
5. Level 2 ROM supply.
6. Level 2 min/max Flipper values.
7. Six-active-gate trace.
8. Exact ROM creation eligibility/order.
9. Definition of active for species/global limits.
10. Current C# Level 2 maximum concurrency.
11. Bounded deterministic measurement, if performed.
12. First fifth-Flipper boundary, if established.
13. Peak active count under bounded no-kill schedule.
14. Current Egg/release/approach behavior.
15. Canflip interaction.
16. Enemy firing interaction.
17. Rim-pressure implications.
18. Death/re-entry implications.
19. Super Zapper implications.
20. Completion/progression implications.
21. Test/helper hang-risk audit.
22. Recording/playback implications.
23. Comparison with accepted Level 1 architecture.
24. Whether simple ready-Egg waiting can isolate the Level 2 cap.
25. Whether the six-active gate independently matters on Flipper-only Level 2.
26. Exact unresolved questions.
27. Recommended diagnostic experiment.
28. Proposed automated tests.
29. Proposed manual A/B checklist.
30. Final Git state proving no tracked change.


FUTURE AUTOMATED TEST PLAN

If a diagnostic is recommended, propose tests covering:

- default production Level 2 remains unchanged;
- diagnostic uses accepted production geometry/configuration;
- total supply remains 12;
- four starting Eggs remain;
- active Flippers never exceed four in diagnostic;
- fifth ready Egg waits;
- no reroll/random consumption while waiting;
- waiting Egg hatches when slot opens;
- supply/accounting remains 12;
- canflip remains unchanged;
- firing remains unchanged;
- bullets remain visible;
- rim-speed calibration remains unchanged;
- death/re-entry remains coherent;
- Super Zapper frees slots correctly;
- completion remains reachable;
- direct diagnostic remains terminal/replayable according to existing policy;
- natural production Level 2 -> 3 remains unchanged;
- Level 1 remains identically accepted;
- Levels 3-8 remain unchanged;
- all test loops are explicitly bounded.


FUTURE MANUAL A/B PLAN

If evidence supports the diagnostic, recommend comparing:

Production Level 2:

dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1

against a future diagnostic such as:

dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1

Ask Neal eventually to observe:

1. Number of simultaneous visible Flippers.
2. Below-rim canflip density.
3. Number of simultaneous pivots.
4. Number approaching the rim together.
5. Number of rim pursuers.
6. Enemy-bullet pressure.
7. Multi-direction threat convergence.
8. Recovery/breathing room.
9. Ability to track targets deliberately.
10. Whether the wave feels too sparse.
11. Whether stationary defense becomes too easy.
12. Whether queued Eggs continue entering as slots open.
13. Whether all 12 obligations appear.
14. Death/re-entry.
15. Super Zapper follow-up pressure.
16. Wave completion.
17. Arcade resemblance.

Do not use exact death count as the main fidelity metric.


STOP POINT

Stop after the complete Level 2 read-only concurrency investigation and
recommendation.

Do not implement the diagnostic.

Do not modify production behavior.

Do not start Level 9.

At completion provide Neal a concise summary answering:

- Does ROM Level 2 have a four-Flipper maximum?
- Does the six-total-active gate matter independently on Level 2?
- How many Flippers can current C# Level 2 activate?
- Does C# demonstrably exceed the ROM concurrency limit?
- Does Level 2's canflip behavior complicate a simple cap experiment?
- Can the accepted Level 1 waiting-Egg mechanism be safely reused?
- Are there death/re-entry or Super Zapper complications?
- Which tests risk hanging after a future cap?
- Would recording/playback require any change?
- Is a Level 2 four-Flipper-cap diagnostic the correct next experiment?

TEMPEST_LEVEL2_CONCURRENCY_INVESTIGATION_20261109