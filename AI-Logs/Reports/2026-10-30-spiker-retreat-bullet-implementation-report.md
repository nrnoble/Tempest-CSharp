# Spiker Retreat Bullet — 2026-10-30

1. **Source behavior confirmed:** Incitti's `Spinner.Update` keeps the existing `growth > 100` and `Rnd(0,120) > 88` retreat trigger, then independently fires only when `Rnd(0,100) > 70`. The bullet starts on the same lane at the post-growth height. The upper-height retreat does not fire.
2. **Implementation:** `Spiker.Update` returns the optional shared `EnemyBullet`; `TempestGame` registers it only when the existing enemy-bullets option is enabled. It joins both existing collections after enemy bullets update, so it first moves next tick and persists independently of its Spiker.
3. **Files changed:** `Gameplay/Spiker.cs`, `TempestGame.cs`, `SimulationTests.cs`, this report, and the archived prompt.
4. **Tests added:** Strict `70`/`71` boundary and one-shot lifecycle; standalone silence; combined-mode creation, origin, next-tick movement, persistence, scoring/accounting; life-reset cleanup. Existing exit-cleanup and Flipper/Tanker/Spiker regressions remain in the suite.
5. **Final test count:** 166 passed, 0 failed.
6. **Manual validation:** Not performed; validation was automated.
7. **Deferred/uncertain behavior:** Incitti behavior is implemented, but arcade authenticity is unverified. No normal-Level-1 bullet activation or other deferred features were added.
