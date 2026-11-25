Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, ROM evidence, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with `TEMPEST_` appears on a line by itself in the user-provided task.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Verify and correct Level 15 starting Spike length

Continue from the current uncommitted production Level 15 candidate.

Neal has manually tested Level 15.

Overall gameplay is good.

The Level 15 Atari presentation currently looks good with:

- tube Y gain `1.15`
- tube scale `1`
- tube vertical offset `75`
- tube depth-Y gain `1`

Do not alter or encode those presentation values during this task.

One gameplay/presentation issue remains:

**The starting Spikes are significantly too long.**

This resembles the previously diagnosed starting-Spike translation issue from an earlier production level, especially Level 11.

Perform a tightly bounded verification and, if confirmed, implement the smallest correction.

Do not perform a broad Spike investigation.

## Objective

Answer:

1. What is Level 15's genuine ROM-backed starting-Spike value?
2. What does that ROM value semantically represent?
3. How does the current Level 15 candidate convert it into C# world Spike height?
4. Is Level 15 incorrectly treating a ROM along-tube tip coordinate as a direct Spike length or otherwise bypassing the established conversion?
5. What exact C# starting Spike height follows from the already-established project translation?

If the current Level 15 conversion is wrong and the established translation unambiguously determines the correct value, implement the correction immediately.

If the evidence contradicts the established translation or does not uniquely determine a value, stop and report the specific conflict instead of guessing.

## Reuse established Spike-coordinate semantics

Do not rediscover the entire Spike model.

Review the accepted projects/tests/reports that established the Spike initialization rule on Levels 11-14.

The previously established model distinguishes:

- ROM starting-Spike value as an Atari along-tube Spike-tip coordinate where applicable;
- Atari far/start coordinate;
- Atari playable along-tube span;
- C# `Level.Depth`;
- resulting C# world Spike length.

Where Level 15 uses the same ROM domain, reuse the established conversion.

Do not introduce a Level-15-specific arbitrary visual constant.

## Established translation to verify

For the known Atari along-coordinate model, the project translation has the form:

`height = (farAlong - rawSpikeTipAlong) * Level.Depth / (farAlong - nearAlong)`

with the established Atari domain values used by the accepted prior levels.

Verify the exact existing project helper/formula and constants from current source/tests rather than blindly copying this prose.

If Level 15 belongs to this same semantic domain, use the existing helper or centralize through the existing established implementation.

Do not create a duplicate formula solely for Level 15.

## Recover Level 15 raw value

Prefer current tracked project evidence.

If the Level 15 raw starting-Spike value is already represented in current candidate source/tests/reports, use it.

If genuinely missing, use only the already-authorized bounded ROM/disassembly evidence path necessary to recover that single value.

Do not enumerate ROM directories.

Do not broadly mine ROM tables.

Do not use unrelated visual assets.

Do not inspect screenshots or videos.

Neal's observation that the visible starting Spikes are too long is sufficient manual evidence of the symptom.

## Compare with current Level 15 behavior

Identify exactly:

- raw Level 15 value;
- current C# starting height;
- current conversion path;
- established expected conversion path;
- resulting corrected height if applicable;
- ratio of current versus corrected C# height where useful.

Do not infer visual length from screenshots.

Use numeric source/runtime evidence.

## Scope classification

Classify the result as exactly one:

- LEVEL-15 SPIKE TRANSLATION OMISSION
- SHARED SPIKE TRANSLATION DEFECT
- CURRENT LEVEL-15 VALUE IS CORRECT
- EVIDENCE CONFLICT — STOP

If the issue exists only because Level 15 failed to use an already-correct shared conversion, classify it as a Level-15 integration omission rather than a global regression.

## Implementation

If correction-ready:

- preserve the raw ROM Level 15 starting-Spike value;
- derive C# height from the accepted conversion;
- use that converted value for Level 15 Spike initialization;
- keep Spike growth/collision/routing semantics unchanged;
- keep Spike count/lane ownership unchanged;
- do not modify renderer projection to disguise the length;
- do not alter Level 15 tube presentation.

Prefer one established conversion helper shared with prior levels.

Do not modify accepted prior Level Spike values.

## Preserve Level 15 accepted candidate

Do not change:

- genuine Level 15 row-10 geometry;
- topology;
- spinner behavior;
- enemy selection;
- Flipper movement;
- Tanker cargo;
- Spikers;
- Fuseballs;
- score;
- lives;
- Super Zapper;
- HUD;
- progression;
- presentation metadata;
- model frame.

Preserve Neal's current presentation candidate:

- Y gain `1.15`
- scale `1`
- vertical offset `75`
- depth-Y gain `1`

Do not encode those values yet.

## Preserve global accepted systems

Do not change:

- Levels 1-14;
- Fuseball animation;
- Fuseball vector bonus;
- Fuseball terminal behavior;
- terminal Spiker-to-Tanker conversion;
- open-rim Flipper correction;
- Atari HUD;
- Blaster-death baseline;
- background-opacity diagnostic;
- playback;
- Approach;
- ExitZoom.

## Automated tests

Add focused regression coverage proving:

1. Level 15 preserves the genuine raw starting-Spike ROM value.
2. The raw value's semantic domain is explicitly represented/documented.
3. Level 15 uses the established Spike-tip-to-C#-length conversion.
4. Level 15 no longer treats the raw tip coordinate as direct C# length if that was the defect.
5. The resulting C# height matches the established formula.
6. All playable initial Level 15 Spikes use the corrected height.
7. Spike lane ownership/count remain unchanged.
8. Spike growth remains unchanged.
9. Spike depletion remains unchanged.
10. Spike collision/routing behavior remains unchanged.
11. Presentation calibration does not modify gameplay Spike height.
12. Accepted prior-level Spike-height tests remain unchanged.
13. Level 15 headless production state remains deterministic.
14. No gameplay identity/ledger regression occurs.

Use appropriate floating-point tolerance.

## Validation

Run bounded:

1. new Level 15 Spike-height regression;
2. existing Spike translation tests;
3. Level 15 focused production tests;
4. Spiker/Spike behavior tests;
5. Flipper Spike-sensitive routing tests where relevant;
6. ledger/reconstruction tests;
7. Level 15 deterministic headless production soak if already present in the candidate;
8. Levels 1-14 Spike regressions;
9. complete solution test suite;
10. Release solution build;
11. `git diff --check`.

Use explicit hang/time bounds.

Do not launch the interactive game.

## Manual handoff

After automated validation provide the normal Level 15 command and Atari-presentation command using Neal's current calibration candidate.

Neal should judge only:

- starting Spikes are now substantially shorter and plausible;
- Spike growth remains normal;
- Spiker interaction remains normal;
- gameplay remains otherwise unchanged.

Do not require presentation recalibration in this task.

## Records and Git

Save this prompt and paired correction report under tracked AI-Logs.

Do not stage, commit, or push.

Preserve unrelated tracked/untracked material and any pending post-push report updates.

Stop after the bounded Spike verification/correction, validation, and manual handoff.

TEMPEST_LEVEL15_SPIKE_HEIGHT_CORRECTION