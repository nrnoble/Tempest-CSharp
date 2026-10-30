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
    private readonly List<Spiker> spikers = [];
    private readonly List<object> developmentCollisionOrder = [];
    private readonly Random random;
    private int shotTimer = 3;
    private int exitDelayTicks;

    public Player Player { get; } = new();
    public Level CurrentLevel { get; }
    public ScoreSystem Score { get; } = new();
    public IReadOnlyList<Shot> Shots => shots;
    public IReadOnlyList<Flipper> Flippers => flippers;
    public IReadOnlyList<Egg> Eggs => CurrentLevel.Eggs;
    public IReadOnlyList<Spiker> Spikers => spikers;
    public IReadOnlyList<Spike> Spikes => CurrentLevel.Spikes;
    public bool SpikeExitControlsEnabled => CurrentLevel.SpikerDevelopmentWave &&
        State is LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom;
    public int DeathTicks { get; private set; }
    public LevelState State => CurrentLevel.State;
    public bool GameOver => State == LevelState.GameOver;
    public bool LevelComplete => State == LevelState.Complete;
    public bool IsExiting => State is LevelState.ExitDelay or LevelState.ExitZoom or
        LevelState.AvoidSpikes or LevelState.ReverseZoom;

    public TempestGame(bool spikerDevelopmentWave = false, Random? random = null)
    {
        this.random = random ?? Random.Shared;
        CurrentLevel = new Level(spikerDevelopmentWave);
        CurrentLevel.SpikeCreated += spike => developmentCollisionOrder.Add(spike);
        ResetLevel();
    }

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
        else if (State == LevelState.ReverseZoom)
        {
            UpdateReverseZoom();
        }
        else if (SpikeExitControlsEnabled)
        {
            UpdateSpikeExit(input);
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
            UpdateSpikers();
            CheckFlippersAgainstPlayer();
            CheckSpikesAgainstPlayer();
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
        developmentCollisionOrder.Clear();
        CurrentLevel.Reset();
        ResetPlayerPosition();
        shots.Clear();
        flippers.Clear();
        spikers.Clear();
    }

    private void ResetAfterDeath()
    {
        ResetPlayerPosition();
        shots.Clear();
        flippers.Clear();
        spikers.Clear();
        developmentCollisionOrder.RemoveAll(target => target is not Spike);
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
            spikers.Clear();
            developmentCollisionOrder.RemoveAll(target => target is not Spike);
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
            shots.Add(new Shot { EdgeIndex = Player.EdgeIndex, Height = Player.Depth });
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
            bool hit = CurrentLevel.SpikerDevelopmentWave
                ? shot.Height >= 0 && CheckShotAgainstDevelopmentTargets(shot)
                : CheckShotAgainstFlippers(shot);
            if (hit || shot.Height < 0)
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
        while (CurrentLevel.HatchReadyEnemy(CreateHatchedEnemy) is Enemy enemy)
        {
            if (enemy is Flipper flipper)
                flippers.Add(flipper);
            else if (enemy is Spiker spiker)
                spikers.Add(spiker);
            if (CurrentLevel.SpikerDevelopmentWave)
                developmentCollisionOrder.Add(enemy);
        }
    }

    private Enemy CreateHatchedEnemy(int lane)
    {
        if (CurrentLevel.SpikerDevelopmentWave && spikers.Count <= 3 &&
            flippers.Count + spikers.Count + Eggs.Count >= 4 && random.Next(-6, 2) <= 0)
        {
            // Fixed development speed avoids guessing BlitzMax's fractional Rand coercion.
            return new Spiker(lane, 3);
        }
        return new Flipper(lane, 0);
    }

    private bool CheckShotAgainstDevelopmentTargets(Shot shot)
    {
        // Incitti tests insertion order, not nearest depth. Level 1 keeps its old path.
        foreach (object target in developmentCollisionOrder)
        {
            switch (target)
            {
                case Spike spike when spike.IsHitBy(shot):
                    spike.Shorten();
                    Score.Add(1);
                    if (spike.IsDepleted)
                    {
                        CurrentLevel.RemoveSpike(spike);
                        developmentCollisionOrder.Remove(spike);
                    }
                    return true;
                case Spiker spiker when spiker.IsHitBy(shot):
                    spikers.Remove(spiker);
                    developmentCollisionOrder.Remove(spiker);
                    CurrentLevel.RecordEnemyDestroyed();
                    Score.Add(50);
                    return true;
                case Flipper flipper when flipper.IsHitBy(shot):
                    flippers.Remove(flipper);
                    developmentCollisionOrder.Remove(flipper);
                    CurrentLevel.RecordEnemyDestroyed();
                    Score.Add(150);
                    return true;
            }
        }
        return false;
    }

    private void UpdateSpikers()
    {
        for (int index = 0; index < spikers.Count; index++)
        {
            Spiker spiker = spikers[index];
            spiker.Update(CurrentLevel, () => random.NextDouble() * 120 > 88);
            if (spiker.State != SpikerState.ReturnedToReserve)
                continue;

            spikers.RemoveAt(index--);
            developmentCollisionOrder.Remove(spiker);
            CurrentLevel.ReturnEnemyToReserve();
        }
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
        if (State != LevelState.Playing || !CurrentLevel.IsComplete(flippers.Count + spikers.Count))
            return false;

        CurrentLevel.State = CurrentLevel.SpikerDevelopmentWave ? LevelState.AvoidSpikes : LevelState.ExitDelay;
        exitDelayTicks = 0;
        shots.Clear();
        return true;
    }

    private void UpdateExitZoom(bool completeWhenFinished = true)
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

        if (completeWhenFinished && Player.Depth < 10)
        {
            CurrentLevel.State = LevelState.Complete;
            RestoreProjection();
        }
    }

    private void UpdateSpikeExit(PlayerInput input)
    {
        if (State == LevelState.ExitZoom)
            UpdateExitZoom(completeWhenFinished: false);

        // Spike travel keeps source ordering locally: depth, shots, hazard, then input.
        UpdatePlayerShots();
        CheckSpikesAgainstPlayer();
        if (State == LevelState.ReverseZoom)
            return;

        if (State == LevelState.ExitZoom && Player.Depth < 10)
        {
            CurrentLevel.State = LevelState.Complete;
            shots.Clear();
            RestoreProjection();
            return;
        }

        UpdatePlayer(input);
        if (State == LevelState.AvoidSpikes)
        {
            RestoreProjection();
            Player.ResetDepth();
            if (++exitDelayTicks > 150)
            {
                CurrentLevel.State = LevelState.ExitDelay;
                exitDelayTicks = 0;
            }
        }
        else if (State == LevelState.ExitDelay && ++exitDelayTicks > 20)
        {
            CurrentLevel.State = LevelState.ExitZoom;
            exitDelayTicks = 0;
        }
    }

    private void CheckSpikesAgainstPlayer()
    {
        if (!CurrentLevel.SpikerDevelopmentWave || DeathTicks > 0 ||
            State is not (LevelState.Playing or LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom))
            return;
        if (!Spikes.Any(spike => spike.HitsPlayer(Player)))
            return;

        DeathTicks = PlayerDeathTicks;
        CurrentLevel.State = LevelState.ReverseZoom;
        exitDelayTicks = 0;
        shots.Clear();
        flippers.Clear();
        spikers.Clear();
        developmentCollisionOrder.RemoveAll(target => target is not Spike);
        CurrentLevel.ReturnSurvivorsToReserve();
    }

    private void UpdateReverseZoom()
    {
        Projection.K = MathF.Max(70, Projection.K - 4);
        Projection.ZOffset += 1;
        if (Projection.ZOffset > 5)
        {
            Projection.ZOffset = 5;
            Player.ResetDepth();
            exitDelayTicks++;
        }

        if (DeathTicks > 0 && --DeathTicks == 0)
        {
            Player.Lives--;
            if (Player.Lives <= 0)
            {
                CurrentLevel.State = LevelState.GameOver;
                RestoreProjection();
                return;
            }
        }

        // Never let a short reverse zoom cancel the established 60-tick life loss.
        if (exitDelayTicks > 20 && DeathTicks == 0)
        {
            exitDelayTicks = 0;
            RestoreProjection();
            if (CurrentLevel.IsComplete(0))
                CurrentLevel.State = LevelState.AvoidSpikes;
            else
                ResetAfterDeath();
        }
    }

    private static void RestoreProjection()
    {
        Projection.K = 70;
        Projection.ZOffset = 5;
    }
}
