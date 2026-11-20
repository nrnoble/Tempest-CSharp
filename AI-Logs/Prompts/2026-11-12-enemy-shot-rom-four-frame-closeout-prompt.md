# Close out accepted ROM-backed four-frame enemy-shot rendering

## Task

Close out the manually accepted ROM-backed four-frame enemy-shot rendering
milestone. This is a closeout only: preserve the candidate exactly as Neal
manually tested it. Do not redesign, retune, or otherwise change the accepted
rendering unless a genuine validation or documentation defect requires it.

The implementation investigation and candidate report are:

- `AI-Logs/Reports/2026-11-12-enemy-shot-rom-vector-rendering-recovery.md`
- `AI-Logs/Reports/2026-11-12-enemy-shot-rom-four-frame-implementation.md`

Read those reports as needed after the required starting safety checks.

## Starting safety and expected state

Before substantive repository work, inspect branch, HEAD, `origin/master`,
staged changes, tracked worktree changes, non-ignored untracked files, and diff
summary using bounded commands. Expected base:

- branch `master`;
- HEAD `cd678572c009d27fa8e1e1f0059ec45f6db5db11`;
- commit subject `Complete accepted Level 9 behavior`;
- `origin/master` at the same baseline before the candidate.

The expected candidate remains in the worktree, including the new rendering
source and source/test/documentation changes. Treat those changes as expected.
If other unexpected tracked changes or unrelated non-ignored untracked files
cannot be accounted for, stop and report the discrepancy. Never reset, restore,
clean, checkout, stash, rebase, merge, amend, discard, or overwrite work. Do
not read `TEMP/myNotes.md` or modify local ROM/reference/user assets.

## Accepted manual result

Neal manually tested the candidate and reported:

- the new enemy shots look pretty good;
- nothing about them is distracting;
- they do not make the game more difficult to play;
- arcade shots may possibly appear somewhat larger, but this comparison is
  unclear because the overall arcade presentation on screen is also larger.

Record this as manual visual/playability acceptance of the candidate. Do not
promote the possible size difference to an established arcade/C# discrepancy,
use it to change enemy-shot size, or claim pixel-identical or proven exact-size
arcade fidelity. Preserve the candidate exactly as tested. Any broader screen
or presentation-scale comparison belongs to the separately deferred whole-game
presentation refinement.

## Preserve the implementation model and gameplay

Keep the evidence distinctions explicit:

**ROM-BACKED:** four distinct enemy-shot vector pictures; global
timer-based/modulo-four selection; four disconnected visible segments and four
point-like marks per picture; blank beam movements are not visible lines; and
the verified logical vector geometry and deliberate asymmetries.

**PROJECT TRANSLATION:** one shared deterministic C# simulation phase; one
enemy-shot frame per four `RunOneStep` calls; unresolved exact arcade real-time
animation frequency; logical Y-up to screen Y-down conversion; current
normalization/scale; one-pixel point rasterization; and current white
presentation as the accepted project constraint. Do not describe these
translations as ROM-proven facts.

Do not alter enemy-shot creation, speed, trajectory, firing, collision,
expiration/lifecycle, scoring, sound, enemy or level behavior,
difficulty/timing, projection/tube gameplay geometry, player shots,
Blaster-shot rendering, or recording/playback schema or input behavior. Do not
remove or repurpose `EnemyBullet.Rotation` because visual frame selection no
longer uses it.

## Image/frame-analysis boundary

No images or frames are provided, and Neal's manual acceptance is the visual
evidence for this closeout. Do not search for or enumerate images/videos,
open screenshots, inspect MAME or C# frames/contact sheets, extract frames,
batch-process images, run pixel/color/component/bounding-box analysis, or
search `TEMP/`, `Videos/`, `UserData/Screenshots/`, or other asset directories.
Do not use broad image/video extension searches. If more visual evidence seems
necessary, stop and report what is missing.

## Validation evidence and closeout checks

