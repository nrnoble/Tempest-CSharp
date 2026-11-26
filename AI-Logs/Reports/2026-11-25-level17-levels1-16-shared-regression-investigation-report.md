# Level 17 Levels 1-16 Shared-Regression Investigation

Date: 2026-11-25

Status: **PARTIAL — shared campaign/Level 15 simulation-state drift confirmed, smallest implicated hunk(s) narrow but not yet isolated without a production edit**

## 1. Executive summary

The current uncommitted Level 17 candidate regressed the accepted Levels 1-16
foundation. Two repeated candidate campaign runs ended in Level 13
`GameOver` instead of Level 16 `Complete`, with zero lives. An exact seed-1982
Level 15 soak failed twice and observed only `Spiker`, `Tanker`, and `Fuseball`;
the ordinary `Flipper` was absent. The accepted baseline passed the same
campaign and soak oracle.

The evidence establishes a real simulation-state/random-stream regression,
not a stale test or rendering-only symptom. Temporary seed-1982 probes found
462 random calls and no selected Flipper in the candidate versus 980 random
calls and `Spiker, Tanker, Fuseball, Flipper` in baseline. The first objective
campaign mismatch was observed inside Level 11 at boundary 19793:
candidate 75525 score versus baseline 75675, then the state diverged further
through Level 11/12 and ended at Level 13. This supports a shared drift in
generic/Level 11-15 runtime behavior, although the exact causal hunk could not
be isolated safely without temporarily changing production code.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Upstream: `origin/master`
- Ahead/behind: `0/0` (unchanged baseline status)
- Staged state: no staged changes
- Pre-existing tracked modification: `TEMPEST-BEHAVIOR-SPEC.md`
- Candidate tracked modifications and additions are listed by the implementation report
- Pre-existing untracked state included existing Level 17 prompts/reports,
  `Reference/arcade/MBUCOD.V05.txt`, and local assets
- These states were preserved; no reset, checkout, restore, stash, clean, or
  staging occurred

## 3. Accepted baseline and candidate identities

- Accepted baseline: `fde09aa31af760958f656aa553555462d51792a0`
- Candidate: current uncommitted worktree at the same HEAD plus Level 17 changes
- The baseline/candidate campaign identity and random seed were identical:
  seed `1990198549`
- Recording: `TEMP/GameplayRecordings/lvl1-16.json`
- The baseline was run from a detached temporary worktree
- The candidate was rebuilt from the active worktree

## 4. Manual campaign-regression evidence

This is recorded as **MANUAL REGRESSION EVIDENCE**. Neal replayed the campaign
and observed failure beginning around Level 11, stopping/missing Rim Flippers,
continued playback out of synchronization, eventual loss of lives, and failure
to complete Levels 1-16. This was treated as a real simulation/presentation
regression, not a cosmetic concern.

## 5. Strict recording pass/fail oracle

PASS requires:

- recording parses
- playback consumes the recording
- remaining frames `0`
- completion reason `RecordingCompleted`
- ending displayed level `16`
- ending Level 16 state `Complete`
- no earlier Game Over or exhausted-lives state replaces the expected boundary

Candidate process exit `0` was not accepted as PASS because its terminal state
was wrong.

## 6. Current candidate headless playback result

Candidate final telemetry:

- process exit: `0`
- completion reason: `RecordingCompleted`
- remaining frames: `0`
- ending displayed level: `13`
- level state: `GameOver`
- lives: `0`
- score: `96,636`
- random seed: `1,990,198,549`
- total ticks: `35,350`
- active entities at end: no Flippers, Tankers, Spikers, or Fuseballs
- `activeSpikeCount`: `13`

Thus the campaign is an unambiguous strict FAIL.

## 7. Deterministic repeat result

Three candidate headless runs/representations were run. Two JSON runs and a
third direct process run produced identical final candidate state:
Level 13, `GameOver`, 0 lives, score 96636, seed 1990198549. The candidate
regression is deterministic.

