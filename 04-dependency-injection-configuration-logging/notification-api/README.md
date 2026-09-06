# Notification API: Dependency Injection, Configuration and Logging

## Learning objectives

After completing this example, students will be able to:

- Register and inject an application service.
- Bind configuration to a strongly typed options class.
- Validate configuration during application startup.
- Use structured logging.
- Understand scoped service lifetime.
- Avoid logging sensitive request content.
- Return `202 Accepted` for accepted asynchronous-style work.

## Request flow

```text
POST /api/notifications
        ↓
Minimal API endpoint
        ↓
INotificationService
        ↓
LoggingNotificationService
        ↓
Structured application log
```

## Dependency injection

The service is registered as scoped:

```csharp
builder.Services.AddScoped<
    INotificationService,
    LoggingNotificationService>();
```

A scoped service instance is created once per HTTP request.

The endpoint depends on the interface, allowing the implementation to be replaced later with an email, SMS, queue or testing implementation.

## Strongly typed configuration

Values in `appsettings.json`:

```json
"Notification": {
  "SenderName": "C Programming Academy",
  "MaximumMessageLength": 200
}
```

are bound to `NotificationOptions`.

The options are validated during startup using:

```csharp
ValidateOnStart()
```

The application fails early when required configuration is missing or invalid.

## Structured logging

The service logs message-template properties:

```csharp
_logger.LogInformation(
    "Notification {NotificationId} accepted " +
    "from {Sender} for {Recipient}",
    ...);
```

This preserves searchable properties for compatible logging systems.

The notification message is deliberately excluded because request bodies may contain personal, confidential or security-sensitive data.

## HTTP response

A successful request returns:

```text
202 Accepted
```

This status indicates that the request has been accepted for processing. The example does not actually send an external notification; it demonstrates service orchestration and logging.

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

Use the exact local address displayed in the terminal.

## Example request

```powershell
$body = @{
    recipient = "student@example.com"
    message = "Your next programming session begins at 10 AM."
} | ConvertTo-Json

Invoke-RestMethod `
    -Method Post `
    -Uri http://localhost:<port>/api/notifications `
    -ContentType "application/json" `
    -Body $body
```

## Expected response

```json
{
  "notificationId": "...",
  "sender": "C Programming Academy",
  "recipient": "student@example.com",
  "acceptedAtUtc": "..."
}
```

## Test cases

| Scenario | Expected result |
|---|---|
| Valid request | `202 Accepted` |
| Missing recipient | `400 Bad Request` |
| Missing message | `400 Bad Request` |
| Message above configured limit | `400 Bad Request` |
| Missing sender configuration | Startup validation failure |
| Invalid maximum length | Startup validation failure |

## Service lifetimes

| Lifetime | Typical behaviour |
|---|---|
| Transient | New instance each time requested |
| Scoped | One instance per HTTP request |
| Singleton | One instance for the application lifetime |

A singleton service must not depend directly on a scoped service.

## Student practice

1. Add a notification channel option.
2. Create separate email and SMS implementations.
3. Select an implementation using configuration.
4. Add a correlation ID to log messages.
5. Store accepted notifications in a repository.
6. Replace the logger-only behaviour with a background queue.
7. Move request validation out of the service.
8. Write tests using a fake notification service.

## Trainer discussion prompts

- Why does the endpoint depend on an interface?
- What is the benefit of strongly typed options?
- Why validate configuration during startup?
- What distinguishes structured logging from string interpolation?
- Why should message bodies usually not be logged?
- Why is a scoped lifetime appropriate here?
- What does `202 Accepted` communicate to the client?
