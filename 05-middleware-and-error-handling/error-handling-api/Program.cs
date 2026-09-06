using ErrorHandlingApi.Exceptions;
using ErrorHandlingApi.Middleware;
using ErrorHandlingApi.Models;

WebApplicationBuilder builder =
    WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

TrainingSession[] sessions =
[
    new TrainingSession(
        1,
        "C Programming Fundamentals",
        50),

    new TrainingSession(
        2,
        "Pointers and Memory",
        50),

    new TrainingSession(
        3,
        "Data Structures",
        50)
];

app.MapGet(
    "/api/sessions/{id:int}",
    (int id) =>
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Session ID must be positive.",
                nameof(id));
        }

        TrainingSession? session =
            sessions.SingleOrDefault(
                item => item.Id == id);

        if (session is null)
        {
            throw new ResourceNotFoundException(
                $"Training session {id} was not found.");
        }

        return Results.Ok(session);
    });

app.MapGet(
    "/api/demo/unexpected",
    GenerateUnexpectedError);

app.Run();

static IResult GenerateUnexpectedError()
{
    throw new InvalidOperationException(
        "Demonstration of an internal exception.");
}
