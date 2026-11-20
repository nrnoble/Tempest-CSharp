FAIL-CLOSED CONTINUATION CHECK

Read this complete continuation before using tools, reading/modifying repository
files, or running repository commands.

Locate the unique standalone TEMPEST_ completion token near the end.

Require exactly one complete token on a line by itself.

If that check fails, STOP before repository action.


TEMPEST SCREENSHOT OWNERSHIP AND USER-DATA PATH CORRECTION

Continue from the existing uncommitted paused-playback screenshot candidate.

Do not restart the screenshot milestone.

Neal has manually tested the candidate.

IMPORTANT MANUAL RESULT:

The actual captured screenshot images look perfect.

The rendered screenshot content does NOT need visual correction.

Preserve the successful capture of:

- the paused Tempest presentation;
- the PAUSED boundary/duration overlay;
- the exact paused boundary.

However, Neal observed an incorrect screenshot ownership/path behavior.


OBSERVED PROBLEM

The console showed files such as:

C:\Users\nrnob\source\repos\Tempest\screenshot000.png
C:\Users\nrnob\source\repos\Tempest\screenshot001.png
C:\Users\nrnob\source\repos\Tempest\screenshot002.png
C:\Users\nrnob\source\repos\Tempest\screenshot003.png

with Raylib messages such as:

INFO: FILEIO: [...] File saved successfully
INFO: FILEIO: [...] Image exported successfully
INFO: SYSTEM: [...] Screenshot taken successfully

These generic screenshotNNN.png files are NOT the intended Tempest-managed
screenshot output.

The candidate was intended to generate one project-owned screenshot using a
boundary-aware filename.

Manual acceptance is therefore NOT complete yet.

Fix this before closeout.


PRIMARY SCREENSHOT REQUIREMENT

Exactly ONE subsystem should own paused-playback screenshots:

TEMPEST.

Desired flow:

paused recorded-file playback
->
Tempest-owned F11 command
->
Tempest chooses destination and unique boundary-aware filename
->
Raylib TakeScreenshot(destination)
->
exactly one PNG.

Do not allow Raylib's independent automatic screenshot shortcut to create
generic screenshotNNN.png files.

Do not alter the actual screenshot rendering/capture ordering that Neal already
judged perfect.


F11 REMAINS ACCEPTED

Neal accepts F11.

The exact screenshot key is not important to Neal.

Avoiding control conflicts and avoiding unintended Windows/background behavior
is more important.

Continue using F11 unless investigation establishes a concrete technical reason
not to.

Do not switch back to Print Screen.

Do not add Windows global keyboard hooks.

Do not use low-level Windows interception.


INVESTIGATE NATIVE RAYLIB SCREENSHOT OWNERSHIP

Inspect the actual installed:

- Raylib-cs 8.1.0 binding;
- native Raylib library;
- startup/configuration API;
- screenshot support.

Determine exactly why generic:

screenshot000.png
screenshot001.png
...

files are being created.

Do not assume.

Investigate whether:

- native Raylib automatic screenshot handling is still enabled;
- its screenshot key can be disabled or changed through the installed binding;
- SetScreenshotKey or an equivalent API is available;
- the project-owned F11 command is firing separately;
- F12/native behavior is involved;
- more than one screenshot path is active.

Report the proven cause.


DISABLE / NEUTRALIZE NATIVE AUTOMATIC SCREENSHOT SHORTCUT

If the installed binding exposes a supported way to disable or safely remap
Raylib's automatic screenshot shortcut, use the smallest supported mechanism.

The end state must be:

- project-owned F11 screenshot works;
- one key press creates exactly one Tempest-managed PNG;
- no generic screenshotNNN.png appears in the current working directory;
- F12 must not create an uncontrolled duplicate/native screenshot if it can be
  safely disabled through supported Raylib configuration.

If native F12 cannot be disabled through the installed supported API without
rebuilding or replacing Raylib, report that limitation explicitly rather than
adding invasive platform-specific code.

Do NOT:

