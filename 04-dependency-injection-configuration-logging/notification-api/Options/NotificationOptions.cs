namespace NotificationApi.Options;

public sealed class NotificationOptions
{
    public const string SectionName = "Notification";

    public string SenderName { get; init; } = string.Empty;

    public int MaximumMessageLength { get; init; }
}
