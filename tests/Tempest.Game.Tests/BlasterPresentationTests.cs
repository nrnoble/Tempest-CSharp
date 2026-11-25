using System.Numerics;
using Raylib_cs;
using Tempest.Game;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

[Collection("Level 2 tests")]
public sealed class BlasterPresentationTests
{
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
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	public void AtariBlasterBodyStaysOutsideTheVisibleTubeOnEveryPlayableLane(int displayedLevel)
	{
		TempestGame game = CreateGame(displayedLevel);
		VectorRenderer renderer = new(game, atariPresentationEnabled: true);
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;

		for (int lane = 0; lane < game.CurrentLevel.PlayableLaneCount; lane++)
		{
			game.Player.ResetPosition(lane, .5f);
			AssertBlasterOutsideVisibleTube(game, renderer, points);
		}
	}

	[Fact]
	public void Level1FixedLeftNormalIsTheTubeInteriorSide()
	{
		TempestGame game = CreateGame(1);
		VectorRenderer renderer = new(game, atariPresentationEnabled: true);
		List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = [];
		renderer.DrawTube((start, end, color) => tubeLines.Add((start, end, color)));
		renderer.GetPlayerEdge(game.CurrentLevel.Definition.Points,
			out Vector2 nearStart, out Vector2 nearEnd);

		Vector2 tangent = nearEnd - nearStart;
		Vector2 leftNormal = Vector2.Normalize(new(-tangent.Y, tangent.X));
		int lane = game.Player.EdgeIndex;
		int nextLane = game.CurrentLevel.NextEdge(lane);
		Vector2 interior = ((tubeLines[lane].End - tubeLines[lane].Start) +
			(tubeLines[nextLane].End - tubeLines[nextLane].Start)) * .5f;

		Assert.True(Vector2.Dot(leftNormal, interior) > 0);
	}

	[Fact]
	public void AtariBlasterNormalHandlesBothProjectedWindingOrientations()
	{
		bool interiorOnLeft = false;
		bool interiorOnRight = false;
		for (int displayedLevel = 1; displayedLevel <= 15; displayedLevel++)
		{
			TempestGame game = CreateGame(displayedLevel);
			VectorRenderer renderer = new(game, atariPresentationEnabled: true);
			IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
			for (int lane = 0; lane < game.CurrentLevel.PlayableLaneCount; lane++)
			{
				game.Player.ResetPosition(lane, .5f);
				if (AssertBlasterOutsideVisibleTube(game, renderer, points))
					interiorOnLeft = true;
				else
					interiorOnRight = true;
			}
		}

		Assert.True(interiorOnLeft);
		Assert.True(interiorOnRight);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	public void AtariBlasterNormalRemainsOutwardWhenTravelDirectionReverses(int displayedLevel)
	{
		TempestGame game = CreateGame(displayedLevel);
		VectorRenderer renderer = new(game, atariPresentationEnabled: true);
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;

		foreach (float movement in new[] { .05f, -.05f })
		{
			game.Player.ResetPosition(7, .5f);
			game.Player.MoveAlongTube(movement, game.CurrentLevel);
			AssertBlasterOutsideVisibleTube(game, renderer, points);
		}
	}

	private static bool AssertBlasterOutsideVisibleTube(
		TempestGame game,
		VectorRenderer renderer,
		IReadOnlyList<Vector2> points)
	{
		List<(Vector2 Start, Vector2 End, Color Color)> tubeLines = [];
		List<(Vector2 Start, Vector2 End, Color Color)> blasterLines = [];
		int laneBefore = game.Player.EdgeIndex;
		float positionBefore = game.Player.EdgePosition;
		float depthBefore = game.Player.Depth;
		int livesBefore = game.Player.Lives;
		int scoreBefore = game.Score.Score;
		int shotsBefore = game.Shots.Count;
		LevelState stateBefore = game.State;

		renderer.DrawTube((start, end, color) => tubeLines.Add((start, end, color)));
		renderer.GetPlayerEdge(points, out Vector2 nearStart, out Vector2 nearEnd);
		renderer.DrawPlayer(points,
			(start, end, color) => blasterLines.Add((start, end, color)));

		Assert.True(tubeLines.Count >= points.Count);
		Assert.Equal(6, blasterLines.Count);

		int lane = game.Player.EdgeIndex;
		int nextLane = game.CurrentLevel.NextEdge(lane);
		Assert.InRange(nextLane, 0, points.Count - 1);
		Vector2 tangent = nearEnd - nearStart;
		float tangentLength = tangent.Length();
		Assert.True(tangentLength > 0);
		Vector2 leftNormal = new(-tangent.Y / tangentLength, tangent.X / tangentLength);
		Vector2 interior = ((tubeLines[lane].End - tubeLines[lane].Start) +
			(tubeLines[nextLane].End - tubeLines[nextLane].Start)) * .5f;
		float interiorSide = Vector2.Dot(leftNormal, interior);
		Vector2 center = nearEnd - tangent * game.Player.EdgePosition;
		float outerSide = Vector2.Dot(leftNormal, blasterLines[0].End - center);
		float innerSide = Vector2.Dot(leftNormal, blasterLines[4].Start - center);
		float clawLength = Vector2.Distance(center, blasterLines[0].End);

		Assert.True(float.IsFinite(interiorSide));
		Assert.True(interiorSide * outerSide < 0);
		Assert.True(interiorSide * innerSide < 0);
		Assert.InRange(MathF.Abs(
			Vector2.Distance(center, blasterLines[4].Start) - clawLength * .5f), 0, .001f);
		AssertNearEdge(blasterLines[2].Start, nearStart, tangent, tangentLength);
		AssertNearEdge(blasterLines[2].End, nearStart, tangent, tangentLength);
		AssertNearEdge(blasterLines[3].Start, nearStart, tangent, tangentLength);
		AssertNearEdge(blasterLines[3].End, nearStart, tangent, tangentLength);
		Assert.All(blasterLines, line => Assert.Equal(Color.Yellow, line.Color));
		Assert.Equal(laneBefore, game.Player.EdgeIndex);
		Assert.Equal(positionBefore, game.Player.EdgePosition);
		Assert.Equal(depthBefore, game.Player.Depth);
		Assert.Equal(livesBefore, game.Player.Lives);
		Assert.Equal(scoreBefore, game.Score.Score);
		Assert.Equal(shotsBefore, game.Shots.Count);
		Assert.Equal(stateBefore, game.State);
		return interiorSide > 0;
	}

	private static void AssertNearEdge(
		Vector2 point,
		Vector2 edgeStart,
		Vector2 edgeTangent,
		float edgeLength)
	{
		float distance = MathF.Abs(
			(edgeTangent.X * (point.Y - edgeStart.Y) -
			 edgeTangent.Y * (point.X - edgeStart.X)) / edgeLength);
		Assert.InRange(distance, 0, .001f);
	}

	private static TempestGame CreateGame(int displayedLevel)
	{
		LevelConfiguration configuration = LevelProgression.GetDirectStartLevel(displayedLevel) ??
			throw new ArgumentOutOfRangeException(nameof(displayedLevel));
		return new TempestGame(configuration, random: new Random(1));
	}
}
