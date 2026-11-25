using System.Numerics;

namespace Tempest.Game.Levels;

public static class LevelLoader
{
	public static class Level9RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(9);

		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
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

	public static class Level10RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(10);

		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x10, 0x10, 0x10, 0x10, 0x16, 0x29, 0x46, 0x69,
			0x97, 0xBA, 0xD7, 0xEA, 0xF0, 0xF0, 0xF0, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0xF0, 0xCB, 0xA6, 0x80, 0x5C, 0x39, 0x20, 0x12,
			0x12, 0x20, 0x39, 0x5C, 0x80, 0xA6, 0xCB, 0xF0
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0C, 0x0C, 0x0C, 0x0D, 0x0E, 0x0F, 0x0F, 0x00,
			0x01, 0x01, 0x02, 0x03, 0x04, 0x04, 0x04, 0x00
		});
	}

	// ROM-BACKED Level 11 geometry evidence only; no production definition is created from this row.
	public static class Level11RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(11);

		public const byte LookupIndex = 10;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
		public const byte FlipperScaleLow = 0x50;
		public const byte FlipperScaleHigh = 0x05;
		public const ushort FlipperScale = 0x02D0;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x10, 0x1E, 0x2D, 0x3C, 0x4B, 0x5A, 0x69, 0x78,
			0x87, 0x96, 0xA5, 0xB4, 0xC3, 0xD2, 0xE1, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40,
			0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		});
	}

	// ROM-BACKED Rev 2 shape row 12 for displayed Level 12.
	public static class Level12RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(12);

		public const byte LookupIndex = 11;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
		public const byte FlipperScaleLow = 0x00;
		public const byte FlipperScaleHigh = 0x04;
		public const ushort FlipperScale = 0x0400;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0xDA, 0xA4, 0x87, 0x80, 0x79, 0x5C, 0x26, 0x10,
			0x10, 0x20, 0x48, 0x80, 0xB8, 0xE0, 0xF0, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0xE4, 0xE8, 0xB7, 0x80, 0xB7, 0xE8, 0xE4, 0xB2,
			0x7A, 0x47, 0x20, 0x10, 0x20, 0x47, 0x7A, 0xB2
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x08, 0x0B, 0x0C, 0x04, 0x05, 0x08, 0x0B, 0x0C,
			0x0D, 0x0E, 0x0F, 0x01, 0x02, 0x03, 0x04, 0x05
		});
	}

	// ROM-BACKED Rev 2 shape row 14 for displayed Level 13.
	public static class Level13RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(13);

		public const byte LookupIndex = 12;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
		public const byte FlipperScaleLow = 0x00;
		public const byte FlipperScaleHigh = 0x04;
		public const ushort FlipperScale = 0x0400;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0xB0, 0x80, 0x50, 0x47, 0x18, 0x30, 0x18, 0x47,
			0x50, 0x80, 0xB0, 0xB9, 0xE8, 0xD4, 0xE8, 0xB9
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0xE6, 0xD0, 0xE6, 0xB9, 0xAE, 0x80, 0x52, 0x47,
			0x14, 0x30, 0x14, 0x47, 0x52, 0x80, 0xAE, 0xB9
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0A, 0x06, 0x0C, 0x08, 0x0E, 0x0A, 0x00, 0x0C,
			0x02, 0x0E, 0x04, 0x00, 0x06, 0x02, 0x08, 0x04
		});
	}

	// ROM/DISASSEMBLY-BACKED Rev 2 shape row 15 for displayed Level 14.
	public static class Level14RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(14);

		public const byte LookupIndex = 13;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
		public const byte FlipperScaleLow = 0x50;
		public const byte FlipperScaleHigh = 0x04;
		public const ushort FlipperScale = 0x0250;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x10, 0x1E, 0x21, 0x28, 0x3C, 0x55, 0x66, 0x73,
			0x8D, 0x9A, 0xAB, 0xC4, 0xD8, 0xDF, 0xE2, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0x7E, 0x6A, 0x51, 0x3A, 0x2C, 0x2C, 0x38, 0x4E,
			0x4E, 0x38, 0x2C, 0x2C, 0x3A, 0x51, 0x6A, 0x7E
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0E, 0x0C, 0x0D, 0x0E, 0x00, 0x02, 0x02, 0x00,
			0x0E, 0x0E, 0x00, 0x02, 0x03, 0x04, 0x02, 0x00
		});
	}

	// ROM-BACKED Rev 2 shape row 11 for displayed Level 16. Coordinates remain in
	// source order; centering, scaling and Y reflection use the established ROM translation.
	public static class Level16RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(16);

		public const byte LookupIndex = 15;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x80, 0x70, 0x48, 0x20, 0x10, 0x20, 0x48, 0x70,
			0x80, 0x90, 0xB8, 0xE0, 0xF0, 0xE0, 0xB8, 0x90
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0x80, 0x57, 0x48, 0x57, 0x80, 0xA9, 0xBA, 0xA9,
			0x80, 0x57, 0x48, 0x57, 0x80, 0xA9, 0xBA, 0xA9
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0B, 0x09, 0x07, 0x05, 0x03, 0x01, 0x0F, 0x0D,
			0x0D, 0x0F, 0x01, 0x03, 0x05, 0x07, 0x09, 0x0B
		});
	}

	// The accepted Level 15 tube uses the manually identified geometry shape row 10.
	public static class Level15RomEvidence
	{
		public static RomPresentationMetadata PresentationMetadata =>
			RomPresentationMetadataCatalog.ForDisplayedLevel(15);

		public const byte LookupIndex = 14;
		public static byte ShapeRemap => checked((byte)PresentationMetadata.TubeTableIndex);
		public static byte Scale => PresentationMetadata.ScaleRaw;
		public static byte Y3d => PresentationMetadata.CameraHeightRaw;
		public static byte Y2dLow => PresentationMetadata.ScreenYOffsetLowRaw;
		public static byte Y2dHigh => PresentationMetadata.ScreenYOffsetHighRaw;
		public static ushort Y2d => PresentationMetadata.ScreenYOffsetCombinedRaw;
		public static byte Open => PresentationMetadata.OpenRaw;
		public const byte FlipperScaleLow = 0x50;
		public const byte FlipperScaleHigh = 0x04;
		public const ushort FlipperScale = 0x0250;

		public static IReadOnlyList<byte> X { get; } = Array.AsReadOnly(new byte[]
		{
			0x10, 0x24, 0x30, 0x36, 0x3E, 0x49, 0x5A, 0x75,
			0x94, 0xA4, 0xAC, 0xBA, 0xDA, 0xE2, 0xEA, 0xF0
		});

		public static IReadOnlyList<byte> Y { get; } = Array.AsReadOnly(new byte[]
		{
			0xC0, 0xA6, 0x8A, 0x6A, 0x4A, 0x2F, 0x14, 0x24,
			0x20, 0x39, 0x59, 0x75, 0x72, 0x90, 0xB0, 0xD0
		});

		public static IReadOnlyList<byte> Angles { get; } = Array.AsReadOnly(new byte[]
		{
			0x0E, 0x0D, 0x0C, 0x0D, 0x0D, 0x0D, 0x01, 0x0F,
			0x02, 0x03, 0x03, 0x00, 0x03, 0x03, 0x03, 0x00
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

	// ROM-BACKED Rev 2 shape row 9; centering, scaling and Y reflection follow the
	// established raw-geometry translation used for the open Level 8 and 9 definitions.
	public static LevelDefinition LoadLevel10OpenTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level10RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level10RomEvidence.X[index] - 128) * scale,
				-(Level10RomEvidence.Y[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 10 - Rev 2 open shape 9");
	}

	// Preserve the manually identified Rev. 2 row-10 tube exactly.
	// Centering, scaling and Y reflection use the established open-tube translation.
	public static LevelDefinition LoadLevel15OpenTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level15RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level15RomEvidence.X[index] - 128) * scale,
				-(Level15RomEvidence.Y[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 15 - Rev 2 open shape 10");
	}

	// ROM-BACKED closed row 11. The ROM's repeated center point is intentional;
	// retaining its order produces two self-touching lobes with zero net area.
	public static LevelDefinition LoadLevel16ClosedTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level16RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level16RomEvidence.X[index] - 128) * scale,
				-(Level16RomEvidence.Y[index] - 128) * scale);

		return new(true, 300, 0, Array.AsReadOnly(points), "Level 16 - Rev 2 closed shape 11");
	}

	// ROM-BACKED Rev 2 shape row 8. The zero-based lookup index is 10,
	// while the selected geometry record index is 8.
	public static LevelDefinition LoadLevel11OpenTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level11RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level11RomEvidence.X[index] - 128) * scale,
				-(Level11RomEvidence.Y[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 11 - Rev 2 open shape 8");
	}

	// ROM-BACKED Rev 2 shape row 12. The zero-based lookup index is 11,
	// while the selected geometry record index is 12.
	public static LevelDefinition LoadLevel12ClosedTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level12RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level12RomEvidence.X[index] - 128) * scale,
				-(Level12RomEvidence.Y[index] - 128) * scale);

		return new(true, 300, 0, Array.AsReadOnly(points), "Level 12 - Rev 2 closed shape 12");
	}

	// ROM-BACKED Rev 2 shape row 14; coordinate centering, scaling and Y reflection
	// remain the established project translation.
	public static LevelDefinition LoadLevel13ClosedTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level13RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level13RomEvidence.X[index] - 128) * scale,
				-(Level13RomEvidence.Y[index] - 128) * scale);

		return new(true, 300, 0, Array.AsReadOnly(points), "Level 13 - Rev 2 closed shape 14");
	}

	// PROJECT TRANSLATION: center row-15 coordinates, apply 25/14 scaling, and reflect Y.
	public static LevelDefinition LoadLevel14OpenTube()
	{
		const float scale = 25f / 14f;
		Vector2[] points = new Vector2[Level14RomEvidence.X.Count];
		for (int index = 0; index < points.Length; index++)
			points[index] = new Vector2(
				(Level14RomEvidence.X[index] - 128) * scale,
				-(Level14RomEvidence.Y[index] - 128) * scale);

		return new(false, 300, 0, Array.AsReadOnly(points), "Level 14 - Rev 2 open shape 15");
	}
}
