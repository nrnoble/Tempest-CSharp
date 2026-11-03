using System.Numerics;
using Raylib_cs;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class TransitionPresentationTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void FirstVisibleStarToArrivalTakesFourSecondsAtTheEstablishedRuntimeCadence(int exitedLevel)
    {
        TempestGame game = EnterExitFrom(exitedLevel);
        VectorRenderer renderer = new(game);
        int exitUpdates = 0;
        int firstVisibleStarUpdate = -1;
        Assert.Empty(CaptureStars(renderer));
        while (game.State == LevelState.ExitZoom && exitUpdates < 200)
        {
            game.RunOneStep();
            exitUpdates++;
            if (firstVisibleStarUpdate < 0 && CaptureStars(renderer).Count > 0)
            {
                Assert.Equal(LevelState.ExitZoom, game.State);
                Assert.InRange(game.Player.Depth, 190, 200);
                firstVisibleStarUpdate = exitUpdates;
            }
        }

        Assert.Equal(65, firstVisibleStarUpdate);
        Assert.Equal(124, exitUpdates);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(exitedLevel + 1, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(0, game.CurrentApproach!.ElapsedTicks);
        Assert.Equal(TransitionStarfield.MaximumStars, CaptureStars(renderer).Count);
        int remainingExitUpdates = exitUpdates - firstVisibleStarUpdate;
        Assert.Equal(59, remainingExitUpdates);

        int approachUpdates = 0;
        while (game.State == LevelState.Approach && approachUpdates < 300)
        {
            game.RunOneStep();
            approachUpdates++;
        }
        Assert.Equal(181, approachUpdates);
        Assert.Equal(LevelState.Playing, game.State);
        int visibleTransitionIntervals = remainingExitUpdates + approachUpdates;
        Assert.Equal(240, visibleTransitionIntervals);
        // Program runs one update per frame with SetTargetFPS(60), not wall-clock catch-up steps.
        Assert.Equal(1.0 / 60, TempestGame.SimulationStepSeconds);
        Assert.Equal(4.0, visibleTransitionIntervals * TempestGame.SimulationStepSeconds);
        Assert.Equal(0, renderer.ApproachDepthOffset);
        Assert.Equal(0, renderer.ApproachVerticalOffset);
        Assert.Empty(CaptureStars(renderer));
        Assert.Empty(CaptureMessage(renderer));
        Assert.All(game.Eggs, egg => Assert.Equal(0, egg.UpdateCounter));
    }

    [Fact]
    public void StarsStartAroundMidExitAndGrowToFullVisibilityBeforeCommit()
    {
        TempestGame game = new();
        VectorRenderer renderer = new(game);
        game.CurrentLevel.State = LevelState.ExitDelay;
        game.Player.AdvanceExit(Level.Depth * .75f);
        Assert.Empty(CaptureStars(renderer));

        game.Player.ResetDepth();
        game.CurrentLevel.State = LevelState.ExitZoom;
        Assert.Empty(CaptureStars(renderer));
        game.Player.AdvanceExit(Level.Depth * .5f);
        Assert.Empty(CaptureStars(renderer));
        game.Player.AdvanceExit(1);
        Assert.NotEmpty(CaptureStars(renderer));

        int previousCount = 0;
        while (game.Player.Depth > 12)
        {
            List<(Vector2 Position, Color Color)> stars = CaptureStars(renderer);
            Assert.InRange(stars.Count, previousCount, TransitionStarfield.MaximumStars);
            previousCount = stars.Count;
            game.Player.AdvanceExit(1);
        }
        Assert.Equal(TransitionStarfield.MaximumStars, CaptureStars(renderer).Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(17)]
    public void ColorPolicyUsesTheExitedLevelWithoutChangingTheDistribution(int exitedLevel)
    {
        Assert.Equal(288, TransitionStarfield.MaximumStars);
        List<(Vector2 Position, Color Color)> stars = CaptureStars(exitedLevel, 1);
        Assert.Equal(TransitionStarfield.MaximumStars, stars.Count);
        Assert.Equal(stars, CaptureStars(exitedLevel, 1));
        Assert.Equal(CaptureStars(1, 1).Select(star => star.Position),
            stars.Select(star => star.Position));
        Assert.Equal(stars.Count, stars.Select(star => star.Position).Distinct().Count());
        Assert.All(stars, star =>
        {
            Assert.True(float.IsFinite(star.Position.X));
            Assert.True(float.IsFinite(star.Position.Y));
            Assert.InRange(star.Position.X, 16, DisplayConfiguration.LogicalWidth - 16);
            Assert.InRange(star.Position.Y, 112, DisplayConfiguration.LogicalHeight - 16);
            Assert.Equal(255, star.Color.A);
        });
        if (exitedLevel <= 4)
            Assert.All(stars, star => Assert.Equal(Brighter(Color.Blue), star.Color));
        else
        {
            Color[] allowed = [Color.White, Brighter(Color.Red), Brighter(Color.Green), Brighter(Color.Blue)];
            Assert.All(stars, star => Assert.Contains(star.Color, allowed));
            Assert.All(allowed, color => Assert.Contains(stars, star => star.Color.Equals(color)));
        }
    }

    [Fact]
    public void RendererUsesLevelFiveExitColorsWithoutAddingProductionProgression()
    {
        TempestGame game = new(LevelConfiguration.Level1 with { DisplayedLevel = 5 });
        game.CurrentLevel.State = LevelState.ExitZoom;
        game.Player.AdvanceExit(Level.Depth - 12);
        VectorRenderer renderer = new(game);
        Assert.Equal(CaptureStars(5, 1), CaptureStars(renderer));
        game.RunOneStep();
        Assert.Equal(LevelState.Complete, game.State);
        Assert.Empty(CaptureStars(renderer));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void StarfieldSurvivesNaturalCommitThenDisappearsBeforeArrival(int incomingLevel)
    {
        TempestGame game = EnterExitFrom(incomingLevel - 1);
        VectorRenderer renderer = new(game);
        game.Player.AdvanceExit(Level.Depth - 12);
        List<(Vector2 Position, Color Color)> exitStars = CaptureStars(renderer);
        Assert.Equal(TransitionStarfield.MaximumStars, exitStars.Count);
        Assert.Empty(CaptureMessage(renderer));
        game.RunOneStep();

        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(incomingLevel, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(incomingLevel - 1, game.CurrentApproach!.ExitedLevel);
        Assert.Equal(exitStars, CaptureStars(renderer));
        Level incoming = game.CurrentLevel;
        var eggs = game.Eggs.Select(egg => (egg, egg.State, egg.Height, egg.UpdateCounter)).ToArray();
        int reserve = incoming.ReserveEggs;
        int originalCount = incoming.Level3Ledger?.UnresolvedOriginalIds.Count ?? 0;
        int score = game.Score.Score;
        int lives = game.Player.Lives;
        int previousCount = exitStars.Count;
        float previousOpacity = 1;
        int firstEmptyTick = -1;
        var initialPlayer = (game.Player.EdgeIndex, game.Player.EdgePosition);
        bool playerMoved = false;

        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
        {
            Assert.Equal(LevelState.Approach, game.State);
            Assert.Equal(tick, game.CurrentApproach!.ElapsedTicks);
            float opacity = TransitionStarfield.ApproachVisibility(game.CurrentApproach.Progress);
            Assert.InRange(opacity, 0, previousOpacity);
            previousOpacity = opacity;
            List<(Vector2 Position, Color Color)> stars = CaptureStars(renderer);
            Assert.Equal(stars, CaptureStars(renderer));
            Assert.InRange(stars.Count, 0, previousCount);
            previousCount = stars.Count;
            Assert.Equal(exitStars.Take(stars.Count).Select(star => star.Position),
                stars.Select(star => star.Position));
            Assert.All(stars, star =>
            {
                Assert.Equal(Brighter(Color.Blue).R, star.Color.R);
                Assert.Equal(Brighter(Color.Blue).G, star.Color.G);
                Assert.Equal(Brighter(Color.Blue).B, star.Color.B);
            });
            if (stars.Count == 0 && firstEmptyTick < 0)
                firstEmptyTick = tick;
            if (tick >= MathF.Ceiling(LevelApproach.DurationTicks *
                TransitionStarfield.ApproachDisappearanceProgress))
                Assert.Empty(stars);

            var message = Assert.Single(CaptureMessage(renderer));
            Assert.Equal("SUPERZAPPER RECHARGE", message.Text);
            Assert.InRange(message.Y, 500, DisplayConfiguration.LogicalHeight - message.Size);
            Assert.Equal(20, message.Size);
            Assert.Equal(Color.White, message.Color);
            Assert.Empty(renderer.GetWaveHudLines());
            AssertPlayerAttachedToRim(game, renderer);
            playerMoved |= initialPlayer != (game.Player.EdgeIndex, game.Player.EdgePosition);
            Assert.Equal(96 * (game.CurrentApproach.DepthOffset / LevelApproach.InitialDepthOffset),
                renderer.ApproachVerticalOffset);
            Assert.Same(incoming, game.CurrentLevel);
            Assert.Equal(eggs, game.Eggs.Select(egg =>
                (egg, egg.State, egg.Height, egg.UpdateCounter)).ToArray());
            Assert.Equal(reserve, incoming.ReserveEggs);
            Assert.Equal(originalCount, incoming.Level3Ledger?.UnresolvedOriginalIds.Count ?? 0);
            Assert.Equal(score, game.Score.Score);
            Assert.Equal(lives, game.Player.Lives);
            Assert.Empty(game.Shots);
            Assert.Equal(2, game.SuperZapperChargesRemaining);
            Assert.Equal(70f, Projection.K);
            Assert.Equal(5f, Projection.ZOffset);
            game.RunOneStep(new PlayerInput(true, 8, false, false, ZapPressed: true));
        }

        Assert.Equal(154, firstEmptyTick);
        Assert.True(playerMoved);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
        Assert.Equal(0, renderer.ApproachVerticalOffset);
        Assert.Equal(0, renderer.ApproachDepthOffset);
        Assert.Empty(CaptureStars(renderer));
        Assert.Empty(CaptureMessage(renderer));
        Assert.All(game.Eggs, egg => Assert.Equal(0, egg.UpdateCounter));
        AssertPlayerAttachedToRim(game, renderer);
        game.RunOneStep();
        Assert.Equal(1, game.Eggs[0].UpdateCounter);
    }

    [Theory]
    [InlineData(LevelState.Playing)]
    [InlineData(LevelState.ExitDelay)]
    [InlineData(LevelState.AvoidSpikes)]
    [InlineData(LevelState.ReverseZoom)]
    [InlineData(LevelState.PlayerDying)]
    [InlineData(LevelState.Complete)]
    [InlineData(LevelState.GameOver)]
    public void OtherStatesShowNeitherStarsNorRechargeMessage(LevelState state)
    {
        TempestGame game = EnterExitFrom(1);
        game.Player.AdvanceExit(Level.Depth - 12);
        game.RunOneStep();
        Assert.NotNull(game.CurrentApproach);
        game.CurrentLevel.State = state;
        VectorRenderer renderer = new(game);
        Assert.Empty(CaptureStars(renderer));
        Assert.Empty(CaptureMessage(renderer));
    }

    [Fact]
    public void RepeatedRenderingDoesNotConsumeGameplayRandomnessOrChangeSimulation()
    {
        CountingRandom renderedRandom = new();
        CountingRandom controlRandom = new();
        TempestGame rendered = new(random: renderedRandom);
        TempestGame control = new(random: controlRandom);
        rendered.CurrentLevel.State = LevelState.ExitZoom;
        control.CurrentLevel.State = LevelState.ExitZoom;
        VectorRenderer renderer = new(rendered);

        for (int tick = 0; tick < 300; tick++)
        {
            int callsBeforeDrawing = renderedRandom.Calls;
            for (int frame = 0; frame < 3; frame++)
            {
                CaptureStars(renderer);
                CaptureMessage(renderer);
            }
            Assert.Equal(callsBeforeDrawing, renderedRandom.Calls);
            PlayerInput input = new(false, 1, false, false);
            float previousK = Projection.K;
            float previousZOffset = Projection.ZOffset;
            rendered.RunOneStep(input);
            float renderedK = Projection.K;
            float renderedZOffset = Projection.ZOffset;
            // Projection is shared mutable state; give both simulations the same starting values.
            Projection.K = previousK;
            Projection.ZOffset = previousZOffset;
            control.RunOneStep(input);
            Assert.Equal(renderedK, Projection.K);
            Assert.Equal(renderedZOffset, Projection.ZOffset);
            Assert.Equal(controlRandom.Calls, renderedRandom.Calls);
            Assert.Equal(control.State, rendered.State);
            Assert.Equal(control.Player.Depth, rendered.Player.Depth);
            Assert.Equal(control.Player.EdgeIndex, rendered.Player.EdgeIndex);
            Assert.Equal(control.Player.EdgePosition, rendered.Player.EdgePosition);
            Assert.Equal(control.CurrentApproach?.ElapsedTicks, rendered.CurrentApproach?.ElapsedTicks);
            Assert.Equal(control.Score.Score, rendered.Score.Score);
            Assert.Equal(control.CurrentLevel.ReserveEggs, rendered.CurrentLevel.ReserveEggs);
            Assert.Equal(control.Eggs.Select(egg => (egg.EdgeIndex, egg.State, egg.Height, egg.UpdateCounter)),
                rendered.Eggs.Select(egg => (egg.EdgeIndex, egg.State, egg.Height, egg.UpdateCounter)));
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void BrightnessPreservesAcceptedPositionsCountsAndAlphaEnvelope(int exitedLevel)
    {
        List<(Vector2 Position, Color Color)> fullField = CaptureStars(exitedLevel, 1);
        Color[] originalPalette = [Color.White, Color.Red, Color.Green, Color.Blue];
        for (int step = 0; step <= 1000; step++)
        {
            float visibility = step / 1000f;
            List<(Vector2 Position, Color Color)> actual = CaptureStars(exitedLevel, visibility);
            List<byte> acceptedAlphas = [];
            if (MathF.Round(255 * visibility * 192) != 0)
            {
                for (int index = 0; index < 288; index++)
                {
                    byte alpha = (byte)MathF.Round(255 * Math.Clamp(visibility * 288 - index, 0, 1));
                    if (alpha == 0)
                        break;
                    acceptedAlphas.Add(alpha);
                }
            }
            Assert.Equal(acceptedAlphas.Count, actual.Count);
            for (int index = 0; index < actual.Count; index++)
            {
                Assert.Equal(fullField[index].Position, actual[index].Position);
                Assert.Equal(acceptedAlphas[index], actual[index].Color.A);
                Color original = exitedLevel <= 4 ? Color.Blue : originalPalette[index % 4];
                Color expected = Brighter(original);
                Color color = actual[index].Color;
                Assert.Equal(expected.R, color.R);
                Assert.Equal(expected.G, color.G);
                Assert.Equal(expected.B, color.B);
                float Luminance(Color value) => .2126f * value.R + .7152f * value.G + .0722f * value.B;
                if (original.Equals(Color.White))
                    Assert.Equal(Luminance(original), Luminance(color));
                else
                    Assert.True(Luminance(color) > Luminance(original));
            }
        }
    }

    private static Color Brighter(Color color) => new(
        (byte)MathF.Min(255, MathF.Round(color.R * 1.6f)),
        (byte)MathF.Min(255, MathF.Round(color.G * 1.6f)),
        (byte)MathF.Min(255, MathF.Round(color.B * 1.6f)), color.A);

    private static TempestGame EnterExitFrom(int displayedLevel)
    {
        TempestGame game = new();
        for (int level = 1; level < displayedLevel; level++)
        {
            game.CurrentLevel.State = LevelState.ExitZoom;
            game.Player.AdvanceExit(Level.Depth - 12);
            game.RunOneStep();
            for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
                game.RunOneStep();
        }
        game.CurrentLevel.State = LevelState.ExitZoom;
        return game;
    }

    private static List<(Vector2 Position, Color Color)> CaptureStars(VectorRenderer renderer)
    {
        List<(Vector2 Position, Color Color)> stars = [];
        renderer.DrawTransitionStars((position, color) => stars.Add((position, color)));
        return stars;
    }

    private static List<(Vector2 Position, Color Color)> CaptureStars(int exitedLevel, float visibility)
    {
        List<(Vector2 Position, Color Color)> stars = [];
        TransitionStarfield.Draw(exitedLevel, visibility, (position, color) => stars.Add((position, color)));
        return stars;
    }

    private static List<(string Text, int Y, int Size, Color Color)> CaptureMessage(VectorRenderer renderer)
    {
        List<(string Text, int Y, int Size, Color Color)> messages = [];
        renderer.DrawRechargeMessage((text, y, size, color) => messages.Add((text, y, size, color)));
        return messages;
    }

    private static void AssertPlayerAttachedToRim(TempestGame game, VectorRenderer renderer)
    {
        List<(Vector2 Near, Vector2 Far)> lines = [];
        renderer.DrawTube((near, far, _) => lines.Add((near, far)));
        renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points, out Vector2 start, out Vector2 end);
        Assert.Equal(lines[game.Player.EdgeIndex].Near, start);
        Assert.Equal(lines[game.CurrentLevel.NextEdge(game.Player.EdgeIndex)].Near, end);
    }

    private sealed class CountingRandom : Random
    {
        public CountingRandom() : base(29) { }
        public int Calls { get; private set; }
        public override int Next(int minValue, int maxValue)
        {
            Calls++;
            return base.Next(minValue, maxValue);
        }
    }
}
