namespace Tempest.Game.Gameplay;

public sealed class Player
{
    public int EdgeIndex { get; private set; } = 7;
    public float EdgePosition { get; private set; } = 0.5f;
    public int Lives { get; set; } = 3;

    public void ResetPosition()
    {
        EdgeIndex = 7;
        EdgePosition = 0.5f;
    }

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
