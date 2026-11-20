Implement C4 — natural Level 2 -> Level 3 progression.

CURRENT CLEAN CHECKPOINT

e76d0b0 Complete direct Level 3 mixed-wave access (C3b)

Supporting checkpoints:

233bf25 Document Level 3 mixed-wave architecture
2b2e50c Implement headless Level 3 mixed-wave lifecycle

PRIMARY AUTHORITY

AI-Logs/Reports/2026-11-01-c4-natural-level3-progression-preimplementation-analysis-report.md

Supporting authority:

- current committed source and tests;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- README.md;
- existing C3 architecture/lifecycle documentation;
- AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md;
- AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-report.md;
- AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-implementation-report.md;
- AI-Logs/Reports/2026-11-01-c3b-level3-mixed-wave-implementation-report.md.

Use the current C4 pre-implementation report as primary guidance where older planning documents are stale.

C4 ANALYSIS DISPOSITION

The completed read-only C4 analysis concluded:

- GO for C4;
- natural Level 2 currently becomes terminal for two independent reasons:
  1. LevelProgression has no Level 2 successor;
  2. the successful-exit commit path separately only permits Level 1 to advance;
- the narrow solution is one explicit Level2Square -> Level3Mixed progression edge plus corresponding successful-exit commit support;
- Level 3 must use the already accepted LevelConfiguration.Level3Mixed model;
- existing fresh-wave initialization should be reused;
- Level 3 remains terminal;
- no Level 4 or campaign cycle;
- the Atari-style between-level approach presentation remains deferred and must NOT be bundled into C4;
- C4 should be one bounded implementation checkpoint;
- no C3 gameplay change is indicated.

C3 ACCEPTANCE BOUNDARY

C3 is complete and accepted.

C3a completed the deterministic/headless Level 3 lifecycle.

C3b completed graphical/direct access and manual acceptance.

Immediately before the C3b commit:

- full solution: 525 passed, 0 failed, 0 skipped;
- build: 0 warnings, 0 errors;
- git diff --check passed;
- direct graphical Level 3 was manually played multiple times by Neal;
- Entry/HUD: PASS;
- Tankers observed: YES;
- production Tanker bullets visible without --enemy-bullets-test: PASS;
- default ordinary/cargo Flippers silent: PASS;
- shot Tanker cargo release: PASS;
- natural Tanker cargo release: PASS;
- Super Zapper: PASS;
- death/re-entry: PASS;
- completion/replay: PASS;
- crashes/problems: NONE observed;
- manual rating: 5/5 — "Impressive."

Do not reopen or retune C3 merely to implement progression.

PURPOSE

Implement ONLY natural campaign progression:

Level 1
  ->
Level 2
  ->
Level 3

using the existing accepted:

LevelConfiguration.Level3Mixed

Level 3 remains terminal.

1. PROGRESSION EDGE

Extend the existing progression mechanism with the narrowest maintainable Level 2 -> Level 3 production edge.

Natural successful progression must be:

LevelConfiguration.Level1
  ->
LevelConfiguration.Level2Square
  ->
LevelConfiguration.Level3Mixed
  ->
terminal

Do not create another Level 3 configuration.

Do not make Level3Plus the production successor.

Level3Plus remains the existing Flipper-only geometry diagnostic configuration.

Do not implement Level 4.

Do not implement a repeating level cycle.

2. SUCCESSFUL EXIT COMMIT

Update the existing successful-exit commit logic only as necessary so normal Level 2 completion can commit progression into Level3Mixed.

Preserve established Level 1 -> Level 2 behavior.

Do not redesign the entire completion/transition architecture if the current mechanism can be extended narrowly.

The progression resolver and successful-exit commit must agree about whether Level 2 has a successor.

Avoid duplicated or competing progression truth.

3. LEVEL 3 FRESH ENTRY

Natural progression into Level 3 must create the SAME production Level3Mixed model already used and accepted by C3.

Use the existing fresh-level/fresh-wave initialization paths wherever possible.

Natural Level 3 entry must correctly establish:

