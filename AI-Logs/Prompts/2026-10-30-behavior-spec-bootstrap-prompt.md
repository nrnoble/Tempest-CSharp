Create the initial Tempest behavior specification.

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

IMPORTANT:
This task is DOCUMENTATION ONLY.

Do not modify gameplay code, tests, project files, README, configuration, or Git state.
Do not commit anything.

Create:

TEMPEST-BEHAVIOR-SPEC.md

at the repository root.

PURPOSE

This document is the durable behavioral specification for the Tempest recreation.

It must describe WHAT the game is intended to do, not HOW the C# implementation happens to accomplish it.

The document must remain useful:
- years from now;
- to another developer unfamiliar with the code;
- to an AI working on the project in a future conversation;
- for detecting regressions or disagreements between arcade behavior, reference behavior, implementation, and tests.

SOURCE PRIORITY

Use these evidence classifications consistently:

1. ARCADE VERIFIED
Behavior Neal has actually observed/validated against Atari Tempest.

2. REFERENCE VERIFIED
Behavior explicitly established by the Incitti/reference source but not yet confirmed against Atari arcade behavior.

3. PROJECT DECISION
An intentional decision made for this recreation where arcade/reference evidence is incomplete, or where the C# architecture deliberately differs.

4. DEVELOPMENT ONLY
Behavior existing solely for development/testing, including scenario flags, extra lives, deterministic random seams, artificial wave composition, and test instrumentation.

Do not promote behavior to ARCADE VERIFIED merely because:
- Incitti implements it;
- C# implements it;
- an automated test passes;
- an AI report claims it.

If the evidence is uncertain, use the weaker classification.

RESEARCH

Inspect:
- current C# source
- SimulationTests.cs
- README.md
- existing project documentation
- Reference/incitti sources
- relevant AI-Logs/Reports milestone reports

Especially inspect the reports for:
- Flipper/enemy bullet work
- Spiker/spike work
- Tanker/cargo work
- death/re-entry corrections
- Fuseball work
- Pulsar analysis
- Pulsar implementation and renderer correction

Use reports as historical evidence, but verify important behavior against current code/reference where practical.

SPECIFICATION STRUCTURE

Begin with:

# Tempest Behavior Specification

Then include:

## 1. Purpose and evidence model
Explain the four classifications and the evidence priority.

## 2. Global gameplay behavior
Cover only behavior currently established well enough to document:
- player/blaster movement
- firing
- scoring
- lives/death
- enemy reconstruction after death
- Egg/release behavior
- level/wave completion
- tube exit behavior

## 3. Flippers

## 4. Enemy bullets

## 5. Spikers and spikes

## 6. Tankers and cargo Flippers

## 7. Fuseballs

## 8. Pulsars

## 9. Rendering/visual behavior that affects gameplay

## 10. Development-only scenarios

## 11. Known uncertainties and deferred behavior

## 12. Behavior-validation history

For each gameplay section, organize behavior naturally around topics such as:
- creation/entry
- movement
- player interaction
- player-shot interaction
- scoring
- death/re-entry
- wave accounting/completion
- visual behavior
but do not force empty subsections.

PULSAR MANUAL VALIDATION

Record Neal's current manual validation accurately.

Neal manually verified:
- Pulsars appear as six-segment zigzags across their lanes.
- Pulsars climb and descend and can pause/transition into neighboring lanes.
- During a pulse the Pulsar appearance changes and the affected lane's radial boundaries flash.
- The pulsing lane is hazardous to the blaster even when the Pulsar itself is farther down the tube.
- Rim contact destroys the blaster.
- Pulsars can be shot and award 200 points.
- After life loss unresolved Pulsars return through the Egg/re-entry behavior while already destroyed Pulsars remain destroyed.
- Destroying all Pulsars allows the established tube-exit sequence.
- The isolated Pulsar scenario does not spawn unrelated enemy types.

Neal discovered and then manually revalidated this arcade behavior:
- During an active Pulsar pulse, the near/top rim connection spanning the affected Pulsar lane disappears.
- It returns when the pulse ends.
- Unaffected near/top rim segments remain visible.
- The affected lane's radial boundaries continue flashing.
- Occupying that lane during the pulse destroys the blaster.

The initial C# implementation incorrectly left this near/top rim segment visible.
That rendering behavior was corrected and Neal manually verified the correction.

Do NOT automatically label exact Pulsar timing constants such as:
- 51-update decision cadence
- 181-update oscillator cycle
- 18/14 pause values
as ARCADE VERIFIED unless historical project evidence explicitly shows Neal validated those exact timings against Atari hardware/gameplay.
Otherwise classify them appropriately as REFERENCE VERIFIED or PROJECT DECISION.

FUSEBALL

Use the completed Fuseball milestone evidence, but maintain the same conservative classification discipline.
Manual gameplay validation established the observed behaviors Neal actually reported; precise source timing/math not manually established against arcade remains REFERENCE VERIFIED where appropriate.

DEVELOPMENT SCENARIOS

Clearly separate flags such as:
--spiker-test
--tanker-test
--fuseball-test
--pulsar-test
--enemy-bullets-test
--lives

These are DEVELOPMENT ONLY and must not be mistaken for authentic arcade wave composition.

DO NOT TURN THIS INTO IMPLEMENTATION DOCUMENTATION

Avoid details such as:
- private method names
- collection implementation
- C# inheritance structure
- renderer callback architecture
- test helper classes

unless absolutely necessary to explain a deliberate PROJECT DECISION.

The specification should remain valid even if the game implementation is later refactored.

VALIDATION HISTORY

Add concise milestone/history entries recording:
- behavior established;
- evidence classification;
- important arcade corrections.

Include the Pulsar top-rim correction because it demonstrates why arcade observation outranks assumptions/reference interpretations.

MAINTAINABILITY

Keep wording precise and factual.
Explicitly distinguish:
- known behavior;
- reference-derived behavior;
- recreation decisions;
- test/development conveniences;
- unresolved questions.

Avoid claiming arcade fidelity where it has not been demonstrated.

AFTER WRITING

Review the entire document once for accidental overstatement, especially ARCADE VERIFIED labels.

Save this prompt to:

AI-Logs/Prompts/2026-10-30-behavior-spec-bootstrap-prompt.md

Save a short report explaining how the specification was constructed to:

AI-Logs/Reports/2026-10-30-behavior-spec-bootstrap-report.md

The report should identify:
- files/reports consulted;
- areas where evidence was insufficient for ARCADE VERIFIED;
- conflicts discovered between historical analysis, Incitti, current C#, or Neal's observations;
- any specification sections intentionally left incomplete.

Do not change tests or implementation to make them agree with the specification.
If a disagreement is discovered, document it for review instead.

STOP after creating the specification, prompt archive, and report.

Do not commit.
Do not start another milestone.
