IMPORTANT: Read this entire prompt before taking any action.

This prompt is complete only if you can find the exact completion token at the very bottom of the prompt, appearing exactly once.

Before using any tool, reading or modifying any repository file, or running any command:

1. Read the entire prompt.
2. Locate the completion token at the bottom.
3. Verify that it appears exactly once.
4. If you cannot find it exactly once, STOP immediately and report that the prompt is incomplete.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

CURRENT ACCEPTED CLEAN CHECKPOINT

3a1791c Establish maintainability comment guidance

The immediately preceding accepted gameplay/audio checkpoint is:

8fcfc24 Implement accepted arcade sound effects

The 3a1791c commit changes only repository-wide Copilot maintainability-comment guidance.

LEVEL 5 ANALYSIS

Before implementing anything, read the complete analysis report:

AI-Logs/Reports/2026-11-03-level5-preimplementation-analysis-report.md

Treat that report as the implementation planning basis, but verify important conclusions against the current source and Reference/incitti where appropriate.

MILESTONE

Implement production Level 5 and natural Level 4 -> Level 5 progression.

Do NOT implement Level 6.

Do NOT generalize progression beyond what Level 5 actually requires.

GOAL

Extend the currently accepted production game from natural progression through Level 4 to natural progression through Level 5.

Level 5 should reuse the already accepted gameplay systems wherever possible.

Do not redesign established systems merely because Level 5 is being added.

AUTHORITY ORDER

1. Neal's directly observed Atari Tempest behavior.
2. TEMPEST-BEHAVIOR-SPEC.md.
3. TEMPEST-DEVELOPMENT-NOTES.md.
4. Mark Incitti's implementation and levelset files as engineering/reference evidence.
5. Current accepted C# behavior.
6. New assumptions only where required.

LEVEL 5 REFERENCE CONFIGURATION

Use the completed analysis and verify against:

Reference/incitti/levelsets/levelset5.txt
Reference/incitti/tempest.bmx

The analysis established Level 5 as:

- original arcade board/index corresponding to the Level 5 cross;
- closed geometry;
- 16 playable lanes;
- CenterY = 415;
- YOffset = -90;
- palette 0;
- 20 original enemy obligations;
- four initial Eggs;
- 16 starting Spikes;
- starting Spike height 60;
- Flippers enabled;
- Tankers enabled;
- Spikers enabled;
- Pulsars disabled;
- Fuseballs disabled.

Do not silently substitute inferred values if the source provides more precise data.

If verification reveals a material disagreement with the analysis report, STOP and report it before implementing the conflicting behavior.

IMPLEMENTATION SCOPE

1. Add a production Level 5 configuration following the established LevelConfiguration model.

2. Represent Level 5 geometry using the existing level/geometry architecture.

Prefer the existing loader/model rather than creating Level-5-specific rendering code.

3. Extend production progression:

Level 1
  ->
Level 2
  ->
Level 3
  ->
Level 4
  ->
Level 5

4. Level 5 remains terminal for this milestone.

After successful Level 5 completion, do NOT install Level 6.

Preserve the project's established terminal-development behavior rather than inventing Level 6.

5. Generalize successful Level 4 Avoid Spikes exit only as much as necessary so that a successful Level 4 exit can install the Level 5 successor.

This is the key bounded generalization identified by the analysis.

Do not redesign the exit state machine.

Preserve:

- Avoid Spikes presentation;
- movement allowed during the waiting period;
- firing blocked during the waiting period;
- firing enabled once the Blaster actually begins exit travel;
- Spike destruction during exit;
- Spike collision/reverse/retry behavior;
- successful outward exit;
- accepted zoom-out audio;
- accepted LevelApproach behavior;
- accepted starfield;
- SUPERZAPPER RECHARGE;
- accepted zoom-in audio;
- spinner-controlled tiny Blaster during approach.

6. When Level 5 begins naturally:

Preserve carried game state where established:
- score;
- lives.

Initialize fresh level state where established:
- new Level 5 enemy/accounting state;
- Level 5 Spikes;
- fresh Super Zapper supply of two charges;
- appropriate release/hatching state;
- appropriate firing state;
- level-specific geometry/palette/presentation.

7. Preserve all established enemy systems.

Level 5 should reuse existing:

- Egg/release behavior;
- Flippers;
- Tankers;
- Tanker cargo;
- Tanker bullets;
- Spikers;
- Spikes;
- enemy bullets;
- death/re-entry reconstruction;
- identity/accounting ledger;
- Super Zapper;
- Hurry-Up;
- all-rim completion;
- player shots;
- collision ordering;
- rim pursuit direction behavior.

Do not create alternate Level-5 versions of these systems unless unavoidable.

8. Level 5 excludes:

- Pulsars;
- Fuseballs.

Protect this explicitly.

9. Preserve the existing documented Flipper-bullet compromise.

Do not introduce new Flipper firing behavior solely because Level 5 is being implemented.

If direct testing later determines that this compromise needs revision, that will be a separate milestone.

PRODUCTION ACCOUNTING

Level 5's 20 original obligations must remain coherent across:

- reserve;
- active Eggs;
- hatched enemies;
- Tanker carrier/cargo identity;
- Spiker lifecycle;
- destroyed identities;
- escaped/resolved identities;
- death/re-entry reconstruction;
- Super Zapper resolution;
- ordinary completion;
- all-rim completion where applicable.

Do not make completion depend merely on the currently visible enemy count.

Reuse the accepted production ledger/invariant architecture wherever possible.

SPIKES

Level 5 begins with the analyzed Spike configuration:

- 16 Spikes;
- starting height 60.

Preserve existing Spike behavior:

- Spiker growth;
- shot shortening;
- one-point-per-successful-hit scoring;
- persistence through the established lifecycle;
- player collision;
- exit behavior.

Do not redesign Spike mechanics.

SUPER ZAPPER

Level 5 starts with two fresh charges, following accepted per-level recharge behavior.

Preserve:

- first-use all-target semantics;
- second-use targeted semantics;
- delayed resolution;
- Tanker/cargo behavior;
- Spiker behavior;
- Spike/bullet exclusions;
- scoring/accounting;
- accepted zap.wav timing.

A life loss must not recharge the Super Zapper.

A new Level 5 reached through natural progression must recharge it.

AUDIO

Checkpoint 8fcfc24 established the accepted arcade sound-effects system.

Preserve all accepted audio behavior exactly, including:

- single persistent PlayerShot voice;
- stop/restart shot.wav from sample zero per accepted shot;
- event timing;
- enemy sounds;
- Spike sound;
- Pulsar sound architecture even though Pulsars are absent on Level 5;
- zap sound;
- death sounds;
- zoom-out;
- zoom-in.

Do not retune audio.

bonus.wav remains deferred because score-threshold extra-life gameplay is not yet implemented.

BETWEEN-LEVEL PRESENTATION

Preserve the accepted approach presentation:

- 288 deterministic stars;
- 1.6x RGB brightness;
- 960 initial approach depth;
- 96-unit perspective bias;
- approximately four seconds from first visible star to arrival at normal cadence;
- SUPERZAPPER RECHARGE;
- spinner-responsive tiny Blaster;
- seamless arrival.

IMPORTANT STARFIELD COLOR RULE

The accepted project rule is:

- exits from Levels 1-4 use blue stars;
- beginning with the EXIT FROM LEVEL 5, use the white/red/green/blue mixture.

Therefore the natural Level 4 -> Level 5 transition should still use BLUE stars.

Do not change it to multicolor merely because the destination is Level 5.

The later Level 5 -> Level 6 exit would be the first multicolor transition, but Level 6 is NOT part of this milestone.

TESTING

Add or extend deterministic tests protecting the new production behavior.

At minimum establish evidence for:

A. LEVEL 5 CONFIGURATION

- production Level 5 is accessible;
- correct displayed/progression identity;
- correct geometry;
- 16 playable lanes;
- correct palette;
- 20 original obligations;
- four initial Eggs;
- 16 height-60 Spikes;
- Flippers enabled;
- Tankers enabled;
- Spikers enabled;
- Pulsars disabled;
- Fuseballs disabled.

B. LEVEL 5 ACCOUNTING

Protect representative lifecycle cases for:

- reserve -> Egg -> enemy;
- destruction;
- Tanker carrier/cargo;
- Spiker lifecycle;
- enemy resolution;
- exact identity conservation;
- completion blockers.

Prefer extending generalized production-ledger coverage over copying large Level-4-specific test bodies.

