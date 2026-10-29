namespace Tempest.Game.Gameplay;

public sealed class Shot
{
    public int EdgeIndex { get; init; }
    public float Height { get; set; }
    public float Rotation { get; private set; }

    public void Update()
    {
        Height -= 6;
        Rotation = (Rotation + 15) % 360;
    }
}
