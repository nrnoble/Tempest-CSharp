# Tempest diagnostic Level 10 implementation

## Objective

Create a deliberately minimal, diagnostic Level 10 implementation based as closely as practical on the existing Level 7 implementation.

This is **not** an arcade-fidelity Level 10 milestone.

The sole purpose is to give Neal a Level 10 that can be manually played and/or deterministically played back under --atari-presentation so he can answer one diagnostic question:

**Does Level 10 exhibit the same apparent depth-dependent enemy-motion defect currently visible on Level 7 with** **--atari-presentation** **enabled?**

The current Level 7 defect is under active investigation. Do not attempt to diagnose or correct it during this task.

## Critical scope constraint: leave Level 7 alone

Treat the existing Level 7 implementation and its current behavior as frozen diagnostic evidence.

Do not:

- fix Level 7;
- tune Level 7;
- change its gameplay;
- change its geometry;
- change its presentation;
- change its timing;
- change its enemy movement or firing;
- alter its recorded-playback behavior;
- refactor Level 7 merely to make the Level 10 implementation cleaner;
- make shared changes intended to correct the known Level 7 Atari-presentation defect.

If a shared infrastructure change is genuinely necessary merely to make Level 10 selectable/runnable, keep it as small and mechanical as possible and demonstrate that Level 7 has not changed behavior.

## Level 10 is intentionally diagnostic, not authentic

Do not research or reconstruct arcade Level 10 behavior for this task.

Do not spend time making Level 10's:

- enemy population,
- difficulty,
- enemy timing,
- firing behavior,
- movement programs,
- presentation,
- tube calibration,
- perspective,
- HUD,
- progression behavior,
- or other level-specific characteristics

match the arcade's actual Level 10.

Instead, make Level 10 behave as closely as practical like the existing Level 7 implementation.

Where practical, reuse existing Level 7 configuration or behavior rather than independently redesigning or approximating it.

The goal is to minimize uncontrolled differences between the Level 7 and Level 10 test cases.

Do not introduce architectural cleanup or generalization merely because duplication or special-case diagnostic code looks inelegant. Maintainability still matters, but experimental validity and bounded scope are more important in this task.

Clearly identify genuinely diagnostic/project-specific behavior in comments where necessary so it is not later mistaken for arcade-authentic Level 10 behavior.

## Presentation

Level 10 must be usable with the existing --atari-presentation path.

Do not tune Level 10 visually during this task.

Do not attempt to establish the correct arcade Level 10 projection or placement.

For this experiment, use the Level 7-compatible presentation path/configuration as directly as the current architecture reasonably permits.

The important requirement is that Neal can observe enemy motion through depth under Atari presentation and compare it with the existing Level 7 behavior.

Legacy/non-Atari presentation must not be unnecessarily disturbed.

## Direct startup and playback

Provide the smallest reasonable way to start Level 10 directly using the project's established direct-level mechanism.

If the existing recorded-input/playback system can support Level 10 through a small, bounded extension, make that extension so Neal can perform observation-focused deterministic playback rather than having to concentrate on manual play.

Do not redesign or extend the general playback system beyond what is necessary for this Level 10 experiment.

The resulting Level 10 should be usable for an Atari-presentation ON/OFF comparison.

## What NOT to investigate

Do not investigate the root cause of the Level 7 visual-motion defect in this task.

Do not repeat the existing framebuffer investigation.

Do not reinterpret the prior Level 7 simulation-versus-presentation evidence.

Do not use this task as an opportunity to change the Atari-presentation architecture.

Do not retune already accepted Level 1-6 presentation.

Do not touch unrelated outstanding defects, including Blaster exit presentation.

Do not implement later levels or broader campaign progression.

## Automated validation

Add or update only the focused automated coverage necessary to establish that:

1. diagnostic Level 10 can be selected/initialized;
2. it uses the intended Level-7-like diagnostic configuration/behavior;
3. required playback support works if extended in this task;
4. existing Level 7 behavior has not been intentionally changed;
5. existing regression coverage continues to pass.

Run appropriately bounded focused tests first.

Then run the normal bounded full regression suite and Release build if the environment permits.

Also run the project's normal whitespace/diff validation.

All test or analysis commands must have explicit finite bounds in accordance with the project's existing stall/self-stop policy.

If Windows Application Control or another environmental condition prevents required validation, follow the project's existing fail-safe policy. Do not weaken security, loop on the same blocked artifact, substitute stale binaries, or claim blocked validation passed.

## Manual acceptance

Do NOT launch the interactive game yourself.

Stop after implementation and automated validation.

Provide Neal with the exact commands needed to perform the diagnostic comparison.

The manual experiment should make it straightforward to compare:

1. diagnostic Level 10 without --atari-presentation;
2. the same diagnostic Level 10 with --atari-presentation.

Where the existing tooling permits it, prefer using the same deterministic playback/input for both runs.

Neal will judge only whether Level 10 reproduces the characteristic Level 7 Atari-presentation motion defect.

Level 10 does **not** need to look or play like authentic arcade Level 10 to pass this diagnostic milestone.

## Documentation and Git

This is an experimental diagnostic milestone, not final Level 10 implementation.

Do not rewrite durable behavior specifications to imply that this is arcade-authentic Level 10.

Follow the project's established AI prompt/report logging rules for substantial work.

Preserve this substantive prompt and produce the corresponding implementation/investigation report.

The report must clearly state:

- why diagnostic Level 10 was created;
- how it relates to Level 7;
- files changed;
- any unavoidable differences between the Level 7 and Level 10 paths;
- tests/builds actually run and their exact outcomes;
- anything blocked or incomplete;
- that manual Atari-presentation ON/OFF observation remains pending;
- that no conclusion about the Level 7 defect may be drawn until Neal performs that comparison.

Do not stage, commit, or push anything unless separately authorized by Neal.

At completion, report the final Git status and provide the exact manual launch/playback commands Neal should run next.

Do not perform unrelated work.

TEMPEST_DIAGNOSTIC_LEVEL10_FROM_LEVEL7
