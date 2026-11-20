FAIL-CLOSED PROMPT CHECK

Read this entire user-provided task before doing anything.

Before project or repository action, verify that exactly one complete token
beginning with "TEMPEST_" appears on a line by itself in the substantive
user-provided task.

Automatically appended VS Code/agent context after the user's token does not
invalidate it.

If exactly one complete token cannot be established, stop before project or
repository action and explain why.


TASK

Perform a bounded READ-ONLY investigation of Level 4 enemy concurrency in the
Tempest C# project.

Repository:

C:\Users\nrnob\source\repos\Tempest

Expected accepted baseline:

af560b100bebad672ec9231d4f88332848378594

Ignore local Tempest ROM reference copies

Levels 1, 2, and 3 concurrency work is completed and accepted.

Do not reopen or modify Levels 1-3.

This task is investigation only.

Do not modify production code.
Do not modify tests.
Do not modify accepted gameplay documentation.
Do not implement a Level 4 correction.
Do not commit or push.
Do not launch the interactive game.


PURPOSE

Determine as accurately as practical how the original Atari Tempest controls
enemy concurrency and ordinary enemy admission on displayed Level 4, then
compare that mechanism with the current C# Level 4 implementation.

Do not assume:

- Level 4 uses the Level 1/2 four-Flipper policy;
- Level 4 uses the accepted Level 3 mixed-admission policy;
- a table maximum is necessarily a hard simultaneous-enemy cap;
- Incitti's concurrency behavior represents Atari behavior.

Investigate Level 4 independently from the Atari ROM evidence.


LEVEL 3 REUSE

The completed Level 3 concurrency ROM investigation already established much of
the generic Atari enemy-admission machinery.

Use that prior investigation as a technical starting point where appropriate:

AI-Logs/Reports/2026-11-09-level3-concurrency-rom-investigation-report.md

Do not unnecessarily redo already established generic ROM analysis.

Instead:

1. verify which previously established generic machinery also applies to
   Level 4;
2. trace Level 4-specific ROM tables, parameters, species eligibility,
   minima/maxima, cargo behavior, and other relevant differences;
3. investigate any new Level 4-specific mechanism not covered by the Level 3
   report.

Recheck generic ROM machinery only where needed to establish that it applies to
Level 4 or where Level 4 introduces a material difference.


A. ESTABLISH ROM-BACKED LEVEL 4 WAVE PARAMETERS

Determine the Level 4 ROM-backed values relevant to concurrency/admission.

At minimum identify, where applicable:

- original/pending enemy supply;
- eligible ordinary entry species;
- Flipper minimum and maximum;
- Tanker minimum and maximum;
- Spiker minimum and maximum;
- Pulsar minimum and maximum;
- Fuseball minimum and maximum;
- Tanker cargo type and quantity;
- physical active-enemy capacity;
- any relevant difficulty-dependent differences;
- other Level 4 table values that materially affect ordinary admission.

Distinguish original/pending entries from eventual Tanker cargo.

Do not infer a fixed final species mixture merely from the original supply if
the ROM selects species dynamically.


B. DETERMINE LEVEL 4 CONCURRENCY / ADMISSION BEHAVIOR

Apply the verified Atari admission machinery to the Level 4-specific data.

Determine, where evidence permits:

- maximum ordinary simultaneously admitted Tankers;
- Flipper availability behavior;
- treatment of Tanker latent cargo;
- treatment of released cargo;
- treatment of any additional Level 4 species;
- whether species share the physical slot capacity;
- whether species-specific availability limits interact;
- whether rim enemies continue occupying capacity;
- whether transition/live enemies continue occupying capacity;
- whether pending-zap enemies continue occupying capacity;
- what frees capacity;
- whether ordinary entry is paused or affected by Super Zapper state;
- whether any Level 4-specific rule changes the generic admission mechanism.

If byte wrapping, clamping, minimum preference, species ordering, cargo
reservation, or another mechanism produces non-obvious results on Level 4,
trace and document it rather than simplifying it into a conventional cap.

