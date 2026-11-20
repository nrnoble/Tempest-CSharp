# Resume interrupted C3a Level 3 mixed-wave implementation

Resume the interrupted C3a implementation from the current uncommitted
worktree. Do not restart C3a from scratch. Do not restore, discard, revert or
overwrite existing C3a work wholesale. The implementation and analysis
authorities are:

- `2026-11-01-c3-level3-mixed-wave-implementation-prompt.md`
- `2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-report.md`

The starting committed checkpoint is
`6d15165 Correct and validate isolated Tanker firing timing`.

## Recovery sequence

1. Inspect Git status, diff stat/check, HEAD, all modified/untracked C3a files
   and any existing C3a report before editing. Classify every C3a requirement
   as implemented, partial, test-only, missing or inconsistent.
2. Diagnose the reported targeted compile failure first. The interrupted
   `Level3MixedWaveTests.cs` referenced a missing/inaccessible
   `BoundedCompositionRandom`; the next constructor diagnostic said
   `LevelConfiguration` could not convert to `bool`; xUnit2030 also flagged
   `Assert.NotEmpty`. Fix these narrowly using the existing random and
   configuration seams, not duplicate abstractions.
3. Rerun:

   ```powershell
   dotnet test .\tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~ProductionWaveEnforcesSixSimultaneousTankers"
   ```

   Diagnose exact behavioral failures without weakening the six-simultaneous-
   Tanker requirement.
4. Continue implementing the complete original C3a gate from the saved
   implementation prompt. Passing the recovered test alone is not completion.
   The gate includes production mixed configuration and source-shaped
   Level-3-only scheduling/selection; stable original and separately owned
   cargo identities; exact runtime representation and duplicate checks;
   natural, shot and zap carrier outcomes; multi-carrier Super Zapper;
   selected-type retention and six-capacity waiting; production Tanker firing
   without promoting default Flipper bullets; death/re-entry identity
   conservation; all-rim and normal completion equality; actual
   `TempestGame` orchestration; terminal headless Level 3 behavior; and every
   old scenario.
5. Inspect interrupted code for duplicate ledger truth, fixed-slot remnants,
   count/set mixing, duplicate cargo allocation, contradictory cargo
   locations, Tanker rerolls, cap-loop stalls, firing-flag coupling, default
   Flipper arming, Zapper ordering/stale timers, incomplete completion and
   death-reconstruction omissions. Repair incrementally.
6. Preserve the C3a boundary: no `--level3-test`, Program routing, production
   HUD/rendering policy, graphical/manual access, or Level 2-to-Level 3
   progression. Do not modify `LevelProgression`, Level 2 exit commit,
   `Reference/incitti`, accepted geometry/cargo route/pivot, C2 firing
   constants, Super Zapper semantics, silent default Flippers, or
   spinner/controller behavior.
7. Add and run deterministic, strict, finite acceptance tests from the saved
   C3a prompt. Preserve Level 1, Level 2 and all established development
   scenarios.
8. Run the required focused C3a test groups, then:

   ```powershell
   dotnet test TempestCSharp.slnx --no-restore
   dotnet build TempestCSharp.slnx --no-restore
   git diff --check
   ```

9. Inspect final status, stat, complete diff, whitespace, reference/spec
   status, untracked files and HEAD; inspect every changed/new file.
10. Update/create the C3a implementation report with a section titled
    **Interrupted-agent recovery**. Preserve the stalled/canceled history,
    initial clean diff check, compile errors and their resolution, every
    additional recovery defect, test/build outcomes, exact changed paths,
    C3b/C4 boundaries and any blocker.
11. Save this recovery prompt under `AI-Logs/Prompts` using the established
    naming convention. Do not commit or push.

C3a is complete only when the complete saved C3a gate is met. Final reporting
must state recovery safety, compile root cause, pre-existing versus newly
completed work, corrected structural issues, focused/full test results, build
and diff-check results, exact changed/untracked paths, `Reference/incitti`
status, C3b/C4 status, review/commit readiness and any remaining blocker.
