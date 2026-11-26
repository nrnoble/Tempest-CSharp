# Tempest Level 17 Bounded Production Implementation

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, modifications, tests, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code/agent-generated reminders, tagged-file information, attachment metadata, current-editor context, or other system-generated context appearing after the user's completion token do not invalidate it.
5. This check exists only to detect an incomplete or truncated user task, not appended agent context.
6. If exactly one complete standalone `TEMPEST-END-` token cannot be found, STOP before any project or repository action and explain why.

Do not require an older underscore-based `TEMPEST_...` token.

The unique completion token for this task is the final standalone line below.

---

# Task

Implement **Level 17 production gameplay and presentation** as one bounded medium/default milestone.

This implementation must follow the completed ROM-to-C# readiness specification.

Implement:

- canonical Tube Shape 1 reuse;
- second-cycle palette identity/data;
- Level 17 medium/default configuration;
- straight `$07` Flipper/Tanker-cargo behavior;
- Pulsars as normal production original identities;
- Pulsar ledger/reconstruction/completion integration;
- Tanker cargo as two Flippers;
- Level 17 firing/shot capacity/holdoff;
- Level 17 translated movement values;
- natural Level 16→17 progression;
- direct Level 17 startup;
- terminal Level 17 boundary;
- focused and full automated validation.

Do not implement Level 18.

Do not implement a general 99-level campaign.

Do not implement a multi-level generator.

Do not implement full easy/hard difficulty selection.

Do not retune Levels 1–16.

Do not stage, commit, or push.

Do not launch the interactive game.

Stop after automated validation and provide Neal a manual Level 17 acceptance matrix.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-rom-reuse-color-pulsar-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-missing-rom-palette-speed-followup-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-map-final-evidence-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

Use the final integration-readiness report as the implementation specification.

When reports conflict, use the latest corrective evidence.

Do not repeat the investigations.

---

# Starting-state protection

Before editing, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files relevant to the task.

Preserve pre-existing unrelated changes, including any previously documented behavior-specification hunk and untracked local reference material.

Do not:

- clean;
- reset;
- restore unrelated work;
- overwrite historical AI reports;
- modify local ROM/reference files;
- stage;
- commit;
- push.

AI-Logs created by this task are continuity records, but this implementation does not authorize Git publication.

---

# Evidence classification

Preserve these classifications explicitly.

## ROM-BACKED

- Level 17 resolves to canonical shape row 0;
- palette block `00 07 0B 08 44 03 0C 0C`;
- medium supply 20;
- Pulsar minimum 2, maximum 5;
- medium Pulsar firing disabled;
- Flipper program `$07`;
- raw Flipper/Tanker speed `$AF`;
- Tanker cargo `$01` = two Flippers;
- shot holdoff `$20` = 32 ROM ticks;
- medium shot maximum index `$02` = three slots;
- starting Spike height `$A0`;
- Fuseballs absent;
- Spiker raw speed input `$00`;
- Pulsar fixed speed `$FE.A0`;
- Pulsar flip rate `$28`;
- pulse beat `$04`.

## PROJECT TRANSLATIONS / DECISIONS

- C# Flipper/Tanker speed `2.53125f`;
- C# Spiker speed `3f`;
- C# enemy-shot speed `3f`;
- current C# Pulsar climb/pause/pulse-cycle behavior retained initially;
- Pulse rate/beat exposed as Level 17 configuration;
- Level 17 medium-only production support;
- existing host role colors retained initially while raw palette data and cycle identity are added;
- Level 1 calibration inherited initially for Level 17;
- Level 17 remains terminal.

Do not relabel project translations as literal ROM behavior.

---

# Phase 1: Canonical tube-shape reuse

Level 17 must reuse canonical Level 1/shape-row-0 geometry.

Requirements:

- do not create a copied Level 17 point array;
- do not duplicate Level 1 geometry constants;
- resolve Level 17 to shape row 0 through a maintainable 16-shape cycle/remap mechanism;
- preserve Level 1 geometry object/data as the canonical source;
- preserve existing Levels 1–16 behavior;
- keep displayed-level identity separate from shape identity.

Add tests proving Level 17 obtains the same canonical geometry definition/data as Level 1 rather than an independently copied array.

Do not implement Level 18–99 mappings beyond the smallest general cycle function needed to resolve Level 17 safely.

If a generalized modulo-16 resolver is added, do not expose unsupported later gameplay levels through startup/progression.

---

# Phase 2: Presentation/model-frame/calibration reuse

