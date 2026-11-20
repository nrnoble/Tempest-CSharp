# Level 7 Spiker repeated-cycle firing investigation

Date: 2026-11-10

Disposition: **READ-ONLY INVESTIGATION COMPLETE.** The current uncommitted
Level 7 candidate was preserved.

## Result

**B. CONFIRMED DIFFERENT MECHANISM.** Neal's report that arcade/MAME
Spikers remain able to fire on later climbs is **ARCADE/MAME OBSERVED**;
this investigation did not repeat that gameplay observation. The local ROM
listing explains the repeat-cycle capability through its shared enemy firing
routine, not through an ammunition, shot-count, or timer reset at the end of a
Spiker cycle.

The confirmed C# mismatch is that Spiker firing is coupled to a
growth-triggered retreat. As the persistent Spike on a lane approaches the
upper height limit, a later cycle can have less than 100 units of new Spike
growth available. That cycle cannot enter the only C# firing branch, and the
upper-height retreat branch does not fire. This is not an exhausted
per-Spiker ammunition counter: C# has no such counter. The firing restriction
predates the current candidate; the candidate's same-lane persistence can
expose it more consistently.

## Evidence and ROM trace

Evidence labels below distinguish Neal's manual observation from static source
analysis. **ROM-BACKED** refers to the checked-in annotated listing, not a new
ROM execution or an assertion about undocumented hardware behavior.

- **ARCADE/MAME OBSERVED:** Neal reports that a Spiker continues to fire on
  later climbs in arcade/MAME, while the C# Spiker continues moving but can
  become silent after its first cycle.
- **ROM-BACKED -- initialization:** The enemy-type dispatch selects Spiker
  movement entry `$00` and attribute `$40` (`$9b00`, `$9b05`). The generic
  enemy-slot initializer `$994d` initializes that slot's `shot_delay` to zero
  (`$9979`) and copies the selected attributes. The Spiker's `$40` attribute
  enables the shared shooter check. The listing has no per-Spiker ammunition
  or remaining-shot field.
- **ROM-BACKED -- how firing is governed:** Active gameplay calls
  `enm_shoot` once after enemy movement (`state_04`, `$971d`). It considers an
  enemy with nonzero `enemy_along` at or beyond `$30`, and whose `$028a` flags
  include `$40`. Its per-slot `shot_delay` counts down; after expiry the
  routine applies the random threshold table indexed by `enm_shotcnt`, then
  searches the globally configured enemy-shot slots (`enm_shotmax`). On a
  successful shot it copies the enemy's position/segment into a free
  projectile slot, reloads that enemy's delay from global `shot_holdoff`, and
  increments `enm_shotcnt`.
- **ROM-BACKED -- distinct counters:** `shot_delay` is a per-enemy cooldown,
  not ammunition. `enm_shotcnt` is the global number of active enemy shots:
  it affects the random threshold and increments/decrements with projectile
  creation/expiry. `enm_shotmax` bounds the shared projectile slots, not the
  total lifetime shots a Spiker may fire. A projectile ends when it reaches
  the player's end of the tube (`move_shots`, `$a1d5`). Thus the ROM has
  cooldown, probability, simultaneous-capacity, and projectile-lifetime
  mechanisms, but no per-Spiker shot allowance.
- **ROM-BACKED -- climb, retreat, and endpoint:** The Spiker p-code at `$a0f7`
  repeatedly moves and executes the Spike/reverse/convert action
  (`eact_0e`, `$9fc6`). While the Spiker remains in its slot, `enm_shoot`
  continues to evaluate it on each active gameplay pass whenever its position
  and flags qualify; firing is not conditioned on Spike growth or on entering
  retreat. Reaching the return endpoint calls `spiker_hop` (`$a001`), sets
  `enemy_along` to `$f0`, and checks `enemies_pending`. If supply remains, the
  same slot continues its Spiker program. The hop chooses a lane and updates
  its segment/heading; it does not call `$994d` or write `shot_delay`.
  Clearing bit `$80` in `$028a` preserves the Spiker's `$40` shooter flag.
