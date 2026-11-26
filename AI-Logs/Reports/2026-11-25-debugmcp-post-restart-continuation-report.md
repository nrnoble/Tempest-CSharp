# DebugMCP Post-Restart Continuation

Date: 2026-11-25

## Baseline reconciliation

- **PASS:** Old VSTest PID 14356, old MSBuild PID 74600, and old project
  host PID 43328 are absent. The process discrepancy is reconciled.
- New project/build host PID 54084 started at 21:57:55 as a child of the
  C# Dev Kit ServiceHost. MSBuild PIDs 45936 and 62424 started at 21:57:57
  as reusable node-mode children of that host. These are new infrastructure,
  not the old unresolved processes. No processes were stopped.
- **PASS:** DebugMCP is absent from the extension inventory and extension
  directory. No listeners on ports 3001, 3002, or 3100.
- **PASS:** User and workspace MCP files, Copilot-home MCP file, DebugMCP
  global-storage directory, and both sets of `debug-live`, `debug`, and
  `really-debug` skill targets are absent. `disabledMcpServers` and DebugMCP
  setting keys are absent; user settings metadata is unchanged.
- **PASS:** Branch `master`, HEAD
  `4c7365a01f944cf2b501164e5015f4954110434f`, `origin/master`, 0/0;
  index and tracked worktree unchanged. Prior untracked files were preserved.
- One local Tempest window is reported. VS Code reports version 1.148.0,
  whereas the earlier prerequisite report recorded 1.145.1; C# 2.160.4
  and C# Dev Kit 3.40.215 remain unchanged. The version difference is
  recorded, not treated as evidence of debugger compatibility.

## Package verification and installation

**PASS:** The official Visual Studio Marketplace version-specific
`ozzafar/debugmcpextension/2.4.4/vspackage` endpoint supplied a 27,329,980-byte
VSIX in the session workspace. Both its extension manifest and VSIX metadata
identify publisher `ozzafar`, extension `debugmcpextension`, and version
`2.4.4`; its repository URL is `https://github.com/microsoft/DebugMCP.git`
and VS Code engine requirement is `^1.104.0`. This is package metadata
verification, not a byte-for-byte source equivalence claim. The installed
VS Code 1.148.0 satisfies the declared engine requirement.

**PASS:** `code --install-extension` of that verified VSIX returned exit 0.
The installed inventory reports `ozzafar.debugmcpextension@2.4.4`; the
extension directory is `ozzafar.debugmcpextension-2.4.4`. No explicit
profile argument, force option, or apply-to-all-profiles option was used.
The CLI printed a Node deprecation warning but no installation error or
reload requirement.

## Activation and security audit

- Activation is evidenced by new DebugMCP global storage, one window-registry
  record, the two newly created `debug-live` skill directories, and a listener.
  Their exact contents, including registry tokens, were not inspected.
- PID 72876 (`Code.exe`) listens on `127.0.0.1:3001` and `[::1]:3001`.
  All other listening sockets owned by that shared process were also on
  `127.0.0.1`; ownership of individual additional ports by DebugMCP versus
  other extensions is not established.
- Home and Copilot-home legacy `debug` and `really-debug` targets remain
  absent; those targets were absent before activation as well.
- User and workspace MCP files and Copilot-home MCP file remain absent.
  User settings metadata is unchanged; `disabledMcpServers` and DebugMCP
  setting keys remain absent. No Copilot registration was made.
- The output channel and the actual visibility of an agent-selection prompt
  were not safely observable through the available non-UI checks. Based on
  the reviewed source, a first-activation agent-selection prompt is possible;
  its presence is not asserted. No UI automation was used.

## Handoff: PARTIAL, awaiting human observation

The extension is installed and active; the proof is not complete. Before
acting on or assuming an agent-selection dialog, Neal should check the
Tempest window and report whether one is visible. Leave the window open and
do not select an agent until its actual state is confirmed. Do not reinstall
DebugMCP. If the selection prompt is visible, the only authorized agent is
GitHub Copilot; the user must select it explicitly in a later step.

The extension's exact loopback listener was verified, but registration,
tool discovery, debugger startup, breakpoint, test, inspection, stepping,
and cleanup are **NOT RUN**. No proof-created debugger session or breakpoint
exists. The extension, two newly created skills, global storage/registry, and
listener remain in place pending the continuation; they are owned by this
proof for a narrowly scoped rollback if needed. No keep-or-rollback decision
has yet been made.

The user confirmed workspace trust and C# Dev Kit test discovery/execution
in the earlier checkpoint; these are manual evidence, not newly repeated
tests. No interactive game was launched.

