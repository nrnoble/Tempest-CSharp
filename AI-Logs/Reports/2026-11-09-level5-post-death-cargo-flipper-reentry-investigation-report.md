# Level 5 post-death cargo Flipper reentry investigation

Date: 2026-11-09

Disposition: **CAUSE CONFIRMED BY SOURCE TRACE. INVESTIGATION ONLY; NO FIX
AUTHORIZED OR IMPLEMENTED.** The current candidate is not manually accepted.

## Starting state and scope

- Repository: `C:\Users\nrnob\source\repos\Tempest`
- HEAD: `62c66e0ab19436ea162935627021154be21e0a41` (the requested accepted
  baseline); the candidate is uncommitted on top of it.
- The candidate's existing files were the modified
  `Level3WaveLedger.cs`, `TempestGame.cs`, `Level5ProductionTests.cs`, and new
  `Level5AdmissionPolicy.cs` and `Level5AdmissionPolicyTests.cs`.
- The complete substantive task was preserved in the ignored
  [investigation prompt](../Prompts/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-prompt.md).
- Read the existing
  [Level 5 ROM investigation](2026-11-09-level5-concurrency-rom-investigation-report.md),
  [admission/terminal-Spiker implementation report](2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md),
  and
  [debug-route validation report](2026-11-09-level5-debug-route-automated-validation-report.md).
- No production code, tests, accepted documentation, or reference files were
  changed. No game was launched; no build, test, or VSTest execution was run.
  The required `git diff --check` was run and passed.

## Manual observation and conclusion

Neal reports that an uninterrupted Level 5 wave presents Flippers only as
Tanker cargo; none independently enter from the deep end. After Tanker cargo
has been released and the Blaster dies with unresolved cargo, the individual
Flippers reappear from the deep end. Neal confirms that this never happens on
the original arcade Level 5, including after Blaster death/reentry. This
candidate is therefore **not manually accepted**.

**Confirmed source cause:** death reconstruction retains each unresolved,
already-materialized Tanker cargo Flipper as the same cargo identity and
species, but changes its ledger location to `ReentryReserve`. Normal reserve
release wraps it in a fresh `Egg`; normal Egg approach/hatching then creates a
new cargo Flipper at height 0 using `CreateFreshTankerCargo`. It is not
recreated through its carrier, and it does not use the Tanker-release pivot or
the position where it previously lived. The policy's cargo branch bypasses
ordinary species selection, but still admits that Egg through the same
deep-entry presentation route. Stable identity conservation is a project
accounting choice; it does not require this arcade-inconsistent visible return.

For normal canonical Level 5 production, a newly observed individual Flipper
following this path is cargo, not an original identity mistakenly selected as
Flipper. Exact owner/ordinal IDs for Neal's manually observed specimens cannot
be recovered from an uninstrumented visual observation; the source path and
the existing integration test establish the cargo attribution for the
reproduced case.

## Identity and lifecycle trace

The following representative uses original identity `4` and its cargo pair:
`Cargo(8)` = owner 4 / ordinal 0, and `Cargo(9)` = owner 4 / ordinal 1. The
numeric identity follows `CargoUnitId(ownerOriginalId, ordinal)`. These numbers
illustrate the source-defined relationship; Neal's manual observation was
not instrumented to identify particular numbers.

