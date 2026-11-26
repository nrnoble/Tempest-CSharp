# VS Code Copilot Development-Tool Capability Assessment

Date: 2026-11-25

## 1. Executive summary

**Decision: NOT READY for bounded DebugMCP installation/validation.**

The local .NET stack already has capable manual C# debugging, test discovery,
test execution, Roslyn navigation, Git history tools, and deterministic
headless playback. The material missing capability is debugger control exposed
to the current agent. Microsoft-hosted DebugMCP is the strongest candidate:
it supports C#/.NET, GitHub Copilot, test debugging, breakpoints, stepping,
inspection, and expression evaluation through a local VS Code MCP extension.

Do not install it yet. VS Code bundles `GitHub.copilot-chat 0.73.0`, but no
separate `GitHub.copilot` extension appears in the user extension list; the
current Copilot SDK tool host does not expose DebugMCP, and it is not
established that installing into this VS Code profile would connect DebugMCP
to this actual agent session. Also, the current C# Dev Kit Marketplace page
lists the .NET 10 SDK while only .NET 8/9 SDKs are present; older C# testing
documentation says .NET 6+. The installed Dev Kit `3.40.215` manifest does
not pin a .NET SDK version, so its exact test-debug prerequisite remains
uncertain. These are compatibility questions, not reasons to install another
SDK or debugger during this assessment.

The reports show a concrete use case: deterministic playback and bounded
snapshots already find a first divergent Level 11 boundary quickly, but live
locals/object inspection and stepping are not exposed to the current agent.
DebugMCP would complement that evidence, not replace recordings, regression
tests, or Neal's manual acceptance.

## 2. Starting Git/worktree state

- Branch: `master`
- HEAD: `4c7365a01f944cf2b501164e5015f4954110434f`
- Upstream: `origin/master`
- Ahead/behind: `0/0`
- Index: no staged changes
- Tracked worktree: clean
- Pre-existing untracked file:
  [Reference/arcade/MBUCOD.V05.txt](../../Reference/arcade/MBUCOD.V05.txt);
  left untouched.
- Targeted ignored-state metadata:
  [Reference/arcade/roms.rev2/](../../Reference/arcade/roms.rev2/),
  [TEMP/](../../TEMP/), [UserData/](../../UserData/), and
  [Videos/](../../Videos/) are ignored. Their contents were not enumerated,
  opened, hashed, or changed.
- The only files created by this assessment are the two requested AI-Logs
  artifacts. Both are untracked and unstaged.

## 3. Current VS Code version

`code --version` reported VS Code `1.145.1`, commit
`6bab64e2d12b04d04c0d99c27ff166023efd1dda`, x64. DebugMCP's manifest requires
VS Code `^1.104.0`, so the version requirement is satisfied.

## 4. Current .NET SDK/tool inventory

- SDKs: `8.0.424`, `9.0.303`, `9.0.304`; selected SDK: `9.0.304`.
- Runtimes: .NET Core `3.1.32`, .NET `8.0.19/8.0.30`, .NET `9.0.7/9.0.8`,
  ASP.NET Core `8.0.19/8.0.30/9.0.7/9.0.8`, and Windows Desktop
  `8.0.19/8.0.30/9.0.7/9.0.8`.
- Both game and test projects target `net9.0` and enable nullable reference
  types.
- No local .NET tools are in the repository manifest. One unrelated global
  tool is installed: `dotnet-ef 9.0.7`.
- `node`, `npm`, `npx`, `dotnet-counters`, `dotnet-trace`, `dotnet-dump`,
  `dotnet-gcdump`, `dotnet-sos`, `ildasm`, `ilspycmd`, `dotnet-coverage`,
  `reportgenerator`, and `gitleaks` were not found on PATH.
- Installed workloads are WASI/WebAssembly experimental workloads; they are
  not relevant to Tempest.
- PowerShell is `5.1.26100.9818`; `Format-Hex` and `certutil.exe` are
  available for bounded local binary inspection.

## 5. Installed relevant extension inventory

`code --list-extensions --show-versions` reported:

| Extension | Version | Relevance |
|---|---:|---|
| `ms-dotnettools.csharp` | `2.160.4` | C# language service and .NET debugging |
| `ms-dotnettools.csdevkit` | `3.40.215` | C# project management, test discovery/run/debug |
| `ms-dotnettools.vscode-dotnet-runtime` | `3.2.0` | .NET runtime installation support for extensions |
| `ms-dotnettools.vscodeintellicode-csharp` | `2.2.3` | C# IntelliSense assistance |
| `eamodio.gitlens` | `19.7.0` | Local blame, history, and diff navigation |
| `ms-vscode.powershell` | `2025.4.0` | PowerShell maintenance workflows |
| `GitHub.copilot-chat` (bundled with VS Code) | `0.73.0` | Copilot Chat package, found by `code --locate-extension` |
| `anthropic.claude-code` | `2.1.332` | Separate agent/model extension; not recommended here |
| `continue.continue` | `2.0.0` | Separate agent/model extension; not recommended here |
| `ollama.ollama` | `0.0.14` | Local-model extension; not recommended here |

