# Tempest Level 17 Production-Presentation and Second-Cycle Palette Investigation

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

Perform a **read-only Level 17 production-presentation and second-cycle palette investigation**.

Level 17 gameplay is now functioning correctly and must remain frozen.

This task has two concerns only:

1. determine why Level 17's reused Shape 1 currently appears to use an old/legacy-looking tube presentation instead of the accepted production presentation already established for Shape 1 / Level 1;
2. determine the smallest evidence-backed way to apply the verified Levels 17–32 ROM color scheme visibly in the current C# production renderer.

This is an investigation milestone.

**Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not change gameplay.
Do not change enemy behavior.
Do not change Flipper behavior.
Do not change Pulsar behavior.
Do not retune speed/timing.
Do not implement Level 18+.
Do not stage, commit, or push.
Do not launch the interactive game.**

Stop after producing a detailed report and bounded implementation recommendation.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-level17-rom-map-final-evidence-report.md`

`AI-Logs/Reports/2026-11-25-level17-rom-to-csharp-integration-readiness-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-bounded-production-implementation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-frozen-rim-flipper-correction-report.md`

Also read the production-presentation promotion closeout report:

`AI-Logs/Reports/2026-11-25-production-presentation-default-promotion-closeout-report.md`

Use the latest corrective reports as authority where historical reports conflict.

Do not reopen completed gameplay investigations.

---

# GAMEPLAY FREEZE

Level 17 gameplay is now frozen for this investigation.

Neal manually reports:

- Level 17 Flippers now move correctly around the rim.
- Pulsars behave correctly.
- Tankers behave correctly.
- Level 17 overall gameplay plays fine.
- Levels 1–16 campaign playback is restored and correct.

The latest automated gate reports:

- Level 17 recording PASS;
- Levels 1–16 accepted campaign boundary PASS;
- Level 15 seed-1982 PASS;
- 68 focused tests PASS;
- full suite 2,060 passed / 0 failed;
- Release build 0 warnings / 0 errors;
- `git diff --check` PASS.

Do not change:

- Level 17 wave selection;
- `$07` Flipper movement;
- Flipper rim handoff;
- Tanker cargo;
- Pulsars;
- ledger/reconstruction/completion;
- enemy firing;
- Spike behavior;
- speeds;
- random behavior;
- progression gameplay state.

If presentation investigation suggests a gameplay change is necessary, STOP and report the conflict instead of making that change.

---

# User presentation requirement

Neal wants new development and testing to use the accepted production presentation, not legacy tube presentation.

The project has already promoted accepted Atari presentation to the normal/default interactive presentation.

Therefore:

> Level 17 must use the same accepted production presentation architecture as existing production levels.

The old/legacy tube presentation must not become the development baseline for Level 17 or later levels merely because it is easier to render.

The purpose is to find and correct the Level 17 production-presentation integration, not to tune the legacy path.

Do not recommend performing Level 17 visual acceptance against a presentation path that is not intended to remain production behavior.

---

# Established Level 17 shape evidence

Treat as established:

- Level 17 begins the second 16-level tube cycle.
- Level 17 resolves to ROM tube/shape row 0.
- Shape row 0 is the same canonical geometry used by displayed Level 1.
- Level 17 must reuse canonical Shape 1 geometry.
- Level 17 should not have a copied point array.
- Shape identity and displayed-level identity are separate concepts.
- Gameplay configuration is Level 17-specific even though geometry is reused.

Do not reinvestigate ROM tube geometry.

Do not change it.

---

# Established Level 17 palette evidence

Treat as verified ROM evidence:

## Levels 1–16 raw palette block

`00 04 08 0C C3 07 0B 0B`

## Levels 17–32 raw palette block

`00 07 0B 08 44 03 0C 0C`

The second block is verified from CPU range:

`$C205-$C20C`

Level 17 uses the second-cycle palette identity.

The existing Level 17 implementation catalogs the raw block but intentionally retained existing host-role colors because complete ROM-role-to-C#-RGB translation was not established.

That is why storing palette index 1 does not by itself guarantee a visible color change.

Do not repeat ROM-byte recovery.

---

# Manual Level 17 presentation evidence

Record this as **MANUAL PRESENTATION DEFECT EVIDENCE**:

Neal reports:

- Level 17 is playable.
- Level 17 appears visually like the old/legacy tube presentation.
- The expected Levels 17–32 color-scheme change is not visible.
- Spikes and Pulsars are visibly present.
- Gameplay itself is currently acceptable after the recent Flipper fixes.

This means Level 17 does **not** yet have presentation acceptance.

Do not conflate the presentation/palette defects with gameplay.

---

# Investigation 1: Trace the effective presentation-selection path for Level 17

Start from an ordinary interactive/direct Level 17 launch with no compatibility flags.

Trace:

- CLI/default presentation selection;
- effective presentation enum/state;
- `Program` renderer construction;
- `VectorRenderer`;
- Atari presentation enablement;
- model-frame resolver;
- level/shape mapping;
- calibration resolver;
- projection routing;
- special fallback routing;
- renderer draw path.

Determine exactly which presentation implementation Level 17 actually uses.

Answer:

1. Is accepted production/Atari presentation selected?
2. Does Level 17 fall back internally to an old/legacy projector despite the global default?
3. Does the resolver fail to recognize displayed Level 17 and therefore use a fallback?
4. Is Shape 1 recognized for geometry but not for presentation mapping?
5. Does Level 17 use a new metadata row with neutral/default values instead of Shape 1's accepted mapping?
6. Does a displayed-level range check still stop at 16?
7. Is any diagnostic/legacy presentation accidentally selected?

Do not infer from appearance alone.

Trace the actual code.

---

# Investigation 2: Compare Level 1 versus Level 17 production presentation routing

Create a read-only path comparison.

For the same Shape 1 geometry, compare:

## Level 1

- displayed-level identity;
- shape identity;
- model-frame mapping;
- calibration row;
- projection function;
- tube coordinates submitted to renderer;
- entity projection;
- Blaster projection;
- approach/ExitZoom routing.

## Level 17

Capture the same fields.

Identify the **first presentation-routing difference** that is not explained by intentionally different displayed-level palette/gameplay identity.

The desired structural relationship is:

```
Level 1 ─┐
         ├─ Shape 1 geometry
         ├─ Shape 1 model frame
         ├─ Shape 1 accepted tube calibration
         └─ shared production projection
