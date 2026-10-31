namespace Tempest.Game.Gameplay;

public abstract class Enemy
{
    public int EdgeIndex { get; protected set; }
    public float Height { get; protected set; }
    public int PendingZapTicks { get; private set; }
    public abstract void Update();

    public virtual bool TryZap(int delay) => false;

    protected bool AcceptZap(int delay)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(delay);
        PendingZapTicks = delay;
        return true;
    }

    public bool AdvanceZapTimer()
    {
        if (PendingZapTicks <= 0)
            return false;

        PendingZapTicks--;
        return PendingZapTicks == 0;
    }
}
