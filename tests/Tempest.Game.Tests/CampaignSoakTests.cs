using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class CampaignSoakTests
{
    private const int CampaignSeed = 1981;
    private static readonly LevelConfiguration[] CampaignLevels =
    [
        LevelConfiguration.Level1,
        LevelConfiguration.Level2Square,
        LevelConfiguration.Level3Mixed,
        LevelConfiguration.Level4Candidate,
        LevelConfiguration.Level5Production,
        LevelConfiguration.Level6Production,
        LevelConfiguration.Level7Production,
        LevelConfiguration.Level8Production,
        LevelConfiguration.Level9Production
    ];

    [Fact]
    // This covers orchestration only, not combat/scoring, enemy distribution, natural Tanker
    // cargo release, Spiker combat, Spike shooting/avoidance, difficulty, rendering/audio,
    // responsiveness, arcade feel, or playability.
    public void ProductionCampaignReachesLevel9WithoutClaimingCombatValidation()
    {
        TempestGame game = new(random: new Random(CampaignSeed));
        int carriedScore = game.Score.Score;
        int carriedLives = game.Player.Lives;
        AssertFreshBoundary(game, CampaignLevels[0], "initial entry",
            LevelState.Playing, carriedScore, carriedLives, exitedLevel: null);

        for (int levelIndex = 0; levelIndex < CampaignLevels.Length - 1; levelIndex++)
        {
            LevelConfiguration outgoingConfiguration = CampaignLevels[levelIndex];
            LevelConfiguration incomingConfiguration = CampaignLevels[levelIndex + 1];
            ResolveFreshCampaignWaveForSoak(game);

            Level outgoingLevel = game.CurrentLevel;
            game.RunOneStep();
            LevelState expectedExitState =
                outgoingConfiguration.ExitPolicy == LevelExitPolicy.AvoidSpikes
                    ? LevelState.AvoidSpikes
                    : LevelState.ExitDelay;
            AssertBoundaryValue("completion exit state", expectedExitState, game.State,
                outgoingConfiguration, "completion detection", game);
            AssertCampaignState(game, outgoingConfiguration, "completion detection",
                carriedScore, carriedLives);

            if (outgoingConfiguration.ExitPolicy == LevelExitPolicy.AvoidSpikes)
                RunAvoidSpikesWait(game, outgoingConfiguration);

            RunExitDelay(game, outgoingConfiguration);
            RunOutwardTravel(game, outgoingLevel, outgoingConfiguration, incomingConfiguration);
            AssertFreshBoundary(game, incomingConfiguration, "approach installation",
                LevelState.Approach, carriedScore, carriedLives,
                exitedLevel: outgoingConfiguration.DisplayedLevel);

            RunApproach(game, incomingConfiguration);
            AssertFreshBoundary(game, incomingConfiguration, "approach arrival",
                LevelState.Playing, carriedScore, carriedLives,
                exitedLevel: outgoingConfiguration.DisplayedLevel);
        }

        AssertFreshBoundary(game, LevelConfiguration.Level9Production, "soak endpoint",
            LevelState.Playing, carriedScore, carriedLives, exitedLevel: 8);
    }

    private static void ResolveFreshCampaignWaveForSoak(TempestGame game)
    {
        Level level = game.CurrentLevel;
        LevelConfiguration configuration = level.Configuration;
        if (game.State != LevelState.Playing)
            ThrowUnsupportedWave(game, "the wave is not Playing");

        bool counterWave = ReferenceEquals(configuration, LevelConfiguration.Level1) ||
            ReferenceEquals(configuration, LevelConfiguration.Level2Square);
        bool ledgerWave = ReferenceEquals(configuration, LevelConfiguration.Level3Mixed) ||
            ReferenceEquals(configuration, LevelConfiguration.Level4Candidate) ||
            ReferenceEquals(configuration, LevelConfiguration.Level5Production) ||
            ReferenceEquals(configuration, LevelConfiguration.Level6Production) ||
            ReferenceEquals(configuration, LevelConfiguration.Level7Production) ||
            ReferenceEquals(configuration, LevelConfiguration.Level8Production) ||
            ReferenceEquals(configuration, LevelConfiguration.Level9Production);
        if (!counterWave && !ledgerWave)
            ThrowUnsupportedWave(game, "only fresh production Levels 1-9 are supported");

        if (level.EnemiesDestroyed != 0 || level.EnemiesRimCleared != 0 ||
            level.EnemiesSpawned != configuration.InitialActiveEggCount ||
            level.Eggs.Count != configuration.InitialActiveEggCount ||
            level.ReserveEggs != configuration.EnemySupply - configuration.InitialActiveEggCount ||
            level.RemainingEnemies != configuration.EnemySupply ||
            level.Eggs.Any(egg => egg.State != EggState.Roaming) ||
            game.Flippers.Count != 0 || game.Tankers.Count != 0 ||
            game.Spikers.Count != 0 || game.Fuseballs.Count != 0 ||
            game.Pulsars.Count != 0 || game.Shots.Count != 0 ||
            game.EnemyBullets.Count != 0 || game.PendingTankerCargoUnits != 0 ||
            game.TankerLatentCargoUnits != 0 || game.TankerMaterializedCargoUnits != 0 ||
            game.TankerCargoUnitsRemaining != 0 ||
            game.CurrentApproach is not null || game.DeathTicks != 0 || game.HurryUp ||
            game.Player.EdgeIndex != configuration.InitialPlayerLane ||
            game.Player.EdgePosition != configuration.InitialPlayerLanePosition ||
            game.Player.Depth != configuration.Depth ||
            game.SuperZapperChargesRemaining != SuperZapperSystem.InitialCharges ||
            game.SuperZapper.FlashTicksRemaining != 0 ||
            Projection.K != 70 || Projection.ZOffset != 5 ||
            game.FuseballScorePopups.Count != 0 ||
            (configuration.StartingSpikeHeight is null
                ? game.Spikes.Count != 0
                : game.Spikes.Count != configuration.PlayableLaneCount ||
                  game.Spikes.Any(spike => spike.Height != configuration.StartingSpikeHeight)))
            ThrowUnsupportedWave(game, "counter, Egg/reserve, or runtime representations are not fresh");

        if (counterWave)
        {
            if (level.UsesProductionLedger || level.Level3Ledger is not null)
                ThrowUnsupportedWave(game, "a counter-based wave unexpectedly has a production ledger");

            for (int obligation = 0; obligation < configuration.EnemySupply; obligation++)
                level.RecordEnemyDestroyed();
        }
        else
        {
            if (level.Level3Ledger is null)
                ThrowUnsupportedWave(game, "the fresh production ledger is missing");
            Level3WaveLedger ledger = level.Level3Ledger!;
            if (!level.UsesProductionLedger ||
                ledger.OriginalCount != configuration.EnemySupply ||
                ledger.UnresolvedOriginalIds.Count != configuration.EnemySupply ||
                ledger.CargoCount != 0 || ledger.UnresolvedCargoIds.Count != 0 ||
                ledger.HasLatentCargo || ledger.HasPendingShotActivation ||
                ledger.OriginalIdsInReserve().Count != level.ReserveEggs ||
                Enumerable.Range(0, ledger.OriginalCount).Any(id =>
                    ledger.GetOriginalType(id) != Level3OriginalType.Unselected ||
                    ledger.GetOriginalState(id) is not
                        (Level3OriginalState.Reserve or Level3OriginalState.Egg)))
                ThrowUnsupportedWave(game,
                    "the production ledger is selected, materialized, or partially played");

            foreach (int id in ledger.UnresolvedOriginalIds.Order().ToArray())
            {
                if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                    ledger.MarkOriginalEgg(id);
                ledger.CommitOriginalAsFlipper(id);
                ledger.MarkOriginalDestroyed(id);
            }
        }

        level.ResetAfterDeath();
    }

    private static void ThrowUnsupportedWave(TempestGame game, string reason)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger? ledger = level.Level3Ledger;
        throw new InvalidOperationException(
            $"TEST-ONLY campaign wave resolution refused Level {level.Configuration.DisplayedLevel}: " +
            $"{reason}; state={game.State}, destroyed={level.EnemiesDestroyed}, " +
            $"rim-cleared={level.EnemiesRimCleared}, unresolved originals=" +
            $"{ledger?.UnresolvedOriginalIds.Count ?? 0}, unresolved cargo=" +
            $"{ledger?.UnresolvedCargoIds.Count ?? 0}, counter remaining={level.RemainingEnemies}, " +
            $"Eggs={level.Eggs.Count}, reserve={level.ReserveEggs}.");
    }

    private static void RunAvoidSpikesWait(
        TempestGame game, LevelConfiguration configuration)
    {
        Spike[] exitLaneSpikes = game.Spikes
            .Where(spike => spike.EdgeIndex == game.Player.EdgeIndex)
            .ToArray();
        AssertBoundaryValue("stationary exit-lane Spike count", 1, exitLaneSpikes.Length,
            configuration, "TEST-ONLY safe exit setup", game);
        Spike inertExitLaneSpike = exitLaneSpikes[0];
        inertExitLaneSpike.Height = 0;

        AssertBoundaryValue("inert exit-lane Spike height", 0, inertExitLaneSpike.Height,
            configuration, "TEST-ONLY safe exit setup", game);
        AssertBoundaryValue("other Spikes retained", configuration.PlayableLaneCount - 1,
            game.Spikes.Count(spike => spike.EdgeIndex != inertExitLaneSpike.EdgeIndex),
            configuration, "TEST-ONLY safe exit setup", game);

        for (int tick = 0; tick <= configuration.AvoidSpikesWaitThreshold; tick++)
        {
            game.RunOneStep();
            LevelState expected = tick < configuration.AvoidSpikesWaitThreshold
                ? LevelState.AvoidSpikes
                : LevelState.ExitDelay;
            AssertBoundaryValue("Avoid Spikes wait state", expected, game.State,
                configuration, "Avoid Spikes waiting period", game);
        }

        AssertBoundaryValue("other Spike starting heights", true,
            game.Spikes.Where(spike => spike.EdgeIndex != inertExitLaneSpike.EdgeIndex)
                .All(spike => spike.Height == configuration.StartingSpikeHeight),
            configuration, "Avoid Spikes waiting period", game);
    }

    private static void RunExitDelay(TempestGame game, LevelConfiguration configuration)
    {
        Spike? exitLaneSpike = game.Spikes.SingleOrDefault(
            spike => spike.EdgeIndex == game.Player.EdgeIndex);
        if (exitLaneSpike is not null)
            exitLaneSpike.Height = 0;

        for (int tick = 0; tick < 21; tick++)
        {
            game.RunOneStep();
            LevelState expected = tick < 20 ? LevelState.ExitDelay : LevelState.ExitZoom;
            AssertBoundaryValue("exit delay state", expected, game.State,
                configuration, "exit delay", game);
        }
    }

    private static void RunOutwardTravel(
        TempestGame game,
        Level outgoingLevel,
        LevelConfiguration outgoingConfiguration,
        LevelConfiguration incomingConfiguration)
    {
        int travelTicks = 0;
        while (ReferenceEquals(outgoingLevel, game.CurrentLevel) && travelTicks < 250)
        {
            AssertBoundaryValue("outward travel state", LevelState.ExitZoom, game.State,
                outgoingConfiguration, "outward tube travel", game);
            game.RunOneStep();
            travelTicks++;
        }

        AssertBoundaryValue("outward travel completed", false,
            ReferenceEquals(outgoingLevel, game.CurrentLevel),
            outgoingConfiguration, "outward tube travel", game);
        AssertBoundaryValue("outward travel tick count is positive", true, travelTicks > 0,
            outgoingConfiguration, "outward tube travel", game);
        AssertBoundaryValue("incoming production configuration", true,
            ReferenceEquals(incomingConfiguration, game.CurrentLevel.Configuration),
            incomingConfiguration, "next-level installation", game);
        AssertBoundaryValue("new level approach state", LevelState.Approach, game.State,
            incomingConfiguration, "next-level installation", game);
    }

    private static void RunApproach(TempestGame game, LevelConfiguration configuration)
    {
        AssertBoundaryValue("approach model present", true, game.CurrentApproach is not null,
            configuration, "between-level approach", game);
        LevelApproach approach = game.CurrentApproach!;
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
        {
            game.RunOneStep();
            LevelState expected = tick < LevelApproach.DurationTicks - 1
                ? LevelState.Approach
                : LevelState.Playing;
            AssertBoundaryValue("real approach state", expected, game.State,
                configuration, "between-level approach", game);
            if (expected == LevelState.Approach)
                AssertBoundaryValue("approach elapsed ticks", tick + 1,
                    game.CurrentApproach?.ElapsedTicks ?? -1,
                    configuration, "between-level approach", game);
        }

        AssertBoundaryValue("approach model cleared after arrival", true,
            game.CurrentApproach is null, configuration, "between-level approach", game);
        AssertBoundaryValue("approach exited-level identity",
            configuration.DisplayedLevel - 1, approach.ExitedLevel,
            configuration, "between-level approach", game);
    }

    private static void AssertFreshBoundary(
        TempestGame game,
        LevelConfiguration configuration,
        string phase,
        LevelState expectedState,
        int expectedScore,
        int expectedLives,
        int? exitedLevel)
    {
        Level level = game.CurrentLevel;
        AssertBoundaryValue("displayed level identity", configuration.DisplayedLevel,
            level.Configuration.DisplayedLevel, configuration, phase, game);
        AssertBoundaryValue("production configuration identity", true,
            ReferenceEquals(configuration, level.Configuration), configuration, phase, game);
        AssertBoundaryValue("progression identity", configuration.ProgressionIndex,
            level.Configuration.ProgressionIndex, configuration, phase, game);
        AssertBoundaryValue("board identity", configuration.BoardIndex,
            level.Configuration.BoardIndex, configuration, phase, game);
        AssertBoundaryValue("board geometry identity", true,
            ReferenceEquals(configuration.Definition, level.Definition), configuration, phase, game);
        AssertBoundaryValue("playable geometry lane count", configuration.PlayableLaneCount,
            level.PlayableLaneCount, configuration, phase, game);
        AssertBoundaryValue("palette identity", configuration.PaletteIndex,
            level.Configuration.PaletteIndex, configuration, phase, game);
        AssertBoundaryValue("configured enemy supply", configuration.EnemySupply,
            level.EnemySupply, configuration, phase, game);
        AssertBoundaryValue("composition policy", configuration.NormalEnemyComposition,
            level.Configuration.NormalEnemyComposition, configuration, phase, game);
        AssertBoundaryValue("exit policy", configuration.ExitPolicy,
            level.Configuration.ExitPolicy, configuration, phase, game);
        AssertBoundaryValue("configured state", expectedState, game.State,
            configuration, phase, game);
        AssertBoundaryValue("initial Egg count", configuration.InitialActiveEggCount,
            level.Eggs.Count, configuration, phase, game);
        AssertBoundaryValue("initial reserve count",
            configuration.EnemySupply - configuration.InitialActiveEggCount,
            level.ReserveEggs, configuration, phase, game);
        AssertBoundaryValue("initial spawned count", configuration.InitialActiveEggCount,
            level.EnemiesSpawned, configuration, phase, game);
        AssertBoundaryValue("Eggs remain in fresh roaming state", true,
            level.Eggs.All(egg => egg.State == EggState.Roaming), configuration, phase, game);
        AssertBoundaryValue("zero destroyed counter", 0, level.EnemiesDestroyed,
            configuration, phase, game);
        AssertBoundaryValue("zero rim-cleared counter", 0, level.EnemiesRimCleared,
            configuration, phase, game);
        AssertBoundaryValue("fresh counter remainder", configuration.EnemySupply,
            level.RemainingEnemies, configuration, phase, game);

        bool requiresLedger = configuration.NormalEnemyComposition is
            LevelEnemyComposition.FlippersAndTankers or
            LevelEnemyComposition.FlippersTankersAndSpikers;
        AssertBoundaryValue("production ledger presence", requiresLedger,
            level.Level3Ledger is not null, configuration, phase, game);
        if (requiresLedger)
        {
            Level3WaveLedger ledger = level.Level3Ledger!;
            AssertBoundaryValue("original identity count", configuration.EnemySupply,
                ledger.OriginalCount, configuration, phase, game);
            AssertBoundaryValue("fresh unresolved original count", configuration.EnemySupply,
                ledger.UnresolvedOriginalIds.Count, configuration, phase, game);
            AssertBoundaryValue("fresh unresolved cargo count", 0,
                ledger.UnresolvedCargoIds.Count, configuration, phase, game);
            AssertBoundaryValue("fresh selected/materialized cargo count", 0,
                ledger.CargoCount, configuration, phase, game);
            AssertBoundaryValue("fresh originals are unselected", true,
                Enumerable.Range(0, ledger.OriginalCount).All(id =>
                    ledger.GetOriginalType(id) == Level3OriginalType.Unselected),
                configuration, phase, game);
            AssertBoundaryValue("fresh Egg original count", level.Eggs.Count,
                Enumerable.Range(0, ledger.OriginalCount).Count(id =>
                    ledger.GetOriginalState(id) == Level3OriginalState.Egg),
                configuration, phase, game);
            AssertBoundaryValue("fresh reserve original count", level.ReserveEggs,
                ledger.OriginalIdsInReserve().Count, configuration, phase, game);
            AssertBoundaryValue("no latent cargo", 0, game.TankerLatentCargoUnits,
                configuration, phase, game);
            AssertBoundaryValue("no materialized cargo", 0, game.TankerMaterializedCargoUnits,
                configuration, phase, game);
            AssertBoundaryValue("no pending cargo", 0, game.PendingTankerCargoUnits,
                configuration, phase, game);
            AssertBoundaryValue("no remaining cargo", 0, game.TankerCargoUnitsRemaining,
                configuration, phase, game);
        }

        AssertBoundaryValue("no stale Flippers", 0, game.Flippers.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale Tankers", 0, game.Tankers.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale Spikers", 0, game.Spikers.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale Fuseballs", 0, game.Fuseballs.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale Pulsars", 0, game.Pulsars.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale player shots", 0, game.Shots.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale enemy bullets", 0, game.EnemyBullets.Count,
            configuration, phase, game);
        AssertBoundaryValue("no stale pending cargo", 0, game.PendingTankerCargoUnits,
            configuration, phase, game);
        AssertBoundaryValue("no stale death state", 0, game.DeathTicks,
            configuration, phase, game);
        AssertBoundaryValue("hurry-up state reset", false, game.HurryUp,
            configuration, phase, game);
        AssertBoundaryValue("Blaster starting lane", configuration.InitialPlayerLane,
            game.Player.EdgeIndex, configuration, phase, game);
        AssertBoundaryValue("Blaster starting lane fraction", configuration.InitialPlayerLanePosition,
            game.Player.EdgePosition, configuration, phase, game);
        AssertBoundaryValue("Blaster starting depth", configuration.Depth,
            game.Player.Depth, configuration, phase, game);
        AssertBoundaryValue("campaign score carried unchanged", expectedScore,
            game.Score.Score, configuration, phase, game);
        AssertBoundaryValue("campaign lives carried unchanged", expectedLives,
            game.Player.Lives, configuration, phase, game);
        AssertBoundaryValue("two fresh Super Zapper charges", SuperZapperSystem.InitialCharges,
            game.SuperZapperChargesRemaining, configuration, phase, game);
        AssertBoundaryValue("cleared Zapper flash", 0, game.SuperZapper.FlashTicksRemaining,
            configuration, phase, game);
        AssertBoundaryValue("normal projection K", 70, Projection.K,
            configuration, phase, game);
        AssertBoundaryValue("normal projection Z offset", 5, Projection.ZOffset,
            configuration, phase, game);
        AssertBoundaryValue("not in exit state", false, game.IsExiting,
            configuration, phase, game);

        int expectedSpikeCount = configuration.StartingSpikeHeight is null
            ? 0
            : configuration.PlayableLaneCount;
        AssertBoundaryValue("configured Spike count", expectedSpikeCount, game.Spikes.Count,
            configuration, phase, game);
        for (int lane = 0; lane < configuration.PlayableLaneCount; lane++)
        {
            Spike[] laneSpikes = game.Spikes.Where(spike => spike.EdgeIndex == lane).ToArray();
            int expectedOnLane = configuration.StartingSpikeHeight is null ? 0 : 1;
            AssertBoundaryValue($"configured Spikes on lane {lane}", expectedOnLane,
                laneSpikes.Length, configuration, phase, game);
            if (configuration.StartingSpikeHeight is float startingHeight && laneSpikes.Length == 1)
                AssertBoundaryValue($"Spike starting height on lane {lane}", startingHeight,
                    laneSpikes[0].Height, configuration, phase, game);
        }

        if (expectedState == LevelState.Approach)
        {
            AssertBoundaryValue("approach model present", true,
                game.CurrentApproach is not null, configuration, phase, game);
            AssertBoundaryValue("approach exited-level identity", exitedLevel ?? -1,
                game.CurrentApproach?.ExitedLevel ?? -1, configuration, phase, game);
        }
        else
        {
            AssertBoundaryValue("no stale approach model", true,
                game.CurrentApproach is null, configuration, phase, game);
        }
    }

    private static void AssertCampaignState(
        TempestGame game,
        LevelConfiguration configuration,
        string phase,
        int expectedScore,
        int expectedLives)
    {
        AssertBoundaryValue("campaign score carried unchanged", expectedScore,
            game.Score.Score, configuration, phase, game);
        AssertBoundaryValue("campaign lives carried unchanged", expectedLives,
            game.Player.Lives, configuration, phase, game);
    }

    private static void AssertBoundaryValue<T>(
        string invariant,
        T expected,
        T actual,
        LevelConfiguration configuration,
        string phase,
        TempestGame game)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger? ledger = level.Level3Ledger;
        string details =
            $"Level {configuration.DisplayedLevel} {phase}: unresolved originals=" +
            $"{ledger?.UnresolvedOriginalIds.Count ?? 0}, unresolved cargo=" +
            $"{ledger?.UnresolvedCargoIds.Count ?? 0}, counter remaining={level.RemainingEnemies}, " +
            $"Eggs={level.Eggs.Count}, reserve={level.ReserveEggs}.";
        Assert.True(EqualityComparer<T>.Default.Equals(expected, actual),
            $"{details} {invariant}: expected {expected}, actual {actual}.");
    }
}
