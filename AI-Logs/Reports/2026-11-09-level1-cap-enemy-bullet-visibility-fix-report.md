# Level 1 Cap Diagnostic Enemy-Bullet Visibility Correction

Date: 2026-11-09
Disposition: **AUTOMATED VALIDATION PASSED - MANUAL ACCEPTANCE PENDING**

## Scope and starting state

Read the complete correction prompt and verified its single standalone
completion token before repository actions. Preserved the existing unstaged
four-Flipper-cap candidate. Read the required historical
[implementation report](./2026-11-09-level1-four-flipper-cap-diagnostic-report.md);
that report remains unchanged.

Started and finished on `master`, tracking `origin/master`, at
`c1a974a625eea2dfaecf30d1b8d729e022ef17e8`, with an empty index.
Neal's initial comparison was promising but incomplete because bullets were
invisible. This correction does not establish manual acceptance or promote
the cap into production.

## Investigation and root cause

- Bullets were created normally by `UpdateFlipperFiring`, retained in the
  authoritative `EnemyBullets` collection, and advanced by `UpdateEnemyBullets`.
  Firing was not disabled in the diagnostic.
- The same bullet instances participate in player-shot and player collision
  processing through the existing active collection and insertion-order
  collision path. They could therefore damage the player while invisible.
- No incorrect removal or separate invisible collision representation was
  found. Expiration and collisions retain their existing removal rules.
- `VectorRenderer.ShouldDrawEnemyBullets` recognized natural progression,
  `EnemyBulletsTest`, or a production ledger. The cap diagnostic is terminal
  (`NaturalProgressionEnabled == false`), deliberately does not enable the
  separate bullet-test mode, and uses Level 1's non-ledger configuration.
  All three conditions were false, so the gameplay draw path skipped bullets.
- The fault was renderer scenario classification, not bullet simulation,
  direct production Level 1 startup, or window-title text. Production Level 1
  already took the accepted visibility path.

A deterministic regression reproduced the defect before the renderer change:
two active diagnostic bullets existed, but rendering eligibility was false.
The test failed at the visibility assertion.

## Surgical correction

Changed only
[VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs)
in runtime code:

- Include the terminal Level 1 cap diagnostic in the existing enemy-bullet
  presentation condition.
- Expose a colored drawing overload that applies that same condition and
  delegates to the existing geometry method. The actual Raylib draw path now
  uses this overload, allowing tests to capture the real visibility decision,
  line endpoints, and accepted white color without launching a window.

No simulation or CLI changes were made in this correction. Existing production
position projection, eight radial lines at 45-degree intervals, six-unit base
size, rotation, color, sound, spawn height/lane, holdoff, thresholds, capacity,
movement, expiration, collision/damage, and update ordering remain unchanged.
The separate `--enemy-bullets-test` option was neither enabled nor required.
Production Level 1 and Levels 2-8 retain their existing presentation decisions.
The only gameplay experiment remains the four-active-Flipper hatch cap,
with waiting Eggs and terminal development completion preserved.

## Tests and validation

Added
[Level1CapEnemyBulletRenderingTests.cs](../../tests/Tempest.Game.Tests/Level1CapEnemyBulletRenderingTests.cs)
with 13 cases:

- Diagnostic visibility with two active bullets and no bullet-test selector.
- Same deterministic Level 1 hatch/firing conditions in baseline and diagnostic:
  four Flippers, two-shot global capacity, 80-tick reload for successful shooters,
  ready delays for capacity-blocked shooters, identical spawn positions and audio.
- Identical authoritative bullet state and colored drawing output over 20
  subsequent simulation ticks; movement is three units and rotation 15 degrees
  per tick, with the original object retained.
- Exact lane-midpoint/depth projection, white color, six-unit projected radius,
  and eight accepted radial line segments for each active bullet.
- Both baseline and diagnostic render the same original bullet after 130 normal
  simulation ticks. Moving the player into its lane lets its next normal advance
  cross the collision threshold, trigger the existing death state/audio, and
  remove it from active drawing output.
- The separate bullet-test option retains identical Level 1 state and geometry.
- Seeded production direct starts at Levels 1-8 produce bullets through normal
  simulation and retain the accepted colored geometry path.

Existing cap/waiting-Egg, supply, death/re-entry, Zapper, firing, lifetime,
projectile, rendering, collision, and CLI isolation tests were not rewritten.

Final validation:

| Selection / command | Result |
|---|---|
| Narrow new enemy-bullet rendering tests | 13 passed |
| Projectile/firing, four-Flipper cap, bullet/collision, player-shot rendering, Level 3 presentation regressions | 125 passed |
| Expanded Levels 1-8, simulation, startup, input and Zapper regressions | 673 passed |
| `dotnet test TempestCSharp.slnx` | 1,120 passed; 0 failed/skipped |
| `dotnet build TempestCSharp.slnx` | Passed; 0 warnings/errors |
| `git diff --check` | Passed |

Validation used .NET SDK 9.0.304, xUnit v2 and VSTest. Git emitted only
line-ending normalization warnings for README and the renderer.

Test-authoring issues were resolved before final validation: an inaccessible
height setter was replaced with normal simulation advances; an unsuitable
constant random stub in the higher-level regression caused missing shots and a
nonterminating picker attempt. That specific test process was stopped and the
higher-level tests now use a real deterministic `Random(1981)`. No gameplay
logic or environment/security settings were changed to accommodate tests.

## Documentation, protected data, and Git

The existing diagnostic sections in README, behavior specification, and
development notes describe the intended unchanged bullet behavior and pending
acceptance correctly. No further durable-documentation changes were necessary
for this implementation defect; the earlier candidate's documentation edits
remain preserved.

New correction files:

- The rendering test above (untracked, unstaged).
- [Complete correction prompt](../Prompts/2026-11-09-level1-cap-enemy-bullet-visibility-fix-prompt.md)
  (ignored).
- This new report (ignored).

The earlier diagnostic test remains untracked and untouched. All earlier
candidate modifications remain unstaged. Nothing was staged, committed, or
pushed. No writes were made to ROMs, UserData, screenshots, recordings, Videos,
TEMP, or unrelated files. No `git clean` or interactive game launch occurred.

## Neal's manual retest

Production baseline:

```powershell
dotnet run --project src\Tempest.Game -- --level 1 --lives 10 --spinner-sensitivity 1
```

Corrected capped diagnostic:

```powershell
dotnet run --project src\Tempest.Game -- --level1-four-flipper-cap-test --lives 10 --spinner-sensitivity 1
```

Verify first:

1. Enemy bullets are visible in the capped diagnostic.
2. Bullet appearance matches production Level 1.
3. Bullets move and collide normally.
4. The diagnostic never exceeds four active Flippers.
5. The capped version still feels better with bullet visibility restored.

**MANUAL ACCEPTANCE PENDING.** Automated output tests establish the corrected
rendering path, not physical-display acceptance, arcade fidelity, or a
difficulty conclusion. Stop here for Neal's corrected manual A/B comparison.