- patch the native DLL;
- rebuild Raylib merely for this milestone;
- introduce Windows keyboard hooks;
- broadly replace Raylib.

Prefer supported configuration.


NEW ACCEPTED USER-DATA LAYOUT

Neal has selected this long-term finished-application layout:

Tempest\
|
|-- Tempest.exe
|-- sfx\
|
`-- UserData\
    |-- GameplayRecordings\
    `-- Screenshots\

This is now the preferred PROJECT DATA-LAYOUT DECISION.


APPLICATION ROOT

"Tempest\" in the layout means the directory containing the executing
Tempest application.

Determine the correct .NET application-base API for locating the directory that
contains the running Tempest executable/application binaries.

Do NOT derive permanent user-data paths from:

- current PowerShell working directory;
- repository root;
- arbitrary relative process launch location.

Use an application-root abstraction appropriate to the current .NET executable.

Document the consequences under:

dotnet run

and under a future standalone/published:

Tempest.exe

Do not guess.

The goal is for the same code to naturally produce:

<application root>\UserData\...

when run as the finished application.


CENTRAL USER-DATA PATH POLICY

Introduce one small maintainable path abstraction if useful.

Conceptually:

ApplicationPaths

TempestUserDataPaths

or another project-consistent name.

It may expose:

ApplicationRoot

UserDataDirectory

GameplayRecordingsDirectory

ScreenshotsDirectory

Do not scatter path construction throughout Program.cs.

Do not create a general filesystem framework.


SCREENSHOT PRODUCTION LOCATION

Change project-owned screenshots from the temporary development location:

Temp\Screenshots\

to the new application user-data location:

<application root>\UserData\Screenshots\

Create UserData and Screenshots on demand.

Preserve:

- PNG output;
- boundary-aware filename;
- displayed level;
- unique suffix;
- no overwrite;
- capture while paused only;
- exact paused presentation;
- PAUSED overlay;
- simulation/cursor preservation.

Example conceptual output:

<application root>\UserData\Screenshots\
    playback-level-7-boundary-004820.png

Repeated capture:

    playback-level-7-boundary-004820-002.png


GAMEPLAY RECORDINGS USER-DATA LOCATION

Establish:

<application root>\UserData\GameplayRecordings\

as the normal long-term home for user-created gameplay recordings.


IMPORTANT BACKWARD-COMPATIBILITY REQUIREMENT

Existing development workflows use explicit paths such as:

Temp\GameplayRecordings\levels1-7.json

and must continue to work.

Do NOT move, migrate, rename, delete, or rewrite Neal's existing recordings.

Do NOT break commands that supply an explicit recording path.


BARE RECORDING FILENAMES

Investigate whether the smallest useful production behavior is:

--record-gameplay levels1-7.json

resolves a bare filename to:

<application root>\UserData\GameplayRecordings\levels1-7.json

and:

--playback-file levels1-7.json

can resolve the same bare filename from:

<application root>\UserData\GameplayRecordings\

Likewise, if appropriate:

--headless-playback levels1-7.json

should resolve the same normal user-data recording.

BUT:

an explicit path such as:

Temp\GameplayRecordings\levels1-7.json

must continue to mean exactly that explicit path.

Do not silently redirect explicit paths.


IMPLEMENTATION AUTHORIZATION FOR RECORDING PATHS

Implement bare-filename UserData resolution ONLY IF it can be done narrowly,
consistently, and without changing explicit-path semantics or recording format.

If current CLI/path architecture makes this materially broader than the
screenshot milestone, then:

- implement the shared UserData path abstraction;
- establish GameplayRecordingsDirectory;
- document it as the target normal application location;
- preserve existing explicit-path CLI behavior;
- defer automatic bare-filename resolution to a separate small follow-up.

Choose the smaller maintainable solution.

Report the decision.

Do not force scope expansion.


DEVELOPMENT TEMP DIRECTORY

Neal's existing:

Temp\GameplayRecordings\

remains a valid ignored location for disposable development recordings.

