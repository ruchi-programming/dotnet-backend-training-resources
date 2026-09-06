using CourseApi.Data;
using CourseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Endpoints;

public static class CourseEndpoints
{
    public static IEndpointRouteBuilder MapCourseEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group =
            endpoints.MapGroup("/api/courses");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/{id:int}", GetByIdAsync);
        group.MapPost("/", CreateAsync);
        group.MapPut("/{id:int}", UpdateAsync);
        group.MapDelete("/{id:int}", DeleteAsync);

        return endpoints;
    }

    private static async Task<IResult> GetAllAsync(
        CourseDbContext database,
        CancellationToken cancellationToken)
    {
        List<Course> courses =
            await database.Courses
                .AsNoTracking()
                .OrderBy(course => course.Id)
                .ToListAsync(cancellationToken);

        return Results.Ok(courses);
    }

    private static async Task<IResult> GetByIdAsync(
        int id,
        CourseDbContext database,
        CancellationToken cancellationToken)
    {
        Course? course =
            await database.Courses
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    item => item.Id == id,
                    cancellationToken);

        return course is null
            ? Results.NotFound(
                new
                {
                    Error = $"Course {id} was not found."
                })
            : Results.Ok(course);
    }

    private static async Task<IResult> CreateAsync(
        CreateCourseRequest request,
        CourseDbContext database,
        CancellationToken cancellationToken)
    {
        string? validationError = Validate(
            request.Title,
            request.DurationHours,
            request.Fee);

        if (validationError is not null)
        {
            return Results.BadRequest(
                new { Error = validationError });
        }

        Course course = new()
        {
            Title = request.Title.Trim(),
            DurationHours = request.DurationHours,
            Fee = request.Fee
        };

        database.Courses.Add(course);
        await database.SaveChangesAsync(cancellationToken);

        return Results.Created(
            $"/api/courses/{course.Id}",
            course);
    }

    private static async Task<IResult> UpdateAsync(
        int id,
        UpdateCourseRequest request,
        CourseDbContext database,
        CancellationToken cancellationToken)
    {
        string? validationError = Validate(
            request.Title,
            request.DurationHours,
            request.Fee);

        if (validationError is not null)
        {
            return Results.BadRequest(
                new { Error = validationError });
        }

        Course? course =
            await database.Courses.FindAsync(
                [id],
                cancellationToken);

        if (course is null)
        {
            return Results.NotFound(
                new
                {
                    Error = $"Course {id} was not found."
                });
        }

        course.Title = request.Title.Trim();
        course.DurationHours = request.DurationHours;
        course.Fee = request.Fee;

        await database.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }

    private static async Task<IResult> DeleteAsync(
        int id,
        CourseDbContext database,
        CancellationToken cancellationToken)
    {
        Course? course =
            await database.Courses.FindAsync(
                [id],
                cancellationToken);

        if (course is null)
        {
            return Results.NotFound(
                new
                {
                    Error = $"Course {id} was not found."
                });
        }

        database.Courses.Remove(course);
        await database.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }

    private static string? Validate(
        string title,
        int durationHours,
        decimal fee)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return "Course title is required.";
        }

        if (title.Trim().Length > 120)
        {
            return "Course title cannot exceed 120 characters.";
        }

        if (durationHours <= 0)
        {
            return "Duration must be greater than zero.";
        }

        if (fee < 0)
        {
            return "Fee cannot be negative.";
        }

        return null;
    }
}
