# Game Over ROM-only implementation report

## Result

The final one-player death path now runs the ROM-informed vector `GAME OVER`
presentation, the state-`$24`-inspired vector-record animation, and then the
existing terminal Game Over state. The terminal R-to-restart path still starts
a fresh Level 1 game. This first reconstruction is experimental; Neal's manual
comparison and acceptance remain pending.

## Evidence and classifications

### ROM-BACKED

- The local master disassembly's shared text routine selects centered vector
  `GAME OVER`, scale 1, palette index 5; selection involves the play-state
  condition and `timectr & $20`. It establishes no fixed Game-Over-only hold.
- State `$24` is labeled a high-score explosion and is reached on a
  score-qualifying route through high-score processing, not unconditionally
  after every arcade Game Over. It hands off to state `$12` after the active
  records clear. The trace stopped at that handoff.
- The state `$24` loop manages 16 indexed records. It initializes three
  coordinates to `$80`, seeds movement values using POKEY randomness and
  `plusminus_7` signed values, applies `$10`/`$F0` bounds to the bounded
  coordinates, and creates records into empty slots while `$010E` is nonzero.
  `$010E` starts at `$20` and decrements on alternating `timectr` values:
  32 decrements over 64 timer increments. Existing records drain after spawning
  ceases; the state exits when `$010D` is zero.
- The common pending-vector layer sets palette index `$0C`. Graphic ID `$1F`
  selects local table bytes `$47` and `$4A`; the local listing does not identify
  their displayed shape. Packed ROM RGB palette bytes are not available in the
  inspected listing.

### CURRENT C# BEHAVIOR

- The existing 60-update Blaster death completes before entering
  `GameOverPresentation`. Spike-exit reverse-zoom final deaths enter the same
  sequence after their existing death interval.
- The text state uses vector line strokes. It blinks from a local
  `timectr & $20`-equivalent phase. The vector animation uses a 16-slot,
  purpose-specific record model, advances through the existing renderer's
  projection and line-drawing paths, and ends only after spawning has stopped
  and every active record has drained.
- Gameplay entities and projectiles are cleared/returned to reserve before the
  presentation. Presentation steps ignore player/gameplay input and do not
  advance combat, score, lives or wave progression. The game reaches the
  existing terminal Game Over state only after the vector records clear.
- R is ignored during both presentation states and remains available in the
  terminal state. Restart resets score/lives and starts Level 1.

### PROJECT DECISION / APPROXIMATION

- The state-`$24`-inspired animation runs after every final one-player C# death.
  This is Neal's requested project decision and is deliberately not represented
  as literal arcade control flow.
- A 120-tick text hold is a project approximation because the ROM establishes
  no Game-Over-only duration. The local blink-clock phase is also a C#
  translation of the shared ROM timer behavior.
- ROM byte coordinates are translated through the existing perspective
  projection; each record is rendered as a small diamond wireframe. The shape,
  screen distribution and scale are localized approximations, not identified
  Atari graphics.
- ROM palette indices 5 and `$0C` are retained. The renderer maps them to
  existing C# white and cyan colors as RGB approximations; no exact color match
  is claimed.
- If both bounded velocities are zero, a one-unit bounded velocity is assigned
  so a randomized record cannot prevent completion forever. This rare-case
  drain guard is a project approximation.

### UNKNOWN / PENDING

- The displayed geometry represented by `$47/$4A`, exact RGB values, timing
  translation, vector distribution and visual fidelity remain unknown.
- No screenshots, video, MAME captures, Internet sources or other visual
  references were searched for or used. Manual comparison with Neal's original
  arcade machine remains pending.

## Files changed

### Production

- `src/Tempest.Game/Levels/LevelState.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/GameOverVectorAnimation.cs`
- `src/Tempest.Game/Rendering/VectorFont.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`

### Tests

- `tests/Tempest.Game.Tests/PlayerDeathPresentationTests.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `tests/Tempest.Game.Tests/LevelProgressionTests.cs`
- `tests/Tempest.Game.Tests/Level4CandidateTests.cs`
- `tests/Tempest.Game.Tests/ExitLaneControlTests.cs`

### Documentation and logs

- `.github/copilot-instructions.md`
- `TEMPEST-DEVELOPMENT-NOTES.md`
- `TEMPEST-BEHAVIOR-SPEC.md`
- `AI-Logs/Prompts/2026-11-05-game-over-rom-only-implementation-prompt.md`
- `AI-Logs/Reports/2026-11-05-game-over-rom-only-implementation-report.md`

## Automated validation

- Focused Game Over/death/rendering tests: **45 passed, 0 failed, 0 skipped**.
- Neighboring Levels 1-7, death, renderer, progression, soak, Super Zapper,
  rim-pursuit, exit and restart regression: **698 passed, 0 failed, 0 skipped**.
- Full `dotnet test TempestCSharp.slnx`: **788 passed, 0 failed, 0 skipped**.
- `dotnet build TempestCSharp.slnx`: **0 warnings, 0 errors**.
- `git diff --check`: passed.

## Manual comparison

No direct Game Over test mode was added; the ordinary one-life command reuses
the production path:

```powershell
dotnet run --project src\Tempest.Game -- --lives 1
```

After launch, evaluate:

1. Does `GAME OVER` look and behave like the arcade?
2. Does the brief animation afterward immediately resemble the arcade effect?
3. Are its vector shapes, movement, distribution and color broadly correct?
4. Does the transition from `GAME OVER` into the animation resemble the arcade?
5. Does the animation end naturally?
6. Is the terminal R-to-restart placeholder acceptable?
7. Which aspects, if any, are visibly wrong?

No interactive game launch was performed as part of this implementation.
