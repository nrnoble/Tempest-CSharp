# C2 Tanker firing correction: implementation and automated validation

Starting checkpoint: `27c43b7 Add validated controller restart and quit aliases`.
Authority: [C2 analysis](2026-10-31-c2-tanker-firing-policy-analysis-report.md).
Approved policy: B, literal reference-tier speed correction only.
Status: automated validation passed; Neal's qualitative manual validation **PASS**
(2026-10-31). C2 is ready for Neal's selective commit after final checks.
No commit or push. No production promotion, C3 or natural Level 3 work.

## 1. Baseline

Requested opening commands:

```text
git status --short
(empty)

git log -3 --oneline
27c43b7 Add validated controller restart and quit aliases
a922172 Ignore local video files
11773c0 Implement Level 3 geometry and directed rotation

git diff --check
(empty)

git status --short -- Reference/incitti
(empty)
```

The worktree was clean at the specified checkpoint. The full authority report,
current Tanker source, factory/update integration, relevant tests and active
documentation were inspected before editing. Source matched the approved scope;
no contradiction requiring broader implementation was found.

Test platform: SDK 9.0.304; net9.0; xUnit v2 2.9.2 and Microsoft.NET.Test.Sdk
17.12.0, running through VSTest. No global.json, Directory.Build.props or
Directory.Packages.props were found in the project tree. No dependencies or
tooling were installed or restored.

## 2. Exact runtime code change

In [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs):

```diff
-    public const float DevelopmentClimbSpeed = 1.85f;
+    // Incitti's 1.75 + lv/20 uses integer division: displayed Level 3 (lv=2) climbs at 1.75.
+    public const float DevelopmentClimbSpeed = 1.75f;
```

The nearby XML summary now describes 1.75 as reference-derived, not
arcade-measured. **This constant is the only executable gameplay source change.**
No general tuning framework, new mode or new production arming switch was added.

**REFERENCE VERIFIED:** literal Incitti `1.75 + lv / 20` has integer division
inside the addition. Displayed Level 3 has internal `lv=2`, so `2/20=0` and the
result is 1.75. This corrects an unsupported fractional-division approximation;
it is not measured Atari timing.

### Unchanged firing and lifecycle contract

- Factory ammo is inclusive 1..2; initial timer is 16 + inclusive 1..64 = 17..80.
- Climb first; only post-climb height strictly below 380 is firing eligible.
- With ammo remaining, decrement once per eligible update; decision only below
  zero; reset to 16 on every decision, including failed rolls.
- Inclusive 0..100 rolls fire only above 75; only success consumes ammo; at most
  one bullet per update. Decision windows remain 17 eligible updates apart.
- Strict natural release above 385, never fire on the release-triggering update.
- Same-lane post-climb bullet origin, +3 motion, next-game-step first movement,
  expiry above 400, independent carrier lifetime, scoring and cleanup unchanged.
- Player-death freeze/reconstruction, pending/expiring Zapper behavior, all-rim
  completion, cargo routing/identities, and live/stillborn cargo behavior unchanged.
- `--tanker-test` alone remains silent.
- `--tanker-test --enemy-bullets-test` retains its existing development arming
  path, also arming ordinary/cargo Flippers. No normal Flipper promotion.
- Production randomness remains untouched. Zero, one and two Tanker shots are
  supported outcomes; two are possible, not guaranteed.

## 3. Tests added and directly coupled expectations updated

All changes are in [SimulationTests.cs](../../tests/Tempest.Game.Tests/SimulationTests.cs).
Added 15 test cases: one 12-row theory and three facts.

