# Tempest Controlled DebugMCP Installation and Proof of Capability

Date: 2026-11-25

## Current continuation checkpoint — 2026-11-25 21:39 PST

The user-provided task reconfirms the manual prerequisites: the Tempest folder
is trusted, C# Dev Kit discovers `Tempest.Game.Tests (net9.0)` and its xUnit
tests, Test Explorer runs passed, and `Debug Test` is available. These resolve
the earlier trust and test-discovery uncertainties, but do not identify the
owners of the existing processes listed below.

The baseline was rechecked before any installation or activation:

- Branch `master`, HEAD `4c7365a01f944cf2b501164e5015f4954110434f`,
  upstream `origin/master`, ahead/behind `0/0`; no staged or tracked changes.
- The existing task prompt and report are already untracked from the earlier
  attempt. Other existing untracked paths remain the prerequisite and
  development-tool prompt/report pairs and
  `Reference/arcade/MBUCOD.V05.txt`; none was removed or staged.
- `code --status` shows one local VS Code window titled
  `ApplicationPathsTests.cs - Tempest`, with the Tempest workspace. The
  installed extensions include C# `2.160.4` and C# Dev Kit `3.40.215`;
  DebugMCP is absent.
- User and workspace MCP files, the prospective DebugMCP global-storage
  directory, and the DebugMCP extension directory are absent. The
  `disabledMcpServers` key is absent. The expected user/Copilot-home
  `debug-live`, `debug`, and `really-debug` skill targets are absent; the
  `%COPILOT_HOME%` value is intentionally omitted.
- No listener is present on ports 3001, 3002, or 3100. The four ignored local
  asset roots checked at metadata level remain ignored; their contents were
  not inspected.
- These pre-existing processes are still running: `dotnet.exe` PID `14356`,
  child of `Microsoft.VisualStudio.Code.ServiceHost.exe`, started
  `2026-11-25 21:08:38`; and `dotnet.exe` PID `74600`, started
  `2026-11-25 21:10:36` (its parent process is no longer present). The C# Dev
  Kit project-system host `dotnet.exe` PID `43328` is also present, started
  `2026-11-24 12:36:46`. Process command lines were not read. The first two
  processes match the previously observed VSTest/MSBuild activity; this
  recheck does not establish whether the user still needs them.

**Disposition: BLOCKED before installation.** The active VSTest/MSBuild
baseline discrepancy remains unresolved. No process was started or stopped by
this continuation, no extension package was retrieved, and no user or
workspace configuration was changed. Do not install or activate DebugMCP
until Neal confirms the processes are no longer active user work (for example,
they completed normally) or stops the associated test/build through its
normal VS Code workflow. Then repeat the pre-install process/listener check.

### Recheck after user reported normal stop

Neal selected “Stopped normally; recheck now.” The immediate recheck still
found `dotnet.exe` PID `14356` (parent `Microsoft.VisualStudio.Code.ServiceHost.exe`)
and `dotnet.exe` PID `74600` (the parent process is absent). Both remain
present with their original start times. No process command lines were read,
and no process was terminated. The loopback-related ports remain clear and
the Git state remains unchanged. The discrepancy is unresolved; keep the task
blocked before package retrieval, installation, or activation until these
processes exit or their state is reconciled.

### User-requested fresh-session handoff

Neal reports that Test Explorer shows no active tests and that the previous
tests completed normally. Neal will close VS Code normally to clear any
remaining C# Dev Kit/VSTest/MSBuild workers. Per Neal's instruction, do not
terminate or otherwise modify those processes. Stop this session here and
wait for a fresh session.

**Continuation checkpoint:** No DebugMCP package has been retrieved or
installed; the extension is absent, no activation or Copilot registration has
occurred, and no debugger/breakpoint was created. The required initial Git,
extension, MCP, skill, global-storage, and listener metadata checks are
recorded above. The only task-file change so far is this report checkpoint;
the prompt file already existed. In the fresh session, read this report and
the prerequisite-resolution report, then perform only a narrow post-restart
recheck of the VSTest/MSBuild processes and the DebugMCP extension/listener
state. Do not repeat the broad tooling survey. If the process discrepancy is
cleared and no unexpected state changed, continue at exact Marketplace
2.4.4 package verification and the authorized installation/activation flow.
Do not assume DebugMCP is installed and do not install a second copy if the
post-restart inventory shows it present; first reconcile that state.

