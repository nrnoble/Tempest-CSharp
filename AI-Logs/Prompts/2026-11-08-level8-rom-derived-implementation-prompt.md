FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Implement a bounded production Level 8 candidate using the Level 8
specification derived independently from the actual local Atari Tempest Rev 2
ROM binaries.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

Confirm the actual Git state before modifying anything.

Do not stage, commit, or push.

Leave the complete candidate unstaged for Neal's manual arcade-fidelity
acceptance.


REQUIRED INVESTIGATION REPORTS

Read these reports fully before designing or modifying production code:

AI-Logs/Reports/2026-11-08-level8-rom-preimplementation-investigation-report.md

AI-Logs/Reports/2026-11-08-level8-rev2-rom-geometry-continuation-report.md

The Rev 2 continuation report resolves the geometry blocker from the first
report.

Use the continuation report where the actual binary ROM data resolves an
earlier unknown.

Do not repeat the complete ROM investigation.

Recheck particular local ROM bytes only when required to verify an
implementation detail.


LOCAL ROM AUTHORITIES

Annotated ROM listing:

Reference/arcade/Tempest Rom Source code.md

Raw Rev 2 ROM binaries:

Reference/arcade/roms.rev2/

The ROM binaries are read-only reference inputs.

Do not:

- edit them;
- rename them;
- move them;
- patch them;
- rewrite them;
- create modified copies alongside them;
- stage them;
- commit them.

Do not search the internet for another Tempest disassembly.

Do not use Mark Incitti's implementation, screenshots, MAME, gameplay video,
online tube charts, another Tempest port, or prior visual knowledge to
determine what Level 8 should look like.


BLIND IMPLEMENTATION EXPERIMENT

Neal intentionally has not supplied the expected visual appearance of Level 8.

Implement Level 8 using only:

- the recovered Rev 2 ROM data;
- the two Level 8 investigation reports;
- the existing accepted C# architecture.

Do not visually cross-check the implementation against another source.

Do not:

- launch MAME;
- inspect a Level 8 screenshot;
- inspect Level 8 gameplay footage;
- inspect another Level 8 implementation;
- ask Neal what the tube is supposed to look like.

VS Code Copilot must not launch the interactive game.

Neal's first launch will be the visual test of whether the independent
ROM-derived reconstruction produced the correct arcade tube.


IMPLEMENTATION SCOPE

Implement production Level 8 with:

1. ROM-derived 16-vertex geometry.
2. Open topology with exactly 15 playable lanes.
3. Correct general open-endpoint behavior.
4. Medium/default original supply of 24.
5. Six-active ordinary creation gate.
6. Tanker and Spiker ordinary selection only.
7. No ordinary bottom-entry Flippers.
8. No Pulsars.
9. No Fuseballs/Fuzzballs.
10. Two Flipper cargo children per Tanker.
11. ROM-derived Tanker-cargo `$07` versus `$19` movement selection.
12. Level 8 `$19` Flipper movement program.
13. ROM-derived/provisionally translated starting Spikes.
14. Level 7-style generic death/re-entry policy.
15. Natural Level 7 to Level 8 progression.
16. Generic direct startup through `--level 8`.
17. Existing recording/playback/headless/scrubbing/screenshot support.
18. Existing `--speed` diagnostic compatibility.
19. Clean terminal Level 8 boundary.
20. No Level 9 implementation.


ROM-DERIVED LEVEL 8 GEOMETRY

The Rev 2 binary investigation establishes:

- displayed Level 8 has zero-based level index 7;
- effective parameter-table level is `$08`;
- `lev_remap[7] = $07`;
- Level 8 selects tube/shape row 7;
- `lev_open[7] = $FF`;
- shape row 7 is open;
- it contains 16 ordered vertices;
- it contains exactly 15 playable consecutive lanes;
- no lane exists between vertex 15 and vertex 0.

The recovered raw ROM coordinates for shape row 7 are:

Index  Raw X  Raw Y

0      $10    $F0
1      $1E    $D0
2      $2C    $B0
3      $3A    $90
4      $48    $70
5      $56    $50
6      $64    $30
7      $70    $10
8      $90    $10
9      $9E    $30
10     $AC    $50
11     $BA    $70
12     $C8    $90
13     $D6    $B0
14     $E4    $D0
15     $F0    $F0

