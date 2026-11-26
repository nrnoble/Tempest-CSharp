# Tempest VS Code Copilot Development-Tool Capability Assessment

## FAIL-CLOSED COMPLETION-TOKEN CHECK

Read the entire user-provided task before taking any action.

Before tools, files, commands, installations, configuration changes, extensions, builds, tests, or repository actions:

1. Locate the completion token near the end of this user-provided task.
2. Verify that exactly one complete token beginning with `TEMPEST-END-` appears on a line by itself in the user-provided task.
3. The token may contain only uppercase ASCII letters, digits, and hyphens after the `TEMPEST-END-` prefix.
4. VS Code or agent-generated reminders, tagged-file information, attachments, metadata, or other system-generated context after the completion token do not invalidate it.
5. This check is intended only to detect an incomplete or truncated user task.
6. If exactly one complete standalone token cannot be found, STOP before any workspace or system action and explain why.

The unique completion token is the final standalone line below.

---

# Task

Perform a read-only assessment of tools that could make VS Code Copilot more capable and efficient when developing, investigating, debugging, validating, and maintaining the Tempest C# project.

This assessment is not limited to debugger extensions.

Evaluate tools that could materially improve VSC’s ability to:

- gather reliable information;
- inspect the repository;
- understand runtime behavior;
- debug .NET code;
- inspect state without repeated source modifications;
- run and target tests efficiently;
- analyze test coverage;
- compare deterministic runs;
- inspect performance;
- analyze builds and logs;
- inspect binaries or assemblies;
- trace Git history and changes;
- validate architecture;
- enforce code quality;
- automate bounded maintenance;
- reduce repetitive compile-run-observe loops;
- improve future Level 18+ development efficiency.

The user is asking about tooling, not additional AI models.

Do not recommend:

- another AI coding model;
- switching between premium reasoning models;
- external AI services intended to increase credit consumption;
- autonomous cloud agents that duplicate current Copilot work;
- paid model subscriptions merely to obtain another opinion;
- tools whose main benefit is sending the same task to more models.

A tool may use VS Code, .NET, Git, Debug Adapter Protocol, Model Context Protocol, local language servers, local analyzers, test infrastructure, profilers, or deterministic project instrumentation.

The preferred direction is to give the current VSC Copilot session better access to engineering tools and evidence.

---

# READ-ONLY AND NO-INSTALLATION BOUNDARY

This task is an assessment only.

Do not:

- install extensions;
- uninstall extensions;
- enable or disable extensions;
- update extensions;
- alter VS Code user settings;
- alter workspace settings;
- alter MCP configuration;
- alter launch configurations;
- add tasks;
- add packages;
- add NuGet dependencies;
- add global tools;
- add local tools;
- modify source;
- modify tests;
- modify durable project documentation;
- modify Git hooks;
- modify CI;
- modify Windows security;
- launch the interactive game;
- stage, commit, or push.

Do not run an extension merely because it is already installed unless using it is clearly read-only, safe, and necessary to inventory its capability.

Do not perform a trial installation in this task.

The output must support a later user decision.

---

# Project context

The Tempest project is a C#/.NET and raylib recreation of Atari Tempest.

Development uses:

- VS Code;
- GitHub Copilot Agent mode;
- .NET build and test tooling;
- deterministic gameplay recordings;
- headless playback;
- local Atari ROM/disassembly references;
- permanent automated tests;
- Neal’s manual arcade/MAME comparisons.

The project has recently demonstrated the value of deterministic runtime evidence.

For example, the accepted Levels 1–16 recording allowed VSC to:

- reproduce a candidate regression;
- compare baseline and candidate behavior;
- locate the first confirmed divergence inside Level 11;
- narrow it to a small tick interval;
- compare deterministic runtime summaries;
- validate the correction against complete campaign playback.

This was successful, but required repeated bounded runs, probes, temporary variants, builds, and state comparison.

A debugger could potentially complement this process by helping VSC inspect the exact execution path after deterministic playback identifies the failing boundary.

The objective is not to replace deterministic recordings, tests, or manual acceptance.

The objective is to identify tools that make each layer more effective.

---

# Required prior project evidence

Read completely:

`AI-Logs/Reports/2026-11-23-agent-runtime-capability-probe-report.md`

`AI-Logs/Reports/2026-11-25-level17-levels1-16-shared-regression-investigation-report.md`

`AI-Logs/Reports/2026-11-25-level17-shared-regression-bounded-correction-report.md`

