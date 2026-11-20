FAIL-CLOSED CONTINUATION CHECK

Read this complete continuation before taking any repository action.

Verify exactly one complete standalone TEMPEST_ completion token appears near
the end on a line by itself.

If that check fails, stop before modifying the repository.


TEMPEST PHASE 2 PLAYBACK NAVIGATION REFINEMENT

Continue from the existing uncommitted Phase 2 visible-playback-controls
candidate.

Do not restart the milestone.

The current candidate is very close to manually accepted.

NEAL'S MANUAL RESULT

Neal reports:

"This is close to perfect. I am very impressed!"

Manual testing established:

- normal recorded playback remains correct;
- Space pause/resume works;
- one simulation tick per Right Arrow press is ideal;
- tick-by-tick visible inspection is genuinely useful.

Refine ONLY the forward-navigation controls before Phase 2 closeout.


ACCEPTED LONG-TERM CONTROL MODEL

Record this intended playback-analysis control scheme:

SPACE
- toggle pause/resume.

RIGHT ARROW TAP
- advance +1 simulation tick while paused.

LEFT ARROW TAP
- future: move -1 simulation tick after backward reconstruction exists.

HOLD RIGHT ARROW
- repeat the SAME +1-tick operation using normal keyboard-repeat behavior,
  similar to holding an arrow key in a text editor.

HOLD LEFT ARROW
- future: repeatedly perform the same -1-tick backward operation.

SHIFT + RIGHT ARROW
- advance +5 simulation ticks while paused.

SHIFT + LEFT ARROW
- future: move -5 simulation ticks.

The Shift operations are grouped navigation conveniences, not alternate
simulation modes.

Do NOT implement Left Arrow/backward behavior yet.


RIGHT ARROW KEY REPEAT

Current Phase 2 uses single press-edge Right Arrow behavior.

Investigate the current Raylib-cs/Raylib 6 binding for:

IsKeyPressedRepeat

or its exact available equivalent.

If the installed binding exposes Raylib's repeat API, prefer that over inventing
a separate wall-clock repeat timer.

Desired behavior:

tap Right Arrow:
one +1-tick operation.

hold Right Arrow:
initial +1-tick operation, then repeated +1-tick operations according to the
available keyboard-repeat event behavior.

Each repeat is exactly equivalent to another ordinary one-tick StepForward.

Do NOT turn a held key into continuous per-render-frame RunOneStep calls.

Do NOT use IsKeyDown in a way that executes 60 steps/second immediately merely
because the key is held.

Do NOT change simulation timing.

If the installed binding does NOT expose a suitable repeat API, stop and report
the available input surface and recommend the smallest fallback rather than
guessing.


SHIFT + RIGHT ARROW

While playback is active AND paused:

SHIFT + RIGHT ARROW performs exactly five ordinary forward playback steps,
subject to remaining recorded frames.

Conceptually:

repeat up to 5 times:
    GameplayPlaybackSession.StepForward()

Do not:

- skip recorded frames;
- use a larger simulation delta;
- alter RNG behavior;
- combine five PlayerInput frames;
- change RunOneStep;
- execute beyond the recording duration.

If fewer than five frames remain:

advance only to the final recording boundary.

Do not throw merely because fewer than five frames remain.

Render the resulting boundary normally.


SHIFT MODIFIER

Accept the normal Shift modifier state appropriate to the existing Raylib input
API.

Do not make Shift itself a gameplay or playback action.

Do not modify the recorded PlayerInput.

Playback navigation controls remain completely outside the recorded simulation
input stream.


SHIFT + RIGHT HOLD

If Raylib keyboard repeat naturally reports repeated Right Arrow events while
Shift remains held:

each repeated navigation event should perform another +5-tick group.

Therefore:

tap Shift+Right:
+5 ticks.

hold Shift+Right:
repeated +5-tick groups at keyboard-repeat cadence.

Again, every underlying simulation tick must still execute normally and
sequentially.


RUNNING PLAYBACK

Right Arrow and Shift+Right must remain navigation controls only while playback
is PAUSED.

While playback is running:

- Right Arrow must not add extra simulation ticks;
- Shift+Right must not add extra simulation ticks;
- normal recorded playback remains one recorded simulation frame per ordinary
  playback iteration.


LEFT ARROW

Continue reserving Left Arrow during active recorded playback.

For this Phase 2 refinement:

- Left Arrow tap does nothing;
- holding Left Arrow does nothing;
- Shift+Left does nothing;
- Left Arrow must not move the recorded Blaster;
- Left Arrow must not enter PlayerInput;
- Left Arrow must not consume recording frames.

Document the future intended behavior:

Left tap = -1
Left hold = repeated -1
Shift+Left = -5
Shift+Left hold = repeated -5

Implementation remains Phase 3B after deterministic backward reconstruction
exists.


LIVE INPUT HANDOFF

After recorded playback completes and normal live input resumes:

restore all ordinary control meanings.

In particular:

- Right Arrow returns to ordinary live Blaster movement;
- Left Arrow returns to ordinary live Blaster movement;
- Space returns to Super Zapper;
- Shift retains its ordinary existing behavior;
- playback autorepeat/group-stepping logic no longer owns those keys.

