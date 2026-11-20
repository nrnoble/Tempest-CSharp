# Project Instructions Fail-Safe Maintenance

## Starting state

- Branch: `master`, tracking `origin/master`.
- Starting HEAD and `origin/master`: `c45ac28096a13ed03cf26827c2df367179c60553`, matching the accepted baseline.
- Index and tracked worktree were clean. Untracked files under `Reference/arcade/roms.rev2/` were present and left untouched.

## Changes

- Modified only `.github/copilot-instructions.md` as a tracked file.
- Read the existing instruction file in full. Its organization was goals and milestones, code comments, AI prompt/report logging, and process requirements.
- Added **Command, validation, and recovery safety** after the existing Process guidance.
- Added requirements for Windows security-block handling and evidence capture; no weakening, bypass, retry loops, or stale-binary substitution; materially different safe retries only; task-appropriate bounds for long-running work and gameplay-state waits; stopping and bounded isolation on stalls; explicit incomplete-result statuses; voluntary self-stop with a precise Neal handoff; and no normal staging/commit/push when required validation is incomplete.
- Established these as project-wide defaults while preserving task-specific scope, validation, manual acceptance, evidence, and Git authorization.
- Preserved all existing instructions, including the no-interactive-game rule, project evidence and logging requirements, and Git safety.
- Saved the complete task prompt under `AI-Logs/Prompts/2026-11-09-project-instructions-fail-safe-maintenance-prompt.md`; its SHA-256 matched the provided task attachment.

## Validation and delivery

- Documentation-only change; gameplay tests and .NET builds were not run.
- Reviewed the full staged instruction diff. `git diff --check` and `git diff --cached --check` passed.
- Staged only `.github/copilot-instructions.md`; AI log files are ignored by Git.
- Commits:
  - `810c5a6d80a828f2c4f65283ac9e295777189d27` — `Add Copilot security and stall fail-safe policy`
  - `7acac60035383bfb6e0cb170c0adfa0dd88b6c4b` — `Clarify blocked artifact-dependent commands`
- Pushed `master` to `origin`. Final local HEAD, local `origin/master`, and live remote `master` all equal `7acac60035383bfb6e0cb170c0adfa0dd88b6c4b`.
- Final index and tracked worktree are clean. The pre-existing untracked ROM directory remains untouched.
- No gameplay, Level 3 concurrency, Level 9, production code, tests, or unrelated cleanup was started.
- No manual gameplay validation is required for this instruction-only change.
