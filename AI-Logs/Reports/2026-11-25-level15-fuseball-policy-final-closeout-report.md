# Level 15 Fuseball Shared-Policy Correction Final Closeout

Date: 2026-11-25

## 1. Executive summary

**STATUS: PRE-COMMIT CLOSEOUT READY.** Neal manually accepted the Level 15
shared Fuseball pending-supply correction. The Level 11/15/17 architecture
audit's bounded generic-lifecycle conclusion is preserved, and the semantic
lifecycle-policy safeguard is now in the durable behavior and development
records. The approximately 20-second terminal traversal observation is
explicitly deferred and is not treated as a defect or evidence of an Atari
hurry-up mode.

Fresh automated validation passed: 190 focused tests, including Level 15
seed-1982 coverage; the complete 2,075-test suite; both required complete
recording replays; and a Release build with zero warnings and zero errors.
No gameplay source behavior was changed during this closeout.

## 2. Starting Git/worktree state

Before closeout edits:

- Branch: `master`
- HEAD: `4c7365a01f944cf2b501164e5015f4954110434f`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: clean; no staged changes.
- Tracked worktree changes: the two expected policy-correction files:
  `src/Tempest.Game/Levels/LevelConfiguration.cs` and
  `src/Tempest.Game/TempestGame.cs`.
- Untracked expected test: `tests/Tempest.Game.Tests/Level15FuseballPolicyTests.cs`.
- Untracked AI continuity records: six existing prompt/report pairs covering
  the architecture audit, Level 15 correction, VS Code development-tool
  assessment, and three DebugMCP experiment records.
- Unrelated protected untracked reference: `Reference/arcade/MBUCOD.V05.txt`.
  It was not opened, searched, changed, or staged.
- No staged work was overwritten or discarded.

Ignored local asset directories were inspected at metadata level only.
`TEMP/`, `UserData/`, `Videos/`, and `Reference/arcade/roms.rev2/` remain
protected and excluded. Recordings were read only by the explicitly requested
headless playback commands; they were not modified.

## 3. Architecture-audit conclusion

**BOUNDED GENERIC-LIFECYCLE PROBLEM — RESOLVED BY THE ACCEPTED CORRECTION.**
The audit found no separate Level 11/15/17 Flipper actor, Tanker actor,
identity ledger, death/reconstruction engine, completion system, collision
engine, or active target-level rim-pursuit implementation. Below-rim movement
strategies converge on shared rim lifecycle. The target-level differences are
predominantly configuration or reusable strategy selection. The project
does not have three separate Level 11/15/17 actor engines.

The single active gap was Level 15's Fuseball pending-supply policy wiring.
It is corrected and manually accepted. The architecture audit was not
reopened or expanded into a broad refactor.

## 4. Original Level 15 policy-wiring defect

`Level15Production` could select a Fuseball through the accepted Level-11
parameter-development baseline. The shared Spiker lifecycle already sourced
generic pending supply for that baseline, but the Fuseball lifecycle omitted
it. `UpdateFuseballs` therefore passed `null` rather than the ledger's generic
pending-original count, so zero supply could not activate the shared
terminal-rimward transition.

This was a shared-policy wiring omission, not a separate Level 15 actor or
evidence of an independent Level 15 ROM terminal program.

## 5. Shared semantic policy correction

`LevelConfiguration.UsesLevel11DerivedGenericPendingSupplyPolicy` derives
from the existing genuine Level 11 ROM-policy flag and the separately
accepted Level 15 parameter-development-baseline flag. The shared Spiker and
Fuseball consumers use the same semantic predicate for applicable
Level-11-derived pending-supply lifecycle decisions. Fuseballs receive the
ledger's generic pending-original count for either applicable configuration.

No target-level actor, state, ledger, movement rule, timer, or completion
condition was added. Existing Fuseball movement and terminal-state behavior
were not changed.

## 6. Why the correction is generic

