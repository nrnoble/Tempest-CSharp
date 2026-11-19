using System.Numerics;
using Raylib_cs;

namespace Tempest.Game.Rendering;

public enum PresentationMode
{
	Legacy,
	ExperimentalPortrait
}

public static class DisplayConfiguration
{
	public const int LogicalWidth = 800;
	public const int LogicalHeight = 600;
	public const int WindowWidth = 1600;
	public const int WindowHeight = 1280;
	public const float WindowScale = 2f;
	public const int PortraitLogicalWidth = 600;
	public const int PortraitLogicalHeight = 800;
	public const int PortraitViewportHeight = WindowHeight;
	public const int PortraitViewportWidth =
		PortraitViewportHeight * PortraitLogicalWidth / PortraitLogicalHeight;
	public const int PortraitViewportLeft = (WindowWidth - PortraitViewportWidth) / 2;
	public const float PortraitOutputScale = PortraitViewportHeight / (float)PortraitLogicalHeight;
	public const float PortraitPlayfieldScale = 1.05f;
	public const float PortraitViewportBorderThickness = 1;
	// PROJECT TRANSLATION: one shared placement fits the production Levels 1-9 tube envelope.
	// Level 6 sets the upper bound; -33.1 leaves about 0.19 host pixels of top-edge clearance.
	public const float PortraitPlayfieldVerticalTranslation = -33.1f;
	public const float PortraitPlayfieldZoom = PortraitOutputScale * PortraitPlayfieldScale;
	public static Rectangle PortraitViewport => new(
		PortraitViewportLeft, 0, PortraitViewportWidth, PortraitViewportHeight);

	// Fit the legacy 4:3 canvas uniformly to the host width and center it vertically.
	public static Camera2D PresentationCamera => new()
	{
		Target = Vector2.Zero,
		Offset = new Vector2(
			(WindowWidth - LogicalWidth * WindowScale) / 2f,
			(WindowHeight - LogicalHeight * WindowScale) / 2f),
		Rotation = 0,
		Zoom = WindowScale
	};

	// The portrait viewport is centered in the host window; only its uniform host scale changes.
	// Projection/model coordinates stay in the legacy 800x600 space and are never rewritten.
	public static Camera2D ExperimentalPortraitPlayfieldCamera
	{
		get
		{
			float targetY = (WindowHeight / 2f -
				PortraitOutputScale * PortraitPlayfieldVerticalTranslation) /
				PortraitPlayfieldZoom;
			return new()
			{
				Target = new Vector2(LogicalWidth / 2f, targetY),
				Offset = new Vector2(WindowWidth / 2f, WindowHeight / 2f),
				Rotation = 0,
				Zoom = PortraitPlayfieldZoom
			};
		}
	}

	// HUD coordinates use the separate 600x800 portrait layout, not the playfield transform.
	public static Camera2D ExperimentalPortraitHudCamera => new()
	{
		Target = Vector2.Zero,
		Offset = new Vector2(PortraitViewportLeft, 0),
		Rotation = 0,
		Zoom = PortraitOutputScale
	};

	public static Camera2D GetPlayfieldCamera(PresentationMode mode) =>
		mode switch
		{
			PresentationMode.Legacy => PresentationCamera,
			PresentationMode.ExperimentalPortrait => ExperimentalPortraitPlayfieldCamera,
			_ => throw new ArgumentOutOfRangeException(nameof(mode))
		};
}
