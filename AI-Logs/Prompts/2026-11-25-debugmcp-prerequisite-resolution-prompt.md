# Tempest DebugMCP Prerequisite Resolution — Read-Only Assessment

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, debugger sessions, installations, configuration changes, extensions, builds, tests, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code or agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the completion token do not invalidate it.
5. This check is intended only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before any workspace or system action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Resolve the specific prerequisites that blocked a bounded DebugMCP installation/validation decision for the Tempest project.

This is a **read-only prerequisite-resolution assessment**.

Answer:

1. Which exact VS Code profile, extension host, and Copilot Agent/tool host is being used for Tempest development?
2. Can that exact Copilot host discover and invoke MCP servers or Language Model Tools contributed by an installed VS Code extension?
3. Can the currently installed Microsoft C# extension and C# Dev Kit discover, run, and manually debug an existing Tempest .NET 9 xUnit test without modifying the project?
4. Can the existing C# debugger manually launch Tempest headless playback with command-line arguments without modifying tracked project files?
5. What user-level registration, MCP, skill, extension-storage, profile, setting, or cache artifacts would DebugMCP create during installation/activation?
6. Can those artifacts be inventoried before installation and rolled back precisely afterward?
7. Are the previous blockers resolved sufficiently to authorize one isolated DebugMCP proof-of-capability milestone?

Do not install DebugMCP.

Do not install any extension.

Do not add another AI model.

Do not use Claude, Continue, Ollama, or another model to answer compatibility questions.

Do not modify project source, tests, configuration, settings, launch files, tasks, MCP files, profiles, or user-level agent files.

Do not stage, commit, or push.

---

# Required prior report

Read completely:

`AI-Logs/Reports/2026-11-25-vscode-copilot-development-tool-assessment-report.md`

Use its findings as the starting point.

Do not repeat the broad tool survey.

This task exists only to resolve its exact blockers.

Also inspect, only where needed:

`AI-Logs/Reports/2026-11-23-agent-runtime-capability-probe-report.md`

Do not reopen Tempest gameplay findings.

---

# Starting safety inventory

Before any probing, record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked project files;
- ignored local assets only at the metadata level;
- VS Code version;
- active VS Code profile identity where safely discoverable;
- current relevant extensions and versions;
- current .NET SDKs/runtimes;
- current working directory/workspace folder.

Do not enumerate or inspect protected ignored directories.

Do not expose secret settings or tokens.

---

# NO-INSTALLATION / NO-CONFIGURATION BOUNDARY

Do not:

- install, update, disable, enable, or uninstall extensions;
- use `code --install-extension`;
- use `code --uninstall-extension`;
- edit VS Code user settings;
- edit workspace settings;
- edit profile settings;
- edit MCP configuration;
- edit Copilot settings;
- edit skill directories;
- edit `.vscode/`;
- create `launch.json`;
- create `tasks.json`;
- create `.runsettings`;
- add packages;
- add SDKs;
- change `global.json`;
- alter environment variables permanently;
- alter PATH;
- alter Windows security;
- launch the Raylib game;
- stage, commit, or push.

Read-only process launching is allowed only where explicitly authorized below and only if it does not persist configuration.

---

# Prerequisite A: identify the actual VS Code profile

Determine the exact VS Code profile used for Tempest development.

Inspect safely:

- `code --version`;
- running VS Code process command lines where available;
- VS Code user-data/profile arguments;
- profile metadata;
- extension installation locations;
- workspace identity;
- bundled versus profile-installed extensions;
- whether the Tempest window uses Default Profile or a named profile;
- whether Remote, WSL, Dev Container, SSH, or local Windows extension host is active.

Record:

- profile name/ID if discoverable;
- profile storage path;
- extension storage path;
- user-data path;
- local/remote extension-host type;
- whether the profile is shared with unrelated workspaces;
- whether a dedicated proof profile would offer meaningful isolation.

Do not open or disclose secrets from settings files.

Read only non-sensitive metadata and extension identifiers.

If profile identity cannot be proved safely, state exactly what remains unknown.

---

# Prerequisite B: identify the actual Copilot agent/tool host

Determine what is executing the current Tempest VSC prompts.

Distinguish among:

- ordinary GitHub Copilot Chat Agent mode inside the VS Code window;
- VS Code SDK/session host;
- built-in Copilot Chat extension;
- cloud or coding agent;
- another local extension host;
- external terminal/CLI agent.

Inspect:

- bundled Copilot Chat extension metadata;
- process/extension-host metadata;
- available agent tool inventory;
- current session/tool descriptions;
- VS Code output/log metadata where safe;
- official documentation describing MCP/tool discovery for the exact host.

Answer:

1. Is this session the same Copilot Agent host that would receive extension-contributed Language Model Tools?
2. Does it consume VS Code MCP registration automatically?
3. Does it require enabling a server in the Tools picker?
4. Does it require a new chat/session after extension activation?
5. Does it require workspace trust?
6. Does it require a separately installed `GitHub.copilot` extension, or is bundled `GitHub.copilot-chat` sufficient?
7. Are tool permissions controlled per session, per workspace, per profile, or globally?
8. Can one extension's tools appear only in ordinary VS Code Chat but not in this SDK session?

Do not infer compatibility from the word “Copilot” in extension marketing.

Use official VS Code/Copilot documentation and installed manifest metadata.

---

# Prerequisite C: verify generic extension-contributed tool discovery without installing anything

Use already installed extensions and built-in tools to determine whether the current host recognizes extension-contributed tools.

Do not install a test extension.

Inspect whether any currently installed extension contributes:

- `languageModelTools`;
- MCP server definitions;
- chat participants;
- agent skills;
- commands visible to Copilot;
- tool manifests.

Determine whether those contributions are visible in the current agent tool list.

If no installed extension provides a suitable harmless example, say so.

Do not modify settings to force discovery.

The purpose is to validate the host/tool-discovery path, not to manufacture a proof.

---

# Prerequisite D: C# Dev Kit and .NET 9 test discovery

Determine whether installed:

- `ms-dotnettools.csharp 2.160.4`;
- `ms-dotnettools.csdevkit 3.40.215`;
- `.NET SDK 9.0.304`;

can discover and run the existing net9.0 xUnit tests in the current workspace.

This probe may:

- inspect VS Code Testing API/test-controller metadata;
- inspect C# Dev Kit logs/output;
- run one existing deterministic test through the existing Test Explorer command path if it can be invoked without configuration changes;
- run the same test by CLI as a control.

Do not change any test.

Do not create a new test.

Do not update packages.

Choose one fast, stable existing deterministic test that does not launch Raylib or use protected user data.

Record:

- whether C# Dev Kit discovers it;
- whether parameterized tests appear correctly;
- whether running it through the Test Explorer path works;
- whether CLI execution works;
- SDK/runtime used;
- exact error if test discovery fails.

Do not assume the Marketplace page's current .NET 10 statement applies to the installed version.

Establish actual behavior where read-only evidence permits.

---

# Prerequisite E: manual C# test debugging

Determine whether the installed VS Code/C# stack can debug one existing net9.0 xUnit test manually.

This does not mean asking Neal to click through the UI during the prompt.

Use only a safe, existing invocation route exposed by VS Code commands/testing APIs if available.

If no command/tool can trigger manual test debugging without changing configuration, establish readiness from:

- test-controller metadata;
- debugger contribution metadata;
- installed C# Dev Kit documentation matching the installed version;
- logs that show debug profiles/controllers;
- exact unresolved gap.

Do not install DebugMCP to test this.

Do not edit source/tests/configuration.

Do not claim a debug session occurred unless it actually occurred.

If a debug session can be started safely:

- use one existing deterministic test;
- stop at an existing breakpoint only if one is already present;
- otherwise do not persist a breakpoint;
- terminate cleanly;
- record whether the test process started under `coreclr`;
- do not inspect sensitive values.

If this cannot be demonstrated safely, report it as unresolved rather than forcing it.

---

# Prerequisite F: headless Tempest debugging readiness

Determine whether the existing Microsoft C# debugger can launch the game assembly with headless-playback arguments without creating tracked configuration.

Assess:

- dynamic C# debug configurations;
- command-palette `.NET: Generate Assets for Build and Debug` behavior, but do not invoke it because it would create files;
- whether C# Dev Kit can launch the startup project with temporary arguments;
- whether an in-memory/dynamic configuration can be supplied by a debugger bridge;
- whether DebugMCP `start_debugging` can pass a configuration name only or accepts inline arguments;
- whether a temporary untracked user/profile launch configuration would be required;
- whether debugging a focused test is preferable for the initial proof.