Where helpful, evaluate a small bounded set of representative occupancy states
to demonstrate the actual ROM arithmetic.

Do not run an unbounded state search.


C. CURRENT C# LEVEL 4 BEHAVIOR

Inspect current production C# Level 4 behavior.

Determine:

- Level 4 configured enemy supply;
- eligible species;
- current admission/concurrency mechanism;
- existing active-enemy limits, if any;
- current Tanker limit;
- current Flipper handling;
- current cargo handling;
- treatment of any other Level 4 species;
- approach/Egg/reserve scheduling relevant to admission;
- which entities currently count toward any active-population condition;
- whether rim/transition/pending-zap enemies count;
- what frees capacity;
- whether Level 4 currently shares Incitti-shaped behavior previously found
  in Level 3;
- whether the accepted Level 3 mixed-admission implementation currently applies
  to Level 4 or is correctly isolated from it.

Do not change anything during this investigation.


D. PROTECT ACCEPTED SYSTEMS

Level 4 already participates in accepted project systems.

Identify relevant accepted behavior that a later concurrency change must not
damage, including as applicable:

- Tanker cargo lifecycle;
- enemy identities/accounting;
- death/reentry;
- Super Zapper behavior;
- firing;
- scoring;
- Spiker/Spike behavior;
- rim behavior;
- completion/all-rim behavior;
- natural progression;
- recording/playback determinism.

Do not propose broad redesign merely because the C# architecture differs from
the ROM.


E. ROM VERSUS C# COMPARISON

Produce a concise comparison covering:

1. ROM-backed Level 4 behavior.
2. Current C# Level 4 behavior.
3. Confirmed mismatches.
4. Areas that already agree.
5. Accepted project translations that should be preserved.
6. Unknowns requiring additional evidence.

Use the evidence classifications defined by the repository-wide Copilot
instructions.

Be especially explicit about:

- whether Level 4 can reuse the accepted Level 3 mixed-admission engineering
  infrastructure;
- which parameters/rules must be Level-4-specific;
- whether simply applying the Level 3 policy unchanged would be incorrect.


F. RECOMMENDED NEXT MILESTONE

After completing the investigation, recommend the smallest safe next step.

Possible conclusions include:

- no Level 4 change is warranted;
- a narrowly scoped diagnostic should be implemented;
- the Level 3 mixed-admission infrastructure can be generalized using
  Level-specific ROM parameters;
- Level 4 needs a distinct policy because its species/cargo mechanics differ.

Do not decide in advance which conclusion is correct.

Let the ROM evidence establish it.

If a correction appears warranted, describe the smallest bounded diagnostic
implementation suitable for Neal's subsequent manual arcade comparison.

Do NOT implement it in this investigation task.


VALIDATION

This is read-only investigative work.

Do not run the game.
Do not perform manual gameplay.
Do not run broad test/build suites merely for ceremony.
Do not alter tests.

Use only bounded read-only repository/ROM analysis necessary to establish the
findings.

At completion, verify that tracked project files remain unchanged.


REPORT

Preserve this substantial prompt and create the corresponding ignored
investigation report according to the repository-wide instructions.

Use a descriptive report name such as:

AI-Logs/Reports/2026-11-09-level4-concurrency-rom-investigation-report.md

The report should include:

- starting baseline/Git state;
- Level 3 generic findings reused versus findings actually reverified;
- ROM/disassembly locations examined;
- Level 4 ROM-backed wave parameters;
- Level 4 species/cargo composition;
- actual Level 4 concurrency/admission mechanism;
- representative bounded arithmetic/state examples where useful;
- current C# Level 4 behavior;
- ROM-versus-C# comparison;
- evidence classifications;
- accepted systems that later implementation must preserve;
- uncertainties;
- recommended smallest next milestone;
- confirmation that no production/test/gameplay changes were made.


STOP

Stop after completing the investigation and report.

Do not implement Level 4 concurrency changes.
Do not commit.
Do not push.
Do not launch the game.

Neal and Copilot will review the evidence before authorizing implementation.


TEMPEST_LEVEL4_CONCURRENCY_INVESTIGATION_COMPLETE
