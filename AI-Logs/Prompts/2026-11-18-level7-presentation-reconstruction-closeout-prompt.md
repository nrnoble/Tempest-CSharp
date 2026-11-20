READ THIS ENTIRE USER-PROVIDED TASK BEFORE TAKING ANY ACTION.

FAIL-CLOSED COMPLETION-TOKEN CHECK

Before using tools, reading files, running commands, or taking repository
action:

1. Locate the unique completion token near the end of this task.
2. Verify that exactly one complete token beginning with "TEMPEST_" appears on
   a line by itself.
3. Automatically appended VS Code/agent context after the token does not
   invalidate it.
4. This check detects an incomplete or truncated task, not appended agent
   context.
5. If exactly one complete token cannot be found, STOP before repository action
   and explain why.

TASK

Close out the manually accepted clean Level 7 Atari-presentation
reconstruction.

Record Neal's manual acceptance, preserve the reconstructed shared
presentation architecture, update durable documentation, perform final
validation, stage only this milestone, commit it, push master to the existing
upstream, and verify the final repository state.

Do not perform new Level 7 presentation development.

Do not begin the separate whole-game viewport-proportion milestone.

MANUAL ACCEPTANCE

Record Neal's authoritative manual result:

- The severe Level 7 Atari-presentation bug is resolved.
- Level 7 now plays as expected.
- Enemy movement from deep to near appears normal.
- Player-shot movement from near to deep appears normal.
- Rim movement appears normal.
- Tube and gameplay entities remain registered.
- The reconstructed tube structure is acceptable.
- Level 7 geometry does not need further correction.
- No further Level-7-specific presentation work is wanted for this milestone.

Neal also compared C# and MAME/arcade-reference captures side-by-side.

The comparison indicates:

- the reconstructed C# tube and reference tube have the same recognizable
  Level 7 structure;
- there is no accepted evidence of incorrect point order, connectivity, lane
  structure, or rail attachment in the reconstructed Level 7 tube;
- the C# capture appears horizontally broader relative to its height;
- that remaining difference is classified as a separate whole-game viewport /
  screenshot-proportion issue;
- do not modify Level 7 tube geometry to compensate for the broader viewport
  presentation.

Do not claim the C# and reference captures are pixel-identical.

Do not claim the production Level 7 geometry is a literal conversion of Atari
record 06.

LEVEL 7 ACCEPTANCE BOUNDARY

Level 7 acceptance covers:

- the clean shared presentation architecture;
- correct gameplay presentation;
- acceptable Level 7 tube structure;
- entity/tube registration;
- depth motion;
- rim motion;
- Flipper transitions;
- exit and Approach compatibility;
- no observed Level-7-specific regression.

Level 7 acceptance does not close or solve the future whole-game viewport
proportion milestone.

FUTURE VIEWPORT MILESTONE

Record a clearly separate deferred presentation item:

C# viewport/screenshot proportions should later be investigated against the
arcade/MAME comparison presentation so that side-by-side screenshots can be
judged directly without mentally compensating for different horizontal and
vertical proportions.

That future milestone concerns, as applicable:

- logical portrait/playfield dimensions;
- horizontal-versus-vertical presentation scale;
- viewport aspect;
- gameplay-area proportions below the HUD;
- host/window mapping;
- screenshot-output proportions;
- cross-level presentation consistency.

It must be treated as a whole-game presentation-envelope issue.

Do not begin that investigation here.

Do not change:

- viewport dimensions;
- window dimensions;
- X/Y scale;
- camera zoom;
- perspective;
- HUD;
- tube geometry;
- per-level placements.

Do not add provisional viewport constants during this closeout.

CONTROLLING IMPLEMENTATION

Use:

AI-Logs/Reports/2026-11-18-level7-presentation-reconstruction-implementation-report.md

Preserve the implementation described there.

The accepted Level 7 architecture is:

- LevelConfiguration.Level7Production remains authoritative.
- LevelLoader.LoadLevel7Clover() remains the single Level 7 geometry.
- The exact 16 production points and closed 16-lane topology remain unchanged.
- Tube and gameplay entities use the same production geometry.
- Tube and entities use the shared AtariPresentationAdapter route.
- The adapter Jacobian is the shared local-scale authority.
- One final presentation translation is applied consistently.
- The common portrait camera is applied once.
- Rendering remains state-read-only.
- No active ordinary Level 7 path uses record-06 endpoint interpolation.

