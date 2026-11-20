FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information may appear after the user's completion
token and do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST GENERIC DIRECT-LEVEL LAUNCH AND CONTROLLER EXIT CONTROL

Implement one small development-infrastructure/control milestone before work
begins on production Levels 8 and 9.

This milestone has two goals:

1. Add a generic command-line option that starts the game directly at any
   currently implemented production level.

2. Change the controller-friendly immediate-exit key from V to Enter.

Do not implement Level 8 or Level 9.

Do not change established gameplay behavior of Levels 1–7.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest

Do not use or modify the frozen former OneDrive copy.


EXPECTED BASELINE

Expected branch:

master

Expected HEAD:

8178429121febe5d543fcd16e546be0b75025c69
Correct Level 5 Flipper Spike behavior

Expected:

- tracked worktree clean;
- index clean;
- no unexplained untracked files;
- master tracking origin/master;
- local master and origin/master at the accepted Level 5 checkpoint.

Before changing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v
git rev-parse origin/master

If the starting repository differs unexpectedly, STOP rather than resetting,
cleaning, restoring, deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Read the accepted Level 5 closeout report and any existing reports concerning:

- direct Level 4 test/startup;
- direct Level 7 test/startup;
- scenario-selectable deterministic playback;
- Program.cs command-line parsing;
- DebugController development switches;
- production level initialization;
- progression;
- input diagnostics;
- controller keyboard compatibility.

Inspect current:

- Program.cs;
- DebugController.cs;
- TempestGame.cs;
- LevelConfiguration.cs;
- level initialization/progression code;
- input handling;
- relevant tests.

Understand and reuse the existing production initialization seams rather than
creating a parallel level implementation.


PART 1 — GENERIC DIRECT-LEVEL LAUNCH

Add a DEVELOPMENT-ONLY command-line option:

--level N

where N is the displayed production level number.

Examples:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

dotnet run --project src\Tempest.Game -- --level 7 --lives 10


SUPPORTED LEVELS

For the current production build, valid direct levels are exactly the currently
implemented production levels:

1 through 7.

Do not add placeholder Level 8 or Level 9 support.

Do not silently clamp invalid values.

Reject:

--level 0

--level -1

--level 8

--level 999

missing value

non-numeric value

with a clear command-line error and non-successful startup as appropriate to
the project's existing argument-error behavior.


DEFAULT BEHAVIOR MUST REMAIN UNCHANGED

When --level is omitted:

the game must start exactly as it does today at normal Level 1.

Do not alter ordinary player-facing startup behavior.


DIRECT LEVEL MUST BE THE REAL PRODUCTION LEVEL

--level N must initialize the same production level configuration/state that
natural campaign progression would eventually reach.

Do not create:

- test-only geometry;
- duplicate enemy configurations;
- alternate Level 5;
- alternate Level 7;
- special simplified enemy supply;
- a parallel test level implementation.

The purpose is only to bypass playing earlier accepted levels to reach the
desired production level.


FRESH-GAME SEMANTICS

A direct-level launch is a DEVELOPMENT convenience representing a fresh game
starting at the selected level.

Use fresh-game state consistent with existing direct-level testing conventions
and production initialization.

At minimum ensure:

- selected displayed level is correct;
- selected tube geometry/topology is correct;
- selected palette/board state is correct;
- selected level enemy supply/eligibility is correct;
- selected starting Spikes are correct;
- selected level-specific Flipper behavior is correct;
- Tanker/cargo configuration is correct;
- Spiker/Pulsar/Fuseball eligibility is correct where applicable;
- level firing parameters are correct;
- lives use the requested --lives value, or normal default when omitted;
- score begins as a fresh-game score;
- Super Zapper starts in the correct fresh-level state;
- no stale enemies, shots, animation, death, exit, approach, ledger, replay, or
  previous-level state leaks into the selected level.

Reuse the production level initialization path wherever possible.


NO ARTIFICIAL TERMINAL BOUNDARY

Direct-level mode must NOT stop after the requested level.

Example:

--level 5

starts at production Level 5.

If Neal completes Level 5 normally, the game should proceed naturally through
the existing production transition to Level 6.

Likewise:

--level 6

should naturally progress to Level 7 after completion.

Do not create:

- "Level 5 complete, press R";
- direct-level terminal mode;
- test-only completion screen;
- forced exit after the selected level.

The purpose is to choose the STARTING level only.


NORMAL PROGRESSION AFTER STARTUP

After direct initialization, use ordinary production progression.

This is important because the diagnostic should eventually permit tests such as:

--level 8

followed by naturally observing:

Level 8 -> Level 9

once those production levels exist.

Do not implement Levels 8 or 9 now.


INTERACTION WITH --lives

Support normal combinations such as:

--level 5 --lives 10

--lives 10 --level 5

Argument order should not alter semantics unless the existing argument parser
has an intentional ordering rule.

Retain normal validation for --lives.


INTERACTION WITH EXISTING DEVELOPMENT SWITCHES

Audit existing level-specific switches such as any:

--level4-test

--level7-test

or similar direct-start diagnostics.

