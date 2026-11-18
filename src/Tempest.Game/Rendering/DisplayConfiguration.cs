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
	public const float WindowScale = 1.6f;
	public const int WindowWidth = (int)(LogicalWidth * WindowScale);
	public const int WindowHeight = (int)(LogicalHeight * WindowScale);
	public const int PortraitLogicalWidth = 600;
	public const int PortraitLogicalHeight = 800;
	public const int PortraitViewportWidth = 720;
	public const int PortraitViewportHeight = 960;
	public const int PortraitViewportLeft = (WindowWidth - PortraitViewportWidth) / 2;
	public const float PortraitOutputScale = 1.2f;
	public const float PortraitPlayfieldScale = 1.05f;
	// PROJECT TRANSLATION: one shared placement fits the production Levels 1-9 tube envelope.
	// Level 6 sets the upper bound; -33.1 leaves about 0.14 host pixels of top-edge clearance.
	public const float PortraitPlayfieldVerticalTranslation = -33.1f;
	public const float PortraitPlayfieldZoom = PortraitOutputScale * PortraitPlayfieldScale;
	public static Rectangle PortraitViewport => new(
		PortraitViewportLeft, 0, PortraitViewportWidth, PortraitViewportHeight);

	// Legacy remains the accepted 800x600 drawing surface uniformly scaled to the host window.
	public static Camera2D PresentationCamera => new()
	{
		Target = Vector2.Zero,
		Offset = Vector2.Zero,
		Rotation = 0,
		Zoom = WindowScale
	};

	// These are experimental project translations: the portrait viewport is centered in the
	// unchanged host window, while one camera uniformly scales and shifts the complete playfield.
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
