FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Perform a tightly bounded, read-only investigation of Tempest simulation timing,
arcade gameplay cadence, host-loop pacing, and the feasibility of a future
command-line speed multiplier.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

e3a3d4cf31449bc710c43d9d912ca5954605ade8
Close out accepted ROM-derived Blaster-shot rendering

Confirm the actual Git state before beginning.

Do not modify production code, tests, durable documentation, configuration,
project files, recordings, screenshots, or Git state.

Do not stage, commit, push, clean, rename, move, or delete anything.


USER'S QUESTIONS

Neal wants to understand how the C# project controls game speed despite running
on a PC that is vastly faster than the original arcade hardware.

"Speed" in this task primarily means:

- how much real elapsed time corresponds to one simulation/game tick;
- how frequently TempestGame.RunOneStep executes;
- whether that cadence accurately represents the arcade game's gameplay update
  cadence;
- whether rendering, input, audio, and simulation share one clock;
- whether the current project drops, catches up, or delays ticks;
- whether the current overall game speed could contribute to the game feeling
  slightly harder or taking longer to clear than Neal's medium-difficulty
  arcade cabinet;
- whether a future command-line option such as `--speed 2` or `--speed 0.5`
  can safely control wall-clock simulation speed.

This task is investigation-only.

Do not implement `--speed` or change any timing behavior.


CONTEXT TO PRESERVE

Neal currently considers the overall gameplay timing very solid, possibly
perfect.

The current project plays extremely close to the original arcade game. Any
difference may be subtle enough that mainly an expert Tempest player would
notice it.

Neal's real arcade cabinet is set to medium difficulty.

Do not assume that the perceived harder or longer-playing C# game is caused by
global timing.

Previous investigation already established that Levels 1-7 base/default enemy
supplies match the ROM.

Other known differences may affect gameplay feel independently of the global
tick rate, including:

- enemy release/pending pacing;
- active-enemy and per-type limits;
- collision/update ordering;
- shot persistence;
- enemy selection;
- movement programs;
- death/re-entry behavior;
- difficulty parameters.

Keep those possibilities separate from global simulation speed.

Do not reopen or modify previous accepted milestones.


LOCAL ROM AUTHORITY

Use the project-local ROM-derived annotated disassembly:

Reference/arcade/Tempest Rom Source code.md

Do not search the internet for another Tempest disassembly.

Trace the arcade timing directly from the local ROM listing and project
references.

Use external/reference implementations only if the local ROM cannot answer a
specific engineering question, and classify them as secondary evidence.


PRIOR PROJECT EVIDENCE

Locate and inspect relevant existing project evidence, including as applicable:

- Program.cs
- TempestGame.cs
- GameplayRecordingFile.cs
- GameplayPlaybackSession.cs
- HeadlessGameplayPlaybackRunner or equivalent
- VisiblePlaybackController.cs
- input sampling code
- audio update/playback code
- rendering/window-loop code
- command-line parsing
- tests covering simulation, playback, determinism, pause, stepping, and
  headless execution
- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md
- previous timing, firing, playback, campaign-soak, and deterministic-playback
  reports.

Reuse established findings rather than starting from assumptions.


EVIDENCE CLASSIFICATION

Classify findings as appropriate:

- ROM-LISTING-BACKED
- HARDWARE/DRIVER REFERENCE
- EXISTING C# IMPLEMENTATION
- AUTOMATED-TEST EVIDENCE
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR
- MANUALLY ARCADE-ACCEPTED
- PROJECT DECISION
- SECONDARY REFERENCE
- INTERPRETATION / HYPOTHESIS
- UNKNOWN / UNRESOLVED

Do not describe an inferred frequency or duration as ROM-backed unless the
relevant control flow and divider are established.


QUESTION 1: CURRENT C# HOST LOOP

Trace the complete interactive host loop.

Determine:

- target render frequency;
- target simulation frequency;
- exact nominal duration of one simulation tick;
- whether one render frame always corresponds to one RunOneStep call;
- whether simulation uses fixed-step or variable elapsed-time updates;
- which timer, sleep, synchronization, or window-framework mechanism paces
  execution;
- whether vertical synchronization affects simulation frequency;
- whether monitor refresh rate can alter gameplay speed;
- whether the loop measures accumulated elapsed time;
- whether it catches up by running multiple simulation steps after a delay;
- whether it drops/loses simulation time after a delay;
- whether it caps the maximum catch-up work;
- behavior when the PC cannot maintain the target rate;
- behavior when the PC is much faster than the target rate;
- whether rendering can be skipped independently of simulation;
- whether pause stops simulation while rendering/UI remains responsive.

Include exact files, methods, constants, and relevant code flow.


QUESTION 2: WHAT ONE C# TICK MEANS

Inventory major systems that advance one unit per RunOneStep call.

