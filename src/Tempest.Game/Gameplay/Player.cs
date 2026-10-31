using Tempest.Game.Levels;

namespace Tempest.Game.Gameplay;

/// <summary>
/// Player (Blaster) position state. Location on the rim is EdgeIndex (the current lane) plus
/// EdgePosition, a continuous 0..1 offset along that lane; crossing either end carries into the
/// adjacent lane, so movement wraps smoothly around the closed tube. Depth is Level.Depth while
/// clamped to the rim during normal play and shrinks toward 0 only during cleared-level exit
/// travel. Lane 7 / offset 0.5 is the fixed starting location restored after every life loss.
/// </summary>
public sealed class Player
{
    // Arcade-default lives; --lives overrides this for development sessions only.
    public const int DefaultStartingLives = 3;

    public Player(int startingLives = DefaultStartingLives)
    {
        Lives = startingLives > 0 ? startingLives : DefaultStartingLives;
    }

    public int EdgeIndex { get; private set; } = 7;
    public float EdgePosition { get; private set; } = 0.5f;
    public int Lives { get; set; }
    public float Depth { get; private set; } = Level.Depth;

    // Restores the fixed spawn location after a life loss; lives and score are managed elsewhere.
    public void ResetPosition(int edgeIndex = 7, float edgePosition = 0.5f)
    {
        EdgeIndex = edgeIndex;
        EdgePosition = edgePosition;
        Depth = Level.Depth;
    }

    // Exit travel moves the player down the tube by reducing Depth (clamped at the far end);
    // ResetDepth snaps back to the rim when travel is cancelled or replayed.
    public void AdvanceExit(float distance) => Depth = MathF.Max(0, Depth - distance);
    public void ResetDepth() => Depth = Level.Depth;

    // Movement is a signed fraction of a lane per call. The while loops (not a single modulo)
    // preserve the exact lane-boundary crossing order for large spinner deltas, carrying any
    // remainder into the new lane so EdgePosition always ends in [0, 1].
    public void Move(float movement, int edgeCount)
    {
        EdgePosition += movement;

        while (EdgePosition < 0)
        {
            EdgeIndex = (EdgeIndex - 1 + edgeCount) % edgeCount;
            EdgePosition += 1;
        }

        while (EdgePosition > 1)
        {
            EdgeIndex = (EdgeIndex + 1) % edgeCount;
            EdgePosition -= 1;
        }
    }

    public void Update() { }
}
