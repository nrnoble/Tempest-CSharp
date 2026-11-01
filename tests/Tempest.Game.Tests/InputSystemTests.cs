using Tempest.Game;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class InputSystemTests
{
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    public void RestartAcceptsEitherPressEdgeAsOneRequest(bool r, bool p, bool expected)
    {
        Assert.Equal(expected, InputSystem.IsRestartPressed(r, p));
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    public void QuitPreservesWindowCloseAndAddsVPress(bool close, bool v, bool expected)
    {
        Assert.Equal(expected, InputSystem.IsQuitRequested(close, v));
    }

    [Theory]
    [InlineData(LevelState.GameOver)]
    [InlineData(LevelState.Complete)]
    public void PAliasAndRUseIdenticalExistingRestartOrReplay(LevelState state)
    {
        TempestGame fromR = new(level3GeometryTest: true, startingLives: 5);
        TempestGame fromP = new(level3GeometryTest: true, startingLives: 5);
        foreach (TempestGame game in new[] { fromR, fromP })
        {
            game.Score.Add(125);
            game.Player.Lives = 2;
            game.CurrentLevel.State = state;
        }
        Level previousR = fromR.CurrentLevel;
        Level previousP = fromP.CurrentLevel;

        fromR.RunOneStep(restartPressed: InputSystem.IsRestartPressed(true, false));
        fromP.RunOneStep(restartPressed: InputSystem.IsRestartPressed(false, true));

        Assert.NotSame(previousR, fromR.CurrentLevel);
        Assert.NotSame(previousP, fromP.CurrentLevel);
        Assert.Same(fromR.CurrentLevel.Configuration, fromP.CurrentLevel.Configuration);
        Assert.Equal(fromR.State, fromP.State);
        Assert.Equal(fromR.Score.Score, fromP.Score.Score);
        Assert.Equal(fromR.Player.Lives, fromP.Player.Lives);
        Assert.Equal(fromR.SuperZapperChargesRemaining, fromP.SuperZapperChargesRemaining);

        // Subsequent held frames have no new press edge at the Raylib boundary.
        Level restarted = fromP.CurrentLevel;
        fromP.CurrentLevel.State = state;
        for (int frame = 0; frame < 5; frame++)
            fromP.RunOneStep(restartPressed: InputSystem.IsRestartPressed(false, false));
        Assert.Same(restarted, fromP.CurrentLevel);
        Assert.Equal(state, fromP.State);
    }

    [Theory]
    [InlineData(LevelState.Playing)]
    [InlineData(LevelState.PlayerDying)]
    [InlineData(LevelState.ExitDelay)]
    [InlineData(LevelState.ExitZoom)]
    [InlineData(LevelState.ReverseZoom)]
    [InlineData(LevelState.AvoidSpikes)]
    public void PAliasDoesNotEnableRestartOutsideExistingEndStates(LevelState state)
    {
        TempestGame game = new(startingLives: 5);
        Level level = game.CurrentLevel;
        game.Score.Add(125);
        game.CurrentLevel.State = state;

        game.RunOneStep(restartPressed: InputSystem.IsRestartPressed(false, true));

        Assert.Same(level, game.CurrentLevel);
        Assert.Equal(125, game.Score.Score);
    }
}
