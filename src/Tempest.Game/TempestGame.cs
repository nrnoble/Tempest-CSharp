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
/// Normal play progresses through the Level 5 production configuration. Direct Level 2/3/4
/// access, the Level 3 geometry diagnostic, the configuration-based headless Level 3 mixed model,
/// and the Spiker, Tanker, Fuseball and Pulsar scenarios are development-only. Natural production
/// Flippers use a separate arcade-style firing phase; enemy bullets retain the existing accounting,
/// collision and travel paths.
/// </remarks>
public sealed class TempestGame
{
    public const double SimulationStepSeconds = 1.0 / 60.0;
    // Length of the death sequence in simulation updates; a life is deducted on the last one.
    public const int PlayerDeathTicks = 60;
    private const float PlayerSpeed = 0.05f;
    private const float SpinnerSensitivity = 0.0125f;
    private const int MaximumShots = 10;
    private const int PlayerShotCooldownTicks = 3;
    private const int SpikeExitShotCooldownTicks = 4;
    // $30 maps to Height 342.86 when arcade $F0/$10 map to project entry/rim 0/400; 343 is the
    // nearest integer C# cutoff, not an exact conversion of the ROM's along coordinate.
    private const int MaximumFlipperFiringHeight = 343;
    private static readonly int[] EnemyShotPressureThresholds = [0x00, 0xE0, 0xF0, 0xFA, 0xFF];
    private readonly List<Shot> shots = [];
    private readonly List<EnemyBullet> enemyBullets = [];
    private readonly List<Flipper> flippers = [];
    // Cargo released by a player shot waits here so the same shot and the same update cannot affect it.
    private readonly List<Flipper> shotReleasedCargoPendingActivation = [];
    private readonly List<Spiker> spikers = [];
    private readonly List<Tanker> tankers = [];
    private readonly List<Fuseball> fuseballs = [];
    private readonly List<Pulsar> pulsars = [];
    private readonly List<FuseballScorePopup> fuseballScorePopups = [];
    // Incitti resolves shot collisions in object-creation order, not nearest depth, so development
    // scenarios record spawn order here. Spikes survive life loss; enemies and bullets do not.
    private readonly List<object> developmentCollisionOrder = [];
    // Tanker scenario ledger. Original wave slots and Tanker cargo units are tracked separately;
    // this development scenario adds cargo on top of the configured original supply.
    // Rim-cleared identities are separate outcomes from kills; unresolved IDs survive life loss,
    // while runtime enemy objects do not.
    private readonly HashSet<int> unresolvedOriginalSlots = [];
    private readonly HashSet<int> destroyedOriginalSlots = [];
    private readonly HashSet<int> escapedOriginalSlots = [];
    private readonly HashSet<int> rimClearedOriginalSlots = [];
    private readonly HashSet<int> unresolvedCargoUnits = [];
    private readonly HashSet<int> resolvedCargoUnits = [];
    private readonly HashSet<int> rimClearedCargoUnits = [];
    private readonly Random random;
    private readonly int startingLives;
    private readonly IAudioEvents audioEvents;
    // New-game restarts target the original selection even after natural progression changes the active Level.
    private readonly LevelConfiguration launchConfiguration;
    // Development scenarios stay terminal even when their runtime uses the Level 1 configuration identity.
    private readonly bool naturalProgressionEnabled;
    private readonly bool spikerDevelopmentWave;
    private readonly bool enemyBulletsTest;
    private readonly bool tankerTest;
    private readonly bool level3Test;
    private readonly bool level4Test;
    private readonly bool level6Test;
    private readonly bool level3GeometryTest;
    private Level? currentLevel;
    private int pulsePhase;
    private float pulseZh;
    private int fuseballAnimationTicks;
    private int nextCargoUnitId;
    private int shotTimer = 4;
    private int exitDelayTicks;

    public Player Player { get; }
    public Level CurrentLevel => currentLevel ??
        throw new InvalidOperationException("The current Level has not been initialized.");
    public PlayerDeathPresentation DeathPresentation { get; } = new(PlayerDeathTicks);
    // The model controls rendering only; logical enemies stay frozen for existing reconstruction.
    public float EnemyRetractionProgress => State == LevelState.PlayerDying
        ? DeathPresentation.EnemyRetractionProgress
        : 0;
    public ScoreSystem Score { get; } = new();
    public SuperZapperSystem SuperZapper { get; } = new();
    public IReadOnlyList<Shot> Shots => shots;
    public IReadOnlyList<EnemyBullet> EnemyBullets => enemyBullets;
    public IReadOnlyList<Flipper> Flippers => flippers;
    public IReadOnlyList<Egg> Eggs => CurrentLevel.Eggs;
    public IReadOnlyList<Spiker> Spikers => spikers;
    public IReadOnlyList<Tanker> Tankers => tankers;
    public IReadOnlyList<Fuseball> Fuseballs => fuseballs;
    public IReadOnlyList<Pulsar> Pulsars => pulsars;
    public IReadOnlyList<FuseballScorePopup> FuseballScorePopups => fuseballScorePopups;
    public bool FuseballTest { get; }
    public bool PulsarTest { get; }
    public bool Level2Test { get; }
    public bool Level3Test => level3Test;
    public bool Level4Test => level4Test;
    public bool Level6Test => level6Test;
    public bool Level3GeometryTest => level3GeometryTest;
    public bool NaturalProgressionEnabled => naturalProgressionEnabled;
    public LevelApproach? CurrentApproach { get; private set; }
    public bool HurryUp { get; private set; }
    public int PulsePhase => pulsePhase;
    public bool Pulsing { get; private set; }
    public float PulseZh => pulseZh;
    // Incitti shares four animation frames across all Fuseballs, changing frame every four ticks.
    public int FuseballAnimationFrame => fuseballAnimationTicks / 4;
    public IReadOnlyList<Spike> Spikes => CurrentLevel.Spikes;
    private bool SpikeExitEnabled =>
        CurrentLevel.SpikerDevelopmentWave ||
        CurrentLevel.Configuration.ExitPolicy == LevelExitPolicy.AvoidSpikes;
    private bool FlipperBulletFiringEnabled =>
        naturalProgressionEnabled || level6Test || enemyBulletsTest && !tankerTest &&
        !spikerDevelopmentWave && !CurrentLevel.UsesProductionLedger;
    public bool SpikeExitControlsEnabled =>
        SpikeExitEnabled &&
        State is LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom;
    public bool ExitMovementInputEnabled =>
        State == LevelState.ExitZoom || SpikeExitControlsEnabled;
    public bool ShouldShowAvoidSpikesWarning =>
        CurrentLevel.Configuration.ExitPolicy == LevelExitPolicy.AvoidSpikes &&
        (State is LevelState.AvoidSpikes or LevelState.ExitDelay) &&
        DeathTicks == 0;
    public bool SpikeExitShotsVisible => SpikeExitEnabled &&
        State is LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom or LevelState.ReverseZoom;
    public bool EnemyBulletsTest => enemyBulletsTest;
    public bool TankerTest => tankerTest;
    public int TankerOriginalSlotsRemaining => unresolvedOriginalSlots.Count;
    public int TankerOriginalSlotsDestroyed => destroyedOriginalSlots.Count;
    public int TankerOriginalSlotsEscaped => escapedOriginalSlots.Count;
    public int TankerOriginalSlotsRimCleared => rimClearedOriginalSlots.Count;
    public int TankerOriginalSlotsResolved =>
        destroyedOriginalSlots.Count + escapedOriginalSlots.Count + rimClearedOriginalSlots.Count;
    public int TankerLatentCargoUnits => tankers.Sum(tanker => tanker.LatentCargoCount) +
        (tankerTest && unresolvedOriginalSlots.Contains(0) && tankers.All(tanker => tanker.OriginalSlotId != 0)
            ? Tanker.CarriedFlipperCount
            : 0);
    public int TankerCargoUnitsRemaining => unresolvedCargoUnits.Count + TankerLatentCargoUnits;
    public int TankerMaterializedCargoUnits =>
        unresolvedCargoUnits.Count + resolvedCargoUnits.Count + rimClearedCargoUnits.Count;
    public int TankerCargoUnitsKilled => resolvedCargoUnits.Count;
    // Resolved is the aggregate terminal count; callers needing score-bearing outcomes use Killed.
    public int TankerCargoUnitsResolved => resolvedCargoUnits.Count + rimClearedCargoUnits.Count;
    public int TankerCargoUnitsRimCleared => rimClearedCargoUnits.Count;
    public int ActiveTankerCargoFlippers => flippers.Count(flipper => flipper.IsTankerCargo);
    public int PendingTankerCargoUnits => Math.Max(0, unresolvedCargoUnits.Count - ActiveTankerCargoFlippers);
    public int ActiveTankerOriginalEnemies =>
        tankers.Count + flippers.Count(flipper => !flipper.IsTankerCargo);
    public int TankerAccountingTotal => destroyedOriginalSlots.Count + escapedOriginalSlots.Count +
        rimClearedOriginalSlots.Count + unresolvedOriginalSlots.Count + unresolvedCargoUnits.Count +
        resolvedCargoUnits.Count + rimClearedCargoUnits.Count;
    public int TankerActiveTotal => Eggs.Count + tankers.Count + flippers.Count + spikers.Count;
    public int DeathTicks { get; private set; }
    public int SuperZapperChargesRemaining => SuperZapper.ChargesRemaining;
    public LevelState State => CurrentLevel.State;
    public bool GameOver => State == LevelState.GameOver;
    public bool LevelComplete => State == LevelState.Complete;
    public bool IsExiting => State is LevelState.ExitDelay or LevelState.ExitZoom or
        LevelState.AvoidSpikes or LevelState.ReverseZoom;

