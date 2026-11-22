Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Correct the deterministic Level 11 runtime-representation crash

Continue from the current uncommitted Level 11 production candidate.

Neal launched:

dotnet run --project src\Tempest.Game -- --level 11

Level 11 rendered and ran for approximately two seconds, then crashed deterministically with:

System.InvalidOperationException: Level 3 runtime representation mismatch

The exception was raised by:

Level3WaveLedger.ValidateRuntimeRepresentations

The exact discrepancy was:

- expected identity Level3Identity { Kind = Original, Id = 4 } at ActiveEnemy;
- actual runtime representations contained no representation for original identity 4;
- all other original identities 0 through 26 remained represented.

The stack ended through:

- TempestGame.ValidateLevel3WaveRepresentation()
- TempestGame.RunOneStep(…)
- Program.Main(…)

This is a blocking Level 11 lifecycle/identity-conservation defect.

Do not treat it as a rendering, Raylib, audio, or presentation problem.

## Objective

Determine exactly how original identity 4 disappears from Level 11 runtime representation, then apply the smallest justified correction.

The corrected implementation must preserve exact identity conservation through admission, hatch, active-enemy placement, deferred/pending placement, rejection, destruction, cargo handling, and reconstruction.

Do not disable or weaken ValidateRuntimeRepresentations.

Do not catch or suppress the exception.

Do not remove identity-validation coverage.

Do not solve the problem by changing Level 11 supply, deleting an obligation, or fabricating terminal resolution.

## Historical comparison

The project previously corrected a similar class of lifecycle failure during Level 6 work, recorded in the historical pending-lane/Spike crash investigation and correction reports.

Review the relevant tracked records and implementation/tests, including the Level 6 pending-lane Spike correction.

Determine whether the Level 11 failure is:

- the same previously corrected lifecycle gap exposed through a new Level 11 admission path;
- a parallel gap in direct Fuseball admission;
- a lane-selection or Spike-avoidance rejection gap;
- an open-tube lane-count translation problem;
- an active enemy removed before replacement representation is installed;
- or another precisely established lifecycle defect.

Do not copy the old fix blindly.

Use the historical correction to understand the invariant and accepted lifecycle pattern.

## Reproduce headlessly and deterministically

Create the smallest bounded deterministic reproduction of the Level 11 crash.

Do not launch the interactive game.

Identify for original identity 4:

1. selected enemy type;
2. prior valid representation;
3. admission/hatch path entered;
4. selected lane and any lane-remapping result;
5. Spike/lane eligibility decision;
6. pending/deferred state, if any;
7. exact mutation that removes the identity;
8. expected replacement representation;
9. why that replacement is absent when validation runs.

Use temporary bounded instrumentation only if existing state inspection is insufficient.

Remove temporary diagnostic output before completion unless a narrowly useful development diagnostic is justified.

Do not ask Neal to rerun Level 11 unless the automated reproduction genuinely cannot expose the needed state.

## Level 11-specific areas to inspect

Investigate the interaction among:

- Level 11’s 27 generic pending identities;
- dynamic enemy selection;
- direct Fuseball eligibility;
- seven physical enemy slots;
- open 15-lane geometry;
- Level 11 Spike configuration;
- admission and hatch capacity;
- lane selection/remapping;
- pending-lane behavior;
- enemy activation;
- wave-ledger representation transitions.

Do not assume Fuseball is responsible merely because direct Fuseballs begin at Level 11.

Establish the actual type of identity 4 from deterministic evidence.

## Required invariant

At every validation boundary, every unresolved identity must have exactly one valid representation.

Examples include, as applicable:

- Reserve;
- Egg;
- pending/deferred activation;
- ActiveEnemy;
- latent Tanker cargo;
- cargo re-entry state;
- terminal resolved state.

No unresolved identity may disappear between containers.

No identity may be represented twice.

If activation cannot complete in the current step, preserve the identity in the correct pending/waiting representation rather than dropping it.

## Scope protection

Do not change the recovered Level 11 evidence unless the crash proves a translation error.

Preserve:

- Level 11 shape row 8;
- all recovered ordered geometry;
- open topology;
- 15 playable lanes;
- supply 27;
- Flipper program $24;
- Tanker cargo $07/$24 depth behavior;
- dynamic enemy-selection limits;
- seven-slot capacity;
- Spike parameter $B0;
- direct Fuseball eligibility;
- Level 9-to-11 production progression;
- Level 12 terminal boundary.

Do not change Atari presentation, HUD, calibration, viewport, Approach, ExitZoom, audio, scoring, lives, or unrelated Levels 1-10 behavior.

Do not perform presentation calibration.

Do not inspect screenshots, videos, UserData, Temp, or ROM files. The crash correction does not require new ROM evidence.

## Regression coverage

Add a focused test that reproduces the exact original-identity-4 disappearance or the underlying general lifecycle sequence.

Verify at minimum:

1. The previous deterministic Level 11 sequence no longer throws.
2. Original identity 4 remains represented throughout the triggering transition.
3. The selected enemy type follows the corrected lifecycle.
4. Failed/deferred placement retains exactly one valid representation.
5. Successful later activation uses the same identity.
6. No duplicate identity allocation occurs.
7. Total identity count remains exactly 27.
8. Every unresolved identity has exactly one runtime representation.
9. Direct Fuseball admission remains enabled and correct if involved.
10. Spike/lane rejection behavior remains correct if involved.
11. Open-tube lane mapping remains valid.
12. Seven-slot capacity remains enforced.
13. Death/re-entry reconstruction remains conserving.
14. Tanker cargo ownership remains intact.
15. Completion accounting remains correct.
16. Existing Level 3 mixed-wave/ledger tests remain valid.
17. The historical Level 6 pending-lane/Spike regression remains valid.
18. Existing Level 11 tests remain valid.
19. Levels 1-10 regressions remain unchanged.
20. Presentation and HUD state remain unaffected.

Run a bounded deterministic Level 11 soak long enough to pass well beyond the prior approximately two-second crash point.

## Validation

Run bounded:

- the exact new crash regression;
- Level 11 focused tests;
- Level 3 mixed-wave and wave-ledger tests;
- historical pending-lane/Spike regressions;
- relevant enemy admission, Fuseball, Flipper, Tanker, Spike, reconstruction, and completion tests;
- complete solution test suite;
- Release solution build;
- git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

## Manual handoff

After automated validation, provide the same direct command:

dotnet run --project src\Tempest.Game -- --level 11

Also provide the Atari-presentation Level 11 command.

Neal’s next manual test should first verify that Level 11 runs beyond the previous crash point. Gameplay and presentation acceptance remain pending until that crash gate passes.

## Records and Git

Save this prompt and the correction report under the tracked AI-Logs convention.

Do not stage, commit, or push.

Preserve unrelated worktree changes and local/protected material.

Stop after the bounded correction, automated validation, and manual handoff.

TEMPEST_LEVEL11_RUNTIME_REPRESENTATION_CRASH_CORRECTION
