using System.Numerics;

namespace Tempest.Game.Levels;

public static class LevelLoader
{
	public static class Level9RomEvidence
	{
		public const byte ShapeRemap = 0x0D;
		public const byte Scale = 0x0C;
		public const byte Y3d = 0xB0;
		public const byte Y2dLow = 0x40;
		public const byte Y2dHigh = 0x01;
		public const ushort Y2d = 0x0140;
		public const byte Open = 0xFF;
		public const byte FlipperScaleLow = 0x50;
		public const byte FlipperScaleHigh = 0x04;
		public const ushort FlipperScale = 0x0250;
		public const byte TubeColorIndex = 0x05;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x10, 0x10, 0x30, 0x30, 0x50, 0x50, 0x70, 0x70,
			0x90, 0x90, 0xB0, 0xB0, 0xD0, 0xD0, 0xF0, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0x90, 0x70, 0x70, 0x50, 0x50, 0x30, 0x30, 0x10,
			0x10, 0x30, 0x30, 0x50, 0x50, 0x70, 0x70, 0x90
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0C, 0x00, 0x0C, 0x00, 0x0C, 0x00, 0x0C, 0x00,
			0x04, 0x00, 0x04, 0x00, 0x04, 0x00, 0x04, 0x00
		});
	}

	public static LevelDefinition LoadLevel1() => new(true, 400, -80,
		new Vector2[]
		{
			new( 200,    0), new( 184,   76), new( 141,  141), new(  76,  184),
			new(   0,  200), new( -76,  184), new(-141,  141), new(-184,   76),
			new(-200,    0), new(-184,  -76), new(-141, -141), new( -76, -184),
			new(   0, -200), new(  76, -184), new( 141, -141), new( 184,  -76)
		}, "Level 1 test wave");

	public static LevelDefinition LoadLevel2Square() => new(true, 400, -80,
		new Vector2[]
		{
			new( 161,  161), new(  80,  161), new(   0,  161), new( -80,  161),
			new(-161,  161), new(-161,   80), new(-161,    0), new(-161,  -80),
			new(-161, -161), new( -80, -161), new(   0, -161), new(  80, -161),
			new( 161, -161), new( 161,  -80), new( 161,    0), new( 161,   80)
		}, "Level 2 square");

	public static LevelDefinition LoadLevel3Plus() => new(true, 400, -80,
		Array.AsReadOnly(new Vector2[]
		{
			new(  98,   98), new(  98,  197), new(   0,  197), new( -98,  197),
			new( -98,   98), new(-197,   98), new(-197,    0), new(-197,  -98),
			new( -98,  -98), new( -98, -197), new(   0, -197), new(  98, -197),
			new(  98,  -98), new( 197,  -98), new( 197,    0), new( 197,   98)
		}), "Level 3 - plus");

	public static LevelDefinition LoadLevel4Binoculars() => new(true, 380, -65,
		Array.AsReadOnly(new Vector2[]
		{
			new(-33, 101), new(-101, 145), new(-185, 116), new(-219, 58),
			new(-219, -29), new(-185, -87), new(-101, -116), new(-33, -72),
			new(33, -72), new(101, -116), new(185, -87), new(219, -29),
			new(219, 58), new(185, 116), new(101, 145), new(33, 101)
		}), "Level 4 - binoculars");

	public static LevelDefinition LoadLevel5Cross() => new(true, 415, -90,
		Array.AsReadOnly(new Vector2[]
		{
			new(-30, 210), new(-60, 120), new(-120, 60), new(-210, 30),
			new(-210, -30), new(-120, -60), new(-60, -120), new(-30, -210),
			new(30, -210), new(60, -120), new(120, -60), new(210, -30),
			new(210, 30), new(120, 60), new(60, 120), new(30, 210)
		}), "Level 5 - cross");

	public static LevelDefinition LoadLevel6Triangle() => new(true, 360, -40,
		Array.AsReadOnly(new Vector2[]
		{
			new(-116, 145), new(-171, 145), new(-137, 72), new(-102, 0),
			new(-68, -72), new(-34, -145), new(0, -218), new(34, -145),
			new(68, -72), new(102, 0), new(137, 72), new(171, 145),
			new(116, 145), new(54, 145), new(0, 145), new(-54, 145)
		}), "Level 6 - triangle");

	public static LevelDefinition LoadLevel7Clover() => new(true, 310, -10,
		Array.AsReadOnly(new Vector2[]
		{
			new(-160, 160), new(-179, 44), new(-89, 0), new(-179, -44),
			new(-160, -160), new(-44, -179), new(0, -89), new(44, -179),
			new(160, -160), new(179, -44), new(89, 0), new(179, 44),
			new(160, 160), new(44, 179), new(0, 89), new(-44, 179)
		}), "Level 7 - clover");

	// Rev 2 shape row 7 is an open 16-point polyline. The authorized screenshot places
	// its open endpoints above the short base, so reflect centered ROM Y for this
	// screen-down projection; the transform preserves the source shape and lane order.
	public static LevelDefinition LoadLevel8OpenTube()
	{
		byte[] rawX =
		[
			0x10, 0x1E, 0x2C, 0x3A, 0x48, 0x56, 0x64, 0x70,
			0x90, 0x9E, 0xAC, 0xBA, 0xC8, 0xD6, 0xE4, 0xF0
		];
		byte[] rawY =
		[
			0xF0, 0xD0, 0xB0, 0x90, 0x70, 0x50, 0x30, 0x10,
			0x10, 0x30, 0x50, 0x70, 0x90, 0xB0, 0xD0, 0xF0
		];
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[rawX.Length];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(rawX[index] - 128) * scale,
				-(rawY[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 8 - Rev 2 open shape 7");
	}

	// Rev 2 shape row 13 is an open 16-point polyline. Its angle bytes are
	// 0C 00 0C 00 0C 00 0C 00 04 00 04 00 04 00 04 00; presentation bytes are
	// scale 0C, Y3D B0, Y2D 0140, open FF, Flipper scale 0250, color index 05.
	// Keep ROM vertex order; centering, scaling and Y reflection are project translations.
	public static LevelDefinition LoadLevel9OpenTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level9RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level9RomEvidence.X[index] - 128) * scale,
				-(Level9RomEvidence.Y[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 9 - Rev 2 open shape 13");
	}
}
