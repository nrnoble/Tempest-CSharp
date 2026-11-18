using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class AtariTubeRecordPrototypeTests
{
    [Fact]
    public void Record06ContainsTheSourceOrderedRawPointsAndRomMetadata()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;

        Assert.Equal(6, record.RecordIndex);
        Assert.Equal(16, record.PointCount);
        Assert.Equal(
            new byte[]
            {
                0xEA, 0xE0, 0x9C, 0x80, 0x64, 0x20, 0x16, 0x50,
                0x16, 0x20, 0x64, 0x80, 0x9C, 0xE0, 0xEA, 0xB0
            },
            record.RawXPoints);
        Assert.Equal(
            new byte[]
            {
                0xA0, 0xE0, 0xEA, 0xB0, 0xEA, 0xE0, 0xA0, 0x80,
                0x60, 0x20, 0x16, 0x50, 0x16, 0x20, 0x60, 0x80
            },
            record.RawYPoints);
        Assert.Equal((byte)0x18, record.ScaleRaw);
        Assert.Equal((byte)0x68, record.CameraHeightRaw);
        Assert.Equal((short)-144, record.ScreenYOffset);
        Assert.Equal(0x00, record.OpenRaw);
        Assert.False(record.IsOpen);
        Assert.Same(record, AtariTubeRecordCatalog.ForRecordIndex(6));
    }

    [Fact]
    public void RecordCopiesItsInputsAndExposesReadOnlyPointLists()
    {
        byte[] rawX = Enumerable.Repeat((byte)0x80, 16).ToArray();
        byte[] rawY = Enumerable.Repeat((byte)0x68, 16).ToArray();
        AtariTubeRecord record = new(6, rawX, rawY, 0x18, 0x68, -144, 0x00);
        rawX[0] = 0;
        rawY[0] = 0;

        Assert.Equal(0x80, record.RawXPoints[0]);
        Assert.Equal(0x68, record.RawYPoints[0]);
        Assert.Throws<NotSupportedException>(() =>
            Assert.IsAssignableFrom<IList<byte>>(record.RawXPoints)[0] = 0);
        Assert.Throws<NotSupportedException>(() =>
            Assert.IsAssignableFrom<IList<byte>>(record.RawYPoints)[0] = 0);
    }

    [Fact]
    public void MalformedRecordPointCountsAndMetadataFailExplicitly()
    {
        byte[] points = new byte[16];
        Assert.Throws<ArgumentException>(() =>
            new AtariTubeRecord(6, points[..15], points, 0x18, 0x68, -144, 0x00));
        Assert.Throws<ArgumentException>(() =>
            new AtariTubeRecord(6, points, points[..15], 0x18, 0x68, -144, 0x00));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariTubeRecord(6, points, points, 0, 0x68, -144, 0x00));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariTubeRecord(6, points, points, 0x18, 0x68, -144, 0x01));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariTubeRecord(16, points, points, 0x18, 0x68, -144, 0x00));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariTubeRecordCatalog.ForRecordIndex(0));
    }

    [Theory]
    [InlineData(0, 13, 13, 0)]
    [InlineData(1, 13, 13, 9)]
    [InlineData(64, 32, 32, 256)]
    [InlineData(255, 13, 13, 2510)]
    [InlineData(64, 283, 283, 28)]
    public void ControlledMathBoxTraceQuotientsMatchExactly(
        int magnitude, int runtimePosition, int scale, int expected)
    {
        Assert.Equal(expected, AtariCoordinateProjector.CalculateControlledQuotient(
            magnitude, runtimePosition, scale));
    }

    [Theory]
    [InlineData(0, 13, 13, 0)]
    [InlineData(1, 13, 13, 4)]
    [InlineData(64, 32, 32, 128)]
    [InlineData(255, 13, 13, 1255)]
    [InlineData(64, 283, 283, 14)]
    public void ConditionalRn000fDiagnosticQuotientsMatchExactIntegerTraceVectors(
        int magnitude, int depth, int scale, int expected)
    {
        Assert.Equal(expected,
            AtariCoordinateProjector.CalculateConditionalRn000fDiagnosticQuotient(
                magnitude, depth, scale));
    }

    [Fact]
    public void Record06PointZeroMatchesBothControlledEndpointVectors()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariCoordinateProjection near = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 16, 0, record.ScreenYOffset,
            AtariQuotientCountState.PrototypeAssumption);
        AtariCoordinateProjection far = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 240, 0, record.ScreenYOffset,
            AtariQuotientCountState.PrototypeAssumption);

        Assert.Equal((106, 56, 40, 678, 358, (short)678, (short)214, new AtariDvgPoint(214, 678)),
            (near.DeltaX, near.DeltaY, near.Divisor, near.QuotientX, near.QuotientY,
                near.SourceX, near.SourceY, near.DvgPoint));
        Assert.Equal((106, 56, 264, 102, 54, (short)102, (short)-90, new AtariDvgPoint(-90, 102)),
            (far.DeltaX, far.DeltaY, far.Divisor, far.QuotientX, far.QuotientY,
                far.SourceX, far.SourceY, far.DvgPoint));
    }

    [Fact]
    public void ConditionalRecord00ExampleMatchesTheReconciledVector()
    {
        byte[] points = new byte[16];
        AtariTubeRecord record = new(0, points, points, 0x18, 0x50, -192, 0x00);
        AtariCoordinateProjection actual = AtariCoordinateProjector.Project(
            record, 0xF0, 0x80, 80, 0, -192,
            AtariQuotientCountState.PrototypeAssumption);

        Assert.Equal(104, actual.Divisor);
        Assert.Equal(275, actual.QuotientX);
        Assert.Equal(118, actual.QuotientY);
        Assert.Equal((short)275, actual.SourceX);
        Assert.Equal((short)-74, actual.SourceY);
        Assert.Equal(new AtariDvgPoint(-74, 275), actual.DvgPoint);
    }

    [Fact]
    public void ConditionalRn000fDiagnosticChangesOnlyTheIntegerQuotientTransfer()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariCoordinateProjection current = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 16, 10, -154,
            AtariQuotientCountState.PrototypeAssumption);
        AtariCoordinateProjection conditional = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 16, 10, -154,
            AtariQuotientCountState.PrototypeAssumption,
            useRn000fDiagnostic: true);

        Assert.Equal((106, 56, 40), (conditional.DeltaX, conditional.DeltaY, conditional.Divisor));
        Assert.Equal((339, 179), (conditional.QuotientX, conditional.QuotientY));
        Assert.Equal((short)349, conditional.SourceX);
        Assert.Equal((short)25, conditional.SourceY);
        Assert.Equal(new AtariDvgPoint(25, 349), conditional.DvgPoint);
        Assert.Equal((short)688, current.SourceX);
        Assert.Equal((short)204, current.SourceY);

        AtariCoordinateProjection negative = AtariCoordinateProjector.Project(
            record, 0x7F, 0x67, 16, 0, 0,
            AtariQuotientCountState.PrototypeAssumption,
            useRn000fDiagnostic: true);
        AtariCoordinateProjection positive = AtariCoordinateProjector.Project(
            record, 0x81, 0x69, 16, 0, 0,
            AtariQuotientCountState.PrototypeAssumption,
            useRn000fDiagnostic: true);
        Assert.Equal(((short)-3, (short)-3), (negative.SourceX, negative.SourceY));
        Assert.Equal(((short)3, (short)3), (positive.SourceX, positive.SourceY));
    }

    [Fact]
    public void Record06ConditionalEndpointsKeepTheSameNearAndFarPositions()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariCoordinateProjection near = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 16, 0, record.ScreenYOffset,
            AtariQuotientCountState.PrototypeAssumption,
            useRn000fDiagnostic: true);
        AtariCoordinateProjection far = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 240, 0, record.ScreenYOffset,
            AtariQuotientCountState.PrototypeAssumption,
            useRn000fDiagnostic: true);

        Assert.Equal((339, 179, (short)339, (short)35, new AtariDvgPoint(35, 339)),
            (near.QuotientX, near.QuotientY, near.SourceX, near.SourceY, near.DvgPoint));
        Assert.Equal((51, 27, (short)51, (short)-117, new AtariDvgPoint(-117, 51)),
            (far.QuotientX, far.QuotientY, far.SourceX, far.SourceY, far.DvgPoint));
    }

    [Theory]
    [InlineData(0x7F, 0x67, -6, -6)]
    [InlineData(0x81, 0x69, 6, 6)]
    public void ProjectorRestoresSignsAroundBothSourceAxes(
        byte rawX, byte rawY, short expectedX, short expectedY)
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariCoordinateProjection actual = AtariCoordinateProjector.Project(
            record, rawX, rawY, 16, 0, 0,
            AtariQuotientCountState.PrototypeAssumption);

        Assert.Equal(expectedX, actual.SourceX);
        Assert.Equal(expectedY, actual.SourceY);
    }

    [Fact]
    public void ProjectorAppliesBasesSaturatesSigned16AndPacksSigned13InDvgAxisOrder()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariCoordinateProjection based = AtariCoordinateProjector.Project(
            record, 0xEA, 0xA0, 16, 10, -154,
            AtariQuotientCountState.PrototypeAssumption);
        Assert.Equal((short)688, based.SourceX);
        Assert.Equal((short)204, based.SourceY);
        Assert.Equal(new AtariDvgPoint(204, 688), based.DvgPoint);

        AtariCoordinateProjection positiveOverflow = AtariCoordinateProjector.Project(
            record, 0xFF, 0xA0, 0, short.MaxValue, 0,
            AtariQuotientCountState.PrototypeAssumption);
        AtariCoordinateProjection negativeOverflow = AtariCoordinateProjector.Project(
            record, 0x00, 0x00, 0, short.MinValue, 0,
            AtariQuotientCountState.PrototypeAssumption);
        Assert.Equal(short.MaxValue, positiveOverflow.SourceX);
        Assert.Equal(short.MinValue, negativeOverflow.SourceX);

        AtariCoordinateProjection packing = AtariCoordinateProjector.Project(
            record, 0x80, 0x68, 16, 4096, -4097,
            AtariQuotientCountState.PrototypeAssumption);
        Assert.Equal(new AtariDvgPoint(4095, -4096), packing.DvgPoint);
        Assert.Equal(4095, AtariCoordinateProjector.PackSigned13(4095));
        Assert.Equal(-4096, AtariCoordinateProjector.PackSigned13(4096));
        Assert.Equal(4095, AtariCoordinateProjector.PackSigned13(-4097));
    }

    [Fact]
    public void ProjectorRejectsUnsupportedRuntimeAndQuotientCountState()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariQuotientCountState assumption = AtariQuotientCountState.PrototypeAssumption;
        Assert.Equal((byte)0x0F, assumption.LowByte);
        Assert.Equal((byte)0x00, assumption.HighByte);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.Project(record, 0x80, 0x68, -1, 0, 0, assumption));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.Project(record, 0x80, 0x68, 256, 0, 0, assumption));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.Project(record, 0x80, 0x68, 16, 0, 0, new(0x0F, 0x01)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.Project(record, 0x80, 0x68, 16, 0, 0, new(0x0E, 0x00)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.CalculateControlledQuotient(1, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.CalculateControlledQuotient(256, 0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariCoordinateProjector.CalculateControlledQuotient(1, ushort.MaxValue, 1));
    }

    [Fact]
    public void Record06VectorListPreservesRowsClosedRimsRailsAndDvgState()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariTubeVectorList vectors = AtariTubeVectorListBuilder.Build(record);
        AtariTubeVectorList repeated = AtariTubeVectorListBuilder.Build(record);

        Assert.Equal(16, vectors.NearPoints.Count);
        Assert.Equal(16, vectors.FarPoints.Count);
        Assert.Equal(16, vectors.Rails.Count);
        Assert.Equal(16, vectors.NearRim.Count);
        Assert.Equal(16, vectors.FarRim.Count);
        Assert.Equal(16, vectors.NearRuntimePosition);
        Assert.Equal(240, vectors.FarRuntimePosition);
        Assert.Equal(new AtariDvgPoint(0, 0), vectors.DvgCenter);
        Assert.Equal(0x8040, vectors.DvgCenterWord);
        Assert.Equal(0x7100, vectors.DvgVectorScaleWord);
        Assert.Equal(vectors.NearPoints, repeated.NearPoints);
        Assert.Equal(vectors.FarPoints, repeated.FarPoints);
        Assert.Equal(vectors.Rails, repeated.Rails);
        Assert.Equal(vectors.NearRim, repeated.NearRim);
        Assert.Equal(vectors.FarRim, repeated.FarRim);

        for (int index = 0; index < record.PointCount; index++)
        {
            AtariCoordinateProjection expectedNear = AtariCoordinateProjector.Project(
                record, record.RawXPoints[index], record.RawYPoints[index], 16, 0,
                record.ScreenYOffset, AtariQuotientCountState.PrototypeAssumption);
            AtariCoordinateProjection expectedFar = AtariCoordinateProjector.Project(
                record, record.RawXPoints[index], record.RawYPoints[index], 240, 0,
                record.ScreenYOffset, AtariQuotientCountState.PrototypeAssumption);
            Assert.Equal(expectedNear.DvgPoint, vectors.NearPoints[index]);
            Assert.Equal(expectedFar.DvgPoint, vectors.FarPoints[index]);
            Assert.Equal(
                new AtariTubeVectorSegment(vectors.NearPoints[index], vectors.FarPoints[index]),
                vectors.Rails[index]);
        }

        Assert.Equal(new AtariTubeVectorSegment(vectors.NearPoints[15], vectors.NearPoints[0]),
            vectors.NearRim[15]);
        Assert.Equal(new AtariTubeVectorSegment(vectors.FarPoints[15], vectors.FarPoints[0]),
            vectors.FarRim[15]);
    }

    [Fact]
    public void ConditionalDiagnosticKeepsRecord06PointOrderAndSameIndexTopology()
    {
        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariTubeVectorList vectors = AtariTubeVectorListBuilder.Build(
            record, useRn000fDiagnostic: true);

        Assert.Equal(16, vectors.NearPoints.Count);
        Assert.Equal(16, vectors.FarPoints.Count);
        Assert.Equal(16, vectors.Rails.Count);
        Assert.Equal(16, vectors.NearRim.Count);
        Assert.Equal(16, vectors.FarRim.Count);
        Assert.Equal(AtariTubeVectorListBuilder.NormalNearRuntimePosition,
            vectors.NearRuntimePosition);
        Assert.Equal(AtariTubeVectorListBuilder.NormalFarRuntimePosition,
            vectors.FarRuntimePosition);
        Assert.Equal(new AtariDvgPoint(0, 0), vectors.DvgCenter);

        for (int index = 0; index < record.PointCount; index++)
        {
            AtariCoordinateProjection near = AtariCoordinateProjector.Project(
                record, record.RawXPoints[index], record.RawYPoints[index], 16, 0,
                record.ScreenYOffset, AtariQuotientCountState.PrototypeAssumption,
                useRn000fDiagnostic: true);
            AtariCoordinateProjection far = AtariCoordinateProjector.Project(
                record, record.RawXPoints[index], record.RawYPoints[index], 240, 0,
                record.ScreenYOffset, AtariQuotientCountState.PrototypeAssumption,
                useRn000fDiagnostic: true);
            Assert.Equal(near.DvgPoint, vectors.NearPoints[index]);
            Assert.Equal(far.DvgPoint, vectors.FarPoints[index]);
            Assert.Equal(
                new AtariTubeVectorSegment(vectors.NearPoints[index], vectors.FarPoints[index]),
                vectors.Rails[index]);
            Assert.Equal(
                new AtariTubeVectorSegment(
                    vectors.NearPoints[index], vectors.NearPoints[(index + 1) % record.PointCount]),
                vectors.NearRim[index]);
            Assert.Equal(
                new AtariTubeVectorSegment(
                    vectors.FarPoints[index], vectors.FarPoints[(index + 1) % record.PointCount]),
                vectors.FarRim[index]);
        }
    }

    [Fact]
    public void ProvisionalViewportMapIsSharedFiniteDeterministicAndContainsRecord06()
    {
        AtariDvgViewportMap map = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        AtariDvgPoint origin = new(0, 0);
        Assert.Equal(
            new Vector2(DisplayConfiguration.PortraitLogicalWidth / 2f,
                DisplayConfiguration.PortraitLogicalHeight / 2f),
            map.Map(origin));
        Assert.Equal(map.Map(new AtariDvgPoint(214, 678)), map.Map(new AtariDvgPoint(214, 678)));
        Assert.True(float.IsFinite(map.GlobalScale));
        Assert.True(float.IsFinite(map.DisplayAspect));
        Assert.True(map.DisplayAspect > 0);

        AtariTubeVectorList vectors =
            AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
        foreach (AtariDvgPoint point in vectors.NearPoints.Concat(vectors.FarPoints))
        {
            Vector2 mapped = map.Map(point);
            Assert.InRange(mapped.X, 0, DisplayConfiguration.PortraitLogicalWidth);
            Assert.InRange(mapped.Y, 0, DisplayConfiguration.PortraitLogicalHeight);
        }
    }

    [Fact]
    public void ReferenceShapeDiagnosticConvertsSharedFitAndFarOffsetToLogicalViewportSpace()
    {
        Assert.Equal(0.4215f, AtariLevel7Presentation.ReferenceHorizontalGain);
        Assert.Equal(0.575f, AtariLevel7Presentation.ReferenceVerticalGain);
        Assert.Equal(0.7330435f,
            AtariTubeReferenceShapeDiagnostic.SharedHorizontalAspect);
        Assert.Equal(0.345f, AtariTubeReferenceShapeDiagnostic.SharedGlobalScale);
        Assert.Equal(33.75f, AtariTubeReferenceShapeDiagnostic.FarTranslationLogicalX);
        Assert.Equal(46.25f, AtariTubeReferenceShapeDiagnostic.FarTranslationLogicalY);
        Assert.Equal(AtariTubeReferenceShapeDiagnostic.SharedHorizontalAspect,
            AtariLevel7Presentation.SharedHorizontalAspect);
        Assert.Equal(AtariTubeReferenceShapeDiagnostic.SharedGlobalScale,
            AtariLevel7Presentation.SharedGlobalScale);
        Assert.Equal(AtariTubeReferenceShapeDiagnostic.FarTranslationLogicalX,
            AtariLevel7Presentation.FarTranslationLogicalX);
        Assert.Equal(AtariTubeReferenceShapeDiagnostic.FarTranslationLogicalY,
            AtariLevel7Presentation.FarTranslationLogicalY);

        Assert.Equal(new Vector2(354.1206f, 166.09f),
            AtariTubeReferenceShapeDiagnostic.MapNear(new AtariDvgPoint(214, 678)));
        Assert.Equal(new Vector2(310.989f, 411.06f),
            AtariTubeReferenceShapeDiagnostic.MapFar(new AtariDvgPoint(-90, 102)));

        Vector2 nearOrigin = AtariTubeReferenceShapeDiagnostic.MapNear(new AtariDvgPoint(0, 0));
        Vector2 farOrigin = AtariTubeReferenceShapeDiagnostic.MapFar(new AtariDvgPoint(0, 0));
        Assert.Equal(new Vector2(300f, 400f), nearOrigin);
        Assert.Equal(new Vector2(333.75f, 446.25f), farOrigin);
        Assert.Equal(new Vector2(33.75f, 46.25f), farOrigin - nearOrigin);

        Vector2 nearX = AtariTubeReferenceShapeDiagnostic.MapNear(new AtariDvgPoint(10, 0));
        Vector2 nearY = AtariTubeReferenceShapeDiagnostic.MapNear(new AtariDvgPoint(0, 10));
        Vector2 farX = AtariTubeReferenceShapeDiagnostic.MapFar(new AtariDvgPoint(10, 0));
        Vector2 farY = AtariTubeReferenceShapeDiagnostic.MapFar(new AtariDvgPoint(0, 10));
        Assert.Equal(nearX - nearOrigin, farX - farOrigin);
        Assert.Equal(nearY - nearOrigin, farY - farOrigin);
        Assert.InRange(MathF.Abs((nearX - nearOrigin).X - 2.529f), 0, 0.0001f);
        Assert.Equal(0, (nearX - nearOrigin).Y);
        Assert.Equal(0, (nearY - nearOrigin).X);
        Assert.InRange(MathF.Abs((nearY - nearOrigin).Y + 3.45f), 0, 0.0001f);
    }

    [Fact]
    public void ViewportMapRejectsInvalidConfigurationAndNonfiniteResults()
    {
        Assert.Throws<InvalidOperationException>(() =>
            default(AtariDvgViewportMap).Map(new AtariDvgPoint(0, 0)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgViewportMap(new(float.NaN, 0), 1, 1, 0, false, 1, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgViewportMap(Vector2.Zero, 0, 1, 0, false, 1, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgViewportMap(Vector2.Zero, 1, float.PositiveInfinity, 0, false, 1, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgViewportMap(Vector2.Zero, 1, 1, float.NaN, false, 1, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgViewportMap(Vector2.Zero, 1, 1, 0, false, 0, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AtariDvgPoint(4096, 0));

        AtariDvgViewportMap overflowing = new(
            Vector2.Zero, float.MaxValue, 1, 0, false, 1, -1);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            overflowing.Map(new AtariDvgPoint(4095, 0)));
    }

    [Fact]
    public void ViewportMapAppliesSharedAxisPermutationSignsRotationAndAspect()
    {
        AtariDvgViewportMap map = new(
            new Vector2(300, 400), 2, 0.5f, 90, true, -1, 1);
        Vector2 actual = map.Map(new AtariDvgPoint(1, 2));

        Assert.InRange(actual.X, 298 - 0.001f, 298 + 0.001f);
        Assert.InRange(actual.Y, 398 - 0.001f, 398 + 0.001f);
    }

    [Fact]
    public void PrototypeOptionIsOptInAndRejectsDuplicateValuesAndIncompatiblePresentationModes()
    {
        Assert.True(DebugController.TryParseAtariTubePrototype(
            [], out bool omitted, out string? omittedError));
        Assert.False(omitted);
        Assert.Null(omittedError);

        Assert.True(DebugController.TryParseAtariTubePrototype(
            ["--atari-tube-prototype"], out bool enabled, out string? enabledError));
        Assert.True(enabled);
        Assert.Null(enabledError);

        string[][] invalid =
        [
            ["--atari-tube-prototype", "--atari-tube-prototype"],
            ["--atari-tube-prototype", "unexpected"],
            ["--atari-tube-prototype", "--atari-presentation"],
            ["--atari-tube-prototype", "--legacy-presentation"],
            ["--atari-tube-prototype", "--portrait-perspective", "0"]
        ];
        foreach (string[] arguments in invalid)
        {
            Assert.False(DebugController.TryParseAtariTubePrototype(
                arguments, out bool parsed, out string? error));
            Assert.False(parsed);
            Assert.NotNull(error);
        }
    }

    [Fact]
    public void ConditionalRn000fDiagnosticOptionIsOptInAndRequiresThePrototype()
    {
        Assert.True(DebugController.TryParseAtariTubeRn000fDiagnostic(
            ["--level", "7", "--atari-tube-prototype"],
            out bool omitted, out string? omittedError));
        Assert.False(omitted);
        Assert.Null(omittedError);

        string[] selected =
        [
            "--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic"
        ];
        Assert.True(DebugController.TryParseAtariTubeRn000fDiagnostic(
            selected, out bool enabled, out string? enabledError));
        Assert.True(enabled);
        Assert.Null(enabledError);
        Assert.True(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, false, false, out string? contextError,
            rn000fDiagnosticEnabled: enabled));
        Assert.Null(contextError);

        string[][] invalid =
        [
            ["--atari-tube-rn000f-diagnostic"],
            ["--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic",
                "--atari-tube-rn000f-diagnostic"],
            ["--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic",
                "unexpected"],
            ["--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic",
                "--atari-presentation"],
            ["--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic",
                "--legacy-presentation"],
            ["--level", "7", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic",
                "--portrait-perspective", "0"]
        ];
        foreach (string[] arguments in invalid)
        {
            Assert.False(DebugController.TryParseAtariTubeRn000fDiagnostic(
                arguments, out bool parsed, out string? error));
            Assert.False(parsed);
            Assert.NotNull(error);
        }

        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            false, [], false, false, out string? noPrototypeError,
            rn000fDiagnosticEnabled: true));
        Assert.Contains("--atari-tube-prototype", noPrototypeError);

        string[][] invalidContexts =
        [
            ["--level", "6", "--atari-tube-prototype", "--atari-tube-rn000f-diagnostic"],
            ["--level", "7", "--level7-test", "--atari-tube-prototype",
                "--atari-tube-rn000f-diagnostic"]
        ];
        foreach (string[] arguments in invalidContexts)
            Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
                true, arguments, false, false, out _,
                rn000fDiagnosticEnabled: true));
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, true, false, out string? headlessError,
            rn000fDiagnosticEnabled: true));
        Assert.Contains("headless playback analysis", headlessError);
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, false, true, out string? inputError,
            rn000fDiagnosticEnabled: true));
        Assert.Contains("--input-diagnostic", inputError);
    }

    [Fact]
    public void ReferenceShapeDiagnosticOptionIsOptInAndRequiresAnExclusivePrototypeContext()
    {
        Assert.True(DebugController.TryParseAtariTubeReferenceShapeDiagnostic(
            ["--level", "7", "--atari-tube-prototype"],
            out bool omitted, out string? omittedError));
        Assert.False(omitted);
        Assert.Null(omittedError);

        string[] selected =
        [
            "--level", "7", "--atari-tube-prototype",
            "--atari-tube-reference-shape-diagnostic"
        ];
        Assert.True(DebugController.TryParseAtariTubeReferenceShapeDiagnostic(
            selected, out bool enabled, out string? enabledError));
        Assert.True(enabled);
        Assert.Null(enabledError);
        Assert.True(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, false, false, out string? contextError,
            referenceShapeDiagnosticEnabled: enabled));
        Assert.Null(contextError);

        string[][] invalid =
        [
            ["--atari-tube-reference-shape-diagnostic"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic",
                "--atari-tube-reference-shape-diagnostic"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic", "unexpected"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic",
                "--atari-tube-rn000f-diagnostic"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic", "--atari-presentation"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic", "--legacy-presentation"],
            ["--level", "7", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic", "--portrait-perspective", "0"]
        ];
        foreach (string[] arguments in invalid)
        {
            Assert.False(DebugController.TryParseAtariTubeReferenceShapeDiagnostic(
                arguments, out bool parsed, out string? error));
            Assert.False(parsed);
            Assert.NotNull(error);
        }

        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            false, selected, false, false, out string? noPrototypeError,
            referenceShapeDiagnosticEnabled: true));
        Assert.Contains("--atari-tube-prototype", noPrototypeError);
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, ["--level", "6", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic"],
            false, false, out _, referenceShapeDiagnosticEnabled: true));
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, ["--atari-tube-prototype", "--atari-tube-reference-shape-diagnostic"],
            false, false, out _, referenceShapeDiagnosticEnabled: true));
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, ["--level", "7", "--level7-test", "--atari-tube-prototype",
                "--atari-tube-reference-shape-diagnostic"],
            false, false, out _, referenceShapeDiagnosticEnabled: true));
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, true, false, out string? headlessError,
            referenceShapeDiagnosticEnabled: true));
        Assert.Contains("headless playback analysis", headlessError);
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, false, true, out string? inputError,
            referenceShapeDiagnosticEnabled: true));
        Assert.Contains("--input-diagnostic", inputError);
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            true, selected, false, false, out string? conflictingDiagnosticError,
            rn000fDiagnosticEnabled: true, referenceShapeDiagnosticEnabled: true));
        Assert.Contains("--atari-tube-rn000f-diagnostic", conflictingDiagnosticError);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "run.json", "--level", "7",
                "--atari-tube-prototype", "--atari-tube-reference-shape-diagnostic"],
            out _, out string? recordingError));
        Assert.Contains("not supported with gameplay recording or playback", recordingError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "run.json", "--level", "7",
                "--atari-tube-prototype", "--atari-tube-reference-shape-diagnostic"],
            out _, out string? playbackError));
        Assert.Contains("not supported with gameplay recording or playback", playbackError);
    }

    [Fact]
    public void PrototypeRequiresInteractiveOrdinaryDirectLevel7AndCannotUseFileModes()
    {
        const bool enabled = true;
        string[] directLevel7 = ["--level", "7", "--atari-tube-prototype"];
        Assert.True(DebugController.TryValidateAtariTubePrototypeContext(
            enabled, directLevel7, false, false, out string? validError));
        Assert.Null(validError);
        Assert.True(DebugController.TryParseGameplayFileCommand(
            directLevel7, out GameplayFileCommand? directLevel7FileCommand,
            out string? directLevel7FileError));
        Assert.Null(directLevel7FileCommand);
        Assert.Null(directLevel7FileError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            [], out GameplayFileCommand? noFileCommand, out string? noFileError));
        Assert.Null(noFileCommand);
        Assert.Null(noFileError);

        string[][] invalid =
        [
            ["--atari-tube-prototype"],
            ["--level", "6", "--atari-tube-prototype"],
            ["--level", "8", "--atari-tube-prototype"],
            ["--level", "7", "--level7-test", "--atari-tube-prototype"],
            ["--level", "7", "--record-gameplay", "run.json", "--atari-tube-prototype"],
            ["--level", "7", "--playback-file", "run.json", "--atari-tube-prototype"],
            ["--level", "7", "--record-gameplay", "run.json", "--atari-tube-prototype",
                "--atari-tube-rn000f-diagnostic"],
            ["--level", "7", "--lives", "5", "--atari-tube-prototype"]
        ];
        foreach (string[] arguments in invalid)
            Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
                enabled, arguments, false, false, out _));

        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            enabled, directLevel7, true, false, out string? headlessError));
        Assert.Contains("headless playback analysis", headlessError);
        Assert.False(DebugController.TryValidateAtariTubePrototypeContext(
            enabled, directLevel7, false, true, out string? diagnosticError));
        Assert.Contains("--input-diagnostic", diagnosticError);

        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "run.json", "--level", "7", "--atari-tube-prototype"],
            out _, out string? recordingConflictError));
        Assert.Contains("not supported with gameplay recording or playback",
            recordingConflictError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "run.json", "--atari-tube-prototype"],
            out _, out string? playbackConflictError));
        Assert.Contains("not supported with gameplay recording or playback",
            playbackConflictError);
        Assert.False(DebugController.TryParseGameplayFileCommand(
            ["--playback-file", "run.json", "--level", "7", "--atari-tube-prototype",
                "--atari-tube-rn000f-diagnostic"],
            out _, out string? diagnosticPlaybackConflictError));
        Assert.Contains("not supported with gameplay recording or playback",
            diagnosticPlaybackConflictError);
        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "run.json", "--level", "7"],
            out GameplayFileCommand? ordinaryRecord, out _));
        Assert.Equal(GameplayFileMode.Record, ordinaryRecord!.Mode);
    }

    [Fact]
    public void PrototypeRendererUsesOnlyRecord06AndLeavesGameplayModelUnchanged()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer prototype = new(game, atariTubePrototypeEnabled: true);
        VectorRenderer ordinary = new(game);
        Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
        int lane = game.Player.EdgeIndex;
        float edgePosition = game.Player.EdgePosition;
        float depth = game.Player.Depth;
        LevelState state = game.State;
        List<(Vector2 Start, Vector2 End, Color Color)> ordinaryLines = [];
        ordinary.DrawTube((start, end, color) => ordinaryLines.Add((start, end, color)));

        List<(Vector2 Start, Vector2 End, Color Color)> first = [];
        prototype.DrawTube((start, end, color) => first.Add((start, end, color)));
        List<(Vector2 Start, Vector2 End, Color Color)> second = [];
        prototype.DrawTube((start, end, color) => second.Add((start, end, color)));

        AtariTubeVectorList vectors =
            AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
        AtariDvgViewportMap map = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        IEnumerable<(Vector2 Start, Vector2 End, Color Color)> expected =
            vectors.Rails.Select(segment =>
                    (map.Map(segment.Start), map.Map(segment.End), Color.Blue))
                .Concat(vectors.NearRim.Select(segment =>
                    (map.Map(segment.Start), map.Map(segment.End), Color.Blue)))
                .Concat(vectors.FarRim.Select(segment =>
                    (map.Map(segment.Start), map.Map(segment.End), Color.Blue)));

        Assert.Equal(expected, first);
        Assert.Equal(first, second);
        Assert.Equal(48, first.Count);
        Assert.NotEqual(ordinaryLines[0].Start, first[0].Start);
        Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(lane, game.Player.EdgeIndex);
        Assert.Equal(edgePosition, game.Player.EdgePosition);
        Assert.Equal(depth, game.Player.Depth);
        Assert.Equal(state, game.State);

        List<(Vector2 Start, Vector2 End, Color Color)> disabled = [];
        new VectorRenderer(game, atariTubePrototypeEnabled: false).DrawTube(
            (start, end, color) => disabled.Add((start, end, color)));
        Assert.Equal(ordinaryLines, disabled);
    }

    [Fact]
    public void ConditionalDiagnosticSelectsOnlyTheLevel7PrototypePresentation()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer diagnostic = new(
            game, atariTubePrototypeEnabled: true, atariTubeRn000fDiagnosticEnabled: true);
        VectorRenderer control = new(game, atariTubePrototypeEnabled: true);
        Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
        int playerLane = game.Player.EdgeIndex;
        float playerPosition = game.Player.EdgePosition;
        float playerDepth = game.Player.Depth;
        LevelState state = game.State;

        Assert.True(diagnostic.UsesAtariTubePrototypeForCurrentLevel);
        Assert.True(diagnostic.UsesAtariTubeRn000fDiagnosticForCurrentLevel);
        List<(Vector2 Start, Vector2 End, Color Color)> actual = [];
        diagnostic.DrawTube((start, end, color) => actual.Add((start, end, color)));
        AtariTubeVectorList expectedVectors = AtariTubeVectorListBuilder.Build(
            AtariTubeRecordCatalog.Level7Record06, useRn000fDiagnostic: true);
        AtariDvgViewportMap viewport = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        IEnumerable<(Vector2 Start, Vector2 End, Color Color)> expected =
            expectedVectors.Rails.Select(edge =>
                    (viewport.Map(edge.Start), viewport.Map(edge.End), Color.Blue))
                .Concat(expectedVectors.NearRim.Select(edge =>
                    (viewport.Map(edge.Start), viewport.Map(edge.End), Color.Blue)))
                .Concat(expectedVectors.FarRim.Select(edge =>
                    (viewport.Map(edge.Start), viewport.Map(edge.End), Color.Blue)));
        Assert.Equal(expected, actual);

        List<(Vector2 Start, Vector2 End, Color Color)> controlLines = [];
        control.DrawTube((start, end, color) => controlLines.Add((start, end, color)));
        Assert.NotEqual(controlLines, actual);
        Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(playerLane, game.Player.EdgeIndex);
        Assert.Equal(playerPosition, game.Player.EdgePosition);
        Assert.Equal(playerDepth, game.Player.Depth);
        Assert.Equal(state, game.State);
        Assert.Throws<ArgumentException>(() => new VectorRenderer(
            game, atariTubeRn000fDiagnosticEnabled: true));

        TempestGame otherLevel = new(LevelConfiguration.Level6Production, random: new Random(1));
        VectorRenderer inactiveDiagnostic = new(
            otherLevel, atariTubePrototypeEnabled: true,
            atariTubeRn000fDiagnosticEnabled: true);
        VectorRenderer ordinary = new(otherLevel);
        Assert.False(inactiveDiagnostic.UsesAtariTubeRn000fDiagnosticForCurrentLevel);
        List<(Vector2 Start, Vector2 End, Color Color)> inactiveLines = [];
        List<(Vector2 Start, Vector2 End, Color Color)> ordinaryLines = [];
        inactiveDiagnostic.DrawTube(
            (start, end, color) => inactiveLines.Add((start, end, color)));
        ordinary.DrawTube((start, end, color) => ordinaryLines.Add((start, end, color)));
        Assert.Equal(ordinaryLines, inactiveLines);
    }

    [Fact]
    public void ReferenceShapeDiagnosticMapsTheSameEndpointSetsAcrossRailsAndClosedRims()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer diagnostic = new(
            game, atariTubePrototypeEnabled: true,
            atariTubeReferenceShapeDiagnosticEnabled: true);
        VectorRenderer control = new(game, atariTubePrototypeEnabled: true);
        Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
        int playerLane = game.Player.EdgeIndex;
        float playerPosition = game.Player.EdgePosition;
        float playerDepth = game.Player.Depth;
        LevelState state = game.State;

        Assert.True(diagnostic.UsesAtariTubePrototypeForCurrentLevel);
        Assert.True(diagnostic.UsesAtariTubeReferenceShapeDiagnosticForCurrentLevel);
        Assert.False(diagnostic.UsesAtariTubeRn000fDiagnosticForCurrentLevel);
        List<(Vector2 Start, Vector2 End, Color Color)> actual = [];
        diagnostic.DrawTube((start, end, color) => actual.Add((start, end, color)));

        AtariTubeVectorList vectors =
            AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
        Vector2[] near = vectors.NearPoints
            .Select(AtariTubeReferenceShapeDiagnostic.MapNear)
            .ToArray();
        Vector2[] far = vectors.FarPoints
            .Select(AtariTubeReferenceShapeDiagnostic.MapFar)
            .ToArray();
        IEnumerable<(Vector2 Start, Vector2 End, Color Color)> expected =
            vectors.Rails.Select((_, index) => (near[index], far[index], Color.Blue))
                .Concat(vectors.NearRim.Select((_, index) =>
                    (near[index], near[(index + 1) % near.Length], Color.Blue)))
                .Concat(vectors.FarRim.Select((_, index) =>
                    (far[index], far[(index + 1) % far.Length], Color.Blue)));
        Assert.Equal(expected, actual);
        Assert.Equal(48, actual.Count);
        Assert.Equal((near[0], far[0]), (actual[0].Start, actual[0].End));
        Assert.Equal((near[15], near[0]),
            (actual[16 + 15].Start, actual[16 + 15].End));
        Assert.Equal((far[15], far[0]),
            (actual[32 + 15].Start, actual[32 + 15].End));

        List<(Vector2 Start, Vector2 End, Color Color)> controlLines = [];
        control.DrawTube((start, end, color) => controlLines.Add((start, end, color)));
        AtariDvgViewportMap controlMap = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        IEnumerable<(Vector2 Start, Vector2 End, Color Color)> expectedControl =
            vectors.Rails.Select(edge =>
                    (controlMap.Map(edge.Start), controlMap.Map(edge.End), Color.Blue))
                .Concat(vectors.NearRim.Select(edge =>
                    (controlMap.Map(edge.Start), controlMap.Map(edge.End), Color.Blue)))
                .Concat(vectors.FarRim.Select(edge =>
                    (controlMap.Map(edge.Start), controlMap.Map(edge.End), Color.Blue)));
        Assert.Equal(expectedControl, controlLines);
        Assert.NotEqual(controlLines, actual);
        Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(playerLane, game.Player.EdgeIndex);
        Assert.Equal(playerPosition, game.Player.EdgePosition);
        Assert.Equal(playerDepth, game.Player.Depth);
        Assert.Equal(state, game.State);

        TempestGame otherLevel = new(LevelConfiguration.Level6Production, random: new Random(1));
        VectorRenderer inactiveDiagnostic = new(
            otherLevel, atariTubePrototypeEnabled: true,
            atariTubeReferenceShapeDiagnosticEnabled: true);
        VectorRenderer ordinary = new(otherLevel);
        Assert.False(inactiveDiagnostic.UsesAtariTubeReferenceShapeDiagnosticForCurrentLevel);
        List<(Vector2 Start, Vector2 End, Color Color)> inactiveLines = [];
        List<(Vector2 Start, Vector2 End, Color Color)> ordinaryLines = [];
        inactiveDiagnostic.DrawTube(
            (start, end, color) => inactiveLines.Add((start, end, color)));
        ordinary.DrawTube((start, end, color) => ordinaryLines.Add((start, end, color)));
        Assert.Equal(ordinaryLines, inactiveLines);

        Assert.Throws<ArgumentException>(() => new VectorRenderer(
            game, atariTubeReferenceShapeDiagnosticEnabled: true));
        Assert.Throws<ArgumentException>(() => new VectorRenderer(
            game, atariTubePrototypeEnabled: true,
            atariTubeRn000fDiagnosticEnabled: true,
            atariTubeReferenceShapeDiagnosticEnabled: true));
    }

    [Fact]
    public void AtariPresentationUsesRecord06AndTheReferenceShapeMappingForLevel7Tube()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer candidate = new(game, atariPresentationEnabled: true);
        Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();
        int playerLane = game.Player.EdgeIndex;
        float playerPosition = game.Player.EdgePosition;
        float playerDepth = game.Player.Depth;
        LevelState state = game.State;

        Assert.True(candidate.UsesAtariLevel7PresentationCandidateForCurrentLevel);
        Assert.False(candidate.UsesAtariTubePrototypeForCurrentLevel);
        Assert.False(candidate.UsesAtariTubeRn000fDiagnosticForCurrentLevel);
        Assert.False(candidate.UsesAtariTubeReferenceShapeDiagnosticForCurrentLevel);

        AtariTubeRecord record = AtariTubeRecordCatalog.Level7Record06;
        AtariTubeVectorList vectors = AtariTubeVectorListBuilder.Build(record);
        Vector2[] near = vectors.NearPoints
            .Select(AtariLevel7Presentation.MapNearCentered)
            .ToArray();
        Vector2[] far = vectors.FarPoints
            .Select(AtariLevel7Presentation.MapFarCentered)
            .ToArray();
        List<(Vector2 Start, Vector2 End, Color Color)> actual = [];
        candidate.DrawTube((start, end, color) => actual.Add((start, end, color)));

        Assert.Equal(6, vectors.RecordIndex);
        Assert.Equal(16, vectors.NearRuntimePosition);
        Assert.Equal(240, vectors.FarRuntimePosition);
        Assert.Equal(48, actual.Count);
        for (int index = 0; index < vectors.Rails.Count; index++)
        {
            Assert.Equal((near[index], far[index]),
                (actual[index].Start, actual[index].End));
            Assert.Equal((near[index], far[index]),
                (AtariLevel7Presentation.MapNearCentered(vectors.NearPoints[index]),
                    AtariLevel7Presentation.MapFarCentered(vectors.FarPoints[index])));

            int next = (index + 1) % near.Length;
            Assert.Equal((near[index], near[next]),
                (actual[16 + index * 2].Start, actual[16 + index * 2].End));
            Assert.Equal((far[index], far[next]),
                (actual[17 + index * 2].Start, actual[17 + index * 2].End));
        }

        Assert.Equal((near[15], near[0]),
            (actual[46].Start, actual[46].End));
        Assert.Equal((far[15], far[0]),
            (actual[47].Start, actual[47].End));
        Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(playerLane, game.Player.EdgeIndex);
        Assert.Equal(playerPosition, game.Player.EdgePosition);
        Assert.Equal(playerDepth, game.Player.Depth);
        Assert.Equal(state, game.State);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(9)]
    public void AtariLevel7PresentationCandidateDoesNotSelectOtherLevels(int displayedLevel)
    {
        TempestGame game = new(GetProductionConfiguration(displayedLevel), random: new Random(1));
        VectorRenderer candidate = new(game, atariPresentationEnabled: true);

        Assert.False(candidate.UsesAtariLevel7PresentationCandidateForCurrentLevel);
    }

    [Theory]
    [InlineData(LevelState.Approach)]
    [InlineData(LevelState.ExitDelay)]
    [InlineData(LevelState.ExitZoom)]
    public void AtariLevel7PresentationCandidateLeavesTransitionTubeDispatchUnchanged(
        LevelState transitionState)
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer candidate = new(game, atariPresentationEnabled: true);
        game.CurrentLevel.State = transitionState;

        Assert.False(candidate.UsesAtariLevel7PresentationCandidateForCurrentLevel);
    }

    [Theory]
    [InlineData(LevelState.Approach)]
    [InlineData(LevelState.ExitDelay)]
    [InlineData(LevelState.ExitZoom)]
    public void PrototypeMarksApproachAndExitStatesUnsupportedWithoutChangingGameState(
        LevelState unsupportedState)
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer prototype = new(game, atariTubePrototypeEnabled: true);
        game.CurrentLevel.State = unsupportedState;
        Vector2[] modelPoints = game.CurrentLevel.Definition.Points.ToArray();

        Assert.False(prototype.UsesAtariTubePrototypeForCurrentLevel);
        Assert.True(prototype.HasUnsupportedAtariTubePrototypeState);
        Assert.Equal(unsupportedState, game.State);
        Assert.Equal(modelPoints, game.CurrentLevel.Definition.Points.ToArray());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(9)]
    public void PrototypeDoesNotSelectAtariRecord06OutsideLevel7(int displayedLevel)
    {
        TempestGame game = new(GetProductionConfiguration(displayedLevel), random: new Random(1));
        VectorRenderer prototype = new(game, atariTubePrototypeEnabled: true);
        VectorRenderer ordinary = new(game);
        List<(Vector2 Start, Vector2 End, Color Color)> prototypeLines = [];
        List<(Vector2 Start, Vector2 End, Color Color)> ordinaryLines = [];

        prototype.DrawTube((start, end, color) => prototypeLines.Add((start, end, color)));
        ordinary.DrawTube((start, end, color) => ordinaryLines.Add((start, end, color)));

        Assert.Equal(ordinaryLines, prototypeLines);
    }

    [Fact]
    public void PrototypeLeavesExistingAtariPresentationAndGameOverTubeDispatchUnchanged()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        VectorRenderer existingCandidate = new(game, atariPresentationEnabled: true);
        VectorRenderer sameCandidateWithPrototypeDisabled =
            new(game, atariPresentationEnabled: true, atariTubePrototypeEnabled: false);
        List<(Vector2 Start, Vector2 End, Color Color)> first = [];
        List<(Vector2 Start, Vector2 End, Color Color)> second = [];
        existingCandidate.DrawTube((start, end, color) => first.Add((start, end, color)));
        sameCandidateWithPrototypeDisabled.DrawTube(
            (start, end, color) => second.Add((start, end, color)));
        Assert.Equal(first, second);

        game.CurrentLevel.State = LevelState.GameOver;
        VectorRenderer prototype = new(game, atariTubePrototypeEnabled: true);
        VectorRenderer ordinary = new(game);
        first.Clear();
        second.Clear();
        prototype.DrawTube((start, end, color) => first.Add((start, end, color)));
        ordinary.DrawTube((start, end, color) => second.Add((start, end, color)));
        Assert.Equal(second, first);
    }

    private static LevelConfiguration GetProductionConfiguration(int displayedLevel) =>
        displayedLevel switch
        {
            1 => LevelConfiguration.Level1,
            2 => LevelConfiguration.Level2Square,
            3 => LevelConfiguration.Level3Mixed,
            4 => LevelConfiguration.Level4Candidate,
            5 => LevelConfiguration.Level5Production,
            6 => LevelConfiguration.Level6Production,
            7 => LevelConfiguration.Level7Production,
            8 => LevelConfiguration.Level8Production,
            9 => LevelConfiguration.Level9Production,
            _ => throw new ArgumentOutOfRangeException(nameof(displayedLevel))
        };
}
