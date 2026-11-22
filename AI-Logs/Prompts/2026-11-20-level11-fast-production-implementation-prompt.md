Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking any repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task. VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Implement production Level 11 using the successful Level 10 workflow

Implement Level 11 as the next production Tempest level.

The goal is a quick, tightly bounded implementation similar to the successful Level 10 workflow.

Do not turn Level 11 into a prolonged investigation milestone unless the available Atari evidence reveals a genuine ambiguity that prevents a defensible implementation.

## Starting point

Begin from the current accepted repository baseline after the completed Atari-presentation and Atari vector-HUD milestones.

Before changing files:

- verify branch, HEAD, upstream, ahead/behind, index, tracked worktree changes, and untracked material;
- preserve unrelated tracked/untracked material;
- identify any post-push AI-Logs report update that is pending from the previous milestone and leave it intact.

Do not reset, restore, stash, clean, rebase, or otherwise normalize Neal's worktree.

## Level 11 evidence check

Perform a concise evidence check before implementation.

Use the tracked project source, behavior specification, development documentation, AI-Logs, and tracked Atari/disassembly/reference text as appropriate.

Do not inspect or access:

Reference/arcade/roms.rev2/

Do not use excluded copyrighted local reference material.

Do not search the web for ROM binaries or ROM dumps.

Determine enough Level 11 information to implement the level correctly, including where available:

- genuine Level 11 tube geometry;
- open/closed topology;
- lane count and connectivity;
- Level 11 enemy composition;
- original/reserve enemy supply;
- initial enemy state;
- Spike configuration;
- enemy admission behavior;
- Tanker behavior and cargo;
- ordinary Flipper behavior;
- Tanker-released Flipper behavior;
- level-specific Flipper lane-change/movement program;
- Spiker behavior where applicable;
- Pulsar behavior where applicable;
- Fuseball behavior where applicable;
- enemy firing parameters;
- Super Zapper behavior;
- completion obligations;
- death/re-entry reconstruction;
- natural Level 10 to Level 11 progression;
- Level 11 to the current next-level boundary.

Clearly distinguish:

- ROM/DISASSEMBLY BACKED facts;
- EXISTING ACCEPTED GENERIC behavior;
- PROJECT TRANSLATION values where exact C# translation is necessary;
- genuinely UNRESOLVED items.

Do not re-investigate generic systems that are already accepted unless Level 11 evidence specifically requires different behavior.

## Genuine Level 11 geometry

Implement the actual Level 11 tube geometry.

Do not clone Level 10 geometry and merely change the displayed level number.

Do not reuse another level's tube unless Atari evidence establishes that Level 11 actually uses the same tube layout.

Use the project's established tube-geometry representation and directed-transition architecture.

Preserve gameplay/model geometry independently from presentation transforms.

## Gameplay implementation

Implement Level 11 through the existing production level/configuration architecture.

Reuse accepted generic mechanics whenever the Level 11 evidence does not require different behavior.

Do not redesign:

- enemy lifecycle architecture;
- wave ledger/accounting;
- Tanker cargo ownership;
- death/re-entry reconstruction;
- collision architecture;
- firing architecture;
- score mechanics;
- lives mechanics;
- Super Zapper architecture;
- deterministic random/input behavior.

When Atari evidence supplies Level-11-specific values or programs, use those rather than inventing generic behavior.

Do not substitute Mark Incitti behavior for explicit Atari ROM/disassembly evidence.

If direct Atari evidence and an existing generic implementation materially conflict, report the conflict rather than silently choosing a convenient behavior.

## Flipper movement

Treat below-rim Flipper movement as potentially level-specific.

If Level 11 has an identifiable ROM movement program, implement that program.

Distinguish ordinary Level 11 Flippers from Tanker-released cargo if the evidence distinguishes them.

Do not assume Level 1, Level 7, or another tube's flipping pattern automatically applies to Level 11.

If existing generic machinery already expresses the exact Level 11 program, reuse it rather than duplicating it.

## Presentation

Use the already accepted Atari-presentation architecture.

Do not fine-tune Level 11's tube presentation automatically.

Get Level 11 into the established presentation path and let Neal perform visual calibration afterward using the existing development options:

--tube-y-gain

--tube-scale

--tube-vertical-offset

and, when useful:

--calibration-guides

Do not impose arbitrary aesthetic limits on those existing controls.

Do not alter the accepted Level 1-10 calibration table.

Do not modify Levels 1-10 presentation in order to accommodate Level 11.

Do not change the accepted portrait viewport, perspective formula, HUD layout, or Atari vector HUD.

If Level 11 needs a first-pass presentation entry so it can render through the normal per-level calibration architecture, use a neutral/identity starting calibration unless the architecture requires another established default. Do not spend this milestone visually tuning it.

