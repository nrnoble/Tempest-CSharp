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
/// Spiker: builds a persistent spike up its lane, then retreats. Not counted as one of the level's
/// enemies in normal accounting. Speed, rotation step and the Depth-4 build limit come from Incitti
/// and are not yet compared to the arcade.
/// </summary>
public sealed class Spiker : Enemy
{
    public Spiker(int edgeIndex = 0, float speed = 3)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(speed);
        EdgeIndex = edgeIndex;
        Speed = speed;
    }

    public float Speed { get; }
    public float Growth { get; private set; }
    public float Rotation { get; private set; }
    public SpikerState State { get; private set; }

    public EnemyBullet? Update(Level level, Func<bool> shouldRetreat,
        Func<int, int, int>? randomInclusive = null)
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
                            // Incitti's independent second roll fires only on this random
                            // growth-driven retreat, not on the upper-height retreat.
                            if (randomInclusive is not null && randomInclusive(0, 100) > 70)
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

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public override void Update() { }
}