No Tempest source, tests, gameplay, tracked configuration, or protected
assets were changed. The tracked worktree and index remained unchanged;
the prior untracked files were preserved and this continuation added its
prompt and report. Nothing was staged, committed, or pushed.

## Follow-up: explicit agent-selection command

Neal reports DebugMCP 2.4.4 is visible in Extensions, with no visible popup,
notification, agent-selection dialog, or other configuration prompt.

The installed extension's README documents the Command Palette command
`DebugMCP: Show Agent Selection Popup`. Its manifest declares
`debugmcp.showAgentSelectionPopup` with category `DebugMCP` and title
`Show Agent Selection Popup`. The installed implementation opens a
multi-select quick pick titled `DebugMCP Setup - Choose AI Agent to
Configure`; items are initially unselected. Registration happens after
the user accepts selected items, not simply from opening the picker.

**PAUSED:** Neal should invoke the documented command, confirm the picker
appears, and report its state without selecting or accepting anything yet.
The only authorized eventual selection is `Configure GitHub Copilot`, not
`Configure GitHub Copilot CLI` or any other agent. No configuration command,
selection, registration, debugger operation, or installation was performed
in this follow-up. The tracked worktree and index remain unchanged.

Neal's screenshot confirms the agent picker is open with zero selected.
The next authorized human action is to select only `Configure GitHub
Copilot` and accept, then report the resulting notification or prompt.

## Copilot registration and tool discovery

Neal subsequently supplied a screenshot of the opened user MCP file.
Metadata verification confirms its only server is `debugmcp`, with URL
`http://localhost:3001/mcp`, an empty `autoApprove` list, and
`disabled: false`. The workspace MCP file and Copilot CLI MCP file remain
absent; user settings metadata is unchanged. The listener remains
`127.0.0.1:3001` and `[::1]:3001`, PID 72876.

The current Copilot SDK session discovers DebugMCP tools. Read-only calls to
`get_debug_status` and `list_breakpoints` succeeded: no active debug session
and no breakpoints. Thus no fresh session was needed for this particular host.
The installed `debug-live` skill was loaded for this proof.

**Schema warning:** VS Code reports four diagnostics on the generated user
MCP entry: `streamableHttp` is not accepted by its schema (valid values
reported are `http` and `sse`), and `autoApprove`, `disabled`, and `timeout`
are not allowed properties. Despite these diagnostics, the SDK tools were
discovered and the two read-only calls succeeded. Ordinary VS Code Chat
integration is not established; no configuration was modified to hide the
warnings.

**BLOCKED before debugger startup:** The `start_debugging` tool schema has
no per-call timeout. Its implementation uses `debugmcp.timeoutInSeconds`,
which is read at activation and defaults to 300 seconds, while the
authorized proof requires an explicit 120-second startup bound. A temporary
user setting plus extension reload may be required; neither has been
performed. Do not start the test or create a breakpoint under the default
300-second bound. Request explicit authorization before changing a shared
user setting; if authorized, preserve its original absence and require a
human-controlled reload and subsequent verification. No test, debugger
session, or breakpoint was created. No production, test, or tracked
configuration changes; no staging, commit, or push.

### Timeout authorization and reload handoff

Neal explicitly authorized a temporary shared VS Code user setting of
`debugmcp.timeoutInSeconds: 120`, to be restored to its original absence
after the proof. One line was inserted into user `settings.json`; verification
found exactly one matching entry, preserved CRLF line endings, no
`disabledMcpServers` key, and the original 469 bytes after excluding the
inserted line. No other user settings were intentionally changed.

The extension reads this setting on activation. **PAUSED:** Neal must reload
the current Tempest VS Code window once, then return to this conversation or
open a fresh one if necessary. Afterward, verify the extension's effective
120-second timeout, loopback listener, session/tool availability, and
breakpoint/debug state before starting the bounded proof. The user MCP
registration and the timeout setting remain in place; do not reinstall.
The original setting was absent, so remove only the proof-created setting
afterward and verify other user settings remain intact.

The generated MCP schema diagnostics remain unresolved, although read-only
DebugMCP tool calls succeeded in this SDK session. No debugger test has run.

### Post-reload proof baseline

Neal confirmed the window reload. The previous listener process 72876 is
gone; new `Code.exe` listener process 31496 started at 22:25:34 and binds
`127.0.0.1:3001` and `[::1]:3001`. One Tempest window is reported.
The user timeout key appears exactly once with value 120 and no workspace
settings file or language-scoped timeout override exists. The reviewed
extension implementation reads this setting when activated; together with
the new listener process this supports the 120-second startup bound, though
the extension's activation output channel was not independently retrieved.
DebugMCP status is `no-session` and its breakpoint list is empty. The
extension remains version 2.4.4 and the tracked worktree and index are clean.

