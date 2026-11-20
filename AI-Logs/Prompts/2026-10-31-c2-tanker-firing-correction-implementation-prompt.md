Proceed with the narrowly approved C2 Tanker firing correction from the completed analysis.

Current clean checkpoint:
27c43b7 Add validated controller restart and quit aliases

Authority report:
AI-Logs/Reports/2026-10-31-c2-tanker-firing-policy-analysis-report.md

This is an implementation and automated-validation task, followed by manual validation by Neal.

Do not begin C3.
Do not implement production Level 3 mixed composition.
Do not add natural Level 2 -> Level 3 progression.
Do not promote Tanker firing into production yet.

Approved C2 policy

Implement Policy B from the analysis:

- Correct the isolated/current Tanker default climb speed from 1.85f to 1.75f.
- Preserve the existing Tanker firing algorithm and all existing firing constants.
- Keep Tanker firing development-only behind the existing combined:
  --tanker-test --enemy-bullets-test
- Keep --tanker-test alone silent.
- This is a literal Incitti reference-fidelity correction, not arcade-measured tuning.
- Do not guarantee that a Tanker fires twice.
- Do not increase ammunition.
- Do not change the firing probability, timer, cutoff, release threshold, or bullet behavior.

Evidence boundary

Literal Incitti displayed-Level-3 Tanker speed is:

1.75 + lv / 20

At displayed Level 3, lv = 2.

Because lv and 20 are integer operands in classic BlitzMax:

2 / 20 = 0

Therefore the literal result is 1.75, not 1.85.

Treat this as REFERENCE VERIFIED.

Neal’s qualitative arcade observation remains:

- Tankers fire individual shots at intervals.
- At least two Tanker shots can occur before release.

That observation proves capability, not a guaranteed shot count or exact hidden constants.

Implementation requirements

1. Verify the baseline:

git status --short
git log -3 --oneline
git diff --check
git status --short -- Reference/incitti

The worktree must be clean at 27c43b7.

2. Read the C2 analysis report and inspect the current source/tests before editing.

Confirm the approved scope still matches:
- Tanker.cs default speed;
- directly coupled Tanker firing tests;
- related documentation only.

If current source contradicts the report, stop and explain the contradiction rather than broadening scope.

3. Change only the Tanker default development/reference-tier climb speed:

- 1.85f -> 1.75f

Update the nearby explanatory comment/documentation so it accurately states:
- the early reference-tier expression uses integer lv / 20 semantics;
- displayed Level 3 therefore uses 1.75;
- this is reference-derived, not arcade-measured timing.

Do not introduce a new general tuning framework merely for this correction.

4. Do not change the existing firing algorithm:

Preserve:
- ammunition range 1..2;
- initial timer 16 + inclusive 1..64, yielding 17..80;
- climb before firing decision;
- strict post-climb Height < 380 firing eligibility;
- decrement timer once per eligible update;
- decision only when timer becomes < 0;
- timer reset to 16 on every decision;
- inclusive 0..100 roll;
- fire only when roll > 75;
- ammunition consumed only on success;
- at most one bullet per update;
- strict natural release at Height > 385;
- no firing on the release-triggering update;
- independent bullet lifetime;
- existing death, Zapper, all-rim, scoring and cleanup behavior.

5. Preserve development gating:

- --tanker-test alone must remain silent.
- --tanker-test --enemy-bullets-test must retain the armed Tanker path.
- --enemy-bullets-test must remain orthogonal.
- Do not promote normal Flipper bullets.
- Do not create a Tanker-only production firing switch.
- Do not add a forced-success manual-debug option.

6. Add focused deterministic acceptance coverage using existing random seams.

Required final acceptance behavior for the default 1.75f speed:

- Constructor height is 10.
- First ordinary/hatch update reaches 11.75.
- There are 211 firing-eligible climb updates under uninterrupted ordinary climb.
- Last eligible height is 379.25.
- First silent near-release update reaches 381.
- Natural release occurs on update 215 at height 386.25.
- Existing explicit-speed strict-boundary tests remain valid.

Add a whole-climb deterministic matrix covering:

- ammo 1 and ammo 2;
- initial offsets 1 and 64;
- all decision rolls fail -> zero bullets;
- exactly one success -> one bullet;
- ammo 2 with two successful rolls -> two bullets;
- ammo 1 with all successful rolls -> exactly one bullet;
- no decisions after ammunition is exhausted;
- no countdown or draw at/above the firing cutoff;
- decision roll 75 fails and 76 succeeds;
- first decision is T + 1 eligible updates;
- later decisions are 17 eligible updates apart.

Use a strict range-aware random test double that:
- verifies requested random ranges;
- fails on unexpected draws;
- does not silently clamp broken values.

