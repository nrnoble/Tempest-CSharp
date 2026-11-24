# Resume accepted `--start-level` / Level 10 production-chain closeout after clarification

Continue the previously blocked closeout using
`AI-Logs/Reports/2026-11-23-start-level-level10-chain-closeout-partial-report.md`
as the handoff.

The prior closeout stopped because the request conflicted about `--level`.
That ambiguity is resolved:

- Do not change `--level` behavior. Existing accepted production `--level N`
  starts permit natural production progression; source and tests assert it.
- Do not describe `--level` as terminal or single-level unless a particular
  selector's source proves that behavior. Separate level-specific
  test/scenario selectors are isolated diagnostics.
- Preserve `--start-level N` as a fresh-production-campaign startup selector:
  start genuine production Level N and follow normal production progression.
- Neal manually verified Level 10 startup and Level 10 -> 11, and normal
  Level 9 -> 10 -> 11 -> 12 play with score/lives carry. No further feature
  adjustment is needed.

Freeze the production chain at `9 -> 10 -> 11 -> 12`. Preserve production
Level 10, Level 10 -> 11, Level 11 -> 12, and Level 12 as terminal while
Level 13 is unimplemented. Do not restore 9 -> 11 or make direct-only Level 15
part of production progression.

Preserve Level 10 evidence classifications: established geometry/raw
presentation data retain their existing ROM/disassembly-backed status; its
preserved Level-7-like gameplay remains a PROJECT TRANSLATION where arcade
mechanics have not been independently established. Manual integration
acceptance does not upgrade gameplay evidence. Do not alter Level 10 gameplay.

Correct current documentation to describe actual `--level` behavior,
`--start-level` semantics, production Level 10, manual acceptance of the
9 -> 10 -> 11 -> 12 chain, and Level 15's direct-only status. Do not invent an
isolated-versus-continuing difference. Keep README CLI guidance concise and
document level-specific scenario selectors as development diagnostics.

Preserve all accepted Level 12/global work, including Level 12 presentation
defaults and headless soak, Fuseball bonus vector digits and animation/terminal
movement, open-rim Flipper pursuit-latch correction, corrected Level 11
terminal-Spiker test, Atari HUD, Blaster-death baseline, playback/reverse
playback, Approach, and deferred Level 12 ExitZoom disposition.

Preserve the paired runtime-capability-probe AI-Logs records. Do not add
telemetry or agent-controlled gameplay.

Before validation, verify repository state. Preserve unrelated tracked changes,
local/untracked reference material, protected ROMs, UserData, Temp, Videos,
screenshots, recordings, and personal scratch material. Continue from the
intact candidate; do not stage unrelated/local/protected material.

Run bounded full closeout validation, including:

1. `--start-level` parser tests.
2. Startup configuration tests.
3. Existing `--level` behavior tests.
4. Production Level 10 startup tests.
5. Level 10 recording/playback identity tests.
6. Level 9 -> Level 10 progression.
7. Level 10 -> Level 11 progression.
8. Level 11 -> Level 12 progression.
9. Score/lives carry.
10. Restart/Game Over startup tests.
11. Atari-presentation startup/progression.
12. HUD active-level tests.
13. Recording/playback tests.
14. Level 12 headless production soak.
15. Level 1-12 regressions.
16. Direct-only Level 15 preservation.
17. Open-rim pursuit regressions.
18. Fuseball bonus-vector regressions.
19. Fuseball animation and terminal behavior regressions.
20. Generic accepted ExitZoom regressions.
21. Complete solution test suite.
22. Release solution build.
23. `git diff --check`.

Use explicit hang/time bounds. Do not launch the interactive game; Neal's
manual acceptance has already passed. If a real production regression appears,
stop before staging.

After validation passes, stage only the accepted `--start-level` and Level 10
production-chain implementation, associated tests, accurate durable
documentation, relevant AI-Logs implementation/closeout records, and clearly
pending historical report updates where they naturally belong. Include the
previous partial closeout report as historical evidence of the clarification
stop if it belongs to the tracked AI-Logs convention. Review staged inventory,
staged source/text diff, and `git diff --cached --check`.

Commit on current `master` with `Add continuing start-level campaign`. Do not
amend previous commits or rewrite history. Push normally to `origin/master`,
then verify local master equals origin/master, ahead/behind is 0/0, and the
index is empty. Where available, independently verify remote master through
GitHub; report an empty/invalid connector response rather than claiming
verification.

Update/create the paired final closeout report with the `--level`
clarification, accepted `--start-level` semantics, production chain, manual
acceptance, validation and full-suite count, Release build, commit SHA/message,
push and GitHub connector results, exact committed files, excluded/local
material, and final worktree/index/upstream state. Do not enter a
commit/amend/report loop merely to include the resulting SHA in its own report.
Do not begin Level 13.

TEMPEST_START_LEVEL_CLOSEOUT_CLARIFIED
