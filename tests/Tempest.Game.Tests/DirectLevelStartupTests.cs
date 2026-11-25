using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class DirectLevelStartupTests
{
    [Fact]
    public void OmittedAndExplicitLevelOneStartTheOrdinaryProductionCampaign()
    {
        TempestGame normal = new();
        TempestGame direct = new(startingProductionLevel: 1);

        AssertFreshProductionStart(normal, LevelConfiguration.Level1, Player.DefaultStartingLives);
        AssertFreshProductionStart(direct, LevelConfiguration.Level1, Player.DefaultStartingLives);
        Assert.True(normal.NaturalProgressionEnabled);
        Assert.True(direct.NaturalProgressionEnabled);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    public void DirectLevelStartsTheFreshProductionConfiguration(int displayedLevel)
    {
        TempestGame game = new(startingProductionLevel: displayedLevel, startingLives: 9);

        AssertFreshProductionStart(game, GetProductionConfiguration(displayedLevel), 9);
        Assert.True(game.NaturalProgressionEnabled);
    }

    [Fact]
    public void DirectLevelFiveUsesAcceptedSpikeSensitiveProductionPolicy()
    {
        TempestGame game = new(startingProductionLevel: 5);

        Assert.Same(LevelConfiguration.Level5Production, game.CurrentLevel.Configuration);
        Assert.Equal(BelowRimFlipperProgram.Level5SpikeSensitive,
            game.CurrentLevel.Configuration.BelowRimFlipperProgram);
        Assert.Equal(20, game.CurrentLevel.EnemySupply);
        Assert.Equal(16, game.Spikes.Count);
    }

    [Fact]
    public void GenuineLevelsElevenAndFifteenAreProduction()
    {
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetProductionLevel(11));
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetDirectStartLevel(11));
        Assert.Same(LevelConfiguration.Level15Production, LevelProgression.GetProductionLevel(15));
        Assert.Same(LevelConfiguration.Level15Production,
            LevelProgression.GetDirectStartLevel(15));
        Assert.Same(LevelConfiguration.Level10Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level9Production));
        Assert.Same(LevelConfiguration.Level11Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level10Production));
        Assert.Same(LevelConfiguration.Level12Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level11Production));
        Assert.Same(LevelConfiguration.Level13Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level12Production));
        Assert.Same(LevelConfiguration.Level14Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level13Production));
        Assert.Same(LevelConfiguration.Level15Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level14Production));
        Assert.Same(LevelConfiguration.Level16Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level15Production));
        Assert.Null(LevelProgression.GetNextProductionLevel(LevelConfiguration.Level16Production));
        Assert.Same(LevelConfiguration.Level16Production, LevelProgression.GetLastProductionLevel());

        TempestGame levelEleven = new(startingProductionLevel: 11);
        Assert.Same(LevelConfiguration.Level11Production,
            levelEleven.CurrentLevel.Configuration);
        Assert.Equal(11, levelEleven.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(10, levelEleven.CurrentLevel.Configuration.BoardIndex);
        Assert.Equal(11, levelEleven.CurrentLevel.Configuration.EffectiveGameplayParameterLevel);
        Assert.Same(LevelConfiguration.Level11Production.Definition,
            levelEleven.CurrentLevel.Definition);
        Assert.True(levelEleven.NaturalProgressionEnabled);

        TempestGame levelFifteen = new(startingProductionLevel: 15);
        Assert.Same(LevelConfiguration.Level15Production,
            levelFifteen.CurrentLevel.Configuration);
        Assert.Equal(15, levelFifteen.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(14, levelFifteen.CurrentLevel.Configuration.BoardIndex);
        Assert.Same(LevelConfiguration.Level15Production.Definition,
            levelFifteen.CurrentLevel.Definition);

        TempestGame levelTwelve = new(startingProductionLevel: 12);
        Assert.Same(LevelConfiguration.Level12Production,
            levelTwelve.CurrentLevel.Configuration);
        Assert.Equal(12, levelTwelve.CurrentLevel.Configuration.DisplayedLevel);

        TempestGame levelThirteen = new(startingProductionLevel: 13);
        Assert.Same(LevelConfiguration.Level13Production,
            levelThirteen.CurrentLevel.Configuration);
        Assert.Equal(13, levelThirteen.CurrentLevel.Configuration.DisplayedLevel);

        TempestGame levelFourteen = new(startingProductionLevel: 14);
        Assert.Same(LevelConfiguration.Level14Production,
            levelFourteen.CurrentLevel.Configuration);
        Assert.Equal(14, levelFourteen.CurrentLevel.Configuration.DisplayedLevel);
    }

    [Fact]
    public void DirectLevelSevenUsesAcceptedProductionWaveAndRoute()
    {
        TempestGame game = new(startingProductionLevel: 7);

        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        Assert.True(game.CurrentLevel.Configuration.UsesLevel7RomWavePolicy);
        Assert.Equal(BelowRimFlipperProgram.Level7ArcadeObserved,
            game.CurrentLevel.Configuration.BelowRimFlipperProgram);
        Assert.Equal(20, game.CurrentLevel.EnemySupply);
        Assert.Equal(16, game.Spikes.Count);
        Assert.True(game.NaturalProgressionEnabled);
    }

    [Fact]
    public void DirectLevelEightContinuesThroughNineAndTen()
    {
        TempestGame levelSeven = new(startingProductionLevel: 7, startingLives: 5);
        Assert.Same(LevelConfiguration.Level8Production,
            LevelProgression.GetNextProductionLevel(levelSeven.CurrentLevel.Configuration));
        Assert.Same(LevelConfiguration.Level9Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level8Production));

        TempestGame game = new(startingProductionLevel: 9, startingLives: 5);
        game.Score.Add(88);
        Assert.Same(LevelConfiguration.Level10Production,
            LevelProgression.GetNextProductionLevel(LevelConfiguration.Level9Production));
        Assert.True(game.NaturalProgressionEnabled);
    }

    private static LevelConfiguration GetProductionConfiguration(int displayedLevel) =>
        LevelProgression.GetProductionLevel(displayedLevel) ??
        throw new ArgumentOutOfRangeException(nameof(displayedLevel));

    private static void AssertFreshProductionStart(
        TempestGame game,
        LevelConfiguration configuration,
        int lives)
    {
        Assert.Same(configuration, game.CurrentLevel.Configuration);
        Assert.Same(configuration.Definition, game.CurrentLevel.Definition);
        Assert.Equal(configuration.DisplayedLevel, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(configuration.BoardIndex, game.CurrentLevel.Configuration.BoardIndex);
        Assert.Equal(configuration.PaletteIndex, game.CurrentLevel.Configuration.PaletteIndex);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Equal(lives, game.Player.Lives);
        Assert.Equal(configuration.InitialActiveEggCount, game.Eggs.Count);
        Assert.Equal(configuration.EnemySupply - configuration.InitialActiveEggCount,
            game.CurrentLevel.ReserveEggs);
        Assert.Equal(configuration.StartingSpikeHeight is null ? 0 : configuration.PlayableLaneCount,
            game.Spikes.Count);
        if (configuration.StartingSpikeHeight is float spikeHeight)
            Assert.All(game.Spikes, spike => Assert.Equal(spikeHeight, spike.Height));

        Assert.Equal(0, game.Score.Score);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);
        Assert.Empty(game.Fuseballs);
        Assert.Empty(game.Pulsars);
        Assert.Empty(game.Shots);
        Assert.Empty(game.EnemyBullets);
        Assert.False(game.HurryUp);
        Assert.Equal(0, game.DeathTicks);
    }
}
