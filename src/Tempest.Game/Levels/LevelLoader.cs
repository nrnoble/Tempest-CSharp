using System.Numerics;

namespace Tempest.Game.Levels;

public static class LevelLoader
{
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
}
