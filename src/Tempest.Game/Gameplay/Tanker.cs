using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum TankerUpdateResult
{
    Active,
    NaturalCargoRelease
}

public enum TankerReleaseKind
{
    Natural,
    PlayerShotDestruction
}

public readonly record struct TankerReleaseResult(
    TankerReleaseKind Kind, IReadOnlyList<TankerCargoSpawn> Cargo);

public enum TankerState
{
    Climbing,
    CargoReleasePending,
    CargoReleased
}

public readonly record struct TankerCargoSpawn(
    int SourceLane, int DestinationLane, int Direction, float Height);

/// <summary>
/// Development-only (<c>--tanker-test</c>) Tanker. Climbs at 1.85 (Incitti-derived, unvalidated) and
/// carries two latent Flipper cargo units. Release happens once: naturally when height passes
/// <see cref="CargoReleaseThreshold"/> (no score) or when shot (200 points, awarded by TempestGame).
/// The Tanker never harms the player directly. OriginalSlotId is the enemy slot it represents;
/// cargo identity is tracked separately by TempestGame.
/// </summary>
public sealed class Tanker : Enemy
{
    public const float DevelopmentClimbSpeed = 1.85f;
    public const int CarriedFlipperCount = 2;
    public const float CargoReleaseThreshold = Level.Depth - 15;

    public Tanker(int edgeIndex, int originalSlotId, float climbSpeed = DevelopmentClimbSpeed)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(climbSpeed);
        EdgeIndex = edgeIndex;
        OriginalSlotId = originalSlotId;
        ClimbSpeed = climbSpeed;
        Height = 10;
        State = TankerState.Climbing;
    }

    public int OriginalSlotId { get; }
    public float ClimbSpeed { get; }
    public int LatentCargoCount { get; private set; } = CarriedFlipperCount;
    public bool CargoReleased { get; private set; }
    public TankerState State { get; private set; }

    public TankerUpdateResult UpdateForDevelopment()
    {
        if (State == TankerState.CargoReleased)
            return TankerUpdateResult.Active;
        if (State == TankerState.CargoReleasePending)
            return TankerUpdateResult.NaturalCargoRelease;

        Height += ClimbSpeed;
        if (Height > CargoReleaseThreshold)
        {
            State = TankerState.CargoReleasePending;
            return TankerUpdateResult.NaturalCargoRelease;
        }

        return TankerUpdateResult.Active;
    }

    // A shot hits only from below the Tanker's current height; there is no upper window.
    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public bool HitsPlayer(Player player) => false;

    // Both cargo Flippers appear on the lanes adjacent to the Tanker, at the Tanker's height, and
    // pivot in opposite directions. Same-tick policy: the caller adds them after that tick's
    // Flipper update, so they first move on the next tick. Fails closed if already released.
    public TankerReleaseResult ReleaseFlipperCargo(
        Level level, int firstSideDirection, TankerReleaseKind kind)
    {
        if (LatentCargoCount != CarriedFlipperCount || CargoReleased)
            return new TankerReleaseResult(kind, []);

        int side = firstSideDirection < 0 ? -1 : 1;
        int firstLane = side > 0 ? level.PreviousEdge(EdgeIndex) : level.NextEdge(EdgeIndex);
        int secondLane = side > 0 ? level.NextEdge(EdgeIndex) : level.PreviousEdge(EdgeIndex);
        LatentCargoCount = 0;
        CargoReleased = true;
        State = TankerState.CargoReleased;

        TankerCargoSpawn[] cargo =
        [
            // Incitti's ReleaseCargo steps each child to the next outward lane, so the pair separates instead of converging on the carrier lane.
            new TankerCargoSpawn(firstLane, side > 0 ? level.PreviousEdge(firstLane) : level.NextEdge(firstLane), side, Height),
            new TankerCargoSpawn(secondLane, side > 0 ? level.NextEdge(secondLane) : level.PreviousEdge(secondLane), -side, Height)
        ];
        return new TankerReleaseResult(kind, cargo);
    }

    public override void Update() { }
}
