namespace Tempest.Game;

public sealed class GameOverVectorBurstSequence
{
    public const int BurstCount = 4;
    public const int SpawnWindowTicks = GameOverVectorAnimation.InitialSpawnCounter * 2;
    public const int BurstStartIntervalTicks = SpawnWindowTicks / 2;

    private readonly GameOverVectorAnimation[] bursts;
    private int nextBurstIndex;

    public IReadOnlyList<GameOverVectorAnimation> Bursts => bursts;
    public int ElapsedTicks { get; private set; }
    public int StartedBurstCount => bursts.Count(burst => burst.IsStarted);
    public int ActiveRecordCount => bursts.Sum(burst => burst.ActiveRecordCount);
    public int CreatedRecordCount => bursts.Sum(burst => burst.CreatedRecordCount);
    public bool IsStarted { get; private set; }
    public bool IsComplete { get; private set; }

    public GameOverVectorBurstSequence(Func<int, int, int> nextRandomInclusive)
    {
        ArgumentNullException.ThrowIfNull(nextRandomInclusive);
        bursts = Enumerable.Range(0, BurstCount)
            .Select(index => new GameOverVectorAnimation(nextRandomInclusive, index))
            .ToArray();
    }

    public void Start()
    {
        foreach (GameOverVectorAnimation burst in bursts)
            burst.Reset();

        bursts[0].Start();
        nextBurstIndex = 1;
        ElapsedTicks = 0;
        IsStarted = true;
        IsComplete = false;
    }

    public void Reset()
    {
        foreach (GameOverVectorAnimation burst in bursts)
            burst.Reset();

        nextBurstIndex = 0;
        ElapsedTicks = 0;
        IsStarted = false;
        IsComplete = false;
    }

    public void Advance()
    {
        if (!IsStarted)
            throw new InvalidOperationException("The Game Over burst sequence has not started.");
        if (IsComplete)
            return;

        while (nextBurstIndex < BurstCount &&
            ElapsedTicks >= nextBurstIndex * BurstStartIntervalTicks)
        {
            bursts[nextBurstIndex].Start();
            nextBurstIndex++;
        }

        foreach (GameOverVectorAnimation burst in bursts)
        {
            if (burst.IsStarted)
                burst.Advance();
        }

        ElapsedTicks++;
        IsComplete = nextBurstIndex == BurstCount && bursts.All(burst => burst.IsComplete);
    }
}