    public TempestGame(bool spikerDevelopmentWave = false, Random? random = null,
        int startingLives = Player.DefaultStartingLives, bool enemyBulletsTest = false,
        bool tankerTest = false, bool fuseballTest = false, bool pulsarTest = false,
        bool level2Test = false, bool level3GeometryTest = false,
        LevelConfiguration? startingConfiguration = null, bool level3Test = false,
        bool level4Test = false, IAudioEvents? audioEvents = null, bool level6Test = false)
    {
        if ((level2Test ? 1 : 0) + (level3Test ? 1 : 0) +
            (level4Test ? 1 : 0) + (level6Test ? 1 : 0) + (level3GeometryTest ? 1 : 0) +
            (spikerDevelopmentWave ? 1 : 0) + (tankerTest ? 1 : 0) +
            (fuseballTest ? 1 : 0) + (pulsarTest ? 1 : 0) > 1 ||
            startingConfiguration is not null &&
            (level2Test || level3Test || level4Test || level3GeometryTest || spikerDevelopmentWave ||
             level6Test || tankerTest || fuseballTest || pulsarTest))
            throw new ArgumentException("Level 2, Level 3 mixed, Level 4 candidate, Level 6 production, Level 3 geometry, Tanker, Spiker, Fuseball and Pulsar development scenarios cannot be combined.");

        this.random = random ?? Random.Shared;
        this.startingLives = startingLives > 0 ? startingLives : Player.DefaultStartingLives;
        this.audioEvents = audioEvents ?? SilentAudioEvents.Instance;
        this.launchConfiguration = startingConfiguration ?? (level2Test
            ? LevelConfiguration.Level2Square
            : level3Test
                ? LevelConfiguration.Level3Mixed
            : level4Test
                ? LevelConfiguration.Level4Candidate
            : level6Test
                ? LevelConfiguration.Level6Production
            : level3GeometryTest
                ? LevelConfiguration.Level3Plus
            : LevelConfiguration.Level1);
        this.spikerDevelopmentWave = spikerDevelopmentWave;
        this.enemyBulletsTest = enemyBulletsTest;
        this.tankerTest = tankerTest;
        this.level3Test = level3Test;
        this.level4Test = level4Test;
        this.level6Test = level6Test;
        this.level3GeometryTest = level3GeometryTest;
        naturalProgressionEnabled = startingConfiguration is null &&
            !level2Test && !level3Test && !level4Test && !level6Test &&
            !spikerDevelopmentWave && !tankerTest &&
            !fuseballTest && !pulsarTest && !level3GeometryTest;
        FuseballTest = fuseballTest;
        PulsarTest = pulsarTest;
        Level2Test = level2Test;
        Player = new Player(this.startingLives);
        InitializeFreshWave(launchConfiguration);
    }

    public TempestGame(
        LevelConfiguration startingConfiguration,
        Random? random = null,
        int startingLives = Player.DefaultStartingLives,
        IAudioEvents? audioEvents = null)
        : this(spikerDevelopmentWave: false, random: random, startingLives: startingLives,
            startingConfiguration: startingConfiguration ??
                throw new ArgumentNullException(nameof(startingConfiguration)),
            audioEvents: audioEvents)
    {
    }

    public void RunOneStep(PlayerInput input = default, bool restartPressed = false)
    {
        if (State == LevelState.Approach)
        {
            UpdateLevelApproach(input);
            return;
        }

        // The display continues to advance during death freezes even though enemy timers do not.
        SuperZapper.AdvanceDisplayStep();
        // Labels have their own short lifetime, including death/exit; gameplay entities freeze.
        foreach (FuseballScorePopup popup in fuseballScorePopups)
            popup.Update();
        fuseballScorePopups.RemoveAll(popup => popup.IsExpired);

        if (GameOver)
        {
            if (restartPressed)
                ResetGame();
        }
        else if (LevelComplete)
        {
            if (restartPressed)
            {
                if (naturalProgressionEnabled &&
                    LevelProgression.GetNextProductionLevel(CurrentLevel.Configuration) is null)
                    // A naturally reached terminal production level starts a fresh campaign.
                    ResetGame();
                else
                    // Development scenarios replay their launch configuration, not a production next level.
                    InitializeFreshWave(launchConfiguration);
            }
        }
        else if (State == LevelState.ReverseZoom)
        {
            UpdateReverseZoom();
            if (SpikeExitEnabled)
                // Incitti keeps shots moving through reverse zoom; enemy simulation stays frozen.
                UpdatePlayerShots(spikesOnly: true);
        }
        else if (SpikeExitControlsEnabled)
        {
            UpdateSpikeExit(input);
        }
        else if (State == LevelState.ExitDelay)
        {
            exitDelayTicks++;
            if (exitDelayTicks > 20)
            {
                CurrentLevel.State = LevelState.ExitZoom;
                audioEvents.Play(AudioEvent.ZoomOut);
            }
        }
        else if (State == LevelState.ExitZoom)
        {
            UpdateExitZoom();
            // As in Spike travel, do not apply outgoing-tube input after the approach handoff.
            if (State == LevelState.ExitZoom)
                UpdatePlayerMovement(input);
        }
        else if (State == LevelState.PlayerDying)
        {
            UpdatePlayerDeath();
        }
        else
        {
            // The one shared pulse clock advances only during active play. Like the current C#
            // entity simulation, it freezes for the full PlayerDying interval.
            AdvancePulseClock();
            // Order matters. Player firing and shot-versus-enemy collision run before enemy updates
            // and zap expiry, so an ordinary shot preempts a pending zap and a last-safe-tick kill
            // saves the player. Do not move the player-collision checks earlier.
            UpdatePlayer(input);
            if (input.ZapPressed && State == LevelState.Playing)
                SuperZapper.TryActivate(GetZapperSourceOrder(), Player.EdgeIndex,
                    CurrentLevel.Definition.Points.Count, CurrentLevel.Continuous);
            if (FuseballTest)
                fuseballAnimationTicks = (fuseballAnimationTicks + 1) % 16;
            UpdatePlayerShots();
            UpdateEnemyBullets();
            if (CheckForLevelComplete())
                return;

            CurrentLevel.UpdateEggRelease();
            if (CurrentLevel.UsesProductionLedger)
                CurrentLevel.UpdateLevel3MixedEggs(
                    CountProductionActiveEnemyUnits(), NextRandomInclusive);
            else
                CurrentLevel.UpdateEggs();
            HatchEggs();
            // Accepted delays count this activation tick; expiry skips that enemy's ordinary update.
            UpdateFlippers();
            UpdateTankers();
            // Runs after UpdateFlippers/UpdateTankers so shot-released cargo skips this tick's update.
            ActivateShotReleasedCargo();
            UpdateSpikers();
            UpdateFuseballs();
            UpdatePulsars();
            UpdateFlipperFiring();
            // Once a collision starts death, stop active gameplay before later collisions,
            // all-rim completion or Hurry-Up can process on the same tick.
            CheckFlippersAgainstPlayer();
            if (State != LevelState.Playing)
                return;
            CheckFuseballsAgainstPlayer();
            if (State != LevelState.Playing)
                return;
            CheckPulsarsAgainstPlayer();
            if (State != LevelState.Playing)
                return;
            CheckEnemyBulletsAgainstPlayer();
            if (State != LevelState.Playing)
                return;
            CheckSpikesAgainstPlayer();
            if (State != LevelState.Playing)
                return;
            if (TryCompleteAllRim())
                return;
            LatchHurryUp();
            CheckForLevelComplete();
            if (CurrentLevel.UsesProductionLedger && State != LevelState.GameOver)
                ValidateLevel3WaveRepresentation();
        }
    }

