namespace Tempest.Game.Gameplay;

public abstract class Enemy
{
    public int EdgeIndex { get; protected set; }
    public float Height { get; protected set; }
    public abstract void Update();
}
