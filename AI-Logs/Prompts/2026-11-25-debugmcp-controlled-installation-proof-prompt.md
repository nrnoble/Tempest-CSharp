# Tempest Controlled DebugMCP Installation and Proof of Capability

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, installation, extension activation, configuration, debugger sessions, tests, MCP registration, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code or agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the completion token do not invalidate it.
5. This check is intended only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before any workspace or system action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform one controlled installation and proof-of-capability milestone for:

`DebugMCP VS Code extension`
`Extension ID: ozzafar.debugmcpextension`
`Authorized version: 2.4.4`

The objective is to determine empirically whether DebugMCP exposes the installed Microsoft C#/.NET debugger to the exact Copilot SDK agent workflow used for Tempest.

This proof must answer:

1. Can DebugMCP 2.4.4 be installed and activated safely?
2. Does its server remain loopback-only?
3. Can it be registered for GitHub Copilot without configuring any other agent?
4. Does a fresh intended VSC/Copilot agent session expose DebugMCP tools?
5. Can those tools start and control a C#/.NET debug session?
6. Can VSC hit a breakpoint, inspect a call stack and variables, evaluate an expression, step, continue, and terminate cleanly?
7. Can all temporary debugger state be removed without changing Tempest source, tests, gameplay, or tracked project configuration?
8. If the proof fails, can DebugMCP and all proof-created artifacts be rolled back precisely?

This is a tooling proof, not a Tempest development task.

Do not investigate or fix gameplay.

Do not perform the deferred Flipper/Tanker architecture audit.

Do not implement Level 18+.

Do not use another AI model.

Do not launch interactive Raylib gameplay.

Do not stage, commit, or push.

---

# Required prior reports

Read completely:

`AI-Logs/Reports/2026-11-25-vscode-copilot-development-tool-assessment-report.md`

`AI-Logs/Reports/2026-11-25-debugmcp-prerequisite-resolution-report.md`

Use the prerequisite-resolution report as the immediate authority for:

- current environment;
- known DebugMCP behavior;
- security controls;
- side-effect inventory;
- rollback requirements;
- known upstream issues;
- unresolved host-integration question.

Do not repeat the broad tooling survey.

---

# User-confirmed prerequisites

Record the following as direct manual evidence from Neal:

## Workspace trust

The Tempest workspace explicitly reports:

`You trust this folder`

The trusted path shown is:

`C:\Users\nrnob\source\repos\Tempest`

Tasks, debugging, workspace settings, and enabled extensions are active.

## C# Dev Kit discovery

The VS Code Testing view visibly discovers:

`Tempest.Game.Tests (net9.0)`

and its xUnit test classes and tests.

## C# Dev Kit execution

Neal successfully ran C# Dev Kit tests through Test Explorer.

A supplied result showed:

- 8 tests passed in one run;
- an individual test passed in a later run;
- zero failures.

## Debug Test availability

Neal confirms an individual test context menu contains:

`Debug Test`

These observations resolve:

- workspace-trust uncertainty;
- net9.0 xUnit Test Explorer discovery uncertainty;
- C# Dev Kit test-execution uncertainty;
- availability of the manual test-debug command.

The remaining principal uncertainty is whether DebugMCP registration reaches the exact Copilot SDK agent host used by VSC.

---

# Scope and authorization

This task authorizes:

- pre-install metadata inventory;
- installation of DebugMCP version 2.4.4 only;
- activation of that extension;
- loopback listener verification;
- configuration for GitHub Copilot only;
- a fresh VSC/Copilot session if required;
- use of DebugMCP tools for one bounded C# debugger proof;
- one deterministic existing Tempest unit test;
- temporary breakpoints;
- harmless variable/expression inspection;
- bounded stepping;
- clean debugger termination;
- removal of proof-created breakpoints;
- rollback if the proof fails;
- creation of the required AI prompt/report files.

This task does not authorize:

- installing a different DebugMCP version without stopping for Neal;
- installing another debugger bridge;
- installing another SDK;
- installing Node/npm;
- adding NuGet packages;
- adding another AI model;
- configuring Claude, Continue, Ollama, Copilot CLI, or another agent;
- modifying Tempest production code or tests;
- modifying tracked project configuration;
- creating a tracked `.vscode/launch.json`;
- launching Raylib;
- stage, commit, or push.

---

# Human-interaction rule

DebugMCP may display VS Code UI prompts that VSC cannot safely operate.

If human interaction is required, STOP at that exact point and tell Neal:

1. the exact dialog or prompt expected;
2. the exact option to select;
3. options that must not be selected;
4. what Neal should report after acting;
5. whether the current VSC session should remain open;
6. whether a fresh VSC chat will be required afterward.

