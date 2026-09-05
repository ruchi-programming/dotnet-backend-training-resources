WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.MapGet(
    "/",
    () => Results.Ok(
        new
        {
            Application = "Hello Minimal API",
            Version = "1.0"
        }));

app.MapGet(
    "/health",
    () => Results.Ok(
        new
        {
            Status = "Healthy",
            CheckedAtUtc = DateTimeOffset.UtcNow
        }));

app.MapGet(
    "/api/greetings/{name}",
    (string name) =>
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Results.BadRequest(
                new
                {
                    Error = "Name is required."
                });
        }

        return Results.Ok(
            new
            {
                Message = $"Hello, {name.Trim()}!",
                GeneratedAtUtc = DateTimeOffset.UtcNow
            });
    })
    .WithName("GetGreeting");

app.Run();