- displayed Level 3;
- existing Level 3 plus-board geometry;
- fresh Level3WaveLedger;
- fresh production mixed-wave state;
- Level 3 randomized original Egg setup;
- Level 3 scheduling state;
- existing Level 3 player start/reset behavior;
- clean shots and enemy bullets appropriate to fresh-level entry;
- established renderer/HUD Level 3 identity;
- existing production Tanker firing policy;
- existing silent default Flipper policy.

Do not duplicate C3 initialization.

4. SCORE AND LIVES CARRY

Preserve existing campaign carry semantics.

Natural progression must carry:

- score from Level 1 -> Level 2;
- score from Level 2 -> Level 3;
- remaining lives from Level 1 -> Level 2;
- remaining lives from Level 2 -> Level 3.

Do not reset score/lives merely because Level 3 creates a fresh Level/wave.

Fresh-level gameplay state and carried player campaign state must remain properly distinct.

5. SUPER ZAPPER LEVEL-ENTRY STATE

Use the C4 analysis and existing Level/configuration initialization boundary to implement the established Level 3 fresh-entry Super Zapper state.

Do not infer new arcade rules.

Do not alter SuperZapperSystem targeting/timer semantics.

Do not invent a charge-display mechanism.

Preserve existing evidence classifications.

If Level3Mixed already defines two fresh charges and existing fresh-wave initialization resets to that configuration value, reuse that mechanism rather than special-casing natural progression.

6. LEVEL 3 REMAINS TERMINAL

C4 ends at natural access to Level 3.

After natural Level 3 completion:

- run the already accepted standard Level 3 exit lifecycle;
- do NOT advance to Level 4;
- do NOT restart Level 1 automatically;
- do NOT invent a campaign cycle.

Preserve established terminal Level 3 behavior unless normal-vs-direct mode requires only a narrow presentation/restart distinction documented by existing conventions.

7. DIRECT DEVELOPMENT MODES REMAIN ISOLATED

Preserve:

--level2-test
--level3-test
--level3-geometry-test
--tanker-test
--spiker-test
--fuseball-test
--pulsar-test

Direct development modes must NOT accidentally start using natural campaign progression.

In particular:

--level2-test must retain its established direct/development terminal behavior unless the C4 authority explicitly says otherwise.

--level3-test remains direct DEVELOPMENT ONLY access to Level3Mixed and remains terminal.

--level3-geometry-test remains Flipper-only and separate.

Normal no-scenario launch is the path that gains Level 2 -> Level 3 progression.

8. GAME OVER / RESTART / REPLAY

Preserve established normal-game restart semantics.

Analyze the existing path while implementing and ensure:

- ordinary death on naturally entered Level 3 stays on the same Level3Mixed configuration and uses accepted C3 reconstruction;
- Game Over restart returns to the proper normal campaign starting state;
- direct --level3-test restart/replay remains unchanged;
- successful natural Level 3 completion remains terminal and does not create Level 4.

Do not redesign input/restart handling.

9. BETWEEN-LEVEL APPROACH PRESENTATION REMAINS DEFERRED

Do NOT implement the Atari-style between-level approach sequence in C4.

The documented observation remains:

- after leaving a tube, the next tube appears distant;
- it grows during approach;
- the small Blaster can move around the distant rim with spinner input;
- enemy engagement begins after arrival.

That presentation remains a future fidelity/presentation milestone.

C4 may continue using the project's existing immediate level-transition presentation.

Do not silently delete or reclassify the deferred observation.

10. C3 PROTECTION BOUNDARY

C4 consumes the existing accepted C3 model.

Do NOT modify or retune:

- LevelConfiguration.Level3Mixed composition;
- Level3WaveSelection;
- Level3WaveLedger semantics;
- Level 3 Egg scheduling;
- six-active-Tanker behavior;
- Tanker firing mechanics;
- Tanker cargo identities/ownership;
- Tanker cargo route/pivot;
- Super Zapper targeting/timer behavior;
- death/reconstruction;
- ordinary Level 3 completion;
- all-rim behavior;
- Level 3 production HUD/rendering policy;
- C1/C1.5 geometry;
- spinner/controller behavior;
- default Flipper firing policy.

