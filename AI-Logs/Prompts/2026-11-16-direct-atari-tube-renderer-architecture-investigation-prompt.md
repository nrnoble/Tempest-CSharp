# Tempest: Read-Only Direct Atari Tube-Renderer Architecture Investigation

Determine whether Tempest can render a tube directly from local Atari ROM-derived
tube records and the reconstructed Atari coordinate pipeline, while leaving
accepted C# gameplay geometry, lanes, mechanics, simulation, and deterministic
recording/playback unchanged. This is an architecture investigation only.

## Scope and safeguards

- Do not implement a renderer or edit production source, tests, durable
  documentation, configuration, Atari reference material, visual assets, or
  Git state.
- Do not launch the game, stage, commit, push, clean, reset, restore, stash, or
  switch branches.
- Preserve accepted Levels 1-6. Do not change Level 7 gameplay geometry, order,
  or lane identity. Keep the candidate opt-in and separate from the existing
  `--atari-presentation` adapter.
- Do not inspect screenshots, images, videos, gameplay recordings,
  contact-sheets, or `TEMP/myNotes.md`; do not use external web sources.
- Read the controlling 2026-11-15 `$c098` reconciliation completely. It
  supersedes earlier reports that called `$0f` a persistent projection mode or
  left the coordinate operation unresolved. Use original local Atari
  documentation and ROM-derived disassembly, plus the already verified
  bounded PROM/ROM findings; do not redo generalized Math Box archaeology or
  access raw ROM copies unnecessarily.
- Create exactly the authorized prompt and report files in `AI-Logs/`; do not
  stage or commit them.
- Capture branch, HEAD, upstream, ahead/behind, index, tracked changes, and
  nonignored untracked paths. Preserve the observed state.
- No full build or test run is required. Do not claim manual visual acceptance.

## Questions to resolve

1. Reconcile obsolete Math Box conclusions with later cross-verified facts:
   actual `$c098` write addresses/functions; `$0f`/NL; `DIVZ`; divisor,
   factor-of-256 magnitude, truncation, CPU sign/offset/saturation; retained
   state and its remaining uncertainties. Distinguish exact hardware
   emulation, ROM-faithful normal-game arithmetic, project translation, and
   approximation.
2. Trace the normal tube path from displayed level and `lev_remap`, through
   selected raw point/record parameters and `lev_open`, runtime `$57`, Math
   Box, CPU post-processing, DVG packing, vector-center/scale commands, and
   ordered vector list. Give units, signedness, orientation, scope (global or
   per-record), and evidence classification at each stage.
3. Identify the strongest canonical DVG output and determine the remaining
   DVG-to-portrait viewport gap. Choose exactly one global-map classification:
   affine map sufficient, map plus global aspect parameter, per-tube host
   calibration required, or unresolved.
4. Define a direct record-to-DVG arithmetic contract, including divisor,
   integer quotient, sign, offsets, signed saturation, 13-bit DVG serialization,
   and output representation.
5. For Level 7 / record 06, compare current C# clover topology/order with Atari
   record-06 rails. Assess point count, closure, winding, index direction,
   cyclic correspondence, adjacency, opposite-neighbor relationships, and
   directed transitions. Treat the prior `+5` fit only as comparison evidence;
   prove or reject it as a gameplay-to-render bridge. Choose exactly one lane
   correspondence classification.
6. Trace C# near/far rims, below-rim entities, player, enemies, shots, Spikes,
   Approach, and exit to Atari runtime position. Choose exact, affine,
   endpoint-normalized project translation, state-dependent, or unknown.
7. Evaluate rendered-lane interpolation, per-object Atari projection, and a
   hybrid for gameplay-object anchoring. Select one or leave unresolved. Address
   local glyph scale/orientation using the recovered projection, not the
   unrelated current `K/(z+ZOffset)` factor.
8. Assess the known Levels 1-3 Blaster-exit presentation defect without
   modifying it. Determine what can and cannot be inferred without visual
   inspection.
9. Establish coexistence with accepted Levels 1-6; define a non-default,
   development-only Level 7 prototype boundary if feasible.
10. Specify deterministic nonvisual test vectors and label tests as
    Atari-documented, ROM/PROM-cross-verified, arithmetic, or
    project-translation tests. Identify visual-only calibration.
11. Select exactly one feasibility classification: feasible from current Atari
    evidence; feasible with one bounded missing piece; feasible only as an
    explicit project translation; not feasible with current evidence; or
    architecturally incompatible.
12. Apply the required seven-question test to every decision-relevant unknown:
    impact on feasibility, arithmetic, lane mapping, depth mapping, global
    display mapping, gameplay/model changes, and risk of a misleading Level 7
    prototype.
13. If feasible, design exactly one smallest future Level 7-only prototype
    milestone: files/classes, immutable inputs, outputs, lane/depth bridge,
    display-map boundary, tests, validation, rollback, and Neal's manual
    acceptance stop. Do not implement it.

## Required report

Save exactly
`AI-Logs/Reports/2026-11-16-direct-atari-tube-renderer-architecture-investigation-report.md`
and the paired prompt at
`AI-Logs/Prompts/2026-11-16-direct-atari-tube-renderer-architecture-investigation-prompt.md`.
The report must include the executive conclusion; evidence reconciliation;
pipeline and arithmetic contract; global display-map classification; Level 7
lane and depth bridges; object strategy and local scale/orientation; Blaster
exit relevance; Levels 1-6 isolation; validation oracle; one prototype design;
decision-relevant unknowns; one next milestone; inspected evidence; bounded
calculations; and exact starting/ending repository state.

No source or gameplay changes are authorized. Stop before any game launch;
manual visual acceptance remains Neal's responsibility after any future
implementation.

TEMPEST_DIRECT_ATARI_TUBE_RENDERER_ARCHITECTURE_READONLY_20261116
