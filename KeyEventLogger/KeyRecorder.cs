namespace KeyEventLogger;

public sealed class KeyRecorder
{
    public const int MaximumEntries = 5000;
    public bool IsRecording { get; private set; }
    public event Action<KeyEntry>? EntryRecorded;
    private readonly List<KeyEntry> entries = [];
    public IReadOnlyList<KeyEntry> Entries => entries.AsReadOnly();

    public void Start() => IsRecording = true;
    public void Stop() => IsRecording = false;

    public void Record(Keys key)
    {
        if (!IsRecording) return;
        var entry = new KeyEntry(DateTime.Now, key);
        if (entries.Count == MaximumEntries) entries.RemoveAt(0);
        entries.Add(entry);
        EntryRecorded?.Invoke(entry);
    }
}