Level 17 ┘

Level 1  -> cycle palette 0
Level 17 -> cycle palette 1
```

Do not assume current code already achieves this.

---

# Investigation 3: Inspect presentation tables/catalogs for displayed-level keying

Inspect:

- ROM presentation metadata;
- Atari presentation calibration tables;
- adapter/model-frame catalogs;
- display-level guards;
- shape-remap helpers;
- any arrays sized specifically for 16 displayed levels;
- level-specific fallback code.

Classify each presentation datum as logically:

## Shape-specific

Should be shared by Levels 1/17/33/etc. using Shape 1.

Potential examples:

- geometry;
- topology;
- model-frame mapping;
- tube calibration;
- shape-local entity scaling.

## Displayed-level or cycle-specific

Should differ despite shared geometry.

Potential examples:

- color palette;
- HUD displayed-level number;
- gameplay configuration;
- possibly transition/cycle effects.

## Truly level-specific

Only where existing evidence justifies it.

Identify any current design where a shape-specific presentation value is unnecessarily duplicated per displayed level.

Do not refactor.

---

# Investigation 4: Determine why Level 17 looks legacy

Give a precise causal explanation.

Possible categories include:

- actual legacy projection path;
- correct Atari path with missing Shape 1 calibration reuse;
- correct projection with neutral model-frame data;
- wrong viewport/model-frame;
- wrong internal fallback;
- color difference creating a misleading visual impression;
- another source-backed cause.

Do not use the term "legacy-looking" as the diagnosis.

State exactly what code path/value differs from accepted Level 1 production presentation.

---

# Investigation 5: Verify inherited Shape 1 calibration

Identify the manually accepted current Shape 1/Level 1 production calibration.

Determine whether Level 17 should initially inherit exactly those values because geometry is identical.

Trace current Level 17 effective values for:

- `tube-y-gain`;
- `tube-scale`;
- `tube-vertical-offset`;
- `tube-depth-y-gain`;
- model-frame bounds/center;
- any shape-local transform.

Do not tune new values.

If Level 17 is not receiving Shape 1's accepted values, explain why.

If it is already receiving them, prove that and move to the next presentation difference.

---

# Investigation 6: Verify entity registration under Shape 1 reuse

Presentation correction must keep:

- Blaster;
- Flippers;
- Tankers;
- Spikers;
- Pulsars;
- shots;
- Spikes;
- Eggs

registered to the same projected tube.

Compare representative Level 1 and Level 17 projection architecture.

Do not compare gameplay state.

Determine whether the Level 17 presentation defect affects:

- tube only;
- entities only;
- both;
- neither, with appearance caused elsewhere.

This is read-only geometry/registration analysis.

---

# Investigation 7: Trace the ROM color variables from `setcolours`

Use the already verified second-cycle raw palette.

Trace the ROM's palette assignment far enough to determine the semantic runtime role of each block entry.

For:

`00 07 0B 08 44 03 0C 0C`

identify, where authoritative source permits:

- array/index destination;
- low/high nibble interpretation;
- vector color/intensity role;
- tube/web role;
- player role;
- enemy role;
- shot role;
- Spike role;
- Pulsar-related role;
- HUD/text role;
- effect/super-zapper role.

Do not force an entity label onto entries whose exact role remains indirect.

If the ROM writes entries into intermediate arrays later indexed by object state, document that structure.

---

# Investigation 8: Decode Atari vector color representation only as far as required

Determine what the raw palette values represent.

Investigate:

- hue bits;
- intensity bits;
- packed-nibble behavior;
- blink/intensity toggles;
- vector-generator color command format where already documented locally;
- palette/index transforms between CPU and vector-generator code.

The objective is **not** to recreate monitor phosphor physics.

The objective is to translate verified ROM color roles into stable C# vector colors without arbitrary guessing.

Do not search the Internet for replacement ROM/color data.

Project-local Atari documentation is preferred.

---

# Investigation 9: Reconcile first-cycle ROM palette with accepted C# colors

This is an important translation anchor.

Use:

First-cycle ROM block:

`00 04 08 0C C3 07 0B 0B`

and the currently manually accepted Levels 1–16 C# vector colors.

Determine whether the accepted C# renderer establishes a consistent mapping such as:

```
ROM color role/code -> accepted C# host color
```

Do not assume raw nibble equals an RGB index.

Trace current production color constants and uses.

For each confidently matched role, record:

- first-cycle raw ROM value;
- established role;
- accepted current C# color;
- evidence supporting the correspondence.

This mapping may provide the safest translation basis for cycle 2.

---

# Investigation 10: Derive a non-arbitrary second-cycle host palette candidate

Using:

- verified second-cycle raw block;
- established color-role trace;
- accepted first-cycle C# translations;

derive the smallest initial second-cycle C# host-palette translation.

Every proposed visible color must be classified:

- direct reuse of an accepted first-cycle translation for same ROM code/role;
- transformation from known Atari color code semantics;
- PROJECT TRANSLATION requiring Neal's visual acceptance;
- unresolved.

Avoid choosing "looks reasonable" colors without source relationship.

The end result should be an implementation candidate suitable for manual comparison, not a claim of pixel-perfect Atari output.

---

# Investigation 11: Keep palette independent from tube shape

Assess the appropriate architecture.

The desired relationship is:

```
Shape 1
  geometry
  projection
  calibration