Subtracting `$80` from both axes gives:

Index  X       Y

0      -112    112
1       -98     80
2       -84     48
3       -70     16
4       -56    -16
5       -42    -48
6       -28    -80
7       -16   -112
8        16   -112
9        30    -80
10       44    -48
11       58    -16
12       72     16
13       86     48
14      100     80
15      112    112

Recovered ROM angle entries are:

0D 0D 0D 0D 0D 0D 0D 00 03 03 03 03 03 03 03 00

Preserve the ROM point order exactly.


C# GEOMETRY TRANSLATION

Use the bounded project translation proposed by the completed ROM
investigation:

scale = 200 / 112
      = 25 / 14

xCSharp = (rawX - 128) * 25 / 14

yCSharp = (rawY - 128) * 25 / 14

Equivalent normalized points are approximately:

0   (-200.0,      200.0)
1   (-175.0,      142.8571)
2   (-150.0,       85.7143)
3   (-125.0,       28.5714)
4   (-100.0,      -28.5714)
5   ( -75.0,      -85.7143)
6   ( -50.0,     -142.8571)
7   ( -28.5714,  -200.0)
8   (  28.5714,  -200.0)
9   (  53.5714,  -142.8571)
10  (  78.5714,   -85.7143)
11  ( 103.5714,   -28.5714)
12  ( 128.5714,    28.5714)
13  ( 153.5714,    85.7143)
14  ( 178.5714,   142.8571)
15  ( 200.0,      200.0)

Use sufficient numerical precision in production data.

Do not round more aggressively merely for readability.

The normalized C# coordinates are a PROJECT TRANSLATION.

The raw point coordinates, point order, shape selector, and open topology are
REV-2-BINARY-CONFIRMED.

Do not visually tune these coordinates in this prompt.


LEVEL DEFINITION

Create a dedicated production Level 8 definition using the existing level
geometry architecture where practical.

Requirements:

- 16 vertices;
- open topology;
- exactly 15 playable lanes;
- playable lane `i` connects vertex `i` to vertex `i + 1` for lanes 0..14;
- no lane 15;
- no vertex-15-to-vertex-0 connection;
- no false closing edge;
- no modulo wrap between lane 14 and lane 0.

Use the existing Continuous/open-topology representation if appropriate.

Preserve the ROM vertex order.


OPEN-TUBE GENERAL ARCHITECTURE

The completed investigation found that the C# configuration model can represent
an open tube, but several existing general helpers assume closed wraparound.

Implement the smallest maintainable general open-topology support required by
Level 8.

Do not implement scattered Level-8-only exceptions when behavior is
fundamentally topology-dependent.

At the same time, do not broadly redesign the geometry system.

Accepted closed tubes in Levels 1-7 must retain their exact existing behavior.


VALID-LANE MODEL

For Level 8:

Valid playable lanes:

0 through 14

Invalid/nonexistent playable lane:

15

No production object or representation may treat lane 15 as playable.

This applies as relevant to:

- Player;
- Eggs;
- Flippers;
- Tankers;
- Tanker cargo;
- Spikers;
- Spikes;
- shots or collision state that references lanes;
- rendering;
- pending transitions.

Create a clear topology-aware mechanism for:

- PlayableLaneCount;
- lane validity;
- requesting a neighbor when one exists;
- detecting an open endpoint;
- reversing at an endpoint where ROM behavior requires it.

Do not silently normalize an invalid Level 8 lane modulo 16.


BLASTER OPEN-ENDPOINT MOVEMENT

The ROM clamps player movement at open endpoints rather than wrapping.

For Level 8:

- the Blaster can traverse lanes 0 through 14;
- movement beyond one open endpoint stops/clamps;
- movement beyond the other open endpoint stops/clamps;
- lane 14 cannot transition directly to lane 0;
- lane 0 cannot transition directly to lane 14;
- lane 15 is never occupied;
- spinner direction remains consistent with the level's point ordering.

Preserve existing wraparound Blaster movement for accepted closed Levels 1-7.


INITIAL BLASTER LANE

The ROM geometry investigation did not establish a C# equivalent initial lane
for the Level 8 transition.

Choose the smallest sensible project translation that fits the existing
startup and progression architecture.

Classify it as:

PROJECT TRANSLATION / MANUAL ACCEPTANCE PENDING

