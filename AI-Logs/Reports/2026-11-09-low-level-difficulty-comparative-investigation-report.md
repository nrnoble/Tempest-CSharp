# Low-Level Difficulty: Arcade / C# Comparative Investigation

Date: 2026-11-09
Disposition: **READ-ONLY INVESTIGATION COMPLETE — NO GAMEPLAY CHANGE RECOMMENDED WITHOUT A CONTROLLED FOLLOW-UP**

## Executive finding

The strongest explanation for excessive lower-level C# survival pressure is a
**combination dominated by concurrent enemy density and the way the accepted
Egg schedule feeds that density**. The C# Level 1 and 2 waves have the same
base total as the ROM, but C# has no four-Flipper active cap; it can put all
10/12 Flippers into active play. The ROM limits Level 1/2 to four Flippers
and six total active enemies. This is a concrete and sizeable presentation
difference before considering rim pursuit or projectiles. C# Levels 3-6 also
lack the ROM's six-active creation gate and have several looser species caps.

The C# 50-tick Egg/release model is not a ROM translation. It releases
different batches, permits queued Eggs to hatch beyond the ROM active cap, and
can synchronize threats differently. A bounded C#-only harness using accepted
Level logic found all 10 Level 1 originals active by tick 515 and all 12 Level
2 originals active by tick 615 in a no-kill/no-enemy-update scenario. These
are **HEADLESS MEASUREMENTS of an intentionally simplified C# schedule**, not
arcade timing or a survival simulation.

Several conspicuous differences point the other way: C# has more player-shot
slots, allows broad pivot-lane shot hits, and resolves player shots before
Flipper contact. They tend to help the player, not explain why C# feels
harder. Per-Flipper firing holdoff, enemy-shot capacity, and random thresholds
largely follow the local ROM values on Levels 1-7; additional active Flippers
can nevertheless create more firing opportunities. The accepted 2.5 rim
calibration is a plausible contributor, but it is a manual project decision,
not ROM equivalence, and its exact timing comparison is unresolved.

**Recommendation:** the next experiment should be a reversible,
development-only Level 1 diagnostic that applies only the ROM's
four-concurrent-Flipper cap. Keep supply, Egg release clock, firing, rim
speed, shots, collisions, and production defaults unchanged. Compare the
same Level 1 input recording on the accepted baseline and diagnostic build,
then make a short normal-play comparison on Neal's cabinet. Do not implement
that experiment as part of this investigation.

## 1. Starting Git state

Initial repository checks established:

- Branch: `master`, tracking `origin/master`.
- `HEAD` and `origin/master`: `c1a974a625eea2dfaecf30d1b8d729e022ef17e8`
  (`Close out accepted spinner sensitivity diagnostic`); zero ahead/behind.
- Index and tracked worktree: clean.
- Sole initial untracked path: `Reference/arcade/roms.rev2/` (21 local Rev 2
  chip files). It was left untouched.
- Remotes: `origin` fetch/push both point to the project's GitHub repository.
- No stage, commit, push, reset, restore, or other Git operation that changes
  repository history or index was performed.

## 2. Accepted baseline and Neal's observation

The comparison uses accepted production Levels 1-8 at the requested baseline,
not earlier candidate worktrees. Neal reports that the accepted C# campaign
feels **materially harder** than his original Atari machine on lower levels.
His observed difference remained after the accepted spinner-sensitivity
feature: `0.8` improved physical control, default remains `1.0`, and changing
sensitivity did not resolve the broader concern.

**ARCADE OBSERVED:** this is Neal's expert comparison with his own machine.
No new arcade play, video recount, or survival-rate measurement was performed.
The cabinet is **believed Medium, but not hardware-verified**.

## 3. Difficulty qualification and service-mode check

**ROM-LISTING-BACKED / REV-2-BINARY-CONFIRMED:** the difficulty bits decode as
`00=Medium`, `01=Easy`, `10=Hard`, and `11=Medium`; the code reads the
`zap_fire_starts` option byte. Easy/Hard affect enemy-side parameters. For
example, Hard adds about one eighth to supply (Level 1: 10 to 11), can increase
enemy-shot capacity, and adjusts Flipper speed; it does not increase player
shot power. The project-local listing does not establish a factory/default
physical switch position or Neal's cabinet's actual value.

The checked-in ROM listing includes a non-selftest service-mode display that
reads and draws a difficulty label. No cabinet service manual or safe
button-by-button entry procedure was found in the stored project references.
The listing does not establish how to access that display on Neal's cabinet or
which physical switch position corresponds to each value. Therefore this
report does **not** instruct Neal to enter an undocumented service procedure
or to alter any switch. If he later wants verification, use the cabinet's
existing service documentation or an arcade technician to read the displayed
difficulty without changing settings.

## 4. Prior evidence reused

