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

Perform a bounded READ-ONLY comparison of canonical Level 6 Tanker-released
Flipper movement against the already-decoded Atari ROM behavior.

Repository:

C:\Users\nrnob\source\repos\Tempest

Accepted baseline beneath the current uncommitted Level 6 candidate:

94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933

The current Level 6 concurrency candidate is present in the worktree.

Its automated validation has passed, the pending-lane Spike crash is fixed,
and Neal reports that the Level 6 manual concurrency/gameplay checks now pass.

Do not modify that candidate.

Read:

AI-Logs/Reports/2026-11-10-level6-flipper-0b-provenance-check-report.md

AI-Logs/Reports/2026-11-10-level6-pending-lane-spike-crash-correction-report.md

The provenance report already established a static mismatch.

Do not repeat broad reverse engineering.

This task is a bounded comparison intended to determine whether that mismatch
is sufficiently gameplay-significant to warrant a separate correction.

Do not modify production code.
Do not modify tests.
Do not modify durable documentation.
Do not commit.
Do not push.
Do not launch the game.


MANUAL CONTEXT

Neal considers tube-specific Flipper movement a critical Tempest gameplay
mechanic.

Experienced players may learn these patterns primarily through repeated play
and muscle memory rather than consciously memorizing numerical sequences.

However, Neal is NOT currently reporting that Level 6 Tanker-released Flipper
movement looks wrong.

Neal has compared the current Level 6 behavior with the arcade and reports
that it looks correct to him.

Therefore:

Do not assume the static ROM/C# mismatch automatically requires a code change.

Determine its concrete behavioral significance first.


ALREADY-ESTABLISHED ROM FACTS

Do not re-prove these unless a narrow clarification is necessary.

The provenance report established:

1. Displayed Level 6 loads Flipper movement program $0b.

2. $0b is a movement p-code offset, not a speed and not a count of eleven
   flips.

3. $0b performs:
   - eight along-tube movement advances;
   - then starts a flip;
   - the flip completes without along-tube movement during the turn;
   - the sequence repeats.

4. The $0b loop does not use:
   - a Spike predicate;
   - player targeting;
   - a scripted direction reversal.

5. Tanker cargo has a ROM depth-dependent branch:
   - cargo with saved enemy_along >= $20 receives the level-selected
     initialization and therefore Level 6 $0b;
   - cargo nearer the rim with saved enemy_along < $20 receives default $07
     straight motion.

6. Player-shot Tanker destruction can reach the deep-cargo $0b path.

7. Natural Tanker splitting occurs in the nearer-rim condition and therefore
   uses $07 rather than $0b.

8. Ordinary Level 6 Flippers are excluded by the Level 6 zero-minimum
   admission rule, so Tanker cargo is the normal gameplay route by which
   Flippers appear.


ALREADY-ESTABLISHED C# FACTS

The provenance report established:

- canonical Level 6 uses BelowRimFlipperProgram.Existing;
- there is no explicit Level 6 $0b movement policy;
- Level 6 Tanker cargo starts in the common cargo release pivot;
- Level 6 cargo climbs while turning;
- it receives a below-rim pause;
- it then performs another same-direction pivot;
- the cycle repeats until rim behavior takes over;
- the C# path does not implement the ROM eight-straight-advances followed by
  a stationary-depth flip;
- Level 6 cargo currently has no ROM-equivalent distinction between deep-shot
  $0b cargo and nearer-rim $07 cargo.

Do not restate these findings without adding useful comparison evidence.


PURPOSE

Determine exactly how visually/gameplay-significantly the existing C# Level 6
Tanker-cargo movement differs from the two relevant ROM paths:

A. Deep-shot cargo using $0b.

B. Near-rim/natural cargo using $07.

The goal is to give Neal a precise manual comparison target.

Do not implement the ROM behavior in this task.


A. DECODE THE PRACTICAL $0b CADENCE

Using the already identified ROM instructions, describe the Level 6 $0b
movement in concrete gameplay-observable terms.

Determine where evidence permits:

- how the eight movement advances translate into progression along the tube;
- when the lane flip starts relative to those advances;
- whether along-depth position remains fixed throughout the flip;
- what happens immediately after the flip completes;
- how direction persists;
- when the next eight-advance interval begins;
- whether the pattern repeats indefinitely below the rim;
- what happens when rim conditions supersede the below-rim program.

Do not invent a C# world-height mapping unless existing project mapping makes
one justified.

Prefer cadence/state descriptions over speculative pixel distances.


B. DECODE $07 SUFFICIENTLY FOR COMPARISON

Trace only enough of default $07 to establish the visible natural/near-rim
cargo behavior.

Determine:

- whether $07 means continued straight along-tube motion until another
  condition supersedes it;
- whether below-rim lane flipping occurs under this path;
- how it reaches rim behavior.

Do not broaden into a complete movement-program catalog.


C. CURRENT C# CADENCE

Describe the corresponding current C# Level 6 cargo sequence in equivalent
terms.

For example establish from source:

- initial release pivot;
- whether depth increases during that pivot;
- post-pivot pause;
- whether depth changes during the pause;
- next pivot;
- direction persistence;
- repetition;
- rim takeover.

The description should make ROM versus C# comparison possible without requiring
Neal to interpret implementation classes or variable names.


D. DEEP RELEASE VERSUS NATURAL RELEASE

Identify practical deterministic ways Neal could create and recognize:

1. a Tanker destroyed sufficiently deep in the tube that arcade cargo should
   use $0b;

2. a Tanker reaching its natural near-rim split where arcade cargo should use
   $07.

Do not require exact ROM coordinate measurement by Neal.

Give observable setup descriptions based on Tanker depth and release cause.

If the current C# implementation does not distinguish those two cases, say so
explicitly.


E. DIRECTION / HANDEDNESS

The provenance report established opposite initial direction bits for the two
cargo children but did not establish screen-left/right semantic equivalence.

Do not invent a clockwise/counterclockwise interpretation.

Determine only what can safely be compared:

- whether the two children begin with opposite lane-travel directions;
- whether each child retains or reverses its direction under $0b;
- whether C# behaves equivalently in that respect.

Keep unsupported screen-direction mapping UNKNOWN.


F. MANUAL COMPARISON CHECKLIST

Produce a very short manual comparison checklist for Neal.

It should focus on observable differences that could distinguish the ROM and
current C# paths.

For deep-shot cargo, likely observations include where established:

- amount of straight climbing before the first flip;
- whether the cargo continues climbing while the flip is visibly occurring;
- cadence between successive flips;
- whether travel direction remains persistent.

For natural/near-rim release:

- whether cargo climbs straight below the rim;
- whether it begins repeated below-rim pivots in C# where the ROM $07 path
  would remain straight;
- transition to rim behavior.

Do not turn this into a long manual-test campaign.

Aim for a few discriminating observations that Neal can recognize quickly
while using normal gameplay or recorded playback/frame stepping.


G. DECISION THRESHOLD

Classify the result into one of these:

A. STATIC DIFFERENCE, MANUALLY INSIGNIFICANT

The implementation differs internally but the observable gameplay difference
is negligible enough that Neal's existing manual acceptance reasonably
controls.

Recommend preserving current C# behavior as an accepted PROJECT TRANSLATION.

B. OBSERVABLE FIDELITY DIFFERENCE

The ROM/C# difference should be visible under a bounded deep-shot or natural-
split scenario.

Provide the exact observations Neal should make before any implementation is
authorized.

Do not modify code.

C. CURRENT C# CANNOT REPRESENT IMPORTANT ROM DISTINCTION

For example, if the missing deep-$0b-versus-near-$07 branch necessarily erases
a clearly gameplay-significant level-specific distinction.

State that precisely and recommend the smallest future implementation
milestone.

Do not implement it.


EVIDENCE CLASSIFICATION

Use:

ROM-BACKED
PROJECT-CURRENT
PROJECT TRANSLATION
ARCADE-OBSERVED
INFERENCE
UNKNOWN

Do not downgrade Neal's current manual observation that Level 6 movement looks
correct.

Do not upgrade current C# movement to ROM-backed merely because Neal accepts
its appearance.


VALIDATION

Read-only only.

Do not:

- modify production code;
- modify tests;
- modify durable documentation;
- build;
- run tests;
- launch the game;
- stage;
- commit;
- push.

At completion verify:

- candidate unchanged;
- index unchanged;
- HEAD remains at
  94ad02f52f8fe7f1eec79d60ffd7d5114b8ee933;
- git diff --check passes.


REPORT

Preserve this prompt and create:

AI-Logs/Reports/2026-11-10-level6-flipper-0b-bounded-comparison-report.md

Keep the report concise and operational.

Include:

- ROM $0b cadence;
- ROM $07 near-rim path;
- current C# cadence;
- deep-shot versus natural-split distinction;
- direction evidence;
- short manual checklist;
- A/B/C decision;
- whether a movement correction is recommended;
- confirmation nothing was modified.


STOP

Stop after the comparison and recommendation.

Do not implement movement changes.
Do not modify the Level 6 concurrency candidate.
Do not commit.
Do not push.
Do not begin Level 7 concurrency.


TEMPEST_LEVEL6_FLIPPER_0B_BOUNDED_COMPARISON_COMPLETE