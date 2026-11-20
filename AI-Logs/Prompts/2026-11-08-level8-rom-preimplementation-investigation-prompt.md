FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, or other system-generated context may appear after the user's token
and do not invalidate it.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain that the supplied user task appears incomplete.


TASK

Perform a comprehensive but tightly bounded, read-only ROM-backed
preimplementation investigation for production Level 8.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

bc109b4597f83477f5c7998bd3a9e258c72c6edf
Close out accepted interactive speed diagnostic

Confirm the actual Git state before beginning.

This task is investigation-only.

Do not modify production code, tests, durable documentation, configuration,
project files, recordings, reference material, screenshots, or Git state.

Do not stage, commit, push, clean, move, rename, or delete anything.


PURPOSE

Levels 1-7 are the existing accepted production campaign foundation.

The next intended production milestone is Level 8.

Before implementing Level 8, derive its behavior from the Atari ROM and compare
those requirements with the existing C# architecture.

The investigation must determine:

- Level 8 tube geometry and topology;
- tube presentation/color information available from the ROM;
- enemy supply;
- starting Spikes;
- enemy-type eligibility;
- per-type minimum/maximum rules;
- total active-enemy limit;
- ordinary bottom-entry behavior;
- Tanker behavior and cargo;
- Flipper behavior and movement program;
- Tanker-cargo Flipper behavior;
- Spiker behavior;
- Pulsar behavior, if eligible;
- Fuseball/Fuzzball behavior, if eligible;
- enemy firing parameters;
- death/re-entry behavior;
- final-enemy or final-pending special cases;
- level completion;
- Level 7 -> Level 8 progression;
- Level 8 -> Level 9 boundary requirements;
- any new production mechanism Level 8 requires.

Do not implement anything yet.


PRIMARY ROM AUTHORITY

Use the project-local ROM-derived annotated disassembly:

Reference/arcade/Tempest Rom Source code.md

This is the project's established local ROM reference.

Do not search the internet for another Tempest disassembly.

Do not use Mark Incitti's implementation, another Tempest port, screenshots,
gameplay videos, MAME captures, or online sources to establish Level 8 behavior
when the local ROM material answers the question.

Atari ROM behavior and data take precedence over secondary implementations.


IMPORTANT BLIND TUBE-GEOMETRY EXPERIMENT

For this investigation, independently reconstruct the Level 8 tube from the ROM.

Neal intentionally is NOT supplying the expected visual shape of Level 8.

Do not determine Level 8 tube geometry from:

- screenshots;
- MAME;
- arcade gameplay footage;
- Mark Incitti's implementation;
- another Tempest implementation;
- an online description;
- a visual level chart;
- filenames or comments that simply describe the expected visible shape;
- prior knowledge of what Tempest Level 8 looks like.

Derive the tube from the actual local ROM geometry/topology data first.

Only AFTER the ROM derivation is complete and fully recorded may existing
project architecture or historical project reports be consulted as an
engineering cross-check.

Do not visually cross-check the derived Level 8 tube against an external
reference in this investigation.

The eventual manual implementation test will serve as the visual check.


LEVEL 8 GEOMETRY INVESTIGATION

Trace exactly how the ROM selects and constructs Level 8's tube.

Determine:

- displayed Level 8's internal/zero-based level index;
- tube/shape index;
- ROM tables used to select the tube;
- raw coordinate data;
- point/vertex data;
- lane count;
- lane ordering;
- lane endpoint relationships;
- adjacent-lane connectivity;
- whether the tube is open or closed;
- wrap behavior, if any;
- shared vertices;
- convex corners;
- reflex/concave corners;
- straight transitions;
- any unusual directed transition;
- any topology important to Flipper/Pulsar movement;
- any geometry-specific collision implications;
- tube center/projection assumptions;
- color/palette selection where established;
- any recoloring versus genuinely new geometry.

Record concrete ROM addresses, tables, indexes and raw values.

Do not merely state a descriptive shape name.

Reconstruct the geometry numerically.


ROM-TO-C# GEOMETRY TRANSLATION

After deriving the ROM topology independently, inspect the existing C# geometry
architecture.

Determine how the ROM-derived Level 8 tube can be represented using existing:

- LevelConfiguration;
- LevelLoader;
- geometry/lane definitions;
- lane endpoints;
- open/closed topology representation;
- directed pivot/turn representation;
- projection;
- collision;
- player rim movement.

