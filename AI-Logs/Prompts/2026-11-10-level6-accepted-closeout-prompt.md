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

Close out the complete accepted Level 6 milestone.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline beneath the current uncommitted Level 6 candidate:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

Implement accepted Level 5 enemy concurrency

The complete Level 6 candidate is already present in the worktree.

Do not redo the implementation.

This closeout includes:

1. Level-6-specific ROM-informed ordinary admission/concurrency.
2. Terminal Spiker-to-Tanker behavior.
3. Level-6-specific generic-obligation death reconstruction.
4. The pending-lane depleted-Spike crash correction and corrected $CC branch.
5. Level 6 Tanker-cargo Flipper $0b/$07 movement behavior.
6. Final durable documentation.
7. Final automated validation.
8. Commit and push.

Do not begin Level 7 concurrency.


HISTORICAL HANDOFFS

Read the existing Level 6 reports as needed, especially:

AI-Logs/Reports/2026-11-10-level6-concurrency-rom-investigation-report.md

AI-Logs/Reports/2026-11-10-level6-combined-concurrency-diagnostic-implementation-report.md

AI-Logs/Reports/2026-11-10-level6-pending-lane-spike-crash-correction-report.md

AI-Logs/Reports/2026-11-10-level6-flipper-0b-provenance-check-report.md

AI-Logs/Reports/2026-11-10-level6-flipper-0b-bounded-comparison-report.md

AI-Logs/Reports/2026-11-10-level6-flipper-0b-07-implementation-report.md

These reports are historical records.

Do not rewrite them to remove former pending states, the crash, the initial
movement mismatch, or other historical findings.


STARTING STATE

Before changing durable documentation:

- verify branch;
- verify HEAD;
- fetch origin;
- verify origin/master;
- verify upstream/tracking;
- verify ahead/behind;
- verify index;
- inspect the complete tracked and nonignored untracked candidate;
- compare the candidate with the historical implementation reports;
- run git diff --check.

The existing candidate must still correspond to the version Neal manually
accepted.

If unexpected unrelated user work or a material candidate discrepancy exists,
stop and report it rather than altering or absorbing it.


FINAL MANUAL ACCEPTANCE

Neal has completed manual Level 6 acceptance.

Treat the Level 6 candidate as manually accepted.

Manual findings include:

- The repeatable first-ten-second pending-lane Spike crash is resolved.
- Level 6 manual gameplay checks pass.
- Ordinary Level 6 admission behaves as expected.
- No blocking Level 6 concurrency/reconstruction problem remains.
- No Level 1-5 regression was reported.

Neal also manually accepted the Level 6 Tanker-cargo Flipper movement
correction.

For the subtle movement verification, Neal recorded gameplay and used
pause/frame-by-frame stepping to compare the candidate carefully with the
arcade.

Neal manually verified the Level 6 deep-release pattern:

- straight along-tube movement interval;
- one lane flip;
- no along-tube climbing during the visible flip;
- another straight interval begins after the flip;
- travel direction persists;
- the two Tanker cargo children travel in opposite directions.

Neal also accepted the distinct near-rim/natural cargo behavior.

Treat this as strong ARCADE-OBSERVED manual acceptance at the observable level
Neal actually verified.

Do not convert project coordinate/timing approximations into literal ROM facts.


ACCEPTED LEVEL 6 ROM-BACKED CONCURRENCY

Document accurately:

- default Easy/Medium generic pending supply: 22;
- Hard ROM supply: 25, without claiming the C# project implements a separate
  Hard supply mode;
- Flipper ordinary minimum/maximum: 0/5;
- Tanker ordinary minimum/maximum: 1/2;
- Spiker ordinary minimum/maximum: 2/4;
- Pulsar and Fuseball ordinary availability: zero;
- ordinary individual Flipper entry is excluded despite positive Flipper
  availability because its minimum is zero;
- unmet Spiker minimum precedes Tanker minimum;
- seven shared physical enemy places;
- latent Tanker cargo consumes no physical enemy place but affects Flipper
  availability;
- released materialized cargo consumes physical capacity and bypasses ordinary
  species selection;
- ordinary availability may stop while a physical place remains unused;
- species maxima are ordinary-admission limits, not absolute late-wave
  population ceilings.


PENDING-SEGMENT SPIKE SELECTOR

Document the corrected Level 6 Tanker-versus-Spiker post-minimum selector.

ROM-BACKED:

- after minima are satisfied and both Tanker and Spiker are available, the
  selector uses the CURRENT PENDING ENTRY'S segment Spike value;
- it is not a global shortest-Spike scan;
- a zero/depleted segment Spike is represented by $FF for this comparison;
- comparison value >= $CC selects Spiker;
- comparison value below $CC selects Tanker.