No DebugMCP, coverage-specific, profiler, hex-editor, or separate
`GitHub.copilot` extension appeared in the user extension list.
`code --locate-extension` resolved the local C# manifests: C# `2.160.4`
contributes the debugger types `coreclr`, `clr`, `blazorwasm`, `dotnet`, and
Mono variants; C# Dev Kit `3.40.215` depends on the .NET Runtime extension and
declares limited Workspace Trust support. The bundled Copilot Chat package's
VS Code engine requirement is `^1.145.1`, matching this VS Code build.

## 6. Current Copilot Agent tool inventory

In this SDK session, the agent has bounded PowerShell, file view, `rg`/glob
search, local semantic search/usages, test/task helpers, web fetch, GitHub MCP
read/search, and Git/GitKraken MCP capabilities. It also has a DAP debugger
and LSP integration for Python through Pylance. Those Python-specific tools do
not debug C#.

No C#/.NET DAP control, generic VS Code debugger-control tool, or DebugMCP
server is exposed to this agent. Although the VS Code application bundles
Copilot Chat, the relationship between that extension and this SDK host's
agent/tool registry has not been established.

## 7. Current C# debugger capability

Manual C# debugging is already available in principle through the installed
Microsoft C# extension (`coreclr` and `dotnet` debugger types), with C# Dev Kit
providing project-aware launch and testing UI. Official VS Code guidance
documents dynamic launch configurations, `launch.json`, .NET process attach,
stack/locals inspection, expression evaluation, stepping, continue/pause, and
session termination.

The workspace has no `.vscode/launch.json`, `.vscode/tasks.json`, or other
workspace launch/task files. No debugger was started in this assessment.
Headless playback can be debugged after providing a launch configuration with
the application arguments; that configuration does not currently exist.

| Manual VS Code capability | Status in this environment |
|---|---|
| Ordinary breakpoints; step over/into/out; continue/pause; stop | Available through the installed C# debugger/UI; not trial-run |
| Call stack, thread selection, locals and object expansion | Available through the normal VS Code debug views; not trial-run |
| Expression evaluation and watch expressions | Available through the debug console/watch view; not trial-run |
| Process attach | Documented by the C# debugger extension |
| Conditional/hit-count breakpoints, logpoints, exception filters | VS Code exposes adapter-specific forms, but support in this installed C# adapter was not verified |
| Test discovery/run/debug | Documented in C# Dev Kit; exact installed-version/SDK behavior remains unverified |
| Headless playback launch | Feasible after supplying CLI args in a dynamic or explicit launch configuration; no such workspace configuration exists |

These are VS Code/debug-adapter capabilities, not capabilities currently
exposed to Copilot. Adapter-specific breakpoint and exception support should
be checked in the future proof rather than inferred from generic UI controls.

## 8. Current debugger capability exposed to Copilot

**None for C# in the current agent toolset.** In particular, the agent cannot
currently set/remove breakpoints, step, continue/pause, inspect C# frames,
enumerate threads, expand C# objects, evaluate C# expressions, or terminate a
C# debug session through a tool.

This is distinct from VS Code and the installed C# debugger being able to do
those things manually.

## 9. Demonstrated Tempest capability gaps

The required reports establish:

- The November 23 probe could run a supplied deterministic recording to an
  exact headless boundary and return a bounded production-state snapshot.
- A 10,000-tick headless campaign segment completed in about 0.421 seconds.
- The current agent could not pause/step a live C# session or inspect arbitrary
  entity internals and call stacks.
- The Level 17 investigation sampled the baseline/candidate campaign to
  boundaries `19792/19793`, identified the first observed mismatch in Level
  11, and compared deterministic state.
- The bounded correction report proved the shared cargo-route cause and
  validated the correction with recordings and tests.
- The accepted Level 17 closeout reports 2,065 full-suite tests passing and
  Level 1–16/Level 17 recording checks.

These are evidence-access gaps, not evidence that more logging or another
model is needed. The task's second attachment included interactive game launch
commands; none was run, because the assessment explicitly forbids launching
the game.

## 10. DebugMCP assessment

**Candidate: DebugMCP VS Code extension, `ozzafar.debugmcpextension` 2.4.4.**
It runs an MCP service in VS Code and drives the VS Code debugger. The
agent-facing operations are:

| Capability | DebugMCP tool surface |
|---|---|
| Start/stop/restart and debug a test | `start_debugging` accepts file/workspace, optional `configurationName`, and optional `testName`; separate stop/restart tools |
| Step/continue/pause | Step over/into/out, continue, and pause tools |
| Breakpoints | Add conditional breakpoint, add conditional logpoint, list/remove/clear; no hit-count argument in the published breakpoint schema |
| Call stack/status | `get_debug_status` returns pause state and selected-frame details/stack data; a stop/frame-selection race is reported in issue #201 |
| Locals and object inspection | List local/global variable names/types, fetch named values, evaluate an expression; expandable children are names/types and may require another expression to inspect values |
| Threads/exception filters/attach | No dedicated thread-enumeration or exception-filter tool is documented; no explicit attach tool is documented. Launching a named VS Code configuration may cover some attach configurations, but that is unverified |
| Cancellation/cleanup | Stop-debugging and clear-breakpoints tools exist; no independent cancellation-token tool is documented. Default operation timeout is 300 seconds; pause waits can request up to 600 seconds but are bounded by the operation timeout |
| Output | Opt-in, bounded in-memory DAP output capture; disabled by default and heuristic redaction |

It supports a test name through the VS Code Testing API. It supports conditional
breakpoints but not an explicit hit-count control in its tool schema.
The routing design supports separate MCP sessions/windows, but concurrent
operation was not tested; issue #191 demonstrates a wrong-workspace routing
failure when an explicit path does not match a registered workspace.

It is the best-fit candidate for the missing “same debugger Neal uses, but
agent-operable” capability. It does not replace deterministic playback or
provide an independent simulator.

## 11. DebugMCP publisher/source/license/security evidence

- Official Marketplace API result: extension ID `ozzafar.debugmcpextension`,
  publisher display name **Microsoft DevLabs**, version `2.4.4`, published
  `2026-10-25`.
- The extension manifest names publisher ID `ozzafar`, author Oz Zafar, and
  links to `https://github.com/microsoft/DebugMCP`.
- The repository is public, source-available, and MIT-licensed. Its README
  identifies Microsoft employee maintainers. The repository has no GitHub
  Releases page entries; Marketplace is the distribution channel.
- Repository metadata showed recent activity (updated November 20); the
  relevant latest issue was opened November 24. This indicates active
  development but also a moving surface.
- Manifest minimum VS Code version is `^1.104.0`.
- MCP transport is Streamable HTTP. The default service hosts are
  `127.0.0.1` and `::1`, port `3001`; changing `debugmcp.bindHost` to a
  non-loopback address exposes an **unauthenticated** debugger, including
  expression evaluation and debug-session launch, to network peers.
- The per-window internal control server is separately bound to `127.0.0.1`
  and uses a per-window token. Do not confuse that internal token with
  authentication on the MCP HTTP endpoint.
