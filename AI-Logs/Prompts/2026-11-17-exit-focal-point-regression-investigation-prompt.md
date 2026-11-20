READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, inspecting images,
or taking any repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears
   on a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check detects an incomplete or truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Perform a bounded READ-ONLY regression investigation of the tube-exit focal
point / zoom-center defect under --atari-presentation.

Manual testing establishes a level-dependent regression:

WORKING EXIT CONTROLS:
- Level 1
- Level 2
- Level 3
- Level 4

FAILING EXIT CASES:
- Level 5
- Level 8
- Level 9

LEVEL 7:
- exclude from behavioral comparison because Level 7 has separate unresolved
  presentation problems.

The expected architecture is that tube exit/zoom is fundamentally shared
across the game. Therefore the selective Level 5/8/9 failure is suspicious and
must be explained from the presentation paths rather than assumed to be an
intended per-level difference.

Do not implement a correction.

Do not change gameplay.

Do not retune any accepted presentation values.

MANUAL OBSERVATION

Neal reports:

- exit begins looking normal;
- the Blaster travels down the tube;
- as exit progresses, the zoom/perspective becomes increasingly wrong;
- the apparent zoom focal point appears to be the wrong coordinate;
- the perspective should remain focused on the tube's far/deep exit opening;
- instead the tube expands around another apparent point, so the visible exit
  opening drifts and the perspective becomes increasingly skewed;
- Levels 1-4 do not show the problem;
- Levels 5, 8, and 9 do show the problem;
- Level 7 is excluded because its separate presentation problems make it an
  unsuitable control;
- this is a regression that did not exist before the current whole-game
  Atari-presentation milestone.

Treat the "wrong focal point" description as a MANUAL DIAGNOSTIC HYPOTHESIS.

Determine the actual mathematical cause from code and deterministic exit state.

RECENT FIXES ARE FROZEN

Do not reopen the accepted Blaster size correction.

Controlling closeout:

AI-Logs/Reports/2026-11-17-blaster-exit-approach-scale-closeout-report.md

That correction is frozen and established that:

- Blaster travel-state size is normalized through the active presentation
  adapter against a stable gameplay-rim baseline;
- extreme Blaster stretching during exit is fixed;
- gameplay/exit simulation was not changed;
- tube geometry/camera simulation was not changed.

Do not attribute this focal-point problem to that scale correction merely
because both concern tube exit.

The focal-point regression existed independently.

AUTHORIZED IMAGE EVIDENCE

You MAY inspect only these four supplied Level 5 frames:

- playback-level-5-boundary-007108.png
- playback-level-5-boundary-007125.png
- playback-level-5-boundary-007140.png
- playback-level-5-boundary-007203.png

Locate only these exact filenames if necessary.

Do not enumerate, open, inspect, or analyze other images, screenshots, videos,
frames, contact sheets, or visual assets.

Use the four frames only to characterize the visible Level 5 symptom.

Do not fit camera coordinates from screenshot pixels.

Do not derive correction constants from the images.

PRIMARY QUESTION

During ExitZoom, around what exact point does the current C# Atari-presentation
geometry mathematically zoom?

For each applicable level, determine:

1. the source-space projection origin;
2. the logical projection center;
3. the level/model-frame origin;
4. the tube's near-rim center;
5. the tube's far-rim/deep-opening center;
6. the post-projection level translation;
7. the playfield-camera target;
8. the host-space zoom center;
9. the effective point that remains fixed as K/ZOffset changes;
10. whether that fixed point coincides with the visible far opening.

Then explain why the answer differs between working and failing levels.

CORE HYPOTHESIS TO TEST

Test this explicitly without assuming it is correct:

Some levels may receive presentation-space transforms after depth projection
that reposition the visible tube, while the ExitZoom projection/camera still
magnifies around an earlier/global projection origin.

If so, a level can look correct during ordinary Playing because tube and
entities share the final translation, yet ExitZoom can increasingly magnify
around the wrong focal point as K/ZOffset changes.

Determine whether this describes Levels 5, 8, and 9.

Also determine why Levels 1-4 remain correct if they pass through nominally
the same global ExitZoom state.

WORKING VERSUS FAILING MATRIX

Build a concise before-any-conclusion comparison for:

- Level 1: working closed control
- Level 2: working control
- Level 3: working control
- Level 4: working nontrivial enclosed control
- Level 5: failing case
- Level 8: failing accepted open tube
- Level 9: failing accepted open tube
- Level 10: diagnostic comparison if its current ExitZoom path supplies useful
  evidence
- Level 7: architecture only if necessary, not behavioral control

For each list:

- geometry source;
- topology;
- CenterY;
- YOffset;
- model-frame mapping;
- ROM/presentation metadata path;
- axis swap/reflection;
- post-projection X translation;
- post-projection Y translation;
- accepted placement translation;
- active AtariPresentationAdapter route;
- ExitZoom projection function;
- final playfield camera;
- any special AvoidSpikes behavior before travel;
- effective far-opening position under normal Playing;
- effective far-opening position through exit samples.

The goal is to identify the smallest property shared by failing 5/8/9 but not
working 1-4.

LEVEL-SPECIFIC TRANSFORM ORDER

Trace the exact ordering of:

raw/model geometry
-> model-frame mapping
-> Projection/AtariProjection operation
-> level-specific presentation transform
-> accepted level placement
-> ExitZoom K/ZOffset transformation
-> playfield camera
-> host viewport

Be precise about which transforms occur before versus after depth-dependent
projection.

A translation after projection does not generally behave the same as changing
the projection center before zoom.

Determine whether any accepted translations are being applied in the wrong
coordinate stage for ExitZoom.

LEVEL 5 SPECIAL ATTENTION

Level 5 is particularly informative because Levels 1-4 work while Level 5
fails.

Trace all presentation differences introduced at Level 5, including where
applicable:

- axis-specific mapper;
- model-frame scale;
- reflection;
- CenterY/YOffset;
- ROM metadata;
- presentation translation;
- Spike-exit/AvoidSpikes state.

Determine which differences can mathematically move the exit focal point.

Do not assume AvoidSpikes is causal merely because Level 5 uses it.

Separate:

A. pre-travel AvoidSpikes state;

from:

B. actual shared ExitZoom state after travel begins.

If Level 5 still uses a wrong focal point once in the common ExitZoom path,
identify the presentation-space cause.

LEVEL 8/9 SPECIAL ATTENTION

Levels 8 and 9 are manually accepted for ordinary Playing presentation.

Their accepted final vertical placements are frozen:

- Level 8: +212.28139 logical Y
- Level 9: +180.96825 logical Y

These translations must not be changed during this investigation.

Determine whether those post-projection placements contribute to exit focal
drift.

If they do, distinguish:

- ordinary placement being correct;

from:

- placement being applied at an inappropriate coordinate stage during
  ExitZoom.

Do not conclude that the accepted placements themselves are wrong merely
because exit is wrong.

LEVEL 10 COMPARISON

Accepted diagnostic Level 10 uses a frozen +50 placement.

If the diagnostic Level 10 exit is available through its existing AvoidSpikes
gate, use it as an architectural comparison.

Determine whether Level 10 behaves like Level 5/8/9 mathematically or whether
another transform compensates for its placement.

Do not alter or manually retest Level 10 during this read-only task.

DETERMINISTIC EXIT SAMPLES

Use existing deterministic state/test infrastructure.

Do not launch the interactive game.

For each representative working/failing level, evaluate bounded ExitZoom
samples such as:

- last Playing / ExitDelay state;
- first ExitZoom update;
- end of ZOffset phase;
- first K-growth update;
- mid K-growth;
- late K-growth before successful handoff.

Use actual production cadence.

For each sample calculate:

- K;
- ZOffset;
- player depth;
- tube near rim center;
- tube far rim/deep-opening center;
- applicable mapped presentation center;
- final logical far-opening coordinate;
- final host-space far-opening coordinate;
- displacement of that opening from the intended exit focal point;
- tube envelope center;
- relevant camera target.

Do not fabricate states.

FAR-OPENING STABILITY TEST

Define a mathematical diagnostic for exit focal stability.

The intended test is not that the far opening never changes size.

Instead determine whether the far opening remains at the correct focal
location as the tube enlarges around it.

For each sample report the far-opening center's host-space drift relative to:

- its intended exit focal point;
- its position at ExitZoom onset.

Compare working Levels 1-4 against failing 5/8/9.

If the controls also drift mathematically but appear correct manually, identify
what distinction makes their result visually coherent.

TRANSFORM COMMUTATION TEST

For suspected level transforms, compare:

A. project/zoom first, then translate;

versus

B. incorporate the translated focal center into projection/zoom, then map to
screen.

Show whether the operations commute.

For an affine final camera translation they may commute as a simple screen
move; for a depth-dependent projector centered elsewhere they may not.

Use the actual C# equations.

This should help identify whether level-specific presentation placement is
being treated as a screen translation when ExitZoom requires a shifted
projection center.

DO NOT CHANGE EXIT SIMULATION

Verify and preserve:

- exit delay;
- AvoidSpikes behavior;
- player exit depth;
- K/ZOffset cadence;
- lane control;
- Spike collision behavior;
- Approach handoff;
- scoring/lives;
- RNG;
- recording/playback.

Do not classify this as simulation unless actual state evolution differs
between controls and failures contrary to intended configuration.

REGRESSION HISTORY

Identify the last known architecture/commit where Levels 5/8/9 exits were
manually or structurally correct before the regression.

Use read-only Git/history and prior reports.

Do not reset/check out older worktrees.

Determine the narrowest change that introduced the failing focal relationship.

Pay particular attention to introduction of:

- AtariPresentationAdapter per-level mappings;
- axis-specific model-frame transforms;
- final placement translations;
- portrait presentation camera changes;
- ExitZoom presentation integration.

Do not assume the newest commit is the cause.

FROZEN WORK

Do not modify or recommend retuning as part of this investigation:

- accepted Level 8 placement;
- accepted Level 9 placement;
- accepted Level 10 placement;
- Blaster ordinary orientation;
- Blaster exit/Approach scale;
- Levels 1-6 accepted tube presentation;
- shared perspective;
- projection K/ZOffset cadence;
- HUD;
- gameplay geometry.

Level 7 remains separate.

NO IMPLEMENTATION

Do not edit:

- source;
- tests;
- project files;
- configuration;
- durable documentation;
- presentation;
- gameplay;
- recordings;
- Git state.

Do not add diagnostics to production code.

Temporary bounded calculations outside tracked project content are permitted.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

AI-LOGS

Follow .github/copilot-instructions.md.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired focal-point regression report under AI-Logs/Reports/.

These required ignored logs are the sole permitted file-writing exception.

Do not stage or commit AI-Logs.

REPOSITORY SAFETY

Record before investigation:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline:

753728e79ed86624c9b1728dfd1c8db8f49d1e2b

on master tracking origin/master at 0/0.

Verify current state.

Preserve the unrelated .github/copilot-instructions.md modification and local
reference files.

EXPECTED REPORT

Near the beginning directly answer:

1. What point does ExitZoom actually zoom around for Levels 1-4?
2. What point does it zoom around for Levels 5, 8, and 9?
3. Why do the failing levels diverge while the working levels do not?
4. Is the far/deep opening moving because of a post-projection transform,
   wrong projection center, wrong camera target, or another cause?
5. Which exact code/path distinction separates the failing and working groups?
6. What regression milestone introduced that distinction?
7. Does gameplay/exit simulation remain correct?
8. Is enough evidence available for one bounded shared correction?

The report must include:

- exact authorized frames inspected;
- objective Level 5 visual characterization;
- working/failing matrix;
- per-level transform chain;
- effective projection/focal center;
- deterministic ExitZoom sample calculations;
- far-opening host-space trajectories;
- focal drift measurements;
- transform-order/commutation analysis;
- Level 5-specific findings;
- Level 8/9 placement findings;
- Level 10 comparison if useful;
- AvoidSpikes relevance or elimination;
- regression boundary;
- exact root cause if established;
- evidence classification;
- smallest technically justified future correction direction;
- whether correction can be shared/global rather than per-level;
- risks to accepted Playing placement;
- confirmation Blaster scale closeout remains frozen;
- confirmation gameplay and Git state remain unchanged;
- exact AI-Logs paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously and stop after reporting evidence.

TEMPEST_EXIT_FOCAL_POINT_REGRESSION_READONLY