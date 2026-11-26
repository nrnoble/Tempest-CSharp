using System.Reflection;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level15FuseballPolicyTests
{
    [Fact]
    public void LevelFifteenBaselineSelectsFuseballAndKeepsItNonterminalWhileSupplyRemains()
    {
        TempestGame game = new(LevelConfiguration.Level15Production, random: new Random(4));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        for (int tick = 0; tick < 126; tick++)
            game.RunOneStep();

        Egg fuseballEgg = Assert.Single(game.Eggs, egg => egg.OriginalSlotId == 4);
        Assert.Equal(EggState.Approaching, fuseballEgg.State);
        Assert.True(ledger.PendingGenericOriginalCount > 0);

        game.RunOneStep();

        Fuseball fuseball = Assert.Single(game.Fuseballs);
        Assert.Equal(4, fuseball.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Fuseball, ledger.GetOriginalType(4));
        Assert.Equal(Level3OriginalState.ActiveFuseball, ledger.GetOriginalState(4));
        Assert.True(ledger.PendingGenericOriginalCount > 0);

        Set(fuseball, nameof(Fuseball.Height), 100f);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Climbing);
        game.RunOneStep();

        Assert.NotEqual(FuseballState.TerminalRimward, fuseball.State);
        Assert.True(ledger.PendingGenericOriginalCount > 0);
        Assert.Equal(Level3OriginalState.ActiveFuseball, ledger.GetOriginalState(4));
    }

    [Fact]
    public void LevelElevenDerivedPendingPolicyIsSharedWithoutRelabelingLevelFifteen()
    {
        LevelConfiguration level11 = LevelConfiguration.Level11Production;
        LevelConfiguration level15 = LevelConfiguration.Level15Production;
        LevelConfiguration level17 = LevelConfiguration.Level17Production;

        Assert.True(level11.UsesLevel11DerivedGenericPendingSupplyPolicy);
        Assert.True(level15.UsesLevel11DerivedGenericPendingSupplyPolicy);
        Assert.False(level15.UsesLevel11RomWavePolicy);
        Assert.True(level15.UsesLevel11ParameterDevelopmentBaseline);
        Assert.False(level17.UsesLevel11DerivedGenericPendingSupplyPolicy);
    }

    [Fact]
    public void LevelFifteenZeroPendingFuseballUsesSharedTerminalMovementAndCompletion()
    {
        TempestGame game = CreateScenario(LevelConfiguration.Level15Production,
            pendingOriginals: 0, out int originalId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.Same(typeof(Fuseball), fuseball.GetType());
        game.RunOneStep();

        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
        Assert.Equal(Level.Depth - fuseball.ClimbSpeed, fuseball.Height);
        Assert.Same(fuseball, Assert.Single(game.Fuseballs));
        Assert.Equal(originalId, fuseball.OriginalSlotId);
        Assert.Equal(Level3OriginalType.Fuseball, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveFuseball, ledger.GetOriginalState(originalId));
        Assert.Equal(LevelState.Playing, game.State);

        for (int tick = 0; tick < 100 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.RimCleared, ledger.GetOriginalState(originalId));
    }

    [Fact]
    public void LevelFifteenTerminalFuseballRemainsPlayerShootable()
    {
        TempestGame game = CreateScenario(LevelConfiguration.Level15Production,
            pendingOriginals: 0, out int originalId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Set(fuseball, nameof(Fuseball.State), FuseballState.TerminalRimward);
        Set(fuseball, nameof(Fuseball.Height), 100f);
        Assert.Single(game.CurrentLevel.Spikes, spike => spike.EdgeIndex == fuseball.EdgeIndex)
            .Height = 0;
        game.RunOneStep();

        Shot shot = new()
        {
            EdgeIndex = fuseball.EdgeIndex,
            Height = fuseball.Height - 8
        };
        bool hit = (bool)(typeof(TempestGame).GetMethod(
            "CheckShotAgainstDevelopmentTargets",
            BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(game, [shot])
            ?? throw new InvalidOperationException("Fuseball shot check was not found."));

        Assert.True(hit);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(originalId));
    }

    [Fact]
    public void LevelFifteenDeathReentryReconstructsTheFuseballOriginalIdentity()
    {
        TempestGame game = CreateScenario(LevelConfiguration.Level15Production,
            pendingOriginals: 0, out int originalId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        game.RunOneStep();
        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);

        typeof(TempestGame).GetMethod("KillPlayer",
            BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(game, null);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Empty(game.Fuseballs);
        Assert.Contains(originalId, ledger.UnresolvedOriginalIds);
        Assert.Contains(ledger.GetOriginalState(originalId),
            new[] { Level3OriginalState.Reserve, Level3OriginalState.Egg });
        Assert.Equal(1, ledger.OriginalIdsInReserve().Count(id => id == originalId) +
            game.Eggs.Count(egg => egg.OriginalSlotId == originalId));
    }

    [Theory]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(16)]
    public void OtherProductionFuseballPoliciesKeepTheirPendingSupply(int displayedLevel)
    {
        LevelConfiguration configuration = ProductionConfiguration(displayedLevel);
        TempestGame game = CreateScenario(configuration,
            pendingOriginals: 1, out int originalId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        Assert.Equal(1, ledger.PendingGenericOriginalCount);
        game.RunOneStep();

        Assert.NotEqual(FuseballState.TerminalRimward, fuseball.State);
        Assert.Equal(Level3OriginalType.Fuseball, ledger.GetOriginalType(originalId));
        Assert.Equal(Level3OriginalState.ActiveFuseball, ledger.GetOriginalState(originalId));
    }

    [Fact]
    public void LevelSeventeenKeepsFuseballsIneligible()
    {
        Assert.Equal((0, 0), (Level17WaveSelection.MinimumFuseballs,
            Level17WaveSelection.MaximumFuseballs));
        Assert.False(LevelConfiguration.Level17Production
            .UsesLevel11DerivedGenericPendingSupplyPolicy);

        TempestGame game = new(startingProductionLevel: 17, random: new Random(1982));
        for (int tick = 0; tick < 200; tick++)
            game.RunOneStep();

        Assert.Empty(game.Fuseballs);
    }

    private static TempestGame CreateScenario(
        LevelConfiguration configuration,
        int pendingOriginals,
        out int originalId,
        out Fuseball fuseball)
    {
        TempestGame game = new(configuration with { ReserveReleaseIntervalTicks = 10_000 },
            random: new Random(4));
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException("Production level has no identity ledger.");
        originalId = ledger.OriginalIdsInReserve()[0];
        ledger.MarkOriginalEgg(originalId);
        ledger.CommitOriginalAsFuseball(originalId);

        foreach (int id in ledger.OriginalIdsInReserve().Skip(pendingOriginals))
        {
            ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        foreach (int id in Enumerable.Range(0, ledger.OriginalCount)
                     .Where(id => ledger.GetOriginalState(id) == Level3OriginalState.Egg))
        {
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        GetField<List<Egg>>(game.CurrentLevel, "eggs").Clear();
        fuseball = new(edgeIndex: 0, originalSlotId: originalId);
        Set(fuseball, nameof(Fuseball.Height), Level.Depth - fuseball.ClimbSpeed);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Climbing);
        GetList<Fuseball>(game, "fuseballs").Add(fuseball);
        GetList<object>(game, "developmentCollisionOrder").Add(fuseball);
        ledger.ValidateState();
        return game;
    }

    private static LevelConfiguration ProductionConfiguration(int displayedLevel) =>
        displayedLevel switch
        {
            12 => LevelConfiguration.Level12Production,
            13 => LevelConfiguration.Level13Production,
            14 => LevelConfiguration.Level14Production,
            16 => LevelConfiguration.Level16Production,
            _ => throw new ArgumentOutOfRangeException(nameof(displayedLevel))
        };

    private static T GetField<T>(object instance, string name)
    {
        FieldInfo field = instance.GetType().GetField(
            name, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Field '{name}' was not found.");
        return (T)(field.GetValue(instance)
            ?? throw new InvalidOperationException($"Field '{name}' was null."));
    }

    private static List<T> GetList<T>(object instance, string name) =>
        GetField<List<T>>(instance, name);

    private static void Set<T>(object instance, string propertyName, T value)
    {
        PropertyInfo property = instance.GetType().GetProperty(
            propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Property '{propertyName}' was not found.");
        property.SetValue(instance, value);
    }
}
