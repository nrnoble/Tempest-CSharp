using System.Numerics;
using Raylib_cs;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

/// <summary>
/// Draws the simulation as Atari-style vector line art via Raylib. Depth convention: gameplay
/// Height runs from 0 (far end of the tube) up to Level.Depth (near rim), so every entity converts
/// to projection depth as Level.Position + Level.Depth - Height; smaller projected depth is closer
/// to the camera and therefore draws larger, which gives approach scaling without per-entity size
/// tables. Glyph shapes/scales are Incitti-derived and await manual arcade comparison.
/// Most Draw*/Project* members take line/point callbacks or are public so headless tests can
/// capture the exact submitted geometry. Those tests can prove submissions are finite and placed
/// at the expected lanes/depths, but not native framebuffer output or arcade visual fidelity.
/// </summary>
public sealed class VectorRenderer
{
	public const int ScreenWidth = 800;
	public const int ScreenHeight = 600;
	public const int EggPointSize = 3;
	private static readonly Color SpikerColor = new(0, 255, 0, 255);
	private static readonly Color TankerColor = new(255, 0, 255, 255);
	private static readonly Color[] FuseballArmColors =
	[
		new(255, 255, 0, 255), new(0, 255, 0, 255), new(255, 0, 255, 255),
		new(0, 255, 255, 255), new(255, 0, 0, 255)
	];
	// Incitti ReadFuseballData: four frames, five colored arms, six points per arm.
	// Values are scaled by 1/6 at draw time; do not replace this asymmetric flicker with rotation.
	private static readonly int[,] FuseballArmPoints =
	{
		{ 0,0, 7,-5, 6,-8, 9,-10, 7,-13, 11,-15 },
		{ 0,0, 6,3, 6,6, 10,4, 12,4, 13,12 },
		{ 0,0, 2,7, -4,10, 1,14, -4,17, 1,20 },
		{ 0,0, -7,4, -7,1, -9,1, -11,6, -14,8 },
		{ 0,0, -4,-5, -3,-10, -5,-13, -9,-13, -9,-15 },
		{ 0,0, 6,-8, 5,-11, 9,-14, 7,-18, 6,-20 },
		{ 0,0, 7,-3, 6,6, 10,6, 6,11, 11,16 },
		{ 0,0, -3,8, -7,6, -5,12, -7,17, -3,21 },
		{ 0,0, -6,3, -7,-4, -10,2, -11,-4, -18,4 },
		{ 0,0, -3,-6, 2,-11, -4,-13, 2,-16, -2,-21 },
		{ 0,0, 5,-8, 7,-7, 7,-12, 10,-16, 14,-14 },
		{ 0,0, 7,1, 8,5, 11,6, 10,9, 14,10 },
		{ 0,0, -5,6, -6,11, -3,11, -6,16, -6,19 },
		{ 0,0, -6,1, -8,4, -11,3, -14,5, -16,6 },
		{ 0,0, -2,-8, -7,-8, -5,-14, -8,-14, -10,-11 },
		{ 0,0, 6,-6, 7,-12, 11,-6, 12,-12, 16,-12 },
		{ 0,0, 6,5, 6,8, 11,9, 12,14, 16,14 },
		{ 0,0, -3,6, -4,10, -9,11, -6,18, -9,21 },
		{ 0,0, -4,2, -6,1, -7,2, -12,1, -15,6 },
		{ 0,0, 1,-11, 3,-8, 5,-14, 3,-17, 6,-24 }
	};

	private readonly TempestGame game;
	public IReadOnlyList<Egg> Eggs => game.Eggs;
	// During normal play Player.Depth == Level.Depth, so this is Level.Position (the near rim).
	// During exit travel Player.Depth shrinks toward 0, moving the projected player down the tube.
	public float PlayerProjectedDepth => Level.Position + Level.Depth - game.Player.Depth;
	// Claw height is a fixed 20 px at the rim; once exit travel changes the player's depth it is
	// rescaled by the same projection ratio so the claw shrinks consistently with the tube. The
	// (Level.Position + 5) / 70 factor re-derives the rim-sized claw from the normal-play
	// projection constants (ZOffset 5, K 70) so the two branches agree at the start of travel.
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
		// Spikes stay visible (and lethal) during exit travel; all other enemies are hidden once
		// the exit starts. Player shots remain visible only in the spike-exit scenario, where the
		// player may still fire while travelling.
		DrawSpikes((start, end) => Raylib.DrawLineV(start, end, SpikerColor),
			point => Raylib.DrawPixelV(point, Color.White));
		if (!game.IsExiting)
		{
			DrawEggs();
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
			DrawFlippers(game.CurrentLevel.Definition.Points);
			DrawSpikers((start, end) => Raylib.DrawLineV(start, end, SpikerColor));
			DrawTankers((start, end) => Raylib.DrawLineV(start, end, TankerColor));
			DrawFuseballs((start, end, color) => Raylib.DrawLineV(start, end, color));
			if (game.EnemyBulletsTest)
				DrawEnemyBullets((start, end) => Raylib.DrawLineV(start, end, Color.White));
		}
		else if (game.SpikeExitControlsEnabled)
		{
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
		}

