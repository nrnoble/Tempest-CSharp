C2 — Isolated Tanker firing validation/policy analysis

Analysis only. Do not implement gameplay changes yet.

Current clean checkpoint:
27c43b7 Add validated controller restart and quit aliases

Major preceding checkpoints:
a922172 Ignore local video files
11773c0 Implement Level 3 geometry and directed rotation

C1/C1.5 geometry/directed rotation/inward-pivot work is manually accepted.
The controller-input checkpoint is also manually accepted.

Purpose

Resolve C2 from the previously approved Level 3 roadmap:

Determine whether the current source-derived Tanker firing policy is an appropriate basis for the recreation before Tanker firing is promoted into production Level 3.

Do not begin C3.
Do not implement production Level 3 mixed composition.
Do not add natural Level 2 -> Level 3 progression.
Do not change Tanker firing yet unless specifically requested in a later implementation prompt.

Evidence hierarchy

Preserve the established project evidence hierarchy:

1. Directly observed Atari Tempest behavior / Neal's arcade observations.
2. Mark Incitti's implementation as an engineering/reference source.
3. Current tested C# behavior.
4. New assumptions only when necessary.

Do not label Incitti-only behavior ARCADE VERIFIED.

However, record this new qualitative development observation:

Neal recently replayed Incitti's recreation after not playing it for several years. Based on Neal's extensive experience with the original Atari Tempest arcade game, Neal considers Incitti's overall gameplay timing extremely close to Atari Tempest. Neal reports that he would expect to notice small timing deviations and did not notice meaningful timing differences during this recent play.

This does NOT promote Incitti's source constants to ARCADE VERIFIED.
It does make literal Incitti timing particularly useful as an engineering comparison when evaluating unexplained timing differences.

Existing Tanker arcade observation

Preserve the currently documented observation:

- Atari Tankers fire individual shots at intervals.
- At least two Tanker shots can occur before the Tanker reaches its release/rim area.

Do not infer:
- exact ammunition count,
- exact initial delay,
- exact shot interval,
- exact random probability,
- exact firing-height cutoff,
- or exact number of shots per Tanker

unless stronger evidence actually establishes those facts.

Current development behavior already documented

The existing analysis says:

- --tanker-test alone leaves the Tanker silent.
- --tanker-test --enemy-bullets-test enables the existing Tanker firing path.
- In an earlier manual run, one C# Tanker fired one shot before release, but Neal could not determine whether there was enough time for a second shot.
- The exact current ammunition/timer/chance behavior is Incitti-derived rather than arcade validated.
- Flipper bullets remain a separate question and must not be promoted merely because Tanker firing is being investigated.

Tasks

1. Verify the baseline before analysis:

git status --short
git log -3 --oneline
git diff --check
git status --short -- Reference/incitti

The worktree should be clean at 27c43b7.

2. Inspect the current C# Tanker firing path in detail.

Trace only what is necessary to establish:

- Tanker initialization and firing-related fields;
- ammunition initialization;
- firing timer/delay initialization;
- which Tanker states/updates attempt firing;
- timer decrement semantics;
- random firing gate/chance;
- ammunition depletion;
- bullet creation and ownership;
- bullet lane/height/direction;
- whether Tanker motion continues while firing;
- release-height interaction;
- whether a firing attempt can occur on the release update;
- whether bullets survive Tanker destruction or natural release;
- effects of player death/reconstruction;
- Super Zapper interaction;
- all-rim behavior where relevant;
- --tanker-test versus --enemy-bullets-test gating.

Distinguish source behavior from C# project behavior.

3. Inspect Mark Incitti's literal Tanker firing implementation.

Use Reference/incitti directly.

Identify the exact source locations and executable behavior for:

- Tanker ammunition creation;
- initial timer/delay;
- timer decrement;
- random gate;
- ammo decrement;
- Tanker Shoot() invocation;
- projectile creation;
- projectile movement/lifecycle if required to understand firing;
- Tanker climb/release ordering;
- whether firing remains possible immediately before release;
- whether existing bullets survive carrier removal;
- any level-dependent formulas affecting Tanker speed or firing at displayed Level 3.

Do not rely only on historical AI summaries. Compare literal source with current C#.

4. Compare Incitti and current C# side by side.

Produce a precise table containing, where established:

- ammunition;
- initial delay/timer;
- eligible firing updates;
- firing probability/gate;
- timer reset behavior;
- ammo consumption;
- climb rate;
- release threshold;
- shot creation;
- shot lifetime independence;
- ordering of firing relative to movement/release;
- development flag differences.

For each row give evidence classification:
- ARCADE VERIFIED
- REFERENCE VERIFIED
- PROJECT DECISION
- DEVELOPMENT ONLY
- unresolved

5. Determine what the existing policy predicts.

Using the actual code/constants, analytically determine whether the current C#/Incitti-style policy can reasonably produce:

- zero shots before release;
- one shot before release;
- two or more shots before release.

Do not use invented data.

If randomness prevents a deterministic count, characterize the reachable outcomes rather than claiming an exact expected number.

If useful and straightforward, calculate the number of eligible Tanker update opportunities between spawn and release under the literal current/reference constants.

Clearly distinguish deterministic arithmetic from randomized outcomes.

6. Determine whether the current C# implementation materially diverges from Incitti.

This is especially important now because Neal's fresh play suggests Incitti's timing is an unusually strong practical comparison target.

If C# differs from Incitti in firing cadence/order/timing, identify exactly where and whether that difference could explain the earlier single-shot observation.

If C# already implements the literal Incitti firing algorithm accurately, state that clearly.

Do not change anything yet.

7. Inspect existing automated Tanker firing tests.

Identify:
- what is already protected;
- what is not;
- whether tests currently prove ammo/timer/chance/release behavior;
- whether deterministic random seams are sufficient to exercise zero/one/multiple-shot paths without changing production randomness.

Recommend focused characterization/acceptance tests for C2.

8. Recommend exactly one C2 policy.

Choose among these general outcomes, or define a more precise alternative if the evidence requires it:

A. Preserve the current firing policy unchanged and promote it for Tanker use after manual validation.

B. Correct current C# behavior to literal Incitti behavior because C# currently diverges and the divergence is not supported by arcade evidence.

C. Make a narrow evidence-supported tuning change because the current/Incitti behavior demonstrably conflicts with Neal's arcade observation.

D. Evidence remains insufficient; preserve firing as development-only and identify the exact manual comparison needed.

Do not choose based on aesthetics or code simplicity.

9. Define the next implementation/manual gate.

If an implementation is warranted, specify the narrowest safe C2 implementation:
- files expected to change;
- tests required;
- exact behaviors protected;
- development launch command(s);
- manual observations Neal should perform.

Manual validation must remain qualitative unless exact arcade timing evidence exists.

At minimum it should make it practical for Neal to observe whether a Tanker can fire multiple individual shots before release.

10. Preserve boundaries.

Do not change:
- Level 3 mixed-wave composition;
- C1/C1.5 geometry;
- Tanker cargo routing;
- Tanker cargo inward spatial pivot;
- Super Zapper semantics;
- Flipper firing policy;
- Level 1/2 behavior;
- spinner movement;
- rendering fidelity;
- controller mappings;
- Reference/incitti;
- natural progression.

11. Documentation.

Analysis only:
- Do not modify TEMPEST-BEHAVIOR-SPEC.md.
- Do not modify README.md.
- Do not modify TEMPEST-DEVELOPMENT-NOTES.md unless the established project workflow absolutely requires an analysis pointer; prefer the AI report only.

Save this exact prompt under AI-Logs/Prompts following the established naming convention.

Create a C2 analysis report under AI-Logs/Reports.

12. Final maintenance.

Because this is analysis-only, no gameplay source/test files should change.

At the end run:

git status --short
git diff --check
git diff --stat
git status --short -- Reference/incitti
git log -1 --oneline

Do not commit.
Do not push.

Final report must include:

- exact current C# Tanker firing algorithm;
- exact Incitti Tanker firing algorithm;
- side-by-side differences;
- evidence classification;
- what zero/one/multiple-shot outcomes are possible under each;
- whether current C# accurately implements Incitti;
- relationship to Neal's qualitative Atari observation;
- recommended C2 policy;
- proposed automated test gate;
- proposed manual validation gate;
- exact next implementation scope if warranted;
- unresolved uncertainties;
- GO/NO-GO for a C2 implementation;
- explicit confirmation that C3 and natural Level 3 were not started.
