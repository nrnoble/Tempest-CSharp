namespace Tempest.Game.Gameplay;

public enum EnemyDestructionEffectProgram
{
    BlasterHitType0
}

public sealed class EnemyDestructionEffect
{
    public const int AgeStep = 3;
    public const int LifetimeThreshold = 10;

    internal EnemyDestructionEffect(int slot)
    {
        Slot = slot;
    }

    public int Slot { get; }
    public bool IsActive { get; private set; }
    public int EdgeIndex { get; private set; }
    public float Height { get; private set; }
    public EnemyDestructionEffectProgram Program { get; private set; }
    public int Age { get; private set; }

    internal void Activate(int edgeIndex, float height, EnemyDestructionEffectProgram program)
    {
        EdgeIndex = edgeIndex;
        Height = height;
        Program = program;
        Age = 0;
        IsActive = true;
    }

    internal void Advance()
    {
        if (!IsActive)
            return;

        Age += AgeStep;
        if (Age >= LifetimeThreshold)
            IsActive = false;
    }

    internal void Deactivate() => IsActive = false;
}

public sealed class EnemyDestructionEffectPool
{
    public const int Capacity = 8;

    private readonly EnemyDestructionEffect[] slots =
        Enumerable.Range(0, Capacity).Select(index => new EnemyDestructionEffect(index)).ToArray();
    private readonly IReadOnlyList<EnemyDestructionEffect> readOnlySlots;

    public EnemyDestructionEffectPool()
    {
        readOnlySlots = Array.AsReadOnly(slots);
    }

    public IReadOnlyList<EnemyDestructionEffect> Slots => readOnlySlots;
    public int ActiveCount => slots.Count(effect => effect.IsActive);

    public EnemyDestructionEffect Add(
        int edgeIndex,
        float height,
        EnemyDestructionEffectProgram program = EnemyDestructionEffectProgram.BlasterHitType0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(edgeIndex);
        if (!float.IsFinite(height) || height < 0)
            throw new ArgumentOutOfRangeException(nameof(height));
        if (!Enum.IsDefined(program))
            throw new ArgumentOutOfRangeException(nameof(program));

        // The ROM allocator scans slots from 7 down and uses the first free one.
        for (int index = Capacity - 1; index >= 0; index--)
        {
            if (slots[index].IsActive)
                continue;

            slots[index].Activate(edgeIndex, height, program);
            return slots[index];
        }

        // A full ROM pool reuses the greatest-age record; descending scan plus >= makes
        // equal-age ties resolve to the lowest slot index.
        EnemyDestructionEffect oldest = slots[0];
        for (int index = Capacity - 1; index >= 0; index--)
        {
            if (slots[index].Age >= oldest.Age)
                oldest = slots[index];
        }

        oldest.Activate(edgeIndex, height, program);
        return oldest;
    }

    public void Advance()
    {
        foreach (EnemyDestructionEffect effect in slots)
            effect.Advance();
    }

    public void Clear()
    {
        foreach (EnemyDestructionEffect effect in slots)
        {
            effect.Deactivate();
        }
    }
}
