using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level4CandidateTests
{
    private static readonly Vector2[] BinocularsPoints =
    [
        new(-33, 101), new(-101, 145), new(-185, 116), new(-219, 58),
        new(-219, -29), new(-185, -87), new(-101, -116), new(-33, -72),
        new(33, -72), new(101, -116), new(185, -87), new(219, -29),
        new(219, 58), new(185, 116), new(101, 145), new(33, 101)
    ];

    [Fact]
    public void CandidateDefinesTheExactClosedSourceBoardAndIndependentSpikes()
    {
        LevelConfiguration configuration = LevelConfiguration.Level4Candidate;
        TempestGame game = new(level4Test: true, startingLives: 10, random: new MinimumRandom());
        Level level = game.CurrentLevel;

        Assert.Equal(3, configuration.ProgressionIndex);
        Assert.Equal(4, configuration.DisplayedLevel);
        Assert.Equal(3, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(BinocularsPoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(380, configuration.Definition.CenterY);
        Assert.Equal(-65, configuration.Definition.YOffset);
        Assert.Equal(17, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(13, level.ReserveEggs);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(17, level.Level3Ledger!.TotalIdentityCount);
        Assert.Equal(17, level.Level3Ledger.OriginalCount);
        Assert.Equal(0, level.Level3Ledger.CargoCount);
        Assert.Equal(LevelExitPolicy.AvoidSpikes, configuration.ExitPolicy);
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.True(configuration.FlipperCanFlip);
        Assert.Same(LevelConfiguration.Level5Production,
            LevelProgression.GetNextProductionLevel(configuration));
        Assert.Same(LevelConfiguration.Level4Candidate,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));
    }

    [Fact]
    public void AvoidSpikesWarningAppearsOnlyDuringTheConfiguredAvoidanceState()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> warnings = [];
        void CaptureWarning(Vector2 start, Vector2 end, Color color) =>
            warnings.Add((start, end, color));

        game.CurrentLevel.State = LevelState.AvoidSpikes;
        Assert.True(game.ShouldShowAvoidSpikesWarning);
        renderer.DrawAvoidSpikesWarning(CaptureWarning);
        List<(Vector2 Start, Vector2 End, Color Color)> expectedWarning = [];
        VectorFont.DrawCenteredText(
            "AVOID SPIKES",
            new Vector2(400, 288),
            8,
            (start, end) => expectedWarning.Add((start, end, Color.White)));
        Assert.Equal(expectedWarning, warnings);
        Assert.All(warnings, line => Assert.Equal(Color.White, line.Color));
        Assert.Equal(new Vector2(400, 288),
            BoundsCenter(warnings.SelectMany(line => new[] { line.Start, line.End })));
        Assert.Equal(56f, BoundsSize(warnings.SelectMany(line =>
            new[] { line.Start, line.End })).Y);

        warnings.Clear();
        game.CurrentLevel.State = LevelState.ExitDelay;
        Assert.True(game.ShouldShowAvoidSpikesWarning);
        renderer.DrawAvoidSpikesWarning(CaptureWarning);
        Assert.Equal(expectedWarning, warnings);

        foreach (LevelState state in new[]
        {
            LevelState.Playing,
            LevelState.PlayerDying,
            LevelState.ExitZoom,
            LevelState.ReverseZoom,
            LevelState.Complete,
            LevelState.GameOver
        })
        {
            warnings.Clear();
            game.CurrentLevel.State = state;
            Assert.False(game.ShouldShowAvoidSpikesWarning);
            renderer.DrawAvoidSpikesWarning(CaptureWarning);
            Assert.Empty(warnings);
        }

        LevelConfiguration futureSpikeLevel = LevelConfiguration.Level4Candidate with
        {
            DisplayedLevel = 8,
            ExitPolicy = LevelExitPolicy.Standard
        };
        TempestGame futureGame = new(futureSpikeLevel, random: new MinimumRandom());
        futureGame.CurrentLevel.State = LevelState.AvoidSpikes;
        Assert.Equal(16, futureGame.Spikes.Count);
        Assert.False(futureGame.ShouldShowAvoidSpikesWarning);
    }

    private static Vector2 BoundsCenter(IEnumerable<Vector2> points)
    {
        Vector2[] values = points.ToArray();
        return new Vector2(
            (values.Min(point => point.X) + values.Max(point => point.X)) / 2,
            (values.Min(point => point.Y) + values.Max(point => point.Y)) / 2);
    }

    private static Vector2 BoundsSize(IEnumerable<Vector2> points)
    {
        Vector2[] values = points.ToArray();
        return new Vector2(
            values.Max(point => point.X) - values.Min(point => point.X),
            values.Max(point => point.Y) - values.Min(point => point.Y));
    }

    [Fact]
    public void AvoidSpikesWarningAllowsMovementButSuppressesNewFiring()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        game.CurrentLevel.State = LevelState.AvoidSpikes;
        Assert.True(game.ShouldShowAvoidSpikesWarning);
        float initialLanePosition = game.Player.EdgePosition;
        int firingLane = game.Player.EdgeIndex;
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == firingLane);
        float initialSpikeHeight = spike.Height;

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: true));

        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.True(game.Player.EdgePosition > initialLanePosition);
        Assert.Empty(game.Shots);
        Assert.Equal(initialSpikeHeight, spike.Height);
    }

    [Fact]
    public void LevelFourDiagnosticIsolatedAndComposesWithLivesAndBulletDiagnostics()
    {
        string[] arguments = ["--level4-test", "--lives", "10", "--enemy-bullets-test"];
        Assert.True(DebugController.UseLevel4Test(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.Equal(10, DebugController.GetStartingLives(arguments));

        TempestGame game = new(
            level4Test: DebugController.UseLevel4Test(arguments),
            enemyBulletsTest: DebugController.UseEnemyBulletsTest(arguments),
            startingLives: DebugController.GetStartingLives(arguments),
            random: new MinimumRandom());

        Assert.True(game.Level4Test);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.True(game.EnemyBulletsTest);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
        Assert.False(DebugController.UseLevel4Test([]));
        Assert.Throws<ArgumentException>(() => new TempestGame(
            level4Test: true, startingConfiguration: LevelConfiguration.Level4Candidate));

        string[] exclusiveScenarios =
        [
            "--level2-test", "--level3-test", "--level3-geometry-test",
            "--tanker-test", "--spiker-test", "--fuseball-test", "--pulsar-test"
        ];
        foreach (string scenario in exclusiveScenarios)
        {
            string[] conflicting = ["--level4-test", scenario];
            Assert.True(DebugController.HasConflictingScenarios(conflicting));
            Assert.Throws<ArgumentException>(() => new TempestGame(
                level2Test: DebugController.UseLevel2Test(conflicting),
                level3Test: DebugController.UseLevel3Test(conflicting),
                level3GeometryTest: DebugController.UseLevel3GeometryTest(conflicting),
                tankerTest: DebugController.UseTankerTest(conflicting),
                spikerDevelopmentWave: DebugController.UseSpikerWave(conflicting),
                fuseballTest: DebugController.UseFuseballTest(conflicting),
                pulsarTest: DebugController.UsePulsarTest(conflicting),
                level4Test: DebugController.UseLevel4Test(conflicting)));
        }

        Level completed = game.CurrentLevel;
        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);
        Assert.NotSame(completed, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);

        Level gameOver = game.CurrentLevel;
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.NotSame(gameOver, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
    }

    [Fact]
    public void LevelFourGeometryAndRenderingStayFiniteAcrossEveryDirectedLane()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        Level level = game.CurrentLevel;
        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> tube = [];
        renderer.DrawTube((start, end, color) => tube.Add((start, end, color)));

        Assert.Equal(48, tube.Count);
        Assert.All(tube, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
        });
        for (int lane = 0; lane < level.PlayableLaneCount; lane++)
        {
            Assert.Equal(Projection.Project(BinocularsPoints[lane].X, BinocularsPoints[lane].Y,
                50, 380, -65), tube[lane].Start);
            Assert.Equal(Projection.Project(BinocularsPoints[lane].X, BinocularsPoints[lane].Y,
                450, 380, -65), tube[lane].End);
            int nearConnection = 16 + lane * 2;
            int farConnection = nearConnection + 1;
            int next = level.NextEdge(lane);
            Assert.Equal(Projection.Project(BinocularsPoints[next].X, BinocularsPoints[next].Y,
                50, 380, -65), tube[nearConnection].End);
            Assert.Equal(Projection.Project(BinocularsPoints[next].X, BinocularsPoints[next].Y,
                450, 380, -65), tube[farConnection].End);
        }
        List<(Vector2 Start, Vector2 End)> spikeLines = [];
        renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), AssertFinite);
        Assert.NotEmpty(spikeLines);
        Assert.All(spikeLines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
        });

        for (int lane = 0; lane < level.PlayableLaneCount; lane++)
        {
            Vector2[] originalPoints = level.Definition.Points.ToArray();
            game.Player.ResetPosition(lane, 0.5f);
            renderer.GetPlayerEdge(level.Definition.Points, out Vector2 nearStart, out Vector2 nearEnd);
            AssertFinite(nearStart);
            AssertFinite(nearEnd);
            AssertFinite(renderer.ProjectShot(new Shot { EdgeIndex = lane, Height = 200 }));

            foreach (int destination in new[] { level.PreviousEdge(lane), level.NextEdge(lane) })
            {
                int direction = destination == level.PreviousEdge(lane) ? 1 : -1;
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(lane, destination, direction);
                Assert.Equal(lane, transition.SourceLane);
                Assert.Equal(destination, transition.DestinationLane);
                Assert.Equal(direction, transition.Direction);
                foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(transition, progress);
                    AssertFinite(start);
                    AssertFinite(end);
                    for (int sample = 0; sample <= 10; sample++)
                    {
                        Vector2 point = Vector2.Lerp(start, end, sample / 10f);
                        Assert.True(IsInsideOrOnPolygon(point, BinocularsPoints));
                    }
                }

                (Vector2 initialStart, Vector2 initialEnd) =
                    level.CalculateFlipperPivotEndpoints(transition, 0);
                Assert.Equal(BinocularsPoints[lane], initialStart);
                Assert.Equal(BinocularsPoints[level.NextEdge(lane)], initialEnd);
                (Vector2 destinationStart, Vector2 destinationEnd) =
                    level.CalculateFlipperPivotEndpoints(transition, 1);
                Assert.Equal(BinocularsPoints[destination], destinationStart);
                Assert.Equal(BinocularsPoints[level.NextEdge(destination)], destinationEnd);
            }

            AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(lane, 200)).Start);
            AssertFinite(renderer.ProjectFlipperEndpoints(new Flipper(lane, 200)).End);
            AssertFinite(renderer.ProjectTankerCenter(new Tanker(lane, 100)));
            AssertFinite(renderer.ProjectEgg(game.Eggs[lane % game.Eggs.Count]));
            Assert.Equal(originalPoints, level.Definition.Points);

            List<(Vector2 Start, Vector2 End, Color Color)> laneLines = [];
            renderer.DrawTube((start, end, color) => laneLines.Add((start, end, color)));
            int[] highlightedRails = laneLines.Take(16)
                .Select((line, index) => (line, index))
                .Where(pair => pair.line.Color == new Color(255, 255, 0, 255))
                .Select(pair => pair.index)
                .ToArray();
            Assert.Equal(new[] { lane, level.NextEdge(lane) }.Order(),
                highlightedRails.Order());
            Assert.Equal(lane, game.Player.EdgeIndex);

            Tanker cargoCarrier = new(lane, lane);
            foreach (TankerCargoSpawn spawn in cargoCarrier.ReleaseFlipperCargo(
                         level, 1, TankerReleaseKind.Natural).Cargo)
            {
                Flipper cargoFlipper = Flipper.CreateTankerCargo(
                    level, spawn, cargoUnitId: lane * Tanker.CarriedFlipperCount +
                    (spawn.SourceLane == level.PreviousEdge(lane) ? 0 : 1));
                (Vector2 cargoStart, Vector2 cargoEnd) =
                    renderer.ProjectFlipperEndpoints(cargoFlipper);
                AssertFinite(cargoStart);
                AssertFinite(cargoEnd);
            }
        }

        Vector2[] beforeZap = level.Definition.Points.ToArray();
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        renderer.DrawTube((_, _, _) => { });
        Assert.Equal(beforeZap, level.Definition.Points);
    }

    [Fact]
    public void SpikerIdentityReturnsUnresolvedAndDeathRebuildKeepsStartingSpikes()
    {
        Level level = new(LevelConfiguration.Level4Candidate, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Spike[] initialSpikes = level.Spikes.ToArray();
        int totalIdentityCount = ledger.TotalIdentityCount;

        ledger.CommitOriginalAsSpiker(0);
        ledger.ReturnSpikerToReserve(0);

        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(0));
        Assert.Contains(0, ledger.UnresolvedOriginalIds);
        Assert.Empty(ledger.GetCargoIdsForOriginal(0));

        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        level.ResetAfterDeath();

        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(0));
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, level.Spikes.Count);
        Assert.Equal(initialSpikes, level.Spikes);

        Level3WaveLedger terminalLedger = new(1);
        terminalLedger.MarkOriginalEgg(0);
        terminalLedger.CommitOriginalAsSpiker(0);
        terminalLedger.MarkOriginalDestroyed(0);
        Assert.Equal(Level3OriginalState.Destroyed, terminalLedger.GetOriginalState(0));
        Assert.DoesNotContain(0, terminalLedger.UnresolvedOriginalIds);
        Assert.Equal(16, level.Spikes.Count);
    }

    [Fact]
    public void ProductionCandidateHatchesAndSuperZapperResolvesAStableSpikerIdentity()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        Assert.Equal(EggState.ReadyToHatch, egg.State);

        game.RunOneStep();

        Spiker spiker = Assert.Single(game.Spikers);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(spiker.OriginalSlotId));
        Assert.Equal(Level3OriginalState.ActiveSpiker,
            ledger.GetOriginalState(spiker.OriginalSlotId));
        Assert.Equal(17, ledger.OriginalCount);
        Assert.Equal(17, ledger.TotalIdentityCount);

        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End)> spikerLines = [];
        renderer.DrawSpikers((start, end) => spikerLines.Add((start, end)));
        Assert.NotEmpty(spikerLines);
        Assert.All(spikerLines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
        });

        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        for (int tick = 0; tick < 20 &&
             ledger.GetOriginalState(spiker.OriginalSlotId) != Level3OriginalState.Destroyed; tick++)
            game.RunOneStep();

        Assert.Equal(Level3OriginalState.Destroyed,
            ledger.GetOriginalState(spiker.OriginalSlotId));
        Assert.DoesNotContain(spiker.OriginalSlotId, ledger.UnresolvedOriginalIds);
        Assert.Equal(50, game.Score.Score);
        Assert.Equal(16, game.Spikes.Count);

        List<(Vector2 Start, Vector2 End)> beforeDeath = CaptureSpikes(renderer);
        game.CurrentLevel.State = LevelState.PlayerDying;
        Assert.Equal(beforeDeath, CaptureSpikes(renderer));
    }

    [Fact]
    public void LevelFourDeathRetractsSpikerAndReconstructsItsIdentityOnce()
    {
        LevelConfiguration configuration = LevelConfiguration.Level4Candidate with
        {
            Flipper = LevelConfiguration.Level4Candidate.Flipper with { CanFlip = false },
            InitialActiveEggCount = 2,
            ReserveReleaseIntervalTicks = 10_000
        };
        TempestGame game = new(configuration, random: new MinimumRandom(), startingLives: 5);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Egg spikerEgg = game.Eggs[0];
        ledger.CommitOriginalAsSpiker(spikerEgg.OriginalSlotId);
        spikerEgg.BeginApproach();
        for (int tick = 0; tick < 100 && spikerEgg.State != EggState.ReadyToHatch; tick++)
            spikerEgg.Update(game.CurrentLevel);
        game.RunOneStep();

        Spiker spiker = Assert.Single(game.Spikers);
        int originalId = spiker.OriginalSlotId;
        Egg flipperEgg = Assert.Single(game.Eggs);
        ledger.CommitOriginalAsFlipper(flipperEgg.OriginalSlotId);
        flipperEgg.BeginApproach();
        for (int tick = 0; tick < 100 && flipperEgg.State != EggState.ReadyToHatch; tick++)
            flipperEgg.Update(game.CurrentLevel);
        game.RunOneStep();

        Flipper flipper = Assert.Single(game.Flippers);
        game.Player.ResetPosition(flipper.EdgeIndex);
        Spike persistentSpike = game.Spikes.Single(
            candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        persistentSpike.Height = 390;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        game.RunOneStep();
        game.RunOneStep();
        Assert.Equal(380, persistentSpike.Height);
        Assert.Equal(1, game.Score.Score);

        for (int tick = 0; tick < 500 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Contains(spiker, game.Spikers);
        HashSet<int> unresolvedOriginals = ledger.UnresolvedOriginalIds.ToHashSet();
        int totalIdentityCount = ledger.TotalIdentityCount;
        float persistentSpikeHeight = persistentSpike.Height;
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathTicks);
        float spikerHeight = spiker.Height;
        SpikerState spikerState = spiker.State;

        game.RunOneStep();
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.Equal(spikerHeight, spiker.Height);
        Assert.Equal(spikerState, spiker.State);
        Assert.Equal(persistentSpikeHeight, persistentSpike.Height);

        for (int tick = 0; tick < TempestGame.PlayerDeathTicks - 1; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(4, game.Player.Lives);
        Assert.Empty(game.Spikers);
        Assert.Equal(1, game.Eggs.Count(candidate => candidate.OriginalSlotId == originalId));
        Assert.Equal(unresolvedOriginals, ledger.UnresolvedOriginalIds);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(1, game.Score.Score);
        Assert.Contains(persistentSpike, game.Spikes);
        Assert.Equal(persistentSpikeHeight, persistentSpike.Height);

        game.Player.ResetPosition(persistentSpike.EdgeIndex);
        for (int tick = 0; tick < 5; tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Contains(persistentSpike, game.Spikes);
        Assert.Equal(persistentSpikeHeight - 10, persistentSpike.Height);
        Assert.Equal(2, game.Score.Score);
    }

    [Fact]
    public void LevelFourTankerKeepsItsCargoPairAndFiringOutsideEnemyIdentityAccounting()
    {
        LevelConfiguration tankerConfiguration = LevelConfiguration.Level4Candidate with
        {
            InitialActiveEggCount = 1,
            ReserveReleaseIntervalTicks = 10_000
        };
        TempestGame game = new(tankerConfiguration, random: new TankerRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();

        Tanker tanker = Assert.Single(game.Tankers);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        int[] cargo = ledger.GetCargoIdsForOriginal(tanker.OriginalSlotId).ToArray();
        Assert.Equal(Level3OriginalType.Tanker,
            ledger.GetOriginalType(tanker.OriginalSlotId));
        Assert.Equal(Level3OriginalState.ActiveTanker,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.Equal(2, cargo.Length);
        Assert.Equal(new[] { 0, 1 }, cargo.Select(ledger.GetCargoOrdinal));
        Assert.All(cargo, id =>
        {
            Assert.Equal(tanker.OriginalSlotId, ledger.GetCargoOwner(id));
            Assert.Equal(Level3CargoState.Latent, ledger.GetCargoState(id));
        });
        Assert.Equal(19, ledger.TotalIdentityCount);

        bool sawBullet = false;
        VectorRenderer renderer = new(game);
        for (int tick = 0; tick < 240 && game.Tankers.Contains(tanker); tick++)
        {
            game.RunOneStep();
            if (game.EnemyBullets.FirstOrDefault() is EnemyBullet bullet)
            {
                sawBullet = true;
                AssertFinite(renderer.ProjectEnemyBullet(bullet));
            }
        }

        Assert.True(sawBullet);
        Assert.True(renderer.ShouldDrawEnemyBullets);
        Assert.DoesNotContain(tanker, game.Tankers);
        Assert.Equal(Level3OriginalState.Escaped,
            ledger.GetOriginalState(tanker.OriginalSlotId));
        Assert.Equal(19, ledger.TotalIdentityCount);
        Assert.All(cargo, id =>
            Assert.Equal(Level3CargoState.Active, ledger.GetCargoState(id)));
        Assert.Equal(2, game.Flippers.Count(flipper => flipper.IsTankerCargo));
        Assert.All(game.EnemyBullets, bullet =>
            Assert.InRange(bullet.EdgeIndex, 0, 15));
    }

    [Fact]
    public void SpikerRetreatBulletDoesNotCreateAnEnemyIdentity()
    {
        Level level = new(LevelConfiguration.Level4Candidate);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsSpiker(0);
        Spiker spiker = new(edgeIndex: 0, speed: 10, originalSlotId: 0);

        EnemyBullet? bullet = null;
        for (int tick = 0; tick < 30 && bullet is null; tick++)
            bullet = spiker.Update(level, () => true, (_, _) => 100);

        Assert.NotNull(bullet);
        Assert.Equal(0, bullet.EdgeIndex);
        Assert.Equal(SpikerState.Retreating, spiker.State);
        Assert.Equal(17, ledger.TotalIdentityCount);
        Assert.Equal(17, ledger.UnresolvedOriginalIds.Count);
    }

    [Fact]
    public void ProductionSpikerRetreatReturnsTheSameUnresolvedObligationToReserve()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();

        Spiker spiker = Assert.Single(game.Spikers);
        int originalId = spiker.OriginalSlotId;
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int tick = 0; tick < 335; tick++)
            game.RunOneStep();

        Assert.Empty(game.Spikers);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(originalId));
        Assert.Contains(originalId, ledger.UnresolvedOriginalIds);
        Assert.Equal(17, ledger.TotalIdentityCount);
        Assert.Equal(16, game.Spikes.Count);
    }

    [Fact]
    public void ReturnedSpikerIdentityReentersAsAnEligibleTypeWithoutNewOriginals()
    {
        Level3WaveLedger ledger = new(originalCount: 1);
        ledger.MarkOriginalEgg(0);
        ledger.CommitOriginalAsSpiker(0);
        HashSet<int> unresolvedBeforeRetreat = ledger.UnresolvedOriginalIds.ToHashSet();
        int originalCount = ledger.OriginalCount;
        int totalIdentityCount = ledger.TotalIdentityCount;

        ledger.ReturnSpikerToReserve(0);

        Assert.Equal(originalCount, ledger.OriginalCount);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(unresolvedBeforeRetreat, ledger.UnresolvedOriginalIds);
        Assert.Equal(Level3OriginalState.Reserve, ledger.GetOriginalState(0));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Assert.Empty(ledger.GetCargoIdsForOriginal(0));

        ledger.MarkOriginalEgg(0);
        Level3OriginalType reselectedType = Level3WaveSelection.SelectLevel4OriginalType(
            (minimum, _) => minimum == -6 ? -6 : 1,
            activeTankers: 0,
            activeSpikers: 0,
            activeEnemyUnits: 0,
            activeEggs: 1);
        Assert.Equal(Level3OriginalType.Flipper, reselectedType);
        ledger.CommitOriginalAsFlipper(0);

        Assert.Equal(originalCount, ledger.OriginalCount);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(unresolvedBeforeRetreat, ledger.UnresolvedOriginalIds);
        Assert.Equal(Level3OriginalType.Flipper, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.ActiveFlipper, ledger.GetOriginalState(0));
        Assert.Empty(ledger.GetCargoIdsForOriginal(0));
        ledger.ValidateState();
    }

    [Fact]
    public void SpikeExitShotsHitOnlyPersistentSpikesAndCanPreventExitCollision()
    {
        TempestGame game = new(level4Test: true, startingLives: 5, random: new MinimumRandom());
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        game.RunOneStep();

        Spiker spiker = Assert.Single(game.Spikers);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        HashSet<int> unresolvedOriginals = ledger.UnresolvedOriginalIds.ToHashSet();
        int totalIdentityCount = ledger.TotalIdentityCount;
        int destroyed = game.CurrentLevel.EnemiesDestroyed;
        float spikerHeight = spiker.Height;
        SpikerState spikerState = spiker.State;
        Egg[] eggs = game.Eggs.ToArray();
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == spiker.EdgeIndex);
        spike.Height = 90;
        game.Player.ResetPosition(spike.EdgeIndex);
        game.Player.AdvanceExit(300);
        game.CurrentLevel.State = LevelState.ExitZoom;

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Shot shot = Assert.Single(game.Shots);
        Assert.Equal(97, shot.Height);

        game.RunOneStep();
        Assert.Equal(91, shot.Height);
        Assert.Equal(90, spike.Height);
        game.RunOneStep();

        Assert.Equal(80, spike.Height);
        Assert.Equal(1, game.Score.Score);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(spikerHeight, spiker.Height);
        Assert.Equal(spikerState, spiker.State);
        Assert.Equal(eggs, game.Eggs);
        Assert.Equal(unresolvedOriginals, ledger.UnresolvedOriginalIds);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(destroyed, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void SpikeExitRepeatedHitsScoreOncePerTenAndRemoveTheDepletedSpike()
    {
        TempestGame game = new(level4Test: true, startingLives: 5, random: new MinimumRandom());
        Spike spike = game.Spikes[game.Player.EdgeIndex];
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        HashSet<int> unresolvedOriginals = ledger.UnresolvedOriginalIds.ToHashSet();
        int totalIdentityCount = ledger.TotalIdentityCount;
        game.CurrentLevel.State = LevelState.ExitZoom;
        Assert.False(game.ShouldShowAvoidSpikesWarning);

        for (int tick = 0; tick < 100 && game.Spikes.Contains(spike); tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.DoesNotContain(spike, game.Spikes);
        Assert.Equal(6, game.Score.Score);
        Assert.Equal(unresolvedOriginals, ledger.UnresolvedOriginalIds);
        Assert.Equal(totalIdentityCount, ledger.TotalIdentityCount);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void SpikeExitClearsShortSpikeBeforeContactWhileFireIsHeld()
    {
        TempestGame game = CreateResolvedLevelFourSpikeExit();
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        const int initialHeight = 60;
        spike.Height = initialHeight;
        game.CurrentLevel.State = LevelState.ExitZoom;
        float previousDepth = game.Player.Depth;
        int ticks = 0;

        while (game.Spikes.Contains(spike) && ticks < 200)
        {
            float previousOffset = Projection.ZOffset;
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

            float expectedTravel = previousOffset >= -40 ? 3 : 3.2f;
            Assert.Equal(MathF.Max(0, previousDepth - expectedTravel), game.Player.Depth, 3);
            Assert.True(game.Player.Depth < previousDepth);
            Assert.Equal(LevelState.ExitZoom, game.State);
            Assert.Equal(5, game.Player.Lives);

            previousDepth = game.Player.Depth;
            ticks++;
        }

        Assert.True(ticks < 200);
        Assert.DoesNotContain(spike, game.Spikes);
        Assert.Equal(initialHeight / 10, game.Score.Score);
        Assert.True(game.Player.Depth > initialHeight);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void SpikeExitContinuousFireCanErodeLongSpikeBeforeSafePassage()
    {
        TempestGame game = CreateResolvedLevelFourSpikeExit();
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 390;
        game.CurrentLevel.State = LevelState.ExitZoom;

        for (int tick = 0; tick < 200 && game.State == LevelState.ExitZoom; tick++)
            game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Equal(LevelState.Complete, game.State);
        Assert.DoesNotContain(spike, game.Spikes);
        Assert.True(game.Player.Depth < 10);
        Assert.True(game.Score.Score > 0);
        Assert.Equal(5, game.Player.Lives);
        Assert.Empty(game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
    }

    [Fact]
    public void SpikeReverseZoomDrawsBlasterEffectWithoutOrdinaryEnemyRetraction()
    {
        TempestGame game = CreateResolvedLevelFourSpikeExit();
        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 390;
        game.CurrentLevel.State = LevelState.ExitZoom;

        for (int tick = 0; tick < 200 && game.State == LevelState.ExitZoom; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathTicks);
        Assert.Equal(0, game.EnemyRetractionProgress);

        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawPlayerExplosion(game.CurrentLevel.Definition.Points,
            (start, end, color) => lines.Add((start, end, color)));
        Assert.NotEmpty(lines);

        game.RunOneStep();

        Assert.Equal(TempestGame.PlayerDeathTicks - 1, game.DeathTicks);
        Assert.Equal(0, game.EnemyRetractionProgress);
        lines.Clear();
        renderer.DrawPlayerExplosion(game.CurrentLevel.Definition.Points,
            (start, end, color) => lines.Add((start, end, color)));
        Assert.NotEmpty(lines);
        Assert.Equal(5, game.Player.Lives);
        Assert.Contains(spike, game.Spikes);
    }

    [Fact]
    public void SpikeExitCollisionWinsSameUpdateShotAndDoesNotHitAfterSpikeWasRemoved()
    {
        TempestGame raceGame = CreateResolvedLevelFourSpikeExit();
        Spike raceSpike = raceGame.Spikes.Single(
            candidate => candidate.EdgeIndex == raceGame.Player.EdgeIndex);
        raceSpike.Height = 390;
        raceGame.CurrentLevel.State = LevelState.ExitZoom;
        raceGame.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Shot travelingShot = Assert.Single(raceGame.Shots);
        travelingShot.Height = 391;
        raceGame.Player.AdvanceExit(7);

        raceGame.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Equal(LevelState.ReverseZoom, raceGame.State);
        Assert.Contains(raceSpike, raceGame.Spikes);
        Assert.Equal(390, raceSpike.Height);
        Assert.Equal(0, raceGame.Score.Score);
        Assert.Equal(391, travelingShot.Height);

        TempestGame clearedGame = CreateResolvedLevelFourSpikeExit();
        Spike clearedSpike = clearedGame.Spikes.Single(
            candidate => candidate.EdgeIndex == clearedGame.Player.EdgeIndex);
        clearedSpike.Height = 10;
        clearedGame.CurrentLevel.State = LevelState.ExitZoom;
        for (int tick = 0; tick < 100 && clearedGame.Spikes.Contains(clearedSpike); tick++)
            clearedGame.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.DoesNotContain(clearedSpike, clearedGame.Spikes);
        Assert.Equal(1, clearedGame.Score.Score);
        clearedGame.Player.AdvanceExit(clearedGame.Player.Depth - 11.5f);
        clearedGame.RunOneStep();

        Assert.Equal(LevelState.Complete, clearedGame.State);
        Assert.InRange(clearedGame.Player.Depth, 0, 10);
        Assert.DoesNotContain(clearedSpike, clearedGame.Spikes);
    }

    [Fact]
    public void AvoidSpikesAndExitDelaySuppressFireAndMovementStillWorks()
    {
        TempestGame game = new(level4Test: true, random: new MinimumRandom());
        Egg egg = game.Eggs[0];
        float eggHeight = egg.Height;
        int firingLane = game.Player.EdgeIndex;
        game.CurrentLevel.State = LevelState.AvoidSpikes;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: true));

        Assert.Empty(game.Shots);
        Assert.True(game.Player.EdgePosition > .5f);
        Assert.Equal(eggHeight, egg.Height);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.True(game.ShouldShowAvoidSpikesWarning);

        game.CurrentLevel.State = LevelState.ExitDelay;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Assert.Empty(game.Shots);
        Assert.Equal(firingLane, game.Player.EdgeIndex);
        Assert.Equal(eggHeight, egg.Height);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.True(game.ShouldShowAvoidSpikesWarning);
    }

    [Fact]
    public void AvoidSpikesTransitionPreservesAndAdvancesShotsAlreadyInFlight()
    {
        LevelConfiguration noInitialEggs =
            LevelConfiguration.Level4Candidate with { InitialActiveEggCount = 0 };
        TempestGame game = new(noInitialEggs, random: new MinimumRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalRimCleared(id);
        }

        Spike spike = game.Spikes.Single(candidate => candidate.EdgeIndex == game.Player.EdgeIndex);
        spike.Height = 390;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));

        Shot shot = Assert.Single(game.Shots);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(394, shot.Height);
        Assert.Equal(0, game.Score.Score);

        for (int tick = 0; tick < 5 && game.Shots.Contains(shot); tick++)
            game.RunOneStep();

        Assert.Empty(game.Shots);
        Assert.Equal(1, game.Score.Score);
        Assert.Equal(380, spike.Height);
    }

    [Fact]
    public void SpikeAvoidExitRetriesAfterCollisionWithoutRecreatingResolvedWave()
    {
        LevelConfiguration noInitialEggs =
            LevelConfiguration.Level4Candidate with { InitialActiveEggCount = 0 };
        TempestGame game = new(noInitialEggs, startingLives: 5, random: new MinimumRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalRimCleared(id);
        }

        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.True(game.SpikeExitControlsEnabled);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
        Assert.Equal(16, game.Spikes.Count);

        Spike damagedSpike = game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex);
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Assert.Empty(game.Shots);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(60, damagedSpike.Height);

        for (int tick = 0; tick < 151 && game.State == LevelState.AvoidSpikes; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);

        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Shot travelingShot = Assert.Single(game.Shots);
        Assert.Equal(397, travelingShot.Height);
        game.Player.Move(1, game.CurrentLevel.PlayableLaneCount);
        Spike exitCollisionSpike = game.Spikes.Single(
            spike => spike.EdgeIndex == game.Player.EdgeIndex);
        exitCollisionSpike.Height = 397;
        game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Same(travelingShot, Assert.Single(game.Shots));
        Assert.Equal(397, travelingShot.Height);
        Assert.True(game.ExitShotsVisible);
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: false));
        Assert.Same(travelingShot, Assert.Single(game.Shots));
        Assert.Equal(391, travelingShot.Height);

        for (int tick = 0; tick < 100 && game.State == LevelState.ReverseZoom; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(1, game.Score.Score);
        Assert.Equal(50, damagedSpike.Height);
        Assert.Equal(397, exitCollisionSpike.Height);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
        Assert.Equal(17, ledger.TotalIdentityCount);
        Assert.Equal(16, game.Spikes.Count);

        game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
        for (int tick = 0; tick < 151; tick++)
            game.RunOneStep();
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        for (int tick = 0; tick < 200 && !game.LevelComplete; tick++)
            game.RunOneStep();

        Assert.True(game.LevelComplete);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(1, game.Score.Score);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
        Assert.Equal(17, ledger.TotalIdentityCount);
        Assert.Equal(16, game.Spikes.Count);
        Assert.Equal(4, game.CurrentLevel.Configuration.DisplayedLevel);
    }

    [Fact]
    public void SpikeExitCollisionCausesGameOverWithoutRebuildingTheResolvedWave()
    {
        LevelConfiguration noInitialEggs =
            LevelConfiguration.Level4Candidate with { InitialActiveEggCount = 0 };
        TempestGame game = new(noInitialEggs, startingLives: 1, random: new MinimumRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalRimCleared(id);
        }

        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Spike lethalSpike = game.Spikes.Single(
            spike => spike.EdgeIndex == game.Player.EdgeIndex);
        lethalSpike.Height = 390;

        for (int tick = 0; tick < 151 && game.State == LevelState.AvoidSpikes; tick++)
            game.RunOneStep();
        for (int tick = 0; tick < 21 && game.State == LevelState.ExitDelay; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);

        for (int tick = 0; tick < 10 && game.State == LevelState.ExitZoom; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Contains(lethalSpike, game.Spikes);

        for (int tick = 0; tick < 600 && !game.GameOver; tick++)
            game.RunOneStep();

        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Assert.Contains(lethalSpike, game.Spikes);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
        Assert.Equal(17, ledger.TotalIdentityCount);
    }

    [Fact]
    public void LevelFourWavePickerOnlyReturnsEligibleEnemyTypesAndHonorsReferenceCaps()
    {
        Assert.Equal(Level3OriginalType.Spiker,
            Level3WaveSelection.SelectLevel4OriginalType((_, _) => -6,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 4, activeEggs: 0));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectLevel4OriginalType((_, _) => 1,
                activeTankers: 0, activeSpikers: 4, activeEnemyUnits: 5, activeEggs: 0));
        int pickerCalls = 0;
        Assert.Equal(Level3OriginalType.Tanker,
            Level3WaveSelection.SelectLevel4OriginalType((_, _) => ++pickerCalls == 1 ? 2 : 3,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 4, activeEggs: 0));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectLevel4OriginalType((_, _) => 1,
                activeTankers: 6, activeSpikers: 0, activeEnemyUnits: 5, activeEggs: 0));
    }

    [Fact]
    public void LevelFourSpikerEligibilityUsesStrictActiveEnemyAndEggBoundary()
    {
        static int Selection(int minimum, int _) => minimum == -6 ? -6 : 1;

        Assert.Equal(Level3OriginalType.Spiker,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 4, activeEggs: 0));
        Assert.Equal(Level3OriginalType.Spiker,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 3, activeEggs: 1));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 3, activeEggs: 0));
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0, activeEnemyUnits: 0, activeEggs: 3));

        Level levelWithReserve = new(LevelConfiguration.Level4Candidate with
        {
            InitialActiveEggCount = 1
        });
        Assert.Equal(16, levelWithReserve.ReserveEggs);
        Assert.Equal(Level3OriginalType.Flipper,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0,
                activeEnemyUnits: 0, activeEggs: levelWithReserve.Eggs.Count));

        Egg approachingEgg = levelWithReserve.Eggs[0];
        approachingEgg.BeginApproach();
        Assert.Single(levelWithReserve.Eggs);
        Assert.Equal(Level3OriginalType.Spiker,
            Level3WaveSelection.SelectLevel4OriginalType(Selection,
                activeTankers: 0, activeSpikers: 0,
                activeEnemyUnits: 3, activeEggs: levelWithReserve.Eggs.Count));
    }

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private static TempestGame CreateResolvedLevelFourSpikeExit()
    {
        LevelConfiguration noInitialEggs =
            LevelConfiguration.Level4Candidate with { InitialActiveEggCount = 0 };
        TempestGame game = new(noInitialEggs, startingLives: 5, random: new MinimumRandom());
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int id = 0; id < ledger.OriginalCount; id++)
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalRimCleared(id);
        }

        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        for (int tick = 0; tick < 151; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.ExitZoom, game.State);
        return game;
    }

    private static bool IsInsideOrOnPolygon(Vector2 point, IReadOnlyList<Vector2> polygon)
    {
        bool inside = false;
        for (int index = 0, previous = polygon.Count - 1;
             index < polygon.Count;
             previous = index++)
        {
            Vector2 start = polygon[previous];
            Vector2 end = polygon[index];
            Vector2 edge = end - start;
            Vector2 relative = point - start;
            float cross = edge.X * relative.Y - edge.Y * relative.X;
            if (MathF.Abs(cross) < 0.001f &&
                Vector2.Dot(relative, edge) >= 0 &&
                Vector2.Dot(relative, edge) <= edge.LengthSquared())
                return true;

            if ((start.Y > point.Y) != (end.Y > point.Y) &&
                point.X < (end.X - start.X) * (point.Y - start.Y) /
                (end.Y - start.Y) + start.X)
                inside = !inside;
        }

        return inside;
    }

    private static List<(Vector2 Start, Vector2 End)> CaptureSpikes(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawSpikes((start, end) => lines.Add((start, end)), _ => { });
        return lines;
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
        public override double NextDouble() => 0;
    }

    private sealed class TankerRandom : Random
    {
        public override int Next(int minValue, int maxValue)
        {
            if (minValue == -6 && maxValue == 5)
                return 4;
            if (minValue == 1 && maxValue == 5)
                return 4;
            if (minValue == 0 && maxValue == 101)
                return 100;
            return minValue;
        }
    }
}
