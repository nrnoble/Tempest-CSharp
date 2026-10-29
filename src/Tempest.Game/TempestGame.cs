using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;

namespace Tempest.Game;

public sealed class TempestGame
{
    public const double SimulationStepSeconds = 1.0 / 60.0;

    public Player Player { get; } = new();
    public Level CurrentLevel { get; } = new();
    public ScoreSystem Score { get; } = new();

    public void RunOneStep()
    {
        CurrentLevel.Update();
        Player.Update();
    }
}
