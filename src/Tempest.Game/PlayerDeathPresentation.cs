namespace Tempest.Game;

public sealed class PlayerDeathPresentation
{
    private bool started;

    internal PlayerDeathPresentation(int durationTicks)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(durationTicks);
        DurationTicks = durationTicks;
    }

    public int ElapsedTicks { get; private set; }
    public int DurationTicks { get; }
    public float EnemyRetractionProgress => started
        ? ElapsedTicks / (float)DurationTicks
        : 0;
    public bool IsComplete => started && ElapsedTicks == DurationTicks;

    internal void Start()
    {
        ElapsedTicks = 0;
        started = true;
    }

    internal void Advance()
    {
        if (started && ElapsedTicks < DurationTicks)
            ElapsedTicks++;
    }

    internal void Reset()
    {
        ElapsedTicks = 0;
        started = false;
    }
}
