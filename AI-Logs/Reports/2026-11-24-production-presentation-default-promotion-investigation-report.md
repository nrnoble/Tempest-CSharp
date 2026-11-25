# Production presentation default promotion: architecture investigation

**Status: COMPLETE — source/test/documentation inspection; no implementation.**

## 1. Executive summary

The existing selection seam is sufficient to make the accepted Atari presentation
the default for interactive Tempest gameplay through the current Level 16
production boundary. The presentation implementation and per-level resolver
should be reused unchanged. The change is not just flipping a parser Boolean:
headless playback and the input diagnostic currently reject an enabled Atari
flag; perspective, calibration, file-mode argument validation, and legacy 4:3
selection also require deliberate handling.

**Recommended PROJECT DESIGN DECISION:** Model A, with a specifically named
development-only **old portrait projection** switch, independent of the existing
`--legacy-presentation` 4:3 switch. This preserves a concrete A/B and runtime
crash-isolation route and the existing `--portrait-perspective` diagnostic,
without leaving the older projection as normal gameplay. It is not required for
headless simulation, nor must it be permanent. Preserve
`--atari-presentation` temporarily as a documented compatibility alias for
interactive Atari selection, with explicit conflict validation. These are
recommendations, not accepted project decisions.

The evidence supports proceeding with a separately authorized bounded
implementation, followed by targeted tests and Neal's manual acceptance.
The source establishes the mapping and selected settings; it does not establish
new manual acceptance of the changed default launch.

## 2. Starting Git state

- Branch `master`; HEAD `03346581b0fa3253bbb010022e8c617ce414d466`
  (`Complete Level 16 production and presentation`); upstream `origin/master`,
  ahead 0, behind 0, based on local tracking refs; no fetch was performed.
- Index clean. Tracked worktree modifications were limited to
  `AI-Logs/Reports/2026-11-24-level15-production-promotion-closeout-report.md`
  and `TEMPEST-BEHAVIOR-SPEC.md`. The latter is the historical Level 15
  checkpoint wording hunk identified in the closeout, not a Level 16 change.
- Untracked state included the prior Level 15/16 prompts and reports,
  including the requested Level 16 final closeout report, and
  `Reference/arcade/MBUCOD.V05.txt`. These were preserved. Both requested
  investigation-log paths were absent initially.
- The closeout identifies the unrelated material and confirms Level 16 manual
  gameplay/presentation acceptance and the completed commit. No material
  mismatch prevented this investigation. The original worktree was not clean;
  the committed production checkpoint was the expected one.

## 3. Current selection flow and affected systems

`Program.cs` calls `DebugController.TryParseAtariPresentation`: omission
returns `false`, the one valueless `--atari-presentation` returns `true`, and
duplicates, a following value, `--legacy-presentation`, and
`--portrait-perspective` are rejected. Removed prototype flags are also
rejected. `TryValidateAtariPresentationContext` rejects **enabled** mode in
headless playback and `--input-diagnostic`. Independently, Program chooses
`ExperimentalPortrait` unless `--legacy-presentation` selects `Legacy`;
these are viewport modes, not the Atari Boolean.

Flow:

```
CLI -> atariPresentationEnabled bool -> VectorRenderer constructor
    -> active-level calibration + ProjectWorldPoint
    -> old Projection.Project OR AtariPresentationAdapter
    -> tube, gameplay-entity anchors/shape scales, transition/world visuals
CLI -> separate PresentationMode -> camera/scissor/portrait HUD OR 4:3 HUD
```

In the ordinary no-option launch, the portrait camera/viewport and portrait
Atari vector HUD are already selected, but the tube and world projection use
the old `Projection.Project`. With Atari enabled, `ProjectWorldPoint` selects
the adapter, current-level settings and ExitZoom focal translation. Tube,
spikes, shots, enemies including Flipper endpoint/pivot projection, Blaster
edge, height and outward orientation, local glyph scales, and score-popup
rendering are affected. Approach uses the shared world mapping with approach
offsets; ExitZoom uses the adapter plus focal compensation and final vertical
offset. The Level 16 shared-center Flipper model calculation remains in the
level/game model; its results are projected by the selected renderer.