Determine which behavior is expressed in:

- units per simulation tick;
- countdown ticks;
- modulo tick decisions;
- random decision per tick;
- wall-clock time;
- audio sample time;
- render interpolation.

Include at least:

- Blaster movement;
- player-shot movement;
- firing cooldown;
- enemy movement;
- enemy firing;
- Egg approach/release;
- Tanker behavior;
- Spiker behavior;
- rim behavior;
- collision order;
- death/re-entry;
- exit delay;
- between-level transition;
- Super Zapper;
- destruction-effect lifetime;
- Game Over presentation;
- playback step boundaries.

The purpose is not to re-investigate every mechanic. It is to establish how
strongly gameplay behavior depends on the global simulation frequency.


QUESTION 3: HEADLESS AND PLAYBACK TIMING

Trace timing behavior for:

- normal interactive play;
- gameplay recording;
- visible playback;
- paused playback;
- single-step forward;
- reverse reconstruction;
- headless playback;
- automated tests;
- campaign soak tests.

Determine:

- whether headless playback runs without real-time throttling;
- whether it produces the same deterministic simulation states as interactive
  playback;
- whether visible playback uses the normal host pacing;
- whether pause/step operates in simulation boundaries rather than elapsed-time
  units;
- whether changing real-time pacing should leave recording files valid;
- whether recordings contain timing/delay information or only per-tick input;
- whether any current tests assume exactly 60 wall-clock frames per second.

Distinguish simulation determinism from real-time presentation speed.


QUESTION 4: ARCADE HARDWARE AND ROM CADENCE

Using the local ROM disassembly and any checked-in authoritative hardware
references, determine the original arcade timing structure.

Trace:

- CPU clock assumptions if documented;
- periodic interrupt frequency;
- interrupt handler;
- interrupt acknowledgement;
- hardware timing bits read by the program;
- AVG-done polling;
- display-list generation;
- main gameplay loop;
- any frame counter;
- any divider/counter controlling gameplay updates;
- any alternating-frame or skip-frame logic;
- player input sampling cadence;
- enemy update cadence;
- shot update cadence;
- sound update cadence;
- display refresh/presentation cadence.

Do not assume that:

- one hardware IRQ equals one gameplay update;
- one AVG display equals one gameplay update;
- one 60 Hz monitor refresh equals one complete gameplay simulation step.

Trace the actual ROM control flow enough to establish the relationship.


QUESTION 5: ARCADE EFFECTIVE GAME-TICK RATE

Provide the most defensible answer for the effective cadence of the arcade
gameplay loop.

Possible outcomes may include:

- one gameplay update per nominal display frame;
- one gameplay update every two display frames;
- mixed subsystem cadences;
- a main update rate plus higher-frequency interrupt services;
- unresolved because the annotated listing does not establish the hardware
  timing sufficiently.

If mixed cadences exist, report them separately.

Where possible, derive:

- nominal updates per second;
- nominal milliseconds per update;
- relevant counters/dividers;
- exact addresses and branches.

Show the derivation.

Do not infer an exact decimal rate from "approximately 60 Hz" unless the source
provides enough information.


QUESTION 6: C# VERSUS ARCADE COMPARISON

Compare the current project with the arcade across:

- main simulation cadence;
- display cadence;
- input sampling;
- shot updates;
- enemy updates;
- timers;
- audio triggers;
- collision ordering;
- presentation effects.

Determine whether the C# 60-Hz model is:

- a direct match;
- an intentional translation of a different arcade cadence;
- approximately equivalent in real-time despite different tick units;
- partly calibrated by manual play rather than literal clock translation;
- or unsupported by sufficient evidence.

Identify existing C# constants that may already compensate for a different tick
rate.

Do not recommend changing those constants in this task.


QUESTION 7: MEDIUM DIFFICULTY

Trace what the arcade medium/default difficulty setting changes.

Determine whether difficulty changes:

- global CPU/game speed;
- wave enemy counts;
- enemy movement;
- enemy firing;
- shot behavior;
- release timing;
- any other gameplay parameter.

Keep hardware/game cadence separate from difficulty unless the ROM explicitly
connects them.

Compare the current C# project's fixed behavior with the ROM's medium/default
difficulty path.

Do not implement DIP switches or difficulty selection.


QUESTION 8: COULD TIMING EXPLAIN THE FEEL DIFFERENCE?

Evaluate whether there is evidence that the C# game globally runs too fast,
too slowly, or at the correct real-time rate.

Use evidence such as:

- existing manual acceptance of transition durations;
- playback timing;
- accepted enemy movement/rim speed;
- shot cadence;
- destruction animation timing;
- measured host-loop constants;
- ROM cadence derivation.

Distinguish:

1. global simulation-rate difference;
2. individual mechanic calibration difference;
3. ordering difference;
4. spawn/release-pacing difference;
5. difficulty-setting difference;
6. subjective/perceptual difference not yet isolated.

Do not claim the cause has been found unless the evidence establishes it.


QUESTION 9: FUTURE `--speed` FEASIBILITY

Assess a future command-line multiplier such as:

- `--speed 2`
- `--speed 0.5`
- `--speed 1`

Do not implement it.

Recommend precise semantics.

Prefer evaluating a model where:

- per-tick simulation mechanics remain unchanged;
- only real-time pacing changes;
- 1.0 is the accepted normal rate;
- 2.0 runs simulation at twice normal real-time speed;
- 0.5 runs at half normal real-time speed;
- deterministic simulation state at tick N remains the same for a given seed
  and input sequence;
- rendering remains visually usable;
- headless playback remains unthrottled unless explicitly requested otherwise.

Investigate implications for:

- input sampling;
- recording and playback;
- pause and stepping;
- reverse scrub;
- audio pitch/rate versus event timing;
- window/render refresh;
- CPU usage;
- catch-up behavior;
- screenshots;
- command-line validation;
- zero, negative, NaN, infinity, and extreme values;
- tests;
- documentation.

Determine whether changing the wall-clock interval alone is sufficient or
whether the current loop architecture would require a fixed-step accumulator.

Do not propose multiplying gameplay movement constants as the implementation
strategy unless there is a compelling reason. That would alter simulation
semantics rather than presentation speed.


OPTIONAL READ-ONLY MEASUREMENT

If narrowly useful and possible without modifying tracked files, perform
read-only/static calculations from existing constants.

Existing tests or short noninteractive commands may be run only when they
answer a specific timing question.

Do not add diagnostics, instrumentation, or source changes.

Do not launch the interactive game.

Do not run long real-time experiments.

Do not benchmark the user's PC as a substitute for tracing the pacing logic.


NO IMPLEMENTATION

Do not:

- add `--speed`;
- change the host loop;
- change the frame target;
- change any gameplay constant;
- change movement;
- change firing;
- change enemy behavior;
- change release timing;
- change audio;
- change playback;
- modify tests;
- update durable documentation;
- stage, commit, or push.

This task is strictly investigation and recommendation.


REPORT

Preserve this complete substantive prompt under the ignored AI-Logs/Prompts
directory according to project policy.

Create a paired date-prefixed report under ignored AI-Logs/Reports.

The report must include:

1. Starting Git state.
2. Files, tests, documentation, and ROM regions inspected.
3. Current interactive host-loop timing.
4. Exact meaning and duration of one C# simulation tick.
5. Fixed-step/variable-step/catch-up behavior.
6. Rendering and VSync relationship.
7. Headless, recording, and playback timing.
8. Arcade interrupt/display/gameplay cadence.
9. Arcade effective gameplay-update derivation.
10. Medium/default difficulty behavior.
11. C# versus arcade timing comparison.
12. Whether global speed plausibly explains Neal's observed difference.
13. Other stronger candidate explanations.
14. Feasibility and recommended semantics of `--speed`.
15. Smallest future implementation architecture.
16. Proposed automated tests for a future speed option.
17. Proposed manual acceptance tests.
18. Unknowns and unresolved evidence.
19. Final Git state proving no tracked change.

Preserve evidence classification throughout.


FUTURE TEST RECOMMENDATIONS

For the non-implemented future option, recommend tests covering:

- default 1.0 behavior unchanged;
- option parsing and invalid values;
- deterministic state after N ticks independent of multiplier;
- wall-clock target interval calculation;
- pause and single-step semantics;
- recording/playback compatibility;
- reverse reconstruction;
- input sampling behavior;
- visible playback;
- headless behavior;
- audio behavior;
- frame/render behavior;
- no regression to existing gameplay tests.

Do not create these tests now.


MANUAL ACCEPTANCE RECOMMENDATIONS

Recommend a future bounded manual test plan using:

- normal 1.0 speed;
- 0.5 speed;
- 2.0 speed;
- a short fresh recording;
- visible playback;
- pause/step/reverse;
- comparison of real-time duration without changing simulation outcomes.

Make clear that a speed-control feature would be a diagnostic/convenience
feature, not evidence that the accepted normal game speed is wrong.


STOP POINT

Stop after the read-only investigation and recommendation.

At completion, provide Neal a concise summary answering:

- How is one C# tick paced today?
- What arcade cadence does the ROM/hardware establish?
- How closely does C# match it?
- Is there evidence that global speed causes the harder/longer feel?
- What does medium difficulty change?
- Can `--speed` be implemented without changing deterministic gameplay?
- What is the smallest safe implementation if Neal chooses to proceed later?

TEMPEST_SIMULATION_TIMING_SPEED_ROM_INVESTIGATION_20261108