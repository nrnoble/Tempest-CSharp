Implement C3b — direct graphical Level 3 mixed-wave access and manual-acceptance groundwork.

STARTING CHECKPOINT

233bf25 Document Level 3 mixed-wave architecture

C3a implementation checkpoint:

2b2e50c Implement headless Level 3 mixed-wave lifecycle

PRIMARY AUTHORITY

AI-Logs/Reports/2026-11-01-c3b-level3-mixed-wave-preimplementation-analysis-report.md

SUPPORTING AUTHORITY

- current committed source and tests;
- TEMPEST-DEVELOPMENT-NOTES.md, including the committed C3a Mermaid architecture/lifecycle diagrams;
- TEMPEST-BEHAVIOR-SPEC.md;
- AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-preimplementation-analysis-report.md;
- AI-Logs/Reports/2026-11-01-c3-level3-mixed-wave-implementation-report.md;
- existing CLI, renderer, HUD, input, replay/restart and development-scenario conventions.

Do not rely on earlier chat context that is not preserved in these files or this prompt.

AI WORK RECORD

Preserve the established Tempest AI-development record.

1. Save this exact implementation prompt under:

   AI-Logs/Prompts/

   Use the established date-based naming convention. Suggested filename:

   2026-11-01-c3b-level3-mixed-wave-implementation-prompt.md

2. Create or update the corresponding concise implementation report under:

   AI-Logs/Reports/

   Suggested filename:

   2026-11-01-c3b-level3-mixed-wave-implementation-report.md

3. Preserve the current VS Code Copilot chat as the working conversation. Do not attempt to replace the normal VS Code chat with another generated transcript.

4. Agent Debug logging is already enabled externally. Do not disable it or change its settings.

5. AI-Logs must remain ignored by Git unless existing project policy is explicitly changed in a separately authorized task.

6. The implementation report must preserve durable engineering information:

   - task scope and starting checkpoint;
   - files inspected;
   - major implementation decisions;
   - significant defects or unexpected findings;
   - corrections made;
   - focused and full validation results;
   - build result;
   - changed files;
   - manual acceptance still required;
   - remaining blockers;
   - relevant evidence classifications.

7. Keep the report concise. Do not turn it into a transcript of routine reads, searches, commands, tool calls or internal reasoning. Agent Debug logging and the VS Code chat provide the detailed execution record.

8. Do not delete, overwrite or discard any previous prompt, report or debug history.

9. Do not commit or push.

C3a AUTOMATED BOUNDARY IS ACCEPTED

C3a already passed its final automated headless gate:

- C3a/regression focused group: 209/209;
- configuration/Level 2/input group: 93/93;
- full solution: 520/520 passed, 0 failed, 0 skipped;
- build: 0 warnings and 0 errors;
- git diff --check passed.

Do not reopen, redesign or retune C3a unless C3b exposes a genuine production defect.

The two direct all-rim fixtures previously documented as production-unreachable remain accepted waivers with equivalent surrounding evidence. Do not manufacture these fixtures through reflection, inaccessible-state mutation, fake simulation or test-only production paths.

PURPOSE

Implement C3b only:

Expose the existing completed LevelConfiguration.Level3Mixed production-shaped Level 3 model graphically through direct DEVELOPMENT ONLY launch access so Neal can perform manual gameplay acceptance.

C3b is integration, access, rendering, HUD and manual-acceptance groundwork.

C3b is NOT a new Level 3 gameplay implementation.

1. DIRECT --level3-test ACCESS

Add:

--level3-test

Use existing project conventions for development-mode command-line parsing and conflict handling.

The flag must route to the existing:

LevelConfiguration.Level3Mixed

It must NOT route to Level3Plus.

It must NOT create:

- another Level 3 configuration;
- a fixed-composition Level 3 variant;
- another gameplay simulation;
- a test-only gameplay implementation.

Preserve --level3-geometry-test as the existing Flipper-only geometry diagnostic.

--level3-test must be mutually exclusive with incompatible development scenarios according to existing project conventions, including:

- --level2-test;
- --level3-geometry-test;
- --tanker-test;
- --spiker-test;
- --fuseball-test;
- --pulsar-test;
- any other exclusive gameplay scenario already enforced by DebugController or TempestGame.

Preserve composable general options where appropriate, including:

--lives

