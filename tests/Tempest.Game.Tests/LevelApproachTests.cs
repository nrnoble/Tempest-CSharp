using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class LevelApproachTests
{
    [Theory]
    [InlineData(2, false)]
    [InlineData(3, true)]
    public void NaturalProductionEdgesEnterTheSameApproachState(int displayedLevel, bool enterLevel3)
    {
        TempestGame game = StartApproach(enterLevel3);

        Assert.True(game.NaturalProgressionEnabled);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Same(enterLevel3
            ? LevelConfiguration.Level3Mixed
            : LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.NotNull(game.CurrentApproach);
        Assert.Equal(0, game.CurrentApproach!.ElapsedTicks);
        Assert.Equal(LevelApproach.InitialDepthOffset, game.CurrentApproach.DepthOffset);
        Assert.Equal(displayedLevel, game.CurrentLevel.Configuration.DisplayedLevel);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ApproachFreezesIncomingWaveAndSuppressesFireAndZapper(bool level3)
    {
        TempestGame game = StartApproach(level3);
        Level incoming = game.CurrentLevel;
        Egg[] eggs = game.Eggs.ToArray();
        (EggState State, float Height, int UpdateCounter)[] eggStates = eggs
            .Select(egg => (egg.State, egg.Height, egg.UpdateCounter))
            .ToArray();
        Level3WaveLedger? ledger = incoming.Level3Ledger;
        int reserveEggs = incoming.ReserveEggs;
        int enemiesSpawned = incoming.EnemiesSpawned;

        for (int tick = 0; tick < 30; tick++)
            game.RunOneStep(new PlayerInput(true, 0, false, false, ZapPressed: true));

        Assert.Same(incoming, game.CurrentLevel);
        Assert.Same(ledger, game.CurrentLevel.Level3Ledger);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Empty(game.Shots);
        Assert.Equal(SuperZapperSystem.InitialCharges, game.SuperZapperChargesRemaining);
        Assert.Equal(0, game.SuperZapper.FlashTicksRemaining);
        Assert.Equal(reserveEggs, incoming.ReserveEggs);
        Assert.Equal(enemiesSpawned, incoming.EnemiesSpawned);
        Assert.False(game.HurryUp);
        Assert.Equal(eggs, game.Eggs);
        Assert.Equal(eggStates, game.Eggs.Select(egg =>
            (egg.State, egg.Height, egg.UpdateCounter)).ToArray());
        Assert.Equal(30, game.CurrentApproach!.ElapsedTicks);
    }

    [Fact]
    public void SpinnerKeyboardAndControllerMovementShareTheIncomingRimPath()
    {
        TempestGame spinnerGame = StartApproach(enterLevel3: false);
        float initialSpinnerPosition = spinnerGame.Player.EdgePosition;
        spinnerGame.RunOneStep(new PlayerInput(false, 1, false, false));
        Assert.True(spinnerGame.Player.EdgePosition > initialSpinnerPosition);

        TempestGame keyboardGame = StartApproach(enterLevel3: false);
        float initialKeyboardPosition = keyboardGame.Player.EdgePosition;
        keyboardGame.RunOneStep(new PlayerInput(false, 0, false, true));
        Assert.True(keyboardGame.Player.EdgePosition > initialKeyboardPosition);

        TempestGame controllerGame = StartApproach(enterLevel3: false);
        float initialControllerPosition = controllerGame.Player.EdgePosition;
        controllerGame.RunOneStep(new PlayerInput(false, 0, false, false, ControllerAxis: 1));
        Assert.True(controllerGame.Player.EdgePosition > initialControllerPosition);
    }

    [Fact]
    public void ApproachModelIsFiniteMonotonicBoundedAndCompletesOnlyOnce()
    {
        LevelApproach approach = new();
        float previousOffset = approach.DepthOffset;
        Assert.Equal(0, approach.Progress);
        Assert.False(approach.IsComplete);

        for (int tick = 1; tick <= LevelApproach.DurationTicks; tick++)
        {
            bool completed = approach.Advance();
            Assert.Equal(tick == LevelApproach.DurationTicks, completed);
            Assert.True(float.IsFinite(approach.DepthOffset));
            Assert.InRange(approach.DepthOffset, 0, previousOffset);
            previousOffset = approach.DepthOffset;
        }

        Assert.Equal(1, approach.Progress);
        Assert.Equal(0, approach.DepthOffset);
        Assert.True(approach.IsComplete);
        Assert.False(approach.Advance());
        Assert.Equal(LevelApproach.DurationTicks, approach.ElapsedTicks);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ArrivalPreservesRimPositionAndBeginsGameplayOnTheNextStep(bool level3)
    {
        TempestGame game = StartApproach(level3);
        Level incoming = game.CurrentLevel;
        Level3WaveLedger? ledger = incoming.Level3Ledger;
        Egg[] eggs = game.Eggs.ToArray();
        VectorRenderer renderer = new(game);
        Vector2[] distantTube = CaptureTube(renderer);
        Assert.InRange(renderer.PlayerClawHeight, 0, 20);
        Assert.True(renderer.PlayerClawHeight < 20);
        renderer.GetPlayerEdge(incoming.Definition.Points, out Vector2 distantPlayerEdge,
            out Vector2 distantPlayerNextEdge);
        Assert.True(float.IsFinite(distantPlayerEdge.X));
        Assert.True(float.IsFinite(distantPlayerEdge.Y));
        Assert.True(float.IsFinite(distantPlayerNextEdge.X));
        Assert.True(float.IsFinite(distantPlayerNextEdge.Y));
        Assert.All(distantTube, point =>
        {
            Assert.True(float.IsFinite(point.X));
            Assert.True(float.IsFinite(point.Y));
        });
        Assert.NotEqual(Projection.Project(incoming.Definition.Points[0].X,
            incoming.Definition.Points[0].Y, Level.Position, incoming.Definition.CenterY,
            incoming.Definition.YOffset), distantTube[0]);

        game.RunOneStep(new PlayerInput(false, 0, false, true));
        int arrivalEdge = game.Player.EdgeIndex;
        float arrivalPosition = game.Player.EdgePosition;
        for (int tick = 1; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Same(ledger, game.CurrentLevel.Level3Ledger);
        Assert.Equal(arrivalEdge, game.Player.EdgeIndex);
        Assert.Equal(arrivalPosition, game.Player.EdgePosition);
        Assert.Equal(eggs, game.Eggs);
        Assert.All(eggs, egg => Assert.Equal(0, egg.UpdateCounter));
        Assert.Equal(0, renderer.ApproachDepthOffset);
        Assert.Equal(20, renderer.PlayerClawHeight);

        Vector2[] gameplayTube = CaptureTube(renderer);
        int pointCount = incoming.Definition.Points.Count;
        Vector2 expectedNear = Projection.Project(incoming.Definition.Points[0].X,
            incoming.Definition.Points[0].Y, Level.Position, incoming.Definition.CenterY,
            incoming.Definition.YOffset);
        Vector2 expectedFar = Projection.Project(incoming.Definition.Points[0].X,
            incoming.Definition.Points[0].Y, Level.Position + Level.Depth,
            incoming.Definition.CenterY, incoming.Definition.YOffset);
        Assert.Equal(expectedNear, gameplayTube[0]);
        Assert.Equal(expectedFar, gameplayTube[1]);

        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Equal(1, eggs[0].UpdateCounter);
    }

    [Fact]
    public void Level3SuccessfulExitBeginsTheLevel4Approach()
    {
        TempestGame game = StartApproach(enterLevel3: true);
        FinishApproach(game);
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();

        Assert.False(game.LevelComplete);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.NotNull(game.CurrentApproach);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        Assert.Equal(16, game.CurrentLevel.PlayableLaneCount);
        Assert.Equal(17, game.CurrentLevel.EnemySupply);
    }

    [Fact]
    public void NaturalLevel3WaitsForEveryLedgerObligationThenUsesTheLevel4Approach()
    {
        TempestGame game = StartApproach(enterLevel3: true);
        FinishApproach(game);
        Assert.True(game.NaturalProgressionEnabled);

        Level outgoing = game.CurrentLevel;
        Level3WaveLedger ledger = outgoing.Level3Ledger!;
        int finalOriginal = ledger.UnresolvedOriginalIds.Max();
        foreach (int id in ledger.UnresolvedOriginalIds.Where(id => id != finalOriginal).ToArray())
        {
            if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                ledger.MarkOriginalEgg(id);
            ledger.CommitOriginalAsFlipper(id);
            ledger.MarkOriginalDestroyed(id);
        }

        outgoing.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.False(game.IsExiting);
        Assert.Contains(finalOriginal, ledger.UnresolvedOriginalIds);

        if (ledger.GetOriginalState(finalOriginal) == Level3OriginalState.Reserve)
            ledger.MarkOriginalEgg(finalOriginal);
        ledger.CommitOriginalAsFlipper(finalOriginal);
        ledger.MarkOriginalDestroyed(finalOriginal);
        outgoing.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.Empty(ledger.UnresolvedOriginalIds);
        Assert.Empty(ledger.UnresolvedCargoIds);

        for (int tick = 0; tick < 300 && ReferenceEquals(outgoing, game.CurrentLevel); tick++)
            game.RunOneStep();

        Level incoming = game.CurrentLevel;
        Assert.NotSame(outgoing, incoming);
        Assert.Same(LevelConfiguration.Level4Candidate, incoming.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(4, incoming.Configuration.DisplayedLevel);
        Assert.Same(LevelConfiguration.Level4Candidate.Definition, incoming.Definition);
        Assert.Equal(16, incoming.PlayableLaneCount);
        Assert.Equal(17, incoming.EnemySupply);
        Assert.Equal(17, incoming.Level3Ledger!.UnresolvedOriginalIds.Count);
        Assert.Equal(4, incoming.Eggs.Count);
        Assert.Equal(13, incoming.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.Tankers);
        Assert.Empty(game.Spikers);

        int[] outgoingStates = Enumerable.Range(0, ledger.OriginalCount)
            .Select(id => (int)ledger.GetOriginalState(id))
            .ToArray();
        Egg[] incomingEggs = incoming.Eggs.ToArray();
        int[] eggUpdates = incomingEggs.Select(egg => egg.UpdateCounter).ToArray();
        float initialPosition = game.Player.EdgePosition;
        game.RunOneStep(new PlayerInput(Fire: true, MouseDeltaX: 0, Left: false, Right: true));
        Assert.True(game.Player.EdgePosition > initialPosition);
        Assert.Empty(game.Shots);
        Assert.Equal(outgoingStates, Enumerable.Range(0, ledger.OriginalCount)
            .Select(id => (int)ledger.GetOriginalState(id)));
        Assert.Equal(eggUpdates, incomingEggs.Select(egg => egg.UpdateCounter));

        for (int tick = 1; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Same(incoming, game.CurrentLevel);
        Assert.Same(LevelConfiguration.Level4Candidate, game.CurrentLevel.Configuration);
        game.RunOneStep();
        Assert.Same(incoming, game.CurrentLevel);
    }

    private static TempestGame StartApproach(bool enterLevel3)
    {
        TempestGame game = new();
        CommitNextLevel(game);
        if (enterLevel3)
        {
            FinishApproach(game);
            CommitNextLevel(game);
        }
        return game;
    }

    private static void CommitNextLevel(TempestGame game)
    {
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();
        Assert.Equal(LevelState.Approach, game.State);
    }

    private static void FinishApproach(TempestGame game)
    {
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
    }

    private static Vector2[] CaptureTube(VectorRenderer renderer)
    {
        List<Vector2> points = [];
        renderer.DrawTube((start, end, _) =>
        {
            points.Add(start);
            points.Add(end);
        });
        return points.ToArray();
    }
}
