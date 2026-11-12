using System.Numerics;

namespace Tempest.Game.Rendering;

public readonly record struct EnemyShotVectorSegment(Vector2 Start, Vector2 End);

public sealed record EnemyShotVectorFrame(
	int SegmentRomColorIndex,
	int PointRomColorIndex,
	IReadOnlyList<EnemyShotVectorSegment> Segments,
	IReadOnlyList<Vector2> Points);

public static class EnemyShotVectorProgram
{
	// PROJECT TRANSLATION: normalize logical coordinates against the existing compact shot scale.
	public const float MaximumLogicalExtent = 24;

	// ROM-BACKED relative geometry and order. Blank beam moves are omitted so they cannot draw lines.
	private static readonly EnemyShotVectorFrame[] FramesData =
	[
		CreateFrame(
			[
				(new(-11, 11), new(-17, 17)),
				(new(-17, -11), new(-11, -17)),
				(new(17, -17), new(11, -11)),
				(new(11, 17), new(17, 11))
			],
			[new(6, 6), new(-6, 6), new(-6, -6), new(6, -6)]),
		CreateFrame(
			[
				(new(-18, 18), new(-18, 4)),
				(new(-8, -14), new(-8, -22)),
				(new(18, -12), new(18, -4)),
				(new(8, 14), new(8, 22))
			],
			[new(-3, 7), new(-7, 3), new(3, -7), new(7, 3)]),
		CreateFrame(
			[
				(new(-17, 3), new(-23, -3)),
				(new(-3, -23), new(3, -17)),
				(new(17, -3), new(23, 3)),
				(new(3, 23), new(-3, 23))
			],
			[new(0, 8), new(-8, 0), new(0, -8), new(8, 0)]),
		CreateFrame(
			[
				(new(-22, -8), new(-14, -8)),
				(new(4, -18), new(12, -18)),
				(new(14, 8), new(22, 8)),
				(new(-4, 18), new(-12, 18))
			],
			[new(-7, 3), new(-3, -7), new(7, 3), new(3, 7)])
	];

	public static IReadOnlyList<EnemyShotVectorFrame> Frames { get; } =
		Array.AsReadOnly(FramesData);

	public static EnemyShotVectorFrame GetFrame(int frame) =>
		frame >= 0 && frame < FramesData.Length
			? FramesData[frame]
			: throw new ArgumentOutOfRangeException(nameof(frame));

	private static EnemyShotVectorFrame CreateFrame(
		(Vector2 Start, Vector2 End)[] segments,
		Vector2[] points)
	{
		EnemyShotVectorSegment[] segmentData = segments
			.Select(segment => new EnemyShotVectorSegment(segment.Start, segment.End))
			.ToArray();
		return new EnemyShotVectorFrame(
			0, 3, Array.AsReadOnly(segmentData), Array.AsReadOnly(points));
	}
}
