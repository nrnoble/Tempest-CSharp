# Atari HUD read-only investigation report

Date: 2026-11-20
Status: Read-only investigation complete
Completion token: `TEMPEST_ATARI_HUD_READONLY_INVESTIGATION`

## 1. Atari HUD architecture

### Main display construction

The gameplay HUD is assembled in the Atari vector video list rather than by a conventional framebuffer font.

- `show_player_stuff` at `$a97f` (`p1_lives`/`p1_score`) owns the reserve-life and score assembly for each player.
- `hdr_template` at `$cde6` contains the canned vector sequence for the one-player header and two-player-only continuation.
- `setup header` at `$aa13` copies `hdr_template` into vector RAM and patches in the live level.
- `char_jsrtbl` at `$31e4` maps character indices to digit/space glyph subroutines:
  - space `$1168`;
  - digits 0-9 at `$10ca` through `$11d6`;
- `ltr_jsrtbl` at `$31fa` maps letters A-Z and other characters to `$1000` through `$115c`.
- `vapp_digit_lz` at `$df19` appends a digit and implements leading-zero/blank behavior.
- `vapp_multdig_y@a` at `$dfb1` appends a pair-based multidigit value.
- `vapp_vjsr_AX` at `$df38` appends a character subroutine to the video list.
- `vapp_scale_A,0`/`vapp_sclstat_A_Y` and `vapp_setcolor` control scale and DVG intensity/color-stat selection.

The header template establishes the score/lives/high-score/level/initials layout:

| Header section | Template location |
|---|---|
| P1 six score positions | `$cdf2-$cdfc` |
| P1 reserve-life slots | `$ce08-$ce12` |
| High score, six slots | `$ce1a-$ce24` |
| Current level, two slots | `$ce2c-$ce2e` |
| High-score initials, three slots | `$ce36-$ce3a` |
| P2 continuation | `$ce3e-$ce64` |

The header is assembled at scale `b=1`, DVG state `z=12`, and explicit level/high-score/initials values are patched into the fixed template.

## 2. Vector glyph construction

### Digits

ROM-backed digit subroutines are actual DVG vector programs:

- 0: `$10ca`/address record `$31ce`
- 1: `$116c`/`$316c`
- 2: `$1174`/`$3174`
- 3: `$1184`/`$3184`
- 4: `$1192`/`$3192`
- 5: `$11a0`/`$31a0`
- 6: `$11ae`/`$31ae`
- 7: `$11bc`/`$31bc`
- 8: `$11c6`/`$31c6`
- 9: `$11d6`/`$31d6`

The digits are block-like, approximately 16-24 DVG units high, with a 24-unit nominal vertical extent and roughly 16-unit main width. The disassembly uses `vsdraw` with a mix of draw, move, and state/l/effect encodings. `vjsr` calls advance each digit by the glyph program’s own move data, and the fixed header reserves fixed character positions.

`vapp_digit_lz` masks the low nibble, suppresses a zero to a space while leading-zero suppression is active, and clears leading-zero state after the first nonzero digit. `vapp_multdig_y@a` processes two-digit pairs from the low pair upward and is also the basis for right-to-left score assembly. This supports six fixed-score/numeric slots with leading blanks rather than visible leading zeroes.

### Uppercase letters

The reusable ROM letter table is `ltr_jsrtbl` at `$31fa`; it recovers A-Z at exact DVG addresses `$1000-$115c`. Uppercase letters are 24-unit class glyphs constructed from relative `vsdraw` moves. Letter A is at `$1000` and is specifically suitable for the fixed `AAA` implementation. It uses a block-like A with a crossbar and a small advance move.

The ROM has an explicit letter table and text-writing routine, so a C# implementation should not infer an alphabet by using screenshots.

### Reusable text and spacing

`vapp_msg` at `$ab14` retrieves message text pointers, sets scale and color, and writes each character’s `vjsr` entry to the video list. Character order is stored as 7-bit text bytes, with high bit terminators. The general scale routine `vapp_setscale` at `$b0dd` and message metadata in `$d121/$d122` supply scale and color metadata.

