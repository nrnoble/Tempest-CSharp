# Final corrected Level 5 generic-obligation validation

Date: 2026-11-10

Status: **PASS - AUTOMATED VALIDATION COMPLETE.** The exact corrected candidate
is ready for Neal's final manual Level 5 acceptance decision. Overall Level 5
acceptance/closeout is **PENDING**, not established by this report.

## Scope and starting state

- The fail-closed check found exactly one standalone completion token in the
  substantive task. The complete task is preserved in the matching ignored
  [prompt](../Prompts/2026-11-10-level5-final-generic-obligation-validation-prompt.md).
- Branch: `master`; configured upstream: `origin/master`.
- HEAD, local `origin/master`, and the resolved upstream all matched
  `62c66e0ab19436ea162935627021154be21e0a41`. No fetch was performed; this is
  verification of the available repository refs, not a claim about a newly
  queried remote.
- Index: no staged changes (`git diff --cached --quiet` returned 0).
- Complete candidate: six modified tracked files and two nonignored untracked
  files, listed below. No other nonignored untracked files were present.
- The complete tracked diff and both untracked candidate files were read
  before testing. The candidate matched the scope described in the
  [latest reconstruction handoff](2026-11-10-level5-generic-obligation-death-reconstruction-report.md)
  and relevant historical admission/restoration reports. No unrelated change
  was found. Historical reports were not rewritten.
- Starting `git diff --check`: **PASS**, exit 0.
- No gameplay, test, configuration, dependency, or authoritative behavior
  documentation changes were made during this validation. Only this ignored
  report and its matching ignored prompt were created.

### Complete candidate file list

Modified tracked files:

- [Level.cs](../../src/Tempest.Game/Levels/Level.cs)
- [Level3WaveLedger.cs](../../src/Tempest.Game/Levels/Level3WaveLedger.cs)
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs)
- [Level3WaveLedgerTests.cs](../../tests/Tempest.Game.Tests/Level3WaveLedgerTests.cs)
- [Level5ProductionTests.cs](../../tests/Tempest.Game.Tests/Level5ProductionTests.cs)

Untracked candidate files:

- [Level5AdmissionPolicy.cs](../../src/Tempest.Game/Levels/Level5AdmissionPolicy.cs)
- [Level5AdmissionPolicyTests.cs](../../tests/Tempest.Game.Tests/Level5AdmissionPolicyTests.cs)

Tracked diff: 326 insertions, 91 deletions across six files; this statistic
does not include the two untracked policy files.

## Preliminary manual result supplied by Neal

**PASS for the specific previously failing behavior**, on this exact corrected
candidate:

- No individual Flippers came up from the deep/bottom end during normal
  Level 5 play.
- No individual Flippers came up from the deep/bottom end after Blaster
  destruction/reentry.

This is Neal's preliminary observation, not an agent-run manual test or overall
Level 5 acceptance. It was not substituted for any automated stage.

## Final syntax and refinement checks

- The extra opening brace in the retained-original selector is absent;
  `CanAdmitRetainedOriginal` has a valid switch expression and method closure.
  Successful compilation of both projects independently confirms that the
  previously reported CS1513 is no longer present.
- Canonical Level 5 cargo Eggs raise an explicit invariant error before the
  generic cargo hatch path. Retained admission allows Tanker/Spiker only;
  retained Flipper species are not admitted.
- Direct `Level.ResetAfterDeath()` forces generic reconstruction only for the
  canonical Level 5 configuration. Copied Level 5 configurations and direct
  Level 1-4/6 reset scope were not broadened. Existing Level 7/8 policy remains.
- Generic reconstruction transfers each unresolved materialized cargo child
  into one new unselected original obligation, terminalizes its historical
  cargo identity, and removes latent cargo instead of counting it as additional
  active supply. There is no captured-position restoration path in this diff.
- The accepted Level 5 admission/conversion direction was not reopened or
  redesigned. No defect requiring a correction was exposed.

## Automated results for this exact candidate

Environment: .NET SDK `9.0.304`, `net9.0`, xUnit v2 through
`Microsoft.NET.Test.Sdk`/Visual Studio adapter, VSTest `17.14.1` x64.
No `global.json`, `Directory.Build.props`, or `Directory.Packages.props` test
runner override was found in the repository locations checked. Restore was
up to date; no dependency installation was performed.

Each test command used normal Release build/compile processing, not
`--no-build`. Each displayed both project outputs, VSTest's start-of-execution
message, one matched test assembly, and the explicit result counts below.
PASS was not inferred from exit status alone. No failure, timeout,
cancellation, partial run, or blocked stage occurred.

| Stage | Compilation | VSTest executed | Passed | Failed | Skipped | Status |
|---|---|---|---:|---:|---:|---|
| Smallest reconstruction group | PASS, both projects | Yes | 7 | 0 | 0 | PASS |
| Complete focused Level 5/ledger/cargo groups | PASS, both projects | Yes | 88 | 0 | 0 | PASS |
| Affected regression groups | PASS, both projects | Yes | 767 | 0 | 0 | PASS |
| Full solution Release tests | PASS, both projects | Yes | 1,171 | 0 | 0 | PASS |
| Full solution Release build | PASS, 0 warnings / 0 errors | No, build only | N/A | N/A | N/A | PASS |

