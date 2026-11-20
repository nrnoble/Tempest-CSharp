FAIL-CLOSED COMPLETENESS CHECK

Read this entire user-provided task before using any tools, reading files,
running commands, modifying files, staging, committing, pushing, or taking any
repository action.

Before doing anything else, locate the unique completion token near the end of
this user-provided task.

Verify exactly one complete standalone token beginning with "TEMPEST_" appears
on a line by itself in the user-provided task.

VS Code/agent-generated reminders, attachment information, tagged-file context,
or other system-generated information after the user's completion token do not
invalidate it.

If exactly one complete standalone TEMPEST_ token cannot be found, STOP before
any project or repository action.


TEMPEST AVOID SPIKES VECTOR TEXT ACCEPTANCE CLOSEOUT

Close out the manually accepted AVOID SPIKES vector-text presentation milestone.

Neal has completed visual testing and ACCEPTS the presentation.

Neal tested the warning on all currently implemented Spike levels and reports:

"Looks perfect."

No further visual testing or tuning is needed.

This task is limited to:

1. recording manual acceptance;
2. adding the uppercase presentation rule to durable documentation;
3. rerunning final automated validation;
4. reviewing the complete accumulated diff;
5. staging only this milestone;
6. committing it;
7. pushing the accepted checkpoint to the existing private GitHub remote.

Do not begin Level 8 or Level 9 work.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED STARTING BASELINE

Expected branch:

master

Expected committed HEAD and origin/master:

b290c0a17ff635a6ddeb40dfb5b2dc4bc0aa5d2b
Fix reconstructed Level 5 cargo direction

Expected worktree contains only the uncommitted AVOID SPIKES vector-text
presentation candidate and its tests/documentation.

Expected candidate files from the implementation report:

src/Tempest.Game/Rendering/VectorFont.cs

src/Tempest.Game/Rendering/VectorRenderer.cs

tests/Tempest.Game.Tests/Level4CandidateTests.cs

tests/Tempest.Game.Tests/VectorFontTests.cs

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

The new VectorFontTests.cs may currently be an unstaged/untracked production
test file.

AI-Logs are intentionally ignored.

Before modifying anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --cached --stat
git ls-files --others --exclude-standard
git diff --check
git remote -v

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting, discarding or overwriting anything.


READ FIRST

Read:

.github/copilot-instructions.md

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

AI-Logs/Reports/2026-11-06-avoid-spikes-vector-text-report.md

Inspect current:

VectorFont.cs

VectorRenderer.cs

Level4CandidateTests.cs

VectorFontTests.cs

the complete accumulated diff

and relevant GAME OVER/Avoid-Spikes rendering tests.

Do not assume implementation details solely from the report.


MANUAL ACCEPTANCE

Record:

AVOID SPIKES VECTOR TEXT:
MANUALLY ACCEPTED.

Neal visually tested the new presentation on all currently implemented levels
that contain the AVOID SPIKES presentation.

Neal reports:

"Looks perfect."

The accepted presentation is:

AVOID SPIKES

in uppercase, using the shared Atari/Tempest-style vector glyph rendering used
by GAME OVER.

Neal accepts:

- vector lettering style;
- readability;
- apparent size;
- existing placement/centering;
- existing white color;
- warning timing;
- tube-exit presentation.

No further testing or visual tuning is requested.

Do not claim pixel/vector-perfect arcade reproduction.


ACCEPTED IMPLEMENTATION

Preserve the implementation candidate:

- AVOID SPIKES uses the shared VectorFont production pathway.
- GAME OVER and AVOID SPIKES share the same vector alphabet/rendering system.
- No independent AVOID SPIKES font/alphabet exists.
- The warning is exactly uppercase:

  AVOID SPIKES

- Existing AVOID SPIKES apparent height remains approximately 56 logical
  pixels.
- Existing visual center remains approximately logical (400,288).
- Warning color remains white.
- Avoid-Spikes warning/state/timing behavior remains unchanged.
- GAME OVER remains visually unchanged.

Do not retune these values during closeout.


VECTOR GLYPHS

The implementation report records that the existing shared vector alphabet
already contained:

A
V
O
E

