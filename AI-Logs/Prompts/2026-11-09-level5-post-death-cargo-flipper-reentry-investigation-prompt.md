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

Perform a bounded READ-ONLY investigation of a Level 5 death/reconstruction
presentation defect in the current uncommitted admission-and-terminal-Spiker
candidate.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the uncommitted candidate:

62c66e0ab19436ea162935627021154be21e0a41

The Level 5 candidate already exists in the worktree and has completed
automated validation.

Read these handoff reports:

AI-Logs/Reports/2026-11-09-level5-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-09-level5-admission-terminal-spiker-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-09-level5-debug-route-automated-validation-report.md

Do not redo the concurrency implementation.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify accepted documentation.
Do not commit.
Do not push.
Do not launch the interactive game.


MANUAL OBSERVATION

Neal manually tested the current Level 5 candidate.

The ordinary no-death path appears correct:

- If the Blaster clears the entire wave without being destroyed, only Tankers
  and Spikers enter from the deep/bottom end of the tube.
- No individual Flippers were observed entering independently from the bottom
  during that no-death path.

The failure appears only after Blaster death:

- Tankers had previously released individual cargo Flippers into the tube.
- The Blaster was destroyed while one or more of those Flippers remained
  unresolved.
- After death and reentry, individual Flippers were then observed entering from
  the deep/bottom end.

Neal's definitive arcade observation is:

On original arcade Level 5, no individual Flippers ever enter independently
from the bottom. Level 5 Flippers originate from Tanker cargo. This remains
true after Blaster death/reentry.

Therefore the current candidate is NOT manually accepted.

Do not reinterpret "ordinary bottom entry" ambiguously. For this task:

- independent bottom entry means a Flipper appearing as an individual entity
  from the deep end through an Egg/reentry/pending-entry presentation;
- Tanker cargo release means Flippers visibly emerging from an existing Tanker
  in the tube.

The observed post-death individual Flippers fail the manual criterion even if
their hidden identities originally belonged to Tanker cargo.


PURPOSE

Determine exactly which identity and lifecycle path produces the post-death
individual bottom-entry Flippers.

The leading hypothesis is that unresolved Tanker cargo Flipper identities are
being reconstructed through the generic Egg/deep-entry path after Blaster
death.

Treat that only as a hypothesis.

Do not speculate or implement a correction during this task.

Establish the result directly from current source, ledger semantics, tests, and
bounded read-only analysis.


INVESTIGATION QUESTIONS

Answer all of the following where the source establishes an answer:

1. What exact identities are reconstructed after a Level 5 Blaster death?

2. How are unresolved Tanker cargo identities represented immediately before
   death?

3. What ledger state does each unresolved cargo identity enter during death
   reconstruction?

4. Does reconstruction place cargo identities into:
   - cargo-specific reentry reserve;
   - ordinary original reserve;
   - Egg state;
   - pending activation;
   - another state?

5. How does a reconstructed cargo identity reach runtime play after reentry?

6. Does it:
   - reappear directly as an active Flipper;
   - become an Egg and approach from the deep end;
   - bypass the Level 5 ordinary species selector while still using the same
     visual Egg/deep-entry path;
   - get mistaken for an original identity;
   - or follow another established path?

7. Is the post-death Flipper definitely cargo, or could an original Level 5
   identity be incorrectly committed as Flipper after reconstruction?

8. Does the new Level5AdmissionPolicy prohibit only fresh unselected originals
   from becoming Flippers, while already committed cargo Flippers use a
   separate physical-headroom reentry path?

9. Does that retained cargo reentry path intentionally preserve species and
   identity but accidentally produce a presentation that looks like prohibited
   ordinary bottom entry?

10. Does converted-Tanker cargo behave differently from ordinary-Tanker cargo
    after death?

11. Does cargo state differ depending on whether the Tanker:
    - released naturally;
    - was destroyed by a player shot;
    - had cargo pending activation;
    - was affected by Super Zapper;
    - or had one/both children already active?

12. Can the defect occur when no Tanker cargo Flipper is unresolved at death?

13. Can the defect occur before the first Tanker releases cargo?

14. Is this behavior newly introduced by the current Level 5 candidate, or did
    the accepted pre-candidate Level 5 reconstruction already present cargo
    Flippers this way?

15. Do existing tests assert identity conservation without asserting the
    arcade-visible reconstruction presentation?

16. Which exact test gap allowed all 28 focused, 345 affected, and 1,165 full
    tests to pass while the visible post-death defect remained?


TRACE THE FULL LIFECYCLE

Trace at least one representative cargo identity through this sequence:

1. Generic original identity becomes a Tanker.
2. Tanker owns two cargo Flipper identities.
3. Tanker releases cargo.
4. One cargo Flipper remains unresolved and active below the rim.
5. Blaster dies.
6. Runtime enemy collections are cleared/reconstructed.
7. Cargo identity enters its reconstructed ledger state.
8. Player reenters.
9. Cargo identity returns to runtime play.
10. Flipper becomes visible at the deep end.

For every step, identify:

- ledger identity kind;
- owner/original relationship;
- cargo ordinal where applicable;
- selected species;
- ledger state;
- runtime collection;
- method/routine responsible;
- whether random selection occurs;
- whether Level5AdmissionPolicy applies;
- whether physical-headroom-only reentry applies;
- whether the visible Egg/deep-entry presentation is shared with normal
  original admission.

Also trace the contrasting no-death path sufficiently to show why no
independent bottom-entry Flippers appear there.


ROM COMPARISON

Use the existing Level 5 ROM investigation before doing new ROM work.

