using System.Numerics;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

// Projection is shared mutable state; Level 2 geometry checks must not race exit-zoom tests.
[CollectionDefinition("Level 2 tests", DisableParallelization = true)]
public sealed class Level2TestCollection { }

// DEVELOPMENT ONLY: deterministic Incitti contracts, not Atari fidelity measurements.
[Collection("Level 2 tests")]
public sealed class Level2CanFlipTests
{
    [Fact]
    public void InstalledFlipperConfigurationNotDisplayedTextControlsCanflip()
    {
        Level level1 = new(LevelConfiguration.Level1 with { DisplayedLevel = 2 });
        Assert.Equal(new FlipperConfiguration(false, 2, 8, 21, 24), level1.Configuration.Flipper);
        Flipper straight = new(4);
        for (int tick = 0; tick < 199; tick++)
            straight.Update(level1, tick % 16, UnexpectedRandom);
        Assert.Equal(398, straight.Height);
        Assert.Equal(4, straight.EdgeIndex);
        Assert.Equal(FlipperState.Climbing, straight.State);
        straight.Update(level1, 7, UnexpectedRandom);
        Assert.Equal(24, straight.PauseTicks);

        Level square = new(LevelConfiguration.Level2SquareTest with { DisplayedLevel = 1 });
        Flipper canflip = new(4);
        canflip.Update(square, 7, UnexpectedRandom);
        Assert.Equal(FlipperState.Pausing, canflip.State);
        Assert.Equal(0, canflip.PauseTicks);
    }