Enable Level 17 presentation using Shape 1's accepted presentation architecture.

Level 17 should initially inherit Shape 1:

- model-frame mapping;
- Y gain;
- scale;
- vertical offset;
- depth-Y gain;
- relevant local entity scaling.

Requirements:

- resolve calibration through shape identity or a bounded fallback to the canonical Level 1 calibration;
- preserve existing displayed-level calibration values for Levels 1–16;
- keep CLI `--tube-*` overrides absolute and functioning;
- do not alter the accepted default production presentation;
- do not perform new visual calibration.

Add tests showing:

- Level 17 uses Shape 1 defaults initially;
- Level 1 remains unchanged;
- Level 17 overrides affect Level 17 normally;
- presentation projection is finite for tube, Blaster, enemies and shots.

Neal will decide whether inherited placement is visually acceptable.

---

# Phase 3: Second-cycle palette catalog

Add a bounded cycle palette representation.

Requirements:

- preserve raw first-cycle block:
`00 04 08 0C C3 07 0B 0B`
- add raw Levels 17–32 block:
`00 07 0B 08 44 03 0C 0C`
- associate Level 17 with palette index/cycle 1;
- keep palette identity independent from shape identity;
- retain current accepted entity-role host colors initially;
- retain Pulsar cyan/pulse-white presentation;
- retain Super Zapper temporary rail-color cycling;
- do not claim exact host RGB is ROM-backed;
- do not fit colors from inaccessible screenshots.

Add tests for:

- exact raw blocks;
- nibble arrays if exposed;
- Level 1 palette 0;
- Level 17 palette 1;
- shape row 0 shared while palette differs;
- no Levels 1–16 color regression;
- Super Zapper remains functional with Level 17 palette selection.

If the raw palette catalog is not yet consumed by every renderer role, document that bounded project-translation boundary rather than inventing semantic role mappings.

---

# Phase 4: Level 17 production configuration

Create one shared Level 17 production configuration used by natural and direct startup.

Medium/default values:

- displayed level: 17;
- canonical shape: row 0;
- palette index: 1;
- ordinary supply: 20;
- physical enemy envelope: existing seven-record project translation;
- Flippers: minimum 0, maximum 3;
- Tankers: minimum 1, maximum 1;
- Spikers: minimum 0, maximum 2;
- Pulsars: minimum 2, maximum 5;
- Fuseballs: minimum 0, maximum 0;
- raw Spike height: `$A0`;
- translated Spike height: existing formula, approximately `142.857`;
- Flipper/Tanker climb speed: `2.53125f`;
- Spiker speed: `3f` project translation;
- enemy-shot speed: existing `3f` project translation;
- enemy-shot holdoff: 32;
- enemy-shot capacity: 3;
- Pulsar firing: disabled;
- Tanker cargo: two Flippers;
- Flipper movement: straight `$07`;
- Level 17 terminal after completion.

Keep hard/easy ROM facts available as named constants or comments/tests only where maintainable, but do not implement player-selectable difficulty.

Do not add Pulse/Fuseball content that Level 17 excludes.

---

# Phase 5: Level 17 wave selection

Implement a dedicated bounded Level 17 ROM-wave policy.

The selector must:

- conserve exactly 20 original wave identities;
- guarantee at least 2 Pulsar originals;
- allow no more than 5 Pulsar originals;
- guarantee exactly 1 Tanker original under the established min/max policy;
- allow up to 3 ordinary Flippers;
- allow up to 2 Spikers;
- exclude Fuseballs;
- use deterministic seeded randomness;
- respect the existing seven-record active/physical envelope;
- account for Tanker latent cargo without allocating duplicate identities;
- avoid hard-coding one predetermined full wave sequence unless the existing selector pattern requires a deterministic policy translation.

Use the current Level 16/later-level policy architecture as an engineering pattern, not as Level 17 behavioral data.

Add selection-property tests across a bounded range of seeds.

Verify all minima/maxima, supply conservation and deterministic repeatability.

---

# Phase 6: Straight `$07` Flipper route

Add a named ordinary/cargo straight `$07` route.

Requirements:

- no below-rim lane changes;
- no below-rim reversals;
- climb directly toward the rim at `2.53125f`;
- preserve current rim clamp/handoff;
- rim pursuit remains normal shared behavior;
- no inherited Level 5/9 Spike-sensitive predicate unless direct existing behavior requires it;
- ordinary Level 17 Flippers and Level 17 Tanker cargo Flippers use the same named route;
- do not alter Level 1 Flipper behavior;
- do not alter Level 8 cargo behavior;
- do not alter Level 16 `$24`.

