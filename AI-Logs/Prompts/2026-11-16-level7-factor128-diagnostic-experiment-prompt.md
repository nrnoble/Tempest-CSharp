# Level 7 conditional factor-128 diagnostic experiment

Implement a tightly bounded DEVELOPMENT-ONLY diagnostic experiment for the
Level 7 direct-Atari wireframe prototype. Preserve its current factor-256
quotient as the control and add an opt-in option selecting the conditional
RN=$000F factor-128 quotient:

    floor((128 * magnitude) / depth)

This tests only whether that quotient materially changes the malformed Level 7
prototype geometry. It does not establish factor 128 as normal Atari behavior,
an Atari-exact production correction, or manual acceptance.

## Evidence boundary

- RN is a 16-bit Math Box register; normal software establishes RN.low=$0F.
- Local evidence does not establish RN.high at the first normal Level 7
  record-06 DIVZ operation.
- The traced PROM result is `floor(128 * magnitude / depth)` conditional on
  RN=$000F. RN.high=0 remains unresolved at the normal call.
- The prototype currently uses `floor(256 * magnitude / depth)`.
- Preserve these qualifications in code, tests, and report. Do not call the
  conditional calculation ROM-BACKED normal Level 7 behavior or promote the
  RN.high=0 hypothesis into an accepted project translation.

## Scope and preservation

Modify only the isolated DEVELOPMENT-ONLY direct-Atari tube prototype as
necessary. The selector must be opt-in, Level-7-prototype-specific,
presentation-only, isolated from production gameplay and `--atari-presentation`,
and easy to remove. Keep the existing

    dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype

as the unchanged control. Add a narrowly named development diagnostic option;
reject invalid or conflicting use explicitly and consistently with the
prototype's existing CLI validation. Do not introduce generic projection
architecture or arbitrary tuning controls.

Do not change record-06 bytes or metadata, point order/count, p=$10/p=$F0
positions, point correspondence, rail/rim topology, open/closed state, axis
ordering, DVG center, viewport map, translation, scale, aspect, rotation, or HUD.
Do not fit to screenshots, trace screenshot coordinates, or add per-point
corrections.

Do not change gameplay geometry/topology, movement, collision, spawning, enemy
behavior, timing, score, lives, RNG, recording/playback semantics, transitions,
accepted Levels 1-6, Levels 8-9, the existing `--atari-presentation` candidate,
or Approach/exit presentation. Do not investigate the Blaster-exit rendering
defect.

The integer quotient must truncate exactly; retain raw differences, sign
restoration, X/Y bases, signed handling, DVG axis mapping, point ordering,
vector construction, and viewport mapping. Document the diagnostic as
DEVELOPMENT-ONLY and conditional on RN=$000F; explicitly say that RN.high=0 is
not established at the normal Level 7 operation.

## Automated coverage and validation

Cover unchanged option-off behavior; exact conditional RN=$000F vectors and
integer truncation; signs and bases; record-06 data and order; near/far values;
topology and same-index rail pairing; presentation-only and Level-7-specific
selection; existing `--atari-presentation` behavior; recording/playback
prototype conflicts; and explicit failures for invalid selector combinations.
Do not weaken tests.

Run focused prototype/projector/option tests; relevant presentation, CLI,
recording/playback, Level 7, and Atari-presentation regressions; the full test
suite if practical; a Release build; and `git diff --check`. Do not launch the
interactive game. Preserve pre-existing work and untracked/reference material.
Do not reset, restore, clean, stash, rebase, switch branches, stage, commit, or
push. Record starting and ending Git/worktree state; do not absorb unrelated
changes.

Save this prompt and a paired report in ignored `AI-Logs/Prompts/` and
`AI-Logs/Reports/`. The report must list changed files, selector and
calculation, evidence classification and RN.high qualification, retained
control, representative numeric vectors, validation outcomes, protected
geometry/presentation/gameplay paths, Git/worktree state, exact A/B commands,
and manual acceptance as PENDING. Do not stage or commit these records.

After automated validation, stop for Neal. Provide the exact control command
above and the exact diagnostic command, explain which is control versus
experiment, and do not claim visual improvement or Atari fidelity. Neal's
manual A/B comparison is pending.

TEMPEST_LEVEL7_FACTOR128_DIAGNOSTIC_EXPERIMENT