Both shared actors ask whether the active configuration uses the
Level-11-derived generic pending-supply policy. A common predicate prevents
the precise synchronization failure found in the audit: adding a baseline
to one actor's flag list but omitting it from another shared consumer. This
small semantic helper avoids introducing a broad policy framework.

## 7. Level 15 baseline authority boundary

Level 15 remains `UsesLevel11ParameterDevelopmentBaseline`, not genuine
`UsesLevel11RomWavePolicy`. Its accepted Level 11 parameter/selector baseline
does not establish that Level 15 has complete Level 11 or Level 15 ROM
gameplay identity. Applying the shared pending-supply translation is a
PROJECT TRANSLATION accepted for this correction; Neal's manual acceptance
does not promote it to ROM-backed evidence.

## 8. Level 11 preservation

Genuine Level 11 remains selected by `UsesLevel11RomWavePolicy`. Its
eligibility, wave selection, shared Fuseball terminal translation, and
Spiker behavior remain unchanged. The semantic predicate is true for Level
11 without changing the meaning of its genuine ROM-policy flag.

## 9. Level 15 Spiker preservation

Level 15's accepted terminal-Spiker restart/conversion policy is unchanged.
The Spiker consumer now uses the same derived predicate already covering
Level 15, with the same pending-positive restart and zero-pending in-place
conversion behavior. Existing Level 15 terminal-Spiker regression tests
passed.

## 10. Level 17 preservation

Level 17 remains ineligible for Fuseballs under its zero-minimum/zero-maximum
policy. It is false for the new Level-11-derived predicate. Level 17 route,
rim handoff, palette, eligibility, and accepted gameplay were not reopened or
changed.

## 11. Manual acceptance

**MANUAL ACCEPTANCE — LEVEL 15 SHARED FUSEBALL POLICY CORRECTION.** Neal
manually tested Level 15 after the correction and observed:

- after generic supply reached zero, the remaining Fuseball entered the
  established terminal-rimward behavior;
- the Fuseball eventually reached the rim;
- the previously concerning indefinite ordinary cycling was not observed;
- Level 15 otherwise played acceptably for this correction milestone.

Neal accepts the intended correction. No further Level 15 Fuseball-policy
source change is requested. No interactive game was launched during this
closeout.

## 12. Terminal Fuseball manual observation

During the manual test, Neal waited at least approximately 20 seconds for the
terminal Fuseball to reach the rim. This is a manual observation of the C#
play session, not a measured arcade comparison.

## 13. Deferred approximately-20-second traversal observation

**DEFERRED FIDELITY QUESTION — NOT A CURRENT DEFECT.** Neal did not observe
an obvious hurry-up or acceleration mode during the traversal and explicitly
does not want to pursue the question at this time. The manual test did not
establish absolute terminal-traversal timing against the arcade.

Do not infer that Atari necessarily has a hurry-up mode, that C# is wrong,
that traversal is definitely too slow, or that shared terminal speed is
incorrect. No speed, timer, Fuseball movement, or terminal-state change was
made. Do not require Neal to repeat the observation.

## 14. Durable semantic-policy requirement

Durable documentation now states that configuration flags representing a
shared semantic lifecycle policy should, where practical, be consumed
through a shared semantic predicate/helper rather than independently
reconstructed at actor-update call sites. It explains that Level 15's
Spiker path already used the Level-11-derived policy while its Fuseball path
did not, and that both actors should ask the same semantic configuration
question.

## 15. Route-semantics caution

Durable guidance now records that movement-strategy identifiers must
describe their actual behavior and initialization. A shared ROM byte alone
does not prove identical host lifecycle semantics. Distinct route
initialization may be legitimate; every below-rim strategy still converges
on the applicable shared lifecycle unless authoritative ROM behavior
establishes otherwise. The existing `Straight07` /
`Level17Straight07` distinction was not renamed or changed.

## 16. Future-level development requirement