Examples include:

- extension installation confirmation;
- extension trust/publisher confirmation;
- agent-selection dialog;
- `Configure GitHub Copilot`;
- MCP server trust;
- tool approval;
- workspace trust;
- VS Code reload/restart.

Do not use UI automation.

Do not guess what Neal clicked.

Do not continue until Neal confirms completion if the action affects registration, trust, permissions, or session restart.

If the prompt can continue safely without interaction, proceed.

---

# Exact authorized extension

Install only:

`ozzafar.debugmcpextension`

Require exact version:

`2.4.4`

Before installing, verify that the retrieved Marketplace package is:

- extension ID `ozzafar.debugmcpextension`;
- version `2.4.4`;
- associated with the previously reviewed Microsoft/DebugMCP source;
- not a similarly named extension;
- not a newer unreviewed version;
- not an Open VSX or third-party fork.

If exact-version installation cannot be guaranteed with the available VS Code command or Marketplace interface, STOP and ask Neal before installing anything.

Do not silently install the latest version.

---

# Security policy

The proof must maintain:

- localhost/loopback binding only;
- no LAN binding;
- no port forwarding;
- no remote workspace;
- one Tempest VS Code window where practical;
- one trusted local workspace;
- no unrelated workspaces open during the debugger proof;
- no blanket tool auto-approval;
- debug-output capture disabled;
- local test data only;
- no credential or secret inspection;
- explicit timeouts;
- clean test/debug termination;
- complete breakpoint cleanup;
- listener verification after completion.

Keep:

`debugmcp.bindHost`

at its default loopback values.

Do not change it to:

- `0.0.0.0`;
- a LAN address;
- a public interface.

Keep DebugMCP output capture disabled for the initial proof.

Keep the generated auto-approval list empty.

Approve only the specific operations required by this proof.

---

# Known upstream risks

Mitigate these known risks from the prior review.

## Wrong-workspace routing

Because DebugMCP issue #191 describes wrong-window/workspace routing risk:

- keep only the relevant Tempest workspace window open if practical;
- use the exact Tempest workspace path;
- verify every returned source frame belongs to:
`C:\Users\nrnob\source\repos\Tempest`;
- stop immediately if a source path, workspace, project, or process is not Tempest.

## Initial stale or empty frame

Because issue #201 describes initial paused-state/frame timing:

- require a nonempty call stack at the first breakpoint;
- require a selected frame;
- require the selected source path and line to match the expected Tempest test;
- do not inspect variables or step until the frame is verified;
- treat an empty, stale, or mismatched first frame as FAIL/BLOCKED;
- do not retry indefinitely.

---

# Phase 1: baseline repository inventory

Before installing anything, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked files;
- ignored local assets at metadata level only;
- current breakpoint state if safely discoverable;
- current debug sessions;
- current VS Code windows;
- active workspace path;
- active profile.

Expected baseline includes:

- Default Profile;
- local Windows extension host;
- Tempest workspace trusted;
- no DebugMCP extension;
- no active DebugMCP listener;
- no DebugMCP global storage;
- no user `mcp.json` at the previously inspected path;
- no existing DebugMCP registration;
- no existing `debug-live` destination in the inspected locations.

If the baseline differs materially, STOP and reconcile ownership before activation.

---

# Phase 2: pre-install user-artifact inventory

Capture presence, paths, metadata, and ownership only for relevant locations.

Do not dump secret-bearing configuration.

Inventory:

- installed extension list and versions;
- Default Profile identity;
- VS Code extension directory;
- DebugMCP prospective extension directory;
- VS Code user-data directory;
- VS Code user MCP file;
- workspace MCP file;
- Copilot MCP configuration path;
- relevant `disabledMcpServers` key only;
- DebugMCP global-storage location;
- DebugMCP log/output locations if discoverable;
- `%USERPROFILE%\.agents\skills\debug-live`;
- `%USERPROFILE%\.agents\skills\debug`;
- `%USERPROFILE%\.agents\skills\really-debug`;
- `%COPILOT_HOME%\skills\debug-live`;
- `%COPILOT_HOME%\skills\debug`;
- `%COPILOT_HOME%\skills\really-debug`;
- listeners on ports 3001, 3002 and 3100;
- existing DebugMCP-related processes;
- active breakpoints and debug sessions.

Do not report `%COPILOT_HOME%` value if it is sensitive.

If any skill destination or legacy sibling exists, STOP before activating DebugMCP because activation may overwrite or remove it.

Do not rely on uninstall to preserve existing user data.

---

# Phase 3: install exact DebugMCP 2.4.4

