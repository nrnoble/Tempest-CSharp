using System.Reflection;
using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Raylib_cs;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class FuseballTerminalMovementTests
{
    [Fact]
    public void ProductionFuseballCyclesFourVisualPhasesWithoutChangingGameplay()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: false,
            out _, out Fuseball fuseball);
        Set(fuseball, nameof(Fuseball.Height), 100f);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Lateral);
        game.CurrentLevel.State = LevelState.Playing;
        VectorRenderer renderer = new(game);
        HashSet<int> frames = [];
        bool sawActiveFuseball = false;

        for (int tick = 0; tick < 16; tick++)
        {
            Assert.Equal(tick % 16 / 4, game.FuseballAnimationFrame);
            frames.Add(game.FuseballAnimationFrame);

            if (game.Fuseballs.Count == 1)
            {
                sawActiveFuseball = true;
                float height = fuseball.Height;
                int edgeIndex = fuseball.EdgeIndex;
                int w = fuseball.W;
                int direction = fuseball.Direction;
                FuseballState state = fuseball.State;
                List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
                renderer.DrawFuseballs((start, end, color) => lines.Add((start, end, color)));
                Assert.Equal(25, lines.Count);
                Assert.Equal(5, lines.Select(line => line.Color).Distinct().Count());
                Assert.Equal(height, fuseball.Height);
                Assert.Equal(edgeIndex, fuseball.EdgeIndex);
                Assert.Equal(w, fuseball.W);
                Assert.Equal(direction, fuseball.Direction);
                Assert.Equal(state, fuseball.State);
            }

            game.RunOneStep();
        }

        Assert.True(sawActiveFuseball);
        Assert.Equal(new[] { 0, 1, 2, 3 }, frames.OrderBy(frame => frame).ToArray());
        Assert.Equal(0, game.FuseballAnimationFrame);
    }

    [Fact]
    public void ProductionFuseballAnimationWrapsAtModuloSixteen()
    {
        TempestGame game = new(startingProductionLevel: 11);
        game.CurrentLevel.State = LevelState.Playing;
        typeof(TempestGame).GetField("fuseballAnimationTicks",
            BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(game, 15);
        game.RunOneStep();
        Assert.Equal(0, game.FuseballAnimationFrame);
    }

    [Fact]
    public void ProductionAndDirectRestartStartWithDeterministicVisualPhase()
    {
        TempestGame first = new(startingProductionLevel: 11);
        TempestGame second = new(startingProductionLevel: 11);

        Assert.Equal(0, first.FuseballAnimationFrame);
        Assert.Equal(0, second.FuseballAnimationFrame);
        for (int tick = 0; tick < 64; tick++)
        {
            first.RunOneStep();
            second.RunOneStep();
            Assert.Equal(first.FuseballAnimationFrame, second.FuseballAnimationFrame);
        }

        Assert.Equal(0, first.FuseballAnimationFrame);
    }

    [Fact]
    public void PendingSupplyPreservesOrdinaryRandomizedFuseballCycling()
    {
        Level level = new();
        Fuseball fuseball = CreateAtLateralBoundary(height: 100, pendingDirection: 1);
        List<(int Min, int Max)> rolls = [];

        fuseball.Update(level, 8, (min, max) =>
        {
            rolls.Add((min, max));
            return max == 2 ? 2 : min;
        }, pendingGenericSupply: 1);

        Assert.Equal(FuseballState.Descending, fuseball.State);
        Assert.Equal(100, fuseball.Height);
        Assert.Equal(new[] { (0, 100), (1, 2) }, rolls);
    }

    [Fact]
    public void ZeroPendingSupplyDuringDescentWaitsForTheNextRimwardEndpoint()
    {
        Level level = new();
        Fuseball fuseball = CreateAtLateralBoundary(height: 100, pendingDirection: 1);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Descending);
        Set(fuseball, nameof(Fuseball.StateTicks), 25);

        fuseball.Update(level, 8, (_, _) => 1, pendingGenericSupply: 0);

        Assert.Equal(FuseballState.Lateral, fuseball.State);
        Assert.Equal(75, fuseball.Height);

        for (int tick = 0; tick < 8; tick++)
            fuseball.Update(level, 8, (_, max) => max == 100
                    ? 0
                    : throw new InvalidOperationException(),
                pendingGenericSupply: 0);
        Assert.Equal(FuseballState.Lateral, fuseball.State);

        fuseball.Update(level, 8, (_, max) => max is 100 or 2
                ? max == 2 ? 1 : 0
                : throw new InvalidOperationException(),
            pendingGenericSupply: 0);
        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.Equal(75, fuseball.Height);

        int climbUpdates = 0;
        while (fuseball.State != FuseballState.TerminalRimward)
        {
            fuseball.Update(level, 8, (_, max) => max == 3
                    ? 1
                    : throw new InvalidOperationException(),
                pendingGenericSupply: 0);
            climbUpdates++;
        }
        Assert.InRange(climbUpdates, 1, 20);
        Assert.Equal(375, fuseball.Height);
        fuseball.Update(level, 8, (_, _) => throw new InvalidOperationException(),
            pendingGenericSupply: 0);
        Assert.Equal(Level.Depth, fuseball.Height);
    }

    [Fact]
    public void TerminalDecisionWaitsIfPendingSupplyReachesZeroAfterThePriorBoundary()
    {
        Level level = new();
        Fuseball fuseball = new(climbSpeed: 10);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Climbing);
        Set(fuseball, nameof(Fuseball.Height), 100f);

        fuseball.Update(level, 8, (_, max) => max == 3
                ? 1
                : throw new InvalidOperationException(),
            pendingGenericSupply: 0);

        Assert.Equal(FuseballState.Climbing, fuseball.State);
        Assert.Equal(110, fuseball.Height);

        Set(fuseball, nameof(Fuseball.Height), 390f);
        fuseball.Update(level, 8, (_, _) => throw new InvalidOperationException(),
            pendingGenericSupply: 0);

        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
        Assert.Equal(390, fuseball.Height);
    }

    [Fact]
    public void TerminalRimwardMovementUsesNormalClimbSpeedAndNeverReturnsToOrdinaryCycling()
    {
        Level level = new();
        Fuseball fuseball = new(edgeIndex: 0, climbSpeed: 100);
        Set(fuseball, nameof(Fuseball.State), FuseballState.TerminalRimward);
        Set(fuseball, nameof(Fuseball.Height), 100f);

        for (int tick = 1; tick <= 3; tick++)
        {
            fuseball.Update(level, 8, (_, _) => throw new InvalidOperationException(),
                hurryUp: true);
            Assert.Equal(100 + tick * 100, fuseball.Height);
            Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
        }

        fuseball.Update(level, 8, (_, _) => throw new InvalidOperationException());
        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
    }

    [Fact]
    public void TerminalFuseballRemainsShootableAndResolvesItsProductionIdentity()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: false,
            out int fuseballId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        game.RunOneStep();
        Shot shot = new() { EdgeIndex = fuseball.EdgeIndex, Height = fuseball.Height - 8 };

        bool hit = (bool)typeof(TempestGame).GetMethod(
            "CheckShotAgainstDevelopmentTargets",
            BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(game, [shot])!;

        Assert.True(hit);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.Destroyed, ledger.GetOriginalState(fuseballId));
        game.RunOneStep();
    }

    [Fact]
    public void FixedInputAndRandomSequenceProduceDeterministicTerminalTiming()
    {
        Level level = new();
        Fuseball first = CreateAtLateralBoundary(height: 90, pendingDirection: 1);
        Fuseball second = CreateAtLateralBoundary(height: 90, pendingDirection: 1);

        for (int tick = 0; tick < 16; tick++)
        {
            int Roll(int min, int max) => max == 2 ? 1 : min;
            first.Update(level, 8, Roll, pendingGenericSupply: 0);
            second.Update(level, 8, Roll, pendingGenericSupply: 0);
            Assert.Equal(first.State, second.State);
            Assert.Equal(first.StateTicks, second.StateTicks);
            Assert.Equal(first.Height, second.Height);
            Assert.Equal(first.W, second.W);
        }

        Assert.Equal(FuseballState.TerminalRimward, first.State);
        Assert.Equal(FuseballState.TerminalRimward, second.State);
    }

    [Fact]
    public void TerminalDelayVariesWithTheFuseballsStateWhenSupplyReachesZero()
    {
        Level level = new();
        Fuseball atBoundary = new(climbSpeed: 300);
        Set(atBoundary, nameof(Fuseball.Height), 90f);
        Set(atBoundary, nameof(Fuseball.StateTicks), 8);
        Set(atBoundary, nameof(Fuseball.W), 7);
        Set(atBoundary, nameof(Fuseball.Direction), 1);
        int atBoundaryTicks = 0;
        int RollUpward(int min, int max) => max == 2 ? 1 : min;
        while (atBoundary.State != FuseballState.TerminalRimward)
        {
            atBoundary.Update(level, 8, RollUpward, pendingGenericSupply: 0);
            atBoundaryTicks++;
        }

        Fuseball awayFromBoundary = new(climbSpeed: 300);
        Set(awayFromBoundary, nameof(Fuseball.Height), 90f);
        Set(awayFromBoundary, nameof(Fuseball.StateTicks), 25);
        Set(awayFromBoundary, nameof(Fuseball.W), 7);
        Set(awayFromBoundary, nameof(Fuseball.Direction), 1);
        Set(awayFromBoundary, nameof(Fuseball.State), FuseballState.Descending);
        int ticksToTerminal = 0;
        awayFromBoundary.Update(level, 8, (_, _) => 1, pendingGenericSupply: 0);
        ticksToTerminal++;
        while (awayFromBoundary.State != FuseballState.TerminalRimward)
        {
            awayFromBoundary.Update(level, 8, RollUpward,
                pendingGenericSupply: 0);
            ticksToTerminal++;
        }

        Assert.Equal(3, atBoundaryTicks);
        Assert.Equal(12, ticksToTerminal);
    }

    [Fact]
    public void ActiveRimFlipperDoesNotReplacePositivePendingSupplyAsTheTrigger()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: true,
            out _, out Fuseball fuseball, remainingPendingOriginals: 1);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        Set(fuseball, nameof(Fuseball.State), FuseballState.Lateral);
        Set(fuseball, nameof(Fuseball.Height), 100f);
        Set(fuseball, nameof(Fuseball.StateTicks), 8);
        Set(fuseball, nameof(Fuseball.W), 7);
        Set(fuseball, nameof(Fuseball.Direction), 1);

        game.RunOneStep();

        Assert.True(ledger.PendingGenericOriginalCount > 0);
        Assert.NotEqual(FuseballState.TerminalRimward, fuseball.State);
        Assert.Equal(100, fuseball.Height);
    }

    [Fact]
    public void LevelElevenLedgerSupplyAndRimFlippersDoNotBlockTerminalMovementOrCompletion()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: true,
            out int fuseballId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        Assert.Equal(0, ledger.PendingGenericOriginalCount);
        Assert.Equal(Level3OriginalState.ActiveFlipper,
            ledger.GetOriginalState(fuseballId + 1));
        Assert.False(game.HurryUp);

        game.RunOneStep();

        Assert.Same(fuseball, Assert.Single(game.Fuseballs));
        Assert.Equal(fuseballId, fuseball.OriginalSlotId);
        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
        Assert.Equal(398, fuseball.Height);
        Assert.Equal(LevelState.Playing, game.State);

        for (int tick = 0; tick < 100 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Fuseballs);
        Assert.Equal(Level3OriginalState.RimCleared, ledger.GetOriginalState(fuseballId));
        Assert.Equal(Level3OriginalState.RimCleared,
            ledger.GetOriginalState(fuseballId + 1));
    }

    [Fact]
    public void TerminalFuseballAtRimStillWaitsForOtherUnresolvedEnemies()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: true,
            out _, out Fuseball fuseball);
        Flipper flipper = Assert.Single(game.Flippers);
        Set(flipper, nameof(Flipper.Height), 100f);

        game.RunOneStep();
        for (int tick = 0; tick < 100; tick++)
            game.RunOneStep();

        Assert.Equal(Level.Depth, fuseball.Height);
        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);
        Assert.Same(fuseball, Assert.Single(game.Fuseballs));
        Assert.Equal(LevelState.Playing, game.State);

        Set(flipper, nameof(Flipper.Height), Level.Depth);
        game.RunOneStep();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(game.Fuseballs);
    }

    [Fact]
    public void DeathDuringTerminalMovementReconstructsTheSameUnresolvedIdentity()
    {
        TempestGame game = CreateLevelElevenGame(withRimFlipper: false,
            out int fuseballId, out Fuseball fuseball);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;

        game.RunOneStep();
        Assert.Equal(FuseballState.TerminalRimward, fuseball.State);

        typeof(TempestGame).GetMethod("KillPlayer",
            BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(game, null);
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Empty(game.Fuseballs);
        Assert.Contains(fuseballId, ledger.UnresolvedOriginalIds);
        Assert.Contains(ledger.GetOriginalState(fuseballId),
            new[] { Level3OriginalState.Reserve, Level3OriginalState.Egg });
        Assert.Equal(1, ledger.OriginalIdsInReserve().Count(id => id == fuseballId) +
            game.Eggs.Count(egg => egg.OriginalSlotId == fuseballId));
    }

    private static Fuseball CreateAtLateralBoundary(float height, int pendingDirection)
    {
        Fuseball fuseball = new(climbSpeed: 25);
        Set(fuseball, nameof(Fuseball.Height), height);
        Set(fuseball, nameof(Fuseball.StateTicks), 8);
        Set(fuseball, nameof(Fuseball.W), 7);
        Set(fuseball, nameof(Fuseball.Direction), pendingDirection);
        return fuseball;
    }

    private static TempestGame CreateLevelElevenGame(bool withRimFlipper,
        out int fuseballId, out Fuseball fuseball, int remainingPendingOriginals = 0)
    {
        TempestGame game = new(startingProductionLevel: 11);
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger!;
        fuseballId = ledger.OriginalIdsInReserve()[0];
        ledger.MarkOriginalEgg(fuseballId);
        ledger.CommitOriginalAsFuseball(fuseballId);
        if (withRimFlipper)
        {
            int flipperId = ledger.OriginalIdsInReserve()[0];
            ledger.MarkOriginalEgg(flipperId);
            ledger.CommitOriginalAsFlipper(flipperId);
            Flipper flipper = new(1, 0);
            Set(flipper, "OriginalSlotId", flipperId);
            Set(flipper, nameof(Flipper.Height), Level.Depth);
            GameList<Flipper>(game, "flippers").Add(flipper);
            GameList<object>(game, "developmentCollisionOrder").Add(flipper);
        }

        foreach (int id in ledger.OriginalIdsInReserve()
                     .Skip(remainingPendingOriginals))
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
        RemoveAllEggs(game);

        fuseball = new(edgeIndex: 0, originalSlotId: fuseballId);
        Set(fuseball, nameof(Fuseball.Height), 398f);
        Set(fuseball, nameof(Fuseball.State), FuseballState.Climbing);
        GameList<Fuseball>(game, "fuseballs").Add(fuseball);
        GameList<object>(game, "developmentCollisionOrder").Add(fuseball);
        return game;
    }

    private static void RemoveAllEggs(TempestGame game)
    {
        FieldInfo field = typeof(Level).GetField(
            "eggs", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The Level Egg collection was not found.");
        ((List<Egg>)(field.GetValue(game.CurrentLevel)
            ?? throw new InvalidOperationException("The Level Egg collection is null."))).Clear();
    }

    private static List<T> GameList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException($"Game collection '{fieldName}' is null."));
    }

    private static void Set<T>(object instance, string propertyName, T value)
    {
        PropertyInfo property = instance.GetType().GetProperty(
            propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Property '{propertyName}' was not found.");
        property.SetValue(instance, value);
    }
}