`AI-Logs/Reports/2026-11-25-level17-final-closeout-report.md`

Use these reports to identify demonstrated capability gaps and high-value workflows.

Do not reopen their gameplay findings.

---

# Starting repository and environment state

Record:

- branch;
- HEAD;
- upstream;
- ahead/behind;
- staged state;
- tracked modifications;
- untracked project files;
- ignored local assets relevant to the assessment.

Also record the current tool environment where safely discoverable:

- VS Code version;
- .NET SDK versions;
- installed C#/.NET extensions and versions;
- installed debugger extensions and versions;
- installed Git-related extensions and versions;
- installed testing and coverage extensions and versions;
- installed static-analysis extensions and versions;
- installed profiling or performance extensions and versions;
- installed MCP-related extensions and versions;
- installed Copilot-related extensions and versions;
- locally available .NET tools;
- relevant command-line development tools;
- current launch configurations;
- current tasks;
- current test adapters;
- current language-server/debug-adapter availability;
- tools currently exposed to Copilot Agent mode.

Do not include unrelated personal extensions unless relevant to development, security, or tool conflicts.

Do not expose secret values from settings.

---

# Core question

Answer:

> What additional local tools would materially improve VSC Copilot’s ability to investigate and develop Tempest accurately and efficiently, without adding AI models or unnecessary credit consumption?

For every proposed tool, distinguish:

1. capability currently missing;
2. evidence that the project would benefit;
3. tool that could supply it;
4. expected workflow improvement;
5. overlap with existing capabilities;
6. security and data-handling implications;
7. maintenance and stability risk;
8. installation/configuration effort;
9. validation experiment needed before adoption;
10. recommendation priority.

Do not produce a generic list of popular extensions.

Every recommendation must connect to a demonstrated or foreseeable Tempest workflow.

---

# Assessment area 1: debugger control for Copilot

Assess the current .NET debugging stack.

Determine:

- whether normal manual C# debugging is already available;
- which C# extension/debug adapter supplies it;
- whether a valid Tempest launch configuration exists;
- whether tests can currently be launched under the debugger;
- whether headless playback can be launched under the debugger;
- whether VSC Copilot currently has tools for:
- adding breakpoints;
- conditional breakpoints;
- breakpoint hit counts;
- logpoints;
- stepping over;
- stepping into;
- stepping out;
- continuing;
- pausing;
- call-stack inspection;
- thread inspection;
- locals and scope inspection;
- object expansion;
- expression evaluation;
- watch expressions;
- exception breakpoints;
- clean debug-session termination.

Distinguish:

- VS Code capability;
- installed .NET debugger capability;
- capability exposed to Copilot;
- capability missing only because a bridge/tool is absent.

Do not confuse “VS Code can debug C#” with “Copilot can control that debugger.”

---

# Assessment area 2: DebugMCP

Evaluate DebugMCP as the primary debugger-bridge candidate.

Assess:

- publisher;
- repository ownership;
- license;
- source availability;
- release activity;
- supported VS Code versions;
- supported Copilot mode;
- C#/.NET support;
- dependency on the existing C# debug adapter;
- local-versus-network operation;
- MCP transport;
- data exposure;
- workspace trust behavior;
- commands/tools exposed to the AI;
- breakpoint support;
- conditional/hit-count support;
- test debugging;
- launch-configuration support;
- attach support;
- call-stack support;
- variable inspection;
- expression evaluation;
- concurrent sessions;
- cancellation and termination;
- known limitations;
- open issues relevant to Windows/.NET/Copilot;
- risk of stalled debugger sessions;
- whether it can debug a deterministic headless playback scenario.

Do not install it.

Do not rely only on Marketplace marketing.

Review official Marketplace metadata, the public repository, documentation, package metadata, security policy, and relevant open issues where locally or publicly available.

---

# Assessment area 3: debugger alternatives

Evaluate a small number of credible alternatives only where they offer a meaningful distinction.

Potential categories include:

- another VS Code debugger bridge exposing Language Model Tools directly;
- another local MCP debugger bridge;
- a tool specialized for .NET;
- a tool with stronger breakpoint-hit-count or tracepoint support;
- a tool with better terminal/debug-console integration.

For each alternative, compare:

- trust and publisher;
- maturity;
- source availability;
- local operation;
- Copilot integration;
- .NET compatibility;
- feature set;
- maintenance;
- security;
- complexity;
- reason it might be preferable to DebugMCP.

Do not recommend installing several overlapping debugger bridges.

The final recommendation should normally select one initial candidate and one fallback at most.

