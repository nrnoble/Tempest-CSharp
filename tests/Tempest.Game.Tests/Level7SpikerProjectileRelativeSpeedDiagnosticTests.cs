using System.Globalization;
using System.Reflection;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;
using Xunit.Abstractions;

namespace Tempest.Game.Tests;

public sealed class Level7SpikerProjectileRelativeSpeedDiagnosticTests(ITestOutputHelper output)
{
    [Fact]
    public void MeasuresSpikerAndProjectileMovementAtRepresentativeLevel7Speeds()
    {
        SpeedCase[] cases =
        [
            new(2.625f, 78.75f, 11.25f, 8d / 7),
            new(2.8125f, 84.375f, 5.625f, 16d / 15),
            new(3f, 90f, 0f, 1d),
            new(4.5f, 135f, -45f, 2d / 3)
        ];

        foreach (SpeedCase speedCase in cases)
        {
            Measurement[] measurements = Measure(speedCase.Speed);
            output.WriteLine(FormattableString.Invariant(
                $"Spiker speed={speedCase.Speed:0.000}/step; projectile speed=3.000/step"));
            output.WriteLine(
                "step | Spiker depth | projectile depth | Spiker moved | projectile moved | separation | separation change | projectile/Spiker travel");

            foreach (Measurement measurement in measurements)
            {
                Assert.Equal(speedCase.Speed * measurement.ActiveStep, measurement.SpikerDistance);
                Assert.Equal(3f * measurement.ActiveStep, measurement.ProjectileDistance);
                Assert.Equal((3f - speedCase.Speed) * measurement.ActiveStep,
                    measurement.SeparationChange);

                string ratio = measurement.TravelRatio?.ToString("0.000000",
                    CultureInfo.InvariantCulture) ?? "n/a";
                output.WriteLine(string.Format(CultureInfo.InvariantCulture,
                    "{0,4} | {1,12:0.000} | {2,16:0.000} | {3,12:0.000} | " +
                    "{4,16:0.000} | {5,10:0.000} | {6,17:0.000} | {7}",
                    measurement.ActiveStep,
                    measurement.SpikerDepth,
                    measurement.ProjectileDepth,
                    measurement.SpikerDistance,
                    measurement.ProjectileDistance,
                    measurement.Separation,
                    measurement.SeparationChange,
                    ratio));
            }

            Measurement final = measurements[^1];
            Assert.Equal(30, final.ActiveStep);
            Assert.Equal(speedCase.SpikerDistanceAt30, final.SpikerDistance);
            Assert.Equal(90f, final.ProjectileDistance);
            Assert.Equal(speedCase.SeparationChangeAt30, final.SeparationChange);
            Assert.Equal(speedCase.TravelRatio, final.TravelRatio);
        }
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(1.5f)]
    [InlineData(2f)]
    [InlineData(2.5f)]
    [InlineData(2.625f)]
    [InlineData(2.8125f)]
    [InlineData(3f)]
    public void Level7OverrideSelectsTheRequestedSpikerSpeedAndLeavesProjectileMovementUnchanged(
        float requestedSpeed)
    {
        CountingRandom random = new(0.5);
        TempestGame game = new(random: random, startingProductionLevel: 7,
            level7SpikerSpeedOverride: requestedSpeed);

        Spiker spiker = CreateSelectedSpiker(game);
        EnemyBullet projectile = new(spiker.EdgeIndex, spiker.Height);

        Assert.Equal(requestedSpeed, spiker.Speed);
        Assert.Equal(0, random.DoubleCalls);
        projectile.Advance();
        Assert.Equal(spiker.Height + 3f, projectile.Height);
    }

    [Theory]
    [InlineData(1f)]
    [InlineData(1.5f)]
    [InlineData(2f)]
    [InlineData(2.5f)]
    public void StartupConfigurationAppliesTheRecordedLevel7DiagnosticSpeed(float requestedSpeed)
    {
        GameplayStartupConfiguration startup = new(7, 10, 1.0, 1981,
            requestedSpeed);
        TempestGame game = new(startup);

        Spiker spiker = CreateSelectedSpiker(game);

        Assert.Equal(requestedSpeed, spiker.Speed);
    }

    [Fact]
    public void NormalLevel7SpikerUsesTankerClimbRateWithoutChangingTankerMovement()
    {
        CountingRandom random = new(0.5);
        TempestGame game = new(startingProductionLevel: 7, random: random);

        Spiker spiker = CreateSelectedSpiker(game);
        Tanker tanker = new(edgeIndex: 2, originalSlotId: 1);
        float initialTankerHeight = tanker.Height;
        tanker.UpdateForDevelopment();

        Assert.Equal(Tanker.DevelopmentClimbSpeed, tanker.ClimbSpeed);
        Assert.Equal(tanker.ClimbSpeed, spiker.Speed);
        Assert.Equal(initialTankerHeight + Tanker.DevelopmentClimbSpeed, tanker.Height);
        Assert.Equal(0, random.DoubleCalls);
    }

