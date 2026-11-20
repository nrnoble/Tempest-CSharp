# Whole-Game Presentation Human-Visible Perspective Calibration

**Date:** 2026-11-14
**Status:** IMPLEMENTED — AUTOMATED VALIDATION PASS; MANUAL CALIBRATION PENDING
**Completion token:** `TEMPEST_PRESENTATION_HUMAN_VISIBLE_PERSPECTIVE_CALIBRATION_20261114`

## Scope and Git state

Removed the aesthetic parser limit from `--portrait-perspective` and established
an automated validation envelope with practical manual probes. The accepted
endpoint-normalized projection equation and presentation architecture were
left unchanged.

Starting state:

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean.
- The expected ten tracked files from the endpoint-normalized candidate were
  modified and unstaged; no non-ignored untracked files existed.
- This matched the prior implementation report and its documented candidate.

The substantive prompt was copied verbatim to
`AI-Logs/Prompts/2026-11-14-whole-game-presentation-human-visible-perspective-calibration-prompt.md`.

## Materials and preserved boundaries

Read the endpoint-normalized implementation report and relevant current
command-line, projection, display, renderer, level-configuration, and test
source. Did not read `TEMP/myNotes.md`.

Did not inspect screenshots, images, videos, gameplay recordings, MAME/C#
frames, contact sheets, Gemini material, Incitti source, web sources, or raw
ROM files. No game was launched. `TEMPEST-DEVELOPMENT-NOTES.md` and
`TEMPEST-BEHAVIOR-SPEC.md` were not changed.

## Implementation

The former parser-enforced domain was `[-5,+5]`. Parsing now uses
invariant-culture `float.TryParse` and accepts any finite value representable
as the existing `float` type. Parsing directly as `float` avoids a finite
`double` silently becoming infinity when cast.

The parser and `Projection.PortraitPerspective` setter still reject non-finite
values. Missing values, a following option instead of a value, malformed
numbers, culture-specific decimal commas, NaN, positive/negative infinity,
values overflowing the finite `float` domain, duplicate use, and combination
with `--legacy-presentation` remain rejected.

The parser-accepted domain is intentionally wider than the validated
envelope. Finite values outside the envelope are experimental and may cause
clipping, extreme or malformed presentation, or explicit projection/runtime
failure. Such behavior outside the envelope is not automatically a project
defect. No per-use runtime warning was added. The default remains `0`.

Only the parser/setter range policy and directly related tests changed. The
projection equation, baseline semantics, endpoint context, near-rim
zero-correction, X projection, line interpolation, shared rendering path,
Approach handling, and transition handling were not modified.

## Mathematical displacement and probes

The unchanged endpoint-normalized formula is:

```text
qLive(z) = 1 / (z + ZOffset)
u(z)     = (qLiveNear - qLive(z)) / (qLiveNear - qLiveFar)
q0Near   = 1 / (near + 5)
q0Far    = 1 / (far + 5)
deltaY   = coefficient * 70 * (q0Far - q0Near) * u(z)
```

For the production stable interval `near=50`, `far=450`, the host-pixel
correction per coefficient is:

| Depth | Correction (host px / coefficient) |
|---:|---:|
| Near rim, 50 | 0 |
| Intermediate, 150 | -1.0346 |
| Intermediate, 250 | -1.2578 |
| Intermediate, 350 | -1.3552 |
| Far rim, 450 | -1.4098 |

Positive values move farther geometry upward; negative values move it
downward. The prior `+5` limit moved the stable far rim only about `-7.05`
host pixels. The selected probes produce:

| Probe | Stable far-rim correction |
|---|---:|
| Neutral `0` | 0 px |
| Moderate `+20` | -28.20 px |
| Strong `+50` | -70.49 px |
| Opposite direction `-20` | +28.20 px |

For reference, the requested analysis candidates `+40`, `+60`, `+80`, and
`+100` correspond to approximately `-56.39`, `-84.59`, `-112.78`, and
`-140.98` host pixels at the stable far rim. These are mathematical estimates,
not visually or automatically accepted probes outside the envelope.

## Validated/recommended envelope and Levels 1–9 bounds

The automated validated/recommended envelope is `[-50,+50]`. Focused tests
exercise the envelope boundaries and representative interior values on
closed Levels 1 and 6 and open Level 9, with Levels 1–9 production geometry
checked for viewport bounds. The endpoint correction is linear in the
coefficient, while the shared projection tests check near/far endpoints,
intermediate line incidence, registration, and transition composition.

The portrait viewport is `x=[280,1000]`, `y=[0,960]` host pixels. The X bounds
do not vary with coefficient. Across all production Levels 1–9 and the
recommended probes, the aggregate tube bounds are approximately
`x=[288.80,991.20]`, `y=[0.14,675.62]`; aggregate clearances are 8.80 px on
each side, 0.14 px at top, and 284.38 px at bottom. The unchanged near-rim
endpoint is the topmost geometry; the far-rim correction does not move it.

Per-level host-pixel Y bounds at neutral are below. At `+20`, `-20`, and
`-50`, these min/max bounds remain the same. At `+50`, only Level 9's minimum
changes, from 292.5 px to 262.3 px; all maxima remain unchanged.

