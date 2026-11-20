# Level 2 Four-Flipper Cap Diagnostic

Date: 2026-11-09
Disposition: **IMPLEMENTED — AUTOMATED VALIDATION PASSED — MANUAL LEVEL 2 A/B ACCEPTANCE PENDING**

## Implementation and evidence

1. **Starting Git state:** `master`, tracking `origin/master`; `HEAD` and
   `origin/master` were both `f362af7f87c23a9b983937f8733f5a4ac71d0162`.
   The index and tracked worktree were clean. The only pre-existing
   untracked content was the protected 21-chip `Reference/arcade/roms.rev2/`
   directory; it remains untouched.
2. **Accepted baseline:** `f362af7f87c23a9b983937f8733f5a4ac71d0162`,
   `Implement accepted Level 1 four-Flipper maximum`.
3. **Investigation reused:** `2026-11-09-level2-concurrency-investigation-report.md`
   and the two required Level 1 correction/closeout reports were read. No
   additional ROM investigation was performed.
4. **ROM policy isolated:** Level 2 permits at most four concurrent active
   Flippers across difficulties. The global `max_enm` value six is not a
   strict six-object ceiling: its runtime comparison is inclusive. No global
   active-enemy gate was implemented.
5. **CLI/scenario identity:** Added only
   `--level2-four-flipper-cap-test`. It is a direct Level 2 terminal scenario,
   replayable with R, with a specific development window title. Duplicate
   selectors and conflicting scenario modes are rejected.
6. **Production Level 2:** Unchanged and uncapped. Generic `--level 2`, the
   existing `--level2-test`, natural Level 1-to-2 entry and natural Level
   2-to-3 progression do not inherit the diagnostic cap.
7. **Level 1:** Its accepted production predicate and four-Flipper behavior
   remain unchanged.
8. **Active-count semantics:** The diagnostic consults `flippers.Count` in
   the existing hatch phase. A Flipper occupies capacity only after the
   ordinary hatch path adds it and remains counted until its ordinary removal.
9. **Waiting Egg:** At capacity, a ready Egg stays unresolved in
   `ReadyToHatch`; the hatch factory is not called. When capacity opens,
   normal hatch order and same-phase multi-hatch behavior remain available.
10. **Determinism:** The bounded production/diagnostic comparison uses the same
    seed and checks equal Egg, Flipper, bullet and reserve state before cap
    pressure. Repeated seeded diagnostic runs reproduce blocked/hatching
    behavior. No cached active counter or secondary queue was added.
11. **Release/approach:** The canonical configuration retains 12 obligations,
    four starting Eggs, eight reserve originals, 50-active-tick reserve
    release, and existing queue, approach, readiness and Egg update cadence.
12. **Canflip:** The diagnostic reuses the canonical Level 2 configuration
    and ordinary Flipper update path. Existing canflip geometry, direction
    latching, pauses, collision and shot-delay tests remain passing.
13. **Firing/bullets:** Ordinary diagnostic Flipper firing is enabled without
    `--enemy-bullets-test`. The accepted two-shot capacity and 77-tick holdoff
    remain; tests cover visible shared bullet state, rendering and player
    collision.
14. **Rim behavior:** The 2.5 rim-speed calibration and existing rim pursuit
    behavior are unchanged. The startup test verifies the multiplier can be
    supplied to the diagnostic.
15. **Death/re-entry:** Two bounded diagnostic deaths preserve unresolved
    supply and the cap; the same Level 2 configuration is retained.
16. **Super Zapper:** Delayed targets continue occupying active slots until
    removal. The waiting-Egg test verifies capacity is refilled only after
    removal and the Egg hatches through the normal path.
17. **Completion/replay:** A bounded synthetic-input test resolves all 12
    obligations, reaches the terminal direct-scenario boundary, and verifies
    replay restarts the same capped configuration.
18. **Recording/playback:** Recording, visible playback, and headless playback
    reject the diagnostic because production recording identity does not
    encode the cap. No recording schema or metadata was added; production
    recording/playback regressions pass.

## Changed files and validation

19. **Tracked files changed:**
    - `README.md`
    - `TEMPEST-BEHAVIOR-SPEC.md`
    - `TEMPEST-DEVELOPMENT-NOTES.md`
    - `src/Tempest.Game/Debug/DebugController.cs`
    - `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`
    - `src/Tempest.Game/Program.cs`
    - `src/Tempest.Game/Rendering/VectorRenderer.cs`
    - `src/Tempest.Game/TempestGame.cs`
    - `tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs`
