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


TEMPEST AVOID SPIKES VECTOR TEXT PRESENTATION

Make one tightly bounded visual presentation correction.

The current "AVOID SPIKES" warning uses an ugly/inconsistent text presentation.

Change AVOID SPIKES so its lettering is rendered using the SAME accepted vector
text style/mechanism as the current GAME OVER presentation.

Do not redesign GAME OVER.

Do not create a second font system.

Do not alter Avoid-Spikes gameplay.


ACTIVE REPOSITORY

Work only in:

C:\Users\nrnob\source\repos\Tempest


EXPECTED BASELINE

Expected branch:

master

Expected HEAD and origin/master:

b290c0a17ff635a6ddeb40dfb5b2dc4bc0aa5d2b
Fix reconstructed Level 5 cargo direction

Expected:

- clean tracked worktree;
- clean index;
- master tracks origin/master;
- local master matches origin/master;
- no unexplained nonignored untracked files.

Before changing anything run:

git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git rev-parse origin/master
git diff --stat
git diff --cached --stat
git ls-files --others --exclude-standard

If repository state differs unexpectedly, STOP rather than resetting, cleaning,
restoring, deleting or discarding anything.


READ FIRST

Read:

.github/copilot-instructions.md

README.md

TEMPEST-DEVELOPMENT-NOTES.md

TEMPEST-BEHAVIOR-SPEC.md

Inspect current source for:

- GAME OVER rendering;
- VectorFont;
- VectorRenderer;
- AVOID SPIKES rendering;
- Avoid-Spikes state/presentation;
- existing text-rendering tests;
- Game Over presentation tests;
- Avoid-Spikes presentation tests.

Determine exactly how GAME OVER currently renders its accepted Atari-style
vector lettering.

Reuse that mechanism.

Do not infer the implementation from historical reports if current source
differs.


MANUAL REQUIREMENT

Neal considers the existing GAME OVER text presentation excellent.

AVOID SPIKES should use the same visual lettering style.

The intended result is:

AVOID SPIKES

drawn in the same Atari/Tempest-like vector lettering style as:

GAME OVER


SIZE

Preserve the CURRENT apparent AVOID SPIKES text size.

Do not simply copy GAME OVER's scale constant if doing so changes the apparent
size of AVOID SPIKES.

Instead:

- reuse GAME OVER's glyph/vector construction and rendering mechanism;
- choose the scale necessary to preserve the existing AVOID SPIKES warning size.

Do not make the warning larger or smaller merely because GAME OVER uses a
different scale.


POSITION / CENTERING

Preserve the existing AVOID SPIKES placement and centering.

Neal does not want positioning or centering tuned in this milestone.

Do not move the warning vertically or horizontally except for the minimal
measurement adjustment required to preserve the same existing visual center when
switching rendering mechanisms.

If vector text measurement differs from the old renderer, calculate its width
using the same vector-text metrics used by the GAME OVER mechanism and preserve
the existing center point.


TEXT CONTENT

Preserve exactly:

AVOID SPIKES

Do not:

- change wording;
- change capitalization;
- add punctuation;
- add flashing text;
- add outline/fill;
- add animation;
- add another message.


COLOR

Preserve the current accepted AVOID SPIKES warning color unless using the shared
GAME OVER vector-text path requires a mechanical color parameter.

Do not copy GAME OVER's color blindly if the current AVOID SPIKES warning uses a
different intentional color.

This task changes lettering style, not the established warning color.


TIMING

Do not change:

- when AVOID SPIKES appears;
- how long it appears;
- Avoid-Spikes warning timing;
- pretravel timing;
- exit timing;
- Spike avoidance movement;
- firing eligibility;
- reverse/retry;
- transition behavior.

This is rendering only.


VECTOR TEXT REUSE

Prefer one shared production vector-text pathway.

If GAME OVER already uses:

VectorFont

or an equivalent reusable helper, use that exact mechanism for AVOID SPIKES.

Do not duplicate GAME OVER glyph definitions.

Do not create:

AvoidSpikesVectorFont

or another independent alphabet.

Avoid two subtly different Tempest text systems.


MISSING GLYPHS

AVOID SPIKES requires these letters:

A
V
O
I
D
S
P
K
E

and a space.

Inspect whether every required glyph already exists in the accepted vector-font
implementation.

If every glyph exists:

do not alter glyph geometry.

If a required glyph is genuinely missing:

add ONLY the missing glyph(s), following the established GAME OVER vector-glyph
construction/style.

Do not retune already accepted GAME OVER glyphs.

Any newly required glyph should visually follow the existing vector alphabet,
not a system font.


FUTURE TEXT POLICY

Record a project presentation guideline for future user-visible arcade-style
game text:

Future in-game Tempest messages should normally use the established shared
Atari-style vector text mechanism rather than Raylib/system/default font text.

This applies to future gameplay/presentation messages where the Tempest vector
style is appropriate.

Important:

Do NOT blindly convert every existing text element in this milestone.

Do not convert:

- HUD;
- score;
- lives;
- diagnostic text;
- development-only labels;
- existing approach/recharge text;
- terminal/debug output;
- other currently accepted presentation.

This milestone changes ONLY AVOID SPIKES.

The durable guideline exists so future new presentation text starts with the
shared vector style rather than introducing another inconsistent font path.


NO GAMEPLAY CHANGES

Do not modify:

- Levels 1–7 gameplay;
- Level geometry;
- Level 5 $53 behavior;
- reconstructed cargo direction fix;
- Flipper movement;
- Tankers;
- Spikes;
- enemy supply;
- firing;
- Super Zapper;
- spinner;
- rim speed;
- scoring;
- death/re-entry;
- progression;
- Game Over behavior;
- recording/playback;
- direct-level startup;
- input controls;
- audio.


GAME OVER MUST REMAIN IDENTICAL

The accepted GAME OVER presentation is frozen.

Ensure this change does not alter:

- GAME OVER glyph shapes;
- GAME OVER scale;
- GAME OVER centering;
- GAME OVER timing;
- GAME OVER color;
- Game Over celebration;
- four-burst effect.

Shared refactoring is allowed only if GAME OVER renders identically afterward.

Prefer no GAME OVER production change at all if the existing vector-text
mechanism can simply be reused.


TESTS

Add or update only focused presentation tests necessary to establish:

1. AVOID SPIKES uses the shared vector-text mechanism.

2. Its text remains exactly "AVOID SPIKES".

3. Existing center/anchor positioning is preserved.

4. Existing apparent size/scale contract is preserved.

5. Existing warning color is preserved.

6. Existing warning visibility/timing is unchanged.

7. GAME OVER still uses the same accepted vector glyph definitions and
   presentation values.

8. Any newly added glyph has deterministic vector geometry.

Do not add brittle pixel-perfect tests unless the repository already uses that
style of rendering test.

Prefer geometry/command/text-layout assertions consistent with existing tests.


DOCUMENTATION

Update durable documentation narrowly to record:

- AVOID SPIKES now uses the same established vector-lettering style as
  GAME OVER;
- its previous size and centering are intentionally preserved;
- this is a presentation-only project correction;
- future new arcade-style gameplay messages should default to the established
  shared vector-text style;
- existing unrelated text is not being globally converted.

Classify this as a PROJECT PRESENTATION DECISION / MANUALLY PENDING candidate,
not a ROM-backed discovery.


AI LOGS

Preserve this complete prompt according to project policy.

Suggested files:

AI-Logs/Prompts/2026-11-06-avoid-spikes-vector-text-prompt.md

AI-Logs/Reports/2026-11-06-avoid-spikes-vector-text-report.md

The report should include:

1. Starting Git state.
2. Old AVOID SPIKES rendering path.
3. Existing GAME OVER rendering path.
4. Shared vector mechanism reused.
5. Glyphs required.
6. Any missing glyphs added.
7. Scale/size preservation.
8. Centering preservation.
9. Color preservation.
10. Timing preservation.
11. GAME OVER preservation.
12. Production files changed.
13. Tests changed.
14. Documentation changed.
15. Focused validation.
16. Full-suite validation.
17. Build warnings/errors.
18. git diff --check result.
19. Manual launch commands.
20. Manual acceptance checklist.
21. Final Git state.


AUTOMATED VALIDATION

Run focused tests for:

- VectorFont/shared glyph behavior;
- AVOID SPIKES rendering;
- Game Over text regression;
- Avoid-Spikes presentation/timing.

Then run a meaningful neighboring presentation regression.

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


NO INTERACTIVE GAME LAUNCH

Do not launch the interactive game.

Neal will perform visual acceptance.

Do not stage.
Do not commit.
Do not push.


MANUAL ACCEPTANCE COMMANDS

Because AVOID SPIKES occurs during tube exit, provide the fastest existing
production/development command for Neal to visually reach an Avoid-Spikes
presentation.

If no existing dedicated Avoid-Spikes diagnostic exists, do not create one just
for this milestone unless clearly necessary.

Also provide ordinary/direct-level commands as appropriate.


MANUAL ACCEPTANCE CHECKLIST

Ask Neal to verify:

1. AVOID SPIKES now visually uses the same lettering style as GAME OVER.

2. The warning remains approximately the same apparent size as before.

3. The warning remains centered in the same place.

4. The warning color remains correct.

5. Warning timing is unchanged.

6. The text is clearly readable as AVOID SPIKES.

7. GAME OVER still looks exactly as previously accepted.

8. No tube-exit or gameplay behavior changed.


FINAL GIT CHECK

Finish with:

git status --short --branch
git rev-parse HEAD
git rev-parse origin/master
git diff --stat
git diff --name-status
git diff
git diff --check
git diff --cached --stat
git ls-files --others --exclude-standard

HEAD must remain:

b290c0a17ff635a6ddeb40dfb5b2dc4bc0aa5d2b

Expected:

- implementation candidate unstaged;
- no commit;
- no push;
- no Level 8/9 work.


FINAL RESPONSE

Return:

1. Old AVOID SPIKES rendering method.
2. New shared rendering method.
3. Whether new glyphs were required.
4. Size preservation.
5. Centering preservation.
6. Color preservation.
7. Timing preservation.
8. GAME OVER regression status.
9. Future vector-text guideline.
10. Production files changed.
11. Tests changed.
12. Documentation changed.
13. Focused test counts.
14. Full-suite counts.
15. Build warnings/errors.
16. git diff --check result.
17. Manual launch command(s).
18. Manual acceptance checklist.
19. Report filename.
20. Final Git status.
21. Confirmation no staging, commit, push, Level 8/9 work or unrelated gameplay
    change occurred.

Stop for Neal's visual acceptance.

TEMPEST_AVOID_SPIKES_VECTOR_TEXT_COMPLETE