Do not distort geometry or topology to compensate for the starting-lane choice.


OPEN-ENDPOINT ENEMY BEHAVIOR

The ROM reverses appropriate enemy movement direction at open endpoints.

For Level 8:

- enemies must not enter lane 15;
- enemies must not wrap from lane 14 to lane 0;
- enemies must not wrap from lane 0 to lane 14;
- Flippers reverse direction or select the valid inward neighbor when an
  endpoint is reached;
- cargo behavior remains valid at both endpoints;
- no fictitious pivot occurs across the open gap.

Preserve accepted closed-level behavior.


TANKER CARGO ENDPOINT CORRECTION

The ROM contains explicit correction for cargo adjacency at an open endpoint.

Implement the equivalent topology-safe behavior.

A Tanker still owns/releases two cargo Flipper identities.

At or near open endpoints:

- both cargo children must be assigned valid playable lanes;
- no child enters lane 15;
- no child wraps across the open gap;
- preserve opposite-direction intent as closely as the valid topology permits;
- do not duplicate cargo identity;
- do not lose a cargo obligation.


OPEN-TUBE RENDERING

Update tube rendering generically for open definitions.

For Level 8:

- draw exactly 15 near-rim lane edges;
- draw exactly 15 far-rim lane edges;
- preserve appropriate depth connections for the 16 vertices;
- do NOT draw vertex 15 to vertex 0;
- do NOT display a false closing side.

Existing closed tubes must continue drawing their closing connection exactly as
before.


DIRECTED TRANSITIONS

Use the recovered ROM point ordering and existing directed-pivot geometry.

Do not add a transition between the two open endpoints.

The major geometric direction changes occur in the interior shape.

Small coordinate-rounding turns that do not correspond to a change in the
ROM's quantized angle sector should not be exaggerated into new visible
corners solely because floating-point geometry detects a few degrees.

Preserve the accepted principle that directed rotation may require the intended
directed path rather than the mathematically shortest path.

Endpoint handling is not a pivot through the open gap.


ROM PRESENTATION DATA

Recovered shape-7 values include:

lev_scale[7]  = $18
lev_y3d[7]    = $B0
lev_y2d[7]    = $0060
lev_open[7]   = $FF
Flipper scale = $0240
Tube color index = $05

Recovered Level 1-16 color-block bytes include:

00 04 08 0C C3 07 0B 0B

These are ROM values.

They do not establish literal C# pixels or RGB colors.

For this first Level 8 candidate:

- preserve/document these ROM values;
- use the bounded normalized geometry;
- choose the smallest reasonable project translation for tube placement and
  color within the existing renderer;
- do not claim the chosen RGB values are literal Atari values;
- do not use screenshots/MAME to tune them.

Classify:

- ROM indices/data as REV-2-BINARY-CONFIRMED;
- C# placement/color choices as PROJECT TRANSLATION / MANUAL ACCEPTANCE
  PENDING.


LEVEL 8 SUPPLY

Use Medium/default Level 8 supply:

24 original/generic pending wave units.

Do not use Hard's 27-unit supply.

Do not add difficulty selection.

Preserve the project's distinctions among:

- original/generic supply;
- Tanker carrier identity;
- latent cargo;
- released cargo;
- active enemies;
- Spikes;
- enemy projectiles.

Latent Tanker cargo does not count as additional original pending supply.


LEVEL 8 ACTIVE/TYPE LIMITS

Use the Rev-2-confirmed Medium/default values:

Global ordinary creation gate:

6 active units

Flipper:

min 0
max 5

Tanker:

min 1
max 2

Spiker:

min 2
max 4

Pulsar:

min 0
max 0

Fuseball/Fuzzball:

min 0
max 0


ORDINARY LEVEL 8 ENTRY

Ordinary bottom entry may select:

- Tankers;
- Spikers.

Ordinary bottom entry may NOT select:

- Flippers;
- Pulsars;
- Fuseballs/Fuzzballs.

Tanker cargo remains capable of producing Flippers.

Do not interpret the Flipper maximum of 5 as permission for ordinary
bottom-entry Flippers.


LEVEL 8 SELECTOR

Implement a bounded Level 8 selection policy consistent with the confirmed
ROM limits.

