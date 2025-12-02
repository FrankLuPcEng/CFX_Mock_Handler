namespace CfxTestTool.Wpf.ViewModels;

public class LogEntryViewModel
{
    public LogEntryViewModel(DateTime timestamp, string message)
    {
        Timestamp = timestamp;
        Message = message;
    }

    public DateTime Timestamp { get; }
    public string Message { get; }

    public override string ToString() => $"[{Timestamp:HH:mm:ss}] {Message}";
}
