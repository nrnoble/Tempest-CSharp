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
