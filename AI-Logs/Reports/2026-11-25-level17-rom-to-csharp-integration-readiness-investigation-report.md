# Level 17 ROM-to-C# integration readiness investigation

**Status: READY FOR BOUNDED LEVEL 17 IMPLEMENTATION**

No production code, tests, durable documentation, ROM files, or Git state were modified.
The interactive game was not launched.

## 1. Executive summary

Level 17 can proceed to a bounded medium/default implementation. The required
ROM-backed facts, shape reuse, and architecture paths are established. The
remaining hard/easy differences and exact RGB role mapping are explicitly
bounded and do not prevent a medium/default implementation.

Readiness is supported by:

- canonical Shape 1 reuse, with no copied Level 1 point array;
- an exact ROM-backed Level 17 palette block;
- ROM-backed `$07` straight Flipper/Tanker movement;
- ROM-backed `$AF` medium Flipper/Tanker speed;
- existing C# straight-cargo and generic ledger mechanisms;
- a direct production path for Pulsars as original identities;
- existing enemy-shot cadence and 32-tick holdoff integration points;
- direct-start/progression resolvers that require only adding Level 17 to the
  existing chain.

## 2. Starting Git state

- Branch: `master`
- Tracking: `origin/master`
- HEAD: `fde09aa31af760958f656aa553555462d51792a0`
- Staged changes: none
- Pre-existing tracked modification:
  [TEMPEST-BEHAVIOR-SPEC.md](../../TEMPEST-BEHAVIOR-SPEC.md)
- Pre-existing untracked state included Level 17 prompts/reports,
  `Reference/arcade/MBUCOD.V05.txt`, and other local files. It was preserved.

## 3. Established ROM evidence accepted

The third prior report is the corrective authority. It establishes:

| Item | Accepted value | Classification |
|---|---:|---|
| Tube remap | row 0 / canonical Shape 1 | ROM-BACKED |
| Palette block | `00 07 0B 08 44 03 0C 0C` | ROM-BACKED |
| Flipper program | `$07` straight move | ROM-BACKED |
| Pulsar minimum/maximum | 2 / 5 | ROM-BACKED |
| Pulsar medium firing | off | ROM-BACKED |
| Pulsar hard firing | `$40` set | ROM-BACKED |
| Pulsar flip rate | `$28` | ROM-BACKED |
| Pulse beat | `$04` | ROM-BACKED |
| Pulsar fixed speed | `$FE.A0` | ROM-BACKED |
| Tanker cargo | `$01` = two Flippers | ROM-BACKED |
| Flipper/Tanker raw speed | `$AF` | ROM-BACKED |
| Spiker raw speed input | `$00` | ROM-BACKED, not proof of zero movement |
| Enemy-shot base input | `$C0`, plus RAM `$0160` | ROM-BACKED base/opcode; modifier unresolved |
| Shot holdoff | `$20` / 32 ROM ticks | ROM-BACKED |
| Enemy-shot maximum | `$02` last index; 3 medium slots | ROM-BACKED |
| Medium supply | 20 | ROM-BACKED |
| Hard supply | 23 | ROM-BACKED |
| Starting Spike raw height | `$A0` | ROM-BACKED raw coordinate |
| Fuseballs | absent | ROM-BACKED |

## 4. Existing C# architecture inventory

### Reusable mechanisms

- immutable level/shape records:
  [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs)
  and [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs);
- canonical tube definitions and ROM remap metadata:
  [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs);
- level policy chain:
  [LevelProgression.cs](../../src/Tempest.Game/Levels/LevelProgression.cs);
- generic original/cargo ledger:
  [Level3WaveLedger.cs](../../src/Tempest.Game/Levels/Level3WaveLedger.cs);
- ROM-style later-level wave selector pattern:
  [Level16WaveSelection.cs](../../src/Tempest.Game/Levels/Level16WaveSelection.cs);
- straight and repeating Flipper/Tanker cargo routes:
  [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs)
  and [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs);