Produce an explicit proposed C# geometry specification, but do not implement it.

For every proposed C# vertex or lane endpoint, identify its source derivation.

Clearly distinguish:

ROM-LISTING-BACKED:
- topology;
- lane order;
- adjacency;
- relevant relative geometry.

PROJECT TRANSLATION:
- coordinate normalization;
- screen-space scale;
- centering;
- any conversion into the existing C# geometry coordinate system.

Do not present normalized C# coordinates as literal ROM coordinates.


GEOMETRY AUDIT OUTPUT

The report must contain a geometry section sufficient for a future agent to
implement the tube without repeating the investigation.

Include:

1. ROM tube/shape selector.
2. Raw geometry table/address.
3. Raw vertices/segments.
4. Ordered lane list.
5. Connectivity map.
6. Open/closed determination.
7. Directed transition implications.
8. Proposed normalized C# coordinates.
9. Proposed C# lane definitions.
10. Evidence classification for each translation.
11. Any uncertainty.

If useful, provide a text-based coordinate/adjacency representation.

Do not create a rendered mockup or implementation in this investigation.


LEVEL 8 SUPPLY

Trace the Level 8 wave-enemy/pending-supply parameters.

Determine:

- base/default Level 8 wave supply;
- Medium behavior;
- Easy adjustment, if applicable;
- Hard adjustment, if applicable;
- whether the existing C# project should initially target the ROM Medium/base
  path consistent with current project policy;
- relationship between generic pending supply and Tanker cargo.

Do not count:

- enemy projectiles;
- Spikes;
- latent Tanker cargo

as additional original pending wave units unless the ROM explicitly does so.


LEVEL 8 ENEMY TYPE TABLES

Trace the Level 8 entries for:

- min_flippers / max_flippers;
- min_tankers / max_tankers;
- min_spikers / max_spikers;
- min_pulsars / max_pulsars;
- min_fuzzballs / max_fuzzballs;
- max_enm or equivalent global active limit;
- Tanker load type;
- Tanker cargo amount/type;
- any selection weighting/order/control flow.

Determine which species may:

- enter ordinarily from the bottom;
- exist only as cargo;
- be created by a special conversion;
- be disabled entirely.

Do not infer a fixed full-wave species distribution from concurrent min/max
tables unless the ROM actually provides one.


FLIPPERS

Level-specific Flipper behavior must be derived rather than inherited by
assumption.

Determine:

- whether ordinary Flippers can enter from the bottom on Level 8;
- whether Tanker cargo Flippers exist;
- CAM/movement program used by ordinary Flippers;
- CAM/movement program used by cargo Flippers;
- whether ordinary and cargo programs differ;
- initial handedness/direction;
- reversal pattern;
- below-rim lane-change sequence;
- transition geometry behavior;
- rim behavior;
- firing parameters;
- any Spike-sensitive behavior;
- whether Level 7's accepted 2-3 movement logic remains applicable or changes.

Do not copy Level 7 behavior merely because Level 8 is adjacent in progression.


TANKERS

Determine:

- ordinary Tanker eligibility;
- active Tanker limits;
- Tanker climb behavior;
- firing parameters;
- cargo type;
- cargo amount;
- cargo release conditions;
- cargo movement initialization;
- special final-pending conversions;
- behavior at the rim;
- death/re-entry implications.

Compare against the existing accepted C# Tanker/cargo identity architecture.

Do not redesign the Level 3-7 identity model unless Level 8 genuinely requires
a new representation.


SPIKERS AND SPIKES

Determine:

- Spiker minimum/maximum rules;
- Spiker movement parameters;
- firing behavior;
- Spike-creation behavior;
- starting Spike count;
- starting Spike-height ROM value(s);
- any Level 8-specific Spike parameter;
- final-pending Spiker behavior;
- special conversion behavior;
- exit implications.

The project currently uses a C# translation for Spike height.

Do not assume the Level 7 C# Spike height is automatically correct for Level 8.

Report the raw ROM data separately from any proposed C# translation.


PULSARS

Determine whether Pulsars are active/eligible on Level 8.

If Level 8 introduces or uses production Pulsars, trace:

- min/max selection values;
- movement;
- lane-change program;
- pulsing cadence;
- pulse hazard;
- firing;
- destruction;
- death/re-entry behavior;
- interaction with geometry;
- whether existing development/test Pulsar architecture is sufficient.

Do not assume earlier isolated Pulsar work automatically matches Level 8.


FUSEBALL / FUZZBALL

Determine whether Fuseballs/Fuzzballs are active/eligible on Level 8.

If they are:

- trace eligibility;
- creation conditions;
- movement;
- rim behavior;
- collision;
- scoring;
- firing if applicable;
- destruction behavior;
- special ROM destruction types 2-4;
- death/re-entry behavior;
- whether existing C# Fuseball architecture is suitable for production use.

Do not implement or resolve deferred Fuseball destruction graphics in this
investigation unless the Level 8 analysis requires understanding the control
flow.

Record any dependency on that deferred milestone.


ENEMY FIRING

Determine Level 8 ROM parameters relevant to enemy firing, including as
applicable:

- enemy shot capacity;
- shot timing/holdoff;
- per-species firing eligibility;
- Tanker firing;
- Flipper firing;
- Spiker firing;
- Pulsar firing;
- difficulty-dependent firing behavior.

Compare with the source-derived firing mechanisms already accepted in C#.

Do not retune earlier levels.


STARTING SPIKES

Determine Level 8's raw ROM starting-Spike parameter.

Establish:

- number of lanes receiving starting Spikes;
- raw height/depth value;
- whether every lane receives one;
- how it compares numerically with Level 7;
- whether the existing C# translation can be reused or needs a Level 8-specific
  conversion.

Do not infer a C# height solely from visual expectations.


LEVEL 7 -> LEVEL 8 PROGRESSION

Trace the ROM and current C# requirements for natural Level 7 completion into
Level 8.

Determine:

- whether the existing between-level transition architecture can be reused;
- Level 8 tube color/presentation;
- Super Zapper recharge behavior;
- score/lives carry;
- any special transition parameters;
- starting wave setup;
- starting Spikes;
- Blaster placement.

The implementation recommendation should include natural Level 7 -> 8
progression, not just direct startup.


LEVEL 8 -> LEVEL 9 BOUNDARY

Level 9 is not part of this implementation milestone.

Determine only what Level 8 completion requires architecturally.

Recommend whether the future Level 8 candidate should:

- stop at a clean terminal Level 8 boundary;
- expose a "no Level 9 implemented" development boundary;
- or reuse an existing terminal-level mechanism.

Do not implement Level 9.

Do not use Level 9 requirements to expand the Level 8 milestone.


DEATH AND RE-ENTRY

Trace the ROM behavior when the Blaster dies during Level 8.

Determine:

- how active enemies return to pending supply;
- whether species identity is preserved;
- what happens to latent Tanker cargo;
- what happens to released cargo;
- any special Spiker/Tanker conversion implications;
- whether the Level 7 generic re-entry model remains appropriate;
- whether Level 3-6 identity-preserving reconstruction would be wrong.

Recommend the smallest C# policy consistent with Level 8.


SUPER ZAPPER

Confirm Level 8 Super Zapper behavior only to the extent necessary to ensure
existing production behavior remains appropriate.

Do not redesign the accepted Super Zapper implementation unless the ROM reveals
a Level 8-specific incompatibility.


DIFFICULTY

Neal's arcade comparison target is Medium difficulty.

Trace Level 8 Medium/default parameters first.

Record Easy/Hard differences only where they materially clarify a parameter.

Do not implement a general difficulty selector as part of Level 8.


PRIOR ACCEPTED LEVELS

Levels 1-7 are frozen accepted production behavior during this investigation.

Do not recommend modifying them merely to make Level 8 implementation easier.

If the Level 8 ROM trace exposes a specific earlier-level defect:

- record it separately;
- explain the evidence;
- do not implement it;
- do not make correcting it a prerequisite unless Level 8 genuinely cannot be
  implemented correctly without doing so.


CURRENT C# ARCHITECTURE REVIEW

After establishing Level 8 ROM behavior, inspect the existing C# architecture.

Identify what Level 8 can reuse from:

- LevelConfiguration;
- LevelLoader;
- LevelProgression;
- Level;
- Level3WaveLedger;
- Level3WaveSelection;
- Level7WaveSelection;
- Flipper;
- Tanker;
- Spiker;
- Pulsar;
- Fuseball;
- Spike;
- enemy firing;
- death/re-entry;
- completion;
- renderer;
- transitions;
- recording/playback.

