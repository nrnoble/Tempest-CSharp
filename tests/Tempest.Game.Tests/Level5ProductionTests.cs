using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level5ProductionTests
{
    private static readonly Vector2[] CrossPoints =
    [
        new(-30, 210), new(-60, 120), new(-120, 60), new(-210, 30),
        new(-210, -30), new(-120, -60), new(-60, -120), new(-30, -210),
        new(30, -210), new(60, -120), new(120, -60), new(210, -30),
        new(210, 30), new(120, 60), new(60, 120), new(30, 210)
    ];

    [Fact]
    public void ProductionConfigurationUsesTheOriginalArcadeCrossAndAcceptedWaveSystems()
    {
        LevelConfiguration configuration = LevelConfiguration.Level5Production;
        Level level = new(configuration, randomInclusive: (_, _) => 0);
        TempestGame game = new(configuration);

        Assert.Equal(4, configuration.ProgressionIndex);
        Assert.Equal(5, configuration.DisplayedLevel);
        Assert.Equal(4, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(415, configuration.Definition.CenterY);
        Assert.Equal(-90, configuration.Definition.YOffset);
        Assert.Equal(CrossPoints, configuration.Definition.Points);
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
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.True(configuration.FlipperCanFlip);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Fuseballs);
        Assert.Null(LevelProgression.GetNextProductionLevel(configuration));
    }

    [Fact]
    public void LevelFiveLedgerRebuildPreservesDestroyedAndCargoIdentitiesWithoutDuplication()
    {
        Level level = new(LevelConfiguration.Level5Production, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;

        ledger.CommitOriginalAsFlipper(0);
        ledger.MarkOriginalDestroyed(0);
        level.RecordEnemyDestroyed();

        ledger.CommitOriginalAsTanker(1);
        IReadOnlyList<int> cargoIds = ledger.MaterializeLiveCargo(1);
        ledger.MarkCargoDestroyed(cargoIds[0]);

        ledger.CommitOriginalAsSpiker(2);
        level.ReturnOriginalToReserve(2);
        Spike persistentSpike = level.Spikes[7];
        persistentSpike.Height = 45;
        ledger.ValidateState();

        level.ResetAfterDeath();

        Assert.Equal(20, ledger.OriginalCount);
        Assert.Equal(2, ledger.CargoCount);
        Assert.Equal(22, ledger.TotalIdentityCount);
        Assert.Equal(19, ledger.UnresolvedOriginalIds.Count);
        Assert.Single(ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(0));
        Assert.Equal(Level3CargoState.Destroyed, ledger.GetCargoState(cargoIds[0]));
        Assert.Contains(ledger.GetCargoState(cargoIds[1]),
            new[] { Level3CargoState.ReentryReserve, Level3CargoState.Egg });
        Assert.Equal(
            ledger.GetCargoState(cargoIds[1]) == Level3CargoState.ReentryReserve ? 1 : 0,
            level.Level3CargoReserveEggs);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(2));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(2));
        Assert.Equal(1, level.EnemiesDestroyed);
        Assert.Equal(19, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Same(persistentSpike, Assert.Single(level.Spikes, spike => spike.EdgeIndex == 7));
        Assert.Equal(45, persistentSpike.Height);
        ledger.ValidateState();
    }

    [Fact]
    public void LevelFiveLifeLossPreservesScoreLivesSpikeAndRemainingZapperCharge()
    {
        TempestGame game = new(LevelConfiguration.Level5Production, startingLives: 5);
        Level level = game.CurrentLevel;
        Spike[] spikes = game.Spikes.ToArray();
        spikes[game.Player.EdgeIndex].Height = 45;
        game.Score.Add(325);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        level.State = LevelState.PlayerDying;

        game.RunOneStep();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level5Production, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.Player.Lives);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(20, level.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(16, level.ReserveEggs);
        Assert.Equal(spikes, game.Spikes);
        Assert.Equal(45, game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height);
    }
}