## Status: BLOCKED before installation

No DebugMCP package was retrieved or installed, no activation or MCP
registration occurred, and no debugger session was started. The pre-install
inventory found a running `vstest.console.dll` process and an MSBuild process
that were already present before this task. Their ownership and whether they
represent active user work are unresolved. Per the task's baseline gate, stop
here and reconcile that state with Neal before proceeding.

Neal's response to the ownership check was **uncertain**. The process state
therefore remains unresolved; no further installation or debugger work is
authorized until the processes are identified or have exited normally.

## Required report fields

1. **Executive summary:** BLOCKED before installation because an existing
   test/build process is active. The observed test process is a VS Code
   Service Host child; it was not started, stopped, or modified by this task.
2. **Starting Git/worktree state:** Branch `master`; HEAD
   `4c7365a01f944cf2b501164e5015f4954110434f`; upstream `origin/master`;
   ahead/behind `0/0`; no staged or tracked changes. Pre-existing untracked
   files were the prerequisite prompt/report pair, the development-tool
   assessment prompt/report pair, and `Reference/arcade/MBUCOD.V05.txt`.
   The local arcade reference was not opened or changed. The required prompt
   copy for this task has been added; this report is the paired artifact.
3. **Manual prerequisite evidence:** Neal confirms the Tempest folder is
   trusted, C# Dev Kit discovers `Tempest.Game.Tests (net9.0)` and its xUnit
   tests, tests have passed through Test Explorer, and the `Debug Test`
   context-menu action is available.
4. **Active profile and workspace:** `code --status` showed one local VS Code
   window titled `AtariPresentationAdapterTests.cs - Tempest`; the standard
   user-data path and absence of a `--profile` process argument are consistent
   with the Default Profile. The active task workspace is
   `C:\Users\nrnob\source\repos\Tempest`. No remote extension host was
   established. User confirmation of trust resolves the trust prerequisite.
5. **Pre-install extension inventory:** Full Default Profile user-extension
   inventory captured. Relevant entries: `ms-dotnettools.csharp 2.160.4`,
   `ms-dotnettools.csdevkit 3.40.215`,
   `ms-dotnettools.vscode-dotnet-runtime 3.2.0`,
   `ms-dotnettools.vscodeintellicode-csharp 2.2.3`,
   `eamodio.gitlens 19.7.0`, and `ms-vscode.powershell 2025.4.0`.
   `ozzafar.debugmcpextension` is not installed. The bundled Copilot Chat
   extension is not part of the user-extension listing.

   Complete user-extension inventory:
   `aaron-bond.better-comments 3.2.0`; `adrianwilczynski.csharp-to-typescript
   1.12.1`; `alexcvzz.vscode-sqlite 0.14.1`; `anthropic.claude-code 2.1.332`;
   `arcanis.vscode-zipfs 3.0.0`; `battlelinegames.assemblyscript-lang 0.3.2`;
   `continue.continue 2.0.0`; `cweijan.vscode-autohotkey-plus 2.7.2`;
   `dbaeumer.vscode-eslint 3.0.34`; `docker.docker 0.19.1`;
   `dtsvet.vscode-wasm 1.4.1`; `eamodio.gitlens 19.7.0`;
   `eliostruyf.vscode-hide-comments 1.10.0`; `esbenp.prettier-vscode
   12.4.0`; `firsttris.vscode-jest-runner 0.4.151`;
   `formulahendry.code-runner 0.12.2`; `github.remotehub 0.66.0`;
   `ms-azuretools.vscode-containers 2.5.3`; `ms-azuretools.vscode-docker
   2.0.0`; `ms-dotnettools.csdevkit 3.40.215`; `ms-dotnettools.csharp
   2.160.4`; `ms-dotnettools.vscode-dotnet-runtime 3.2.0`;
   `ms-dotnettools.vscodeintellicode-csharp 2.2.3`;
   `ms-edgedevtools.vscode-edge-devtools 2.1.14`; `ms-python.debugpy
   2026.6.0`; `ms-python.isort 2026.8.0`; `ms-python.python 2026.10.0`;
   `ms-python.vscode-pylance 2026.4.1`; `ms-python.vscode-python-envs
   1.44.0`; `ms-toolsai.jupyter 2025.9.1`; `ms-toolsai.jupyter-keymap
   1.1.2`; `ms-toolsai.jupyter-renderers 1.3.0`;
   `ms-toolsai.vscode-jupyter-cell-tags 0.1.9`;
   `ms-toolsai.vscode-jupyter-slideshow 0.1.6`;
   `ms-vscode-remote.remote-containers 0.481.0`;
   `ms-vscode-remote.remote-ssh 0.128.0`;
   `ms-vscode-remote.remote-ssh-edit 0.87.0`;
   `ms-vscode-remote.remote-wsl 1.104.7`;
   `ms-vscode-remote.vscode-remote-extensionpack 0.26.0`;
   `ms-vscode.azure-repos 0.40.0`; `ms-vscode.powershell 2025.4.0`;
   `ms-vscode.remote-explorer 0.5.0`; `ms-vscode.remote-repositories
   0.44.0`; `ms-vscode.remote-server 1.5.4`;
   `ms-vscode.vscode-typescript-next 6.0.20260416`; `ollama.ollama 0.0.14`;
   `orta.vscode-jest 6.4.4`; `ritwickdey.liveserver 5.7.10`;
   `rust-lang.rust-analyzer 0.3.3130`; `saulecabrera.asls 0.4.0`.