Do not create `launch.json`.

Do not launch Raylib.

Do not launch interactive gameplay.

A read-only ordinary headless CLI run may be used only as confirmation of the target arguments, not as debugger proof.

Record the exact minimal configuration that a future authorized proof would need.

---

# Prerequisite G: DebugMCP registration behavior

Without installing DebugMCP, inspect its current public source/package manifest/documentation to determine installation and activation side effects.

Identify exact likely artifacts:

- extension installation directory;
- profile extension registration;
- extension globalState/workspaceState;
- `globalStorageUri` content;
- MCP server registration;
- generated server configuration;
- debug-live skill files;
- `~/.copilot/skills/`;
- `~/.agents/skills/`;
- profile settings entries;
- workspace settings entries;
- cached endpoints;
- logs/output channels;
- localhost ports;
- any commands that explicitly write configuration;
- whether registration is automatic or user-invoked;
- whether uninstall removes each artifact automatically.

Distinguish:

- known from source;
- known from manifest;
- known from docs;
- uncertain until installation.

Do not execute registration commands.

---

# Prerequisite H: pre-install and rollback inventory plan

Design a precise pre-install inventory procedure for a future proof.

The inventory should capture metadata only for relevant locations:

- installed extension list;
- active profile;
- VS Code user-data/profile directories;
- extension directory;
- relevant MCP configuration files;
- skill directories;
- DebugMCP global storage path if pre-existing;
- relevant settings keys;
- running localhost listeners;
- breakpoints;
- debug sessions;
- Git status.

Do not perform destructive cleanup.

Define rollback ownership rules:

- remove only artifacts created by the proof;
- preserve pre-existing files in shared directories;
- remove only the newly installed extension;
- stop debug sessions;
- remove temporary breakpoints;
- restore only settings changed by the authorized proof;
- close the MCP listener;
- verify no project files changed.

Do not assume uninstall cleans user-level skill/MCP files unless source/docs prove it.

---

# Prerequisite I: dedicated proof profile feasibility

Assess whether a dedicated VS Code profile would materially isolate the proof.

Answer:

- Can the C# extension, C# Dev Kit, Copilot Chat, and DebugMCP be installed only into a named proof profile?
- Are extension installs profile-specific in the installed VS Code version?
- Are MCP registrations/profile settings profile-specific?
- Are user skill directories shared across profiles?
- Is extension global storage profile-specific or shared?
- Would Tempest workspace trust carry across profiles?
- Would the same Copilot account/session be available?
- Would the exact VSC agent host used for development be reproducible in that profile?
- Would using a proof profile make the result irrelevant to the real development profile?

Recommend:

- dedicated profile;
- current profile;
- or no proof until host identity is resolved.

Do not create a profile.

---

# Prerequisite J: security confirmation

Resolve the security posture for a future proof.

Require:

- `debugmcp.bindHost` remains default loopback only;
- no LAN exposure;
- no blanket auto-approval;
- output capture disabled initially;
- one trusted Tempest workspace;
- one relevant VS Code window where practical;
- no unrelated workspaces open;
- local test data only;
- no secrets inspected;
- explicit timeout for debug operations;
- clean termination;
- breakpoint cleanup;
- listener cleanup.

Determine:

- whether port 3001 or another configured port is currently occupied;
- whether a future proof should use a different loopback port;
- how to confirm the listener is loopback-only;
- how to verify the server has stopped;
- how wrong-workspace routing risk will be mitigated;
- how stale-frame issue #201 will be detected.

Do not start a listener in this assessment.

---

# Decision logic

End with exactly one:

## READY FOR ISOLATED DEBUGMCP PROOF

Use only if all are established:

- exact VS Code profile is known;
- actual Copilot host/tool discovery path is known;
- that host can consume extension-contributed MCP/tools;
- C#/.NET 9 debugging has a viable target path;
- user-level side effects are sufficiently known and inventoryable;
- rollback is precise;
- security controls are adequate.

Provide the exact recommended proof environment and sequence.

or:

## NOT READY

Name only the remaining blockers.

For every blocker state:

- whether Neal can answer it manually;
- whether a small non-install read-only action can answer it;
- whether only installation can answer it;
- whether it materially prevents a safe proof.

Do not use installation as a shortcut for unresolved architecture/security questions.

---

# Future proof design if READY

If READY, specify but do not execute:

## Proof 1: basic .NET debugger capability

Use an existing deterministic test.

Demonstrate:

1. DebugMCP tools appear in the intended Copilot Agent session.
2. Start a .NET debug session.
3. Add a temporary source breakpoint.
4. Hit the breakpoint.
5. Retrieve a nonempty call stack.
6. Inspect a harmless local.
7. Expand an object or inspect a member.
8. Evaluate a harmless expression.
9. Step over.
10. Step into or out where supported.
11. Continue.
12. Terminate cleanly.
13. Remove the breakpoint.
14. Confirm no breakpoints remain.
15. Confirm Git state is unchanged.

## Proof 2: Tempest deterministic workflow

Only after Proof 1 passes:

- use an existing deterministic test or headless playback target;
- stop near a known simulation transition;
- inspect game/actor state;
- perform one or two steps;
- terminate;
- compare usefulness against existing boundary snapshots.

Do not launch interactive Raylib.

Do not attempt a real bug fix during the proof.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-debugmcp-prerequisite-resolution-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-debugmcp-prerequisite-resolution-report.md`

This task does not authorize staging, commit, or push.

Keep these separate from the completed Level 17 closeout.

---

# Required report contents

1. Executive summary
2. Starting Git/worktree state
3. VS Code profile identity
4. VS Code user-data/profile/extension-host paths
5. Local versus remote host
6. Actual Copilot Agent/tool host identity
7. Copilot tool-discovery mechanism
8. MCP discovery mechanism for that host
9. Requirement for Tools picker/new session/workspace trust
10. Bundled Copilot Chat versus separate extension finding
11. Installed extension-contributed tool examples
12. Visibility of those tools to the current host
13. C# Dev Kit/.NET 9 test-discovery result
14. Existing-test run result
15. CLI control-test result
16. C# test-debug readiness/result
17. Headless playback debug-readiness result
18. Minimal future launch/debug configuration
19. DebugMCP installation side effects
20. MCP registration side effects
21. skill-directory side effects
22. extension storage/global-state effects
23. what uninstall removes automatically
24. what rollback must remove manually
25. dedicated-profile feasibility
26. dedicated-profile isolation limitations
27. security posture
28. localhost port/listener plan
29. wrong-workspace mitigation
30. stale-frame detection
31. exact pre-install inventory plan
32. exact rollback plan
33. remaining uncertainties
34. READY/NOT READY result
35. blockers and resolution owner
36. future Proof 1 plan
37. future Proof 2 plan
38. files/resources inspected
39. commands run
40. production code changed: no
41. tests changed: no
42. project configuration changed: no
43. VS Code/user/profile settings changed: no
44. extensions installed/updated/removed: no
45. MCP/skill/launch/task files changed: no
46. debugger sessions launched: state exact result
47. stage/commit/push: none
48. recommended next step

Do not shorten the report because a prerequisite remains unresolved.

---

# Stop conditions

STOP and report rather than guessing if:

- profile metadata cannot be read safely;
- identifying the Copilot host requires secret/session-token access;
- tool discovery cannot be determined without installing an extension;
- C# test discovery would require modifying the project;
- a debug probe risks launching Raylib;
- user-level side effects cannot be scoped safely;
- commands stall or Windows security blocks them;
- any action would persist configuration.

Do not weaken security.

---

# Completion criteria

This prerequisite assessment is complete only when:

- the exact profile is established or narrowly blocked;
- the actual Copilot host is established or narrowly blocked;
- extension-contributed tool discovery is established or narrowly blocked;
- C# Dev Kit/.NET 9 test operation is demonstrated or precisely unresolved;
- headless debug readiness is established;
- DebugMCP side effects are mapped;
- a safe inventory and rollback plan exists;
- security controls are defined;
- READY or NOT READY is issued with exact blockers;
- no installation or persistent configuration change occurred;
- repository and user settings remain unchanged;
- nothing is staged, committed, or pushed.

Then STOP.

TEMPEST-END-DEBUGMCP-PREREQUISITE-RESOLUTION-COMPLETE