    [Fact]
    public void LegacyTankerMatchMetadataKeepsTheAcceptedDefaultSpikerMovement()
    {
        GameplayStartupConfiguration startup = new(7, 10, 1.0, 1981,
            level7SpikerMatchesTankerSpeed: true);
        TempestGame game = new(startup);

        Spiker spiker = CreateSelectedSpiker(game);

        Assert.Equal(Tanker.DevelopmentClimbSpeed, spiker.Speed);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(8)]
    public void SpikerSelectionOutsideLevel7KeepsItsExistingSpeedFormula(int displayedLevel)
    {
        CountingRandom random = new(0.5);
        LevelConfiguration configuration = LevelProgression.GetProductionLevel(displayedLevel)
            ?? throw new InvalidOperationException("The selected production level is not implemented.");
        TempestGame game = new(spikerDevelopmentWave: false,
            startingConfiguration: configuration with { }, random: random);

        Spiker spiker = CreateSelectedSpiker(game);

        Assert.Equal((displayedLevel - 1) / 48f + 2.5f + 1f, spiker.Speed);
        Assert.Equal(1, random.DoubleCalls);
    }

    [Fact]
    public void Level7WithoutOverrideUsesTheAcceptedTankerMovementRate()
    {
        CountingRandom random = new(0.5);
        TempestGame game = new(random: random, startingProductionLevel: 7);

        Spiker spiker = CreateSelectedSpiker(game);

        Assert.Equal(Tanker.DevelopmentClimbSpeed, spiker.Speed);
        Assert.Equal(0, random.DoubleCalls);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(8)]
    public void NonLevel7MixedLevelsKeepTheirProductionSpikerSpeedSelectionWhenOverrideIsSet(
        int displayedLevel)
    {
        CountingRandom random = new(0.5);
        LevelConfiguration configuration = LevelProgression.GetProductionLevel(displayedLevel)
            ?? throw new InvalidOperationException("The selected production level is not implemented.");
        TempestGame game = new(startingConfiguration: configuration with { }, random: random,
            level7SpikerSpeedOverride: 2.8125f);

        Spiker spiker = CreateSelectedSpiker(game);
        float expectedSpeed = (displayedLevel - 1) / 48f + 2.5f + 1f;

        Assert.False(game.CurrentLevel.Configuration.UsesLevel7RomWavePolicy);
        Assert.Equal(expectedSpeed, spiker.Speed);
        Assert.Equal(1, random.DoubleCalls);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void LevelsWithoutSpikersDoNotEnableTheLevel7SpeedOverride(int displayedLevel)
    {
        TempestGame game = new(startingProductionLevel: displayedLevel,
            level7SpikerSpeedOverride: 2.8125f);

        Assert.False(game.CurrentLevel.Configuration.UsesLevel7RomWavePolicy);
        Assert.Empty(game.Spikers);
    }

    private static Spiker CreateSelectedSpiker(TempestGame game)
    {
        Egg egg = game.Eggs[0];
        Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger
            ?? throw new InvalidOperationException("The selected production level has no mixed-wave ledger.");
        ledger.CommitOriginalAsSpiker(egg.OriginalSlotId);

        MethodInfo factory = typeof(TempestGame).GetMethod(
            "CreateLevel3MixedEnemy", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("The mixed-wave enemy factory was not found.");
        return Assert.IsType<Spiker>(factory.Invoke(game, [egg]));
    }

    private static Measurement[] Measure(float spikerSpeed)
    {
        Level level = new(LevelConfiguration.Level7Production);
        Spiker spiker = new(edgeIndex: 1, speed: spikerSpeed);

        // Production gives a newly hatched Spiker its initialization update before its first climb.
        spiker.Update(level, static () => false, allowGrowthRetreatShot: false);
        spiker.Update(level, static () => false, allowGrowthRetreatShot: false);

        float initialSpikerDepth = spiker.Height;
        EnemyBullet projectile = new(spiker.EdgeIndex, initialSpikerDepth);
        List<Measurement> measurements =
        [
            new(0, initialSpikerDepth, projectile.Height, 0, 0, 0, 0, null)
        ];

        foreach (int activeStep in new[] { 1, 10, 30 })
        {
            int previousStep = measurements[^1].ActiveStep;
            for (int step = previousStep; step < activeStep; step++)
            {
                spiker.Update(level, static () => false, allowGrowthRetreatShot: false);
                projectile.Advance();
            }

            float spikerDistance = spiker.Height - initialSpikerDepth;
            float projectileDistance = projectile.Height - initialSpikerDepth;
            float separation = projectile.Height - spiker.Height;
            measurements.Add(new(
                activeStep,
                spiker.Height,
                projectile.Height,
                spikerDistance,
                projectileDistance,
                separation,
                separation,
                spikerDistance == 0 ? null : projectileDistance / (double)spikerDistance));
        }

        return measurements.ToArray();
    }

    private readonly record struct SpeedCase(
        float Speed,
        float SpikerDistanceAt30,
        float SeparationChangeAt30,
        double TravelRatio);

    private readonly record struct Measurement(
        int ActiveStep,
        float SpikerDepth,
        float ProjectileDepth,
        float SpikerDistance,
        float ProjectileDistance,
        float Separation,
        float SeparationChange,
        double? TravelRatio);

    private sealed class CountingRandom(double value) : Random
    {
        public int DoubleCalls { get; private set; }

        public override double NextDouble()
        {
            DoubleCalls++;
            return value;
        }
    }
}
