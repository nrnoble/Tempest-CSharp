using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class AtariLevel7EntityPresentationTests
{
    [Fact]
    public void IdentityLaneMappingAndDepthInterpolationUseTheCenteredRecordEndpoints()
    {
        AtariTubeVectorList vectors =
            AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
        Assert.Equal(AtariTubeRecord.RequiredPointCount, AtariLevel7Presentation.NearPoints.Count);
        Assert.Equal(vectors.NearPoints.Count, AtariLevel7Presentation.FarPoints.Count);

        for (int lane = 0; lane < AtariLevel7Presentation.NearPoints.Count; lane++)
        {
            Assert.Equal(lane,
                AtariLevel7EntityPresentation.MapGameplayLaneToPresentationLane(lane));
            Assert.Equal(
                AtariLevel7Presentation.MapNearCentered(vectors.NearPoints[lane]),
                AtariLevel7Presentation.NearPoints[lane]);
            Assert.Equal(
                AtariLevel7Presentation.MapFarCentered(vectors.FarPoints[lane]),
                AtariLevel7Presentation.FarPoints[lane]);

            int next = (lane + 1) % AtariLevel7Presentation.NearPoints.Count;
            float nearWidth = Vector2.Distance(
                AtariLevel7Presentation.NearPoints[lane],
                AtariLevel7Presentation.NearPoints[next]);
            foreach (float depth in new[] { 0f, .25f, .5f, .85f, 1f })
            {
                AtariLevel7LaneGeometry geometry =
                    AtariLevel7EntityPresentation.MapLaneAtDepth(lane, depth);
                Vector2 expectedStart = Vector2.Lerp(
                    AtariLevel7Presentation.NearPoints[lane],
                    AtariLevel7Presentation.FarPoints[lane],
                    depth);
                Vector2 expectedEnd = Vector2.Lerp(
                    AtariLevel7Presentation.NearPoints[next],
                    AtariLevel7Presentation.FarPoints[next],
                    depth);

                Assert.Equal(lane, geometry.GameplayLane);
                Assert.Equal(lane, geometry.PresentationLane);
                Assert.Equal(expectedStart, geometry.Start);
                Assert.Equal(expectedEnd, geometry.End);
                Assert.Equal((expectedStart + expectedEnd) / 2, geometry.Center);
                Assert.InRange(MathF.Abs(geometry.Tangent.Length() - 1), 0, .0001f);
                Assert.InRange(MathF.Abs(Vector2.Dot(geometry.Tangent, geometry.Normal)), 0, .0001f);
                Assert.InRange(MathF.Abs(geometry.LocalScale -
                    Vector2.Distance(expectedStart, expectedEnd) / nearWidth), 0, .0001f);
            }
        }

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariLevel7EntityPresentation.MapGameplayLaneToPresentationLane(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            AtariLevel7EntityPresentation.MapGameplayLaneToPresentationLane(
                AtariTubeRecord.RequiredPointCount));
    }

    [Fact]
    public void DepthNormalizationAndEggExtrapolationRemainLinearAndUnclamped()
    {
        Assert.Equal(0, AtariLevel7EntityPresentation.NormalizeDepth(50, 50, 400));
        Assert.Equal(.25f, AtariLevel7EntityPresentation.NormalizeDepth(150, 50, 400));
        Assert.Equal(.5f, AtariLevel7EntityPresentation.NormalizeDepth(250, 50, 400));
        Assert.Equal(.85f, AtariLevel7EntityPresentation.NormalizeDepth(390, 50, 400));
        Assert.Equal(1, AtariLevel7EntityPresentation.NormalizeDepth(450, 50, 400));
        Assert.Equal(1.5f, AtariLevel7EntityPresentation.NormalizeDepth(650, 50, 400));

        AtariLevel7LaneGeometry far =
            AtariLevel7EntityPresentation.MapLaneAtDepth(0, 1);
        AtariLevel7LaneGeometry beyondFar =
            AtariLevel7EntityPresentation.MapLaneAtDepth(0, 1.5f);
        Vector2 expectedStart = far.Start +
            (AtariLevel7Presentation.FarPoints[0] -
                AtariLevel7Presentation.NearPoints[0]) * .5f;
        Vector2 expectedEnd = far.End +
            (AtariLevel7Presentation.FarPoints[1] -
                AtariLevel7Presentation.NearPoints[1]) * .5f;

        Assert.InRange(Vector2.Distance(expectedStart, beyondFar.Start), 0, .0001f);
        Assert.InRange(Vector2.Distance(expectedEnd, beyondFar.End), 0, .0001f);
        Assert.True(beyondFar.Center != far.Center);
    }

    [Fact]
    public void CenteringIsSharedAndCentersTheTubeInTheHudClearedHostComposition()
    {
        Vector2 translation = AtariLevel7Presentation.CenteringTranslationLogical;
        Assert.InRange(MathF.Abs(translation.X - 97.47f), 0, .02f);
        Assert.InRange(MathF.Abs(translation.Y - 78.19f), 0, .02f);

        AtariTubeVectorList vectors =
            AtariTubeVectorListBuilder.Build(AtariTubeRecordCatalog.Level7Record06);
        AtariDvgViewportMap provisional = AtariDvgViewportMap.DevelopmentOnlyProvisional;
        AtariDvgViewportMap nearMap = new(
            provisional.Translation,
            AtariLevel7Presentation.SharedGlobalScale,
            AtariLevel7Presentation.SharedHorizontalAspect,
            provisional.RotationDegrees,
            provisional.SwapAxes,
            provisional.XSign,
            provisional.YSign);
        AtariDvgViewportMap farMap = new(
            provisional.Translation + new Vector2(
                AtariLevel7Presentation.FarTranslationLogicalX,
                AtariLevel7Presentation.FarTranslationLogicalY),
            AtariLevel7Presentation.SharedGlobalScale,
            AtariLevel7Presentation.SharedHorizontalAspect,
            provisional.RotationDegrees,
            provisional.SwapAxes,
            provisional.XSign,
            provisional.YSign);

        for (int index = 0; index < AtariTubeRecord.RequiredPointCount; index++)
        {
            Vector2 uncenteredNear = nearMap.Map(vectors.NearPoints[index]);
            Vector2 uncenteredFar = farMap.Map(vectors.FarPoints[index]);
            Assert.Equal(uncenteredNear + translation,
                AtariLevel7Presentation.NearPoints[index]);
            Assert.Equal(uncenteredFar + translation,
                AtariLevel7Presentation.FarPoints[index]);
            Assert.InRange(Vector2.Distance(
                uncenteredFar - uncenteredNear,
                AtariLevel7Presentation.FarPoints[index] -
                    AtariLevel7Presentation.NearPoints[index]), 0, .0001f);
        }

        Vector2[] endpoints = AtariLevel7Presentation.NearPoints
            .Concat(AtariLevel7Presentation.FarPoints)
            .ToArray();
        Vector2 minimum = new(
            endpoints.Min(point => point.X),
            endpoints.Min(point => point.Y));
        Vector2 maximum = new(
            endpoints.Max(point => point.X),
            endpoints.Max(point => point.Y));
        Camera2D camera = DisplayConfiguration.ExperimentalPortraitPlayfieldCamera;
        Vector2 hostCenter = camera.Offset +
            (((minimum + maximum) / 2) - camera.Target) * camera.Zoom;
        Assert.InRange(MathF.Abs(hostCenter.X - DisplayConfiguration.WindowWidth / 2f), 0, .001f);
        float hudBottom = (120 + 18) * DisplayConfiguration.PortraitOutputScale;
        float gameplayCenterY =
            (hudBottom + DisplayConfiguration.PortraitViewportHeight) / 2;
        Assert.InRange(MathF.Abs(hostCenter.Y - gameplayCenterY), 0, .001f);
    }

    [Theory]
    [InlineData(0, 0, 503.44f, 255.32f)]
    [InlineData(0, .25f, 481.65f, 314.19f)]
    [InlineData(0, .5f, 459.87f, 373.06f)]
    [InlineData(0, .85f, 429.37f, 455.48f)]
    [InlineData(0, 1, 416.30f, 490.80f)]
    [InlineData(0, 1.5f, 372.73f, 608.54f)]
    public void RepresentativeLaneZeroProbesMatchTheInvestigation(
        int lane,
        float depth,
        float expectedX,
        float expectedY)
    {
        Vector2 actual =
            AtariLevel7EntityPresentation.MapLaneAtDepth(lane, depth).Center;
        Assert.InRange(MathF.Abs(actual.X - expectedX), 0, .1f);
        Assert.InRange(MathF.Abs(actual.Y - expectedY), 0, .1f);
    }

    [Fact]
    public void FlipperTransitionsUseMappedRecordGeometryAtTheirExistingProgress()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        DirectedLaneTransition transition =
            game.CurrentLevel.CalculateDirectedLaneTransition(3, 2, 1);

        (Vector2 initialStart, Vector2 initialEnd) =
            AtariLevel7EntityPresentation.MapFlipperTransition(transition, 0, .5f);
        (Vector2 pivotStart, Vector2 pivotEnd) =
            AtariLevel7EntityPresentation.MapFlipperTransition(transition, .5f, .5f);
        (Vector2 finalStart, Vector2 finalEnd) =
            AtariLevel7EntityPresentation.MapFlipperTransition(transition, 1, .5f);

        AtariLevel7LaneGeometry source =
            AtariLevel7EntityPresentation.MapLaneAtDepth(3, .5f);
        AtariLevel7LaneGeometry destination =
            AtariLevel7EntityPresentation.MapLaneAtDepth(2, .5f);
        Assert.Equal((source.Start, source.End), (initialStart, initialEnd));
        Assert.Equal((destination.Start, destination.End), (finalStart, finalEnd));
        Assert.True(float.IsFinite(pivotStart.X));
        Assert.True(float.IsFinite(pivotStart.Y));
        Assert.True(float.IsFinite(pivotEnd.X));
        Assert.True(float.IsFinite(pivotEnd.Y));
        Assert.NotEqual(initialEnd, pivotEnd);
    }

    [Fact]
    public void EveryProductionLevel7FlipperTransitionMapsAcrossPlayableDepths()
    {
        Level level = new(LevelConfiguration.Level7Production);
        float[] normalizedDepths = [0, .125f, .25f, .5f, .75f, .875f, 1, 1.5f, 2];
        double[] productionTurnSpeeds =
        [
            level.Configuration.Flipper.TurnSpeedDegrees,
            level.RimTurnSpeedDegrees
        ];

        for (int source = 0; source < level.PlayableLaneCount; source++)
        {
            foreach (int direction in new[] { -1, 1 })
            {
                int destination = direction > 0
                    ? level.PreviousEdge(source)
                    : level.NextEdge(source);
                DirectedLaneTransition transition =
                    level.CalculateDirectedLaneTransition(source, destination, direction);
                SortedSet<float> progresses = [0, .1f, .25f, .5f, .75f, .9f, 1];
                foreach (double turnSpeed in productionTurnSpeeds)
                {
                    for (int tick = 1; tick <= 100; tick++)
                    {
                        double turnAngle = tick * turnSpeed;
                        if (turnAngle >= transition.SweepMagnitudeDegrees)
                            break;

                        progresses.Add((float)(turnAngle / transition.SweepMagnitudeDegrees));
                    }
                }

                foreach (float progress in progresses)
                foreach (float depth in normalizedDepths)
                {
                    (Vector2 start, Vector2 end) =
                        AtariLevel7EntityPresentation.MapFlipperTransition(
                            transition, progress, depth);
                    AssertFinite(start);
                    AssertFinite(end);

                    AtariLevel7LaneGeometry sourceGeometry =
                        AtariLevel7EntityPresentation.MapLaneAtDepth(source, depth);
                    AtariLevel7LaneGeometry destinationGeometry =
                        AtariLevel7EntityPresentation.MapLaneAtDepth(destination, depth);
                    if (progress == 0)
                    {
                        Assert.Equal((sourceGeometry.Start, sourceGeometry.End), (start, end));
                        continue;
                    }

                    if (progress == 1)
                    {
                        Assert.Equal((destinationGeometry.Start, destinationGeometry.End), (start, end));
                        continue;
                    }

                    Vector2 pivot = AtariLevel7EntityPresentation.MapVertexAtDepth(
                        transition.HingeVertex, depth);
                    Vector2 initial = AtariLevel7EntityPresentation.MapVertexAtDepth(
                        transition.SourceFreeVertex, depth) - pivot;
                    Vector2 destinationVector = AtariLevel7EntityPresentation.MapVertexAtDepth(
                        transition.DestinationFreeVertex, depth) - pivot;
                    float mappedSweep = ExpectedMappedSweep(
                        initial, destinationVector, transition.SpatialSweepDegrees);
                    float radius = initial.Length() +
                        (destinationVector.Length() - initial.Length()) * progress;
                    Vector2 expectedMoving = pivot +
                        Rotate(initial, mappedSweep * progress * MathF.PI / 180) *
                        (radius / initial.Length());
                    Vector2 actualMoving = transition.HingeIsStart ? end : start;
                    Vector2 actualPivot = transition.HingeIsStart ? start : end;

                    AssertVectorNear(pivot, actualPivot, .001f);
                    AssertVectorNear(expectedMoving, actualMoving, .001f);
                    Vector2 tangent = Vector2.Normalize(end - start);
                    Vector2 normal = new(-tangent.Y, tangent.X);
                    Assert.InRange(MathF.Abs(tangent.Length() - 1), 0, .0001f);
                    Assert.InRange(MathF.Abs(Vector2.Dot(tangent, normal)), 0, .0001f);
                }
            }
        }
    }

    [Fact]
    public void ReproducedLongTransitionUsesMappedEndpointsAndRemainsContinuous()
    {
        Level level = new(LevelConfiguration.Level7Production);
        DirectedLaneTransition transition =
            level.CalculateDirectedLaneTransition(1, 2, direction: -1);
        Assert.Equal(307.893f, transition.SweepMagnitudeDegrees, precision: 2);

        const float depth = 0;
        Vector2 pivot = AtariLevel7EntityPresentation.MapVertexAtDepth(
            transition.HingeVertex, depth);
        Vector2 initial = AtariLevel7EntityPresentation.MapVertexAtDepth(
            transition.SourceFreeVertex, depth) - pivot;
        Vector2 destination = AtariLevel7EntityPresentation.MapVertexAtDepth(
            transition.DestinationFreeVertex, depth) - pivot;
        float mappedSweep = ExpectedMappedSweep(
            initial, destination, transition.SpatialSweepDegrees);
        Assert.True(MathF.Abs(mappedSweep) > 180,
            $"Expected the mapped sweep to remain the long route, got {mappedSweep} degrees.");

        Vector2 previousMoving = initial + pivot;
        for (int step = 1; step <= 100; step++)
        {
            float progress = step / 100f;
            (Vector2 start, Vector2 end) =
                AtariLevel7EntityPresentation.MapFlipperTransition(
                    transition, progress, depth);
            Vector2 moving = transition.HingeIsStart ? end : start;
            AssertFinite(moving);
            if (progress < 1)
            {
                Assert.True(Vector2.Distance(previousMoving, moving) < 20,
                    $"The endpoint jumped at progress {progress}.");
            }
            else
            {
                Assert.Equal(
                    AtariLevel7EntityPresentation.MapVertexAtDepth(
                        transition.DestinationFreeVertex, depth),
                    moving);
            }

            previousMoving = moving;
        }

        (Vector2 expectedStart, Vector2 expectedEnd) =
            AtariLevel7EntityPresentation.MapFlipperTransition(
                transition, .75f, depth);
        Vector2 expectedAtThreeQuarters = pivot +
            Rotate(initial, mappedSweep * .75f * MathF.PI / 180) *
            ((initial.Length() + (destination.Length() - initial.Length()) * .75f) /
                initial.Length());
        AssertVectorNear(expectedAtThreeQuarters,
            transition.HingeIsStart ? expectedEnd : expectedStart, .001f);

        DirectedLaneTransition opposite =
            level.CalculateDirectedLaneTransition(1, 0, direction: 1);
        var forwardMidpoint =
            AtariLevel7EntityPresentation.MapFlipperTransition(transition, .5f, .5f);
        var reverseMidpoint =
            AtariLevel7EntityPresentation.MapFlipperTransition(opposite, .5f, .5f);
        Assert.NotEqual(forwardMidpoint, reverseMidpoint);
    }

    [Fact]
    public void MalformedMappedTransitionStillFailsExplicitly()
    {
        Assert.Throws<ArgumentException>(() =>
            AtariLevel7EntityPresentation.MapFlipperTransition(default, 0, .5f));
    }

    [Fact]
    public void OrdinaryAndReleasedCargoFlippersUseRenderOnlyMappedTransitions()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        Level level = game.CurrentLevel;
        Flipper ordinary = new(edge: 1);
        for (int tick = 0; tick < 100 && ordinary.State != FlipperState.Flipping; tick++)
            ordinary.Update(level, playerEdge: 0, (_, _) => 1);
        Assert.Equal(FlipperState.Flipping, ordinary.State);
        ordinary.Update(level, playerEdge: 0, (_, _) => 1);

        Tanker carrier = new(edgeIndex: 5, originalSlotId: 0);
        TankerReleaseResult release = carrier.ReleaseFlipperCargo(
            level, firstSideDirection: 1, TankerReleaseKind.PlayerShotDestruction);
        Flipper cargo = Flipper.CreateTankerCargo(
            level, release.Cargo[0], cargoUnitId: 0);
        Assert.True(cargo.IsTankerCargo);
        Assert.Equal(FlipperState.Flipping, cargo.State);

        Vector2[] gameplayPoints = level.Definition.Points.ToArray();
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);
        Assert.True(renderer.UsesAtariLevel7PresentationCandidateForCurrentLevel);

        foreach (Flipper flipper in new[] { ordinary, cargo })
        {
            var before = CaptureFlipperState(flipper);
            (Vector2 start, Vector2 end) = renderer.ProjectFlipperEndpoints(flipper);
            AssertFinite(start);
            AssertFinite(end);
            Assert.NotEqual(start, end);
            Assert.Equal(before, CaptureFlipperState(flipper));
        }

        Assert.Equal(gameplayPoints, level.Definition.Points.ToArray());
    }

    [Fact]
    public void Level7EntityAnchorsUseMappedGeometryWithoutChangingGameplayState()
    {
        TempestGame game = new(LevelConfiguration.Level7Production, random: new Random(1));
        Vector2[] gameplayPoints = game.CurrentLevel.Definition.Points.ToArray();
        int playerLane = game.Player.EdgeIndex;
        float playerPosition = game.Player.EdgePosition;
        float playerDepth = game.Player.Depth;
        Assert.NotEmpty(game.Eggs);
        Egg egg = game.Eggs[0];
        int eggLane = egg.EdgeIndex;
        float eggHeight = egg.Height;
        VectorRenderer renderer = new(game, atariPresentationEnabled: true);

        Assert.True(renderer.UsesAtariLevel7PresentationCandidateForCurrentLevel);

        renderer.GetPlayerEdge(gameplayPoints, out Vector2 playerStart, out Vector2 playerEnd);
        Assert.Equal(AtariLevel7Presentation.NearPoints[playerLane], playerStart);
        Assert.Equal(
            AtariLevel7Presentation.NearPoints[(playerLane + 1) % gameplayPoints.Length],
            playerEnd);

        List<(Vector2 Start, Vector2 End, Raylib_cs.Color Color)> tubeLines = [];
        renderer.DrawTube((start, end, color) => tubeLines.Add((start, end, color)));
        Assert.Equal(gameplayPoints.Length * 3, tubeLines.Count);
        Assert.Equal(new Raylib_cs.Color(255, 255, 0, 255), tubeLines[playerLane].Color);
        Assert.Equal(
            new Raylib_cs.Color(255, 255, 0, 255),
            tubeLines[(playerLane + 1) % gameplayPoints.Length].Color);

        LevelConfiguration configuration = game.CurrentLevel.Configuration;
        Vector2 expectedEgg = AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
            eggLane,
            configuration.Position + configuration.Depth - egg.Height * 20,
            configuration.Position,
            configuration.Depth).Center;
        Assert.Equal(expectedEgg, renderer.ProjectEgg(egg));

        Tanker tanker = new(0, 100);
        float tankerDepth = configuration.Position + configuration.Depth - tanker.Height;
        Assert.Equal(
            AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
                tanker.EdgeIndex, tankerDepth, configuration.Position, configuration.Depth).Center,
            renderer.ProjectTankerCenter(tanker));

        EnemyBullet enemyBullet = new(1, 150);
        float bulletDepth = configuration.Position + configuration.Depth - enemyBullet.Height;
        Assert.Equal(
            AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
                enemyBullet.EdgeIndex, bulletDepth, configuration.Position, configuration.Depth).Center,
            renderer.ProjectEnemyBullet(enemyBullet));

        Shot shot = new() { EdgeIndex = 5, Height = 120 };
        float shotDepth = configuration.Position + configuration.Depth - shot.Height;
        Assert.Equal(
            AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
                shot.EdgeIndex, shotDepth, configuration.Position, configuration.Depth).Center,
            renderer.ProjectShot(shot));

        Assert.NotEmpty(game.Spikes);
        List<(Vector2 Start, Vector2 End)> spikeLines = [];
        List<Vector2> spikeTips = [];
        Spike[] spikesBefore = game.Spikes
            .Select(spike => new Spike { EdgeIndex = spike.EdgeIndex, Height = spike.Height })
            .ToArray();
        renderer.DrawSpikes(
            (start, end) => spikeLines.Add((start, end)),
            tip => spikeTips.Add(tip));
        Assert.Equal(game.Spikes.Count, spikeLines.Count);
        for (int index = 0; index < game.Spikes.Count; index++)
        {
            Spike spike = game.Spikes[index];
            AtariLevel7LaneGeometry baseGeometry =
                AtariLevel7EntityPresentation.MapLaneAtDepth(spike.EdgeIndex, 1);
            AtariLevel7LaneGeometry tipGeometry =
                AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
                    spike.EdgeIndex,
                    configuration.Position + configuration.Depth - spike.Height,
                    configuration.Position,
                    configuration.Depth);
            Assert.Equal(baseGeometry.Center, spikeLines[index].Start);
            Assert.Equal(tipGeometry.Center, spikeLines[index].End);
            Assert.Equal(tipGeometry.Center, spikeTips[index]);
        }

        Flipper flipper = new(2, 180);
        float flipperDepth = configuration.Position + configuration.Depth - flipper.Height;
        AtariLevel7LaneGeometry flipperLane =
            AtariLevel7EntityPresentation.MapLaneAtProjectedDepth(
                flipper.EdgeIndex, flipperDepth, configuration.Position, configuration.Depth);
        Assert.Equal((flipperLane.Start, flipperLane.End),
            renderer.ProjectFlipperEndpoints(flipper));

        Assert.Equal(gameplayPoints, game.CurrentLevel.Definition.Points.ToArray());
        Assert.Equal(playerLane, game.Player.EdgeIndex);
        Assert.Equal(playerPosition, game.Player.EdgePosition);
        Assert.Equal(playerDepth, game.Player.Depth);
        Assert.Equal(eggLane, egg.EdgeIndex);
        Assert.Equal(eggHeight, egg.Height);
        Assert.Equal(spikesBefore.Select(spike => (spike.EdgeIndex, spike.Height)),
            game.Spikes.Select(spike => (spike.EdgeIndex, spike.Height)));
    }

    private static (int EdgeIndex, int SourceEdge, int DestinationEdge, int Direction,
        float Height, FlipperState State, int PauseTicks, float TurnAngle, float TotalTurnAngle,
        DirectedLaneTransition? Transition, int CargoUnitId, bool IsTankerCargo,
        bool CompletedIntoPlayer, int ShotDelayTicks, int? RimPursuitDirection)
        CaptureFlipperState(Flipper flipper) =>
        (flipper.EdgeIndex, flipper.SourceEdge, flipper.DestinationEdge, flipper.Direction,
            flipper.Height, flipper.State, flipper.PauseTicks, flipper.TurnAngle,
            flipper.TotalTurnAngle, flipper.ActiveTransition, flipper.CargoUnitId,
            flipper.IsTankerCargo, flipper.CompletedIntoPlayer, flipper.ShotDelayTicks,
            flipper.RimPursuitDirection);

    private static float ExpectedMappedSweep(
        Vector2 initial,
        Vector2 destination,
        float gameplaySweep)
    {
        float shortest = MathF.Atan2(Cross(initial, destination), Vector2.Dot(initial, destination)) *
            180 / MathF.PI;
        if (MathF.Abs(MathF.Abs(shortest) - 180) <= .0001f)
            return MathF.CopySign(180, gameplaySweep);
        return MathF.Abs(gameplaySweep) > 180
            ? shortest - MathF.CopySign(360, shortest)
            : shortest;
    }

    private static Vector2 Rotate(Vector2 vector, float angle)
    {
        float cosine = MathF.Cos(angle);
        float sine = MathF.Sin(angle);
        return new(
            vector.X * cosine - vector.Y * sine,
            vector.X * sine + vector.Y * cosine);
    }

    private static float Cross(Vector2 first, Vector2 second) =>
        first.X * second.Y - first.Y * second.X;

    private static void AssertFinite(Vector2 point)
    {
        Assert.True(float.IsFinite(point.X));
        Assert.True(float.IsFinite(point.Y));
    }

    private static void AssertVectorNear(Vector2 expected, Vector2 actual, float tolerance)
    {
        Assert.InRange(Vector2.Distance(expected, actual), 0, tolerance);
    }
}
