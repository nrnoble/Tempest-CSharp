# Diagnostic Level 10 with genuine Level 10 tube geometry — BLOCKED

## Outcome

**BLOCKED before implementation.** The available authoritative material does not provide Level 10's remap value and ordered tube points, so implementing the required geometry would require guessing. No gameplay, presentation, test, or durable behavior-specification source files were changed. The previous Level 7-clone result was not investigated or reinterpreted.

## Evidence and blocker

- The local Rev. 2 disassembly, [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md), describes the 16-by-16 `lev_x`, `lev_y`, and `lev_angle` tables and the per-level `lev_remap`, scale, camera-height, placement, and open-topology arrays around source lines 7468–7479. Its `get_tube_no` routine uses the level index to select `lev_remap` around lines 8282–8311. The listing leaves the relevant table contents as `.chunk` declarations, not decoded values.
- Existing project evidence establishes other rows, not Level 10: Level 8 uses shape row 7 and Level 9 uses row 13. The existing diagnostic Level 10 currently shares Level 7's definition and mapping; those values cannot establish genuine Level 10 geometry.
- Bounded inspection of the named Rev. 2 program-chip files found each of `136002-129.h1`, `136002-130.j1`, `136002-131.k1`, and `136002-132.l1` is only 256 bytes. Targeted reads at candidate offsets in `136002-131.k1` returned no table bytes. The local binary material therefore did not resolve Level 10's remap or points. No other ROM contents were opened or enumerated.
- Consequently, the authoritative source does not uniquely establish the shape from the available material. No Level 10 definition, gameplay adaptation, or Atari-presentation change was attempted.

## Intended experiment and implementation status

- **Authoritative source consulted:** the local Rev. 2 arcade disassembly; its geometry table declarations are present, but the required values are absent from the listing.
- **Level 10 tube definition implemented:** none; blocked pending authoritative remap and geometry values.
- **Non-alias proof:** none; existing diagnostic Level 10 remains unchanged and still aliases Level 7. No test result should be taken as evidence for the requested second experiment.
- **Level 7-like gameplay reuse:** unchanged from the existing diagnostic. This task made no gameplay changes and did not research authentic Level 10 gameplay.
- **Geometry-forced gameplay differences:** none implemented or established.
- **Atari presentation:** unchanged. It has not been validated with genuine Level 10 geometry.
- **Recording/playback:** unchanged; no recording was created or replayed for the requested experiment.
- **Files changed:** only this paired report and the task prompt under ignored `AI-Logs/`. No project source, tests, configuration, protected reference files, or durable behavior documentation were modified.

## Validation

- Focused tests: **NOT RUN** — no candidate implementation exists to test.
- Full regression suite: **NOT RUN** — blocked by missing authoritative geometry.
- Release build: **NOT RUN** — blocked by missing authoritative geometry.
- `git diff --check`: **PASS** on the pre-existing worktree diff; Git emitted only line-ending conversion warnings for `LevelLoader.cs` and `InputSystem.cs`.

## Git status

Branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`, tracking `origin/master`. The substantial pre-existing tracked edits and untracked investigation/reference files observed at task start remain untouched. No files were staged, committed, or pushed. The prompt and report are under ignored `AI-Logs/`.

## Manual acceptance and next step

Manual diagnostic result: **PENDING**. There is no valid new-geometry run to evaluate. No runnable manual commands are supplied: using the existing Level 10 commands now would replay the old Level 7-clone experiment and would not answer the requested geometry question.

To unblock, make the authoritative Level 10 remap and ordered geometry available in the local Rev. 2 evidence (for example, a complete, verified table extraction or complete relevant ROM image). Resume by validating that evidence, then implement and test the controlled experiment. Do not infer the result from the prior experiment.