The preserved discipline is to establish eligibility before parameters;
establish configuration; choose an existing matching shared strategy; add a
reusable strategy only for genuinely distinct behavior; connect it to shared
actor lifecycle and semantic policy predicates; and add regression coverage
for applicable earlier levels. This is a safeguard, not authorization for
Level 18+ implementation.

## 17. Files changed

Closeout changes:

- `.github/copilot-instructions.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `AI-Logs/Prompts/2026-11-25-level15-fuseball-policy-final-closeout-prompt.md`
- `AI-Logs/Reports/2026-11-25-level15-fuseball-policy-final-closeout-report.md`

The following expected pre-existing candidate changes are included without
changing their gameplay behavior:

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level15FuseballPolicyTests.cs`

## 18. Tests added/updated

`Level15FuseballPolicyTests` permanently covers production Fuseball
eligibility, positive generic pending supply, zero pending supply, shared
`TerminalRimward`, identity continuity, completion blocking, player-shot
destruction, death/reconstruction, semantic predicate scope, Levels 12-14/16
policy preservation, and Level 17 ineligibility. Existing tests cover
genuine Level 11 policy and Level 15 Spiker behavior. The Level 15 production
soak includes deterministic seed 1982.

## 19. Focused validation

**PASS — 190 passed, 0 failed, 0 skipped.** The focused VSTest matrix covered
`Level15FuseballPolicyTests`, `FuseballTerminalMovementTests`,
`Level11ProductionCorrectionTests`, `Level15TerminalSpikerTests`,
`Level15ProductionSoakTests`, ledger/mixed-wave coverage, Levels 12-17
production coverage, and `RimPursuitTests`. It used a five-minute
`--blame-hang-timeout`.

SDK: `9.0.304`; VSTest: `17.14.1`.

## 20. Level 15 seed-1982

**PASS.** `Level15ProductionSoakTests.SeededProductionSoakConservesIdentitiesAcross1200Ticks`
ran both configured seeds, including seed 1982, for 1,200 ticks with
determinism and identity-conservation assertions.

## 21. Levels 1-16 recording result

**PASS — complete playback.** The unmodified
`TEMP/GameplayRecordings/lvl1-16.json` completed all 35,350 recorded frames:

- `achievedBoundary`: 35,350; `remainingFrames`: 0;
- `completionReason`: `RecordingCompleted`;
- final summary: displayed Level 17, `Playing`, score 134,039, 9 lives;
- player lane 7, lane position 0.5, depth 400, two Super Zapper charges;
- zero player shots and enemy bullets; four Eggs;
- zero active Flippers, Tankers, Spikers, Spikes, Fuseballs, and Pulsars.

Playback completion means all recorded frames were consumed; the resulting
simulation state at the last frame is Level 17 Playing. No recording content
was modified.

## 22. Level 17 recording result

**PASS — complete playback.** The unmodified
`TEMP/GameplayRecordings/lvl-17-test.json` completed all 2,609 recorded
frames:

- `achievedBoundary`: 2,609; `remainingFrames`: 0;
- `completionReason`: `RecordingCompleted`;
- final state: Level 17 `Complete`, score 5,975, 8 lives;
- player lane 4, lane position 0.9273126, depth 7.7998915, two Super Zapper
  charges;
- zero player shots, enemy bullets, Eggs, Flippers, Tankers, Spikers, Spikes,
  Fuseballs, or Pulsars.

No recording content was modified.

## 23. Full-suite result

**PASS — 2,075 passed, 0 failed, 0 skipped.** The complete solution test
suite used VSTest with `--blame-hang-timeout 5min`.

## 24. Release-build result

**PASS.** `dotnet build TempestCSharp.slnx --configuration Release
--no-restore --verbosity minimal` completed with zero warnings and zero
errors.

## 25. Whitespace checks

