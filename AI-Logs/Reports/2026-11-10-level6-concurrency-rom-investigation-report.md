# Level 6 concurrency and lifecycle: ROM investigation

Date: 2026-11-10

Disposition: **READ-ONLY INVESTIGATION COMPLETE. A LEVEL-6-SPECIFIC
ADMISSION, TERMINAL-LIFECYCLE, AND DEATH-RECONSTRUCTION DIAGNOSTIC IS
RECOMMENDED; NO IMPLEMENTATION AUTHORIZED OR PERFORMED.**

## 1. Scope, baseline, and evidence

The complete substantive prompt and its single standalone completion token
are preserved in the
[matching prompt record](../Prompts/2026-11-10-level6-concurrency-rom-investigation-prompt.md).

Starting branch was `master`, tracking `origin/master`, at the requested
baseline `94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933`. Initial status, index,
and tracked worktree were clean. No unexpected nonignored untracked material
was reported.

Evidence classifications:

- **ROM-BACKED:** supported by instructions/data in the local Rev. 2
  [ROM-derived listing](../../Reference/arcade/Tempest%20Rom%20Source%20code.md).
  This task used the listing, not raw ROM chips or a live cabinet.
- **REFERENCE VERIFIED:** prior accepted documentation's Level 6 source
  findings; not new arcade observations.
- **PROJECT-CURRENT:** source/configuration/test-contract inspection; tests
  were not executed.
- **PROJECT DECISION / PROJECT TRANSLATION:** accepted C# behavior or mapping,
  not a claim of literal ROM equivalence.
- **MANUALLY ACCEPTED:** Neal's prior acceptance of visible C# Level 6 play,
  not hidden concurrency or reconstruction internals.
- **INFERENCE:** consequences of ROM data/routines or recommended work.
- **UNKNOWN:** not established by the available evidence.

The common allocator/selector, occupancy, cargo, zap, and reconstruction
machinery was reused from the
[Level 3](2026-11-09-level3-concurrency-rom-investigation-report.md),
[Level 4](2026-11-09-level4-concurrency-rom-investigation-report.md), and
[Level 5](2026-11-09-level5-concurrency-rom-investigation-report.md)
investigations. Level 6 table values and its implications were checked
separately. No Level 1-5 behavior was reopened.

## 2. Executive findings

1. **ROM-BACKED:** Easy/Medium Level 6 has 22 generic pending originals;
   Hard has 25. There is no fixed final Flipper/Tanker/Spiker recipe.
2. **ROM-BACKED:** Ordinary selectable species are Flippers (minimum 0,
   maximum 5), Tankers (1/2), and Spikers (2/4). Pulsars and Fuseballs are
   unavailable. Despite positive Flipper availability, the zero minimum
   excludes ordinary Flipper entry. Unmet Spiker minimum is preferred before
   Tanker minimum. Tanker creation also requires positive Flipper availability
   because its cargo is Flippers.
3. **ROM-BACKED:** There are seven shared physical enemy places (indices
   0-6). Species availability and physical headroom both constrain ordinary
   admission. A reachable count snapshot can leave a place unused because the
   only positive species availability is for zero-minimum Flippers.
4. **ROM-BACKED:** Each Level 6 Tanker carries two latent Flippers without
   occupying their places. Cargo release bypasses ordinary selection and
   allocates children individually after removing the carrier. Terminal
   Spiker conversion also bypasses ordinary Tanker admission: when generic
   pending supply reaches zero, a retreating Spiker becomes a Flipper-carrying
   Tanker in its existing slot. Thus live Tankers and Flippers can exceed
   ordinary maxima, subject to physical allocation.
5. **ROM-BACKED / INFERENCE:** The Level 6 Spiker action is the shared
   grow/retreat/hop/convert program. However, a species-selection dead end
   can leave generic pending supply nonzero, delaying that terminal conversion.
   The conversion is not a guaranteed fixed wave ending.
6. **ROM-BACKED:** Death reconstruction adds active physical enemies plus
   remaining generic pending supply. Species identity is not retained.
   Materialized cargo counts as active objects; latent cargo is not added as
   two extra obligations. The shared reconstruction routine applies to Level
   6, but the exact visible Level 6 behavior has not been arcade-observed.