| Level | Neutral Y bounds | `+50` Y bounds |
|---:|---:|---:|
| 1 | 15.3–656.7 | 15.3–656.7 |
| 2 | 77.8–594.2 | 77.8–594.2 |
| 3 | 20.1–651.9 | 20.1–651.9 |
| 4 | 148.8–567.4 | 148.8–567.4 |
| 5 | 2.1–675.6 | 2.1–675.6 |
| 6 | 0.1–582.3 | 0.1–582.3 |
| 7 | 47.8–621.9 | 47.8–621.9 |
| 8 | 17.6–659.0 | 17.6–659.0 |
| 9 | 292.5–659.0 | 262.3–659.0 |

The production upper HUD text ends at logical y=138 (the y=120 wave line plus
its 18-unit text size), or 165.6 host pixels. At `+50`, the Level 8 far-rim
minimum is about 229.0 px and Level 9's is 262.3 px, leaving at least 63 px
of clearance from that static HUD band. The focused Level 9 test also checks
this relationship with margin. No viewport clipping occurs at the
recommended probes or envelope boundaries. Bounds describe tube geometry;
they do not certify every possible game object or arbitrary experimental
coefficient.

## Transition, Approach, lines, and registration

At stable endpoints the correction is zero at the near rim and `-70.49` host
pixels at `+50` on the far rim. The endpoint-normalized fraction keeps that
endpoint displacement bounded independently of live camera `K` and
`ZOffset`. For a representative middle depth, the correction per coefficient
is approximately `-1.2578` host pixels at stable `ZOffset=5` and
`-1.3794` at `ZOffset=-41`; it does not exceed the stable far-end magnitude.
The tests cover first/early ExitZoom samples, `ZOffset=-41/K=70`, high-K
ExitZoom (`K=440`), ReverseZoom, restoration, and deterministic frame samples.

At Approach depth offsets 32 and 960, the far-end correction is respectively
about `-0.8327` and `-0.0246` host pixels per coefficient. At `+50` these are
about `-41.63` px and `-1.23` px; completion returns to the stable
`-70.49` px. The Approach sweep verifies finite, repeatable output and
restoration without changing transition timing.

At `+50`, focused tests verify:

- Near/far/intermediate rail incidence and straightness on Levels 1, 6, and 9
  through stable, exit, reverse, and Approach contexts; perpendicular
  departure remains within the existing `0.001` projection tolerance.
- X projection remains unchanged; near-rim coordinates remain anchored.
- Player/Blaster edge, player shot, enemy shot, Fuseball center, and Level 9
  Spike start/tip remain registered to their projected tube/lane geometry
  within `0.001` logical units.
- Strong-positive Level 9 far-rim position remains below the production HUD
  band.

## Simulation neutrality and composability

A 240-step paired simulation test with coefficients `0` and `+50` confirms
identical gameplay state, recorded inputs/startup metadata, and RNG call
progression. Perspective remains presentation-only and is not recording
metadata.

Parser tests confirm composition with `--level`, `--lives`, `--speed`,
`--rim-speed`, `--record-gameplay`, `--playback-file`, deterministic visible
playback, and compatible scenario selectors. The `--legacy-presentation`
conflict remains explicit. No recording schema or playback behavior changed.

## Validation

- Focused Release tests (VSTest hang timeout 3m):
  **PASS** — 197 passed, 0 failed, 0 skipped.
- Full Release suite (VSTest hang timeout 3m):
  **PASS** — 1,411 passed, 0 failed, 0 skipped.
- Release solution build:
  **PASS** — 0 warnings, 0 errors.
- `git diff --check`:
  **PASS** (final check recorded after this report was written).

An initial focused run exposed an obsolete assertion that disallowed any
geometry from moving above its neutral minimum. The new visible probe
intentionally moves far geometry while remaining inside the viewport, so the
assertion was replaced with the relevant viewport-bound invariant; the
focused suite and full suite then passed.

## Manual calibration handoff

Run from the repository root in PowerShell; these commands were not launched:

```powershell
dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective 0
dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective 20
dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective 50
dotnet run --project src\Tempest.Game -- --level 9 --portrait-perspective -20
```

Composable Level 9 run:

```powershell
dotnet run --project src\Tempest.Game -- --level 9 --lives 8 --speed 0.5 --portrait-perspective 50
```

Playback example (replace the placeholder with a recording path):

```powershell
dotnet run --project src\Tempest.Game -- --playback-file "Temp\GameplayRecordings\<recording>.json" --portrait-perspective 50
```

Reusable Levels 1–8 pattern:

```powershell
dotnet run --project src\Tempest.Game -- --level <1-8> --portrait-perspective 50
```

The manual question is whether the endpoint-normalized perspective moves
Level 9 toward an arcade-looking-down-the-tube presentation when the change
is large enough to see immediately. Compare neutral to strong. If the
direction is correct, Neal can bracket the visually useful region manually;
do not fine-tune or change the default in code. If the direction is wrong,
stop coefficient calibration and investigate the perspective model.

Finite values outside `[-50,+50]` remain available for deliberate
experimentation. Their visual and rendering behavior is explicitly
unvalidated; clipping, extreme/malformed presentation, or rendering failure
outside the envelope is not automatically a defect.

## Final Git state

- Branch: `master`
- `HEAD` and `origin/master`: `385f603a94391756b2ca6cccd524dc17b281e782`
- Index: clean.
- The same ten expected candidate files remain modified and unstaged.
- No non-ignored untracked files.
- The prompt and report are ignored `AI-Logs` records.
- Nothing was staged, committed, or pushed.
- Endpoint-normalized formula remained unchanged.

**Remaining:** Neal's manual Level 9 visual calibration and selection of any
future non-zero project value.