- production Spiker, Tanker, Egg, enemy-shot, and completion systems;
- existing development Pulsar actor and pulse renderer:
  [Pulsar.cs](../../src/Tempest.Game/Gameplay/Pulsar.cs)
  and [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs);
- direct startup and presentation calibration resolvers.

### Current hard-coded policy seams

- Levels 1–16 are explicitly listed in `LevelConfiguration`;
- later levels use `UsesLevel...RomWavePolicy` flags;
- `Level16WaveSelection` owns Level 16 supply, minima, maxima, and shot counts;
- `TempestGame` contains Level 16-specific selection and Spiker-firing branches;
- `LevelProgression` ends after Level 16;
- ROM metadata/calibration stop at Level 16;
- all production ledger types stop at Flipper, Tanker, Spiker, and Fuseball.

### Development-only Pulsar seams

Pulsars exist only in `PulsarTest` and `PulsarTest` hatches every ordinary wave
unit directly as a Pulsar. This must not be confused with the production
admission model.

The existing 181-call pulse cycle, 51-update decision threshold, 18-tick lane
pause, 14-tick rim pause, and 12-degree pulse flip are
**REFERENCE-DERIVED / PROJECT TRANSLATION**, not ROM-backed Level 17 constants.

### Missing Level 17 points

- `Level17Production` configuration;
- a Level 17 wave selector;
- `Pulsar` in `Level3OriginalType` and `Level3OriginalState`;
- production admission/hatch/ledger/firing support for Pulsars;
- 17-cycle shape resolution and palette catalog entries;
- calibration resolution for Shape 1 reuse;
- natural/direct Level 17 startup extension.

### Generalization justified now

The existing generic ROM ledger and `UsesLevel...RomWavePolicy` pattern should
be extended to a bounded sixth species (`Pulsar`) and a new Level 17 policy.
It does not justify redesigning the ledger or implementing later levels.

## 5. `$07` C# correspondence

**ROM-BACKED facts:**

- P-code `$07` entry is move-per-speed with no flip/reversal sequence;
- ordinary or Tanker-cargo Flippers can use the same route where `$07` is
  assigned.

**EXISTING ACCEPTED PROJECT BEHAVIOR:**

- `TankerCargoMovementProgram.Straight07` initializes
  `InitializeLevel8CargoRoute(..., constantFlipProgram: false)`;
- the cargo route begins at the release pivot, remains straight below the rim,
  and hands off to rim pursuit.
- ordinary Flippers can additionally initialize repeating routes, but no
  ordinary `$07` route flag currently exists.

**Classification:** reusable with bounded parameterization.

The implementation should add a new named straight-`$07` path or a general
`BelowRimFlipperProgram.Straight07` enum value rather than overload the existing
Level 8 route or a Level 16 repeating route. Tanker cargo can reuse the same
program parameter on its cargo Flippers.

The route does not itself change lanes below the rim and does not reverse. The
initial direction should remain the existing `ProgrammedFlipperInitialDirection`
value (`+1` medium project convention), while rim pursuit selects from the live
player lane independently.

Spike sensitivity is not established for `$07`; the straight route must not
inherit the Level 5 or Level 9 Spike predicates. Level 1 behavior remains
separate because it is geometry-only reuse, not gameplay reuse.

## 6. Flipper/Tanker Atari movement equation

`crack_speed` shifts the raw parameter left three places and derives a signed
8.8 MSB/LSB word.

For raw `$AF`:

1. `$AF << 3`-style fixed-point conversion yields `$FD.A?`; the bounded source
   trace gives the stored word `MSB=$FD`, `LSB=$78`;
2. therefore magnitude = `0x0278 / 0x100 = 2.53125` in the along-position
   update for the positive direction;
3. `eact_0c` adds the LSB/MSB pair each active P-code update;
4. negative direction is represented by subtracting the same pair;
5. reaching the rim or reversing at the far end is handled by the existing
   P-code/end-of-tube branches.