Do not remove it.

Do not reorganize existing development files.

The distinction is:

Temp\
= disposable repository-development data.

UserData\
= normal application-created persistent user content.


USERDATA AND GIT

Because UserData will normally live relative to the executable/application root,
determine whether any development invocation could place it inside the source
repository.

If a repository-level UserData directory could realistically be created during
development, ensure the repository ignore policy safely excludes generated
UserData content without accidentally ignoring tracked source.

Do not add unnecessary ignore rules if application-base placement means the data
cannot appear at repository root.

Base the decision on actual path behavior.


SCREENSHOT CONFIRMATION

Update the success message to report the actual user-data destination.

Prefer a clear path.

Example:

Screenshot saved:
C:\...\Tempest\UserData\Screenshots\playback-level-7-boundary-004820.png

or an equally clear application-relative path.

Do not leave Neal wondering which screenshot mechanism created the image.


SCREENSHOT OUTPUT VERIFICATION

After correction, precisely distinguish:

PROJECT-OWNED SCREENSHOT:
boundary-aware UserData filename.

UNWANTED NATIVE SCREENSHOT:
generic screenshotNNN.png.

Automated policy tests should ensure the project always requests the former.

Neal will manually verify the latter no longer appears from the accepted
project screenshot key.


EXISTING GENERIC SCREENSHOTS

Do not delete Neal's existing:

screenshot000.png
screenshot001.png
screenshot002.png
screenshot003.png

during this task.

They are user-generated manual-test artifacts.

Do not stage them.

Do not move them.

Neal can remove them manually later if desired.


CAPTURE TIMING

DO NOT modify the successful rendering/capture ordering unless required to fix
the ownership bug.

Neal already reports the screenshots themselves are perfect.

Preserve capture after:

- game render;
- PAUSED overlay render;

at the existing correct point in the drawing lifecycle.


PAUSED-ONLY POLICY

Preserve:

- screenshot active only for paused --playback-file;
- no screenshot during running playback;
- no screenshot during live gameplay;
- no screenshot during headless playback;
- no simulation/cursor movement;
- no gameplay input effect.


PLAYBACK/SCRUBBING FREEZE

Do not alter accepted:

Space

Right tap/hold

Shift+Right tap/hold

Left tap/hold

Shift+Left tap/hold

reverse reconstruction

headless runner

live-input handoff.


NO RECORDING FORMAT CHANGE

Do not add user-data paths or screenshots to recording JSON.

Do not modify:

v1

v2

startup metadata

frame contents

strict validation.


TESTS — USER DATA PATHS

Add focused tests for:

- application-root calculation through the chosen abstraction;
- UserData child path;
- GameplayRecordings child path;
- Screenshots child path;
- path construction independent of current working directory where practical;
- directory creation only when needed.


TESTS — SCREENSHOT POLICY

Preserve/extend tests proving:

- F11 only active during paused file playback;
- one project capture request per accepted F11 press;
- running playback does not request capture;
- live gameplay does not request capture;
- boundary/cursor unchanged;
- filename includes boundary;
- duplicate filename suffixing;
- no overwrite;
- selected screenshot output is under UserData\Screenshots;
- screenshot command remains outside PlayerInput.


TESTS — RECORDING PATH POLICY

If bare-filename resolution is implemented, test:

- bare recording output name maps to UserData\GameplayRecordings;
- bare playback name maps there;
- bare headless name maps there if supported;
- explicit Temp\GameplayRecordings path remains explicit;
- absolute path remains absolute;
- no recording format or save semantics change.

If bare resolution is deferred, state so and do not add fake tests.


TESTS — RAYLIB SCREENSHOT CONFLICT

Where practical through a testable seam/configuration:

verify the application configures native screenshot behavior so the project-owned
F11 path is the only intended application screenshot route.

Do not write a brittle unit test against undocumented native internals.

Manual verification will remain authoritative for absence of generic
screenshotNNN.png files.


REGRESSION VALIDATION