Add focused route tests for:

- source lane preserved below rim;
- no flip transition;
- no reversal;
- exact translated speed;
- rim arrival;
- cargo equivalence;
- prior movement-program regression.

---

# Phase 7: Tanker behavior and cargo

Level 17 Tankers:

- use `2.53125f` climb speed;
- fire under the existing medium firing mechanism;
- carry exactly two Flippers;
- release both Flippers with existing owner/ordinal identity;
- assign straight `$07` to cargo;
- preserve lane placement and activation semantics;
- participate in reconstruction/completion through the existing generic ledger.

Do not implement Pulsar cargo.

Add tests for:

- exactly one original Tanker under wave policy;
- two owned cargo Flippers;
- no cargo Pulsars;
- correct speed;
- straight-route cargo;
- release/destruction;
- identity conservation;
- reconstruction;
- completion blocking.

---

# Phase 8: Pulsar configuration parameterization

Promote the existing Pulsar actor from isolated-scenario-only use into a configurable production actor without redesigning it.

Add/use configuration fields for Level 17:

- raw flip rate `$28`;
- raw pulse beat `$04`;
- medium firing disabled;
- host climb speed initially existing 1-unit project translation;
- existing 51-update decision threshold;
- existing 18-tick lane pause;
- existing 14-tick rim pause;
- existing 181-update pulse cycle;
- existing 12-degree pulse-flip presentation;
- existing hazard/pulse state;
- existing rim pursuit.

Each retained non-ROM-exact timing must remain labeled as PROJECT TRANSLATION or REFERENCE-DERIVED.

Do not make these global constants silently become Level 17 ROM facts.

Prefer a `PulsarConfiguration` or equivalent bounded configuration seam rather than Level 17 checks scattered through the actor.

Preserve `--pulsar-test`.

---

# Phase 9: Pulsar production admission

Add Pulsars to normal Level 17 admission.

Requirements:

- Pulsars are original wave identities;
- minimum 2 means guaranteed original identities in the Level 17 ledger;
- maximum 5 limits selected/admitted Pulsar originals;
- Pulsars hatch through the normal Egg/original lifecycle;
- Pulsars are not Tanker cargo on Level 17;
- Pulsars consume normal active capacity;
- deterministic selection remains stable;
- medium Pulsars do not fire.

Do not make minimum 2 mean two Pulsars must always be simultaneously active.

Add tests for:

- guaranteed minimum;
- maximum;
- reserve/Egg/active transitions;
- deterministic selection;
- capacity blocking;
- no cargo Pulsars;
- firing disabled.

---

# Phase 10: Ledger extension

Extend the existing generic original-identity ledger minimally.

Requirements:

- add Pulsar original type;
- add active Pulsar state;
- add commit/activation path;
- validate runtime representation;
- include Pulsars in unresolved identity sets;
- include Pulsars in all-rim representation;
- mark destroyed/rim-cleared using shared terminal transitions;
- preserve existing Flipper/Tanker/Spiker/Fuseball behavior;
- do not add a separate Pulsar counter;
- do not create a Level 17-only ledger.

If historically named types such as `Level3OriginalType` remain, make the smallest maintainable extension rather than a broad rename/refactor during this milestone.

Add representation and conservation tests.

---

# Phase 11: Pulsar death/reconstruction

Integrate Pulsars into existing death/re-entry reconstruction.

Requirements:

- reserve/unhatched Pulsar identities remain unresolved;
- Pulsar Eggs reconstruct correctly;
- active Pulsar originals return exactly once;
- no duplicate allocation;
- no lost identities;
- fresh host runtime state after re-entry;
- pause/pulse clock may restart as project translation;
- firing state reflects Level 17 medium disabled;
- rim Pulsars reconstruct through the same safe production principles used for other enemies;
- identity total remains conserved.

Add tests for:

- active Pulsar death;
- reserve Pulsar;
- Pulsar Egg;
- rim Pulsar;
- multiple Pulsars;
- mixed Pulsar/Tanker/cargo wave;
- consecutive deaths;
- exact unresolved identity set.

---

# Phase 12: Completion obligations

Pulsars must block completion while unresolved in any nonterminal relevant state.

Cover:

- reserve;
- Egg;
- ready-to-hatch;
- active below rim;
- transition;
- rim;
- pending reconstruction;
- missing representation.

Completion may proceed only when all original and cargo identities are terminal and no active/pending representation remains.

No Pulsar-specific completion counter.