PROJECT TRANSLATION / APPROXIMATION:

- C# maps the pending Egg lane directly to its corresponding Level 6 segment;
- live C# Spike height uses the project's established ROM-along position
  translation;
- a missing C# Spike object for a depleted segment maps to the ROM-backed $FF
  comparison sentinel;
- no claim is made that C# world height is literally identical to ROM
  coordinate representation.

Record that the original diagnostic candidate crashed when a valid pending
lane's persistent Spike had been destroyed because the lookup assumed the
Spike object always existed.

That defect was corrected.

Do not preserve the former incorrect comparison direction as accepted history
except where useful to explain the correction.


TERMINAL SPIKER LIFECYCLE

Document:

- while generic pending supply remains, terminal Tanker conversion does not
  occur merely because ordinary admission is temporarily blocked;
- conversion requires actual mapped generic pending supply zero;
- at zero pending supply, a retreating Spiker converts in place into a
  Flipper-carrying Tanker;
- conversion bypasses ordinary Tanker availability and can therefore produce
  live Tankers beyond ordinary maximum 2;
- the accepted C# conversion retains project identity/accounting semantics.

Preserve the distinction between ROM behavior and C# project translation.


LEVEL 6 DEATH RECONSTRUCTION

Document the accepted generic-obligation reconstruction direction.

ROM-BACKED:

active physical enemies + remaining generic pending supply become generic
post-death pending obligations; active species identity is not retained.

Important consequences:

- one live Tanker with latent cargo is one active obligation;
- its two latent cargo children are not two additional active obligations;
- each already-materialized unresolved cargo Flipper is one active obligation;
- active pre-death species commitments are discarded;
- reconstructed obligations subsequently use normal Level 6 Tanker/Spiker
  selection.

Accepted C# PROJECT TRANSLATION:

- stable project accounting represents this generic reconstruction without
  preserving pre-death active species;
- unresolved quantity is conserved;
- terminal outcomes remain terminal;
- latent cargo does not inflate reconstructed quantity;
- materialized cargo contributes individually;
- repeated death conserves the then-current unresolved quantity.

Do not claim literal ROM stable-identity reproduction.

Record Neal's Level 6 manual acceptance separately from the earlier Level 5
arcade observations that motivated this engineering approach.


LEVEL 6 TANKER-CARGO MOVEMENT

This is now important accepted Level 6 behavior.

ROM-BACKED:

Displayed Level 6 uses Flipper movement program $0b.

For sufficiently deep/player-shot Tanker cargo:

- ROM preserved enemy_along >= $20 selects Level 6 $0b;
- eight along-tube movement advances occur;
- then one lane flip occurs;
- along-tube depth remains fixed during that flip;
- after completion, a new eight-advance interval begins immediately;
- $0b contains no scripted direction reversal;
- each cargo child retains its lane-travel direction;
- the two cargo children begin with opposite direction bits.

For near-rim/natural cargo:

- ROM enemy_along < $20 selects default $07;
- $07 climbs straight below the rim;
- no repeating below-rim lane flip is performed by $07;
- normal top/rim handling takes over afterward.

Accepted C# PROJECT TRANSLATION / APPROXIMATION:

- use the existing Tanker.RomAlongPositionForHeight mapping and ROM $20
  boundary;
- do not claim literal coordinate identity between C# height and ROM
  enemy_along;
- deep/player-shot Level 6 cargo uses the $0b cadence;
- natural/near-rim Level 6 cargo uses $07 straight climbing;
- $0b cargo does not advance height during its flip;
- no old generic 23-tick below-rim cargo pause is inserted into the $0b cycle;
- pair direction remains opposite and individually persistent;
- both modes use existing accepted rim behavior when rim handling takes over.

Record Neal's frame-by-frame manual acceptance of this behavior.


FUTURE LEVEL MOVEMENT LESSON

Add a concise durable development rule if the existing documentation has an
appropriate planning/strategy location.

The rule should state in substance:

When implementing or refining a future Tempest level, investigate that level's
ROM-defined Flipper movement program explicitly rather than assuming movement
from another level or accepting a generic "moves normally" check.

When Tanker cargo has ROM-defined depth/release-specific movement behavior,
investigate those paths separately.

Manual acceptance should specify the distinguishing movement cadence or
pattern to observe.

Avoid vague acceptance language such as "Flippers move normally" when the ROM
provides a level-specific movement program.

Keep this concise.

Do not turn the Level 6 closeout into a broad rewrite of future-development
documentation.


COMPLETE-PAIR CARGO TRANSLATION

Preserve the accepted project rule:

- both Tanker cargo identities are retained and materialized;
- strict ROM child-loss behavior at physical saturation is not reproduced;
- complete-pair release may temporarily exceed the ROM's seven physical
  places;
