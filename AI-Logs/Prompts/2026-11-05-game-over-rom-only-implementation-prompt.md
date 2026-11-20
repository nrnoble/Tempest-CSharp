FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, committing, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" exists in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project/repository action.


TEMPEST ROM-ONLY GAME OVER VECTOR PRESENTATION IMPLEMENTATION

Implement a deliberately bounded first reconstruction of the original arcade Tempest Game Over presentation using ONLY project-local ROM evidence.

One purpose of this milestone is experimental:

Determine how accurately the distinctive Atari Tempest color-vector presentation can be reconstructed from ROM behavior without giving the implementation AI screenshots, video, MAME captures, or other visual examples.

DO NOT search for, request, or use visual reference material.

Neal will compare the resulting first implementation against his original Tempest arcade machine afterward.


STARTING BASELINE

Require the clean accepted checkpoint:

b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f
Correct Super Zapper color presentation

First run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Require:

- master;
- exact expected HEAD;
- clean tracked worktree/index;
- no unexplained non-ignored untracked files.

If not clean, STOP rather than resetting, restoring, cleaning or discarding anything.


VECTOR-GAME DESIGN PRINCIPLE

Tempest is an Atari color-vector arcade game with a distinctive visual vocabulary.

For this milestone and future visual work, use this principle:

Tempest visual features should be reconstructed using the project's established vector geometry, line rendering, projection, palette/color and animation systems.

Do not reinterpret arcade presentation as sprite-, texture-, bitmap-, raster-, or generic particle-based effects unless explicitly required by evidence.

When recreating an arcade visual effect, prefer ROM-backed vector drawing behavior and Tempest's established visual vocabulary over a merely plausible generic visual approximation.

Where ROM behavior is incomplete, explicitly distinguish PROJECT APPROXIMATION from ROM-BACKED behavior.

Add this principle concisely to the appropriate durable project AI/development guidance if it is not already stated clearly.

Do not turn this into a large documentation rewrite.


SOURCE AUTHORITY

The sole Atari disassembly authority is:

Reference/arcade/Tempest Rom Source code.md

Do not search the Internet.

Do not use:

- screenshots;
- video;
- MAME captures;
- another Tempest disassembly;
- another ROM revision;
- MAME source;
- another arcade emulator implementation;
- Atari 2600 Tempest;
- another console/computer port.

Mark Incitti's implementation may be inspected only if required to understand existing project architecture, NOT as evidence of arcade Game Over presentation.

The objective is specifically to test a ROM-only visual reconstruction.


INVESTIGATION FINDINGS TO VERIFY

Before implementing, verify the relevant findings directly against the project-local master ROM source.

Do not blindly trust this task's summary where the source can be checked easily.

The prior bounded investigation found:

1. Final single-player death transitions into post-game score processing.

2. GAME OVER is produced through shared presentation code rather than a clearly isolated fixed-duration "Game Over state."

3. In the traced Game Over presentation:
   - GAME OVER is centered;
   - scale is 1;
   - palette index is 5;
   - selection involves the play-state condition and timectr & $20;
   - no independent ROM timer establishing a fixed GAME OVER-only duration was identified.

4. The distinctive brief animation Neal remembers corresponds to ROM gamestate $24, labeled in the disassembly as a "high-score explosion."

5. State $24:
   - seeds randomized vector records;
   - uses POKEY random values;
   - assigns movement data to those records;
   - updates/moves them within bounded coordinates;
   - stops creating additional records after its counter expires;
   - then waits for the remaining active records to clear.

6. $010E starts at $20 and decrements on every other timectr value.
   The prior analysis characterized this as 64 timectr increments before spawning ceases.
   Verify that interpretation before relying on it.

7. The animation does not appear to be a transformation of the tube/web itself.

8. The common rendering infrastructure remains responsible for the ordinary vector layers.

9. The pending-vector drawing layer associated with the effect uses palette index $0C.

10. The effect terminates when its active records have cleared. The prior investigation identified $010D reaching zero followed by gamestate = $12 at approximately $A656.

11. State $12 is associated with subsequent high-score/initials handling.

STOP the arcade behavioral trace at that handoff.

