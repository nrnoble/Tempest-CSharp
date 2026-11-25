using System.Numerics;
using System.Reflection;
using Raylib_cs;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class Level16AtariPresentationTests
{
    [Fact]
    public void DirectLevelSixteenAtariBlasterRendersOnItsInitialLane()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Assert.Equal(7, game.Player.EdgeIndex);
        Assert.Equal(0.5f, game.Player.EdgePosition);
        game.CurrentLevel.State = LevelState.Playing;
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);
        Vector2[] nearPoints = CaptureNearRimPoints(
            renderer, game.CurrentLevel.PlayableLaneCount);
        List<(Vector2 Start, Vector2 End, Color Color)> blasterLines = [];

        renderer.DrawPlayer(game.CurrentLevel.Definition.Points,
            (start, end, color) => blasterLines.Add((start, end, color)));

        Assert.Equal(6, blasterLines.Count);
        Assert.All(blasterLines, line => AssertFinite(line.Start, line.End));
        AssertBlasterFacesOutward(game, renderer, nearPoints, lane: 7);
    }

    [Fact]
    public void EveryLevelSixteenBlasterLaneHasFiniteOutwardClaws()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        game.CurrentLevel.State = LevelState.Playing;
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);
        Vector2[] nearPoints = CaptureNearRimPoints(
            renderer, game.CurrentLevel.PlayableLaneCount);
        int firstLobeWinding = GetLocalLobeWindingSign(nearPoints, 0);
        int secondLobeWinding = GetLocalLobeWindingSign(nearPoints, 8);
        HashSet<int> repeatedCenterAdjacentLanes =
            FindRepeatedVertexAdjacentLanes(nearPoints);

        Assert.Equal(-firstLobeWinding, secondLobeWinding);
        Assert.Equal(4, repeatedCenterAdjacentLanes.Count);
        Assert.Contains(0, repeatedCenterAdjacentLanes);
        Assert.Contains(7, repeatedCenterAdjacentLanes);
        Assert.Contains(8, repeatedCenterAdjacentLanes);
        Assert.Contains(15, repeatedCenterAdjacentLanes);
        for (int lane = 0; lane < game.CurrentLevel.PlayableLaneCount; lane++)
        {
            foreach (float position in new[] { 0.1f, 0.5f, 0.9f })
                AssertBlasterFacesOutward(game, renderer, nearPoints, lane, position);
        }
    }

    [Theory]
    [InlineData("tube-y-gain")]
    [InlineData("tube-scale")]
    [InlineData("tube-vertical-offset")]
    [InlineData("tube-depth-y-gain")]
    public void LevelSixteenBlasterOrientationSurvivesNonNeutralTubeControls(string control)
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        game.CurrentLevel.State = LevelState.Playing;
        VectorRenderer renderer = control switch
        {
            "tube-y-gain" => new(game, atariPresentationEnabled: true, tubeYGain: 1.25f),
            "tube-scale" => new(game, atariPresentationEnabled: true, tubeScale: 0.8f),
            "tube-vertical-offset" =>
                new(game, atariPresentationEnabled: true, tubeVerticalOffset: 23f),
            "tube-depth-y-gain" =>
                new(game, atariPresentationEnabled: true, tubeDepthYGain: 0.5f),
            _ => throw new ArgumentOutOfRangeException(nameof(control))
        };
        Vector2[] nearPoints = CaptureNearRimPoints(
            renderer, game.CurrentLevel.PlayableLaneCount);

        for (int lane = 0; lane < game.CurrentLevel.PlayableLaneCount; lane++)
        {
            foreach (float position in new[] { 0.1f, 0.5f, 0.9f })
                AssertBlasterFacesOutward(game, renderer, nearPoints, lane, position);
        }
    }

    [Fact]
    public void LevelSixteenPresentationDrawCallbacksCoverTubePlayerFlipperAndEntities()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        game.CurrentLevel.State = LevelState.Playing;
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);
        Level level = game.CurrentLevel;

        Flipper flipper = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(0, 15, 1, 10), cargoUnitId: 100,
            tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);
        for (int tick = 0; tick < 18; tick++)
            flipper.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, flipper.State);
        GetGameList<Flipper>(game, "flippers").Add(flipper);
        GetGameList<Tanker>(game, "tankers").Add(new Tanker(edgeIndex: 1, originalSlotId: 41));
        GetGameList<Fuseball>(game, "fuseballs").Add(new Fuseball(edgeIndex: 4));
        GetGameList<Shot>(game, "shots").Add(new Shot { EdgeIndex = 5, Height = 100 });
        GetGameList<EnemyBullet>(game, "enemyBullets")
            .Add(new EnemyBullet(edgeIndex: 6, height: 100));

        List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = [];
        List<(Vector2 Start, Vector2 End, Color Color)> playerLines = [];
        List<(Vector2 Start, Vector2 End)> flipperLines = [];
        List<(Vector2 Start, Vector2 End)> tankerLines = [];
        List<(Vector2 Start, Vector2 End, Color Color)> fuseballLines = [];
        List<(Vector2 Position, Vector2 Size)> eggPoints = [];
        List<(Vector2 Start, Vector2 End)> spikeLines = [];
        List<Vector2> spikeTips = [];
        List<(Vector2 Position, Color Color)> shotPoints = [];
        List<(Vector2 Start, Vector2 End, Color Color)> bulletLines = [];
        List<(Vector2 Position, Color Color)> bulletPoints = [];

        renderer.DrawTube((start, end, color) => tubeLines.Add((start, end, color)));
        renderer.DrawPlayer(level.Definition.Points,
            (start, end, color) => playerLines.Add((start, end, color)));
        renderer.DrawFlippers((start, end) => flipperLines.Add((start, end)));
        renderer.DrawTankers((start, end) => tankerLines.Add((start, end)));
        renderer.DrawFuseballs(
            (start, end, color) => fuseballLines.Add((start, end, color)));
        renderer.DrawEggs((position, size) => eggPoints.Add((position, size)));
        renderer.DrawSpikes(
            (start, end) => spikeLines.Add((start, end)), spikeTips.Add);
        renderer.DrawPlayerShots(
            (position, color) => shotPoints.Add((position, color)));
        renderer.DrawEnemyBullets(
            (start, end, color) => bulletLines.Add((start, end, color)),
            (position, color) => bulletPoints.Add((position, color)));

        Assert.Equal(48, tubeLines.Count);
        Assert.Equal(6, playerLines.Count);
        Assert.Equal(6, flipperLines.Count);
        Assert.NotEmpty(tankerLines);
        Assert.NotEmpty(fuseballLines);
        Assert.NotEmpty(eggPoints);
        Assert.NotEmpty(spikeLines);
        Assert.NotEmpty(spikeTips);
        Assert.NotEmpty(shotPoints);
        Assert.NotEmpty(bulletLines);

        Assert.All(tubeLines, line => AssertFinite(line.Start, line.End));
        Assert.All(playerLines, line => AssertFinite(line.Start, line.End));
        Assert.All(flipperLines, line => AssertFinite(line.Start, line.End));
        Assert.All(tankerLines, line => AssertFinite(line.Start, line.End));
        Assert.All(fuseballLines, line => AssertFinite(line.Start, line.End));
        Assert.All(eggPoints, item => AssertFinite(item.Position, item.Size));
        Assert.All(spikeLines, line => AssertFinite(line.Start, line.End));
        Assert.All(spikeTips, AssertFinite);
        Assert.All(shotPoints, item => AssertFinite(item.Position));
        Assert.All(bulletLines, line => AssertFinite(line.Start, line.End));
        Assert.All(bulletPoints, item => AssertFinite(item.Position));
    }

    [Fact]
    public void RowElevenModelFrameProjectsClosedTubeNearAndDeepWithoutChangingGeometry()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Assert.Same(LevelConfiguration.Level16Production, game.CurrentLevel.Configuration);

        Assert.True(AtariPresentationAdapter.HasModelFrameMapping(16));
        Assert.Equal((25f / 14f, -1),
            AtariPresentationAdapter.GetModelFrameMapping(16));
        RomPresentationMetadata metadata =
            RomPresentationMetadataCatalog.ForDisplayedLevel(16);
        Assert.Equal(11, metadata.TubeTableIndex);
        Assert.False(metadata.IsOpen);

        Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        new VectorRenderer(game, atariPresentationEnabled: true)
            .DrawTube((start, end, color) => lines.Add((start, end, color)));

        Assert.Equal(48, lines.Count);
        Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points);
        Assert.Equal(originalPoints[0], originalPoints[8]);
        Assert.Equal(lines[0].Start, lines[8].Start);
        Assert.Equal(lines[0].End, lines[8].End);
        Assert.NotEqual(lines[1].Start, lines[9].Start);
        Assert.NotEqual(lines[1].End, lines[9].End);
        Assert.All(lines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
        });
    }

    [Fact]
    public void LevelSixteenAcceptedCalibrationDefaultsRemainRuntimeOverrideable()
    {
        AtariPresentationCalibration accepted =
            new(1.1f, 1f, 100f, 1f);
        Assert.Equal(accepted, AtariPresentationCalibration.ForDisplayedLevel(16));
        Assert.Equal(accepted, AtariPresentationCalibration.Resolve(16));
        Assert.Equal(new AtariPresentationCalibration(1.25f, .8f, -12f, .5f),
            AtariPresentationCalibration.Resolve(
                16, tubeYGainOverride: 1.25f, tubeScaleOverride: .8f,
                verticalOffsetOverride: -12f, tubeDepthYGainOverride: .5f));

        TempestGame game = new(startingProductionLevel: 16);
        Assert.Equal(
            CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true)),
            CaptureTube(new VectorRenderer(
                game, atariPresentationEnabled: true,
                tubeYGain: 1.1f, tubeScale: 1f, tubeVerticalOffset: 100f,
                tubeDepthYGain: 1f)));
        Assert.NotEqual(
            CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true)),
            CaptureTube(new VectorRenderer(
                game, atariPresentationEnabled: true,
                tubeYGain: 1.25f, tubeScale: .8f, tubeVerticalOffset: -12f,
                tubeDepthYGain: .5f)));
    }

    [Fact]
    public void AtariPresentationCandidateDoesNotClaimLevelSeventeen()
    {
        Assert.Null(LevelProgression.GetProductionLevel(17));
        Assert.False(AtariPresentationAdapter.HasModelFrameMapping(17));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => AtariPresentationAdapter.GetModelFrameMapping(17));
    }

    [Fact]
    public void AllFourTubeOptionsParseForDirectLevelSixteenStartup()
    {
        string[] args =
        [
            "--start-level", "16", "--atari-presentation",
            "--tube-y-gain", "1.25", "--tube-scale", ".8",
            "--tube-vertical-offset", "-12", "--tube-depth-y-gain", ".5"
        ];

        Assert.True(DebugController.TryParseTubeYGainOptions(
            args, out TubeYGainOptions yGain, out string? yError));
        Assert.True(DebugController.TryParseTubeScaleOptions(
            args, out TubeScaleOptions scale, out string? scaleError));
        Assert.True(DebugController.TryParseTubeVerticalOffsetOptions(
            args, out TubeVerticalOffsetOptions offset, out string? offsetError));
        Assert.True(DebugController.TryParseTubeDepthYGainOptions(
            args, out TubeDepthYGainOptions depthYGain, out string? depthError));

        Assert.Null(yError);
        Assert.Null(scaleError);
        Assert.Null(offsetError);
        Assert.Null(depthError);
        Assert.Equal(1.25f, yGain.Gain);
        Assert.Equal(.8f, scale.Scale);
        Assert.Equal(-12f, offset.Offset);
        Assert.Equal(.5f, depthYGain.Gain);
        Assert.True(DebugController.TryParseStartingLevel(
            args, out int? level, out string? levelError));
        Assert.Equal(16, level);
        Assert.Null(levelError);
    }

    [Fact]
    public void TubeYGainChangesOnlyLevelSixteenProjectedCrossSectionY()
    {
        TempestGame game = new(startingProductionLevel: 16);
        var baseline = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));
        var adjusted = CaptureTube(new VectorRenderer(
            game, atariPresentationEnabled: true, tubeYGain: 1.4f));

        Assert.Equal(baseline.Count, adjusted.Count);
        Assert.Equal(baseline[1].Start.X, adjusted[1].Start.X);
        Assert.NotEqual(baseline[1].Start.Y, adjusted[1].Start.Y);
        Assert.NotEqual(baseline[1].End.Y, adjusted[1].End.Y);
    }

    [Fact]
    public void UniformScaleChangesLevelSixteenProjectedTubeAroundItsDepthAnchors()
    {
        TempestGame game = new(startingProductionLevel: 16);
        var baseline = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));
        var adjusted = CaptureTube(new VectorRenderer(
            game, atariPresentationEnabled: true, tubeScale: 1.25f));

        Assert.NotEqual(baseline[1].Start, adjusted[1].Start);
        Assert.NotEqual(baseline[1].End, adjusted[1].End);
    }

    [Fact]
    public void VerticalOffsetMovesLevelSixteenProjectionByTheRequestedAmount()
    {
        const float offset = 23f;
        float defaultOffset =
            AtariPresentationCalibration.ForDisplayedLevel(16).VerticalOffset;
        TempestGame game = new(startingProductionLevel: 16);
        var baseline = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));
        var adjusted = CaptureTube(new VectorRenderer(
            game, atariPresentationEnabled: true, tubeVerticalOffset: offset));

        Assert.Equal(baseline[1].Start.X, adjusted[1].Start.X);
        Assert.Equal(offset - defaultOffset,
            adjusted[1].Start.Y - baseline[1].Start.Y, precision: 3);
        Assert.Equal(offset - defaultOffset,
            adjusted[1].End.Y - baseline[1].End.Y, precision: 3);
    }

    [Fact]
    public void DepthYGainChangesOnlyLevelSixteenDepthDependentModelYProjection()
    {
        TempestGame game = new(startingProductionLevel: 16);
        var baseline = CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));
        var adjusted = CaptureTube(new VectorRenderer(
            game, atariPresentationEnabled: true, tubeDepthYGain: 0));

        Assert.Equal(baseline[1].Start, adjusted[1].Start);
        Assert.Equal(baseline[1].End.X, adjusted[1].End.X);
        Assert.NotEqual(baseline[1].End.Y, adjusted[1].End.Y);
        Assert.Equal(baseline[1].Start.Y, adjusted[1].Start.Y);
    }

    [Fact]
    public void SharedCenterFlippersCargoAndRegisteredEntitiesProjectFinite()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Level level = game.CurrentLevel;
        Vector2[] originalPoints = level.Definition.Points.ToArray();
        int playerLane = game.Player.EdgeIndex;
        float playerDepth = game.Player.Depth;
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);

        Tanker tanker = new(edgeIndex: 1, originalSlotId: 41);
        TankerReleaseResult release = tanker.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.Natural);
        foreach (TankerCargoSpawn cargo in release.Cargo)
        {
            Flipper cargoFlipper = Flipper.CreateTankerCargo(
                level, cargo, cargoUnitId: 82 + cargo.SourceLane,
                tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);
            (Vector2 start, Vector2 end) =
                renderer.ProjectFlipperEndpoints(cargoFlipper);
            AssertFinite(start);
            AssertFinite(end);
        }

        Flipper sharedCenterFlipper = Flipper.CreateTankerCargo(
            level, new TankerCargoSpawn(0, 15, 1, 10), cargoUnitId: 100,
            tankerCargoMovementProgram: TankerCargoMovementProgram.Repeating24);
        for (int tick = 0; tick < 18; tick++)
            sharedCenterFlipper.Update(level, playerEdge: 0);
        Assert.Equal(FlipperState.Flipping, sharedCenterFlipper.State);
        Assert.NotNull(sharedCenterFlipper.ActiveTransition?.RomAnglePath);
        (Vector2 sharedStart, Vector2 sharedEnd) =
            renderer.ProjectFlipperEndpoints(sharedCenterFlipper);
        AssertFinite(sharedStart);
        AssertFinite(sharedEnd);

        AssertFinite(renderer.ProjectTankerCenter(tanker));
        AssertFinite(renderer.ProjectFuseballCenter(new Fuseball(edgeIndex: 4)));
        AssertFinite(renderer.ProjectEgg(game.Eggs[0]));
        AssertFinite(renderer.ProjectEnemyBullet(new EnemyBullet(edgeIndex: 6, height: 100)));
        AssertFinite(renderer.ProjectShot(new Shot { EdgeIndex = 5, Height = 100 }));
        List<(Vector2 Start, Vector2 End)> spikeLines = [];
        List<Vector2> spikeTips = [];
        renderer.DrawSpikes((start, end) => spikeLines.Add((start, end)), spikeTips.Add);
        Assert.NotEmpty(spikeLines);
        Assert.All(spikeLines, line =>
        {
            AssertFinite(line.Start);
            AssertFinite(line.End);
        });
        Assert.All(spikeTips, AssertFinite);

        Assert.Equal(originalPoints, level.Definition.Points);
        Assert.Equal(playerLane, game.Player.EdgeIndex);
        Assert.Equal(playerDepth, game.Player.Depth);
    }

    [Fact]
    public void AtariPresentationDoesNotChangeLevelSixteenGameplayGeometryOrState()
    {
        TempestGame game = new(startingProductionLevel: 16, random: new Random(1981));
        Vector2[] geometry = game.CurrentLevel.Definition.Points.ToArray();
        int lane = game.Player.EdgeIndex;
        float depth = game.Player.Depth;
        int score = game.Score.Score;
        LevelState state = game.State;

        CaptureTube(new VectorRenderer(game, atariPresentationEnabled: true));

        Assert.Equal(geometry, game.CurrentLevel.Definition.Points);
        Assert.Equal(lane, game.Player.EdgeIndex);
        Assert.Equal(depth, game.Player.Depth);
        Assert.Equal(score, game.Score.Score);
        Assert.Equal(state, game.State);
    }

    private static void AssertBlasterFacesOutward(
        TempestGame game,
        VectorRenderer renderer,
        IReadOnlyList<Vector2> nearPoints,
        int lane,
        float edgePosition = 0.5f)
    {
        game.Player.ResetPosition(lane, edgePosition);
        IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
        renderer.GetPlayerEdge(points, out Vector2 nearStart, out Vector2 nearEnd);
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        int laneBefore = game.Player.EdgeIndex;
        float positionBefore = game.Player.EdgePosition;
        float depthBefore = game.Player.Depth;
        LevelState stateBefore = game.State;
        int scoreBefore = game.Score.Score;
        int shotCountBefore = game.Shots.Count;

        renderer.DrawPlayer(points,
            (start, end, color) => lines.Add((start, end, color)));

        Assert.Equal(6, lines.Count);
        Assert.All(lines, line => AssertFinite(line.Start, line.End));
        Vector2 tangent = nearEnd - nearStart;
        float tangentLength = tangent.Length();
        Assert.True(float.IsFinite(tangentLength) && tangentLength > 0);
        Vector2 leftNormal = new(-tangent.Y / tangentLength, tangent.X / tangentLength);
        int lobeWinding = GetLocalLobeWindingSign(nearPoints, lane);
        Vector2 expectedOutward = lobeWinding > 0 ? -leftNormal : leftNormal;
        Vector2 center = nearEnd - tangent * game.Player.EdgePosition;
        Vector2 clawOffset = lines[0].End - center;

        AssertFinite(clawOffset);
        Assert.True(clawOffset.Length() > 0);
        Assert.True(Vector2.Dot(clawOffset, expectedOutward) > 0,
            $"lane={lane}, lobe winding={lobeWinding}, offset={clawOffset}, expected={expectedOutward}");
        float sideError = expectedOutward.X * clawOffset.Y -
            expectedOutward.Y * clawOffset.X;
        Assert.InRange(MathF.Abs(sideError), 0, 0.001f);
        Assert.Equal(laneBefore, game.Player.EdgeIndex);
        Assert.Equal(positionBefore, game.Player.EdgePosition);
        Assert.Equal(depthBefore, game.Player.Depth);
        Assert.Equal(stateBefore, game.State);
        Assert.Equal(scoreBefore, game.Score.Score);
        Assert.Equal(shotCountBefore, game.Shots.Count);
    }

    private static Vector2[] CaptureNearRimPoints(VectorRenderer renderer, int pointCount)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = CaptureTube(renderer);
        Assert.Equal(pointCount * 3, tubeLines.Count);
        return tubeLines.Take(pointCount).Select(line => line.Start).ToArray();
    }

    private static int GetLocalLobeWindingSign(
        IReadOnlyList<Vector2> points, int lane)
    {
        int first = -1;
        int repeated = -1;
        for (int firstCandidate = 0; firstCandidate < points.Count; firstCandidate++)
        {
            for (int repeatedCandidate = firstCandidate + 2;
                 repeatedCandidate < points.Count;
                 repeatedCandidate++)
            {
                if ((firstCandidate == 0 && repeatedCandidate == points.Count - 1) ||
                    points[firstCandidate] != points[repeatedCandidate])
                    continue;

                Assert.True(first < 0, "Expected exactly one nonadjacent repeated vertex.");
                first = firstCandidate;
                repeated = repeatedCandidate;
            }
        }

        Assert.True(first >= 0, "Expected a repeated vertex defining local lobes.");
        double twiceArea = 0;
        if (lane >= first && lane < repeated)
        {
            for (int index = first; index < repeated; index++)
                twiceArea += Cross(points[index], points[index + 1]);
        }
        else
        {
            for (int index = repeated; index < points.Count - 1; index++)
                twiceArea += Cross(points[index], points[index + 1]);
            twiceArea += Cross(points[^1], points[0]);
            for (int index = 0; index < first; index++)
                twiceArea += Cross(points[index], points[index + 1]);
        }

        Assert.True(double.IsFinite(twiceArea) && Math.Abs(twiceArea) >= 0.0001);
        return Math.Sign(twiceArea);
    }

    private static HashSet<int> FindRepeatedVertexAdjacentLanes(
        IReadOnlyList<Vector2> points)
    {
        HashSet<int> lanes = [];
        for (int first = 0; first < points.Count; first++)
        {
            for (int repeated = first + 2; repeated < points.Count; repeated++)
            {
                if ((first == 0 && repeated == points.Count - 1) ||
                    points[first] != points[repeated])
                    continue;

                lanes.Add((first + points.Count - 1) % points.Count);
                lanes.Add(first);
                lanes.Add((repeated + points.Count - 1) % points.Count);
                lanes.Add(repeated);
            }
        }
        return lanes;
    }

    private static double Cross(Vector2 left, Vector2 right) =>
        (double)left.X * right.Y - (double)left.Y * right.X;

    private static List<T> GetGameList<T>(TempestGame game, string fieldName)
    {
        FieldInfo field = typeof(TempestGame).GetField(
            fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException(
                $"The game collection '{fieldName}' was not found.");
        return (List<T>)(field.GetValue(game)
            ?? throw new InvalidOperationException(
                $"The game collection '{fieldName}' is null."));
    }

    private static List<(Vector2 Start, Vector2 End, Color Color)> CaptureTube(
        VectorRenderer renderer)
    {
        List<(Vector2 Start, Vector2 End, Color Color)> lines = [];
        renderer.DrawTube((start, end, color) => lines.Add((start, end, color)));
        return lines;
    }

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }

    private static void AssertFinite(Vector2 start, Vector2 end)
    {
        AssertFinite(start);
        AssertFinite(end);
    }
}
