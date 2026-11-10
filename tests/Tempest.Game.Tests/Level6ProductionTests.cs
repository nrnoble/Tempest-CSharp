using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level6ProductionTests
{
    private static readonly Vector2[] TrianglePoints =
    [
        new(-116, 145), new(-171, 145), new(-137, 72), new(-102, 0),
        new(-68, -72), new(-34, -145), new(0, -218), new(34, -145),
        new(68, -72), new(102, 0), new(137, 72), new(171, 145),
        new(116, 145), new(54, 145), new(0, 145), new(-54, 145)
    ];

    [Fact]
    public void ProductionConfigurationUsesTheOriginalArcadeTriangleAndAcceptedWaveSystems()
    {
        LevelConfiguration configuration = LevelConfiguration.Level6Production;
        Level level = new(configuration, randomInclusive: (_, _) => 0);

        Assert.Equal(5, configuration.ProgressionIndex);
        Assert.Equal(6, configuration.DisplayedLevel);
        Assert.Equal(5, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(360, configuration.Definition.CenterY);
        Assert.Equal(-40, configuration.Definition.YOffset);
        Assert.Equal(TrianglePoints, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(Level.Depth, configuration.Depth);
        Assert.Equal(Level.Position, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(22, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(18, level.ReserveEggs);
        Assert.Equal(22, level.Level3Ledger!.OriginalCount);
        Assert.Equal(22, level.Level3Ledger.TotalIdentityCount);
        Assert.Equal(22, level.Level3Ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(0, level.Level3Ledger.CargoCount);
        Assert.Equal(16, level.Spikes.Count);
        Assert.All(level.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, configuration.ExitPolicy);
        Assert.Equal(70, configuration.AvoidSpikesWaitThreshold);
        Assert.Equal(LevelEnemyComposition.FlippersTankersAndSpikers,
            configuration.NormalEnemyComposition);
        Assert.True(configuration.Flipper.CanFlip);
        Assert.Equal(2, configuration.Flipper.ClimbSpeed);
        Assert.Equal(9, configuration.Flipper.TurnSpeedDegrees);
        Assert.Equal(20, configuration.Flipper.LanePauseTicks);
        Assert.Equal(23, configuration.Flipper.RimPauseTicks);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge, configuration.FlipperPivotMode);
    }

    [Fact]
    public void LevelSixLifeLossRetainsConfigurationAccountingScoreLivesAndSpikeDamage()
    {
        TempestGame game = new(LevelConfiguration.Level6Production, startingLives: 5);
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        Spike damagedSpike = game.Spikes.Single(spike => spike.EdgeIndex == 7);
        damagedSpike.Height = 45;
        game.Score.Add(325);
        game.Player.Lives = 2;
        Assert.True(game.SuperZapper.TryActivate([], game.Player.EdgeIndex, 16, continuous: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        level.State = LevelState.PlayerDying;

        game.RunOneStep();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level6Production, level.Configuration);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(1, game.Player.Lives);
        Assert.Equal(325, game.Score.Score);
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(22, ledger.UnresolvedOriginalIds.Count);
        Assert.Equal(0, level.EnemiesDestroyed);
        Assert.Equal(22, level.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(18, level.ReserveEggs);
        Assert.Same(damagedSpike, Assert.Single(game.Spikes, spike => spike.EdgeIndex == 7));
        Assert.Equal(45, damagedSpike.Height);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
    }

    [Fact]
    public void LevelSixReconstructionClearsSpeciesAndLatentCargoWhilePreservingSpikes()
    {
        Level level = new(LevelConfiguration.Level6Production, randomInclusive: (_, _) => 0);
        Level3WaveLedger ledger = level.Level3Ledger!;
        ledger.CommitOriginalAsFlipper(0);
        ledger.CommitOriginalAsTanker(1);
        ledger.CommitOriginalAsSpiker(2);
        level.ReturnOriginalToReserve(2);
        Spike persistentSpike = level.Spikes[7];
        persistentSpike.Height = 45;
        ledger.ValidateState();

        level.ResetAfterDeath();

        Assert.Equal(22, ledger.OriginalCount);
        Assert.Equal(0, ledger.CargoCount);
        Assert.Equal(22, ledger.TotalIdentityCount);
        Assert.Equal(22, ledger.UnresolvedOriginalIds.Count);
        Assert.Empty(ledger.UnresolvedCargoIds);
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(0));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(0));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(1));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(1));
        Assert.Equal(Level3OriginalType.Unselected, ledger.GetOriginalType(2));
        Assert.Equal(Level3OriginalState.Egg, ledger.GetOriginalState(2));
        Assert.Empty(ledger.GetCargoIdsForOriginal(1));
        Assert.Equal(0, level.EnemiesDestroyed);
        Assert.Equal(22, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(18, level.ReserveEggs);
        Assert.Same(persistentSpike, Assert.Single(level.Spikes, spike => spike.EdgeIndex == 7));
        Assert.Equal(45, persistentSpike.Height);
        ledger.ValidateState();
    }

    [Fact]
    public void DirectLevelSixAvoidSpikesWaitUsesExactly71Updates()
    {
        TempestGame game = new(level6Test: true);
        Level level = game.CurrentLevel;
        Level3WaveLedger ledger = level.Level3Ledger!;
        foreach (int id in ledger.UnresolvedOriginalIds.ToArray())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
            level.RecordEnemyDestroyed();
        }

        level.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);

        for (int tick = 0; tick < 70; tick++)
        {
            game.RunOneStep();
            Assert.Equal(LevelState.AvoidSpikes, game.State);
        }

        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Same(level, game.CurrentLevel);
        Assert.False(game.NaturalProgressionEnabled);
    }
}