Preserve the existing optional:

--enemy-bullets-test

Do not broaden or redesign the general CLI architecture.

2. PROGRAM AND GAME ROUTING

Wire Program and the existing construction path so this command:

dotnet run --project src/Tempest.Game -- --level3-test --lives 10

starts the real production-shaped Level3Mixed C3a model.

Do not special-case C3a gameplay behavior based on the CLI flag after launch.

Production behavior must remain selected by the installed Level3Mixed configuration/composition.

Natural progression must remain disabled for this direct-development mode.

Do NOT modify LevelProgression.

Do NOT add natural Level 2 -> Level 3 progression.

3. PRODUCTION TANKER BULLET VISIBILITY

C3a already arms production Level 3 Tankers without --enemy-bullets-test.

C3b must make those real production Tanker bullets visible graphically.

Inspect the current renderer and reuse the existing enemy-bullet projection and drawing path.

Do not:

- modify the Tanker firing algorithm;
- modify ammunition values;
- modify initial or reset timers;
- modify firing probability;
- modify the firing ceiling;
- modify Tanker climb speed;
- modify the release threshold;
- modify bullet speed or collision behavior;
- add fake bullets;
- create a second bullet renderer.

Rendering policy must recognize actual Level3Mixed production enemy bullets even when --enemy-bullets-test is false.

Preserve existing behavior:

- normal Level 1 and Level 2 Flippers remain silent by default;
- ordinary and cargo Level 3 Flippers remain silent by default;
- isolated --tanker-test behavior remains unchanged;
- existing combined and optional enemy-bullet development scenarios remain valid.

If:

--level3-test --enemy-bullets-test

is supported under existing composability rules, it may additionally enable development Flipper bullets according to the established optional policy. It must not change mixed-wave composition or production Tanker firing semantics.

4. LEVEL 3 HUD AND DISPLAY

Provide accurate graphical information for direct Level3Mixed access using existing HUD conventions.

At minimum ensure:

- displayed level is 3;
- score is correct;
- lives are correct;
- Level 3 direct-development labeling or window title is unambiguous;
- mixed-wave remaining information does not misleadingly describe the wave as a simple fixed Flipper count when committed Tanker cargo adds obligations.

Use existing C3a public/internal accounting rather than creating parallel mutable counters.

Level3WaveLedger remains authoritative.

Do not introduce another source of identity or completion truth.

Prefer a concise, understandable mixed-wave HUD over excessive internal detail.

IMPORTANT SUPER ZAPPER DISPLAY BOUNDARY

Do not invent a new Super Zapper charge counter merely because Level 3 starts with two charges.

Preserve existing SuperZapperSystem display and flash conventions unless the repository already has an established charge-display mechanism intended for reuse.

Per-enemy PendingZapTicks is distinct from game-level SuperZapperSystem charge/flash state.

Do not conflate these two timer/state systems.

5. WINDOW TITLE AND DEVELOPMENT LABELING

Use existing Program and title conventions to identify direct Level 3 mixed-wave DEVELOPMENT ONLY access clearly.

Do not rename unrelated scenarios.

Do not present --level3-test as natural campaign progression.

6. TERMINAL LEVEL 3, REPLAY AND RESTART

Preserve the C3a terminal Level 3 model.

After successful completion and exit travel:

- do not advance to Level 4;
- do not enter natural campaign progression;
- direct Level 3 remains terminal.

Exercise existing replay and restart conventions.

Expected intent:

- successful terminal Level 3 replay starts a fresh Level3Mixed wave using established replay semantics;
- Game Over restart uses existing full new-game semantics;
- ordinary death remains on the same Level/configuration and preserves C3a unresolved-identity behavior.

Do not redesign restart or input handling.

7. AUTOMATED C3b TESTS

Before manual acceptance, add or strengthen focused tests for C3b integration.

Cover at least:

CLI AND ROUTING

- --level3-test is recognized;
- it selects LevelConfiguration.Level3Mixed;
- displayed identity is Level 3;
- incompatible scenario combinations are rejected;
- --lives remains composable;
- --enemy-bullets-test remains composable where intended;
- --level3-geometry-test remains unchanged and Flipper-only;
- direct Level 3 does not enable natural progression.

RENDERING

