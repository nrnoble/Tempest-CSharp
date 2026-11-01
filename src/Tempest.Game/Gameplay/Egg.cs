namespace Tempest.Game.Gameplay;

public enum EggState
{
    Roaming,
    Approaching,
    ReadyToHatch
}

/// <summary>
/// Spawn-in stage before an enemy hatches. Roaming: bobs below the rim while sliding lanes.
/// Approaching: rises to height 0, then ReadyToHatch. Cadence (every 8th update) is from Incitti.
/// OriginalSlotId/CargoUnitId carry Tanker-ledger identity through a fresh-Egg restart; they are
/// -1 for ordinary Level 1 and Level 2 Eggs.
/// </summary>
public sealed class Egg
{
    public Egg(int edgeIndex = 0, int movementDirection = 1, int originalSlotId = -1, int cargoUnitId = -1)
    {
        EdgeIndex = edgeIndex;
        MovementDirection = movementDirection;
        OriginalSlotId = originalSlotId;
        CargoUnitId = cargoUnitId;
    }

    public int EdgeIndex { get; private set; }
    public int OriginalSlotId { get; }
    public int CargoUnitId { get; }
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
