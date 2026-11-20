IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using any tools, reading project files, running commands, or changing the repository, locate the unique completion token included near the end of this user-provided task.

The completion token begins with "TEMPEST_" and appears on a line by itself.

Verify that exactly one complete token beginning with "TEMPEST_" is present in the user-provided task.

VS Code or the agent environment may append reminders, tagged-file information, attachment metadata, or other system-generated context after the user-provided task. Such automatically appended material does not invalidate the completion token.

The purpose of this check is to detect an incomplete or truncated user-provided task, not to reject context automatically appended by the agent environment.

If you cannot find exactly one complete token beginning with "TEMPEST_" in the user-provided task, STOP immediately without using tools or taking any project or repository action and explain why.

If verification succeeds, proceed with the task below.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Required starting checkpoint:
dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

MILESTONE

Implement production Level 6 and natural Level 5→6 progression.

Use the completed preimplementation analysis:

AI-Logs\Reports\2026-11-04-level6-preimplementation-analysis-report.md

Read that report completely before modifying files.

Levels 1–5 are the manually accepted production foundation. Preserve their established behavior.

Do not launch the interactive game.

Do not commit or push.

After automated validation, stop and provide the exact launch command and manual acceptance checklist for Neal.

STEP 1 - VERIFY THE BASELINE

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat

HEAD must be exactly:

dbf00313b609f81ff26860fd0fcdf0bd0f46c3b0

The tracked worktree and index must be clean before implementation begins.

If HEAD differs or tracked/staged changes exist, STOP and report the discrepancy.

Do not clean, reset, restore, stash, or alter unexpected changes.

STEP 2 - READ THE REQUIRED AUTHORITY

Read:

.github/copilot-instructions.md
AI-Logs\Reports\2026-11-04-level6-preimplementation-analysis-report.md

Read the relevant current sections of:

TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
README.md

Inspect the existing Level 4, Level 5, progression, firing, approach, Spike-exit, production-wave, reconstruction, and campaign-soak implementations and tests as needed.

Inspect the cited Incitti and Atari ROM reference locations from the analysis report when necessary.

Do not repeat the entire analysis unless implementation exposes a genuine contradiction.

If the source inspected during implementation materially contradicts the approved analysis, STOP before making the conflicting behavior change and report the contradiction.

APPROVED IMPLEMENTATION DECISIONS

The following decisions are approved for this milestone:

1. Preserve the existing accepted C# production enemy picker.

The preimplementation analysis found that candidate 3 is accepted as a Tanker by the existing C# picker while Incitti rejects that candidate.

Do not redesign or source-normalize the shared picker.

Do not alter accepted Levels 3–5 composition behavior.

Use the existing accepted production selection policy for Level 6.

Document this as preservation of accepted C# behavior, not literal Incitti equivalence or new arcade verification.

2. Configure Level 6 with its source-specific 71-update Avoid Spikes interval.

Do not copy the existing 151-update Level 4/5 interval into Level 6.

Preserve the accepted 151-update behavior for Levels 4 and 5.

Add the narrowest explicit configuration seam necessary so Level 6 can select 71 updates without changing earlier levels.

Do not derive this from a broad displayed-level threshold.

Do not alter the accepted firing, movement, warning, Spike damage, swept collision, reverse/retry, life-loss, or in-flight-shot policies merely to support the new interval.

3. Add a direct Level 6 development selector.

Add:

--level6-test

This selector must:

- use the exact production Level 6 configuration;
- use the normal production simulation, renderer, audio, enemies, accounting, reconstruction, Super Zapper, and Spike-exit paths;
- enable the same production Flipper firing phase and Level 6 firing configuration used by natural Level 6;
- start directly on fresh Level 6;
- remain isolated from natural campaign progression;
- remain terminal after Level 6 completion;
- replay fresh Level 6 on completion or restart at Level 6 after Game Over, following the established direct-level diagnostic pattern;
- compose with the existing appropriate general options such as --lives;
- not define a second composition or diagnostic-only wave.

Do not add Level 6 deterministic visible-playback support. That is outside this milestone.

LEVEL 6 PRODUCTION CONFIGURATION

Add one immutable production Level 6 configuration using the Original Arcade Level 6 triangle from:

Reference\incitti\levelsets\levelset1.txt

Required configuration:

- Internal progression identity: 5
- Displayed level: 6
- Board identity: Original Arcade triangle / board index 5
- Palette: 0
- Closed tube
- 16 ordered vertices
- 16 playable lanes
- Center Y: 360
- Y offset: -40
- Use the exact reference triangle coordinates, not an approximation
- Use the established depth/position policy supported by the analysis and current production levels
- Use the appropriate existing endpoint-hinge/directed geometry policy
- 22 original enemy obligations
- Four initial Eggs
- 18 reserve obligations
- Flippers eligible
- Tankers eligible
- Spikers eligible
- Fuseballs excluded
- Pulsars excluded
- 16 starting Spikes
- Each starting Spike has height 60
- Spike-avoid exit enabled
- Avoid Spikes interval: 71 updates

