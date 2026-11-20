# Portrait Perspective Range Expansion — BLOCKED

## Status

**BLOCKED — no range expansion retained.** Neal chose to abort expansion when
the unchanged perspective formula was found to amplify during ExitZoom and
ReverseZoom. The range remains `[-5, +5]`, and the default remains neutral `0`.
No gameplay or projection behavior was changed by this follow-up.

## Scope and evidence

The analysis used the existing formula and production Levels 1–9:

```text
yProjected = CenterY + (y + YOffset) K / (z + ZOffset)
             + a K (1 / (z + ZOffset) - 1 / (nearDepth + ZOffset))
```

For stable portrait play (`K=70`, `ZOffset=5`, near depth 50), host-space
displacement at the normal far depth 450 is `-1.40979 * a` pixels. These are
calibration calculations, not arcade measurements.

| Coefficient | Far-depth displacement |
|---:|---:|
| -40 | +56.39 px |
| 0 | 0 px |
| +40 | -56.39 px |
| +80 | -112.78 px |
| +100 | -140.98 px |

Minimum far-rim Y across each production layout in stable play:

| Level | -40 | 0 | +40 | +80 | +100 |
|---:|---:|---:|---:|---:|---:|
| 1 | 466.39 | 410.00 | 353.61 | 297.22 | 269.02 |
| 2 | 473.95 | 417.56 | 361.17 | 304.78 | 276.58 |
| 3 | 466.98 | 410.58 | 354.19 | 297.80 | 269.61 |
| 4 | 460.39 | 403.99 | 347.60 | 291.21 | 263.01 |
| 5 | 481.42 | 425.03 | 368.63 | 312.24 | 284.05 |
| 6 | 420.26 | 363.87 | 307.48 | 251.08 | 222.89 |
| 7 | 370.63 | 314.24 | 257.85 | 201.46 | 173.26 |
| 8 | 355.90 | 299.51 | 243.12 | 186.73 | 158.53 |
| 9 | 389.13 | 332.74 | 276.35 | 219.96 | 191.76 |

The limiting stable far rim is Level 8. The whole normal tube envelope remains
`Y=0.14..675.62` because its extrema are at the unchanged near rim. The lowest
normal HUD text ends near host Y=136.8; at +100 the Level 8 far rim would be
about 21.7 px below it. Negative coefficients remain within the normal viewport.
The near-rim anchor is unchanged for every finite coefficient. Reciprocal-depth
affinity preserves rail incidence algebraically; the existing tests cover it at
the current range.

At the maximum enemy-retraction depth (850), the +100 Level 8 minimum is about
Y=167.60. Game Over records are drawn without the normal HUD; their valid Z range
is 16–239, giving projected depths 66–289. During approach, the near-depth anchor
moves with the approach offset; at the full 960-unit offset, the +100 far-depth
displacement is only about -2.46 px. These stable and extended contexts do not
cause the blocking failure.

## Blocking transition behavior

The shared projection also runs during exit/reverse zoom. ExitZoom decreases
`ZOffset` from 5 to -41 while `K` is 70, then holds `ZOffset` at -41 while
increasing `K`. At the start of that second phase, the near denominator is 9 and
the far-depth host displacement becomes about `-9.584 * a` px. Thus the
otherwise useful +40 probe moves far-depth geometry about 383.4 px upward:
the Level 8 far-rim envelope at that transition point becomes approximately
`Y=-88.22..125.68`, clipping above the viewport. The effect grows further as
`K` increases; the last rendered exit frame reaches `K=440`, where it is about
`-60.24 * a` px. ReverseZoom also animates
these projection parameters back toward their stable values.

This makes meaningful positive probes incompatible with the task's strict
all-context clipping constraint while keeping the formula and transition
behavior unchanged. Gating perspective during transitions would require an
additional production behavior change, which was outside the authorized scope.
Following Neal's choice, the range was not expanded.

## Files, validation, and remaining work

- No source or test changes from this follow-up were retained.
- The original nine-file portrait/perspective candidate remains untouched and
  unstaged; the index remains clean.
- Temporary expanded-candidate focused suite: **PASS**, 43 tests; it did not
  exercise animated exit/reverse projection parameters.
- Full baseline suite after restoring the prior range: **PASS**, 1,393 tests.
- `git diff --check`: **PASS**.
- No game was launched. No manual calibration was performed.
- The expanded-range prompt and this report are ignored `AI-Logs` working
  records; neither is staged or committed.

Further work would need explicit authorization either to exempt intentional
exit/reverse zoom clipping from the range-safety criterion or to gate the
experimental coefficient during those transitions.
