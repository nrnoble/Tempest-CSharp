FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, committing, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" exists in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context may appear after the user's completion token and do not invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before any project/repository action.


TEMPEST POST-GAME VECTOR EFFECT VISUAL-REFERENCE REFINEMENT

Refine the existing uncommitted ROM-only Game Over vector-effect implementation using the original arcade visual evidence already supplied in:

Videos\fireworks\

The directory contains 99 sequential extracted frames from an original Atari Tempest arcade display.

DO NOT extract another set of frames from video.

The intended evidence is already available as these 99 ordered images.

IMPORTANT:

Do NOT characterize, label, model, or implement this effect using a real-world metaphor.

Do not assume what the effect is "supposed to be."

Analyze what the Atari display actually does in the sequential frames and reproduce that observed behavior.

One purpose of this milestone is to see how accurately the AI can reconstruct the arcade presentation by combining:

1. ROM-backed internal behavior; and
2. direct sequential visual evidence.

Do not replace those sources with a generic visual effect.


STARTING REPOSITORY STATE

The committed baseline remains:

b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f
Correct Super Zapper color presentation

However, the worktree contains the intentionally uncommitted ROM-only Game Over implementation documented by:

AI-Logs/Reports/2026-11-05-game-over-rom-only-implementation-report.md

That implementation is the foundation for this refinement.

Do NOT revert it.

Do NOT start the Game Over implementation over from the committed baseline.

First run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Verify:

- branch master;
- HEAD remains b349e0b3f3517c9ae9c2e1f22ce78f6d801f862f;
- the tracked/untracked implementation work corresponds to the ROM-only Game Over milestone;
- no unrelated or unexplained changes exist.

If the worktree contains unexpected changes, STOP rather than resetting, restoring, cleaning, deleting or discarding anything.


READ FIRST

Read completely:

- .github/copilot-instructions.md
- AI-Logs/Reports/2026-11-05-game-over-rom-only-implementation-report.md
- the relevant Game Over production source;
- GameOverVectorAnimation.cs;
- relevant VectorRenderer/VectorFont code;
- relevant tests changed by the ROM-only implementation;
- applicable Game Over sections of TEMPEST-DEVELOPMENT-NOTES.md;
- applicable Game Over sections of TEMPEST-BEHAVIOR-SPEC.md.

Use:

Reference/arcade/Tempest Rom Source code.md

as the sole ROM/disassembly authority.

Do NOT search the Internet.

Do NOT use MAME captures, another Tempest implementation, another disassembly, or another ROM revision.


EVIDENCE MODEL

Maintain these distinctions rigorously.

ROM-BACKED:

Behavior established by the project-local Atari ROM/disassembly.

ARCADE OBSERVED:

Visible behavior established by the sequential original-arcade frames in Videos\fireworks\ and Neal's direct arcade observations.

PROJECT DECISION / APPROXIMATION:

C# behavior necessary to bridge information neither source establishes literally.

UNKNOWN:

Anything not established sufficiently by either source.

The visual-reference images do not supersede the ROM for internal mechanics.

The ROM does not supersede direct arcade visual evidence about what those mechanics actually produce on the display.

Use the two sources together.


IMPORTANT CORRECTION ABOUT WHEN THE EFFECT OCCURS

The ROM investigation and Neal's subsequent arcade observation agree that the state-$24 visual effect is NOT shown after every arcade Game Over.

It occurs on the score-qualifying/high-score route.

Neal originally remembered it as an ordinary Game Over effect and subsequently corrected that observation after checking the arcade.

For the current C# recreation, preserve the already-authorized PROJECT DECISION:

show this visually distinctive state-$24-derived effect after every final one-player C# death.

This is intentional because the leaderboard/high-score system has not been implemented and Neal wants this arcade presentation retained in the current simplified flow.

Do not misdocument this as literal arcade behavior.


VISUAL-REFERENCE DIRECTORY

Analyze:

Videos\fireworks\

Do not use FFmpeg to re-extract frames.

Enumerate the directory first.

Verify the intended reference set.

The user reports that there are 99 sequential frames.

Determine their chronological order from the filenames.

If there are not 99 usable images or ordering is ambiguous, report the discrepancy before relying on assumptions.

Do not rename, modify or delete the reference images.

Videos\ remains development/reference material and must not be committed.


CRITICAL REFLECTION / PHOTOGRAPHY RULE

The reference images are photographs of an original arcade vector monitor.

They contain substantial real-world reflected imagery.

IGNORE ALL REAL-WORLD REFLECTIONS AND PHOTOGRAPHIC ARTIFACTS.

Specifically ignore:

- every reflected person;
- phone/camera;
- hands;
- arms;
- clothing;
- room;
- furniture;
- walls;
- ambient room lighting;
- monitor bezel;
- monitor glass;
- glare attributable to the photographed glass;
- camera exposure;
- camera sensor noise;
- compression noise;
- uneven photographic brightness;
- any other non-game imagery.

Do not attempt to:

- identify reflected people;
- interpret reflected people;
- trace reflected shapes;
- reproduce a reflection;
- remove a reflection in source files;
- model the room;
- draw a camera;
- draw a person;
- draw monitor glass;
- reconstruct photography artifacts.

The implementation target is ONLY the bright emitted Atari vector graphics visible on the arcade display.


DO NOT ASSUME A REAL-WORLD METAPHOR

Except for the literal existing directory name:

Videos\fireworks\

do NOT use the directory name as a description of what needs to be created.

Do not search for examples of real-world pyrotechnics.

Do not reason from how real-world pyrotechnics behave.

Do not design a generic celebration animation.

Do not create something because it looks aesthetically plausible.

Instead ask:

"What spatial and temporal vector behavior is directly observable in these 99 sequential arcade frames, and how does that reconcile with the ROM mechanics already traced?"

The output should be derived from the evidence, not the directory label.


ANALYZE THE FRAMES AS A SEQUENCE

Do not treat the 99 images as independent screenshots.

Analyze them chronologically.

Compare successive frames.

The investigation should characterize observable properties including, where supported:

1. Where bright vector elements first become visible.

2. Whether there is one apparent origin or multiple origins.

3. How visible elements move from frame to frame.

4. Whether they:
   - radiate;
   - translate;
   - fan outward;
   - diverge;
   - converge;
   - branch;
   - form clusters;
   - form lines;
   - form points;
   - form short segments;
   - form persistent shapes;
   - follow another observable pattern.

Do not choose among those descriptions before looking at the evidence.

5. How the spatial extent changes over time.

6. How density changes over time.

7. Whether new structures continue appearing while older structures move.

8. Whether several visibly distinct structures coexist.

9. Whether the effect appears centrally concentrated, screen-filling, directional, asymmetric or variable over the sequence.

10. How individual visible elements disappear.

11. Whether visible trails are:
   - actual multi-segment vector geometry;
   - successive movement positions;
   - CRT/camera persistence;
   - or cannot be distinguished reliably.

12. Apparent color behavior:
   - number/range of distinguishable arcade colors;
   - whether an individual structure keeps one color;
   - whether nearby structures commonly use different colors;
   - whether colors appear ordered or random;
   - whether the center and outward regions differ.

13. Apparent size/shape behavior:
   - point-like;
   - short line-like;
   - multi-line;
   - changing with position/depth;
   - changing with lifetime;
   - otherwise.

14. Whether one ROM record appears visually capable of representing more than one visible luminous/vector element.

15. The approximate visual end condition seen in the frames.

Record observations conservatively.

Do not claim that photographic bloom or CRT persistence is literal vector geometry.


DO NOT OVERENGINEER IMAGE PROCESSING

This is not a computer-vision research project.

Use the frames directly with available image inspection/viewing tools.

Simple scripts may be used if they materially help with:

- enumerating frames;
- sampling chosen frames;
- constructing contact sheets for internal inspection;
- comparing positions across neighboring frames;
- checking color clusters or bounding regions.

Do not build a generalized object tracker.

Do not attempt perfect pixel segmentation.

Do not spend substantial effort mathematically removing the reflected environment.

The goal is sufficient visual understanding for a faithful Tempest vector reconstruction.


SAMPLE BROADLY

Because there are 99 frames, do not manually inspect only the first handful.

At minimum inspect:

- beginning;
- early development;
- first substantial expansion;
- middle;
- dense/maximum-extent portions;
- later evolution;
- dissipation/end.

Use additional adjacent frames wherever motion or shape interpretation is ambiguous.

Do not assume the sample frames Neal previously showed in chat are sufficient; the project directory is the reference set for VS Code Copilot.


ROM RECONCILIATION

After visual analysis, revisit only the relevant portions of:

Reference/arcade/Tempest Rom Source code.md

and the existing ROM-only implementation.

The prior report established:

- state $24;
- 16 indexed records;
- three coordinates initialized to $80;
- POKEY-derived random movement values;
- plusminus_7 signed values;
- $10/$F0 movement bounds on bounded coordinates;
- creation into free slots while $010E remains nonzero;
- $010E = $20 and alternating decrement;
- records drain after creation stops;
- exit when active records clear;
- pending-vector palette index $0C;
- graphic ID $1F;
- local table bytes $47 and $4A;
- displayed geometry of those graphics remained UNKNOWN.

Use the frame evidence to revisit the unresolved interpretation.

In particular, investigate whether the visual evidence gives a stronger interpretation of what:

graphic ID $1F
table bytes $47/$4A

produce through the ROM's vector rendering path.

Do not perform an unbounded graphics-ROM reconstruction.

If exact ROM graphic geometry still cannot be fully decoded, reproduce the observed arcade structure through the existing vector renderer and classify that representation as ARCADE-OBSERVED / PROJECT TRANSLATION rather than ROM-BACKED geometry.


EXISTING ROM-ONLY IMPLEMENTATION

Do not throw away useful mechanics merely because its visual approximation was wrong.

Preserve where still appropriate:

- GameOverPresentation state;
- GameOverAnimation state;
- final death ordering;
- no gameplay during presentation;
- eventual terminal GameOver state;
- R-to-restart behavior;
- 16-slot purpose-specific record model;
- ROM-derived coordinate/state/counter concepts;
- deterministic/testable randomness;
- spawn/drain lifecycle;
- established final-life path coverage.

Review each current approximation against the images before deciding whether to retain it.


KNOWN ROM-ONLY APPROXIMATION TO REVISIT

The existing implementation currently renders each active record as a small cyan wireframe diamond.

Neal's manual arcade comparison found that this does NOT visually resemble the original arcade effect.

Treat the diamond strictly as an obsolete experimental placeholder unless the visual evidence unexpectedly supports part of it.

Do NOT merely:

- increase the number of diamonds;
- randomize diamond colors;
- add glow around diamonds;
- increase diamond speed;

unless sequential-frame analysis actually supports those choices.

First understand what is visibly happening.


COLOR

The old C# implementation maps ROM palette $0C to cyan.

The reference sequence visibly contains multiple distinct bright colors.

Determine what can be established from:

- the frames;
- ROM color/palette behavior;
- existing Tempest palette translations.

Do not sample the photographed screen and claim those camera RGB values are exact Atari palette RGB.

The photographed colors are evidence of:

- relative color diversity;
- approximate hue categories;
- temporal/spatial color relationships.

Use the project's established Tempest vector colors where appropriate.

If state $24, graphic data or another ROM path explains the multi-color display, preserve the ROM mechanism.

If not, classify the C# mapping needed to reproduce the arcade-visible multicolor effect as ARCADE OBSERVED / PROJECT TRANSLATION.


