using Tempest.Game.Systems;

namespace Tempest.Game.Debug;

/// <summary>
/// Executes one validated production recording against the normal headless game simulation.
/// Boundary 0 is the restored startup state; consuming frame N produces boundary N+1.
/// </summary>
public sealed class GameplayPlaybackSession
{
	private readonly PlayerInputReplay replay;
	private IAudioEvents audioEvents;

	public GameplayStartupConfiguration StartupConfiguration { get; }
	public TempestGame Game { get; }
	public int CurrentBoundary { get; private set; }
	public int RecordingDurationTicks => replay.DurationTicks;
	public int NextRecordingFrame => CurrentBoundary;
	public int RemainingRecordedFrames => RecordingDurationTicks - CurrentBoundary;
	public bool IsRecordingComplete => CurrentBoundary == RecordingDurationTicks;

	public GameplayPlaybackSession(
		GameplayStartupConfiguration startupConfiguration,
		PlayerInputReplay replay,
		IAudioEvents? audioEvents = null)
	{
		StartupConfiguration = startupConfiguration ??
			throw new ArgumentNullException(nameof(startupConfiguration));
		this.replay = replay ?? throw new ArgumentNullException(nameof(replay));

		if (replay.RandomSeed != startupConfiguration.RandomSeed)
			throw new ArgumentException(
				"The replay seed must match its startup configuration.", nameof(replay));
		if (!ReferenceEquals(replay.StartingConfiguration,
			startupConfiguration.ResolveStartingConfiguration()))
			throw new ArgumentException(
				"The replay production level must match its startup configuration.", nameof(replay));

		this.audioEvents = audioEvents ?? SilentAudioEvents.Instance;
		Game = new TempestGame(startupConfiguration, this.audioEvents);
	}

	public bool StepForward()
	{
		if (IsRecordingComplete)
			return false;

		PlayerInputFrame frame = replay.GetFrame(CurrentBoundary);
		(PlayerInput input, bool restartPressed) = ApplyPlaybackInputGates(Game, frame);
		Game.RunOneStep(input, restartPressed);
		CurrentBoundary++;
		return true;
	}

	public GameplayStateSummary RunToBoundary(int targetBoundary)
	{
		if (targetBoundary < 0 || targetBoundary > RecordingDurationTicks)
			throw new ArgumentOutOfRangeException(nameof(targetBoundary),
				$"The target boundary must be between 0 and {RecordingDurationTicks}.");
		if (targetBoundary < CurrentBoundary)
			throw new InvalidOperationException(
				"Backward playback is not supported; the target precedes the current boundary.");

		while (CurrentBoundary < targetBoundary)
		{
			if (!StepForward())
				throw new InvalidOperationException(
					"The playback session completed before reaching its validated target boundary.");
		}

		return GetStateSummary();
	}

	/// <summary>
	/// Restores recorded startup and deterministically replays to a boundary without emitting
	/// historical audio; the returned session resumes through this session's visible audio sink.
	/// </summary>
	public GameplayPlaybackSession ReconstructToBoundary(int targetBoundary)
	{
		if (targetBoundary < 0 || targetBoundary > RecordingDurationTicks)
			throw new ArgumentOutOfRangeException(nameof(targetBoundary),
				$"The target boundary must be between 0 and {RecordingDurationTicks}.");

		GameplayPlaybackSession reconstructed = new(
			StartupConfiguration, replay, SilentAudioEvents.Instance);
		reconstructed.RunToBoundary(targetBoundary);
		// Retain the visible sink on the replacement session for subsequent reconstructions.
		reconstructed.audioEvents = audioEvents;
		reconstructed.Game.SetAudioEvents(audioEvents);
		return reconstructed;
	}

	public GameplayStateSummary GetStateSummary() =>
		GameplayStateSummary.Capture(this, Game);

	// Program uses this same seam so file/profile playback keeps identical gates.
	internal static (PlayerInput Input, bool RestartPressed) ApplyPlaybackInputGates(
		TempestGame game, PlayerInputFrame frame)
	{
		ArgumentNullException.ThrowIfNull(game);

		if (game.GameOver || game.LevelComplete)
			return (default, frame.RestartPressed);

		if (game.DeathTicks != 0 ||
			game.IsExiting && !game.ExitMovementInputEnabled)
			return (default, false);

		return (frame.Input, false);
	}
}