The accepted Level 7 candidate placement is:

Level7VerticalPlacement = 228.99702f

Horizontal correction remains zero.

Preserve this accepted value exactly unless final validation proves that the
current source differs from the controlling implementation report.

Do not retune or round it.

PRESERVE GAMEPLAY GEOMETRY

Do not change:

- any production Level 7 point;
- point order;
- CenterY;
- YOffset;
- Position;
- Depth;
- topology;
- playable lane count;
- initial lane;
- collisions;
- movement;
- Tanker behavior;
- ordinary Flipper behavior;
- cargo Flipper behavior;
- directed transition routes;
- Spikers;
- Spikes;
- Eggs;
- shots;
- wave supply;
- completion;
- Super Zapper;
- score;
- lives;
- timing;
- RNG;
- recording/playback.

Manual presentation acceptance is not authorization to change gameplay.

PRESERVE RAW RECORD-06 EVIDENCE

Retain verbatim:

- AtariTubeRecordCatalog.Level7Record06;
- record-06 raw X/Y bytes;
- record metadata;
- point order;
- closed topology;
- Level 7 RomPresentationMetadataCatalog entry;
- retained raw-data/provenance tests.

Record 06 remains ROM/Atari-backed evidence but does not drive the ordinary
Level 7 visible tube.

Do not delete record-06 evidence during closeout.

Do not reactivate the record-06 prototype.

FROZEN RECONSTRUCTED PATH

Preserve the removal of the failed active two-geometry architecture.

Do not restore or recreate:

- AtariLevel7Presentation active runtime path;
- AtariLevel7EntityPresentation;
- record-06 tube overlay;
- record-lane near/far entity interpolation;
- separate Level 7 lane-width scale;
- special mapped-Flipper presentation;
- screenshot-calibrated gains;
- screenshot-derived far-set translation;
- old record-06 centering;
- prototype viewport mapping;
- factor-128 presentation experiments.

Do not leave stale comments or documentation describing those components as
the current Level 7 design.

FLIPPER TRANSITIONS

Preserve the reconstructed normal Flipper presentation:

- gameplay-selected DirectedLaneTransition;
- production Level 7 geometry;
- normal CalculateFlipperPivotEndpoints path;
- shared adapter projection;
- long/reflex route semantics;
- ordinary and cargo Flippers on the same presentation route.

Do not restore the old mapped-record hinge implementation.

SHARED ACCEPTED SYSTEMS

Preserve all accepted shared presentation work:

- Blaster outward rim orientation;
- current Blaster color/intensity;
- removed brightness diagnostic;
- Blaster ExitZoom/Approach scale correction;
- ExitZoom focal compensation;
- closed-tube mapped far-rim focal rule;
- open-tube mapped rail-convergence rule;
- accepted Levels 1-6 presentation;
- Level 8 +212.28139;
- Level 9 +180.96825;
- diagnostic Level 10 +50;
- Approach and transition behavior;
- recording/playback determinism.

Do not change these systems during Level 7 closeout.

LEVEL 7 DIAGNOSTIC CLEANUP

The reconstruction plan retained these diagnostics through manual acceptance:

- --level7-visible-runtime-diagnostic
- --level7-framebuffer-diagnostic

Manual acceptance is now complete.

Inspect their current consumers and continuing purpose.

If these diagnostics are dedicated only to investigating the now-resolved old
Level 7 presentation architecture and have no continuing general value:

- remove the Level-7-specific diagnostic CLI options;
- remove DebugController and Program wiring;
- remove renderer hooks used only by those diagnostics;
- remove their dedicated source files and tests;
- remove stale help/usage text;
- ensure the removed switches follow ordinary unsupported-option behavior;
- preserve all AI-Logs reports and historical evidence.

If either diagnostic still has a clearly documented continuing general
development purpose independent of the removed architecture:

- retain it explicitly as DEVELOPMENT-ONLY;
- retain its parser/help/tests;
- report the exact continuing purpose.

Do not retain dead diagnostic code merely because it exists.

Do not remove generic playback, screenshot capture, pause, stepping, or normal
diagnostic infrastructure used outside this Level 7 investigation.

If diagnostic cleanup would unexpectedly touch unrelated shared tooling, STOP
that cleanup portion and report the boundary rather than broadening the task.

DURABLE DOCUMENTATION

Update the appropriate durable authorities:

- TEMPEST-DEVELOPMENT-NOTES.md
- TEMPEST-BEHAVIOR-SPEC.md

Record proportionally:

- clean Level 7 reconstruction completed;
- manual acceptance passed;
- severe Atari-presentation depth-motion bug resolved;
- Level 7 plays as expected;
- production Level 7 geometry is the shared gameplay/presentation authority;
- tube and entities share AtariPresentationAdapter;
- failed record-06 two-geometry runtime path was removed;
- raw record-06 data remains preserved as ROM/Atari evidence;
- Level7VerticalPlacement = 228.99702f is the accepted project translation;
- X correction is zero;
- gameplay and deterministic playback remained unchanged;
- no further Level-7-specific tube-geometry work is wanted;
- viewport/screenshot proportion matching is deferred as a separate whole-game
  milestone;
- broader appearance in side-by-side comparison must not be recorded as a
  Level 7 geometry defect.

Preserve evidence classifications.

The accepted production Level 7 clover remains REFERENCE-DERIVED / current
production authority unless existing durable documentation establishes a more
specific classification.

Do not relabel it as ROM-backed.

The placement value remains a PROJECT TRANSLATION.

NO NEW VISUAL ANALYSIS

Do not inspect or analyze screenshots, MAME captures, videos, frames, contact
sheets, or visual assets during closeout.

Neal has already performed manual comparison and acceptance.

Do not derive new values from the supplied comparison images.

REPOSITORY SAFETY

Before documentation edits, cleanup, validation, or staging, record:

- branch;
- HEAD;
- upstream/tracking;
- ahead/behind;
- index;
- tracked worktree;
- relevant untracked state.

Expected committed baseline beneath the candidate:

aee59b39c1d68be830d0014a7b9497d1bb40f3c7

on master tracking origin/master at 0/0.

Verify rather than assume.

Preserve unrelated local work, including if still present:

- .github/copilot-instructions.md;
- Reference/arcade/MBUCOD.V05.txt;
- Reference/arcade/MBUDOC.DOC.

Do not reset, restore, clean, stash, rebase, or switch branches.

If the accepted reconstruction cannot be cleanly separated from unrelated
changes, STOP with a precise BLOCKED/PARTIAL report.

FINAL VALIDATION

After documentation and any authorized diagnostic cleanup are complete, run:

1. Focused Level 7 shared-presentation tests:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests" --blame-hang-timeout 4m --verbosity minimal

2. Targeted Level 7 gameplay/presentation regressions:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Level7AtariPresentationTests|FullyQualifiedName~Level7ProductionTests|FullyQualifiedName~Level7AdmissionPolicyTests|FullyQualifiedName~Level7FlipperMovementTests|FullyQualifiedName~Level7SpikerFiringTests|FullyQualifiedName~GameplayRecordingTests|FullyQualifiedName~GameplayPlayback" --blame-hang-timeout 5m --verbosity minimal

3. Targeted shared/cross-level presentation regressions:

dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~AtariPresentationAdapterTests|FullyQualifiedName~VectorRendererAtariPresentationTests|FullyQualifiedName~Level8AtariPresentationTests|FullyQualifiedName~Level9AtariPresentationTests|FullyQualifiedName~DiagnosticLevel10Tests|FullyQualifiedName~BlasterPresentationTests|FullyQualifiedName~BlasterScaleRegressionTests|FullyQualifiedName~ExitZoomFocalCompensationTests|FullyQualifiedName~TransitionPresentationTests|FullyQualifiedName~DirectedRotationInvestigationTests" --blame-hang-timeout 5m --verbosity minimal

4. Full solution tests with explicit hang protection.

5. Release solution build.

