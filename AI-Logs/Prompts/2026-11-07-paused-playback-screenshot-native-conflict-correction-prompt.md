FAIL-CLOSED CONTINUATION CHECK

Read this complete continuation before any repository action.

Require exactly one complete standalone TEMPEST_ token on a line by itself near
the end.

If that check fails, STOP before repository action.


TEMPEST SCREENSHOT NATIVE-CAPTURE CONFLICT — SECOND CORRECTION

Continue from the current uncommitted screenshot/UserData candidate.

Do not restart the milestone.

Do not stage.
Do not commit.
Do not push.


NEW MANUAL RESULT

The problem remains.

While Neal tested the corrected candidate, Raylib continued creating generic
working-directory screenshots:

C:\Users\nrnob\source\repos\Tempest\screenshot002.png
C:\Users\nrnob\source\repos\Tempest\screenshot003.png
C:\Users\nrnob\source\repos\Tempest\screenshot004.png
C:\Users\nrnob\source\repos\Tempest\screenshot005.png

Raylib reported:

INFO: FILEIO: [...] File saved successfully
INFO: FILEIO: [...] Image exported successfully
INFO: SYSTEM: [...] Screenshot taken successfully

Therefore the prior practical conclusion that project-owned F11 would avoid the
native screenshot path is disproven by actual manual testing.

Do not mark this milestone accepted.

The screenshot images themselves remain visually perfect.

Do not change screenshot rendering or capture ordering unless necessary to
resolve this conflict.


PRIMARY GOAL

Determine exactly why Neal's accepted screenshot interaction still triggers
Raylib's native:

screenshotNNN.png

capture path.

Then make the smallest safe correction so that one accepted Tempest screenshot
command creates exactly one Tempest-managed screenshot under:

<application root>\UserData\Screenshots\

and creates no generic screenshotNNN.png in the process working directory.


VERIFY THE ACTUAL MANUAL PATH

Do not assume which physical/logical key Raylib receives.

Instrument or expose a narrowly bounded development diagnostic capable of
establishing, for one screenshot attempt:

- whether Tempest observed F11;
- whether Tempest observed F12;
- whether the project-owned capture request was created;
- the exact project-owned destination;
- whether the native generic screenshot appeared;
- whether one physical key press is being surfaced as more than one logical
  key event.

Do not log every keyboard event continuously.

Do not leave noisy permanent diagnostics after resolving the issue.

A temporary bounded diagnostic is acceptable if removed or converted into a
maintainable development-only seam before completion.


CHECK THE KEYBOARD / RAYLIB MAPPING

Investigate whether Neal's physical F11 press is being surfaced to Raylib as:

- F11;
- F12;
- both F11 and F12;
- another mapped function-key event.

Consider that keyboard firmware, Fn-lock behavior, Windows handling, GLFW, or
Raylib may affect the logical key.

Do not attribute the cause without evidence.


VERIFY PROJECT-OWNED OUTPUT

Determine whether the same physical press also created the intended file under:

<application root>\UserData\Screenshots\

Report both outcomes separately:

1. Tempest-managed boundary-aware screenshot.
2. Raylib-native screenshotNNN.png.

Do not treat successful native capture as proof that the project capture worked.


NATIVE RAYLIB LIMITATION

The previous investigation found:

- native Raylib 6 handles F12 automatically;
- the installed Raylib-cs binding exposes no SetScreenshotKey;
- disabling native capture would require replacing/rebuilding the native
  library or platform interception.

Re-verify this against the actual loaded assembly/native DLL if necessary.

Do not rebuild or replace Raylib in this correction unless Neal separately
authorizes that larger dependency change.

Do not add a Windows global keyboard hook.


KEY-SELECTION POLICY

The specific key is not important to Neal.

Conflict avoidance is the priority.

If actual evidence shows the physical F11 path is reaching native F12 handling,
select another simple, conflict-free development key.

Good candidates may include:

F10
F9
or another otherwise-unused key proven not to invoke OS/Raylib behavior.

Inspect all existing Tempest input mappings first.

Do not select a key that conflicts with:

- gameplay;
- playback scrubbing;
- Enter quit;
- restart;
- Super Zapper;
- Raylib native capture/GIF features;
- Windows global behavior.

Prefer a single function key with press-edge behavior.


DO NOT FIGHT WINDOWS OR RAYLIB

Do not attempt to suppress native behavior through:

- global hooks;
- raw Windows input;
- DLL patching;
- native-library replacement;
- keyboard-driver assumptions.

If choosing a different nonconflicting project key completely avoids the native
path, that is the preferred solution.


ONE PRESS, ONE PROJECT CAPTURE

After correction, prove through the project-owned control seam:

- one accepted key press creates one capture request;
- no key-repeat capture;
- no capture while running;
- no capture during live gameplay;
- no PlayerInput;
- no boundary/cursor advancement.

Manual testing remains authoritative for proving no generic native PNG appears.


USERDATA POLICY

Preserve the accepted application layout:

Tempest\
|
|-- Tempest.exe
|-- sfx\
|
`-- UserData\
    |-- GameplayRecordings\
    `-- Screenshots\

Preserve:

- AppContext.BaseDirectory application root;
- UserData\Screenshots;
- UserData\GameplayRecordings;
- explicit Temp\GameplayRecordings paths;
- bare-filename resolution policy;
- boundary-aware filenames;
- collision-safe suffixes;
- no overwrite.

Do not reopen or redesign the UserData work.


EXISTING GENERIC SCREENSHOTS

Do not delete, move, rename, or stage:

screenshot000.png
screenshot001.png
screenshot002.png
screenshot003.png
screenshot004.png
screenshot005.png

They remain Neal's manual-test artifacts.


TESTS

Add or update focused tests for the finally selected key:

- accepted key produces one project capture request;
- held/repeat input does not produce repeated screenshots;
- F11 behavior reflects the final decision;
- F12 is not used by Tempest;
- selected key remains separate from PlayerInput;
- paused-only policy;
- running/live/headless paths do not capture;
- boundary/cursor remain unchanged;
- UserData destination remains correct;
- duplicate suffix behavior remains correct.

Do not claim unit tests prove native Raylib does not create screenshotNNN.png.

That requires Neal's manual test.


REGRESSION VALIDATION

Run focused screenshot/UserData tests.

Run neighboring playback/input/rendering regressions.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Require:

- 0 failures;
- 0 unexpected skips;
- build 0 errors;
- warnings reported accurately;
- diff check passes.


DOCUMENTATION

Update the candidate documentation and correction report with:

- the second manual failure;
- the disproven F11/no-conflict assumption;
- the proven actual key mapping/cause;
- the final selected key;
- why that key avoids native/OS conflicts;
- project-owned screenshot destination;
- native Raylib limitation;
- manual acceptance still pending.

Do not describe the milestone as accepted.


AI LOGS

Preserve this continuation according to repository policy.

Create or update an appropriately paired report, preferably:

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-native-conflict-correction-report.md

Include:

1. Starting worktree.
2. Second manual failure.
3. Key-event evidence.
4. Project-owned output evidence.
5. Native output evidence.
6. Proven cause.
7. Final selected key.
8. Native-Raylib limitation.
9. UserData preservation.
10. Production changes.
11. Test changes.
12. Documentation changes.
13. Focused counts.
14. Neighboring counts.
15. Full-suite count.
16. Build result.
17. diff check.
18. Manual retest instructions.
19. Final Git state.


NO INTERACTIVE LAUNCH

Do not launch the game.

Neal performs manual testing.

Do not stage.
Do not commit.
Do not push.


MANUAL RETEST

Provide the actual selected key and run:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json

Ask Neal to:

1. Note the highest existing generic screenshotNNN.png number.
2. Launch visible playback.
3. Pause with Space.
4. Scrub to an identifiable boundary.
5. Press the newly selected capture key exactly once.
6. Verify exactly one boundary-aware PNG appears under the application's
   UserData\Screenshots directory.
7. Open it and verify the already accepted perfect image and PAUSED overlay.
8. Verify no new generic screenshotNNN.png appears in the repository working
   directory.
9. Press the key again at the same boundary.
10. Verify one unique-suffix UserData screenshot is added.
11. Verify no generic screenshot is added.
12. Verify boundary/cursor remain unchanged.
13. Verify capture key does nothing while playback runs.
14. Verify scrubbing remains correct.


FINAL GIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

HEAD must remain:

50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50

Require candidate unstaged and uncommitted.


FINAL RESPONSE

Return:

1. Proven physical/logical key behavior.
2. Proven cause of generic screenshot creation.
3. Whether project-owned screenshot also fired.
4. Final selected key.
5. Native Raylib limitation.
6. Project screenshot destination.
7. UserData preservation.
8. Simulation/cursor preservation.
9. PlayerInput separation.
10. Production files changed.
11. Tests changed.
12. Documentation changed.
13. Focused count.
14. Neighboring count.
15. Full-suite count.
16. Build warnings/errors.
17. diff check.
18. Manual retest checklist.
19. Report filename.
20. Final Git status.
21. Confirmation no gameplay change, recording-format change, snapshot,
    enemy-explosion implementation, Level 8/9, stage, commit, or push occurred.

Stop for Neal's manual acceptance.

TEMPEST_SCREENSHOT_NATIVE_CONFLICT_SECOND_CORRECTION_COMPLETE