- production Level3Mixed Tanker bullets are selected for drawing without --enemy-bullets-test;
- existing renderer projection/callback seams are reused;
- generated/projected coordinates remain finite;
- headless renderer tests are not described as visual-fidelity acceptance;
- old modes retain established bullet-visibility behavior.

HUD

- Level 3 identity is correct;
- score and lives use established values;
- mixed-wave presentation reflects real original/cargo obligations;
- the HUD does not pretend the production wave is simply 15 Flippers;
- no parallel mutable accounting is introduced.

REPLAY AND RESTART

- terminal direct Level 3 does not progress;
- successful replay creates a fresh Level 3 wave and ledger according to established semantics;
- Game Over restart follows established full-reset behavior;
- ordinary death remains on the same C3a lifecycle.

REGRESSION

- existing Level 3 geometry diagnostic stays unchanged;
- Level 1 and Level 2 behavior stay unchanged;
- input/controller aliases and diagnostics stay unchanged;
- established scenario conflicts stay unchanged except for the deliberate addition of --level3-test.

Use existing test seams.

Do not use reflection.

Do not mutate inaccessible gameplay internals solely for testing.

Do not duplicate C3a ledger, random, identity or test infrastructure.

8. C3a REOPEN RULE

If C3b graphical integration exposes an apparent defect in any of these:

- Level3Mixed composition;
- Egg scheduling;
- Tanker selection;
- identity ledger;
- cargo ownership or lifecycle;
- Tanker firing mechanics;
- Super Zapper resolution;
- death/reconstruction;
- ordinary completion;
- all-rim behavior;
- geometry or pivot behavior;

STOP at a clean recoverable point and report the exact defect before changing that behavior.

Do not bury a C3a lifecycle correction inside C3b presentation work.

If C3a must be reopened, identify:

- the exact observed defect;
- the failing path;
- why it is not merely a renderer/HUD/access problem;
- the narrowest likely correction;
- the tests that would need to be reopened.

9. HARD SCOPE BOUNDARY — NO C4

Do NOT implement:

- natural Level 2 -> Level 3 progression;
- LevelProgression changes;
- changes to successful Level 2 exit commit behavior;
- Level 4;
- campaign-cycle architecture;
- between-level approach presentation.

Also do NOT retune:

- C1/C1.5 directed geometry;
- spinner/controller behavior;
- C2 Tanker firing constants or algorithm;
- Tanker cargo route or pivot;
- mixed-wave selection or scheduling;
- identity or reconstruction policy;
- ordinary/default Flipper firing policy;
- Super Zapper semantics.

Do not modify Reference/incitti.

10. TRACKED DOCUMENTATION

Update tracked documentation only where implemented C3b access needs an accurate user-facing or developer-facing note.

Do not claim manual acceptance until Neal performs it.

Do not upgrade source-derived or reference-derived behavior to ARCADE VERIFIED merely because graphical access now works.

Preserve evidence distinctions:

- directly observed and accepted arcade behavior retains its existing classification;
- Incitti/source-derived behavior remains REFERENCE VERIFIED;
- C# identity and lifecycle architecture remains PROJECT DECISIONS;
- --level3-test remains DEVELOPMENT ONLY.

Do not broadly rewrite TEMPEST-BEHAVIOR-SPEC.md during implementation.

Behavior-spec updates based on new manual observations must wait for Neal's manual acceptance results unless a narrow factual access note is required.

11. AUTOMATED VALIDATION

Run focused tests iteratively while implementing.

Before asking Neal for manual acceptance, run an appropriate focused C3b/C3a regression group covering:

- CLI and DebugController behavior;
- Program/configuration routing where testable;
- Level3MixedWave;
- Level3WaveLedger;
- Tanker;
- Cargo;
- SuperZapper;
- renderer and rendering policy;
- HUD/display;
- Level3Geometry;
- replay and restart;
- InputSystem and InputDiagnostic;
- LevelConfiguration;
- LevelProgression regression protection.

Use valid repository test names and xUnit/VSTest filter syntax.

Then run:

dotnet test TempestCSharp.slnx --no-restore

dotnet build TempestCSharp.slnx --no-restore

git diff --check

FINAL INSPECTION

Inspect:

git status --short
git diff --stat
git diff
git status --short -- Reference/incitti
git status --short -- TEMPEST-BEHAVIOR-SPEC.md
git ls-files --others --exclude-standard
git log -1 --oneline