That report established:

- Level 5 ordinary Flipper selection is excluded;
- Tanker cargo bypasses ordinary species selection;
- ROM death/reconstruction resets active enemies into generic pending supply;
- the project's stable identity/cargo reconstruction is an accepted project
  translation rather than literal ROM reconstruction.

Determine what those facts imply for this visible defect.

Be explicit about the distinction between:

- preserving a stable unresolved cargo identity internally; and
- presenting that identity after death in an arcade-inconsistent way.

Do not assume identity conservation requires an individual cargo Flipper to
reenter visibly from the bottom.

Do not propose abandoning the stable identity model unless the investigation
establishes that no narrower presentation/lifecycle correction is possible.


CURRENT CANDIDATE COMPARISON

Determine whether the candidate changed any of these paths:

- cargo reconstruction;
- cargo reserve/Egg state;
- retained cargo species;
- retained converted-Tanker marker;
- physical-headroom reentry;
- ready-Egg admission;
- visual placement or approach of retained cargo;
- death/reentry ordering.

Separate:

- behavior newly introduced by Level5AdmissionPolicy;
- behavior changed by the retained converted-Tanker work;
- pre-existing shared Level3WaveLedger behavior;
- pre-existing accepted Tanker cargo reconstruction;
- behavior inherited from Level 3/4 infrastructure.

Do not blame the new Level 5 policy without tracing the responsible path.


BOUNDED TEST INSPECTION

Inspect existing tests relevant to:

- Level 5 death/reentry;
- cargo identity conservation;
- cargo reserve/Egg reconstruction;
- retained species;
- physical-headroom reentry;
- Tanker release;
- pending cargo activation;
- converted Tanker reconstruction;
- visual/deep-entry placement where tested;
- ordinary Flipper exclusion.

Identify what is covered and what is missing.

Do not modify tests.

If useful, run only read-only test discovery/listing commands that do not build
or execute generated assemblies.

Do not run tests in this investigation.
Do not build.
Do not invoke VSTest.
Do not risk another Windows Application Control block.


REPRODUCTION MODEL

Without launching the game, define the smallest deterministic automated
scenario that a later correction task should use to reproduce the defect.

The proposed scenario should include:

- canonical Level 5;
- one Tanker with known original identity;
- known cargo pair identities;
- released cargo;
- at least one unresolved cargo Flipper at death;
- Blaster death/reconstruction;
- reentry;
- subsequent cargo return;
- assertion distinguishing prohibited deep-end individual entry from an
  acceptable arcade-faithful reconstruction.

Do not implement this scenario now.

Specify what state or visible/runtime property a future test must inspect.

Do not rely only on enemy type, because the identity may correctly remain a
Flipper while its post-death presentation is wrong.


RECOMMENDATION

Recommend the smallest safe next milestone based on established evidence.

Possible outcomes may include:

- preserve cargo identity but reconstruct it through a carrier;
- preserve cargo identity but use a non-Egg/non-bottom-entry return path;
- restore cargo directly to an appropriate live state;
- defer cargo until its owner/carrier representation is restored;
- correct mistaken original/cargo classification;
- another narrowly demonstrated solution.

Do not choose one without evidence from the trace.

The recommendation must state:

- exact cause;
- exact affected identities/states;
- whether the defect predates the current candidate;
- whether ordinary Level 5 admission is correct and should remain unchanged;
- smallest production change;
- focused tests required;
- accepted systems that must remain untouched;
- manual retest required.

Do not implement the recommendation.


PROTECT ACCEPTED/CANDIDATE SYSTEMS

Do not disturb:

- the current Level 5 Tanker/Spiker ordinary admission policy unless directly
  implicated;
- species minima/maxima;
- zero-minimum Flipper exclusion;
- terminal Spiker-to-Tanker conversion;
- converted-Tanker reentry;
- complete-pair cargo identity conservation;
- scoring;
- Super Zapper lifecycle;
- persistent Spikes;
- rim behavior;
- completion;
- recording/playback determinism;
- Levels 1-4;
- Levels 6-8.

The objective is to isolate the post-death presentation defect, not reopen the
entire Level 5 concurrency milestone.


VALIDATION

This is read-only.

At completion verify:

- no production file changed;
- no test file changed;
- no accepted documentation changed;
- no files are staged;
- candidate diff remains otherwise intact;
- HEAD remains at the accepted baseline;
- git diff --check still passes.

Do not launch the game.
Do not build.
Do not run tests.
Do not commit.
Do not push.


REPORT

Preserve this prompt and create a new ignored investigation report according
to the repository instructions.

Use a descriptive name such as:

AI-Logs/Reports/2026-11-09-level5-post-death-cargo-flipper-reentry-investigation-report.md

The report must include:

- repository/candidate starting state;
- Neal's no-death versus post-death observations;
- exact identity/lifecycle trace;
- code paths and methods inspected;
- ordinary original versus cargo handling;
- whether the defect predates the candidate;
- ROM-versus-project translation analysis;
- existing test coverage;
- test gap;
- smallest deterministic future reproduction;
- confirmed cause or remaining uncertainty;
- recommended smallest correction milestone;
- confirmation that no implementation or validation execution occurred.


STOP

Stop after the read-only investigation and report.

Do not implement a correction.
Do not launch the game.
Do not run tests.
Do not commit.
Do not push.
Do not begin Level 6 concurrency.

Neal and Copilot will review the trace before authorizing a fix.


TEMPEST_LEVEL5_POST_DEATH_CARGO_FLIPPER_INVESTIGATION_COMPLETE