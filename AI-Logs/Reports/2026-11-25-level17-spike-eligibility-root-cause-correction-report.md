# Level 17 Spike Eligibility Root-Cause and Bounded Correction

Date: 2026-11-25

Status: **PASS - root cause established and bounded correction validated; Neal manual gameplay acceptance remains pending**

## 1. Executive summary

Level 17 received unauthorized starting Spikes because C# treated raw parameter `$A0` as proof of feature eligibility and copied it into `StartingSpikeHeight`. The ROM has separate Spike-height parameter data and enemy/feature eligibility. `wave_spikeht` indexes through a 16-position payload; its first positions are `$00, $00, $00, $E0`, so source Levels 17-19 resolve `$00` and no starting Spikes exist. Source Level 20 is position 4 and resolves `$E0`.

The correction adds a generic ROM-cycle Spike-height resolver, returns `null` for a zero payload, and uses it for Level 17. Level 17 Spiker maximum is corrected to zero for the source 17-19 eligibility range.

## 2. Starting Git/worktree state

- Branch: `master`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Staged state: no staged changes
- The active worktree contained pre-existing Level 17, presentation, palette, and test changes.
- Protected recordings, screenshots, ROM/reference material, AI-Logs, and unrelated assets were preserved.
- No reset, clean, restore, stash, stage, commit, or push was used.

## 3. Accepted gameplay/presentation checkpoint

The accepted checkpoint remains preserved:

- Levels 1-16 playback is accepted by Neal.
- Level 17 Shape 1 projection, palette, ordinary rail, Tanker, Egg, Flipper-rim, and Pulsar corrections remain unchanged.
- Gameplay freeze is limited only to the Spike eligibility correction.

## 4. Manual Spike evidence

**ARCADE/MAME-OBSERVED:**

- Displayed Level 17 has no Spikes.
- Levels 17, 18, and 19 have no Spikes.
- Spikes resume at displayed Level 20.

## 5. Current Level 17 Spike initialization path

`Level.Reset` clears the Spike list, then creates one Spike in every playable lane whenever `Configuration.StartingSpikeHeight` is non-null. Level 17 previously supplied `Level17StartingSpikeHeight`, so startup created 16 Spikes.

## 6. Source of `$A0` and attributed parameter

The prior Level 17 implementation defined `Level17WaveSelection.StartingSpikeHeightRaw = 0xA0` and translated it as `(0xF0 - 0xA0) * Level.Depth / 224`. The `$A0` payload is in the later `wave_spikeht` payload sequence, not at the first Level 17 cycle position. It was an incorrect parameter-to-eligibility inference.

## 7. Prior evidence-to-implementation causal chain

Earlier evidence recovery established the ROM table and parameter bytes but the bounded integration promoted the attributed `$A0` value into `StartingSpikeHeight`. `Level17Production` then used that value, and `Level.Reset` interpreted it as eligibility. The implementation therefore conflated parameter presence with feature presence.

## 8. Spike versus Spiker distinction

- A **Spike** is an obstacle left in a lane.
- A **Spiker** is the enemy actor that can climb, leave, or build a Spike.
- Zero starting-height data disables initial Spikes.
- `min_spikers`/`max_spikers` separately control whether Spikers may exist or be admitted.
- A Spiker may not imply a permanent starting-Spike parameter.

## 9. ROM Spike-enable control flow

`set_enm_and_spikes` copies `wave_spikeht` into all lane `spike_ht` entries at level startup. The parameter table loader separately obtains `wave_spikeht` by effective level modulo 16. Therefore `$00` is an absent initial Spike parameter. The ROM's separate `min_spikers` and `max_spikers` tables determine Spiker eligibility and admission.

## 10. Displayed/internal/cycle-local level mapping

`curlevel + 1` supplies the effective level. The parameter decoder subtracts 1 and applies `& $0F`, then adds 1, producing cycle position 1-16. Displayed Level 17 maps to position 1. Displayed Level 20 maps to position 4.

## 11. Level 17-20 eligibility table

| Displayed level | Cycle position | `wave_spikeht` raw | Initial Spikes |
|---|---:|---:|---|
| 17 | 1 | `$00` | None |
| 18 | 2 | `$00` | None |
| 19 | 3 | `$00` | None |
| 20 | 4 | `$E0` | Eligible |

## 12. Full 16-position Spike eligibility pattern

`[00, 00, 00, E0, D8, D4, D0, C8, C0, B8, B0, A8, A0, A0, A0, A8]`

The pattern repeats over the 16-level cycle. Zero is not an obstacle height; it is the eligibility/parameter-dormant value.

## 13. Difficulty effects

The startup Spike-height payload is shown as a shared cycle/level parameter in the inspected ROM listing. Easy/hard difficulty does not establish a different Level 17-19 starting-Spike payload.

## 14. Whether Level 17 can dynamically create Spikes

Level 17 should not dynamically admit Spikers in source Levels 17-19 because ROM `min_spikers` and `max_spikers` are 0/0. C# now sets `MaximumSpikers = 0`, so the generic selector cannot admit a Level 17 Spiker.

## 15. Exact root cause

The exact root is the conflation of parameter presence with feature eligibility:

1. `$A0` was copied from a later position in the Spike-height payload.
2. `StartingSpikeHeight` was populated.
3. `Level.Reset` interpreted every non-null height as “create starting Spikes.”
4. Level 17 thus initialized 16 Spikes.

## 16. Level 17 feature eligibility audit