- **ROM-BACKED -- zero pending:** The endpoint performs the hop before testing
  `enemies_pending`. At zero, that same slot is rewritten as a Tanker; it is
  not re-created through the generic enemy initializer. No firing-state
  refresh is specified before conversion. This terminal path is separate from
  the continuing Spiker cycle.
- **ROM-BACKED -- scope:** The Spiker's shooter flag, its shared p-code, and
  `enm_shoot` are not Level-7-only reset machinery. The listing shows no
  cycle-boundary ammunition or timer reinitialization on the Spiker hop.

Relevant local source: [ROM enemy-slot initialization](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L3293),
[Spiker action and endpoint](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4103),
[Spiker hop](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4145),
[shared firing routine](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4546),
and [projectile expiry](../../Reference/arcade/Tempest%20Rom%20Source%20code.md#L4449).

## Current C# trace and candidate relationship

**PROJECT-CURRENT:** [Spiker](../../src/Tempest.Game/Gameplay/Spiker.cs)
contains no ammunition, shot counter, or firing cooldown. It returns a bullet
only when `Growth > 100`, the randomized `shouldRetreat` check succeeds, and
the second `randomInclusive(0, 100) > 70` check succeeds. Reaching the upper
height cap instead changes the state to `Retreating` without a firing attempt.
`Growth` is cleared during retreat.

**PROJECT-CURRENT:** The Spike is a separate persistent object in the Level.
Spiker growth increments only after its height passes the existing lane
Spike's height. A high retained Spike therefore reduces how much new growth is
available before the upper cap. Once that remaining interval cannot exceed
100, C# cannot reach its growth-driven firing branch on that cycle, regardless
of the random outcome. This accounts for a Spiker that keeps cycling but
eventually loses its only firing opportunity.

**INFERENCE about Neal's exact silent cycle:** The code path and its
growth-limit condition are established, and they fit the reported gradual
silence. No state snapshot from the first silent cycle was supplied, so the
exact cycle/Spike height at which Neal's recording first loses the firing
opportunity is not independently measured here.

**PROJECT TRANSLATION:** The current Level 7 candidate restarts the same live
Spiker on the same lane when pending supply remains. `RestartForPendingSupply`
sets `Growth` to zero and `State` to `Building`; it does not reset a firing
field because none exists, and the lane's Spike remains. This differs from
the ROM's lane hop.

**Candidate attribution:** Comparing the worktree Spiker implementation with
`HEAD` shows the firing logic was already present at the accepted baseline;
the candidate adds `RestartForPendingSupply`. The previous return-to-reserve
path removed the object, and a later hatch constructed a fresh Spiker with
fresh height/growth state. The Spike itself remained persistent, and the new
Egg could use a different lane, so re-hatching did not guarantee restored
firing. The candidate did not introduce the growth-only firing rule, but
restarting the same Spiker on the same lane can make the retained-high-Spike
limit recur predictably. The candidate is an exposure of the pre-existing
firing-model mismatch, not evidence that exhausted ammunition was preserved.

In the current Level 7 controller, `UpdateSpikers` calls `Spiker.Update`,
adds a returned bullet, and only then branches at the retreat endpoint:
pending supply causes the same object to restart; zero pending removes it and
converts it to a Tanker. The zero-pending conversion is not a missing reset
case and should remain unchanged.

## Answers to the requested questions

| Question | Finding |
|---|---|
| ROM initialization | A newly allocated Spiker slot gets the `$40` shooter flag and `shot_delay = 0` through generic enemy initialization. No ammo is initialized. |
| Shot limit/governor | Shared projectile-slot capacity (`enm_shotmax`), active-shot-dependent random threshold (`enm_shotcnt`), and each enemy's cooldown (`shot_delay` / `shot_holdoff`). There is no lifetime per-Spiker shot limit. |
| While climbing | The shared firing routine checks eligibility each active pass; after a shot it reloads that slot's cooldown and increments the global active-shot count. |
| While retreating | The routine remains active whenever position and flags qualify; there is no retreat-specific ammo depletion or reset. |
| Bottom with pending supply | The same live slot lane-hops, resets its along position, retains the shooter flag and continues. |
| Explicit cycle refresh? | No ammunition or firing timer is reinitialized at the hop. The cooldown is reloaded after each successful shot, not at each Spiker cycle. |
| Generic enemy initializer on hop? | No. `spiker_hop` does not call `$994d`; the same slot remains live. |
| Level-7-specific? | No. This is shared Spiker machinery and shared enemy firing logic. |
| C# boundary behavior | It restarts the same object and resets `Growth`, but firing state is absent. Zero pending follows the separate conversion branch. |
| Missing/incorrect state? | No missing ammunition reset. C# is missing the ROM-style ongoing shared firing eligibility/cooldown path; its only shot opportunity is tied to growth-driven retreat, which persistent Spike height can make unreachable. |

## Test-gap analysis

| Behavior | Existing coverage |
|---|---|
| First-cycle Spiker firing | **Yes, narrow unit coverage:** `SpikerFiresAtMostOnceOnlyOnSuccessfulGrowthRetreatDecision` tests the growth-retreat random roll and one bullet. |
| Spiker ammo exhaustion | **No:** there is no C# Spiker ammunition field to exhaust. |
| Retreat / persistent Spike | **Yes:** unit and simulation tests cover retreat, endpoint removal, and Spike persistence. |
| Restart with pending supply | **Yes, lifecycle only:** Level 7 tests verify restart and retained identity, not firing state or a subsequent shot. |
| Second/third-cycle firing | **No.** |
| Firing across a lane hop | **No:** the C# candidate intentionally stays on the same lane and there is no firing-state test at that boundary. |
| Zero-pending conversion | **Partial:** Level 7 admission/production coverage checks terminal conversion policy and cargo disposition, but it does not assert Spiker firing immediately before conversion. |

The first-cycle test explicitly encodes the current growth-retreat-only firing
behavior, while lifecycle tests stop at identity/state conservation. No test
crosses a second or third climb and asserts an independently available firing
opportunity. Relevant tests are in
[SimulationTests](../../tests/Tempest.Game.Tests/SimulationTests.cs#L4049),
[Level7AdmissionPolicyTests](../../tests/Tempest.Game.Tests/Level7AdmissionPolicyTests.cs#L378),
and [Level7ProductionTests](../../tests/Tempest.Game.Tests/Level7ProductionTests.cs#L425).

## Smallest future regression and recommended milestone

For the confirmed mismatch, add one deterministic Level 7 lifecycle regression
after the firing model is corrected:

1. Use canonical Level 7, one known Spiker identity, a scripted random source,
   an available enemy-shot slot, and pending generic supply.
2. Leave a persistent Spike high enough that the next climb cannot accumulate
   more than 100 new growth before the upper-height retreat.
3. Run through retreat and the pending-supply restart, retaining the same
   Spiker identity and lane.
4. On the second climb, assert that the ROM-backed shared firing path can
   create a shot when its eligibility, cooldown, random threshold, and global
   capacity conditions are satisfied, despite the lack of a growth-triggered
   retreat. Optionally repeat for a third climb.
5. Assert that cycling adds no generic obligation and preserves identity /
   occupancy. Separately retain the existing zero-pending Tanker conversion
   behavior.

This should fail on the current code because a cycle that cannot cross the
100-growth threshold cannot fire at all. The test should not demand a
cycle-boundary timer reset or per-Spiker ammo; neither is supported by the
ROM trace.

**Exactly one recommended milestone:** implement ROM-backed, ongoing shared
Spiker firing eligibility for canonical Level 7, independent of Spike-growth
retreat, with the existing projectile-capacity/random-pressure concepts and
without inventing ammunition or a forced hop reset. Keep it separate from
movement, cargo allocation, projectile speed, Super Zapper, broader
concurrency, and Level 8.

## Validation and final state

- Read-only source trace and targeted test inspection only; no tests/builds
  were run.
- No production code, tests, behavior specification, development notes, or
  other durable documentation were changed. The required prompt copy and this
  report are the only files created.
- `HEAD` remains `8f1c29a7d3e6ef4150c8ab64264108c3c2a9547b`; the worktree
  candidate and empty index were preserved. No staging, commit, push, or
  interactive game launch occurred.
- Final `git diff --check` verification is recorded after report creation.