C. DEATH / RE-ENTRY

Demonstrate that Level 5 death reconstruction:

- preserves score/lives appropriately;
- does not restore destroyed enemies;
- conserves unresolved identities;
- preserves applicable Spike state;
- does not duplicate Tanker cargo;
- does not incorrectly recharge Super Zapper.

D. NATURAL LEVEL 4 -> LEVEL 5

Test the actual successful progression path.

Establish:

- Level 4 wave completes;
- Avoid Spikes state behaves normally;
- successful exit installs Level 5;
- transition/approach behavior is entered;
- Level 5 begins with correct identity/configuration;
- score carries;
- lives carry;
- fresh two-charge Super Zapper;
- fresh Level 5 wave/accounting;
- appropriate Level 5 Spikes;
- transition stars remain BLUE for Level 4 -> 5.

E. LEVEL 5 COMPLETION

For this milestone:

- successful Level 5 completion remains terminal;
- no Level 6 configuration is installed;
- no Level 6 gameplay begins;
- level/accounting state remains coherent.

Do not implement Level 6 merely to make this test easier.

F. REGRESSION

Protect existing:

- Levels 1–4;
- Level 4 Avoid Spikes;
- accepted player-shot behavior;
- accepted audio events;
- Super Zapper;
- player death/re-entry;
- progression;
- transition presentation.

Do not add pixel-perfect graphical tests.

VALIDATION

After implementation:

1. Run focused Level 5/configuration/progression tests.

2. Run relevant affected production-ledger, Level 4, transition, death/re-entry, Super Zapper and audio tests.

3. Run the complete solution:

dotnet test TempestCSharp.slnx

4. Build:

dotnet build TempestCSharp.slnx

5. Run:

git diff --check

6. Review the complete tracked diff.

7. Confirm:
- HEAD remains 3a1791c;
- nothing is staged;
- nothing is committed;
- TEMP/ remains untouched;
- sfx/ assets remain unchanged;
- AI-Logs remains ignored.

MANUAL VALIDATION

After all automated validation passes, STOP for Neal's manual gameplay acceptance.

Provide a normal launch command with additional lives:

dotnet run --project src\Tempest.Game -- --lives 10

Neal should manually validate natural play through Level 4 and into Level 5.

Primary manual checklist:

1. Levels 1–4 still play normally.
2. Finish Level 4.
3. Avoid Spikes presentation behaves normally.
4. Shoot/dodge Spikes during exit as already accepted.
5. Successful exit begins the accepted blue-star transition.
6. Upcoming Level 5 cross appears correctly during approach.
7. SUPERZAPPER RECHARGE appears normally.
8. Spinner controls the tiny Blaster during approach.
9. Level 5 begins seamlessly.
10. HUD identifies Level 5 correctly.
11. Score carries correctly.
12. Lives carry correctly.
13. Level 5 starts with correct Spikes.
14. Level 5 wave visibly contains appropriate Flipper/Tanker/Spiker behavior.
15. Pulsars do not appear.
16. Fuseballs do not appear.
17. Enemy bullets behave normally.
18. Super Zapper has two fresh charges and behaves normally.
19. Player death/re-entry behaves normally.
20. Completing the Level 5 wave reaches the intended terminal-development behavior and does not begin Level 6.
21. Audio remains correct.
22. No crashes, incorrect board, transition jump, accounting failure, or obvious regression.

Do not claim manual/visual acceptance yourself.

DOCUMENTATION

Update the appropriate tracked project documentation with implemented Level 5 behavior and evidence classification.

The repository-wide .github/copilot-instructions.md already contains the standing maintainability-comment policy.

Follow that policy automatically.

Do not duplicate the generic comment instructions in milestone documentation.

Save the implementation prompt and report under AI-Logs using established conventions.

Do not stage.
Do not commit.
Do not begin Level 6.

At completion report:

- files changed;
- final Level 5 configuration;
- progression change;
- any generalized code;
- confirmation no new gameplay system was needed;
- focused tests;
- affected tests;
- full-suite result;
- build result;
- git diff --check;
- final worktree status;
- manual launch command;
- manual acceptance checklist;
- report path.

COMPLETION TOKEN:
TEMPEST_LEVEL5_IMPLEMENTATION_COMPLETE