| Test | Protected acceptance behavior |
|---|---|
| `TankerWholeClimbProtectsReferenceSpeedAndRandomizedShotCounts` (12 rows) | Ammo 1/2, offsets 1/64; all-failure zero shots, one success, ammo=2 two successes, ammo=1 all-success exhaustion. Constructor height 10, first height 11.75, 211 eligible climb updates, last eligible 379.25, first silent height 381, release update 215 at 386.25. First decision T+1, later +17, endpoint window totals 12/8 with ammo remaining, 75 fails/76 succeeds, reset/depletion, no draws/countdown when exhausted or at/above cutoff, no pending/released fire. |
| `TankerLateDecisionWindowsPermitTwoShotsBeforeNaturalRelease` | Ammo=2, T=80; failures at 81/98/115/132/149/166, successes at 183/200, two creations before release. No assertion requiring the old 1.85 behavior. |
| `TankerTimedGameShotsKeepClimbingAndSurviveNaturalReleaseIndependently` | Ordinary game updates including hatch; controlled successes at 194/211. Continuous climb, correct lane/post-climb origins, first movement next step, first bullet naturally expires while the second survives carrier release, no shot on release. Firing scores zero and preserves ledger totals. Natural cargo has IDs 0/1, established lanes/directions, live initial pivots and first movement next step. Player survives without destroying the carrier. |
| `ArmedTankerPendingZapAllowsFireButExpirySkipsReadyDecisionAndKeepsBullet` | Pending zap permits update-18 climb/fire; expiry at update 35 suppresses an otherwise-ready decision and motion. Existing bullet survives; carrier scores 200, two independently identified stillborn children begin with delay 8, resolve for total score 500, and accounting remains consistent. |

The game-level multiple-shot test counts **creation events**, not concurrent
bullet count: only the second remains by natural release. Ordinary Flipper
decisions are explicitly scripted as failures through the existing shared
random seam so unrelated bullets cannot confuse the assertion.

Added a test-only `StrictTankerRandom`, with finite queues keyed by exact
exclusive-upper-bound ranges, an inclusive adapter for entity tests, and
completion assertions. It rejects unknown ranges, exhausted scripts,
out-of-range values, unexpected one-bound/unbounded/floating-point draws, and
unconsumed decisions. It never clamps or silently supplies default values.
The game fixture queues the current ordinary Flippers' expected failure rolls
before each scheduled Tanker roll, matching established game ordering.
No production random seam or entity accessibility changed.

Four directly coupled existing tests now assert exact 11.75 hatch/first-update
height and/or exact 1.75 default speed. The duplicate historical default-speed
test name no longer calls the speed an approximation. Unrelated assertions
were retained, including explicit-speed strict-385 boundary coverage.

## 4. Intermediate failures and corrections

1. Initial focused invocation failed at compilation with CS1061: the new fixture
   attempted to call Flipper's internal `SetBulletAmmunition`, which is not
   accessible to the test project. Removed that approach. The final fixture
   controls failures using only the existing injected `Random`; no access
   modifier or gameplay policy was changed.
2. The next focused invocation passed 140 cases and failed the new timed game
   test on an unexpected `(0,101)` draw during its post-release follow-up step.
   An ordinary Flipper had a legitimate decision there. Added that step's
   explicit failure script and decision-queue completion assertion; no default
   or ignored draw was introduced.
3. During fixture review, projectile-height assertions were bounded by the
   projectile's actual expiry lifetime so retained references to expired
   bullets are not incorrectly expected to keep moving after removal.

These were test-fixture issues, not gameplay regressions requiring any broader
production change.

## 5. Final automated validation

Commands executed from the repository:

```powershell
dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Tanker|FullyQualifiedName~Bullet|FullyQualifiedName~SuperZapper|FullyQualifiedName~AllRim"
dotnet test .\TempestCSharp.slnx --no-restore
dotnet build .\TempestCSharp.slnx --no-restore
git diff --check
```

| Gate | Final result |
|---|---|
| Focused Tanker/bullets/Zapper/all-rim tests | **141 passed, 0 failed, 0 skipped** |
| Full solution tests | **467 passed, 0 failed, 0 skipped** |
| Full solution build | **Succeeded; 0 warnings, 0 errors** |
| `git diff --check` | **Passed; no whitespace errors** |

Git emitted its normal LF-to-CRLF working-copy warnings for two edited files;
these are Git line-ending notices, not build warnings or whitespace errors.
No new dependency, restore, configuration or SDK change was needed.

