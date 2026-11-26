# Tempest Project Instructions

## 1. Purpose and evidence authority

This project is an arcade-faithful modern C# recreation of Atari Tempest. The
goal is to reproduce original arcade gameplay and presentation to Neal's
accepted fidelity standard, not to emulate Atari hardware cycle-for-cycle or
clone another implementation exactly.

When evidence or behavior conflicts, use this order:

1. Directly observed original arcade behavior, especially behavior Neal has
   carefully verified and accepted.
2. Atari ROM-backed behavior and data, using the local arcade disassembly and
   bounded verification of local ROMs when genuinely needed.
3. Explicit accepted project decisions and translations in the durable project
   documentation.
4. Mark Incitti's implementation as an important secondary engineering
   reference.
5. Existing C# behavior, where already accepted or where stronger evidence
   does not establish otherwise.

Incitti is useful for engineering comparison, timing, controller behavior, and
areas without stronger Atari evidence; it is not Atari authority. Do not retain
Incitti behavior when stronger ROM or arcade evidence disagrees. Do not silently
discard an accepted project translation because literal ROM behavior differs:
identify the conflict and preserve the accepted translation unless
reconsideration is authorized.
When arcade, ROM, Incitti, and C# behavior disagree, state the disagreement and
its evidence rather than obscuring it.

The public ROM-derived arcade disassembly is already available at
`Reference/arcade/Tempest Rom Source code.md`. Use project-local Atari reference
material first; do not search the Internet for a disassembly that is already
available locally or substitute online research for local primary evidence.
Raw ROM copies in `Reference/arcade/roms.rev2/` are protected local reference
material. Do not modify them, and do not open, hash, or enumerate their
contents unless bounded ROM verification is genuinely required.

## 2. Architecture and coding

- Prefer readable, testable modern C# over line-for-line transliteration.
- Keep simulation independent of rendering; keep level and tube data
  data-driven; favor small classes and explicit state.
- Preserve the accepted deterministic fixed-step simulation, deterministic
  recording/playback, and diagnostic facilities unless a task explicitly
  authorizes redesign.
- Tempest presentation is vector-based. Prefer established geometry,
  projection, line rendering, palette, and animation systems over sprites,
  textures, bitmaps, or generic particles.
- Avoid speculative gameplay changes. Investigate unclear behavior against
  appropriate evidence; mark unresolved behavior as unknown rather than
  guessing.
- Add concise maintainability comments for non-obvious rules and design choices.
  Explain why, especially for gameplay timing/order, state lifecycles, unusual
  geometry, deterministic behavior, and fidelity decisions. Do not narrate
  obvious code; preserve useful comments while their behavior remains valid.
  Distinguish evidence provenance when it matters.
- Avoid broad refactoring during bug fixes. Do not replace working behavior
  merely because another design appears cleaner; demonstrate a benefit and
  obtain authorization for behavior-affecting design changes.

## 3. Evidence classification

Use these canonical classifications when provenance matters:

- **ARCADE-OBSERVED** — directly established by arcade observation or manual
  comparison at the stated level of specificity.
- **ROM-BACKED** — supported by the Atari arcade ROM or its verified data.
- **REFERENCE-DERIVED** — established by Incitti or another explicitly named
  secondary reference, not thereby established as arcade behavior.
- **PROJECT DECISION** — an accepted intentional recreation rule.
- **PROJECT TRANSLATION** — an accepted engineering mapping from arcade/ROM
  behavior into the modern project.
- **DEVELOPMENT-ONLY** — instrumentation, synthetic scenarios, or test seams
  that are not claims about normal arcade behavior.
- **APPROXIMATION** — a known substitute that has not been established as
  equivalent to arcade behavior.
- **INFERENCE** — a reasoned but not directly established interpretation.
- **UNKNOWN** — available evidence does not establish the behavior.

Use the existing equivalent labels when maintaining a durable document whose
established terminology differs (for example, `ARCADE VERIFIED`,
`REFERENCE VERIFIED`, or `DEVELOPMENT ONLY`); do not change evidence merely by
changing a label. Never promote inference or reference-derived behavior to
ROM-backed. Manual acceptance of a deliberate translation does not make it
ROM-backed. Do not claim arcade observation unless that specific behavior was
actually observed or compared.