Do not investigate state $12 beyond what is strictly necessary to establish that state $24 has ended.


IMPORTANT ROM QUALIFICATION

The prior investigation found that state $24 is NOT an unconditional animation following every arcade Game Over.

It is associated with a score-qualifying/high-score route.

Another post-game route can bypass it.

Therefore implementing:

final death
-> GAME OVER
-> state-$24-inspired vector animation

for every C# one-player Game Over is NOT literal ROM control flow.

For this personal recreation, Neal explicitly authorizes the following PROJECT DECISION:

Always present the discovered arcade Game Over visual sequence, including the state-$24 vector animation, after the single player's final death.

This is intentionally done because Neal wants the recognizable arcade Game Over presentation in the current one-player recreation without implementing the arcade high-score/front-end infrastructure.

Document this distinction clearly.

Do NOT modify ROM/reference documentation to make the arcade appear unconditional.


STRICT IMPLEMENTATION SCOPE

Implement only:

FINAL ONE-PLAYER DEATH
  ->
GAME OVER presentation
  ->
ROM-derived state-$24 vector animation
  ->
existing C# terminal Game Over behavior
  ->
R remains available to start a fresh game

That's it.


AFTER THE ANIMATION

The existing C# R-to-restart behavior is the intentional project placeholder for everything the actual arcade would eventually do involving:

- high-score processing;
- initials;
- top-player lists;
- attract mode;
- credits;
- coins;
- start buttons;
- arcade front-end flow.

Do not implement any of those systems.

Do not imitate them.

Do not add placeholder screens for them.

Once the requested vector animation finishes, return to the project's existing terminal Game Over presentation/state and preserve R-to-restart.


TWO-PLAYER IS COMPLETELY OUT OF SCOPE

Neal does not plan to implement two-player Tempest.

Do not implement, analyze, prepare for, architect around or add abstraction for:

- Player 2;
- alternating players;
- two-player Game Over;
- two-player scores;
- player switching;
- two-player credits.

Where ROM routines contain two-player branches, use only enough information to isolate the one-player behavior.


GAME OVER PRESENTATION

Reproduce the ROM-backed visible GAME OVER presentation as closely as the available evidence permits.

Preserve ROM-backed:

- text content;
- positioning logic;
- vector-text style;
- palette index behavior;
- any established timectr-dependent display behavior;
- established background rendering layers.

Do NOT use a conventional bitmap font if the project already has the appropriate Atari/vector text mechanism for this presentation.

Do not guess an exact wall-clock GAME OVER hold period if the ROM doesn't establish one.

Where the ROM state cannot provide a self-contained duration appropriate to this simplified C# flow, choose the smallest reasonable PROJECT APPROXIMATION necessary to produce the presentation Neal described.

Make that approximation explicit and easy to refine after manual comparison.

Do not tune it from Neal's "probably less than ten seconds" recollection as though that were ROM data.


STATE-$24 VECTOR ANIMATION

This is the critical part of the experiment.

Reconstruct the effect from the actual ROM vector-record behavior rather than inventing a generic explosion.

Trace sufficiently to reproduce as much of the following as the ROM establishes:

- number/capacity of active records;
- record creation cadence;
- initialization;
- starting coordinates;
- coordinate ranges;
- random selection;
- vector/graphic identifier selection;
- movement direction;
- movement amount/rate;
- bounds;
- lifetime/termination;
- palette/color slot;
- renderer layer;
- creation counter;
- drain-after-spawning behavior;
- completion condition.

Preserve ROM data exactly where it maps reasonably into the existing C# logical/vector coordinate model.


NO GENERIC PARTICLE SYSTEM

Do NOT respond to "randomized moving vectors" by creating a generic modern particle system.

Do not add:

- circles;
- textured particles;
- glowing sprites;
- bitmap explosions;
- alpha-blended particle clouds;
- generic fireworks;
- conventional raster effects.

The original effect is produced through Atari vector-display mechanisms.

Represent it through Tempest's existing line/vector rendering vocabulary.

If a small collection type is needed to represent the ROM-like temporary vector records, keep it narrow and purpose-specific.


UNKNOWN VECTOR SHAPES

