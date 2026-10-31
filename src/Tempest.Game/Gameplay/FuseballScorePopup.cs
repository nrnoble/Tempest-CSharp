namespace Tempest.Game.Gameplay;

/// <summary>
/// Incitti's Fuseball-only score label: 30 ticks, sized by kill height. Not a general popup system.
/// The renderer uses the lane midpoint, not the Fuseball's lateral w position, as in the source.
/// </summary>
public sealed class FuseballScorePopup(int edgeIndex, float height, int points)
{
    public int EdgeIndex { get; } = edgeIndex;
    public float Height { get; } = height;
    public int Points { get; } = points;
    public float Scale => Height / 64 + .5f;
    public int RemainingTicks { get; private set; } = 30;
    public bool IsExpired => RemainingTicks <= 0;

    public void Update() => RemainingTicks--;
}
