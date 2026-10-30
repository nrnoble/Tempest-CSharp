using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Tempest.Game.Rendering;

namespace Tempest.Game;

/// <summary>
/// Owns the fixed 60 Hz gameplay simulation: player, shots, enemies, collision order,
/// death/reset handling and wave accounting. It has no rendering dependencies; the
/// renderer only reads the public state exposed here.
/// </summary>
/// <remarks>
/// Three opt-in development scenarios (Spiker, enemy bullets, Tanker) layer on top of the
/// normal Level 1 path. Normal Level 1 keeps its original accounting and shot path;
/// do not generalize it casually, because the existing regression tests pin it.
/// </remarks>
public sealed class TempestGame
{
    public const double SimulationStepSeconds = 1.0 / 60.0;
    // Length of the death sequence in simulation updates; a life is deducted on the last one.
    public const int PlayerDeathTicks = 60;
    private const float PlayerSpeed = 0.05f;
    private const float SpinnerSensitivity = 0.0125f;
    private const int MaximumShots = 10;
    private readonly List<Shot> shots = [];
    private readonly List<EnemyBullet> enemyBullets = [];
    private readonly List<Flipper> flippers = [];
    // Cargo released by a player shot waits here so the same shot and the same update cannot affect it.
    private readonly List<Flipper> shotReleasedCargoPendingActivation = [];
    private readonly List<Spiker> spikers = [];
    private readonly List<Tanker> tankers = [];
    // Incitti resolves shot collisions in object-creation order, not nearest depth, so development
    // scenarios record spawn order here. Spikes survive life loss; enemies and bullets do not.
    private readonly List<object> developmentCollisionOrder = [];
    // Tanker scenario ledger. Original wave slots (0..EnemyTotal-1) and Tanker cargo units are
    // tracked separately: cargo is added on top of the normal ten-unit wave and has its own IDs.
    // An unresolved ID survives a life loss; the runtime enemy object does not.
    private readonly HashSet<int> unresolvedOriginalSlots = [];
    private readonly HashSet<int> destroyedOriginalSlots = [];
    private readonly HashSet<int> escapedOriginalSlots = [];
    private readonly HashSet<int> unresolvedCargoUnits = [];
    private readonly HashSet<int> resolvedCargoUnits = [];
    private readonly Random random;
    private readonly int startingLives;
    private readonly bool enemyBulletsTest;
    private readonly bool tankerTest;
    private int nextCargoUnitId;
    private int shotTimer = 3;
    private int exitDelayTicks;

    public Player Player { get; }
    public Level CurrentLevel { get; }
    public ScoreSystem Score { get; } = new();
    public IReadOnlyList<Shot> Shots => shots;
    public IReadOnlyList<EnemyBullet> EnemyBullets => enemyBullets;
    public IReadOnlyList<Flipper> Flippers => flippers;
    public IReadOnlyList<Egg> Eggs => CurrentLevel.Eggs;
    public IReadOnlyList<Spiker> Spikers => spikers;
    public IReadOnlyList<Tanker> Tankers => tankers;
    public IReadOnlyList<Spike> Spikes => CurrentLevel.Spikes;
    public bool SpikeExitControlsEnabled => CurrentLevel.SpikerDevelopmentWave &&
        State is LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom;
    public bool EnemyBulletsTest => enemyBulletsTest;
    public bool TankerTest => tankerTest;
    public int TankerOriginalSlotsRemaining => unresolvedOriginalSlots.Count;
    public int TankerOriginalSlotsDestroyed => destroyedOriginalSlots.Count;
    public int TankerOriginalSlotsEscaped => escapedOriginalSlots.Count;
    public int TankerOriginalSlotsResolved => destroyedOriginalSlots.Count + escapedOriginalSlots.Count;
    public int TankerLatentCargoUnits => tankers.Sum(tanker => tanker.LatentCargoCount) +
        (tankerTest && unresolvedOriginalSlots.Contains(0) && tankers.All(tanker => tanker.OriginalSlotId != 0)
            ? Tanker.CarriedFlipperCount
            : 0);
    public int TankerCargoUnitsRemaining => unresolvedCargoUnits.Count + TankerLatentCargoUnits;
    public int TankerMaterializedCargoUnits => unresolvedCargoUnits.Count + resolvedCargoUnits.Count;
    public int TankerCargoUnitsResolved => resolvedCargoUnits.Count;
    public int ActiveTankerCargoFlippers => flippers.Count(flipper => flipper.IsTankerCargo);
    public int PendingTankerCargoUnits => Math.Max(0, unresolvedCargoUnits.Count - ActiveTankerCargoFlippers);
    public int ActiveTankerOriginalEnemies =>
        tankers.Count + flippers.Count(flipper => !flipper.IsTankerCargo);
    public int TankerAccountingTotal => destroyedOriginalSlots.Count + escapedOriginalSlots.Count +
        unresolvedOriginalSlots.Count + unresolvedCargoUnits.Count + resolvedCargoUnits.Count;
    public int TankerActiveTotal => Eggs.Count + tankers.Count + flippers.Count + spikers.Count;
    public int DeathTicks { get; private set; }
    public LevelState State => CurrentLevel.State;
    public bool GameOver => State == LevelState.GameOver;
    public bool LevelComplete => State == LevelState.Complete;
    public bool IsExiting => State is LevelState.ExitDelay or LevelState.ExitZoom or
        LevelState.AvoidSpikes or LevelState.ReverseZoom;

