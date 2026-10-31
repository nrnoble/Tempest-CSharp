using System.Numerics;
using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

/// <summary>
/// Level state: tube geometry, Egg release and normal enemy accounting. Normal accounting conserves
/// configured supply: every enemy is reserve (supply - EnemiesSpawned), an Egg/active unit, destroyed
/// (EnemiesDestroyed), or rim-cleared (EnemiesRimCleared). After a death, EnemiesSpawned is rewound
/// to the resolved count so only unresolved enemies re-enter as fresh Eggs. Spawn lane order and
/// timing and deterministic sequences are project decisions, not literal Incitti behavior.
/// Tanker queues are development-only
/// (<c>--tanker-test</c>).
/// </summary>
public sealed class Level
{
    // Depth is the rim; heights run from 0 (far end) up to Depth (near the player).
    public const float Depth = 400f;
    public const float Position = 50;
    public static int EnemyTotal => LevelConfiguration.Level1.EnemySupply;
    private readonly List<Egg> eggs = [];
    private readonly List<Spike> spikes = [];
    private readonly Queue<int> tankerPendingOriginalSlots = new();
    private readonly Queue<int> tankerPendingCargoUnits = new();
    private int spawnTimer;
    private int hatchSelectionTimer;
    private int nextSpawnLaneIndex;

    public LevelConfiguration Configuration { get; }
    public LevelDefinition Definition => Configuration.Definition;
    public int EnemySupply => Configuration.EnemySupply;
    public int PlayableLaneCount => Configuration.PlayableLaneCount;
    public int EnemiesSpawned { get; private set; }
    public int EnemiesDestroyed { get; private set; }
    public int EnemiesRimCleared { get; private set; }
    // Development-only tanker accounting counts reserve as queued original slots; normal (and
    // Spiker) accounting derives reserve from the configured supply.
    public int ReserveEggs => TankerDevelopmentWave
        ? tankerPendingOriginalSlots.Count
        : EnemySupply - EnemiesSpawned;
    public int PendingTankerCargoEggs => TankerDevelopmentWave ? tankerPendingCargoUnits.Count : 0;
    public int RemainingEnemies => EnemySupply - EnemiesDestroyed - EnemiesRimCleared;
    public IReadOnlyList<Egg> Eggs => eggs;
    public IReadOnlyList<Spike> Spikes => spikes;
    public event Action<Spike>? SpikeCreated;
    public bool SpikerDevelopmentWave { get; }
    public bool TankerDevelopmentWave { get; }
    // Level stores the state value, but every transition is owned by TempestGame's step logic;
    // nothing in Level changes State.
    public LevelState State { get; set; } = LevelState.Begin;
    public bool Continuous => Configuration.Definition.Continuous;

    public Level(bool spikerDevelopmentWave = false, bool tankerDevelopmentWave = false)
        : this(LevelConfiguration.Level1, spikerDevelopmentWave, tankerDevelopmentWave)
    {
    }

