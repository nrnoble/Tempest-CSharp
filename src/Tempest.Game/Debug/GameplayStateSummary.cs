using Tempest.Game.Gameplay;
using Tempest.Game.Levels;

namespace Tempest.Game.Debug;

public sealed record GameplayFlipperSummary(
	int OriginalSlotId,
	int CargoUnitId,
	bool IsTankerCargo,
	int Lane,
	float Height,
	FlipperState MovementState,
	int Direction,
	int? RimPursuitDirection,
	int PivotSourceLane,
	int PivotDestinationLane,
	float PivotAngleDegrees,
	float PivotTotalAngleDegrees,
	float PivotProgress);

/// <summary>
/// Immutable, intentionally bounded diagnostic values for one playback boundary.
/// </summary>
public sealed class GameplayStateSummary
{
	public int Boundary { get; }
	public int RecordingDurationTicks { get; }
	public int NextRecordingFrame { get; }
	public int RemainingRecordedFrames { get; }
	public bool IsRecordingComplete { get; }
	public int StartingDisplayedLevel { get; }
	public int StartingLives { get; }
	public double StartingRimSpeedMultiplier { get; }
	public int RandomSeed { get; }
	public int CurrentDisplayedLevel { get; }
	public LevelState LevelState { get; }
	public int Score { get; }
	public int RemainingLives { get; }
	public int PlayerLane { get; }
	public float PlayerLanePosition { get; }
	public float PlayerDepth { get; }
	public int SuperZapperChargesRemaining { get; }
	public int PlayerShotCount { get; }
	public int EnemyBulletCount { get; }
	public int ActiveEggCount { get; }
	public int ActiveFlipperCount { get; }
	public int ActiveTankerCount { get; }
	public int ActiveSpikerCount { get; }
	public int ActiveSpikeCount { get; }
	public int ActiveFuseballCount { get; }
	public int ActivePulsarCount { get; }
	public IReadOnlyList<GameplayFlipperSummary> Flippers { get; }

	private GameplayStateSummary(
		int boundary,
		int recordingDurationTicks,
		int nextRecordingFrame,
		int remainingRecordedFrames,
		bool isRecordingComplete,
		GameplayStartupConfiguration startupConfiguration,
		TempestGame game,
		IEnumerable<GameplayFlipperSummary> flippers)
	{
		Boundary = boundary;
		RecordingDurationTicks = recordingDurationTicks;
		NextRecordingFrame = nextRecordingFrame;
		RemainingRecordedFrames = remainingRecordedFrames;
		IsRecordingComplete = isRecordingComplete;
		StartingDisplayedLevel = startupConfiguration.StartingDisplayedLevel;
		StartingLives = startupConfiguration.StartingLives;
		StartingRimSpeedMultiplier = startupConfiguration.RimSpeedMultiplier;
		RandomSeed = startupConfiguration.RandomSeed;
		CurrentDisplayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
		LevelState = game.State;
		Score = game.Score.Score;
		RemainingLives = game.Player.Lives;
		PlayerLane = game.Player.EdgeIndex;
		PlayerLanePosition = game.Player.EdgePosition;
		PlayerDepth = game.Player.Depth;
		SuperZapperChargesRemaining = game.SuperZapperChargesRemaining;
		PlayerShotCount = game.Shots.Count;
		EnemyBulletCount = game.EnemyBullets.Count;
		ActiveEggCount = game.Eggs.Count;
		ActiveFlipperCount = game.Flippers.Count;
		ActiveTankerCount = game.Tankers.Count;
		ActiveSpikerCount = game.Spikers.Count;
		ActiveSpikeCount = game.Spikes.Count;
		ActiveFuseballCount = game.Fuseballs.Count;
		ActivePulsarCount = game.Pulsars.Count;
		Flippers = Array.AsReadOnly(flippers.ToArray());
	}

	internal static GameplayStateSummary Capture(
		GameplayPlaybackSession session, TempestGame game)
	{
		GameplayFlipperSummary[] flippers = game.Flippers
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

		return new GameplayStateSummary(
			session.CurrentBoundary,
			session.RecordingDurationTicks,
			session.NextRecordingFrame,
			session.RemainingRecordedFrames,
			session.IsRecordingComplete,
			session.StartupConfiguration,
			game,
			flippers);
	}
}