**PASS.** Pre-staging `git diff --check` and staged `git diff --cached --check`
both pass. The first staged check identified Markdown hard-break trailing
spaces in eight lines of the historical architecture-audit prompt. Those
line-break markers were changed to equivalent backslash hard breaks; no
prompt wording or report content was rewritten. Both whitespace checks then
passed.

## 26. Durable documentation updates

- `TEMPEST-DEVELOPMENT-NOTES.md` records manual acceptance, the bounded
  architecture conclusion, the derived shared-policy correction, route
  semantics, future-level discipline, validation summary, and deferred
  traversal observation.
- `TEMPEST-BEHAVIOR-SPEC.md` records the accepted project translation,
  authority/evidence boundaries, semantic-policy rule, route caution,
  future-level discipline, validation summary, and deferred timing question.
- `.github/copilot-instructions.md` records semantic policy consumption,
  route semantics, and future-level regression discipline.
- README was not changed; user-facing usage is unaffected.

## 27. AI-Logs inventory and safety scan

The intended continuity set is seven current prompts and seven paired
reports: the architecture audit, Level 15 policy correction, final closeout,
VS Code development-tool assessment, and three DebugMCP experiment records.
Final inventory: 14 untracked AI-Logs files (7 prompts, 7 reports).
`AI-Logs/Copilot.txt` is already tracked and remains unchanged. The
architecture-audit and correction prompt/report records are preserved as
historical artifacts; only the audit prompt's Markdown hard-break whitespace
was normalized as described above.

**PASS — BOUNDED SAFETY SCAN:** All 14 current untracked AI-Logs files were
scanned for obvious password, API-key, authentication-token, private-key,
connection-string-secret, and common credential-token patterns. No matches
were found. Suspected values are never reproduced. Normal SHAs, seeds, paths,
ROM addresses, localhost debugging-port metadata, and CLI arguments are not
automatically secrets.

## 28. Staged-file audit

**PASS.** Exactly 20 files are staged: the two expected source correction
files; `Level15FuseballPolicyTests.cs`; the three durable documentation
files; and the seven current prompt/report pairs. The staged diff is 8,792
insertions and 7 deletions. The full source/test/documentation diff matches
the intended correction and requirements. `AI-Logs/Copilot.txt` remains
present and tracked unchanged. The unrelated untracked
`Reference/arcade/MBUCOD.V05.txt` and all protected local assets are
unstaged and excluded. `git diff --cached --check` and pre-staging
`git diff --check` both pass.

## 29. Excluded local/reference assets

Excluded: both local gameplay recordings, unrelated `TEMP/` content,
screenshots, videos, MAME captures, ROM binaries, reference files including
the untracked arcade text file, generated build output, user/editor state,
DebugMCP extension files, VS Code user MCP files, and user skill directories.
No excluded file was staged or modified.

## 30. Primary commit

**PASS — PRIMARY CLOSEOUT COMMIT CREATED.**

- SHA: `c4480a06138d94a52189d598a1b40041b0fa7bf0`
- Subject: `Close accepted Level 15 Fuseball policy correction`
- Stat: 20 files changed, 8,792 insertions, 7 deletions.
- AI-Logs: seven prompts and seven reports committed. `AI-Logs/Copilot.txt`
  remains tracked and unchanged.

Committed files:

- `.github/copilot-instructions.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/TempestGame.cs`
- `tests/Tempest.Game.Tests/Level15FuseballPolicyTests.cs`
- `AI-Logs/Prompts/2026-11-25-debugmcp-controlled-installation-proof-prompt.md`
- `AI-Logs/Prompts/2026-11-25-debugmcp-post-restart-continuation-prompt.md`
- `AI-Logs/Prompts/2026-11-25-debugmcp-prerequisite-resolution-prompt.md`
- `AI-Logs/Prompts/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-prompt.md`
- `AI-Logs/Prompts/2026-11-25-level15-fuseball-policy-final-closeout-prompt.md`
- `AI-Logs/Prompts/2026-11-25-level15-fuseball-shared-policy-correction-prompt.md`
- `AI-Logs/Prompts/2026-11-25-vscode-copilot-development-tool-assessment-prompt.md`
- `AI-Logs/Reports/2026-11-25-debugmcp-controlled-installation-proof-report.md`
- `AI-Logs/Reports/2026-11-25-debugmcp-post-restart-continuation-report.md`
- `AI-Logs/Reports/2026-11-25-debugmcp-prerequisite-resolution-report.md`
- `AI-Logs/Reports/2026-11-25-level11-15-17-behavior-special-case-architecture-audit-report.md`
- `AI-Logs/Reports/2026-11-25-level15-fuseball-policy-final-closeout-report.md`
- `AI-Logs/Reports/2026-11-25-level15-fuseball-shared-policy-correction-report.md`
- `AI-Logs/Reports/2026-11-25-vscode-copilot-development-tool-assessment-report.md`

The only remaining unrelated untracked path is the protected
`Reference/arcade/MBUCOD.V05.txt`; it remains untouched and excluded.

## 31. Push result

**PASS.** `git push origin master` pushed the primary commit normally; no
force push was used. Local HEAD is
`c4480a06138d94a52189d598a1b40041b0fa7bf0`; upstream is `origin/master`;
post-push ahead/behind is `0/0`.

## 32. Independent GitHub verification

**PASS — VERIFIED THROUGH THE GITHUB CONNECTOR.**

- Commit lookup by primary SHA and independent `master` branch lookup both
  returned `c4480a06138d94a52189d598a1b40041b0fa7bf0` with subject
  `Close accepted Level 15 Fuseball policy correction`.
- The commit reports all 20 expected paths and the 8,792/7 change totals.
- GitHub content retrieval succeeded for both corrected source files, the
  new Level 15 policy test, both durable behavior/development documents,
  `.github/copilot-instructions.md`, the architecture-audit report, the
  Level 15 correction report, the development-tool assessment report,
  `AI-Logs/Copilot.txt`, and this closeout report.
- The retrieved GitHub blob IDs matched the pushed content for each file.
  No retrieval failure or invalid data was encountered.

## 33. Report-finalization commit

One report-only finalization commit is used to carry the primary SHA, push
result, independent GitHub verification, and final repository state. Its
subject is `Record Level 15 Fuseball closeout verification`. It is pushed and
independently verified after this report update. Its own SHA is stated in
the final chat closeout rather than added in a prohibited third commit.

## 34. Final Git/worktree state

After the primary push, branch `master` matched `origin/master` at
`c4480a06138d94a52189d598a1b40041b0fa7bf0`, with ahead/behind `0/0` and no
staged or tracked changes. The only remaining untracked item was the
protected `Reference/arcade/MBUCOD.V05.txt`. The report-only finalization
commit is pushed to the same upstream; its resulting SHA and sync state are
recorded in the final chat closeout.

## 35. Remaining untracked/unrelated material

The protected `Reference/arcade/MBUCOD.V05.txt` is to remain untracked and
untouched. Any other remaining untracked or unrelated material will be
reported after the selective commit; no user/reference assets will be
cleaned, moved, or staged.

## 36. DebugMCP and Level 18+ boundary

DebugMCP was not installed or revisited; the existing continuity records are
only retained as requested. Level 18+ was not implemented or modified.

## 37. Recommended next milestone

Proceed only with a separately scoped and authorized next-level milestone.
Reuse shared eligibility, semantic lifecycle policy, actor lifecycle, and
rim handoff. Keep the deferred terminal Fuseball traversal/hurry-up question
out of scope unless Neal explicitly reopens it with appropriate evidence.

## 38. Completion status

Behavioral acceptance, fresh automated validation, selective staging,
primary commit/push, and independent GitHub verification are complete. The
single report-only finalization commit carries these post-push facts; no
third commit is used.
