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
/// tables. Glyph shapes generally follow Incitti; scales remain project choices for arcade
/// comparison.
/// Most Draw*/Project* members take line/point callbacks or are public so headless tests can
/// capture the exact submitted geometry. Those tests can prove submissions are finite and placed
/// at the expected lanes/depths, but not native framebuffer output or arcade visual fidelity.
/// </summary>
public sealed class VectorRenderer
{
	public const int ScreenWidth = DisplayConfiguration.WindowWidth;
	public const int ScreenHeight = DisplayConfiguration.WindowHeight;
	public const int EggPointSize = 3;
	public const float PlayerShotBaseSize = 4.5f;
	// Experimental viewpoint elevation, in pre-projection units; not a measured Atari angle.
	public const float InitialApproachVerticalOffset = 96;
	private static readonly Color SpikerColor = new(0, 255, 0, 255);
	private static readonly Color TankerColor = new(255, 0, 255, 255);
	private static readonly Color PulsarColor = new(0, 255, 255, 255);
	// PROJECT TRANSLATION: use established Tempest colors; ROM palette $0C is not an RGB value.
	private static readonly Color[] GameOverVectorColors =
	[
		Color.White, Color.Yellow, SpikerColor, Color.Red, PulsarColor, TankerColor, Color.Blue
	];
	private static readonly Color GameOverTextColor = Color.White;
	private static readonly Color PaletteZeroBlasterLaneColor = new(255, 255, 0, 255);
	private static readonly Color DeathOrangeColor = new(255, 80, 0, 255);
	private const int GameOverVectorBaseSpecksPerParent = 2;
	private const float GameOverVectorMarkLength = 2f;
	// PROJECT APPROXIMATION: deterministic child specks stay compact around their ROM parent.
	private const float GameOverVectorClusterRadius = 3f;
	private const double GameOverVectorGoldenAngle = 2.39996322972865332;
	private static readonly Color[] FuseballArmColors =
	[
		new(255, 255, 0, 255), new(0, 255, 0, 255), new(255, 0, 255, 255),
		new(0, 255, 255, 255), new(255, 0, 0, 255)
	];
	private static readonly float[] DeathStarburstRadii =
	[
		1, .34f, .78f, .30f, 1, .40f, .72f, .34f,
		1, .34f, .78f, .30f, 1, .40f, .72f, .34f
	];
	private static readonly DeathEffectLayer[] DeathEffectLayers =
	[
		new(Color.Yellow, .88f, -.055f),
		new(DeathOrangeColor, 1f, .025f),
		new(Color.White, .76f, .075f)
	];
	private static readonly DeathEffectKeyframe[] DeathEffectEnvelope =
	[
		new(0, .12f),
		new(.10f, .28f),
		new(.25f, .70f),
		new(.40f, 1f),
		new(.52f, .92f),
		new(.68f, .62f),
		new(.82f, .32f),
		new(.94f, .14f),
		new(1, 0)
	];
	private const float DeathEffectPeakRadius = 52;
	private const float DeathEffectVisualScale = 1.5f;

	private readonly record struct DeathEffectLayer(Color Color, float Scale, float Rotation);
	private readonly record struct DeathEffectKeyframe(float Age, float Scale);

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
	private readonly double gameOverDensityMultiplier;
	public IReadOnlyList<Egg> Eggs => game.Eggs;
	public bool ShouldDrawEnemyBullets =>
		game.NaturalProgressionEnabled || game.EnemyBulletsTest || game.CurrentLevel.UsesProductionLedger;
	// During normal play Player.Depth == Level.Depth, so this is Level.Position (the near rim).
	// During exit travel Player.Depth shrinks toward 0, moving the projected player down the tube.
	public float PlayerProjectedDepth => Level.Position + Level.Depth - game.Player.Depth;
	// Claw height is 20 px at the gameplay rim. It scales by the projection ratio during approach
	// and exit travel so it remains consistent with the tube.
	public float ApproachDepthOffset => game.State == LevelState.Approach
		? (game.CurrentApproach ??
			throw new InvalidOperationException("The approach state has no transition model.")).DepthOffset
		: 0;
	public float ApproachVerticalOffset =>
		InitialApproachVerticalOffset * (ApproachDepthOffset / LevelApproach.InitialDepthOffset);
	public float PlayerClawHeight => game.State == LevelState.Approach
		? 20 * (PlayerProjectedDepth + Projection.ZOffset) /
			(PlayerProjectedDepth + ApproachDepthOffset + Projection.ZOffset)
		: game.Player.Depth == Level.Depth
			? 20
			: 20 * ((Level.Position + 5) / 70) / ((PlayerProjectedDepth + Projection.ZOffset) / Projection.K);