Confirm:

- no accidental C4 or progression work;
- no Reference/incitti change;
- no controller/spinner remapping;
- no C2 firing-algorithm change;
- no geometry or cargo-route change;
- no C3a identity/lifecycle redesign;
- no default Flipper firing-policy change;
- no reflection or prohibited state mutation;
- no accidental tracked TEMP/debug artifacts;
- existing untracked TEMP/ remains untouched.

Do not commit.

Do not push.

12. MANUAL ACCEPTANCE HANDOFF

If automated gates pass, STOP and provide Neal with a concise manual test sequence beginning with:

dotnet run --project src/Tempest.Game -- --level3-test --lives 10

The primary manual run must NOT use --enemy-bullets-test.

This is necessary to verify that production Tanker firing and bullet visibility work independently of the optional Flipper-bullet development flag.

Manual checklist:

A. ENTRY AND PRESENTATION

- Level 3 plus geometry appears correctly.
- Displayed level is 3.
- Lives and score appear correct.
- No obvious HUD or remaining-wave accounting nonsense.
- Blaster movement and direction remain correct.
- Development-only Level 3 labeling is understandable.

B. MIXED WAVE

- Incoming Eggs behave normally.
- Flippers and Tankers appear according to natural random selection.
- A zero-Tanker run is allowed and is not automatically a failure.
- Several Tankers may occur naturally.
- Do not require six naturally concurrent Tankers.

C. FIRING

- Tanker bullets are visible without --enemy-bullets-test.
- Tankers continue climbing while firing.
- No release-point firing burst occurs.
- When naturally observed, separate multiple shots before release remain possible but are not mandatory for every Tanker.
- Ordinary and cargo Flippers remain silent by default.
- Already-fired Tanker bullets behave independently after carrier removal.

D. TANKER CARGO

- Shooting a Tanker produces two live Flippers normally.
- Cargo enters distinct non-Tanker lanes.
- The established outward lane route remains intact.
- Visible pivots remain inward through the web.
- Below-rim direction remains stable.
- No obvious duplicate children or scoring appears.

E. NATURAL RELEASE

- A naturally escaping Tanker produces live cargo.
- No carrier kill-score behavior appears for natural release.

F. SUPER ZAPPER

- First and second charges work sensibly with mixed targets.
- No obvious duplicate score appears.
- No stale behavior appears from a removed carrier.
- Delayed carrier and child behavior remains plausible.
- Do not require exact hidden countdown verification manually.

G. DEATH AND RE-ENTRY

- Lose lives in useful states when practical.
- Unresolved enemies return appropriately.
- Resolved carriers or children do not visibly resurrect.
- No obvious duplicate carrier/cargo behavior appears.
- Hidden identity-set equality remains automated evidence and is not a manual requirement.

H. ALL-RIM AND COMPLETION

- When naturally encountered, qualifying rim survivors disappear correctly.
- No kill score appears merely from rim clearing.
- No orphan cargo remains.
- Successful exit behaves normally.

I. TERMINAL AND REPLAY

- Direct Level 3 does not advance to another campaign level.
- Successful R replay works according to established semantics.
- Game Over R restart follows established full-restart behavior.

J. REGRESSION OBSERVATION

- Spinner/controller/keyboard input remains normal.
- No rendering crash or obvious artifact occurs.
- Optional spot checks of --level3-geometry-test and normal Level 1/Level 2 may be performed if warranted.

Do not require exact timing measurements or hidden ledger verification manually.

If a random event is not observed, report it as NOT OBSERVED rather than forcing production composition.

13. END-OF-RUN REPORT

At the end of the implementation run, report only:

- files changed;
- exact prompt/report files saved under ignored AI-Logs;
- --level3-test routing behavior;
- conflict and option-composability behavior;
- renderer and bullet-visibility behavior;
- HUD and title behavior;
- replay and restart behavior;
- automated tests added or changed;
- focused validation results;
- full solution test result;
- build result;
- git diff --check;
- prohibited-scope inspection;
- Reference/incitti and behavior-spec status;
- whether any C3a defect was exposed;
- exact manual command and checklist;
- anything Neal should pay special attention to manually;
- any blocker.

Do not declare C3b complete until Neal reports the manual acceptance results.

Do not commit.

Do not push.