The exact 6502 signed interpretation is not an exact C# frame conversion:
the ROM updates a 16-bit along coordinate in active P-code calls, not C#
floating-point pixels every host frame.

## 7. Current C# Flipper/Tanker movement

- ordinary Flippers climb with `FlipperConfiguration.ClimbSpeed`;
- current production Level 16 uses climb speed `2`;
- Tankers have a separate development climb formula, normally `1.75`;
- Level 16 Tanker cargo uses repeating `$24`;
- no random climb-speed adjustment is used for the production Flipper
  routine beyond route policies;
- no existing C# ROM 8.8 helper maps `$AF` directly.

## 8. Recommended Level 17 Flipper/Tanker translation

**PROJECT TRANSLATION, bounded:**

- ordinary and cargo Flippers use straight `$07`;
- medium Flipper climb speed = `2.53125f`;
- medium Tanker speed = the same `2.53125f`;
- retain C# per-simulation-update cadence;
- keep existing rim clamp and lane-rim handoff;
- no random speed variation;
- no Level 16 `$24` or Level 1 flip behavior.

This is the smallest value derived directly from the ROM fixed-point word,
not a ratio chosen for appearance. Manual gameplay acceptance remains required.

Easy/hard speed modifiers are future difficulty facts: the local report
established raw adjustment paths but is not sufficient to claim a final C#
per-update host value. They should remain deferred until a difficulty mode is
selected.

## 9. Spiker effective ROM movement

**ROM-BACKED:** raw Spiker input `$00`.

The source calls `crack_speed`, so `$00` must not be promoted to zero C# speed.
The source also uses the Spiker P-code for spike growth/reversal and
terminal-to-Tanker conversion. The bounded listing pass did not establish the
complete C# equivalent of all ascent/retreat/eact updates.

`crack_speed($00)` produces an exact signed fixed-point result that is not a
standalone movement guarantee. Therefore direct integer-zero behavior would be
invented only if it were used to claim the Spiker as stationary.

## 10. Current C# Spiker comparison

Existing C#:

- new Spikers use a formula at or near `2.5 + level factor + random 0–2`;
- Level 7/9 use accepted level-specific overrides;
- Level 11+ has a fixed policy pattern;
- ascent adds `Speed`;
- retreat subtracts `Speed * 0.90f`;
- projectiles remain 3 units/update.

The project therefore already has a defensible shared translated Spiker range
and explicitly ensures projectiles visibly outrun firing Spikers.

## 11. Recommended Level 17 Spiker translation

**PROJECT TRANSLATION:** use the established host `3` unit/update fixed value
for production Level 17 unless a future bounded ROM/arcade result justifies
otherwise. This is consistent with the existing production test seam and keeps
Spiker projectiles faster at 3 units/update only if projectile movement remains
strictly 3 and the Spiker growth is 3 or less. Prefer a separate explicit
Level 17 Spiker `Speed=3f` while preserving the retreat rule.

This is not claimed as literal ROM `$00` movement. Randomized Spiker speed
must not remain for a medium/default Level 17 implementation; no evidence
supports randomizing this level.

## 12. Effective enemy-shot speed trace

**ROM-BACKED:** base input `$C0`, opcode `$0A`, and addition of RAM `$0160`.

The listing establishes the arithmetic path but not a resolved Level 17
`$0160` value, level/difficulty dependence, or carry output.

**EXISTING ACCEPTED PROJECT BEHAVIOR:** `EnemyBullet.Advance()` adds `3`
units per update.

## 13. Recommended C# enemy-shot translation

Use the existing shared 3-unit enemy-shot update for medium/default Level 17.
This is a **PROJECT TRANSLATION**, not a literal ROM `$C0` 8.8 value.

The existing shot behavior is defensible because:

- it is accepted and used across later production levels;
- it preserves the speed relationship over Firing Spikers;
- it has one direct host mechanism;
- it has a bounded holdoff and capacity integration.