The prior ROM investigation reportedly did not establish a definitive real-screen identification of every generated/vector shape.

Do not use screenshots/video to resolve this.

Investigate the local ROM tables, graphic IDs and vector drawing paths as far as reasonably possible.

If exact shape identity still cannot be established:

- use the closest ROM-supported interpretation possible;
- classify the unresolved shape appearance as UNKNOWN;
- make any C# visual translation an explicit PROJECT APPROXIMATION;
- keep that translation localized and easy to revise after Neal's arcade comparison.

Do not spend an unbounded amount of time reverse-engineering unrelated vector graphics.


COLOR

Use ROM palette/color indices where established.

If the ROM identifies palette index $0C for the animation but the master listing does not expose sufficient packed palette data to derive literal RGB:

- preserve $0C as ROM-BACKED evidence;
- select the closest existing C# Tempest palette translation if one is already established;
- otherwise make the translation an explicit PROJECT APPROXIMATION.

Do not claim exact RGB fidelity without evidence.


BACKGROUND / EXISTING GAME OBJECTS

Trace what the common renderer displays during the relevant states.

Preserve the ROM-backed visibility relationship as far as the C# architecture reasonably allows.

Do not assume that because the common renderer has layers, every surviving gameplay object must necessarily remain active in the C# implementation.

Separate:

- simulation activity;
- visible retained geometry;
- temporary Game Over animation records.

Do not allow enemies or shots to resume meaningful gameplay that can affect score/lives during the Game Over presentation.


NO GAMEPLAY SEMANTICS

Once the final life is gone and the Game Over presentation begins:

- score should not continue through ordinary combat;
- no player comeback;
- no new enemy-wave progression;
- no additional lives;
- no Super Zapper interaction;
- no gameplay input other than the existing restart behavior once the terminal state is reached.

The Game Over animation is presentation, not continued gameplay.


CURRENT ACCEPTED FEATURES MUST REMAIN INTACT

Do not change:

- Levels 1-7;
- 2.5 calibrated Flipper rim pursuit;
- --rim-speed diagnostic;
- Super Zapper gameplay;
- accepted Super Zapper color sequence;
- Blaster firing;
- player shots;
- enemy firing;
- below-rim Flipper programs;
- Level 7 2-3-2-2-2 route;
- spinner;
- geometry;
- approaches;
- exits;
- ordinary death animation;
- scoring before Game Over;
- audio not directly required by the presentation.


TIMING

Keep ROM counter behavior wherever it maps directly.

Do not pretend ROM timectr increments are wall-clock seconds unless the project establishes that relationship.

If C# requires a presentation timer or cadence translation:

- preserve the ROM-relative progression as far as possible;
- classify the C# mapping as PROJECT APPROXIMATION;
- localize the timing constant so Neal can refine it after seeing the first reconstruction.

Do NOT create a command-line tuning system unless absolutely necessary.

Initial visual judgment should come first.


TESTING

Add focused tests sufficient to protect:

1. Final life transitions into the new Game Over presentation rather than immediately entering the old terminal screen.

2. Ordinary Blaster death completes before Game Over presentation begins.

3. GAME OVER presentation uses the intended vector presentation path.

4. State-$24-inspired animation starts after the Game Over portion.

5. Temporary vector records are created deterministically under a seeded/test randomness source.

6. Record count/capacity respects the ROM-derived design.

7. Record movement follows the translated ROM rules.

8. Record spawning stops at the translated ROM counter boundary.

9. Existing records drain afterward.

10. Completion occurs only after the appropriate records clear.

11. Animation cannot alter score/lives/wave progression.

12. Gameplay input cannot resume during presentation.

13. Once animation ends, existing C# terminal Game Over state is reached.

14. R-to-restart still starts a fresh Level 1 game afterward.

15. Ordinary non-final death/re-entry remains unchanged.

16. Levels 1-7 progression is unaffected.

17. Super Zapper behavior/color presentation remains unaffected.

18. Rim-speed calibration remains unaffected.

Use deterministic tests for randomized records.

Do not require screenshot-perfect visual assertions.


DEVELOPMENT-ONLY DIRECT TEST PATH

