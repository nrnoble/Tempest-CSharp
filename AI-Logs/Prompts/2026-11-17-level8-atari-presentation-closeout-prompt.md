# Level 8 Atari-presentation closeout

Date: 2026-11-17

## Scope

Close out the manually accepted Level 8 `--atari-presentation` milestone.
Neal reports that all manual tests passed, no regressions were observed, the
visuals look perfect, and no further Level 8 work is wanted. Record this
acceptance, perform final automated validation, update durable documentation,
commit only the bounded Level 8 milestone, push it to the configured upstream,
and verify the final repository state.

## Accepted implementation to preserve

- Displayed Level 8 uses the recovered Rev. 2 shape-7 geometry.
- `RomPresentationMetadata` maps Level 8 to tube-table index 7.
- Production geometry remains the existing data used by
  `LevelLoader.LoadLevel8OpenTube`.
- Atari presentation continues using the existing Level 8 model-frame mapping,
  ROM metadata, and shared projector for tube and gameplay entity presentation.
- The accepted Level-8-specific post-projection logical Y translation is
  exactly **+212.28139**. Do not retune, round, reinterpret, or replace it.
- The translation is a project decision/translation, not an Atari ROM constant.
- Gameplay, geometry, projection, scale, perspective, camera architecture,
  timing, RNG, recording/playback simulation, and entity behavior must not be
  changed.
- Freeze Level 8 after closeout. Future changes require a demonstrated
  regression or a separately authorized milestone.

## Boundaries

- Do no new Level 8 development or investigation.
- Do not inspect screenshots/videos or raw ROMs, reanalyze geometry or
  projection, compare against MAME, add diagnostics, refactor presentation, or
  modify gameplay.
- Preserve accepted Level 10, including its frozen +50 logical-pixel placement.
- Do not change Level 7; its depth-motion/presentation investigation remains
  separate.
- Do not modify Level 9 or other levels, and do not begin Level 9 work.
- Never reset, restore, clean, stash, rebase, or discard existing work.
- Keep unrelated modified and untracked files out of the commit.
- Do not launch the interactive game; manual acceptance is already complete.
- Keep `AI-Logs/` ignored and unstaged.

## Required closeout

Inspect and update `TEMPEST-DEVELOPMENT-NOTES.md` and
`TEMPEST-BEHAVIOR-SPEC.md` as appropriate. Record the manual acceptance,
no-regression result, perfect visual assessment, no-further-work direction,
exact frozen placement, implementation path, evidence classifications, and
unchanged gameplay without promoting project translations to ROM-backed facts.

Save a paired report under `AI-Logs/Reports/`. Include the changed files,
validation commands/results, staged file list and review, staged whitespace
check, commit hash/message, push/upstream result, final Git state, excluded
unrelated work, preserved Level 10 and Level 7 status, and confirmation that
the interactive game was not launched.

Run final validation after documentation changes:

1. Focused Level 8 presentation/production tests.
2. Relevant presentation regressions covering Level 8, Levels 1-9
   presentation selection, Level 10 placement, entity registration,
   display/camera, recording/playback, Approach/transition/exit, and option-off.
3. Full solution tests with hang protection.
4. Release solution build.
5. `git diff --check`.

Do not treat canceled, timed-out, blocked, stalled, or incomplete validation as
a pass. After validation, stage only the Level 8 implementation/test files and
the durable documentation actually updated. Review the staged file list and
diff, run `git diff --cached --check`, and commit only if every required check
passes and the staged changes are bounded. Push the new commit to the existing
upstream without changing remotes, then verify the branch, upstream, 0/0
ahead/behind state, commit at HEAD, and clean Level 8 index state. If push
fails, diagnose once and report the exact failure without repeated retries.

TEMPEST_LEVEL8_ATARI_PRESENTATION_CLOSEOUT
