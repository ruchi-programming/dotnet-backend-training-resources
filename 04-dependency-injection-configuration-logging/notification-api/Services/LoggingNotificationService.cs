using Microsoft.Extensions.Options;
using NotificationApi.Models;
using NotificationApi.Options;

namespace NotificationApi.Services;

public sealed class LoggingNotificationService
    : INotificationService
{
    private readonly NotificationOptions _options;

    private readonly ILogger<LoggingNotificationService>
        _logger;

    public LoggingNotificationService(
        IOptions<NotificationOptions> options,
        ILogger<LoggingNotificationService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public NotificationResult Send(
        NotificationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Recipient))
        {
            throw new ArgumentException(
                "Recipient is required.",
                nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            throw new ArgumentException(
                "Message is required.",
                nameof(request));
        }

        if (request.Message.Length >
            _options.MaximumMessageLength)
        {
            throw new ArgumentException(
                $"Message cannot exceed " +
                $"{_options.MaximumMessageLength} characters.",
                nameof(request));
        }

        NotificationResult result = new(
            Guid.NewGuid(),
            _options.SenderName,
            request.Recipient.Trim(),
            DateTimeOffset.UtcNow);

        _logger.LogInformation(
            "Notification {NotificationId} accepted " +
            "from {Sender} for {Recipient}",
            result.NotificationId,
            result.Sender,
            result.Recipient);

        return result;
    }
}