---

# Assessment area 4: deterministic playback plus debugger workflow

Design a proposed combined workflow using existing Tempest infrastructure and debugger tooling.

The ideal model may be:

1. reproduce using a deterministic recording;
2. locate the last matching and first differing boundary using headless playback;
3. launch or reconstruct the scenario under a debugger;
4. stop on simulation tick, state transition, identity, or method of interest;
5. inspect live state;
6. step through the responsible update;
7. identify the wrong branch or mutation;
8. apply the smallest correction;
9. verify through focused tests and full recording playback.

Determine whether the current application can practically support that workflow.

Identify potential prerequisites, such as:

- a debug launch configuration;
- a debug-only boundary or tick argument;
- a breakpoint condition on the simulation tick;
- deterministic start/reconstruction support;
- a focused test reproducer;
- an attach-versus-launch strategy.

Do not implement prerequisites.

Clearly distinguish:

- already available;
- requires configuration only;
- requires a small future project change;
- blocked by debugger limitations.

---

# Assessment area 5: test discovery and targeted test execution

Assess tools that could improve:

- test discovery;
- launching one test;
- launching one parameterized test;
- debugging one test;
- viewing failures;
- navigating stack traces;
- repeating a deterministic failed test;
- identifying slow or hanging tests;
- applying explicit hang-timeout policy;
- managing large test suites;
- comparing baseline/candidate results.

Consider:

- built-in VS Code Testing APIs;
- the installed .NET test adapter;
- test-explorer integrations;
- debugger integration;
- command-line filters;
- test-result viewers.

Do not add a new test framework.

Recommend additions only if they improve evidence access or reduce repetitive manual command construction.

---

# Assessment area 6: code coverage and change-risk analysis

Assess local tools for:

- line coverage;
- branch coverage;
- coverage of changed files;
- identifying untested branches;
- displaying coverage in VS Code;
- exporting machine-readable coverage;
- comparing coverage before and after a milestone.

Evaluate whether coverage would have helped detect recent classes of Tempest defects.

Do not treat high coverage as proof of arcade fidelity.

Do not add cloud coverage services.

Prefer local .NET-compatible tooling.

Explain where coverage is useful and where recordings/manual acceptance remain stronger.

---

# Assessment area 7: static analysis and architecture validation

Assess tools that could improve:

- nullable analysis;
- Roslyn analyzers;
- code-style enforcement;
- unused-code detection;
- switch exhaustiveness;
- enum/default-value risks;
- duplicated lifecycle logic;
- architecture dependency rules;
- accidental per-level special cases;
- cyclomatic/complexity hotspots;
- code ownership and change impact;
- API compatibility.

Relate recommendations to specific Tempest risks:

- level-specific hacks;
- duplicated actor lifecycles;
- eligibility-versus-parameter confusion;
- enum/switch expansion;
- shared-route regressions;
- presentation/gameplay coupling.

Do not recommend broad analyzer churn that would create thousands of unrelated warnings during active game development.

Prefer incremental and maintainable adoption.

---

# Assessment area 8: profiling and performance tools

Assess whether local profiling tools would help with:

- simulation hot paths;
- rendering hot paths;
- allocations;
- garbage collection;
- test performance;
- campaign soak performance;
- startup;
- deterministic playback throughput;
- hangs or stalls.

Consider:

- .NET diagnostic tools;
- CPU profiling;
- allocation profiling;
- trace collection;
- counters;
- VS Code integrations.

Do not optimize without evidence.

Classify profiling as:

- currently valuable;
- useful later;
- unnecessary for present project goals.

---

# Assessment area 9: runtime tracing and logging tools

Assess improvements to:

- structured logs;
- bounded tracepoints;
- event tracing;
- state snapshots;
- log comparison;
- deterministic fingerprinting;
- baseline/candidate diffing;
- first-divergence localization;
- viewing build/test/debug output.

The goal is not to add permanent noisy logging.

Recommend tools or workflows that allow:

- targeted runtime evidence;
- temporary tracepoints;
- automatic cleanup;
- bounded output;
- low impact on deterministic simulation.

Determine what a debugger provides more cleanly than new logging.

---

# Assessment area 10: Git and repository-analysis tools

Assess tools that could improve:

- commit/history search;
- blame;
- diff navigation;
- changed-line review;
- hunk ownership;
- selective staging;
- worktree management;
- baseline/candidate comparison;
- identifying when a shared behavior changed;
- preventing accidental staging of recordings, screenshots, ROM material, or unrelated work.

