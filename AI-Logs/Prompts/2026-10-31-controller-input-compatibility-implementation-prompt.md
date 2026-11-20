Continue the controller-input compatibility task using Neal's completed physical USB controller diagnostic.

This is the permanent implementation step following the diagnostic investigation.

Do not begin C2.

Current project baseline before this controller task:
a922172 Ignore local video files

Previous gameplay checkpoint:
11773c0 Implement Level 3 geometry and directed rotation

The current worktree contains the uncommitted DEVELOPMENT ONLY controller-input diagnostic from the preceding task. Preserve that useful diagnostic unless there is a concrete reason to change it.

IMPORTANT CONTROL POLICY

The new controller-friendly keys are ALTERNATIVE bindings only.

Do NOT replace any existing keyboard or mouse controls.

The intended permanent controls are:

Super Zapper:
- Space remains the existing Super Zapper key.
- Do not replace or remove it.
- No additional Super Zapper key is needed yet.

Restart/Start/Replay:
- Existing R remains supported.
- Add P as an alternative.
- Effective behavior: R OR P invokes exactly the same existing restart/replay action wherever that action is currently permitted.

Quit:
- Existing Escape remains supported.
- Existing window-close behavior remains supported.
- Add V as an alternative quit key.
- Effective behavior: Escape OR V terminates through the same normal application shutdown behavior as closely as the current architecture permits.

Fire:
- Preserve all existing Fire controls unchanged.

Movement:
- Preserve spinner/mouse movement unchanged.
- Preserve existing keyboard movement controls unchanged.

Do not map Z, X, C, Enter, Ctrl, Alt, Shift, 1, or 5 to additional new actions in this checkpoint.

PHYSICAL CONTROLLER DIAGNOSTIC RESULTS

Neal ran the DEVELOPMENT ONLY --input-diagnostic mode with the physical USB arcade controller.

The important result is that the ordinary controller buttons DO reach Raylib as real keyboard key events. Therefore:

- Do not implement a character-input workaround.
- Do not implement Raw Input.
- Do not add a Win32 keyboard/input hook.
- Do not require third-party remapping software.
- Do not infer a USB/HID implementation beyond what was actually observed.

Observed physical controller signals included:

P:
STATE key=P(80) down=True pressed=True
KEY_QUEUE key=P(80) down=True pressed=True
CHAR_QUEUE U+0070 decimal=112

Enter:
STATE key=Enter(257) down=True pressed=True
KEY_QUEUE key=Enter(257) down=True pressed=True

Ctrl:
STATE key=LeftControl(341) down=True pressed=True
KEY_QUEUE key=LeftControl(341) down=True pressed=True

Alt:
STATE key=LeftAlt(342) down=True pressed=True
KEY_QUEUE key=LeftAlt(342) down=True pressed=True

Space:
STATE key=Space(32) down=True pressed=True
KEY_QUEUE key=Space(32) down=True pressed=True
CHAR_QUEUE U+0020 decimal=32

Shift:
STATE key=LeftShift(340) down=True pressed=True
KEY_QUEUE key=LeftShift(340) down=True pressed=True

Z:
STATE key=Z(90) down=True pressed=True
KEY_QUEUE key=Z(90) down=True pressed=True
CHAR_QUEUE U+007A decimal=122

X:
STATE key=X(88) down=True pressed=True
KEY_QUEUE key=X(88) down=True pressed=True
CHAR_QUEUE U+0078 decimal=120

C:
STATE key=C(67) down=True pressed=True
KEY_QUEUE key=C(67) down=True pressed=True
CHAR_QUEUE U+0063 decimal=99

V:
STATE key=V(86) down=True pressed=True
KEY_QUEUE key=V(86) down=True pressed=True
CHAR_QUEUE U+0076 decimal=118

The two small red Atari-style Player 1 / Player 2 volcano buttons located under "Turbo Mode" produced no observable Raylib diagnostic event.

