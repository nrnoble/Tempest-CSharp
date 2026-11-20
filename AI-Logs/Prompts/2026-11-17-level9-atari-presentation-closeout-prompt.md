READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading project files, running commands, or taking any
repository action:

1. Locate the unique completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears
   on a line by itself in the user-provided task.
3. VS Code/agent-generated reminders, tagged-file information, attachment
   metadata, or other system-generated context may appear after the token and
   do not invalidate it.
4. This check detects an incomplete/truncated user task, not appended agent
   context.
5. If exactly one complete TEMPEST_ token cannot be found, STOP before project
   or repository action and explain why.

TASK

Close out the manually accepted Level 9 --atari-presentation milestone.

Neal has completed manual testing and explicitly reports:

- all Level 9 manual tests passed;
- no regressions were observed;
- nothing more needs to be changed.

Record that manual acceptance, preserve the accepted Level 9 implementation
without further tuning, perform final closeout validation, update durable
documentation, commit the bounded Level 9 milestone, push it to the existing
upstream remote, and verify the final repository state.

Do not perform new Level 9 development.

CONTROLLING IMPLEMENTATION

The controlling implementation report is:

AI-Logs/Reports/2026-11-17-level9-atari-presentation-implementation-report.md

Preserve the candidate described there.

The accepted implementation uses:

- existing recovered Atari Level 9 geometry;
- displayed Level 9 / Atari shape row 13;
- 16 ordered vertices;
- open topology;
- 15 playable lanes;
- existing Level 9 ROM presentation metadata;
- existing model-frame mapping;
- existing shared tube/entity projection path;
- existing projection, perspective, scale, and camera architecture;
- one Level-9-specific final post-projection logical Y translation.

The manually accepted final placement is:

+180.96825 logical Y

No final X correction is required.

Do not retune, round, reinterpret, replace, or rederive the accepted Y value.

MANUAL ACCEPTANCE

Record Neal's manual acceptance as authoritative for this milestone.

The accepted manual result is:

- Level 9 tube presentation passed.
- Open topology passed.
- Horizontal placement passed.
- Vertical placement beneath the HUD passed.
- Blaster/rim registration passed.
- Enemy registration passed.
- Player/enemy shot registration passed.
- Along-tube motion passed.
- Rim motion passed.
- Gameplay passed.
- No regressions observed.
- No further Level 9 changes are wanted.

Do not add qualifications to Neal's manual acceptance that are not supported by
existing evidence.

Preserve existing evidence classifications for implementation provenance.

FREEZE LEVEL 9

After successful closeout, treat the accepted Level 9 Atari presentation as
frozen.

Do not reopen or retune:

- Level 9 recovered geometry;
- Atari shape-row selection;
- point order;
- open topology;
- lane count;
- projection;
- perspective;
- scale;
- horizontal placement;
- vertical placement;
- entity registration;
- motion presentation;
- gameplay timing;
- gameplay mechanics.

Future Level 9 changes require a separately demonstrated regression or an
explicitly authorized future milestone.

EVIDENCE CLASSIFICATION

Preserve evidence boundaries accurately.

The recovered Level 9 geometry and presentation metadata retain their existing
ROM/Rev. 2 evidence classifications.

The C# model-frame translation and final +180.96825 placement remain PROJECT
TRANSLATIONS, not Atari ROM constants or claims of hardware-equivalent
rendering.

Manual visual acceptance establishes project acceptance of the candidate; it
does not change provenance classifications.

DURABLE DOCUMENTATION

Update the appropriate durable project documentation.

At minimum inspect and update, where appropriate:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Record proportionally:

- Level 9 --atari-presentation manual acceptance passed;
- all required manual checks passed;
- no regressions were observed;
- nothing more needs changing;
- the accepted Level-9-specific final logical Y placement is +180.96825;
- no horizontal correction is required;
- the accepted placement is frozen;
- Level 9 retains its existing recovered geometry and coherent shared
  tube/entity projector;
- gameplay was unchanged by this presentation milestone.

Do not add unsupported Atari claims.

NO NEW INVESTIGATION OR DEVELOPMENT

Do not:

- search for better placement;
- retune scale/perspective;
- inspect screenshots or video;
- open raw ROMs;
- redo geometry analysis;
- refactor working presentation code;
- add unrelated tests;
- modify Level 9 gameplay;
- investigate other bugs.

This is closeout only.

LEVEL 8 PROTECTION

Level 8 is manually accepted, closed, committed and frozen.

Preserve exactly:

- existing Level 8 geometry/presentation path;
- accepted +212.28139 final logical Y placement;
- entity registration;
- gameplay;
- durable acceptance documentation.

Do not change Level 8.

LEVEL 10 PROTECTION

Preserve accepted Level 10 exactly, including its frozen +50 final Y placement.

Do not modify Level 10 implementation or documentation except where a shared
status statement genuinely needs to mention that Level 9 is now accepted.

LEVEL 7 STATUS

Do not modify or investigate Level 7 during this task.

Level 7 remains a separate future reconstruction/repair milestone.

