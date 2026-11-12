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

public enum TankerCargoMovementProgram
{
    Existing,
    Level6Deep0B,
    Level6Straight07,
    Level9SpikeSensitive53,
    Level9Straight07
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
    private const byte Level6DeepCargoRomAlongThreshold = 0x20;

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
        Func<int, int, int>? randomInclusive, out EnemyBullet? bullet,
        int shotHoldoffTicks = BulletTimerResetTicks)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(shotHoldoffTicks);
        bullet = null;
        if (State == TankerState.CargoReleased)
            return TankerUpdateResult.Active;
        if (State == TankerState.CargoReleasePending)
            return TankerUpdateResult.NaturalCargoRelease;

        Height += ClimbSpeed;
        // Incitti climbs first and then gates firing on the post-climb height (`If height < 380`),
        // so the bullet origin is always the just-advanced height and exactly 380 is silent.
        if (Height < BulletFiringCeiling)
            bullet = TryFireBullet(randomInclusive, shotHoldoffTicks);

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
    private EnemyBullet? TryFireBullet(
        Func<int, int, int>? randomInclusive, int shotHoldoffTicks)
    {
        if (BulletAmmunition <= 0 || randomInclusive is null)
            return null;

        BulletTimer--;
        if (BulletTimer >= 0)
            return null;

        BulletTimer = shotHoldoffTicks;
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

    public bool UsesLevel8ProgrammedCargo(
        LevelConfiguration configuration, TankerReleaseKind releaseKind)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.BelowRimFlipperProgram ==
                BelowRimFlipperProgram.Level8ConstantFlip &&
            releaseKind == TankerReleaseKind.PlayerShotDestruction &&
            IsLevel8DeepCargoPosition(Height);
    }

    public bool UsesLevel9SpikeSensitiveCargo(
        LevelConfiguration configuration, TankerReleaseKind releaseKind)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return GetCargoMovementProgram(configuration, releaseKind) ==
            TankerCargoMovementProgram.Level9SpikeSensitive53;
    }

    public TankerCargoMovementProgram GetCargoMovementProgram(
        LevelConfiguration configuration, TankerReleaseKind releaseKind)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (releaseKind is not
            (TankerReleaseKind.Natural or TankerReleaseKind.PlayerShotDestruction or
             TankerReleaseKind.SuperZapperDestruction))
            throw new ArgumentOutOfRangeException(nameof(releaseKind));

        if (!ReferenceEquals(configuration, LevelConfiguration.Level6Production) ||
            releaseKind == TankerReleaseKind.SuperZapperDestruction)
        {
            if (configuration.UsesLevel9RomWavePolicy)
                return GetLevel9CargoMovementProgram(Height, releaseKind);
            return TankerCargoMovementProgram.Existing;
        }

        // The existing ROM-along conversion is a project approximation, but preserves the
        // ROM's $20 release branch without guessing a C# height cutoff.
        return RomAlongPositionForHeight(Height) >= Level6DeepCargoRomAlongThreshold
            ? TankerCargoMovementProgram.Level6Deep0B
            : TankerCargoMovementProgram.Level6Straight07;
    }

    public static float HeightForRomAlongPosition(byte rawAlong) =>
        (0xF0 - rawAlong) * Level.Depth / (float)(0xF0 - 0x10);

    public static double RomAlongPositionForHeight(float height) =>
        0xF0 - (double)height * (0xF0 - 0x10) / Level.Depth;

    public static bool IsLevel8DeepCargoPosition(float height)
    {
        if (!float.IsFinite(height))
            throw new ArgumentOutOfRangeException(nameof(height));
        return RomAlongPositionForHeight(height) >= 0x20;
    }

    public static bool IsLevel9DeepCargoPosition(float height)
    {
        if (!float.IsFinite(height))
            throw new ArgumentOutOfRangeException(nameof(height));
        return RomAlongPositionForHeight(height) >= 0x20;
    }

    public static TankerCargoMovementProgram GetLevel9CargoMovementProgram(
        float height, TankerReleaseKind releaseKind)
    {
        if (!float.IsFinite(height))
            throw new ArgumentOutOfRangeException(nameof(height));
        if (releaseKind is not
            (TankerReleaseKind.Natural or TankerReleaseKind.PlayerShotDestruction or
             TankerReleaseKind.SuperZapperDestruction))
            throw new ArgumentOutOfRangeException(nameof(releaseKind));

        return releaseKind == TankerReleaseKind.PlayerShotDestruction &&
            IsLevel9DeepCargoPosition(height)
                ? TankerCargoMovementProgram.Level9SpikeSensitive53
                : TankerCargoMovementProgram.Level9Straight07;
    }

    public override bool TryZap(int delay) => AcceptZap(delay);

    public bool HitsPlayer(Player player) => false;

    // Cargo follows the project-selected outward lane route; its shared Flipper geometry still
    // pivots inward into the web. Same-tick policy: children first move on the next tick.
    // Fails closed if already released.
    public TankerReleaseResult ReleaseFlipperCargo(
        Level level, int firstSideDirection, TankerReleaseKind kind)
    {
        ArgumentNullException.ThrowIfNull(level);
        if (LatentCargoCount != CarriedFlipperCount || CargoReleased)
            return new TankerReleaseResult(kind, []);

        int side = firstSideDirection < 0 ? -1 : 1;
        TankerCargoSpawn[] cargo;
        if (level.Continuous)
        {
            int firstLane = side > 0 ? level.PreviousEdge(EdgeIndex) : level.NextEdge(EdgeIndex);
            int secondLane = side > 0 ? level.NextEdge(EdgeIndex) : level.PreviousEdge(EdgeIndex);
            int firstDirection = side;
            int secondDirection = -side;
            cargo =
            [
                new TankerCargoSpawn(firstLane,
                    firstDirection > 0 ? level.PreviousEdge(firstLane) : level.NextEdge(firstLane),
                    firstDirection, Height),
                new TankerCargoSpawn(secondLane,
                    secondDirection > 0 ? level.PreviousEdge(secondLane) : level.NextEdge(secondLane),
                    secondDirection, Height)
            ];
        }
        else
        {
            cargo = CreateOpenTubeCargoPair(level, side);
        }

        LatentCargoCount = 0;
        CargoReleased = true;
        State = TankerState.CargoReleased;
        return new TankerReleaseResult(kind, cargo);
    }

    private TankerCargoSpawn[] CreateOpenTubeCargoPair(Level level, int side)
    {
        int firstDelta = side > 0 ? -1 : 1;
        int secondDelta = -firstDelta;
        int firstLane = level.TryGetAdjacentLane(EdgeIndex, firstDelta, out int adjacentFirst)
            ? adjacentFirst
            : EdgeIndex;
        int secondLane = level.TryGetAdjacentLane(EdgeIndex, secondDelta, out int adjacentSecond)
            ? adjacentSecond
            : EdgeIndex;
        int firstDirection = side;
        int secondDirection = -side;

        int firstDestination = MakeCargoDirectionPlayable(level, firstLane, ref firstDirection);
        int secondDestination = MakeCargoDirectionPlayable(level, secondLane, ref secondDirection);
        if (firstDirection == secondDirection)
        {
            int oppositeFirstDirection = -firstDirection;
            if (TryGetCargoDestination(level, firstLane, oppositeFirstDirection, out int oppositeFirst))
            {
                firstDirection = oppositeFirstDirection;
                firstDestination = oppositeFirst;
            }
            else
            {
                int oppositeSecondDirection = -secondDirection;
                if (!TryGetCargoDestination(level, secondLane, oppositeSecondDirection,
                        out secondDestination))
                    throw new InvalidOperationException("Open-tube Tanker cargo has no valid inward route.");
                secondDirection = oppositeSecondDirection;
            }
        }

        return
        [
            new TankerCargoSpawn(firstLane, firstDestination, firstDirection, Height),
            new TankerCargoSpawn(secondLane, secondDestination, secondDirection, Height)
        ];
    }

    private static int MakeCargoDirectionPlayable(Level level, int lane, ref int direction)
    {
        if (TryGetCargoDestination(level, lane, direction, out int destination))
            return destination;

        direction = -direction;
        if (!TryGetCargoDestination(level, lane, direction, out destination))
            throw new InvalidOperationException("Open-tube Tanker cargo has no adjacent playable lane.");
        return destination;
    }

    private static bool TryGetCargoDestination(Level level, int lane, int direction, out int destination) =>
        level.TryGetAdjacentLane(lane, direction > 0 ? -1 : 1, out destination);

    public override void Update() { }
}