6. git diff --check.

Use the actual current project paths and validated command syntax if they differ
slightly from the report.

Do not weaken tests.

Canceled, blocked, stalled, timed-out, or incomplete commands are not PASS.

Do not launch the game. Manual acceptance is complete.

FINAL VALIDATION MUST ESTABLISH

- Level 7 production geometry remains byte/value identical.
- Level 7 uses one shared tube/entity geometry.
- No active ordinary path calls removed special mappers.
- Entity anchors use the shared adapter.
- Adapter Jacobian remains the local-scale authority.
- Level 7 placement remains +228.99702 logical Y.
- Horizontal correction remains zero.
- Directed Flipper transitions remain correct.
- Gameplay state remains unchanged by rendering.
- Deterministic recording/playback remains unchanged.
- ExitZoom focal compensation remains unchanged.
- Blaster travel scaling remains unchanged.
- Levels 8/9/10 remain frozen.
- Option-off/legacy behavior remains unchanged.
- Removed prototype and retired diagnostic switches are not silently accepted.

STAGING

After all final validation passes:

1. Identify every surviving file belonging to the accepted Level 7
   reconstruction and authorized diagnostic cleanup.

2. Stage only those source/test files and the durable documentation files
   updated for closeout.

3. Do not automatically stage a file merely because the implementation report
   lists it.

4. Do not stage:
   - .github/copilot-instructions.md;
   - local reference files;
   - screenshots;
   - videos;
   - recordings;
   - TEMP;
   - UserData;
   - AI-Logs;
   - unrelated changes.

Before committing:

- inspect `git diff --cached --name-status`;
- inspect the complete staged diff;
- verify every staged hunk belongs to this milestone;
- run `git diff --cached --check`.

The staged diff may include file deletions belonging to obsolete Level 7
presentation/prototype/diagnostic code.

Verify every deletion has no remaining consumer.

AI-LOGS

Follow `.github/copilot-instructions.md`.

Save:

- this closeout prompt under `AI-Logs/Prompts/`;
- a paired Level 7 reconstruction closeout report under
  `AI-Logs/Reports/`.

Do not stage or commit AI-Logs.

Historical AI-Logs remain preserved.

COMMIT

If and only if:

- manual acceptance is recorded accurately;
- durable documentation is correct;
- authorized cleanup is complete or clearly deferred;
- focused validation passes;
- both targeted validation groups pass;
- full suite passes;
- Release build passes;
- unstaged whitespace check passes;
- staging contains only this milestone;
- staged whitespace check passes;

create one bounded commit with message:

Close out Level 7 Atari presentation reconstruction

Do not amend or rewrite existing history.

PUSH

Push master to the existing configured upstream.

Do not create or change remotes.

After push verify:

- branch remains master;
- upstream remains origin/master;
- HEAD is the new Level 7 closeout commit;
- ahead/behind is 0/0;
- index is clean;
- no accepted reconstruction changes remain unstaged;
- unrelated local work remains preserved.

If commit or push cannot complete safely, report PARTIAL/BLOCKED rather than
forcing completion.

EXPECTED CLOSEOUT REPORT

The paired report must include:

- exact manual acceptance statement;
- accepted Level 7 shared-presentation architecture;
- accepted +228.99702 logical Y placement;
- zero-X-placement confirmation;
- production geometry preservation;
- record-06 evidence preservation;
- exact obsolete components removed;
- exact diagnostics removed or retained and why;
- viewport-proportion issue recorded as a separate future whole-game
  milestone;
- durable documentation changes;
- final focused test command/count/result;
- targeted Level 7 command/count/result;
- targeted cross-level command/count/result;
- full-suite count/result;
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
- unrelated local work excluded;
- confirmation Levels 8/9/10 remain frozen;
- confirmation accepted Blaster and ExitZoom systems remain frozen;
- exact AI-Logs prompt/report paths.

Do not ask Neal questions during execution.

Do not pause for manual input.

Complete autonomously unless a genuine validation, cleanup, staging, safety,
commit, or push blocker requires fail-closed termination.

TEMPEST_LEVEL7_PRESENTATION_RECONSTRUCTION_CLOSEOUT