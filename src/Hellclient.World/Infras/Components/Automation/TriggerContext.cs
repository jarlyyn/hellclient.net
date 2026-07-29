using Hellclient.World.Types;

namespace Hellclient.World.Components.Automation;

public class TriggerContext
{
	public TriggerContext(string line, Dictionary<string, string> @params)
	{
		Line = line;
		Params = @params;
	}
	public Dictionary<string, string> Params { get; init; }
	public string Line { get; init; }
	public List<ReplacePair>? Expanded { get; set; } = null;
}
