using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class GameplayPlaybackSessionTests
{
	[Fact]
	public void SessionStartsAtBoundaryZeroWithRestoredProductionStartup()
	{
		GameplayStartupConfiguration startup = new(5, 10, 2.5, 1981);
		GameplayPlaybackSession session = CreateSession(startup, 3);

		GameplayStateSummary summary = session.GetStateSummary();

		Assert.Equal(0, session.CurrentBoundary);
		Assert.Equal(0, session.NextRecordingFrame);
		Assert.Equal(3, session.RecordingDurationTicks);
		Assert.Equal(3, session.RemainingRecordedFrames);
		Assert.False(session.IsRecordingComplete);
		Assert.Equal(startup.StartingDisplayedLevel, summary.StartingDisplayedLevel);
		Assert.Equal(startup.StartingLives, summary.StartingLives);
		Assert.Equal(startup.RimSpeedMultiplier, summary.StartingRimSpeedMultiplier);
		Assert.Equal(startup.RandomSeed, summary.RandomSeed);
		Assert.Same(LevelConfiguration.Level5Production, session.Game.CurrentLevel.Configuration);
		Assert.Equal(startup.StartingLives, session.Game.Player.Lives);
		Assert.Equal(startup.RimSpeedMultiplier,
			session.Game.CurrentLevel.RimTurnSpeedMultiplier);
		Assert.Equal(session.Game.Shots.Count, summary.PlayerShotCount);
		Assert.Equal(session.Game.EnemyBullets.Count, summary.EnemyBulletCount);
		Assert.Equal(session.Game.Eggs.Count, summary.ActiveEggCount);
		Assert.Equal(session.Game.Flippers.Count, summary.ActiveFlipperCount);
		Assert.Equal(session.Game.Tankers.Count, summary.ActiveTankerCount);
		Assert.Equal(session.Game.Spikers.Count, summary.ActiveSpikerCount);
		Assert.Equal(session.Game.Spikes.Count, summary.ActiveSpikeCount);
		Assert.Equal(session.Game.Fuseballs.Count, summary.ActiveFuseballCount);
		Assert.Equal(session.Game.Pulsars.Count, summary.ActivePulsarCount);
	}

	[Fact]
	public void OneForwardStepConsumesOneFrameAndRunsOneProductionStep()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame frame = new(0, new PlayerInput(
			true, 0.5f, false, true, ZapPressed: true, ControllerAxis: 0.25f));
		GameplayPlaybackSession session = CreateSession(startup, 2, frame);
		TempestGame expected = new(startup);
		expected.RunOneStep(frame.Input, frame.RestartPressed);

		Assert.True(session.StepForward());

		Assert.Equal(1, session.CurrentBoundary);
		Assert.Equal(1, session.NextRecordingFrame);
		Assert.Equal(1, session.RemainingRecordedFrames);
		Assert.False(session.IsRecordingComplete);
		AssertEquivalentGameplayState(expected, session.Game);
		Assert.Equal(1, session.GetStateSummary().Boundary);
	}

	[Fact]
	public void RunToCurrentBoundaryIsANoOpAndRunToBoundaryAdvancesExactlyThere()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 4);
		GameplayStateSummary initial = session.GetStateSummary();

		AssertEquivalentSummary(initial, session.RunToBoundary(0));
		Assert.Equal(0, session.CurrentBoundary);

		Assert.True(session.StepForward());
		GameplayStateSummary current = session.GetStateSummary();
		AssertEquivalentSummary(current, session.RunToBoundary(1));
		Assert.Equal(1, session.CurrentBoundary);

		GameplayStateSummary boundaryThree = session.RunToBoundary(3);
		Assert.Equal(3, boundaryThree.Boundary);
		Assert.Equal(3, session.NextRecordingFrame);
		Assert.Equal(1, session.RemainingRecordedFrames);
	}

	[Fact]
	public void RepeatedStepsAndRunToBoundaryProduceEquivalentDeterministicSummaries()
	{
		GameplayStartupConfiguration startup = new(5, 10, 2.5, 9127);
		PlayerInputFrame[] frames =
		[
			new(0, new PlayerInput(true, 0, false, true)),
			new(1, new PlayerInput(false, 0.5f, true, false)),
			new(2, new PlayerInput(false, 0, false, false, ZapPressed: true))
		];
		GameplayPlaybackSession stepped = CreateSession(startup, 6, frames);
		GameplayPlaybackSession runTo = CreateSession(startup, 6, frames);

		for (int boundary = 1; boundary <= 6; boundary++)
		{
			Assert.True(stepped.StepForward());
			GameplayStateSummary steppedSummary = stepped.GetStateSummary();
			GameplayStateSummary runToSummary = runTo.RunToBoundary(boundary);

			AssertEquivalentSummary(steppedSummary, runToSummary);
		}
	}

	[Fact]
	public void PlaybackBoundaryStateIsIndependentOfInteractiveSpeedSelection()
	{
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", "0.5"], out InteractiveSpeedOptions slowOptions, out _));
		Assert.True(DebugController.TryParseInteractiveSpeedOptions(
			["--speed", "2"], out InteractiveSpeedOptions fastOptions, out _));
		Assert.NotEqual(slowOptions.TargetFramesPerSecond, fastOptions.TargetFramesPerSecond);

		GameplayStartupConfiguration startup = new(1, 3, 1.0, 4225);
		PlayerInputFrame[] frames =
		[
			new(0, new PlayerInput(true, 0, false, true)),
			new(20, new PlayerInput(false, 0.5f, true, false)),
			new(45, new PlayerInput(false, 0, false, false, ZapPressed: true))
		];
		GameplayPlaybackSession slow = CreateSession(startup, 90, frames);
		GameplayPlaybackSession fast = CreateSession(startup, 90, frames);

		AssertEquivalentSummary(slow.RunToBoundary(60), fast.RunToBoundary(60));
	}

	[Fact]
	public void RecordingCompletionPreventsAnyAdditionalForwardStep()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 2);

		Assert.Equal(2, session.RunToBoundary(2).Boundary);
		GameplayStateSummary complete = session.GetStateSummary();
		Assert.Equal(2, session.CurrentBoundary);
		Assert.Equal(2, session.NextRecordingFrame);
		Assert.Equal(0, session.RemainingRecordedFrames);
		Assert.True(session.IsRecordingComplete);

		Assert.False(session.StepForward());

		Assert.Equal(2, session.CurrentBoundary);
		AssertEquivalentSummary(complete, session.GetStateSummary());
	}

	[Fact]
	public void InvalidAndBackwardTargetsAreRejectedWithoutClamping()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 2);

		Assert.Throws<ArgumentOutOfRangeException>(() => session.RunToBoundary(-1));
		Assert.Throws<ArgumentOutOfRangeException>(() => session.RunToBoundary(3));
		Assert.Throws<ArgumentOutOfRangeException>(() => session.ReconstructToBoundary(-1));
		Assert.Throws<ArgumentOutOfRangeException>(() => session.ReconstructToBoundary(3));
		Assert.True(session.StepForward());
		Assert.Throws<InvalidOperationException>(() => session.RunToBoundary(0));
		Assert.Equal(1, session.CurrentBoundary);
	}

	[Fact]
	public void PlaybackRestartFrameIsAppliedOnlyAtGameOver()
	{
		GameplayStartupConfiguration startup = new(5, 10, 2.5, 1981);
		GameplayPlaybackSession session = CreateSession(startup, 1,
			new PlayerInputFrame(0, new PlayerInput(true, 1, true, false),
				RestartPressed: true));
		session.Game.CurrentLevel.State = LevelState.GameOver;

		Assert.True(session.StepForward());

		Assert.Same(LevelConfiguration.Level1, session.Game.CurrentLevel.Configuration);
		Assert.Equal(startup.StartingLives, session.Game.Player.Lives);
		Assert.Equal(startup.RimSpeedMultiplier,
			session.Game.CurrentLevel.RimTurnSpeedMultiplier);
	}

	[Fact]
	public void StateSpecificExitGateSuppressesMovementButConsumesTheRecordedFrame()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 1,
			new PlayerInputFrame(0, new PlayerInput(false, 0, false, true)));
		session.Game.CurrentLevel.State = LevelState.ExitDelay;
		(int lane, float position) =
			(session.Game.Player.EdgeIndex, session.Game.Player.EdgePosition);

		Assert.True(session.StepForward());

		Assert.Equal((lane, position),
			(session.Game.Player.EdgeIndex, session.Game.Player.EdgePosition));
		Assert.Equal(LevelState.ExitDelay, session.Game.State);
		Assert.Equal(1, session.CurrentBoundary);
	}

	[Fact]
	public void SessionUsesNormalProductionProgressionAtSuccessfulExitBoundary()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 1);
		session.Game.CurrentLevel.State = LevelState.ExitZoom;
		session.Game.Player.AdvanceExit(Level.Depth - 12);

		Assert.True(session.StepForward());

		Assert.Same(LevelConfiguration.Level2Square, session.Game.CurrentLevel.Configuration);
		Assert.Equal(2, session.GetStateSummary().CurrentDisplayedLevel);
		Assert.Equal(LevelState.Approach, session.Game.State);
		Assert.Equal(1, session.CurrentBoundary);
	}

	[Fact]
	public void StateSummaryIsReadOnlyAndReadingItDoesNotAdvanceTheGame()
	{
		GameplayPlaybackSession session = CreateSession(
			new GameplayStartupConfiguration(1, 3, 1.0, 1981), 3);
		(int lane, float position, int score) = (
			session.Game.Player.EdgeIndex,
			session.Game.Player.EdgePosition,
			session.Game.Score.Score);

		GameplayStateSummary first = session.GetStateSummary();
		GameplayStateSummary second = session.GetStateSummary();
		IList<GameplayFlipperSummary> flippers =
			Assert.IsAssignableFrom<IList<GameplayFlipperSummary>>(first.Flippers);

		Assert.True(flippers.IsReadOnly);
		Assert.Throws<NotSupportedException>(() => flippers.Add(
			new GameplayFlipperSummary(-1, -1, false, 0, 0, FlipperState.Climbing,
				0, null, 0, 0, 0, 0, 0)));
		AssertEquivalentSummary(first, second);
		Assert.Equal(0, session.CurrentBoundary);
		Assert.Equal((lane, position, score), (
			session.Game.Player.EdgeIndex,
			session.Game.Player.EdgePosition,
			session.Game.Score.Score));
	}

	[Fact]
	public void FlipperSummariesCaptureStateAndUseStableIdentityThenLaneOrdering()
	{
		GameplayStartupConfiguration startup = new(5, 10, 1.0, 1981);
		GameplayPlaybackSession session = CreateSession(startup, 2500);

		while (session.Game.Flippers.Count < 2 && session.StepForward())
		{
		}

		Assert.True(session.Game.Flippers.Count >= 2);
		GameplayStateSummary summary = session.GetStateSummary();
		GameplayFlipperSummary[] expected = session.Game.Flippers
			.OrderBy(flipper => flipper.IsTankerCargo)
			.ThenBy(flipper => flipper.IsTankerCargo
				? flipper.CargoUnitId
				: flipper.OriginalSlotId)
			.ThenBy(flipper => flipper.EdgeIndex)
			.ThenBy(flipper => flipper.Height)
			.ThenBy(flipper => flipper.State)
			.ThenBy(flipper => flipper.Direction)
			.ThenBy(flipper => flipper.SourceEdge)
			.ThenBy(flipper => flipper.DestinationEdge)
			.ThenBy(flipper => flipper.TurnAngle)
			.ThenBy(flipper => flipper.TotalTurnAngle)
			.ThenBy(flipper => flipper.RimPursuitDirection)
			.Select(flipper => new GameplayFlipperSummary(
				flipper.OriginalSlotId,
				flipper.CargoUnitId,
				flipper.IsTankerCargo,
				flipper.EdgeIndex,
				flipper.Height,
				flipper.State,
				flipper.Direction,
				flipper.RimPursuitDirection,
				flipper.SourceEdge,
				flipper.DestinationEdge,
				flipper.TurnAngle,
				flipper.TotalTurnAngle,
				flipper.TotalTurnAngle > 0
					? Math.Clamp(flipper.TurnAngle / flipper.TotalTurnAngle, 0, 1)
					: 0))
			.ToArray();

		Assert.Equal(expected, summary.Flippers);
		Assert.Equal(session.Game.Flippers.Count, summary.ActiveFlipperCount);
		Assert.All(summary.Flippers, flipper =>
		{
			Assert.Contains(flipper.Direction, new[] { -1, 0, 1 });
			Assert.InRange(flipper.PivotProgress, 0, 1);
		});
	}

	private static GameplayPlaybackSession CreateSession(
		GameplayStartupConfiguration startup, int durationTicks,
		params PlayerInputFrame[] frames) =>
		new(startup, new PlayerInputReplay(startup.RandomSeed, durationTicks, frames,
			startup.ResolveStartingConfiguration()));

	private static void AssertEquivalentSummary(
		GameplayStateSummary expected, GameplayStateSummary actual)
	{
		Assert.Equal(expected.Boundary, actual.Boundary);
		Assert.Equal(expected.RecordingDurationTicks, actual.RecordingDurationTicks);
		Assert.Equal(expected.NextRecordingFrame, actual.NextRecordingFrame);
		Assert.Equal(expected.RemainingRecordedFrames, actual.RemainingRecordedFrames);
		Assert.Equal(expected.IsRecordingComplete, actual.IsRecordingComplete);
		Assert.Equal(expected.StartingDisplayedLevel, actual.StartingDisplayedLevel);
		Assert.Equal(expected.StartingLives, actual.StartingLives);
		Assert.Equal(expected.StartingRimSpeedMultiplier, actual.StartingRimSpeedMultiplier);
		Assert.Equal(expected.RandomSeed, actual.RandomSeed);
		Assert.Equal(expected.CurrentDisplayedLevel, actual.CurrentDisplayedLevel);
		Assert.Equal(expected.LevelState, actual.LevelState);
		Assert.Equal(expected.Score, actual.Score);
		Assert.Equal(expected.RemainingLives, actual.RemainingLives);
		Assert.Equal(expected.PlayerLane, actual.PlayerLane);
		Assert.Equal(expected.PlayerLanePosition, actual.PlayerLanePosition);
		Assert.Equal(expected.PlayerDepth, actual.PlayerDepth);
		Assert.Equal(expected.SuperZapperChargesRemaining, actual.SuperZapperChargesRemaining);
		Assert.Equal(expected.PlayerShotCount, actual.PlayerShotCount);
		Assert.Equal(expected.EnemyBulletCount, actual.EnemyBulletCount);
		Assert.Equal(expected.ActiveEggCount, actual.ActiveEggCount);
		Assert.Equal(expected.ActiveFlipperCount, actual.ActiveFlipperCount);
		Assert.Equal(expected.ActiveTankerCount, actual.ActiveTankerCount);
		Assert.Equal(expected.ActiveSpikerCount, actual.ActiveSpikerCount);
		Assert.Equal(expected.ActiveSpikeCount, actual.ActiveSpikeCount);
		Assert.Equal(expected.ActiveFuseballCount, actual.ActiveFuseballCount);
		Assert.Equal(expected.ActivePulsarCount, actual.ActivePulsarCount);
		Assert.Equal(expected.Flippers, actual.Flippers);
	}

	private static void AssertEquivalentGameplayState(TempestGame expected, TempestGame actual)
	{
		Assert.Same(expected.CurrentLevel.Configuration, actual.CurrentLevel.Configuration);
		Assert.Equal(expected.State, actual.State);
		Assert.Equal(expected.Player.EdgeIndex, actual.Player.EdgeIndex);
		Assert.Equal(expected.Player.EdgePosition, actual.Player.EdgePosition);
		Assert.Equal(expected.Player.Depth, actual.Player.Depth);
		Assert.Equal(expected.Player.Lives, actual.Player.Lives);
		Assert.Equal(expected.Score.Score, actual.Score.Score);
		Assert.Equal(expected.SuperZapperChargesRemaining, actual.SuperZapperChargesRemaining);
		Assert.Equal(expected.Shots.Select(shot => (shot.EdgeIndex, shot.Height)),
			actual.Shots.Select(shot => (shot.EdgeIndex, shot.Height)));
		Assert.Equal(expected.Flippers.Select(flipper =>
				(flipper.EdgeIndex, flipper.Height, flipper.State, flipper.Direction)),
			actual.Flippers.Select(flipper =>
				(flipper.EdgeIndex, flipper.Height, flipper.State, flipper.Direction)));
	}
}
