C3 — Level 3 mixed-wave identities and direct production-wave access
PRE-IMPLEMENTATION ANALYSIS ONLY

Do not implement C3 yet.

Current clean checkpoint:
6d15165 Correct and validate isolated Tanker firing timing

Prior validated checkpoints include:
27c43b7 Add validated controller restart and quit aliases
11773c0 Implement Level 3 geometry and directed rotation

C1/C1.5 is manually accepted.
C2 Tanker firing is manually accepted and committed.

Authority inputs:
- TEMPEST-BEHAVIOR-SPEC.md
- TEMPEST-DEVELOPMENT-NOTES.md
- AI-Logs/Reports/2026-10-31-level3-tanker-progression-analysis-report.md
- AI-Logs/Reports/2026-10-31-level3-c1-geometry-implementation-report.md
- AI-Logs/Reports/2026-10-31-c2-tanker-firing-policy-analysis-report.md
- AI-Logs/Reports/2026-10-31-c2-tanker-firing-correction-implementation-report.md
- current C# source/tests
- literal Reference/incitti source

Purpose

Prepare the exact implementation plan for C3 only:

- Production-shaped Level 3 configuration.
- Direct terminal --level3-test access.
- Flipper/Tanker mixed hatching.
- Stable original-unit identities.
- Separate Tanker cargo identities.
- Zero/one/multiple Tankers, including the six-active-Tanker source cap.
- Shot, natural-release and Super-Zapper Tanker outcomes.
- Multi-carrier Super Zapper behavior.
- Death/re-entry identity conservation.
- All-rim/completion accounting.
- Validated C2 Tanker firing policy in the Level 3 design.

C3 must make the production-shaped Level 3 wave directly testable, but natural Level 2 -> Level 3 progression remains C4 and must NOT be implemented here.

Evidence hierarchy

Preserve the established evidence hierarchy exactly:

1. Neal's direct Atari arcade observations.
2. Mark Incitti's implementation as an engineering/reference source.
3. Current tested C# behavior.
4. Explicit new project decisions only where required.

Do not promote Incitti-only behavior to ARCADE VERIFIED.

Important accepted behavior to preserve

- C1/C1.5 directed geometry and inward pivot behavior are accepted.
- Full 90/180/270 directed sweep determines transition duration.
- Enemy visible pivots swing inward through the web.
- Tanker cargo retains the previously accepted outward lane route while its visible pivot swings inward.
- Existing cargo behavior/accounting in the isolated Tanker scenario must not regress.
- C2 Tanker firing uses the validated 1.75 reference-derived climb speed.
- Tanker firing can naturally produce two separated shots before release but does not guarantee two.
- Exact Atari Tanker firing constants remain unknown.
- Existing Level 1 and Level 2 behavior must remain unchanged.
- Flipper bullets are NOT promoted merely because Tanker firing has been accepted.

Tasks

1. Verify baseline:

git status --short
git log -3 --oneline
git diff --check
git status --short -- Reference/incitti

Confirm HEAD = 6d15165 and a clean worktree.

2. Re-read the earlier Level 3 progression analysis, but do not assume its proposed C3 architecture is automatically correct.

Compare it against:
- the final post-C1/C1.5 source;
- final post-C2 source;
- current tests;
- behavior spec;
- development notes;
- literal Incitti source.

Identify anything in the old C3 plan that is now stale because C1/C1.5 or C2 changed the implementation.

3. Establish the exact Level 3 configuration that C3 would use.

Confirm from literal source and current accepted architecture:

- internal/displayed identity;
- board/palette identity;
- exact plus-board geometry already introduced by C1;
- closed topology and playable lane count;
- player starting lane/position;
- depth/position;
- original enemy supply;
- initial Eggs/reserve;
- which enemy types are eligible;
- Level 3 Tanker cargo type;
- Tanker firing policy;
- Flipper tuning/canflip policy;
- Super Zapper initialization;
- terminal behavior for direct --level3-test.

Clearly classify every value as REFERENCE VERIFIED, PROJECT DECISION, DEVELOPMENT ONLY, or existing ARCADE VERIFIED observation.

Do not invent production settings simply because a development scenario already has them.

4. Analyze the exact mixed-wave selection algorithm needed.

Compare literal Incitti PickEnemy behavior with current C# hatching.

Establish:

- how Flipper vs Tanker selection occurs;
- relevant random ranges/retries;
- six-active-Tanker cap semantics;
- whether the cap is simultaneous or per-wave;
- behavior after a Tanker leaves and another Egg later hatches;
- near-end selection differences if relevant;
- which source enemy candidates are disabled at displayed Level 3;
- how deterministic tests can force zero, one and multiple Tankers without making production composition deterministic.

The production path must not hard-code original slot 0 as Tanker.

5. Re-evaluate the proposed identity model against current code.

The previous analysis proposed stable original IDs 0..14 plus separate child identities.

Inspect the current post-C2 accounting implementation and determine whether that remains the narrowest safe model.

For every original identity, define legal states such as:

- reserve;
- Egg;
- active Flipper;
- active Tanker;
- destroyed;
- escaped;
- rim-cleared.

For every Tanker-owned cargo identity, define:

- latent;
- queued/pending activation if applicable;
- active child;
- reconstructed Egg;
- destroyed;
- rim-cleared.

Specify exact invariants preventing:
- duplication;
- disappearing obligations;
- double scoring;
- cargo counted twice;
- latent cargo becoming independent Eggs while the carrier is also reconstructed.

Do not migrate Level 1/2 to the Level 3 production ledger unless absolutely required. The previous design specifically intended Level 1/2 to remain on their established simpler accounting.

6. Analyze each Tanker resolution path end-to-end.

For a production Level 3 Tanker, specify exact identity/accounting consequences for:

A. Natural release
B. Shot destruction
C. Super Zapper delayed carrier resolution
D. All-rim clearing
E. Death before cargo release
F. Death after cargo release
G. Death after only one child has resolved
H. Repeated deaths

For every case identify:
- carrier terminal state;
- whether Tanker score is awarded;
- cargo materialization;
- child identities;
- child score eligibility;
- reconstruction representation;
- pending timers;
- completion blockers.

Preserve existing accepted Tanker/cargo behavior unless production identity generalization genuinely requires a structural change.

7. Analyze multiple simultaneous Tankers.

C3 must not merely make the current single-Tanker diagnostic larger.

Establish how the production design safely handles:
- zero Tankers;
- one Tanker;
- several Tankers;
- six simultaneously active Tankers;
- a later Tanker after an earlier carrier resolves;
- several carriers each owning two cargo identities;
- overlapping natural, shot and zap resolution paths.

Prove conceptually that carrier/cargo identity namespaces cannot collide.

8. Analyze multi-carrier Super Zapper behavior.

Use the already accepted Super Zapper semantics.

Establish tests/design for:
- first all-target activation with multiple Tankers;
- staggered carrier timers;
- unique stillborn children per carrier;
- child timers;
- exact-once 200-point carrier scoring;
- exact-once 150-point child scoring where applicable;
- second single-target activation;
- shot pre-emption before zap expiry;
- death while carriers or children have pending timers;
- no stale timer resolving a removed identity;
- all-rim interaction with pending targets.

Do not redesign Super Zapper globally unless analysis proves the current architecture cannot safely support multiple production Tankers.

9. Analyze all-rim and completion using identity-set equality, not merely counts.

C3 must prove that completion cannot begin while any unresolved obligation is lost or unrepresented.

Identify blockers including:
- reserve originals;
- active Eggs;
- below-rim Flippers;
- below-rim Tankers;
- latent Tanker cargo;
- queued cargo;
- below-rim active cargo;
- reconstructed cargo Eggs;
- pending zap targets as applicable.

Maintain distinct outcomes:
- destroyed;
- natural Tanker escape;
- rim-cleared.

No kill score for natural escape or rim-clear.

10. Analyze death/re-entry thoroughly.

Use current project policy:
- resolved outcomes remain resolved;
- unresolved obligations reconstruct;
- runtime enemy/zap state is discarded as established;
- score/lives policies remain unchanged.

Determine how selected Tanker identity and its latent children survive death safely.

The prior analysis proposed retaining the selected Tanker type across reconstruction so a carrier cannot re-hatch as a Flipper and lose its latent obligations.

Confirm whether that is still the safest explicit PROJECT DECISION.

Include repeated-death cases and compare identity sets before/after reconstruction.

11. Define direct --level3-test behavior.