Do not implement lower-level Windows/raw-input support for those volcano buttons in this task.

Player 1 as a future Start button would be desirable if its input can eventually be identified reliably, but that is explicitly deferred. P is the Start/Restart/Replay alternative for this checkpoint.

Neal also reported two non-blocking diagnostic usability issues:
- text displayed in the diagnostic window is difficult to read because of its presentation/font;
- Tee-Object did not create the expected text file during Neal's test, although console logging worked.

Do not spend significant time polishing those diagnostic-only issues. They are not blockers for this permanent input implementation.

IMPLEMENTATION TASKS

1. Re-check the current worktree and controller diagnostic changes before editing:

git status --short
git log -3 --oneline
git diff --check
git diff

Confirm that the existing work corresponds to the controller diagnostic and that no unrelated change has appeared.

2. Inspect the actual existing input/action routing before making the permanent change.

Preserve the current architecture wherever practical.

Specifically confirm:
- existing R restart/replay handling;
- existing Space Super Zapper handling;
- existing Escape/window-close handling;
- existing held Fire handling;
- spinner/mouse movement;
- input state gates for Playing, Game Over, Level Complete, death, exit travel, diagnostics, etc.

3. Add P as an alternative to R.

Requirements:
- R must continue working exactly as before.
- P must invoke exactly the same existing restart/start/replay request as R.
- P must be accepted only in the same states/contexts where R is currently accepted.
- Do not duplicate restart/game-reset logic.
- Do not create a P-specific restart path.
- Do not change score/lives/replay/new-game/progression semantics.
- Preserve press-edge behavior. Holding P must not repeatedly trigger restart.

Conceptually:

RestartPressed = R pressed OR P pressed

Implement this through the project's appropriate input abstraction rather than duplicating action logic.

4. Add V as an alternative to Escape/window quit.

Requirements:
- Escape must continue working.
- Window X/close must continue working.
- V should request normal application termination.
- Preserve press-edge behavior where applicable.
- Holding V must not repeatedly perform a gameplay action.
- Do not use V to alter simulation/game state before quitting unless that is already inherently part of the established shutdown path.
- Reuse the current Raylib/application termination architecture as closely as practical.
- Do not replace Raylib's existing Escape/default-close behavior.

Conceptually:

Quit = existing Escape/window-close behavior OR V press

Implement the smallest clean solution supported by the current architecture.

5. Keep Space exactly as the existing Super Zapper binding.

The physical diagnostic proves that the controller's Space button reaches Raylib as a normal Space press.

Do not add GetCharPressed or KEY_QUEUE consumption to production gameplay merely to support controller Space.

Do not create a controller-specific Super Zapper path.

Do not alter:
- Super Zapper charges,
- targeting,
- delayed resolution,
- scoring,
- lifecycle,
- state gating,
- or any other Super Zapper mechanic.

The earlier failure of controller Space in Neal's gameplay test is not established as an input-format incompatibility by the diagnostic. We will manually retest Space in actual gameplay after implementation.

6. Leave all other controller-generated keys alone.

Do not newly map:
- Z
- X
- C
- Enter
- Ctrl
- Alt
- Shift
- 1
- 5

Existing behavior associated with any of those keys, such as an already-established Fire control, must remain unchanged.

7. Preserve all existing mouse/keyboard controls.

This checkpoint must remain usable without this particular arcade controller.

The controller is not a required hardware device for Tempest.

8. Preserve the existing DEVELOPMENT ONLY --input-diagnostic unless cleanup is clearly necessary for correctness.

Do not let the diagnostic run during normal gameplay.

Do not make its logging/input queues part of normal production input routing.

If a trivial readability improvement is genuinely low-risk and does not distract from the task, it may be made, but it is not required.

Do not spend time diagnosing Tee-Object or PowerShell behavior; that is outside the game implementation.

9. Add focused automated tests where practical.

Tests should establish the permanent application semantics, not pretend to simulate the physical USB controller.

