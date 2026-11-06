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
    PlayerShotDestruction,
    SuperZapperDestruction
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
/// A Tanker used by the isolated <c>--tanker-test</c> scenario and the headless Level 3 mixed
/// model. Climbs at 1.75 (reference-derived, not arcade-measured) and carries two latent Flipper
/// cargo units. Release happens once: naturally when height passes
/// <see cref="CargoReleaseThreshold"/> (no score), when shot (200 points), or when a zap expires
/// (200 points plus stillborn cargo), with scoring and accounting owned by TempestGame.
/// The Tanker never collides with the player directly, but when armed by production composition
/// or by combining the diagnostic with <c>--enemy-bullets-test</c>, it fires the shared
/// EnemyBullet using Incitti's Tanker rules. OriginalSlotId is the enemy slot it represents;
/// cargo identity is tracked separately.
/// </summary>
public sealed class Tanker : Enemy
{
    // Incitti's 1.75 + lv/20 uses integer division: displayed Level 3 (lv=2) climbs at 1.75.
    public const float DevelopmentClimbSpeed = 1.75f;
    public const int CarriedFlipperCount = 2;
    public const float CargoReleaseThreshold = Level.Depth - 15;
    public const int BulletTimerResetTicks = 16;
    // Incitti stops Tanker fire at height 380, safely below the 385 release threshold, so a
    // Tanker never shoots from point-blank rim range or during release processing.
    public const float BulletFiringCeiling = 380f;

    public Tanker(int edgeIndex, int originalSlotId, float climbSpeed = DevelopmentClimbSpeed,
        int bulletAmmunition = 0, int bulletTimer = BulletTimerResetTicks)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(climbSpeed);
        EdgeIndex = edgeIndex;
        OriginalSlotId = originalSlotId;
        ClimbSpeed = climbSpeed;
        Height = 10;
        State = TankerState.Climbing;
        BulletAmmunition = Math.Max(0, bulletAmmunition);
        BulletTimer = bulletTimer;
    }

    public int OriginalSlotId { get; }
    public float ClimbSpeed { get; }
    public int LatentCargoCount { get; private set; } = CarriedFlipperCount;
    public bool CargoReleased { get; private set; }
    public TankerState State { get; private set; }
    public int BulletAmmunition { get; private set; }
    public int BulletTimer { get; private set; }

    public TankerUpdateResult UpdateForDevelopment() => UpdateForDevelopment(null, out _);

    // Firing seam mirrors Flipper.Update: the caller supplies the inclusive random source only
    // when the enemy-bullets development option is active, and receives at most one bullet per
    // update to register. The Tanker itself never touches game collections.
    public TankerUpdateResult UpdateForDevelopment(
        Func<int, int, int>? randomInclusive, out EnemyBullet? bullet)
    {
        bullet = null;
        if (State == TankerState.CargoReleased)
            return TankerUpdateResult.Active;
        if (State == TankerState.CargoReleasePending)
            return TankerUpdateResult.NaturalCargoRelease;

        Height += ClimbSpeed;
        // Incitti climbs first and then gates firing on the post-climb height (`If height < 380`),
        // so the bullet origin is always the just-advanced height and exactly 380 is silent.
        if (Height < BulletFiringCeiling)
            bullet = TryFireBullet(randomInclusive);

        if (Height > CargoReleaseThreshold)
        {
            State = TankerState.CargoReleasePending;
            return TankerUpdateResult.NaturalCargoRelease;
        }

        return TankerUpdateResult.Active;
    }

    // Incitti Tanker firing: the countdown only runs while ammunition remains, resets to 16 on
    // expiry, and Rand(0,100) > 75 decides the shot. Ammunition is consumed only on a created
    // bullet, so a failed roll keeps the shot available for the next 16-tick window (same
    // convention as Flipper.TryFireBullet).
    private EnemyBullet? TryFireBullet(Func<int, int, int>? randomInclusive)
    {
        if (BulletAmmunition <= 0 || randomInclusive is null)
            return null;

        BulletTimer--;
        if (BulletTimer >= 0)
            return null;

        BulletTimer = BulletTimerResetTicks;
        if (randomInclusive(0, 100) <= 75)
            return null;

        BulletAmmunition--;
        return new EnemyBullet(EdgeIndex, Height);
    }

    // A shot hits only from below the Tanker's current height; there is no upper window.
    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public bool UsesLevel7ProgrammedCargo(
        LevelConfiguration configuration, TankerReleaseKind releaseKind)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.BelowRimFlipperProgram ==
                BelowRimFlipperProgram.Level7ArcadeObserved &&
            releaseKind == TankerReleaseKind.PlayerShotDestruction &&
            Height <= LevelConfiguration.Level7DeepCargoMaximumHeight;
    }

    public bool UsesLevel5SpikeSensitiveCargo(
        LevelConfiguration configuration, TankerReleaseKind releaseKind)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        // Reuse the established C# deep-cargo cutoff; it approximates the ROM split, not its byte scale.
        return configuration.BelowRimFlipperProgram ==
                BelowRimFlipperProgram.Level5SpikeSensitive &&
            releaseKind == TankerReleaseKind.PlayerShotDestruction &&
            Height <= LevelConfiguration.Level7DeepCargoMaximumHeight;
    }

    public override bool TryZap(int delay) => AcceptZap(delay);

    public bool HitsPlayer(Player player) => false;

    // Cargo follows the project-selected outward lane route; its shared Flipper geometry still
    // pivots inward into the web. Same-tick policy: children first move on the next tick.
    // Fails closed if already released.
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

        int firstDirection = side;
        int secondDirection = -side;
        TankerCargoSpawn[] cargo =
        [
            // Lane routing away from the Tanker is independent of the inward spatial pivot.
            new TankerCargoSpawn(firstLane,
                firstDirection > 0 ? level.PreviousEdge(firstLane) : level.NextEdge(firstLane),
                firstDirection, Height),
            new TankerCargoSpawn(secondLane,
                secondDirection > 0 ? level.PreviousEdge(secondLane) : level.NextEdge(secondLane),
                secondDirection, Height)
        ];
        return new TankerReleaseResult(kind, cargo);
    }

    public override void Update() { }
}