Run focused screenshot/UserData tests.

Run neighboring playback/input/rendering tests.

Run Phase 1/2/3 playback regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Require:

0 failed
0 unexpected skipped
build 0 errors

Report warnings accurately.


DOCUMENTATION

Update current screenshot candidate documentation.

Record:

- screenshot images themselves manually look perfect;
- manual acceptance remains pending because duplicate/native path issue was
  found;
- F11 remains accepted;
- conflict avoidance takes priority over specific screenshot key;
- proven Raylib native screenshot cause;
- native screenshot configuration/correction;
- UserData layout;
- application-root definition;
- Screenshots production location;
- GameplayRecordings production location;
- Temp development role;
- explicit recording paths remain compatible;
- bare-filename behavior if implemented;
- no recording-format change;
- no gameplay/playback semantics change.

Do not mark final acceptance until Neal retests.


AI LOGS

Preserve this continuation according to repository logging policy.

Suggested report:

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-userdata-correction-report.md

Record:

1. Starting Git/worktree state.
2. Neal manual screenshot result.
3. Generic screenshotNNN problem.
4. Proven native Raylib cause.
5. Raylib configuration fix.
6. F11 decision.
7. Capture path.
8. UserData layout.
9. Application-root implementation.
10. Screenshots path.
11. GameplayRecordings path.
12. Development Temp policy.
13. Bare filename decision.
14. Explicit-path compatibility.
15. Git-ignore decision.
16. Existing screenshot artifact preservation.
17. Simulation/cursor preservation.
18. PlayerInput separation.
19. Recording-format preservation.
20. Production files changed.
21. Test files changed.
22. Documentation changed.
23. Focused tests.
24. Neighboring regression.
25. Full suite.
26. Build warnings/errors.
27. git diff --check.
28. Manual retest command/checklist.
29. Final Git state.


NO INTERACTIVE LAUNCH

Copilot must not launch the game.

Neal performs final screenshot acceptance.

Do not stage.
Do not commit.
Do not push.


MANUAL RETEST

Provide:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json

Ask Neal to:

1. pause with Space;
2. scrub to an interesting boundary;
3. press F11 once;
4. verify exactly one Tempest-owned screenshot is created;
5. verify its image remains perfect;
6. verify it includes PAUSED boundary information;
7. verify the boundary did not change;
8. verify filename identifies level/boundary;
9. verify file is under the new UserData\Screenshots location;
10. verify no new generic screenshotNNN.png appears in the working directory;
11. press F11 again at the same boundary;
12. verify a unique suffix and no overwrite;
13. continue scrubbing normally;
14. verify F11 while running does nothing;
15. verify existing Temp\GameplayRecordings commands still work.


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

Expected committed HEAD remains:

50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50

Require:

- corrected screenshot candidate unstaged;
- no commit;
- no push.


FINAL RESPONSE

Return:

1. Proven cause of generic screenshotNNN files.
2. Native Raylib screenshot disposition.
3. Selected project screenshot key.
4. Project screenshot capture path.
5. UserData directory design.
6. Application-root definition.
7. Screenshots directory.
8. GameplayRecordings directory.
9. Development Temp policy.
10. Bare-filename recording decision.
11. Explicit-path compatibility.
12. Screenshot filename behavior.
13. Duplicate policy.
14. Screenshot manual result preserved.
15. Simulation/cursor preservation.
16. PlayerInput separation.
17. Recording format preservation.
18. Production files changed.
19. Tests changed.
20. Documentation changed.
21. Focused test counts.
22. Neighboring regression counts.
23. Full-suite counts.
24. Build warnings/errors.
25. git diff --check.
26. Manual retest checklist.
27. Report filename.
28. Final Git status.
29. Confirmation no gameplay change, recording-format change, snapshot,
    enemy-explosion implementation, Level 8/9, stage, commit, or push occurred.

Stop for Neal's final manual acceptance.

TEMPEST_SCREENSHOT_USERDATA_CORRECTION_COMPLETE