    private void ResetGame()
    {
        Score.Reset();
        Player.Lives = startingLives;
        InitializeFreshWave(launchConfiguration);
    }

    // Fresh initialization installs a new Level; life reconstruction deliberately retains it.
    // Detaching and rebinding the one SpikeCreated handler keeps old hazards out of the new wave.
    private void InitializeFreshWave(LevelConfiguration configuration, bool beginApproach = false)
    {
        LevelApproach? approach = beginApproach
            ? new LevelApproach(CurrentLevel.Configuration.DisplayedLevel)
            : null;
        if (currentLevel is not null)
            currentLevel.SpikeCreated -= OnSpikeCreated;

        developmentCollisionOrder.Clear();
        currentLevel = new Level(configuration, spikerDevelopmentWave, tankerTest,
            NextRandomInclusive);
        currentLevel.SpikeCreated += OnSpikeCreated;
        developmentCollisionOrder.AddRange(currentLevel.Spikes);
        CurrentLevel.State = beginApproach ? LevelState.Approach : LevelState.Playing;
        CurrentApproach = approach;
        if (beginApproach)
            audioEvents.Play(AudioEvent.ZoomIn);
        exitDelayTicks = 0;
        RestoreProjection();
        DeathTicks = 0;
        DeathPresentation.Reset();
        HurryUp = false;
        SuperZapper.Reset();
        ResetPlayerPosition();
        shots.Clear();
        enemyBullets.Clear();
        flippers.Clear();
        shotReleasedCargoPendingActivation.Clear();
        spikers.Clear();
        tankers.Clear();
        fuseballs.Clear();
        pulsars.Clear();
        fuseballScorePopups.Clear();
        fuseballAnimationTicks = 0;
        ResetTankerLedger();
        if (CurrentLevel.UsesProductionLedger)
            ValidateLevel3WaveRepresentation();
    }

    private void OnSpikeCreated(Spike spike) => developmentCollisionOrder.Add(spike);

    private void ResetTankerLedger()
    {
        unresolvedOriginalSlots.Clear();
        destroyedOriginalSlots.Clear();
        escapedOriginalSlots.Clear();
        rimClearedOriginalSlots.Clear();
        unresolvedCargoUnits.Clear();
        resolvedCargoUnits.Clear();
        rimClearedCargoUnits.Clear();
        nextCargoUnitId = 0;
        if (tankerTest)
            for (int slotId = 0; slotId < CurrentLevel.EnemySupply; slotId++)
                unresolvedOriginalSlots.Add(slotId);
    }

    private void ResetTankerScenarioAfterDeath()
    {
        // Preserve unresolved cargo identities, but discard each runtime child's pending timer.
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
        DeathPresentation.Reset();
        if (CurrentLevel.UsesProductionLedger)
        {
            ValidateLevel3WaveRepresentation();
            ResetPlayerPosition();
            shots.Clear();
            enemyBullets.Clear();
            flippers.Clear();
            shotReleasedCargoPendingActivation.Clear();
            tankers.Clear();
            spikers.Clear();
            fuseballs.Clear();
            pulsars.Clear();
            fuseballScorePopups.Clear();
            developmentCollisionOrder.Clear();
            developmentCollisionOrder.AddRange(CurrentLevel.Spikes);
            CurrentLevel.ResetAfterDeath();
            CurrentLevel.State = LevelState.Playing;
            ValidateLevel3WaveRepresentation();
            return;
        }

        // Reconstruction keeps the same Level/configuration; only a fresh wave creates a new Level.
        // Ordinary life loss: every active enemy, shot and bullet is discarded and unresolved
        // enemies re-enter as fresh Eggs. Destroyed enemies stay destroyed and score is kept.
        // Spikes are level geometry and persist.
        // Incitti's rimit belongs to the same-level attempt, not the reconstructed enemy objects.
        // Keep the latch even though fresh Eggs temporarily make its original trigger false.
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
        fuseballs.Clear();
        pulsars.Clear();
        fuseballScorePopups.Clear();
        developmentCollisionOrder.RemoveAll(target => target is not Spike);
        CurrentLevel.ResetAfterDeath();
        CurrentLevel.State = LevelState.Playing;
    }

    private void ResetPlayerPosition()
    {
        Player.ResetPosition(CurrentLevel.Configuration.InitialPlayerLane,
            CurrentLevel.Configuration.InitialPlayerLanePosition);
        shotTimer = 4;
    }

    private void UpdatePlayerDeath()
    {
        DeathPresentation.Advance();
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
            fuseballs.Clear();
            pulsars.Clear();
            fuseballScorePopups.Clear();
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
        DeathPresentation.Start();
        CurrentLevel.State = LevelState.PlayerDying;
        shots.Clear();
    }

    private void UpdatePlayer(
        PlayerInput input,
        bool firingEnabled = true,
        int shotCooldownTicks = PlayerShotCooldownTicks)
    {
        UpdatePlayerFiring(input.Fire && firingEnabled, shotCooldownTicks);

        UpdatePlayerMovement(input);
    }

    private void UpdatePlayerMovement(PlayerInput input)
    {
        float movement = input.MouseDeltaX * SpinnerSensitivity;
        if (input.Left)
            movement -= PlayerSpeed;
        if (input.Right)
            movement += PlayerSpeed;
        movement += input.ControllerAxis * PlayerSpeed;

        int previousLane = Player.EdgeIndex;
        Player.MoveAlongTube(movement, CurrentLevel);
        if (Player.EdgeIndex != previousLane)
            audioEvents.Play(AudioEvent.PlayerMove);
    }

    private void UpdateLevelApproach(PlayerInput input)
    {
        LevelApproach approach = CurrentApproach ??
            throw new InvalidOperationException("The approach state has no transition model.");
        UpdatePlayerMovement(input);
        if (approach.Advance())
        {
            CurrentLevel.State = LevelState.Playing;
            CurrentApproach = null;
        }
    }

    private IEnumerable<Enemy> GetZapperSourceOrder()
    {
        if (CurrentLevel.UsesProductionLedger ||
            CurrentLevel.SpikerDevelopmentWave || enemyBulletsTest || tankerTest ||
            naturalProgressionEnabled ||
            FuseballTest || PulsarTest)
            // Spikes are not Enemy instances; bullets remain in this source order and reject by default.
            return developmentCollisionOrder.OfType<Enemy>();

        // Normal Flipper waves keep creation order; their separate shot scan remains reversed.
        return flippers;
    }

    private void UpdatePlayerFiring(bool fireButtonDown, int cooldownTicks)
    {
        if (fireButtonDown && shotTimer >= cooldownTicks && shots.Count < MaximumShots)
        {
            shots.Add(new Shot { EdgeIndex = Player.EdgeIndex, Height = Player.Depth });
            shotTimer = 0;
            audioEvents.Play(AudioEvent.PlayerShot);
        }

        shotTimer++;
    }

    private void UpdatePlayerShots(bool spikesOnly = false)
    {
        for (int shotIndex = shots.Count - 1; shotIndex >= 0; shotIndex--)
        {
            Shot shot = shots[shotIndex];
            shot.Update();
            bool hit = shot.Height >= 0 && (spikesOnly
                ? CheckShotAgainstSpikes(shot)
                : (CurrentLevel.UsesProductionLedger ||
                CurrentLevel.SpikerDevelopmentWave || enemyBulletsTest || tankerTest ||
                FuseballTest || PulsarTest)
                    ? CheckShotAgainstDevelopmentTargets(shot)
                    : CheckShotAgainstFlippers(shot));
            if (hit || shot.Height < 0)
                shots.RemoveAt(shotIndex);
        }
    }