- Open issue [#191](https://github.com/microsoft/DebugMCP/issues/191) reports
  an explicit path for an unregistered workspace being forwarded to a cached
  or sole different VS Code window. The reviewed `resolveTarget` implementation
  retains its cached target when a path lookup misses; do not assume routing
  isolation across unrelated workspaces.
- Open issue [#201](https://github.com/microsoft/DebugMCP/issues/201) reports
  the initial paused state being returned before VS Code selects a frame,
  yielding empty/stale call-stack data. Its linked PR [#203](https://github.com/microsoft/DebugMCP/pull/203)
  was observed but no fix is assumed. These issues raise stale-state and
  debugger-stall risk; the proof must check the first stop and clean termination.
- Optional debug-output capture is off by default, bounded and in-memory, but
  its credential redaction is explicitly heuristic. It should remain off for
  the initial proof.
- The v2.4.4 package manifest has no `capabilities.untrustedWorkspaces`
  declaration. VS Code's Workspace Trust guide says an extension with no such
  declaration is treated as unsupported in Restricted Mode and is disabled
  there. The Tempest workspace must therefore be trusted for activation.
- Debugger tool responses may contain source values, object state, paths, and
  logs. DebugMCP itself is local, but values passed through an agent are still
  subject to the existing Copilot service's data handling. Do not inspect
  secrets; do not enable output capture without a specific need.
- Installation/activation may register the MCP service with the assistant and
  install its `debug-live` skill in standard user skill directories. These are
  user-profile side effects beyond project files and must be considered in a
  rollback.

Sources: [Marketplace listing](https://marketplace.visualstudio.com/items?itemName=ozzafar.debugmcpextension),
[Marketplace metadata API](https://marketplace.visualstudio.com/_apis/public/gallery/extensionquery?api-version=7.1-preview.1),
[repository](https://github.com/microsoft/DebugMCP),
[README](https://github.com/microsoft/DebugMCP/blob/main/README.md),
[manifest](https://github.com/microsoft/DebugMCP/blob/main/package.json),
[security policy](https://github.com/microsoft/DebugMCP/blob/main/SECURITY.md),
[routing issue #191](https://github.com/microsoft/DebugMCP/issues/191),
[debug-state issue #201](https://github.com/microsoft/DebugMCP/issues/201),
[linked PR #203](https://github.com/microsoft/DebugMCP/pull/203),
[VS Code Workspace Trust guide](https://code.visualstudio.com/api/extension-guides/workspace-trust).

## 12. DebugMCP .NET compatibility

The README lists C#/.NET and the Microsoft C# extension as supported. This
workspace has `ms-dotnettools.csharp` and C# Dev Kit installed, targets
`net9.0`, and has .NET 9 SDK/runtime available. DebugMCP's extension itself
does not require a separate .NET adapter installation; it uses the VS Code
debugger.

There is a material test-debug uncertainty: the current C# Dev Kit Marketplace
page says its requirement is the .NET 10 SDK, but the installed machine has no
.NET 10 SDK. The official C# testing documentation also describes .NET 6+.
The installed C# Dev Kit `3.40.215` manifest only declares its VS Code engine
and extension dependency, not an SDK version. Do not assume its test
discovery/debug behavior on this `net9.0` project until verified. Direct
`dotnet test` for a `net9.0` project remains available through SDK 9.

## 13. DebugMCP Copilot compatibility

The project README explicitly lists GitHub Copilot and says the extension
offers MCP registration. The VS Code version meets its declared minimum.
However, the current Copilot SDK session does not expose DebugMCP. The bundled
Copilot Chat extension is present in the VS Code application, but Marketplace
support for “Copilot” is not proof that this specific SDK host/profile will
load the server. This must be resolved before installation.

## 14. DebugMCP deterministic-playback compatibility

**Practical, with configuration.** A VS Code debug configuration can launch
the Tempest console project with the existing headless-playback recording and
boundary arguments. Because no `launch.json` exists, this needs a temporary
debug configuration or a supported dynamic configuration that accepts those
arguments. Alternatively, `start_debugging` can target one deterministic
existing test by name through the Testing API, if C# Dev Kit test discovery is
working.

The debugger does not itself binary-search a recording or reconstruct a game
from an arbitrary boundary. Recommended order remains:

1. Use the existing recording/headless CLI to identify last-match and
   first-difference boundaries.
2. Debug the smallest deterministic test or a launch-configured headless
   playback at that boundary.
3. Inspect production state and step through the transition.
4. Apply the smallest authorized correction.
5. Re-run focused tests and strict campaign playback.

The first proof should use one test, not the interactive Raylib application.

## 15. Debugger alternative comparison

**Alternative reviewed:** [`debugmcp/mcp-debugger`](https://github.com/debugmcp/mcp-debugger),
MIT-licensed, headless DAP-over-MCP service. Its README lists .NET/C# via
`netcoredbg`, content/function-addressed breakpoints, logpoints, exception
stops, and optional IDE mirroring. Its structural distinction is that it
launches its own adapter/process rather than controlling the active VS Code
debugger. This may suit CI, remote attach, or container work, but not the
current local in-editor investigation.

It introduces a Node.js runtime (not installed), a separately configured
`netcoredbg` adapter (not installed), and another maintained debugging stack.
It will not reuse the existing C# extension's active launch/test session.
Therefore it is a fallback only if the actual Copilot host cannot consume the
VS Code extension and a standalone adapter proof is separately authorized.

DebugMCP's own standalone CLI is another distribution route for the same
project, not an independent alternative. It needs Node/npm and explicit DAP
adapter configuration, both absent here, and gives up the integrated VS Code
debug session.

## 16. Recommended debugger candidate

**Preferred candidate after blockers are resolved:** DebugMCP VS Code extension
2.4.4. It has the strongest match to the identified gap, reuses the installed
C# debugger, claims C#/.NET and Copilot support, and can debug a selected test
without changing Tempest source.

Do not install multiple debugger bridges. Do not choose the headless
`mcp-debugger` for the initial local proof.

## 17. Test-execution/tooling assessment

The test project uses xUnit `2.9.2`, `Microsoft.NET.Test.Sdk 17.12.0`, and
`xunit.runner.visualstudio 2.8.2`. C# Dev Kit is installed and its official
documentation supports xUnit test discovery, individual/parameterized test
execution, debugging, test result reporting, and stack-trace navigation.
`dotnet test` and VSTest filters remain the simplest precise CLI route; a
bounded timeout can be passed with VSTest's `--blame-hang-timeout` option.

No separate .NET test-explorer extension is needed. The test project has no
`launch.json` or `.runsettings`; no hang timeout was run. Existing VSTest
bounded timeout/blame options and a command-level deadline can be used without
adding another framework. Test-explorer UI is useful for Neal; exposing
debugger controls to the agent is the missing layer.

## 18. Coverage-tool assessment

C# Dev Kit documents coverage through VS Code's Coverage APIs, but the current
test project has no explicit `coverlet.collector` or other coverage collector
reference. No `dotnet-coverage`, Coverlet CLI, or ReportGenerator command was
found. VSTest offers a built-in Microsoft coverage collector; the cross-platform
Coverlet collector is another local option and can emit Cobertura data for
ReportGenerator. C# Dev Kit can display supported coverage data in the editor.

**Value: MEDIUM, useful later.** It could reveal an unexecuted shared
Flipper/Tanker branch or state transition, and a before/after changed-file
coverage check could support a milestone. It would not show that an executed
branch has the correct arcade behavior, and it would not replace the strict
recording oracle or manual acceptance. No cloud service is recommended.

## 19. Static-analysis assessment

Both projects enable nullable analysis. No project `.editorconfig` or explicit
analyzer package was found. The .NET SDK/Roslyn analyzer baseline is already
available; there is no need to install an analyzer extension. Incrementally
enable only a few relevant rules (null-state, switch/default handling,
disposable/lifecycle mistakes, and suspicious equality) after measuring the
warning delta. Avoid a broad ruleset that creates unrelated churn before
Level 18.

## 20. Architecture-analysis assessment

Potential risks include level-specific branches, duplicated actor lifecycle,
eligibility/parameter confusion, enum growth, and presentation/gameplay
coupling. These are better guarded first by targeted xUnit architectural
invariants and Roslyn diagnostics than by introducing a paid architecture
suite or graph service. If a repeatable rule emerges, a focused test can assert
shared lifecycle/eligibility behavior using existing project test patterns.

**Value: MEDIUM for targeted tests; LOW for a new architecture product now.**
No evidence justifies a broad architecture rewrite or new dependency.

## 21. Profiling assessment

No .NET diagnostic CLI tools were found. There is no supplied evidence that
simulation/rendering throughput, allocations, startup, or test performance is
currently a blocker. Headless playback already provides useful performance
evidence for debugging.

**Classification: USEFUL LATER.** If a measured slowdown appears, use local
Microsoft `dotnet-trace`, `dotnet-counters`, or `dotnet-gcdump` captures with
short durations and bounded artifacts. Do not profile or optimize without an
observed performance question. No VS Code profiler extension is recommended.

## 22. Runtime tracing/logging assessment

Existing deterministic state summaries, recording boundaries, focused
playback, and tests provide stronger, lower-noise evidence than permanent
logging for the recent regression class. DebugMCP could add a conditional
breakpoint or logpoint, inspect locals at a chosen pause, and step the exact
transition. Its output capture is opt-in and should stay off during the first
proof.

For future repeatable diffs, a bounded machine-readable state fingerprint and
first-divergence report could be useful if Neal authorizes that project work.
Do not add it here. A debugger is preferable to a permanent trace when the
last/first divergent boundaries are already known and arbitrary live fields
are needed.

## 23. Git/repository-analysis assessment

Git `2.51.0.windows.1` and GitLens `19.7.0` are present. Git CLI gives exact
status, history, range diffs, blame, and selective staging; GitLens improves
interactive blame/history/diff navigation. The current agent also has
Git/GitKraken operations available, but this assessment used read-only
commands.

No extra Git UI extension is recommended. Continue the project rule of checking
status first and staging only authorized paths. Explicitly exclude or avoid
selecting `TEMP/`, `UserData/`, `Videos/`, local recordings, and ROM/reference
assets. No staging, commit, or push occurred.

## 24. Binary/ROM/assembly-tool assessment

PowerShell `Format-Hex` and Windows `certutil.exe` are available for
deliberately bounded local byte inspection. `ildasm` and `ilspycmd` were not
found; no hex editor extension was listed. The local arcade disassembly is the
preferred evidence for known ROM maps. For a future authorized verification,
use a small specified byte range, explicit address/offset mapping, and
known-byte anchors; use local byte comparison rather than downloading ROM
content.

Assembly/IL inspection is low-value unless generated IL or a dependency
behavior becomes relevant. No ROM file was opened, hashed, or enumerated in
this assessment.

## 25. Dependency/security-tool assessment

No NuGet or secret-scanning extension was identified. The installed .NET SDK
supports local package inventory/vulnerability queries (for example
`dotnet list package --vulnerable --include-transitive`); no scan was run.
This queries package metadata/advisories and does not require uploading source
to a scanner service. For secrets, a local scanner such as Gitleaks could be
considered only for explicitly selected tracked files/diffs; it is not
installed and must not traverse ignored personal/reference assets.

The primary security issue in this assessment is DebugMCP's local privileged
endpoint, not a need for more security extensions. Keep it loopback-only,
avoid auto-approval of all tools, do not inspect secrets, and review any
assistant registration or skill files before removing them.

## 26. Navigation/documentation-tool assessment

C# Dev Kit includes Roslyn-backed definition/reference search, semantic
awareness, project/test navigation, and refactoring. GitLens covers line
history; `rg` and the local arcade disassembly support exact text search.
These are sufficient for normal source navigation.

There is no demonstrated need for automatic documentation generators, large
dependency maps, or external code indexing. Use small manually maintained
Mermaid/state diagrams only if a concrete lifecycle becomes hard to retain;
keep behavior evidence in the existing behavior spec and development notes.

## 27. Maintenance-automation assessment

The repository has no `.vscode` task/launch configuration and no GitHub
workflow files; `.github/copilot-instructions.md` is present. Existing
PowerShell, `dotnet test`, `dotnet build`, Git, deterministic recording CLI,
and `git diff --check` are enough to execute bounded gates.

If repeated command construction becomes a measurable cost, a transparent
PowerShell script or named VS Code task could group the exact focused test,
Release build, recording oracle, and diff check, each with a visible command,
deadline, and explicit PASS/FAIL/TIMEOUT result. Do not hide failures, modify
tasks/CI now, or create report scaffolding automation before it is justified.

## 28. Information-retrieval assessment

Local `rg`, the current agent's semantic search/usages tools, C# Roslyn
navigation, GitLens, Git history, and the local disassembly cover the main
retrieval needs. GitHub MCP search is useful for public upstream tool evidence
but is not a local repository index. Pylance LSP is Python-specific; the
current agent has no C# DAP debugger-control tool.

No remote indexing of private code, gameplay recordings, user assets, or ROM
material is recommended. Existing local search is adequate until a concrete
cross-report lookup bottleneck is demonstrated.

## 29. Credit/model-use assessment

DebugMCP and the alternative debugger are tools, not models. Neither requires
an additional AI service or model subscription. Debugger results still consume
the normal context/usage of the active Copilot session when returned to it.
Local tests, profilers, analyzers, Git, and coverage add no model credits.

The installed Claude Code, Continue, and Ollama extensions were inventory
items only; they were not used or recommended. The task explicitly excludes
additional AI models and credit-driven second opinions.

## 30. Candidate comparison matrix

| Candidate | Tempest/evidence value | Local/Copilot/.NET fit | Security/maintenance/overlap | Credit | Decision |
|---|---|---|---|---|---|
| DebugMCP VS Code extension 2.4.4 | High: inspect exact deterministic test/playback state | Strong documented Copilot and C# support; reuses current VS Code debugger | Medium risk: unauthenticated loopback MCP, powerful expression tool, open routing/state issues, user-scope side effects | Existing Copilot only | Preferred proof candidate after blockers |
| Existing C# + C# Dev Kit | High for Neal's manual debug/test workflow | Installed and .NET-native | Low added risk; not agent-controlled | None | Keep; no install |
| `debugmcp/mcp-debugger` 0.26.0 | Medium for future remote/headless work | .NET via separate `netcoredbg`; not existing VS Code session | More adapters/runtime/configuration; duplicated capability | Existing Copilot only | Defer |
| DebugMCP standalone CLI | Medium for terminal agents | Explicit DAP adapter configuration; Node/npm absent | Same product/security surface, less IDE integration | Existing Copilot only | Not selected |
| Coverlet + C# Dev Kit coverage UI | Medium for changed-branch gaps | Local .NET, compatible xUnit path | Additional test package and report lifecycle | None | Tier 2 |
| SDK analyzers + narrow rules | Medium for C# correctness | Already available in SDK | Warning churn if enabled broadly | None | Tier 2, incremental |
| `dotnet-trace`/counters/gcdump | Low now; useful after measured slowdown | Local .NET | Capture size and diagnostic overhead | None | Defer |
| New external code index/cloud scanner | Low; local tools adequate | Weak local-data fit | Unnecessary data exposure/maintenance | May add cost | Reject |

## 31. Tier 1 recommendations

**One candidate only:** DebugMCP VS Code extension, after compatibility blockers
are resolved. Use a single deterministic test or bounded headless playback
after a recording has already localized the boundary. Do not add a second
debugger bridge.

## 32. Tier 2 recommendations

- Add a local coverage collector and use C# Dev Kit's coverage UI only for a
  milestone where changed-path branch gaps matter.
- Enable a small number of SDK/Roslyn analyzer rules with an explicit baseline.
- Add short-duration .NET diagnostic captures only after a measured
  performance issue.
- Consider a transparent PowerShell task wrapper only after repeated manual
  command construction becomes a demonstrated cost.
- Consider an architecture invariant test only when a specific shared-path
  invariant can be stated and tested.

## 33. Tier 3: already sufficient

- Manual C# debugging and test navigation: installed Microsoft C# and C# Dev
  Kit extensions, subject to the SDK compatibility caveat.
- Test execution: xUnit/VSTest, `dotnet test`, and C# Dev Kit Test Explorer.
- Source/Git navigation: Roslyn, `rg`, Git, and GitLens.
- Regression evidence: deterministic recordings, headless boundaries,
  production-state summaries, focused tests, and Neal's manual acceptance.
- Binary byte inspection for future bounded local work: PowerShell
  `Format-Hex`/`certutil`; no extension required.

## 34. Tier 4: not recommended

- Multiple debugger MCP bridges installed together.
- Cloud code indexing, hosted coverage, or repository-upload scanners.
- New AI models, autonomous cloud agents, or paid model subscriptions.
- A broad analyzer rollout, paid architecture product, or generated
  documentation pipeline without a specific observed need.
- Enabling DebugMCP's non-loopback bind host or auto-approving every debug tool.
- Installing/running an unverified bridge merely to discover its security
  behavior.

## 35. Preferred tool prerequisites

Before any bounded DebugMCP pilot:

1. Identify the exact VS Code profile and Copilot SDK/Agent host used for this
   project; establish that it consumes this extension's MCP registration.
2. Resolve the agent-host mismatch: although Copilot Chat is bundled, no
   DebugMCP tools are currently exposed to this SDK session. Verify whether
   this host consumes the bundled extension's MCP registration.
3. Confirm whether installed C# Dev Kit `3.40.215` can discover/debug the
   `net9.0` xUnit project with SDK 9, despite the current Marketplace page's
   .NET 10 requirement.
4. Confirm the extension remains loopback-only and understand its skill and
   MCP-registration writes before authorizing installation.
5. Use one trusted Tempest workspace, one open DebugMCP-enabled VS Code
   workspace, and no unrelated workspace windows for the first proof.

## 36. Proposed proof-of-capability milestone

This is a future plan, **not executed**:

1. In a dedicated, user-approved VS Code profile, install only DebugMCP 2.4.4;
   leave the existing C# and C# Dev Kit extensions available.
2. Confirm the current intended Copilot SDK/Agent session lists DebugMCP tools.
   Do not enable blanket tool auto-approval.
3. Select one existing deterministic `Tempest.Game.Tests` test from test
   discovery; do not add or modify a test.
4. Start that .NET debug session and stop at one known executable line.
5. Verify the call stack, one local variable, object-child expansion, expression
   evaluation, step over, step into or out, continue, and clean termination.
6. Remove the temporary breakpoint and confirm the breakpoint list is empty.
7. As the Tempest-specific check, debug one deterministic existing test or a
   launch-configured headless playback boundary; never start with Raylib.
8. Compare `git status` and tracked diff before/after. Require no production
   code, test, project configuration, recording, or behavior changes.
9. Record time, tool calls, errors, cleanup, and whether the session was
   materially faster/more informative than the current playback/test workflow.

Pass only if each requested debugger operation returns the expected C# state,
the session terminates, temporary breakpoints are removed, and project files
remain unchanged. A timeout, stale frame, wrong workspace, or unexplained
session is not a pass.

## 37. Security and rollback plan

- Keep `debugmcp.bindHost` at its default loopback addresses and do not expose
  the unauthenticated MCP endpoint to a LAN.
- Use only a trusted workspace and known local test data; avoid secrets,
  credentials, and output capture.
- Do not auto-approve all tools. Approve only the bounded debug operations.
- Keep only the Tempest window open for the proof, because an open
  wrong-workspace routing issue is documented.
- Before install, record the exact profile, MCP registration, and skill-file
  state. The extension may write to standard user skill directories; do not
  delete pre-existing files during rollback.
- On failure, stop the debug session, remove breakpoints, disable/uninstall
  only the newly added extension, and remove only its newly created MCP
  registration/skill files after verifying exact paths and ownership. Do not
  modify project settings or files unless separately authorized.

## 38. READY/NOT READY result

**NOT READY.** The product fit is strong, but the exact Copilot host/profile and C# test
debugger prerequisites are not established in the environment requested for
the assessment. No installation or trial should be used to guess those facts.

## 39. Exact blockers

1. `code --list-extensions --show-versions` reports no separate
   `GitHub.copilot` extension (although `GitHub.copilot-chat 0.73.0` is bundled
   with VS Code); the current SDK session does not expose DebugMCP. It is
   unknown whether the Marketplace extension can connect to this actual agent
   host/profile.
2. The installed C# Dev Kit is `3.40.215`; current Marketplace requirements
   state .NET 10, but the machine has only .NET 8 and 9 SDKs. Public C# test
   docs also state .NET 6+, so the exact installed-version test/debug
   prerequisite is ambiguous.
3. The extension installs/ registers MCP/skill artifacts in user-level
   locations; a profile-only install may not isolate every side effect.
   Those effects must be inspected and authorized before a proof.

## 40. Files/resources inspected

### Local repository files

- [Tempest.Game.csproj](../../src/Tempest.Game/Tempest.Game.csproj)
- [Tempest.Game.Tests.csproj](../../tests/Tempest.Game.Tests/Tempest.Game.Tests.csproj)
- [.github/copilot-instructions.md](../../.github/copilot-instructions.md)
  (searched for completion-token and logging requirements)
- [.github/](../../.github/) file listing (no workflow files found)
- Presence/absence checks for `.vscode/launch.json`, `.vscode/tasks.json`,
  `global.json`, `.config/dotnet-tools.json`, `.runsettings`, and
  `.editorconfig`
- [2026-11-23-agent-runtime-capability-probe-report.md](./2026-11-23-agent-runtime-capability-probe-report.md)
- [2026-11-25-level17-levels1-16-shared-regression-investigation-report.md](./2026-11-25-level17-levels1-16-shared-regression-investigation-report.md)
- [2026-11-25-level17-shared-regression-bounded-correction-report.md](./2026-11-25-level17-shared-regression-bounded-correction-report.md)
- [2026-11-25-level17-final-closeout-report.md](./2026-11-25-level17-final-closeout-report.md)

### External public resources

- DebugMCP Marketplace page and public Marketplace API metadata
- `microsoft/DebugMCP` README, package manifest, security policy, extension
  source, routing source, control-server source, repository metadata, issues
  #191/#201, and linked PR #203
- DebugMCP Open VSX version metadata
- [VS Code C# debugging](https://code.visualstudio.com/docs/csharp/debugging),
  [testing](https://code.visualstudio.com/docs/csharp/testing), and
  [debugger configuration](https://code.visualstudio.com/docs/csharp/debugger-settings)
  documentation
- [C# Dev Kit Marketplace page](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- [VS Code Workspace Trust guide](https://code.visualstudio.com/api/extension-guides/workspace-trust)
- `debugmcp/mcp-debugger` README, manifest, and MIT license
- Microsoft [.NET diagnostics](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostics-client-library)
  and [coverage](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage)
  documentation

No protected ROM, recording, screenshot, video, TEMP, or UserData file content
was opened.

## 41. Commands and queries run

- `git --no-pager status --short --branch`
- `git rev-parse HEAD`
- `git branch --show-current`
- `git rev-parse --abbrev-ref --symbolic-full-name '@{u}'`
- `git rev-list --left-right --count '@{u}...HEAD'`
- Targeted `git status --short --ignored -- <protected paths>` metadata query
- `code --version`
- `code --list-extensions --show-versions`
- `code --locate-extension` for C# Dev Kit, C# and Copilot Chat, followed by
  selected manifest-field reads
- Parsed only selected C# extension manifest fields (version, engine,
  debugger types, extension dependency, and Workspace Trust support)
- `dotnet --list-sdks`, `dotnet --list-runtimes`, `dotnet --version`
- `dotnet tool list --local`, `dotnet tool list --global`,
  `dotnet workload list`
- Bounded `Get-Command` availability checks for .NET diagnostics, coverage,
  binary, Node/npm, and secret-scanning tools
- Read-only `Get-Content` of the two project files
- Read-only `.github` file listing and `rg` searches of project metadata
- Read-only official Marketplace API query for the DebugMCP extension
- Public GitHub, Marketplace, Open VSX, and official documentation fetches

No build, test, game launch, install, restore, debugger trial, profiling
session, security scan, or ROM inspection was run.

## 42. Production code changed

**No.**

## 43. Tests changed

**No.**

## 44. Configuration changed

**No.**

## 45. Extensions installed/updated/removed

**None.**

## 46. Settings/MCP/launch/tasks changed

**None.** No VS Code settings, MCP configuration, launch configuration,
workspace tasks, or user-level settings were changed.

## 47. Stage/commit/push

**None.** The two required AI-Logs artifacts are untracked and unstaged. The
pre-existing untracked `Reference/arcade/MBUCOD.V05.txt` remains untouched.

## 48. Recommended next step

First identify the intended VS Code profile and Copilot SDK/Agent host, and
confirm how that exact host receives VS Code MCP servers. Then confirm the
installed C# Dev Kit/SDK 9 test-debug compatibility without changing project
files. If both prerequisites are resolved, Neal can authorize one isolated
DebugMCP proof using a deterministic unit test and the security/rollback plan
above. Until then, retain the current manual debugger, deterministic
recordings, tests, and GitLens workflow; install nothing.

## NOT READY

- The current Copilot SDK/Agent host's consumption of this VS Code extension's
  MCP registration is unverified.
- C# Dev Kit `3.40.215` test discovery/debug compatibility with the installed
  .NET 9 SDK is unverified; the current Marketplace page lists .NET 10.
- User-level MCP/skill registration side effects must be scoped and authorized
  before a profile-only proof.