7. **PROJECT-CURRENT:** Production C# Level 6 uses a 22-identity ledger and
   the older Incitti-shaped mixed selector, without Level 6 species maxima or
   a seven-slot hatch gate. Its Spikers return to reserve rather than
   converting at zero pending. Death preserves selected original species and
   stable cargo identities, unlike the ROM's generic reconstruction.
8. **INFERENCE:** A full Level 6 concurrency correction cannot be assessed
   with an admission-only diagnostic. At minimum the diagnostic must include
   terminal Spiker conversion/cargo and death reconstruction. A
   Level-6-specific generic-obligation reconstruction translation is likely
   needed for ROM-like behavior, subject to Neal's separate Level 6 arcade
   verification and authorization. The zap-entry mismatch is confirmed but
   should remain a separate decision.

## 3. Level 6 ROM parameter table

The ROM parameter loader uses the displayed level's table entries. Displayed
Level 6 values are:

| Parameter | Level 6 value | Interpretation |
|---|---:|---|
| Easy/Medium generic pending supply | 22 (`$16`) | Original generic entries, dynamically selected into species |
| Hard generic pending supply | 25 | `22 + floor(22/8) + bit2(22) = 22 + 2 + 1`; species limits unchanged |
| Flipper minimum / maximum | 0 / 5 | Positive availability is not selectable with minimum zero |
| Tanker minimum / maximum | 1 / 2 | Ordinary selection thresholds, not a late-live cap |
| Spiker minimum / maximum | 2 / 4 | Ordinary selection thresholds |
| Pulsar minimum / maximum | 0 / 0 | Not eligible |
| Fuseball minimum / maximum | 0 / 0 | Not eligible |
| Tanker cargo types `[0..3]` | 1, 1, 1, 1 | Each selected load is a pair of Flippers |
| Physical enemy capacity | 7 | `max_enm=6` is the highest slot index, indices 0-6 |
| Enemy-shot capacity | 3 | `enm_shotmax=2` is the highest shot index; not enemy places |
| Initial Spike parameter | `$d4` | ROM coordinate; C# maps starting Spikes to height 60 |
| Flipper movement program | `$0b` | Movement p-code selection, not an admission limit |
| Flipper shot holdoff | 65 updates | Separate from enemy-place capacity |

Relevant listing areas are the parameter tables around `$93fa-$965d`, the
shared loader at `$92d6-$93df`, and the admission/allocator at `$98a2-$9afc`.
The Level 6 supply is a count of pending original entries, not active enemies
or a fixed species distribution. Initial Spikes are hazards, not enemies.
ROM pending entries have lane/approach state but no C# stable identity.

Keep the populations distinct:

- **Generic pending supply:** originals still waiting for successful ordinary
  admission; decremented only when an enemy is allocated.
- **Active physical enemies:** one place each, independent of climbing/rim/
  transition state.
- **Latent cargo:** two future Flippers per Level 6 Tanker; no physical place
  until allocated, but it affects Flipper availability.
- **Released/materialized cargo:** each successfully allocated child is a
  live Flipper and consumes one place.
- **Derived enemy:** a terminally converted Spiker becomes a Tanker in the
  same place; it is not a newly selected original.
- **Project stable identities:** C# ledger IDs are an intentional engineering
  representation, not ROM identity fields.

## 4. Ordinary admission and representative states

**ROM-BACKED:** The common `create_enemies` path gates ordinary entry on
`enemies_in + enemies_top <= max_enm` and `zap_running == 0`. Its allocator
searches the seven slots. Successful allocation decrements generic pending
supply. The selector computes max-minus-current availability for each type,
subtracts two Flipper-availability units for each active Tanker carrying
Flippers using byte arithmetic, then clamps availability to physical
headroom.

While ordinary admissions are pending, let `F` include all materialized
Flippers, `T` be active ordinary Tankers, `S` be active Spikers, and
`H = max(0, 7 - F - T - S)`. All selected Level 6 Tankers carry Flippers.
The relevant availability is:

```text
Flipper: min(H, (max(5 - F, 0) - 2*T) modulo 256)
Tanker:  min(H, max(2 - T, 0))
Spiker:  min(H, max(4 - S, 0))
```

