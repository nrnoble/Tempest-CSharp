using System.Numerics;
using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

/// <summary>
/// Level state: tube geometry, Egg release and normal enemy accounting. Normal accounting conserves
/// EnemyTotal: every enemy is reserve (EnemyTotal - EnemiesSpawned), an Egg/active unit, destroyed
/// (EnemiesDestroyed), or rim-cleared (EnemiesRimCleared). After a death, EnemiesSpawned is rewound
/// to the resolved count so only unresolved enemies re-enter as fresh Eggs. Spawn lane order and
/// timers are Incitti-derived and unvalidated against the arcade. Tanker queues are development-only
/// (<c>--tanker-test</c>).
/// </summary>
public sealed class Level
{
    // Depth is the rim; heights run from 0 (far end) up to Depth (near the player).
    public const float Depth = 400f;
    public const float Position = 50;
    public const int EnemyTotal = 10;
    private const int SpawnIntervalTicks = 50;
    private const int MaximumActiveEggs = 4;
    private const int MaximumApproachingEggs = 2;
    private const int HatchSelectionIntervalTicks = 50;
    private readonly int[] spawnLanes = [12, 4, 9, 1, 14, 6, 10, 2, 15, 7];
    private readonly List<Egg> eggs = [];
    private readonly List<Spike> spikes = [];
    private readonly Queue<int> tankerPendingOriginalSlots = new();
    private readonly Queue<int> tankerPendingCargoUnits = new();
    private int spawnTimer;
    private int hatchSelectionTimer;
    private int nextSpawnLaneIndex;

    public LevelDefinition Definition { get; } = LevelLoader.LoadLevel1();
    public int EnemiesSpawned { get; private set; }
    public int EnemiesDestroyed { get; private set; }
    public int EnemiesRimCleared { get; private set; }
    // Development-only tanker accounting counts reserve as queued original slots; normal (and
    // Spiker) accounting derives reserve from the conserved ten-unit ledger.
    public int ReserveEggs => TankerDevelopmentWave
        ? tankerPendingOriginalSlots.Count
        : EnemyTotal - EnemiesSpawned;
    public int PendingTankerCargoEggs => TankerDevelopmentWave ? tankerPendingCargoUnits.Count : 0;
    public int RemainingEnemies => EnemyTotal - EnemiesDestroyed - EnemiesRimCleared;
    public IReadOnlyList<Egg> Eggs => eggs;
    public IReadOnlyList<Spike> Spikes => spikes;
    public event Action<Spike>? SpikeCreated;
    public bool SpikerDevelopmentWave { get; }
    public bool TankerDevelopmentWave { get; }
    // Level stores the state value, but every transition is owned by TempestGame's step logic;
    // nothing in Level changes State.
    public LevelState State { get; set; } = LevelState.Begin;
    public bool Continuous { get; set; } = true;

    public Level(bool spikerDevelopmentWave = false, bool tankerDevelopmentWave = false)
    {
        SpikerDevelopmentWave = spikerDevelopmentWave;
        TankerDevelopmentWave = tankerDevelopmentWave;
        Reset();
    }

    public void Reset()
    {
        EnemiesDestroyed = 0;
        EnemiesRimCleared = 0;
        tankerPendingOriginalSlots.Clear();
        tankerPendingCargoUnits.Clear();
        if (TankerDevelopmentWave)
        {
            for (int originalSlotId = 0; originalSlotId < EnemyTotal; originalSlotId++)
                tankerPendingOriginalSlots.Enqueue(originalSlotId);
        }
        spikes.Clear();
        if (SpikerDevelopmentWave)
            for (int lane = 0; lane < Definition.Points.Count; lane++)
                CreateSpike(lane, 60);
        ResetAfterDeath();
    }

    public Spike CreateSpike(int lane, float height)
    {
        Spike spike = new() { EdgeIndex = lane, Height = height };
        spikes.Add(spike);
        SpikeCreated?.Invoke(spike);
        return spike;
    }

    public void RemoveSpike(Spike spike) => spikes.Remove(spike);

    public void ReturnEnemyToReserve() => EnemiesSpawned--;

    // Life-loss reconstruction: survivors return to reserve, then up to the four-active-Egg cap is
    // immediately refilled with fresh Eggs. Also runs at level start, where it performs the
    // initial four-Egg release.
    public void ResetAfterDeath()
    {
        ReturnSurvivorsToReserve();
        for (int eggIndex = 0; eggIndex < MaximumActiveEggs && ReserveEggs > 0; eggIndex++)
            ReleaseEgg();
    }