Historical reports were read and reused rather than re-running already
settled work:

- `2026-11-04-blaster-fire-rim-difficulty-rom-investigation-report.md`
- `2026-11-04-rim-speed-2.5-default-candidate-report.md`
- `2026-11-09-spinner-sensitivity-closeout-report.md`
- `2026-11-08-level1-7-enemy-supply-rom-investigation-report.md`
- `2026-11-05-rim-speed-2.5-acceptance-closeout-report.md`
- `2026-11-03-flipper-rim-speed-investigation-report.md`
- `2026-11-03-production-flipper-firing-investigation-report.md`
- `2026-10-31-c2-tanker-firing-policy-analysis-report.md`
- `2026-10-30-tanker-enemy-bullets-implementation-report.md`
- `2026-10-30-spiker-source-analysis.md`
- `2026-11-04-level7-reentry-rom-investigation-report.md`
- `2026-11-04-level1-stationary-rim-combat-comparison-report.md`
- `2026-11-04-player-shot-flipper-contact-ordering-implementation-report.md`
- `2026-11-08-simulation-timing-speed-rom-investigation-report.md`
- `2026-11-08-blaster-shot-rom-visual-investigation-report.md`
- `2026-11-08-level8-accepted-production-closeout-report.md`
- `2026-10-30-super-zapper-analysis-report.md`
- Related Level 1-8 implementation/acceptance, Tanker, Super Zapper, playback,
  and campaign-soak reports.

The earlier stationary-defense reports describe why C# was too safe, not the
current question. The shot/contact-ordering change discussed there was
rejected and is not in this accepted baseline. Neal's later acceptance of the
2.5 rim calibration is relevant evidence, but it does not prove that the
current whole-wave pressure matches the arcade.

## 5. Files, tests, ROMs, and references inspected

Current code inspected included `TempestGame.cs`, `Program.cs`, `Flipper.cs`,
`Shot.cs`, `EnemyBullet.cs`, `Egg.cs`, `Tanker.cs`, `Spiker.cs`,
`SuperZapperSystem.cs`, `Level.cs`, `LevelConfiguration.cs`, and the
Level 3 wave ledger/selection path. Tests consulted included
`ArcadeFlipperFiringTests`, `CampaignSoakTests`, `LevelConfigurationTests`,
`RimPursuitTests`, and the existing simulation, projectile, Super Zapper,
death/re-entry, progression, recording, and headless-playback coverage.

The sole Atari listing authority was
`Reference/arcade/Tempest Rom Source code.md`. No Internet source, other
disassembly, other Tempest port, or external source was used. The annotated
instruction bytes for the active-game order, enemy cap, ROM shot routine,
main-loop wait, and difficulty display were matched against the local Rev 2
ROM chips. The raw Rev 2 `tube_angle` and `lev_remap` bytes were also read
directly from `136002-118.k1`; displayed Levels 1-8 map to tube rows 0-7.
This confirms the local ROM evidence, not the listing author's comments where
code/data flow is ambiguous.

Incitti source was not used as Atari authority. Existing Incitti-based reports
were consulted only to identify which current C# timers/speeds are
secondary-reference translations rather than direct ROM matches.

No existing test suite was run. One temporary, ignored console harness
referenced the current game project to measure accepted Level release logic;
it was removed after measurement. No interactive game was launched.

## 6. Current C# update-order map

In active play, `TempestGame.RunOneStep` performs, in order:

1. Player firing, then player movement.
2. Super Zapper activation.
3. Move/collide player shots; move existing enemy bullets.
4. Check early wave completion.
5. Advance reserve Egg release and Egg approach; hatch ready units.
6. Update Flippers, Tankers, Spikers, Fuseballs, and Pulsars.
7. Update Flipper firing and check enemy/player contacts and enemy-bullet hits.
8. Resolve Spike contact, all-rim completion, Hurry-Up, and completion.

Thus C# shots can remove a Flipper before that tick's movement/contact check;
the accepted ordering is player-favorable on the same-tick rescue case. Bullets
created by an enemy during its later update do not move until the next tick.
The production tick is `1/60` second by project definition.

## 7. ROM gameplay-pass order

The ROM `state_04` order is:

`move_player -> player_fire -> check_zap -> create_enemies -> move_enemies ->
move_shots -> enm_shoot -> pshot_hit -> hit/death processing -> wave/state check`.

The newly created player shot gets a birth-position hit check in `player_fire`.
An already-moving shot's hit scan is later than enemy movement/contact in the
pass. The main loop waits for nine increments of an interrupt-updated counter
before a game-state pass. The local source does not establish the interrupt
frequency or real-time duration of those nine increments.

## 8. Player-shot comparison