    public Level(LevelConfiguration configuration, bool spikerDevelopmentWave = false,
        bool tankerDevelopmentWave = false)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        Configuration = configuration;
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
            for (int originalSlotId = 0; originalSlotId < EnemySupply; originalSlotId++)
                tankerPendingOriginalSlots.Enqueue(originalSlotId);
        }
        spikes.Clear();
        if (SpikerDevelopmentWave)
            for (int lane = 0; lane < PlayableLaneCount; lane++)
                CreateSpike(lane, 60);
        ReturnSurvivorsToReserve();
        for (int eggIndex = 0; eggIndex < Configuration.InitialActiveEggCount && ReserveEggs > 0; eggIndex++)
            ReleaseEgg();
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

    // Life reconstruction retains this Level and restores unresolved units as fresh Eggs. Fresh
    // wave construction instead uses the configured initial Egg count in Reset.
    public void ResetAfterDeath()
    {
        ReturnSurvivorsToReserve();
        for (int eggIndex = 0; eggIndex < Configuration.MaximumActiveEggs && ReserveEggs > 0; eggIndex++)
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
        nextSpawnLaneIndex = EnemiesSpawned % Configuration.SpawnLaneSequence.Count;
    }

    public void RecordEnemyDestroyed() => EnemiesDestroyed++;

    public void RecordEnemyRimCleared() => EnemiesRimCleared++;

    // Reserve release cadence is configuration data; its current values are project decisions.
    public void UpdateEggRelease()
    {
        spawnTimer++;
        if (spawnTimer < Configuration.ReserveReleaseIntervalTicks)
            return;

        spawnTimer = 0;
        if (eggs.Count < Configuration.MaximumActiveEggs &&
            (ReserveEggs > 0 || PendingTankerCargoEggs > 0))
            ReleaseEgg();
    }

    private void ReleaseEgg()
    {
        int lane = Configuration.SpawnLaneSequence[nextSpawnLaneIndex % Configuration.SpawnLaneSequence.Count];
        int direction = Configuration.EggDirectionSequence[
            nextSpawnLaneIndex % Configuration.EggDirectionSequence.Count];
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
        int lane = Configuration.SpawnLaneSequence[nextSpawnLaneIndex % Configuration.SpawnLaneSequence.Count];
        int direction = Configuration.EggDirectionSequence[
            nextSpawnLaneIndex % Configuration.EggDirectionSequence.Count];
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

        EnemiesSpawned = EnemySupply - tankerPendingOriginalSlots.Count;
        nextSpawnLaneIndex = (EnemiesSpawned + tankerPendingCargoUnits.Count) %
            Configuration.SpawnLaneSequence.Count;
        spawnTimer = 0;
        hatchSelectionTimer = 0;
        for (int eggIndex = 0; eggIndex < Configuration.MaximumActiveEggs; eggIndex++)
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
        if (hatchSelectionTimer >= Configuration.HatchSelectionIntervalTicks)
        {
            hatchSelectionTimer = 0;
            // Every 50 active ticks, select the oldest roaming Egg, with at most
            // two in transit. This preserves the milestone's deterministic pacing.
            if (eggs.Count(egg => egg.State != EggState.Roaming) < Configuration.MaximumApproachingEggs)
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
        EnemiesDestroyed + EnemiesRimCleared == EnemySupply &&
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

    // Literal Incitti FindShortDir for the existing 16-lane reference scenarios. Keep the
    // established Level 1 tie policy above separate: same-lane ties here consume one RNG call.
    public int FindReferenceShortestDirection(int fromEdge, int toEdge,
        Func<int, int, int>? randomInclusive)
    {
        int direction = -(toEdge - fromEdge);
        direction = Continuous && Math.Abs(direction) > 8
            ? -Math.Sign(direction)
            : Math.Sign(direction);
        if (direction != 0)
            return direction;

        ArgumentNullException.ThrowIfNull(randomInclusive);
        return 1 - randomInclusive(0, 1) * 2;
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

    public float CalculateFlipperTurnAngle(int sourceEdge, int destinationEdge) =>
        Configuration.FlipperPivotMode switch
        {
            FlipperPivotGeometry.LegacyLaneTurn =>
                CalculateEdgeTurnAngle(sourceEdge, destinationEdge),
            FlipperPivotGeometry.SquareEndpointHinge =>
                MathF.Abs(CalculateSquareFlipperHingeRotation(sourceEdge, destinationEdge).Degrees),
            _ => throw new InvalidOperationException("The configured Flipper pivot geometry is unsupported.")
        };

    public float CalculateSquareFlipperHingeRotationDegrees(int sourceEdge, int destinationEdge)
    {
        if (Configuration.FlipperPivotMode != FlipperPivotGeometry.SquareEndpointHinge)
            throw new InvalidOperationException("Square hinge angles require the square Flipper pivot configuration.");

        return CalculateSquareFlipperHingeRotation(sourceEdge, destinationEdge).Degrees;
    }

    public (Vector2 Start, Vector2 End) CalculateSquareFlipperPivotEndpoints(
        int sourceEdge, int destinationEdge, float progress)
    {
        if (Configuration.FlipperPivotMode != FlipperPivotGeometry.SquareEndpointHinge)
            throw new InvalidOperationException("Square hinge endpoints require the square Flipper pivot configuration.");
        if (!float.IsFinite(progress))
            throw new ArgumentOutOfRangeException(nameof(progress));

        IReadOnlyList<Vector2> points = Definition.Points;
        if ((uint)sourceEdge >= (uint)points.Count)
            throw new ArgumentOutOfRangeException(nameof(sourceEdge));

        progress = Math.Clamp(progress, 0, 1);
        (int pivotIndex, int movingIndex, int destinationIndex, bool movingStart, float degrees) =
            CalculateSquareFlipperHingeRotation(sourceEdge, destinationEdge);

        Vector2 pivot = points[pivotIndex];
        Vector2 moving = points[movingIndex];
        Vector2 destination = points[destinationIndex];
        if (progress == 0)
            return (points[sourceEdge], points[NextEdge(sourceEdge)]);
        if (progress == 1)
            return movingStart ? (pivot, destination) : (destination, pivot);

        Vector2 initialVector = moving - pivot;
        Vector2 destinationVector = destination - pivot;
        // The reference square's rounded side segments alternate lengths 80 and 81; preserve the
        // endpoint hinge arc while adjusting radius so the free endpoint lands on the exact lane.
        float angle = degrees * progress * MathF.PI / 180;
        float sin = MathF.Sin(angle);
        float cos = MathF.Cos(angle);
        Vector2 rotated = new(
            initialVector.X * cos - initialVector.Y * sin,
            initialVector.X * sin + initialVector.Y * cos);
        float radius = initialVector.Length() +
            (destinationVector.Length() - initialVector.Length()) * progress;
        rotated = Vector2.Normalize(rotated) * radius;
        Vector2 movedEndpoint = pivot + rotated;

        return movingStart
            ? (pivot, movedEndpoint)
            : (movedEndpoint, pivot);
    }

    private (int PivotIndex, int MovingIndex, int DestinationIndex, bool MovingStart, float Degrees)
        CalculateSquareFlipperHingeRotation(int sourceEdge, int destinationEdge)
    {
        IReadOnlyList<Vector2> points = Definition.Points;
        if ((uint)sourceEdge >= (uint)points.Count)
            throw new ArgumentOutOfRangeException(nameof(sourceEdge));

        int previous = PreviousEdge(sourceEdge);
        int next = NextEdge(sourceEdge);
        int pivotIndex;
        int movingIndex;
        int destinationIndex;
        bool movingStart;
        if (destinationEdge == previous)
        {
            pivotIndex = sourceEdge;
            movingIndex = next;
            destinationIndex = previous;
            movingStart = false;
        }
        else if (destinationEdge == next)
        {
            pivotIndex = next;
            movingIndex = sourceEdge;
            destinationIndex = NextEdge(next);
            movingStart = true;
        }
        else
        {
            throw new ArgumentException("Square Flipper pivots must connect adjacent lanes.", nameof(destinationEdge));
        }

        Vector2 pivot = points[pivotIndex];
        // Straight square lanes share a 0-degree heading change; the Flipper instead sweeps
        // between endpoint rays around their shared vertex.
        Vector2 from = points[movingIndex] - pivot;
        Vector2 to = points[destinationIndex] - pivot;
        float cross = from.X * to.Y - from.Y * to.X;
        float dot = Vector2.Dot(from, to);
        float degrees = MathF.Atan2(cross, dot) * 180 / MathF.PI;
        if (cross == 0 && dot < 0)
            degrees = movingStart ? -180 : 180;

        return (pivotIndex, movingIndex, destinationIndex, movingStart, degrees);
    }

    private static float NormalizeAngleDegrees(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
    }
}
