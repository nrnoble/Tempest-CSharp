FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before using tools, reading files, running commands, modifying files, or taking
any repository action, locate the unique completion token near the end of this
user-provided task.

Verify that exactly one complete token beginning with "TEMPEST_" appears on a
line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment
metadata, automatically supplied workspace context, or other agent-generated
material may appear after the user's token and do not invalidate the task.

This check is intended to detect an incomplete or truncated USER task, not
appended agent context.

If exactly one complete TEMPEST_ token cannot be found, stop before any project
or repository action and explain why.


TASK

Perform a bounded repository-instructions maintenance milestone for the
Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted starting baseline:

1d5f37f6c309dee7fface6f99d09abd85d8a7f4c

Implement accepted Level 3 enemy concurrency

Before modifying anything, verify the actual:

- branch;
- HEAD;
- origin/master;
- tracking state;
- index;
- tracked worktree;
- relevant untracked state.

The expected tracked starting state is clean.

Known protected local reference material includes the existing untracked ROM
copies under:

Reference/arcade/roms.rev2/

Do not modify, stage, delete, relocate, normalize, or commit those files.


PURPOSE

Review, clean up, modernize, and improve the repository-wide VS Code Copilot
instructions in:

.github/copilot-instructions.md

The goal is to make this file the durable source for recurring Tempest project
rules so future user prompts can be shorter and focus primarily on their
specific investigation, implementation, validation, or closeout objective.

The instructions should give future VS Code Copilot agents strong, efficient,
current project guidance without unnecessary historical clutter, conflicting
authority rules, obsolete milestones, or avoidable repetition.

This is a PROJECT-INSTRUCTIONS-ONLY maintenance milestone.

Do not modify production code.
Do not modify tests.
Do not modify README.md.
Do not modify TEMPEST-DEVELOPMENT-NOTES.md.
Do not modify TEMPEST-BEHAVIOR-SPEC.md.
Do not modify gameplay behavior.
Do not begin Level 4 concurrency.
Do not launch the interactive game.

The only intended tracked file modification is:

.github/copilot-instructions.md


CURRENT FILE

Read the complete existing:

.github/copilot-instructions.md

before editing it.

Preserve useful existing rules unless the task below explicitly modernizes,
consolidates, or replaces them.

Do not simply append another large section to the existing file.

Refactor the instructions into a clearer, more current, less redundant
repository-wide rulebook.


1. UPDATE PROJECT PURPOSE AND EVIDENCE AUTHORITY

The current introduction gives Mark Incitti's Tempest 3.1 implementation more
authority than reflects the project's present development policy.

Rewrite the project-purpose/authority guidance to accurately describe the
current project.

This is an arcade-faithful modern C# recreation of Atari Tempest.

The goal is not cycle-accurate Atari hardware emulation or an identical emulator
clone. The goal is to reproduce the original arcade gameplay and presentation
to Neal's accepted fidelity standard while allowing reasonable modern C#
engineering translations.

Establish the durable evidence hierarchy clearly.

Use this practical authority order when sources disagree:

1. Directly observed original arcade behavior, especially behavior Neal has
   carefully verified and accepted.
2. Atari ROM-backed behavior and data, including the project-local ROM-derived
   disassembly and verified local ROM evidence.
3. Explicit accepted project decisions/translations recorded in the durable
   project documentation.
4. Mark Incitti's implementation as an important secondary engineering
   reference.
5. Existing C# behavior where already accepted or where stronger evidence does
   not establish otherwise.

Do not preserve Incitti behavior merely because it was the original engineering
reference if stronger ROM or arcade evidence disagrees with it.

Do not silently override a deliberate accepted project translation merely
because literal ROM behavior differs. Identify the conflict and preserve the
accepted translation unless the task authorizes reconsideration.

Make clear that Incitti remains particularly useful for engineering comparison,
timing, controller behavior, and areas where stronger Atari evidence is not
available, but is not Atari authority.


2. ADD LOCAL ROM / DISASSEMBLY GUIDANCE

Record that the project already contains the public ROM-derived Tempest
disassembly/reference material locally.

Future agents should use the project-local Atari reference material when ROM
analysis is required.

Do not search the Internet for the Tempest disassembly when the required
reference already exists locally.

Internet research should not substitute for available project-local primary
evidence.

The raw ROM copies under Reference/arcade/roms.rev2/ are protected local
reference material.

Do not modify them.

Do not inspect or enumerate binary contents unnecessarily merely because they
appear as untracked files.

Git-state verification may identify their presence without opening every ROM.

Read/hash binary ROM material only when a task genuinely requires bounded ROM
verification.


