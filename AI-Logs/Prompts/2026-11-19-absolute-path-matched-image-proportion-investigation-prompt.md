READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED CHECK

There are only TWO conditions that authorize stopping this task before the
analysis is complete:

1. The completion-token check below fails.
2. One or more of the four explicitly authorized absolute image paths cannot
   actually be opened as an image.

Do NOT stop merely because:

- some tube lines are obscured by gameplay objects;
- automatic line extraction is imperfect;
- one or more landmarks are uncertain;
- the C# and reference gameplay states differ;
- an individual requested metric cannot be calculated reliably;
- the repository contains unrelated existing work;
- image segmentation requires manual/bounded geometric interpretation;
- Python is unavailable;
- one preferred analysis technique fails.

In those situations, use another bounded method where practical, continue with
the available evidence, report the uncertainty, and complete the investigation.

COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, opening images, or
taking repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify exactly one complete token beginning with "TEMPEST_" appears on a
   line by itself.
3. Automatically appended VS Code/agent context after the user's token does not
   invalidate it.
4. If exactly one complete token cannot be found, STOP before project action.

TASK

Perform the bounded READ-ONLY matched-image presentation-proportion analysis
that the preceding attempts could not complete.

The four comparison images are now stored locally at four explicit authorized
paths.

Open those files directly.

Do NOT search the workspace for images.

Do NOT require chat attachments.

Do NOT inspect any other visual asset.

AUTHORIZED ABSOLUTE IMAGE PATHS

Level 1:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-1 SideBySide.png

Level 2:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-2 sidebyside.png

Level 5:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\level-5 Sidebyside.png

Level 7:

C:\Users\nrnob\source\repos\Tempest\UserData\Screenshots\comparisons\Level-7 SidebySide.png

These four paths are explicitly authorized even though they are inside
UserData.

Opening these four exact files does NOT violate the project's opt-in image
policy.

Do not enumerate UserData.

Do not list neighboring files.

Do not search for screenshots.

Do not inspect any other PNG, JPG, video, frame, contact sheet, thumbnail, or
visual asset.

IMAGE AVAILABILITY GATE

Open all four exact absolute paths.

If all four open successfully, proceed immediately with the investigation.

If one cannot be opened:

- identify the exact failing path;
- verify the path once using a direct existence/readability check;
- if the file genuinely cannot be opened, STOP and report that specific
  failure.

Do not substitute another image.

Do not generate another N/A report if the four images open successfully.

COMPARISON LAYOUT

Each authorized image is a side-by-side comparison:

LEFT = current C# presentation
RIGHT = arcade/MAME visual reference

"MAME" identifies the source of the comparison capture only.

Do not investigate MAME source, emulation internals, scaling configuration,
video settings, aspect configuration, or implementation.

Treat the right side only as visual reference evidence.

REPOSITORY BASELINE

Expected committed baseline:

c8bc40416eff4ffaac17862f7c894e5a34d133f9

Record current:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- index;
- tracked worktree state;
- relevant untracked state.

Repository-state differences are NOT a blocker for this read-only investigation
unless they make the required project source unavailable.

Preserve all existing work.

Do not modify Git state.

CURRENT FROZEN DISPLAY CONTRACT

Preserve:

Host window:
1600 x 1280

Portrait viewport:
960 x 1280

Viewport origin:
(320,0)

Aspect:
3:4

The viewport mapping is uniform.

Do not investigate changing the viewport.

Do not change global X/Y scaling.

CONTROLLING SOURCE/MATH REPORT

Use:

AI-Logs/Reports/2026-11-19-cross-level-presentation-proportion-investigation-report.md

as the controlling source/math evidence.

It established:

- Level 1 source tube bounds W:H = 1.000.
- Level 2 source tube bounds W:H = 1.000.
- Level 5 source tube bounds W:H = 1.000.
- Level 7 source tube bounds W:H = 1.000.
- Level 7 centered RMS X:Y = 1.000.
- Level 1 and Level 7 current adapter X/Y gains are isotropic.
- Level 2 uses a Y presentation factor of 1.314.
- Level 5 uses .670 X / .880 Y presentation gains.
- Levels 2 and 5 therefore receive substantial vertical emphasis relative to
  Level 1/7.
- Level 7 record 06 itself is approximately isotropic.
- shared host camera/viewport scaling is uniform.

Do not repeat this entire source analysis.

Use it to interpret the image evidence.

PRIMARY QUESTIONS

Answer:

1. After removing translation and uniform overall size, does Level 7 still look
   materially broader than its reference?

2. If yes, approximately how much?

3. Is the Level 7 mismatch primarily:
   - X:Y proportion;
   - near/far perspective;
   - different tube geometry;
   - capture-state mismatch;
   - or a combination?

4. Does Level 1 objectively compare better under the same measurement method?

5. Do Level 2 and Level 5 demonstrate the expected vertical emphasis strongly
   enough to validate the measurement approach?

6. Would one Level-7-only presentation Y gain explain most of the residual
   difference?

7. Does a general affine map materially outperform that one-Y-gain model?

No implementation is authorized.

IMAGE ANALYSIS TOOLS

You are authorized to use bounded local analysis tools for these four images.

Python may be used for:

- image loading;
- color masking;
- pixel-coordinate extraction;
- line/edge detection;
- connected-component analysis;
- numerical optimization;
- least-squares fitting;
- residual calculations;
- geometric transformation fitting.

If Python is unavailable, use another available bounded local approach such as
PowerShell/.NET tooling or direct image inspection.

Python availability is NOT a stop condition.

Do not install packages from the Internet.

Do not modify the project to perform the analysis.

Temporary analysis files, if absolutely needed, must remain outside tracked
project content and should be cleaned up when finished.

IMAGE EXTRACTION

For each file:

1. Determine the divider between C# and reference halves.
2. Treat each half separately.
3. Identify the blue tube geometry.
4. Exclude HUD text and digits.
5. Exclude yellow Blaster vectors.
6. Exclude red/green/magenta enemies, shots, and effects.
7. Exclude viewport borders and divider lines.
8. Identify stable tube landmarks.

Automatic extraction is preferred but NOT mandatory.

If color masking misses lines or gameplay obscures a vertex:

- use direct geometric inspection;
- use nearby rail/rim intersections;
- use a reduced but well-distributed landmark set;
- mark the landmark/measurement lower confidence.

Do not stop the task because one landmark is obscured.

LANDMARK MATCHING

Prefer:

- near/outer rim vertices;
- far/deep rim vertices;
- distinctive corners;
- stable rail/rim intersections;
- characteristic shape extrema.

Do not use moving gameplay objects.

Use enough well-distributed matched landmarks to distinguish:

- translation;
- uniform size;
- X:Y proportion;
- perspective;
- local geometry difference.

Record which landmarks are high confidence and which are approximate.

CAPTURE STATE

Assess whether each left/right pair is broadly compatible.

Different:

- score;
- Blaster lane;
- enemy positions;
- shot positions;
- lives/status

do NOT make the pair incompatible.

Only treat capture state as a major problem if the tube itself is clearly in a
different state such as:

- ordinary Playing versus ExitZoom;
- ordinary Playing versus Approach;
- obviously different tube-transition projection.

If state compatibility is uncertain, continue and lower confidence.

Do not stop.

NORMALIZE TRANSLATION AND SIZE FIRST

For every level, first fit only:

- translation;
- ONE common/uniform scale;
- discrete orientation/reflection if necessary.

Do not use independent X/Y scale in the initial fit.

This removes:

- tube placement;
- raw capture size;
- ordinary uniform scaling.

Then measure residual shape difference.

Normalize residual values by an explicit reference quantity such as reference
tube height or RMS landmark radius.

LEVEL 1

Use Level 1 as the key isotropic control.

Compare:

- outer/near rim;
- deep opening;
- representative radial rails;
- top/bottom extent;
- left/right extent.

Measure:

- similarity-fit RMS residual;
- maximum residual;
- systematic horizontal residual;
- systematic vertical residual;
- near/far perspective ratios.

Determine objectively how close Level 1 is.

LEVEL 2

Use:

- four outer corners;
- four far/deep corners;
- representative rails.

Measure:

- outer C# W:H;
- outer reference W:H;
- far C# W:H;
- far reference W:H;
- similarity residual;
- best diagnostic X:Y ratio.

Compare the visual tendency with the existing C# Y factor 1.314.

The measured visual value need NOT equal 1.314 exactly.

This is a sanity check, not a pass/fail condition.

LEVEL 5

Use:

- upper stem corners;
- left/right wing extrema and bends;
- lower stem corners;
- central/deep structure;
- representative rails.

Measure similarity residual and diagnostic X:Y relationship.

Compare qualitatively/numerically with the existing .670 X / .880 Y
presentation relationship.

Again, exact recovery of code constants is NOT required.

LEVEL 7

Use the most complete reliable landmark set.

Prefer:

- four major outer lobes;
- four inward notches;
- top V;
- bottom V;
- left/right extrema;
- central/deep geometry;
- representative near-to-far rails.

After uniform similarity normalization determine:

- whether broadness remains;
- amount of broadness;
- symmetry;
- upper/lower consistency;
- notch discrepancy;
- rail-angle discrepancy;
- near/far behavior;
- local shape residuals.

Do not assume Level 7 needs a Y gain.

Measure first.

NEAR/FAR PERSPECTIVE

For each level, where measurements are reliable, report:

- near envelope width;
- near envelope height;
- far/deep envelope width;
- far/deep envelope height;
- far/near width ratio;
- far/near height ratio;
- far center displacement;
- representative rail convergence behavior.

If a quantity cannot be measured reliably:

- mark it N/A or low confidence;
- continue the rest of the task.

An unavailable individual metric is NOT a blocker.

DIAGNOSTIC LEVEL 7 Y-GAIN FIT

After the similarity fit, test one diagnostic Level-7-only Y gain.

Use an explicit fixed anchor.

Prefer the projected tube-envelope center unless source architecture provides a
more appropriate existing presentation anchor.

Fit:

current X unchanged
Y offset from anchor multiplied by gain

plus ordinary overall translation/uniform comparison alignment as needed.

Report:

- best diagnostic Y gain;
- anchor;
- RMS residual;
- max residual;
- near residual;
- far residual;
- rail-angle improvement.

This gain is VISUAL DIAGNOSTIC only.

Do not place it into source.

Do not claim Atari provenance.

GENERAL AFFINE CONTROL

Calculate a best general 2D affine fit as an upper-bound diagnostic.

Compare:

- similarity RMS;
- one-Y-gain RMS;
- affine RMS.

If the affine model is only marginally better than one Y gain, report that a
simple proportion correction explains most of the difference.

If affine is substantially better, report that one Y gain is insufficient.

If neither resolves large local residuals, identify geometry differences.

Do not implement an affine correction.

LEVEL 1 / LEVEL 7 RECONCILIATION

Resolve as far as evidence allows:

- source math says Level 1 and Level 7 are both isotropic;
- manual observation says Level 1 looks closer to reference;
- Level 7 looks broader.

Determine whether image evidence shows:

- Level 1 also needs a proportion correction;
- the Level 7 reference is inherently narrower;
- Level 7 has a perspective difference;
- Level 7 needs a presentation-only proportion change;
- or another explanation.

Do not force a conclusion if measurements genuinely cannot distinguish two
possibilities.

LEVEL 2 / LEVEL 5 SANITY CHECK

Use Levels 2 and 5 to determine whether the measurement method detects their
known visual vertical emphasis.

If the method does not reproduce exact code factors, that is acceptable.

The relevant question is whether the measured direction and approximate
magnitude are plausible.