### Proof attempt and rollback

**FAIL:** A breakpoint was added at
`tests/Tempest.Game.Tests/AtariPresentationCalibrationTests.cs:54` in
`LevelThirteenDepthGainIsAcceptedAndIndependentOfOtherOverrides`. The
breakpoint list showed exactly that one breakpoint. `start_debugging` was
invoked with that file, method, and the Tempest working directory; the tool
reported that the test ran to completion without stopping, with no active
session. This does not independently establish a passing test result or
that a C# debug adapter successfully attached. First-frame, call-stack,
variable, expression, stepping, and continue proofs were not run.

Targeted removal returned `No breakpoint found` even though the list still
showed it. The initial breakpoint baseline was empty and the only listed
breakpoint was the proof breakpoint, so `clear_all_breakpoints` was used
and reported one removed; the follow-up list was empty. Debug status is
`no-session`. No testhost/VSTest process was observed afterward; the only
dotnet processes were the project host and two reusable MSBuild nodes that
started at reload.

The temporary `debugmcp.timeoutInSeconds` user setting was removed. Its
original absence and original 469-byte file size and CRLF format were
verified; no other setting was intentionally changed. The restored setting
takes effect on subsequent extension activation, not necessarily in the
running extension. The tracked worktree and index remain clean.

Rollback under the original failed-proof policy is in progress. Do not retry
the proof. The extension, Copilot registration, two installed skill
directories and global storage were created by this proof and remain until
their removal is safely completed and verified.

### Rollback reload handoff

`code --uninstall-extension ozzafar.debugmcpextension` returned success,
and the extension no longer appears in the CLI inventory. However, the
running window still has PID 31496 listening on `127.0.0.1:3001` and
`[::1]:3001`, and the extension directory remains present. Uninstall has
therefore not yet established deactivation or listener cleanup.

**PAUSED / rollback incomplete:** Neal should reload the Tempest window
once, then confirm completion. Do not reinstall. Afterward verify that the
listener stopped, inventory the extension and the proof-created artifacts,
and safely remove only the proof-created Copilot MCP registration, skill
directories and global storage as appropriate. Verify prior absence and
ownership before removal; preserve any unexpected changes and do not delete
shared caches. The temporary timeout setting is already restored to its
original absence. No debugger session or breakpoint remains.

### Post-reload rollback result

Neal confirmed reload. **PASS:** The former extension host PID 31496 is gone
and ports 3001, 3002, and 3100 have no listener. DebugMCP no longer appears
in VS Code's installed extension list. The user MCP file contained only the
proof-created `debugmcp` entry with its original expected settings; it was
removed. Workspace and Copilot CLI MCP files remain absent.

Both `debug-live` directories were absent before activation and their nine
files each matched the verified VSIX byte-for-byte; no reparse points or
extra files were present. Only those verified files and their now-empty
directories were removed. Both legacy skill sibling sets remain absent.
The proof-created DebugMCP global storage contained only an empty
`window-registry` directory and was removed. The timeout setting remains
absent and user settings length is 469 bytes, as at baseline. No VSTest or
testhost process was detected.

**PARTIAL rollback:** VS Code's host-managed extension package directory
`ozzafar.debugmcpextension-2.4.4` still exists after uninstall/reload.
The shared VS Code `.obsolete` registry marks that exact version obsolete.
It is not listed as installed and has no listener. The package directory and
shared registry were not manually altered; automatic host-managed cleanup
has not been verified. Extension global-state records in VS Code's shared
state database were not inspected or deleted. Do not claim a complete
byte-for-byte restoration of the VS Code profile.

**Final proof classification: FAIL.** The debugger tool reported that the
target test ran to completion without hitting the breakpoint; a passing test,
adapter attach and first frame were not independently established. Successful
read-only tool discovery/status is the only demonstrated integration value.
The generated user MCP file had four VS Code schema diagnostics before
removal. Neither those warnings nor the failed test-debug path were fixed or
retried. No manual gameplay acceptance was required or attempted.

Tracked worktree and index remain clean at HEAD
`4c7365a01f944cf2b501164e5015f4954110434f`; the previously recorded
untracked materials and this continuation's prompt/report remain unstaged.
No Tempest production code, tests, tracked project configuration, gameplay,
or protected assets were changed. Nothing was staged, committed, or pushed.
Further investigation of the test-debug failure and any eventual
host-managed obsolete-package cleanup requires a separately bounded task.