Reuse or safely generalize existing Level 7 selection architecture only where
the behavior genuinely matches.

Do not route Level 8 through Level 7-specific movement assumptions.

Preserve deterministic seeded behavior.

Do not change Level 7's accepted selector.

If exact ROM post-minimum selection cannot be translated cleanly without
expanding scope, use the narrowest deterministic project translation and
document it clearly.


TANKERS

For production Level 8:

- Tankers are eligible ordinary entrants;
- minimum policy 1;
- maximum policy 2;
- each Tanker carries two Flippers;
- one Tanker consumes one original/generic supply unit;
- latent cargo does not consume two additional generic pending units;
- cargo identities are represented consistently with existing C# ledger
  architecture;
- Tanker destruction and natural release preserve scoring, audio, ledger,
  destruction-effect, and cargo semantics.

Do not modify accepted Tanker behavior on Levels 1-7.


TANKER CARGO MOVEMENT BOUNDARY

Implement the recovered unsigned ROM along-position distinction:

ROM along < $20:

use default straight Flipper entry `$07`

ROM along >= $20:

use Level 8 Flipper program `$19`

Equality at `$20` belongs to `$19`.

Natural Tanker release occurs after movement at a qualifying ROM along value
from `$11` through `$1F`, so natural release uses `$07`.

Player-shot destruction of a Tanker uses:

- `$07` when its captured ROM-equivalent along position is below `$20`;
- `$19` when it is at or beyond `$20`.

Do not compare a C# height directly with literal integer 32 unless the
ROM-to-C# coordinate conversion establishes that comparison.

Create a small explicit conversion or boundary predicate.

Document the C# equivalent boundary.

Test ROM-equivalent positions:

$1F
$20
$21


LEVEL 8 FLIPPER PROGRAM `$19`

Implement the confirmed Level 8 program for cargo that selects `$19`.

The ROM program repeatedly performs:

1. one along-movement interval;
2. begin a lane flip;
3. continue/complete the flip;
4. repeat the cycle.

The `$19` program contains:

- no programmed direction reversal;
- no Spike-test opcode;
- no Level 7 two-flips/reverse/three-flips sequence.

Do not reuse Level 7's `$24` routing algorithm.

Do not impose the arcade-observed Level 7 2-3-2-2-2 pattern.

Level 8 deep cargo should repeatedly alternate:

move along -> flip -> move along -> flip

subject to normal transition completion and open-endpoint behavior.


LEVEL 8 NATURAL `$07` CARGO

Tanker cargo created below the `$20` boundary uses the default straight `$07`
entry.

Natural cargo must:

- climb straight below the rim;
- not execute the `$19` repeated-flipping route below the rim;
- enter existing normal top/rim behavior when appropriate.

Preserve this distinction in code and tests.


FLIPPER DIRECTION

Tanker cargo children begin with opposite directions.

Preserve this opposing directional intent.

For `$19`:

- direction normally remains stable through successive flips;
- no reversing pattern should be invented;
- direction reverses only when required by an open endpoint or by existing
  relevant top/rim behavior.

Document C# direction-bit mapping as PROJECT TRANSLATION if necessary.


SPIKERS

Use:

min 2
max 4

Reuse accepted Spiker systems where ROM-compatible:

- bottom entry;
- Spike growth;
- lane selection/hopping;
- firing;
- destruction;
- scoring;
- final-pending Tanker conversion;
- accounting.

Every Level 8 Spiker lane and Spike lane must remain in playable range 0..14.

Never create lane 15.


STARTING SPIKES

The ROM starting-Spike along value is:

$C8

Use the investigation's bounded translation:

height =
    (0xF0 - rawAlong) * 400 / (0xF0 - 0x10)

For Level 8:

(240 - 200) * 400 / 224
= 71.428571...

Use approximately 71.428571 as the Level 8 starting Spike C# height.

Preserve enough precision for deterministic tests.

Create starting Spikes on playable Level 8 lanes:

0 through 14

Do not create a gameplay Spike on lane 15.

Classify the C# height as:

PROJECT TRANSLATION CANDIDATE / MANUAL ACCEPTANCE PENDING

Do not reuse height 60 silently.

Do not use 200 directly.


SPIKE EROSION

Preserve existing accepted C# Spike erosion unless a narrow Level 8 adaptation
is required by the coordinate translation.

