IMPORTANT FAIL-CLOSED INSTRUCTION

Read this entire user-provided task before doing anything.

Before using any tools, reading project files, running commands, or changing the repository, locate the unique completion token included near the end of this user-provided task.

The completion token begins with "TEMPEST_" and appears on a line by itself.

Verify that exactly one complete token beginning with "TEMPEST_" is present in the user-provided task.

VS Code or the agent environment may append reminders, tagged-file information, attachment metadata, or other system-generated context after the user-provided task. Such automatically appended material does not invalidate the completion token.

If exactly one complete TEMPEST_ token cannot be found, STOP before any project or repository action and explain why.

PROJECT

Repository:
C:\Users\nrnob\OneDrive\Repro\Tempest

Required starting checkpoint:
79e33886772424a31973332f33f3781b55e4c2f8

PURPOSE

Perform a read-only preimplementation analysis for:

Production Level 7 and natural Level 6→7 progression.

Do NOT implement Level 7 in this task.

Do NOT modify production code or tests.

Do NOT launch the interactive game.

Follow the existing project-wide prompt/report logging requirements.

STEP 1 - VERIFY BASELINE

Run:

Set-Location 'C:\Users\nrnob\OneDrive\Repro\Tempest'
git rev-parse HEAD
git status --short --branch
git diff --stat
git diff --cached --stat

HEAD must exactly equal:

79e33886772424a31973332f33f3781b55e4c2f8

Tracked worktree and index must be clean.

Otherwise STOP without altering anything.

STEP 2 - READ CURRENT PROJECT AUTHORITY

Read the repository copies of:

.github/copilot-instructions.md
TEMPEST-DEVELOPMENT-NOTES.md
TEMPEST-BEHAVIOR-SPEC.md
README.md

Treat Levels 1–6 as the accepted production foundation.

Read the Level 6 implementation report and relevant Level 6 code/tests as useful immediate precedent.

Do not reopen accepted Levels 1–6 behavior unless Level 7 exposes a narrowly defined reusable limitation.

STEP 3 - ESTABLISH LEVEL 7 REFERENCE CONFIGURATION

Using the checked-in Incitti/reference material and Atari ROM source where useful, determine the Level 7 reference data.

Establish:

- exact board/tube identity and geometry;
- whether the tube is closed or open;
- vertex and playable-lane counts;
- exact reference coordinates;
- center/offset/projection-relevant data;
- palette/color group;
- player starting lane/position;
- enemy supply;
- initial and reserve Egg counts;
- Flipper eligibility;
- Tanker eligibility and cargo;
- Spiker eligibility;
- Fuseball eligibility;
- Pulsar eligibility;
- starting Spikes;
- exit policy;
- Avoid Spikes interval if applicable;
- Flipper climb/turn/pause timing;
- production enemy-firing values;
- any level-specific Tanker or Spiker timing;
- Super Zapper state;
- reference behavior after successful Level 7 completion.

Do not extrapolate Level 6 values.

Explicitly distinguish:

1. REFERENCE VERIFIED information.
2. ROM-backed information.
3. Existing accepted C# behavior reusable unchanged.
4. PROJECT DECISION behavior.
5. Unknown behavior requiring manual/arcade evidence.

STEP 4 - INVESTIGATE LEVEL-SPECIFIC FLIPPER MOVEMENT

This is an important new fidelity requirement.

Do NOT assume that enabled below-rim Flipper lane changing means one universal generic movement policy across all tubes.

Neal's established observation:

- Level 1 Flippers climb straight up their lanes and do not change lanes below the rim. The current Level 1 implementation is correct.
- Other Tempest tubes may have characteristic lane-change behavior as Flippers climb.
- A tube might favor or require particular directions or sequences rather than merely allowing arbitrary generic lane changes.
- Pulsars may eventually require the same type of tube-specific investigation, but this Level 7 analysis should focus primarily on Flippers unless Pulsar eligibility makes Pulsar behavior immediately relevant.

Investigate Level 7 specifically.

Trace Incitti's Flipper logic and all relevant level/board/geometry inputs.

Determine:

- whether Level 7 has only a generic canflip enable/disable rule;
- whether lane choice is influenced by tube topology;
- whether direction is influenced by player position;
- whether direction is random;
- whether direction persists;
- whether a recognizable sequence can emerge from geometry/state;
- whether there is explicit board/level-specific logic;
- whether the reference contains any table or data controlling movement by level;
- whether the resulting movement pattern should differ visibly from Levels 2–6.

Do not invent a pattern.

If static source inspection cannot establish the characteristic arcade Level 7 pattern, say so explicitly.

If useful, identify what Neal should observe in arcade footage to resolve the uncertainty.

The game's accepted recorded-input playback may later help Neal observe C# enemy motion without concentrating on playing, but do not modify replay infrastructure in this task.

