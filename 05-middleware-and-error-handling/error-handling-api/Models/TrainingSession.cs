namespace ErrorHandlingApi.Models;

public sealed record TrainingSession(
    int Id,
    string Topic,
    int DurationMinutes);