Cycle palette 0
  Levels 1–16

Cycle palette 1
  Levels 17–32
```

Changing cycle palette must not require creating another Shape 1 geometry or calibration object.

Changing Shape 1 geometry/calibration in the future should apply wherever Shape 1 recurs.

Determine whether the existing architecture already supports this separation or needs a bounded new palette-selection seam.

Do not implement it.

---

# Investigation 12: Super Zapper and transient colors

Trace how transient effects interact with base cycle colors.

At minimum inspect:

- Super Zapper;
- pulse-white Pulsars;
- enemy destruction;
- player shots;
- enemy shots;
- Spike colors;
- HUD/vector text if palette-dependent.

Determine which effects:

- override base palette temporarily;
- remain fixed regardless of cycle;
- return to the selected cycle palette afterward.

Do not alter effect behavior.

Report what the future implementation must preserve.

---

# Investigation 13: Approach and Level 16→17 transition colors

Determine what palette is active during natural Level 16→17 transition.

Trace:

- outgoing Level 16;
- ExitZoom;
- starfield;
- incoming Level 17 approach;
- tube appearance during approach;
- palette activation point;
- HUD level identity.

Determine the ROM/source-backed state boundary at which cycle-2 colors become active, where possible.

Do not implement or tune.

---

# Investigation 14: Testing implications

Recommend focused automated protection for the future implementation.

At minimum:

## Presentation routing

- no-option/default Level 17 selects production Atari path;
- Level 17 Shape 1 projection matches Level 1 Shape 1 projection under identical shape/camera inputs;
- no legacy fallback;
- model frame shared correctly.

## Calibration

- Level 17 inherits Shape 1 accepted defaults;
- Level 1 unchanged;
- runtime `--tube-*` override remains functional;
- shape calibration remains single-source.

## Palette

- first-cycle raw block unchanged;
- second-cycle raw block exact;
- cycle selection at Level 17;
- Level 16 retains cycle 0;
- Level 17 selects cycle 1;
- geometry unaffected by palette;
- transient colors restore correct base palette.

## Registration

- tube/entities remain co-registered;
- Pulsar/Flipper/Blaster/shot projection finite.

## Gameplay freeze

- Level 17 gameplay regression tests remain unchanged/passing;
- strict Levels 1–16 recording remains passing;
- Level 17 rim-Flipper recording remains passing;
- no new random/gameplay state changes.

---

# Investigation 15: Determine whether legacy presentation can be avoided entirely for new-level development

Neal's development policy is:

**New Level 17+ visual development should use production presentation, not legacy tube presentation.**

Audit current workflows/tests for Level 17 that still:

- instantiate legacy projection by default;
- use baseline portrait when unnecessary;
- bypass accepted production adapter;
- validate geometry only through obsolete presentation paths.

Classify:

- tests that should remain low-level/legacy because they test a primitive intentionally;
- tests that should exercise production presentation because they validate gameplay-visible output;
- diagnostic A/B paths that remain useful but must be opt-in.

Do not remove legacy/debug code.

The purpose is to prevent future development accidentally being validated against the wrong presentation.

---

# FINAL READINESS RESULT

End with exactly one:

## READY FOR BOUNDED LEVEL 17 PRESENTATION/PALETTE IMPLEMENTATION

Use only if:

- exact reason for legacy-looking Level 17 is established;
- Shape 1 production presentation reuse path is known;
- required calibration inheritance is established;
- second-cycle palette roles are understood sufficiently;
- a non-arbitrary initial host-palette translation exists;
- gameplay can remain completely untouched.

or:

## NOT READY

List only the exact presentation/palette blockers.

Do not block implementation merely because final visible RGB acceptance requires Neal's comparison.

Manual visual acceptance is expected.

---

# Execution limits

Use read-only:

- source inspection;
- tests inspection;
- local Atari disassembly/reference;
- small calculations;
- static/path tracing.

Do not:

- modify production/tests/docs;
- launch the game;
- perform new gameplay investigation;
- inspect unrelated screenshots/videos/images;
- search screenshot directories;
- run broad tests for ceremony;
- stage/commit/push.

If the historical two supplied screenshots are not available in this task session, do not search for them.

The future implementation can be manually compared by Neal.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-level17-production-presentation-second-cycle-palette-investigation-report.md`

