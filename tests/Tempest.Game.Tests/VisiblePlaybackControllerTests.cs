using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class VisiblePlaybackControllerTests
{
	[Fact]
	public void ControllerStartsRunningAndUsesTheSessionForEachVisibleFrame()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame frame = new(0, new PlayerInput(
			true, 0.5f, false, true, ZapPressed: true, ControllerAxis: 0.25f));
		VisiblePlaybackController controller = CreateController(startup, 2, frame);
		TempestGame expected = new(startup);
		expected.RunOneStep(frame.Input, frame.RestartPressed);

		Assert.False(controller.IsPaused);
		Assert.True(controller.IsActive);
		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(
			PauseTogglePressed: false, ForwardNavigationPressed: true, ShiftDown: true)));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		AssertEquivalentGameplayState(expected, controller.Session.Game);
	}

	[Fact]
	public void PausePressDoesNotRunAStepAndHeldSpaceDoesNotRetoggle()
	{
		RecordingAudioEvents audio = new();
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		VisiblePlaybackController controller = CreateController(startup, 4, audio);
		controller.Session.Game.RunOneStep(new PlayerInput(
			false, 0, false, false, ZapPressed: true));
		GameplayStateSummary before = controller.Session.GetStateSummary();
		int flashTicks = controller.Session.Game.SuperZapper.FlashTicksRemaining;
		audio.Events.Clear();

		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.True(controller.IsPaused);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(false, false, false)));
		Assert.True(controller.IsPaused);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(false, false, false)));
		Assert.Equal(0, controller.Session.CurrentBoundary);
		Assert.Equal(flashTicks, controller.Session.Game.SuperZapper.FlashTicksRemaining);
		Assert.Empty(audio.Events);
		AssertEquivalentSummary(before, controller.Session.GetStateSummary());
	}

	[Fact]
	public void ResumeUsesTheExactNextRecordedFrameAndSpaceIsNotGameplayInput()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame nextFrame = new(1,
			new PlayerInput(false, 0, false, true, ZapPressed: false));
		VisiblePlaybackController controller = CreateController(startup, 3,
			new PlayerInputFrame(0, default), nextFrame);

		Assert.Equal(1, controller.ProcessControls(default));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.Equal(0, controller.ProcessControls(default));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(2, controller.Session.Game.SuperZapperChargesRemaining);

		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.False(controller.IsPaused);
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(2, controller.Session.Game.SuperZapperChargesRemaining);
		Assert.Equal(1, controller.ProcessControls(default));
		Assert.Equal(2, controller.Session.CurrentBoundary);
		Assert.Equal(2, controller.Session.Game.SuperZapperChargesRemaining);

		TempestGame expected = new(startup);
		expected.RunOneStep(default);
		expected.RunOneStep(nextFrame.Input);
		AssertEquivalentGameplayState(expected, controller.Session.Game);
	}

	[Fact]
	public void RightArrowStepsExactlyOneTickOnlyWhilePaused()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame[] frames =
		[
			new(0, new PlayerInput(false, 0, false, true)),
			new(1, new PlayerInput(true, 0, false, false)),
			new(2, default)
		];
		VisiblePlaybackController controller = CreateController(startup, 3, frames);

		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.True(controller.IsPaused);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(false, false, false)));
		Assert.Equal(1, controller.Session.CurrentBoundary);

		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(2, controller.Session.CurrentBoundary);
		Assert.Equal(0, controller.ProcessControls(default));
		Assert.Equal(2, controller.Session.CurrentBoundary);
	}

	[Fact]
	public void RepeatedRightEventAdvancesOneMoreTickButNoEventDoesNotRepeatEveryFrame()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame[] frames =
		[
			new(0, default),
			new(1, default),
			new(2, default),
			new(3, default)
		];
		VisiblePlaybackController controller = CreateController(startup, 4, frames);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));

		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.Equal(0, controller.ProcessControls(default));
		Assert.Equal(0, controller.ProcessControls(default));
		Assert.Equal(1, controller.Session.CurrentBoundary);

		// IsKeyPressedRepeat is reduced to the same single forward-navigation event.
		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(2, controller.Session.CurrentBoundary);
	}

	[Fact]
	public void ShiftRightRunsFiveOrdinarySequentialStepsAndAnotherRepeatRunsFiveMore()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame[] frames = Enumerable.Range(0, 12)
			.Select(tick => new PlayerInputFrame(tick, new PlayerInput(
				Fire: tick % 2 == 0,
				MouseDeltaX: tick / 10f,
				Left: false,
				Right: tick % 3 == 0,
				ZapPressed: false)))
			.ToArray();
		VisiblePlaybackController controller = CreateController(startup, frames.Length, frames);
		TempestGame expected = new(startup);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));

		Assert.Equal(5, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		for (int tick = 0; tick < 5; tick++)
			expected.RunOneStep(frames[tick].Input, frames[tick].RestartPressed);
		Assert.Equal(5, controller.Session.CurrentBoundary);
		AssertEquivalentGameplayState(expected, controller.Session.Game);

		Assert.Equal(5, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		for (int tick = 5; tick < 10; tick++)
			expected.RunOneStep(frames[tick].Input, frames[tick].RestartPressed);
		Assert.Equal(10, controller.Session.CurrentBoundary);
		AssertEquivalentGameplayState(expected, controller.Session.Game);
	}

	[Fact]
	public void ShiftRightStopsAtDurationWhenFewerThanFiveFramesRemain()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame[] frames = Enumerable.Range(0, 7)
			.Select(tick => new PlayerInputFrame(tick, default))
			.ToArray();
		VisiblePlaybackController controller = CreateController(startup, frames.Length, frames);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));

		Assert.Equal(5, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		Assert.Equal(2, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		Assert.Equal(frames.Length, controller.Session.CurrentBoundary);
		Assert.True(controller.Session.IsRecordingComplete);
		Assert.True(controller.IsPaused);
		Assert.True(controller.IsActive);
		Assert.Equal(0, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		Assert.Equal(frames.Length, controller.Session.CurrentBoundary);
	}

	[Fact]
	public void ShiftWithoutForwardEventIsInertAndPausedOverlayTracksOneAndFiveTickOperations()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		PlayerInputFrame[] frames = Enumerable.Range(0, 8)
			.Select(tick => new PlayerInputFrame(tick, default))
			.ToArray();
		VisiblePlaybackController controller = CreateController(startup, frames.Length, frames);
		VectorRenderer renderer = new(controller.Session.Game);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));

		Assert.Equal(0, controller.ProcessControls(
			new PlaybackControlInput(false, false, true)));
		Assert.Equal(0, controller.Session.CurrentBoundary);
		Assert.Equal(1, controller.ProcessControls(
			new PlaybackControlInput(false, true, false)));
		AssertPausedOverlayMatches(controller, renderer);

		Assert.Equal(5, controller.ProcessControls(
			new PlaybackControlInput(false, true, true)));
		AssertPausedOverlayMatches(controller, renderer);
		Assert.Equal(6, controller.Session.CurrentBoundary);
	}

	[Fact]
	public void PausedStepPreservesRestartBitAndFinalBoundaryUntilExplicitResume()
	{
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		VisiblePlaybackController controller = CreateController(startup, 1,
			new PlayerInputFrame(0, default, RestartPressed: true));
		controller.Session.Game.CurrentLevel.State = LevelState.GameOver;
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));

		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.True(controller.Session.IsRecordingComplete);
		Assert.Equal(LevelState.Playing, controller.Session.Game.State);
		Assert.True(controller.IsPaused);
		Assert.True(controller.IsActive);
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(1, controller.Session.CurrentBoundary);
		Assert.True(controller.TryMarkCompletionReported());
		Assert.False(controller.TryMarkCompletionReported());

		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.False(controller.IsPaused);
		Assert.False(controller.IsActive);
		Assert.Equal(1, controller.Session.CurrentBoundary);

		controller.Session.Game.RunOneStep(new PlayerInput(
			false, 0, false, false, ZapPressed: true));
		Assert.Equal(1, controller.Session.Game.SuperZapperChargesRemaining);
		(int lane, float position) = (
			controller.Session.Game.Player.EdgeIndex,
			controller.Session.Game.Player.EdgePosition);
		controller.Session.Game.RunOneStep(new PlayerInput(
			false, 0, false, true));
		Assert.NotEqual((lane, position), (
			controller.Session.Game.Player.EdgeIndex,
			controller.Session.Game.Player.EdgePosition));
	}

	[Fact]
	public void APausedStepEmitsOnlyThatFramesAudioAndOverlayUsesSharedUppercaseVectorFont()
	{
		RecordingAudioEvents audio = new();
		GameplayStartupConfiguration startup = new(1, 3, 1.0, 1981);
		VisiblePlaybackController controller = CreateController(startup, 2, audio,
			new PlayerInputFrame(0, new PlayerInput(true, 0, false, false)));
		Assert.Equal(0, controller.ProcessControls(new PlaybackControlInput(true, false, false)));
		Assert.Empty(audio.Events);
		Assert.Equal(1, controller.ProcessControls(new PlaybackControlInput(false, true, false)));
		Assert.Equal(new[] { AudioEvent.PlayerShot }, audio.Events);

		VectorRenderer renderer = new(controller.Session.Game);
		List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End)> actual = [];
		List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End)> expected = [];
		renderer.DrawPlaybackPausedStatus(4820, 7260,
			(start, end) => actual.Add((start, end)));
		VectorFont.DrawCenteredText("PAUSED  4820 / 7260",
			new System.Numerics.Vector2(400, 570), 2,
			(start, end) => expected.Add((start, end)));

		Assert.Equal(expected, actual);
		Assert.NotEmpty(actual);
		Assert.Equal(new System.Numerics.Vector2(400, 570),
			BoundsCenter(actual.SelectMany(line => new[] { line.Start, line.End })));
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			renderer.DrawPlaybackPausedStatus(3, 2, (_, _) => { }));

		// Redrawing the same paused boundary is presentation-only and emits no audio.
		renderer.DrawPlaybackPausedStatus(1, 2, (_, _) => { });
		Assert.Equal(new[] { AudioEvent.PlayerShot }, audio.Events);
	}

	private static VisiblePlaybackController CreateController(
		GameplayStartupConfiguration startup, int durationTicks,
		params PlayerInputFrame[] frames) =>
		CreateController(startup, durationTicks, SilentAudioEvents.Instance, frames);

	private static VisiblePlaybackController CreateController(
		GameplayStartupConfiguration startup, int durationTicks,
		IAudioEvents audioEvents, params PlayerInputFrame[] frames)
	{
		PlayerInputReplay replay = new(startup.RandomSeed, durationTicks, frames,
			startup.ResolveStartingConfiguration());
		return new VisiblePlaybackController(
			new GameplayPlaybackSession(startup, replay, audioEvents));
	}

	private static System.Numerics.Vector2 BoundsCenter(
		IEnumerable<System.Numerics.Vector2> points)
	{
		System.Numerics.Vector2[] values = points.ToArray();
		return new System.Numerics.Vector2(
			(values.Min(point => point.X) + values.Max(point => point.X)) / 2,
			(values.Min(point => point.Y) + values.Max(point => point.Y)) / 2);
	}

	private static void AssertPausedOverlayMatches(
		VisiblePlaybackController controller, VectorRenderer renderer)
	{
		List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End)> actual = [];
		List<(System.Numerics.Vector2 Start, System.Numerics.Vector2 End)> expected = [];
		int boundary = controller.Session.CurrentBoundary;
		int duration = controller.Session.RecordingDurationTicks;
		string text = FormattableString.Invariant($"PAUSED  {boundary} / {duration}");
		renderer.DrawPlaybackPausedStatus(boundary, duration,
			(start, end) => actual.Add((start, end)));
		VectorFont.DrawCenteredText(text, new System.Numerics.Vector2(400, 570), 2,
			(start, end) => expected.Add((start, end)));
		Assert.Equal(expected, actual);
	}

	private static void AssertEquivalentSummary(
		GameplayStateSummary expected, GameplayStateSummary actual)
	{
		Assert.Equal(expected.Boundary, actual.Boundary);
		Assert.Equal(expected.Score, actual.Score);
		Assert.Equal(expected.RemainingLives, actual.RemainingLives);
		Assert.Equal(expected.PlayerLane, actual.PlayerLane);
		Assert.Equal(expected.PlayerLanePosition, actual.PlayerLanePosition);
		Assert.Equal(expected.PlayerDepth, actual.PlayerDepth);
		Assert.Equal(expected.SuperZapperChargesRemaining, actual.SuperZapperChargesRemaining);
		Assert.Equal(expected.PlayerShotCount, actual.PlayerShotCount);
		Assert.Equal(expected.ActiveEggCount, actual.ActiveEggCount);
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

	private sealed class RecordingAudioEvents : IAudioEvents
	{
		public List<AudioEvent> Events { get; } = [];

		public void Play(AudioEvent audioEvent) => Events.Add(audioEvent);
	}
}
