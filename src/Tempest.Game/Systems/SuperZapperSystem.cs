namespace Tempest.Game.Systems;

using Tempest.Game.Gameplay;

public sealed class SuperZapperSystem
{
    public const int InitialCharges = 2;
    public const int FlashDurationTicks = 60;

    private int displayPhase;

    public int ChargesRemaining { get; private set; }
    public int FlashTicksRemaining { get; private set; }
    // Atari $b3d6-$b3e9: active rails cycle 0..6,1; inactive rails use slot 6.
    public int TubeColorIndex => FlashTicksRemaining > 0
        ? displayPhase == 7 ? 1 : displayPhase
        : 6;

    public SuperZapperSystem() => Reset();

    public void Reset()
    {
        ChargesRemaining = InitialCharges;
        FlashTicksRemaining = 0;
    }

    public void AdvanceDisplayStep()
    {
        displayPhase = (displayPhase + 1) & 7;
        if (FlashTicksRemaining > 0)
            FlashTicksRemaining--;
    }

    public bool TryActivate(
        IEnumerable<Enemy> sourceOrder, int playerEdge, int edgeCount, bool continuous)
    {
        ArgumentNullException.ThrowIfNull(sourceOrder);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(edgeCount);
        if (ChargesRemaining == 0)
            return false;

        int strength = ChargesRemaining;
        if (strength == 2)
        {
            int delay = 4;
            // Rejected bullets are visited but do not consume an accepted-target delay slot.
            foreach (Enemy enemy in sourceOrder)
                if (enemy.TryZap(delay))
                    delay += 2;

            FlashTicksRemaining = FlashDurationTicks;
        }
        else
        {
            Enemy? selected = SelectTarget(sourceOrder, playerEdge, edgeCount, continuous);
            if (selected is not null && selected.TryZap(4))
                FlashTicksRemaining = FlashDurationTicks;
        }

        ChargesRemaining--;
        return true;
    }

    public static Enemy? SelectTarget(
        IEnumerable<Enemy> sourceOrder, int playerEdge, int edgeCount, bool continuous)
    {
        ArgumentNullException.ThrowIfNull(sourceOrder);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(edgeCount);

        Enemy? best = null;
        int bestDistance = 16;
        float bestHeight = 0;
        int bestType = 0;

        foreach (Enemy enemy in sourceOrder)
        {
            int sourceType = GetSourceType(enemy);
            if (sourceType <= 1)
                continue;

            int distance = CalculateLaneDistance(
                enemy.EdgeIndex, playerEdge, edgeCount, continuous);

            // These are the literal source comparisons; the type branch also admits a
            // non-taller candidate one lane farther inside the strict distance gate.
            if (distance < bestDistance + 2)
            {
                if (enemy.Height > bestHeight)
                {
                    best = enemy;
                    bestDistance = distance;
                    bestHeight = enemy.Height;
                    bestType = sourceType;
                }
                else if (distance < bestDistance)
                {
                    best = enemy;
                    bestDistance = distance;
                    bestHeight = enemy.Height;
                    bestType = sourceType;
                }
                else if (sourceType > bestType)
                {
                    best = enemy;
                    bestDistance = distance;
                    bestHeight = enemy.Height;
                    bestType = sourceType;
                }
            }
        }

        return best;
    }

    public static int CalculateLaneDistance(
        int enemyEdge, int playerEdge, int edgeCount, bool continuous)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(edgeCount);
        int distance = Math.Abs(enemyEdge - playerEdge);
        if (continuous && distance > edgeCount / 2)
            distance = edgeCount - distance;
        return distance;
    }

    private static int GetSourceType(Enemy enemy) => enemy switch
    {
        Spiker => 2,
        Flipper => 3,
        Tanker => 4,
        Pulsar => 5,
        Fuseball => 6,
        _ => 0
    };
}