Where relevant, distinguish these classes in implementation comments,
investigation reports, durable documentation, and closeouts.
`TEMPEST-DEVELOPMENT-NOTES.md` and `TEMPEST-BEHAVIOR-SPEC.md` are the durable
records for accepted decisions and behavior/evidence classifications.

## 4. Scope and preservation

- Work on one bounded milestone at a time. Do not mix unrelated defects,
  refactors, polish, fidelity changes, or future features into it.
- Report important unrelated findings without silently fixing them; leave them
  for separately authorized work.
- Do not reopen manually accepted behavior merely to pursue a cleaner design.
  Reopen it only when explicitly authorized or to address a demonstrated,
  relevant regression or contradiction.
- Treat user-created and local reference material as protected, including
  gameplay recordings, screenshots, videos, `TEMP/` or `Temp/` working
  material, `UserData/`, diagnostic output, and untracked assets. Unless the
  task specifically requires otherwise, do not modify, delete, relocate,
  normalize, overwrite, stage, commit, or unnecessarily inspect it.
- `TEMP/myNotes.md` is Neal's private convenience scratchpad. Unless Neal
  explicitly requests access to this file in the current task, VS Code Copilot
  must not read, search, quote, edit, or use it as project context, evidence,
  requirements, a basis for inferring current or historical behavior, or an
  authority for project commands. When access is explicitly requested for a
  specific task, limit it to that expressly authorized purpose.
- Never use `git clean`, destructive reset, or equivalent cleanup against user
  or local material.

## 5. Repository and Git safety

Before substantial repository work, inspect the relevant starting state:
branch, HEAD, upstream/tracking, index, tracked worktree, and relevant
untracked state. Compare against any expected baseline in the task. If a
material discrepancy makes the task unsafe or ambiguous, stop and report it.

Never reset away pre-existing work, clean unrelated files, silently stash user
work, or absorb unrelated changes. Stage only files authorized by the current
milestone, then review the staged file list and staged diff before committing.
Commit or push only when the current user task explicitly authorizes it.
Investigation and implementation candidates normally stop before commit.
Failed, blocked, timed-out, canceled, stalled, or otherwise incomplete required
validation blocks normal commit/push unless an explicitly authorized partial
checkpoint applies.

## 6. Validation and manual acceptance

- Run the smallest relevant automated validation for the task; do not build or
  run unrelated suites merely for ceremony.
- Preserve deterministic behavior relied on by recordings and tests unless the
  task explicitly addresses it. Do not casually convert tick-based mechanics
  to elapsed-time mechanics.
- Prefer deterministic recorded or synthetic input for automated gameplay
  regression when useful; it supplements, never replaces, Neal's manual
  acceptance.
- Timing changes are gameplay changes and require appropriate automated
  validation plus Neal's manual acceptance.
- Automated tests can establish mechanical invariants; they do not establish
  arcade-fidelity acceptance. Neal's manual gameplay acceptance is the final
  project authority for gameplay fidelity, movement feel, visual fidelity,
  controller/spinner response, perceived timing, sound presentation, and
  enemy behavior as experienced in play.
- VS Code Copilot must not launch the interactive game for acceptance. After
  automated validation, give the exact task-appropriate launch, diagnostic, or
  playback command and stop for Neal. Do not default to the generic Level 1
  launch command when a more appropriate command exists.

## 7. Prompts, reports, and durable documentation

For substantial development, investigation, debugging, implementation, or
closeout work, save the substantive user prompt in `AI-Logs/Prompts/` and a
paired report in `AI-Logs/Reports/`. Use descriptive date-prefixed names.
Include the scope, constraints, validation and manual-acceptance requirements,
and completion token in the prompt; omit automatically appended VS Code or
agent context. Reports should record work, changed files, evidence/design
findings, validation and results, unresolved issues, and remaining manual
acceptance. Keep logging proportional; routine questions and minor checks do
not need dedicated records.

The required AI-Logs prompt and report files are an explicit exception to
read-only investigation constraints. Unless Neal explicitly prohibits logging
for the current task, instructions such as "read-only," "do not modify files,"
"leave the repository unchanged," or "make no repository changes" apply to
project source, tests, configuration, durable documentation, tracked assets,
protected user/reference material, and Git state; they do not prohibit creating
the task's required records under AI-Logs/Prompts/ and
AI-Logs/Reports/. A report returned only in Copilot chat does not satisfy the
paired-report requirement. The task is not complete until both required files
have been written to their designated AI-Logs directories.


