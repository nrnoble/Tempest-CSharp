# Direct-Hatched Fuseball Milestone

## 1. Source behavior implemented

Verified against Incitti's `Fuseball`, `FindShortDir`, `fusepoint`,
`ReadFuseballData`, hatch call, and speed setup. Implemented height-zero direct
hatch, initial lateral state/w=0/direction=1, continuous wrap and open endpoint
bounce, random retarget/phase decisions, climb/descent, strict collision bands,
height-derived 250-point score steps, and four-frame/five-arm colored animation.

The earlier analysis's "8-update cadence" disagrees with literal source:
`cnt` increments before `cnt > 8`, giving nine updates per lateral step.
Vertical duration is rerolled every update against `25 * Rand(1,3)`, not
selected once on state entry. The implementation preserves these source rules.

## 2. Development scenario

`--fuseball-test` selects ten direct-hatched Fuseballs on the existing tube,
using the established Egg schedule and ordinary one-unit accounting. Composition
is deterministic; movement decisions remain random and test-injectable.
Speed is the unscaled source term 3.3, not a claim about a particular arcade
level. Spiker/Tanker scenarios are mutually exclusive with this flag.
The enemy-bullets flag adds no firing to this Fuseball-only wave.

## 3. Files changed

- `src/Tempest.Game/Gameplay/Fuseball.cs`
- `src/Tempest.Game/Gameplay/FuseballScorePopup.cs`
- `src/Tempest.Game/TempestGame.cs`
- `src/Tempest.Game/Debug/DebugController.cs`
- `src/Tempest.Game/Program.cs`
- `src/Tempest.Game/Rendering/VectorRenderer.cs`
- `tests/Tempest.Game.Tests/SimulationTests.cs`
- `README.md`
- This report and the requested prompt archive (locally saved; AI-Logs is ignored).

## 4. Important design decisions

Simulation remains rendering-independent. Fuseballs use the existing
insertion-ordered development shot path and normal death/fresh-Egg accounting;
there is no cargo ledger. Shot collision still precedes enemy movement/player
collision. `w/8` interpolates from the current tube point toward the next;
only lateral rim travel with `1 < w < 7` kills, regardless of player sub-lane
offset. The score label is Fuseball-specific, expires after 30 ticks, and uses
the source height-based scale/lane-midpoint anchor without a general framework.

## 5. Tests added and test result

Added focused deterministic cases for start/hatch/accounting, literal lateral
cadence, both wrap/bounce directions, retarget/tie behavior, vertical duration
rolls, descending clamp, strict shot/rim collision boundaries, score thresholds,
kill/popup lifetime, life reconstruction, completion blocking, and animation
geometry/colors/projection.

An intermediate focused run passed 38/38. The latest version adds the w=8
boundary and refines popup lifetime/animation assertions (39 focused cases).
Build succeeds with 0 warnings and 0 errors.

**Final runtime validation is blocked:** two standard focused attempts could
not load the rebuilt game assembly because Windows Application Control
reported `0x800711C7`. No policy settings or bypasses were attempted.
The latest focused tests and full regression suite remain unverified; 205
total cases are expected, not claimed passed. Validation is paused pending
resolution of that external policy block.

## 6. Manual validation instructions

After the policy block is resolved, run:

```powershell
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore --filter "FullyQualifiedName~Fuseball"
dotnet test tests\Tempest.Game.Tests\Tempest.Game.Tests.csproj --no-restore
dotnet run --project src\Tempest.Game -- --fuseball-test --lives 10
```

Compare boundary travel, climb/descent timing, five-color animation and score
labels. Check rim contact, life loss/re-entry, and replay with R. Launch normal,
Spiker and Tanker modes separately to confirm their established appearance.
Manual gameplay validation has not been performed.

## 7. Deferred/uncertain arcade behavior

Incitti behavior is not proof of arcade accuracy. Compare movement, speed,
collision feel and animation manually. Source score 1250 occurs only at height
zero; ordinary shots expire below zero before collision, so the formula's
1250 endpoint is not normally reachable by shooting in this direct-hatch path.
Popup text uses the existing Raylib font and remains tube-projected during
exit, rather than reproducing Incitti's vector-font/screen-fixed label exactly.
Death freezes Fuseball movement/animation consistently with existing C# enemies.

No Tanker-carried Fuseballs, Zapper, hurry-up, Pulsars, progression, audio,
bonuses, or overlapping-Flipper changes were implemented. No commit or push.