- Spikes: absent for Levels 17-19.
- Spikers: excluded for source Levels 17-19.
- Tankers: retained under the ROM 1/1 policy.
- Pulsars: retained.
- Fuseballs: absent.
- Flippers: retained.

## 17. Level 17 enemy-count evidence audit

Level 17 remains at ordinary supply 20 and the existing seven-record physical envelope. Minimum/maximum evidence remains:

- Flippers: 0/3
- Tankers: 1/1
- Spikers: 0/0
- Pulsars: 2/5
- Fuseballs: 0/0

No enemy-count redesign or ledger behavior was changed.

## 18. Other eligibility risks found

No additional material change was made from this Spike audit. Raw parameter fields for unrelated features remain separate from ROM eligibility controls. Presentation/palette/HUD/Flipper/Pulsar code was not changed.

## 19. Production correction

`LevelConfiguration` now provides `StartingSpikeHeightForDisplayedLevel(displayedLevel)` using the ROM-BACKED 16-position payload. A `$00` payload returns `null`; a nonzero payload translates to the existing C# height scale.

`Level17Production.StartingSpikeHeight` now resolves generically from displayed Level 17. `Level17WaveSelection.MaximumSpikers` is zero.

## 20. Why correction is generic rather than a Level 17 hack

The correction is keyed by cycle position and `null` eligibility, not `displayedLevel == 17`. It can establish 17-20 and repeat for the remaining 16-position cycle without a Level 17 special case.

## 21. Spike-height parameter handling

The wrong Level 17 `$A0` field was removed. `$A0` remains valid for later cycle positions and is not promoted to Level 17 eligibility. The existing `(0xF0 - raw) * Level.Depth / 224` project translation remains unchanged for eligible levels.

## 22. Tests corrected

`StartLevelTests` now asserts no Spikes when entering Level 17 and retains 16 Spikes for Level 16.

## 23. New eligibility tests

`Level17ProductionTests.SpikeEligibilityUsesWaveSpikeHeightCyclePositions` asserts:

- the complete 16-position raw payload;
- Levels 17-19 return `null`;
- Level 20 returns the translated `$E0` height;
- Level 17 config has no starting height;
- Level 17 minimum/maximum Spikers are 0/0.

## 24. Configuration validation seam

`RomSpikeHeightRawByCyclePosition` and `StartingSpikeHeightForDisplayedLevel` form the bounded configuration seam. `$00` is consistently converted to absent eligibility before C# parameter creation.

## 25. Level 17 recording classification/result

**PASS**: headless replay consumed 2609/2609 ticks and completed `RecordingCompleted`; final state was Level 17 Complete, score 5975, lives 8, 0 Spikes.

This is a gameplay-correction replay pass, not manual visual or arcade-fidelity acceptance.

## 26. Levels 1-16 recording result

**PASS**: headless replay consumed 35350/35350 ticks, completed `RecordingCompleted`, ended at the accepted campaign boundary, score 134039, lives 9, no remaining frames.

## 27. Focused test result

**PASS**: 39 focused Level 17/startup/campaign-soak tests passed.

## 28. Full-suite result

**PASS**: 2065 passed, 0 failed, 0 skipped.

## 29. Release-build result

**PASS**: 0 warnings, 0 errors.

## 30. `git diff --check`

**PASS**: no whitespace errors. Git emitted only the existing LF-to-CRLF line-ending warning for `LevelLoader.cs`.

## 31. Presentation/palette freeze confirmation

No presentation, projection, calibration, palette, HUD, Flipper, Pulsar, Tanker, or rail code was changed for this milestone.

## 32. Manual-validation instructions

Neal should launch a direct Level 17 session only for gameplay acceptance:

```powershell
dotnet run --project src\Tempest.Game -- --start-level 17 --lives 10
```

Confirm Level 17 starts with no Spikes, no Spikers enter naturally, and gameplay otherwise matches the accepted checkpoint.

## 33. Proposed durable eligibility wording

`StartingSpikeHeight` is a parameter only when the ROM cycle payload is nonzero. A zero `wave_spikeht` value means that starting Spikes are not eligible and must not be instantiated.

## 34. Proposed shared-core-behavior wording

Shared Spiker/Spike core behavior remains unchanged across levels. Levels configure eligibility and supply separately; they do not receive level-specific enemy classes or lifecycle engines.

## 35. Proposed cycle-palette wording

Cycle palette remains separate from Spike eligibility. Palette index 1 for Levels 17-32 does not imply that cycle-level gameplay parameters are active.

## 36. Files changed

- `src/Tempest.Game/Levels/LevelConfiguration.cs`
- `src/Tempest.Game/Levels/Level17WaveSelection.cs`
- `tests/Tempest.Game.Tests/Level17ProductionTests.cs`
- `tests/Tempest.Game.Tests/StartLevelTests.cs`
- `AI-Logs/Prompts/2026-11-25-level17-spike-eligibility-root-cause-correction-prompt.md`
- `AI-Logs/Reports/2026-11-25-level17-spike-eligibility-root-cause-correction-report.md`

## 37. Final Git/worktree state

- Branch: `master`
- Tracking: `origin/master`
- HEAD unchanged from the starting checkpoint
- Multiple pre-existing tracked/untracked Level 17 candidate changes remain preserved
- The index remains empty

## 38. Stage/commit/push

No staging, commit, or push was performed.

## 39. Level 18+ implementation

No Level 18+ gameplay implementation was added.

## 40. Recommended next step

Neal should manually accept or reject the Level 17 no-Spike startup and normal no-Spiker behavior. No further code or gameplay changes are recommended until that acceptance or a relevant contradiction is reported.
