using System.Numerics;
using Raylib_cs;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

public sealed class VectorRenderer
{
	public const int ScreenWidth = 800;
	public const int ScreenHeight = 600;
	public const int EggPointSize = 3;

	private readonly TempestGame game;
	public IReadOnlyList<Egg> Eggs => game.Eggs;
	public float PlayerProjectedDepth => Level.Position + Level.Depth - game.Player.Depth;
	public float PlayerClawHeight => game.Player.Depth == Level.Depth
		? 20
		: 20 * ((Level.Position + 5) / 70) / ((PlayerProjectedDepth + Projection.ZOffset) / Projection.K);

	public VectorRenderer(TempestGame game)
	{
		this.game = game;
	}

	public void Draw()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Black);

		DrawTube(game.CurrentLevel.Definition.Points);
		if (!game.IsExiting)
		{
			DrawEggs();
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
			DrawFlippers(game.CurrentLevel.Definition.Points);
		}

		if (!game.GameOver)
		{
			if (game.DeathTicks > 0)
				DrawPlayerExplosion(game.CurrentLevel.Definition.Points);
			else
				DrawPlayer(game.CurrentLevel.Definition.Points);
		}

		DrawHud();
		Raylib.EndDrawing();
	}

	private static int NextEdge(int edge, int edgeCount) => (edge + 1) % edgeCount;

	public Vector2 ProjectEgg(Egg egg)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		int next = NextEdge(egg.EdgeIndex, points.Count);
		Vector2 midpoint = new(
			(points[egg.EdgeIndex].X + points[next].X) / 2,
			(points[egg.EdgeIndex].Y + points[next].Y) / 2);
		float projectedDepth = Level.Position + Level.Depth - (egg.Height * 20);
		return Projection.Project(midpoint.X, midpoint.Y, projectedDepth,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	public void DrawEggs(Action<Vector2, Vector2> drawPoint)
	{
		foreach (Egg egg in Eggs)
		{
			Vector2 size = new(EggPointSize, EggPointSize);
			drawPoint(ProjectEgg(egg) - Vector2.One, size);
		}
	}

	private void DrawEggs()
	{
		DrawEggs((point, size) => Raylib.DrawRectangleV(point, size, Color.Red));
	}

	private void DrawTube(IReadOnlyList<Vector2> points)
	{
		Vector2[] nearRim = new Vector2[points.Count];
		Vector2[] farRim = new Vector2[points.Count];

		for (int i = 0; i < points.Count; i++)
		{
			nearRim[i] = Projection.Project(
				points[i].X, points[i].Y, Level.Position, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			farRim[i] = Projection.Project(
				points[i].X, points[i].Y, Level.Position + Level.Depth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
		}

		for (int i = 0; i < points.Count; i++)
			Raylib.DrawLineV(nearRim[i], farRim[i], Color.Blue);

		for (int i = 0; i < points.Count; i++)
		{
			int next = NextEdge(i, points.Count);
			Raylib.DrawLineV(nearRim[i], nearRim[next], Color.Blue);
			Raylib.DrawLineV(farRim[i], farRim[next], Color.Blue);
		}
	}

	private void DrawPlayerShots(IReadOnlyList<Vector2> points)
	{
		foreach (Shot shot in game.Shots)
		{
			int next = NextEdge(shot.EdgeIndex, points.Count);
			Vector2 midpoint = new(
				(points[shot.EdgeIndex].X + points[next].X) / 2,
				(points[shot.EdgeIndex].Y + points[next].Y) / 2);
			float projectedDepth = Level.Depth - shot.Height + Level.Position;
			Vector2 center = Projection.Project(
				midpoint.X, midpoint.Y, projectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			float size = 4 / ((projectedDepth + Projection.ZOffset) / Projection.K);
			DrawRotatingShot(center, size, shot.Rotation);
		}
	}

	private void DrawRotatingShot(Vector2 center, float size, float rotation)
	{
		for (int angle = 0; angle < 360; angle += 45)
		{
			float radians = (rotation + angle) * MathF.PI / 180;
			Vector2 offset = new(
				MathF.Cos(radians) * size,
				MathF.Sin(radians) * size);
			Raylib.DrawLineV(center - offset, center + offset, Color.White);
		}
	}

	private void DrawFlippers(IReadOnlyList<Vector2> points)
	{
		foreach (Flipper flipper in game.Flippers)
			DrawFlipper(points, flipper);
	}

	private void DrawFlipper(IReadOnlyList<Vector2> points, Flipper flipper)
	{
		int edge = flipper.State == FlipperState.Flipping
			? flipper.SourceEdge
			: flipper.EdgeIndex;
		int next = NextEdge(edge, points.Count);
		float projectedDepth = Level.Position + Level.Depth - flipper.Height;

		Vector2 p1 = Projection.Project(
			points[edge].X, points[edge].Y, projectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
		Vector2 p2 = Projection.Project(
			points[next].X, points[next].Y, projectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);

		if (flipper.State == FlipperState.Flipping)
			ApplyFlipperPivot(flipper, ref p1, ref p2);

		DrawFlipperShape(p1, p2, projectedDepth);
	}

	private void ApplyFlipperPivot(Flipper flipper, ref Vector2 p1, ref Vector2 p2)
	{
		float angle = MathF.Min(flipper.TurnAngle, flipper.TotalTurnAngle);
		if (flipper.Direction > 0)
			p2 = RotatePoint(p2, p1, -angle);
		else
			p1 = RotatePoint(p1, p2, angle);
	}

	private Vector2 RotatePoint(Vector2 point, Vector2 pivot, float degrees)
	{
		float radians = degrees * MathF.PI / 180;
		float sin = MathF.Sin(radians);
		float cos = MathF.Cos(radians);
		Vector2 translated = point - pivot;
		return pivot + new Vector2(
			translated.X * cos - translated.Y * sin,
			translated.X * sin + translated.Y * cos);
	}

	private void DrawFlipperShape(Vector2 p1, Vector2 p2, float projectedDepth)
	{
		float dx = p2.X - p1.X;
		float dy = p2.Y - p1.Y;
		float length = MathF.Sqrt(dx * dx + dy * dy);
		if (length == 0) return;

		float xn = -dy / length;
		float yn = dx / length;
		float height = 20 / ((projectedDepth + Projection.ZOffset) / Projection.K);

		Vector2 point2 = new(p2.X - dx * .3f - xn * height / 2,
							 p2.Y - dy * .3f - yn * height / 2);
		Vector2 point3 = new(p2.X - dx * .1f - xn * height,
							 p2.Y - dy * .1f - yn * height);
		Vector2 point5 = new(p2.X - dx * .9f - xn * height,
							 p2.Y - dy * .9f - yn * height);
		Vector2 point6 = new(p2.X - dx * .7f - xn * height / 2,
							 p2.Y - dy * .7f - yn * height / 2);

		Raylib.DrawLineV(p1, point3, Color.Red);
		Raylib.DrawLineV(point5, p2, Color.Red);
		Raylib.DrawLineV(point6, point5, Color.Red);
		Raylib.DrawLineV(point2, point3, Color.Red);
		Raylib.DrawLineV(point6, p1, Color.Red);
		Raylib.DrawLineV(point2, p2, Color.Red);
	}

	private void DrawPlayer(IReadOnlyList<Vector2> points)
	{
		GetPlayerEdge(points, out Vector2 p1, out Vector2 p2);
		float dx = p2.X - p1.X;
		float dy = p2.Y - p1.Y;
		float length = MathF.Sqrt(dx * dx + dy * dy);
		if (length == 0) return;

		float xn = -dy / length;
		float yn = dx / length;
		float clawHeight = PlayerClawHeight;
		Vector2 edgeVector = new(dx, dy);
		Vector2 center = p2 - edgeVector * game.Player.EdgePosition;
		Vector2 outer1 = center + new Vector2(xn, yn) * clawHeight;
		Vector2 outer2 = center + new Vector2(xn, yn) * clawHeight * .5f;
		Vector2 left1 = p2 - edgeVector * .2f;
		Vector2 left2 = p2 - edgeVector * .3f;
		Vector2 right1 = p2 - edgeVector * .7f;
		Vector2 right2 = p2 - edgeVector * .8f;

		Raylib.DrawLineV(left1, outer1, Color.Yellow);
		Raylib.DrawLineV(right1, outer1, Color.Yellow);
		Raylib.DrawLineV(left1, left2, Color.Yellow);
		Raylib.DrawLineV(right1, right2, Color.Yellow);
		Raylib.DrawLineV(outer2, left2, Color.Yellow);
		Raylib.DrawLineV(outer2, right2, Color.Yellow);
	}

	public void GetPlayerEdge(IReadOnlyList<Vector2> points, out Vector2 p1, out Vector2 p2)
	{
		int next = NextEdge(game.Player.EdgeIndex, points.Count);
		p1 = Projection.Project(
			points[game.Player.EdgeIndex].X, points[game.Player.EdgeIndex].Y,
			PlayerProjectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
		p2 = Projection.Project(
			points[next].X, points[next].Y,
			PlayerProjectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	private void DrawPlayerExplosion(IReadOnlyList<Vector2> points)
	{
		GetPlayerEdge(points, out Vector2 p1, out Vector2 p2);
		Vector2 center = Vector2.Lerp(p2, p1, game.Player.EdgePosition);
		float progress = 1f - game.DeathTicks / (float)TempestGame.PlayerDeathTicks;
		float radius = 8 + progress * 48;

		for (int angle = 0; angle < 360; angle += 45)
		{
			float radians = angle * MathF.PI / 180;
			Vector2 offset = new(
				MathF.Cos(radians) * radius,
				MathF.Sin(radians) * radius);
			Raylib.DrawLineV(center - offset * .25f, center + offset, Color.Yellow);
		}
	}

	private void DrawHud()
	{
		Raylib.DrawText(game.Score.Score.ToString(), 10, 10, 28, Color.Yellow);
		Raylib.DrawText($"LIVES {game.Player.Lives}", 10, 44, 20, Color.Yellow);
		int remaining = Level.EnemyTotal - game.CurrentLevel.EnemiesDestroyed;
		Raylib.DrawText($"FLIPPERS {remaining}", 10, 72, 18, Color.Red);
		Raylib.DrawText($"ACTIVE {game.Eggs.Count + game.Flippers.Count}", 10, 96, 18, Color.Gray);

		if (game.GameOver)
		{
			DrawCenteredText("GAME OVER - PRESS R", 270, 32, Color.Red);
			return;
		}

		if (game.LevelComplete)
		{
			DrawCenteredText("LEVEL 1 COMPLETE", 250, 36, Color.Yellow);
			DrawCenteredText("PRESS R TO REPLAY", 296, 24, Color.Gray);
			return;
		}

		if (game.DeathTicks > 0)
			Raylib.DrawText("PLAYER DESTROYED", 10, 120, 18, Color.Red);
	}

	private void DrawCenteredText(string text, int y, int size, Color color)
	{
		int width = Raylib.MeasureText(text, size);
		Raylib.DrawText(text, (ScreenWidth - width) / 2, y, size, color);
	}
}
