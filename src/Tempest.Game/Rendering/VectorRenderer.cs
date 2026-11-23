using System.Numerics;
using Raylib_cs;
using Tempest.Game.Debug;
using Tempest.Game.Gameplay;
using Tempest.Game.Levels;

namespace Tempest.Game.Rendering;

/// <summary>
/// Draws the simulation as Atari-style vector line art via Raylib. Depth convention: gameplay
/// Height runs from 0 (far end of the tube) up to the configured depth (near rim), so every entity
/// converts to projection depth as Position + Depth - Height; smaller projected depth is closer
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
	public const int DevelopmentPortraitGuideTopY = 180;
	public const int DevelopmentPortraitGuideBottomY = 1210;
	// Experimental viewpoint elevation, in pre-projection units; not a measured Atari angle.
	public const float InitialApproachVerticalOffset = 96;
	private static readonly Color SpikerColor = new(0, 255, 0, 255);
	private static readonly Color TankerColor = new(255, 0, 255, 255);
	private static readonly Color PulsarColor = new(0, 255, 255, 255);
	private static readonly Color PortraitViewportBorderColor = Color.Blue;
	// PROJECT TRANSLATION: use established Tempest colors; ROM palette $0C is not an RGB value.
	private static readonly Color[] GameOverVectorColors =
	[
		Color.White, Color.Yellow, SpikerColor, Color.Red, PulsarColor, TankerColor, Color.Blue
	];
	private static readonly Color GameOverTextColor = Color.White;
	private static readonly Color AvoidSpikesWarningColor = Color.White;
	// Preserve the former text center and 56-pixel height (seven vector units at scale 8).
	private static readonly Vector2 AvoidSpikesWarningCenter = new(400, 288);
	private const float AvoidSpikesWarningScale = 8;
	private static readonly Color PaletteZeroBlasterLaneColor = new(255, 255, 0, 255);
	private static readonly Color DeathOrangeColor = new(255, 80, 0, 255);
	// Type 0 uses graphic-table entries 0, 0, 1, 2 at ages 0, 3, 6, 9. These
	// single-color programs are the ROM's size-1, size-2, and size-3 explosions.
	private static readonly (bool Draw, Vector2 Offset)[] TypeZeroExplosionSizeOneVectors =
	[
		(false, new(7, 3)), (true, new(-14, -6)),
		(false, new(1, -3)), (true, new(12, 12)),
		(false, new(-3, 1)), (true, new(-6, -14)),
		(false, new(3, -1)), (true, new(0, 16)),
		(false, new(-3, -1)), (true, new(6, -14)),
		(false, new(3, 1)), (true, new(-12, 12)),
		(false, new(-1, -3)), (true, new(14, -6)),
		(false, new(1, 3)), (true, new(-16, 0))
	];
	private static readonly (bool Draw, Vector2 Offset)[] TypeZeroExplosionSizeTwoVectors =
	[
		(false, new(14, 6)), (true, new(-28, -10)),
		(false, new(2, -6)), (true, new(24, 26)),
		(false, new(-6, 2)), (true, new(-12, -26)),
		(false, new(6, -2)), (true, new(0, 32)),
		(false, new(-6, -2)), (true, new(12, -26)),
		(false, new(6, 2)), (true, new(-24, 26)),
		(false, new(-2, -6)), (true, new(28, -10)),
		(false, new(2, 6)), (true, new(-32, 0))
	];
	private static readonly (bool Draw, Vector2 Offset)[] TypeZeroExplosionSizeThreeVectors =
	[
		(false, new(28, 12)), (true, new(-56, -24)),
		(false, new(4, -12)), (true, new(48, 48)),
		(false, new(-12, 4)), (true, new(-24, -56)),
		(false, new(12, -4)), (true, new(0, 64)),
		(false, new(-12, -4)), (true, new(24, -56)),
		(false, new(12, 4)), (true, new(-48, 48)),
		(false, new(-4, -12)), (true, new(56, -24)),
		(false, new(4, 12)), (true, new(-64, 0))
	];
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

	private TempestGame game;
	private readonly double gameOverDensityMultiplier;
	private readonly bool atariPresentationEnabled;
	private readonly bool calibrationGuidesEnabled;
	private readonly float? tubeYGainOverride;
	private readonly float? tubeScaleOverride;
	private readonly float? tubeVerticalOffsetOverride;
	private readonly AtariHudCalibrationOptions hudCalibration;
	private readonly ExitZoomFocalCompensation exitZoomFocalCompensation = new();
	private readonly Func<Vector2, float, Vector2> projectFocalOpeningPoint;
	private readonly HashSet<int> legacyFallbackLevels = [];
	public IReadOnlyList<Egg> Eggs => game.Eggs;
	// The active simulation collection is authoritative; unsupported scenarios have no bullets to draw.
	public bool ShouldDrawEnemyBullets => game.EnemyBullets.Count > 0;
	// During normal play Player.Depth == Level.Depth, so this is Level.Position (the near rim).
	// During exit travel Player.Depth shrinks toward 0, moving the projected player down the tube.
	public float PlayerProjectedDepth => game.CurrentLevel.Configuration.Position +
		game.CurrentLevel.Configuration.Depth - game.Player.Depth;
	// Claw height is 20 px at the gameplay rim. It scales by the projection ratio during approach
	// and exit travel so it remains consistent with the tube.
	public float ApproachDepthOffset => game.State == LevelState.Approach
		? (game.CurrentApproach ??
			throw new InvalidOperationException("The approach state has no transition model.")).DepthOffset
		: 0;
	public float ApproachVerticalOffset =>
		InitialApproachVerticalOffset * (ApproachDepthOffset / LevelApproach.InitialDepthOffset);
	public float PlayerClawHeight
	{
		get
		{
			if (UsesAtariPresentationForCurrentLevel)
			{
				float depth = PlayerProjectedDepth;
				if (game.State != LevelState.Approach &&
					game.Player.Depth == Level.Depth)
					return 20;

				return GetStableBaselinePlayerClawHeight(
					depth, ApproachDepthOffset, ApproachVerticalOffset);
			}

			return game.State == LevelState.Approach
				? 20 * (PlayerProjectedDepth + Projection.ZOffset) /
					(PlayerProjectedDepth + ApproachDepthOffset + Projection.ZOffset)
				: game.Player.Depth == Level.Depth
					? 20
					: 20 * ((Level.Position + 5) / 70) /
						((PlayerProjectedDepth + Projection.ZOffset) / Projection.K);
		}
	}

	public VectorRenderer(
		TempestGame game,
		double gameOverDensityMultiplier = 2.0,
		bool atariPresentationEnabled = false,
		float? tubeYGain = null,
		float? tubeScale = null,
		float? tubeVerticalOffset = null,
		bool calibrationGuidesEnabled = false,
		AtariHudCalibrationOptions? hudCalibration = null)
	{
		ArgumentNullException.ThrowIfNull(game);
		if (!double.IsFinite(gameOverDensityMultiplier) || gameOverDensityMultiplier <= 0)
			throw new ArgumentOutOfRangeException(nameof(gameOverDensityMultiplier),
				"The Game Over density multiplier must be a positive finite value.");
		if (tubeYGain is float yGain && (!float.IsFinite(yGain) || yGain <= 0))
			throw new ArgumentOutOfRangeException(nameof(tubeYGain),
				"The development tube Y gain must be positive and finite.");
		if (!atariPresentationEnabled &&
			tubeYGain is float explicitYGain &&
			explicitYGain != AtariPresentationAdapter.DefaultTubeYGain)
			throw new ArgumentException(
				"The development tube Y gain requires Atari presentation.",
				nameof(tubeYGain));
		if (tubeScale is float scale && (!float.IsFinite(scale) || scale <= 0))
			throw new ArgumentOutOfRangeException(nameof(tubeScale),
				"The development tube scale must be positive and finite.");
		if (!atariPresentationEnabled && tubeScale is float explicitScale &&
			explicitScale != TubeScaleOptions.DefaultScale)
			throw new ArgumentException(
				"The development tube scale requires Atari presentation.",
				nameof(tubeScale));
		if (tubeVerticalOffset is float offset && !float.IsFinite(offset))
			throw new ArgumentOutOfRangeException(nameof(tubeVerticalOffset),
				"The development tube vertical offset must be finite.");
		if (!atariPresentationEnabled && tubeVerticalOffset is float explicitOffset &&
			explicitOffset != TubeVerticalOffsetOptions.DefaultOffset)
			throw new ArgumentException(
				"The development tube vertical offset requires Atari presentation.",
				nameof(tubeVerticalOffset));
		if (calibrationGuidesEnabled && !atariPresentationEnabled)
			throw new ArgumentException(
				"The development calibration guides require Atari presentation.",
				nameof(calibrationGuidesEnabled));
		AtariHudCalibrationOptions resolvedHudCalibration =
			hudCalibration ?? new AtariHudCalibrationOptions();
		if (!atariPresentationEnabled && resolvedHudCalibration.HasExplicitOverrides)
			throw new ArgumentException(
				"Atari HUD calibration controls require Atari presentation.",
				nameof(hudCalibration));
		float[] hudCoordinates =
		[
			resolvedHudCalibration.ResolvedScoreRight,
			resolvedHudCalibration.ResolvedScoreBaselineY,
			resolvedHudCalibration.ResolvedHighScoreStartX,
			resolvedHudCalibration.ResolvedHighScoreBaselineY,
			resolvedHudCalibration.ResolvedLevelStartX,
			resolvedHudCalibration.ResolvedLevelBaselineY,
			resolvedHudCalibration.ResolvedReserveLifeStartX,
			resolvedHudCalibration.ResolvedReserveLifeBaselineY,
			resolvedHudCalibration.ResolvedReserveLifeAdvance
		];
		if (hudCoordinates.Any(value => !float.IsFinite(value)))
			throw new ArgumentOutOfRangeException(
				nameof(hudCalibration),
				"Atari HUD coordinates must be finite.");
		float[] hudScales =
		[
			resolvedHudCalibration.ResolvedScoreScale,
			resolvedHudCalibration.ResolvedHighScoreScale,
			resolvedHudCalibration.ResolvedLevelScale,
			resolvedHudCalibration.ResolvedReserveLifeScale
		];
		if (hudScales.Any(scale => !float.IsFinite(scale) || scale <= 0))
			throw new ArgumentOutOfRangeException(
				nameof(hudCalibration),
				"Atari HUD scales must be positive and finite.");
		this.game = game;
		this.gameOverDensityMultiplier = gameOverDensityMultiplier;
		this.atariPresentationEnabled = atariPresentationEnabled;
		this.calibrationGuidesEnabled = calibrationGuidesEnabled;
		tubeYGainOverride = tubeYGain;
		tubeScaleOverride = tubeScale;
		tubeVerticalOffsetOverride = tubeVerticalOffset;
		this.hudCalibration = resolvedHudCalibration;
		projectFocalOpeningPoint = ProjectFocalOpeningPoint;
	}

	private AtariPresentationCalibration CurrentCalibration =>
		atariPresentationEnabled
			? AtariPresentationCalibration.Resolve(
				game.CurrentLevel.Configuration.DisplayedLevel,
				tubeYGainOverride,
				tubeScaleOverride,
				tubeVerticalOffsetOverride)
			: new(
				AtariPresentationAdapter.DefaultTubeYGain,
				TubeScaleOptions.DefaultScale,
				TubeVerticalOffsetOptions.DefaultOffset);

	private float TubeYGain => CurrentCalibration.TubeYGain;
	private float TubeScale => CurrentCalibration.TubeScale;
	private float TubeVerticalOffset => CurrentCalibration.VerticalOffset;

	public void SetGame(TempestGame game)
	{
		this.game = game ?? throw new ArgumentNullException(nameof(game));
		exitZoomFocalCompensation.Reset();
	}

	private Vector2 ProjectApproachGeometry(Vector2 point, float depth) =>
		ProjectWorldPoint(point.X, point.Y, depth);

	// Tube vertices and entity anchors share this projector; Level 7 uses its production definition.
	private Vector2 ProjectWorldPoint(float x, float y, float depth)
	{
		Vector2 mappedPoint = ProjectWorldPointWithoutExitCompensation(x, y, depth);
		if (!atariPresentationEnabled)
		{
			exitZoomFocalCompensation.Reset();
			return mappedPoint;
		}

		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		Vector2 compensation = exitZoomFocalCompensation.GetTranslation(
			game.State,
			game.CurrentLevel.Definition,
			configuration,
			Projection.K,
			Projection.ZOffset,
			Projection.PortraitPerspective,
			projectFocalOpeningPoint);
		mappedPoint += compensation;
		if (TubeVerticalOffset != 0)
		{
			// DEVELOPMENT ONLY: translate the finished presentation, after ExitZoom compensation.
			mappedPoint.Y += TubeVerticalOffset;
		}
		return mappedPoint;
	}

	private Vector2 ProjectFocalOpeningPoint(Vector2 point, float depth) =>
		ProjectWorldPointWithoutExitCompensation(point.X, point.Y, depth);

	private Vector2 ProjectWorldPointWithoutExitCompensation(float x, float y, float depth)
	{
		if (!atariPresentationEnabled)
			return ProjectWorldPointLegacy(x, y, depth);
		if (IsLevel6GameOverState)
			return ProjectDiagnosticsOnLegacyPoint(x, y, depth);

		int displayedLevel = game.CurrentLevel.Configuration.DisplayedLevel;
		if (displayedLevel == 5)
			return ProjectAtariPresentation(x, y, depth,
				ApproachDepthOffset, ApproachVerticalOffset).Position;
		if (displayedLevel == 6)
		{
			// Keep Level 6 in its C# model/depth domain; this is only a portrait render transform.
			return ProjectAtariPresentation(x, y, depth,
				ApproachDepthOffset, ApproachVerticalOffset).Position;
		}
		if (AtariPresentationAdapter.HasModelFrameMapping(displayedLevel))
			return ProjectAtariPresentation(x, y, depth,
				ApproachDepthOffset, ApproachVerticalOffset).Position;

		if (displayedLevel is < 2 or > 6)
			throw new InvalidOperationException(
				$"The Atari presentation candidate has no model-frame mapping for Level {displayedLevel}.");
		// Unmapped configurations in Levels 2-6 retain legacy presentation; Level 6's visual mapping
		// is explicitly dispatched above without claiming an Atari model-frame correspondence.
		if (legacyFallbackLevels.Add(displayedLevel))
		{
			Console.Error.WriteLine(
				$"DEVELOPMENT ONLY — Atari presentation uses the unchanged legacy projection for Level {displayedLevel}; its model-frame mapping is not established.");
		}
		return ProjectWorldPointLegacy(x, y, depth);
	}

	private Vector2 ProjectDiagnosticsOnLegacyPoint(float x, float y, float depth)
	{
		Vector2 position = ProjectWorldPointLegacy(x, y * TubeYGain, depth);
		if (TubeScale == TubeScaleOptions.DefaultScale)
			return position;

		Vector2 anchor = ProjectWorldPointLegacy(0, 0, depth);
		Vector2 scaled = anchor + (position - anchor) * TubeScale;
		if (!float.IsFinite(scaled.X) || !float.IsFinite(scaled.Y))
			throw new InvalidOperationException(
				"The development tube scale produced a non-finite presentation.");
		return scaled;
	}

	private Vector2 ProjectWorldPointLegacy(float x, float y, float depth)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		float approachOffset = ApproachDepthOffset;
		return Projection.Project(x, y, depth + approachOffset,
			game.CurrentLevel.Definition.CenterY,
			game.CurrentLevel.Definition.YOffset + ApproachVerticalOffset,
			configuration.Position + approachOffset,
			configuration.Position + configuration.Depth + approachOffset);
	}

	// Camera overrides let the stable-rim baseline use this same adapter without mutating live zoom state.
	private AtariPresentationProjection ProjectAtariPresentation(
		float x,
		float y,
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset,
		float? projectionK = null,
		float? projectionZOffset = null)
	{
		AtariPresentationProjection projection = ProjectAtariPresentationCore(
			x, y, depth, approachDepthOffset, approachVerticalOffset,
			projectionK, projectionZOffset);
		EnsureFinitePresentationProjection(projection);
		if (TubeScale == TubeScaleOptions.DefaultScale)
			return projection;

		// Keep each depth's existing zero-coordinate anchor; scaling must not recenter the tube.
		AtariPresentationProjection anchor = ProjectAtariPresentationCore(
			0, 0, depth, approachDepthOffset, approachVerticalOffset,
			projectionK, projectionZOffset);
		EnsureFinitePresentationProjection(anchor);
		Vector2 position = anchor.Position + (projection.Position - anchor.Position) * TubeScale;
		float localScale = projection.LocalScale * TubeScale;
		float localScaleX = projection.LocalScaleX * TubeScale;
		float localScaleY = projection.LocalScaleY * TubeScale;
		if (!float.IsFinite(position.X) || !float.IsFinite(position.Y) ||
			!float.IsFinite(localScale) || !float.IsFinite(localScaleX) ||
			!float.IsFinite(localScaleY))
		{
			throw new InvalidOperationException(
				"The development tube scale produced a non-finite presentation.");
		}

		return new AtariPresentationProjection(position, localScale)
		{
			LocalScaleX = localScaleX,
			LocalScaleY = localScaleY
		};
	}

	private static void EnsureFinitePresentationProjection(
		AtariPresentationProjection projection)
	{
		if (!float.IsFinite(projection.Position.X) ||
			!float.IsFinite(projection.Position.Y) ||
			!float.IsFinite(projection.LocalScale) ||
			!float.IsFinite(projection.LocalScaleX) ||
			!float.IsFinite(projection.LocalScaleY))
		{
			throw new InvalidOperationException(
				"The development tube diagnostics produced a non-finite presentation.");
		}
	}

	private AtariPresentationProjection ProjectAtariPresentationCore(
		float x,
		float y,
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset,
		float? projectionK,
		float? projectionZOffset)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		int displayedLevel = configuration.DisplayedLevel;
		float k = projectionK ?? Projection.K;
		float zOffset = projectionZOffset ?? Projection.ZOffset;
		if (displayedLevel == 5)
		{
			// These dedicated C# projection paths bypass the shared adapter's Y-gain handling.
			AtariPresentationProjection projection = AtariPresentationAdapter.ProjectLevel5(
				x,
				y * TubeYGain,
				depth,
				game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset,
				configuration.Position,
				configuration.Depth,
				approachDepthOffset,
				approachVerticalOffset,
				k,
				zOffset);
			return projection with
			{
				LocalScaleY = projection.LocalScaleY * TubeYGain
			};
		}
		if (displayedLevel == 6)
		{
			// Keep Level 6's presentation mapping while transforming its source Y axis uniformly.
			AtariPresentationProjection projection = AtariPresentationAdapter.ProjectLevel6(
				x,
				y * TubeYGain,
				depth,
				game.CurrentLevel.Definition.CenterY,
				game.CurrentLevel.Definition.YOffset,
				configuration.Position,
				configuration.Depth,
				approachDepthOffset,
				approachVerticalOffset,
				k,
				zOffset);
			return projection with
			{
				LocalScaleY = projection.LocalScaleY * TubeYGain
			};
		}

		RomPresentationMetadata metadata =
			RomPresentationMetadataCatalog.ForDisplayedLevel(displayedLevel);
		AtariPresentationAdapter.ValidateTopology(metadata, game.CurrentLevel.Definition.Continuous);
		return AtariPresentationAdapter.Project(
			displayedLevel,
			x,
			y,
			depth,
			game.CurrentLevel.Definition.CenterY,
			game.CurrentLevel.Definition.YOffset,
			configuration.Position,
			configuration.Depth,
			approachDepthOffset,
			approachVerticalOffset,
			k,
			zOffset,
			metadata,
			tubeYGain: TubeYGain);
	}

	private bool UsesAtariPresentationForCurrentLevel =>
		atariPresentationEnabled &&
		!IsLevel6GameOverState &&
		(game.CurrentLevel.Configuration.DisplayedLevel == 5 ||
		 game.CurrentLevel.Configuration.DisplayedLevel == 6 ||
		 AtariPresentationAdapter.HasModelFrameMapping(
			 game.CurrentLevel.Configuration.DisplayedLevel));

	private bool IsLevel6GameOverState =>
		game.CurrentLevel.Configuration.DisplayedLevel == 6 &&
		game.State is LevelState.GameOver or LevelState.GameOverPresentation or
			LevelState.GameOverAnimation;

	// Screen-oriented artwork uses the scalar scale; tube-oriented glyphs use both Jacobian axes.
	private float GetAtariLocalScale(
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset,
		float? projectionK = null,
		float? projectionZOffset = null) =>
		ProjectAtariPresentation(
			0, 0, depth, approachDepthOffset, approachVerticalOffset,
			projectionK, projectionZOffset).LocalScale;

	// PROJECT TRANSLATION: travel artwork follows the active adapter Jacobian relative to the
	// stable gameplay-rim projection. This reverses the former live-baseline ratio; Playing stays
	// fixed at 20, and evaluating presentation scale never changes gameplay state.
	private float GetStableBaselinePlayerClawHeight(
		float depth,
		float approachDepthOffset,
		float approachVerticalOffset)
	{
		float currentScale = GetAtariLocalScale(
			depth, approachDepthOffset, approachVerticalOffset);
		float baselineScale = GetAtariLocalScale(
			game.CurrentLevel.Configuration.Position,
			0,
			0,
			Projection.PortraitPerspectiveBaselineK,
			Projection.PortraitPerspectiveBaselineZOffset);
		if (!float.IsFinite(currentScale) || currentScale <= 0 ||
			!float.IsFinite(baselineScale) || baselineScale <= 0)
			throw new InvalidOperationException(
				"The Blaster projection scales must be positive and finite.");

		float height = 20 * (currentScale / baselineScale);
		if (!float.IsFinite(height) || height <= 0)
			throw new InvalidOperationException(
				"The projected Blaster height must be positive and finite.");
		return height;
	}

	private Vector2 GetAtariLocalScaleAxes(
		float depth, float approachDepthOffset, float approachVerticalOffset)
	{
		AtariPresentationProjection projection = ProjectAtariPresentation(
			0, 0, depth, approachDepthOffset, approachVerticalOffset);
		return new Vector2(projection.LocalScaleX, projection.LocalScaleY);
	}

	private float TubeLocalPresentationScale => TubeScale;

	private Vector2 ApplyAtariLocalPresentationToOffset(Vector2 offset) =>
		!atariPresentationEnabled
			? offset
			: game.CurrentLevel.Configuration.DisplayedLevel switch
			{
				2 => ScaleLocalOffset(offset, new Vector2(
					TubeScale,
					TubeScale * TubeYGain *
						AtariPresentationAdapter.Level2VerticalAspectFactor)),
				6 => ScaleLocalOffset(offset, new Vector2(
					TubeScale * AtariPresentationAdapter.Level6HorizontalPresentationScale,
					TubeScale * TubeYGain *
						AtariPresentationAdapter.Level6VerticalPresentationScale)),
				5 => ScaleLocalOffset(offset, new Vector2(
					TubeScale * AtariPresentationAdapter.Level5HorizontalPresentationScale,
					TubeScale * TubeYGain *
						AtariPresentationAdapter.Level5VerticalPresentationScale)),
				_ => ScaleLocalOffset(offset,
					new Vector2(TubeScale, TubeScale * TubeYGain))
			};

	private static Vector2 ScaleLocalOffset(Vector2 offset, Vector2 scale) =>
		new(offset.X * scale.X, offset.Y * scale.Y);

	public void Draw(
		PlaybackOverlayStatus? playbackStatus = null,
		Action? beforeEndDrawing = null,
		PresentationMode presentationMode = PresentationMode.ExperimentalPortrait)
	{
		if (atariPresentationEnabled && presentationMode != PresentationMode.ExperimentalPortrait)
			throw new InvalidOperationException("The Atari presentation candidate is portrait-only.");
		bool experimentalPortrait = presentationMode == PresentationMode.ExperimentalPortrait;
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.Black);
		if (experimentalPortrait)
			Raylib.BeginScissorMode(
				DisplayConfiguration.PortraitViewportLeft,
				0,
				DisplayConfiguration.PortraitViewportWidth,
				DisplayConfiguration.PortraitViewportHeight);
		Raylib.BeginMode2D(DisplayConfiguration.GetPlayfieldCamera(presentationMode));

		if (game.State == LevelState.GameOverAnimation)
		{
			DrawGameOverAnimation((start, end, color) => Raylib.DrawLineV(start, end, color));
			if (!experimentalPortrait && playbackStatus is PlaybackOverlayStatus status)
				DrawPlaybackPausedStatus(status.Boundary, status.DurationTicks,
					(start, end) => Raylib.DrawLineV(start, end, Color.White));
			Raylib.EndMode2D();
			if (experimentalPortrait)
			{
				Raylib.EndScissorMode();
				if (playbackStatus is PlaybackOverlayStatus portraitStatus)
					DrawPortraitPlaybackStatus(portraitStatus);
				DrawDevelopmentPortraitCalibrationGuides(
					(start, end, color) => Raylib.DrawLineV(start, end, color));
				DrawPortraitViewportBorder();
			}
			beforeEndDrawing?.Invoke();
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
			DrawEnemyBullets(
				(start, end, color) => Raylib.DrawLineV(start, end, color),
				(point, color) => Raylib.DrawPixelV(point, color));
		}
		else if (game.ExitShotsVisible)
		{
			DrawPlayerShots(game.CurrentLevel.Definition.Points);
		}

		if (!approaching)
			DrawEnemyDestructionEffects((start, end, color) => Raylib.DrawLineV(start, end, color));

		if (!approaching)
		{
			if (UsesAtariPresentationForCurrentLevel)
				DrawAtariFuseballScorePopups((start, end, color) =>
					Raylib.DrawLineV(start, end, color));
			else
				DrawFuseballScorePopups((text, position, scale) =>
					Raylib.DrawTextEx(Raylib.GetFontDefault(), text, position,
						scale * 7, scale, Color.White));
		}

		if (!experimentalPortrait)
			DrawAvoidSpikesWarning((start, end, color) => Raylib.DrawLineV(start, end, color));

		if (!game.GameOver &&
			game.State is not (LevelState.GameOverPresentation or LevelState.GameOverAnimation))
		{
			if (game.DeathTicks > 0)
				DrawPlayerExplosion(game.CurrentLevel.Definition.Points,
					(start, end, color) => Raylib.DrawLineV(start, end, color));
			else
				DrawPlayer(game.CurrentLevel.Definition.Points);
		}

		if (!experimentalPortrait)
			DrawGameOverMessage((start, end, color) => Raylib.DrawLineV(start, end, color));
		DrawGameOverVectorRecords((start, end, color) => Raylib.DrawLineV(start, end, color));
		if (!experimentalPortrait)
		{
			DrawRechargeMessage((text, y, size, color) =>
				DrawCenteredText(text, y, size, color, presentationMode));
			DrawHud(presentationMode);
		}
		if (!experimentalPortrait && playbackStatus is PlaybackOverlayStatus pausedStatus)
			DrawPlaybackPausedStatus(pausedStatus.Boundary, pausedStatus.DurationTicks,
				(start, end) => Raylib.DrawLineV(start, end, Color.White));
		Raylib.EndMode2D();
		if (experimentalPortrait)
		{
			Raylib.EndScissorMode();
			DrawPortraitOverlays(playbackStatus);
			DrawDevelopmentPortraitCalibrationGuides(
				(start, end, color) => Raylib.DrawLineV(start, end, color));
			DrawPortraitViewportBorder();
		}
		// Raylib reads the completed back buffer here; EndDrawing swaps it for presentation.
		beforeEndDrawing?.Invoke();
		Raylib.EndDrawing();
	}

	public void DrawDevelopmentPortraitCalibrationGuides(
		Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (!calibrationGuidesEnabled)
			return;
		float left = DisplayConfiguration.PortraitViewportLeft;
		float right = left + DisplayConfiguration.PortraitViewportWidth - 1;
		drawLine(new Vector2(left, DevelopmentPortraitGuideTopY),
			new Vector2(right, DevelopmentPortraitGuideTopY), Color.Yellow);
		drawLine(new Vector2(left, DevelopmentPortraitGuideBottomY),
			new Vector2(right, DevelopmentPortraitGuideBottomY), Color.Yellow);
	}

	private void DrawPortraitOverlays(PlaybackOverlayStatus? playbackStatus)
	{
		Raylib.BeginScissorMode(
			DisplayConfiguration.PortraitViewportLeft,
			0,
			DisplayConfiguration.PortraitViewportWidth,
			DisplayConfiguration.PortraitViewportHeight);
		Raylib.BeginMode2D(DisplayConfiguration.ExperimentalPortraitHudCamera);
		DrawAvoidSpikesWarning((start, end, color) => Raylib.DrawLineV(start, end, color),
			new Vector2(DisplayConfiguration.PortraitLogicalWidth / 2f,
				DisplayConfiguration.PortraitLogicalHeight / 2f));
		DrawGameOverMessage((start, end, color) => Raylib.DrawLineV(start, end, color),
			new Vector2(DisplayConfiguration.PortraitLogicalWidth / 2f,
				DisplayConfiguration.PortraitLogicalHeight / 2f));
		DrawRechargeMessage((text, y, size, color) =>
			DrawCenteredText(text, y, size, color, PresentationMode.ExperimentalPortrait));
		DrawHud(PresentationMode.ExperimentalPortrait);
		if (playbackStatus is PlaybackOverlayStatus status)
			DrawPlaybackPausedStatus(status.Boundary, status.DurationTicks,
				(start, end) => Raylib.DrawLineV(start, end, Color.White),
				new Vector2(DisplayConfiguration.PortraitLogicalWidth / 2f,
					DisplayConfiguration.PortraitLogicalHeight - 40));
		Raylib.EndMode2D();
		Raylib.EndScissorMode();
		DrawPortraitDevelopmentDiagnostics();
	}

	private static void DrawPortraitViewportBorder() =>
		Raylib.DrawRectangleLinesEx(
			DisplayConfiguration.PortraitViewport,
			DisplayConfiguration.PortraitViewportBorderThickness,
			PortraitViewportBorderColor);

	private static void DrawPortraitPlaybackStatus(PlaybackOverlayStatus status)
	{
		Raylib.BeginScissorMode(
			DisplayConfiguration.PortraitViewportLeft,
			0,
			DisplayConfiguration.PortraitViewportWidth,
			DisplayConfiguration.PortraitViewportHeight);
		Raylib.BeginMode2D(DisplayConfiguration.ExperimentalPortraitHudCamera);
		DrawPlaybackPausedStatus(status.Boundary, status.DurationTicks,
			(start, end) => Raylib.DrawLineV(start, end, Color.White),
			new Vector2(DisplayConfiguration.PortraitLogicalWidth / 2f,
				DisplayConfiguration.PortraitLogicalHeight - 40));
		Raylib.EndMode2D();
		Raylib.EndScissorMode();
	}

	public void DrawPlaybackPausedStatus(
		int boundary, int durationTicks, Action<Vector2, Vector2> drawLine)
		=> DrawPlaybackPausedStatus(boundary, durationTicks, drawLine, new Vector2(400, 570));

	private static void DrawPlaybackPausedStatus(
		int boundary,
		int durationTicks,
		Action<Vector2, Vector2> drawLine,
		Vector2 center)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (durationTicks < 0)
			throw new ArgumentOutOfRangeException(nameof(durationTicks),
				"The playback duration cannot be negative.");
		if (boundary < 0 || boundary > durationTicks)
			throw new ArgumentOutOfRangeException(nameof(boundary),
				"The playback boundary must be within the recording duration.");

		string text = FormattableString.Invariant($"PAUSED  {boundary} / {durationTicks}");
		VectorFont.DrawCenteredText(text, center, 2,
			(start, end) => drawLine(start, end));
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
		return ProjectWorldPoint(position.X, position.Y, ProjectEnemyDepth(fuseball.Height));
	}

	public void DrawFuseballs(Action<Vector2, Vector2, Color> drawLine)
	{
		foreach (Fuseball fuseball in game.Fuseballs)
		{
			Vector2 center = ProjectFuseballCenter(fuseball);
			float depth = ProjectEnemyDepth(fuseball.Height);
			Vector2 scale = UsesAtariPresentationForCurrentLevel
				? (8 * GetAtariLocalScaleAxes(
					depth, ApproachDepthOffset, ApproachVerticalOffset) +
					new Vector2(.25f * TubeLocalPresentationScale)) / 6
				: new Vector2((8 / ((depth + Projection.ZOffset) / Projection.K) + .25f) / 6);
			for (int arm = 0; arm < 5; arm++)
			{
				int row = game.FuseballAnimationFrame * 5 + arm;
				for (int point = 0; point < 5; point++)
				{
					Vector2 start = new(FuseballArmPoints[row, point * 2], FuseballArmPoints[row, point * 2 + 1]);
					Vector2 end = new(FuseballArmPoints[row, point * 2 + 2], FuseballArmPoints[row, point * 2 + 3]);
					drawLine(center + ScaleLocalOffset(start, scale),
						center + ScaleLocalOffset(end, scale), FuseballArmColors[arm]);
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
				p1 = ProjectWorldPoint(end.X, end.Y, projectedDepth);
				p2 = ProjectWorldPoint(start.X, start.Y, projectedDepth);
			}
			else
			{
				int next = NextEdge(pulsar.EdgeIndex, points.Count);
				p1 = ProjectWorldPoint(points[next].X, points[next].Y, projectedDepth);
				p2 = ProjectWorldPoint(points[pulsar.EdgeIndex].X, points[pulsar.EdgeIndex].Y,
					projectedDepth);
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
				Vector2.Lerp(p2, p1, .2f) - ApplyAtariLocalPresentationToOffset(normal * offset),
				Vector2.Lerp(p2, p1, .35f) + ApplyAtariLocalPresentationToOffset(normal * offset),
				Vector2.Lerp(p2, p1, .5f) - ApplyAtariLocalPresentationToOffset(normal * offset),
				Vector2.Lerp(p2, p1, .65f) + ApplyAtariLocalPresentationToOffset(normal * offset),
				Vector2.Lerp(p2, p1, .8f) - ApplyAtariLocalPresentationToOffset(normal * offset),
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
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2 nearStart;
			Vector2 farStart;
			Vector2 nearEnd;
			Vector2 farEnd;
			nearStart = ProjectWorldPoint(points[pulsar.EdgeIndex].X,
				points[pulsar.EdgeIndex].Y, configuration.Position);
			farStart = ProjectWorldPoint(points[pulsar.EdgeIndex].X,
				points[pulsar.EdgeIndex].Y, configuration.Position + configuration.Depth);
			nearEnd = ProjectWorldPoint(points[next].X, points[next].Y,
				configuration.Position);
			farEnd = ProjectWorldPoint(points[next].X, points[next].Y,
				configuration.Position + configuration.Depth);

			drawLine(nearStart, farStart, color);
			drawLine(nearEnd, farEnd, color);
		}
	}

	public void DrawFuseballScorePopups(Action<string, Vector2, float> drawText)
	{
		foreach (FuseballScorePopup popup in game.FuseballScorePopups)
		{
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2 center = ProjectLane(popup.EdgeIndex,
				configuration.Position + configuration.Depth - popup.Height);
			drawText(popup.Points.ToString(), center - new Vector2(popup.Scale * 1.5f, 0), popup.Scale);
		}
	}

	public void DrawAtariFuseballScorePopups(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		foreach (FuseballScorePopup popup in game.FuseballScorePopups)
		{
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2 center = ProjectLane(popup.EdgeIndex,
				configuration.Position + configuration.Depth - popup.Height);
			foreach (AtariHudLine line in AtariHudVector.RenderCenteredText(
				popup.Points.ToString(), center, popup.AtariGlyphScale, Color.White))
				drawLine(line.Start, line.End, line.Color);
		}
	}

	public void DrawEnemyDestructionEffects(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		foreach (EnemyDestructionEffect effect in game.EnemyDestructionEffects)
		{
			if (!effect.IsActive)
				continue;

			float projectedDepth = ProjectEnemyDepth(effect.Height);
			Vector2 center = ProjectLane(effect.EdgeIndex, projectedDepth);
			float scale = UsesAtariPresentationForCurrentLevel
				? GetAtariLocalScale(projectedDepth, ApproachDepthOffset, ApproachVerticalOffset)
				: 1 / ((projectedDepth + Projection.ZOffset) / Projection.K);
			Vector2 pen = Vector2.Zero;
			foreach ((bool draw, Vector2 offset) in GetTypeZeroExplosionVectors(effect.Age))
			{
				Vector2 next = pen + offset;
				if (draw)
				{
					// ROM vstat c=0 has no asserted hardware RGB match; white is the project's
					// narrow, legible translation for each single-color picture.
					drawLine(center + pen * scale, center + next * scale, Color.White);
				}
				pen = next;
			}
		}
	}

	private static IReadOnlyList<(bool Draw, Vector2 Offset)> GetTypeZeroExplosionVectors(int age) =>
		age switch
		{
			0 or 3 => TypeZeroExplosionSizeOneVectors,
			6 => TypeZeroExplosionSizeTwoVectors,
			9 => TypeZeroExplosionSizeThreeVectors,
			_ => throw new InvalidOperationException(
				$"Unsupported active type-0 effect age {age}.")
		};

	public void DrawAvoidSpikesWarning(
		Action<Vector2, Vector2, Color> drawLine,
		Vector2? center = null)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (!game.ShouldShowAvoidSpikesWarning)
			return;

		VectorFont.DrawCenteredText(
			"AVOID SPIKES",
			center ?? AvoidSpikesWarningCenter,
			AvoidSpikesWarningScale,
			(start, end) => drawLine(start, end, AvoidSpikesWarningColor));
	}

	public void DrawGameOverMessage(
		Action<Vector2, Vector2, Color> drawLine,
		Vector2? center = null)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		if (!game.GameOverMessageVisible)
			return;

		VectorFont.DrawGameOver(
			center ?? new Vector2(DisplayConfiguration.LogicalWidth / 2f,
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
				float depth = game.CurrentLevel.Configuration.Position + record.Z;
				// Game Over vector records use a different raw coordinate frame and stay legacy.
				Vector2 center = ProjectWorldPointLegacy(
					(record.X - 0x80) * 1.5f,
					(record.Y - 0x80) * 1.5f,
					depth);

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
			Vector2 scale = UsesAtariPresentationForCurrentLevel
				? 26 * GetAtariLocalScaleAxes(
					depth, ApproachDepthOffset, ApproachVerticalOffset) +
					Vector2.One * TubeLocalPresentationScale
				: new Vector2(26 / ((depth + Projection.ZOffset) / Projection.K) + 1);
			Vector2 third = scale / 3;
			Vector2 top = center + new Vector2(0, -scale.Y);
			Vector2 bottom = center + new Vector2(0, scale.Y);
			Vector2 left = center + new Vector2(-scale.X, 0);
			Vector2 right = center + new Vector2(scale.X, 0);
			Vector2 topThird = center + new Vector2(0, -third.Y);
			Vector2 bottomThird = center + new Vector2(0, third.Y);
			Vector2 leftThird = center + new Vector2(-third.X, 0);
			Vector2 rightThird = center + new Vector2(third.X, 0);

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
			drawLine(rightThird, center + new Vector2(scale.X, 0));
			drawLine(leftThird, center + new Vector2(-scale.X, 0));
		}
	}

	public Vector2 ProjectEnemyBullet(EnemyBullet bullet)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		Vector2 center = ProjectLane(bullet.EdgeIndex,
			configuration.Position + configuration.Depth - bullet.Height);
		return center;
	}

	public void DrawEnemyBullets(Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		DrawEnemyBullets(drawLine, (_, _) => { });
	}

	public void DrawEnemyBullets(Action<Vector2, Vector2> drawLine)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		DrawEnemyBullets(
			(start, end, _) => drawLine(start, end),
			(_, _) => { });
	}

	public void DrawEnemyBullets(
		Action<Vector2, Vector2, Color> drawLine,
		Action<Vector2, Color> drawPoint)
	{
		ArgumentNullException.ThrowIfNull(drawLine);
		ArgumentNullException.ThrowIfNull(drawPoint);
		if (!ShouldDrawEnemyBullets)
			return;

		EnemyShotVectorFrame frame =
			EnemyShotVectorProgram.GetFrame(game.EnemyShotAnimationFrame);
		foreach (EnemyBullet bullet in game.EnemyBullets)
		{
			Vector2 center = ProjectEnemyBullet(bullet);
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			float projectedDepth = configuration.Position + configuration.Depth - bullet.Height;
			// PROJECT TRANSLATION: normalize logical ROM units to the prior compact shot scale.
			float coordinateScale = UsesAtariPresentationForCurrentLevel
				? 6 * GetAtariLocalScale(
					projectedDepth, ApproachDepthOffset, ApproachVerticalOffset) /
					EnemyShotVectorProgram.MaximumLogicalExtent
				: 6 / ((projectedDepth + Projection.ZOffset) / Projection.K) /
					EnemyShotVectorProgram.MaximumLogicalExtent;
			foreach (EnemyShotVectorSegment segment in frame.Segments)
			{
				// ROM coordinates are Y-up; the projected screen coordinate system is Y-down.
				Vector2 start = center + new Vector2(segment.Start.X, -segment.Start.Y) * coordinateScale;
				Vector2 end = center + new Vector2(segment.End.X, -segment.End.Y) * coordinateScale;
				drawLine(start, end, Color.White);
			}

			// PROJECT TRANSLATION: a zero-displacement ROM draw is represented by one visible pixel.
			foreach (Vector2 point in frame.Points)
				drawPoint(center + new Vector2(point.X, -point.Y) * coordinateScale, Color.White);
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
			LevelConfiguration configuration = game.CurrentLevel.Configuration;
			Vector2 start = ProjectLane(spike.EdgeIndex,
				configuration.Position + configuration.Depth);
			Vector2 tip = ProjectLane(spike.EdgeIndex,
				configuration.Position + configuration.Depth - spike.Height);
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
			Vector2 scale = UsesAtariPresentationForCurrentLevel
				? 22 * GetAtariLocalScaleAxes(
					depth, ApproachDepthOffset, ApproachVerticalOffset) +
					new Vector2(2 * TubeLocalPresentationScale)
				: new Vector2(22 / ((depth + Projection.ZOffset) / Projection.K) + 2);
			for (int segment = 0; segment >= -13; segment--)
			{
				float angle1 = (spiker.Rotation + segment * 60) * MathF.PI / 180;
				float angle2 = (spiker.Rotation + (segment + 1) * 60) * MathF.PI / 180;
				Vector2 first = new(MathF.Sin(angle1), MathF.Cos(angle1));
				Vector2 second = new(MathF.Sin(angle2), MathF.Cos(angle2));
				drawLine(center + new Vector2(
					first.X * segment * scale.X / 16,
					first.Y * segment * scale.Y / 16),
					center + new Vector2(
						second.X * (segment + 1) * scale.X / 16,
						second.Y * (segment + 1) * scale.Y / 16));
			}
		}
	}

	// Egg heights are negative while roaming (0 means ready to hatch at the web), so the ×20
	// amplification pushes the projected depth beyond Level.Depth: Eggs visibly bob behind and
	// deeper than the far web until they approach and hatch.
	public Vector2 ProjectEgg(Egg egg)
	{
		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		float projectedDepth = configuration.Position + configuration.Depth - (egg.Height * 20);
		// DEVELOPMENT ONLY: retain t>1 through linear presentation extrapolation without changing Egg state.
		return ProjectLane(egg.EdgeIndex, projectedDepth);
	}

	public void DrawEggs(Action<Vector2, Vector2> drawPoint)
	{
		foreach (Egg egg in Eggs)
		{
			Vector2 size = new(EggPointSize, EggPointSize);
			Vector2 anchor = ProjectEgg(egg);
			Vector2 topLeft = anchor - Vector2.One;
			drawPoint(topLeft, size);
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

		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		for (int i = 0; i < points.Count; i++)
		{
			nearRim[i] = ProjectApproachGeometry(points[i], configuration.Position);
			farRim[i] = ProjectApproachGeometry(points[i],
				configuration.Position + configuration.Depth);
		}

		for (int i = 0; i < points.Count; i++)
		{
			bool playerLaneBoundary = highlightPlayerLane &&
				(i == playerLane || i == nextPlayerLaneBoundary);
			drawLine(nearRim[i], farRim[i], playerLaneBoundary ? playerLaneColor : railColor);
		}

		for (int i = 0; i < game.CurrentLevel.PlayableLaneCount; i++)
		{
			int next = game.CurrentLevel.NextEdge(i);
			// Arcade observation: only the pulsing lane's near-rim connection disappears.
			// Incitti also hides rims outside the pulse; retain normal geometry otherwise.
			if (!IsPulsingLane(i))
				drawLine(nearRim[i], nearRim[next], tubeColor);
			drawLine(farRim[i], farRim[next], tubeColor);
		}
	}

	// The ROM picture is static; only its external lane/depth projection and scale vary per shot.
	private void DrawPlayerShots(IReadOnlyList<Vector2> points)
	{
		DrawPlayerShots(points, (position, color) => Raylib.DrawPixelV(position, color));
	}

	public void DrawPlayerShots(Action<Vector2, Color> drawPoint)
	{
		ArgumentNullException.ThrowIfNull(drawPoint);
		DrawPlayerShots(game.CurrentLevel.Definition.Points, drawPoint);
	}

	private void DrawPlayerShots(
		IReadOnlyList<Vector2> points,
		Action<Vector2, Color> drawPoint)
	{
		foreach (Shot shot in game.Shots)
		{
			Vector2 center = ProjectShot(shot, points);
			float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
				game.CurrentLevel.Configuration.Position;
			float scale = UsesAtariPresentationForCurrentLevel
				? PlayerShotBaseSize * GetAtariLocalScale(
					projectedDepth, ApproachDepthOffset, ApproachVerticalOffset)
				: PlayerShotBaseSize /
					((projectedDepth + Projection.ZOffset) / Projection.K);
			float coordinateScale = scale / PlayerShotVectorProgram.MaximumPointRadius;
			foreach (PlayerShotVectorPointGroup group in PlayerShotVectorProgram.Groups)
			{
				foreach (Vector2 point in group.Points)
				{
					// ROM vector Y is up; screen-space Y is down. Keep the point picture screen-oriented.
					Vector2 screenOffset = new Vector2(point.X, -point.Y) * coordinateScale;
					Vector2 submittedPoint = center + screenOffset;
					drawPoint(submittedPoint, group.ProjectColor);
				}
			}
		}
	}

	public Vector2 ProjectShot(Shot shot)
	{
		ArgumentNullException.ThrowIfNull(shot);
		return ProjectShot(shot, game.CurrentLevel.Definition.Points);
	}

	private Vector2 ProjectShot(Shot shot, IReadOnlyList<Vector2> points)
	{
		float projectedDepth = game.CurrentLevel.Configuration.Depth - shot.Height +
			game.CurrentLevel.Configuration.Position;
		int next = NextEdge(shot.EdgeIndex, points.Count);
		Vector2 midpoint = (points[shot.EdgeIndex] + points[next]) / 2;
		return ProjectWorldPoint(midpoint.X, midpoint.Y, projectedDepth);
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
				ProjectWorldPoint(start.X, start.Y, projectedDepth),
				ProjectWorldPoint(end.X, end.Y, projectedDepth));
		}

		// During a legacy pivot the glyph stays on the source lane until completion.
		int edge = flipper.State == FlipperState.Flipping
			? flipper.SourceEdge
			: flipper.EdgeIndex;
		int next = NextEdge(edge, points.Count);
		Vector2 p1 = ProjectWorldPoint(points[edge].X, points[edge].Y, projectedDepth);
		Vector2 p2 = ProjectWorldPoint(points[next].X, points[next].Y, projectedDepth);

		return (p1, p2);
	}

	// Flipper body: six red lines forming the bow-tie glyph between the two projected lane
	// endpoints. Fractions along the edge and the normal-based height are Incitti-derived.
	private void DrawFlipperShape(
		Vector2 p1,
		Vector2 p2,
		float projectedDepth,
		Action<Vector2, Vector2>? drawLine = null)
	{
		float dx = p2.X - p1.X;
		float dy = p2.Y - p1.Y;
		float length = MathF.Sqrt(dx * dx + dy * dy);
		if (length == 0) return;

		float xn = -dy / length;
		float yn = dx / length;
		Vector2 height = UsesAtariPresentationForCurrentLevel
			? 20 * GetAtariLocalScaleAxes(
				projectedDepth, ApproachDepthOffset, ApproachVerticalOffset)
			: new Vector2(20 / ((projectedDepth + Projection.ZOffset) / Projection.K));
		Vector2 normalHeight = new(xn * height.X, yn * height.Y);

		Vector2 point2 = new(p2.X - dx * .3f - normalHeight.X / 2,
							 p2.Y - dy * .3f - normalHeight.Y / 2);
		Vector2 point3 = new(p2.X - dx * .1f - normalHeight.X,
							 p2.Y - dy * .1f - normalHeight.Y);
		Vector2 point5 = new(p2.X - dx * .9f - normalHeight.X,
							 p2.Y - dy * .9f - normalHeight.Y);
		Vector2 point6 = new(p2.X - dx * .7f - normalHeight.X / 2,
							 p2.Y - dy * .7f - normalHeight.Y / 2);

		if (drawLine is null)
		{
			Raylib.DrawLineV(p1, point3, Color.Red);
			Raylib.DrawLineV(point5, p2, Color.Red);
			Raylib.DrawLineV(point6, point5, Color.Red);
			Raylib.DrawLineV(point2, point3, Color.Red);
			Raylib.DrawLineV(point6, p1, Color.Red);
			Raylib.DrawLineV(point2, p2, Color.Red);
		}
		else
		{
			drawLine(p1, point3);
			drawLine(point5, p2);
			drawLine(point6, point5);
			drawLine(point2, point3);
			drawLine(point6, p1);
			drawLine(point2, p2);
		}
	}

	private void DrawPlayer(IReadOnlyList<Vector2> points) =>
		DrawPlayer(points, (start, end, color) => Raylib.DrawLineV(start, end, color));

	public void DrawPlayer(
		IReadOnlyList<Vector2> points,
		Action<Vector2, Vector2, Color> drawLine)
	{
		ArgumentNullException.ThrowIfNull(points);
		ArgumentNullException.ThrowIfNull(drawLine);
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
		Vector2 clawOffset = ApplyAtariLocalPresentationToOffset(
			new Vector2(xn, yn) * clawHeight);
		if (atariPresentationEnabled && game.State == LevelState.Playing)
			clawOffset = GetOutwardBlasterClawOffset(points, p1, p2, clawOffset);
		Vector2 outer1 = center + clawOffset;
		Vector2 outer2 = center + clawOffset * .5f;
		Vector2 left1 = p2 - edgeVector * .2f;
		Vector2 left2 = p2 - edgeVector * .3f;
		Vector2 right1 = p2 - edgeVector * .7f;
		Vector2 right2 = p2 - edgeVector * .8f;
		drawLine(left1, outer1, Color.Yellow);
		drawLine(right1, outer1, Color.Yellow);
		drawLine(left1, left2, Color.Yellow);
		drawLine(right1, right2, Color.Yellow);
		drawLine(outer2, left2, Color.Yellow);
		drawLine(outer2, right2, Color.Yellow);
	}

	private Vector2 GetOutwardBlasterClawOffset(
		IReadOnlyList<Vector2> points,
		Vector2 nearStart,
		Vector2 nearEnd,
		Vector2 currentOffset)
	{
		int lane = game.Player.EdgeIndex;
		int nextLane;
		Vector2 farStart;
		Vector2 farEnd;
		nextLane = game.CurrentLevel.NextEdge(lane);
		if (nextLane < 0)
			throw new InvalidOperationException(
				"The Blaster is not on an edge with a corresponding far rail.");

		LevelConfiguration configuration = game.CurrentLevel.Configuration;
		float farDepth = configuration.Position + configuration.Depth;
		farStart = ProjectApproachGeometry(points[lane], farDepth);
		farEnd = ProjectApproachGeometry(points[nextLane], farDepth);

		Vector2 tangent = nearEnd - nearStart;
		float tangentLength = tangent.Length();
		if (!float.IsFinite(tangentLength) || tangentLength <= 0)
			throw new InvalidOperationException("The Blaster rim edge has degenerate presentation geometry.");

		// Matched near-to-far rails identify the tube interior without relying on polygon winding or closure.
		Vector2 inward = ((farStart - nearStart) + (farEnd - nearEnd)) * .5f;
		Vector2 normal = new(-tangent.Y / tangentLength, tangent.X / tangentLength);
		float interiorSide = Vector2.Dot(normal, inward);
		if (!float.IsFinite(interiorSide) || MathF.Abs(interiorSide) <= 1e-4f)
			throw new InvalidOperationException(
				"The Blaster rim edge has no determinable interior side from its presentation rails.");

		Vector2 outwardNormal = interiorSide > 0 ? -normal : normal;
		float currentOffsetLength = currentOffset.Length();
		if (!float.IsFinite(currentOffsetLength))
			throw new InvalidOperationException("The Blaster presentation offset is not finite.");
		return outwardNormal * currentOffsetLength;
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

	private void DrawHud(PresentationMode presentationMode)
	{
		bool portrait = presentationMode == PresentationMode.ExperimentalPortrait;
		int logicalWidth = portrait
			? DisplayConfiguration.PortraitLogicalWidth
			: DisplayConfiguration.LogicalWidth;
		if (portrait)
		{
			foreach (AtariHudLine line in AtariHudVector.Render(game, hudCalibration))
				Raylib.DrawLineV(line.Start, line.End, line.Color);
		}
		else
		{
			Raylib.DrawText(game.Score.Score.ToString(), 10, 10, 28, Color.Yellow);
			Raylib.DrawText($"LIVES {game.Player.Lives}", 10, 44, 20, Color.Yellow);
			string levelText = $"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel}";
			Raylib.DrawText(levelText, 640, 10, 20, Color.Yellow);
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
		}

		if (game.GameOver)
		{
			DrawCenteredText("GAME OVER - PRESS R", 270, 32, Color.Red, presentationMode);
			return;
		}

		if (game.LevelComplete)
		{
			if (game.NaturalProgressionEnabled &&
				LevelProgression.GetNextProductionLevel(game.CurrentLevel.Configuration) is null)
			{
				DrawCenteredText($"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel} COMPLETE",
					230, 36, Color.Yellow, presentationMode);
				DrawCenteredText("PRESS R TO START A NEW CAMPAIGN", 276, 24, Color.Gray,
					presentationMode);
			}
			else
			{
				DrawCenteredText(game.Level3Test ? "LEVEL 3 MIXED WAVE TEST COMPLETE" :
					game.Level3GeometryTest ? "LEVEL 3 GEOMETRY DIAGNOSTIC COMPLETE" :
					game.Level2Test ? "LEVEL 2 TEST COMPLETE" :
					$"LEVEL {game.CurrentLevel.Configuration.DisplayedLevel} COMPLETE",
					250, 36, Color.Yellow, presentationMode);
				DrawCenteredText(game.Level3Test ? "PRESS R TO REPLAY LEVEL 3 TEST" :
					game.Level3GeometryTest ? "PRESS R TO REPLAY DIAGNOSTIC" :
					game.Level2Test ? "PRESS R TO REPLAY TEST" : "PRESS R TO REPLAY",
					296, 24, Color.Gray, presentationMode);
			}
			return;
		}

		if (game.DeathTicks > 0)
			Raylib.DrawText("PLAYER DESTROYED", 10,
				portrait ? 190 : game.Level3Test ? 168 :
				game.TankerTest || game.Level2Test || game.Level3GeometryTest ? 144 : 120,
				18, Color.Red);
	}

	private void DrawPortraitDevelopmentDiagnostics()
	{
		IReadOnlyList<string> waveLines = GetWaveHudLines();
		for (int index = 0; index < waveLines.Count; index++)
		{
			Color color = index == 0 ? Color.Red :
				(game.TankerTest || game.CurrentLevel.UsesProductionLedger) && index == 1
					? TankerColor
					: Color.Gray;
			int y = DisplayConfiguration.DevelopmentDiagnosticHostTop +
				index * DisplayConfiguration.DevelopmentDiagnosticHostLineAdvance;
			Raylib.DrawText(waveLines[index],
				DisplayConfiguration.DevelopmentDiagnosticHostLeft, y, 18, color);
		}

		if (game.Level2Test)
			Raylib.DrawText("LEVEL 2 TEST - DEVELOPMENT ONLY",
				DisplayConfiguration.DevelopmentDiagnosticHostLeft, 120, 16, Color.Gray);
		else if (game.Level3Test)
			Raylib.DrawText("LEVEL 3 MIXED WAVE TEST - DEVELOPMENT ONLY",
				DisplayConfiguration.DevelopmentDiagnosticHostLeft, 144, 16, Color.Gray);
		else if (game.Level3GeometryTest)
			Raylib.DrawText("LEVEL 3 GEOMETRY DIAGNOSTIC - DEVELOPMENT ONLY",
				DisplayConfiguration.DevelopmentDiagnosticHostLeft, 120, 16, Color.Gray);
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

	private static void DrawCenteredText(
		string text,
		int y,
		int size,
		Color color,
		PresentationMode presentationMode)
	{
		bool portrait = presentationMode == PresentationMode.ExperimentalPortrait;
		int logicalWidth = portrait
			? DisplayConfiguration.PortraitLogicalWidth
			: DisplayConfiguration.LogicalWidth;
		int drawSize = size;
		if (portrait)
		{
			while (drawSize > 16 && Raylib.MeasureText(text, drawSize) > logicalWidth - 20)
				drawSize--;
			y += 120;
		}

		int width = Raylib.MeasureText(text, drawSize);
		Raylib.DrawText(text, (logicalWidth - width) / 2, y, drawSize, color);
	}
}
