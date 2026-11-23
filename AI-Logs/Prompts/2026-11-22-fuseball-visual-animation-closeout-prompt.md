Read this entire user-provided task before taking any action.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, files, commands, or taking repository action, locate the unique completion token near the end of this user-provided task.

Verify that exactly one complete token beginning with TEMPEST_ appears on a line by itself in the user-provided task.

VS Code/agent-generated reminders, tagged-file information, attachment metadata, or other system-generated context appearing after the user's token do not invalidate it.

If exactly one complete token cannot be found, stop before any project or repository action and explain why.

# Task: Close out, commit, and push the accepted Fuseball visual-animation enhancement

The Fuseball production visual-animation enhancement has passed Neal's manual acceptance.

This is a CLOSEOUT task.

Do not redesign, retune, or further investigate the Fuseball animation.

## Manual acceptance

Record Neal's final manual acceptance:

- The production Fuseball animation is near perfect.
- No further changes are needed.
- The subtle four-phase animation looks appropriate during normal gameplay.
- The Fuseball no longer appears like one rigid static vector shape merely translating through the tube.
- The current visual cadence is acceptable.
- Fuseball movement and gameplay remain normal.
- No additional geometry, color, phase-order, or cadence tuning is wanted.

Freeze the current implementation.

## Accepted implementation

Preserve exactly the current production animation behavior:

- four existing geometry phases;
- five existing color groups;
- existing 25-line vector submission structure;
- existing modulo-16 visual counter;
- existing divide-by-four phase selection;
- deterministic phase sequence;
- deterministic initialization;
- normal production Fuseballs participate in the visual clock;
- FuseballTest remains functional;
- no double-advance in the development scenario;
- simulation-tied rather than independent wall-clock advancement.

The precise cadence remains classified as a **PROJECT TRANSLATION**.

Do not claim that the current cadence is proven frame-exact Atari behavior.

The tracked Atari evidence establishes four Fuseball vector-picture records and a changing vector presentation, but the exact original arcade animation clock remains unresolved.

Neal's manual acceptance establishes that the current project translation is good enough and should be frozen.

## Preserve Fuseball gameplay

Do not change:

- normal Fuseball movement;
- Climbing/Descending/Lateral behavior;
- terminal-rimward behavior;
- pending-supply terminal trigger;
- Hurry-Up;
- collision;
- hitbox;
- identity/lifecycle;
- death/re-entry;
- shot destruction;
- Super Zapper destruction;
- scoring;
- admission;
- wave accounting;
- all-rim completion.

This closeout is visual only.

## Preserve presentation

Do not change:

- Level 11 presentation;
- tube calibration;
- model-frame mapping;
- portrait projection;
- viewport;
- HUD;
- Approach;
- ExitZoom.

Legacy and Atari presentation must continue using the same underlying Fuseball animation phase.

## Durable documentation

Update the appropriate durable project documentation to record:

- production Fuseballs now animate through the existing four-phase vector model;
- the tracked Atari disassembly establishes four Fuseball vector-picture records;
- normal production previously remained on phase zero because the counter advanced only in the development FuseballTest path;
- the correction enables the existing visual clock during normal active production play;
- geometry and colors were not redesigned;
- the current modulo-16/divide-by-four cadence is an accepted PROJECT TRANSLATION rather than a claim of exact arcade timing;
- Neal manually judged the result near perfect and wants no further Fuseball visual-animation changes.

Keep historical/evidence classifications accurate.

Do not convert project translation into ROM-backed fact.

## Repository inspection

Before final validation or staging:

1. Verify branch, HEAD, upstream, and ahead/behind state.
2. Inspect the index.
3. Inspect tracked and untracked worktree changes.
4. Identify exactly the Fuseball visual-animation source/test changes.
5. Identify the related investigation, implementation, and closeout AI-Logs.
6. Identify the necessary durable documentation update.
7. Distinguish unrelated material.

Do not assume every changed file belongs to this milestone.

Preserve protected/local material.

Do not inspect or stage ROM contents, UserData, Temp, Videos, screenshots, or other unrelated local assets.

## Final validation

Run a fresh bounded gate from the exact manually accepted Fuseball-animation candidate.

At minimum run:

1. Production Fuseball animation tests.
2. Existing Fuseball rendering/vector tests.
3. FuseballTest/Hurry-Up regressions.
4. Terminal Fuseball movement regressions.
5. Direct Fuseball identity/lifecycle regressions.
6. Level 11 Fuseball/gameplay regressions.
7. Relevant legacy/Atari presentation regressions.
8. Playback/determinism regressions.
9. Complete solution test suite.
10. Release solution build.
11. git diff --check.

Use established explicit hang timeouts.

Do not weaken Windows security.

Do not enter same-artifact retry loops.

Do not launch the interactive game.

Manual acceptance has already passed.

If a genuine regression appears, stop before staging and report it.

## Stage only this milestone

After validation passes:

1. Stage the Fuseball visual-animation source change.
2. Stage its tests.
3. Stage the durable documentation update.
4. Stage the relevant tracked AI-Logs records for:
- the read-only visual-animation investigation;
- production visual-animation implementation;
- this closeout.
5. Include a clearly pending historical-report update only if it naturally belongs in this regular commit.
6. Do not manufacture unrelated bookkeeping.
7. Inspect the staged inventory.
8. Inspect staged source/text diffs as appropriate.
9. Run git diff --cached --check.
10. Verify no unrelated or protected material entered the index.

If staged-file ownership is ambiguous, stop before committing and report it.

## Commit and push

If and only if validation and staged inspection pass:

Commit to the current master branch with:

Animate production Fuseballs

Push normally to:

origin/master

Do not force-push.

Do not create another branch.

Do not amend or rewrite previous commits.

Do not reset, restore, stash, clean, rebase, or otherwise alter unrelated work.

After pushing verify:

- local master and origin/master resolve to the same new commit;
- ahead/behind is 0/0;
- index is empty;
- protected/local material was not published.

## Final closeout report

Save this prompt and a paired final report using the tracked AI-Logs convention.

The report must record:

- starting HEAD;
- final commit SHA;
- commit message;
- push result;
- focused validation;
- full-suite result;
- Release-build result;
- whitespace checks;
- exact files committed;
- Neal's manual acceptance;
- accepted four-phase visual behavior;
- cadence evidence classification;
- unchanged gameplay behavior;
- final Git/index/worktree state.

Do not enter a commit/amend/report loop merely to place the resulting SHA into the same commit.

If the post-push report is updated after the commit, allow that update to ride with the next normal project commit.

Do not begin Level 12.

TEMPEST_FUSEBALL_VISUAL_ANIMATION_CLOSEOUT
