using System.Numerics;
using Raylib_cs;

namespace Tempest.Game.Rendering;

public readonly record struct AtariHudLine(Vector2 Start, Vector2 End, Color Color);

public readonly record struct AtariHudCalibrationOptions(
	float? ScoreRight = null,
	float? ScoreBaselineY = null,
	float? ScoreScale = null,
	float? HighScoreStartX = null,
	float? HighScoreBaselineY = null,
	float? HighScoreScale = null,
	float? LevelStartX = null,
	float? LevelBaselineY = null,
	float? LevelScale = null,
	float? ReserveLifeStartX = null,
	float? ReserveLifeBaselineY = null,
	float? ReserveLifeScale = null,
	float? ReserveLifeAdvance = null)
{
	public float ResolvedScoreRight => ScoreRight ?? AtariHudVector.ScoreRight;
	public float ResolvedScoreBaselineY => ScoreBaselineY ?? AtariHudVector.ScoreBaselineY;
	public float ResolvedScoreScale => ScoreScale ?? AtariHudVector.HudGlyphScale;
	public float ResolvedHighScoreStartX => HighScoreStartX ?? AtariHudVector.HighScoreStartX;
	public float ResolvedHighScoreBaselineY =>
		HighScoreBaselineY ?? AtariHudVector.SecondaryBaselineY;
	public float ResolvedHighScoreScale =>
		HighScoreScale ?? AtariHudVector.HighScoreGlyphScale;
	public float ResolvedLevelStartX => LevelStartX ?? AtariHudVector.LevelStartX;
	public float ResolvedLevelBaselineY =>
		LevelBaselineY ?? AtariHudVector.LevelBaselineY;
	public float ResolvedLevelScale => LevelScale ?? AtariHudVector.LevelGlyphScale;
	public float ResolvedReserveLifeStartX =>
		ReserveLifeStartX ?? AtariHudVector.ReserveLifeStartX;
	public float ResolvedReserveLifeBaselineY =>
		ReserveLifeBaselineY ?? AtariHudVector.ReserveLifeBaselineY;
	public float ResolvedReserveLifeScale =>
		ReserveLifeScale ?? AtariHudVector.ReserveLifeScale;
	public float ResolvedReserveLifeAdvance =>
		ReserveLifeAdvance ?? AtariHudVector.ReserveLifeAdvance;
	public bool HasExplicitOverrides =>
		ScoreRight is not null ||
		ScoreBaselineY is not null ||
		ScoreScale is not null ||
		HighScoreStartX is not null ||
		HighScoreBaselineY is not null ||
		HighScoreScale is not null ||
		LevelStartX is not null ||
		LevelBaselineY is not null ||
		LevelScale is not null ||
		ReserveLifeStartX is not null ||
		ReserveLifeBaselineY is not null ||
		ReserveLifeScale is not null ||
		ReserveLifeAdvance is not null;
}

public static class AtariHudVector
{
	public const int MaximumDisplayReserveLives = 25;
	public const int ScoreFieldPositions = 6;
	public const int LevelFieldPositions = 2;
	public const float DigitAdvance = 24;
	public const float DigitHeight = 24;

	public const float ReserveLifeStartX = 30;
	public const float ReserveLifeBaselineY = 100;
	public const float ReserveLifeAdvance = 22;
	public const float ReserveLifeScale = 0.60f;

	public const float ScoreRight = 250;
	public const float ScoreBaselineY = 75;
	public const float HudGlyphScale = 1.50f;

	public const float HighScoreStartX = 270;
	public const float InitialsStartX = 380;
	public const float SecondaryBaselineY = 48;
	public const float HighScoreGlyphScale = 0.75f;
	public const float InitialsGlyphScale = 0.75f;

	public const float LevelStartX = 270;
	public const float LevelBaselineY = 75;
	public const float LevelGlyphScale = 0.75f;

	// PROJECT TRANSLATION: Raylib RGB choices are centered here for manual palette adjustment.
	// They correspond to the DVG score/high-score/initials, level, and reserve-life color-stat groups.
	public static readonly Color ScoreColor = Color.Green;
	public static readonly Color HighScoreColor = Color.Green;
	public static readonly Color InitialsColor = Color.Green;
	public static readonly Color LevelColor = Color.Blue;
	public static readonly Color ReserveLifeColor = Color.Yellow;