## 8. Baseline headless playback result

Baseline final telemetry:

- process exit: `0`
- completion reason: `RecordingCompleted`
- remaining frames: `0`
- ending displayed level: `16`
- level state: `Complete`
- lives: `9`
- score: `134,039`
- random seed: `1,990,198,549`
- total ticks: `35,350`

The baseline satisfies every strict campaign boundary.

## 9. Baseline/candidate final telemetry comparison

| Result | Baseline | Candidate |
|---|---:|---:|
| Level | 16 | 13 |
| State | Complete | GameOver |
| Lives | 9 | 0 |
| Score | 134039 | 96636 |
| Remaining frames | 0 | 0 |
| Completion reason | RecordingCompleted | RecordingCompleted |

## 10. Last matching and first differing tick/boundary

- Matching objective summary through boundary `19792`:
  Level 11, Playing, 75525 score, 3 Flippers, 0 Tankers, 3 Spikers, 8 Eggs
- First sampled objective difference: boundary `19793`
- Candidate at 19793: score `75525`, 3 Flippers, 0 Tankers, 3 Spikers
- Baseline at 19793: score `75675`, 2 Flippers, 0 Tankers, 3 Spikers
- The narrow confirmed difference interval is boundaries `19793-19799`
- The first differences include a 150-score jump timing shift and later
  candidate/baseline Flipper/Tanker/egg identity differences

## 11. Level at first divergence

The first confirmed objective divergence is inside Level 11, with Level 11
already in `Playing` state.

## 12. Random-state comparison

Temporary seed-1982 probes used candidate and baseline assemblies without
production or test modification:

- Candidate: `462` recorded `Random.Next*` calls; selected species
  `Spiker, Tanker, Fuseball`
- Baseline: `980` recorded `Random.Next*` calls; selected species
  `Spiker, Tanker, Fuseball, Flipper`
- The first 300 call envelopes were not enough to establish an extra draw in
  untouched pre-Level-15 code; instead, candidate stream divergence corresponded
  to a different active enemy/Tanker-cargo lifecycle and fewer subsequent draws
- The campaign random seed remained identical, but gameplay state was not

## 13. Level-transition comparison

Coarse boundary samples:

- 5000: both Level 5, 20 lives, 12143
- 10000: both Level 6, 19 lives, 30089
- 15000: both Level 9, 18 lives, 51958
- 20000: both Level 11, lives 17; candidate score 76091, baseline 76716
- 25000: candidate Level 12 and 13 lives; baseline Level 13 PlayerDying
- 30000: candidate Level 13 Playing; baseline Level 14 Playing
- 35350: candidate Level 13 GameOver; baseline Level 16 Complete

## 14. First affected Flipper identity/state

No complete Flipper ledger snapshot was instrumented. Existing state summaries
show candidate/baseline active Flipper counts diverging at the Level 11 boundary.
By boundary 19799:

- Candidate: 2 active Flippers, 0 Tankers
- Baseline: 1 active Flipper, 1 Tanker

The narrow evidence does not establish stable identity, cargo ownership,
lane, movement state, or projected rendering for a first Flipper. It does
establish that the early objective difference is state-side, not merely absent
render submission.

## 15. Simulation-versus-rendering determination

**Campaign classification: confirmed simulation regression.**

The baseline/candidate score, lives, active species, and Tanker/Flipper identity
counts differ in headless mode, which disables presentation. A rendering-only
cause is therefore disproved as the cause of the campaign failure. Rendering
may also expose the state failure, but headless state itself diverges.

## 16. Level 11 shared-path analysis

Level 11 uses genuine ROM Level 11 policy with:

- `UsesLevel11RomWavePolicy`
- repeating `$24` ordinary Flipper program
- shared EndpointHinge pivot
- Level 11 Tanker cargo `$07/$24` behavior
- terminal lifecycle and ledger reconstruction

