Complete the unfinished maintainability-documentation pass for the Tempest C# project.

This is a DOCUMENTATION-ONLY maintenance task.

Do not change gameplay, executable behavior, tests, algorithms, constants, method
signatures, identifiers, ordering, project configuration or reference files.

CURRENT VERIFIED STATE

The previous maintainability pass completed comments in the core gameplay files but
explicitly identified these remaining documentation areas:

- src/Tempest.Game/Rendering/VectorRenderer.cs
- src/Tempest.Game/Program.cs
- src/Tempest.Game/Debug/DebugController.cs
- src/Tempest.Game/Gameplay/Player.cs
- src/Tempest.Game/Gameplay/Shot.cs
- additional useful method-level intent in src/Tempest.Game/Levels/Level.cs
- limited organizational/explanatory comments in
  tests/Tempest.Game.Tests/SimulationTests.cs

The previous pass also updated:

- .github/copilot-instructions.md
- TEMPEST-DEVELOPMENT-NOTES.md

The current verified baseline is:

dotnet build: passed
dotnet test: 146 passed, 0 failed

The previous Agent verified that comment-stripped, whitespace-stripped C# was
identical before and after its pass.

Preserve that standard.

READ FIRST

Read:

1. .github/copilot-instructions.md
2. TEMPEST-DEVELOPMENT-NOTES.md
3. AI-Logs/Reports/2026-10-30-maintainability-comment-pass-report.md
4. Relevant current source files listed above

Do not reread large reference files unless necessary to prevent an inaccurate
comment.

Reference/incitti is read-only and must not be modified.

COMMENTING STANDARD

The purpose of comments is long-term maintainability.

A developer returning to this project years from now should be able to understand
important design intent without access to AI conversation history.

Prefer comments that explain:

- WHY behavior exists;
- WHY ordering matters;
- WHAT a non-obvious algorithm represents;
- WHAT coordinate/depth system is being used;
- WHICH behavior is arcade-derived, Incitti-derived, development-only or still
  approximate;
- WHAT invariant would be broken by an apparently harmless future change.

Do not add comments that merely translate obvious C# statements into English.

Bad example:

// Increment rotation.
Rotation += 15;

Useful example:

// Projection depth decreases as an object approaches the rim, so rendered vector
// size naturally increases without entity-specific scale tables.

Keep comments concise. Do not turn source files into essays.

VECTORRENDERER.CS

This is the highest-priority file in this pass.

Document the reasoning behind non-obvious rendering behavior, including where
present:

- the tube's near/far projection relationship;
- how entity height is converted into projection depth;
- player/blaster projection during normal play versus exit travel;
- why Flipper pivots must interpolate between source and destination lanes rather
  than jumping immediately;
- Egg projection behind/deeper than the visible web;
- Spiker spiral/vector construction;
- spike line and tip representation;
- Tanker line glyph projection and scale;
- enemy-bullet radial geometry;
- player-shot rendering where its geometry differs from EnemyBullet;
- which rendering helpers/callbacks exist to permit headless geometry testing;
- why rendering tests can prove finite/correct submissions but cannot prove native
  framebuffer or arcade visual fidelity.

Do not annotate every drawing statement or vector point.

Do not change any geometry, color, projection equation, scale, rotation, draw order
or rendering callback.

PROGRAM.CS

Add only enough comments to explain its role as the small composition root/main
loop.

Where useful, clarify:

- parsing/selection of development command-line modes;
- why normal Level 1 remains the default;
- why Tanker and Spiker development scenarios are isolated;
- how --enemy-bullets-test and --lives compose with scenarios;
- why Program should not accumulate gameplay simulation logic.

Do not change command-line behavior.

DEBUGCONTROLLER.CS

Comment the intent of the development switches where it would otherwise be unclear.

Distinguish:

- normal gameplay;
- --spiker-test;
- --tanker-test;
- --enemy-bullets-test;
- --lives.

Make clear that these switches are controlled development/testing tools and are not
the finished arcade level-selection/progression system.

Do not alter parsing or validation.

PLAYER.CS

Add concise comments only where necessary to retain understanding of:

- EdgeIndex / EdgePosition representation;
- wrapping around continuous tube geometry;
- depth during normal play and tube exit;
- reset behavior;
- configurable starting lives versus the normal default.

Do not comment trivial getters or setters.

SHOT.CS

Document only non-obvious projectile conventions, such as:

- what Height means relative to the tube/rim;
- travel direction;
- rotation if meaningful;
- why player shots differ from EnemyBullet behavior.

Do not change projectile behavior.

LEVEL.CS

The previous pass already added important comments.

Do not blanket-comment every method.

Review it specifically for still-undocumented non-obvious responsibilities such as:

- normal ten-unit conservation;
- Egg release scheduling;
- life-loss reconstruction;
- Tanker original-slot identity;
- Tanker cargo-unit identity;
- fresh-Egg reconstruction after death;
- development-only accounting versus normal Level 1 accounting;
- Level state transitions where Level owns them;
- geometry helper semantics such as previous/next edge and shortest-direction rules.

Add comments only where the existing code still requires reverse engineering.