Install only the exact authorized extension/version into the currently authorized VS Code profile.

Do not apply it to all profiles.

Do not install it into unrelated profiles.

Record:

- installation command or UI path;
- exact installed version;
- extension directory;
- success/failure;
- whether VS Code reload is requested;
- whether activation occurred immediately;
- newly created artifacts.

If Windows security blocks installation or activation:

- do not bypass security;
- do not retry the same artifact repeatedly;
- record the exact artifact, command, and error;
- STOP with BLOCKED status.

---

# Phase 4: activation-side-effect audit

After activation, but before configuring an agent, verify:

- extension process/activation state;
- DebugMCP output channel presence;
- listener address and port;
- listener owning PID;
- loopback-only binding;
- per-window control server behavior where visible;
- global-storage directory;
- window-registry record presence;
- skill directories created;
- whether legacy directories were removed;
- whether an agent-selection prompt appears;
- whether user MCP configuration changed before explicit selection;
- whether any other agent configuration changed;
- whether the repository changed.

If listener binds beyond loopback, STOP and roll back.

If unrelated agent configuration is modified automatically, STOP and roll back.

If an unexpected skill directory was overwritten or deleted, STOP and roll back.

---

# Phase 5: configure GitHub Copilot only

When DebugMCP asks which agent to configure:

Select only:

`GitHub Copilot`

Do not select:

- Copilot CLI;
- Claude;
- Continue;
- Ollama;
- Codex;
- Cline;
- Cursor;
- Windsurf;
- Roo Code;
- any other agent.

If VSC cannot interact with the prompt, STOP and give Neal the exact instruction.

After configuration, verify metadata only:

- user MCP file now exists if expected;
- only the intended DebugMCP server entry was added;
- unrelated MCP entries remain unchanged;
- auto-approval list remains empty;
- bind host remains loopback;
- output capture remains disabled;
- no unrelated settings changed.

Do not print secrets or full configuration contents.

---

# Phase 6: current-session tool discovery

Determine whether DebugMCP tools appear in the current VSC/Copilot agent session.

Look specifically for the documented DebugMCP tool family, including equivalents of:

- debugging instructions;
- start debugging;
- stop debugging;
- restart debugging;
- pause;
- continue;
- step over;
- step into;
- step out;
- add breakpoint;
- remove breakpoint;
- clear breakpoints;
- list breakpoints;
- debug status;
- variable inspection;
- expression evaluation.

If tools appear, continue to the debugger proof.

If tools do not appear:

1. do not assume installation failed;
2. verify MCP registration and server health;
3. determine whether an ordinary VS Code Chat Tools picker must enable the server;
4. determine whether a new Copilot/VSC session is required;
5. STOP and give Neal precise instructions if a fresh session or UI tool-enablement step is required.

Do not repeatedly reload or restart without a bounded plan.

---

# Session-handoff rule

A fresh VSC chat may be necessary before newly registered DebugMCP tools appear.

If so, the current session must first write a checkpoint into the required report containing:

- completed baseline;
- installed version;
- activation result;
- actual loopback listener;
- side effects;
- Copilot registration result;
- current tool-discovery failure;
- exact cleanup ownership;
- exact next-session instruction.

Then STOP and tell Neal:

- whether to close only the current chat or reload VS Code;
- whether to leave the Tempest window open;
- how to start the fresh VSC session;
- what short continuation request to use;
- which report the fresh session must read;
- not to reinstall the extension.

Do not declare the proof failed merely because a fresh session is required.

---

# Phase 7: choose the proof test

Use one existing, deterministic, fast Tempest xUnit test already visible in Test Explorer.

Prefer a simple test that:

- does not initialize Raylib;
- does not use a gameplay recording;
- does not require user input;
- does not access protected assets;
- executes a small amount of production C#;
- has a safe local variable/object suitable for inspection;
- reliably completes quickly.

Do not modify the test.

Do not add a test.

Do not choose:

- campaign soak;
- full recording;
- long-running test;
- Application Control-sensitive generated binary;
- networking;
- filesystem mutation;
- screenshot creation.

Record the selected test and occurrence.

---

# Phase 8: Proof 1 — basic C# debugger control

Using DebugMCP tools, perform the following bounded proof.

## Breakpoint

Add one temporary breakpoint at a known executable source line used by the selected test.

Record:

- file;
- line;
- expected method;
- why the line should be reached.

Verify breakpoint list contains exactly the proof breakpoint plus any pre-existing user breakpoints.

Do not remove pre-existing user breakpoints.

## Start

Start the specific existing test under the C#/.NET debugger.

Use an explicit startup timeout of 120 seconds.

