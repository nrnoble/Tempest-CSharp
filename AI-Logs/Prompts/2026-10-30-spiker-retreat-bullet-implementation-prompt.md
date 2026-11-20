**Implement the Incitti-source-backed Spiker retreat bullet.**

Follow .github/copilot-instructions.md and the established Tempest validation and
maintainability rules.

CURRENT CHECKPOINT

Commit:
21a60b8 Add Tanker enemy bullet firing

Current verified test suite:
161 passed, 0 failed.

The working tree should be clean.

SCOPE

Modify only what is necessary in:

- Gameplay/Spiker.cs
- TempestGame.cs
- SimulationTests.cs
- documentation only if required

Reuse the existing EnemyBullet implementation.

Do not modify EnemyBullet behavior or rendering.

SOURCE BEHAVIOR

Recheck the relevant Spinner/Spiker retreat path in
Reference/incitti/tempest.bmx before editing.

The previous source analysis established:

- a Spiker can fire at most one bullet;
- it has no ammunition counter or firing timer;
- the firing opportunity occurs only when the Spiker makes its random transition
  from building to retreating;
- the retreat trigger remains the existing source-backed growth/random condition;
- when that retreat transition occurs, a separate source random decision strictly
  greater than 70 determines whether the bullet is created;
- the bullet originates on the Spiker's lane at its current height;
- the projectile is the same shared EnemyBullet used by Flippers and Tankers.

Confirm these details directly against Incitti before implementing them.

MODE BEHAVIOR

Enable the Spiker retreat bullet only when the existing flags are combined:

--spiker-test --enemy-bullets-test

Do not add a new flag.

Therefore:

--spiker-test
    Spikers remain silent.

--enemy-bullets-test
    Existing behavior remains unchanged.

--spiker-test --enemy-bullets-test
    Flippers retain their existing bullet behavior and Spikers gain the optional
    retreat-transition bullet.

IMPLEMENTATION

Keep the design small.

Spiker should expose the optional bullet produced during its existing update/retreat
decision without knowing about TempestGame collections, scoring, or flags.

TempestGame should register the returned bullet in:

- the existing enemyBullets collection;
- the existing insertion-ordered development collision list.

Preserve existing update ordering.

Because enemy bullets are updated before Spikers in the game step, a newly created
Spiker bullet should first move on the following simulation tick.

Once created, the bullet is independent of the Spiker. Removing/retreating the
Spiker must not remove the bullet.

Do not alter:

- Spiker movement or growth;
- retreat probability;
- spike growth;
- spike ownership/persistence;
- enemy accounting;
- Spiker score;
- Flipper bullets;
- Tanker bullets;
- player-death/reset behavior;
- exit behavior.

TESTS

Add focused deterministic coverage for:

1. --spiker-test alone remains silent.
2. Combined --spiker-test --enemy-bullets-test enables the retreat bullet.
3. A retreat firing decision at or below 70 does not fire.
4. A decision above 70 fires exactly one bullet.
5. The bullet starts on the Spiker lane at its current retreat-transition height.
6. A Spiker cannot produce multiple retreat bullets from the same lifecycle.
7. The bullet survives Spiker removal/retreat.
8. Shooting it still awards the existing 25 points without changing enemy
   accounting.
9. Life reset and exit use the existing EnemyBullet cleanup.
10. Existing Flipper/Tanker bullet behavior and Spiker/spike behavior remain green.

Avoid creating a large number of overlapping integration tests. Use existing
regressions for behavior already covered elsewhere.

DOCUMENTATION

Add concise maintainability comments only where the retreat-transition firing rule
or update ordering is not obvious.

Do not perform another general documentation pass.

AI ARCHIVE

Save this task specification to:

AI-Logs/Prompts/2026-10-30-spiker-retreat-bullet-implementation-prompt.md

Save a concise final report to:

AI-Logs/Reports/2026-10-30-spiker-retreat-bullet-implementation-report.md

If the prompt cannot be archived verbatim, clearly label it as a faithful
reconstruction.

FINAL REPORT

Keep the report concise:

1. Source behavior confirmed
2. Implementation
3. Files changed
4. Tests added
5. Final test count
6. Manual validation
7. Deferred/uncertain behavior

Do not commit or push.

Do not implement Fuseballs, Pulsars, Super Zapper, progression, normal-Level-1
bullet activation, or the suspected overlapping-Flipper issue.