The exact HUD template advance is encoded in the fixed header sequence and VLDRAW moves, not in a separate modern “text-layout table.” The DVG glyph subroutines contain advance moves and the header inserts additional offsets where required.

### Junction/intensity behavior

The ROM evidence establishes DVG state fields and the `sparkle` bit in `vstat`, but it does not identify a separate HUD junction feature. The brighter junction points at line intersections should therefore be classified as **UNRESOLVED**: potentially a natural DVG overlap/intensity/state consequence, or an incidental raster/DVG behavior, rather than an established explicit feature.

No junction-effect implementation belongs in the first HUD milestone.

## 3. Current score

### State and conversion

ROM state stores P1 and P2 scores as three BCD bytes:

- `p1_score_l/m/h` and `p2_score_l/m/h`;
- `inc_score` at `$ca6d` adds to the BCD score;
- `show_player_stuff` at `$a97f` formats the active player and writes six digit slots.

The score routine extracts high/low nibbles, suppresses leading zeroes through carry/leading-zero state, and appends `char_jsrtbl` entries. The fixed header reserves six digit slots plus one O/digit-zero terminator/space slot. P1/P2 position is selected by player-indexed offsets.

### Geometry and alignment

The supplied reference establishes that current score is right-justified. The ROM template also presents a fixed six-position right-justified field: leading positions are blanked, and the numeric glyphs occupy the remaining right side.

No row of conventional font spacing exists. Digit width/advance comes from the ROM digit subroutines and header moves.

### Color and C# translation

The header template sets P1 score to DVG state `vstat z=12 c=5 sparkle=1`; current score and high score/initials share this state in the header.

For C#, the target appearance is **green**. That RGB mapping is a **PROJECT TRANSLATION** from the Atari DVG color-stat value, not a literal recovered ROM RGB value.

## 4. High score and initials

The header reserves:

- six high-score digit slots at `$ce1a-$ce24`;
- three initial slots at `$ce36-$ce3a`;
- a fixed move and scale between the high-score field and initials.

The high-score byte storage is `hs_score_1` and the initials storage is `hs_initials_1`; lower RAM entries hold `hs_score_2...8` and `hs_initials_2...8`.

The initials routine at `$a929` reads each stored character, doubles the character index, and appends a `ltr_jsrtbl` `vjsr`. The ROM defaults include BEH/MJP/SDL/DFT/MPH/RRR/DES/EJD, but the task-specific first implementation uses fixed `AAA` rather than preserving or extending EAROM initialization.

The first implementation should therefore display fixed `38930` in the six-position field and fixed `AAA` using the ROM A glyph. It should not implement persistence, qualification, initials entry, or leaderboard behavior.

## 5. Current level

The header reserves two level digit slots at `$ce2c-$ce2e`; the level text is DVG state `c=7`, identified by the header as blue. `setup header` at `$aa13` loads `curlevel`, adds one, and calls `vapp_2dig_bin`.

ROM state:

- `curlevel` and `p1_level/p2_level`;
- level progression increments `p1_level` and `curlevel`;
- the ROM comparison near `$c98e` prevents exceeding level 99 and sets the displayed level in the 1-based header as `curlevel + 1`.

Thus ROM header support is two-digit. The C# HUD should use `CurrentLevel.Configuration.DisplayedLevel` and support at least multi-digit text. The exact C# layout/scale may translate the two-slot Atari header while not changing tube calibration.

## 6. Lives/Blaster symbols

### ROM reserve-life semantics

`show_player_stuff` at `$a97f` obtains `p1_lives`/`p2_lives`, then for the active player subtracts one before rendering:

```text
$38 = p1_lives[p]
if p == curplayer
    $38 -= 1
```

It then loops `Y = 1` through `Y = 6`, and writes one of two Blaster vjsr entries into six fixed slots:

- `$3284` points to the nominal Player subroutine `$126e`;
- `$3286` points to the alternate Player subroutine `$127e`.