    private bool CheckShotAgainstSpikes(Shot shot)
    {
        foreach (Spike spike in CurrentLevel.Spikes)
        {
            if (!spike.IsHitBy(shot))
                continue;

            HitSpike(spike);
            return true;
        }

        return false;
    }

    private void HitSpike(Spike spike)
    {
        spike.Shorten();
        Score.Add(1);
        audioEvents.Play(AudioEvent.SpikeHit);
        if (!spike.IsDepleted)
            return;

        CurrentLevel.RemoveSpike(spike);
        developmentCollisionOrder.Remove(spike);
    }

    private bool CheckShotAgainstFlippers(Shot shot)
    {
        for (int flipperIndex = flippers.Count - 1; flipperIndex >= 0; flipperIndex--)
        {
            if (!flippers[flipperIndex].IsHitBy(shot))
                continue;

            Flipper flipper = flippers[flipperIndex];
            flippers.RemoveAt(flipperIndex);
            developmentCollisionOrder.Remove(flipper);
            CurrentLevel.RecordEnemyDestroyed();
            Score.Add(150);
            audioEvents.Play(AudioEvent.EnemyDestroyed);
            return true;
        }

        for (int bulletIndex = enemyBullets.Count - 1; bulletIndex >= 0; bulletIndex--)
        {
            EnemyBullet bullet = enemyBullets[bulletIndex];
            if (!bullet.IsHitBy(shot))
                continue;

            enemyBullets.RemoveAt(bulletIndex);
            developmentCollisionOrder.Remove(bullet);
            Score.Add(25);
            return true;
        }

        return false;
    }

    private void HatchEggs()
    {
        if (CurrentLevel.UsesProductionLedger)
        {
            CurrentLevel.HatchReadyMixedEnemies(
                CreateLevel3MixedEnemy, AddHatchedEnemy);
            return;
        }

        while ((tankerTest
            ? CurrentLevel.HatchReadyEnemyWithIdentity(CreateTankerWaveEnemy)
            : CurrentLevel.HatchReadyEnemy(CreateHatchedEnemy)) is Enemy enemy)
            AddHatchedEnemy(enemy);
    }

    private void AddHatchedEnemy(Enemy enemy)
    {
        if (enemy is Flipper flipper)
            flippers.Add(flipper);
        else if (enemy is Spiker spiker)
            spikers.Add(spiker);
        else if (enemy is Tanker tanker)
            tankers.Add(tanker);
        else if (enemy is Fuseball fuseball)
            fuseballs.Add(fuseball);
        else if (enemy is Pulsar pulsar)
            pulsars.Add(pulsar);
        else
            throw new InvalidOperationException($"Unsupported hatched enemy type {enemy.GetType().Name}.");

        if (CurrentLevel.UsesProductionLedger || CurrentLevel.SpikerDevelopmentWave ||
            FlipperBulletFiringEnabled || tankerTest || FuseballTest || PulsarTest)
            developmentCollisionOrder.Add(enemy);
    }

    private Enemy? CreateLevel3MixedEnemy(Egg egg)
    {
        Level3WaveLedger ledger = CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException("Level 3 mixed wave has no identity ledger.");

        if (egg.CargoUnitId >= 0)
        {
            if (egg.OriginalSlotId >= 0)
                throw new InvalidOperationException("A Level 3 Egg cannot hold both identity namespaces.");
            ledger.MarkCargoActive(egg.CargoUnitId);
            return Flipper.CreateFreshTankerCargo(egg.EdgeIndex, egg.CargoUnitId);
        }

        int originalId = egg.OriginalSlotId;
        if (originalId < 0)
            throw new InvalidOperationException("A production Level 3 Egg has no stable identity.");

        Level3OriginalType selectedType = ledger.GetOriginalType(originalId);
        if (selectedType == Level3OriginalType.Unselected)
        {
            selectedType = CurrentLevel.Configuration.NormalEnemyComposition ==
                LevelEnemyComposition.FlippersTankersAndSpikers
                ? SelectLevel4OriginalType()
                : SelectLevel3OriginalType();
            if (selectedType == Level3OriginalType.Tanker)
                Level3WaveSelection.SelectTwoFlipperCargo(NextRandomInclusive);
        }

        if (selectedType == Level3OriginalType.Spiker)
        {
            if (spikers.Count > 3)
                return null;

            float speed = CurrentLevel.Configuration.ProgressionIndex / 48f +
                2.5f + (float)(random.NextDouble() * 2);
            Spiker spiker = new(egg.EdgeIndex, speed, originalId);
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Egg)
                ledger.CommitOriginalAsSpiker(originalId);
            return spiker;
        }

        if (selectedType == Level3OriginalType.Tanker)
        {
            if (tankers.Count >= 6)
                return null;

            int ammunition = NextRandomInclusive(1, 2);
            int timer = 16 + NextRandomInclusive(1, 64);
            Tanker tanker = new(egg.EdgeIndex, originalId,
                bulletAmmunition: ammunition, bulletTimer: timer);
            if (ledger.GetOriginalState(originalId) == Level3OriginalState.Egg)
                ledger.CommitOriginalAsTanker(originalId);
            return tanker;
        }

        if (selectedType != Level3OriginalType.Flipper)
            throw new InvalidOperationException($"Unsupported Level 3 original type {selectedType}.");