| Step | Identity / owner / species / ledger state | Runtime representation and path | Selection / presentation |
|---|---|---|---|
| 1. Original admission | `Original(4)`, initially Unselected/Egg; then Tanker, ActiveTanker | An original `Egg` is hatched by `CreateLevel3MixedEnemy`; `TrySelectLevel5DiagnosticType` commits it as Tanker. | Fresh ordinary original uses the Level 5 policy. The candidate selects only Tanker/Spiker; species choice is policy-driven (minima/Spike threshold), not a random Flipper roll. |
| 2. Carrier owns pair | `Cargo(8)` and `Cargo(9)`, owner Original(4), ordinals 0/1, Flipper species, Latent | The ledger allocates a complete pair when the original commits as Tanker. No child Flipper runtime objects exist yet; the Tanker retains two latent cargo units. | No independent Flipper selection or entry. |
| 3. Cargo release | Both identities become Active, or briefly PendingShotActivation for a player-shot release. The carrier becomes Escaped on natural release or Destroyed on shot/Zapper destruction. | `ReleaseTankerCargo` materializes the complete pair. Natural/Zapper children enter `flippers`; shot children enter `shotReleasedCargoPendingActivation` and are activated by `ActivateShotReleasedCargo`. | No species selector. Normal no-death presentation uses `CreateTankerCargo`: children begin at the carrier's source lane/height in the mandatory release pivot, rather than at the deep-entry height. |
| 4. One remains unresolved | E.g. Cargo(8) is Active and unresolved; Cargo(9) may be active or terminal. Owner remains the original Tanker identity, often already terminal. | The unresolved child's runtime Flipper is in `flippers`; a shot child may transiently be in the pending-activation list. | It is already committed cargo; ordinary Level 5 admission is not involved. |
| 5. Blaster death | The cargo identity is still unresolved; no new species choice occurs. | `ResetAfterDeath` clears runtime enemy collections, including active and pending cargo Flippers, then calls `Level.ResetAfterDeath`. | Death/reentry order is shared existing production-ledger behavior, not introduced by the new Level 5 selector. |
| 6. Ledger reconstruction | Every unresolved materialized cargo ID becomes ReentryReserve. Terminal cargo remains Destroyed/RimCleared. Latent cargo remains Latent. Original identities are independently retained/reset according to their own state and selected type. | `Level3WaveLedger.RebuildAfterDeath` preserves cargo owner, ordinal, and identity; it does not turn a cargo identity into an ordinary original. | No random species choice. The cargo still means Flipper. |
| 7. First reentry staging | Cargo(8): ReentryReserve -> Egg. | `Level.ResetAfterDeath` releases initial Eggs; `ReleaseLevel3MixedEgg` prefers cargo reentry reserve before ordinary original reserve, calls `MarkCargoEgg`, and creates an `Egg` carrying `CargoUnitId=8`. | This is the defect's shared Egg/deep-entry presentation. It is not the ordinary original species selector. Lane and direction use the existing random source. |
| 8. Player returns / Egg advances | Cargo(8) remains ledger Egg until hatch. | The Egg follows the ordinary roaming/approach/ready lifecycle through `UpdateLevel3MixedEggs`; its rendered point follows `DrawEggs`/`ProjectEgg`. | Same visible Egg approach as original admission. The current policy does not supply a cargo-specific visual route. |
| 9. Cargo hatches | Cargo(8): Egg -> Active; owner/ordinal/species are unchanged. | `HatchReadyMixedEnemies` calls `CreateLevel3MixedEnemy`; the cargo branch marks the same ID Active and creates `CreateFreshTankerCargo`, then adds it to `flippers`. | `TrySelectLevel5DiagnosticType` recognizes `CargoUnitId >= 0` and uses only `CanAdmitCargoReentry` (physical headroom). It bypasses the ordinary Tanker/Spiker selector; only an initial direction tie may consume randomness. |
| 10. Visible return | Cargo(8) remains the same Flipper identity. | `CreateFreshTankerCargo` constructs it at height 0 in Climbing state. This is not the Tanker-height release pivot (`CreateTankerCargo`), and the previous cargo position was discarded during death. | It appears as an individual deep-end entrant, which fails Neal's arcade criterion even though species and identity are conserved. |

`Level3WaveLedger.RebuildAfterDeath` snapshots unresolved IDs, leaves latent
cargo latent, and sets every other unresolved materialized cargo entry to
`ReentryReserve`. `Level.ReturnSurvivorsToReserve` then releases the configured
initial Egg count. In `ReleaseLevel3MixedEgg`, `CargoIdsInReentryReserve` is
checked before `OriginalIdsInReserve`, so cargo reserve has priority for those
Egg slots. Remaining cargo reserve is also preferred on later reserve release.
No independent Egg is produced for latent cargo: an unresolved Tanker original
is reconstructed as a retained Tanker with its latent pair, then its cargo
remains attached until release.

## Ordinary Flippers, cargo admission, and variants

- **Ordinary originals:** On the canonical configuration,
  `AllowsOrdinaryFlipperSelection` is false. The candidate's
  `GetOrdinaryEligibleTypes` contains only Tanker and Spiker. A fresh
  unselected original therefore cannot become an ordinary Flipper. The
  candidate does support retained original types separately, but normal
  canonical Level 5 admission never commits a fresh original as Flipper.
  Thus this path does not mistake a correctly reconstructed original for
  cargo or vice versa.