STEP 5 - CHECK WHETHER CURRENT LEVELS NEED FUTURE REVISIT

Without changing anything, determine whether the existing Levels 2–6 all currently share a generic Flipper below-rim movement policy where the reference or arcade evidence suggests possible tube-specific distinctions.

Do NOT declare existing accepted behavior wrong merely because the implementation is shared.

Classify findings as:

- known correct;
- reference-consistent but not arcade-verified;
- potentially needing future fidelity investigation;
- concrete demonstrated defect.

Only a concrete demonstrated defect should be treated as requiring immediate correction.

The purpose is to prevent future levels from blindly inheriting a generic movement assumption.

STEP 6 - AUDIT LEVEL 7 C# IMPLEMENTATION SEAMS

Inspect the minimum relevant production code and tests.

Determine:

- where Level 6 currently terminates;
- required Level 7 configuration additions;
- progression resolver changes;
- direct-level diagnostic implications;
- Flipper tuning/range changes;
- enemy-firing configuration changes;
- Spike/exit configuration changes;
- geometry support;
- renderer assumptions;
- wave/ledger compatibility;
- reconstruction compatibility;
- campaign-soak extension;
- presentation/approach compatibility.

Identify any hard-coded Level 1–6 limits.

Do not redesign working systems.

STEP 7 - DEFINE THE SMALLEST IMPLEMENTATION MILESTONE

Recommend ONE bounded Level 7 implementation milestone.

Preferred shape if supported by evidence:

- production Level 7 configuration;
- natural Level 6→7 progression;
- exact reference-backed per-level timing/configuration;
- direct --level7-test production-equivalent diagnostic if useful;
- terminal Level 7;
- focused automated coverage;
- campaign soak through Level 7;
- manual acceptance afterward.

However, if Level 7 Flipper movement requires a new tube-specific movement capability that cannot safely be specified yet, explicitly recommend resolving that as a prerequisite rather than guessing.

Do not implement Level 8.

STEP 8 - EXPLICIT EXCLUSIONS

Unless Level 7 itself requires them, keep out of scope:

- Level 8+
- open-tube architecture for later levels
- Pulsars before their established production eligibility
- Fuseballs before established production eligibility
- broad Flipper/Pulsar redesign
- speculative tube-specific movement patterns
- replay/attract-mode changes
- high scores
- end-game animation
- bonus-life work
- deferred enemy-destruction sound-volume balancing
- spinner retuning
- broad rendering-fidelity work
- unrelated refactoring

STEP 9 - PROPOSE VALIDATION

Recommend focused automated validation for the eventual implementation.

Include:

- exact Level 7 configuration;
- exact geometry;
- timing/firing values;
- enemy eligibility;
- Spike/exit policy;
- natural Level 6→7 progression;
- score/lives carry;
- fresh Super Zapper;
- fresh wave/accounting;
- death/re-entry;
- terminal behavior;
- direct-access parity if added;
- preservation of Levels 1–6;
- campaign soak through Level 7.

Also propose a concise manual checklist emphasizing:

- Level 7 visual identity;
- Flipper below-rim movement pattern;
- whether that movement looks characteristic of the tube rather than obviously generic;
- enemy composition;
- firing pressure;
- Tanker/cargo and Spiker behavior where applicable;
- approach;
- Spike exit if applicable;
- death/re-entry;
- terminal behavior.

STEP 10 - FINAL REPOSITORY CHECK

Run:

git status --short --branch
git diff --stat
git diff --cached --stat

No production/test/project-document/reference modifications are permitted.

Account separately for ignored prompt/report files required by project instructions.

FINAL REPORT

Report:

1. Verified starting HEAD.
2. Exact Level 7 tube/geometry.
3. Level 7 enemy eligibility and production composition rules.
4. Level 7 Spike/exit policy.
5. Level 7 Flipper timing.
6. Level 7 firing parameters.
7. Level 7 Super Zapper/fresh-state requirements.
8. Natural Level 6→7 requirements.
9. Reference behavior after Level 7.
10. Detailed findings on Level 7 below-rim Flipper movement.
11. Whether the reference establishes a level/tube-specific Flipper pattern or only a more general movement mechanism.
12. Any implications for previously accepted Levels 2–6, without changing them.
13. Existing C# functionality reusable unchanged.
14. Exact C# limitations blocking Level 7.
15. Unknowns requiring arcade/video/manual evidence rather than guessing.
16. Recommended smallest Level 7 implementation scope.
17. Explicit exclusions.
18. Recommended automated validation.
19. Recommended manual acceptance checklist.
20. Final Git state and confirmation that Level 7 was not implemented.

Stop after analysis.

Wait for Neal's review before implementation.

TEMPEST_LEVEL7_PREIMPLEMENTATION_ANALYSIS_79E3388
