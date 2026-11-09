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
            AssertProductionGeometry(game, renderer, marks);
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
        AssertProductionGeometry(game, renderer, marks);
        Assert.Contains(bullet, game.EnemyBullets);

        game.RunOneStep();

        Assert.Equal(Level.Depth - 5, bullet.Height);
        Assert.True(bullet.HitsPlayer(game.Player));
        Assert.DoesNotContain(bullet, game.EnemyBullets);
        Assert.Equal(LevelState.PlayerDying, game.State);
        Assert.Contains(AudioEvent.PlayerKilledByBullet, audio.Events);
        Assert.Equal(8 * game.EnemyBullets.Count, Capture(renderer).Count);
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
        AssertProductionGeometry(game, renderer, Capture(renderer));
    }

    private static (int Lane, float Height, float Rotation) BulletState(EnemyBullet bullet) =>
        (bullet.EdgeIndex, bullet.Height, bullet.Rotation);

    private static List<(Vector2 Start, Vector2 End, Color Color)> Capture(VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> marks = [];
        renderer.DrawEnemyBullets((start, end, color) => marks.Add((start, end, color)));
        return marks;
    }

    private static void AssertProductionGeometry(TempestGame game, VectorRenderer renderer,
        List<(Vector2 Start, Vector2 End, Color Color)> marks)
    {
        Assert.Equal(8 * game.EnemyBullets.Count, marks.Count);
        for (int index = 0; index < game.EnemyBullets.Count; index++)
        {
            EnemyBullet bullet = game.EnemyBullets[index];
            IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
            Vector2 midpoint = (points[bullet.EdgeIndex] + points[bullet.EdgeIndex + 1 <
                points.Count ? bullet.EdgeIndex + 1 : 0]) / 2;
            float depth = Level.Position + Level.Depth - bullet.Height;
            Vector2 center = Projection.Project(midpoint.X, midpoint.Y, depth,
                game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
            Assert.Equal(center, renderer.ProjectEnemyBullet(bullet));
            float radius = 6 / ((depth + Projection.ZOffset) / Projection.K);
            for (int spoke = 0; spoke < 8; spoke++)
            {
                float radians = (bullet.Rotation + 45 * spoke) * MathF.PI / 180;
                Vector2 offset = new(MathF.Cos(radians) * radius, MathF.Sin(radians) * radius);
                Assert.Equal((center - offset, center + offset, Color.White),
                    marks[index * 8 + spoke]);
            }
        }
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