Do not leak playback-control state across the handoff.


PAUSED OVERLAY

Preserve the current accepted Phase 2 overlay:

PAUSED  boundary / duration

Do not redesign it.

After a +1 or +5 operation, it must display the resulting current boundary.

The overlay must remain responsive during repeated key navigation.


AUDIO

Preserve current Phase 2 stepping semantics.

Every actual simulation tick may emit its normal audio event.

Do not replay audio when merely redrawing a paused state.

For Shift+Right, do not invent a new audio policy in this refinement.

If five rapidly executed ticks produce several ordinary events, let existing
audio behavior stand for manual judgment.

No audio redesign.


TESTS

Extend Phase 2 tests to prove:

1. Right Arrow initial press while paused advances exactly +1.

2. Right Arrow repeat event advances exactly another +1.

3. A held Right Arrow does not become uncontrolled every-render-frame
   simulation.

4. Right Arrow repeat while running adds no simulation step.

5. Shift+Right while paused advances exactly +5 when at least five frames
   remain.

6. Shift+Right is implemented as five sequential ordinary session steps.

7. Shift+Right near recording completion stops exactly at DurationTicks.

8. No nonexistent frame is consumed.

9. Shift+Right repeat event performs another +5 group when appropriate.

10. Neither Right nor Shift+Right enters PlayerInput.

11. Left Arrow remains inert during recorded playback.

12. Shift+Left remains inert during recorded playback.

13. Pause overlay boundary updates after +1 and +5 navigation.

14. Live-input handoff restores ordinary Left/Right/Space behavior.

15. Recording format and recording capture remain unchanged.

Use a testable input/control seam rather than requiring native interactive
keyboard automation where practical.


VALIDATION

Rerun the Phase 2 focused tests.

Run neighboring playback/input/recording regressions.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report warning count accurately;
- git diff --check passes.


DOCUMENTATION

Update the Phase 2 candidate documentation with the accepted intended navigation
model:

SPACE = pause/resume

RIGHT TAP = +1 tick

RIGHT HOLD = repeated +1 operations

SHIFT+RIGHT = +5 ticks

SHIFT+RIGHT HOLD = repeated +5 groups

Future Phase 3B:

LEFT TAP = -1 tick

LEFT HOLD = repeated -1 operations

SHIFT+LEFT = -5 ticks

SHIFT+LEFT HOLD = repeated -5 groups

Make clear:

- these are playback-analysis controls;
- they never become recorded gameplay input;
- every forward tick still executes the ordinary production simulation;
- backward functionality remains unimplemented.


AI LOGS

Preserve this continuation according to project logging policy.

Update the existing Phase 2 report or create the appropriate paired refinement
record consistent with project instructions.

Record:

- Neal's "close to perfect" manual result;
- confirmation that one tick per tap is ideal;
- repeat implementation mechanism;
- +5 group implementation;
- tests and validation;
- backward controls remain deferred;
- manual acceptance remains pending after this refinement.


NO INTERACTIVE LAUNCH

Do not launch the game.

Neal will manually test the refinement.

Do not stage.
Do not commit.
Do not push.


MANUAL ACCEPTANCE

After validation, ask Neal to verify:

1. Space still pauses/resumes correctly.

2. Right Arrow tap advances exactly one tick.

3. Holding Right Arrow now repeats +1 steps in a comfortable text-editor-like
   manner.

4. Releasing Right Arrow stops repeated stepping immediately.

5. Shift+Right advances exactly five ticks.

6. Holding Shift+Right repeats five-tick groups.

7. The PAUSED boundary counter tracks these operations correctly.

8. Left Arrow and Shift+Left remain inert during playback.

9. Normal playback fidelity is unchanged.

10. Playback completion and live-input handoff remain correct.

11. Normal gameplay controls return after handoff.

12. No duplicate/skipped playback event, crash, or input regression occurs.


FINAL GIT CHECK

Finish with:

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

1fd28c1b9a7eaad0a0a1ed35c781f57a8b3d90af

Do not stage, commit, or push.


FINAL RESPONSE

Return:

1. Right Arrow repeat mechanism.
2. Right tap behavior.
3. Right hold behavior.
4. Shift+Right behavior.
5. Shift+Right hold behavior.
6. End-of-recording behavior.
7. Left/Shift+Left deferred behavior.
8. PlayerInput separation.
9. Live-control restoration.
10. Overlay behavior.
11. Audio behavior.
12. Production files changed.
13. Tests changed.
14. Documentation changed.
15. Focused test counts.
16. Neighboring regression counts.
17. Full-suite counts.
18. Build warnings/errors.
19. git diff --check result.
20. Manual acceptance checklist.
21. Final Git status.
22. Confirmation no backward navigation, rewind, snapshots, headless CLI,
    recording change, stage, commit, push, Level 8/9, or unrelated gameplay
    change occurred.

Stop for Neal's manual acceptance.

TEMPEST_PHASE2_PLAYBACK_NAVIGATION_REFINEMENT_COMPLETE