Its unresolved presentation history must not block Level 9 closeout unless
final validation directly demonstrates that the Level 9 milestone itself
introduced a shared regression.

OTHER LEVEL PROTECTION

Do not alter accepted Levels 1-6 or other presentation behavior.

STARTING REPOSITORY SAFETY

Before documentation edits or staging, inspect and record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree modifications;
- relevant untracked files.

Expected clean committed baseline before the Level 9 candidate:

- branch master;
- HEAD 60b1e95f080ff9a3f0c78d7b000fd12e22cf0d4b;
- upstream origin/master;
- ahead/behind 0/0.

Expected Level 9 candidate files from the implementation report:

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs
- tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs

Known unrelated pre-existing local work may include:

- .github/copilot-instructions.md
- Reference/arcade/MBUCOD.V05.txt
- Reference/arcade/MBUDOC.DOC
- screenshot000.png

Verify current state rather than blindly assuming these lists remain exact.

Do not destroy or normalize unrelated work to match expected state.

If staging boundaries cannot be established safely, STOP with a precise
BLOCKED/PARTIAL report.

CLOSEOUT VALIDATION

After durable documentation is finalized, run final bounded validation.

At minimum:

1. Focused Level 9 presentation/production tests with explicit hang timeout.

2. Targeted presentation/regression tests covering:
   - Level 9 production/presentation;
   - Levels 1-10 Atari-presentation selection;
   - entity registration;
   - Level 8 frozen +212.28139 placement;
   - Level 10 frozen +50 placement;
   - recording/playback;
   - Approach/transition/exit;
   - camera/display;
   - option-off behavior.

3. Full solution test suite with explicit hang timeout.

4. Release solution build.

5. git diff --check.

Use the already validated filters from the Level 9 implementation where
appropriate.

Do not weaken tests.

Canceled, stalled, timed-out, blocked or incomplete commands are not PASS.

Do not launch the interactive game. Neal has already completed manual
acceptance.

STAGING

After all validation passes, stage ONLY the accepted Level 9 closeout.

Expected production/test files:

- src/Tempest.Game/Rendering/AtariPresentationAdapter.cs
- tests/Tempest.Game.Tests/Level9AtariPresentationTests.cs
- tests/Tempest.Game.Tests/AtariPresentationAdapterTests.cs

Also stage the durable documentation files actually updated:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Do not automatically stage .github/copilot-instructions.md.

If it remains the unrelated pre-existing modification, preserve it unstaged.

Do not stage:

- Reference/arcade/MBUCOD.V05.txt
- Reference/arcade/MBUDOC.DOC
- screenshot000.png
- AI-Logs/
- TEMP/
- UserData/
- recordings;
- screenshots;
- unrelated modified or untracked files.

Before commit:

- inspect git diff --cached --name-status;
- inspect the complete staged diff;
- verify every staged hunk belongs to Level 9 closeout;
- run git diff --cached --check.

If an expected file mixes inseparable unrelated edits, STOP rather than
committing them blindly.

AI-LOGS

Follow .github/copilot-instructions.md.

Save this substantive closeout prompt under AI-Logs/Prompts/.

Save a paired Level 9 closeout report under AI-Logs/Reports/.

AI-Logs remain ignored and must not be staged or committed.

A chat response is not a substitute for the report.

COMMIT

If and only if:

- Neal's manual acceptance is recorded;
- durable documentation is correctly updated;
- all required validation passes;
- staging contains only the Level 9 milestone;
- staged whitespace check passes;

create one bounded commit.

Use the commit message:

Close out Level 9 Atari presentation

Do not amend or rewrite existing commits.

PUSH

Push the new master commit to the existing configured upstream.

Do not add or change remotes.

After push verify:

- branch remains master;
- HEAD is the new Level 9 closeout commit;
- upstream remains origin/master;
- ahead/behind is 0/0;
- index is clean;
- no Level 9 closeout changes remain unstaged;
- unrelated pre-existing local work remains preserved.

If push fails, diagnose the failure without entering an unchanged retry loop.

A successful local commit with failed push must be reported as PARTIAL/BLOCKED,
not fully closed.

EXPECTED REPORT

The paired closeout report must include:

- exact manual acceptance statement;
- accepted Level 9 +180.96825 logical Y placement;
- frozen Level 9 status;
- evidence/provenance classification;
- durable documentation changes;
- exact validation commands/results;
- focused test count/results;
- targeted regression count/results;
- full-suite count/results;
- Release build result;
- git diff --check result;
- exact staged file list;
- staged-diff review result;
- git diff --cached --check result;
- commit hash;
- commit message;
- push result;
- upstream verification;
- final branch/HEAD/index/worktree state;
- unrelated local work intentionally excluded;
- confirmation Level 8 remained frozen;
- confirmation Level 10 remained frozen;
- confirmation Level 7 was not changed;
- confirmation no interactive game was launched;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously unless a genuine safety, validation, staging or push
blocker requires fail-closed termination.

TEMPEST_LEVEL9_ATARI_PRESENTATION_CLOSEOUT