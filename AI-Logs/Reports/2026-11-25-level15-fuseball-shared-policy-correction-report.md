# Level 15 Fuseball shared-policy correction

**Status: AUTOMATED VALIDATION PASS — NEAL'S MANUAL ACCEPTANCE PENDING.**

## 1. Executive summary

Level 15 Fuseballs now receive the shared generic pending-original count when
the configuration selects the Level-11-derived parameter-development baseline.
Positive supply preserves ordinary cycling; zero supply enters the existing
shared `TerminalRimward` movement at its existing boundary. Level 15 remains a
project baseline, not genuine Level 11 ROM policy. No Level-specific actor,
state, ledger, or lifecycle was added.

## 2. Starting Git/worktree state

- Branch `master`, HEAD `4c7365a01f944cf2b501164e5015f4954110434f`,
  upstream `origin/master`.
- Tracked worktree and index were clean; no staged changes.
- Pre-existing untracked paths were five AI prompt/report pairs (the
  DebugMCP prerequisite, continuation, and installation records; the
  Level 11/15/17 audit records; and the VS Code development-tool assessment)
  plus `Reference/arcade/MBUCOD.V05.txt`.
- All pre-existing untracked material was preserved. The local arcade
  reference was not opened.

## 3. Architecture-audit finding

The completed 2026-11-25 architecture audit found one active gap: Level 15
can select a Fuseball through the shared Level 11 selector, but
`UpdateFuseballs` omitted the Level-11-derived baseline flag when supplying
generic pending supply. The resulting `null` cannot activate the existing
zero-pending terminal branch. The audit found no duplicate actor, ledger,
reconstruction, collision, or completion lifecycle.

## 4. Current Level 15 baseline authority

`Level15Production` has displayed level 15, effective gameplay parameter
level 11, and `UsesLevel11ParameterDevelopmentBaseline = true`, while
`UsesLevel11RomWavePolicy` remains false. The behavior specification,
Level 15 promotion/baseline records, and accepted terminal-Spiker correction
document this as a deliberate project baseline. No Level 15 exception to the
shared Level-11-derived terminal Fuseball behavior was found. This correction
does not claim complete Level 11 or Level 15 ROM identity.

## 5. Static defect reconfirmation

The current source matched the audit before editing:

- Level 15's baseline branch in `CreateLevel3MixedEnemy` calls
  `Level11WaveSelection.SelectOriginalType`.
- Level 11's selector requires one Fuseball (`1/1`) and its shared production
  path admits one.
- `UpdateSpikers` already included the Level 15 baseline for pending-count
  sourcing, positive-pending restart, and zero-pending conversion.
- `UpdateFuseballs` included Level 11 ROM policy and Levels 12-14/16, but
  omitted `UsesLevel11ParameterDevelopmentBaseline`.
- `Fuseball.Update` enters the shared `TerminalRimward` state only when it
  receives zero at the existing toward-rim boundary. A `null` count does not
  enter that branch.
- Level 17's selector has zero minimum/maximum Fuseballs and does not select
  that species.

## 6. Fuseball eligibility path

The deterministic Level 15 production path using seed 4 selected original
identity 4 as a Fuseball. The new test confirms the ledger records that same
identity as `Fuseball/ActiveFuseball`, with generic supply still pending.
Level 15 eligibility was not broadened or changed.

## 7. Pending-supply call path

`Level3WaveLedger.PendingGenericOriginalCount` counts generic original
Reserve/Egg states. `TempestGame.UpdateFuseballs` supplies it to the shared
`Fuseball.Update`; the actor's existing boundary decision then chooses
ordinary cycling or terminal-rimward movement.

## 8. Level 11 comparison

Genuine Level 11 still uses `UsesLevel11RomWavePolicy`. Its selector and
Fuseball lifecycle are unchanged. Existing Level 11 positive-pending,
zero-pending, terminal movement, identity, shot, death, and completion tests
passed in the focused matrix.

## 9. Level 15 omission

Before the fix, the Level 15 baseline passed the selector but failed the
Fuseball update predicate, yielding `pendingGenericSupply = null`. The shared
zero-pending condition therefore could not engage.

## 10. Intended baseline-scope evidence

The Level 15 production-promotion and baseline records establish the
Level-11-derived parameter/selector baseline. The terminal-Spiker correction
already wires that baseline into the shared pending-supply lifecycle. The
architecture audit explicitly classified the Fuseball issue as another
missing connection to shared behavior and found no contrary Level 15 rule.
No new ROM claim was required or made.

