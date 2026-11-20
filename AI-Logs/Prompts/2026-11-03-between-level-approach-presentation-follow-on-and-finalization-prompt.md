# Between-level Approach Follow-on Requirements and Finalization Record

This is a consolidated requirements record, not a verbatim transcript.
The original bounded perspective implementation prompt is preserved separately:
[2026-11-02-between-level-approach-perspective-fidelity-experiment-prompt.md](2026-11-02-between-level-approach-perspective-fidelity-experiment-prompt.md).
The final implementation, evidence, validation and acceptance history is in
[the implementation report](../Reports/2026-11-02-between-level-approach-starfield-presentation-implementation-report.md).

## Starting boundary and scope

Accepted starting checkpoint: `15933a8 Refine accepted Blaster death animation`.
Refine the existing working LevelApproach, not its architecture or projection
system. Preserve gameplay, wave freeze, progression, input restrictions,
spinner movement, Blaster/rim attachment, smooth handoff and arrival,
normal projection, HUD, and accepted 1280x960 scaling.
No general camera, particle engine, nondeterministic stars or gameplay RNG use.
No automatic staging/commit. Do not investigate `Temp/`.

## Supplied arcade evidence and starfield extension

Neal supplied actual Atari frames `Transition-frame_0038.png`,
`Transition-frame_0066.png`, `Transition-frame_0080.png`,
`Transition-frame_0110.png`, `Transition-frame_0130.png` and
`Transition-frame_0140.png`; the browser AI inspected them.

Direct visual evidence: tiny point stars against black, blue early-level
stars, distant tube surrounded by stars, stars diminishing while the tube
grows, visible tiny Blaster on the upcoming rim, and exact uppercase
`SUPERZAPPER RECHARGE`. Stars should resemble distant night-sky points, not
warp-speed streaks. Preserve normal top HUD; use existing text rendering for
the lower-central recharge message.

Neal directly observes onset around halfway through the current tube's exit;
exits 1-4 use blue stars; exit 5 onward mixes white/red/green/blue; stars are
gone shortly before arrival; spinner moves the tiny rim Blaster.
Treat qualitative timing/angles as qualitative, not measured constants.

Implement a compact deterministic presentation spanning ExitZoom and
Approach without collision, scoring, gameplay entities or RNG consumption.
Exact count, coordinates, deterministic distribution, activation/removal
ticks, fade algorithm and later color proportions are PROJECT DECISIONS.
Preserve the 96-unit approach-only bias unless integration shows a concrete
problem. Do not implement broader 16-level palette recycling or later levels.

## Distance and timing refinement

Neal tested the combined presentation and found it very good. Remaining
requests: incoming tube initially farther away and approximately four seconds
from first visible exit stars through arrival.

Inspect the actual runtime cadence, rather than assuming a tick conversion.
Prefer tuning initial presentation depth over shrinking tube geometry.
Preserve the starfield, perspective, message, spinner, smooth arrival and
normal projection. Tune only presentation duration/distance as necessary.
Tests must trace actual onset and arrival, protect monotonic tube growth,
rim attachment, exact normal projection/zero bias, disappearance before
arrival, gameplay freeze and progression.

Implemented/accepted choices: depth 960 instead of 240; 181 approach updates
instead of 120; first visible star at ExitZoom update 65; commit on update
124; 59 remaining exit intervals plus 181 approach updates = 240 intervals
at the normal one-update-per-frame 60-FPS cadence.

## Density-only comparison

Neal found the distance/timing presentation close to perfect. Change only
192 stars to approximately 288 (+50%), allowing only strictly necessary
mechanical generator adjustments. Preserve timing, positions algorithm,
point size, colors, distance, bias, message, input, HUD and all gameplay.
If overly dense, choose between 192 and 288 rather than arbitrary retuning.

Manual result: 288 looks much better; keep 288 and do not retune density.

## Brightness-only comparison

Neal requested visibly brighter points while preserving 288 count,
distribution, point size, blue/later four-color policy, exact visibility
boundaries, continuity, four-second journey, 960 depth, 96 bias and all
other presentation/gameplay behavior.

Inspect alpha/intensity first. Brighten in one centralized place; no larger
points, halos, sprites, streaks or particles. Preserve the visibility envelope.
Exact brightness is project tuning, not measured Atari intensity.

Implemented choice: clamped 1.6x RGB gain; alpha unchanged. White is already
full intensity. Tests protect counts, colors, coordinates, alpha envelope,
first-star timing, pre-arrival disappearance and four-second interval.

## Final manual acceptance and finalization

Neal reports the current combined presentation looks very good and should
be retained. Accepted: 288 deterministic points, centralized 1.6x RGB gain,
960 depth, 96-unit bias, approximately four seconds at normal 60-FPS cadence,
late-exit onset, seamless field continuity, disappearance before arrival,
blue exits 1-4/later four-color policy, exact recharge wording, distant tube,
tiny visible spinner-controlled Blaster and smooth normal gameplay arrival.

No further transition visual tuning. The project is not a frame-perfect
clone: aim for Neal-accepted arcade fidelity with reasonable compromises.
Do not chase exact equivalence after acceptance.

Preserve the implementation exactly. Update README, behavior specification,
development notes and existing report to record acceptance, keeping directly
observed arcade behavior separate from project count/distribution/gain/
depth/bias/threshold/tick/interpolation choices. Verify records cover the
requirements, evidence, implementation and validation history.

Review the intentional `.gitignore` maintenance and include it in the
recommended staging boundary if the change is only the established `Temp/`
entry. Do not alter other ignore rules.

Final validation must actually execute: focused transition/approach,
affected progression/rendering, complete solution tests, complete solution
build and `git diff --check`. Do not bypass Windows Application Control or
interpret a no-tests-executed exit code as passing. Inspect the complete diff.

Report all changed files, accepted choices, actual test counts/results,
build warnings/errors, diff check, final Git status, unrelated-change review,
an exact recommended `git add` command and concise commit message:
`Refine accepted between-level approach presentation`.

Do not stage, commit or start another milestone.
