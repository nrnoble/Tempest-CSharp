using Raylib_cs;

namespace Tempest.Game.Systems;

public sealed class AudioSystem : IAudioEvents, IDisposable
{
    private const int VoicesPerSound = 4;
    private static readonly (AudioEvent Event, string FileName)[] Assets =
    [
        (AudioEvent.PlayerMove, "tick.wav"),
        (AudioEvent.PlayerShot, "shot.wav"),
        (AudioEvent.EnemyBullet, "bullet.wav"),
        (AudioEvent.SpikeHit, "spikeshot.wav"),
        (AudioEvent.PulsarPulse, "pulse.wav"),
        (AudioEvent.ZapResolution, "zap.wav"),
        (AudioEvent.PlayerKilledByBullet, "killedbybullet.wav"),
        (AudioEvent.PlayerKilledByEnemy, "killedbyflipper.wav"),
        (AudioEvent.EnemyDestroyed, "flippershot.wav"),
        (AudioEvent.ZoomIn, "zoomin.wav"),
        (AudioEvent.ZoomOut, "zoomout.wav"),
        (AudioEvent.BonusLife, "bonus.wav")
    ];

    private readonly Dictionary<AudioEvent, Sound[]> sounds = [];
    private bool disposed;

    public AudioSystem()
    {
        if (!Raylib.IsAudioDeviceReady())
            throw new InvalidOperationException("The Raylib audio device must be initialized before loading sounds.");

        try
        {
            foreach ((AudioEvent audioEvent, string fileName) in Assets)
                Load(audioEvent, fileName);
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void Play(AudioEvent audioEvent)
    {
        ObjectDisposedException.ThrowIf(disposed, this);
        Sound[] voices = sounds[audioEvent];
        if (audioEvent == AudioEvent.PlayerShot)
        {
            Sound voice = voices[0];
            if (Raylib.IsSoundPlaying(voice))
                Raylib.StopSound(voice);

            Raylib.PlaySound(voice);
            return;
        }

        foreach (Sound voice in voices)
        {
            if (Raylib.IsSoundPlaying(voice))
                continue;

            Raylib.PlaySound(voice);
            return;
        }
    }

    public void Dispose()
    {
        if (disposed)
            return;

        disposed = true;
        foreach (Sound[] voices in sounds.Values)
        {
            for (int index = voices.Length - 1; index >= 0; index--)
            {
                if (!Raylib.IsSoundValid(voices[index]))
                    continue;

                if (index == 0)
                    Raylib.UnloadSound(voices[index]);
                else
                    Raylib.UnloadSoundAlias(voices[index]);
            }
        }

        sounds.Clear();
    }

    private void Load(AudioEvent audioEvent, string fileName)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "sfx", fileName);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Required sound asset was not found: {path}", path);

        Sound[] voices = new Sound[audioEvent == AudioEvent.PlayerShot ? 1 : VoicesPerSound];
        voices[0] = Raylib.LoadSound(path);
        if (!Raylib.IsSoundValid(voices[0]))
            throw new InvalidOperationException($"Raylib could not load sound asset: {path}");

        sounds.Add(audioEvent, voices);
        for (int index = 1; index < voices.Length; index++)
        {
            voices[index] = Raylib.LoadSoundAlias(voices[0]);
            if (!Raylib.IsSoundValid(voices[index]))
                throw new InvalidOperationException($"Raylib could not create a playback voice for: {path}");
        }
    }
}
