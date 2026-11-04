using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class ArcadeFlipperFiringTests
{
    [Theory]
    [InlineData(1, 80, 2)]
    [InlineData(2, 77, 2)]
    [InlineData(3, 74, 2)]
    [InlineData(4, 71, 3)]
    [InlineData(5, 68, 4)]
    [InlineData(6, 65, 3)]
    [InlineData(7, 62, 3)]
    public void ProductionLevelsUseRomHoldoffAndDefaultShotCapacity(
        int displayedLevel, int holdoff, int capacity)
    {
        LevelConfiguration configuration = ConfigurationFor(displayedLevel);

        Assert.Equal(holdoff, configuration.FlipperShotHoldoffTicks);
        Assert.Equal(capacity, configuration.MaximumActiveEnemyShots);
    }

    [Fact]
    public void NewProductionFlipperStartsReadyAndFirstZeroPressureShotIsGuaranteed()
    {
        RecordingAudioEvents audio = new();
        TempestGame game = CreateGame(LevelConfiguration.Level1, new ScriptedRandom([255]), audio);
        Egg egg = game.Eggs[0];
        MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Flipper flipper = Assert.Single(game.Flippers);
        EnemyBullet bullet = Assert.Single(game.EnemyBullets);
        Assert.Equal(80, flipper.ShotDelayTicks);
        Assert.Equal(flipper.EdgeIndex, bullet.EdgeIndex);
        Assert.Equal(flipper.Height, bullet.Height);
        Assert.Equal(1, audio.Count(AudioEvent.EnemyBullet));
    }

    [Fact]
    public void PressureThresholdsAreInclusiveAndRejectionOrFullCapacityKeepsFlipperReady()
    {
        int[] rolls = [255, 223, 224, 239, 240, 249, 250, 254, 255];
        LevelConfiguration configuration = LevelConfiguration.Level1 with
        {
            DisplayedLevel = 5,
            InitialActiveEggCount = rolls.Length,
            MaximumActiveEggs = rolls.Length,
            SpawnLaneSequence = Array.AsReadOnly(Enumerable.Range(0, 10).ToArray())
        };
        RecordingAudioEvents audio = new();
        TempestGame game = CreateGame(configuration, new ScriptedRandom(rolls), audio);
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Flipper[] flippers = game.Flippers.ToArray();
        Assert.Equal(rolls.Length, flippers.Length);
        Assert.Equal(4, game.EnemyBullets.Count);
        Assert.Equal(new[] { 68, 0, 68, 0, 68, 0, 68, 0, 0 },
            flippers.Select(flipper => flipper.ShotDelayTicks));
        Assert.Equal(4, audio.Count(AudioEvent.EnemyBullet));
        Assert.Equal(0, flippers[^1].ShotDelayTicks);
    }

    [Theory]
    [InlineData(343, true)]
    [InlineData(344, false)]
    public void ArcadeAlongCutoffMapsToIntegerProjectHeightAndIneligibleDelayStaysReady(
        float climbSpeed, bool fires)
    {
        LevelConfiguration configuration = LevelConfiguration.Level1 with
        {
            Flipper = new(false, climbSpeed, 8, 21, 24)
        };
        TempestGame game = CreateGame(configuration, new ScriptedRandom([0]));
        MakeReady(game.Eggs[0], game.CurrentLevel);

        game.RunOneStep();

        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Equal(climbSpeed, flipper.Height);
        Assert.Equal(fires ? 80 : 0, flipper.ShotDelayTicks);
        Assert.Equal(fires ? 1 : 0, game.EnemyBullets.Count);
    }

    [Fact]
    public void Level2LaneChangesDoNotFreezeShotDelayAndReadyShotWaitsForPivotCompletion()
    {
        TempestGame game = new(random: new ArcadeFiringRandom());
        CompleteLevel1Wave(game);
        for (int tick = 0; tick < 200 &&
             !ReferenceEquals(game.CurrentLevel.Configuration, LevelConfiguration.Level2Square); tick++)
            game.RunOneStep();
        Assert.Same(LevelConfiguration.Level2Square, game.CurrentLevel.Configuration);
        for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
            game.RunOneStep();

        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Single(game.EnemyBullets);
        Assert.Equal(77, flipper.ShotDelayTicks);

        int laneChanges = 0;
        int previousLane = flipper.EdgeIndex;
        int successfulReloads = 0;
        for (int tick = 0; tick < 240 && flipper.Height <= 343 && successfulReloads < 1; tick++)
        {
            int previousDelay = flipper.ShotDelayTicks;
            game.RunOneStep();
            if (flipper.Height <= 343 && previousDelay > 1)
                Assert.Equal(previousDelay - 1, flipper.ShotDelayTicks);
            else if (flipper.Height <= 343 && previousDelay == 1)
                Assert.Contains(flipper.ShotDelayTicks, new[] { 0, 77 });
            if (flipper.ShotDelayTicks == 77 && previousDelay <= 1)
            {
                successfulReloads++;
                Assert.True(flipper.Height < Level.Depth);
            }
            if (flipper.EdgeIndex != previousLane)
            {
                laneChanges++;
                previousLane = flipper.EdgeIndex;
            }
        }

        Assert.Equal(1, successfulReloads);
        Assert.True(laneChanges >= 2);
        Assert.Equal(2, game.EnemyBullets.Count);
        Assert.Equal(77, flipper.ShotDelayTicks);
        Assert.True(flipper.Height < Level.Depth);
    }

    [Fact]
    public void ReadyFlipperWaitsForActivePivotThenFiresImmediatelyAfterCompletion()
    {
        LevelConfiguration slowPivotLevel = LevelConfiguration.Level2Square with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1,
            MaximumApproachingEggs = 1,
            SpawnLaneSequence = Array.AsReadOnly(new[] { 12 }),
            Flipper = new(true, 2, 1, 21, 24)
        };
        TempestGame game = CreateGame(
            slowPivotLevel, new ScriptedRandom(Enumerable.Repeat(255, 10)));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Single(game.EnemyBullets);

        for (int tick = 0; tick < 77; tick++)
            game.RunOneStep();

        Assert.Equal(0, flipper.ShotDelayTicks);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        Assert.Single(game.EnemyBullets);

        for (int tick = 0; tick < 200 && flipper.State == FlipperState.Flipping; tick++)
            game.RunOneStep();

        Assert.NotEqual(FlipperState.Flipping, flipper.State);
        Assert.Equal(2, game.EnemyBullets.Count);
        Assert.Equal(77, flipper.ShotDelayTicks);
    }

    [Fact]
    public void NormalLevel2BelowRimLaneChangesContinueAdvancingTheShotDelay()
    {
        LevelConfiguration oneFlipperLevel = LevelConfiguration.Level2Square with
        {
            EnemySupply = 1,
            InitialActiveEggCount = 1,
            MaximumActiveEggs = 1,
            MaximumApproachingEggs = 1,
            SpawnLaneSequence = Array.AsReadOnly(new[] { 12 })
        };
        TempestGame game = CreateGame(
            oneFlipperLevel, new ScriptedRandom(Enumerable.Repeat(255, 10)));
        MakeReady(game.Eggs[0], game.CurrentLevel);
        game.RunOneStep();
        Flipper flipper = Assert.Single(game.Flippers);
        Assert.Single(game.EnemyBullets);

        int laneChanges = 0;
        bool becameReady = false;
        int previousLane = flipper.EdgeIndex;
        for (int tick = 0; tick < 150 && !becameReady; tick++)
        {
            int previousDelay = flipper.ShotDelayTicks;
            game.RunOneStep();
            if (previousDelay > 0 && flipper.ShotDelayTicks == 0)
                becameReady = true;
            if (flipper.EdgeIndex != previousLane)
            {
                laneChanges++;
                previousLane = flipper.EdgeIndex;
            }
        }

        Assert.True(becameReady);
        Assert.True(laneChanges >= 2);
        Assert.True(flipper.Height < Level.Depth);
    }

    private static LevelConfiguration ConfigurationFor(int displayedLevel) => displayedLevel switch
    {
        1 => LevelConfiguration.Level1,
        2 => LevelConfiguration.Level2Square,
        3 => LevelConfiguration.Level3Mixed,
        4 => LevelConfiguration.Level4Candidate,
        5 => LevelConfiguration.Level5Production,
        6 => LevelConfiguration.Level6Production,
        7 => LevelConfiguration.Level7Production,
        _ => throw new ArgumentOutOfRangeException(nameof(displayedLevel))
    };

    [Fact]
    public void DirectLevel6DiagnosticUsesProductionFlipperFiringAndReplaysLevel6()
    {
        string[] arguments = ["--level6-test", "--lives", "10"];
        TempestGame game = new(level6Test: DebugController.UseLevel6Test(arguments),
            startingLives: DebugController.GetStartingLives(arguments),
            random: new Level6FiringRandom());

        Assert.True(DebugController.UseLevel6Test(arguments));
        Assert.False(DebugController.HasConflictingScenarios(arguments));
        Assert.Equal(10, game.Player.Lives);
        Assert.True(game.Level6Test);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.False(game.EnemyBulletsTest);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Equal(4, game.Flippers.Count);
        Assert.Equal(3, game.EnemyBullets.Count);
        Assert.Equal(new[] { 65, 65, 65, 0 },
            game.Flippers.Select(flipper => flipper.ShotDelayTicks));
        Assert.True(DebugController.HasConflictingScenarios(["--level6-test", "--level4-test"]));
        Assert.Throws<ArgumentException>(() => new TempestGame(level6Test: true, level4Test: true));

        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);

        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        Assert.Equal(4, game.Eggs.Count);
        Assert.Equal(18, game.CurrentLevel.ReserveEggs);
        Assert.Empty(game.Flippers);
        Assert.Empty(game.EnemyBullets);

        game.Score.Add(100);
        game.Player.Lives = 0;
        game.CurrentLevel.State = LevelState.GameOver;
        game.RunOneStep(restartPressed: true);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        Assert.Equal(10, game.Player.Lives);
        Assert.Equal(0, game.Score.Score);
    }

    [Fact]
    public void NaturalLevel6UsesProductionFlipperFiringConfiguration()
    {
        TempestGame game = new(random: new Level6FiringRandom());
        while (game.CurrentLevel.Configuration.DisplayedLevel < 6)
        {
            Level outgoing = game.CurrentLevel;
            if (outgoing.Configuration.StartingSpikeHeight is not null)
                game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
            outgoing.State = LevelState.ExitZoom;
            game.Player.AdvanceExit(Level.Depth - 8);
            game.RunOneStep();
            Assert.NotSame(outgoing, game.CurrentLevel);
            Assert.Equal(outgoing.Configuration.DisplayedLevel + 1,
                game.CurrentLevel.Configuration.DisplayedLevel);
            for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
                game.RunOneStep();
        }

        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Equal(4, game.Flippers.Count);
        Assert.Equal(3, game.EnemyBullets.Count);
        Assert.Equal(new[] { 65, 65, 65, 0 },
            game.Flippers.Select(flipper => flipper.ShotDelayTicks));
    }

    [Fact]
    public void NaturalLevel7UsesProductionPickerAndFlipperFiringConfiguration()
    {
        TempestGame game = new(random: new Level6FiringRandom());
        while (game.CurrentLevel.Configuration.DisplayedLevel < 7)
        {
            Level outgoing = game.CurrentLevel;
            if (outgoing.Configuration.StartingSpikeHeight is not null)
                game.Spikes.Single(spike => spike.EdgeIndex == game.Player.EdgeIndex).Height = 0;
            outgoing.State = LevelState.ExitZoom;
            game.Player.AdvanceExit(Level.Depth - 8);
            game.RunOneStep();
            Assert.NotSame(outgoing, game.CurrentLevel);
            for (int tick = 0; tick < LevelApproach.DurationTicks; tick++)
                game.RunOneStep();
        }

        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(LevelConfiguration.Level7Production, game.CurrentLevel.Configuration);
        foreach (Egg egg in game.Eggs)
            MakeReady(egg, game.CurrentLevel);

        game.RunOneStep();

        Assert.Empty(game.Flippers);
        Assert.Equal(3, game.Spikers.Count);
        Assert.Single(game.Tankers);
        Assert.Equal(62, game.CurrentLevel.Configuration.FlipperShotHoldoffTicks);
        Assert.Equal(3, game.CurrentLevel.Configuration.MaximumActiveEnemyShots);
    }

    private static TempestGame CreateGame(
        LevelConfiguration configuration, Random random, IAudioEvents? audio = null) =>
        new(enemyBulletsTest: true, random: random, startingConfiguration: configuration,
            audioEvents: audio);

    private static void MakeReady(Egg egg, Level level)
    {
        egg.BeginApproach();
        for (int tick = 0; tick < 100 && egg.State != EggState.ReadyToHatch; tick++)
            egg.Update(level);
        Assert.Equal(EggState.ReadyToHatch, egg.State);
    }

    private static void CompleteLevel1Wave(TempestGame game)
    {
        for (int enemyIndex = 0; enemyIndex < game.CurrentLevel.EnemySupply; enemyIndex++)
            game.CurrentLevel.RecordEnemyDestroyed();
        game.CurrentLevel.ResetAfterDeath();
        game.RunOneStep();
        Assert.Equal(LevelState.ExitDelay, game.State);
    }

    private sealed class ArcadeFiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == 0 && maxValue == 256 ? 255 : minValue;
    }

    private sealed class Level6FiringRandom : Random
    {
        public override int Next(int minValue, int maxValue) =>
            minValue == -6 && maxValue == 5 ? 1 :
            minValue == 0 && maxValue == 256 ? 255 : minValue;
    }

    private sealed class ScriptedRandom(IEnumerable<int> values) : Random
    {
        private readonly Queue<int> values = new(values);

        public override int Next(int minValue, int maxValue)
        {
            if (minValue == 0 && maxValue == 2)
                return 0;
            if (minValue != 0 || maxValue != 256)
                throw new InvalidOperationException($"Unexpected random range [{minValue}, {maxValue}).");
            if (values.Count == 0)
                throw new InvalidOperationException("The scripted enemy-shot random values were exhausted.");
            return values.Dequeue();
        }
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        private readonly List<AudioEvent> events = [];
        public int Count(AudioEvent audioEvent) => events.Count(entry => entry == audioEvent);
        public void Play(AudioEvent audioEvent) => events.Add(audioEvent);
    }
}
