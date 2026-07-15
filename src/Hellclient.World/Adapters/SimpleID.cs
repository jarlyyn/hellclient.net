using System.Text;
using Hellclient.World.Interfaces;

namespace Hellclient.World.Adapters;
//实现go版本的SimpleID
public class SimpleID : IUniqueid
{
    public SimpleID Instance { get; set; } = new SimpleID();
    public SimpleID()
    {
        var random = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        Current = (UInt32)random.Next(0, int.MaxValue);
    }
    private const string tokens = "0123456789abcdefghijklmnpqrstvwxyz";

    private static string ToBase32(long value)
    {
        if (value == 0) return "0";
        var sb = new StringBuilder();
        long num = Math.Abs(value);
        while (num > 0)
        {
            sb.Append(tokens[(int)(num % 32)]);
            num /= 32;
        }
        char[] array = sb.ToString().ToCharArray();
        Array.Reverse(array);
        return new string(array);
    }
    private static string encodeu32(UInt32 data)
    {
        return encode64(data);
    }
    private static string encode64(Int64 data)
    {
        var hexstr = ToBase32(data);
        var lengthtoken = ToBase32(hexstr.Length);
        return $"{lengthtoken}{hexstr}";
    }
    public UInt32 Current = 0;
    public string Suff { get; set; } = string.Empty;
    public string GenerateID()
    {
        var ts = encode64(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        var val = encodeu32(Interlocked.Increment(ref Current));
        return $"{ts}{val}{Suff}";
    }
}