Identify any genuinely new production mechanism Level 8 needs.

Prefer extending existing general mechanisms when appropriate.

Do not force Level 8 into Level 7-specific code when ROM behavior differs.


PLAYBACK / TOOLING

The accepted project now includes:

- direct-level startup;
- gameplay recording;
- visible playback;
- headless playback;
- pause/resume;
- forward/grouped stepping;
- reverse/grouped scrubbing;
- playback screenshots;
- --speed diagnostic playback.

Plan Level 8 so all these tools continue to work naturally.

A future Level 8 implementation should support:

- direct Level 8 startup;
- a fresh Level 8 recording;
- headless validation;
- slow-motion observation;
- accelerated playback;
- pause/step/reverse inspection;
- screenshots.

Do not modify these systems during this investigation.


EXISTING TESTS AND REPORTS

Review relevant prior project history, especially:

- Level 7 preimplementation analysis;
- Level 7 Flipper ROM analysis;
- Level 7 implementation/correction/closeout reports;
- Level 3 mixed-wave/ledger tests and reports;
- Tanker/cargo reports;
- Spiker work;
- Pulsar analysis/implementation reports;
- Fuseball analysis/implementation reports if present;
- source-derived enemy firing work;
- Levels 4-7 production/progression tests;
- current behavior spec and development notes.

Use this material for architecture/history context only after establishing the
relevant Level 8 ROM facts.

Do not let historical C# assumptions override Level 8 ROM evidence.


EVIDENCE CLASSIFICATION

Classify every significant finding as appropriate:

- ROM-LISTING-BACKED
- RAW-ROM-DATA
- PREVIOUSLY ACCEPTED PROJECT BEHAVIOR
- EXISTING C# IMPLEMENTATION
- AUTOMATED-TEST EVIDENCE
- MANUALLY ARCADE-ACCEPTED
- PROJECT TRANSLATION CANDIDATE
- SECONDARY REFERENCE
- INTERPRETATION / HYPOTHESIS
- UNKNOWN / UNRESOLVED

Do not present project-normalized geometry, guessed movement patterns, color
names, or C# height conversions as literal ROM facts.


NO IMPLEMENTATION

Do not:

- add Level 8;
- create Level 8 geometry;
- modify LevelConfiguration;
- modify LevelLoader;
- modify LevelProgression;
- modify enemy selection;
- alter Levels 1-7;
- modify tests;
- update durable documentation;
- add diagnostics;
- launch the game;
- create new gameplay recordings;
- stage;
- commit;
- push.

This task must stop after analysis and implementation planning.


OPTIONAL READ-ONLY VALIDATION

Use code search, ROM inspection, existing tests, and small read-only
calculations as required.

Do not run the full test suite or build merely as ritual when no code changes
are made.

Do not run the interactive game.

At completion verify:

- HEAD remains unchanged;
- no tracked or staged changes were created;
- existing local/untracked material remains untouched.


AI-LOG POLICY

This instruction is explicit and must not be interpreted as conflicting:

1. Save the complete substantive user prompt under:

   AI-Logs/Prompts/

2. Create a NEW Level 8 preimplementation investigation report under:

   AI-Logs/Reports/

3. The new investigation report is expected and permitted.

4. Do NOT rewrite existing historical reports unless this task explicitly says
   to do so. This task does not.

5. AI-Logs/ is Git-ignored project history.

6. Do NOT stage or commit any file under AI-Logs/.

7. Do NOT place this milestone report under TEMP/ or Temp/.

TEMP/ and Temp/ are not the normal destination for milestone reports.


REPORT REQUIREMENTS

Create a descriptive date-prefixed report in AI-Logs/Reports/.

The report must contain:

