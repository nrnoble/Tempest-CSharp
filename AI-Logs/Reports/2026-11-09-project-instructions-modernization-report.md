# Project Instructions Modernization Report

**Status: COMPLETE**

## Starting state

- Branch: `master`
- Starting HEAD and `origin/master`: `1d5f37f6c309dee7fface6f99d09abd85d8a7f4c`
- Upstream: `origin/master`; starting ahead/behind: `0/0`
- At the original baseline check, the index and tracked worktree were clean.
- `Reference/arcade/roms.rev2/` was present as protected local reference
  material. Its binary contents were not opened or enumerated.

## Changes

- Replaced `.github/copilot-instructions.md` with a consolidated rulebook for
  project purpose and evidence authority, coding and deterministic simulation,
  evidence classification, scope, local-material and Git safety, testing and
  manual acceptance, AI logs and durable documentation, command/security
  recovery, and completion-token handling.
- Removed the obsolete initial-milestone checklist and replaced preliminary
  fixed-rate/debug wording with accepted deterministic fixed-step guidance.
- Added local disassembly-first guidance, protected-material safeguards, Neal's
  manual acceptance authority, bounded milestone scope, and explicit
  commit/push authorization rules.
- Preserved recovery and Windows security protections, including bounded
  operations, no security bypass or stale-binary fallback, and explicit
  outcomes for incomplete validation.
- Saved the original task and resume request in ignored `AI-Logs/Prompts/`;
  both contain one standalone completion token. The matching report is also
  ignored under `AI-Logs/Reports/`.

## Validation and review

- Inspected the complete resulting instruction file and reviewed the staged
  diff. The staged file list contained only `.github/copilot-instructions.md`.
- `git diff --check` and the committed-diff whitespace check passed.
- No gameplay tests, solution tests, build, or game launch were run, as
  required for this documentation-only milestone.
- Self-review found no blocking authority conflict or weakened security/stall
  rule. The obsolete initial milestone is absent; the evidence classes,
  manual-acceptance boundary, local-ROM guidance, and Git/logging protections
  are represented.

## Commit and final repository state

- Commit: `10b0e6a3d46bf327e1296aecb0ccc7d5558f6f8c`
  (`Modernize Tempest Copilot project instructions`)
- Pushed `master` to `origin` successfully.
- Final `HEAD` and `origin/master` match; upstream is `origin/master`;
  ahead/behind is `0/0`.
- The commit changes only `.github/copilot-instructions.md`.
- `.gitignore` is intentional authorized user work, confirmed by Neal during
  the task. It was never staged or committed. It remained unstaged and its
  SHA-256 was unchanged across our staging and final checks.
- `AI-Logs/` remains ignored and uncommitted.
- The protected ROM directory remains present and untouched; its binary
  contents were not inspected.
- Final tracked worktree state has the intentional unstaged `.gitignore`
  change only; the index is clean.

No Level 4 concurrency, production, test, or gameplay changes were made.
