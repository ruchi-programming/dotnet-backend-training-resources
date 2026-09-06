namespace NotificationApi.Models;

public sealed record NotificationRequest(
    string Recipient,
    string Message);