1. Starting Git state.
2. ROM regions/tables/addresses inspected.
3. Blind Level 8 tube-geometry derivation.
4. Raw geometry values.
5. Lane count/order/connectivity.
6. Open/closed topology.
7. Proposed normalized C# geometry.
8. Geometry evidence classification.
9. Tube color/presentation data.
10. Level 8 Medium/base enemy supply.
11. Easy/Hard differences where relevant.
12. Global active-enemy limit.
13. Per-type min/max table values.
14. Ordinary bottom-entry eligibility by type.
15. Tanker/cargo behavior.
16. Ordinary Flipper behavior and movement program.
17. Cargo Flipper behavior and movement program.
18. Spiker/Spike behavior.
19. Pulsar behavior or proof of ineligibility.
20. Fuseball/Fuzzball behavior or proof of ineligibility.
21. Enemy firing parameters.
22. Starting-Spike raw ROM data and proposed C# translation.
23. Death/re-entry behavior.
24. Special final-pending/conversion behavior.
25. Level 7 -> 8 progression requirements.
26. Level 8 -> 9 terminal-boundary recommendation.
27. Existing C# architecture that can be reused.
28. Any genuinely new Level 8 mechanism required.
29. Known Level 7 behavior that must NOT be blindly copied.
30. Exact unresolved questions.
31. Proposed Level 8 implementation scope.
32. Proposed automated tests.
33. Proposed manual arcade-acceptance checklist.
34. Proposed recording/playback validation scenarios.
35. Final Git state proving no tracked changes.


IMPLEMENTATION RECOMMENDATION REQUIREMENTS

The report must conclude with a concrete bounded implementation recommendation.

Prefer a Level 8 candidate that includes:

- ROM-derived Level 8 tube;
- direct --level 8 startup using the existing generic mechanism;
- natural Level 7 -> Level 8 progression;
- ROM-derived Medium/default supply;
- correct ordinary enemy selection;
- correct Tanker cargo;
- correct Spiker behavior;
- correct Pulsar/Fuseball behavior if eligible;
- ROM-derived movement programs;
- starting Spikes;
- enemy firing;
- appropriate death/re-entry;
- clean terminal Level 8 boundary if Level 9 remains unimplemented;
- recording/playback/headless compatibility.

Do not recommend bundling unrelated earlier-level corrections.


PROPOSED AUTOMATED TESTS

Recommend tests including as appropriate:

- exact Level 8 geometry and connectivity;
- open/closed topology;
- player lane traversal;
- directed enemy pivots;
- supply;
- initial setup;
- enemy-type eligibility;
- active/type caps;
- no-direct-entry rules;
- Tanker/cargo;
- Flipper movement-program sequences;
- Spiker/Spike behavior;
- Pulsar/Fuseball behavior;
- enemy firing;
- death/re-entry;
- completion;
- Level 7 -> 8 progression;
- direct startup;
- terminal Level 8 behavior;
- deterministic recording/playback;
- headless playback;
- no regressions to accepted Levels 1-7.

Do not create these tests in this analysis task.


MANUAL ACCEPTANCE PLAN

Prepare an eventual manual Level 8 acceptance checklist that includes:

- visually compare the independently ROM-derived tube with the arcade;
- verify lane count/topology and Blaster traversal;
- verify tube color/presentation;
- verify ordinary bottom-entry species;
- verify Tankers and cargo;
- verify Flipper movement patterns;
- verify Spikers and starting Spikes;
- verify Pulsars if eligible;
- verify Fuseballs if eligible;
- verify enemy firing;
- verify Super Zapper;
- test at least one death/re-entry if practical;
- verify natural Level 7 -> 8 transition;
- verify Level 8 completion/terminal behavior;
- create a fresh recording;
- inspect difficult behaviors with pause/forward/reverse;
- optionally use --speed below 1.0 for detailed observation and above 1.0 for
  rapid replay.

Do not require Neal to manually verify invisible identity bookkeeping when
automated tests can prove it.


STOP POINT

Stop after the complete read-only Level 8 preimplementation investigation and
implementation recommendation.

Do not implement Level 8.

At completion, provide Neal a concise summary answering:

- What tube did the ROM data independently produce?
- How many lanes does it have and is it open or closed?
- What are the Level 8 supply and starting-Spike parameters?
- Which enemies can enter normally?
- What Tanker cargo does Level 8 use?
- What Flipper movement program applies?
- Are Pulsars present?
- Are Fuseballs/Fuzzballs present?
- What special final-pending behavior exists?
- What death/re-entry policy best matches the ROM?
- What existing C# mechanisms can be reused?
- What genuinely new mechanisms are required?
- Is Level 8 ready for a bounded implementation milestone?

TEMPEST_LEVEL8_ROM_PREIMPLEMENTATION_INVESTIGATION_20261108