namespace Tempest.Game.Systems;

public sealed class ScoreSystem
{
    public int Score { get; private set; }
    public void Add(int points) => Score += points;

    public static int CalculateLevelBonus(int board, int color)
    {
        var level = board + color * 16;
        return level * 3000 + level * color * 500 + color * 2000 + (level > 3 ? level * 4100 : 0);
    }
}