| Property | ROM | Accepted C# | Likely direction |
|---|---|---|---|
| Slots | 8 | 10 | More C# capacity can help defense. |
| Held-fire attempt | One attempt per gameplay pass if a slot is free | Immediate first shot, then at most every 3 simulation ticks | Rate comparison is unknown without a clock conversion; C# cooldown can constrain firing. |
| Travel | +9 along per movement pass, from `$10` until `$f0`; 25 unimpeded movement calls | Height -6 from player depth 400 to below zero; about 67 updates | Different coordinate clocks; C# shots occupy the path longer in project ticks. |
| Normal hit/retirement | A target hit marks that shot for removal; Spike path has its own slowdown | Hit shot is removed | Similar broad effect, different paths. |
| Flipper pivot lanes | Hit-tolerance plus ROM pivot-state/shot-time segment comparisons | Both source and destination lanes are accepted during the pivot | C# may be more permissive for player shots. |
| Collision order | Moving-shot collision after enemy movement/contact; new-shot birth check is earlier | Shots collide before enemy updates/contact | C# can save the player on a same-tick lethal pivot. |

Prior deterministic C#-clock measurements (historical report, not rerun here)
found 20 accepted shots and 40 capacity-blocked attempts in a 100-tick
held-fire/no-hit C# stream; an 8-slot same-clock ablation accepted 16. In a
four-pursuer fixture, 8 versus 10 slots did not change the death tick. Those
figures do not convert ROM passes to C# ticks or establish full-wave
survivability.

**Assessment:** shot count, lane eligibility, and ordering are not strong
explanations for C# being harder; several provide C# advantages. Slower
effective availability from cooldown/long occupancy remains possible, but no
matched-clock or matched-arcade observation establishes it.

## 9. Enemy-shot comparison

On Levels 1-7, current C# Flipper firing uses per-level holdoffs
`80, 77, 74, 71, 68, 65, 62` ticks, capacities `2, 2, 2, 3, 4, 3, 3`, and
the ROM's probability thresholds conditioned on the number of active enemy
shots (`00, E0, F0, FA, FF`). Level 8 retains the accepted C# Level 7
holdoff/cadence translation and uses capacity 4; that is not claimed to be a
literal ROM-time conversion. A new eligible Level 1 Flipper can fire
immediately when no enemy shot is active. The ROM permits shots only when the
enemy's current position/state gates are satisfied; the C# height cutoff
translates that position boundary but is not an independently measured
coordinate conversion.

At Level 1, the global two-shot capacity matches ROM Medium/default. Thus the
individual C# firing rule is not shown to be more aggressive. However, with
more than four active C# Level 1 Flippers, more independent Flippers can reach
their firing checks and compete for that same two-shot pool. This is a
plausible **density interaction**, not proof of higher projectile rate.

From Level 3 onward, C# Tankers are armed with 1-2 bullets and a randomized
initial timer based on the Incitti implementation. The ROM uses a shared
per-enemy `shot_delay`/holdoff path; the local listing does not establish the
same finite-ammunition policy. C# Tanker bullets have +3 height/update and a
strict player gap under 8; exact ROM-to-C# speed and collision-width conversion
is not proven. The C# finite ammunition could reduce rather than increase
Tanker fire pressure. Projectile lifetimes in real time remain unknown.

No finding supports “enemy bullets are simply too fast/frequent” as the main
Level 1 cause. Aggregate pressure could be higher when excess C# Flippers
coexist; the ROM/C# update clocks, bullet hit region, and actual shot counts
have not been matched experimentally.

## 10. Supply and composition

Base original supplies match the ROM table through Level 7:
`10, 12, 15, 17, 20, 22, 20`. The ROM Hard branch raises these to
`11, 14, 17, 19, 23, 25, 23`; the cabinet's setting is unknown and C# has no
production difficulty selector. The accepted Level 8 supply is 24.

ROM Level 1-2 type tables permit Flippers with a per-type maximum of four.
ROM Level 3 allows at most four Flippers and one Tanker. Level 4 allows at most
four Flippers and two Spikers, with no ordinary Tanker selection. Levels 5-6
exclude ordinary bottom Flippers (while still allowing cargo Flippers) and
have tighter Tanker/Spiker limits. The ROM also gates ordinary enemy creation
at six total active units for all levels.

C# Levels 1-2 are Flipper-only with no four-Flipper or six-total active cap.
Levels 3-6 do not implement the ROM's total six-active gate; the shared picker
can allow up to six Tankers and four Spikers, and Levels 5-6 can differ in
ordinary Flipper eligibility. The accepted Level 5 no-direct-Flipper rule
remains in place. Levels 7-8 have ROM-shaped six-active/species policies and
generic re-entry approximations; they are materially closer on this point.

The ROM's pending counter is generic; it is not equivalent to C# original
identity IDs. Tanker cargo is latent until release in the ROM and separately
tracked in the C# ledger. Do not add cargo children to the original supply
count or treat a matching supply total as proof of matching visible enemy
count.

