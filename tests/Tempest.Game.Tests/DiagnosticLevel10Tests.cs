using System.Numerics;
using Tempest.Game;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Tempest.Game.Systems;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class DiagnosticLevel10Tests
{
    [Fact]
    public void DirectLevelTenUsesRecoveredOpenGeometryAndRetainsLevelSevenGameplay()
    {
        LevelConfiguration level7 = LevelConfiguration.Level7Production;
        LevelConfiguration diagnostic = LevelConfiguration.Level10Diagnostic;
        TempestGame game = new(startingProductionLevel: 10, startingLives: 9);

        Assert.Equal((byte)9, LevelLoader.Level10RomEvidence.ShapeRemap);
        Assert.Equal(
            new byte[]
            {
                0x10, 0x10, 0x10, 0x10, 0x16, 0x29, 0x46, 0x69,
                0x97, 0xBA, 0xD7, 0xEA, 0xF0, 0xF0, 0xF0, 0xF0
            },
            LevelLoader.Level10RomEvidence.X);
        Assert.Equal(
            new byte[]
            {
                0xF0, 0xCB, 0xA6, 0x80, 0x5C, 0x39, 0x20, 0x12,
                0x12, 0x20, 0x39, 0x5C, 0x80, 0xA6, 0xCB, 0xF0
            },
            LevelLoader.Level10RomEvidence.Y);
        Assert.Equal(
            new byte[]
            {
                0x0C, 0x0C, 0x0C, 0x0D, 0x0E, 0x0F, 0x0F, 0x00,
                0x01, 0x01, 0x02, 0x03, 0x04, 0x04, 0x04, 0x00
            },
            LevelLoader.Level10RomEvidence.Angles);

        LevelDefinition definition = diagnostic.Definition;
        Assert.NotSame(level7.Definition, definition);
        Assert.False(definition.Points.SequenceEqual(level7.Definition.Points));
        Assert.True(level7.Definition.Continuous);
        Assert.Equal(16, level7.Definition.PlayableLaneCount);
        Assert.False(definition.Continuous);
        Assert.Equal(16, definition.VertexCount);
        Assert.Equal(15, definition.PlayableLaneCount);
        for (int pointIndex = 0; pointIndex < definition.VertexCount; pointIndex++)
        {
            Vector2 expected = new(
                (LevelLoader.Level10RomEvidence.X[pointIndex] - 128) * (25f / 14f),
                -(LevelLoader.Level10RomEvidence.Y[pointIndex] - 128) * (25f / 14f));
            Assert.Equal(expected, definition.Points[pointIndex]);
        }

        Assert.Equal(Enumerable.Range(0, 15), diagnostic.SpawnLaneSequence);
        Assert.True(diagnostic.UsesLevel7RomWavePolicy);
        Assert.True(diagnostic.UsesLevel7SpikerFiringPolicy);
        Assert.Equal(level7.EnemySupply, diagnostic.EnemySupply);
        Assert.Equal(level7.InitialActiveEggCount, diagnostic.InitialActiveEggCount);
        Assert.Equal(level7.ReserveReleaseIntervalTicks, diagnostic.ReserveReleaseIntervalTicks);
        Assert.Equal(level7.MaximumActiveEggs, diagnostic.MaximumActiveEggs);
        Assert.Equal(level7.MaximumApproachingEggs, diagnostic.MaximumApproachingEggs);
        Assert.Equal(level7.HatchSelectionIntervalTicks, diagnostic.HatchSelectionIntervalTicks);
        Assert.Equal(level7.EggDirectionSequence, diagnostic.EggDirectionSequence);
        Assert.Equal(level7.Flipper, diagnostic.Flipper);
        Assert.Equal(level7.FlipperPivotMode, diagnostic.FlipperPivotMode);
        Assert.Equal(level7.BelowRimFlipperProgram, diagnostic.BelowRimFlipperProgram);
        Assert.Equal(level7.ExitPolicy, diagnostic.ExitPolicy);
        Assert.Equal(level7.NormalEnemyComposition, diagnostic.NormalEnemyComposition);
        Assert.Equal(level7.FlipperShotHoldoffTicks, diagnostic.FlipperShotHoldoffTicks);
        Assert.Equal(level7.MaximumActiveEnemyShots, diagnostic.MaximumActiveEnemyShots);
        Assert.Equal(15, game.CurrentLevel.Spikes.Count);
        Assert.Equal(20, game.CurrentLevel.EnemySupply);
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.True(game.NaturalProgressionEnabled);
        Assert.Same(diagnostic, game.CurrentLevel.Configuration);

        game.CurrentLevel.State = LevelState.Complete;
        game.RunOneStep(restartPressed: true);
        Assert.Same(diagnostic, game.CurrentLevel.Configuration);
        Assert.True(game.NaturalProgressionEnabled);
    }

    [Fact]
    public void LevelTenAtariPresentationProjectsItsOpenModelGeometryWithoutLevelSevenAlias()
    {
        TempestGame level10Game = new(startingProductionLevel: 10);
        TempestGame level7Game = new(startingProductionLevel: 7);
        VectorRenderer renderer = new(level10Game, atariPresentationEnabled: true);
        LevelConfiguration configuration = LevelConfiguration.Level10Diagnostic;
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(10);

        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(10));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(10));
        Assert.Equal(9, metadata.TubeTableIndex);
        Assert.Equal((byte)0x18, metadata.ScaleRaw);
        Assert.Equal((byte)0x50, metadata.CameraHeightRaw);
        Assert.Equal((byte)0x20, metadata.ScreenYOffsetLowRaw);
        Assert.Equal((byte)0xFF, metadata.ScreenYOffsetHighRaw);
        Assert.Equal(unchecked((short)0xFF20), metadata.ScreenYOffsetSigned);
        Assert.Equal((byte)0xFF, metadata.OpenRaw);
        Assert.True(metadata.IsOpen);

        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTube((start, end, _) => lines.Add((start, end)));
        Vector2 ExpectedAtDepth(Vector2 point, float depth) =>
            AtariPresentationAdapter.Project(
                10, point.X, point.Y, depth,
                configuration.Definition.CenterY, configuration.Definition.YOffset,
                configuration.Position, configuration.Depth, 0, 0,
                Projection.K, Projection.ZOffset, metadata).Position;

        Assert.Equal(ExpectedAtDepth(configuration.Definition.Points[0],
            configuration.Position), lines[0].Start);
        Assert.Equal(ExpectedAtDepth(configuration.Definition.Points[0],
            configuration.Position + configuration.Depth), lines[0].End);

        bool HasSegment(Vector2 start, Vector2 end) => lines.Any(line =>
            (line.Start == start && line.End == end) ||
            (line.Start == end && line.End == start));
        Vector2 nearFirst = ExpectedAtDepth(configuration.Definition.Points[0],
            configuration.Position);
        Vector2 nearLast = ExpectedAtDepth(configuration.Definition.Points[15],
            configuration.Position);
        Vector2 farFirst = ExpectedAtDepth(configuration.Definition.Points[0],
            configuration.Position + configuration.Depth);
        Vector2 farLast = ExpectedAtDepth(configuration.Definition.Points[15],
            configuration.Position + configuration.Depth);

        Assert.False(HasSegment(nearLast, nearFirst));
        Assert.False(HasSegment(farLast, farFirst));
        Assert.Equal(15, level10Game.CurrentLevel.PlayableLaneCount);
    }

    [Fact]
    public void LevelTenVerticalPlacementMovesRenderedTubeDownAndPreservesXCenterAndEntityRegistration()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            TempestGame game = new(startingProductionLevel: 10);
            VectorRenderer renderer = new(game, atariPresentationEnabled: true);
            List<(Vector2 Start, Vector2 End)> lines = [];
            renderer.DrawTube((start, end, _) => lines.Add((start, end)));

            int vertexCount = game.CurrentLevel.Configuration.Definition.VertexCount;
            Vector2[] nearRim = lines.Take(vertexCount).Select(line => line.Start).ToArray();
            Vector2[] farRim = lines.Take(vertexCount).Select(line => line.End).ToArray();
            float nearCenterX = nearRim.Average(point => point.X);
            float farCenterX = farRim.Average(point => point.X);

            const float previousCandidateNearEndpointY = 125;
            Assert.Equal(50f, AtariPresentationAdapter.Level10VerticalPlacement);
            Assert.InRange(
                MathF.Abs(nearRim[0].Y -
                    (previousCandidateNearEndpointY +
                        AtariPresentationAdapter.Level10VerticalPlacement)),
                0, 0.02f);
            Assert.InRange(
                MathF.Abs(nearRim[0].Y - previousCandidateNearEndpointY -
                    AtariPresentationAdapter.Level10VerticalPlacement),
                0, 0.02f);

            Assert.InRange(MathF.Abs(nearCenterX - 400), 0, 0.001f);
            Assert.InRange(MathF.Abs(farCenterX - 400), 0, 0.001f);

            Vector2 projectedBullet = renderer.ProjectEnemyBullet(new EnemyBullet(0, 0));
            Vector2 expectedFarLaneCenter = (farRim[0] + farRim[1]) / 2;
            Assert.Equal(expectedFarLaneCenter, projectedBullet);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
        }
    }

    [Fact]
    public void DirectLevelTenStartupAndOneRecordingReplayInBothPresentationModes()
    {
        Assert.True(DebugController.TryParseStartingLevel(
            ["--level", "10"], out int? selectedLevel, out string? levelError));
        Assert.Equal(10, selectedLevel);
        Assert.Null(levelError);

        Assert.True(DebugController.TryParseGameplayFileCommand(
            ["--record-gameplay", "diagnostic.json", "--level", "10"],
            out GameplayFileCommand? command, out string? commandError));
        Assert.Equal(10, command!.StartingDisplayedLevel);
        Assert.Null(commandError);

        GameplayStartupConfiguration startup = new(10, 4, 1.0, 1981);
        PlayerInputRecorder recorder = new(startup);
        recorder.CaptureTick(0, new PlayerInput(true, 0, false, false));
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");
        try
        {
            GameplayRecordingFile.SaveNew(path, recorder);
            GameplayRecording recording = GameplayRecordingFile.LoadRecording(path);
            GameplayPlaybackSession standardPlayback =
                new(recording.StartupConfiguration, recording.Replay);
            GameplayPlaybackSession atariPlayback =
                new(recording.StartupConfiguration, recording.Replay);
            _ = new VectorRenderer(standardPlayback.Game);
            VectorRenderer atariRenderer = new(
                atariPlayback.Game, atariPresentationEnabled: true);

            Assert.Equal(10, recording.StartupConfiguration.StartingDisplayedLevel);
            Assert.Equal(1, recording.Replay.DurationTicks);
            Assert.Equal(recorder.Frames[0], recording.Replay.GetFrame(0));
            Assert.Same(LevelConfiguration.Level10Diagnostic,
                recording.Replay.StartingConfiguration);
            Assert.Same(LevelConfiguration.Level10Diagnostic,
                standardPlayback.Game.CurrentLevel.Configuration);
            Assert.Same(LevelConfiguration.Level10Diagnostic,
                atariPlayback.Game.CurrentLevel.Configuration);

            Assert.True(standardPlayback.StepForward());
            Assert.True(atariPlayback.StepForward());
            Assert.Equal(standardPlayback.CurrentBoundary, atariPlayback.CurrentBoundary);
            Assert.Equal(standardPlayback.Game.CurrentLevel.State,
                atariPlayback.Game.CurrentLevel.State);
            Assert.Equal(standardPlayback.Game.Player.EdgeIndex,
                atariPlayback.Game.Player.EdgeIndex);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