Distinguish between:

- existing Git CLI capability;
- VS Code built-ins;
- extensions that materially help Copilot;
- tools useful only to Neal manually.

Do not recommend extensions solely for cosmetic Git UI.

---

# Assessment area 11: binary, assembly, and ROM inspection

Assess local tools that could help with:

- bounded hex inspection;
- address/offset mapping;
- binary comparison;
- checksum verification;
- disassembly navigation;
- .NET assembly inspection/decompilation;
- symbol inspection;
- generated IL inspection;
- ROM table extraction and validation.

Relate this to recent work:

- validating ROM chip-to-address maps;
- recovering palette blocks;
- distinguishing eligibility tables from parameter tables;
- verifying known-byte anchors.

Do not recommend tools that download or redistribute ROM material.

Prefer local, read-only analysis.

---

# Assessment area 12: dependency and security tools

Assess tools for:

- NuGet vulnerability scanning;
- dependency inventory;
- outdated package detection;
- secret scanning;
- license review;
- extension trust;
- MCP server exposure;
- local-port or named-pipe behavior;
- workspace trust;
- command approval;
- audit logs.

Do not send the repository to external scanning services.

Prefer local or official repository tooling.

Document security costs of any recommended extension.

---

# Assessment area 13: documentation, diagrams, and code navigation

Assess whether tools could improve:

- call hierarchy;
- reference search;
- symbol navigation;
- type hierarchy;
- Mermaid architecture diagrams;
- generating bounded dependency views;
- keeping behavior specifications linked to code;
- understanding lifecycle/state machines.

Do not recommend automatic documentation generators that produce large, low-value generated files.

Focus on aids for understanding complex shared behavior years later.

---

# Assessment area 14: repetitive maintenance automation

Assess tools/workflows that could safely automate:

- bounded formatting;
- static validation;
- test filters;
- fresh Release builds;
- recording gates;
- diff checks;
- report scaffolding;
- AI-Logs inventory;
- secret scans;
- closeout staging audits.

Determine whether these are better implemented as:

- existing scripts;
- VS Code tasks;
- local .NET tools;
- PowerShell scripts;
- GitHub Actions;
- Copilot tools/extensions.

Do not implement them.

Do not recommend automation that hides commands or makes failure states less visible.

---

# Assessment area 15: information-retrieval tools

Assess whether VSC would benefit from local tools for:

- indexed repository search;
- semantic code search;
- large-file search;
- cross-report search;
- AI-Logs search;
- Git history search;
- ROM/disassembly symbol lookup;
- test-to-production-code navigation.

Differentiate:

- repository-local search;
- GitHub remote search;
- web search;
- deterministic project indexes.

Do not recommend uploading private local assets to external indexing services.

---

# Credit and model-use boundary

This assessment is about tools, not more model use.

For every recommendation, identify whether it:

- uses the existing Copilot agent with additional local tools;
- invokes another AI model;
- sends data to another AI service;
- may create additional paid usage;
- has no additional model/credit cost.

Strongly prefer tools that:

- expose existing local capabilities to the current agent;
- run deterministically;
- return structured evidence;
- reduce repeated prompting and reruns;
- do not require a separate model subscription.

Do not recommend a tool merely because it includes “AI” in its marketing.

If an extension embeds or calls another model, treat that as a material negative unless explicitly authorized by Neal.

---

# Evaluation criteria

Score each serious candidate qualitatively against:

- Tempest relevance;
- evidence quality;
- time savings;
- correctness benefit;
- local operation;
- Copilot compatibility;
- .NET compatibility;
- deterministic workflow compatibility;
- security;
- maintainability;
- maturity;
- configuration complexity;
- overlap;
- failure/stall risk;
- credit/model implications.

Use classifications such as:

- HIGH VALUE
- MEDIUM VALUE
- LOW VALUE
- NOT RECOMMENDED
- DEFER

Do not invent precision scores unsupported by evidence.

---

# Recommendation tiers

Organize final recommendations into:

## Tier 1: recommended for a bounded proof

One or a small number of tools likely to deliver immediate value.

## Tier 2: useful later

Tools with real value, but not necessary before Level 18.

## Tier 3: already sufficient

Areas where current tools are adequate and no installation is recommended.

## Tier 4: not recommended

Tools that create duplication, security concerns, maintenance burden, extra AI-model use, or minimal benefit.

---

# Required proof-of-capability plan

For the highest-priority recommendation, design a future bounded validation milestone.

Do not execute it.

