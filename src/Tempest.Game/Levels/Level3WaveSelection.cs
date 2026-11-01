namespace Tempest.Game.Levels;

public static class Level3WaveSelection
{
    // These are source-shaped Level 3 decisions, not arcade-verified probabilities.
    public static Level3OriginalType SelectOriginalType(
        Func<int, int, int> randomInclusive,
        int activeTankers,
        int activeEnemyUnits,
        int activeEggs)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(activeTankers);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEnemyUnits);
        ArgumentOutOfRangeException.ThrowIfNegative(activeEggs);

        int upper = activeTankers < 6 ? 4 : 3;
        bool nearEnd = activeEnemyUnits + activeEggs < 4;

        while (true)
        {
            int candidate = Math.Max(0, randomInclusive(-6, upper));
            if (nearEnd)
                candidate = randomInclusive(1, upper);

            if (candidate == 1)
                return Level3OriginalType.Flipper;
            if (candidate == 4)
                return Level3OriginalType.Tanker;
        }
    }

    public static void SelectTwoFlipperCargo(
        Func<int, int, int> randomInclusive)
    {
        ArgumentNullException.ThrowIfNull(randomInclusive);
        while (randomInclusive(0, 2) != 0)
        {
        }
    }
}