6. **Pre-install MCP/config metadata:** `%APPDATA%\Code\User\mcp.json`,
   workspace `.vscode\mcp.json`, and `%COPILOT_HOME%\mcp-config.json` are
   absent. User `settings.json` exists (469 bytes; last modified
   2026-11-23 21:02:22 UTC); its `disabledMcpServers` key is absent.
   Configuration values and unrelated MCP entries were not printed.
7. **Pre-install skill metadata:** Both user-home and Copilot-home
   `debug-live`, `debug`, and `really-debug` targets are absent. The
   `%COPILOT_HOME%` value is intentionally not recorded.
8. **Pre-install global-storage metadata:** The prospective
   `ozzafar.debugmcpextension` global-storage directory is absent.
9. **Pre-install listener/process metadata:** No listener was found on ports
   3001, 3002, or 3100. Three `.NET` processes were observed: the C# Dev Kit
   project-system build host (PID 43328, started 2026-11-24 20:36:46 UTC);
   `vstest.console.dll` (PID 14356, parent `Microsoft.VisualStudio.Code.ServiceHost.exe`,
   started 2026-11-26 05:08:38 UTC); and `MSBuild.dll` (PID 74600, started
   2026-11-26 05:10:36 UTC). The VSTest and MSBuild processes predate this
   task. Their state/ownership is unresolved. No DebugMCP process was found.
   Breakpoints are not safely inspectable through the available tools.
10. **Exact extension identity/version/source verification:** Not performed
    in this run because installation was blocked at baseline reconciliation.
    The prerequisite report records prior review of Marketplace identity
    `ozzafar.debugmcpextension`, version `2.4.4`, publisher display name
    Microsoft DevLabs, and source repository `microsoft/DebugMCP`; package
    retrieval and package-manifest verification remain mandatory before any
    installation.
11. **Installation method and result:** Not attempted.
12. **Activation result:** Not attempted.
13. **Activation-time side effects:** None attributable to DebugMCP.
14. **Skill files created/changed:** None.
15. **MCP files created/changed:** None.
16. **Global storage created:** None.
17. **Listener addresses, ports, and owning PID:** No listener on ports
    3001/3002/3100; no DebugMCP listener PID.
18. **Loopback-only verification:** No DebugMCP listener existed to verify.
19. **Agent-selection action:** None; no prompt occurred.
20. **GitHub Copilot registration result:** Not attempted.
21. **Current-session tool discovery:** DebugMCP tools are absent from this
    session's current tool inventory; no registration was made in this task.
22. **Fresh-session/tool-refresh requirement:** Not determined; registration
    has not been attempted.
