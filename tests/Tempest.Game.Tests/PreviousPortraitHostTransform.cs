using System.Numerics;
using Tempest.Game.Rendering;

namespace Tempest.Game.Tests;

internal static class PreviousPortraitHostTransform
{
	// The prior 720x960 viewport was centered at (640,480); the current one is 4/3 larger.
	private static readonly Vector2 PreviousViewportCenter = new(640, 480);
	private static readonly Vector2 CurrentViewportCenter = new(
		DisplayConfiguration.WindowWidth / 2f,
		DisplayConfiguration.WindowHeight / 2f);

	public static Vector2 ToCurrentHost(Vector2 previousHostPoint) =>
		CurrentViewportCenter + (previousHostPoint - PreviousViewportCenter) * (4f / 3f);
}