## 11. Release/hatch pacing and bounded headless measurement

ROM setup creates generic pending entries with per-entry visibility delays.
`create_enemies` runs every active game pass, subject to the six-total active
gate and species selection limits. It is not a four-initial-Egg plus
50-tick-reserve model.

C# starts with four Eggs. Levels 1-2 release one reserve Egg at each
50-active-tick interval if there is queue room; approach selection is
50-tick-based with at most two Eggs approaching. Their Eggs move on an
8-update cadence. Ledger-backed Levels 3-8 release up to six reserve/cargo
Eggs together per 50-tick interval; approaches are limited to two, with a
different active-count gate. Several Eggs may hatch in one simulation update.
These are accepted project policies, not proof of arcade-equivalent pacing.

Temporary headless schedule measurement, with no kills or enemy movement:

| C# level/path | Measurement | ROM bound |
|---|---|---|
| Level 1 | 4 starting Eggs; 10 total Flippers became active by tick 515; 6 active by tick 339. | At most 4 Flippers; at most 6 total active enemies. |
| Level 2 | 4 starting Eggs; 12 total Flippers became active by tick 615; 6 active by tick 339. | At most 4 Flippers; at most 6 total active enemies. |
| Level 3 mixed schedule, forced all-Flipper selection, 16 seeds | 6 active at ticks 110-286; 9 at 177-394; 10 at 179-404. | At most 6 total active; at most 4 Flippers. |
| Level 4 schedule, forced all-Flipper selection, 16 seeds | 6 active at ticks 110-282; 9 at 156-390; 10 at 226-408. | At most 6 total active; at most 4 Flippers. |

The Level 3/4 harness deliberately forced Flipper selection and did not update
enemy movement, collisions, bullets, deaths, or actual game combat. It measures
only accepted C# Egg/release/hatch scheduling and the active cap behavior
available to that path. These seed ranges are **HEADLESS MEASUREMENT**, not a
species-distribution estimate, an Atari comparison, or a full-wave outcome.
No production/test files were retained from the harness.

**Assessment:** release/hatch density plus absent caps is the strongest
survival-pressure candidate. Matching total supply does not negate this
difference.

## 12. Below-rim movement

Level 1 C# Flippers climb at 2 project units/update and do not lane-flip below
the rim; Level 2 enables below-rim flipping. Levels 3-5 use the accepted
2/8/24 configuration, Level 6 uses 2/9/23, and Levels 7-8 have their own
accepted movement programs/configuration. Current Egg-to-Flipper startup and
accepted project routes are distinct from literal ROM along coordinates.

The ROM selects movement programs by level, stores per-level Flipper speed,
and uses tube-angle/Spike conditions for some routes. The Level 1 ROM route
starts with straight climbing and hands off to the shared rim program. A
source-level numeric comparison of ROM fixed-point along units to C# height
is not established for general climb timing. No specific verified evidence
shows Level 1 C# Flippers reach the combat rim sooner than the ROM. Level 2-8
route discrepancies are secondary to the measured concurrency concern and
should not be bundled into its first experiment.

## 13. Rim arrival and pursuit

Accepted C# rim speed is `configured turn rate * 2.5` by default; the
multiplier is a **PROJECT DECISION / MANUAL CALIBRATION**, not a ROM literal.
Levels 1-5 therefore use 20 degrees/update with a 24-tick pause; Levels 6-8
use 22.5 degrees/update with a 23-tick pause. On accepted C# Level 1 geometry,
the measured directed adjacent-lane sweep is about 156.2-158.4 degrees:
8 updates at 20 degrees/update, then 24 pause updates before the next
adjacent-lane turn begins. This is approximately 32 updates from one turn's
start to the next turn's start.

The ROM Level 1 top program uses a four-count check and `flip_top_accel=2`.
Raw Rev 2 `tube_angle` data for Level 1 increments one of 16 angle sectors per
adjacent lane. The ROM continues that source-defined angular state machine,
but a complete gameplay-pass lane-transition cadence and real-time ratio have
not been established here. Current C# latches the pursuit direction and does
not use the ROM's other-rim-enemy direction-selection branch. ROM direction is
selected at arrival and has that branch.

The 2.5 calibration materially shortens the C# pivot relative to its former
8-degree rate: Level 1's approximate adjacent turn changes from 20 updates to
8, while the 24-tick pause remains. It is therefore a plausible pressure
amplifier, but neither a clean explanation nor a literal arcade mismatch has
been proven. Neal manually accepted it across Levels 1-7 to correct a prior
stationary-defense weakness; that acceptance is important but not dispositive
for his current whole-wave observation.

## 14. Collision timing and tolerances

