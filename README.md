# Tempest C# Recreation

Personal C# recreation based on the gameplay behavior of Mark Incitti's Tempest 3.1 BlitzMax project.

## Initial goal
Get one tube playable with player movement, shooting, a Flipper, collision, scoring, and a fixed 60 Hz simulation.

## VS Code
Install the current .NET SDK and Microsoft's C# Dev Kit extension, then open this folder in VS Code.

```powershell
dotnet build
dotnet test
dotnet run --project src/Tempest.Game
```

## Code organization
- `Program.cs` composes the game, input and renderer, and owns the window and frame loop.
- `TempestGame.RunOneStep` advances one gameplay tick without calling Raylib.
- `Gameplay` owns player movement, shots and the Flipper state machine.
- `Levels` owns the current tube geometry, deterministic spawn sequence and wave counters.
- `Systems/InputSystem.cs` samples Raylib input; `ScoreSystem.cs` owns the score.
- `Rendering/VectorRenderer.cs` draws the tube, entities, explosion and HUD.

The active tick order is player/firing, shots and shot collisions, reserve Egg
release, Egg updates, hatching, Flipper updates, completed-pivot player
collisions, then level completion.
The frame loop retains one simulation tick per frame at the existing 60 FPS target.

Level 1 starts with four roaming Eggs and six in reserve. Reserve releases use
a 50-tick interval and a four-active-Egg limit. Every 50 active ticks the oldest
roaming Egg is selected to approach, with at most two approaching or ready to
hatch. Release lanes and alternating roaming directions remain deterministic.
Eggs hatch into Flippers without changing the remaining-enemy count. After a
life loss, surviving enemies return to the Egg supply, with up to four released
immediately; defeated enemies and score remain preserved. The HUD's ACTIVE
counter includes both active Eggs and active Flippers.

Tests run headlessly without initializing a Raylib window. Visual appearance,
spinner feel and cursor capture still require manual gameplay testing.

## Reference mapping
- `tempest.bmx` -> gameplay and state behavior
- `transformfunctions.bmx` -> `Rendering/Projection.cs`
- `vectorfont.bmx` -> `Rendering/VectorFont.cs`
- levelset files -> `Levels/LevelLoader.cs` / data files later

The repository-specific Copilot guidance is in `.github/copilot-instructions.md`.
