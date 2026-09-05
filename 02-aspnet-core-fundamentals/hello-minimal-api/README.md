# Hello ASP.NET Core Minimal API

## Learning objectives

After completing this example, students will be able to:

- Create an ASP.NET Core application.
- Explain the application builder and request pipeline.
- Map HTTP GET endpoints.
- Read a value from a route.
- Return JSON responses with HTTP status codes.
- Run and stop a local web server.
- Test endpoints using a browser or API client.

## Application startup

```csharp
WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();
```

The builder prepares application configuration, logging and dependency injection.

`Build()` creates the web application.

```csharp
app.Run();
```

starts the web server and processes incoming requests.

## Endpoints

| Method | Route | Purpose |
|---|---|---|
| `GET` | `/` | Returns application information |
| `GET` | `/health` | Returns a basic health response |
| `GET` | `/api/greetings/{name}` | Returns a personalized greeting |

## Route parameter

In this route:

```text
/api/greetings/{name}
```

`{name}` is bound to the endpoint’s `string name` parameter.

Example:

```text
/api/greetings/Ruchi
```

produces:

```json
{
  "message": "Hello, Ruchi!",
  "generatedAtUtc": "..."
}
```

## HTTP responses

The sample uses:

```csharp
Results.Ok(...)
```

for a successful `200 OK` response and:

```csharp
Results.BadRequest(...)
```

for a `400 Bad Request` response.

ASP.NET Core serializes the anonymous objects to JSON.

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

Use the exact HTTP or HTTPS address displayed in the terminal.

Stop the server using:

```text
Ctrl + C
```

## Testing

### Browser

Open:

```text
http://localhost:<port>/
http://localhost:<port>/health
http://localhost:<port>/api/greetings/Ruchi
```

Replace `<port>` with the port displayed by `dotnet run`.

### PowerShell

```powershell
Invoke-RestMethod http://localhost:<port>/health
```

```powershell
Invoke-RestMethod http://localhost:<port>/api/greetings/Ruchi
```

## Expected status codes

| Request | Expected status |
|---|---:|
| `GET /` | `200` |
| `GET /health` | `200` |
| `GET /api/greetings/Ruchi` | `200` |
| Unknown route | `404` |

A route segment normally contains a value, so a request ending at `/api/greetings/` does not match the greeting endpoint and generally produces `404`, rather than entering the handler with an empty name.

## Student practice

1. Add an endpoint that returns trainer information.
2. Add two integer route parameters and return their sum.
3. Read a query-string parameter.
4. Return a different status code for invalid values.
5. Add a POST endpoint accepting a request model.
6. Test every endpoint using an API client.
7. Move endpoint registration into an extension method.

## Trainer discussion prompts

- What services does the application builder prepare?
- What happens when `app.Run()` executes?
- How does route binding work?
- Who serializes the response to JSON?
- What is the difference between `200`, `400` and `404`?
- When is a Minimal API suitable?
- How does this style differ from controller-based APIs?
