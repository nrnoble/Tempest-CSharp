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
}
