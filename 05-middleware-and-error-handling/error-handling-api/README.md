# Global Exception-Handling Middleware

## Learning objectives

After completing this example, students will be able to:

- Explain the ASP.NET Core middleware pipeline.
- Create custom middleware.
- Handle exceptions in one central location.
- Map exception types to HTTP status codes.
- Return consistent Problem Details responses.
- Add trace identifiers for diagnostics.
- Prevent internal exception information from reaching clients.

## Request pipeline

```text
HTTP request
    ↓
ExceptionHandlingMiddleware
    ↓
Endpoint
    ↓
HTTP response
```

The middleware calls the next pipeline component inside a `try` block. If a later component throws, the middleware creates an appropriate response.

## Exception mapping

| Exception | HTTP status | Log level |
|---|---:|---|
| `ArgumentException` | `400 Bad Request` | Warning |
| `ResourceNotFoundException` | `404 Not Found` | Warning |
| Unexpected exception | `500 Internal Server Error` | Error |

Known exceptions can return useful client-facing details. Unexpected exceptions return a generic message.

## Problem Details response

An error response contains:

```json
{
  "title": "Resource not found",
  "status": 404,
  "detail": "Training session 99 was not found.",
  "instance": "/api/sessions/99",
  "traceId": "..."
}
```

The trace identifier links the client-visible failure with the corresponding server log entry.

## Security behaviour

For an unexpected exception, the API does not return:

- Stack traces
- Source-file paths
- Database details
- Internal type names
- Sensitive configuration
- The original exception message

Detailed information remains in server-side logs.

## Response-started check

Once response headers or body content have been sent, middleware cannot safely replace the response.

The middleware checks:

```csharp
context.Response.HasStarted
```

and rethrows when replacement is no longer possible.

## Middleware ordering

Exception-handling middleware must appear early enough to wrap components that may fail:

```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

Middleware added before it cannot be handled by it. Middleware and endpoints executed after it are inside its exception-handling boundary.

## Endpoints

| Route | Expected result |
|---|---|
| `/api/sessions/1` | `200 OK` |
| `/api/sessions/99` | `404 Not Found` |
| `/api/sessions/0` | `400 Bad Request` |
| `/api/demo/unexpected` | `500 Internal Server Error` |

Because the route uses the `{id:int}` constraint, a value such as `/api/sessions/abc` does not match the route and normally results in `404`.

## Build and run

From the sample folder:

```bash
dotnet build
dotnet run
```

Use the exact local address shown in the terminal.

## Student practice

1. Add a conflict exception mapped to `409`.
2. Add a correlation-ID middleware.
3. Move exception mapping into a separate service.
4. Add validation-specific error details.
5. Handle request cancellation separately.
6. Write integration tests for all responses.
7. Compare custom middleware with ASP.NET Core’s built-in exception-handling facilities.
8. Add environment-specific diagnostic behaviour without exposing details in production.

## Trainer discussion prompts

- Why centralize exception handling?
- Why should unexpected messages remain private?
- What is the purpose of a trace identifier?
- Why does middleware order matter?
- What happens after a response has started?
- Which errors should be warnings rather than errors?
- When should an endpoint return a result instead of throwing?