	public VectorRenderer(TempestGame game, double gameOverDensityMultiplier = 2.0)
	{
		if (!double.IsFinite(gameOverDensityMultiplier) || gameOverDensityMultiplier <= 0)
			throw new ArgumentOutOfRangeException(nameof(gameOverDensityMultiplier),
				"The Game Over density multiplier must be a positive finite value.");
		this.game = game;
		this.gameOverDensityMultiplier = gameOverDensityMultiplier;
	}

	private Vector2 ProjectApproachGeometry(Vector2 point, float depth) =>
		Projection.Project(point.X, point.Y, depth + ApproachDepthOffset,
			game.CurrentLevel.Definition.CenterY,
			game.CurrentLevel.Definition.YOffset + ApproachVerticalOffset);

	public void Draw()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Black);
		Raylib.BeginMode2D(DisplayConfiguration.PresentationCamera);

		if (game.State == LevelState.GameOverAnimation)
		{
			DrawGameOverAnimation((start, end, color) => Raylib.DrawLineV(start, end, color));
			Raylib.EndMode2D();
			Raylib.EndDrawing();
			return;
		}

		DrawTransitionStars((position, color) => Raylib.DrawPixelV(position, color));
		DrawTube((start, end, color) => Raylib.DrawLineV(start, end, color));
		bool approaching = game.State == LevelState.Approach;
		if (!approaching)
			DrawPulsingLaneBoundaries((start, end, color) => Raylib.DrawLineV(start, end, color));
		// Spikes stay visible (and lethal) during exit travel; all other enemies are hidden once
		// the exit starts. Player shots remain visible only in the spike-exit scenario, where the
		// player may still fire while travelling.
		DrawSpikes((start, end) => Raylib.DrawLineV(start, end, SpikerColor),
			point => Raylib.DrawPixelV(point, Color.White));
		if (!game.IsExiting && !approaching)
		{
			DrawEggs();
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
			DrawFlippers(game.CurrentLevel.Definition.Points);
			DrawSpikers((start, end) => Raylib.DrawLineV(start, end, SpikerColor));
			DrawTankers((start, end) => Raylib.DrawLineV(start, end, TankerColor));
			DrawFuseballs((start, end, color) => Raylib.DrawLineV(start, end, color));
			DrawPulsars((start, end, color) => Raylib.DrawLineV(start, end, color));
			if (ShouldDrawEnemyBullets)
				DrawEnemyBullets((start, end) => Raylib.DrawLineV(start, end, Color.White));
		}
		else if (game.SpikeExitShotsVisible)
		{
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
		}

		if (!approaching)
			DrawFuseballScorePopups((text, position, scale) =>
				Raylib.DrawTextEx(Raylib.GetFontDefault(), text, position, scale * 7, scale, Color.White));

		DrawAvoidSpikesWarning((text, position, scale) =>
			Raylib.DrawTextEx(Raylib.GetFontDefault(), text, position, scale * 7, scale, Color.White));

		if (!game.GameOver &&
			game.State is not (LevelState.GameOverPresentation or LevelState.GameOverAnimation))
		{
			if (game.DeathTicks > 0)
				DrawPlayerExplosion(game.CurrentLevel.Definition.Points,
					(start, end, color) => Raylib.DrawLineV(start, end, color));
			else
				DrawPlayer(game.CurrentLevel.Definition.Points);
		}

		DrawGameOverMessage((start, end, color) => Raylib.DrawLineV(start, end, color));
		DrawGameOverVectorRecords((start, end, color) => Raylib.DrawLineV(start, end, color));
		DrawRechargeMessage(DrawCenteredText);
		DrawHud();
		Raylib.EndMode2D();
		Raylib.EndDrawing();
	}

	public void DrawGameOverAnimation(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		DrawGameOverVectorRecords(drawLine);
	}

	private static int NextEdge(int edge, int edgeCount) => (edge + 1) % edgeCount;

	public void DrawTransitionStars(Action<Vector2, Color> drawPoint)
	{
		ArgumentNullException.ThrowIfNull(drawPoint);
		if (game.State == LevelState.ExitZoom)
		{
			TransitionStarfield.Draw(game.CurrentLevel.Configuration.DisplayedLevel,
				TransitionStarfield.ExitVisibility(game.Player.Depth, Level.Depth), drawPoint);
		}
		else if (game.State == LevelState.Approach)
		{
			LevelApproach approach = game.CurrentApproach ??
				throw new InvalidOperationException("The approach state has no transition model.");
			TransitionStarfield.Draw(approach.ExitedLevel,
				TransitionStarfield.ApproachVisibility(approach.Progress), drawPoint);
		}
	}

	public void DrawRechargeMessage(Action<string, int, int, Color> drawCenteredText)
	{
		ArgumentNullException.ThrowIfNull(drawCenteredText);
		if (game.State == LevelState.Approach)
			drawCenteredText("SUPERZAPPER RECHARGE", 540, 20, Color.White);
	}

	public Vector2 ProjectFuseballCenter(Fuseball fuseball)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		// Incitti edges use p2 = points[lane], p1 = points[next]; w runs p2 -> p1.
		Vector2 position = Vector2.Lerp(points[fuseball.EdgeIndex],
			points[NextEdge(fuseball.EdgeIndex, points.Count)], fuseball.W / 8f);
		return Projection.Project(position.X, position.Y, ProjectEnemyDepth(fuseball.Height),
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	public void DrawFuseballs(Action<Vector2, Vector2, Color> drawLine)
	{
		foreach (Fuseball fuseball in game.Fuseballs)
		{
			Vector2 center = ProjectFuseballCenter(fuseball);
			float depth = ProjectEnemyDepth(fuseball.Height);
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

	public void DrawPulsars(Action<Vector2, Vector2, Color> drawLine)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		foreach (Pulsar pulsar in game.Pulsars)
		{
			float projectedDepth = ProjectEnemyDepth(pulsar.Height);
			Vector2 p1;
			Vector2 p2;
			if (pulsar.State is PulsarState.Flipping or PulsarState.FlippingWithoutClimb)
			{
				DirectedLaneTransition transition = pulsar.ActiveTransition ??
					throw new InvalidOperationException("A rendered Pulsar flip requires an active transition.");
				float progress = pulsar.TotalTurnAngle <= 0
					? 1
					: Math.Clamp(pulsar.TurnAngle / pulsar.TotalTurnAngle, 0, 1);
				(Vector2 start, Vector2 end) =
					game.CurrentLevel.CalculateFlipperPivotEndpoints(transition, progress);
				p1 = Projection.Project(end.X, end.Y, projectedDepth,
					game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
				p2 = Projection.Project(start.X, start.Y, projectedDepth,
					game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			}
			else
			{
				int next = NextEdge(pulsar.EdgeIndex, points.Count);
				p1 = Projection.Project(points[next].X, points[next].Y, projectedDepth,
					game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
				p2 = Projection.Project(points[pulsar.EdgeIndex].X, points[pulsar.EdgeIndex].Y,
					projectedDepth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			}

			Vector2 delta = p2 - p1;
			float length = delta.Length();
			if (length == 0)
				continue;

			Vector2 normal = new(-delta.Y / length, delta.X / length);
			float offset = game.PulseZh;
			Vector2[] vertices =
			[
				p2,
				Vector2.Lerp(p2, p1, .2f) - normal * offset,
				Vector2.Lerp(p2, p1, .35f) + normal * offset,
				Vector2.Lerp(p2, p1, .5f) - normal * offset,
				Vector2.Lerp(p2, p1, .65f) + normal * offset,
				Vector2.Lerp(p2, p1, .8f) - normal * offset,
				p1
			];
			Color color = game.Pulsing ? Color.White : PulsarColor;
			for (int index = 0; index < vertices.Length - 1; index++)
				drawLine(vertices[index], vertices[index + 1], color);
		}
	}

	public void DrawPulsingLaneBoundaries(Action<Vector2, Vector2, Color> drawLine)
	{
		if (!game.Pulsing)
			return;

		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		HashSet<int> drawnEdges = [];
		Color color = game.PulsePhase % 8 > 3 ? PulsarColor : Color.White;
		foreach (Pulsar pulsar in game.Pulsars)
		{
			if (!IsPulsingLane(pulsar.EdgeIndex) ||
				!drawnEdges.Add(pulsar.EdgeIndex))
				continue;

			int next = NextEdge(pulsar.EdgeIndex, points.Count);
			Vector2 nearStart = Projection.Project(points[pulsar.EdgeIndex].X, points[pulsar.EdgeIndex].Y,
				Level.Position, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			Vector2 farStart = Projection.Project(points[pulsar.EdgeIndex].X, points[pulsar.EdgeIndex].Y,
				Level.Position + Level.Depth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			Vector2 nearEnd = Projection.Project(points[next].X, points[next].Y,
				Level.Position, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
			Vector2 farEnd = Projection.Project(points[next].X, points[next].Y,
				Level.Position + Level.Depth, game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);

			drawLine(nearStart, farStart, color);
			drawLine(nearEnd, farEnd, color);
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

	public void DrawAvoidSpikesWarning(Action<string, Vector2, float> drawText)
	{
		ArgumentNullException.ThrowIfNull(drawText);
		if (!game.ShouldShowAvoidSpikesWarning)
			return;

		drawText("Avoid Spikes", new Vector2(200, 260), 8);
	}

	public void DrawGameOverMessage(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (!game.GameOverMessageVisible)
			return;

		VectorFont.DrawGameOver(
			new Vector2(DisplayConfiguration.LogicalWidth / 2f,
				DisplayConfiguration.LogicalHeight / 2f),
			4,
			(start, end) => drawLine(start, end, GameOverTextColor));
	}

	public void DrawGameOverVectorRecords(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (game.State != LevelState.GameOverAnimation)
			return;

		foreach (GameOverVectorAnimation burst in game.GameOverEffect.Bursts)
		{
			foreach (GameOverVectorRecord? record in burst.Records)
			{
				if (record is null)
					continue;

				Color color = GameOverVectorColors[record.SpawnOrdinal % GameOverVectorColors.Length];
				float depth = Level.Position + record.Z;
				Vector2 center = Projection.Project(
					(record.X - 0x80) * 1.5f,
					(record.Y - 0x80) * 1.5f,
					depth,
					game.CurrentLevel.Definition.CenterY,
					game.CurrentLevel.Definition.YOffset);

				int markCount = GetDensityScaledSpeckCount(record.SpawnOrdinal);
				for (int speck = 0; speck < markCount; speck++)
				{
					Vector2 markCenter = center + GetGameOverSpeckOffset(
						burst.BurstIndex, record.SpawnOrdinal, speck);
					drawLine(
						markCenter - new Vector2(GameOverVectorMarkLength / 2, 0),
						markCenter + new Vector2(GameOverVectorMarkLength / 2, 0),
						color);
				}
			}
		}
	}

	private int GetDensityScaledSpeckCount(int parentSpawnOrdinal)
	{
		double start = parentSpawnOrdinal * GameOverVectorBaseSpecksPerParent *
			gameOverDensityMultiplier;
		double end = (parentSpawnOrdinal + 1) * GameOverVectorBaseSpecksPerParent *
			gameOverDensityMultiplier;
		return checked((int)(Math.Floor(end) - Math.Floor(start)));
	}

	private static Vector2 GetGameOverSpeckOffset(
		int burstIndex,
		int parentSpawnOrdinal,
		int childSpeckOrdinal)
	{
		int patternOrdinal = burstIndex * GameOverVectorAnimation.RecordCapacity +
			parentSpawnOrdinal;
		uint parentHash = unchecked((uint)patternOrdinal * 0x9E3779B9);
		parentHash ^= parentHash >> 16;
		parentHash *= 0x7FEB352D;
		parentHash ^= parentHash >> 15;
		double parentRotation = parentHash / (double)uint.MaxValue * Math.Tau;
		double angle = parentRotation + childSpeckOrdinal * GameOverVectorGoldenAngle;
		float radius = Math.Min(GameOverVectorClusterRadius,
			0.85f * MathF.Sqrt(childSpeckOrdinal + 1));
		return new Vector2(
			MathF.Cos((float)angle) * radius,
			MathF.Sin((float)angle) * radius);
	}

	public Vector2 ProjectTankerCenter(Tanker tanker) =>
		ProjectLane(tanker.EdgeIndex, ProjectEnemyDepth(tanker.Height));

	// Tanker: magenta diamond-with-lattice line glyph (Incitti-derived) at the lane midpoint.
	// Scale uses a 26-unit base plus a 1 px floor so the glyph never collapses at the far end.
	public void DrawTankers(Action<Vector2, Vector2> drawLine)
	{
		foreach (Tanker tanker in game.Tankers)
		{
			Vector2 center = ProjectTankerCenter(tanker);
			float depth = ProjectEnemyDepth(tanker.Height);
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
		return ProjectApproachGeometry(midpoint, depth);
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
			float depth = ProjectEnemyDepth(spiker.Height);
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

	private bool IsPulsingLane(int edge) => game.Pulsars.Any(pulsar =>
		pulsar.EdgeIndex == edge && Pulsar.IsLanePulsing(pulsar.Height, game.Pulsing));

	private bool ShouldHighlightPlayerLane => game.State is
		LevelState.Playing or LevelState.Approach or LevelState.PlayerDying or LevelState.AvoidSpikes;

	private Color ResolvePlayerLaneColor() =>
		game.CurrentLevel.Configuration.PaletteIndex switch
		{
			0 => PaletteZeroBlasterLaneColor,
			_ => throw new InvalidOperationException(
				$"No Blaster-lane color is configured for palette {game.CurrentLevel.Configuration.PaletteIndex}.")
		};

	private Color ResolveSuperZapperRailColor() =>
		// Project RGB translations; the local ROM's $c1fd palette bytes are omitted.
		game.SuperZapper.TubeColorIndex switch
		{
			0 => Color.White,
			1 => ResolvePlayerLaneColor(),
			2 => SpikerColor,
			3 => Color.Red,
			4 => PulsarColor,
			5 => TankerColor,
			6 => Color.Blue,
			_ => throw new InvalidOperationException("Invalid Super Zapper rail color index.")
		};

	public void DrawTube(Action<Vector2, Vector2, Color> drawLine)
	{
		IReadOnlyList<Vector2> points = game.CurrentLevel.Definition.Points;
		Vector2[] nearRim = new Vector2[points.Count];
		Vector2[] farRim = new Vector2[points.Count];
		// Atari patches longitudinal rail colors, not the rim connections ($b44b, $c358).
		Color tubeColor = Color.Blue;
		Color railColor = ResolveSuperZapperRailColor();
		int playerLane = game.Player.EdgeIndex;
		int nextPlayerLaneBoundary = game.CurrentLevel.NextEdge(playerLane);
		bool highlightPlayerLane = ShouldHighlightPlayerLane;
		Color playerLaneColor = highlightPlayerLane ? ResolvePlayerLaneColor() : tubeColor;

		for (int i = 0; i < points.Count; i++)
		{
			nearRim[i] = ProjectApproachGeometry(points[i], Level.Position);
			farRim[i] = ProjectApproachGeometry(points[i], Level.Position + Level.Depth);
		}

		for (int i = 0; i < points.Count; i++)
		{
			bool playerLaneBoundary = highlightPlayerLane &&
				(i == playerLane || i == nextPlayerLaneBoundary);
			drawLine(nearRim[i], farRim[i], playerLaneBoundary ? playerLaneColor : railColor);
		}

		for (int i = 0; i < points.Count; i++)
		{
			int next = NextEdge(i, points.Count);
			// Arcade observation: only the pulsing lane's near-rim connection disappears.
			// Incitti also hides rims outside the pulse; retain normal geometry otherwise.
			if (!IsPulsingLane(i))
				drawLine(nearRim[i], nearRim[next], tubeColor);
			drawLine(farRim[i], farRim[next], tubeColor);
		}
	}

	// Shot.Height decreases as it travels away from the player, so projected depth grows and the
	// star naturally shrinks into the tube.
	private void DrawPlayerShots(IReadOnlyList<Vector2> points)
	{
		DrawPlayerShots(points, (start, end) => Raylib.DrawLineV(start, end, Color.White));
	}

	public void DrawPlayerShots(Action<Vector2, Vector2> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		DrawPlayerShots(game.CurrentLevel.Definition.Points, drawLine);
	}

	private void DrawPlayerShots(
		IReadOnlyList<Vector2> points,
		Action<Vector2, Vector2> drawLine)
	{
		foreach (Shot shot in game.Shots)
		{
			Vector2 center = ProjectShot(shot, points);
			float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
				game.CurrentLevel.Configuration.Position;
			float size = PlayerShotBaseSize /
				((projectedDepth + Projection.ZOffset) / Projection.K);
			DrawRotatingShot(center, size, shot.Rotation, drawLine);
		}
	}

	public Vector2 ProjectShot(Shot shot)
	{
		ArgumentNullException.ThrowIfNull(shot);
		return ProjectShot(shot, game.CurrentLevel.Definition.Points);
	}

	private Vector2 ProjectShot(Shot shot, IReadOnlyList<Vector2> points)
	{
		int next = NextEdge(shot.EdgeIndex, points.Count);
		Vector2 midpoint = (points[shot.EdgeIndex] + points[next]) / 2;
		float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
			game.CurrentLevel.Configuration.Position;
		return Projection.Project(midpoint.X, midpoint.Y, projectedDepth,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
	}

	private void DrawRotatingShot(
		Vector2 center,
		float size,
		float rotation,
		Action<Vector2, Vector2> drawLine)
	{
		for (int angle = 0; angle < 360; angle += 45)
		{
			float radians = (rotation + angle) * MathF.PI / 180;
			Vector2 offset = new(
				MathF.Cos(radians) * size,
				MathF.Sin(radians) * size);
			drawLine(center - offset, center + offset);
		}
	}

	private void DrawFlippers(IReadOnlyList<Vector2> points)
	{
		foreach (Flipper flipper in game.Flippers)
			DrawFlipper(points, flipper);
	}

	private void DrawFlipper(IReadOnlyList<Vector2> points, Flipper flipper)
	{
		float projectedDepth = ProjectEnemyDepth(flipper.Height);
		(Vector2 p1, Vector2 p2) = ProjectFlipperEndpoints(flipper, points, projectedDepth);
		DrawFlipperShape(p1, p2, projectedDepth);
	}

	public (Vector2 Start, Vector2 End) ProjectFlipperEndpoints(Flipper flipper)
	{
		ArgumentNullException.ThrowIfNull(flipper);
		float projectedDepth = ProjectEnemyDepth(flipper.Height);
		return ProjectFlipperEndpoints(flipper, game.CurrentLevel.Definition.Points, projectedDepth);
	}

	private float ProjectEnemyDepth(float height)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		float projectedDepth = configuration.Position + configuration.Depth - height;
		float retreatOffset = configuration.Depth * game.EnemyRetractionProgress;
		// Retreat is a render-only projection offset; gameplay height and lane remain untouched.
		return retreatOffset == 0
			? projectedDepth
			: Math.Clamp(projectedDepth + retreatOffset,
				configuration.Position, configuration.Position + 2 * configuration.Depth);
	}

	private (Vector2 Start, Vector2 End) ProjectFlipperEndpoints(
		Flipper flipper, IReadOnlyList<Vector2> points, float projectedDepth)
	{
		if (flipper.State == FlipperState.Flipping)
		{
			DirectedLaneTransition transition = flipper.ActiveTransition ??
				throw new InvalidOperationException("A rendered Flipper pivot requires an active transition.");
			float progress = flipper.TotalTurnAngle <= 0
				? 1
				: Math.Clamp(flipper.TurnAngle / flipper.TotalTurnAngle, 0, 1);
			(Vector2 start, Vector2 end) = game.CurrentLevel.CalculateFlipperPivotEndpoints(
				transition, progress);
			return (
				Projection.Project(start.X, start.Y, projectedDepth,
					game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset),
				Projection.Project(end.X, end.Y, projectedDepth,
					game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset));
		}

		// During a legacy pivot the glyph stays on the source lane until completion.
		int edge = flipper.State == FlipperState.Flipping
			? flipper.SourceEdge
			: flipper.EdgeIndex;
		int next = NextEdge(edge, points.Count);
		Vector2 p1 = Projection.Project(points[edge].X, points[edge].Y, projectedDepth,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);
		Vector2 p2 = Projection.Project(points[next].X, points[next].Y, projectedDepth,
			game.CurrentLevel.Definition.CenterY, game.CurrentLevel.Definition.YOffset);

		return (p1, p2);
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
		p1 = ProjectApproachGeometry(points[game.Player.EdgeIndex], PlayerProjectedDepth);
		p2 = ProjectApproachGeometry(points[next], PlayerProjectedDepth);
	}

	public void DrawPlayerExplosion(
		IReadOnlyList<Vector2> points,
		Action<Vector2, Vector2, Color> drawLine)
	{
		if (game.DeathTicks <= 0)
			return;

		GetPlayerEdge(points, out Vector2 p1, out Vector2 p2);
		Vector2 center = Vector2.Lerp(p2, p1, game.Player.EdgePosition);
		float age = 1 - game.DeathTicks / (float)TempestGame.PlayerDeathTicks;
		float radius = DeathEffectPeakRadius * DeathEffectVisualScale *
			SampleDeathEffectScale(age);

		foreach (DeathEffectLayer layer in DeathEffectLayers)
		{
			Vector2 previous = GetDeathEffectVertex(center, radius, layer, 0);
			for (int index = 1; index <= DeathStarburstRadii.Length; index++)
			{
				Vector2 next = GetDeathEffectVertex(
					center, radius, layer, index % DeathStarburstRadii.Length);
				drawLine(previous, next, layer.Color);
				previous = next;
			}
		}
	}

	private static Vector2 GetDeathEffectVertex(
		Vector2 center, float radius, DeathEffectLayer layer, int index)
	{
		float angle = index * MathF.Tau / DeathStarburstRadii.Length + layer.Rotation;
		float vertexRadius = radius * layer.Scale * DeathStarburstRadii[index];
		return center + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * vertexRadius;
	}

	private static float SampleDeathEffectScale(float age)
	{
		for (int index = 1; index < DeathEffectEnvelope.Length; index++)
		{
			DeathEffectKeyframe next = DeathEffectEnvelope[index];
			if (age > next.Age)
				continue;

			DeathEffectKeyframe previous = DeathEffectEnvelope[index - 1];
			float progress = (age - previous.Age) / (next.Age - previous.Age);
			return previous.Scale + (next.Scale - previous.Scale) * progress;
		}

		return 0;
	}

	private void DrawHud()
	{
		Raylib.DrawText(game.Score.Score.ToString(), 10, 10, 28, Color.Yellow);
		Raylib.DrawText($"LIVES {game.Player.Lives}", 10, 44, 20, Color.Yellow);
		Raylib.DrawText($"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel}", 640, 10, 20, Color.Yellow);
		IReadOnlyList<string> waveLines = GetWaveHudLines();
		for (int index = 0; index < waveLines.Count; index++)
		{
			Color color = index == 0 ? Color.Red :
				(game.TankerTest || game.CurrentLevel.UsesProductionLedger) && index == 1
					? TankerColor
					: Color.Gray;
			Raylib.DrawText(waveLines[index], 10, 72 + index * 24, 18, color);
		}
		if (game.Level2Test)
			Raylib.DrawText("LEVEL 2 TEST - DEVELOPMENT ONLY", 10, 120, 16, Color.Gray);
		else if (game.Level3Test)
			Raylib.DrawText("LEVEL 3 MIXED WAVE TEST - DEVELOPMENT ONLY", 10, 144, 16, Color.Gray);
		else if (game.Level3GeometryTest)
			Raylib.DrawText("LEVEL 3 GEOMETRY DIAGNOSTIC - DEVELOPMENT ONLY", 10, 120, 16, Color.Gray);

		if (game.GameOver)
		{
			DrawCenteredText("GAME OVER - PRESS R", 270, 32, Color.Red);
			return;
		}

		if (game.LevelComplete)
		{
			if (game.NaturalProgressionEnabled &&
				LevelProgression.GetNextProductionLevel(game.CurrentLevel.Configuration) is null)
			{
				DrawCenteredText($"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel} COMPLETE",
					230, 36, Color.Yellow);
				DrawCenteredText("PRESS R TO START A NEW CAMPAIGN", 276, 24, Color.Gray);
			}
			else
			{
				DrawCenteredText(game.Level3Test ? "LEVEL 3 MIXED WAVE TEST COMPLETE" :
					game.Level3GeometryTest ? "LEVEL 3 GEOMETRY DIAGNOSTIC COMPLETE" :
					game.Level2Test ? "LEVEL 2 TEST COMPLETE" :
					$"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel} COMPLETE",
					250, 36, Color.Yellow);
				DrawCenteredText(game.Level3Test ? "PRESS R TO REPLAY LEVEL 3 TEST" :
					game.Level3GeometryTest ? "PRESS R TO REPLAY DIAGNOSTIC" :
					game.Level2Test ? "PRESS R TO REPLAY TEST" : "PRESS R TO REPLAY",
					296, 24, Color.Gray);
			}
			return;
		}

		if (game.DeathTicks > 0)
			Raylib.DrawText("PLAYER DESTROYED", 10,
				game.Level3Test ? 168 :
				game.TankerTest || game.Level2Test || game.Level3GeometryTest ? 144 : 120,
				18, Color.Red);
	}

	public IReadOnlyList<string> GetWaveHudLines()
	{
		if (game.State == LevelState.Approach)
			return [];

		if (game.TankerTest)
		{
			return
			[
				$"ORIGINAL {game.TankerOriginalSlotsRemaining}",
				$"CARGO {game.TankerCargoUnitsRemaining}",
				$"ACTIVE {game.TankerActiveTotal}"
			];
		}

		if (game.CurrentLevel.UsesProductionLedger)
		{
			Level3WaveLedger ledger = game.CurrentLevel.Level3Ledger
				?? throw new InvalidOperationException("The mixed Level 3 wave has no identity ledger.");
			return
			[
				$"ORIGINAL {ledger.UnresolvedOriginalIds.Count}",
				$"CARGO {ledger.UnresolvedCargoIds.Count}",
				$"ACTIVE {game.Eggs.Count + game.Flippers.Count + game.Tankers.Count + game.Spikers.Count}"
			];
		}

		int remaining = game.CurrentLevel.RemainingEnemies;
		string enemyLabel = game.PulsarTest ? "PULSARS" :
			game.FuseballTest ? "FUSEBALLS" :
			game.CurrentLevel.SpikerDevelopmentWave ? "ENEMIES" : "FLIPPERS";
		return
		[
			$"{enemyLabel} {remaining}",
			$"ACTIVE {game.Eggs.Count + game.Flippers.Count + game.Spikers.Count + game.Fuseballs.Count + game.Pulsars.Count}"
		];
	}

	private void DrawCenteredText(string text, int y, int size, Color color)
	{
		int width = Raylib.MeasureText(text, size);
		Raylib.DrawText(text, (DisplayConfiguration.LogicalWidth - width) / 2, y, size, color);
	}
}