## 11. Semantic-policy abstraction decision

Added `LevelConfiguration.UsesLevel11DerivedGenericPendingSupplyPolicy`,
derived from the existing ROM-policy and parameter-development-baseline
flags. This is the smallest abstraction that names the shared semantic
policy. `UpdateSpikers` and `UpdateFuseballs` now consume that same predicate
for their Level-11-derived lifecycle decisions; the remaining independent
Level 7-10 and Level 12-16 policy gates were not refactored.

## 12. Production correction

`UpdateFuseballs` now supplies the ledger's generic pending-original count
when `UsesLevel11DerivedGenericPendingSupplyPolicy` is true. `UpdateSpikers`
uses the same derived predicate in its existing pending-count,
pending-positive restart, and zero-pending conversion decisions. Those
replacements preserve the prior Level 11 and Level 15 Spiker behavior.

## 13. Why the correction is generic rather than Level-specific

The baseline flag is not copied into the genuine Level 11 ROM-policy flag.
The same `Fuseball`, `Fuseball.Update`, `TerminalRimward` state, identity
ledger, collision, death/re-entry, and completion paths remain in use. No
Level 15 class, state, counter, or special completion condition was added.

## 14. Files changed

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level15FuseballPolicyTests.cs`
- `AI-Logs/Prompts/2026-11-25-level15-fuseball-shared-policy-correction-prompt.md`
- This report.

No behavior specification or development-notes update was made before Neal's
manual acceptance.

## 15. Tests added/changed

The new focused class verifies Level 15 production selection, positive
pending supply, zero-pending terminal movement, identity continuity,
completion, player-shot destruction, death/re-entry, the shared policy
predicate, existing positive-pending behavior for Levels 12-14/16, and Level
17 Fuseball ineligibility. Existing shared Level 11 and Level 15 terminal
Spiker tests were run unchanged.

The first shot-destruction test attempt was **FAIL** because a Level 15
starting Spike intercepted the shot before it reached the Fuseball. The
fixture now clears only the coincident test-lane Spike; production behavior
was not changed. The focused class then passed.

## 16. Level 15 pending-positive result

**PASS.** Seed 4 selects original 4 as a production Fuseball while generic
pending supply remains positive. After a controlled ordinary climbing update,
the same actor remains `ActiveFuseball` and does not enter
`TerminalRimward`.

## 17. Level 15 zero-pending terminal result

**PASS.** With pending supply at zero, a production-update tick at the
established final rimward-step boundary changes the same Fuseball to
`TerminalRimward` without an invented traversal count. The following movement
uses the existing normal climb speed.

## 18. Identity/ledger result

**PASS.** The original ID remains represented as `Fuseball/ActiveFuseball`
through terminal movement. Ledger validation passed in the bounded fixtures.
At the rim, the existing all-rim path resolves the ID as `RimCleared`.

## 19. Completion result

**PASS.** Completion remains blocked while the terminal Fuseball is below the
rim. Once it reaches the rim and the other fixture obligations are resolved,
the existing transition reaches `ExitDelay`.

## 20. Death/reconstruction result

**PASS.** Death during terminal movement removes the runtime object and
reconstructs the same unresolved original ID as a Reserve or Egg, represented
exactly once.

## 21. Player-shot destruction result

**PASS.** A terminal Fuseball remains hittable through the normal shared
player-shot collision path. The same original is marked `Destroyed`.

## 22. Policy-consistency regression coverage

**PASS.** The new semantic property is true for genuine Level 11 and the
Level 15 baseline, false for Level 17, and Level 15 remains explicitly
`UsesLevel11RomWavePolicy = false`. Production Fuseball tests exercise the
shared caller; existing Level 15 terminal-Spiker tests exercise the other
shared consumer.

## 23. Level 11 regression result

**PASS.** `FuseballTerminalMovementTests` and `Level11ProductionCorrectionTests`
passed. The shared policy substitution preserves the genuine Level 11 flag's
meaning and existing terminal behavior.

## 24. Levels 12-14/16 regression result

**PASS.** Four focused production-policy cases confirm that Levels 12, 13,
14, and 16 continue to keep an active Fuseball nonterminal while one generic
original remains pending.

## 25. Level 15 Spiker regression result

**PASS.** `Level15TerminalSpikerTests` passed, preserving positive-pending
restart, zero-pending in-place conversion, identity continuity, and Tanker
cargo ownership.

## 26. Level 17 ineligibility result

**PASS.** Level 17 retains Fuseball min/max `0/0`; its production test remains
empty of Fuseballs after 200 ticks. No dormant parameter was enabled.

## 27. Level 15 seed-1982 result

**PASS.** `Level15ProductionSoakTests` ran both seeded cases, including seed
1982, for 1,200 ticks with identity and deterministic-run assertions.

## 28. Levels 1-16 recording result

**PASS — replay completed; no sampled summary divergence detected.** The
read-only replay of `TEMP/GameplayRecordings/lvl1-16.json` completed all
35,350 ticks. A temporary Release build from unmodified HEAD
`4c7365a01f944cf2b501164e5015f4954110434f` was compared with the corrected
build at boundaries 29,000; 30,000; 31,000; 31,500; 32,000; 32,500; 33,000;
and 35,350. The summaries matched for displayed level, level state, score,
lives, and active Egg/Flipper/Tanker/Spiker/Fuseball/Pulsar counts. The
recording had one active Fuseball at boundaries 31,000 and 31,500, none at
32,000, and was in `ExitDelay` at 32,500 in both builds.

The headless recording format stores inputs, not a golden per-tick simulation
trace, and its summaries do not expose Fuseball movement state. Therefore
these matching checkpoints do not establish frame-by-frame movement
equivalence; no sampled gameplay-state divergence was observed. The temporary
baseline source/build was removed. The recording was not modified.

## 29. Level 17 recording result

**PASS.** `TEMP/GameplayRecordings/lvl-17-test.json` completed all 2,609 ticks
without modification. It ended at Level 17 `Complete`, with no active
Fuseballs.

## 30. Focused validation result

**PASS — 224 tests, 0 failed, 0 skipped.** The bounded VSTest filter covered
the new Level 15 policy tests; Level 15 production, seed soak, and terminal
Spiker; shared Fuseball terminal movement; Level 11; Levels 12-14/16/17;
ledger/mixed-wave; all-rim; and reconstruction tests. The command used
`--blame-hang-timeout 5min`.

The new focused policy class separately passed **10 tests** with
`--blame-hang-timeout 2min`.

## 31. Full-suite result

**PASS — 2,075 passed, 0 failed, 0 skipped.** VSTest used
`--blame-hang-timeout 5min`.

## 32. Release-build result

**PASS.** `dotnet build TempestCSharp.slnx --configuration Release
--no-restore --verbosity minimal` completed with 0 warnings and 0 errors.

## 33. `git diff --check`

**PASS.** Tracked changes passed `git diff --check`; the new test and copied
prompt were separately scanned for trailing whitespace with no matches.

## 34. Manual acceptance instructions

No interactive game was launched. After automated validation, Neal should run:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 15
```