Candidate changes do not explicitly select a Level 17 policy for Level 11,
but candidate `TempestGame`, `Flipper`, `Tanker`, and ledger code are all
common runtime surfaces reached by Level 11.

## 17. Level 15 shared-path analysis

Level 15 uses a separate Level 11 parameter-development baseline:

- `UsesLevel11ParameterDevelopmentBaseline`
- Level 11 repeating `$24` Flipper program
- ordinary Flipper selection disabled
- Flippers are retained through Tanker cargo/latent identity
- 27-original level supply
- 4 initial Eggs
- shared ledger reconstruction

Level 15 seed-1982 regression is a real behavior drift in this direct-start
production path.

## 18. Exact Level 11/15 common mechanisms

Shared surfaces include:

- generic production ledger and original/cargo identities
- Tanker cargo construction and release
- Flipper cargo/ordinary runtime representation
- endpoint-hinge/rim pursuit update
- Egg hatch/production admission
- random hatch/selection
- ledger terminal/destroy/reconstruction checks
- generic state validation

Both levels use the `Level11Repeating24`/development-baseline Flipper route.

## 19. Exact Level 11/15 differences

- Level 11 uses ROM-policy flag `UsesLevel11RomWavePolicy`
- Level 15 uses `UsesLevel11ParameterDevelopmentBaseline`
- Level 11 presentation and geometry are distinct from Level 15
- Level 15 remains direct-only in its accepted campaign status
- Level 15 has Level 15 starting Spike height
- Level 11 uses genuine Level 11 supply/policy constants

## 20. Level 15 seed-1982 isolated result

Candidate exact test: **FAIL**.

Repeated result: selected set `[Spiker, Tanker, Fuseball]`, not ordinary
`Flipper`, at line 106 of the soak invariant.

## 21. Level 15 seed-1982 baseline result

Baseline exact test: **PASS**.

## 22. Level 15 failure classification

**Independent real candidate regression in a related shared Level 11/15
mechanism**, but it is currently established as a real root/symptom in the
same Level 11/15 runtime family. The evidence does not yet prove one-line
identity between campaign and soak; the same `Flipper/Tanker/cargo/ledger`
surface is implicated on both, so classify relationship as
**related shared drift but different symptom** until a bounded hunk variant
proves one root.

It is not stale or over-specific: baseline passes and candidate reproducibly
fails.

## 23. Candidate changed-file classification

- `LevelConfiguration.cs`: can affect gameplay gates and now adds Level 17 enum/config
- `LevelProgression.cs`: affects Level 17 progression and terminal boundary only
- `RomPresentationMetadata.cs`: presentation/shape metadata; uncertain direct runtime side only
- `LevelLoader.cs`: presentation shape reuse; canonical Level 1 object may only affect geometry
- `AtariPresentationCalibration.cs`: presentation only
- `AtariPresentationAdapter.cs`: presentation only
- `VectorRenderer.cs`: presentation only
- `Flipper.cs`: gameplay state, but Straight07 branch is gated to Level 17 route
- `Tanker.cs`: gameplay state/cargo, but Straight07 cargo branch is gated to Level 17
- `Pulsar.cs`: production identity/runtime
- `Level3WaveLedger.cs`: gameplay state, ledger, reconstruction, representation
- `TempestGame.cs`: gameplay state, admission, hatching, random, Tanker, ledger, completion
- Level 17 tests: test-only

## 24. Smallest implicated hunk(s)

The smallest likely attribution is:

- generic candidate changes in `TempestGame.cs` or `Level3WaveLedger.cs`;
- and/or `Flipper.cs`/`Tanker.cs` cargo-route changes

The exact direct random audit found no added `Random.Next*` call in the
candidate for Levels 1-16. The candidate has only Level17-gated branches in
the directly diffed shared classes. Therefore the exact first hunk remains
**narrowly bounded but unresolved**; it must be established with one bounded
temporary A/B hunk exclusion or a temporary random/ledger diagnostic.

