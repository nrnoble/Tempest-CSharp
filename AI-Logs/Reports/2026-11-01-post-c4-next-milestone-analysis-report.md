# Post-C4 next-milestone reassessment

## Checkpoint and disposition

**Checkpoint:** `df0dc19 Complete natural Level 3 progression`.
`git status --short` contains only the existing untracked `TEMP/` directory;
there is no tracked diff. This is a read-only reassessment; no source, tests,
tracked documentation, reference material, or Git history was changed.

**Recommendation:** Make the interactive Atari between-level approach the
next feature area, but begin with a bounded behavior/architecture analysis
and acceptance plan, not implementation. It is the strongest remaining
directly observed, gameplay-relevant gap that applies to both accepted
production transitions. Preserve the qualitative observations; do not
invent timing, zoom, projection, or engagement rules.

## Current accepted state

- **Level 1:** Normal no-flag startup; validated Flipper baseline, Egg wave,
  scoring/lives, and broad natural exit to Level 2. Ordinary enemy composition
  and project Egg pacing remain distinct from claims about unknown arcade
  internals.
- **Level 2:** Natural square/canflip configuration and direct development
  access share the accepted geometry. Below-rim and rim behavior have manual
  acceptance; direct `--level2-test` remains isolated and terminal.
- **Level 3:** Natural campaign enters the accepted plus-board mixed
  `Level3Mixed` configuration. C3b direct graphical access remains
  DEVELOPMENT ONLY. Neal rated the accepted visible C3 behavior 5/5,
  “Impressive.” C4 manually accepted natural entry, HUD/board, fresh mixed
  wave, two usable Zapper charges, gameplay spot-check, terminal/no-Level-4,
  and restart.
- **Natural progression:** Exactly Level 1 -> Level 2 -> Level 3 -> terminal.
  Score/lives carry on both edges; terminal completion/restart returns to
  fresh Level 1. No Level 4 or cycle exists.
- **Flippers:** C1/C1.5 directed full-sweep and inward geometry, Level 1/2/
  plus-board observations, and completed-pivot player collision have manual
  acceptance. Direct climbing rim-arrival lethality is implemented from
  reference evidence but has not been separately established against Atari.
  The suspected converging-rim-Flipper case remains unreproduced.
- **Tankers/cargo:** C1/C1.5 validated the visible cargo route/pivots and
  survival outcomes; C2 accepted randomized Tanker firing behavior and the
  reference-tier 1.75 climb correction; C3/C3b/C4 consume the accepted mixed
  wave. Exact Atari firing distributions and the full intermediate cargo
  route/timing remain unknown. No currently identified implementation defect
  warrants retuning.
- **Super Zapper:** Accepted visible use across ordinary and development
  scenarios and fresh-level charges at natural transitions. Exact selection,
  timers, accounting, and internal ordering remain reference/project details.
- **Fuseball, Pulsar, Spiker/Spike, Hurry-Up and all-rim:** Substantial
  development-only implementations and manual observations exist; broad
  all-rim exit was manually accepted in normal Level 1 and the four enemy
  scenarios. Exact timers/thresholds remain reference/project details.
  Hurry-Up is not enabled in the current normal production wave policy.
- **Input/controller/spinner:** Physical keyboard/controller aliases and
  diagnostics were accepted; spinner movement works and is playable. A small
  perceived C# response delay relative to Incitti is a qualitative comparison,
  not a measured Atari discrepancy. Two volcano buttons still expose no
  observable Raylib event.
- **Rendering/presentation:** C1/C1.5 geometry and C3b visible mixed-wave
  behavior are accepted, but Neal rated the broad C# Blaster/enemy rendering
  around 3.5/5 versus Incitti around 5/5. The Atari between-level approach is
  directly observed but unimplemented. Audio, bonus/entry presentation and
  open-tube rendering are not current implemented features.
- **Validation:** C4 finished with 494/494 focused and 527/527 full tests,
  zero build warnings/errors, and clean diff-check. Automated tests verify C#
  behavior, not arcade fidelity. C1/C1.5, C2, C3b and C4 have documented
  manual acceptance; C3a's production-shaped model was automated before C3b
  graphical acceptance.

The review read the current README, development notes and behavior
specification; C1/C1.5, C2, C3a, C3b, C4 and progression implementation
reports; and relevant current progression, state, player, projection,
renderer, simulation, and test structure. Older reports remain historical
evidence and were not rewritten.

## Deferred and pending audit

