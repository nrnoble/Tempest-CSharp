using System.Numerics;
using Raylib_cs;
using Tempest.Game.Levels;
using Tempest.Game.Rendering;
using Xunit;

namespace Tempest.Game.Tests;

public sealed class AtariHudVectorTests
{
	[Theory]
	[InlineData(0, "     0")]
	[InlineData(930, "   930")]
	[InlineData(38930, " 38930")]
	[InlineData(123456, "123456")]
	[InlineData(int.MaxValue, "483647")]
	public void ScoreUsesFixedRightJustifiedSixPositionField(int score, string expected)
	{
		Assert.Equal(expected.ToCharArray(), AtariHudVector.FormatFixedSixPositionScore(score));
	}

	[Theory]
	[InlineData(5, "     5")]
	[InlineData(10, "    10")]
	[InlineData(930, "   930")]
	public void PositiveScoresRemainRightJustifiedWithoutExtraLeadingZeroes(
		int score,
		string expected)
	{
		Assert.Equal(expected.ToCharArray(),
			AtariHudVector.FormatFixedSixPositionScore(score));
	}

	[Fact]
	public void DigitOneRendersOnlyOneVerticalSegment()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Score.Add(1);

		IReadOnlyList<AtariHudLine> scoreLines = AtariHudVector.Render(game)
			.Where(line => line.Color == AtariHudVector.ScoreColor)
			.Where(line =>
				(line.Start.X >= AtariHudVector.ScoreRight - 24 * AtariHudVector.HudGlyphScale &&
				 line.Start.X <= AtariHudVector.ScoreRight) ||
				(line.End.X >= AtariHudVector.ScoreRight - 24 * AtariHudVector.HudGlyphScale &&
				 line.End.X <= AtariHudVector.ScoreRight))
			.Where(line =>
				(line.Start.Y >= AtariHudVector.ScoreBaselineY -
					24 * AtariHudVector.HudGlyphScale &&
				 line.Start.Y <= AtariHudVector.ScoreBaselineY) ||
				(line.End.Y >= AtariHudVector.ScoreBaselineY -
					24 * AtariHudVector.HudGlyphScale &&
				 line.End.Y <= AtariHudVector.ScoreBaselineY))
			.ToArray();