Use 32-tick holdoff and medium capacity 3. Easy/hard remain deferred; hard
capacity should be 4 and easy 2 if difficulty support is later enabled.

## 14. Pulsar ROM/C# field comparison

| C# field/state | Level 17 assessment |
|---|---|
| Climbing/descending | Existing actor is reusable; 1-unit host climb is retained for medium/default |
| Lane pause | Existing 18 ticks is a project translation |
| Rim pause | Existing 14 ticks is a project translation |
| Flip decision | Existing strict update 51 threshold is project translation |
| Pulse flip | Existing 12 degrees is project translation |
| Pulse cycle | Existing 181-update cycle is project translation |
| Fixed speed | `$FE.A0` needs bounded host parameterization; do not claim exact 8.8 conversion |
| Flip rate | `$28` should become a configuration field |
| Pulse beat | `$04` should become a configuration field |
| Firing medium/easy | disabled |
| Firing hard | `$40` enabled |
| Hazard state | Existing pulse-clock and lane electrification can be reused |
| Rim pursuit | Existing pursuit state can be reused |
| Terminal/zap | Existing generic zap/removal/scoring is reusable |

## 15. Pulsar timing decisions

For the medium/default implementation:

- retain existing C# pulse cycle as a temporary translation;
- set configuration fields for flip rate `$28` and pulse beat `$04`;
- do not replace the timing constants until ROM/arcade evidence establishes
  exact host update semantics;
- medium Pulsar firing remains off;
- pulse hazard remains represented by the existing global pulse state.

Manual acceptance is required for perceived pulse timing and motion.

## 16. Pulsar production-admission policy

Level 17 medium Pulsar admission should:

- reserve 2 guaranteed Pulsar identities from the 20-unit supply;
- admit up to 5 active/reserve Pulsar identities;
- reserve the remaining supply for random Flipper/Tanker/Spiker selection;
- not derive Pulsars from Tanker cargo;
- use ordinary 7-slot physical capacity accounting;
- count latent/active cargo and active species consistently;
- use deterministic seeded random selection through the existing `NextRandomInclusive`.

This is a **PROJECT DECISION** implementing the ROM minima/maxima. “Minimum”
means guaranteed admitted identities in the Level 17 ledger; it does not mean
two active Pulsars at once.

## 17. Pulsar identity/ledger design

The smallest extension is:

- add `Pulsar` to `Level3OriginalType`;
- add `ActivePulsar` to `Level3OriginalState`;
- add `CommitOriginalAsPulsar`;
- validate Pulsar runtime type;
- include active Pulsars in unresolved identity sets;
- include Pulsars in all-rim represented identities;
- mark Pulsar rim-cleared/destroyed through the existing terminal transitions;
- add no separate Pulsar counter or cargo identity.

No ledger redesign is required. The existing generic names are historically
tied to Level 3 but the ledger already generalizes ROM later-level state.
Calling an enum generalization across the ledger is justified; parallel
Level 17-only ledger objects are not.

## 18. Pulsar reconstruction

After life loss:

- reserve/unhatched Pulsar originals must remain unresolved;
- active Pulsar originals reconstruct once;
- Eggs retain original identity;
- cargo remains a separate Tanker-owned identity;
- pulse clock and pause timing restart as fresh host runtime state;
- firing state must reflect medium/hard configuration;
- rim state reconstructs as a fresh below-rim or rim-safe state, not an
  invented exact ROM restoration;
- no duplicate original allocation is permitted.

This follows existing **PROJECT TRANSLATION** reconstruction principles.

## 19. Pulsar completion obligations

Pulsars must participate in the generic unresolved identity set for:

- reserve;
- Egg;
- ready-to-hatch;
- active below-rim;
- lane transition;
- active rim;
- destroyed;
- rim-cleared;
- pending reconstruction.

No Pulsar-specific completion counter should be added. Completion should
continue to require every original/cargo identity terminal and no active or
pending Egg/cargo unit.

## 20. Difficulty policy

### Medium/default