and added only the glyphs required by AVOID SPIKES that were missing:

I
D
S
P
K

Preserve those shared glyph definitions if final source review confirms this.

Do not alter previously accepted GAME OVER glyph geometry.

Do not create another vector alphabet.


UPPERCASE ARCADE PRESENTATION RULE

Add an explicit durable presentation guideline:

The original arcade Tempest presentation strictly uses UPPERCASE lettering for
its in-game text.

Uppercase lettering is part of the game's visual style.

Therefore, going forward:

- new arcade-style in-game Tempest messages should use UPPERCASE lettering;
- new arcade-style in-game messages should normally use the established shared
  Tempest vector-text rendering system;
- do not introduce conventional system/default-font text for new arcade-style
  gameplay messages when the shared vector system is appropriate.

Classify the original arcade uppercase convention as ARCADE OBSERVED if that is
consistent with the project's evidence policy.

Classify use of the shared C# VectorFont for future messages as a PROJECT
PRESENTATION DECISION.


DO NOT PERFORM A GLOBAL TEXT CONVERSION

This milestone changes only AVOID SPIKES.

Do NOT convert existing unrelated text merely because the uppercase/vector
guideline is being recorded.

Do not modify:

- HUD;
- score;
- lives;
- development diagnostics;
- debug labels;
- existing approach presentation;
- SUPERZAPPER RECHARGE;
- terminal/console output;
- other already accepted presentation.

Future presentation work can apply the guideline when separately authorized.


GAME OVER MUST REMAIN FROZEN

Preserve accepted GAME OVER exactly.

Do not change:

- glyph shapes;
- scale;
- center;
- color;
- timing;
- GAME OVER state;
- tube suppression;
- celebration effect;
- four-burst presentation;
- density behavior.

The implementation report records GAME OVER as 44 vector segments with
204-by-28 logical bounds at scale 4.

If current tests/source confirm those values, retain their regression coverage.

Do not alter GAME OVER solely to make the shared helper cleaner.


NO GAMEPLAY CHANGES

Do not modify:

- Levels 1–7 gameplay;
- Level geometry;
- Level 5 $53 behavior;
- cargo-direction crash fix;
- Flipper movement;
- Tankers;
- Spikes;
- enemy supply;
- firing;
- Super Zapper;
- spinner;
- rim speed;
- score;
- death/re-entry;
- progression;
- direct-level startup;
- recording/playback;
- input controls;
- Game Over behavior;
- audio.


DOCUMENTATION

Reconcile durable documentation so that:

AVOID SPIKES VECTOR TEXT:
MANUALLY ACCEPTED.

Replace candidate/pending wording referring specifically to this milestone.

Record:

- old rendering used Raylib/default font rendering;
- accepted rendering uses shared VectorFont;
- text is uppercase AVOID SPIKES;
- size is intentionally preserved;
- centering/placement is intentionally preserved;
- color is preserved;
- warning timing is preserved;
- all implemented Spike levels were visually tested;
- Neal reports the result looks perfect;
- GAME OVER remains unchanged;
- future arcade-style messages should use uppercase shared vector lettering;
- no global conversion of already accepted unrelated text is authorized.

Do not rewrite historical AI reports merely because manual acceptance is now
complete.


AI LOGS

Preserve this complete task as:

AI-Logs/Prompts/2026-11-06-avoid-spikes-vector-text-acceptance-closeout-prompt.md

Create:

AI-Logs/Reports/2026-11-06-avoid-spikes-vector-text-acceptance-closeout-report.md

The report must include:

1. Starting Git state.
2. Manual acceptance.
3. Old rendering mechanism.
4. Accepted shared VectorFont mechanism.
5. Uppercase text.
6. Shared glyph additions.
7. Size preservation.
8. Centering preservation.
9. Color preservation.
10. Timing preservation.
11. GAME OVER preservation.
12. Uppercase arcade-style guideline.
13. Future shared-vector-text guideline.
14. Confirmation no global text conversion occurred.
15. Production files changed.
16. Test files changed.
17. Documentation files changed.
18. Focused test counts.
19. Neighboring regression counts.
20. Full-suite test counts.
21. Build warnings/errors.
22. git diff --check result.
23. Staged file list.
24. staged git diff --check result.
25. Commit message/hash.
26. Push result.
27. Final Git status.
28. origin/master verification.
29. Confirmation no Level 8/9 or unrelated gameplay work occurred.