	// ROM/DISASSEMBLY BACKED glyph coordinates; PROJECT TRANSLATION:
	// C# distinguishes pen-up DVG moves/advance from visible pen-down strokes.
	private static readonly IReadOnlyDictionary<char, Vector2[][]> RomHudGlyphStrokes =
		new Dictionary<char, Vector2[][]>
		{
			['0'] = [[new(0, 0), new(0, 24), new(16, 24), new(16, 0), new(0, 0)]],
			// ROM/DISASSEMBLY BACKED: digit 1 draws one vertical segment after a pen-up advance
			// and uses a later pen-up move for glyph advance; no top or backward-7 stroke.
			['1'] = [[new(8, 0), new(8, 24)]],
			['2'] = [[new(0, 24), new(16, 24), new(16, 12), new(0, 12), new(0, 0), new(16, 0)]],
			['3'] = [[new(0, 0), new(16, 0), new(16, 24), new(0, 24)], [new(16, 12), new(0, 12)]],
			['4'] = [[new(0, 24), new(0, 12), new(16, 12)], [new(16, 24), new(16, 0)]],
			['5'] = [[new(0, 0), new(16, 0), new(16, 12), new(0, 12), new(0, 24), new(16, 24)]],
			['6'] = [[new(0, 12), new(16, 12), new(16, 0), new(0, 0), new(0, 24), new(16, 24)]],
			['7'] = [[new(0, 24), new(16, 24), new(8, 0)]],
			['8'] = [[new(0, 0), new(16, 0), new(16, 24), new(0, 24), new(0, 0)], [new(0, 12), new(16, 12)]],
			['9'] = [[new(16, 0), new(16, 24), new(0, 24), new(0, 12), new(16, 12)]],
			['A'] = [[new(0, 0), new(0, 16), new(8, 24), new(16, 16), new(16, 0)], [new(0, 8), new(16, 8)]],
		};

	public static char[] FormatFixedSixPositionScore(int score)
	{
		uint magnitude = score < 0 ? (uint)(-(long)score) : (uint)score;
		uint displayed = magnitude % 1_000_000;
		string text = displayed.ToString("D6");
		char[] field = new char[ScoreFieldPositions];
		if (displayed == 0)
		{
			Array.Fill(field, ' ');
			field[ScoreFieldPositions - 1] = '0';
			return field;
		}
		bool foundNonzero = false;
		for (int index = 0; index < field.Length; index++)
		{
			char digit = text[index];
			if (!foundNonzero && digit == '0')
				field[index] = ' ';
			else
			{
				foundNonzero = true;
				field[index] = digit;
			}
		}
		return field;
	}

	public static char[] FormatFixedTwoPositionLevel(int displayedLevel)
	{
		long magnitude = displayedLevel < 0 ? -(long)displayedLevel : displayedLevel;
		int displayed = (int)(magnitude % 100);
		string text = displayed.ToString("D2");
		return text.ToCharArray();
	}

	public static IReadOnlyList<AtariHudLine> RenderCenteredText(
		string text,
		Vector2 center,
		float scale,
		Color color)
	{
		ArgumentNullException.ThrowIfNull(text);
		if (text.Any(character => character is < '0' or > '9'))
			throw new ArgumentException("Fuseball bonus text must contain decimal digits only.",
				nameof(text));
		if (!float.IsFinite(scale) || scale <= 0)
			throw new ArgumentOutOfRangeException(nameof(scale));
		if (!float.IsFinite(center.X) || !float.IsFinite(center.Y))
			throw new ArgumentOutOfRangeException(nameof(center));
		if (text.Length == 0)
			return [];

		float visibleWidth = text.Length * DigitAdvance - 8;
		float originX = center.X - visibleWidth * scale / 2;
		float baselineY = center.Y + DigitHeight * scale / 2;
		List<AtariHudLine> lines = [];
		foreach (char character in text)
		{
			AddGlyph(character, originX, baselineY, scale, color, lines);
			originX += DigitAdvance * scale;
		}

		foreach (AtariHudLine line in lines)
		{
			if (!float.IsFinite(line.Start.X) || !float.IsFinite(line.Start.Y) ||
				!float.IsFinite(line.End.X) || !float.IsFinite(line.End.Y))
				throw new InvalidOperationException("Atari digit geometry must remain finite.");
		}
		return lines;
	}

	public static int GetReserveLifeCount(int totalLives)
	{
		int reserveLives = Math.Max(totalLives - 1, 0);
		return Math.Min(reserveLives, MaximumDisplayReserveLives);
	}

	public static IReadOnlyList<AtariHudLine> Render(TempestGame game)
	{
		return Render(game, new AtariHudCalibrationOptions());
	}