If implementing natural progression exposes a genuine C3 defect:

STOP at a clean recoverable point.

Report the exact conflict.

Do not silently repair C3 inside C4 progression work.

11. AUTOMATED C4 TESTS

Add or strengthen deterministic tests for natural progression.

At minimum prove:

A. PROGRESSION RESOLUTION

- Level 1 production successor remains Level2Square;
- Level2Square production successor is Level3Mixed;
- Level3Mixed has no production successor;
- diagnostic/development configurations do not accidentally acquire production successors.

B. NATURAL LEVEL 1 -> LEVEL 2

Existing accepted behavior remains correct.

C. NATURAL LEVEL 2 -> LEVEL 3

Through actual TempestGame progression:

- complete Level 2 successfully;
- successful exit commit enters Level3Mixed;
- displayed level is 3;
- geometry/configuration identity is Level3Mixed;
- score carries;
- lives carry.

D. FRESH LEVEL 3 STATE

Verify natural Level 3 receives fresh C3 state:

- a new Level object/configuration as appropriate;
- fresh Level3WaveLedger;
- correct original identity supply;
- correct fresh Eggs/reserve arrangement;
- valid runtime representation;
- fresh Level 3 scheduling state;
- Super Zapper fresh-entry state;
- no stale Level 2 enemies, Eggs, shots or bullets.

Use existing public/internal test seams.

Do not use reflection.

Do not mutate inaccessible gameplay state solely to manufacture progression.

E. LEVEL 3 TERMINAL BEHAVIOR

- natural Level 3 completion does not progress to any Level 4;
- no unsupported campaign edge exists.

F. DIRECT MODE REGRESSION

- --level2-test remains isolated/terminal according to established behavior;
- --level3-test remains direct Level3Mixed access;
- --level3-geometry-test remains separate and Flipper-only;
- direct modes do not enable natural progression.

G. GAME OVER/RESTART

Protect the established normal restart path after progression changes.

H. C3 REGRESSION

Keep existing C3a/C3b tests green.

Do not duplicate Level3WaveLedger or mixed-wave deterministic infrastructure.

12. TEST DEVELOPMENT PRACTICE

Run new/affected tests individually or in tightly related groups while developing.

Do not immediately run the complete suite after every small edit.

Once the C4 progression tests pass, run a focused progression/C3 regression group covering at minimum:

- LevelProgression;
- LevelConfiguration;
- Level2;
- Level3MixedWave;
- Level3WaveLedger;
- Level3TestMode;
- Level3Geometry;
- Tanker;
- SuperZapper;
- InputSystem/InputDiagnostic where affected;
- Simulation regressions relevant to completion/restart.

Use actual repository test names and valid VSTest/xUnit filter syntax.

13. FULL AUTOMATED GATE

Before manual acceptance run:

dotnet test TempestCSharp.slnx --no-restore

dotnet build TempestCSharp.slnx --no-restore

git diff --check

Then inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git ls-files --others --exclude-standard
git log -1 --oneline

Confirm:

- no Level 4;
- no campaign cycle;
- no between-level approach implementation;
- no Reference/incitti changes;
- no controller/spinner changes;
- no C2 firing changes;
- no geometry/cargo-route changes;
- no C3 identity/lifecycle changes;
- no debug/TEMP artifacts accidentally tracked;
- existing untracked TEMP/ remains excluded.

14. MANUAL C4 ACCEPTANCE HANDOFF

If automated gates pass, STOP before commit and give Neal a concise manual acceptance sequence.

Primary manual run must start NORMAL campaign play, not --level3-test.

Recommend a known starting lives value if supported, for example:

dotnet run --project src/Tempest.Game -- --lives 10

Manual objective:

Verify the natural progression chain:

Level 1 -> Level 2 -> Level 3

without re-performing the entire C3 manual matrix.

Manual checks should include:

LEVEL 1

- normal Level 1 starts;
- score/lives are known;
- optionally lose one or more lives;
- earn score;
- complete Level 1.

LEVEL 2 ENTRY

