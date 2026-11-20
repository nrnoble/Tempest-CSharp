READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check is intended to detect an incomplete or truncated user task, not
   appended agent context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a bounded READ-ONLY investigation of the normal Atari Level 7 Math Box
call-state used to project record-06 tube coordinates.

The previous Level 7 direct-Atari geometry investigation narrowed the first
unresolved numerical uncertainty to the Math Box quotient state used by the
prototype, particularly its assumption that RN low byte is $0F and RN high
byte is zero.

Resolve that uncertainty as far as the available Atari evidence permits before
any further Level 7 implementation work.

This is investigation only.

Do NOT implement a correction.

READ-ONLY MEANING AND REQUIRED AI-LOGS EXCEPTION

Follow the current .github/copilot-instructions.md rules.

This task is read-only with respect to project source, tests, configuration,
durable documentation, protected user/reference material, and Git state.

The normal ignored paired records under AI-Logs are REQUIRED and are the
explicit permitted file-writing exception:

- Save this substantive prompt under AI-Logs/Prompts/.
- Save the completed investigation report under AI-Logs/Reports/.
- Use descriptive date-prefixed paired filenames.
- A report returned only in Copilot chat is not a substitute for the report
  file.
- Do not stage or commit AI-Logs.

CONTEXT

The direct-Atari Level 7 prototype now launches with:

dotnet run --project .\src\Tempest.Game\Tempest.Game.csproj -- --level 7 --atari-tube-prototype

Manual comparison established that the prototype is recognizably related to
the Atari/MAME Level 7 tube but its internal geometry is not rendered
correctly.

The preceding read-only geometry investigation found no demonstrated defect
in:

- record-06 extraction;
- the 16-point record ordering;
- normal near/far positions p=$10 / 16 and p=$F0 / 240;
- same-index near/far correspondence;
- rail pairing;
- closed-rim topology;
- basic conditional coordinate arithmetic;
- or the final global viewport map as an explanation for a non-uniform
  internal-geometry discrepancy.

It identified one later ROM coordinate clamp that the prototype does not
implement, but found that the listed Level 7 endpoints do not reach the range
where that clamp would alter them. Do not assume that difference explains the
observed Level 7 geometry.

The earliest unresolved numerical assumption was the state feeding the Math
Box quotient operation. The current AtariCoordinateProjector produces its
coordinates under an explicit prototype assumption involving RN low byte $0F
and RN high byte zero.

That conditional result must not be promoted to actual Atari behavior unless
the call-state evidence establishes it.

PRIMARY QUESTION

For normal Atari Level 7 tube construction using record 06, what Math Box state
actually exists when the coordinate projection operation runs?

In particular, determine whether the currently assumed quotient state is
correct, incorrect, partially correct, or unresolvable from available evidence.

TRACE THE ACTUAL CALL PATH

Using the project-local Atari disassembly and the existing Math Box/projection
investigation reports, trace the normal Level 7 path sufficiently far in both
directions to establish the state that matters to coordinate projection.

Investigate, as applicable:

1. How Level 7 selects record 06.
2. How the selected record data reaches the tube-coordinate projection code.
3. The calls/branches leading into the coordinate operation.
4. Relevant Math Box operation selection or PROM mode.
5. Registers/state written immediately before the divide/projection step.
6. Registers/state intentionally retained from previous Math Box operations.
7. RN low byte.
8. RN high byte.
9. Any counter, shift, quotient, remainder, sign, or normalization state that
   affects the resulting coordinate.
10. Whether the Math Box operation resets any of this state or depends on
    retained hardware state.
11. Whether the first projected point differs in starting state from subsequent
    points.
12. Whether X projection and Y projection begin with identical relevant state.
13. Whether near and far tube construction begin with identical relevant state.
14. How Y3D, Y2D/base, scale, and runtime position are established at these
    call sites.
15. Whether any relevant state differs between the normal p=$10 and p=$F0
    passes.

Do not infer zero initialization merely because the C# prototype currently uses
zero.

Likewise, do not infer retained nonzero state merely because retention is
possible. Establish it from evidence where possible.

MATH BOX EVIDENCE

Use the strongest local evidence available, including:

- Reference/arcade/Tempest Rom Source code.md
- existing local Math Box / PROM / projection investigation reports;
- existing verified extracted tables and metadata;
- local ROM material only if the current project instructions and evidence
  genuinely require a bounded verification that cannot be answered from the
  established local reports/disassembly.

Do not search the Internet for another Tempest disassembly or substitute an
outside reconstruction for available project-local Atari evidence.

If an earlier report made a conditional assumption, trace its cited evidence
rather than treating the report's assumption as independently proven.

NUMERIC RECONSTRUCTION

If the call-state can be established sufficiently, reconstruct the expected
normal Level 7 record-06 coordinate calculation.

At minimum, calculate representative points that are capable of distinguishing
the current prototype assumption from the established Atari state.

Preferably calculate all 16 Level 7 record-06 endpoints for both:

- near p=$10 / 16
- far p=$F0 / 240

when the evidence is sufficient to do so reliably.

For each useful calculation, show enough intermediate state to make the result
auditable, including relevant raw point values, divisor inputs, relevant Math
Box state, quotient/output, sign/base handling, and resulting coordinate.

Compare the resulting Atari-supported numeric coordinates directly with the
coordinates produced by the current AtariCoordinateProjector.

Do not change AtariCoordinateProjector.

ACTIVE Y BASE

The earlier geometry investigation also identified the active Y base as worth
confirming.

Determine from the actual normal call path whether signed Y2D $FF70 (-144) is
the correct base at the coordinate operation being modeled, and whether it is
used in the same manner currently assumed by the prototype.

Distinguish:

- correct extracted metadata;
- correct call-site value;
- correct semantic interpretation;
- correct point-by-point application.

Do not assume that a numerically correct table extraction proves all four.

SCREENSHOT ROLE

The previously supplied MAME and C# Level 7 screenshots are valid diagnostic
evidence that a visible geometry discrepancy exists.

They MAY be inspected if useful to reason about a specific hypothesis already
arising from the Atari/math investigation.

Do not fit coordinates, projection architecture, constants, or viewport values
to the screenshots.

The purpose is to determine why the Atari-derived prototype is wrong, not to
make the output resemble one screenshot through visual tuning.

If the numeric investigation establishes the cause without requiring further
image inspection, additional visual analysis is unnecessary.

DO NOT REOPEN ESTABLISHED AREAS WITHOUT EVIDENCE

Do not re-investigate record-06 extraction, ordering, rail topology, or rim
closure merely because they are available.

Reopen one of those areas only if new call-state evidence materially
contradicts the preceding investigation.

Do not retune:

- viewport translation;
- scale;
- display aspect;
- rotation;
- axis orientation;
- perspective;
- record coordinates;
- Level 7 C# gameplay geometry.

Do not modify --atari-presentation.

Do not investigate the separate Blaster-exit presentation defect.

Do not reopen accepted Level 1-6 presentations.

NO IMPLEMENTATION

Do not:

- edit source;
- edit tests;
- edit project configuration;
- edit durable project documentation;
- add diagnostic production code;
- generate replacement geometry;
- modify the prototype;
- modify gameplay;
- launch the interactive game;
- stage, commit, push, reset, restore, stash, rebase, clean, or switch branches.

Temporary bounded calculations outside tracked project content are permitted.

VALIDATION OF REPOSITORY PRESERVATION

Inspect the relevant Git/worktree state at the beginning.

At completion, verify that project source, tests, configuration, protected
material, tracked worktree state, index, branch, and HEAD were not changed by
this investigation.

The required ignored paired AI-Logs prompt/report files are expected and do
not count as violating this read-only investigation.

EVIDENCE CLASSIFICATION

For each important conclusion, classify the evidence appropriately using the
project's established evidence terminology.

Where useful for diagnostic disposition, also clearly distinguish:

- confirmed implementation mismatch;
- unsupported current prototype assumption;
- unresolved hardware state;
- conditional mathematical result.

Do not turn an inference into ROM-backed behavior.

EXPECTED REPORT

Create the required report file under AI-Logs/Reports/ and also summarize the
result in chat.

The report must state:

1. The normal Level 7 call path relevant to record-06 projection.
2. What establishes the relevant Math Box operation/mode.
3. The established RN low-byte state, if determinable.
4. The established RN high-byte state, if determinable.
5. Whether state is reset, initialized, or retained at the relevant point.
6. Whether first/subsequent points can begin from different relevant states.
7. Whether X/Y projections can begin from different relevant states.
8. Whether near/far passes differ in relevant state.
9. Whether Y2D $FF70 / -144 is the correct active base and whether its current
   prototype use is semantically correct.
10. Numeric coordinates supported by the established state, with representative
    intermediate arithmetic.
11. A comparison with the current prototype coordinates.
12. Whether the current RN.high=0 assumption is:
    - CONFIRMED,
    - REFUTED,
    - PARTIALLY SUPPORTED,
    - or UNRESOLVED.
13. Whether a specific cause of the observed Level 7 geometric discrepancy is
    now established.
14. If established, identify the earliest incorrect transformation/state and
    explain why it can produce the observed structural discrepancy.
15. If not established, identify the single narrowest remaining uncertainty and
    the evidence needed to resolve it.
16. Whether enough evidence now exists for a bounded correction implementation
    prompt.
17. The smallest technically justified future correction direction, if one is
    established, without implementing it.
18. Confirmation that no project/Git state was changed except the required
    ignored AI-Logs records.

Do not ask Neal questions during execution.

Do not pause for manual input or visual acceptance.

Complete the bounded investigation autonomously and report the evidence.

TEMPEST_LEVEL7_MATHBOX_NORMAL_CALLSTATE_READONLY_INVESTIGATION