Do not stop if the sanity check is imperfect.

RESULT TABLE

Produce a concise table for Levels 1, 2, 5, and 7 with:

- resolved image path;
- matched landmark count;
- capture-state confidence;
- raw C# W:H;
- raw reference W:H;
- similarity scale;
- similarity RMS;
- similarity maximum residual;
- diagnostic Y gain;
- Y-gain RMS;
- affine RMS;
- near/far classification;
- final diagnosis;
- confidence.

SOURCE / IMAGE RECONCILIATION

Keep separate:

IMPLEMENTATION-ESTABLISHED:
facts from current source/math.

VISUAL DIAGNOSTIC:
measurements/fits from these four images.

MANUAL OBSERVATION:
Neal's supplied visual judgments.

PROJECT DECISION / PROJECT TRANSLATION:
accepted current presentation choices.

UNRESOLVED:
anything neither source nor image evidence settles.

Do not turn image-fit values into ROM facts.

FROZEN SYSTEMS

Do not change or recommend reopening the global viewport unless the four image
comparisons reveal one common systematic global error.

Preserve:

- 1600x1280 host;
- 960x1280 viewport;
- viewport origin (320,0);
- Level 7 gameplay geometry;
- Level 7 shared presentation architecture;
- accepted per-level placement;
- Levels 1-6/8/9;
- Level 10 diagnostic;
- gameplay mechanics.

POTENTIAL NEXT EXPERIMENT

If image evidence strongly supports a simple Level-7-only Y gain:

Describe the smallest future DEVELOPMENT-ONLY experiment.

Do not implement it.

The possible experiment should preserve:

- gameplay geometry;
- tube/entity registration;
- shared projector;
- horizontal center;
- viewport;
- gameplay mechanics;
- accepted cross-level systems.

Manual A/B acceptance would remain required.

If image evidence indicates perspective rather than proportion, describe that
instead and do not recommend a Y-gain experiment.

If geometry difference dominates, identify that without changing geometry.

NO PROJECT CHANGES

Do not modify:

- source;
- tests;
- configuration;
- documentation;
- viewport;
- presentation;
- geometry;
- gameplay;
- recordings;
- Git state.

Do not launch the game.

Do not stage, commit, push, reset, restore, clean, stash, rebase, or switch
branches.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this substantive prompt under AI-Logs/Prompts/;
- the paired absolute-path image-comparison report under AI-Logs/Reports/.

These two ignored log files are the only permitted project writes.

REQUIRED REPORT

Answer near the beginning:

1. Did all four absolute image paths open successfully?
2. After translation/uniform-size normalization, does Level 7 remain broader?
3. If yes, by approximately how much?
4. Is the issue mainly proportion, perspective, geometry, or capture state?
5. Does Level 1 objectively compare more closely?
6. Do Levels 2 and 5 provide a useful sanity check?
7. What diagnostic Level 7 Y gain best fits, if justified?
8. How much does that improve over uniform similarity?
9. Does general affine fitting materially improve beyond one Y gain?
10. Should the viewport remain frozen?
11. Should gameplay geometry remain frozen?
12. Is one bounded Level-7-only presentation experiment justified?

The report must include:

- all four exact paths;
- confirmation no other visual files were inspected;
- extraction method;
- landmark method;
- capture-state assessment;
- Level 1 findings;
- Level 2 findings;
- Level 5 findings;
- Level 7 findings;
- comparison table;
- near/far findings;
- Level 7 similarity fit;
- Level 7 one-Y-gain fit;
- Level 7 affine fit;
- Level 1/7 reconciliation;
- Level 2/5 sanity check;
- source/image reconciliation;
- evidence classifications;
- confidence and limitations;
- smallest next experiment if justified;
- repository start/finish state;
- confirmation no project/Git state changed except ignored AI-Logs;
- exact saved prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete the analysis autonomously.

TEMPEST_ABSOLUTE_PATH_MATCHED_IMAGE_PROPORTION_READONLY