Do not duplicate comments already added during the previous pass.

SIMULATIONTESTS.CS

Do not comment every test.

Add a small number of organizational comments or section headings where they
materially help a maintainer navigate the large test file.

Potential groups include:

- Tanker/cargo regression tests;
- Flipper collision/timing regressions;
- enemy-bullet tests;
- Spiker/spike tests;
- normal Level 1 accounting and exit regressions.

Add a comment for particularly non-obvious test setup only when future maintainers
could otherwise mistakenly simplify it and destroy the boundary condition being
tested.

Do not:

- rename tests;
- reorder tests;
- alter test setup;
- change assertions;
- remove duplicate-looking tests;
- add new tests;
- change the test count.

HISTORICAL TEST COUNTS

Review TEMPEST-DEVELOPMENT-NOTES.md only for the test-count wording changed by the
previous maintainability pass.

The prior Agent reported replacing historical references to "151 tests" with the
current value "146".

Determine from surrounding wording whether those references describe:

A. the historical state of a specific earlier milestone; or
B. the current test-suite state.

If a number describes a historical milestone, preserve the historical number and
make the wording explicit, for example:

"At the completion of the original Tanker implementation, 151 tests passed."

If the number describes the current project state, 146 is appropriate.

Do not rewrite historical facts merely to make every test count equal today's
suite.

If the historical intent cannot be determined confidently from the document, leave
the passage unchanged and report the ambiguity.

Do not alter gameplay documentation beyond this narrow test-count review.

SUSPECTED FLIPPER CONVERGENCE

Do not investigate or fix the suspected overlapping/converging rim-Flipper issue.

Ensure the existing development-note entry remains labeled:

SUSPECTED / NOT YET REPRODUCED

The user's arcade observation that original Atari Tempest does not show this
behavior may remain documented as an observation, not as proof of the current
implementation's root cause.

NO-BEHAVIOR-CHANGE REQUIREMENT

Permitted modifications:

- comments in the specified .cs files;
- organizational comments in SimulationTests.cs;
- narrow historical/current clarification in TEMPEST-DEVELOPMENT-NOTES.md;
- AI archive files requested below.

Do not modify:

- executable C# statements;
- expressions;
- conditions;
- constants;
- method/property signatures;
- identifiers;
- namespaces;
- collection types;
- control flow;
- gameplay state;
- test behavior;
- test assertions;
- package/project configuration;
- .github/copilot-instructions.md unless a clear error introduced by the previous
  documentation pass is discovered;
- README.md unless correcting an actual documentation error discovered during this
  review;
- Reference/incitti;
- levelset files.

Do not refactor or optimize.

If you discover a code defect, record it in the report. Do not fix it.

BASELINE

Before editing:

1. Run git status --short.
2. Run dotnet build.
3. Run dotnet test.

Required baseline:

146 total
146 passed
0 failed

If the baseline is not exactly green, stop and report the discrepancy before
editing.

PRESERVE PRE-PASS SNAPSHOTS

Before modifying the remaining .cs files, create temporary copies outside the
tracked source tree or otherwise preserve their exact pre-pass content for
comparison.

These snapshots are temporary validation artifacts only and must be removed before
finishing.

VERIFICATION

After commenting:

1. Run dotnet build.
2. Run dotnet test.
3. Run git diff --check.
4. Run git diff --stat.
5. Inspect the complete diff.

For every .cs file modified during THIS completion pass, compare its post-pass
content with its pre-pass snapshot after removing:

- comments;
- insignificant whitespace.

There must be zero executable-code differences.

If executable code changed accidentally, restore only that accidental change.

The final test count must remain exactly:

146 total
146 passed
0 failed

Remove all temporary source snapshots before finishing.

AI ARCHIVE

AI-Logs/ already exists.

Create these directories if necessary:

AI-Logs/Prompts/
AI-Logs/Reports/

Save this task specification to:

AI-Logs/Prompts/2026-10-30-maintainability-comment-completion-prompt.md

Preserve this prompt verbatim if available.

If exact verbatim preservation is unavailable, begin the archive with:

"Faithful task specification reconstructed by the VS Code Agent. The original
prompt was not archived verbatim during execution."

Save the final report to:

AI-Logs/Reports/2026-10-30-maintainability-comment-completion-report.md

FINAL REPORT

Include:

1. Baseline build/test result
2. Files reviewed
3. Files receiving comments
4. Rendering/projection concepts documented
5. Program/DebugController development-mode documentation
6. Player/Shot documentation
7. Level documentation completed
8. SimulationTests organization/comments added
9. Historical test-count review and any corrections/restorations
10. Suspected issues observed but deliberately not fixed
11. Verification that comment-stripped/whitespace-stripped C# is unchanged
12. Final build/test result
13. Exact final test count
14. git diff --check result
15. Temporary snapshot cleanup confirmation
16. Any documentation still genuinely worth addressing later

Do not include raw tool activity or internal reasoning.

TODO HANDLING

A todo list may be used.

Before finishing:

- reconcile all todos;
- identify anything deliberately deferred;
- leave nothing silently incomplete.

Do not commit or push.