using NotificationApi.Models;
using NotificationApi.Options;
using NotificationApi.Services;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<NotificationOptions>()
    .Bind(
        builder.Configuration.GetSection(
            NotificationOptions.SectionName))
    .Validate(
        options =>
            !string.IsNullOrWhiteSpace(options.SenderName),
        "Notification sender name is required.")
    .Validate(
        options =>
            options.MaximumMessageLength is > 0 and <= 1000,
        "Maximum message length must be between 1 and 1000.")
    .ValidateOnStart();

builder.Services.AddScoped<
    INotificationService,
    LoggingNotificationService>();

WebApplication app = builder.Build();

app.MapPost(
    "/api/notifications",
    (
        NotificationRequest request,
        INotificationService service) =>
    {
        try
        {
            NotificationResult result =
                service.Send(request);

            return Results.Accepted(value: result);
        }
        catch (ArgumentException exception)
        {
            return Results.BadRequest(
                new
                {
                    Error = exception.Message
                });
        }
    });

app.Run();