CRT BRIGHTNESS AND BLOOM

The photographs show bright vector emissions with CRT/camera bloom.

Do NOT attempt photorealistic bloom matching.

Do not add modern blur/shader/post-processing merely because the photographed vectors bloom.

The project's established vector-rendering style remains authoritative for C# presentation.

Reproduce:

- geometry;
- positions;
- line/point structure;
- color relationships;
- motion;
- density;
- timing;

not photographic optics.


GAME OVER TEXT

Do not broaden this task into a new GAME OVER text redesign unless the 99 frames include relevant text evidence and directly show that the current implementation is materially wrong.

The primary refinement target is the bright post-GAME-OVER vector effect.

Keep the existing ROM-informed text implementation unless there is direct evidence requiring a narrow correction.


BACKGROUND / TUBE

Do not infer tube/web behavior from portions hidden by reflections or exposure.

Preserve the existing ROM-only implementation's background behavior unless the frame sequence clearly establishes otherwise.

Do not introduce new gameplay activity during the visual effect.


IMPLEMENTATION GOAL

After analysis, make the smallest maintainable change that causes the current ROM-only implementation to reproduce the arcade-observed vector effect substantially more faithfully.

Priority order:

1. observed geometry/structure;
2. observed motion;
3. observed spatial distribution;
4. observed color behavior;
5. observed density/lifecycle;
6. timing;
7. minor appearance details.

Do not add visual complexity unsupported by evidence.


VECTOR-ONLY IMPLEMENTATION

Preserve the project's durable vector-game principle.

The result must use:

- vector lines;
- vector points represented through the established line/vector system where necessary;
- existing projection where applicable;
- established Tempest palette/colors.

Do not use:

- sprites;
- textures;
- bitmap animation;
- frame playback;
- pre-rendered images;
- the reference photographs at runtime;
- generic raster particles;
- alpha-cloud effects.

The 99 images are evidence only.


REFERENCE FILES MUST NOT SHIP

Do not make runtime code depend on Videos\fireworks\.

Do not copy reference frames into production assets.

Do not add them to Git.

Do not modify them.

The final executable must reproduce the effect procedurally.


TIMING

Use temporal visual evidence from the frame sequence to determine relative event ordering.

Do not infer real seconds unless frame timestamps/frame rate are explicitly established by the reference filenames/metadata or other project evidence.

Preserve ROM counter timing where it is established.

Where the C# 60 Hz simulation needs a translation, keep it explicitly classified as PROJECT APPROXIMATION.

Do not tune unrelated game timing.


GAMEPLAY SCOPE

Do not change gameplay semantics.

Preserve the existing project decision:

final one-player death
-> GAME OVER presentation
-> visually refined state-$24-derived effect
-> existing terminal Game Over state
-> R starts a fresh game

The arcade itself gates state $24 behind the score-qualifying/high-score route.

Our unconditional presentation remains a PROJECT DECISION until leaderboard functionality exists.

Do not implement leaderboard qualification.

Do not implement:

- leaderboard;
- initials entry;
- top-player list;
- credits;
- coins;
- start-button front end;
- attract mode;
- two-player support.


PRESERVE ACCEPTED FEATURES

Do not change:

- Levels 1-7 gameplay;
- 2.5 rim calibration;
- --rim-speed diagnostic;
- Super Zapper gameplay/colors;
- Blaster firing;
- player shots;
- enemy firing;
- below-rim patterns;
- Level 7 2-3-2-2-2 route;
- spinner;
- ordinary death animation;
- approaches/exits;
- scoring;
- audio;
- geometry unrelated to the presentation.


TESTING

Update focused tests to protect the refined model without encoding incidental pixels from the photographs.

Protect established mechanics such as:

- final death -> presentation -> effect -> terminal state;
- 16-record capacity if still supported by ROM;
- deterministic seeded behavior;
- record creation and drain;
- no gameplay mutation;
- restart only in terminal state;
- non-final death unaffected.