The complete source/test/documentation diff was inspected. No unrelated
production file changed. In particular no Zapper, Flipper, bullet, orchestration,
geometry, renderer, input, composition or progression implementation changed.

## 6. Documentation and evidence boundaries

- [Development notes](../../TEMPEST-DEVELOPMENT-NOTES.md): correct both active
  historical speed descriptions, identifying the old approximation as historical;
  add a concise C2 entry documenting reference semantics, unchanged firing,
  deterministic capability and completed qualitative manual PASS.
- [Behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md): correct its active
  development increment from 1.85 to 1.75 and add a **REFERENCE VERIFIED**
  explanation, explicitly not arcade-measured; record Neal's completed
  observable two-before-release validation without promoting hidden internals.
- [README](../../README.md) remains unchanged: it contains no incorrect 1.85
  claim and already documents the existing silent/combined modes.
- Existing **ARCADE VERIFIED** qualitative evidence remains separate: individual
  Tanker shots at intervals, with at least two possible before release. Exact
  Atari ammunition, delay, interval, chance, cutoff and per-carrier count remain
  unknown.
- Neal's recent impression of Incitti's close overall arcade timing remains
  qualitative comparative evidence in the authority analysis. It does not
  make source constants arcade verified or prove binary/wall-clock identity.

## 7. Changed files and Git maintenance

Exactly four modified tracked files:

```text
 M TEMPEST-BEHAVIOR-SPEC.md
 M TEMPEST-DEVELOPMENT-NOTES.md
 M src/Tempest.Game/Gameplay/Tanker.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs
```

Persistent local C2 deliverables:

- [Exact implementation prompt](../Prompts/2026-10-31-c2-tanker-firing-correction-implementation-prompt.md).
- This implementation report.
- [Exact manual acceptance/finalization prompt](../Prompts/2026-10-31-c2-manual-validation-acceptance-prompt.md).

