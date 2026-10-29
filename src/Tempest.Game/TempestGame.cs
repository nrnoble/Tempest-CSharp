using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;

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

    public Player Player { get; } = new();
    public Level CurrentLevel { get; } = new();
    public ScoreSystem Score { get; } = new();
    public IReadOnlyList<Shot> Shots => shots;
    public IReadOnlyList<Flipper> Flippers => flippers;
    public IReadOnlyList<Egg> Eggs => CurrentLevel.Eggs;
    public int DeathTicks { get; private set; }
    public bool GameOver { get; private set; }
    public bool LevelComplete { get; private set; }

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
        else if (DeathTicks > 0)
        {
            UpdatePlayerDeath();
        }
        else
        {
            UpdatePlayer(input);
            UpdatePlayerShots();
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
        GameOver = false;
        ResetLevel();
    }

    private void ResetLevel()
    {
        LevelComplete = false;
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
            GameOver = true;
            shots.Clear();
            flippers.Clear();
            CurrentLevel.ReturnSurvivorsToReserve();
            return;
        }

        ResetAfterDeath();
    }

    private void KillPlayer()
    {
        if (DeathTicks > 0 || GameOver)
            return;

        DeathTicks = PlayerDeathTicks;
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

    private void CheckForLevelComplete()
    {
        if (CurrentLevel.IsComplete(flippers.Count))
            LevelComplete = true;
    }
}