Appropriate coverage should verify, through the project's testable input/action abstraction:
- existing R restart/replay remains accepted;
- P produces the same restart/replay request;
- inappropriate game states do not gain new restart behavior;
- P retains press-edge semantics if that is exposed/testable at this layer;
- V requests application quit through the new routing;
- existing ordinary input behavior remains intact;
- Super Zapper behavior has not been changed.

Do not create fake USB/HID tests.

10. Documentation.

Update README.md to document the permanent alternative controls clearly.

The controls should be represented conceptually as:

Fire: existing controls unchanged
Movement: spinner/mouse and existing keyboard controls unchanged
Super Zapper: Space
Restart/Start/Replay: R or P
Quit: Escape or V

Use wording consistent with what the actual program supports.

Update TEMPEST-DEVELOPMENT-NOTES.md to record:
- the physical controller diagnostic result;
- ordinary controller buttons reached Raylib as genuine keyboard press events;
- the volcano Player 1/2 buttons produced no observable Raylib event;
- P and V were selected as additive compatibility aliases;
- existing controls were preserved;
- volcano-button investigation is deferred.

Do not describe these convenience aliases as ARCADE VERIFIED gameplay behavior.

Only change TEMPEST-BEHAVIOR-SPEC.md if its established scope/conventions genuinely require permanent input aliases to be recorded. Do not add controller-specific implementation narrative to the behavior spec unnecessarily.

11. Update the existing controller-input AI report.

Record:
- Neal's physical diagnostic evidence;
- correction of the earlier uncertainty about normal controller buttons;
- confirmation that P, Space, V and the other observed buttons arrive as actual Raylib keyboard events;
- Player 1/Player 2 volcano buttons produced no observable event;
- permanent P and V aliases implemented;
- Space remains the existing Super Zapper binding;
- existing controls remain available;
- tests/build/diff results;
- manual validation still required.

Preserve historical investigation details where useful, but clearly label superseded uncertainty so the report does not continue to imply that physical diagnostic results are pending.

12. Save this exact prompt under AI-Logs/Prompts using the project's established naming convention.

13. Validation.

Run appropriate focused tests for the controller/input changes.

Then run:

dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check

Inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git status --short -- TEMPEST-BEHAVIOR-SPEC.md

Confirm:
- no C2 work occurred;
- Reference/incitti is unchanged;
- C1/C1.5 behavior is unchanged;
- no gameplay logic beyond input/action routing changed;
- no existing control was removed;
- AI-Logs remain ignored;
- Videos/ remains ignored.

Do not commit.
Do not push.
Do not begin C2.

FINAL REPORT

At the end, report:

1. Exact production input changes made.
2. Whether R remains Restart/Replay.
3. Whether P is now an alternative Restart/Replay key.
4. Whether Escape/window close remain unchanged.
5. Whether V is now an alternative Quit key.
6. Confirmation that Space remains the existing Super Zapper key with no new controller-specific mechanism.
7. Confirmation Fire and spinner behavior were not changed.
8. Focused test results.
9. Full solution test result.
10. Build result.
11. git diff --check result.
12. Final git status/diff summary.
13. Confirmation Reference/incitti is unchanged.
14. Confirmation C2 was not started.
15. Any remaining concern that should block manual testing.

Then give Neal this short manual gameplay checklist:

A. Normal gameplay:
- controller spinner moves the Blaster normally;
- existing controller Fire works normally;
- controller Space activates Super Zapper when Super Zapper is legitimately available;
- ordinary keyboard Space still activates Super Zapper.

B. Restart/Replay:
- in each existing state where R is accepted, controller P performs the same action;
- ordinary keyboard R still works;
- holding P does not cause repeated unintended restarts.

C. Quit:
- controller V exits normally;
- ordinary keyboard Escape still exits normally;
- window X still closes normally.

D. Regression:
- no obvious gameplay, movement, firing, Super Zapper, progression, geometry, or rendering regression.

Do not commit until Neal reports the manual controller results.