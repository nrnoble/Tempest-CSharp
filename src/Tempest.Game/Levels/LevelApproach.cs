namespace Tempest.Game.Levels;

public sealed class LevelApproach
{
    // At the normal 60 FPS cadence, 59 remaining exit updates + 181 approach updates take 4 seconds.
    // Exact distance and smoothstep remain project tuning, not measured Atari geometry.
    public const int DurationTicks = 181;
    public const float InitialDepthOffset = 960;

    private int elapsedTicks;
    private bool isComplete;

    public LevelApproach(int exitedLevel)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exitedLevel);
        ExitedLevel = exitedLevel;
    }

    public int ExitedLevel { get; }
    public int ElapsedTicks => elapsedTicks;
    public float Progress => elapsedTicks / (float)DurationTicks;
    public float DepthOffset
    {
        get
        {
            if (isComplete)
                return 0;

            float progress = Progress;
            float easedProgress = progress * progress * (3 - 2 * progress);
            return InitialDepthOffset * (1 - easedProgress);
        }
    }

    public bool IsComplete => isComplete;

    public bool Advance()
    {
        if (isComplete)
            return false;

        elapsedTicks++;
        if (elapsedTicks < DurationTicks)
            return false;

        elapsedTicks = DurationTicks;
        isComplete = true;
        return true;
    }
}
