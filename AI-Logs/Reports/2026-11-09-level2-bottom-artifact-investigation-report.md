# Level 2 Bottom-of-Tube Artifact Investigation

Date: 2026-11-09
Disposition: **EXPECTED/HARMLESS — NO FIX**

## Scope and starting state

1. The task token `TEMPEST_LEVEL2_BOTTOM_ARTIFACT_INVESTIGATION_20261109` was
   verified exactly once on its own line. The complete user prompt was copied
   verbatim to
   [`../Prompts/2026-11-09-level2-bottom-artifact-investigation-prompt.md`](../Prompts/2026-11-09-level2-bottom-artifact-investigation-prompt.md);
   SHA-256: `D345A12D3A60516E27D56986530813E8909B4328EC6C832D5E15C1DC3BC31AB4`.
2. Starting branch/HEAD: `master` tracking `origin/master`,
   `f362af7f87c23a9b983937f8733f5a4ac71d0162`
   (`Implement accepted Level 1 four-Flipper maximum`).
3. The index was empty. The inherited unstaged diagnostic changed
   `README.md`, `TEMPEST-BEHAVIOR-SPEC.md`, `TEMPEST-DEVELOPMENT-NOTES.md`,
   `src/Tempest.Game/Debug/DebugController.cs`,
   `src/Tempest.Game/Debug/HeadlessGameplayPlaybackCli.cs`,
   `src/Tempest.Game/Program.cs`,
   `src/Tempest.Game/Rendering/VectorRenderer.cs`,
   `src/Tempest.Game/TempestGame.cs`, and added
   `tests/Tempest.Game.Tests/Level2FourFlipperCapTests.cs`.
   The untracked `Reference/arcade/roms.rev2/` material was protected and
   untouched.
4. The required concurrency and diagnostic reports were read. Neal's manual
   Level 2 A/B result is strong acceptance of the four-Flipper gameplay
   comparison, but this task does not promote the cap.
5. The reported artifact was preserved as uncertain: a small object at the
   deep end, seen once in each of three diagnostic plays, later disappearing;
   the original observation did not establish its identity or disappearance
   event.

## Bounded reproduction

6. A temporary test in the existing `Tempest.Game.Tests` assembly stepped
   production Level 2 and the cap diagnostic side-by-side with seed `1981`,
   1,000 lives, no input, and a 5,000-tick maximum. It used the existing
   simulation collections and `VectorRenderer` projection/draw callback. The
   test source was removed after the investigation; no standalone project or
   executable was created.
7. At diagnostic tick 289, the waiting entity was the `Egg` object in
   `CurrentLevel.Eggs` (also exposed as `game.Eggs`): lane `EdgeIndex=0`,
   `Height=0`, `State=ReadyToHatch`, `UpdateCounter=0`, direction `1`, and
   ordinary Level 2 identity fields `OriginalSlotId=-1`,
   `CargoUnitId=-1`. The diagnostic had four active Flippers, three Eggs,
   reserve five, and all 12 obligations unresolved:
   `4 + 3 + 5 = 12`.
8. A second cap-blocked Egg appeared at tick 339, lane 4, height 0, direction
   -1. At that boundary there were four Eggs, four Flippers, reserve four, and
   all 12 obligations remained unresolved. The two-Egg approach/readiness
   limit bounds the number waiting at once; it does not guarantee only one
   waiting Egg.
9. The renderer callback submitted the lane-0 Egg at projected center
   `<418.53845, 412.46155>` with draw mark top-left
   `<417.53845, 411.46155>` and size `3x3`. `DrawEggs` renders all Eggs without
   filtering on lifecycle state. At that boundary the Egg was at height zero,
   so `ProjectEgg` computed depth `Position + Depth - Height*20 = 50 + 400 =
   450`: the far/deep web end. There is no projection clamp involved.
10. The Level 2 renderer uses the same small red point for each Egg state.
    During ordinary play `VectorRenderer.Draw` submits Eggs before Flippers,
    other enemies, and enemy bullets. The waiting Egg is not a Flipper glyph
    or a stale renderer reference; the renderer reads the current Egg
    collection.
11. At tick 289, the diagnostic had zero player shots and zero active
    destruction effects; it did have two enemy bullets. Those use the
    separate white, rotating eight-line bullet renderer and were not the
    3x3 Egg mark. The object was not a shot, bullet, effect, or projection
    endpoint artifact.
12. The Flipper-cap hatch check occurs before `HatchReadyEnemy` is called.
    While four Flippers are active the ready Egg remains in the collection;
    `Egg.Update` returns immediately for `ReadyToHatch`, so its lane and
    height remain fixed. No partial Flipper is created and there is no
    simultaneous Egg/Flipper representation for that waiting Egg.
13. In same-seed production at tick 289, the corresponding lane-0 unit had
    already hatched: production had five Flippers, including lane 0 at height
    2 in `Pausing`, and two Eggs. At tick 339 production had six Flippers
    while the diagnostic remained at four. Thus the ready-Egg state exists
    transiently during the production update but is consumed in that same
    update; the renderer does not get a frame with that Egg visible. The cap
    prolongs an existing transition across rendered frames rather than
    introducing an invalid state.
