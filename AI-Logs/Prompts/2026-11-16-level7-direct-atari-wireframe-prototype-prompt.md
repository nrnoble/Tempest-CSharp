# Tempest: Isolated Direct-Atari Level 7 Wireframe Prototype

Implement a separate, development-only `--atari-tube-prototype` presentation
for an explicitly selected, interactive portrait Level 7 start. Drive the
wireframe from Atari Rev 2 record 06 using the controlled integer Math Box
translation, canonical signed-13 DVG coordinates, and one shared provisional
DVG-to-portrait viewport map.

The prototype must not replace or alter `--atari-presentation`, legacy
rendering, accepted Levels 1-6, existing Level 7 gameplay/model geometry,
simulation, recordings, or playback. Do not modify gameplay systems, protected
configuration/projection sources, or durable behavior documentation. Do not
inspect screenshots, tune against MAME, search the Internet, modify reference
ROM material, launch the game, stage, commit, push, reset, restore, stash,
rebase, or switch branches. Preserve existing work and do not overwrite
untracked files.

Use the controlling architecture and Math Box reconciliation reports. Treat
the quotient result as conditional on a zero quotient-count high byte with
low byte `$0f`; implement integer truncation, CPU sign/base/saturation rules,
source-Y-to-DVG-X ordering, and signed-13 packing. Validate record 06's raw
point data and metadata against existing local ROM-derived evidence. Keep the
record, projector, canonical vector-list builder, and viewport map pure and
isolated from mutable gameplay entities. Draw only the direct near/far rims
and 16 rails; omit gameplay glyphs, Approach, exit, Eggs, and unsupported
levels/states. Make the option opt-in, non-recorded, explicitly experimental,
and reject incompatible starts and diagnostics.

The global viewport map may expose only shared display transforms. Physical
orientation, signs, gain, and aspect are not established; label any provisional
values as such, do not introduce Level-7-specific tuning, and stop PARTIAL if
a defensible shared map cannot place the wireframe in the portrait viewport.
Keep Game Over and all option-disabled paths unchanged.

Add focused tests for record immutability/validation, exact arithmetic vectors,
canonical DVG packing/order/topology, deterministic viewport mapping and
invalid inputs, CLI isolation, renderer/model isolation, and preservation of
existing presentation behavior. Run bounded focused regressions, the full test
suite, Release solution build with zero warnings/errors, `git diff --check`,
and final repository-state checks using the established VSTest timeout and
isolated-output policy. Never report incomplete or security-blocked validation
as passing.

Do not launch the game. If every gate passes, provide this proposed manual
command and ask only whether the record-06 wireframe is recognizable, whether
its perspective looks plausibly like Tempest, whether it is contained/usable
in portrait, and whether to continue lane/depth registration. Do not request
entity-registration, Levels 8-9, or recording evaluation.

Create only this prompt and its paired implementation report under ignored
`AI-Logs/`; do not stage or commit either artifact. Completion token:

TEMPEST_LEVEL7_DIRECT_ATARI_WIREFRAME_PROTOTYPE_20261116