C# resolves player shots before enemy updates and consumes direct rim-arrival
or completed-pivot player contact after enemy updates. C# Flippers signal
lethal contact only on arrival in the occupied lane or pivot completion into
it; a player can leave before that moment. C# permits a player shot to hit
either pivot lane during a pivot with a strict positive separation below 16
height units.

ROM `state_04` moves enemies before moving-shot hit processing. A completed
enemy contact/death can therefore precede an already-moving shot hit; a new
shot's earlier birth check is a separate exception. ROM hit tolerance and
pivot shot-lane predicates use its enemy type/state and shot-time segment
fields; they are not established as the C# two-lane rule. The Level 1 ROM
hit tolerance is 7 in the prior source trace, versus C#'s strict 1-15-unit
Flipper window, but no validated scale conversion exists.

**Assessment:** C# same-tick shot ordering and broad pivot hit eligibility
generally favor the player. They are unlikely explanations for C# being
harder. Do not revise them based solely on static source differences.

## 15. Death and re-entry

C# freezes active combat during a 60-update death presentation, clears shots
and enemy bullets, then rebuilds unresolved enemies as fresh Eggs. Levels 1-2
release up to four Eggs on reconstruction; ledger-backed Levels preserve
accepted identity/species decisions except where Level 7/8 use ROM-shaped
generic re-entry. Super Zapper charges persist through an ordinary life loss.
There is no established extra post-respawn invulnerability window in the
inspected C# flow.

The ROM death path converts active-below, at-rim, and pending counts into a
generic pending quantity, clears active occupancy, and rebuilds generic
entries. It does not retain species or cargo identity. The ROM's active cap
continues to constrain re-entry. Both systems discard enemy shots during
death/reconstruction, but the timing and internal reconstruction are
different.

Releasing several C# Eggs after death can create a dense re-entry phase, and
Levels 3-6 can retain species decisions. This is a **moderate interaction**,
not the leading explanation for initial Level 1 play. It can amplify the same
active-cap/release difference after a lost life. No new post-death full-wave
measurement was run.

## 16. Super Zapper

C# starts each fresh level with two charges, preserves remaining charges
through life reconstruction, schedules delayed enemy zap resolutions, and
targets active Enemy instances. Eggs/reserve are not zap targets. The C#
active Egg-release/hatch phases continue while combat ticks advance. The ROM
has two per-level uses and `zap_running` blocks ordinary enemy creation while
the zap is active; its creation/pending and zap phases are not the same C#
policy.

This can leave newly hatched C# threats after a zap, especially when Eggs are
already approaching, and is a plausible **situational** pressure contributor.
Its impact depends on use timing; it cannot explain a run in which no zap is
used. No C# versus arcade zap sequence was newly measured. It is not selected
as the first experiment because the active-cap discrepancy affects ordinary
play continuously.

## 17. Randomness and clustering

Level 1/2 lane and Egg-direction sequences are deterministic project
sequences; enemy bullets use random thresholds. Levels 3-8 add seeded
random species, lane/direction, and cargo decisions; a recording preserves
startup seed and input and can reproduce the C# run. The ROM uses POKEY
randomness and delayed pending entries; same-seed equivalence is not
available.

The 16-seed Level 3/4 schedule probe forced all Flippers and did not test
realistic species clustering or combat. It only showed that accepted C# Egg
scheduling can fill active slots well beyond six. No evidence supports
judging the campaign from a single seed, and no arcade randomness clustering
was measured.

## 18. Global cadence and `--speed`

C# sets a 60-FPS host target and calls one fixed-step simulation update per
loop; there is no elapsed-time accumulator, catch-up, or update skipping. If
the host misses its target, simulation slows in wall-clock time. Default
`--speed` is 1.0; it affects wall-clock pacing, not per-tick constants.

The ROM waits for nine interrupt-counter increments per game pass. The
interrupt/NMI frequency and actual game-pass rate are not proven by the
project-local listing. Therefore 60 Hz is a deliberate project model, not an
established arcade equivalence. A global cadence mismatch remains **UNKNOWN**
and cannot be ruled out absolutely; there is no evidence it makes C# run
faster than Neal's machine. Prior qualitative observation that overall timing
feels solid weighs against changing it without a measured timing comparison.

## 19. Spinner sensitivity

Default `1.0` and Neal's reported useful `0.8` are input calibration values,
not enemy balance. Only live horizontal spinner/mouse delta is scaled; keys,
controller input, enemy pressure, and recorded playback semantics are not
rescaled. Keep sensitivity fixed at `1.0` for the controlled first experiment
so physical control does not confound the active-density question.

## 20. Level 1-8 difficulty profile

Base supplies and species/caps below are ROM listing data and accepted C#
policy; timings are not asserted equivalent.

