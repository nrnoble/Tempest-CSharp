using System.Numerics;
using Tempest.Game;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class GameOverVectorBurstSequenceTests
{
    [Fact]
    public void StartsFourBurstsHalfwayThroughEachSpawnWindowWhilePriorBurstsRemainActive()
    {
        static int AlwaysMinimum(int minimum, int maximum) => minimum;

        GameOverVectorBurstSequence sequence = new(AlwaysMinimum);
        sequence.Start();

        Assert.Equal(GameOverVectorBurstSequence.BurstCount, sequence.Bursts.Count);
        Assert.Equal(1, sequence.StartedBurstCount);
        Assert.True(sequence.Bursts[0].IsStarted);
        Assert.All(sequence.Bursts.Skip(1), burst => Assert.False(burst.IsStarted));
        Assert.Equal(
            GameOverVectorBurstSequence.SpawnWindowTicks / 2,
            GameOverVectorBurstSequence.BurstStartIntervalTicks);

        Step(sequence, GameOverVectorBurstSequence.BurstStartIntervalTicks);
        Assert.Equal(16, sequence.Bursts[0].SpawnCounter);
        sequence.Advance();
        Assert.Equal(2, sequence.StartedBurstCount);
        Assert.True(sequence.Bursts[0].ActiveRecordCount > 0);
        Assert.False(sequence.Bursts[0].IsComplete);

        Step(sequence, GameOverVectorBurstSequence.BurstStartIntervalTicks - 1);
        Assert.Equal(16, sequence.Bursts[1].SpawnCounter);
        sequence.Advance();
        Assert.Equal(3, sequence.StartedBurstCount);
        Assert.True(sequence.Bursts[1].ActiveRecordCount > 0);
        Assert.False(sequence.Bursts[1].IsComplete);

        Step(sequence, GameOverVectorBurstSequence.BurstStartIntervalTicks - 1);
        Assert.Equal(16, sequence.Bursts[2].SpawnCounter);
        sequence.Advance();
        Assert.Equal(GameOverVectorBurstSequence.BurstCount, sequence.StartedBurstCount);
        Assert.True(sequence.Bursts[2].ActiveRecordCount > 0);
        Assert.False(sequence.Bursts[2].IsComplete);
        Assert.All(sequence.Bursts, burst =>
        {
            Assert.Equal(GameOverVectorAnimation.RecordCapacity, burst.Records.Count);
            Assert.Equal(GameOverVectorAnimation.RecordCapacity, burst.ActiveRecordCount);
        });
        Assert.False(sequence.IsComplete);
    }

    [Fact]
    public void BurstParentPatternsAreSeededButDifferBetweenEvents()
    {
        static GameOverVectorBurstSequence CreateSequence(int seed)
        {
            Random random = new(seed);
            GameOverVectorBurstSequence sequence = new((minimum, maximum) =>
                random.Next(minimum, maximum + 1));
            sequence.Start();
            Step(sequence, GameOverVectorBurstSequence.BurstStartIntervalTicks *
                (GameOverVectorBurstSequence.BurstCount - 1) + 1);
            return sequence;
        }

        GameOverVectorBurstSequence first = CreateSequence(313);
        GameOverVectorBurstSequence repeated = CreateSequence(313);

        Assert.Equal(GameOverVectorBurstSequence.BurstCount, first.StartedBurstCount);
        for (int burstIndex = 0; burstIndex < GameOverVectorBurstSequence.BurstCount; burstIndex++)
        {
            Assert.Equal(Snapshot(first.Bursts[burstIndex]), Snapshot(repeated.Bursts[burstIndex]));
            Assert.Equal(burstIndex, first.Bursts[burstIndex].BurstIndex);
        }

        string[] patterns = first.Bursts
            .Select(burst => string.Join("|", burst.Records.Select(record =>
                record is null
                    ? "-"
                    : $"{record.VelocityX},{record.VelocityY},{record.VelocityZ}")))
            .ToArray();
        Assert.Equal(GameOverVectorBurstSequence.BurstCount, patterns.Distinct().Count());
    }

    [Fact]
    public void ConcurrentBurstsIncreaseMarkCountWithoutChangingMarkLength()
    {
        TempestGame game = new(random: new Random(23));
        game.CurrentLevel.State = LevelState.GameOverAnimation;
        game.GameOverEffect.Start();
        Step(game, GameOverVectorBurstSequence.BurstStartIntervalTicks *
            (GameOverVectorBurstSequence.BurstCount - 1) + 1);

        Assert.Equal(GameOverVectorBurstSequence.BurstCount, game.GameOverEffect.StartedBurstCount);
        Assert.True(game.GameOverEffect.ActiveRecordCount >
            game.GameOverEffect.Bursts[0].ActiveRecordCount);

        List<(Vector2 Start, Vector2 End)> marks = [];
        new VectorRenderer(game).DrawGameOverAnimation((start, end, _) =>
            marks.Add((start, end)));

        Assert.Equal(game.GameOverEffect.ActiveRecordCount * 4, marks.Count);
        Assert.True(marks.Count > game.GameOverEffect.Bursts[0].ActiveRecordCount * 4);
        Assert.All(marks, mark =>
            Assert.Equal(2, Vector2.Distance(mark.Start, mark.End), 4));
    }

    private static void Step(GameOverVectorBurstSequence sequence, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            sequence.Advance();
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }

    private static (byte X, byte Y, byte Z, sbyte VelocityX, sbyte VelocityY, sbyte VelocityZ,
        int SpawnOrdinal)?[] Snapshot(GameOverVectorAnimation burst)
    {
        List<(byte X, byte Y, byte Z, sbyte VelocityX, sbyte VelocityY, sbyte VelocityZ,
            int SpawnOrdinal)?> records = [];
        foreach (GameOverVectorRecord? record in burst.Records)
        {
            records.Add(record is null
                ? null
                : (record.X, record.Y, record.Z, record.VelocityX, record.VelocityY,
                    record.VelocityZ, record.SpawnOrdinal));
        }
        return records.ToArray();
    }
}
