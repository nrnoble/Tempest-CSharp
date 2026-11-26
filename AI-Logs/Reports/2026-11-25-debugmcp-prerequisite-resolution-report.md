# Tempest DebugMCP Prerequisite Resolution — Read-Only Assessment

Date: 2026-11-25

## 1. Executive summary

The current environment is a local VS Code Default Profile running a Copilot
SDK agent session. This session can invoke MCP tools associated with installed
extensions: Pylance contributes `pylanceMcp`, and its MCP tools are present in
the current tool inventory; GitLens also starts its GitKraken MCP provider.
That establishes a generic extension-to-agent MCP path, but does **not**
establish that this SDK session consumes the separate user-level
`User/mcp.json` registration DebugMCP writes for VS Code Copilot.

The .NET 9 CLI control passed one existing deterministic xUnit test. VS Code's
test-runner tool reported two controllers but discovered no matching test
items, so C# Dev Kit Test Explorer discovery and test debugging remain
unverified. The current C# debugger is installed and declares `coreclr`, and
official C# Dev Kit documentation supports xUnit test debugging on .NET 6+;
no debugger session was started.

DebugMCP 2.4.4 starts a local MCP endpoint on loopback port 3001 at extension
activation, installs a shared `debug-live` skill automatically, and prompts
the user to select an agent configuration. Its Copilot registration targets
the user-level VS Code MCP file, not a VS Code extension-contributed tool
provider. Its skill installer overwrites an existing `debug-live` directory
and removes two legacy skill directories. Those side effects are now mapped
and can be inventoried, but the SDK-host integration and .NET test-discovery
gaps block a safe proof decision.

No extension was installed; no project source, tests, configuration, user
settings, profile, MCP or skill files were changed. No debugger or game was
launched. One CLI test ran with `--no-restore`; its normal ignored build
outputs were refreshed.

## 2. Starting Git/worktree state

- Branch `master`; HEAD `4c7365a01f944cf2b501164e5015f4954110434f`;
  upstream `origin/master`; ahead/behind `0/0`.
- Initial status had no staged or tracked modifications.
- Pre-existing untracked paths: the prior assessment prompt/report under
  `AI-Logs/` and `Reference/arcade/MBUCOD.V05.txt`. They were preserved.
- No protected ignored directory was enumerated or opened. The ROMs,
  recordings, screenshots, `TEMP/`, `UserData/`, and videos were not inspected.
- This task adds only its required prompt and report under `AI-Logs/`; both
  remain unstaged. The test refreshed ignored `bin/obj` build outputs.

## 3. VS Code profile identity

The active window is the **Default Profile**, with high confidence: all
processes use the standard `%APPDATA%\Code` user-data directory, no `--profile`
argument was present, `User/profiles/profiles.json` is absent, and the only
profile-storage subdirectory is VS Code's `builtin` directory. There is no
named profile ID to report. VS Code's profile documentation says the Default
Profile is the current configuration and has no displayed profile name.

## 4. VS Code user-data, profile, and extension-host paths

- User data: `C:\Users\nrnob\AppData\Roaming\Code`
- Default profile data: `C:\Users\nrnob\AppData\Roaming\Code\User`
- Default profile global storage: `...\Code\User\globalStorage`
- User extensions: `C:\Users\nrnob\.vscode\extensions`
- Current working directory/workspace: `C:\Users\nrnob\source\repos\Tempest`
- `code --status` showed one open VS Code window for `Tempest` and a local
  extension host. C# and C# Dev Kit processes ran locally; no Remote, WSL,
  SSH, container, or remote extension host was indicated.
- The default profile is shared across workspaces; workspace storage has 21
  entries. Their names/content were not enumerated.

## 5. VS Code and relevant extension versions

VS Code is `1.145.1`, x64, commit `6bab64e2d12b04d04c0d99c27ff166023efd1dda`.
Relevant installed versions are C# `2.160.4`, C# Dev Kit `3.40.215`,
Pylance `2026.4.1`, and GitLens `19.7.0`. No DebugMCP extension is installed.

The application bundles `GitHub.copilot-chat` version `0.73.0` under its
built-in `copilot` extension directory. No separate `GitHub.copilot` extension
was listed in the user extension directory. The bundled Copilot Chat package
is sufficient for ordinary VS Code Chat's tool APIs; this does not establish
that the current SDK session is that Chat host.

