namespace Tempest.Game.Levels;

public sealed class LevelApproach
{
    // Provisional project presentation tuning; Atari observation establishes no numeric curve.
    public const int DurationTicks = 120;
    public const float InitialDepthOffset = 240;

    private int elapsedTicks;
    private bool isComplete;

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
