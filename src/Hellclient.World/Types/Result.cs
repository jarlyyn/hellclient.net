namespace Hellclient.World.Types;

public record StringResult(string Info, bool Successed)
{
    public string Info = Info;
    public bool Successed = Successed;
    public FoundStringResult Found()
    {
        return new FoundStringResult(Info, Successed, true);
    }
}

public record BoolResult(bool Value, bool Successed)
{
    public bool Value = Value;
    public bool Successed = Successed;
    public FoundBoolResult Found()
    {
        return new FoundBoolResult(Value, Successed, true);
    }
}

public record FoundStringResult(string Info, bool Successed, bool Found)
{
    public static FoundStringResult NotFound { get; } = new FoundStringResult("", false, false);
    public string Info = Info;
    public bool Successed = Successed;
    public bool Found = Found;
}

public record FoundBoolResult(bool Value, bool Successed, bool Found)
{
    public static FoundBoolResult NotFound { get; } = new FoundBoolResult(false, false, false);

    public bool Value = Value;
    public bool Successed = Successed;
    public bool Found = Found;
}