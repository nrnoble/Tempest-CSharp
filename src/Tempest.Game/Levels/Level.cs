using System.Numerics;

namespace Tempest.Game.Levels;

public sealed class Level
{
    public const float Depth = 400f;
    public const float Position = 50;
    public const int EnemyTotal = 10;
    private const int SpawnIntervalTicks = 50;
    private const int MaximumActiveFlippers = 4;
    private readonly int[] spawnLanes = [12, 4, 9, 1, 14, 6, 10, 2, 15, 7];
    private int spawnTimer;
    private int nextSpawnLaneIndex;

    public LevelDefinition Definition { get; } = LevelLoader.LoadLevel1();
    public int EnemiesSpawned { get; private set; }
    public int EnemiesDestroyed { get; private set; }
    public LevelState State { get; set; } = LevelState.Begin;
    public bool Continuous { get; set; } = true;

    public void Reset()
    {
        EnemiesSpawned = 0;
        EnemiesDestroyed = 0;
        spawnTimer = 0;
        nextSpawnLaneIndex = 0;
    }

    public void ResetAfterDeath()
    {
        spawnTimer = 0;
        EnemiesSpawned = EnemiesDestroyed;
        nextSpawnLaneIndex = EnemiesDestroyed % spawnLanes.Length;
    }

    public void RecordEnemyDestroyed() => EnemiesDestroyed++;

    public int? UpdateSpawning(int activeFlippers)
    {
        if (EnemiesSpawned >= EnemyTotal || activeFlippers >= MaximumActiveFlippers)
            return null;

        spawnTimer++;
        if (spawnTimer < SpawnIntervalTicks)
            return null;

        int lane = spawnLanes[nextSpawnLaneIndex % spawnLanes.Length];
        nextSpawnLaneIndex++;
        EnemiesSpawned++;
        spawnTimer = 0;
        return lane;
    }

    public bool IsComplete(int activeFlippers) =>
        EnemiesDestroyed == EnemyTotal && activeFlippers == 0;

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