23. **Human interactions required:** No VS Code prompt was reached. Neal's
    process-ownership reconciliation is required before proceeding.
24. **Selected deterministic test:** None selected. Although Neal confirms
    Test Explorer discovery and `Debug Test` availability, the active
    VSTest/MSBuild state must be reconciled first.
25. **Breakpoint file/line/method:** None added.
26. **Debug-session startup:** Not attempted.
27. **Debug-adapter/process identity:** No debugger adapter started.
28. **First-stop source/frame validation:** Not run.
29. **Call-stack result:** Not run.
30. **Local-variable result:** Not run.
31. **Object/member result:** Not run.
32. **Expression-evaluation result:** Not run.
33. **Step-over result:** Not run.
34. **Step-into/out result:** Not run.
35. **Continue/test-completion result:** Not run.
36. **Debug-session termination:** No session started by this task.
37. **Breakpoint cleanup:** No breakpoint created; existing breakpoint state
    was not safely inspectable or changed.
38. **Orphan-process check:** No process launched by this task. The observed
    VSTest/MSBuild processes remain unclassified; do not terminate them as part
    of this task without resolving ownership.
39. **Repository-integrity result:** No production, test, project, tracked
    configuration, recording, screenshot, video, or ROM/reference file was
    modified. Pre-existing untracked files were preserved.
40. **Configuration-integrity result:** No VS Code user, workspace, MCP,
    Copilot, or agent configuration was changed.
41. **Security result:** No DebugMCP endpoint or debugger was started; no
    configuration secrets or protected assets were inspected.
42. **Proof classification:** **BLOCKED** before installation.
43. **Demonstrated Tempest value:** None; no debugger capability was tested.
44. **Known limitations:** Active VSTest/MSBuild process ownership is
    unresolved. The current SDK agent host's use of a user MCP registration
    remains unproven. Exact Marketplace package verification has not been
    repeated in this run.
45. **Keep-versus-rollback decision:** No extension was installed, so there is
    nothing to keep or roll back.
46. **Rollback actions:** None required; no proof-created system artifacts
    exist.
47. **Final listener state:** No listener on ports 3001/3002/3100 at inventory
    time.
48. **Final extension inventory:** No DebugMCP extension installed; the
    Default Profile inventory is listed above.
49. **Final MCP/skill/global-storage state:** All inspected DebugMCP
    registrations, skill targets, and extension storage remain absent.
50. **Proposed operational policy if later retained:** Use deterministic
    tests/playback to localize first; inspect live state only after the failing
    boundary is known; use explicit debugger bounds; verify workspace and
    frame before inspection; keep capture disabled and auto-approval empty;
    never inspect secrets; clear temporary breakpoints and terminate every
    session; treat debugging as supplemental to tests and Neal's manual
    acceptance.
51. **Recommended next proof:** After the active process state is reconciled,
    re-run the baseline gate, verify the exact Marketplace VSIX identity and
    version, then attempt only the authorized DebugMCP 2.4.4 installation and
    one existing deterministic C# test.
52. **Production source changed:** No.
53. **Tests changed:** No.
54. **Tracked project configuration changed:** No.
55. **Gameplay recordings changed:** No.
56. **Stage/commit/push:** None.
57. **Final Git/worktree state:** No tracked changes; pre-existing untracked
    files remain. This task's prompt and report are the only new repository
    artifacts; nothing is staged.
58. **Recommended next step:** Confirm whether the active VSTest and MSBuild
    processes are expected/current work. If they are active, allow them to
    finish or stop them through the normal VS Code/test workflow, then tell me
    when that is done. They were not launched or terminated by this task.

## Validation and manual acceptance

- **PASS:** Completion token check found exactly one standalone token:
  `TEMPEST-END-DEBUGMCP-CONTROLLED-INSTALLATION-PROOF-COMPLETE`.
- **PASS:** Initial Git baseline and pre-install DebugMCP path/listener
  metadata were collected without changing user configuration.
- **BLOCKED:** Installation and all debugger proofs stopped at the active
  process baseline discrepancy.
- **NOT RUN:** Tests, build, game launch, install, activation, registration,
  debugger, and Marketplace package retrieval.
