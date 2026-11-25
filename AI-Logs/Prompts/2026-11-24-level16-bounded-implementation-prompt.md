# Tempest Level 16 Bounded Implementation

Implement Level 16 as a bounded production milestone using
`AI-Logs/Reports/2026-11-24-level16-rom-configuration-investigation-report.md`
as the specification and local Atari disassembly when verification is needed.
Preserve this prompt and create its paired report at
`AI-Logs/Reports/2026-11-24-level16-bounded-implementation-report.md`.

## Scope and evidence

- Implement and automatically validate Level 16 only. Do not launch the game,
  commit, push, calibrate presentation, or implement Level 17/recycling.
- Use arcade observation when available, Atari ROM/disassembly, accepted shared
  project behavior, then Incitti only as a secondary engineering reference.
- Keep ROM-backed facts distinct from project translations, decisions,
  reference-derived behavior, hypotheses, and unresolved behavior.
- Do not use Level 15 as a gameplay, architecture, or development model. Level
  14 may inform process only; derive all values from Level 16 evidence.
- Before editing, preserve and record branch, HEAD, upstream, staged/unstaged
  changes, and relevant untracked files. Do not disturb unrelated work.
- Stop if geometry needs a broad redesign, shared firing changes accepted
  prior levels, lifecycle defects cannot be fixed narrowly, unexplained
  regressions occur, validation is blocked, or evidence requires speculation.

## Gate 1: geometry

Represent ROM row 11 exactly: a closed 16-lane tube, source ordering and cyclic
adjacency intact, with points 0 and 8 sharing the central coordinate. Preserve
the self-touching two-lobed outline and its zero total signed area; do not
reorder, merge, delete, approximate, or globally reverse vertices. Replace the
invalid global shoelace-winding assumption with the narrowest justified
general local/lobe interpretation. Preserve accepted spinner handedness and
inward pivot behavior on existing simple tubes. First add and run focused tests
for exact geometry, zero-area construction, adjacency, all relevant lane
transitions/pivots, and existing simple-tube winding semantics. Continue only
if this gate passes without unexplained behavior changes.

## Gate 2: production configuration

Add the exact ROM X/Y/angle bytes and translate coordinates through the
established ROM-coordinate mapping. Add raw Level 16 presentation metadata
needed for normal pipeline lookup, but do not select or calibrate any
`--tube-*` host-space values.

Implement a Level-16-specific policy: original supply 27 at default/easy and
30 at hard; ordinary Flipper minimum 0/maximum 5; Pulsar 0/0; Tanker 1/2;
Spiker 2/3; Fuseball 1/1. Do not admit ordinary Flippers merely due to their
maximum; Tanker cargo Flippers remain allowed. Do not invent a fixed spawn
order or treat ROM `max_enm=6` as a six-actor C# cap.

Initialize all 16 lane Spikes from raw `$A0` using the existing documented
raw-to-world project translation. Use the existing shared repeating `$24`
Flipper program (two lane flips, reverse, three flips, reverse, repeat) for
ordinary/re-entry and Tanker cargo Flippers. Do not invent Level-16-specific
initial handedness. Tankers use two independently identified Flipper cargo
children, with adjacent-side release and the represented opposite second-child
direction. Preserve all shared ledger identity and lifecycle states.

Wire terminal Spiker-to-Tanker conversion through the existing ledger while
preserving the accepted no-lane-hop C# translation. The converted Tanker gets
two cargo identities and cannot allow premature wave completion. Permit one
Fuseball with raw `fuzz_move_prb=$C0` and `fuzz_move_flg=$00`; do not admit
Pulsars or make Fuseballs shooters. Do not invent finite Tanker ammunition.

Translate firing deliberately: raw `shot_holdoff=$23`, shot-speed low byte
`$C0`, and `enm_shotmax=$02` means slots 0–2 (three projectiles), not capacity
two. Easy decrements the final index; hard increments it, capped at 3. Keep
previously accepted Level 14 behavior unchanged. Eligible species are
Flippers, Tankers, and Spikers. Do not turn ROM physical records into a C#
actor cap or lose latent, active, pending, reconstructed, converted, or
terminal cargo/identity obligations.

Use the shared death reconstruction, wave ledger/completion, fresh-wave
Super-Zapper, HUD, recording/playback, direct-start, and progression paths.
Support `--start-level 16` and natural Level 15-to-16 progression; Level 16 is
terminal for this milestone. Do not add a Level-16 completion counter or
invent a meaning for ROM `$0116`.

## Validation and stopping point

Add focused tests for geometry, policy/configuration, cargo and conversion,
identity conservation, reconstruction, completion obligations, firing index
semantics, startup/progression/HUD, and deterministic compatibility as
appropriate. Run focused Level 16, geometry, lifecycle/progression tests, then
the complete automated solution suite, Release build, and `git diff --check`
with bounded operations. Do not weaken tests or change accepted Levels 1–15
to hide failures. Do not bypass Windows security controls.

Do not perform manual acceptance or claim arcade fidelity. Stop after
automated validation for Neal's gameplay/presentation review. Verify the exact
launch syntax before providing:
`dotnet run --project src\Tempest.Game -- --start-level 16`

The implementation report must record starting/final Git state, evidence,
changed files, geometry rationale, configuration and translations, lifecycle,
firing index/count semantics, tests and validation results, uncertainties,
manual acceptance still needed and exact launch command, unstaged AI records,
no commit/push, and confirmation Level 17/recycling and Level 15-as-model were
not implemented/used.

TEMPEST_LEVEL16_BOUNDED_IMPLEMENTATION_COMPLETE
