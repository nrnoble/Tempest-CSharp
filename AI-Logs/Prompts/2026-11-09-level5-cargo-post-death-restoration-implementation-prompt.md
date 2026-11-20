FAIL-CLOSED PROMPT CHECK

Read this entire substantive user task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive user
task.

Automatically appended VS Code/agent context after the user's completion token
does not invalidate the task.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Implement a bounded Level 5 cargo-specific post-death restoration correction
on top of the existing uncommitted Level 5 concurrency candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the existing candidate:

62c66e0ab19436ea162935627021154be21e0a41

The existing Level 5 admission-and-terminal-Spiker candidate is already present
in the worktree and has completed automated validation.

Read these reports before modifying behavior:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md

AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md

Treat the post-death investigation as the direct causal handoff for this task.

Do not redo that investigation unless a narrowly specific implementation
question requires verification.


MANUAL FAILURE BEING CORRECTED

The existing Level 5 concurrency candidate is NOT yet manually accepted.

Neal established:

- If the Blaster clears Level 5 without dying, only Tankers and Spikers enter
  independently from the deep/bottom end.
- No individual Flippers were observed entering independently from the bottom
  on that no-death path.
- If a Tanker has released cargo and the Blaster dies while a cargo Flipper
  remains unresolved, an individual Flipper can later appear from the deep end
  during post-death reconstruction/reentry.

Neal's arcade criterion is definitive:

On original arcade Level 5, individual Flippers never independently enter from
the bottom, including after Blaster death/reentry.

Level 5 Flippers originate from Tanker cargo.

The correction must eliminate the post-death individual bottom-entry
presentation without reopening ordinary Level 5 admission.


CONFIRMED CAUSE

The read-only investigation established the exact current path:

1. A Tanker owns two stable cargo Flipper identities.
2. Cargo is materialized normally from the Tanker.
3. A materialized cargo Flipper remains unresolved when the Blaster dies.
4. Death reconstruction preserves that cargo identity but moves it to
   ReentryReserve.
5. Cargo reentry reserve is preferentially released as a new Egg.
6. That cargo Egg follows the normal deep-end approach.
7. The Egg hatches through CreateFreshTankerCargo.
8. The Flipper is created at height 0.
9. It therefore appears as an independent deep-end Flipper.

This path predates the current Level 5 concurrency candidate.

The new Level5AdmissionPolicy is NOT ordinary-selecting those Flippers.

Fresh unselected Level 5 originals correctly remain restricted to Tanker and
Spiker.

Do not "fix" this by altering ordinary Level 5 species selection.


PURPOSE

Preserve stable cargo identity/accounting while eliminating the arcade-
inconsistent post-death Egg/deep-entry presentation for already-materialized
unresolved Level 5 Tanker cargo.

Implement the smallest safe Level-5-specific restoration translation.

The preferred bounded design established by the investigation is:

- capture sufficient runtime state for unresolved materialized Level 5 cargo
  Flippers before death clears runtime enemy collections;
- preserve their existing ledger identity, owner, ordinal, species, and
  completion obligation;
- after death/reentry, restore those identities as cargo-specific active
  in-tube Flippers rather than placing them in ReentryReserve/Egg;
- do not recreate them at height 0 through CreateFreshTankerCargo;
- do not reconstruct a terminal/escaped Tanker merely to display them.

Do not assume additional arcade behavior beyond what Neal established.

The exact captured-position restoration is a PROJECT TRANSLATION chosen to
avoid the known-wrong bottom-entry presentation while retaining the project's
stable identity model.


LEVEL 5 ONLY

This correction is specifically for canonical production Level 5.

Do not silently alter the established reconstruction presentation for Levels
1-4 or Levels 6-8.

Do not generalize this into a project-wide cargo reconstruction redesign.

Copied/synthetic configurations should remain unaffected unless a focused test
deliberately constructs the canonical Level 5 behavior required to test this
correction.


STATE TO PRESERVE

For each materialized unresolved Level 5 cargo Flipper that qualifies for this
restoration, preserve enough state to restore an appropriate active in-tube
representation.

At minimum consider and preserve where genuinely required:

- CargoUnitId;
- owner original ID;
- cargo ordinal;
- Flipper species;
- lane;
- height/depth;
- routing/direction state needed to avoid malformed movement;
- any existing movement/pivot state that must safely survive reconstruction;
- lifecycle state required for correct collision/rendering;
- identity/completion accounting.

Do not preserve transient state merely because it exists.

Do not preserve stale references to runtime objects that are destroyed during
death reset.

Prefer a small immutable/value snapshot keyed by stable cargo identity if a
snapshot is necessary.


ELIGIBILITY

The special restoration path applies only to materialized, unresolved Level 5
cargo identities that would otherwise be reconstructed as individual cargo
Eggs.

Do not apply it to:

- latent cargo that has never been released;
- destroyed cargo;
- rim-cleared cargo;
- otherwise terminal cargo identities;
- fresh ordinary originals;
- retained Tanker originals;
- unresolved cargo from unrelated levels unless separately authorized.