- supply 20;
- Pulsar min/max 2/5;
- Pulsar firing off;
- 3 enemy-shot slots;
- shot holdoff 32;
- `$AF` medium Flipper/Tanker input;
- straight `$07`;
- two-Flipper Tanker cargo;
- no Fuseballs.

### Easy

The ROM source table does not establish a separate easy supply or Pulsar
maximum/minimum adjustment. It yields 2 enemy-shot slots and clear Pulsar
firing unless contrary source evidence appears. Level 17 easy implementation
is therefore deferred.

### Hard

- supply 23;
- 4 enemy-shot slots;
- Pulsar firing `$40`;
- raw Flipper/Tanker and pulse values require host-translation validation;
- no C# difficulty-selection feature is required for the initial
  implementation.

The project has no production difficulty model for this exact data. The
medium/default configuration is implementable now; easy/hard data is for
future parameterized support and tests.

## 21. Palette runtime-role trace

`setcolours` splits each selected byte:

- low nibble -> `$0019,y` and `col_ram,y`;
- high nibble -> `$0021,y` and `$0808,y`.

The disassembly establishes array layout and nibble splitting but does not
name the eight semantic entity roles. No ROM-backed entity-role table was
found in the bounded trace.

Current C# consumes fixed role constants:

- tube rim/rail;
- Blaster lane;
- Spiker green;
- Tanker magenta;
- Pulsar cyan;
- red;
- blue;
- white/yellow HUD and effects.

## 22. First-cycle/current-C# reconciliation

The first-cycle block is:

`00 04 08 0C C3 07 0B 0B`

The low-nibble array is `[0,4,8,12,3,7,11,11]`; high-nibble array is
`[0,0,0,0,12,0,0,0]`.

This does not justify claiming exact ROM entity RGBs. The defensible C#
reconciliation is role-based:

- preserve existing palette 0 fixed RGB constants;
- keep entity-specific constants unless a bounded ROM role consumer proves
  otherwise;
- separate `PaletteIndex` from shape and gameplay.

## 23. Proposed second-cycle host palette

Add a palette catalog entry for cycle 17–32 with raw block:

`00 07 0B 08 44 03 0C 0C`

The initial host translation should remain the existing Level 1/current C#
role colors. Add the raw nibbles to the catalog and expose them by palette
index, but do not reinterpret each nibble as an entity RGB.

The Level 17 palette index can be `1`, with `PaletteIndex:1`:

- tube web/blaster fixed-role assignments remain traceable;
- Pulsar cyan and pulse-white behavior remain intact;
- Super Zapper's rail cycling remains index 0–6 and does not depend on shape;
- exact host RGB remains subject to manual visual acceptance.

This is the smallest non-arbitrary initial mapping:
**ROM-BACKED raw cycle + PROJECT TRANSLATION of existing host role colors**.

## 24. Shape-calibration inheritance

Level 17:

- reuses `LevelLoader.LoadLevel1Rectangle()` or the same canonical Level 1
  definition object;
- resolves shape row 0 through a modulo-16 ROM remap;
- inherits Shape 1 scale/topology/model-frame mapping;
- should initially inherit Level 1 accepted Y gain/scale/offset/depth-Y gain;
- must not inherit Level 1 gameplay policy or palette.

The current calibrations are displayed-level keyed. A bounded shape-based
resolver is justified:

- resolve shape row first;
- fall back to Level 1 calibration for Level 17 until manual Level 17 tuning;
- CLI overrides remain absolute overrides.

Manual visual acceptance is required.

## 25. Natural progression requirements

Add `Level16Production -> Level17Production` in `LevelProgression`.
Level 17 initialization must preserve score/lives and recharge/reset Super
Zapper under existing level initialization rules. It must install palette
cycle 17–32, shape row 0, 20-unit medium supply, and terminal exit behavior.

## 26. Direct-start requirements