The prompt was compared with the read-only supplied attachment and matched
after normalizing line endings. [AI-Logs](../) is ignored by the established
[Git rule](../../.gitignore#L6), so these new files do not appear as untracked
Git changes. There are no other reported untracked files. Existing analysis
deliverables were not modified.

Requested final maintenance includes `git status --short`, `git diff --stat`,
the complete `git diff`, `git diff --check`, reference/spec-specific status,
and `git log -1 --oneline`.

- `Reference/incitti` status: empty; unchanged.
- Behavior-spec status: modified for the factual reference-speed correction and
  completed qualitative manual observation with explicit evidence limits.
- HEAD remains `27c43b7 Add validated controller restart and quit aliases`.
- No staging, commit or push.

## 8. Neal's qualitative manual validation: PASS (2026-10-31)

Neal reported both required scenarios complete and **PASS**. This supersedes
the pre-validation pending status and historical instruction to await his
manual result before considering a commit. No commit or push was performed;
Neal retains the commit decision. Firing remains **DEVELOPMENT ONLY** even
after acceptance; production integration requires a separate authorization.

### Silent control

```powershell
dotnet run --project .\src\Tempest.Game -- --tanker-test --lives 10
```

- The Tanker remained silent.
- Tanker/cargo behavior remained normal.
- No input regression was observed.

### Armed Tanker

```powershell
dotnet run --project .\src\Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10
```

- Neal observed one uninterrupted Tanker fire two individual, separated shots
  before natural release.
- This was the behavior Neal expected from Atari Tempest.
- The Tanker continued climbing while firing.
- There was no burst of firing at the release point.
- An already-fired Tanker bullet was not erased merely because the carrier
  released or was destroyed.
- Cargo behavior remained normal.
- Super Zapper behavior remained normal.
- Input behavior remained normal.
- No unrelated gameplay regression was observed.

### Evidence limits and accepted policy

**ARCADE VERIFIED** is limited to Neal's qualitative Atari/manual requirement
and the listed observable validation, especially two-before-release capability.
It does not establish that every Tanker fires exactly twice. The accepted policy
remains randomized. Exact Atari ammunition, initial delay, firing probability,
timer interval, cutoff and number of shots per individual Tanker remain unknown.
The literal 1.75 climb increment remains **REFERENCE VERIFIED** from Incitti,
not arcade-measured timing. No hidden internals were promoted.

**C2 manually accepted and ready for its own checkpoint. No production firing promotion. C3,
mixed Level 3 composition and natural Level 2 -> Level 3 progression were not
started. Neal will commit C2 first and explicitly authorize any next milestone.**

## 9. Post-manual finalization and commit readiness

Finalization began with exactly the four expected modified tracked C2 files,
no reported untracked files and unchanged HEAD `27c43b7`. No unrelated work
had appeared. Only documentation and the local AI report/prompt were edited
during this finalization. SHA-256 checks confirmed that both gameplay source
and tests remained byte-identical to their pre-finalization versions:

```text
Tanker.cs
51EFD2C700A3378C7444F9BD1D55260BB973FCC1F738FF8E7DAE89DBC1F8AA24
SimulationTests.cs
7330A2EBF52EB9EA60E9A63279A9C599A012CA66F9F64FF90297DEDE452F750A
```

The focused tests, full solution tests, full solution build and whitespace
gate in section 5 were rerun after recording the manual PASS:
**141 focused passed; 467 full passed; zero failed/skipped; build succeeded
with zero warnings/errors; git diff --check passed.**
There were no new validation failures or corrections.

The complete final `git diff` was inspected for unrelated changes, firing-policy
changes, incorrect evidence promotion and stale current C2 pending wording.
The former one-shot uncertainty is explicitly labeled historical and superseded;
unrelated older milestone notes were not rewritten. The current C2 report and
notes record PASS rather than an unperformed gate. The README's production
Tanker integration deferral is still accurate, so it was left unchanged.

Final requested maintenance:

```text
git status --short
 M TEMPEST-BEHAVIOR-SPEC.md
 M TEMPEST-DEVELOPMENT-NOTES.md
 M src/Tempest.Game/Gameplay/Tanker.cs
 M tests/Tempest.Game.Tests/SimulationTests.cs

git diff --stat
 TEMPEST-BEHAVIOR-SPEC.md                    |  17 +-
 TEMPEST-DEVELOPMENT-NOTES.md                |  50 +++-
 src/Tempest.Game/Gameplay/Tanker.cs         |   5 +-
 tests/Tempest.Game.Tests/SimulationTests.cs | 350 +++++++++++++++++++++++++++-
 4 files changed, 407 insertions(+), 15 deletions(-)

git diff --check
(no whitespace errors; only existing Git LF/CRLF notices)

git status --short -- Reference/incitti
(empty)

git status --short -- TEMPEST-BEHAVIOR-SPEC.md
 M TEMPEST-BEHAVIOR-SPEC.md

git ls-files --others --exclude-standard
(empty)

git log -1 --oneline
27c43b7 Add validated controller restart and quit aliases
```

Local AI logs remain ignored; no forced addition is recommended. The saved
finalization prompt and updated implementation report persist locally.

**C2 is cleanly ready for Neal's selective commit. No blocker remains before
commit.** The worktree intentionally contains the validated uncommitted C2
changes; "ready" does not mean an already clean/committed worktree.

Recommended command, not executed:

```powershell
git add -- .\src\Tempest.Game\Gameplay\Tanker.cs .\tests\Tempest.Game.Tests\SimulationTests.cs .\TEMPEST-DEVELOPMENT-NOTES.md .\TEMPEST-BEHAVIOR-SPEC.md
```

Recommended commit message:

```text
Correct and validate isolated Tanker firing timing

Use literal Incitti early-tier climb speed and protect randomized firing.
Record C2 automated and qualitative manual PASS without production promotion.

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
```

No `git add`, commit or push was performed. C3, production Level 3 mixed
composition, Flipper bullet promotion and natural Level 2 -> Level 3
progression remain unauthorized and were not started.