| Item | Classification | Current assessment |
|---|---|---|
| Interactive between-level approach | **A — STILL OPEN** | Direct Atari observation: distant next tube grows during approach; small Blaster can move around its rim; enemy engagement begins after arrival. C4 intentionally accepted immediate transition instead. Timing, zoom, projection and exact engagement are unmeasured, so analyze before implementing. |
| Spinner response | **D/E — VALID, LOW-PRIORITY FIDELITY / MORE EVIDENCE** | Current movement is playable. The small perceived delay versus Incitti needs controlled measurement and comparison before any tuning; do not alter Windows mouse settings on assumption. |
| Blaster/enemy rendering fidelity | **D — VALID FIDELITY WORK** | A real qualitative quality gap was reported, but it is broad and lacks a specific measured failure. A later visual comparison should identify concrete glyph/scale/animation differences before code changes. |
| Rim Flipper convergence | **A/E — UNREPRODUCED; MORE EVIDENCE REQUIRED** | Still suspected and not reproduced. Log/reproduce in normal play or compare with Atari first; do not alter movement, lane selection, or collision to preemptively fix it. |
| Direct climbing Flipper rim arrival | **E — MORE ARCADE EVIDENCE** | Current C# behavior follows Incitti and has tests, but direct-arrival lethality is not separately Atari-verified. Validate the observable case before changing established code. |
| Tanker/cargo follow-ups | **B/E — LATER WORK RESOLVED; DETAILS UNKNOWN** | C1/C1.5, C2, C3/C3b and C4 supersede old pending implementation/manual statements. Only exact firing rates/distribution, unlock/composition rules and full Atari cargo route/timing remain evidence gaps; no present defect is established. |
| Fuseball/Pulsar/Spiker and Hurry-Up production use | **D/E — VALID CONTENT GAP; EVIDENCE/DEPENDENCY LIMITED** | Current isolated development scenarios are not authentic level composition. Adding them to normal play depends on later level definitions and progression. Do not promote artificial scenarios directly into production. |
| Level 4 and later | **A — OPEN, ANALYSIS ONLY** | C4 intentionally ends at terminal Level 3. Existing configuration/resolver/fresh-level boundaries are useful groundwork, but they do not supply Level 4's board, composition, pacing, or evidence. Analyze content/progression policy before implementation; do not design or add Level 4 in this milestone. |
| Controller volcano buttons | **D — LOW-PRIORITY INVESTIGATION** | No Raylib event was observed in the diagnostic. No production behavior should be added without an observable input path and a use case. |
| Audio, bonus rules, open tube, death presentation | **D/E — DEFERRED OR UNSPECIFIED** | These are not blockers to accepted C4. Any future work needs a defined requirement and, where behavior is claimed, appropriate arcade/reference evidence. |

Resolved or superseded: Level 1 -> Level 2 progression; C1/C1.5 geometry
and inward-pivot work; Tanker cargo correction; C2 firing-policy correction
and manual validation; C3a mixed-wave lifecycle; C3b direct graphical
acceptance; and C4 natural progression/manual acceptance. The old claim that
natural Level 2 is terminal is explicitly historical in the behavior spec.
Do not reopen these based only on older “pending” reports.

## Candidate ranking

1. **Between-level approach behavior/architecture analysis** — Best next
   milestone. The observation is direct, relevant to both production
   transitions, and changes interactive gameplay as well as presentation.
   The transition is a reusable foundation for future content and offers a
   clean checkpoint without inventing a later level's composition.
2. **Later-level progression/content architecture analysis** — Necessary
   before Level 4 or production use of additional enemy types, but no current
   Level 4 composition or authentic schedule is established. It is the next
   campaign-expansion analysis, not the next implementation.
3. **Targeted visual-fidelity analysis** — Neal has identified a quality gap,
   but “3.5/5 vs 5/5” is not an actionable rendering requirement. Compare
   specific objects, transforms, scale and animation before selecting a
   narrow change.
4. **Rim-Flipper collision/convergence validation** — Potential gameplay
   value, but no reproducible convergence defect exists. Prefer a focused
   observation/reproduction pass; change code only after a concrete mismatch.
5. **Spinner-response characterization** — Worth revisiting if the small
   perceived delay affects play, but lower priority while controls remain
   playable and the source comparison is unmeasured.
6. **Later enemy integration, audio, open-tube rendering, and other broad
   fidelity work** — Valuable only after specific behavior/content is
   established; currently too broad or dependent on an unmade progression
   decision.

## Primary recommendation and next-step boundary

**PRIMARY NEXT MILESTONE: Analyze the Atari between-level approach and define
a bounded C# transition milestone.**

First establish observable requirements and current integration points:

- Confirm the exact visible start/end and player-control observations using
  Neal's Atari knowledge or footage where available; record unknowns rather
  than guessing.
- Inspect the current exit/fresh-level lifecycle, projection/render path,
  player rim movement and unused transition-state vocabulary. Determine how
  to defer enemy engagement until arrival without corrupting score/lives,
  level identity, fresh wave setup, Zapper state or restart behavior.
- Compare Incitti read-only as an engineering reference only. Do not elevate
  its behavior to Atari truth.
- Define a focused scope and measurable manual acceptance before
  implementation. Keep the two existing natural progression edges and direct
  development modes isolated.

This analysis is the next **single bounded checkpoint**. It should decide
whether implementation can remain one cohesive transition milestone or needs
separate rendering and simulation boundaries. Do not refactor unrelated game
systems preemptively.

**SECONDARY FOLLOW-UP:** Analyze Level 4+ progression and production enemy
composition after the transition direction is settled. This must precede any
Level 4 or natural Fuseball/Pulsar/Spiker promotion.