Accordingly, the ROM renders **reserve lives only** and never includes the currently active Blaster. The C# decision `max(totalLives - 1, 0)` matches this semantic.

### Blaster vector geometry

The nominal Player subroutine is at `$326c` with DVG state `z=12 c=1 sparkle=1`. It contains:

- relative moves/draws around a nominal 60-unit rightward extent;
- symmetric left/right side points near `(-18,+/-6)` and `(+12,+/-6)`;
- rightward and leftward geometry;
- a final offdraw move `x=+60`.

The alternate subroutine entry starts at `$3286` and resolves to `$127e`. It is present in the ROM as a selected alternate character slot, but no separate public ROM-backed geometry comparison establishes which state chooses or distinguishes it. The fixed header supplies six vjsr slots and no explicit conventional spacing table.

`show_player_stuff` also writes DVG state `vstat z=12 c=1` for the lives row. In C#, the target reserve-life color is **yellow**. The exact converter is a PROJECT TRANSLATION unless project-side Atari palette evidence establishes it more directly.

### Atari display limit

The ROM loop explicitly has `Y < 7`, giving six fixed slots. That is stronger evidence of a six-slot header convention than of a general data structure or explicit “maximum reserve lives” rule.

Separately, bonus-life code at `$cadc` refuses to increment `p1_lives` at or above six:

```text
cmp #$06
bcs no_bonus_life
inc p1_lives
```

The evidence therefore supports:

- the normal ROM life accounting maximum is six total lives;
- the header convention is six rendered Blaster slots;
- no separate show-only cap beyond the header slots is needed in the normal ROM.

This does not impose a six-life restriction on C#. C# has unrestricted `Player.Lives`; the chosen temporary **25-symbol display cap** is a PROJECT DECISION. It must not mutate game state. The first implementation should calculate `reserveCount = Math.Max(game.Player.Lives - 1, 0)` and stop drawing after 25.

## 7. Current C# architecture

### Existing HUD

The only gameplay HUD implementation is `DrawHud(PresentationMode)` in [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs). It currently uses Raylib font calls:

- `Raylib.DrawText(game.Score.Score.ToString(), 10, 10, 28, Color.Yellow)`
- `Raylib.DrawText($"LIVES {game.Player.Lives}", 10, 44, 20, Color.Yellow)`
- `Raylib.DrawText(levelText, levelX, 10, 20, Color.Yellow)`

It also draws wave diagnostics and development diagnostics with conventional fonts. Those parts are outside the target first HUD milestone.

### Existing vector text

[VectorFont.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorFont.cs) provides deterministic geometry for a set of modern/project glyphs and digits. It supports centered text and already is used by AVOID SPIKES, Game Over, and playback status. It is retained as a rendering primitive, but its current stroke tables are not yet established as the ROM HUD glyph tables:

- geometry generally follows Incitti or project choices;
- comments in [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs) explicitly describe glyphs as generally Incitti and scales as project choices;
- the current font lacks several ROM letters needed for generic text.

The existing [VectorFontTests.cs](/C:/Users/nrnob/source/repos/Tempest/tests/Tempest.Game.Tests/VectorFontTests.cs) verifies deterministic geometry and center/bounds; it does not establish arcade fidelity.

### State integration

Live HUD state already reaches the renderer:

- `TempestGame.Score` exposes `ScoreSystem.Score`;
- `TempestGame.Player.Lives` exposes live unrestricted lives;
- `TempestGame.CurrentLevel.Configuration.DisplayedLevel` exposes the active level.

No game-state redesign is needed.

### Portrait integration

[DisplayConfiguration.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/DisplayConfiguration.cs) defines:

- 600x800 logical portrait layout;
- `ExperimentalPortraitHudCamera`, with the existing portrait viewport offset and `PortraitOutputScale`;
- a separate playfield camera;
- a separate 960x1280 accepted portrait viewport centered at host x 320.

`DrawPortraitOverlays` in [VectorRenderer.cs](/C:/Users/nrnob/source/repos/Tempest/src/Tempest.Game/Rendering/VectorRenderer.cs) uses the separate HUD camera and calls `DrawHud(PresentationMode.ExperimentalPortrait)`. This is the correct host for the first HUD implementation.

