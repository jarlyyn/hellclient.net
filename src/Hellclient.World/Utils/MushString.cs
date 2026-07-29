namespace Hellclient.World.Utils;

public class MushString
{
	public static string StringYes = "y";
    public const int UpdateOK =0;
    public const int UpdateFailNotFound = 1;
    public const int UpdateFailDuplicateName = 2;

	public static string ToStringBool(bool v)
	{
		return v ? StringYes : "";
	}

	public static bool FromStringBool(string v)
	{
		return v == StringYes || v == "1";
	}

	public static int FromStringInt(string v)
	{
		return int.TryParse(v, out var i) ? i : 0;
	}

	public static double FromStringFloat(string v)
	{
		return double.TryParse(v, out var i) ? i : 0;
	}

}