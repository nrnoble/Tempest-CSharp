# Preserve Level 15 tube and reconcile Levels 11-16 mapping

Date: 2026-11-21

## Scope

Continue the existing uncommitted production-level candidate. Neal's manual
identification establishes that its preserved tube geometry is the arcade
Level 15 tube, not Level 11. Preserve the geometry, gameplay/infrastructure
work, generic Fuseball identity conservation, runtime representation
validation, and `$24` / `$07` Tanker-cargo Flipper factory support. Do not
reshape the tube or discard useful work.

Reconcile displayed-level identity, zero-based geometry lookup, the
`lev_remap` shape-row selection, and effective gameplay-parameter indexing
using tracked disassembly, historical recovery reports, existing C# records,
and bounded read-only access to only
`Reference/arcade/roms.rev2/136002-118.k1` when necessary. Do not enumerate
the ROM directory, inspect `MBUCOD.V05.txt` or unrelated ROM regions, or
copy, modify, stage, commit, publish, or redistribute ROM data. Do not infer
the mapping from screenshots.

Preserve the current geometry as a direct Level 15 candidate, including its
HUD identity and Atari presentation path. Keep Level 11 unimplemented unless
its genuine geometry can be recovered unambiguously; record evidence for the
next milestone without fabricating production behavior. Do not let production
progression skip from Level 9 to Level 15. Keep diagnostic Level 10 unchanged
and preserve Level 1-10 behavior.

Keep Level 11 gameplay parameter evidence distinct from Level 15 identity.
Any temporarily retained Level 11 parameter row must be marked as an
unaccepted development baseline, not Level 15 gameplay evidence. Classify
Level 15 tube identity as manually established, gameplay as generally good
but not formally accepted for Level 15 fidelity, and presentation fine-tuning
as pending as needed.

## Validation and preservation

Add focused coverage for Levels 11-16 remapping, preserved Level 15 geometry
and direct startup, Level 15 HUD identity, unsupported direct Level 11,
terminal Level 9 progression, Level 11 geometry evidence, generic Fuseball
identity/lifecycle, independent `$24` / `$07` factory paths, Level 1-10 and
diagnostic Level 10 behavior, recording/playback, Atari presentation/HUD,
Approach/ExitZoom, and fail-closed runtime representation validation.

Run focused tests, the complete solution test suite, Release solution build,
and `git diff --check`. Do not launch the interactive game. Save a paired
report, give direct Level 15 commands for normal presentation, Atari
presentation, calibration guides, and the existing tube calibration controls.
State remaining manual acceptance and Level 11 evidence. Do not stage, commit,
or push, and do not modify unrelated local material.

Completion token:
`TEMPEST_RECLASSIFY_LEVEL15_AND_RECONCILE_TUBE_MAPPING`