3. REMOVE OBSOLETE INITIAL-MILESTONE MATERIAL

Remove the obsolete "Initial milestone" section describing early tasks such as:

- draw one tube;
- move the player left/right;
- add relative mouse/spinner input;
- fire player shots;
- add one Flipper;
- add collision and scoring.

Those are historical completed tasks, not repository-wide instructions.

Historical development state belongs in the durable project documentation, not
in the always-applied Copilot instruction context.


4. MODERNIZE SIMULATION / DIAGNOSTIC GUIDANCE

Replace obsolete early-development wording such as:

- use a fixed 60 Hz simulation "initially";
- add debug/single-step facilities "early";
- preserve reference tick values "until behavior has been validated";

with current durable invariants.

The project already has an accepted deterministic fixed-step gameplay
foundation.

Future work should:

- preserve the accepted deterministic fixed-step simulation unless a task
  explicitly authorizes redesign;
- preserve deterministic recording/playback and diagnostic facilities;
- treat timing changes as gameplay changes requiring appropriate automated
  validation and Neal's manual acceptance;
- avoid casually converting accepted tick-based mechanics into elapsed-time
  mechanics;
- preserve deterministic behavior relied upon by recordings/tests unless the
  task explicitly addresses that behavior.


5. STANDARDIZE EVIDENCE CLASSIFICATION

Define a concise canonical evidence vocabulary for project work.

Use appropriate classifications such as:

ARCADE-OBSERVED
ROM-BACKED
REFERENCE-DERIVED
PROJECT DECISION
PROJECT TRANSLATION
DEVELOPMENT-ONLY
APPROXIMATION
INFERENCE
UNKNOWN

The exact formatting can be made readable and consistent, but preserve the
semantic distinctions.

Require future agents to distinguish these classes where evidence provenance
matters in:

- implementation comments;
- investigation reports;
- TEMPEST-DEVELOPMENT-NOTES.md;
- TEMPEST-BEHAVIOR-SPEC.md;
- closeout documentation.

Do not promote inference or reference-derived behavior to ROM-backed.

Do not promote a project translation to ROM-backed merely because manual
gameplay acceptance succeeds.

Do not label behavior ARCADE-OBSERVED unless it was actually established by
direct arcade observation/manual comparison at that level of specificity.


6. STRENGTHEN MANUAL ACCEPTANCE AUTHORITY

Record the project-wide principle that automated validation supplements rather
than replaces Neal's manual gameplay acceptance.

For gameplay feel and presentation, Neal's manual acceptance is the final
project authority, especially for:

- arcade gameplay fidelity;
- movement feel;
- visual fidelity;
- controller/spinner response;
- perceived timing;
- sound balance/presentation;
- enemy behavior as experienced during play.

Tests may establish mechanical invariants but must not be described as manual
arcade-fidelity acceptance.

VS Code Copilot must not launch the interactive game for acceptance testing.

After automated validation, provide the exact task-appropriate launch command
and stop for Neal's manual test.

Do not always provide the generic Level 1 command when a more appropriate
specific level, diagnostic, or playback command exists.


7. ADD ONE-MILESTONE / SCOPE-CONTROL GUIDANCE

Add a durable scope rule.

Work on one bounded milestone at a time.

Do not mix unrelated defects, refactors, polish, fidelity changes, or future
features into the active milestone.

If unrelated issues are discovered:

- record/report them when important;
- do not silently fix them;
- leave them for separately authorized work.

Do not reopen manually accepted behavior merely because an alternative design
appears cleaner.

Reopen accepted behavior only when explicitly authorized or when necessary to
address a demonstrated regression/contradiction relevant to the current task.

Preserve the existing useful guidance against broad refactoring during bug
fixes and against replacing working behavior merely for design cleanliness.


8. PROTECT USER / LOCAL REFERENCE MATERIAL

Add a general protected-local-material rule.

Examples include:

- Reference/arcade/roms.rev2/;
- gameplay recordings;
- screenshots;
- videos;
- TEMP/ or Temp/ working material;
- UserData;
- diagnostic output;
- other user-created or untracked local reference assets.

Unless the current task specifically requires the material:

- do not modify it;
- do not delete it;
- do not relocate it;
- do not normalize it;
- do not stage or commit it;
- do not overwrite it;
- do not unnecessarily inspect binary contents.

Do not use git clean, destructive reset, or equivalent cleanup actions against
user/local material.


9. CLARIFY AI PROMPT / REPORT HANDLING

Preserve and improve the existing AI-Logs rules.

For substantial development, investigation, debugging, implementation, or
closeout tasks:

- preserve the substantive user prompt under AI-Logs/Prompts/;
- preserve the corresponding report under AI-Logs/Reports/;
- use descriptive date-prefixed paired filenames;
- retain scope, constraints, validation/manual-acceptance requirements, and the
  completion token;
- omit automatically appended VS Code/agent context from the saved user
  prompt.

AI-Logs are ignored historical/working artifacts.

Never stage or commit AI-Logs unless Neal explicitly changes that project
policy.

Existing reports are historical records.

Do not rewrite an earlier report merely because the milestone later advanced
from investigation to implementation or from pending to manually accepted.

Create a new implementation, acceptance, or closeout report as appropriate.

Keep logging proportional. Trivial interactions do not require dedicated
prompt/report files.


10. CLARIFY DURABLE DOCUMENTATION LIFECYCLE

TEMPEST-DEVELOPMENT-NOTES.md and TEMPEST-BEHAVIOR-SPEC.md remain the durable
authorities for accepted development decisions and behavior/evidence
classification.

Clarify when they should be updated.

Investigation-only work must not automatically promote findings into accepted
behavior.

An implementation candidate that still requires Neal's gameplay acceptance
must not be documented as accepted.

After Neal manually accepts a behavior, the subsequent closeout milestone
should update the appropriate durable documentation with:

- accepted behavior;
- evidence classification;
- project decisions/translations;
- important fidelity boundaries;
- manual acceptance status.

Historical AI reports remain historical evidence and are not substitutes for
the durable accepted documentation.


11. ADD EXPLICIT GIT / WORKTREE SAFETY

Before modifying the repository for a substantial task, inspect the relevant
starting repository state, including:

- branch;
- HEAD;
- tracking/upstream state;
- index;
- tracked worktree;
- relevant untracked state.

If a prompt supplies an expected baseline, compare the actual state to it.

If there is a material unexpected discrepancy that makes the requested task
unsafe or ambiguous, stop/report rather than silently changing or discarding
state.

Do not:

- reset away pre-existing user work;
- clean/delete unrelated files;
- silently stash user work;
- overwrite unrelated changes;
- absorb unrelated modifications into the task.

Stage only files authorized by the current milestone.

Review the staged file list and staged diff before commit.


12. REQUIRE EXPLICIT COMMIT / PUSH AUTHORIZATION

Add a durable Git authorization boundary.

Do not commit or push merely because implementation or validation succeeds.

Commit and/or push only when the current user task explicitly authorizes it.

Investigation and implementation-candidate tasks normally stop before commit
unless explicitly authorized otherwise.

Manual-acceptance closeout tasks may commit/push when the user explicitly
requests that closeout and required validation succeeds.

Preserve the existing rule that blocked/incomplete required validation prevents
normal commit/push unless the user explicitly authorized a partial checkpoint.


13. INCORPORATE THE COMPLETION-TOKEN POLICY

Add the durable completion-token policy to the repository instructions.

For substantial Tempest tasks, before project/repository action, locate the
completion token in the substantive user-provided task.

The token:

- begins with TEMPEST_;
- appears complete on a line by itself;
- should appear exactly once in the substantive user task.

Automatically appended VS Code reminders, attachments, tagged-file metadata,
workspace context, or other agent-generated material may follow the user's
token and do not invalidate it.

The purpose is to detect incomplete/truncated USER tasks, not appended agent
context.

If exactly one complete TEMPEST_ token cannot be established for a substantial
task that uses the convention, stop before project/repository action and
explain why.

Do not require the token convention for trivial conversational questions,
simple command lookups, or other minor interactions that do not warrant a
substantial task prompt.

IMPORTANT:

Although this rule becomes repository-wide, Neal/Copilot may continue including
a SHORT fail-closed completion-token header in substantial generated prompts as
defense against prompt truncation before the task can be understood.

Do not create needless duplication between that short prompt header and the
repository instructions.


14. PRESERVE AND CONSOLIDATE COMMAND / SECURITY / RECOVERY SAFETY

The existing command, validation, and recovery safety section contains
important accepted rules.

Preserve its substantive protections, including:

- bounded potentially long operations;
- task-appropriate timeout/tick/iteration/process/hang limits;
- voluntary stopping on no meaningful progress;
- progressively smaller bounded isolation;
- Windows Application Control / Smart App Control / Code Integrity handling;
- no weakening or bypassing security;
- no stale-binary substitution;
- retry only through a materially different safe route;
- canceled/timed-out/hung/stalled/blocked commands are never PASS;
- explicit PASS / FAIL / BLOCKED / TIMEOUT / CANCELED / PARTIAL reporting;
- voluntary self-stop when required work cannot continue;
- precise handoff;
- no normal commit/push after incomplete required validation.