Count bullet creation events, not only simultaneously active bullets.

7. Add the late-window regression identified by the analysis:

Use:
- ammo = 2;
- initial timer T = 80;
- decision failures at updates 81, 98, 115, 132, 149 and 166;
- successes at updates 183 and 200.

Verify:
- corrected 1.75f speed permits two bullet creations before release;
- this proves capability only;
- production randomness still does not guarantee two shots.

Do not retain an acceptance assertion for the old 1.85f behavior.

8. Add one game-level timed multiple-shot path through normal updates.

Verify:
- two bullets can be created by one Tanker before natural release;
- Tanker keeps climbing while firing;
- bullets use the Tanker lane and post-climb origin;
- first bullet motion still begins on the next game step under the current C# contract;
- natural release removes the carrier but not existing bullets;
- no Tanker bullet is created on the release-triggering update;
- score/accounting remain unchanged by firing itself;
- cargo behavior remains unchanged.

Arrange the fixture safely so the player is not killed and the Tanker is not destroyed during the observation.

9. Add or preserve an armed-pending-Zapper regression:

- A pending non-expiring Tanker zap does not stop ordinary climb/firing.
- On the expiry update, normal movement/firing does not occur.
- Existing Tanker bullets survive carrier zap resolution.
- Existing carrier/cargo scoring and identities remain unchanged.

Put the test in SimulationTests.cs or SuperZapperTests.cs according to the established organization. Do not change production Zapper code unless a real regression is discovered, and stop before broadening scope if that occurs.

10. Update directly coupled existing expectations:

Examples include default hatch height changing from approximately 11.85 to 11.75 and any test that currently locks the unsupported 1.85 default.

Do not weaken unrelated assertions.

11. Documentation:

Update TEMPEST-DEVELOPMENT-NOTES.md concisely to record:
- the literal early-tier Tanker speed correction;
- firing algorithm remains unchanged;
- firing remains development-only pending manual validation;
- exact arcade ammo/timer/chance values remain unknown;
- two-shot behavior is possible, not guaranteed.

Update TEMPEST-BEHAVIOR-SPEC.md only if its established evidence model and current text require correcting an active factual claim. If changed:
- label the speed REFERENCE VERIFIED;
- do not promote it to ARCADE VERIFIED;
- preserve Neal’s qualitative two-shot capability observation separately.

Update README.md only if current user-facing development documentation contains the incorrect 1.85 claim or needs clarification of the existing manual command. Do not add a new launch mode.

12. Preserve all unrelated systems.

Do not change:
- C1/C1.5 geometry or directed rotation;
- inward spatial pivots;
- Tanker cargo routing;
- cargo identities/accounting;
- Tanker release thresholds;
- Super Zapper semantics;
- Flipper firing policy;
- Level 1 or Level 2 behavior;
- spinner movement;
- entity rendering;
- controller mappings;
- Reference/incitti;
- production composition;
- progression.

13. Save this exact prompt under AI-Logs/Prompts using the established naming convention.

Create/update the C2 implementation report under AI-Logs/Reports with:
- exact speed correction;
- unchanged firing algorithm;
- tests added/updated;
- intermediate failures and corrections;
- focused/full validation;
- manual checklist;
- evidence labels and remaining uncertainty.

14. Validation.

Run focused tests covering Tanker, bullets, Super Zapper and all-rim behavior.

Then run:

dotnet test TempestCSharp.slnx --no-restore
dotnet build TempestCSharp.slnx --no-restore
git diff --check

Inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git status --short -- TEMPEST-BEHAVIOR-SPEC.md
git log -1 --oneline

Do not commit.
Do not push.
Do not begin C3.

Final report must include:

1. Exact production code change.
2. Confirmation that Tanker firing constants/algorithm are otherwise unchanged.
3. Focused test results.
4. Full solution test results.
5. Build result.
6. git diff --check result.
7. Exact changed and untracked files.
8. Reference/incitti status.
9. Behavior-specification status and evidence labels.
10. Confirmation that firing remains development-only.
11. Confirmation that C3 and progression were not started.
12. Any blocker to manual validation.
13. A short manual checklist using:

dotnet run --project src/Tempest.Game -- --tanker-test --lives 10

dotnet run --project src/Tempest.Game -- --tanker-test --enemy-bullets-test --lives 10

Manual validation should establish:

- silent control remains silent;
- combined mode can produce separated Tanker bullets before release;
- at least one uninterrupted Tanker instance can be observed firing two individual shots before natural release;
- Tanker keeps climbing while firing;
- near-release region remains silent;
- released or destroyed Tanker does not erase an existing bullet;
- cargo, Zapper and input behavior remain normal;
- no exact shot count is required for every Tanker.

Do not commit until Neal reports the manual result.
