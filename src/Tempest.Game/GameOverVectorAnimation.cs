namespace Tempest.Game;

public sealed class GameOverVectorRecord
{
    public const int GraphicId = 0x1F;
    public const int PaletteIndex = 0x0C;

    public byte X { get; private set; } = 0x80;
    public byte Y { get; private set; } = 0x80;
    public byte Z { get; private set; } = 0x80;
    public sbyte VelocityX { get; }
    public sbyte VelocityY { get; }
    public sbyte VelocityZ { get; }
    // C# visual-color cycling follows record creation; the ROM path does not expose a per-record hue.
    public int SpawnOrdinal { get; }

    internal GameOverVectorRecord(
        sbyte velocityX,
        sbyte velocityY,
        sbyte velocityZ,
        int spawnOrdinal)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(spawnOrdinal);
        VelocityX = velocityX;
        VelocityY = velocityY;
        VelocityZ = velocityZ;
        SpawnOrdinal = spawnOrdinal;
    }

    internal bool Advance()
    {
        int nextY = Y + VelocityY;
        int nextZ = Z + VelocityZ;
        if (nextY < 0x10 || nextY >= 0xF0 || nextZ < 0x10 || nextZ >= 0xF0)
            return false;

        X = unchecked((byte)(X + VelocityX));
        Y = (byte)nextY;
        Z = (byte)nextZ;
        return true;
    }
}

public sealed class GameOverVectorAnimation
{
    public const int RecordCapacity = 16;
    public const int InitialSpawnCounter = 0x20;

    private readonly Func<int, int, int> nextRandomInclusive;
    private readonly GameOverVectorRecord?[] records = new GameOverVectorRecord?[RecordCapacity];
    private int romTimeCounter;

    public IReadOnlyList<GameOverVectorRecord?> Records => records;
    public int ActiveRecordCount => records.Count(record => record is not null);
    public int CreatedRecordCount { get; private set; }
    public int SpawnCounter { get; private set; }
    public int RomTimeCounter => romTimeCounter;
    public int BurstIndex { get; }
    public bool IsStarted { get; private set; }
    public bool IsComplete { get; private set; }

    public GameOverVectorAnimation(
        Func<int, int, int> nextRandomInclusive,
        int burstIndex = 0)
    {
        ArgumentNullException.ThrowIfNull(nextRandomInclusive);
        ArgumentOutOfRangeException.ThrowIfNegative(burstIndex);
        this.nextRandomInclusive = nextRandomInclusive;
        BurstIndex = burstIndex;
    }

    public void Start()
    {
        Array.Clear(records);
        SpawnCounter = InitialSpawnCounter;
        CreatedRecordCount = 0;
        romTimeCounter = 0;
        IsStarted = true;
        IsComplete = false;
    }

    public void Reset()
    {
        Array.Clear(records);
        SpawnCounter = 0;
        CreatedRecordCount = 0;
        romTimeCounter = 0;
        IsStarted = false;
        IsComplete = false;
    }

    public void Advance()
    {
        if (!IsStarted)
            throw new InvalidOperationException("The Game Over vector animation has not started.");
        if (IsComplete)
            return;

        for (int index = records.Length - 1; index >= 0; index--)
        {
            GameOverVectorRecord? record = records[index];
            if (record is null)
            {
                if (SpawnCounter > 0)
                {
                    records[index] = CreateRecord();
                    CreatedRecordCount++;
                }
            }
            else if (!record.Advance())
            {
                records[index] = null;
            }
        }

        if (SpawnCounter > 0 && (romTimeCounter & 1) == 0)
            SpawnCounter--;
        romTimeCounter = (romTimeCounter + 1) & 0xFF;
        IsComplete = SpawnCounter == 0 && ActiveRecordCount == 0;
    }

    private GameOverVectorRecord CreateRecord()
    {
        sbyte velocityX = NextVelocity();
        sbyte velocityY = NextVelocity();
        sbyte velocityZ = NextVelocity();

        // The ROM permits zero velocity on both bounded axes; avoid a permanently retained record.
        if (velocityY == 0 && velocityZ == 0)
            velocityY = nextRandomInclusive(0, 1) == 0 ? (sbyte)-1 : (sbyte)1;

        return new GameOverVectorRecord(velocityX, velocityY, velocityZ, CreatedRecordCount);
    }

    private sbyte NextVelocity()
    {
        int magnitude = nextRandomInclusive(0, 7);
        return nextRandomInclusive(0, 1) == 0
            ? (sbyte)-magnitude
            : (sbyte)magnitude;
    }
}