Do not casually remove working diagnostics in this milestone.

Prefer making --level N the generic forward-looking direct-level mechanism.

If an old switch can safely delegate to the same underlying production startup
seam without changing its documented behavior, that is acceptable.

Do not create unnecessary duplicate initialization paths.

Document any legacy diagnostic that remains and why.


PART 2 — CONTROLLER-FRIENDLY IMMEDIATE EXIT

The current project documentation records V as an additive immediate quit alias.

That behavior is no longer desired.

One physical button on Neal's arcade controller emits ordinary keyboard V.

Therefore:

V MUST NO LONGER EXIT THE GAME.


ENTER BECOMES THE CONTROLLER-FRIENDLY QUIT KEY

Pressing ordinary Enter must immediately exit the running game.

This should behave like the current V immediate-exit path behaves.

The goal is explicitly:

Neal should be able to conduct a manual playtest entirely from the arcade
controller and quit the C# game from the controller without reaching for the
computer keyboard.

One controller button emits Enter and is the preferred quit button.


NO CONFIRMATION

Enter should immediately request/perform the normal clean game exit.

Do not add:

- confirmation dialog;
- "Are you sure?";
- hold-to-exit;
- double-press requirement;
- pause menu;
- delay;
- new quit screen.

Immediate exit is intentional.


PRESERVE OTHER EXIT METHODS

Preserve:

- Escape's existing normal Raylib/application quit behavior;
- window close-button behavior;
- application cleanup;
- cursor restoration/release;
- audio/window cleanup;
- any normal shutdown semantics.

Enter is additive to those ordinary exit methods.


V MUST BECOME INERT WITH RESPECT TO QUITTING

Pressing V must not:

- exit;
- restart;
- pause;
- zap;
- fire;
- start a level;
- trigger another gameplay action merely as compensation for removing quit.

Do not assign V a new action.

Other existing controller mappings must remain unchanged.


ENTER MUST NOT ACQUIRE OTHER GAMEPLAY SEMANTICS

Audit whether Enter currently has any gameplay or diagnostic action.

If Enter is presently unused in production gameplay, map it to immediate quit.

If the repository contains diagnostic-only observations of Enter, preserve the
input diagnostic's ability to report the physical key event while making
production Enter perform immediate quit.

Do not make Enter simultaneously trigger another gameplay action before exit.


INPUT DIAGNOSTIC

Preserve the DEVELOPMENT-ONLY input diagnostic.

If it currently logs V and Enter keyboard events, keep those observations useful.

Production mappings and diagnostic event observation are separate concerns.


TESTS — DIRECT LEVEL STARTUP

Add focused automated coverage for at least:

1. omitted --level retains ordinary Level 1 startup;

2. --level 1 starts real production Level 1;

3. --level 4 starts real production Level 4;

4. --level 5 starts the accepted production Level 5, including:
   - correct geometry/configuration;
   - starting Spikes;
   - accepted $53 Spike-sensitive Flipper policy;
   - appropriate enemy supply/eligibility;
   - fresh Zapper state;

5. --level 6 starts real production Level 6;

6. --level 7 starts accepted production Level 7, including:
   - correct geometry;
   - starting Spikes;
   - correct supply/eligibility;
   - accepted 2-3-2-2-2 ordinary route;
   - appropriate cargo behavior;
   - fresh Zapper state;

7. --level N honors --lives;

8. fresh score/state semantics are correct;

9. no previous-level state leaks into direct startup;

10. invalid levels are rejected;

11. missing/malformed values are rejected.


TESTS — PROGRESSION

Protect natural onward progression after direct startup.

At minimum cover:

--level 4:
Level 4 can naturally progress to production Level 5.

--level 5:
Level 5 can naturally progress to production Level 6.

--level 6:
Level 6 can naturally progress to production Level 7.

Do not create a terminal boundary for direct-level startup.

Preserve the currently established terminal behavior after the highest
implemented production level if Level 7 is still the current campaign boundary.


TESTS — EXIT CONTROLS

Add focused coverage demonstrating:

1. V no longer requests application quit.

2. Enter requests immediate application quit.

3. Enter uses the same clean exit path expected for the former V alias.

4. Enter does not mutate game state before shutdown beyond normal shutdown
   handling.

5. Escape/window-close behavior is not changed by production code modifications.

6. Other controller mappings remain unchanged.

7. Input diagnostic behavior still reports relevant raw keyboard events where
   already supported.


DO NOT CHANGE GAMEPLAY

This is infrastructure/control work.

Do not alter:

- Level 1–7 geometry;
- enemies;
- supply;
- Level 5 $53 correction;
- Level 7 2-3-2-2-2 route;
- Level 7 Tanker cargo;
- Spike mechanics;
- rim speed;
- --rim-speed;
- firing;
- shots;
- Super Zapper;
- ordinary spinner behavior;
- scoring;
- death/re-entry;
- Game Over;
- approaches/exits;
- audio;
- palettes.

No Level 8/9 production work.


DOCUMENTATION

Update durable documentation to record:

1. Generic DEVELOPMENT-ONLY direct startup:

   --level N

