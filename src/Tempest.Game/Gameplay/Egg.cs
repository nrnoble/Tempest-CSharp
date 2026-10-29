namespace Tempest.Game.Gameplay;

public enum EggState
{
    Roaming,
    Approaching,
    ReadyToHatch
}

public sealed class Egg
{
    public Egg(int edgeIndex = 0, int movementDirection = 1)
    {
        EdgeIndex = edgeIndex;
        MovementDirection = movementDirection;
    }

    public int EdgeIndex { get; private set; }
    public float Height { get; private set; } = -10;
    public int UpdateCounter { get; private set; }
    public int MovementDirection { get; }
    public int VerticalDirection { get; private set; } = 1;
    public EggState State { get; private set; } = EggState.Roaming;

    public void BeginApproach()
    {
        if (State != EggState.Roaming)
            return;

        State = EggState.Approaching;
        UpdateCounter = 0;
    }

    public void Update(Levels.Level level)
    {
        if (State == EggState.ReadyToHatch)
            return;

        UpdateCounter++;
        if (UpdateCounter <= 7)
            return;

        UpdateCounter = 0;
        if (State == EggState.Roaming)
        {
            Height += VerticalDirection;
            if (Height >= -1 || Height <= -20)
                VerticalDirection = -VerticalDirection;

            EdgeIndex = MovementDirection > 0
                ? level.NextEdge(EdgeIndex)
                : level.PreviousEdge(EdgeIndex);
        }
        else
        {
            Height += 2;
            if (Height >= 0)
            {
                Height = 0;
                State = EggState.ReadyToHatch;
            }
        }
    }
}