HUD choice is mostly **viewport-mode**, not the Atari Boolean:
`DrawPortraitOverlays`/`DrawHud` render `AtariHudVector` for portrait even
when the Atari projection flag is absent; the 4:3 mode uses its legacy text
HUD. HUD override options are gated by the Atari flag, however. Background,
viewport border, playback overlay and guides have their own presentation
paths. Blaster orientation and some artwork branches directly use the Atari
Boolean; other artwork branches use `UsesAtariPresentationForCurrentLevel`.

The Boolean is passed to the renderer, not to `TempestGame`, level startup,
progression, or input replay. No production gameplay/levels/systems reference
the renderer, projection or Atari adapter in the inspected search. It changes
visible output, not simulation geometry, collision or progression; rendering
exceptions can of course interrupt an interactive session.

The flag does not configure window size or transparency. `Program.cs` sets
projection globals, configures background opacity before `InitWindow`, and
initializes the same window dimensions regardless of Atari selection. The
separate presentation mode chooses cameras/scissor. Input diagnostic creates
its own window without this renderer.

## 4. Old/default architecture and current role

The old projector is `VectorRenderer.ProjectWorldPointLegacy`, calling
`Projection.Project` with the level model, center/offset and approach depth.
It is the current default **portrait** world projection. The existing
`--legacy-presentation` additionally selects a 4:3 camera, different HUD and
overlays; it is not an exact switch back to today's ordinary portrait.
`VectorRenderer` itself defaults its constructor Boolean to false, which is a
separate API default from Program's runtime selection.

It is an actively exercised baseline, not proven obsolete: `DisplayConfigurationTests`
assert legacy projected tube coordinates, renderer tests compare Atari and old
projections, and perspective calibration targets the shared non-Atari portrait
projection. Some independent tests use default-constructed renderers for
render-only mechanics. README commands omit the flag in many ordinary
record/playback examples; its Atari-calibration section explicitly says
“With `--atari-presentation`”. The README's old “Windowed presentation
candidate” section also describes a 1280x960 candidate, conflicting with the
current 1600x1280 display configuration and later acceptance; it is historical
or stale wording, not evidence that the actual window is 1280x960.

There is no identified accepted gameplay simulation dependency on the old
projection. Its rendering still has concrete independent internal uses even
when Atari is on, described below. Simulation can instead be checked
headlessly, without any projection.

## 5. Accepted Atari architecture and Levels 1–16

`AtariPresentationCalibration.ForDisplayedLevel` has explicit rows 1–16;
`Resolve` chooses the current displayed level and applies each explicit
override independently and absolutely. Default rows `(Y, scale, offset,
depth-Y)` are:

| Levels | Defaults |
|---|---|
| 1–4 | `(1.4,.8,35,1)`, `(1,1.1,35,1)`, `(1.2,.97,35,1)`, `(1,1,35,1)` |
| 5–8 | `(1,1.2,15,1)`, `(1,1.28,15,1)`, `(1.3,.9,-25,1)`, `(1,1,-75,1)` |
| 9–12 | `(1.5,1.05,-150,1)`, `(1.2,.9,15,1)`, `(1.5,1.1,0,1)`, `(1.3,.8,0,1)` |
| 13–16 | `(1.2,1,0,.3)`, `(1,1.1,200,1)`, `(1.15,1,75,1)`, `(1.1,1,100,1)` |

`HasModelFrameMapping` covers 1–4 and 7–16; Levels 5 and 6 have explicit
`ProjectLevel5`/`ProjectLevel6` C#-domain projection and local scale paths.
Level 7 maps production geometry, not an external caller-selected prototype.
Callers simply select Atari: dispatch and calibration are internal.

