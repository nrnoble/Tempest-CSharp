using System.Numerics;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level7ProductionTests
{
    private static readonly Vector2[] CloverPoints =
    [
        new(-160, 160), new(-179, 44), new(-89, 0), new(-179, -44),
        new(-160, -160), new(-44, -179), new(0, -89), new(44, -179),
        new(160, -160), new(179, -44), new(89, 0), new(179, 44),
        new(160, 160), new(44, 179), new(0, 89), new(-44, 179)
    ];

    [Fact]
    public void ProductionConfigurationUsesTheOriginalArcadeCloverAndApprovedWaveSystems()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production;
        Level level = new(configuration, randomInclusive: (_, _) => 0);

        Assert.Equal(6, configuration.ProgressionIndex);
        Assert.Equal(7, configuration.DisplayedLevel);
        Assert.Equal(6, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(310, configuration.Definition.CenterY);
        Assert.Equal(-10, configuration.Definition.YOffset);
        Assert.Equal(CloverPoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(Level.Depth, configuration.Depth);
        Assert.Equal(Level.Position, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(20, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Equal(20, level.Level3Ledger!.OriginalCount);
        Assert.Equal(20, level.Level3Ledger.TotalIdentityCount);
        Assert.Equal(20, level.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(0, level.Level3Ledger.CargoCount);
        Assert.Equal(16, level.Spikes.Count);
        Assert.All(level.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, configuration.ExitPolicy);
        Assert.Equal(30, configuration.AvoidSpikesWaitThreshold);
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.Equal(62, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(3, configuration.MaximumActiveEnemyShots);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.Equal(BelowRimFlipperProgram.Level7ArcadeObserved, configuration.BelowRimFlipperProgram);
        Assert.True(configuration.UsesLevel7RomWavePolicy);
        Assert.Equal(1, configuration.ProgrammedFlipperInitialDirection);
        Assert.Equal(new FlipperConfiguration(true, 2, 9, 0, 23), configuration.Flipper);
    }

    [Fact]
    public void DeterministicLevel7SelectionsNeverCreateOrdinaryBottomFlippers()
    {
        for (int seed = 0; seed < 32; seed++)
        {
            TempestGame game = new(
                LevelConfiguration.Level7Production with
                {
                    EnemySupply = Level7WaveSelection.MaximumActiveEnemies,
                    InitialActiveEggCount = Level7WaveSelection.MaximumActiveEnemies
                },
                random: new Random(seed));
            foreach (Egg egg in game.Eggs)
                MakeReady(egg, game.CurrentLevel);

            game.RunOneStep();

            Assert.DoesNotContain(game.Flippers, flipper => !flipper.IsTankerCargo);
            Assert.InRange(game.Tankers.Count, 1, 2);
            Assert.InRange(game.Spikers.Count, 2, 4);
            Assert.True(game.Tankers.Count + game.Spikers.Count + game.Flippers.Count <=
                Level7WaveSelection.MaximumActiveEnemies);
            Assert.Equal(4, game.CurrentLevel.Level3Ledger!.CargoCount);
            Assert.Equal(Level7WaveSelection.MaximumActiveEnemies,
                game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds.Count);
            Assert.Equal(4, game.TankerLatentCargoUnits);
            Assert.Equal(0, game.CurrentLevel.PendingGenericEnemyUnits);
        }
    }

    [Fact]
    public void OneUnitDeathReconstructionClearsSpeciesAndSelectsTheLevel7Spiker()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1
        };
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg originalEgg = Assert.Single(level.Eggs);
        ledger.CommitOriginalAsFlipper(originalEgg.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Single(ledger.UnresolvedOriginalIds);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Assert.Equal(1, level.PendingGenericEnemyUnits);
        Egg reentryEgg = Assert.Single(level.Eggs);
        MakeReady(reentryEgg, level);
        Spiker? reconstructed = null;
        level.HatchReadyMixedEnemies(egg =>
        {
            Level3OriginalType type = Level7WaveSelection.SelectOriginalType(
                (_, _) => 0, activeTankers: 0,
                activeSpikers: 0, activeEnemyUnits: 0)
                ?? throw new InvalidOperationException("The Level 7 one-unit selection was unavailable.");
            Assert.Equal(Level3OriginalType.Spiker, type);
            ledger.CommitOriginalAsSpiker(egg.OriginalSlotId);
            reconstructed = new Spiker(egg.EdgeIndex, originalSlotId: egg.OriginalSlotId);
            return reconstructed;
        }, _ => { });

        Assert.NotNull(reconstructed);
        Assert.Empty(level.Eggs);
        Assert.Equal(0, level.PendingGenericEnemyUnits);
        Assert.Equal(Level3OriginalType.Spiker, ledger.GetOriginalType(0));
        Assert.Single(ledger.UnresolvedOriginalIds);
    }

    [Fact]
    public void ReleasedTankerCargoReturnsAsGenericPendingSupplyAfterLevel7Death()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            EnemySupply = 2,
            InitialActiveEggCount = 2,
            MaximumActiveEggs = 2
        };
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        Egg tankerEgg = level.Eggs[0];
        ledger.CommitOriginalAsTanker(tankerEgg.OriginalSlotId);
        int[] cargoIds = ledger.MaterializeLiveCargo(tankerEgg.OriginalSlotId).ToArray();
        ledger.MarkOriginalEscaped(tankerEgg.OriginalSlotId);

        level.ResetAfterDeath(clearUnresolvedSpeciesSelections: true);

        Assert.Equal(3, level.PendingGenericEnemyUnits);
        Assert.Equal(3, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.All(ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(id)));
        Assert.All(cargoIds,
            id => Assert.Equal(Level3CargoState.Reconstructed, ledger.GetCargoState(id)));
    }

    [Fact]
    public void FinalPendingSpikerConvertsInPlaceToFlipperCarryingTanker()
    {
        LevelConfiguration configuration = LevelConfiguration.Level7Production with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1
        };
        TempestGame game = new(configuration, random: new Level7FiringRandom());
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);
        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        int originalId = spiker.OriginalSlotId;
        int edge = spiker.EdgeIndex;

        for (int tick = 0; tick < 500 && game.Tankers.Count == 0; tick++)
            game.RunOneStep();

        Tanker converted = Assert.Single(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Equal(originalId, converted.OriginalSlotId);
        Assert.Equal(edge, converted.EdgeIndex);
        Assert.Equal(10, converted.Height);
        Assert.Equal(Level3OriginalType.Tanker,
            game.CurrentLevel.Level3Ledger!.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveTanker,
            game.CurrentLevel.Level3Ledger.GetOriginalState(originalId));
        Assert.Equal(2, game.CurrentLevel.Level3Ledger.GetCargoIdsForOriginal(originalId).Count);
        Assert.Single(game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds);
        Assert.Equal(0, game.CurrentLevel.ReserveEggs);
        Assert.Equal(0, game.CurrentLevel.PendingGenericEnemyUnits);
        Assert.Equal(2, game.TankerLatentCargoUnits);
    }

    [Fact]
    public void Level7DeathPreservesResolvedStateScoreLivesZapperAndDamagedSpikes()
    {
        TempestGame game = new(LevelConfiguration.Level7Production,
            startingLives: 5, random: new Level7FiringRandom());
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Spiker resolvedSpiker = game.Spikers[0];
        int resolvedId = resolvedSpiker.OriginalSlotId;
        Assert.True(resolvedSpiker.TryZap(1));
        for (int tick = 0; tick < 4 && game.Spikers.Contains(resolvedSpiker); tick++)
            game.RunOneStep();
        Assert.DoesNotContain(game.Spikers, spiker => spiker.OriginalSlotId == resolvedId);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger!.GetOriginalState(resolvedId));

        game.Score.Add(1234);
        game.Spikes[0].Height = 35;
        Assert.True(game.SuperZapper.TryActivate(
            game.Flippers.Cast<Enemy>().Concat(game.Tankers).Concat(game.Spikers),
            game.Player.EdgeIndex, game.CurrentLevel.PlayableLaneCount, continuous: true));
        int charges = game.SuperZapperChargesRemaining;
        int flashTicks = game.SuperZapper.FlashTicksRemaining;
        int lives = game.Player.Lives;
        int unresolved = game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count;
        int score = game.Score.Score;
        game.CurrentLevel.State = LevelState.PlayerDying;
        game.RunOneStep();

        Assert.Equal(score, game.Score.Score);
        Assert.Equal(lives - 1, game.Player.Lives);
        Assert.Equal(charges, game.SuperZapperChargesRemaining);
        Assert.Equal(flashTicks - 1, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(35, game.Spikes[0].Height);
        Assert.Equal(unresolved, game.CurrentLevel.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(Level3OriginalState.Destroyed,
            game.CurrentLevel.Level3Ledger.GetOriginalState(resolvedId));
        Assert.All(game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds,
            id => Assert.Equal(Level3OriginalType.Unselected,
                game.CurrentLevel.Level3Ledger.GetOriginalType(id)));
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Flippers);
    }

    [Fact]
    public void Level7CompositionUsesOnlyFlippersTankersAndSpikers()
    {
        foreach (int candidate in new[] { 0, 1 })
        {
            Level3OriginalType? selected = Level7WaveSelection.SelectOriginalType(
                (_, _) => candidate, activeTankers: 1,
                activeSpikers: 2, activeEnemyUnits: 3);
            Assert.NotNull(selected);
            Assert.Contains(selected.Value, new[]
            {
                Level3OriginalType.Tanker,
                Level3OriginalType.Spiker
            });
        }
        Assert.Equal(Level3OriginalType.Spiker,
            Level7WaveSelection.SelectOriginalType(
                (_, _) => throw new InvalidOperationException(),
                activeTankers: 0, activeSpikers: 0,
                activeEnemyUnits: 0));
        Assert.Equal(Level3OriginalType.Tanker,
            Level7WaveSelection.SelectOriginalType(
                (_, _) => throw new InvalidOperationException(),
                activeTankers: 0, activeSpikers: 2,
                activeEnemyUnits: 2));
        Assert.Null(Level7WaveSelection.SelectOriginalType(
            (_, _) => throw new InvalidOperationException(),
            activeTankers: 1, activeSpikers: 4,
            activeEnemyUnits: Level7WaveSelection.MaximumActiveEnemies));
    }

    [Fact]
    public void CloverSupportsEveryDirectedAdjacentTransitionAndPreservesReflexSweeps()
    {
        Level level = new(LevelConfiguration.Level7Production);
        Vector2[] points = level.Definition.Points.ToArray();
        int[] reflexHinges = [2, 6, 10, 14];

        Assert.Equal(15, level.PreviousEdge(0));
        Assert.Equal(0, level.NextEdge(15));

        for (int source = 0; source < level.PlayableLaneCount; source++)
        {
            foreach (int direction in new[] { 1, -1 })
            {
                int destination = direction > 0
                    ? level.PreviousEdge(source)
                    : level.NextEdge(source);
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(source, destination, direction);

                Assert.Equal(source, transition.SourceLane);
                Assert.Equal(destination, transition.DestinationLane);
                Assert.Equal(direction, transition.Direction);
                Assert.True(float.IsFinite(transition.SweepMagnitudeDegrees));
                Assert.True(transition.SweepMagnitudeDegrees > 0);
                Assert.Equal(reflexHinges.Contains(transition.HingeVertex),
                    transition.SweepMagnitudeDegrees > 180);

                foreach (float progress in new[] { 0f, .25f, .5f, .75f, 1f })
                {
                    (Vector2 start, Vector2 end) =
                        level.CalculateFlipperPivotEndpoints(transition, progress);
                    AssertFinite(start);
                    AssertFinite(end);
                    Assert.True(IsInsideOrOnPolygon(start, points),
                        $"Start endpoint escaped at lane {source}, direction {direction}, progress {progress}.");
                    Assert.True(IsInsideOrOnPolygon(end, points),
                        $"End endpoint escaped at lane {source}, direction {direction}, progress {progress}.");
                }
            }
        }
    }

    [Fact]
    public void ProductionProgressionStopsAtTerminalLevel7AndEarlierLevelsKeepTheirRoute()
    {
        Assert.Same(LevelConfiguration.Level2Square,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level1));
        Assert.Same(LevelConfiguration.Level3Mixed,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level2Square));
        Assert.Same(LevelConfiguration.Level4Candidate,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));
        Assert.Same(LevelConfiguration.Level5Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level4Candidate));
        Assert.Same(LevelConfiguration.Level6Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level5Production));
        Assert.Same(LevelConfiguration.Level7Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level6Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level7Production));

        foreach (LevelConfiguration configuration in new[]
                 {
                     LevelConfiguration.Level1,
                     LevelConfiguration.Level2Square,
                     LevelConfiguration.Level3Mixed,
                     LevelConfiguration.Level4Candidate,
                     LevelConfiguration.Level6Production
                 })
            Assert.Equal(BelowRimFlipperProgram.Existing, configuration.BelowRimFlipperProgram);
        Assert.Equal(BelowRimFlipperProgram.Level5SpikeSensitive,
            LevelConfiguration.Level5Production.BelowRimFlipperProgram);

        Assert.Equal(150, LevelConfiguration.Level4Candidate.AvoidSpikesWaitThreshold);
        Assert.Equal(150, LevelConfiguration.Level5Production.AvoidSpikesWaitThreshold);
        Assert.Equal(70, LevelConfiguration.Level6Production.AvoidSpikesWaitThreshold);
        Assert.Equal(30, LevelConfiguration.Level7Production.AvoidSpikesWaitThreshold);
        Assert.True(LevelConfiguration.Level7Production.UsesLevel7RomWavePolicy);
        Assert.All(new[]
        {
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production
        }, configuration => Assert.False(configuration.UsesLevel7RomWavePolicy));
    }

    [Fact]
    public void DirectLevel7DiagnosticUsesProductionConfigurationFiringAndFreshReplay()
    {
        string[] arguments = ["--level7-test", "--lives", "10"];
        TempestGame game = new(level7Test: DebugController.UseLevel7Test(arguments),
            startingLives: DebugController.GetStartingLives(arguments),
            random: new Level7FiringRandom());

        Assert.True(DebugController.UseLevel7Test(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.True(DebugController.HasConflictingScenarios(
            ["--level7-test", "--level6-test"]));
        Assert.Throws<ArgumentException>(() =>
            new TempestGame(level6Test: true, level7Test: true));
        Assert.Equal(10, game.Player.Lives);
        Assert.True(game.Level7Test);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);

        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);
        game.RunOneStep();

        Assert.Empty(game.Flippers);
        Assert.Equal(3, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Assert.Empty(game.EnemyBullets);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(BelowRimFlipperProgram.Level7ArcadeObserved,
            game.CurrentLevel.Configuration.BelowRimFlipperProgram);

        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        AssertFreshLevel7(game);

        game.Score.Add(100);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
        AssertFreshLevel7(game);
    }

    private static void AssertFreshLevel7(TempestGame game)
    {
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(20, game.CurrentLevel.Level3Ledger!.OriginalCount);
        Assert.Equal(20, game.CurrentLevel.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, game.CurrentLevel.ReserveEggs);
        Assert.Equal(16, game.Spikes.Count);
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static bool IsInsideOrOnPolygon(Vector2 point, IReadOnlyList<Vector2> polygon)
    {
        bool inside = false;
        for (int index = 0, previous = polygon.Count - 1;
             index < polygon.Count;
             previous = index++)
        {
            Vector2 start = polygon[index];
            Vector2 end = polygon[previous];
            Vector2 edge = end - start;
            Vector2 offset = point - start;
            float cross = edge.X * offset.Y - edge.Y * offset.X;
            if (MathF.Abs(cross) < .001f &&
                point.X >= MathF.Min(start.X, end.X) - .001f &&
                point.X <= MathF.Max(start.X, end.X) + .001f &&
                point.Y >= MathF.Min(start.Y, end.Y) - .001f &&
                point.Y <= MathF.Max(start.Y, end.Y) + .001f)
                return true;

            if ((start.Y > point.Y) != (end.Y > point.Y) &&
                point.X < (end.X - start.X) * (point.Y - start.Y) /
                (end.Y - start.Y) + start.X)
                inside = !inside;
        }
        return inside;
    }

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }

    private sealed class Level7FiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue == 5 ? 1 :
            minValue == 0 && maxValue == 256 ? 255 :
            minValue;
    }
}