Do not rewrite Spike erosion for Levels 1-7.

The ROM investigation established that ROM Spike erosion is position-driven,
while existing C# behavior is a project translation.

Document that distinction.

Neal will manually judge Level 8 Spike placement and erosion.


PULSARS

Level 8 Pulsar min/max is 0/0.

Do not create Pulsars in production Level 8.

Do not alter isolated/development Pulsar support.


FUSEBALLS / FUZZBALLS

Level 8 Fuseball/Fuzzball min/max is 0/0.

Do not create Fuseballs/Fuzzballs in production Level 8.

Do not implement deferred Fuseball destruction effects.


ENEMY FIRING

Reuse the accepted source-derived firing system.

Recovered Level 8 ROM values include:

- four enemy-shot slots;
- shot holdoff 59 ROM updates.

Do not directly interpret 59 ROM updates as 59 C# ticks unless an existing
validated translation establishes that equivalence.

Preserve current source-derived C# firing translation unless the completed
Level 8 reports specify a validated Level 8-specific conversion.

Do not retune Levels 1-7.


FINAL-PENDING SPIKER CONVERSION

Preserve the ROM-backed final/no-pending behavior:

- a qualifying Spiker converts in place to a Flipper-carrying Tanker;
- it remains the same pending/original unit;
- generic supply does not increase;
- cargo ownership is allocated normally;
- open-lane validity remains enforced;
- cargo follows the same `$07`/`$19` movement rules.

Reuse the accepted Level 7 same-unit conversion architecture where appropriate.

Do not import Level 7 Flipper movement behavior.


DEATH / RE-ENTRY

Use Level 7-style generic ROM-shaped reconstruction for Level 8.

On death:

- unresolved generic quantity returns;
- unresolved species selection is cleared;
- movement-program identity is cleared;
- latent cargo attached to a lost Tanker disappears with the carrier;
- released unresolved cargo returns through generic unresolved reconstruction;
- resolved outcomes remain resolved;
- no identities or obligations are duplicated.

Do not use the Level 3-6 species-preserving re-entry policy for Level 8.

Existing ledger structures may be retained internally so long as visible and
counting behavior remains ROM-shaped.


LEVEL 7 -> LEVEL 8 PROGRESSION

Implement natural production progression from Level 7 into Level 8.

Preserve:

- score;
- lives;
- fresh Super Zapper charges;
- existing approach presentation;
- existing starfield policy;
- spinner-responsive Blaster during approach;
- fresh Level 8 wave setup.

The incoming tube must be the new ROM-derived open Level 8 tube.

Do not alter accepted Level 7 gameplay.


DIRECT LEVEL 8 STARTUP

The generic direct-level interface must support:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10

Do not add a Level-8-specific startup option when generic --level support is
sufficient.

Direct Level 8 startup must initialize the same production configuration used
by natural Level 7 -> Level 8 progression.


LEVEL 8 COMPLETION

Level 9 is not part of this milestone.

When Level 8 is completed:

- all required original/cargo obligations must resolve;
- use the existing terminal campaign boundary;
- do not load Level 9;
- do not manufacture Level 9 geometry;
- do not crash or hang;
- preserve existing restart behavior back to fresh Level 1.


SUPER ZAPPER

Give Level 8 normal fresh Super Zapper charges.

Preserve accepted first-use and second-use behavior.

Verify completion/accounting after Level 8 Super Zapper use.

Do not redesign the Super Zapper.


RECORDING / PLAYBACK / DEBUG TOOLING

Production Level 8 must remain compatible with:

- gameplay recording;
- playback v2 startup identity;
- visible playback;
- headless playback;
- pause/resume;
- single/grouped forward stepping;
- single/grouped reverse scrubbing;
- screenshot capture;
- live-control handoff;
- `--speed` diagnostic playback.

A new Level 8 recording must restore:

- production Level 8;
- requested lives;
- deterministic startup state.

Do not modify the recording schema if the existing generic level field already
supports Level 8.

Do not serialize `--speed`.


LEVELS 1-7 ARE FROZEN

Do not intentionally change accepted Levels 1-7.

Any open-topology architecture changes must be conditional on topology.

Closed-tube behavior must remain exactly as accepted.

Protect:

- wrapping;
- movement;
- pivot geometry;
- rendering;
- collision;
- Tanker cargo;
- Spikes;
- death/re-entry;
- progression;
- recording/playback.


PRESENTATION TRANSLATIONS PENDING MANUAL ACCEPTANCE

These Level 8 details remain project translations requiring Neal's manual
judgment:

- normalized tube scale;
- screen placement;
- CenterY / YOffset if applicable;
- tube color mapping;
- initial Blaster lane;
- starting Spike height translation;
- visible `$19` movement quality.

Do not claim arcade acceptance.

Do not use visual references to tune them before Neal's first launch.


REQUIRED AUTOMATED TESTS

Add focused tests for the following.


1. GEOMETRY

Verify:

- shape row 7;
- 16 expected points;
- exact ordering;
- normalized coordinate transformation;
- open topology;
- Continuous=false or equivalent;
- exactly 15 playable lanes.


2. CONNECTIVITY

Verify:

- lanes 0..14 exist;
- lane 15 does not;
- each lane connects consecutive vertices;
- no connection exists from vertex 15 to vertex 0;
- endpoint neighbor requests do not wrap.


3. CLOSED-TUBE REGRESSION

Verify accepted closed definitions retain:

- point counts;
- lane counts;
- wrapping;
- topology;
- rendering.


4. BLASTER ENDPOINTS

Verify on Level 8:

- Blaster traverses valid lanes;
- Blaster stops/clamps at both open endpoints;
- no 14-to-0 wrap;
- no 0-to-14 wrap;
- Blaster never occupies lane 15.

Verify closed levels still wrap.


5. OPEN-TUBE RENDERING

Verify:

- 15 near-rim lane connections;
- 15 far-rim lane connections;
- no false closing edge;
- normal depth edges;
- existing closed tubes still close.


6. ENEMY ENDPOINT BEHAVIOR

Verify:

- no enemy enters lane 15;
- no enemy crosses the open gap;
- Flipper endpoint behavior reverses appropriately;
- interior movement remains directed and valid.


7. TANKER CARGO ENDPOINTS

Verify:

- two cargo identities;
- valid lanes at both endpoints;
- opposite directional intent;
- no lane 15;
- no missing or duplicated obligations.


8. SUPPLY / SELECTOR

Verify:

- supply 24;
- global ordinary active gate 6;
- Tanker 1/2;
- Spiker 2/4;
- Flipper ordinary min 0;
- no bottom Flippers;
- no Pulsars;
- no Fuseballs;
- cargo Flippers remain possible.


9. `$07` / `$19` CARGO BOUNDARY

Test ROM-equivalent positions:

$1F -> $07
$20 -> $19
$21 -> $19

Verify:

- natural release -> $07;
- sufficiently deep shot destruction -> $19;
- equality -> $19.


10. LEVEL 8 `$19` PROGRAM

Verify:

- one along interval;
- one directed flip;
- repeated cycle;
- no programmed reversal;
- no Spike-test behavior;
- no Level 7 2/3 pattern.


11. NATURAL `$07` CARGO

Verify:

- straight climbing below rim;
- no `$19` flips below rim;
- normal top/rim transition.


12. STARTING SPIKES

Verify:

- approximately 71.428571 C# height;
- one starting Spike for each playable lane 0..14;
- none for lane 15;
- Spiker growth remains valid;
- player-shot erosion remains functional;
- Level 8 does not use the earlier AVOID SPIKES interstitial.


13. FINAL SPIKER CONVERSION

Verify:

- same unit becomes Tanker;
- no new generic supply;
- cargo owned correctly;
- valid open lanes maintained.


14. DEATH / RE-ENTRY

Verify:

- species cleared;
- generic unresolved quantity conserved;
- latent cargo discarded with lost carrier;
- released cargo reconstructed generically;
- resolved identities stay resolved;
- no duplicate allocations;
- repeated death/re-entry deterministic.


15. NATURAL PROGRESSION

Verify:

- Level 7 completes into Level 8;
- correct Level 8 tube loads;
- Level 8 supply initializes;
- starting Spikes initialize;
- score/lives carry;
- Super Zapper recharges;
- Level 7 remains unchanged.


16. DIRECT STARTUP

Verify:

--level 8

uses production Level 8.

Verify requested lives are honored.


17. LEVEL 8 COMPLETION

Verify:

- all required obligations resolve;
- terminal Level 8 boundary occurs;
- Level 9 does not load;
- existing restart returns to fresh Level 1.


18. SUPER ZAPPER

Verify accepted behavior and correct Level 8 accounting.


19. RECORDING / PLAYBACK

Verify:

- recording Level 8 startup;
- restoring Level 8;
- requested lives;
- deterministic playback;
- headless playback;
- pause;
- forward stepping;
- reverse reconstruction;
- screenshots;
- `--speed` visible pacing;
- unchanged recording schema.


20. REGRESSION

Run appropriate regression coverage demonstrating Levels 1-7 remain accepted
and unchanged.


VALIDATION

Run narrow affected tests throughout implementation.

Before stopping, run expanded focused regression covering:

- Level 8 geometry;
- generic topology helpers;
- open endpoints;
- player movement;
- rendering;
- lane validation;
- Flippers;
- Tankers/cargo;
- Spikers/Spikes;
- wave selection;
- active limits;
- wave ledger/accounting;
- death/re-entry;
- completion;
- Super Zapper;
- Level 7 progression;
- direct-level startup;
- recording/playback;
- headless playback;
- playback controls;
- screenshot capture;
- `--speed`;
- closed Levels 1-7.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Do not launch the interactive game.


DOCUMENTATION

Update:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Update README.md only if production campaign/direct-level documentation
genuinely requires it.

Record:

- Rev 2 derived geometry;
- open topology;
- topology architecture changes;
- Level 8 supply/type policy;
- Tanker cargo behavior;
- `$07` / `$19` boundary;
- `$19` Flipper movement;
- starting Spike translation;
- Spiker conversion;
- death/re-entry;
- natural Level 7 -> Level 8 progression;
- direct Level 8 startup;
- terminal Level 8 boundary;
- recording/playback tooling compatibility;
- project presentation translations;
- MANUAL ACCEPTANCE PENDING.

Preserve evidence categories:

RAW-ROM-DATA

ROM-LISTING-BACKED

REV-2-BINARY-CONFIRMED

EXISTING PROJECT BEHAVIOR

PROJECT TRANSLATION

MANUAL ACCEPTANCE PENDING

UNKNOWN / UNRESOLVED


ROM FILE SAFETY

Before and after implementation verify:

Reference/arcade/roms.rev2/

remains unchanged.

Do not stage the ROM directory.

Do not commit ROM files.

Do not alter ROM timestamps intentionally.

Do not place generated files beside the ROM binaries.


LOCAL FILE SAFETY

Preserve:

- UserData;
- gameplay recordings;
- Videos;
- screenshots;
- AI-Logs;
- unrelated local files.

Do not run git clean.


AI-LOG POLICY

These instructions are explicit and non-conflicting.

1. Save this complete substantive prompt under:

AI-Logs/Prompts/

2. Create a NEW implementation report under:

AI-Logs/Reports/

3. Creating the new ignored prompt/report files is authorized.

4. Existing reports are historical records and must remain unchanged.

5. Do NOT modify:

2026-11-08-level8-rom-preimplementation-investigation-report.md

or:

2026-11-08-level8-rev2-rom-geometry-continuation-report.md

6. AI-Logs/ is Git-ignored.

7. Do not stage or commit AI-Logs.

8. Do not place the implementation report under TEMP/ or Temp/.


IMPLEMENTATION REPORT

Create a descriptive date-prefixed Level 8 implementation report under:

AI-Logs/Reports/

Include:

1. Starting Git state.
2. Reports reused.
3. ROM data rechecked.
4. Geometry implemented.
5. Open-topology architecture changes.
6. Blaster endpoint behavior.
7. Enemy endpoint behavior.
8. Rendering changes.
9. Supply/selector.
10. Tanker/cargo behavior.
11. `$07` / `$19` boundary translation.
12. `$19` movement implementation.
13. Starting-Spike translation.
14. Spiker/final conversion.
15. Death/re-entry.
16. Natural progression.
17. Direct startup.
18. Terminal boundary.
19. Recording/playback/tool compatibility.
20. Project presentation translations.
21. Files changed.
22. Tests added/changed.
23. Focused validation.
24. Expanded validation.
25. Full suite result.
26. Build result.
27. `git diff --check`.
28. Proof Levels 1-7 remain unchanged.
29. Proof ROM/local files unchanged.
30. Final Git state.
31. Manual commands.
32. Manual acceptance checklist.
33. Remaining questions.