Important exceptions: during Level 6 Game Over states,
`ProjectDiagnosticsOnLegacyPoint` deliberately applies selected Y/scale
diagnostics to the old projection, and `UsesAtariPresentationForCurrentLevel`
is false. Game Over vector records also explicitly use the old projector
because their raw coordinates are a different frame, at any level.
The general unestablished-mapping fallback for 2–6 logs a warning, but all
current production 2–4 mappings and 5/6 special dispatches cover normal
levels; it is not evidence that normal Level 5/6 gameplay silently falls back.
These internal rules remain part of the accepted path; do not remove the
legacy projector merely because runtime mode selection changes.

Support is established by dispatch, calibration and source/test inspection;
manual acceptance is a separate fact. The durable first-pass table records
manual selection/acceptance for Levels 1–12, Level 13 against MAME, separately
accepted Levels 15 and 16, and the Level 14 accepted closeout/calibration.
The Level 16 closeout explicitly records accepted gameplay, presentation and
`(1.1,1,100,1)`. README also records acceptance through Levels 13–16. These
are accepted project presentation decisions/translations, not ROM-derived
pixel dimensions or proof of a newly tested default launch. Activating the
same Atari path with no overrides selects the same per-level settings as the
accepted opt-in path, including internal exceptions. Manual acceptance of the
new default and transition workflow is still needed.

## 6. Complete option dependency audit

“Redundant” below means only the **literal flag prerequisite** becomes
redundant for an interactive Atari-default launch. Diagnostic-mode exclusions,
format validation and other prerequisites must not be silently removed.

| Option/group | Requires Atari today and why | Promotion effect and development status |
|---|---|---|
| `--calibration-guides` | Yes, literal argument check; fixed yellow portrait guides are Atari diagnostic | Drop literal prerequisite but require effective Atari mode; remove syntax only in Atari mode. Keep development-only; reject incompatible fallback/headless. |
| `--tube-y-gain`, `--tube-scale`, `--tube-vertical-offset`, `--tube-depth-y-gain` | Yes, parser enables only Atari; additionally each requires `--level`, `--start-level`, or `--playback-file`; values have independent finite/range rules | Literal flag redundant; retain effective-mode and existing selector constraints unless separately authorized. Omitted values remain per-level defaults; explicit absolute overrides keep semantics. Retain development-only. Depth-Y has no camera-origin effect on 1–6; Level 16 uses its model-Y displacement rule. |
| `--hud-score-x/y/scale`, `--hud-high-x/y/scale`, `--hud-level-x/y/scale`, `--hud-lives-x/y/scale/advance` | Yes for any explicit override; calibrates portrait vector HUD | Literal flag redundant; retain validation and effective-mode gating. Their omission leaves existing HUD defaults. Development-only. HUD itself is not exclusively gated on Atari flag. |
| `--portrait-perspective` | No; explicitly incompatible with Atari and with 4:3 `--legacy-presentation`. It controls shared non-Atari portrait projection, not Atari calibration | Must not start silently applying to Atari. Require explicit old-portrait diagnostic mode, or otherwise explicitly choose and document a migration; rejecting an existing unqualified invocation is a CLI behavior change. Development-only. |
| `--legacy-presentation` | No; conflicts with Atari and selects 4:3, not old portrait alone | Must still force Atari Boolean false if retained; preserve its current meaning rather than repurpose it. It is not accepted by current gameplay-file mode allowlist. Development/legacy comparison. |
| `--record-gameplay`, `--playback-file`, seeded gameplay playback | No; interactive Atari flag composes with file modes, file parser validates and consumes presentation arguments | Default affects visible rendering only; preserve existing recordings and parsing/allowlist. Keep recording/playback diagnostics available. Explicit `--atari-presentation` compatibility alias avoids breaking existing invocations. |
| `--level`, `--start-level`, scenario selectors | No; choose simulation startup, not presentation. Playback rejects direct startup selectors because metadata owns startup | Default syntax changes visible presentation, not startup/progression. Direct selectors and scenarios remain development diagnostics; test their compatibility with the chosen default. |
| F9/F10 live pause/capture; Space/Right/Left visible playback; playback screenshot diagnostic | No Atari prerequisite; captures current rendered frame or controls playback outside recorded input. `--screenshot-key-diagnostic` requires file playback | Automatically captures the selected presentation; semantics of image appearance change, not capture/input behavior. Keep the diagnostics; no screenshot artifacts were opened. |
| `--background-opacity` | No; global clear-alpha/window flag; rejected headlessly | No prerequisite to remove; preserve validation, global scope and default 1. Development-only. |
| `--headless-playback`, `--boundary`, `--json`; `--input-diagnostic` | Do not require Atari; explicitly enabled Atari is rejected before headless execution/input diagnostic | Do not set effective interactive Atari state before routing these modes or reject them merely due to a new default. Explicit rendering-only options remain rejected. Headless parser accepts only boundary/JSON after file. Development-only. |
| `--speed`, `--spinner-sensitivity`, `--game-over-density`, lives/rim-speed and level-specific Spiker controls | No direct Atari dependency; existing mode-specific parser restrictions remain | Do not alter their meanings. Speed/pacing and spinner affect input/run behavior as separately documented; density is visual. Development controls, not Atari toggles. |

