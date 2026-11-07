using System.Numerics;

namespace Tempest.Game.Rendering;

public static class VectorFont
{
    private static readonly IReadOnlyDictionary<char, Vector2[][]> GlyphStrokes =
        new Dictionary<char, Vector2[][]>
        {
            ['G'] =
            [
                [new(5, 1), new(4, 0), new(1, 0), new(0, 1), new(0, 6), new(1, 7), new(4, 7), new(5, 6), new(5, 4)],
                [new(5, 4), new(3, 4)]
            ],
            ['A'] =
            [
                [new(0, 7), new(0, 1), new(1, 0), new(4, 0), new(5, 1), new(5, 7)],
                [new(0, 4), new(5, 4)]
            ],
            ['M'] =
            [
                [new(0, 7), new(0, 0), new(2.5f, 3), new(5, 0), new(5, 7)]
            ],
            ['E'] =
            [
                [new(5, 0), new(0, 0), new(0, 7), new(5, 7)],
                [new(0, 3.5f), new(4, 3.5f)]
            ],
            ['O'] =
            [
                [new(1, 0), new(4, 0), new(5, 1), new(5, 6), new(4, 7), new(1, 7), new(0, 6), new(0, 1), new(1, 0)]
            ],
            ['V'] =
            [
                [new(0, 0), new(2.5f, 7), new(5, 0)]
            ],
            ['R'] =
            [
                [new(0, 7), new(0, 0), new(4, 0), new(5, 1), new(5, 2), new(4, 3), new(0, 3)],
                [new(3, 3), new(5, 7)]
            ],
            ['I'] =
            [
                [new(0, 0), new(5, 0)],
                [new(2.5f, 0), new(2.5f, 7)],
                [new(0, 7), new(5, 7)]
            ],
            ['D'] =
            [
                [new(0, 0), new(3, 0), new(5, 1), new(5, 6), new(4, 7), new(0, 7), new(0, 0)]
            ],
            ['S'] =
            [
                [new(5, 0), new(1, 0), new(0, 1), new(0, 2.5f), new(1, 3.5f),
                    new(4, 3.5f), new(5, 4.5f), new(5, 6), new(4, 7), new(0, 7)]
            ],
            ['P'] =
            [
                [new(0, 7), new(0, 0), new(4, 0), new(5, 1), new(5, 2), new(4, 3), new(0, 3)]
            ],
            ['K'] =
            [
                [new(0, 0), new(0, 7)],
                [new(5, 0), new(0, 3.5f), new(5, 7)]
            ],
            ['0'] =
            [
                [new(1, 0), new(4, 0), new(5, 1), new(5, 6), new(4, 7),
                    new(1, 7), new(0, 6), new(0, 1), new(1, 0)]
            ],
            ['1'] =
            [
                [new(1, 1), new(2.5f, 0), new(2.5f, 7)],
                [new(1, 7), new(4, 7)]
            ],
            ['2'] =
            [
                [new(0, 1), new(1, 0), new(4, 0), new(5, 1), new(5, 2),
                    new(0, 6), new(0, 7), new(5, 7)]
            ],
            ['3'] =
            [
                [new(0, 0), new(4, 0), new(5, 1), new(4, 3.5f),
                    new(5, 6), new(4, 7), new(0, 7)],
                [new(1, 3.5f), new(4, 3.5f)]
            ],
            ['4'] =
            [
                [new(4, 7), new(4, 0), new(0, 4), new(5, 4)]
            ],
            ['5'] =
            [
                [new(5, 0), new(0, 0), new(0, 3.5f), new(4, 3.5f),
                    new(5, 4.5f), new(5, 6), new(4, 7), new(0, 7)]
            ],
            ['6'] =
            [
                [new(4, 0), new(1, 0), new(0, 1), new(0, 6),
                    new(1, 7), new(4, 7), new(5, 6), new(5, 5),
                    new(4, 4), new(0, 4)]
            ],
            ['7'] =
            [
                [new(0, 0), new(5, 0), new(2, 7)]
            ],
            ['8'] =
            [
                [new(1, 0), new(4, 0), new(5, 1), new(5, 2), new(4, 3.5f),
                    new(5, 5), new(5, 6), new(4, 7), new(1, 7), new(0, 6),
                    new(0, 5), new(1, 3.5f), new(0, 2), new(0, 1), new(1, 0)],
                [new(1, 3.5f), new(4, 3.5f)]
            ],
            ['9'] =
            [
                [new(5, 4), new(1, 4), new(0, 3), new(0, 1), new(1, 0),
                    new(4, 0), new(5, 1), new(5, 6), new(4, 7), new(1, 7)]
            ],
            ['/'] =
            [
                [new(5, 0), new(0, 7)]
            ]
        };

    public static void DrawGameOver(
        Vector2 center,
        float scale,
        Action<Vector2, Vector2> drawLine)
    {
        DrawCenteredText("GAME OVER", center, scale, drawLine);
    }

    public static void DrawCenteredText(
        string text,
        Vector2 center,
        float scale,
        Action<Vector2, Vector2> drawLine)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(drawLine);
        if (!float.IsFinite(scale) || scale <= 0)
            throw new ArgumentOutOfRangeException(nameof(scale));

        float width = text.Sum(character => character == ' ' ? 4 : 6) - 1;
        float x = center.X - width * scale / 2;
        float y = center.Y - 7 * scale / 2;
        foreach (char character in text)
        {
            if (GlyphStrokes.TryGetValue(character, out Vector2[][]? strokes))
            {
                foreach (Vector2[] stroke in strokes)
                {
                    for (int index = 1; index < stroke.Length; index++)
                    {
                        Vector2 start = new(x + stroke[index - 1].X * scale,
                            y + stroke[index - 1].Y * scale);
                        Vector2 end = new(x + stroke[index].X * scale,
                            y + stroke[index].Y * scale);
                        drawLine(start, end);
                    }
                }
            }

            x += (character == ' ' ? 4 : 6) * scale;
        }
    }
}
