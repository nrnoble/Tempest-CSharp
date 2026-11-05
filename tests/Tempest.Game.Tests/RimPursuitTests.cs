using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class RimPursuitTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public void FlipperKeepsItsRimDirectionWhenThePlayerMovesBehind(int direction)
    {
        Level level = Square();
        const int source = 4;
        int firstDestination = AdjacentEdge(level, source, direction);
        int playerBehind = source;
        Flipper flipper = BeginRimPivot(level, source, direction);

        CompletePivot(flipper, level, playerBehind);
        Assert.Equal(firstDestination, flipper.EdgeIndex);
        Assert.NotEqual(direction, level.FindShortestDirection(firstDestination, playerBehind));

        StartNextPivot(flipper, level, playerBehind);

        Assert.Equal(direction, flipper.Direction);
        Assert.Equal(AdjacentEdge(level, firstDestination, direction), flipper.DestinationEdge);
        Assert.Equal(level.CalculateDirectedLaneTransition(
            firstDestination, flipper.DestinationEdge, direction).SweepMagnitudeDegrees,
            flipper.TotalTurnAngle);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public void FlipperDoesNotOscillateWhenThePlayerCrossesSidesRepeatedly(int direction)
    {
        Level level = Square();
        Flipper flipper = BeginRimPivot(level, source: 4, direction);

        for (int cycle = 0; cycle < 5; cycle++)
        {
            int currentSource = flipper.EdgeIndex;
            int expectedDestination = AdjacentEdge(level, currentSource, direction);
            int crossover = 0;
            while (flipper.State == FlipperState.Flipping)
            {
                bool playerAhead = crossover % 2 == 0;
                int playerEdge = AdjacentEdge(level, flipper.EdgeIndex,
                    playerAhead ? direction : -direction);
                Assert.Equal(playerAhead ? direction : -direction,
                    level.FindShortestDirection(flipper.EdgeIndex, playerEdge));
                flipper.Update(level, playerEdge, UnexpectedRandom);
                Assert.Equal(direction, flipper.Direction);
                crossover++;
            }

            int currentEdge = flipper.EdgeIndex;
            Assert.Equal(expectedDestination, currentEdge);

            for (int tick = 0; tick < 24; tick++)
            {
                int shortestDirection = (tick + cycle) % 2 == 0 ? direction : -direction;
                int playerEdge = AdjacentEdge(level, currentEdge, shortestDirection);
                Assert.Equal(shortestDirection, level.FindShortestDirection(currentEdge, playerEdge));
                flipper.Update(level, playerEdge, UnexpectedRandom);
            }

            Assert.Equal(direction, flipper.Direction);
            Assert.Equal(AdjacentEdge(level, currentEdge, direction), flipper.DestinationEdge);
        }
    }

    [Fact]
    public void PulsarKeepsItsRimDirectionAcrossPlayerCrossover()
    {
        Level level = new(LevelConfiguration.Level1);
        Pulsar pulsar = new(edgeIndex: 4, height: Level.Depth);

        pulsar.Update(level, playerEdge: 3, AlwaysOne);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        Assert.Equal(1, pulsar.RimPursuitDirection);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            pulsar.Update(level, playerEdge: 4, AlwaysOne);
        while (pulsar.State == PulsarState.Flipping)
            pulsar.Update(level, playerEdge: 4, AlwaysOne);
        Assert.Equal(3, pulsar.EdgeIndex);
        Assert.NotEqual(pulsar.Direction, level.FindShortestDirection(pulsar.EdgeIndex, 4));

        for (int tick = 0; tick < Pulsar.RimPauseDuration; tick++)
            pulsar.Update(level, playerEdge: 4, AlwaysOne);
        pulsar.Update(level, playerEdge: 4, AlwaysOne);
        Assert.Equal(PulsarState.LanePause, pulsar.State);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            pulsar.Update(level, playerEdge: 4, AlwaysOne);

        Assert.Equal(1, pulsar.Direction);
        Assert.Equal(2, pulsar.EdgeIndex);
    }

    [Fact]
    public void PulsarClearsRimDirectionWhenItChoosesToDescend()
    {
        Level level = new(LevelConfiguration.Level1);
        Pulsar pulsar = new(edgeIndex: 4, height: Level.Depth);

        pulsar.Update(level, playerEdge: 3, AlwaysOne);
        for (int tick = 0; tick < Pulsar.LanePauseDuration; tick++)
            pulsar.Update(level, playerEdge: 3, AlwaysOne);
        while (pulsar.State == PulsarState.Flipping)
            pulsar.Update(level, playerEdge: 3, AlwaysOne);
        Assert.NotNull(pulsar.RimPursuitDirection);

        for (int tick = 0; tick < Pulsar.RimPauseDuration; tick++)
            pulsar.Update(level, playerEdge: 4, AlwaysOne);
        pulsar.Update(level, playerEdge: 4, AlwaysZero);

        Assert.Equal(PulsarState.RimPause, pulsar.State);
        Assert.Equal(PulsarState.Descending, pulsar.ResumeState);
        Assert.Null(pulsar.RimPursuitDirection);
    }

    [Fact]
    public void TankerCargoKeepsItsBelowRimRouteUntilRimPursuitBegins()
    {
        Level level = Square();
        TankerCargoSpawn cargo = new(SourceLane: 4, DestinationLane: 3, Direction: 1, Height: 385);
        Flipper flipper = Flipper.CreateTankerCargo(level, cargo, cargoUnitId: 7);

        CompletePivot(flipper, level, playerEdge: 10);
        Assert.Equal(1, flipper.Direction);
        Assert.Null(flipper.RimPursuitDirection);

        StartNextPivot(flipper, level, playerEdge: 4);

        Assert.Equal(-1, flipper.RimPursuitDirection);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(4, flipper.DestinationEdge);
    }

    [Theory]
    [InlineData(0.5, 10)]
    [InlineData(1, 20)]
    [InlineData(2, 40)]
    [InlineData(2.5, 50)]
    [InlineData(4, 80)]
    public void RimSpeedMultiplierChangesAngularTurnsButNotTheRimPause(
        double multiplier, double expectedTurnRate)
    {
        Level level = new(LevelConfiguration.Level1, rimTurnSpeedMultiplier: multiplier);
        FlipperConfiguration tuning = level.Configuration.Flipper;
        int playerEdge = level.PreviousEdge(4);
        Flipper flipper = new(edge: 4, height: Level.Depth);

        Assert.Equal(8, tuning.TurnSpeedDegrees);
        Assert.Equal(2, tuning.ClimbSpeed);
        Assert.Equal(expectedTurnRate, level.RimTurnSpeedDegrees);

        flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(24, flipper.PauseTicks);

        for (int tick = 0; tick < 23; tick++)
            flipper.Update(level, playerEdge, UnexpectedRandom);

        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(1, flipper.PauseTicks);
        flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(0, flipper.TurnAngle);
        float totalTurnAngle = flipper.TotalTurnAngle;

        flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal((float)expectedTurnRate, flipper.TurnAngle);
        int rimTurnUpdates = 1;
        while (flipper.State == FlipperState.Flipping)
        {
            flipper.Update(level, playerEdge, UnexpectedRandom);
            rimTurnUpdates++;
        }

        Assert.Equal((int)Math.Ceiling(totalTurnAngle / expectedTurnRate),
            rimTurnUpdates);
        Assert.Equal(playerEdge, flipper.EdgeIndex);
    }

    [Fact]
    public void Level1BelowRimCargoKeepsTheOriginalTurnAndClimbRates()
    {
        Level level = new(LevelConfiguration.Level1);
        Flipper flipper = Flipper.CreateTankerCargo(
            level,
            new TankerCargoSpawn(4, 3, 1, Level.Depth - 2),
            cargoUnitId: 7);

        flipper.Update(level, playerEdge: 8, UnexpectedRandom);

        Assert.Equal(8, flipper.TurnAngle);
        Assert.Equal(Level.Depth, flipper.Height);
    }

    [Fact]
    public void EveryLevelKeepsItsOwnConfiguredBaseRimTurnRate()
    {
        LevelConfiguration[] configurations =
        [
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Plus,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production,
            LevelConfiguration.Level7Production
        ];
        float[] expectedBaseRates = [8, 8, 8, 8, 8, 8, 9, 9];

        for (int index = 0; index < configurations.Length; index++)
        {
            LevelConfiguration configuration = configurations[index];
            Level level = new(configuration, rimTurnSpeedMultiplier: 2);
            Flipper flipper = BeginRimPivot(level, source: 4, direction: 1);
            flipper.Update(level, flipper.DestinationEdge, UnexpectedRandom);

            Assert.Equal(expectedBaseRates[index], configuration.Flipper.TurnSpeedDegrees);
            double expectedRimRate = expectedBaseRates[index] *
                Level.RimTurnSpeedCalibrationMultiplier * 2;
            Assert.Equal(expectedRimRate, level.RimTurnSpeedDegrees);
            Assert.Equal((float)expectedRimRate, flipper.TurnAngle);
        }
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(4)]
    public void BelowRimTankerMovementIsIndependentOfTheMultiplier(double multiplier)
    {
        LevelConfiguration[] configurations =
        [
            LevelConfiguration.Level1,
            LevelConfiguration.Level2Square,
            LevelConfiguration.Level3Mixed,
            LevelConfiguration.Level4Candidate,
            LevelConfiguration.Level5Production,
            LevelConfiguration.Level6Production
        ];

        foreach (LevelConfiguration configuration in configurations)
        {
            Level level = new(configuration, rimTurnSpeedMultiplier: multiplier);
            Flipper flipper = Flipper.CreateTankerCargo(
                level,
                new TankerCargoSpawn(4, 3, 1, Level.Depth - 20),
                cargoUnitId: 7);

            flipper.Update(level, playerEdge: 8, UnexpectedRandom);

            Assert.Equal(configuration.Flipper.TurnSpeedDegrees, flipper.TurnAngle);
            Assert.Equal(Level.Depth - 18, flipper.Height);
        }
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(4)]
    public void Level7ProgrammedBelowRimRouteIsIndependentOfTheMultiplier(double multiplier)
    {
        Level level = new(LevelConfiguration.Level7Production, rimTurnSpeedMultiplier: multiplier);
        Flipper flipper = Flipper.CreateTankerCargo(
            level,
            new TankerCargoSpawn(4, 3, 1, Level.Depth - 20),
            cargoUnitId: 7,
            useLevel7ProgrammedRoute: true);

        flipper.Update(level, playerEdge: 8, UnexpectedRandom);

        Assert.Equal(LevelConfiguration.Level7Production.Flipper.TurnSpeedDegrees,
            flipper.TurnAngle);
        Assert.Equal(Level.Depth - 18, flipper.Height);
    }

    [Fact]
    public void Level7ProgrammedRouteIsUnchangedThroughItsRimHandoff()
    {
        Level normalLevel = new(LevelConfiguration.Level7Production);
        Level multipliedLevel = new(LevelConfiguration.Level7Production,
            rimTurnSpeedMultiplier: 4);
        Flipper normal = Flipper.CreateTankerCargo(
            normalLevel,
            new TankerCargoSpawn(4, 3, 1, Level.Depth - 120),
            cargoUnitId: 7,
            useLevel7ProgrammedRoute: true);
        Flipper multiplied = Flipper.CreateTankerCargo(
            multipliedLevel,
            new TankerCargoSpawn(4, 3, 1, Level.Depth - 120),
            cargoUnitId: 7,
            useLevel7ProgrammedRoute: true);
        bool reachedRimHandoff = false;

        for (int tick = 0; tick < 100; tick++)
        {
            normal.Update(normalLevel, playerEdge: 8, UnexpectedRandom);
            multiplied.Update(multipliedLevel, playerEdge: 8, UnexpectedRandom);

            Assert.Equal(
                (normal.EdgeIndex, normal.Height, normal.State, normal.TurnAngle, normal.Direction),
                (multiplied.EdgeIndex, multiplied.Height, multiplied.State,
                    multiplied.TurnAngle, multiplied.Direction));
            if (normal.State == FlipperState.Pausing && normal.Height == Level.Depth)
            {
                reachedRimHandoff = true;
                break;
            }
        }

        Assert.True(reachedRimHandoff);
    }

    private static Level Square() => new(LevelConfiguration.Level2Square);

    private static Flipper BeginRimPivot(Level level, int source, int direction)
    {
        int playerEdge = AdjacentEdge(level, source, direction);
        Flipper flipper = new(source, Level.Depth);
        flipper.Update(level, playerEdge, UnexpectedRandom);
        for (int tick = 0; tick < level.Configuration.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge, UnexpectedRandom);

        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(direction, flipper.Direction);
        Assert.Equal(AdjacentEdge(level, source, direction), flipper.DestinationEdge);
        return flipper;
    }

    private static void CompletePivot(Flipper flipper, Level level, int playerEdge)
    {
        for (int tick = 0; tick < 1000 && flipper.State == FlipperState.Flipping; tick++)
            flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    private static void StartNextPivot(Flipper flipper, Level level, int playerEdge)
    {
        for (int tick = 0; tick < level.Configuration.Flipper.RimPauseTicks; tick++)
            flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal(FlipperState.Flipping, flipper.State);
    }

    private static int AdjacentEdge(Level level, int source, int direction) =>
        direction > 0 ? level.PreviousEdge(source) : level.NextEdge(source);

    private static int UnexpectedRandom(int minimum, int maximum) =>
        throw new InvalidOperationException($"Unexpected random call: [{minimum}, {maximum}].");

    private static int AlwaysOne(int minimum, int maximum) => maximum;

    private static int AlwaysZero(int minimum, int maximum) => minimum;
}