    public TempestGame(bool spikerDevelopmentWave = false, Random? random = null,
        int startingLives = Player.DefaultStartingLives, bool enemyBulletsTest = false,
        bool tankerTest = false)
    {
        if (spikerDevelopmentWave && tankerTest)
            throw new ArgumentException("Tanker and Spiker development waves cannot be combined.");
        this.random = random ?? Random.Shared;
        this.startingLives = startingLives > 0 ? startingLives : Player.DefaultStartingLives;
        this.enemyBulletsTest = enemyBulletsTest;
        this.tankerTest = tankerTest;
        Player = new Player(this.startingLives);
        CurrentLevel = new Level(spikerDevelopmentWave, tankerTest);
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
            // Order matters. Player firing and shot-versus-enemy collision run before enemy updates
            // and before any enemy-to-player collision, so a shot that destroys a Flipper on the
            // last safe tick saves the player. Do not move the player-collision checks earlier.
            UpdatePlayer(input);
            UpdatePlayerShots();
            UpdateEnemyBullets();
            if (CheckForLevelComplete())
                return;

            CurrentLevel.UpdateEggRelease();
            CurrentLevel.UpdateEggs();
            HatchEggs();
            UpdateFlippers();
            UpdateTankers();
            // Runs after UpdateFlippers/UpdateTankers so shot-released cargo skips this tick's update.
            ActivateShotReleasedCargo();
            UpdateSpikers();
            CheckFlippersAgainstPlayer();
            CheckEnemyBulletsAgainstPlayer();
            CheckSpikesAgainstPlayer();
            CheckForLevelComplete();
        }
    }

    private void ResetGame()
    {
        Score.Reset();
        Player.Lives = startingLives;
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
        enemyBullets.Clear();
        flippers.Clear();
        shotReleasedCargoPendingActivation.Clear();
        spikers.Clear();
        tankers.Clear();
        ResetTankerLedger();
    }

    private void ResetTankerLedger()
    {
        unresolvedOriginalSlots.Clear();
        destroyedOriginalSlots.Clear();
        escapedOriginalSlots.Clear();
        unresolvedCargoUnits.Clear();
        resolvedCargoUnits.Clear();
        nextCargoUnitId = 0;
        if (tankerTest)
            for (int slotId = 0; slotId < Level.EnemyTotal; slotId++)
                unresolvedOriginalSlots.Add(slotId);
    }

    private void ResetTankerScenarioAfterDeath()
    {
        // Gather surviving cargo identities before the runtime objects are discarded below.
        HashSet<int> cargoUnitsToReenter = CurrentLevel.PendingTankerCargoUnitIds.ToHashSet();
        foreach (Egg cargoEgg in Eggs.Where(egg => egg.CargoUnitId >= 0))
            cargoUnitsToReenter.Add(cargoEgg.CargoUnitId);
        foreach (Flipper cargo in flippers.Where(flipper => flipper.IsTankerCargo))
            if (unresolvedCargoUnits.Contains(cargo.CargoUnitId))
                cargoUnitsToReenter.Add(cargo.CargoUnitId);

        ResetPlayerPosition();
        shots.Clear();
        enemyBullets.Clear();
        flippers.Clear();
        shotReleasedCargoPendingActivation.Clear();
        spikers.Clear();
        tankers.Clear();
        developmentCollisionOrder.Clear();
        // A life loss rebuilds unresolved enemies as fresh Eggs, never resumed enemy objects.
        CurrentLevel.RestoreTankerEntries(unresolvedOriginalSlots, cargoUnitsToReenter);
        CurrentLevel.State = LevelState.Playing;
    }

    private void ResetAfterDeath()
    {
        // Ordinary life loss: every active enemy, shot and bullet is discarded and unresolved
        // enemies re-enter as fresh Eggs. Destroyed enemies stay destroyed and score is kept.
        // Spikes are level geometry and persist.
        if (tankerTest)
        {
            ResetTankerScenarioAfterDeath();
            return;
        }

        ResetPlayerPosition();
        shots.Clear();
        enemyBullets.Clear();
        flippers.Clear();
        shotReleasedCargoPendingActivation.Clear();
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
            // Game Over clears the field; unlike an ordinary life loss nothing re-enters until restart,
            // which goes through ResetGame (score and lives reset).
            CurrentLevel.State = LevelState.GameOver;
            shots.Clear();
            enemyBullets.Clear();
            flippers.Clear();
            shotReleasedCargoPendingActivation.Clear();
            spikers.Clear();
            tankers.Clear();
            developmentCollisionOrder.RemoveAll(target => target is not Spike);
            if (!tankerTest)
                CurrentLevel.ReturnSurvivorsToReserve();
            return;
        }

        ResetAfterDeath();
    }

    private void KillPlayer()
    {
        // Single entry to the death lifecycle; the Playing guard prevents a second lethal event in
        // the same update from restarting the sequence or deducting another life.
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
            bool hit = (CurrentLevel.SpikerDevelopmentWave || enemyBulletsTest || tankerTest)
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
        while ((tankerTest
            ? CurrentLevel.HatchReadyEnemyWithIdentity(CreateTankerWaveEnemy)
            : CurrentLevel.HatchReadyEnemy(CreateHatchedEnemy)) is Enemy enemy)
        {
            if (enemy is Flipper flipper)
                flippers.Add(flipper);
            else if (enemy is Spiker spiker)
                spikers.Add(spiker);
            else if (enemy is Tanker tanker)
                tankers.Add(tanker);
            if (CurrentLevel.SpikerDevelopmentWave || enemyBulletsTest || tankerTest)
                developmentCollisionOrder.Add(enemy);
        }
    }

    private Enemy CreateTankerWaveEnemy(Egg egg)
    {
        if (egg.CargoUnitId >= 0)
        {
            // Released cargo returns as separate fresh Flipper entrants, not as a new Tanker.
            Flipper cargo = Flipper.CreateFreshTankerCargo(egg.EdgeIndex, egg.CargoUnitId);
            if (enemyBulletsTest)
                cargo.SetBulletAmmunition(random.Next(1, 4));
            return cargo;
        }

        if (egg.OriginalSlotId == CurrentLevel.TankerSlotId && unresolvedOriginalSlots.Contains(egg.OriginalSlotId))
        {
            // Tankers fire only when --tanker-test is combined with --enemy-bullets-test, mirroring
            // how that flag arms Flippers; --tanker-test alone stays the silent cargo scenario.
            // Incitti Tanker.Create: hasbullets = Rand(1,2), bullettime = 16 + Rand(64); the random
            // offset staggers the first shot instead of firing on a fixed early tick. Every hatch
            // draws fresh values, including the fresh Egg after a life loss, so firing state is
            // deliberately recreated rather than preserved across lives.
            return enemyBulletsTest
                ? new Tanker(egg.EdgeIndex, egg.OriginalSlotId,
                    bulletAmmunition: NextRandomInclusive(1, 2),
                    bulletTimer: 16 + NextRandomInclusive(1, 64))
                : new Tanker(egg.EdgeIndex, egg.OriginalSlotId);
        }

        Enemy enemy = CreateHatchedEnemy(egg.EdgeIndex);
        if (enemy is Flipper flipper)
            flipper.SetOriginalSlotId(egg.OriginalSlotId);
        return enemy;
    }

    private Enemy CreateHatchedEnemy(int lane)
    {
        if (CurrentLevel.SpikerDevelopmentWave && spikers.Count <= 3 &&
            flippers.Count + spikers.Count + Eggs.Count >= 4 && random.Next(-6, 2) <= 0)
        {
            // Fixed development speed avoids guessing BlitzMax's fractional Rand coercion.
            return new Spiker(lane, 3);
        }
        int bulletAmmunition = enemyBulletsTest ? random.Next(1, 4) : 0;
        return new Flipper(lane, 0, bulletAmmunition);
    }

    private bool CheckShotAgainstDevelopmentTargets(Shot shot)
    {
        // Incitti tests insertion order, not nearest depth. Level 1 keeps its old path.
        // A hit consumes the shot (return true), so a shot that destroys a Tanker can never also
        // hit the cargo created by that same release.
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
                case EnemyBullet enemyBullet when enemyBullet.IsHitBy(shot):
                    enemyBullets.Remove(enemyBullet);
                    developmentCollisionOrder.Remove(enemyBullet);
                    Score.Add(25);
                    return true;
                case Tanker tanker when tanker.IsHitBy(shot):
                    tankers.Remove(tanker);
                    developmentCollisionOrder.Remove(tanker);
                    ResolveOriginalSlotDestroyed(tanker.OriginalSlotId);
                    Score.Add(200);
                    ReleaseTankerCargo(tanker, TankerReleaseKind.PlayerShotDestruction);
                    return true;
                case Flipper flipper when flipper.IsHitBy(shot):
                    flippers.Remove(flipper);
                    developmentCollisionOrder.Remove(flipper);
                    if (tankerTest)
                    {
                        if (flipper.IsTankerCargo)
                            ResolveCargoUnit(flipper.CargoUnitId);
                        else
                            ResolveOriginalSlotDestroyed(flipper.OriginalSlotId);
                    }
                    else
                    {
                        CurrentLevel.RecordEnemyDestroyed();
                    }
                    Score.Add(150);
                    return true;
            }
        }
        return false;
    }

    private void UpdateSpikers()
    {
        // A retreating Spiker returns its enemy to the reserve instead of destroying it, so the
        // unit is conserved for later re-entry. Its spikes remain on the tube.
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

    private void UpdateTankers()
    {
        // A Tanker that climbs past the release threshold escapes its original slot (no score)
        // and hands its cargo to the field.
        for (int index = 0; index < tankers.Count; index++)
        {
            Tanker tanker = tankers[index];
            TankerUpdateResult result = tanker.UpdateForDevelopment(
                enemyBulletsTest ? NextRandomInclusive : null, out EnemyBullet? bullet);
            if (bullet is not null)
            {
                // UpdateEnemyBullets already ran this tick, so a bullet created here first moves
                // next tick (same policy as Flipper bullets). It joins the insertion-ordered
                // development collision list at its creation point and lives independently of the
                // Tanker afterwards: destroying or releasing the Tanker never removes it.
                enemyBullets.Add(bullet);
                developmentCollisionOrder.Add(bullet);
            }

            if (result != TankerUpdateResult.NaturalCargoRelease)
                continue;

            tankers.RemoveAt(index--);
            developmentCollisionOrder.Remove(tanker);
            ResolveOriginalSlotEscaped(tanker.OriginalSlotId);
            ReleaseTankerCargo(tanker, TankerReleaseKind.Natural);
        }
    }

    private void ReleaseTankerCargo(Tanker tanker, TankerReleaseKind releaseKind)
    {
        // Natural and shot release both create exactly two cargo Flippers with fresh cargo IDs.
        // They are added after this tick's Flipper update, so their first update is next tick.
        // Natural release awards no score; the 200 points for a shot kill are awarded by the caller.
        TankerReleaseResult result = tanker.ReleaseFlipperCargo(
            CurrentLevel, firstSideDirection: 1, releaseKind);
        foreach (TankerCargoSpawn spawn in result.Cargo)
        {
            int cargoUnitId = nextCargoUnitId++;
            unresolvedCargoUnits.Add(cargoUnitId);
            int ammunition = enemyBulletsTest ? random.Next(1, 4) : 0;
            Flipper flipper = Flipper.CreateTankerCargo(CurrentLevel, spawn, cargoUnitId, ammunition);
            if (releaseKind == TankerReleaseKind.PlayerShotDestruction)
                // The shot that killed the Tanker is already consumed; deferring activation keeps
                // the new cargo out of the development collision list until the next update.
                shotReleasedCargoPendingActivation.Add(flipper);
            else
            {
                flippers.Add(flipper);
                developmentCollisionOrder.Add(flipper);
            }
        }
    }

    private void ResolveOriginalSlotDestroyed(int originalSlotId)
    {
        if (originalSlotId < 0 || !unresolvedOriginalSlots.Remove(originalSlotId))
            return;
        destroyedOriginalSlots.Add(originalSlotId);
    }

    private void ResolveOriginalSlotEscaped(int originalSlotId)
    {
        if (originalSlotId < 0 || !unresolvedOriginalSlots.Remove(originalSlotId))
            return;
        escapedOriginalSlots.Add(originalSlotId);
    }

    private void ResolveCargoUnit(int cargoUnitId)
    {
        if (unresolvedCargoUnits.Remove(cargoUnitId))
            resolvedCargoUnits.Add(cargoUnitId);
    }

    private void UpdateFlippers()
    {
        foreach (Flipper flipper in flippers)
        {
            EnemyBullet? bullet = flipper.Update(CurrentLevel, Player.EdgeIndex,
                enemyBulletsTest ? NextRandomInclusive : null);
            if (bullet is null)
                continue;

            enemyBullets.Add(bullet);
            if (CurrentLevel.SpikerDevelopmentWave || enemyBulletsTest)
                developmentCollisionOrder.Add(bullet);
        }
    }

    private void ActivateShotReleasedCargo()
    {
        foreach (Flipper flipper in shotReleasedCargoPendingActivation)
        {
            flippers.Add(flipper);
            developmentCollisionOrder.Add(flipper);
        }
        shotReleasedCargoPendingActivation.Clear();
    }

    private int NextRandomInclusive(int minimum, int maximum) =>
        random.Next(minimum, maximum + 1);

    private void UpdateEnemyBullets()
    {
        for (int bulletIndex = enemyBullets.Count - 1; bulletIndex >= 0; bulletIndex--)
        {
            EnemyBullet bullet = enemyBullets[bulletIndex];
            bullet.Advance();
            if (!bullet.IsExpired)
                continue;

            enemyBullets.RemoveAt(bulletIndex);
            developmentCollisionOrder.Remove(bullet);
        }
    }

    private void CheckEnemyBulletsAgainstPlayer()
    {
        if (State != LevelState.Playing)
            return;

        for (int bulletIndex = enemyBullets.Count - 1; bulletIndex >= 0; bulletIndex--)
        {
            EnemyBullet bullet = enemyBullets[bulletIndex];
            if (!bullet.HitsPlayer(Player))
                continue;

            enemyBullets.RemoveAt(bulletIndex);
            developmentCollisionOrder.Remove(bullet);
            KillPlayer();
            return;
        }
    }

    private void CheckFlippersAgainstPlayer()
    {
        // CompletedIntoPlayer is raised by the Flipper both for a direct rim arrival in the occupied
        // lane and for a pivot that completes into it. It is consumed here so the Flipper never
        // touches game state. Runs after player-shot collision, so a same-tick kill saves the player.
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
        // Normal Level 1 uses the conservation check in Level.IsComplete. The Tanker scenario
        // has a dynamic ledger (cargo adds units), so it requires every slot and cargo unit resolved.
        bool complete = tankerTest
            ? unresolvedOriginalSlots.Count == 0 && unresolvedCargoUnits.Count == 0 &&
              CurrentLevel.ReserveEggs == 0 && Eggs.Count == 0 && tankers.Count == 0 &&
              flippers.Count == 0 && spikers.Count == 0
            : CurrentLevel.IsComplete(flippers.Count + spikers.Count);
        if (State != LevelState.Playing || !complete)
            return false;

        CurrentLevel.State = CurrentLevel.SpikerDevelopmentWave ? LevelState.AvoidSpikes : LevelState.ExitDelay;
        exitDelayTicks = 0;
        shots.Clear();
        enemyBullets.Clear();
        developmentCollisionOrder.RemoveAll(target => target is EnemyBullet);
        return true;
    }

    private void UpdateExitZoom(bool completeWhenFinished = true)
    {
        // Two phases: first pull the camera back (ZOffset) while the player rides down the tube,
        // then increase projection K to rush the tube past. Constants are frame-based values from the reference.
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
            enemyBullets.Clear();
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
        // Development-only: touching a spike during or after the wave starts a reverse zoom instead
        // of the normal death sequence. Spikes are not enemies and never count toward accounting.
        if (!CurrentLevel.SpikerDevelopmentWave || DeathTicks > 0 ||
            State is not (LevelState.Playing or LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom))
            return;
        if (!Spikes.Any(spike => spike.HitsPlayer(Player)))
            return;

        DeathTicks = PlayerDeathTicks;
        CurrentLevel.State = LevelState.ReverseZoom;
        exitDelayTicks = 0;
        shots.Clear();
        enemyBullets.Clear();
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
