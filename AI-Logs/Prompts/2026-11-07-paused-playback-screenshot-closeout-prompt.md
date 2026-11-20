FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, or modifying the repository, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the token and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before taking any project or repository action and explain that the supplied user task appears incomplete.


TASK

Close out the now manually accepted paused-playback screenshot/playback milestone.

Repository:
C:\Users\nrnob\source\repos\Tempest

Relevant prior report:
AI-Logs/Reports/2026-11-07-paused-playback-screenshot-relative-path-correction-report.md

MANUAL ACCEPTANCE PROVIDED BY NEAL

Neal has now manually tested the candidate and reports that it is functioning as designed.

Specifically:

- Screenshots are being saved correctly under:
  C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\

- Ctrl+Shift+S takes a screenshot only while visible file playback is paused.

- Pause and resume work as designed.

- Forward stepping works as designed.

- Reverse stepping works as designed.

- The saved screenshots were manually inspected and are valid gameplay captures at the selected playback boundaries.

Treat this as final manual acceptance of the paused-playback screenshot/playback milestone.

CLOSEOUT WORK

1. Inspect the current Git state before making changes.

2. Review the accumulated screenshot/playback candidate and the prior report sufficiently to ensure the closeout remains bounded to the accepted milestone.

3. Update the durable project documentation as appropriate, particularly:
   - TEMPEST-DEVELOPMENT-NOTES.md
   - TEMPEST-BEHAVIOR-SPEC.md
   - README.md only if its existing screenshot/playback documentation requires a closeout adjustment.

Document the manual acceptance accurately.

Preserve the project's evidence distinctions. Do not describe project decisions or manually observed behavior as ROM-backed unless that classification is already justified.

4. Preserve generated screenshots as user data. Do not stage or commit files under UserData/Screenshots.

5. Do not stage or commit old generic screenshotNNN.png artifacts or other unrelated generated/local files.

6. Do not broaden this task into gameplay changes, enemy behavior, Level 8/9 development, playback redesign, screenshot enhancements, packaging/install-root work, or unrelated cleanup.

7. Run the appropriate final validation for the complete accepted candidate, including at minimum:
   - focused relevant screenshot/path/input/playback tests
   - full solution tests
   - full solution build
   - git diff --check
   - verification that generated UserData content remains ignored/not staged

8. If validation fails, investigate only failures plausibly caused by this milestone. Do not commit or push a failing candidate.

9. If validation succeeds:
   - inspect the final diff and staged contents carefully
   - stage only the intended milestone files
   - commit the accepted screenshot/playback milestone with a clear descriptive commit message
   - push master to the configured GitHub origin

10. After the push, verify:
    - local HEAD
    - origin/master
    - final Git status
    - no generated screenshot/user-data files were committed

11. Create/update the corresponding AI report recording:
    - the manual acceptance above
    - final documentation changes
    - validation results
    - commit hash and message
    - push result
    - final Git state
    - any genuinely unresolved issue

AI-Logs are historical/ignored records and must not be committed unless existing project policy explicitly says otherwise.

Do not launch the interactive game. Manual gameplay acceptance has already been completed by Neal.

At completion, provide a concise closeout report including the final commit hash, validation results, push status, and Git status.

TEMPEST_SCREENSHOT_PLAYBACK_CLOSEOUT_ACCEPTED_20261107
