# Level 7 Atari on/off deterministic state A/B report

## Result

Neal's visual A/B observation is preserved: entity presentation was reported
to differ substantially from early playback while the sampled HUD progression
matched. No screenshots, videos, frames, or other visual assets were opened or
analyzed.

The deterministic A/B state comparison found no divergence from restored
boundary 0 through the complete 3,510-tick recording (3,511 boundaries
including boundary 0).

## Recording and controls

- Recording: `TEMP\GameplayRecordings\Level-7-slowmo.json`
- SHA-256 before and after: `C13D201C00C90A3B497FF0A2E57948CB57F662881C8AC3333AB744298629B5B5`
- Runtime diagnostic metadata
  (`TEMP\Diagnostics\Level7VisibleRuntime\level7-control-20261117.json`)
  names that exact playback path and `recordedPlayback` mode.
- Recording metadata: production format v2; starting displayed level 7;
  starting lives 21; rim multiplier 1; seed 307820959; duration and frame count
  3510.
- The same loaded recording/replay and startup configuration were used for
  both runs. Each run constructed the production `GameplayPlaybackSession`
  and a `VectorRenderer` with `atariPresentationEnabled` false or true. Runs
  were sequential, with `Projection.K=70`, `Projection.ZOffset=5`, and the
  default portrait-perspective value restored before each run.
- No Raylib window, renderer draw call, interactive game, screenshot, or
  framebuffer capture was used.

## Fingerprint and limits

At each boundary, SHA-256 was computed over JSON containing
`GameplayStateSummary` plus the current `TempestGame` object graph. The
temporary external harness captured public properties and public/private
instance fields on game-state objects in the `Tempest.Game`,
`Gameplay`, `Levels`, `Systems`, and `Debug` namespaces. This covers the
player, game/level state and counters, wave/ledger obligations, pending
activation and identity sets, all live gameplay entity collections and their
state/timers, shots, enemy bullets, spikes, and stable entity identities.
Ordered lists retain their order; dictionaries and sets are canonicalized;
field/property names are sorted. No object addresses or diagnostic runtime
IDs are used.

The fingerprint excludes the `Random` instance itself (its internal state is
not exposed cleanly), audio sinks, immutable level-definition geometry, and
types outside the simulation-state namespaces, including renderer and
`System.Numerics` presentation coordinates. It contains no screen positions,
camera state, framebuffer data, or timestamps. The same seed/startup was
restored in each run, and all downstream simulation state was fingerprinted at
every boundary; no RNG instrumentation was added.

## Direct answers

1. **Recording:** `TEMP\GameplayRecordings\Level-7-slowmo.json`, SHA-256
   `C13D201C00C90A3B497FF0A2E57948CB57F662881C8AC3333AB744298629B5B5`.
2. **Does `--atari-presentation` enter simulation?** In the current worktree,
   the flag is presentation-only. `Program` parses/validates it, rejects it
   for headless playback, constructs the same restored `TempestGame`, and
   passes the flag to `VectorRenderer`; it is not part of recording startup
   metadata or the playback session/update input. `PresentationMode` is a
   separate renderer selection. The game does share mutable
   `Projection.K`/`ZOffset` with rendering and changes them during exit/reverse
   zoom, but those values are initialized identically and are not selected by
   `--atari-presentation`.
3. **Initial restored states:** identical at boundary 0.
4. **Boundaries 100, 150, 200, 250, 300:** identical at each boundary.
5. **Before boundary 100:** no divergence; every intervening boundary was
   compared.
6. **Complete playback:** no divergence across all 3,511 boundary
   fingerprints.
7. **First divergent boundary/state difference:** none.
8. **Identity evidence:** equal SHA-256 fingerprints for both runs at every
   boundary, including:
   - 0: `5AAF006C8B5C4DF7F02198A925ADADAF2D0A6C1727CD61C79D15ED4B537D62D7`
   - 100: `87989FB6D8515C4A227B82DE28983F53FCD7D1184389577E7B2D37D50C3A561A`
   - 150: `ABB702A7A14E30C37E2C127B65FAE601D45D458847140ED94C5B120C247CCB1D`
   - 200: `D1797F070475B29BD42CDBF4EFC720A3AD1480A6C0F392BA6E1EC69523381AAA`
   - 250: `44BE1D3B9C7FC0F03048146D601B13D17EFA4FE988FD676406047F742DD0EF1A`
   - 300: `859A013488873015A00FABA920344D93A0C27778B6A332D52AB94F7DB537244A`
9. **Narrowest next step:** investigate only the presentation boundary for the
   same recorded playback—compare the Level 7 renderer's submitted per-entity
   geometry with the resulting framebuffer at equivalent boundaries.
10. **Framebuffer diagnostic:** exercise the already implemented diagnostic
    in that next presentation investigation. It was not run here; no
    interactive game or visual output was produced.

## Harness note

An initial in-process interleaved two-session probe differed at boundary 3204
in `Player.Depth` (364 versus 363.8). An OFF/OFF control reproduced the same
difference, so it was harness contamination, not an Atari-mode divergence:
`TempestGame` exit/reverse-zoom updates mutate shared static
`Projection.K`/`ZOffset`. The comparison was rerun sequentially with those
startup projection values restored before each run and then matched through
completion.

The headless application build with an initial custom
`BaseIntermediateOutputPath` failed on duplicate target-framework attributes
because existing repository `obj` generated sources were included alongside
the external output. The bounded retry using `dotnet build --artifacts-path`
completed successfully with 0 warnings and 0 errors. No tracked production,
test, configuration, recording, diagnostic, or documentation files were
changed. Branch `master`, HEAD `385f603a94391756b2ca6cccd524dc17b281e782`,
upstream `origin/master`, and the pre-existing tracked/untracked worktree
state were preserved; no files were staged or committed.

## Validation and remaining acceptance

- Current game source built successfully with output redirected to external
  session storage (`dotnet build ... --artifacts-path ...`).
- External read-only harness compared all 3,511 boundaries and completed with
  no differences.
- The built headless CLI accepted ordinary playback (exit 0) and rejected
  `--atari-presentation` in headless mode as designed (exit 2).
- Recording SHA-256 remained unchanged.
- No gameplay behavior changed, and no manual gameplay acceptance was
  attempted or claimed. Neal's observed presentation difference remains the
  motivating manual evidence.

SIMULATION IDENTICAL THROUGH COMPLETE PLAYBACK