2. Current supported range:

   Levels 1–7.

3. Direct launch selects the starting production level only.

4. Normal campaign progression continues after that level.

5. This exists to make later level-by-level manual arcade testing practical.

6. V is no longer a quit alias because a physical controller button emits V.

7. Enter is now the controller-friendly immediate quit alias.

8. Escape/window-close behavior remains.

9. The goal is to let Neal perform and exit manual gameplay tests from the
   arcade controller without reaching for the keyboard.

Update prior durable text that says V is a quit alias so the documentation does
not contradict current behavior.


FUTURE LEVEL WORKFLOW

Preserve the agreed development workflow already documented:

analyze one level completely
-> implement that production level
-> automated validation
-> direct-launch/manual arcade testing
-> correct discrepancies
-> manual acceptance
-> document
-> commit/push
-> proceed to next level

The generic --level N option is infrastructure supporting this workflow.

Do not perform Level 8 analysis or implementation now.


AI LOGS

Save paired ignored records:

AI-Logs/Prompts/2026-11-06-generic-direct-level-and-enter-quit-prompt.md

AI-Logs/Reports/2026-11-06-generic-direct-level-and-enter-quit-report.md

The report must include:

1. Starting Git state.
2. Existing direct-level mechanisms found.
3. Generic --level N design.
4. Production initialization seam reused.
5. Fresh-game state semantics.
6. Supported range and error handling.
7. Natural progression behavior.
8. Existing V behavior found.
9. Enter quit implementation.
10. V removal.
11. Other exit behavior preservation.
12. Input diagnostic preservation.
13. Production files changed.
14. Test files changed.
15. Documentation changed.
16. Focused test results.
17. Progression/regression results.
18. Full-suite/build/diff-check results.
19. Manual test commands.
20. Final Git state.


AUTOMATED VALIDATION

Run focused command-line/direct-level/input-control tests.

Run progression tests covering direct-start onward transitions.

Run neighboring regression covering:

- Levels 1–7;
- Level 5 $53 behavior;
- Level 7 movement/cargo;
- campaign progression/soak;
- death/re-entry;
- Super Zapper;
- rim pursuit;
- replay/playback if affected by startup composition;
- input diagnostics and production controls.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report build warning count accurately;
- git diff --check passes.


NO INTERACTIVE LAUNCH BY COPILOT

Do not launch the interactive game.

Stop after automated validation.

Do not stage.
Do not commit.
Do not push.

Neal will manually test the infrastructure first.


MANUAL TEST COMMANDS

Provide ready-to-copy commands including:

NORMAL START:

dotnet run --project src\Tempest.Game -- --lives 10

DIRECT LEVEL 1:

dotnet run --project src\Tempest.Game -- --level 1 --lives 10

DIRECT LEVEL 4:

dotnet run --project src\Tempest.Game -- --level 4 --lives 10

DIRECT LEVEL 5:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

DIRECT LEVEL 6:

dotnet run --project src\Tempest.Game -- --level 6 --lives 10

DIRECT LEVEL 7:

dotnet run --project src\Tempest.Game -- --level 7 --lives 10


MANUAL ACCEPTANCE CHECKLIST

Ask Neal to verify:

1. Normal launch still starts Level 1.

2. --level 5 immediately produces the accepted production Level 5 rather than
   requiring Levels 1–4.

3. Direct Level 5 visually/gameplay-wise matches naturally reached Level 5.

4. Completing directly started Level 5 proceeds normally into Level 6.

5. --level 7 immediately produces the accepted production Level 7.

6. Direct Level 7 looks and plays like naturally reached Level 7.

7. Requested --lives value is honored.

8. V no longer exits the game.

9. V does not trigger another gameplay action.

10. Enter immediately exits the game.

11. The controller button mapped to Enter therefore exits without requiring the
    keyboard.

12. Escape and window-close behavior remain normal.

13. Spinner, Fire, Super Zapper and other controller buttons remain correct.

14. No obvious gameplay regression is visible.


GIT

Do not stage.
Do not commit.
Do not push.

Finish with:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

HEAD must remain:

8178429121febe5d543fcd16e546be0b75025c69


FINAL RESPONSE

Return:

1. Exact --level N syntax.
2. Supported level range.
3. Production initialization path reused.
4. Fresh-game semantics.
5. Normal progression behavior.
6. Invalid-option behavior.
7. V-key behavior.
8. Enter-key behavior.
9. Escape/window-close preservation.
10. Input-diagnostic behavior.
11. Production files changed.
12. Tests changed.
13. Documentation changed.
14. Focused test counts.
15. Progression/regression counts.
16. Full-suite counts.
17. Build warnings/errors.
18. git diff --check result.
19. Ready-to-copy manual commands.
20. Manual acceptance checklist.
21. Report filename.
22. Final Git status.
23. Confirmation no Level 8/9, gameplay retuning, staging, commit or push
    occurred.

Stop for Neal's manual infrastructure/control acceptance.

TEMPEST_GENERIC_DIRECT_LEVEL_ENTER_QUIT_COMPLETE