Neal already ran this against the final candidate after the earlier Windows
Application Control block cleared normally:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --blame-hang-timeout 2m
```

The `Tempest.Game` and `Tempest.Game.Tests` projects succeeded, xUnit
discovery completed, and all tests ran to completion: 1341 total, 1341
succeeded, 0 failed, 0 skipped; test duration 2.9 seconds and overall
command/build duration 3.4 seconds. The Blame collector warned, "All tests
finished running, Sequence file will not be generated." This is not a test
failure. The previously blocked test assembly loaded in this final run.
Record the earlier Application Control failure as an intermittent block that
did not prevent final validation; do not claim a root cause for the different
outcomes.

Do not rerun the full suite merely to duplicate Neal's result unless a
closeout edit changes production code or tests. Review the final candidate
diff; verify no unintended gameplay or unrelated changes; make durable
documentation accurately record acceptance and validation; run a solution
build if closeout edits warrant it; run `git diff --check`; inspect staged and
unstaged state; and confirm the expected milestone files. If production code
or tests are changed during closeout, rerun appropriate automated validation.
Do not launch the game or perform manual acceptance on Neal's behalf.

## Documentation and reporting

Update `TEMPEST-DEVELOPMENT-NOTES.md` and, where appropriate,
`TEMPEST-BEHAVIOR-SPEC.md`, narrowly recording implementation and manual
acceptance; explicit ROM-BACKED versus PROJECT TRANSLATION distinctions; the
shared phase as a project translation rather than a proven arcade cadence;
accepted white color; unchanged gameplay; Neal's visual/playability result;
the uncertain size observation and its whole-game presentation context; no
size retuning; the final 1341/1341 result; and the earlier Application Control
block clearing without security weakening. Do not broaden documentation into
unrelated deferred work.

Do not rewrite historical AI reports as though their earlier state never
existed. The implementation report remains accurate that its final test run
was blocked at that time. Create a new ignored closeout report under
`AI-Logs/Reports/` and preserve this prompt under `AI-Logs/Prompts/`, using
paired descriptive date-prefixed `2026-11-12` filenames. The report must
include starting Git state; relationship to the recovery and implementation
reports; manual acceptance and treatment of the uncertain size observation;
Neal-supplied final validation and historical security-block context; final
build/diff validation; durable documentation changes; committed files, commit
hash/message, push result, and final Git/worktree state; confirmation no
unauthorized image/frame analysis occurred; and remaining non-blocking
deferred items.

## Commit and push authorization

Neal explicitly authorizes committing and pushing this accepted milestone.
After checks pass, stage only intended tracked/new project files belonging to
the milestone. Do not stage `AI-Logs/`, `TEMP/`, user screenshots, recordings,
videos, ROM/reference copies, `myNotes.md`, or unrelated local assets. Review
the staged file list and diff, confirm no unintended files, commit with a
concise descriptive message, push `master` to configured `origin`, and verify
local HEAD and `origin/master` after push as well as final tracked worktree and
index state. Do not force-push. If push fails, do not rewrite history or do
unrelated Git surgery; report the exact failure and preserve the local commit.

Treat Application Control, Smart App Control, Code Integrity, and similar
blocks as security/infrastructure results. Never weaken or bypass security,
retry the same blocked artifact repeatedly, substitute stale binaries, or
claim blocked validation passed. If blocked again, stop and report the exact
command, artifact, error, completed work, and remaining work. Bound potentially
long commands and stop stalled work rather than waiting indefinitely.

## Required final response

Report the closeout result; manual acceptance; automated validation; durable
documentation; files committed; commit hash/message; push result; final
branch/HEAD/origin state; final worktree/index state; remaining
non-blocking questions; closeout report path; and confirmation that no
unauthorized images or frames were searched for or analyzed. If all succeeds,
state clearly that the ROM-backed four-frame enemy-shot rendering milestone
is CLOSED and no further enemy-shot size/rendering change is currently
requested.

TEMPEST_ENEMY_SHOT_ROM_FOUR_FRAME_CLOSEOUT_20261112
