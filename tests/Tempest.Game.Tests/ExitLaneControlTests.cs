using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class ExitLaneControlTests
{
    private static readonly PlayerInput[] MovementInputs =
    [
        new(false, 80, false, false),
        new(false, -80, false, false),
        new(false, 0, true, false),
        new(false, 0, false, true),
        new(false, 0, false, false, ControllerAxis: -1),
        new(false, 0, false, false, ControllerAxis: 1)
    ];

    public static IEnumerable<object[]> OrdinaryExitInputs()
    {
        for (int level = 1; level <= 3; level++)
            for (int source = 0; source < MovementInputs.Length; source++)
                yield return [level, source];
    }

    [Theory]
    [MemberData(nameof(OrdinaryExitInputs))]
    public void OrdinaryExitUsesNormalMovementThroughoutBothTravelPhasesAndReachesApproach(
        int displayedLevel, int inputSource)
    {
        RecordingAudioEvents audio = new();
        TempestGame game = CreateNaturalLevel(displayedLevel, audio);
        ResolveWave(game);
        Level outgoing = game.CurrentLevel;
        game.Score.Add(125);
        audio.Events.Clear();

        Assert.Equal(LevelState.ExitDelay, game.State);
        Assert.False(game.ExitMovementInputEnabled);
        PlayerInput heldActions = new(true, 80, false, true, ZapPressed: true, ControllerAxis: 1);
        for (int tick = 0; tick < 21; tick++)
        {
            game.RunOneStep(heldActions);
            Assert.Equal(tick < 20 ? LevelState.ExitDelay : LevelState.ExitZoom, game.State);
            Assert.Equal(7, game.Player.EdgeIndex);
            Assert.Equal(.5f, game.Player.EdgePosition);
            Assert.Equal(Level.Depth, game.Player.Depth);
            Assert.Equal(70, Projection.K);
            Assert.Equal(5, Projection.ZOffset);
            Assert.Empty(game.Shots);
        }
        Assert.Equal(1, audio.Events.Count(e => e == AudioEvent.ZoomOut));

        Player expected = new();
        float expectedDepth = Level.Depth;
        float expectedK = 70;
        float expectedZOffset = 5;
        for (int tick = 0; tick < 123; tick++)
        {
            PlayerInput input = MovementInputs[inputSource] with
            {
                Fire = true,
                ZapPressed = true
            };
            int previousLane = expected.EdgeIndex;
            expected.MoveAlongTube(AcceptedMovement(input), outgoing);
            int previousMoveEvents = audio.Events.Count(e => e == AudioEvent.PlayerMove);
            Assert.True(game.ExitMovementInputEnabled);
            Assert.False(game.SpikeExitControlsEnabled);

            game.RunOneStep(input);

            Assert.Equal(expected.EdgeIndex, game.Player.EdgeIndex);
            Assert.Equal(expected.EdgePosition, game.Player.EdgePosition);
            Assert.Equal(previousMoveEvents + (expected.EdgeIndex != previousLane ? 1 : 0),
                audio.Events.Count(e => e == AudioEvent.PlayerMove));
            if (expectedZOffset >= -40)
            {
                expectedZOffset -= 2;
                expectedDepth -= 3;
            }
            else
            {
                expectedK += 2;
                if (expectedK > 100)
                    expectedK += 2;
                expectedDepth -= 3.2f;
            }
            Assert.Equal(expectedDepth, game.Player.Depth);
            Assert.Equal(expectedZOffset, Projection.ZOffset);
            Assert.Equal(expectedK, Projection.K);
            Assert.Same(outgoing, game.CurrentLevel);
            Assert.Equal(LevelState.ExitZoom, game.State);
            Assert.InRange(game.Shots.Count, 0, 10);
            Assert.True(game.ExitShotsVisible);
            Assert.Equal(2, game.SuperZapperChargesRemaining);
            Assert.Equal(125, game.Score.Score);
            Assert.Equal(5, game.Player.Lives);
        }
        Assert.InRange(game.Player.Depth, 10.99f, 11.01f);
        Assert.NotEmpty(game.Shots);
        Assert.Contains(AudioEvent.PlayerMove, audio.Events);

        game.RunOneStep(heldActions);

        Assert.NotSame(outgoing, game.CurrentLevel);
        Assert.Equal(displayedLevel + 1, game.CurrentLevel.Configuration.DisplayedLevel);
        Assert.Equal(LevelState.Approach, game.State);
        Assert.Equal(0, game.CurrentApproach!.ElapsedTicks);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(.5f, game.Player.EdgePosition);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Equal(70, Projection.K);
        Assert.Equal(5, Projection.ZOffset);
        Assert.Empty(game.Shots);
        Assert.Equal(125, game.Score.Score);
        Assert.Equal(5, game.Player.Lives);
        Assert.Equal(1, audio.Events.Count(e => e == AudioEvent.ZoomOut));
        Assert.Equal(1, audio.Events.Count(e => e == AudioEvent.ZoomIn));
        Step(game, LevelApproach.DurationTicks);
        Assert.Equal(LevelState.Playing, game.State);
        Assert.Null(game.CurrentApproach);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void SpikeExitStillUsesNormalMovementDuringActualTravel(int displayedLevel)
    {
        TempestGame game = CreateSpikeExit(displayedLevel);
        Step(game, game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold + 1);
        Assert.Equal(LevelState.ExitDelay, game.State);
        Step(game, 21);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Player expected = new();

        for (int tick = 0; tick < 30; tick++)
        {
            PlayerInput input = MovementInputs[tick % MovementInputs.Length];
            expected.MoveAlongTube(AcceptedMovement(input), game.CurrentLevel);
            Assert.True(game.SpikeExitControlsEnabled);
            Assert.True(game.ExitMovementInputEnabled);
            game.RunOneStep(input);
            Assert.Equal(LevelState.ExitZoom, game.State);
            Assert.Equal(expected.EdgeIndex, game.Player.EdgeIndex);
            Assert.Equal(expected.EdgePosition, game.Player.EdgePosition);
            Assert.True(game.Player.Depth < Level.Depth);
        }
        Assert.All(game.Spikes, spike => Assert.Equal(60, spike.Height));
        Assert.Equal(5, game.Player.Lives);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void SpikeWaitCollisionReverseAndRetryRemainUnchanged(int displayedLevel)
    {
        TempestGame game = CreateSpikeExit(displayedLevel);
        Level outgoing = game.CurrentLevel;
        Spike[] spikes = game.Spikes.ToArray();
        PlayerInput fire = new(true, 0, false, false);
        int avoidanceThreshold = game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold;
        for (int tick = 0; tick <= avoidanceThreshold; tick++)
        {
            Assert.True(game.ExitMovementInputEnabled);
            game.RunOneStep(fire);
            Assert.Equal(tick < avoidanceThreshold
                ? LevelState.AvoidSpikes : LevelState.ExitDelay, game.State);
            Assert.Equal(Level.Depth, game.Player.Depth);
            Assert.Empty(game.Shots);
        }
        for (int tick = 0; tick < 21; tick++)
        {
            game.RunOneStep(fire);
            Assert.Equal(tick < 20 ? LevelState.ExitDelay : LevelState.ExitZoom, game.State);
            Assert.Empty(game.Shots);
        }

        game.RunOneStep(fire);
        Shot shot = Assert.Single(game.Shots);
        Assert.Equal(397, shot.Height);
        Spike collisionSpike = spikes.Single(s => s.EdgeIndex == game.Player.EdgeIndex);
        collisionSpike.Height = 397;
        game.RunOneStep(new PlayerInput(true, 80, false, false));

        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.False(game.ExitMovementInputEnabled);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(.5f, game.Player.EdgePosition);
        Assert.Equal(TempestGame.PlayerDeathTicks, game.DeathTicks);
        Assert.Equal(397, shot.Height);
        Step(game, TempestGame.PlayerDeathTicks - 1);
        Assert.Equal(LevelState.ReverseZoom, game.State);
        Assert.Equal(5, game.Player.Lives);
        game.RunOneStep();
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(Level.Depth, game.Player.Depth);
        Assert.Same(outgoing, game.CurrentLevel);
        Assert.Equal(spikes, game.Spikes);
        Assert.Empty(outgoing.Level3Ledger!.UnresolvedOriginalIds);
        Assert.Empty(game.Eggs);
        // The already-fired shot resumes during reverse zoom and shortens the contacted Spike.
        Assert.Equal(387, collisionSpike.Height);
        Assert.Equal(1, game.Score.Score);

        collisionSpike.Height = 0;
        Step(game, avoidanceThreshold + 1 + 21);
        Assert.Equal(LevelState.ExitZoom, game.State);
        Step(game, 123);
        Assert.Equal(LevelState.ExitZoom, game.State);
        game.RunOneStep();
        Assert.Equal(LevelState.Complete, game.State);
        Assert.Equal(4, game.Player.Lives);
        Assert.Equal(1, game.Score.Score);
    }

    [Fact]
    public void LevelSixFinalSpikeCollisionEndsWithoutInstallingAnotherLevel()
    {
        TempestGame game = CreateSpikeExit(6);
        int avoidanceThreshold = game.CurrentLevel.Configuration.AvoidSpikesWaitThreshold;
        Step(game, avoidanceThreshold + 1 + 21);
        Assert.Equal(LevelState.ExitZoom, game.State);

        game.RunOneStep(new PlayerInput(true, 0, false, false));
        Shot shot = Assert.Single(game.Shots);
        Spike collisionSpike = game.Spikes.Single(spike =>
            spike.EdgeIndex == game.Player.EdgeIndex);
        collisionSpike.Height = shot.Height;
        game.Player.Lives = 1;
        game.RunOneStep(new PlayerInput(true, 80, false, false));
        Assert.Equal(LevelState.ReverseZoom, game.State);

        Step(game, TempestGame.PlayerDeathTicks);

        Assert.Equal(LevelState.GameOverPresentation, game.State);
        Step(game, 500);
        Assert.True(game.GameOver);
        Assert.Same(LevelConfiguration.Level6Production, game.CurrentLevel.Configuration);
        Assert.False(game.NaturalProgressionEnabled);
        Assert.Equal(0, game.Player.Lives);
    }

    private static float AcceptedMovement(PlayerInput input) =>
        input.MouseDeltaX * .0125f - (input.Left ? .05f : 0) +
        (input.Right ? .05f : 0) + input.ControllerAxis * .05f;

    private static TempestGame CreateNaturalLevel(int displayedLevel, IAudioEvents audio)
    {
        TempestGame game = new(startingLives: 5, random: new Random(0), audioEvents: audio);
        // Earlier handoffs are setup only; the selected wave's completion and full exit are exercised.
        while (game.CurrentLevel.Configuration.DisplayedLevel < displayedLevel)
        {
            game.CurrentLevel.State = LevelState.ExitZoom;
            game.Player.AdvanceExit(Level.Depth - 12);
            game.RunOneStep();
            Step(game, LevelApproach.DurationTicks);
        }
        return game;
    }

    private static TempestGame CreateSpikeExit(int displayedLevel)
    {
        LevelConfiguration configuration = displayedLevel switch
        {
            4 => LevelConfiguration.Level4Candidate,
            5 => LevelConfiguration.Level5Production,
            6 => LevelConfiguration.Level6Production,
            7 => LevelConfiguration.Level7Production,
            _ => throw new ArgumentOutOfRangeException(nameof(displayedLevel))
        };
        TempestGame game = new(configuration, startingLives: 5, random: new Random(0));
        ResolveWave(game);
        Assert.Equal(LevelState.AvoidSpikes, game.State);
        return game;
    }

    private static void ResolveWave(TempestGame game)
    {
        Level level = game.CurrentLevel;
        if (level.Level3Ledger is Level3WaveLedger ledger)
        {
            foreach (int id in ledger.UnresolvedOriginalIds.Order())
            {
                if (ledger.GetOriginalState(id) == Level3OriginalState.Reserve)
                    ledger.MarkOriginalEgg(id);
                ledger.CommitOriginalAsFlipper(id);
                ledger.MarkOriginalDestroyed(id);
            }
        }
        else
        {
            for (int enemy = 0; enemy < level.EnemySupply; enemy++)
                level.RecordEnemyDestroyed();
        }
        level.ResetAfterDeath();
        game.RunOneStep();
    }

    private static void Step(TempestGame game, int ticks)
    {
        for (int tick = 0; tick < ticks; tick++)
            game.RunOneStep();
    }

    private sealed class RecordingAudioEvents : IAudioEvents
    {
        public List<AudioEvent> Events { get; } = [];
        public void Play(AudioEvent audioEvent) => Events.Add(audioEvent);
    }
}