These are selector availabilities, not final-population caps. Tanker creation
also fails if Flipper availability is zero; the ROM requires a nonzero
available cargo type, not two available units. Cargo subtraction can wrap
before the physical clamp. The ordinary Flipper minimum remains zero, so
wrapping/positive availability does not make ordinary Flippers selectable.

Selection behavior:

- When multiple types are available, unmet minima are checked in descending
  species-index order: Spiker minimum 2 precedes Tanker minimum 1.
- With both Tanker and Spiker available after minima are satisfied, the ROM
  consults the current pending entry's segment Spike height: below `$cc`
  selects Tanker; `$cc` or above selects Spiker. The code reads the pending
  segment's Spike value; it is not a scan of all global Spike heights.
- The random fallback considers available types only when their minimum is
  nonzero. A sole available Flipper type is likewise rejected because its
  minimum is zero.
- Therefore ordinary Flippers are never selected on Level 6, even if their
  max-derived/cargo-adjusted availability is positive. Tankers and Spikers
  are dynamically selected; this is not a fixed recipe.

Bounded arithmetic examples (not a simulation or a claim that every state is
reached):

| F / T / S | Free places | F / T / S availability after physical clamp | Consequence |
|---|---:|---:|---|
| 0 / 0 / 0 | 7 | 5 / 2 / 4 | Spiker minimum is unmet, so Spiker is selected first |
| 0 / 0 / 2 | 5 | 5 / 2 / 2 | Spiker minimum is met; Tanker minimum is next |
| 0 / 1 / 2 | 4 | 3 / 1 / 2 | Both minima met; pending-segment `$cc` rule decides between Tanker and Spiker |
| 0 / 2 / 4 | 1 | 1 / 0 / 0 | Only Flipper availability is positive, but its zero minimum blocks admission; one place remains unused |
| 2 / 1 / 2 | 2 | 1 / 1 / 2 | One Tanker reserves two of the Flipper availability units; species minima are already met |

In the `0/2/4` snapshot, the six existing objects are two Tankers and four
Spikers; their latent cargo does not occupy additional places. The last free
place alone cannot admit an ordinary Flipper. A pending entry can retry when
physical space and relevant species/cargo availability reopen, for example
after an enemy is actually removed, a species count falls below its maximum,
or `zap_running` returns to zero. A Tanker split changes both species counts
and occupancy; the resulting children can consume the freed place(s).

## 5. Tanker and cargo lifecycle

**ROM-BACKED:** Ordinary Tanker eligibility has minimum 1, maximum 2 and
requires positive Flipper availability. Each live carrier occupies one
physical slot; its two latent Flippers occupy none. Tanker selection does not
reserve two free physical places at admission.

On natural/shot split, ROM removal clears the carrier's place and then tries
to allocate its cargo children independently without ordinary selection or a
generic-pending decrement. With one available place, at most one child can
be installed. A carrier removal followed by two successful child allocations
can increase live population by one. Cargo can exceed the ordinary Flipper
maximum; ordinary Tankers can be exceeded by terminal Spiker-derived Tankers.
The seven-slot array remains the literal ROM physical bound.

**PROJECT TRANSLATION:** C# tracks two stable cargo identities per Tanker and
preserves complete-pair cargo behavior. That accepted complete-pair
translation is not the ROM's two independent physical allocations and can
temporarily exceed seven live C# enemy objects. It was not reopened here.
Any future Level 6 admission/headroom diagnostic must preserve the accepted
pair and block subsequent ordinary admission as appropriate; it must not
claim strict ROM seven-object behavior for the pair-release edge.

## 6. Spiker lifecycle and the terminal condition

**ROM-BACKED:** Eligible Level 6 Spikers use the shared p-code program at
`$a0f7` and action `$9fc4`: grow the persistent Spike, reverse/retreat, hop
to a shortest-Spike lane, and continue in the same live physical place.
Growth, retreat, and hop do not free or reacquire a place. While
`enemies_pending != 0`, a retreating Spiker continues this lifecycle; it is
not returned to generic supply or passed through ordinary selection again.

