namespace Tempest.Game.Systems;

public enum AudioEvent
{
    PlayerMove,
    PlayerShot,
    EnemyBullet,
    SpikeHit,
    PulsarPulse,
    ZapResolution,
    PlayerKilledByBullet,
    PlayerKilledByEnemy,
    EnemyDestroyed,
    ZoomIn,
    ZoomOut,
    BonusLife
}

public interface IAudioEvents
{
    void Play(AudioEvent audioEvent);
}

public sealed class SilentAudioEvents : IAudioEvents
{
    public static SilentAudioEvents Instance { get; } = new();

    private SilentAudioEvents() { }

    public void Play(AudioEvent audioEvent) { }
}
