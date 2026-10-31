using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class SuperZapperTests
{
    [Fact]
    public void EligibleEnemiesAcceptAndReplaceZapTimersWhileBulletsReject()
    {
        Enemy[] eligible =
        [
            new Flipper(0, 10),
            new Tanker(1, 0),
            new Spiker(2),
            new Fuseball(3),
            new Pulsar(4)
        ];

        foreach (Enemy enemy in eligible)
        {
            Assert.True(enemy.TryZap(8));
            Assert.Equal(8, enemy.PendingZapTicks);
            Assert.True(enemy.TryZap(4));
            Assert.Equal(4, enemy.PendingZapTicks);
        }

        EnemyBullet bullet = new(0, 10);
        Assert.False(bullet.TryZap(4));
        Assert.Equal(0, bullet.PendingZapTicks);
        Assert.IsNotAssignableFrom<Enemy>(new Spike());
    }

    [Fact]
    public void FirstUseAssignsAcceptedDelaysInOrderAndSkipsRejectedBullets()
    {
        SuperZapperSystem zapper = new();
        Flipper first = new(1, 10);
        EnemyBullet bullet = new(2, 10);
        Spiker second = new(3);
        Tanker third = new(4, 0);

        Assert.True(zapper.TryActivate(
            [first, bullet, second, third], playerEdge: 0, edgeCount: 16, continuous: true));

        Assert.Equal(4, first.PendingZapTicks);
        Assert.Equal(0, bullet.PendingZapTicks);
        Assert.Equal(6, second.PendingZapTicks);
        Assert.Equal(8, third.PendingZapTicks);
        Assert.Equal(1, zapper.ChargesRemaining);
        Assert.Equal(60, zapper.FlashTicksRemaining);
    }

    [Fact]
    public void StrengthTwoStartsFlashWithoutTargetsButStrengthOneDoesNot()
    {
        SuperZapperSystem zapper = new();
        Assert.True(zapper.TryActivate([], 0, 16, continuous: true));
        Assert.Equal(60, zapper.FlashTicksRemaining);

        for (int tick = 0; tick < 60; tick++)
            zapper.AdvanceDisplayStep();
        Assert.Equal(0, zapper.FlashTicksRemaining);

        Assert.True(zapper.TryActivate([], 0, 16, continuous: true));
        Assert.Equal(0, zapper.FlashTicksRemaining);
        Assert.Equal(0, zapper.ChargesRemaining);
        Assert.False(zapper.TryActivate([], 0, 16, continuous: true));
    }

    [Fact]
    public void FlashColorUsesTheLiteralDisplayCounterPhaseFormula()
    {
        SuperZapperSystem zapper = new();
        zapper.TryActivate([], 0, 16, continuous: true);
        Assert.True(zapper.UsesAlternateFlashColor);

        zapper.AdvanceDisplayStep();
        Assert.False(zapper.UsesAlternateFlashColor);
        for (int tick = 0; tick < 3; tick++)
            zapper.AdvanceDisplayStep();
        Assert.False(zapper.UsesAlternateFlashColor);
        zapper.AdvanceDisplayStep();
        Assert.True(zapper.UsesAlternateFlashColor);
    }

    [Fact]
    public void SingleUseReplacesPendingTimerAndStartsFlash()
    {
        SuperZapperSystem zapper = new();
        Flipper target = new(0, 10);
        Assert.True(zapper.TryActivate([target], 0, 16, continuous: true));
        Assert.True(target.TryZap(9));
        zapper.AdvanceDisplayStep();

        Assert.True(zapper.TryActivate([target], 0, 16, continuous: true));

        Assert.Equal(4, target.PendingZapTicks);
        Assert.Equal(60, zapper.FlashTicksRemaining);
        Assert.Equal(0, zapper.ChargesRemaining);
    }

    [Fact]
    public void SingleTargetUsesCircularOrDirectLaneDistanceAsConfigured()
    {
        Assert.Equal(8, SuperZapperSystem.CalculateLaneDistance(8, 0, 16, continuous: true));
        Assert.Equal(1, SuperZapperSystem.CalculateLaneDistance(15, 0, 16, continuous: true));
        Assert.Equal(15, SuperZapperSystem.CalculateLaneDistance(15, 0, 16, continuous: false));

        Pulsar wrappedCandidate = new(15, 20);
        Flipper nearbyCandidate = new(1, 10);

        Assert.Same(wrappedCandidate, SuperZapperSystem.SelectTarget(
            [wrappedCandidate, nearbyCandidate], 0, 16, continuous: true));
        Assert.Same(nearbyCandidate, SuperZapperSystem.SelectTarget(
            [wrappedCandidate, nearbyCandidate], 0, 16, continuous: false));
    }

    [Fact]
    public void SingleTargetKeepsLiteralHeightDistanceAndTypeComparisonOrder()
    {
        Spiker incumbent = new(0);
        Assert.Same(incumbent, SuperZapperSystem.SelectTarget(
            [incumbent, new Fuseball(2)], 0, 16, continuous: true));

        Spiker bestDistance = new(0);
        Fuseball oneLaneFarther = new(1);
        Assert.Same(oneLaneFarther, SuperZapperSystem.SelectTarget(
            [bestDistance, oneLaneFarther], 0, 16, continuous: true));

        Flipper closerButShorter = new(1, 5);
        Pulsar fartherAndTaller = new(2, 10);
        Assert.Same(closerButShorter, SuperZapperSystem.SelectTarget(
            [fartherAndTaller, closerButShorter], 0, 16, continuous: true));

        Flipper equalFirst = new(3, 10);
        Flipper equalSecond = new(3, 10);
        Assert.Same(equalFirst, SuperZapperSystem.SelectTarget(
            [equalFirst, equalSecond], 0, 16, continuous: true));
    }

    [Fact]
    public void SingleTargetAllowsTallerFartherCandidateOnlyInsideStrictGate()
    {
        Spiker nearest = new(0, 10);
        Pulsar oneLaneFartherAndTaller = new(1, 11);
        Assert.Same(oneLaneFartherAndTaller, SuperZapperSystem.SelectTarget(
            [nearest, oneLaneFartherAndTaller], 0, 16, continuous: true));

        Spiker bestDistance = new(0, 10);
        Fuseball excluded = new(2, 100);
        Assert.Same(bestDistance, SuperZapperSystem.SelectTarget(
            [bestDistance, excluded], 0, 16, continuous: true));
    }

    [Fact]
    public void PlayerInputKeepsFourArgumentConstructionAndUsesEdgePressSources()
    {
        PlayerInput legacy = new(false, 0, false, false);
        Assert.False(legacy.ZapPressed);
        Assert.True(InputSystem.IsSuperZapperPressed(spacePressed: true, rightMousePressed: false));
        Assert.True(InputSystem.IsSuperZapperPressed(spacePressed: false, rightMousePressed: true));
        Assert.False(InputSystem.IsSuperZapperPressed(spacePressed: false, rightMousePressed: false));
    }

    [Fact]
    public void GameConsumesInjectedZapOnlyOnceForEachPressedEdge()
    {
        TempestGame game = new();
        Assert.Equal(2, game.SuperZapperChargesRemaining);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(60, game.SuperZapper.FlashTicksRemaining);

        game.RunOneStep();
        Assert.Equal(1, game.SuperZapperChargesRemaining);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void FirstUseCountdownIncludesActivationTickAndSkipsExpiryUpdateMovement()
    {
        TempestGame game = HatchOneFlipper();
        Flipper flipper = Assert.Single(game.Flippers);
        float initialHeight = flipper.Height;

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(3, flipper.PendingZapTicks);
        Assert.Equal(initialHeight + 2, flipper.Height);
        Assert.Equal(0, game.Score.Score);

        Step(game, 2);
        Assert.Equal(1, flipper.PendingZapTicks);
        float beforeExpiry = flipper.Height;
        game.RunOneStep();

        Assert.Empty(game.Flippers);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(beforeExpiry, flipper.Height);
    }

    [Fact]
    public void NormalLevelOneZapOrderUsesFlipperCreationOrder()
    {
        TempestGame game = new();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper first = game.Flippers[0];
        Flipper second = game.Flippers[1];

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(3, first.PendingZapTicks);
        Assert.Equal(5, second.PendingZapTicks);
    }

    [Fact]
    public void DelaySixExpiresOnItsSixthTimerVisit()
    {
        Flipper flipper = new(0, 10);
        Assert.True(flipper.TryZap(6));

        for (int visit = 0; visit < 5; visit++)
            Assert.False(flipper.AdvanceZapTimer());
        Assert.Equal(1, flipper.PendingZapTicks);
        Assert.True(flipper.AdvanceZapTimer());
        Assert.Equal(0, flipper.PendingZapTicks);
    }

    [Fact]
    public void PlayerShotPreemptsPendingFlipperZapWithoutDoubleResolution()
    {
        TempestGame game = HatchOneFlipper();
        Flipper flipper = Assert.Single(game.Flippers);
        game.Player.Move(flipper.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = flipper.Height + 5;
        flipper.TryZap(1);

        game.RunOneStep();
        Step(game, 4);

        Assert.Empty(game.Flippers);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void SpikerZapLeavesItsSpikeAndAwardsFiftyOnce()
    {
        TempestGame game = HatchSpiker();
        Spiker spiker = Assert.Single(game.Spikers);
        game.RunOneStep();
        Spike spike = Assert.Single(game.Spikes, spike => spike.EdgeIndex == spiker.EdgeIndex);
        float spikeHeight = spike.Height;
        spiker.TryZap(1);

        game.RunOneStep();
        Step(game, 3);

        Assert.Empty(game.Spikers);
        Assert.Contains(spike, game.Spikes);
        Assert.Equal(spikeHeight, spike.Height);
        Assert.Equal(50, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void PulsarZapAllowsEarlierMovementAndKeepsTheSharedPulseClockRunning()
    {
        TempestGame game = new(pulsarTest: true);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Pulsar pulsar = Assert.Single(game.Pulsars);
        float initialHeight = pulsar.Height;
        int initialPhase = game.PulsePhase;
        pulsar.TryZap(2);

        game.RunOneStep();
        Assert.Equal(1, pulsar.PendingZapTicks);
        Assert.Equal(initialHeight + 1, pulsar.Height);
        Assert.Equal(initialPhase + 1, game.PulsePhase);
        float expiryHeight = pulsar.Height;

        Assert.Contains(pulsar, game.Pulsars);
        game.RunOneStep();
        Assert.Empty(game.Pulsars);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(expiryHeight, pulsar.Height);
        Assert.Equal(initialPhase + 2, game.PulsePhase);
    }

    [Fact]
    public void SpikerRetreatPreemptsPendingZapAndKeepsTheSpike()
    {
        TempestGame game = HatchSpiker();
        Spiker spiker = Assert.Single(game.Spikers);
        spiker.TryZap(400);

        for (int tick = 0; tick < 350 && game.Spikers.Contains(spiker); tick++)
            game.RunOneStep();

        Assert.DoesNotContain(spiker, game.Spikers);
        Assert.NotEmpty(game.Spikes);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void FuseballExpiryUsesCurrentHeightAndCreatesOneScorePopup()
    {
        TempestGame game = new(fuseballTest: true);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Fuseball fuseball = Assert.Single(game.Fuseballs);
        fuseball.TryZap(2);
        game.RunOneStep();
        Assert.Equal(1, fuseball.PendingZapTicks);
        float expiryHeight = fuseball.Height;
        int expectedScore = fuseball.KillScore;

        game.RunOneStep();

        Assert.Empty(game.Fuseballs);
        Assert.Equal(expectedScore, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        FuseballScorePopup popup = Assert.Single(game.FuseballScorePopups);
        Assert.Equal(expiryHeight, popup.Height);
        Assert.Equal(expectedScore, popup.Points);
        Assert.Equal(30, popup.RemainingTicks);
    }

    [Fact]
    public void PendingTankerCanBeDestroyedByShotBeforeZapExpiry()
    {
        TempestGame game = HatchTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        game.Player.Move(tanker.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        shot.Height = tanker.Height - 5 + 6;
        tanker.TryZap(1);

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(200, game.Score.Score);
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(0, flipper.PendingZapTicks));
    }

    [Fact]
    public void PendingTankerNaturalReleasePreemptsZapWithoutScoring()
    {
        TempestGame game = HatchTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        while (tanker.Height + Tanker.DevelopmentClimbSpeed <= Tanker.CargoReleaseThreshold)
            tanker.UpdateForDevelopment();
        Assert.Equal(TankerState.Climbing, tanker.State);
        tanker.TryZap(2);

        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsEscaped);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(0, game.Score.Score);
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo),
            flipper => Assert.Equal(0, flipper.PendingZapTicks));
    }

    [Fact]
    public void TankerExpiryTransfersLatentCargoToTwoStillbornChildren()
    {
        TempestGame game = HatchTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(2, game.TankerLatentCargoUnits);

        tanker.TryZap(1);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(0, game.Score.Score);
        game.RunOneStep();

        Assert.Empty(game.Tankers);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(200, game.Score.Score);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.Flippers.Where(flipper => flipper.IsTankerCargo), flipper =>
        {
            Assert.Equal(FlipperState.Flipping, flipper.State);
            Assert.Equal(8, flipper.PendingZapTicks);
            Assert.Equal(tanker.Height, flipper.Height);
        });
        Flipper[] children = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .OrderBy(flipper => flipper.CargoUnitId).ToArray();
        Assert.Equal(2, children.Length);
        Assert.Equal(0, children[0].CargoUnitId);
        Assert.Equal(1, children[1].CargoUnitId);
        Assert.Equal(game.CurrentLevel.PreviousEdge(tanker.EdgeIndex), children[0].SourceEdge);
        Assert.Equal(game.CurrentLevel.NextEdge(tanker.EdgeIndex), children[1].SourceEdge);
        Assert.Equal(game.CurrentLevel.PreviousEdge(children[0].SourceEdge), children[0].DestinationEdge);
        Assert.Equal(game.CurrentLevel.NextEdge(children[1].SourceEdge), children[1].DestinationEdge);
        Assert.DoesNotContain(tanker.EdgeIndex, new[] { children[0].SourceEdge, children[0].DestinationEdge });
        Assert.DoesNotContain(tanker.EdgeIndex, new[] { children[1].SourceEdge, children[1].DestinationEdge });
        Assert.Equal(1, children[0].Direction);
        Assert.Equal(-1, children[1].Direction);
        Assert.Equal(
            game.CurrentLevel.CalculateEdgeTurnAngle(children[0].SourceEdge, children[0].DestinationEdge),
            children[0].TotalTurnAngle);

        game.Player.Move(5, game.CurrentLevel.Definition.Points.Count);
        float materializedHeight = children[0].Height;
        game.RunOneStep();
        Assert.Equal(7, children[0].PendingZapTicks);
        Assert.Equal(materializedHeight + 2, children[0].Height);
        Assert.Equal(1, children[0].Direction);
        Assert.Equal(game.CurrentLevel.PreviousEdge(children[0].SourceEdge), children[0].DestinationEdge);
        Assert.False(children[0].CompletedIntoPlayer);
        Step(game, 6);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo &&
            flipper.PendingZapTicks == 1));
        game.RunOneStep();

        Assert.Equal(0, game.TankerCargoUnitsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsResolved);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(500, game.Score.Score);
    }

    [Fact]
    public void RemainingChargeCanReplaceStillbornCargoTimer()
    {
        Level level = new();
        Tanker tanker = new(0, 0, climbSpeed: 1);
        TankerCargoSpawn cargo = tanker.ReleaseFlipperCargo(
            level, 1, TankerReleaseKind.SuperZapperDestruction).Cargo[0];
        Flipper child = Flipper.CreateTankerCargo(level, cargo, 7);
        child.TryZap(8);
        SuperZapperSystem zapper = new();
        zapper.TryActivate([], 0, 16, continuous: true);

        Assert.True(zapper.TryActivate([child], 0, 16, continuous: true));

        Assert.Equal(0, zapper.ChargesRemaining);
        Assert.Equal(4, child.PendingZapTicks);
        Assert.Equal(7, child.CargoUnitId);
        Assert.Equal(FlipperState.Flipping, child.State);
        Assert.Equal(level.PreviousEdge(level.PreviousEdge(tanker.EdgeIndex)), child.DestinationEdge);
    }

    [Fact]
    public void TankerDeathBeforeCarrierExpiryRestoresSlotAndLatentCargo()
    {
        TempestGame game = HatchTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        tanker.TryZap(100);
        HatchOriginalFlipper(game);
        CauseTankerDeath(game);
        Step(game, TempestGame.PlayerDeathTicks);

        Assert.Empty(game.Tankers);
        Assert.Equal(0, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(10, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerLatentCargoUnits);
        Assert.Equal(10, game.TankerAccountingTotal);
        Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        Assert.Equal(0, game.Score.Score);
    }

    [Fact]
    public void TankerDeathAfterCarrierExpiryRestoresEachUnresolvedCargoIdentity()
    {
        TempestGame game = HatchTanker();
        Tanker tanker = Assert.Single(game.Tankers);
        tanker.TryZap(1);
        game.RunOneStep();
        int[] cargoIds = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .Select(flipper => flipper.CargoUnitId).Order().ToArray();
        Assert.Equal(2, cargoIds.Distinct().Count());
        foreach (Flipper child in game.Flippers.Where(flipper => flipper.IsTankerCargo))
            child.TryZap(100);

        HatchOriginalFlipper(game);
        CauseTankerDeath(game);
        Step(game, TempestGame.PlayerDeathTicks);

        int[] restoredIds = game.Eggs.Where(egg => egg.CargoUnitId >= 0)
            .Select(egg => egg.CargoUnitId).Order().ToArray();
        Assert.Equal(cargoIds, restoredIds);
        Assert.Equal(1, game.TankerOriginalSlotsDestroyed);
        Assert.Equal(9, game.TankerOriginalSlotsRemaining);
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(200, game.Score.Score);
    }

    [Fact]
    public void RepeatedTankerDeathAndReentryPreservesCargoIdsWithoutDuplication()
    {
        TempestGame game = HatchTanker();
        Assert.Single(game.Tankers).TryZap(1);
        game.RunOneStep();
        int[] originalIds = game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .Select(flipper => flipper.CargoUnitId).Order().ToArray();
        foreach (Flipper child in game.Flippers.Where(flipper => flipper.IsTankerCargo))
            child.TryZap(100);

        HatchOriginalFlipper(game);
        CauseTankerDeath(game);
        Step(game, TempestGame.PlayerDeathTicks);
        HatchCargoAndOriginalFlippers(game, originalIds);
        CauseTankerDeath(game);
        Step(game, TempestGame.PlayerDeathTicks);

        int[] restoredIds = game.Eggs.Where(egg => egg.CargoUnitId >= 0)
            .Select(egg => egg.CargoUnitId).Order().ToArray();
        Assert.Equal(originalIds, restoredIds);
        Assert.Equal(originalIds.Length, originalIds.Distinct().Count());
        Assert.Equal(2, game.TankerCargoUnitsRemaining);
        Assert.Equal(12, game.TankerAccountingTotal);
        Assert.Equal(200, game.Score.Score);
    }

    [Fact]
    public void TankerDeathAfterOneChildResolvesRestoresOnlyItsUnresolvedSibling()
    {
        TempestGame game = HatchTanker();
        Assert.Single(game.Tankers).TryZap(1);
        game.RunOneStep();
        Flipper[] children = game.Flippers.Where(flipper => flipper.IsTankerCargo).ToArray();
        Assert.Equal(2, children.Length);
        Flipper resolved = children[0];
        Flipper pending = children[1];
        int resolvedId = resolved.CargoUnitId;
        int pendingId = pending.CargoUnitId;
        resolved.TryZap(1);
        pending.TryZap(100);
        game.RunOneStep();
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        HatchOriginalFlipper(game);
        CauseTankerDeath(game);
        Step(game, TempestGame.PlayerDeathTicks);

        Assert.DoesNotContain(game.Eggs, egg => egg.CargoUnitId == resolvedId);
        Assert.Single(game.Eggs, egg => egg.CargoUnitId == pendingId);
        Assert.Equal(1, game.TankerCargoUnitsResolved);
        Assert.Equal(1, game.TankerCargoUnitsRemaining);
        Assert.Equal(350, game.Score.Score);
        Assert.Equal(12, game.TankerAccountingTotal);
    }

    [Fact]
    public void DeathFreezePreservesChargeAndDoesNotAdvancePendingZapTimer()
    {
        TempestGame game = HatchPulsar();
        Pulsar pulsar = Assert.Single(game.Pulsars);
        AdvancePulsarToRim(pulsar, game);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(3, pulsar.PendingZapTicks);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Step(game, TempestGame.PlayerDeathTicks - 1);
        Assert.Equal(3, pulsar.PendingZapTicks);
        Assert.Equal(1, game.SuperZapper.FlashTicksRemaining);
        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(0, game.Score.Score);
        Assert.Empty(game.Pulsars);
    }

    [Fact]
    public void RestartAndReplayRestoreChargesWhileInactiveInputIsRejected()
    {
        TempestGame game = new();
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(2, game.SuperZapperChargesRemaining);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        game.RunOneStep(restartPressed: true);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void FinalPendingEnemyBlocksCompletionUntilItsZapExpires()
    {
        TempestGame game = new();
        for (int index = 0; index < Level.EnemyTotal - 1; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Flipper last = Assert.Single(game.Flippers);
        last.TryZap(2);

        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        game.RunOneStep();

        Assert.Equal(10, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Equal(150, game.Score.Score);
    }

    [Fact]
    public void PendingZapAtTheRimIsClearedWithoutDelayedScoreWhileFlashContinues()
    {
        TempestGame game = new();
        for (int index = 0; index < Level.EnemyTotal - 1; index++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Flipper last = Assert.Single(game.Flippers);
        game.Player.Move(3 - game.Player.EdgeIndex, game.CurrentLevel.Definition.Points.Count);
        for (int tick = 0; tick < 300 && last.Height < Level.Depth; tick++)
            last.Update(game.CurrentLevel, game.Player.EdgeIndex);
        Assert.Equal(Level.Depth, last.Height);

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Flippers);
        Assert.Equal(9, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(3, last.PendingZapTicks);
        Assert.Equal(60, game.SuperZapper.FlashTicksRemaining);

        Step(game, 5);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(1, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(55, game.SuperZapper.FlashTicksRemaining);
    }

    [Fact]
    public void TubeFlashChangesOnlyColorAndExpiresAfterSixtyDisplaySteps()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End, Color Color)> normal = [];
        renderer.DrawTube((start, end, color) => normal.Add((start, end, color)));
        Assert.All(normal, line => Assert.Equal(Color.Blue, line.Color));

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End, Color Color)> flash = [];
        renderer.DrawTube((start, end, color) => flash.Add((start, end, color)));
        Assert.Equal(normal.Count, flash.Count);
        Assert.All(flash, line => Assert.Equal(new Color(255, 0, 255, 255), line.Color));
        Assert.Equal(normal.Select(line => (line.Start, line.End)),
            flash.Select(line => (line.Start, line.End)));

        Step(game, 59);
        Assert.Equal(1, game.SuperZapper.FlashTicksRemaining);
        game.RunOneStep();
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End, Color Color)> restored = [];
        renderer.DrawTube((start, end, color) => restored.Add((start, end, color)));
        Assert.Equal(normal, restored);
    }

    private static TempestGame HatchOneFlipper()
    {
        TempestGame game = new();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame HatchTanker()
    {
        TempestGame game = new(tankerTest: true, startingLives: 10);
        Egg tankerEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 0);
        MakeReady(tankerEgg, game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame HatchSpiker()
    {
        TempestGame game = new(spikerDevelopmentWave: true, random: new SpikerRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static TempestGame HatchPulsar()
    {
        TempestGame game = new(pulsarTest: true);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        return game;
    }

    private static Flipper HatchOriginalFlipper(TempestGame game)
    {
        Egg originalEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 1);
        MakeReady(originalEgg, game.CurrentLevel);
        game.RunOneStep();
        return Assert.Single(game.Flippers, flipper =>
            !flipper.IsTankerCargo && flipper.OriginalSlotId == 1);
    }

    private static void HatchCargoAndOriginalFlippers(TempestGame game, IReadOnlyCollection<int> cargoIds)
    {
        Egg[] eggs = game.Eggs.Where(egg =>
            cargoIds.Contains(egg.CargoUnitId) || egg.OriginalSlotId == 1).ToArray();
        Assert.Equal(cargoIds.Count + 1, eggs.Length);
        foreach (Egg egg in eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.Equal(cargoIds.Count, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.Equal(cargoIds.Order(), game.Flippers.Where(flipper => flipper.IsTankerCargo)
            .Select(flipper => flipper.CargoUnitId).Order());
    }

    private static void CauseTankerDeath(TempestGame game)
    {
        Flipper killer = game.Flippers.FirstOrDefault(flipper => !flipper.IsTankerCargo)
            ?? HatchOriginalFlipper(game);
        while (killer.State == FlipperState.Climbing)
            killer.Update(game.CurrentLevel, playerEdge: -1);
        Assert.Equal(FlipperState.Pausing, killer.State);

        int destination = game.CurrentLevel.PreviousEdge(killer.EdgeIndex);
        game.Player.Move(destination - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        for (int tick = 0; tick < 100 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
    }

    private static void AdvancePulsarToRim(Pulsar pulsar, TempestGame game)
    {
        int updateCount = 0;
        while (pulsar.Height < Level.Depth && updateCount++ < 5000)
            pulsar.Update(game.CurrentLevel, pulsar.EdgeIndex, static (_, _) => 1);
        Assert.Equal(Level.Depth, pulsar.Height);
        game.Player.Move(pulsar.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.Definition.Points.Count);
        pulsar.BeginRimPlayerKill();
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }

    private sealed class SpikerRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
        public override double NextDouble() => 0;
    }
}