Latent cargo must remain associated with its Tanker lifecycle.

Resolved cargo must remain resolved.


NATURAL / SHOT / ZAPPER VARIANTS

Preserve the established identity semantics for materialized cargo created by:

- natural Tanker release;
- player-shot Tanker release;
- Super Zapper paths where a child is materialized and remains unresolved.

The investigation established that materialized unresolved children can share
the ReentryReserve/Egg defect regardless of release origin.

Do not rewrite Tanker release itself.

Do not alter accepted:

- natural release;
- shot release;
- deferred shot activation;
- Super Zapper targeting/timing;
- scoring;
- stillborn/zap lifecycle.

If a cargo child is still in a legitimate committed pending-activation or
pending-zap representation at the exact death boundary, determine the smallest
correct restoration mapping from existing lifecycle semantics rather than
losing or duplicating the identity.


POST-DEATH RESTORATION TIMING

Integrate the restoration into the existing Level 5 death/reentry lifecycle
without changing the accepted Blaster death timing, freeze, tube presentation,
or player reentry sequence.

Do not make restored enemies attack or collide during a period in which normal
enemy gameplay is frozen.

Restore them at the appropriate existing lifecycle boundary when normal enemy
activity resumes.

Preserve deterministic simulation ordering.


PHYSICAL CAPACITY

Restoration must respect the candidate's physical-capacity accounting without
losing identities.

If all eligible cargo Flippers cannot safely become active at the restoration
boundary because physical headroom is unavailable:

- preserve their stable restoration snapshots/identities;
- restore as physical headroom becomes available;
- do not turn them into Eggs;
- do not send them to the deep-entry presentation;
- do not discard or resolve them;
- do not reroll them;
- do not repeatedly consume randomness while waiting.

This waiting mechanism is cargo-specific restoration, not ordinary Level 5
admission.


POSITION AND MOVEMENT TRANSLATION

The investigation establishes that the current bottom-entry restoration is
wrong, but it does not independently establish the arcade machine's exact
post-death placement for every released cargo state.

For this candidate, restoring the unresolved materialized cargo at its captured
pre-death lane and height is authorized as a PROJECT TRANSLATION.

Preserve enough movement state to make the restored Flipper continue
sensibly from that in-tube position.

Do not introduce an arbitrary lane change, height reset, or new Egg approach.

Do not call this literal ROM reconstruction.

Neal will determine through manual play whether the resulting presentation
matches the arcade.


DO NOT RECREATE THE CARRIER

Do not solve this by reconstructing a Tanker that had already escaped,
released cargo, or been destroyed merely so the children can be released
again.

That would alter terminal carrier state, scoring, identity ownership, and
visible history.

Keep terminal carrier outcomes terminal.

Restore the unresolved cargo identities themselves.


KEEP ORDINARY LEVEL 5 ADMISSION FROZEN

Do not change the now-validated Level 5 admission candidate:

- ordinary Tanker/Spiker selection only;
- no ordinary Flipper selection;
- F minimum/maximum 0/5;
- T minimum/maximum 1/1;
- S minimum/maximum 2/3;
- Spiker-first unmet minimum;
- shortest-Spike post-minimum translation;
- seven-place ordinary physical headroom;
- latent-cargo availability reservation;
- blocked-Egg waiting;
- terminal Spiker-to-Tanker conversion;
- converted-Tanker reentry.

Those systems are outside this correction unless a direct implementation
dependency requires a tiny mechanical adjustment.

Do not retune them based on this presentation defect.


PRESERVE OTHER ACCEPTED SYSTEMS

Do not redesign or retune:

- Level 5 tube geometry;
- Level 4-to-5 progression;
- Level 5 completion/progression;
- starting/persistent Spikes;
- Flipper movement program;
- Tanker movement;
- Spiker growth/retreat behavior;
- Tanker cargo release presentation;
- enemy firing;
- shared shots;
- scoring;
- Super Zapper lifecycle;
- rim combat;
- all-rim behavior;
- player death/reentry timing;
- stable identity conservation;
- recording/playback determinism;
- Levels 1-4;
- Levels 6-8.


TESTS

Add a narrowly focused regression test that expresses Neal's missing
arcade-visible invariant.

Primary deterministic reproduction:

1. Use canonical Level 5.
2. Create/identify one original Tanker with a known stable original ID.
3. Establish its two cargo owner/ordinal identities.
4. Materialize the cargo through an established Tanker release path.
5. Resolve one child if useful and leave at least one child active/unresolved.
6. Give that unresolved cargo Flipper a known nonzero in-tube lane/height.
7. Trigger Blaster death/reconstruction.
8. Continue through player/enemy reentry.
9. Verify the same cargo identity, owner, ordinal, and Flipper species remain
   conserved.