## 6. .NET inventory and target

The selected SDK is `9.0.304`; SDKs `8.0.424` and `9.0.303` are also installed.
The installed .NET runtimes include `Microsoft.NETCore.App 9.0.7` and `9.0.8`.
The test project targets `net9.0`, uses xUnit `2.9.2`,
`Microsoft.NET.Test.Sdk 17.12.0`, and `xunit.runner.visualstudio 2.8.2`.
There is no `global.json`, `Directory.Build.props`, or
`Directory.Packages.props` in the checked project scope.

## 7. Actual Copilot agent/tool host

The current conversation is a **Copilot SDK agent-host session in VS Code**,
with the SDK-provided tool registry. It is not established as an ordinary
GitHub Copilot Chat Agent session, Copilot CLI process, or VS Code cloud
coding-agent session. The session's available tools include local file and
PowerShell tools, Pylance MCP tools, GitKraken tools, and GitHub MCP tools.
No DebugMCP tool is present.

The SDK host identity is known at that level, but there is no exposed
read-only API here to inspect the Copilot Chat session target or prove that
the SDK consumes every MCP configuration file used by ordinary VS Code Chat.

## 8. Extension-contributed tool discovery

Generic extension MCP discovery is demonstrated, with an important boundary:

- The installed Pylance manifest contributes the MCP server definition
  provider `pylanceMcp`; the current agent tool list contains
  `pylance-mcp-server` tools.
- GitLens contributes `gitlens.gkMcpProvider`; the active VS Code process
  status showed GitLens-started `gk.exe mcp --host=vscode --source=gitlens`
  processes, and the current tool inventory includes GitKraken tools.
- Neither provider is a DebugMCP installation or proof that arbitrary new
  user MCP registrations are loaded into an already-running SDK session.
- No installed extension's `languageModelTools` contribution was established
  as present in this SDK tool registry. The demonstrated example is MCP.

Thus, this host can invoke at least the existing extension-associated MCP
tools. It is not proven that it will consume DebugMCP's registration.

## 9. Copilot tool discovery, picker, session, and trust

Official VS Code documentation distinguishes built-in, MCP, and
extension-contributed Language Model Tools. VS Code Agent mode can invoke
extension tools; MCP servers are registered in the Copilot/VS Code MCP
configuration and subject to server trust and tool approval.

DebugMCP's Copilot target is `%APPDATA%\Code\User\mcp.json`. The current SDK
session has no exposed Tools picker or command to refresh its MCP registry.
For ordinary VS Code Copilot Chat, tools can be enabled/disabled in the Chat
view, the user must configure the server and respond to trust/approval
prompts, and the MCP documentation directs users to choose the Copilot
configuration scope. These are host/configuration controls, not evidence that
the current SDK session reads the same file. Whether this SDK host reads that
user file, requires a new session, or has an equivalent picker is unresolved.
Tools can consequently appear in ordinary VS Code Chat without being exposed
to a distinct SDK tool registry. A fresh session and an explicit low-impact
tool-list check would be prudent in any later authorized proof.

The C# Dev Kit is active in the local window. The workspace's trust state was
not safely exposed by the available tools. DebugMCP's reviewed manifest has
no `capabilities.untrustedWorkspaces` declaration; the workspace must be
trusted for activation/debugging. Do not infer trust from the fact that the
workspace is open.

## 10. C# Dev Kit and .NET 9 test discovery/run

The C# and C# Dev Kit extensions are installed and running locally. Their
logs recognize the Tempest test project path and Test Explorer watches the
test assembly output, but those facts do not establish discovered test items.
The official C# testing page documents xUnit and states .NET 6.0 SDK or later;
the installed C# Dev Kit manifest does not declare a different SDK floor.
No `global.json`, `Directory.Build.props`, or `Directory.Packages.props` was
found at the repository root or along the test project's directory chain;
the project therefore uses VSTest through `Microsoft.NET.Test.Sdk` and the
xUnit Visual Studio adapter.

The available VS Code test runner reported **two registered controllers**,
but for the selected test it reported no discovered items and `discovery:
unknown`. It did not run a Test Explorer test. Parameterized test display was
not verified. This is a discovery result from the current host tooling, not
proof that C# Dev Kit itself cannot discover tests.

## 11. Existing-test CLI control