14. In this no-input reproduction, the two waiting Egg references were
    removed at tick 451 when a player life was lost and the wave reconstructed:
    lives became 999, active Flippers became zero, four fresh Eggs were
    present, reserve was eight, and all 12 obligations remained unresolved.
    The second pair was similarly replaced at tick 3423. This path is
    `ResetAfterDeath` / `Level.ResetAfterDeath`, which clears Eggs and releases
    fresh ones for unresolved supply; it was not hatching.
15. The other normal disappearance path is established by the existing
    `ReadyEggWaitsWithoutResolvingAndHatchesWhenZapperRemovalFreesCapacity`
    test: when a Zapper target is removed and a slot opens, the ordinary hatch
    phase consumes the oldest ready Egg and adds its Flipper. In live play,
    the particular disappearance Neal saw cannot be assigned to hatch versus
    life reconstruction from the supplied observation alone.
16. The seeded 5,000-tick run reproduced cap-wait events at lanes 0 and 4,
    then reproduced them again after life reconstruction. The fixed Level 2
    spawn-lane sequence and deterministic update order explain repeatable
    locations for this seeded path. “Once per wave” is not a code invariant:
    more than one Egg may wait concurrently, and later waits can recur when
    the cap is reached again. The observation of one noticed object per wave
    is compatible with a tiny mark being easy to miss; it does not establish
    that only one can occur.

## Hypothesis results and fix decision

17. **Confirmed:** a cap-blocked `ReadyToHatch` Egg is the source of the
    reproduced bottom mark. It is a normal pending wave obligation, not a
    newly hatched Flipper or partially created enemy.
18. **Ruled out for the reproduced mark:** roaming/approaching state (the
    entity was `ReadyToHatch`), below-rim canflip pivot, enemy bullet, player
    shot, destruction effect, duplicate Egg/enemy rendering, stale renderer
    reference, and a Flipper paused at height zero. No terminal interpolation
    was active.
19. **Accounting/collision/firing/progression:** the Egg stays unresolved and
    conserved; it is not in the active-enemy collision or firing lists. It
    does not change score, enemy-shot capacity, or progression. Hatching and
    death reconstruction use their existing paths.
20. **Root cause:** the diagnostic intentionally blocks hatch while four
    Flippers are active, preserving the ready Egg; the shared renderer
    intentionally continues to draw every Egg at its current projected
    position. With Egg height zero, that position is the far web. The longer
    visible lifetime is caused by the diagnostic cap.
21. **Fix decision:** no correction is warranted. Hiding, moving, deleting, or
    prematurely hatching this Egg would conceal or alter a real unresolved
    simulation object and could break the accepted waiting-Egg/accounting
    behavior. Treat this as expected diagnostic presentation, not a
    simulation, collision, or rendering-state defect. A distinct visual
    refinement would require separate justification and acceptance.
22. The evidence here establishes C# behavior only. The prior reports establish
    the arcade four-Flipper limit, but this investigation did not establish
    ROM or Incitti rendering for a pending Egg. The red 3x3 waiting mark is
    therefore a project presentation behavior, not an arcade-equivalence
    claim.

## Changes and validation

23. No tracked production source, test, or durable documentation was changed.
    The temporary probe test was removed. The inherited diagnostic candidate
    was preserved.
24. No persistent test was added or updated. The temporary bounded probe
    passed and captured the state/render callback values above.
25. Focused existing regressions passed: the deterministic production versus
    diagnostic cap comparison and the waiting-Egg/Zapper hatch test —
    **2 passed, 0 failed, 0 skipped**.
26. Those test commands built `Tempest.Game` and `Tempest.Game.Tests` for
    `net9.0` successfully. No separate full solution build was run.
27. No expanded/full suite was run because no fix was made; this follows the
    task's no-fix validation scope.
28. `git diff --check` was run after cleanup and passed.
29. No Windows Application Control or Smart App Control block occurred. No
    security setting was changed.
30. No timeout, hang, stall, or canceled command occurred. The probe loop was
    bounded at 5,000 ticks; the focused existing tests use bounded loops and
    completed normally. The Blame collector produced no sequence or dump.
31. Protected ROM files, `UserData/`, screenshots, gameplay recordings,
    `Videos/`, existing reports, `TEMP/`, and build output were not manually
    edited, moved, staged, or deleted. No TEMP evidence was read.

## Handoff and final state

32. The inherited diagnostic remains unstaged on the accepted baseline; no
    production Level 2 cap promotion was made. Production Level 2's uncapped
    behavior and the accepted production Level 1 cap were not changed by this
    investigation. The protected untracked ROM directory remains untouched.
33. No manual retest is required for this no-fix outcome. Neal's prior A/B
    gameplay acceptance remains applicable. If he wants to identify the exact
    disappearance event in a future visual capture, note the Egg's lane and
    whether a Flipper slot opened or a life was lost at that moment.
34. Recommendation: retain the Level 2 four-Flipper diagnostic for separate
    acceptance/promotion review; do not promote it in this task.
35. Final status: **EXPECTED/HARMLESS — NO FIX**. The object's general
    lifecycle and the reproduced disappearance events are established; which
    event ended the specific object in Neal's three observations remains
    unknowable without a recording or a targeted follow-up observation.