At `enemies_pending == 0`, the action changes that same object to a
Flipper-carrying Tanker (type bits and cargo kind 1). Level 6's load tables
also encode kind 1, i.e. two Flippers. This conversion bypasses ordinary
Tanker availability and uses the existing slot. It can raise live Tankers
above ordinary maximum 2; at most four admitted Spikers can be converted,
subject to the shared seven-place population. Conversion does not update the
species counters in the action code; with no generic pending supply, no
further ordinary admission should rely on those counters before reset.

The terminal condition is a pending-supply count, not merely "no currently
eligible species." For example, `F=0,T=2,S=4` leaves one physical place but
no selectable species. A failed selector attempt leaves `enemies_pending`
unchanged, so Spikers do not yet convert. Later actual removals can reduce a
species count and/or open enough headroom to resume admission. Thus Level 6
can have admission stalls before all generic obligations have entered, and
the terminal conversion occurs only if pending supply eventually reaches
zero.

**PROJECT-CURRENT:** Level 6 Spikers that reach the C# `ReturnedToReserve`
state are removed and their original IDs are returned to reserve. Level 6
does not enable the Level 4/5 terminal conversion or Level 7/8 generic
conversion branches. It can therefore re-hatch Spikers instead of retaining
the same live slot as a terminal Tanker. Existing Spike hazards persist.
The C# retreat boundary is not the ROM's literal along-coordinate boundary.

## 7. Pulsars, Fuseballs, capacity, rim, and completion

**ROM-BACKED:** Level 6 Pulsar and Fuseball minima/maxima are zero; neither
can enter ordinarily, and neither contributes a Level 6 species lifecycle.
Their general ROM actions are irrelevant to this wave.

| Entity/state | ROM physical-capacity treatment |
|---|---|
| Flipper, Tanker, or Spiker, including climb, pause, transition, retreat, or hop | One occupied enemy-array slot |
| Enemy transferred from tube to rim | Still one occupied slot; `enemies_in` becomes `enemies_top` |
| Converted Spiker/Tanker | Same occupied slot |
| Latent Tanker cargo | No slot |
| Each allocated cargo Flipper | One slot |
| Pending original / pending Egg-like entry | No active enemy slot until allocation |
| Pending Super Zapper target | Still occupies its enemy slot until removed |
| Enemy bullet / persistent Spike | Separate projectile/hazard storage; no enemy place |

The ROM allocator uses `max_enm=6` as a highest index and separately checks
species availability against the seven-place headroom. A visual state change
does not free a place. Actual destruction/removal frees the enemy slot;
reaching the rim only transfers its accounting. Cargo release removes the
carrier and independently allocates children. Super Zapper removal clears
Tanker cargo flags before removing that carrier, so it does not produce a
live cargo split.

ROM completion is separate from admission capacity: the common end check
waits for zero generic pending supply and the applicable transient/shot
conditions, then requires surviving enemies to have reached the rim before
the level exit path. Rim transfer alone does not erase the enemy.

**PROJECT-CURRENT:** C# Level 6 has no shared seven-slot allocator. Active
lists include rim/transition/pending-zap enemy objects, and the approach gate
counts Flippers, Tankers, Spikers, and pending shot-cargo activation; it is
not a hard hatch cap. Eggs are pending obligations rather than active enemy
objects. C# production completion/all-rim logic instead checks the stable
original/cargo ledger, reserve and Egg state, pending cargo activation, and
that each remaining active enemy is at rim; exact identity sets must match.
This project accounting is intentional infrastructure, not ROM accounting.

## 8. Death and reconstruction

**ROM-BACKED:** The shared death routine reconstructs generic pending supply
from:

```text
enemies_in + enemies_top + enemies_pending
```

The result is capped at `$3f`. The active enemies are discarded and the
pending-entry machinery subsequently selects their species normally using
the loaded Level 6 data. No active Flipper/Tanker/Spiker species identity is
restored.

The formula counts each physical carrier as one active enemy. Its two latent
cargo Flippers are not separate active slots and do not add two to this
reconstructed quantity. Once cargo has materialized, each live child is a
physical active enemy and contributes one. Thus the ROM-backed consequence
is one generic obligation for an unresolved carrier, but one per unresolved
materialized child; it is not a fixed former-Tanker recipe. Level 6 has no
species-specific exception to the shared reconstruction path.