GIT / STOP POINT

Do not stage.

Do not commit.

Do not push.

Leave the complete candidate unstaged for Neal's manual acceptance.

Stop after automated validation and manual-test handoff.

Do not begin Level 9.


MANUAL TEST COMMANDS

Provide Neal these commands.


Direct Level 8:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10


Direct Level 8 at slower diagnostic speed:

dotnet run --project src\Tempest.Game -- --level 8 --lives 10 --speed 0.75


Natural campaign progression:

dotnet run --project src\Tempest.Game -- --lives 10


Fresh Level 8 recording:

dotnet run --project src\Tempest.Game -- --record-gameplay Temp\GameplayRecordings\level8-candidate.json --level 8 --lives 10


Visible playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json


Accelerated playback:

dotnet run --project src\Tempest.Game -- --playback-file Temp\GameplayRecordings\level8-candidate.json --speed 2


MANUAL ACCEPTANCE CHECKLIST

Do not visually inspect another Level 8 implementation before Neal's first
launch.


A. FIRST BLIND VISUAL CHECK

- Launch Level 8 directly.
- Observe the independently ROM-derived tube.
- Judge whether its geometry matches the arcade Level 8 tube.
- Check scale and placement.
- Check tube color.
- Record observations before requesting changes.


B. OPEN TOPOLOGY

- Confirm two open endpoints.
- Confirm Blaster stops at both endpoints.
- Confirm no wrap across the gap.
- Confirm no false closing edge.


C. BLASTER CONTROL

- Traverse the entire playable rim.
- Confirm spinner direction.
- Verify both endpoints.
- Confirm no crash or malformed location.


D. ENEMY ENTRY

Expected ordinary entries:

- Tankers;
- Spikers.

Not expected as ordinary bottom entries:

- Flippers;
- Pulsars;
- Fuseballs.


E. TANKER CARGO

- Confirm two Flipper cargo children.
- Confirm valid cargo lanes at endpoints.
- Natural cargo should climb straight below-rim.
- Deep shot-destroyed cargo should execute Level 8 `$19`.
- Confirm cargo never crosses the open gap.


F. LEVEL 8 `$19` FLIPPERS

- Observe deep cargo.
- Look for repeated move-one-interval then flip behavior.
- Do not expect Level 7's 2/3 reversal route.
- Verify endpoint reversal rather than wrapping.


G. SPIKES

- Confirm a starting Spike on each playable lane.
- Judge proposed height ~71.43.
- Verify Spiker growth.
- Verify shot erosion.
- Note whether height looks materially too tall or too short.


H. COMBAT

- Confirm enemy firing feels sensible.
- Confirm player shots and enemy destruction effects.
- Confirm scoring and sound.
- Confirm Super Zapper operation.


I. DEATH / RE-ENTRY

- Take at least one death if practical.
- Confirm normal reconstruction.
- Confirm no invalid lanes or stale entities.


J. PROGRESSION

- Verify natural Level 7 -> Level 8.
- Confirm score/lives carry.
- Confirm Super Zapper recharge.
- Complete Level 8 if practical.
- Confirm terminal Level 8 behavior.
- Confirm no Level 9.


K. RECORDING / PLAYBACK

- Make a fresh Level 8 recording.
- Replay it.
- Pause.
- Step forward.
- Scrub backward.
- Capture screenshots when useful.
- Use `--speed` below 1 for inspection.
- Use `--speed` above 1 for rapid review.


MANUAL ACCEPTANCE STATUS

Do not claim Level 8 accepted before Neal reports the manual results.

The implementation remains pending acceptance for:

- blind tube geometry;
- open-endpoint behavior;
- tube scale/placement/color;
- Blaster control;
- Tanker/cargo behavior;
- `$19` Flipper behavior;
- Spike height;
- Spiker behavior;
- death/re-entry;
- progression;
- terminal completion;
- recording/playback.


STOP

After automated validation and manual-test handoff:

STOP.

Do not launch the game.

Do not stage.

Do not commit.

Do not push.

Do not implement Level 9.

TEMPEST_LEVEL8_ROM_DERIVED_IMPLEMENTATION_20261108