## 25. Temporary A/B variants used

- Detached Release baseline worktree at `C:\Users\nrnob\Tempest-baseline-investigation`
- One campaign recording copy in that worktree only
- Temporary seed-1982 C# random-sequence probes outside the repository
- Baseline and candidate Release builds
- No candidate hunk exclusion was used

## 26. Relationship between campaign and soak failures

Both exhibit candidate-only real gameplay-state drift in the same Level 11/15
Flipper/Tanker/ledger family. The campaign first diverges in Level 11 and the
Level 15 soak loses a Flipper/Tanker-cargo branch. A common root is likely but
not yet proven to be a single hunk.

## 27. Relationship to Level 17 visual defects

The legacy-looking tube and missing changed color scheme are independent
presentation-mapping defects in the candidate. Existing host-role palette
translation remains unchanged. The presentation/adapter/calibration changes
do not explain the headless seed or campaign state drift.

Spikes/Pulsars being present are separate Level 17 content observations.

## 28. Recommended campaign regression assertion/tooling

Permanently assert all required strict fields for
`TEMP/GameplayRecordings/lvl1-16.json`:

- parse success
- `CompletionReason == RecordingCompleted`
- `RemainingFrames == 0`
- `EndingDisplayedLevel == 16`
- `LevelState == Complete`
- `RemainingLives > 0`
- no earlier `GameOver`

Useful intermediate assertions:

- level boundary sequence
- no Level 11 or earlier state divergence
- deterministic final score/fingerprint once established stable

The existing CLI already emits the needed final fields.

## 29. Smallest bounded correction recommendation

Before any Level 17 continuation, create a temporary A/B production variant
that first excludes the candidate's generic Ledger/Pulsar additions, one hunk
at a time, and runs:

1. strict campaign oracle
2. seed-1982 Level 15 soak

If the generic ledger exclusion restores both, fix the ledger hunk.
If it does not, isolate `TempestGame.cs` admission/hatch/Pulsar branches next,
then cargo route code only if the campaign/soak remain affected.

Preserve the accepted foundation and intended Level 17 work; do not run a
full-suite correction until the two foundation oracles are green.

## 30. Remaining uncertainties

- Exact first diverging tick was narrow to 19792/19793, not one exact state
  transition
- Exact first Flipper identity and ledger state not captured
- The one-hunk causal root was not proven without a temporary production
  exclusion
- Random call drift may be downstream of a state bug rather than the cause

## 31. Commands/tests run and bounds

- Release build: candidate PASS
- Candidate campaign: 3 runs, bounded 120 seconds
- Baseline campaign: PASS in detached worktree
- Candidate seed-1982 test: FAIL twice, 2-minute VSTest hang timeout
- Baseline seed-1982 test: PASS
- Candidate/baseline boundary probes: bounded per process
- Temporary random probes: 1200 ticks for Level 15 seed 1982

No full suite was run repeatedly.

## 32. Temporary worktree/tool cleanup

- Detached baseline worktree removed
- Git worktree list returned to the active worktree only
- Temporary candidate playback JSON files removed
- Temporary random probes remain only in session-state and are not repository artifacts

## 33. Production code changed

**No.**

## 34. Tests changed

**No.**

## 35. Durable docs changed

**No.**

## 36. Recording changed

**No.**

## 37. Stage/commit/push

**None.**

## 38. Level 17 feature work continued

**No.**

## 39. Level 18+ work

**None.**

## Required classifications

### Campaign playback

**Confirmed simulation regression.**

### Level 15 soak

**Related shared drift but different symptom.**

### Level 17 presentation

**Independent known palette-translation gap.**

## 40. Recommended next step

Authorized follow-up: a bounded read/write correction investigation that uses
temporary A/B hunk exclusion to identify the one shared
`Ledger/TempestGame/Flipper/Tanker` regression, restores the strict campaign
oracle and seed-1982 soak, and preserves Level 17 presentation work until the
foundation is green.