Require:

- `coreclr` or the expected installed .NET debug adapter;
- correct Tempest project/test process;
- correct workspace;
- breakpoint hit.

If the test does not stop within the bound, stop the session and report failure.

## First-stop validation

Require:

- nonempty call stack;
- selected frame;
- source path inside Tempest;
- expected source file;
- expected line or nearby executable line;
- expected test/method context.

Do not continue if the frame is stale, empty or mismatched.

## Inspection

Inspect only harmless local test data.

Demonstrate:

- one local variable;
- one object/member or expanded child;
- one harmless expression evaluation.

Do not inspect:

- credentials;
- environment secrets;
- tokens;
- unrelated user information;
- recording contents;
- ROM contents.

## Stepping

Demonstrate:

- step over;
- either step into or step out, where the source structure permits;
- updated source location after each operation.

Use a maximum of five total stepping operations.

Do not step through thousands of calls.

## Continue and termination

Continue execution.

Require:

- test reaches a normal terminal result where possible;
- debugger session can be stopped/terminated cleanly;
- no orphaned test/debug process remains.

If the test completion result is not available through DebugMCP, verify through existing C# Dev Kit/test output without starting another run.

---

# Phase 9: breakpoint cleanup

After the debug session:

- remove only the proof-created breakpoint;
- verify it is absent;
- verify pre-existing breakpoints remain unchanged;
- verify no active debug session remains;
- verify no orphaned test process remains;
- verify DebugMCP listener remains loopback-only while the extension is enabled.

A proof with a leftover breakpoint or debug session is not complete.

---

# Phase 10: repository-integrity check

After Proof 1, verify:

- no production file changed;
- no test file changed;
- no project file changed;
- no tracked `.vscode` file appeared;
- no launch/task/runsettings file appeared;
- no gameplay recording changed;
- no screenshot or video changed;
- no ROM/reference asset changed;
- ignored build outputs may have refreshed but remain ignored;
- Git tracked state matches baseline;
- expected AI prompt/report files are the only repository artifacts created by the task.

Do not stage, commit, or push.

---

# Phase 11: Proof 1 result

Classify Proof 1 as:

## PASS

Only if all required operations succeeded:

- tools visible;
- correct debugger started;
- breakpoint hit;
- correct nonempty frame;
- local inspected;
- object/member inspected;
- expression evaluated;
- step over succeeded;
- step into/out succeeded where supported;
- continue succeeded;
- termination clean;
- breakpoint cleanup complete;
- project unchanged.

## PARTIAL

Use if the core debugger works but one noncritical operation is unsupported or ambiguous.

State whether the tool is still useful for Tempest.

## FAIL

Use for:

- tools absent after required refresh/session handoff;
- wrong workspace;
- stale frame that cannot be resolved within one bounded retry;
- breakpoint never hits;
- test cannot start;
- debugger cannot terminate;
- cleanup incomplete;
- unexpected project modifications;
- listener security problem.

Do not reinterpret failures as passes.

---

# Phase 12: Optional Proof 2 readiness only

Do not perform a full Tempest gameplay-debugging proof in this task unless Proof 1 passes and no additional configuration is needed.

After Proof 1, assess whether DebugMCP can next support:

- one deterministic existing gameplay test; or
- a headless playback launch using a separately authorized temporary debug configuration.

Do not create `launch.json` in this task.

Do not launch Raylib.

Do not debug the full Levels 1–16 recording.

Recommend the smallest next proof only.

---

# Keep or roll back decision

After Proof 1, determine whether DebugMCP should remain installed.

## If PASS

Keep it installed, but:

- keep loopback-only binding;
- keep output capture disabled;
- keep empty auto-approval;
- preserve the Copilot MCP registration;
- preserve the installed `debug-live` skill;
- document all installed artifacts;
- do not add another debugger bridge.

## If PARTIAL

Recommend keep or rollback based on demonstrated value, security, cleanup, and the missing operation.

Do not assume partial value is sufficient.

## If FAIL

Roll back all proof-created DebugMCP artifacts.

Rollback must include, where created by this proof:

- stop debug sessions;
- remove proof breakpoints;
- disable/uninstall only DebugMCP 2.4.4;
- remove only the DebugMCP MCP registration;
- restore only prior `disabledMcpServers` state;
- remove only proof-created `debug-live` skill directories;
- restore any explicitly backed-up prior skill content;
- remove DebugMCP global storage only if absent at baseline and proven proof-owned;
- verify listener stopped;
- verify extension no longer installed;
- verify unrelated settings/MCP entries preserved;
- verify Git state unchanged.

