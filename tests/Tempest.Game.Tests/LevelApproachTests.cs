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
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void PerspectiveBiasIsDeterministicDepthDependentAndRestoresNormalGeometry(int displayedLevel)
    {
        TempestGame game = StartApproach(displayedLevel);
        VectorRenderer renderer = new(game);
        Level incoming = game.CurrentLevel;
        Vector2[] originalPoints = incoming.Definition.Points.ToArray();
        float previousBias = float.PositiveInfinity;
        float previousNearShift = float.PositiveInfinity;
        float previousFarShift = float.PositiveInfinity;
        float firstBiasStep = 0;
        float middleBiasStep = 0;
        float previousNearEdgeLength = 0;
        float previousFarEdgeLength = 0;

        Assert.Equal(181, LevelApproach.DurationTicks);
        Assert.Equal(960, LevelApproach.InitialDepthOffset);
        Assert.Equal(VectorRenderer.InitialApproachVerticalOffset, renderer.ApproachVerticalOffset);

        for (int tick = 0; tick <= LevelApproach.DurationTicks; tick++)
        {
            float progress = tick / (float)LevelApproach.DurationTicks;
            float remaining = 1 - progress * progress * (3 - 2 * progress);
            Assert.Equal(LevelApproach.InitialDepthOffset * remaining, renderer.ApproachDepthOffset);
            float bias = renderer.ApproachVerticalOffset;
            Assert.Equal(VectorRenderer.InitialApproachVerticalOffset *
                (renderer.ApproachDepthOffset / LevelApproach.InitialDepthOffset), bias);
            Assert.True(float.IsFinite(bias));
            Assert.InRange(bias, 0, previousBias);
            if (tick < LevelApproach.DurationTicks)
            {
                Assert.Equal(LevelState.Approach, game.State);
                Assert.True(bias > 0);
                Assert.Equal(tick, game.CurrentApproach!.ElapsedTicks);
            }
            if (tick == 1)
                firstBiasStep = previousBias - bias;
            if (tick == LevelApproach.DurationTicks / 2)
                middleBiasStep = previousBias - bias;

            Vector2[] tube = CaptureTube(renderer);
            Assert.Equal(tube, CaptureTube(renderer));
            float nearEdgeLength = Vector2.Distance(tube[0], tube[2]);
            float farEdgeLength = Vector2.Distance(tube[1], tube[3]);
            Assert.True(nearEdgeLength > previousNearEdgeLength);
            Assert.True(farEdgeLength > previousFarEdgeLength);
            previousNearEdgeLength = nearEdgeLength;
            previousFarEdgeLength = farEdgeLength;
            if (tick == 0)
            {
                Vector2 first = originalPoints[0];
                Vector2 second = originalPoints[1];
                Vector2 PreviousStart(Vector2 point) => Projection.Project(point.X, point.Y,
                    Level.Position + 240, incoming.Definition.CenterY, incoming.Definition.YOffset);
                float previousInitialLength = Vector2.Distance(PreviousStart(first), PreviousStart(second));
                Assert.InRange(nearEdgeLength / previousInitialLength, .28f, .30f);
                Assert.InRange(renderer.PlayerClawHeight, 1, 2);
            }
            float[] shifts = new float[2];
            for (int vertex = 0; vertex < originalPoints.Length; vertex++)
            {
                Vector2 point = originalPoints[vertex];
                for (int rim = 0; rim < 2; rim++)
                {
                    float depth = Level.Position + rim * Level.Depth + renderer.ApproachDepthOffset;
                    Vector2 accepted = Projection.Project(point.X, point.Y, depth,
                        incoming.Definition.CenterY, incoming.Definition.YOffset);
                    Vector2 expected = Projection.Project(point.X, point.Y, depth,
                        incoming.Definition.CenterY, incoming.Definition.YOffset + bias);
                    Vector2 actual = tube[vertex * 2 + rim];
                    Assert.True(float.IsFinite(actual.X));
                    Assert.True(float.IsFinite(actual.Y));
                    Assert.Equal(accepted.X, actual.X);
                    Assert.Equal(expected, actual);
                    Assert.InRange(MathF.Abs((actual.Y - accepted.Y) -
                        bias * Projection.K / (depth + Projection.ZOffset)), 0, .0001f);
                    if (vertex == 0)
                        shifts[rim] = actual.Y - accepted.Y;
                }
            }
            if (tick < LevelApproach.DurationTicks)
                Assert.True(shifts[0] > shifts[1]);
            Assert.InRange(shifts[0], 0, previousNearShift + .0001f);
            Assert.InRange(shifts[1], 0, previousFarShift + .0001f);
            previousNearShift = shifts[0];
            previousFarShift = shifts[1];

            renderer.GetPlayerEdge(originalPoints, out Vector2 playerStart, out Vector2 playerEnd);
            Assert.Equal(tube[game.Player.EdgeIndex * 2], playerStart);
            Assert.Equal(tube[incoming.NextEdge(game.Player.EdgeIndex) * 2], playerEnd);
            Assert.Equal(70f, Projection.K);
            Assert.Equal(5f, Projection.ZOffset);
            Assert.Equal(originalPoints, incoming.Definition.Points);
            if (tick < LevelApproach.DurationTicks)
            {
                previousBias = bias;
                game.RunOneStep(new PlayerInput(false, 8, false, false));
            }
        }

        Assert.True(firstBiasStep < middleBiasStep);
        Assert.True(previousBias < middleBiasStep);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Equal(0, renderer.ApproachVerticalOffset);
        Assert.Equal(0, renderer.ApproachDepthOffset);
        Assert.All(game.Eggs, egg => Assert.Equal(0, egg.UpdateCounter));
        Vector2[] arrivalTube = CaptureTube(renderer);
        game.RunOneStep();
        Assert.Equal(arrivalTube, CaptureTube(renderer));
        Assert.Equal(1, game.Eggs[0].UpdateCounter);
    }

    [Theory]
    [InlineData(LevelState.Playing)]
    [InlineData(LevelState.ExitDelay)]
    [InlineData(LevelState.ExitZoom)]
    [InlineData(LevelState.ReverseZoom)]
    [InlineData(LevelState.PlayerDying)]
    [InlineData(LevelState.Complete)]
    public void PerspectiveBiasDoesNotAffectRenderingOutsideApproach(LevelState state)
    {
        TempestGame game = StartApproach(2);
        VectorRenderer renderer = new(game);
        game.CurrentLevel.State = state;

        Assert.Equal(0, renderer.ApproachVerticalOffset);
        Assert.Equal(0, renderer.ApproachDepthOffset);
        Vector2[] tube = CaptureTube(renderer);
        for (int vertex = 0; vertex < game.CurrentLevel.Definition.Points.Count; vertex++)
        {
            Vector2 point = game.CurrentLevel.Definition.Points[vertex];
            for (int rim = 0; rim < 2; rim++)
                Assert.Equal(Projection.Project(point.X, point.Y, Level.Position + rim * Level.Depth,
                    game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset),
                    tube[vertex * 2 + rim]);
        }
    }

    [Fact]
    public void IncomingLevel4SpikesUseTheSameApproachProjectionAsTheWeb()
    {
        TempestGame game = StartApproach(4);
        VectorRenderer renderer = new(game);
        Assert.NotEmpty(game.Spikes);

        for (int tick = 0; tick <= LevelApproach.DurationTicks; tick++)
        {
            List<(Vector2 Start, Vector2 Tip)> lines = [];
            List<Vector2> tips = [];
            renderer.DrawSpikes((start, tip) => lines.Add((start, tip)), tips.Add);
            Assert.Equal(game.Spikes.Count, lines.Count);
            for (int i = 0; i < game.Spikes.Count; i++)
            {
                Spike spike = game.Spikes[i];
                IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
                Vector2 midpoint = (points[spike.EdgeIndex] +
                    points[game.CurrentLevel.NextEdge(spike.EdgeIndex)]) / 2;
                Vector2 Expected(float depth) => Projection.Project(midpoint.X, midpoint.Y,
                    depth + renderer.ApproachDepthOffset, game.CurrentLevel.Definition.CenterY,
                    game.CurrentLevel.Definition.YOffset + renderer.ApproachVerticalOffset);
                Assert.Equal(Expected(Level.Position + Level.Depth), lines[i].Start);
                Assert.Equal(Expected(Level.Position + Level.Depth - spike.Height), lines[i].Tip);
                Assert.Equal(lines[i].Tip, tips[i]);
                Assert.True(float.IsFinite(lines[i].Start.X));
                Assert.True(float.IsFinite(lines[i].Start.Y));
                Assert.True(float.IsFinite(lines[i].Tip.X));
                Assert.True(float.IsFinite(lines[i].Tip.Y));
            }
            if (tick < LevelApproach.DurationTicks)
                game.RunOneStep();
        }
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
        LevelApproach approach = new(exitedLevel: 1);
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
        Assert.Equal(0, renderer.ApproachVerticalOffset);
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

    private static TempestGame StartApproach(bool enterLevel3) =>
        StartApproach(enterLevel3 ? 3 : 2);

    private static TempestGame StartApproach(int displayedLevel)
    {
        TempestGame game = new();
        for (int nextLevel = 2; nextLevel <= displayedLevel; nextLevel++)
        {
            CommitNextLevel(game);
            if (nextLevel < displayedLevel)
                FinishApproach(game);
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