- Level 2 square starts naturally;
- displayed Level 2 is correct;
- carried score is correct;
- remaining lives carry correctly;
- no obvious regression.

LEVEL 2 COMPLETION

- complete Level 2 naturally;
- successful exit occurs.

LEVEL 3 NATURAL ENTRY

- plus-board Level 3 appears naturally;
- displayed Level is 3;
- score carries from Level 2;
- lives carry from Level 2;
- Level 3 starts with a fresh production mixed wave;
- fresh Eggs/enemies appear normally;
- established Level 3 HUD looks correct;
- Super Zapper fresh-entry state is correct, including two usable charges if that is the established configuration behavior;
- spot-check Flipper/Tanker mixed play and visible Tanker bullets;
- do not require a particular random Tanker count.

LEVEL 3 TERMINAL

If practical:

- complete Level 3;
- confirm it remains terminal and does not enter Level 4.

RESTART

If practical:

- verify Game Over restart returns to a fresh normal Level 1 campaign.

BETWEEN-LEVEL PRESENTATION

The current immediate transition is acceptable for C4.

Do not fail C4 because the Atari distant-tube approach presentation is not implemented.

Record that presentation as still deferred.

Do not require hidden ledger/RNG verification manually.

15. TRACKED DOCUMENTATION

Update documentation narrowly to reflect implemented natural progression.

Likely review:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Do not overstate evidence.

Manual acceptance must occur before promoting newly observed visible behavior.

Keep the Atari between-level approach presentation explicitly deferred.

16. AI WORK RECORD

Preserve the established Tempest AI-development record.

Save this exact implementation prompt under:

AI-Logs/Prompts/

Suggested filename:

2026-11-01-c4-natural-level3-progression-implementation-prompt.md

Create/update:

AI-Logs/Reports/2026-11-01-c4-natural-level3-progression-implementation-report.md

AI-Logs remains ignored by Git.

The implementation report should preserve:

- starting checkpoint;
- scope;
- progression change;
- significant implementation decisions;
- defects/unexpected findings;
- exact test/build results;
- changed files;
- protected-scope inspection;
- manual acceptance still required;
- deferred between-level presentation;
- blockers.

Do not turn the report into a transcript.

Agent Debug logging and the VS Code chat provide detailed execution history.

Do not delete or overwrite historical prompt/report files.

17. END-OF-RUN REPORT

At the end of implementation report only:

- exact files changed;
- LevelProgression change;
- successful-exit commit change;
- natural Level 2 -> Level 3 behavior;
- Level 3 entry-state behavior;
- score/lives carry behavior;
- Super Zapper entry-state result;
- direct-development-mode regression result;
- natural Level 3 terminal behavior;
- tests added/changed;
- focused validation results;
- full solution result;
- build result;
- git diff --check;
- protected/prohibited-scope inspection;
- Reference/incitti status;
- tracked documentation status;
- whether any C3 defect was exposed;
- deferred between-level presentation status;
- exact manual command/checklist;
- any blocker.

Do not declare C4 complete until Neal reports manual acceptance.

Do not commit.

Do not push.

=== REQUIRED PROMPT COMPLETENESS CHECK ===

BEFORE DOING ANY INSPECTION, EDITING, TESTING OR REPOSITORY WORK:

Confirm that this exact completion marker appears at the END of the received prompt:

TEMPEST-C4-NATURAL-PROGRESSION-IMPLEMENTATION-PROMPT-COMPLETE-E76D0B0

If this exact marker is NOT present at the END:

- STOP immediately.
- Do not inspect the repository.
- Do not edit any file.
- Do not run commands.
- Do not run tests.
- Do not reconstruct missing instructions from memory, previous chats, reports or source.
- Respond in the VS Code chat exactly:

STOPPED: The complete C4 implementation prompt was not received. The required end marker is missing.

- Wait for Neal to provide the complete prompt.

If the exact marker IS present at the END, proceed with the implementation above.

TEMPEST-C4-NATURAL-PROGRESSION-IMPLEMENTATION-PROMPT-COMPLETE-E76D0B0

=== END OF PROMPT ===