These are fresh results for the final corrected candidate, not the historical
passes in the preceding handoff. The affected-stage selector below differs
from the earlier historical 814-test selection; its actual executed count is
767. The subsequent unfiltered 1,171-test solution run also passed. Stages
overlap and their counts must not be added as unique tests.

### Commands, in executed order

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~GenericDeathReconstruction|FullyQualifiedName~EightReconstructedCargoObligationsUseOrdinaryTankerAndSpikerSelection|FullyQualifiedName~DeathReconstructsReleasedLevelFiveCargoAsGenericObligations" --blame-hang-timeout 5min

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~Level5AdmissionPolicyTests|FullyQualifiedName~Level5ProductionTests|FullyQualifiedName~Level3WaveLedgerTests|FullyQualifiedName~Level3MixedWaveTests" --blame-hang-timeout 5min

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --configuration Release --filter "FullyQualifiedName~SimulationTests|FullyQualifiedName~Level1ProductionFlipperCapTests|FullyQualifiedName~Level2|FullyQualifiedName~Level3|FullyQualifiedName~Level4|FullyQualifiedName~Level5|FullyQualifiedName~LevelConfigurationTests|FullyQualifiedName~LevelProgressionTests|FullyQualifiedName~SuperZapperTests|FullyQualifiedName~PlayerDeathPresentationTests|FullyQualifiedName~RimPursuitTests|FullyQualifiedName~DirectLevelStartupTests|FullyQualifiedName~CampaignSoakTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlaybackTests|FullyQualifiedName~GameplayPlaybackSessionTests|FullyQualifiedName~HeadlessGameplayPlaybackTests|FullyQualifiedName~VisiblePlaybackControllerTests" --blame-hang-timeout 5min

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

dotnet build TempestCSharp.slnx --configuration Release
```

### Coverage interpretation

The seven-test first stage contains five `GenericDeathReconstruction` ledger
tests, the eight-obligation admission policy test, and the released-cargo
production reconstruction test. Together they check:

- Four unresolved cargo Flippers become four fresh generic obligations.
- Four mixed active species lose retained selections without adding latent
  cargo to the active obligation quantity.
- Eight unresolved cargo Flippers become eight unselected obligations;
  subsequent real cargo release changes the unresolved quantity normally, and
  a repeated death conserves that current quantity.
- Terminal and unresolved cargo siblings remain distinct.
- Pending-shot and active cargo transfer separately.
- No cargo Flipper Eggs or restored in-tube/rim Flippers remain immediately
  after production reconstruction; all reentry Eggs have original identities.
- Reconstructed supply admits only Tanker/Spiker, not Flipper.

The 88-test stage adds complete Level 5 admission/production, shared ledger,
and mixed-wave tests, including minima/maxima, seven-place headroom,
shortest-Spike threshold translation, cargo reservation, blocked-Egg waiting,
terminal conversion, converted-Tanker admission, direct reset, and copied
configuration isolation.

The affected stage covers Tanker cargo, identity/accounting, death/reentry,
terminal Spiker conversion, converted Tankers, Super Zapper, persistent Spikes,
completion/all-rim, direct/campaign/recorded startup and playback, and Levels
1-4 concurrency. The full solution additionally executes the existing
Level 6-8 and remaining regression groups. These tests are regression
evidence, not proof of cycle-accurate arcade behavior.

## Windows security outcome

No Windows Application Control block, signing/trust failure, or security error
was reported by any command in this validation. VSTest loaded the Release test
assembly and executed all requested stages. No equivalent-route retry,
artifact relocation, policy change, signing change, or bypass was attempted.
Historical Debug security failures were not reproduced or used as current
validation evidence.

## Final static review and repository state

- Final `git diff --check`: **PASS**, exit 0.
- Complete final tracked production/test diff reviewed; both untracked policy
  files were reviewed before testing and remained byte-identical afterward.
- All eight candidate files had matching start/end SHA-256 fingerprints.
  Thus validation did not silently alter the exact candidate.
- HEAD, local `origin/master`, and upstream remain
  `62c66e0ab19436ea162935627021154be21e0a41`; branch/upstream remain
  `master` / `origin/master`.
- Index remains unstaged. The same six modified tracked files and two
  untracked candidate files remain; no unrelated nonignored changes appeared.
- The new prompt and this report are ignored under `AI-Logs/`; they do not
  enter the candidate's tracked diff.
- No commit, push, interactive game launch, or Level 6 concurrency work was
  performed.

## Handoff and limits

**Ready for Neal's final manual Level 5 acceptance decision.** The exact
corrected candidate remains uncommitted. No overall acceptance/closeout or
durable accepted-behavior documentation change is claimed.

Neal's launch command (not executed by the agent):

```powershell
dotnet run --project src\Tempest.Game -- --lives 10
```

For final Level 5 acceptance, confirm the preliminary no-independent-bottom-
Flipper result during normal play and death/reentry, along with ordinary
Tanker/Spiker entry, normal live Tanker cargo, repeated-death quantity,
Spikes, Super Zapper, rim combat and completion. Automated deterministic
regressions supplement, but do not replace, that manual decision.