`DebugController.TryParseGameplayFileCommand` has a separate option allowlist:
any new old-portrait switch must be recognized there for recording and visible
playback, rather than only in top-level parsing. `Program` separately invokes
the depth-Y parser even though the file parser's preliminary validation list
does not call it; preserve top-level validation when changing the CLI.

## 7. Recording and playback

`GameplayRecordingFile` version 2 writes format/version, seed, startup level,
lives, rim speed, relevant Spiker startup metadata, duration and input frames;
version 1 and 2 readers validate their respective startup/input formats.
Neither mode, perspective, calibration, guides nor background opacity is
recorded or checked as recording compatibility metadata. There is no
presentation fingerprint in the inspected source/tests; no claim is made about
external fingerprinting tools. `GameplayPlaybackSession`/headless runner
reconstruct simulation from recording startup and frames; visible playback
uses the game from that session with a separately constructed renderer.

Existing recordings should continue to load and produce the same deterministic
simulation when the implementation preserves startup and input handling.
Their default **visible output** will intentionally change to accepted Atari
presentation. Old command lines using `--atari-presentation` should still work
with a compatibility alias. Recordings never promised reproduction of the
previous visual presentation, and no format change is warranted. Regression
tests must establish simulation equivalence and recording compatibility after
the change; this investigation did not execute those tests.

## 8. Headless and test assumptions

Categories, with representative rather than exhaustive file lists:

1. **Accepted presentation:** `AtariPresentationCalibrationTests`,
   `AtariPresentationAdapterTests`, `VectorRendererAtariPresentationTests`,
   `Level7AtariPresentationTests`, `Level8AtariPresentationTests`,
   `Level9AtariPresentationTests`, `Level16AtariPresentationTests`,
   `ExitZoomFocalCompensationTests` and Blaster/Flipper projection tests.
   Preserve their explicit Atari setup, accepted numeric invariants and
   transition checks.
2. **Legacy/A-B:** `DisplayConfigurationTests` explicitly asserts the old
   projected tube coordinates; `VectorRendererAtariPresentationTests`
   compares legacy and Atari lines and tests Level 6 opt-in. Keep meaningful
   explicit old-mode tests, rather than accidentally relying on a changed
   constructor default. 4:3 viewport tests are a distinct category.
3. **Parser/CLI:** `DebugControllerTests`, `PortraitCalibrationGuideTests`,
   `TubeYGainTests`, `TubeDepthYGainTests`, `TubeVerticalOffsetTests`,
   `StartLevelTests`, `BackgroundOpacityOptionsTests` and recording file-mode
   tests cover combinations, explicit flag requirements and errors.
4. **Stale assumptions:** `AtariPresentationIsExplicitOptIn...`, no-flag
   calibration rejection, guide flag requirement, candidate terminology and
   `Level6CandidateIsOptIn...` when asserted as a runtime policy. Replace only
   the obsolete policy expectations; retain input validation and intentional
   incompatibilities. Tests using `new VectorRenderer(game)` implicitly mean
   old projection today: audit each if the constructor default changes.