The proof must use local test data and must demonstrate objectively useful capability.

For debugger tooling, require at minimum:

1. start a .NET debug session;
2. hit a known breakpoint;
3. inspect the call stack;
4. inspect a local variable;
5. expand an object;
6. evaluate an expression;
7. step over;
8. step into or out where supported;
9. continue;
10. terminate cleanly;
11. remove temporary breakpoints;
12. confirm no project source/test behavior changed.

Also assess a second, Tempest-relevant proof such as:

- debugging one deterministic unit test; or
- reaching a known headless playback boundary and inspecting a state transition.

Do not begin with the interactive Raylib application unless simpler proofs pass first.

---

# Required implementation-decision output

End with one of:

## READY FOR BOUNDED TOOL INSTALLATION/VALIDATION

Include:

- preferred tool;
- exact reason;
- prerequisites;
- security considerations;
- proposed installation scope;
- proposed safe proof;
- rollback/uninstall plan;
- competing tools not selected and why.

or:

## NOT READY

List the exact missing information or compatibility blockers.

Do not install anything as part of this assessment.

---

# Required AI artifacts

Save this prompt as:

`AI-Logs/Prompts/2026-11-25-vscode-copilot-development-tool-assessment-prompt.md`

Create:

`AI-Logs/Reports/2026-11-25-vscode-copilot-development-tool-assessment-report.md`

This assessment does not authorize staging, commit, or push.

Do not add these files to the completed Level 17 closeout commits.

They belong to the separate tooling milestone.

---

# Required report contents

The report must include:

1. Executive summary
2. Starting Git/worktree state
3. Current VS Code version
4. Current .NET SDK/tool inventory
5. Installed relevant extension inventory
6. Current Copilot Agent tool inventory
7. Current C# debugger capability
8. Current debugger capability exposed to Copilot
9. Demonstrated Tempest capability gaps
10. DebugMCP assessment
11. DebugMCP publisher/source/license/security evidence
12. DebugMCP .NET compatibility
13. DebugMCP Copilot compatibility
14. DebugMCP deterministic-playback compatibility
15. Debugger alternative comparison
16. Recommended debugger candidate
17. Test-execution/tooling assessment
18. Coverage-tool assessment
19. Static-analysis assessment
20. Architecture-analysis assessment
21. Profiling assessment
22. Runtime tracing/logging assessment
23. Git/repository-analysis assessment
24. Binary/ROM/assembly-tool assessment
25. Dependency/security-tool assessment
26. Navigation/documentation-tool assessment
27. Maintenance-automation assessment
28. Information-retrieval assessment
29. Credit/model-use assessment
30. Candidate comparison matrix
31. Tier 1 recommendations
32. Tier 2 recommendations
33. Tier 3 existing-sufficient areas
34. Tier 4 rejected/deferred tools
35. Preferred tool prerequisites
36. Proposed proof-of-capability milestone
37. Security and rollback plan
38. READY/NOT READY result
39. Exact blockers if any
40. Files/resources inspected
41. Commands run
42. Production code changed: no
43. Tests changed: no
44. Configuration changed: no
45. Extensions installed/updated/removed: no
46. Settings/MCP/launch/tasks changed: no
47. Stage/commit/push: none
48. Recommended next step

Do not shorten the report because no installation occurred. The assessment itself is the requested deliverable.

---

# Stop conditions

STOP and report rather than guessing if:

- installed tool inventory cannot be obtained safely;
- VS Code extension metadata is unavailable;
- debugger prerequisites cannot be established;
- extension publisher/source identity is ambiguous;
- local-versus-external data handling cannot be determined;
- a recommendation would require trying an extension before assessing its security;
- Windows security blocks read-only inventory;
- commands stall;
- the assessment would require modifying the repository or user configuration.

Do not weaken security to gather evidence.

---

# Completion criteria

This assessment is complete only when:

- current capabilities are inventoried;
- missing capabilities are tied to real Tempest workflows;
- credible tool candidates are evaluated;
- DebugMCP is assessed against the actual environment;
- alternatives are compared without installing several overlapping tools;
- non-debugging productivity tools are evaluated;
- model/credit implications are explicit;
- one bounded priority recommendation is made or exact blockers are identified;
- a safe future proof-of-capability plan exists;
- no installation or configuration change occurred;
- repository state remains unchanged;
- nothing is staged, committed, or pushed.

Then STOP.

TEMPEST-END-VSCODE-COPILOT-TOOL-ASSESSMENT-COMPLETE