Do not invent a fixed number of Flippers, Tankers, or Spikers. Their counts remain controlled by the accepted production selection policy.

LEVEL 6 FLIPPER AND FIRING VALUES

Level 6 must use its source-derived Flipper timing values rather than copying Level 5:

- Turn speed: 9 degrees per simulation update
- Below-rim lane pause: 20 updates
- Rim pause: 23 updates

Preserve the current accepted climb behavior unless the analysis report specifically establishes a required Level 6 value.

Extend the production Flipper firing configuration for displayed Level 6:

- Successful-shot holdoff: 65 updates
- Maximum active enemy-shot capacity: 3

Preserve all existing Levels 1–5 firing values exactly.

Preserve the accepted independent production Flipper firing phase, initial-ready behavior, transition emission gate, height cutoff, probability rules, bullet/audio behavior, and shared active-shot pressure with Tanker bullets.

Do not reintroduce the retired Incitti movement-bound firing model.

Do not retune Tanker or Spiker firing unless a narrow Level 6 range guard must be extended without changing their accepted policy.

NATURAL LEVEL 5→6 PROGRESSION

Extend production progression so a successful Level 5 completion installs Level 6 only after the established safe progression boundary:

- the Level 5 production wave and identity/cargo obligations are resolved;
- the configured Spike/Avoid Spikes exit proceeds normally;
- successful exit travel reaches the existing progression commit point.

The Level 5→6 transition must:

- carry score unchanged;
- carry remaining lives unchanged;
- clear outgoing runtime entities and pending targets through the existing fresh-level path;
- install a fresh Level 6 wave and ledger;
- install the configured 16 height-60 starting Spikes;
- restore two fresh Super Zapper charges;
- clear stale Super Zapper flash and pending target state;
- enter the existing LevelApproach presentation;
- show mixed-color stars because the exited level is displayed Level 5;
- permit established spinner/keyboard/controller rim movement during approach;
- suppress Fire and Super Zapper during approach;
- restore ordinary Level 6 play after arrival without a projection, board, or Blaster-position discontinuity.

Do not create a special Level 5→6 transition implementation if the existing approach and initialization systems can be reused.

LEVEL 6 TERMINAL POLICY

For this bounded milestone, successful production Level 6 completion is terminal.

Do not install Level 7.

The terminal presentation must identify Level 6 completion and that the next level is not implemented, following the established terminal-production pattern.

A restart from natural terminal Level 6 must begin a fresh Level 1 campaign with reset score and configured starting lives, following the established natural-campaign policy.

The direct --level6-test selector must replay/restart Level 6 instead of starting a natural campaign.

Document terminal Level 6 as a temporary PROJECT DECISION.

The reference continuing into Level 7 clover does not authorize Level 7 implementation in this milestone.

REUSE REQUIREMENTS

Reuse the existing accepted systems unless a narrow extension is required:

- tube rendering and projection;
- directed endpoint-hinge geometry;
- player controls and spinner behavior;
- player firing;
- Flipper gameplay and production firing phase;
- Tanker selection, firing, cargo, identity ownership, and reconstruction;
- Spiker selection, Spike construction, retreat/reselection, and firing;
- production identity ledger and accounting;
- all-rim completion;
- death presentation and reconstruction;
- Super Zapper;
- audio events and playback;
- between-level approach;
- mixed-color starfield policy;
- Avoid Spikes warning and exit;
- Spike shooting, scoring, swept collision, reverse/retry, and persistent damage;
- normal campaign restart policy.

Do not replace working implementations merely because a cleaner abstraction is possible.

Keep code comments concise and maintainability-focused under the project instructions.

AUTOMATED VALIDATION REQUIREMENTS

Add focused automated coverage for at least:

1. Exact Level 6 identity, displayed level, board, palette, topology, ordered coordinates, center, offset, lane count, player start policy, supply, Egg counts, enemy eligibility, Spike count/height, and exit policy.
2. Level 6 Flipper turn speed and pause values.
3. Level 6 firing holdoff and maximum active-shot capacity.
4. Preservation of exact Levels 1–5 firing values.
5. Natural Level 5→6 successor resolution.
6. Failed/premature Level 5 states not advancing to Level 6.
7. Successful Level 5 Spike exit committing Level 6 only at the established safe progression boundary.
8. Score and lives carrying into Level 6.
9. Fresh Level 6 production ledger/wave.
10. Fresh two-charge Super Zapper state with no stale flash or pending target state.
11. Correct 16 starting Spikes at height 60.
12. Level 6 enemy eligibility and absence of Fuseballs/Pulsars.
13. Level 6 production Flipper firing using 65/3 through the natural and direct paths.
14. Direct --level6-test selecting the exact production Level 6 configuration and production firing behavior.
15. Direct Level 6 isolation from natural campaign progression.
16. Level 6 death/re-entry preserving configuration, unresolved identities, resolved outcomes, Spike persistence/damage, score, lives policy, and established reconstruction behavior.
17. Level 6 Spike shooting, scoring, collision, reverse/retry, and final-life behavior.
18. Level 6 using 71 Avoid Spikes updates.
19. Levels 4 and 5 retaining their accepted 151-update behavior.
20. Mixed-color approach stars for the Level 5→6 transition.
21. Approach gameplay freeze and input policy.
22. Terminal Level 6 behavior with no Level 7.
23. Natural restart from terminal Level 6 returning to a fresh Level 1 campaign.
24. Direct Level 6 replay/restart returning to Level 6.
25. Regression protection for Levels 1–5.
26. Campaign soak extended through successful arrival at Level 6 Playing.

