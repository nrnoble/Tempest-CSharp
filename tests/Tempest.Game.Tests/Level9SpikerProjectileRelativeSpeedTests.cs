using System.Reflection;
using System.Text.Json;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level9SpikerProjectileRelativeSpeedTests
{
    [Fact]
    public void NormalProductionUsesExplicitSpeedWithoutConsumingRandomSpeedSelection()
    {
        CountingRandom random = new(0.5);
        TempestGame game = new(random: random, startingProductionLevel: 9);

        Spiker spiker = CreateSelectedSpiker(game);

        Assert.Equal(LevelConfiguration.Level9ProductionSpikerSpeed, spiker.Speed);
        Assert.Equal(0, random.DoubleCalls);
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(1.75f)]
    [InlineData(2.0f)]
    [InlineData(2.25f)]
    [InlineData(2.5f)]
    [InlineData(2.75f)]
    public void DiagnosticSpeedsStayBelowSharedProjectileRate(float requestedSpeed)
    {
        CountingRandom random = new(0.5);
        TempestGame game = new(random: random, startingProductionLevel: 9,
            level9SpikerSpeedOverride: requestedSpeed);

        Spiker spiker = CreateSelectedSpiker(game);
        spiker.Update(game.CurrentLevel, static () => false, allowGrowthRetreatShot: false);
        EnemyBullet projectile = new(spiker.EdgeIndex, spiker.Height);
        float initialSpikerHeight = spiker.Height;
        float initialProjectileHeight = projectile.Height;

        for (int step = 0; step < 10; step++)
        {
            spiker.Update(game.CurrentLevel, static () => false,
                allowGrowthRetreatShot: false);
            projectile.Advance();
        }

        float spikerTravel = spiker.Height - initialSpikerHeight;
        float projectileTravel = projectile.Height - initialProjectileHeight;

        Assert.Equal(requestedSpeed, spiker.Speed);
        Assert.Equal(0, random.DoubleCalls);
        Assert.Equal(requestedSpeed * 10, spikerTravel);
        Assert.Equal(30f, projectileTravel);
        Assert.True(spikerTravel < projectileTravel);
    }

    [Fact]
    public void DiagnosticSpeedCannotBeAppliedToAnotherProductionLevel()
    {
        Assert.Throws<ArgumentException>(() => new TempestGame(
            startingProductionLevel: 8, level9SpikerSpeedOverride: 2.5f));
    }

    [Fact]
    public void NormalLevelNineSpikerAndProjectileSeparateOverTenMatchedMovementSteps()
    {
        TempestGame game = new(startingProductionLevel: 9, random: new CountingRandom(0.5));
        Spiker spiker = CreateSelectedSpiker(game);

        spiker.Update(game.CurrentLevel, static () => false, allowGrowthRetreatShot: false);
        EnemyBullet projectile = new(spiker.EdgeIndex, spiker.Height);
        float initialSpikerHeight = spiker.Height;
        float initialProjectileHeight = projectile.Height;

        for (int step = 0; step < 10; step++)
        {
            spiker.Update(game.CurrentLevel, static () => false,
                allowGrowthRetreatShot: false);
            projectile.Advance();
        }

        float spikerTravel = spiker.Height - initialSpikerHeight;
        float projectileTravel = projectile.Height - initialProjectileHeight;
        float separationGrowth = (projectile.Height - spiker.Height) -
            (initialProjectileHeight - initialSpikerHeight);

        Assert.Equal(20f, spikerTravel);
        Assert.Equal(30f, projectileTravel);
        Assert.Equal(10f, separationGrowth);
        Assert.Equal(1.5, projectileTravel / spikerTravel);
        Assert.Equal(3f, projectileTravel / 10);
    }

    [Fact]
    public void LevelNineTankerFlipperAndProjectileAuthoritiesRemainUnchanged()
    {
        Level level = new(LevelConfiguration.Level9Production);
        Tanker tanker = new(edgeIndex: 3, originalSlotId: 0);
        Flipper flipper = new(edge: 3, height: 0);
        float initialTankerHeight = tanker.Height;

        tanker.UpdateForDevelopment();
        flipper.Update(level, playerEdge: 0);
        EnemyBullet projectile = new(edgeIndex: 3, height: 0);
        projectile.Advance();

        Assert.Equal(initialTankerHeight + Tanker.DevelopmentClimbSpeed, tanker.Height);
        Assert.Equal(LevelConfiguration.Level9Production.Flipper.ClimbSpeed,
            flipper.Height);
        Assert.Equal(3f, projectile.Height);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(2.5f)]
    public void RepeatedLevelNineSpikerFiringRemainsIntact(float? requestedSpeed)
    {
        LevelConfiguration testConfiguration =
            LevelConfiguration.Level9Production with
            {
                EnemySupply = 27,
                InitialActiveEggCount = 1,
                MaximumActiveEggs = 1,
                MaximumApproachingEggs = 1,
                ReserveReleaseIntervalTicks = 1000
            };
        TempestGame game = new(testConfiguration, random: new MinimumRandom(),
            level9SpikerSpeedOverride: requestedSpeed);
        MakeReady(Assert.Single(game.Eggs), game.CurrentLevel);

        game.RunOneStep();
        Spiker spiker = Assert.Single(game.Spikers);
        Assert.Equal(requestedSpeed ?? LevelConfiguration.Level9ProductionSpikerSpeed,
            spiker.Speed);
        game.RunOneStep();

        EnemyBullet firstShot = Assert.Single(game.EnemyBullets);
        Assert.Equal(spiker.Height, firstShot.Height);
        Assert.Equal(56, spiker.ShotDelayTicks);
        float initialShotHeight = firstShot.Height;
        game.RunOneStep();
        Assert.Equal(initialShotHeight + 3, firstShot.Height);

        bool sawRetreat = false;
        bool sawRepeatedAscent = false;
        for (int tick = 0; tick < 1000 && !sawRepeatedAscent; tick++)
        {
            game.RunOneStep();
            sawRetreat |= spiker.State == SpikerState.Retreating;
            sawRepeatedAscent = sawRetreat &&
                spiker.State == SpikerState.Building &&
                spiker.Height > 0;
        }

        Assert.True(sawRepeatedAscent);
        Assert.Contains(spiker, game.Spikers);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(2.5f)]
    public void LevelNineRecordingRestoresProductionOrDiagnosticSpeed(float? requestedSpeed)
    {
        GameplayStartupConfiguration startup = new(9, 10, 1.0, 20261111,
            level9SpikerSpeedOverride: requestedSpeed);
        PlayerInputRecorder recorder = new(startup);
        recorder.CaptureTick(0, default);

        WithTemporaryDirectory(directory =>
        {
            string path = Path.Combine(directory, "level9-speed.json");
            GameplayRecordingFile.SaveNew(path, recorder);
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
            JsonElement root = document.RootElement;

            Assert.Equal(2, root.GetProperty("Version").GetInt32());
            if (requestedSpeed is float diagnosticSpeed)
                Assert.Equal(diagnosticSpeed,
                    root.GetProperty("Level9SpikerSpeedOverride").GetSingle());
            else
                Assert.False(root.TryGetProperty("Level9SpikerSpeedOverride", out _));

            GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
            Assert.Equal(startup, recording.StartupConfiguration);
            GameplayPlaybackSession session = new(recording.StartupConfiguration,
                recording.Replay);
            VisiblePlaybackController visible = new(session);
            GameplayPlaybackRunResult headless = new HeadlessGameplayPlaybackRunner(recording)
                .RunToBoundary(1);
            GameplayPlaybackRunResult repeatedHeadless =
                new HeadlessGameplayPlaybackRunner(recording).RunToBoundary(1);

            Spiker visibleSpiker = CreateSelectedSpiker(session.Game);
            float expectedSpeed = requestedSpeed ??
                LevelConfiguration.Level9ProductionSpikerSpeed;

            Assert.Same(LevelConfiguration.Level9Production,
                visible.Session.Game.CurrentLevel.Configuration);
            Assert.Equal(expectedSpeed, visibleSpiker.Speed);
            Assert.Equal(9, headless.StateSummary.StartingDisplayedLevel);
            Assert.Equal(9, headless.StateSummary.CurrentDisplayedLevel);
            Assert.Equal(1, headless.StateSummary.NextRecordingFrame);
            Assert.Equal(headless.StateSummary.RandomSeed,
                repeatedHeadless.StateSummary.RandomSeed);
            Assert.Equal(headless.StateSummary.CurrentDisplayedLevel,
                repeatedHeadless.StateSummary.CurrentDisplayedLevel);
            Assert.Equal(headless.StateSummary.ActiveSpikerCount,
                repeatedHeadless.StateSummary.ActiveSpikerCount);
        });
    }

    private static Spiker CreateSelectedSpiker(TempestGame game)
    {
        Egg egg = game.Eggs[0];
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException(
                "The selected Level 9 production game has no mixed-wave ledger.");
        ledger.CommitOriginalAsSpiker(egg.OriginalSlotId);

        MethodInfo factory = typeof(TempestGame).GetMethod(
            "CreateLevel3MixedEnemy", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The mixed-wave enemy factory was not found.");
        return Assert.IsType<Spiker>(factory.Invoke(game, [egg]));
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void WithTemporaryDirectory(Action<string> action)
    {
        string directory = Path.Combine(Path.GetTempPath(),
            $"tempest-level9-speed-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            action(directory);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private sealed class CountingRandom(double value) : Random
    {
        public int DoubleCalls { get; private set; }

        public override double NextDouble()
        {
            DoubleCalls++;
            return value;
        }
    }

    private sealed class MinimumRandom : Random
    {
        public override int Next(int minValue, int maxValue) => minValue;
    }
}