**PROJECT-CURRENT:** `Level6Production` does not enable
`UsesGenericRomDeathReconstruction`; that setting applies to canonical Level
5 and the Level 7/8 ROM-policy configurations. C# Level 6 calls the ledger's
identity-preserving reconstruction: unresolved originals retain selected
species, Tanker cargo identities (including latent cargo) remain, and
materialized cargo can return through cargo Eggs. Existing tests assert
Level 6 ledger/cargo reconstruction and persistent Spike behavior. This
differs from ROM generic obligations and can change both post-death species
and unresolved quantity when latent or materialized Tanker cargo exists.

**ARCADE-OBSERVED boundary:** Neal's Level 5 reconstruction observation and
accepted one-child-to-one-generic-obligation translation are recorded in the
durable spec. They are not direct Level 6 observations. The shared ROM
routine makes an analogous Level 6 translation likely, but Level 6's
post-death visible composition and cargo behavior need their own manual
verification before acceptance.

## 9. Super Zapper entry interaction

**ROM-BACKED:** The same `create_enemies` routine used on Level 6 blocks
ordinary pending entry while `zap_running` is nonzero. The zap routine removes
targets over its sequence; a target continues occupying its place until
removal. This is a shared ROM gate, not a Level 6 table exception.

**PROJECT-CURRENT:** C# Level 6 uses the normal production Egg/approach path
without a `SuperZapper`-active admission gate, so ready/approaching ordinary
enemies can continue through the C# hatch path during a zap. This is a
confirmed ROM/C# admission mismatch. The corresponding Level 5 zap deferment
was explicitly accepted; this investigation does not reopen it or recommend
a Super Zapper redesign. Treat any Level 6 change to this interaction as a
separate, explicitly authorized choice.

## 10. Current canonical C# Level 6

Inspected the canonical
[Level 6 configuration](../../src/Tempest.Game/Levels/LevelConfiguration.cs),
[Level scheduling](../../src/Tempest.Game/Levels/Level.cs),
[gameplay selection/lifecycle](../../src/Tempest.Game/TempestGame.cs),
[shared selector](../../src/Tempest.Game/Levels/Level3WaveSelection.cs),
[identity/cargo ledger](../../src/Tempest.Game/Levels/Level3WaveLedger.cs),
[Spiker](../../src/Tempest.Game/Gameplay/Spiker.cs),
[Tanker](../../src/Tempest.Game/Gameplay/Tanker.cs), and the
[Level 6 production tests](../../tests/Tempest.Game.Tests/Level6ProductionTests.cs).
Inspection is not a claim those tests were run.

- **Supply/configuration:** 22 fixed original ledger identities, four initial
  Eggs and 18 reserve; Flippers/Tankers/Spikers enabled; Pulsars/Fuseballs
  absent; 16 starting Spikes mapped to C# height 60. No ROM Hard-supply
  adjustment is applied to this fixed C# configuration.
- **Scheduling:** the production-ledger path releases up to six reserve
  identities per 50 active ticks. It begins approaches while active units
  are `< 9` and fewer than two Eggs are approaching, with a random roll except
  near the end. This is not a physical or hatch ceiling: ready Eggs are
  processed sequentially and can hatch without a seven-enemy cap.
- **Selection:** Level 6 is not routed through the accepted Level 3, 4, or 5
  admission policy. The three-species configuration uses
  `SelectLevel4OriginalType`, an Incitti-shaped random candidate picker.
  Flippers remain allowed; Tanker candidates use the general six-Tanker
  bound; Spikers are suppressed near the end or when the selector's
  Spiker count is above three, and the factory allows up to four. It has no
  ROM min/max availability accounting or physical-headroom check.
- **Cargo:** C# Tankers use stable paired cargo identities and the accepted
  complete-pair release translation. Latent cargo is represented in the
  ledger but is not used by a Level 6 admission policy. Cargo release does
  not use the ROM's independent physical-slot allocator.
- **Spikers:** C# Spikers build/retreat and return the original to reserve.
  There is no canonical Level 6 zero-pending Spiker-to-Tanker conversion.
