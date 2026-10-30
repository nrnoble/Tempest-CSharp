namespace Tempest.Game.Debug;
public sealed class DebugController
{
	public static bool UseSpikerWave(IEnumerable<string> arguments) =>
		arguments.Contains("--spiker-test", StringComparer.Ordinal);
}
