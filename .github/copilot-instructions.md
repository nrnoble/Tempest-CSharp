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

Maintainability:
- Code must be maintainable by a developer returning to the project years later without access to AI conversation history. Add concise comments explaining class responsibilities, gameplay state machines, non-obvious algorithms, arcade-specific rules, important ordering requirements, unusual constants, and design decisions whose purpose is not evident from the code itself. Prefer comments explaining why over comments restating what the C# statement does. Do not add comments to trivial properties, obvious assignments, simple loops, or self-explanatory code.
- Comments must distinguish established arcade behavior, behavior derived from Incitti, development-only behavior, and approximations needing arcade comparison.

Process:
- Do not replace a working implementation merely because another design is cleaner; design changes need demonstrated benefit.
- Identify disagreements among arcade, Incitti and the C# implementation explicitly.
- Avoid broad refactoring during bug fixes.
- Preserve validated behavior unless a change is explicitly authorized.
- Report any genuine instruction conflict rather than resolving it silently.