5. **Simulation-only:** `HeadlessGameplayPlaybackTests`,
   `GameplayPlaybackSessionTests`, `GameplayRecordingTests` and gameplay
   lifecycle/progression tests should keep existing simulation expectations;
   add top-level routing coverage rather than changing the headless runner to
   render. Screenshot capture/key tests should preserve capture semantics.

The smallest seam may keep `VectorRenderer`'s constructor default false and
pass the correct interactive effective value explicitly from Program, avoiding
an unrelated API behavior change. If a later decision changes that constructor
default, many rendering tests must explicitly identify their intended mode.
Add a testable selection/routing seam: no-argument interactive startup selects
Atari; direct starts/progression and visible playback do too; old portrait and
4:3 switches select their intended distinct modes; headless/input diagnostics
still succeed without rendering; conflicts and per-level calibration remain
correct. Source-only inspection cannot prove process launch behavior after a
future change.

## 9. Legacy diagnostic value and model evaluation

Concrete value: current perspective tuning explicitly targets old portrait;
existing coordinate and comparison tests use it; switching the projection but
not the viewport can isolate adapter-specific rendering failures while keeping
the same simulation and camera. This is a plausible diagnostic method, not
proof that a particular prior crash would have been isolated by it. The
existing 4:3 switch changes more variables, so is a weaker controlled A/B.
Headless playback isolates simulation without *any* renderer, but cannot
isolate a render-path crash in an interactive scene. None requires old portrait
to remain the default.

- **A:** recommended: default Atari plus a separately named opt-in old-portrait
  diagnostic. Concrete perspective and A/B uses justify retaining it for now.
  Keep the existing 4:3 selection separate. Minor parser/allowlist/test cost.
- **B:** viable if Neal decides interactive comparison and perspective tuning
  are no longer useful. It is not necessary for promotion; removing runtime
  access is distinct from deleting the legacy projector, which remains used by
  Level 6 Game Over and vector-record rendering. Would remove or redesign an
  existing diagnostic, and change old command behavior.
- **C:** no identified architecture dependency requires opt-in. Only a
  demonstrated blocker or failed/manual validation would justify delaying
  promotion; retaining opt-in solely because old projection has uses is not
  justified.

**Proposed PROJECT DESIGN DECISION:** a descriptive name such as
`--baseline-portrait-presentation` for old portrait projection, not a final
approved CLI name. Make it mutually exclusive with explicit
`--atari-presentation`, 4:3 `--legacy-presentation`, and Atari-only
calibration/guides; permit `--portrait-perspective` only with this explicit
mode. Validate consistently for interactive recording/playback and do not
introduce it into headless. Preserve `--legacy-presentation` for its existing
4:3 behavior; assess its existing file-mode rejection explicitly rather than
silently broadening it.

## 10. Fate of `--atari-presentation`

**Proposed PROJECT DESIGN DECISION:** keep a valueless compatibility alias
for an interim transition, documented as redundant for interactive gameplay.
Preserve duplicate/value checks, explicit incompatibility with old portrait
and 4:3 modes, and rejection in non-rendering contexts. The alias denotes the
same effective selection rather than silently overriding a conflicting mode.
No removal date is asserted. Historical AI/manual commands and current README
commands remain usable; update current examples so new users need not type it.

Removing or rejecting it immediately breaks saved invocations without a
technical benefit; giving it a new behavior is misleading. Eventual removal
can be a separately authorized migration after usage is understood. Avoid a
warning on every normal launch: if deprecation warnings are desired, apply
them only when the alias is explicitly supplied and decide whether that noise
is worthwhile.

## 11. Wording and implementation scope