Add tests that each Pulsar state blocks completion and that fully resolved Level 17 begins exit/terminal behavior.

---

# Phase 13: Pulsar rendering

Reuse the existing Pulsar rendering.

Ensure Level 17 production supports:

- six-segment zigzag glyph;
- pulse-white state;
- lane-boundary flashing/electrification;
- Atari/local projection;
- depth/tube registration;
- rim state;
- player collision/hazard presentation.

Do not tune graphics from screenshots.

Do not redesign the glyph.

Add headless geometry/draw-path tests proving finite rendering under Level 17 presentation.

Manual visual acceptance remains required.

---

# Phase 14: Spiker translation

For Level 17 medium/default:

- use fixed C# speed `3f`;
- preserve existing growth, retreat and terminal conversion;
- preserve retreat multiplier if currently shared;
- ensure enemy shots travel at least as fast and preserve the accepted visible relationship;
- do not add random Level 17 Spiker speed;
- maximum 2, minimum 0.

Do not claim `3f` is a literal translation of raw `$00`.

Label it as PROJECT TRANSLATION.

Add speed and lifecycle tests.

---

# Phase 15: Enemy firing

Implement Level 17 medium/default firing:

- holdoff: 32 ticks;
- capacity: 3 simultaneous enemy-shot slots;
- Flippers eligible;
- Tankers eligible;
- Spikers eligible;
- Pulsars disabled;
- Fuseballs absent;
- existing random/state/depth gates remain;
- existing enemy-shot speed `3f`.

Audit whether current C# uses count versus last-index semantics.

For Level 17, the production capacity must be exactly three slots.

Do not alter accepted prior-level capacities merely to normalize the architecture.

If a shared API expects a count, pass 3.

Add tests for:

- holdoff;
- capacity;
- fourth shot blocked;
- species eligibility;
- Pulsar disabled;
- deterministic firing gates;
- prior-level regression.

---

# Phase 16: Spike configuration

Use raw `$A0` through the established conversion:

`(0xF0 - raw) * Level.Depth / 224`

Expected C# value is approximately `142.857`.

Initialize Spikes using the established production topology policy unless direct Level 17 evidence requires another lane set.

Do not copy Level 1 or Level 16 constants manually.

Add exact raw/translated tests and lane/topology validation.

---

# Phase 17: Progression

Extend natural progression:

Level 16 → Level 17

Requirements:

- Level 16 accepted behavior remains unchanged;
- Level 16 completion enters existing exit/approach transition;
- score/lives carry;
- Super Zapper recharge/reset remains normal;
- Level 17 displayed identity appears;
- canonical Shape 1 geometry selected;
- palette cycle 1 selected;
- Level 17 wave initialized once;
- terminal Level 17 after completion;
- no Level 18.

Add natural progression tests.

---

# Phase 18: Direct startup

Enable:

`--start-level 17`

and the currently supported direct diagnostic equivalent:

`--level 17`

Requirements:

- both use the same `Level17Production` configuration as natural progression;
- HUD displays 17;
- score/lives/startup semantics match existing direct-start rules;
- presentation resolves;
- terminal boundary remains 17;
- unsupported Level 18 remains rejected.

Add parser/startup tests.

---

# Phase 19: Campaign and presentation regression

Preserve all accepted Levels 1–16 behavior.

Run/add focused tests proving:

- Level 1 geometry and palette remain unchanged;
- Level 17 reuses shape without changing Level 1;
- existing per-level calibration rows remain unchanged;
- Level 17 inherited calibration does not overwrite Level 1;
- Levels 5/6 special presentation remains intact;
- Level 16 shared-center Flipper/Blaster behavior remains intact;
- default production presentation remains the accepted Atari path;
- visible recording/playback selection remains unchanged.

Use local:

`TEMP/GameplayRecordings/lvl1-16.json`

for bounded deterministic headless regression if available.

Do not modify or commit the recording.

---

# Phase 20: Automated validation

Run in this order with explicit hang/process bounds:

1. Level 17 shape reuse and palette tests;
2. Level 17 configuration/wave-selection tests;
3. `$07` Flipper/Tanker tests;
4. Pulsar configuration and production admission tests;
5. Pulsar ledger/reconstruction/completion tests;
6. Tanker cargo tests;
7. Spiker/shot/firing tests;
8. progression/direct-start tests;
9. Level 17 presentation draw-path tests;
10. focused Levels 1–16 regression;
11. deterministic `lvl1-16.json` headless playback if available;
12. full automated solution suite;
13. Release build;
14. `git diff --check`.

