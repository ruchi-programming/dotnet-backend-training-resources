namespace StudentRegistrationApi.Models;

public sealed record StudentRegistrationResponse(
    Guid RegistrationId,
    string FullName,
    string Email,
    string Course,
    DateTimeOffset RegisteredAtUtc);
