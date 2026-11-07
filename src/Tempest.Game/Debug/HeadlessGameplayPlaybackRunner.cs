using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public enum GameplayPlaybackCompletionReason
{
	RequestedBoundaryReached,
	RecordingCompleted
}

public sealed record GameplayPlaybackRunResult(
	int? RequestedBoundary,
	int AchievedBoundary,
	int RecordingDurationTicks,
	int RemainingFrames,
	GameplayPlaybackCompletionReason CompletionReason,
	GameplayStateSummary StateSummary)
{
	public bool IsRecordingComplete =>
		CompletionReason == GameplayPlaybackCompletionReason.RecordingCompleted;
}

/// <summary>
/// Runs a validated recording through the same production session used by visible playback,
/// without exposing mutable game entities or initializing presentation systems.
/// </summary>
public sealed class HeadlessGameplayPlaybackRunner
{
	private readonly GameplayPlaybackSession session;

	public HeadlessGameplayPlaybackRunner(GameplayRecording recording)
	{
		ArgumentNullException.ThrowIfNull(recording);
		// The production simulation still emits semantic audio events; headless playback discards them.
		session = new GameplayPlaybackSession(recording.StartupConfiguration,
			recording.Replay, SilentAudioEvents.Instance);
	}

	public HeadlessGameplayPlaybackRunner(string recordingPath)
		: this(GameplayRecordingFile.LoadRecording(recordingPath))
	{
	}

	public GameplayPlaybackRunResult RunToCompletion(
		Action<int, GameplayStateSummary>? boundaryObserver = null) =>
		RunToTarget(null, session.RecordingDurationTicks, boundaryObserver);

	public GameplayPlaybackRunResult RunToBoundary(
		int targetBoundary,
		Action<int, GameplayStateSummary>? boundaryObserver = null) =>
		RunToTarget(targetBoundary, targetBoundary, boundaryObserver);

	private GameplayPlaybackRunResult RunToTarget(
		int? requestedBoundary,
		int targetBoundary,
		Action<int, GameplayStateSummary>? boundaryObserver)
	{
		if (targetBoundary < 0 || targetBoundary > session.RecordingDurationTicks)
			throw new ArgumentOutOfRangeException(nameof(targetBoundary),
				$"The target boundary must be between 0 and {session.RecordingDurationTicks}.");
		if (targetBoundary < session.CurrentBoundary)
			throw new InvalidOperationException(
				"Backward playback is not supported; the target precedes the current boundary.");

		ObserveCurrentBoundary(boundaryObserver);
		while (session.CurrentBoundary < targetBoundary)
		{
			if (!session.StepForward())
				throw new InvalidOperationException(
					"The playback session completed before reaching its validated target boundary.");

			ObserveCurrentBoundary(boundaryObserver);
		}

		GameplayStateSummary summary = session.GetStateSummary();
		GameplayPlaybackCompletionReason reason = session.IsRecordingComplete
			? GameplayPlaybackCompletionReason.RecordingCompleted
			: GameplayPlaybackCompletionReason.RequestedBoundaryReached;
		return new GameplayPlaybackRunResult(
			requestedBoundary,
			session.CurrentBoundary,
			session.RecordingDurationTicks,
			session.RemainingRecordedFrames,
			reason,
			summary);
	}

	private void ObserveCurrentBoundary(
		Action<int, GameplayStateSummary>? boundaryObserver)
	{
		if (boundaryObserver is null)
			return;

		GameplayStateSummary summary = session.GetStateSummary();
		boundaryObserver(session.CurrentBoundary, summary);
	}
}