10. Verify that identity is NOT staged as a cargo Egg.
11. Verify it is NOT recreated at deep-entry height 0 through
    CreateFreshTankerCargo.
12. Verify it returns through the new cargo-specific in-tube restoration path.
13. Verify appropriate lane/height restoration within the chosen project
    translation.
14. Verify it remains an unresolved completion obligation until legitimately
    resolved.

Add focused variants sufficient to protect the causal boundaries:

- natural Tanker cargo release;
- player-shot release;
- one resolved sibling / one unresolved sibling;
- both unresolved children;
- latent pair at death: no individual child restoration;
- fully resolved cargo: no restoration;
- converted Tanker materialized cargo where practical;
- pending shot activation at death;
- an unresolved Zapper-materialized child where existing semantics make that
  reachable;
- physical-capacity blocking and later restoration without Egg presentation.

Update the existing test that currently expects reconstructed Level 5 cargo to
appear as an Egg:

ReconstructedLevelFiveTankerCargoHasDirectionBeforeSpikeGatedPivot

That old expectation encoded the now-rejected visible presentation.

Preserve any still-valid identity/direction/pivot assertions by moving them to
the corrected restoration representation rather than simply deleting useful
coverage.

Also preserve ledger tests for owner/ordinal conservation and latent/terminal
cargo behavior.


NO-DEATH CONTROL

Keep or add a control verifying the already-correct no-death Level 5 behavior:

- fresh originals do not ordinary-select Flipper;
- normal Tanker release produces cargo in-tube;
- no independent bottom-entry cargo reconstruction occurs without death.

Do not confuse the new death-restoration path with ordinary admission.


VALIDATION

Use the repository-wide bounded validation/security rules.

Prefer the Debug validation route that successfully executed the current Level
5 candidate.

Start with the smallest new cargo-restoration tests.

Then run focused Level 5 admission/production and death/reconstruction tests.

If focused validation passes, run affected Debug regressions covering:

- Level 5 admission;
- Tanker cargo;
- ledger identity conservation;
- death/reentry;
- converted Tankers;
- pending cargo activation;
- Super Zapper;
- persistent Spikes;
- completion/all-rim;
- direct/campaign/recorded startup;
- Levels 1-4 concurrency regression.

If affected validation passes, run:

dotnet test TempestCSharp.slnx --configuration Debug --blame-hang-timeout 5min

Then:

dotnet build TempestCSharp.slnx --configuration Debug

Also run:

- git diff --check;
- complete tracked-diff review;
- final repository-state verification.

If Windows Application Control blocks a generated artifact, follow the
repository-wide self-stop policy.

Do not bypass security.
Do not repeat equivalent blocked routes.
Do not report a blocked command as passed.


MANUAL HANDOFF

If automated validation succeeds, leave the candidate uncommitted and stop for
Neal's manual test.

Do not launch the game.

Provide:

dotnet run --project src\Tempest.Game -- --level 5 --lives 10

Primary manual acceptance scenario:

1. Play until a Tanker releases Flippers.
2. Leave at least one released Flipper unresolved in the tube.
3. Allow the Blaster to be destroyed.
4. Reenter.
5. Verify that no individual Flipper subsequently appears from the deep/bottom
   end.
6. Verify the surviving cargo behavior looks natural and consistent with the
   arcade.
7. Repeat with more than one death if practical.
8. Verify Tanker/Spiker ordinary admission remains correct.
9. Verify Tanker cargo release remains normal.
10. Verify death/reentry, Spikes, Super Zapper, rim combat, and completion
    remain normal.

If this specific post-death behavior passes, Neal can resume the broader Level
5 concurrency acceptance checklist.

Do not claim manual acceptance before Neal reports it.


DOCUMENTATION AND REPORT

This is still an implementation candidate.

Do not update TEMPEST-DEVELOPMENT-NOTES.md or TEMPEST-BEHAVIOR-SPEC.md to claim
the Level 5 concurrency candidate is accepted.

Preserve the existing implementation/validation/investigation reports as
historical records.

Create a new paired prompt/report such as:

AI-Logs/Prompts/2026-11-09-level5-cargo-post-death-restoration-implementation-prompt.md

AI-Logs/Reports/2026-11-09-level5-cargo-post-death-restoration-implementation-report.md

Record:

- confirmed defect cause;
- files changed;
- cargo snapshot/restoration design;
- eligibility boundaries;
- physical-headroom waiting;
- project-translation classification;
- tests added/updated;
- superseded cargo-Egg test expectation;
- focused/affected/full validation;
- build result;
- diff/repository state;
- unresolved fidelity questions;
- exact manual command and acceptance scenario;
- explicit manual-acceptance status.


GIT AND STOP

Do not commit or push.

Even if all automated validation passes, stop with the entire Level 5 candidate
uncommitted.

Do not begin Level 6 concurrency.

Do not perform unrelated cleanup.


TEMPEST_LEVEL5_CARGO_POST_DEATH_RESTORATION_IMPLEMENTATION_COMPLETE