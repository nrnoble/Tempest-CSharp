# Tempest C# project instructions

This is a personal C# recreation inspired by Mark Incitti's Tempest 3.1 BlitzMax implementation and the original Atari Tempest gameplay.

Goals:
- Preserve the rhythm, timing, scoring feel, enemy behavior, tube movement, and warp behavior that made the BlitzMax version convincing.
- Do not pursue cycle-accurate Atari hardware emulation.
- Prefer readable, testable modern C# over line-for-line BlitzMax transliteration.
- Keep simulation logic independent of rendering.
- Use a fixed 60 Hz gameplay simulation initially because the reference BlitzMax game targets an approximately 16 ms main loop.
- Preserve frame/tick-based values from the reference until behavior has been validated.
- Avoid speculative gameplay changes. If behavior is unclear, mark it TODO and compare with the BlitzMax reference before changing it.
- Keep level/tube data data-driven.
- Tempest visuals are vector-based: prefer established geometry, projection, line rendering, palette and animation systems over sprites, textures, bitmaps or generic particles. Distinguish ROM-backed behavior from project approximations.
- Add debug/single-step facilities early.
- Favor small classes and explicit state enums over global state.

Initial milestone:
1. Draw one tube.
2. Move the player left/right around the tube.
3. Add relative mouse/spinner input.
4. Fire player shots.
5. Add one Flipper.
6. Add collision and scoring.

Code comments:
- When creating or modifying code, add appropriate maintainability comments by default unless explicitly instructed otherwise. Comments should help a developer returning years later understand non-obvious behavior and design decisions.
- Prefer explaining why behavior exists, especially non-obvious gameplay rules, arcade-specific behavior that might look like a bug, timing and update-order requirements, state transitions and lifecycle invariants, unusual geometry or rendering, deterministic or accounting rules, and deliberate compatibility, fidelity, or engineering decisions.
- Keep comments concise. Do not comment on straightforward code or merely restate obvious assignments, simple loops, ordinary conditions, self-explanatory method names, or clear syntax. Preserve useful comments unless behavior changes; update affected comments when it does.
- Comments must distinguish established arcade behavior, behavior derived from Incitti, development-only behavior, and approximations needing arcade comparison.

AI prompts and reports:
- For substantial development, investigation, analysis, debugging, or milestone work performed by VS Code Copilot, preserve both the user-provided prompt and the resulting AI report as durable project records.
- Save substantial prompts under `AI-Logs/Prompts/`.
- Save corresponding reports under `AI-Logs/Reports/`.
- Use descriptive, date-prefixed filenames that make related prompts and reports easy to associate, for example `2026-11-03-level5-implementation-prompt.md` and `2026-11-03-level5-implementation-report.md`.
- Preserve the complete substantive user-provided task in the prompt record, including its scope, constraints, validation requirements, manual-acceptance requirements, and completion token. Do not include VS Code-generated reminders, tagged-file metadata, attachment metadata, or other automatically appended agent context merely because it followed the user prompt.
- The report should concisely record what was investigated or implemented, files changed, important design or evidence findings, validation performed and results, unresolved issues, limitations, and any manual validation still required from Neal.
- AI reports are historical development records, not authoritative evidence of arcade behavior. `TEMPEST-DEVELOPMENT-NOTES.md` and `TEMPEST-BEHAVIOR-SPEC.md` remain the durable project records for accepted development decisions and behavior/evidence classifications.
- Keep logging proportional. Routine one-line questions, simple command lookups, trivial read-only checks, and minor conversational follow-ups do not require dedicated prompt/report files unless explicitly requested.

Process:
- VS Code Copilot must not launch the interactive Tempest game for manual acceptance. After automated validation, stop and provide the exact launch command: `dotnet run --project src\Tempest.Game -- --lives 10`. Neal will launch and test it; he often walks away while agent prompts run because completion time is unpredictable.
- Future automated gameplay verification using deterministic, recorded or synthetic controller/input events is desirable for regression testing, but supplements rather than replaces Neal's manual gameplay acceptance.
- Do not replace a working implementation merely because another design is cleaner; design changes need demonstrated benefit.
- Identify disagreements among arcade, Incitti and the C# implementation explicitly.
- Avoid broad refactoring during bug fixes.
- Preserve validated behavior unless a change is explicitly authorized.
- Report any genuine instruction conflict rather than resolving it silently.

Command, validation, and recovery safety:
- Bound potentially long-running tests, simulations, state searches, probes, process waits, polling, and loops with a task-appropriate timeout, tick/iteration/seed/level limit, or other explicit termination condition. Do not impose one arbitrary timeout on every command. Helpers waiting for gameplay state must have a hard bound and, when practical, report relevant state on exhaustion.
- If a command makes no meaningful progress beyond its reasonable expected duration, stop it, inspect available output/process state, and isolate the cause with progressively smaller bounded runs as appropriate. Do not immediately rerun the same unrestricted operation. Distinguish stale test assumptions, unbounded helpers, production defects, security blocks, and infrastructure failures when practical; correct tests only when expected production behavior is established, and do not weaken valid tests to hide production defects.
- Treat Windows Application Control, Smart App Control, Code Integrity, and other Windows security blocks on generated or required executables/assemblies as security or infrastructure results, not ordinary test failures. Record the exact command, artifact and path, error/message, and error code when available. Never weaken or disable security, change policy, bypass it, or ask Neal to do so. Do not repeatedly rebuild or rerun the same blocked artifact or create equivalent disposable probes. Retry only through a materially different safe route; never substitute stale binaries for the current candidate or claim blocked/incomplete work validated it.
- A canceled, timed-out, hung, stalled, security-blocked, terminated, killed, or otherwise incomplete command is never a pass. Report applicable outcomes distinctly as PASS, FAIL, BLOCKED, TIMEOUT, CANCELED, or PARTIAL, and claim validation passed only when all validation required for that claim completed successfully.
- If required work cannot continue safely or reliably, stop voluntarily rather than waiting indefinitely. Preserve candidate work, unrelated local files, and Git state; do not pretend the task is complete. When possible, create the requested AI report and mark it BLOCKED or PARTIAL, stating what completed and remains, the blocker, validation status, and Neal's precise next action.
- If required final validation is blocked, stalled, timed out, canceled, or incomplete, do not stage, commit, or push unless the user explicitly authorized a partial checkpoint under those conditions. For normal milestones, stop before commit and preserve the worktree.
- These are project-wide defaults; future prompts need not repeat them. Task-specific prompts continue to define implementation scope, required validation, manual acceptance, evidence, and Git authorization; they may set a tighter bound or explicitly authorize an exception.