    // Rim clears are resolved but not kills, so reconstruction must retain their disposition and
    // must not recreate them as Eggs. Realigning the lane sequence to both resolved outcomes keeps
    // only unresolved units in the fresh-Egg supply after death or spike-exit retry.
    public void ReturnSurvivorsToReserve()
    {
        eggs.Clear();
        spawnTimer = 0;
        hatchSelectionTimer = 0;
        EnemiesSpawned = EnemiesDestroyed + EnemiesRimCleared;
        nextSpawnLaneIndex = EnemiesSpawned % spawnLanes.Length;
    }

    public void RecordEnemyDestroyed() => EnemiesDestroyed++;

    public void RecordEnemyRimCleared() => EnemiesRimCleared++;

    // Reserve release cadence: one Egg every 50 ticks, only while under the four-active cap.
    // Incitti-derived pacing; unvalidated against the arcade.
    public void UpdateEggRelease()
    {
        spawnTimer++;
        if (spawnTimer < SpawnIntervalTicks)
            return;

        spawnTimer = 0;
        if (eggs.Count < MaximumActiveEggs &&
            (ReserveEggs > 0 || PendingTankerCargoEggs > 0))
            ReleaseEgg();
    }

    private void ReleaseEgg()
    {
        int lane = spawnLanes[nextSpawnLaneIndex % spawnLanes.Length];
        // Alternate directions in release order to keep roaming repeatable.
        int direction = nextSpawnLaneIndex % 2 == 0 ? 1 : -1;
        nextSpawnLaneIndex++;
        if (TankerDevelopmentWave)
        {
            ReleaseTankerEgg(lane, direction);
            return;
        }

        eggs.Add(new Egg(lane, direction));
        EnemiesSpawned++;
    }

    private void ReleaseTankerEgg(bool preferCargo = false)
    {
        int lane = spawnLanes[nextSpawnLaneIndex % spawnLanes.Length];
        int direction = nextSpawnLaneIndex % 2 == 0 ? 1 : -1;
        nextSpawnLaneIndex++;
        ReleaseTankerEgg(lane, direction, preferCargo);
    }

    // Tanker-mode Egg identity: restored cargo units take priority only when explicitly preferred
    // (life-loss restore), otherwise original slots drain first, then newly materialized cargo.
    // The dequeued id travels with the Egg so the same ledger entry survives death/rebirth cycles.
    private void ReleaseTankerEgg(int lane, int direction, bool preferCargo = false)
    {
        if (preferCargo && tankerPendingCargoUnits.TryDequeue(out int preferredCargoId))
            eggs.Add(new Egg(lane, direction, cargoUnitId: preferredCargoId));
        else if (tankerPendingOriginalSlots.TryDequeue(out int originalSlotId))
            eggs.Add(new Egg(lane, direction, originalSlotId));
        else if (tankerPendingCargoUnits.TryDequeue(out int cargoUnitId))
            eggs.Add(new Egg(lane, direction, cargoUnitId: cargoUnitId));
        EnemiesSpawned++;
    }

    // Tanker-mode life-loss reconstruction. Callers pass the surviving identities; everything is
    // rebuilt as fresh Eggs (resolved slots stay resolved because they are simply not passed in).
    // Distinct()/Order() make the rebuild deterministic regardless of destruction order, and
    // preferCargo: true returns surviving cargo to play before untouched original slots.
    public void RestoreTankerEntries(IEnumerable<int> originalSlotIds, IEnumerable<int> cargoUnitIds)
    {
        if (!TankerDevelopmentWave)
            return;

        eggs.Clear();
        tankerPendingOriginalSlots.Clear();
        tankerPendingCargoUnits.Clear();
        foreach (int slotId in originalSlotIds.Distinct().Order())
            tankerPendingOriginalSlots.Enqueue(slotId);
        foreach (int cargoId in cargoUnitIds.Distinct().Order())
            tankerPendingCargoUnits.Enqueue(cargoId);

        EnemiesSpawned = EnemyTotal - tankerPendingOriginalSlots.Count;
        nextSpawnLaneIndex = (EnemiesSpawned + tankerPendingCargoUnits.Count) % spawnLanes.Length;
        spawnTimer = 0;
        hatchSelectionTimer = 0;
        for (int eggIndex = 0; eggIndex < MaximumActiveEggs; eggIndex++)
        {
            if (tankerPendingOriginalSlots.Count == 0 && tankerPendingCargoUnits.Count == 0)
                break;
            ReleaseTankerEgg(preferCargo: true);
        }
    }