	public static IReadOnlyList<AtariHudLine> Render(
		TempestGame game,
		AtariHudCalibrationOptions calibration)
	{
		ArgumentNullException.ThrowIfNull(game);
		float[] coordinates =
		[
			calibration.ResolvedScoreRight,
			calibration.ResolvedScoreBaselineY,
			calibration.ResolvedHighScoreStartX,
			calibration.ResolvedHighScoreBaselineY,
			calibration.ResolvedLevelStartX,
			calibration.ResolvedLevelBaselineY,
			calibration.ResolvedReserveLifeStartX,
			calibration.ResolvedReserveLifeBaselineY,
			calibration.ResolvedReserveLifeAdvance
		];
		if (coordinates.Any(value => !float.IsFinite(value)))
			throw new InvalidOperationException("Atari HUD coordinates must remain finite.");
		float[] scales =
		[
			calibration.ResolvedScoreScale,
			calibration.ResolvedHighScoreScale,
			calibration.ResolvedLevelScale,
			calibration.ResolvedReserveLifeScale
		];
		if (scales.Any(scale => !float.IsFinite(scale) || scale <= 0))
			throw new InvalidOperationException(
				"Atari HUD scales must be positive and finite.");

		List<AtariHudLine> lines = [];

		char[] scoreField = FormatFixedSixPositionScore(game.Score.Score);
		DrawRightJustifiedField(scoreField,
			calibration.ResolvedScoreRight,
			calibration.ResolvedScoreBaselineY,
			calibration.ResolvedScoreScale,
			ScoreColor, lines);

		DrawFixedText("38930",
			calibration.ResolvedHighScoreStartX,
			calibration.ResolvedHighScoreBaselineY,
			calibration.ResolvedHighScoreScale,
			HighScoreColor, lines);
		DrawFixedText("AAA",
			calibration.ResolvedHighScoreStartX +
				(InitialsStartX - HighScoreStartX) *
				(calibration.ResolvedHighScoreScale / HighScoreGlyphScale),
			calibration.ResolvedHighScoreBaselineY,
			calibration.ResolvedHighScoreScale,
			InitialsColor, lines);

		char[] levelField = FormatFixedTwoPositionLevel(
			game.CurrentLevel.Configuration.DisplayedLevel);
		DrawFixedText(new string(levelField),
			calibration.ResolvedLevelStartX,
			calibration.ResolvedLevelBaselineY,
			calibration.ResolvedLevelScale,
			LevelColor, lines);

		int reserveCount = GetReserveLifeCount(game.Player.Lives);
		for (int index = 0; index < reserveCount; index++)
		{
			float originX = calibration.ResolvedReserveLifeStartX +
				index * calibration.ResolvedReserveLifeAdvance;
			AddNominalBlaster(originX,
				calibration.ResolvedReserveLifeBaselineY,
				calibration.ResolvedReserveLifeScale,
				lines);
		}

		foreach (AtariHudLine line in lines)
		{
			if (!float.IsFinite(line.Start.X) || !float.IsFinite(line.Start.Y) ||
				!float.IsFinite(line.End.X) || !float.IsFinite(line.End.Y))
				throw new InvalidOperationException("Atari HUD geometry must remain finite.");
		}
		return lines;
	}

	private static void DrawRightJustifiedField(
		IReadOnlyList<char> field,
		float right,
		float baselineY,
		float scale,
		Color color,
		List<AtariHudLine> lines)
	{
		// A ROM glyph occupies 16 of its 24-unit advance cell; retain the 8-unit trailing
		// bearing inside the fixed field so visible strokes end at the stable right edge.
		float x = right - (field.Count * 24 - 8) * scale;
		foreach (char character in field)
		{
			if (character != ' ')
				AddGlyph(character, x, baselineY, scale, color, lines);
			x += 24 * scale;
		}
	}

	private static void DrawFixedText(
		string text,
		float startX,
		float baselineY,
		float scale,
		Color color,
		List<AtariHudLine> lines)
	{
		float x = startX;
		foreach (char character in text)
		{
			AddGlyph(character, x, baselineY, scale, color, lines);
			x += 24 * scale;
		}
	}

	public static void AddGlyph(
		char character,
		float originX,
		float baselineY,
		float scale,
		Color color,
		List<AtariHudLine> lines)
	{
		if (!RomHudGlyphStrokes.TryGetValue(character, out Vector2[][]? strokes))
			throw new ArgumentException($"Unsupported Atari HUD glyph: {character}", nameof(character));

		foreach (Vector2[] stroke in strokes)
		{
			for (int index = 1; index < stroke.Length; index++)
			{
				Vector2 start = new(
					originX + stroke[index - 1].X * scale,
					baselineY - stroke[index - 1].Y * scale);
				Vector2 end = new(
					originX + stroke[index].X * scale,
					baselineY - stroke[index].Y * scale);
				lines.Add(new AtariHudLine(start, end, color));
			}
		}
	}

	private static void AddNominalBlaster(
		float originX,
		float baselineY,
		float scale,
		List<AtariHudLine> lines)
	{
		Vector2[] points =
		[
			new(24, -12), new(6, -18), new(18, -12), new(0, -6),
			new(-18, -12), new(-6, -18), new(-24, -12), new(0, 0)
		];
		Vector2 previous = new(originX, baselineY);
		foreach (Vector2 point in points)
		{
			Vector2 next = new(
				originX + point.X * scale,
				baselineY - point.Y * scale);
			lines.Add(new AtariHudLine(previous, next, ReserveLifeColor));
			previous = next;
		}
	}
}