Use existing test patterns and helpers where practical.

Do not overfit tests to private implementation when observable/configuration contracts are sufficient.

Run focused tests during implementation as needed.

FINAL VALIDATION GATE

Before reporting, run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check
git status --short --branch
git diff --stat

Review the complete diff.

Confirm:

- only intended Level 6 implementation, tests, documentation, and required ignored AI prompt/report records were changed;
- Levels 1–5 values and accepted behavior remain protected;
- no unrelated refactoring occurred;
- no generated gameplay-recording file was created or included;
- no reference file was modified;
- no accidental line-ending churn occurred;
- no Level 7 implementation exists;
- no interactive game was launched;
- no commit or push occurred.

DOCUMENTATION

Update the appropriate current sections of:

README.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md

Record:

- Level 6 production configuration and geometry;
- natural Level 5→6 progression;
- enemy eligibility and exclusions;
- Level 6 Flipper timing and firing values;
- explicit 71-update Level 6 Avoid Spikes interval;
- preservation of Levels 4–5 151-update behavior;
- accepted C# picker preservation and its evidence boundary;
- direct --level6-test as DEVELOPMENT ONLY;
- Level 6 terminal boundary as PROJECT DECISION;
- reference continuation to Level 7 remaining unimplemented;
- automated validation results;
- pending manual acceptance.

Preserve the behavior specification's evidence classifications.

Do not label implementation or passing tests as ARCADE VERIFIED.

PROMPT AND REPORT LOGGING

Follow the existing project-wide instructions in:

.github/copilot-instructions.md

Use the established paired names:

AI-Logs\Prompts\2026-11-04-level6-implementation-prompt.md
AI-Logs\Reports\2026-11-04-level6-implementation-report.md

Do not duplicate or alter the global logging policy.

Do not create prompt/report files elsewhere.

MANUAL ACCEPTANCE HANDOFF

Do not launch the interactive game.

After automated validation succeeds, provide Neal with these launch commands:

Direct Level 6 validation:

dotnet run --project src\Tempest.Game -- --level6-test --lives 10

Natural campaign validation:

dotnet run --project src\Tempest.Game -- --lives 10

Provide a concise manual checklist covering:

Direct Level 6:
- correct closed triangle board and HUD;
- correct movement and geometry;
- four initial Eggs and expected mixed production activity;
- Flippers, Tankers/cargo, and Spikers only;
- no Fuseballs or Pulsars;
- appropriate enemy firing without a bullet flood;
- 16 starting Spikes;
- two fresh Super Zapper charges;
- death/re-entry;
- Avoid Spikes warning and noticeably shorter Level 6 wait;
- Spike shooting, collision, reverse/retry;
- terminal completion and direct Level 6 replay/restart;
- no crash or obvious regression.

Natural Level 5→6:
- successful Level 5 Spike exit;
- mixed-color starfield;
- distant triangle Level 6;
- SUPERZAPPER RECHARGE;
- spinner-controlled Blaster during approach;
- seamless Level 6 arrival;
- score/lives carry;
- fresh Level 6 Spikes/wave/Super Zapper;
- normal Level 6 play;
- terminal Level 6 with no Level 7;
- natural restart returns to fresh Level 1;
- no malformed transition, accounting issue, crash, or obvious Levels 1–5 regression.

FINAL REPORT

Report:

1. Verified starting HEAD.
2. Files changed.
3. Exact Level 6 configuration implemented.
4. Exact Level 6 Flipper and firing values implemented.
5. Level 6 Avoid Spikes interval and preservation of Levels 4–5.
6. Natural Level 5→6 progression behavior.
7. Direct --level6-test behavior and production parity.
8. Level 6 terminal/restart behavior.
9. Enemy-picker decision and evidence boundary.
10. Focused validation results.
11. Full solution test count.
12. Build result including warnings/errors.
13. git diff --check result.
14. Final Git status and diff summary.
15. Confirmation that no game was launched.
16. Confirmation that no commit or push occurred.
17. Exact manual launch commands.
18. Concise manual acceptance checklist.
19. Any remaining uncertainty that Neal should observe rather than the implementation guessing.

Stop after reporting.

Do not commit.
Do not begin Level 7.
Wait for Neal's manual acceptance.

TEMPEST_LEVEL6_IMPLEMENTATION_DBF0031