### NOT YET

- Do not add Level 4, a campaign cycle, or later enemy types to natural play.
- Do not tune spinner sensitivity, movement rates, or Windows mouse settings
  based only on an Incitti comparison.
- Do not retune Flipper convergence, arrival collision, Tanker firing/cargo,
  Super Zapper, Egg scheduling, Hurry-Up, or all-rim logic without reproduced
  evidence and explicit scope.
- Do not begin a broad renderer rewrite from a subjective score alone.
- Do not add audio, bonus rules, or open-tube gameplay as incidental scope.

## Current fidelity gaps by category

- **Gameplay/mechanics:** Main accepted production loop is complete through
  Level 3. Remaining evidence questions include direct climbing-Flipper
  arrival, suspected rim convergence, exact Tanker/cargo arcade details, and
  unmeasured source-derived timings. No confirmed C4/C3 defect is open.
- **Presentation/rendering:** Between-level approach is the clearest
  gameplay-relevant missing behavior. Broader entity rendering quality is
  lower-rated but needs specific comparison. Audio and open-tube presentation
  remain unspecified/deferred.
- **Control feel:** Spinner is usable; response comparison is qualitative.
  The controller diagnostic's unobserved volcano buttons remain low-priority.
- **Progression/content:** C4 is complete; Level 4+ content and production
  enemy composition require a separate evidence/architecture decision.
- **Audio:** Not implemented; no accepted audio requirement was identified.
- **Architecture/maintainability:** `TempestGame` is a substantial
  orchestration class (~1,467 lines); `VectorRenderer` and `Level` also carry
  significant responsibilities. However, progression, configuration,
  projection, enemy types and the Level 3 ledger already have distinct
  components, and current tests pass. No isolated failure or demonstrated
  correctness risk justifies a broad refactor before the next feature.
  Transition analysis should identify a seam if its state machine warrants
  one. The concrete maintenance concern is conflicting historical status
  text in current docs; fix it narrowly during a later documentation pass.
- **Unreproduced defects:** Suspected rim Flipper convergence only; retain as
  observation/reproduction work, not a code-change task.

## Maintenance and stale documentation

No separate refactor milestone is recommended. Current code concentration
deserves restraint and deliberate boundaries, not extraction for aesthetics.
The existing C3 lifecycle diagrams remain useful for identity/accounting; no
new diagram is needed.

Material stale statements to correct in a later narrow documentation pass:

- `TEMPEST-DEVELOPMENT-NOTES.md` still says natural progression is Level 1 ->
  Level 2 and terminal after Level 2, and says Level 3/C2/C3/C4 work remains
  deferred (the old C1.5 geometry section).
- Its older Super Zapper and Hurry-Up paragraphs still describe manual
  comparisons/spec review as pending, and say Level 3/broader campaign work is
  out of scope. Later reports and the behavior spec supersede those statuses.
- Its Tanker-cargo section still says final geometry/manual validation is
  pending, despite later C1.5 acceptance.
- `README.md`'s older Super Zapper summary still says Level 3 and broader
  campaign progression are out of scope.
- Historical AI reports legitimately preserve their checkpoint-time status;
  update current tracked summaries, not old reports.

## Test and manual strategy

**Automated surface (after analysis authorizes implementation):**

- State/transition tests for entry into approach, approach progression,
  input-enabled Blaster movement/wrap, enemy and firing inactivity before
  arrival, and ordinary Level activation after arrival.
- Tests for score/lives and current production identity continuity, fresh
  wave/ledger/Zapper invariants, both Level 1 -> 2 and Level 2 -> 3 edges,
  Game Over/restart, and direct `--level2-test`, `--level3-test`, and geometry
  mode isolation.
- Projection/render calculation tests using the headless seams where
  available. Do not add Raylib-window tests unless an explicit integration
  harness is justified.

**Manual surface:** In normal campaign play, inspect both transitions:
next tube starts distant and grows; spinner can move the Blaster around its
rim; enemy engagement begins only after arrival; identities, HUD, score/lives,
and gameplay remain correct. Record crashes/regressions. Do not assert exact
timing or projection values until directly measured.

**Development access:** Existing direct Level 2/Level 3 modes cover steady
states, not the natural transition. Normal campaign play is the preferred
acceptance route. A new development-only mode is not necessary unless the
analysis finds a concrete testability problem.

## Evidence discipline and GO/NO-GO

Retain **ARCADE VERIFIED** for directly observed behavior only;
**REFERENCE VERIFIED** for Incitti source facts; **PROJECT DECISION** for
recreation policy; and **DEVELOPMENT ONLY** for artificial scenarios and
diagnostics. Neal's qualitative preference for Incitti timing/rendering does
not prove Incitti equals Atari.

**GO:** perform the bounded between-level approach analysis and define
acceptance criteria.
**NO-GO:** feature implementation before the behavior/state boundary is
defined; Level 4/cycle/enemy promotion; ungrounded control/render/gameplay
retuning; broad architecture refactoring.