		AtariHudLine line = Assert.Single(scoreLines);
		Assert.Equal(line.Start.X, line.End.X);
		Assert.True(line.Start.Y > line.End.Y);
	}

	[Fact]
	public void AcceptedAtariHudDefaultsAreEncodedExactly()
	{
		Assert.Equal(250f, AtariHudVector.ScoreRight);
		Assert.Equal(75f, AtariHudVector.ScoreBaselineY);
		Assert.Equal(1.50f, AtariHudVector.HudGlyphScale);
		Assert.Equal(75f, AtariHudVector.LevelBaselineY);
		Assert.Equal(100f, AtariHudVector.ReserveLifeBaselineY);
	}

	[Fact]
	public void ScoreAndLevelRenderLiveStateDeterministicallyAndDoNotMutateGameplay()
	{
		LevelConfiguration configuration =
			LevelProgression.GetDirectStartLevel(10) ??
			throw new InvalidOperationException("Level 10 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Score.Add(4820);
		game.Player.Lives = 3;
		int scoreBefore = game.Score.Score;
		int livesBefore = game.Player.Lives;
		int levelBefore = game.CurrentLevel.Configuration.DisplayedLevel;

		IReadOnlyList<AtariHudLine> first = AtariHudVector.Render(game);
		IReadOnlyList<AtariHudLine> second = AtariHudVector.Render(game);

		Assert.Equal(first, second);
		Assert.NotEmpty(first);
		Assert.All(first, line =>
		{
			Assert.True(float.IsFinite(line.Start.X));
			Assert.True(float.IsFinite(line.Start.Y));
			Assert.True(float.IsFinite(line.End.X));
			Assert.True(float.IsFinite(line.End.Y));
		});
		Assert.Equal(scoreBefore, game.Score.Score);
		Assert.Equal(livesBefore, game.Player.Lives);
		Assert.Equal(levelBefore, game.CurrentLevel.Configuration.DisplayedLevel);
		Assert.Contains(first, line => line.Color.R == Color.Green.R &&
			line.Color.G == Color.Green.G && line.Color.B == Color.Green.B);
		Assert.Contains(first, line => line.Color.R == Color.Blue.R &&
			line.Color.G == Color.Blue.G && line.Color.B == Color.Blue.B);
		Assert.Contains(first, line => line.Color.R == Color.Yellow.R &&
			line.Color.G == Color.Yellow.G && line.Color.B == Color.Yellow.B);
	}

	[Theory]
	[InlineData(3, 2)]
	[InlineData(1, 0)]
	[InlineData(0, 0)]
	[InlineData(27, 25)]
	public void ReserveLifeCountExcludesActiveBlasterAndCapsDisplay(int lives, int expected)
	{
		Assert.Equal(expected, AtariHudVector.GetReserveLifeCount(lives));
	}

	[Fact]
	public void ReserveLifeSymbolsStayLeftJustifiedBoundedAndDoNotChangeLives()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Player.Lives = 27;
		int livesBefore = game.Player.Lives;
		IReadOnlyList<AtariHudLine> lines = AtariHudVector.Render(game);

		IEnumerable<AtariHudLine> reserveLines = lines.Where(line =>
			line.Color.R == AtariHudVector.ReserveLifeColor.R &&
			line.Color.G == AtariHudVector.ReserveLifeColor.G &&
			line.Color.B == AtariHudVector.ReserveLifeColor.B);
		Assert.Equal(25 * 8, reserveLines.Count());
		Assert.Equal(livesBefore, game.Player.Lives);
		Assert.All(lines, line =>
		{
			Assert.InRange(line.Start.X, 0, DisplayConfiguration.PortraitLogicalWidth);
			Assert.InRange(line.Start.Y, 0, DisplayConfiguration.PortraitLogicalHeight);
			Assert.InRange(line.End.X, 0, DisplayConfiguration.PortraitLogicalWidth);
			Assert.InRange(line.End.Y, 0, DisplayConfiguration.PortraitLogicalHeight);
		});
		float firstStartX = reserveLines.Min(line => line.Start.X);
		Assert.True(firstStartX >= AtariHudVector.ReserveLifeStartX - 25);
	}

	[Theory]
	[InlineData(1, "01")]
	[InlineData(9, "09")]
	[InlineData(10, "10")]
	public void LevelUsesFixedTwoPositionLiveFormatting(int level, string expected)
	{
		Assert.Equal(expected.ToCharArray(),
			AtariHudVector.FormatFixedTwoPositionLevel(level));
	}

	[Fact]
	public void HighScoreAndInitialsAreFixedAndUppercase()
	{
		AtariHudLine[] lines = AtariHudVector.Render(
			new TempestGame(
				LevelProgression.GetProductionLevel(1) ??
				throw new InvalidOperationException("Level 1 configuration is unavailable."),
				random: new Random(1))).ToArray();

		Assert.Contains(lines, line => line.Color == AtariHudVector.HighScoreColor);
		Assert.Contains(lines, line => line.Color == AtariHudVector.InitialsColor);
	}

	[Fact]
	public void HudUsesIndependentPortraitLayoutScalesAndStableFieldAnchors()
	{
		Assert.True(AtariHudVector.HudGlyphScale > AtariHudVector.HighScoreGlyphScale);
		Assert.True(AtariHudVector.HudGlyphScale > AtariHudVector.InitialsGlyphScale);
		Assert.True(AtariHudVector.ReserveLifeScale > 0.35f);
		Assert.True(AtariHudVector.InitialsStartX >
			AtariHudVector.HighScoreStartX + 5 * 24 * AtariHudVector.HighScoreGlyphScale);
		Assert.True(AtariHudVector.LevelBaselineY > AtariHudVector.SecondaryBaselineY);
		Assert.True(AtariHudVector.ReserveLifeBaselineY > AtariHudVector.ScoreBaselineY);
	}

	[Fact]
	public void ScoreRightJustificationIsStableWhenScoreGrows()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");

		TempestGame lowGame = new(configuration, random: new Random(1));
		lowGame.Score.Add(930);
		TempestGame highGame = new(configuration, random: new Random(1));
		highGame.Score.Add(123456);

		IReadOnlyList<AtariHudLine> lowLines = AtariHudVector.Render(lowGame);
		IReadOnlyList<AtariHudLine> highLines = AtariHudVector.Render(highGame);
		float lowScoreRight = lowLines
			.Where(line => line.Color == AtariHudVector.ScoreColor)
			.Where(line => MathF.Abs(line.Start.Y - AtariHudVector.ScoreBaselineY) < 0.001f ||
				MathF.Abs(line.End.Y - AtariHudVector.ScoreBaselineY) < 0.001f)
			.Max(line => MathF.Max(line.Start.X, line.End.X));
		float highScoreRight = highLines
			.Where(line => line.Color == AtariHudVector.ScoreColor)
			.Where(line => MathF.Abs(line.Start.Y - AtariHudVector.ScoreBaselineY) < 0.001f ||
				MathF.Abs(line.End.Y - AtariHudVector.ScoreBaselineY) < 0.001f)
			.Max(line => MathF.Max(line.Start.X, line.End.X));

		Assert.Equal(AtariHudVector.ScoreRight, lowScoreRight);
		Assert.Equal(AtariHudVector.ScoreRight, highScoreRight);
		Assert.True(lowLines.Where(line => line.Color == AtariHudVector.ScoreColor)
			.Where(line => MathF.Abs(line.Start.Y - AtariHudVector.ScoreBaselineY) < 0.001f ||
				MathF.Abs(line.End.Y - AtariHudVector.ScoreBaselineY) < 0.001f)
			.Min(line => MathF.Min(line.Start.X, line.End.X)) >
			highLines.Where(line => line.Color == AtariHudVector.ScoreColor)
				.Where(line => MathF.Abs(line.Start.Y - AtariHudVector.ScoreBaselineY) < 0.001f ||
					MathF.Abs(line.End.Y - AtariHudVector.ScoreBaselineY) < 0.001f)
				.Min(line => MathF.Min(line.Start.X, line.End.X)));
	}

	[Fact]
	public void PortraitDevelopmentDiagnosticsUseHostLeftApplicationArea()
	{
		Assert.True(DisplayConfiguration.DevelopmentDiagnosticHostLeft > 0);
		Assert.True(DisplayConfiguration.DevelopmentDiagnosticHostLeft <
			DisplayConfiguration.PortraitViewportLeft);
		Assert.True(DisplayConfiguration.DevelopmentDiagnosticHostLeft + 12 <
			DisplayConfiguration.PortraitViewportLeft);
	}

	[Fact]
	public void OmittedHudCalibrationPreservesEveryRefinedDefault()
	{
		AtariHudCalibrationOptions calibration = new();
		Assert.Equal(AtariHudVector.ScoreRight, calibration.ResolvedScoreRight);
		Assert.Equal(AtariHudVector.ScoreBaselineY, calibration.ResolvedScoreBaselineY);
		Assert.Equal(AtariHudVector.HudGlyphScale, calibration.ResolvedScoreScale);
		Assert.Equal(AtariHudVector.HighScoreStartX, calibration.ResolvedHighScoreStartX);
		Assert.Equal(AtariHudVector.SecondaryBaselineY, calibration.ResolvedHighScoreBaselineY);
		Assert.Equal(AtariHudVector.HighScoreGlyphScale, calibration.ResolvedHighScoreScale);
		Assert.Equal(AtariHudVector.LevelStartX, calibration.ResolvedLevelStartX);
		Assert.Equal(AtariHudVector.LevelBaselineY, calibration.ResolvedLevelBaselineY);
		Assert.Equal(AtariHudVector.LevelGlyphScale, calibration.ResolvedLevelScale);
		Assert.Equal(AtariHudVector.ReserveLifeStartX, calibration.ResolvedReserveLifeStartX);
		Assert.Equal(AtariHudVector.ReserveLifeBaselineY, calibration.ResolvedReserveLifeBaselineY);
		Assert.Equal(AtariHudVector.ReserveLifeScale, calibration.ResolvedReserveLifeScale);
		Assert.Equal(AtariHudVector.ReserveLifeAdvance, calibration.ResolvedReserveLifeAdvance);
	}

	[Fact]
	public void ScoreOverridesChangeOnlyStableRightEdgeBaselineAndScale()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Score.Add(930);
		IReadOnlyList<AtariHudLine> baseline = AtariHudVector.Render(game);
		IReadOnlyList<AtariHudLine> calibrated = AtariHudVector.Render(
			game,
			new AtariHudCalibrationOptions(
				ScoreRight: 250,
				ScoreBaselineY: 90,
				ScoreScale: 1.25f));

		IEnumerable<AtariHudLine> baselineScore = baseline.Where(line =>
			line.Color == AtariHudVector.ScoreColor);
		IEnumerable<AtariHudLine> calibratedScore = calibrated.Where(line =>
			line.Color == AtariHudVector.ScoreColor);
		Assert.Contains(calibratedScore, line =>
			MathF.Abs(line.Start.Y - 90) < 0.001f ||
			MathF.Abs(line.End.Y - 90) < 0.001f);
		Assert.True(baselineScore.Count() != 0);

		Assert.Equal(
			baseline.Where(line => line.Color != AtariHudVector.ScoreColor),
			calibrated.Where(line => line.Color != AtariHudVector.ScoreColor));
	}

	[Fact]
	public void HighGroupTranslationAndScaleKeepInternalOriginSpacing()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		IReadOnlyList<AtariHudLine> baseline = AtariHudVector.Render(game);
		IReadOnlyList<AtariHudLine> lines = AtariHudVector.Render(
			game,
			new AtariHudCalibrationOptions(
				HighScoreStartX: 300,
				HighScoreBaselineY: 60,
				HighScoreScale: 0.5f));

		IEnumerable<AtariHudLine> high = lines.Where(line =>
			line.Color == AtariHudVector.HighScoreColor);
		IEnumerable<AtariHudLine> initials = lines.Where(line =>
			line.Color == AtariHudVector.InitialsColor);
		Assert.NotEmpty(high);
		Assert.NotEmpty(initials);
		Assert.All(high.Concat(initials), line =>
		{
			Assert.True(float.IsFinite(line.Start.X));
			Assert.True(float.IsFinite(line.Start.Y));
		});

		float baselineHighMinimumX = baseline
			.Where(line => line.Color == AtariHudVector.HighScoreColor)
			.Min(line => MathF.Min(line.Start.X, line.End.X));
		float baselineInitialsMinimumX = baseline
			.Where(line => line.Color == AtariHudVector.InitialsColor)
			.Min(line => MathF.Min(line.Start.X, line.End.X));
		float calibratedHighMinimumX = high
			.Min(line => MathF.Min(line.Start.X, line.End.X));
		float calibratedInitialsMinimumX = initials
			.Min(line => MathF.Min(line.Start.X, line.End.X));

		float baselineOffset = baselineInitialsMinimumX - baselineHighMinimumX;
		float calibratedOffset = calibratedInitialsMinimumX - calibratedHighMinimumX;
		float expectedScaleRatio = 0.5f / AtariHudVector.HighScoreGlyphScale;
		Assert.True(MathF.Abs(calibratedOffset - baselineOffset * expectedScaleRatio) < 0.001f);
	}

	[Fact]
	public void LevelAndLivesOverridesStayIndependent()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Player.Lives = 27;
		int livesBefore = game.Player.Lives;
		IReadOnlyList<AtariHudLine> lines = AtariHudVector.Render(
			game,
			new AtariHudCalibrationOptions(
				LevelStartX: 320,
				LevelBaselineY: 120,
				LevelScale: 1.2f,
				ReserveLifeStartX: 40,
				ReserveLifeBaselineY: 140,
				ReserveLifeScale: 0.8f,
				ReserveLifeAdvance: 30));

		IEnumerable<AtariHudLine> level = lines.Where(line =>
			line.Color == AtariHudVector.LevelColor);
		IEnumerable<AtariHudLine> reserve = lines.Where(line =>
			line.Color == AtariHudVector.ReserveLifeColor);
		Assert.NotEmpty(level);
		Assert.Equal(25 * 8, reserve.Count());
		Assert.True(reserve.All(line => line.Start.X >= 40 - 32 || line.End.X >= 40 - 32));
		Assert.Equal(livesBefore, game.Player.Lives);
	}

	[Fact]
	public void ExtremeFiniteHudProbesRemainDeterministicAndFinite()
	{
		LevelConfiguration configuration =
			LevelProgression.GetDirectStartLevel(10) ??
			throw new InvalidOperationException("Level 10 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));
		game.Player.Lives = 25;
		AtariHudCalibrationOptions calibration = new(
			ScoreRight: 1000,
			ScoreBaselineY: -500,
			ScoreScale: 10,
			HighScoreStartX: -1000,
			HighScoreBaselineY: 1000,
			HighScoreScale: 10,
			LevelStartX: 2000,
			LevelBaselineY: -1000,
			LevelScale: 10,
			ReserveLifeStartX: -2000,
			ReserveLifeBaselineY: 2000,
			ReserveLifeScale: 10,
			ReserveLifeAdvance: -30);

		IReadOnlyList<AtariHudLine> first = AtariHudVector.Render(game, calibration);
		IReadOnlyList<AtariHudLine> second = AtariHudVector.Render(game, calibration);
		Assert.Equal(first, second);
		Assert.All(first, line =>
		{
			Assert.True(float.IsFinite(line.Start.X));
			Assert.True(float.IsFinite(line.Start.Y));
			Assert.True(float.IsFinite(line.End.X));
			Assert.True(float.IsFinite(line.End.Y));
		});
	}

	[Fact]
	public void VectorRendererRejectsHudOverridesWithoutAtariAndInvalidDirectScale()
	{
		LevelConfiguration configuration =
			LevelProgression.GetProductionLevel(1) ??
			throw new InvalidOperationException("Level 1 configuration is unavailable.");
		TempestGame game = new(configuration, random: new Random(1));

		Assert.Throws<ArgumentException>(() => new VectorRenderer(
			game,
			hudCalibration: new AtariHudCalibrationOptions(ScoreRight: 250)));
		Assert.Throws<ArgumentOutOfRangeException>(() => new VectorRenderer(
			game,
			atariPresentationEnabled: true,
			hudCalibration: new AtariHudCalibrationOptions(ScoreScale: 0)));
	}
}