All long-running tests require explicit VSTest hang timeout or process timeout.

Do not weaken assertions to make the implementation pass.

If prior accepted behavior changes unexpectedly, STOP.

---

# Manual-validation boundary

Do not launch the game.

After automated validation, STOP for Neal.

Provide exact commands for:

## Direct Level 17

`dotnet run --project src\Tempest.Game -- --start-level 17 --lives 12`

## Natural progression

Provide the normal command/path Neal should use to play through Level 16 into Level 17 without requiring the compatibility alias.

## Palette comparison

Provide a Level 1 and Level 17 direct-start pair, optionally with `--background-opacity` for Neal's overlay workflow.

## Pulsar observation

Identify what Neal should evaluate:

- number/presence;
- climb/descend;
- lane changes;
- pulse timing;
- hazard state;
- rim behavior;
- collision;
- firing absent on medium;
- rendering/color.

## Speed/firing

Identify:

- straight `$07`;
- Flipper/Tanker perceived speed;
- cargo release;
- Spiker/projectile relationship;
- three-shot pressure;
- 32-tick cadence.

## Completion

Confirm Pulsars and all cargo/enemies must resolve before exit.

Do not ask Neal to estimate exact percentages or ROM ticks visually.

---

# Documentation boundary

This is not final Level 17 acceptance.

Update durable documentation only when necessary to record:

- implemented candidate;
- evidence classifications;
- manual acceptance still pending.

Do not mark Level 17 accepted.

Do not rewrite historical reports.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-bounded-production-implementation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

These are GitHub continuity records, but do not stage, commit, or push them in this implementation milestone.

They will be included in the future explicitly authorized closeout.

---

# Required implementation report

Include:

1. Starting Git state
2. Files changed
3. Shape-reuse implementation
4. Calibration inheritance
5. Raw palette catalog and host translation
6. Level 17 configuration
7. Wave selector
8. `$07` route implementation
9. Flipper/Tanker translated speed
10. Tanker cargo
11. Pulsar configuration
12. Pulsar production admission
13. Ledger extension
14. Pulsar reconstruction
15. Completion obligations
16. Pulsar rendering
17. Spiker translation
18. Enemy firing
19. Spike translation
20. Natural progression
21. Direct startup
22. Terminal boundary
23. Tests added/changed
24. Focused validation
25. Levels 1–16 regression
26. Recording regression
27. Full-suite result
28. Release-build result
29. `git diff --check`
30. Evidence classifications
31. Deferred hard/easy boundaries
32. Manual-validation matrix
33. Final Git/worktree state
34. Confirmation nothing staged/committed/pushed
35. Confirmation Level 18+ untouched

---

# Stop conditions

STOP instead of expanding scope if:

- canonical shape reuse requires copying geometry;
- palette implementation requires arbitrary role claims beyond the bounded project translation;
- `$07` cannot share a maintainable straight route;
- Pulsar integration requires replacing the actor;
- ledger extension causes prior-level identity regressions;
- production admission cannot conserve exactly 20 identities;
- completion cannot incorporate Pulsars generically;
- firing changes accepted prior levels;
- progression exposes Level 18;
- tests reveal unresolved architecture conflict;
- validation stalls or is security-blocked;
- unrelated worktree state prevents safe isolation.

Preserve the worktree and report exact status.

---

# Completion criteria

Complete only when:

- Level 17 production configuration exists;
- Level 17 reuses canonical Shape 1;
- palette cycle 1 raw data exists;
- Level 1 remains unchanged;
- Level 17 uses straight `$07`;
- Flipper/Tanker speed is `2.53125f`;
- Tanker cargo is exactly two Flippers;
- Pulsar originals are selected 2–5;
- no Level 17 Pulsar cargo;
- medium Pulsars do not fire;
- Pulsars participate in ledger/reconstruction/completion;
- Spiker translation is fixed and bounded;
- firing holdoff is 32;
- shot capacity is 3;
- raw Spike `$A0` is translated correctly;
- natural Level 16→17 works in tests;
- direct Level 17 startup works;
- Level 17 is terminal;
- Level 18 remains unsupported;
- Levels 1–16 regressions pass;
- local campaign recording passes if available;
- full suite passes;
- Release build succeeds with zero errors;
- `git diff --check` passes;
- nothing is staged, committed, or pushed.

Then STOP for Neal's manual acceptance.

TEMPEST-END-LEVEL17-BOUNDED-PRODUCTION-IMPLEMENTATION-COMPLETE