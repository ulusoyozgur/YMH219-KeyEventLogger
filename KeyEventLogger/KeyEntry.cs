namespace KeyEventLogger;

public sealed record KeyEntry(DateTime Time, Keys Key)
{
    public override string ToString() => $"{Time:HH:mm:ss.fff}    {Key}";
}
