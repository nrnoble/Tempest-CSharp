# Tempest Level 18 Final Closeout

Date: 2026-11-26

## Scope

Perform the final closeout of the accepted medium/default Level 18 production
milestone. Record Neal's manual acceptance; reconcile implementation against
the ROM/configuration, Pulsar, and bounded implementation reports; preserve
Levels 1-17; update durable documentation; run fresh automated validation;
stage only intended Level 18 source, tests, documentation, and Level 18
AI-Logs; create one primary closeout commit; push normally; attempt independent
GitHub verification; and use at most one report-finalization commit.

Do not add Level 19, reopen DebugMCP or the deferred Level 15 Fuseball timing
question, change accepted gameplay, or launch the interactive game.

## Required authorities

- `AI-Logs/Reports/2026-11-25-level18-rom-configuration-readonly-investigation-report.md`
- `AI-Logs/Reports/2026-11-26-level18-pulsar-movement-translation-investigation-report.md`
- `AI-Logs/Reports/2026-11-26-level18-bounded-production-implementation-report.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `.github/copilot-instructions.md`

## Final acceptance

Neal reports that Level 18 passes manual testing, with no new problems or
regressions and acceptance of gameplay, Shape 2 presentation, cycle-1 palette,
HUD, Flippers, Tankers, cargo, Pulsars, firing pressure, death/re-entry,
natural progression, direct startup, and terminal behavior. Easy/hard variants
are not manually accepted.

## Required implementation invariants

Level 18 uses displayed identity 18, progression index 17, ROM
`curlevel=$11`, effective `lev=$12`, palette index 1, canonical Shape 2/board
index 1, 21 original supply, seven physical slots, 4 initial Eggs, eligible
Flippers 0-3, Tanker 1, Pulsars 2-5, no Spikers/Fuseballs/starting or dynamic
Spikes, and exactly two Flipper Tanker cargo identities.

Preserve shared `EightAdvanceTurn` for ordinary and cargo Flippers, 2.625
Tanker/Flipper host speed, firing holdoff 29/capacity 3, parameterized shared
Pulsar movement, Level 17's no-threshold fixed-1 compatibility, and Level 18's
threshold 392.92035 and fixed/deep speeds 1/21-11. Preserve terminal Level 18
and unsupported Level 19.

## Validation and Git

Use fresh focused gameplay/presentation regressions, shared regressions,
both unmodified recordings, a full solution suite, a fresh non-incremental
Release build, whitespace checks, scoped staging, one primary commit, normal
push, and independent GitHub verification where available.

Protected recordings, screenshots, videos, raw ROM/reference assets, TEMP
material, generated output, and unrelated user state are excluded.

TEMPEST-END-LEVEL18-FINAL-CLOSEOUT-COMPLETE