- **Death:** Level 6 preserves selected original types, unresolved cargo
  identities, and latent cargo through ledger reconstruction. This is
  different from Level 6 ROM's species-agnostic active-plus-pending count.
- **Population/completion:** active units include the relevant enemy lists
  and pending shot-cargo activation for the approach gate; rim and pending-zap
  objects remain in their lists. There is no ROM-equivalent seven-slot
  enforcement. The C# all-rim/complete path validates unresolved original
  and cargo identity sets rather than ROM species counters.
- **Accepted-policy isolation:** Level 1/2 Flipper limits and Level 3/4/5
  admission policies are scoped to their canonical configurations. Level 6
  does not inherit their gameplay constants and remains on the older shared
  mixed selector. Level 5's generic death-reconstruction translation is not
  enabled on Level 6.
- **Other aligned values:** the accepted Level 6 C# firing configuration is
  a 65-update Flipper shot holdoff and three enemy-shot slots, separate from
  enemy concurrency. Existing Level 6 tests cover configuration,
  reconstruction, persistent Spikes, life-loss score/lives, and the
  71-update Avoid Spikes wait.

## 11. Accepted Level 6 behavior to protect

The durable [behavior specification](../../TEMPEST-BEHAVIOR-SPEC.md) and
[development notes](../../TEMPEST-DEVELOPMENT-NOTES.md) record prior
REFERENCE-VERIFIED source findings, PROJECT DECISIONS, and visible C# manual
acceptance. A future concurrency milestone must preserve, unless separately
authorized:

- the accepted closed 16-lane triangle geometry and Level 5-to-6 safe
  progression, plus the later accepted Level 6-to-7 safe progression;
- the 22-original C# supply, four initial Eggs/18 reserve, Flipper/Tanker/
  Spiker eligibility, starting persistent Spikes, and accepted visible
  movement, firing, scoring, damage, and collision behavior;
- the existing complete-pair Tanker cargo identity/release translation,
  Spiker/Spike gameplay and hazard behavior, Super Zapper behavior, and
  accepted death/re-entry experience;
- rim combat, all-rim/exit requirements, score/lives carry, fresh-wave
  initialization, and deterministic recording/playback behavior.

Neal manually accepted visible C# Level 6 gameplay. Documentation explicitly
does not claim that this established exact arcade timing, random species
composition, Spiker speed, hidden accounting, or concurrency. Do not treat
current source behavior as accepted ROM fidelity, and do not retune accepted
visible behavior as part of a diagnostic.

## 12. ROM versus C# comparison and recommendation

| Area | ROM-backed Level 6 | Current canonical C# | Assessment |
|---|---|---|---|
| Supply/species | 22 default / 25 Hard; dynamic F/T/S selection | Fixed 22; legacy shared mixed selector | Supply aligns at default; admission differs |
| Ordinary limits | F 0/5, T 1/2, S 2/4; zero-min F excluded | No matching min/max policy; Tanker up to 6, Spiker up to 4, Flippers allowed | Confirmed mismatch |
| Physical capacity | Seven shared enemy places; species headroom too | No seven-place allocator or hatch ceiling | Confirmed mismatch |
| Tanker cargo | Two latent Flippers; cargo reduces F availability; children allocate separately | Accepted stable complete pairs; can exceed seven at pair release | Intentional project translation; preserve |
| Spiker terminal lifecycle | Same-slot Tanker conversion only at zero generic pending | Returns to reserve; can be selected again | Confirmed mismatch |
| Death | Active objects plus pending become generic obligations; species lost; latent cargo not counted separately | Selected original species and cargo IDs survive | Confirmed mismatch; arcade observation needed |
| Pulsars/Fuseballs | Ineligible | Excluded from canonical Level 6 | Aligned |
| Zap entry | Paused while `zap_running` | Ordinary hatch path does not pause | Confirmed mismatch; separate decision |
| Shot limit | Three enemy-shot slots; separate from enemy capacity | Three slots | Aligned |
| Geometry/progression/visible play | Listing/reference data and shared ROM flow | Prior manually accepted project behavior | Accepted project behavior; not proof of hidden equivalence |

