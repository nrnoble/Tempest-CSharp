using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class Level2FourFlipperCapTests
{
    private const int ActiveFlipperLimit = 4;
    private const int OriginalSupply = 12;
    private const int MaximumComparisonTicks = 1_000;

    [Fact]
    public void ProductionStartupUsesCanonicalLevelTwoAcrossDirectAndRecordedPaths()
    {
        Assert.True(DebugController.TryParseStartingLevel(["--level", "2"],
            out int? productionLevel, out string? error));
        Assert.Null(error);
        Assert.Equal(2, productionLevel);
        TempestGame direct = new(startingProductionLevel: productionLevel,
            startingLives: 10, rimSpeedMultiplier: 2.5);
        GameplayStartupConfiguration playbackStartup = new(2, 10, 2.5, 1981);
        TempestGame recorded = new(playbackStartup);
        AssertProductionLevelTwoStartup(direct);
        AssertProductionLevelTwoStartup(recorded);
        Assert.True(direct.NaturalProgressionEnabled);
        Assert.True(recorded.NaturalProgressionEnabled);
        Assert.Equal(2.5, direct.CurrentLevel.RimTurnSpeedMultiplier);
        Assert.Equal(2.5, recorded.CurrentLevel.RimTurnSpeedMultiplier);

        TempestGame campaign = CreateNaturalLevelTwo();
        AssertProductionLevelTwoStartup(campaign);
        Assert.True(campaign.NaturalProgressionEnabled);
        Egg campaignWaitingEgg = RunToWaitingEgg(campaign, MaximumComparisonTicks);
        Assert.Equal(ActiveFlipperLimit, campaign.Flippers.Count);
        Assert.Contains(campaignWaitingEgg, campaign.Eggs);
    }

    [Fact]
    public void ProductionLevelTwoNeverExceedsFourAndReadyEggWaitsUntilCapacityOpens()
    {
        TempestGame production = new(startingProductionLevel: 2, startingLives: 1000,
            random: new Random(1981));
        TempestGame replay = new(startingProductionLevel: 2, startingLives: 1000,
            random: new Random(1981));
        bool sawWaitingEgg = false;

        for (int tick = 0; tick < MaximumComparisonTicks; tick++)
        {
            production.RunOneStep();
            replay.RunOneStep();
            AssertProductionLevelTwoConservation(production, tick);
            AssertProductionLevelTwoConservation(replay, tick);
            Assert.Equal(GetFlipperSnapshots(production), GetFlipperSnapshots(replay));
            Assert.Equal(GetEggSnapshots(production), GetEggSnapshots(replay));
            Assert.Equal(GetBulletSnapshots(production), GetBulletSnapshots(replay));
            Assert.Equal(production.CurrentLevel.ReserveEggs, replay.CurrentLevel.ReserveEggs);

            if (production.Flippers.Count == ActiveFlipperLimit &&
                production.Eggs.Any(egg => egg.State == EggState.ReadyToHatch))
            {
                sawWaitingEgg = true;
                break;
            }
        }

        Assert.True(sawWaitingEgg,
            $"A ready Egg did not wait at the production cap within {MaximumComparisonTicks} ticks. " +
            StateSnapshot(production, MaximumComparisonTicks));
        Assert.Equal(ActiveFlipperLimit, production.Flippers.Count);
        Assert.Contains(production.Eggs, egg => egg.State == EggState.ReadyToHatch);
    }

    [Fact]
    public void ReadyEggWaitsWithoutResolvingAndHatchesWhenZapperRemovalFreesCapacity()
    {
        TempestGame first = CreateProductionLevelTwo(random: new Random(1981), startingLives: 1000);
        TempestGame replay = CreateProductionLevelTwo(random: new Random(1981), startingLives: 1000);
        Egg waitingEgg = RunToWaitingEgg(first, MaximumComparisonTicks);
        Egg replayWaitingEgg = RunToWaitingEgg(replay, MaximumComparisonTicks);
        Assert.Equal(GetEggSnapshot(waitingEgg), GetEggSnapshot(replayWaitingEgg));
        Assert.Equal(GetFlipperSnapshots(first), GetFlipperSnapshots(replay));

        var blockedSnapshot = GetEggSnapshot(waitingEgg);
        int unresolvedBeforeWait = first.CurrentLevel.RemainingEnemies;
        for (int tick = 0; tick < 3; tick++)
        {
            first.RunOneStep();
            replay.RunOneStep();
            AssertProductionLevelTwoConservation(first, tick);
            AssertProductionLevelTwoConservation(replay, tick);
            Assert.Contains(waitingEgg, first.Eggs);
            Assert.Equal(EggState.ReadyToHatch, waitingEgg.State);
            Assert.Equal(0, waitingEgg.Height);
            Assert.Equal(blockedSnapshot, GetEggSnapshot(waitingEgg));
            Assert.Equal(unresolvedBeforeWait, first.CurrentLevel.RemainingEnemies);
            Assert.Equal(GetEggSnapshots(first), GetEggSnapshots(replay));
            Assert.Equal(GetFlipperSnapshots(first), GetFlipperSnapshots(replay));
            Assert.Equal(GetBulletSnapshots(first), GetBulletSnapshots(replay));
            Assert.Equal(LevelState.Playing, first.State);
        }

        Flipper[] zapTargets = first.Flippers.ToArray();
        first.RunOneStep(new PlayerInput(false, 0, false, false, ZapPressed: true));
        Assert.Equal(ActiveFlipperLimit, first.Flippers.Count);
        Assert.Contains(waitingEgg, first.Eggs);
        Assert.All(zapTargets, target => Assert.Contains(target, first.Flippers));

        bool resumed = false;
        for (int tick = 0; tick < 120; tick++)
        {
            first.RunOneStep();
            AssertProductionLevelTwoConservation(first, tick);
            Assert.True(first.Flippers.Count <= ActiveFlipperLimit,
                StateSnapshot(first, tick));
            Assert.All(zapTargets.Where(target => target.PendingZapTicks > 0),
                target => Assert.Contains(target, first.Flippers));
            if (!first.Eggs.Contains(waitingEgg))
            {
                resumed = true;
                break;
            }
        }

        Assert.True(resumed,
            "The waiting Egg should hatch through the normal path when a Zapper target is removed. " +
            StateSnapshot(first, 120));
        Assert.Equal(ActiveFlipperLimit, first.Flippers.Count);
        Assert.DoesNotContain(waitingEgg, first.Eggs);
        AssertProductionLevelTwoConservation(first, 120);
    }

    [Fact]
    public void ProductionFiringAndBulletRenderingNeedNoOptInBulletScenario()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = CreateProductionLevelTwo(random: new ArcadeFiringRandom(),
            startingLives: 1000, audioEvents: audio);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Single(game.Flippers);
        Assert.False(game.EnemyBulletsTest);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(77, game.CurrentLevel.Configuration.FlipperShotHoldoffTicks);
        Assert.Equal(1, audio.Count(AudioEvent.EnemyBullet));

        VectorRenderer renderer = new(game);
        Assert.True(renderer.ShouldDrawEnemyBullets);
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End, Raylib_cs.Color Color)> lines = [];
        renderer.DrawEnemyBullets((start, end, color) => lines.Add((start, end, color)));
        Assert.Equal(4, lines.Count);
        Assert.All(lines, line => Assert.Equal(Raylib_cs.Color.White, line.Color));
        Assert.Same(bullet, game.EnemyBullets[0]);

        int maximumBullets = game.EnemyBullets.Count;
        for (int tick = 0; tick < 240; tick++)
        {
            game.RunOneStep();
            maximumBullets = Math.Max(maximumBullets, game.EnemyBullets.Count);
            Assert.True(game.EnemyBullets.Count <= 2, StateSnapshot(game, tick));
        }
        Assert.Equal(2, maximumBullets);
    }

    [Fact]
    public void ProductionRenderedEnemyBulletIsTheSameObjectThatCanHitThePlayer()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = CreateProductionLevelTwo(random: new ArcadeFiringRandom(),
            startingLives: 1000, audioEvents: audio);
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        game.Player.ResetPosition(bullet.EdgeIndex);

        for (int tick = 0; tick < 133 && bullet.Height < Level.Depth - 11; tick++)
            bullet.Advance();
        Assert.True(bullet.Height == Level.Depth - 11,
            $"Bullet advance bound was reached. {StateSnapshot(game, 133)}");
        VectorRenderer renderer = new(game);
        Assert.Contains(bullet, game.EnemyBullets);
        Assert.Equal(4, CaptureEnemyBulletLines(renderer));

        game.RunOneStep();

        Assert.Same(bullet, Assert.Single(game.EnemyBullets));
        Assert.Equal(Level.Depth - 8, bullet.Height);
        Assert.Equal(LevelState.Playing, game.State);

        game.RunOneStep();

        Assert.DoesNotContain(bullet, game.EnemyBullets);
        Assert.True(game.State == LevelState.PlayerDying, StateSnapshot(game, 133));
        Assert.Contains(AudioEvent.PlayerKilledByBullet, audio.Events);
    }

    [Fact]
    public void DeathReentryRetainsProductionCapAndConservesUnresolvedSupply()
    {
        TempestGame game = CreateProductionLevelTwo(random: new Random(1981), startingLives: 1000);
        const int startingLives = 1000;
        Egg waitingEgg = RunToWaitingEgg(game, MaximumComparisonTicks);
        Flipper target = game.Flippers[0];
        PrepareLethalRimPivot(game, target);

        game.RunOneStep();

        Assert.True(game.State == LevelState.PlayerDying,
            StateSnapshot(game, 1));
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.True(game.State == LevelState.Playing,
            StateSnapshot(game, TempestGame.PlayerDeathTicks));
        Assert.Empty(game.Flippers);
        Assert.DoesNotContain(waitingEgg, game.Eggs);
        AssertProductionLevelTwoConservation(game, TempestGame.PlayerDeathTicks);

        for (int tick = 0; tick < MaximumComparisonTicks && game.Flippers.Count == 0; tick++)
        {
            game.RunOneStep();
            AssertProductionLevelTwoConservation(game, tick);
        }
        Assert.True(game.Flippers.Count > 0,
            StateSnapshot(game, MaximumComparisonTicks));
        PrepareLethalRimPivot(game, game.Flippers[0]);
        game.RunOneStep();
        Assert.True(game.State == LevelState.PlayerDying,
            StateSnapshot(game, MaximumComparisonTicks));
        for (int tick = 0; tick < TempestGame.PlayerDeathTicks; tick++)
            game.RunOneStep();

        Assert.True(game.Player.Lives == startingLives - 2,
            StateSnapshot(game, TempestGame.PlayerDeathTicks * 2));
        Assert.Empty(game.Flippers);
        AssertProductionLevelTwoConservation(game, TempestGame.PlayerDeathTicks * 2);

        bool sawFourAfterReentry = false;
        for (int tick = 0; tick < MaximumComparisonTicks; tick++)
        {
            game.RunOneStep();
            AssertProductionLevelTwoConservation(game, tick);
            Assert.True(game.Flippers.Count <= ActiveFlipperLimit,
                StateSnapshot(game, tick));
            sawFourAfterReentry |= game.Flippers.Count == ActiveFlipperLimit;
        }

        Assert.True(sawFourAfterReentry);
    }

    [Fact]
    public void AllTwelveProductionObligationsResolveAndAdvanceToLevelThree()
    {
        TempestGame game = CreateProductionLevelTwo(random: new Random(1981), startingLives: 1000);
        Level outgoing = game.CurrentLevel;
        const int maximumTicks = 60_000;

        for (int tick = 0; tick < maximumTicks &&
             ReferenceEquals(outgoing, game.CurrentLevel) && !game.GameOver; tick++)
        {
            if (game.State == LevelState.Playing && game.Flippers.Count > 0 &&
                game.Shots.Count == 0)
            {
                Flipper target = game.Flippers[0];
                if (target.Height < 20)
                {
                    game.RunOneStep();
                }
                else
                {
                    game.Player.Move(target.EdgeIndex - game.Player.EdgeIndex,
                        game.CurrentLevel.PlayableLaneCount);
                    game.RunOneStep(new PlayerInput(true, 0, false, false));
                    if (game.Shots.Count > 0)
                        game.Shots[game.Shots.Count - 1].Height = target.Height - 6;
                }
            }
            else
            {
                game.RunOneStep();
            }

            if (ReferenceEquals(outgoing, game.CurrentLevel))
                AssertProductionLevelTwoConservation(game, tick);
        }

        Assert.False(ReferenceEquals(outgoing, game.CurrentLevel),
            $"Level 2 did not complete and progress within {maximumTicks} ticks. " +
            StateSnapshot(game, maximumTicks));
        Assert.Same(LevelConfiguration.Level3Mixed, game.CurrentLevel.Configuration);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Equal(OriginalSupply,
            outgoing.EnemiesDestroyed + outgoing.EnemiesRimCleared);
        Assert.Equal(0, outgoing.ReserveEggs);
        Assert.Empty(outgoing.Eggs);
        Assert.Empty(game.Flippers);
        Assert.Equal(2, game.SuperZapperChargesRemaining);
    }

    [Fact]
    public void ProductionLevelTwoRecordingVisibleAndHeadlessPlaybackKeepTheCap()
    {
        GameplayStartupConfiguration startup = new(2, 1000, 1.0, 1981);
        PlayerInputRecorder recorder = new(startup);
        TempestGame recordedGame = new(startup);
        const int duration = 400;

        for (int tick = 0; tick < duration; tick++)
        {
            recordedGame.RunOneStep();
            recorder.CaptureTick(tick, default);
            AssertProductionLevelTwoConservation(recordedGame, tick);
        }

        GameplayRecording recording = new(startup, recorder.CreateReplay());
        VisiblePlaybackController visible = new(
            new GameplayPlaybackSession(startup, recording.Replay));
        HeadlessGameplayPlaybackRunner headless = new(recording);
        GameplayPlaybackRunResult headlessResult = headless.RunToBoundary(duration);

        Assert.Equal(2, recording.StartupConfiguration.StartingDisplayedLevel);
        for (int tick = 0; tick < 300; tick++)
            Assert.Equal(1, visible.ProcessControls(default));
        Assert.Equal(ActiveFlipperLimit, visible.Session.Game.Flippers.Count);
        Assert.Contains(visible.Session.Game.Eggs, egg => egg.State == EggState.ReadyToHatch);
        Assert.Equal(ActiveFlipperLimit, headlessResult.StateSummary.ActiveFlipperCount);
        Assert.Equal(visible.Session.Game.Eggs.Count,
            headlessResult.StateSummary.ActiveEggCount);

        Assert.Equal(0, visible.ProcessControls(
            new PlaybackControlInput(true, false, false)));
        Assert.True(visible.IsPaused);
        Assert.Equal(-1, visible.ProcessControls(new PlaybackControlInput(
            false, false, false, BackwardNavigationPressed: true)));
        Assert.Equal(299, visible.Session.CurrentBoundary);
        Assert.True(visible.Session.Game.Flippers.Count <= ActiveFlipperLimit);
        Assert.Equal(1, visible.ProcessControls(
            new PlaybackControlInput(false, true, false)));
        Assert.Equal(300, visible.Session.CurrentBoundary);
        Assert.True(visible.Session.Game.Flippers.Count <= ActiveFlipperLimit);
        Assert.Equal(0, visible.ProcessControls(
            new PlaybackControlInput(true, false, false)));
        Assert.False(visible.IsPaused);
        Assert.Equal(1, visible.ProcessControls(default));
        Assert.Equal(301, visible.Session.CurrentBoundary);
        Assert.True(visible.Session.Game.Flippers.Count <= ActiveFlipperLimit);

        Assert.Equal(recordedGame.Flippers.Count,
            headlessResult.StateSummary.ActiveFlipperCount);
    }

    private static TempestGame CreateProductionLevelTwo(Random? random = null, int startingLives = 3,
        double rimSpeedMultiplier = 1.0, IAudioEvents? audioEvents = null) =>
        new(random: random, startingLives: startingLives,
            rimSpeedMultiplier: rimSpeedMultiplier, audioEvents: audioEvents,
            startingProductionLevel: 2);

    private static TempestGame CreateNaturalLevelTwo()
    {
        TempestGame game = new(random: new Random(1981), startingLives: 1000);
        Level levelOne = game.CurrentLevel;
        for (int enemy = 0; enemy < levelOne.EnemySupply; enemy++)
            levelOne.RecordEnemyDestroyed();
        levelOne.ResetAfterDeath();

        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
        for (int tick = 0; tick < 1_000 &&
             ReferenceEquals(levelOne, game.CurrentLevel); tick++)
            game.RunOneStep();

        Assert.False(ReferenceEquals(levelOne, game.CurrentLevel),
            $"Natural campaign did not reach Level 2 within 1,000 ticks. state={game.State}");
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        for (int tick = 0; tick < 1_000 && game.State == LevelState.Approach; tick++)
            game.RunOneStep();
        Assert.True(game.State == LevelState.Playing,
            "The Level 2 approach did not complete within 1,000 ticks.");
        return game;
    }

    private static void AssertProductionLevelTwoStartup(TempestGame game)
    {
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        Assert.Same(LevelConfiguration.Level2Square.Definition, game.CurrentLevel.Definition);
        Assert.Equal(2, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(1, game.CurrentLevel.Configuration.ProgressionIndex);
        Assert.Equal(1, game.CurrentLevel.Configuration.BoardIndex);
        Assert.Equal(0, game.CurrentLevel.Configuration.PaletteIndex);
        Assert.Equal(16, game.CurrentLevel.PlayableLaneCount);
        Assert.Equal(OriginalSupply, game.CurrentLevel.EnemySupply);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(8, game.CurrentLevel.ReserveEggs);
        Assert.Equal(50, game.CurrentLevel.Configuration.ReserveReleaseIntervalTicks);
        Assert.Equal(4, game.CurrentLevel.Configuration.MaximumActiveEggs);
        Assert.Equal(2, game.CurrentLevel.Configuration.MaximumApproachingEggs);
        Assert.Equal(50, game.CurrentLevel.Configuration.HatchSelectionIntervalTicks);
        Assert.Equal(new[] { 12, 4, 9, 1, 14, 6, 10, 2, 15, 7, 3, 11 },
            game.CurrentLevel.Configuration.SpawnLaneSequence);
        Assert.True(game.CurrentLevel.Configuration.FlipperCanFlip);
        Assert.Equal(FlipperPivotGeometry.EndpointHinge,
            game.CurrentLevel.Configuration.FlipperPivotMode);
        Assert.Equal(2, game.CurrentLevel.Configuration.MaximumActiveEnemyShots);
        Assert.Equal(77, game.CurrentLevel.Configuration.FlipperShotHoldoffTicks);
    }

    private static void AssertProductionLevelTwoConservation(TempestGame game, int tick)
    {
        Assert.True(game.Flippers.Count <= ActiveFlipperLimit, StateSnapshot(game, tick));
        Assert.True(game.EnemyBullets.Count <=
            game.CurrentLevel.Configuration.MaximumActiveEnemyShots,
            StateSnapshot(game, tick));
        Assert.Equal(OriginalSupply,
            game.CurrentLevel.ReserveEggs + game.Eggs.Count + game.Flippers.Count +
            game.CurrentLevel.EnemiesDestroyed + game.CurrentLevel.EnemiesRimCleared);
    }

    private static Egg RunToWaitingEgg(TempestGame game, int maximumTicks)
    {
        for (int tick = 0; tick < maximumTicks; tick++)
        {
            game.RunOneStep();
            AssertProductionLevelTwoConservation(game, tick);
            Egg? waitingEgg = game.Eggs.FirstOrDefault(
                egg => egg.State == EggState.ReadyToHatch);
            if (game.Flippers.Count == ActiveFlipperLimit && waitingEgg is not null)
                return waitingEgg;
        }

        Assert.Fail(
            $"No ready Egg waited at the four-Flipper cap within {maximumTicks} ticks. " +
            StateSnapshot(game, maximumTicks));
        throw new InvalidOperationException("The bounded waiting-Egg search failed.");
    }

    private static void PrepareLethalRimPivot(TempestGame game, Flipper flipper)
    {
        for (int tick = 0; tick < 3_000 &&
             (flipper.Height < Level.Depth || flipper.State == FlipperState.Flipping); tick++)
            flipper.Update(game.CurrentLevel, (flipper.DestinationEdge + 8) % 16);
        Assert.True(flipper.Height == Level.Depth,
            $"{StateSnapshot(game, 3_000)} flipperLane={flipper.EdgeIndex}, " +
            $"height={flipper.Height}, flipperState={flipper.State}.");
        Assert.True(flipper.State == FlipperState.Pausing,
            $"{StateSnapshot(game, 3_000)} flipperLane={flipper.EdgeIndex}, " +
            $"height={flipper.Height}, flipperState={flipper.State}.");

        int destination = game.CurrentLevel.NextEdge(flipper.EdgeIndex);
        game.Player.Move(destination - game.Player.EdgeIndex, 16);
        for (int tick = 0; tick < 1_000 && flipper.State == FlipperState.Pausing; tick++)
            flipper.Update(game.CurrentLevel, destination);
        Assert.True(flipper.State == FlipperState.Flipping,
            $"{StateSnapshot(game, 1_000)} flipperLane={flipper.EdgeIndex}, " +
            $"height={flipper.Height}, flipperState={flipper.State}.");

        int turnTicks = (int)Math.Ceiling(
            flipper.TotalTurnAngle / game.CurrentLevel.RimTurnSpeedDegrees);
        for (int tick = 0; tick < turnTicks - 1; tick++)
            flipper.Update(game.CurrentLevel, destination);
        Assert.False(flipper.CompletedIntoPlayer,
            $"{StateSnapshot(game, turnTicks)} flipperLane={flipper.EdgeIndex}, " +
            $"destination={destination}, flipperState={flipper.State}.");
    }

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.True(egg.State == EggState.ReadyToHatch,
            $"Egg readiness bound was reached at tick 100: state={egg.State}, " +
            $"lane={egg.EdgeIndex}, height={egg.Height}, updates={egg.UpdateCounter}.");
    }

    private static EggStateSnapshot[] GetEggSnapshots(TempestGame game) =>
        game.Eggs.Select(GetEggSnapshot).ToArray();

    private static EggStateSnapshot GetEggSnapshot(Egg egg) =>
        new(egg.EdgeIndex, egg.MovementDirection, egg.Height, egg.UpdateCounter, egg.State);

    private static FlipperStateSnapshot[] GetFlipperSnapshots(TempestGame game) =>
        game.Flippers.Select(flipper => new FlipperStateSnapshot(
            flipper.EdgeIndex, flipper.Height, flipper.Direction, flipper.State,
            flipper.SourceEdge, flipper.DestinationEdge, flipper.ShotDelayTicks,
            flipper.PendingZapTicks, flipper.TurnAngle, flipper.TotalTurnAngle)).ToArray();

    private static EnemyBulletSnapshot[] GetBulletSnapshots(TempestGame game) =>
        game.EnemyBullets.Select(bullet =>
            new EnemyBulletSnapshot(bullet.EdgeIndex, bullet.Height, bullet.Rotation)).ToArray();

    private static string StateSnapshot(TempestGame game, int tick) =>
        $"tick={tick}, activeFlippers={game.Flippers.Count}, reserve={game.CurrentLevel.ReserveEggs}, " +
        $"eggs(roaming={game.Eggs.Count(egg => egg.State == EggState.Roaming)}, " +
        $"approaching={game.Eggs.Count(egg => egg.State == EggState.Approaching)}, " +
        $"ready={game.Eggs.Count(egg => egg.State == EggState.ReadyToHatch)}), " +
        $"unresolved={game.CurrentLevel.RemainingEnemies}, state={game.State}.";

    private static int CaptureEnemyBulletLines(VectorRenderer renderer)
    {
        List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End,
            Raylib_cs.Color Color)> lines = [];
        renderer.DrawEnemyBullets((start, end, color) => lines.Add((start, end, color)));
        return lines.Count;
    }

    private sealed record EggStateSnapshot(
        int Lane, int Direction, float Height, int UpdateCounter, EggState State);

    private sealed record FlipperStateSnapshot(
        int Lane, float Height, int Direction, FlipperState State, int SourceLane,
        int DestinationLane, int ShotDelay, int PendingZapTicks, float TurnAngle,
        float TotalTurnAngle);

    private sealed record EnemyBulletSnapshot(int Lane, float Height, float Rotation);

    private sealed class ArcadeFiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == 0 && maxValue == 256 ? 255 : minValue;
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        private readonly List<AudioEvent> events = [];
        public IReadOnlyList<AudioEvent> Events => events;
        public int Count(AudioEvent audioEvent) => events.Count(entry => entry == audioEvent);
        public void Play(AudioEvent audioEvent) => events.Add(audioEvent);
    }
}
