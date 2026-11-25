# Tempest Level 16 Final Closeout Prompt

## Scope

Complete the final Level 16 closeout: encode Neal's accepted Atari-presentation
defaults; reconcile the accumulated Level 16 implementation and evidence;
record manual gameplay and presentation acceptance; update durable project
documentation; run the fresh final validation gate; carefully audit and stage
only Level 16 production, test, and durable-documentation changes; then commit
and push to the configured upstream. Do not add gameplay features, redesign
accepted behavior, or begin Level 17 or later work.

## Accepted behavior and values

- Neal reports that Level 16 gameplay passes manual testing, the runtime
  shared-center Flipper crash is fixed, the Atari-presentation Blaster
  interior-side crash is fixed, and final manual play passes with no reported
  Level 16 gameplay blocker.
- The Level 16 tube presentation is manually accepted. Preserve the exact
  ROM-backed row-11 geometry, closed 16-lane topology, repeated center points,
  and accepted two-lobed/self-touching representation.
- Encode Level 16 Atari-presentation defaults: Y gain `1.1`, scale `1`,
  vertical offset `100`, and depth-Y gain `1`. Keep all four existing
  `--tube-*` runtime overrides functional and do not change Levels 1–15.
- Background opacity `1` remains the global ordinary opaque-black setting.
  Do not add a Level-16-specific opacity default or change the global
  development-only `--background-opacity` diagnostic.

## Reconciliation and boundaries

Read the seven required 2026-11-24 Level 16 historical reports named in the
user request. Preserve their evidence classifications and do not rewrite them.
Retain the implemented Level 16 ROM-backed configuration and its documented
project translations, `$24` 2/3 Flipper route, Tanker/cargo lifecycle,
shared-center ROM-aligned projection, Spiker conversion, Fuseball/Pulsar
policy, firing, identity/reconstruction/completion behavior, direct startup,
natural progression, HUD/state continuity, and terminal campaign boundary.
Do not retune gameplay or change gameplay timing.

Level 15 is not a Level 16 implementation, geometry, behavioral, or
presentation model. Do not copy its values or workarounds. Level 15 may be
checked only to preserve existing behavior. Do not implement, investigate, or
document speculative Level 17+ behavior. Do not launch the interactive game.

## Documentation and tests

Review and update `TEMPEST-BEHAVIOR-SPEC.md`,
`TEMPEST-DEVELOPMENT-NOTES.md`, and `README.md` to distinguish ROM-backed
behavior, project translations/decisions, and manual acceptance; record the
accepted defaults, opaque global background setting, Level 16 campaign
boundary, and any genuinely deferred whole-game issue. Preserve historical
reports.

Review existing Level 16 geometry, gameplay, Flipper, cargo, lifecycle,
progression, Atari presentation, Blaster, calibration, and prior-level tests.
Add only coverage needed for the newly accepted defaults or a demonstrated
gap. Keep presentation changes render-only.

## Repository safety, validation, and delivery

Before modifying or staging, capture branch, HEAD, upstream, ahead/behind,
index, tracked and untracked worktree state, ignored AI-Logs status, and the
complete Level 16 candidate diff. Reconcile ownership; preserve unrelated
Level 15 work, AI-Logs, local references, and user assets. Stop before staging
if any tracked ownership is ambiguous.

Run focused Level 16 validation, affected prior-level regressions, the full
solution test suite with a bounded VSTest hang timeout, a fresh Release build
with zero errors, and `git diff --check`. Report warnings exactly and never
count blocked, timed-out, or incomplete work as passing.

Review the final complete diff. Stage only intended Level 16 production,
tests, and durable documentation; AI-Logs must remain unstaged and
uncommitted. Audit the staged diff and file list, then commit with a concise
Level 16 completion subject and push without force to the configured upstream.
Record the commit, committed files, post-commit and post-push state, and
independent GitHub visibility only if actually verified. Do not claim remote
visibility from a local push alone.

Create the paired final report at
`AI-Logs/Reports/2026-11-24-level16-final-closeout-report.md`. Report initial
and final Git state, ownership, manual acceptance, implementation and
documentation status, tests and validation, diff/staging audit, commit/push,
remaining unrelated work, AI-Logs/assets exclusions, and deferred general
issues.

TEMPEST_LEVEL16_FINAL_CLOSEOUT_DOCUMENT_VALIDATE_COMMIT_PUSH_COMPLETE