- **Cargo reentry:** `TrySelectLevel5DiagnosticType` branches on the Egg's
  cargo ID before checking any retained original or selecting an ordinary
  type. It passes the cargo through `CanAdmitCargoReentry`, which tests
  physical headroom only. This candidate behavior preserves cargo species and
  identity but leaves the shared Egg path intact.
- **Natural Tanker release:** Both cargo children become Active and the
  carrier becomes Escaped. Any child still unresolved at death becomes
  ReentryReserve/Egg. A resolved child does not return.
- **Player-shot Tanker destruction:** Cargo is first PendingShotActivation
  and then Active in ordinary tick ordering; `RebuildAfterDeath` maps any
  still-unresolved materialized child to ReentryReserve. A reset at the
  pending state is also covered by ledger semantics/tests and has the same
  reentry destination.
- **Super Zapper Tanker destruction:** Both children are materialized Active
  with zap timers. If a child resolves before death, it does not return. If it
  remains unresolved at death, the same ReentryReserve/Egg path applies.
- **One/both children active:** Reconstruction considers each unresolved
  cargo identity, not only children currently in a particular runtime list.
  Resolved children are excluded; every unresolved materialized child returns
  individually. It does not require both siblings to remain unresolved.
- **No release yet:** Both child identities remain Latent and the owner
  Tanker's original identity is reconstructed. No individual cargo Egg is
  made, so the observed defect cannot occur before the first cargo release
  through this path.
- **No unresolved cargo at death:** No cargo reentry Egg is made. This defect
  requires at least one unresolved materialized cargo identity.
- **Converted Tanker:** The candidate newly retains an `IsConvertedTanker`
  marker across Level 5 death and uses it to let the converted original
  reenter subject to physical headroom rather than the ordinary one-Tanker
  maximum. Its latent pair, if unreleased, stays latent. Once materialized,
  its child identities follow the same cargo reconstruction route as an
  ordinary Tanker; the marker does not change cargo Egg creation, identity,
  or appearance. Converted-Tanker creation is candidate-specific, but the
  cargo presentation mechanism is not.

## Candidate comparison and accepted baseline

The comparison against the requested baseline and candidate diff establishes:

| Path | Candidate effect |
|---|---|
| Cargo identity reconstruction and `ReentryReserve` transition | Unchanged shared `Level3WaveLedger` behavior. |
| Cargo reserve-to-Egg release and identity on Egg | Unchanged shared `Level` behavior. |
| Cargo retained species and factory | Unchanged; it remains a Flipper and hatches via `CreateFreshTankerCargo`. |
| Cargo ready-Egg eligibility | New Level 5 gate. Cargo now waits for physical headroom only; it still uses the shared Egg route. |
| Fresh ordinary Level 5 species admission | New Level 5 policy; fresh ordinary Flippers are excluded. This is correct for Neal's criterion and should remain unchanged. |
| Converted-Tanker marker and its headroom-only original reentry | New candidate behavior; it preserves the converted carrier identity/type. |
| Death clearing/reentry order and visual Egg approach | Unchanged. No candidate change restores cargo position or routes it around the Egg. |

The defect **predates the current admission-and-terminal-Spiker candidate**:
the accepted-baseline versions already used `RebuildAfterDeath` to place
unresolved materialized cargo in reentry reserve, and already released
identity-bearing cargo Eggs that hatched through `CreateFreshTankerCargo`.
The existing Level 5 integration test for reconstructed Tanker cargo also
predates this candidate. The candidate adds the Level 5 headroom gate and
converted-Tanker support, not the cargo-Egg presentation. The gate can delay
the hatch at full occupancy; it does not create the presentation defect.
Levels 3/4 ledger and Egg infrastructure supplied the shared lifecycle.

## ROM comparison

The existing ROM investigation reports that Level 5's ordinary Flipper
minimum is zero, Tanker cargo bypasses ordinary species selection, and ROM
death/reconstruction resets active enemies into generic pending supply.
It explicitly distinguishes that ROM behavior from this project's accepted
stable identity/cargo model. The ROM facts support keeping ordinary Flippers
excluded, but do not require preserving an individual released cargo
Flipper's identity as a new visible bottom entrant after death.

The C# ledger's stable cargo identity is useful for accounting, owner/ordinal
conservation, and completion. The visible Egg/deep-entry return is a separate
presentation/lifecycle decision layered on that identity. Neal's arcade
observation rejects the latter, not the former. Replacing the whole stable
identity model or relaxing ordinary admission is not supported by this
investigation.

