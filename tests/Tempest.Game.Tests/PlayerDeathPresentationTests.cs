using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class PlayerDeathPresentationTests
{
    [Fact]
    public void RetractionStartsAtDeathWithoutMovingLogicalEnemiesOrPersistentSpikes()
    {
        TempestGame game = PrepareDyingGame();
        VectorRenderer renderer = new(game);
        Flipper flipper = game.Flippers.MaxBy(candidate => candidate.Height) ??
            throw new InvalidOperationException("The death scenario has no active Flipper.");
        Flipper farFlipper = game.Flippers.MinBy(candidate => candidate.Height) ??
            throw new InvalidOperationException("The death scenario has no active Flipper.");
        Assert.Equal(Level.Depth, flipper.Height);
        Assert.True(flipper.Height > farFlipper.Height);
        Spike spike = Assert.Single(game.Spikes);
        List<(Vector2 Start, Vector2 End)> spikeLines = CaptureSpikes(renderer);

        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(0, game.DeathPresentation.ElapsedTicks);
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathPresentation.DurationTicks);
        Assert.True(game.DeathPresentation.DurationTicks * TempestGame.SimulationStepSeconds <= 2);
        Assert.Equal(0, game.EnemyRetractionProgress);

        float height = flipper.Height;
        int lane = flipper.EdgeIndex;
        FlipperState state = flipper.State;
        float turnAngle = flipper.TurnAngle;
        Assert.True(flipper.TryZap(13));
        (Vector2 Start, Vector2 End) initial = renderer.ProjectFlipperEndpoints(flipper);
        float initialLength = Vector2.Distance(initial.Start, initial.End);
        float initialNearScale = ProjectedScale(game, renderer, flipper);
        float initialFarScale = ProjectedScale(game, renderer, farFlipper);
        Assert.True(initialNearScale > initialFarScale);

        game.RunOneStep();

        (Vector2 Start, Vector2 End) retracted = renderer.ProjectFlipperEndpoints(flipper);
        float retractedNearScale = ProjectedScale(game, renderer, flipper);
        float retractedFarScale = ProjectedScale(game, renderer, farFlipper);
        Assert.Equal(1, game.DeathPresentation.ElapsedTicks);
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.True(Vector2.Distance(retracted.Start, retracted.End) < initialLength);
        Assert.True(retractedNearScale < initialNearScale);
        Assert.True(retractedFarScale < initialFarScale);
        Assert.True(retractedNearScale > retractedFarScale);
        AssertFinite(retracted.Start);
        AssertFinite(retracted.End);
        Assert.Equal(height, flipper.Height);
        Assert.Equal(lane, flipper.EdgeIndex);
        Assert.Equal(state, flipper.State);
        Assert.Equal(turnAngle, flipper.TurnAngle);
        Assert.Equal(13, flipper.PendingZapTicks);
        Assert.Equal(60, spike.Height);
        Assert.Equal(spikeLines, CaptureSpikes(renderer));
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);

        Step(game, TempestGame.PlayerDeathTicks - 2);
        Assert.Equal(TempestGame.PlayerDeathTicks - 1, game.DeathPresentation.ElapsedTicks);
        Assert.Equal((TempestGame.PlayerDeathTicks - 1f) / TempestGame.PlayerDeathTicks,
            game.EnemyRetractionProgress);
        Assert.Equal(height, flipper.Height);
        Assert.Equal(lane, flipper.EdgeIndex);

        game.RunOneStep();

        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Equal(0, game.DeathPresentation.ElapsedTicks);
        Assert.Equal(0, game.EnemyRetractionProgress);
        Assert.Same(spike, Assert.Single(game.Spikes));
        Assert.Equal(60, spike.Height);
        Assert.Equal(spikeLines, CaptureSpikes(renderer));
        Assert.Equal(0, game.Score.Score);
        Assert.Equal(0, game.CurrentLevel.EnemiesDestroyed);
    }

    [Fact]
    public void BlasterDeathEffectExpandsContractsAndLeavesAStableMulticolorRemnant()
    {
        TempestGame game = PrepareDyingGame();
        VectorRenderer renderer = new(game);
        Vector2 deathOrigin = GetPlayerDeathOrigin(game, renderer);

        List<(Vector2 Start, Vector2 End, Color Color)> initial =
            CapturePlayerDeathEffect(game, renderer);
        float initialRadius = MaximumRadius(initial, deathOrigin, new Color(255, 80, 0, 255));
        Assert.True(initialRadius > 0);
        AssertClose(deathOrigin, AverageEndpoint(initial));

        Step(game, 6);
        List<(Vector2 Start, Vector2 End, Color Color)> developed =
            CapturePlayerDeathEffect(game, renderer);
        float developedRadius = MaximumRadius(developed, deathOrigin, new Color(255, 80, 0, 255));
        Assert.True(developedRadius > initialRadius);
        AssertClose(deathOrigin, AverageEndpoint(developed));

        Step(game, 18);
        List<(Vector2 Start, Vector2 End, Color Color)> peak =
            CapturePlayerDeathEffect(game, renderer);
        float peakRadius = MaximumRadius(peak, deathOrigin, new Color(255, 80, 0, 255));
        Assert.True(peakRadius > developedRadius * 1.5f);
        Assert.Contains(peak, line => line.Color == Color.White);
        Assert.Contains(peak, line => line.Color == new Color(255, 80, 0, 255));
        Assert.Contains(peak, line => line.Color == Color.Yellow);
        AssertClose(deathOrigin, AverageEndpoint(peak));

        Step(game, 18);
        List<(Vector2 Start, Vector2 End, Color Color)> contracted =
            CapturePlayerDeathEffect(game, renderer);
        float contractedRadius = MaximumRadius(contracted, deathOrigin, new Color(255, 80, 0, 255));
        Assert.True(contractedRadius < peakRadius);

        Step(game, 16);
        List<(Vector2 Start, Vector2 End, Color Color)> remnant =
            CapturePlayerDeathEffect(game, renderer);
        float remnantRadius = MaximumRadius(remnant, deathOrigin, new Color(255, 80, 0, 255));
        Assert.True(remnantRadius > 0);
        Assert.True(remnantRadius < contractedRadius);

        Step(game, 2);
        Assert.Empty(CapturePlayerDeathEffect(game, renderer));
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
    }

    [Fact]
    public void FinalLifeCompletesRetractionAtExistingGameOverBoundaryAndRestartResetsIt()
    {
        TempestGame game = PrepareDyingGame();
        game.Player.Lives = 1;

        Step(game, TempestGame.PlayerDeathTicks);

        Assert.True(game.GameOver);
        Assert.Equal(0, game.Player.Lives);
        Assert.True(game.DeathPresentation.IsComplete);
        Assert.Equal(game.DeathPresentation.DurationTicks, game.DeathPresentation.ElapsedTicks);
        Assert.Equal(0, game.EnemyRetractionProgress);

        game.RunOneStep(restartPressed: true);

        Assert.False(game.GameOver);
        Assert.Equal(3, game.Player.Lives);
        Assert.False(game.DeathPresentation.IsComplete);
        Assert.Equal(0, game.DeathPresentation.ElapsedTicks);
        Assert.Equal(0, game.EnemyRetractionProgress);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RetractionUsesFiniteSharedProjectionOnLevelsOneThroughThree(int level)
    {
        TempestGame game = level switch
        {
            1 => new TempestGame(),
            2 => new TempestGame(level2Test: true),
            3 => new TempestGame(level3Test: true, random: new Random(1)),
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
        PrepareClimbingDeath(game);
        VectorRenderer renderer = new(game);
        Flipper nearRim = game.Flippers.MaxBy(candidate => candidate.Height) ??
            throw new InvalidOperationException("The death scenario has no active Flipper.");
        (Vector2 Start, Vector2 End) initial = renderer.ProjectFlipperEndpoints(nearRim);
        float initialLength = Vector2.Distance(initial.Start, initial.End);

        Assert.Equal(level, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Equal(0, game.EnemyRetractionProgress);
        Tanker? tanker = level == 3 ? game.Tankers.FirstOrDefault() : null;
        if (level == 3)
            Assert.NotNull(tanker);
        List<(Vector2 Start, Vector2 End)> initialTankerLines = [];
        int tankerOwner = tanker?.OriginalSlotId ?? -1;
        int latentCargo = tanker?.LatentCargoCount ?? 0;
        if (tanker is not null)
        {
            initialTankerLines = CaptureTankers(renderer);
            Assert.NotEmpty(initialTankerLines);
        }

        game.RunOneStep();

        (Vector2 Start, Vector2 End) retracted = renderer.ProjectFlipperEndpoints(nearRim);
        Assert.Equal(1f / TempestGame.PlayerDeathTicks, game.EnemyRetractionProgress);
        Assert.True(Vector2.Distance(retracted.Start, retracted.End) < initialLength);
        AssertFinite(retracted.Start);
        AssertFinite(retracted.End);
        if (tanker is not null)
        {
            List<(Vector2 Start, Vector2 End)> retractedTankerLines = CaptureTankers(renderer);
            Assert.True(Vector2.Distance(retractedTankerLines[0].Start,
                retractedTankerLines[0].End) <
                Vector2.Distance(initialTankerLines[0].Start, initialTankerLines[0].End));
            Assert.Equal(tankerOwner, tanker.OriginalSlotId);
            Assert.Equal(latentCargo, tanker.LatentCargoCount);
        }
    }

    private static TempestGame PrepareDyingGame()
    {
        TempestGame game = new();
        Egg egg = game.Eggs[0];
        egg.BeginApproach();
        for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(game.CurrentLevel);
        Assert.Equal(EggState.ReadyToHatch, egg.State);

        game.RunOneStep();
        game.Player.Move(egg.EdgeIndex - 1 - game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount);
        Step(game, 199);
        Assert.Equal(FlipperState.Pausing, game.Flippers[0].State);
        Step(game, 24);
        Assert.Equal(FlipperState.Flipping, game.Flippers[0].State);
        int pivotTicks = (int)Math.Ceiling(game.Flippers[0].TotalTurnAngle /
            game.CurrentLevel.RimTurnSpeedDegrees);
        game.CurrentLevel.CreateSpike(0, 60);
        Step(game, pivotTicks);
        Assert.Equal(LevelState.PlayerDying, game.State);
        return game;
    }

    private static void PrepareClimbingDeath(TempestGame game)
    {
        Flipper? playerTarget = null;
        for (int attempt = 0; attempt < 8 && playerTarget is null; attempt++)
        {
            Egg? egg = game.Eggs.FirstOrDefault(candidate =>
                candidate.State != EggState.ReadyToHatch);
            if (egg is null)
                break;

            egg.BeginApproach();
            for (int tick = 0; tick < 80 && egg.State != EggState.ReadyToHatch; tick++)
                egg.Update(game.CurrentLevel);
            game.RunOneStep();
            playerTarget = game.Flippers.FirstOrDefault(candidate => !candidate.IsTankerCargo);
        }

        Assert.NotNull(playerTarget);
        game.Player.Move(playerTarget.EdgeIndex - game.Player.EdgeIndex,
            game.CurrentLevel.PlayableLaneCount);
        for (int tick = 0; tick < 500 && game.State == LevelState.Playing; tick++)
            game.RunOneStep();
        Assert.Equal(LevelState.PlayerDying, game.State);
    }

    private static List<(Vector2 Start, Vector2 End)> CaptureSpikes(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawSpikes((start, end) => lines.Add((start, end)), _ => { });
        return lines;
    }

    private static List<(Vector2 Start, Vector2 End)> CaptureTankers(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTankers((start, end) => lines.Add((start, end)));
        return lines;
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CapturePlayerDeathEffect(
        TempestGame game, VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawPlayerExplosion(game.CurrentLevel.Definition.Points,
            (start, end, color) => lines.Add((start, end, color)));
        return lines;
    }

    private static Vector2 GetPlayerDeathOrigin(TempestGame game, VectorRenderer renderer)
    {
        renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points,
            out Vector2 p1, out Vector2 p2);
        return Vector2.Lerp(p2, p1, game.Player.EdgePosition);
    }

    private static float MaximumRadius(
        IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> lines,
        Vector2 center,
        Color color) =>
        lines.Where(line => line.Color == color)
            .SelectMany(line => new[] { line.Start, line.End })
            .Max(point => Vector2.Distance(point, center));

    private static Vector2 AverageEndpoint(
        IReadOnlyList<(Vector2 Start, Vector2 End, Color Color)> lines)
    {
        Vector2 sum = Vector2.Zero;
        foreach (var line in lines)
            sum += line.Start + line.End;
        return sum / (lines.Count * 2);
    }

    private static void AssertClose(Vector2 expected, Vector2 actual) =>
        Assert.InRange(Vector2.Distance(expected, actual), 0, 0.001f);

    private static float ProjectedScale(
        TempestGame game, VectorRenderer renderer, Flipper flipper)
    {
        (Vector2 Start, Vector2 End) projected = renderer.ProjectFlipperEndpoints(flipper);
        Vector2 worldStart;
        Vector2 worldEnd;
        if (flipper.State == FlipperState.Flipping)
        {
            DirectedLaneTransition transition = flipper.ActiveTransition ??
                throw new InvalidOperationException("A flipping Flipper has no transition.");
            float progress = flipper.TotalTurnAngle <= 0
                ? 1
                : Math.Clamp(flipper.TurnAngle / flipper.TotalTurnAngle, 0, 1);
            (worldStart, worldEnd) = game.CurrentLevel.CalculateFlipperPivotEndpoints(
                transition, progress);
        }
        else
        {
            IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
            worldStart = points[flipper.EdgeIndex];
            worldEnd = points[game.CurrentLevel.NextEdge(flipper.EdgeIndex)];
        }

        return Vector2.Distance(projected.Start, projected.End) /
            Vector2.Distance(worldStart, worldEnd);
    }

    private static void AssertFinite(Vector2 position)
    {
        Assert.True(float.IsFinite(position.X));
        Assert.True(float.IsFinite(position.Y));
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }
}
