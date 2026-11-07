using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

public sealed class VisiblePlaybackController
{
	private bool completionReported;

	public GameplayPlaybackSession Session { get; }
	public bool IsPaused { get; private set; }
	// A paused final boundary remains under debugger control until Neal resumes to live input.
	public bool IsActive => !Session.IsRecordingComplete || IsPaused;

	public VisiblePlaybackController(GameplayPlaybackSession session)
	{
		Session = session ?? throw new ArgumentNullException(nameof(session));
	}

	public int ProcessControls(PlaybackControlInput controls)
	{
		if (!IsActive)
			return 0;

		if (controls.PauseTogglePressed)
		{
			IsPaused = !IsPaused;
			return 0;
		}

		if (!IsPaused)
			return Session.StepForward() ? 1 : 0;

		if (!controls.ForwardNavigationPressed)
			return 0;

		int requestedSteps = controls.ShiftDown ? 5 : 1;
		int stepsAdvanced = 0;
		// A group still consumes recorded frames through ordinary sequential simulation ticks.
		while (stepsAdvanced < requestedSteps && Session.StepForward())
			stepsAdvanced++;

		return stepsAdvanced;
	}

	public bool TryMarkCompletionReported()
	{
		if (!Session.IsRecordingComplete || completionReported)
			return false;

		completionReported = true;
		return true;
	}
}
