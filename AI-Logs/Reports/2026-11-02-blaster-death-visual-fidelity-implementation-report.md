# Blaster Death Visual-Fidelity Implementation

## Summary

Refined the existing Blaster death rendering into a deterministic, layered
angular vector effect. The implementation reuses `TempestGame.DeathTicks`;
ordinary death timing, Spike reverse-zoom timing, and gameplay behavior remain
unchanged.

## Implementation

`VectorRenderer.DrawPlayerExplosion` now draws three related starburst
outlines in yellow, orange-red, and white. Each outline has 16 angular points
and differs in scale and rotation. The effect remains centered on the
projected Blaster death location and is drawn over the tube.

The renderer samples a piecewise-linear scale envelope from normalized age
`1 - DeathTicks / PlayerDeathTicks`:

| Normalized age | Scale |
|---:|---:|
| 0.00 | 0.12 |
| 0.10 | 0.28 |
| 0.25 | 0.70 |
| 0.40 | 1.00 |
| 0.52 | 0.92 |
| 0.68 | 0.62 |
| 0.82 | 0.32 |
| 0.94 | 0.14 |
| 1.00 | 0.00 |

The base peak radius is 52 logical pixels, multiplied by the single
`DeathEffectVisualScale` value of 1.5, for an accepted peak radius of 78
logical pixels. This makes the complete effect approximately 50% larger than
the initial implementation without changing its center, outline geometry,
layer relationships, or timing envelope. The geometry, radius, layer,
multiplier, and interpolation values are project implementation choices; the
supplied Atari frames establish the qualitative progression, not these
measurements. `DeathTicks <= 0` submits no effect geometry.

## Manual Acceptance

After comparing the enlarged effect with the supplied Atari sequence, Neal
reported that it is nearly perfect, that no further visual tweaking is
needed, and that no new gameplay issue was reported. This milestone is
accepted. The project goal is an acceptably faithful arcade experience with
reasonable compromises, not a frame-perfect or identical clone. Acceptance
does not make the implementation's exact geometry, timing, or scale constants
arcade-verified.

Ordinary death continues to use `PlayerDeathPresentation` only for enemy
retraction; the Blaster effect is sampled independently from `DeathTicks`.
Spike reverse-zoom does not start ordinary enemy retraction, but its existing
countdown still drives the same Blaster drawing. No gameplay entities,
collisions, scoring, or state transitions were added.

## Tests

- Added deterministic line-callback coverage for the death origin, initial
  development, substantial expansion, contraction, colored layers, a
  diminishing remnant, clean disappearance, and unchanged ordinary
  life/reconstruction timing.
- Added Spike reverse-zoom coverage confirming the effect draws while ordinary
  enemy retraction remains inactive and Spike/life state is preserved.
- Existing lifecycle and regression tests remain in place.

## Validation

- Focused `PlayerDeathPresentationTests`: **6 passed**.
- Broader Level 4 tests: **24 passed**.
- Complete solution test suite: **605 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **succeeded, 0 warnings, 0 errors**.
- `git diff --check`: **passed**.

Automated validation does not establish visual acceptance. Manual comparison
against the supplied arcade frame sequence was completed and accepted as
described above.

## Worktree

No files were staged or committed. `TEMP/` remains untouched. `AI-Logs/` is
gitignored by the repository, so the saved prompt and this report exist in the
worktree but do not appear in ordinary `git status` output.