These are GitHub continuity records, but this investigation does not authorize commit or push.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Gameplay-freeze confirmation
4. Manual presentation defect evidence
5. Level 17 effective presentation-selection trace
6. Level 1 effective presentation trace
7. First Level 1/17 routing difference
8. Presentation table/catalog audit
9. Shape-specific data classification
10. Level/cycle-specific data classification
11. Exact cause of legacy-looking Level 17
12. Shape 1 accepted calibration values
13. Level 17 effective calibration values
14. Calibration inheritance assessment
15. Model-frame reuse assessment
16. Tube/entity registration assessment
17. Verified second-cycle raw palette
18. ROM palette runtime-array trace
19. Color-code/intensity semantics
20. First-cycle ROM-to-C# reconciliation
21. Second-cycle host-palette proposal
22. Evidence classification for every proposed host color
23. Shape-versus-palette architecture assessment
24. Super Zapper/transient color interactions
25. Pulsar pulse-white interaction
26. Level 16→17 transition palette timing
27. Production-presentation test recommendations
28. Palette test recommendations
29. Gameplay-freeze regression recommendations
30. Legacy-development-path audit
31. Smallest bounded implementation scope
32. Manual visual-acceptance questions
33. Remaining uncertainties
34. READY/NOT READY result
35. Exact blockers, if any
36. Files/resources inspected
37. Commands/calculations run
38. Production code changed: no
39. Tests changed: no
40. Durable documentation changed: no
41. Stage/commit/push: none
42. Gameplay changed: no
43. Level 18+ work: none
44. Recommended next step

Do not produce a short summary if detailed evidence was gathered.

---

# Stop conditions

STOP rather than guessing if:

- Level 17 presentation route cannot be determined;
- Level 1 accepted presentation cannot be traced;
- Shape 1 model-frame/calibration reuse remains ambiguous;
- palette roles cannot be mapped sufficiently for a non-arbitrary candidate;
- proposed presentation correction would require gameplay changes;
- the only solution is to tune the legacy path;
- exact host colors would be wholly arbitrary with no ROM/accepted-C# anchor.

Report NOT READY if needed.

---

# Completion criteria

This investigation is complete when it establishes or narrowly blocks:

- why Level 17 looks legacy;
- how Level 17 must reuse Shape 1 production projection;
- how accepted Shape 1 calibration is inherited;
- how cycle-2 palette differs independently from Shape 1;
- how verified ROM palette roles map to an initial C# host palette;
- what transient colors must remain independent;
- how Level 16→17 palette transition should work;
- what production-presentation tests need to replace legacy-path validation;
- the smallest implementation scope;
- READY or NOT READY.

Then STOP.

Do not implement the presentation correction.

TEMPEST-END-LEVEL17-PRODUCTION-PRESENTATION-SECOND-CYCLE-PALETTE-INVESTIGATION-COMPLETE