## HUD

The accepted Atari vector HUD is frozen.

Level 11 must display:

- live current score;
- fixed high score 38930;
- fixed initials AAA;
- live displayed level 11;
- correct reserve-life Blasters.

Do not change HUD glyphs, position, colors, calibration defaults, score formatting, lives semantics, or HUD diagnostic controls.

Digit 1 must retain its accepted one-vertical-line geometry.

## Progression

Add Level 11 to the production progression architecture.

Verify:

- direct --level 11 startup;
- natural Level 10 to Level 11 progression;
- score and lives carry correctly;
- displayed level changes correctly;
- Super Zapper state follows the established between-level rules;
- Atari HUD follows the active level;
- Atari-presentation calibration resolves the active level correctly.

Do not implement Level 12 in this task.

At completion of Level 11, stop at an explicit safe Level-12-not-implemented boundary using the project's established terminal/development behavior.

Do not fabricate Level 12 simply to permit continuation.

## Playback and deterministic behavior

Integrate Level 11 with existing deterministic/headless/visible playback infrastructure where the architecture already supports new production levels.

Do not redesign recording format.

Do not create a new playback version unless genuinely required.

Do not break existing Level 1-10 recordings.

If existing multi-level recordings end before Level 11, do not fabricate recorded input merely to satisfy a test.

Use deterministic simulation/headless coverage where appropriate.

## Scope protection

Do not use Level 11 implementation to fix:

- whole-game difficulty/timing;
- Levels 1-10 fine tuning;
- deferred HUD differences;
- audio balance;
- high-score persistence;
- initials entry;
- leaderboard behavior;
- attract mode;
- later Levels 12-16.

If Level 11 testing exposes an unrelated defect, record it separately and keep this milestone bounded.

## Visual assets

Do not search for, enumerate, open, inspect, or analyze screenshots, images, videos, frames, UserData, Temp, or other visual assets.

No visual evidence is required for this implementation prompt.

Neal will perform the visual/gameplay review manually.

## Automated validation

Add focused Level 11 coverage appropriate to the actual implemented mechanics.

At minimum verify:

1. Direct Level 11 initialization.
2. Genuine Level 11 geometry representation.
3. Required lane/connectivity properties.
4. Correct Level 11 enemy composition and supply.
5. Correct initial state.
6. Level-specific Spike configuration where applicable.
7. Level-specific enemy firing configuration.
8. Ordinary Flipper movement behavior where applicable.
9. Tanker cargo behavior and cargo Flipper movement where applicable.
10. Spiker/Pulsar/Fuseball behavior where applicable.
11. Enemy admission/concurrency obligations using existing architecture.
12. Completion accounting.
13. Death/re-entry conservation/reconstruction.
14. Super Zapper behavior.
15. Natural Level 10 to Level 11 progression.
16. Score/lives carry across the transition.
17. Correct displayed Level 11 HUD state.
18. Atari-presentation path accepts Level 11.
19. Existing tube diagnostics operate on Level 11.
20. Gameplay/model state is unchanged by presentation diagnostics.
21. Level 11 terminates safely at the unimplemented Level 12 boundary.
22. Existing Levels 1-10 regressions remain intact.
23. Playback/recording deterministic behavior remains intact.
24. Existing Atari HUD regressions remain intact.
25. Approach and ExitZoom regressions remain intact.

Run bounded:

- focused Level 11 tests;
- relevant progression/enemy/lifecycle/reconstruction tests;
- relevant presentation/HUD/Approach/ExitZoom/playback regressions;
- complete solution test suite;
- Release solution build;
- git diff --check.

Use established hang timeouts and project security/stall behavior.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide Neal concise commands for:

1. Direct Level 11 normal/legacy presentation.
2. Direct Level 11 Atari presentation.
3. Level 11 Atari presentation with --calibration-guides.
4. Level 11 Atari presentation demonstrating the existing tube calibration options.
5. Any existing Level-11-relevant diagnostic/test option genuinely used by the implementation.

Do not invent unnecessary new calibration controls.

Neal will manually evaluate:

- genuine tube shape;
- overall playability;
- enemy composition;
- Flipper/Tanker-cargo movement pattern;
- Spike behavior;
- firing;
- Super Zapper;
- death/re-entry;
- HUD level 11;
- tube presentation.

If presentation needs adjustment, Neal intends to use the existing presentation controls rather than asking this implementation task to fine-tune it.

## Records and Git

Save this substantial task prompt and implementation report under the tracked AI-Logs convention.

Do not stage, commit, or push this Level 11 candidate.

Do not modify unrelated historical AI-Logs.

Preserve protected/local material.

Stop after implementation, automated validation, and manual-test handoff.

TEMPEST_LEVEL11_FAST_PRODUCTION_IMPLEMENTATION