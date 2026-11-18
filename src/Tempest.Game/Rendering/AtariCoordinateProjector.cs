using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public readonly record struct AtariQuotientCountState(byte LowByte, byte HighByte)
{
    // DEVELOPMENT-ONLY hypothesis: RN.low is $0f, while RN.high=0 is unresolved at normal Level 7 DIVZ.
    public static AtariQuotientCountState PrototypeAssumption => new(0x0F, 0x00);
}

public readonly record struct AtariDvgPoint
{
    public AtariDvgPoint(int x, int y)
    {
        if (x is < -4096 or > 4095)
            throw new ArgumentOutOfRangeException(nameof(x), "DVG coordinates are signed 13-bit values.");
        if (y is < -4096 or > 4095)
            throw new ArgumentOutOfRangeException(nameof(y), "DVG coordinates are signed 13-bit values.");

        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
}

public readonly record struct AtariCoordinateProjection(
    int DeltaX,
    int DeltaY,
    int Divisor,
    int QuotientX,
    int QuotientY,
    short SourceX,
    short SourceY,
    AtariDvgPoint DvgPoint);

public static class AtariCoordinateProjector
{
    public static AtariCoordinateProjection Project(
        AtariTubeRecord record,
        byte rawX,
        byte rawY,
        int runtimePosition,
        short xBase,
        short yBase,
        AtariQuotientCountState quotientCountState,
        bool useRn000fDiagnostic = false)
    {
        ArgumentNullException.ThrowIfNull(record);
        if (runtimePosition is < byte.MinValue or > byte.MaxValue)
            throw new ArgumentOutOfRangeException(
                nameof(runtimePosition), "Atari runtime position must fit in one byte.");
        if (quotientCountState != AtariQuotientCountState.PrototypeAssumption)
            throw new ArgumentOutOfRangeException(
                nameof(quotientCountState),
                "The prototype assumes quotient-count low byte 0x0f and high byte zero.");

        int divisor = runtimePosition + record.ScaleRaw;

        int deltaX = Math.Abs(rawX - 0x80);
        int deltaY = Math.Abs(rawY - record.CameraHeightRaw);
        int quotientX = useRn000fDiagnostic
            ? CalculateConditionalRn000fDiagnosticQuotient(deltaX, runtimePosition, record.ScaleRaw)
            : CalculateControlledQuotient(deltaX, runtimePosition, record.ScaleRaw);
        int quotientY = useRn000fDiagnostic
            ? CalculateConditionalRn000fDiagnosticQuotient(deltaY, runtimePosition, record.ScaleRaw)
            : CalculateControlledQuotient(deltaY, runtimePosition, record.ScaleRaw);
        long signedX = rawX < 0x80 ? -quotientX : quotientX;
        long signedY = rawY < record.CameraHeightRaw ? -quotientY : quotientY;
        short sourceX = SaturateSigned16(xBase + signedX);
        short sourceY = SaturateSigned16(yBase + signedY);

        return new AtariCoordinateProjection(
            deltaX,
            deltaY,
            divisor,
            quotientX,
            quotientY,
            sourceX,
            sourceY,
            new AtariDvgPoint(PackSigned13(sourceY), PackSigned13(sourceX)));
    }

    // Controlled trace vectors include arithmetic operands wider than the byte runtime state;
    // Project separately enforces the Atari one-byte runtime-position boundary.
    public static int CalculateControlledQuotient(
        int magnitude,
        int positionOperand,
        int scale) =>
        CalculateQuotient(magnitude, positionOperand, scale, 256);

    // DEVELOPMENT-ONLY conditional RN=$000F result; RN.high=0 is not established at the normal Level 7 call.
    public static int CalculateConditionalRn000fDiagnosticQuotient(
        int magnitude,
        int positionOperand,
        int scale) =>
        CalculateQuotient(magnitude, positionOperand, scale, 128);

    private static int CalculateQuotient(
        int magnitude,
        int positionOperand,
        int scale,
        int numerator)
    {
        if (magnitude is < 0 or > byte.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(magnitude));
        if (positionOperand < 0)
            throw new ArgumentOutOfRangeException(nameof(positionOperand));
        if (scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));
        long divisorValue = (long)positionOperand + scale;
        if (divisorValue <= 0 || divisorValue > ushort.MaxValue)
            throw new ArgumentOutOfRangeException(
                nameof(positionOperand), "Atari projection requires a positive 16-bit divisor.");

        return (int)((magnitude * (long)numerator) / divisorValue);
    }

    public static int PackSigned13(short value)
    {
        int packed = value & 0x1FFF;
        return packed >= 0x1000 ? packed - 0x2000 : packed;
    }

    private static short SaturateSigned16(long value)
    {
        if (value > short.MaxValue)
            return short.MaxValue;
        if (value < short.MinValue)
            return short.MinValue;
        return (short)value;
    }
}
