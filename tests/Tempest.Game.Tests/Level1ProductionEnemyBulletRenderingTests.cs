using System.Numerics;
using Raylib_cs;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level1ProductionEnemyBulletRenderingTests
{
    [Fact]
    public void ProductionLevelOneBulletsAreActiveAndVisibleWithoutBulletTest()
    {
        TempestGame game = CreateGame();
        MakeStartingEggsReady(game);
        game.RunOneStep();

        Assert.Equal(2, game.EnemyBullets.Count);
        Assert.False(game.EnemyBulletsTest);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.True(new VectorRenderer(game).ShouldDrawEnemyBullets);
    }

    [Fact]
    public void ProductionLevelOneBulletSimulationAndColoredRenderingAreDeterministic()
    {
        RecordingAudioEvents firstAudio = new();
        RecordingAudioEvents repeatedAudio = new();
        TempestGame game = CreateGame(firstAudio);
        TempestGame repeated = CreateGame(repeatedAudio);
        MakeStartingEggsReady(game);
        MakeStartingEggsReady(repeated);
        VectorRenderer renderer = new(game);
        VectorRenderer repeatedRenderer = new(repeated);

        game.RunOneStep();
        repeated.RunOneStep();

        Assert.Equal(2, game.CurrentLevel.Configuration.MaximumActiveEnemyShots);
        Assert.Equal(4, game.Flippers.Count);
        Assert.Equal(new[] { 80, 80, 0, 0 },
            game.Flippers.Select(flipper => flipper.ShotDelayTicks));
        Assert.Equal(2, firstAudio.Events.Count(entry => entry == AudioEvent.EnemyBullet));
        Assert.Equal(firstAudio.Events, repeatedAudio.Events);
        for (int index = 0; index < 2; index++)
        {
            EnemyBullet bullet = game.EnemyBullets[index];
            Assert.Equal(game.Flippers[index].EdgeIndex, bullet.EdgeIndex);
            Assert.Equal(game.Flippers[index].Height, bullet.Height);
            Assert.Equal(0, bullet.Rotation);
        }

        EnemyBullet advancingBullet = game.EnemyBullets[0];
        float initialHeight = advancingBullet.Height;
        for (int tick = 0; tick <= 20; tick++)
        {
            Assert.Equal(2, game.EnemyBullets.Count);
            Assert.Equal(game.EnemyBullets.Select(BulletState),
                repeated.EnemyBullets.Select(BulletState));
            List<(Vector2 Start, Vector2 End, Color Color)> marks = Capture(renderer);
            Assert.Equal(Capture(repeatedRenderer), marks);
            Assert.Equal(4 * game.EnemyBullets.Count, marks.Count);
            Assert.Same(advancingBullet, game.EnemyBullets[0]);
            Assert.Equal(initialHeight + 3 * tick, advancingBullet.Height);
            Assert.Equal(15 * tick % 360, advancingBullet.Rotation);
            Assert.Equal(firstAudio.Events, repeatedAudio.Events);
            if (tick < 20)
            {
                game.RunOneStep();
                repeated.RunOneStep();
            }
        }
    }

    [Fact]
    public void DrawnBulletUsesTheSameStateThatAdvancesAndCollidesWithThePlayer()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = CreateGame(audio);
        MakeStartingEggsReady(game);
        game.RunOneStep();
        EnemyBullet bullet = game.EnemyBullets[0];
        for (int tick = 0; tick < 130; tick++)
        {
            game.RunOneStep();
            Assert.Contains(bullet, game.EnemyBullets);
        }
        Assert.Equal(Level.Depth - 8, bullet.Height);
        game.Player.ResetPosition(bullet.EdgeIndex);
        VectorRenderer renderer = new(game);

        Assert.False(bullet.HitsPlayer(game.Player));
        List<(Vector2 Start, Vector2 End, Color Color)> marks = Capture(renderer);
        Assert.Equal(4 * game.EnemyBullets.Count, marks.Count);
        Assert.Contains(bullet, game.EnemyBullets);

        game.RunOneStep();

        Assert.Equal(Level.Depth - 5, bullet.Height);
        Assert.True(bullet.HitsPlayer(game.Player));
        Assert.DoesNotContain(bullet, game.EnemyBullets);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Contains(AudioEvent.PlayerKilledByBullet, audio.Events);
        Assert.Equal(4 * game.EnemyBullets.Count, Capture(renderer).Count);
    }

    [Fact]
    public void SeparateEnemyBulletsTestKeepsItsExistingPresentation()
    {
        TempestGame legacy = new(random: new FiringRandom(), enemyBulletsTest: true);
        TempestGame baseline = CreateGame();
        MakeStartingEggsReady(legacy);
        MakeStartingEggsReady(baseline);
        legacy.RunOneStep();
        baseline.RunOneStep();

        Assert.True(legacy.EnemyBulletsTest);
        Assert.Equal(baseline.EnemyBullets.Select(BulletState),
            legacy.EnemyBullets.Select(BulletState));
        Assert.Equal(Capture(new VectorRenderer(baseline)),
            Capture(new VectorRenderer(legacy)));
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
    public void ProductionLevelsRetainBulletRendering(int displayedLevel)
    {
        TempestGame game = new(random: new Random(1981), startingLives: 1000,
            startingProductionLevel: displayedLevel);
        MakeStartingEggsReady(game);
        VectorRenderer renderer = new(game);
        for (int tick = 0; tick < 1000 && game.EnemyBullets.Count == 0; tick++)
            game.RunOneStep();

        Assert.True(renderer.ShouldDrawEnemyBullets);
        Assert.NotEmpty(game.EnemyBullets);
        Assert.Equal(4 * game.EnemyBullets.Count, Capture(renderer).Count);
    }

    [Fact]
    public void FourROMFramesKeepTheirVerifiedDisconnectedSegmentsAndPoints()
    {
        (EnemyShotVectorSegment[] Segments, Vector2[] Points)[] expected =
        [
            (
                [new(new(-11, 11), new(-17, 17)), new(new(-17, -11), new(-11, -17)),
                    new(new(17, -17), new(11, -11)), new(new(11, 17), new(17, 11))],
                [new(6, 6), new(-6, 6), new(-6, -6), new(6, -6)]),
            (
                [new(new(-18, 18), new(-18, 4)), new(new(-8, -14), new(-8, -22)),
                    new(new(18, -12), new(18, -4)), new(new(8, 14), new(8, 22))],
                [new(-3, 7), new(-7, 3), new(3, -7), new(7, 3)]),
            (
                [new(new(-17, 3), new(-23, -3)), new(new(-3, -23), new(3, -17)),
                    new(new(17, -3), new(23, 3)), new(new(3, 23), new(-3, 23))],
                [new(0, 8), new(-8, 0), new(0, -8), new(8, 0)]),
            (
                [new(new(-22, -8), new(-14, -8)), new(new(4, -18), new(12, -18)),
                    new(new(14, 8), new(22, 8)), new(new(-4, 18), new(-12, 18))],
                [new(-7, 3), new(-3, -7), new(7, 3), new(3, 7)])
        ];

        Assert.Equal(4, EnemyShotVectorProgram.Frames.Count);
        for (int frameIndex = 0; frameIndex < expected.Length; frameIndex++)
        {
            EnemyShotVectorFrame frame = EnemyShotVectorProgram.GetFrame(frameIndex);
            Assert.Equal(0, frame.SegmentRomColorIndex);
            Assert.Equal(3, frame.PointRomColorIndex);
            Assert.Equal(expected[frameIndex].Segments, frame.Segments);
            Assert.Equal(expected[frameIndex].Points, frame.Points);
            Assert.Equal(4, frame.Segments.Count);
            Assert.Equal(4, frame.Points.Count);
        }
    }

    [Fact]
    public void RenderingUsesOneSharedFrameWithLaneDepthAndWhitePointTranslations()
    {
        TempestGame game = CreateGame();
        MakeStartingEggsReady(game);
        game.RunOneStep();
        EnemyBullet[] bullets = game.EnemyBullets.ToArray();
        Assert.Equal(2, bullets.Length);
        bullets[1].Advance();
        bullets[1].Advance();
        Assert.NotEqual(bullets[0].Height, bullets[1].Height);
        Assert.NotEqual(bullets[0].Rotation, bullets[1].Rotation);

        VectorRenderer renderer = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        List<(Vector2 Position, Color Color)> points = [];
        renderer.DrawEnemyBullets(
            (start, end, color) => lines.Add((start, end, color)),
            (position, color) => points.Add((position, color)));

        EnemyShotVectorFrame frame =
            EnemyShotVectorProgram.GetFrame(game.EnemyShotAnimationFrame);
        AssertRenderedGeometry(game, renderer, frame, lines, points);

        float nearScale = 6 /
            ((Level.Position + Level.Depth - bullets[1].Height + Projection.ZOffset) /
                Projection.K) / EnemyShotVectorProgram.MaximumLogicalExtent;
        float farScale = 6 /
            ((Level.Position + Level.Depth - bullets[0].Height + Projection.ZOffset) /
                Projection.K) / EnemyShotVectorProgram.MaximumLogicalExtent;
        Assert.True(nearScale > farScale);
    }

    [Fact]
    public void SharedAnimationAdvancesEveryFourStepsAndRenderingDoesNotAdvanceIt()
    {
        TempestGame game = CreateGame();
        MakeStartingEggsReady(game);
        VectorRenderer renderer = new(game);
        Assert.False(renderer.ShouldDrawEnemyBullets);
        game.RunOneStep();
        Assert.True(renderer.ShouldDrawEnemyBullets);
        Assert.Equal(0, game.EnemyShotAnimationFrame);

        for (int frame = 0; frame < 4; frame++)
        {
            Assert.Equal(frame, game.EnemyShotAnimationFrame);
            List<(Vector2 Start, Vector2 End, Color Color)> firstLines = Capture(renderer);
            List<(Vector2 Position, Color Color)> firstPoints = [];
            renderer.DrawEnemyBullets((_, _, _) => { },
                (position, color) => firstPoints.Add((position, color)));
            List<(Vector2 Start, Vector2 End, Color Color)> repeatedLines = Capture(renderer);
            List<(Vector2 Position, Color Color)> repeatedPoints = [];
            renderer.DrawEnemyBullets((_, _, _) => { },
                (position, color) => repeatedPoints.Add((position, color)));
            Assert.Equal(firstLines, repeatedLines);
            Assert.Equal(firstPoints, repeatedPoints);
            AssertRenderedGeometry(game, renderer,
                EnemyShotVectorProgram.GetFrame(frame), firstLines, firstPoints);
            for (int step = 0; step < 4; step++)
                game.RunOneStep();
        }
        Assert.Equal(0, game.EnemyShotAnimationFrame);
    }

    private static void AssertRenderedGeometry(
        TempestGame game,
        VectorRenderer renderer,
        EnemyShotVectorFrame frame,
        List<(Vector2 Start, Vector2 End, Color Color)> lines,
        List<(Vector2 Position, Color Color)> points)
    {
        Assert.Equal(frame.Segments.Count * game.EnemyBullets.Count, lines.Count);
        Assert.Equal(frame.Points.Count * game.EnemyBullets.Count, points.Count);
        for (int bulletIndex = 0; bulletIndex < game.EnemyBullets.Count; bulletIndex++)
        {
            EnemyBullet bullet = game.EnemyBullets[bulletIndex];
            Vector2 center = renderer.ProjectEnemyBullet(bullet);
            float depth = Level.Position + Level.Depth - bullet.Height;
            float coordinateScale = 6 /
                ((depth + Projection.ZOffset) / Projection.K) /
                EnemyShotVectorProgram.MaximumLogicalExtent;
            for (int segmentIndex = 0; segmentIndex < frame.Segments.Count; segmentIndex++)
            {
                EnemyShotVectorSegment segment = frame.Segments[segmentIndex];
                Vector2 expectedStart = center +
                    new Vector2(segment.Start.X, -segment.Start.Y) * coordinateScale;
                Vector2 expectedEnd = center +
                    new Vector2(segment.End.X, -segment.End.Y) * coordinateScale;
                Assert.Equal((expectedStart, expectedEnd, Color.White),
                    lines[bulletIndex * frame.Segments.Count + segmentIndex]);
            }
            for (int pointIndex = 0; pointIndex < frame.Points.Count; pointIndex++)
            {
                Vector2 position = frame.Points[pointIndex];
                Vector2 expectedPoint = center +
                    new Vector2(position.X, -position.Y) * coordinateScale;
                Assert.Equal((expectedPoint, Color.White),
                    points[bulletIndex * frame.Points.Count + pointIndex]);
            }
        }
    }

    private static (int Lane, float Height, float Rotation) BulletState(EnemyBullet bullet) =>
        (bullet.EdgeIndex, bullet.Height, bullet.Rotation);

    private static List<(Vector2 Start, Vector2 End, Color Color)> Capture(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> marks = [];
        renderer.DrawEnemyBullets((start, end, color) => marks.Add((start, end, color)));
        return marks;
    }

    private static TempestGame CreateGame(IAudioEvents? audio = null) =>
        new(random: new FiringRandom(), audioEvents: audio);

    private static void MakeStartingEggsReady(TempestGame game)
    {
        foreach (Egg egg in game.Eggs)
        {
            egg.BeginApproach();
            for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
                egg.Update(game.CurrentLevel);
            Assert.Equal(EggState.ReadyToHatch, egg.State);
        }
    }

    private sealed class FiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == 0 && maxValue == 256 ? 255 : minValue;
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        public List<AudioEvent> Events { get; } = [];
        public void Play(AudioEvent audioEvent) => Events.Add(audioEvent);
    }
}