		DrawFuseballScorePopups((text, position, scale) =>
			Raylib.DrawTextEx(Raylib.GetFontDefault(), text, position, scale * 7, scale, Color.White));

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

	public Vector2 ProjectFuseballCenter(Fuseball fuseball)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		// Incitti edges use p2 = points[lane], p1 = points[next]; w runs p2 -> p1.
		Vector2 position = Vector2.Lerp(points[fuseball.EdgeIndex],
			points[NextEdge(fuseball.EdgeIndex, points.Count)], fuseball.W / 8f);
		return Projection.Project(position.X, position.Y, Level.Position + Level.Depth - fuseball.Height,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	public void DrawFuseballs(Action<Vector2, Vector2, Color> drawLine)
	{
		foreach (Fuseball fuseball in game.Fuseballs)
		{
			Vector2 center = ProjectFuseballCenter(fuseball);
			float depth = Level.Position + Level.Depth - fuseball.Height;
			float scale = (8 / ((depth + Projection.ZOffset) / Projection.K) + .25f) / 6;
			for (int arm = 0; arm < 5; arm++)
			{
				int row = game.FuseballAnimationFrame * 5 + arm;
				for (int point = 0; point < 5; point++)
				{
					Vector2 start = new(FuseballArmPoints[row, point * 2], FuseballArmPoints[row, point * 2 + 1]);
					Vector2 end = new(FuseballArmPoints[row, point * 2 + 2], FuseballArmPoints[row, point * 2 + 3]);
					drawLine(center + start * scale, center + end * scale, FuseballArmColors[arm]);
				}
			}
		}
	}

	public void DrawFuseballScorePopups(Action<string, Vector2, float> drawText)
	{
		foreach (FuseballScorePopup popup in game.FuseballScorePopups)
		{
			Vector2 center = ProjectLane(popup.EdgeIndex, Level.Position + Level.Depth - popup.Height);
			drawText(popup.Points.ToString(), center - new Vector2(popup.Scale * 1.5f, 0), popup.Scale);
		}
	}

	public Vector2 ProjectTankerCenter(Tanker tanker) =>
		ProjectLane(tanker.EdgeIndex, Level.Position + Level.Depth - tanker.Height);

	// Tanker: magenta diamond-with-lattice line glyph (Incitti-derived) at the lane midpoint.
	// Scale uses a 26-unit base plus a 1 px floor so the glyph never collapses at the far end.
	public void DrawTankers(Action<Vector2, Vector2> drawLine)
	{
		foreach (Tanker tanker in game.Tankers)
		{
			Vector2 center = ProjectTankerCenter(tanker);
			float depth = Level.Position + Level.Depth - tanker.Height;
			float scale = 26 / ((depth + Projection.ZOffset) / Projection.K) + 1;
			float third = scale / 3;
			Vector2 top = center + new Vector2(0, -scale);
			Vector2 bottom = center + new Vector2(0, scale);
			Vector2 left = center + new Vector2(-scale, 0);
			Vector2 right = center + new Vector2(scale, 0);
			Vector2 topThird = center + new Vector2(0, -third);
			Vector2 bottomThird = center + new Vector2(0, third);
			Vector2 leftThird = center + new Vector2(-third, 0);
			Vector2 rightThird = center + new Vector2(third, 0);

			drawLine(topThird, left);
			drawLine(top, rightThird);
			drawLine(bottom, leftThird);
			drawLine(bottomThird, right);
			drawLine(bottom, right);
			drawLine(bottom, left);
			drawLine(top, right);
			drawLine(top, left);
			drawLine(bottomThird, rightThird);
			drawLine(bottomThird, leftThird);
			drawLine(topThird, rightThird);
			drawLine(topThird, leftThird);
			drawLine(bottomThird, center);
			drawLine(topThird, center);
			drawLine(rightThird, center + new Vector2(scale, 0));
			drawLine(leftThird, center + new Vector2(-scale, 0));
		}
	}

	public Vector2 ProjectEnemyBullet(EnemyBullet bullet)
	{
		Vector2 center = ProjectLane(bullet.EdgeIndex, Level.Position + Level.Depth - bullet.Height);
		return center;
	}

	// Enemy bullet: 8 radial white lines (45° apart) around the lane midpoint, spinning with the
	// bullet's own Rotation. Uses a 6-unit base size; compare DrawPlayerShots, which uses 4.
	public void DrawEnemyBullets(Action<Vector2, Vector2> drawLine)
	{
		foreach (EnemyBullet bullet in game.EnemyBullets)
		{
			Vector2 center = ProjectEnemyBullet(bullet);
			float projectedDepth = Level.Position + Level.Depth - bullet.Height;
			float size = 6 / ((projectedDepth + Projection.ZOffset) / Projection.K);
			for (int angle = 0; angle < 360; angle += 45)
			{
				float radians = (bullet.Rotation + angle) * MathF.PI / 180;
				Vector2 offset = new(MathF.Cos(radians) * size, MathF.Sin(radians) * size);
				drawLine(center - offset, center + offset);
			}
		}
	}

	// Projects the midpoint of a lane (the segment between adjacent rim points) at a given depth.
	// Shared by every entity that renders centered on a lane rather than on a tube edge line.
	private Vector2 ProjectLane(int lane, float depth)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		Vector2 midpoint = (points[lane] + points[NextEdge(lane, points.Count)]) / 2;
		return Projection.Project(midpoint.X, midpoint.Y, depth,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	// Spike: green line anchored at the far web growing toward the rim, with a single white pixel
	// marking the live tip the player can shoot.
	public void DrawSpikes(Action<Vector2, Vector2> drawLine, Action<Vector2> drawTip)
	{
		foreach (Spike spike in game.Spikes)
		{
			Vector2 start = ProjectLane(spike.EdgeIndex, Level.Position + Level.Depth);
			Vector2 tip = ProjectLane(spike.EdgeIndex, Level.Position + Level.Depth - spike.Height);
			drawLine(start, tip);
			drawTip(tip);
		}
	}

	// Spiker: green spiral approximated by 14 chord segments, 60° apart, with radius growing
	// outward per segment (segment indexes run 0..-13 so the spiral winds inward to the center).
	// Rotation advances 45° per simulation update in the gameplay code; the renderer only reads it.
	// Scale uses a 22-unit base plus a 2 px floor (Incitti-derived constants).
	public void DrawSpikers(Action<Vector2, Vector2> drawLine)
	{
		foreach (Spiker spiker in game.Spikers)
		{
			float depth = Level.Position + Level.Depth - spiker.Height;
			Vector2 center = ProjectLane(spiker.EdgeIndex, depth);
			float scale = 22 / ((depth + Projection.ZOffset) / Projection.K) + 2;
			for (int segment = 0; segment >= -13; segment--)
			{
				float angle1 = (spiker.Rotation + segment * 60) * MathF.PI / 180;
				float angle2 = (spiker.Rotation + (segment + 1) * 60) * MathF.PI / 180;
				Vector2 first = new(MathF.Sin(angle1), MathF.Cos(angle1));
				Vector2 second = new(MathF.Sin(angle2), MathF.Cos(angle2));
				drawLine(center + first * segment * scale / 16,
					center + second * (segment + 1) * scale / 16);
			}
		}
	}

	// Egg heights are negative while roaming (0 means ready to hatch at the web), so the ×20
	// amplification pushes the projected depth beyond Level.Depth: Eggs visibly bob behind and
	// deeper than the far web until they approach and hatch.
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

	// Player shot: white rotating 8-radial star like EnemyBullet, but with a smaller 4-unit base
	// size. Shot.Height decreases as it travels away from the player, so projected depth grows and
	// the star naturally shrinks into the tube.
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
		// While flipping, the glyph is anchored to the source lane and ApplyFlipperPivot swings
		// one endpoint toward the destination. Drawing at EdgeIndex instead would make the Flipper
		// jump lanes the instant the simulation retargets it, losing the signature hinge motion.
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

	// Rotates the trailing endpoint of the source lane about the leading endpoint by the pivot
	// progress so far. TurnAngle grows each update toward TotalTurnAngle; interpolating between
	// the lanes (rather than snapping) is what makes the Flipper appear to cartwheel edge-to-edge.
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

	// Flipper body: six red lines forming the bow-tie glyph between the two projected lane
	// endpoints. Fractions along the edge and the normal-based height are Incitti-derived.
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

	// Public (rather than private like the other player helpers) so headless tests can verify the
	// player's projected edge during normal play and exit travel without a graphics window.
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

	// Death burst: 8 asymmetric radials (short inner tail, long outer ray) expanding over the
	// fixed 60-tick death freeze.
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
		if (game.TankerTest)
		{
			Raylib.DrawText($"ORIGINAL {game.TankerOriginalSlotsRemaining}", 10, 72, 18, Color.Red);
			Raylib.DrawText($"CARGO {game.TankerCargoUnitsRemaining}", 10, 96, 18, TankerColor);
			Raylib.DrawText($"ACTIVE {game.TankerActiveTotal}", 10, 120, 18, Color.Gray);
		}
		else
		{
			int remaining = Level.EnemyTotal - game.CurrentLevel.EnemiesDestroyed;
			string enemyLabel = game.FuseballTest ? "FUSEBALLS" :
				game.CurrentLevel.SpikerDevelopmentWave ? "ENEMIES" : "FLIPPERS";
			Raylib.DrawText($"{enemyLabel} {remaining}", 10, 72, 18, Color.Red);
			Raylib.DrawText($"ACTIVE {game.Eggs.Count + game.Flippers.Count + game.Spikers.Count + game.Fuseballs.Count}", 10, 96, 18, Color.Gray);
		}

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
			Raylib.DrawText("PLAYER DESTROYED", 10, game.TankerTest ? 144 : 120, 18, Color.Red);
	}

	private void DrawCenteredText(string text, int y, int size, Color color)
	{
		int width = Raylib.MeasureText(text, size);
		Raylib.DrawText(text, (ScreenWidth - width) / 2, y, size, color);
	}
}