Add tests for newly established procedural visual structure where reasonable, such as:

- per-record visible primitive count/type;
- color diversity/selection rule;
- movement/distribution properties;
- any newly decoded graphic geometry;
- no runtime dependency on image files.

Do not add screenshot-diff tests against photographed frames.


AUTOMATED VALIDATION

Run focused:

- Game Over;
- death;
- rendering/vector effect;
- relevant visual primitive tests.

Run neighboring regression covering:

- Levels 1-7;
- player death;
- progression;
- campaign soak;
- Super Zapper;
- rim pursuit;
- restart.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual counts.

Build must have 0 errors.


DOCUMENTATION

Update existing experimental Game Over notes/spec/report only enough to distinguish:

ROM-BACKED mechanics;
ARCADE-OBSERVED visual behavior from the 99 frames;
PROJECT DECISION / APPROXIMATION;
remaining UNKNOWN details.

Do not mark this presentation accepted yet.

Neal must perform another manual arcade comparison.


AI LOGS

Save paired records:

AI-Logs/Prompts/2026-11-05-game-over-vector-visual-refinement-prompt.md

AI-Logs/Reports/2026-11-05-game-over-vector-visual-refinement-report.md

The report must include:

1. Starting Git state.
2. Reference-frame inventory and ordering.
3. Reflection/artifact exclusion confirmation.
4. Temporal visual observations.
5. Spatial/geometry observations.
6. Color observations.
7. Density/lifecycle observations.
8. ROM-to-frame reconciliation.
9. Interpretation of graphic $1F / $47 / $4A if improved.
10. What the diamond approximation got wrong.
11. Exact refined procedural representation.
12. Evidence classifications.
13. Remaining unknowns.
14. Production/test/documentation files changed.
15. Validation results.
16. Manual comparison checklist.
17. Final Git state.


NO GAME LAUNCH

Do not launch interactively.

Do not generate a video of the C# result.

Stop after automated validation.


MANUAL COMPARISON

Provide:

dotnet run --project src\Tempest.Game -- --lives 1

Neal will compare the refined C# result against the original arcade.

Ask Neal only:

1. Does the bright vector effect now immediately resemble the original arcade presentation?

2. Is its characteristic spatial structure correct?

3. Is its motion approximately correct?

4. Is its density/build-up/dissipation approximately correct?

5. Are its color relationships broadly correct?

6. Does the transition into and out of the effect look right?

7. What remains visibly wrong?

Do not request more visual material unless this refinement still exposes a specific unresolved detail.


GIT

Do not stage.
Do not commit.
Do not push.

Finish with:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

HEAD must remain at the starting committed checkpoint.


FINAL RESPONSE

Return:

1. Confirmation that all 99 frames were inventoried and analyzed in chronological order.
2. Concise objective description of the observed bright vector effect without assigning it a real-world metaphor.
3. Reflection/non-game imagery exclusion.
4. ROM/frame reconciliation.
5. Graphic $1F / $47 / $4A conclusion.
6. What changed from the diamond approximation.
7. New procedural vector representation.
8. Color behavior.
9. Motion/distribution behavior.
10. Timing/lifecycle behavior.
11. Remaining UNKNOWN details.
12. Production files changed.
13. Test files changed.
14. Documentation files changed.
15. Focused test counts.
16. Neighboring regression counts.
17. Full-suite counts.
18. Build warnings/errors.
19. git diff --check result.
20. Manual launch command.
21. Concise manual comparison checklist.
22. Report filename.
23. Final Git status.
24. Confirmation the 99 reference frames were not modified/committed and are not runtime dependencies.
25. Confirmation no leaderboard/credits/attract/two-player/unrelated gameplay work, staging, commit, push or interactive launch occurred.

Stop for Neal's manual visual comparison.

TEMPEST_GAME_OVER_VECTOR_VISUAL_REFINEMENT_COMPLETE