`AI-Logs/Prompts/`, `AI-Logs/Reports/`, and `AI-Logs/Copilot.txt` are
development-continuity records intended to remain in GitHub. In an explicitly
authorized milestone closeout, include all legitimate current prompt/report
records and `Copilot.txt`, not only the current pair. This policy does not
authorize a commit or push: Neal must explicitly authorize it, either directly
or through the current task. Do not rewrite historical reports when a
milestone advances; create an appropriate follow-up report. Keep TEMP files,
gameplay recordings, screenshots, videos, ROM/reference and user assets out of
Git unless separately authorized. Future archival, ZIP consolidation, or
removal of AI-Logs requires separate authorization.

Investigation findings and implementation candidates are not accepted
behavior. After Neal manually accepts a behavior, its closeout should update
the appropriate durable documentation with the accepted behavior, evidence
classification, decisions/translations, fidelity boundaries, and acceptance
status. AI reports are historical records, not substitutes for those durable
authorities.

## 8. Command, security, and recovery safety

- Give potentially long tests, simulations, searches, probes, waits, polling,
  and loops a task-appropriate timeout or explicit work bound. Do not impose
  one arbitrary timeout on every command.
- If an operation makes no meaningful progress within a reasonable duration,
  stop it, inspect available output/state, and isolate the cause with smaller
  bounded runs. Do not immediately repeat the same unrestricted operation.
  Correct tests only after expected production behavior is established; do
  not weaken valid tests to hide defects.
- Treat Windows Application Control, Smart App Control, Code Integrity, and
  similar blocks on required executables/assemblies as security or
  infrastructure results. Record the command, artifact/path, message, and
  error code when available. Never weaken or bypass security or ask Neal to do
  so. Do not repeatedly rebuild/retry the same blocked artifact or use stale
  binaries. Retry only by a materially different safe route.
- Canceled, timed-out, hung, stalled, blocked, terminated, killed, or otherwise
  incomplete work is never a pass. Report applicable outcomes explicitly as
  **PASS**, **FAIL**, **BLOCKED**, **TIMEOUT**, **CANCELED**, or **PARTIAL**;
  claim validation passed only when the required validation completed.
- Stop voluntarily if required work cannot continue safely or reliably.
  Preserve candidate work and unrelated files, and give a precise handoff.
  If useful, create the requested report as **BLOCKED** or **PARTIAL**, stating
  what completed, what remains, the blocker, validation status, and Neal's
  next action.

## 9. Completion requirements

For substantial Tempest tasks using the completion-token convention, read the
entire substantive user task first. Before project or repository action,
verify it contains exactly one complete standalone token beginning with
`TEMPEST-END-`. The token must be on a line by itself and may contain only
uppercase ASCII letters, digits, and hyphens after that prefix. Automatically
appended VS Code reminders, attachment/tagged-file metadata, workspace
context, and other agent-generated material after the user's token do not
invalidate it. This is truncation detection, not appended-context rejection.
If exactly one valid token cannot be established, stop before project or
repository action and explain why. The former underscore-based `TEMPEST_...`
format is obsolete for new prompts; do not require both formats or rewrite
historical prompts/reports. Do not require tokens for trivial questions,
simple lookups, or minor interactions.

## 10. Enemy, palette, and tube architecture requirements

- Establish enemy or gameplay-feature eligibility before applying ROM
  parameters. A valid nonzero parameter does not imply that the feature is
  active.
- Keep enemy and gameplay-object core behavior shared across levels. Level
  configurations select eligibility, supply, speed, timing, cargo, firing,
  and movement programs rather than duplicating per-level actor engines.
- A level-specific below-rim Flipper program ends at rim arrival unless ROM
  evidence establishes a distinct model. Rim pursuit then uses the shared
  lifecycle.
- Persistent gameplay and HUD colors resolve by active 16-level palette cycle
  and semantic role, independently of tube shape. Transient effects restore
  to the active cycle palette.
- The canonical 16 tube shapes are reused by later levels. Shape-specific
  geometry, projection, and calibration remain shape-keyed; displayed-level
  gameplay and cycle palette remain separate.