Both `--start-level 17` and `--level 17` should resolve through
`GetProductionLevel(17)` and use the same `Level17Production` instance.
Add ROM metadata/calibration coverage for Level 17, while keeping direct
startup and natural startup on the same configuration.

## 27. Complete implementation specification

| Field | Medium/default behavior | Source | C# mechanism | Manual acceptance |
|---|---|---|---|---|
| Shape | canonical row 0 / Level 1 geometry | ROM-BACKED | shared Level 1 definition; modulo-16 remap | yes |
| Displayed level | 17 | ROM-BACKED | `LevelConfiguration.Level17Production` | HUD only |
| Palette | second 8-byte cycle | ROM-BACKED/raw, project RGB | palette catalog/index 1 | yes |
| Supply | 20 | ROM-BACKED | `EnemySupply=20` | no |
| Pulsars | guaranteed 2, maximum 5 | ROM-BACKED | ledger admission policy | timing/visual yes |
| Pulsar firing | off | ROM-BACKED | medium fire gate | no/visual |
| Pulsar flip rate | `$28` | ROM-BACKED | Pulsar configuration field | yes |
| Pulse beat | `$04` | ROM-BACKED | Pulsar configuration field | yes |
| Pulsar fixed speed | `$FE.A0` | ROM-BACKED | bounded parameter later, existing host climb now | yes |
| Flipper move | `$07` | ROM-BACKED | named straight `BelowRimFlipperProgram` | yes |
| Flipper speed | raw `$AF` -> 2.53125 project update | ROM-BACKED conversion/project | fixed climb 2.53125 | yes |
| Tanker speed | copy cracked Flipper speed | ROM-BACKED/project | tanker climb 2.53125 | yes |
| Tanker cargo | two Flippers | ROM-BACKED | existing cargo pair/Straight07 | yes |
| Spiker | fixed 3 C# units; raw `$00` unresolved | project translation | `Spiker(speed:3)` | yes |
| Shot holdoff | 32 ticks | ROM-BACKED/project ticks | 32 | yes |
| Shot slots | 3 | ROM-BACKED last index 2 +1 | capacity 3 | yes |
| Enemy shots | existing 3 units/update | existing accepted/project | `EnemyBullet.Advance` | yes |
| Fuseballs | absent | ROM-BACKED | selector excludes | no |
| Spike | raw `$A0` -> 142.857 C# translation | ROM-BACKED/project | existing conversion formula | yes |
| Hard supply | 23 | ROM-BACKED | future difficulty parameter | no |
| Hard Pulsar firing | `$40` | ROM-BACKED | future difficulty gate | no |
| Hard shots | 4 | ROM-BACKED | future difficulty parameter | no |
| Easy shots | 2 | ROM-BACKED | future difficulty parameter | no |
| Terminal | Level 17 terminal for milestone | PROJECT DECISION | progression chain ends | no |

## 28. Recommended test matrix

Permanent implementation coverage should include:

- Shape 1 object identity/shared definition for Level 17;
- no copied Level 17 point array;
- exact raw palette block and nibble arrays;
- palette 0 unchanged and palette 1 on Level 17;
- Super Zapper base and flash color behavior;
- Level 17 straight `$07` ordinary and cargo routes;
- no lane change/reversal below rim;
- `$AF` medium translated Flipper/Tanker value;
- Tanker cargo release to two Flippers;
- 20-unit medium supply;
- Pulsar guaranteed 2/maximum 5 admission;
- no Pulsar cargo;
- Pulsar ledger reserve/Egg/active/destroyed/rim-cleared/reconstruction;
- Level 17 completion with Pulsars unresolved;
- Spiker fixed 3-unit translation and faster enemy-shot relationship;
- 32-tick holdoff and 3-slot medium capacity;
- hard/easy encoded as future constants when difficulty support exists;
- natural 16→17 and direct `--start-level 17`/`--level 17`;
- terminal Level 17;
- focused Level 1–16 regression;
- complete existing suite, Release build, and whitespace checks.

The existing local Level 1–16 recording remains a manual playback boundary,
not a committed fixture.