If the existing project architecture supports a narrow direct Game Over presentation test mode cleanly, a development-only command-line option may be added so Neal can reach the new presentation without intentionally losing all lives.

For example conceptually:

--game-over-test

Only add this if it can reuse the production Game Over presentation path rather than implementing a separate demo.

It must:

- start immediately at, or very near, the production Game Over sequence;
- exercise the exact production presentation;
- contain no duplicate animation implementation;
- remain development-only.

If such an option would require significant extra architecture, skip it.

The ordinary:

dotnet run --project src\Tempest.Game -- --lives 1

path is sufficient if practical.


DURABLE VECTOR PRINCIPLE

Add the concise Tempest vector-game design principle to the most appropriate durable project guidance.

Keep evidence classification clear.

Do not document the new Game Over presentation as accepted yet.

This first implementation requires Neal's manual comparison.


DOCUMENTATION

Update development notes/spec only enough to record:

- Game Over presentation is under ROM-only experimental implementation;
- state $24 behavior is ROM-BACKED where traced;
- unconditional use after every C# Game Over is a PROJECT DECISION;
- any timing/RGB/shape translations are PROJECT APPROXIMATIONS;
- no visual arcade reference has yet been supplied;
- manual acceptance remains pending.

Do not mark it accepted.


AI LOGS

Save paired records:

AI-Logs/Prompts/2026-11-05-game-over-rom-only-implementation-prompt.md

AI-Logs/Reports/2026-11-05-game-over-rom-only-implementation-report.md

The report must clearly distinguish:

ROM-BACKED
CURRENT C# BEHAVIOR
PROJECT DECISION / APPROXIMATION
UNKNOWN


AUTOMATED VALIDATION

Run focused Game Over/death/rendering tests.

Run neighboring regression covering:

- player death;
- renderer;
- Levels 1-7;
- progression;
- campaign soak;
- Super Zapper;
- rim pursuit;
- restart.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual counts.

Build must have 0 errors.


NO INTERACTIVE LAUNCH

Do not launch the game.

Stop after automated validation so Neal can see the ROM-only reconstruction for the first time himself.


MANUAL COMPARISON

Provide the appropriate launch command.

If --game-over-test was implemented:

dotnet run --project src\Tempest.Game -- --game-over-test

Also provide:

dotnet run --project src\Tempest.Game -- --lives 1

Neal will compare the result directly with his original arcade machine.

Do not request screenshots or video before Neal performs that initial comparison.


WHAT NEAL SHOULD EVALUATE

Ask only:

1. Does GAME OVER itself look and behave like the arcade?

2. Does the brief animation afterward immediately resemble the arcade effect?

3. Are the vector shapes, movement, distribution and color broadly correct?

4. Does the transition from GAME OVER into the vector animation resemble the arcade?

5. Does the animation end naturally?

6. Does the resulting terminal R-to-restart state remain acceptable as the current placeholder?

7. Which aspects, if any, are visibly wrong?

Do not ask Neal to provide screenshots/video unless the first comparison identifies something that ROM evidence could not reproduce.


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

HEAD must remain the starting committed checkpoint.


FINAL RESPONSE

Return:

1. Exact Game Over sequence implemented.
2. ROM-backed GAME OVER behavior.
3. ROM-backed state-$24 behavior.
4. Explicit project decision making the animation unconditional.
5. Vector-game design principle added.
6. Vector-record representation.
7. Shape-identification result and remaining UNKNOWN details.
8. Color mapping and its evidence classification.
9. Timing mapping and its evidence classification.
10. Background/game-object behavior during presentation.
11. Production files changed.
12. Test files changed.
13. Focused test counts.
14. Neighboring regression counts.
15. Full-suite counts.
16. Build warnings/errors.
17. git diff --check result.
18. Direct test command if added.
19. Ordinary launch command.
20. Concise manual comparison checklist.
21. Report filename.
22. Final Git status.
23. Confirmation no screenshots/video/Internet visual references, unrelated gameplay changes, commit, push or interactive game launch occurred.

Stop for Neal's ROM-only visual comparison.

TEMPEST_GAME_OVER_ROM_ONLY_IMPLEMENTATION_COMPLETE