### Preservation boundary

The implementation milestone should:

- change only Atari/portrait HUD drawing;
- retain legacy `PresentationMode.Legacy` behavior unless separately authorized;
- keep the playfield camera and its accepted levels 1-10 calibration frozen;
- keep the existing upper reserved information region and not move/reproject/recalibrate tubes;
- retain playfield entities and deterministic simulation/recording behavior.

## 8. Evidence classification

| Finding | Classification |
|---|---|
| Three BCD score bytes, digit conversion, and leading-zero suppression | **ROM/DISASSEMBLY BACKED** |
| Fixed header six score slots, six lives slots, six high-score slots, two level slots, three initial slots | **ROM/DISASSEMBLY BACKED** |
| `ltr_jsrtbl` and exact A-Z DVG subroutine addresses | **ROM/DISASSEMBLY BACKED** |
| Exact digit subroutine addresses and block-vector construction | **ROM/DISASSEMBLY BACKED** |
| Nominal Blaster subroutine `$326c`/`$126e` and alternate `$127e` | **ROM/DISASSEMBLY BACKED** |
| Reserve-life count equals total lives minus one for the active player | **ROM/DISASSEMBLY BACKED** |
| ROM normally stops bonus lives at six total | **ROM/DISASSEMBLY BACKED** |
| Six-slot header convention / no explicit separate Atari cap | **ROM/DISASSEMBLY BACKED**; the normal-life limit makes the conventional cap redundant, but no generic cap routine was established |
| Arcade target appearance: green score/high score/initials, blue level, yellow reserves | **ARCADE/SCREENSHOT OBSERVED** in the supplied task reference |
| Exact RGB values for C# | **UNRESOLVED**; existing project Raylib color choices are the implementation boundary |
| Brighter glyph junction points | **UNRESOLVED**; no explicit separate ROM feature established |
| Existing C# state exposes live score, unrestricted lives, and displayed level | **CURRENT C# BEHAVIOR** |
| Existing C# HUD uses font rendering | **CURRENT C# BEHAVIOR** |
| Existing C# VectorFont uses shared deterministic project/Incitti-derived geometry | **CURRENT C# BEHAVIOR** |
| Fixed C# high score `38930`, initials `AAA`, and 25 reserve-symbol display cap | **PROJECT DECISION** |
| C# unrestricted underlying lives | **PROJECT DECISION** |
| Preserve legacy presentation and accepted portrait tube calibration | **PROJECT DECISION** |

## 9. Bounded implementation recommendation

The smallest subsequent milestone should add an Atari-presentation-only vector HUD, without changing the tubes or game-state rules:

1. Add or extract a portrait-HUD glyph path from the existing `VectorFont` primitives.
   - Prefer an implementation table based on recoverable ROM digit and uppercase-A geometry rather than the current screenshot-fitted geometry.
   - Keep at least `0-9` and `A` exact enough for this milestone.
   - Isolate project-scale/layout constants from tube projection.

2. Replace only the font-driven gameplay HUD in the portrait/Atari path:
   - reserve-life symbols left-justified in the upper reserved region;
   - live score right-justified and green;
   - fixed high score `38930` and initials `AAA` green;
   - live displayed level blue.

3. Derive reserve lives from live state:
   - `count = Math.Max(game.Player.Lives - 1, 0)`;
   - draw no more than 25 symbols;
   - never alter `Player.Lives`.

4. Add focused tests for:
   - reserve count excludes the active Blaster;
   - the 25-symbol cap leaves `Player.Lives` unchanged;
   - score right-justified formatting;
   - fixed high score/initials;
   - live multi-digit level;
   - deterministic finite vector submissions;
   - legacy presentation remaining unchanged.

5. Leave wave/development HUD diagnostics, high-score persistence, initials entry, leaderboards, attract mode, scoring, and tube presentation outside the milestone.

Manual acceptance remains required for perceived size, placement, glyph fidelity, line brightness, and composition.