**Can Level 6 reuse Levels 3-5 engineering infrastructure?** Yes: the
identity/cargo ledger, ready-Egg wait behavior, validated admission snapshots,
physical-headroom calculation, and Spiker-to-Tanker conversion mechanics are
reusable engineering. Keep Level 6 supply, minima/maxima, zero-minimum
Flipper exclusion, Tanker cargo eligibility, segment-Spike decision,
terminal condition, and death translation explicit and Level-6-specific.

**Would copying an earlier policy unchanged be incorrect?** Yes. Level 3 has
no Spikers and a one-Tanker limit; Level 4 has no ordinary Tankers and
different Flipper/Spiker limits; Level 5 has F/T/S limits 0/5, 1/1, 2/3.
Level 6 is 0/5, 1/2, 2/4 and has its own Level 6 pending-segment Spike input.
Do not copy those constants or their selected species behavior.

**Is an admission-only diagnostic sufficient?** No, not for concurrency
behavior as experienced over a wave. Cargo release and terminal Spiker
conversion can exceed ordinary species maxima and alter active populations;
death reconstruction can replace preserved C# species/cargo identities with
generic ROM obligations. Test these alongside ordinary admission.

**Is a Level-6-specific generic-obligation death translation likely
required?** Yes, if the goal is ROM-like reconstruction: the shared ROM code
and Level 6's absence of a species-specific exception give strong evidence
for it. The Level 5 accepted translation is useful engineering precedent,
not automatic Level 6 acceptance. Neal should verify Level 6 unresolved
ordinary enemies, a Tanker with latent cargo, and materialized cargo after
death before accepting the C# mapping.

**Recommended smallest safe next milestone:** After Neal reviews this report,
authorize a bounded Level-6-specific diagnostic for ordinary admission plus
terminal Spiker/cargo lifecycle and generic-obligation death reconstruction.
Keep current production behavior unchanged until the diagnostic and manual
Level 6 comparison are accepted. Use separate bounded deterministic cases for
minima/zero-minimum selection, the open-place dead end, Tanker cargo
eligibility/release, zero-pending Spiker conversion, and deaths with latent
versus materialized cargo. Preserve complete-pair cargo translation and all
accepted visible behavior. Keep the known zap-entry mismatch as a separately
authorized follow-up, not a Super Zapper redesign.

Visible Level 6 arcade behaviors Neal should later verify include ordinary
Tanker/Spiker admission and relative frequency; whether ordinary Flippers
ever enter; concurrency when species limits are reached with places open;
Spiker grow/retreat/hop and zero-pending conversion; Tanker cargo count and
release when places are scarce; post-death species/presentation and unresolved
quantity for latent versus materialized cargo; and whether entry pauses during
the Super Zapper. These are not established by the prior C# manual acceptance.

## 13. Unknowns, validation, and final state

Unknowns/limits:

- No Level 6 MAME/cabinet comparison was made. Frequency of dynamic species
  choices, exact visible conversion timing, full-slot cargo outcomes, and
  post-death presentation still require Neal's observation.
- The report is based on the local Rev. 2 listing, not a new binary-level
  verification or comparison against other ROM revisions.
- No ROM-to-C# timing/coordinate calibration was attempted. `$d4`, `$cc`,
  Spiker movement cadence, and C# height 60 must not be equated literally.
- The selector comment describes a shortest-Spike choice, while the
  instructions inspect the pending entry's segment Spike value; no claim is
  made that this is a global minimum scan.
- Dynamic selection does not establish a fixed final species recipe or
  frequency distribution.

**PASS:** Initial HEAD matched the requested baseline; branch/upstream and
clean index/worktree were checked. The exact prompt copy was created under
ignored `AI-Logs/Prompts/`.

**PASS:** Bounded source/disassembly and documentation inspection completed;
representative occupancy states were calculated without simulation.

**PASS:** No production code, tests, accepted gameplay documentation,
reference material, protected local recordings/assets, or project settings
were modified. No test/build/gameplay test or interactive game launch was
performed. No staging, commit, or push occurred.

Final checks confirmed HEAD remains at the requested baseline, no staged or
tracked changes exist, `git diff --check` passes, and both AI-Logs records are
ignored. No commit/push or Level 7 concurrency work was started. Neal and
Copilot should review these findings before authorizing any Level 6 change.