**Current facts:** `Program.cs` prints “DEVELOPMENT ONLY — Atari presentation
candidate enabled for production Levels 1-16”; its comments and
`DebugController` XML describe opt-in/development-only candidate behavior.
`--help` describes a development-only calibration control and its current
flag/selector prerequisites. README examples and calibration text require or
include the flag. README's old window-candidate section has independently
stale dimensions/acceptance wording. Durable docs contain both current
accepted status and historical candidate checkpoints; the historical
checkpoint text should not be rewritten as though it never existed.

**Proposed bounded implementation:**

1. Resolve effective interactive presentation at a single selection seam:
   default Atari portrait, explicit old portrait diagnostic, and existing 4:3
   mode. Keep explicit option presence distinct from effective mode, so
   headless/input-diagnostic defaults are not treated as explicit rendering
   flags. Resolve conflict and `--portrait-perspective` behavior deliberately.
2. Pass the selected Boolean and existing `PresentationMode` to renderer/draw.
   Preserve existing Game Over, focal, HUD and level-specific branches; do not
   rewrite adapter, geometry, calibration, model or window initialization.
3. Replace literal Atari flag prerequisites with effective-mode checks for
   guides, HUD and four tube options; retain value checks and separate
   level/start/playback prerequisites. Reject incompatible non-Atari modes
   explicitly. Wire any new switch through file-mode allowlists.
4. Preserve recording format, simulation state, visible playback controls,
   transparent window setup and headless-only option rules.
5. Update only affected parser/selection tests; add no-flag interactive default,
   alias/conflict, all diagnostic modes, visible playback/recording and
   headless/input routing tests. Keep accepted projection and simulation tests
   intact and add mode-specific A/B coverage.
6. Revise current startup/help, README launch and calibration instructions,
   user-visible candidate terminology, and current durable behavior notes to
   reflect the new accepted default and explicitly identify diagnostics and
   compatibility alias. Mark historical sections as historical where needed;
   retain provenance. Update durable acceptance status only at authorized
   closeout after Neal's manual acceptance.

Risks/stop conditions: accidental headless rejection; ambiguous 4:3 versus
old-portrait semantics; perspective option silently ignored or applied to
Atari; bypassed per-level defaults or ignored absolute override; accidental
loss of Level 6/Game Over internal legacy use; unsupported scenario behavior;
file-mode allowlist mismatch; recording state/input changes; a failed test or
interactive crash. Stop and report rather than changing accepted geometry or
guessing about missing support. Manual acceptance should cover a normal
no-flag Level 1 launch, appropriate direct starts spanning special Level 5/6,
mapped levels including 13 and 16, transitions/progression and visible
playback, plus any selected diagnostics. Automated tests do not establish
visual fidelity.

## 12. Inspected resources and validation

- Level 16 final closeout report; `README.md`; relevant current and historical
  sections of `TEMPEST-BEHAVIOR-SPEC.md` and `TEMPEST-DEVELOPMENT-NOTES.md`.
- `src/Tempest.Game/Program.cs`, `Debug/DebugController.cs`,
  `Debug/GameplayRecordingFile.cs`, `Debug/HeadlessGameplayPlaybackCli.cs`,
  renderer `VectorRenderer.cs`, `AtariPresentationAdapter.cs`,
  `AtariPresentationCalibration.cs`; bounded searches of gameplay, level,
  systems and debug sources.
- Representative test inspection/search: `DebugControllerTests`,
  `DisplayConfigurationTests`, `VectorRendererAtariPresentationTests`,
  `TubeDepthYGainTests`, and the test groups identified above.
- Commands: `git status --branch --porcelain=v2 --untracked-files=normal`,
  `git log -1`, `git rev-parse` upstream, `git diff`/`diff --cached` and
  output-path existence checks; bounded source/test/document searches and
  file reads. No fetch, build, tests, game, screenshot, image or video was
  run/opened. **Runtime validation: NOT RUN** — not needed to establish the
  present architecture; future implementation requires targeted automated and
  manual validation.

Only the required paired prompt and this report were created. No production
code, tests, durable documentation or protected reference/user material was
changed. Nothing was staged, committed or pushed. Level 17 and later were
neither investigated nor implemented. This report is an investigation and
recommendation, not an implementation or manual acceptance.