        Flipper flipper = new(egg.EdgeIndex, 0);
        flipper.SetOriginalSlotId(originalId);
        if (ledger.GetOriginalState(originalId) == Level3OriginalState.Egg)
            ledger.CommitOriginalAsFlipper(originalId);
        return flipper;
    }

    private Level3OriginalType SelectLevel3OriginalType()
        => Level3WaveSelection.SelectOriginalType(
            NextRandomInclusive,
            tankers.Count,
            CountProductionActiveEnemyUnits(),
            Eggs.Count);

    private Level3OriginalType SelectLevel4OriginalType()
        => Level3WaveSelection.SelectLevel4OriginalType(
            NextRandomInclusive,
            tankers.Count,
            spikers.Count,
            flippers.Count + tankers.Count + spikers.Count +
                shotReleasedCargoPendingActivation.Count,
            Eggs.Count);

    private int CountProductionActiveEnemyUnits() =>
        flippers.Count + tankers.Count + spikers.Count + shotReleasedCargoPendingActivation.Count;

    private Enemy CreateTankerWaveEnemy(Egg egg)
    {
        if (egg.CargoUnitId >= 0)
        {
            // Released cargo returns as separate fresh Flipper entrants, not as a new Tanker.
            return Flipper.CreateFreshTankerCargo(egg.EdgeIndex, egg.CargoUnitId);
        }

        if (egg.OriginalSlotId == CurrentLevel.TankerSlotId && unresolvedOriginalSlots.Contains(egg.OriginalSlotId))
        {
            // In the isolated Tanker scenario, the enemy-bullet flag explicitly arms Tankers;
            // their Flipper cargo remains outside the production firing phase.
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
        // Development-only composition: all ten ordinary units hatch directly as Fuseballs.
        // Keep the existing Egg release cadence and ledger, without implying arcade progression.
        if (FuseballTest)
            return new Fuseball(lane);

        // All ordinary wave units hatch directly as Pulsars only in this isolated test mode.
        if (PulsarTest)
            return new Pulsar(lane, height: 0);

        if (CurrentLevel.SpikerDevelopmentWave && spikers.Count <= 3 &&
            flippers.Count + spikers.Count + Eggs.Count >= 4 && random.Next(-6, 2) <= 0)
        {
            // Fixed development speed avoids guessing BlitzMax's fractional Rand coercion.
            return new Spiker(lane, 3);
        }
        return CurrentLevel.Configuration.NormalEnemyComposition switch
        {
            LevelEnemyComposition.FlippersOnly => new Flipper(lane, 0),
            _ => throw new InvalidOperationException("The configured normal enemy composition is unsupported.")
        };
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
                case Fuseball fuseball when fuseball.IsHitBy(shot):
                    fuseballs.Remove(fuseball);
                    developmentCollisionOrder.Remove(fuseball);
                    CurrentLevel.RecordEnemyDestroyed();
                    // Incitti rewards deeper kills: 250 at the rim, 1250 only at height zero.
                    Score.Add(fuseball.KillScore);
                    fuseballScorePopups.Add(new FuseballScorePopup(
                        fuseball.EdgeIndex, fuseball.Height, fuseball.KillScore));
                    audioEvents.Play(AudioEvent.EnemyDestroyed);
                    return true;
                case Pulsar pulsar when pulsar.IsHitBy(shot):
                    pulsars.Remove(pulsar);
                    developmentCollisionOrder.Remove(pulsar);
                    CurrentLevel.RecordEnemyDestroyed();
                    Score.Add(200);
                    audioEvents.Play(AudioEvent.EnemyDestroyed);
                    return true;
                case Spike spike when spike.IsHitBy(shot):
                    HitSpike(spike);
                    return true;
                case Spiker spiker when spiker.IsHitBy(shot):
                    spikers.Remove(spiker);
                    developmentCollisionOrder.Remove(spiker);
                    if (CurrentLevel.UsesProductionLedger)
                        CurrentLevel.Level3Ledger!.MarkOriginalDestroyed(spiker.OriginalSlotId);
                    else
                        CurrentLevel.RecordEnemyDestroyed();
                    Score.Add(50);
                    audioEvents.Play(AudioEvent.EnemyDestroyed);
                    return true;
                case EnemyBullet enemyBullet when enemyBullet.IsHitBy(shot):
                    enemyBullets.Remove(enemyBullet);
                    developmentCollisionOrder.Remove(enemyBullet);
                    Score.Add(25);
                    return true;
                case Tanker tanker when tanker.IsHitBy(shot):
                    tankers.Remove(tanker);
                    developmentCollisionOrder.Remove(tanker);
                    if (!CurrentLevel.UsesProductionLedger)
                    {
                        ResolveOriginalSlotDestroyed(tanker.OriginalSlotId);
                        Score.Add(200);
                    }
                    ReleaseTankerCargo(tanker, TankerReleaseKind.PlayerShotDestruction);
                    audioEvents.Play(AudioEvent.EnemyDestroyed);
                    return true;
                case Flipper flipper when flipper.IsHitBy(shot):
                    flippers.Remove(flipper);
                    developmentCollisionOrder.Remove(flipper);
                    if (CurrentLevel.UsesProductionLedger && flipper.IsTankerCargo)
                        CurrentLevel.Level3Ledger!.MarkCargoDestroyed(flipper.CargoUnitId);
                    else if (CurrentLevel.UsesProductionLedger)
                        CurrentLevel.Level3Ledger!.MarkOriginalDestroyed(flipper.OriginalSlotId);
                    else if (tankerTest)
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
                    audioEvents.Play(AudioEvent.EnemyDestroyed);
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
            if (spiker.AdvanceZapTimer())
            {
                spikers.RemoveAt(index--);
                developmentCollisionOrder.Remove(spiker);
                if (CurrentLevel.UsesProductionLedger)
                    CurrentLevel.Level3Ledger!.MarkOriginalDestroyed(spiker.OriginalSlotId);
                else
                    CurrentLevel.RecordEnemyDestroyed();
                Score.Add(50);
                audioEvents.Play(AudioEvent.ZapResolution);
                continue;
            }

            EnemyBullet? bullet = spiker.Update(CurrentLevel,
                () => random.NextDouble() * 120 > 88,
                enemyBulletsTest ||
                CurrentLevel.Configuration.NormalEnemyComposition ==
                    LevelEnemyComposition.FlippersTankersAndSpikers
                    ? NextRandomInclusive
                    : null);
            if (bullet is not null)
            {
                // Enemy bullets were already advanced this tick, so this one first moves next
                // tick. It remains independent of the Spiker's retreat/removal lifecycle.
                enemyBullets.Add(bullet);
                developmentCollisionOrder.Add(bullet);
                audioEvents.Play(AudioEvent.EnemyBullet);
            }

            if (spiker.State != SpikerState.ReturnedToReserve)
                continue;

            spikers.RemoveAt(index--);
            developmentCollisionOrder.Remove(spiker);
            CurrentLevel.ReturnOriginalToReserve(spiker.OriginalSlotId);
        }
    }

    private void UpdateFuseballs()
    {
        for (int index = 0; index < fuseballs.Count; index++)
        {
            Fuseball fuseball = fuseballs[index];
            if (fuseball.AdvanceZapTimer())
            {
                fuseballs.RemoveAt(index--);
                developmentCollisionOrder.Remove(fuseball);
                CurrentLevel.RecordEnemyDestroyed();
                int points = fuseball.KillScore;
                Score.Add(points);
                fuseballScorePopups.Add(new FuseballScorePopup(
                    fuseball.EdgeIndex, fuseball.Height, points));
                audioEvents.Play(AudioEvent.ZapResolution);
                continue;
            }

            fuseball.Update(CurrentLevel, Player.EdgeIndex, NextRandomInclusive, HurryUp);
        }
    }

    private void AdvancePulseClock()
    {
        pulsePhase++;
        if (pulsePhase > 180)
            pulsePhase = 0;

        bool wasPulsing = Pulsing;
        Pulsing = pulsePhase > 70 && pulsePhase < 110;
        if (!wasPulsing && Pulsing && pulsars.Count > 0)
            audioEvents.Play(AudioEvent.PulsarPulse);
        // Resetting after 180 makes the cycle 181 calls long; changing this to >=180 shortens it.
        float amplitude = MathF.Abs((pulsePhase % 90) - 45) / 4f;
        pulseZh = amplitude < 2 ? 0 : MathF.Min(amplitude, 12);
    }

    private void UpdatePulsars()
    {
        for (int index = 0; index < pulsars.Count; index++)
        {
            Pulsar pulsar = pulsars[index];
            if (pulsar.AdvanceZapTimer())
            {
                pulsars.RemoveAt(index--);
                developmentCollisionOrder.Remove(pulsar);
                CurrentLevel.RecordEnemyDestroyed();
                Score.Add(200);
                audioEvents.Play(AudioEvent.ZapResolution);
                continue;
            }

            pulsar.Update(CurrentLevel, Player.EdgeIndex, NextRandomInclusive, HurryUp);
        }
    }

    private void LatchHurryUp()
    {
        // Like Incitti's end-of-loop rimit decision, observe post-hatch, post-enemy-update state.
        // Only later updates see the latch; reaching the rim cannot undo same-attempt hurry-up.
        if (!HurryUp && (FuseballTest || PulsarTest) &&
            CurrentLevel.ReserveEggs == 0 && Eggs.Count == 0 &&
            (fuseballs.Any(enemy => enemy.Height < Level.Depth) ||
             pulsars.Any(enemy => enemy.Height < Level.Depth)))
            HurryUp = true;
    }

    private void CheckPulsarsAgainstPlayer()
    {
        if (State != LevelState.Playing)
            return;

        foreach (Pulsar pulsar in pulsars)
        {
            // This branch is lane-wide, not a point overlap: the source electrifies the Pulsar's
            // whole edge during the global pulse window.
            PulsarCollision collision = pulsar.CheckPlayerCollision(Player, Pulsing);
            if (collision == PulsarCollision.None)
                continue;

            if (collision == PulsarCollision.Rim)
                pulsar.BeginRimPlayerKill();
            audioEvents.Play(AudioEvent.PlayerKilledByEnemy);
            KillPlayer();
            return;
        }
    }

    private void CheckFuseballsAgainstPlayer()
    {
        if (State != LevelState.Playing)
            return;

        foreach (Fuseball fuseball in fuseballs)
        {
            if (!fuseball.HitsPlayer(Player))
                continue;

            fuseball.BeginPlayerKill();
            audioEvents.Play(AudioEvent.PlayerKilledByEnemy);
            KillPlayer();
            return;
        }
    }

    private void UpdateTankers()
    {
        // A Tanker that climbs past the release threshold escapes its original slot (no score)
        // and hands its cargo to the field.
        for (int index = 0; index < tankers.Count; index++)
        {
            Tanker tanker = tankers[index];
            if (tanker.AdvanceZapTimer())
            {
                tankers.RemoveAt(index--);
                developmentCollisionOrder.Remove(tanker);
                if (!CurrentLevel.UsesProductionLedger)
                {
                    ResolveOriginalSlotDestroyed(tanker.OriginalSlotId);
                    Score.Add(200);
                }
                ReleaseTankerCargo(tanker, TankerReleaseKind.SuperZapperDestruction);
                audioEvents.Play(AudioEvent.ZapResolution);
                continue;
            }

            TankerUpdateResult result = tanker.UpdateForDevelopment(
                enemyBulletsTest || CurrentLevel.UsesProductionLedger
                    ? NextRandomInclusive
                    : null, out EnemyBullet? bullet);
            if (bullet is not null)
            {
                // UpdateEnemyBullets already ran this tick, so a bullet created here first moves
                // next tick (same policy as Flipper bullets). It joins the insertion-ordered
                // development collision list at its creation point and lives independently of the
                // Tanker afterwards: destroying or releasing the Tanker never removes it.
                enemyBullets.Add(bullet);
                developmentCollisionOrder.Add(bullet);
                audioEvents.Play(AudioEvent.EnemyBullet);
            }

            if (result != TankerUpdateResult.NaturalCargoRelease)
                continue;

            tankers.RemoveAt(index--);
            developmentCollisionOrder.Remove(tanker);
            if (!CurrentLevel.UsesProductionLedger)
                ResolveOriginalSlotEscaped(tanker.OriginalSlotId);
            ReleaseTankerCargo(tanker, TankerReleaseKind.Natural);
        }
    }

    private void ReleaseTankerCargo(Tanker tanker, TankerReleaseKind releaseKind)
    {
        // Every release materializes fresh cargo IDs; only zap expiry gives them their own timers.
        // Children are added after this tick's Flipper update, so their first update is next tick.
        TankerReleaseResult result = tanker.ReleaseFlipperCargo(
            CurrentLevel, firstSideDirection: 1, releaseKind);

        if (CurrentLevel.UsesProductionLedger)
        {
            if (result.Cargo.Count != Tanker.CarriedFlipperCount)
                throw new InvalidOperationException(
                    $"Tanker {tanker.OriginalSlotId} did not release its complete cargo pair.");

            Level3WaveLedger ledger = CurrentLevel.Level3Ledger
                ?? throw new InvalidOperationException("Level 3 mixed wave has no identity ledger.");
            IReadOnlyList<int> cargoIds = releaseKind switch
            {
                TankerReleaseKind.Natural => ledger.MaterializeLiveCargo(tanker.OriginalSlotId),
                TankerReleaseKind.PlayerShotDestruction => ledger.QueueCargoForShotActivation(tanker.OriginalSlotId),
                TankerReleaseKind.SuperZapperDestruction => ledger.MaterializeStillbornCargo(tanker.OriginalSlotId),
                _ => throw new ArgumentOutOfRangeException(nameof(releaseKind))
            };

            if (cargoIds.Count != result.Cargo.Count)
                throw new InvalidOperationException("Tanker cargo identities do not match released cargo.");

            if (releaseKind == TankerReleaseKind.Natural)
                ledger.MarkOriginalEscaped(tanker.OriginalSlotId);
            else
            {
                ledger.MarkOriginalDestroyed(tanker.OriginalSlotId);
                Score.Add(200);
            }

            for (int index = 0; index < result.Cargo.Count; index++)
            {
                int cargoId = cargoIds[index];
                Flipper child = Flipper.CreateTankerCargo(
                    CurrentLevel, result.Cargo[index], cargoId);

                if (releaseKind == TankerReleaseKind.SuperZapperDestruction)
                    child.TryZap(8);

                if (releaseKind == TankerReleaseKind.PlayerShotDestruction)
                    shotReleasedCargoPendingActivation.Add(child);
                else
                {
                    flippers.Add(child);
                    developmentCollisionOrder.Add(child);
                }
            }
            return;
        }

        foreach (TankerCargoSpawn spawn in result.Cargo)
        {
            int cargoUnitId = nextCargoUnitId++;
            unresolvedCargoUnits.Add(cargoUnitId);
            Flipper flipper = Flipper.CreateTankerCargo(CurrentLevel, spawn, cargoUnitId);
            if (releaseKind == TankerReleaseKind.SuperZapperDestruction)
                // Carrier expiry transfers latent units to new unresolved IDs, each with its own timer.
                flipper.TryZap(8);
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

    private void ResolveOriginalSlotRimCleared(int originalSlotId)
    {
        if (originalSlotId < 0 || !unresolvedOriginalSlots.Remove(originalSlotId))
            return;
        rimClearedOriginalSlots.Add(originalSlotId);
    }

    private void ResolveCargoUnit(int cargoUnitId)
    {
        if (unresolvedCargoUnits.Remove(cargoUnitId))
            resolvedCargoUnits.Add(cargoUnitId);
    }

    private void ResolveCargoUnitRimCleared(int cargoUnitId)
    {
        if (unresolvedCargoUnits.Remove(cargoUnitId))
            rimClearedCargoUnits.Add(cargoUnitId);
    }

    private void UpdateFlippers()
    {
        for (int index = 0; index < flippers.Count; index++)
        {
            Flipper flipper = flippers[index];
            if (flipper.AdvanceZapTimer())
            {
                flippers.RemoveAt(index--);
                developmentCollisionOrder.Remove(flipper);
                if (CurrentLevel.UsesProductionLedger)
                {
                    if (flipper.IsTankerCargo)
                        CurrentLevel.Level3Ledger!.MarkCargoDestroyed(flipper.CargoUnitId);
                    else
                        CurrentLevel.Level3Ledger!.MarkOriginalDestroyed(flipper.OriginalSlotId);
                }
                else if (tankerTest)
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
                audioEvents.Play(AudioEvent.ZapResolution);
                continue;
            }

            flipper.Update(CurrentLevel, Player.EdgeIndex,
                FlipperBulletFiringEnabled || CurrentLevel.Configuration.FlipperCanFlip
                    ? NextRandomInclusive
                    : null);
        }
    }

    private void ActivateShotReleasedCargo()
    {
        foreach (Flipper flipper in shotReleasedCargoPendingActivation)
        {
            if (CurrentLevel.UsesProductionLedger)
                CurrentLevel.Level3Ledger!.ActivateShotCargo(flipper.CargoUnitId);
            flippers.Add(flipper);
            developmentCollisionOrder.Add(flipper);
        }
        shotReleasedCargoPendingActivation.Clear();
    }

    private int NextRandomInclusive(int minimum, int maximum) =>
        random.Next(minimum, maximum + 1);

    private void UpdateFlipperFiring()
    {
        if (!FlipperBulletFiringEnabled)
            return;

        LevelConfiguration configuration = CurrentLevel.Configuration;
        int shotHoldoff = configuration.FlipperShotHoldoffTicks;
        int shotCapacity = configuration.MaximumActiveEnemyShots;
        foreach (Flipper flipper in flippers)
        {
            if (flipper.Height > MaximumFlipperFiringHeight)
                continue;

            // ROM decrements an eligible enemy's delay before checking its between-segments flag.
            if (!flipper.AdvanceShotDelay() || flipper.State == FlipperState.Flipping)
                continue;

            int activeShots = enemyBullets.Count;
            int threshold = EnemyShotPressureThresholds[
                Math.Min(activeShots, EnemyShotPressureThresholds.Length - 1)];
            if (random.Next(0, 256) < threshold)
                continue;

            // The arcade tests probability before looking for a free shot slot; rejection stays ready.
            if (activeShots >= shotCapacity)
                continue;

            EnemyBullet bullet = new(flipper.EdgeIndex, flipper.Height);
            enemyBullets.Add(bullet);
            developmentCollisionOrder.Add(bullet);
            flipper.ReloadShotDelay(shotHoldoff);
            audioEvents.Play(AudioEvent.EnemyBullet);
        }
    }

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
            audioEvents.Play(AudioEvent.PlayerKilledByBullet);
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
            audioEvents.Play(AudioEvent.PlayerKilledByEnemy);
            KillPlayer();
            return;
        }
    }

    private bool CheckForLevelComplete()
    {
        if (CurrentLevel.UsesProductionLedger)
        {
            if (State != LevelState.Playing)
                return false;

            Level3WaveLedger ledger = CurrentLevel.Level3Ledger
                ?? throw new InvalidOperationException("Level 3 mixed wave has no identity ledger.");
            bool mixedComplete = ledger.UnresolvedOriginalIds.Count == 0 &&
                ledger.UnresolvedCargoIds.Count == 0 &&
                CurrentLevel.ReserveEggs == 0 &&
                CurrentLevel.Level3CargoReserveEggs == 0 &&
                Eggs.Count == 0 &&
                tankers.Count == 0 &&
                flippers.Count == 0 &&
                shotReleasedCargoPendingActivation.Count == 0 &&
                spikers.Count == 0 &&
                fuseballs.Count == 0 &&
                pulsars.Count == 0;
            if (!mixedComplete)
                return false;

            ValidateLevel3WaveRepresentation();
            BeginExit();
            return true;
        }
        // Normal Level 1 uses the conservation check in Level.IsComplete. The Tanker scenario
        // has a dynamic ledger (cargo adds units), so it requires every slot and cargo unit resolved.
        if (State != LevelState.Playing)
            return false;

        bool complete = tankerTest
            ? unresolvedOriginalSlots.Count == 0 && unresolvedCargoUnits.Count == 0 &&
              CurrentLevel.ReserveEggs == 0 && Eggs.Count == 0 && tankers.Count == 0 &&
              flippers.Count == 0 && spikers.Count == 0
            : CurrentLevel.IsComplete(flippers.Count + spikers.Count + fuseballs.Count + pulsars.Count);
        if (!complete)
            return false;

        BeginExit();
        return true;
    }

    private bool TryCompleteAllRim()
    {
        if (CurrentLevel.UsesProductionLedger)
            return TryCompleteLevel3AllRim();

        if (!IsAllRimCompletionEligible(out List<Enemy> activeEnemies))
            return false;

        // Removing the enemy object also discards its pending zap timer, so it cannot earn a
        // delayed kill score after disappearing through the non-kill rim-clear path.
        foreach (Enemy enemy in activeEnemies)
        {
            developmentCollisionOrder.Remove(enemy);
            switch (enemy)
            {
                case Flipper flipper:
                    flippers.Remove(flipper);
                    if (tankerTest)
                    {
                        if (flipper.IsTankerCargo)
                            ResolveCargoUnitRimCleared(flipper.CargoUnitId);
                        else
                            ResolveOriginalSlotRimCleared(flipper.OriginalSlotId);
                    }
                    else
                    {
                        // Rim disappearance resolves a unit without claiming it was kill-scored.
                        CurrentLevel.RecordEnemyRimCleared();
                    }
                    break;
                case Tanker tanker:
                    tankers.Remove(tanker);
                    ResolveOriginalSlotRimCleared(tanker.OriginalSlotId);
                    break;
                case Spiker spiker:
                    spikers.Remove(spiker);
                    CurrentLevel.RecordEnemyRimCleared();
                    break;
                case Fuseball fuseball:
                    fuseballs.Remove(fuseball);
                    CurrentLevel.RecordEnemyRimCleared();
                    break;
                case Pulsar pulsar:
                    pulsars.Remove(pulsar);
                    CurrentLevel.RecordEnemyRimCleared();
                    break;
            }
        }

        BeginExit();
        return true;
    }

    private void ValidateLevel3WaveRepresentation()
    {
        Level3WaveLedger ledger = CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException("Level 3 mixed wave has no identity ledger.");
        if (tankers.Count > 6)
            throw new InvalidOperationException("A production mixed wave cannot have more than six active Tankers.");

        List<Level3WaveRepresentation> represented = [];

        represented.AddRange(ledger.OriginalIdsInReserve()
            .Select(id => new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Original, id),
                Level3RepresentationLocation.Reserve)));
        represented.AddRange(ledger.CargoIdsInReentryReserve()
            .Select(id => new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Cargo, id),
                Level3RepresentationLocation.Reserve)));

        foreach (Egg egg in Eggs)
        {
            if (egg.OriginalSlotId >= 0 && egg.CargoUnitId >= 0)
                throw new InvalidOperationException("A Level 3 Egg cannot hold both identity namespaces.");
            if (egg.OriginalSlotId >= 0)
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Original, egg.OriginalSlotId),
                    Level3RepresentationLocation.Egg));
            else if (egg.CargoUnitId >= 0)
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Cargo, egg.CargoUnitId),
                    Level3RepresentationLocation.Egg));
            else
                throw new InvalidOperationException("A production Level 3 Egg has no stable identity.");
        }

        foreach (Flipper flipper in flippers)
        {
            if (flipper.IsTankerCargo)
            {
                if (flipper.OriginalSlotId >= 0)
                    throw new InvalidOperationException("A cargo Flipper cannot also represent an original.");
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Cargo, flipper.CargoUnitId),
                    Level3RepresentationLocation.ActiveEnemy));
            }
            else
            {
                ledger.ValidateOriginalRuntimeType(
                    flipper.OriginalSlotId, Level3OriginalType.Flipper);
                represented.Add(new Level3WaveRepresentation(
                    new Level3Identity(Level3IdentityKind.Original, flipper.OriginalSlotId),
                    Level3RepresentationLocation.ActiveEnemy));
            }
        }

        foreach (Tanker tanker in tankers)
        {
            ledger.ValidateOriginalRuntimeType(
                tanker.OriginalSlotId, Level3OriginalType.Tanker);
            int[] ownedCargo = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
            if (tanker.LatentCargoCount != Tanker.CarriedFlipperCount ||
                tanker.CargoReleased ||
                ownedCargo.Length != Tanker.CarriedFlipperCount ||
                ownedCargo.Any(id => ledger.GetCargoState(id) != Level3CargoState.Latent))
                throw new InvalidOperationException(
                    $"Active Tanker {tanker.OriginalSlotId} does not own its unresolved latent cargo pair.");
            represented.Add(new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Original, tanker.OriginalSlotId),
                Level3RepresentationLocation.ActiveEnemy));
        }

        foreach (Spiker spiker in spikers)
        {
            ledger.ValidateOriginalRuntimeType(
                spiker.OriginalSlotId, Level3OriginalType.Spiker);
            represented.Add(new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Original, spiker.OriginalSlotId),
                Level3RepresentationLocation.ActiveEnemy));
        }

        foreach (Flipper pending in shotReleasedCargoPendingActivation)
        {
            if (!pending.IsTankerCargo || pending.OriginalSlotId >= 0)
                throw new InvalidOperationException("Pending shot activation must represent cargo only.");
            represented.Add(new Level3WaveRepresentation(
                new Level3Identity(Level3IdentityKind.Cargo, pending.CargoUnitId),
                Level3RepresentationLocation.PendingActivation));
        }

        ledger.ValidateRuntimeRepresentations(represented);

        HashSet<Enemy> expectedTargets = [.. flippers, .. tankers, .. spikers, .. enemyBullets];
        List<Enemy> orderedTargets = developmentCollisionOrder.OfType<Enemy>().ToList();
        if (orderedTargets.Count != expectedTargets.Count ||
            orderedTargets.Distinct().Count() != orderedTargets.Count ||
            !expectedTargets.SetEquals(orderedTargets))
            throw new InvalidOperationException(
                "Production runtime enemies and insertion-ordered targets do not agree.");
    }

    private bool TryCompleteLevel3AllRim()
    {
        Level3WaveLedger ledger = CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException("Level 3 mixed wave has no identity ledger.");
        if (State != LevelState.Playing ||
            CurrentLevel.ReserveEggs != 0 ||
            CurrentLevel.Level3CargoReserveEggs != 0 ||
            Eggs.Count != 0 ||
            shotReleasedCargoPendingActivation.Count != 0 ||
            ledger.HasLatentCargo ||
            ledger.HasPendingShotActivation)
            return false;

        List<Enemy> active = [.. flippers, .. tankers, .. spikers];
        if (active.Count == 0 || active.Any(enemy => !(enemy.Height >= Level.Depth)))
            return false;

        HashSet<int> representedOriginals = [];
        HashSet<int> representedCargo = [];
        foreach (Enemy enemy in active)
        {
            bool added = enemy switch
            {
                Tanker tanker => representedOriginals.Add(tanker.OriginalSlotId),
                Spiker spiker => representedOriginals.Add(spiker.OriginalSlotId),
                Flipper flipper when flipper.IsTankerCargo =>
                    representedCargo.Add(flipper.CargoUnitId),
                Flipper flipper => representedOriginals.Add(flipper.OriginalSlotId),
                _ => false
            };
            if (!added)
                throw new InvalidOperationException("A Level 3 all-rim identity is missing or duplicated.");
        }

        if (!ledger.UnresolvedOriginalIds.SetEquals(representedOriginals) ||
            !ledger.UnresolvedCargoIds.SetEquals(representedCargo))
            throw new InvalidOperationException(
                "Level 3 all-rim represented identities do not match the unresolved ledger.");
        ValidateLevel3WaveRepresentation();

        foreach (Enemy enemy in active)
        {
            developmentCollisionOrder.Remove(enemy);
            switch (enemy)
            {
                case Tanker tanker:
                    tankers.Remove(tanker);
                    ledger.MarkOriginalRimCleared(tanker.OriginalSlotId);
                    break;
                case Spiker spiker:
                    spikers.Remove(spiker);
                    ledger.MarkOriginalRimCleared(spiker.OriginalSlotId);
                    break;
                case Flipper flipper when flipper.IsTankerCargo:
                    flippers.Remove(flipper);
                    ledger.MarkCargoRimCleared(flipper.CargoUnitId);
                    break;
                case Flipper flipper:
                    flippers.Remove(flipper);
                    ledger.MarkOriginalRimCleared(flipper.OriginalSlotId);
                    break;
            }
        }

        BeginExit();
        return true;
    }

    private bool IsAllRimCompletionEligible(out List<Enemy> activeEnemies)
    {
        activeEnemies = [];
        if (State != LevelState.Playing ||
            CurrentLevel.ReserveEggs != 0 ||
            Eggs.Count != 0 ||
            CurrentLevel.PendingTankerCargoEggs != 0)
            return false;

        activeEnemies.AddRange(flippers);
        activeEnemies.AddRange(tankers);
        activeEnemies.AddRange(spikers);
        activeEnemies.AddRange(fuseballs);
        activeEnemies.AddRange(pulsars);

        if (activeEnemies.Count == 0 || activeEnemies.Any(enemy => !(enemy.Height >= Level.Depth)))
            return false;

        if (tankerTest)
            return TankerUnresolvedUnitsAreRepresented(activeEnemies);

        return CurrentLevel.RemainingEnemies == activeEnemies.Count;
    }

    private bool TankerUnresolvedUnitsAreRepresented(IEnumerable<Enemy> activeEnemies)
    {
        // Distinct identities prevent a carrier/cargo disappearance from silently dropping a
        // ledger unit; latent and queued cargo remain blockers until represented by active enemies.
        if (TankerLatentCargoUnits != 0 || PendingTankerCargoUnits != 0)
            return false;

        HashSet<int> representedOriginalSlots = [];
        HashSet<int> representedCargoUnits = [];
        foreach (Enemy enemy in activeEnemies)
        {
            if (enemy is Tanker tanker)
            {
                if (!representedOriginalSlots.Add(tanker.OriginalSlotId))
                    return false;
            }
            else if (enemy is Flipper flipper && flipper.IsTankerCargo)
            {
                if (!representedCargoUnits.Add(flipper.CargoUnitId))
                    return false;
            }
            else if (enemy is Flipper originalFlipper)
            {
                if (!representedOriginalSlots.Add(originalFlipper.OriginalSlotId))
                    return false;
            }
            else
            {
                return false;
            }
        }

        return unresolvedOriginalSlots.SetEquals(representedOriginalSlots) &&
            unresolvedCargoUnits.SetEquals(representedCargoUnits);
    }

    private void BeginExit()
    {
        CurrentLevel.State = CurrentLevel.SpikerDevelopmentWave ||
            CurrentLevel.Configuration.ExitPolicy == LevelExitPolicy.AvoidSpikes
            ? LevelState.AvoidSpikes
            : CurrentLevel.Configuration.ExitPolicy switch
            {
                LevelExitPolicy.Standard => LevelState.ExitDelay,
                _ => throw new InvalidOperationException("The configured Level exit policy is unsupported.")
            };
        exitDelayTicks = 0;
        if (CurrentLevel.Configuration.ExitPolicy != LevelExitPolicy.AvoidSpikes)
            shots.Clear();
        enemyBullets.Clear();
        developmentCollisionOrder.RemoveAll(target => target is EnemyBullet);
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
            CompleteSuccessfulExit();
    }

    private void CompleteSuccessfulExit()
    {
        // Wave clear only starts travel; production progression commits after successful travel.
        if (naturalProgressionEnabled &&
            LevelProgression.GetNextProductionLevel(CurrentLevel.Configuration) is LevelConfiguration next)
        {
            InitializeFreshWave(next, beginApproach: true);
            return;
        }

        CurrentLevel.State = LevelState.Complete;
        RestoreProjection();
    }

    private void UpdateSpikeExit(PlayerInput input)
    {
        bool firingEnabled = State == LevelState.ExitZoom ||
            CurrentLevel.Configuration.ExitPolicy != LevelExitPolicy.AvoidSpikes;
        bool exitTravel = State == LevelState.ExitZoom;
        if (exitTravel)
        {
            float previousPlayerDepth = Player.Depth;
            UpdateExitZoom(completeWhenFinished: false);
            CheckSpikesAgainstPlayer(previousPlayerDepth);
            if (State == LevelState.ReverseZoom)
                return;
        }

        // Avoidance/delay can still carry shots; during travel contact is resolved before shots.
        UpdatePlayerShots(spikesOnly: true);
        if (!exitTravel)
            CheckSpikesAgainstPlayer();
        if (State == LevelState.ReverseZoom)
            return;

        if (State == LevelState.ExitZoom && Player.Depth < 10)
        {
            shots.Clear();
            enemyBullets.Clear();
            CompleteSuccessfulExit();
            return;
        }

        UpdatePlayer(input, firingEnabled,
            exitTravel ? SpikeExitShotCooldownTicks : PlayerShotCooldownTicks);
        if (State == LevelState.AvoidSpikes)
        {
            RestoreProjection();
            Player.ResetDepth();
            if (++exitDelayTicks > CurrentLevel.Configuration.AvoidSpikesWaitThreshold)
            {
                CurrentLevel.State = LevelState.ExitDelay;
                exitDelayTicks = 0;
            }
        }
        else if (State == LevelState.ExitDelay && ++exitDelayTicks > 20)
        {
            CurrentLevel.State = LevelState.ExitZoom;
            exitDelayTicks = 0;
            audioEvents.Play(AudioEvent.ZoomOut);
        }
    }

    private void CheckSpikesAgainstPlayer(float? previousPlayerDepth = null)
    {
        // The configured spike-avoid exit and the Spiker diagnostic use the same persistent hazard.
        if (!SpikeExitEnabled ||
            DeathTicks > 0 ||
            State is not (LevelState.Playing or LevelState.AvoidSpikes or LevelState.ExitDelay or LevelState.ExitZoom))
            return;
        bool collided = Spikes.Any(spike =>
            spike.EdgeIndex == Player.EdgeIndex &&
            (spike.HitsPlayer(Player) ||
             previousPlayerDepth is float previousDepth &&
             previousDepth >= spike.Height &&
             Player.Depth <= spike.Height &&
             previousDepth > Player.Depth));
        if (!collided)
            return;

        DeathTicks = PlayerDeathTicks;
        audioEvents.Play(AudioEvent.PlayerKilledByEnemy);
        CurrentLevel.State = LevelState.ReverseZoom;
        exitDelayTicks = 0;
        enemyBullets.Clear();
        flippers.Clear();
        spikers.Clear();
        developmentCollisionOrder.RemoveAll(target => target is not Spike);
        // An all-rim clear is already a resolved disposition; reversing the spike exit must not
        // rebuild those enemies as Eggs when the exit is retried.
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
