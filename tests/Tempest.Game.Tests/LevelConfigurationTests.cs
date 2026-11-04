using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class LevelConfigurationTests
{
    [Fact]
    public void Level1ConfigurationDefinesIdentityAndExistingClosedGeometry()
    {
        LevelConfiguration configuration = LevelConfiguration.Level1;

        Assert.Equal(0, configuration.ProgressionIndex);
        Assert.Equal(1, configuration.DisplayedLevel);
        Assert.Equal(0, configuration.BoardIndex);
        Assert.Equal(0, configuration.PaletteIndex);
        Assert.True(configuration.Definition.Continuous);
        Assert.Equal(400, configuration.Definition.CenterY);
        Assert.Equal(-80, configuration.Definition.YOffset);
        Assert.Equal(new[]
        {
            new Vector2(200, 0), new Vector2(184, 76), new Vector2(141, 141), new Vector2(76, 184),
            new Vector2(0, 200), new Vector2(-76, 184), new Vector2(-141, 141), new Vector2(-184, 76),
            new Vector2(-200, 0), new Vector2(-184, -76), new Vector2(-141, -141), new Vector2(-76, -184),
            new Vector2(0, -200), new Vector2(76, -184), new Vector2(141, -141), new Vector2(184, -76)
        }, configuration.Definition.Points);
        Assert.Equal(16, configuration.VertexCount);
        Assert.Equal(16, configuration.PlayableLaneCount);
        Assert.Equal(LevelExitPolicy.Standard, configuration.ExitPolicy);
        Assert.Equal(LevelEnemyComposition.FlippersOnly, configuration.NormalEnemyComposition);
    }

    [Fact]
    public void OpenDefinitionFixtureHasOneFewerPlayableLaneThanVertices()
    {
        LevelDefinition openDefinition = LevelConfiguration.Level1.Definition with { Continuous = false };

        Assert.Equal(16, openDefinition.VertexCount);
        Assert.Equal(15, openDefinition.PlayableLaneCount);
    }

    [Fact]
    public void Level1ConfigurationPreservesSupplyEggPacingAndDeterministicSequences()
    {
        LevelConfiguration configuration = LevelConfiguration.Level1;
        Level level = new(configuration);

        Assert.Equal(400, configuration.Depth);
        Assert.Equal(50, configuration.Position);
        Assert.Equal(7, configuration.InitialPlayerLane);
        Assert.Equal(0.5f, configuration.InitialPlayerLanePosition);
        Assert.Equal(10, configuration.EnemySupply);
        Assert.Equal(4, configuration.InitialActiveEggCount);
        Assert.Equal(50, configuration.ReserveReleaseIntervalTicks);
        Assert.Equal(4, configuration.MaximumActiveEggs);
        Assert.Equal(2, configuration.MaximumApproachingEggs);
        Assert.Equal(50, configuration.HatchSelectionIntervalTicks);
        Assert.Equal(new[] { 12, 4, 9, 1, 14, 6, 10, 2, 15, 7 }, configuration.SpawnLaneSequence);
        Assert.Equal(new[] { 1, -1 }, configuration.EggDirectionSequence);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(6, level.ReserveEggs);
        Assert.Equal(new[] { 12, 4, 9, 1 }, level.Eggs.Select(egg => egg.EdgeIndex));
        Assert.Equal(new[] { 1, -1, 1, -1 }, level.Eggs.Select(egg => egg.MovementDirection));
    }

    [Fact]
    public void DefaultLevelConstructionUsesExplicitLevel1Configuration()
    {
        Level level = new();

        Assert.Same(LevelConfiguration.Level1, level.Configuration);
        Assert.Equal(10, level.EnemySupply);
    }

    [Fact]
    public void ConfiguredSupplyDrivesLevelCompletionAndAccounting()
    {
        LevelConfiguration configuration = LevelConfiguration.Level1 with { EnemySupply = 6 };
        Level level = new(configuration);

        Assert.Equal(6, level.RemainingEnemies);
        Assert.Equal(4, level.Eggs.Count);
        Assert.Equal(2, level.ReserveEggs);

        for (int enemy = 0; enemy < configuration.EnemySupply; enemy++)
            level.RecordEnemyDestroyed();
        level.ResetAfterDeath();

        Assert.Equal(configuration.EnemySupply, level.EnemiesDestroyed);
        Assert.Equal(0, level.ReserveEggs);
        Assert.True(level.IsComplete(activeFlippers: 0));
    }

    [Fact]
    public void SuccessfulExitInstallsLevel2AndPreservesScoreAndLives()
    {
        TempestGame game = new();
        Level previousLevel = game.CurrentLevel;
        game.Score.Add(1500);
        game.Player.Lives = 2;
        CompleteLevel1Wave(game);

        for (int tick = 0; tick < 200 &&
             !ReferenceEquals(game.CurrentLevel.Configuration, LevelConfiguration.Level2Square); tick++)
            game.RunOneStep();

        Assert.NotSame(previousLevel, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(1500, game.Score.Score);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        Assert.Equal(400, game.Player.Depth);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }

    [Fact]
    public void NaturalLevel2UsesTheArcadeFlipperFiringPhase()
    {
        TempestGame game = new(random: new MinimumRandom());
        CompleteLevel1Wave(game);
        for (int tick = 0; tick < 200 &&
             !ReferenceEquals(game.CurrentLevel.Configuration, LevelConfiguration.Level2Square); tick++)
            game.RunOneStep();

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();

        Assert.False(game.EnemyBulletsTest);
        Assert.Equal(77, Assert.Single(game.Flippers).ShotDelayTicks);
        Assert.Single(game.EnemyBullets);
    }

    [Fact]
    public void GameOverRestartReplacesLevelAndResetsScoreAndStartingLives()
    {
        TempestGame game = new(startingLives: 5);
        Level previousLevel = game.CurrentLevel;
        game.Score.Add(1500);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;

        game.RunOneStep(restartPressed: true);

        Assert.NotSame(previousLevel, game.CurrentLevel);
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(6, game.CurrentLevel.ReserveEggs);
    }

    [Fact]
    public void LifeReconstructionRetainsLevelAndResolvedOutcomes()
    {
        TempestGame game = new();
        Level level = game.CurrentLevel;
        level.RecordEnemyDestroyed();
        game.Score.Add(150);

        level.ResetAfterDeath();

        Assert.Same(level, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level1, game.CurrentLevel.Configuration);
        Assert.Equal(1, level.EnemiesDestroyed);
        Assert.Equal(9, level.RemainingEnemies);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(5, level.ReserveEggs);
        Assert.Equal(150, game.Score.Score);
    }

    [Fact]
    public void RepeatedSpikerReplayRebindsOnlyTheCurrentSpikeCallback()
    {
        TempestGame game = new(spikerDevelopmentWave: true);

        for (int replay = 0; replay < 3; replay++)
        {
            Level previousLevel = game.CurrentLevel;
            previousLevel.State = LevelState.Complete;
            game.RunOneStep(restartPressed: true);

            Assert.NotSame(previousLevel, game.CurrentLevel);
            Assert.Equal(16, game.Spikes.Count);

            Spike detachedSpike = previousLevel.CreateSpike(game.Player.EdgeIndex, 300);
            game.RunOneStep(new PlayerInput(true, 0, false, false));
            for (int tick = 0; tick < 16; tick++)
                game.RunOneStep();

            Assert.Equal(300, detachedSpike.Height);
            Assert.Equal(16, game.Spikes.Count);
        }
    }

    [Fact]
    public void ProductionProgressionContainsOnlyTheImplementedLevelOneToSixCampaignEdges()
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
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level6Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Plus));
        Assert.Null(LevelProgression.GetNextProductionLevel(
            LevelConfiguration.Level1 with { DisplayedLevel = 1 }));
        Assert.Null(LevelProgression.GetNextProductionLevel(
            LevelConfiguration.Level2Square with { DisplayedLevel = 2 }));
    }

    [Fact]
    public void SpikeAvoidanceExitPolicyIsIndependentOfSpikePresenceAndLevelSixIsTerminal()
    {
        LevelConfiguration[] existingLevels =
        [
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Plus,
            LevelConfiguration.Level3Mixed
        ];
        Assert.All(existingLevels,
            configuration => Assert.Equal(LevelExitPolicy.Standard, configuration.ExitPolicy));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, LevelConfiguration.Level4Candidate.ExitPolicy);
        Assert.Same(LevelConfiguration.Level4Candidate,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level3Mixed));
        Assert.Same(LevelConfiguration.Level5Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level4Candidate));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, LevelConfiguration.Level5Production.ExitPolicy);
        Assert.Same(LevelConfiguration.Level6Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level5Production));
        Assert.Equal(LevelExitPolicy.AvoidSpikes, LevelConfiguration.Level6Production.ExitPolicy);
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level6Production));
        Assert.Equal(150, LevelConfiguration.Level4Candidate.AvoidSpikesWaitThreshold);
        Assert.Equal(150, LevelConfiguration.Level5Production.AvoidSpikesWaitThreshold);
        Assert.Equal(70, LevelConfiguration.Level6Production.AvoidSpikesWaitThreshold);

        LevelConfiguration futureSpikeLevel = LevelConfiguration.Level4Candidate with
        {
            DisplayedLevel = 8,
            ExitPolicy = LevelExitPolicy.Standard
        };
        Assert.NotNull(futureSpikeLevel.StartingSpikeHeight);
        Assert.Equal(LevelExitPolicy.Standard, futureSpikeLevel.ExitPolicy);
    }

    private static void CompleteLevel1Wave(TempestGame game)
    {
        for (int enemyIndex = 0; enemyIndex < game.CurrentLevel.EnemySupply; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
