We are pausing before C2 for a small controller-input compatibility task.

Current clean checkpoint:
a922172 Ignore local video files

The preceding gameplay checkpoint is:
11773c0 Implement Level 3 geometry and directed rotation

The working tree should be clean before this task.

Purpose:
Investigate and, if justified by the findings, implement compatibility for my USB arcade controller so physical controller buttons can invoke existing Tempest actions. This is strictly an input/control convenience change. Do not alter gameplay mechanics, enemy behavior, timing, scoring, progression, geometry, or the semantics of Super Zapper, restart, or exit.

Controller observations from Windows:

- Spinner = mouse horizontal movement and already works in Tempest.
- Existing controller Fire control already works in Tempest.

Known physical-button mappings:
- Top black left = P
- Top black right = Enter
- Small red left = unknown
- Small red right = unknown
- Main red = Control
- Upper black left = Alt
- Upper black center = Space
- Upper black right = Shift
- Lower black 1 = Z
- Lower black 2 = X
- Lower black 3 = C
- Lower black 4 = V
- Left-side buttons = 5 and 1
- Trackball has no buttons.

Important observation:
The controller button mapped to Space creates an ordinary space character in Notepad, but DOES NOT activate Super Zapper in Tempest even though Space is an existing Tempest Super Zapper control.

Similarly, Z/X/C/V/P/1/5 generate ordinary characters in Notepad.

Windows On-Screen Keyboard visibly reacts to the controller's Ctrl, Alt, and Shift buttons, but does not visibly react to the letter/number buttons. Do not assume this establishes which Windows input mechanism the controller uses.

Goal:
Determine why controller-generated Space reaches Notepad but is not recognized as Tempest's existing Space/Super-Zapper input. Then establish a reliable way for selected controller buttons to invoke:
- Super Zapper
- Restart
- Escape/quit

Do not choose final Z/X/C assignments merely by assumption. First determine what input events/signals Tempest can reliably receive from this controller.

Tasks:

1. Verify baseline:
   git status --short
   git log -2 --oneline
   git diff --check

2. Inspect the existing input architecture thoroughly enough to identify:
   - keyboard polling/event APIs currently used,
   - mouse/spinner handling,
   - ordinary Fire input,
   - Super Zapper input,
   - restart input,
   - Escape/quit input,
   - where input is translated into existing game actions.

3. Explain specifically why normal keyboard Space works or is intended to work and identify plausible reasons the controller-generated Space could create text in Notepad but fail Tempest's current input path.

Do not guess beyond what the code and framework/API behavior establish.

4. Determine the smallest safe diagnostic needed to identify what Tempest receives when these controller buttons are pressed:
   P, Enter, Space, Ctrl, Alt, Shift, Z, X, C, V, 1, 5.

Prefer a DEVELOPMENT ONLY diagnostic mode or temporary input logging that uses the game's actual window/input framework.

The diagnostic should make it easy for Neal to press controller buttons and report what Tempest itself receives.

Do not require third-party controller remapping software unless the existing application/framework genuinely cannot expose a usable input signal.

5. If the existing code/framework already makes the cause and correct solution unambiguous, you may implement the smallest robust compatibility change directly. Otherwise STOP after adding the diagnostic and provide exact manual test instructions. Do not guess at a Windows/controller input workaround.

6. Preserve all existing keyboard and mouse controls.

Any controller compatibility mappings must be additive. Existing Space, R, Escape, Fire, mouse/spinner, and other established controls must continue working normally.

7. Do not change the gameplay implementation behind:
   - Super Zapper
   - restart/replay
   - Escape/quit

Only route additional accepted input into the same existing actions.

8. Prevent duplicate activation if one physical controller press is observable through more than one input path. Super Zapper and restart must retain their existing edge-triggered semantics.

9. Do not modify:
   - Reference/incitti
   - gameplay progression
   - Level 3 behavior
   - C1/C1.5 geometry/directed rotation
   - Tanker firing/C2 behavior
   - TEMPEST-BEHAVIOR-SPEC.md unless an actual permanent control behavior is implemented and the existing spec convention clearly requires documenting controls.

A diagnostic-only change is DEVELOPMENT ONLY and must not be presented as arcade gameplay behavior.

10. Add automated tests where practical for any abstraction or permanent input-routing change. Do not create tests that pretend to simulate physical USB behavior that the test framework cannot actually reproduce.

11. Save this exact prompt under AI-Logs/Prompts using the project's established naming convention.

12. Create/update an AI-Logs report describing:
   - current input architecture,
   - why the controller's text-producing buttons may differ from inputs Tempest currently recognizes, but only if established by evidence,
   - diagnostic or implementation changes,
   - tests performed,
   - manual controller test required,
   - any uncertainty still requiring Neal's observation.

13. Run appropriate tests/build for whatever changes are made and run git diff --check.

Do not commit or push.

Do not begin C2.

At the end, report whether:
A. the cause and permanent compatibility solution were established and implemented, or
B. a diagnostic is ready and Neal must run it and report the controller results.

If B, give Neal a short ordered button-testing procedure and explain exactly what diagnostic output should be reported back.
