using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum SpikerState
{
    Initializing,
    Building,
    Retreating,
    ReturnedToReserve
}

/// <summary>
/// Spiker: builds a persistent spike up its lane, then retreats. Production identity is owned by
/// the level wave ledger; the Spike remains a separate persistent hazard.
/// </summary>
public sealed class Spiker : Enemy
{
    public Spiker(int edgeIndex = 0, float speed = 3, int originalSlotId = -1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(speed);
        ArgumentOutOfRangeException.ThrowIfLessThan(originalSlotId, -1);
        EdgeIndex = edgeIndex;
        Speed = speed;
        OriginalSlotId = originalSlotId;
    }

    public int OriginalSlotId { get; }
    public float Speed { get; }
    public float Growth { get; private set; }
    public float Rotation { get; private set; }
    public SpikerState State { get; private set; }
    public int ShotDelayTicks { get; private set; }

    public EnemyBullet? Update(Level level, Func<bool> shouldRetreat,
        Func<int, int, int>? randomInclusive = null, bool allowGrowthRetreatShot = true)
    {
        Rotation = (Rotation + 45) % 360;
        switch (State)
        {
            case SpikerState.Initializing:
                State = SpikerState.Building;
                break;
            case SpikerState.Building:
                if (Height < Level.Depth - 4)
                {
                    Height += Speed;
                    Spike? spike = level.Spikes.FirstOrDefault(candidate => candidate.EdgeIndex == EdgeIndex);
                    if (spike is null)
                    {
                        level.CreateSpike(EdgeIndex, Height);
                    }
                    else if (Height > spike.Height)
                    {
                        // Incitti assigns height - 4, even if this briefly shortens the spike.
                        spike.Height = Height - 4;
                        Growth += Speed;
                        if (Growth > 100 && shouldRetreat())
                        {
                            State = SpikerState.Retreating;
                            // Preserve the development/legacy firing path; canonical Level 7
                            // uses TempestGame's independent shared-shot phase instead.
                            if (allowGrowthRetreatShot &&
                                randomInclusive is not null && randomInclusive(0, 100) > 70)
                                return new EnemyBullet(EdgeIndex, Height);
                        }
                    }
                }
                else
                {
                    State = SpikerState.Retreating;
                }
                break;
            case SpikerState.Retreating:
                Growth = 0;
                if (Height > 0)
                    Height -= Speed * .90f;
                else
                {
                    Height = 0;
                    State = SpikerState.ReturnedToReserve;
                }
                break;
        }

        return null;
    }

    internal bool AdvanceShotDelay()
    {
        if (ShotDelayTicks == 0)
            return true;

        ShotDelayTicks--;
        return false;
    }

    internal void ReloadShotDelay(int ticks)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticks);
        ShotDelayTicks = ticks;
    }

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public void RestartForPendingSupply()
    {
        if (State != SpikerState.ReturnedToReserve)
            throw new InvalidOperationException(
                "Only a Spiker at its retreat endpoint can restart its pending-supply lifecycle.");

        // PROJECT TRANSLATION: retain the live identity without inventing the ROM's lane hop.
        Growth = 0;
        State = SpikerState.Building;
    }

    public override bool TryZap(int delay) => AcceptZap(delay);

    public override void Update() { }
}