20. **Tests added:** `Level2FourFlipperCapTests` covers canonical startup,
    strict CLI composition, uncapped production/direct Level 2, the bounded
    comparison, cap/waiting/accounting, deterministic waiting, firing and
    bullets, repeated death/re-entry, Zapper release, completion and replay.
    All test loops have explicit iteration/tick bounds and state-bearing
    failure messages where they wait for a state transition.
21. **Bounded comparison:** Production Level 2 exceeded four active Flippers;
    the diagnostic stayed at four with the next ready Egg waiting. Both
    retained supply 12 and matched before the cap became relevant. The
    existing direct `--level2-test` was also verified to remain uncapped.
22. **Focused validation:** The new diagnostic test class passed (7 tests).
    The expanded affected regression selection passed **384 tests**,
    including Level 2 square/canflip, firing, bullets, Level 1 cap,
    progression, Super Zapper, rim behavior, CLI and recording/playback.
23. **Expanded regression result:** 384 passed, 0 failed, 0 skipped.
24. **Full solution tests:** `dotnet test TempestCSharp.slnx --blame-hang-timeout 60s`
    — **1,125 passed, 0 failed, 0 skipped**.
25. **Full build:** `dotnet build TempestCSharp.slnx` — passed, 0 warnings,
    0 errors.
26. **Whitespace:** `git diff --check` passed. Git printed only existing
    LF-to-CRLF normalization warnings for `README.md` and
    `VectorRenderer.cs`.
27. **Windows security:** No Application Control or Smart App Control block
    occurred. No security setting was changed.
28. **Stalls/timeouts:** None. All selected and full test commands completed;
    no test dump or sequence was produced.
29. **Protected files:** The 21 local Rev. 2 chip files and all other protected
    user material, screenshots, recordings, videos, TEMP content and build
    output were left untouched. No protected path appears in the tracked diff.

## Handoff and final state

30. **Final Git state:** Still on `master`, `HEAD` and `origin/master` remain
    `f362af7f87c23a9b983937f8733f5a4ac71d0162`; upstream is `origin/master`.
    All candidate tracked changes and the new test are unstaged. The index is
    empty. The protected ROM directory remains the only unrelated untracked
    content.
31. **Manual commands:**

    Production Level 2:

    ```powershell
    dotnet run --project src\Tempest.Game -- --level 2 --lives 10 --spinner-sensitivity 1
    ```

    Four-Flipper cap comparison:

    ```powershell
    dotnet run --project src\Tempest.Game -- --level2-four-flipper-cap-test --lives 10 --spinner-sensitivity 1
    ```

32. **Manual A/B checklist:** Use the same physical spinner, sensitivity,
    lives, window/display setup, firing strategy and Super Zapper strategy.
    Compare simultaneous visible Flippers and any fifth Flipper; below-rim
    pivot count and lane clustering; multi-direction convergence, grouped rim
    arrivals and rim pursuit; enemy-bullet pressure, visibility and collision;
    breathing room, deliberate target tracking, sparsity and stationary
    defense; waiting-Egg hatch/refill and whether all 12 obligations appear;
    death/re-entry, Zapper follow-up pressure, completion/replay; and overall
    resemblance to arcade Level 2. Do not use exact death count as the primary
    acceptance measure.
33. **Known limitations:** These tests establish current C# behavior and
    controlled experiment isolation, not arcade timing or feel. Four
    canflip-capable Flippers may still create significant clustering and rim
    pressure. No conclusion is made about promoting the cap.
34. **Manual status:** **MANUAL LEVEL 2 A/B ACCEPTANCE PENDING** from Neal.
35. **Blockers/incomplete work:** None for automated implementation and
    validation. Interactive gameplay was not launched. Manual A/B acceptance
    remains the required next step; do not stage, commit, push or promote the
    candidate before that handoff.

The complete task prompt was archived byte-for-byte at
`AI-Logs/Prompts/2026-11-09-level2-four-flipper-cap-diagnostic-prompt.md`.
Its SHA-256 matches the supplied prompt attachment. `AI-Logs/` is Git-ignored.
