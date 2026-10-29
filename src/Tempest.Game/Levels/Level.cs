using System.Numerics;
using Tempest.Game.Gameplay;

namespace Tempest.Game.Levels;

public sealed class Level
{
    public const float Depth = 400f;
    public const float Position = 50;
    public const int EnemyTotal = 10;
    private const int SpawnIntervalTicks = 50;
    private const int MaximumActiveEggs = 4;
    private const int MaximumApproachingEggs = 2;
    private const int HatchSelectionIntervalTicks = 50;
    private readonly int[] spawnLanes = [12, 4, 9, 1, 14, 6, 10, 2, 15, 7];
    private readonly List<Egg> eggs = [];
    private int spawnTimer;
    private int hatchSelectionTimer;
    private int nextSpawnLaneIndex;

    public LevelDefinition Definition { get; } = LevelLoader.LoadLevel1();
    public int EnemiesSpawned { get; private set; }
    public int EnemiesDestroyed { get; private set; }
    public int ReserveEggs => EnemyTotal - EnemiesSpawned;
    public int RemainingEnemies => EnemyTotal - EnemiesDestroyed;
    public IReadOnlyList<Egg> Eggs => eggs;
    public LevelState State { get; set; } = LevelState.Begin;
    public bool Continuous { get; set; } = true;

    public Level() => Reset();

    public void Reset()
    {
        EnemiesDestroyed = 0;
        ResetAfterDeath();
    }

    public void ResetAfterDeath()
    {
        ReturnSurvivorsToReserve();
        for (int eggIndex = 0; eggIndex < MaximumActiveEggs && ReserveEggs > 0; eggIndex++)
            ReleaseEgg();
    }

    public void ReturnSurvivorsToReserve()
    {
        eggs.Clear();
        spawnTimer = 0;
        hatchSelectionTimer = 0;
        EnemiesSpawned = EnemiesDestroyed;
        nextSpawnLaneIndex = EnemiesDestroyed % spawnLanes.Length;
    }

    public void RecordEnemyDestroyed() => EnemiesDestroyed++;

    public void UpdateEggRelease()
    {
        spawnTimer++;
        if (spawnTimer < SpawnIntervalTicks)
            return;

        spawnTimer = 0;
        if (ReserveEggs > 0 && eggs.Count < MaximumActiveEggs)
            ReleaseEgg();
    }

    private void ReleaseEgg()
    {
        int lane = spawnLanes[nextSpawnLaneIndex % spawnLanes.Length];
        // Alternate directions in release order to keep roaming repeatable.
        int direction = nextSpawnLaneIndex % 2 == 0 ? 1 : -1;
        nextSpawnLaneIndex++;
        eggs.Add(new Egg(lane, direction));
        EnemiesSpawned++;
    }

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

    public Flipper? HatchReadyEgg()
    {
        int eggIndex = eggs.FindIndex(egg => egg.State == EggState.ReadyToHatch);
        if (eggIndex < 0)
            return null;

        Egg egg = eggs[eggIndex];
        eggs.RemoveAt(eggIndex);
        return new Flipper(egg.EdgeIndex, 0);
    }

    public bool IsComplete(int activeFlippers) =>
        EnemiesDestroyed == EnemyTotal && ReserveEggs == 0 && eggs.Count == 0 && activeFlippers == 0;

    public int NextEdge(int edge) => (edge + 1) % Definition.Points.Count;
    public int PreviousEdge(int edge) =>
        (edge - 1 + Definition.Points.Count) % Definition.Points.Count;

    public int FindShortestDirection(int fromEdge, int toEdge)
    {
        int edgeCount = Definition.Points.Count;
        int clockwise = (toEdge - fromEdge + edgeCount) % edgeCount;
        int counterclockwise = (fromEdge - toEdge + edgeCount) % edgeCount;
        return clockwise < counterclockwise ? -1 : 1;
    }

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