FINAL AUTOMATED VALIDATION

Neal has explicitly stated that no further manual testing is needed.

Do NOT launch the interactive game.

Rerun focused automated coverage for:

- VectorFont;
- AVOID SPIKES vector geometry;
- AVOID SPIKES visibility/presentation;
- GAME OVER vector text;
- warning timing/state gates.

Run neighboring presentation regression covering:

- Avoid-Spikes exit presentation;
- transition presentation;
- GAME OVER text/presentation;
- Game Over density;
- rendering tests relevant to the shared vector text pathway.

Then run:

dotnet test TempestCSharp.slnx

dotnet build TempestCSharp.slnx

git diff --check

Tests must actually execute.

Require:

- 0 failed;
- 0 unexpected skipped;
- build 0 errors;
- report warning count accurately;
- git diff --check passes.

Record actual counts instead of assuming the candidate counts remain unchanged.


FINAL DIFF REVIEW

Before staging run:

git status --short --branch
git rev-parse HEAD
git diff --stat
git diff --name-status
git diff
git diff --check
git ls-files --others --exclude-standard

Verify the entire accumulated diff contains only:

- AVOID SPIKES vector rendering;
- required shared missing glyphs;
- focused rendering tests;
- durable presentation documentation.

Verify there is no:

- gameplay change;
- Level 8 work;
- Level 9 work;
- Game Over visual change;
- unrelated font conversion;
- global uppercase rewrite;
- recording/playback change;
- input change;
- generated recording;
- TEMP content;
- Videos content;
- bin/obj output;
- reference modification;
- Git configuration change.


STAGING

After validation and review pass, stage ONLY the accepted vector-text milestone.

Expected tracked paths should be reconciled against the actual diff, but are
expected to include:

src/Tempest.Game/Rendering/VectorFont.cs

src/Tempest.Game/Rendering/VectorRenderer.cs

tests/Tempest.Game.Tests/Level4CandidateTests.cs

tests/Tempest.Game.Tests/VectorFontTests.cs

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Do not stage AI-Logs.

Do not stage anything under TEMP/ or Temp/.

Then run:

git status --short
git diff --cached --stat
git diff --cached --name-status
git diff --cached
git diff --cached --check
git diff --stat

Require:

- bounded staged patch;
- no unintended deletion;
- no unrelated file;
- no remaining unstaged tracked change requiring resolution;
- staged diff check passes.


COMMIT

If staged review is correct, commit with:

git commit -m "Use vector text for Avoid Spikes"

Do not amend prior commits.

Use the project's existing co-author policy if required by current instructions.


POST-COMMIT CHECK

Run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

Require clean tracked worktree/index before push.


PUSH

Push normally:

git push

Do not force-push.

Do not modify remote configuration.

Do not create a branch.

After push run:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master

Require local master and origin/master to identify the same accepted commit.


FINAL RESPONSE

Return:

1. Manual acceptance status.
2. Exact accepted text.
3. Old rendering mechanism.
4. New rendering mechanism.
5. New glyphs added.
6. Size result.
7. Centering result.
8. Color result.
9. Timing result.
10. GAME OVER regression status.
11. Uppercase arcade presentation guideline.
12. Future shared-vector-text guideline.
13. Confirmation no global text conversion occurred.
14. Production files committed.
15. Test files committed.
16. Documentation files committed.
17. Focused test counts.
18. Neighboring regression counts.
19. Full-suite counts.
20. Build warnings/errors.
21. git diff --check result.
22. staged diff --check result.
23. Commit message.
24. New commit hash.
25. Push result.
26. Final Git status.
27. origin/master verification.
28. Closeout report filename.
29. Confirmation no Level 8/9, unrelated gameplay, recording/playback, Game
    Over visual, force-push, or remote-reconfiguration work occurred.

Stop after successful push.

TEMPEST_AVOID_SPIKES_VECTOR_TEXT_ACCEPTANCE_CLOSEOUT_COMPLETE