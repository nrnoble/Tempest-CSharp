# Read-only Level 11 Spike-height and terminal-Spiker investigation

Date: 2026-11-21

Perform a tightly bounded READ-ONLY investigation of two Level 11 gameplay observations.

Do not implement corrections in this task.

Do not modify production source, tests, configuration, project documentation, reference files, Git state, or gameplay behavior.

The current uncommitted genuine Level 11 candidate is preserved.

## Current manually observed state

Neal reports:

- Level 11 presentation looks good enough with:
  - `--tube-y-gain 1.5`
  - `--tube-scale 1.1`
- The spinner-direction regression is fixed.
- The Atari-presentation model-frame crash is fixed.
- The earlier direct-Fuseball runtime-representation crash is fixed.

Two gameplay issues remain:

1. Initial Level 11 Spikes extend roughly halfway up the tube and appear significantly too long.
2. Near the end of the wave, if Neal intentionally leaves approximately two or three Spikers alive, they continue as Spikers and do not become Tankers.

Do not encode the presentation values during this investigation.

## Question 1: ROM Spike-height initialization and C# translation

The prior recovered Level 11 evidence identifies:

`wave_spikeht = $B0`

Do not stop at confirming that byte.

Determine how Atari turns `$B0` into the actual initial Spike length used by gameplay and display.

Trace, as far as authorized evidence permits:

- the `wave_spikeht` table lookup;
- how the value is copied into the 16 Spike-height entries;
- whether `$B0` is used directly or transformed;
- signed/unsigned interpretation;
- scaling, shifting, complementing, subtraction, normalization, or coordinate inversion;
- the coordinate range used by Atari Spike growth;
- the coordinate used for Spiker position versus Spike-tip position;
- how Spike height participates in collision or Flipper routing;
- whether tube depth or per-shape metadata changes visible Spike length;
- the raw Level 11 initial Spike length relative to the ROM’s full tube-depth domain.

Then inspect the current C# Level 11 implementation and determine:

- the exact C# initial Spike value created from `$B0`;
- the conversion formula;
- the provenance of that formula;
- whether it uses the same units as C# tube depth;
- whether it accidentally treats `$B0` as a direct C# height;
- whether it applies a conversion intended for another level/domain;
- whether the resulting “about halfway up the tube” length follows from that translation.

Compare against already accepted levels where ROM Spike-height bytes and C# starting Spike lengths are known.

Do not change any Spike value.

Report the exact justified Level 11 C# starting value or conversion if the evidence determines one.

If the evidence only proves that the current translation is wrong but does not uniquely establish a replacement, state that precisely.

Classify each conclusion as:

- ROM/DISASSEMBLY BACKED
- CURRENT C# BEHAVIOR
- PROJECT TRANSLATION
- MANUAL OBSERVATION
- UNRESOLVED

## Question 2: Terminal Spiker-to-Tanker behavior

Investigate Neal’s observation that the final few Level 11 Spikers can continue indefinitely without becoming Tankers.

Determine first what the original Atari game actually does.

Trace the authorized ROM/disassembly evidence for:

- Spiker creation;
- Spike growth;
- grow/retreat cycles;
- repeated cycles;
- terminal/hurry-up behavior;
- remaining-enemy or remaining-supply checks;
- any Spiker-to-Tanker conversion;
- any destruction/removal-and-replacement path that may appear visually as conversion;
- Tanker creation from a Spiker-related state;
- conditions that differ when Eggs/reserve supply are exhausted;
- conditions involving only a few unresolved enemies;
- whether the behavior is global or level-dependent.

Do not assume Spikers must become Tankers merely from Neal’s current expectation.

Determine whether that expectation is supported by ROM/disassembly evidence, accepted historical project evidence, arcade observation, or another reference implementation.

Inspect current C# production behavior across relevant levels and determine:

- whether C# contains a Spiker-to-Tanker conversion path;
- its exact trigger;
- which levels can reach it;
- whether Level 11 bypasses it;
- whether dynamic Level 11 selection limits interfere with it;
- whether persistent Spikers remain represented correctly but never satisfy a conversion condition;
- whether generic supply, Eggs, active counts, Tanker capacity, latent cargo capacity, or the all-rim completion rule affects conversion;
- whether this behavior already exists on Levels 4-9 but had not been manually exposed;
- whether Level 11’s direct Fuseball/admission changes affect the terminal condition.

Use bounded deterministic/headless observation if static inspection cannot establish the current C# behavior.

Do not launch the interactive game.

Do not alter random behavior merely to force an answer.

If practical, create no permanent test or instrumentation in this read-only task. Temporary local instrumentation must be removed before completion.

## Global-versus-Level-11 determination

Provide a direct answer:

- LEVEL-11-SPECIFIC
- GLOBAL EXISTING DEFECT
- EXPECTED CURRENT BEHAVIOR
- ROM/C# MODEL DIFFERENCE
- INCONCLUSIVE

Support the classification with exact source/lifecycle evidence.

If multiple mechanisms contribute, explain them separately.

## Historical evidence

Review relevant tracked project history, including:

- previous Spiker repeated-cycle investigations;
- terminal-Spiker diagnostics from earlier levels;
- Level 7 Spiker/Tanker work;
- Level 11 selection/admission implementation;
- all-rim completion behavior;
- hurry-up behavior;
- current Spiker and Tanker source.

Do not treat old diagnostic assumptions as accepted Atari facts.

Neal’s direct arcade observations remain the highest manual authority where applicable.

## ROM access

Prefer the tracked disassembly and existing recovered reports.

If a narrowly missing byte or routine is unavailable in tracked text and is essential to answer either question, a bounded read-only access to the already-authorized local Rev. 2 program ROM is permitted.

Do not enumerate the ROM directory.

Do not copy, modify, stage, commit, publish, or broadly mine ROM files.

Do not use `Reference/arcade/MBUCOD.V05.txt`.

## Scope exclusions

Do not change:

- genuine Level 11 geometry;
- orientation;
- spinner mapping;
- Atari model-frame mapping;
- enemy supply;
- selection limits;
- Fuseball lifecycle correction;
- Tanker-cargo `$24`/`$07` integration;
- presentation calibration;
- HUD;
- progression;
- Levels 1-10;
- direct Level 15.

Do not inspect screenshots, images, video, UserData, or Temp.

## Report requirements

Produce a focused report with:

### 1. Level 11 Spike-height chain

- ROM byte;
- Atari initialization/use;
- coordinate domain;
- C# conversion;
- cause of current visible length;
- exact justified recommendation, if available.

### 2. Terminal Spiker behavior

- Atari mechanism;
- current C# mechanism;
- conversion trigger or proof that no conversion exists;
- why the last few Level 11 Spikers remain Spikers.

### 3. Scope classification

- global or Level-11-specific;
- levels potentially affected.

### 4. Implementation readiness

For each issue, choose:

- READY FOR BOUNDED CORRECTION
- NEEDS MANUAL ARCADE OBSERVATION
- STILL UNRESOLVED
- NO CORRECTION JUSTIFIED

If correction-ready, specify the smallest evidence-supported change and regression tests, but do not implement it.

Save the prompt and report under the tracked AI-Logs convention.

Do not stage, commit, or push.

Stop after the investigation report.

Completion token:

`TEMPEST_LEVEL11_SPIKE_AND_TERMINAL_SPIKER_READONLY`
