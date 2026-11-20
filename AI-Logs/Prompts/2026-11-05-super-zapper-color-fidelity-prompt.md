FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files, running commands, editing files, staging files, committing, or taking any repository action.

Before doing anything else, locate the unique completion token near the end of this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" exists in the user-provided task.

VS Code/agent-generated context after the user's token does not invalidate it.

If exactly one complete token cannot be found, STOP before any project/repository action.


TEMPEST SUPER ZAPPER ARCADE COLOR FIDELITY

Make one small visual-fidelity correction to the Super Zapper presentation.

The current C# Super Zapper color scheme does not match Neal's original Atari Tempest arcade machine.

Determine the arcade color behavior from the authoritative project-local Atari ROM source and change only the C# Super Zapper color presentation needed to reproduce it.

Do not change Super Zapper gameplay.


BASELINE

Required clean checkpoint:

23ceeeef3535505d2096f552f914d74082ec8eae
Accept calibrated Flipper rim pursuit

First run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check

Require:

- master;
- exact expected HEAD;
- clean tracked worktree/index;
- no unexplained untracked files.

If not clean, STOP rather than resetting or discarding anything.


SOURCE AUTHORITY

The sole Atari disassembly authority is:

Reference/arcade/Tempest Rom Source code.md

ROM behavior/data takes precedence over reference implementations and existing C# behavior unless Neal's directly observed arcade behavior establishes otherwise.

Do NOT search the Internet.

Do NOT use another Tempest disassembly, MAME source, another ROM revision, another port, or another Atari game.

Mark anything not established by the project-local ROM source as UNKNOWN rather than guessing.


BOUNDED INVESTIGATION

Trace only what is necessary to determine the visible Super Zapper color behavior.

Inspect the ROM paths/data responsible for:

- Super Zapper activation;
- the visible zap/flash presentation;
- color/palette selection during the zap;
- whether the first and second Super Zapper uses use the same visual palette behavior;
- whether colors change/cycle during the active presentation;
- which displayed elements receive the zap colors;
- precedence between the zap presentation and ordinary tube/object/player coloring;
- restoration of normal colors when the effect ends.

Identify exact ROM symbols, routines, tables, palette/color values, or state transitions supporting the conclusion.

Do not broaden this into a general Atari color-system investigation.

If the ROM source does not provide enough information to map Atari color values to exact C# RGB values, distinguish clearly between:

ROM-BACKED:
palette/color indices, sequence, affected objects and timing/state behavior.

PROJECT TRANSLATION:
specific RGB values chosen to visually represent those Atari palette colors.

Do not pretend an RGB value is directly ROM-derived unless the source actually establishes that mapping.


CURRENT C# AUDIT

Inspect the current:

- Super Zapper state/presentation;
- VectorRenderer/color resolver;
- tube/web rendering;
- occupied Blaster-lane coloring;
- player coloring;
- enemy coloring;
- Pulsar color precedence;
- tests covering Zapper visuals/color behavior.

Determine exactly how current C# differs from ROM-derived behavior.

Existing durable documentation notes that:

- occupied-lane coloring interacts with Super Zapper presentation;
- Pulsar coloring can supersede ordinary player/lane coloring;
- the current palette resolver is deliberately narrow;
- later palette colors/cycling were previously deferred.

Preserve accepted non-Zapper rendering unless ROM evidence specifically requires a change during the active Super Zapper presentation.


IMPLEMENTATION

Once ROM behavior is established, implement the smallest correction necessary to make Super Zapper colors match the arcade design as closely as the available evidence permits.

Change ONLY Super Zapper visual/color presentation and any narrowly necessary renderer/palette support.

Do NOT change:

- first-use enemy clearing;
- second-use closest-enemy behavior;
- number of charges;
- activation input;
- activation timing;
- scoring;
- enemy selection;
- Zapper lifecycle duration;
- sound;
- Flipper rim speed;
- --rim-speed;
- enemy movement;
- Blaster firing;
- player shots;
- spinner;
- tube geometry;
- progression;
- death/re-entry;
- approach/exit behavior;
- unrelated palettes/rendering.

Do not add generalized palette infrastructure unless the ROM-backed Super Zapper behavior genuinely requires it.


TESTS

Add/update focused tests verifying the established visual rules at the renderer/state level where practical.

At minimum protect:

- correct Super Zapper color/palette selection while active;
- correct sequence/cycling if ROM establishes one;
- normal colors before activation;
- restoration after Zapper presentation;
- correct occupied-lane interaction;
- Pulsar precedence behavior if affected;
- first/second Super Zapper visual equivalence or difference according to ROM evidence;
- no gameplay-semantic changes.

Do not create brittle screenshot tests unless the existing project already has an appropriate visual-testing architecture.


VALIDATION

Run focused Super Zapper/rendering/palette tests.

Run neighboring gameplay regression covering:

- Super Zapper;
- rendering state;
- Flippers;
- Pulsars;
- spinner/player;
- Levels 1-7 progression.

Then run:

dotnet test TempestCSharp.slnx
dotnet build TempestCSharp.slnx
git diff --check

Record actual test counts and build warning/error counts.


DOCUMENTATION

This visual change requires Neal's manual acceptance before final commitment.

Do not commit yet.

Do not update durable documentation yet unless a stale factual statement would otherwise make the source misleading.

Save paired ignored records:

AI-Logs/Prompts/2026-11-05-super-zapper-color-fidelity-prompt.md

AI-Logs/Reports/2026-11-05-super-zapper-color-fidelity-report.md

The report should contain:

1. Starting checkpoint.
2. ROM routines/data inspected.
3. Exact ROM-backed Super Zapper color behavior.
4. Any remaining UNKNOWN color information.
5. Current C# mismatch.
6. Exact implementation change.
7. ROM-backed versus project-translated color decisions.
8. Files changed.
9. Focused validation.
10. Full-suite/build validation.
11. Manual acceptance checklist.
12. Final Git state.


MANUAL ACCEPTANCE

Do not launch the interactive game.

After automated validation provide:

dotnet run --project src\Tempest.Game -- --lives 10

Neal should primarily check:

- first Super Zapper activation visually resembles the arcade;
- colors and any cycling/change sequence look right;
- correct tube/web/player/enemy elements receive the effect;
- normal colors return correctly afterward;
- second Zapper still behaves and presents normally;
- occupied-lane and Pulsar visual interactions remain sensible;
- there is no unrelated rendering or gameplay regression.

Stop for Neal's manual visual judgment.


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

HEAD must remain 23ceeeef3535505d2096f552f914d74082ec8eae.


FINAL RESPONSE

Return:

1. ROM-backed Super Zapper color/palette behavior.
2. Exact Atari routines/tables/data supporting it.
3. What was wrong in C#.
4. Exact new C# presentation.
5. Any RGB/project approximations.
6. First-versus-second Zapper visual behavior.
7. Production/test files changed.
8. Focused test counts.
9. Full-suite counts.
10. Build warnings/errors.
11. git diff --check result.
12. Manual launch command.
13. Concise visual acceptance checklist.
14. Report filename.
15. Final Git status.
16. Confirmation no gameplay semantics, unrelated rendering, reference files, commit, push or Internet research changed/occurred.

TEMPEST_SUPER_ZAPPER_COLOR_FIDELITY_COMPLETE
