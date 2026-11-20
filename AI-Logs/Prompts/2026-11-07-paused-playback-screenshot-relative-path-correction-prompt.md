FAIL-CLOSED CONTINUATION CHECK

Read this complete continuation before using tools, reading or modifying files,
running commands, or taking any repository action.

Locate the unique standalone TEMPEST_ completion token near the end.

Require exactly one complete token on a line by itself.

If the check fails, STOP before repository action.


TEMPEST SCREENSHOT CANDIDATE — RAYLIB PATH-HANDOFF CORRECTION

Continue from the current uncommitted paused-playback screenshot/UserData
candidate.

Do not restart the milestone.

This correction is limited to the now-proven Raylib screenshot pathname defect
and the simplified development UserData root selected by Neal.

Do not change the accepted screenshot chord.

Do not redesign screenshot rendering.

Do not stage.
Do not commit.
Do not push.
Do not launch the game.

Neal performs interactive testing.


CLEAN COMMITTED BASELINE

The last accepted committed checkpoint remains:

50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50
Add reverse recorded playback scrubbing

The entire screenshot/UserData effort remains uncommitted.

If this isolated path correction does not create one valid project-owned PNG in
the requested location, STOP and recommend abandoning the entire uncommitted
screenshot milestone.

Do not perform an automatic rollback.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


STARTING SAFETY CHECK

Before editing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard
git remote -v

Require:

- current branch master;
- HEAD and origin/master both remain
  50be0b88c9f8f2f1ac9229db54dc0cfa7a292f50;
- clean index;
- only the expected accumulated screenshot/UserData candidate changes and known
  local screenshot artifacts are present.

Known generic screenshot artifacts may include:

screenshot000.png
screenshot001.png
screenshot002.png
screenshot003.png
screenshot004.png
screenshot005.png
screenshot006.png

Do not delete, move, rename, alter, or stage these files.

If unexpected production, test, documentation, reference, configuration, or
recording changes are present, STOP.


READ FIRST

Read:

.github/copilot-instructions.md
README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Read the accumulated screenshot reports, including:

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-report.md

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-userdata-correction-report.md

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-native-conflict-correction-report.md

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-chord-correction-report.md

If a report filename differs, locate the corresponding report rather than
assuming it is missing.

Inspect current versions and complete diffs for:

src/Tempest.Game/Program.cs

src/Tempest.Game/ApplicationPaths.cs

src/Tempest.Game/Debug/PlaybackScreenshotCapture.cs

src/Tempest.Game/Systems/InputSystem.cs

src/Tempest.Game/Rendering/VectorRenderer.cs

tests/Tempest.Game.Tests/ApplicationPathsTests.cs

tests/Tempest.Game.Tests/PlaybackScreenshotCaptureTests.cs

tests/Tempest.Game.Tests/InputSystemTests.cs

and all other files in the cumulative screenshot candidate.

Current source is authoritative.


DECISIVE LATEST MANUAL EVIDENCE

Neal ran:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic

The diagnostic reported:

Control observed=True
Shift observed=True
S press edge observed=True (1)
F12 press edge observed=False (0)

Project capture request created: True.

Project capture destination:

C:\Users\nrnob\source\repos\Tempest\
src\Tempest.Game\bin\Debug\net9.0\UserData\Screenshots\
playback-level-2-boundary-000988.png

Project screenshot file exists: False.

New or overwritten native screenshotNNN.png files since diagnostic start:

none observed.


WHAT THIS PROVES

The following is now established:

1. CTRL+SHIFT+S is detected correctly.
2. The Tempest project-owned screenshot request is created.
3. F12 is not detected.
4. Raylib native screenshotNNN behavior is not triggered by the chord.
5. The key-conflict problem is solved.
6. The project currently passes an absolute Windows screenshot path to Raylib.
7. Raylib transforms that input into an invalid combined pathname.

Raylib logged an attempted path conceptually equivalent to:

C:\Users\nrnob\source\repos\Tempest/
C:\Users\nrnob\source\repos\Tempest\
src\Tempest.Game\bin\Debug\net9.0\UserData\Screenshots\...

Raylib then reported:

Failed to open file
Failed to export image
Screenshot could not be saved

Do not reopen key selection.

Do not change CTRL+SHIFT+S.

Do not investigate F9, F10, F11, F12, or Print Screen further.


DEFINITIVE SCREENSHOT COMMAND

The only Tempest-owned screenshot command is:

CTRL + SHIFT + S

while visible recorded-file playback is paused.

Preserve:

- left or right Control;
- left or right Shift;
- S press edge;
- no key-repeat captures;
- no capture while running;
- no capture during live gameplay;
- no capture during headless playback;
- no PlayerInput;
- no recording event.


PROVEN PATH DEFECT

The defect is the pathname supplied to Raylib.TakeScreenshot.

The installed Raylib 6 behavior treats the supplied string as a filename/path
relative to the current working directory and mishandles the absolute Windows
pathname by prefixing the current working directory.

Correct this at the Tempest/Raylib seam.

Do not treat this as:

- a permissions defect;
- missing PNG support;
- a keyboard defect;
- a playback defect;
- a renderer defect;
- an absent-directory defect unless source inspection proves otherwise.


SIMPLIFIED DEVELOPMENT USERDATA LOCATION

Neal has explicitly selected the following current development layout:

C:\Users\nrnob\source\repos\Tempest\
|
|-- Temp\
|   `-- GameplayRecordings\
|
|-- UserData\
|   |-- GameplayRecordings\
|   `-- Screenshots\
|
|-- src\
|-- tests\
`-- ...

For the current development milestone:

UserData must be created at the same filesystem level as Temp.

Therefore, with the present working directory:

C:\Users\nrnob\source\repos\Tempest

the intended screenshot directory is:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\

NOT:

C:\Users\nrnob\source\repos\Tempest\
src\Tempest.Game\bin\Debug\net9.0\UserData\Screenshots\


DEVELOPMENT ROOT DECISION

For this current development tool, define the development data root from the
process current working directory because Neal deliberately launches the project
from the Tempest repository root and wants UserData beside Temp.

Use the appropriate .NET current-working-directory API.

Keep the distinction explicit:

DEVELOPMENT ROOT:
current process working directory for this repository workflow.

APPLICATION BINARY ROOT:
AppContext.BaseDirectory.

Do not describe these as equivalent.

Do not continue routing current development screenshots through
AppContext.BaseDirectory.


FUTURE STANDALONE LAYOUT

Retain this as the intended future packaged layout:

Tempest\
|
|-- Tempest.exe
|-- sfx\
|
`-- UserData\
    |-- GameplayRecordings\
    `-- Screenshots\

Do not attempt to solve published-installation root detection in this correction.

Document that packaging/deployment can later provide the application data root
appropriate to the standalone build.

The immediate milestone uses the repository working directory because that is
the explicit current development decision.


PATH ABSTRACTION

Keep or revise the small path abstraction so it clearly models the selected
current policy.

Possible concepts:

DevelopmentRoot

UserDataRoot

ScreenshotsDirectory

GameplayRecordingsDirectory

Do not leave a misleading property named ApplicationRoot if it actually means
the process working directory.

Prefer dependency injection or an explicit root parameter in low-level path
tests so behavior can be tested without changing the actual process working
directory globally.

Do not create a general filesystem framework.


RAYLIB PATH-HANDOFF RULE

.NET may use absolute paths internally for:

- calculating the intended destination;
- creating UserData\Screenshots;
- detecting collisions;
- checking whether output exists;
- printing the final full path.

However, do NOT pass the absolute Windows path to Raylib.TakeScreenshot.

Pass a Raylib-compatible relative path from the current working directory,
using forward slashes where appropriate.

Conceptual example:

UserData/Screenshots/playback-level-2-boundary-000988.png

The expected Raylib resolution is then:

C:\Users\nrnob\source\repos\Tempest\
UserData\Screenshots\
playback-level-2-boundary-000988.png

Do not pass:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\...

directly to Raylib.TakeScreenshot in the current native build.


RELATIVE-PATH SAFETY

Before capture:

1. Determine the current working directory.
2. Determine the full intended destination beneath:
   <working directory>\UserData\Screenshots.
3. Create the screenshot directory using .NET.
4. Select the collision-safe final filename using .NET.
5. Derive a relative path from the working directory to that exact destination.
6. Normalize the path into a Raylib-compatible form if required.
7. Verify the relative path cannot escape the selected UserData root.
8. Pass only that relative path to Raylib.TakeScreenshot.
9. Verify the intended full destination exists and is non-empty afterward.

Do not temporarily change the process working directory to make Raylib work.

Do not use Directory.SetCurrentDirectory around the capture.

Do not introduce global mutable path behavior.


DIRECTORY CREATION

The UserData folder structure does not presently exist.

That is expected.

On the first accepted project-owned screenshot request, automatically create:

C:\Users\nrnob\source\repos\Tempest\UserData\

and:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\

Do not require Neal to create the directories manually.

Do not create empty directories at application startup.

Create only when needed.


SCREENSHOT FILENAME

Preserve the accepted naming format:

playback-level-<displayed-level>-boundary-<zero-padded-boundary>.png

Example:

playback-level-2-boundary-000988.png

Repeated capture at the same exact boundary must use collision-safe suffixes:

playback-level-2-boundary-000988-002.png

then:

playback-level-2-boundary-000988-003.png

Do not overwrite.


CAPTURE ORDER

Preserve the manually approved capture order:

1. draw the Tempest presentation;
2. draw PAUSED boundary/duration;
3. invoke the project capture callback;
4. finish the drawing lifecycle as currently established.

Do not change it merely to fix the pathname.

The generated screenshot image was already judged visually perfect.


CAPTURE VERIFICATION

Raylib.TakeScreenshot provides no useful success return value in the current
binding.

After capture, verify through the full intended .NET path:

- file exists;
- file length is greater than zero.

If successful, report:

Screenshot saved:
C:\Users\nrnob\source\repos\Tempest\
UserData\Screenshots\
playback-level-2-boundary-000988.png

If unsuccessful:

- report the Raylib-relative path;
- report the expected full path;
- keep playback paused;
- preserve boundary/cursor;
- do not pretend success.


GAMEPLAY RECORDING POLICY

Do not move Neal's existing development recordings.

Preserve:

Temp\GameplayRecordings\levels1-7.json

and all explicit paths beneath Temp.

For this correction, do not broaden or rework recording-path resolution unless
required to remove an internal inconsistency caused by the prior AppContext
design.

The immediate requirement is screenshot output.

If the cumulative candidate currently resolves bare recording filenames under
AppContext.BaseDirectory\UserData\GameplayRecordings, review that behavior.

Because Neal's simplified current development layout places UserData beside
Temp, any retained bare-filename recording behavior should be coherent with:

<working directory>\UserData\GameplayRecordings\

But do not migrate existing recordings.

Do not rewrite recording files.

Do not change explicit-path semantics.

If changing bare-filename recording behavior materially expands the correction,
defer bare-filename resolution and preserve explicit recording paths only.

State the decision clearly.


GIT IGNORE

The new repository-root UserData directory contains generated user files.

Ensure repository ignore policy excludes:

UserData/

or an appropriately precise generated-content rule.

Do not ignore source code or existing tracked content.

Do not stage generated UserData PNGs.

Do not commit generated screenshots.


KNOWN GENERIC SCREENSHOTS

Do not move, delete, rename, alter, or stage the existing generic:

screenshot000.png
screenshot001.png
screenshot002.png
screenshot003.png
screenshot004.png
screenshot005.png
screenshot006.png

Those remain Neal's manual-test artifacts.


REMOVE OBSOLETE ASSUMPTIONS

Update code, tests, comments, documentation, and reports that currently say:

- development UserData is beneath AppContext.BaseDirectory;
- current screenshot output is beneath bin\Debug\net9.0;
- the selected screenshot destination is an absolute pathname passed directly
  into Raylib.

Preserve any accurate statement that AppContext.BaseDirectory represents the
application binary root.

Do not preserve contradictory path rules.


DIAGNOSTIC MODE

Retain the bounded:

--screenshot-key-diagnostic

for the final manual test.

Update diagnostic output to report separately:

- Control observed;
- Shift observed;
- S press edge observed;
- F12 observed;
- project capture request created;
- Raylib-relative destination passed to TakeScreenshot;
- expected full destination;
- project screenshot file exists;
- project screenshot file length;
- whether new generic screenshotNNN files appeared.

The diagnostic must remain bounded and concise.

Do not enumerate unrelated keys.

Do not leave frame-by-frame logging.


TESTS — RELATIVE RAYLIB PATH

Add focused tests proving:

1. Full intended destination is:
   <selected development root>\UserData\Screenshots\<filename>.

2. Raylib receives:
   UserData/Screenshots/<filename>
   or the project-equivalent relative path.

3. Raylib does not receive an absolute Windows path.

4. Raylib path is relative to the selected current development root.

5. Raylib path cannot escape UserData\Screenshots.

6. Directory creation occurs on the first accepted capture.

7. No directory is required before capture.

8. File verification checks the full intended path.

9. Failure diagnostics contain both relative and full destination.

10. Duplicate suffix calculation occurs before deriving the relative path.

Avoid calling native Raylib in pure unit tests.


TESTS — ROOT POLICY

Add/revise tests proving:

- current selected development root is injectable/testable;
- UserData is beside Temp for the intended repository-root invocation;
- current working directory and AppContext.BaseDirectory are not conflated;
- screenshots do not go beneath bin\Debug\net9.0 for the development policy;
- path behavior is independent of assembly location in the lowest-level pure
  path tests;
- UserData creation remains on demand.


TESTS — SCREENSHOT CONTROL

Preserve tests proving:

- CTRL+SHIFT+S is the only Tempest screenshot command;
- paused-only behavior;
- one request per S press edge;
- no repeat capture;
- no capture while running;
- no capture during live gameplay;
- F11 is not a project screenshot command;
- F12 is not a project screenshot command;
- screenshot command remains outside PlayerInput;
- simulation boundary and recording cursor do not move.


TESTS — PLAYBACK REGRESSION

Protect:

- Space;
- forward/reverse ±1 scrubbing;
- Shift ±5 scrubbing;
- reverse reconstruction;
- PAUSED overlay;
- live-input handoff;
- headless runner;
- recording v1/v2;
- explicit Temp\GameplayRecordings paths.


STABILITY / ROLLBACK GATE

This is an isolated path fix.

After implementation, report:

- cumulative screenshot-milestone files changed;
- cumulative diff size;
- whether any production code outside screenshot/input/path/render capture seams
  changed;
- whether current candidate remains understandable and internally consistent.

If the correction requires changes to gameplay simulation, recording schemas,
playback-session semantics, audio, enemy behavior, or accepted scrubbing logic:

STOP.

Do not proceed.

Recommend abandoning the entire uncommitted screenshot milestone.

If the isolated correction passes automated validation but Neal's next manual
test still cannot produce the PNG at repository-root
UserData\Screenshots:

STOP.

Recommend abandoning the entire uncommitted screenshot milestone and returning
to the clean Phase 3B checkpoint.

Do not attempt another speculative correction.


REGRESSION VALIDATION

Run focused screenshot/chord/path tests.

Run neighboring playback/input/rendering tests.

Run Phase 1/2/3 playback regressions.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Require:

- tests actually execute;
- 0 failures;
- 0 unexpected skips;
- build 0 errors;
- warning count reported accurately;
- diff check passes.


DOCUMENTATION

Update the candidate documentation to state accurately:

- CTRL+SHIFT+S is definitive;
- key conflict is resolved;
- F12 was false during the chord test;
- no native screenshotNNN appeared;
- project screenshot request was created;
- capture then failed because Raylib 6 mangled an absolute Windows destination;
- current development UserData is repository-root UserData beside Temp;
- Raylib receives a relative path;
- .NET retains the full verification path;
- directories are created on demand;
- existing Temp recordings remain;
- future standalone layout remains a packaging goal;
- manual acceptance remains pending.

Do not mark the milestone accepted.


AI LOGS

Preserve this continuation under repository logging policy.

Create or update an appropriately paired report such as:

AI-Logs/Reports/2026-11-07-paused-playback-screenshot-relative-path-correction-report.md

Include:

1. Starting Git state.
2. Latest manual diagnostic.
3. Proven chord success.
4. Proven F12 absence.
5. Proven project-request creation.
6. Proven absolute-path failure.
7. Raylib path behavior.
8. Development root decision.
9. UserData beside Temp.
10. Relative Raylib path.
11. Full .NET verification path.
12. Directory creation.
13. Screenshot filename/suffix.
14. Recording-path decision.
15. Git-ignore decision.
16. Capture ordering preservation.
17. Boundary/cursor preservation.
18. PlayerInput separation.
19. Cumulative files/diff.
20. Stability assessment.
21. Focused tests.
22. Neighboring tests.
23. Full suite.
24. Build result.
25. diff check.
26. Manual test instructions.
27. Rollback gate.
28. Final Git state.


NO INTERACTIVE LAUNCH

Do not launch the game.

Neal performs the manual test.

Do not stage.
Do not commit.
Do not push.


FINAL MANUAL RETEST COMMAND

Provide:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\levels1-7.json --screenshot-key-diagnostic

Ask Neal to perform exactly this test:

1. From:
   C:\Users\nrnob\source\repos\Tempest

   note the current generic screenshotNNN files.

2. Run the command.

3. Pause with Space.

4. Scrub to an identifiable boundary.

5. Press CTRL+SHIFT+S exactly once.

6. Verify diagnostic reports:
   - Control=True;
   - Shift=True;
   - S press edge=True;
   - F12=False;
   - project request=True;
   - Raylib-relative path populated;
   - expected full destination populated;
   - project file exists=True;
   - file length greater than zero.

7. Verify this directory was created automatically:

   C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\

8. Verify exactly one boundary-aware PNG appears there.

9. Open it and verify the already accepted image and PAUSED overlay.

10. Verify no new repository-root screenshotNNN.png appears.

11. Verify the paused boundary did not move.

12. Press CTRL+SHIFT+S again deliberately.

13. Verify one unique-suffix screenshot is created.

14. Verify the first PNG remains unchanged.

15. Resume playback and verify the chord does nothing while running.

16. Verify forward/reverse scrubbing remains correct.


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

- screenshot candidate remains unstaged;
- no generated UserData contents staged;
- no commit;
- no push.


FINAL RESPONSE

Return:

1. Proven path defect.
2. Definitive screenshot command.
3. Selected development root.
4. UserData location.
5. Raylib-relative destination.
6. Full .NET destination.
7. Directory-creation behavior.
8. Filename/suffix behavior.
9. Capture verification.
10. Recording-path decision.
11. Git-ignore decision.
12. Capture-order preservation.
13. Boundary/cursor preservation.
14. PlayerInput separation.
15. Production files changed.
16. Tests changed.
17. Documentation changed.
18. Cumulative diff size.
19. Stability assessment.
20. Focused counts.
21. Neighboring counts.
22. Full-suite count.
23. Build warnings/errors.
24. git diff --check.
25. Manual retest checklist.
26. Rollback gate.
27. Report filename.
28. Final Git status.
29. Confirmation no gameplay change, recording-format change, snapshot,
    enemy-explosion implementation, Level 8/9, stage, commit, push, native
    library replacement, or Windows hook occurred.

Stop for Neal's manual acceptance.

TEMPEST_SCREENSHOT_RELATIVE_PATH_CORRECTION_COMPLETE