    [Fact]
    public void FirstClimbEntersZeroPauseThenStartsAdjacentTurnOnTheNextUpdate()
    {
        Level level = Square();
        Flipper flipper = new(4);
        Assert.Equal(FlipperState.Climbing, flipper.State);
        flipper.Update(level, 7, UnexpectedRandom);
        Assert.Equal(2, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(0, flipper.PauseTicks);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(4, flipper.SourceEdge);
        Assert.Equal(4, flipper.DestinationEdge);
        flipper.Update(level, 1, UnexpectedRandom);
        Assert.Equal(3, flipper.Height);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(4, flipper.EdgeIndex);
        Assert.Equal(4, flipper.SourceEdge);
        Assert.Equal(5, flipper.DestinationEdge);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(0, flipper.TurnAngle);
        Assert.Equal(180, flipper.TotalTurnAngle);
    }

    [Theory]
    [InlineData(0, 1, 3)]
    [InlineData(1, -1, 5)]
    public void InitialSameLaneTieUsesOneInjectedRollAfterTheShootAttempt(
        int roll, int direction, int destination)
    {
        Level level = Square();
        Flipper flipper = new(4, 0, bulletAmmunition: 1);
        int calls = 0;
        flipper.Update(level, 4, (minimum, maximum) =>
        {
            Assert.Equal((0, 1), (minimum, maximum));
            Assert.Equal(15, flipper.BulletTimer);
            Assert.Equal(FlipperState.Pausing, flipper.State);
            calls++;
            return roll;
        });
        Assert.Equal(1, calls);
        Assert.Equal(direction, flipper.Direction);
        flipper.Update(level, 4, UnexpectedRandom);
        Assert.Equal(destination, flipper.DestinationEdge);
        Assert.Equal(14, flipper.BulletTimer);
    }

    [Fact]
    public void ReferenceDirectionMatchesAllClosedSixteenLanePairsIncludingSignedAntipodes()
    {
        Level level = Square();
        for (int from = 0; from < 16; from++)
        {
            for (int to = 0; to < 16; to++)
            {
                if (from == to)
                    continue;
                int difference = from - to;
                int expected = Math.Abs(difference) > 8 ? -Math.Sign(difference) : Math.Sign(difference);
                Flipper flipper = new(from);
                flipper.Update(level, to, UnexpectedRandom);
                Assert.Equal(expected, flipper.Direction);
                flipper.Update(level, (to + 8) % 16, UnexpectedRandom);
                Assert.Equal((from - expected + 16) % 16, flipper.DestinationEdge);
            }
        }
        Assert.Equal(-1, level.FindReferenceShortestDirection(0, 8, UnexpectedRandom));
        Assert.Equal(1, level.FindReferenceShortestDirection(8, 0, UnexpectedRandom));
        Assert.Equal(1, level.FindShortestDirection(0, 8));
        Assert.Equal(1, level.FindShortestDirection(0, 0));
    }

    [Fact]
    public void SameLaneReferenceDecisionWithoutRandomSeamFailsExplicitly()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Square().FindReferenceShortestDirection(4, 4, randomInclusive: null));
    }

    [Fact]
    public void BelowRimDirectionAndDestinationsStayLatchedAcrossPlayerMovementAndLaterPauses()
    {
        Level level = Square();
        Flipper flipper = new(4);
        flipper.Update(level, 7, UnexpectedRandom);
        int expectedSource = 4;
        for (int cycle = 0; cycle < 3; cycle++)
        {
            while (flipper.State == FlipperState.Pausing)
                flipper.Update(level, flipper.EdgeIndex, UnexpectedRandom);
            int destination = level.NextEdge(expectedSource);
            Assert.Equal(expectedSource, flipper.SourceEdge);
            Assert.Equal(destination, flipper.DestinationEdge);
            int tick = 0;
            while (flipper.State == FlipperState.Flipping)
            {
                Assert.Equal(expectedSource, flipper.SourceEdge);
                Assert.Equal(destination, flipper.DestinationEdge);
                Assert.Equal(-1, flipper.Direction);
                flipper.Update(level, tick++ % 16, UnexpectedRandom);
                Assert.False(flipper.CompletedIntoPlayer);
            }
            Assert.Equal(destination, flipper.EdgeIndex);
            Assert.Equal(21, flipper.PauseTicks);
            Assert.True(flipper.Height < Level.Depth);
            expectedSource = destination;
        }
    }

    [Fact]
    public void EverySquareMidTubeTransitionUsesB1HingeTimingAndProjectedEndpoints()
    {
        TempestGame game = new(level2Test: true);
        Level level = game.CurrentLevel;
        VectorRenderer renderer = new(game);
        for (int source = 0; source < 16; source++)
        {
            foreach (int direction in new[] { -1, 1 })
            {
                int destination = (source - direction + 16) % 16;
                Flipper flipper = new(source);
                flipper.Update(level, destination, UnexpectedRandom);
                flipper.Update(level, destination, UnexpectedRandom);
                float angle = (direction == -1 ? (source + 1) % 4 == 0 : source % 4 == 0) ? 90 : 180;
                int ticks = angle == 90 ? 12 : 23;
                Assert.Equal(angle, flipper.TotalTurnAngle);
                for (int tick = 0; tick < ticks; tick++)
                {
                    Assert.Equal(FlipperState.Flipping, flipper.State);
                    float progress = tick * 8 / angle;
                    (Vector2 start, Vector2 end) =
                        level.CalculateSquareFlipperPivotEndpoints(source, destination, progress);
                    float depth = Level.Position + Level.Depth - flipper.Height;
                    (Vector2 actualStart, Vector2 actualEnd) = renderer.ProjectFlipperEndpoints(flipper);
                    Assert.Equal(Project(level, start, depth), actualStart);
                    Assert.Equal(Project(level, end, depth), actualEnd);
                    AssertFinite(actualStart);
                    AssertFinite(actualEnd);
                    flipper.Update(level, destination, UnexpectedRandom);
                    Assert.Equal(3 + 2 * (tick + 1), flipper.Height);
                    Assert.False(flipper.CompletedIntoPlayer);
                }
                Assert.Equal(FlipperState.Pausing, flipper.State);
                Assert.Equal(destination, flipper.EdgeIndex);
                Assert.Equal(destination, flipper.SourceEdge);
                Assert.Equal(destination, flipper.DestinationEdge);
                Assert.Equal(0, flipper.TurnAngle);
                Assert.Equal(0, flipper.TotalTurnAngle);
                Assert.Equal(21, flipper.PauseTicks);
                (Vector2 finalStart, Vector2 finalEnd) = renderer.ProjectFlipperEndpoints(flipper);
                float finalDepth = Level.Position + Level.Depth - flipper.Height;
                Assert.Equal(Project(level, level.Definition.Points[destination], finalDepth), finalStart);
                Assert.Equal(Project(level, level.Definition.Points[level.NextEdge(destination)], finalDepth), finalEnd);

                float height = flipper.Height;
                for (int tick = 1; tick <= 21; tick++)
                {
                    flipper.Update(level, source, UnexpectedRandom);
                    Assert.Equal(height + tick, flipper.Height);
                    Assert.Equal(tick == 21 ? FlipperState.Flipping : FlipperState.Pausing, flipper.State);
                    Assert.Equal(direction, flipper.Direction);
                }
            }
        }
    }

    [Theory]
    [InlineData(395f)]
    [InlineData(398f)]
    [InlineData(399f)]
    [InlineData(399.5f)]
    public void ClimbAndTurnClampAtRimAndOnlyCompletedPivotIsLethal(float height)
    {
        Level level = Square();
        Flipper flipper = new(1, height);
        flipper.Update(level, 0, UnexpectedRandom);
        Assert.Equal(MathF.Min(400, height + 2), flipper.Height);
        Assert.False(flipper.CompletedIntoPlayer);
        flipper.Update(level, 0, UnexpectedRandom);
        Assert.Equal(MathF.Min(400, height + 3), flipper.Height);
        Assert.Equal(0, flipper.DestinationEdge);
        for (int tick = 0; tick < 23; tick++)
        {
            Assert.False(flipper.CompletedIntoPlayer);
            flipper.Update(level, 0, UnexpectedRandom);
            Assert.InRange(flipper.Height, 0, 400);
        }
        Assert.Equal(400, flipper.Height);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(24, flipper.PauseTicks);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    [Fact]
    public void LanePauseReachingRimKeepsItsDurationAndResamplesOnlyAtExpiry()
    {
        Level level = Square();
        Flipper flipper = new(1, 335);
        flipper.Update(level, 0, UnexpectedRandom);
        flipper.Update(level, 0, UnexpectedRandom);
        FinishPivot(flipper, level, playerEdge: 8);
        Assert.Equal(384, flipper.Height);
        Assert.Equal(21, flipper.PauseTicks);
        for (int tick = 1; tick < 21; tick++)
        {
            flipper.Update(level, 2, UnexpectedRandom);
            Assert.Equal(MathF.Min(400, 384 + tick), flipper.Height);
            Assert.Equal(1, flipper.Direction);
            Assert.False(flipper.CompletedIntoPlayer);
        }
        flipper.Update(level, 2, UnexpectedRandom);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(1, flipper.DestinationEdge);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(400, flipper.Height);
    }

    [Theory]
    [InlineData(335f)]
    [InlineData(335.5f)]
    public void ArrivalDuringLanePauseSignalsRimContactWithoutAnyBelowRimDeath(float initialHeight)
    {
        Level level = Square();
        Flipper flipper = new(1, initialHeight);
        flipper.Update(level, 0, UnexpectedRandom);
        flipper.Update(level, 0, UnexpectedRandom);
        FinishPivot(flipper, level, 0);
        for (int tick = 0; tick < 15; tick++)
        {
            flipper.Update(level, 0, UnexpectedRandom);
            Assert.False(flipper.CompletedIntoPlayer);
        }
        Assert.Equal(initialHeight + 64, flipper.Height);
        flipper.Update(level, 0, UnexpectedRandom);
        Assert.Equal(400, flipper.Height);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(5, flipper.PauseTicks);
    }

    [Fact]
    public void Level2DirectClimbArrivalStillSignalsCollisionAndKeepsNonPivotGeometry()
    {
        Level level = Square();
        Flipper flipper = new(7, 399);
        flipper.Update(level, 7, (_, _) => 0);
        Assert.Equal(400, flipper.Height);
        Assert.True(flipper.CompletedIntoPlayer);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(flipper.EdgeIndex, flipper.SourceEdge);
        Assert.Equal(flipper.EdgeIndex, flipper.DestinationEdge);
        Assert.Equal(0, flipper.TotalTurnAngle);
    }

    [Theory]
    [InlineData(0, 1, 15)]
    [InlineData(1, -1, 1)]
    public void RimSameLaneTieIsSampledAtExpiryAndNeverRetargetsTheActivePivot(
        int roll, int direction, int destination)
    {
        Level level = Square();
        Flipper flipper = new(0, 400);
        flipper.Update(level, 8, UnexpectedRandom);
        for (int tick = 0; tick < 23; tick++)
            flipper.Update(level, 0, UnexpectedRandom);
        int calls = 0;
        flipper.Update(level, 0, (minimum, maximum) =>
        {
            Assert.Equal((0, 1), (minimum, maximum));
            calls++;
            return roll;
        });
        Assert.Equal(1, calls);
        Assert.Equal(direction, flipper.Direction);
        Assert.Equal(destination, flipper.DestinationEdge);
        int ticks = (int)MathF.Ceiling(flipper.TotalTurnAngle / 8);
        for (int tick = 0; tick < ticks; tick++)
        {
            flipper.Update(level, 8, UnexpectedRandom);
            Assert.Equal(direction, flipper.Direction);
            Assert.Equal(destination, flipper.DestinationEdge);
            Assert.Equal(400, flipper.Height);
        }
        Assert.False(flipper.CompletedIntoPlayer);
        Assert.Equal(24, flipper.PauseTicks);
        for (int tick = 0; tick < 24; tick++)
            flipper.Update(level, 0, UnexpectedRandom);
        Assert.Equal(-direction, flipper.Direction);
        Assert.Equal(0, flipper.DestinationEdge);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(15, true)]
    [InlineData(16, false)]
    public void MidTubeShotsKeepExistingTwoLanePolicyAndStrictHeightBand(float separation, bool hit)
    {
        Level level = Square();
        Flipper flipper = new(3, 100);
        flipper.Update(level, 4, UnexpectedRandom);
        flipper.Update(level, 4, UnexpectedRandom);
        for (int tick = 0; tick < 12; tick++)
        {
            Assert.Equal(FlipperState.Flipping, flipper.State);
            Assert.Equal(hit, flipper.IsHitBy(new Shot { EdgeIndex = 3, Height = flipper.Height - separation }));
            Assert.Equal(hit, flipper.IsHitBy(new Shot { EdgeIndex = 4, Height = flipper.Height - separation }));
            Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 5, Height = flipper.Height - separation }));
            flipper.Update(level, 4, UnexpectedRandom);
        }
        Assert.False(flipper.IsHitBy(new Shot { EdgeIndex = 3, Height = flipper.Height - 1 }));
        Assert.True(flipper.IsHitBy(new Shot { EdgeIndex = 4, Height = flipper.Height - 1 }));
    }

    [Theory]
    [InlineData(75, false)]
    [InlineData(76, true)]
    public void BulletTimerCountsOnlyClimbAndPauseExpiryAttempts(int roll, bool fires)
    {
        Level level = Square();
        Flipper flipper = new(4, 0, bulletAmmunition: 1);
        int attempts = 0;
        int randomCalls = 0;
        EnemyBullet? bullet = null;
        for (int tick = 0; tick < 3000 && attempts < 17; tick++)
        {
            bool eligible = flipper.State == FlipperState.Climbing ||
                flipper.State == FlipperState.Pausing && flipper.PauseTicks <= 1;
            int timer = flipper.BulletTimer;
            bullet = flipper.Update(level, (flipper.EdgeIndex + 8) % 16, (minimum, maximum) =>
            {
                Assert.Equal((0, 100), (minimum, maximum));
                randomCalls++;
                return roll;
            });
            if (eligible)
            {
                attempts++;
                Assert.Equal(attempts == 17 ? 16 : 16 - attempts, flipper.BulletTimer);
            }
            else
                Assert.Equal(timer, flipper.BulletTimer);
            if (attempts < 17)
                Assert.Null(bullet);
        }
        Assert.Equal(17, attempts);
        Assert.Equal(1, randomCalls);
        Assert.Equal(fires ? 0 : 1, flipper.BulletAmmunition);
        if (fires)
        {
            Assert.NotNull(bullet);
            Assert.Equal(flipper.DestinationEdge, bullet.EdgeIndex);
            Assert.Equal(flipper.Height, bullet.Height);
        }
        else
            Assert.Null(bullet);
    }

    [Fact]
    public void RimPauseExpiryDrawsDirectionBeforeTheExpiredFiringRoll()
    {
        Level level = Square();
        Flipper flipper = new(4, 0, bulletAmmunition: 1);
        for (int tick = 0; tick < 3000 &&
            !(flipper.Height == 400 && flipper.State == FlipperState.Pausing &&
              flipper.PauseTicks == 1 && flipper.BulletTimer == 0); tick++)
            flipper.Update(level, (flipper.EdgeIndex + 8) % 16, UnexpectedRandom);
        Assert.Equal(0, flipper.BulletTimer);
        Assert.Equal(1, flipper.PauseTicks);
        List<(int, int)> calls = [];
        EnemyBullet? bullet = flipper.Update(level, flipper.EdgeIndex, (minimum, maximum) =>
        {
            calls.Add((minimum, maximum));
            return maximum == 1 ? 1 : 76;
        });
        Assert.Equal(new[] { (0, 1), (0, 100) }, calls);
        Assert.Equal(-1, flipper.Direction);
        Assert.NotNull(bullet);
        Assert.Equal(flipper.DestinationEdge, bullet.EdgeIndex);
        Assert.Equal(400, bullet.Height);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void HatchUsesExistingGameRandomWithOptionalAmmoBeforeInitialDirection(bool bullets)
    {
        RecordingRandom random = new();
        TempestGame game = new(level2Test: true, enemyBulletsTest: bullets, random: random);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.Player.ResetPosition(egg.EdgeIndex);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(bullets ? new[] { (1, 4), (0, 2) } : new[] { (0, 2) }, random.Calls);
        Assert.Equal(bullets ? 1 : 0, flipper.BulletAmmunition);
        Assert.Equal(bullets ? 15 : 16, flipper.BulletTimer);
        Assert.Equal(2, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(0, flipper.PauseTicks);
        game.Player.ResetPosition((egg.EdgeIndex + 8) % 16);
        game.RunOneStep();
        Assert.Equal((egg.EdgeIndex + 15) % 16, flipper.DestinationEdge);
        Assert.Equal(bullets ? 2 : 1, random.Calls.Count);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(0, game.DeathTicks);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SimulationMovementDoesNotRedirectActiveOrSubsequentBelowRimTransitions(bool bullets)
    {
        RecordingRandom random = new();
        TempestGame game = new(level2Test: true, enemyBulletsTest: bullets, random: random);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);
        game.Player.ResetPosition(game.CurrentLevel.NextEdge(egg.EdgeIndex));
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(-1, flipper.Direction);
        game.RunOneStep();
        int source = flipper.SourceEdge;
        int destination = flipper.DestinationEdge;
        int calls = random.Calls.Count;
        for (int tick = 0; tick < 23; tick++)
        {
            game.RunOneStep(new PlayerInput(false, tick % 2 == 0 ? 160 : -160, false, false));
            Assert.Equal(destination, flipper.DestinationEdge);
            Assert.Equal(-1, flipper.Direction);
            Assert.Equal(0, game.DeathTicks);
            if (flipper.State == FlipperState.Flipping)
                Assert.Equal(source, flipper.SourceEdge);
        }
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(21, flipper.PauseTicks);
        game.Player.ResetPosition(game.CurrentLevel.PreviousEdge(destination));
        for (int tick = 0; tick < 21; tick++)
            game.RunOneStep();
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Equal(game.CurrentLevel.NextEdge(destination), flipper.DestinationEdge);
        Assert.Equal(-1, flipper.Direction);
        Assert.Equal(calls, random.Calls.Count);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.True(flipper.Height < 400);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BothZapperChargesRemoveAnActiveMidTubePivotWithoutStaleGeometry(bool bullets)
    {
        TempestGame game = new(level2Test: true, enemyBulletsTest: bullets,
            random: new RecordingRandom());
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        VectorRenderer renderer = new(game);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(1, game.SuperZapperChargesRemaining);
        Assert.Equal(3, flipper.PendingZapTicks);
        game.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(0, game.SuperZapperChargesRemaining);
        Assert.Equal(3, flipper.PendingZapTicks);
        for (int tick = 0; tick < 3; tick++)
        {
            AssertFinite(renderer.ProjectFlipperEndpoints(flipper).Start);
            AssertFinite(renderer.ProjectFlipperEndpoints(flipper).End);
            game.RunOneStep();
        }
        Assert.DoesNotContain(flipper, game.Flippers);
        Assert.Equal(1, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(0, game.CurrentLevel.EnemiesRimCleared);
        Assert.Equal(150, game.Score.Score);
        Assert.Equal(0, game.DeathTicks);
        Assert.Equal(LevelState.Playing, game.State);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void LastSafePivotTickShotWinsOrDeathKeepsCanonicalRenderableState(bool bullets, bool shoot)
    {
        RecordingRandom random = new();
        TempestGame game = new(level2Test: true, enemyBulletsTest: bullets, random: random);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        PrepareLastRimPivotTick(game, flipper);
        int callsBefore = random.Calls.Count;
        game.RunOneStep(new PlayerInput(shoot, 0, false, false));
        Assert.Equal(shoot ? 150 : 0, game.Score.Score);
        Assert.Equal(shoot ? 1 : 0, game.CurrentLevel.EnemiesDestroyed);
        Assert.Equal(shoot ? LevelState.Playing : LevelState.PlayerDying, game.State);
        Assert.Equal(shoot ? 0 : 60, game.DeathTicks);
        Assert.Equal(callsBefore, random.Calls.Count);
        if (shoot)
        {
            Assert.DoesNotContain(flipper, game.Flippers);
            return;
        }
        Assert.Equal(FlipperState.Pausing, flipper.State);
        Assert.Equal(flipper.EdgeIndex, flipper.SourceEdge);
        Assert.Equal(flipper.EdgeIndex, flipper.DestinationEdge);
        Assert.Equal(0, flipper.TotalTurnAngle);
        VectorRenderer renderer = new(game);
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).Start);
        AssertFinite(renderer.ProjectFlipperEndpoints(flipper).End);
        float height = flipper.Height;
        int pause = flipper.PauseTicks;
        int timer = flipper.BulletTimer;
        for (int tick = 0; tick < 59; tick++)
            game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Equal(height, flipper.Height);
        Assert.Equal(pause, flipper.PauseTicks);
        Assert.Equal(timer, flipper.BulletTimer);
        Assert.Equal(callsBefore, random.Calls.Count);
        game.RunOneStep();
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Equal(2, game.Player.Lives);
        Assert.Same(LevelConfiguration.Level2SquareTest, game.CurrentLevel.Configuration);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper fresh = Assert.Single(game.Flippers);
        Assert.NotSame(flipper, fresh);
        Assert.Equal(2, fresh.Height);
        Assert.Equal(FlipperState.Pausing, fresh.State);
        Assert.Equal(0, fresh.PauseTicks);
        Assert.Equal(bullets ? 15 : 16, fresh.BulletTimer);
    }

    private static Level Square() => new(LevelConfiguration.Level2SquareTest);

    private static int UnexpectedRandom(int minimum, int maximum) =>
        throw new InvalidOperationException($"Unexpected random range [{minimum}, {maximum}].");

    private static Vector2 Project(Level level, Vector2 point, float depth) =>
        Projection.Project(point.X, point.Y, depth, level.Definition.CenterY, level.Definition.YOffset);

    private static void AssertFinite(Vector2 point) =>
        Assert.True(float.IsFinite(point.X) && float.IsFinite(point.Y));

    private static void FinishPivot(Flipper flipper, Level level, int playerEdge)
    {
        int ticks = (int)MathF.Ceiling(flipper.TotalTurnAngle / 8);
        for (int tick = 0; tick < ticks; tick++)
            flipper.Update(level, playerEdge, UnexpectedRandom);
        Assert.Equal(FlipperState.Pausing, flipper.State);
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void PrepareLastRimPivotTick(TempestGame game, Flipper flipper)
    {
        for (int tick = 0; tick < 1000 &&
            (flipper.Height < 400 || flipper.State == FlipperState.Flipping); tick++)
            flipper.Update(game.CurrentLevel, (flipper.DestinationEdge + 8) % 16);
        Assert.Equal(400, flipper.Height);
        Assert.Equal(FlipperState.Pausing, flipper.State);
        int destination = game.CurrentLevel.NextEdge(flipper.EdgeIndex);
        game.Player.ResetPosition(destination);
        int pause = flipper.PauseTicks;
        for (int tick = 0; tick < pause; tick++)
            flipper.Update(game.CurrentLevel, destination);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        int ticks = (int)MathF.Ceiling(flipper.TotalTurnAngle / 8);
        for (int tick = 0; tick < ticks - 1; tick++)
            flipper.Update(game.CurrentLevel, destination);
        Assert.False(flipper.CompletedIntoPlayer);
    }

    private sealed class RecordingRandom : Random
    {
        public List<(int, int)> Calls { get; } = [];

        public override int Next(int minValue, int maxValue)
        {
            Assert.Contains((minValue, maxValue), new[] { (1, 4), (0, 2), (0, 101) });
            Calls.Add((minValue, maxValue));
            return minValue;
        }
    }
}