| Level | Supply (ROM base / accepted C#) | ROM eligible pressure and concurrency | Accepted C# difference most relevant to survival | Confidence |
|---:|---|---|---|---|
| 1 | 10 / 10 | Flippers only; max 4 Flippers; max 6 total active. | All-Flipper wave; no active cap; first Flippers can fire; C# can reach 10 active in the bounded no-kill schedule. 2.5 rim calibration applies. | High for cap difference; medium for whole-game effect. |
| 2 | 12 / 12 | Flippers only; max 4 Flippers; max 6 total active. | No active cap; can reach 12 active in the bounded no-kill schedule; below-rim flipping adds route pressure. | High for cap difference; medium for route effect. |
| 3 | 15 / 15 (Hard 17) | Max 6 total; Flippers max 4; Tanker max 1; no Spikers. | No six-total cap; C# permits up to 6 Tankers, and randomized mixed wave/cargo changes simultaneous threats. | High for source mismatch; medium for realized pressure. |
| 4 | 17 / 17 (Hard 19) | Max 6 total; Flippers max 4; Tanker ordinary max 0; Spiker max 2. | No six-total cap; C# ordinary Tankers and up to 4 Spikers are possible; starting Spikes and exit add hazards. | High for mismatch; medium for survival impact. |
| 5 | 20 / 20 (Hard 23) | Max 6 total; no ordinary Flippers; Tanker max 1; Spiker max 3. | Accepted no-direct-Flipper rule matches this key restriction, but C# lacks six-total gate and has looser carrier/Spiker selection. | High for cap mismatch. |
| 6 | 22 / 22 (Hard 25) | Max 6 total; no ordinary Flippers; Tanker max 2; Spiker max 4. | C# retains ordinary Flipper eligibility, lacks six-total cap, and permits looser Tanker selection. | High for source mismatch. |
| 7 | 20 / 20 (Hard 23) | Max 6 total; no ordinary Flippers; Tanker max 2; Spiker max 4. | Dedicated ROM-shaped composition/re-entry is accepted; 50-tick batch release and 2.5 rim calibration remain project behavior. | High on policy; medium on timing. |
| 8 | 24 / 24 | Rev 2 row 7, open tube; local Rev 2 evidence supports 24 base supply and ROM-shaped six-active/type limits. | Accepted geometry/open endpoints and wave policies; Egg cadence, Spike conversion, and 60-Hz timing are project translations. | High on accepted source geometry/policy; medium on gameplay timing. |

Level 8's open endpoints change rim pursuit opportunities and exit/spike
context; no evidence here makes that the principal low-level cause.

## 21. Ranked harder-than-arcade candidates

| Rank / category | Exact C# behavior and ROM comparison | Why it could make C# harder / affected levels | Evidence, interaction, experimentability |
|---|---|---|---|
| 1 — **CRITICAL / STRONG CANDIDATE**: active enemy cap and composition | C# L1/2 can activate all supply; ROM caps L1/2 at 4 Flippers and 6 total. C# L3-6 lack the six-total gate and several ROM species maxima. | More simultaneous attackers, rim pursuers, and potential shot sources despite matching base supply. Strongest on L1-4; continues on L5-6. | Direct ROM tables + current code + bounded schedule measurement. Interacts with release pace and projectile opportunities. Isolatable with a diagnostic L1 cap. |
| 2 — **STRONG CANDIDATE**: Egg release/hatch cadence | C# starts four Eggs and uses 50-tick releases; ledger waves can release six at once. ROM maintains delayed generic pending entries and only creates below its active/type limits. | Can synchronize threats and continue feeding the field in bursts; all levels, strongest L1-6 where caps differ. | Exact C# scheduling measured, ROM rule source-backed; actual arcade timing and full-wave outcome unknown. Keep pacing unchanged in the first cap experiment. |
| 3 — **MODERATE CANDIDATE**: accepted 2.5 rim-turn calibration | C# applies 2.5 to configured rate; L1 effectively uses 20 degrees/update, 24 pause, ~8 turn updates per adjacent lane. ROM uses sector-angle progression and top program. | Shorter pivot duration and cumulative pursuit can increase contact pressure after enemies reach rim; L1-8, especially L1-6. | C# measured; ROM raw angles confirmed but full comparable cadence/wall time unresolved. A separate experiment is possible, but do not combine it with the first. |
| 4 — **MODERATE, SITUATIONAL**: post-death reconstruction | C# restarts unresolved enemies as Eggs, with multiple starting Eggs and some retained species; ROM reconstructs generic pending count behind its active cap. | Can create a clustered retry phase after a death; L1-6. | Source and C# paths established, no matched survival measurement. It interacts with the active cap. |
| 5 — **WEAK / SITUATIONAL**: enemies emerging during Super Zapper | C# does not zap Eggs/reserve and continues Egg updates while the ROM gates ordinary creation during `zap_running`. | Can blunt a zap used during a queued-wave surge; any level with queued enemies. Not explanatory if zap unused. | Code/source-level discrepancy only; depends on activation timing. |
| 6 — **WEAK / UNKNOWN**: enemy-projectile collision or wall-clock rate | Per-level shot capacity, Flipper holdoff, and random thresholds largely match the ROM at L1-7; C# hit band/speed and clock conversion remain unresolved. Excess enemy density increases opportunities. | Could add aggregate pressure with extra enemies, but no evidence of an individually faster/higher-cap Level 1 projectile system. | Static comparison; no matched bullet-rate experiment. |

## 22. Causes ruled down

- **Wrong base supply:** ruled down for Levels 1-7 at ROM base/default. Values
  match; Hard totals differ conditionally, and the cabinet setting is
  unverified. Level 8's accepted base supply is 24.
- **Spinner sensitivity alone:** ruled down by Neal's report; it changes player
  control, not enemy generation or pressure.
- **Player-shot slot count alone:** ruled down as the leading cause. C# has 10
  versus ROM 8, and the prior 8-slot deterministic ablation did not change
  survival in its four-pursuer scenario.
- **Shot/contact ordering:** ruled down as a harder-than-arcade cause. Current
  C# ordering is player-favorable on the documented final-pivot race.
- **Broad pivot-lane shot eligibility:** likely player-favorable; not a
  demonstrated source of excess C# danger.
- **Simple “C# fires Flipper bullets too often”:** ruled down as an individual
  rule mismatch at Level 1. The accepted holdoff, probability table, and
  global capacity follow ROM values; aggregate fire opportunities can still
  rise with active enemy count.
- **Global cadence as a demonstrated cause:** ruled down as a present
  conclusion, not as a possible unknown. ROM time conversion is unresolved and
  no faster C# wall-clock cadence was measured.
- **Rim speed as sole explanation:** not supported. It remains plausible as
  an interaction but was manually accepted and lacks a full ROM cadence
  comparison.

## 23. Exact unknowns

1. Neal's cabinet's physical difficulty setting and whether it is Medium.
2. ROM normal gameplay-pass frequency and its relation to C# 60-Hz ticks.
3. Whole-wave arcade active-enemy timelines, overlap, bullet counts, and
   distribution of species for each random run.
4. Exact C# versus ROM real-time Level 1/2 Egg-entry and first-rim-arrival
   intervals.
5. Exact ROM Level 1 rim-cycle ticks from the `flip_top_accel=2`, four-count
   program, despite the Rev 2 angle sectors now being confirmed.
6. Exact scale mapping for ROM enemy-shot velocity and collision tolerance.
7. The amount by which over-four C# active Flippers changes survival under
   Neal's actual play style, independently of rim speed and bullets.
8. Which lower level Neal most notices in recent natural play and whether the
   difference is before or after a life loss/Super Zapper use.

## 24. Primary controlled experiment

Run one development-only A/B experiment: **Level 1 with accepted production
behavior versus Level 1 with only a four-concurrent-Flipper creation cap**.

The diagnostic should preserve the accepted production default, ten-original
supply, four starting Eggs, 50-tick release/approach policy, enemy movement,
shot behavior, 2.5 rim calibration, player input, and scoring. When four
Flippers are active, ready ordinary Flipper Eggs wait instead of hatching;
they must not be discarded or counted resolved. Releasing Eggs remains
unchanged. This isolates concurrent Flipper cap from retuning release cadence
or total supply. The cap value is ROM-listing-backed for Level 1; the way
queued C# Eggs wait is an experimental project implementation, not asserted
as literal ROM pending behavior.

This is the single strongest reversible experiment. Do not also change rim
speed, bullet rules, collision, supply, or player firing in that build.

## 25. Proposed automated checks for that experiment

If separately authorized and implemented:

1. The no-option production Level 1 path remains byte-for-byte behaviorally
   unchanged; the diagnostic is opt-in and does not become a production
   default.
2. Under a deterministic seed and no kills, active Level 1 Flippers never
   exceed four in the diagnostic; a fifth ready Egg remains unresolved and
   later hatches when an active slot becomes available.
3. Original supply remains 10; Eggs, reserve, active, killed, and rim-cleared
   accounting conserves all 10 identities, including after death/re-entry.
4. The cap does not alter player-shot capacity/cadence, enemy-shot capacity,
   80-tick holdoff, Flipper movement, rim speed/pause, collision ordering,
   Super Zapper charge state, or Level 2 and later level defaults.
5. Record one seeded Level 1 input sequence and replay it headlessly against
   baseline and diagnostic. Compare per-boundary active Flippers, Eggs,
   bullets, rim arrivals, and contact events, not only lives/deaths.
6. Keep the existing campaign-soak test's scope: it validates orchestration,
   not combat or arcade balance. Add no test or production change during this
   investigation.

## 26. Bounded manual comparison for Neal

- **First level:** Level 1.
- **Startup:** normal natural campaign entry, which begins on accepted
  production Level 1; do not jump into a later level.
- **Lives:** 10.
- **Playback:** normal play for the cabinet comparison. For the C# A/B
  engineering comparison, also use the same seeded recording if available;
  recordings reproduce C# inputs/seed, not the arcade's random stream.
- **Spinner:** leave C# at default sensitivity `1.0` for both builds. Do not
  use `0.8` in only one arm. Keep control device and movement strategy
  consistent.
- **Observe, without relying only on death count:** count visibly active
  Flippers at once (especially after the fourth), the time/spacing of new
  hatches, how many Flippers reach the rim together, simultaneous enemy
  bullets, whether the next Flipper can appear while four are already active,
  and how often the player must leave a lane to respond to converging rim
  threats. Note any crowding during the same strategy on the arcade.
- **Supports the cap hypothesis:** diagnostic stays at four active Flippers,
  queued units wait rather than disappear, and the arcade-like spacing and
  rim-pressure feel improves while the same shots, rim timing, movement, and
  controls remain in use.
- **Refutes/weakens it:** Level 1 still feels materially harder at the
  four-Flipper cap, or repeat arcade observation shows comparable periods
  with more than four active Flippers. Then keep the cap out of production
  and investigate a separate mechanism (most plausibly rim cadence or
  release timing) in a later isolated experiment.
- **Launch command for a future Neal-run baseline:** `dotnet run --project src\Tempest.Game -- --lives 10`.
  The game was not launched in this investigation. The future diagnostic
  command should only be supplied after that experiment is authorized and
  implemented.

The normal-play arcade comparison remains **NEAL-OBSERVED ARCADE MACHINE,
BELIEVED MEDIUM BUT NOT YET HARDWARE-VERIFIED**. Do not alter the cabinet's
difficulty setting.

## 27. Final Git and local-data verification

At completion:

- `HEAD` remained `c1a974a625eea2dfaecf30d1b8d729e022ef17e8`.
- Branch remained `master`, tracking `origin/master`; `origin/master` remained
  the same commit, with zero ahead/behind.
- Index and tracked worktree remained clean; `git diff --stat`,
  `git diff --cached --stat`, and both `git diff --check` commands were empty.
- The original untracked Rev 2 directory remained the only untracked
  non-ignored repository item.
- Final recursive file counts: 21 Rev 2 chips, 50 `UserData/` files, and 17
  `Temp/GameplayRecordings/` files. Current canonical manifest SHA-256 values
  are respectively
  `548343A9DCD01D8F3E12A4137ACAD78742C37CB1EF3AA51317B6558831981CCA`,
  `6D5B00EF8F80D44977DD673EC5E84909B62145F94CBE4F4108E0E572BE0CD645`, and
  `37B6427630BBFE2DCDA1752978DD38446A88A8244E87846EC167A5B50C24ECB9`.
  Each manifest hashes UTF-8 text made from recursively enumerated files
  sorted by lowercase relative path, with each line formatted as
  `<lowercase relative path> <lowercase per-file SHA-256>`, LF-separated with
  no trailing LF. The pre-experiment manifest values were captured earlier
  but are not present in the resumed investigation context, so these final
  values cannot be claimed as a verified comparison to those baselines. No
  writes to these three locations were intended.
- The prompt archive exactly matches the read-only task attachment:
  SHA-256 `A6EF2F932CF04A2C493B28C2A5505E7AF4FBD553DAE7D756D07E4CD56D242616`.
- The only files added are the authorized ignored AI prompt/report under
  `AI-Logs/`. The temporary analysis harness was removed; it and its build
  outputs were not retained.
- No production code, tests, durable documentation, project configuration,
  ROM/reference data, recordings, videos, or user data were modified.
- No test suite or interactive game was run.

## Final recommendation

The strongest low-level explanation is **too many concurrent C# Flippers,
fed by a non-ROM Egg schedule**, with the accepted 2.5 rim calibration
potentially compounding the pressure. It is a combination, not a single
constant. Enemy projectile rules are not shown to be individually too
aggressive; density may increase aggregate opportunities. Supply totals are
not the main error at base difficulty. Player-shot and collision differences
mostly favor the C# player. Death/re-entry and Super Zapper can amplify
pressure in particular runs; global cadence and exact rim timing remain
unresolved.

Run only the isolated Level 1 four-Flipper-cap diagnostic next. Compare
simultaneous enemies, hatch spacing, rim arrivals, bullet overlap, and
required movement, keeping default spinner sensitivity and every other
accepted mechanic unchanged. Stop after the comparison; do not implement
Level 9.
