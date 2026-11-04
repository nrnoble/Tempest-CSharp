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
