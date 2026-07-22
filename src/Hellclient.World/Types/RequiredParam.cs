namespace Hellclient.World.Types;

public class RequiredParam
{
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string Intro { get; set; } = string.Empty;
}

public class ParamsInfo
{
    public Dictionary<string, string> Params { get; set; } = [];
    public Dictionary<string, string> ParamComments { get; set; } = [];
    public List<RequiredParam> RequiredParams { get; set; } = [];
}