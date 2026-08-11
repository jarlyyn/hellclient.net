using Hellclient.World.Types;

namespace Hellclient.Core.Types.Forms;
public class RequiredParamsForm
{
    public string Current { get; set; } = "";
    public List<RequiredParam> RequiredParams { get; set; } = [];
}