    // Development determinism: original slot 0 is always the one Tanker in --tanker-test, so its
    // identity is stable across deaths, restarts and tests. Not arcade behavior.
    public int? TankerSlotId => TankerDevelopmentWave ? 0 : null;

    public IReadOnlyList<int> PendingTankerCargoUnitIds => tankerPendingCargoUnits.ToArray();

    public void UpdateEggs()
    {
        hatchSelectionTimer++;
        if (hatchSelectionTimer >= HatchSelectionIntervalTicks)
        {
            hatchSelectionTimer = 0;
            // Every 50 active ticks, select the oldest roaming Egg, with at most
            // two in transit. This preserves the milestone's deterministic pacing.
            if (eggs.Count(egg => egg.State != EggState.Roaming) < MaximumApproachingEggs)
                eggs.FirstOrDefault(egg => egg.State == EggState.Roaming)?.BeginApproach();
        }

        foreach (Egg egg in eggs)
            egg.Update(this);
    }

    public Flipper? HatchReadyEgg() => (Flipper?)HatchReadyEnemy(lane => new Flipper(lane, 0));

    public Enemy? HatchReadyEnemy(Func<int, Enemy> createEnemy)
        => HatchReadyEnemyWithIdentity(egg => createEnemy(egg.EdgeIndex));

    // Hatch consumes the oldest ready Egg; the factory receives the Egg so Tanker-mode identity
    // (OriginalSlotId/CargoUnitId) can be transferred onto the hatched enemy.
    public Enemy? HatchReadyEnemyWithIdentity(Func<Egg, Enemy> createEnemy)
    {
        int eggIndex = eggs.FindIndex(egg => egg.State == EggState.ReadyToHatch);
        if (eggIndex < 0)
            return null;

        Egg egg = eggs[eggIndex];
        Enemy enemy = createEnemy(egg);
        eggs.RemoveAt(eggIndex);
        return enemy;
    }

    public bool IsComplete(int activeFlippers) =>
        EnemiesDestroyed + EnemiesRimCleared == EnemyTotal &&
        ReserveEggs == 0 && eggs.Count == 0 && activeFlippers == 0;

    public int NextEdge(int edge) => (edge + 1) % Definition.Points.Count;
    public int PreviousEdge(int edge) =>
        (edge - 1 + Definition.Points.Count) % Definition.Points.Count;

    // Sign convention matches Flipper pivots: +1 means stepping toward PreviousEdge (decreasing
    // index), -1 toward NextEdge; wrap-around distances count correctly on the closed tube.
    // Equidistant ties resolve to +1; regression tests pin these rules for Flipper pathing.
    public int FindShortestDirection(int fromEdge, int toEdge)
    {
        int edgeCount = Definition.Points.Count;
        int clockwise = (toEdge - fromEdge + edgeCount) % edgeCount;
        int counterclockwise = (fromEdge - toEdge + edgeCount) % edgeCount;
        return clockwise < counterclockwise ? -1 : 1;
    }

    // Pivot sweep for a Flipper moving between two lanes: the angle between the lanes' edge
    // direction vectors, folded into [0, 180]. On a regular 16-lane tube this is constant, but it
    // is computed from geometry so irregular future tubes keep correct flip arcs.
    public float CalculateEdgeTurnAngle(int sourceEdge, int destinationEdge)
    {
        IReadOnlyList<Vector2> points = Definition.Points;
        Vector2 sourceVector = points[NextEdge(sourceEdge)] - points[sourceEdge];
        Vector2 destinationVector = points[NextEdge(destinationEdge)] - points[destinationEdge];
        float sourceAngle = MathF.Atan2(sourceVector.Y, sourceVector.X);
        float destinationAngle = MathF.Atan2(destinationVector.Y, destinationVector.X);
        float difference = MathF.Abs(NormalizeAngleDegrees(
            (destinationAngle - sourceAngle) * 180 / MathF.PI));
        return difference > 180 ? 360 - difference : difference;
    }

    private static float NormalizeAngleDegrees(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }
}