The focused CLI control **PASS**:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName=Tempest.Game.Tests.AtariPresentationCalibrationTests.LevelThirteenDepthGainIsAcceptedAndIndependentOfOtherOverrides" --verbosity minimal
```

SDK `9.0.304` selected VSTest `17.14.1`; one test was matched and passed
(1 passed, 0 failed, 31 ms). No restore or package change was performed. The
test constructs game/rendering state but does not initialize Raylib. The
existing C# Dev Kit Test Explorer execution path remains unverified.

## 12. C# test-debug readiness

The installed C# debugger manifest contributes `coreclr` (and other) debugger
types. Official C# Dev Kit documentation supports run/debug of discovered
test cases, including xUnit. The project is `net9.0`, and .NET 9 is installed.
No safe SDK command in this session starts a **manual VS Code test-debug**
session without an already discovered Test Explorer item. No debug session,
breakpoint, `coreclr` test process, call stack, or local inspection was
produced. Test debugging is documented and plausible, but not demonstrated;
the missing test discovery blocks using the selected test for Proof 1.

## 13. Headless playback debug readiness

The prior local capability report establishes the existing headless CLI
arguments as `--headless-playback <recording> --boundary <tick> --json`.
No playback was run for this assessment. DebugMCP's `start_debugging` schema
accepts a source file, working directory, optional `testName`, and optional
`configurationName`; it does not accept inline program arguments. Its
generated default configuration cannot be assumed to supply playback args.

Therefore headless playback requires an existing debug configuration or a
temporary launch configuration. No `.vscode/launch.json` exists, and none was
created. The documented C# Dev Kit F5 project-start path is not by itself
evidence that arbitrary playback args can be supplied dynamically.

## 14. Minimal future headless launch configuration

Only if a later task explicitly authorizes creating a temporary, untracked
launch file, the minimal `coreclr` shape to validate is:

```json
{
  "name": "Tempest headless playback",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}\\src\\Tempest.Game\\bin\\Debug\\net9.0\\Tempest.Game.dll",
  "args": [
    "--headless-playback",
    "<trusted-recording.json>",
    "--boundary",
    "<tick>",
    "--json"
  ],
  "cwd": "${workspaceFolder}"
}
```

This is a candidate configuration, not a proven launch. DebugMCP would be
given the configuration name, not inline args. The first proof should prefer a
single discovered deterministic test and avoid Raylib.

## 15. DebugMCP package/source evidence

Reviewed the public Microsoft/DebugMCP repository README, extension manifest
and source matching version `2.4.4`, including `extension.ts`,
`debugMCPServer.ts`, `agentConfigurationManager.ts`, `agentCatalog.ts`,
`debugSkillInstaller.ts`, `copilotMcpConfig.ts`, and `debugStartup.ts`.
The manifest has VS Code engine `^1.104.0`, activates on startup/debug, and
does not contribute `languageModelTools` or `mcpServerDefinitionProviders`.
It starts its own Streamable HTTP MCP server.

These are source/manifest facts; they are not an installation observation.
Confirm the exact Marketplace VSIX source/version before any future install.

## 16. DebugMCP activation and registration effects

On extension activation, DebugMCP:

- creates a DebugMCP output/log channel;
- reads settings with defaults: loopback bind hosts `127.0.0.1` and `::1`,
  port `3001`, output capture disabled, and a 300-second contributed timeout;
- starts its MCP HTTP service and a per-window loopback control server;
- writes a window-registry record below `context.globalStorageUri` containing
  workspace identity, control port, and a random control token, then refreshes
  its heartbeat; deactivation unregisters it and stops both servers;
- checks existing supported agent configs for obsolete DebugMCP SSE entries
  and may migrate them automatically during activation;
- installs the shared `debug-live` skill automatically; and
- shows an agent-selection popup after startup if its global popup state says
  it has not already been handled.

Selecting “Configure GitHub Copilot” writes a `debugmcp` server entry to
`%APPDATA%\Code\User\mcp.json`. The source also manages
`disabledMcpServers` in the adjacent user `settings.json` if `debugmcp` is
listed there. Copilot CLI configuration instead targets
`%COPILOT_HOME%\mcp-config.json`. The new server registration is user-invoked;
the local server and skill installation are activation-time effects.

The activation migration scans supported agent config paths and rewrites an
existing legacy DebugMCP entry when needed. It does not rewrite unrelated
entries, but any pre-existing DebugMCP config must be inventoried before
activation. Do not select or configure any other agent.

## 17. Skill-directory effects

The extension copies bundled `skills/debug-live` into:

- `%USERPROFILE%\.agents\skills\debug-live` (always); and
- `%COPILOT_HOME%\skills\debug-live` when that home directory exists.

Copy uses `force: true`, so an existing destination can be overwritten. It
also recursively removes sibling legacy skill directories named `debug` and
`really-debug`. These operations are automatic during activation's
configuration migration. The extension's deactivation/uninstall path does
not remove these user-level skill files.

At assessment time `%USERPROFILE%\.agents\skills` did not exist. `COPILOT_HOME`
is set and its target `skills\debug-live` did not exist; its value is
intentionally omitted from this report. Both destinations were checked only
for existence, not enumerated or opened.

## 18. Extension storage, state, logs, caches, and workspace writes

DebugMCP uses `context.globalStorageUri` for `window-registry` files and
`context.globalState` for popup/server-message flags. For the current Default
Profile, its prospective extension storage is under
`%APPDATA%\Code\User\globalStorage\ozzafar.debugmcpextension`; that directory
does not exist. VS Code extension/profile registration, extension package
files, logs, and download caches may also be created. Their exact cleanup is
host-managed and is not guaranteed by DebugMCP source.

The extension does not need to write Tempest source or workspace config to
register its Copilot MCP server. The optional later headless proof would need
a temporary launch config only if explicitly authorized. VS Code uninstall
must not be assumed to remove DebugMCP global storage, user MCP registration,
skill directories, logs, or caches.

## 19. Current MCP/config artifact metadata

Read-only checks found:

- `%APPDATA%\Code\User\mcp.json` absent;
- no workspace `.vscode/mcp.json` or `.vscode/settings.json`;
- no workspace `.vscode/launch.json` or `.vscode/tasks.json`;
- user `settings.json` exists, but no DebugMCP setting keys and no
  `debugmcp` entry in `disabledMcpServers`;
- `%COPILOT_HOME%\mcp-config.json` absent;
- DebugMCP extension global storage absent; and
- no DebugMCP listener was installed or started.

Only allowlisted MCP metadata was emitted; no settings values, tokens, or
other agent credentials were opened or reported.

## 20. Side-effect removal behavior

Deactivation removes the extension's active window registry entry and stops
the services. Source does not provide an uninstall routine for the MCP
registration, `debug-live` skill, global-state storage, logs, or VS Code
download cache. Skill installation is force-copying, and legacy skill
directories are deleted during activation. An uninstall alone is therefore
not a sufficient rollback.

## 21. Pre-install inventory procedure

Before any later authorized proof, capture metadata only:

1. `git status --short --branch --untracked-files=all`, HEAD, upstream, and
   staged/tracked diff; preserve all pre-existing untracked paths.
2. `code --status`, active profile identity, `code --list-extensions
   --show-versions --profile "<profile>"`, user-data/profile roots, extension
   path, and DebugMCP extension storage path.
3. Presence and metadata for `User/mcp.json`, `User/settings.json`'s
   `disabledMcpServers` entry only, workspace `.vscode/mcp.json`, and
   `%COPILOT_HOME%\mcp-config.json`. Never dump whole settings/config files.
4. Presence and file metadata for the two `debug-live` skill destinations
   plus the `debug` and `really-debug` legacy siblings. If any target exists,
   preserve it and do not activate DebugMCP until overwrite/deletion risk is
   separately approved and backed up.
5. Check only for existing `debugmcp` entries in every config path that the
   extension's automatic migration scans. Do not expose unrelated config
   values. If an existing entry may be migrated, preserve its exact prior
   state privately or stop the proof.
6. Record DebugMCP global-storage directory existence, current listeners and
   owning PIDs for port 3001, active debug sessions/breakpoints, workspace
   trust status, open VS Code windows, and exact Tempest workspace path.
7. Keep any necessary private configuration backup local and access-controlled;
   never copy secrets into the report or repository.

This assessment performed only the presence/metadata checks described above;
it did not create an inventory backup.

## 22. Rollback ownership rules

- Stop the debug session; remove only proof-created breakpoints.
- Deactivate/uninstall only the DebugMCP extension instance installed for the
  proof profile. Do not remove extensions from the Default Profile.
- Stop the MCP/control listeners and verify port 3001 is no longer listening.
- Remove only a newly added `servers.debugmcp` entry or Copilot CLI
  `mcpServers.debugmcp` entry. Restore only the prior `disabledMcpServers`
  state if the proof changed it; preserve every unrelated setting/server.
- Remove only skill directories created by the proof. Preserve and restore
  any pre-existing skill content; because activation deletes legacy siblings,
  do not proceed if those exist without an explicit backup/approval.
- Remove the extension global-storage directory only if it was absent in the
  baseline and is proven to contain only proof-created data. Do not delete
  shared cache/log directories wholesale.
- Verify the original profile settings, extension inventory, debug state,
  listener state, and Git state after rollback.

## 23. Dedicated proof-profile feasibility

VS Code supports named profiles with profile-specific extension and MCP
configuration; an extension can be applied to all profiles, so that option
must remain off. A dedicated profile can materially isolate the extension
registration. DebugMCP uses VS Code's `context.globalStorageUri`, which is
scoped to the active profile; confirm the resolved URI before installation
and cleanup.

It does **not** isolate the current DebugMCP source's fixed
`%APPDATA%\Code\User\mcp.json` Copilot target, the user-level
`%COPILOT_HOME%` CLI config, or home-level skill directories. Those are shared
outside the profile. Workspace trust is shared between VS Code and the
Agents window; the documentation reviewed does not prove whether switching
profiles changes Tempest trust state, so verify trust in the proof window.

The same Copilot account may be signed into a new profile, but there is no
evidence the current SDK host/session is reproducible there. A proof in a
different profile would not by itself prove this current SDK session consumes
DebugMCP.

## 24. Security posture and listener plan

- The manifest default is loopback-only, `127.0.0.1` and `::1`; changing
  `debugmcp.bindHost` to a LAN address exposes an unauthenticated debugger.
  Keep the default and verify actual bound addresses after activation.
- Generated Copilot configuration uses an empty `autoApprove` list. Keep it
  empty; approve only individual bounded tool requests.
- `debugmcp.captureDebugOutput` defaults to `false`; leave it off.
- Use an explicit bounded operation budget (for example 120 seconds for test
  startup and 30 seconds for inspection/step calls), not the product's
  300-second default for every operation.
- At check time, no listener was bound to ports 3001, 3002, or 3100. Use the
  default 3001 only after checking it again at proof time; do not expose it
  beyond loopback.
- One Tempest VS Code window was open. Verify workspace trust and close
  unrelated windows before the proof. Do not inspect secrets or enable output
  capture.
- After cleanup, check listener address/port/PID and confirm no DebugMCP
  listener remains.

## 25. Wrong-workspace routing mitigation

DebugMCP issue #191 documents a wrong-window routing risk when the requested
workspace path does not match a registered workspace. Keep one relevant VS
Code window open, explicitly use the Tempest workspace path, and verify the
returned source file/frame belongs to Tempest before evaluating anything. If
the workspace/session identity does not match, stop immediately and classify
the proof as failed; do not continue on a cached target.

## 26. Stale-frame detection

Issue #201 reports the initial paused state can precede VS Code frame
selection. At the first breakpoint, require a nonempty call stack and a
selected frame whose source path and line match the expected Tempest test.
Treat an empty, stale, or mismatched frame as a failed proof; do not inspect
locals or step until the debugger returns a verified frame. Stop the session,
clear proof breakpoints, and record the result.

## 27. Remaining uncertainties

- Whether this exact Copilot SDK host reads DebugMCP's VS Code user MCP file.
- Whether a fresh SDK session/tool refresh is required after registration.
- Whether C# Dev Kit has discovered the specific xUnit test in the actual
  Test Explorer UI; the SDK runner returned `discovery: unknown`.
- Whether Test Explorer can start this test under `coreclr` in the current
  installed extension versions; no debug session was launched.
- Whether the current Tempest workspace is trusted, and whether that trust
  state is reused in another profile.
- Whether a temporary `coreclr` launch config with the shown headless args
  starts successfully; not tested.
- Whether any other supported agent config contains a legacy DebugMCP entry;
  those configurations were not read.
- Whether the Marketplace VSIX has any difference from the reviewed
  version-matched upstream source.

## 28. READY/NOT READY criteria

The existing extension MCP providers establish generic extension-to-SDK MCP
visibility. They do not prove DebugMCP's user-config registration is consumed
by this SDK host. Test execution succeeds through CLI, but discovery/debug
through Test Explorer is unverified. The security controls and inventory/
rollback procedure are defined, but the required host and test-target evidence
is incomplete.

## 29. Future Proof 1 — basic .NET debugger capability

Only after all blockers below are resolved and a later task authorizes the
installation:

1. Use one isolated profile/window and install only DebugMCP 2.4.4 there;
   ensure C# and C# Dev Kit are enabled in that profile.
2. Confirm a fresh intended Copilot SDK Agent session exposes DebugMCP tools;
   do not enable blanket approvals.
3. Select one already discovered deterministic xUnit test. Do not add or
   change tests.
4. Start the test under `coreclr`; add one temporary source breakpoint and
   verify it is hit.
5. Retrieve a nonempty call stack; inspect one harmless local and an object
   member; evaluate a harmless expression.
6. Step over and step into/out where supported; continue; terminate cleanly.
7. Remove the proof breakpoint and verify no proof-created breakpoint remains.
8. Compare Git/profile/config/listener inventory with baseline and require no
   project source, test, or configuration change.
9. Treat wrong workspace, stale frame, missing tool, timeout, or incomplete
   cleanup as **FAIL/BLOCKED**, never as a pass.

## 30. Future Proof 2 — Tempest deterministic workflow

Only after Proof 1 passes and a later task separately authorizes any temporary
launch configuration:

- Prefer a deterministic existing test or headless playback, never the
  interactive Raylib application.
- Use a known local recording and a bounded boundary; stop near a previously
  identified transition.
- Inspect only the relevant game/actor state, step one or two times, then stop.
- Compare against existing boundary snapshots and the current deterministic
  test/playback workflow; do not make a gameplay change during the proof.
- Remove temporary launch configuration, breakpoints, server registration,
  and listeners according to the rollback rules.

## 31. Required prompt/report artifacts

- Prompt: [2026-11-25-debugmcp-prerequisite-resolution-prompt.md](../Prompts/2026-11-25-debugmcp-prerequisite-resolution-prompt.md)
- Report: this file.

## 32. Files/resources inspected

Local repository:

- [2026-11-25-vscode-copilot-development-tool-assessment-report.md](./2026-11-25-vscode-copilot-development-tool-assessment-report.md)
- [2026-11-23-agent-runtime-capability-probe-report.md](./2026-11-23-agent-runtime-capability-probe-report.md)
- [AtariPresentationCalibrationTests.cs](../../tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs)
- [Tempest.Game.Tests.csproj](../../tests/Tempest.Game.Tests/Tempest.Game.Tests.csproj)
- Selected C# Dev Kit, C#, Pylance, GitLens, and bundled Copilot Chat manifests.
- Sanitized VS Code status and selected C# Dev Kit/Test Explorer log matches.
- Selected user MCP/settings metadata only; no secrets or full settings were
  read into the report.

Public documentation/source:

- VS Code Language Model Tool API, MCP server configuration, Profiles,
  Workspace Trust, C# testing, and C# debugging documentation.
- Microsoft/DebugMCP README, version 2.4.4 package manifest, extension/server,
  agent configuration/catalog, Copilot MCP config, skill installer, and
  debug-startup sources.
- DebugMCP issues #191 (routing) and #201 (stale frame), as recorded in the
  required prior report.

The companion user note was read but its interactive launch commands were not
run. No protected ROM, recording, screenshot, video, TEMP, or UserData file
content was opened.

## 33. Commands and probes run

- `git branch --show-current`, `git rev-parse HEAD`,
  `git rev-parse --abbrev-ref --symbolic-full-name '@{upstream}'`,
  `git rev-list --left-right --count HEAD...origin/master`,
  `git status --short --branch --untracked-files=all`.
- `code --version`, `code --list-extensions --show-versions`,
  `code --status`, `code --locate-extension` for selected manifests.
- Read-only process metadata filtered to VS Code profile/remote arguments;
  no process tokens or unrelated command arguments were recorded.
- `dotnet --info`, `dotnet --version`, and one focused
  `dotnet test ... --no-restore --filter ...` control.
- VS Code `runTests` for the one selected test; the tool returned two
  controllers but no matching discovered test.
- Read-only listener check for ports 3001, 3002, and 3100.
- Read-only existence/key-name checks for selected MCP, skill, profile,
  extension-storage, and workspace configuration paths.
- Targeted `rg` on C# Dev Kit logs; no broad protected-directory search.
- Public `web_fetch` of VS Code docs and DebugMCP source/package metadata.

## 34. Production code changed

**No.**

## 35. Tests changed

**No.**

## 36. Project configuration changed

**No.** No project or workspace settings, `.vscode` files, launch files,
tasks, packages, SDK configuration, or MCP files were created or changed.

## 37. VS Code/user/profile settings changed

**No.** Only selected non-sensitive setting keys and metadata were read.

## 38. Extensions installed/updated/removed

**None.**

## 39. MCP/skill/launch/task files changed

**No.** The required prompt/report logs are the only repository files added.

## 40. Debugger sessions launched

**None.** No breakpoint was added or changed. No Raylib/game process or
headless playback process was launched.

## 41. Stage/commit/push

**None.** Nothing was staged, committed, or pushed.

## 42. Validation outcomes

- **PASS:** focused CLI control test, 1/1.
- **BLOCKED/UNKNOWN:** VS Code test discovery; the selected test did not appear
  in the runner's discovered test items.
- **NOT RUN:** Test Explorer execution/debug session, DebugMCP, headless
  playback, and game launch.
- **PASS:** no installation or persistent configuration change occurred.

## 43. Test/debug impact

The CLI control confirms a stable net9.0 xUnit test can run on SDK 9.0.304.
It does not prove C# Dev Kit test discovery, parameterized display, or
manual debugging. The current agent cannot start or control a C# debugger.

## 44. Security impact

No endpoint was started and no user configuration was changed. A future
proof must retain loopback binding, empty auto-approval, disabled output
capture, one trusted Tempest window, explicit timeouts, local test data,
verified cleanup, and a before/after inventory.

## 45. Profile recommendation

Do not create a profile in this assessment. For a future authorized proof, a
dedicated profile is preferable for extension enablement/global storage, but
it does not isolate the DebugMCP user MCP file or shared home skill
directories. Use it only after confirming the same intended Copilot SDK host
can be reproduced there; otherwise the result would not validate the current
development session.

## 46. User-side-effect recommendation

Before any activation, inventory the supported config paths for legacy
`debugmcp` entries and ensure the `debug-live`, `debug`, and `really-debug`
skill targets are absent or separately backed up/approved. Activation itself
starts a privileged local endpoint and writes skill files, even before a user
selects Copilot configuration.

## 47. Recommended next step

Resolve the two material blockers without installing anything: verify how this
Copilot SDK host refreshes/consumes the VS Code user MCP registration, and
confirm that the installed C# Dev Kit discovers the selected test in the
Tempest Test Explorer. Also confirm Tempest workspace trust in the intended
proof window. Reassess after those checks; no DebugMCP installation is
authorized by this report.

## 48. Blocker ownership

- **Copilot SDK versus DebugMCP registration:** Neal can identify the intended
  VS Code Copilot session target, but cannot establish config compatibility
  by assertion alone. A bounded non-install host/tool-registry inspection may
  resolve it. Only an installation could empirically test this exact product
  integration; do not use installation merely to guess. This materially
  blocks the proof.
- **C# Dev Kit discovery/debug:** Neal can inspect the current Test Explorer;
  the current non-install test-controller query returned no items, and a
  focused read-only discovery check may resolve it. Installation is not
  needed. The current SDK test runner did not discover the target. This
  materially blocks Proof 1.
- **Workspace trust:** Neal can verify the trust indicator in the intended
  window; a read-only UI check is sufficient and installation is not needed.
  This must be confirmed before activation but is not a product compatibility
  blocker.
- **Headless playback args:** no install is needed to review the temporary
  launch configuration; actual debugger launch would require separate
  authorization. This is not a blocker to the initial test-only Proof 1.

## NOT READY

- The current Copilot SDK host's consumption of DebugMCP's user-level Copilot
  MCP registration is not established.
- C# Dev Kit discovery and manual debugging of the existing net9.0 test are
  not demonstrated; VS Code test discovery returned no matching item.
- Tempest workspace trust must be confirmed before any extension activation.