Do not delete shared VS Code cache or log directories wholesale.

---

# DebugMCP operational policy if retained

If the proof passes and DebugMCP remains installed, recommend project instructions equivalent to:

- Use deterministic playback/tests to reproduce and localize first.
- Use DebugMCP to inspect live state and control flow after the failing boundary is known.
- Prefer deterministic tests or headless playback over interactive Raylib.
- Require explicit bounds for debugger startup, waits and stepping.
- Verify workspace/source frame before evaluating state.
- Treat empty/stale/mismatched frames as failure.
- Keep output capture off unless specifically authorized.
- Never inspect credentials or unrelated user state.
- Clear temporary breakpoints and terminate every session.
- Do not use debugger success as a substitute for regression tests or Neal’s manual acceptance.
- Do not auto-approve all DebugMCP tools.
- Keep MCP loopback-only.

Do not update durable project instructions in this task.

Include proposed wording in the report for later authorization.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-debugmcp-controlled-installation-proof-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-debugmcp-controlled-installation-proof-report.md`

These files remain untracked and unstaged unless a later task authorizes a tooling commit.

Do not amend the completed Level 17 closeout.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Manual prerequisite evidence
4. Active profile and workspace
5. Pre-install extension inventory
6. Pre-install MCP/config metadata
7. Pre-install skill metadata
8. Pre-install global-storage metadata
9. Pre-install listener/process metadata
10. Exact extension identity/version/source verification
11. Installation method and result
12. Activation result
13. Activation-time side effects
14. Skill files created/changed
15. MCP files created/changed
16. Global storage created
17. Listener addresses, ports and owning PID
18. Loopback-only verification
19. Agent-selection action
20. GitHub Copilot registration result
21. Current-session tool discovery
22. Fresh-session/tool-refresh requirement
23. Human interactions required
24. Selected deterministic test
25. Breakpoint file/line/method
26. Debug-session startup
27. Debug-adapter/process identity
28. First-stop source/frame validation
29. Call-stack result
30. Local-variable result
31. Object/member result
32. Expression-evaluation result
33. Step-over result
34. Step-into/out result
35. Continue/test-completion result
36. Debug-session termination
37. Breakpoint cleanup
38. Orphan-process check
39. Repository-integrity result
40. Configuration-integrity result
41. Security result
42. Proof classification
43. Demonstrated Tempest value
44. Known limitations
45. Keep-versus-rollback decision
46. Rollback actions if performed
47. Final listener state
48. Final extension inventory
49. Final MCP/skill/global-storage state
50. Proposed operational policy
51. Recommended next proof
52. Production source changed: no
53. Tests changed: no
54. Tracked project configuration changed: no
55. Gameplay recordings changed: no
56. Stage/commit/push: none
57. Final Git/worktree state
58. Recommended next step

If the task pauses for a fresh session, preserve an interim checkpoint in this report before stopping.

---

# Stop conditions

STOP and report or request Neal’s action rather than guessing if:

- exact DebugMCP 2.4.4 cannot be obtained;
- extension publisher/source identity differs;
- installation requests a broader scope than authorized;
- a user prompt requires selection and cannot be safely handled;
- any skill target already exists unexpectedly;
- activation deletes or overwrites pre-existing user data;
- listener binds beyond loopback;
- unrelated MCP or agent configuration changes;
- Copilot registration cannot be isolated;
- tools remain absent after one authorized refresh/fresh-session attempt;
- wrong workspace is selected;
- first frame is empty/stale/mismatched after one bounded retry;
- test does not stop at breakpoint within 120 seconds;
- debugger cannot terminate;
- cleanup cannot be verified;
- repository changes unexpectedly;
- Windows security blocks an artifact or command;
- any operation stalls.

Do not weaken security.

Do not enter retry loops.

A blocked proof is preferable to an unsafe installation.

---

# Completion criteria

This proof is complete only when:

- baseline inventory is captured;
- exact DebugMCP 2.4.4 is installed or installation is precisely blocked;
- activation side effects are audited;
- listener security is verified;
- GitHub Copilot registration is controlled;
- tool visibility to the intended VSC agent is determined;
- one existing deterministic C# test is debugged if tools are available;
- breakpoint, call stack, variables, expression evaluation and stepping are objectively tested;
- debugger terminates cleanly;
- temporary breakpoints are removed;
- repository integrity is verified;
- DebugMCP is either retained under a documented safe policy or fully rolled back;
- no project source, tests, gameplay, or tracked configuration changed;
- nothing is staged, committed, or pushed.

Then STOP.

TEMPEST-END-DEBUGMCP-CONTROLLED-INSTALLATION-PROOF-COMPLETE