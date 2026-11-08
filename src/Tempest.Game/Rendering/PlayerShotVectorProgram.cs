using System.Numerics;
using Raylib_cs;

namespace Tempest.Game.Rendering;

public readonly record struct PlayerShotVectorPointGroup(
	int RomColorIndex, Color ProjectColor, IReadOnlyList<Vector2> Points);

public static class PlayerShotVectorProgram
{
	public static readonly float MaximumPointRadius = MathF.Sqrt(242);

	// PROJECT TRANSLATION / MANUALLY ACCEPTED PRESENTATION: the ROM specifies indices, not these RGB values.
	public static Color RomColor8ProjectTranslation { get; } = new(255, 176, 0, 255);
	public static Color RomColor1ProjectTranslation { get; } = new(255, 64, 0, 255);

	private static readonly Vector2[] RomColor8RelativeMoves =
	[
		new(0, 0), new(7, 0), new(-2, 5), new(-5, 2), new(-5, -2),
		new(-2, -5), new(2, -5), new(5, -2), new(5, 2)
	];

	// The final four raw words decode to (0,-11), (0,-11), (+11,0), (+11,0).
	// Preserve the listing's decoded sequence rather than inferring a symmetric ring.
	private static readonly Vector2[] RomColor1RelativeMoves =
	[
		new(10, 5), new(-4, 11), new(-11, 4), new(-11, -4),
		new(0, -11), new(0, -11), new(11, 0), new(11, 0)
	];

	public static IReadOnlyList<PlayerShotVectorPointGroup> Groups { get; } = CreateGroups();

	private static IReadOnlyList<PlayerShotVectorPointGroup> CreateGroups()
	{
		Vector2 beamPosition = Vector2.Zero;
		PlayerShotVectorPointGroup[] groups =
		[
			CreateGroup(8, RomColor8ProjectTranslation, RomColor8RelativeMoves, ref beamPosition),
			CreateGroup(1, RomColor1ProjectTranslation, RomColor1RelativeMoves, ref beamPosition)
		];
		return Array.AsReadOnly(groups);
	}

	private static PlayerShotVectorPointGroup CreateGroup(
		int romColorIndex,
		Color projectColor,
		IReadOnlyList<Vector2> relativeMoves,
		ref Vector2 beamPosition)
	{
		Vector2[] points = new Vector2[relativeMoves.Count];
		for (int index = 0; index < relativeMoves.Count; index++)
		{
			beamPosition += relativeMoves[index];
			points[index] = beamPosition;
		}

		return new PlayerShotVectorPointGroup(
			romColorIndex, projectColor, Array.AsReadOnly(points));
	}
}