It must:
- use the exact same Level 3 configuration and randomized production selection policy intended for future natural progression;
- be directly launchable for C3 manual validation;
- remain terminal after Level 3 completion;
- replay/restart appropriately using existing project conventions;
- not create a fixed/debug Tanker composition;
- remain distinct from --level3-geometry-test;
- not enable Level 2 -> Level 3 progression.

Any deterministic forcing belongs in automated test seams, not production/manual --level3-test behavior.

12. Incorporate the accepted C2 firing policy carefully.

Determine how production Level 3 Tankers become armed without accidentally promoting ordinary Flipper bullets through the existing --enemy-bullets-test mechanism.

C2 acceptance does not mean:
- ordinary Flippers should fire by default;
- the existing combined diagnostic flag should determine production Level 3 composition.

Propose the narrow production Tanker arming architecture consistent with accepted C2 behavior.

13. Define automated gates.

At minimum cover:

Configuration/composition:
- exact Level 3 identity/config;
- zero/one/multiple Tankers;
- six-active cap;
- Tanker eligibility returning after a carrier resolves;
- only Flipper/Tanker choices;
- production randomness unforced.

Identity/accounting:
- every original identity represented exactly once;
- every latent/materialized cargo identity represented exactly once;
- natural, shot, zap and rim-clear paths;
- no duplicate/lost IDs;
- exact-once scoring.

Death:
- before release;
- after natural release;
- after shot release;
- after zap carrier resolution;
- during child zap timers;
- partially resolved children;
- multiple carriers;
- at least two consecutive deaths.

Super Zapper:
- multiple Tankers;
- staggered carrier/child timers;
- first and second charge;
- shot pre-emption;
- no duplicate score.

Completion/all-rim:
- every unresolved-obligation class blocks correctly;
- all-rim clears without kill score;
- no stale pending score after removal;
- terminal direct Level 3 behavior.

Regression:
- Level 1;
- Level 2;
- isolated Tanker;
- C1/C1.5 geometry;
- C2 firing;
- Spiker;
- Fuseball;
- Pulsar;
- controller input.

Do not prescribe fake automated USB or graphical tests.

14. Define the C3 manual gate.

Use direct --level3-test only after implementation.

The manual gate should cover:
- plus-board geometry remains correct;
- randomized mixed Flipper/Tanker composition is visibly present;
- Tanker count can vary;
- Tanker firing behavior remains qualitatively accepted;
- shot Tanker releases correct live cargo;
- natural Tanker release;
- death/re-entry before and after cargo materialization;
- multiple Tankers if naturally observed;
- Super Zapper carrier/child behavior;
- all-rim;
- clean terminal Level 3 replay/restart;
- no regression in input or obvious rendering.

Do not require Neal to measure exact timers or force random outcomes manually when deterministic automated tests already establish them.

15. Decide whether C3 should remain one implementation milestone or be split.

This is important.

Based on actual post-C2 architecture and risk, explicitly recommend either:

A. Implement C3 atomically as originally planned.

or

B. Split C3 into smaller independently safe checkpoints, specifying the exact boundaries and why.

Prefer splitting if identity/accounting and multi-carrier Zapper/completion risk would make one implementation prompt excessively broad or difficult to manually isolate.

Do not split merely to create more commits. Split only for engineering/validation reasons.

16. Do not implement anything in this task.

No source/test/project/documentation changes other than ignored AI-Logs analysis deliverables.

Do not edit:
- README.md
- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md
- Reference/incitti

Do not begin C4.

17. Save this exact prompt under AI-Logs/Prompts following the established naming convention.

Create a detailed C3 pre-implementation analysis report under AI-Logs/Reports.

18. Final maintenance:

git status --short
git diff --check
git diff --stat
git status --short -- Reference/incitti
git log -1 --oneline

The tracked worktree must remain clean.

Do not commit.
Do not push.

Final report must include:

- current architecture findings;
- anything stale in the old C3 plan;
- exact proposed Level 3 configuration;
- exact proposed mixed selection policy;
- identity model and invariants;
- Tanker/cargo lifecycle table;
- multi-carrier design;
- Super Zapper design;
- death/re-entry design;
- all-rim/completion design;
- production Tanker firing integration;
- direct --level3-test design;
- automated acceptance matrix;
- manual acceptance checklist;
- recommendation: atomic C3 or split C3;
- exact next implementation scope;
- unresolved arcade/reference uncertainties;
- GO/NO-GO;
- explicit confirmation that natural Level 2 -> Level 3 progression/C4 was not started.