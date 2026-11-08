namespace Tempest.Game.Gameplay;

/// <summary>
/// Player projectile. Height uses the shared depth convention (Level.Depth at the rim, 0 at the
/// far end): a shot spawns at the player's depth and Height falls 6 per update as it travels away
/// toward the far web, expiring once below 0. Unlike EnemyBullet (which climbs toward the rim at 3
/// per update), shots only ever move away from the player. Rotation remains legacy cosmetic state,
/// but the static ROM-derived rendering ignores it; it has no collision meaning.
/// </summary>
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
