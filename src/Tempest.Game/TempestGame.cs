using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Tempest.Game.Rendering;

namespace Tempest.Game;

public sealed class TempestGame
{
    public const double SimulationStepSeconds = 1.0 / 60.0;
    public const int PlayerDeathTicks = 60;
    private const float PlayerSpeed = 0.05f;
    private const float SpinnerSensitivity = 0.0125f;
    private const int MaximumShots = 10;
    private readonly List<Shot> shots = [];
    private readonly List<Flipper> flippers = [];
    private int shotTimer = 3;
    private int exitDelayTicks;

    public Player Player { get; } = new();
    public Level CurrentLevel { get; } = new();
    public ScoreSystem Score { get; } = new();
    public IReadOnlyList<Shot> Shots => shots;
    public IReadOnlyList<Flipper> Flippers => flippers;
    public IReadOnlyList<Egg> Eggs => CurrentLevel.Eggs;
    public int DeathTicks { get; private set; }
    public LevelState State => CurrentLevel.State;
    public bool GameOver => State == LevelState.GameOver;
    public bool LevelComplete => State == LevelState.Complete;
    public bool IsExiting => State is LevelState.ExitDelay or LevelState.ExitZoom;

    public TempestGame() => ResetLevel();

    public void RunOneStep(PlayerInput input = default, bool restartPressed = false)
    {
        if (GameOver)
        {
            if (restartPressed)
                ResetGame();
        }
        else if (LevelComplete)
        {
            if (restartPressed)
                ResetLevel();
        }
        else if (State == LevelState.ExitDelay)
        {
            exitDelayTicks++;
            if (exitDelayTicks > 20)
                CurrentLevel.State = LevelState.ExitZoom;
        }
        else if (State == LevelState.ExitZoom)
        {
            UpdateExitZoom();
        }
        else if (State == LevelState.PlayerDying)
        {
            UpdatePlayerDeath();
        }
        else
        {
            UpdatePlayer(input);
            UpdatePlayerShots();
            if (CheckForLevelComplete())
                return;

            CurrentLevel.UpdateEggRelease();
            CurrentLevel.UpdateEggs();
            HatchEggs();
            UpdateFlippers();
            CheckFlippersAgainstPlayer();
            CheckForLevelComplete();
        }
    }

    private void ResetGame()
    {
        Score.Reset();
        Player.Lives = 3;
        ResetLevel();
    }

    private void ResetLevel()
    {
        CurrentLevel.State = LevelState.Playing;
        exitDelayTicks = 0;
        RestoreProjection();
        DeathTicks = 0;
        CurrentLevel.Reset();
        ResetPlayerPosition();
        shots.Clear();
        flippers.Clear();
    }

    private void ResetAfterDeath()
    {
        ResetPlayerPosition();
        shots.Clear();
        flippers.Clear();
        CurrentLevel.ResetAfterDeath();
        CurrentLevel.State = LevelState.Playing;
    }

    private void ResetPlayerPosition()
    {
        Player.ResetPosition();
        shotTimer = 3;
    }

    private void UpdatePlayerDeath()
    {
        DeathTicks--;
        if (DeathTicks > 0)
            return;

        Player.Lives--;
        if (Player.Lives <= 0)
        {
            CurrentLevel.State = LevelState.GameOver;
            shots.Clear();
            flippers.Clear();
            CurrentLevel.ReturnSurvivorsToReserve();
            return;
        }

        ResetAfterDeath();
    }

    private void KillPlayer()
    {
        if (State != LevelState.Playing)
            return;

        DeathTicks = PlayerDeathTicks;
        CurrentLevel.State = LevelState.PlayerDying;
        shots.Clear();
    }

    private void UpdatePlayer(PlayerInput input)
    {
        UpdatePlayerFiring(input.Fire);

        float movement = input.MouseDeltaX * SpinnerSensitivity;
        if (input.Left)
            movement -= PlayerSpeed;
        if (input.Right)
            movement += PlayerSpeed;

        Player.Move(movement, CurrentLevel.Definition.Points.Count);
    }

    private void UpdatePlayerFiring(bool fireButtonDown)
    {
        if (fireButtonDown && shotTimer > 2 && shots.Count < MaximumShots)
        {
            shots.Add(new Shot { EdgeIndex = Player.EdgeIndex, Height = Level.Depth });
            shotTimer = 0;
        }

        shotTimer++;
    }

    private void UpdatePlayerShots()
    {
        for (int shotIndex = shots.Count - 1; shotIndex >= 0; shotIndex--)
        {
            Shot shot = shots[shotIndex];
            shot.Update();
            if (CheckShotAgainstFlippers(shot) || shot.Height < 0)
                shots.RemoveAt(shotIndex);
        }
    }

    private bool CheckShotAgainstFlippers(Shot shot)
    {
        for (int flipperIndex = flippers.Count - 1; flipperIndex >= 0; flipperIndex--)
        {
            if (!flippers[flipperIndex].IsHitBy(shot))
                continue;

            flippers.RemoveAt(flipperIndex);
            CurrentLevel.RecordEnemyDestroyed();
            Score.Add(150);
            return true;
        }

        return false;
    }

    private void HatchEggs()
    {
        while (CurrentLevel.HatchReadyEgg() is Flipper flipper)
            flippers.Add(flipper);
    }

    private void UpdateFlippers()
    {
        foreach (Flipper flipper in flippers)
            flipper.Update(CurrentLevel, Player.EdgeIndex);
    }

    private void CheckFlippersAgainstPlayer()
    {
        foreach (Flipper flipper in flippers)
        {
            if (!flipper.CompletedIntoPlayer)
                continue;

            flipper.ClearCompletedCollision();
            KillPlayer();
            return;
        }
    }

    private bool CheckForLevelComplete()
    {
        if (State != LevelState.Playing || !CurrentLevel.IsComplete(flippers.Count))
            return false;

        CurrentLevel.State = LevelState.ExitDelay;
        exitDelayTicks = 0;
        shots.Clear();
        return true;
    }

    private void UpdateExitZoom()
    {
        if (Projection.ZOffset >= -40)
        {
            Projection.ZOffset -= 2;
            Player.AdvanceExit(3);
        }
        else
        {
            Projection.K += 2;
            if (Projection.K > 100)
                Projection.K += 2;
            Player.AdvanceExit(3.2f);
        }

        if (Player.Depth < 10)
        {
            CurrentLevel.State = LevelState.Complete;
            RestoreProjection();
        }
    }

    private static void RestoreProjection()
    {
        Projection.K = 70;
        Projection.ZOffset = 5;
    }
}