## 29. Manual-acceptance questions

Neal should accept or reject:

- Shape 1 calibration inherited for Level 17;
- Level 17 medium host movement and straight `$07`;
- Tanker cargo Flipper movement;
- Spiker and enemy-shot perceived relative speed;
- Pulsar fixed speed, pulse timing, hazard state and rim behavior;
- second-cycle palette and Super Zapper interaction;
- Level 17 HUD, approach and terminal presentation.

## 30. Deferred whole-game issues

Deferred without implementation:

- Level 18+;
- exact easy/hard Pulsar speed and pulse differences;
- exact `$0160` enemy-shot modifier;
- exact easy supply adjustment;
- exact host RGB of every ROM palette nibble;
- full ROM-level 6502 tick-to-host cadence equivalence;
- global difficulty-selection architecture.

## 31. Readiness result

## READY FOR BOUNDED LEVEL 17 IMPLEMENTATION

The medium/default implementation has a bounded translation for movement,
admission, firing, identity, completion, palette, and startup.

## 32. Remaining blockers

None for a medium/default implementation. Hard/easy and exact host RGB remain
explicit manual/parameterization boundaries, not blockers to initial
implementation.

## 33. Files/resources inspected

- prior Level 17 prompts/reports under [AI-Logs/](../../AI-Logs/);
- [Tempest Rom Source code.md](../../Reference/arcade/Tempest%20Rom%20Source%20code.md);
- [LevelConfiguration.cs](../../src/Tempest.Game/Levels/LevelConfiguration.cs);
- [LevelLoader.cs](../../src/Tempest.Game/Levels/LevelLoader.cs);
- [LevelProgression.cs](../../src/Tempest.Game/Levels/LevelProgression.cs);
- [RomPresentationMetadata.cs](../../src/Tempest.Game/Levels/RomPresentationMetadata.cs);
- [AtariPresentationCalibration.cs](../../src/Tempest.Game/Rendering/AtariPresentationCalibration.cs);
- [Level16WaveSelection.cs](../../src/Tempest.Game/Levels/Level16WaveSelection.cs);
- [Level3WaveLedger.cs](../../src/Tempest.Game/Levels/Level3WaveLedger.cs);
- [Flipper.cs](../../src/Tempest.Game/Gameplay/Flipper.cs);
- [Tanker.cs](../../src/Tempest.Game/Gameplay/Tanker.cs);
- [Spiker.cs](../../src/Tempest.Game/Gameplay/Spiker.cs);
- [Pulsar.cs](../../src/Tempest.Game/Gameplay/Pulsar.cs);
- [EnemyBullet.cs](../../src/Tempest.Game/Gameplay/EnemyBullet.cs);
- [TempestGame.cs](../../src/Tempest.Game/TempestGame.cs);
- [VectorRenderer.cs](../../src/Tempest.Game/Rendering/VectorRenderer.cs);
- [SuperZapperSystem.cs](../../src/Tempest.Game/Systems/SuperZapperSystem.cs);
- relevant tests for later-level admission, presentation, ledger and playback.

No screenshots, videos or private scratch files were inspected.

## 34. Commands/calculations run

- read-only `git status --short --branch`;
- read-only `git rev-parse HEAD` and upstream tracking verification;
- focused source/disassembly searches and views;
- fixed-point calculation for `$AF -> $FD.78 -> 2.53125`;
- shot capacity and holdoff calculation;
- no full build/test suite and no full ROM-byte enumeration.

## 35. Production code changed

No.

## 36. Tests changed

No.

## 37. Durable docs changed

No.

## 38. Stage/commit/push

None.

## 39. Level 18+ investigation/implementation

None.

## 40. Recommended next step

Implement a bounded Level 17 production milestone in one future authorized
change set, starting with configuration/remap/palette, straight `$07`,
Pulsar original-ledger admission, medium enemy-shot policy, and shared startup
coverage. Do not add Level 18 or a full difficulty system in that milestone.
