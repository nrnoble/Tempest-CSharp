using System.Globalization;
using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class GameOverDensityDiagnosticTests
{
    [Fact]
    public void DensityOptionDefaultsToAcceptedTwoAndParsesInvariantDecimals()
    {
        Assert.True(DebugController.TryParseGameOverDensityMultiplier(
            [], out double omitted, out string? omittedError));
        Assert.Equal(2, omitted);
        Assert.Null(omittedError);

        Assert.True(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", "2"], out double explicitTwo, out string? explicitTwoError));
        Assert.Equal(2, explicitTwo);
        Assert.Null(explicitTwoError);

        Assert.True(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", "0.5"], out double half, out string? halfError));
        Assert.Equal(0.5, half);
        Assert.Null(halfError);

        Assert.True(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", "1"], out double one, out string? oneError));
        Assert.Equal(1, one);
        Assert.Null(oneError);

        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            Assert.True(DebugController.TryParseGameOverDensityMultiplier(
                ["--game-over-density", "2.5"], out double parsed, out string? error));
            Assert.Equal(2.5, parsed);
            Assert.Null(error);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("NaN")]
    [InlineData("Infinity")]
    [InlineData("-Infinity")]
    [InlineData("not-a-number")]
    public void DensityOptionRejectsInvalidValues(string value)
    {
        Assert.False(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", value], out _, out string? error));
        Assert.Contains("positive finite", error);
    }

    [Fact]
    public void DensityOptionRejectsMissingAndRepeatedValues()
    {
        Assert.False(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density"], out _, out string? missingError));
        Assert.Contains("positive finite", missingError);

        Assert.False(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", "--lives", "1"], out _, out string? nextOptionError));
        Assert.Contains("positive finite", nextOptionError);

        Assert.False(DebugController.TryParseGameOverDensityMultiplier(
            ["--game-over-density", "2", "--game-over-density", "4"],
            out _, out string? repeatedError));
        Assert.Contains("only once", repeatedError);
    }

    [Fact]
    public void OmittedAndExplicitTwoRenderIdenticalGameOverMarks()
    {
        TempestGame game = CreateAnimationGame(23);
        game.RunOneStep();

        List<(Vector2 Start, Vector2 End, Color Color)> omitted =
            CaptureGameOverMarks(new VectorRenderer(game));
        List<(Vector2 Start, Vector2 End, Color Color)> explicitTwo =
            CaptureGameOverMarks(new VectorRenderer(game, 2));

        Assert.Equal(64, omitted.Count);
        Assert.Equal(omitted, explicitTwo);
    }

    [Theory]
    [InlineData(0.5, 16)]
    [InlineData(0.75, 24)]
    [InlineData(2, 64)]
    [InlineData(2.5, 80)]
    [InlineData(4, 128)]
    [InlineData(6, 192)]
    public void DensityScalesVisibleMarksDeterministically(double density, int expectedCount)
    {
        TempestGame game = CreateAnimationGame(23);
        game.RunOneStep();
        VectorRenderer renderer = new(game, density);

        List<(Vector2 Start, Vector2 End, Color Color)> first =
            CaptureGameOverMarks(renderer);
        List<(Vector2 Start, Vector2 End, Color Color)> repeated =
            CaptureGameOverMarks(renderer);

        Assert.Equal(expectedCount, first.Count);
        Assert.Equal(first, repeated);
        Assert.All(first, mark => Assert.Equal(2, Vector2.Distance(mark.Start, mark.End), 4));
        Assert.Equal(first.Count, first.Select(mark => (mark.Start, mark.End)).Distinct().Count());
        Assert.All(game.GameOverEffect.Bursts, burst =>
            Assert.Equal(GameOverVectorAnimation.RecordCapacity, burst.Records.Count));
        Assert.Empty(CaptureGameOverMessage(new VectorRenderer(game, density)));
    }

    [Fact]
    public void ChildSpecksAreStableCompactAndTwoDimensionalAroundTheirParent()
    {
        TempestGame game = CreateAnimationGame(71);
        game.RunOneStep();
        GameOverVectorRecord?[] records =
            Assert.IsType<GameOverVectorRecord?[]>(game.GameOverEffect.Bursts[0].Records);
        GameOverVectorRecord parent = records.First(record => record is not null)!;
        Array.Clear(records);
        records[0] = parent;

        VectorRenderer renderer = new(game, 4);
        List<(Vector2 Start, Vector2 End, Color Color)> first = CaptureGameOverMarks(renderer);
        List<(Vector2 Start, Vector2 End, Color Color)> repeated = CaptureGameOverMarks(renderer);

        Assert.Equal(8, first.Count);
        Assert.Equal(first, repeated);
        Assert.All(first, mark => Assert.Equal(2, Vector2.Distance(mark.Start, mark.End), 4));
        Assert.Single(first.Select(mark => mark.Color).Distinct());
        Assert.Equal(
            CaptureGameOverMarks(new VectorRenderer(game, 1)),
            first.Take(2));
        Assert.Equal(
            CaptureGameOverMarks(new VectorRenderer(game, 0.5)),
            first.Take(1));
        TempestGame sameSeedGame = CreateAnimationGame(71);
        sameSeedGame.RunOneStep();
        GameOverVectorRecord?[] sameSeedRecords =
            Assert.IsType<GameOverVectorRecord?[]>(sameSeedGame.GameOverEffect.Bursts[0].Records);
        GameOverVectorRecord sameSeedParent =
            sameSeedRecords.First(record => record is not null)!;
        Array.Clear(sameSeedRecords);
        sameSeedRecords[0] = sameSeedParent;
        Assert.Equal(first,
            CaptureGameOverMarks(new VectorRenderer(sameSeedGame, 4)));

        Vector2 parentCenter = Projection.Project(
            (parent.X - 0x80) * 1.5f,
            (parent.Y - 0x80) * 1.5f,
            Tempest.Game.Levels.Level.Position + parent.Z,
            game.CurrentLevel.Definition.CenterY,
            game.CurrentLevel.Definition.YOffset);
        Vector2[] offsets = first
            .Select(mark => (mark.Start + mark.End) / 2 - parentCenter)
            .ToArray();

        Assert.All(offsets, offset => Assert.InRange(offset.Length(), 0, 3.001f));
        Assert.Equal(offsets.Length, offsets.Distinct().Count());
        Assert.True(offsets.Max(offset => MathF.Abs(offset.X)) > 1);
        Assert.True(offsets.Max(offset => MathF.Abs(offset.Y)) > 1);

        Vector2 movement = new(parent.VelocityX, -parent.VelocityY);
        movement = movement.LengthSquared() > 0 ? Vector2.Normalize(movement) : Vector2.UnitX;
        Assert.True(offsets.Max(offset => MathF.Abs(
            offset.X * -movement.Y + offset.Y * movement.X)) > 1);

    }

    [Fact]
    public void DensityDoesNotChangeTheEmptyBlackEffectPresentation()
    {
        TempestGame game = CreateAnimationGame(29);
        VectorRenderer baseline = new(game);
        VectorRenderer dense = new(game, 4);

        Assert.Empty(CaptureGameOverMarks(baseline));
        Assert.Empty(CaptureGameOverMarks(dense));
        Assert.Empty(CaptureGameOverMessage(baseline));
        Assert.Empty(CaptureGameOverMessage(dense));
    }

    [Fact]
    public void DensityDoesNotChangeTextPresentationOrSeededRecordLifecycle()
    {
        TempestGame textGame = new(random: new Random(41));
        textGame.CurrentLevel.State = LevelState.GameOverPresentation;
        for (int tick = 0; tick < 32; tick++)
            textGame.RunOneStep();

        List<(Vector2 Start, Vector2 End, Color Color)> defaultText =
            CaptureGameOverMessage(new VectorRenderer(textGame));
        List<(Vector2 Start, Vector2 End, Color Color)> denseText =
            CaptureGameOverMessage(new VectorRenderer(textGame, 4));
        Assert.NotEmpty(defaultText);
        Assert.Equal(defaultText, denseText);

        TempestGame baseline = CreateAnimationGame(59);
        TempestGame dense = CreateAnimationGame(59);
        VectorRenderer baselineRenderer = new(baseline, 1);
        VectorRenderer denseRenderer = new(dense, 4);
        int initialScore = baseline.Score.Score;
        int initialLives = baseline.Player.Lives;
        int initialEdge = baseline.Player.EdgeIndex;
        int initialEnemiesRemaining = baseline.CurrentLevel.RemainingEnemies;
        int initialFlipperCount = baseline.Flippers.Count;
        int initialShotCount = baseline.Shots.Count;
        int initialZapperCharges = baseline.SuperZapper.ChargesRemaining;
        int ticks = 0;

        while (baseline.State != LevelState.GameOver && ticks < 600)
        {
            List<(Vector2 Start, Vector2 End, Color Color)> baselineMarks =
                CaptureGameOverMarks(baselineRenderer);
            List<(Vector2 Start, Vector2 End, Color Color)> denseMarks =
                CaptureGameOverMarks(denseRenderer);
            Assert.Equal(baselineMarks.Count * 4, denseMarks.Count);

            baseline.RunOneStep();
            dense.RunOneStep();
            Assert.Equal(baseline.State, dense.State);
            Assert.Equal(baseline.GameOverEffect.CreatedRecordCount,
                dense.GameOverEffect.CreatedRecordCount);
            Assert.Equal(baseline.GameOverEffect.ActiveRecordCount,
                dense.GameOverEffect.ActiveRecordCount);
            Assert.Equal(baseline.GameOverEffect.IsComplete, dense.GameOverEffect.IsComplete);
            for (int burstIndex = 0;
                burstIndex < GameOverVectorBurstSequence.BurstCount;
                burstIndex++)
            {
                Assert.Equal(
                    Snapshot(baseline.GameOverEffect.Bursts[burstIndex]),
                    Snapshot(dense.GameOverEffect.Bursts[burstIndex]));
            }
            Assert.Equal(
                baseline.GameOverEffect.Bursts.Select(burst => burst.SpawnCounter),
                dense.GameOverEffect.Bursts.Select(burst => burst.SpawnCounter));
            Assert.Equal(
                baseline.GameOverEffect.Bursts.Select(burst => burst.RomTimeCounter),
                dense.GameOverEffect.Bursts.Select(burst => burst.RomTimeCounter));
            Assert.All(dense.GameOverEffect.Bursts, burst =>
                Assert.Equal(GameOverVectorAnimation.RecordCapacity, burst.Records.Count));
            Assert.Equal(initialScore, dense.Score.Score);
            Assert.Equal(initialLives, dense.Player.Lives);
            Assert.Equal(initialEdge, dense.Player.EdgeIndex);
            Assert.Equal(initialEnemiesRemaining, dense.CurrentLevel.RemainingEnemies);
            Assert.Equal(initialFlipperCount, dense.Flippers.Count);
            Assert.Equal(initialShotCount, dense.Shots.Count);
            Assert.Equal(initialZapperCharges, dense.SuperZapper.ChargesRemaining);
            ticks++;
        }

        Assert.Equal(LevelState.GameOver, baseline.State);
        Assert.Equal(baseline.State, dense.State);
        Assert.True(baseline.GameOverEffect.IsComplete);
        Assert.True(dense.GameOverEffect.IsComplete);
        Assert.Equal(baseline.Score.Score, dense.Score.Score);
        Assert.Equal(baseline.Player.Lives, dense.Player.Lives);
    }

    private static TempestGame CreateAnimationGame(int seed)
    {
        TempestGame game = new(random: new Random(seed));
        game.CurrentLevel.State = LevelState.GameOverAnimation;
        game.GameOverEffect.Start();
        return game;
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureGameOverMarks(
        VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> marks = [];
        renderer.DrawGameOverAnimation((start, end, color) => marks.Add((start, end, color)));
        return marks;
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureGameOverMessage(
        VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawGameOverMessage((start, end, color) => lines.Add((start, end, color)));
        return lines;
    }

    private static (byte X, byte Y, byte Z, sbyte VelocityX, sbyte VelocityY, sbyte VelocityZ,
        int SpawnOrdinal)?[] Snapshot(GameOverVectorAnimation animation)
    {
        List<(byte X, byte Y, byte Z, sbyte VelocityX, sbyte VelocityY, sbyte VelocityZ,
            int SpawnOrdinal)?> records = [];
        foreach (GameOverVectorRecord? record in animation.Records)
        {
            records.Add(record is null
                ? null
                : (record.X, record.Y, record.Z, record.VelocityX, record.VelocityY,
                    record.VelocityZ, record.SpawnOrdinal));
        }
        return records.ToArray();
    }
}
