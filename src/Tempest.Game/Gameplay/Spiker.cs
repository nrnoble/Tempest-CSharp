using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

public enum SpikerState
{
    Initializing,
    Building,
    Retreating,
    ReturnedToReserve
}

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

    public void Update(Level level, Func<bool> shouldRetreat)
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
                            State = SpikerState.Retreating;
                        // The optional retreat bullet is deliberately outside this milestone.
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
    }

    public bool IsHitBy(Shot shot) => shot.EdgeIndex == EdgeIndex && shot.Height < Height;

    public override void Update() { }
}