## Test coverage and exact gap

Covered:

- Ledger tests verify owner/ordinal, unresolved identity conservation, latent
  cargo preservation, terminal cargo preservation, and reentry-reserve/Egg
  representations.
- Level 5 tests cover no-input death stability, Tanker/cargo reconstruction,
  retained species, converted-Tanker marker retention, and policy headroom.
- `ReconstructedLevelFiveTankerCargoHasDirectionBeforeSpikeGatedPivot`
  creates Tanker cargo, causes death with cargo unresolved, explicitly finds
  the same cargo ID in `game.Eggs`, makes it ready, and verifies that the same
  ID hatches as a Flipper. It then tests direction/pivot behavior.
- `DeathResetReturnsBothPendingShotCargoIdentitiesAsEggs` verifies cargo
  Egg reconstruction at the shared ledger level.
- The handoff reports record 28 focused, 345 affected, and 1,165 full Debug
  tests passing for the candidate. This investigation did not rerun them.

**Exact gap:** No test stated Neal's arcade-visible invariant that released
Level 5 cargo must not reappear after death as an individual deep-end entrant.
The existing Level 5 integration test actually expects the cargo identity in
an Egg and hatches it, but asserts only identity, type, direction, and pivot
behavior—not whether that Egg/deep-entry presentation is acceptable. Ledger
identity-conservation tests likewise do not assert presentation. Thus the
automated suite passed while the manually visible route violated the arcade
criterion; this is not a gap in identity conservation coverage.

## Smallest deterministic future reproduction

Use the canonical `LevelConfiguration.Level5Production` and a deterministic
random source. Drive one known original ID (for example 4) through the normal
Level 5 ordinary policy as a Tanker, verify its cargo IDs/ordinals, release
the pair, resolve one child, and leave the other child active/unresolved.
Trigger Blaster death, complete reconstruction/reentry, and continue until
the cargo would otherwise return.

The test must inspect the cargo ID's **presentation/runtime representation**,
not only its species:

- verify the same unresolved cargo identity and owner/ordinal are conserved;
- verify it is not staged in a cargo `Egg` and does not create a new Flipper
  at the deep-entry height through `CreateFreshTankerCargo`;
- verify it returns only through the approved arcade-faithful in-tube route.

Keep a contrasting no-release-at-death case: the Tanker original may reenter
with its latent pair, but no child cargo ID may be an individual Egg. Add
focused variants for natural release, player-shot release, a Zapper child
still unresolved at death, one resolved sibling, and a converted Tanker once
the primary case is fixed. Do not use enemy type alone as the assertion.

## Recommended next milestone

**Cause:** already-materialized, unresolved Tanker cargo Flippers are
reconstructed as cargo reserve, preferentially wrapped in Eggs, and hatched
as fresh height-zero Flippers after death. The Level 5 policy preserves the
species/identity and checks only physical headroom; it does not own or fix
their visual reentry route.

**Smallest safe production change to evaluate:** keep the existing cargo
identity and unresolved/terminal accounting, but give *materialized unresolved
Level 5 cargo* a cargo-specific death restoration path that returns it as an
active in-tube Flipper rather than a reserve Egg. Since the current lifecycle
discards its runtime position before rebuilding, a narrow implementation
would capture the unresolved cargo's lane/height before clearing collections
and restore it there (subject to physical capacity). Do not reconstruct a
terminal/escaped carrier just to display the child. Neal must review/authorize
the exact post-death placement before implementation; source alone does not
establish further arcade placement details beyond “not an individual bottom
entry.”

Leave ordinary Level 5 Tanker/Spiker admission, minima/maxima, Flipper
exclusion, terminal Spiker conversion, converted-Tanker reentry, complete
cargo-pair identity conservation, scoring, Zapper lifecycle, persistent
Spikes, rim/completion behavior, recording determinism, and Levels 1-4 and
6-8 untouched. Focused tests should cover the reproduction above, capacity
waiting, and latent-carrier/no-release control. After the fix, Neal must
manually retest the death/reentry presentation against the arcade; this
investigation did not launch the game.

## Closeout

- No implementation, build, test, VSTest, or game launch occurred.
- Only the ignored prompt and this ignored report were written.
- HEAD remains at the accepted baseline; no files are staged.
- The existing candidate diff remains intact; `git diff --check` passes.
- No commit or push was made.