Consolidate redundant wording where doing so improves clarity.

Do not weaken these rules merely to shorten the file.


15. PRESERVE GOOD CODING / ARCHITECTURE GUIDANCE

Retain useful current principles where still valid, including:

- readable/testable modern C#;
- simulation independent of rendering;
- data-driven level/tube information;
- vector-based Tempest presentation;
- small classes and explicit state;
- maintainability-focused comments;
- comments explain WHY rather than obvious syntax;
- preserve useful comments when behavior remains valid;
- avoid speculative gameplay changes;
- identify source disagreements rather than hiding them.

Modernize wording where necessary to align with the evidence hierarchy
established above.


STRUCTURE / EFFICIENCY

Reorganize the file into a compact, coherent structure.

A useful organization is approximately:

1. Project purpose and evidence authority
2. Architecture and coding
3. Evidence classification
4. Scope and preservation
5. Repository and local-file safety
6. Testing and manual acceptance
7. Prompts, reports, and durable documentation
8. Command, security, and recovery safety
9. Git and completion requirements

This section list is guidance, not an exact required format.

Prefer:

- short declarative rules;
- grouped related requirements;
- one clear statement instead of several equivalent statements;
- durable project-wide guidance.

Avoid:

- obsolete project history;
- completed milestone descriptions;
- repeating the same rule in multiple sections;
- excessive narrative explanation;
- task-specific details that belong in individual prompts.

The final file should be efficient enough to include automatically with future
Copilot work without unnecessarily consuming working context.


SELF-REVIEW

After editing the instruction file, review it specifically for:

- contradictions;
- duplicated requirements;
- obsolete guidance;
- ambiguous authority ordering;
- accidental weakening of security/stall rules;
- excessive verbosity;
- instructions that belong in task prompts rather than repository instructions;
- missing recurring project rules;
- conflicts with the currently accepted Tempest workflow.

If two instructions genuinely conflict, resolve the conflict only when the
current project policy above clearly establishes the correct rule. Otherwise
report the conflict rather than inventing policy.


VALIDATION

This is an instructions/documentation-only maintenance milestone.

Do not run gameplay tests.
Do not run the full solution tests.
Do not run the game.
Do not build the solution merely for ceremony.

Validate the actual changed artifact and repository state instead.

At minimum:

- inspect the complete resulting .github/copilot-instructions.md;
- inspect the diff;
- run git diff --check;
- verify no unintended tracked file changed;
- verify protected local/untracked material remains unmodified/uncommitted.

If there is an appropriate lightweight Markdown/text check already available
and genuinely useful, it may be used, but do not introduce tooling solely for
this maintenance milestone.


AI LOGGING

This is a substantial project-instructions maintenance milestone.

Preserve this substantive prompt under:

AI-Logs/Prompts/

Create a matching report under:

AI-Logs/Reports/

Use descriptive date-prefixed names, for example:

2026-11-09-project-instructions-modernization-prompt.md

2026-11-09-project-instructions-modernization-report.md

The report should record:

- starting baseline/Git state;
- the prior instruction problems identified;
- major changes made;
- obsolete material removed;
- authority/evidence changes;
- workflow/Git/local-material protections added;
- completion-token policy;
- security/stall policy preservation;
- self-review findings;
- validation performed;
- final diff/file state;
- any unresolved concern.

AI-Logs are ignored and must not be staged or committed.


COMMIT AND PUSH

This maintenance milestone IS authorized for commit and push if validation
succeeds.

If and only if:

- .github/copilot-instructions.md is the only intended tracked modification;
- the resulting instructions accurately implement this task;
- self-review finds no blocking conflict;
- git diff --check passes;
- protected user/reference material remains untouched;

stage exactly:

.github/copilot-instructions.md

Review the staged diff and staged file list.

Commit with an appropriate concise message, for example:

Modernize Tempest Copilot project instructions

Push master to origin.

Then verify:

- final pushed HEAD;
- origin/master;
- tracking state;
- tracked index/worktree clean;
- AI-Logs remain ignored/uncommitted;
- protected local reference material remains uncommitted.


STOP CONDITION

Stop after the project-instructions maintenance milestone is validated,
committed, pushed, and verified.

Do NOT begin Level 4 concurrency.

Do NOT make production/test/gameplay changes.

Provide Neal a concise final summary including:

- important instruction improvements;
- removed obsolete material;
- validation;
- commit hash;
- push status;
- final repository state;
- anything future Tempest prompts no longer need to repeat because it is now
  repository-wide guidance.


TEMPEST_PROJECT_INSTRUCTIONS_MODERNIZATION_COMPLETE