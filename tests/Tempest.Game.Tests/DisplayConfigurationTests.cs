using System.Numerics;
using Raylib_cs;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class DisplayConfigurationTests
{
    [Fact]
    public void LegacyPresentationFitsLogicalFourByThreeCanvasUniformly()
    {
        Assert.Equal(800, DisplayConfiguration.LogicalWidth);
        Assert.Equal(600, DisplayConfiguration.LogicalHeight);
        Assert.Equal(2f, DisplayConfiguration.WindowScale);
        Assert.Equal(1600, DisplayConfiguration.WindowWidth);
        Assert.Equal(1280, DisplayConfiguration.WindowHeight);
        Assert.Equal(new Vector2(800, 640),
            new Vector2(DisplayConfiguration.WindowWidth / 2f, DisplayConfiguration.WindowHeight / 2f));
        Assert.Equal(DisplayConfiguration.WindowWidth, VectorRenderer.ScreenWidth);
        Assert.Equal(DisplayConfiguration.WindowHeight, VectorRenderer.ScreenHeight);
        Assert.Equal(800f, Projection.Width);
        Assert.Equal(600f, Projection.Height);
        var camera = DisplayConfiguration.PresentationCamera;
        Assert.Equal(DisplayConfiguration.WindowScale, camera.Zoom);
        Assert.Equal(Vector2.Zero, camera.Target);
        Assert.Equal(new Vector2(0, 40), camera.Offset);
        Assert.Equal(0, camera.Rotation);
        Assert.Equal(new Vector2(800, 640),
            Raylib.GetWorldToScreen2D(new Vector2(400, 300), camera));
    }

    [Theory]
    [InlineData(0, 0, 0, 40)]
    [InlineData(800, 0, 1600, 40)]
    [InlineData(0, 600, 0, 1240)]
    [InlineData(800, 600, 1600, 1240)]
    [InlineData(400, 300, 800, 640)]
    [InlineData(640, 10, 1280, 60)]
    [InlineData(12.5f, 22.5f, 25, 85)]
    public void NativeCameraMapsLogicalCoordinatesToPhysicalCoordinates(
        float x, float y, float physicalX, float physicalY)
    {
        Vector2 logical = new(x, y);
        Vector2 physical = Raylib.GetWorldToScreen2D(logical, DisplayConfiguration.PresentationCamera);

        Assert.InRange(Vector2.Distance(new(physicalX, physicalY), physical), 0, .001f);
        Assert.InRange(Vector2.Distance(logical,
            Raylib.GetScreenToWorld2D(physical, DisplayConfiguration.PresentationCamera)), 0, .001f);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void PresentationPreservesEstablishedTubeCoordinatesAndProportions(int level)
    {
        LevelConfiguration configuration = ProductionConfiguration(level);
        TempestGame game = new(configuration, random: new Random(1));
        VectorRenderer renderer = new(game);
        Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
        List<(Vector2 Start, Vector2 End)> lines = [];
        renderer.DrawTube((start, end, _) => lines.Add((start, end)));

        Assert.Equal(70f, Projection.K);
        Assert.Equal(5f, Projection.ZOffset);
        Assert.Equal(
            originalPoints.Length + configuration.Definition.PlayableLaneCount * 2,
            lines.Count);
        for (int vertex = 0; vertex < originalPoints.Length; vertex++)
        {
            Vector2 point = originalPoints[vertex];
            Vector2 LegacyProjection(float depth) => new(
                400 + point.X / ((depth + 5) / 70),
                configuration.Definition.CenterY +
                    (point.Y + configuration.Definition.YOffset) / ((depth + 5) / 70));
            Assert.Equal(LegacyProjection(Level.Position), lines[vertex].Start);
            Assert.Equal(LegacyProjection(Level.Position + Level.Depth), lines[vertex].End);
        }

        foreach ((Vector2 start, Vector2 end) in lines)
        {
            Vector2 physicalStart = Raylib.GetWorldToScreen2D(start, DisplayConfiguration.PresentationCamera);
            Vector2 physicalEnd = Raylib.GetWorldToScreen2D(end, DisplayConfiguration.PresentationCamera);
            Vector2 expectedStart = start * DisplayConfiguration.WindowScale +
                DisplayConfiguration.PresentationCamera.Offset;
            Vector2 expectedEnd = end * DisplayConfiguration.WindowScale +
                DisplayConfiguration.PresentationCamera.Offset;
            Assert.InRange(Vector2.Distance(expectedStart, physicalStart), 0, .001f);
            Assert.InRange(Vector2.Distance(expectedEnd, physicalEnd), 0, .001f);
            Assert.InRange(Vector2.Distance(
                (end - start) * DisplayConfiguration.WindowScale,
                physicalEnd - physicalStart), 0, .001f);
            Assert.InRange(physicalStart.X, 0, DisplayConfiguration.WindowWidth);
            Assert.InRange(physicalStart.Y, 0, DisplayConfiguration.WindowHeight);
            Assert.InRange(physicalEnd.X, 0, DisplayConfiguration.WindowWidth);
            Assert.InRange(physicalEnd.Y, 0, DisplayConfiguration.WindowHeight);
        }
        Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(70f, Projection.K);
        Assert.Equal(5f, Projection.ZOffset);
    }

    [Fact]
    public void ExperimentalPortraitUsesTheConfiguredViewportAndUniformPlayfieldTransform()
    {
        Assert.Equal(600, DisplayConfiguration.PortraitLogicalWidth);
        Assert.Equal(800, DisplayConfiguration.PortraitLogicalHeight);
        Assert.Equal(960, DisplayConfiguration.PortraitViewportWidth);
        Assert.Equal(1280, DisplayConfiguration.PortraitViewportHeight);
        Assert.Equal(320, DisplayConfiguration.PortraitViewportLeft);
        Assert.Equal(new Rectangle(320, 0, 960, 1280), DisplayConfiguration.PortraitViewport);
        Assert.Equal(320, DisplayConfiguration.PortraitViewportLeft);
        Assert.Equal(320, DisplayConfiguration.WindowWidth -
            DisplayConfiguration.PortraitViewportLeft - DisplayConfiguration.PortraitViewportWidth);
        Assert.Equal(0f, DisplayConfiguration.PortraitViewport.Y);
        Assert.Equal(0, DisplayConfiguration.WindowHeight -
            DisplayConfiguration.PortraitViewportHeight);
        Assert.Equal(
            DisplayConfiguration.PortraitViewportWidth /
                (float)DisplayConfiguration.PortraitLogicalWidth,
            DisplayConfiguration.PortraitViewportHeight /
                (float)DisplayConfiguration.PortraitLogicalHeight);
        Assert.Equal(3f / 4f,
            DisplayConfiguration.PortraitViewportWidth /
            (float)DisplayConfiguration.PortraitViewportHeight);
        Assert.Equal(4f / 3f,
            DisplayConfiguration.PortraitViewportWidth / 720f);
        Assert.Equal(4f / 3f,
            DisplayConfiguration.PortraitViewportHeight / 960f);
        Assert.Equal(1.6f, DisplayConfiguration.PortraitOutputScale);
        Assert.Equal(1.2f * 4f / 3f, DisplayConfiguration.PortraitOutputScale);
        Assert.Equal(1.05f, DisplayConfiguration.PortraitPlayfieldScale);
        Assert.Equal(-33.1f, DisplayConfiguration.PortraitPlayfieldVerticalTranslation);
        Assert.Equal(1.68f, DisplayConfiguration.PortraitPlayfieldZoom);
        Assert.Equal(1f, DisplayConfiguration.PortraitViewportBorderThickness);

        Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
        Vector2 center = Raylib.GetWorldToScreen2D(new(400, 300), camera);
        Assert.InRange(Vector2.Distance(new(800, 451.04f), center), 0, .001f);
        Assert.Equal(new Vector2(800, 640), camera.Offset);
        Assert.InRange(MathF.Abs(camera.Target.Y - 412.4762f), 0, 0.0001f);

        Camera2D previousCamera = new()
        {
            Target = camera.Target,
            Offset = new Vector2(640, 480),
            Rotation = camera.Rotation,
            Zoom = 1.26f
        };
        Vector2[] logicalPoints =
        [
            Vector2.Zero, new(400, 300), new(800, 600), new(251.25f, 471.75f)
        ];
        foreach (Vector2 logicalPoint in logicalPoints)
        {
            Vector2 previous = Raylib.GetWorldToScreen2D(logicalPoint, previousCamera);
            Vector2 enlarged = Raylib.GetWorldToScreen2D(logicalPoint, camera);
            Vector2 expected = new Vector2(800, 640) +
                (previous - new Vector2(640, 480)) * (4f / 3f);
            Assert.InRange(Vector2.Distance(expected, enlarged), 0, .001f);
        }

        Assert.Equal(DisplayConfiguration.PresentationCamera.Zoom,
            DisplayConfiguration.GetPlayfieldCamera(PresentationMode.Legacy).Zoom);
    }

    [Fact]
    public void AcceptedAtariLevelPlacementsRemainLogicalCoordinates()
    {
        Assert.Equal(228.99702f, AtariPresentationAdapter.Level7VerticalPlacement);
        Assert.Equal(212.28139f, AtariPresentationAdapter.Level8VerticalPlacement);
        Assert.Equal(180.96825f, AtariPresentationAdapter.Level9VerticalPlacement);
        Assert.Equal(50f, AtariPresentationAdapter.Level10VerticalPlacement);
    }

    [Fact]
    public void AtariLogicalProjectionStillReceivesTheFrozenPortraitCameraPlacementAfterward()
    {
        LevelConfiguration configuration = LevelConfiguration.Level1;
        Vector2 logical = AtariPresentationAdapter.Project(
            1,
            configuration.Definition.Points[0].X,
            configuration.Definition.Points[0].Y,
            configuration.Position,
            configuration.Definition.CenterY,
            configuration.Definition.YOffset,
            configuration.Position,
            configuration.Depth,
            0,
            0,
            70,
            5,
            RomPresentationMetadataCatalog.ForDisplayedLevel(1)).Position;

        Camera2D placedCamera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
        Camera2D unplacedCamera = new()
        {
            Target = new Vector2(
                placedCamera.Target.X,
                DisplayConfiguration.WindowHeight / 2f / placedCamera.Zoom),
            Offset = placedCamera.Offset,
            Rotation = placedCamera.Rotation,
            Zoom = placedCamera.Zoom
        };
        Vector2 placed = Raylib.GetWorldToScreen2D(logical, placedCamera);
        Vector2 unplaced = Raylib.GetWorldToScreen2D(logical, unplacedCamera);

        Assert.Equal(-33.1f, DisplayConfiguration.PortraitPlayfieldVerticalTranslation);
        Assert.InRange(MathF.Abs(
            placed.Y - unplaced.Y -
            DisplayConfiguration.PortraitOutputScale *
            DisplayConfiguration.PortraitPlayfieldVerticalTranslation), 0, .001f);
    }

    [Fact]
    public void ExperimentalPortraitHudFillsOnlyTheCenteredPortraitViewport()
    {
        Camera2D camera = DisplayConfiguration.ExperimentalPortraitHudCamera;
        Vector2 topLeft = Raylib.GetWorldToScreen2D(Vector2.Zero, camera);
        Vector2 bottomRight = Raylib.GetWorldToScreen2D(
            new(DisplayConfiguration.PortraitLogicalWidth, DisplayConfiguration.PortraitLogicalHeight),
            camera);

        Assert.Equal(new Vector2(320, 0), topLeft);
        Assert.InRange(Vector2.Distance(new(1280, 1280), bottomRight), 0, .001f);

        Camera2D previousCamera = new()
        {
            Target = Vector2.Zero,
            Offset = new Vector2(280, 0),
            Rotation = 0,
            Zoom = 1.2f
        };
        Vector2[] logicalPoints =
        [
            Vector2.Zero, new(300, 400), new(600, 800), new(120, 60)
        ];
        foreach (Vector2 logicalPoint in logicalPoints)
        {
            Vector2 previous = Raylib.GetWorldToScreen2D(logicalPoint, previousCamera);
            Vector2 enlarged = Raylib.GetWorldToScreen2D(logicalPoint, camera);
            Vector2 expected = new Vector2(800, 640) +
                (previous - new Vector2(640, 480)) * (4f / 3f);
            Assert.InRange(Vector2.Distance(expected, enlarged), 0, .001f);
        }
    }

    [Fact]
    public void ExperimentalLevel9TubeRetainsPortraitViewportProminenceWithoutChangingModelPoints()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            TempestGame game = new(LevelConfiguration.Level9Production, random: new Random(1));
            Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
            List<(Vector2 Start, Vector2 End)> lines = [];
            new VectorRenderer(game).DrawTube((start, end, _) => lines.Add((start, end)));

            Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
            float left = lines.Take(originalPoints.Length)
                .Min(line => Raylib.GetWorldToScreen2D(line.Start, camera).X);
            float right = lines.Take(originalPoints.Length)
                .Max(line => Raylib.GetWorldToScreen2D(line.Start, camera).X);
            float viewportWidth = DisplayConfiguration.PortraitViewportWidth;

            Assert.True((right - left) / viewportWidth >= .88f);
            Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
            Assert.Equal(70f, Projection.K);
            Assert.Equal(5f, Projection.ZOffset);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void ExperimentalPortraitKeepsEveryProductionTubeOrientedAndInsideItsViewport(int level)
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            LevelConfiguration configuration = ProductionConfiguration(level);
            TempestGame game = new(configuration, random: new Random(1));
            Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
            List<(Vector2 Start, Vector2 End)> lines = [];
            new VectorRenderer(game).DrawTube((start, end, _) => lines.Add((start, end)));

            Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
            Vector2[] nearRim = lines.Take(originalPoints.Length)
                .Select(line => Raylib.GetWorldToScreen2D(line.Start, camera))
                .ToArray();
            Assert.Equal(Math.Sign(SignedArea(lines.Take(originalPoints.Length)
                    .Select(line => line.Start))),
                Math.Sign(SignedArea(nearRim)));

            foreach ((Vector2 start, Vector2 end) in lines)
            {
                AssertInsidePortraitViewport(Raylib.GetWorldToScreen2D(start, camera));
                AssertInsidePortraitViewport(Raylib.GetWorldToScreen2D(end, camera));
            }

            Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
            Assert.Same(configuration.Definition, game.CurrentLevel.Definition);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
        }
    }

    [Fact]
    public void PresentationModesDoNotRewriteProjectedTubeOrGameplayGeometry()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            TempestGame game = new(LevelConfiguration.Level9Production, random: new Random(1));
            Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
            List<(Vector2 Start, Vector2 End)> first = [];
            List<(Vector2 Start, Vector2 End)> second = [];

            new VectorRenderer(game).DrawTube((start, end, _) => first.Add((start, end)));
            new VectorRenderer(game).DrawTube((start, end, _) => second.Add((start, end)));

            Assert.Equal(first, second);
            Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
            Assert.Equal(originalPoints.Length, game.CurrentLevel.Definition.Points.Count);
            Assert.Equal(15, game.CurrentLevel.PlayableLaneCount);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
        }
    }

    [Fact]
    public void TubePlayerShotAndEnemyShotAnchorsStayRegisteredAcrossCameraStates()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.PortraitPerspective = 50;
            (int Level, int Lane)[] samples = [(1, 4), (6, 8), (9, 4)];
            (float K, float ZOffset)[] cameras =
            {
                (70, 5), (70, 3), (70, -41), (440, -41),
                (436, -40), (360, -21), (276, 0)
            };

            foreach ((int level, int lane) in samples)
            {
                LevelConfiguration configuration = ProductionConfiguration(level);
                TempestGame game = new(configuration, random: new Random(1));
                VectorRenderer renderer = new(game);
                IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
                int next = (lane + 1) % points.Count;
                float nearDepth = configuration.Position;
                float farDepth = configuration.Position + configuration.Depth;
                float depth = nearDepth + configuration.Depth * .42f;
                float height = farDepth - depth;

                foreach ((float k, float zOffset) in cameras)
                {
                    Projection.K = k;
                    Projection.ZOffset = zOffset;
                    List<(Vector2 Start, Vector2 End)> rails = [];
                    renderer.DrawTube((start, end, _) => rails.Add((start, end)));

                    float qNear = 1 / (nearDepth + zOffset);
                    float qFar = 1 / (farDepth + zOffset);
                    float fraction = (qNear - 1 / (depth + zOffset)) / (qNear - qFar);
                    Vector2 laneNear = (rails[lane].Start + rails[next].Start) / 2;
                    Vector2 laneFar = (rails[lane].End + rails[next].End) / 2;
                    Vector2 railAnchor = Vector2.Lerp(laneNear, laneFar, fraction);
                    Vector2 playerShotAnchor = renderer.ProjectShot(new Shot
                    {
                        EdgeIndex = lane,
                        Height = height
                    });
                    Vector2 enemyShotAnchor =
                        renderer.ProjectEnemyBullet(new EnemyBullet(lane, height));
                    Vector2 enemyCenter = renderer.ProjectFuseballCenter(new Fuseball(lane));

                    Assert.InRange(Vector2.Distance(railAnchor, playerShotAnchor), 0, .001f);
                    Assert.InRange(Vector2.Distance(playerShotAnchor, enemyShotAnchor), 0, .001f);
                    Assert.InRange(Vector2.Distance(enemyCenter, rails[lane].End), 0, .001f);

                    renderer.GetPlayerEdge(points, out Vector2 playerStart, out Vector2 playerEnd);
                    Vector2 expectedPlayerStart = Projection.Project(
                        points[game.Player.EdgeIndex].X,
                        points[game.Player.EdgeIndex].Y,
                        renderer.PlayerProjectedDepth,
                        game.CurrentLevel.Definition.CenterY,
                        game.CurrentLevel.Definition.YOffset,
                        nearDepth,
                        farDepth);
                    int nextPlayerLane = (game.Player.EdgeIndex + 1) % points.Count;
                    Vector2 expectedPlayerEnd = Projection.Project(
                        points[nextPlayerLane].X,
                        points[nextPlayerLane].Y,
                        renderer.PlayerProjectedDepth,
                        game.CurrentLevel.Definition.CenterY,
                        game.CurrentLevel.Definition.YOffset,
                        nearDepth,
                        farDepth);
                    Assert.InRange(Vector2.Distance(expectedPlayerStart, playerStart), 0, .001f);
                    Assert.InRange(Vector2.Distance(expectedPlayerEnd, playerEnd), 0, .001f);

                    if (level == 9)
                    {
                        List<(Vector2 Start, Vector2 Tip)> spikes = [];
                        renderer.DrawSpikes(
                            (start, tip) => spikes.Add((start, tip)), _ => { });
                        Assert.Equal(game.Spikes.Count, spikes.Count);
                        for (int spikeIndex = 0; spikeIndex < game.Spikes.Count; spikeIndex++)
                        {
                            Spike spike = game.Spikes[spikeIndex];
                            (Vector2 actualStart, Vector2 actualTip) = spikes[spikeIndex];
                            Vector2 midpoint = (points[spike.EdgeIndex] +
                                points[game.CurrentLevel.NextEdge(spike.EdgeIndex)]) / 2;
                            Vector2 expectedStart = Projection.Project(
                                midpoint.X, midpoint.Y, farDepth,
                                game.CurrentLevel.Definition.CenterY,
                                game.CurrentLevel.Definition.YOffset,
                                nearDepth, farDepth);
                            Vector2 expectedTip = Projection.Project(
                                midpoint.X, midpoint.Y, farDepth - spike.Height,
                                game.CurrentLevel.Definition.CenterY,
                                game.CurrentLevel.Definition.YOffset,
                                nearDepth, farDepth);
                            Assert.InRange(Vector2.Distance(expectedStart, actualStart), 0, .001f);
                            Assert.InRange(Vector2.Distance(expectedTip, actualTip), 0, .001f);
                        }
                    }
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void TubeProjectionUsesTheActiveConfigurationNearAndFarDepths()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = Projection.PortraitPerspectiveBaselineK;
            Projection.ZOffset = Projection.PortraitPerspectiveBaselineZOffset;
            Projection.PortraitPerspective = 50;
            LevelConfiguration configuration = LevelConfiguration.Level1 with
            {
                Position = 63,
                Depth = 421
            };
            TempestGame game = new(configuration, random: new Random(1));
            Vector2 point = game.CurrentLevel.Definition.Points[0];
            List<(Vector2 Start, Vector2 End)> lines = [];
            new VectorRenderer(game).DrawTube((start, end, _) => lines.Add((start, end)));

            Vector2 expectedNear = Projection.Project(
                point.X, point.Y, configuration.Position,
                configuration.Definition.CenterY, configuration.Definition.YOffset,
                configuration.Position, configuration.Position + configuration.Depth);
            Vector2 expectedFar = Projection.Project(
                point.X, point.Y, configuration.Position + configuration.Depth,
                configuration.Definition.CenterY, configuration.Definition.YOffset,
                configuration.Position, configuration.Position + configuration.Depth);

            Assert.InRange(Vector2.Distance(lines[0].Start, expectedNear), 0, .001f);
            Assert.InRange(Vector2.Distance(lines[0].End, expectedFar), 0, .001f);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void EndpointNormalizedPerspectiveMatchesThePreviousStableFormula()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = Projection.PortraitPerspectiveBaselineK;
            Projection.ZOffset = Projection.PortraitPerspectiveBaselineZOffset;
            foreach (float perspective in new[] { -5f, 0f, 5f })
            {
                Projection.PortraitPerspective = perspective;
                foreach (int level in new[] { 1, 6, 9 })
                {
                    LevelConfiguration configuration = ProductionConfiguration(level);
                    TempestGame game = new(configuration, random: new Random(1));
                    float nearDepth = configuration.Position;
                    float farDepth = nearDepth + configuration.Depth;
                    float[] depths =
                    [
                        nearDepth,
                        nearDepth + configuration.Depth * .25f,
                        nearDepth + configuration.Depth * .5f,
                        nearDepth + configuration.Depth * .75f,
                        farDepth,
                        nearDepth + 16,
                        nearDepth + 239,
                        farDepth + configuration.Depth
                    ];

                    foreach (Vector2 point in game.CurrentLevel.Definition.Points)
                    {
                        foreach (float depth in depths)
                        {
                            float denominator = depth + Projection.ZOffset;
                            float nearDenominator = nearDepth + Projection.ZOffset;
                            Vector2 expected = new(
                                Projection.Width / 2f + point.X / (denominator / Projection.K),
                                game.CurrentLevel.Definition.CenterY +
                                    (point.Y + game.CurrentLevel.Definition.YOffset) /
                                    (denominator / Projection.K) +
                                    perspective * Projection.K *
                                    (1 / denominator - 1 / nearDenominator));
                            Vector2 actual = Projection.Project(
                                point.X,
                                point.Y,
                                depth,
                                game.CurrentLevel.Definition.CenterY,
                                game.CurrentLevel.Definition.YOffset,
                                nearDepth,
                                farDepth);

                            Assert.InRange(Vector2.Distance(expected, actual), 0, .0001f);
                        }
                    }
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void EndpointNormalizedPerspectiveKeepsRailsStraightAcrossTransitionsAndApproach()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.PortraitPerspective = 50;
            (float K, float ZOffset, float ApproachOffset)[] contexts =
            [
                (70, 5, 0),
                (70, 3, 0),
                (70, -41, 0),
                (440, -41, 0),
                (436, -40, 0),
                (360, -21, 0),
                (276, 0, 0),
                (70, 5, 960),
                (70, 5, 32),
                (70, 5, 0)
            ];

            foreach (int level in new[] { 1, 6, 9 })
            {
                LevelConfiguration configuration = ProductionConfiguration(level);
                TempestGame game = new(configuration, random: new Random(1));
                foreach ((float k, float zOffset, float approachOffset) in contexts)
                {
                    Projection.K = k;
                    Projection.ZOffset = zOffset;
                    float nearDepth = configuration.Position + approachOffset;
                    float farDepth = nearDepth + configuration.Depth;
                    foreach (Vector2 point in game.CurrentLevel.Definition.Points)
                    {
                        Vector2 near = Projection.Project(
                            point.X, point.Y, nearDepth,
                            game.CurrentLevel.Definition.CenterY,
                            game.CurrentLevel.Definition.YOffset + approachOffset *
                                (VectorRenderer.InitialApproachVerticalOffset /
                                    LevelApproach.InitialDepthOffset),
                            nearDepth, farDepth);
                        Vector2 far = Projection.Project(
                            point.X, point.Y, farDepth,
                            game.CurrentLevel.Definition.CenterY,
                            game.CurrentLevel.Definition.YOffset + approachOffset *
                                (VectorRenderer.InitialApproachVerticalOffset /
                                    LevelApproach.InitialDepthOffset),
                            nearDepth, farDepth);
                        Vector2 rail = far - near;
                        float railLengthSquared = rail.LengthSquared();

                        foreach (float depth in new[]
                        {
                            nearDepth + configuration.Depth * .25f,
                            nearDepth + configuration.Depth * .5f,
                            nearDepth + configuration.Depth * .75f
                        })
                        {
                            Vector2 pointOnRail = Projection.Project(
                                point.X, point.Y, depth,
                                game.CurrentLevel.Definition.CenterY,
                                game.CurrentLevel.Definition.YOffset + approachOffset *
                                    (VectorRenderer.InitialApproachVerticalOffset /
                                        LevelApproach.InitialDepthOffset),
                                nearDepth, farDepth);
                            Vector2 fromNear = pointOnRail - near;
                            float perpendicularDistance = MathF.Abs(
                                rail.X * fromNear.Y - rail.Y * fromNear.X) /
                                MathF.Sqrt(railLengthSquared);
                            float fraction = Vector2.Dot(fromNear, rail) / railLengthSquared;

                            Assert.InRange(perpendicularDistance, 0, .001f);
                            Assert.InRange(fraction, 0, 1);
                        }
                    }
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void EndpointNormalizedCorrectionStaysWithinItsStableEndpointDisplacement()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.PortraitPerspective = 50;
            (float K, float ZOffset)[] cameras =
            [
                (70, 3), (70, -41), (440, -41),
                (436, -40), (360, -21), (276, 0)
            ];
            foreach ((float k, float zOffset) in cameras)
            {
                Projection.K = k;
                Projection.ZOffset = zOffset;
                const float nearDepth = 50;
                const float farDepth = 450;
                const float centerY = 300;

                Projection.PortraitPerspective = 0;
                float neutralNear = Projection.Project(
                    0, 0, nearDepth, centerY, 0, nearDepth, farDepth).Y;
                float neutralFar = Projection.Project(
                    0, 0, farDepth, centerY, 0, nearDepth, farDepth).Y;
                Projection.PortraitPerspective = 50;
                float correctedNear = Projection.Project(
                    0, 0, nearDepth, centerY, 0, nearDepth, farDepth).Y;
                float correctedFar = Projection.Project(
                    0, 0, farDepth, centerY, 0, nearDepth, farDepth).Y;

                float expectedFarCorrection = 50 * Projection.PortraitPerspectiveBaselineK *
                    (1 / (farDepth + Projection.PortraitPerspectiveBaselineZOffset) -
                        1 / (nearDepth + Projection.PortraitPerspectiveBaselineZOffset));
                Assert.InRange(MathF.Abs(correctedNear - neutralNear), 0, .0001f);
                Assert.InRange(MathF.Abs((correctedFar - neutralFar) - expectedFarCorrection), 0, .0001f);

                foreach (float depth in new[] { 150f, 250f, 350f })
                {
                    Projection.PortraitPerspective = 0;
                    float neutral = Projection.Project(
                        0, 0, depth, centerY, 0, nearDepth, farDepth).Y;
                    Projection.PortraitPerspective = 50;
                    float corrected = Projection.Project(
                        0, 0, depth, centerY, 0, nearDepth, farDepth).Y;
                    Assert.InRange(MathF.Abs(corrected - neutral),
                        0, MathF.Abs(expectedFarCorrection) + .0001f);
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void EndpointCorrectionRemainsDeterministicAndBoundedThroughExitReverseAndApproachSamples()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            List<(float K, float ZOffset, float ApproachOffset)> frames = [];
            for (int zOffset = 5; zOffset >= -41; zOffset -= 2)
                frames.Add((70, zOffset, 0));
            for (int k = 70; k <= 440; k += 2)
                frames.Add((k, -41, 0));

            int reverseK = 440;
            int reverseZOffset = -41;
            while (reverseK > 70 || reverseZOffset < 5)
            {
                reverseK = Math.Max(70, reverseK - 4);
                reverseZOffset = Math.Min(5, reverseZOffset + 1);
                frames.Add((reverseK, reverseZOffset, 0));
            }

            for (int tick = 0; tick <= LevelApproach.DurationTicks; tick++)
            {
                float t = tick / (float)LevelApproach.DurationTicks;
                float progress = t * t * (3 - 2 * t);
                frames.Add((70, 5, LevelApproach.InitialDepthOffset * (1 - progress)));
            }

            Projection.PortraitPerspective = 50;
            float previousMidCorrection = 0;
            float previousCorrectionBound = 0;
            bool hasPreviousMidCorrection = false;
            foreach ((float k, float zOffset, float approachOffset) in frames)
            {
                Projection.K = k;
                Projection.ZOffset = zOffset;
                float nearDepth = 50 + approachOffset;
                float farDepth = 450 + approachOffset;
                float middleDepth = (nearDepth + farDepth) / 2;

                float CorrectionAt(float depth)
                {
                    Projection.PortraitPerspective = 0;
                    float neutral = Projection.Project(
                        0, 0, depth, 300, 0, nearDepth, farDepth).Y;
                    Projection.PortraitPerspective = 50;
                    float corrected = Projection.Project(
                        0, 0, depth, 300, 0, nearDepth, farDepth).Y;
                    return corrected - neutral;
                }

                float nearCorrection = CorrectionAt(nearDepth);
                float middleCorrection = CorrectionAt(middleDepth);
                float farCorrection = CorrectionAt(farDepth);
                float expectedFarCorrection = 50 * Projection.PortraitPerspectiveBaselineK *
                    (1 / (farDepth + Projection.PortraitPerspectiveBaselineZOffset) -
                        1 / (nearDepth + Projection.PortraitPerspectiveBaselineZOffset));
                Assert.InRange(MathF.Abs(nearCorrection), 0, .0001f);
                Assert.InRange(MathF.Abs(farCorrection - expectedFarCorrection), 0, .0001f);
                Assert.InRange(MathF.Abs(middleCorrection),
                    0, MathF.Abs(expectedFarCorrection) + .0001f);

                Vector2 first = Projection.Project(
                    125, -17, middleDepth, 300, 0, nearDepth, farDepth);
                Vector2 repeated = Projection.Project(
                    125, -17, middleDepth, 300, 0, nearDepth, farDepth);
                Assert.Equal(first, repeated);
                if (hasPreviousMidCorrection)
                    Assert.InRange(MathF.Abs(middleCorrection - previousMidCorrection),
                        0, previousCorrectionBound + MathF.Abs(expectedFarCorrection) + .0001f);
                previousMidCorrection = middleCorrection;
                previousCorrectionBound = MathF.Abs(expectedFarCorrection);
                hasPreviousMidCorrection = true;
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void ProjectionRejectsSingularOrNonFiniteCameraContexts()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Projection.PortraitPerspective = 50;

            Assert.Throws<InvalidOperationException>(() =>
                Projection.Project(0, 0, 100, 300, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Projection.Project(0, 0, -5, 300, 0, 50, 450));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Projection.Project(0, 0, 100, 300, 0, -5, 450));
            Assert.Throws<ArgumentException>(() =>
                Projection.Project(0, 0, 100, 300, 0, 50, 50));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Projection.Project(float.NaN, 0, 100, 300, 0, 50, 450));

            Projection.K = float.NaN;
            Assert.Throws<InvalidOperationException>(() =>
                Projection.Project(0, 0, 100, 300, 0, 50, 450));
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Theory]
    [InlineData(-50)]
    [InlineData(-20)]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(50)]
    public void PortraitPerspectivePreservesAllProductionBoundsAndNearRimPlacement(
        float perspective)
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;

            for (int level = 1; level <= 9; level++)
            {
                LevelConfiguration configuration = ProductionConfiguration(level);
                TempestGame game = new(configuration, random: new Random(1));
                Vector2[] originalPoints = game.CurrentLevel.Definition.Points.ToArray();
                List<(Vector2 Start, Vector2 End)> neutralLines = [];
                Projection.PortraitPerspective = 0;
                new VectorRenderer(game).DrawTube(
                    (start, end, _) => neutralLines.Add((start, end)));

                List<(Vector2 Start, Vector2 End)> candidateLines = [];
                Projection.PortraitPerspective = perspective;
                new VectorRenderer(game).DrawTube(
                    (start, end, _) => candidateLines.Add((start, end)));

                Assert.Equal(neutralLines.Count, candidateLines.Count);
                for (int index = 0; index < neutralLines.Count; index++)
                {
                    (Vector2 neutralStart, Vector2 neutralEnd) = neutralLines[index];
                    (Vector2 candidateStart, Vector2 candidateEnd) = candidateLines[index];
                    Vector2 neutralStartScreen = Raylib.GetWorldToScreen2D(neutralStart, camera);
                    Vector2 neutralEndScreen = Raylib.GetWorldToScreen2D(neutralEnd, camera);
                    Vector2 candidateStartScreen = Raylib.GetWorldToScreen2D(candidateStart, camera);
                    Vector2 candidateEndScreen = Raylib.GetWorldToScreen2D(candidateEnd, camera);

                    Assert.Equal(neutralStartScreen.X, candidateStartScreen.X);
                    Assert.Equal(neutralEndScreen.X, candidateEndScreen.X);
                    if (index < originalPoints.Length)
                        Assert.Equal(neutralStart, candidateStart);

                    AssertInsidePortraitViewport(candidateStartScreen);
                    AssertInsidePortraitViewport(candidateEndScreen);
                }

                Assert.Equal(originalPoints, game.CurrentLevel.Definition.Points.ToArray());
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void StrongPositivePerspectiveKeepsLevel9FarRimClearOfTheUpperTextRegion()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Projection.PortraitPerspective = 50;
            LevelConfiguration configuration = LevelConfiguration.Level9Production;
            TempestGame game = new(configuration, random: new Random(1));
            Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
            List<(Vector2 Start, Vector2 End)> lines = [];
            new VectorRenderer(game).DrawTube(
                (start, end, _) => lines.Add((start, end)));

            float highestFarRimY = lines
                .Take(configuration.Definition.Points.Count)
                .Min(line => Raylib.GetWorldToScreen2D(line.End, camera).Y);
            float upperTextBottom =
                (120 + 18) * DisplayConfiguration.PortraitOutputScale;

            Assert.True(highestFarRimY >= upperTextBottom + 10,
                $"Level 9 far rim at {highestFarRimY:F2}px must remain below the production HUD text region ending at {upperTextBottom:F2}px.");
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Theory]
    [InlineData(1, -50)]
    [InlineData(1, -20)]
    [InlineData(1, 0)]
    [InlineData(1, 20)]
    [InlineData(1, 50)]
    [InlineData(6, -50)]
    [InlineData(6, -20)]
    [InlineData(6, 0)]
    [InlineData(6, 20)]
    [InlineData(6, 50)]
    [InlineData(9, -50)]
    [InlineData(9, -20)]
    [InlineData(9, 0)]
    [InlineData(9, 20)]
    [InlineData(9, 50)]
    public void PortraitPerspectiveKeepsEveryIntermediateRailAnchorCollinear(
        int level, float perspective)
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Projection.PortraitPerspective = perspective;
            LevelConfiguration configuration = ProductionConfiguration(level);
            TempestGame game = new(configuration, random: new Random(1));
            Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
            float nearDepth = configuration.Position;
            float farDepth = configuration.Position + configuration.Depth;
            float nearReciprocal = 1 / (nearDepth + Projection.ZOffset);
            float farReciprocal = 1 / (farDepth + Projection.ZOffset);
            List<(Vector2 Start, Vector2 End)> tubeLines = [];
            new VectorRenderer(game).DrawTube(
                (start, end, _) => tubeLines.Add((start, end)));

            for (int vertex = 0; vertex < game.CurrentLevel.Definition.Points.Count; vertex++)
            {
                Vector2 point = game.CurrentLevel.Definition.Points[vertex];
                Vector2 near = Raylib.GetWorldToScreen2D(tubeLines[vertex].Start, camera);
                Vector2 far = Raylib.GetWorldToScreen2D(tubeLines[vertex].End, camera);
                Vector2 rail = far - near;
                float railLengthSquared = rail.LengthSquared();

                foreach (float intermediateDepth in new[] { 125f, 250f, 375f })
                {
                    Vector2 intermediate = Raylib.GetWorldToScreen2D(Projection.Project(
                        point.X, point.Y, intermediateDepth, game.CurrentLevel.Definition.CenterY,
                        game.CurrentLevel.Definition.YOffset, nearDepth, farDepth), camera);
                    if (railLengthSquared == 0)
                    {
                        Assert.Equal(near, intermediate);
                        continue;
                    }

                    Vector2 fromNear = intermediate - near;
                    float perpendicularDistance = MathF.Abs(
                        rail.X * fromNear.Y - rail.Y * fromNear.X) / MathF.Sqrt(railLengthSquared);
                    float alongRail = Vector2.Dot(fromNear, rail) / railLengthSquared;
                    float reciprocal = 1 / (intermediateDepth + Projection.ZOffset);
                    float expectedFraction =
                        (reciprocal - nearReciprocal) / (farReciprocal - nearReciprocal);

                    Assert.InRange(perpendicularDistance, 0, .001f);
                    Assert.InRange(alongRail, 0, 1);
                    Assert.InRange(MathF.Abs(alongRail - expectedFraction), 0, .001f);
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Theory]
    [InlineData(-50)]
    [InlineData(50)]
    public void PortraitPerspectiveProjectionRemainsFiniteAcrossGameplayAndApproachDepths(
        float perspective)
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Projection.PortraitPerspective = perspective;
            for (int level = 1; level <= 9; level++)
            {
                LevelConfiguration configuration = ProductionConfiguration(level);
                TempestGame game = new(configuration, random: new Random(1));
                float nearDepth = configuration.Position;
                float farDepth = nearDepth + configuration.Depth;
                float approachNearDepth = nearDepth + LevelApproach.InitialDepthOffset;
                float approachFarDepth = farDepth + LevelApproach.InitialDepthOffset;
                (float Depth, float Near, float Far)[] depths =
                [
                    (nearDepth, nearDepth, farDepth),
                    (farDepth, nearDepth, farDepth),
                    (nearDepth + 2 * configuration.Depth, nearDepth, farDepth),
                    (nearDepth + 16, nearDepth, farDepth),
                    (nearDepth + 239, nearDepth, farDepth),
                    (approachNearDepth, approachNearDepth, approachFarDepth),
                    (approachFarDepth, approachNearDepth, approachFarDepth),
                    (approachFarDepth + configuration.Depth,
                        approachNearDepth, approachFarDepth)
                ];

                foreach (Vector2 point in game.CurrentLevel.Definition.Points)
                {
                    foreach ((float depth, float near, float far) in depths)
                    {
                        Vector2 projected = Projection.Project(
                            point.X,
                            point.Y,
                            depth,
                            game.CurrentLevel.Definition.CenterY,
                            game.CurrentLevel.Definition.YOffset,
                            near,
                            far);
                        Assert.True(float.IsFinite(projected.X));
                        Assert.True(float.IsFinite(projected.Y));
                    }
                }
            }
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Theory]
    [InlineData(-50)]
    [InlineData(-20)]
    [InlineData(-2)]
    [InlineData(2)]
    [InlineData(20)]
    [InlineData(50)]
    public void PortraitPerspectiveLeavesNearRimAndHorizontalProjectionUnchanged(
        float perspective)
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            const float x = 172.5f;
            const float y = -93.25f;
            const float centerY = 360;
            const float yOffset = -40;
            const float nearDepth = 50;
            const float farDepth = 450;

            Projection.PortraitPerspective = 0;
            Vector2 neutralNear = Projection.Project(
                x, y, nearDepth, centerY, yOffset, nearDepth, farDepth);
            Vector2 neutralFar = Projection.Project(
                x, y, farDepth, centerY, yOffset, nearDepth, farDepth);
            Projection.PortraitPerspective = perspective;
            Vector2 candidateNear = Projection.Project(
                x, y, nearDepth, centerY, yOffset, nearDepth, farDepth);
            Vector2 candidateFar = Projection.Project(
                x, y, farDepth, centerY, yOffset, nearDepth, farDepth);
            Assert.Equal(neutralNear, candidateNear);
            Assert.Equal(neutralFar.X, candidateFar.X);
            Assert.NotEqual(neutralFar.Y, candidateFar.Y);
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    [Fact]
    public void NeutralPortraitPerspectiveExactlyPreservesTheExistingProjection()
    {
        float originalK = Projection.K;
        float originalZOffset = Projection.ZOffset;
        float originalPerspective = Projection.PortraitPerspective;
        try
        {
            Projection.K = 70;
            Projection.ZOffset = 5;
            Projection.PortraitPerspective = Projection.PortraitPerspectiveDefault;
            const float x = -137.25f;
            const float y = 81.75f;
            const float depth = 243.5f;
            const float centerY = 310;
            const float yOffset = -10;
            float denominator = depth + Projection.ZOffset;
            Vector2 expected = new(
                Projection.Width / 2f + x / (denominator / Projection.K),
                centerY + (y + yOffset) / (denominator / Projection.K));

            Assert.Equal(expected, Projection.Project(x, y, depth, centerY, yOffset));
        }
        finally
        {
            Projection.K = originalK;
            Projection.ZOffset = originalZOffset;
            Projection.PortraitPerspective = originalPerspective;
        }
    }

    private static LevelConfiguration ProductionConfiguration(int level) =>
        level switch
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
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };

    private static double SignedArea(IEnumerable<Vector2> points)
    {
        Vector2[] vertices = points.ToArray();
        double area = 0;
        for (int index = 0; index < vertices.Length; index++)
        {
            Vector2 current = vertices[index];
            Vector2 next = vertices[(index + 1) % vertices.Length];
            area += current.X * next.Y - next.X * current.Y;
        }

        return area / 2;
    }

    private static void AssertInsidePortraitViewport(Vector2 point)
    {
        Assert.InRange(point.X, DisplayConfiguration.PortraitViewportLeft,
            DisplayConfiguration.PortraitViewportLeft + DisplayConfiguration.PortraitViewportWidth);
        Assert.InRange(point.Y, 0, DisplayConfiguration.PortraitViewportHeight);
    }
}