When a Level 15 Fuseball remains late in the wave after generic supply is
exhausted, does it now behave like the accepted shared terminal Fuseball
behavior rather than cycling indefinitely? Also watch for no obvious Level 15
regression, normal Flippers/Tankers/Spikers, and natural completion.

## 35. Proposed durable semantic-policy wording

> Configuration flags that represent a shared semantic lifecycle policy must
> be consumed through a shared semantic predicate/helper where practical,
> rather than independently reconstructing equivalent flag lists at multiple
> actor-update call sites. This reduces the risk that adding a new level
> policy to one lifecycle path leaves another shared actor path unwired.

Do not add this durable wording until Neal accepts the behavior.

## 36. Remaining uncertainties

- Neal's manual Level 15 gameplay acceptance is pending.
- Recording comparison covered complete playback and sampled boundary
  summaries, not exact per-tick Fuseball state.
- No independent Level 15 ROM Fuseball policy was asserted or investigated;
  the implementation applies the accepted project baseline through shared
  behavior.

## 37. Final Git/worktree state

Branch and HEAD remain `master` at
`4c7365a01f944cf2b501164e5015f4954110434f`. The intended tracked changes are
the two production files above; the new test and current prompt/report are
untracked. All pre-existing untracked records and the arcade reference remain
untouched.

## 38. Stage/commit/push

**None.** Nothing was staged, committed, or pushed.

## 39. Level 18+ work

**None.** Level 18+ implementation did not begin. Level 17 behavior was not
reopened.

## 40. Recommended next step

Neal manually evaluates the Level 15 terminal-wave behavior using the command
above. After acceptance, authorize a separate closeout to update the durable
behavior documentation with the accepted project translation and semantic
policy rule. No further source change is recommended before that acceptance.