- subsequent ordinary admission waits for physical headroom.

Classify this as PROJECT TRANSLATION rather than ROM-BACKED strict-slot
fidelity.


SUPER ZAPPER BOUNDARY

The ROM pauses ordinary enemy creation while zap_running is active.

Current C# Level 6 does not implement a literal equivalent admission pause.

This remains deliberately deferred.

Do not broaden closeout into Super Zapper timing/admission work.

Manual acceptance does not establish hidden zap_running timing equivalence.


DURABLE DOCUMENTATION

Update:

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Record concisely:

- Level 6 ROM concurrency investigation;
- accepted Level 6 admission;
- species minima/maxima;
- seven-place physical headroom;
- pending-segment Spike selector and depleted-Spike $FF handling;
- resolved startup crash;
- terminal Spiker conversion;
- generic-obligation death reconstruction;
- complete-pair cargo translation;
- Level 6 $0b/$07 Tanker-cargo movement;
- Neal's manual Level 6 acceptance;
- Neal's frame-by-frame movement acceptance;
- automated validation;
- no observed Level 1-5 regression;
- relevant PROJECT TRANSLATIONS / APPROXIMATIONS;
- explicitly deferred boundaries.

Do not rewrite unrelated history.


SCOPE

This is complete Level 6 closeout only.

Do not:

- begin Level 7 concurrency;
- alter Levels 1-5;
- alter Levels 7-8;
- investigate additional movement programs;
- refactor shared movement architecture unnecessarily;
- implement deferred Super Zapper entry pause;
- retune accepted movement;
- modify Windows security;
- perform unrelated cleanup.


FINAL CLOSEOUT VALIDATION

After durable documentation changes:

1. Run git diff --check.

2. Run focused Release tests covering:
   - Level 6 admission;
   - pending-lane Spike mapping;
   - terminal Spiker conversion;
   - generic-obligation reconstruction;
   - Level 6 Tanker cargo $0b/$07 movement.

3. Run affected Release regressions covering:
   - Tanker cargo;
   - Flipper movement;
   - Spiker/Spike behavior;
   - death/reentry;
   - Super Zapper;
   - progression;
   - rim/completion;
   - recording/playback;
   - Levels 1-5;
   - Levels 7-8 isolation.

4. If affected validation passes:

dotnet test TempestCSharp.slnx --configuration Release --blame-hang-timeout 5min

5. If full tests pass:

dotnet build TempestCSharp.slnx --configuration Release

6. Run:
   - final git diff --check;
   - complete candidate diff review;
   - intended-file list review.

Do not launch the game. Manual acceptance is complete.

Follow repository-wide Windows security/stall self-stop rules.


AI RECORDS

Preserve all previous Level 6 reports as historical records.

Do not rewrite:

- the investigation report;
- original diagnostic implementation report;
- crash correction report;
- $0b provenance report;
- bounded comparison report;
- $0b/$07 implementation report.

Create a new paired closeout record:

AI-Logs/Prompts/2026-11-10-level6-accepted-closeout-prompt.md

AI-Logs/Reports/2026-11-10-level6-accepted-closeout-report.md

The report should record:

- starting baseline/Git state;
- accepted Level 6 concurrency implementation;
- crash correction;
- generic death reconstruction;
- ROM-backed $0b/$07 movement distinction;
- project translations;
- Neal's manual acceptance;
- frame-by-frame movement validation;
- final automated validation;
- durable documentation updates;
- exact committed files;
- commit hash;
- push status;
- final local/remote repository state;
- deferred fidelity boundaries.

AI-Logs remain ignored and must not be committed.


COMMIT AND PUSH

If and only if:

- current candidate matches Neal's manually accepted version;
- documentation accurately records the evidence and acceptance;
- final closeout validation passes;
- no blocking discrepancy exists;

stage exactly the intended Level 6 production/test changes and durable
documentation.

Review:

- staged file list;
- staged diff;
- staged git diff --check.

Do not stage AI-Logs or protected local material.

Commit with a concise message such as:

Implement accepted Level 6 concurrency and cargo movement

Push master to origin.

After push, verify:

- local HEAD;
- local origin/master;
- direct remote refs/heads/master;
- ahead/behind 0/0;
- index clean;
- tracked worktree clean;
- AI-Logs ignored/uncommitted;
- local ROM/reference material ignored/uncommitted;
- no unrelated files committed.


STOP

Stop after Level 6 is:

- documented;
- final validated;
- committed;
- pushed;
- remote verified.

Do not begin Level 7 concurrency.

Provide Neal:

- final commit hash;
- committed file list;
- focused/affected/full test results;
- build result;
- push verification;
- final repository state.


TEMPEST_LEVEL6_ACCEPTED_CLOSEOUT_COMPLETE