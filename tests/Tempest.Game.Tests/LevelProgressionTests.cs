using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class LevelProgressionTests
{
    [Fact]
    public void NormalLaunchWaitsForSuccessfulExitAndInstallsLevel2OnlyOnce()
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Level outgoing = game.CurrentLevel;
        game.Score.Add(1500);
        game.Player.Lives = 2;
        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Step(game, 20);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);

        game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Step(game, 123);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);

        game.RunOneStep();

        AssertFreshLevel2(game, outgoing, expectedScore: 1500, expectedLives: 2);
        Assert.Equal(LevelState.Approach, game.State);
        FinishApproach(game);
        Level installed = game.CurrentLevel;
        Step(game, 50);
        Assert.Same(installed, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(1, game.CurrentLevel.Configuration.ProgressionIndex);
    }

    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 10)]
    [InlineData(8, 2)]
    public void KillAndRimResolutionCountsConvergeOnNaturalLevel2(int kills, int rimClears)
    {
        TempestGame game = CreateClearedLevel1(kills);
        game.Score.Add(kills * 150);
        if (rimClears > 0)
            CompleteRemainingUnitsAtRim(game, rimClears);
        else
            game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(kills, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(rimClears, game.CurrentLevel.EnemiesRimCleared);
        AssertFreshLevel2AfterExit(game, expectedScore: kills * 150, expectedLives: 3);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void FreshLevel2AlwaysRechargesZapperAndClearsFlash(int chargesBeforeExit)
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        for (int use = SuperZapperSystem.InitialCharges; use > chargesBeforeExit; use--)
            Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));

        Assert.Equal(chargesBeforeExit, game.SuperZapperChargesRemaining);
        if (chargesBeforeExit < SuperZapperSystem.InitialCharges)
            Assert.True(game.SuperZapper.FlashTicksRemaining > 0);

        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        FinishApproach(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void FreshTransitionDiscardsOldFlippersAndPendingZapTargets()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 40; tick++)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();
        Flipper outgoingFlipper = Assert.Single(game.Flippers);
        Assert.True(game.SuperZapper.TryActivate(game.Flippers, game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount, game.CurrentLevel.Continuous));
        Assert.True(outgoingFlipper.PendingZapTicks > 0);

        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Empty(game.Flippers);
        Assert.DoesNotContain(outgoingFlipper, game.Flippers);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void NaturalProgressionIsOrthogonalToLivesAndEnemyBulletOption(
        bool enemyBulletsTest, bool livesOverride)
    {
        TempestGame game = new(startingLives: livesOverride ? 10 : Player.DefaultStartingLives,
            enemyBulletsTest: enemyBulletsTest);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);

        game = CreateClearedLevel1(kills: 10, enemyBulletsTest: enemyBulletsTest,
            startingLives: livesOverride ? 10 : Player.DefaultStartingLives);
        game.Score.Add(91);
        game.Player.Lives--;
        AssertFreshLevel2AfterExit(game, expectedScore: 91,
            expectedLives: livesOverride ? 9 : Player.DefaultStartingLives - 1);
        Assert.True(game.EnemyBulletsTest == enemyBulletsTest);
    }

    [Fact]
    public void DevelopmentScenariosAreNotEligibleForProductionProgression()
    {
        TempestGame[] developmentGames =
        [
            new(level2Test: true),
            new(level3Test: true),
            new(level3GeometryTest: true),
            new(spikerDevelopmentWave: true),
            new(tankerTest: true),
            new(fuseballTest: true),
            new(pulsarTest: true)
        ];

        Assert.All(developmentGames, game => Assert.False(game.NaturalProgressionEnabled));
        Assert.All(developmentGames, game => Assert.Null(game.CurrentApproach));
        Assert.All(developmentGames, game => Assert.Equal(LevelState.Playing, game.State));
        Assert.Same(LevelConfiguration.Level2Square, developmentGames[0].CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level3Mixed, developmentGames[1].CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level3Plus, developmentGames[2].CurrentLevel.Configuration);
        Assert.All(developmentGames.Skip(3),
            game => Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration));
    }

    [Fact]
    public void LifeLossAndGameOverBeforeSuccessfulExitRemainOnLevel1()
    {
        TempestGame game = new(startingLives: 5);
        Level initialLevel = game.CurrentLevel;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(initialLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(4, game.Player.Lives);

        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep();
        Assert.True(game.GameOver);
        Assert.Same(initialLevel, game.CurrentLevel);

        game.Score.Add(125);
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    [Fact]
    public void NaturalLevel2DeathReconstructsTheSameConfigurationAndGameOverStartsLevel1()
    {
        TempestGame game = CreateClearedLevel1(kills: 10);
        game.Score.Add(400);
        AssertFreshLevel2AfterExit(game, expectedScore: 400, expectedLives: 3);
        Level level2 = game.CurrentLevel;
        game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true);
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(level2, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(400, game.Score.Score);
        Assert.Equal(1, game.SuperZapperChargesRemaining);

        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void NaturalLevel2KillAndAllRimCompletionAdvanceToLevel3(bool allRim)
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        game.Score.Add(650);
        game.Player.Lives = 2;
        AssertFreshLevel2AfterExit(game, expectedScore: 650, expectedLives: 2);

        if (allRim)
            CompleteRemainingUnitsAtRim(game, game.CurrentLevel.EnemySupply);
        else
        {
            for (int enemy = 0; enemy < game.CurrentLevel.EnemySupply; enemy++)
                game.CurrentLevel.RecordEnemyDestroyed();
            game.CurrentLevel.ResetAfterDeath();
            game.RunOneStep();
        }

        Assert.Equal(LevelState.ExitDelay, game.State);
        Level outgoing = game.CurrentLevel;
        game.Score.Add(125);
        game.Player.Lives = 1;
        FinishLevel2Exit(game);
        AssertFreshLevel3(game, outgoing, expectedScore: 775, expectedLives: 1);

        CompleteNaturalLevel3Wave(game);
        Level level3 = game.CurrentLevel;
        FinishLevel3Exit(game);

        Assert.NotSame(level3, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(775, game.Score.Score);
        Assert.Equal(1, game.Player.Lives);
        FinishApproach(game);
    }

    [Fact]
    public void NaturalLevel4InstallsTheAcceptedFreshConfigurationAndCarriesCampaignState()
    {
        TempestGame game = CreateNaturalLevel3(startingLives: 7);
        game.Score.Add(1775);
        game.Player.Lives = 3;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        Assert.True(game.SuperZapper.FlashTicksRemaining > 0);

        CompleteNaturalLevel3Wave(game);
        Level outgoing = game.CurrentLevel;
        FinishLevel3Exit(game);

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Same(LevelConfiguration.Level4Candidate, incoming.Configuration);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.False(game.Level4Test);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(4, incoming.Configuration.DisplayedLevel);
        Assert.Equal(3, incoming.Configuration.ProgressionIndex);
        Assert.Equal(3, incoming.Configuration.BoardIndex);
        Assert.Equal(0, incoming.Configuration.PaletteIndex);
        Assert.True(incoming.Definition.Continuous);
        Assert.Same(LevelConfiguration.Level4Candidate.Definition, incoming.Definition);
        Assert.Equal(16, incoming.PlayableLaneCount);
        Assert.Equal(17, incoming.EnemySupply);
        Assert.Equal(17, incoming.Level3Ledger!.OriginalCount);
        Assert.Equal(0, incoming.Level3Ledger.CargoCount);
        Assert.Equal(17, incoming.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(13, incoming.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            incoming.Configuration.NormalEnemyComposition);
        Assert.Equal(LevelExitPolicy.AvoidSpikes, incoming.Configuration.ExitPolicy);
        Assert.Equal(1775, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Empty(outgoing.Level3Ledger!.UnresolvedOriginalIds);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);

        FinishApproach(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Equal(1775, game.Score.Score);
        Assert.Equal(3, game.Player.Lives);
    }

    [Fact]
    public void NaturalLevel4DeathReconstructsInPlaceAndGameOverRestartsLevel1()
    {
        TempestGame game = CreateNaturalLevel4(startingLives: 5);
        FinishApproach(game);
        Level level = game.CurrentLevel;
        Spike[] spikes = game.Spikes.ToArray();
        int[] unresolvedBeforeDeath = level.Level3Ledger!.UnresolvedOriginalIds.Order().ToArray();
        game.Score.Add(425);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        int chargesBeforeDeath = game.SuperZapperChargesRemaining;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.Player.Lives);
        Assert.Equal(425, game.Score.Score);
        Assert.Equal(chargesBeforeDeath, game.SuperZapperChargesRemaining);
        Assert.Equal(unresolvedBeforeDeath, level.Level3Ledger!.UnresolvedOriginalIds.Order());
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(spikes, game.Spikes);

        game.Player.Lives = 1;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();
        FinishGameOverPresentation(game);
        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Assert.Equal(425, game.Score.Score);
        Assert.Equal(spikes, game.Spikes);

        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Level4AvoidSpikesExitInstallsLevel5FromNaturalOrDirectStart(bool directStart)
    {
        TempestGame game = directStart
            ? new TempestGame(startingProductionLevel: 4)
            : CreateNaturalLevel4();
        Assert.True(game.NaturalProgressionEnabled);
        if (game.State == LevelState.Approach)
            FinishApproach(game);
        game.Score.Add(88);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        CompleteLevel4Wave(game);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(16, game.Spikes.Count);
        Assert.True(game.ShouldShowAvoidSpikesWarning);

        float initialPosition = game.Player.EdgePosition;
        int firingLane = game.Player.EdgeIndex;
        Spike pathSpike = game.Spikes.Single(spike => spike.EdgeIndex == firingLane);
        float initialSpikeHeight = pathSpike.Height;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: true));
        Assert.True(game.Player.EdgePosition > initialPosition);
        Assert.Empty(game.Shots);
        Assert.Equal(initialSpikeHeight, pathSpike.Height);
        Assert.True(game.ShouldShowAvoidSpikesWarning);

        for (int tick = 0; tick < 100 && game.Spikes.Contains(pathSpike); tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Assert.Contains(pathSpike, game.Spikes);
        Assert.Equal(16, game.Spikes.Count);
        Assert.Empty(game.Shots);

        for (int tick = 0; tick < 151 && game.State == LevelState.AvoidSpikes; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.True(game.ShouldShowAvoidSpikesWarning);

        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.False(game.ShouldShowAvoidSpikesWarning);

        pathSpike = game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex);
        for (int tick = 0; tick < 100 && game.Spikes.Contains(pathSpike); tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Assert.DoesNotContain(pathSpike, game.Spikes);
        Assert.Equal(15, game.Spikes.Count);

        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Same(LevelConfiguration.Level5Production, incoming.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(4, game.CurrentApproach!.ExitedLevel);
        Assert.Equal(5, incoming.Configuration.DisplayedLevel);
        Assert.Equal(4, incoming.Configuration.ProgressionIndex);
        Assert.Equal(4, incoming.Configuration.BoardIndex);
        Assert.Equal(0, incoming.Configuration.PaletteIndex);
        Assert.Equal(20, incoming.EnemySupply);
        Assert.Equal(20, incoming.Level3Ledger!.OriginalCount);
        Assert.Equal(20, incoming.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, incoming.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(94, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);

        List<Color> transitionColors = [];
        TransitionStarfield.Draw(game.CurrentApproach.ExitedLevel, 1, (_, color) =>
            transitionColors.Add(color));
        Assert.NotEmpty(transitionColors);
        Assert.All(transitionColors, color =>
        {
            Assert.Equal(0, color.R);
            Assert.Equal(194, color.G);
            Assert.Equal(255, color.B);
        });

        FinishApproach(game);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(incoming, game.CurrentLevel);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Level5AvoidSpikesExitInstallsLevel6FromNaturalOrDirectStart(bool directStart)
    {
        TempestGame game = directStart
            ? new TempestGame(startingProductionLevel: 5, startingLives: 5)
            : CreateNaturalLevel5(startingLives: 5);
        Assert.True(game.NaturalProgressionEnabled);
        game.Score.Add(88);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        Assert.True(game.SuperZapper.FlashTicksRemaining > 0);

        CompleteLevel5Wave(game);
        Level outgoing = game.CurrentLevel;
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Same(LevelConfiguration.Level5Production, outgoing.Configuration);
        Assert.Same(LevelConfiguration.Level6Production,
            LevelProgression.GetNextProductionLevel(outgoing.Configuration));
        Assert.Empty(outgoing.Level3Ledger!.UnresolvedOriginalIds);

        for (int tick = 0; tick < 150; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);

        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Same(LevelConfiguration.Level6Production, incoming.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(5, game.CurrentApproach!.ExitedLevel);
        Assert.Equal(22, incoming.Level3Ledger!.OriginalCount);
        Assert.Equal(22, incoming.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(incoming.Level3Ledger.UnresolvedCargoIds);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(18, incoming.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(88, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Empty(outgoing.Level3Ledger.UnresolvedOriginalIds);

        List<Color> transitionColors = [];
        TransitionStarfield.Draw(game.CurrentApproach.ExitedLevel, 1, (_, color) =>
            transitionColors.Add(color));
        Assert.True(transitionColors.Distinct().Count() > 1);

        int initialLane = game.Player.EdgeIndex;
        float initialPosition = game.Player.EdgePosition;
        game.RunOneStep(new PlayerInput(
            Fire: true, MouseDeltaX: 0, Left: false, Right: true, ZapPressed: true));
        Assert.NotEqual((initialLane, initialPosition),
            (game.Player.EdgeIndex, game.Player.EdgePosition));
        Assert.Empty(game.Shots);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(LevelState.Approach, game.State);

        FinishApproach(game);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(88, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
    }

    [Fact]
    public void FailedLevelFiveSpikeExitRemainsOnLevelFive()
    {
        TempestGame game = CreateNaturalLevel5();
        CompleteLevel5Wave(game);
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick <= outgoing.Configuration.AvoidSpikesWaitThreshold; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);

        for (int tick = 0; tick < 200 && game.State != LevelState.ReverseZoom; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level5Production, game.CurrentLevel.Configuration);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SuccessfulLevelSixExitInstallsLevel7FromNaturalOrDirectStart(bool directStart)
    {
        TempestGame game = directStart
            ? new TempestGame(startingProductionLevel: 6, startingLives: 5)
            : CreateNaturalLevel6(startingLives: 5);
        Assert.True(game.NaturalProgressionEnabled);
        game.Score.Add(88);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        Assert.True(game.SuperZapper.FlashTicksRemaining > 0);

        CompleteProductionLedgerWave(game);
        Level outgoing = game.CurrentLevel;
        Assert.Same(LevelConfiguration.Level6Production, outgoing.Configuration);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Same(LevelConfiguration.Level7Production,
            LevelProgression.GetNextProductionLevel(outgoing.Configuration));
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;

        for (int tick = 0; tick < outgoing.Configuration.AvoidSpikesWaitThreshold; tick++)
        {
            game.RunOneStep();
            Assert.Equal(LevelState.AvoidSpikes, game.State);
            Assert.Same(outgoing, game.CurrentLevel);
        }
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Step(game, 21);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Same(LevelConfiguration.Level7Production, incoming.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(6, game.CurrentApproach!.ExitedLevel);
        Assert.Equal(20, incoming.Level3Ledger!.OriginalCount);
        Assert.Equal(20, incoming.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(incoming.Level3Ledger.UnresolvedCargoIds);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, incoming.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.Equal(88, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.True(game.NaturalProgressionEnabled);

        List<Color> transitionColors = [];
        TransitionStarfield.Draw(game.CurrentApproach.ExitedLevel, 1, (_, color) =>
            transitionColors.Add(color));
        Assert.True(transitionColors.Distinct().Count() > 1);
        (int initialLane, float initialPosition) =
            (game.Player.EdgeIndex, game.Player.EdgePosition);
        game.RunOneStep(new PlayerInput(
            Fire: true, MouseDeltaX: 0, Left: false, Right: true, ZapPressed: true));
        Assert.NotEqual((initialLane, initialPosition),
            (game.Player.EdgeIndex, game.Player.EdgePosition));
        Assert.Empty(game.Shots);
        Assert.Equal(2, game.SuperZapperChargesRemaining);

        FinishApproach(game);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(88, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
    }

    [Fact]
    public void FailedNaturalLevelSixSpikeExitRemainsOnLevelSix()
    {
        TempestGame game = CreateNaturalLevel6();
        CompleteProductionLedgerWave(game);
        Level outgoing = game.CurrentLevel;
        Spike exitSpike = game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex);
        exitSpike.Height = 0;
        Step(game, outgoing.Configuration.AvoidSpikesWaitThreshold + 1 + 21);
        Assert.Equal(LevelState.ExitZoom, game.State);

        exitSpike.Height = 398;
        game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
    }

    [Fact]
    public void NaturalLevelSevenEntersLevelEightAndLevelEightCompletionIsTerminal()
    {
        TempestGame game = CreateNaturalLevel6(startingLives: 5);
        CompleteProductionLedgerWave(game);
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        Step(game, game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold + 1 + 21);
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        FinishApproach(game);

        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        CompleteProductionLedgerWave(game);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        Step(game, level.Configuration.AvoidSpikesWaitThreshold + 1 + 21);
        for (int tick = 0; tick < 250 && ReferenceEquals(level, game.CurrentLevel); tick++)
            game.RunOneStep();

        Assert.NotSame(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level8Production, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        FinishApproach(game);

        Level levelEight = game.CurrentLevel;
        Level3WaveLedger levelEightLedger = levelEight.Level3Ledger!;
        CompleteProductionLedgerWave(game);
        Assert.Equal(LevelState.ExitDelay, game.State);
        foreach (Spike spike in game.Spikes)
            spike.Height = 0;
        Step(game, 21);
        for (int tick = 0; tick < 250 && !game.LevelComplete; tick++)
            game.RunOneStep();

        Assert.True(game.LevelComplete);
        Assert.Same(levelEight, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level8Production, game.CurrentLevel.Configuration);
        Assert.Null(LevelProgression.GetNextProductionLevel(game.CurrentLevel.Configuration));
        Assert.Empty(levelEightLedger.UnresolvedOriginalIds);

        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    private static TempestGame CreateNaturalLevel5(
        int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = CreateNaturalLevel4(startingLives);
        FinishApproach(game);
        CompleteLevel4Wave(game);
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        int waitTicks = game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold + 1;
        Step(game, waitTicks);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Step(game, 21);
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level5Production, game.CurrentLevel.Configuration);
        FinishApproach(game);
        return game;
    }

    private static TempestGame CreateNaturalLevel6(
        int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = CreateNaturalLevel5(startingLives);
        CompleteLevel5Wave(game);
        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        Step(game, game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold + 1);
        Step(game, 21);
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 250 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        FinishApproach(game);
        return game;
    }

    private static void CompleteLevel5Wave(TempestGame game)
    {
        CompleteProductionLedgerWave(game);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
    }

    private static TempestGame CreateNaturalLevel3(int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: startingLives);
        AssertFreshLevel2AfterExit(game, expectedScore: 0, expectedLives: startingLives);

        for (int enemy = 0; enemy < game.CurrentLevel.EnemySupply; enemy++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        FinishLevel2Exit(game);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        return game;
    }

    private static TempestGame CreateNaturalLevel4(int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = CreateNaturalLevel3(startingLives);
        CompleteNaturalLevel3Wave(game);
        FinishLevel3Exit(game);
        return game;
    }

    private static void CompleteNaturalLevel3Wave(TempestGame game)
    {
        CompleteProductionLedgerWave(game);
        Assert.Equal(LevelState.ExitDelay, game.State);
    }

    private static void CompleteLevel4Wave(TempestGame game)
    {
        CompleteProductionLedgerWave(game);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
    }

    private static void CompleteProductionLedgerWave(TempestGame game)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        foreach (int id in ledger.UnresolvedOriginalIds.Order())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        level.ResetAfterDeath();
        game.RunOneStep();
    }

    private static void FinishLevel3Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 300 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
    }

    [Fact]
    public void NaturalLevel2SuccessfulExitCreatesFreshMixedWaveAndCarriesCampaignState()
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        game.Score.Add(1500);
        game.Player.Lives = 3;
        AssertFreshLevel2AfterExit(game, expectedScore: 1500, expectedLives: 3);

        Level outgoing = game.CurrentLevel;
        game.Score.Add(275);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.True(game.SuperZapper.FlashTicksRemaining > 0);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Assert.NotEmpty(game.Shots);

        for (int enemy = 0; enemy < outgoing.EnemySupply; enemy++)
            outgoing.RecordEnemyDestroyed();
        outgoing.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Shots);

        FinishLevel2Exit(game);

        AssertFreshLevel3(game, outgoing, expectedScore: 1775, expectedLives: 2);
    }

    [Fact]
    public void NaturalLevel3DeathReconstructsInPlaceAndGameOverRestartStartsLevel1()
    {
        TempestGame game = CreateClearedLevel1(kills: 10, startingLives: 5);
        AssertFreshLevel2AfterExit(game, expectedScore: 0, expectedLives: 5);
        Level level2 = game.CurrentLevel;
        for (int enemy = 0; enemy < level2.EnemySupply; enemy++)
            level2.RecordEnemyDestroyed();
        level2.ResetAfterDeath();
        game.RunOneStep();
        FinishLevel2Exit(game);

        Level level3 = game.CurrentLevel;
        Level3WaveLedger ledger = level3.Level3Ledger!;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Same(level3, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Same(ledger, game.CurrentLevel.Level3Ledger);
        Assert.Equal(LevelState.Playing, game.State);

        game.Score.Add(300);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);

        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
    }

    private static TempestGame CreateClearedLevel1(
        int kills, bool enemyBulletsTest = false, int startingLives = Player.DefaultStartingLives)
    {
        TempestGame game = new(startingLives: startingLives, enemyBulletsTest: enemyBulletsTest);
        for (int enemy = 0; enemy < kills; enemy++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        return game;
    }

    private static void CompleteRemainingUnitsAtRim(TempestGame game, int rimClears)
    {
        Level level = game.CurrentLevel;
        for (int batch = 0; batch <= level.EnemySupply &&
             (level.ReserveEggs > 0 || level.Eggs.Count > 0); batch++)
        {
            if (level.Eggs.Count == 0)
                for (int tick = 0; tick < level.Configuration.ReserveReleaseIntervalTicks; tick++)
                    level.UpdateEggRelease();

            foreach (Egg egg in level.Eggs)
            {
                if (egg.State != EggState.ReadyToHatch)
                {
                    egg.BeginApproach();
                    for (int tick = 0; tick < 40; tick++)
                        egg.Update(level);
                }
                Assert.Equal(EggState.ReadyToHatch, egg.State);
            }

            for (int hatched = 0; hatched < level.Configuration.MaximumActiveEggs; hatched++)
                if (level.HatchReadyEnemy(lane => new Flipper(lane, 1)) is null)
                    break;
        }

        Assert.Equal(0, level.ReserveEggs);
        Assert.Empty(level.Eggs);
        for (int enemy = 0; enemy < rimClears; enemy++)
            level.RecordEnemyRimCleared();
        game.RunOneStep();
        Assert.Equal(rimClears, level.EnemiesRimCleared);
    }

    private static void AssertFreshLevel2AfterExit(TempestGame game, int expectedScore, int expectedLives)
    {
        Level outgoing = game.CurrentLevel;
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        FinishLevel1Exit(game);
        AssertFreshLevel2(game, outgoing, expectedScore: expectedScore, expectedLives: expectedLives);
        FinishApproach(game);
    }

    private static void AssertFreshLevel2(TempestGame game, Level outgoing, int expectedScore, int expectedLives)
    {
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1, game.CurrentLevel.Configuration.ProgressionIndex);
        Assert.Equal(2, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(1, game.CurrentLevel.Configuration.BoardIndex);
        Assert.Equal(0, game.CurrentLevel.Configuration.PaletteIndex);
        Assert.True(game.CurrentLevel.Configuration.Definition.Continuous);
        Assert.Equal(16, game.CurrentLevel.PlayableLaneCount);
        Assert.True(game.CurrentLevel.Configuration.Flipper.CanFlip);
        Assert.Equal(12, game.CurrentLevel.EnemySupply);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(12, game.CurrentLevel.RemainingEnemies);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikes);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(0, game.TankerAccountingTotal);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(expectedScore, game.Score.Score);
        Assert.Equal(expectedLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.All(game.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
    }

    private static void FinishLevel1Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 200 &&
             ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
    }

    private static void FinishLevel2Exit(TempestGame game)
    {
        Level outgoing = game.CurrentLevel;
        for (int tick = 0; tick < 200 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();
        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        FinishApproach(game);
    }

    private static void AssertFreshLevel3(TempestGame game, Level outgoing,
        int expectedScore, int expectedLives)
    {
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = Assert.IsType<Level3WaveLedger>(level.Level3Ledger);

        Assert.NotSame(outgoing, level);
        Assert.Same(LevelConfiguration.Level3Mixed, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, level.Configuration.ProgressionIndex);
        Assert.Equal(3, level.Configuration.DisplayedLevel);
        Assert.Equal(2, level.Configuration.BoardIndex);
        Assert.Equal(0, level.Configuration.PaletteIndex);
        Assert.True(level.IsLevel3Mixed);
        Assert.Equal(15, level.EnemySupply);
        Assert.Equal(15, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(11, level.ReserveEggs);
        Assert.Equal(4, level.EnemiesSpawned);
        Assert.Equal(0, level.EnemiesDestroyed);
        Assert.Equal(0, level.EnemiesRimCleared);
        Assert.Equal(15, ledger.OriginalCount);
        Assert.Equal(0, ledger.CargoCount);
        Assert.Equal(15, ledger.TotalIdentityCount);
        Assert.Equal(15, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Shots);
        Assert.Empty(game.Spikes);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Equal(0, game.PendingTankerCargoUnits);
        Assert.Equal(0, game.TankerAccountingTotal);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Equal(expectedScore, game.Score.Score);
        Assert.Equal(expectedLives, game.Player.Lives);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.All(level.Eggs, egg => Assert.Equal(EggState.Roaming, egg.State));
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }

    private static void FinishGameOverPresentation(TempestGame game)
    {
        for (int tick = 0; tick < 500 && !game.GameOver; tick++)
            game.RunOneStep();
        Assert.True(game.GameOver);
    }

    private static void FinishApproach(TempestGame game)
    {
        Assert.Equal(LevelState.Approach, game.State);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }
}
